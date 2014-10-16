Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Framework
Imports System.Windows.Forms
Imports ESRI.ArcGIS.Catalog
Imports System.IO
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.GeoAnalyst
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.DataSourcesFile
Imports BAGIS_ClassLibrary

Public Class FrmPRMS

    Dim m_aoi As Aoi
    Dim m_version As String
    Dim m_lstHruLayersItem As LayerListItem = Nothing

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ' Set AOI if already set
        Dim hruExt As HruExtension = HruExtension.GetExtension
        Dim aoi As Aoi = hruExt.aoi
        If aoi IsNot Nothing Then
            m_aoi = aoi
            TxtAoiPath.Text = m_aoi.FilePath
            ResetFormAfterPRMS()
            LoadHruLayers()
        End If

    End Sub

    Private Sub BtnAOI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAOI.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim DataPath As String
        Dim pFilter As IGxObjectFilter = New GxFilterContainers

        Dim hruExt As HruExtension = HruExtension.GetExtension
        m_version = hruExt.version

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

            '    'check AOI/BASIN status
            Dim success As BA_ReturnCode = BA_CheckAoiStatus(DataPath, My.ArcMap.Application.hWnd, My.ArcMap.Document)
            If success = BA_ReturnCode.Success Then
                BA_SetDefaultProjection(My.ArcMap.Application)
                Dim aoiName As String = BA_GetBareName(DataPath)
                m_aoi = New Aoi(aoiName, DataPath, Nothing, m_version)
                ResetFormAfterPRMS()
                LoadHruLayers()
                Show()
            End If
        Catch ex As Exception
            MessageBox.Show("BtnAOI_Click Exception: " & ex.Message)
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGxDialog)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGxObject)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFilter)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(hruExt)
        End Try
    End Sub

    Public Sub LoadHruLayers(Optional ByVal aoi As Aoi = Nothing)
        If aoi IsNot Nothing Then m_aoi = aoi

        ' Create a DirectoryInfo of the HRU directory.
        Dim zonesDirectory As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, Nothing)
        Dim dirZones As New DirectoryInfo(zonesDirectory)
        TxtAoiPath.Text = m_aoi.FilePath

        If dirZones.Exists Then
            Dim dirZonesArr As DirectoryInfo() = dirZones.GetDirectories
            Dim item As LayerListItem

            ' Create/configure a step progressor
            Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, dirZonesArr.Length)
            ' Create/configure the ProgressDialog. This automatically displays the dialog
            Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Loading HRU zone layers", "Loading...")
            pStepProg.Show()
            progressDialog2.ShowDialog()

            pStepProg.Step()
            LstSelectHruLayers.Items.Clear()

            For Each dri In dirZonesArr
                Dim hruFilePath As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, dri.Name) & BA_EnumDescription(PublicPath.HruGrid)
                Dim hruXmlFilePath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, dri.Name) & BA_EnumDescription(PublicPath.HruXml)

                ' Add hru to the list if the grid exists
                If BA_File_Exists(hruFilePath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) And _
                    BA_File_ExistsWindowsIO(hruXmlFilePath) Then
                    'Assume hru is discrete raster since we create it to be so
                    item = New LayerListItem(dri.Name, hruFilePath, LayerType.Raster, True)
                    LstSelectHruLayers.Items.Add(item)
                End If
                pStepProg.Step()
            Next dri

            pStepProg.Hide()
            pStepProg = Nothing
            progressDialog2.HideDialog()
            progressDialog2 = Nothing
        End If

        If LstSelectHruLayers.Items.Count = 0 Then
            'Pop error message if no zones in this aoi If CboParentHru.Items.Count < 1 Then
            MessageBox.Show("No HRU datasets have been generated for this AOI. Use the 'Define Zones' tool to create an HRU dataset.", "Missing HRU datasets", MessageBoxButtons.OK, MessageBoxIcon.Information)
            'MessageBox.Show("The AOI does not have any HRU layer. The tool can only be applied on HRU layers. Please use the Define HRU tool to create HRU layers first.", "Load HRU Layers")
        End If

        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(dirZones)
    End Sub

    Private Sub Btnclear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btnclear.Click
        LstSelectHruLayers.SelectedIndex = -1
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub ResetFormAfterPRMS()
        LstSelectHruLayers.SelectedIndex = -1
        BtnPRMSStats.Enabled = False
        Btnclear.Enabled = False
    End Sub

    Private Sub LstSelectHruLayers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstSelectHruLayers.SelectedIndexChanged
        ' unselect previous selected item
        If m_lstHruLayersItem IsNot Nothing Then
            LstSelectHruLayers.SelectedItems.Remove(m_lstHruLayersItem)
        End If

        ' reset selected index to new value
        m_lstHruLayersItem = LstSelectHruLayers.SelectedItem

        Btnclear.Enabled = True
        If m_lstHruLayersItem Is Nothing Then
            BtnPRMSStats.Enabled = False
        Else
            BtnPRMSStats.Enabled = True
        End If
    End Sub

    Private Sub BtnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAbout.Click
        Dim toolHelpForm As FrmHelp = New FrmHelp(BA_HelpTopics.PRMSRadiation)
        toolHelpForm.ShowDialog()
    End Sub


    Private Sub BtnGpBug_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnGpBug.Click

        ' Aspect
        Dim tempAspectFileName As String = "TempAspect"
        Dim hruPath As String = "C:\Docs\Lesley\UCO_RioG_SantaFe_R_nr_SantaFe_092010\output"
        Dim layerPath1 As String = hruPath & "\surfaces\dem\filled\aspect"
        Dim fullLayerPath As String = layerPath1 & "\" & BA_EnumDescription(MapsFileName.aspect)
        Dim outputFullPath1 As String = layerPath1 & "\" & tempAspectFileName
        If BA_File_ExistsIDEUtil(outputFullPath1) Then
            BA_Remove_Raster(layerPath1, tempAspectFileName)
        End If
        Dim tempFilePath As String = hruPath & "\zones\hru001\"
        Dim tempFileName As String = "zoneTbl.dbf"

        Dim pNumberReMap As INumberRemap = New NumberRemap
        'pNumberReMap.LoadNumbersFromASCIIFile(m_aoi.FilePath & "\RemapAspect.dat")
        pNumberReMap.MapRange(-1, -1, 0)
        pNumberReMap.MapRange(0, 23.0, 0)
        pNumberReMap.MapRange(23.0, 68.0, 45)
        pNumberReMap.MapRange(68.0, 113.0, 90)
        pNumberReMap.MapRange(113.0, 158.0, 135)
        pNumberReMap.MapRange(158.0, 203.0, 180)
        pNumberReMap.MapRange(203.0, 248.0, 225)
        pNumberReMap.MapRange(248.0, 293.0, 270)
        pNumberReMap.MapRange(293.0, 338.0, 315)
        pNumberReMap.MapRange(338.0, 360.0, 0)

        If BA_ReclassRasterFromRemap(fullLayerPath, outputFullPath1, pNumberReMap) <> BA_ReturnCode.Success Then
            Exit Sub
        End If

        'The geoprocessor tool to use
        Dim tool As ESRI.ArcGIS.SpatialAnalystTools.ZonalStatisticsAsTable = New ESRI.ArcGIS.SpatialAnalystTools.ZonalStatisticsAsTable()
        Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
        Dim pResult As ESRI.ArcGIS.Geoprocessing.IGeoProcessorResult = Nothing
        Dim geoDataSet As IGeoDataset = Nothing
        Dim rasDataset As IRasterDataset = Nothing
        Dim pBandCol As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pSrcTable As ITable = Nothing
        ' Get a temp workspace.
        Dim pWSFactory As IWorkspaceFactory = New ShapefileWorkspaceFactory
        Dim pWorkSpace As IFeatureWorkspace = pWSFactory.OpenFromFile(tempFilePath, 0)
        Dim disposeDataSet As IDataset = Nothing
        'The output table that holds results of zonal calculation
        Dim pOutputTable As ITable = Nothing

        Try
            ' Check for attribute table before running GP tool
            geoDataSet = BA_OpenRasterFromFile(layerPath1, tempAspectFileName)
            rasDataset = CType(geoDataSet, IRasterDataset)
            pBandCol = rasDataset
            pRasterBand = pBandCol.Item(0)
            pSrcTable = pRasterBand.AttributeTable

            If pSrcTable Is Nothing Then
                MsgBox("Missing attribute table")
            Else
                MsgBox("Found attribute table")
            End If

            ' Run GP tool
            tool.in_zone_data = hruPath & "\zones\hru001\grid"
            tool.zone_field = "VALUE"
            tool.in_value_raster = outputFullPath1
            tool.out_table = tempFilePath & tempFileName
            tool.statistics_type = "MAJORITY"
            GP.OverwriteOutput = True
            GP.SetEnvironmentValue("cellsize", "MINOF")
            GP.AddOutputsToMap = False
            pResult = GP.Execute(tool, Nothing)

            ' Delete temporary files
            pOutputTable = pWorkSpace.OpenTable(tempFileName)
            If pOutputTable IsNot Nothing Then
                disposeDataSet = CType(pOutputTable, IDataset)
                disposeDataSet.Delete()
                MsgBox("output table deleted")
            End If
            Dim success As Integer = BA_Remove_Raster(layerPath1, tempAspectFileName)
            If success = 1 Then
                MsgBox(tempAspectFileName & " raster deleted")
            End If
        Catch ex As Exception
            MsgBox("Oops! An error occurred")
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(tool)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pResult)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(GP)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(geoDataSet)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(rasDataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pBandCol)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBand)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pSrcTable)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pOutputTable)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(disposeDataSet)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWorkSpace)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWSFactory)
        End Try
    End Sub

    Private Sub BtnPRMSStats_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnPRMSStats.Click

        ' Create/configure a step progressor
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 7)
        pStepProg.Show()
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Calculating PRMS Radiation Plane", "Calculating...")
        progressDialog2.ShowDialog()

        Try

            'Delete temp files before starting if they already exist
            ' Aspect
            Dim layerPath1 As String = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Surfaces, True)
            Dim aspectInputFullPath As String = layerPath1 & BA_EnumDescription(MapsLayerName.aspect)
            Dim outputPath As String = BA_StandardizePathString(BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, m_lstHruLayersItem.Name), True)
            Dim aspectOutputFullPath As String = outputPath & BA_FIELD_TEMP_ASPECT
            If BA_File_Exists(aspectOutputFullPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                BA_RemoveRasterFromGDB(outputPath, BA_FIELD_TEMP_ASPECT)
            End If

            ' Slope
            Dim layerPath2 As String = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Surfaces, True)
            Dim slopeInputFullPath As String = layerPath2 & BA_EnumDescription(MapsLayerName.slope)
            Dim slopeOutputFullPath As String = outputPath & BA_FIELD_TEMP_SLOPE
            'If BA_File_Exists(outputFullPath2) Then
            If BA_File_Exists(slopeOutputFullPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                BA_RemoveRasterFromGDB(outputPath, BA_FIELD_TEMP_SLOPE)
            End If

            'Aspect remap
            Dim pNumberReMap As INumberRemap = New NumberRemap
            pNumberReMap.MapRange(-1, -1, 0)
            pNumberReMap.MapRange(0, 23.0, 0)
            pNumberReMap.MapRange(23.0, 68.0, 45)
            pNumberReMap.MapRange(68.0, 113.0, 90)
            pNumberReMap.MapRange(113.0, 158.0, 135)
            pNumberReMap.MapRange(158.0, 203.0, 180)
            pNumberReMap.MapRange(203.0, 248.0, 225)
            pNumberReMap.MapRange(248.0, 293.0, 270)
            pNumberReMap.MapRange(293.0, 338.0, 315)
            pNumberReMap.MapRange(338.0, 360.0, 0)

            If BA_ReclassRasterFromRemap(aspectInputFullPath, aspectOutputFullPath, pNumberReMap) <> BA_ReturnCode.Success Then
                Exit Sub
            End If

            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pNumberReMap)
            pStepProg.Step()

            'Slope remap
            pNumberReMap = New NumberRemap
            pNumberReMap.MapRange(0, 10, 5)
            pNumberReMap.MapRange(10, 20, 15)
            pNumberReMap.MapRange(20, 30, 25)
            pNumberReMap.MapRange(30, 40, 35)
            pNumberReMap.MapRange(40, 50, 45)
            pNumberReMap.MapRange(50, 60, 55)
            pNumberReMap.MapRange(60, 70, 65)
            pNumberReMap.MapRange(70, 80, 75)
            pNumberReMap.MapRange(80, 90, 85)
            pNumberReMap.MapRange(91, 999, 95)

            If BA_ReclassRasterFromRemap(slopeInputFullPath, slopeOutputFullPath, pNumberReMap) <> BA_ReturnCode.Success Then
                Exit Sub
            End If
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pNumberReMap)
            pStepProg.Step()

            Dim outputLayerName As String = "SlpAsp"
            Dim layerList As List(Of String) = New List(Of String)
            layerList.Add(aspectOutputFullPath)
            layerList.Add(slopeOutputFullPath)

            Dim snapRasterPath As String = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
            If BA_ZoneOverlay("", layerList, outputPath, outputLayerName, False, True, snapRasterPath, WorkspaceType.Geodatabase) <> BA_ReturnCode.Success Then
                Exit Sub
            End If
            pStepProg.Step()

            Dim rasterPath As String = ""
            Dim rasterName As String = BA_GetBareName(m_lstHruLayersItem.Value, rasterPath)
            Dim vectorName As String = BA_StandardizeShapefileName(BA_GetBareName(BA_EnumDescription(PublicPath.HruVector)), False)
            rasterPath = BA_StandardizePathString(rasterPath, False)
            Dim tempFilePath As String = rasterPath
            Dim tempFileName As String = "zoneTbl"
            pStepProg.Step()

            Dim valueLyr As String = outputPath & outputLayerName
            '@ToDo: This line is causing the bug!
            If BA_ZonalStats2Att(rasterPath, rasterName, tempFilePath, tempFileName, valueLyr, BA_FIELD_PRMS_MAJORITY, snapRasterPath, True, StatisticsTypeString.MAJORITY) <> 1 Then
                Exit Sub
            End If
            pStepProg.Step()
            If BA_ZonalStats2Att(rasterPath, vectorName, tempFilePath, tempFileName, valueLyr, BA_FIELD_PRMS_MAJORITY, snapRasterPath, True, StatisticsTypeString.MAJORITY) <> 1 Then
                Exit Sub
            End If

            ''THIS BLOCK OF CODE IS USED FOR TESTING THE BUG WHERE YOU CAN'T RUN THIS
            ''SUBROUTINE TWICE IN THE SAME ARCMAP SESSION
            ''Dim geoDataSet As IGeoDataset = BA_OpenRasterFromFile(layerPath1, tempAspectFileName)
            ''Dim rasDataset As IRasterDataset = CType(geoDataSet, IRasterDataset)
            ''Dim pBandCol As IRasterBandCollection = rasDataset
            ''Dim pRasterBand As IRasterBand = pBandCol.Item(0)
            ''Dim pSrcTable As ITable = pRasterBand.AttributeTable

            ''If pSrcTable Is Nothing Then
            ''    MsgBox("Missing attribute table")
            ''Else
            ''    MsgBox("Found attribute table")
            ''End If

            ''ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(geoDataSet)
            ''ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(rasDataset)
            ''ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pBandCol)
            ''ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBand)
            ''ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pSrcTable)

            If BA_CopyAttribRasterToRaster(outputPath, outputLayerName, rasterPath, rasterName) <> BA_ReturnCode.Success Then
                MessageBox.Show("Failed to copy fields to " & rasterName)
            End If
            If BA_CopyAttribRasterToRaster(outputPath, outputLayerName, rasterPath, vectorName) <> BA_ReturnCode.Success Then
                MessageBox.Show("Failed to copy fields to " & vectorName)
            End If
            BA_RemoveRasterFromGDB(outputPath, outputLayerName)
            BA_RemoveRasterFromGDB(outputPath, BA_FIELD_TEMP_ASPECT)
            BA_RemoveRasterFromGDB(outputPath, BA_FIELD_TEMP_SLOPE)
            pStepProg.Step()
            MessageBox.Show("PRMS Radiation Plane Calculations Completed ", "PRMS", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MsgBox("BtnPRMSStats_Click: " + ex.Message)
        Finally
            progressDialog2.HideDialog()
            ResetFormAfterPRMS()
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub
End Class