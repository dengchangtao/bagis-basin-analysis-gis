Public Class BtnFileLocator
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()

  End Sub

  Protected Overrides Sub OnClick()
        Dim frmfileLocator1 As frmFileLocator = New frmFileLocator
        frmfileLocator1.ShowDialog()
  End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
