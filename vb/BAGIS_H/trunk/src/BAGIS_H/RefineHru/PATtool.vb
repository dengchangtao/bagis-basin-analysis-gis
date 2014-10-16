Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.ArcMapUI
Imports System.Windows.Forms
Imports BAGIS_ClassLibrary

Public Class PATtool
    Inherits ESRI.ArcGIS.Desktop.AddIns.Tool

    Public Sub New()
        Me.Cursor = Windows.Forms.Cursors.Cross
    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub

    Protected Overrides Sub OnMouseDown(ByVal arg As ESRI.ArcGIS.Desktop.AddIns.Tool.MouseEventArgs)

        MyBase.OnMouseDown(arg)

        ' Get handle to UI (form) to update area
        Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of FrmEliminatePoly.AddinImpl)(My.ThisAddIn.IDs.FrmEliminatePoly)
        Dim elimPoly As FrmEliminatePoly = dockWindowAddIn.UI

        Dim item As LayerListItem = CType(elimPoly.LstSelectHruLayers.SelectedItem, LayerListItem)

        Dim layerPath As String = ""
        BA_GetBareName(item.Value, layerPath) 'get the HRU path
        If layerPath.Trim.Length = 0 Then
            MsgBox("Please select an HRU first!")
            Exit Sub
        End If
        'remove the padding backslash from path
        layerPath = BA_StandardizePathString(layerPath)

        Dim layerName As String = BA_EnumDescription(PublicPath.HruVector)
        'remove shp extension from the layerName
        layerName = BA_StandardizeShapefileName(layerName, False)

        'determine attribute field name based on the radio button selection
        Dim fieldName As String = BA_FIELD_AREA_SQKM 'i.e., "AREA-SqKm" 'read area from the SQKM field

        Dim pMxDoc As IMxDocument = My.ArcMap.Document
        Dim pMap As IMap = pMxDoc.FocusMap
        Dim pFLayer As IFeatureLayer = pMap.Layer(BA_GetLayerIndexByName(My.ArcMap.Document, layerName))

        Dim pPoint As IPoint
        Dim polygonArea As Double

        Try
            If pFLayer IsNot Nothing Then
                pPoint = GetPointFromMouseClick(My.Document.ActivatedView)
                polygonArea = BA_GetLayerAttributebyPoint(pFLayer, pPoint, fieldName)

                'Change_TxtPolyArea() automatically converts polygonArea from Sq Km 
                'to corresponding unit selected by user
                elimPoly.Change_TxtPolyArea(polygonArea)
                elimPoly.TxtNoZonesRemoved.Text = BA_CountFeaturesSmallerThanOrEqualTo(layerName, layerPath, BA_FIELD_AREA_SQKM, polygonArea)
            End If

        Catch ex As Exception
            MsgBox("OnMouseDown Exception: " & ex.Message)

        Finally
            pFLayer = Nothing
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