Imports ESRI.ArcGIS.Geoprocessing
Imports ESRI.ArcGIS.GeoprocessingUI
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Display
Imports BAGIS_ClassLibrary
Imports System.Text

Module ToolboxModule

    Public Function BA_OpenModel(ByVal toolboxPath As String, ByVal toolboxName As String, _
                                 ByVal modelName As String) As IGPTool
        Dim wsf As IWorkspaceFactory = New ToolboxWorkspaceFactory
        Dim ws As IToolboxWorkspace = Nothing
        Dim tBox As IGPToolbox = Nothing
        Dim tool As IGPTool = Nothing
        Try
            ws = wsf.OpenFromFile(toolboxPath, 0)
            If ws IsNot Nothing Then
                tBox = ws.OpenToolbox(toolboxName)
                If tBox IsNot Nothing Then
                    tool = tBox.OpenTool(modelName)
                    Return tool
                End If
            End If
            Return Nothing
        Catch ex As Exception
            Debug.Print("BA_OpenModel Exception: " & ex.Message)
            Return Nothing
        Finally
            wsf = Nothing
            ws = Nothing
            tBox = Nothing
            tool = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    Public Function BA_GetModelParameters(ByVal pTool As IGPTool) As List(Of ModelParameter)
        Dim pList As List(Of ModelParameter) = Nothing
        Dim pParamArray As IArray = Nothing
        Dim pDataType As IGPDataType = Nothing
        Try
            pParamArray = pTool.ParameterInfo
            If pParamArray IsNot Nothing AndAlso pParamArray.Count > 0 Then
                pList = New List(Of ModelParameter)
                For j As Int16 = 0 To pParamArray.Count - 1
                    Dim gpParam As IGPParameter = pParamArray.Element(j)
                    Dim mParam As ModelParameter = New ModelParameter(gpParam.Name)
                    Dim strValue As String = gpParam.Value.GetAsText
                    mParam.Value = strValue
                    pList.Add(mParam)
                Next
            End If
            Return pList
        Catch ex As Exception
            Debug.Print("BA_GetModelParameters Exception: " & ex.Message)
            Return Nothing
        Finally
            pParamArray = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    Public Function BA_CheckParamsTable(ByVal hruPath As String, ByVal profileName As String, ByVal overwriteTable As Boolean) As BA_ReturnCode
        'Passing in Nothing for pDataTable parameter since data sources aren't needed here
        Dim tableName As String = BA_GetBareName(BA_CalculateSystemParameter(SystemModelParameterName.sys_param_table.ToString, _
                                                                             hruPath, profileName, Nothing))
        Dim hruParamsGdbPath As String = hruPath & BA_EnumDescription(PublicPath.BagisParamGdb)
        Dim pTable As ITable = Nothing
        Dim pModel As IGPTool = Nothing
        Dim params As List(Of ModelParameter) = Nothing
        Dim pParamArray As IVariantArray = New VarArray
        Dim pArray As IArray = New Array
        'Dim pPropertySet As IPropertySet = New PropertySet
        Try
            pTable = BA_OpenTableFromGDB(hruParamsGdbPath, tableName)
            'Drop table if it exists and we want to overwrite it
            If overwriteTable = True AndAlso pTable IsNot Nothing Then
                Dim success As BA_ReturnCode = BA_Remove_TableFromGDB(hruParamsGdbPath, tableName)
                pTable = BA_OpenTableFromGDB(hruParamsGdbPath, tableName)
            End If
            If pTable Is Nothing Then
                Dim bExt As BagisPExtension = BagisPExtension.GetExtension
                Dim settingsPath As String = bExt.SettingsPath
                Dim toolBoxPrefix As String = BA_GetPublicMethodsPath(settingsPath)
                pModel = BA_OpenModel(toolBoxPrefix, "bagis_method_building_blocks.tbx", "CreateHRUParamsTable")
                If pModel IsNot Nothing Then
                    pArray = pModel.ParameterInfo
                    Dim pParameter As IGPParameter
                    Dim pParamEdit As IGPParameterEdit
                    Dim pDataType As IGPDataType
                    Dim sValue As String

                    For i As Integer = 0 To pArray.Count - 1
                        pParameter = pArray.Element(i)
                        Debug.Print("Parameter Value Data Type: " + pParameter.Value.DataType.Name)
                        Debug.Print("Parameter Value as Text: " + pParameter.Value.GetAsText())
                        pParamEdit = pParameter
                        pDataType = pParameter.DataType
                        If pParameter.Name = SystemModelParameterName.sys_param_table.ToString Then
                            sValue = tableName
                        Else
                            'Passing in Nothing for pDataTable parameter since data sources aren't needed here
                            sValue = BA_CalculateSystemParameter(pParameter.Name, hruPath, profileName, Nothing)
                        End If
                        pParamEdit.Value = pDataType.CreateValue(sValue)
                    Next

                    'http://forums.esri.com/Thread.asp?c=93&f=992&t=170385
                    ''First Parameter
                    'pParameter = pArray.Element(0)
                    'pParamEdit = pParameter
                    'pDataType = pParameter.DataType
                    'sValue = "Profile1_params"
                    'pParamEdit.Value = pDataType.CreateValue(sValue)

                    Return BA_ExecuteTool(pModel, pArray)
                End If
            Else
                Return BA_ReturnCode.Success
            End If
        Catch ex As Exception
            Debug.Print("BA_CheckParamsTable Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            pTable = Nothing
            pModel = Nothing
            pParamArray = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    Public Function BA_CheckDefaultWorkspace(ByVal aoiPath As String) As BA_ReturnCode
        Dim myPath As String = aoiPath & BA_EnumDescription(PublicPath.BagisPDefaultWorkspace)
        Dim success As BA_ReturnCode = BA_ReturnCode.Success
        If Not BA_Folder_ExistsWindowsIO(myPath) Then
            Dim gdbName As String = BA_GetBareName(myPath)
            success = BA_CreateFileGdb(aoiPath, gdbName)
        End If
        Return success
    End Function

    Public Function BA_ExecuteModel(ByVal toolboxPath As String, ByVal modelName As String, _
                                    ByVal pParamArray As IVariantArray, ByVal scratchDir As String, _
                                    ByRef errorMessage As String) As BA_ReturnCode
        Dim GP As GeoProcessor = New GeoProcessor
        Dim pResult As IGeoProcessorResult = Nothing
        Dim enumList As IGpEnumList = Nothing
        Try
            GP.OverwriteOutput = True
            GP.AddOutputsToMap = False
            'Set workspace for geoprocessor; Don't set this for now per JDuh
            'GP.SetEnvironmentValue("workspace", scratchDir)
            'Set scratchworkspace for geoprocessor
            GP.SetEnvironmentValue("scratchworkspace", scratchDir)
            'GP.AddToolbox("C:\Documents and Settings\Lesley\Application Data\ESRI\Desktop10.0\ArcToolbox\My Toolboxes\BAGIS-P.tbx")
            GP.AddToolbox(toolboxPath)
            'Return GP.Execute("VegCov-Density", pParamArray, Nothing)
            Dim gpMsg As IGPMessages = Nothing
            gpMsg = GP.Validate(modelName, pParamArray, True)
            For Counter As Integer = 0 To gpMsg.Count - 1
                Debug.Print("msg: " & gpMsg.GetMessage(Counter).Description)
            Next

            pResult = GP.Execute(modelName, pParamArray, Nothing)
            Dim idxLastMsg = pResult.MessageCount - 1
            For Counter As Integer = 0 To idxLastMsg
                Dim nextMsg As String = pResult.GetMessage(Counter)
                Debug.Print("GP: " & nextMsg)
            Next
            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_ExecuteModel Exception: " & ex.Message)
            Dim sb As StringBuilder = New StringBuilder
            Dim idxLastMsg = GP.MessageCount - 1
            For Counter As Integer = 0 To idxLastMsg
                Dim nextMsg As String = GP.GetMessage(Counter)
                Debug.Print("GP Error: " & nextMsg)
                If Not String.IsNullOrEmpty(nextMsg) AndAlso nextMsg.IndexOf("ERROR ") > -1 Then
                    sb.AppendLine(nextMsg)
                End If
            Next
            errorMessage = sb.ToString
            Return BA_ReturnCode.UnknownError
        Finally
            'Reset GP environment; This fixes an HRESULT error I was receiving if I ran
            'two Soils tools in a row; Also a problem with certain types of geoprocessor errors affecting subsequent methods
            GP.ResetEnvironments()
            GP = Nothing
            pResult = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Function

    ' This function presents the tool dialog box so it can be run interactively
    ' Not what we need for BAGIS-P but good to know how to do it.
    Public Function BA_ExecuteModelWithToolCommandHelper(ByVal pTool As IGPTool, ByVal pParamArray As IArray) As BA_ReturnCode
        Dim pCommand As IGPToolCommandHelper = New GPToolCommandHelper
        'Sample of parameter input
        'Dim pArray As IArray = New Array
        'pArray.Add(pValue)
        Try
            pCommand.SetTool(pTool)
            pCommand.Invoke(pParamArray)
            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_ExecuteModel2 Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            pCommand = Nothing
            pTool = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try

    End Function

    Public Function BA_ExecuteTool(ByVal pModel As IGPTool, ByVal paramValues As IArray) As BA_ReturnCode
        Dim pGPEnvironmentManager As IGPEnvironmentManager
        Dim pGPmessages As IGPMessages = New GPMessages
        Dim mcancel As ITrackCancel = New CancelTracker

        Try
            pGPEnvironmentManager = New GPEnvironmentManager
            pModel.Execute(paramValues, mcancel, pGPEnvironmentManager, pGPmessages)
            'For Counter As Integer = 0 To pGPmessages.Count
            '    Debug.Print("GP Msg: " & pGPmessages.GetMessage(Counter).Description)
            'Next
            Return BA_ReturnCode.Success
        Catch ex As Exception
            Dim lastLine As String = Nothing
            If pGPmessages IsNot Nothing Then
                For counter As Integer = 0 To pGPmessages.Count
                    lastLine = pGPmessages.GetMessage(counter).Description
                Next
            End If
            If lastLine IsNot Nothing AndAlso lastLine.IndexOf("Succeeded") > -1 Then
                Return BA_ReturnCode.Success
            Else
                Debug.Print("BA_ExecuteModel Exception: " & ex.Message)
                Return BA_ReturnCode.UnknownError
            End If
        Finally
            pModel = Nothing
            pGPEnvironmentManager = Nothing
            pGPmessages = Nothing
            mcancel = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Function

    Public Function BA_RunModelFromMethod(ByVal inMethod As Method, ByVal hruPath As String, _
                                          ByVal profileName As String, ByVal scratchDir As String, _
                                          ByRef errorMessage As String) As BA_ReturnCode
        Dim pModel As IGPTool = Nothing
        Dim params As List(Of ModelParameter) = Nothing
        Dim pParamArray As IVariantArray = New VarArray
        Try
            pModel = BA_OpenModel(inMethod.ToolBoxPath, inMethod.ToolboxName, inMethod.ModelName)
            If pModel IsNot Nothing Then
                params = BA_GetModelParameters(pModel)
                If params IsNot Nothing Then
                    'Extract aoiPath
                    Dim aoiPath As String = Nothing
                    Dim pos As Integer = hruPath.IndexOf(BA_EnumDescription(PublicPath.HruDirectory))
                    If pos > -1 Then
                        aoiPath = hruPath.Substring(0, pos)
                    End If
                    'Check to see if any of the parameters are units parameters or db parameters
                    'If so, load the data sources and their units. We do it here so we only
                    'have to do it once if there are multiple units
                    Dim pDataTable As Hashtable = New Hashtable
                    For Each pParam As ModelParameter In params
                        If pParam.Name.Substring(0, BA_DATABIN_PREFIX.Length).ToLower = BA_DATABIN_PREFIX Or
                            pParam.Name.ToLower = SystemModelParameterName.sys_units_elevation.ToString.ToLower Or
                            pParam.Name.ToLower = SystemModelParameterName.sys_units_slope.ToString.ToLower Or
                            pParam.Name.ToLower = SystemModelParameterName.sys_units_depth.ToString.ToLower Or
                            pParam.Name.ToLower = SystemModelParameterName.sys_units_temperature.ToString.ToLower Then
                            pDataTable = BA_LoadSettingsFile(BA_GetLocalSettingsPath(aoiPath))
                            BA_AppendUnitsToDataSources(pDataTable, aoiPath)
                            Exit For
                        End If
                    Next
                    For Each pParam As ModelParameter In params
                        Dim pValue As String = BA_CalculateSystemParameter(pParam.Name, hruPath, profileName, pDataTable)
                        If pValue Is Nothing Then
                            'The parameter was not a system parameter
                            pValue = BA_CalculateDbParameter(aoiPath, pDataTable, pParam.Name, inMethod)
                            If pValue = Nothing Then
                                'The parameter was not a databin parameter either; Pass value through
                                pValue = pParam.Value
                            End If
                        End If
                        Debug.Print(pParam.Name & "--->" & pValue)
                        pParamArray.Add(pValue)
                    Next
                End If
                Return BA_ExecuteModel(pModel.Toolbox.PathName, pModel.Name, pParamArray, scratchDir, errorMessage)
            End If
        Catch ex As Exception
            Debug.Print("BA_RunModelFromMethod Exception: " & ex.Message)
        Finally
            pModel = Nothing
            pParamArray = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Function

    Public Function BA_RunModelFromMethodFilledParameters(ByVal inMethod As Method, ByVal scratchDir As String, _
                                                          ByRef errorMessage As String) As BA_ReturnCode
        Dim pModel As IGPTool = Nothing
        Dim params As List(Of ModelParameter) = Nothing
        Dim pParamArray As IVariantArray = New VarArray
        Try
            pModel = BA_OpenModel(inMethod.ToolBoxPath, inMethod.ToolboxName, inMethod.ModelName)
            If pModel IsNot Nothing And inMethod.FilledModelParameters IsNot Nothing Then
                params = inMethod.FilledModelParameters
                For Each pParam As ModelParameter In params
                    pParamArray.Add(pParam.Value)
                Next
            End If
            Return BA_ExecuteModel(pModel.Toolbox.PathName, pModel.Name, pParamArray, scratchDir, errorMessage)
        Catch ex As Exception
            Debug.Print("BA_RunModelFromMethod Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            pModel = Nothing
            pParamArray = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Function

    Public Function BA_OpenToolbox(ByVal toolboxPath As String, ByVal toolboxName As String) As IGPToolbox
        Dim wsf As IWorkspaceFactory = New ToolboxWorkspaceFactory
        Dim ws As IToolboxWorkspace = Nothing
        Dim tBox As IGPToolbox = Nothing
        Try
            ws = wsf.OpenFromFile(toolboxPath, 0)
            If ws IsNot Nothing Then
                tBox = ws.OpenToolbox(toolboxName)
            End If
            Return tBox
        Catch ex As Exception
            Debug.Print("BA_OpenToolbox Exception: " & ex.Message)
            Return Nothing
        Finally
            wsf = Nothing
            ws = Nothing
            tBox = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    Public Function BA_ModelExists(ByVal toolboxPath As String, ByVal toolboxName As String, _
                                   ByVal modelName As String) As Boolean
        Dim tool As IGPTool = Nothing
        Try
            tool = BA_OpenModel(toolboxPath, toolboxName, modelName)
            If tool IsNot Nothing Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Debug.Print("BA_ModelExists Exception: " & ex.Message)
            Return False
        Finally
            tool = Nothing
        End Try

    End Function

End Module
