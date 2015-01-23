Public Class BtnEditSlope
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()
        'Me.Enabled = False
  End Sub

  Protected Overrides Sub OnClick()
        Dim slopeTemplates As List(Of BAGIS_ClassLibrary.TemplateRule) = BA_GetTemplates(BAGIS_ClassLibrary.HruRuleType.Slope)
        Dim slopeForm As FrmSlopeTemplate = New FrmSlopeTemplate(slopeTemplates, False)
        slopeForm.ShowDialog()
  End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
