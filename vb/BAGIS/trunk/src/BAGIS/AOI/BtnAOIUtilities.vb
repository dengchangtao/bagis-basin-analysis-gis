Public Class BtnAOIUtilities
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()

  End Sub

  Protected Overrides Sub OnClick()
        Dim frmAOIInfo As frmAOIInfo = New frmAOIInfo
        frmAOIInfo.ShowDialog()
    End Sub

    Public WriteOnly Property selectedProperty As Boolean
        Set(ByVal value As Boolean)
            Me.Enabled = value
        End Set
    End Property

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
