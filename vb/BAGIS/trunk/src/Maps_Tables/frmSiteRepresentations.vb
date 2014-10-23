Imports ESRI.ArcGIS.esriSystem
Imports BAGIS_ClassLibrary
Imports System.Windows.Forms
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Geometry
Imports System.Text

''' <summary>
''' Designer class of the dockable window add-in. It contains user interfaces that
''' make up the dockable window.
''' </summary>
Public Class frmSiteRepresentations

    Protected Shared m_CurrentAOI As String = ""
    Private m_basinName As String
    Private m_displayInMeters As Boolean = True
    Private m_sitesOnMap As IList(Of String) = New List(Of String)
    'Identify dgv column indexes
    Private idxSelected As Integer = 0
    Private idxObjectId As Integer = 1
    Private idxSiteType As Integer = 2
    Private idxSiteName As Integer = 3
    Private idxRasterName As Integer = 4
    Private idxElevation As Integer = 5
    Private idxUpper As Integer = 6
    Private idxLower As Integer = 7

    Public Sub New(ByVal hook As Object)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Hook = hook
    End Sub


    Private m_hook As Object
    ''' <summary>
    ''' Host object of the dockable window
    ''' </summary> 
    Public Property Hook() As Object
        Get
            Return m_hook
        End Get
        Set(ByVal value As Object)
            m_hook = value
        End Set
    End Property

    ''' <summary>
    ''' Implementation class of the dockable window add-in. It is responsible for
    ''' creating and disposing the user interface class for the dockable window.
    ''' </summary>
    Public Class AddinImpl
        Inherits ESRI.ArcGIS.Desktop.AddIns.DockableWindow

        Private m_windowUI As frmSiteRepresentations

        Protected Overrides Function OnCreateChild() As System.IntPtr
            m_windowUI = New frmSiteRepresentations(Me.Hook)
            Return m_windowUI.Handle
        End Function

        Protected Overrides Sub Dispose(ByVal Param As Boolean)
            If m_windowUI IsNot Nothing Then
                m_windowUI.Dispose(Param)
            End If

            MyBase.Dispose(Param)
        End Sub

        Protected Friend ReadOnly Property UI() As frmSiteRepresentations
            Get
                Return m_windowUI
            End Get
        End Property

        Protected Friend ReadOnly Property Ready(ByVal matchString As String) As Boolean
            Get
                If matchString = m_CurrentAOI Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property
    End Class

    Public Sub LoadAOIInfo(ByVal aoiPath As String)
        ' Add any initialization after the InitializeComponent() call.
        If Len(AOIFolderBase) = 0 Then
            MsgBox("Please select an AOI first!")
            Exit Sub
        End If
        TxtRasterFolder.Text = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Analysis)

        Dim cboSelectedBasin = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of cboTargetedBasin)(My.ThisAddIn.IDs.cboTargetedBasin)
        Dim cboSelectedAoi = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of cboTargetedAOI)(My.ThisAddIn.IDs.cboTargetedAOI)
        If Len(Trim(cboSelectedBasin.getValue)) = 0 Then
            m_basinName = ""
        Else
            m_basinName = cboSelectedBasin.getValue
        End If

        If Len(Trim(cboSelectedAoi.getValue)) = 0 Then
            m_CurrentAOI = ""
        Else
            m_CurrentAOI = cboSelectedAoi.getValue
        End If

        'Get a handle to the parent form
        Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of frmSiteScenario.AddinImpl)(My.ThisAddIn.IDs.frmSiteScenario)
        Dim siteScenarioForm As frmSiteScenario = dockWindowAddIn.UI

        'set buffer unit based on the unit from the site scenario form
        Select Case siteScenarioForm.BufferUnits
            Case esriUnits.esriFeet
                LblDistanceUnit.Text = "Feet"
            Case esriUnits.esriMeters
                LblDistanceUnit.Text = "Meters"
            Case esriUnits.esriMiles
                LblDistanceUnit.Text = "Miles"
            Case esriUnits.esriKilometers
                LblDistanceUnit.Text = "Km"
        End Select
        'Set the values from the site scenario form
        TxtBufferDistance.Text = siteScenarioForm.TxtBufferDistance.Text
        LblBufferDistance.Enabled = siteScenarioForm.LblBufferDistance.Enabled
        LblDistanceUnit.Enabled = siteScenarioForm.CmboxDistanceUnit.Enabled
        TxtUpperRange.Text = siteScenarioForm.TxtUpperRange.Text
        LblUpperRange.Enabled = siteScenarioForm.LblUpperRange.Enabled
        TxtLowerRange.Text = siteScenarioForm.TxtLowerRange.Text
        LblLowerRange.Enabled = siteScenarioForm.LblLowerRange.Enabled
        If LblLowerRange.Enabled = False AndAlso LblUpperRange.Enabled = False Then
            LblZUnit.Enabled = False
        End If

        LblZUnit.Text = "Meters"  'set unit based on the Z Unit from the site scenario form
        If siteScenarioForm.ElevationUnits = esriUnits.esriFeet Then
            LblZUnit.Text = "Feet"
            m_displayInMeters = False
        End If

        'Remove any existing rows before loading grid
        GrdExistingSites.Rows.Clear()
        For Each pRow As DataGridViewRow In siteScenarioForm.GrdScenario1.Rows
            Dim item As New DataGridViewRow
            item.CreateCells(GrdExistingSites)
            With item
                .Cells(idxSelected).Value = pRow.Cells(siteScenarioForm.idxSelected).Value
                .Cells(idxObjectId).Value = pRow.Cells(siteScenarioForm.idxObjectId).Value
                .Cells(idxSiteType).Value = pRow.Cells(siteScenarioForm.idxSiteType).Value
                Dim siteType As SiteType = BA_GetSiteType(CStr(.Cells(idxSiteType).Value))
                .Cells(idxSiteName).Value = pRow.Cells(siteScenarioForm.idxSiteName).Value
                .Cells(idxElevation).Value = pRow.Cells(siteScenarioForm.idxElevation).Value
                .Cells(idxUpper).Value = pRow.Cells(siteScenarioForm.idxUpper).Value
                .Cells(idxLower).Value = pRow.Cells(siteScenarioForm.idxLower).Value
                Dim pSite As Site = New Site(CInt(.Cells(idxObjectId).Value), .Cells(idxSiteName).Value, siteType, _
                                             CDbl(.Cells(idxElevation).Value), CBool(.Cells(idxSelected).Value))
                Dim analysisFolder As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis)
                Dim rasterName As String = BA_GetSiteScenarioFileName(analysisFolder, pSite)
                .Cells(idxRasterName).Value = rasterName
            End With
            GrdExistingSites.Rows.Add(item)
        Next

        For Each pRow As DataGridViewRow In siteScenarioForm.GrdScenario2.Rows
            Dim objectId As Integer = Convert.ToInt32(pRow.Cells(siteScenarioForm.idxObjectId - 1).Value)
            Dim strType As String = Convert.ToString(pRow.Cells(siteScenarioForm.idxSiteType - 1).Value)
            SetSiteSelected(strType, objectId)
        Next

    End Sub

    Private Sub BtnClose_Click(sender As System.Object, e As System.EventArgs) Handles BtnClose.Click
        Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow
        Dim dockWinID As UID = New UIDClass()
        dockWinID.Value = My.ThisAddIn.IDs.frmSiteRepresentations
        dockWindow = My.ArcMap.DockableWindowManager.GetDockableWindow(dockWinID)
        dockWindow.Show(False)
    End Sub

    'Checks to see if the site is already in GrdScenarioSites
    Private Sub SetSiteSelected(ByVal sType As String, ByVal oid As Integer)
        For Each nextRow As DataGridViewRow In GrdExistingSites.Rows
            Dim siteType As String = Convert.ToString(nextRow.Cells(idxSiteType).Value)
            Dim siteId As Integer = Convert.ToInt32(nextRow.Cells(idxObjectId).Value)
            If siteType = sType AndAlso siteId = oid Then
                nextRow.Cells(idxSelected).Value = True
            End If
        Next
    End Sub

    Private Function DisplaySiteWithSymbol(ByVal pMxDoc As IMxDocument, ByVal LayerPathName As String, _
                                           ByVal DisplayName As String, _
                                           ByVal Buffer_Factor As Integer, ByVal sType As SiteType, _
                                           ByVal objectId As Integer) As BA_ReturnCode

        Dim pTempLayer As ILayer = Nothing
        Dim pEnv As IEnvelope = Nothing
        Dim pFLayer As IFeatureLayer = Nothing
        Dim pFSele As IFeatureSelection = Nothing
        Dim pFQueryFilter As IQueryFilter = New QueryFilter
        Dim pFCursor As IFeatureCursor = Nothing
        Dim pFeature As IFeature = Nothing
        Dim sitePoint As IPoint = Nothing
        Dim bufferFClass As IFeatureClass = Nothing
        Dim bufferCursor As IFeatureCursor = Nothing
        Dim bufferPoly As IFeature = Nothing

        Try
            'check if a layer with the assigned name exists
            'search layer of the specified name, if found
            Dim pMap As IMap = pMxDoc.FocusMap
            For i = pMap.LayerCount To 1 Step -1
                pTempLayer = CType(pMap.Layer(i - 1), ILayer)   'Explicit cast
                If DisplayName = pTempLayer.Name Then 'remove the layer
                    pMap.DeleteLayer(pTempLayer)
                End If
            Next

            'Clear any old selection sets
            For i = pMap.LayerCount To 1 Step -1
                pTempLayer = CType(pMap.Layer(i - 1), ILayer)   'Explicit cast
                If TypeOf pTempLayer Is IFeatureLayer Then
                    If pTempLayer.Name = BA_EnumDescription(MapsLayerName.pseudo_sites) Or
                        pTempLayer.Name = BA_EnumDescription(MapsLayerName.Snotel) Or
                        pTempLayer.Name = BA_EnumDescription(MapsLayerName.SnowCourse) Then
                        pFSele = TryCast(pTempLayer, IFeatureSelection)
                        pFSele.Clear()
                    End If
                End If
            Next

            Dim redColor As IRgbColor = New RgbColor
            redColor.RGB = RGB(255, 0, 0)
            Dim success As BA_ReturnCode = BA_MapDisplayPolygon(My.Document, LayerPathName, DisplayName, redColor)
            Dim idxNew As Integer
            ' We want to move the scenario layers directly on top of the streams layer
            For i = pMap.LayerCount To 1 Step -1
                pTempLayer = pMap.Layer(i - 1)
                If BA_MAPS_BOTH_REPRESENTATION = pTempLayer.Name Then
                    idxNew = i - 1
                    Exit For
                End If
            Next
            'Get handle to newest layer
            For i = 0 To pMap.LayerCount - 1
                pTempLayer = pMap.Layer(i)
                If DisplayName = pTempLayer.Name Then 'move the layer
                    pMap.MoveLayer(pTempLayer, idxNew)
                    Exit For
                End If
            Next

            If success = BA_ReturnCode.Success Then
                If ChkAutoZoom.Checked = True Then 'zoom to layer
                    'create a buffer around the AOI
                    'pEnv = pRLayer.AreaOfInterest

                    Dim sitesLayerName As String = ""
                    Select Case sType
                        Case SiteType.Pseudo
                            sitesLayerName = BA_EnumDescription(MapsLayerName.pseudo_sites)
                        Case SiteType.Snotel
                            sitesLayerName = BA_EnumDescription(MapsLayerName.Snotel)
                        Case SiteType.SnowCourse
                            sitesLayerName = BA_EnumDescription(MapsLayerName.SnowCourse)
                    End Select

                    For i = pMap.LayerCount To 1 Step -1
                        pTempLayer = CType(pMap.Layer(i - 1), ILayer)   'Explicit cast
                        If TypeOf pTempLayer Is FeatureLayer AndAlso pTempLayer.Name = sitesLayerName Then
                            pFLayer = CType(pTempLayer, IFeatureLayer)
                        End If
                    Next
                    If pFLayer IsNot Nothing Then
                        pFQueryFilter.WhereClause = BA_FIELD_OBJECT_ID & " = " & objectId
                        pFCursor = pFLayer.Search(pFQueryFilter, Nothing)
                        pFeature = pFCursor.NextFeature
                        If pFeature IsNot Nothing Then
                            bufferFClass = BA_OpenFeatureClassFromFile(AOIFolderBase, BA_StandardizeShapefileName(BA_BufferDistanceFile, False))
                            If bufferFClass IsNot Nothing Then
                                bufferCursor = bufferFClass.Search(Nothing, False)
                                bufferPoly = bufferCursor.NextFeature
                                If bufferPoly IsNot Nothing Then
                                    pEnv = bufferPoly.Shape.Envelope
                                    sitePoint = CType(pFeature.Shape, IPoint)
                                    pEnv.Expand(1.5, 1.5, True)
                                    pEnv.CenterAt(sitePoint)
                                End If
                            End If
                        End If
                        pFSele = TryCast(pFLayer, IFeatureSelection)
                        pFSele.SelectFeatures(pFQueryFilter, esriSelectionResultEnum.esriSelectionResultNew, False)
                    End If

                    Dim llx As Double, lly As Double, urx As Double, ury As Double
                    Dim xrange As Double, yrange As Double
                    Dim xoffset As Double, yoffset As Double

                    pEnv.QueryCoords(llx, lly, urx, ury)
                    xrange = urx - llx
                    yrange = ury - lly
                    xoffset = xrange * (Buffer_Factor - 1) / 2
                    yoffset = yrange * (Buffer_Factor - 1) / 2
                    llx = llx - xoffset
                    lly = lly - yoffset
                    urx = urx + xoffset
                    ury = ury + yoffset
                    pEnv.PutCoords(llx, lly, urx, ury)

                    Dim pActiveView As IActiveView = pMxDoc.FocusMap
                    pActiveView.Extent = pEnv
                Else
                    For i = 0 To pMap.LayerCount - 1
                        pTempLayer = pMap.Layer(i)
                        If BA_MAPS_AOI_BOUNDARY = pTempLayer.Name Then 'move the layer
                            pEnv = pTempLayer.AreaOfInterest
                            Exit For
                        End If
                    Next
                    Dim pActiveView As IActiveView = pMxDoc.FocusMap
                    pActiveView.Extent = pEnv
                End If
            End If

            pMxDoc.UpdateContents()
            'refresh the active view
            pMxDoc.ActivatedView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("DisplaySiteWithSymbol Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTempLayer)
            bufferCursor = Nothing
            bufferFClass = Nothing
            bufferPoly = Nothing
        End Try
    End Function

    Private Function DisplaySiteMap(ByVal pMxDocument As IMxDocument, ByVal selectedBasin As String, ByVal selectedAoi As String, _
                                    ByVal displayElevationInMeters As Boolean, ByVal siteLayerName As String) As BA_ReturnCode
        Dim LayerNames() As String
        Dim response As Integer
        Dim maptitle As String
        Dim KeyLayerName As String
        Dim UnitText As String
        Dim subtitle As String
        Dim Basin_Name As String

        If Len(Trim(selectedBasin)) = 0 Then
            Basin_Name = ""
        Else
            Basin_Name = vbCrLf & " at " & selectedBasin
        End If

        maptitle = selectedAoi & Basin_Name

        If ChkOnlyOne.Checked = False Then
            'Keep track of sites on map if we want to see > 1 at a time
            If Not m_sitesOnMap.Contains(siteLayerName) Then
                m_sitesOnMap.Add(siteLayerName)
            End If
        Else
            'If we only want one, clear the others out before adding the current
            m_sitesOnMap.Clear()
            m_sitesOnMap.Add(siteLayerName)
        End If

        Dim layerCount As Integer = 7 + m_sitesOnMap.Count

        ReDim LayerNames(0 To layerCount)
        LayerNames(1) = BA_MAPS_AOI_BOUNDARY
        LayerNames(2) = BA_MAPS_STREAMS
        LayerNames(3) = BA_MAPS_HILLSHADE
        LayerNames(4) = BA_MAPS_ELEVATION_ZONES
        LayerNames(5) = BA_MAPS_SNOTEL_SITES
        LayerNames(6) = BA_MAPS_SNOW_COURSE_SITES
        LayerNames(7) = BA_MAPS_PSEUDO_SITES
        'LayerNames(7) = siteLayerName
        Dim i As Integer = 8
        For Each siteName As String In m_sitesOnMap
            LayerNames(i) = siteName
            i += 1
        Next
        subtitle = "Site Representations"
        KeyLayerName = Nothing
        UnitText = " "

        BA_MapUpdateSubTitle(pMxDocument, maptitle, subtitle, UnitText)
        response = BA_ToggleLayersinMapFrame(pMxDocument, LayerNames)
        'BA_ZoomToAOI(AOIFolderBase) 'zoom to current AOI
        BA_ToggleView(pMxDocument, False) 'switch to the may layout view
        BA_SetLegendFormat(pMxDocument, KeyLayerName)
        Return 1
    End Function

    Private Sub GrdExistingSites_CellDoubleClick(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GrdExistingSites.CellDoubleClick
        Dim aRow As DataGridViewRow = GrdExistingSites.Rows(e.RowIndex)
        Dim selected As Boolean = Convert.ToBoolean(aRow.Cells(idxSelected).Value)
        If selected = True Then
            Dim fileName As String = Convert.ToString(aRow.Cells(idxRasterName).Value)
            Dim objectId As Integer = Convert.ToInt32(aRow.Cells(idxObjectId).Value)
            Dim strType As String = Convert.ToString(aRow.Cells(idxSiteType).Value)
            Dim success As BA_ReturnCode = DisplaySiteWithSymbol(My.Document, TxtRasterFolder.Text & "\" & fileName, fileName, 2, BA_GetSiteType(strType), _
                                                                 objectId)
            If success = BA_ReturnCode.Success Then
                BA_AddMapElements(My.Document, m_CurrentAOI & m_basinName, "Subtitle BAGIS")
                BA_RemoveLayersfromLegend(My.Document)
                success = DisplaySiteMap(My.Document, m_basinName, m_CurrentAOI, m_displayInMeters, fileName)
                'RemoveSiteLayerFromLegend(fileName)
            Else
                Dim sb As StringBuilder = New StringBuilder
                sb.Append("The map could not be displayed. The path to the map is " & TxtRasterFolder.Text & "\" & fileName & ". ")
                sb.Append("Please contact your System Administrator with this information.")
                MessageBox.Show(sb.ToString, "Error displaying site map", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

        Else
            MessageBox.Show("The site you selected was not included in an analysis scenario and has no representation map", "No representation", _
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
    End Sub

    Private Sub RemoveSiteLayerFromLegend(ByVal layerName As String)

        Dim pPageLayout As IPageLayout = My.Document.PageLayout
        Dim pGraphicsContainer As IGraphicsContainer = CType(pPageLayout, IGraphicsContainer)   'Explicit Cast
        Dim pMElem As IElement
        Dim pElemProp As IElementProperties2

        pGraphicsContainer.Reset()
        pMElem = pGraphicsContainer.Next
        Dim IsLegend As Boolean = False

        Do While Not pMElem Is Nothing
            pElemProp = pMElem
            If pElemProp.Name = "Legend" Then
                IsLegend = True
                Exit Do
            End If
            pMElem = pGraphicsContainer.Next
        Loop

        Dim pMapSurround As IMapSurround
        Dim pMapSurroundFrame As IMapSurroundFrame
        Dim pLegend As ILegend

        If IsLegend Then
            pMapSurroundFrame = pMElem
            pMapSurround = pMapSurroundFrame.MapSurround
            pLegend = pMapSurround

            For i = pLegend.ItemCount - 1 To 0 Step -1
                If pLegend.Item(i).Layer.Name = layerName Then
                    pLegend.RemoveItem(i)
                    Exit For
                End If
            Next

            pLegend.Refresh()

            pLegend = Nothing
            pMapSurroundFrame = Nothing
            pMElem = Nothing
            pMapSurround = Nothing

            My.Document.ActiveView.Refresh()
        End If

    End Sub
End Class