Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataManagementTools
Imports System.ComponentModel
Imports System.Text
Imports System.Windows.Forms
Imports System.IO

Public Class BtnExportToGdb
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()

    End Sub

    Protected Overrides Sub OnClick()
        Dim exportForm As FrmExportToGdb = New FrmExportToGdb()
        exportForm.ShowDialog()

        'Export raster layers; Due to legacy VBA code, data starts at position 1 in arrays
        'Dim RasterCount As Integer = UBound(AOIRasterList)
        'Dim bTime As DateTime = DateTime.Now
        'If RasterCount > 0 Then
        '    Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
        '    Dim pResult As ESRI.ArcGIS.Geoprocessing.IGeoProcessorResult = Nothing
        '    GP.OverwriteOutput = True
        '    GP.SetEnvironmentValue("cellsize", "MINOF")
        '    GP.AddOutputsToMap = False
        '    For i = 1 To RasterCount
        '        'ts1: 921
        '        'sourceRaster = BA_OpenRasterFromFile(aoiPath, AOIRasterList(i))
        '        'BA_SaveRasterDatasetGDB(sourceRaster, gdbPath, BA_RASTER_FORMAT, AOIRasterList(i))
        '        Dim copyRaster As CopyRaster = New CopyRaster
        '        copyRaster.in_raster = aoiPath & "\" & AOIRasterList(i)
        '        copyRaster.out_rasterdataset = gdbPath & "\" & AOIRasterList(i)
        '        pResult = GP.Execute(copyRaster, Nothing)
        '    Next
        'End If
    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub
End Class
