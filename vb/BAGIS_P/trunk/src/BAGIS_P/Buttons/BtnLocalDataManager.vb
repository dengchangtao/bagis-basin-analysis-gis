Public Class BtnLocalDataManager
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()

  End Sub

  Protected Overrides Sub OnClick()
        Try
            Dim frmDataLayerManager As FrmDataManager = New FrmDataManager(BAGIS_ClassLibrary.BA_BAGISP_MODE_LOCAL)
            frmDataLayerManager.ShowDialog()
        Catch ex As Exception
            Dim errMsg As String = BA_GetButtonErrorMessage("Data Layer Manager", ex)
            Windows.Forms.MessageBox.Show(errMsg, "Failed to open form", Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Error)
        End Try
  End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
