Imports System.Windows.Forms
Imports ESRI.ArcGIS.Geoprocessing
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.DataManagementTools
Imports ESRI.ArcGIS.Geoprocessor
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports System.IO
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Carto
Imports BAGIS_ClassLibrary

''' <summary>
''' Designer class of the dockable window add-in. It contains user interfaces that
''' make up the dockable window.
''' </summary>
Public Class FrmEliminatePoly
    Implements IDisposable

    Dim m_featurePath As String
    Dim m_featureName As String
    Dim m_aoi As Aoi
    Dim m_NumRecords As Integer
    Dim m_version As String
    Dim m_lstSelectHruLayersItem As LayerListItem = Nothing
    Dim m_selectedfield As String
    Dim m_area As Double 'unit always in Square Km
    Dim m_minHRUarea As Double
    Dim m_maxHRUarea As Double
    Dim m_deletedHRU As Long

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

        Private m_windowUI As FrmEliminatePoly

        Protected Overrides Function OnCreateChild() As System.IntPtr
            m_windowUI = New FrmEliminatePoly(Me.Hook)
            Return m_windowUI.Handle
        End Function

        Protected Overrides Sub Dispose(ByVal Param As Boolean)
            If m_windowUI IsNot Nothing Then
                m_windowUI.Dispose(Param)
            End If

            MyBase.Dispose(Param)
        End Sub
        ' This property allows other forms access to the UI of this form
        Protected Friend ReadOnly Property UI() As FrmEliminatePoly
            Get
                Return m_windowUI
            End Get
        End Property
    End Class

    Public Sub ResetEliminateFrm()
        TxtAOIPath.Text = "AOI is not specified"
        TxtNoZones.Text = ""
        'TxtNoZones.Enabled = False
        TxtMinZone.Text = ""
        'TxtMinZone.Enabled = False
        TxtMaxZone.Text = ""
        'TxtMaxZone.Enabled = False

        RadKm.Checked = True
        m_selectedfield = BA_FIELD_AREA_SQKM
        'Reset global variables
        m_NumRecords = 0
        m_lstSelectHruLayersItem = Nothing
        m_area = 0
        m_minHRUarea = 0
        m_maxHRUarea = 0
        m_deletedHRU = 0
        RadArea.Checked = True
        RadPercentile.Checked = True
        RadPercentile.Enabled = False
        RadAreaOfAoi.Enabled = False
        BtnGoToMap.Enabled = False

        TxtPolyArea.Text = ""
        TxtPolyArea.Enabled = False
        cboThreshPercnt.SelectedIndex = 0
        cboThreshPercnt.Enabled = False
        TxtHruName.Text = ""
        TxtHruName.Enabled = False
        BtnEliminate.Enabled = False
        TxtHruPath.Text = ""
        'TxtHruPath.Enabled = False
        TxtNoZonesRemoved.Text = ""
    End Sub

    Private Sub BtnSelectAOI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSelectAOI.Click
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
                LoadHruLayers(Nothing)
                Show()
            End If
        Catch ex As Exception
            MessageBox.Show("BtnSelectAOI_Click: " & ex.Message)
        End Try
    End Sub

    Public Sub LoadHruLayers(Optional ByVal aoi As Aoi = Nothing)
        If aoi IsNot Nothing Then m_aoi = aoi
        ' Create a DirectoryInfo of the HRU directory.
        Dim zonesDirectory As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, Nothing)
        Dim dirZones As New DirectoryInfo(zonesDirectory)

        ResetEliminateFrm()
        TxtAOIPath.Text = m_aoi.FilePath

        If dirZones.Exists Then
            Dim dirZonesArr As DirectoryInfo() = dirZones.GetDirectories
            Dim item As LayerListItem

            ' Create/configure a step progressor
            Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, dirZonesArr.Length + 2)
            ' Create/configure the ProgressDialog. This automatically displays the dialog
            Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Loading HRU list", "Loading...")
            pStepProg.Show()
            progressDialog2.ShowDialog()

            LstSelectHruLayers.Items.Clear()
            pStepProg.Step()

            For Each dri In dirZonesArr
                Dim hruFilePath As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, dri.Name) & BA_EnumDescription(PublicPath.HruGrid)
                Dim hruXmlFilePath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, dri.Name) & BA_EnumDescription(PublicPath.HruXml)

                ' Add hru to the list if the grid exists
                If BA_File_Exists(hruFilePath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) And _
                    BA_File_ExistsWindowsIO(hruXmlFilePath) Then
                    'Assume hru is discrete raster since we create it to be so
                    item = New LayerListItem(dri.Name, hruFilePath, LayerType.Raster, True)
                    LstSelectHruLayers.Items.Add(item)
                    pStepProg.Step()
                End If
            Next dri

            pStepProg.Hide()
            pStepProg = Nothing
            progressDialog2.HideDialog()
            progressDialog2 = Nothing
        Else
            LstSelectHruLayers.Items.Clear()
        End If

        If LstSelectHruLayers.Items.Count = 0 Then
            'Pop error message if no zones in this aoi If CboParentHru.Items.Count < 1 Then
            MessageBox.Show("No HRU datasets have been generated for this AOI. Use the 'Define Zones' tool to create an HRU dataset.", "Missing HRU datasets", MessageBoxButtons.OK, MessageBoxIcon.Information)
            'MessageBox.Show("The AOI does not have any HRU layer. The tool can only be applied on HRU layers. Please use the Define HRU tool to create HRU layers first.", "Load HRU Layers")
        End If
    End Sub

    Private Sub LstSelectHruLayers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstSelectHruLayers.SelectedIndexChanged
        ' unselect previous selected item
        'If m_lstSelectHruLayersItem IsNot Nothing Then
        '    LstSelectHruLayers.SelectedItems.Remove(m_lstSelectHruLayersItem)
        '    ResetEliminateFrm()
        'End If
        Change_TxtPolyArea(0) 'reset TxtPolyArea.Text and TxtNoZonesRemoved.Text

        m_lstSelectHruLayersItem = Nothing
        cboThreshPercnt.SelectedIndex = 0
        cboThreshPercnt.Enabled = True

        m_lstSelectHruLayersItem = LstSelectHruLayers.SelectedItem

        Dim pathName As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, m_lstSelectHruLayersItem.Name)
        m_featureName = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruVector), False)
        m_featurePath = BA_StandardizePathString(pathName)
        Dim hruVecPath As String = pathName & "\" & m_featureName
        'If Not BA_File_Exists(hruVecPath) Then
        If Not BA_File_Exists(hruVecPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            MessageBox.Show("The selected HRU doesn't contain a vector file")
            Exit Sub
        End If

        'remove vector layer "grid_v" from map
        BA_RemoveLayers(My.ArcMap.Document, m_featureName)

        'RadKm.Enabled = True
        'RadMile.Enabled = True
        'RadAcres.Enabled = True
        'RadMile.Checked = True 'select Sq. Miler radio button by default
        'm_selectedfield = BA_FIELD_AREA_ACRE

        Dim statResults As BA_DataStatistics
        If BA_GetDataStatistics(hruVecPath, BA_FIELD_AREA_SQKM, statResults) <> 0 Then
            MessageBox.Show("Can't get data statistics")
            Exit Sub
        End If

        TxtNoZones.Enabled = True
        TxtNoZones.Text = statResults.Count
        TxtMinZone.Enabled = True

        m_minHRUarea = statResults.Minimum  'internal unit is sq km
        m_maxHRUarea = statResults.Maximum

        Dim UnitConversionFactor As Double = 1
        If RadAcres.Checked Then
            UnitConversionFactor = BA_SQKm_To_ACRE
        ElseIf RadMile.Checked Then
            UnitConversionFactor = BA_SQKm_To_SQMile
        End If

        TxtMinZone.Text = Format(m_minHRUarea * UnitConversionFactor, "###,###,##0.00000")
        TxtMaxZone.Enabled = True
        TxtMaxZone.Text = Format(m_maxHRUarea * UnitConversionFactor, "###,###,##0.00000")

        m_NumRecords = statResults.Count

        'RadArea.Enabled = True
        'RadLength.Enabled = True
        'RadArea.Checked = True ' By Area is checked by default

        RadPercentile.Enabled = True
        RadAreaOfAoi.Enabled = True
        If RadPercentile.Checked = True Then
            BtnGoToMap.Enabled = False
        Else
            BtnGoToMap.Enabled = True
        End If

        TxtHruName.Enabled = True
    End Sub

    Private Sub RadKm_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadKm.CheckedChanged
        m_selectedfield = BA_FIELD_AREA_SQKM
        If Val(TxtNoZones.Text) > 0 Then
            TxtMinZone.Text = Format(m_minHRUarea, "###,###,##0.00000")
            TxtMaxZone.Text = Format(m_maxHRUarea, "###,###,##0.00000")
        End If
        If TxtPolyArea.Text <> "" Then Change_TxtPolyArea(m_area)
    End Sub

    Private Sub RadMile_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadMile.CheckedChanged
        m_selectedfield = BA_FIELD_AREA_SQMI
        If Val(TxtNoZones.Text) > 0 Then
            TxtMinZone.Text = Format(m_minHRUarea * BA_SQKm_To_SQMile, "###,###,##0.00000")
            TxtMaxZone.Text = Format(m_maxHRUarea * BA_SQKm_To_SQMile, "###,###,##0.00000")
        End If
        If TxtPolyArea.Text <> "" Then Change_TxtPolyArea(m_area)
    End Sub

    Private Sub RadAcres_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadAcres.CheckedChanged
        m_selectedfield = BA_FIELD_AREA_ACRE
        If Val(TxtNoZones.Text) > 0 Then
            TxtMinZone.Text = Format(m_minHRUarea * BA_SQKm_To_ACRE, "###,###,##0.00000")
            TxtMaxZone.Text = Format(m_maxHRUarea * BA_SQKm_To_ACRE, "###,###,##0.00000")
        End If
        If TxtPolyArea.Text <> "" Then Change_TxtPolyArea(m_area)
    End Sub

    Private Sub RadPercentile_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadPercentile.CheckedChanged
        If RadPercentile.Checked Then
            PanelPercentile.Visible = True
            PanelArea.Visible = False
            BtnGoToMap.Enabled = False
            TxtNoZonesRemoved.Text = Math.Round((cboThreshPercnt.SelectedItem / 100) * m_NumRecords + 0.5)
        Else
            PanelPercentile.Visible = False
            PanelArea.Visible = True
        End If
    End Sub

    Private Sub RadAreaOfAoi_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadAreaOfAoi.CheckedChanged
        If RadAreaOfAoi.Checked Then
            PanelPercentile.Visible = False
            PanelArea.Visible = True
            TxtPolyArea.Enabled = True
            'TxtNoZonesRemoved.Text = ""
            BtnGoToMap.Enabled = True
        Else
            PanelPercentile.Visible = True
            PanelArea.Visible = False
        End If
    End Sub

    Private Sub TxtPolyArea_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TxtPolyArea.KeyDown
        If e.KeyCode = Keys.Enter Then
            '   this is the call on the routine that does the password checking
            TxtPolyArea_Validated(Nothing, Nothing)
        End If
    End Sub

    'Private Sub TxtPolyArea_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtPolyArea.TextChanged
    '    If TxtPolyArea.Text <> "" Then
    '        m_area = TxtPolyArea.Text
    '        If m_selectedfield = BA_FIELD_AREA_SQMI Then
    '            m_area = m_area / BA_SQKm_To_SQMile 'convert area to Sq Km
    '        ElseIf m_selectedfield = BA_FIELD_AREA_ACRE Then
    '            m_area = m_area / BA_SQKm_To_ACRE
    '            'Else
    '            'do nothing
    '        End If
    '        TxtNoZonesRemoved.Text = BA_CountFeaturesSmallerThanOrEqualTo(m_featureName, m_featurePath, BA_FIELD_AREA_SQKM, m_area)
    '    End If
    'End Sub

    'Area is passed in Sq.Km.
    Public Sub Change_TxtPolyArea(ByVal pArea As Double)
        If pArea = 0 Then 'pArea cannot be zero, if zero is provided, reset the text boxes
            TxtPolyArea.Text = ""
            TxtNoZonesRemoved.Text = ""
            Exit Sub
        End If

        m_area = pArea 'm_area always uses Sq Km as unit
        If m_selectedfield = BA_FIELD_AREA_ACRE Then
            TxtPolyArea.Text = m_area * BA_SQKm_To_ACRE
        ElseIf m_selectedfield = BA_FIELD_AREA_SQMI Then
            TxtPolyArea.Text = m_area * BA_SQKm_To_SQMile
        Else
            TxtPolyArea.Text = m_area
        End If
    End Sub

    Private Sub cboThreshPercnt_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboThreshPercnt.SelectedIndexChanged
        If m_lstSelectHruLayersItem IsNot Nothing Then
            'TxtNoZonesRemoved.Text = Math.Round((cboThreshPercnt.SelectedItem / 100) * m_NumRecords + 0.5)
            'Count the number of zones we want to remove given the selected percent
            Dim desiredCount As Integer = Math.Round((cboThreshPercnt.SelectedItem / 100) * m_NumRecords + 0.5)
            Dim pAreaPrcnt As Double = cboThreshPercnt.SelectedItem
            'Get the area threshold using the selected percent
            m_area = BA_GetPercentileValueFromFeatureClass(m_featureName, m_featurePath, BA_FIELD_AREA_SQKM, pAreaPrcnt)
            Change_TxtPolyArea(m_area)
            'Get the actual count based on the area threshold, this may exceed the desired count if the data has a large number
            'of small zones
            Dim actualCount As Integer = BA_CountFeaturesSmallerThanOrEqualTo(m_featureName, m_featurePath, BA_FIELD_AREA_SQKM, m_area)
            'If the actual percentage exceeds the desired percentage, warn the user
            Dim actualPrcnt As Integer = Math.Round(actualCount / m_NumRecords * 100)
            If actualPrcnt > pAreaPrcnt Then
                MessageBox.Show("The percentile of zones to be eliminated (" & CStr(actualPrcnt) & "%) exceeds the percentile threshold due to data distribution.", "Post Processing: Eliminate Tool", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            'Update the No. of zones removed field with the actual count to be removed
            TxtNoZonesRemoved.Text = CStr(actualCount)
        End If
    End Sub

    Private Sub TxtHruName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtHruName.TextChanged
        TxtHruPath.Enabled = True
        TxtHruPath.Text = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, TxtHruName.Text)
        BtnEliminate.Enabled = True
    End Sub

    Private Sub BtnEliminate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnEliminate.Click
        Dim elim_opt As String = "AREA"
        If RadLength.Checked Then
            elim_opt = "LENGTH"
        End If

        If RadAreaOfAoi.Checked Then
            If TxtPolyArea.Text = "" Then
                MessageBox.Show("Threshhold area is not specified.")
                Exit Sub
            End If
        End If

        ' Create/configure a step progressor
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 8)
        pStepProg.Show()
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Eliminating HRU Zones", "Eliminating...")
        progressDialog2.ShowDialog()

        If RadPercentile.Checked Then
            Dim pAreaPrcnt As Double = cboThreshPercnt.SelectedItem
            'm_area always uses sq km as unit
            m_area = BA_GetPercentileValueFromFeatureClass(m_featureName, m_featurePath, BA_FIELD_AREA_SQKM, pAreaPrcnt)
        End If
        pStepProg.Step()

        If m_area <= 0 Then
            pStepProg.Hide()
            pStepProg = Nothing
            progressDialog2.HideDialog()
            progressDialog2 = Nothing
            MessageBox.Show("Please enter a valid area threshold value.")
            Exit Sub
        End If

        Try
            Dim hruOutputPath2 As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, TxtHruName.Text)
            Dim hruFolderPath As String = BA_HruFolderPathFromGdbString(hruOutputPath2)
            If BA_Workspace_Exists(hruFolderPath) Then
                Dim result As DialogResult = MessageBox.Show("HRU directory already exists. Overwrite existing directory ?", "Folder exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If result = DialogResult.Yes Then
                    Dim success As BA_ReturnCode = BA_DeleteHRU(hruFolderPath, My.Document)
                    If success <> BA_ReturnCode.Success Then
                        MessageBox.Show("Unable to delete HRU '" & TxtHruName.Text & "'. Please restart ArcMap and try again", "Unable to delete HRU", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Exit Sub
                    End If
                    pStepProg.Step()
                Else
                    pStepProg.Hide()
                    pStepProg = Nothing
                    progressDialog2.HideDialog()
                    progressDialog2 = Nothing
                    TxtHruName.Focus()
                    Exit Sub
                End If
            End If

            BA_CreateHruOutputFolders(m_aoi.FilePath, TxtHruName.Text)
            ' Create new file GDB for HRU
            BA_CreateHruOutputGDB(m_aoi.FilePath, TxtHruName.Text)

            pStepProg.Step()

            Dim vName As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruVector), False, True)
            Dim rInputPath As String = hruOutputPath2 & BA_EnumDescription(PublicPath.HruGrid)
            Dim vOutputPath As String = hruOutputPath2 & vName

            If vName(0) = "\" Then
                vName = vName.Remove(0, 1)
            End If

            Dim response As Integer
            Dim statResults As BA_DataStatistics
            Dim success2 As Integer
            statResults.Minimum = m_area + 1

            Dim tempm_featureName As String
            If m_featureName = vName Then
                tempm_featureName = m_featureName & "_temp"
            Else
                tempm_featureName = m_featureName
            End If

            'eliminate polygons iteratively
            response = BA_CopyFeatures(m_featurePath & "\" & m_featureName, hruOutputPath2 & "\" & tempm_featureName)
            success2 = BA_AddShapeAreaToAttrib(hruOutputPath2 & "\" & tempm_featureName)
            success2 = BA_GetDataStatistics(hruOutputPath2 & "\" & tempm_featureName, BA_FIELD_AREA_SQKM, statResults)
            Dim maxiteration As Integer = 3
            Dim ncount As Integer = 1
            Do While success2 = 0 And statResults.Minimum <= m_area
                If BA_File_Exists(hruOutputPath2 & "\" & vName, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                    BA_Remove_ShapefileFromGDB(hruOutputPath2, vName)
                End If

                'MsgBox("Debug message: Slivers need to be eliminated!")
                BA_EliminatePoly(hruOutputPath2, tempm_featureName, hruOutputPath2, vName, elim_opt, m_area, BA_FIELD_AREA_SQKM)
                pStepProg.Step()

                If BA_File_Exists(hruOutputPath2 & "\" & tempm_featureName, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                    BA_Remove_ShapefileFromGDB(hruOutputPath2, tempm_featureName)
                End If

                statResults.Minimum = m_area + 1
                success2 = BA_AddShapeAreaToAttrib(hruOutputPath2 & "\" & vName)
                success2 = BA_GetDataStatistics(hruOutputPath2 & "\" & vName, BA_FIELD_AREA_SQKM, statResults)

                ncount += 1
                If ncount > maxiteration Then success2 = 1 'this prevents infinite loops

                If success2 = 0 And statResults.Minimum <= m_area Then
                    response = BA_CopyFeatures(hruOutputPath2 & "\" & vName, hruOutputPath2 & "\" & tempm_featureName)
                End If
            Loop

            If BA_File_Exists(hruOutputPath2 & "\" & tempm_featureName, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                BA_Remove_ShapefileFromGDB(hruOutputPath2, tempm_featureName)
            End If

            'update TxtNoZonesRemoved value
            Dim originalCount As Long = BA_GetFeatureCount(m_featurePath, m_featureName)
            Dim newcount As Long = BA_GetFeatureCount(hruOutputPath2, vName)
            TxtNoZonesRemoved.Text = originalCount - newcount

            'add HRUID_CO and HRUID_NC fields to the Vector file
            If BA_AddCTAndNonCTToAttrib(vOutputPath) <> BA_ReturnCode.Success Then
                Throw New Exception("Eliminate Tool: Error adding CT and NonCT to Shape file.")
            End If
            pStepProg.Step()

            'Get cell size value from the form
            Dim fullLayerPath As String = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Surfaces, True) & BA_EnumDescription(MapsFileName.filled_dem_gdb)
            Dim cellSize As Double
            Dim rasterStat As IRasterStatistics = BA_GetRasterStatsGDB(fullLayerPath, cellSize)
            If cellSize <= 0 Then cellSize = 30
            rasterStat = Nothing

            Dim inFeaturesPath As String = hruOutputPath2 & "\" & vName
            Dim snapRasterPath As String = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
            'Dim snapRasterPath As String = fullLayerPath
            Dim outRasterPath As String = hruOutputPath2 & BA_EnumDescription(PublicPath.HruGrid)
            If CkNonContiguous.Checked Then
                BA_Feature2RasterGP(inFeaturesPath, outRasterPath, BA_FIELD_HRUID_NC, cellSize, snapRasterPath)
                'Additional processing to ensure grid_v is compatible with rest of app
                Dim vOutputFileName As String = BA_GetBareName(vOutputPath)
                Dim polyFileName As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruPolyVector), False)
                Dim success As BA_ReturnCode = BA_RenameFeatureClassInGDB(hruOutputPath2, vOutputFileName, polyFileName)
                If success = BA_ReturnCode.Success Then
                    BA_Dissolve(hruOutputPath2 & "\" & polyFileName, BA_FIELD_HRUID_NC, vOutputPath)
                    BA_UpdateRequiredColumns(hruOutputPath2, vOutputFileName)
                End If

            Else
                BA_Feature2RasterGP(inFeaturesPath, outRasterPath, BA_FIELD_HRUID_CO, cellSize, snapRasterPath)
            End If
            pStepProg.Step()

            'prepare HRU log file
            Dim pHru As Hru = BA_CreateHru(TxtHruName.Text, rInputPath, vOutputPath, Nothing, _
                                           Nothing, CkNonContiguous.Checked)
            pHru.RetainSourceAttributes = CkRetainAttributes.Checked
            'add eliminate details to log
            'the maximum area of the polygons to be deleted in units selected on the form
            Dim tempArea As Double
            'the number of polygons that were eliminated
            Dim tempElimPolygons As Long
            Dim elimProcess As EliminateProcess = Nothing
            Dim polyAreaUnits As MeasurementUnit
            If RadAcres.Checked = True Then
                polyAreaUnits = MeasurementUnit.Acres
                tempArea = m_area * BA_SQKm_To_ACRE
            ElseIf RadKm.Checked = True Then
                polyAreaUnits = MeasurementUnit.SquareKilometers
                tempArea = m_area
            ElseIf RadMile.Checked = True Then
                polyAreaUnits = MeasurementUnit.SquareMiles
                tempArea = m_area * BA_SQKm_To_SQMile
            End If
            tempElimPolygons = Val(TxtNoZonesRemoved.Text)

            If RadAreaOfAoi.Checked = True Then
                elimProcess = New EliminateProcess(elim_opt, True, tempArea, polyAreaUnits, tempElimPolygons)
            Else
                elimProcess = New EliminateProcess(elim_opt, True, tempArea, CDbl(cboThreshPercnt.SelectedItem), polyAreaUnits, tempElimPolygons)
            End If
            pHru.EliminateProcess = elimProcess

            Dim parentHruName As LayerListItem = CType(LstSelectHruLayers.SelectedItem, LayerListItem)  'explicit cast

            Dim hruInputPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, parentHruName.Name)
            Dim aoi As Aoi = BA_LoadHRUFromXml(hruInputPath)
            For Each parentHru As Hru In aoi.HruList
                ' We found the hru the user selected
                If String.Compare(parentHru.Name, parentHruName.Name) = 0 Then
                    pHru.ParentHru = parentHru
                End If
            Next
            pStepProg.Step()

            If CkRetainAttributes.Checked = True And pHru.ParentHru IsNot Nothing Then
                Dim origHru As Hru = pHru.ParentHru
                Dim parentPath As String = Nothing
                Dim ruleFilePath As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, origHru.Name)
                If origHru IsNot Nothing AndAlso origHru.ParentHru IsNot Nothing Then
                    parentPath = origHru.ParentHru.FilePath & GRID
                End If
                BA_AddAttributesToHru(m_aoi.FilePath, hruOutputPath2, ruleFilePath, origHru.RuleList, parentPath)
                pStepProg.Step()
            End If

            Dim pHruList As IList(Of Hru) = New List(Of Hru)
            pHruList.Add(pHru)
            m_aoi.HruList = pHruList

            Dim xmlOutputPath As String = hruFolderPath & BA_EnumDescription(PublicPath.HruXml)
            m_aoi.Save(xmlOutputPath)
            progressDialog2.HideDialog()
            LoadHruLayers(Nothing)

            'Reload Layers in Define Zones dockable window if it's visible
            'Get handle to Define Zones dockable window so we can check visibility
            Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow
            Dim dockWinID As UID = New UIDClass()
            dockWinID.Value = My.ThisAddIn.IDs.frmHruZone
            dockWindow = My.ArcMap.DockableWindowManager.GetDockableWindow(dockWinID)
            If dockWindow.IsVisible Then
                ' Get handle to UI (form) to reload lists
                Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of frmHruZone.AddinImpl)(My.ThisAddIn.IDs.frmHruZone)
                Dim hruZoneForm As frmHruZone = dockWindowAddIn.UI
                hruZoneForm.ReloadListLayers()
            End If

            'Show()
            MessageBox.Show("New HRU " & TxtHruName.Text & " successfully created", "HRU: " & TxtHruName.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("BtnEliminate_Click() Exception: " & ex.Message)
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

    Private Sub BtnGoToMap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnGoToMap.Click
        'check if layer already added to the map
        If m_aoi Is Nothing Then
            MessageBox.Show("Please select an HRU dataset first.")
            Exit Sub
        End If

        Dim pMxDoc As IMxDocument = My.ArcMap.Document

        'remove vector layer "grid_v" from map
        'BA_RemoveLayers(pMxDoc, m_featureName)

        Dim layerIndex As Short = BA_GetLayerIndexByName(My.ArcMap.Document, m_featureName)

        If layerIndex < 0 Then
            If BA_DisplayVector(pMxDoc, m_featurePath & "\" & m_featureName) < 0 Then
                MessageBox.Show("Can't open selected HRU vector layer in ArcMap. The selected HRU dataset is invalid.")
                Exit Sub
            End If
        End If

        Dim UIDCls As ESRI.ArcGIS.esriSystem.UID = New ESRI.ArcGIS.esriSystem.UIDClass()

        ' id property of menu from Config.esriaddinx document
        UIDCls.Value = "Portland_State_University_ClassLibraryAddin_PATTool"

        Dim document As ESRI.ArcGIS.Framework.IDocument = pMxDoc
        Dim commandItem As ESRI.ArcGIS.Framework.ICommandItem = TryCast(document.CommandBars.Find(UIDCls), ESRI.ArcGIS.Framework.ICommandItem)
        If commandItem Is Nothing Then
            Exit Sub
        End If
        My.ArcMap.Application.CurrentTool = commandItem
        TxtPolyArea.Text = ""
    End Sub

    Private Sub EliminatePoly_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim tmpCtr As Integer = 5
        cboThreshPercnt.Items.Clear()
        While tmpCtr < 100
            cboThreshPercnt.Items.Add(tmpCtr)
            tmpCtr = tmpCtr + 5
        End While
        cboThreshPercnt.SelectedIndex = 0
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow
        Dim dockWinID As UID = New UIDClass()
        dockWinID.Value = My.ThisAddIn.IDs.FrmEliminatePoly
        dockWindow = My.ArcMap.DockableWindowManager.GetDockableWindow(dockWinID)
        dockWindow.Show(False)
    End Sub

    Private Sub TxtPolyArea_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtPolyArea.Validated
        If TxtPolyArea.Text <> "" Then
            m_area = TxtPolyArea.Text
            If m_selectedfield = BA_FIELD_AREA_SQMI Then
                m_area = m_area / BA_SQKm_To_SQMile 'convert area to Sq Km
            ElseIf m_selectedfield = BA_FIELD_AREA_ACRE Then
                m_area = m_area / BA_SQKm_To_ACRE
                'Else
                'do nothing
            End If
            TxtNoZonesRemoved.Text = BA_CountFeaturesSmallerThanOrEqualTo(m_featureName, m_featurePath, BA_FIELD_AREA_SQKM, m_area)
        End If
    End Sub

    Private Sub BtnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAbout.Click
        Dim toolHelpForm As FrmHelp = New FrmHelp(BA_HelpTopics.Eliminate)
        toolHelpForm.ShowDialog()
    End Sub
End Class