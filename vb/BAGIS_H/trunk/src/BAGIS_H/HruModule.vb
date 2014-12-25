Option Explicit On

Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesFile
Imports System.IO
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.ArcMapUI
Imports System.Windows.Forms
Imports ESRI.ArcGIS.GeoAnalyst
Imports ESRI.ArcGIS.SpatialAnalyst
Imports System.Text
Imports BAGIS_ClassLibrary

Module HruModule

    ' Builds up the folder structure for the hru output
    Public Sub BA_CreateHruOutputFolders(ByVal aoiPath As String, ByVal hruName As String)
        Dim outputFolder As String = aoiPath & BA_EnumDescription(PublicPath.HruDirectory)
        Dim layersFolder As String = outputFolder & BA_EnumDescription(PublicPath.Layers)
        '\zones
        BA_CreateFolder(aoiPath, BA_EnumDescription(PublicPath.HruDirectory))
        '\zones\hruName
        BA_CreateFolder(aoiPath & BA_EnumDescription(PublicPath.HruDirectory), hruName)
        '\zones\hruName\layers
        'BA_CreateFolder(aoiPath & BA_EnumDescription(PublicPath.HruDirectory) & "\" & hruName, BA_EnumDescription(PublicPath.Layers))
    End Sub

    ' Creates the output GDB
    Public Sub BA_CreateHruOutputGDB(ByVal aoiPath As String, ByVal hruName As String)
        Dim outputFolder As String = aoiPath & BA_EnumDescription(PublicPath.HruDirectory)
        Dim layersFolder As String = outputFolder & BA_EnumDescription(PublicPath.Layers)
        '\zones
        BA_CreateFolder(aoiPath, BA_EnumDescription(PublicPath.HruDirectory))
        '\zones\hruName
        BA_CreateFolder(aoiPath & BA_EnumDescription(PublicPath.HruDirectory), hruName)
        'create gdb in hru-name folder
        BA_CreateFileGdb(BA_GetHruPath(aoiPath, PublicPath.HruDirectory, hruName), hruName & ".gdb")
    End Sub


    ' Generate the output folder name for hru input layer data; Abstraction layer
    ' because some output folders do not have the same name as the layer file name
    Public Function BA_GetOutputFolderName(ByVal layerName As String, ByVal key As Integer) As String
        Dim mapsLayerName As MapsLayerName = BA_GetMapsLayerName(layerName)
        Dim tempOutputFolderName As String
        If mapsLayerName = mapsLayerName.no_name Then
            tempOutputFolderName = "r" & key.ToString("000") & "_" & layerName
        Else
            tempOutputFolderName = "r" & key.ToString("000") & "_" & mapsLayerName.ToString
        End If
        'limit the output folder name to the first 15 characters
        Dim maxFolderLength As Integer = 15
        If Len(tempOutputFolderName) <= maxFolderLength Then
            Return tempOutputFolderName
        Else
            Return tempOutputFolderName.Substring(0, maxFolderLength)
        End If
    End Function

    ' Read the zones from an hru output grid to extract an array of Zone objects
    Public Function BA_ReadZonesFromRaster(ByVal hruFilePath As String, _
                                           ByVal workspaceType As WorkspaceType) As Zone()
        Dim filePath As String = "blank"
        Dim fileName As String = BA_GetBareName(hruFilePath, filePath)
        Dim pGeoDS As IGeoDataset = Nothing
        Dim pRasterBandColl As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pTable As ITable = Nothing
        Dim pDataCursor As ICursor = Nothing
        Dim pRow As IRow = Nothing

        Try
            If workspaceType = workspaceType.Raster Then
                pGeoDS = BA_OpenRasterFromFile(filePath, fileName)
            ElseIf workspaceType = workspaceType.Geodatabase Then
                pGeoDS = BA_OpenRasterFromGDB(filePath, fileName)
            End If
            If pGeoDS Is Nothing Then
                Return Nothing
            End If
            pRasterBandColl = CType(pGeoDS, IRasterBandCollection)
            pRasterBand = pRasterBandColl.Item(0)
            pTable = pRasterBand.AttributeTable
            Dim pZones(pTable.RowCount(Nothing) - 1) As Zone
            Dim pValueIdx As Short = pTable.FindField(BA_FIELD_VALUE)
            pDataCursor = pTable.Search(Nothing, False)
            pRow = pDataCursor.NextRow
            Dim i = 0
            While pRow IsNot Nothing
                Dim pValue As Integer = pRow.Value(pValueIdx)
                Dim pZone As New Zone(pValue)
                pZones(i) = pZone
                pRow = pDataCursor.NextRow
                i = i + 1
            End While
            Return pZones
        Catch ex As Exception
            MessageBox.Show("BA_ReadZonesFromRaster() Exception: " + ex.Message)
            Return Nothing
        Finally
            'Note: ComReleaser doesn't release file locks in this case but setting the objects to
            'nothing and call GC does
            pRow = Nothing
            pDataCursor = Nothing
            pTable = Nothing
            pRasterBand = Nothing
            pRasterBandColl = Nothing
            pGeoDS = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try

    End Function

    ' Read the values from an grid to extract an array of Long values
    Public Function BA_ReadValuesFromRaster(ByVal fullPath As String, ByVal queryField As String) As Long()
        Dim filePath As String = "blank"
        Dim fileName As String = BA_GetBareName(fullPath, filePath)
        Dim pGeoDS As IGeoDataset = Nothing
        Dim pRasterBandColl As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pTable As ITable = Nothing
        Dim pDataCursor As ICursor = Nothing
        Dim pRow As IRow = Nothing

        Try
            Dim pWorkspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(fullPath)
            If pWorkspaceType = WorkspaceType.Raster Then
                pGeoDS = BA_OpenRasterFromFile(filePath, fileName)
            ElseIf pWorkspaceType = WorkspaceType.Geodatabase Then
                pGeoDS = BA_OpenRasterFromGDB(filePath, fileName)
            End If
            If pGeoDS Is Nothing Then
                Return Nothing
            End If
            pRasterBandColl = CType(pGeoDS, IRasterBandCollection)
            pRasterBand = pRasterBandColl.Item(0)
            pTable = pRasterBand.AttributeTable
            Dim pValues(pTable.RowCount(Nothing) - 1) As Long
            Dim pValueIdx As Short = pTable.FindField(queryField)   'BA_FIELD_VALUE
            If pValueIdx > -1 Then
                pDataCursor = pTable.Search(Nothing, False)
                pRow = pDataCursor.NextRow
                Dim i = 0
                While pRow IsNot Nothing
                    Dim pValue As Integer = pRow.Value(pValueIdx)
                    pValues(i) = pValue
                    pRow = pDataCursor.NextRow
                    i = i + 1
                End While
                Return pValues
            End If
            Return Nothing
        Catch ex As Exception
            MessageBox.Show("ReadValuesFromRaster() Exception: " + ex.Message)
            Return Nothing
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRow)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDataCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTable)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBand)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBandColl)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGeoDS)
        End Try

    End Function

    ' Constructs and populates an HRU object; Saves statistics (feature count, avgSize, maxSize) in new HRU 
    Public Function BA_CreateHru(ByVal hruName As String, ByVal rasterFilePath As String, ByVal vectorFilePath As String, _
                                 ByVal selectZonesArray As Zone(), ByVal ruleList As List(Of BAGIS_ClassLibrary.IRule), ByVal contiguousHru As Boolean) As Hru
        Try
            Dim dateCreated As Date = Now
            Dim hruPath As String = "PleaseReturn"
            Dim bareName As String = BA_GetBareName(vectorFilePath, hruPath)
            Dim featureCount As Integer
            Dim avgSize As Double
            Dim maxSize As Double
            If LCase(Right(vectorFilePath, 4)) = ".shp" Then
                vectorFilePath = Left(vectorFilePath, Len(vectorFilePath) - 4)
            End If
            Dim success As Integer = BA_AddShapeAreaToAttrib(vectorFilePath)
            If success = 0 Then
                Dim statResults As BA_DataStatistics
                ' We are currently always saving/displaying in Sq. Kilometers. This could be changed by changing the
                ' field in the data statistics and the measurement unit being sent to create the new hru.
                Dim success2 As Integer = BA_GetDataStatistics(vectorFilePath, BA_FIELD_AREA_SQKM, statResults)
                If success2 = 0 Then
                    featureCount = statResults.Count
                    avgSize = statResults.Mean
                    maxSize = statResults.Maximum
                End If
            End If
            Dim hru As New Hru(hruName, hruPath, selectZonesArray, contiguousHru, _
                               featureCount, avgSize, maxSize, MeasurementUnit.SquareKilometers, _
                               dateCreated)
            hru.RuleList = ruleList
            Return hru
        Catch ex As Exception
            MsgBox("BA_CreateHru Exception: " + ex.Message)
            Return Nothing
        Finally

        End Try

    End Function

    ' Deletes an HRU folder. Checks first to see if any layers in target HRU rolderare in the
    ' current map document. If so, these layers are removed before the HRU is deleted
    Public Function BA_DeleteHRU(ByVal hruPath As String, ByVal pmxDoc As IMxDocument) As BA_ReturnCode
        ' Remove any layers from ArcMap in the hru folder
        BA_RemoveLayersInFolder(pmxDoc, hruPath)
        ' Delete the hru folder
        'BA_Remove_Folder(hruPath)
        BA_Remove_FolderGP(hruPath)
        If BA_Workspace_Exists(hruPath) Then
            Return BA_ReturnCode.UnknownError
        End If
        Return BA_ReturnCode.Success
    End Function

    ' Populates an Aoi object, including any child HRU's, from an XML file
    Public Function BA_LoadHRUFromXml(ByVal hruPath As String) As Aoi
        Dim xmlInputPath As String = hruPath & BA_EnumDescription(PublicPath.HruXml)
        If BA_File_ExistsWindowsIO(xmlInputPath) Then
            Dim obj As Object = SerializableData.Load(xmlInputPath, GetType(Aoi))
            If obj IsNot Nothing Then
                Dim pAoi As Aoi = CType(obj, Aoi)
                Return pAoi
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If
    End Function

    ' Creates an INumberRemap object from an array of ReclassItems. Assumes the reclass items
    ' are sorted as mapRanges must be added to an INumberRemap object in order
    Public Function BA_GetReclassRemap(ByVal reclassItems() As ReclassItem) As INumberRemap
        Dim pNumberReMap As INumberRemap = New NumberRemap
        Try
            For Each item In reclassItems
                pNumberReMap.MapRange(item.FromValue, item.ToValue, item.OutputValue)
            Next
            Return pNumberReMap
        Catch ex As Exception
            MsgBox("BA_GetReclassRemap: " + ex.Message)
            Return Nothing
        End Try
    End Function

    'Not used as of June 15, 2011 LCB
    'Public Function BA_ReclassRasterFromTable(ByVal inputFolderPath As String, ByVal reclassItems As ReclassItem()) As IGeoDataset
    '    Dim pGeoDataset As IGeoDataset = Nothing
    '    Dim pReclassOp As IReclassOp = New RasterReclassOp
    '    Dim pInputRaster As IGeoDataset = Nothing
    '    Dim pNumberReMap As INumberRemap = Nothing
    '    Try
    '        Dim filePath As String = "PleaseReturn"
    '        Dim fileName As String = BA_GetBareName(inputFolderPath, filePath)
    '        pInputRaster = BA_OpenRasterFromFile(filePath, fileName)
    '        pNumberReMap = New NumberRemap
    '        For Each item In reclassItems
    '            pNumberReMap.MapRange(item.FromValue - 0.01, item.ToValue + 0.01, item.OutputValue)
    '        Next
    '        pGeoDataset = pReclassOp.ReclassByRemap(pInputRaster, pNumberReMap, False)
    '        Return pGeoDataset
    '    Catch ex As Exception
    '        MessageBox.Show("BA_ReclassRasterFromTable Exception: " + ex.Message)
    '        Return Nothing
    '    Finally
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pNumberReMap)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pInputRaster)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pReclassOp)
    '    End Try
    'End Function

    'Not used as of June 15, 2011 LCB
    'Public Function BA_ReclassRasterFromDoubleTable(ByVal inputFolderPath As String, ByVal reclassItems As ReclassItem()) As IGeoDataset
    '    Dim pGeoDataset As IGeoDataset = Nothing
    '    Dim pReclassOp As IReclassOp = New RasterReclassOp
    '    Dim pInputRaster As IGeoDataset = Nothing
    '    Dim pNumberReMap As INumberRemap
    '    Try
    '        Dim filePath As String = "PleaseReturn"
    '        Dim fileName As String = BA_GetBareName(inputFolderPath, filePath)
    '        pInputRaster = BA_OpenRasterFromFile(filePath, fileName)
    '        pNumberReMap = BA_GetReclassRemap(reclassItems)
    '        pGeoDataset = pReclassOp.ReclassByRemap(pInputRaster, pNumberReMap, False)
    '        Return pGeoDataset
    '    Catch ex As Exception
    '        MessageBox.Show("Exception: " + ex.Message)
    '        Return Nothing
    '    Finally
    '        pNumberReMap = Nothing
    '        pInputRaster = Nothing
    '        pReclassOp = Nothing
    '        GC.Collect()
    '        GC.WaitForPendingFinalizers()
    '    End Try
    'End Function

    ' Reclasses a raster using an INumberRemap object. Persists reclassed raster
    Public Function BA_ReclassRasterFromRemap(ByVal inputFullPath As String, _
                                              ByVal outputFullPath As String, _
                                              ByVal numberRemap As INumberRemap) As BA_ReturnCode
        Dim pGeoDataset As IGeoDataset = Nothing
        Dim pReclassOp As IReclassOp = New RasterReclassOp
        Dim pInputRaster As IGeoDataset = Nothing
        Dim retVal As BA_ReturnCode = BA_ReturnCode.UnknownError
        Try
            Dim filePath As String = "PleaseReturn"
            Dim fileName As String = BA_GetBareName(inputFullPath, filePath)
            Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(inputFullPath)
            If workspaceType = workspaceType.Raster Then
                pInputRaster = BA_OpenRasterFromFile(filePath, fileName)
            ElseIf workspaceType = workspaceType.Geodatabase Then
                pInputRaster = BA_OpenRasterFromGDB(filePath, fileName)
            End If
            pGeoDataset = pReclassOp.ReclassByRemap(pInputRaster, numberRemap, False)
            Dim oFilePath As String = "PleaseReturn"
            Dim oFileName As String = BA_GetBareName(outputFullPath, oFilePath)
            BA_SaveRasterDatasetGDB(pGeoDataset, oFilePath, BA_RASTER_FORMAT, oFileName)
            retVal = BA_ReturnCode.Success
            Return retVal
        Catch ex As Exception
            MessageBox.Show("BA_ReclassRasterFromRemap Exception: " + ex.Message)
            Return retVal
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pInputRaster)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pReclassOp)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGeoDataset)
        End Try
    End Function

    ' Run HRU zone delineation IRule and persist output
    Public Function BA_RunRule(ByVal aoiPath As String, ByVal hruOutputPath As String, _
                               ByRef ruleToRun As BAGIS_ClassLibrary.IRule) As BA_ReturnCode
        Dim success As BA_ReturnCode = BA_ReturnCode.UnknownError
        If TypeOf ruleToRun Is RasterSliceRule Then
            Dim nextRule As RasterSliceRule = CType(ruleToRun, RasterSliceRule)
            success = BA_SliceRaster(nextRule.InputFolderPath, hruOutputPath, _
                                           nextRule.OutputDatasetName, nextRule.SliceType, _
                                           nextRule.ZoneCount, nextRule.BaseZone)
        ElseIf TypeOf ruleToRun Is RasterReclassRule Then
            Dim nextRule As RasterReclassRule = CType(ruleToRun, RasterReclassRule)
            Dim newFullPath As String = hruOutputPath & "\" & ruleToRun.OutputDatasetName
            Dim snapRasterPath As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
            If nextRule.ReclassItems IsNot Nothing AndAlso nextRule.ReclassItems.Length > 0 Then
                success = BA_ReclassifyRasterFromTable(nextRule.InputFolderPath, nextRule.ReclassField, _
                                                       nextRule.ReclassItems, newFullPath, ActionType.ReclDisc, _
                                                       snapRasterPath)
            Else
                success = BA_ReclassifyRasterFromReclassTextItems(nextRule.InputFolderPath, nextRule.ReclassField, _
                                                                  nextRule.ReclassTextItems, newFullPath, snapRasterPath)
            End If
        ElseIf TypeOf ruleToRun Is ReclassContinuousRule Then
            Dim nextRule As ReclassContinuousRule = CType(ruleToRun, ReclassContinuousRule)
            Dim newFullPath As String = hruOutputPath & "\" & ruleToRun.OutputDatasetName
            Dim snapRasterPath As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
            success = BA_ReclassifyRasterFromTable(nextRule.InputFolderPath, nextRule.ReclassField, _
                                                   nextRule.ReclassItems, newFullPath, ActionType.ReclCont, _
                                                   snapRasterPath)
        ElseIf TypeOf ruleToRun Is PrismPrecipRule Then
            Dim nextRule As PrismPrecipRule = CType(ruleToRun, PrismPrecipRule)
            success = BA_RunPrismPrecipRule(aoiPath, hruOutputPath, nextRule)
        ElseIf TypeOf ruleToRun Is TemplateRule Then
            Dim nextRule As TemplateRule = CType(ruleToRun, TemplateRule)
            success = BA_RunTemplateRule(aoiPath, hruOutputPath, nextRule)
            'DAFlow tag
        ElseIf TypeOf ruleToRun Is DAFlowTypeZonesRule Then
            Dim nextRule As DAFlowTypeZonesRule = CType(ruleToRun, DAFlowTypeZonesRule)
            success = BA_CreateDAFlowZones(hruOutputPath, ruleToRun.OutputDatasetName, nextRule)
        ElseIf TypeOf ruleToRun Is ContributingAreasRule Then
            'CA tag
            Dim nextRule As ContributingAreasRule = CType(ruleToRun, ContributingAreasRule)
            success = BA_CreateContributingAreas(aoiPath, hruOutputPath, ruleToRun.OutputDatasetName, nextRule)
        End If
        If TypeOf ruleToRun Is TemplateRule Then
            success = AddReclassAttribToLulcOutput(ruleToRun, hruOutputPath, ruleToRun.OutputDatasetName)
        End If
        If success = BA_ReturnCode.Success Then
            ruleToRun.FactorStatus = FactorStatus.Complete
        Else
            ruleToRun.FactorStatus = FactorStatus.Failed
        End If
        Return success
    End Function

    'Returns number of IRules in a list that have FactorStatus of Complete
    Public Function BA_CompletedRuleCount(ByVal rules As List(Of BAGIS_ClassLibrary.IRule)) As Integer
        Dim retVal As Integer = 0
        If IsNothing(rules) Then
            Return retVal
        End If
        For Each rule In rules
            If rule.FactorStatus = FactorStatus.Complete Then
                retVal = retVal + 1
            End If
        Next
        Return retVal
    End Function

    ' Accepts a List of raster filepaths as input; Add the raster bands together and persist
    ' in the output path/file provided by the caller
    Public Function BA_SumRasterBands(ByVal filePaths As List(Of String), ByVal aoiPath As String, _
                                      ByVal pOutPath As String, ByVal pOutFile As String) As BA_ReturnCode
        Dim pRasterDataset As IGeoDataset = Nothing
        Dim pTempBandCol As IRasterBandCollection = Nothing
        Dim pBandCol As IRasterBandCollection = New Raster
        Dim pRasterBand As IRasterBand = Nothing
        Dim pLocalOp As ILocalOp = New RasterLocalOp
        Dim pOutRaster As IGeoDataset = Nothing
        Dim tempGeodataset As IGeoDataset = Nothing
        Try
            If BA_File_ExistsRaster(aoiPath, pOutPath) Then
                BA_Remove_Raster(aoiPath, pOutPath)
            End If
            For Each filePath In filePaths
                Dim parentPath As String = "Please return"
                Dim fName As String = BA_GetBareName(filePath, parentPath)
                'pRasterDataset = BA_OpenRasterFromFile(parentPath, fName)
                pRasterDataset = BA_OpenRasterFromGDB(parentPath, fName)
                pTempBandCol = CType(pRasterDataset, IRasterBandCollection)
                pRasterBand = pTempBandCol.Item(0)
                pBandCol.AppendBand(pRasterBand)
            Next
            pOutRaster = pLocalOp.LocalStatistics(pBandCol, esriGeoAnalysisStatisticsEnum.esriGeoAnalysisStatsSum)
            'Save raster temporarily to a GRID at the AOI root
            'Geoanalyst tool unable to save to file GDB
            BA_SaveRasterDataset(pOutRaster, aoiPath, pOutFile)

            Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(pOutPath)
            If workspaceType = workspaceType.Geodatabase Then
                pOutRaster = BA_OpenRasterFromFile(aoiPath, pOutFile)
                BA_SaveRasterDatasetGDB(pOutRaster, pOutPath, BA_RASTER_FORMAT, pOutFile)
                BA_Remove_Raster(aoiPath, pOutFile)
            End If

            Return BA_ReturnCode.Success
        Catch ex As Exception
            MessageBox.Show("BA_SumRasterBands Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(tempGeodataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pOutRaster)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pBandCol)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBand)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTempBandCol)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterDataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pLocalOp)
        End Try

    End Function

    ' Sums selected prism rasters before slicing
    Public Function BA_RunPrismPrecipRule(ByVal aoiPath As String, ByVal hruOutputPath As String, _
                                          ByVal prismRule As PrismPrecipRule) As BA_ReturnCode
        Dim pRasterStatistics As IRasterStatistics = Nothing
        Dim success As BA_ReturnCode = BA_ReturnCode.UnknownError
        Try
            Dim rasterResolutionPath As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
            'Get cell size from DEM grid to resample PRISM data
            Dim cellSize As Double = 0
            pRasterStatistics = BA_GetRasterStatsGDB(rasterResolutionPath, cellSize)
            'Assemble temporary output path for resampled PRISM data
            Dim tempOutputFile As String = BA_TEMP_PREFIX & "1"
            Dim tempOutputPath As String = hruOutputPath & "\" & tempOutputFile
            Dim newFullPath = hruOutputPath & "\" & prismRule.OutputDatasetName
            If prismRule.DataRange <> PrismDataRange.Custom Then
                Dim snapRasterPath As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
                success = BA_Resample_Raster(prismRule.InputFolderPath, tempOutputPath, cellSize, snapRasterPath)
                If success = BA_ReturnCode.Success Then
                    Dim tmpReclassItems(prismRule.ReclassItems.GetUpperBound(0)) As ReclassItem
                    If prismRule.DisplayUnits <> prismRule.DataUnits Then
                        If prismRule.DisplayUnits = MeasurementUnit.Millimeters And prismRule.DataUnits = MeasurementUnit.Inches Then
                            ' Convert millimeters to inches
                            For i As Integer = 0 To prismRule.ReclassItems.Length - 1
                                Dim item As New ReclassItem(prismRule.ReclassItems(i).FromValue, prismRule.ReclassItems(i).ToValue, prismRule.ReclassItems(i).OutputValue)
                                item.FromValue = item.FromValue / BA_Inches_To_Millimeters
                                item.ToValue = item.ToValue / BA_Inches_To_Millimeters
                                tmpReclassItems(i) = item
                            Next
                        ElseIf prismRule.DisplayUnits = MeasurementUnit.Inches And prismRule.DataUnits = MeasurementUnit.Millimeters Then
                            ' Convert inches to millimeters
                            For i As Integer = 0 To prismRule.ReclassItems.Length - 1
                                Dim item As New ReclassItem(prismRule.ReclassItems(i).FromValue, prismRule.ReclassItems(i).ToValue, prismRule.ReclassItems(i).OutputValue)
                                item.FromValue = item.FromValue * BA_Inches_To_Millimeters
                                item.ToValue = item.ToValue * BA_Inches_To_Millimeters
                                tmpReclassItems(i) = item
                            Next
                        End If
                    Else
                        System.Array.Copy(prismRule.ReclassItems, tmpReclassItems, tmpReclassItems.Length)
                    End If
                    success = BA_ReclassifyRasterFromTable(tempOutputPath, BA_FIELD_VALUE, _
                                                           tmpReclassItems, newFullPath, ActionType.ReclCont, snapRasterPath)
                    If success = BA_ReturnCode.Success Then
                        BA_RemoveFilesByPrefix(hruOutputPath, BA_TEMP_PREFIX)
                    End If
                End If
            Else
                'code for custom data range
                Dim monthList As List(Of Integer) = BA_PrismMonthList(prismRule)
                Dim strPath As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Prism, True)
                Dim nextFolder As String
                ' Build list of filePaths from list of months
                Dim filePaths As New List(Of String)
                For Each pMonth In monthList
                    nextFolder = BA_GetPrismFolderName(pMonth + 1)
                    Dim prismPath = strPath & nextFolder
                    filePaths.Add(prismPath)
                Next
                ' Call function to sum the raster bands together
                success = BA_SumRasterBands(filePaths, aoiPath, hruOutputPath, BA_EnumDescription(MapsFileName.sum))
                ' Slice the summed raster
                If success = BA_ReturnCode.Success Then
                    prismRule.InputFolderPath = hruOutputPath & "\" & BA_EnumDescription(MapsFileName.sum)
                    Dim snapRasterPath As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
                    success = BA_Resample_Raster(prismRule.InputFolderPath, tempOutputPath, cellSize, snapRasterPath)
                    If success = BA_ReturnCode.Success Then
                        success = BA_ReclassifyRasterFromTable(tempOutputPath, BA_FIELD_VALUE, _
                                                               prismRule.ReclassItems, newFullPath, ActionType.ReclCont, snapRasterPath)
                        If success = BA_ReturnCode.Success Then
                            BA_RemoveFilesByPrefix(hruOutputPath, BA_TEMP_PREFIX)
                        End If
                    End If
                End If
            End If
            Return success
        Catch ex As Exception
            MessageBox.Show("BA_RunPrismPrecipRule Exception: " & ex.Message)
            Return success
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterStatistics)
        End Try

    End Function

    'Extracts the list of prism months to add together from a PrismPrecipRule
    'Builds list of months to add together. Used for custom prism date ranges
    Public Function BA_PrismMonthList(ByVal prismRule As PrismPrecipRule) As List(Of Integer)
        Dim monthList As New List(Of Integer)
        Dim monthDiff As Short
        If prismRule.fMonthIdx = prismRule.lMonthIdx Then 'only one month is selected
            monthList.Add(prismRule.fMonthIdx)
        ElseIf prismRule.fMonthIdx < prismRule.lMonthIdx Then 'single calendar year span
            monthDiff = prismRule.lMonthIdx - prismRule.fMonthIdx + 1
            Dim monthCount As Short = 1
            For i = 1 To monthDiff
                monthList.Add(prismRule.fMonthIdx + i - 1)
            Next
        ElseIf prismRule.fMonthIdx > prismRule.lMonthIdx Then 'cross-calendar year span
            monthDiff = (12 - prismRule.fMonthIdx) + prismRule.lMonthIdx + 1
            For i = 0 To prismRule.lMonthIdx
                monthList.Add(i)
            Next
            For i = prismRule.fMonthIdx To 11
                monthList.Add(i)
            Next
        End If
        Return monthList
    End Function

    ' Run HRU zone delineation IRule template and persist output
    Public Function BA_RunTemplateRule(ByVal aoiPath As String, ByVal outputFolderPath As String, _
                                       ByVal tRule As TemplateRule) As BA_ReturnCode
        Dim retVal As BA_ReturnCode = BA_ReturnCode.UnknownError

        Try
            Dim maskFolder As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Aoi, True)
            Dim maskFile As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.AoiVector), False)
            Dim snapRasterPath As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
            tRule.TemplateActions.Sort()
            Dim pos As Integer = 1
            Dim newFilePath As String = ""
            Dim lastAction As Boolean = False
            For Each pAction In tRule.TemplateActions
                If pos >= tRule.TemplateActions.Count Then lastAction = True
                Dim inputFolderPath As String
                'If it's the first time through, the input layer is the original source
                If pos = 1 Then
                    inputFolderPath = tRule.InputFolderPath
                    'If later time through, the input layer is the result of the previous action
                Else
                    inputFolderPath = newFilePath
                End If
                'Assemble output file name
                'Output file name = actionType.toString() unless it is too long; Then we trim it
                'Dim outputFileName As String = pAction.actionType.ToString
                'If outputFileName.Length > BA_GRID_NAME_MAX_LENGTH Then
                '    outputFileName = outputFileName.Substring(0, BA_GRID_NAME_MAX_LENGTH)
                'End If
                Dim outputFileName As String = tRule.OutputDatasetName & "_" & CStr(pAction.id)
                'Alternatively if this is the last action, we need to use the standard rule 
                'output file name
                If lastAction = True Then
                    outputFileName = tRule.OutputDatasetName
                End If
                If pAction.actionType = ActionType.ReclCont Then
                    'CONTINUOUS RECLASS
                    Dim reclassItems As ReclassItem() = pAction.parameters(ActionParameter.ReclassItems)
                    'tempRaster = BA_ReclassRasterFromDoubleTable(inputFolderPath, reclassItems)
                    Dim reclassField As String = pAction.parameters(ActionParameter.ReclassField)
                    Dim reclFilePath As String = outputFolderPath & "\" & outputFileName
                    retVal = BA_ReclassifyRasterFromTable(inputFolderPath, reclassField, _
                              reclassItems, reclFilePath, ActionType.ReclCont, snapRasterPath)
                    If retVal <> BA_ReturnCode.Success Then
                        Throw New Exception("An error occurred while reclassing")
                    End If
                ElseIf pAction.actionType = ActionType.ReclDisc Then
                    'DISCRETE RECLASS
                    Dim reclassItems As ReclassItem() = pAction.parameters(ActionParameter.ReclassItems)
                    Dim reclassField As String = pAction.parameters(ActionParameter.ReclassField)
                    Dim reclFilePath As String = outputFolderPath & "\" & outputFileName
                    retVal = BA_ReclassifyRasterFromTable(inputFolderPath, reclassField, _
                                                          reclassItems, reclFilePath, ActionType.ReclDisc, _
                                                          snapRasterPath)
                    If retVal <> BA_ReturnCode.Success Then
                        Throw New Exception("An error occurred while reclassing")
                    End If
                    'BA_AddReclassAttribToRas(reclFilePath, reclassItems)
                ElseIf pAction.actionType = ActionType.MajorityFilter Then
                    'MAJORITY FILTER
                    Dim height As Integer = CInt(pAction.parameters(ActionParameter.RectangleHeight))
                    Dim width As Integer = CInt(pAction.parameters(ActionParameter.RectangleWidth))
                    Dim iterations As Integer = CInt(pAction.parameters(ActionParameter.IterationCount))
                    retVal = BA_FilterRaster(inputFolderPath, outputFolderPath, _
                                             outputFileName, maskFolder, maskFile, _
                                             snapRasterPath, height, width, iterations, _
                                             esriGeoAnalysisStatisticsEnum.esriGeoAnalysisStatsMajority, _
                                             aoiPath)
                    If retVal <> BA_ReturnCode.Success Then
                        Throw New Exception("An error occurred while filtering")
                    End If
                ElseIf pAction.actionType = ActionType.LowPassFilter Then
                    'LOW PASS FILTER
                    Dim height As Integer = CInt(pAction.parameters(ActionParameter.RectangleHeight))
                    Dim width As Integer = CInt(pAction.parameters(ActionParameter.RectangleWidth))
                    Dim iterations As Integer = CInt(pAction.parameters(ActionParameter.IterationCount))
                    retVal = BA_FilterRaster(inputFolderPath, outputFolderPath, _
                                             outputFileName, maskFolder, maskFile, _
                                             snapRasterPath, height, width, iterations, _
                                             esriGeoAnalysisStatisticsEnum.esriGeoAnalysisStatsMean, _
                                             aoiPath)
                    If retVal <> BA_ReturnCode.Success Then
                        Throw New Exception("An error occurred while filtering")
                    End If
                End If
                ' Update newFilePath; Will be used as input for the next pass
                ' If there is one
                newFilePath = outputFolderPath & "\" & outputFileName
                pos = pos + 1
            Next
            Return retVal
        Catch ex As Exception
            MessageBox.Show("BA_RunTemplateRule Exception: " & ex.Message)
            Return retVal
        Finally

        End Try

    End Function

    ' Returns the file name for an HRU IRule file when provided with the full file path.
    ' Assumes the file name is 'grid'
    Public Function BA_RuleFileName(ByVal ruleFilePath As String) As String
        If String.IsNullOrEmpty(ruleFilePath) Then
            Return Nothing
        Else
            Dim pos As Integer = ruleFilePath.LastIndexOf("\")
            If pos > -1 Then
                Return ruleFilePath.Substring(pos + 1, 4)
            Else
                Return Nothing
            End If
        End If
    End Function

    ' Applies filter to a raster and persists filtered raster
    Public Function BA_FilterRaster(ByVal inputFilePath As String, ByVal outputParentPath As String, _
                                    ByVal outputFileName As String, _
                                    ByVal maskFolder As String, ByVal maskFile As String, _
                                    ByVal snapRasterPath As String, _
                                    ByVal height As Integer, ByVal width As Integer, _
                                    ByVal iterations As Integer, ByVal statisticType As esriGeoAnalysisStatisticsEnum, _
                                    ByVal tempOutputFileName As String) As BA_ReturnCode
        Dim retValue As BA_ReturnCode = BA_ReturnCode.UnknownError
        Dim inputParentPath As String = "Please return"
        Dim inputFileName As String = BA_GetBareName(inputFilePath, inputParentPath)
        retValue = BA_AddRasFilter(inputParentPath, inputFileName, outputParentPath, outputFileName, _
                                   maskFolder, maskFile, snapRasterPath, width, height, statisticType, _
                                   iterations, tempOutputFileName)
        Return retValue
    End Function

    ' Interrogates a collection of LayerListItems from a ListBox form control to see 
    ' if a given layer is in the ListBox
    Public Function BA_LayerInList(ByVal layerName As String, ByVal listBox As System.Windows.Forms.ListBox)
        Dim inList As Boolean = False
        For Each listItem As LayerListItem In listBox.Items
            Dim name As String = listItem.Name
            If String.Compare(layerName, name) = 0 Then
                Return True
            End If
        Next
        Return inList
    End Function

    ' Moves a file from one folder to another. Checks first to see if the target file is in the
    ' current map document. If so, this layer is removed before the file is moved
    Public Function BA_MoveLayer(ByVal pmxDoc As IMxDocument, ByVal gdbFolder As String, _
                                 ByVal oldPrefix As String, ByVal newPrefix As String) As BA_ReturnCode

        Try
            BA_RemoveLayerByFileNamePrefix(pmxDoc, gdbFolder, oldPrefix)
            Return BA_RenameFilesByPrefix(gdbFolder, oldPrefix, newPrefix)
        Catch ex As Exception
            MessageBox.Show("BA_MoveLayer Exception: " & ex.Message)
            Return Nothing
        End Try
    End Function

    ' Reorder rule list when a rule is deleted. Rule file paths are dependent upon the order of
    ' the rules. If a completed rule's index changes during the sort process, the corresponding
    ' files will be moved. Checks first to see if any layers in target rule are in the
    ' current map document. If so, these layers are removed before the rule is moved
    Public Sub BA_SortPendingRules(ByVal pmxDoc As IMxDocument, ByRef ruleList As List(Of BAGIS_ClassLibrary.IRule), _
                                   ByVal hruOutputPath As String)
        Dim position As Integer = 1
        For Each pRule In ruleList
            If pRule.RuleId > position Then
                Dim newRulePrefix As String = "r" & position.ToString("000")
                If pRule.FactorStatus = FactorStatus.Complete Then
                    ' Need to rename/move the output
                    Dim oldRulePrefix As String = "r" & pRule.RuleId.ToString("000")
                    BA_MoveLayer(pmxDoc, hruOutputPath, oldRulePrefix, newRulePrefix)
                End If
                pRule.RuleId = position
                pRule.OutputDatasetName = newRulePrefix
            End If
            position += 1
        Next
    End Sub

    ' Converts a an IList of selected zones to an array of Zone objects
    Public Function BA_ReadSelectedZones(ByVal selZonesList As IList) As Zone()
        Dim selectZonesArray(0) As Zone
        If selZonesList IsNot Nothing Then
            System.Array.Resize(selectZonesArray, selZonesList.Count())
            Dim pos As Integer = 0
            For Each nextObj In selZonesList
                Dim nextZone As Long = CType(nextObj, Long) ' Explicit cast
                selectZonesArray(pos) = New Zone(nextZone)
                pos += 1
            Next
        End If
        Return selectZonesArray
    End Function

    ' Converts a an IList of selected zones to an array of type Long
    Public Function BA_ReadSelectedValues(ByVal selValuesList As IList) As Long()
        Dim selectValuesArray(0) As Long
        If selValuesList IsNot Nothing Then
            System.Array.Resize(selectValuesArray, selValuesList.Count())
            Dim pos As Integer = 0
            For Each nextObj In selValuesList
                Dim nextValue As Long = CType(nextObj, Long) ' Explicit cast
                selectValuesArray(pos) = nextValue
                pos += 1
            Next
        End If
        Return selectValuesArray
    End Function

    'Copies integer values from DataGridView form control to an array of ReclassItem objects
    'Takes column indexes for the input and output value columns as arguments
    Public Function BA_CopyIntegerValuesFromGridToArray(ByVal inputGrid As Windows.Forms.DataGridView, ByVal idxFromValue As Integer, _
                                                        ByVal idxOutputValue As Integer) As ReclassItem()
        Dim rowCount As Integer = inputGrid.Rows.Count
        Dim reclassItems(rowCount - 1) As ReclassItem
        For i = 0 To rowCount - 1
            Dim nextRow As DataGridViewRow = inputGrid.Rows(i)
            Dim fromValue As Integer = CInt(nextRow.Cells(idxFromValue).Value)
            'Validating for idxOutputValue works for FrmHruRasterReclassRule; If this breaks something else we'll have to pass
            'the validation index in as an argument
            If nextRow.Cells(idxOutputValue).Value Is Nothing Then
                Dim missingRow As String = CStr(i + 1)
                MessageBox.Show("Value missing in row " & missingRow & ". New value required for all rows")
                Return Nothing
            Else
                Dim outputValue As Integer = CInt(nextRow.Cells(idxOutputValue).Value)
                Dim nextItem As ReclassItem = New ReclassItem
                nextItem.FromValue = fromValue
                nextItem.ToValue = fromValue
                nextItem.OutputValue = outputValue
                reclassItems(i) = nextItem
            End If
        Next
        Return reclassItems
    End Function

    'Copies integer values from DataGridView form control to an array of ReclassItem objects
    'Takes column indexes for the input and output value columns as arguments
    Public Function BA_CopyReclassTextItemsFromGridToArray(ByVal inputGrid As Windows.Forms.DataGridView, ByVal idxFromValue As Integer, _
                                                           ByVal idxOutputValue As Integer) As ReclassTextItem()
        Dim rowCount As Integer = inputGrid.Rows.Count
        Dim reclassTextItems(rowCount - 1) As ReclassTextItem
        For i = 0 To rowCount - 1
            Dim nextRow As DataGridViewRow = inputGrid.Rows(i)
            Dim fromValue As String = CStr(nextRow.Cells(idxFromValue).Value)
            'Validating for idxOutputValue works for FrmHruRasterReclassRule; If this breaks something else we'll have to pass
            'the validation index in as an argument
            If nextRow.Cells(idxOutputValue).Value Is Nothing Then
                Dim missingRow As String = CStr(i + 1)
                MessageBox.Show("Value missing in row " & missingRow & ". To values required for all rows")
                Return Nothing
            Else
                Dim outputValue As Integer = CInt(nextRow.Cells(idxOutputValue).Value)
                Dim nextItem As ReclassTextItem = New ReclassTextItem(fromValue, outputValue)
                reclassTextItems(i) = nextItem
            End If
        Next
        Return reclassTextItems
    End Function

    'Copies Double, Integer, and String values from DataGridView form control to an array of ReclassItem objects. 
    'Takes column indexes for the from, to, description, and output value columns as arguments.
    Public Function BA_CopyTemplateItemsFromGridToArray(ByVal inputGrid As Windows.Forms.DataGridView, ByVal idxFromValue As Integer, _
                                                        ByVal idxFromDescr As Integer, ByVal idxOutputValue As Integer, _
                                                        ByVal idxOutputDescr As Integer) As ReclassItem()
        Dim rowCount As Integer = inputGrid.Rows.Count
        Dim reclassItems(rowCount - 1) As ReclassItem
        Dim counter As Integer
        For i = 0 To rowCount - 1
            Dim nextRow As DataGridViewRow = inputGrid.Rows(i)
            Dim fromValue As Integer = CInt(nextRow.Cells(idxFromValue).Value)
            Dim fromDescr As String = CStr(nextRow.Cells(idxFromDescr).Value)
            If nextRow.Cells(idxFromValue).Value Is Nothing Then
                ' Do Nothing. No from value = no row to add
            Else
                Dim outputValue As Integer = CInt(nextRow.Cells(idxOutputValue).Value)
                Dim outputDescr As String = CStr(nextRow.Cells(idxOutputDescr).Value)
                Dim nextItem As ReclassItem = New ReclassItem
                nextItem.FromValue = fromValue
                nextItem.FromDescr = fromDescr
                nextItem.ToValue = fromValue
                nextItem.OutputValue = outputValue
                nextItem.OutputDescr = outputDescr
                reclassItems(i) = nextItem
                counter += 1
            End If
        Next
        System.Array.Resize(reclassItems, counter)
        Return reclassItems
    End Function

    'NOT USED AS OF JUNE 14, 2011 LCB
    'Public Function BA_CopyDoubleValuesFromGridToArray(ByVal inputGrid As Windows.Forms.DataGridView, ByVal idxFromValue As Integer, _
    '                                                   ByVal idxOutputValue As Integer) As ReclassItem()
    '    Dim rowCount As Integer = inputGrid.Rows.Count
    '    Dim reclassItems(rowCount - 1) As ReclassItem
    '    For i = 0 To rowCount - 1
    '        Dim nextRow As DataGridViewRow = inputGrid.Rows(i)
    '        Dim fromValue As Double = CDbl(nextRow.Cells(idxFromValue).Value)
    '        Dim toValue As Double = CDbl(nextRow.Cells(idxOutputValue).Value)
    '        If nextRow.Cells(idxFromValue).Value Is Nothing Then
    '            ' Do Nothing. No from value = no row to add
    '        Else
    '            Dim outputValue As Double = CDbl(nextRow.Cells(2).Value)
    '            Dim nextItem As ReclassItem = New ReclassItem
    '            nextItem.FromValue = fromValue
    '            nextItem.ToValue = toValue
    '            nextItem.OutputValue = outputValue
    '            reclassItems(i) = nextItem
    '        End If
    '    Next
    '    Return reclassItems
    'End Function

    'Copies Double values from DataGridView form control to an array of ReclassItem objects. 
    'Takes column indexes for the from, to, and output value columns as arguments
    Public Function BA_CopySlopeValuesFromGridToArray(ByVal inputGrid As Windows.Forms.DataGridView, ByVal idxFromValue As Integer, _
                                                      ByVal idxToValue As Integer, ByVal idxOutputValue As Integer) As ReclassItem()
        Dim rowCount As Integer = inputGrid.Rows.Count
        Dim reclassItems(rowCount - 1) As ReclassItem
        For i = 0 To rowCount - 1
            Dim nextRow As DataGridViewRow = inputGrid.Rows(i)
            Dim fromValue As Double = CDbl(nextRow.Cells(idxFromValue).Value)
            Dim toValue As Double = CDbl(nextRow.Cells(idxToValue).Value)
            If nextRow.Cells(idxToValue).Value Is Nothing Then
                MessageBox.Show("Value missing in row " & i.ToString & ". To values required for all rows")
                Return Nothing
            Else
                Dim outputValue As Double = CDbl(nextRow.Cells(idxOutputValue).Value)
                Dim nextItem As ReclassItem = New ReclassItem
                nextItem.FromValue = fromValue
                nextItem.ToValue = toValue
                nextItem.OutputValue = outputValue
                reclassItems(i) = nextItem
            End If
        Next
        Return reclassItems
    End Function

    'Copies Double values from DataGridView form control to an ArcObjects ITable object and persists them 
    Public Function BA_CopyValuesFromGridToTable(ByVal folderPath As String, ByVal grid As DataGridView, _
                                                 ByVal gridFromIdx As Short, ByVal gridToIdx As Short, _
                                                 ByVal gridOutIdx As Short, ByVal actionType As ActionType, _
                                                 ByVal tableName As String) As BA_ReturnCode

        Dim pWSFactory As IWorkspaceFactory = New ShapefileWorkspaceFactory
        Dim pTable As ITable = Nothing
        Dim pFeatWSpace As IFeatureWorkspace = Nothing
        Dim pWorkspace2 As IWorkspace2 = Nothing
        Dim deleteCursor As ICursor = Nothing
        Dim deleteRow As IRow = Nothing
        Dim objectClassDescription As IObjectClassDescription = Nothing
        Dim pFields As IFields = Nothing
        Dim fieldsEdit As IFieldsEdit = Nothing
        Dim fieldChecker As IFieldChecker = Nothing
        Dim enumFieldError As IEnumFieldError = Nothing
        Dim validatedFields As IFields = Nothing
        Dim pCursor As ICursor = Nothing
        Dim pRowBuffer As IRowBuffer = Nothing

        Dim fromIdx As Short = -1
        Dim toIdx As Short = -1
        Dim outIdx As Short = -1
        Dim retVal As BA_ReturnCode = BA_ReturnCode.UnknownError

        Try
            ' Open the workspace.
            Dim directoryInfo_check As DirectoryInfo = New DirectoryInfo(folderPath)
            If directoryInfo_check.Exists Then
                pFeatWSpace = pWSFactory.OpenFromFile(folderPath, 0)
                pWorkspace2 = CType(pFeatWSpace, IWorkspace2)   'Explicit cast
                If pWorkspace2.NameExists(esriDatasetType.esriDTTable, tableName) Then
                    'If pFeatWSpace.NameExists(esriDatasetType.esriDTTable, tableName) Then
                    pTable = pFeatWSpace.OpenTable(tableName)
                    deleteCursor = pTable.Update(Nothing, False)
                    deleteRow = deleteCursor.NextRow
                    While deleteRow IsNot Nothing
                        deleteRow.Delete()
                        deleteRow = deleteCursor.NextRow()
                    End While
                    fromIdx = pTable.FindField(BA_FIELD_FROM)
                    toIdx = pTable.FindField(BA_FIELD_TO)
                    outIdx = pTable.FindField(BA_FIELD_OUT)
                End If

                objectClassDescription = New ObjectClassDescription
                pFields = objectClassDescription.RequiredFields
                fieldsEdit = CType(pFields, IFieldsEdit) ' Explicit Cast
                ' From column
                If fromIdx < 0 Then
                    Dim fromField As IField = New Field
                    Dim fromFieldEdit As IFieldEdit = CType(fromField, IFieldEdit) ' Explicit Cast
                    fromFieldEdit.Name_2 = BA_FIELD_FROM
                    fromFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
                    fromFieldEdit.IsNullable_2 = True
                    fromFieldEdit.Editable_2 = True
                    If pTable Is Nothing Then
                        ' Add the field to the field collection
                        fieldsEdit.AddField(fromFieldEdit)
                    Else
                        ' Add the field to the existing table if missing
                        pTable.AddField(fromFieldEdit)
                    End If
                End If

                'To column
                If toIdx < 0 Then
                    Dim toField As IField = New Field
                    Dim toFieldEdit As IFieldEdit = CType(toField, IFieldEdit) ' Explicit Cast
                    toFieldEdit.Name_2 = BA_FIELD_TO
                    toFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
                    toFieldEdit.IsNullable_2 = True
                    toFieldEdit.Editable_2 = True
                    If pTable Is Nothing Then
                        ' Add the field to the field collection
                        fieldsEdit.AddField(toFieldEdit)
                    Else
                        ' Add the field to the existing table if missing
                        pTable.AddField(toFieldEdit)
                    End If
                End If

                'Out column
                If outIdx < 0 Then
                    Dim outField As IField = New Field
                    Dim outFieldEdit As IFieldEdit = CType(outField, IFieldEdit) ' Explicit Cast
                    outFieldEdit.Name_2 = BA_FIELD_OUT
                    outFieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeInteger
                    outFieldEdit.IsNullable_2 = True
                    outFieldEdit.Editable_2 = True
                    If pTable Is Nothing Then
                        ' Add the field to the field collection
                        fieldsEdit.AddField(outFieldEdit)
                    Else
                        ' Add the field to the existing table if missing
                        pTable.AddField(outFieldEdit)
                    End If
                End If

                ' Create new table with fields collection
                If pTable Is Nothing Then
                    pFields = CType(fieldsEdit, ESRI.ArcGIS.Geodatabase.IFields) ' Explicit Cast
                    ' Use IFieldChecker to create a validated fields collection.
                    fieldChecker = New ESRI.ArcGIS.Geodatabase.FieldChecker
                    enumFieldError = Nothing
                    validatedFields = Nothing
                    fieldChecker.ValidateWorkspace = pFeatWSpace
                    fieldChecker.Validate(pFields, enumFieldError, validatedFields)
                    pTable = pFeatWSpace.CreateTable(tableName, validatedFields, Nothing, Nothing, "")
                End If

                ' Get indexes for the columns we need
                fromIdx = pTable.FindField(BA_FIELD_FROM)
                toIdx = pTable.FindField(BA_FIELD_TO)
                outIdx = pTable.FindField(BA_FIELD_OUT)

                pCursor = pTable.Insert(False)
                pRowBuffer = pTable.CreateRowBuffer

                Dim rowCount As Integer = grid.Rows.Count

                For i = 0 To rowCount - 1
                    Dim nextRow As DataGridViewRow = grid.Rows(i)
                    Dim fromValue As Double = CDbl(nextRow.Cells(gridFromIdx).Value)
                    If nextRow.Cells(gridOutIdx).Value Is Nothing Then
                        MessageBox.Show("Value missing in row " & i.ToString & ". Output values required for all rows")
                        Return Nothing
                    Else
                        Dim toValue As Double
                        If actionType = actionType.ReclDisc Then
                            toValue = fromValue
                        ElseIf actionType = actionType.ReclCont Then
                            toValue = CDbl(nextRow.Cells(gridToIdx).Value)
                        End If
                        Dim newValue As Integer = CDbl(nextRow.Cells(gridOutIdx).Value)
                        pRowBuffer.Value(fromIdx) = fromValue
                        pRowBuffer.Value(toIdx) = toValue
                        pRowBuffer.Value(outIdx) = newValue
                        pCursor.InsertRow(pRowBuffer)
                        pCursor.Flush()
                    End If
                Next
            End If
            MessageBox.Show("Reclass " & tableName & " successfully saved", "Reclass saved", MessageBoxButtons.OK, MessageBoxIcon.Information)
            retVal = BA_ReturnCode.Success
            Return retVal
        Catch ex As Exception
            MessageBox.Show("BA_CopyValuesFromGridToTable Exception: " + ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(deleteRow)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(deleteCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(objectClassDescription)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFields)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(fieldsEdit)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRowBuffer)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(validatedFields)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(enumFieldError)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(fieldChecker)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeatWSpace)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTable)
        End Try

    End Function

    'Adds contiguous/non-contiguous id columns to the hru vector
    'If contiguous hru only, remove the original hru grid and re-generate it from the
    'hru vector using the HRUID_CO field as the grid value
    Public Function BA_ProcessNonContiguousGrids(ByVal allowNonContiguous As Boolean, _
                                                  ByVal vOutputPath As String, _
                                                  ByVal hruOutputPath As String, _
                                                  ByVal cellSize As Double, _
                                                  ByVal snapRasterPath As String) As BA_ReturnCode

        Dim success As BA_ReturnCode = BA_ReturnCode.UnknownError
        'add HRUID_CO and HRUID_NC fields to the Vector file
        If BA_AddCTAndNonCTToAttrib(vOutputPath) <> BA_ReturnCode.Success Then
            Throw New Exception("Error adding CT and NonCT to Shape file.")
        End If

        Dim vOutputFileName As String = BA_GetBareName(vOutputPath)
        If allowNonContiguous = False Then
            'Delete initial grid so we can overwrite it
            Dim gridName As String = BA_EnumDescription(PublicPath.HruGrid)
            gridName = gridName.Remove(0, 1) 'remove the backslash
            Dim retVal As Integer = BA_RemoveRasterFromGDB(hruOutputPath, gridName)

            If retVal = 1 Then
                'Convert the feature class to raster in final grid file
                Dim outRasterPath As String = hruOutputPath & BA_EnumDescription(PublicPath.HruGrid)
                success = BA_Feature2RasterGP(vOutputPath, outRasterPath, BA_FIELD_HRUID_CO, cellSize, snapRasterPath)
                If success = BA_ReturnCode.Success Then
                    'Ensure that the grid_code in grid_v matches the grid that was generated during BA_Feature2RasterGP
                    BA_UpdateGridCodeForContig(hruOutputPath, vOutputFileName)
                End If
            End If
        Else
            Dim polyFileName As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruPolyVector), False)
            success = BA_RenameFeatureClassInGDB(hruOutputPath, vOutputFileName, polyFileName)
            If success = BA_ReturnCode.Success Then
                success = BA_Dissolve(hruOutputPath & "\" & polyFileName, BA_FIELD_HRUID_NC, vOutputPath)
                If success = BA_ReturnCode.Success Then
                    BA_UpdateRequiredColumns(hruOutputPath, vOutputFileName)
                End If
            End If
        End If
        Return success
    End Function

    'Adds reclass attribute descriptions to the output of the LULC template
    Private Function AddReclassAttribToLulcOutput(ByVal pRule As BAGIS_ClassLibrary.IRule, ByVal inputFilePath As String, _
                                                  ByVal inputFileName As String) As BA_ReturnCode
        Dim tempRule As TemplateRule = CType(pRule, TemplateRule) ' Explicit cast
        If tempRule.RuleType = HruRuleType.LandUse Then
            ' Find reclass items
            For Each pAction In tempRule.TemplateActions
                If pAction.actionType = ActionType.ReclDisc Then
                    Dim reclassItems As ReclassItem() = pAction.parameters(ActionParameter.ReclassItems)
                    If reclassItems IsNot Nothing AndAlso reclassItems.Length > 0 Then
                        Return BA_AddReclassAttribToRas(inputFilePath, inputFileName, reclassItems)
                    End If
                End If
            Next
        Else
            Return BA_ReturnCode.Success
        End If
    End Function

    ' Copy values from hruid_nc to grid_code field, and from OBJECT_ID to Id field 
    ' for compability with rest of app; Used for non-contig zones where we have to 
    ' dissolve/regenerate the vector layer
    Public Sub BA_UpdateRequiredColumns(ByVal featClassPath, ByVal featClassName)
        Dim fc As IFeatureClass = Nothing
        Dim pCursor As IFeatureCursor = Nothing
        Dim pFeature As IFeature = Nothing

        Try
            fc = BA_OpenFeatureClassFromGDB(featClassPath, featClassName)
            Dim idxGridCd As Integer = fc.FindField(BA_FIELD_GRIDCODE_GDB)
            'Try alternate name for grid_code for compatibility with later versions of ArcMap
            If idxGridCd < 0 Then
                idxGridCd = fc.FindField(BA_FIELD_GRIDCODE)
            End If
            Dim idxId As Integer = fc.FindField(BA_FIELD_ID)

            If idxGridCd < 0 Then
                Dim pFieldCont As IFieldEdit = New Field
                With pFieldCont
                    .Type_2 = esriFieldType.esriFieldTypeInteger
                    .Name_2 = BA_FIELD_GRIDCODE_GDB
                End With
                fc.AddField(pFieldCont)
                idxGridCd = fc.FindField(BA_FIELD_GRIDCODE_GDB)
            End If

            If idxId < 0 Then
                Dim pFieldCont As IFieldEdit = New Field
                With pFieldCont
                    .Type_2 = esriFieldType.esriFieldTypeInteger
                    .Name_2 = BA_FIELD_ID
                End With
                fc.AddField(pFieldCont)
                idxId = fc.FindField(BA_FIELD_ID)
            End If


            Dim idxNc As Short = fc.FindField(BA_FIELD_HRUID_NC)
            Dim idxObjId As Short = fc.FindField(BA_FIELD_OBJECT_ID)

            pCursor = fc.Update(Nothing, False)
            pFeature = pCursor.NextFeature
            Do While Not pFeature Is Nothing
                pFeature.Value(idxGridCd) = pFeature.Value(idxNc)
                pFeature.Value(idxId) = pFeature.Value(idxObjId)
                pCursor.UpdateFeature(pFeature)
                pFeature = pCursor.NextFeature
            Loop
        Catch ex As Exception
            MsgBox("BA_UpdateRequiredColumns Exception" & ex.Message)
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(fc)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeature)
        End Try

    End Sub

    ' Copy values from hruid_co to grid_code field 
    ' for compability with rest of app; Used for contig zones where we have to 
    ' dissolve/regenerate the grid layer
    Public Sub BA_UpdateGridCodeForContig(ByVal featClassPath, ByVal featClassName)
        Dim fc As IFeatureClass = Nothing
        Dim pCursor As IFeatureCursor = Nothing
        Dim pFeature As IFeature = Nothing

        Try
            fc = BA_OpenFeatureClassFromGDB(featClassPath, featClassName)
            Dim idxGridCd As Integer = fc.FindField(BA_FIELD_GRIDCODE_GDB)
            'Try alternate name for grid_code for compatibility with later versions of ArcMap
            If idxGridCd < 0 Then
                idxGridCd = fc.FindField(BA_FIELD_GRIDCODE)
            End If

            If idxGridCd < 0 Then
                Dim pFieldCont As IFieldEdit = New Field
                With pFieldCont
                    .Type_2 = esriFieldType.esriFieldTypeInteger
                    .Name_2 = BA_FIELD_GRIDCODE_GDB
                End With
                fc.AddField(pFieldCont)
                idxGridCd = fc.FindField(BA_FIELD_GRIDCODE_GDB)
            End If

            Dim idxCo As Short = fc.FindField(BA_FIELD_HRUID_CO)

            pCursor = fc.Update(Nothing, False)
            pFeature = pCursor.NextFeature
            Do While Not pFeature Is Nothing
                pFeature.Value(idxGridCd) = pFeature.Value(idxCo)
                pCursor.UpdateFeature(pFeature)
                pFeature = pCursor.NextFeature
            Loop
        Catch ex As Exception
            MsgBox("BA_UpdateGridCodeForContig Exception" & ex.Message)
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(fc)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeature)
        End Try

    End Sub

    Public Sub BA_CopyValuesFromTableToGrid(ByVal fileTable As ITable, ByVal outputGrid As DataGridView)
        Dim pCursor As ICursor = Nothing
        Dim pRow As IRow = Nothing
        Try
            Dim fromFileIdx As Short = fileTable.FindField(BA_FIELD_FROM)
            Dim toFileIdx As Short = fileTable.FindField(BA_FIELD_TO)
            Dim outFileIdx As Short = fileTable.FindField(BA_FIELD_OUT)
            pCursor = fileTable.Search(Nothing, False)
            pRow = pCursor.NextRow
            While pRow IsNot Nothing
                Dim pFrom As Double = CDbl(pRow.Value(fromFileIdx))
                Dim pTo As Double = CDbl(pRow.Value(toFileIdx))
                Dim pOut As Double = CDbl(pRow.Value(outFileIdx))
                Dim pArray As Object() = {pFrom, pTo, pOut}
                outputGrid.Rows.Add(pArray)
                pRow = pCursor.NextRow
            End While
        Catch ex As Exception
            MessageBox.Show("BA_CopyValuesFromTableToGrid Exception: " + ex.Message)
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRow)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pCursor)
        End Try

    End Sub

    Public Function BA_RenameHRU(ByVal aoiPath As String, ByVal oldHruName As String, _
                                  ByVal newHruName As String, ByVal listItems As ListBox.ObjectCollection) As BA_ReturnCode
        Dim oldGdbPath As String = BA_GetHruPathGDB(aoiPath, PublicPath.HruDirectory, oldHruName)
        Dim newGdbPath As String = BA_GetHruPathGDB(aoiPath, PublicPath.HruDirectory, newHruName)
        Dim oldGdbFolder As String = "blah"
        Dim oldFileName As String = BA_GetBareName(oldGdbPath, oldGdbFolder)
        Dim newGdbFolder As String = "blah"
        Dim newFileName As String = BA_GetBareName(newGdbPath, newGdbFolder)
        Try
            'Rename parent folder that contains all HRU data
            Dim success As BA_ReturnCode = BA_RenameWorkspace(oldGdbFolder, newGdbFolder)
            If success = BA_ReturnCode.Success Then
                Dim tmpGdbPath As String = newGdbFolder & "\" & oldFileName
                'Rename .gdb folder containing the file geodatabase
                success = BA_RenameWorkspace(tmpGdbPath, newGdbPath)
                If success = BA_ReturnCode.Success Then
                    'Open that log.xml file from the recently renamed hru folder
                    Dim pAoi As Aoi = BA_LoadHRUFromXml(newGdbFolder)
                    If pAoi IsNot Nothing Then
                        Dim hruList As List(Of Hru) = pAoi.HruList
                        'Retrieve the hru from the xml
                        Dim hru As Hru = hruList(0)
                        'Rename the hru in the xml
                        hru.Name = newHruName
                        hru.FilePath = newGdbPath
                        hruList(0) = hru
                        pAoi.HruList = hruList
                        Dim xmlOutputPath As String = newGdbFolder & BA_EnumDescription(PublicPath.HruXml)
                        'Save the updated xml
                        pAoi.Save(xmlOutputPath)
                    End If
                    'Loop through all hru in aoi to rename this hru where it is a parent
                    For Each item In listItems
                        Dim listItem As LayerListItem = CType(item, LayerListItem)
                        Dim listPath As String = BA_GetHruPath(aoiPath, PublicPath.HruDirectory, listItem.Name)
                        'This loads nothing for the renamed hru but that's okay
                        Dim nextAoi As Aoi = BA_LoadHRUFromXml(listPath)
                        If nextAoi IsNot Nothing Then
                            Dim hruList As List(Of Hru) = nextAoi.HruList
                            'Retrieve the hru from the xml
                            Dim hru As Hru = hruList(0)
                            If hru IsNot Nothing Then
                                Dim parentHru As Hru = hru.ParentHru
                                While parentHru IsNot Nothing
                                    If parentHru.Name = oldHruName Then
                                        parentHru.Name = newHruName
                                        parentHru.FilePath = newGdbPath
                                        Dim xmlOutputPath As String = listPath & BA_EnumDescription(PublicPath.HruXml)
                                        nextAoi.Save(xmlOutputPath)
                                    End If
                                    parentHru = parentHru.ParentHru
                                End While
                            End If
                        End If
                    Next
                End If
            End If
            Return success
        Catch ex As Exception
            MsgBox("BA_RenameHRU Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        End Try
    End Function

    ' Populates an HRU objectfrom an XML file
    Public Function BA_LoadRulesFromXml(ByVal aoiPath As String) As Hru
        Try
            Dim xmlInputPath As String = aoiPath & BA_EnumDescription(PublicPath.RulesXml)
            If BA_File_ExistsWindowsIO(xmlInputPath) Then
                Dim obj As Object = SerializableData.Load(xmlInputPath, GetType(Hru))
                If obj IsNot Nothing Then
                    Dim pHru As Hru = CType(obj, Hru)
                    Return pHru
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Debug.Print("BA_LoadRulesFromXml Exception: " & ex.Message)
            Return Nothing
        End Try
    End Function

    Public Function BA_AddAttributesToHru(ByVal aoiPath As String, ByVal zoneFilePath As String, _
                                          ByVal ruleFilePath As String, _
                                          ByVal pRules As List(Of BAGIS_ClassLibrary.IRule), _
                                          ByVal parentLyr As String) As BA_ReturnCode
        Dim aoiGdbPath = aoiPath & "\" & BA_EnumDescription(GeodatabaseNames.Aoi)
        Dim snapRasterPath As String = aoiGdbPath & "\" & BA_EnumDescription(AOIClipFile.AOIExtentCoverage)
        Dim success As Short = 1
        Dim tempFileName As String = Nothing
        For Each nextRule In pRules
            tempFileName = nextRule.OutputDatasetName & "Tbl"
            Dim valueLyr As String = ruleFilePath & "\" & nextRule.OutputDatasetName
            success = BA_ZonalStats2Att(zoneFilePath, GRID, zoneFilePath, tempFileName, _
                                                     valueLyr, nextRule.OutputDatasetName, _
                                                     snapRasterPath, True, StatisticsTypeString.MAJORITY)
            If success < 1 Then
                Return BA_ReturnCode.UnknownError
            End If
        Next
        If success > 0 And Not String.IsNullOrEmpty(parentLyr) Then
            tempFileName = "parTbl"
            success = BA_ZonalStats2Att(zoneFilePath, GRID, zoneFilePath, tempFileName, _
                                        parentLyr, "parent", snapRasterPath, True, StatisticsTypeString.MAJORITY)
        End If
        If success > 0 Then
            Return BA_ReturnCode.Success
        Else
            Return BA_ReturnCode.UnknownError
        End If
    End Function

    Public Sub BA_GetMeasurementUnitsForAoi(ByVal aoiPath As String, ByRef slopeUnit As SlopeUnit, _
                                            ByRef elevUnit As MeasurementUnit, ByRef depthUnit As MeasurementUnit)
        'Slope units
        Dim inputFolder As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Surfaces)
        Dim inputFile As String = BA_GetBareName(BA_EnumDescription(PublicPath.Slope))
        Dim tagsList As IList(Of String) = BA_ReadMetaData(inputFolder, inputFile, _
                                                   LayerType.Raster, BA_XPATH_TAGS)
        If tagsList IsNot Nothing Then
            For Each pInnerText As String In tagsList
                'This is our BAGIS tag
                If pInnerText.IndexOf(BA_BAGIS_TAG_PREFIX) = 0 Then
                    Dim strUnits As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_VALUE_TAG)
                    If strUnits IsNot Nothing Then
                        slopeUnit = BA_GetSlopeUnit(strUnits)
                    End If
                    Exit For
                End If
            Next
        End If

        'Elevation units
        inputFile = BA_EnumDescription(MapsFileName.filled_dem_gdb)
        tagsList = BA_ReadMetaData(inputFolder, inputFile, _
                                   LayerType.Raster, BA_XPATH_TAGS)
        If tagsList IsNot Nothing Then
            For Each pInnerText As String In tagsList
                'This is our BAGIS tag
                If pInnerText.IndexOf(BA_BAGIS_TAG_PREFIX) = 0 Then
                    Dim strUnits As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_VALUE_TAG)
                    If strUnits IsNot Nothing Then
                        elevUnit = BA_GetMeasurementUnit(strUnits)
                    End If
                    Exit For
                End If
            Next
        End If

        'Depth units
        inputFolder = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Prism)
        inputFile = AOIPrismFolderNames.annual.ToString
        tagsList = BA_ReadMetaData(inputFolder, inputFile, _
                           LayerType.Raster, BA_XPATH_TAGS)
        If tagsList IsNot Nothing Then
            For Each pInnerText As String In tagsList
                'This is our BAGIS tag
                If pInnerText.IndexOf(BA_BAGIS_TAG_PREFIX) = 0 Then
                    Dim strUnits As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_VALUE_TAG)
                    If strUnits IsNot Nothing Then
                        depthUnit = BA_GetMeasurementUnit(strUnits)
                    End If
                    Exit For
                End If
            Next
        End If

    End Sub

    Public Function BA_AssignSequentialHruId(ByVal aoiPath As String, ByVal hruName As String) As BA_ReturnCode
        Dim hruGDS As IGeoDataset
        Dim pRasterBandColl As IRasterBandCollection
        Dim pRasterBand As IRasterBand
        Dim pTable As ITable
        Dim pField As IFieldEdit
        Dim gridField As IField
        Dim gridFields As IFields
        Dim pCursor As ICursor
        Dim pRow As IRow
        Try
            'Open hru grid
            Dim hruFolder As String = BA_GetHruPathGDB(aoiPath, PublicPath.HruDirectory, hruName)
            hruGDS = BA_OpenRasterFromGDB(hruFolder, GRID)
            If hruGDS IsNot Nothing Then
                pRasterBandColl = CType(hruGDS, IRasterBandCollection)
                pRasterBand = pRasterBandColl.Item(0)
                pTable = pRasterBand.AttributeTable
                'Create new column for new id
                Dim idxNewId As Short = pTable.FindField(BA_FIELD_NEW_HRU_ID)
                Dim idxValue As Short = pTable.FindField(BA_FIELD_VALUE)
                If idxNewId = -1 Then
                    pField = New Field
                    pField.Name_2 = BA_FIELD_NEW_HRU_ID
                    gridFields = pTable.Fields
                    gridField = gridFields.Field(idxValue)
                    pField.Type_2 = gridField.Type
                    pTable.AddField(pField)
                    idxNewId = pTable.FindField(BA_FIELD_NEW_HRU_ID)
                End If

                'Open a cursor on the grid
                pCursor = pTable.Update(Nothing, False)
                pRow = pCursor.NextRow
                Dim newId As Integer = 1
                'Look at each record
                Do Until pRow Is Nothing
                    'Copy grid value to old id column
                    pRow.Value(idxNewId) = newId
                    'Store record
                    pCursor.UpdateRow(pRow)
                    'Increment id by 1
                    newId += 1
                    'Next record
                    pRow = pCursor.NextRow
                Loop
                Return BA_ReturnCode.Success
            End If
            Return BA_ReturnCode.UnknownError
         Catch ex As Exception
            Debug.Print("BA_AssignSequentialHruId Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            hruGDS = Nothing
            pRasterBand = Nothing
            pRasterBandColl = Nothing
            pTable = Nothing
            pField = Nothing
            gridField = Nothing
            gridFields = Nothing
            pCursor = Nothing
            pRow = Nothing
        End Try
 
    End Function

End Module


