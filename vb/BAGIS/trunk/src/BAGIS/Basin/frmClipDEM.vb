Imports ESRI.ArcGIS.Geoprocessing
Imports ESRI.ArcGIS.esriSystem
Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Geodatabase
Imports System.Windows.Forms
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports System.IO
Imports ESRI.ArcGIS.DataSourcesFile
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Desktop.AddIns
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.DataSourcesGDB
Imports System.Text

Public Class frmClipDEMtoAOI

    Private Sub CmbSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbSelectAll.Click
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

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        SetAllCheckBoxes(False)
    End Sub

    Private Sub CmbRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbRun.Click
        Dim nstep As Integer

        'verify filter size parameters
        If Val(txtHeight.Text) <= 0 Or Val(txtWidth.Text) <= 0 Then
            MsgBox("Invalid filter size! Please reenter.")
            Exit Sub
        End If

        nstep = 13 'step counter for frmmessage

        Dim response As Short = 0
        Dim strDEMDataSet As String
        Dim strDEMText As String
        Dim setDEMExtenttool = AddIn.FromID(Of setDEMExtenttool)(My.ThisAddIn.IDs.setDEMExtenttool)
        Dim clipDEMButton = AddIn.FromID(Of BtnClipDEM)(My.ThisAddIn.IDs.BtnClipDEM)
        Dim selectAOIButton = AddIn.FromID(Of BtnAOI_Tool)(My.ThisAddIn.IDs.BtnAOI_Tool)
        Dim basinInfoButton = AddIn.FromID(Of BtnBasinInfo)(My.ThisAddIn.IDs.BtnBasinInfo)

        Me.Hide()

        If Opt10Meters.Checked Then
            'MsgBox "10 meters DEM is clipped to the basin folder!"
            strDEMDataSet = BA_SystemSettings.DEM10M
            If Len(strDEMDataSet) = 0 Then
                MsgBox("The source of 10 meters DEM is not specified in the setting!")
                Exit Sub
            End If
            strDEMText = "10 meters DEM"
        Else
            'MsgBox "30 meters DEM is clipped to the basin folder!"
            strDEMDataSet = BA_SystemSettings.DEM30M
            If Len(strDEMDataSet) = 0 Then
                MsgBox("The source of 30 meters DEM is not specified in the setting!")
                Exit Sub
            End If
            strDEMText = "30 meters DEM"
        End If

        'check if the Surfaces GDB exists, if so delete the GDB, otherwise, create it
        Dim sourceDEMPath As String = ""
        Dim sourceDEMName As String = BA_GetBareName(strDEMDataSet, sourceDEMPath)
        Dim sourceAOIGDB As String = BasinFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Aoi)
        Dim destSurfGDB As String = BasinFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces)
        Dim destAOIGDB As String = BasinFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Aoi)

        If BA_Workspace_Exists(destSurfGDB) Then
            'Delete gdb if it exists
            If BA_DeleteGeodatabase(destSurfGDB, My.ArcMap.Document) <> BA_ReturnCode.Success Then
                MessageBox.Show("Unable to delete Geodatabase '" & destSurfGDB & "'. Please restart ArcMap and try again", "Unable to delete Geodatabase", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
        End If

        'create the Surfaces Geodatabase
        Dim surfssuccess As BA_ReturnCode = BA_CreateFileGdb(BasinFolderBase, BA_EnumDescription(GeodatabaseNames.Surfaces))
        If surfssuccess <> BA_ReturnCode.Success Then
            MsgBox("unable to creat gdb! Please chech disk space.")
            Exit Sub
        End If

        'Checks if the aoi gdb exists delete it
        If BA_Workspace_Exists(destAOIGDB) Then
            'response = BA_RemoveLayersInFolder(My.ArcMap.Document, aoigdbpath)  the BA_DeleteGeodatabase sub has this routine embedded.
            If BA_DeleteGeodatabase(destAOIGDB, My.ArcMap.Document) <> BA_ReturnCode.Success Then
                MessageBox.Show("Unable to delete Geodatabase '" & destAOIGDB & "'. Please restart ArcMap and try again", "Unable to delete Geodatabase", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                clipDEMButton.selectedProperty = False
                Exit Sub
            End If
        End If

        'create aoi Geodatabase
        Dim aoisuccess As BA_ReturnCode = BA_CreateFileGdb(BasinFolderBase, BA_EnumDescription(GeodatabaseNames.Aoi))
        If aoisuccess <> BA_ReturnCode.Success Then
            MsgBox("Unable to create gdb! Please check disk space.")
            Exit Sub
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
        progressDialog2 = BA_GetProgressDialog(pStepProg, "Clipping DEM to Basin Folder ", "Clipping...")
        pStepProg.Show()
        progressDialog2.ShowDialog()
        pStepProg.Message = "Preparing for clipping..."

        Dim inputraster As String

        ' Set Workspace to Parent Path
        Dim pDEM As IGeoDataset2 = BA_OpenRasterFromFile(sourceDEMPath, sourceDEMName)
        'Dim pRasterDataset As IRasterDataset2 = New RasterDataset
        'Dim pRasterBand As IRasterBand = Nothing
        'Dim pRasterBandCollection As IRasterBandCollection = Nothing

        Try
            'save the dem, dem extent, and the dem info file
            'response = BA_Create_FolderType_File(BasinFolderBase, BA_Basin_Type, strDEMText)
            response = BA_Graphic2FeatureClass(destAOIGDB, BA_DEMExtentShapefile)
            System.Windows.Forms.Application.DoEvents()

            If response = 0 Then 'error when creating the shapefile
                pStepProg.Hide()
                progressDialog2.HideDialog()
                MsgBox("Unable to save the DEM extent file! Please check disk space.")
                GoTo AbandonClipDEM
            End If

            'update the attribute table of the AOI using basin name
            response = BA_AddAOIVectorAttributes(destAOIGDB, BA_GetBareName(BasinFolderBase))
            System.Windows.Forms.Application.DoEvents()

            'Display DEM Extent layer
            If ChkDEMExtent.Enabled Then
                Dim strPathandName As String = destAOIGDB & "\" & BA_DEMExtentShapefile
                Dim pDColor As IRgbColor = pDisplayColor
                Dim mysuccess As BA_ReturnCode = BA_AddExtentLayer(My.ArcMap.Document, strPathandName, pDColor) ' BasinLayerDisplayNames(1), 0, 2)
                If mysuccess <> BA_ReturnCode.Success Then
                    pStepProg.Hide()
                    progressDialog2.HideDialog()
                    MsgBox("Unable to add the extent layer", "Unknown error")
                End If
            End If

            ClearMap() 'clear graphic container
            System.Windows.Forms.Application.DoEvents()

            'clip DEM then save it
            pStepProg.Message = "Clipping DEM... (step 1 of " & nstep & ")"
            pStepProg.Step()

            inputraster = sourceDEMPath & "\" & sourceDEMName

            If ChkSmoothDEM.Checked Then
                response = BA_ClipAOIRaster(BasinFolderBase, inputraster, "tempdem", destSurfGDB, AOIClipFile.AOIExtentCoverage, False)
                Dim ptempDEM As IGeoDataset2 = BA_OpenRasterFromGDB(destSurfGDB, "tempdem")
                pClippedDEM = Smooth(ptempDEM, Val(txtHeight.Text), Val(txtWidth.Text))
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(ptempDEM)

                If pClippedDEM Is Nothing Then 'no dem within the selected area
                    pStepProg.Hide()
                    progressDialog2.HideDialog()
                    MsgBox("Unable to perform smoothing on the selected DEM!")
                    GoTo AbandonClipDEM
                End If

                response = BA_SaveRasterDatasetGDB(pClippedDEM, destSurfGDB, BA_RASTER_FORMAT, BA_EnumDescription(MapsFileName.dem_gdb))
                If response = 0 Then
                    pStepProg.Hide()
                    progressDialog2.HideDialog()
                    MsgBox("Unable to save CLIPPED DEM to Surfaces GDB!", "Warning")
                    GoTo AbandonClipDEM
                End If

                BA_ComputeStatsRasterDatasetGDB(destSurfGDB, BA_EnumDescription(MapsFileName.dem_gdb))
            Else
                response = BA_ClipAOIRaster(BasinFolderBase, inputraster, BA_EnumDescription(MapsFileName.dem_gdb), destSurfGDB, AOIClipFile.AOIExtentCoverage, False)
            End If

            If response <= 0 Then
                pStepProg.Hide()
                progressDialog2.HideDialog()
                MsgBox("There is no DEM within the specified area! Please check your DEM source data.")
                clipDEMButton.selectedProperty = False
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDEM)
                GoTo AbandonClipDEM
            End If

            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDEM)


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
                GoTo AbandonClipDEM
            End If

            'compute stats of the raster
            'pRasterDataset = CType(pFilledDEM, IRasterDataset2) ' Explicit cast
            'pRasterBandCollection = pRasterDataset
            'pRasterBand = pRasterBandCollection.Item(0)
            'pRasterBand.ComputeStatsAndHist()
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBand)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBandCollection)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterDataset)

            'save filled DEM into surfaces GDB
            pStepProg.Message = "Saving Filled DEM... (step 3 of " & nstep & ")"
            pStepProg.Step()
            System.Windows.Forms.Application.DoEvents()

            response = BA_SaveRasterDatasetGDB(pFilledDEM, destSurfGDB, BA_RASTER_FORMAT, BA_EnumDescription(MapsFileName.filled_dem_gdb))
            If response = 0 Then
                pStepProg.Hide()
                progressDialog2.HideDialog()
                MsgBox("Unable to save FILLED DEM to Surfaces GDB!", "Warning")
                GoTo AbandonClipDEM
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
                GoTo AbandonClipDEM
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
                GoTo AbandonClipDEM
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
                GoTo AbandonClipDEM
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
                GoTo AbandonClipDEM
            End If
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFlowDir)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFlowAcc)

            BA_ComputeStatsRasterDatasetGDB(destSurfGDB, BA_EnumDescription(MapsFileName.flow_accumulation_gdb))

            'display Flow Accumulation
            If ChkFlowAccum.Checked = True Then
                response = BA_DisplayRaster(My.ArcMap.Application, destSurfGDB & "\" & BA_EnumDescription(MapsFileName.flow_accumulation_gdb)) ', BasinLayerDisplayNames(6))
            End If
            System.Windows.Forms.Application.DoEvents()


            '=====================================================================================================================
            'generate and save Hillshade
            '=====================================================================================================================
            pStepProg.Message = "Calculating Hillshade... (step 12 of " & nstep & ")"
            pStepProg.Step()
            System.Windows.Forms.Application.DoEvents()

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
                GoTo AbandonClipDEM
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

            Dim inputFolder As String
            Dim inputFile As String
            Dim unitText As String

            'We need to update the elevation units
            inputFolder = BA_GeodatabasePath(BasinFolderBase, GeodatabaseNames.Surfaces)
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
            inputFolder = BA_GeodatabasePath(BasinFolderBase, GeodatabaseNames.Surfaces)
            inputFile = BA_GetBareName(BA_EnumDescription(PublicPath.Slope))
            sb.Clear()
            sb.Append(BA_BAGIS_TAG_PREFIX)
            sb.Append(BA_ZUNIT_CATEGORY_TAG & MeasurementUnitType.Slope.ToString & "; ")
            sb.Append(BA_ZUNIT_VALUE_TAG & BA_EnumDescription(SlopeUnit.PctSlope) & ";")
            sb.Append(BA_BAGIS_TAG_SUFFIX)
            BA_UpdateMetadata(inputFolder, inputFile, LayerType.Raster, BA_XPATH_TAGS, _
                              sb.ToString, BA_BAGIS_TAG_PREFIX.Length)

            'if DEM is successfully clipped then disable the DEM clip tools and enable the AOI_tool
            'declare the property value of buttons in the main toolbar
            setDEMExtenttool.selectedProperty = False
            clipDEMButton.selectedProperty = False
            selectAOIButton.selectedProperty = True
            basinInfoButton.selectedProperty = True

            'set the value of cboSelectedBasin on the main toolbar
            Dim cboSelectedbasin = AddIn.FromID(Of cboTargetedBasin)(My.ThisAddIn.IDs.cboTargetedBasin)
            cboSelectedbasin.setValue(BA_GetBareName(BasinFolderBase))

            pStepProg.Hide()
            progressDialog2.HideDialog()
            MsgBox("Basin Tool finished! Basin " & (BA_GetBareName(BasinFolderBase)) & " is created!")
AbandonClipDEM:
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

        Finally

            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStepProg)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(progressDialog2)
            'pDEUtility.ReleaseInternals()

            GC.WaitForPendingFinalizers()
            GC.Collect()

        End Try
        Me.Close()
    End Sub

    Private Sub frmClipDEM_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If BA_SystemSettings.DEM10MPreferred Then
            Opt10Meters.Checked = True
        Else
            Opt30Meters.Checked = True
        End If
    End Sub

    Public Function OpenRasterDataset(ByVal path As String, ByVal name As String) As IRasterDataset
        'this example opens a raster from a raster format
        Try
            Dim wsFactory As IWorkspaceFactory = New RasterWorkspaceFactory
            Dim ws As IRasterWorkspace = CType(wsFactory.OpenFromFile(path, 0), IRasterWorkspace)
            Dim rasterDataset As IRasterDataset = ws.OpenRasterDataset(name)
            Return rasterDataset
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine(ex.Message)
            Return Nothing
        End Try
    End Function

    'Public Function DeleteRasterDataset(ByVal rasterdataset As IRasterDataset3) As Short
    '    Dim retValue As Short
    '    Try
    '        Dim pDataSet As IDataset = rasterdataset
    '        pDataSet.Delete()
    '        retValue = 1

    '    Catch ex As Exception
    '        MsgBox("Error", ex.Message)
    '        retValue = 0
    '    End Try
    '    Return retValue
    'End Function

    Private Sub ChkSmoothDEM_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ChkSmoothDEM.CheckedChanged
        GrpBoxFilter.Enabled = ChkSmoothDEM.Checked
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
End Class