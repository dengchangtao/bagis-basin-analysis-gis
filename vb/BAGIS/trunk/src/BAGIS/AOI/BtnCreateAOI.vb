Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Carto

Public Class BtnCreateAOI
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()
        Me.Enabled = False
    End Sub
    Public WriteOnly Property selectedProperty As Boolean
        Set(ByVal value As Boolean)
            Me.Enabled = value
        End Set
    End Property

    Protected Overrides Sub OnClick()
        Dim pMxDoc As IMxDocument = My.ArcMap.Document
        Dim pMap As IMap = pMxDoc.FocusMap
        'Create graphics container and set to map document
        Dim pGC As IGraphicsContainer
        pGC = pMap

        'Create element and assign it to the graphic element from other subroutine
        Dim pElem As IElement
        pGC.Reset()
        pElem = pGC.Next

        pMxDoc = Nothing
        pMap = Nothing
        pGC = Nothing

        'Debug, if there are no graphics in the active view, then display message box
        If pElem Is Nothing Then
            MsgBox("No defined pourpoint location in the active view!")
            Exit Sub
        End If
        pElem = Nothing

        MsgBox("Please review and verify the Data Units on the next dialog!")

        Dim myForm As frmCreateAOI = New frmCreateAOI
        myForm.ShowDialog()
    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub
End Class
