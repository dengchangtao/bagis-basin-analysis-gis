Public Class BtnCreateAOIStream
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()
        Me.Enabled = False
    End Sub

    Protected Overrides Sub OnClick()
        Dim frmAOIStream As frmAOIStream = New frmAOIStream
        frmAOIStream.ShowDialog()
    End Sub
    Public WriteOnly Property selectedProperty As Boolean
        Set(ByVal value As Boolean)
            Me.Enabled = value
        End Set
    End Property
  Protected Overrides Sub OnUpdate()

  End Sub
End Class
