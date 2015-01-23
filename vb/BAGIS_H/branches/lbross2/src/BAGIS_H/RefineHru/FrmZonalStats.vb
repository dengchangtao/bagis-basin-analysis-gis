Imports ESRI.ArcGIS.Catalog
Imports System.Windows.Forms
Imports ESRI.ArcGIS.CatalogUI
Imports System.IO
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Geodatabase
Imports BAGIS_ClassLibrary

''' <summary>
''' Designer class of the dockable window add-in. It contains user interfaces that
''' make up the dockable window.
''' </summary>
Public Class FrmZonalStats
    Dim m_aoi As Aoi
    Dim m_version As String
    Dim m_lstHruLayersItem As LayerListItem = Nothing
    Dim m_lstRasterItem As LayerListItem = Nothing
    Dim m_lstPrismItem As LayerListItem = Nothing
    Dim m_lstDemItem As LayerListItem = Nothing

    Public Sub New(ByVal hook As Object)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Hook = hook

        ResetForm()
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

        Private m_windowUI As FrmZonalStats

        Protected Overrides Function OnCreateChild() As System.IntPtr
            m_windowUI = New FrmZonalStats(Me.Hook)
            Return m_windowUI.Handle
        End Function

        Protected Overrides Sub Dispose(ByVal Param As Boolean)
            If m_windowUI IsNot Nothing Then
                m_windowUI.Dispose(Param)
            End If

            MyBase.Dispose(Param)
        End Sub
        ' This property allows other forms access to the UI of this form
        Protected Friend ReadOnly Property UI() As FrmZonalStats
            Get
                Return m_windowUI
            End Get
        End Property
    End Class

    Public Sub ResetForm()
        LstSelectHruLayers.Items.Clear()
        LstAttributeField.Items.Clear()
        LstDemLayers.Items.Clear()
        LstPrismLayers.Items.Clear()
        LstAoiRasterLayers.Items.Clear()
        BtnStats.Enabled = False
        Btnclear.Enabled = False
        BtnViewAoi.Enabled = False
        BtnClearSelected.Enabled = False
        ChkMean.Checked = False
        ChkRange.Checked = False
        ChkSTD.Checked = False
        ChkSum.Checked = False
        ChkVariety.Checked = False
        ChkMinority.Checked = False
        ChkMajority.Checked = False
        ChkMax.Checked = False
        ChkMedian.Checked = False
        ChkMin.Checked = False
        Panel1.Enabled = False
        TxtAoiPath.Text = "AOI is not specified"
    End Sub

    Private Sub ResetFormAfterStat()
        LstAttributeField.Items.Clear()
        LstSelectHruLayers.SelectedIndex = -1
        LstDemLayers.SelectedIndex = -1
        LstPrismLayers.SelectedIndex = -1
        LstAoiRasterLayers.SelectedIndex = -1
        BtnStats.Enabled = False
        Btnclear.Enabled = False
        BtnViewAoi.Enabled = False
        BtnClearSelected.Enabled = False
        ChkMean.Checked = False
        ChkRange.Checked = False
        ChkSTD.Checked = False
        ChkSum.Checked = False
        ChkVariety.Checked = False
        ChkMinority.Checked = False
        ChkMajority.Checked = False
        ChkMax.Checked = False
        ChkMedian.Checked = False
        ChkMin.Checked = False
        Panel1.Enabled = False
    End Sub

    Private Sub BtnAOI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAOI.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim DataPath As String
        Dim pFilter As IGxObjectFilter = New GxFilterContainers

        Dim hruExt As HruExtension = HruExtension.GetExtension
        m_version = hruExt.version

        Try
            'initialize and open mini browser
            With pGxDialog
                .AllowMultiSelect = False
                .ButtonCaption = "Select"
                .Title = "Select AOI Folder"
                .ObjectFilter = pFilter
                bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
            End With

            If bObjectSelected = False Then Exit Sub

            'get the name of the selected folder
            Dim pGxDataFolder As IGxFile
            pGxDataFolder = pGxObject.Next
            DataPath = pGxDataFolder.Path
            If String.IsNullOrEmpty(DataPath) Then Exit Sub 'user cancelled the action

            'check AOI/BASIN status
            Dim success As BA_ReturnCode = BA_CheckAoiStatus(DataPath, My.ArcMap.Application.hWnd, My.ArcMap.Document)
            If success = BA_ReturnCode.Success Then
                BA_SetDefaultProjection(My.ArcMap.Application)
                Dim aoiName As String = BA_GetBareName(DataPath)
                m_aoi = New Aoi(aoiName, DataPath, Nothing, m_version)
                ResetForm()
                LoadHruLayers()
                LoadLstLayers()
                Show()
            End If
        Catch ex As Exception
            MessageBox.Show("COMException: " & ex.Message)
        End Try
    End Sub

    Public Sub LoadHruLayers(Optional ByVal aoi As Aoi = Nothing)
        If aoi IsNot Nothing Then m_aoi = aoi

        ' Create a DirectoryInfo of the HRU directory.
        Dim zonesDirectory As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, Nothing)
        Dim dirZones As New DirectoryInfo(zonesDirectory)
        TxtAoiPath.Text = m_aoi.FilePath

        If dirZones.Exists Then
            Dim dirZonesArr As DirectoryInfo() = dirZones.GetDirectories
            Dim item As LayerListItem

            ' Create/configure a step progressor
            Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, dirZonesArr.Length)
            ' Create/configure the ProgressDialog. This automatically displays the dialog
            Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Loading HRU list", "Loading...")
            pStepProg.Show()
            progressDialog2.ShowDialog()

            pStepProg.Step()
            LstSelectHruLayers.Items.Clear()

            For Each dri In dirZonesArr
                Dim hruFilePath As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, dri.Name) & BA_EnumDescription(PublicPath.HruGrid)
                Dim hruXmlFilePath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, dri.Name) & BA_EnumDescription(PublicPath.HruXml)
                ' Add hru to the list if the grid exists
                If BA_File_Exists(hruFilePath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) And _
                    BA_File_ExistsWindowsIO(hruXmlFilePath) Then
                    'Assume hru is discrete raster since we create it to be so
                    item = New LayerListItem(dri.Name, hruFilePath, LayerType.Raster, True)
                    LstSelectHruLayers.Items.Add(item)
                End If
                pStepProg.Step()
            Next dri

            pStepProg.Hide()
            pStepProg = Nothing
            progressDialog2.HideDialog()
            progressDialog2 = Nothing
        End If

        If LstSelectHruLayers.Items.Count = 0 Then
            'Pop error message if no zones in this aoi If CboParentHru.Items.Count < 1 Then
            MessageBox.Show("No HRU datasets have been generated for this AOI. Use the 'Define Zones' tool to create an HRU dataset.", "Missing HRU datasets", MessageBoxButtons.OK, MessageBoxIcon.Information)
            'MessageBox.Show("The AOI does not have any HRU layer. The tool can only be applied on HRU layers. Please use the Define HRU tool to create HRU layers first.", "Load HRU Layers")
        End If
    End Sub

    Public Sub LoadLstLayers(Optional ByVal aoi As Aoi = Nothing)
        Dim RasterCount As Long
        Dim i As Long

        If aoi IsNot Nothing Then m_aoi = aoi

        ' Get the count of zone directories so we know how many steps to put into the StepProgressor
        ' Create a DirectoryInfo of the HRU directory.
        Dim zonesDirectory As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, Nothing)
        Dim dirZones As New DirectoryInfo(zonesDirectory)
        Dim dirZonesArr As DirectoryInfo() = Nothing
        Dim zoneCount As Integer
        If dirZones.Exists Then
            dirZonesArr = dirZones.GetDirectories
            If dirZonesArr IsNot Nothing Then zoneCount = dirZonesArr.Length
        End If

        ' Create/configure a step progressor
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 8 + zoneCount)
        pStepProg.Show()
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Loading layer lists", "Loading...")

        Try
            LstAoiRasterLayers.Items.Clear()
            LstDemLayers.Items.Clear()
            LstPrismLayers.Items.Clear()
            progressDialog2.ShowDialog()
            pStepProg.Step()
            Dim AOIVectorList() As String = Nothing
            Dim AOIRasterList() As String = Nothing
            Dim layerPath As String = m_aoi.FilePath & "\" & BA_EnumDescription(GeodatabaseNames.Layers)
            BA_ListLayersinGDB(layerPath, AOIRasterList, AOIVectorList)
            pStepProg.Step()

            'display raster layers
            RasterCount = UBound(AOIRasterList)
            If RasterCount > 0 Then
                For i = 1 To RasterCount
                    Dim fullLayerPath As String = layerPath & "\" & AOIRasterList(i)
                    Dim isDiscrete As Boolean = BA_IsIntegerRasterGDB(fullLayerPath)
                    Dim item As LayerListItem = New LayerListItem(AOIRasterList(i), fullLayerPath, LayerType.Raster, isDiscrete)
                    LstAoiRasterLayers.Items.Add(item)
                Next
            End If
            pStepProg.Step()

            'display dem layers
            LoadDemLayers()
            pStepProg.Step()

            'display prism layers
            LoadPrismLayers()
            pStepProg.Step()

        Finally
            ' Clean up step progressor
            pStepProg.Hide()
            pStepProg = Nothing
            progressDialog2.HideDialog()
            progressDialog2 = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try

    End Sub

    Private Sub LoadDemLayers()
        Dim layerPath As String = m_aoi.FilePath & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces)
        Dim item As LayerListItem
        ' Filled DEM
        Dim fullLayerPath As String = layerPath & "\" & BA_EnumDescription(MapsFileName.filled_dem_gdb)
        Dim isDiscrete As Boolean = BA_IsIntegerRaster(fullLayerPath)
        If BA_File_Exists(fullLayerPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            item = New LayerListItem(BA_EnumDescription(MapsLayerName.filled_dem), fullLayerPath, LayerType.Raster, isDiscrete)
            LstDemLayers.Items.Add(item)
        End If
        ' Original DEM
        Dim demFileName As String = BA_GetBareName(BA_EnumDescription(PublicPath.SourceDEM))
        fullLayerPath = layerPath & "\" & demFileName
        isDiscrete = BA_IsIntegerRaster(fullLayerPath)
        If BA_File_Exists(fullLayerPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            item = New LayerListItem(BA_EnumDescription(MapsLayerName.dem), fullLayerPath, LayerType.Raster, isDiscrete)
            LstDemLayers.Items.Add(item)
        End If
        ' Flow Direction
        fullLayerPath = layerPath & "\" & BA_EnumDescription(MapsFileName.flow_direction_gdb)
        isDiscrete = BA_IsIntegerRaster(fullLayerPath)
        If BA_File_Exists(fullLayerPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            item = New LayerListItem(BA_EnumDescription(MapsLayerName.flow_direction_gdb), fullLayerPath, LayerType.Raster, isDiscrete)
            LstDemLayers.Items.Add(item)
        End If
        ' Flow Accumulation
        fullLayerPath = layerPath & "\" & BA_EnumDescription(MapsFileName.flow_accumulation_gdb)
        isDiscrete = BA_IsIntegerRaster(fullLayerPath)
        If BA_File_Exists(fullLayerPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            item = New LayerListItem(BA_EnumDescription(MapsLayerName.flow_accumulation_gdb), fullLayerPath, LayerType.Raster, isDiscrete)
            LstDemLayers.Items.Add(item)
        End If
        ' Slope
        fullLayerPath = layerPath & "\" & BA_EnumDescription(MapsLayerName.slope)
        isDiscrete = BA_IsIntegerRaster(fullLayerPath)
        If BA_File_Exists(fullLayerPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            item = New LayerListItem(BA_EnumDescription(MapsLayerName.slope), fullLayerPath, LayerType.Raster, isDiscrete)
            LstDemLayers.Items.Add(item)
        End If
        ' Aspect
        fullLayerPath = layerPath & "\" & BA_EnumDescription(MapsLayerName.aspect)
        isDiscrete = BA_IsIntegerRaster(fullLayerPath)
        If BA_File_Exists(fullLayerPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            item = New LayerListItem(BA_EnumDescription(MapsLayerName.aspect), fullLayerPath, LayerType.Raster, isDiscrete)
            LstDemLayers.Items.Add(item)
        End If
        ' Hillshade
        fullLayerPath = layerPath & "\" & BA_EnumDescription(MapsLayerName.hillshade)
        isDiscrete = BA_IsIntegerRaster(fullLayerPath)
        If BA_File_Exists(fullLayerPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            item = New LayerListItem(BA_EnumDescription(MapsLayerName.hillshade), fullLayerPath, LayerType.Raster, isDiscrete)
            LstDemLayers.Items.Add(item)
        End If

    End Sub

    Private Sub LoadPrismLayers()
        Dim item As LayerListItem
        Dim strPath As String = m_aoi.FilePath & "\" & BA_EnumDescription(GeodatabaseNames.Prism)
        Dim layerCount As Short = [Enum].GetValues(GetType(AOIPrismFolderNames)).Length
        Dim i = 1
        Do Until i > layerCount
            Dim nextFolder As String = BA_GetPrismFolderName(i)
            Dim prismPath = strPath & "\" & nextFolder
            'Dim isDiscrete As Boolean = BA_IsIntegerRaster(prismPath)
            'For performance, we assume that prism layers are not discrete
            If BA_File_Exists(prismPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                item = New LayerListItem(nextFolder, prismPath, LayerType.Raster, False)
                LstPrismLayers.Items.Add(item)
            End If
            i += 1
        Loop
    End Sub

    Private Sub LstSelectHruLayers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstSelectHruLayers.SelectedIndexChanged
        ' unselect previous selected item
        If m_lstHruLayersItem IsNot Nothing Then
            LstSelectHruLayers.SelectedItems.Remove(m_lstHruLayersItem)
            LstAttributeField.Items.Clear()
        End If

        ' reset selected index to new value
        m_lstHruLayersItem = LstSelectHruLayers.SelectedItem

        If m_lstHruLayersItem Is Nothing Then
            'MessageBox.Show("No HRU Layer is selected.")
            Exit Sub
        End If

        Dim hruPath As String = ""
        Dim rasterName As String = BA_GetBareName(m_lstHruLayersItem.Value, hruPath)
        hruPath = BA_StandardizePathString(hruPath, False)
        Dim vecWPath As String = hruPath & BA_EnumDescription(PublicPath.HruVector)
        Dim vecPath As String = ""
        Dim vecName As String = BA_GetBareName(vecWPath, vecPath)
        vecPath = BA_StandardizePathString(vecPath, False)
        vecName = BA_StandardizeShapefileName(vecName, False, False)
        Dim vrFeatureClass As IFeatureClass = BA_OpenFeatureClassFromGDB(vecPath, vecName)
        If vrFeatureClass Is Nothing Then
            Throw New Exception("Feature Class Not Found.")
        End If

        ' Cycle through all fields and add to the list
        For pIndex As Integer = 0 To vrFeatureClass.Fields.FieldCount - 1
            Dim pField As IField = vrFeatureClass.Fields.Field(pIndex)
            LstAttributeField.Items.Add(CStr(pField.Name))
        Next
        LstAttributeField.SelectionMode = SelectionMode.None
        Btnclear.Enabled = True
        ManageAoiLayerButtons()
    End Sub

    Private Sub Btnclear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btnclear.Click
        LstSelectHruLayers.SelectedIndex = -1
        'LstAttributeField.SelectedIndex = -1
    End Sub

    Private Sub LstAoiRasterLayers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstAoiRasterLayers.SelectedIndexChanged
        '' unselect previous selected item and reset selected index to new value
        'If m_lstRasterItem IsNot Nothing Then
        '    LstAoiRasterLayers.SelectedItems.Remove(m_lstRasterItem)
        'End If
        'If m_lstDemItem IsNot Nothing Then
        '    LstDemLayers.SelectedItems.Remove(m_lstDemItem)
        'End If
        'If m_lstPrismItem IsNot Nothing Then
        '    LstPrismLayers.SelectedItems.Remove(m_lstPrismItem)
        'End If
        'm_lstRasterItem = LstAoiRasterLayers.SelectedItem
        ManageAoiLayerButtons()
    End Sub

    Private Sub LstDemLayers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstDemLayers.SelectedIndexChanged
        '' unselect previous selected item and reset selected index to new value
        'If m_lstRasterItem IsNot Nothing Then
        '    LstAoiRasterLayers.SelectedItems.Remove(m_lstRasterItem)
        'End If
        'If m_lstDemItem IsNot Nothing Then
        '    LstDemLayers.SelectedItems.Remove(m_lstDemItem)
        'End If
        'If m_lstPrismItem IsNot Nothing Then
        '    LstPrismLayers.SelectedItems.Remove(m_lstPrismItem)
        'End If
        'm_lstDemItem = LstDemLayers.SelectedItem
        ManageAoiLayerButtons()
    End Sub

    Private Sub LstPrismLayers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstPrismLayers.SelectedIndexChanged
        '' unselect previous selected item and reset selected index to new value
        'If m_lstRasterItem IsNot Nothing Then
        '    LstAoiRasterLayers.SelectedItems.Remove(m_lstRasterItem)
        'End If
        'If m_lstDemItem IsNot Nothing Then
        '    LstDemLayers.SelectedItems.Remove(m_lstDemItem)
        'End If
        'If m_lstPrismItem IsNot Nothing Then
        '    LstPrismLayers.SelectedItems.Remove(m_lstPrismItem)
        'End If
        'm_lstPrismItem = LstPrismLayers.SelectedItem
        ManageAoiLayerButtons()
    End Sub

    Private Sub ManageAoiLayerButtons()
        Dim lCount As Integer = LstAoiRasterLayers.SelectedItems.Count + _
                                LstDemLayers.SelectedItems.Count + _
                                LstPrismLayers.SelectedItems.Count
        If lCount > 0 Then
            BtnViewAoi.Enabled = True
            BtnClearSelected.Enabled = True
            If m_lstHruLayersItem IsNot Nothing Then
                Panel1.Enabled = True
            End If
        Else
            BtnViewAoi.Enabled = False
            BtnClearSelected.Enabled = False
            Panel1.Enabled = False
        End If
    End Sub


    Private Sub BtnViewAoi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnViewAoi.Click
        Dim fileNamesWithStyle As List(Of String) = BA_ListOfLayerNamesWithStyles()
        ' Display raster layers
        If LstAoiRasterLayers.SelectedIndex > -1 Then
            Dim items As IList = LstAoiRasterLayers.SelectedItems
            For Each item As LayerListItem In items
                If fileNamesWithStyle.IndexOf(item.Name) > -1 Then
                    Dim symbology As BA_Map_Symbology = BA_GetRasterMapSymbology(item.Name)
                    BA_DisplayRasterWithSymbol(My.ArcMap.Document, item.Value, symbology.DisplayName, _
                                               symbology.DisplayStyle, symbology.Transparency, WorkspaceType.Raster)
                Else
                    BA_DisplayRaster(My.ArcMap.Application, item.Value)
                End If
            Next
        End If
        ' Display DEM layers
        If LstDemLayers.SelectedIndex > -1 Then
            Dim items As IList = LstDemLayers.SelectedItems
            For Each item As LayerListItem In items
                If item.LayerType = LayerType.Raster Then
                    BA_DisplayRaster(My.ArcMap.Application, item.Value, item.Name)
                Else
                    'Need special processing here for the pourpoint layer
                    'We assume this is a point layer because all vector styles are points at this time
                    Dim strFileName As String = BA_GetBareName(item.Value)
                    If fileNamesWithStyle.IndexOf(strFileName) > -1 Then
                        Dim symbology As BA_Map_Symbology = BA_GetPointMapSymbology(strFileName)
                        BA_MapDisplayPointMarkers(My.ArcMap.Application, item.Value, symbology.DisplayName, symbology.Color, symbology.MarkerType)
                    Else
                        BA_DisplayVector(My.ArcMap.Document, item.Value)
                    End If
                End If
            Next
        End If
        'Prism layers
        If LstPrismLayers.SelectedIndex > -1 Then
            Dim items As IList = LstPrismLayers.SelectedItems
            For Each item As LayerListItem In items
                BA_DisplayRaster(My.ArcMap.Application, item.Value, item.Name)
            Next
        End If

    End Sub

    Private Sub BtnClearSelected_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClearSelected.Click
        LstAoiRasterLayers.SelectedIndex = -1
        LstPrismLayers.SelectedIndex = -1
        LstDemLayers.SelectedIndex = -1
    End Sub

    Private Sub BtnStats_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnStats.Click

        Dim rasterPath As String = ""
        Dim rasterName As String = BA_GetBareName(m_lstHruLayersItem.Value, rasterPath)

        rasterPath = BA_StandardizePathString(rasterPath, False)
        Dim itemLst As IList = New List(Of LayerListItem)
        For idx = 0 To LstAoiRasterLayers.SelectedItems.Count - 1
            itemLst.Add(LstAoiRasterLayers.SelectedItems.Item(idx))
        Next
        For idx = 0 To LstDemLayers.SelectedItems.Count - 1
            itemLst.Add(LstDemLayers.SelectedItems.Item(idx))
        Next
        For idx = 0 To LstPrismLayers.SelectedItems.Count - 1
            itemLst.Add(LstPrismLayers.SelectedItems.Item(idx))
        Next

        ' Create/configure a step progressor
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, itemLst.Count * 10 * 2)
        pStepProg.Show()
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Calculating Zonal Statistics", "Calculating...")
        progressDialog2.ShowDialog()

        Dim pathName As String = ""
        Try
            Dim aoiGdbPath = m_aoi.FilePath & "\" & BA_EnumDescription(GeodatabaseNames.Aoi)
            Dim snapRasterPath As String = aoiGdbPath & "\" & BA_EnumDescription(AOIClipFile.AOIExtentCoverage)
            For idx = 0 To itemLst.Count - 1
                Dim fname As String = itemLst.Item(idx).name
                If Len(fname) > 5 Then
                    fname = fname.Substring(0, 5)
                End If

                Dim tempFilePath As String = ""
                BA_GetBareName(m_lstHruLayersItem.Value, tempFilePath)
                Dim tempFileName As String = "zoneTbl"

                Dim valueLyr As String = itemLst.Item(idx).value
                If ChkMean.Checked Then
                    BA_ZonalStats2Att(rasterPath, rasterName, tempFilePath, tempFileName, valueLyr, fname & "_Me", snapRasterPath, True, StatisticsTypeString.MEAN)
                End If
                pStepProg.Step()
                If ChkRange.Checked Then
                    BA_ZonalStats2Att(rasterPath, rasterName, tempFilePath, tempFileName, valueLyr, fname & "_Rg", snapRasterPath, True, StatisticsTypeString.RANGE)
                End If
                pStepProg.Step()
                If ChkSTD.Checked Then
                    BA_ZonalStats2Att(rasterPath, rasterName, tempFilePath, tempFileName, valueLyr, fname & "_ST", snapRasterPath, True, StatisticsTypeString.STD)
                End If
                pStepProg.Step()
                If ChkSum.Checked Then
                    BA_ZonalStats2Att(rasterPath, rasterName, tempFilePath, tempFileName, valueLyr, fname & "_Su", snapRasterPath, True, StatisticsTypeString.SUM)
                End If
                pStepProg.Step()
                If ChkVariety.Checked Then
                    BA_ZonalStats2Att(rasterPath, rasterName, tempFilePath, tempFileName, valueLyr, fname & "_Vr", snapRasterPath, True, StatisticsTypeString.VARIETY)
                End If
                pStepProg.Step()
                If ChkMinority.Checked Then
                    BA_ZonalStats2Att(rasterPath, rasterName, tempFilePath, tempFileName, valueLyr, fname & "_Mt", snapRasterPath, True, StatisticsTypeString.MINORITY)
                End If
                pStepProg.Step()
                If ChkMajority.Checked Then
                    BA_ZonalStats2Att(rasterPath, rasterName, tempFilePath, tempFileName, valueLyr, fname & "_Mj", snapRasterPath, True, StatisticsTypeString.MAJORITY)
                End If
                pStepProg.Step()
                If ChkMax.Checked Then
                    BA_ZonalStats2Att(rasterPath, rasterName, tempFilePath, tempFileName, valueLyr, fname & "_Mx", snapRasterPath, True, StatisticsTypeString.MAXIMUM)
                End If
                pStepProg.Step()
                If ChkMedian.Checked Then
                    BA_ZonalStats2Att(rasterPath, rasterName, tempFilePath, tempFileName, valueLyr, fname & "_Md", snapRasterPath, True, StatisticsTypeString.MEDIAN)
                End If
                pStepProg.Step()
                If ChkMin.Checked Then
                    BA_ZonalStats2Att(rasterPath, rasterName, tempFilePath, tempFileName, valueLyr, fname & "_Mi", snapRasterPath, True, StatisticsTypeString.MINIMUM)
                End If
                pStepProg.Step()

                If ChkMean.Checked Then
                    BA_ZonalStats2Att(rasterPath, "grid_v", tempFilePath, tempFileName, valueLyr, fname & "_Me", snapRasterPath, True, StatisticsTypeString.MEAN)
                End If
                pStepProg.Step()
                If ChkRange.Checked Then
                    BA_ZonalStats2Att(rasterPath, "grid_v", tempFilePath, tempFileName, valueLyr, fname & "_Rg", snapRasterPath, True, StatisticsTypeString.RANGE)
                End If
                pStepProg.Step()
                If ChkSTD.Checked Then
                    BA_ZonalStats2Att(rasterPath, "grid_v", tempFilePath, tempFileName, valueLyr, fname & "_ST", snapRasterPath, True, StatisticsTypeString.STD)
                End If
                pStepProg.Step()
                If ChkSum.Checked Then
                    BA_ZonalStats2Att(rasterPath, "grid_v", tempFilePath, tempFileName, valueLyr, fname & "_Su", snapRasterPath, True, StatisticsTypeString.SUM)
                End If
                pStepProg.Step()
                If ChkVariety.Checked Then
                    BA_ZonalStats2Att(rasterPath, "grid_v", tempFilePath, tempFileName, valueLyr, fname & "_Vr", snapRasterPath, True, StatisticsTypeString.VARIETY)
                End If
                pStepProg.Step()
                If ChkMinority.Checked Then
                    BA_ZonalStats2Att(rasterPath, "grid_v", tempFilePath, tempFileName, valueLyr, fname & "_Mt", snapRasterPath, True, StatisticsTypeString.MINORITY)
                End If
                pStepProg.Step()
                If ChkMajority.Checked Then
                    BA_ZonalStats2Att(rasterPath, "grid_v", tempFilePath, tempFileName, valueLyr, fname & "_Mj", snapRasterPath, True, StatisticsTypeString.MAJORITY)
                End If
                pStepProg.Step()
                If ChkMax.Checked Then
                    BA_ZonalStats2Att(rasterPath, "grid_v", tempFilePath, tempFileName, valueLyr, fname & "_Mx", snapRasterPath, True, StatisticsTypeString.MAXIMUM)
                End If
                pStepProg.Step()
                If ChkMedian.Checked Then
                    BA_ZonalStats2Att(rasterPath, "grid_v", tempFilePath, tempFileName, valueLyr, fname & "_Md", snapRasterPath, True, StatisticsTypeString.MEDIAN)
                End If
                pStepProg.Step()
                If ChkMin.Checked Then
                    BA_ZonalStats2Att(rasterPath, "grid_v", tempFilePath, tempFileName, valueLyr, fname & "_Mi", snapRasterPath, True, StatisticsTypeString.MINIMUM)
                End If
                pStepProg.Step()
            Next
            progressDialog2.HideDialog()

            MessageBox.Show("Zonal Statistics Calculations Completed ", "Zonal Statistics", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ResetFormAfterStat()

        Catch ex As Exception
            MessageBox.Show("BtnStats_Click() Exception: " & ex.Message)
        Finally
            ' Clean up step progressor
            pStepProg = Nothing
            If progressDialog2 IsNot Nothing Then
                progressDialog2.HideDialog()
                progressDialog2 = Nothing
            End If
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub

    Private Sub Manage_Checkbox()
        If ChkMean.Checked Or ChkRange.Checked Or ChkSTD.Checked Or _
           ChkSum.Checked Or ChkVariety.Checked Or ChkMinority.Checked Or _
           ChkMajority.Checked Or ChkMax.Checked Or ChkMedian.Checked Or _
           ChkMin.Checked Then
            BtnStats.Enabled = True
        Else
            BtnStats.Enabled = False
        End If
    End Sub

    Private Sub ChkMean_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkMean.CheckedChanged
        Manage_Checkbox()
    End Sub

    Private Sub ChkMajority_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkMajority.CheckedChanged
        Manage_Checkbox()
    End Sub

    Private Sub ChkMax_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkMax.CheckedChanged
        Manage_Checkbox()
    End Sub

    Private Sub ChkMedian_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkMedian.CheckedChanged
        Manage_Checkbox()
    End Sub

    Private Sub ChkMin_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkMin.CheckedChanged
        Manage_Checkbox()
    End Sub

    Private Sub ChkMinority_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkMinority.CheckedChanged
        Manage_Checkbox()
    End Sub

    Private Sub ChkRange_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkRange.CheckedChanged
        Manage_Checkbox()
    End Sub

    Private Sub ChkSTD_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkSTD.CheckedChanged
        Manage_Checkbox()
    End Sub

    Private Sub ChkMinMax_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkSum.CheckedChanged
        Manage_Checkbox()
    End Sub

    Private Sub ChkVariety_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkVariety.CheckedChanged
        Manage_Checkbox()
    End Sub

    'Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    RtxtZoneFieldName.Text = "Mean-1,Range-2,STD-3,Sum-4,Variety-5,Minority-6,Majority-7,Max-8,Median-9,Min-10"
    'End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow
        Dim dockWinID As UID = New UIDClass()
        dockWinID.Value = My.ThisAddIn.IDs.FrmZonalStats
        dockWindow = My.ArcMap.DockableWindowManager.GetDockableWindow(dockWinID)
        dockWindow.Show(False)
    End Sub

    Private Sub BtnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAbout.Click
        Dim toolHelpForm As FrmHelp = New FrmHelp(BA_HelpTopics.ZonalStatistics)
        toolHelpForm.ShowDialog()
    End Sub
End Class