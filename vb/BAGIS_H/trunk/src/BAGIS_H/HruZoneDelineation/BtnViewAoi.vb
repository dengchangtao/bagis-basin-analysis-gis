Public Class BtnViewAoi
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()

  End Sub

  Protected Overrides Sub OnClick()

        Dim frmAoiView As FrmAoiViewer = New FrmAoiViewer
        frmAoiView.ShowDialog()

    End Sub


  Protected Overrides Sub OnUpdate()

  End Sub
End Class