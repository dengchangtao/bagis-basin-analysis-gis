Public Class BtnVecToRas
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()
        'Me.Enabled = False
  End Sub

    Protected Overrides Sub OnClick()
        Dim frmVecToRasNum As FrmVecToRasNum = New FrmVecToRasNum()
        frmVecToRasNum.ShowDialog()
    End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
