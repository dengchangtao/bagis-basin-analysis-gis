Public Class BtnBasinInfo
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()
        Me.Enabled = False
    End Sub
    Public WriteOnly Property selectedProperty As Boolean
        Set(ByVal value As Boolean)
            Me.Enabled = value
        End Set
    End Property

  Protected Overrides Sub OnClick()
        Dim frmBasinInfo As frmBasinInfo = New frmBasinInfo
        frmBasinInfo.ShowDialog()
  End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
