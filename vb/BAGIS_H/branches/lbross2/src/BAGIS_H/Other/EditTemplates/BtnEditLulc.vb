Public Class BtnEditLulc
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()

  End Sub

    Protected Overrides Sub OnClick()
        Dim landUseTemplates As List(Of BAGIS_ClassLibrary.TemplateRule) = BA_GetTemplates(BAGIS_ClassLibrary.HruRuleType.LandUse)
        Dim landUseForm As FrmLandUseTemplate = New FrmLandUseTemplate(landUseTemplates, False)
        landUseForm.ShowDialog()
    End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
