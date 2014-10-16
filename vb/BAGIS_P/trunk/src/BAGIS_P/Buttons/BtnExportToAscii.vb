Public Class BtnExportToAscii
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()

  End Sub

  Protected Overrides Sub OnClick()
        Try
            Dim frmExportAscii As New FrmExportToAscii
            frmExportAscii.ShowDialog()
        Catch ex As Exception
            Dim errMsg As String = BA_GetButtonErrorMessage("Export To ASCII", ex)
            Windows.Forms.MessageBox.Show(errMsg, "Failed to open form", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        End Try
  End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
