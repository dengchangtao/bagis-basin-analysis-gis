Public Class BtnUseTemplate
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()
        'Me.Enabled = False
  End Sub

  Protected Overrides Sub OnClick()
        Dim myForm As New FrmUseTemplate()
        myForm.ShowDialog()
  End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
