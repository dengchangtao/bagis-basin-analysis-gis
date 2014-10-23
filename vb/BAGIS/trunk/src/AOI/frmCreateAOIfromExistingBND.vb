Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.Framework
Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.Desktop.AddIns
Imports System.ComponentModel
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.SpatialAnalyst
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.ArcMapUI
Imports System.Text
Imports ESRI.ArcGIS.DataSourcesRaster
Imports System.Windows.Forms
Imports System.IO
Imports ESRI.ArcGIS.DataSourcesGDB
Imports ESRI.ArcGIS.GeoAnalyst
Imports ESRI.ArcGIS.SpatialAnalystTools

Public Class frmCreateAOIfromExistingBND

    Private Sub cmbSelectAll_Click(sender As System.Object, e As System.EventArgs) Handles cmbSelectAll.Click
        SetAllCheckBoxes(True)
    End Sub

    Private Sub SetAllCheckBoxes(ByVal Value As Boolean)
        ChkDEMExtent.Checked = Value
        ChkFilledDEM.Checked = Value
        ChkFlowDir.Checked = Value
        ChkFlowAccum.Checked = Value
        ChkSlope.Checked = Value
        ChkAspect.Checked = Value
        ChkHillshade.Checked = Value
    End Sub

    Private Sub cmbSelectNone_Click(sender As System.Object, e As System.EventArgs) Handles cmbSelectNone.Click
        SetAllCheckBoxes(False)
    End Sub

    Private Sub btnSelectWorkspace_Click(sender As System.Object, e As System.EventArgs) Handles btnSelectWorkspace.Click
        Dim bObjectSelected As Boolean = True
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim basinfoldertext As String = ""

        Dim pFilter As IGxObjectFilter = New GxFilterContainers

        Dim nSubFolder As Long = 0
        Dim SubFlist() As BA_SubFolderList = Nothing

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select Basin Folder"
            .ObjectFilter = pFilter
            'bObjectSelected = .DoModalOpen(0, pGxObject)
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then 'user canceled the action
            Exit Sub
        End If

        Dim pGxDataFolder As IGxFile
        pGxDataFolder = pGxObject.Next
        basinfoldertext = pGxDataFolder.Path

        If String.IsNullOrEmpty(basinfoldertext) Then
            pGxDataFolder = Nothing
            pGxDataFolder = Nothing
            pGxObject = Nothing
            Exit Sub 'user cancelled the action
        Else
            txtOutputWorkspace.Text = basinfoldertext
        End If
        Me.Activate()
    End Sub

    Private Sub lblWhy_Click(sender As System.Object, e As System.EventArgs) Handles lblWhy.Click
        Dim mText = "Smoothing DEM using a directional filter can effectively remove the"
        mText = mText & " striping artifact in older USGS 7.5 minute (i.e., 30 meters) DEM."
        mText = mText & " When present, the striping is most prominent on DEM derivative"
        mText = mText & " surfaces such as slope, curvature, or hillshade. Please inspect"
        mText = mText & " these derivatives right after a BASIN was created. If there is clear"
        mText = mText & " striping, then recreate the BASIN with the smooth DEM option"
        mText = mText & " checked. A recommended filter size is 3 by 7 (height by width)."
        MsgBox(mText, MsgBoxStyle.Information, "Why Smooth DEM")
    End Sub

    Private Sub cmbRun_Click(sender As System.Object, e As System.EventArgs) Handles cmbRun.Click
        Dim response As Integer
        Dim nstep As Integer

        'verify input parameters
        'output workspace and name

        If Len(Trim(txtOutputWorkspace.Text)) = 0 Or Len(Trim(txtOutputName.Text)) = 0 Then
            MsgBox("Missing output workspace or AOI name!")
            Exit Sub
        End If

        'verify filter size parameters
        If Val(txtHeight.Text) <= 0 Or Val(txtWidth.Text) <= 0 Then
            MsgBox("Invalid filter size! Please reenter.")
            Exit Sub
        End If

        If Not BA_Workspace_Exists(Trim(txtOutputWorkspace.Text)) Then
            MsgBox("Output workspace does not exist!")
            Exit Sub
        End If

        Dim UserAOIFolderBase As String = txtOutputWorkspace.Text + "\" + txtOutputName.Text
        Dim ListLayerCount As Integer = BA_SystemSettings.listCount
        Dim internalLayerCount As Integer = 0

        If BA_SystemSettings.GenerateAOIOnly Then
            internalLayerCount = 6
        Else
            internalLayerCount = 25
        End If

        nstep = internalLayerCount + ListLayerCount 'step counter for frmmessage

        Dim strDEMDataSet As String
        Me.Hide()

        If Opt10Meters.Checked Then
            strDEMDataSet = BA_SystemSettings.DEM10M
            If Len(strDEMDataSet) = 0 Then
                MsgBox("The source of 10 meters DEM is not specified in the setting!")
                Exit Sub
            End If
        Else
            strDEMDataSet = BA_SystemSettings.DEM30M
            If Len(strDEMDataSet) = 0 Then
                MsgBox("The source of 30 meters DEM is not specified in the setting!")
                Exit Sub
            End If
        End If

        'check if the Surfaces GDB exists, if so delete the GDB, otherwise, create it
        Dim sourceDEMPath As String = ""
        Dim sourceDEMName As String = BA_GetBareName(strDEMDataSet, sourceDEMPath)
        Dim destSurfGDB As String = UserAOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces)
        Dim destAOIGDB As String = UserAOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Aoi)
        Dim destLayersGDB As String = UserAOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Layers)
        Dim destPRISMGDB As String = UserAOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Prism)

        Dim comReleaser As ESRI.ArcGIS.ADF.ComReleaser = New ESRI.ArcGIS.ADF.ComReleaser()

        If BA_Workspace_Exists(UserAOIFolderBase) Then
            ''''If BA_File_Exists(TempAOIFolderName, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            response = MsgBox(UserAOIFolderBase & " folder already exists. Overwrite?", vbYesNo)
            If response = vbYes Then
                'delete the aoi folder
                Dim LayerRemoved As Short = Nothing
                Dim DeleteStatus As Integer
                'clean up the stuff that was created
                'remove layers from the map that are in the AOI folder
                LayerRemoved = BA_RemoveLayersInFolder(My.ArcMap.Document, UserAOIFolderBase)

                'delete aoi folder
                DeleteStatus = BA_Remove_Folder(UserAOIFolderBase)
                If DeleteStatus = 0 Then 'unable to delete the folder
                    MsgBox("Unable to remove the folder. Program stopped. Please restart ArcMap and try again.")
                    Exit Sub
                End If
                BA_CreateFolder(txtOutputWorkspace.Text, txtOutputName.Text)
            Else
                Exit Sub
            End If
        Else
            BA_CreateFolder(txtOutputWorkspace.Text, txtOutputName.Text)
        End If


        'The BA_Create_Output_Folders function can delete the file structure if it exists
        For Each pName In [Enum].GetValues(GetType(GeodatabaseNames))
            Dim EnumConstant As [Enum] = pName
            Dim fi As Reflection.FieldInfo = EnumConstant.GetType().GetField(EnumConstant.ToString())
            Dim aattr() As DescriptionAttribute = _
                DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
            Dim gdbName As String = aattr(0).Description
            Dim gdbpath As String = UserAOIFolderBase & "\" & gdbName

            If BA_Workspace_Exists(gdbpath) Then
                If BA_DeleteGeodatabase(UserAOIFolderBase & "\" & gdbName, My.ArcMap.Document) <> BA_ReturnCode.Success Then
                    MessageBox.Show("Unable to delete Geodatabase '" & gdbName & "'. Please restart ArcMap and try again", "Unable to delete Geodatabase", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            End If
        Next

        Dim success As BA_ReturnCode = BA_CreateGeodatabaseFolders(UserAOIFolderBase, FolderType.AOI)

        If success <> BA_ReturnCode.Success Then
            MsgBox("Unable to create GDBs! Please check disk space")
            Exit Sub
        End If

        'create maps folder to save MXDs
        Dim mappath As String = UserAOIFolderBase & "\maps"
        If Not BA_Workspace_Exists(mappath) Then
            mappath = BA_CreateFolder(UserAOIFolderBase, "maps")
        End If

        ' Call Sub Functions to Generate Raster Layers
        Dim pClippedDEM As IGeoDataset2
        Dim pFilledDEM As IGeoDataset2
        Dim pFlowDir As IGeoDataset2
        Dim pFlowAcc As IGeoDataset2
        Dim pSlope As IGeoDataset2
        Dim pAspect As IGeoDataset2
        Dim pHillshade As IGeoDataset2

        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, nstep)
        Dim progressDialog2 As IProgressDialog2 = Nothing
        progressDialog2 = BA_GetProgressDialog(pStepProg, "Clipping DEM to AOI Folder ", "Clipping...")
        pStepProg.Show()
        progressDialog2.ShowDialog()
        pStepProg.Message = "Preparing for clipping..."

        Dim inputraster As String
        Dim sourceShapePath As String = ""
        Dim sourceShapeName As String = BA_GetBareName(txtSourceData.Text, sourceShapePath)

        Dim Distance As Double
        Dim DEMCellSize As Double = 0
        BA_GetRasterStats(sourceDEMPath & "\" & sourceDEMName, DEMCellSize)

        Try
            ''create a raster version of the AOI boundary
            'BA_EnumDescription(PublicPath.AoiGrid)
            If BA_AddValueFieldtoShapefile(txtSourceData.Text, "RASTERID", 1) > 0 Then
                Dim pinFClass As IFeatureClass = BA_OpenFeatureClassFromFile(sourceShapePath, BA_StandardizeShapefileName(sourceShapeName, False, False))
                BA_ShapeFile2RasterGDB(pinFClass, destAOIGDB, BA_AOIExtentRaster, DEMCellSize, "RASTERID", sourceDEMPath & "\" & sourceDEMName)
                pinFClass = Nothing
            Else
                MsgBox("Input shapefile does not contain valid polygons! Please visually inspect the file.")
                Exit Sub
            End If

            'rasterize the raster to vector
            Dim pWatershed As IGeoDataset = BA_OpenRasterFromGDB(destAOIGDB, BA_AOIExtentRaster)
            'convert aoibagis watershed raster to polygon 
            response = BA_Raster2PolygonShapefile(destAOIGDB, BA_AOIExtentCoverage, pWatershed)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWatershed)

            'save the dem, dem extent, and the dem info file
            'response = BA_ConvertShapeFileToGDB(sourceShapePath, sourceShapeName, destAOIGDB, BA_DEMExtentShapefile)
            System.Windows.Forms.Application.DoEvents()

            If response <> 1 Then 'error when creating the shapefile
                pStepProg.Hide()
                progressDialog2.HideDialog()
                MsgBox("Unable to save the DEM extent file! Please check disk space.")
                Exit Sub
            End If

            'update the attribute table of the AOI using basin name
            response = BA_AddAOIVectorAttributes(destAOIGDB, BA_GetBareName(UserAOIFolderBase))
            System.Windows.Forms.Application.DoEvents()

            'Display DEM Extent layer
            If ChkDEMExtent.Enabled Then
                Dim strPathandName As String = destAOIGDB & "\" & BA_DEMExtentShapefile
                Dim pDColor As IRgbColor = New RgbColor
                pDColor.RGB = RGB(255, 0, 0)
                Dim mysuccess As BA_ReturnCode = BA_AddExtentLayer(My.ArcMap.Document, strPathandName, pDColor) ' BasinLayerDisplayNames(1), 0, 2)
                If mysuccess <> BA_ReturnCode.Success Then
                    pStepProg.Hide()
                    progressDialog2.HideDialog()
                    MsgBox("Unable to add the extent layer", "Unknown error")
                End If
            End If

            'clip DEM then save it
            pStepProg.Message = "Clipping DEM... (step 1 of " & nstep & ")"
            pStepProg.Step()

            'buffer AOI for clipping PRISM data
            'create PRISM clipping buffered polygon
            Distance = BA_PRISMClipBuffer    '1000 Meters by default
            If Distance <= 0 Then Distance = 1000

            'use Buffer GP to perform buffer and save the result as a shapefile
            Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
            Dim BufferTool As ESRI.ArcGIS.AnalysisTools.Buffer = New ESRI.ArcGIS.AnalysisTools.Buffer
            With BufferTool
                .in_features = destAOIGDB & "\" & BA_AOIExtentCoverage
                .buffer_distance_or_field = Distance
                .dissolve_option = "ALL"
                .out_feature_class = UserAOIFolderBase & "\" & BA_PRISMClipAOI & ".shp"
            End With
            GP.AddOutputsToMap = False
            GP.Execute(BufferTool, Nothing)

            'save the buffered AOI as a shapefile and then import it into the GDB
            'to prevent a bug when the buffer distance exceed the xy domain limits of the GDB
            'Copy the temporary line shape file to the aoi.gdb
            Dim retVal As BA_ReturnCode = BA_ReturnCode.UnknownError
            retVal = BA_ConvertShapeFileToGDB(UserAOIFolderBase, BA_StandardizeShapefileName(BA_PRISMClipAOI, True, False), destAOIGDB, BA_PRISMClipAOI)
            'create a raster version of the buffered AOI
            Dim fullLayerPath As String = destAOIGDB & BA_EnumDescription(PublicPath.AoiGrid)
            retVal = BA_Feature2RasterGP(UserAOIFolderBase & BA_StandardizeShapefileName(BA_PRISMClipAOI, True, True), destAOIGDB & BA_EnumDescription(PublicPath.AoiPrismGrid), "ID", DEMCellSize, fullLayerPath)
            BA_Remove_Shapefile(UserAOIFolderBase, BA_StandardizeShapefileName(BA_PRISMClipAOI, False))

            If ChkAOIBuffer.Checked Then 'buffer the AOI polygon for clipping
                'use the IFeatureCursorBuffer2 interface to buffer the AOI
                Distance = CDbl(txtBufferD.Text)    'Unit is Meter
                If Distance <= 0 Then Distance = 1000 'default buffer distance
            Else
                Distance = 1 'one meter buffer to dissolve polygons connected at a point
            End If

            With BufferTool
                .in_features = destAOIGDB & "\" & BA_AOIExtentCoverage
                .buffer_distance_or_field = Distance
                .dissolve_option = "ALL"
                .out_feature_class = UserAOIFolderBase & "\" & BA_BufferedAOIExtentCoverage & ".shp"
            End With
            GP.AddOutputsToMap = False
            GP.Execute(BufferTool, Nothing)

            BufferTool = Nothing
            GP = Nothing

            retVal = BA_ConvertShapeFileToGDB(UserAOIFolderBase, BA_StandardizeShapefileName(BA_BufferedAOIExtentCoverage, True, False), destAOIGDB, BA_BufferedAOIExtentCoverage)
            retVal = BA_Feature2RasterGP(UserAOIFolderBase & BA_StandardizeShapefileName(BA_BufferedAOIExtentCoverage, True, True), destAOIGDB & BA_EnumDescription(PublicPath.AoiBufferedGrid), "ID", DEMCellSize, fullLayerPath)
            BA_Remove_Shapefile(UserAOIFolderBase, BA_StandardizeShapefileName(BA_BufferedAOIExtentCoverage, False))

            inputraster = sourceDEMPath & "\" & sourceDEMName

            If ChkSmoothDEM.Checked Then
                response = BA_ClipAOIRaster(UserAOIFolderBase, inputraster, "originaldem", destSurfGDB, AOIClipFile.BufferedAOIExtentCoverage, False)
                Dim ptempDEM As IGeoDataset2 = BA_OpenRasterFromGDB(destSurfGDB, "originaldem")
                pClippedDEM = Smooth(ptempDEM, Val(txtHeight.Text), Val(txtWidth.Text))
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(ptempDEM)

                If pClippedDEM Is Nothing Then 'no dem within the selected area
                    pStepProg.Hide()
                    progressDialog2.HideDialog()
                    MsgBox("Unable to perform smoothing on the selected DEM!")
                    ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStepProg)
                    ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(progressDialog2)
                    GC.WaitForPendingFinalizers()
                    GC.Collect()
                    Exit Sub
                End If

                response = BA_SaveRasterDatasetGDB(pClippedDEM, destSurfGDB, BA_RASTER_FORMAT, BA_EnumDescription(MapsFileName.dem_gdb))
                If response = 0 Then
                    pStepProg.Hide()
                    progressDialog2.HideDialog()
                    MsgBox("Unable to save CLIPPED DEM to Surfaces GDB!", "Warning")
                    ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStepProg)
                    ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(progressDialog2)
                    GC.WaitForPendingFinalizers()
                    GC.Collect()
                    Exit Sub
                End If

                BA_ComputeStatsRasterDatasetGDB(destSurfGDB, BA_EnumDescription(MapsFileName.dem_gdb))
            Else
                response = BA_ClipAOIRaster(UserAOIFolderBase, inputraster, BA_EnumDescription(MapsFileName.dem_gdb), destSurfGDB, AOIClipFile.BufferedAOIExtentCoverage, False)
            End If

            If response <= 0 Then
                pStepProg.Hide()
                progressDialog2.HideDialog()
                MsgBox("There is no DEM within the specified area! Please check your DEM source data.")
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStepProg)
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(progressDialog2)
                GC.WaitForPendingFinalizers()
                GC.Collect()
                Exit Sub
            End If


            '=====================================================================================================================
            'Generate and save Filled DEM
            '=====================================================================================================================
            pClippedDEM = BA_OpenRasterFromGDB(destSurfGDB, BA_EnumDescription(MapsFileName.dem_gdb))
            pStepProg.Message = "Filling DEM... (step 2 of " & nstep & ")"
            pStepProg.Step()
            System.Windows.Forms.Application.DoEvents()
            pFilledDEM = Fill(pClippedDEM)
            'pClippedDEM = Nothing 'release memory
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pClippedDEM)

            If pFilledDEM Is Nothing Then 'no dem within the selected area
                pStepProg.Hide()
                progressDialog2.HideDialog()
                MsgBox("There is no DEM within the specified area! Please check your DEM source data.")
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStepProg)
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(progressDialog2)
                GC.WaitForPendingFinalizers()
                GC.Collect()
                Exit Sub
            End If

            'save filled DEM into surfaces GDB
            pStepProg.Message = "Saving Filled DEM... (step 3 of " & nstep & ")"
            pStepProg.Step()
            System.Windows.Forms.Application.DoEvents()

            response = BA_SaveRasterDatasetGDB(pFilledDEM, destSurfGDB, BA_RASTER_FORMAT, BA_EnumDescription(MapsFileName.filled_dem_gdb))
            If response = 0 Then
                pStepProg.Hide()
                progressDialog2.HideDialog()
                MsgBox("Unable to save FILLED DEM to Surfaces GDB!", "Warning")
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStepProg)
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(progressDialog2)
                GC.WaitForPendingFinalizers()
                GC.Collect()
                Exit Sub
            End If

            BA_ComputeStatsRasterDatasetGDB(destSurfGDB, BA_EnumDescription(MapsFileName.filled_dem_gdb))

            'display filled DEM
            If ChkFilledDEM.Checked Then
                response = BA_DisplayRaster(My.ArcMap.Application, destSurfGDB & "\" & BA_EnumDescription(MapsFileName.filled_dem_gdb)) ' BasinLayerDisplayNames(2))
            End If
            System.Windows.Forms.Application.DoEvents()


            '=====================================================================================================================
            'generate and save Slope
            '=====================================================================================================================
            pStepProg.Message = "Calculating Slope... (step 4 of " & nstep & ")"
            pStepProg.Step()
            System.Windows.Forms.Application.DoEvents()

            pSlope = Slope(pFilledDEM) 'slope in degree

            pStepProg.Message = "Saving Slope...  (step 5 of " & nstep & ")"
            pStepProg.Step()
            System.Windows.Forms.Application.DoEvents()

            response = BA_SaveRasterDatasetGDB(pSlope, destSurfGDB, BA_RASTER_FORMAT, BA_EnumDescription(MapsFileName.slope_gdb))
            If response = 0 Then
                pStepProg.Hide()
                progressDialog2.HideDialog()
                MsgBox("Unable to save SLOPE to Surfaces GDB!", "Warning")
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pSlope)
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFilledDEM)
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStepProg)
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(progressDialog2)
                GC.WaitForPendingFinalizers()
                GC.Collect()
                Exit Sub
            End If
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pSlope)

            BA_ComputeStatsRasterDatasetGDB(destSurfGDB, BA_EnumDescription(MapsFileName.slope_gdb))

            'display Slope
            If ChkSlope.Checked = True Then
                response = BA_DisplayRaster(My.ArcMap.Application, destSurfGDB & "\" & BA_EnumDescription(MapsFileName.slope_gdb)) ' BasinLayerDisplayNames(3))
            End If
            System.Windows.Forms.Application.DoEvents()


            '=====================================================================================================================
            'generate and save Aspect
            '=====================================================================================================================
            pStepProg.Message = "Calculating Aspect... (step 6 of " & nstep & ")"
            pStepProg.Step()
            System.Windows.Forms.Application.DoEvents()

            pAspect = Aspect(pFilledDEM)

            pStepProg.Message = "Saving Aspect... (step 7 of " & nstep & ")"
            pStepProg.Step()
            System.Windows.Forms.Application.DoEvents()

            response = BA_SaveRasterDatasetGDB(pAspect, destSurfGDB, BA_RASTER_FORMAT, BA_EnumDescription(MapsFileName.aspect_gdb))
            If response = 0 Then
                pStepProg.Hide()
                progressDialog2.HideDialog()
                MsgBox("Unable to save ASPECT to Surfaces GDB!", "Warning")
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pAspect)
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFilledDEM)
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStepProg)
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(progressDialog2)
                GC.WaitForPendingFinalizers()
                GC.Collect()
                Exit Sub
            End If
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pAspect)

            BA_ComputeStatsRasterDatasetGDB(destSurfGDB, BA_EnumDescription(MapsFileName.aspect_gdb))

            'display Aspect
            If ChkAspect.Checked = True Then
                response = BA_DisplayRaster(My.ArcMap.Application, destSurfGDB & "\" & BA_EnumDescription(MapsFileName.aspect_gdb))
            End If
            System.Windows.Forms.Application.DoEvents()

            '====================================================================================================================
            'generate and save flow direction
            '====================================================================================================================
            pStepProg.Message = "Calculating Flow Direction... (step 8 of " & nstep & ")"
            pStepProg.Step()
            System.Windows.Forms.Application.DoEvents()

            pFlowDir = FlowDirection(pFilledDEM)

            pStepProg.Message = "Saving Flow Direction... (step 9 of " & nstep & ")"
            pStepProg.Step()
            System.Windows.Forms.Application.DoEvents()

            response = BA_SaveRasterDatasetGDB(pFlowDir, destSurfGDB, BA_RASTER_FORMAT, BA_EnumDescription(MapsFileName.flow_direction_gdb))
            If response = 0 Then
                pStepProg.Hide()
                progressDialog2.HideDialog()
                MsgBox("Unable to save FLOW DIRECTION to Surfaces GDB!", "Warning")
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFlowDir)
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFilledDEM)
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStepProg)
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(progressDialog2)
                GC.WaitForPendingFinalizers()
                GC.Collect()
                Exit Sub
            End If

            BA_ComputeStatsRasterDatasetGDB(destSurfGDB, BA_EnumDescription(MapsFileName.flow_direction_gdb))

            'display Flow Direction
            If ChkFlowDir.Checked = True Then
                response = BA_DisplayRaster(My.ArcMap.Application, destSurfGDB & "\" & BA_EnumDescription(MapsFileName.flow_direction_gdb))
            End If
            System.Windows.Forms.Application.DoEvents()


            '======================================================================================================================
            'generate and save Flow Accumulation
            '======================================================================================================================
            pStepProg.Message = "Calculating Flow Accumulation... (step 10 of " & nstep & ")"
            pStepProg.Step()
            System.Windows.Forms.Application.DoEvents()

            pFlowAcc = FlowAccumulation(pFlowDir)

            pStepProg.Message = "Saving Flow Accumulation... (step 11 of " & nstep & ")"
            pStepProg.Step()
            System.Windows.Forms.Application.DoEvents()

            response = BA_SaveRasterDatasetGDB(pFlowAcc, destSurfGDB, BA_RASTER_FORMAT, BA_EnumDescription(MapsFileName.flow_accumulation_gdb))
            If response = 0 Then
                pStepProg.Hide()
                progressDialog2.HideDialog()
                MsgBox("Unable to save FLOW ACCUMULATION to Surfaces GDB!", "Warning")
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFlowDir)
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFlowAcc)
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFilledDEM)
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStepProg)
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(progressDialog2)
                GC.WaitForPendingFinalizers()
                GC.Collect()
                Exit Sub
            End If
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFlowDir)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFlowAcc)

            BA_ComputeStatsRasterDatasetGDB(destSurfGDB, BA_EnumDescription(MapsFileName.flow_accumulation_gdb))

            'display Flow Accumulation
            If ChkFlowAccum.Checked = True Then
                response = BA_DisplayRaster(My.ArcMap.Application, destSurfGDB & "\" & BA_EnumDescription(MapsFileName.flow_accumulation_gdb)) ', BasinLayerDisplayNames(6))
            End If
            System.Windows.Forms.Application.DoEvents()

            'use centroid of the AOI as the pourpoint

            'create pourpoint using the max of flow_acc value within the AOI
            'get the max of flow acc
            Dim pRasterStates As IRasterStatistics = BA_GetRasterStatsGDB(destSurfGDB & "\" & BA_EnumDescription(MapsFileName.flow_accumulation_gdb), DEMCellSize)
            Dim maxflowacc As Double = pRasterStates.Maximum
            pRasterStates = Nothing

            If BA_ExtractRasterbyValue(destSurfGDB & "\" & BA_EnumDescription(MapsFileName.flow_accumulation_gdb), maxflowacc, destSurfGDB & "\" & "ppraster", 1) = BA_ReturnCode.Success Then
                Dim pPPRaster As IGeoDataset = BA_OpenRasterFromGDB(destSurfGDB, "ppraster")

                If pPPRaster IsNot Nothing Then
                    ' Create Feature from Raster
                    Dim pWSF As IWorkspaceFactory = New FileGDBWorkspaceFactory
                    Dim pFWS As IWorkspace2 = pWSF.OpenFromFile(destAOIGDB, 0)
                    Dim pConversionOp As IConversionOp = New RasterConversionOp
                    pConversionOp.RasterDataToPointFeatureData(pPPRaster, pFWS, BA_POURPOINTCoverage)
                    pPPRaster = Nothing
                    pConversionOp = Nothing
                    pFWS = Nothing
                    pWSF = Nothing

                    'remove temp ppraster from GDB
                    BA_RemoveRasterFromGDB(destSurfGDB, "ppraster")
                Else
                    MsgBox("Unable to create pourpoint layer for the AOI. The output AOI might not be functional.")
                End If
            Else
                MsgBox("Unable to create pourpoint layer for the AOI. The output AOI might not be functional.")
            End If

            'get AOI area and prompt if the user wants to continue
            Dim AOIArea As Double
            AOIArea = BA_GetShapeArea(destAOIGDB & "\" & BA_AOIExtentCoverage) / 1000000 'the shape unit is in sq meters, converted to sq km

            'update the area information in the pourpoint shapefile
            AOI_ShapeArea = AOIArea
            AOI_ShapeUnit = "Square Km"
            AOI_ReferenceUnit = BA_SystemSettings.PourAreaUnit
            response = BA_UpdatePPAttributes(destAOIGDB)

            '=====================================================================================================================
            'generate and save Hillshade
            '=====================================================================================================================
            pStepProg.Message = "Calculating Hillshade... (step 12 of " & nstep & ")"
            pStepProg.Step()
            System.Windows.Forms.Application.DoEvents()

            If txtZFactor.Text <= 0 Then
                txtZFactor.Text = 1
            End If

            pHillshade = Hillshade(pFilledDEM, Val(txtZFactor.Text))

            pStepProg.Message = "Saving Hillshade... (step 13 of " & nstep & ")"
            pStepProg.Step()
            System.Windows.Forms.Application.DoEvents()

            response = BA_SaveRasterDatasetGDB(pHillshade, destSurfGDB, BA_RASTER_FORMAT, BA_EnumDescription(MapsFileName.hillshade_gdb))
            If response = 0 Then
                pStepProg.Hide()
                progressDialog2.HideDialog()
                MsgBox("Unable to save HILLSHADE to Surfaces GDB!", "Warning")
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pHillshade)
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFilledDEM)
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStepProg)
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(progressDialog2)
                GC.WaitForPendingFinalizers()
                GC.Collect()
                Exit Sub
            End If
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pHillshade)

            BA_ComputeStatsRasterDatasetGDB(destSurfGDB, BA_EnumDescription(MapsFileName.hillshade_gdb))

            'display Hillshade layer
            If ChkHillshade.Checked = True Then
                response = BA_DisplayRaster(My.ArcMap.Application, destSurfGDB & "\" & BA_EnumDescription(MapsFileName.hillshade_gdb)) ' BasinLayerDisplayNames(7))
            End If
            System.Windows.Forms.Application.DoEvents()

            'pFilledDEM = Nothing   'release memory
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFilledDEM)

        Catch ex As Exception
            MsgBox("Clipping Error!", ex.Message)

            'remove layers in the DEM folder from the active map
            response = BA_RemoveLayersInFolder(My.ArcMap.Document, destSurfGDB)
            response = BA_RemoveLayersInFolder(My.ArcMap.Document, destAOIGDB)

            BA_Remove_FolderGP(destSurfGDB)
            BA_Remove_FolderGP(destAOIGDB)

            If BA_Workspace_Exists(destSurfGDB) Or BA_Workspace_Exists(destAOIGDB) Then
                MsgBox("Unable to clear BASIN's internal file geodatabase. Please restart ArcMap and try again.")
            End If
            Exit Sub

        End Try

        ''get AOI area and prompt if the user wants to continue
        'Dim AOIArea As Double
        'AOIArea = BA_GetShapeArea(destAOIGDB & "\" & BA_AOIExtentCoverage) / 1000000 'the shape unit is in sq meters, converted to sq km

        ''update the area information in the pourpoint shapefile
        'AOI_ShapeArea = AOIArea
        'AOI_ShapeUnit = "Square Km"
        'AOI_ReferenceUnit = BA_SystemSettings.PourAreaUnit
        'AOI_ReferenceArea = 0
        'response = BA_UpdatePPAttributes(destAOIGDB)

        'update the Z unit metadata of DEM, slope, and PRISM
        Dim inputFolder As String
        Dim inputFile As String
        Dim unitText As String

        'We need to update the elevation units
        inputFolder = BA_GeodatabasePath(UserAOIFolderBase, GeodatabaseNames.Surfaces)
        inputFile = BA_EnumDescription(MapsFileName.filled_dem_gdb)

        If BA_SystemSettings.DEM_ZUnit_IsMeter Then
            unitText = BA_EnumDescription(MeasurementUnit.Meters)
        Else
            unitText = BA_EnumDescription(MeasurementUnit.Feet)
        End If

        Dim sb As StringBuilder = New StringBuilder
        sb.Clear()
        sb.Append(BA_BAGIS_TAG_PREFIX)
        sb.Append(BA_ZUNIT_CATEGORY_TAG & MeasurementUnitType.Elevation.ToString & "; ")
        sb.Append(BA_ZUNIT_VALUE_TAG & unitText & ";")
        sb.Append(BA_BAGIS_TAG_SUFFIX)
        BA_UpdateMetadata(inputFolder, inputFile, LayerType.Raster, BA_XPATH_TAGS, _
                          sb.ToString, BA_BAGIS_TAG_PREFIX.Length)

        'We need to update the slope units 'always set it to Degree slope
        inputFolder = BA_GeodatabasePath(UserAOIFolderBase, GeodatabaseNames.Surfaces)
        inputFile = BA_GetBareName(BA_EnumDescription(PublicPath.Slope))
        sb.Clear()
        sb.Append(BA_BAGIS_TAG_PREFIX)
        sb.Append(BA_ZUNIT_CATEGORY_TAG & MeasurementUnitType.Slope.ToString & "; ")
        sb.Append(BA_ZUNIT_VALUE_TAG & BA_EnumDescription(SlopeUnit.PctSlope) & ";")
        sb.Append(BA_BAGIS_TAG_SUFFIX)
        BA_UpdateMetadata(inputFolder, inputFile, LayerType.Raster, BA_XPATH_TAGS, _
                          sb.ToString, BA_BAGIS_TAG_PREFIX.Length)

        'get vector clipping mask, raster clipping mask is created earlier, i.e., pWaterRDS
        Dim strInLayerBareName As String
        Dim strInLayerPath As String
        Dim strExtension As String = "please Return"
        Dim strParentName As String = "Please return"

        If Not BA_SystemSettings.GenerateAOIOnly Then
            'clip snotel layer
            strInLayerPath = BA_SystemSettings.SNOTELLayer
            strInLayerBareName = BA_GetBareNameAndExtension(strInLayerPath, strParentName, strExtension)

            pStepProg.Message = "Clipping" & strInLayerBareName & " layer... (step 7 of " & nstep & ")"
            pStepProg.Step()
            System.Windows.Forms.Application.DoEvents()

            'clip snotel shapefile
            If strExtension = "(Shapefile)" Then
                response = BA_ClipAOISNOTEL(UserAOIFolderBase, strParentName & "\" & strInLayerBareName, True)
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
                            'MsgBox("No SNOTEL data exists within the AOI. Unable to clip new SNOTEL data to AOI!")
                    End Select
                End If
            End If

            'clip snow course layer
            strInLayerPath = BA_SystemSettings.SCourseLayer
            strInLayerBareName = BA_GetBareNameAndExtension(strInLayerPath, strParentName, strExtension)

            pStepProg.Message = "Clipping " & strInLayerBareName & " layer... (step 8 of " & nstep & ")"
            pStepProg.Step()
            System.Windows.Forms.Application.DoEvents()

            'clip snow course shapefile
            If strExtension = "(Shapefile)" Then
                response = BA_ClipAOISNOTEL(UserAOIFolderBase, strParentName & "\" & strInLayerBareName, False)
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
                            'MsgBox("No Snow course data exists within the AOI. Unable to clip new SNOTEL data to AOI!")
                    End Select
                End If
            End If

            'clip PRISM raster data
            ''''strInLayerPath = frmSettings.txtPRISM.Text
            strInLayerPath = BA_SystemSettings.PRISMFolder
            'remove back slash if exists
            If Microsoft.VisualBasic.Right(strInLayerPath, 1) = "\" Then strInLayerPath = Microsoft.VisualBasic.Left(strInLayerPath, Len(strInLayerPath) - 1)

            'there are 17 prism rasters to be clipped
            BA_SetPRISMFolderNames()
            Dim j As Integer
            For j = 0 To 16
                System.Windows.Forms.Application.DoEvents()
                strInLayerBareName = PRISMLayer(j)
                pStepProg.Message = "Clipping PRISM " & strInLayerBareName & " layer... (step " & j + 9 & " of " & nstep & ")"
                pStepProg.Step()

                response = BA_ClipAOIRaster(UserAOIFolderBase, strInLayerPath & "\" & strInLayerBareName & "\grid", strInLayerBareName, destPRISMGDB, AOIClipFile.PrismClipAOIExtentCoverage)
                If response <= 0 Then
                    MsgBox("frmCreateAOI: Clipping " & strInLayerBareName & "\grid" & " failed! Return value = " & response & ".")
                End If
            Next

            'update the Z unit metadata of PRISM
            'We need to update the depth units if AOI for Basin Analysis was created
            inputFolder = BA_GeodatabasePath(UserAOIFolderBase, GeodatabaseNames.Prism)
            inputFile = AOIPrismFolderNames.annual.ToString

            If rbtnDepthInch.Checked Then
                unitText = BA_EnumDescription(MeasurementUnit.Inches)
            Else
                unitText = BA_EnumDescription(MeasurementUnit.Millimeters)
            End If

            sb.Clear()
            sb.Append(BA_BAGIS_TAG_PREFIX)
            sb.Append(BA_ZUNIT_CATEGORY_TAG & MeasurementUnitType.Depth.ToString & "; ")
            sb.Append(BA_ZUNIT_VALUE_TAG & unitText & ";")
            sb.Append(BA_BAGIS_TAG_SUFFIX)
            BA_UpdateMetadata(inputFolder, inputFile, LayerType.Raster, BA_XPATH_TAGS, _
                              sb.ToString, BA_BAGIS_TAG_PREFIX.Length)
        End If

        'clip other participating layers
        Try
            If ListLayerCount > 0 Then
                For j = 0 To ListLayerCount - 1
                    System.Windows.Forms.Application.DoEvents()
                    strInLayerPath = BA_SystemSettings.OtherLayers(j)
                    strInLayerBareName = BA_GetBareNameAndExtension(strInLayerPath, strParentName, strExtension)

                    pStepProg.Message = "Clipping " & strInLayerBareName & " layer... (step " & j + internalLayerCount + 1 & " of " & nstep & ")"
                    pStepProg.Step()

                    If strExtension = "(Shapefile)" Then
                        If BA_Shapefile_Exists(strParentName & strInLayerBareName) Then
                            response = BA_ClipAOIVector(UserAOIFolderBase, strParentName & strInLayerBareName, strInLayerBareName, _
                                                        destLayersGDB, True)
                        Else
                            MsgBox(strInLayerBareName & " does not exist", "Missing input")
                        End If

                    ElseIf strExtension = "(Raster)" Then
                        If BA_File_ExistsRaster(strParentName, strInLayerBareName) Then
                            response = BA_ClipAOIRaster(UserAOIFolderBase, strParentName & strInLayerBareName, strInLayerBareName, destLayersGDB, AOIClipFile.BufferedAOIExtentCoverage)
                            If response <= 0 Then
                                MsgBox("Clipping " & strInLayerBareName & " failed! Return value = " & response & ".")
                            End If
                        Else
                            MsgBox(strInLayerBareName & " does not exist", "Missing input")
                        End If
                    Else
                        MsgBox(strInLayerBareName & " cannot be clipped.")
                    End If
                Next
            End If

        Catch ex As Exception
            MsgBox("Create AOI Exception: ", ex.Message)

        End Try

        pStepProg.Hide()
        progressDialog2.HideDialog()
        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStepProg)
        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(progressDialog2)

        'copy the basinanalyst.def file to the aoi folder
        'response = BA_CopyBAGISSettings(UserAOIFolderBase)
        If BA_Save_Settings(UserAOIFolderBase & "\" & BA_Settings_Filename) = BA_ReturnCode.Success Then
            MsgBox("AOI was created!")
        Else
            MsgBox("AOI was created but the definition was not copied to the AOI folder!")
        End If

        GC.WaitForPendingFinalizers()
        GC.Collect()
        Me.Close()

    End Sub

    Private Sub frmCreateAOIfromExistingBND_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        Dim response As Integer = 0

        BA_SetSettingPath()

        If Not File.Exists(BA_Settings_Filepath & "\" & BA_Settings_Filename) Then
            MessageBox.Show("Settings file does not exist!" & vbCrLf & "Please go to Options to set settings files", "Missing file!")
            Exit Sub
        End If

        response = BA_ReadBAGISSettings(BA_Settings_Filepath) 'set the system setting parameters

        If response <> 1 Then
            MsgBox("Unable to get critical system settings information. Program stopped!")
            Me.Hide()
            Exit Sub
        End If

        If BA_SystemSettings.DEM10MPreferred Then
            Opt10Meters.Checked = True
        Else
            Opt30Meters.Checked = True
        End If

        If BA_SystemSettings.DEM_ZUnit_IsMeter Then
            lblDEMUnit.Text = BA_EnumDescription(MeasurementUnit.Meters)
        Else
            lblDEMUnit.Text = BA_EnumDescription(MeasurementUnit.Feet)
        End If

        lblSlopeUnit.Text = BA_EnumDescription(SlopeUnit.PctSlope) 'BAGIS generates Slope in Degree

        grpboxPRISMUnit.Visible = Not BA_SystemSettings.GenerateAOIOnly 'when generate AOI only, no PRISM will be clipped to the AOI
    End Sub

    'return value:
    '   0: error occurred
    '   1: AOI attribute updated
    Private Function BA_AddValueFieldtoShapefile(ByVal shapefilePathName As String, FieldName As String, ByVal FieldValue As Integer) As Integer
        Dim return_value As Integer = 0

        Dim comReleaser As ESRI.ArcGIS.ADF.ComReleaser = New ESRI.ArcGIS.ADF.ComReleaser
        ' Open the folder to contain the shapefile as a workspace
        Dim shapefilePath As String = "", shapefileName As String
        shapefileName = BA_GetBareName(shapefilePathName, shapefilePath)
        shapefileName = BA_StandardizeShapefileName(shapefileName, False, False)
        Dim pFClass As IFeatureClass = BA_OpenFeatureClassFromFile(shapefilePath, shapefileName)
        comReleaser.ManageLifetime(pFClass)

        Dim pFCursor As IFeatureCursor = Nothing
        Dim pFeature As IFeature = Nothing

        Dim FieldIndex As Integer
        Dim pFld As IFieldEdit = New Field

        Try
            ' check if field exist
            FieldIndex = pFClass.FindField(FieldName)

            ' Define field type
            If FieldIndex < 0 Then 'add field
                'Define field name
                pFld = New Field
                pFld.Name_2 = FieldName
                pFld.Type_2 = esriFieldType.esriFieldTypeSmallInteger
                pFld.Length_2 = 4
                pFld.Required_2 = 0
                ' Add field
                pFClass.AddField(pFld)
            End If

            'update value
            ' Get field index again
            Dim FI1 As Integer = pFClass.FindField(FieldName)

            pFCursor = pFClass.Update(Nothing, False)
            comReleaser.ManageLifetime(pFCursor)
            pFeature = pFCursor.NextFeature

            Do While Not pFeature Is Nothing
                pFeature.Value(FI1) = FieldValue
                pFCursor.UpdateFeature(pFeature)
                pFeature = pFCursor.NextFeature
                return_value = return_value + 1
            Loop
            Return return_value

        Catch ex As Exception
            MsgBox("BA_AddValueFieldtoShapefile Exception: " & ex.Message)
            Return return_value

        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFld)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeature)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFClass)
            GC.WaitForPendingFinalizers()
            GC.Collect()

        End Try
    End Function


    Public Sub BA_ShapeFile2RasterGDB(ByVal featClass As IFeatureClass, ByVal gdbPath As String, _
                                ByVal FileName As String, ByVal Cellsize As Object, _
                                ByVal valueField As String, ByVal snapRasterPath As String)
        Dim pWS As IWorkspace = Nothing
        Dim pWSFactory As IWorkspaceFactory = New RasterWorkspaceFactory
        Dim pConversionOp As IConversionOp = New RasterConversionOp
        Dim pEnv As IRasterAnalysisEnvironment = Nothing
        Dim pRDS As IRasterDataset = Nothing
        Dim snapGDS As IGeoDataset = Nothing
        Dim envelope As IEnvelope = Nothing
        Dim pFDesc As IFeatureClassDescriptor = New FeatureClassDescriptor
        pFDesc.Create(featClass, Nothing, valueField)
        Dim pGeoDataset As IGeoDataset = CType(featClass, IGeoDataset)

        Try
            'Commenting out check for integer values since we may also convert text values; Field values should be checked
            'before this is called from either BA_Feature2RasterInteger or BA_Feature2RasterDouble
            'If BA_IsIntegerField(featClassDescr, valueField) Then
            Dim hruPath As String = ""
            Dim tmpName As String = BA_GetBareName(gdbPath, hruPath)
            pWS = pWSFactory.OpenFromFile(hruPath, 0)
            pEnv = pConversionOp
            pEnv.SetCellSize(esriRasterEnvSettingEnum.esriRasterEnvValue, Cellsize)
            If Not String.IsNullOrEmpty(snapRasterPath) Then
                Dim snapPath As String = "PleaseReturn"
                Dim snapName As String = BA_GetBareName(snapRasterPath, snapPath)

                Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(snapPath)
                If workspaceType = workspaceType.Geodatabase Then
                    snapGDS = BA_OpenRasterFromGDB(snapPath, snapName)
                ElseIf workspaceType = workspaceType.Raster Then 'input is a GRID
                    snapGDS = BA_OpenRasterFromFile(snapPath, snapName)
                End If

                envelope = pGeoDataset.Extent
                Dim object_Envelope As System.Object = CType(envelope, System.Object) ' Explicit Cast
                pEnv.SetExtent(esriRasterEnvSettingEnum.esriRasterEnvValue, object_Envelope, snapRasterPath)
            End If
            pRDS = pConversionOp.ToRasterDataset(pFDesc, "GRID", pWS, FileName)
            Dim success As Integer = BA_SaveRasterDatasetGDB(pRDS, gdbPath, BA_RASTER_FORMAT, FileName)
            If success = 1 Then
                BA_Remove_Raster(hruPath, FileName)
            End If
            'Else
            'Throw New Exception("Invalid values found in value field. Values must be whole numbers.")
            'End If
        Catch ex As Exception
            MessageBox.Show("BA_ShapeFile2RasterGDB Exception: " + ex.Message)
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFDesc)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pConversionOp)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(snapGDS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(envelope)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pEnv)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRDS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGeoDataset)
        End Try
    End Sub

End Class