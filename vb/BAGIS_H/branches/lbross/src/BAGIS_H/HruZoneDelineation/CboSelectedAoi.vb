Public Class CboSelectedAoi
  Inherits ESRI.ArcGIS.Desktop.AddIns.ComboBox

  Public Sub New()

  End Sub

  Protected Overrides Sub OnUpdate()

    End Sub

    Public Sub setValue(ByVal value As String)
        Me.Clear()
        Dim c1 As Integer = Me.Add(value)
        Me.Select(c1)
        Me.Value = value
    End Sub

End Class
