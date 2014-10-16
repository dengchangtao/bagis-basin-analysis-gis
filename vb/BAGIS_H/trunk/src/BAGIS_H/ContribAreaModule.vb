Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.GeoAnalyst
Imports ESRI.ArcGIS.DataSourcesFile
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.ArcMapUI
Imports System.Windows.Forms
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.SpatialAnalyst
Imports ESRI.ArcGIS.SpatialAnalystTools
Imports ESRI.ArcGIS.SpatialStatisticsTools
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Geoprocessing
Imports System.Text
Imports System.IO
Imports System
Imports ESRI.ArcGIS.DataSourcesGDB
Imports ESRI.ArcGIS.DataManagementTools
Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.Framework

Module ContribAreaModule
    'the list of intermediate shapefiles that users might want to keep
    Private Const StreamLinkFile As String = "streamlinks"
    Private Const WatershedFile As String = "watersheds"
    Private Const WatershedRaster As String = "watersheds_r"
    Private Const CA_1AreaFile As String = "contributingarea_1"
    Private Const CA_2AreaFile As String = "contributingarea_2"
    Private Const CA_3AreaFile As String = "contributingarea_3"
    Private Const EndCatchmentFile As String = "endcatchments"
    Private Const ThresholdLink_LUTFilename As String = "threshold_link_LUT.txt"
    Private Const CA_1TempFile As String = "ca_1tmp"
    Private Const maximumSliverSize As Double = 0.005 'param for eliminating sliver polygons (i.e., area < 5 30-meter pixels or 4500 m^2)

    'main entry point of the processing
    Public Function BA_CreateContributingAreas(ByVal aoiPath As String, ByVal outputFolder As String, ByVal outputName As String, _
                                     ByVal paramContribArea As ContributingAreasRule) As BA_ReturnCode
        Dim returnCode As BA_ReturnCode = BA_ReturnCode.UnknownError

        'contributing areas are created based on flowacc and flowdir
        Select Case paramContribArea.NumberofArea
            Case 1 'one area contributing area
                If BA_1AreaWatersheds(aoiPath, outputFolder, outputName, paramContribArea) = BA_ReturnCode.Success Then
                    Return BA_ReturnCode.Success
                Else
                    MessageBox.Show("BA_CreateContributingAreas Error: 1-Area Contributing Areas creation error.")
                    Return BA_ReturnCode.WriteError
                End If
            Case 2 'two areas contributing area
                If BA_2AreasWatersheds(aoiPath, outputFolder, outputName, paramContribArea) = BA_ReturnCode.Success Then
                    Return BA_ReturnCode.Success
                Else
                    MessageBox.Show("BA_CreateContributingAreas Error: 2-Area Contributing Areas creation error.")
                    Return BA_ReturnCode.WriteError
                End If
                Return BA_ReturnCode.WriteError
            Case Else 'three areas contributing area
                If BA_3AreasWatersheds(aoiPath, outputFolder, outputName, paramContribArea) = BA_ReturnCode.Success Then
                    Return BA_ReturnCode.Success
                Else
                    MessageBox.Show("BA_CreateContributingAreas Error: 3-Area Contributing Areas creation error.")
                    Return BA_ReturnCode.WriteError
                End If
                Return BA_ReturnCode.WriteError
        End Select
    End Function

    'supporting module for contributing area
    'load threshold_link_LUT to memory
    Public Function BA_LoadStreamLinkCountLUT(ByVal aoiFolder As String, ByRef Median_Arr() As Double, _
                                              ByRef Threshold_Arr() As Long, ByRef Count_Arr() As Long, _
                                              ByVal LUT_Version_Keywords As String) As BA_ReturnCode
        'Dim LUTfile As String = BA_GetPath(aoiFolder, PublicPath.FlowAccumulation) & "\" & ThresholdLink_LUTFilename
        Dim LUTFile As String = BA_GeodatabasePath(aoiFolder, GeodatabaseNames.Surfaces, True) & ThresholdLink_LUTFilename
        Dim returnValue As BA_ReturnCode = BA_ReturnCode.UnknownError
        Dim nrecords As Long = 0
        Dim lineString As String = ""
        Dim splitString As String()

        If Not File.Exists(LUTfile) Then
            Return BA_ReturnCode.ReadError
        End If

        System.Array.Resize(Median_Arr, 200)
        System.Array.Resize(Threshold_Arr, 200)
        System.Array.Resize(Count_Arr, 200)

        Try
            Using sr As StreamReader = File.OpenText(LUTfile)
                lineString = sr.ReadLine() 'read the keyword line

                If String.Compare(lineString, LUT_Version_Keywords) = 0 Then 'version does match
                    sr.ReadLine() 'skip the header line
                    Do While sr.Peek() >= 0
                        lineString = sr.ReadLine()
                        splitString = lineString.Split(",")

                        'retrieve threshold value
                        Threshold_Arr(nrecords) = Val(splitString(0))

                        'retrieve count value
                        Count_Arr(nrecords) = Val(splitString(1))

                        'retrieve median factor value
                        Median_Arr(nrecords) = Val(splitString(2))

                        nrecords += 1
                    Loop

                    System.Array.Resize(Median_Arr, nrecords - 1)
                    System.Array.Resize(Threshold_Arr, nrecords - 1)
                    System.Array.Resize(Count_Arr, nrecords - 1)
                    returnValue = BA_ReturnCode.Success

                Else
                    returnValue = BA_ReturnCode.ReadError

                End If
                sr.Close()

            End Using

        Catch ex As Exception
            MessageBox.Show("BA_LoadStreamLinkCountLUT Exception: " & ex.Message)
            returnValue = BA_ReturnCode.OtherError

        End Try

        Return returnValue
    End Function

    'count the number of streamlinks based on a flowaccumulation threshold value
    Public Function BA_StreamLinkCountLUT(ByVal aoiFolder As String, _
                                          ByVal LUT_Version_Keywords As String) As BA_ReturnCode

        Dim returnValue As BA_ReturnCode = BA_ReturnCode.UnknownError

        Dim nInterval As Integer = 0 'number of interval classes - values specified by the program based on flowacc stats
        Dim arrInterval(200) As Double

        Dim flowAccThreshold As Long = 0
        Dim flowAccStDev As Double = 0
        Dim flowAccMax As Double = 0

        Dim pDEUtility As IDEUtilities = New DEUtilities

        'Use the AOI extent to limit the process extent
        Dim pAOIRaster As IRasterDataset3 = Nothing
        Dim pFAccGDS As IGeoDataset = Nothing
        Dim pTempFAccRaster As IGeoDataset = Nothing
        Dim pFDirGDS As IGeoDataset = Nothing
        Dim pFAccRasterDS As IRasterDataset3 = Nothing
        Dim pFAccRaster As IRaster = Nothing

        Dim pExtractOp As IExtractionOp2 = New RasterExtractionOp
        Dim pQFilter As IQueryFilter = New QueryFilter
        Dim pRasDes As IRasterDescriptor = New RasterDescriptor
        Dim pStreamGDS As IGeoDataset = Nothing
        Dim pHydrologyOp As IHydrologyOp = New RasterHydrologyOp
        Dim pStreamlinkGDS As IGeoDataset = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pRasterBandCollection As IRasterBandCollection = Nothing

        Dim pStepProg As ESRI.ArcGIS.esriSystem.IStepProgressor = Nothing
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = Nothing

        Try
            'set flow_accum path and name
            Dim rasterfilepath As String
            'Dim rasterFileName As String = BA_EnumDescription(MapsFileName.flow_accumulation)
            Dim rasterFileName As String = BA_EnumDescription(MapsFileName.flow_accumulation_gdb)
            'rasterFileName = BA_StandardizeShapefileName(rasterFileName, False, False)
            'rasterfilepath = BA_GetPath(aoiFolder, PublicPath.FlowAccumulation)
            rasterfilepath = BA_GeodatabasePath(aoiFolder, GeodatabaseNames.Surfaces)

            'save the look-up table in the flow accumulation folder of the AOI
            'Dim output As String = BA_GetPath(aoiFolder, PublicPath.FlowAccumulation) & "\" & ThresholdLink_LUTFilename
            Dim output As String = BA_GeodatabasePath(aoiFolder, GeodatabaseNames.Surfaces, True) & ThresholdLink_LUTFilename

            'Open AOI Polygon to set the analysis mask
            rasterFileName = BA_EnumDescription(PublicPath.AoiGrid)
            rasterFileName = BA_StandardizeShapefileName(rasterFileName, False, False)
            'pAOIRaster = BA_OpenRasterFromFile(aoiFolder, rasterFileName)
            pAOIRaster = BA_OpenRasterFromGDB(BA_GeodatabasePath(aoiFolder, GeodatabaseNames.Aoi), rasterFileName)
            If pAOIRaster Is Nothing Then
                MsgBox("Cannot locate AOI boundary raster in the AOI!")
                Return BA_ReturnCode.ReadError
            End If

            'open flow_accum raster
            'rasterFileName = BA_EnumDescription(MapsFileName.flow_accumulation)
            'rasterFileName = BA_StandardizeShapefileName(rasterFileName, False, False)
            rasterFileName = BA_EnumDescription(MapsFileName.flow_accumulation_gdb)
            'pFAccGDS = BA_OpenRasterFromFile(rasterfilepath, rasterFileName)
            pFAccGDS = BA_OpenRasterFromGDB(rasterfilepath, rasterFileName)
            pTempFAccRaster = pExtractOp.Raster(pFAccGDS, pAOIRaster)

            'get raster statistics of the Flow Accumulation raster
            pRasterBandCollection = pTempFAccRaster
            pRasterBand = pRasterBandCollection.Item(0)
            flowAccStDev = pRasterBand.Statistics.StandardDeviation
            flowAccMax = pRasterBand.Statistics.Maximum

            'determine the class interval
            Dim unadjustedInterval As Long = CLng((flowAccStDev / 2) / 20)
            Dim adjustedInterval As Long = Left(unadjustedInterval.ToString, 1) * Math.Pow(10, (Len(unadjustedInterval.ToString) - 1))
            nInterval = CLng((flowAccStDev / 2) / adjustedInterval)

            'set the threshold intervals for facc values below 1 stdev
            For ni As Integer = 0 To nInterval - 1
                arrInterval(ni) = adjustedInterval * (ni + 1)
            Next

            'set the threshold intervals for facc values above 1 median value
            Dim lastInterval As Double = arrInterval(nInterval - 1) + adjustedInterval
            Dim additionalInterval As Integer = 0
            Dim nFactor As Integer = 1
            Do While lastInterval < flowAccMax And nInterval + additionalInterval <= 200
                arrInterval(nInterval + additionalInterval) = lastInterval
                lastInterval = lastInterval + adjustedInterval * nFactor
                additionalInterval += 1
                If additionalInterval Mod 2 = 0 Then
                    nFactor *= 2
                End If
            Loop

            nInterval = nInterval + additionalInterval
            ReDim Preserve arrInterval(nInterval - 1)

            pStepProg = BA_GetStepProgressor(My.ArcMap.Application.hWnd, nInterval / 2)
            ' Create/configure the ProgressDialog. This automatically displays the dialog
            progressDialog2 = BA_GetProgressDialog(pStepProg, "Counting Streamlinks...", "Counting Streamlinks")
            pStepProg.StepValue = 1
            pStepProg.Step()

            rasterFileName = BA_EnumDescription(MapsFileName.flow_direction_gdb)
            rasterFileName = BA_StandardizeShapefileName(rasterFileName, False, False)
            'rasterfilepath = BA_GetPath(aoiFolder, PublicPath.FlowDirection)
            'pFDirGDS = BA_OpenRasterFromFile(rasterfilepath, rasterFileName)
            pFDirGDS = BA_OpenRasterFromGDB(rasterfilepath, rasterFileName)

            ' Create/configure a step progressor

            Using sw As StreamWriter = File.CreateText(output)
                sw.WriteLine(LUT_Version_Keywords)
                sw.WriteLine("FAcc Threshold, Link No, StDev Factor")
                Dim MedianFactorValue As Double = 0

                For ni As Integer = 0 To nInterval - 1
                    flowAccThreshold = arrInterval(ni)
                    MedianFactorValue = Math.Round(flowAccThreshold / flowAccStDev, 6)

                    'Query the Raster and Create a new dataset on the buffered FAcc layer
                    'the buffering would ensure that stream links intersect with the watershed boundaries
                    pQFilter.WhereClause = "Value > " & flowAccThreshold
                    pRasDes.Create(pTempFAccRaster, pQFilter, "Value")
                    pStreamGDS = pExtractOp.Attribute(pRasDes)

                    'pStreamGDS is masked by the AOI raster
                    pStreamlinkGDS = pHydrologyOp.StreamLink(pStreamGDS, pFDirGDS)

                    'get raster statistics
                    pRasterBandCollection = pStreamlinkGDS
                    pRasterBand = pRasterBandCollection.Item(0)

                    sw.WriteLine(flowAccThreshold & ", " & pRasterBand.Statistics.Maximum & ", " & MedianFactorValue)

                    pStepProg.Step()

                Next
                sw.Flush()
                sw.Close()
            End Using
            returnValue = BA_ReturnCode.Success

        Catch ex As Exception
            MessageBox.Show("BA_StreamlinkCountLUT Exception: " & ex.Message)
            returnValue = BA_ReturnCode.OtherError

        Finally
            If progressDialog2 IsNot Nothing Then
                progressDialog2.HideDialog()
            End If

            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pAOIRaster)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFAccGDS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFAccRaster)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFAccRasterDS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFDirGDS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pExtractOp)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pQFilter)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasDes)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pHydrologyOp)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStreamlinkGDS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBandCollection)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBand)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTempFAccRaster)
            pDEUtility.ReleaseInternals()

        End Try

        Return returnValue

    End Function

    '====================================================
    'beginning of the code for 3 zones subroutines
    Public Function BA_3AreasWatersheds(ByVal aoiFolder As String, _
                                      ByVal outputFolder As String, _
                                      ByVal outputName As String, _
                                      ByVal ContribRule As ContributingAreasRule) As BA_ReturnCode

        Dim response As Integer
        'temporary file names
        Dim linkNodesFile As String = "linknodes"

        'the outputFolder points to a gdb, create a string variable to hold the folder containing the gdb for saving shapefiles
        Dim shapefileFolder As String = ""
        Dim gdbName As String = BA_GetBareName(outputFolder, shapefileFolder)
        shapefileFolder = BA_StandardizePathString(shapefileFolder, False) 'this is where the temp shapefiles will be saved

        'determine threshold value
        If ContribRule.FACCThresholdValue <= 0 Then
            MessageBox.Show("Invalid threshold value. Please re-enter.")
            Return BA_ReturnCode.OtherError
            Exit Function
        End If

        Dim FAccStDev As Double = ContribRule.FACCStandardDeviation
        Dim FAccThreshold As Double = ContribRule.FACCThresholdValue 'use facc value directly, no longer use st dev as unit

        'first create 1-area watersheds
        Dim OneArea_Outputname As String = outputName & "_1"
        Dim OneArea_ReturnCode As BA_ReturnCode = _
            BA_Streamlinks2Watersheds(aoiFolder, outputFolder, OneArea_Outputname, FAccThreshold, True)

        If Not OneArea_ReturnCode = BA_ReturnCode.Success Then
            Return OneArea_ReturnCode
            Exit Function
        End If

        'extract end nodes from the streamlinks
        BA_StreamEnds(aoiFolder, shapefileFolder, StreamLinkFile, shapefileFolder, linkNodesFile)

        'use stream end nodes to create end catchments
        BA_CreateWatershedsfromPourpoints(aoiFolder, shapefileFolder, linkNodesFile, outputFolder, WatershedRaster, 0)

        'vectorize the end watershed raster
        'check if output vector exist, if yes, remove it
        If BA_Shapefile_Exists(shapefileFolder & "\" & EndCatchmentFile) Then
            response = BA_Remove_Shapefile(shapefileFolder, EndCatchmentFile)
        End If

        Dim inRasterPath As String = outputFolder & "\" & WatershedRaster
        Dim outFeaturesPath As String = shapefileFolder & "\" & EndCatchmentFile & ".shp"

        Dim snapRasterPath As String = BA_GeodatabasePath(aoiFolder, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
        Dim success As BA_ReturnCode = BA_Raster2Polygon_GP(inRasterPath, outFeaturesPath, snapRasterPath)
        If success <> BA_ReturnCode.Success Then
            MsgBox("BA_3AreasWatersheds Error: Unable to save the temporary end-watershed file. Program stopped.")
            Return BA_ReturnCode.WriteError
        End If

        'merge the 1-area contributing watersheds with the end watersheds
        'check if output exist, if yes, remove it
        Dim CA_3AreaFileTemp As String = "ContributingArea_3Temp"
        Dim CA_3AreaFileTemp1 As String = "CA_3Temp1"

        If BA_Shapefile_Exists(shapefileFolder & "\" & CA_3AreaFileTemp) Then
            response = BA_Remove_Shapefile(shapefileFolder, CA_3AreaFileTemp)
        End If

        ' merge watershed polygons, streamlin lines, and endcatchment polygons
        If BA_MergeVectorsToPolygons(shapefileFolder, CA_1AreaFile, EndCatchmentFile, StreamLinkFile, shapefileFolder, CA_3AreaFileTemp, False) <> BA_ReturnCode.Success Then Return BA_ReturnCode.NotSupportedOperation
        'If BA_MergeVectorsToPolygons(shapefileFolder, CA_3AreaFileTemp1, EndCatchmentFile, "", shapefileFolder, CA_3AreaFileTemp, False) <> BA_ReturnCode.Success Then Return BA_ReturnCode.NotSupportedOperation

        Dim statResults As BA_DataStatistics
        Dim success2 As Integer
        statResults.Minimum = maximumSliverSize + 1
        'eliminate sliver polygons (i.e., area < 5 30-meter pixels or 4500 m^2)
        success = BA_AddShapeAreaToAttrib(shapefileFolder & "\" & CA_3AreaFileTemp)
        success2 = BA_GetDataStatistics(shapefileFolder & "\" & CA_3AreaFileTemp & ".shp", BA_FIELD_AREA_SQKM, statResults)

        Dim maxiteration As Integer = 3
        Dim ncount As Integer = 1
        Do While success2 = 0 And statResults.Minimum < maximumSliverSize
            If BA_Shapefile_Exists(shapefileFolder & "\" & CA_3AreaFile) Then
                response = BA_Remove_Shapefile(shapefileFolder, CA_3AreaFile)
            End If
            'MsgBox("Debug message: Slivers need to be eliminated!")
            BA_EliminatePoly(shapefileFolder, CA_3AreaFileTemp & ".shp", shapefileFolder, CA_3AreaFile & ".shp", "AREA", maximumSliverSize, BA_FIELD_AREA_SQKM)
            response = BA_Remove_Shapefile(shapefileFolder, CA_3AreaFileTemp)

            statResults.Minimum = maximumSliverSize + 1
            success = BA_AddShapeAreaToAttrib(shapefileFolder & "\" & CA_3AreaFile)
            success2 = BA_GetDataStatistics(shapefileFolder & "\" & CA_3AreaFile & ".shp", BA_FIELD_AREA_SQKM, statResults)

            ncount += 1
            If ncount > maxiteration Then success2 = 1 'this prevents infinite loops

            If success2 = 0 And statResults.Minimum < maximumSliverSize Then
                response = BA_CopyFeatures(shapefileFolder & "\" & CA_3AreaFile & ".shp", shapefileFolder & "\" & CA_3AreaFileTemp & ".shp")
            End If
        Loop

        'convert the vector HRU to raster
        'prepare attribute field for rasterization
        Dim featureCount As Long = 0
        Dim attributeFieldName As String = "ID"
        featureCount = BA_UpdateShapefileID(shapefileFolder, CA_3AreaFile, attributeFieldName)

        If featureCount <= 0 Then
            MessageBox.Show("BA_3AreasWatersheds Error: No polygon in the merged 3-zone watershed shapefile!")
            Return BA_ReturnCode.ReadError
        End If

        'convert vector to raster
        'get raster resolution
        Dim cellSize As Double
        Dim rasterStat As IRasterStatistics = Nothing
        'Dim rasterfilepath As String = BA_GetPath(aoiFolder, PublicPath.FlowAccumulation)
        Dim rasterfilepath As String = BA_GeodatabasePath(aoiFolder, GeodatabaseNames.Surfaces, True)

        rasterStat = BA_GetRasterStatsGDB(rasterfilepath & BA_EnumDescription(MapsFileName.flow_direction_gdb), cellSize)
        rasterStat = Nothing

        Dim vInputPath As String = shapefileFolder & "\" & CA_3AreaFile & ".shp"
        'Dim outRasterPath As String = outputFolder & "\" & outputName
        Dim tempRasterName As String = "tempR001"
        Dim outRasterPath As String = outputFolder & "\" & tempRasterName

        If BA_File_Exists(outputFolder & "\" & tempRasterName, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            BA_RemoveRasterFromGDB(outputFolder, tempRasterName)
        End If

        'save raster and return to the calling routine
        success = BA_Feature2RasterGP(vInputPath, outRasterPath, attributeFieldName, cellSize, snapRasterPath)

        'Removing NoData from the output raster
        Dim maskFolder As String = BA_GeodatabasePath(aoiFolder, GeodatabaseNames.Aoi, True)
        Dim maskFile As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.AoiVector), False)

        If BA_RasterHasNodata(aoiFolder, outputFolder, tempRasterName) Then
            If BA_RemNodataFromRas(outputFolder, tempRasterName, maskFolder, maskFile, _
                                   outputFolder, outputName, snapRasterPath, aoiFolder) <> BA_ReturnCode.Success Then
                Return BA_ReturnCode.UnknownError
                Exit Function
            End If
            BA_RemoveRasterFromGDB(outputFolder, tempRasterName)
        Else
            ' Rename output of filter to outfile name if we don't have to remove NoData values
            BA_RenameRasterInGDB(outputFolder, tempRasterName, outputName)
        End If

        'the temp file is saved as a shapefile, instead of a feature class in GDB, to prevent issues with GDB's min tolerance limit 
        If BA_Shapefile_Exists(shapefileFolder & "\" & CA_3AreaFileTemp) Then
            response = BA_Remove_Shapefile(shapefileFolder, CA_3AreaFileTemp)
        End If

        If BA_File_Exists(outputFolder & "\" & WatershedRaster, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            response = BA_RemoveRasterFromGDB(outputFolder, WatershedRaster)
        End If

        If BA_Shapefile_Exists(shapefileFolder & "\" & CA_1AreaFile) Then
            response = BA_Remove_Shapefile(shapefileFolder, CA_1AreaFile)
        End If

        If BA_File_ExistsRaster(shapefileFolder, CA_1TempFile) Then
            response = BA_Remove_Raster(shapefileFolder, CA_1TempFile)
        End If

        If BA_Shapefile_Exists(shapefileFolder & "\" & CA_3AreaFile) Then
            response = BA_Remove_Shapefile(shapefileFolder, CA_3AreaFile)
        End If

        'clean up temporary files
        If Not ContribRule.KeepTemporaryFiles Then
            'check if output exist, if yes, remove it

            If BA_File_Exists(outputFolder & "\" & OneArea_Outputname, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                response = BA_RemoveRasterFromGDB(outputFolder, OneArea_Outputname)
            End If

            If BA_Shapefile_Exists(shapefileFolder & "\" & EndCatchmentFile) Then
                response = BA_Remove_Shapefile(shapefileFolder, EndCatchmentFile)
            End If

            If BA_Shapefile_Exists(shapefileFolder & "\" & StreamLinkFile) Then
                response = BA_Remove_Shapefile(shapefileFolder, StreamLinkFile)
            End If

            If BA_Shapefile_Exists(shapefileFolder & "\" & linkNodesFile) Then
                response = BA_Remove_Shapefile(shapefileFolder, linkNodesFile)
            End If

        End If

        Return BA_ReturnCode.Success
    End Function

    'create watersheds using a pourpoint feature class.
    'The output is a watershed raster saved in the same location as the pourpoint feature class.
    Public Function BA_CreateWatershedsfromPourpoints(ByVal aoiFolder As String, ByVal PourPntFolder As String, ByVal PourPntName As String, _
                                                      ByVal outputFolder As String, ByVal OutWatershedName As String, _
                                                      Optional ByVal Snap_Distance As Double = 0) As BA_ReturnCode
        Dim pDEUtility As IDEUtilities = New DEUtilities
        Dim SnappedPPName As String = PourPntName & "_snp"
        Dim layerFileName As String

        'set pourpoint, flow_accum, and flow direction raster file path
        Dim snapRasterPath As String
        layerFileName = BA_EnumDescription(PublicPath.AoiGrid)
        snapRasterPath = BA_GeodatabasePath(aoiFolder, GeodatabaseNames.Aoi) & layerFileName

        Dim FAccfilePathName As String
        layerFileName = BA_EnumDescription(MapsFileName.flow_accumulation_gdb)
        FAccfilePathName = BA_GeodatabasePath(aoiFolder, GeodatabaseNames.Surfaces, True) & layerFileName
        'FAccfilePathName = BA_GetPath(aoiFolder, PublicPath.FlowAccumulation) & "\" & BA_StandardizeShapefileName(layerFileName, False, False)

        Dim FDirfilePathName As String
        layerFileName = BA_EnumDescription(MapsFileName.flow_direction_gdb)
        FDirfilePathName = BA_GeodatabasePath(aoiFolder, GeodatabaseNames.Surfaces, True) & layerFileName

        Dim PourpointPathName As String = PourPntFolder & "\" & PourPntName & ".shp"

        'Use the stream ends to delineate watersheds
        Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
        Dim result As ESRI.ArcGIS.Geoprocessing.IGeoProcessorResult2

        ' Set default workspace
        GP.SetEnvironmentValue("workspace", outputFolder)
        GP.SetEnvironmentValue("mask", snapRasterPath)
        GP.SetEnvironmentValue("snapRaster", snapRasterPath)
        GP.AddOutputsToMap = False
        GP.OverwriteOutput = True

        Dim SnapTool As ESRI.ArcGIS.SpatialAnalystTools.SnapPourPoint = New ESRI.ArcGIS.SpatialAnalystTools.SnapPourPoint
        Dim WatershedTool As ESRI.ArcGIS.SpatialAnalystTools.Watershed = New ESRI.ArcGIS.SpatialAnalystTools.Watershed

        With SnapTool
            .in_accumulation_raster = FAccfilePathName
            .in_pour_point_data = PourpointPathName
            .snap_distance = Snap_Distance
            .pour_point_field = "ID" 'ID generated by the BA_UpdateShapefileID
            .out_raster = SnappedPPName
        End With

        result = GP.Execute(SnapTool, Nothing)
        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(SnapTool)

        ' If the job failed, retrieve the feature result.
        If result Is Nothing Then
            MessageBox.Show("BA_CreateWatershedsfromPourpoints Error: Snapping pourpoint to flow accumulation execution failed.")
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(GP)
            Return BA_ReturnCode.UnknownError
            Exit Function
        End If

        With WatershedTool
            .in_flow_direction_raster = FDirfilePathName
            .in_pour_point_data = outputFolder & "\" & SnappedPPName
            .pour_point_field = "VALUE"
            .out_raster = OutWatershedName
        End With
        result = GP.Execute(WatershedTool, Nothing)
        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(WatershedTool)

        ' If the job failed, retrieve the feature result.
        If result Is Nothing Then
            MessageBox.Show("BA_CreateWatershedsfromPourpoints Error: Creating watershed from pourpoints execution failed.")
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(GP)
            Return BA_ReturnCode.UnknownError
            Exit Function
        End If

        'clean up the temp layers
        BA_RemoveRasterFromGDB(outputFolder, SnappedPPName)

        result = Nothing
        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(GP)
        pDEUtility.ReleaseInternals()
    End Function

    'count the occurrance of points in each point location
    Public Function BA_CollectEvents(ByVal AOIFolder As String, ByVal inputFolder As String, ByVal inputName As String, ByVal outputName As String) As BA_ReturnCode

        Dim pDEUtility As IDEUtilities = New DEUtilities
        inputFolder = BA_StandardizePathString(inputFolder, False)
        inputName = BA_StandardizeShapefileName(inputName, False, False)

        'generate a near table
        Dim gpUtils As IGPUtilities = New GPUtilities
        Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
        Dim result As ESRI.ArcGIS.Geoprocessing.IGeoProcessorResult2

        Dim CollectEventTool As ESRI.ArcGIS.SpatialStatisticsTools.CollectEvents = New ESRI.ArcGIS.SpatialStatisticsTools.CollectEvents
        GP.SetEnvironmentValue("workspace", inputFolder)
        GP.AddOutputsToMap = False
        GP.OverwriteOutput = True

        'create an input layer with only points within the AOI selected
        ' Create the MakeFeatureLayer tool process object.
        Dim makFeatTool As ESRI.ArcGIS.DataManagementTools.MakeFeatureLayer = New MakeFeatureLayer()
        Dim templayer As String = "TempLyr"

        ' Populate the MakeFeatureLayerTool with parameter values.            
        With makFeatTool
            .in_features = inputFolder & "\" & inputName & ".shp"
            .out_layer = templayer
        End With

        ' Execute the model tool by name.
        result = GP.Execute(makFeatTool, Nothing)
        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(makFeatTool)

        ' If the job failed, retrieve the feature result.
        If result Is Nothing Then
            MessageBox.Show("Error: MakeFeatureLayer tool execution failed.")
            Exit Function
        End If

        ' Create the SelectLayerByAttribute tool process object.
        Dim selLayLocationTool As ESRI.ArcGIS.DataManagementTools.SelectLayerByLocation = New SelectLayerByLocation()
        Dim AOIFileName As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.AoiVector), False, True)
        Dim AOIFilePathName As String = BA_GeodatabasePath(AOIFolder, GeodatabaseNames.Aoi) & AOIFileName

        ' Populate the MakeFeatureLayerTool with parameter values.            
        With selLayLocationTool
            .in_layer = templayer
            .select_features = AOIFilePathName
            .search_distance = 0
            .selection_type = "NEW_SELECTION"
            .overlap_type = "INTERSECT"
        End With

        ' Execute the model tool by name.
        result = GP.Execute(selLayLocationTool, Nothing)
        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(selLayLocationTool)

        ' If the job failed, retrieve the feature result.
        If result Is Nothing Then
            MessageBox.Show("Error: SelectLayerByAttribute tool execution failed.")
            Exit Function
        End If

        With CollectEventTool
            .Input_Incident_Features = templayer
            .Output_Weighted_Point_Feature_Class = inputFolder & "\" & outputName & ".shp"
        End With

        result = GP.Execute(CollectEventTool, Nothing)
        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(CollectEventTool)
        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(GP)

        If result Is Nothing Then
            MessageBox.Show("BA_CollectEvents Error: Unable to collect the count of colocated point features.")
            Return BA_ReturnCode.UnknownError
            Exit Function
        End If

        'add unique ID to the point features
        BA_UpdateShapefileID(inputFolder, outputName)

        result = Nothing
        gpUtils.RemoveInternalLayer(templayer)
        gpUtils.ReleaseInternals()
        gpUtils = Nothing
        pDEUtility.ReleaseInternals()

        Return BA_ReturnCode.Success
    End Function

    'merge a polygon shapefile and a polyline shapefile and return a merged polygon shapefile
    Public Function BA_MergePolylines(ByVal inputFolder1 As String, _
        ByVal inputName1 As String, ByVal inputFolder2 As String, ByVal inputName2 As String, _
        ByVal outputFolder As String, ByVal outputName As String) As BA_ReturnCode

        Dim pDEUtility As IDEUtilities = New DEUtilities
        Dim returnValue As BA_ReturnCode = BA_ReturnCode.UnknownError

        'this subroutine is based on GP
        Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
        Dim result As ESRI.ArcGIS.Geoprocessing.IGeoProcessorResult2

        'standardize filepath and filenames
        inputFolder1 = BA_StandardizePathString(inputFolder1, False)
        inputFolder2 = BA_StandardizePathString(inputFolder2, False)
        outputFolder = BA_StandardizePathString(outputFolder, False)
        inputName1 = BA_StandardizeShapefileName(inputName1, False, False)
        inputName2 = BA_StandardizeShapefileName(inputName2, False, False)
        outputName = BA_StandardizeShapefileName(outputName, False, False)

        ' Set default workspace
        GP.SetEnvironmentValue("workspace", outputFolder)
        GP.AddOutputsToMap = False
        GP.OverwriteOutput = True

        Dim MergeTool As ESRI.ArcGIS.DataManagementTools.Merge = New ESRI.ArcGIS.DataManagementTools.Merge
        'Dim infeatures As IGpObjectArray

        Try
            ' merge the two line featureclasses
            Dim inputFeatureclasses As String = ""
            inputFeatureclasses = inputFolder1 & "\" & inputName1 & ".shp;"
            inputFeatureclasses = inputFeatureclasses & inputFolder2 & "\" & inputName2 & ".shp;"

            With MergeTool
                .inputs = inputFeatureclasses
                .output = outputName & ".shp"
            End With
            result = GP.Execute(MergeTool, Nothing)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(MergeTool)

            ' If the job failed, retrieve the feature result.
            If result Is Nothing Then
                MessageBox.Show("Merge Polylines Error: Merge tool execution failed.")
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(GP)
                Return BA_ReturnCode.UnknownError
                Exit Function
            End If

            returnValue = BA_ReturnCode.Success

        Catch ex As Exception
            Dim errMsg As String = "Contributing Area GP Error: "
            For counter As Integer = 0 To GP.MessageCount - 1
                errMsg = errMsg & vbCrLf & GP.GetMessage(counter)
            Next
            MsgBox(errMsg)

        Finally
            result = Nothing
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(GP)
            pDEUtility.ReleaseInternals()
        End Try

        Return returnValue
    End Function

    'get the upstream end points from a streamlink vector layer
    Public Function BA_StreamEnds(ByVal aoiFolder As String, _
                               ByVal inputFolder As String, ByVal inputName As String, _
                               ByVal outputFolder As String, ByVal outputName As String, _
                               Optional ByVal streamMinLength As Double = 0) As BA_ReturnCode

        Dim pDEUtility As IDEUtilities = New DEUtilities
        Dim tempStreamNodes As String = "temp_snodes"
        Dim tempNodeEvents As String = "temp_nodeevents"
        Dim eventFieldName As String = "ICOUNT"

        Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
        Dim result As ESRI.ArcGIS.Geoprocessing.IGeoProcessorResult2

        Dim Vertice2PTool As ESRI.ArcGIS.DataManagementTools.FeatureVerticesToPoints = New ESRI.ArcGIS.DataManagementTools.FeatureVerticesToPoints
        Dim Value2PTool As ESRI.ArcGIS.SpatialAnalystTools.ExtractMultiValuesToPoints = New ESRI.ArcGIS.SpatialAnalystTools.ExtractMultiValuesToPoints
        Dim SelectTool As ESRI.ArcGIS.AnalysisTools.Select = New ESRI.ArcGIS.AnalysisTools.Select
        Dim NearTool As ESRI.ArcGIS.AnalysisTools.Near = New ESRI.ArcGIS.AnalysisTools.Near

        Dim snapRasterPath As String = BA_GeodatabasePath(aoiFolder, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
        ' Set default workspace
        GP.SetEnvironmentValue("workspace", outputFolder)
        GP.SetEnvironmentValue("snapRaster", snapRasterPath)
        GP.AddOutputsToMap = False
        GP.OverwriteOutput = True

        inputName = BA_StandardizeShapefileName(inputName, False, False)
        outputName = BA_StandardizeShapefileName(outputName, False, False)

        With Vertice2PTool
            .in_features = inputFolder & "\" & inputName & ".shp"
            .out_feature_class = tempStreamNodes & ".shp"
            .point_location = "BOTH_ENDS"
        End With
        result = GP.Execute(Vertice2PTool, Nothing)
        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(Vertice2PTool)

        If result Is Nothing Then
            MessageBox.Show("BA_SteamEnds Error: Unable to convert stream end points to point features.")
            GP = Nothing
            Return BA_ReturnCode.UnknownError
            Exit Function
        End If

        'count the number of colocated points for each locations
        If Not BA_CollectEvents(aoiFolder, outputFolder, tempStreamNodes, tempNodeEvents) = BA_ReturnCode.Success Then
            Return BA_ReturnCode.UnknownError
            Exit Function
        End If

        BA_Remove_Shapefile(outputFolder, tempStreamNodes)

        'select only the end nodes to the output
        With SelectTool
            .in_features = outputFolder & "\" & tempNodeEvents & ".shp"
            .out_feature_class = outputFolder & "\" & outputName & ".shp"
            .where_clause = eventFieldName & " = 1"
        End With
        result = GP.Execute(SelectTool, Nothing)
        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(SelectTool)

        If result Is Nothing Then
            MessageBox.Show("BA_SteamEnds Error: Unable to select stream end points from end nodes.")
            GP = Nothing
            Return BA_ReturnCode.UnknownError
            Exit Function
        End If

        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(GP)
        BA_Remove_Shapefile(outputFolder, tempNodeEvents)
        pDEUtility.ReleaseInternals()

        Return BA_ReturnCode.Success
    End Function

    'merge multiple polygon or polyline shapefiles and return a merged polygon shapefile
    'at least two input shapefiles are required, if there is no third files, enter a null string ("") as the input. 
    Public Function BA_MergeVectorsToPolygons(ByVal InputFolder As String, _
        ByVal Input1FileName As String, ByVal Input2FileName As String, ByVal Input3FileName As String, _
        ByVal outputFolder As String, ByVal outputFileName As String, _
        Optional ByVal KeepTempFiles As Boolean = False) As BA_ReturnCode

        Dim returnValue As BA_ReturnCode = BA_ReturnCode.UnknownError

        'this subroutine is based on GP
        Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
        Dim result As ESRI.ArcGIS.Geoprocessing.IGeoProcessorResult2 = Nothing

        'standardize filepath and filenames
        InputFolder = BA_StandardizePathString(InputFolder, False)
        outputFolder = BA_StandardizePathString(outputFolder, False)
        outputFileName = BA_StandardizeShapefileName(outputFileName, True, False)

        Dim inputFeatureclasses As String = ""
        Dim nInput As Integer = 0

        inputFeatureclasses = inputFeatureclasses & ";" & InputFolder & "\" & Input1FileName

        If Len(Input1FileName.Trim) > 0 Then
            Input1FileName = BA_StandardizeShapefileName(Input1FileName, True, False)
            inputFeatureclasses = InputFolder & "\" & Input1FileName
            nInput = nInput + 1
        End If

        If Len(Input2FileName.Trim) > 0 Then
            Input2FileName = BA_StandardizeShapefileName(Input2FileName, True, False)
            If Len(inputFeatureclasses) > 0 Then inputFeatureclasses = inputFeatureclasses & ";"
            inputFeatureclasses = inputFeatureclasses & InputFolder & "\" & Input2FileName
            nInput = nInput + 1
        End If

        If Len(Input3FileName.Trim) > 0 Then
            Input3FileName = BA_StandardizeShapefileName(Input3FileName, True, False)
            If Len(inputFeatureclasses) > 0 Then inputFeatureclasses = inputFeatureclasses & ";"
            inputFeatureclasses = inputFeatureclasses & InputFolder & "\" & Input3FileName
            nInput = nInput + 1
        End If

        If nInput < 2 Then 'at least two input shapefiles are required
            Return BA_ReturnCode.NotSupportedOperation
        End If

        ' Set default workspace
        GP.SetEnvironmentValue("workspace", outputFolder)
        GP.AddOutputsToMap = False
        GP.OverwriteOutput = True

        Dim Feat2PolygonTool As ESRI.ArcGIS.DataManagementTools.FeatureToPolygon = New ESRI.ArcGIS.DataManagementTools.FeatureToPolygon
        Dim RepairGeometryTool As ESRI.ArcGIS.DataManagementTools.RepairGeometry = New ESRI.ArcGIS.DataManagementTools.RepairGeometry
        'Dim infeatures As IGpObjectArray

        Try
            ' convert watershed polylines to polygons.
            With Feat2PolygonTool
                .in_features = inputFeatureclasses
                .out_feature_class = outputFileName
                .cluster_tolerance = 0 'prevent unintended snapping that creates "holes" in polygon feature class
            End With
            result = GP.Execute(Feat2PolygonTool, Nothing)

            Feat2PolygonTool = Nothing
            If result Is Nothing Then
                MessageBox.Show("BA_MergeVectorsToPolygons Error: Feature to Polygon tool execution failed.")
                GP = Nothing
                Return BA_ReturnCode.UnknownError
                Exit Function
            End If

            'repair geometry on the merged feature to prevent V2R conversion error
            With RepairGeometryTool
                .delete_null = True
                .in_features = outputFileName
                '.out_feature_class = outputFileName
            End With
            result = GP.Execute(RepairGeometryTool, Nothing)

            ' If the job failed, retrieve the feature result.
            RepairGeometryTool = Nothing
            If result Is Nothing Then
                MessageBox.Show("BA_MergeVectorsToPolygons Error: Unable to repair the geometry of " & outputFileName & ".")
                GP = Nothing
                Return BA_ReturnCode.UnknownError
            End If

            Return BA_ReturnCode.Success
        Catch ex As Exception
            Dim errMsg As String = "Contributing Area GP Error (BA_MergeVectorsToPolygons): "
            For counter As Integer = 0 To GP.MessageCount - 1
                errMsg = errMsg & vbCrLf & GP.GetMessage(counter)
            Next
            MsgBox(errMsg)
            Return BA_ReturnCode.UnknownError

        Finally
            'clean up temporary files
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(Feat2PolygonTool)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(RepairGeometryTool)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(result)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(GP)
        End Try
    End Function

    'end of the code for 3 zones subroutines

    '====================================================
    'beginning of the code for 1 zone subroutines
    Public Function BA_1AreaWatersheds(ByVal aoiFolder As String, _
                                           ByVal outputFolder As String, _
                                           ByVal outputName As String, _
                                           ByVal ContribRule As ContributingAreasRule) As BA_ReturnCode
        'Pass value in; inputFolderPath contains the inputFolderPath
        'Dim AOIFolder As String = ContribRule.InputFolderPath

        'determine threshold value
        If ContribRule.FACCThresholdValue <= 0 Then
            MessageBox.Show("Invalid threshold value. Please re-enter.")
            Return BA_ReturnCode.OtherError
            Exit Function
        End If

        Dim FAccStDev As Double = ContribRule.FACCStandardDeviation
        Dim FAccThreshold As Double = ContribRule.FACCThresholdValue  'use facc value directly, no longer use st dev as unit

        If BA_Streamlinks2Watersheds(aoiFolder, outputFolder, outputName, FAccThreshold, ContribRule.KeepTemporaryFiles) = BA_ReturnCode.Success Then
            Return BA_ReturnCode.Success
        Else
            Return BA_ReturnCode.UnknownError
        End If
    End Function

    Public Function BA_Streamlinks2Watersheds(ByVal aoiFolder As String, _
                                              ByVal outputFolder As String, _
                                              ByVal outputName As String, _
                                              ByVal FaccThreshold As Double, _
                                              ByVal KeepTempFiles As Boolean) As BA_ReturnCode
        'verify threshold value
        If FaccThreshold <= 0 Then
            MessageBox.Show("Invalid flow accumulation threshold value. Please re-enter.")
            Return BA_ReturnCode.OtherError
            Exit Function
        End If

        Dim pDEUtility As IDEUtilities = New DEUtilities
        'Use the AOI extent to limit the process extent
        Dim pAOIRaster As IGeoDataset = Nothing
        Dim pFAccGDS As IGeoDataset = Nothing
        Dim pFAccRasterDS As IRasterDataset = Nothing
        Dim pFAccRaster As IRaster = Nothing
        Dim pFDirGDS As IGeoDataset = Nothing
        Dim pExtractOp As IExtractionOp2 = New RasterExtractionOp
        Dim pQFilter As IQueryFilter = New QueryFilter
        Dim pRasDes As IRasterDescriptor = New RasterDescriptor
        Dim pStreamGDS As IGeoDataset = Nothing
        Dim pHydrologyOp As IHydrologyOp = New RasterHydrologyOp
        Dim pStreamlinkGDS As IGeoDataset = Nothing
        Dim pWatershedGDS As IGeoDataset = Nothing
        Dim rasterStat As IRasterStatistics = Nothing
        Dim shapefileFolder As String = ""
        Dim gdbName As String = BA_GetBareName(outputFolder, shapefileFolder)
        Dim gdbInputGDS As IGeoDataset = Nothing
        shapefileFolder = BA_StandardizePathString(shapefileFolder, False) 'this is where the temp shapefiles will be saved

        'note: GDB versus shapefile
        'all rasters are stored in GDB
        'all temp vectors are stored as shapefiles to prevent triggering of GP bugs related to tolerance

        Try
            'Open AOI Polygon to set the analysis mask
            Dim rasterFileName As String = BA_EnumDescription(PublicPath.AoiGrid)
            rasterFileName = BA_StandardizeShapefileName(rasterFileName, False, False)
            'pAOIRaster = BA_OpenRasterFromFile(aoiFolder, rasterFileName)
            pAOIRaster = BA_OpenRasterFromGDB(BA_GeodatabasePath(aoiFolder, GeodatabaseNames.Aoi), rasterFileName)
            If pAOIRaster Is Nothing Then
                MsgBox("Cannot locate AOI boundary raster in the AOI.  Please re-run AOI Tool.")
                Return BA_ReturnCode.ReadError
            End If

            'open flow_accum and flow direction raster
            Dim rasterfilepath As String
            rasterFileName = BA_EnumDescription(MapsFileName.flow_accumulation_gdb)
            rasterfilepath = BA_GeodatabasePath(aoiFolder, GeodatabaseNames.Surfaces)

            'pFAccGDS = BA_OpenRasterFromFile(rasterfilepath, rasterFileName)
            pFAccGDS = BA_OpenRasterFromGDB(rasterfilepath, rasterFileName)
            pFAccRasterDS = pFAccGDS 'qi
            pFAccRaster = pFAccRasterDS.CreateDefaultRaster

            rasterFileName = BA_EnumDescription(MapsFileName.flow_direction_gdb)
            pFDirGDS = BA_OpenRasterFromGDB(rasterfilepath, rasterFileName)

            'Query the Raster and Create a new dataset on the buffered FAcc layer
            'the buffering would ensure that stream links intersect with the watershed boundaries
            pQFilter.WhereClause = "Value > " & FaccThreshold
            pRasDes.Create(pFAccRaster, pQFilter, "Value")
            pStreamGDS = pExtractOp.Attribute(pRasDes)

            ''Run Extraction Operation to limit the analysis within the AOI
            pStreamlinkGDS = pHydrologyOp.StreamLink(pStreamGDS, pFDirGDS)

            Dim pTempRaster As IGeoDataset = pExtractOp.Raster(pStreamlinkGDS, pAOIRaster)
            pWatershedGDS = pHydrologyOp.Watershed(pFDirGDS, pTempRaster)
            pTempRaster = Nothing

            Dim response As Integer

            'Save temporary grid file; Cannot save output of pHydrologyOp directly to FGDB
            'this seems not the case on my computer, save pHydrologyOp output directly to FGDB
            response = BA_SaveRasterDatasetGDB(pWatershedGDS, outputFolder, BA_RASTER_FORMAT, outputName)
            'response = BA_ComputeStatsRasterDatasetGDB(outputFolder, outputName)

            ''Persist output of RasterHydrologyOp so we can access it with the GP to convert to Polygon
            'If BA_File_Exists(shapefileFolder & "\" & CA_1TempFile, WorkspaceType.Raster, esriDatasetType.esriDTRasterDataset) Then
            '    response = BA_Remove_Raster(shapefileFolder, CA_1TempFile)
            'End If

            'response = BA_SaveRasterDataset(pWatershedGDS, shapefileFolder, CA_1TempFile)

            'gdbInputGDS = BA_OpenRasterFromFile(shapefileFolder, CA_1TempFile)
            'If BA_File_Exists(outputFolder & "\" & outputName, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            '    response = BA_RemoveRasterFromGDB(outputFolder, outputName)
            'End If
            'response = BA_SaveRasterDatasetGDB(gdbInputGDS, outputFolder, BA_RASTER_FORMAT, outputName)
            'gdbInputGDS = Nothing

            'create/clean up temporary files. these files are used in 2 and 3 zones CA
            If KeepTempFiles Then
                'vectorize the watersheds
                If BA_Shapefile_Exists(shapefileFolder & "\" & WatershedFile) Then 'this function takes either file name with or without .shp extension
                    response = BA_Remove_Shapefile(shapefileFolder, WatershedFile) 'this function will append shp to the shapefilename
                End If

                Dim snapRasterPath As String = BA_GeodatabasePath(aoiFolder, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)

                'the watershed polgyon shapefile is used in 2 and 3 zone CA, not in the 1 zone CA
                Dim success As BA_ReturnCode = BA_Raster2Polygon_GP(outputFolder & "\" & outputName, shapefileFolder & "\" & CA_1AreaFile & ".shp", snapRasterPath)
                If success <> BA_ReturnCode.Success Then
                    MsgBox("BA_Streamlinks2Watersheds Error: Unable to save the temporary watershed file. Program stopped.")
                    Return BA_ReturnCode.WriteError
                End If

                'vectorize the streamlinks
                'check if output exist, if yes, remove it
                If BA_Shapefile_Exists(shapefileFolder & "\" & StreamLinkFile) Then
                    response = BA_Remove_Shapefile(shapefileFolder, StreamLinkFile)
                End If

                'the streamlink shapefile is used in 2 and 3 zone CA, not in the 1 zone CA
                response = BA_Raster2LineShapefile(pStreamlinkGDS, shapefileFolder, StreamLinkFile & ".shp", False)
                If response <> BA_ReturnCode.Success Then
                    MsgBox("BA_Streamlinks2Watersheds Error: Unable to save the temporary watershed file. Program stopped.")
                    Return BA_ReturnCode.WriteError
                End If

            Else
                'check if output exist, if yes, remove it
                If BA_Shapefile_Exists(shapefileFolder & "\" & StreamLinkFile) Then
                    response = BA_Remove_Shapefile(shapefileFolder, StreamLinkFile)
                End If

                If BA_Shapefile_Exists(shapefileFolder & "\" & CA_1AreaFile) Then
                    response = BA_Remove_Shapefile(shapefileFolder, CA_1AreaFile)
                End If
            End If

            Return BA_ReturnCode.Success
        Catch ex As Exception
            MessageBox.Show("BA_Streamlinks2Watersheds Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            pDEUtility.ReleaseInternals()
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pAOIRaster)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFAccRaster)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFAccRasterDS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFAccGDS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFDirGDS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pExtractOp)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pQFilter)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasDes)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pHydrologyOp)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStreamlinkGDS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWatershedGDS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(rasterStat)
        End Try

    End Function

    Public Function BA_UpdateShapefileID(ByVal Folder As String, ByVal FileName As String, Optional ByVal IDFieldName As String = "") As Long
        Dim FieldName As String = ""
        If IDFieldName = "" Then
            FieldName = "ID"
        Else
            FieldName = IDFieldName
        End If

        Dim pFClass As IFeatureClass = Nothing
        Dim pField As IFieldEdit2 = Nothing
        Dim pFCursor As IFeatureCursor = Nothing
        Dim pFeature As IFeature = Nothing

        Try
            pFClass = BA_OpenFeatureClassFromFile(Folder, FileName)
            If pFClass Is Nothing Then
                Return -1
            End If

            Dim fieldIndex As Long = pFClass.FindField(FieldName)

            If fieldIndex < 0 Then
                pField = New Field
                pField.Name_2 = FieldName
                pField.Type_2 = esriFieldType.esriFieldTypeInteger
                pField.Required_2 = False
                ' Add field

                pFClass.AddField(pField)
                fieldIndex = pFClass.FindField(FieldName)
            End If

            pFCursor = pFClass.Update(Nothing, False)
            pFeature = pFCursor.NextFeature

            Dim featureCount As Long = 0
            Do While Not pFeature Is Nothing
                featureCount += 1
                pFeature.Value(fieldIndex) = featureCount
                pFCursor.UpdateFeature(pFeature)
                pFeature = pFCursor.NextFeature
            Loop
            Return featureCount

        Catch ex As Exception
            MessageBox.Show("BA_UpdateShapefileID Exception: " & ex.Message)
            Return -1
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFClass)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pField)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeature)
        End Try

    End Function

    'Convert a raster dataset to vector
    'The workspace needs to be created first.
    'return value: 0 error occurred
    '              1 successfully converted the raster and saved the vector
    Public Function BA_Raster2LineShapefile(ByVal InRaster As IGeoDataset, ByVal ShapefilePath As String, _
        ByVal shapefilename As String, Optional ByVal SmoothLine As Boolean = True) As Integer
        Dim return_value As BA_ReturnCode = BA_ReturnCode.UnknownError

        'verfify input
        If InRaster Is Nothing Or (Len(ShapefilePath) * Len(shapefilename) = 0) Then
            BA_Raster2LineShapefile = BA_ReturnCode.ReadError
            Exit Function
        End If

        Dim pWSF As IWorkspaceFactory = Nothing
        Dim pWS As IFeatureWorkspace
        Dim pConversionOp As IConversionOp = New RasterConversionOp
        Dim pFClass As IFeatureClass

        Try
            Dim wksType As WorkspaceType = BA_GetWorkspaceTypeFromPath(ShapefilePath)
            If wksType = WorkspaceType.Raster Then
                pWSF = New ShapefileWorkspaceFactory
            ElseIf wksType = WorkspaceType.Geodatabase Then
                pWSF = New FileGDBWorkspaceFactory()
            End If
            pWS = pWSF.OpenFromFile(ShapefilePath, 0)
            ' Calls function to open a feature class from disk
            pFClass = pConversionOp.RasterDataToLineFeatureData(InRaster, pWS, shapefilename, True, SmoothLine, 0)
            return_value = BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_Raster2LineShapefile exception: " & ex.Message)
            return_value = BA_ReturnCode.WriteError
        Finally
            pFClass = Nothing
            pWS = Nothing
            pWSF = Nothing
            pConversionOp = Nothing
            BA_Raster2LineShapefile = return_value
        End Try
    End Function

    'end of the code for 1 zone subroutines

    '====================================================
    'beginning of the code for 2 zones subroutines
    '2 zones CA is formed by 1. 1 zone CA boundaries, 2. streamlinks, and 3. divides of the headwatersheds
    Public Function BA_2AreasWatersheds(ByVal aoiFolder As String, _
                                      ByVal outputFolder As String, _
                                      ByVal outputName As String, _
                                      ByVal ContribRule As ContributingAreasRule) As BA_ReturnCode

        'temporary file names
        Dim linkNodesFile As String = "linknodes"
        Dim costPathShapefile As String = "costPath_v"
        Dim headPointsShapefile As String = "headpoints_v"
        Dim CA_2AreaFileTemp As String = "ContributingArea_2Temp"
        Dim CA_2AreaFileTemp1 As String = "CA_2Temp1"

        'verify threshold value
        If ContribRule.FACCThresholdValue <= 0 Then
            MessageBox.Show("Invalid flow accumulation threshold value. Please re-enter.")
            Return BA_ReturnCode.OtherError
            Exit Function
        End If

        Dim pDEUtility As IDEUtilities = New DEUtilities
        'Use the AOI extent to limit the process extent
        Dim pAOIRaster As IGeoDataset = Nothing
        Dim pFAccGDS As IGeoDataset = Nothing
        Dim pFAccRasterDS As IRasterDataset = Nothing
        Dim pFAccRaster As IRaster = Nothing
        Dim pFDirGDS As IGeoDataset = Nothing
        Dim pExtractOp As IExtractionOp2 = New RasterExtractionOp
        Dim pQFilter As IQueryFilter = New QueryFilter
        Dim pRasDes As IRasterDescriptor = New RasterDescriptor
        Dim pStreamGDS As IGeoDataset = Nothing
        Dim pFlowLenGDS As IGeoDataset = Nothing
        Dim pHydrologyOp As IHydrologyOp = New RasterHydrologyOp
        Dim pStreamlinkGDS As IGeoDataset = Nothing
        Dim pWatershedGDS As IGeoDataset = Nothing
        Dim rasterStat As IRasterStatistics = Nothing

        'the outputFolder points to a gdb, create a string variable to hold the folder containing the gdb for saving shapefiles
        'note: GDB versus shapefile
        'all rasters are stored in GDB
        'all temp vectors are stored as shapefiles to prevent triggering of GP bugs related to tolerance
        Dim shapefileFolder As String = ""
        Dim gdbName As String = BA_GetBareName(outputFolder, shapefileFolder)
        Dim gdbInputGDS As IGeoDataset = Nothing
        shapefileFolder = BA_StandardizePathString(shapefileFolder, False) 'this is where the temp shapefiles will be saved

        Dim response As Integer
        Dim FAccThreshold As Double = ContribRule.FACCThresholdValue 'use facc value directly, no longer use st dev as unit
        Dim cellSize As Double = 0

        Dim surfaceFilepath As String = BA_GeodatabasePath(aoiFolder, GeodatabaseNames.Surfaces, True)
        Dim flowLenFileName As String = BA_EnumDescription(MapsFileName.flow_length_gdb)
        Dim FlowDirFileName As String = BA_EnumDescription(MapsFileName.flow_direction_gdb)
        Dim FlowAccFileName As String = BA_EnumDescription(MapsFileName.flow_accumulation_gdb)
        Dim AOIRasterFileName As String = BA_EnumDescription(PublicPath.AoiGrid)
        AOIRasterFileName = BA_StandardizeShapefileName(AOIRasterFileName, False, False)

        Try
            'check the required data
            'AOI grid
            'Open AOI Polygon to set the analysis mask
            pAOIRaster = BA_OpenRasterFromGDB(BA_GeodatabasePath(aoiFolder, GeodatabaseNames.Aoi), AOIRasterFileName)
            If pAOIRaster Is Nothing Then
                MsgBox("BA_2AreasWatersheds: Cannot locate AOI boundary raster in the AOI.  Please re-run AOI Tool.")
                Return BA_ReturnCode.ReadError
            End If

            'flow direction
            pFDirGDS = BA_OpenRasterFromGDB(surfaceFilepath, FlowDirFileName)
            If pFDirGDS Is Nothing Then
                MsgBox("BA_2AreasWatersheds: Cannot locate flow direction raster in the AOI.  Please re-run AOI Tool.")
                Return BA_ReturnCode.ReadError
            End If

            'create a flowlength raster in the surface gdb
            If BA_File_Exists(surfaceFilepath & "\" & flowLenFileName, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                pFlowLenGDS = BA_OpenRasterFromGDB(surfaceFilepath, flowLenFileName)
            Else
                pFlowLenGDS = pHydrologyOp.FlowLength(pFDirGDS, True)
                response = BA_SaveRasterDatasetGDB(pFlowLenGDS, surfaceFilepath, BA_RASTER_FORMAT, flowLenFileName)
                response = BA_ComputeStatsRasterDatasetGDB(surfaceFilepath, flowLenFileName)
            End If

            pFlowLenGDS = Nothing

            'create streamlinks using flow accumulation
            pFAccGDS = BA_OpenRasterFromGDB(surfaceFilepath, FlowAccFileName)
            If pFAccGDS Is Nothing Then
                MsgBox("BA_2AreasWatersheds: Cannot locate flow accumulation raster in the AOI.  Please re-run AOI Tool.")
                Return BA_ReturnCode.ReadError
            End If
            pFAccRasterDS = pFAccGDS
            pFAccRaster = pFAccRasterDS.CreateDefaultRaster

            'first create 1-zone watersheds
            'Query the Raster and Create a new dataset on the buffered FAcc layer
            'the buffering would ensure that stream links intersect with the watershed boundaries
            pQFilter.WhereClause = "Value > " & FAccThreshold
            pRasDes.Create(pFAccRaster, pQFilter, "Value")
            pStreamGDS = pExtractOp.Attribute(pRasDes)
            pStreamlinkGDS = pHydrologyOp.StreamLink(pStreamGDS, pFDirGDS)

            'vectorize the streamlinks
            'check if output exist, if yes, remove it
            If BA_Shapefile_Exists(shapefileFolder & "\" & StreamLinkFile) Then
                response = BA_Remove_Shapefile(shapefileFolder, StreamLinkFile)
            End If

            response = BA_Raster2LineShapefile(pStreamlinkGDS, shapefileFolder, StreamLinkFile & ".shp", False)
            If response <> BA_ReturnCode.Success Then
                MsgBox("BA_2AreasWatersheds Error: Unable to save the streamlink file. Rule not implemented.")
                Return BA_ReturnCode.WriteError
            End If

            'Run Extraction Operation to limit the analysis within the AOI
            Dim pTempRaster As IGeoDataset = pExtractOp.Raster(pStreamlinkGDS, pAOIRaster)
            pWatershedGDS = pHydrologyOp.Watershed(pFDirGDS, pTempRaster)
            pTempRaster = Nothing

            'Save temporary grid file; Cannot save output of pHydrologyOp directly to FGDB
            'this seems not the case on my computer, save pHydrologyOp output directly to FGDB
            response = BA_SaveRasterDatasetGDB(pWatershedGDS, outputFolder, BA_RASTER_FORMAT, WatershedRaster)

            pWatershedGDS = Nothing
            pFAccGDS = Nothing
            pFAccRaster = Nothing
            pFAccRasterDS = Nothing
            pFDirGDS = Nothing
            pFlowLenGDS = Nothing

            'vectorize the 1 zone watershed raster
            'check if output vector exist, if yes, remove it
            If BA_Shapefile_Exists(shapefileFolder & "\" & CA_1AreaFile) Then
                response = BA_Remove_Shapefile(shapefileFolder, CA_1AreaFile)
            End If

            Dim inRasterPath As String = outputFolder & "\" & WatershedRaster
            Dim outFeaturesPath As String = shapefileFolder & "\" & CA_1AreaFile & ".shp"

            Dim snapRasterPath As String = BA_GeodatabasePath(aoiFolder, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
            Dim success As BA_ReturnCode = BA_Raster2Polygon_GP(inRasterPath, outFeaturesPath, snapRasterPath)
            If success <> BA_ReturnCode.Success Then
                MsgBox("BA_2AreasWatersheds Error: Unable to save the temporary end-watershed file. Program stopped.")
                Return BA_ReturnCode.WriteError
            End If

            'extract end nodes from the streamlinks
            BA_StreamEnds(aoiFolder, shapefileFolder, StreamLinkFile, shapefileFolder, linkNodesFile)

            'use stream end nodes to create head watersheds, headpoints, and cost paths from the headpoints to the AOI pourpoint
            'the headPointsShapefile is a vector delineating the headpoint pixels and is used to prevent cost path not connecting to the head watershed boundaries
            BA_CreateCostPath(aoiFolder, shapefileFolder, linkNodesFile, outputFolder, costPathShapefile, headPointsShapefile)

            'merge the 1-area contributing watersheds with the costpath polyline featureclass
            'check if output exist, if yes, remove it
            If BA_Shapefile_Exists(shapefileFolder & "\" & CA_2AreaFileTemp) Then
                response = BA_Remove_Shapefile(shapefileFolder, CA_2AreaFileTemp)
            End If

            If BA_Shapefile_Exists(shapefileFolder & "\" & CA_2AreaFileTemp1) Then
                response = BA_Remove_Shapefile(shapefileFolder, CA_2AreaFileTemp1)
            End If

            ' merge watershed polygons, costpath, and headpoint polygons (headpoint polygons are to ensure no gaps between the headpoint and the head watershed boundaries)
            'If BA_MergeVectorsToPolygons(shapefileFolder, headPointsShapefile, costPathShapefile, "", shapefileFolder, CA_2AreaFileTemp1, False) <> BA_ReturnCode.Success Then Return BA_ReturnCode.NotSupportedOperation
            If BA_MergeVectorsToPolygons(shapefileFolder, CA_1AreaFile, headPointsShapefile, costPathShapefile, shapefileFolder, CA_2AreaFileTemp1, False) <> BA_ReturnCode.Success Then Return BA_ReturnCode.NotSupportedOperation

            Dim statResults As BA_DataStatistics
            Dim success2 As Integer
            statResults.Minimum = maximumSliverSize + 1
            'eliminate sliver polygons (i.e., area < 5 30-meter pixels or 4500 m^2)
            success = BA_AddShapeAreaToAttrib(shapefileFolder & "\" & CA_2AreaFileTemp1)
            success2 = BA_GetDataStatistics(shapefileFolder & "\" & CA_2AreaFileTemp1 & ".shp", BA_FIELD_AREA_SQKM, statResults)

            Dim maxiteration As Integer = 3
            Dim ncount As Integer = 1
            Do While success2 = 0 And statResults.Minimum < maximumSliverSize
                If BA_Shapefile_Exists(shapefileFolder & "\" & CA_2AreaFileTemp) Then
                    response = BA_Remove_Shapefile(shapefileFolder, CA_2AreaFileTemp)
                End If
                'MsgBox("Debug message: Slivers need to be eliminated!")
                BA_EliminatePoly(shapefileFolder, CA_2AreaFileTemp1 & ".shp", shapefileFolder, CA_2AreaFileTemp & ".shp", "AREA", maximumSliverSize, BA_FIELD_AREA_SQKM)
                response = BA_Remove_Shapefile(shapefileFolder, CA_2AreaFileTemp1)

                statResults.Minimum = maximumSliverSize + 1
                success = BA_AddShapeAreaToAttrib(shapefileFolder & "\" & CA_2AreaFileTemp)
                success2 = BA_GetDataStatistics(shapefileFolder & "\" & CA_2AreaFileTemp & ".shp", BA_FIELD_AREA_SQKM, statResults)

                ncount += 1
                If ncount > maxiteration Then success2 = 1 'this prevents infinite loops

                If success2 = 0 And statResults.Minimum < maximumSliverSize Then
                    response = BA_CopyFeatures(shapefileFolder & "\" & CA_2AreaFileTemp & ".shp", shapefileFolder & "\" & CA_2AreaFileTemp1 & ".shp")
                End If
            Loop

            'convert the vector HRU to raster
            'prepare attribute field for rasterization
            Dim featureCount As Long = 0
            Dim attributeFieldName As String = "ID"
            featureCount = BA_UpdateShapefileID(shapefileFolder, CA_2AreaFileTemp, attributeFieldName)

            If featureCount <= 0 Then
                MessageBox.Show("BA_2AreasWatersheds Error: No polygon in the merged watershed shapefile!")
                Return BA_ReturnCode.ReadError
            End If

            'convert vector to raster
            rasterStat = BA_GetRasterStatsGDB(surfaceFilepath & "\" & BA_EnumDescription(MapsFileName.filled_dem_gdb), cellSize)
            rasterStat = Nothing

            Dim vInputPath As String = shapefileFolder & "\" & CA_2AreaFileTemp & ".shp"
            Dim tempRasterName As String = "tempR001"
            Dim outRasterPath As String = outputFolder & "\" & tempRasterName

            If BA_File_Exists(outputFolder & "\" & tempRasterName, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                BA_RemoveRasterFromGDB(outputFolder, tempRasterName)
            End If

            success = BA_Feature2RasterGP(vInputPath, outRasterPath, attributeFieldName, cellSize, snapRasterPath)

            'Removing NoData from the output raster
            Dim maskFolder As String = BA_GeodatabasePath(aoiFolder, GeodatabaseNames.Aoi, True)
            Dim maskFile As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.AoiVector), False)

            If BA_RasterHasNodata(aoiFolder, outputFolder, tempRasterName) Then
                If BA_RemNodataFromRas(outputFolder, tempRasterName, maskFolder, maskFile, _
                                       outputFolder, outputName, snapRasterPath, aoiFolder) <> BA_ReturnCode.Success Then
                    Return BA_ReturnCode.UnknownError
                    Exit Function
                End If
                BA_RemoveRasterFromGDB(outputFolder, tempRasterName)
            Else
                ' Rename output of filter to outfile name if we don't have to remove NoData values
                BA_RenameRasterInGDB(outputFolder, tempRasterName, outputName)
            End If

            'the temp file is saved as a shapefile, instead of a feature class in GDB, to prevent issues with GDB's min tolerance limit 
            'remove temp file
            If BA_Shapefile_Exists(shapefileFolder & "\" & CA_2AreaFileTemp1) Then
                response = BA_Remove_Shapefile(shapefileFolder, CA_2AreaFileTemp1)
            End If
            response = BA_RemoveRasterFromGDB(outputFolder, WatershedRaster)
            response = BA_Remove_Shapefile(shapefileFolder, CA_1AreaFile)

            'clean up temporary files
            If Not ContribRule.KeepTemporaryFiles Then
                response = BA_Remove_Shapefile(shapefileFolder, headPointsShapefile)
                response = BA_Remove_Shapefile(shapefileFolder, costPathShapefile)
                response = BA_Remove_Shapefile(shapefileFolder, CA_2AreaFileTemp)

                'check if output exist, if yes, remove it
                If BA_Shapefile_Exists(shapefileFolder & "\" & StreamLinkFile) Then
                    response = BA_Remove_Shapefile(shapefileFolder, StreamLinkFile)
                End If

                If BA_Shapefile_Exists(shapefileFolder & "\" & linkNodesFile) Then
                    response = BA_Remove_Shapefile(shapefileFolder, linkNodesFile)
                End If
            End If

            Return BA_ReturnCode.Success

        Catch ex As Exception
            MessageBox.Show("BA_2AreasWatersheds Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError

        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pAOIRaster)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFAccRaster)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFAccRasterDS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFAccGDS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFDirGDS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFlowLenGDS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pExtractOp)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pQFilter)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasDes)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pHydrologyOp)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStreamlinkGDS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWatershedGDS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(rasterStat)
            pDEUtility.ReleaseInternals()
        End Try
    End Function

    'trace the head watershed dividess using head watersheds' pourpoints as the starting points.
    'The output is a polyline shapefile that can be saved in the same location as the pourpoint feature class.
    Public Function BA_CreateCostPath(ByVal aoiFolder As String, ByVal PourPntFolder As String, ByVal PourPntName As String, _
                        ByVal tempGDBPath As String, ByVal pathShapefileName As String, ByVal headShapefileName As String) As BA_ReturnCode
        Dim headZonesRaster As String = "headzones"
        Dim headZonesStats As String = "headzonemaxflen"
        Dim headZonesRndStats As String = "headzonemaxrnd"
        Dim headPointsRaster As String = "headpoints"
        Dim costPathRaster As String = "costpath"
        Dim randomRaster As String = "random_ras"

        Dim pDEUtility As IDEUtilities = New DEUtilities
        Dim layerFileName As String

        'set aoi, flow length, and flow direction raster file path
        layerFileName = BA_EnumDescription(PublicPath.AoiGrid)
        Dim snapRasterPath As String = BA_GeodatabasePath(aoiFolder, GeodatabaseNames.Aoi) & layerFileName

        layerFileName = BA_EnumDescription(MapsFileName.flow_direction_gdb)
        Dim FDirfilePathName As String = BA_GeodatabasePath(aoiFolder, GeodatabaseNames.Surfaces, True) & layerFileName

        layerFileName = BA_EnumDescription(MapsFileName.flow_length_gdb)
        Dim FLenfilePathName As String = BA_GeodatabasePath(aoiFolder, GeodatabaseNames.Surfaces, True) & layerFileName

        'Use the stream ends to delineate head watersheds
        BA_CreateWatershedsfromPourpoints(aoiFolder, PourPntFolder, PourPntName, tempGDBPath, headZonesRaster, 0)

        Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
        Dim result As ESRI.ArcGIS.Geoprocessing.IGeoProcessorResult2

        ' Set default workspace
        GP.SetEnvironmentValue("workspace", tempGDBPath)
        GP.SetEnvironmentValue("mask", snapRasterPath)
        GP.SetEnvironmentValue("snapRaster", snapRasterPath)
        GP.AddOutputsToMap = False
        GP.OverwriteOutput = True

        Try
            'find the max of flowlength in each headzone
            Dim zonaltool As ZonalStatistics = New ZonalStatistics
            With zonaltool
                .in_value_raster = FLenfilePathName
                .in_zone_data = tempGDBPath & "\" & headZonesRaster
                .zone_field = "Value"
                .out_raster = tempGDBPath & "\" & headZonesStats
                .statistics_type = "MAXIMUM"
            End With

            result = GP.Execute(zonaltool, Nothing)

            ' If the job failed, retrieve the feature result.
            If result Is Nothing Then
                MessageBox.Show("BA_CreateCostPath Error: ZonalStatistics execution failed. Unable to find the max flow-acc values in each head watershed.")
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(GP)
                Return BA_ReturnCode.UnknownError
                Exit Function
            End If

            Dim mapAlgebraOp As IMapAlgebraOp = New RasterMapAlgebraOp
            Dim rasMakerOp As IRasterMakerOp = New RasterMakerOp

            Dim inGeoFLen As IGeoDataset = Nothing
            Dim inRasFLen As IRasterDataset = Nothing
            Dim inGeoMaxFLen As IGeoDataset = Nothing
            Dim inRasMaxFlen As IRasterDataset = Nothing
            Dim inGeoHdZone As IGeoDataset = Nothing
            Dim inRasHdZone As IRasterDataset = Nothing

            Dim env As IRasterAnalysisEnvironment = Nothing
            Dim wksFactory As IWorkspaceFactory = New FileGDBWorkspaceFactory()

            Dim workspace As IWorkspace = Nothing
            Dim rasOut As IRaster = Nothing

            Try
                'Get rasters.
                inGeoFLen = BA_OpenRasterFromGDB(BA_GeodatabasePath(aoiFolder, GeodatabaseNames.Surfaces, True), BA_EnumDescription(MapsFileName.flow_length_gdb))
                inRasFLen = CType(inGeoFLen, IRasterDataset3) ' Explicit cast
                inGeoMaxFLen = BA_OpenRasterFromGDB(tempGDBPath, headZonesStats)
                inRasMaxFlen = CType(inGeoMaxFLen, IRasterDataset3) ' Explicit cast
                inGeoHdZone = BA_OpenRasterFromGDB(tempGDBPath, headZonesRaster)
                inRasHdZone = CType(inGeoHdZone, IRasterDataset3) ' Explicit cast

                'Set environment.
                env = CType(mapAlgebraOp, IRasterAnalysisEnvironment) ' Explicit cast
                workspace = wksFactory.OpenFromFile(tempGDBPath, 0)
                env.OutWorkspace = workspace
                'Bind rasters.
                mapAlgebraOp.BindRaster(inRasFLen, "flow_len")
                mapAlgebraOp.BindRaster(inRasMaxFlen, "maxflow_len")
                mapAlgebraOp.BindRaster(inRasHdZone, "headzone")
                'Execute script.
                rasOut = mapAlgebraOp.Execute("Con([flow_len] == [maxflow_len],[headzone])")
                If rasOut IsNot Nothing Then
                    If BA_SaveRasterDatasetGDB(rasOut, tempGDBPath, BA_RASTER_FORMAT, headPointsRaster) <> 1 Then
                        MsgBox("BA_CreateCostPath Error: Unable to save head points to GDB.")
                    End If
                Else
                    MsgBox("BA_CreateCostPath Error: con model error.")
                End If

            Catch ex As Exception
                MsgBox("BA_CreateCostPath Error: " & ex.Message)
                Return BA_ReturnCode.OtherError

            Finally
                inGeoFLen = Nothing
                inRasFLen = Nothing
                inGeoMaxFLen = Nothing
                inRasMaxFlen = Nothing
                inGeoHdZone = Nothing
                inRasHdZone = Nothing
                env = Nothing
                workspace = Nothing
                mapAlgebraOp = Nothing
                rasOut = Nothing

            End Try

            'need to make sure that there is only one headpoint in each head watershed
            'open the attribute table of headZonesRaster and check if any COUNT value > 1
            Dim pRDataset As IGeoDataset = BA_OpenRasterFromGDB(tempGDBPath, headPointsRaster)
            Dim pBandCol As IRasterBandCollection = CType(pRDataset, IRasterBandCollection)
            Dim pRasterBand As IRasterBand = pBandCol.Item(0)
            Dim rAttTable As ITable = pRasterBand.AttributeTable

            Dim moreThanoneHeadPoint As Boolean = False
            Dim headZoneList As New List(Of Integer)

            If rAttTable IsNot Nothing Then
                Dim countIndex As Integer = rAttTable.FindField("COUNT")
                Dim valueIndex As Integer = rAttTable.FindField("VALUE")
                Dim pCursor As ICursor = rAttTable.Search(Nothing, False)
                Dim pRow As IRow = pCursor.NextRow

                Do While pRow IsNot Nothing
                    If pRow.Value(countIndex) > 1 Then
                        headZoneList.Add(pRow.Value(valueIndex))
                        moreThanoneHeadPoint = True
                    End If
                    pRow = pCursor.NextRow
                Loop

                pCursor = Nothing
                rAttTable = Nothing
            Else
                MsgBox("Unable to open the attribute table of " & headPointsRaster)
            End If

            pBandCol = Nothing
            pRasterBand = Nothing

            'if more than one headpoint in any of the head watersheds, then remove the extra points to ensure that only one headpoint in each head watershed
            If moreThanoneHeadPoint Then
                Dim rasterStat As IRasterStatistics = Nothing
                Dim cellSize As Double = 0

                'convert vector to raster
                rasterStat = BA_GetRasterStatsGDB(BA_GeodatabasePath(aoiFolder, GeodatabaseNames.Surfaces, True) & BA_EnumDescription(MapsFileName.filled_dem_gdb), cellSize)
                rasterStat = Nothing

                Dim extentString As String = pRDataset.Extent.XMin & " " & pRDataset.Extent.YMin & " " & pRDataset.Extent.XMax & " " & pRDataset.Extent.YMax
                ' Set default workspace
                Dim maskRasterPath As String = tempGDBPath & "\" & headPointsRaster

                GP.SetEnvironmentValue("workspace", tempGDBPath)
                GP.SetEnvironmentValue("mask", maskRasterPath)
                GP.SetEnvironmentValue("snapRaster", snapRasterPath)
                GP.AddOutputsToMap = False
                GP.OverwriteOutput = True

                'generate random rasters on headpoint locations
                Try

                    Dim randomtool As ESRI.ArcGIS.SpatialAnalystTools.CreateRandomRaster = New ESRI.ArcGIS.SpatialAnalystTools.CreateRandomRaster
                    With randomtool
                        '.extent = tempGDBPath & "\" & headPointsRaster
                        .cell_size = cellSize
                        .seed_value = 1
                        .extent = extentString
                        .out_raster = tempGDBPath & "\" & randomRaster
                    End With

                    result = GP.Execute(randomtool, Nothing)
                    ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(randomtool)

                    ' If the job failed, retrieve the feature result.
                    If result Is Nothing Then
                        MessageBox.Show("BA_CreateCostPath Error: con execution failed. Unable to create the random raster.")
                        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(GP)
                        Return BA_ReturnCode.UnknownError
                        Exit Function
                    End If

                Catch ex As Exception
                    MsgBox("BA_CreateCostPath Error: " & ex.Message)
                    Return BA_ReturnCode.OtherError

                End Try

                'perform zonalmax on the random raster and select the points that have the largest random values as the head point in each head watershed
                Try
                    With zonaltool
                        .in_value_raster = tempGDBPath & "\" & randomRaster
                        .in_zone_data = tempGDBPath & "\" & headZonesRaster
                        .zone_field = "Value"
                        .out_raster = tempGDBPath & "\" & headZonesRndStats
                        .statistics_type = "MAXIMUM"
                    End With

                    result = GP.Execute(zonaltool, Nothing)

                    ' If the job failed, retrieve the feature result.
                    If result Is Nothing Then
                        MessageBox.Show("BA_CreateCostPath Error: ZonalStatistics execution failed. Unable to find the max random values in each head watershed.")
                        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(GP)
                        Return BA_ReturnCode.UnknownError
                        Exit Function
                    End If

                Catch ex As Exception
                    MsgBox("BA_CreateCostPath Error: " & ex.Message)
                    Return BA_ReturnCode.OtherError

                End Try

                'create unique head watershed points by selecting the headpoints with the max random values within each zone
                Dim inGeoRnd As IGeoDataset = Nothing
                Dim inRasRnd As IRasterDataset = Nothing
                Dim inGeoMaxRnd As IGeoDataset = Nothing
                Dim inRasMaxRnd As IRasterDataset = Nothing
                'Dim inGeoHdZone As IGeoDataset = Nothing
                'Dim inRasHdZone As IRasterDataset = Nothing

                'GP = Nothing 'release GP before deleting the maskPointRaster

                'remove the original headpointRaster before generating the final unique headpointRaster
                BA_RemoveRasterFromGDB(tempGDBPath, headPointsRaster)

                Try
                    'Get rasters.
                    inGeoRnd = BA_OpenRasterFromGDB(tempGDBPath, randomRaster)
                    inRasRnd = CType(inGeoRnd, IRasterDataset3) ' Explicit cast
                    inGeoMaxRnd = BA_OpenRasterFromGDB(tempGDBPath, headZonesRndStats)
                    inRasMaxRnd = CType(inGeoMaxRnd, IRasterDataset3) ' Explicit cast
                    inGeoHdZone = BA_OpenRasterFromGDB(tempGDBPath, headZonesRaster)
                    inRasHdZone = CType(inGeoHdZone, IRasterDataset3) ' Explicit cast

                    'Set environment.
                    mapAlgebraOp = New RasterMapAlgebraOp
                    env = CType(mapAlgebraOp, IRasterAnalysisEnvironment) ' Explicit cast
                    workspace = wksFactory.OpenFromFile(tempGDBPath, 0)
                    env.OutWorkspace = workspace

                    'Bind rasters.
                    mapAlgebraOp.BindRaster(inRasRnd, "hzrnd")
                    mapAlgebraOp.BindRaster(inRasMaxRnd, "hzmaxrnd")
                    mapAlgebraOp.BindRaster(inRasHdZone, "headzone")

                    'Execute script.
                    rasOut = mapAlgebraOp.Execute("Con([hzrnd] == [hzmaxrnd],[headzone])")
                    If rasOut IsNot Nothing Then
                        If BA_SaveRasterDatasetGDB(rasOut, tempGDBPath, BA_RASTER_FORMAT, headPointsRaster) <> 1 Then
                            MsgBox("BA_CreateCostPath Error: Unable to save head points to GDB.")
                        End If
                    Else
                        MsgBox("BA_CreateCostPath Error: con model error.")
                    End If

                Catch ex As Exception
                    MsgBox("BA_CreateCostPath Error: " & ex.Message)
                    Return BA_ReturnCode.OtherError

                Finally
                    inGeoRnd = Nothing
                    inRasRnd = Nothing
                    inGeoMaxRnd = Nothing
                    inRasMaxRnd = Nothing
                    inGeoHdZone = Nothing
                    inRasHdZone = Nothing
                    env = Nothing
                    workspace = Nothing
                    mapAlgebraOp = Nothing
                    rasOut = Nothing

                End Try

                'remove temp rasters
                BA_RemoveRasterFromGDB(tempGDBPath, randomRaster)
                BA_RemoveRasterFromGDB(tempGDBPath, headZonesRndStats)
            End If

            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(zonaltool)
            pRDataset = Nothing

            'create a vector polygon version of the headpoints
            'check if output vector exist, if yes, remove it
            If BA_Shapefile_Exists(PourPntFolder & "\" & headShapefileName) Then
                BA_Remove_Shapefile(PourPntFolder, headShapefileName)
            End If

            Dim inRasterPath As String = tempGDBPath & "\" & headPointsRaster
            Dim outFeaturesPath As String = PourPntFolder & "\" & headShapefileName & ".shp"

            Dim success As BA_ReturnCode = BA_Raster2Polygon_GP(inRasterPath, outFeaturesPath, snapRasterPath)
            If success <> BA_ReturnCode.Success Then
                MsgBox("BA_CreateCostPath Error: Unable to create a vector version of the headpoints.")
                Return BA_ReturnCode.WriteError
            End If

            'find the cost path between headpoints and the AOI pourpoint using flowlength as cost
            Dim outCostPath As String = tempGDBPath & "\" & costPathRaster
            Dim costpathtool As CostPath = New CostPath
            With costpathtool
                .in_destination_data = tempGDBPath & "\" & headPointsRaster
                .in_cost_distance_raster = FLenfilePathName
                .in_cost_backlink_raster = FDirfilePathName
                .out_raster = outCostPath
            End With

            result = GP.Execute(costpathtool, Nothing)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(costpathtool)

            ' If the job failed, retrieve the feature result.
            If result Is Nothing Then
                MessageBox.Show("BA_CreateCostPath Error: CostPath execution failed. Unable to find the cost path of the headpoints to AOI pourpoint.")
                Return BA_ReturnCode.UnknownError
            End If

            'vectorize the costpath raster
            'check if output vector exist, if yes, remove it
            If BA_Shapefile_Exists(PourPntFolder & "\" & pathShapefileName) Then
                BA_Remove_Shapefile(PourPntFolder, pathShapefileName)
            End If

            Dim inRaster As IGeoDataset = BA_OpenRasterFromGDB(tempGDBPath, costPathRaster)
            success = BA_Raster2LineShapefile(inRaster, PourPntFolder, BA_StandardizeShapefileName(pathShapefileName, True, False), False)
            If success <> BA_ReturnCode.Success Then
                MsgBox("BA_CreateCostPath Error: Unable to save the costpath file.")
                Return BA_ReturnCode.WriteError
            End If
            inRaster = Nothing

            'clean up the temp layers
            BA_RemoveRasterFromGDB(tempGDBPath, costPathRaster)
            BA_RemoveRasterFromGDB(tempGDBPath, headPointsRaster)
            BA_RemoveRasterFromGDB(tempGDBPath, headZonesStats)
            BA_RemoveRasterFromGDB(tempGDBPath, headZonesRaster)

            Return BA_ReturnCode.Success
        Catch ex As Exception
            MsgBox("BA_CreateCostPath Error: " & ex.Message)
            Return BA_ReturnCode.OtherError
        Finally
            result = Nothing
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(GP)
            pDEUtility.ReleaseInternals()
        End Try
    End Function


    Public Function BA_CreatePointShapefile(ByVal pSpatialRef As ISpatialReference, ByVal savePath As String, ByVal strName As String, ByVal SpatialBoundary As IFeatureClass, ByVal addExtent As Boolean) As IFeatureClass
        'BA_CreatePointShapefile is a custom public function that was desgined for the BA_Watershed Function: Watershed #3.
        'this function creates a blank point shapefile and populates the first 4 features with points that represent the spatial extent of the previously generated watershed.
        'the reason why these 4 points are generated is to create a perfect match for Watershed #1 and Watershed #3 to union.

        'Determine if file already exists
        Dim CheckPath As String
        Dim response As Integer

        CheckPath = savePath & "\" & strName & ".shp"
        'Check if Watershed Vector Exists and delete if does
        If BA_File_ExistsIDEUtil(CheckPath) Then
            'Remove from Maps
            response = BA_RemoveLayers(My.ArcMap.Document, strName)
            'Delete Shapefile from Drive
            response = BA_Remove_Shapefile(savePath, strName)
            If response = 0 Then
                'Report Error
                MsgBox("Unable to delete the previous Watershed raster.")
            End If
        End If

        ' Open the folder to contain the shapefile as a workspace
        Dim pFWS As IFeatureWorkspace
        Dim pWorkspaceFactory As IWorkspaceFactory
        pWorkspaceFactory = New ShapefileWorkspaceFactory
        pFWS = pWorkspaceFactory.OpenFromFile(savePath, 0)

        ' Set up fields
        Dim pFields As IFields
        Dim pFieldsEdit As IFieldsEdit
        Dim pField As IField
        Dim pFieldEdit As IFieldEdit
        pFields = New Fields
        pFieldsEdit = pFields

        ' Make the shape field
        ' it will need a geometry definition, with a spatial reference
        pField = New Field
        pFieldEdit = pField
        pFieldEdit.Name_2 = "Shape"
        pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry

        'Query the domain of reference layer
        Dim pFL As IFeatureLayer
        Dim pFeatSelection As IFeatureSelection
        Dim pSelSet As ISelectionSet
        pFL = New FeatureLayer
        pFL.FeatureClass = SpatialBoundary
        pFeatSelection = pFL
        pFeatSelection.SelectFeatures(Nothing, esriSelectionResultEnum.esriSelectionResultAdd, False)
        pSelSet = pFeatSelection.SelectionSet
        Dim pFeatCursor As IFeatureCursor = Nothing
        pSelSet.Search(Nothing, False, pFeatCursor)
        Dim pFeature As IFeature
        pFeature = pFeatCursor.NextFeature
        Dim pGeoBag As IGeometryBag
        pGeoBag = New GeometryBag
        Dim pgeocollection As IGeometryCollection
        pgeocollection = New GeometryBag

        'Add each feature's geometry to the geometry collection
        Do While Not pFeature Is Nothing
            pgeocollection.AddGeometry(pFeature.Shape, 0)
            pFeature = pFeatCursor.NextFeature
        Loop
        pGeoBag = pgeocollection

        'Get Coordinates
        Dim pBoundaryEnv As IEnvelope
        pBoundaryEnv = New Envelope
        pBoundaryEnv = pGeoBag.Envelope

        'query boundary layer's envelope coordinates and input as the new data frame's envelope coordinates
        Dim xMin As Double, yMin As Double, xMax As Double, yMax As Double
        pBoundaryEnv.QueryCoords(xMin, yMin, xMax, yMax)

        Dim pGeomDef As IGeometryDef
        Dim pGeomDefEdit As IGeometryDefEdit
        pGeomDef = New GeometryDef
        pGeomDefEdit = pGeomDef
        'options are esriGeometryPolygon or esriGeometryPoint
        pGeomDefEdit.GeometryType_2 = 1 'for points
        pSpatialRef.GetDomain(xMin, xMax, yMin, yMax)
        pGeomDefEdit.SpatialReference_2 = pSpatialRef

        pFieldEdit.GeometryDef_2 = pGeomDef
        pFieldsEdit.AddField(pField)

        ' Create the shapefile
        BA_CreatePointShapefile = pFWS.CreateFeatureClass(strName, pFields, Nothing, _
                                            Nothing, esriFeatureType.esriFTSimple, "Shape", "")

        'Add Coordinate Points (optional)
        If addExtent = True Then
            'Store Envelope coordinates as point values
            Dim pPointArray() As IPoint
            ReDim pPointArray(0 To 3)
            pPointArray(0) = New Point
            pPointArray(0).PutCoords(xMax, yMax)
            pPointArray(1) = New Point
            pPointArray(1).PutCoords(xMax, yMin)
            pPointArray(2) = New Point
            pPointArray(2).PutCoords(xMin, yMax)
            pPointArray(3) = New Point
            pPointArray(3).PutCoords(xMin, yMin)

            Dim i As Integer
            Dim pGeo As IGeometry
            'Adds 4 coordinate points to point shapefile to represent the spatial extent of the watershed envelope
            For i = 0 To 3
                pGeo = pPointArray(i)
                pFeature = New Feature
                pFeature = BA_CreatePointShapefile.CreateFeature
                pFeature.Shape = pGeo
                pFeature.Store()
            Next
            pGeo = Nothing
            pFeature = Nothing
        End If

        'clears memory
        pWorkspaceFactory = Nothing
        pFWS = Nothing
        pFields = Nothing
        pFieldsEdit = Nothing
        pField = Nothing
        pGeomDef = Nothing
        pGeomDefEdit = Nothing
        pSpatialRef = Nothing

    End Function

    '==============================================================
    'supplemental routines
    'This procedure adds a polyline shapefile to ArcMap
    'DisplayName if specified, is used as the layer name in the TOC in ArcMap
    'Action code: 0, default add and replace if layer of the same name already added, and zoom to layer
    '                   1, add and replace only
    '                   2, add
    'Return values: -1 unknown error occurred
    '                       -2 filename error
    '                       -3 not polygon shapefile
    '                       , otherwise, the same value as the Action code
    Public Function BA_AddLineLayer(ByVal LayerPathName As String, ByVal DisplayName As String, ByVal LineColor As IColor, Optional ByVal Action As MapsAddLayerOptions = MapsAddLayerOptions.AddReplaceNoZoom) As Integer
        Dim return_code As BA_ReturnCode = BA_ReturnCode.UnknownError
        Dim File_Path As String = ""
        Dim File_Name As String = ""

        File_Name = BA_GetBareName(LayerPathName, File_Path)

        'exit if file_name is null
        If Len(File_Name) = 0 Then
            return_code = BA_ReturnCode.ReadError
            Return return_code
            Exit Function
        End If

        'Dim pWksFactory As IWorkspaceFactory
        'Dim pFeatWorkspace As IFeatureWorkspace
        Dim pFeatClass As IFeatureClass
        Dim pFLayer As IFeatureLayer = New FeatureLayer

        'map display
        Dim pMxDoc As IMxDocument = My.Document
        Dim pMap As IMap = pMxDoc.FocusMap
        Dim pGFLayer As IGeoFeatureLayer
        Dim pRenderer As ISimpleRenderer
        Dim pLineSym As ILineSymbol

        Try
            'text exists for the setting of this layer
            'pWksFactory = New ShapefileWorkspaceFactory
            'pWksFactory = New FileGDBWorkspaceFactory()
            'pFeatWorkspace = pWksFactory.OpenFromFile(File_Path, 0)
            'pFeatClass = pFeatWorkspace.OpenFeatureClass(File_Name)
            pFeatClass = BA_OpenFeatureClassFromGDB(File_Path, File_Name)

            'add featureclass to current data frame
            pFLayer.FeatureClass = pFeatClass
            pFLayer.Name = DisplayName

            'check feature geometry type, only polyline layers can be used as an extent layer
            If pFLayer.FeatureClass.ShapeType <> ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline Then
                return_code = BA_ReturnCode.NotSupportedOperation
                pFLayer = Nothing
                pFeatClass = Nothing
                'pFeatWorkspace = Nothing
                'pWksFactory = Nothing
                Return return_code
                Exit Function
            End If

            'set layer symbology - hollow with red outline
            pLineSym = New SimpleLineSymbol
            pLineSym.Color = LineColor

            pRenderer = New SimpleRenderer
            pRenderer.Symbol = pLineSym

            pGFLayer = pFLayer
            pGFLayer.Renderer = pRenderer

            'check if a layer with the assigned name exists
            If Action <> MapsAddLayerOptions.AddOnly Then
                'search layer of the specified name, if found
                Dim nlayers As Long
                Dim i As Long
                Dim pTempLayer As ILayer
                nlayers = pMap.LayerCount
                For i = nlayers To 1 Step -1
                    pTempLayer = pMap.Layer(i - 1)
                    If DisplayName = pTempLayer.Name Then 'remove the layer
                        pMap.DeleteLayer(pTempLayer)
                    End If
                Next
            End If

            pMap.AddLayer(pFLayer)
            Dim Buffer_Factor As Double = 2

            If Action = MapsAddLayerOptions.AddReplaceZoom Then 'zoom to layer
                'create a buffer around the AOI
                Dim pEnv As IEnvelope = pFLayer.AreaOfInterest

                Dim llx As Double, lly As Double, urx As Double, ury As Double
                Dim xrange As Double, yrange As Double
                Dim xoffset As Double, yoffset As Double

                pEnv.QueryCoords(llx, lly, urx, ury)
                xrange = urx - llx
                yrange = ury - lly
                xoffset = xrange * (Buffer_Factor - 1) / 2
                yoffset = yrange * (Buffer_Factor - 1) / 2
                llx = llx - xoffset
                lly = lly - yoffset
                urx = urx + xoffset
                ury = ury + yoffset
                pEnv.PutCoords(llx, lly, urx, ury)

                'pFLayer.AreaOfInterest.PutCoords llx, lly, urx, ury
                Dim pActiveView As IActiveView = pMxDoc.ActiveView
                pActiveView.Extent = pEnv
            End If

            'refresh the active view
            pMxDoc.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing) 'esriViewGeography
            pMxDoc.UpdateContents()

            return_code = BA_ReturnCode.Success

        Catch ex As Exception
            Debug.Print("BA_AddLineLayer exception: " & ex.Message)
            return_code = BA_ReturnCode.UnknownError

        Finally
            BA_AddLineLayer = return_code
        End Try
    End Function

    'check if there is any NODATA cell in a raster within the AOI boundary
    Public Function BA_RasterHasNodata(ByVal aoiPath As String, ByVal rasterPath As String, ByVal rasterName As String) As Boolean
        Dim pGeoDataset As IGeoDataset = Nothing
        Dim pRasterDataset As IRasterDataset3 = Nothing
        Dim pInputRaster As IRaster = Nothing
        Dim resultGeoDataset As IGeoDataset = Nothing
        Dim extractionOp As IExtractionOp2 = New RasterExtractionOp
        Dim pAOIRaster As IGeoDataset = Nothing
        Dim maskPath As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Aoi)
        Dim maskName As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.AoiGrid), False)
        Dim pTempRaster As IGeoDataset = Nothing
        Dim pLogicalOp As ILogicalOp = New RasterMathOps
        Dim pRasterBandCollection As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pRStats As IRasterStatistics = Nothing

        Try
            Dim pWorkspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(rasterPath)
            If pWorkspaceType = WorkspaceType.Raster Then
                pGeoDataset = BA_OpenRasterFromFile(rasterPath, rasterName)
            ElseIf pWorkspaceType = WorkspaceType.Geodatabase Then
                pGeoDataset = BA_OpenRasterFromGDB(rasterPath, rasterName)
            End If

            resultGeoDataset = pLogicalOp.IsNull(pGeoDataset)
            'need to save the output for verification
            BA_SaveRasterDatasetGDB(resultGeoDataset, rasterPath, BA_RASTER_FORMAT, rasterName & "_mask")
            resultGeoDataset = Nothing
            pGeoDataset = Nothing

            pGeoDataset = BA_OpenRasterFromGDB(rasterPath, rasterName & "_mask")
            pAOIRaster = BA_OpenRasterFromGDB(maskPath, maskName)
            If pAOIRaster Is Nothing Then
                MsgBox("BA_RasterHasNodata() Error: Cannot locate AOI boundary raster in the AOI.")
                Return Nothing
            End If

            'extract raster to the AOI boundary
            pRasterDataset = CType(pGeoDataset, RasterDataset)  'Explicit cast
            ' Create IRaster from IGeoDataset
            pInputRaster = pRasterDataset.CreateDefaultRaster()
            pTempRaster = extractionOp.Raster(pInputRaster, pAOIRaster)
            pAOIRaster = Nothing

            'get the stats of the resultGeoDataset and see if there is any 1 in it
            pRasterBandCollection = CType(pTempRaster, IRasterBandCollection) 'Explicit cast
            pRasterBand = pRasterBandCollection.Item(0)
            pRStats = pRasterBand.Statistics

            If pRStats IsNot Nothing Then
                If pRStats.Maximum > 0 Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return Nothing
            End If

        Catch ex As Exception
            MsgBox("BA_RasterHasNodata() Exception: " & ex.Message)
            Return Nothing

        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(extractionOp)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pLogicalOp)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(resultGeoDataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pInputRaster)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTempRaster)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pAOIRaster)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBandCollection)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBand)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRStats)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterDataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGeoDataset)
            BA_RemoveRasterFromGDB(rasterPath, rasterName & "_mask")
        End Try
    End Function
End Module
