Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Framework
Imports System.Windows.Forms
Imports ESRI.ArcGIS.Catalog
Imports System.IO
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.GeoAnalyst
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.DataSourcesFile
Imports BAGIS_ClassLibrary

Public Class FrmSequentialId

    Dim m_aoi As Aoi
    Dim m_version As String
    Dim m_lstHruLayersItem As LayerListItem = Nothing

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ' Set AOI if already set
        Dim hruExt As HruExtension = HruExtension.GetExtension
        Dim aoi As Aoi = hruExt.aoi
        If aoi IsNot Nothing Then
            m_aoi = aoi
            TxtAoiPath.Text = m_aoi.FilePath
            ResetFormAfterPRMS()
            LoadHruLayers()
        End If

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

            '    'check AOI/BASIN status
            Dim success As BA_ReturnCode = BA_CheckAoiStatus(DataPath, My.ArcMap.Application.hWnd, My.ArcMap.Document)
            If success = BA_ReturnCode.Success Then
                BA_SetDefaultProjection(My.ArcMap.Application)
                Dim aoiName As String = BA_GetBareName(DataPath)
                m_aoi = New Aoi(aoiName, DataPath, Nothing, m_version)
                ResetFormAfterPRMS()
                LoadHruLayers()
                Show()
            End If
        Catch ex As Exception
            MessageBox.Show("BtnAOI_Click Exception: " & ex.Message)
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGxDialog)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGxObject)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFilter)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(hruExt)
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
            Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Loading HRU zone layers", "Loading...")
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

        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(dirZones)
    End Sub

    Private Sub Btnclear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btnclear.Click
        LstSelectHruLayers.SelectedIndex = -1
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub ResetFormAfterPRMS()
        LstSelectHruLayers.SelectedIndex = -1
        BtnApply.Enabled = False
        Btnclear.Enabled = False
    End Sub

    Private Sub LstSelectHruLayers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstSelectHruLayers.SelectedIndexChanged
        ' unselect previous selected item
        'If m_lstHruLayersItem IsNot Nothing Then
        '    LstSelectHruLayers.SelectedItems.Remove(m_lstHruLayersItem)
        'End If

        ' reset selected index to new value
        m_lstHruLayersItem = LstSelectHruLayers.SelectedItem

        Btnclear.Enabled = True
        If m_lstHruLayersItem Is Nothing Then
            BtnApply.Enabled = False
        Else
            BtnApply.Enabled = True
        End If
    End Sub

    Private Sub BtnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAbout.Click
        Dim toolHelpForm As FrmHelp = New FrmHelp(BA_HelpTopics.SequentialId)
        toolHelpForm.ShowDialog()
    End Sub

    Private Sub BtnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnApply.Click

        ' Create/configure a step progressor
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, LstSelectHruLayers.SelectedIndices.Count * 5)
        pStepProg.Show()
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Calculating sequential HRU ID numbers", "Calculating...")
        progressDialog2.ShowDialog()
        Dim hruGDS As IGeoDataset
        Dim tempGDS As IGeoDataset
        Dim pRasterBandColl As IRasterBandCollection
        Dim pRasterBand As IRasterBand
        Dim pTable As ITable
        Dim pField As IFieldEdit
        Dim gridField As IField
        Dim gridFields As IFields
        Dim pCursor As ICursor
        Dim pRow As IRow
        Dim tempBandColl As IRasterBandCollection
        Dim tempBand As IRasterBand
        Dim tempTable As ITable = Nothing
        Dim targetCursor As ICursor
        Dim targetRow As IRow
        Dim queryFilter As IQueryFilter = New QueryFilter()
        Try
            For Each pItem As LayerListItem In LstSelectHruLayers.SelectedItems
                Dim selectedItem As LayerListItem = CType(pItem, LayerListItem)
                pStepProg.Message = "Calculating HRU " & pItem.Name

                'Delete temporary grid if it exists
                Dim tempGrid As String = "tmpGrid"
                Dim oldGrid As String = "oldGrid"

                Dim hruFolder As String = "PleaseReturn"
                Dim hruFile As String = BA_GetBareName(selectedItem.Value, hruFolder)
                If BA_File_Exists(hruFolder & tempGrid, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                    BA_RemoveRasterFromGDB(hruFolder, tempGrid)
                End If

                'Delete old grid if it exists
                If BA_File_Exists(hruFolder & oldGrid, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                    BA_RemoveRasterFromGDB(hruFolder, oldGrid)
                End If

                'Open hru grid
                hruGDS = BA_OpenRasterFromGDB(hruFolder, hruFile)
                If hruGDS IsNot Nothing Then
                    Dim memTable As Hashtable = New Hashtable
                    Dim newId As Integer = 1
                    pRasterBandColl = CType(hruGDS, IRasterBandCollection)
                    pRasterBand = pRasterBandColl.Item(0)
                    pTable = pRasterBand.AttributeTable
                    gridFields = pTable.Fields
                    'Create new column for new id
                    Dim idxNewId As Short = pTable.FindField(BA_FIELD_NEW_HRU_ID)
                    Dim idxValue As Short = pTable.FindField(BA_FIELD_VALUE)
                    If idxNewId = -1 Then
                        pField = New Field
                        pField.Name_2 = BA_FIELD_NEW_HRU_ID
                        gridField = gridFields.Field(idxValue)
                        pField.Type_2 = gridField.Type
                        pTable.AddField(pField)
                        idxNewId = pTable.FindField(BA_FIELD_NEW_HRU_ID)
                    End If
                    'Open a cursor on the grid
                    pCursor = pTable.Update(Nothing, False)
                    pRow = pCursor.NextRow
                    'Look at each record
                    Do Until pRow Is Nothing
                        Dim oldValue As String = Convert.ToString(pRow.Value(idxValue))
                        If memTable(oldValue) Is Nothing Then
                            Dim txtItem As ReclassTextItem = New ReclassTextItem(oldValue, CStr(newId))
                            memTable(oldValue) = txtItem
                            'Populate newId column
                            pRow.Value(idxNewId) = newId
                        Else
                            Dim txtItem As ReclassTextItem = memTable(oldValue)
                            'Populate newId column
                            pRow.Value(idxNewId) = Convert.ToInt16(txtItem.OutputValue)
                        End If
                        'Store record
                        pCursor.UpdateRow(pRow)
                        'Increment id by 1
                        newId += 1
                        'Next record
                        pRow = pCursor.NextRow
                    Loop
                    Dim itemArray(memTable.Keys.Count - 1) As ReclassTextItem
                    Dim idx As Integer = 0
                    'Put Reclass items in array so we can use them for the reclass
                    For Each pKey As String In memTable.Keys
                        itemArray(idx) = memTable(pKey)
                        idx += 1
                    Next
                    pStepProg.Step()
                    Dim snapRasterPath As String = BA_GeodatabasePath(TxtAoiPath.Text, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
                    Dim success As BA_ReturnCode = BA_ReclassifyRasterFromReclassTextItems(selectedItem.Value, BA_FIELD_VALUE, _
                                                    itemArray, hruFolder & "\" & tempGrid, snapRasterPath)
                    If success = BA_ReturnCode.Success Then
                        'Copy attribute fields from old grid to new grid
                        tempGDS = BA_OpenRasterFromGDB(hruFolder, tempGrid)
                        Dim idxSource As IList(Of Integer) = New List(Of Integer)
                        Dim idxTarget As IList(Of Integer) = New List(Of Integer)
                        If tempGDS IsNot Nothing Then
                            tempBandColl = CType(tempGDS, IRasterBandCollection)
                            tempBand = tempBandColl.Item(0)
                            tempTable = tempBand.AttributeTable
                            'Start at 3 because the first three columns are OID, Value, and Count
                            For i As Integer = 3 To gridFields.FieldCount - 1
                                Dim sourceField As IField = gridFields.Field(i)
                                If i < idxNewId Then
                                    Dim targetField As IFieldEdit = New Field
                                    targetField.Name_2 = sourceField.Name
                                    targetField.Length_2 = sourceField.Length
                                    targetField.AliasName_2 = sourceField.AliasName
                                    targetField.Type_2 = sourceField.Type
                                    tempTable.AddField(targetField)
                                    idxSource.Add(i)
                                    idxTarget.Add(tempTable.FindField(sourceField.Name))
                                End If
                            Next
                            pStepProg.Step()
                            'Copy attributes from old to new grid
                            targetCursor = tempTable.Update(Nothing, False)
                            targetRow = targetCursor.NextRow
                            Do Until targetRow Is Nothing
                                queryFilter.WhereClause = BA_FIELD_NEW_HRU_ID & " = " & targetRow.Value(idxValue)
                                Dim sourceCursor As ICursor = pTable.Search(queryFilter, False)
                                Dim sourceRow As IRow = sourceCursor.NextRow
                                If sourceRow IsNot Nothing Then
                                    For nextIdx As Integer = 0 To idxSource.Count - 1
                                        Dim fld1 As Integer = idxSource(nextIdx)
                                        Dim fld2 As Integer = idxTarget(nextIdx)
                                        targetRow.Value(fld2) = sourceRow.Value(fld1)
                                    Next
                                End If
                                targetCursor.UpdateRow(targetRow)
                                targetRow = targetCursor.NextRow
                            Loop
                            pStepProg.Step()
                            'Copy old grid to different location "oldGrid"
                            success = BA_RenameRasterInGDB(hruFolder, hruFile, oldGrid)
                            If success = BA_ReturnCode.Success Then
                                'Copy new grid to standard grid location "grid"
                                success = BA_RenameRasterInGDB(hruFolder, tempGrid, hruFile)
                                If success = BA_ReturnCode.Success Then
                                    Dim vOutputPath As String = BA_StandardizePathString(hruFolder, True) & BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruVector), False)
                                    'Delete existing vector representation
                                    Dim vReturnVal As Short = BA_Remove_ShapefileFromGDB(hruFolder, BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruVector), False))
                                    If vReturnVal = 1 Then
                                        'Created updated vector representation
                                        vReturnVal = BA_Raster2PolygonShapefileFromPath(selectedItem.Value, vOutputPath, False)
                                        If vReturnVal = 1 Then
                                            'This method returns a 0 if it succeeds instead of 1
                                            If BA_AddShapeAreaToAttrib(vOutputPath) = 0 Then
                                                vReturnVal = 1
                                            Else
                                                vReturnVal = -1
                                            End If
                                        End If
                                    End If
                                    'Handle failure(s) with vector file if they occurred
                                    If vReturnVal <> 1 Then
                                        'Delete new grid so we can restore the old one
                                        BA_RemoveRasterFromGDB(hruFolder, hruFile)
                                        'Restore old grid
                                        BA_RenameRasterInGDB(hruFolder, oldGrid, hruFile)
                                        MessageBox.Show("An error occurred while trying to assign sequential ID numbers. Please restart ArcMap and try again.", "File locked", _
                                                        MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        Exit Sub
                                    End If
                                    pStepProg.Step()
                                    'Delecte old grid if user doesn't want to keep it
                                    If CkMakeCopy.Checked = False Then
                                        BA_RemoveRasterFromGDB(hruFolder, oldGrid)
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
                pStepProg.Step()
            Next
            MessageBox.Show("Sequential HRU ID numbers have been calculated ", "Sequential HRU ID numbers", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MsgBox("BtnApply_Click: " + ex.Message)
        Finally
            If progressDialog2 IsNot Nothing Then
                progressDialog2.HideDialog()
            End If
            ResetFormAfterPRMS()
            hruGDS = Nothing
            pRasterBand = Nothing
            pRasterBandColl = Nothing
            pField = Nothing
            gridField = Nothing
            gridFields = Nothing
            pCursor = Nothing
            pRow = Nothing
            tempBand = Nothing
            tempBandColl = Nothing
            tempGDS = Nothing
            tempTable = Nothing
            targetCursor = Nothing
            targetRow = Nothing
            queryFilter = Nothing

            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub
End Class