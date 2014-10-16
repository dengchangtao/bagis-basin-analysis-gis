Public Class BtnParameterDescription
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()

  End Sub

  Protected Overrides Sub OnClick()
        Process.Start("http://maps.geog.pdx.edu/BAGIS/")
  End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
