Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.esriSystem
Imports System.Windows.Forms
Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.Desktop.AddIns

Public Class setDEMExtenttool
    Inherits ESRI.ArcGIS.Desktop.AddIns.Tool

    Public Sub New()
        Me.Enabled = False
    End Sub
    Public WriteOnly Property selectedProperty As Boolean
        Set(ByVal value As Boolean)
            Me.Enabled = value
        End Set
    End Property

    Protected Overrides Sub OnUpdate()
        Me.Cursor = Cursors.Cross
    End Sub
    Protected Overrides Sub OnMouseDown(ByVal arg As ESRI.ArcGIS.Desktop.AddIns.Tool.MouseEventArgs)
        MyBase.OnMouseDown(arg)
        'Create a new rubberband and envelope
        Dim pMxDoc As IMxDocument = My.ArcMap.Document

        'delete all existing graphics if any
        Dim pGContain As IGraphicsContainer = pMxDoc.ActivatedView
        pGContain.DeleteAllElements()

        Dim pRubberEnv As IRubberBand2 = New RubberEnvelope
        Dim pEnv As IEnvelope = pRubberEnv.TrackNew(pMxDoc.ActivatedView.ScreenDisplay, Nothing)
        Dim pElem As IElement = New RectangleElement
        pElem.Geometry = pEnv

        'exit the subroutine if the user did nothing
        If pElem.Geometry.IsEmpty Then Exit Sub
        On Error GoTo ErrorHandlerUndefinedBasin

        'Create graphic elements to be displayed as the rectangle
        Dim pFillShapeElem As IFillShapeElement = pElem

        'set symbology
        Dim pPolySym As ISimpleFillSymbol = New SimpleFillSymbol
        'pPolySym.Outline.Color = pSelectColor

        Dim pLineSym As ILineSymbol = New SimpleLineSymbol
        pLineSym.Color = pSelectColor
        pPolySym.Outline = pLineSym
        pPolySym.Style = esriSimpleFillStyle.esriSFSHollow
        pFillShapeElem.Symbol = pPolySym

        'snap the elem to raster cell boundary
        Dim strDEMDataSet As String

        ' If frmSettings.Opt10M.Value Then
        If BA_SystemSettings.DEM10MPreferred Then
            strDEMDataSet = BA_SystemSettings.DEM10M
        Else
            strDEMDataSet = BA_SystemSettings.DEM30M
        End If

        BA_GetRasterDimension(strDEMDataSet, BA_DEMDimension)
        If BA_DEMDimension.X_CellSize * BA_DEMDimension.Y_CellSize = 0 Then _
            MsgBox("The extent boundaries will be snapped to the DEM raster cells.")
        BA_SnapGCElementtoRaster(pElem, BA_DEMDimension, strDEMDataSet)

        'Create a graphics container to manage the graphic elements
        pGContain.AddElement(pElem, 0)
        pMxDoc.ActivatedView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
        Dim clipDEMButton = AddIn.FromID(Of BtnClipDEM)(My.ThisAddIn.IDs.BtnClipDEM)
        clipDEMButton.selectedProperty = True
        Exit Sub
ErrorHandlerUndefinedBasin:
        MsgBox("Error occurred when trying to set DEM extent! " & Err.Description)
        Err.Clear()
    End Sub
    Private Function GetPointFromMouseClick(ByVal activeView As IActiveView) As IPoint

        'get the screenDisplay from the activeView which comes from My.Document.ActivatedView
        Dim screenDisplay As IScreenDisplay = activeView.ScreenDisplay

        'use the RubberBand object to track the movement of mouse cursor
        Dim rubberBand As IRubberBand2 = New RubberEnvelope

        'RubberBand.TrackNew() returns an IGeometry object
        Dim pGeometry As IGeometry5 = rubberBand.TrackNew(screenDisplay, Nothing)

        'Cast IGeometry to polygon
        Dim pPoly As IPolygon5 = CType(pGeometry, IPolygon5)

        Return pPoly
    End Function
End Class
