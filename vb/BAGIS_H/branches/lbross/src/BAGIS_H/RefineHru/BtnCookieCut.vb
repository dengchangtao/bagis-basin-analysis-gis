Public Class BtnCookieCut
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()
        'Me.Enabled = False
  End Sub

    Protected Overrides Sub OnClick()

        Dim frmCookieCut As New FrmCookieCut(BAGIS_ClassLibrary.BA_MODE_COOKIE_CUT)
        frmCookieCut.LblToolTip.Text = "Use selected value(s) in the cookie-cut layer to cut the template HRU."
        'frmCookieCut.LblValues.Text = "Cookie-cut value(s)"
        frmCookieCut.ShowDialog()

    End Sub

  Protected Overrides Sub OnUpdate()

    End Sub

End Class
