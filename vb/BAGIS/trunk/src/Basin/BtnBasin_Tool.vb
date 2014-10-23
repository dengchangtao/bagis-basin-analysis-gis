Public Class BtnBasin_Tool
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button
    Public Sub New()

    End Sub

    Protected Overrides Sub OnClick()
        Dim frmbasin_tool As frmBasin_Tool = New frmBasin_Tool
        frmbasin_tool.ShowDialog()
    End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
