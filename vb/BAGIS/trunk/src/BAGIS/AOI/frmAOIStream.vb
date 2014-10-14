Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.SpatialAnalyst
Imports ESRI.ArcGIS.GeoAnalyst
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.Framework

Public Class frmAOIStream

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'validate setting
        If Len(AOIFolderBase) = 0 Then
            MsgBox("Please select an AOI first!")
            Me.Close()
        End If

        'get flow accumulation stats
        Dim pRasterStats As IRasterStatistics = Nothing
        Try

            Dim raster_res As Double
            Dim parentFolder As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces, True)
            pRasterStats = BA_GetRasterStatsGDB(parentFolder & BA_EnumDescription(MapsFileName.flow_accumulation_gdb), raster_res)

            If Not pRasterStats Is Nothing Then
                txtMax.Text = pRasterStats.Maximum
                txtSTDV.Text = Format(pRasterStats.StandardDeviation, "#0")
                pRasterStats = Nothing
                txtThresholdSTDV.Text = 1
                OptStdv.Checked = True
            Else
                MsgBox("Unable to read the statistics of the flow accumulation layer!")
            End If
        Catch ex As Exception
            Debug.Print("New() exception: " & ex.Message)
        Finally
            pRasterStats = Nothing
        End Try

    End Sub

    Private Sub CmdCreateStream_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdCreateStream.Click
        Dim ThresholdV As Double
        Dim response As Integer

        'determine threshold value
        If OptStdv.Enabled Then
            ThresholdV = txtThresholdSTDV.Text * txtSTDV.Text
        Else
            ThresholdV = Val(txtThresholdFAcc.Text)
        End If

        If ThresholdV <= 0 Then 'invalid input
            MsgBox("Invalid input! Reset to default value.")
            txtThresholdSTDV.Text = 1
            ThresholdV = txtThresholdSTDV.Text * txtSTDV.Text
        End If

        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim pProgD As IProgressDialog2 = BA_GetAnimationProgressor(My.ArcMap.Application.hWnd, "Generating stream lines...", "AOI Stream Tool")

        Dim pAOIRaster As IGeoDataset
        Dim pFAccGDS As IGeoDataset
        Dim pFDirGDS As IGeoDataset
        Dim pFAccRasterDS As IRasterDataset
        Dim pFAccRaster As IRaster
        Dim pTempRaster As IGeoDataset
        Dim pExtractOp As IExtractionOp2 = New RasterExtractionOp
        Dim pQFilter As IQueryFilter = New QueryFilter
        Dim pRasDes As IRasterDescriptor = New RasterDescriptor
        Dim pStreamGDS As IGeoDataset
        Dim pHydrologyOp As IHydrologyOp = New RasterHydrologyOp
        Dim pStreamlinkGDS As IGeoDataset
        Dim pLColor As IColor = New RgbColor

        Try
            'Use the AOI extent to limit the process extent
            'Open AOI Polygon to set the analysis mask
            Dim parentPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi)
            Dim fileName As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.AoiGrid), False)
            pAOIRaster = BA_OpenRasterFromGDB(parentPath, fileName)
            If pAOIRaster Is Nothing Then
                MsgBox("Cannot locate AOI boundary raster in the AOI.  Please re-run AOI Tool.")
                Exit Sub
            End If

            'open flow_accum and flow direction raster
            Dim surfacesGDB As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces)
            pFAccGDS = BA_OpenRasterFromGDB(surfacesGDB, BA_EnumDescription(MapsFileName.flow_accumulation_gdb))
            pFAccRasterDS = CType(pFAccGDS, IRasterDataset) 'qi
            pFAccRaster = pFAccRasterDS.CreateDefaultRaster
            pFDirGDS = BA_OpenRasterFromGDB(surfacesGDB, BA_EnumDescription(MapsFileName.flow_direction_gdb))

            'Run Extraction Operation
            pTempRaster = pExtractOp.Raster(pFAccRaster, pAOIRaster)
            pAOIRaster = Nothing

            'Query the Raster and Create a new dataset
            pQFilter.WhereClause = "Value > " & ThresholdV
            pRasDes.Create(pTempRaster, pQFilter, "Value")
            pStreamGDS = pExtractOp.Attribute(pRasDes)

            pStreamlinkGDS = pHydrologyOp.StreamLink(pStreamGDS, pFDirGDS)

            'save output shapefile
            Dim shapefpath As String, shapefname As String
            shapefpath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers)
            shapefname = BA_AOIStreamShapefile

            'Temporarily save the streamlink raster so that we can convert it to a feature class later
            Dim tempShapeFile As String = "aoi_streams.shp"

            'check if outputs exist
            Dim outputFeaturePath As String = shapefpath & "\" & shapefname
            If BA_File_Exists(outputFeaturePath, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                response = BA_RemoveLayersInFolder(My.Document, shapefpath)
                response = BA_Remove_ShapefileFromGDB(shapefpath, shapefname)
            End If
            If BA_File_ExistsWindowsIO(AOIFolderBase & "\" & tempShapeFile) Then
                response = BA_Remove_Shapefile(AOIFolderBase, BA_StandardizeShapefileName(tempShapeFile, False))
            End If

            'Create line shapefile at AOI root; There is a bug with this function; It can only create shapefiles
            'Doesn't work with file geodatabase
            Dim retVal As BA_ReturnCode = BA_Raster2LineShapefile(pStreamlinkGDS, AOIFolderBase, tempShapeFile)
            If retVal <> BA_ReturnCode.Success Then
                MsgBox("Unable to create the output stream shapefile.")
                Me.Close()
            End If

            'Copy the temporary line shape file to the layers.gdb
            retVal = BA_ConvertShapeFileToGDB(AOIFolderBase, tempShapeFile, shapefpath, shapefname)
            'Delete original line shapefile. Due the filelock issue if FGDB, the shapefile is used for display purpose.
            'BA_Remove_Shapefile(AOIFolderBase, BA_StandardizeShapefileName(tempShapeFile, False))

            'Add to Map
            BA_ToggleView(My.Document, True)

            'set line color

            pLColor.RGB = RGB(0, 0, 255)
            response = BA_AddLineLayer(My.Document, AOIFolderBase & "\" & tempShapeFile, BA_MapStream, pLColor, 0)

            Me.Close()
        Catch ex As Exception
            Debug.Print("CmdCreateStream_Click Exception: " & ex.Message)
            MsgBox("Creating AOI stream layer failed!")
            Me.Close()
        Finally
            pHydrologyOp = Nothing
            pRasDes = Nothing
            pQFilter = Nothing
            pExtractOp = Nothing
            pFAccGDS = Nothing
            pFAccRasterDS = Nothing
            pFAccRaster = Nothing
            pFDirGDS = Nothing
            pStreamGDS = Nothing
            pStreamlinkGDS = Nothing
            pTempRaster = Nothing
            pLColor = Nothing

            pProgD.HideDialog()
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pProgD)
        End Try
    End Sub

    Private Sub CmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdCancel.Click
        Me.Close()
    End Sub

    Private Sub OptFlowAcc_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles OptFlowAcc.CheckedChanged
        txtThresholdSTDV.Enabled = False
        txtThresholdFAcc.Enabled = True
    End Sub


    Private Sub OptStdv_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles OptStdv.CheckedChanged
        txtThresholdSTDV.Enabled = True
        txtThresholdFAcc.Enabled = False
    End Sub

End Class