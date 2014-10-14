Imports ESRI.ArcGIS.Desktop.AddIns
Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.DataSourcesRaster

Public Class frmBasinInfo

    Private Sub frmBasinInfo_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'update caption
        Dim cboSelectedBasin = AddIn.FromID(Of cboTargetedBasin)(My.ThisAddIn.IDs.cboTargetedBasin)
        Me.Tag = cboSelectedBasin.myvalue(Text)
        Me.Text = "Basin: " & Me.Tag

        'display aoi area
        Dim BasinArea As Double
        BasinArea = BA_GetShapeArea(BasinFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Aoi) & "\" & BA_DEMExtentShapefile) / 1000000 'the shape unit is in sq meters, converted to sq km
        If BasinArea < 0 Then
            MsgBox("Unable to read the Basin extent layer!")
            'Unload(Me)
            Me.Close()
            'frmBasinInfo.Hide
            Exit Sub
        End If

        txtArea.Text = Format(BasinArea, "#0.00")
        txtAreaAcre.Text = Format(BasinArea * 247.1044, "#0.00")
        txtAreaSQMile.Text = Format(BasinArea * 0.3861022, "#0.00")

        'display dem elevation stats
        'Dim response As Integer
        'Dim deminfo As BA_DEMInfo = Nothing
        'response = BA_ReadDEMInfo(AOIFolderBase, deminfo)
        'If deminfo.Exist Then
        '    txtMinElev.Text = Math.Round(deminfo.Min - 0.005, 2)
        '    txtMaxElev.Text = Math.Round(deminfo.Max + 0.005, 2)
        'End If

        'display dem elevation stats
        'Get DEM statistics
        Dim raster_res As Double
        Dim pRasterStats As IRasterStatistics = Nothing
        Dim fullfilepath As String = BasinFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\" & BA_EnumDescription(MapsFileName.filled_dem_gdb)

        pRasterStats = BA_GetRasterStatsGDB(fullfilepath, raster_res)

        txtMinElev.Text = Math.Round(pRasterStats.Minimum - 0.005, 2)
        txtMaxElev.Text = Math.Round(pRasterStats.Maximum + 0.005, 2)

        pRasterStats = Nothing

    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        Me.Close()
    End Sub
End Class