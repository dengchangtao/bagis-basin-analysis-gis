Public Class BtnSubAOIIdTool
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()

  End Sub

  Protected Overrides Sub OnClick()
        Dim frmSubAoi As FrmSubAoiId = New FrmSubAoiId
        frmSubAoi.Show()
  End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
