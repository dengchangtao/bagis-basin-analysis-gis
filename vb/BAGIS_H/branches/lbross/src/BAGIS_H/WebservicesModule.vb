Imports BAGIS_ClassLibrary
Imports System.Text
Imports System.Web
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.DataSourcesGDB
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.DataSourcesRaster

Module WebservicesModule

    Public Function BA_ClipFeatureService(ByVal clipFilePath As String, ByVal webServiceUrl As String, _
                                          ByVal newFilePath As String, ByVal aoiFolder As String) As BA_ReturnCode
        Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(newFilePath)
        If wType = WorkspaceType.Raster Then
            Debug.Print("BA_ClipFeatureService can only write to FileGeodatabase format. Please supply an output path to a FileGeodatabase folder.")
            Return BA_ReturnCode.WriteError
        End If
        Dim sb As StringBuilder = New StringBuilder()
        'url base for query
        sb.Append("http://atlas.geog.pdx.edu/arcgis/rest/services/AWDB/AWDB_COOP/FeatureServer/0/")
        'append the query; where clause is required; This one returns all records
        Dim whereClause As String = "query?&where={0}"
        sb.Append(String.Format(whereClause, HttpUtility.UrlEncode(String.Format("OBJECTID>{0}", 0))))
        'return all fields
        sb.Append("&outFields=*")
        'return the geometries
        sb.Append("&returnGeometry=true")
        'append the geometry type for spatial query
        sb.Append("&geometryType=esriGeometryEnvelope")
        'append the spatial relation
        sb.Append("&spatialRel=esriSpatialRelIntersects")
        'append the geometry
        sb.Append("&geometry=" & GetJSONEnvelope(clipFilePath))
        'return results in JSON format
        sb.Append("&f=json")
        Dim query As String = sb.ToString
        'read the JSON request
        Dim jsonFeatures As String = GetResult(query)

        Dim jsonReader As IJSONReader = New JSONReader
        Dim JSONConverterGdb As IJSONConverterGdb = New JSONConverterGdb()
        Dim originalToNewFieldMap As IPropertySet = Nothing
        Dim recordSet As IRecordSet = Nothing
        Dim recordSet2 As IRecordSet2 = Nothing
        Dim workspaceFactory As IWorkspaceFactory = New FileGDBWorkspaceFactory
        Dim workspace As IFeatureWorkspace = Nothing
        Dim searchWS As IWorkspace2 = Nothing
        Dim deleteFClass As IFeatureClass = Nothing
        Dim deleteDataset As IDataset = Nothing
        Try
            jsonReader.ReadFromString(jsonFeatures)
            JSONConverterGdb.ReadRecordSet(jsonReader, Nothing, Nothing, recordSet, originalToNewFieldMap)
            Dim outputFolder As String = "PleaseReturn"
            Dim outputFile As String = BA_GetBareName(newFilePath, outputFolder)
            ' Strip trailing "\" if exists
            If outputFolder(Len(outputFolder) - 1) = "\" Then
                outputFolder = outputFolder.Remove(Len(outputFolder) - 1, 1)
            End If

            Dim tempFile As String = "webQuery"
            workspace = workspaceFactory.OpenFromFile(outputFolder, 0)
            searchWS = CType(workspace, IWorkspace2)
            recordSet2 = CType(recordSet, IRecordSet2)

            'Delete temp file if it exists
            If searchWS.NameExists(esriDatasetType.esriDTFeatureClass, tempFile) Then
                deleteFClass = workspace.OpenFeatureClass(tempFile)
                deleteDataset = CType(deleteFClass, IDataset)
                deleteDataset.Delete()
            End If

            'Delete output file if it exists
            If searchWS.NameExists(esriDatasetType.esriDTFeatureClass, outputFile) Then
                deleteFClass = workspace.OpenFeatureClass(outputFile)
                deleteDataset = CType(deleteFClass, IDataset)
                deleteDataset.Delete()
            End If

            'Save query results to temp file in target geodatabase
            recordSet2.SaveAsTable(workspace, tempFile)
            'Clip queried layer to aoi
            Dim retVal As Short = BA_ClipAOIVector(aoiFolder, outputFolder & "\" & tempFile, outputFile, outputFolder, True)
            If retVal = 1 Then
                'Delete temporary query file
                'Re-initialize workspace to resolve separated RCW error
                workspace = workspaceFactory.OpenFromFile(outputFolder, 0)
                deleteFClass = workspace.OpenFeatureClass(tempFile)
                deleteDataset = CType(deleteFClass, IDataset)
                deleteDataset.Delete()
                Return BA_ReturnCode.Success
            End If
            Return BA_ReturnCode.UnknownError
        Catch ex As Exception
            Debug.Print("BA_ClipFeatureService Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            recordSet = Nothing
            recordSet2 = Nothing
            workspace = Nothing
            searchWS = Nothing
            deleteFClass = Nothing
            deleteDataset = Nothing
        End Try

    End Function

    Private Function GetJSONEnvelope(ByVal clipFilePath As String) As String
        Dim aoiFolder As String = "Please return"
        Dim aoiFile As String = BA_GetBareName(clipFilePath, aoiFolder)
        Dim fClass As IFeatureClass = Nothing
        Dim pClipFCursor As IFeatureCursor = Nothing
        Dim pClipFeature As IFeature = Nothing
        Dim pEnv As IEnvelope = Nothing
        Dim jsonOut As IJSONObject = New JSONObject
        Dim JSONConverter As IJSONConverterGeometry = New JSONConverterGeometry()

        Try
            Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(aoiFolder)
            If wType = WorkspaceType.Geodatabase Then
                fClass = BA_OpenFeatureClassFromGDB(aoiFolder, aoiFile)
            Else
                fClass = BA_OpenFeatureClassFromFile(aoiFolder, aoiFile)
            End If
            If fClass IsNot Nothing Then
                'retrieve IFeature from FeatureClass
                pClipFCursor = fClass.Search(Nothing, False)
                pClipFeature = pClipFCursor.NextFeature
                pEnv = pClipFeature.Shape.Envelope
                'Querying the geometry returned a string that was too long for the url so we use the envelope insead
                JSONConverter.QueryJSONEnvelope(pEnv, False, jsonOut)
                Return jsonOut.ToJSONString(Nothing)
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Debug.Print("GetJSONEnvelope Exception: " & ex.Message)
            Return Nothing
        Finally
            fClass = Nothing
            pClipFCursor = Nothing
            pClipFeature = Nothing
            pEnv = Nothing
        End Try
    End Function

    Private Function GetResult(ByVal url As String) As String
        Dim req As System.Net.WebRequest = System.Net.WebRequest.Create(url)
        Dim resp As System.Net.WebResponse = req.GetResponse()
        Using stream As System.IO.Stream = resp.GetResponseStream
            Using streamReader As System.IO.StreamReader = New System.IO.StreamReader(stream)
                Return streamReader.ReadToEnd
            End Using
        End Using
    End Function

    'http://resources.arcgis.com/en/help/arcobjects-net/conceptualhelp/index.html#/How_to_create_an_image_server_layer/00010000047t000000/
    Public Function BA_ClipImageService(ByVal clipFilePath As String, ByVal webServiceUrl As String, _
                                        ByVal newFilePath As String) As BA_ReturnCode

        Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(newFilePath)
        If wType = WorkspaceType.Raster Then
            Debug.Print("BA_ClipImageService can only write to FileGeodatabase format. Please supply an output path to a FileGeodatabase folder.")
            Return BA_ReturnCode.WriteError
        End If

        Dim isLayer As IImageServerLayer = New ImageServerLayerClass
        Dim imageRaster As IRaster = Nothing
        Dim imageRasterProps As IRasterProps = Nothing
        Dim clipRaster As IRaster = Nothing
        Dim clipRasterProps As IRasterProps = Nothing
        Try
            'Create an image server layer by passing a URL.
            Dim URL As String = webServiceUrl
            isLayer.Initialize(URL)
            'Get the raster from the image server layer.
            imageRaster = isLayer.Raster

            'For services that require https/authentication, use the following
            'Dim isLayer As IImageServerLayer = CreateSecuredISLayer("http://server:6080/arcgis/services", "serviceName")

            'The raster from an image server is normally large; Define the size of the raster.
            imageRasterProps = DirectCast(imageRaster, IRasterProps)
            clipRaster = GetClipRaster(clipFilePath)
            clipRasterProps = DirectCast(clipRaster, IRasterProps)

            '@ToDo: May need to worry about the projection in real-life
            imageRasterProps.Extent = clipRasterProps.Extent
            imageRasterProps.Width = clipRasterProps.Width
            imageRasterProps.Height = clipRasterProps.Height

            'Save the clipped raster to the file geodatabase.
            Dim newFolder As String = "PleaseReturn"
            Dim newFile As String = BA_GetBareName(newFilePath, newFolder)
            ' Strip trailing "\" if exists
            If newFolder(Len(newFolder) - 1) = "\" Then
                newFolder = newFolder.Remove(Len(newFolder) - 1, 1)
            End If

            'Remove the target raster if it exists
            Dim retVal As Short = 1
            If BA_File_Exists(newFilePath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                retVal = BA_RemoveRasterFromGDB(newFolder, newFile)
            End If
            If retVal = 1 Then
                retVal = BA_SaveRasterDatasetGDB(imageRaster, newFolder, BA_RASTER_FORMAT, newFile)
            End If

            If retVal = 1 Then
                Return BA_ReturnCode.Success
            Else
                Return BA_ReturnCode.UnknownError
            End If
        Catch ex As Exception
            Debug.Print("BA_ClipImageService Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            imageRaster = Nothing
            imageRasterProps = Nothing
            clipRaster = Nothing
            clipRasterProps = Nothing
        End Try
    End Function

    Private Function GetClipRaster(ByVal clipRasterPath As String) As IRaster
        Dim aoiFolder As String = "PleaseReturn"
        Dim aoiFile As String = BA_GetBareName(clipRasterPath, aoiFolder)
        ' Strip trailing "\" if exists
        If aoiFolder(Len(aoiFolder) - 1) = "\" Then
            aoiFolder = aoiFolder.Remove(Len(aoiFolder) - 1, 1)
        End If
        Dim pGeoDataset As IGeoDataset = Nothing
        Dim pRasterDataset As IRasterDataset = Nothing
        Try
            Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(clipRasterPath)
            If wType = WorkspaceType.Geodatabase Then
                pGeoDataset = BA_OpenRasterFromGDB(aoiFolder, aoiFile)
            Else
                pGeoDataset = BA_OpenRasterFromFile(aoiFolder, aoiFile)
            End If
            pRasterDataset = CType(pGeoDataset, IRasterDataset)
            Return pRasterDataset.CreateDefaultRaster
        Catch ex As Exception
            Debug.Print("GetClipRaster Exception: " & ex.Message)
            Return Nothing
        Finally
            pGeoDataset = Nothing
            pRasterDataset = Nothing
        End Try

    End Function

End Module
