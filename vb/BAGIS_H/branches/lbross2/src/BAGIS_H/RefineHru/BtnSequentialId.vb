Public Class BtnSequentialId
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()

  End Sub

  Protected Overrides Sub OnClick()
        Dim frmSequentialId As FrmSequentialId = New FrmSequentialId
        frmSequentialId.ShowDialog()
  End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
