Public Class BtnAbout
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Dim frmAbout As FrmAbout

    Public Sub New()
        'Me.Enabled = False
    End Sub

  Protected Overrides Sub OnClick()
        frmAbout = New FrmAbout
        frmAbout.ShowDialog()
  End Sub

  Protected Overrides Sub OnUpdate()

    End Sub

End Class
