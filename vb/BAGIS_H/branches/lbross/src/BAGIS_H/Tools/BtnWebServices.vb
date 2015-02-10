Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Geometry
Imports BAGIS_ClassLibrary
Imports System.Web
Imports ESRI.ArcGIS.esriSystem

Public Class BtnWebServices
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()

    End Sub

    Protected Overrides Sub OnClick()
        AccessImageServerLayer()
    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub

    'http://resources.arcgis.com/en/help/arcobjects-net/conceptualhelp/index.html#/How_to_create_an_image_server_layer/00010000047t000000/
    Protected Sub AccessImageServerLayer()

        Try
            'Create an image server layer by passing a URL.
            Dim isLayer As IImageServerLayer = New ImageServerLayerClass
            Dim URL As String = "http://atlas.geog.pdx.edu/arcgis/services/30_Meters_DEM/westus_30m"
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
            BA_SaveRasterDataset(raster, "D:\Momeni\AOIs\teton_aoi", "isLayer")
            MsgBox("I'm done!")
        Catch ex As Exception
            Debug.Print("AccessImageServerLayer Exception:" & ex.Message)
        End Try
    End Sub

    Protected Sub AccessFeatureLayer()
        Try
            Dim query As String = String.Format("http://services2.arcgis.com/iaresdfsofjwerr/ArcGIS/rest/services/fognatura/FeatureServer/2/query?where={0}&units=esriSRUnit_Meter&outFields=*&returnGeometry=true&f=json", HttpUtility.UrlEncode(String.Format("OBJECTID>{0}", 0)))
            Dim jsonFeatures As String = GetResult(query)
            Dim jsonReader As IJSONReader = New JSONReader
            jsonReader.ReadFromString(jsonFeatures)
            Dim JSONConverterGdb As IJSONConverterGdb

            'IJSONConverterGdb JSONConverterGdb = new JSONConverterGdbClass();
            'IPropertySet originalToNewFieldMap;
            'IRecordSet recorset;
            'JSONConverterGdb.ReadRecordSet(jsonReader, null, null, out recorset, out originalToNewFieldMap);
            'Type factoryType = Type.GetTypeFromProgID("esriDataSourcesGDB.FileGDBWorkspaceFactory");
            'IWorkspaceFactory workspaceFactory = (IWorkspaceFactory)Activator.CreateInstance(factoryType);
            'IWorkspace workspace = workspaceFactory.OpenFromFile(@"C:\Temp\MyGDB.gdb", 0);
            'IRecordSet2 recordSet2 = recorset as IRecordSet2;
            'recordSet2.SaveAsTable(workspace, "TheNameTable");

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
        Dim aoiFolder As String = "D:\Momeni\AOIs\teton_aoi\aoi.gdb"
        Dim aoiFile As String = "aoib_v"
        Dim fClass As IFeatureClass = BA_OpenFeatureClassFromGDB(aoiFolder, aoiFile)

        'retrieve IFeature from FeatureClass
        Dim pClipFCursor As IFeatureCursor = fClass.Search(Nothing, False)
        Dim pClipFeature As IFeature = pClipFCursor.NextFeature
        Dim pGeo As IGeometry = pClipFeature.Shape
        Return pGeo.Envelope
    End Function

End Class
