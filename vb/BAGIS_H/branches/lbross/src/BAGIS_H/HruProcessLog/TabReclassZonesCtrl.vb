Imports BAGIS_ClassLibrary

Public Class TabReclassZonesCtrl

    Private Sub BtnViewInput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnViewInput.Click
        Dim filePath As String = TxtInputLayerPath.Text
        Dim displayName As String = TxtInputLayerName.Text
        BA_DisplayRaster(My.ArcMap.Application, filePath, displayName)
    End Sub
End Class
