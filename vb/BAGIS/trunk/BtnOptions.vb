Public Class BtnOptions
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()
    End Sub

  Protected Overrides Sub OnClick()
        Dim frm1 As frmSettings = New frmSettings
        frm1.ShowDialog()
  End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
