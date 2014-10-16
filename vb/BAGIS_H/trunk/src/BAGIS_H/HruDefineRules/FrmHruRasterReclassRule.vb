Imports System.Windows.Forms
Imports System.Drawing
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.DataSourcesFile
Imports ESRI.ArcGIS.GeoAnalyst
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.CatalogUI
Imports System.IO
Imports BAGIS_ClassLibrary

Public Class FrmHruRasterReclassRule

    Dim m_lstRaster As ListBox = New ListBox
    Dim m_lstDem As ListBox = New ListBox
    Dim m_lstPRISM As ListBox = New ListBox
    Dim m_lstHru As ListBox = New ListBox
    Dim m_isSlice As Boolean
    Dim m_AoiFolder As String
    Dim m_filePath As String
    Dim m_fileName As String
    Dim m_rasterReclassCaption As String = "Raster reclassification"
    Dim m_rasterReclassHeight As Integer = 515
    Dim m_rasterReclassWidth As Integer = 435
    Dim m_sliceCaption As String = "Raster slice"
    Dim m_sliceHeight As Integer = 330
    Dim m_sliceWidth As Integer = 435
    Private idxFromValue As Integer = 0
    Private idxOutputValue As Integer = 1


    Public Sub New(ByVal isSlice As Boolean, ByVal lstRasters As ListBox, _
                   ByVal lstDem As ListBox, ByVal lstPrism As ListBox, ByVal lstHru As ListBox, _
                   ByVal resetLists As Boolean, ByVal sender As System.Object, ByVal e As System.EventArgs)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        m_isSlice = isSlice
        Dim hruExt As HruExtension = HruExtension.GetExtension
        Dim aoi As Aoi = hruExt.aoi
        Dim aoiFolderPath As String = Nothing
        If aoi IsNot Nothing Then m_AoiFolder = aoi.FilePath
        TxtRuleId.Text = Nothing
        If resetLists = True Then
            m_lstRaster.Items.Clear()
            m_lstRaster.Items.AddRange(lstRasters.Items)
            m_lstDem.Items.Clear()
            For i = 0 To lstDem.Items.Count - 1 Step 1
                Dim nextItem As LayerListItem = lstDem.Items(i)
                If nextItem.LayerType = LayerType.Raster Then
                    m_lstDem.Items.Add(nextItem)
                End If
            Next
            m_lstPRISM.Items.Clear()
            m_lstPRISM.Items.AddRange(lstPrism.Items)
            m_lstHru.Items.Clear()
            m_lstHru.Items.AddRange(lstHru.Items)
            'LstAoiRasterLayers.Items.Clear()
            'LstAoiRasterLayers.Items.AddRange(m_lstRaster.Items)
            RdoRaster.Checked = True
            RdoRaster_Click(sender, e)
        End If
        LstAoiRasterLayers.ClearSelected()
        SetFormType(m_isSlice)
    End Sub

    Private Sub RdoRaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RdoRaster.Click
        LstAoiRasterLayers.Items.Clear()
        If m_isSlice = True Then
            ' load all items
            LstAoiRasterLayers.Items.AddRange(m_lstRaster.Items)
            ClearStatistics()
        Else
            ' load only discrete rasters
            For Each item As LayerListItem In m_lstRaster.Items
                If item.IsDiscrete = True Then
                    LstAoiRasterLayers.Items.Add(item)
                End If
            Next
            ClearReclass(Nothing, Nothing)
        End If
    End Sub

    Private Sub RdoDem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RdoDem.Click
        LstAoiRasterLayers.Items.Clear()
        If m_isSlice = True Then
            ' load all items
            LstAoiRasterLayers.Items.AddRange(m_lstDem.Items)
            ClearStatistics()
        Else
            ' load only discrete rasters
            For Each item As LayerListItem In m_lstDem.Items
                If item.IsDiscrete = True Then
                    LstAoiRasterLayers.Items.Add(item)
                End If
            Next
            ClearReclass(Nothing, Nothing)
        End If
    End Sub


    Private Sub RdoPrism_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RdoPrism.Click
        LstAoiRasterLayers.Items.Clear()
        If m_isSlice = True Then
            ' load all items
            LstAoiRasterLayers.Items.AddRange(m_lstPRISM.Items)
            ClearStatistics()
        Else
            ' load only discrete rasters
            For Each item As LayerListItem In m_lstPRISM.Items
                If item.IsDiscrete = True Then
                    LstAoiRasterLayers.Items.Add(item)
                End If
            Next
            ClearReclass(Nothing, Nothing)
        End If
    End Sub

    Private Sub RdoHru_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RdoHru.Click
        LstAoiRasterLayers.Items.Clear()
        If m_isSlice = True Then
            ' load all items
            LstAoiRasterLayers.Items.AddRange(m_lstHru.Items)
            ClearStatistics()
        Else
            ' load only discrete rasters
            For Each item As LayerListItem In m_lstHru.Items
                If item.IsDiscrete = True Then
                    LstAoiRasterLayers.Items.Add(item)
                End If
            Next
            ClearReclass(Nothing, Nothing)
        End If
    End Sub

    Private Sub LstAoiRasterLayers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles LstAoiRasterLayers.SelectedIndexChanged
        Dim item As LayerListItem = LstAoiRasterLayers.SelectedItem
        Dim selGeoDataset As IGeoDataset = Nothing
        Dim pRasterBandCollection As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pRasterStats As IRasterStatistics = Nothing

        Try
            If item IsNot Nothing Then
                m_filePath = "PleaseReturn"
                m_fileName = BA_GetBareName(item.Value, m_filePath)
                selGeoDataset = BA_OpenRasterFromGDB(m_filePath, m_fileName)
                If selGeoDataset IsNot Nothing Then
                    pRasterBandCollection = CType(selGeoDataset, IRasterBandCollection)
                    pRasterBand = pRasterBandCollection.Item(0)
                    Dim validVAT As Boolean = False
                    pRasterBand.HasTable(validVAT)
                    ' Re-initialize detail fields
                    ClearStatistics()
                    Dim textFontSize As Short = 8
                    Dim numberFontSize As Single = 9.5
                    If validVAT = False Then
                        Dim errMsg As String = "The selected raster does not have an attribute table. Please use ArcMap to create an attribute table for the raster."
                        MessageBox.Show(errMsg, "Missing attribute table", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        BtnLoad.Enabled = False
                        BtnUnique.Enabled = False
                        pRasterStats = pRasterBand.Statistics
                        If pRasterStats IsNot Nothing Then
                            LblMin.Text = Format(pRasterStats.Minimum, "######0.##")
                            LblMin.Font = New Font(LblMin.Font.FontFamily, numberFontSize, LblMin.Font.Style)
                            LblMax.Text = Format(pRasterStats.Maximum, "######0.##")
                            LblMax.Font = New Font(LblMax.Font.FontFamily, numberFontSize, LblMax.Font.Style)
                            LblSTDV.Text = Format(pRasterStats.StandardDeviation, "######0.##")
                            LblSTDV.Font = New Font(LblSTDV.Font.FontFamily, numberFontSize, LblSTDV.Font.Style)
                            LblMean.Text = Format(pRasterStats.Mean, "######0.##")
                            LblMean.Font = New Font(LblMean.Font.FontFamily, numberFontSize, LblMean.Font.Style)
                        Else
                            LblMin.Text = "Not available"
                            LblMin.Font = New Font(LblMin.Font.FontFamily, textFontSize, LblMin.Font.Style)
                            LblMax.Text = "Not available"
                            LblMax.Font = New Font(LblMax.Font.FontFamily, textFontSize, LblMax.Font.Style)
                            LblSTDV.Text = "Not available"
                            LblSTDV.Font = New Font(LblSTDV.Font.FontFamily, textFontSize, LblSTDV.Font.Style)
                            LblMean.Text = "Not available"
                            LblMean.Font = New Font(LblMean.Font.FontFamily, textFontSize, LblMean.Font.Style)
                        End If
                    Else
                        LblMin.Text = "Not available"
                        LblMin.Font = New Font(LblMin.Font.FontFamily, textFontSize, LblMin.Font.Style)
                        LblMax.Text = "Not available"
                        LblMax.Font = New Font(LblMax.Font.FontFamily, textFontSize, LblMax.Font.Style)
                        LblSTDV.Text = "Not available"
                        LblSTDV.Font = New Font(LblSTDV.Font.FontFamily, textFontSize, LblSTDV.Font.Style)
                        LblMean.Text = "Not available"
                        LblMean.Font = New Font(LblMean.Font.FontFamily, textFontSize, LblMean.Font.Style)
                    End If
                    If m_isSlice = True Then
                        BtnApply.Enabled = True
                    Else
                        ClearReclass(pRasterBand, validVAT)
                    End If
                Else
                    Dim errorMsg As String = "Unable to open layer '" & item.Name & "'"
                    MessageBox.Show(errorMsg, "Invalid layer", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("LstAoiRasterLayers_SelectedIndexChanged Exception: " + ex.Message)
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterStats)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBand)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBandCollection)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(selGeoDataset)
        End Try

    End Sub

    Private Sub TxtNumberZones_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TxtNumberZones.KeyPress

        Select Case Convert.ToInt32(e.KeyChar)
            Case 46 'decimal
                ' Invalid character
                ToolTip1.IsBalloon = True
                ToolTip1.ToolTipTitle = "Input Rejected"
                ToolTip1.Show("You can only use numeric characters (0-9) in number field.", Me.TxtNumberZones, 0, -65, 2500)
                e.Handled = True ' true means the keypress is suppressed
            Case 48 To 57  ' numbers
            Case Keys.Delete ' Delete
            Case Keys.Back ' Backspace
            Case Else
                ' Invalid character
                ToolTip1.IsBalloon = True
                ToolTip1.ToolTipTitle = "Input Rejected"
                ToolTip1.Show("You can only use numeric characters (0-9) in number field.", Me.TxtNumberZones, 0, -65, 2500)
                e.Handled = True ' true means the keypress is suppressed
        End Select

    End Sub

    Private Sub TxtBaseZone_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TxtBaseZone.KeyPress
        Select Case Convert.ToInt32(e.KeyChar)
            Case 46 'decimal
                ' Invalid character
                ToolTip1.IsBalloon = True
                ToolTip1.ToolTipTitle = "Input Rejected"
                ToolTip1.Show("You can only use numeric characters (0-9) in number field.", Me.TxtBaseZone, 0, -65, 2500)
                e.Handled = True ' true means the keypress is suppressed
            Case 48 To 57  ' numbers
            Case Keys.Delete ' Delete
            Case Keys.Back ' Backspace
            Case Else
                ' Invalid character
                ToolTip1.IsBalloon = True
                ToolTip1.ToolTipTitle = "Input Rejected"
                ToolTip1.Show("You can only use numeric characters (0-9) in number field.", Me.TxtBaseZone, 0, -65, 2500)
                e.Handled = True ' true means the keypress is suppressed
        End Select
    End Sub

    Private Sub BtnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        Dim reclassifyZoneRule As BAGIS_ClassLibrary.IRule = Nothing
        Dim item As LayerListItem = LstAoiRasterLayers.SelectedItem
        If item Is Nothing Then
            MessageBox.Show("A layer must be selected before applying a rule")
            Exit Sub
        End If
        'Validate the datum
        Dim hruExt As HruExtension = HruExtension.GetExtension
        If hruExt IsNot Nothing Then
            Dim validDatum As Boolean = BA_DatumMatch(item.Value, hruExt.Datum)
            If validDatum = False Then
                MessageBox.Show("The selected layer '" & item.Name & "' cannot be used in a rule because the datum does not match the AOI DEM.", "Invalid datum", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If
        ' Increment ruleId before using
        Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of frmHruZone.AddinImpl)(My.ThisAddIn.IDs.frmHruZone)
        Dim hruZoneForm As frmHruZone = dockWindowAddIn.UI
        Dim ruleId As Integer
        If String.IsNullOrEmpty(TxtRuleId.Text) Then
            ruleId = hruZoneForm.GetNextRuleId
        Else
            ruleId = CInt(TxtRuleId.Text)
        End If
        If m_isSlice = True Then
            'Validate input
            If String.IsNullOrEmpty(TxtNumberZones.Text) Then
                MessageBox.Show("Number of zones is required by the slice tool")
                Exit Sub
            End If
            Dim baseZone As Integer = 1
            If Not String.IsNullOrEmpty(TxtBaseZone.Text) Then
                baseZone = CInt(TxtBaseZone.Text)
            End If
            Dim sliceType As esriGeoAnalysisSliceEnum = esriGeoAnalysisSliceEnum.esriGeoAnalysisSliceEqualArea
            If RdoEqInterval.Checked = True Then
                sliceType = esriGeoAnalysisSliceEnum.esriGeoAnalysisSliceEqualInterval
            End If
            reclassifyZoneRule = New RasterSliceRule(item.Name, CLng(TxtNumberZones.Text), baseZone, sliceType, item.Value, ruleId)
        ElseIf DataGridView1.Rows.Count > 0 Then
            Dim fromValue As Object = DataGridView1.Rows(0).Cells(idxFromValue).Value
            If fromValue IsNot Nothing Then
                If IsNumeric(fromValue) Then
                    Dim pReclassItems As ReclassItem() = BA_CopyIntegerValuesFromGridToArray(DataGridView1, idxFromValue, idxOutputValue)
                    If pReclassItems IsNot Nothing AndAlso pReclassItems.GetLength(0) > 0 Then
                        Dim tempReclassRule As RasterReclassRule = New RasterReclassRule(item.Name, CStr(CboReclassField.SelectedItem), item.Value, ruleId)
                        tempReclassRule.ReclassItems = pReclassItems
                        reclassifyZoneRule = tempReclassRule
                    End If
                Else
                    Dim pReclassItems As ReclassTextItem() = BA_CopyReclassTextItemsFromGridToArray(DataGridView1, idxFromValue, idxOutputValue)
                    If pReclassItems IsNot Nothing AndAlso pReclassItems.GetLength(0) > 0 Then
                        Dim tempReclassRule As RasterReclassRule = New RasterReclassRule(item.Name, CStr(CboReclassField.SelectedItem), item.Value, ruleId)
                        tempReclassRule.ReclassTextItems = pReclassItems
                        reclassifyZoneRule = tempReclassRule
                    End If
                End If
            End If
        End If
        If reclassifyZoneRule IsNot Nothing Then
            hruZoneForm.AddPendingRule(reclassifyZoneRule)
            BtnCancel_Click(sender, e)
        End If
    End Sub

    Private Sub BtnUnique_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnUnique.Click
        If CboReclassField.SelectedItem IsNot Nothing Then
            CopyUniqueValuesToReclass()
            EnableAllButtons()
        End If
    End Sub

 

    Private Sub BtnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClear.Click
        Dim pCollection As DataGridViewSelectedCellCollection = DataGridView1.SelectedCells
        If pCollection.Count > 0 Then
            'Clear only selected rows
            For Each nextCell As DataGridViewCell In pCollection
                Dim nextRow As DataGridViewRow = nextCell.OwningRow
                nextRow.Cells(1).Value = Nothing
                nextRow.Selected = False
            Next
        Else
            'Clear everything
            For i = 0 To DataGridView1.Rows.Count - 1
                Dim nextRow As DataGridViewRow = DataGridView1.Rows(i)
                nextRow.Cells(1).Value = Nothing
            Next
        End If
    End Sub

    Private Sub BtnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pTblFilter As IGxObjectFilter = New GxFilterTables
        Dim pFilterCol As IGxObjectFilterCollection = Nothing
        Try
            pFilterCol = pGxDialog
            pFilterCol.AddFilter(pTblFilter, True)
            pGxDialog.Title = "Save Table"
            If Not pGxDialog.DoModalSave(0) Then
                Exit Sub 'Exit if user press cancel.    
            End If
            Dim filePath As String = pGxDialog.FinalLocation.FullName
            If String.IsNullOrEmpty(filePath) Then Exit Sub 'user cancelled the action
            Dim fileName As String = pGxDialog.Name
            If String.IsNullOrEmpty(fileName) Then Exit Sub 'user cancelled the action
            Dim success As BA_ReturnCode = BA_CopyValuesFromGridToTable(filePath, DataGridView1, _
                                                                        idxFromValue, idxFromValue, _
                                                                        idxOutputValue, ActionType.ReclDisc, fileName)
        Catch ex As Exception
            MessageBox.Show("BtnSave_Click Exception: " + ex.Message)
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFilterCol)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTblFilter)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGxDialog)
        End Try
    End Sub

    Private Sub BtnLoad_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnLoad.Click
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pTblFilter As IGxObjectFilter = New GxFilterTables
        Dim pGxObject As IEnumGxObject = Nothing
        Dim pGxDataset As IGxDataset = Nothing
        Dim pDatasetName As IDatasetName = Nothing
        Dim pTable As ITable = Nothing
        Try
            If CboReclassField.SelectedItem IsNot Nothing Then
                'initialize and open mini browser
                Dim bObjectSelected As Boolean
                'initialize and open mini browser
                With pGxDialog
                    .AllowMultiSelect = False
                    .ButtonCaption = "Select"
                    .Title = "Select a remap table"
                    .ObjectFilter = pTblFilter
                    bObjectSelected = .DoModalOpen(0, pGxObject)
                End With

                If bObjectSelected = False Then Exit Sub

                'get the name of the selected folder
                pGxDataset = pGxObject.Next
                pDatasetName = pGxDataset.DatasetName
                Dim filePath As String = pDatasetName.WorkspaceName.PathName
                Dim fileName As String = pDatasetName.Name
                pTable = BA_OpenTableFromFile(filePath, fileName)
                If pTable IsNot Nothing Then
                    DataGridView1.Rows.Clear()
                    CopyValuesFromTableToGrid(pTable)
                Else
                    MessageBox.Show("An error occurred when trying to load your selected table")
                End If
                EnableAllButtons()
            End If

        Catch ex As Exception
            MessageBox.Show("BtnLoad_Click Exception: " + ex.Message)
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTable)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDatasetName)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGxDataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGxObject)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTblFilter)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGxDialog)
        End Try

    End Sub

    Private Sub CopyValuesFromTableToGrid(ByVal fileTable As ITable)
        Dim pCursor As ICursor = Nothing
        Dim pRow As IRow = Nothing
        Try
            ' Load unique values to UI table
            CopyUniqueValuesToReclass()
            Dim fromFileIdx As Short = fileTable.FindField(BA_FIELD_FROM)
            Dim outFileIdx As Short = fileTable.FindField(BA_FIELD_OUT)
            pCursor = fileTable.Search(Nothing, False)
            pRow = pCursor.NextRow
            Dim missingFromLayer As Integer = 0
            ' Loop through rows in load table
            Do While pRow IsNot Nothing
                Dim pFrom As String = CStr(pRow.Value(fromFileIdx))
                Dim j As Integer = 0
                Dim match As Boolean = False
                ' Loop through rows in UI table
                For Each Row As DataGridViewRow In DataGridView1.Rows
                    Dim nextValue As String = CStr(DataGridView1.Rows(j).Cells(0).Value)
                    ' Match! Load table value into UI table
                    If String.Compare(nextValue, pFrom) = 0 Then
                        Dim pOut As String = CStr(pRow.Value(outFileIdx))
                        DataGridView1.Rows(j).Cells(1).Value = pOut
                        match = True
                        Exit For
                    End If
                    j += 1
                Next Row
                ' Record that value from load table was missing from UI table
                If match = False Then missingFromLayer += 1
                pRow = pCursor.NextRow
            Loop
            Dim i As Integer = 0
            ' Verify that all values from selected layer are present in load file
            For Each Row As DataGridViewRow In DataGridView1.Rows
                Dim nextValue As Object = DataGridView1.Rows(i).Cells(1).Value
                ' Send message to UI warning user that values did not match
                If nextValue Is Nothing Then
                    MessageBox.Show("Warning: Values in the selected layer were missing from the source file")
                    Exit For
                End If
                i += 1
            Next
            ' Send message to UI warning user that values did not match
            If missingFromLayer > 0 Then
                MessageBox.Show("Warning: Values in the source file were missing from the selected layer")
            End If

        Catch ex As Exception
            MessageBox.Show("CopyValuesFromTableToGrid Exception: " + ex.Message)
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRow)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pCursor)
        End Try
    End Sub

    Private Sub BtnAuto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAuto.Click
        For i = 0 To DataGridView1.Rows.Count - 1
            Dim nextRow As DataGridViewRow = DataGridView1.Rows(i)
            Dim nextObj As Object = nextRow.Cells(0).Value
            Dim fromValue As Integer
            If IsNumeric(nextObj) Then
                'If cell holds numeric value then this will be the automatic value
                fromValue = CInt(nextRow.Cells(0).Value)
            Else
                'If text value, automatically assign a unique numeric reclass value
                fromValue = i + 1
            End If
            nextRow.Cells(1).Value = fromValue
        Next
    End Sub

    Private Sub CboReclassField_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CboReclassField.SelectedIndexChanged
        If CboReclassField.Items.Count > 0 Then
            BtnUnique.Enabled = True
            BtnLoad.Enabled = True
        Else
            BtnUnique.Enabled = False
            BtnLoad.Enabled = False
        End If
    End Sub

    Private Sub ClearStatistics()
        LblMin.Text = Nothing
        LblMax.Text = Nothing
        LblSTDV.Text = Nothing
        LblMean.Text = Nothing
    End Sub

    Private Sub ClearReclass(ByVal rasterBand As IRasterBand, ByVal hasAttributeTable As Boolean)
        CboReclassField.Items.Clear()
        If rasterBand IsNot Nothing AndAlso hasAttributeTable = True Then
            Dim pTable As ITable = rasterBand.AttributeTable
            Dim pFields As IFields = pTable.Fields
            Dim uBound As Integer = pFields.FieldCount - 1
            For i = 1 To uBound
                Dim pField As IField = pFields.Field(i)
                CboReclassField.Items.Add(pField.Name)
            Next
            CboReclassField.SelectedIndex = 0
        End If
        DataGridView1.Rows.Clear()
        BtnAuto.Enabled = False
        BtnId.Enabled = False
        BtnSave.Enabled = False
        BtnClear.Enabled = False
        BtnApply.Enabled = False
    End Sub

    Private Sub CopyUniqueValuesToReclass()
        DataGridView1.Rows.Clear()
        Dim pGeoDataset As IGeoDataset = Nothing
        Dim pRasterBandCollection As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pTable As ITable = Nothing
        Dim pCursor As ICursor = Nothing
        Dim pData As IDataStatistics = New DataStatistics
        Dim pEnumVar As IEnumerator = Nothing
        Try
            Dim pWorkspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(m_filePath)
            If pWorkspaceType = WorkspaceType.Raster Then
                pGeoDataset = BA_OpenRasterFromFile(m_filePath, m_fileName)
            ElseIf pWorkspaceType = WorkspaceType.Geodatabase Then
                pGeoDataset = BA_OpenRasterFromGDB(m_filePath, m_fileName)
            End If

            pRasterBandCollection = CType(pGeoDataset, IRasterBandCollection)
            pRasterBand = pRasterBandCollection.Item(0)
            pTable = pRasterBand.AttributeTable
            pCursor = pTable.Search(Nothing, False)
            Dim fieldName As String = CStr(CboReclassField.SelectedItem)
            pData.Field = fieldName
            pData.Cursor = pCursor
            pEnumVar = pData.UniqueValues
            Dim valueCount As Integer = pData.UniqueValueCount
            Dim maxValues As Integer = 300
            If valueCount > maxValues Then
                MessageBox.Show("Cannot reclassify this raster. Number of unique values exceeds " & CStr(maxValues) & ".")
                Exit Sub
            End If
            pEnumVar.MoveNext()
            Dim pObj As Object = pEnumVar.Current
            While pObj IsNot Nothing
                Dim pArray As String() = {pObj, Nothing}
                DataGridView1.Rows.Add(pArray)
                pEnumVar.MoveNext()
                pObj = pEnumVar.Current
            End While
        Catch ex As Exception
            MessageBox.Show("Exception: " + ex.Message)
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pEnumVar)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pData)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTable)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBand)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBandCollection)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGeoDataset)
        End Try

    End Sub

    Private Sub EnableAllButtons()
        BtnAuto.Enabled = True
        BtnId.Enabled = True
        BtnSave.Enabled = True
        BtnClear.Enabled = True
        BtnApply.Enabled = True
    End Sub

    Public Sub LoadForm(ByVal rule As BAGIS_ClassLibrary.IRule, ByVal lstRasters As ListBox, _
                         ByVal lstDem As ListBox, ByVal lstPrism As ListBox, ByVal lstHru As ListBox, _
                         ByVal sender As System.Object, ByVal e As System.EventArgs)
        If TypeOf rule Is RasterSliceRule Then m_isSlice = True
        Dim hruExt As HruExtension = HruExtension.GetExtension
        Dim aoi As Aoi = hruExt.aoi
        Dim aoiFolderPath As String = Nothing
        If aoi IsNot Nothing Then m_AoiFolder = aoi.FilePath
        TxtRuleId.Text = CStr(rule.RuleId)
        LstAoiRasterLayers.SelectedItems.Clear()
        m_lstRaster.Items.Clear()
        m_lstRaster.Items.AddRange(lstRasters.Items)
        Dim selectedIndex As Integer = -1
        Dim counter As Integer = 0
        For Each item In m_lstRaster.Items
            Dim listItem As LayerListItem = CType(item, LayerListItem)
            If listItem.Name = rule.InputLayerName And _
                listItem.Value = rule.InputFolderPath Then
                selectedIndex = counter
                RdoRaster.Checked = True
                RdoRaster_Click(sender, e)
                LstAoiRasterLayers.SelectedItem = item
                selectedIndex = counter
                Exit For
            End If
            counter = counter + 1
        Next
        m_lstDem.Items.Clear()
        Dim selectedDemItem As LayerListItem = Nothing
        For i = 0 To lstDem.Items.Count - 1 Step 1
            Dim nextItem As LayerListItem = lstDem.Items(i)
            If nextItem.LayerType = LayerType.Raster Then
                m_lstDem.Items.Add(nextItem)
                If selectedIndex < 0 Then
                    If nextItem.Name = rule.InputLayerName And _
                        nextItem.Value = rule.InputFolderPath Then
                        selectedIndex = i
                        selectedDemItem = nextItem
                    End If
                End If
            End If
        Next
        If selectedDemItem IsNot Nothing Then
            RdoDem.Checked = True
            RdoDem_Click(sender, e)
            LstAoiRasterLayers.SelectedItem = selectedDemItem
        End If
        m_lstPRISM.Items.Clear()
        m_lstPRISM.Items.AddRange(lstPrism.Items)
        If selectedIndex < 0 Then
            counter = 0
            For Each item In m_lstPRISM.Items
                Dim listItem As LayerListItem = CType(item, LayerListItem)
                If listItem.Name = rule.InputLayerName And _
                    listItem.Value = rule.InputFolderPath Then
                    selectedIndex = counter
                    RdoPrism.Checked = True
                    RdoPrism_Click(sender, e)
                    selectedIndex = counter
                    LstAoiRasterLayers.SelectedItem = listItem
                    Exit For
                End If
                counter = counter + 1
            Next
        End If
        m_lstHru.Items.Clear()
        m_lstHru.Items.AddRange(lstHru.Items)
        If selectedIndex < 0 Then
            counter = 0
            For Each item In m_lstHru.Items
                Dim listItem As LayerListItem = CType(item, LayerListItem)
                If listItem.Name = rule.InputLayerName And _
                    listItem.Value = rule.InputFolderPath Then
                    selectedIndex = counter
                    RdoHru.Checked = True
                    RdoHru_Click(sender, e)
                    selectedIndex = counter
                    LstAoiRasterLayers.SelectedItem = listItem
                    Exit For
                End If
                counter = counter + 1
            Next
        End If

        SetFormType(m_isSlice)

        If m_isSlice = True Then
            Dim sliceRule As RasterSliceRule = CType(rule, RasterSliceRule)
            If sliceRule.SliceType = esriGeoAnalysisSliceEnum.esriGeoAnalysisSliceEqualArea Then
                RdoEqArea.Checked = True
                RdoEqInterval.Checked = False
            Else
                RdoEqArea.Checked = False
                RdoEqInterval.Checked = True
            End If
            TxtNumberZones.Text = CStr(sliceRule.ZoneCount)
            TxtBaseZone.Text = CStr(sliceRule.BaseZone)
        Else
            Dim reclassRule As RasterReclassRule = CType(rule, RasterReclassRule)
            DataGridView1.Rows.Clear()
            'Rebuild grid from reclassItems
            Dim reclassItems As ReclassItem() = reclassRule.ReclassItems
            Dim reclassTextItems As ReclassTextItem() = reclassRule.ReclassTextItems
            If reclassItems IsNot Nothing Then
                For i As Short = 0 To reclassItems.GetUpperBound(0) Step 1
                    Dim reclassItem As ReclassItem = reclassItems(i)
                    Dim displayItem As String() = {CStr(reclassItem.FromValue), CStr(reclassItem.OutputValue)}
                    DataGridView1.Rows.Add(displayItem)
                Next
            ElseIf reclassTextItems IsNot Nothing Then
                For i As Short = 0 To reclassTextItems.GetUpperBound(0) Step 1
                    Dim reclassItem As ReclassTextItem = reclassTextItems(i)
                    Dim displayItem As String() = {reclassItem.FromValue, CStr(reclassItem.OutputValue)}
                    DataGridView1.Rows.Add(displayItem)
                Next
            End If
            'Set CboReclassField to selected reclass field
            Dim pos As Integer = 0
            For Each item In CboReclassField.Items
                If item = reclassRule.ReclassField Then
                    CboReclassField.SelectedIndex = pos
                    Exit For
                End If
                pos += 1
            Next
            BtnApply.Enabled = True
            BtnUnique.Enabled = True
            BtnAuto.Enabled = True
            BtnId.Enabled = True
            BtnLoad.Enabled = True
            BtnSave.Enabled = True
            BtnClear.Enabled = True
        End If
    End Sub

    Private Sub SetFormType(Optional ByVal IsSlice As Boolean = True)
        If m_isSlice = True Then
            LblToolTip.Text = "Use a sliced raster to define zones"
            Me.Text = m_sliceCaption
            Me.Height = m_sliceHeight
            Me.Width = m_sliceWidth
            ClearStatistics()
            PnlStatistics.Visible = True
            LblDiscrete.Visible = False
            RdoEqArea.Checked = True
            RdoEqInterval.Checked = False
            TxtNumberZones.Text = "10"
            TxtBaseZone.Text = "1"
            BtnCancel.Location = New Point(280, 273)
            BtnApply.Location = New Point(350, 273)
            GrpReclass.Visible = False
            GrpSlice.Visible = True
            RdoPrism.Visible = True
        Else
            LblToolTip.Text = "Use a reclassified raster to define zones"
            Me.Text = m_rasterReclassCaption
            Me.Height = m_rasterReclassHeight
            Me.Width = m_rasterReclassWidth
            'CboReclassField.Items.Clear()
            'CboReclassField.ResetText()
            DataGridView1.Rows.Clear()
            BtnCancel.Location = New Point(280, 449)
            BtnApply.Location = New Point(350, 449)
            BtnApply.Enabled = False
            BtnUnique.Enabled = False
            BtnLoad.Enabled = False
            GrpSlice.Visible = False
            GrpReclass.Visible = True
            RdoPrism.Visible = False
            PnlStatistics.Visible = False
            'LblDiscrete.Location = New Point(160, 62)
            LblDiscrete.Visible = True
        End If
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub BtnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAbout.Click
        If m_isSlice Then
            Dim toolHelpForm As FrmHelp = New FrmHelp(BA_HelpTopics.Slice)
            toolHelpForm.ShowDialog()
        Else 'reclass rule
            Dim toolHelpForm As FrmHelp = New FrmHelp(BA_HelpTopics.Reclass)
            toolHelpForm.ShowDialog()
        End If
        Dim btn As BtnAbout = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of BtnAbout)(My.ThisAddIn.IDs.BtnAbout)

    End Sub

    Private Sub BtnId_Click(sender As System.Object, e As System.EventArgs) Handles BtnId.Click
        Dim myId As Long = 1
        For i = 0 To DataGridView1.Rows.Count - 1
            Dim nextRow As DataGridViewRow = DataGridView1.Rows(i)
            Dim nextObj As Object = nextRow.Cells(0).Value
            nextRow.Cells(1).Value = myId
            myId += 1
        Next
    End Sub
End Class