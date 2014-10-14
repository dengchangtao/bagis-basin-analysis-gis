Imports ESRI.ArcGIS.Geoprocessing
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework
Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.DataSourcesFile
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.GeoAnalyst
Imports ESRI.ArcGIS.SpatialAnalyst
Imports System.Windows.Forms
Imports ESRI.ArcGIS.Desktop.AddIns
Imports System.IO
Imports ESRI.ArcGIS.DataSourcesGDB
Imports System.ComponentModel
Imports System.Text

Public Class frmCreateAOI
    Private Sub CmbRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbRun.Click
        Me.Hide()
        Dim nstep As Integer
        Dim ListLayerCount As Integer
        'ListLayerCount = frmSettings.lstLayers.listcount

        Dim comReleaser As ESRI.ArcGIS.ADF.ComReleaser = New ESRI.ArcGIS.ADF.ComReleaser()
        'Dim pDEUtility As IDEUtilities = New DEUtilities
        BA_SetSettingPath()
        BA_ReadBAGISSettings(BA_Settings_Filepath)
        ListLayerCount = BA_SystemSettings.listCount

        Dim internalLayerCount As Integer = 0

        If BA_SystemSettings.GenerateAOIOnly Then
            internalLayerCount = 6
        Else
            internalLayerCount = 25
        End If

        nstep = internalLayerCount + ListLayerCount 'step counter for frmmessage

        Dim cboSelectedAOI = AddIn.FromID(Of cboTargetedAOI)(My.ThisAddIn.IDs.cboTargetedAOI)
        Dim AOIName As String = cboSelectedAOI.getValue()

        AOIFolderBase = BasinFolderBase & "\" & AOIName

        Dim sourceSurfGDB As String = BasinFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces)
        Dim sourceAOIGDB As String = BasinFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Aoi)
        Dim destSurfGDB As String = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces)
        Dim destAOIGDB As String = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Aoi)
        Dim destLayersGDB As String = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Layers)
        Dim destPRISMGDB As String = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Prism)

        Dim pProgD As IProgressDialog2 = BA_GetAnimationProgressor(My.ArcMap.Application.hWnd, "Initializing process...", "Creating AOI")
        System.Windows.Forms.Application.DoEvents()

        'The BA_Create_Output_Folders function can delete the file structure if it exists
        Dim response As Integer
        'response = BA_Create_Output_Folders(AOIFolderBase, True)
        For Each pName In [Enum].GetValues(GetType(GeodatabaseNames))
            Dim EnumConstant As [Enum] = pName
            Dim fi As Reflection.FieldInfo = EnumConstant.GetType().GetField(EnumConstant.ToString())
            Dim aattr() As DescriptionAttribute = _
                DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
            Dim gdbName As String = aattr(0).Description
            Dim gdbpath As String = AOIFolderBase & "\" & gdbName

            If BA_Workspace_Exists(gdbpath) Then
                If BA_DeleteGeodatabase(AOIFolderBase & "\" & gdbName, My.ArcMap.Document) <> BA_ReturnCode.Success Then
                    MsgBox("Cannot delete existing Geodatabase folders", "Unknown Error")
                End If
            End If
        Next

        Dim success As BA_ReturnCode = BA_CreateGeodatabaseFolders(AOIFolderBase, FolderType.AOI)

        If success <> BA_ReturnCode.Success Then
            MsgBox("Unable to create GDBs! Please check disk space")
            pProgD.HideDialog()
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pProgD)
            Exit Sub
        End If

        pProgD.Description = "Creating GDB Folders"
        System.Windows.Forms.Application.DoEvents()

        'create maps folder to save MXDs
        Dim mappath As String = AOIFolderBase & "\maps"
        If Not BA_Workspace_Exists(mappath) Then
            mappath = BA_CreateFolder(AOIFolderBase, "maps")
        End If

        'set pourpoint filename and save pourpoint as a shapefile
        Dim unsnappedppname As String
        If ChkSnapPP.Checked Then
            unsnappedppname = "unsnappedpp"
        Else
            unsnappedppname = BA_POURPOINTCoverage
        End If

        If BA_Graphic2FeatureClass(destAOIGDB, unsnappedppname) <> 1 Then
            MsgBox("Unable to save Pour Point")
        End If

        'Load unsnapped pourpoint
        Dim pPF As IFeatureClass = BA_OpenFeatureClassFromGDB(destAOIGDB, unsnappedppname)
        Dim pPSource As IGeoDataset = pPF
        'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pPF)

        Dim pSnapPourPoint As IGeoDataset = Nothing
        Dim strSnapName As String 'name of the saved pourpoint
        strSnapName = BA_POURPOINTCoverage

        'Load Flow Direction
        Dim pDirection As IGeoDataset = BA_OpenRasterFromGDB(sourceSurfGDB, BA_EnumDescription(MapsFileName.flow_direction_gdb))

        Dim pWatershed As IGeoDataset = Nothing
        pProgD.Description = "Delineating AOI Boundaries..."
        System.Windows.Forms.Application.DoEvents()

        Dim pHydrologyOp As IHydrologyOp = New RasterHydrologyOp
        Dim pExtractOp As IExtractionOp2 = New RasterExtractionOp

        If ChkSnapPP.Enabled Then 'snap the pourpoint
            Dim SnapDistance As Double
            SnapDistance = Val(txtSnapD.Text)

            'Load Flow Accumulation
            Dim pAccum As IGeoDataset = BA_OpenRasterFromGDB(sourceSurfGDB, BA_EnumDescription(MapsFileName.flow_accumulation_gdb))

            'Snap Pour Point
            pSnapPourPoint = pHydrologyOp.SnapPourPoint(pPSource, pAccum, SnapDistance) ' ### note that this is in DD not Meters

            'Save the snapped pourpoint
            'Query the Previous Raster to Include only the PP location
            'Set QFilter to 0 (Pour Point Value)
            Dim pQFilter As IQueryFilter = New QueryFilter
            pQFilter.WhereClause = "VALUE = 0"
            Dim pRasDes As IRasterDescriptor = New RasterDescriptor
            pRasDes.Create(pSnapPourPoint, pQFilter, "VALUE")

            'Run an Extraction Operation
            Dim pSourceDS As IGeoDataset = pExtractOp.Attribute(pRasDes)

            ' Create Feature from Raster
            Dim pWSF As IWorkspaceFactory = New FileGDBWorkspaceFactory
            Dim pFWS As IWorkspace2 = pWSF.OpenFromFile(destAOIGDB, 0)

            Dim pConversionOp As IConversionOp = New RasterConversionOp
            Dim pSnapGDS As IGeoDataset = pConversionOp.RasterDataToPointFeatureData(pSourceDS, pFWS, strSnapName)
            pWatershed = pHydrologyOp.Watershed(pDirection, pSnapPourPoint)

            'release memory
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pQFilter)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasDes)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pAccum)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pSourceDS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pConversionOp)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFWS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWSF)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pSnapGDS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pSnapPourPoint)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pExtractOp)
        Else
            pWatershed = pHydrologyOp.Watershed(pDirection, pPSource)

        End If

        'release memory
        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pHydrologyOp)
        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDirection)
        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pPSource)
        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pPF)

        'Add pourpoint to Map
        Dim pMColor As IRgbColor = New RgbColor
        pMColor.RGB = RGB(0, 255, 255)

        If BA_File_Exists(destAOIGDB & "\" & BA_POURPOINTCoverage, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            response = BA_MapDisplayPointMarkers(My.ArcMap.Application, destAOIGDB & "\" & BA_POURPOINTCoverage, MapsLayerName.Pourpoint, pMColor, MapsMarkerType.Pourpoint)
        End If

        pProgD.Description = "Saving AOI Boundaries..."
        System.Windows.Forms.Application.DoEvents()

        'Workaround for bug when saving output of pHydrologyOp directly to a File GDB
        'We work with the regular raster file for aoibagis until the end of this subroutine when we copy it to aoi.gdb
        response = BA_SaveRasterDataset(pWatershed, AOIFolderBase, BA_AOIExtentRaster)
        pWatershed = Nothing

        Dim DisplayName As String
        Dim comboBox = AddIn.FromID(Of cboTargetedAOI)(My.ThisAddIn.IDs.cboTargetedAOI)
        DisplayName = "AOI " & comboBox.getValue

        'Convert watershed to polygon
        'open aoibagis raster and convert it to watershed polygon
        pWatershed = BA_OpenRasterFromFile(AOIFolderBase, BA_AOIExtentRaster)
        'convert aoibagis watershed raster to polygon 
        response = BA_Raster2PolygonShapefile(destAOIGDB, BA_AOIExtentCoverage, pWatershed)
        pWatershed = Nothing

        'update the attribute table of the AOI using basin name
        response = BA_AddAOIVectorAttributes(destAOIGDB, comboBox.getValue())

        'add AOI extent (aoi_v) to map
        Dim pColor As IRgbColor = pDisplayColor
        response = BA_AddExtentLayer(My.ArcMap.Document, destAOIGDB & "\" & BA_AOIExtentCoverage, pColor, DisplayName, 0, 2)
        If response < 0 Then 'error occurred
            pProgD.HideDialog()
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pProgD)
            MsgBox("Unable to add AOI polygon to ArcMap! Program stopped.")

            'BA_DeleteGeodatabase(AOIFolderBase, My.ArcMap.Document)
            'If BA_Workspace_Exists(AOIFolderBase) Then 'cannot delete the folder
            '    MsgBox("Cannot delete the AOI! It's probably caused by a file lock in the Geodatabase. Please restart ArcGIS and repeat the process.")
            'End If

            Me.Close()
            Exit Sub
        End If

        'get AOI area and prompt if the user wants to continue
        Dim AOIArea As Double, AOIArea_String As String
        AOIArea = BA_GetShapeArea(destAOIGDB & "\" & BA_AOIExtentCoverage) / 1000000 'the shape unit is in sq meters, converted to sq km

        If AOIArea < 0 Then 'error when getting the area of aoi
            pProgD.HideDialog()
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pProgD)
            MsgBox("Unable to get the area of the AOI! Program stopped.")

            Me.Close()
            Exit Sub
        End If

        If AOIArea < BA_MinimumAOIArea Then
            pProgD.HideDialog()
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pProgD)
            MsgBox("The size of the AOI is too small!" & Chr(13) & Chr(10) & "Please select a new pour point location or use the auto snapping option.")

            Me.Close()
            Exit Sub
        End If

        AOI_ReferenceArea = Format(AOI_ReferenceArea, "#0.00")
        Dim tempRefArea As String = CStr(AOI_ReferenceArea)
        If UCase(BA_SystemSettings.PourAreaField) = "NO DATA" Or AOI_ReferenceArea = 0 Then
            tempRefArea = "Not specified"
        End If

        Dim tempAreaUnit As String = BA_SystemSettings.PourAreaUnit
        If UCase(tempAreaUnit) = "UNKNOWN" Or AOI_ReferenceArea = 0 Then
            tempAreaUnit = ""
        End If

        AOIArea_String = "The area of AOI is:"
        AOIArea_String = AOIArea_String & Chr(13) & Chr(10) & Chr(9) & " " & Format(AOIArea, "#0.00") & Chr(9) & " Square Km"
        AOIArea_String = AOIArea_String & Chr(13) & Chr(10) & Chr(9) & " " & Format(AOIArea * 247.1044, "#0.00") & Chr(9) & " Acre"
        AOIArea_String = AOIArea_String & Chr(13) & Chr(10) & Chr(9) & " " & Format(AOIArea * 0.3861022, "#0.00") & Chr(9) & " Square Miles"
        AOIArea_String = AOIArea_String & Chr(13) & Chr(10) & Chr(9) & " " & Chr(10) & "Reference Area for AOI is: "
        AOIArea_String = AOIArea_String & Chr(13) & Chr(10) & Chr(9) & " " & tempRefArea & Chr(9) & " " & tempAreaUnit
        AOIArea_String = AOIArea_String & Chr(13) & Chr(10) & Chr(9) & " " & Chr(10) & "Do you want to use this AOI boundary?"

        pProgD.HideDialog()
        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pProgD)

        response = MsgBox(AOIArea_String, vbYesNo)

        If response = vbNo Then 'user abandon the process
            Me.Close()
            Exit Sub
        End If

        'update the area information in the pourpoint shapefile
        AOI_ShapeArea = AOIArea
        AOI_ShapeUnit = "Square Km"
        AOI_ReferenceUnit = BA_SystemSettings.PourAreaUnit
        response = BA_UpdatePPAttributes(destAOIGDB)

        Dim Distance As Double

        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, nstep)
        Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Creating AOI...", "Creating AOI")
        System.Windows.Forms.Application.DoEvents()

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
            .out_feature_class = AOIFolderBase & "\" & BA_PRISMClipAOI & ".shp"
        End With
        GP.AddOutputsToMap = False
        GP.Execute(BufferTool, Nothing)

        'save the buffered AOI as a shapefile and then import it into the GDB
        'to prevent a bug when the buffer distance exceed the xy domain limits of the GDB
        'response = BA_Graphic2Shapefile(AOIFolderBase, BA_PRISMClipAOI)

        'Copy the temporary line shape file to the aoi.gdb
        Dim retVal As BA_ReturnCode = BA_ReturnCode.UnknownError
        retVal = BA_ConvertShapeFileToGDB(AOIFolderBase, BA_StandardizeShapefileName(BA_PRISMClipAOI, True, False), destAOIGDB, BA_PRISMClipAOI)
        'create a raster version of the buffered AOI
        Dim Cellsize As Double = 0
        Dim fullLayerPath As String = AOIFolderBase & "\" & BA_AOIExtentRaster
        'Dim rasterStat As IRasterStatistics = BA_GetRasterStatsGDB(fullLayerPath, Cellsize)
        Dim rasterStat As IRasterStatistics = BA_GetRasterStats(fullLayerPath, Cellsize)
        retVal = BA_Feature2RasterGP(AOIFolderBase & BA_StandardizeShapefileName(BA_PRISMClipAOI, True, True), destAOIGDB & BA_EnumDescription(PublicPath.AoiPrismGrid), "ID", Cellsize, fullLayerPath)
        BA_Remove_Shapefile(AOIFolderBase, BA_StandardizeShapefileName(BA_PRISMClipAOI, False))

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
            .out_feature_class = AOIFolderBase & "\" & BA_BufferedAOIExtentCoverage & ".shp"
        End With
        GP.AddOutputsToMap = False
        GP.Execute(BufferTool, Nothing)

        BufferTool = Nothing
        GP = Nothing

        'response = BA_Graphic2Shapefile(AOIFolderBase, BA_BufferedAOIExtentCoverage)
        retVal = BA_ConvertShapeFileToGDB(AOIFolderBase, BA_StandardizeShapefileName(BA_BufferedAOIExtentCoverage, True, False), destAOIGDB, BA_BufferedAOIExtentCoverage)
        retVal = BA_Feature2RasterGP(AOIFolderBase & BA_StandardizeShapefileName(BA_BufferedAOIExtentCoverage, True, True), destAOIGDB & BA_EnumDescription(PublicPath.AoiBufferedGrid), "ID", Cellsize, fullLayerPath)
        BA_Remove_Shapefile(AOIFolderBase, BA_StandardizeShapefileName(BA_BufferedAOIExtentCoverage, False))

        '=========================
        'start the clipping preparation
        '=========================
        'pStepProg.Show()
        pStepProg.Message = "Clipping DEM layer... (step 1 of " & nstep & ")"
        pStepProg.Step()
        System.Windows.Forms.Application.DoEvents()

        Dim inputraster As String
        Dim DemFilePath As String
        DemFilePath = sourceSurfGDB & "\" & BA_EnumDescription(MapsFileName.filled_dem_gdb)
        If Not BA_File_Exists(DemFilePath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            pStepProg.Hide()
            progressDialog2.HideDialog()
            pStepProg = Nothing
            progressDialog2 = Nothing
            MsgBox("Source dem was not found. DEM statistics cannot be obtained!" & vbCrLf & "Clipping was faield", "Missing Data")

            BA_DeleteGeodatabase(AOIFolderBase, My.ArcMap.Document)
            If BA_Workspace_Exists(AOIFolderBase) Then 'cannot delete the folder
                MsgBox("Cannot delete the AOI! It's probably caused by a file lock in the Geodatabase. Please restart ArcGIS and repeat the process.")
            End If

            Me.Close()
            Exit Sub
        End If

        Dim pRasterStats As IRasterStatistics = BA_GetRasterStatsGDB(DemFilePath, Cellsize)
        pRasterStats = Nothing

        If Cellsize = 0 Then 'error reading raster dem file
            pStepProg.Hide()
            progressDialog2.HideDialog()
            pStepProg = Nothing
            progressDialog2 = Nothing
            MsgBox("Unable to read basin DEM information. Program stopped.")

            BA_DeleteGeodatabase(AOIFolderBase, My.ArcMap.Document)
            If BA_Workspace_Exists(AOIFolderBase) Then 'cannot delete the folder
                MsgBox("Cannot delete the AOI! It is probably caused by a file lock in the Geodatabase. Please restart ArcGIS and repeat the process.")
            End If

            Me.Close()
            Exit Sub
        End If

        '======================
        'Clip and save DEM and Filled DEM
        '======================
        ' Open and Clip DEM
        inputraster = sourceSurfGDB & "\" & BA_EnumDescription(MapsFileName.dem_gdb)
        response = BA_ClipAOIRaster(AOIFolderBase, inputraster, BA_EnumDescription(MapsFileName.dem_gdb), destSurfGDB, AOIClipFile.BufferedAOIExtentCoverage, False)
        If response <= 0 Then
            MsgBox("Clipping DEM failed! Return value = " & response & ".")
        End If

        ' Open and Clip Filled DEM
        inputraster = sourceSurfGDB & "\" & BA_EnumDescription(MapsFileName.filled_dem_gdb)
        response = BA_ClipAOIRaster(AOIFolderBase, inputraster, BA_EnumDescription(MapsFileName.filled_dem_gdb), destSurfGDB, AOIClipFile.BufferedAOIExtentCoverage, False)
        If response <= 0 Then
            MsgBox("Clipping FILLED DEM failed! Return value = " & response & ".")
        End If

        '___________________________________________________________________________________________________________
        '_____________________________________________________________________________________________________________
        'creating weasel source file type and rasters are no longer required
        'make the aoi compatiable with a basin file structure
        'read the basin dem status file
        'Dim demstring As String
        'demstring = Check_Folder_Type(BasinFolderBase, BA_Basin_Type)

        'response = BA_Create_FolderType_File(AOIFolderBase, BA_Basin_Type, demstring)
        'creating weasel source file type is no longer required
        'the identical filled DEM is the source dem to be used in weasel
        'SourceRasterPath = BA_GetPath(AOIFolderBase, "SOURCEDEM")
        'response = BA_SaveRasterDataset(pDEMClip, SourceRasterPath, "grid")
        '___________________________________________________________________________________________________________
        '_____________________________________________________________________________________________________________

        '======================
        'Clip and save ASPECT
        '======================
        pStepProg.Message = "Clipping ASPECT... (step 2 of " & nstep & ")"
        pStepProg.Step()
        System.Windows.Forms.Application.DoEvents()

        inputraster = sourceSurfGDB & "\" & BA_EnumDescription(MapsFileName.aspect_gdb)
        response = BA_ClipAOIRaster(AOIFolderBase, inputraster, BA_EnumDescription(MapsFileName.aspect_gdb), destSurfGDB, AOIClipFile.BufferedAOIExtentCoverage, False)
        If response <= 0 Then
            MsgBox("Clipping ASPECT failed! Return value = " & response & ".")
        End If

        '======================
        'Clip and save SLOPE
        '======================
        pStepProg.Message = "Clipping SLOPE... (step 3 of " & nstep & ")"
        pStepProg.Step()
        System.Windows.Forms.Application.DoEvents()

        inputraster = sourceSurfGDB & "\" & BA_EnumDescription(MapsFileName.slope_gdb)
        response = BA_ClipAOIRaster(AOIFolderBase, inputraster, BA_EnumDescription(MapsFileName.slope_gdb), destSurfGDB, AOIClipFile.BufferedAOIExtentCoverage, False)
        If response <= 0 Then
            MsgBox("Clipping SLOPE failed! Return value = " & response & ".")
        End If

        '======================
        'Clip and save FLOW DIRECTION
        '======================
        pStepProg.Message = "Clipping FLOW DIRECTION... (step 4 of " & nstep & ")"
        pStepProg.Step()
        System.Windows.Forms.Application.DoEvents()

        inputraster = sourceSurfGDB & "\" & BA_EnumDescription(MapsFileName.flow_direction_gdb)
        response = BA_ClipAOIRaster(AOIFolderBase, inputraster, BA_EnumDescription(MapsFileName.flow_direction_gdb), destSurfGDB, AOIClipFile.BufferedAOIExtentCoverage)
        If response <= 0 Then
            MsgBox("Clipping FLOW DIRECTION failed! Return value = " & response & ".")
        End If

        '======================
        'Clip and save FLOW ACCUMULATION
        '======================
        pStepProg.Message = "Clipping FLOW ACCUMULATION... (step 5 of " & nstep & ")"
        pStepProg.Step()
        System.Windows.Forms.Application.DoEvents()

        inputraster = sourceSurfGDB & "\" & BA_EnumDescription(MapsFileName.flow_accumulation_gdb)
        response = BA_ClipAOIRaster(AOIFolderBase, inputraster, BA_EnumDescription(MapsFileName.flow_accumulation_gdb), destSurfGDB, AOIClipFile.BufferedAOIExtentCoverage, False)
        If response <= 0 Then
            MsgBox("Clipping FLOW ACCUMULATION failed! Return value = " & response & ".")
        End If

        '======================
        'Clip and save Hillshade
        '======================
        pStepProg.Message = "Clipping Hillshade... (step 6 of " & nstep & ")"
        pStepProg.Step()
        System.Windows.Forms.Application.DoEvents()

        inputraster = sourceSurfGDB & "\" & BA_EnumDescription(MapsFileName.hillshade_gdb)
        response = BA_ClipAOIRaster(AOIFolderBase, inputraster, BA_EnumDescription(MapsFileName.hillshade_gdb), destSurfGDB, AOIClipFile.BufferedAOIExtentCoverage, False)
        If response <= 0 Then
            MsgBox("Clipping HILLSHADE failed! Return value = " & response & ".")
        End If

        'update the Z unit metadata of DEM, slope, and PRISM
        Dim inputFolder As String
        Dim inputFile As String
        Dim unitText As String

        'We need to update the elevation units
        inputFolder = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces)
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
        inputFolder = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces)
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
                response = BA_ClipAOISNOTEL(AOIFolderBase, strParentName & "\" & strInLayerBareName, True)
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
                response = BA_ClipAOISNOTEL(AOIFolderBase, strParentName & "\" & strInLayerBareName, False)
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

                response = BA_ClipAOIRaster(AOIFolderBase, strInLayerPath & "\" & strInLayerBareName & "\grid", strInLayerBareName, destPRISMGDB, AOIClipFile.PrismClipAOIExtentCoverage)
                If response <= 0 Then
                    MsgBox("frmCreateAOI: Clipping " & strInLayerBareName & "\grid" & " failed! Return value = " & response & ".")
                End If
            Next

            'update the Z unit metadata of PRISM
            'We need to update the depth units if AOI for Basin Analysis was created
            inputFolder = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Prism)
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

                    Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(strParentName)
                    If strExtension = "(Shapefile)" Then
                        Dim featureClassExists As Boolean = False
                        If workspaceType = BAGIS_ClassLibrary.WorkspaceType.Raster Then
                            featureClassExists = BA_Shapefile_Exists(strParentName & strInLayerBareName)
                        Else
                            featureClassExists = BA_File_Exists(strParentName & strInLayerBareName, workspaceType, esriDatasetType.esriDTFeatureClass)
                        End If
                        If featureClassExists = True Then
                            'If BA_Shapefile_Exists(strParentName & strInLayerBareName) Then
                            response = BA_ClipAOIVector(AOIFolderBase, strParentName & strInLayerBareName, strInLayerBareName, _
                                                        destLayersGDB, True) 'always use buffered aoi to clip the layers
                        Else
                            MsgBox(strInLayerBareName & " does not exist", "Missing input")
                        End If

                    ElseIf strExtension = "(Raster)" Then
                        'If BA_File_ExistsRaster(strParentName, strInLayerBareName) Then
                        If BA_File_Exists(strParentName & strInLayerBareName, workspaceType, esriDatasetType.esriDTRasterDataset) Then
                            response = BA_ClipAOIRaster(AOIFolderBase, strParentName & strInLayerBareName, strInLayerBareName, destLayersGDB, AOIClipFile.BufferedAOIExtentCoverage)
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
            MsgBox("Create AOI Exception: " & ex.Message, MsgBoxStyle.OkOnly)
        End Try

        'Move aoibagis from folder to aoi.gdb; workaround for bug where watershed tool can't save to file gdb correctly
        Dim aoiBagisDSet As IGeoDataset = BA_OpenRasterFromFile(AOIFolderBase, BA_AOIExtentRaster)
        response = BA_SaveRasterDatasetGDB(aoiBagisDSet, destAOIGDB, BA_RASTER_FORMAT, BA_AOIExtentRaster)
        aoiBagisDSet = Nothing
        BA_Remove_Raster(AOIFolderBase, BA_AOIExtentRaster)

        pStepProg.Hide()
        progressDialog2.HideDialog()
        pStepProg = Nothing
        progressDialog2 = Nothing

        'copy the basinanalyst.def file to the aoi folder
        'response = BA_CopyBAGISSettings(AOIFolderBase)
        If BA_Save_Settings(AOIFolderBase & "\" & BA_Settings_Filename) = BA_ReturnCode.Success Then
            MsgBox("AOI for the selected gauge station was created!")
        Else
            MsgBox("AOI for the selected gauge station was created but the definition was not copied to the AOI folder!")
        End If

        'enable and disable relevant UI buttons
        Dim SelectedAOICombo = AddIn.FromID(Of cboTargetedAOI)(My.ThisAddIn.IDs.cboTargetedAOI)
        ''''BA_SetAOI(ThisDocument.SelectedAOI.Text)
        Dim aoinamestring As String = AOIFolderBase & "\" & SelectedAOICombo.getValue()
        BA_SetAOI(aoinamestring, True)
        Dim createAOIbutton = AddIn.FromID(Of BtnCreateAOI)(My.ThisAddIn.IDs.BtnCreateAOI)
        createAOIbutton.selectedProperty = False
        ClearMap() 'remove the graphics elements from the map

        Me.Close()
    End Sub

    Private Sub ChkSnapPP_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkSnapPP.Click
        lblSnapD.Enabled = ChkSnapPP.Checked
        lblSnapUnit.Enabled = ChkSnapPP.Checked
        txtSnapD.Enabled = ChkSnapPP.Checked
    End Sub

    Private Sub ChkAOIBuffer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkAOIBuffer.Click
        lblBufferD.Enabled = ChkAOIBuffer.Checked
        lblBufferUnit.Enabled = ChkAOIBuffer.Checked
        txtBufferD.Enabled = ChkAOIBuffer.Checked
    End Sub

    Private Sub frmCreateAOI_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ChkSnapPP.Checked = True
        ChkAOIBuffer.Checked = True
        txtSnapD.Text = "15"
        txtBufferD.Text = "100"

        If BA_SystemSettings.DEM_ZUnit_IsMeter Then
            lblDEMUnit.Text = BA_EnumDescription(MeasurementUnit.Meters)
        Else
            lblDEMUnit.Text = BA_EnumDescription(MeasurementUnit.Feet)
        End If

        lblSlopeUnit.Text = BA_EnumDescription(SlopeUnit.PctSlope) 'BAGIS generates Slope in Degree

        grpboxPRISMUnit.Visible = Not BA_SystemSettings.GenerateAOIOnly 'when generate AOI only, no PRISM will be clipped to the AOI

    End Sub

    Private Sub txtBufferD_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtBufferD.TextChanged
    End Sub

    Private Sub txtSnapD_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSnapD.TextChanged
    End Sub

    Private Sub lblBufferD_DoubleClick(sender As Object, e As System.EventArgs) Handles lblBufferD.DoubleClick
        Dim response As String
        response = InputBox("Please enter a PRISM buffer distance in meters", "Set/Check PRISM Buffer Distance", BA_PRISMClipBuffer)
        If Len(Trim(response)) > 0 Then
            BA_PRISMClipBuffer = Val(response)
        End If
    End Sub

    Private Sub lblWhyBuffer_Click(sender As System.Object, e As System.EventArgs) Handles lblWhyBuffer.Click
        Dim mText = "Layers can be clipped to an AOI using a buffered AOI boundaries."
        mText = mText & " This practice allows users to include data outside the AOI boundaries in basin analysis."
        mText = mText & " When this option is checked, all AOI associated layers, including DEM,"
        mText = mText & " its derivatives, SNOTEL, snow courses, and other participating layers"
        mText = mText & " are clipped to the AOI using the buffered boundaries." & vbCrLf & vbCrLf
        mText = mText & "Due to the significantly coarser resolution of PRISM precipitation layers, "
        mText = mText & " a differnt buffer distance is always used in clipping PRISM layers."
        mText = mText & " The default buffer distance for PRISM is 1000 meters."
        mText = mText & " Using any value smaller than 1000 could result in missing PRISM pixel values within the AOI boundaries."
        mText = mText & " To change the buffer distance for PRISM, please double-click on the Buffer Distance label."
        MsgBox(mText, MsgBoxStyle.Information, "Why Buffer an AOI")
    End Sub
End Class