Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Display
Imports System.Windows.Forms
Imports System.Text
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.esriSystem

Public Class SelectSiteTool
    Inherits ESRI.ArcGIS.Desktop.AddIns.Tool

    Public Sub New()
        Me.Cursor = Windows.Forms.Cursors.Cross
    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub

    Private m_siteName As String

    Friend WriteOnly Property SiteName As String
        Set(value As String)
            m_siteName = value
        End Set
    End Property

    Protected Overrides Sub OnMouseDown(ByVal arg As ESRI.ArcGIS.Desktop.AddIns.Tool.MouseEventArgs)
        MyBase.OnMouseDown(arg)
        '### List of Procedures ###
        ' Insert Graphics on to Map
        ' Refresh the activeview
        ' Assign Geometry of Element to Envelope

        ' Insert Graphics on to Map
        ' Create Objects for Inserting Graphics
        Dim Button As Long
        Dim pMxDoc As IMxDocument = My.ArcMap.Document
        Dim pPoint As IPoint = Nothing
        Dim pGraCont As IGraphicsContainer = pMxDoc.ActiveView
        Dim pElem As IElement = Nothing
        Dim demDataSet As IRasterDataset = Nothing
        Dim demLayer As IRasterLayer = Nothing
        Dim pIdentify As IIdentify
        Dim pIArray As ESRI.ArcGIS.esriSystem.IArray
        Dim prasobj As IRasterIdentifyObj
        'Get handle to select elements tool so we can use it in exception handler
        Dim document As ESRI.ArcGIS.Framework.IDocument = CType(My.Document, ESRI.ArcGIS.Framework.IDocument)
        Dim commandBars As ESRI.ArcGIS.Framework.ICommandBars = document.CommandBars
        Dim uid As UID = New UIDClass()
        ' Example: "esriFramework.HelpContentsCommand" or "{D74B2F25-AC90-11D2-87F8-0000F8751720}"
        uid.Value = "esriArcMapUI.SelectTool"
        Dim commandItem As ESRI.ArcGIS.Framework.ICommandItem = commandBars.Find(uid, False, False)

        ' Set Objects for Inserting Graphics
        Dim pRubberPoint As IRubberBand = New RubberPoint

        'Set marker symbol
        Dim pStyleGallery As IStyleGallery = pMxDoc.StyleGallery
        Dim pEnumMarkers As IEnumStyleGalleryItem = pStyleGallery.Items("Marker Symbols", "ESRI.style", "default")
        pEnumMarkers.Reset()
        Dim pStyleItem As IStyleGalleryItem = pEnumMarkers.Next

        Try
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

            ' Get filled DEM layer to get elevation of point
            Dim pixelVal As String = Nothing
            Dim filepath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces)
            Dim FileName As String = BA_GetBareName(BA_EnumDescription(MapsFileName.filled_dem_gdb))
            Dim idxLayer As Integer = BA_GetLayerIndexByFilePath(My.ArcMap.Document, filepath, FileName)
            If idxLayer > -1 Then
                'Use the existing filled dem if it is on the map
                demLayer = CType(My.Document.FocusMap.Layer(idxLayer), IRasterLayer)
            Else
                'Otherwise create a layer from the raster dataset
                demDataSet = CType(BA_OpenRasterFromGDB(filepath, FileName), IRasterDataset)
                demLayer = New RasterLayer
                demLayer.CreateFromDataset(demDataSet)
            End If

            pIdentify = CType(demLayer, IIdentify)
            pIArray = pIdentify.Identify(pPoint)
            If pIArray IsNot Nothing Then
                prasobj = pIArray.Element(0)
                pixelVal = prasobj.Name
            End If
            'Clean up the objects
            demLayer = Nothing
            demDataSet = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()

            Dim siteElevation As Double
            If IsNumeric(pixelVal) Then siteElevation = CDbl(pixelVal)
            'Get a handle to the parent form
            Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of frmSiteScenario.AddinImpl)(My.ThisAddIn.IDs.frmSiteScenario)
            Dim siteScenarioForm As frmSiteScenario = dockWindowAddIn.UI
            Dim formUnits As ESRI.ArcGIS.esriSystem.esriUnits
            Dim formElevation As Double
            Dim displayUnits As ESRI.ArcGIS.esriSystem.esriUnits = ESRI.ArcGIS.esriSystem.esriUnits.esriMeters

            Dim converter As IUnitConverter = New UnitConverter
            If siteScenarioForm.DemInMeters = True Then
                If siteScenarioForm.ElevationUnits = ESRI.ArcGIS.esriSystem.esriUnits.esriMeters Then
                    formUnits = ESRI.ArcGIS.esriSystem.esriUnits.esriMeters
                    formElevation = siteElevation
                ElseIf siteScenarioForm.ElevationUnits = ESRI.ArcGIS.esriSystem.esriUnits.esriFeet Then
                    formUnits = ESRI.ArcGIS.esriSystem.esriUnits.esriFeet
                    formElevation = converter.ConvertUnits(siteElevation, esriUnits.esriMeters, esriUnits.esriFeet)
                End If
            Else
                If siteScenarioForm.ElevationUnits = ESRI.ArcGIS.esriSystem.esriUnits.esriFeet Then
                    formUnits = ESRI.ArcGIS.esriSystem.esriUnits.esriFeet
                    formElevation = siteElevation
                ElseIf siteScenarioForm.ElevationUnits = ESRI.ArcGIS.esriSystem.esriUnits.esriMeters Then
                    formUnits = ESRI.ArcGIS.esriSystem.esriUnits.esriMeters
                    formElevation = converter.ConvertUnits(siteElevation, esriUnits.esriFeet, esriUnits.esriMeters)
                End If
            End If
            Dim newSiteForm As FrmNewSite = New FrmNewSite(formElevation, formUnits, AOIFolderBase)
            newSiteForm.ShowDialog()
            'Stop processing if user hit cancel or didn't enter a site name
            If String.IsNullOrEmpty(m_siteName) Then
                MessageBox.Show("This pseudo site will not be saved.", "New pseudo site", _
                                MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            'Saves the point to the psuedo-sites shapefile
            Dim layersFolder As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers)
            Dim siteOID As Integer = -1
            Dim retVal As Integer
            If Not BA_File_Exists(layersFolder & "\" & BA_EnumDescription(MapsFileName.Pseudo), WorkspaceType.Geodatabase, _
                                  ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureClass) Then
                retVal = BA_Graphic2FeatureClass(layersFolder, BA_EnumDescription(MapsFileName.Pseudo))
                'If we just created the feature class, we will assume the new record has an OID of 1
                siteOID = 1
            Else
                siteOID = BA_AddGraphic2Shapefile(layersFolder, BA_EnumDescription(MapsFileName.Pseudo))
            End If
            'If not, use -1 to exclude the value from calculations
            Dim newSite As Site = New Site(siteOID, m_siteName, SiteType.Pseudo, siteElevation, False)
            'Reset m_site to Nothing for the next site
            m_siteName = Nothing
            'Updates the site attributes
            Dim success As BA_ReturnCode = BA_UpdatePseudoSiteAttributes(layersFolder, BA_EnumDescription(MapsFileName.Pseudo), _
                                                                         siteOID, newSite)
            If success = BA_ReturnCode.Success Then
                'Adds the sites to 'existing sites' on the form
                siteScenarioForm.AddNewPseudoSite(newSite)

                'Reload the pseudo sites layer
                filepath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers, True)
                Dim pColor As IColor = New RgbColor
                FileName = BA_EnumDescription(MapsFileName.Pseudo)
                pColor.RGB = RGB(255, 170, 0) 'electron gold
                retVal = BA_MapDisplayPointMarkers(My.ThisApplication, filepath & FileName, MapsLayerName.pseudo_sites, pColor, MapsMarkerType.PseudoSite)

                'Set the global variable for pseudo-sites to true
                AOI_HasPseudoSite = True
            Else
                MessageBox.Show("An error occurred while trying to add this pseudo-site to the feature class.", "Error adding site", MessageBoxButtons.OK, _
                                MessageBoxIcon.Warning)
            End If
            pGraCont.DeleteElement(pElem)
        Catch ex As Exception
            Debug.Print("OnMouseDown Exception: " & ex.Message)
        Finally
            My.ArcMap.Application.CurrentTool = commandItem
        End Try
    End Sub

End Class