Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Geometry
Imports BAGIS_ClassLibrary
Imports System.Web
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.DataSourcesGDB
Imports System.Text

Public Class BtnWebServices
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()

    End Sub

    Protected Overrides Sub OnClick()
        'AccessImageServerLayer()
        AccessFeatureLayer()
    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub

    'http://resources.arcgis.com/en/help/arcobjects-net/conceptualhelp/index.html#/How_to_create_an_image_server_layer/00010000047t000000/
    Protected Sub AccessImageServerLayer()

        Try
            'Create an image server layer by passing a URL.
            Dim isLayer As IImageServerLayer = New ImageServerLayerClass
            Dim URL As String = "http://atlas.geog.pdx.edu/arcgis/services/30_Meters_DEM/westus_30m/ImageServer"
            isLayer.Initialize(URL)

            'For services that require https/authentication, use the following
            'Dim isLayer As IImageServerLayer = CreateSecuredISLayer("http://server:6080/arcgis/services", "serviceName")

            'Get the raster from the image server layer.
            Dim raster As IRaster = isLayer.Raster

            'The raster from an image server is normally large;
            'define the size of the raster.
            Dim rasterProps As IRasterProps = DirectCast(raster, IRasterProps)
            Dim clipEnvelope As IEnvelope = GetEnvelope()
            '@ToDo: May need to worry about the projection in real-life
            'clipEnvelope.PutCoords(779000, 9628000, 786000, 9634000)
            rasterProps.Extent = clipEnvelope
            rasterProps.Width = 1822
            rasterProps.Height = 1771

            'Save the clipped raster to the file system.
            BA_SaveRasterDataset(raster, "C:\Docs\Lesley\teton_aoi", "isLayer")
            MsgBox("I'm done!")
        Catch ex As Exception
            Debug.Print("AccessImageServerLayer Exception:" & ex.Message)
        End Try
    End Sub

    Protected Sub AccessFeatureLayer()
        Try
            Dim sb As StringBuilder = New StringBuilder()
            'url base for query
            sb.Append("http://atlas.geog.pdx.edu/arcgis/rest/services/AWDB/AWDB_COOP/FeatureServer/0/query?")
            'units for output
            sb.Append("units=esriSRUnit_Meter")
            'append the query; where clause is required; This one returns all records
            sb.Append("&where={0}")
            'return all fields
            sb.Append("&outFields=*")
            'return the geometries
            sb.Append("&returnGeometry=true")
            'append the geometry type for spatial quer
            sb.Append("&geometryType=esriGeometryPolygon")
            'append the spatial relation
            sb.Append("&spatialRel=esriSpatialRelIntersects")
            'append the geometry
            Dim strGeo As String = GetGeometry()
            sb.Append("&geometry=" & strGeo)
            'return results in JSON format
            sb.Append("&f=json")
            'Dim query As String = sb.ToString
            Dim query As String = String.Format(sb.ToString, HttpUtility.UrlEncode(String.Format("OBJECTID>{0}", 0)))
            Dim jsonFeatures As String = GetResult(query)
            Dim jsonReader As IJSONReader = New JSONReader
            jsonReader.ReadFromString(jsonFeatures)
            Dim JSONConverterGdb As IJSONConverterGdb = New JSONConverterGdb()
            Dim originalToNewFieldMap As IPropertySet = Nothing
            Dim recordSet As IRecordSet = Nothing
            JSONConverterGdb.ReadRecordSet(jsonReader, Nothing, Nothing, recordSet, originalToNewFieldMap)
            Dim workspaceFactory As IWorkspaceFactory = New FileGDBWorkspaceFactory
            Dim workspace As IFeatureWorkspace = workspaceFactory.OpenFromFile("C:\Docs\Lesley\teton_aoi\aoi.gdb", 0)
            Dim searchWS As IWorkspace2 = CType(workspace, IWorkspace2)
            Dim recordSet2 As IRecordSet2 = CType(recordSet, IRecordSet2)
            'Delete existing file if it exists
            Dim fName As String = "newFClass"
            If searchWS.NameExists(esriDatasetType.esriDTFeatureClass, fName) Then
                Dim oldClass As IFeatureClass = workspace.OpenFeatureClass(fName)
                Dim pDataset As IDataset = CType(oldClass, IDataset)
                pDataset.Delete()
            End If
            recordSet2.SaveAsTable(workspace, fName)

            MsgBox("Finish!")
        Catch ex As Exception
            Debug.Print("AccessFeatureLayer Exception:" & ex.Message)
        End Try
    End Sub

    Protected Function GetResult(ByVal url As String) As String
        Dim req As System.Net.WebRequest = System.Net.WebRequest.Create(url)
        Dim resp As System.Net.WebResponse = req.GetResponse()
        Using stream As System.IO.Stream = resp.GetResponseStream
            Using streamReader As System.IO.StreamReader = New System.IO.StreamReader(stream)
                Return streamReader.ReadToEnd
            End Using
        End Using
    End Function


    Protected Function GetEnvelope() As IEnvelope
        Dim aoiFolder As String = "C:\Docs\Lesley\teton_aoi\aoi.gdb"
        Dim aoiFile As String = "aoib_v"
        Dim fClass As IFeatureClass = BA_OpenFeatureClassFromGDB(aoiFolder, aoiFile)

        'retrieve IFeature from FeatureClass
        Dim pClipFCursor As IFeatureCursor = fClass.Search(Nothing, False)
        Dim pClipFeature As IFeature = pClipFCursor.NextFeature
        Dim pGeo As IGeometry = pClipFeature.Shape
        Return (pGeo.Envelope)
    End Function

    Protected Function GetGeometry() As String
        Dim aoiFolder As String = "C:\Docs\Lesley\teton_aoi\aoi.gdb"
        Dim aoiFile As String = "aoib_v"
        Dim fClass As IFeatureClass = BA_OpenFeatureClassFromGDB(aoiFolder, aoiFile)

        'retrieve IFeature from FeatureClass
        Dim pClipFCursor As IFeatureCursor = fClass.Search(Nothing, False)
        Dim pClipFeature As IFeature = pClipFCursor.NextFeature
        Dim pGeo As IGeometry = pClipFeature.Shape
        Dim jsonOut As IJSONObject = New JSONObject
        Dim JSONConverter As IJSONConverterGeometry = New JSONConverterGeometry()
        JSONConverter.QueryJSONGeometry(pGeo, False, jsonOut)
        Return jsonOut.ToJSONString(Nothing)
    End Function

End Class
