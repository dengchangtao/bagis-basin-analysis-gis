'Imports ESRI.ArcGIS.Geoprocessor
Imports ESRI.ArcGIS.Geoprocessing
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Geodatabase

'1. Subdivide Toolbox into categories sub-toolboxes
'2. Create default models in the toolboxes and zip them up for distribution
'3. Provide place on form for user to tell us where the toolbox is
'4. User manual shows how to define toolbox to local installation of ArcMap

Public Class BtnModel
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()

    End Sub

    Protected Overrides Sub OnClick()
        Try
            'Dim pParamArray As IVariantArray = New VarArray
            'Dim inputPath As String = "C:\Docs\Lesley\miami_aoi_gdb\layers.gdb\cov_den"
            'pParamArray.Add(inputPath)
            'Dim outputPath As String = "C:\Docs\Lesley\miami_aoi_gdb\layers.gdb\out1"
            'pParamArray.Add(outputPath)
            'Dim divisor As Integer = 7
            'pParamArray.Add(divisor)
            'Dim GP As GeoProcessor = New GeoProcessor
            'GP.OverwriteOutput = True
            'GP.AddOutputsToMap = False
            ''Add the custom toolbox containing the model tool 
            'GP.AddToolbox("C:\Documents and Settings\Lesley\Application Data\ESRI\Desktop10.0\ArcToolbox\My Toolboxes\BAGIS-P.tbx")
            'Dim pResult As IGeoProcessorResult = GP.Execute("VegCov-Density", pParamArray, Nothing)

            GetParameters()

        Catch ex As Exception
            MsgBox("Exception: " & ex.Message)
        End Try

    End Sub

    Protected Sub GetParameters()

        Try
            Dim toolboxPath As String = "C:\Documents and Settings\Lesley\Application Data\ESRI\Desktop10.0\ArcToolbox\My Toolboxes"
            Dim wsf As IWorkspaceFactory = New ToolboxWorkspaceFactory
            Dim ws As IToolboxWorkspace = wsf.OpenFromFile(toolboxPath, 0)
            Dim tBox As IGPToolbox = ws.OpenToolbox("BAGIS-P.tbx")
            Dim tool As IGPTool = tBox.OpenTool("VegCov-Density")
            Dim pParamArray As IArray = tool.ParameterInfo
            For j As Int16 = 0 To pParamArray.Count
                Dim gpParam As IGPParameter = pParamArray.Element(j)
                Debug.Print("Parameter name: " & gpParam.Name)
                Debug.Print("Parameter Data Type Display Name: " & gpParam.DataType.DisplayName)
                Debug.Print("Parameter Data Type Name: " + gpParam.DataType.Name)
                Debug.Print("Is Parameter required?: " + gpParam.ParameterType.ToString)
                Debug.Print("Parameter Value Data Type: " + gpParam.Value.DataType.Name)
                Debug.Print("Parameter Value as Text: " + gpParam.Value.GetAsText())
            Next
        Catch ex As Exception

        Finally


        End Try
    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub
End Class
