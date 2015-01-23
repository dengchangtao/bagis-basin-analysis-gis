Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geodatabase
Imports System.Windows.Forms
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Geoprocessing
Imports ESRI.ArcGIS.esriSystem
Imports BAGIS_ClassLibrary

Module DAFlowModule

    Public Function BA_CreateDAFlowZones(ByVal outputFolder As String, ByVal outputName As String, _
                                         ByVal paramDAFlow As DAFlowTypeZonesRule) As BA_ReturnCode
        Dim returnCode As BA_ReturnCode = BA_ReturnCode.UnknownError
        Dim pFeatClass As IFeatureClass
        Dim pEnv As IEnvelope
        Dim pFLayer As IFeatureLayer = New FeatureLayer
        'Dim fileName As String = BA_EnumDescription(AOIClipFile.AOIExtentCoverage)
        Dim filePath As String = "PleaseReturn"
        Dim fileName As String = BA_GetBareName(paramDAFlow.InputFolderPath, filePath)

        Dim m_mapXOff, m_mapYOff As Double 'extend offset
        Dim m_mapX0, m_mapY0 As Double 'extend origin (lower left)

        'pFeatClass = BA_OpenFeatureClassFromFile(filePath, fileName)
        pFeatClass = BA_OpenFeatureClassFromGDB(filePath, fileName)
        pFLayer.FeatureClass = pFeatClass
        pEnv = pFLayer.AreaOfInterest

        m_mapX0 = pEnv.XMin
        m_mapY0 = pEnv.YMin
        m_mapXOff = pEnv.XMax - pEnv.XMin
        m_mapYOff = pEnv.YMax - pEnv.YMin

        returnCode = BA_CreateFishnet(filePath, outputFolder, outputName, m_mapX0, m_mapY0, _
            m_mapX0 + m_mapXOff, m_mapY0 + m_mapYOff, _
            paramDAFlow.HruCol, paramDAFlow.HruRow)

        Return returnCode
    End Function

    'create fishnet based on the AOI shape boundary, return the number of DAFlow zones
    Public Function BA_CreateFishnet(ByVal AOIFolder As String, ByVal outputFolder As String, _
        ByVal outputName As String, _
        ByVal LLMapX As Double, ByVal LLMapY As Double, _
        ByVal URMapX As Double, ByVal URMapY As Double, _
        ByVal numCols As Long, ByVal numRows As Long) As BA_ReturnCode

        Dim AOIShapefileName As String = BA_EnumDescription(AOIClipFile.AOIExtentCoverage)
        'If Not AOIShapefileName.Contains(".shp") Then AOIShapefileName = AOIShapefileName & ".shp"

        'temporary file names
        Dim tempFishNetFileName As String = "tmpDAFlow" 'initial fishnet polygons
        'outputName: final DAFlowZones
        Dim outputClippedVectorName As String = outputName & "_Clip" 'clipped output vector polygon

        If BA_File_Exists(outputFolder & "\" & tempFishNetFileName, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            BA_Remove_ShapefileFromGDB(outputFolder, tempFishNetFileName)
        End If

        If BA_File_Exists(outputFolder & "\" & outputClippedVectorName, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            BA_Remove_ShapefileFromGDB(outputFolder, outputClippedVectorName)
        End If

        Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
        Dim result As ESRI.ArcGIS.Geoprocessing.IGeoProcessorResult2
        ' Set default workspace
        GP.SetEnvironmentValue("workspace", outputFolder)
        GP.AddOutputsToMap = False
        GP.OverwriteOutput = True

        'Dim FishNetTool As ESRI.ArcGIS.DataManagementTools.CreateFishnet = New ESRI.ArcGIS.DataManagementTools.CreateFishnet
        Dim VectorClipTool As ESRI.ArcGIS.AnalysisTools.Clip = New ESRI.ArcGIS.AnalysisTools.Clip
        Dim featuresUpdated As Long = -1

        Try
            ' Populate the FishNetTool with parameter values.
            'With FishNetTool
            '    .template = AOIFolder & "\" & AOIShapefileName & ".shp"
            '    .out_feature_class = tempFishNetFileName & ".shp"
            '    .origin_coord = LLMapX & " " & LLMapY
            '    .y_axis_coord = LLMapX & " " & URMapY
            '    .corner_coord = URMapX & " " & URMapY

            '    .cell_width = 0 'Val(TxtXSize.Text)
            '    .cell_height = 0 'Val(TxtYSize.Text)
            '    .number_rows = numRows
            '    .number_columns = numCols
            '    .labels = "NO_LABELS"
            '    .geometry_type = "POLYGON"
            'End With

            ' Execute the CreateFishnet tool.
            'result = GP.Execute(FishNetTool, Nothing)

            'Call custom fishnet tool due to ArcGIS 10 fishnet bug
            'Dim templateFileName As String = AOIFolder & AOIShapefileName & ".shp"
            Dim templateFileName As String = AOIFolder & "\" & AOIShapefileName
            Dim retVal As BA_ReturnCode = BA_CustomFishnet(templateFileName, outputFolder, _
                                                           tempFishNetFileName, LLMapX, LLMapY, URMapX, URMapY, _
                                                           numCols, numRows)

            ' If the job failed, retrieve the feature result.
            If retVal <> BA_ReturnCode.Success Then
                MessageBox.Show("DAFlowZone Error: CreateFishNet tool execution failed.")
                'FishNetTool = Nothing
                VectorClipTool = Nothing
                GP = Nothing
                Return retVal
            End If

            'perform vector clipping
            With VectorClipTool
                .in_features = outputFolder & "\" & tempFishNetFileName
                .clip_features = AOIFolder & "\" & AOIShapefileName
                .out_feature_class = outputFolder & "\" & outputClippedVectorName
            End With

            ' Execute the CreateFishnet tool.
            result = GP.Execute(VectorClipTool, Nothing)
            'remove temp shapefiles
            BA_Remove_ShapefileFromGDB(outputFolder, tempFishNetFileName)

            ' If the job failed, retrieve the feature result.
            If result Is Nothing Then
                MessageBox.Show("DAFlowZone Error: Vector Clip tool execution failed.")
                GP = Nothing
                'FishNetTool = Nothing
                VectorClipTool = Nothing
                Return retVal
            End If

            'update vector attribute table to create a unique ID for each feature
            Dim tempIDFieldName As String = "BA_TmpID"
            featuresUpdated = BA_AddVectorUniqueID(outputFolder, outputClippedVectorName, tempIDFieldName)

            'convert the vector to raster using the unique ID
            'check if output raster already exists
            If BA_Workspace_Exists(outputFolder & "\" & outputName) Then
                'delete raster
                Dim response As Integer = BA_Remove_Raster(outputFolder, outputName)
                If response = 0 Then 'unable to delete the folder
                    MessageBox.Show("Unable to remove the folder " & outputFolder & "\" & outputName)
                    Return BA_ReturnCode.UnknownError
                End If
            End If

            'vector to raster
            'get raster resolution
            Dim aoiRoot As String = ""
            Dim tmpFile As String = BA_GetBareName(AOIFolder, aoiRoot)
            Dim fullLayerPath As String = aoiRoot & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\" & BA_EnumDescription(MapsFileName.filled_dem_gdb)
            Dim cellSize As Double
            Dim rasterStat As IRasterStatistics = BA_GetRasterStatsGDB(fullLayerPath, cellSize)
            'Dim vFeatureClass As IFeatureClass = BA_OpenFeatureClassFromFile(outputFolder, outputClippedVectorName)
            'Dim vFeatureClass As IFeatureClass = BA_OpenFeatureClassFromGDB(outputFolder, outputClippedVectorName)
            'rasterStat = Nothing

            'If vFeatureClass Is Nothing Then
            '    MessageBox.Show("DAFlowZone Error: Unable to open clipped DAFlowZone file.")
            '    GP = Nothing
            '    'FishNetTool = Nothing
            '    VectorClipTool = Nothing
            '    Return -1
            '    Exit Function
            'End If

            Dim vFeatureClassPath As String = outputFolder & "\" & outputClippedVectorName
            Dim snapRasterPath As String = BA_GetPath(AOIFolder, PublicPath.AoiGrid)
            'BA_Feature2RasterInteger(vFeatureClass, outputFolder, outputName, cellSize, tempIDFieldName, WorkspaceType.Raster)
            retVal = BA_Feature2RasterGP(vFeatureClassPath, outputFolder & "\" & outputName, tempIDFieldName, cellSize, snapRasterPath)
            'vFeatureClass = Nothing

            'remove temp shapefiles
            'BA_Remove_Shapefile(outputFolder, outputClippedVectorName)
            BA_Remove_ShapefileFromGDB(outputFolder, outputClippedVectorName)
            Return retVal
        Catch ex As Exception
            Dim errMsg As String = "CreateFishnet GP Error: "
            For counter As Integer = 0 To GP.MessageCount - 1
                errMsg = errMsg & vbCrLf & GP.GetMessage(counter)
            Next
            MsgBox(errMsg)
            Return BA_ReturnCode.UnknownError
        Finally
            'FishNetTool = Nothing
            VectorClipTool = Nothing
            GP = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Function

    'The function returns the number of unique IDs added to the shapefile
    '-1 indicates error
    '0 no feature in the shapefile
    Public Function BA_AddVectorUniqueID(ByVal FileFolder, ByVal FileName, ByVal IDFieldName) As Long
        Dim i As Long = -1 'UID counter

        'If Not BA_Shapefile_Exists(FileFolder & "\" & FileName) Then
        If Not BA_File_Exists(FileFolder & "\" & FileName, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            Return i
            Exit Function
        End If

        'Dim pFC As IFeatureClass = BA_OpenFeatureClassFromFile(FileFolder, FileName)
        Dim pFC As IFeatureClass = BA_OpenFeatureClassFromGDB(FileFolder, FileName)
        If pFC IsNot Nothing Then
            Dim indexUID As Long = pFC.FindField(IDFieldName)

            ' Check for Unique ID field - add it if it doesn't exist
            If indexUID < 0 Then
                Dim pFieldUID As IFieldEdit = New Field
                With pFieldUID
                    .Type_2 = esriFieldType.esriFieldTypeInteger
                    .Name_2 = IDFieldName
                End With
                pFC.AddField(pFieldUID)
                pFieldUID = Nothing
            End If

            indexUID = pFC.FindField(IDFieldName)

            'update UID
            If indexUID >= 0 Then
                Dim pFCursor As IFeatureCursor = pFC.Update(Nothing, False)
                Dim pFeature As IFeature = pFCursor.NextFeature
                i = 0

                Do While Not pFeature Is Nothing
                    i += 1
                    pFeature.Value(indexUID) = i
                    pFCursor.UpdateFeature(pFeature)
                    pFeature = pFCursor.NextFeature
                Loop

                pFeature = Nothing
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFCursor)
            End If
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFC)
        End If
        Return i
    End Function

    Public Function BA_VectorClip(ByVal InputFolder As String, ByVal InputVector As String, _
        ByVal ClipFolder As String, ByVal ClipVector As String, _
        ByVal OutputFolder As String, ByVal OutputVector As String) As Integer
        Dim returnCode As BA_ReturnCode = BA_ReturnCode.UnknownError

        'Define the input feature class
        Dim pInputFC As IFeatureClass = BA_OpenFeatureClassFromFile(InputFolder, InputVector)
        If pInputFC Is Nothing Then
            Return BA_ReturnCode.ReadError
            Exit Function
        End If

        'Define the overlay table (second in the table of contents)
        Dim pClipFC As IFeatureClass = BA_OpenFeatureClassFromFile(ClipFolder, ClipVector)
        If pClipFC Is Nothing Then
            pInputFC = Nothing
            Return BA_ReturnCode.ReadError
            Exit Function
        End If

        'Define the feature class name and output location
        Dim pNewWSName As IWorkspaceName = New WorkspaceName
        Dim pFeatClassName As IFeatureClassName = New FeatureClassName
        Dim pDatasetName As IDatasetName = pFeatClassName
        pNewWSName.WorkspaceFactoryProgID = "esriDataSourcesFile.ShapefileWorkspaceFactory"
        pNewWSName.PathName = OutputFolder
        pDatasetName.WorkspaceName = pNewWSName
        pDatasetName.Name = OutputVector

        'Perform Intersect
        'Define a basic geoprocessor object
        Dim pBGP As IBasicGeoprocessor = New BasicGeoprocessor
        Dim tol As Double = 0 'Use default tolerance

        'Run clipping
        Try
            pBGP.Intersect(pInputFC, False, pClipFC, False, tol, pFeatClassName)
            returnCode = BA_ReturnCode.Success
        Catch ex As Exception
            returnCode = BA_ReturnCode.OtherError
        Finally
            pBGP = Nothing
            pNewWSName = Nothing
            pDatasetName = Nothing
            pFeatClassName = Nothing
            pInputFC = Nothing
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pClipFC)
            'pClipFC = Nothing
        End Try
        Return returnCode
    End Function

    'Private Function CreateFishnetManagement(ByVal AOIFolder As String, ByVal outputFolder As String, _
    '                                         ByVal tempFishNetFileName As String, ByVal AOIShapefileName As String, _
    '                                         ByVal outputName As String, _
    '                                         ByVal LLMapX As Double, ByVal LLMapY As Double, _
    '                                         ByVal URMapX As Double, ByVal URMapY As Double, _
    '                                         ByVal numCols As Long, ByVal numRows As Long) As BA_ReturnCode
    '    Dim GP As IGeoProcessor2 = New GeoProcessor
    '    Dim parameters As IVariantArray = New VarArray
    '    Dim result As ESRI.ArcGIS.Geoprocessing.IGeoProcessorResult2 = Nothing

    '    Try
    '        ' Set environment variables
    '        GP.SetEnvironmentValue("workspace", outputFolder)
    '        GP.AddOutputsToMap = False
    '        GP.OverwriteOutput = True

    '        'out_feature_class
    '        parameters.Add(tempFishNetFileName & ".shp")
    '        'origin_coord
    '        parameters.Add(LLMapX & " " & LLMapY)
    '        'y_axis_coord
    '        parameters.Add(LLMapX & " " & URMapY)
    '        'cell_width
    '        parameters.Add(0)
    '        'cell_height
    '        parameters.Add(0)
    '        'number_rows
    '        parameters.Add(numRows)
    '        'number_columns
    '        parameters.Add(numCols)
    '        'corner_coord
    '        parameters.Add(URMapX & " " & URMapY)
    '        'labels
    '        parameters.Add("NO_LABELS")
    '        'template
    '        parameters.Add(AOIFolder & "\" & AOIShapefileName & ".shp")
    '        'geometry type
    '        parameters.Add("POLYGON")
    '        result = GP.Execute("CreateFishnet_management", parameters, Nothing)

    '        Return BA_ReturnCode.Success
    '    Catch ex As Exception
    '        MessageBox.Show("DAFlowZone Error: CreateFishNet tool execution failed.")
    '        Dim errMsg As String = Nothing
    '        For counter As Integer = 0 To GP.MessageCount - 1
    '            errMsg = errMsg & vbCrLf & GP.GetMessage(counter)
    '            Debug.Print(errMsg)
    '        Next
    '        Return BA_ReturnCode.UnknownError
    '    Finally
    '        'System.Runtime.InteropServices.Marshal.ReleaseComObject(parameters)
    '        'System.Runtime.InteropServices.Marshal.ReleaseComObject(result)
    '        'System.Runtime.InteropServices.Marshal.ReleaseComObject(GP)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(parameters)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(result)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(GP)
    '    End Try


    'End Function
End Module
