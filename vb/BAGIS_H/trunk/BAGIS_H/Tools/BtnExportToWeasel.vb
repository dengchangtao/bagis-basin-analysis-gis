Imports BAGIS_ClassLibrary

Public Class BtnExportToWeasel
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()
        'Me.Enabled = False
    End Sub

    Protected Overrides Sub OnClick()
        Dim myForm As New FrmExportToWeasel()
        myForm.ShowDialog()
    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub
End Class
