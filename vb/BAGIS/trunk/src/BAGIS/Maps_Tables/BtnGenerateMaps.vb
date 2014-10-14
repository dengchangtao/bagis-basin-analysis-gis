Public Class BtnGenerateMaps
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()
        Me.Enabled = False
    End Sub
    Protected Overrides Sub OnClick()
        Try
            Dim frmGeneratemaps As frmGenerateMaps = New frmGenerateMaps
            'loadfrmGenerateMaps()
            frmGeneratemaps.ShowDialog()
        Catch ex As Exception
            'Debug.Print("BtnGenerateMaps.OnClick Exception: " & ex.Message)
            Windows.Forms.MessageBox.Show("BtnGenerateMaps.OnClick Exception: " & ex.Message)
            Windows.Forms.MessageBox.Show("An error occurred while trying to open the Generate Maps menu.", "Unable to open menu", Windows.Forms.MessageBoxButtons.OK)
        End Try
    End Sub
    Public WriteOnly Property SelectedProperty As Boolean
        Set(ByVal value As Boolean)
            Me.Enabled = value
        End Set
    End Property
    Protected Overrides Sub OnUpdate()

    End Sub
End Class
