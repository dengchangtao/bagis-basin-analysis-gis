Public Class BtnAbout
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()

    End Sub

  Protected Overrides Sub OnClick()
        Dim frmAbout As frmAbout = New frmAbout
        frmAbout.ShowDialog()
  End Sub

  Protected Overrides Sub OnUpdate()
        
  End Sub
End Class
