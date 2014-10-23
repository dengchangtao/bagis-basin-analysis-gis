Imports System.Text
Imports BAGIS_ClassLibrary

Public Class FrmNewSite

    Public Sub New(ByVal siteElevation As Double, ByVal units As ESRI.ArcGIS.esriSystem.esriUnits, _
                   ByVal aoiPath As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim strUnits = BA_StandardizeEsriUnits(units)
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("You are adding a pseudo site at " & Math.Round(siteElevation) & " " & strUnits & ".")
        sb.Append("To save this new pseudo site, please enter a name below. ")
        sb.Append("Click the cancel button if you don't wish to add this site. ")
        TxtMessage.Text = sb.ToString

        Dim layersGDBPath As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Layers, True)
        TxtFCPath.Text = layersGDBPath & BA_EnumDescription(MapsFileName.Pseudo)
        TxtSiteName.Focus()
    End Sub

    Private Sub BtnCancel_Click(sender As System.Object, e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub BtnSave_Click(sender As System.Object, e As System.EventArgs) Handles BtnSave.Click
        Dim siteTool As SelectSiteTool = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of SelectSiteTool)(My.ThisAddIn.IDs.SelectSiteTool)
        siteTool.SiteName = Trim(TxtSiteName.Text)
        Me.Close()
    End Sub
End Class