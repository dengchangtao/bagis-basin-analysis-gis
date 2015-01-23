Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports System.Windows.Forms
Imports System.IO
Imports System.Text
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Carto

''' <summary>
''' Designer class of the dockable window add-in. It contains user interfaces that
''' make up the dockable window.
''' </summary>
Public Class FrmReclassZones

    Dim m_aoi As Aoi
    Dim m_oldValuesIdx As Int16 = 0
    Dim m_newValuesIdx As Int16 = 1

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

        Private m_windowUI As FrmReclassZones

        Protected Overrides Function OnCreateChild() As System.IntPtr
            m_windowUI = New FrmReclassZones(Me.Hook)
            Return m_windowUI.Handle
        End Function

        Protected Overrides Sub Dispose(ByVal Param As Boolean)
            If m_windowUI IsNot Nothing Then
                m_windowUI.Dispose(Param)
            End If

            MyBase.Dispose(Param)
        End Sub

        ' This property allows other forms access to the UI of this form
        Protected Friend ReadOnly Property UI() As FrmReclassZones
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
            ' Reset form fields
            CboParentHru.Items.Clear()
            TxtNewHruName.Text = ""

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
                Me.Text = "Current AOI: " & m_aoi.Name
                TxtAoiPath.Text = m_aoi.FilePath
                LoadHruLayers()
                TxtHruPath.Text = m_aoi.FilePath & BA_EnumDescription(PublicPath.HruDirectory)
                Dim dockWinID As UID = New UIDClass()
                dockWinID.Value = My.ThisAddIn.IDs.FrmReclassZones
                Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow = My.ArcMap.DockableWindowManager.GetDockableWindow(dockWinID)
                dockWindow.Caption = "Current AOI: " & aoiName
                TxtNewHruName.Enabled = True
            End If
        Catch ex As Exception
            MessageBox.Show("BtnSelectAoi_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow
        Dim dockWinID As UID = New UIDClass()
        dockWinID.Value = My.ThisAddIn.IDs.FrmReclassZones
        dockWindow = My.ArcMap.DockableWindowManager.GetDockableWindow(dockWinID)
        dockWindow.Show(False)
    End Sub

    Private Sub LoadHruLayers()
        ' Get the count of zone directories so we know how many steps to put into the StepProgressor
        ' Create a DirectoryInfo of the HRU directory.
        Dim zonesDirectory As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, Nothing)
        Dim dirZones As New DirectoryInfo(zonesDirectory)
        Dim dirZonesArr As DirectoryInfo() = Nothing
        If dirZones.Exists Then
            dirZonesArr = dirZones.GetDirectories
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
                        CboParentHru.Items.Add(item)
                    End If
                Next dri
                BtnViewLayer.Enabled = True
                CboParentHru.SelectedIndex = 0
            Else
                BtnViewLayer.Enabled = False
            End If
        Else
            BtnViewLayer.Enabled = False
        End If

        'Pop error message if no zones in this aoi
        If CboParentHru.Items.Count < 1 Then
            MessageBox.Show("No HRU datasets have been generated for this AOI. Use the 'Define Zones' tool to create an HRU dataset.", "Missing HRU datasets", MessageBoxButtons.OK, MessageBoxIcon.Information)
            TxtAoiPath.Text = "Function disabled. The selected AOI has no HRU."
        End If

    End Sub

    Private Sub BtnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAbout.Click
        Dim toolHelpForm As FrmHelp = New FrmHelp(BA_HelpTopics.ReclassZones)
        toolHelpForm.ShowDialog()
    End Sub

    Public Sub LoadForm()
        Dim hruExt As HruExtension = HruExtension.GetExtension
        If hruExt.aoi IsNot Nothing Then
            m_aoi = hruExt.aoi
            TxtAoiPath.Text = m_aoi.FilePath
            LoadHruLayers()
            TxtHruPath.Text = m_aoi.FilePath & BA_EnumDescription(PublicPath.HruDirectory)
            TxtNewHruName.Enabled = True
        Else

        End If
    End Sub

    Protected Sub UnloadForm(ByVal resetAoi As Boolean)
        If resetAoi = True Then
            m_aoi = Nothing
            TxtAoiPath.Text = "AOI is not specified"
            BtnViewLayer.Enabled = False
            TxtNewHruName.Enabled = False
            BtnSelectZones.Enabled = False
            TxtSelZone.Enabled = False
            CboParentHru.Items.Clear()
        Else
            LoadHruLayers()
            TxtNewHruName.Enabled = True
        End If

        TxtNewHruName.Text = Nothing
        TxtHruPath.Text = "HRU is not specified"
        TxtSelZone.Text = Nothing
        DataGridView1.Rows.Clear()
    End Sub

    ' This event fires when the x in the upper right-hand corner of the dockableWindow is clicked or the cancel button is used
    Private Sub FrmReclassZones_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.VisibleChanged
        UnloadForm(True)
    End Sub

    Private Sub BtnViewLayer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnViewLayer.Click
        If CboParentHru.SelectedIndex > -1 Then
            Dim item As LayerListItem = CboParentHru.SelectedItem
            BA_DisplayHruZonesRaster(My.ArcMap.Document, item.Value, item.Name)
            Dim parentPath As String = "Please Return"
            Dim tmpFilePath As String = BA_GetBareName(item.Value, parentPath)
            Dim actionCode As Short = 0
            Dim vectorName As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruVector), False)
            Dim success As BA_ReturnCode = BA_AddExtentLayer(My.ArcMap.Document, parentPath & vectorName, _
                                                             Nothing, vectorName, actionCode, 1.0)
            BtnSelectZones.Enabled = True
            TxtSelZone.Enabled = True
        Else
            BtnSelectZones.Enabled = False
            TxtSelZone.Enabled = False
        End If
    End Sub

    Private Sub BtnSelectZones_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSelectZones.Click
        Dim pMxDoc As IMxDocument = My.ArcMap.Document
        If CboParentHru.SelectedIndex > -1 Then
            Dim item As LayerListItem = CboParentHru.SelectedItem

            Dim layerIndex As Short = BA_GetLayerIndexByName(My.ArcMap.Document, item.Name)

            If layerIndex < 0 Then
                MessageBox.Show("Can't access the selected zones layer in ArcMap. The selected zones dataset is invalid.")
                Exit Sub
            End If

            Dim UIDCls As ESRI.ArcGIS.esriSystem.UID = New ESRI.ArcGIS.esriSystem.UIDClass()

            ' id property of menu from Config.esriaddinx document
            UIDCls.Value = "Portland_State_University_BAGIS_H_IdentifyTool"

            Dim document As ESRI.ArcGIS.Framework.IDocument = pMxDoc
            Dim commandItem As ESRI.ArcGIS.Framework.ICommandItem = TryCast(document.CommandBars.Find(UIDCls), ESRI.ArcGIS.Framework.ICommandItem)
            If commandItem Is Nothing Then
                Exit Sub
            End If
            My.ArcMap.Application.CurrentTool = commandItem
        End If
    End Sub

    Private Sub DataGridView1_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles DataGridView1.RowsAdded
        BtnDelete.Enabled = True
        ManageGenerateButton()
    End Sub

    Private Sub DataGridView1_RowsRemoved(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsRemovedEventArgs) Handles DataGridView1.RowsRemoved
        If DataGridView1.Rows.Count < 1 Then
            BtnDelete.Enabled = False
        End If
        ManageGenerateButton()
    End Sub

    Private Sub BtnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnDelete.Click
        Dim pCollection As DataGridViewSelectedCellCollection = DataGridView1.SelectedCells
        If pCollection.Count > 0 Then
            For Each nextCell As DataGridViewCell In pCollection
                nextCell.OwningRow.Selected = True
            Next
        End If
        For Each row As DataGridViewRow In DataGridView1.SelectedRows
            DataGridView1.Rows.Remove(row)
        Next
    End Sub

    'Check for numeric values and the duplication of old values
    Private Sub DataGridView1_CellValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs)

        Dim pGrid As DataGridView = DataGridView1
        'Check for numeric values
        Dim toValue As Integer
        Dim oldCell As DataGridViewCell = pGrid.Rows(e.RowIndex).Cells(e.ColumnIndex)
        If Not Integer.TryParse(e.FormattedValue, toValue) And CStr(e.FormattedValue) <> BA_NODATA Then
            MessageBox.Show("Value must be numeric")
            If pGrid.EditingControl IsNot Nothing Then pGrid.EditingControl.Text = oldCell.Value
            e.Cancel = True
        End If
    End Sub

    Private Sub BtnGenerateHru_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnGenerateHru.Click

        ' Create/configure a step progressor
        Dim stepCount As Integer = 7
        Dim pStepProg As IStepProgressor = Nothing
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = Nothing

        Try
            Dim hruOutputPath2 As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, TxtNewHruName.Text)
            Dim hruFolderPath As String = BA_HruFolderPathFromGdbString(hruOutputPath2)
            If BA_Workspace_Exists(hruFolderPath) Then
                Dim result As DialogResult = MessageBox.Show("HRU directory already exists. Overwrite existing directory ?", "Folder exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If result = DialogResult.Yes Then
                    Dim success As BA_ReturnCode = BA_DeleteHRU(hruFolderPath, My.Document)
                    If success <> BA_ReturnCode.Success Then
                        MessageBox.Show("Unable to delete HRU '" & TxtNewHruName.Text & "'. Please restart ArcMap and try again", "Unable to delete HRU", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        BtnGenerateHru.Enabled = True
                        Exit Sub
                    End If
                    pStepProg = BA_GetStepProgressor(My.ArcMap.Application.hWnd, stepCount)
                    ' Create/configure the ProgressDialog. This automatically displays the dialog
                    progressDialog2 = BA_GetProgressDialog(pStepProg, "Generating HRUs...", "Generate HRUs")
                    pStepProg.Step()
                Else
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

            Dim pGrid As DataGridView = Me.DataGridView1
            Dim sb As StringBuilder = New StringBuilder
            Dim pTable As Hashtable = GetChangedValues()
            Dim reclassRule As RasterReclassRule = Nothing
            If pTable IsNot Nothing AndAlso pTable.Count > 0 Then
                Dim reclassItems() As ReclassItem = CopyGridToArray(pTable)
                If reclassItems IsNot Nothing AndAlso reclassItems.Length > 0 Then
                    Dim item As LayerListItem = CboParentHru.SelectedItem
                    Dim inputFolderPath As String = item.Value
                    Dim outputFolderPath As String = hruOutputPath2 & BA_EnumDescription(PublicPath.HruGrid)
                    Dim snapRasterPath As String = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
                    Dim success As BA_ReturnCode = BA_ReclassifyRasterFromTableWithNoData(inputFolderPath, BA_FIELD_VALUE, _
                                                                                reclassItems, outputFolderPath, _
                                                                                snapRasterPath)
                    If success = BA_ReturnCode.Success Then
                        reclassRule = New RasterReclassRule(item.Name, BA_FIELD_VALUE, item.Value, 0)
                        reclassRule.ReclassItems = reclassItems
                        Dim hruInputPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, item.Name)
                        Dim aoi As Aoi = BA_LoadHRUFromXml(hruInputPath)
                        Dim parentHru As Hru = Nothing
                        For Each pHru In aoi.HruList
                            ' We found the parent hru
                            If String.Compare(pHru.Name, item.Name) = 0 Then
                                parentHru = pHru
                            End If
                        Next
                        pStepProg.Step()

                        Dim rInputPath As String = hruOutputPath2 & BA_EnumDescription(PublicPath.HruGrid)
                        Dim vOutputPath As String = BA_StandardizePathString(hruOutputPath2, True) & BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruVector), False)
                        Dim vReturnVal As Short = BA_Raster2PolygonShapefileFromPath(rInputPath, vOutputPath, False)

                        If success = BA_ReturnCode.Success Then
                            pStepProg.Step()
                            ' Placeholder for rules
                            Dim rules As List(Of BAGIS_ClassLibrary.IRule) = New List(Of BAGIS_ClassLibrary.IRule)
                            Dim pHru As Hru = BA_CreateHru(TxtNewHruName.Text, rInputPath, vOutputPath, Nothing, _
                                                           rules, parentHru.AllowNonContiguousHru)
                            pHru.RetainSourceAttributes = CkRetainAttributes.Checked
                            If parentHru IsNot Nothing Then pHru.ParentHru = parentHru
                            pHru.ReclassZonesRule = reclassRule
                            pStepProg.Step()

                            If CkRetainAttributes.Checked = True And pHru.ParentHru IsNot Nothing Then
                                Dim joinPath As String = hruOutputPath2 & "\" & "tmpVat"
                                success = BA_JoinRasters(rInputPath, item.Value, joinPath)
                                If success = BA_ReturnCode.Success Then
                                    success = BA_RemoveDupAttribFields(joinPath, "_1")
                                    If success = BA_ReturnCode.Success Then
                                        Dim retVal As Short = BA_RemoveRasterFromGDB(hruOutputPath2, BA_GetBareName(BA_EnumDescription(PublicPath.HruGrid)))
                                        If retVal = 1 Then
                                            success = BA_RenameRasterInGDB(hruOutputPath2, "tmpVat", BA_GetBareName(BA_EnumDescription(PublicPath.HruGrid)))
                                        End If
                                    End If
                                End If
                                pStepProg.Step()
                            End If

                            Dim pHruList As IList(Of Hru) = New List(Of Hru)
                            pHruList.Add(pHru)
                            m_aoi.HruList = pHruList
                            Dim xmlOutputPath As String = hruFolderPath & BA_EnumDescription(PublicPath.HruXml)
                            'MessageBox.Show("7. Generating XML")
                            m_aoi.Save(xmlOutputPath)
                            progressDialog2.HideDialog()

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

                            'Cache new hru name so we can display in the msg; Name is cleared in UnloadForm subroutine
                            Dim hruName As String = TxtNewHruName.Text
                            UnloadForm(False)
                            MessageBox.Show("New HRU " & hruName & " successfully created", "HRU: " & hruName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("BtnGenerateHru_Click Exception: " & ex.Message)
        Finally
            ' Clean up step progressor
            If progressDialog2 IsNot Nothing Then progressDialog2.HideDialog()
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStepProg)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(progressDialog2)
        End Try

    End Sub

    Private Sub ManageGenerateButton()
        If DataGridView1.Rows.Count > 0 And TxtNewHruName.Text.Length > 0 Then
            BtnGenerateHru.Enabled = True
        Else
            BtnGenerateHru.Enabled = False
        End If
    End Sub

    Private Sub TxtNewHruName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtNewHruName.TextChanged
        ManageGenerateButton()
        If m_aoi IsNot Nothing Then
            TxtHruPath.Text = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, TxtNewHruName.Text)
        End If
    End Sub

    Private Sub BtnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAdd.Click
        Dim pixelVal As String = TxtSelZone.Text
        Dim oldValue As Integer
        If Integer.TryParse(pixelVal, oldValue) Then
            Dim pGrid As DataGridView = DataGridView1
            'Check to see if new value already exists in table
            For Each pRow As DataGridViewRow In pGrid.Rows
                Dim oldCell As DataGridViewCell = pRow.Cells(m_oldValuesIdx)
                If oldCell.Value = pixelVal Then
                    Dim result As DialogResult = MessageBox.Show("Zone value " & pixelVal & " already exists on the reclass table. Do you wish to overwrite it?", "Value exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If result = DialogResult.No Or result = DialogResult.Cancel Then
                        Exit Sub
                    Else
                        DataGridView1.Rows.Remove(pRow)
                        Exit For
                    End If
                End If
            Next
            Dim pArray As String() = {pixelVal, pixelVal}
            pGrid.Rows.Insert(0, pArray)
        End If

    End Sub

    Private Sub TxtSelZone_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtSelZone.TextChanged
        Dim oldValue As Integer
        If Integer.TryParse(TxtSelZone.Text, oldValue) Then
            BtnAdd.Enabled = True
            BtnUpdate.Enabled = True
        ElseIf TxtSelZone.Text = BA_NODATA Then
            BtnAdd.Enabled = False
            BtnUpdate.Enabled = True
        Else
            BtnAdd.Enabled = False
            BtnUpdate.Enabled = False
        End If
    End Sub

    Private Sub BtnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnUpdate.Click
        Dim pixelVal As String = TxtSelZone.Text
        Dim newValue As Integer
        If Integer.TryParse(TxtSelZone.Text, newValue) Then
            Dim pCollection As DataGridViewSelectedCellCollection = DataGridView1.SelectedCells
            Dim newValueCount As Int16 = 0
            If pCollection.Count > 0 Then
                For Each nextCell As DataGridViewCell In pCollection
                    If nextCell.ColumnIndex = m_newValuesIdx Then
                        nextCell.Value = pixelVal
                        newValueCount += 1
                    End If
                Next
            End If
            If newValueCount = 0 Then
                Dim sb As StringBuilder = New StringBuilder
                sb.Append("No values were selected to update. Please select" & vbCrLf)
                sb.Append("one or more new values from the table to update.")
                MessageBox.Show(sb.ToString)
            End If
        End If
    End Sub

    Private Function CopyGridToArray(ByVal changedTable As Hashtable) As ReclassItem()
        Dim pGeodataset As IGeoDataset = Nothing
        Dim pRasterBandCollection As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pAttribTable As ITable = Nothing
        Dim pCursor As ICursor = Nothing
        Dim pRow As IRow = Nothing
        Dim pFields As IFields = Nothing
        Dim idxValue As Int16 = -1
        Try
            If CboParentHru.SelectedIndex > -1 Then
                Dim item As LayerListItem = CboParentHru.SelectedItem
                Dim reclassItems() As ReclassItem = Nothing
                Dim i As Int16 = 0
                Dim filePath As String = ""
                Dim fileName As String = BA_GetBareName(item.Value, filePath)
                pGeodataset = BA_OpenRasterFromGDB(filePath, fileName)
                If pGeodataset IsNot Nothing Then
                    pRasterBandCollection = CType(pGeodataset, IRasterBandCollection)
                    pRasterBand = pRasterBandCollection.Item(0)
                    pAttribTable = pRasterBand.AttributeTable
                    If pAttribTable IsNot Nothing Then
                        idxValue = pAttribTable.Fields.FindField(BA_FIELD_VALUE)
                        pCursor = pAttribTable.Search(Nothing, False)
                        System.Array.Resize(reclassItems, pAttribTable.RowCount(Nothing))
                        pRow = pCursor.NextRow
                        Do While pRow IsNot Nothing
                            Dim oldVal As String = CStr(pRow.Value(idxValue))
                            Dim newItem As ReclassItem = Nothing
                            Dim newVal As String = changedTable.Item(oldVal)
                            If newVal IsNot Nothing Then
                                If CStr(newVal = BA_NODATA) Then
                                    newItem = New ReclassItem(oldVal, oldVal, BA_9999)
                                Else
                                    newItem = New ReclassItem(oldVal, oldVal, newVal)
                                End If
                            Else
                                newItem = New ReclassItem(oldVal, oldVal, oldVal)
                            End If
                                reclassItems(i) = newItem
                                pRow = pCursor.NextRow
                                i += 1
                        Loop
                    End If
                End If
                Return reclassItems
            End If
            Return Nothing
        Catch ex As Exception
            Debug.Print("CopyGridToTable() Exception: " & ex.Message)
            Return Nothing
        Finally
            pFields = Nothing
            pCursor = Nothing
            pRow = Nothing
            pGeodataset = Nothing
            pRasterBandCollection = Nothing
            pRasterBand = Nothing
            pAttribTable = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    'Put the values we want to change into a list
    Private Function GetChangedValues() As Hashtable
        Dim pTable As Hashtable = New Hashtable
        For Each pRow As DataGridViewRow In DataGridView1.Rows
            Dim oldCell As DataGridViewCell = pRow.Cells(m_oldValuesIdx)
            Dim newCell As DataGridViewCell = pRow.Cells(m_newValuesIdx)
            Dim oldValue As Integer
            Dim newValue As Integer
            Dim validNewValue As Boolean = False
            If Integer.TryParse(newCell.Value, newValue) Or CStr(newCell.Value) = BA_NODATA Then validNewValue = True
            If Integer.TryParse(oldCell.Value, oldValue) And validNewValue Then
                pTable.Add(oldCell.Value, newCell.Value)
            End If
        Next
        Return pTable
    End Function

    Private Sub CboParentHru_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboParentHru.SelectedIndexChanged
        Dim pMap As IMap = Nothing
        Dim pLayer As ILayer = Nothing
        Dim pDSet As IDataset = Nothing
        Try
            pMap = My.Document.FocusMap
            Dim layerDataPath As String = Nothing
            Dim selItem As LayerListItem = CType(CboParentHru.SelectedItem, LayerListItem)
            'Loop through all layers in the map
            For i As Integer = 0 To pMap.LayerCount - 1
                pLayer = pMap.Layer(i)
                'Zone datasets must be rasters; Check to make sure layer is raster and valid
                If TypeOf pLayer Is IRasterLayer AndAlso pLayer.Valid Then
                    pDSet = CType(pLayer, IDataset)
                    'Assemble the full path to the data layer
                    layerDataPath = pDSet.Workspace.PathName & "\" & pDSet.FullName.NameString
                    'Check to see if the layer path is the same as the selected layer path
                    If layerDataPath = selItem.Value Then
                        'If so, enable the zones button
                        BtnSelectZones.Enabled = True
                        TxtSelZone.Enabled = True
                        'Move selected layer to top if it isn't already
                        If i <> 0 Then
                            pMap.MoveLayer(pLayer, 0)
                        End If
                        Exit For
                    End If
                End If
            Next
        Catch ex As Exception
            Debug.Print("CboParentHru_SelectedIndexChanged Exception" & ex.Message)
        Finally
            pMap = Nothing
            pLayer = Nothing
            pDSet = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Sub

 
    Private Sub TxtSelZone_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles TxtSelZone.Validating
        Dim intValue As Integer
        If Integer.TryParse(TxtSelZone.Text, intValue) = True AndAlso intValue > 0 Then
            'Do Nothing; We have a valid integer
        Else
            MessageBox.Show("Zone value must be a positive integer", "Invalid zone value", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ' Cancel the event moving off of the control.
            e.Cancel = True
            ' Select the offending text.
            TxtSelZone.Select(0, TxtSelZone.Text.Length)
        End If
    End Sub
End Class