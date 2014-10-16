Imports ESRI.ArcGIS.DataSourcesRaster
Imports BAGIS_ClassLibrary
Imports System.Windows.Forms
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.ArcMapUI
Imports System.IO
Imports ESRI.ArcGIS.Framework
Imports System.Text
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.SpatialAnalyst
Imports ESRI.ArcGIS.GeoAnalyst

''' <summary>
''' Designer class of the dockable window add-in. It contains user interfaces that
''' make up the dockable window.
''' </summary>
Public Class FrmTimberlineTool

    Dim m_elevUnit As MeasurementUnit
    Dim m_selElevUnit As MeasurementUnit
    Dim m_dirtyFlag As Boolean

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

        Private m_windowUI As FrmTimberlineTool

        Protected Overrides Function OnCreateChild() As System.IntPtr
            m_windowUI = New FrmTimberlineTool(Me.Hook)
            Return m_windowUI.Handle
        End Function

        Protected Overrides Sub Dispose(ByVal Param As Boolean)
            If m_windowUI IsNot Nothing Then
                m_windowUI.Dispose(Param)
            End If

            MyBase.Dispose(Param)
        End Sub

        ' This property allows other forms access to the UI of this form
        Protected Friend ReadOnly Property UI() As FrmTimberlineTool
            Get
                Return m_windowUI
            End Get
        End Property

    End Class

    Public ReadOnly Property ElevUnit As MeasurementUnit
        Get
            Return m_elevUnit
        End Get
    End Property

    Public ReadOnly Property SelElevUnit As MeasurementUnit
        Get
            Return m_selElevUnit
        End Get
    End Property

    Public Property DirtyFlag As Boolean
        Get
            Return m_dirtyFlag
        End Get
        Set(value As Boolean)
            m_dirtyFlag = value
        End Set
    End Property

    Private Sub LoadTimberlineData()
        Dim rasterStats As IRasterStatistics = Nothing
        Try
            'LblTimberlineHeading.Text = "Timberline elevation (" & m_elevUnit.ToString & ")"
            Dim inputFolder As String = BA_GeodatabasePath(TxtAoiPath.Text, GeodatabaseNames.Surfaces)
            Dim inputFile As String = BA_EnumDescription(MapsFileName.filled_dem_gdb)
            rasterStats = BA_GetRasterStatsGDB(inputFolder & "\" & inputFile, 0)
            TxtElevMax.Text = Math.Round(rasterStats.Maximum)
            TxtElevMin.Text = Math.Round(rasterStats.Minimum)
            TxtElevMean.Text = Math.Round(rasterStats.Mean)
            TxtSelElev.Text = TxtElevMean.Text
        Catch ex As Exception
            Debug.Print("LoadTimberlineData Exception: " & ex.Message)
        Finally
            rasterStats = Nothing
        End Try
    End Sub

    Private Sub BtnAbout_Click(sender As System.Object, e As System.EventArgs) Handles BtnAbout.Click
        Dim toolHelpForm As FrmHelp = New FrmHelp(BA_HelpTopics.TimberlineTool)
        toolHelpForm.ShowDialog()
    End Sub

    Private Sub BtnViewHru_Click(sender As System.Object, e As System.EventArgs) Handles BtnViewHru.Click
        Try
            If CboParentHru.SelectedIndex > -1 Then
                Dim hruName As String = CStr(CboParentHru.SelectedItem)
                Dim hruGdbPath As String = BA_GetHruPathGDB(TxtAoiPath.Text, PublicPath.HruDirectory, hruName)
                'Check to see if the BAGIS-P copy of the vector exists. If not, we create it before proceeding
                If Not BA_File_Exists(hruGdbPath & BA_EnumDescription(PublicPath.HruZonesVector), WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                    CreateZonesVectorForBagisP(hruName)
                End If
                Dim demFolder As String = BA_GeodatabasePath(TxtAoiPath.Text, GeodatabaseNames.Surfaces, True)
                Dim demFile As String = BA_EnumDescription(MapsFileName.filled_dem_gdb)
                'Always display filled DEM
                BA_DisplayRasterNoBuffer(My.ArcMap.Application, demFolder & demFile, demFile, True)
                'Display hru vector
                Dim hruFolder As String = BA_GetHruPathGDB(TxtAoiPath.Text, PublicPath.HruDirectory, hruName)
                Dim actionCode As Short = 0
                Dim vectorName As String = BA_EnumDescription(PublicPath.HruZonesVector)
                Dim red As IRgbColor = New RgbColor()
                red.RGB = RGB(255, 0, 0)
                Dim success As BA_ReturnCode = BA_AddExtentLayer(My.ArcMap.Document, hruFolder & vectorName, _
                                                                 red, hruName, actionCode, 1.0)
                Dim idxHruVectorLayer As Integer = BA_GetLayerIndexByName(My.Document, hruName)
                If success = BA_ReturnCode.Success And idxHruVectorLayer > -1 Then
                    'Add labels to layer
                    Dim pFont As stdole.IFontDisp = New stdole.StdFont
                    pFont.Name = "Microsoft Sans Serif"
                    pFont.Size = 9.75
                    pFont.Bold = True
                    Dim hruVectorLayer As ILayer = My.Document.FocusMap.Layer(idxHruVectorLayer)
                    BA_AnnotateLayer(hruVectorLayer, BA_FIELD_HRU_ID, red, pFont, True)
                    BtnIdentify.Enabled = True
                    BtnSatellite.Enabled = True
                    BtnCustom.Enabled = True
                    RdoSelect.Enabled = True
                    RdoMouseClick.Enabled = True
                    UpdateGrid()
                Else
                    BtnIdentify.Enabled = False
                    BtnSatellite.Enabled = False
                    BtnCustom.Enabled = False
                    RdoSelect.Enabled = False
                    TxtSelElev.Enabled = False
                    RdoMouseClick.Enabled = False
                    DataGridView1.Rows.Clear()
                End If
            Else
                BtnIdentify.Enabled = False
                BtnSatellite.Enabled = False
                BtnCustom.Enabled = False
                RdoSelect.Enabled = False
                TxtSelElev.Enabled = False
                RdoMouseClick.Enabled = False
                DataGridView1.Rows.Clear()
            End If
        Catch ex As Exception
            MessageBox.Show("Unable to display all layers for timberline tool.", "Display Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            BtnIdentify.Enabled = False
        End Try

    End Sub

    Private Sub BtnClose_Click(sender As System.Object, e As System.EventArgs) Handles BtnClose.Click
        If m_dirtyFlag = True Then
            Dim msg As String = "You have changed elevation values but you have not saved them. "
            msg = msg & "Would you like to save these changes before closing this tool ?"
            Dim res As DialogResult = MessageBox.Show(msg, "Unsaved changes", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            If res = DialogResult.Yes Then
                BtnSave.PerformClick()
            End If
        End If
        Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow
        Dim dockWinID As UID = New UID()
        dockWinID.Value = My.ThisAddIn.IDs.FrmTimberlineTool
        dockWindow = My.ArcMap.DockableWindowManager.GetDockableWindow(dockWinID)
        dockWindow.Show(False)
        ResetForm()
    End Sub

    Private Sub BtnIdentify_Click(sender As System.Object, e As System.EventArgs) Handles BtnIdentify.Click
        Dim pMxDoc As IMxDocument = My.ArcMap.Document
        If CboParentHru.SelectedIndex > -1 Then
            Dim hruName As String = CStr(CboParentHru.SelectedItem)
            Dim layerIndex As Short = BA_GetLayerIndexByName(My.ArcMap.Document, hruName)

            If layerIndex < 0 Then
                MessageBox.Show("Can't access the selected zones layer in ArcMap. The selected zones dataset is invalid.")
                Exit Sub
            End If

            Dim UIDCls As ESRI.ArcGIS.esriSystem.UID = New ESRI.ArcGIS.esriSystem.UID()

            ' id property of menu from Config.esriaddinx document
            UIDCls.Value = "Portland_State_University_BAGIS_P_IdentifyTool"

            Dim document As ESRI.ArcGIS.Framework.IDocument = pMxDoc
            Dim commandItem As ESRI.ArcGIS.Framework.ICommandItem = TryCast(document.CommandBars.Find(UIDCls), ESRI.ArcGIS.Framework.ICommandItem)
            If commandItem Is Nothing Then
                Exit Sub
            End If
            My.ArcMap.Application.CurrentTool = commandItem
        End If
    End Sub

    Private Sub LoadHruLayers()
        CboParentHru.Items.Clear()
        ' Create a DirectoryInfo of the HRU directory.
        Dim zonesDirectory As String = BA_GetHruPath(TxtAoiPath.Text, PublicPath.HruDirectory, Nothing)
        Dim dirZones As New DirectoryInfo(zonesDirectory)
        Dim dirZonesArr As DirectoryInfo() = Nothing
        If dirZones.Exists Then
            dirZonesArr = dirZones.GetDirectories
        End If
        If dirZonesArr IsNot Nothing Then
            Dim item As String
            For Each dri In dirZonesArr
                Dim hruFilePath As String = BA_GetHruPathGDB(TxtAoiPath.Text, PublicPath.HruDirectory, dri.Name) & BA_EnumDescription(PublicPath.HruGrid)
                Dim hruXmlFilePath As String = BA_GetHruPath(TxtAoiPath.Text, PublicPath.HruDirectory, dri.Name) & BA_EnumDescription(PublicPath.HruXml)
                ' Add hru to the list if the grid exists
                If BA_File_Exists(hruFilePath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) And _
                   BA_File_ExistsWindowsIO(hruXmlFilePath) Then
                    'Assume hru is discrete raster since we create it to be so
                    item = dri.Name
                    CboParentHru.Items.Add(item)
                End If
            Next dri
        End If
        If CboParentHru.Items.Count > 0 Then
            CboParentHru.SelectedIndex = 0
        End If
    End Sub

    Private Sub CreateZonesVectorForBagisP(ByVal selHruName As String)
        ' Create/configure a step progressor
        Dim pStepProg As IStepProgressor = Nothing
        Dim progressDialog2 As IProgressDialog2 = Nothing
        Try
            pStepProg = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 4)
            progressDialog2 = BA_GetProgressDialog(pStepProg, "Creating copy of zones layer for BAGIS-P...", "Copying zones layer for BAGIS-P")
            pStepProg.Show()
            progressDialog2.ShowDialog()
            pStepProg.Step()
            Dim hruGdbPath As String = BA_GetHruPathGDB(TxtAoiPath.Text, PublicPath.HruDirectory, selHruName)
            'Convert raster grid to 'grid_zones_v' shapefile
            Dim retVal As Short = BA_Raster2PolygonShapefileFromPath(hruGdbPath & BA_EnumDescription(PublicPath.HruGrid), hruGdbPath & BA_EnumDescription(PublicPath.HruZonesVector), False)
            'If successful, add HRU_ID column and copy values from 'grid_code' column
            If retVal = 1 Then
                Dim success As BA_ReturnCode = BA_CreateHruIdField(hruGdbPath, BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruZonesVector), False))
            End If
        Catch ex As Exception
            Debug.Print("CreateZonesVectorForBagisP" & ex.Message)
        Finally
            'The step progressor will be undefined if the cancel button was clicked
            If pStepProg IsNot Nothing Then
                pStepProg.Hide()
                pStepProg = Nothing
                progressDialog2.HideDialog()
                progressDialog2 = Nothing
            End If
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Sub

    Private Sub UpdateGrid()
        DataGridView1.Rows.Clear()
        If CboParentHru.SelectedIndex > -1 Then
            Dim hruName As String = CStr(CboParentHru.SelectedItem)
            Dim hruFolder As String = BA_GetHruPathGDB(TxtAoiPath.Text, PublicPath.HruDirectory, hruName)
            Dim vectorName As String = BA_GetBareName(BA_EnumDescription(PublicPath.HruZonesVector))
            Dim pFeatureClass As IFeatureClass = Nothing
            Dim pFeature As IFeature = Nothing
            Dim pCursor As IFeatureCursor = Nothing
            'Dim pDataStatistics As IDataStatistics = New DataStatistics
            'Dim uniqueValuesEnum As IEnumerator
            Try
                pFeatureClass = BA_OpenFeatureClassFromGDB(hruFolder, vectorName)
                If pFeatureClass IsNot Nothing Then
                    Dim idxHruId As Integer = pFeatureClass.FindField(BA_FIELD_HRU_ID)
                    Dim idxElev As Integer = pFeatureClass.FindField(BA_FIELD_TIMBER_ELEV)
                    Dim idList As IList(Of Integer) = New List(Of Integer)
                    pCursor = pFeatureClass.Search(Nothing, False)
                    pFeature = pCursor.NextFeature
                    Do Until pFeature Is Nothing
                        Dim pRow As New DataGridViewRow
                        pRow.CreateCells(DataGridView1)
                        Dim hruId As Integer = Convert.ToInt32(pFeature.Value(idxHruId))
                        'We need to keep track of "used" hru Id's in case we have non-contiguous HRU's
                        If Not idList.Contains(hruId) Then
                            idList.Add(hruId)
                            pRow.Cells(0).Value = hruId
                            If idxElev > -1 Then
                                If Not IsDBNull(pFeature.Value(idxElev)) Then
                                    pRow.Cells(1).Value = Convert.ToDouble(pFeature.Value(idxElev))
                                End If
                            Else
                                'Setting the default timberline to 0
                                pRow.Cells(1).Value = 0
                            End If
                            DataGridView1.Rows.Add(pRow)
                        End If
                        pFeature = pCursor.NextFeature
                    Loop
                    'initialize properties for the dataStatistics interface
                    'pDataStatistics.Field = BA_FIELD_HRU_ID
                    'pDataStatistics.Cursor = pCursor
                    ''Get the result statistics; Using UniqueValues in case we have non-contiguous HRU's where > 1 polygon has the same HRU ID
                    'uniqueValuesEnum = pDataStatistics.UniqueValues
                    'Do Until uniqueValuesEnum.MoveNext = False
                    '    Dim pRow As New DataGridViewRow
                    '    pRow.CreateCells(DataGridView1)
                    '    pRow.Cells(0).Value = Convert.ToString(uniqueValuesEnum.Current)
                    '    'Setting the default timberline to 0
                    '    pRow.Cells(1).Value = 0
                    '    DataGridView1.Rows.Add(pRow)
                    'Loop
                    'Sort by HRU_ID
                    Dim sortCol As DataGridViewColumn = DataGridView1.Columns(0)
                    DataGridView1.Sort(sortCol, System.ComponentModel.ListSortDirection.Ascending)
                    DataGridView1.ClearSelection()
                    DataGridView1.CurrentCell = Nothing
                    BtnClearSelection.Enabled = True
                    BtnSave.Enabled = True
                    m_dirtyFlag = False
                End If
            Catch ex As Exception
                Debug.Print("UpdateGrid Exception: " & ex.Message)
            Finally
                pFeatureClass = Nothing
                pFeature = Nothing
                pCursor = Nothing
                'uniqueValuesEnum = Nothing
                GC.WaitForPendingFinalizers()
                GC.Collect()
            End Try
        End If
    End Sub

    Private Sub BtnSave_Click(sender As System.Object, e As System.EventArgs) Handles BtnSave.Click
        Dim pFeatureClass As IFeatureClass = Nothing
        Dim updateCursor As IFeatureCursor = Nothing
        Dim pFeature As IFeature = Nothing
        BtnSave.Enabled = False
        BtnClose.Enabled = False
        Try
            If CboParentHru.SelectedIndex > -1 Then
                Dim hruName As String = CStr(CboParentHru.SelectedItem)
                Dim hruFolder As String = BA_GetHruPathGDB(TxtAoiPath.Text, PublicPath.HruDirectory, hruName)
                Dim vectorName As String = BA_GetBareName(BA_EnumDescription(PublicPath.HruZonesVector))
                pFeatureClass = BA_OpenFeatureClassFromGDB(hruFolder, vectorName)
                If pFeatureClass IsNot Nothing Then
                    'Get column index TIMBER_ELEV; If it doesn't exist, add it
                    Dim idxElev As Integer = pFeatureClass.Fields.FindField(BA_FIELD_TIMBER_ELEV)
                    If idxElev < 0 Then
                        Dim pFieldElev As IFieldEdit = New Field
                        With pFieldElev
                            .Type_2 = esriFieldType.esriFieldTypeDouble
                            .Name_2 = BA_FIELD_TIMBER_ELEV
                        End With
                        pFeatureClass.AddField(pFieldElev)
                        idxElev = pFeatureClass.FindField(BA_FIELD_TIMBER_ELEV)
                    End If

                    'Put all the HRU ID/Elevation values in a Hashtable to access when we are updating the FeatureClass
                    Dim dgTable As Hashtable = New Hashtable
                    For Each dgRow As DataGridViewRow In DataGridView1.Rows
                        Dim dgId As String = Convert.ToString(dgRow.Cells(0).Value)
                        Dim dgElev As Double = Convert.ToDouble(dgRow.Cells(1).Value)
                        'Check to make sure the units are the same as filled_dem; Convert if they aren't
                        If m_elevUnit <> m_selElevUnit Then
                            If m_elevUnit = MeasurementUnit.Meters And m_selElevUnit = MeasurementUnit.Feet Then
                                'Convert from feet to meters
                                dgElev = dgElev * BA_FEET_TO_METERS
                            ElseIf m_elevUnit = MeasurementUnit.Feet And m_selElevUnit = MeasurementUnit.Meters Then
                                'Convert from Meters to feet
                                dgElev = dgElev * BA_METERS_TO_FEET
                            End If
                        End If
                        dgTable(dgId) = Math.Round(dgElev)
                    Next

                    'Open an update cursor and update the TIMBER_ELEV column with the values from the grid
                    updateCursor = pFeatureClass.Update(Nothing, True)
                    pFeature = updateCursor.NextFeature
                    Dim idxHruId As Integer = pFeatureClass.FindField(BA_FIELD_HRU_ID)
                    Do Until pFeature Is Nothing
                        Dim fHruId As String = Convert.ToString(pFeature.Value(idxHruId))
                        Dim fElev As Double = dgTable(fHruId)
                        pFeature.Value(idxElev) = fElev
                        updateCursor.UpdateFeature(pFeature)
                        pFeature = updateCursor.NextFeature
                    Loop
                    'Reset dirty flag
                    m_dirtyFlag = False
                    'Inform user that the update was successful
                    MessageBox.Show(dgTable.Keys.Count & " HRU timberline values have been successfully updated.", "Timberline values updated", _
                                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
        Catch ex As Exception
            Debug.Print("BtnSave_Click Exception: " & ex.Message)
        Finally
            pFeatureClass = Nothing
            updateCursor = Nothing
            pFeature = Nothing
            BtnSave.Enabled = True
            BtnClose.Enabled = True
        End Try
    End Sub

    Private Sub BtnSatellite_Click(sender As System.Object, e As System.EventArgs) Handles BtnSatellite.Click
        BtnSatellite.Enabled = False
        'When ArcGIS 10 adds the satellite layer, this is the layer name
        Dim layerName As String = "Basemap"
        Dim pMap As IMap = My.Document.FocusMap
        If BtnIdentify.Enabled = True And CboParentHru.SelectedIndex > -1 Then
            Dim idxSatellite As Integer = BA_GetLayerIndexByName(My.Document, layerName)
            If idxSatellite > -1 Then
                Dim layer As ILayer = pMap.Layer(idxSatellite)
                pMap.DeleteLayer(layer)
            Else
                'Display satellite view if desired
                Dim layerFile As ILayerFile = New LayerFile
                'How to access maps from ArcGIS.com
                'http://blogs.esri.com/esri/arcgis/2010/11/30/programmatically-working-with-packages-and-web-maps-new-at-10-sp1/
                'Bing maps road
                'layerFile.Open("http://www.arcgis.com/sharing/content/items/b6969de2b84d441692f5bb8792e65d1f/item.pkinfo")
                'Bing maps hybrid
                'layerFile.Open("http://www.arcgis.com/sharing/content/items/71d6d656cb2a4ded8fce35982ebdff25/item.pkinfo")
                'Bing maps aerial
                layerFile.Open("http://www.arcgis.com/sharing/content/items/25c3f49d4ce3451e8a7f5b5aebccab48/item.pkinfo")
                Dim layer As ILayer = layerFile.Layer
                pMap.AddLayer(layer)
                'Make sure the satellite layer is under the hru layer
                Dim hruName As String = CStr(CboParentHru.SelectedItem)
                Dim idxHru As Integer = BA_GetLayerIndexByName(My.Document, hruName)
                pMap.MoveLayer(layer, idxHru)
            End If
            'refresh the active view
            My.Document.ActiveView.Refresh()
        End If
        BtnSatellite.Enabled = True
    End Sub

    Private Function BatchWarningMessage() As Boolean
        If DataGridView1.SelectedCells.Count > 0 Then
            Dim msg As String = "You have asked to set the elevation value to a single value for all selected HRU ID's."
            msg = msg & "This will overwrite any existing elevation values for these HRU ID's on the grid."
            msg = msg & vbCrLf & "Do you wish to continue ?"
            Dim result As DialogResult = MessageBox.Show(msg, "Overwrite values", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.Yes Then
                Return True
            Else
                Return False
            End If
        Else
            'If no cells are selected, don't need to show the warning message
            Return True
        End If
    End Function

    Private Sub DisableIdentifyTool()
        BtnIdentify.Enabled = False
        TxtHruId.Text = Nothing
        TxtElev.Text = Nothing
        My.ArcMap.Application.CurrentTool = Nothing
    End Sub

    Private Sub RdoSelect_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles RdoSelect.CheckedChanged
        If RdoSelect.Checked = True Then
            DisableIdentifyTool()
            BtnUpdate.Enabled = True
            TxtSelElev.Enabled = True
        End If
    End Sub

    Private Sub BtnUpdate_Click(sender As System.Object, e As System.EventArgs) Handles BtnUpdate.Click
        If DataGridView1.SelectedCells.Count > 0 Then
            If BatchWarningMessage() = False Then
                Exit Sub
            Else
                Dim pVal As Double = CDbl(TxtSelElev.Text)
                For Each pCell As DataGridViewCell In DataGridView1.SelectedCells
                    If pCell.ColumnIndex = 1 Then
                        pCell.Value = Math.Round(pVal)
                    End If
                Next
                m_dirtyFlag = True
                TxtSelElev.Enabled = True
            End If
        End If
    End Sub

    Private Sub RdoMouseClick_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles RdoMouseClick.CheckedChanged
        If RdoMouseClick.Checked = True Then
            Dim hruName As String = CStr(CboParentHru.SelectedItem)
            Dim idxHruVectorLayer As Integer = BA_GetLayerIndexByName(My.Document, hruName)
            Dim idxDemLayer As Integer = BA_GetLayerIndexByName(My.ArcMap.Document, BA_EnumDescription(MapsFileName.filled_dem_gdb))
            If idxHruVectorLayer > -1 And idxDemLayer > -1 Then
                BtnIdentify.Enabled = True
            End If
            TxtSelElev.Enabled = False
            BtnUpdate.Enabled = False
        End If
    End Sub

    Private Sub BtnClearSelection_Click(sender As System.Object, e As System.EventArgs) Handles BtnClearSelection.Click
        DataGridView1.ClearSelection()
        DataGridView1.CurrentCell = Nothing
    End Sub

    Private Sub LoadElevationUnits()
        'We only care about elevation units so pass in Nothing for the others
        BA_SetMeasurementUnitsForAoi(TxtAoiPath.Text, Nothing, Nothing, m_elevUnit, _
                                     Nothing, Nothing)
        If m_elevUnit = MeasurementUnit.Missing Then
            'Here we have to pass in something so the irrelevant units don't show up on the form
            Dim aoiName As String = BA_GetBareName(TxtAoiPath.Text)
            Dim bagisPExt As BagisPExtension = BagisPExtension.GetExtension
            Dim pAoi As Aoi = New Aoi(aoiName, TxtAoiPath.Text, Nothing, bagisPExt.version)
            Dim frmDataUnits As FrmDataUnits = New FrmDataUnits(pAoi, SlopeUnit.Degree, m_elevUnit, MeasurementUnit.Inches)
            frmDataUnits.ShowDialog()
            'Update with changes
            BA_SetMeasurementUnitsForAoi(TxtAoiPath.Text, Nothing, Nothing, m_elevUnit, _
                                         Nothing, Nothing)
        End If
        m_selElevUnit = m_elevUnit
        If m_elevUnit = MeasurementUnit.Meters Then
            RdoMeters.Checked = True
            RdoFeet.Checked = False
        ElseIf m_elevUnit = MeasurementUnit.Feet Then
            RdoMeters.Checked = False
            RdoFeet.Checked = True
        End If
    End Sub

    Private Sub RdoFeet_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles RdoFeet.CheckedChanged
        If RdoFeet.Checked Then
            If m_selElevUnit <> MeasurementUnit.Feet Then
                'We need to convert from meters to feet
                Dim minElev As Double = CDbl(TxtElevMin.Text)
                Dim maxElev As Double = CDbl(TxtElevMax.Text)
                Dim meanElev As Double = CDbl(TxtElevMean.Text)
                minElev = minElev * BA_METERS_TO_FEET
                maxElev = maxElev * BA_METERS_TO_FEET
                meanElev = meanElev * BA_METERS_TO_FEET
                TxtElevMin.Text = Math.Round(minElev)
                TxtElevMax.Text = Math.Round(maxElev)
                TxtElevMean.Text = Math.Round(meanElev)
                TxtSelElev.Text = TxtElevMean.Text
                'Update the values on the grid
                For Each pRow As DataGridViewRow In DataGridView1.Rows
                    Dim newValue As Double = pRow.Cells(1).Value * BA_METERS_TO_FEET
                    pRow.Cells(1).Value = Math.Round(newValue)
                Next
            End If
            m_selElevUnit = MeasurementUnit.Feet
        End If
    End Sub

    Private Sub RdoMeters_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles RdoMeters.CheckedChanged
        If RdoMeters.Checked Then
            If m_selElevUnit <> MeasurementUnit.Meters Then
                'We need to convert from meters to feet
                Dim minElev As Double = CDbl(TxtElevMin.Text)
                Dim maxElev As Double = CDbl(TxtElevMax.Text)
                Dim meanElev As Double = CDbl(TxtElevMean.Text)
                minElev = minElev * BA_FEET_TO_METERS
                maxElev = maxElev * BA_FEET_TO_METERS
                meanElev = meanElev * BA_FEET_TO_METERS
                TxtElevMin.Text = Math.Round(minElev)
                TxtElevMax.Text = Math.Round(maxElev)
                TxtElevMean.Text = Math.Round(meanElev)
                TxtSelElev.Text = TxtElevMean.Text
                'Update the values on the grid
                For Each pRow As DataGridViewRow In DataGridView1.Rows
                    Dim newValue As Double = pRow.Cells(1).Value * BA_FEET_TO_METERS
                    pRow.Cells(1).Value = Math.Round(newValue)
                Next
            End If
            m_selElevUnit = MeasurementUnit.Meters
        End If
    End Sub

    Private Sub TxtSelElev_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles TxtSelElev.Validating
        Dim res As Decimal
        If Decimal.TryParse(TxtSelElev.Text, res) = False Then
            MessageBox.Show("Timberline elevation needs to be a numeric value", "Numeric value required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        Else
            If BatchWarningMessage() = False Then
                Exit Sub
            Else
                Dim pVal As Double = CDbl(TxtSelElev.Text)
                For Each pCell As DataGridViewCell In DataGridView1.SelectedCells
                    If pCell.ColumnIndex = 1 Then
                        pCell.Value = Math.Round(pVal)
                    End If
                Next
                m_dirtyFlag = True
            End If
        End If
    End Sub

    Private Sub BtnSelectAoi_Click(sender As System.Object, e As System.EventArgs) Handles BtnSelectAoi.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim DataPath As String
        Dim pFilter As IGxObjectFilter = New GxFilterContainers
        Dim bagisPExt As BagisPExtension = BagisPExtension.GetExtension

        Try
            'TestSort()
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
                Dim pAoi As Aoi = New Aoi(aoiName, DataPath, Nothing, bagisPExt.version)
                TxtAoiPath.Text = pAoi.FilePath
                Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow = GetTimberlineDockableWindow()
                dockWindow.Caption = "Current AOI: " & aoiName
                bagisPExt.aoi = pAoi
                RdoMeters.Enabled = True
                RdoFeet.Enabled = True
                LoadHruLayers()
                LoadElevationUnits()
                LoadTimberlineData()
                RdoMouseClick.Enabled = False
                RdoMouseClick.Checked = True
                ClearGrid()
            End If
        Catch ex As Exception
            MessageBox.Show("BtnSelectAoi_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub CboParentHru_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles CboParentHru.SelectedIndexChanged
        If CboParentHru.SelectedIndex > -1 Then
            BtnViewHru.Enabled = True
        End If
    End Sub

    Private Sub ClearGrid()
        DataGridView1.Rows.Clear()
        BtnClearSelection.Enabled = False
        BtnSave.Enabled = False
        BtnIdentify.Enabled = False
        BtnCustom.Enabled = False
        RdoSelect.Enabled = False
        TxtSelElev.Enabled = False
    End Sub

    Public Sub ResetForm()
        m_elevUnit = MeasurementUnit.Missing
        m_selElevUnit = MeasurementUnit.Missing
        TxtAoiPath.Text = ""
        Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow = GetTimberlineDockableWindow()
        dockWindow.Caption = "Current AOI: (Not selected)"
        Dim bagisPExt As BagisPExtension = BagisPExtension.GetExtension
        bagisPExt.aoi = Nothing
        RdoMeters.Enabled = False
        RdoMeters.Checked = False
        RdoFeet.Enabled = False
        RdoFeet.Checked = False
        CboParentHru.Items.Clear()
        BtnViewHru.Enabled = False
        TxtElevMax.Text = ""
        TxtElevMean.Text = ""
        TxtElevMin.Text = ""
        BtnSatellite.Enabled = False
        BtnCustom.Enabled = False
        BtnUpdate.Enabled = False
        DisableIdentifyTool()
        RdoMouseClick.Checked = False
        RdoSelect.Checked = False
        TxtSelElev.Text = "0"
        ClearGrid()
    End Sub

    Private Function GetTimberlineDockableWindow() As ESRI.ArcGIS.Framework.IDockableWindow
        Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow
        Dim dockWinID As ESRI.ArcGIS.esriSystem.UID = New ESRI.ArcGIS.esriSystem.UID()
        dockWinID.Value = My.ThisAddIn.IDs.FrmTimberlineTool
        dockWindow = My.ArcMap.DockableWindowManager.GetDockableWindow(dockWinID)
        Return dockWindow
    End Function

    Private Sub BtnCustom_Click(sender As System.Object, e As System.EventArgs) Handles BtnCustom.Click

        Dim pFilter As IGxObjectFilter = New GxFilterDatasets
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim bObjectSelected As Boolean
        Dim importDone As Boolean = False

        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select a GIS dataset to add to the map"
            .ObjectFilter = pFilter
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With
        If bObjectSelected = False Then Exit Sub

        'get the name of the selected folder
        Dim pGxDataset As IGxDataset = pGxObject.Next
        Dim pDatasetName As IDatasetName = pGxDataset.DatasetName
        Dim Data_Path As String = pDatasetName.WorkspaceName.PathName
        Dim Data_Name As String = pDatasetName.Name
        Dim data_type As Object = pDatasetName.Type
        Dim data_type_code As Integer '1. shapefile, 2. Raster, 0. Unsupported format

        'Set Data Type Name from Data Type
        Select Case data_type
            Case 4, 5 'shapefile
                data_type_code = 1

            Case 12, 13 'raster
                data_type_code = 2

            Case Else 'unsupported format
                data_type_code = 0
        End Select

        'pad a backslash to the path if it doesn't have one.
        If Data_Path(Len(Data_Path) - 1) <> "\" Then Data_Path = Data_Path & "\"
        Dim data_fullname As String = Data_Path & Data_Name
        If Len(Trim(data_fullname)) = 0 Then Exit Sub 'user cancelled the action

        Dim pLayer As ILayer = Nothing
        Dim pTempFClass As IFeatureClass = Nothing
        Dim pTempFCursor As IFeatureCursor = Nothing
        Dim pTempFeature As IFeature = Nothing
        Dim pAoiFClass As IFeatureClass = Nothing
        Dim pAoiFCursor As IFeatureCursor = Nothing
        Dim pAoiFeature As IFeature = Nothing
        Dim pSFilter As ISpatialFilter = New SpatialFilter
        Dim pRasterDS As IGeoDataset = Nothing
        Dim pAoiGeo As IGeometry
        Dim pCollection As IRasterBandCollection
        Dim pRasterBand As IRasterBand
        Dim pRasterProps As IRasterProps
        Dim pRasterEnv As IEnvelope
        Dim relOp As IRelationalOperator

        Try
            Dim pMap As IMap = My.Document.ActiveView.FocusMap
            Dim hruName As String = CStr(CboParentHru.SelectedItem)

            'get the aoi boundary geometry
            pAoiFClass = BA_OpenFeatureClassFromGDB(BA_GeodatabasePath(TxtAoiPath.Text, GeodatabaseNames.Aoi, True), BA_EnumDescription(AOIClipFile.AOIExtentCoverage))
            pAoiFCursor = pAoiFClass.Search(Nothing, False)
            pAoiFeature = pAoiFCursor.NextFeature
            pAoiGeo = pAoiFeature.Shape
            Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(data_fullname)

            'Adding a vector
            If data_type_code = 1 Then
                'Open the selected feature class so we can check the geometry against the aoi
                If wType = WorkspaceType.Geodatabase Then
                    pTempFClass = BA_OpenFeatureClassFromGDB(pDatasetName.WorkspaceName.PathName, Data_Name)
                Else
                    pTempFClass = BA_OpenFeatureClassFromFile(pDatasetName.WorkspaceName.PathName, Data_Name)
                End If

                'create a spatial filter for checking if the selected vector is within the aoi boundary
                With pSFilter
                    .Geometry = pAoiGeo
                    .GeometryField = BA_FIELD_SHAPE
                    .SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects
                End With

                'apply the spatial filter
                pTempFCursor = pTempFClass.Search(pSFilter, False)
                'get the first (only feature)
                pTempFeature = pTempFCursor.NextFeature

                If pTempFeature Is Nothing Then
                    MessageBox.Show("Warning: The layer you are trying to add does not overlap with the selected AOI and may not be visible.", "No overlap", _
                                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                BA_DisplayVector(My.Document, data_fullname, Data_Name, 0, 1)
                'Make sure the new layer is under the hru layer
                Dim idxHru As Integer = BA_GetLayerIndexByName(My.Document, hruName)
                Dim idxNewLayer As Integer = BA_GetLayerIndexByName(My.Document, Data_Name)
                pLayer = pMap.Layer(idxNewLayer)
                pMap.MoveLayer(pLayer, idxHru)
            ElseIf data_type_code = 2 Then
                'Open the selected raster so we can check the geometry against the aoi
                If wType = WorkspaceType.Geodatabase Then
                    pRasterDS = BA_OpenRasterFromGDB(pDatasetName.WorkspaceName.PathName, Data_Name)
                Else
                    pRasterDS = BA_OpenRasterFromFile(pDatasetName.WorkspaceName.PathName, Data_Name)
                End If
                pCollection = CType(pRasterDS, IRasterBandCollection)
                pRasterBand = pCollection.Item(0)
                'Work our way down to the IRasterProps object to get the IEnvelope
                pRasterProps = CType(pRasterBand, IRasterProps)
                pRasterEnv = pRasterProps.Extent
                'Use the IRelationalOperator interface to see if the raster IEnvelope overlaps the AOI IEnvelope
                relOp = CType(pRasterEnv, IRelationalOperator)
                If relOp.Disjoint(pAoiGeo) = True Then
                    MessageBox.Show("Warning: The layer you are trying to add does not overlap with the selected AOI and may not be visible.", "No overlap", _
                                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                BA_DisplayRasterNoBuffer(My.ThisApplication, data_fullname, Data_Name, False)
            End If

        Catch ex As Exception
            Debug.Print("BtnCustom_Click Exception: " & ex.Message)
        Finally
            pLayer = Nothing
            pTempFClass = Nothing
            pTempFCursor = Nothing
            pTempFeature = Nothing
            pAoiFClass = Nothing
            pAoiFCursor = Nothing
            pAoiFeature = Nothing
            pAoiGeo = Nothing
            pSFilter = Nothing
            pRasterDS = Nothing
            pCollection = Nothing
            pRasterBand = Nothing
            pRasterProps = Nothing
            pRasterEnv = Nothing
            relOp = Nothing
        End Try

    End Sub
End Class