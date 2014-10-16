Public Class BtnLocalProfileBuilder
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()

    End Sub

    Protected Overrides Sub OnClick()
        Try
            Dim myForm As FrmProfileBuilder = New FrmProfileBuilder(BAGIS_ClassLibrary.BA_BAGISP_MODE_LOCAL)
            myForm.ShowDialog()
        Catch ex As Exception
            Dim errMsg As String = BA_GetButtonErrorMessage("Profile Builder", ex)
            Windows.Forms.MessageBox.Show(errMsg, "Failed to open form", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        End Try
    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub
End Class
