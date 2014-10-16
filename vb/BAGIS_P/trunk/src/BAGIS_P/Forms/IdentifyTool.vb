Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.esriSystem
Imports BAGIS_ClassLibrary
Imports System.Windows.Forms
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.Geodatabase

Public Class IdentifyTool
    Inherits ESRI.ArcGIS.Desktop.AddIns.Tool

    Public Sub New()
        Me.Cursor = Windows.Forms.Cursors.Cross
    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub

    Protected Overrides Sub OnMouseDown(ByVal arg As ESRI.ArcGIS.Desktop.AddIns.Tool.MouseEventArgs)
        MyBase.OnMouseDown(arg)

        ' Get handle to UI (form) to update area
        Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of FrmTimberlineTool.AddinImpl)(My.ThisAddIn.IDs.FrmTimberlineTool)
        Dim frmTimberline As FrmTimberlineTool = dockWindowAddIn.UI

        Dim layerName As String = CStr(frmTimberline.CboParentHru.SelectedItem)

        If layerName.Trim.Length = 0 Then
            MsgBox("Please select an HRU first!")
            Exit Sub
        End If

        Dim pMxDoc As IMxDocument = My.ArcMap.Document
        Dim pMap As IMap = pMxDoc.FocusMap
        Dim pRLayer As IRasterLayer = Nothing
        Dim pFLayer As IFeatureLayer = Nothing
        Dim pIdentify As IIdentify
        Dim pPoint As IPoint
        Dim pIArray As IArray
        Dim prasobj As IRasterIdentifyObj
        Dim prowobj As IRowIdentifyObject
        Try
            Dim hruLayerAvailable As Boolean = False
            Dim demLayerAvailable As Boolean = False
            If pMap.LayerCount > 0 Then
                If BA_GetLayerIndexByName(My.ArcMap.Document, layerName) > -1 Then hruLayerAvailable = True
                If BA_GetLayerIndexByName(My.ArcMap.Document, BA_EnumDescription(MapsFileName.filled_dem_gdb)) > -1 Then demLayerAvailable = True
            End If
            If hruLayerAvailable And demLayerAvailable Then
                'Identify the point that was clicked
                pPoint = GetPointFromMouseClick(My.Document.ActivatedView)
                'Get the elevation value from the filled DEM
                pRLayer = pMap.Layer(BA_GetLayerIndexByName(My.ArcMap.Document, BA_EnumDescription(MapsFileName.filled_dem_gdb)))
                Dim prevElev = frmTimberline.TxtElev.Text
                If pRLayer IsNot Nothing Then
                    pIdentify = CType(pRLayer, IIdentify)
                    Dim pixelVal As String
                    pIArray = pIdentify.Identify(pPoint)
                    If pIArray IsNot Nothing Then
                        prasobj = pIArray.Element(0)
                        pixelVal = prasobj.Name
                        'Format value for display
                        If IsNumeric(pixelVal) Then
                            Dim fVal As Double = CDbl(pixelVal)
                            'Check the measurement units
                            If frmTimberline.ElevUnit <> frmTimberline.SelElevUnit Then
                                If frmTimberline.ElevUnit = MeasurementUnit.Meters And frmTimberline.SelElevUnit = MeasurementUnit.Feet Then
                                    'Convert from feet to meters
                                    fVal = fVal * BA_METERS_TO_FEET
                                ElseIf frmTimberline.ElevUnit = MeasurementUnit.Feet And frmTimberline.SelElevUnit = MeasurementUnit.Meters Then
                                    'Convert from Meters to feet
                                    fVal = fVal * BA_FEET_TO_METERS
                                End If
                            End If
                            pixelVal = Math.Round(fVal)
                        End If
                        frmTimberline.TxtElev.Text = pixelVal
                    End If
                End If
                'Get the zone layer from the zones vector file
                pFLayer = pMap.Layer(BA_GetLayerIndexByName(My.ArcMap.Document, layerName))
                If pFLayer IsNot Nothing Then
                    pIdentify = CType(pFLayer, IIdentify)
                    pIArray = pIdentify.Identify(pPoint)
                    If pIArray IsNot Nothing Then
                        prowobj = pIArray.Element(0)
                        Dim pRow As IRow = prowobj.Row
                        Dim idxHruId As Integer = pRow.Fields.FindField(BA_FIELD_HRU_ID)
                        If idxHruId > -1 Then
                            Dim pValue As Integer = Convert.ToString(pRow.Value(idxHruId))
                            frmTimberline.TxtHruId.Text = pValue
                        End If
                    End If
                End If
                'Get a handle to the grid on the Timberline form
                Dim pGrid As DataGridView = frmTimberline.DataGridView1
                'Search for the row with the hru id that the user clicked
                Dim idxUpdate As Integer = 0
                For Each pRow As DataGridViewRow In pGrid.Rows
                    Dim hruId As Integer = pRow.Cells(0).Value
                    If hruId = frmTimberline.TxtHruId.Text Then
                        'If we find it, set the elevation value to the elevation from the DEM
                        If IsNumeric(frmTimberline.TxtElev.Text) Then
                            pRow.Cells(1).Value = CDbl(frmTimberline.TxtElev.Text)
                        Else
                            pRow.Cells(1).Value = 0
                        End If
                        Exit For
                    End If
                    idxUpdate += 1
                Next
                'Manage the dirty flag
                If prevElev <> frmTimberline.TxtSelElev.Text Then
                    frmTimberline.DirtyFlag = True
                End If
                'Clear the previous selection
                pGrid.ClearSelection()
                'Select the row we just changed
                'http://social.msdn.microsoft.com/Forums/en-US/winformsdatacontrols/thread/47e9c3ef-a8de-48c9-8e0d-4f3fdd34517e/
                pGrid.FirstDisplayedScrollingRowIndex = pGrid.Rows(idxUpdate).Index
                pGrid.Refresh()
                pGrid.CurrentCell = pGrid.Rows(idxUpdate).Cells(1)
                pGrid.Rows(idxUpdate).Selected = True
            Else
                If hruLayerAvailable = False Then
                    'There are no layers in the maps
                    MessageBox.Show("The selected HRU zone layer is not on the map. Please use the 'View Layers' button to add it.", "Missing zone layer", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
                If demLayerAvailable = False Then
                    'There are no layers in the maps
                    MessageBox.Show("The filled DEM layer is not on the map. Please use the 'View Layers' button to add it.", "Missing DEM layer", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
                My.ArcMap.Application.CurrentTool = Nothing
                frmTimberline.BtnIdentify.Enabled = False
                frmTimberline.TxtHruId.Text = Nothing
                frmTimberline.TxtElev.Text = Nothing
            End If


        Catch ex As Exception
            Debug.Print("OnMouseDown Exception() " & ex.Message)
        Finally
            pRLayer = Nothing
            pFLayer = Nothing
            pPoint = Nothing
            pMap = Nothing
            pIdentify = Nothing
            pPoint = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try

    End Sub

    Private Function GetPointFromMouseClick(ByVal activeView As IActiveView) As IPoint

        'get the screenDisplay from the activeView which comes from My.Document.ActivatedView
        Dim screenDisplay As IScreenDisplay = activeView.ScreenDisplay

        'use the RubberBand object to track the movement of mouse cursor
        Dim rubberBand As IRubberBand2 = New RubberPoint

        'RubberBand.TrackNew() returns an IGeometry object
        Dim pGeometry As IGeometry5 = rubberBand.TrackNew(screenDisplay, Nothing)

        'Cast IGeometry to point
        Dim pPoint As IPoint = CType(pGeometry, IPoint)

        Return pPoint
    End Function
End Class
