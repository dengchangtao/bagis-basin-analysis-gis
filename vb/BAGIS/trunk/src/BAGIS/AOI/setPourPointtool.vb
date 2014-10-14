Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.ArcMapUI
Imports System.Windows.Forms

Public Class setPourPointtool
    Inherits ESRI.ArcGIS.Desktop.AddIns.Tool

    Public Sub New()
        Me.Enabled = False
    End Sub
    Private Function SetPourPoint_CourseID() As VariantType
        SetPourPoint_CourseID = 3
    End Function
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
        '### List of Procedures ###
        ' Insert Graphics on to Map
        ' Refresh the activeview
        ' Assign Geometry of Element to Envelope

        ' Insert Graphics on to Map
        ' Create Objects for Inserting Graphics
        Dim Button As Long, Shift As Long, X As Long, Y As Long
        Dim pMxDoc As IMxDocument = My.ArcMap.Document
        Dim pPoint As IPoint
        Dim pGraCont As IGraphicsContainer = pMxDoc.ActiveView
        Dim pElem As IElement

        ' Set Objects for Inserting Graphics
        Dim pGraContSel As IGraphicsContainerSelect
        Dim pRubberPoint As IRubberBand = New RubberPoint

        'Set marker symbol
        Dim pStyleGallery As IStyleGallery = pMxDoc.StyleGallery
        Dim pEnumMarkers As IEnumStyleGalleryItem = pStyleGallery.Items("Marker Symbols", "ESRI.style", "default")
        pEnumMarkers.Reset()
        Dim pStyleItem As IStyleGalleryItem = pEnumMarkers.Next

        On Error GoTo ErrorHandlerUndefinedBasin
        Dim pMSymbol As IMarkerSymbol = Nothing
        Do Until pStyleItem Is Nothing
            If pStyleItem.Name = "Circle 8" Then
                pMSymbol = pStyleItem.Item
                pMSymbol.Size = 18
                pMSymbol.Color = pSelectColor
                Exit Do
            End If
            pStyleItem = pEnumMarkers.Next
        Loop

        Dim pMElem As IMarkerElement

        ' Algorithm
        If Button >= 0 Then ' Create a new point for left or right click
            pGraCont.DeleteAllElements()
            pPoint = pRubberPoint.TrackNew(pMxDoc.ActiveView.ScreenDisplay, Nothing)
            If Not pPoint Is Nothing Then
                pElem = New MarkerElement
                pMElem = pElem
                pMElem.Symbol = pMSymbol
                pElem.Geometry = pPoint
                pGraCont.AddElement(pElem, 0)
            End If
        End If

        ' Refresh the activeview
        pMxDoc.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
        Exit Sub

ErrorHandlerUndefinedBasin:
        MsgBox("Please set a basin folder first!")
    End Sub
End Class
