Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.GeoAnalyst
Imports ESRI.ArcGIS.GeoAnalyst.esriGeoAnalysisUnitsEnum
Imports ESRI.ArcGIS.GeoAnalyst.esriGeoAnalysisStatisticsEnum
Imports ESRI.ArcGIS.SpatialAnalyst
Imports System.Windows.Forms
Imports ESRI.ArcGIS.DataManagementTools
Imports ESRI.ArcGIS.Geoprocessing
Imports ESRI.ArcGIS.DataSourcesRaster
Imports BAGIS_ClassLibrary

'Description: Calculates statistics on a raster over a specified neighborhood.
Module FilterModule1

    Public Function BA_AddRasFilter(ByVal infilepath As String, ByVal infile As String, _
                ByVal outfilepath As String, ByVal outfile As String, _
                ByVal maskFolder As String, ByVal maskFile As String, _
                ByVal snapRasterPath As String, _
                ByVal nbrWd As Integer, ByVal nbrHt As Integer, _
                ByVal filterType As esriGeoAnalysisStatisticsEnum, _
                ByVal iterations As Integer, ByVal tempOutFilePath As String) As BA_ReturnCode

        'Declare ArcObjects outside of Try/Catch so we can release them in Finally
        'Create a RasterNeighborhoodOp operator    
        Dim pNeighborhoodOp As INeighborhoodOp = New RasterNeighborhoodOp
        Dim pRasterNeighborhood As IRasterNeighborhood = New RasterNeighborhood
        Dim pRaster As IGeoDataset = Nothing
        Dim pOutRaster As IGeoDataset = Nothing
        Dim pStatRaster As IGeoDataset = Nothing
        Dim maskGDS As IGeoDataset = Nothing
        Dim pEnv As IRasterAnalysisEnvironment = Nothing

        Try
            Dim inputWorkspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(infilepath)
            Dim isIntegerRaster As Boolean
            If inputWorkspaceType = WorkspaceType.Raster Then
                isIntegerRaster = BA_IsIntegerRaster(infilepath + "\" + infile)
                pRaster = BA_OpenRasterFromFile(infilepath, infile)
            ElseIf inputWorkspaceType = WorkspaceType.Geodatabase Then
                isIntegerRaster = BA_IsIntegerRaster(infilepath + "\" + infile)
                pRaster = BA_OpenRasterFromGDB(infilepath, infile)
            End If

            'If BA_IsIntegerRaster(infilepath + "\" + infile) = 0 And _
            If isIntegerRaster = True And _
                filterType = esriGeoAnalysisStatisticsEnum.esriGeoAnalysisStatsMajority Then
                Return BA_ReturnCode.NotSupportedOperation
                Exit Function
            End If

            'Specify a rectangle neighborhood
            pRasterNeighborhood.SetRectangle(nbrWd, nbrHt, esriUnitsCells)

            'Configure raster analysis environment
            pEnv = CType(pNeighborhoodOp, IRasterAnalysisEnvironment)  ' Explicit cast
            maskGDS = BA_OpenFeatureClassFromGDB(maskFolder, maskFile)
            pEnv.Mask = maskGDS
            ' Set the analysis extent to match the mask
            Dim extentProvider As Object = CType(maskGDS.Extent, Object)
            pEnv.SetExtent(esriRasterEnvSettingEnum.esriRasterEnvValue, extentProvider)

            '22-JUL-2011 Decided to use INeighborhoodOp instead of GP because it was MUCH faster - LCB
            'Perform focal statistics
            'Dim strFilterType As String = GetStatisticsTypeFromEnum(filterType)
            'Dim maskPath As String = maskFolder & maskFile
            'Dim name1 As String = Nothing
            'Dim name2 As String = Nothing
            'For index As Integer = 1 To iterations
            '    ' Is this the next-to-last iteration
            '    If index = iterations Then
            '        name1 = "tempR001"
            '    Else
            '        name1 = "zzz" & index
            '    End If

            '    ' Is this the first iteration? If so, we use infilepath for "from" raster
            '    If index = 1 Then
            '        'pOutRaster = pNeighborhoodOp.FocalStatistics(pRaster, filterType, pRasterNeighborhood, True)
            '        BA_FocalStatistics_GP(infilepath & infile, outfilepath & "\" & name1, maskPath, "Rectangle 5 5 Cell", strFilterType)
            '    Else
            '        'pOutRaster = pNeighborhoodOp.FocalStatistics(pStatRaster, filterType, pRasterNeighborhood, True)
            '        BA_FocalStatistics_GP(outfilepath & "\" & name2, outfilepath & "\" & name1, maskPath, "Rectangle 5 5 Cell", strFilterType)
            '    End If
            '    name2 = name1
            'Next
            'BA_RemoveTemporaryRasters(outfilepath, "zzz")

            For index As Integer = 1 To iterations
                ' Is this the first iteration? If so, we use infilepath for "from" raster
                If index = 1 Then
                    pOutRaster = pNeighborhoodOp.FocalStatistics(pRaster, filterType, pRasterNeighborhood, True)
                Else
                    ' Otherwise use output from previous run
                    pOutRaster = pNeighborhoodOp.FocalStatistics(pStatRaster, filterType, pRasterNeighborhood, True)
                End If
                pStatRaster = pOutRaster
            Next

            ' Persist focal statistics output to GDB
            ' Geoanalyst tool unable to save to file GDB
            BA_SaveRasterDatasetGDB2(pOutRaster, tempOutFilePath, outfilepath, BA_RASTER_FORMAT, "tempR001")

            If filterType = esriGeoAnalysisStatsMajority Then
                'Removing NoData only required for Majority filter
                If BA_RemNodataFromRas(outfilepath, "tempR001", maskFolder, maskFile, _
                                       outfilepath, outfile, snapRasterPath, tempOutFilePath) <> BA_ReturnCode.Success Then
                    Return BA_ReturnCode.UnknownError
                    Exit Function
                End If
            Else
                ' Rename output of filter to outfile name if we don't have to remove NoData values
                BA_RenameRasterInGDB(outfilepath, "tempR001", outfile)
            End If

            BA_RemoveRasterFromGDB(outfilepath, "tempR001")
            Return BA_ReturnCode.Success

        Catch ex As Exception
            MessageBox.Show("BA_AddRasFilter() Exception: " & ex.Message)

        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterNeighborhood)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pNeighborhoodOp)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pEnv)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRaster)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStatRaster)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pOutRaster)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(maskGDS)
        End Try

    End Function

    Public Function BA_RemNodataFromRas(ByVal inRasPath As String, ByVal inRasName As String, _
                                        ByVal maskRasPath As String, ByVal maskRasName As String, _
                                        ByVal outRasPath As String, ByVal outRasName As String, _
                                        ByVal snapRasPath As String, ByVal aoiFolder As String) As BA_ReturnCode

        Dim geoDataSet As IGeoDataset = Nothing
        Dim featureClass As IFeatureClass = Nothing

        Dim tempVector1 As String = "tempV001"
        Dim tempVector2 As String = "tempV002"
        Dim tempRaster2 As String = "tempR002"

        Try
            Dim retVal As BA_ReturnCode = BA_ReplaceNoDataCells(inRasPath, inRasName, outRasPath, tempRaster2, _
                                                                -9999, maskRasPath, maskRasName, aoiFolder)
            If retVal <> BA_ReturnCode.Success Then
                Return retVal
                Exit Function
            End If
            geoDataSet = BA_OpenRasterFromGDB(outRasPath, tempRaster2)
            If geoDataSet Is Nothing Then
                Return BA_ReturnCode.UnknownError
                Exit Function
            End If

            Dim cellSize As Double = BA_CellSizeFromGeoDataset(geoDataSet)

            'Replace with BA_Raster2Polygon_GP
            'retVal = BA_Raster2PolygonShapefile(outRasPath, tempVector1, geoDataSet)
            'If retVal <> 1 Then
            '    Return BA_ReturnCode.UnknownError
            '    Exit Function
            'End If

            retVal = BA_Raster2Polygon_GP(outRasPath & "\" & tempRaster2, outRasPath & "\" & tempVector1, snapRasPath)

            'Need to determine grid_code field name
            Dim fldGridCode As String = BA_FindGridCodeFieldNameForFC(outRasPath, tempVector1)

            If retVal = BA_ReturnCode.Success Then
                BA_Eliminate(tempVector1, tempVector2, outRasPath, fldGridCode, -9999, " = ", "LENGTH")
            End If
            'featureClass = BA_OpenFeatureClassFromFile(outRasPath, tempVector2)
            'featureClass = BA_OpenFeatureClassFromGDB(outRasPath, tempVector2)
            'If featureClass Is Nothing Then
            '    Return BA_ReturnCode.UnknownError
            '    Exit Function
            'End If

            'Use GP Raster2Polygon rather than GeoAnalyst library. Could not use GeoAnalysst
            'output with Combine tool when writing to a geodatabase
            'ERROR 000864 Input raster: The input is not within the defined domain. 
            'ERROR 000863: Invalid GP data type
            'BA_Feature2RasterInteger(featureClass, outRasPath, outRasName, cellSize, BA_FIELD_GRIDCODE, WorkspaceType.Geodatabase)
            Dim inFeaturesPath As String = outRasPath & "\" & tempVector2
            Dim outRasterFullPath As String = outRasPath & "\" & outRasName
            retVal = BA_Feature2RasterGP(inFeaturesPath, outRasterFullPath, fldGridCode, cellSize, snapRasPath)

            'BA_Remove_Raster(outRasPath, tempRaster2)
            'BA_Remove_Shapefile(outRasPath, tempVector1)
            'BA_Remove_Shapefile(outRasPath, tempVector2)
            BA_RemoveRasterFromGDB(outRasPath, tempRaster2)
            BA_Remove_ShapefileFromGDB(outRasPath, tempVector1)
            BA_Remove_ShapefileFromGDB(outRasPath, tempVector2)
            Return retVal

        Catch ex As Exception
            MessageBox.Show("BA_RemNodataFromRas() Exception: " & ex.Message)
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(geoDataSet)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(featureClass)
        End Try

    End Function

    Public Sub BA_Eliminate(ByVal inFeatName As String, ByVal outFeatName As String, ByVal featurePath As String, _
                                ByVal fieldName As String, ByVal fieldValue As Double, ByVal pCond As String, _
                                ByVal pElim_opt As String)

        Dim pGP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
        pGP.OverwriteOutput = True
        pGP.AddOutputsToMap = False
        Dim gpUtils As IGPUtilities = New GPUtilities
        Dim templayer As String = "TempLyr"

        Try
            ' Set default workspace
            pGP.SetEnvironmentValue("workspace", featurePath)

            ' Create the MakeFeatureLayer tool process object.
            Dim makFeatTool As MakeFeatureLayer = New MakeFeatureLayer()
            Dim pResult As IGeoProcessorResult

            ' Populate the MakeFeatureLayerTool with parameter values.            
            With makFeatTool
                '.in_features = inFeatName & ".shp"
                .in_features = inFeatName
                .out_layer = templayer
            End With

            ' Execute the model tool by name.
            pResult = pGP.Execute(makFeatTool, Nothing)
            makFeatTool = Nothing

            ' If the job failed, retrieve the feature result.
            If pResult IsNot Nothing Then

                ' Create the SelectLayerByAttribute tool process object.
                Dim selLayAttribTool As SelectLayerByAttribute = New SelectLayerByAttribute()

                ' Populate the MakeFeatureLayerTool with parameter values.            
                With selLayAttribTool
                    .in_layer_or_view = templayer
                    .selection_type = "NEW_SELECTION"
                    .where_clause = """" & fieldName & """" & pCond & Str(fieldValue)
                End With

                ' Execute the model tool by name.
                pResult = pGP.Execute(selLayAttribTool, Nothing)
                selLayAttribTool = Nothing

                ' If the job failed, retrieve the feature result.
                If pResult IsNot Nothing Then

                    ' Create the Eliminatetool process object.
                    Dim elmPolyTool As Eliminate = New Eliminate

                    ' Populate the EliminatePolygonPartTool with parameter values.Selection can be AREA,LENGTH
                    With elmPolyTool
                        .in_features = templayer
                        .out_feature_class = outFeatName
                        .selection = pElim_opt
                    End With

                    ' Execute the model tool by name.
                    pResult = pGP.Execute(elmPolyTool, Nothing)
                    elmPolyTool = Nothing

                    ' If the job failed, retrieve the feature result.
                    If pResult Is Nothing Then
                        MessageBox.Show("Error: Stage 3 tool execution failed.")
                    End If

                Else
                    MessageBox.Show("Error: Stage 2 tool execution failed.")
                End If

                ' Create the Delete tool to delete templyr.
                Dim delTool As Delete = New Delete
                delTool.in_data = templayer
                pResult = pGP.Execute(delTool, Nothing)
                delTool = Nothing

            Else
                MessageBox.Show("Error: Stage 1 tool execution failed.")
            End If


        Catch ex As Exception
            For c As Integer = 0 To pGP.MessageCount - 1
                Debug.Print("GP error: " & pGP.GetMessage(c))
            Next
            If pGP.MessageCount > 0 Then
                MessageBox.Show("Geoprocessor error: " + pGP.GetMessages(2))
            Else
                MessageBox.Show("Exception: " + ex.Message)
            End If
        Finally
            gpUtils.RemoveInternalLayer(templayer)
            gpUtils.ReleaseInternals()
            gpUtils = Nothing
            pGP = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Sub


    'Use the GP MakeFeature and Eliminate tools to eliminate polygons from a vector that are
    'smaller than a given area
    Public Sub BA_EliminatePoly(ByVal inFeatPath As String, ByVal inFeatName As String, ByVal outFeatPath As String, ByVal outFeatName As String, _
                                ByVal pElim_opt As String, ByVal pArea As Double, ByVal area_unit As String)

        Dim pGP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
        ' Set default workspace
        pGP.SetEnvironmentValue("workspace", outFeatPath)
        pGP.AddOutputsToMap = False
        pGP.OverwriteOutput = True
        Dim gpUtils As IGPUtilities = New GPUtilities
        Dim templayer As String = "TempLyr"

        Try
            ' Create the MakeFeatureLayer tool process object.
            Dim makFeatTool As MakeFeatureLayer = New MakeFeatureLayer()
            Dim pResult As IGeoProcessorResult

            ' Populate the MakeFeatureLayerTool with parameter values.            
            With makFeatTool
                .in_features = inFeatPath & "\" & inFeatName
                .out_layer = templayer
            End With

            ' Execute the model tool by name.
            pResult = pGP.Execute(makFeatTool, Nothing)
            makFeatTool = Nothing

            ' If the job failed, retrieve the feature result.
            If pResult IsNot Nothing Then

                ' Create the SelectLayerByAttribute tool process object.
                Dim selLayAttribTool As SelectLayerByAttribute = New SelectLayerByAttribute()

                ' Populate the MakeFeatureLayerTool with parameter values.            
                With selLayAttribTool
                    .in_layer_or_view = templayer
                    .selection_type = "NEW_SELECTION"
                    .where_clause = """" & area_unit & """" & " <= " & Str(pArea)
                End With

                ' Execute the model tool by name.
                pResult = pGP.Execute(selLayAttribTool, Nothing)
                selLayAttribTool = Nothing

                ' If the job failed, retrieve the feature result.
                If pResult IsNot Nothing Then

                    ' Create the Eliminatetool process object.
                    Dim elmPolyTool As Eliminate = New Eliminate

                    ' Populate the EliminatePolygonPartTool with parameter values.Selection can be AREA,LENGTH
                    With elmPolyTool
                        .in_features = templayer
                        .out_feature_class = outFeatName
                        .selection = pElim_opt
                    End With

                    ' Execute the model tool by name.
                    pResult = pGP.Execute(elmPolyTool, Nothing)
                    elmPolyTool = Nothing

                    ' If the job failed, retrieve the feature result.
                    If pResult Is Nothing Then
                        MessageBox.Show("Error: Eliminate tool execution failed.")
                    End If

                Else
                    MessageBox.Show("Error: SelectLayerByAttribute tool execution failed.")
                End If

                ' Create the Delete tool to delete templyr.
                Dim delTool As Delete = New Delete
                delTool.in_data = templayer
                pResult = pGP.Execute(delTool, Nothing)
                delTool = Nothing

            Else
                MessageBox.Show("Error: MakeFeatureLayer tool execution failed.")
            End If

        Catch ex As Exception
            For c As Integer = 0 To pGP.MessageCount - 1
                Debug.Print("GP error: " & pGP.GetMessage(c))
            Next
            If pGP.MessageCount > 0 Then
                MessageBox.Show("Geoprocessor error: " + pGP.GetMessages(2))
            Else
                MessageBox.Show("Exception: " + ex.Message)
            End If

        Finally

            pGP = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
            gpUtils.RemoveInternalLayer(templayer)
            gpUtils.ReleaseInternals()
            gpUtils = Nothing
            pGP = Nothing

        End Try

    End Sub

    Private Function GetStatisticsTypeFromEnum(ByVal filterType As esriGeoAnalysisStatisticsEnum) As String
        Dim strFilter As String = filterType.ToString
        Dim len1 As Integer = "esriGeoAnalysisStats".Length
        Return strFilter.Substring(len1)
    End Function



End Module