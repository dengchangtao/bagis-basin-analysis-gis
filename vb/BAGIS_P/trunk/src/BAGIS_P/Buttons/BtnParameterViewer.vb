Public Class BtnParameterViewer
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()
        'Me.Enabled = False
  End Sub

    Protected Overrides Sub OnClick()
        Try

            Dim frmParameterViewer As FrmParameterViewer = New FrmParameterViewer
            frmParameterViewer.ShowDialog()
        Catch ex As Exception
            Dim errMsg As String = BA_GetButtonErrorMessage("Parameter Viewer", ex)
            Windows.Forms.MessageBox.Show(errMsg, "Failed to open form", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        End Try
    End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
