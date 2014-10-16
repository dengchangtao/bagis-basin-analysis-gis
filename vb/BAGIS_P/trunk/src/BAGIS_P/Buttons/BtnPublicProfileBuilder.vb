Public Class BtnPublicProfileBuilder
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()

    End Sub

    Protected Overrides Sub OnClick()
        Try
            'Dim frmGroupProfile As FrmGroupProfile = New FrmGroupProfile()
            'frmGroupProfile.ShowDialog()
            Dim frmProfileBuilder As FrmProfileBuilder = New FrmProfileBuilder(BAGIS_ClassLibrary.BA_BAGISP_MODE_PUBLIC)
            frmProfileBuilder.ShowDialog()
        Catch ex As Exception
            Dim errMsg As String = BA_GetButtonErrorMessage("Profile Builder", ex)
            Windows.Forms.MessageBox.Show(errMsg, "Failed to open form", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        End Try
    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub
End Class
