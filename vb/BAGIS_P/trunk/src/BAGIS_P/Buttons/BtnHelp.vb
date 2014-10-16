Public Class BtnHelp
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()
        Me.Enabled = False
  End Sub

  Protected Overrides Sub OnClick()
        Try
            'Insert Help form here
        Catch ex As Exception
            Dim errMsg As String = BA_GetButtonErrorMessage("Help", ex)
            Windows.Forms.MessageBox.Show(errMsg, "Failed to open form", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        End Try
  End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
