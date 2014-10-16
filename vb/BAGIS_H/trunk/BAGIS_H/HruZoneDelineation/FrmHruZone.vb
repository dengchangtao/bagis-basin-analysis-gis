Imports System.Windows.Forms
Imports System.IO
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Desktop.AddIns
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Geometry
Imports BAGIS_ClassLibrary

''' <summary>
''' Form used for defining hru rules resulting in hru zones
''' </summary>
Public Class frmHruZone

    Dim m_aoi As Aoi
    Dim m_pendingRules As New List(Of BAGIS_ClassLibrary.IRule)
    Dim m_ruleId As Integer = 0
    Dim m_lstHruLayersItem As LayerListItem = Nothing
    Dim LstAoiVectorLayers As ListBox = New ListBox
    Dim LstAoiRasterLayers As ListBox = New ListBox
    Dim LstDemLayers As ListBox = New ListBox
    Dim LstPrismLayers As ListBox = New ListBox


    Public Sub New(ByVal hook As Object)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        InitializeCboRuleType()

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

        Private m_windowUI As frmHruZone

        Protected Overrides Function OnCreateChild() As System.IntPtr
            m_windowUI = New frmHruZone(Me.Hook)
            Return m_windowUI.Handle
        End Function

        Protected Overrides Sub Dispose(ByVal Param As Boolean)
            If m_windowUI IsNot Nothing Then
                m_windowUI.Dispose(Param)
            End If

            MyBase.Dispose(Param)
        End Sub

        ' This property allows other forms access to the UI of this form
        Protected Friend ReadOnly Property UI() As frmHruZone
            Get
                Return m_windowUI
            End Get
        End Property

    End Class

    Private Sub BtnSelectAoi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSelectAoi.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim DataPath As String
        Dim pFilter As IGxObjectFilter = New GxFilterContainers
        Dim hruExt As HruExtension = HruExtension.GetExtension

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
                m_aoi = New Aoi(aoiName, DataPath, Nothing, hruExt.version)
                TxtAoiPath.Text = m_aoi.FilePath
                ResetForm()
                Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow = GetHruZoneDockableWindow()
                dockWindow.Caption = "Current AOI: " & aoiName & m_aoi.ApplicationVersion
                'Set current aoi in comboBox on Toolbar
                Dim comboBox = AddIn.FromID(Of CboSelectedAoi)(My.ThisAddIn.IDs.CboSelectedAoi)
                If comboBox IsNot Nothing Then
                    comboBox.setValue(aoiName)
                End If
                hruExt.aoi = m_aoi
                SetDatumInExtension()

                'Check to make sure the units are set in the metadata before proceeding
                Dim slopeUnit As SlopeUnit
                Dim elevUnit As MeasurementUnit
                Dim depthUnit As MeasurementUnit    'prism data
                BA_GetMeasurementUnitsForAoi(m_aoi.FilePath, slopeUnit, elevUnit, depthUnit)
                If slopeUnit = slopeUnit.Missing Or _
                   elevUnit = MeasurementUnit.Missing Or _
                   depthUnit = MeasurementUnit.Missing Then
                    Dim frmDataUnits As FrmDataUnits = New FrmDataUnits(m_aoi, slopeUnit, elevUnit, depthUnit)
                    frmDataUnits.ShowDialog()
                End If

                'Load rule info from xml if it exists in aoi
                Dim xmlOutputPath As String = m_aoi.FilePath & BA_EnumDescription(PublicPath.RulesXml)
                ' Open old rules file if there is one
                If BA_File_ExistsWindowsIO(xmlOutputPath) Then
                    Dim pHru As Hru = BA_LoadRulesFromXml(m_aoi.FilePath)
                    'Only load the rules if the version hasn't changed
                    If pHru IsNot Nothing Then
                        If pHru.ApplicationVersion = hruExt.version Then
                            ReloadRules(pHru)
                        Else
                            'Delete the rule file if version has changed
                            BA_Remove_File(xmlOutputPath)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("BtnSelectAoi_Click Exception: " & ex.Message)
        End Try
    End Sub

    Public Sub LoadLstLayers(Optional ByVal aoi As Aoi = Nothing)
        Dim ShapefileCount As Long, RasterCount As Long
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
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 12 + zoneCount)
        pStepProg.Show()
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Loading layer lists", "Loading...")

        Try
            Dim bTime As DateTime = DateTime.Now
            LstAoiVectorLayers.Items.Clear()
            LstAoiRasterLayers.Items.Clear()
            LstDemLayers.Items.Clear()
            LstPrismLayers.Items.Clear()
            LstHruLayers.Items.Clear()
            LstSelectedZones.Items.Clear()
            progressDialog2.ShowDialog()
            pStepProg.Step()
            Dim AOIVectorList() As String = Nothing
            Dim AOIRasterList() As String = Nothing
            Dim layerPath As String = m_aoi.FilePath & "\" & BA_EnumDescription(GeodatabaseNames.Layers)
            BA_ListLayersinGDB(layerPath, AOIRasterList, AOIVectorList)
            Dim ts As TimeSpan = DateTime.Now.Subtract(bTime)
            'Debug.Print("ts1: " & ts.Milliseconds)
            bTime = DateTime.Now
            pStepProg.Step()

            'display shapefiles
            ShapefileCount = UBound(AOIVectorList)
            If ShapefileCount > 0 Then
                For i = 1 To ShapefileCount
                    ' Vectors are always discrete
                    Dim isDiscrete As Boolean = True
                    Dim fullLayerPath As String = layerPath & "\" & AOIVectorList(i)
                    Dim item As LayerListItem = New LayerListItem(AOIVectorList(i), fullLayerPath, LayerType.Vector, isDiscrete)
                    LstAoiVectorLayers.Items.Add(item)
                Next
            End If
            ts = DateTime.Now.Subtract(bTime)
            'Debug.Print("ts2: " & ts.Milliseconds)
            bTime = DateTime.Now
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
            ts = DateTime.Now.Subtract(bTime)
            'Debug.Print("ts3: " & ts.Milliseconds)
            bTime = DateTime.Now
            pStepProg.Step()

            'display dem layers
            LoadDemLayers()
            ts = DateTime.Now.Subtract(bTime)
            'Debug.Print("ts4: " & ts.Milliseconds)
            bTime = DateTime.Now
            pStepProg.Step()

            'display prism layers
            LoadPrismLayers()
            ts = DateTime.Now.Subtract(bTime)
            'Debug.Print("ts5: " & ts.Milliseconds)
            bTime = DateTime.Now
            pStepProg.Step()

            'display hru layers
            LoadHruLayers(dirZonesArr, pStepProg)
            ts = DateTime.Now.Subtract(bTime)
            'Debug.Print("ts6: " & ts.Milliseconds)
            bTime = DateTime.Now
            pStepProg.Step()

            'display hru parent template layers
            LoadParentHruLayers()
            ts = DateTime.Now.Subtract(bTime)
            'Debug.Print("ts7: " & ts.Milliseconds)
            bTime = DateTime.Now
            pStepProg.Step()

        Catch ex As Exception
            MessageBox.Show("LoadLstLayers() Exception: " & ex.Message)
        Finally
            ' Clean up step progressor
            pStepProg.Hide()
            progressDialog2.HideDialog()
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStepProg)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(progressDialog2)
        End Try

    End Sub

    Public Sub ReloadListLayers()
        If m_aoi IsNot Nothing Then
            If Not String.IsNullOrEmpty(m_aoi.FilePath) Then
                LoadLstLayers()
            End If
        End If
    End Sub

    ' Call this method to reload the user-controlled layers
    Public Sub BA_ReloadUserLayers()
        If m_aoi IsNot Nothing Then
            Dim ShapefileCount As Long, RasterCount As Long
            LstAoiVectorLayers.Items.Clear()
            LstAoiRasterLayers.Items.Clear()
            Dim AOIVectorList() As String = Nothing
            Dim AOIRasterList() As String = Nothing
            Dim layerPath As String = BA_GetPath(m_aoi.FilePath, PublicPath.Layers)
            BA_ListLayersinAOI(layerPath, AOIRasterList, AOIVectorList)
            'display shapefiles
            ShapefileCount = UBound(AOIVectorList)
            If ShapefileCount > 0 Then
                For i = 1 To ShapefileCount
                    ' Vectors are always discrete
                    Dim isDiscrete As Boolean = True
                    Dim fullLayerPath As String = layerPath & "\" & AOIVectorList(i)
                    Dim item As LayerListItem = New LayerListItem(AOIVectorList(i), fullLayerPath, LayerType.Vector, isDiscrete)
                    LstAoiVectorLayers.Items.Add(item)
                Next
            End If

            'display raster layers
            RasterCount = UBound(AOIRasterList)
            If RasterCount > 0 Then
                For i = 1 To RasterCount
                    Dim fullLayerPath As String = layerPath & "\" & AOIRasterList(i)
                    Dim isDiscrete As Boolean = BA_IsIntegerRaster(fullLayerPath)
                    Dim item As LayerListItem = New LayerListItem(AOIRasterList(i), fullLayerPath, LayerType.Raster, isDiscrete)
                    LstAoiRasterLayers.Items.Add(item)
                Next
            End If
        End If
    End Sub

    Private Sub InitializeCboRuleType()
        ' Initialize Layer Type dropdown list
        CboRuleType.SelectedIndex = CType(HruRuleType.ContributingArea, Short)
    End Sub

    Private Sub CopyItemsFromListBoxToComboBox(ByVal SourceListBox As ListBox, ByRef TargetComboBox As System.Windows.Forms.ComboBox)
        TargetComboBox.Items.Clear()
        If SourceListBox.Items.Count > 0 Then
            Dim i As Short
            While i < SourceListBox.Items.Count
                TargetComboBox.Items.Add(SourceListBox.Items(i))
                i += 1
            End While
            If TargetComboBox.Items.Count > 0 Then TargetComboBox.SelectedIndex = 0
        End If
    End Sub

    Private Sub LstHruLayers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles LstHruLayers.SelectedIndexChanged
        ' unselect previous selected item
        If m_lstHruLayersItem IsNot Nothing Then
            LstHruLayers.SelectedItems.Remove(m_lstHruLayersItem)
        End If
        ' reset selected index to new value
        m_lstHruLayersItem = LstHruLayers.SelectedItem
        If m_lstHruLayersItem IsNot Nothing Then
            BtnViewLayer.Enabled = True
            BtnViewLayerLog.Enabled = True
            BtnRenameLayer.Enabled = True
            BtnDeleteLayer.Enabled = True
        Else
            ResetHruDetails()
        End If
    End Sub

    'Private Sub BtnViewAoi_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    Dim fileNamesWithStyle As List(Of String) = BA_ListOfLayerNamesWithStyles()
    '    ' Display raster layers
    '    If LstAoiRasterLayers.SelectedIndex > -1 Then
    '        Dim items As IList = LstAoiRasterLayers.SelectedItems
    '        For Each item As LayerListItem In items
    '            If fileNamesWithStyle.IndexOf(item.Name) > -1 Then
    '                Dim symbology As BA_Map_Symbology = BA_GetRasterMapSymbology(item.Name)
    '                BA_DisplayRasterWithSymbol(My.ArcMap.Document, item.Value, symbology.DisplayName, symbology.DisplayStyle, symbology.Transparency)
    '            Else
    '                BA_DisplayRaster(My.ArcMap.Application, item.Value)
    '            End If
    '        Next
    '    End If
    '    ' Display vector layers
    '    If LstAoiVectorLayers.SelectedIndex > -1 Then
    '        Dim items As IList = LstAoiVectorLayers.SelectedItems
    '        For Each item As LayerListItem In items
    '            If fileNamesWithStyle.IndexOf(item.Name) > -1 Then
    '                Dim symbology As BA_Map_Symbology = BA_GetPointMapSymbology(item.Name)
    '                BA_MapDisplayPointMarkers(My.ArcMap.Application, item.Value, symbology.DisplayName, symbology.Color, symbology.MarkerType)
    '            Else
    '                BA_DisplayVector(My.ArcMap.Document, item.Value)
    '            End If
    '        Next
    '    End If
    '    ' Display DEM layers
    '    If LstDemLayers.SelectedIndex > -1 Then
    '        Dim items As IList = LstDemLayers.SelectedItems
    '        For Each item As LayerListItem In items
    '            If item.LayerType = LayerType.Raster Then
    '                BA_DisplayRaster(My.ArcMap.Application, item.Value, item.Name)
    '            Else
    '                'Need special processing here for the pourpoint layer
    '                'We assume this is a point layer because all vector styles are points at this time
    '                Dim strFileName As String = BA_GetBareName(item.Value)
    '                If fileNamesWithStyle.IndexOf(strFileName) > -1 Then
    '                    Dim symbology As BA_Map_Symbology = BA_GetPointMapSymbology(strFileName)
    '                    BA_MapDisplayPointMarkers(My.ArcMap.Application, item.Value, symbology.DisplayName, symbology.Color, symbology.MarkerType)
    '                Else
    '                    BA_DisplayVector(My.ArcMap.Document, item.Value)
    '                End If
    '            End If
    '        Next
    '    End If
    '    'Prism layers
    '    If LstPrismLayers.SelectedIndex > -1 Then
    '        Dim items As IList = LstPrismLayers.SelectedItems
    '        For Each item As LayerListItem In items
    '            BA_DisplayRaster(My.ArcMap.Application, item.Value, item.Name)
    '        Next
    '    End If

    'End Sub

    Private Sub BtnViewLayerLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnViewLayerLog.Click
        Dim hruItem As LayerListItem = CType(LstHruLayers.SelectedItem, LayerListItem)
        Dim hruInputPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, hruItem.Name)
        Dim aoi As Aoi = Nothing
        Try
            aoi = BA_LoadHRUFromXml(hruInputPath)
            Dim hruExt As HruExtension = HruExtension.GetExtension
            If aoi IsNot Nothing Then
                If aoi.ApplicationVersion <> hruExt.version Then
                    MessageBox.Show("This Hru was created using an older version of BAGIS-H. The log may not display correctly", "Old version", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                Dim frmHruTabLog As FrmHruTabLog = New FrmHruTabLog(aoi, hruItem.Name, True)
                frmHruTabLog.ShowDialog()
            Else
                MessageBox.Show("The log file is missing for this HRU. It cannot be displayed.", "Missing file", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show("An unknown error occurred while trying to load the log file and it cannot be viewed.", "Unknown error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub BtnDeleteRule_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnDeleteRule.Click
        Dim selectedCellCount As Integer = GridViewRules.GetCellCount(DataGridViewElementStates.Selected)
        If selectedCellCount > 0 Then
            Dim sRowIndex As Short = GridViewRules.SelectedCells(0).RowIndex
            Dim delRow As DataGridViewRow = GridViewRules.Rows(sRowIndex)
            Dim rowIdCol As DataGridViewColumn = GridViewRules.Columns("RuleId")
            Dim rowIdCell As DataGridViewCell = delRow.Cells(rowIdCol.Index)
            Dim rowId As Integer = rowIdCell.Value

            Dim workspacePath As String = TxtLayerLocation.Text & "\" & TxtNewHruName.Text & ".gdb"
            For Each pRule As BAGIS_ClassLibrary.IRule In m_pendingRules
                If pRule.RuleId = rowId Then
                    Dim result As DialogResult = MessageBox.Show("About to permanently delete '" & pRule.RuleTypeText & "' rule. This action cannot be undone. Do you wish to continue ?", "Delete rule", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                    If result = DialogResult.Yes Then
                        m_pendingRules.Remove(pRule)
                        If pRule.FactorStatus = FactorStatus.Complete Then
                            Dim prefix As String = "r" & pRule.RuleId.ToString("000")
                            BA_RemoveLayerByFileNamePrefix(My.Document, TxtLayerLocation.Text, prefix)
                            BA_RemoveFilesByPrefix(workspacePath, prefix)
                        End If
                    End If
                    Exit For
                End If
            Next
            If m_pendingRules.Count = 0 Then
                BtnDeleteRule.Enabled = False
                BtnUpdateRule.Enabled = False
                BtnRunRule.Enabled = False
                BtnGenerateHru.Enabled = False
                m_ruleId = 0
            Else
                BA_SortPendingRules(My.Document, m_pendingRules, workspacePath)
                Dim ruleCount As Integer = m_pendingRules.Count
                'reset ruleId to # rules
                m_ruleId = ruleCount
            End If
            RefreshGridView(False)
            ManageTxtNewHruName()
        Else
            MessageBox.Show("No rows are selected to delete")
        End If
    End Sub

    Private Sub BtnDefineRule_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnDefineRule.Click

        Dim ruleIdx As Short = CboRuleType.SelectedIndex
        Dim temporaryMessage As String = "This rule will be implemented in a future release."
        ' Contributing area rule selected
        If CType(HruRuleType.ContributingArea, Short) = ruleIdx Then
            Dim frmContribArea As FrmHruContribAreaRule = New FrmHruContribAreaRule
            frmContribArea.ShowDialog()
            'MessageBox.Show(temporaryMessage, "Contributing Area", MessageBoxButtons.OK, MessageBoxIcon.Information)
            'DAFlow Tag
        ElseIf CType(HruRuleType.DAFlowTypeZones, Short) = ruleIdx Then
            Dim hruDAFlowZoneForm As FrmDAFlowRule = New FrmDAFlowRule(m_aoi.FilePath)
            hruDAFlowZoneForm.ShowDialog()
        ElseIf CType(HruRuleType.RasterReclassContinuous, Short) = ruleIdx Then
            Dim reclassContinuousForm As FrmReclassContinuous = New FrmReclassContinuous(LstAoiRasterLayers, LstDemLayers, _
                                                                    LstPrismLayers, sender, e)
            reclassContinuousForm.ShowDialog()
        ElseIf CType(HruRuleType.RasterReclassification, Short) = ruleIdx Then
            Dim hruReclassifyZoneForm As FrmHruRasterReclassRule = New FrmHruRasterReclassRule(False, LstAoiRasterLayers, _
                                                                                                 LstDemLayers, LstPrismLayers, LstHruLayers, _
                                                                                                 True, sender, e)
            hruReclassifyZoneForm.ShowDialog()
        ElseIf CType(HruRuleType.RasterSlices, Short) = ruleIdx Then
            Dim hruReclassifyZoneForm As FrmHruRasterReclassRule = New FrmHruRasterReclassRule(True, LstAoiRasterLayers, LstDemLayers, LstPrismLayers, LstHruLayers, True, sender, e)
            hruReclassifyZoneForm.ShowDialog()
        ElseIf CType(HruRuleType.PrismPrecipitation, Short) = ruleIdx Then
            Dim prismPrecipForm As FrmPrismPrecipRule = New FrmPrismPrecipRule(m_aoi.FilePath, LstPrismLayers)
            If prismPrecipForm.DataUnits <> MeasurementUnit.Missing Then
                prismPrecipForm.ShowDialog()
            End If
        ElseIf CType(HruRuleType.Aspect, Short) = ruleIdx Then
            If BA_LayerInList(BA_EnumDescription(MapsLayerName.aspect), LstDemLayers) = True Then
                Dim aspectForm As FrmAspectTemplate = New FrmAspectTemplate(m_aoi.FilePath)
                aspectForm.ShowDialog()
            Else
                MessageBox.Show("Aspect layer is missing from this AOI. The Aspect template cannot be run.", "Missing input", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        ElseIf CType(HruRuleType.Slope, Short) = ruleIdx Then
            Dim slopeTemplates As List(Of TemplateRule) = BA_GetTemplates(HruRuleType.Slope)
            Dim slopeForm As FrmSlopeTemplate = New FrmSlopeTemplate(slopeTemplates, True)
            If slopeForm.ValidUnits = True Then
                slopeForm.ShowDialog()
            End If
            ElseIf CType(HruRuleType.Canopy, Short) = ruleIdx Then
                'Dim canopyLayerPath As String = BA_EnumDescription(PublicPath.Canopy)
                'Dim canopyDisplayName As String = BA_GetBareName(canopyLayerPath)
                Dim canopyTemplates As List(Of TemplateRule) = BA_GetTemplates(HruRuleType.Canopy)
                Dim canopyForm As FrmCanopyTemplate = New FrmCanopyTemplate(canopyTemplates, True)
                canopyForm.ShowDialog()
            ElseIf CType(HruRuleType.LandUse, Short) = ruleIdx Then
                Dim landUseTemplates As List(Of TemplateRule) = BA_GetTemplates(HruRuleType.LandUse)
                Dim landUseForm As FrmLandUseTemplate = New FrmLandUseTemplate(landUseTemplates, True)
                landUseForm.ShowDialog()
            End If

    End Sub

    Private Sub RadApplyRulesSelect_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadApplyRulesSelect.CheckedChanged
        LstSelectedZones.Enabled = RadApplyRulesSelect.Checked
        If LstSelectedZones.Enabled Then
            For i = LstSelectedZones.Items.Count - 1 To 0 Step -1
                LstSelectedZones.SelectedItems.Add(LstSelectedZones.Items(i))
            Next i
            BtnToggle.Enabled = True
        Else
            For i = LstSelectedZones.Items.Count - 1 To 0 Step -1
                LstSelectedZones.SelectedItems.Remove(LstSelectedZones.Items(i))
            Next i
            BtnToggle.Enabled = False
        End If
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
            Dim nextFolder As String = BA_GetPRISMFolderName(i)
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

    Private Sub LoadHruLayers(ByVal dirZonesArr As DirectoryInfo(), _
                              ByVal pStepProg As IStepProgressor)
        If dirZonesArr IsNot Nothing Then
            Dim item As LayerListItem
            For Each dri In dirZonesArr
                Dim hruFilePath As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, dri.Name) & BA_EnumDescription(PublicPath.HruGrid)
                Dim hruXmlFilePath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, dri.Name) & BA_EnumDescription(PublicPath.HruXml)
                ' Add hru to the list if the grid exists
                If BA_File_Exists(hruFilePath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) And _
                   BA_File_ExistsWindowsIO(hruXmlFilePath) Then
                    'Assume hru is discrete raster since we create it to be so
                    item = New LayerListItem(dri.Name, hruFilePath, LayerType.Raster, True)
                    LstHruLayers.Items.Add(item)
                    pStepProg.Step()
                End If
            Next dri
        End If
    End Sub

    ' Note: this sub is dependent on LoadHruLayers() running first
    Private Sub LoadParentHruLayers()
        CboParentHru.Items.Clear()
        Dim noneItem As LayerListItem = New LayerListItem("None", Nothing, Nothing, True)
        CboParentHru.Items.Add(noneItem)
        For Each item In LstHruLayers.Items
            Dim layerListItem As LayerListItem = CType(item, LayerListItem)
            CboParentHru.Items.Add(layerListItem)
        Next
        CboParentHru.SelectedIndex = 0
    End Sub

    Private Sub LoadZoneLayers(ByVal hruFilePath As String)
        Dim zones As Zone() = BA_ReadZonesFromRaster(hruFilePath, WorkspaceType.Geodatabase)
        For i As Integer = 0 To zones.GetUpperBound(0)
            LstSelectedZones.Items.Add(zones(i).Id)
        Next
    End Sub

    'Private Sub BtnClearSelected_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '    LstAoiRasterLayers.SelectedIndex = -1
    '    LstAoiVectorLayers.SelectedIndex = -1
    '    LstPrismLayers.SelectedIndex = -1
    '    LstDemLayers.SelectedIndex = -1
    '    LstHruLayers.SelectedIndex = -1
    'End Sub

    Private Sub ResetHruDetails()
        m_lstHruLayersItem = Nothing
        BtnViewLayer.Enabled = False
        BtnViewLayerLog.Enabled = False
        BtnDeleteLayer.Enabled = False
        BtnRenameLayer.Enabled = False
    End Sub

    Public Sub AddPendingRule(ByVal newRule As BAGIS_ClassLibrary.IRule)
        If m_pendingRules.Count > 0 Then
            Dim idx As Integer = -1
            For i As Integer = 0 To m_pendingRules.Count - 1
                If m_pendingRules(i).RuleId = newRule.RuleId Then
                    idx = i
                    Exit For
                End If
            Next
            If idx > -1 Then
                m_pendingRules.RemoveAt(idx)
                m_pendingRules.Insert(idx, newRule)
            Else
                m_pendingRules.Add(newRule)
            End If
        Else
            m_pendingRules.Add(newRule)
        End If
        RefreshGridView(False)
        BtnDeleteRule.Enabled = True
        BtnUpdateRule.Enabled = True
        If TxtNewHruName.Text.Length > 0 Then
            BtnGenerateHru.Enabled = True
            BtnRunRule.Enabled = True
        End If
    End Sub

    Private Function GetHruZoneDockableWindow() As ESRI.ArcGIS.Framework.IDockableWindow
        Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow
        Dim dockWinID As ESRI.ArcGIS.esriSystem.UID = New ESRI.ArcGIS.esriSystem.UIDClass()
        dockWinID.Value = My.ThisAddIn.IDs.frmHruZone
        dockWindow = My.ArcMap.DockableWindowManager.GetDockableWindow(dockWinID)
        Return dockWindow
    End Function

    Private Sub BtnGenerateHru_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnGenerateHru.Click
        If String.IsNullOrEmpty(TxtNewHruName.Text) Then
            MessageBox.Show("Please supply an HRU name before trying to generate an HRU.")
            Exit Sub
        End If

        ' Create/configure a step progressor
        Dim stepCount As Integer = 7 + m_pendingRules.Count
        Dim pStepProg As IStepProgressor = Nothing
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = Nothing

        Try
            EnableFormButtons(False)
            ' Delete any saved rules
            BA_Remove_File(m_aoi.FilePath & BA_EnumDescription(PublicPath.RulesXml))
            Dim hruOutputPath2 As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, TxtNewHruName.Text)
            Dim hruFolderPath As String = BA_HruFolderPathFromGdbString(hruOutputPath2)
            If (BA_Workspace_Exists(hruFolderPath) And BA_CompletedRuleCount(m_pendingRules) = 0) Then
                Dim result As DialogResult = MessageBox.Show("HRU directory already exists. Overwrite existing directory ?", "Folder exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If result = DialogResult.Yes Then
                    Dim success As BA_ReturnCode = BA_DeleteHRU(hruFolderPath, My.ArcMap.Document)
                    If success <> BA_ReturnCode.Success Then
                        MessageBox.Show("Unable to delete HRU '" & TxtNewHruName.Text & "'. Please restart ArcMap and try again", "Unable to delete HRU", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        EnableFormButtons(True)
                        Exit Sub
                    End If
                    pStepProg = BA_GetStepProgressor(My.ArcMap.Application.hWnd, stepCount)
                    ' Create/configure the ProgressDialog. This automatically displays the dialog
                    progressDialog2 = BA_GetProgressDialog(pStepProg, "Generating HRUs...", "Generate HRUs")
                    pStepProg.Step()
                Else
                    EnableFormButtons(True)
                    TxtNewHruName.Focus()
                    Exit Sub
                End If
            Else
                pStepProg = BA_GetStepProgressor(My.ArcMap.Application.hWnd, stepCount)
                ' Create/configure the ProgressDialog. This automatically displays the dialog
                progressDialog2 = BA_GetProgressDialog(pStepProg, "Generating HRUs...", "Generate HRUs")
                pStepProg.Step()
            End If

            BA_CreateHruOutputFolders(m_aoi.FilePath, TxtNewHruName.Text)
            ' Create new file GDB for HRU
            BA_CreateHruOutputGDB(m_aoi.FilePath, TxtNewHruName.Text)
            pStepProg.Step()

            ' List of paths to rule layers to combine
            Dim layerPathList As New List(Of String)
            Dim returnVal As BA_ReturnCode = BA_ReturnCode.UnknownError
            For Each pRule As BAGIS_ClassLibrary.IRule In m_pendingRules
                Dim ruleFilePath As String = hruOutputPath2 & "\" & pRule.OutputDatasetName
                If pRule.FactorStatus = FactorStatus.Pending Then
                    returnVal = BA_RunRule(m_aoi.FilePath, hruOutputPath2, pRule)
                    If returnVal = BA_ReturnCode.Success Then
                        layerPathList.Add(ruleFilePath)
                    End If
                    RefreshGridView(True)
                Else
                    layerPathList.Add(ruleFilePath)
                End If
                pStepProg.Step()
            Next
            Dim aoiGdbPath = m_aoi.FilePath & "\" & BA_EnumDescription(GeodatabaseNames.Aoi)
            Dim maskFilePath As String = aoiGdbPath & "\" & BA_EnumDescription(AOIClipFile.AOIExtentCoverage)
            Dim snapRasterPath As String = aoiGdbPath & BA_EnumDescription(PublicPath.AoiGrid)
            Dim selectZonesArray(0) As Zone
            Dim parentHru As Hru = Nothing
            If CboParentHru.SelectedIndex > 0 Then
                Dim selectedCollection As ListBox.SelectedObjectCollection = LstSelectedZones.SelectedItems
                Dim selectedZonesList As IList = CType(selectedCollection, IList)   ' explicit cast
                selectZonesArray = BA_ReadSelectedZones(selectedZonesList)
                Dim zoneHru As LayerListItem = CType(CboParentHru.SelectedItem, LayerListItem)  'explicit cast
                Dim zoneHruFullPath As String = zoneHru.Value
                returnVal = BA_CreateHruFromZones(maskFilePath, zoneHruFullPath, layerPathList, _
                                                  RadApplyRulesSelect.Checked, selectZonesArray, _
                                                  hruOutputPath2, GRID, snapRasterPath)
                If returnVal = BA_ReturnCode.Success Then
                    Dim hruInputPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, zoneHru.Name)
                    Dim aoi As Aoi = BA_LoadHRUFromXml(hruInputPath)
                    For Each pHru In aoi.HruList
                        ' We found the hru the user selected
                        If String.Compare(pHru.Name, zoneHru.Name) = 0 Then
                            parentHru = pHru
                        End If
                    Next
                End If
            Else
                ' We save the output to a temporary grid; It will be renamed as final grid if non-contiguous is allowed
                returnVal = BA_ZoneOverlay(maskFilePath, layerPathList, hruOutputPath2, GRID, False, _
                                           True, snapRasterPath, WorkspaceType.Geodatabase)
            End If
            pStepProg.Step()

            If returnVal = BA_ReturnCode.Success Then
                Dim rInputPath As String = hruOutputPath2 & BA_EnumDescription(PublicPath.HruGrid)
                Dim vOutputPath As String = hruOutputPath2 & BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruVector), False, True)
                Dim vReturnVal As Short = BA_Raster2PolygonShapefileFromPath(rInputPath, vOutputPath, False)

                ' Filled DEM Path
                Dim layerPath As String = m_aoi.FilePath & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces)
                Dim fullLayerPath As String = layerPath & "\" & BA_EnumDescription(MapsFileName.filled_dem_gdb)
                'get raster resolution
                Dim cellSize As Double
                Dim rasterStat As IRasterStatistics = BA_GetRasterStatsGDB(fullLayerPath, cellSize)
                returnVal = BA_ProcessNonContiguousGrids(CkNonContiguous.Checked, vOutputPath, _
                             hruOutputPath2, cellSize, snapRasterPath)

                If returnVal = BA_ReturnCode.Success Then
                    Dim parentPath As String = Nothing
                    If parentHru IsNot Nothing Then
                        parentPath = parentHru.FilePath & GRID
                    End If
                    pStepProg.Step()

                    If CkRetainAttributes.Checked = True Then
                        returnVal = BA_AddAttributesToHru(m_aoi.FilePath, hruOutputPath2, hruOutputPath2, _
                                                          m_pendingRules, parentPath)
                        pStepProg.Step()
                    End If

                    Dim pHru As Hru = BA_CreateHru(TxtNewHruName.Text, rInputPath, vOutputPath, selectZonesArray, _
                                                   m_pendingRules, CkNonContiguous.Checked)
                    If parentHru IsNot Nothing Then pHru.ParentHru = parentHru
                    pHru.RetainSourceAttributes = CkRetainAttributes.Checked
                    pStepProg.Step()

                    Dim pHruList As IList(Of Hru) = New List(Of Hru)
                    pHruList.Add(pHru)
                    m_aoi.HruList = pHruList
                    Dim xmlOutputPath As String = hruFolderPath & BA_EnumDescription(PublicPath.HruXml)
                    'MessageBox.Show("7. Generating XML")
                    m_aoi.Save(xmlOutputPath)
                    progressDialog2.HideDialog()

                    MessageBox.Show("New HRU " & TxtNewHruName.Text & " successfully created", "HRU: " & TxtNewHruName.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ResetForm()
                End If
            End If
            If returnVal <> BA_ReturnCode.Success Then
                MessageBox.Show("Failed to create HRU " & TxtNewHruName.Text, "HRU: " & TxtNewHruName.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show("Failed to create HRU " & TxtNewHruName.Text & ". Exception: " & ex.Message)
        Finally
            ' Clean up step progressor
            If progressDialog2 IsNot Nothing Then
                progressDialog2.HideDialog()
            End If
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStepProg)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(progressDialog2)
        End Try

    End Sub

    Private Sub RefreshGridView(ByVal hruInProcess As Boolean)
        GridViewRules.Rows.Clear()
        For Each rule In m_pendingRules
            Dim ruleType As String = rule.RuleTypeText
            Dim layerUsed As String = rule.InputLayerName
            Dim outputFolder As String = rule.OutputDatasetName
            Dim status As String = rule.FactorStatus.ToString
            Dim ruleId As String = CStr(rule.RuleId)
            '---create a row---
            Dim item As New DataGridViewRow
            item.CreateCells(GridViewRules)
            With item
                .Cells(0).Value = ruleType
                .Cells(1).Value = layerUsed
                .Cells(2).Value = outputFolder
                .Cells(3).Value = status
                .Cells(5).Value = ruleId
            End With
            '---add the row---
            GridViewRules.Rows.Add(item)
            If rule.FactorStatus = FactorStatus.Complete Then
                'format button if rule is complete
                Dim col As DataGridViewColumn = GridViewRules.Columns(4)
                col.DefaultCellStyle.Padding = New Padding(2)
                Dim buttonCol As DataGridViewButtonColumn = CType(col, DataGridViewButtonColumn)
                If hruInProcess = True Then
                    buttonCol.Text = rule.FactorStatus.ToString
                    buttonCol.UseColumnTextForButtonValue = True
                    buttonCol.FlatStyle = FlatStyle.Flat
                Else
                    buttonCol.Text = "View"
                    buttonCol.UseColumnTextForButtonValue = True
                End If
            Else
                ' hide button with blank cell if rule isn't complete
                item.Cells(4) = New DataGridViewTextBoxCell
            End If
        Next
    End Sub

    Public Function GetNextRuleId() As Integer
        m_ruleId = m_ruleId + 1
        Return m_ruleId
    End Function

    ' May 10, 2011 - Comment out for now. We have an event to check this when generating output
    Private Sub TxtNewHruName_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtNewHruName.Leave
        'Dim hruOutputPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, TxtNewHruName.Text)
        'If BA_Workspace_Exists(hruOutputPath) Then
        '    Dim result As DialogResult = MessageBox.Show("HRU directory already exists.", "Folder exists", MessageBoxButtons.OK, MessageBoxIcon.Information)
        'End If
    End Sub

    Private Sub TxtNewHruName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtNewHruName.TextChanged
        ' Must have at least one pending rule
        If m_pendingRules IsNot Nothing And m_pendingRules.Count > 0 Then
            If TxtNewHruName.Text.Length > 0 Then
                BtnGenerateHru.Enabled = True
                BtnRunRule.Enabled = True
            Else
                BtnGenerateHru.Enabled = False
                BtnRunRule.Enabled = False
            End If
        End If
        TxtLayerLocation.Text = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, TxtNewHruName.Text)
    End Sub

    Private Sub BtnViewLayer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnViewLayer.Click
        If LstHruLayers.SelectedIndex > -1 Then
            Dim item As LayerListItem = LstHruLayers.SelectedItem
            BA_DisplayHruZonesRaster(My.ArcMap.Document, item.Value, item.Name)
            'BA_DisplayRasterWithSymbol(My.ArcMap.Document, item.Value, item.Name, _
            '               MapsDisplayStyle.Range_Random, 0, WorkspaceType.Geodatabase)
        End If
    End Sub

    Private Sub LstSelectedZones_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles LstSelectedZones.MouseMove
        Dim MousePositionInClientCoords As Drawing.Point = Me.LstSelectedZones.PointToClient(frmHruZone.MousePosition)
        Dim indexUnderTheMouse As Integer = Me.LstSelectedZones.IndexFromPoint(MousePositionInClientCoords)
        If indexUnderTheMouse > -1 Then
            Me.ToolTipZones.SetToolTip(Me.LstSelectedZones, "Click to deselect zone")
        End If

    End Sub

    ' Reset form to defaults except for AOI
    Public Sub ResetForm(Optional ByVal pAoi As Aoi = Nothing)
        If pAoi IsNot Nothing Then m_aoi = pAoi

        'Clear rules table
        m_pendingRules.Clear()
        RefreshGridView(False)
        'Reset rule id
        m_ruleId = 0
        'Reset comboBox default
        InitializeCboRuleType()
        BtnDefineRule.Enabled = True
        'Reload list layers 
        LoadLstLayers()
        'Clean HRU section
        ResetHruDetails()
        'Reset HRU path info
        CkNonContiguous.Checked = False
        TxtLayerLocation.Text = m_aoi.FilePath & BA_EnumDescription(PublicPath.HruDirectory)
        TxtNewHruName.Text = Nothing
        TxtNewHruName.Enabled = True
        BtnGenerateHru.Enabled = False
        BtnRunRule.Enabled = False

        SetDatumInExtension()
    End Sub

    Private Sub BtnDeleteLayer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDeleteLayer.Click
        Dim hruItem As LayerListItem = CType(LstHruLayers.SelectedItem, LayerListItem)
        Dim result As DialogResult = MessageBox.Show("About to permanently delete " & hruItem.Name & ". This action cannot be undone. Do you wish to continue ?", "Delete HRU", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If result = DialogResult.Yes Then
            Dim parentPath1 As String = "PleaseReturn"
            Dim parentPath2 As String = "PleaseReturn"
            Dim fileName As String = BA_GetBareName(hruItem.Value, parentPath1)
            ' Go up one more level for parent folder of .gdb
            fileName = BA_GetBareName(parentPath1, parentPath2)
            'remove the last character if it's a backslash
            If Microsoft.VisualBasic.Right(parentPath2, 1) = "\" Then parentPath2 = Microsoft.VisualBasic.Left(parentPath2, Len(parentPath2) - 1)
            Dim success As BA_ReturnCode = BA_DeleteHRU(parentPath2, My.ArcMap.Document)
            If success <> BA_ReturnCode.Success Then
                MessageBox.Show("Unable to delete HRU '" & hruItem.Name & "'. Please restart ArcMap and try again", "Unable to delete HRU", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
            LstHruLayers.Items.Remove(hruItem)
            CboParentHru.Items.Remove(hruItem)
        Else
            Exit Sub
        End If
    End Sub

    Private Sub BtnUpdateRule_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnUpdateRule.Click
        Dim selectedCellCount As Integer = GridViewRules.GetCellCount(DataGridViewElementStates.Selected)
        If selectedCellCount > 0 Then
            Dim sRowIndex As Short = GridViewRules.SelectedCells(0).RowIndex
            Dim updateRow As DataGridViewRow = GridViewRules.Rows(sRowIndex)
            Dim rowIdCol As DataGridViewColumn = GridViewRules.Columns("RuleId")
            Dim rowIdCell As DataGridViewCell = updateRow.Cells(rowIdCol.Index)
            Dim rowId As Integer = rowIdCell.Value
            For Each pRule As BAGIS_ClassLibrary.IRule In m_pendingRules
                If pRule.RuleId = rowId Then
                    If pRule.FactorStatus = FactorStatus.Complete Then
                        Dim result As DialogResult = MessageBox.Show("This rule has already been run. Updating the rule will delete the previous output. Do you wish to continue ?", _
                                                                     "Rule output exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        If result = DialogResult.Yes Then
                            Dim prefix As String = "r" & pRule.RuleId.ToString("000")
                            Dim workspacePath As String = TxtLayerLocation.Text & "\" & TxtNewHruName.Text & ".gdb"
                            BA_RemoveLayerByFileNamePrefix(My.Document, TxtLayerLocation.Text, prefix)
                            BA_RemoveFilesByPrefix(workspacePath, prefix)
                        Else
                            Exit Sub
                        End If
                    End If
                    If TypeOf pRule Is RasterReclassRule Or TypeOf pRule Is RasterSliceRule Then
                        Dim rasterReclassForm As FrmHruRasterReclassRule = New FrmHruRasterReclassRule(False, LstAoiRasterLayers, _
                                                                                     LstDemLayers, LstPrismLayers, LstHruLayers, _
                                                                                     True, sender, e)
                        rasterReclassForm.LoadForm(pRule, LstAoiRasterLayers, LstDemLayers, LstPrismLayers, LstHruLayers, sender, e)
                        rasterReclassForm.ShowDialog()
                    ElseIf TypeOf pRule Is ReclassContinuousRule Then
                        Dim reclassContinuousForm As FrmReclassContinuous = New FrmReclassContinuous(LstAoiRasterLayers, LstDemLayers, _
                                                        LstPrismLayers, sender, e)
                        reclassContinuousForm.ReloadForm(pRule, LstAoiVectorLayers, LstDemLayers, _
                                                         LstPrismLayers, sender, e)
                        reclassContinuousForm.ShowDialog()
                        'ElseIf TypeOf pRule Is PrismPrecipRule Then
                        '    Dim prismForm As New FrmPrismPrecipRule(m_aoi.FilePath, LstPrismLayers)
                        '    prismForm.LoadForm(pRule)
                        '    prismForm.Show()
                    ElseIf TypeOf pRule Is PrismPrecipRule Then
                        Dim prismForm As New FrmPrismPrecipRule(m_aoi.FilePath, LstPrismLayers)
                        prismForm.LoadForm(pRule)
                        prismForm.Show()
                        'ContributingAreas Tag
                    ElseIf TypeOf pRule Is ContributingAreasRule Then
                        Dim contribForm As FrmHruContribAreaRule = New FrmHruContribAreaRule()
                        contribForm.LoadForm(pRule)
                        contribForm.ShowDialog()
                        'DAFlow Tag
                    ElseIf TypeOf pRule Is DAFlowTypeZonesRule Then
                        Dim hruDAFlowZoneForm As FrmDAFlowRule = New FrmDAFlowRule(m_aoi.FilePath)
                        hruDAFlowZoneForm.LoadForm(pRule)
                        hruDAFlowZoneForm.ShowDialog()
                    ElseIf TypeOf pRule Is TemplateRule Then
                        Dim tRule As TemplateRule = CType(pRule, TemplateRule)
                        If tRule.RuleType = HruRuleType.Aspect Then
                            Dim aspectForm As New FrmAspectTemplate(m_aoi.FilePath)
                            aspectForm.LoadForm(pRule)
                            aspectForm.Show()
                        ElseIf tRule.RuleType = HruRuleType.Slope Then
                            Dim slopeTemplates As List(Of TemplateRule) = BA_GetTemplates(HruRuleType.Slope)
                            Dim slopeForm As FrmSlopeTemplate = New FrmSlopeTemplate(slopeTemplates, True)
                            slopeForm.ReloadForm(pRule)
                            slopeForm.Show()
                        ElseIf tRule.RuleType = HruRuleType.Canopy Then
                            Dim canopyTemplates As List(Of TemplateRule) = BA_GetTemplates(HruRuleType.Canopy)
                            Dim canopyForm As FrmCanopyTemplate = New FrmCanopyTemplate(canopyTemplates, True)
                            canopyForm.ReloadForm(pRule)
                            canopyForm.Show()
                        ElseIf tRule.RuleType = HruRuleType.LandUse Then
                            Dim landUseTemplates As List(Of TemplateRule) = BA_GetTemplates(HruRuleType.LandUse)
                            Dim landUseForm As FrmLandUseTemplate = New FrmLandUseTemplate(landUseTemplates, True)
                            landUseForm.LoadForm(pRule)
                            landUseForm.Show()
                        End If
                    End If
                    ManageTxtNewHruName()
                    Exit Sub
                End If
            Next
        Else
            MessageBox.Show("No rows are selected to update.")
        End If
    End Sub

    Private Sub BtnRunRule_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnRunRule.Click
        If String.IsNullOrEmpty(TxtNewHruName.Text) Then
            MessageBox.Show("Please supply an HRU name before trying to run a rule.")
            Exit Sub
        End If
        Dim selectedCellCount As Integer = GridViewRules.GetCellCount(DataGridViewElementStates.Selected)
        If selectedCellCount > 0 Then
            Dim sRowIndex As Short = GridViewRules.SelectedCells(0).RowIndex
            Dim updateRow As DataGridViewRow = GridViewRules.Rows(sRowIndex)
            Dim rowIdCol As DataGridViewColumn = GridViewRules.Columns("RuleId")
            Dim rowIdCell As DataGridViewCell = updateRow.Cells(rowIdCol.Index)
            Dim rowId As Integer = rowIdCell.Value
            Dim ruleId As String = Nothing
            ' Create/configure a step progressor
            Dim pStepProg As IStepProgressor = Nothing
            ' Create/configure the ProgressDialog. This automatically displays the dialog
            Dim progressDialog2 As IProgressDialog2 = Nothing

            Try
                For Each pRule As BAGIS_ClassLibrary.IRule In m_pendingRules
                    If pRule.RuleId = rowId Then
                        ' Create hru directory. Duplicates logic in BtnGenerateHru_Click
                        Dim hruOutputPath2 As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, TxtNewHruName.Text)
                        Dim hruFolderPath As String = BA_HruFolderPathFromGdbString(hruOutputPath2)
                        'Has selected rule already been run?
                        If pRule.FactorStatus = FactorStatus.Complete Then
                            Dim result As DialogResult = MessageBox.Show("This rule has already been run. Overwrite existing output ?", "Completed rule", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            ' Don't delete existing content
                            If result = DialogResult.No Then
                                Exit Sub
                            Else
                                pStepProg = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 4)
                                progressDialog2 = BA_GetProgressDialog(pStepProg, "Running rule", "Running rule...")
                                pStepProg.Step()
                                'Delete existing content; Rule will be re-run below
                                Dim prefix As String = "r" & pRule.RuleId.ToString("000")
                                BA_RemoveLayerByFileNamePrefix(My.Document, TxtLayerLocation.Text, prefix)
                                BA_RemoveFilesByPrefix(hruOutputPath2, prefix)
                                pRule.FactorStatus = FactorStatus.Pending
                                m_pendingRules.Item(pRule.RuleId - 1) = pRule
                                RefreshGridView(False)
                                'Dim statusCol As DataGridViewColumn = GridViewRules.Columns("Status")
                                'Dim statusCell As DataGridViewCell = updateRow.Cells(rowIdCol.Index)
                                'statusCell.Value = pRule.FactorStatus.ToString
                            End If
                            'Does hru folder have content from an old hru
                        ElseIf (BA_Workspace_Exists(hruFolderPath) And BA_CompletedRuleCount(m_pendingRules) = 0) Then
                            Dim result As DialogResult = MessageBox.Show("HRU directory already exists. Overwrite existing directory ?", "Folder exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            If result = DialogResult.Yes Then
                                Dim success As BA_ReturnCode = BA_DeleteHRU(hruFolderPath, My.ArcMap.Document)
                                If success <> BA_ReturnCode.Success Then
                                    MessageBox.Show("Unable to delete HRU '" & TxtNewHruName.Text & "'. Please restart ArcMap and try again", "Unable to delete HRU", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                    Exit Sub
                                End If
                                ' Delete previous content of hru folder before running rule
                                pStepProg = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 4)
                                progressDialog2 = BA_GetProgressDialog(pStepProg, "Running rule", "Running rule...")
                                pStepProg.Step()
                            Else
                                ' Set focus to hru name so it can be chanted
                                TxtNewHruName.Focus()
                                Exit Sub
                            End If
                        Else
                            pStepProg = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 4)
                            progressDialog2 = BA_GetProgressDialog(pStepProg, "Running rule", "Running rule...")
                            pStepProg.Step()
                        End If

                        BA_CreateHruOutputFolders(m_aoi.FilePath, TxtNewHruName.Text)
                        ' Create new file GDB for HRU
                        BA_CreateHruOutputGDB(m_aoi.FilePath, TxtNewHruName.Text)
                        pStepProg.Step()

                        ' Code to run rule here
                        BA_RunRule(m_aoi.FilePath, hruOutputPath2, pRule)
                        pStepProg.Step()

                        ruleId = CStr(pRule.RuleId)
                        Exit For
                    End If
                Next
                RefreshGridView(False)
                ManageTxtNewHruName()
                pStepProg.Step()
                'MessageBox.Show("Rule " & ruleId & " has finished running.", "Rule: " & ruleId, MessageBoxButtons.OK, MessageBoxIcon.Information)

            Catch ex As Exception
                MessageBox.Show("BtnRunRule_Click Exception: " & ex.Message)
            Finally
                ' Clean up step progressor
                pStepProg = Nothing
                If progressDialog2 IsNot Nothing Then progressDialog2.HideDialog()
                progressDialog2 = Nothing
                GC.WaitForPendingFinalizers()
                GC.Collect()
            End Try
        Else
            MessageBox.Show("No rows are selected to run")
        End If

    End Sub

    Private Sub GridViewRules_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridViewRules.CellContentClick
        Dim colIdx As Integer = e.ColumnIndex
        If colIdx = 4 Then
            Dim rowIdx As Integer = e.RowIndex
            Dim strRuleId As String = GridViewRules.Rows(rowIdx).Cells(5).Value
            For Each rule In m_pendingRules
                If rule.RuleId = CInt(strRuleId) Then
                    Dim hruOutputPath As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, TxtNewHruName.Text)
                    Dim filePath As String = hruOutputPath & "\" & rule.OutputDatasetName
                    Dim displayName As String = rule.OutputDatasetName
                    BA_DisplayRaster(My.ArcMap.Application, filePath, displayName)
                    Exit Sub
                End If
            Next
        End If
    End Sub

    Private Sub CboParentHru_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboParentHru.SelectedIndexChanged
        Dim pItem As LayerListItem = CType(CboParentHru.SelectedItem, LayerListItem) ' Explicit cast
        LstSelectedZones.Items.Clear()
        If CboParentHru.SelectedIndex = 0 Then
            'none selected
            RadApplyRulesAll.Enabled = False
            RadApplyRulesAll.Checked = False
            RadApplyRulesSelect.Enabled = False
        Else
            'selected a parent layer
            LoadZoneLayers(pItem.Value)
            RadApplyRulesAll.Enabled = True
            RadApplyRulesAll.Checked = True
            RadApplyRulesSelect.Enabled = True
        End If
    End Sub

    'Private Sub ManageAoiLayerButtons()
    '    Dim lCount As Integer = LstAoiRasterLayers.SelectedItems.Count + _
    '           LstAoiVectorLayers.SelectedItems.Count + _
    '           LstDemLayers.SelectedItems.Count + _
    '           LstPrismLayers.SelectedItems.Count
    '    If lCount > 0 Then
    '        BtnViewAoi.Enabled = True
    '        BtnClearSelected.Enabled = True
    '    Else
    '        BtnViewAoi.Enabled = False
    '        BtnClearSelected.Enabled = False
    '    End If
    'End Sub

    'Private Sub LstAoiVectorLayers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    ManageAoiLayerButtons()
    'End Sub

    'Private Sub LstAoiRasterLayers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    ManageAoiLayerButtons()
    'End Sub

    'Private Sub LstDemLayers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    ManageAoiLayerButtons()
    'End Sub

    'Private Sub LstPrismLayers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    ManageAoiLayerButtons()
    'End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow
        Dim dockWinID As UID = New UIDClass()
        dockWinID.Value = My.ThisAddIn.IDs.frmHruZone
        dockWindow = My.ArcMap.DockableWindowManager.GetDockableWindow(dockWinID)
        dockWindow.Show(False)
    End Sub

    Private Sub ManageTxtNewHruName()
        If m_pendingRules.Count = 0 Then
            TxtNewHruName.Enabled = True
            Exit Sub
        End If
        For Each pRule As BAGIS_ClassLibrary.IRule In m_pendingRules
            If pRule.FactorStatus = FactorStatus.Complete Then
                TxtNewHruName.Enabled = False
                Exit Sub
            End If
        Next
        TxtNewHruName.Enabled = True
    End Sub

    Private Sub BtnRenameLayer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnRenameLayer.Click
        Dim renameItem As LayerListItem = CType(LstHruLayers.SelectedItem, LayerListItem)
        Dim message, title As String
        message = "Please enter a new name for HRU " & renameItem.Name & ":"
        title = "Rename HRU"
        Dim result As Object = InputBox(message, title)
        Dim strResult As String = Trim(CStr(result))
        If Not String.IsNullOrEmpty(strResult) Then
            If strResult.ToUpper = renameItem.Name.ToUpper Then
                MessageBox.Show("The selected HRU is already named " & renameItem.Name & " and cannot be renamed.", "HRU exists", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
            Dim hruOutputPath2 As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, strResult)
            Dim hruFolderPath As String = BA_HruFolderPathFromGdbString(hruOutputPath2)
            If BA_Workspace_Exists(hruFolderPath) Then
                Dim dupResult As DialogResult = MessageBox.Show("HRU directory already exists. Overwrite existing directory ?", "Folder exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If dupResult = DialogResult.Yes Then
                    Dim success As BA_ReturnCode = BA_DeleteHRU(hruFolderPath, My.Document)
                    If success <> BA_ReturnCode.Success Then
                        MessageBox.Show("Unable to delete HRU '" & strResult & "'. Please restart ArcMap and try again", "Unable to delete HRU", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Exit Sub
                    End If
                Else
                    Exit Sub
                End If
            End If
            Dim renameFolderPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, renameItem.Name)
            BA_RemoveLayersInFolder(My.Document, renameFolderPath)
            BA_RenameHRU(m_aoi.FilePath, renameItem.Name, strResult, LstHruLayers.Items)
            ReloadListLayers()
        End If
    End Sub

    ' Sets the datum string from the source DEM in the hru extension
    Private Sub SetDatumInExtension()
        Dim hruExt As HruExtension = HruExtension.GetExtension
        Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(m_aoi.FilePath)
        Dim parentPath As String = m_aoi.FilePath & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces)
        Dim pGeoDataSet As IGeoDataset = BA_OpenRasterFromGDB(parentPath, BA_EnumDescription(MapsFileName.filled_dem_gdb))
        If pGeoDataSet IsNot Nothing Then
            'Spatial reference for the dataset in question
            Dim pSpRef As ESRI.ArcGIS.Geometry.ISpatialReference = pGeoDataSet.SpatialReference
            If pSpRef IsNot Nothing Then
                hruExt.Datum = BA_DatumString(pSpRef)
                hruExt.SpatialReference = pSpRef.Name
            End If
            pSpRef = Nothing
        End If
        pGeoDataSet = Nothing
        GC.WaitForPendingFinalizers()
        GC.Collect()
    End Sub

    'Save the rules to an xml file at the root of the AOI directory
    Private Sub SavePendingRules()
        Dim hruOutputPath2 As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, TxtNewHruName.Text)
        Dim rInputPath As String = hruOutputPath2 & BA_EnumDescription(PublicPath.HruGrid)
        Dim vOutputPath As String = hruOutputPath2 & BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruVector), False, True)
        Dim selectZonesArray(0) As Zone
        Dim parentHru As Hru = Nothing
        If CboParentHru.SelectedIndex > 0 Then
            Dim selectedCollection As ListBox.SelectedObjectCollection = LstSelectedZones.SelectedItems
            Dim selectedZonesList As IList = CType(selectedCollection, IList)   ' explicit cast
            selectZonesArray = BA_ReadSelectedZones(selectedZonesList)
            Dim zoneHru As LayerListItem = CType(CboParentHru.SelectedItem, LayerListItem)  'explicit cast

            Dim hruInputPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, zoneHru.Name)
            Dim aoi As Aoi = BA_LoadHRUFromXml(hruInputPath)
            If aoi Is Nothing Then
                MessageBox.Show("Parent HRU information could not be located. Parent HRU information will not be saved.")
            Else
                For Each pHru In aoi.HruList
                    ' We found the hru the user selected
                    If String.Compare(pHru.Name, zoneHru.Name) = 0 Then
                        parentHru = pHru
                    End If
                Next
            End If

        End If

        Dim newHru As Hru = BA_CreateHru(TxtNewHruName.Text, rInputPath, vOutputPath, selectZonesArray, _
                                       m_pendingRules, CkNonContiguous.Checked)
        If parentHru IsNot Nothing Then newHru.ParentHru = parentHru

        'Set the version so we can catch it if it changes
        Dim hruExt As HruExtension = HruExtension.GetExtension
        newHru.ApplicationVersion = hruExt.version
        newHru.ApplyToSelectedZones = RadApplyRulesSelect.Checked
        newHru.ApplyToAllZones = RadApplyRulesAll.Checked
        newHru.RetainSourceAttributes = CkRetainAttributes.Checked
        Dim xmlOutputPath As String = m_aoi.FilePath & BA_EnumDescription(PublicPath.RulesXml)
        ' Delete old rules file if there is one
        If BA_File_ExistsIDEUtil(xmlOutputPath) Then
            BA_Remove_File(xmlOutputPath)
        End If

        'Save rules to disk
        newHru.Save(xmlOutputPath)
    End Sub

    Public Function ReloadRules(ByVal rulesHru As Hru) As BA_ReturnCode

        Dim retVal As BA_ReturnCode = ReloadParentTemplate(rulesHru)

        If Not String.IsNullOrEmpty(rulesHru.Name) Then
            TxtNewHruName.Text = rulesHru.Name
        End If
        CkNonContiguous.Checked = rulesHru.AllowNonContiguousHru

        ' Sort the rules by ruleId
        rulesHru.RuleList.Sort()
        Dim missingLayer As Boolean = False
        For Each pRule In rulesHru.RuleList
            'Verify that source file exists for each rule
            Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(pRule.InputFolderPath)
            Dim datasetType As esriDatasetType = esriDatasetType.esriDTRasterDataset
            ' Make an exception for DAFlow because it is based on a vector
            If TypeOf pRule Is DAFlowTypeZonesRule Then
                datasetType = esriDatasetType.esriDTFeatureClass
            End If
            ' Make an exception for the source layer of custom prism calculations since that is created when the rule is run
            Dim checkData As Boolean = True
            If TypeOf pRule Is PrismPrecipRule Then
                Dim prismRule As PrismPrecipRule = CType(pRule, PrismPrecipRule)
                If prismRule.DataRange = PrismDataRange.Custom Then
                    checkData = False
                End If
            End If
            If checkData = False Or BA_File_Exists(pRule.InputFolderPath, workspaceType, datasetType) = True Then
                'Reset status to pending for failed rules
                If pRule.FactorStatus = FactorStatus.Failed Then pRule.FactorStatus = FactorStatus.Pending
                'Reset status to pending for completed rules with missing output
                If pRule.FactorStatus = FactorStatus.Complete Then
                    Dim hruOutputPath As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, TxtNewHruName.Text)
                    Dim filePath As String = hruOutputPath & "\" & pRule.OutputDatasetName
                    If BA_File_Exists(filePath, workspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) = False Then
                        pRule.FactorStatus = FactorStatus.Pending
                    End If
                End If
                AddPendingRule(pRule)
                'Increment ruleId in case new rules are added later
                Dim tempInt As Integer = GetNextRuleId()
            Else
                Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder
                sb.Append("The saved rule source layer '" & pRule.InputLayerName & "' is missing. ")
                sb.Append("Therefore the " & pRule.RuleTypeText & " rule will not be added to the rules list.")
                MessageBox.Show(sb.ToString, "Missing rule source layer", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                missingLayer = True
            End If
        Next
        If missingLayer = True Then
            Dim workspacePath As String = TxtLayerLocation.Text & "\" & TxtNewHruName.Text & ".gdb"
            BA_SortPendingRules(My.Document, m_pendingRules, workspacePath)
            RefreshGridView(False)
        End If

        Return retVal
    End Function

    Public Function ReloadParentTemplate(ByVal rulesHru As Hru) As BA_ReturnCode
        If rulesHru.ParentHru IsNot Nothing Then
            Dim fullPath As String = rulesHru.ParentHru.FilePath & GRID
            If BA_File_Exists(fullPath, WorkspaceType.Geodatabase, ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTRasterDataset) = True Then

                For Each pItem In CboParentHru.Items
                    Dim listItem As LayerListItem = CType(pItem, LayerListItem)
                    If listItem.Name = rulesHru.ParentHru.Name Then
                        CboParentHru.SelectedItem = listItem
                        Exit For
                    End If
                Next

                RadApplyRulesAll.Checked = rulesHru.ApplyToAllZones
                RadApplyRulesSelect.Checked = rulesHru.ApplyToSelectedZones
                CkRetainAttributes.Checked = rulesHru.RetainSourceAttributes

                If rulesHru.SelectedZones IsNot Nothing And rulesHru.SelectedZones.Length > 0 Then
                    Dim allZones As New List(Of Long)
                    For Each lZone As Long In LstSelectedZones.Items
                        allZones.Add(lZone)
                    Next
                    LstSelectedZones.SelectedItems.Clear()
                    For Each pZone As Zone In rulesHru.SelectedZones
                        Dim idx As Long = 0
                        For Each lZone As Long In allZones
                            If pZone.Id = lZone Then
                                LstSelectedZones.SelectedIndices.Add(idx)
                            End If
                            idx += 1
                        Next
                    Next
                End If
            Else
                MessageBox.Show("The saved parent HRU layer '" & rulesHru.ParentHru.Name & "' is missing. Parent HRU layer properties will not be loaded.", "Missing parent HRU layer", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        End If
        Return BA_ReturnCode.Success
    End Function

    Public ReadOnly Property AoiName As String
        Get
            If m_aoi IsNot Nothing Then
                Return m_aoi.Name
            Else
                Return Nothing
            End If
        End Get
    End Property

    ' This event fires when the x in the upper right-hand corner of the dockableWindow is clicked or the cancel button is used
    Private Sub frmHruZone_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.VisibleChanged
        If m_pendingRules.Count > 0 Then
            Dim result As DialogResult = MessageBox.Show("There are pending rules for this HRU definition. Would you like to save them?", "Pending rules", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.Yes Then
                SavePendingRules()
            Else
                ' Delete any saved rules
                BA_Remove_File(m_aoi.FilePath & BA_EnumDescription(PublicPath.RulesXml))
            End If
        End If
    End Sub

    Private Sub BtnToggle_Click(sender As System.Object, e As System.EventArgs) Handles BtnToggle.Click
        For i = LstSelectedZones.Items.Count - 1 To 0 Step -1
            Dim newValue As Boolean = Not LstSelectedZones.GetSelected(i)
            LstSelectedZones.SetSelected(i, newValue)
        Next i
    End Sub

    Private Sub EnableFormButtons(ByVal enabled As Boolean)
        BtnDeleteRule.Enabled = enabled
        BtnUpdateRule.Enabled = enabled
        BtnRunRule.Enabled = enabled
        BtnDefineRule.Enabled = enabled
        BtnGenerateHru.Enabled = enabled
    End Sub
End Class