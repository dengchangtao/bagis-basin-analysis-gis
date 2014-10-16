Imports ESRI.ArcGIS.GeoAnalyst
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.SpatialAnalyst
Imports System.Text
Imports ESRI.ArcGIS.DataSourcesGDB
Imports BAGIS_ClassLibrary

Module SelectedZonesModule

    ' Creates an hru raster from a collection of layers generated from a collection of iRules.
    ' The analysis area is limited to a selected set of zones from a parent hru dataset
    Public Function BA_CreateHruFromZones(ByVal maskFilePath As String, ByVal inputFullPath As String, _
                                          ByVal layerList As List(Of String), ByVal useZones As Boolean, _
                                          ByVal zones As Zone(), ByVal outputFolder As String, _
                                          ByVal outputFile As String, ByVal snapRasterPath As String) As BA_ReturnCode

        Dim retVal As BA_ReturnCode = BA_ReturnCode.UnknownError
        Dim zoneFileName As String = BA_TEMP_PREFIX & "1"
        Dim combineFileName As String = BA_TEMP_PREFIX & "2"
        Dim replaceFileName As String = BA_TEMP_PREFIX & "3"
        Dim inputFolder As String = "PleaseReturn"
        Dim inputFile As String = BA_GetBareName(inputFullPath, inputFolder)
        If useZones = True Then
            ' Create the analysis mask file using only selected zones
            retVal = CreateMask(inputFolder, inputFile, BA_FIELD_VALUE, outputFolder, zoneFileName, zones)
            If retVal = BA_ReturnCode.Success Then
                Dim zoneFilePath As String = outputFolder & "\" & zoneFileName
                ' Combine the rule layers and mask with the analysis mask
                retVal = BA_ZoneOverlay(zoneFilePath, layerList, outputFolder, combineFileName, False, _
                                        True, snapRasterPath, WorkspaceType.Geodatabase)
                If retVal = BA_ReturnCode.Success Then
                    ' Replace NoData cells in the combined layer with 0
                    retVal = BA_ReplaceNoDataCellsGDB(outputFolder, combineFileName, outputFolder, _
                                                      replaceFileName, 0, Nothing, Nothing)
                    If retVal = BA_ReturnCode.Success Then
                        Dim replaceFullPath As String = outputFolder & "\" & replaceFileName
                        ' Combine updated NoData file with all zones from original input
                        retVal = CombineMaskWithAllZones(maskFilePath, outputFolder, outputFile, inputFullPath, _
                                                         replaceFullPath, snapRasterPath)
                        If retVal = BA_ReturnCode.Success Then
                            ' Remove temporary files
                            retVal = BA_RemoveFilesByPrefix(outputFolder, BA_TEMP_PREFIX)
                        End If
                    End If
                End If
            End If
        Else
            retVal = BA_ZoneOverlay(maskFilePath, layerList, outputFolder, combineFileName, False, _
                                    True, snapRasterPath, WorkspaceType.Geodatabase)
            If retVal = BA_ReturnCode.Success Then
                Dim combineFullPath As String = outputFolder & "\" & combineFileName
                retVal = CombineMaskWithAllZones(maskFilePath, outputFolder, outputFile, inputFullPath, _
                                                 combineFullPath, snapRasterPath)
                If retVal = BA_ReturnCode.Success Then
                    ' Remove temporary files
                    retVal = BA_RemoveFilesByPrefix(outputFolder, BA_TEMP_PREFIX)
                End If
            End If
        End If
        Return retVal
    End Function

    ' Implements Stamp functionality
    Public Function BA_StampHru(ByVal aoiMaskFullPath As String, ByVal stampFullPath As String, _
                                   ByVal allZones As Long(), ByVal values As Long(), _
                                   ByVal parentFullPath As String, ByVal outputFolder As String, _
                                   ByVal outputFile As String, ByVal snapRasterPath As String, _
                                   ByVal aoiFolder As String) As BA_ReturnCode

        Dim retVal As BA_ReturnCode = BA_ReturnCode.UnknownError
        Dim unselectedValue As Long = 0
        Dim tempPrefix As String = "tmp_"
        Dim stampBoundaryName As String = tempPrefix & "step1"
        Dim parentBoundaryName As String = tempPrefix & "step2"
        Dim reclassUnselectedName As String = tempPrefix & "step3"
        Dim stampName As String = tempPrefix & "step4"
        Dim stampFolder As String = "PleaseReturn"
        Dim stampFile As String = BA_GetBareName(stampFullPath, stampFolder)
        Dim parentFolder As String = "PleaseReturn"
        Dim parentFile As String = BA_GetBareName(parentFullPath, parentFolder)
        Dim maskFolder As String = "PleaseReturn"
        Dim maskFile As String = BA_GetBareName(aoiMaskFullPath, maskFolder)
        Dim pRasterStat As IRasterStatistics = Nothing

        ' Replace NoData cells in stamp layer inside aoi boundary with unselectedValue
        Array.Sort(allZones)
        ' highValue is the maximum value on the cookieCut layer
        Dim highValue As Long = allZones(allZones.Length - 1)
        ' Set unselectedValue to highValue + 1; -1 doesn't work in combine tool
        unselectedValue = highValue + 1
        ' Get raster statistics to check high value in parent
        pRasterStat = BA_GetRasterStatsGDB(parentFullPath, 0)
        Dim highParent = pRasterStat.Maximum
        If highParent >= unselectedValue Then
            unselectedValue = highParent + 1
        End If
        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterStat)

        retVal = BA_ReplaceNoDataCells(stampFolder, stampFile, outputFolder, _
                                       stampBoundaryName, unselectedValue, maskFolder, _
                                       maskFile, aoiFolder)
        If retVal = BA_ReturnCode.Success Then
            ' Replace NoData cells in parent layer inside aoi boundary with unselectedValue
            retVal = BA_ReplaceNoDataCells(parentFolder, parentFile, outputFolder, _
                               parentBoundaryName, unselectedValue, maskFolder, _
                               maskFile, aoiFolder)
            If retVal = BA_ReturnCode.Success Then
                ' Create an INumberRemap from for all zones: Selected zones retain their value;
                ' Unselected zones are set to unselectedValue
                Dim zoneRemap As INumberRemap = GetZonesValueRemap(allZones, values, unselectedValue)
                Dim stampBoundaryFilePath As String = outputFolder & "\" & stampBoundaryName
                Dim reclassFilePath As String = outputFolder & "\" & reclassUnselectedName
                'Reclass unselected zones to -1
                If zoneRemap IsNot Nothing Then
                    'Output saved to reclassFileName = "step3"
                    retVal = BA_ReclassRasterFromRemap(stampBoundaryFilePath, reclassFilePath, zoneRemap)
                    If retVal = BA_ReturnCode.Success Then
                        'Raster calculator sets cells on parent layer to unselectedValue if they are selected cells in stamp layer
                        retVal = ApplyStamp(outputFolder, parentBoundaryName, reclassUnselectedName, stampName, unselectedValue)
                        If retVal = BA_ReturnCode.Success Then
                            Dim applyStampFullPath As String = outputFolder & "\" & stampName
                            retVal = CombineMaskWithAllZones(aoiMaskFullPath, outputFolder, outputFile, applyStampFullPath, _
                                                             reclassFilePath, snapRasterPath)
                            If retVal = BA_ReturnCode.Success Then
                                ' Remove temporary files
                                retVal = BA_RemoveFilesByPrefix(outputFolder, BA_TEMP_PREFIX)
                            End If
                        End If
                    End If
                End If
            End If
        End If
        Return retVal
    End Function

    ' Implements cookie cutter functionality. 
    Public Function BA_CookieCutterHru(ByVal aoiMaskFullPath As String, ByVal cookieCutFullPath As String, _
                                       ByVal queryField As String, ByVal values As Long(), ByVal parentFullPath As String, _
                                       ByVal outputFolder As String, ByVal outputFile As String, _
                                       ByVal preserveAoiBoundary As Boolean, ByVal snapRasterPath As String, _
                                       ByVal aoiPath As String) As BA_ReturnCode
        Dim retVal As BA_ReturnCode = BA_ReturnCode.UnknownError
        Dim tempPrefix As String = "tmp_"
        Dim zoneFileName As String = tempPrefix & "step1"
        Dim combineFileName As String = tempPrefix & "step2"
        Dim inputFolder As String = "PleaseReturn"
        Dim inputFile As String = BA_GetBareName(cookieCutFullPath, inputFolder)
        ' Create the analysis mask file using only selected values
        retVal = CreateMaskFromValues(inputFolder, inputFile, queryField, outputFolder, zoneFileName, values)
        If retVal = BA_ReturnCode.Success Then
            ' Combine the cookie-cut/parent layers and mask with the analysis mask
            Dim zoneFilePath As String = outputFolder & "\" & zoneFileName
            Dim layerList As List(Of String) = New List(Of String)
            layerList.Add(cookieCutFullPath)
            layerList.Add(parentFullPath)

            If preserveAoiBoundary = True Then
                retVal = BA_ZoneOverlay(zoneFilePath, layerList, outputFolder, combineFileName, False, _
                                        True, snapRasterPath, WorkspaceType.Geodatabase)
                If retVal = BA_ReturnCode.Success Then
                    ' Replace NoData cells in the combined layer with 0
                    Dim maskFolder As String = "PleaseReturn"
                    Dim maskFile As String = BA_GetBareName(aoiMaskFullPath, maskFolder)
                    retVal = BA_ReplaceNoDataCells(outputFolder, combineFileName, outputFolder, _
                                                   outputFile, 0, maskFolder, maskFile, aoiPath)
                End If
            Else
                retVal = BA_ZoneOverlay(zoneFilePath, layerList, outputFolder, outputFile, False, _
                                        True, snapRasterPath, WorkspaceType.Geodatabase)
            End If
            ' @ToDo: delete temporary files
            If retVal = BA_ReturnCode.Success Then
                BA_RemoveFilesByPrefix(outputFolder, tempPrefix)
            End If
        End If
        Return retVal
    End Function


    Private Function CreateMask(ByVal inputFolder As String, ByVal inputFile As String, ByVal queryField As String, _
                                ByVal outputFolder As String, ByVal outputFile As String, _
                                ByVal zones As Zone()) As BA_ReturnCode
        Dim pRasterDescriptor As IRasterDescriptor = New RasterDescriptor
        Dim pGeoDataset As IGeoDataset = Nothing
        Dim pRasterDataset As IRasterDataset3 = Nothing
        Dim pInputRaster As IRaster = Nothing
        Dim pQFilter As IQueryFilter = New QueryFilter
        Dim retVal As BA_ReturnCode = BA_ReturnCode.UnknownError
        Dim maskGeoDataset As IGeoDataset = Nothing
        Dim extractionOp As IExtractionOp2 = New RasterExtractionOp

        Try
            Dim pWorkspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(inputFolder)
            If pWorkspaceType = WorkspaceType.Raster Then
                pGeoDataset = BA_OpenRasterFromFile(inputFolder, inputFile)
            ElseIf pWorkspaceType = WorkspaceType.Geodatabase Then
                pGeoDataset = BA_OpenRasterFromGDB(inputFolder, inputFile)
            End If
            pRasterDataset = CType(pGeoDataset, RasterDataset)  'Explicit cast
            ' Create IRaster from IGeoDataset
            pInputRaster = pRasterDataset.CreateDefaultRaster()
            ' Create csv string of selected zones
            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder
            If zones IsNot Nothing And zones.Length > 0 Then
                For Each pZone In zones
                    sb.Append(CStr(pZone.Id))
                    sb.Append(", ")
                Next
                ' Remove trailing comma
                sb.Remove(sb.Length - 2, 2)
            End If
            ' Add where clause to query filter - BA_FIELD_VALUE
            pQFilter.WhereClause = queryField & " in (" & sb.ToString & ")"
            ' Create raster descriptor and run extractionOp
            pRasterDescriptor.Create(pInputRaster, pQFilter, queryField)
            maskGeoDataset = extractionOp.Attribute(pRasterDescriptor)

            ' Save output to disk
            Dim retInt As Integer = BA_SaveRasterDatasetGDB(maskGeoDataset, outputFolder, BA_RASTER_FORMAT, outputFile)
            If retInt = 1 Then retVal = BA_ReturnCode.Success
            Return retVal
        Catch ex As Exception
            MsgBox("CreateMask() Exception: " & ex.Message)
            Return retVal
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(extractionOp)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(maskGeoDataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pQFilter)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterDescriptor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pInputRaster)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterDataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGeoDataset)
        End Try

    End Function

    'Wrapper for CreateMask() method that converts selected values to zones
    Private Function CreateMaskFromValues(ByVal inputFolder As String, ByVal inputFile As String, ByVal queryField As String, _
                                          ByVal outputFolder As String, ByVal outputFile As String, _
                                          ByVal values As Long()) As BA_ReturnCode
        Dim selectZonesArray(0) As Zone
        If values IsNot Nothing Then
            System.Array.Resize(selectZonesArray, values.Length)
            Dim pos As Integer = 0
            For Each nextValue In values
                selectZonesArray(pos) = New Zone(nextValue)
                pos += 1
            Next
        End If
        Return CreateMask(inputFolder, inputFile, queryField, outputFolder, outputFile, selectZonesArray)
    End Function

    ' Generates a new raster replacing NoData cells with a selected value. The replacement
    ' extent may be masked if a mask file path is provided
    Public Function BA_ReplaceNoDataCells(ByVal inputFolder As String, ByVal inputFile As String, _
                                          ByVal outputFolder As String, ByVal outputFile As String, _
                                          ByVal replaceValue As Integer, ByVal maskFolder As String, _
                                          ByVal maskFile As String, ByVal aoiFolder As String) As BA_ReturnCode
        Dim mapAlgebraOp As IMapAlgebraOp = New RasterMapAlgebraOp
        Dim inputGeodataset As IGeoDataset = Nothing
        Dim pRasterDataset As IRasterDataset3 = Nothing
        Dim isNullGeodataset As IGeoDataset = Nothing
        Dim outputGeodataset As IGeoDataset = Nothing
        Dim maskFeatureClass As IFeatureClass = Nothing
        Dim maskGeodataset As IGeoDataset = Nothing
        Dim pWSFactory As IWorkspaceFactory = Nothing
        Dim pWorkspace As IWorkspace = Nothing
        Dim pEnv As IRasterAnalysisEnvironment = Nothing
        Dim retVal As BA_ReturnCode = BA_ReturnCode.UnknownError

        Try
            Dim inputWorkspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(inputFolder)

            If inputWorkspaceType = WorkspaceType.Raster Then
                inputGeodataset = BA_OpenRasterFromFile(inputFolder, inputFile)
            ElseIf inputWorkspaceType = WorkspaceType.Geodatabase Then
                inputGeodataset = BA_OpenRasterFromGDB(inputFolder, inputFile)
            End If
            pWSFactory = New FileGDBWorkspaceFactory()

            If inputGeodataset IsNot Nothing Then
                pRasterDataset = CType(inputGeodataset, IRasterDataset3) ' Explicit cast
                'Set environment
                If Not String.IsNullOrEmpty(maskFolder) AndAlso Not String.IsNullOrEmpty(maskFolder) Then
                    pEnv = CType(mapAlgebraOp, IRasterAnalysisEnvironment)  ' Explicit cast
                    maskFeatureClass = BA_OpenFeatureClassFromGDB(maskFolder, maskFile)
                    maskGeodataset = CType(maskFeatureClass, IGeoDataset)
                    pEnv.Mask = maskGeodataset
                    ' Set the analysis extent to match the mask
                    Dim extentProvider As Object = CType(maskGeodataset.Extent, Object)
                    pEnv.SetExtent(esriRasterEnvSettingEnum.esriRasterEnvValue, extentProvider)
                    pWorkspace = pWSFactory.OpenFromFile(outputFolder, 0)
                    pEnv.OutWorkspace = pWorkspace
                End If
                mapAlgebraOp.BindRaster(pRasterDataset, "noData1")
                Debug.Print("Raster calculator expression: " & "Con(IsNull([noData1]), " & replaceValue & ", [noData1])")
                isNullGeodataset = mapAlgebraOp.Execute("Con(IsNull([noData1]), " & replaceValue & ", [noData1])")
                If isNullGeodataset IsNot Nothing Then
                    ' Geoanalyst tool unable to save to file GDB
                    ' Save to aoi root
                    BA_SaveRasterDataset(isNullGeodataset, aoiFolder, outputFile)
                    Dim fileGeodataset As IGeoDataset = BA_OpenRasterFromFile(aoiFolder, outputFile)
                    BA_SaveRasterDatasetGDB(fileGeodataset, outputFolder, BA_RASTER_FORMAT, outputFile)
                    BA_Remove_Raster(aoiFolder, outputFile)
                    retVal = BA_ReturnCode.Success
                End If
            End If
            Return retVal
        Catch ex As Exception
            MsgBox("BA_ReplaceNoDataCells() Exception: " & ex.Message)
            Return retVal
        Finally
            'mapAlgebraOp.UnbindRaster("noData1")
            pEnv = Nothing
            maskGeodataset = Nothing
            maskFeatureClass = Nothing
            outputGeodataset = Nothing
            isNullGeodataset = Nothing
            inputGeodataset = Nothing
            pWSFactory = Nothing
            pWorkspace = Nothing
            mapAlgebraOp = Nothing
        End Try

    End Function

    Private Function CombineMaskWithAllZones(ByVal maskFilePath As String, ByVal outputFolder As String, _
                                             ByVal outputFileName As String, ByVal inputFullPath As String, _
                                             ByVal replaceFullPath As String, ByVal snapRasterPath As String) As BA_ReturnCode
        Dim layerList As List(Of String) = New List(Of String)
        layerList.Add(inputFullPath)
        layerList.Add(replaceFullPath)
        Dim retVal As BA_ReturnCode = BA_ZoneOverlay(maskFilePath, layerList, outputFolder, _
                                                     outputFileName, False, True, snapRasterPath, _
                                                     WorkspaceType.Geodatabase)
        Return retVal
    End Function

    Private Function GetZonesValueRemap(ByVal allValues As Long(), ByVal values As Long(), _
                                        ByVal unselectedValue As Long) As INumberRemap

        Dim pNumberReMap As INumberRemap = New NumberRemap
        Dim pTable As New Hashtable

        For Each pValue In allValues
            pTable.Add(pValue, unselectedValue)
        Next
        'Add entry for unselected value (NoData inside AOI boundary)
        pTable.Add(unselectedValue, unselectedValue)

        'Get zone value for selected zones to overwrite unselected value
        For Each pValue In values
            For Each pKey In pTable.Keys
                If pKey = pValue Then
                    pTable(pKey) = pValue
                    Exit For
                End If
            Next
        Next

        ' Sort the Hashtable keys so the remap is sorted
        Dim keys As ICollection = pTable.Keys
        Dim keysArray(pTable.Count - 1) As Long
        keys.CopyTo(keysArray, 0)
        Array.Sort(keysArray)

        For Each key As Long In keysArray
            Dim toValue As Integer = CType(pTable(key), Integer)
            pNumberReMap.MapValue(key, toValue)
        Next
        Return pNumberReMap
    End Function

    'Private Function ExtractMask(ByVal inputFolder As String, ByVal inputFile As String, _
    '                             ByVal cookieCutFolder As String, ByVal cookieCutFile As String, _
    '                             ByVal outputFolder As String, ByVal outputFile As String, _
    '                             ByVal unselectedValue As Integer) As BA_ReturnCode
    '    Dim pGeoDataset As IGeoDataset = Nothing
    '    Dim cookieGeoDataset As IGeoDataset = Nothing
    '    Dim cookieRasterDataset As IRasterDataset3 = Nothing
    '    Dim cookieRaster As IRaster = Nothing
    '    Dim maskGeoDataset As IGeoDataset = Nothing
    '    Dim outputGeoDataset As IGeoDataset = Nothing
    '    Dim pQFilter As IQueryFilter = New QueryFilter
    '    Dim pRasterDescriptor As IRasterDescriptor = New RasterDescriptor
    '    Dim extractionOp As IExtractionOp = New RasterExtractionOpClass
    '    Dim retVal As BA_ReturnCode = BA_ReturnCode.UnknownError

    '    Try
    '        pGeoDataset = BA_OpenRasterFromFile(inputFolder, inputFile)
    '        cookieGeoDataset = BA_OpenRasterFromFile(cookieCutFolder, cookieCutFile)
    '        cookieRasterDataset = CType(cookieGeoDataset, RasterDataset)  'Explicit cast
    '        ' Create IRaster from IGeoDataset
    '        cookieRaster = cookieRasterDataset.CreateDefaultRaster()
    '        ' Add where clause to query filter
    '        pQFilter.WhereClause = BA_FIELD_VALUE & " = " & unselectedValue
    '        ' Count unselected cells
    '        Dim pRDataset As IRasterDataset = CType(cookieGeoDataset, IRasterDataset)
    '        Dim pBandCol As IRasterBandCollection = pRDataset
    '        Dim pRasterBand As IRasterBand = pBandCol.Item(0)
    '        Dim pTable As ITable = pRasterBand.AttributeTable
    '        Dim unselectedCount As Integer = pTable.RowCount(pQFilter)
    '        If unselectedCount > 0 Then
    '            ' Create raster descriptor and run extractionOp
    '            pRasterDescriptor.Create(cookieRaster, pQFilter, BA_FIELD_VALUE)
    '            maskGeoDataset = extractionOp.Attribute(pRasterDescriptor)
    '            BA_SaveRasterDataset(maskGeoDataset, outputFolder, "descr")
    '            outputGeoDataset = extractionOp.Raster(pGeoDataset, maskGeoDataset)
    '        Else
    '            outputGeoDataset = extractionOp.Raster(pGeoDataset, cookieGeoDataset)
    '        End If
    '        BA_SaveRasterDataset(outputGeoDataset, outputFolder, outputFile)
    '        retVal = BA_ReturnCode.Success
    '        Return retVal
    '    Catch ex As Exception
    '        MsgBox("ExtractMask exception: " & ex.Message)
    '        Return retVal
    '    Finally
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGeoDataset)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(cookieGeoDataset)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(cookieRasterDataset)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(cookieRaster)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(maskGeoDataset)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(outputGeoDataset)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pQFilter)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterDescriptor)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(extractionOp)
    '    End Try
    'End Function

    Private Function ApplyStamp(ByVal outputFolder As String, ByVal parentFile As String, _
                                ByVal stampFile As String, ByVal outputFile As String, _
                                ByVal replaceValue As Integer) As BA_ReturnCode
        'Create RasterMapAlgebraOp.
        Dim mapAlgebraOp As IMapAlgebraOp = New RasterMapAlgebraOp()
        Dim inGeo01 As IGeoDataset = Nothing
        Dim inRas01 As IRasterDataset = Nothing
        Dim inGeo02 As IGeoDataset = Nothing
        Dim inRas02 As IRasterDataset = Nothing
        Dim env As IRasterAnalysisEnvironment = Nothing
        Dim workspaceFactory As IWorkspaceFactory = New FileGDBWorkspaceFactory()

        Dim workspace As IWorkspace = Nothing
        Dim rasOut As IRaster = Nothing
        Dim retVal As BA_ReturnCode = BA_ReturnCode.UnknownError
        Try
            'Get rasters.
            inGeo01 = BA_OpenRasterFromGDB(outputFolder, parentFile)
            inRas01 = CType(inGeo01, IRasterDataset3) ' Explicit cast
            inGeo02 = BA_OpenRasterFromGDB(outputFolder, stampFile)
            inRas02 = CType(inGeo02, IRasterDataset3) ' Explicit cast
            'Set environment.
            env = CType(mapAlgebraOp, IRasterAnalysisEnvironment) ' Explicit cast
            workspace = workspaceFactory.OpenFromFile(outputFolder, 0)
            env.OutWorkspace = workspace
            'Bind rasters.
            mapAlgebraOp.BindRaster(inRas01, "Ras01")
            mapAlgebraOp.BindRaster(inRas02, "Ras02")
            'Execute script.
            rasOut = mapAlgebraOp.Execute("Con([Ras02] == " & replaceValue & ",[Ras01]," & replaceValue & ")")
            Debug.Print("Con([Ras02] == " & replaceValue & ",[Ras01]," & replaceValue & ")")
            If rasOut IsNot Nothing Then
                If BA_SaveRasterDatasetGDB(rasOut, outputFolder, BA_RASTER_FORMAT, outputFile) = 1 Then
                    retVal = BA_ReturnCode.Success
                End If
            End If
            Return retVal
        Catch ex As Exception
            MsgBox("ApplyStamp() Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            'mapAlgebraOp.UnbindRaster("Ras01")
            'mapAlgebraOp.UnbindRaster("Ras02")
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(mapAlgebraOp)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(inGeo01)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(inGeo02)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(inRas01)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(inRas02)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(workspace)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(workspaceFactory)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(env)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(rasOut)
        End Try

    End Function


End Module
