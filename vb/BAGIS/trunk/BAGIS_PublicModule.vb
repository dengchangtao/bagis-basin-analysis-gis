Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesFile
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Geoprocessing
Imports ESRI.ArcGIS.SpatialAnalyst
'Imports ESRI.ArcGIS.DataSourcesGDB
Imports System.Windows.Forms
Imports System.IO
Imports ESRI.ArcGIS.esriSystem
Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.Desktop.AddIns
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.GeoAnalyst
Imports ESRI.ArcGIS.DataSourcesGDB
Imports ESRI.ArcGIS.Framework

Module BAGIS_PublicModule

    'load terrain, drainage, and watershed layers if specified in the settings
    Public Sub BA_LoadReferenceLayers(ByVal TerrainString As String, _
                                      ByVal DrainageString As String, _
                                      ByVal WatershedString As String, ByVal pourpointString As String)
        If Len(Trim(DrainageString)) = 0 And _
        Len(Trim(TerrainString)) = 0 And _
        Len(Trim(WatershedString)) = 0 And _
        Len(Trim(pourpointString)) Then
            MsgBox("No reference layer specified in the settings!")
            Exit Sub
        End If

        Dim response As Integer
        Dim pMxDoc As IMxDocument = My.ArcMap.Document
        Dim pMap As IMap = pMxDoc.FocusMap

        'initialize the boolean variables
        Dim TerrainLayerNotExist As Boolean = True
        Dim DrainageLayerNotExist As Boolean = True
        Dim WatershedLayerNotExist As Boolean = True
        Dim pourpointLayerNotExist As Boolean = True

        'set layer names
        Dim TerrainLayerName As String = "Terrain Reference"
        Dim DrainageLayerName As String = "Drainage Reference"
        Dim WatershedLayerName As String = "Watershed Reference"
        Dim PourPointLayerName As String = "Gauge Stations"

        'check if reference layers with the assigned name exists
        'search layer of the specified name, if found set the boolean variables to true
        Dim nlayers As Long
        Dim i As Long
        Dim pTempLayer As ILayer
        nlayers = pMap.LayerCount
        For i = nlayers To 1 Step -1
            pTempLayer = pMap.Layer(i - 1)
            If pTempLayer.Name = TerrainLayerName Then TerrainLayerNotExist = False
            If pTempLayer.Name = DrainageLayerName Then DrainageLayerNotExist = False
            If pTempLayer.Name = WatershedLayerName Then WatershedLayerNotExist = False
            If pTempLayer.Name = PourPointLayerName Then pourpointLayerNotExist = False
        Next
        pTempLayer = Nothing

        'all layers are in the active map, exit the sub
        If Not TerrainLayerNotExist And Not DrainageLayerNotExist And Not WatershedLayerNotExist And Not pourpointLayerNotExist Then
            Exit Sub
        End If

        'append drainage layer to the map
        If Len(DrainageString) > 0 And DrainageLayerNotExist Then  'load drainage reference layer
            response = BA_AddLayerFromFile(DrainageString, DrainageLayerName, True)
            If response <> 1 Then MsgBox("Cannot load the drainage reference layer! Please check settings.")
        End If

        'append watershed layer to the map
        If Len(WatershedString) > 0 And WatershedLayerNotExist Then  'load watershed reference layer
            response = BA_AddLayerFromFile(WatershedString, WatershedLayerName, True)
            If response <> 1 Then MsgBox("Cannot load the watershed reference layer! Please check settings.")
        End If

        'append terrain layer to the map
        If Len(TerrainString) > 0 And TerrainLayerNotExist Then 'load terrain reference layer
            response = BA_AddLayerFromFile(TerrainString, TerrainLayerName, True)
            If response <> 1 Then MsgBox("Cannot load the terrain reference layer! Please check settings.")
        End If

        'append pourpoint layer to the map
        If Not String.IsNullOrEmpty(pourpointString) And pourpointLayerNotExist Then  'load drainage reference layer
            'Dim pMarkercolor As IRgbColor = New RgbColor
            'pMarkercolor.RGB = RGB(255, 0, 0) 'red
            Dim success As BA_ReturnCode = AddShapefile(pourpointString, PourPointLayerName)
            'Dim success As BA_ReturnCode = BA_MapDisplayPointMarkers(My.ArcMap.Application, pourpointString, MapsLayerName.Pourpoint, pMarkercolor, MapsMarkerType.Pourpoint)
            If success <> BA_ReturnCode.Success Then MsgBox("Cannot load the pourpoint layer! Please check settings.")
        End If

        'refresh the active view
        pMxDoc.UpdateContents()
        pMxDoc.ActiveView.PartialRefresh(2, Nothing, Nothing) 'esriViewGeography
        Exit Sub
    End Sub

    Private Function AddShapefile(ByVal pourpointpath As String, ByVal displayename As String) As BA_ReturnCode
        Dim mypath As String = ""
        Dim pourpointlayer As String = BA_GetBareName(pourpointpath, mypath)
        Dim pMxDoc As IMxDocument = My.ArcMap.Document
        Dim pMap As IMap = pMxDoc.FocusMap
        Dim pWksFactory As IWorkspaceFactory
        Dim pFeatWorkspace As IFeatureWorkspace
        Dim pFeatClass As IFeatureClass
        Dim pFLayer As IFeatureLayer
        Dim Markercolor As IRgbColor

        Try
            'open file from the containing folder
            pWksFactory = New ShapefileWorkspaceFactory
            pFeatWorkspace = pWksFactory.OpenFromFile(mypath, 0)
            pFeatClass = pFeatWorkspace.OpenFeatureClass(pourpointlayer)

            'add featureclass to current data frame
            pFLayer = New FeatureLayer
            pFLayer.FeatureClass = pFeatClass
            pFLayer = New FeatureLayer
            pFLayer.FeatureClass = pFeatClass
            pFLayer.Name = displayename

            'set layer symbology - hollow with red outline
            'Set marker symbol
            Dim pStyleGallery As IStyleGallery = pMxDoc.StyleGallery

            Dim pEnumMarkers As IEnumStyleGalleryItem = pStyleGallery.Items("Marker Symbols", "ESRI.style", "default")
            pEnumMarkers.Reset()

            Dim pStyleItem As IStyleGalleryItem = pEnumMarkers.Next

            Markercolor = New RgbColor
            Markercolor.RGB = RGB(255, 0, 0)

            Dim pMSymbol As IMarkerSymbol = Nothing
            Do Until pStyleItem Is Nothing
                If pStyleItem.Name = "Circle 8" Then
                    pMSymbol = pStyleItem.Item
                    pMSymbol.Size = 15
                    pMSymbol.Color = Markercolor
                    Exit Do
                End If
                pStyleItem = pEnumMarkers.Next
            Loop

            Dim pGFLayer As IGeoFeatureLayer
            Dim pRenderer As ISimpleRenderer

            pRenderer = New SimpleRenderer
            pRenderer.Symbol = pMSymbol
            pGFLayer = pFLayer
            pGFLayer.Renderer = pRenderer

            'Add the Layer to the focus map
            'refresh the active view
            pMxDoc.AddLayer(pFLayer)
            pMxDoc.UpdateContents()
            pMxDoc.ActivatedView.Refresh()
            Return BA_ReturnCode.Success

        Catch ex As Exception
            Return BA_ReturnCode.UnknownError

        Finally
            pWksFactory = Nothing
            pFeatWorkspace = Nothing
            pFLayer = Nothing
            pFeatClass = Nothing
            pFeatWorkspace = Nothing
            pWksFactory = Nothing

            GC.Collect()
            GC.WaitForPendingFinalizers()

        End Try
    End Function

    'add a layer file (.lyr) to map
    'return value 1: normal, 0: failed
    Public Function BA_AddLayerFromFile(ByVal layerPathFile As String, ByVal LayerName As String, ByVal AddToButtom As Boolean) As Integer
        Dim return_value As Integer
        return_value = 0

        On Error GoTo AddLayer_Error

        'Create a new GxLayer.
        Dim gxLayerCls As IGxLayer = New GxLayer
        Dim gxFile As IGxFile = gxLayerCls 'QI.

        'Set the path for where the layer file is located on disk.
        gxFile.Path = layerPathFile
        gxLayerCls.Layer.Name = LayerName

        'Test if you have a valid layer and add it to the map.
        If Not gxLayerCls.Layer Is Nothing Then
            Dim pMxDoc As IMxDocument = My.ArcMap.Document
            Dim pMap As IMap = pMxDoc.FocusMap
            Dim pLayer As ILayer = gxLayerCls.Layer

            pMap.AddLayer(pLayer)
            If AddToButtom Then
                pMap.MoveLayer(pLayer, pMap.LayerCount - 1) 'move to end
            Else
                pMap.MoveLayer(pLayer, 0) 'move to top
            End If
        End If
        return_value = 1

AddLayer_Error:
        BA_AddLayerFromFile = return_value
    End Function

    'This procedure adds a vector layer to ArcMap
    'DisplayName if specified, is used as the layer name in the TOC in ArcMap
    'Action code: 0, default add and replace if layer of the same name already added, and zoom to layer
    'Return values: -1 unknown error occurred
    '                       -2 filename error
    '                       -3 not a raster dataset
    '                       , otherwise, the same value as the Action code
    'add a raster file to the active view
    Public Function BA_MapDisplayVector(ByVal LayerPathName As String, Optional ByVal DisplayName As String = "") As Integer
        Dim return_code As Integer
        Dim File_Path As String = ""
        Dim File_Name As String
        File_Name = BA_GetBareName(LayerPathName, File_Path)

        return_code = -1

        'exit if file_name is null
        If Len(File_Name) = 0 Then
            return_code = -2
            GoTo ErrorHandler
        End If

        On Error GoTo ErrorHandler

        'text exists for the setting of this layer
        Dim pWksFactory As IWorkspaceFactory
        pWksFactory = New ShapefileWorkspaceFactory
        Dim pFeatWorkspace As IFeatureWorkspace
        pFeatWorkspace = pWksFactory.OpenFromFile(File_Path, 0)
        Dim pFeatClass As IFeatureClass
        pFeatClass = pFeatWorkspace.OpenFeatureClass(File_Name)

        'add featureclass to current data frame
        Dim pFLayer As IFeatureLayer
        pFLayer = New FeatureLayer
        pFLayer.FeatureClass = pFeatClass

        'set layer name
        If Len(DisplayName) = 0 Then
            pFLayer.Name = pFLayer.FeatureClass.AliasName
        Else
            pFLayer.Name = DisplayName
        End If

        'add layer
        Dim pMxDoc As IMxDocument = My.ArcMap.Document
        Dim pMap As IMap = pMxDoc.FocusMap


        'check if a layer with the assigned name exists
        'search layer of the specified name, if found
        Dim nlayers As Long
        Dim i As Long
        Dim pTempLayer As ILayer
        nlayers = pMap.LayerCount
        For i = nlayers To 1 Step -1
            pTempLayer = pMap.Layer(i - 1)
            If DisplayName = pTempLayer.Name Then 'remove the layer
                pMap.DeleteLayer(pTempLayer)
            End If
        Next

        'refresh the active view
        pMxDoc.AddLayer(pFLayer)
        pMxDoc.UpdateContents()
        pMxDoc.ActivatedView.Refresh()

        return_code = 0

ErrorHandler:
        BA_MapDisplayVector = return_code
        pFeatClass = Nothing
        pFLayer = Nothing
        pWksFactory = Nothing
        pFeatWorkspace = Nothing
        pTempLayer = Nothing
    End Function

    Public Sub BA_SetPRISMFolderNames()
        'set PRISM file names
        PRISMLayer(0) = "Jan"
        PRISMLayer(1) = "Feb"
        PRISMLayer(2) = "Mar"
        PRISMLayer(3) = "Apr"
        PRISMLayer(4) = "May"
        PRISMLayer(5) = "Jun"
        PRISMLayer(6) = "Jul"
        PRISMLayer(7) = "Aug"
        PRISMLayer(8) = "Sep"
        PRISMLayer(9) = "Oct"
        PRISMLayer(10) = "Nov"
        PRISMLayer(11) = "Dec"
        PRISMLayer(12) = "Q1"
        PRISMLayer(13) = "Q2"
        PRISMLayer(14) = "Q3"
        PRISMLayer(15) = "Q4"
        PRISMLayer(16) = "Annual"
    End Sub

    'call this sub when an AOI is set
    Public Sub BA_SetAOI(ByVal aoiname_string As String, Optional ByVal IsNewAOI As Boolean = False)
        'error checking
        'if the function is called after a new AOI was created, then the CreateAOI button remains active and the Analysis and Maps features follow the setting's parameter
        Dim response As Integer
        Dim BasinFolderString As String = ""

        aoiname_string = BA_GetBareName(aoiname_string, BasinFolderString)

        If Len(aoiname_string) = 0 Then
            MsgBox("Unable to set AOI! AOI name is invalid.")
            Exit Sub
        End If

        Dim comboBox = AddIn.FromID(Of cboTargetedAOI)(My.ThisAddIn.IDs.cboTargetedAOI)
        'If comboBox IsNot Nothing Then
        comboBox.setValue(aoiname_string)
        'thisdocument.selectedaoi.text = aoiname_string

        'enable the SaveAOIMXD button
        'SaveAOIMXD_Flag = True

        Dim SaveAOIMXDButton = AddIn.FromID(Of BtnSaveAOIMXD)(My.ThisAddIn.IDs.BtnSaveAOIMXD)
        If IsNewAOI Then
            SaveAOIMXDButton.selectedProperty = True
            'use system BA_systemsettings file, ignore the AOI settings file
            'read AOI definition file into BA_SystemSettings
            'If BA_ReadBAGISSettings(AOIFolderBase) <= 0 Then 'unable to read the definition file in the AOI folder
            BA_SystemSettings.Status = 0 'reset the settings variable
            BA_SetSettingPath() 'this subroutine sets the BA_Settings_Filepath global variable
            response = BA_ReadBAGISSettings(BA_Settings_Filepath)  'read the system settings instead
            'MsgBox("The AOI does not have a system settings definition file. The system setting file is used.")
            'End If
        Else
            SaveAOIMXDButton.selectedProperty = False
        End If

        If Len(BasinFolderBase) = 0 Then 'user bypass BASIN, select an AOI directly
            'MsgBox "Basin folder is not specified."
            AOIFolderBase = BasinFolderString & aoiname_string
        Else
            AOIFolderBase = BasinFolderBase & "\" & aoiname_string
        End If

        'enable AOI info tool
        'AOIInfo_Flag = True
        Dim AOIInfoToolButton = AddIn.FromID(Of BtnAOI_Tool)(My.ThisAddIn.IDs.BtnAOI_Tool)
        AOIInfoToolButton.selectedProperty = True

        'enable BtnCreateAOIStream when AOI is selected
        Dim createAOIStreamButton = AddIn.FromID(Of BtnCreateAOIStream)(My.ThisAddIn.IDs.BtnCreateAOIStream)
        createAOIStreamButton.selectedProperty = True

        System.Windows.Forms.Application.DoEvents()

        'disable map selection tools
        BA_Enable_MapFlags(False)

        'CreateAOIStream_Flag = True
        createAOIStreamButton.selectedProperty = True

        'disable AOI tools
        'SetPourPoint_Flag = False
        Dim SetPourPointtool = AddIn.FromID(Of setPourPointtool)(My.ThisAddIn.IDs.setPourPointtool)
        SetPourPointtool.selectedProperty = False

        'enable the map generating tools if the required data (SNOTEL, Snow Course, and PRISM) are available, check only for PRISM Q4 raster
        Dim GeneratMapsButton = AddIn.FromID(Of BtnGenerateMaps)(My.ThisAddIn.IDs.BtnGenerateMaps)
        Dim SiteScenarioButton = AddIn.FromID(Of BtnSiteScenario)(My.ThisAddIn.IDs.BtnSiteScenario)
        Dim Scenario1RepButton = AddIn.FromID(Of BtnScenario1)(My.ThisAddIn.IDs.BtnScenario1)
        Dim Scenario2RepButton = AddIn.FromID(Of BtnScenario2)(My.ThisAddIn.IDs.BtnScenario2)
        Dim DifferenceConditionButton = AddIn.FromID(Of BtnDifferenceCondition)(My.ThisAddIn.IDs.BtnDifferenceCondition)
        Dim SiteRepresentationButton = AddIn.FromID(Of BtnSiteRepresentation)(My.ThisAddIn.IDs.BtnSiteRepresentation)
        'disable addition elevational representation maps
        Scenario1RepButton.SelectedProperty = False
        Scenario2RepButton.SelectedProperty = False
        DifferenceConditionButton.SelectedProperty = False
        SiteRepresentationButton.selectedProperty = False
        'disable SNOTEL, Snow Course, and Pseudo-Site flags; They will be re-calculated by Maps or Site Scenario tools
        AOI_HasSNOTEL = False
        AOI_HasSnowCourse = False
        AOI_HasPseudoSite = False

        If BA_SystemSettings.GenerateAOIOnly Then
            GeneratMapsButton.SelectedProperty = False
            SiteScenarioButton.selectedProperty = False
            SaveAOIMXDButton.selectedProperty = False
        Else
            GeneratMapsButton.SelectedProperty = True
            SiteScenarioButton.selectedProperty = True
            SaveAOIMXDButton.selectedProperty = True
        End If

        'Close the dockable windows for Site Scenario and Representation if they are open
        Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow
        Dim dockWinID As UID = New UIDClass()
        dockWinID.Value = My.ThisAddIn.IDs.frmSiteScenario
        dockWindow = My.ArcMap.DockableWindowManager.GetDockableWindow(dockWinID)
        If dockWindow IsNot Nothing AndAlso dockWindow.IsVisible Then dockWindow.Show(False)
        Dim dockWindow2 As ESRI.ArcGIS.Framework.IDockableWindow
        Dim dockWinID2 As UID = New UIDClass()
        dockWinID2.Value = My.ThisAddIn.IDs.frmSiteRepresentations
        dockWindow2 = My.ArcMap.DockableWindowManager.GetDockableWindow(dockWinID2)
        If dockWindow2 IsNot Nothing AndAlso dockWindow2.IsVisible Then dockWindow2.Show(False)

    End Sub

    Public Sub BA_Enable_MapFlags(ByVal BValue As Boolean)
        'Basin Analysis Tools
        'ElevDistMap_Flag = BValue
        Dim ElevDistButton = AddIn.FromID(Of BtnElevationDist)(My.ThisAddIn.IDs.BtnElevationDist)
        ElevDistButton.SelectedProperty = BValue

        Dim ElevSNOTELButton = AddIn.FromID(Of BtnElevationSNOTEL)(My.ThisAddIn.IDs.BtnElevationSNOTEL)
        Dim ElevSnowCourse = AddIn.FromID(Of BtnElevSnowCourse)(My.ThisAddIn.IDs.BtnElevSnowCourse)

        If BValue Then 'enable flags
            'If AOI_HasSNOTEL Then ElevSNOTELMap_Flag = BValue
            'If AOI_HasSnowCourse Then ElevSnowCourseMap_Flag = BValue

            If AOI_HasSNOTEL Then ElevSNOTELButton.SelectedProperty = BValue
            If AOI_HasSnowCourse Then ElevSnowCourse.SelectedProperty = BValue

        Else 'disable flags
            'ElevSNOTELMap_Flag = BValue
            'ElevSnowCourseMap_Flag = BValue
            ElevSNOTELButton.SelectedProperty = BValue
            ElevSnowCourse.SelectedProperty = BValue

        End If
        Dim PrecipDistMapButton = AddIn.FromID(Of BtnPrecipitationDist)(My.ThisAddIn.IDs.BtnPrecipitationDist)
        Dim SlopeDistMapButton = AddIn.FromID(Of BtnSlopeDist)(My.ThisAddIn.IDs.BtnSlopeDist)
        Dim AspectDistMapButton = AddIn.FromID(Of BtnAspectDist)(My.ThisAddIn.IDs.BtnAspectDist)
        PrecipDistMapButton.SelectedProperty = BValue
        SlopeDistMapButton.SelectedProperty = BValue
        AspectDistMapButton.SelectedProperty = BValue
        'PrecipDistMap_Flag = BValue
        'SlopeDistMap_Flag = BValue
        ' AspectDistMap_Flag = BValue
        'If ActualRepMap_Flag = True Then
        '    Dim ActualRepButton = AddIn.FromID(Of BtnActualCondition)(My.ThisAddIn.IDs.BtnActualCondition)
        '    ActualRepButton.SelectedProperty = True
        'End If
        'If PseudoRepMap_Flag = True Then
        '    Dim PseudoRepButton = AddIn.FromID(Of BtnPseudoCondition)(My.ThisAddIn.IDs.BtnPseudoCondition)
        '    PseudoRepButton.SelectedProperty = True
        'End If

        'If RepDifferenceMap_Flag = True Then
        '    Dim RefDifferenceButton = AddIn.FromID(Of BtnDifferenceCondition)(My.ThisAddIn.IDs.BtnDifferenceCondition)
        '    RefDifferenceButton.SelectedProperty = True
        'End If

        'Dim SiteConditionButton = AddIn.FromID(Of BtnSiteCondition)(My.ThisAddIn.IDs.BtnSiteCondition)
        'SiteConditionButton.selectedProperty = True
        Maps_Are_Generated = BValue
    End Sub

    Public Sub BA_Enable_ScenarioMapFlags(ByVal BValue As Boolean)
        'Basin Analysis Tools
        'ElevDistMap_Flag = BValue
        'Dim ElevDistButton = AddIn.FromID(Of BtnElevationDist)(My.ThisAddIn.IDs.BtnElevationDist)
        'ElevDistButton.SelectedProperty = BValue

        'Dim ElevSNOTELButton = AddIn.FromID(Of BtnElevationSNOTEL)(My.ThisAddIn.IDs.BtnElevationSNOTEL)
        'Dim ElevSnowCourse = AddIn.FromID(Of BtnElevSnowCourse)(My.ThisAddIn.IDs.BtnElevSnowCourse)

        'If BValue Then 'enable flags
        '    'If AOI_HasSNOTEL Then ElevSNOTELMap_Flag = BValue
        '    'If AOI_HasSnowCourse Then ElevSnowCourseMap_Flag = BValue

        '    If AOI_HasSNOTEL Then ElevSNOTELButton.SelectedProperty = BValue
        '    If AOI_HasSnowCourse Then ElevSnowCourse.SelectedProperty = BValue

        'Else 'disable flags
        '    'ElevSNOTELMap_Flag = BValue
        '    'ElevSnowCourseMap_Flag = BValue
        '    ElevSNOTELButton.SelectedProperty = BValue
        '    ElevSnowCourse.SelectedProperty = BValue

        'End If
        'Dim PrecipDistMapButton = AddIn.FromID(Of BtnPrecipitationDist)(My.ThisAddIn.IDs.BtnPrecipitationDist)
        'Dim SlopeDistMapButton = AddIn.FromID(Of BtnSlopeDist)(My.ThisAddIn.IDs.BtnSlopeDist)
        'Dim AspectDistMapButton = AddIn.FromID(Of BtnAspectDist)(My.ThisAddIn.IDs.BtnAspectDist)
        'PrecipDistMapButton.SelectedProperty = BValue
        'SlopeDistMapButton.SelectedProperty = BValue
        'AspectDistMapButton.SelectedProperty = BValue
        'PrecipDistMap_Flag = BValue
        'SlopeDistMap_Flag = BValue
        'AspectDistMap_Flag = BValue
        Dim SiteScenario1RepButton = AddIn.FromID(Of BtnScenario1)(My.ThisAddIn.IDs.BtnScenario1)
        Dim SiteScenario2RepButton = AddIn.FromID(Of BtnScenario2)(My.ThisAddIn.IDs.BtnScenario2)
        Dim RefDifferenceButton = AddIn.FromID(Of BtnDifferenceCondition)(My.ThisAddIn.IDs.BtnDifferenceCondition)
        Dim SiteRepresentationButton = AddIn.FromID(Of BtnSiteRepresentation)(My.ThisAddIn.IDs.BtnSiteRepresentation)

        If Scenario1Map_Flag = True Then
            SiteScenario1RepButton.SelectedProperty = True
        Else
            SiteScenario1RepButton.SelectedProperty = False
        End If

        If Scenario2Map_Flag = True Then
            SiteScenario2RepButton.SelectedProperty = True
        Else
            SiteScenario2RepButton.SelectedProperty = False
        End If

        If RepDifferenceMap_Flag = True Then
            RefDifferenceButton.SelectedProperty = True
        Else
            RefDifferenceButton.SelectedProperty = False
        End If

        If SiteRepresentationMap_Flag = True Then
            SiteRepresentationButton.selectedProperty = True
        Else
            SiteRepresentationButton.selectedProperty = False
        End If
        ScenarioMaps_Are_Generated = BValue
    End Sub

    '    'write and update the information in the deminfo.txt file associated with a basin or an AOI
    '    Public Function BA_ReadDEMInfo(ByVal FolderBase As String, ByRef deminfo As BA_DEMInfo) As BA_ReturnCode
    '        Dim fileno As Integer = 0
    '        Dim FileName As String
    '        Dim return_value As BA_ReturnCode = BA_ReturnCode.UnknownError
    '        Dim linestring As String
    '        Dim response As Integer = 0

    '        'return_value = -1
    '        deminfo.Exist = False
    '        If Len(FolderBase) = 0 Then GoTo Function_Exit

    '        'pad a backslash to the path if it doesn't have one.
    '        If Right(FolderBase, 1) <> "\" Then
    '            FileName = FolderBase & "\" & BA_Basin_Type
    '        Else
    '            FileName = FolderBase & BA_Basin_Type
    '        End If

    '        'check if the deminfo.txt file exists
    '        If Not File.Exists(FileName) Then Return BA_ReturnCode.ReadError

    '        Using sr As StreamReader = File.OpenText(FileName)
    '            Try
    '                'read the idtext(i.e., first line) from the deminfo file
    '                linestring = sr.ReadLine
    '                deminfo.IDText = linestring

    '                linestring = sr.ReadLine
    '                deminfo.Cellsize = Val(linestring)

    '                linestring = sr.ReadLine
    '                deminfo.Min = Val(linestring)

    '                linestring = sr.ReadLine
    '                deminfo.Max = Val(linestring)

    '                linestring = sr.ReadLine
    '                deminfo.Range = Val(linestring)

    '                ' ''Close #fileno
    '                sr.Close()

    '                deminfo.Exist = True
    '                return_value = BA_ReturnCode.Success

    '            Catch
    '                sr.Close()
    '                'if deminfo file doesn't have the dem stats, then get raster stats from DEM and save the info to the deminfo.txt
    '                BA_GetDEMStats(FolderBase, deminfo)
    '                return_value = BA_UpdateDEMInfo(FolderBase, deminfo)
    '                Return return_value
    '            End Try
    '        End Using

    'Function_Exit:
    '        Return return_value
    '    End Function

    '    'get rasterresolution
    '    Public Sub BA_GetDEMStats(ByVal FolderBase As String, ByVal deminfo As BA_DEMInfo)
    '        Dim DemFileName As String
    '        Dim DemFilePath As String
    '        Dim response As Integer

    '        If Len(FolderBase) = 0 Then 'not a valide input
    '            deminfo.Exist = False
    '            Exit Sub
    '        End If

    '        'get the location and name of the dem.
    '        DemFilePath = BA_GetPath(FolderBase, PublicPath.DEM)
    '        DemFileName = DemFilePath & "\grid"

    '        'check if the raster exists
    '        If Not BA_Workspace_Exists(DemFileName) Then
    '            deminfo.Exist = False
    '            Exit Sub
    '        End If

    '        On Error GoTo ErrorHandler

    '        'Open Raster
    '        Dim pDEUtility As IDEUtilities = New DEUtilities
    '        Dim pRasterDataset As IRasterDataset
    '        Dim pTempRaster As IGeoDataset

    '        Dim pWS As IRasterWorkspace
    '        Dim pWSF As IWorkspaceFactory = New RasterWorkspaceFactory
    '        pWS = pWSF.OpenFromFile(DemFilePath, 0)
    '        pRasterDataset = pWS.OpenRasterDataset("grid")

    '        Dim pRasterBand As IRasterBand
    '        Dim pRasterBandCollection As IRasterBandCollection
    '        Dim pRasterP As IRasterProps
    '        Dim pRStats As IRasterStatistics
    '        Dim pPnt As IPnt

    '        'see if the DEM was created with a buffer
    '        'BASIN dem wasn't bufferred
    '        'AOI dem could be bufferred. If bufferred, there will be an aoib_v.shp in the AOI folder
    '        If Len(Dir(FolderBase & "\" & BA_BufferedAOIExtentCoverage & ".shp", vbNormal)) <> 0 Then 'DEM is bufferred
    '            'use the unbufferred aoi to mask out the dem
    '            'Use the AOI extent for analysis
    '            Dim pAOIRaster As IGeoDataset
    '            'Open AOI Polygon to set the analysis mask
    '            pAOIRaster = BA_OpenRasterFromFile(FolderBase, BA_AOIExtentRaster)

    '            If pAOIRaster Is Nothing Then GoTo ErrorHandler

    '            Dim pExtractOp As IExtractionOp = New RasterExtractionOp
    '            pTempRaster = pExtractOp.Raster(pRasterDataset, pAOIRaster)

    '            pRasterBandCollection = pTempRaster
    '            pExtractOp = Nothing
    '            pAOIRaster = Nothing
    '        Else 'DEM is not bufferred
    '            pRasterBandCollection = pRasterDataset
    '        End If

    '        pRasterBand = pRasterBandCollection.Item(0)
    '        pRStats = pRasterBand.Statistics
    '        pRasterP = pRasterBand
    '        pPnt = New DblPnt
    '        pPnt = pRasterP.MeanCellSize

    '        With deminfo
    '            .Min = pRStats.Minimum
    '            .Max = pRStats.Maximum
    '            .Range = .Max - .Min
    '            .Cellsize = (pPnt.X + pPnt.Y) / 2
    '            .Exist = True
    '        End With

    '        pTempRaster = Nothing
    '        pRasterBandCollection = Nothing
    '        pRasterBand = Nothing
    '        pRasterDataset = Nothing
    '        pRStats = Nothing
    '        pWS = Nothing
    '        pWSF = Nothing
    '        pDEUtility.ReleaseInternals()

    '        Exit Sub

    'ErrorHandler:
    '        pTempRaster = Nothing
    '        pRasterBand = Nothing
    '        pRasterBandCollection = Nothing
    '        pRasterDataset = Nothing
    '        pRStats = Nothing
    '        pWS = Nothing
    '        pWSF = Nothing
    '        pDEUtility.ReleaseInternals()
    '        deminfo.Exist = False
    '    End Sub

    'write and update the information in the deminfo.txt file associated with a basin or an AOI
    Public Function BA_UpdateDEMInfo(ByVal FolderBase As String, ByVal deminfo As BA_DEMInfo) As BA_ReturnCode
        Dim fileno As Integer
        Dim FileName As String
        Dim return_value As BA_ReturnCode = BA_ReturnCode.UnknownError
        Dim IDText As String

        return_value = -1
        If Len(FolderBase) = 0 Or deminfo.Exist <> True Then GoTo Function_Exit

        'pad a backslash to the path if it doesn't have one.
        If Right(FolderBase, 1) <> "\" Then
            FileName = FolderBase & "\" & BA_Basin_Type
        Else
            FileName = FolderBase & BA_Basin_Type
        End If

        'check if the deminfo.txt file exists
        If Len(Dir(FileName, vbNormal)) = 0 Then GoTo Function_Exit 'i.e., file does not exist

        On Error GoTo Function_Exit
        'read the idtext(i.e., first line) from the deminfo file

        Using sr As StreamReader = File.OpenText(FileName)
            IDText = sr.ReadLine()
            sr.Close()
        End Using

        'open file for output
        Using sw As StreamWriter = File.CreateText(FileName)
            sw.WriteLine(IDText)
            sw.WriteLine(deminfo.Cellsize)
            sw.WriteLine(deminfo.Min)
            sw.WriteLine(deminfo.Max)
            sw.WriteLine(deminfo.Range)
            sw.Close()
        End Using

        return_value = BA_ReturnCode.Success

Function_Exit:
        BA_UpdateDEMInfo = return_value
    End Function

    'create the source.weasel file
    'return value: -1 nothing was created, 1, file was created sussessfully
    Public Function BA_Create_SourceWeasel_File(ByVal path_string As String, ByVal name_string As String, ByVal text_string As String) As BA_ReturnCode
        Dim FileName As String
        Dim sw As StreamWriter = Nothing

        Try
            If Len(path_string) = 0 Or Len(name_string) = 0 Then Return BA_ReturnCode.ReadError
            BA_StandardizePathString(path_string, True)
            FileName = path_string & "\" & name_string

            'open file for output
            sw = New StreamWriter(FileName)
            If sw IsNot Nothing Then
                sw.WriteLine(text_string)
            End If
            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_Create_SourceWeasel_File: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            sw.Close()
        End Try
    End Function

    'generate a list of the subfolders and their basin/aoi designations in a folder
    'a basin folder has a file called info.basin
    'an aoi folder has a file called source.weasel in the output subfolder
    'return code: the number of subfolders found, -1 for error
    Public Function BA_Dir(ByVal target_folder As String, ByRef return_list() As BA_SubFolderList, Optional ByRef HasAOIinFolder As Boolean = False) As Integer
        Dim nSubFolder As Long
        Dim subfoldername As String
        Dim FolderPath As String = Nothing
        Dim return_string As String
        HasAOIinFolder = False 'reset the value

        If String.IsNullOrEmpty(target_folder) Then
            nSubFolder = -1
            Return nSubFolder
        End If

        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim pProgD As IProgressDialog2 = Nothing
        pProgD = BA_GetAnimationProgressor(My.ArcMap.Application.hWnd, "Checking folder properties...", "BA_Dir (BAGIS Directory)")

        'pad a backslash to the path if it doesn't have one.
        If Right(target_folder, 1) <> "\" Then target_folder = target_folder & "\"

        nSubFolder = 0

        Dim dirList = My.Computer.FileSystem.GetDirectories(target_folder)
        nSubFolder = dirList.Count

        If nSubFolder = 0 Then 'GoTo Function_Exit 'no subfolder
            pProgD.HideDialog()
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pProgD)
            Return nSubFolder
        End If

        System.Array.Resize(return_list, nSubFolder)
        nSubFolder = 0
        For Each FolderPath In dirList
            ' Use bitwise comparison to make sure MyName is a directory.
            subfoldername = BA_GetBareName(FolderPath)

            ' Ignore the current directory and the encompassing directory.
            If subfoldername <> "." And subfoldername <> ".." Then

                return_list(nSubFolder).Name = subfoldername
                'check GDB's dem and AOI status

                'Checks if the Geodatabase contains an aoi, if it does, then the folder is also a BASIN
                Dim folderType As FolderType = BA_GetFGDBFolderType(FolderPath)
                Dim demPath As String = FolderPath & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\" & BA_EnumDescription(MapsFileName.filled_dem_gdb)
                Dim rasterResolution As Double = 0

                If folderType <> folderType.AOI Then
                    If BA_File_Exists(demPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then 'The folder has DEM
                        BA_GetRasterStatsGDB(demPath, rasterResolution)
                        return_list(nSubFolder).gdbdem = Math.Round(rasterResolution, 2) & " meters resolution"
                    Else
                        return_list(nSubFolder).gdbdem = "No"
                    End If
                    return_list(nSubFolder).gdbAOI = False
                Else
                    If BA_GetRasterStatsGDB(demPath, rasterResolution) Is Nothing Then
                        return_list(nSubFolder).gdbdem = "No"
                        return_list(nSubFolder).gdbAOI = False
                    Else
                        return_list(nSubFolder).gdbdem = Math.Round(rasterResolution, 2) & " meters resolution"
                        return_list(nSubFolder).gdbAOI = True
                        HasAOIinFolder = True
                    End If
                End If

                'check weasel dem status
                return_string = BA_Check_Folder_Type(FolderPath, BA_Basin_Type)
                If Not String.IsNullOrEmpty(return_string) Then 'the folder has DEM
                    return_list(nSubFolder).weaseldem = return_string
                Else
                    return_list(nSubFolder).weaseldem = "No"
                End If

                'check weasel AOI status
                return_string = BA_Check_Folder_Type(FolderPath, BA_AOI_Type)
                If Not String.IsNullOrEmpty(return_string) Then 'the folder is an AOI
                    return_list(nSubFolder).weaselAOI = True
                    HasAOIinFolder = True
                Else
                    return_list(nSubFolder).weaselAOI = False
                End If
                nSubFolder = nSubFolder + 1

            End If

        Next

        pProgD.HideDialog()
        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pProgD)

        Return nSubFolder
    End Function


    ''' *************************************************************************
    ''' Module Constant Declaractions Follow
    ''' *************************************************************************
    ''' Constant for the dwDesiredAccess parameter of the OpenProcess API function.
    Private Const PROCESS_QUERY_INFORMATION As Long = &H400
    ''' Constant for the lpExitCode parameter of the GetExitCodeProcess API function.
    Private Const STILL_ACTIVE As Long = &H103

    ''' *************************************************************************
    ''' Module Variable Declaractions Follow
    ''' *************************************************************************
    ''' It's critical for the shell and wait procedure to trap for errors, but I
    ''' didn't want that to distract from the example, so I'm employing a very
    ''' rudimentary error handling scheme here. This variable is used to pass error
    ''' messages between procedures.
    Public gszErrMsg As String

    ''' *************************************************************************
    ''' Module DLL Declaractions Follow
    ''' *************************************************************************
    Private Declare Function OpenProcess Lib "kernel32" (ByVal dwDesiredAccess As Long, ByVal bInheritHandle As Long, ByVal dwProcessId As Long) As Long
    Private Declare Function GetExitCodeProcess Lib "kernel32" (ByVal hProcess As Long, ByVal lpExitCode As Long) As Long

    'Private Declare Function MessageBox Lib "User32" Alias "MessageBoxA" (ByVal hWnd As Long, _
    '        ByVal lpText As String, ByVal lpCaption As String, ByVal wType As Long) As Long
    'MessageBox &H0, ErrMessage, "ArcMap VBA ERROR!", vbInformation + vbSystemModal

    Public Declare Function CoFreeUnusedLibraries Lib "ole32.dll" () As Long

    '    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    '    ''' Comments:   Shells out to the specified command line and waits for it to
    '    '''             complete. The Shell function runs asynchronously, so you must
    '    '''             run it using this function if you need to do something with
    '    '''             its output or wait for it to finish before continuing.
    '    '''
    '    ''' Arguments:  szCommandLine   [in] The command line to execute using Shell.
    '    '''             iWindowState    [in] (Optional) The window state parameter to
    '    '''                             pass to the Shell function. Default = vbHide.
    '    '''
    '    ''' Returns:    Boolean         True on success, False on error.
    '    ''' 05/19/05    Rob Bovey       Created
    '    '''

    Public Function bShellAndWait(ByVal szCommandLine As String, Optional ByVal iWindowState As Integer = vbHide) As Boolean
        Dim lTaskID As Long
        Dim lProcess As Long
        Dim lExitCode As Long
        Dim lResult As Long

        Try
            ' Run the Shell function.
            lTaskID = Shell(szCommandLine, iWindowState)
            ' Check for errors.
            If lTaskID = 0 Then Err.Raise(9999, , "Shell function error.")

            ' Get the process handle from the task ID returned by Shell.
            lProcess = OpenProcess(PROCESS_QUERY_INFORMATION, 0&, lTaskID)

            ' Check for errors.
            If lProcess = 0 Then Err.Raise(9999, , "Unable to open Shell process handle.")

            ' Loop while the shelled process is still running.
            Do
                ' lExitCode will be set to STILL_ACTIVE as long as the shelled process is running.
                lResult = GetExitCodeProcess(lProcess, lExitCode)
                System.Windows.Forms.Application.DoEvents()
            Loop While lExitCode = STILL_ACTIVE
            Return True

        Catch ex As Exception
            gszErrMsg = Err.Description
            Return False

        End Try
    End Function

    'Public Function BA_DisplayPointMarkers(ByVal filepath As String, ByVal FileName As String, ByVal LayerName As String, ByVal MarkerColor As IRgbColor) As Integer

    '    Dim returnvalue As Integer = 0

    '    'exit if file_name is null
    '    If Len(FileName) = 0 Then
    '        Return 1
    '    End If

    '    Dim comReleaser As ESRI.ArcGIS.ADF.ComReleaser = New ESRI.ArcGIS.ADF.ComReleaser
    '    'load gauge station layer if it doesn't already exist
    '    Dim pMxDoc As IMxDocument = My.ArcMap.Document
    '    Dim pMap As IMap = pMxDoc.FocusMap

    '    'if a layer with the assigned name exists, then don't add the point layer
    '    'search layer of the specified name, if found
    '    Dim nlayers As Long
    '    Dim i As Long
    '    Dim pTempLayer As ILayer

    '    Dim pFLayer As IFeatureLayer
    '    nlayers = pMap.LayerCount

    '    For i = nlayers To 1 Step -1
    '        pTempLayer = pMap.Layer(i - 1)
    '        If LayerName = pTempLayer.Name Then
    '            pMap.DeleteLayer(pTempLayer)
    '        End If
    '    Next

    '    'open the point layer
    '    Dim pFeatClass As IFeatureClass = BA_OpenFeatureClassFromGDB(filepath, FileName)
    '    comReleaser.ManageLifetime(pFeatClass)

    '    Try
    '        'add featureclass to current data frame
    '        pFLayer = New FeatureLayer
    '        pFLayer.FeatureClass = pFeatClass
    '        pFLayer.Name = LayerName

    '        'check feature geometry type, only polygon layers can be used as an extent layer
    '        If pFLayer.FeatureClass.ShapeType <> esriGeometryType.esriGeometryPoint Then
    '            returnvalue = 2
    '        Else
    '            'add layer
    '            'set layer symbology - hollow with red outline
    '            'Set marker symbol
    '            Dim pStyleGallery As IStyleGallery = pMxDoc.StyleGallery
    '            Dim pEnumMarkers As IEnumStyleGalleryItem = pStyleGallery.Items("Marker Symbols", "ESRI.style", "default")
    '            pEnumMarkers.Reset()

    '            Dim pStyleItem As IStyleGalleryItem = pEnumMarkers.Next
    '            Dim pMarkerColor As IColor = New RgbColor
    '            pMarkerColor.RGB = MarkerColor.RGB

    '            Dim pMSymbol As IMarkerSymbol = Nothing
    '            Do Until pStyleItem Is Nothing
    '                If pStyleItem.Name = "Circle 8" Then
    '                    pMSymbol = pStyleItem.Item
    '                    pMSymbol.Size = 18
    '                    pMSymbol.Color = pMarkerColor
    '                    Exit Do
    '                End If
    '                pStyleItem = pEnumMarkers.Next
    '            Loop

    '            Dim pGFLayer As IGeoFeatureLayer = pFLayer
    '            Dim pRenderer As ISimpleRenderer = New SimpleRenderer
    '            pRenderer.Symbol = pMSymbol
    '            pGFLayer.Renderer = pRenderer

    '            pMap.AddLayer(pFLayer)

    '            'refresh the active view
    '            pMxDoc.ActivatedView.PartialRefresh(esriViewDrawPhase.esriViewForeground, Nothing, Nothing)
    '            pMxDoc.UpdateContents()
    '            returnvalue = 0
    '        End If

    '    Catch ex As Exception
    '        MsgBox("BA_DisplayPointMarkers Exception: " & ex.Message)
    '        returnvalue = -1

    '    End Try

    '    Return returnvalue
    'End Function

    '' Use Geoprocessor to delete a workspace (folder)
    'Public Function BA_CompactFGDB(ByVal FGDBPath As String) As BA_ReturnCode

    '    Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
    '    Dim pResult As ESRI.ArcGIS.Geoprocessing.IGeoProcessorResult = Nothing
    '    Dim tool As ESRI.ArcGIS.DataManagementTools.Compact = New ESRI.ArcGIS.DataManagementTools.Compact
    '    Try
    '        tool.in_workspace = FGDBPath
    '        pResult = GP.Execute(tool, Nothing)
    '        If pResult Is Nothing Then
    '            Return BA_ReturnCode.UnknownError
    '        Else
    '            Return BA_ReturnCode.Success
    '        End If

    '    Catch ex As Exception
    '        MsgBox("BA_CompactFGDB Exception: " & ex.Message)
    '        Return BA_ReturnCode.UnknownError

    '    Finally
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(tool)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pResult)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(GP)
    '    End Try

    'End Function

    '    'This procedure adds the specified DEM extent layer to ArcMap
    '    'DisplayName if specified, is used as the layer name in the TOC in ArcMap
    '    'Action code: 0, default add and replace if layer of the same name already added, and zoom to layer
    '    '                   1, add and replace only
    '    '                   2, add
    '    'Return values: -1 unknown error occurred
    '    '                       -2 filename error
    '    '                       -3 not polygon shapefile
    '    '                       , otherwise, the same value as the Action code
    '    'Note: the DEM extent of a basin is saved as dem.shp in the output\surfaces\filled folder
    '    Public Function BA_AddExtentLayer(ByVal LayerPathName As String, Optional ByVal DisplayName As String = Nothing, Optional ByVal Action As Integer = 0, Optional ByVal Buffer_Factor As Double = 0) As Integer
    '        Dim return_code As Integer
    '        Dim File_Path As String = Nothing
    '        Dim File_Name As String

    '        File_Name = BA_GetBareName(LayerPathName, File_Path)

    '        return_code = -1

    '        'exit if file_name is null
    '        If Len(File_Name) = 0 Then
    '            return_code = -2
    '            GoTo ErrorHandler
    '        End If

    '        On Error GoTo ErrorHandler

    '        'text exists for the setting of this layer
    '        Dim pWksFactory As IWorkspaceFactory
    '        pWksFactory = New ShapefileWorkspaceFactory
    '        Dim pFeatWorkspace As IFeatureWorkspace
    '        pFeatWorkspace = pWksFactory.OpenFromFile(File_Path, 0)
    '        Dim pFeatClass As IFeatureClass
    '        pFeatClass = pFeatWorkspace.OpenFeatureClass(File_Name)

    '        'add featureclass to current data frame
    '        Dim pFLayer As IFeatureLayer
    '        pFLayer = New FeatureLayer
    '        pFLayer.FeatureClass = pFeatClass

    '        'check feature geometry type, only polygon layers can be used as an extent layer
    '        If pFLayer.FeatureClass.ShapeType <> esriGeometryType.esriGeometryPolygon Then
    '            return_code = -3
    '            GoTo ErrorHandler
    '        End If

    '        'set layer name
    '        If IsNothing(DisplayName) Then
    '            pFLayer.Name = pFLayer.FeatureClass.AliasName
    '        Else
    '            pFLayer.Name = DisplayName
    '        End If

    '        If IsNothing(Action) Then 'default is to replace existing layer when adding
    '            Action = 0
    '        End If

    '        If IsNothing(Buffer_Factor) Then
    '            Buffer_Factor = 2
    '        End If

    '        'add layer
    '        Dim pMxDoc As IMxDocument = My.ArcMap.Document
    '        Dim pMap As IMap = pMxDoc.FocusMap

    '        'set layer symbology - hollow with red outline
    '        Dim pPolySym As ISimpleFillSymbol = New SimpleFillSymbol
    '        Dim pGFLayer As IGeoFeatureLayer
    '        Dim pRenderer As ISimpleRenderer

    '        Dim pLineSym As ILineSymbol = New SimpleLineSymbol
    '        pLineSym.Color = pDisplayColor

    '        pPolySym.Outline = pLineSym
    '        pPolySym.Style = esriSimpleFillStyle.esriSFSHollow
    '        pRenderer = New SimpleRenderer
    '        pRenderer.Symbol = pPolySym

    '        pGFLayer = pFLayer
    '        pGFLayer.Renderer = pRenderer

    '        'check if a layer with the assigned name exists
    '        Dim pTempLayer As ILayer
    '        If Action <> 2 Then
    '            'search layer of the specified name, if found
    '            Dim nlayers As Long
    '            Dim i As Long

    '            nlayers = pMap.LayerCount
    '            For i = nlayers To 1 Step -1
    '                pTempLayer = pMap.Layer(i - 1)
    '                If DisplayName = pTempLayer.Name Then 'remove the layer
    '                    pMap.DeleteLayer(pTempLayer)
    '                End If
    '            Next
    '        End If

    '        pMap.AddLayer(pFLayer)
    '        Dim pEnv As IEnvelope
    '        If Action = 0 Then 'zoom to layer
    '            'create a buffer around the AOI

    '            pEnv = pFLayer.AreaOfInterest
    '            Dim llx As Double, lly As Double, urx As Double, ury As Double
    '            Dim xrange As Double, yrange As Double
    '            Dim xoffset As Double, yoffset As Double

    '            pEnv.QueryCoords(llx, lly, urx, ury)
    '            xrange = urx - llx
    '            yrange = ury - lly
    '            xoffset = xrange * (Buffer_Factor - 1) / 2
    '            yoffset = yrange * (Buffer_Factor - 1) / 2
    '            llx = llx - xoffset
    '            lly = lly - yoffset
    '            urx = urx + xoffset
    '            ury = ury + yoffset
    '            pEnv.PutCoords(llx, lly, urx, ury)

    '            'pFLayer.AreaOfInterest.PutCoords llx, lly, urx, ury
    '            Dim pActiveView As IActiveView
    '            pActiveView = pMxDoc.ActiveView
    '            pActiveView.Extent = pEnv
    '        End If

    '        'refresh the active view
    '        pMxDoc.ActivatedView.Refresh()
    '        pMxDoc.UpdateContents()

    '        pFeatClass = Nothing
    '        pFLayer = Nothing
    '        pGFLayer = Nothing
    '        pWksFactory = Nothing
    '        pFeatWorkspace = Nothing
    '        pEnv = Nothing
    '        pTempLayer = Nothing
    '        pRenderer = Nothing

    '        return_code = Action
    '        BA_AddExtentLayer = return_code
    '        Exit Function

    'ErrorHandler:
    '        pFeatClass = Nothing
    '        pFLayer = Nothing
    '        pGFLayer = Nothing
    '        pWksFactory = Nothing
    '        pFeatWorkspace = Nothing
    '        pEnv = Nothing
    '        pTempLayer = Nothing
    '        pRenderer = Nothing
    '        BA_AddExtentLayer = return_code
    '        MsgBox(Err.Description)
    '        Err.Clear()
    '    End Function

    Public Sub BA_Reset_AOIFlags()
        'SelectAOI_Flag = False
        'SetPourPoint_Flag = False
        'GenerateAOI_Flag = False : Make sure this is the same as btnCreateAOI_flag
        'CreateAOIStream_Flag = False

        'ElevationScenario_Flag = False
        'ActualRepMap_Flag = False
        'PseudoRepMap_Flag = False

        Dim selectAOIButton = AddIn.FromID(Of BtnAOI_Tool)(My.ThisAddIn.IDs.BtnAOI_Tool)
        Dim setPourPointtool = AddIn.FromID(Of setPourPointtool)(My.ThisAddIn.IDs.setPourPointtool)
        Dim createAOIButton = AddIn.FromID(Of BtnCreateAOI)(My.ThisAddIn.IDs.BtnCreateAOI)
        Dim createAOIStreamButton = AddIn.FromID(Of BtnCreateAOIStream)(My.ThisAddIn.IDs.BtnCreateAOIStream)

        selectAOIButton.selectedProperty = False
        setPourPointtool.selectedProperty = False
        createAOIButton.selectedProperty = False
        createAOIStreamButton.selectedProperty = False

        Dim SiteScenarioButton = AddIn.FromID(Of BtnSiteScenario)(My.ThisAddIn.IDs.BtnSiteScenario)
        Dim SiteScenario1RepButton = AddIn.FromID(Of BtnScenario1)(My.ThisAddIn.IDs.BtnScenario1)
        Dim SiteScenario2RepButton = AddIn.FromID(Of BtnScenario2)(My.ThisAddIn.IDs.BtnScenario2)

        SiteScenarioButton.selectedProperty = False
        SiteScenario1RepButton.SelectedProperty = False
        SiteScenario2RepButton.SelectedProperty = False

        'ThisDocument.SelectedAOI.Text = ""
        'Dim cboSelectedBasin = AddIn.FromID(Of cboTargetedBasin)(My.ThisAddIn.IDs.cboTargetedBasin)
        'cboSelectedBasin.setValue("")

        'Disable AOI flags also disable all the map flags
        BA_Enable_MapFlags(False)
    End Sub

    Public Sub BA_GetRasterDimension(ByVal RasterFile_PathName As String, ByRef dem As BA_DEMInfo)
        'get raster information
        Dim filepath As String = ""
        Dim FileName As String

        If Len(RasterFile_PathName) = 0 Then 'not a valide input
            Exit Sub
        End If

        FileName = BA_GetBareName(RasterFile_PathName, filepath)

        On Error GoTo ErrorHandler

        Dim pDEUtility As IDEUtilities = New DEUtilities

        Dim pRasterDataset As IRasterDataset = New RasterDataset
        Dim pWS As IRasterWorkspace
        Dim pWSF As IWorkspaceFactory = New RasterWorkspaceFactory
        pWS = pWSF.OpenFromFile(filepath, 0)
        pRasterDataset = pWS.OpenRasterDataset(FileName)

        '2. QI Raster to IRasterBand
        Dim pRasterBand As IRasterBand
        Dim pRasterBandCollection As IRasterBandCollection
        pRasterBandCollection = pRasterDataset
        pRasterBand = pRasterBandCollection.Item(0)

        'QI IRasterProps
        Dim pRasterP As IRasterProps
        pRasterP = pRasterBand

        'get map extent coordinates
        Dim pRasterExt As IEnvelope
        pRasterExt = pRasterP.Extent
        pRasterExt.QueryCoords(dem.Min_MapX, dem.Min_MapY, dem.Max_MapX, dem.Max_MapY)

        'get cell size
        dem.Height_inPixels = pRasterP.Height
        dem.Width_inPixels = pRasterP.Width
        dem.X_CellSize = CDbl((dem.Max_MapX - dem.Min_MapX) / dem.Width_inPixels)
        dem.Y_CellSize = CDbl((dem.Max_MapY - dem.Min_MapY) / dem.Height_inPixels)

ErrorHandler:
        pRasterBand = Nothing
        pRasterBandCollection = Nothing
        pRasterDataset = Nothing
        pRasterP = Nothing
        pRasterExt = Nothing
        pWS = Nothing
        pWSF = Nothing
        pDEUtility.ReleaseInternals()
    End Sub

    'reset aoi selection and its associated functions
    Public Sub BA_ResetAOI()

        'Dim AOIInfoButton = AddIn.FromID(Of BtnAOIUtilities)(My.ThisAddIn.IDs.BtnAOIUtilities)
        'AOIInfoButton.selectedProperty = False
        AOIFolderBase = ""    'aoi folder base global variable

        'GenerateMaps_Flag = False
        'CreateAOIStream_Flag = False
        'ElevationScenario_Flag = False
        'ElevDistMap_Flag = False
        'ElevSNOTELMap_Flag = False
        ' ElevSnowCourseMap_Flag = False
        'PrecipDistMap_Flag = False
        'SlopeDistMap_Flag = False
        'AspectDistMap_Flag = False
        'ActualRepMap_Flag = False
        'PseudoRepMap_Flag = False
        Dim GenerateMapsButton = AddIn.FromID(Of BtnGenerateMaps)(My.ThisAddIn.IDs.BtnGenerateMaps)
        Dim createAOIStreamButton = AddIn.FromID(Of BtnCreateAOIStream)(My.ThisAddIn.IDs.BtnCreateAOIStream)
        Dim SiteScenarioButton = AddIn.FromID(Of BtnSiteScenario)(My.ThisAddIn.IDs.BtnSiteScenario)
        Dim ElevDistButton = AddIn.FromID(Of BtnElevationDist)(My.ThisAddIn.IDs.BtnElevationDist)
        Dim ElevSNOTEL = AddIn.FromID(Of BtnElevationSNOTEL)(My.ThisAddIn.IDs.BtnElevationSNOTEL)
        Dim ElevSnowCourseButton = AddIn.FromID(Of BtnElevSnowCourse)(My.ThisAddIn.IDs.BtnElevSnowCourse)
        Dim PrecipDistButton = AddIn.FromID(Of BtnPrecipitationDist)(My.ThisAddIn.IDs.BtnPrecipitationDist)
        Dim SlopeDistButton = AddIn.FromID(Of BtnSlopeDist)(My.ThisAddIn.IDs.BtnSlopeDist)
        Dim AspectDistButton = AddIn.FromID(Of BtnAspectDist)(My.ThisAddIn.IDs.BtnAspectDist)
        Dim SiteScenario1RepButton = AddIn.FromID(Of BtnScenario1)(My.ThisAddIn.IDs.BtnScenario1)
        Dim SiteScenario2RepButton = AddIn.FromID(Of BtnScenario2)(My.ThisAddIn.IDs.BtnScenario2)

        GenerateMapsButton.SelectedProperty = False
        createAOIStreamButton.selectedProperty = False
        SiteScenarioButton.selectedProperty = False
        ElevDistButton.SelectedProperty = False
        ElevSNOTEL.SelectedProperty = False
        ElevSnowCourseButton.SelectedProperty = False
        PrecipDistButton.SelectedProperty = False
        SlopeDistButton.SelectedProperty = False
        AspectDistButton.SelectedProperty = False
        SiteScenario1RepButton.SelectedProperty = False
        SiteScenario2RepButton.SelectedProperty = False

        'ThisDocument.SelectedAOI.Text = ""
        'Dim cboSelectedBasin = AddIn.FromID(Of cboTargetedBasin)(My.ThisAddIn.IDs.cboTargetedBasin)
        ' cboSelectedBasin.setValue("")  'aoi text on the toolbar
        Dim comboBox = AddIn.FromID(Of cboTargetedAOI)(My.ThisAddIn.IDs.cboTargetedAOI)
        If comboBox IsNot Nothing Then comboBox.setValue("")

        'Disable AOI flags also disable all the map flags
        BA_Enable_MapFlags(False)
    End Sub

    'this function will ensure BASIN clipped out of the DEM to follow cell boundaries in the DEM
    Public Sub BA_SnapGCElementtoRaster(ByRef pElement As IElement, ByVal dem As BA_DEMInfo, ByVal RasterFile_PathName As String)

        If pElement Is Nothing Then Exit Sub

        If dem.X_CellSize * dem.Y_CellSize = 0 Then
            BA_GetRasterDimension(RasterFile_PathName, dem)
            If dem.X_CellSize * dem.Y_CellSize = 0 Then Exit Sub
        End If

        On Error GoTo ErrorHandler

        'get the original extent coordinates
        Dim pElemExt As IEnvelope
        Dim ElemXMin As Double, ElemYMin As Double
        Dim ElemXMax As Double, ElemYMax As Double
        pElemExt = pElement.Geometry.Envelope
        pElemExt.QueryCoords(ElemXMin, ElemYMin, ElemXMax, ElemYMax)

        'adjust the coordinates by expanding the geometry to the boundaries of the closest raster cells
        ElemXMin = dem.Min_MapX + Fix((ElemXMin - dem.Min_MapX) / dem.X_CellSize) * dem.X_CellSize
        ElemXMax = dem.Min_MapX + (Fix((ElemXMax - dem.Min_MapX) / dem.X_CellSize) + 1) * dem.X_CellSize

        ElemYMin = dem.Min_MapY + Fix((ElemYMin - dem.Min_MapY) / dem.Y_CellSize) * dem.Y_CellSize
        ElemYMax = dem.Min_MapY + (Fix((ElemYMax - dem.Min_MapY) / dem.Y_CellSize) + 1) * dem.Y_CellSize

        '    Dim messagestring As String
        '    messagestring = "Cell size x, y = " & CellSize_X & ", " & CellSize_Y & vbCrLf
        '    messagestring = messagestring & "Original coord: " & ElemXMin & ", " & ElemYMin & ", " & ElemXMax & ", " & ElemYMax & vbCrLf
        '    messagestring = messagestring & "New coord: " & NewXMin & ", " & NewYMin & ", " & NewXMax & ", " & NewYMax
        '    MsgBox messagestring

        pElemExt.PutCoords(ElemXMin, ElemYMin, ElemXMax, ElemYMax)
        pElemExt.SnapToSpatialReference() 'final adjustment of the coordinates so that they match the precision of the data
        pElement.Geometry = pElemExt

        Exit Sub
ErrorHandler:
        MsgBox(Err.Description)
        Err.Clear()
    End Sub

    Public Function BA_Remove_Basin_Layers() As Integer
        'There are 7 layers to be removed
        'Basin DEM Extent, Filled DEM, Slope, Aspect, Flow Direction, and Flow Accumulation
        Dim pMxDoc As IMxDocument = My.ArcMap.Document
        Dim pMap As IMap = pMxDoc.FocusMap
        Dim layers_removed As Integer

        'check if a layer with the assigned name exists
        'search layer of the specified name, if found
        Dim nlayers As Long
        Dim i As Long, j As Long
        Dim pTempLayer As ILayer
        Dim LayerName As String

        layers_removed = 0
        nlayers = pMap.LayerCount
        For i = nlayers To 1 Step -1
            pTempLayer = pMap.Layer(i - 1)
            For j = 1 To 7
                LayerName = BasinLayerDisplayNames(j)
                If LayerName = pTempLayer.Name Then 'remove the layer
                    pMap.DeleteLayer(pTempLayer)
                    layers_removed = layers_removed + 1
                    Exit For
                End If
            Next
        Next

        'refresh map
        pMxDoc.UpdateContents()
        pMxDoc.ActivatedView.PartialRefresh(esriViewDrawPhase.esriViewForeground, Nothing, Nothing)
        BA_Remove_Basin_Layers = layers_removed
    End Function

    ' This procedure converts dem extent or pourpoint graphic elements (a polygon or a point) to a shapefile.
    'return value: 1 shapefile was created, 0 no shapefile was created
    Public Function BA_Graphic2FeatureClass(ByVal FolderPath As String, ByVal shapefilename As String) As Integer
        Dim inGeoType As Integer 'indicate the geometry type, point or polygon

        'MsgBox(FolderPath & vbCrLf & shapefilename)

        'Exit if there are no graphic element in the Activeview, exit the sub
        Dim pMxDoc As IMxDocument = My.ArcMap.Document
        Dim pMap As IMap = pMxDoc.FocusMap

        'Create graphics container and set to map document
        Dim pGC As IGraphicsContainer = pMap

        'Create element and assign it to the graphic element from other subroutine
        Dim pElem As IElement
        pGC.Reset()
        pElem = pGC.Next

        'Debug, if there are no graphics in the active view, then abort the process
        If pElem Is Nothing Then
            Return 0
        End If

        Dim pGeom As IGeometry = pElem.Geometry
        'Determine the geometry type
        Select Case pGeom.GeometryType
            Case esriGeometryType.esriGeometryPoint 'point
                inGeoType = esriGeometryType.esriGeometryPoint
            Case esriGeometryType.esriGeometryPolygon  'polygon
                inGeoType = esriGeometryType.esriGeometryPolygon
            Case Else
                Return 0 'other geometry types are not supported
        End Select

        Dim comReleaser As ESRI.ArcGIS.ADF.ComReleaser = New ESRI.ArcGIS.ADF.ComReleaser()
        ' Open the gdb to contain the shapefile as a workspace
        Dim pWorkspaceFactory As IWorkspaceFactory = New FileGDBWorkspaceFactory
        Dim pFWS As IFeatureWorkspace = pWorkspaceFactory.OpenFromFile(FolderPath, 0)
        Dim pFeatClass As IFeatureClass = Nothing
        Dim pFeat As IFeature = Nothing
        Dim pSpatialRef As ISpatialReference = Nothing

        Dim pFields As IFields = New Fields
        Dim pFieldsEdit As IFieldsEdit = Nothing

        Dim pField As IField = New Field
        Dim pFieldEdit As IFieldEdit = Nothing

        Dim pGeomDef As IGeometryDef = New GeometryDef
        Dim pGeomDefEdit As IGeometryDefEdit = Nothing

        Dim return_value As Integer = 0
        Try
            'create the shapefile
            'set spatial reference parameters
            pSpatialRef = pMxDoc.ActiveView.FocusMap.SpatialReference

            ' Set up fields
            pFieldsEdit = pFields

            ' Make the shape field
            ' it will need a geometry definition, with a spatial reference
            pFieldEdit = pField
            pFieldEdit.Name_2 = "Shape"
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry

            pGeomDefEdit = pGeomDef

            'options are esriGeometryPolygon or esriGeometryPoint
            pGeomDefEdit.GeometryType_2 = inGeoType
            pGeomDefEdit.SpatialReference_2 = pSpatialRef

            pFieldEdit.GeometryDef_2 = pGeomDef
            pFieldsEdit.AddField(pField)

            ' Add other miscellaneous fields if necessary
            'Set pField = New Field
            'Set pFieldEdit = pField
            'With pFieldEdit
            '.Length = 50
            '.Name = "Name"
            '.Type = esriFieldTypeString
            'End With
            'pFieldsEdit.AddField pField

            ' Create the shapefile
            pFeatClass = pFWS.CreateFeatureClass(shapefilename, pFields, Nothing, Nothing, esriFeatureType.esriFTSimple, "Shape", "")
            comReleaser.ManageLifetime(pFeatClass)

            'Add the elements
            pFeat = pFeatClass.CreateFeature
            pFeat.Shape = pGeom
            pFeat.Store()

            'pGC.DeleteAllElements
            return_value = 1

        Catch ex As Exception
            MsgBox("BA_Graphic2Shapefile Exception: " & ex.Message)

        Finally
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGC)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pField)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFieldEdit)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFields)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFieldsEdit)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeat)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeatClass)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFWS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWorkspaceFactory)
        End Try

        Return return_value
    End Function

    ' This procedure converts dem extent or pourpoint graphic elements (a polygon or a point) to a shapefile.
    'return value: 1 shapefile was created, 0 no shapefile was created
    Public Function BA_Graphic2Shapefile(ByVal FolderPath As String, ByVal shapefilename As String) As Integer
        Dim inGeoType As Integer 'indicate the geometry type, point or polygon

        Dim outputName As String = BA_StandardizeShapefileName(shapefilename, True, False)

        'MsgBox(FolderPath & vbCrLf & shapefilename)

        'Exit if there are no graphic element in the Activeview, exit the sub
        Dim pMxDoc As IMxDocument = My.ArcMap.Document
        Dim pMap As IMap = pMxDoc.FocusMap

        'Create graphics container and set to map document
        Dim pGC As IGraphicsContainer = pMap

        'Create element and assign it to the graphic element from other subroutine
        Dim pElem As IElement
        pGC.Reset()
        pElem = pGC.Next

        'Debug, if there are no graphics in the active view, then abort the process
        If pElem Is Nothing Then
            Return 0
        End If

        Dim pGeom As IGeometry = pElem.Geometry
        'Determine the geometry type
        Select Case pGeom.GeometryType
            Case esriGeometryType.esriGeometryPoint 'point
                inGeoType = esriGeometryType.esriGeometryPoint
            Case esriGeometryType.esriGeometryPolygon  'polygon
                inGeoType = esriGeometryType.esriGeometryPolygon
            Case Else
                Return 0 'other geometry types are not supported
        End Select

        Dim comReleaser As ESRI.ArcGIS.ADF.ComReleaser = New ESRI.ArcGIS.ADF.ComReleaser()
        ' Open the gdb to contain the shapefile as a workspace
        Dim pWorkspaceFactory As IWorkspaceFactory = New ShapefileWorkspaceFactory
        Dim pFWS As IFeatureWorkspace = pWorkspaceFactory.OpenFromFile(FolderPath, 0)
        Dim pFeatClass As IFeatureClass = Nothing
        Dim pFeat As IFeature = Nothing
        Dim pSpatialRef As ISpatialReference = Nothing

        Dim pFields As IFields = New Fields
        Dim pFieldsEdit As IFieldsEdit = Nothing

        Dim pField As IField = New Field
        Dim pFieldEdit As IFieldEdit = Nothing

        Dim pGeomDef As IGeometryDef = New GeometryDef
        Dim pGeomDefEdit As IGeometryDefEdit = Nothing

        Dim return_value As Integer = 0
        Try
            'create the shapefile
            'set spatial reference parameters
            pSpatialRef = pMxDoc.ActiveView.FocusMap.SpatialReference

            ' Set up fields
            pFieldsEdit = pFields

            ' Make the shape field
            ' it will need a geometry definition, with a spatial reference
            pFieldEdit = pField
            pFieldEdit.Name_2 = "Shape"
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry

            pGeomDefEdit = pGeomDef

            'options are esriGeometryPolygon or esriGeometryPoint
            pGeomDefEdit.GeometryType_2 = inGeoType
            pGeomDefEdit.SpatialReference_2 = pSpatialRef

            pFieldEdit.GeometryDef_2 = pGeomDef
            pFieldsEdit.AddField(pField)

            ' Add other miscellaneous fields if necessary
            'Set pField = New Field
            'Set pFieldEdit = pField
            'With pFieldEdit
            '.Length = 50
            '.Name = "Name"
            '.Type = esriFieldTypeString
            'End With
            'pFieldsEdit.AddField pField

            ' Create the shapefile
            pFeatClass = pFWS.CreateFeatureClass(shapefilename, pFields, Nothing, Nothing, esriFeatureType.esriFTSimple, "Shape", "")
            comReleaser.ManageLifetime(pFeatClass)

            'Add the elements
            pFeat = pFeatClass.CreateFeature
            pFeat.Shape = pGeom
            pFeat.Store()

            'pGC.DeleteAllElements
            return_value = 1

        Catch ex As Exception
            MsgBox("BA_Graphic2Shapefile Exception: " & ex.Message)

        Finally
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGC)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pField)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFieldEdit)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFields)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFieldsEdit)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeat)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeatClass)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFWS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWorkspaceFactory)
        End Try

        Return return_value
    End Function

    'return value:
    '   0: error occurred
    '   1: AOI attribute updated
    ' this function also works on BASIN
    Public Function BA_AddAOIVectorAttributes(ByVal gdbPath As String, ByVal Attribute_Value As String) As Integer
        'fields added
        'AOINAME - esriFieldTypeString: for labeling purpose
        'Dim pDEUtility As IDEUtilities = New DEUtilities
        Dim return_value As Integer = 0
        Dim Field_Value As String
        Dim StringLength As Integer = 40

        Attribute_Value = Trim(Attribute_Value)
        If Len(Attribute_Value) >= StringLength Then
            Field_Value = Left(Attribute_Value, StringLength)
        Else
            Field_Value = Attribute_Value
        End If

        Dim comReleaser As ESRI.ArcGIS.ADF.ComReleaser = New ESRI.ArcGIS.ADF.ComReleaser
        ' Open the folder to contain the shapefile as a workspace
        Dim pFClass As IFeatureClass = BA_OpenFeatureClassFromGDB(gdbPath, BA_AOIExtentCoverage) 'i.e., aoi_v
        comReleaser.ManageLifetime(pFClass)

        Dim pFCursor As IFeatureCursor = Nothing
        Dim pFeature As IFeature = Nothing

        Dim FieldIndex As Integer, FID_Index As Integer  'Ver1E Update - throughout this sub
        Dim pFld As IFieldEdit = New Field
        Dim FName As String, FIDName As String

        Try
            'add AOIName field
            ' check if field exist
            FName = "AOINAME"
            FieldIndex = pFClass.FindField(FName)

            ' Define field type
            If FieldIndex < 0 Then 'add field
                'Define field name
                pFld = New Field
                pFld.Name_2 = FName
                pFld.Type_2 = esriFieldType.esriFieldTypeString
                pFld.Length_2 = StringLength
                pFld.Required_2 = 0
                ' Add field
                pFClass.AddField(pFld)
            End If

            FIDName = BA_AOI_IDField
            FID_Index = pFClass.FindField(FIDName)

            'Ver1E Update - to handle the forecast point id
            ' Define field type
            If FID_Index < 0 Then 'add field
                'Define field name
                pFld = New Field
                pFld.Name_2 = FIDName
                pFld.Type_2 = esriFieldType.esriFieldTypeString
                pFld.Length_2 = 30
                pFld.Required_2 = 0
                ' Add field
                pFClass.AddField(pFld)
            End If

            Dim FBasinName As String, FB_Index As Integer
            FBasinName = "BASIN"
            FB_Index = pFClass.FindField(FBasinName)

            'Ver1E Update - to record basin name
            ' Define field type
            If FID_Index < 0 Then 'add field
                'Define field name
                pFld = New Field
                pFld.Name_2 = FBasinName
                pFld.Type_2 = esriFieldType.esriFieldTypeString
                pFld.Length_2 = 30
                pFld.Required_2 = 0
                ' Add field
                pFClass.AddField(pFld)
            End If

            'update value
            ' Get field index again
            Dim FI1 As Integer, FI2 As Integer, FI3 As Integer

            FI1 = pFClass.FindField(FName)
            FI2 = pFClass.FindField(FIDName)  'Ver1E Update
            FI3 = pFClass.FindField(FBasinName)  'Ver1E Update

            pFCursor = pFClass.Update(Nothing, False)
            comReleaser.ManageLifetime(pFCursor)
            pFeature = pFCursor.NextFeature

            Do While Not pFeature Is Nothing
                pFeature.Value(FI1) = Field_Value
                pFeature.Value(FI2) = BA_AOI_Forecast_ID 'Ver1E Update
                pFeature.Value(FI3) = BA_GetBareName(BasinFolderBase)
                pFCursor.UpdateFeature(pFeature)
                pFeature = pFCursor.NextFeature
                return_value = return_value + 1
            Loop
            Return return_value

        Catch ex As Exception
            MsgBox("BA_AddAOIVectorAttributes Exception: " & ex.Message)
            Return return_value

        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFld)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeature)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFClass)
            GC.WaitForPendingFinalizers()
            GC.Collect()

        End Try
    End Function

    Public Sub ClearMap()
        'Dim map document
        Dim pMxDoc As IMxDocument = My.ArcMap.Document
        'Make Graphics container
        Dim pGC As IGraphicsContainer
        pGC = pMxDoc.ActivatedView
        'Select graphic elements
        pGC.Next()
        'Delete all elements
        pGC.DeleteAllElements()
        'Refresh map view
        pMxDoc.ActivatedView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)
        System.Windows.Forms.Application.DoEvents()
    End Sub

    ''clip pRaster by pElem and return a RasterDataset
    'Public Function ClipRaster(ByVal pRaster As IGeoDataset, ByVal pEnv As IEnvelope) As IGeoDataset

    '    'Create envelope to hold graphic's geometry
    '    'Dim pEnv As IEnvelope
    '    If pEnv Is Nothing Then 'no clip rectangle is specified
    '        Return Nothing
    '    End If

    '    Dim pClippedRaster As IGeoDataset = Nothing
    '    Dim pTransformationOp As ITransformationOp = New RasterTransformationOp
    '    Try
    '        pClippedRaster = pTransformationOp.Clip(pRaster, pEnv)
    '        System.Windows.Forms.Application.DoEvents()

    '    Catch ex As Exception
    '        MsgBox("ClipRaster Exception: " & ex.Message)

    '    Finally
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTransformationOp)

    '    End Try

    '    Return pClippedRaster
    'End Function

    Public Function Smooth(ByVal pRaster As IGeoDataset2, height As Double, width As Double) As IGeoDataset2
        ' Declare Variables
        Dim pNBOp As INeighborhoodOp = New RasterNeighborhoodOp
        Dim pSmooth As IGeoDataset2 = Nothing
        Dim pNB As IRasterNeighborhood = New RasterNeighborhood

        pNB.SetRectangle(width, height, ESRI.ArcGIS.GeoAnalyst.esriGeoAnalysisUnitsEnum.esriUnitsCells)

        Try
            ' Create HydrologyOP and Complete Function
            pSmooth = pNBOp.FocalStatistics(pRaster, ESRI.ArcGIS.GeoAnalyst.esriGeoAnalysisStatisticsEnum.esriGeoAnalysisStatsMean, pNB, True)

        Catch ex As Exception
            MsgBox("Smooth Exception: " & ex.Message)

        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pNB)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pNBOp)
        End Try

        Return pSmooth
    End Function

    Public Function Fill(ByVal pRaster As IGeoDataset2) As IGeoDataset2
        ' Declare Variables
        Dim pHydrologyOp As IHydrologyOp = New RasterHydrologyOp
        Dim pFill As IGeoDataset2 = Nothing

        Try
            ' Create HydrologyOP and Complete Function
            pFill = pHydrologyOp.Fill(pRaster, Nothing)

        Catch ex As Exception
            MsgBox("Fill Exception: " & ex.Message)

        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pHydrologyOp)
        End Try

        Return pFill
    End Function

    Public Function Slope(ByVal pRaster As IGeoDataset2) As IGeoDataset2
        ' Declare Variables
        Dim pSurfaceOp As ISurfaceOp = New RasterSurfaceOp
        Dim pSlope As IGeoDataset2 = Nothing

        'The XY unit of the DEM has to be in meters
        'Determine if the Z-Unit and XY-Unit are the same or different.
        ' If frmSettings.OptMeter.Value = True Then
        If BA_SystemSettings.DEM_ZUnit_IsMeter = True Then
            'The Z-Unit is the same as the XY-Unit, no Z-Factor needed.
            pSlope = pSurfaceOp.Slope(pRaster, esriGeoAnalysisSlopeEnum.esriGeoAnalysisSlopePercentrise, 1)
            ' ElseIf frmSettings.OptFoot.Value = True Then
        ElseIf BA_SystemSettings.DEM_ZUnit_IsMeter = False Then
            'The Z-Unit differs from the XY-Unit, a Z-Factor of 0.3048 is used to convert the Z-Unit from Feet to Meters.
            pSlope = pSurfaceOp.Slope(pRaster, esriGeoAnalysisSlopeEnum.esriGeoAnalysisSlopePercentrise, 0.3048)
        Else
            MsgBox("Please set the Elevation Unit in the Basin Analyst Options.")
        End If
        System.Windows.Forms.Application.DoEvents()
        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pSurfaceOp)
        Return pSlope
    End Function

    Public Function Aspect(ByVal pRaster As IGeoDataset2) As IGeoDataset2
        ' Declare Variables
        Dim pSurfaceOp As ISurfaceOp = New RasterSurfaceOp
        Dim pAspect As IGeoDataset2 = Nothing

        Try
            ' Create HydrologyOP and Complete Function
            pAspect = pSurfaceOp.Aspect(pRaster)
            System.Windows.Forms.Application.DoEvents()

        Catch ex As Exception
            MsgBox("Aspect Exception: " & ex.Message)

        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pSurfaceOp)
        End Try

        Return pAspect
    End Function

    Public Function FlowDirection(ByVal pRaster As IGeoDataset2) As IGeoDataset2
        ' Declare Variables
        Dim pHydrologyOp As IHydrologyOp = New RasterHydrologyOp
        Dim pFlowDir As IGeoDataset2 = Nothing

        Try
            pFlowDir = pHydrologyOp.FlowDirection(pRaster, False, True)
            System.Windows.Forms.Application.DoEvents()

        Catch ex As Exception
            MsgBox("FlowDirection Exception: " & ex.Message)

        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pHydrologyOp)
        End Try

        Return pFlowDir
    End Function

    Public Function FlowAccumulation(ByVal pRaster As IGeoDataset2) As IGeoDataset2

        ' Declare Variables
        Dim pHydrologyOp As IHydrologyOp = New RasterHydrologyOp
        Dim pFlowAcc As IGeoDataset2 = Nothing

        Try
            pFlowAcc = pHydrologyOp.FlowAccumulation(pRaster)
            System.Windows.Forms.Application.DoEvents()

        Catch ex As Exception
            MsgBox("FlowAccumulation Exception: " & ex.Message)

        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pHydrologyOp)
        End Try

        Return pFlowAcc
    End Function

    Public Function Hillshade(ByVal pRaster As IGeoDataset2, ByVal ZFactor As Double) As IGeoDataset2
        ' Declare Variables
        Dim pSurfaceOp As ISurfaceOp = New RasterSurfaceOp
        Dim pHillshade As IGeoDataset2 = Nothing

        ' Create SurfaceOP and Complete Function
        If ZFactor = 0 Then ZFactor = 1

        Try
            pHillshade = pSurfaceOp.HillShade(pRaster, 315, 30, True, ZFactor)
            System.Windows.Forms.Application.DoEvents()

        Catch ex As Exception
            MsgBox("Hillshade Exception: " & ex.Message)

        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pSurfaceOp)
        End Try

        Return pHillshade
    End Function

    Public Function BA_ReplaceCharacters(ByVal instring As String, ByVal findchar As String, ByVal replacechar As String, ByRef outstring As String) As Integer
        Dim charpos As Integer
        Dim icount As Integer
        Dim findlen As Integer
        Dim replacelen As Integer
        Dim inlen As Integer

        findlen = Len(findchar)
        replacelen = Len(replacechar)
        inlen = Len(instring)

        If inlen * findlen * replacelen = 0 Then
            BA_ReplaceCharacters = -1
            Exit Function
        End If

        outstring = instring
        charpos = InStr(1, outstring, findchar, vbTextCompare)
        icount = 0
        Do While charpos > 0
            outstring = Left(outstring, charpos - 1) & replacechar & Right(outstring, (Len(outstring) - charpos - findlen + 1))
            charpos = InStr(1, outstring, findchar, vbTextCompare)
            icount = icount + 1
        Loop
        outstring = outstring
        BA_ReplaceCharacters = icount
    End Function

    Public Function BA_GetShapeGeometry(ByVal shapefile_pathname As String) As IGeometry
        Dim filepath As String = ""
        Dim FileName As String

        If Len(shapefile_pathname) = 0 Then 'not a valide input
            Return Nothing
        End If

        FileName = BA_GetBareName(shapefile_pathname, filepath)

        Dim pFeatureClass As IFeatureClass = Nothing
        Dim pCursor As IFeatureCursor = Nothing
        Dim pFeature As IFeature = Nothing

        Try
            'Get shape FeatureClass
            pFeatureClass = BA_OpenFeatureClassFromGDB(filepath, FileName)

            'Extract Area
            pCursor = pFeatureClass.Search(Nothing, False)
            pFeature = pCursor.NextFeature
            pFeatureClass = Nothing
            pCursor = Nothing

            If Not pFeature Is Nothing Then
                Return pFeature.Shape
            Else
                Return Nothing
            End If

        Catch ex As Exception
            pFeatureClass = Nothing
            pFeature = Nothing
            pCursor = Nothing
            Return Nothing
        End Try
    End Function

    Public Sub BA_Feature2Raster(ByVal InputFeatureClass As IFeatureClass, ByVal Save2Path As String, ByVal SaveName As String, ByVal Cellsize As Object)
        Dim pWS As IWorkspace
        Dim pWSF As IWorkspaceFactory
        Dim pConversionOp As IConversionOp
        Dim pEnv As IRasterAnalysisEnvironment
        Dim pRDS As IRasterDataset

        On Error GoTo ErrorHandler
        pWSF = New RasterWorkspaceFactory
        pWS = pWSF.OpenFromFile(Save2Path, 0)
        pConversionOp = New RasterConversionOp
        pEnv = pConversionOp
        pEnv.SetCellSize(esriRasterEnvSettingEnum.esriRasterEnvValue, Cellsize)
        pRDS = pConversionOp.ToRasterDataset(InputFeatureClass, "GRID", pWS, SaveName)
        pWS = Nothing
        pWSF = Nothing
        pConversionOp = Nothing
        pEnv = Nothing
        pRDS = Nothing
        Exit Sub

ErrorHandler:
        pWS = Nothing
        pWSF = Nothing
        pConversionOp = Nothing
        pEnv = Nothing
        pRDS = Nothing
    End Sub

    'return the datum string of a GIS dataset
    Public Function BA_GetProjectionString(ByVal layerPathName As String) As String
        Dim parentPath As String = "PleaseReturn"
        Dim fileName As String = BA_GetBareName(layerPathName, parentPath)
        Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(layerPathName)
        Dim pGeoDataset As IGeoDataset = Nothing
        Dim pSpRef As ISpatialReference = Nothing
        Try
            'Open the dataset to extract the spatial reference
            If workspaceType = workspaceType.Raster Then
                If layerPathName.Substring(layerPathName.Length - 3, 3).ToUpper = "SHP" Then 'input is a shapefile
                    pGeoDataset = BA_OpenFeatureClassFromFile(parentPath, BA_StandardizeShapefileName(fileName, False, False))
                Else 'input is a grid
                    pGeoDataset = BA_OpenRasterFromFile(parentPath, fileName)
                End If

            ElseIf workspaceType = workspaceType.Geodatabase Then 'input is in a fgdb
                Dim pWSFactory As IWorkspaceFactory = New FileGDBWorkspaceFactory()
                Dim pWS As IWorkspace2 = Nothing
                pWS = pWSFactory.OpenFromFile(parentPath, 0)

                If pWS.NameExists(esriDatasetType.esriDTRasterDataset, fileName) Then 'raster in FGDB
                    pGeoDataset = BA_OpenRasterFromGDB(parentPath, fileName)
                ElseIf pWS.NameExists(esriDatasetType.esriDTFeatureClass, fileName) Then 'featureclass in FGDB
                    pGeoDataset = BA_OpenFeatureClassFromGDB(parentPath, fileName)
                Else
                    pGeoDataset = Nothing
                End If

                pWS = Nothing
                pWSFactory = Nothing
            End If

            If pGeoDataset IsNot Nothing Then
                'Spatial reference for the dataset in question
                pSpRef = pGeoDataset.SpatialReference
                'Extract datum string from spatial reference
                If pSpRef IsNot Nothing Then
                    ' Explicit cast
                    Dim parameterExport1 As IESRISpatialReferenceGEN2 = CType(pSpRef, IESRISpatialReferenceGEN2)
                    Dim buffer As String = Nothing
                    Dim bytes As Long = Nothing
                    parameterExport1.ExportToESRISpatialReference2(buffer, bytes)
                    Dim datumPos As Integer = InStr(buffer, "PROJCS")
                    Dim primePos As Integer = InStr(buffer, "GEOGCS")
                    Return buffer.Substring(datumPos + 7, primePos - datumPos - 10)
                End If
            End If
            Return Nothing
        Catch ex As Exception
            MessageBox.Show("BA_GetDatumString Exception: " & ex.Message)
            Return Nothing
        Finally
            pGeoDataset = Nothing
            pSpRef = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    '    Public Function OpenRasterDataset(ByVal spath As String, ByVal sFileName As String) As IGeoDataset
    '        On Error GoTo ErrorHandler

    '        ' Create RasterWorkSpaceFactory
    '        Dim pWSF As IWorkspaceFactory
    '        pWSF = New RasterWorkspaceFactory
    '        Dim pRDataset As IRasterDataset = Nothing

    '        ' Get RasterWorkspace
    '        Dim pRasWS As IRasterWorkspace
    '        If pWSF.IsWorkspace(spath) Then
    '            pRasWS = pWSF.OpenFromFile(spath, 0)
    '            pRDataset = pRasWS.OpenRasterDataset(sFileName)
    '        End If

    '        OpenRasterDataset = New RasterDataset
    '        OpenRasterDataset = pRDataset

    '        ' Release memeory
    '        pRasWS = Nothing
    '        pWSF = Nothing
    '        Exit Function
    'ErrorHandler:
    '        OpenRasterDataset = Nothing
    '        MsgBox("Unable to open " & spath & "\" & sFileName)
    '    End Function

    Public Function BA_GetArcGISVersion() As String
        Dim success As Boolean = ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.Desktop)

        If success = True Then
            Dim activeRuntimeInfo As ESRI.ArcGIS.RuntimeInfo = ESRI.ArcGIS.RuntimeManager.ActiveRuntime
            Return activeRuntimeInfo.Version
        Else
            Return Nothing
        End If
    End Function
    ' ''' Constant for the dwDesiredAccess parameter of the OpenProcess API function.
    'Private Const PROCESS_QUERY_INFORMATION As Long = &H400
    ' ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ' ''' Comments:   Shells out to the specified command line and waits for it to
    ' '''             complete. The Shell function runs asynchronously, so you must
    ' '''             run it using this function if you need to do something with
    ' '''             its output or wait for it to finish before continuing.
    ' '''
    ' ''' Arguments:  szCommandLine   [in] The command line to execute using Shell.
    ' '''             iWindowState    [in] (Optional) The window state parameter to
    ' '''                             pass to the Shell function. Default = vbHide.
    ' '''
    ' ''' Returns:    Boolean         True on success, False on error.
    ' ''' 05/19/05    Rob Bovey       Created
    ' '''
    '    Public Function bShellAndWait(ByVal szCommandLine As String, Optional ByVal iWindowState As Integer = vbHide) As Boolean

    '        Dim lTaskID As Long
    '        Dim lProcess As Long
    '        Dim lExitCode As Long
    '        Dim lResult As Long

    '        On Error GoTo ErrorHandler

    '        ' Run the Shell function.
    '        lTaskID = Shell(szCommandLine, iWindowState)

    '        ' Check for errors.
    '        If lTaskID = 0 Then Err.Raise(9999, , "Shell function error.")

    '        ' Get the process handle from the task ID returned by Shell.
    '        lProcess = OpenProcess(PROCESS_QUERY_INFORMATION, 0&, lTaskID)

    '        ' Check for errors.
    '        If lProcess = 0 Then Err.Raise(9999, , "Unable to open Shell process handle.")

    '        ' Loop while the shelled process is still running.
    '        Do
    '            ' lExitCode will be set to STILL_ACTIVE as long as the shelled process is running.
    '            lResult = GetExitCodeProcess(lProcess, lExitCode)
    '            DoEvents()
    '        Loop While lExitCode = STILL_ACTIVE

    '        bShellAndWait = True
    '        Exit Function

    'ErrorHandler:
    '        gszErrMsg = Err.Description
    '        bShellAndWait = False
    '    End Function

End Module