Public Class BtnEditCanopy
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()
        'Me.Enabled = False
  End Sub

    Protected Overrides Sub OnClick()

        Dim canopyTemplates As List(Of BAGIS_ClassLibrary.TemplateRule) = BA_GetTemplates(BAGIS_ClassLibrary.HruRuleType.Canopy)
            Dim canopyForm As FrmCanopyTemplate = New FrmCanopyTemplate(canopyTemplates, False)
            canopyForm.ShowDialog()
    End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
