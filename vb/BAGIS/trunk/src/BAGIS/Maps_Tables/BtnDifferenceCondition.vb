Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.Display

Public Class BtnDifferenceCondition
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()
        Me.Enabled = False
    End Sub

    Protected Overrides Sub OnClick()
        Try
            'Need to re-display the scenario vectors so we can change the color
            Dim filepath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True)
            Dim FileName As String = BA_EnumDescription(MapsFileName.ActualRepresentedArea)
            Dim filepathname As String = filepath & FileName
            Dim pColor As IColor = New RgbColor
            pColor.RGB = RGB(232, 157, 116)
            Dim success As BA_ReturnCode = BA_MapDisplayPolygon(My.Document, filepathname, BA_MAPS_SCENARIO1_REPRESENTATION, pColor)

            FileName = BA_EnumDescription(MapsFileName.PseudoRepresentedArea)
            filepathname = filepath & FileName
            pColor.RGB = RGB(159, 167, 201)
            success = BA_MapDisplayPolygon(My.Document, filepathname, BA_MAPS_SCENARIO2_REPRESENTATION, pColor)
            'Reorder scenario layers so things are visible
            BA_MoveScenarioLayers()
        Catch ex As Exception
            Windows.Forms.MessageBox.Show("An error occurred while trying to display the difference map.", "Error", Windows.Forms.MessageBoxButtons.OK)
            Debug.Print("OnClick" & ex.Message)
        End Try


        Dim Basin_Name As String
        Dim cboSelectedBasin = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of cboTargetedBasin)(My.ThisAddIn.IDs.cboTargetedBasin)
        Dim cboSelectedAoi = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of cboTargetedAOI)(My.ThisAddIn.IDs.cboTargetedAOI)
        If Len(Trim(cboSelectedBasin.getValue)) = 0 Then
            Basin_Name = ""
        Else
            Basin_Name = cboSelectedBasin.getValue
        End If
        BA_RemoveLayersfromLegend(My.Document)
        BAGIS_ClassLibrary.BA_DisplayMap(My.Document, 9, Basin_Name, cboSelectedAoi.getValue, Map_Display_Elevation_in_Meters, _
                                         "Difference of Representations")
        BA_ZoomToAOI(My.Document, AOIFolderBase)
    End Sub

    Public WriteOnly Property SelectedProperty As Boolean
        Set(ByVal value As Boolean)
            Me.Enabled = value
        End Set
    End Property

    Protected Overrides Sub OnUpdate()

    End Sub
End Class
