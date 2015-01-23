Public Class BtnSnodas
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()

  End Sub

  Protected Overrides Sub OnClick()
        Try
            Dim frmSnodas As FrmSnodas = New FrmSnodas
            frmSnodas.ShowDialog()
        Catch ex As Exception
            Debug.Print("BtnSnodas.OnClick() Exception: " & ex.Message)
        End Try
  End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
