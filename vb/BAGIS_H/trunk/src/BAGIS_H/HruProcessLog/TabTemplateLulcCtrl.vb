Imports System.Windows.Forms
Imports BAGIS_ClassLibrary

Public Class TabTemplateLulcCtrl

    Private Sub BtnViewRule_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnViewRule.Click
        Dim filePath As String = LblHruPath.Text & TxtOutputLayer.Text
        Dim displayName As String = TxtOutputLayer.Text
        BA_DisplayRaster(My.ArcMap.Application, filePath, displayName)
    End Sub

    Private Sub BtnViewInput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnViewInput.Click
        Dim filePath As String = TxtInputLayerPath.Text
        Dim displayName As String = TxtInputLayerName.Text
        BA_DisplayRaster(My.ArcMap.Application, filePath, displayName)
    End Sub

End Class
