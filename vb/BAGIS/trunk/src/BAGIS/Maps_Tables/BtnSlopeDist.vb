Public Class BtnSlopeDist
  Inherits ESRI.ArcGIS.Desktop.AddIns.Button

  Public Sub New()
        Me.Enabled = False
  End Sub

  Protected Overrides Sub OnClick()
        Dim Basin_Name As String
        Dim cboSelectedBasin = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of cboTargetedBasin)(My.ThisAddIn.IDs.cboTargetedBasin)
        Dim cboSelectedAoi = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of cboTargetedAOI)(My.ThisAddIn.IDs.cboTargetedAOI)
        If Len(Trim(cboSelectedBasin.getValue)) = 0 Then
            Basin_Name = ""
        Else
            Basin_Name = cboSelectedBasin.getValue
        End If
        BAGIS_ClassLibrary.BA_DisplayMap(My.Document, 6, Basin_Name, cboSelectedAoi.getValue, Map_Display_Elevation_in_Meters, _
                                         "SLOPE DISTRIBUTION")
    End Sub

    Public WriteOnly Property SelectedProperty As Boolean
        Set(ByVal value As Boolean)
            Me.Enabled = value
        End Set
    End Property

  Protected Overrides Sub OnUpdate()

  End Sub
End Class
