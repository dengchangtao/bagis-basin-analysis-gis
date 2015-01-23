Public Class BtnPrmsRadiation
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()
        'Me.Enabled = False
  End Sub

  Protected Overrides Sub OnClick()
        Dim frmprms As FrmPRMS = New FrmPRMS
        frmprms.ShowDialog()
  End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
