Public Class BtnToolsDataUnits
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()

  End Sub

    Protected Overrides Sub OnClick()
        Try
            Dim frmUnits As FrmToolsDataUnits = New FrmToolsDataUnits
            frmUnits.ShowDialog()
        Catch ex As Exception
            Windows.Forms.MessageBox.Show("An error occurred while accessing the data units.", "System Error")
            Debug.Print("BtnToolsDataUnits Exception: " & ex.Message)
        End Try
    End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
