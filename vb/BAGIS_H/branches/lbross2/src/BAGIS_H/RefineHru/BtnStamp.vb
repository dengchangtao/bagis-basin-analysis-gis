Public Class BtnStamp
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()

  End Sub

  Protected Overrides Sub OnClick()

        Dim frmCookieCut As New FrmCookieCut(BAGIS_ClassLibrary.BA_MODE_STAMP)
        frmCookieCut.LblToolTip.Text = "Use selected value(s) in the stamp layer to cover the template HRU."
        'frmCookieCut.LblValues.Text = "Stamp value(s)"
        frmCookieCut.ShowDialog()

  End Sub

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
