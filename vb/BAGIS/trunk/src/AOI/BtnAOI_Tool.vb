Public Class BtnAOI_Tool
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
        If Trim(BasinFolderBase) = "" Then
            MsgBox("Please select a basin first to use this tool.")
        Else
            Dim frmaoi_Tool As frmAOI_Tool = New frmAOI_Tool
            frmaoi_Tool.Show()
        End If
    End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
