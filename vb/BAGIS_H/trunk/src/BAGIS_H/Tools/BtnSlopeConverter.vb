Public Class BtnSlopeConverter
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()

  End Sub

  Protected Overrides Sub OnClick()
        Dim slopeForm As FrmSlopeConverter = New FrmSlopeConverter
        slopeForm.ShowDialog()
  End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
