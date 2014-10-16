Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports System.Windows.Forms
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework
Imports System.Text
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesRaster

Public Class FrmExportProfile

    Dim m_aoi As Aoi
    Dim m_profile As Profile
    Dim m_dataTable As Hashtable
    Const IDX_AOI As Integer = 0
    Const IDX_FILE_PATH As Integer = 1
    Const IDX_COPIED As Integer = 2
    Const IDX_AOI_PATH As Integer = 3

    Public Sub New(ByVal copyAoi As Aoi, ByVal copyProfile As Profile)
        InitializeComponent()
        m_aoi = copyAoi
        m_profile = copyProfile
        TxtProfileName.Text = m_profile.Name
    End Sub

    Private Sub BtnAddAoi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAddAoi.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim DataPath As String
        Dim pFilter As IGxObjectFilter = New GxFilterContainers
        Dim bagisPExt As BagisPExtension = BagisPExtension.GetExtension
        Dim rasterStats As IRasterStatistics = Nothing

        Try
            'initialize and open mini browser
            With pGxDialog
                .AllowMultiSelect = False
                .ButtonCaption = "Select"
                .Title = "Select AOI Folder"
                .ObjectFilter = pFilter
                bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
            End With

            If bObjectSelected = False Then Exit Sub

            'get the name of the selected folder
            Dim pGxDataFolder As IGxFile
            pGxDataFolder = pGxObject.Next
            DataPath = pGxDataFolder.Path
            If String.IsNullOrEmpty(DataPath) Then Exit Sub 'user cancelled the action

            'check AOI/BASIN status
            Dim success As BA_ReturnCode = BA_CheckAoiStatus(DataPath, My.ArcMap.Application.hWnd, My.ArcMap.Document)
            If success = BA_ReturnCode.Success Then
                BA_SetDefaultProjection(My.ArcMap.Application)
                Dim aoiName As String = BA_GetBareName(DataPath)
                If AlreadyOnList(aoiName) = False Then
                    Dim pAoi As Aoi = New Aoi(aoiName, DataPath, Nothing, bagisPExt.version)
                    '@ToDo: Check to see if the profile exists in the target AOI
                    Dim profileExists As Boolean = False
                    Dim copyProfile As Boolean = True
                    Dim profilesFolder As String = BA_GetLocalProfilesDir(DataPath)
                    If Not String.IsNullOrEmpty(profilesFolder) Then
                        Dim profileList As List(Of Profile) = BA_LoadProfilesFromXml(profilesFolder)
                        If profileList IsNot Nothing Then
                            For Each nextProfile As Profile In profileList
                                If nextProfile.Name.ToUpper = m_profile.Name.ToUpper Then
                                    profileExists = True
                                    Exit For
                                End If
                            Next
                        End If
                        If profileExists = True Then
                            Dim sb As StringBuilder = New StringBuilder
                            sb.Append("A profile named " & m_profile.Name & " already exists in AOI " & aoiName & "." & vbCrLf)
                            sb.Append("Do you want to overwrite this profile ?" & vbCrLf)
                            Dim result As DialogResult = MessageBox.Show(sb.ToString, "Profile exists", MessageBoxButtons.YesNo, _
                                                                         MessageBoxIcon.Warning)
                            If result = DialogResult.No Then copyProfile = False
                        End If
                    End If
                    If copyProfile = True Then
                        '---create a row---
                        Dim pRow As New DataGridViewRow
                        pRow.CreateCells(GrdAoi)
                        With pRow
                            .Cells(IDX_AOI).Value = aoiName
                            .Cells(IDX_AOI_PATH).Value = DataPath
                            .Cells(IDX_FILE_PATH).Value = DataPath & BA_EnumDescription(PublicPath.BagisParamFolder)
                            .Cells(IDX_COPIED).Value = NO
                        End With
                        '---add the row---
                        GrdAoi.Rows.Add(pRow)
                    End If
                End If
            End If
            Dim sortCol As DataGridViewColumn = GrdAoi.Columns(IDX_AOI)
            GrdAoi.Sort(sortCol, System.ComponentModel.ListSortDirection.Ascending)
            For Each pRow As DataGridViewRow In GrdAoi.SelectedRows
                pRow.Selected = False
            Next
            ManageClipAndRemoveButtons()
        Catch ex As Exception
            MessageBox.Show("BtnSelectAoi_Click Exception: " & ex.Message)
        Finally
            rasterStats = Nothing
        End Try
    End Sub

    Private Function AlreadyOnList(ByVal aoiName) As Boolean
        For Each pRow As DataGridViewRow In GrdAoi.Rows
            Dim nextAoi As String = CStr(pRow.Cells(IDX_AOI).Value)
            If nextAoi = aoiName Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub BtnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnExport.Click
        ' Create/configure a step progressor
        Dim pStepProg As IStepProgressor = Nothing
        Dim progressDialog2 As IProgressDialog2 = Nothing
        Dim rasterStats As IRasterStatistics = Nothing
        BtnExport.Enabled = False
        BtnAddAoi.Enabled = False
        BtnRemove.Enabled = False
        BtnCancel.Enabled = False
        Try
            If GrdAoi.Rows.Count > 0 Then
                pStepProg = BA_GetStepProgressor(My.ArcMap.Application.hWnd, GrdAoi.Rows.Count + 4)
                progressDialog2 = BA_GetProgressDialog(pStepProg, "Exporting profile ", "Exporting...")
                pStepProg.Show()
                progressDialog2.ShowDialog()
                pStepProg.Step()
                Dim idxRow As Integer = 0
                For Each pRow As DataGridViewRow In GrdAoi.Rows
                    Dim aoiName As String = CStr(pRow.Cells(IDX_AOI).Value)
                    Dim aoiPath As String = CStr(pRow.Cells(IDX_AOI_PATH).Value)
                    Dim exported As String = NO
                    progressDialog2.Description = "Exporting " & TxtProfileName.Text & " to AOI " & aoiName
                    pStepProg.Step()
                    'If the settings path exists, we know we have aoipath\param folder
                    Dim settingsPath As String = BA_GetLocalSettingsPath(aoiPath)
                    'Load the data sources once we have the settings path; Will use this in validation
                    m_dataTable = BA_LoadSettingsFile(settingsPath)
                    BA_AppendUnitsToDataSources(m_dataTable, aoiPath)
                    Dim targetMethodsPath As String = BA_GetLocalMethodsDir(aoiPath)
                    Dim targetMethodsList As List(Of Method) = BA_LoadMethodsFromXml(targetMethodsPath)
                    Dim methodNamesList As List(Of String) = New List(Of String)
                    Dim sourceMethodsPath As String = BA_GetLocalMethodsDir(m_aoi.FilePath)
                    Dim isValid As Boolean = True
                    If Not String.IsNullOrEmpty(settingsPath) Then
                        'Save method definitions
                        For Each mName As String In m_profile.MethodNames
                            Dim srcMethod As Method = BA_LoadMethodFromXml(sourceMethodsPath, mName)
                            Dim methodExists As Boolean = False
                            For Each oldMethod As Method In targetMethodsList
                                If oldMethod.Name = srcMethod.Name Then
                                    methodExists = True
                                    Exit For
                                End If
                            Next
                            If Not methodExists Then
                                'Generate a new methodId for the local method
                                Dim methodId As Int16 = GetNextMethodId(targetMethodsList)
                                'Create the local method
                                Dim lclMethod As Method = New Method(methodId, mName, _
                                                                     srcMethod.Description, srcMethod.ToolboxName, _
                                                                     srcMethod.ModelName, srcMethod.ModelLabel, _
                                                                     srcMethod.ToolBoxPath)
                                'copy the parameters
                                If srcMethod.ModelParameters IsNot Nothing Then
                                    Dim lclParamList As List(Of ModelParameter) = New List(Of ModelParameter)
                                    For Each srcParam As ModelParameter In srcMethod.ModelParameters
                                        Dim lclParam As ModelParameter = New ModelParameter(srcParam.Id, _
                                                                                            srcParam.Name, srcParam.Value)
                                        lclParamList.Add(lclParam)
                                    Next
                                    lclMethod.ModelParameters = lclParamList
                                End If
                                'persist the method
                                lclMethod.Save(BA_BagisPXmlPath(targetMethodsPath, lclMethod.Name))
                                methodNamesList.Add(lclMethod.Name)
                            Else
                                methodNamesList.Add(srcMethod.Name)
                            End If
                        Next
                        'Save profile definition
                        Dim targetProfilesPath As String = BA_GetLocalProfilesDir(aoiPath)
                        'Delete old profile if it exists
                        Dim targetProfilesList As IList(Of Profile) = BA_LoadProfilesFromXml(targetProfilesPath)
                        For Each oProfile As Profile In targetProfilesList
                            If oProfile.Name = m_profile.Name Then
                                'Delete xml record
                                BA_Remove_File(BA_BagisPXmlPath(targetProfilesPath, m_profile.Name))
                            End If
                        Next
                        Dim profileId As Int16 = GetNextProfileId(aoiPath)
                        Dim bExt As BagisPExtension = BagisPExtension.GetExtension
                        Dim exportProfile As Profile = New Profile(profileId, m_profile.Name, ProfileClass.BA_Local, m_profile.Description, bExt.version)
                        exportProfile.MethodNames = methodNamesList
                        'persist profile
                        exportProfile.Save(BA_BagisPXmlPath(BA_GetLocalProfilesDir(aoiPath), m_profile.Name))
                        exported = YES
                        'create data bin so it's there if we need it
                        Dim dataBinPath As String = BA_GetDataBinPath(aoiPath)
                        'Update grid
                        pRow.Cells(IDX_COPIED).Value = exported
                        'Validate profile to get error messages for log
                        Dim validatedMethods As List(Of Method) = ValidateProfile(aoiPath, m_profile)
                        'Save xml log of export results so we can convert to html with xsl
                        Dim exportProfileLog As New ExportProfile(aoiName, aoiPath, DateAndTime.Now)
                        exportProfileLog.Profile = exportProfile
                        exportProfileLog.MethodList = validatedMethods
                        Dim paramPath As String = "PleaseReturn"
                        Dim tempName As String = BA_GetBareName(settingsPath, paramPath)
                        Dim xmlOutputPath As String = paramPath & BA_EnumDescription(PublicPath.ExportProfileXml)
                        exportProfileLog.Save(xmlOutputPath)
                        GenerateLog(paramPath)
                    End If
                 Next
                MessageBox.Show(TxtProfileName.Text & " has been exported to the selected AOIs. " & vbCrLf & _
                                "Please review the scenario_report.html in the param folder of each AOI for validation errors that may need to be corrected." & vbCrLf, _
                                "Profile exported", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            Debug.Print("BtnExport_Click Exception: " & ex.Message)
        Finally
            'The step progressor will be undefined if the cancel button was clicked
            If pStepProg IsNot Nothing Then
                pStepProg.Hide()
                pStepProg = Nothing
                progressDialog2.HideDialog()
                progressDialog2 = Nothing
                rasterStats = Nothing
                BtnCancel.Enabled = True
                BtnAddAoi.Enabled = True
            End If
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try

    End Sub

    Private Sub ManageClipAndRemoveButtons()
        If GrdAoi.Rows.Count > 0 Then
            BtnExport.Enabled = True
            BtnRemove.Enabled = True
        Else
            BtnExport.Enabled = False
            BtnRemove.Enabled = False
        End If
    End Sub

    Private Sub UpdateDataSourceStatusOnGrid(ByVal aoiName As String, ByVal clipped As String)
        For Each pRow As DataGridViewRow In GrdAoi.Rows
            'This is the row with the right method
            If pRow.Cells(IDX_AOI).Value = aoiName Then
                'Update the status
                pRow.Cells(IDX_COPIED).Value = clipped
            End If
        Next
    End Sub

    Private Sub GrdAoi_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GrdAoi.SelectionChanged
        ManageClipAndRemoveButtons()
    End Sub

    Private Sub BtnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnRemove.Click
        Dim pCollection As DataGridViewSelectedRowCollection = GrdAoi.SelectedRows

        For Each pRow As DataGridViewRow In pCollection
            GrdAoi.Rows.Remove(pRow)
        Next

        For Each pRow As DataGridViewRow In GrdAoi.SelectedRows
            pRow.Selected = False
        Next
    End Sub

    'Find the highest current id and then add 1 to get the next valid id
    Private Function GetNextMethodId(ByVal mList As List(Of Method)) As Integer
        Dim id As Integer = 0
        For Each nextMethod As Method In mList
            If nextMethod.Id >= id Then
                id = nextMethod.Id
            End If
        Next
        Return id + 1
    End Function

    'Find the highest current id and then add 1 to get the next valid id
    Private Function GetNextProfileId(ByVal aoiPath As String) As Integer
        Dim id As Integer = 0
        Dim profilesFolder As String = BA_GetLocalProfilesDir(aoiPath)
        If Not String.IsNullOrEmpty(profilesFolder) Then
            Dim profileList As List(Of Profile) = BA_LoadProfilesFromXml(profilesFolder)
            For Each oldProfile As Profile In profileList
                If oldProfile.Id >= id Then
                    id = oldProfile.Id
                End If
            Next
        End If
        Return id + 1
    End Function

    'Returns a hashtable of methods with associated validation messages 
    Private Function ValidateProfile(ByVal aoiPath As String, ByVal newProfile As Profile) As List(Of Method)
        Dim mNames As IList(Of String) = newProfile.MethodNames
        Dim mList As New List(Of Method)
        Dim targetMethodsPath As String = BA_GetLocalMethodsDir(aoiPath)
        Dim slopeUnit As SlopeUnit
        Dim elevUnit As MeasurementUnit
        Dim depthUnit As MeasurementUnit
        Dim degreeUnit As MeasurementUnit
        BA_SetMeasurementUnitsForAoi(aoiPath, m_dataTable, slopeUnit, elevUnit, depthUnit, degreeUnit)
        If mNames IsNot Nothing Then
            For Each mName As String In mNames
                Dim pMethod As Method = BA_LoadMethodFromXml(targetMethodsPath, mName)
                Dim errorMessages As IList(Of String) = New List(Of String)
                Dim pModel As ESRI.ArcGIS.Geoprocessing.IGPTool
                If pMethod IsNot Nothing Then
                    'Validate existence of model on disk
                    pModel = BA_OpenModel(pMethod.ToolBoxPath, pMethod.ToolboxName, pMethod.ModelName)
                    If pModel IsNot Nothing Then
                        'Validate local data source presence
                        Dim params As List(Of ModelParameter) = BA_GetModelParameters(pModel)
                        If params IsNot Nothing Then
                            Dim sourceExists As Boolean = True
                            Dim dsName As String = Nothing
                            For Each pParam As ModelParameter In params
                                'Only run function if paramName has "db_" prefix
                                If pParam.Name.Substring(0, BA_DATABIN_PREFIX.Length).ToLower = BA_DATABIN_PREFIX Then
                                    'Get BAGIS-P DS name from method so we can use it in the error message
                                    Dim bagispParams As IList(Of ModelParameter) = pMethod.ModelParameters
                                    Dim bagispName As String = Nothing
                                    For Each bParam As ModelParameter In bagispParams
                                        If bParam.Name = pParam.Name Then
                                            bagispName = bParam.Value
                                            Exit For
                                        End If
                                    Next
                                    Dim pValue As String = BA_CalculateDbParameter(m_aoi.FilePath, m_dataTable, pParam.Name, pMethod)
                                    If Not String.IsNullOrEmpty(pValue) Then
                                        'Get the name of the data source so we can access it from the data source manager
                                        dsName = BA_GetParamValueFromMethod(pMethod, pParam.Name)
                                        Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(pValue)
                                        'First try to open it as a raster
                                        sourceExists = BA_File_Exists(pValue, wType, ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTRasterDataset)
                                        'Follow-up by trying to open it as a vector
                                        If sourceExists = False Then
                                            sourceExists = BA_File_Exists(pValue, wType, ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureClass)
                                        End If
                                        If sourceExists = False Then
                                            errorMessages.Add("Unable to locate associated data source: " & bagispName)
                                        End If
                                    Else
                                        errorMessages.Add("Unable to locate associated data source: " & bagispName)
                                    End If
                                    'Check for unit conflict if units are present with custom DS
                                    If Not String.IsNullOrEmpty(dsName) Then
                                        Dim pDataSource As DataSource = m_dataTable(dsName)
                                        For Each dataParam As ModelParameter In params
                                            Dim modelParam As SystemModelParameterName = BA_GetSystemModelParameterName(dataParam.Name)
                                            Select Case modelParam
                                                Case SystemModelParameterName.sys_units_elevation
                                                    If pDataSource.MeasurementUnitType = MeasurementUnitType.Elevation Then
                                                        If pDataSource.MeasurementUnit <> elevUnit Then
                                                            errorMessages.Add("Elevation units of data source " & dsName & " don't match AOI elevation units")
                                                        End If
                                                    End If
                                                Case SystemModelParameterName.sys_units_slope
                                                    If pDataSource.MeasurementUnitType = MeasurementUnitType.Slope Then
                                                        If pDataSource.SlopeUnit <> slopeUnit Then
                                                            errorMessages.Add("Slope units of data source " & dsName & " don't match AOI elevation units")
                                                        End If
                                                    End If
                                                Case SystemModelParameterName.sys_units_depth
                                                    If pDataSource.MeasurementUnitType = MeasurementUnitType.Depth Then
                                                        If pDataSource.MeasurementUnit <> depthUnit Then
                                                            errorMessages.Add("Depth units of data source " & dsName & " don't match AOI elevation units")
                                                        End If
                                                    End If
                                            End Select
                                        Next
                                    End If
                                End If
                            Next
                        End If
                        'Check for correct slope units, if used by method
                        'Also make sure the temperature units are present if used. This is also a system model parameter
                        'Other units are verified at the AOI level when the form loads
                        If params IsNot Nothing Then
                            For Each pParam As ModelParameter In params
                                Dim modelParam As SystemModelParameterName = BA_GetSystemModelParameterName(pParam.Name)
                                If modelParam = SystemModelParameterName.sys_slope_path AndAlso _
                                   slopeUnit = slopeUnit.Degree Then
                                    errorMessages.Add("Slope layer is in degree and should be in percent")
                                ElseIf modelParam = SystemModelParameterName.sys_units_temperature Then
                                    If degreeUnit = MeasurementUnit.Missing Then
                                        errorMessages.Add("No temperature units have been defined")
                                    End If
                                End If
                            Next
                        End If
                    Else
                        errorMessages.Add("Unable to open model")
                    End If
                Else
                    errorMessages.Add("Unable to locate method definition in AOI")
                End If
                'Add record to table with error messages
                pMethod.ValidationMessages = errorMessages
                mList.Add(pMethod)
            Next
        End If
        Return mList
    End Function

    Private Sub GenerateLog(ByVal xmlInputFolder As String)
        Dim xslTemplate As String = BA_GetAddInDirectory() & BA_EnumDescription(PublicPath.ExportProfileXsl)
        Dim xslFileExists As Boolean = BA_File_ExistsWindowsIO(xslTemplate)
        If xslFileExists Then
            Dim inputFile As String = xmlInputFolder & BA_EnumDescription(PublicPath.ExportProfileXml)
            Dim outputFile As String = xmlInputFolder & BA_EnumDescription(PublicPath.ExportReportHtml)
            Dim success As BA_ReturnCode = BA_XSLTransformToHtml(inputFile, xslTemplate, outputFile)
        End If
    End Sub

End Class