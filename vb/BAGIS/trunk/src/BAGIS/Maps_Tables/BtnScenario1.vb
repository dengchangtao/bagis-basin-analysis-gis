Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Display

Public Class BtnScenario1
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()
        Me.Enabled = False
    End Sub

    Public WriteOnly Property SelectedProperty As Boolean
        Set(ByVal value As Boolean)
            Me.Enabled = value
        End Set
    End Property

    Protected Overrides Sub OnClick()

        Try
            'Check to see if the layer is already red
            Dim redColor As IRgbColor = New RgbColor
            redColor.RGB = RGB(255, 0, 0)
            Dim pMap As IMap = My.Document.FocusMap
            Dim pTempLayer As ILayer
            Dim changeColor As Boolean = True
            For i = 0 To pMap.LayerCount - 1
                pTempLayer = pMap.Layer(i)
                If BA_MAPS_SCENARIO1_REPRESENTATION = pTempLayer.Name Then 'move the layer
                    If TypeOf pTempLayer Is IFeatureLayer Then
                        Dim gfLayer As IGeoFeatureLayer = CType(pTempLayer, IGeoFeatureLayer)
                        Dim renderer As IFeatureRenderer = gfLayer.Renderer
                        If TypeOf renderer Is ISimpleRenderer Then
                            Dim sRenderer As ISimpleRenderer = CType(renderer, ISimpleRenderer)
                            Dim pSymbol As ISymbol = sRenderer.Symbol
                            If TypeOf pSymbol Is ISimpleFillSymbol Then
                                Dim fSymbol As ISimpleFillSymbol = CType(pSymbol, ISimpleFillSymbol)
                                Dim fColor As IColor = fSymbol.Color
                                If fColor.RGB = redColor.RGB Then
                                    changeColor = False
                                End If
                            End If
                        End If
                    End If
                    Exit For
                End If
            Next
            If changeColor = True Then
                'If not, re-add it as red
                Dim filepath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True)
                Dim FileName As String = BA_EnumDescription(MapsFileName.ActualRepresentedArea)
                Dim filepathname As String = filepath & FileName
                Dim success As BA_ReturnCode = BA_MapDisplayPolygon(My.Document, filepathname, BA_MAPS_SCENARIO1_REPRESENTATION, redColor)
                'Reorder scenario layers so things are visible
                BA_MoveScenarioLayers()
            End If
        Catch ex As Exception
            Windows.Forms.MessageBox.Show("An error occurred while trying to display the scenario 1 map.", "Error", Windows.Forms.MessageBoxButtons.OK)
            Debug.Print("OnClick" & ex.Message)
        End Try
        Dim Basin_Name As String
        Dim cboSelectedBasin = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of cboTargetedBasin)(My.ThisAddIn.IDs.cboTargetedBasin)
        Dim cboSelectedAoi = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of cboTargetedAOI)(My.ThisAddIn.IDs.cboTargetedAOI)
        Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of frmSiteScenario.AddinImpl)(My.ThisAddIn.IDs.frmSiteScenario)
        Dim frmSiteScenario As frmSiteScenario = dockWindowAddIn.UI
        If Len(Trim(cboSelectedBasin.getValue)) = 0 Then
            Basin_Name = ""
        Else
            Basin_Name = cboSelectedBasin.getValue
        End If
        BA_RemoveLayersfromLegend(My.Document)
        BAGIS_ClassLibrary.BA_DisplayMap(My.Document, 7, Basin_Name, cboSelectedAoi.getValue, Map_Display_Elevation_in_Meters, _
                                         Trim(frmSiteScenario.TxtScenario1.Text))
        BA_ZoomToAOI(My.Document, AOIFolderBase)
    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub
End Class
