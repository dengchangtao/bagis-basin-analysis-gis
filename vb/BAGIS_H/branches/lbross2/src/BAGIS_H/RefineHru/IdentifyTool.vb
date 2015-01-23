Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Display
Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.esriSystem
Imports System.Windows.Forms

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
        Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of FrmReclassZones.AddinImpl)(My.ThisAddIn.IDs.FrmReclassZones)
        Dim frmReclass As FrmReclassZones = dockWindowAddIn.UI

        Dim item As LayerListItem = CType(frmReclass.CboParentHru.SelectedItem, LayerListItem)

        Dim layerName As String = item.Name
        If layerName.Trim.Length = 0 Then
            MsgBox("Please select an HRU first!")
            Exit Sub
        End If

        Dim pMxDoc As IMxDocument = My.ArcMap.Document
        Dim pMap As IMap = pMxDoc.FocusMap
        Dim pFLayer As IRasterLayer = Nothing
        Dim pIdentify As IIdentify
        Dim pPoint As IPoint
        Dim pIArray As IArray
        Dim prasobj As IRasterIdentifyObj

        Try
            Dim layerAvailable As Boolean = False
            If pMap.LayerCount > 0 AndAlso _
                BA_GetLayerIndexByName(My.ArcMap.Document, layerName) > -1 Then
                layerAvailable = True
            End If
            If layerAvailable Then
                pFLayer = pMap.Layer(BA_GetLayerIndexByName(My.ArcMap.Document, layerName))
                pIdentify = CType(pFLayer, IIdentify)
                If pFLayer IsNot Nothing Then
                    Dim pixelVal As String
                    pPoint = GetPointFromMouseClick(My.Document.ActivatedView)
                    pIArray = pIdentify.Identify(pPoint)
                    If pIArray IsNot Nothing Then
                        prasobj = pIArray.Element(0)
                        pixelVal = prasobj.Name
                        'Debug.Print("Pixel value ---> " & pixelVal)
                        frmReclass.TxtSelZone.Text = pixelVal
                    End If
                End If
            Else
                'There are no layers in the maps
                MessageBox.Show("The selected zone layer is not on the map to reclass. Please add the zone layer.", "Missing zone layer", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                My.ArcMap.Application.CurrentTool = Nothing
                frmReclass.BtnSelectZones.Enabled = False
                frmReclass.TxtSelZone.Text = Nothing
            End If
        Catch ex As Exception
            Debug.Print("OnMouseDown Exception() " & ex.Message)
        Finally
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
