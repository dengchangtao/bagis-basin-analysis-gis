Imports System.Windows.Forms
Imports System.Drawing
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports BAGIS_ClassLibrary

Public Class FrmReclassContinuous

    Dim m_AoiFolder As String
    Dim m_filePath As String
    Dim m_fileName As String
    Dim m_lstRaster As ListBox = New ListBox
    Dim m_lstDem As ListBox = New ListBox
    Dim m_lstPRISM As ListBox = New ListBox
    Private idxFromValue As Integer = 0
    Private idxToValue As Integer = 1
    Private idxOutputValue As Integer = 2
    Private m_maxValue As Double



    Public Sub New(ByVal lstRasters As ListBox, ByVal lstDem As ListBox, _
                   ByVal lstPrism As ListBox, ByVal sender As System.Object, ByVal e As System.EventArgs)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Dim hruExt As HruExtension = HruExtension.GetExtension
        Dim aoi As Aoi = hruExt.aoi
        Dim aoiFolderPath As String = Nothing
        If aoi IsNot Nothing Then m_AoiFolder = aoi.FilePath
        TxtRuleId.Text = Nothing

        ' Populate lists
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

        ' Show DEM layers first as the default
        RdoDem.Checked = True
        RdoDem_Click(sender, e)

        ' Initialize statistics fields
        ClearStatistics()

    End Sub

    Private Sub RdoRaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RdoRaster.Click
        LstAoiRasterLayers.Items.Clear()
        ' load only continuous rasters
        For Each item As LayerListItem In m_lstRaster.Items
            If item.IsDiscrete = False Then
                LstAoiRasterLayers.Items.Add(item)
            End If
        Next
        ClearReclass(Nothing)
    End Sub

    Private Sub RdoDem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RdoDem.Click
        LstAoiRasterLayers.Items.Clear()
        ' load only continuous  rasters
        For Each item As LayerListItem In m_lstDem.Items
            If item.IsDiscrete = False Then
                LstAoiRasterLayers.Items.Add(item)
            End If
        Next
        ClearReclass(Nothing)
    End Sub

    Private Sub RdoPrism_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RdoPrism.Click
        LstAoiRasterLayers.Items.Clear()
        ' load only continuous rasters
        For Each item As LayerListItem In m_lstPRISM.Items
            If item.IsDiscrete = False Then
                LstAoiRasterLayers.Items.Add(item)
            End If
        Next
        ClearReclass(Nothing)
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub BtnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAbout.Click
        Dim toolHelpForm As FrmHelp = New FrmHelp(BA_HelpTopics.ReclassContinuous)
        toolHelpForm.ShowDialog()
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
                        pRasterStats = pRasterBand.Statistics
                        If pRasterStats IsNot Nothing Then
                            LblMin.Text = Format(pRasterStats.Minimum, "######0.##")
                            LblMin.Font = New Font(LblMin.Font.FontFamily, numberFontSize, LblMin.Font.Style)
                            ' Increment max value in case it is rounded down to catch actual max value
                            LblMax.Text = Format(pRasterStats.Maximum + 0.01, "######0.##")
                            LblMax.Font = New Font(LblMax.Font.FontFamily, numberFontSize, LblMax.Font.Style)
                            LblSTDV.Text = Format(pRasterStats.StandardDeviation, "######0.##")
                            LblSTDV.Font = New Font(LblSTDV.Font.FontFamily, numberFontSize, LblSTDV.Font.Style)
                            LblMean.Text = Format(pRasterStats.Mean, "######0.##")
                            LblMean.Font = New Font(LblMean.Font.FontFamily, numberFontSize, LblMean.Font.Style)
                            TxtMinValue.Text = LblMin.Text
                            TxtMaxValue.Text = LblMax.Text
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
                    ClearReclass(pRasterBand)
                    BtnLoad.Enabled = True
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

    Private Sub BtnReclass_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnReclass.Click
        ' Call shared function to populate reclass table
        BA_BuildReclassTable(DataGridView1, TxtMinValue, TxtMaxValue, TxtClasses, idxToValue, _
                             BA_GetReadOnlyCell(TxtTemplatesFile), 2)
        ' Cache max value in case the user changes it on us
        m_maxValue = CInt(TxtMaxValue.Text)
        BtnReclass.Enabled = True
        BtnClear.Enabled = True
        BtnSave.Enabled = True
    End Sub

    Private Sub BtnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClear.Click
        Dim pCollection As DataGridViewSelectedCellCollection = DataGridView1.SelectedCells
        If pCollection.Count > 0 Then
            'Clear only selected rows
            For Each nextCell As DataGridViewCell In pCollection
                Dim nextRow As DataGridViewRow = nextCell.OwningRow
                nextRow.Cells(idxOutputValue).Value = Nothing
                nextRow.Selected = False
            Next
        Else
            'Clear everything
            For i = 0 To DataGridView1.Rows.Count - 1
                Dim nextRow As DataGridViewRow = DataGridView1.Rows(i)
                nextRow.Cells(idxOutputValue).Value = Nothing
            Next
        End If
    End Sub

    Private Sub BtnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnApply.Click
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
        If DataGridView1.Rows.Count > 0 Then
            Dim reclassifyZoneRule As BAGIS_ClassLibrary.IRule = Nothing
            Dim pReclassItems As ReclassItem() = BA_CopySlopeValuesFromGridToArray(DataGridView1, idxFromValue, idxToValue, _
                                                                                   idxOutputValue)
            If pReclassItems IsNot Nothing AndAlso pReclassItems.GetLength(0) > 0 Then
                reclassifyZoneRule = New ReclassContinuousRule(item.Name, CStr(CboReclassField.SelectedItem), pReclassItems, item.Value, ruleId)
            End If
            hruZoneForm.AddPendingRule(reclassifyZoneRule)
        End If
        BtnCancel_Click(sender, e)
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
                                                                        idxFromValue, idxToValue, idxOutputValue, _
                                                                        ActionType.ReclCont, fileName)
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
                    BA_CopyValuesFromTableToGrid(pTable, DataGridView1)
                    ' Set read-only formats
                    Dim readOnlyCell As DataGridViewCell = DataGridView1.Item(idxToValue, DataGridView1.RowCount - 1)
                    readOnlyCell.ReadOnly = True
                    readOnlyCell.Style = BA_GetReadOnlyCell(TxtTemplatesFile).Style
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

    Private Sub DataGridView1_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellEndEdit
        ' Convert user input to Int32 so it can be sorted with existing values
        Dim strA As String = TryCast(DataGridView1.Item(e.ColumnIndex, e.RowIndex).Value, String)
        Dim intA As Int32 = -1
        If Not String.IsNullOrEmpty(strA) Then
            Integer.TryParse(strA, intA)
            If intA > 0 Then
                DataGridView1.Item(e.ColumnIndex, e.RowIndex).Value = intA
            End If
        End If
    End Sub

    Private Sub DataGridView1_CellValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) _
    Handles DataGridView1.CellValidating

        'Call shared cell validation function
        BA_ValidateCell(DataGridView1, idxToValue, idxFromValue, idxOutputValue, m_maxValue, _
                        False, sender, e)

    End Sub

    Private Sub ClearReclass(ByVal rasterBand As IRasterBand)
        CboReclassField.Items.Clear()
        If rasterBand IsNot Nothing Then
            Dim pTable As ITable = rasterBand.AttributeTable
            If pTable IsNot Nothing Then
                Dim pFields As IFields = pTable.Fields
                Dim uBound As Integer = pFields.FieldCount - 1
                For i = 1 To uBound
                    Dim pField As IField = pFields.Field(i)
                    CboReclassField.Items.Add(pField.Name)
                Next
            Else
                CboReclassField.Items.Add("Value")
            End If
            CboReclassField.SelectedIndex = 0
            BtnReclass.Enabled = True
        End If
        DataGridView1.Rows.Clear()
    End Sub

    Private Sub ClearStatistics()
        LblMin.Text = Nothing
        LblMax.Text = Nothing
        LblSTDV.Text = Nothing
        LblMean.Text = Nothing
    End Sub

    Public Sub ReloadForm(ByVal rule As BAGIS_ClassLibrary.IRule, ByVal lstRasters As ListBox, _
                     ByVal lstDem As ListBox, ByVal lstPrism As ListBox, _
                     ByVal sender As System.Object, ByVal e As System.EventArgs)
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

        Dim reclassRule As ReclassContinuousRule = CType(rule, ReclassContinuousRule)
        DataGridView1.Rows.Clear()
        Dim reclassItems As ReclassItem() = reclassRule.ReclassItems
        For i As Short = 0 To reclassItems.GetUpperBound(0) Step 1
            Dim reclassItem As ReclassItem = reclassItems(i)
            Dim displayItem As String() = {reclassItem.FromValue, reclassItem.ToValue, reclassItem.OutputValue}
            DataGridView1.Rows.Add(displayItem)
        Next
        ' Set read-only formats
        Dim readOnlyCell As DataGridViewCell = DataGridView1.Item(idxToValue, DataGridView1.RowCount - 1)
        readOnlyCell.ReadOnly = True
        readOnlyCell.Style = BA_GetReadOnlyCell(TxtTemplatesFile).Style

        TxtMinValue.Text = DataGridView1.Item(idxFromValue, 0).Value
        TxtMaxValue.Text = DataGridView1.Item(idxToValue, DataGridView1.RowCount - 1).Value
        m_maxValue = DataGridView1.Item(idxToValue, DataGridView1.RowCount - 1).Value
        TxtClasses.Text = DataGridView1.RowCount
        BtnApply.Enabled = True
        BtnReclass.Enabled = True
        BtnLoad.Enabled = True
        BtnSave.Enabled = True
        BtnClear.Enabled = True
    End Sub

    Private Sub EnableAllButtons()
        BtnSave.Enabled = True
        BtnClear.Enabled = True
        BtnApply.Enabled = True
    End Sub

    Private Sub DataGridView1_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles DataGridView1.RowsAdded
        If DataGridView1.RowCount > 0 Then
            BtnApply.Enabled = True
        End If
    End Sub

    Private Sub DataGridView1_RowsRemoved(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsRemovedEventArgs) Handles DataGridView1.RowsRemoved
        If DataGridView1.RowCount > 0 Then
            BtnApply.Enabled = True
        Else
            BtnApply.Enabled = False
        End If
    End Sub
End Class