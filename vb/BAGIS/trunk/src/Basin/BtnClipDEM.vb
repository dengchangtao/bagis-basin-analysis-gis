Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Carto

Public Class BtnClipDEM
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public WriteOnly Property selectedProperty As Boolean
        Set(ByVal status As Boolean)
            Me.Enabled = status
        End Set
    End Property

    Public Sub New()
        Me.Enabled = False
    End Sub

    Protected Overrides Sub OnClick()
        Dim pMxDoc As IMxDocument = My.ArcMap.Document
        Dim pMap As IMap = pMxDoc.FocusMap
        'Create graphics container and set to map document
        Dim pGC As IGraphicsContainer = pMap

        Try
            'Create element and assign it to the graphic element from other subroutine
            Dim pElem As IElement
            pGC.Reset()
            pElem = pGC.Next

            'Debug, if there are no graphics in the active view, then display message box
            If pElem Is Nothing Then
                MsgBox("No defined extent in the active view!")
                Exit Sub
            End If

            'remove the grid layers that are to be overwritten from the map
            'they are Basin DEM Extent, Filled DEM, Slope, Aspect, Flow Direction, and Flow Accumulation
            Dim response As Integer = BA_Remove_Basin_Layers() 'response reports the number of layers removed
            Dim frmClipDEM As frmClipDEMtoAOI = New frmClipDEMtoAOI

            frmClipDEM.ShowDialog()
            pMxDoc = Nothing
            pGC = Nothing
            pElem = Nothing

        Catch ex As Exception
            MsgBox("Error occurred in the ClipDEM subroutine! " & ex.Message)

        End Try
    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub
End Class
