Public Class BtnPublicMethodEditor
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()

  End Sub

    Protected Overrides Sub OnClick()
        Try

            Dim frmMethod As FrmEditMethod = New FrmEditMethod()
            frmMethod.ShowDialog()
        Catch ex As Exception
            Dim errMsg As String = BA_GetButtonErrorMessage("Method Editor", ex)
            Windows.Forms.MessageBox.Show(errMsg, "Failed to open form", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        End Try
    End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
