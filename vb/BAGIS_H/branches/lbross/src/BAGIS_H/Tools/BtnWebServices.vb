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
        Dim wForm As FrmWebservices = New FrmWebservices
        wForm.ShowDialog()
        'AccessImageServerLayer()
        'AccessFeatureLayer()
        'MsgBox("Finish!")
    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub

    Protected Sub AccessImageServerLayer()
        Dim clipFilePath As String = "C:\Docs\Lesley\teton_aoi\aoi.gdb\aoib"
        Dim imageUrl As String = "http://atlas.geog.pdx.edu/arcgis/services/30_Meters_DEM/westus_30m/ImageServer"
        Dim newFilePath As String = "C:\Docs\Lesley\teton_aoi\layers.gdb\dem_web"
        Dim success As BA_ReturnCode = BA_ClipImageService(clipFilePath, imageUrl, newFilePath)
    End Sub

    Protected Sub AccessFeatureLayer()
        Dim aoiPath As String = "C:\Docs\Lesley\teton_aoi"
        Dim clipFilePath As String = aoiPath & "\aoi.gdb\aoib_v"
        Dim webServiceUrl As String = "http://atlas.geog.pdx.edu/arcgis/rest/services/AWDB/AWDB_COOP/FeatureServer/0"
        Dim newFilePath As String = aoiPath & "\layers.gdb\snotel_sites_web"
        Dim success As BA_ReturnCode = BA_ClipFeatureService(clipFilePath, webServiceUrl, newFilePath, aoiPath)
    End Sub

    Protected Sub QueryFields()
        Dim webServiceUrl As String = "http://atlas.geog.pdx.edu/arcgis/rest/services/AWDB_ALL/AWDB_SNOTEL_ALL/FeatureServer/0"
        Dim fieldNames As IList(Of String) = BA_QueryFeatureServiceFieldNames(webServiceUrl)
    End Sub

End Class
