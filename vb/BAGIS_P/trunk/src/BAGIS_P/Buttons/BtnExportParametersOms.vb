Public Class BtnExportParametersOms
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()

  End Sub

  Protected Overrides Sub OnClick()
        Try
            Dim frmExportParameters As FrmExportParametersOms = New FrmExportParametersOms
            frmExportParameters.ShowDialog()
        Catch ex As Exception
            Dim errMsg As String = BA_GetButtonErrorMessage("Export Parameters (PRMS)", ex)
            Windows.Forms.MessageBox.Show(errMsg, "Failed to open form", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        End Try
  End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
