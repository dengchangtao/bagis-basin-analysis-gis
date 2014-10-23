Imports System.Windows.Forms
Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.Desktop.AddIns
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesFile
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework
Imports System.ComponentModel
Imports ESRI.ArcGIS.DataSourcesGDB
Imports ESRI.ArcGIS.DataSourcesRaster
Imports System.IO

Public Class frmAOIInfo
    Private AOIRasterList() As String
    Private AOIVectorList() As String
    Dim m_aoi As Aoi
    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Me.Close()
    End Sub

    Private Sub CmdClearSelected_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClearSelected.Click
        LstRasters.SelectedIndex = -1
        LstVectors.SelectedIndex = -1
    End Sub
    Dim m_version As String

    Private Sub CmdSetAOI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSetAOI.Click
        Dim bObjectSelected As Boolean = True
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim tempAOIFolderBase As String = ""
        Dim response As Integer

        If BA_Enable_SAExtension(My.ArcMap.Application) <> ESRI.ArcGIS.esriSystem.esriExtensionState.esriESEnabled Then
            Windows.Forms.MessageBox.Show("Spatial Analyst is required for BAGIS and is not available. Program stopped.")
            Exit Sub
        End If

        Dim pFilter As IGxObjectFilter = New GxFilterContainers

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select AOI Folder"
            .ObjectFilter = pFilter
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then
            Exit Sub
        End If

        'get the name of the selected folder
        Dim pGxDataFolder As IGxFile = pGxObject.Next
        tempAOIFolderBase = pGxDataFolder.Path
        pGxDialog = Nothing
        pGxDataFolder = Nothing

        If String.IsNullOrEmpty(tempAOIFolderBase) Then Exit Sub 'user cancelled the action

        'check if the selected folder is a valid AOI
        Dim success As BA_ReturnCode = BA_CheckAoiStatus(tempAOIFolderBase, My.ArcMap.Application.hWnd, My.ArcMap.Document)
        If success <> BA_ReturnCode.Success Then 'the folder is not an AOI or has missing layers
            Exit Sub
        End If

        BA_SetDefaultProjection(My.ArcMap.Application)

        'set mapframe name to default name
        response = BA_SetDefaultMapFrameName(BA_DefaultMapName)
        Dim response1 As Integer = BA_SetMapFrameDimension(BA_DefaultMapName, 1, 2, 7.5, 9, True)

        'global variables: pSelectColor and pDisplayColor
        pSelectColor = New RgbColor
        'pSelectColor determines the color for AOI and Pourpoint graphics
        pSelectColor.RGB = RGB(0, 255, 0)

        'pDisplayColor determines the color for AOI and Pourpoint shapfiles
        pDisplayColor = New RgbColor
        pDisplayColor.RGB = RGB(255, 0, 0)

        FrameBAGISLayers.Enabled = True
        CmdAddLayer.Enabled = True
        CmdUpdateWeasel.Enabled = True
        FrameUserLayers.Enabled = True

        'check Snotel, snow course, and PRISM data and determine the value for BA_SystemSettings.GenerateAOIOnly value
        'reset their selected checkboxes
        ChkPRISMSelected.Checked = False
        ChkSNOTELSelected.Checked = False
        ChkSnowCourseSelected.Checked = False
        CmdReClip.Enabled = False

        BA_SystemSettings.GenerateAOIOnly = False

        'PRISM
        Dim temppathname As String = tempAOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Prism) & "\Q4"
        If BA_File_Exists(temppathname, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            ChkPRISMExist.Checked = True
        Else
            ChkPRISMExist.Checked = False
            BA_SystemSettings.GenerateAOIOnly = True
        End If

        'SNOTEL
        temppathname = tempAOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Layers) & "\" & BA_SNOTELSites
        If BA_File_Exists(temppathname, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            ChkSNOTELExist.Checked = True
        Else
            ChkSNOTELExist.Checked = False
            'BA_SystemSettings.GenerateAOIOnly = True 'some AOIs that don't have SNOTEL sites do not have SNOTEL layer
        End If

        'Snow Courses
        temppathname = tempAOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Layers) & "\" & BA_SnowCourseSites
        If BA_File_Exists(temppathname, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            ChkSnowCourseExist.Checked = True
        Else
            ChkSnowCourseExist.Checked = False
            'BA_SystemSettings.GenerateAOIOnly = True  'some AOIs that don't have snow course sites do not have snow course layer
        End If

        'BA_SetAOI activates/deactivates the Analysis and Maps menu items based on the presence of SNOTEL, Snow Course, and PRISM data in the AOI.
        'It overwrites the Options setttings.
        BA_SetAOI(tempAOIFolderBase)
        AOIFolderBase = tempAOIFolderBase

        'update caption
        Me.Text = "AOI: " & Me.Tag

        If String.IsNullOrEmpty(AOIFolderBase) Then
            'MsgBox "Please set an AOI first!"
            FrameBAGISLayers.Enabled = False
            CmdAddLayer.Enabled = False
            FrameUserLayers.Enabled = False
            'Exit Sub
        End If

        'display folder paths
        txtDEMFolder.Text = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces)
        txtPRISMFolder.Text = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Prism)
        txtLayersFolder.Text = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Layers)

        'display aoi area
        BA_ReadPPAttributes(AOIFolderBase, AOI_ShapeArea, AOI_ShapeUnit, AOI_ReferenceArea, AOI_ReferenceUnit)

        If AOI_ShapeArea <= 0 Then
            MsgBox("Unable to read the pour point layer of the AOI!" & vbCrLf & "AOI folder: " & AOIFolderBase & _
            vbCrLf & "Featureclass: " & BA_POURPOINTCoverage)
            Exit Sub
            Me.Close()
        End If

        txtArea.Text = Format(AOI_ShapeArea, "#0.00")
        txtAreaAcre.Text = Format(AOI_ShapeArea * 247.1044, "#0.00")
        txtAreaSQMile.Text = Format(AOI_ShapeArea * 0.3861022, "#0.00")

        txtRefArea.Text = Format(AOI_ReferenceArea, "#0.00")
        lblRefUnit.Text = AOI_ReferenceUnit

        'Get DEM statistics
        Dim pRasterStats As IRasterStatistics = Nothing
        Dim fullfilepath As String = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\" & BA_EnumDescription(MapsFileName.filled_dem_gdb)
        pRasterStats = BA_GetDemStatsGDB(AOIFolderBase)

        If pRasterStats IsNot Nothing Then
            txtMinElev.Text = Math.Round(pRasterStats.Minimum - 0.005, 2)
            txtMaxElev.Text = Math.Round(pRasterStats.Maximum + 0.005, 2)
            txtRangeElev.Text = Math.Round((pRasterStats.Maximum - pRasterStats.Minimum) + 0.005, 2)

            pRasterStats = Nothing
        Else
            MsgBox("Unable to read the filled DEM! The AOI surface.gdb is corrupted.")
        End If

        Dim aoiName As String = BA_GetBareName(tempAOIFolderBase)
        m_aoi = New Aoi(aoiName, tempAOIFolderBase, Nothing, m_version)
        'TxtAoiPath.Text = m_aoi.FilePath
        LoadLstLayers()

        MsgBox(aoiName & " is set as the current AOI!", MsgBoxStyle.Information)
        ''set the value of cbotargeted basin to the name of the aoi since an aoi should also be a basin
        'Dim cboselectbasin = AddIn.FromID(Of cboTargetedBasin)(My.ThisAddIn.IDs.cboTargetedBasin)
        'cboselectbasin.setValue(BA_GetBareName(tempAOIFolderBase))
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        Call CoFreeUnusedLibraries()
    End Sub

    Private Sub frmAOIInfo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If String.IsNullOrEmpty(AOIFolderBase) Then
            'MsgBox "Please set an AOI first!"
            FrameBAGISLayers.Enabled = False
            CmdAddLayer.Enabled = False
            CmdUpdateWeasel.Enabled = False
            FrameUserLayers.Enabled = False
            Exit Sub
        End If

        'global variables: pSelectColor and pDisplayColor
        pSelectColor = New RgbColor
        'pSelectColor determines the color for AOI and Pourpoint graphics
        pSelectColor.RGB = RGB(0, 255, 0)

        'pDisplayColor determines the color for AOI and Pourpoint shapfiles
        pDisplayColor = New RgbColor
        pDisplayColor.RGB = RGB(255, 0, 0)

        FrameBAGISLayers.Enabled = True
        CmdAddLayer.Enabled = True
        CmdUpdateWeasel.Enabled = True
        FrameUserLayers.Enabled = True

        'update caption
        Me.Text = "AOI: " & Me.Tag

        'display folder paths
        txtDEMFolder.Text = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces)
        txtPRISMFolder.Text = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Prism)
        txtLayersFolder.Text = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Layers)

        'display aoi area
        BA_ReadPPAttributes(AOIFolderBase, AOI_ShapeArea, AOI_ShapeUnit, AOI_ReferenceArea, AOI_ReferenceUnit)

        If AOI_ShapeArea <= 0 Then
            MsgBox("Unable to read the pour point layer of the AOI!" & vbCrLf & "AOI folder: " & AOIFolderBase & _
            vbCrLf & "Featureclass: " & BA_POURPOINTCoverage)
            Exit Sub
            Me.Close()
        End If

        txtArea.Text = Format(AOI_ShapeArea, "#0.00")
        txtAreaAcre.Text = Format(AOI_ShapeArea * 247.1044, "#0.00")
        txtAreaSQMile.Text = Format(AOI_ShapeArea * 0.3861022, "#0.00")

        txtRefArea.Text = Format(AOI_ReferenceArea, "#0.00")
        lblRefUnit.Text = AOI_ReferenceUnit

        'display dem elevation stats
        ' ''Dim pRasterStatistics As IRasterStatistics = Nothing
        ' ''Dim success As BA_ReturnCode = BA_ReturnCode.UnknownError
        ' ''Try
        ' ''    Dim rasterResolutionPath As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
        ' ''    'Get cell size from DEM grid to resample PRISM data
        ' ''    Dim cellSize As Double = 0
        ' ''    pRasterStatistics = BA_GetRasterStatsGDB(rasterResolutionPath, cellSize)

        'Get DEM statistics
        Dim pRasterStats As IRasterStatistics = Nothing
        Dim fullfilepath As String = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\" & BA_EnumDescription(MapsFileName.filled_dem_gdb)
        pRasterStats = BA_GetDemStatsGDB(AOIFolderBase)

        txtMinElev.Text = Math.Round(pRasterStats.Minimum - 0.005, 2)
        txtMaxElev.Text = Math.Round(pRasterStats.Maximum + 0.005, 2)
        txtRangeElev.Text = Math.Round((pRasterStats.Maximum - pRasterStats.Minimum) + 0.005, 2)

        pRasterStats = Nothing

        'check Snotel, snow course, and PRISM data
        'reset their selected checkboxes
        ChkPRISMSelected.Checked = False
        ChkSNOTELSelected.Checked = False
        ChkSnowCourseSelected.Checked = False
        CmdReClip.Enabled = False

        'PRISM
        Dim temppathname As String = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Prism) & "\Q4"
        If Not BA_File_Exists(temppathname, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            ChkPRISMExist.Checked = False
        Else
            ChkPRISMExist.Checked = True
        End If

        'SNOTEL
        temppathname = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Layers) & "\" & BA_SNOTELSites
        If Not BA_File_Exists(temppathname, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            ChkSNOTELExist.Checked = False
        Else
            ChkSNOTELExist.Checked = True
        End If

        'Snow Courses
        temppathname = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Layers) & "\" & BA_SnowCourseSites
        If BA_File_Exists(temppathname, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            ChkSnowCourseExist.Checked = True
        Else
            ChkSnowCourseExist.Checked = False
        End If

        Dim aoiName As String = BA_GetBareName(AOIFolderBase)
        m_aoi = New Aoi(aoiName, AOIFolderBase, Nothing, m_version)
        LoadLstLayers()
    End Sub

    Private Sub CmdReClip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdReClip.Click
        Me.Hide()

        'read the basinanalyst.def file to get the path/name of the required input layers
        'the value should be available in BA_SystemSettings

        Dim response As Short
        Dim j As Integer
        Dim DataName As String

        'remove all layers of the AOI from the data frame
        BA_SetSettingPath()
        BA_ReadBAGISSettings(BA_Settings_Filepath)
        response = BA_RemoveLayersInFolder(My.ArcMap.Document, AOIFolderBase)
        System.Windows.Forms.Application.DoEvents()

        Dim nstep As Integer = 0
        'delete the files/folders that need to be re-created
        Dim prismgdbpath As String = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Prism)
        If ChkPRISMExist.Checked = True And ChkPRISMSelected.Checked = True Then 'delete PRISM Folder
            'response = BA_Remove_Folder(BA_GetPath(AOIFolderBase, GeodatabaseNames.Prism))
            If BA_Workspace_Exists(prismgdbpath) Then
                Dim gdbSuccess As BA_ReturnCode = BA_DeleteGeodatabase(prismgdbpath, My.ArcMap.Document)
                If gdbSuccess <> BA_ReturnCode.Success Then
                    MessageBox.Show("Unable to delete Geodatabase '" & prismgdbpath & "'. Please restart ArcMap and try again", "Unable to delete Geodatabase", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Exit Sub
                End If
                nstep = nstep + 17
            End If
        End If

        If ChkSNOTELExist.Checked = True And ChkSNOTELSelected.Checked = True Then 'delete SNOTEL file
            response = BA_Remove_ShapefileFromGDB(AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Layers), BA_SNOTELSites)
            ChkSNOTELExist.Checked = False
            nstep = nstep + 1
        End If

        If ChkSnowCourseExist.Checked = True And ChkSnowCourseSelected.Checked = True Then 'delete Snow Course file
            response = BA_Remove_ShapefileFromGDB(AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Layers), BA_SnowCourseSites)
            ChkSnowCourseExist.Checked = False
            nstep = nstep + 1
        End If

        ' Create/configure a step progressor
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, nstep)
        Dim progressDialog2 As IProgressDialog2 = Nothing
        progressDialog2 = BA_GetProgressDialog(pStepProg, "Clipping selected layers ", "Clipping...")
        pStepProg.Show()
        progressDialog2.ShowDialog()
        System.Windows.Forms.Application.DoEvents()

        'regenerate the files/folders
        If ChkPRISMSelected.Checked = True Then 'Clip PRISM data
            'set the PRISM folder variables: PRISMLayer()
            'BA_SetPRISMFolderNames()

            'create the PRISM Geodatabase
            Dim gdbName As String = BA_EnumDescription(GeodatabaseNames.Prism)
            Dim success As BA_ReturnCode = BA_CreateFileGdb(AOIFolderBase, gdbName)

            'Dim rasterNamesTable As Hashtable = New Hashtable
            'BA_UpdateHashtableForPrism(AOIFolderBase, rasterNamesTable)

            Dim InPRISMPath As String = BA_SystemSettings.PRISMFolder

            'PRISM
            If String.IsNullOrEmpty(Trim(InPRISMPath)) Then
                MsgBox("PRISM data source is not defined! Please use the Options dialog to define the data source.")
            Else
                Dim temppathname As String = InPRISMPath & "\Q4\grid"
                If BA_Workspace_Exists(temppathname) Then
                    BA_SetPRISMFolderNames()
                    'there are 17 prism rasters to be clipped
                    For j = 0 To 16
                        DataName = PRISMLayer(j)
                        pStepProg.Step()
                        System.Windows.Forms.Application.DoEvents()

                        'input PRISM raster is in GRID format, output is in FGDB format 
                        Dim outputFolder As String = m_aoi.FilePath & "\" & BA_EnumDescription(GeodatabaseNames.Prism)
                        response = BA_ClipAOIRaster(m_aoi.FilePath, InPRISMPath & "\" & DataName & "\grid", DataName, outputFolder, AOIClipFile.PrismClipAOIExtentCoverage)
                        If response <= 0 Then
                            MsgBox("Clipping " & DataName & " failed! Return value = " & response & ".")
                            Exit Sub 'I added this part to avoid getting bunch of exceprion messages related to clipping prism layers
                        End If
                    Next
                    Me.ChkPRISMExist.Checked = True
                    'response = BA_RemoveLayers(My.Document, "grid")
                Else
                    MsgBox("The specified PRISM data source is missing! Please verify the data source information in the Options dialog.")
                End If
            End If
        End If

        Dim InLayerString As String
        Dim LayerPath As String = ""
        Dim LayerName As String, strExtension As String = ""

        'SNOTEL
        If ChkSNOTELSelected.Checked = True Then 'clip SNOTEL data
            InLayerString = BA_SystemSettings.SNOTELLayer
            If String.IsNullOrEmpty(Trim(InLayerString)) Then
                MsgBox("SNOTEL data source is not defined! Please use the Options dialog to define the data source.")
            Else
                LayerName = BA_GetBareNameAndExtension(InLayerString, LayerPath, strExtension)
                If BA_Shapefile_Exists(LayerPath & LayerName) Then
                    response = BA_ClipAOISNOTEL(AOIFolderBase, LayerPath & LayerName, True)
                    If response <> 1 Then
                        Select Case response
                            Case -1 '-1: unknown error
                                MsgBox("Unknown error occurred when clipping data to AOI!")
                            Case -2 '-2: output exists
                                MsgBox("Output target layer exists in the AOI. Unable to clip new data to AOI!")
                            Case -3 '-3: missing parameters
                                MsgBox("Missing clipping parameters. Unable to clip new data to AOI!")
                            Case -4 '-4: no input shapefile
                                MsgBox("Missing the clipping shapefile. Unable to clip new data to AOI!")
                            Case 0 '0: no intersect between the input and the clip layers
                                MsgBox("No SNOTEL data exists within the AOI. Unable to clip new SNOTEL data to AOI!")
                        End Select
                    Else
                        Me.ChkSNOTELExist.Checked = True
                    End If
                    pStepProg.Step()
                    System.Windows.Forms.Application.DoEvents()
                Else
                    MsgBox("The specified SNOTEL data source is missing! Please verify the data source information in the Options dialog.")
                End If
            End If
        End If

        If ChkSnowCourseSelected.Checked = True Then 'clip Snow Course data
            InLayerString = BA_SystemSettings.SCourseLayer

            If String.IsNullOrEmpty(Trim(InLayerString)) Then
                MsgBox("Snow Course data source is not defined! Please use the Options dialog to define the data source.")
            Else
                LayerName = BA_GetBareNameAndExtension(InLayerString, LayerPath, strExtension)
                If BA_Shapefile_Exists(LayerPath & LayerName) Then
                    response = BA_ClipAOISNOTEL(AOIFolderBase, LayerPath & LayerName, False)
                    If response <> 1 Then
                        Select Case response
                            Case -1 '-1: unknown error
                                MsgBox("Unknown error occurred when clipping data to AOI!")
                            Case -2 '-2: output exists
                                MsgBox("Output target layer exists in the AOI. Unable to clip new data to AOI!")
                            Case -3 '-3: missing parameters
                                MsgBox("Missing clipping parameters. Unable to clip new data to AOI!")
                            Case -4 '-4: no input shapefile
                                MsgBox("Missing the clipping shapefile. Unable to clip new data to AOI!")
                            Case 0 '0: no intersect between the input and the clip layers
                                MsgBox("No Snow Course data exists within the AOI. Unable to clip new Snow Course data to AOI!")
                        End Select
                    Else
                        Me.ChkSnowCourseExist.Checked = True
                    End If
                    pStepProg.Step()
                    System.Windows.Forms.Application.DoEvents()
                Else
                    MsgBox("The specified Snow Course data source is missing! Please verify the data source information in the Options dialog.")
                End If
            End If
        End If

        pStepProg.Hide()
        pStepProg = Nothing
        progressDialog2.HideDialog()
        progressDialog2 = Nothing

        'unload.frmMessage()
        MsgBox("Re-clipping layer(s) to the AOI completed!")
        ChkSnowCourseSelected.Checked = False
        ChkSNOTELSelected.Checked = False
        ChkPRISMSelected.Checked = False
        Me.Show()
    End Sub

    Private Sub LstRasters_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstRasters.SelectedIndexChanged
        ManageAoiLayerButtons()
    End Sub

    Private Sub LstVectors_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstVectors.SelectedIndexChanged
        ManageAoiLayerButtons()
    End Sub

    Private Sub ManageAoiLayerButtons()
        Dim lCount As Integer = LstRasters.SelectedItems.Count + _
                                    LstVectors.SelectedItems.Count

        If lCount > 0 Then
            CmdClearSelected.Enabled = True
            CmbAddSelectionsToMap.Enabled = True
            'Dim m_lstHruLayersItem As LayerListItem = Nothing
            'If m_lstHruLayersItem IsNot Nothing Then
            '    Panel1.Enabled = True
            'End If
        Else
            CmdClearSelected.Enabled = False
            CmbAddSelectionsToMap.Enabled = False
            'Panel1.Enabled = False
        End If
    End Sub

    Private Sub CmbAddSelectionsToMap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbAddSelectionsToMap.Click
        Try
            Dim fileNamesWithStyle As List(Of String) = BA_ListOfLayerNamesWithStyles()
            ' Display raster layers
            If LstRasters.SelectedIndex > -1 Then
                Dim items As IList = LstRasters.SelectedItems
                For Each item As LayerListItem In items
                    If fileNamesWithStyle.IndexOf(item.Name) > -1 Then
                        Dim symbology As BA_Map_Symbology = BA_GetRasterMapSymbology(item.Name)
                        BA_DisplayRasterWithSymbol(My.ArcMap.Document, item.Value, symbology.DisplayName, _
                                                   symbology.DisplayStyle, symbology.Transparency, WorkspaceType.Geodatabase)
                    Else
                        BA_DisplayRaster(My.ArcMap.Application, item.Value)
                    End If
                Next
            End If
            ' Display vector layers
            If LstVectors.SelectedIndex > -1 Then
                Dim items As IList = LstVectors.SelectedItems
                For Each item As LayerListItem In items
                    Dim strFileName As String = BA_GetBareName(item.Value)
                    If fileNamesWithStyle.IndexOf(strFileName) > -1 Then
                        Dim symbology As BA_Map_Symbology = BA_GetPointMapSymbology(strFileName)
                        BA_MapDisplayPointMarkers(My.ArcMap.Application, item.Value, symbology.DisplayName, symbology.Color, symbology.MarkerType)
                    Else
                        BA_DisplayVector(My.ArcMap.Document, item.Value)
                    End If
                Next
            End If

        Catch ex As Exception
            MessageBox.Show("An error occurred while trying to view one or more of the layers you requested.", "Error", MessageBoxButtons.OK)
            Debug.Print("AOI path: " & m_aoi.FilePath)
            Debug.Print("AOI path length: " & m_aoi.FilePath.Length)
            Debug.Print("BtnViewAoi_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub ChkPRISMSelected_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkPRISMSelected.CheckedChanged
        If ChkPRISMSelected.Checked Or ChkSNOTELSelected.Checked Or ChkSnowCourseSelected.Checked = True Then
            CmdReClip.Enabled = True
        Else
            CmdReClip.Enabled = False
        End If
    End Sub

    Private Sub ChkSNOTELSelected_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkSNOTELSelected.CheckedChanged
        If ChkSNOTELSelected.Checked Or ChkPRISMSelected.Checked Or ChkSnowCourseSelected.Checked = True Then
            CmdReClip.Enabled = True
        Else
            CmdReClip.Enabled = False
        End If
    End Sub

    Private Sub ChkSnowCourseSelected_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkSnowCourseSelected.CheckedChanged
        If ChkSnowCourseSelected.Checked Or ChkPRISMSelected.Checked Or ChkSNOTELSelected.Checked = True Then
            CmdReClip.Enabled = True
        Else
            CmdReClip.Enabled = False
        End If
    End Sub

    Private Sub LoadLstLayers()
        Dim ShapefileCount As Long, RasterCount As Long
        Dim i As Long

        ' Create/configure a step progressor
        Dim stepCount As Short = 6  'Loading 4 dropdowns
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, stepCount)
        pStepProg.Show()
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Loading list layers", "loading...")

        Try
            LstVectors.Items.Clear()
            LstRasters.Items.Clear()
            progressDialog2.ShowDialog()
            pStepProg.Step()
            Dim AOIVectorList() As String = Nothing
            Dim AOIRasterList() As String = Nothing
            Dim layerPath As String = m_aoi.FilePath & "\" & BA_EnumDescription(GeodatabaseNames.Layers)
            BA_ListLayersinGDB(layerPath, AOIRasterList, AOIVectorList)
            pStepProg.Step()

            'display shapefiles
            ShapefileCount = UBound(AOIVectorList)
            If ShapefileCount > 0 Then
                For i = 1 To ShapefileCount
                    ' Vectors are always discrete
                    Dim isDiscrete As Boolean = True
                    Dim fullLayerPath As String = layerPath & "\" & AOIVectorList(i)
                    Dim item As LayerListItem = New LayerListItem(AOIVectorList(i), fullLayerPath, LayerType.Vector, isDiscrete)
                    LstVectors.Items.Add(item)
                Next
            End If

            pStepProg.Step()

            'display raster layers
            RasterCount = UBound(AOIRasterList)
            If RasterCount > 0 Then
                For i = 1 To RasterCount
                    Dim fullLayerPath As String = layerPath & "\" & AOIRasterList(i)
                    Dim isDiscrete As Boolean = BA_IsIntegerRasterGDB(fullLayerPath)
                    Dim item As LayerListItem = New LayerListItem(AOIRasterList(i), fullLayerPath, LayerType.Raster, isDiscrete)
                    LstRasters.Items.Add(item)
                Next
            End If
            pStepProg.Step()

        Catch ex As Exception
            MessageBox.Show("LoadLstLayers() Exception: " & ex.Message)
        Finally
            ' Clean up step progressor
            pStepProg.Hide()
            pStepProg = Nothing
            progressDialog2.HideDialog()
            progressDialog2 = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub

    'Private Sub frmAOIInfo_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
    '    LstRasters.Items.Clear()
    '    LstVectors.Items.Clear()

    '    Dim layerPath As String = m_aoi.FilePath & "\" & BA_EnumDescription(GeodatabaseNames.Layers)
    '    BA_ListLayersinGDB(layerPath, AOIRasterList, AOIVectorList)

    '    'display shapefiles
    '    Dim ShapefileCount As Long
    '    ShapefileCount = UBound(AOIVectorList)
    '    If ShapefileCount > 0 Then
    '        For i = 1 To ShapefileCount
    '            ' Vectors are always discrete
    '            Dim isDiscrete As Boolean = True
    '            Dim fullLayerPath As String = layerPath & "\" & AOIVectorList(i)
    '            Dim item As LayerListItem = New LayerListItem(AOIVectorList(i), fullLayerPath, LayerType.Vector, isDiscrete)
    '            LstVectors.Items.Add(item)
    '        Next
    '    End If


    '    'display raster layers
    '    Dim RasterCount As Long
    '    RasterCount = UBound(AOIRasterList)
    '    If RasterCount > 0 Then
    '        For i = 1 To RasterCount
    '            Dim fullLayerPath As String = layerPath & "\" & AOIRasterList(i)
    '            Dim isDiscrete As Boolean = BA_IsIntegerRasterGDB(fullLayerPath)
    '            Dim item As LayerListItem = New LayerListItem(AOIRasterList(i), fullLayerPath, LayerType.Raster, isDiscrete)
    '            LstRasters.Items.Add(item)
    '        Next
    '    End If
    'End Sub







    'Back up for BA_ClipAOIRaster
    'Public Function BA_ClipAOIRaster(ByVal AOIFolder As String, ByVal InputRaster As String, ByVal OutputRasterName As String, _
    '                                 ByVal gdbName As GeodatabaseNames, ByVal AOIClipKey As AOIClipFile) As Short
    '    'prepare for data clipping
    '    'get vector clipping mask, raster clipping mask is created earlier, i.e., pWaterRDS
    '    Dim return_value As Short = 0
    '    Dim Data_Path As String = ""
    '    Dim Data_Name As String
    '    Dim OutputName As String
    '    Dim ClipShapeFile As String = Nothing
    '    Dim pClipFCursor As IFeatureCursor
    '    Dim pClipFeature As IFeature
    '    Dim pGeo As IGeometry
    '    Dim pAOIEnvelope As IEnvelope
    '    Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
    '    Dim tool As ESRI.ArcGIS.DataManagementTools.Clip = New ESRI.ArcGIS.DataManagementTools.Clip()

    '    If String.IsNullOrEmpty(InputRaster) Then
    '        Return -3
    '    End If

    '    Data_Name = BA_GetBareName(InputRaster, Data_Path)

    '    Dim pClipFeatureLayer As IFeatureLayer = New FeatureLayer
    '    Dim pClipFClass As IFeatureClass

    '    Try
    '        ClipShapeFile = BA_EnumDescription(AOIClipKey)

    '        'Get gdb Workspace path added by Momeni
    '        Dim outputFolder As String = AOIFolder & "\" & BA_EnumDescription(gdbName)

    '        Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(outputFolder)
    '        If Not BA_Folder_ExistsWindowsIO(outputFolder) Then
    '            Throw New Exception("Output geodatabase folder " + outputFolder + " does not exist.")
    '        End If
    '        If String.IsNullOrEmpty(OutputRasterName) Then 'user didn't specify an output name
    '            OutputName = outputFolder & "\" & Data_Name
    '        Else

    '            If workspaceType = BAGIS_ClassLibrary.WorkspaceType.Raster And OutputRasterName.Length > BA_GRID_NAME_MAX_LENGTH Then
    '                Throw New Exception("Output raster name cannot exceed " + CStr(BA_GRID_NAME_MAX_LENGTH) + " characters")
    '            End If
    '            OutputName = outputFolder & "\" & OutputRasterName
    '        End If

    '        'check if a layer of the same name exists in the AOI
    '        If BA_File_Exists(OutputName, workspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
    '            Return -2
    '        End If

    Private Sub CmdUpdateWeasel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdUpdateWeasel.Click
        If Len(AOIFolderBase) = 0 Then
            MsgBox("Please set an AOI first!")
            Exit Sub
        End If
        Dim sweaselfilepath As String = m_aoi.FilePath & "\" & BA_AOI_Type
        Dim wweaselfilepath As String = m_aoi.FilePath & "\" & BA_AOIWindow_Type

        'Check if both source weasel and window weasel files exit 
        If Not File.Exists(sweaselfilepath) And Not File.Exists(wweaselfilepath) Then
            MessageBox.Show("The AOI folder does not contain weasel files!", "Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        'check the length of the flow_accumulation path. If the length exceeds 115 char,
        'then Arcinfo (i.e., weasel) cannot use the folder as a workspace.
        'flow_acc path = AOIFolderbase & "\output\surfaces\dem\filled\flow-direction\flow-accumulation"
        'i.e., len(AOIFolderbase) + 60 <= 115 (len(AOIFolderbase) should be shorter than 55
        If Len(m_aoi.FilePath) >= 55 Then 'the aoi folder base name is too long for ArcInfo
            MsgBox("ERROR!" & vbCrLf & _
            "The AOI path name is too long (" & Len(m_aoi.FilePath) - 54 & " character(s) more than the allowed length)!" & vbCrLf & _
            "The AOI cannot be processed in Weasel GIS." & vbCrLf & _
            "ArcInfo doesn't allow the path string to exceed 128 char and a workspace path to exceed 115 char.")
            Exit Sub
        End If

        Dim response As Integer
        Dim update_message As String
        update_message = "Do you want to update the Weasel GIS files in the AOI?" & vbCrLf & "AOI Folder:" & vbCrLf & AOIFolderBase

        response = MsgBox(update_message, vbYesNo, "Update Weasel Info in AOI")

        If response = vbNo Then Exit Sub

        Dim filepath As String

        'save the dem definition file, dem extent file, and the dem info file
        Dim aoiTypSuccess As BA_ReturnCode = BA_Create_FolderType_File(AOIFolderBase, BA_AOI_Type, BA_GetPath(AOIFolderBase, PublicPath.SourceDEM) & "/grid")
        If aoiTypSuccess <> BA_ReturnCode.Success Then
            ' response = BA_Create_FolderType_File(AOIFolderBase, BA_AOI_Type, BA_GetPath(AOIFolderBase, "SOURCEDEM") & "\grid")
            'If response <> 1 Then
            filepath = AOIFolderBase & "\" & BA_AOI_Type
            MsgBox("Update failed!" & vbCrLf & "Cannot write to " & filepath & vbCrLf & "Please check if the path exists.")
            Exit Sub
        End If

        Dim WTypeSuccess As BA_ReturnCode = BA_Create_FolderType_File(AOIFolderBase, BA_AOIWindow_Type, BA_GetPath(AOIFolderBase, PublicPath.SourceDEM) & "/grid")
        If WTypeSuccess <> BA_ReturnCode.Success Then
            filepath = AOIFolderBase & BA_AOIWindow_Type
            MsgBox("Update failed!" & vbCrLf & "Cannot write to " & filepath & vbCrLf & "Please check if the path exists.")
            Exit Sub
        End If

        'create source.weasel file for flow_direction
        Dim FDirSuccess As BA_ReturnCode = BA_Create_SourceWeasel_File(AOIFolderBase & BA_EnumDescription(PublicPath.FlowDirection), "source.weasel", AOIFolderBase & BA_EnumDescription(PublicPath.FlowDirection) & "\grid")
        If FDirSuccess <> BA_ReturnCode.Success Then
            filepath = AOIFolderBase & PublicPath.FlowDirection
            MsgBox("Update failed!" & vbCrLf & "Cannot write to " & filepath & vbCrLf & "Please check if the path exists.")
            Exit Sub
        End If

        'create source.weasel file for flow_accumulation
        Dim FAccSucces As BA_ReturnCode = BA_Create_SourceWeasel_File(AOIFolderBase & BA_EnumDescription(PublicPath.FlowAccumulation), "source.weasel", AOIFolderBase & BA_EnumDescription(PublicPath.FlowAccumulation) & "\grid")
        If FAccSucces <> BA_ReturnCode.Success Then
            filepath = AOIFolderBase & PublicPath.FlowAccumulation
            MsgBox("Update failed!" & vbCrLf & "Cannot write to " & filepath & vbCrLf & "Please check if the path exists.")
            Exit Sub
        End If

        MsgBox("The Weasel GIS files in the AOI are updated for the current AOI workspace!")
    End Sub

    Private Sub CmdAddLayer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdAddLayer.Click
        'MsgBox("Test")

        Dim bObjectSelected As Boolean = True
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim Data_Path As String = "", Data_Name As String, data_type As Object
        Dim data_type_code As Integer '1. shapefile, 2. Raster, 0. Unsupported format
        Dim data_fullname As String
        Dim importDone As Boolean = False

        Dim pFilter As IGxObjectFilter = New GxFilterDatasets

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select a GIS dataset to add to AOI"
            .ObjectFilter = pFilter
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then Exit Sub

        'get the name of the selected folder
        Dim pGxDataset As IGxDataset
        pGxDataset = pGxObject.Next
        Dim pDatasetName As IDatasetName
        pDatasetName = pGxDataset.DatasetName
        Data_Path = pDatasetName.WorkspaceName.PathName
        Data_Name = pDatasetName.Name
        data_type = pDatasetName.Type

        'Set Data Type Name from Data Type
        Select Case data_type
            Case 4, 5 'shapefile
                data_type_code = 1
            Case 12, 13 'raster
                data_type_code = 2
            Case Else 'unsupported format
                data_type_code = 0
        End Select

        'pad a backslash to the path if it doesn't have one.
        'If String.(Data_Path, 1) <> "\" Then Data_Path = Data_Path & "\"
        Data_Path = BA_StandardizePathString(Data_Path, True)

        data_fullname = Data_Path & Data_Name
        If String.IsNullOrEmpty(data_fullname) Then Exit Sub 'user cancelled the action

        'allow user to specify a different output name
        Dim outlayername As String
        outlayername = InputBox("Set output layer name (please don't use space in the name):", "Clip Layer to AOI", Data_Name)

        If String.IsNullOrEmpty(outlayername) Then 'user cancelled the action
            Exit Sub
        End If

        ' Create/configure a step progressor
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 4)
        Dim progressDialog2 As IProgressDialog2 = Nothing
        If data_type_code = 1 Then
            progressDialog2 = BA_GetProgressDialog(pStepProg, "Adding the vector layer ", "Adding...")
        ElseIf data_type_code = 2 Then
            progressDialog2 = BA_GetProgressDialog(pStepProg, "Adding the raster layer ", "Adding...")
        End If
        pStepProg.Show()
        progressDialog2.ShowDialog()
        pStepProg.Step()

        'check if a layer of the same name exist
        Try
            Dim Layer_Exist As Boolean = False
            If data_type_code = 1 Then 'shapefile
                For i As Integer = 0 To LstVectors.Items.Count - 1
                    If LstVectors.Items(i).name = outlayername Then
                        Layer_Exist = True
                        Exit For
                    End If
                Next

                'check if the selected layer is raster then check if the chosen name exists in the raster list or not
            ElseIf data_type_code = 2 Then
                For i As Integer = 0 To LstRasters.Items.Count - 1
                    If LstRasters.Items(i).name = outlayername Then
                        Layer_Exist = True
                        Exit For
                    End If
                Next
            Else
                MsgBox("The data type of " & Data_Name & " is not supported. No layer was added to the AOI.")
                Exit Sub
            End If

            If Layer_Exist Then
                'MsgBox(outlayername & " already exists in the AOI! Please Choose another name.")
                Throw New Exception(vbCrLf + outlayername + " is already exists")
            End If
            'confirm the selection
            'response = MsgBox("Clip " & Data_Name & " to the AOI?" & vbCrLf & "Output: " & outlayername, vbYesNo)
            'If response = vbNo Then Exit Sub

            'prepare for data clipping
            Dim response As Integer
            Dim outputFolder As String = m_aoi.FilePath & "\" & BA_EnumDescription(GeodatabaseNames.Layers)
            If data_type_code = 1 Then 'clip shapefile
                response = BA_ClipAOIVector(m_aoi.FilePath, data_fullname, outlayername, outputFolder, True)
                If response <= 0 Then
                    Throw New Exception(vbCrLf + "Vector clipping failed! Return value = " & response & ".")
                End If
                pStepProg.Step()

                'Add vector to the list for display
                Dim fullLayerPath As String = Data_Path & outlayername
                Dim item As LayerListItem = New LayerListItem(outlayername, fullLayerPath, LayerType.Vector, True)
                LstVectors.Items.Add(item)

            ElseIf data_type_code = 2 Then 'raster clip
                Dim clipKey As AOIClipFile = 1
                response = BA_ClipAOIRaster(m_aoi.FilePath, data_fullname, outlayername, outputFolder, clipKey)
                If response <= 0 Then
                    Throw New Exception(vbCrLf + "Raster Clipping failed! Return value = " & response & ".")
                    Exit Sub
                End If
                importDone = True
                pStepProg.Step()
                'Add Raster to the list for display
                Dim fullLayerPath As String = Data_Path & outlayername
                Dim isDiscrete As Boolean = BA_IsIntegerRaster(fullLayerPath)
                Dim item As LayerListItem = New LayerListItem(outlayername, fullLayerPath, LayerType.Raster, isDiscrete)
                LstRasters.Items.Add(item)
            End If
            frmAOIInfo_Load(Me, Nothing)
            MsgBox("Clipping done!")

        Catch ex As Exception
            MessageBox.Show("Add new layer exception: " & ex.Message)
        Finally

            pStepProg.Hide()
            pStepProg = Nothing
            progressDialog2.HideDialog()
            progressDialog2 = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try


        'From BAGIS-H, frmAOIViewer/btnImport
        '    Dim pFilter As IGxObjectFilter = New GxFilterDatasets
        '    Dim pGxDialog As IGxDialog = New GxDialog
        '    Dim pGxObject As IEnumGxObject = Nothing
        '    Dim bObjectSelected As Boolean
        '    Dim importDone As Boolean = False

        '    With pGxDialog
        '        .AllowMultiSelect = False
        '        .ButtonCaption = "Select"
        '        .Title = "Select a GIS dataset to add to AOI"
        '        .ObjectFilter = pFilter
        '        bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        '    End With
        '    If bObjectSelected = False Then Exit Sub

        '    'get the name of the selected folder
        '    Dim pGxDataset As IGxDataset = pGxObject.Next
        '    Dim pDatasetName As IDatasetName = pGxDataset.DatasetName
        '    Dim Data_Path As String = pDatasetName.WorkspaceName.PathName
        '    Dim Data_Name As String = pDatasetName.Name
        '    Dim data_type As Object = pDatasetName.Type
        '    Dim data_type_code As Integer '1. shapefile, 2. Raster, 0. Unsupported format

        '    pGxDialog = Nothing
        '    pGxObject = Nothing
        '    pGxDataset = Nothing
        '    pFilter = Nothing
        '    pDatasetName = Nothing

        '    'Set Data Type Name from Data Type
        '    Select Case data_type
        '        Case 4, 5 'shapefile
        '            data_type_code = 1

        '        Case 12, 13 'raster
        '            data_type_code = 2

        '        Case Else 'unsupported format
        '            data_type_code = 0
        '    End Select

        '    'pad a backslash to the path if it doesn't have one.
        '    If Data_Path(Len(Data_Path) - 1) <> "\" Then Data_Path = Data_Path & "\"
        '    Dim data_fullname As String = Data_Path & Data_Name
        '    If Len(Trim(data_fullname)) = 0 Then Exit Sub 'user cancelled the action

        '    'allow user to specify a different output name
        '    Dim outlayername As String = InputBox("Set output layer name (please don't use any space in the name):", "Clip Layer to AOI", Data_Name)
        '    If Len(Trim(outlayername)) = 0 Then Exit Sub

        '    ' Create/configure a step progressor
        '    Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 4)
        '    Dim progressDialog2 As IProgressDialog2 = Nothing
        '    If data_type_code = 1 Then
        '        progressDialog2 = BA_GetProgressDialog(pStepProg, "Importing the vector file ", "Importing...")
        '    ElseIf data_type_code = 2 Then
        '        progressDialog2 = BA_GetProgressDialog(pStepProg, "Importing the raster file ", "Importing...")
        '    End If
        '    pStepProg.Show()
        '    progressDialog2.ShowDialog()
        '    pStepProg.Step()

        '    'check if a layer is in the correct projection
        '    'Dim validDatum As Boolean
        '    'Dim hruExt As HruExtension = HruExtension.GetExtension
        '    'If data_type_code = 1 Then 'shapefile
        '    '    validDatum = BA_VectorDatumMatch(data_fullname, hruExt.Datum)
        '    'ElseIf data_type_code = 2 Then
        '    '    validDatum = BA_DatumMatch(data_fullname, hruExt.Datum)
        '    'End If
        '    'If validDatum = False Then
        '    '    MessageBox.Show("The selected layer '" & Data_Name & "' cannot be imported because the datum does not match the AOI DEM. Please reproject to " & hruExt.SpatialReference & " and try again.", "Invalid datum", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    '    Exit Sub
        '    'End If

        '    'check if a layer of the same name exist
        '    Dim Layer_Exist As Boolean = False
        '    If data_type_code = 1 Then 'shapefile
        '        For idx As Integer = 0 To LstVectors.Items.Count - 1
        '            If LstVectors.Items(idx).name = outlayername Then
        '                Layer_Exist = True
        '                Exit For
        '            End If
        '        Next
        '    ElseIf data_type_code = 2 Then
        '        For idx As Integer = 0 To LstRasters.Items.Count - 1
        '            If LstRasters.Items(idx).name = outlayername Then
        '                Layer_Exist = True
        '                Exit For
        '            End If
        '        Next
        '    Else
        '        MsgBox("The data type of " & Data_Name & " is not supported. No layer was added to the AOI.")
        '        Exit Sub
        '    End If

        '    If Layer_Exist Then
        '        MsgBox(outlayername & " already exists in the AOI! The action is aborted.")
        '        Exit Sub
        '    End If
        '    pStepProg.Step()

        '    Dim response As Integer

        '    Try

        '        'confirm the selection
        '        'response = MsgBox("Clip " & data_name & " to the AOI?" & vbCrLf & "Output: " & outlayername, vbYesNo)
        '        'If response = vbNo Then Exit Sub

        '        ''prepare for data clipping
        '        Dim outFoldername As String = BA_EnumDescription(GeodatabaseNames.Layers)
        '        If data_type_code = 1 Then 'clip shapefile
        '            MsgBox(AOIFolderBase & ", " & data_fullname & ", " & outlayername & ", " & GeodatabaseNames.Layers.ToString)
        '            response = BA_ClipAOIVector(m_aoi.FilePath, data_fullname, outlayername, outFoldername, True)

        '            If response <= 0 Then
        '                MsgBox("Import failed! Layer is out of range of AOI.")
        '                Exit Sub
        '            End If

        '            importDone = True
        '            pStepProg.Step()
        '            'Add vector to the list for display
        '            Dim fullLayerPath As String = Data_Path & "\" & outlayername
        '            Dim item As LayerListItem = New LayerListItem(outlayername, fullLayerPath, LayerType.Vector, True)
        '            LstVectors.Items.Add(item)

        '        ElseIf data_type_code = 2 Then 'raster clip
        '            Dim outfolder As String = BA_EnumDescription(GeodatabaseNames.Layers)
        '            Dim clipKey As AOIClipFile = 1
        '            response = BA_ClipAOIRaster(m_aoi.FilePath, data_fullname, outlayername, outfolder, clipKey)
        '            If response <= 0 Then
        '                MsgBox("Import Failed! Layer is out of range of AOI.")
        '                Exit Sub
        '            End If
        '            importDone = True
        '            pStepProg.Step()
        '            'Add Raster to the list for display
        '            Dim fullLayerPath As String = Data_Path & "\" & outlayername
        '            Dim isDiscrete As Boolean = BA_IsIntegerRaster(fullLayerPath)
        '            Dim item As LayerListItem = New LayerListItem(outlayername, fullLayerPath, LayerType.Raster, isDiscrete)
        '            LstRasters.Items.Add(item)
        '            'End If

        '            ''Get handle to UI (form) to reload user layer lists
        '            'Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of frmHruZone.AddinImpl)(My.ThisAddIn.IDs.frmHruZone)
        '            'If dockWindowAddIn IsNot Nothing Then
        '            '    Dim hruZoneForm As frmHruZone = dockWindowAddIn.UI
        '            '    If hruZoneForm IsNot Nothing Then
        '            '        hruZoneForm.BA_ReloadUserLayers()
        '            '    End If
        '        End If

        '    Catch ex As Exception
        '        MessageBox.Show("BtnImport_Click Exception: " & ex.Message)
        '    Finally
        '        'pStepProg.Hide()
        '        'pStepProg = Nothing
        '        'progressDialog2.HideDialog()
        '        'progressDialog2 = Nothing
        '        If importDone Then MessageBox.Show("Import is completed.")
        '        GC.WaitForPendingFinalizers()
        '        GC.Collect()
        '    End Try
    End Sub
End Class