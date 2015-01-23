Imports System.Windows.Forms
Imports System.Text
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesRaster
Imports BAGIS_ClassLibrary

Public Class FrmLandUseTemplate

    Private m_ruleId As Integer
    Private m_aoi As Aoi
    ' Flag to indicate if filtering function is disabled so we know to reset
    ' the default filter values when a filter is added
    Private m_disableFilter As Boolean = False
    Private m_reclassItems(0) As ReclassItem
    Private m_inputLayerPath As String
    Private m_inputLayerFolder As String
    Private m_templateList As New List(Of TemplateRule)
    Private Shared m_DefaultSelectedLayer As String = "west_covtype"
    Private Shared m_DefaultSelectedReclassField As String = "VALUE"
    Private m_selectedReclassField As String = m_DefaultSelectedReclassField
    Private Shared m_DefaultTemplateName As String = "Option 1"
    Private Shared m_DefaultFilterHeightWidth As String = "5"
    Private Shared m_DefaultFilterIterations As String = "5"
    Private writeHeight As Integer = 430
    Private readOnlyHeight As Integer = writeHeight - 70
    Private m_missing As Integer
    Private m_readOnly As Boolean
    Private idxFromValue As Integer = 0
    Private idxFromDescr As Integer = 1
    Private idxOutputValue As Integer = 2
    Private idxOutputDescr As Integer = 3

    Public Sub New(ByVal templateList As List(Of TemplateRule), ByVal blnReadOnly As Boolean)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Set AOI
        Dim hruExt As HruExtension = HruExtension.GetExtension
        m_aoi = hruExt.aoi
        m_readOnly = blnReadOnly

        ' Load cboBox with list of templates and find default
        m_templateList.AddRange(templateList)
        Dim pOption As String = Nothing
        For Each pRule In m_templateList
            CboOptions.Items.Add(pRule.TemplateName)
            If pRule.IsDefault = True Then
                pOption = pRule.TemplateName
            End If
        Next

        ' Initialize GUI
        If m_readOnly = True Then
            InitReadOnly()
            ' Set default template for read-only GUI
            CboOptions.SelectedItem = pOption
        Else
            TxtTemplatesFile.Text = BA_GetTemplatesFullPath()
            BtnAbout.Visible = False
        End If
        PnlButtons.BringToFront()
        CboOptions.Focus()
    End Sub

    Public Sub LoadFormFromTemplate(ByVal templateRule As TemplateRule)

        Dim pActions As List(Of TemplateAction) = templateRule.TemplateActions
        CkDefault.Checked = templateRule.IsDefault
        TxtMissingValue.Text = CStr(templateRule.MissingValue)
        For Each pAction In pActions
            ' Initialize filter fields
            If pAction.actionType = ActionType.MajorityFilter Then
                Dim params As Hashtable = pAction.parameters
                TxtFilterHeight.Text = CStr(pAction.parameters(ActionParameter.RectangleHeight))
                TxtFilterWidth.Text = CStr(pAction.parameters(ActionParameter.RectangleWidth))
                TxtIterations.Text = CStr(pAction.parameters(ActionParameter.IterationCount))
            ElseIf pAction.actionType = ActionType.ReclDisc Then
                Dim params As Hashtable = pAction.parameters
                m_selectedReclassField = CStr(pAction.parameters(ActionParameter.ReclassField))
                Dim reclassItems As ReclassItem() = params(ActionParameter.ReclassItems)
                ' Set default layer/Field
                TxtRasterLayer.Text = templateRule.InputLayerName
                TxtReclassField.Text = m_selectedReclassField
                m_inputLayerFolder = CStr(pAction.parameters(ActionParameter.InputLayer))
                RefreshValuesFromTemplate(reclassItems)
                GrdLandUse.ReadOnly = False
            End If
        Next

        ' Initialize buttons
        PnlButtons.BringToFront()

    End Sub

    Private Function LoadReadOnlyFormFromTemplate(ByVal templateRule As TemplateRule) As Integer
        TxtRasterLayer.Text = templateRule.InputLayerName
        CboOptions.SelectedItem = templateRule.TemplateName
        TxtMissingValue.Text = templateRule.MissingValue
        Dim missingItemCount As Integer
        Dim pActions As List(Of TemplateAction) = templateRule.TemplateActions
        For Each pAction In pActions
            ' Initialize filter fields
            If pAction.actionType = ActionType.MajorityFilter Then
                Dim params As Hashtable = pAction.parameters
                TxtFilterHeight.Text = CStr(pAction.parameters(ActionParameter.RectangleHeight))
                TxtFilterWidth.Text = CStr(pAction.parameters(ActionParameter.RectangleWidth))
                TxtIterations.Text = CStr(pAction.parameters(ActionParameter.IterationCount))
            ElseIf pAction.actionType = ActionType.ReclDisc Then
                Dim params As Hashtable = pAction.parameters
                TxtReclassField.Text = CStr(pAction.parameters(ActionParameter.ReclassField))
                Dim reclassItems As ReclassItem() = params(ActionParameter.ReclassItems)
                Array.Resize(m_reclassItems, reclassItems.Length)
                Array.Copy(reclassItems, m_reclassItems, reclassItems.Length)
                Dim inputLayer As String = CStr(pAction.parameters(ActionParameter.InputLayer))
                Dim layersPath As String = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Layers, True)
                m_inputLayerPath = layersPath & templateRule.InputLayerName
                If Not BA_File_Exists(m_inputLayerPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                        MessageBox.Show("The input layer (" & m_inputLayerPath & ") is missing for this template. It cannot be run.", "Missing layer", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Exit Function
                    End If
                    Dim inputPrefix As String = "PleaseReturn"
                    Dim tmpFileName As String = BA_GetBareName(m_inputLayerPath, inputPrefix)
                    RefreshFromValues(inputPrefix, TxtRasterLayer.Text, TxtReclassField.Text)
                    RefreshNewValues()
                    Dim missingValue As Integer = CInt(TxtMissingValue.Text)
                    missingItemCount = CheckForMissingValues(missingValue)
                End If
        Next
        ' Disable apply button if grid could not be loaded
        If GrdLandUse.RowCount = 0 Then
            BtnApply.Enabled = False
        End If
        Return missingItemCount
    End Function

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub BtnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        Dim templateActions As New List(Of TemplateAction)

        '*** reclass action ***'
        Dim reclassAction As New TemplateAction(GetNextRuleId)
        reclassAction.actionType = ActionType.ReclDisc
        Dim reclassItems As ReclassItem() = BA_CopyTemplateItemsFromGridToArray(GrdLandUse, _
                                                                                idxFromValue, _
                                                                                idxFromDescr, _
                                                                                idxOutputValue, _
                                                                                idxOutputDescr)
        If reclassItems Is Nothing Or reclassItems.Length = 0 Then Exit Sub
        If m_readOnly = True Then
            Dim reclassDescr As StringBuilder = New StringBuilder()
            For Each pItem In reclassItems
                reclassDescr.Append(pItem.OutputValue & ": ")
                reclassDescr.Append(pItem.FromValue & ", ")
            Next
            ' remove trailing comma
            reclassDescr = reclassDescr.Remove(reclassDescr.Length - 2, 2)
            reclassAction.addParameter(ActionParameter.LandUseDescr, reclassDescr.ToString)
        End If
        reclassAction.addParameter(ActionParameter.ReclassItems, reclassItems)

        reclassAction.addParameter(ActionParameter.InputLayer, m_inputLayerPath)
        reclassAction.addParameter(ActionParameter.ReclassField, TxtReclassField.Text)
        reclassAction.addParameter(ActionParameter.LandUseOptions, CStr(CboOptions.SelectedItem))
        templateActions.Add(reclassAction)

        '*** Post-filter (Majority) action **
        Dim minIteration As Integer = 0
        Dim maxIteration As Integer = 25
        If String.IsNullOrEmpty(TxtIterations.Text) Or IsNumeric(TxtIterations.Text) = False Then
            MessageBox.Show("Numeric value required for filter iterations")
            TxtIterations.Focus()
            Exit Sub
        ElseIf CInt(TxtIterations.Text) < minIteration Or CInt(TxtFilterWidth.Text > maxIteration) Then
            MessageBox.Show("Value between " & minIteration & " and " & maxIteration & " required for filter iterations")
            TxtIterations.Focus()
            Exit Sub
        End If
        If CInt(TxtIterations.Text) > 0 Then
            Dim minFilterSize As Integer = 3
            Dim maxFilterSize As Integer = 25
            If String.IsNullOrEmpty(TxtFilterWidth.Text) Or IsNumeric(TxtFilterWidth.Text) = False Then
                MessageBox.Show("Numeric value required for filter width")
                TxtFilterWidth.Focus()
                Exit Sub
            ElseIf CInt(TxtFilterWidth.Text < minFilterSize) Or CInt(TxtFilterWidth.Text > maxFilterSize) Then
                MessageBox.Show("Value between " & minFilterSize & " and " & maxFilterSize & " required for filter width")
                TxtFilterWidth.Focus()
                Exit Sub
            End If
            If String.IsNullOrEmpty(TxtFilterHeight.Text) Or IsNumeric(TxtFilterHeight.Text) = False Then
                MessageBox.Show("Numeric value required for filter height")
                TxtFilterHeight.Focus()
                Exit Sub
            ElseIf CInt(TxtFilterHeight.Text < minFilterSize) Or CInt(TxtFilterHeight.Text > maxFilterSize) Then
                MessageBox.Show("Value between " & minFilterSize & " and " & maxFilterSize & " required for filter height")
                TxtFilterHeight.Focus()
                Exit Sub
            End If
            Dim filterAction As New TemplateAction(GetNextRuleId)
            filterAction.actionType = ActionType.MajorityFilter
            filterAction.addParameter(ActionParameter.RectangleWidth, TxtFilterWidth.Text)
            filterAction.addParameter(ActionParameter.RectangleHeight, TxtFilterHeight.Text)
            filterAction.addParameter(ActionParameter.IterationCount, TxtIterations.Text)
            templateActions.Add(filterAction)
        End If

        ' Increment ruleId before using
        If m_readOnly = True Then
            Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of frmHruZone.AddinImpl)(My.ThisAddIn.IDs.frmHruZone)
            Dim hruZoneForm As frmHruZone = dockWindowAddIn.UI
            Dim ruleId As Integer
            If String.IsNullOrEmpty(TxtRuleId.Text) Then
                ruleId = hruZoneForm.GetNextRuleId
            Else
                ruleId = CInt(TxtRuleId.Text)
            End If
            Dim fileName As String = BA_GetBareName(m_inputLayerPath)
            Dim landUseRule As New TemplateRule(HruRuleType.LandUse, templateActions, fileName, _
                                                m_inputLayerPath, ruleId)
            landUseRule.MissingValue = TxtMissingValue.Text
            hruZoneForm.AddPendingRule(landUseRule)
            BtnCancel_Click(sender, e)
        Else
            'Populate template-only fields
            Dim editedRule As TemplateRule = Nothing
            For Each pRule In m_templateList
                If pRule.TemplateName = CStr(CboOptions.SelectedItem) Then
                    editedRule = pRule
                    Exit For
                End If
            Next

            If editedRule IsNot Nothing Then
                editedRule.TemplateActions = templateActions
                editedRule.InputLayerName = TxtRasterLayer.Text
                editedRule.InputFolderPath = m_inputLayerPath
                editedRule.IsDefault = CkDefault.Checked
                editedRule.MissingValue = CInt(TxtMissingValue.Text)
            Else
                editedRule = New TemplateRule(HruRuleType.LandUse, templateActions, TxtRasterLayer.Text, _
                                              m_inputLayerPath, 1)
                editedRule.TemplateName = CboOptions.Text
                editedRule.IsDefault = CkDefault.Checked
                editedRule.MissingValue = CInt(TxtMissingValue.Text)
            End If
            BA_AddUpdateTemplateRule(editedRule)

            'Reset form
            m_templateList.Clear()
            m_templateList.AddRange(BA_GetTemplates(HruRuleType.LandUse))
            CboOptions.Items.Clear()
            For Each pRule In m_templateList
                CboOptions.Items.Add(pRule.TemplateName)
            Next
            ClearTemplateEditor()
        End If
    End Sub

    Private Function GetNextRuleId() As Integer
        m_ruleId = m_ruleId + 1
        Return m_ruleId
    End Function

    'Reload form when existing rule template is edited; Always read-only for users
    Public Sub LoadForm(ByVal tRule As TemplateRule)
        m_readOnly = True
        TxtRuleId.Text = CStr(tRule.RuleId)
        TxtRasterLayer.Text = tRule.InputLayerName
        TxtMissingValue.Text = tRule.MissingValue
        If tRule.RuleId > 0 Then
            TxtFilterHeight.Text = 0
            TxtFilterHeight.Enabled = False
            TxtFilterWidth.Text = 0
            TxtFilterWidth.Enabled = False
            TxtIterations.Text = 0
        End If
        Dim actions As List(Of TemplateAction) = tRule.TemplateActions
        For Each action In actions
            Dim params As Hashtable = action.parameters
            If action.actionType = ActionType.MajorityFilter Then
                Dim oIteration As Object = params(ActionParameter.IterationCount)
                Dim iteration As String = TryCast(oIteration, String)
                TxtIterations.Text = iteration
                Dim oHeight As Object = params(ActionParameter.RectangleHeight)
                TxtFilterHeight.Enabled = True
                TxtFilterHeight.Text = TryCast(oHeight, String)
                Dim oWidth As Object = params(ActionParameter.RectangleWidth)
                TxtFilterWidth.Enabled = True
                TxtFilterWidth.Text = TryCast(oWidth, String)
            ElseIf action.actionType = ActionType.ReclDisc Then
                Dim pOption As String = CStr(params(ActionParameter.LandUseOptions))
                CboOptions.SelectedItem = pOption
                TxtReclassField.Text = params(ActionParameter.ReclassField)
                Dim reclassItems As ReclassItem() = params(ActionParameter.ReclassItems)
                Array.Resize(m_reclassItems, reclassItems.Length)
                Array.Copy(reclassItems, m_reclassItems, reclassItems.Length)
                m_inputLayerPath = CStr(params(ActionParameter.InputLayer))
                Dim inputPrefix As String = "PleaseReturn"
                Dim tmpFileName As String = BA_GetBareName(m_inputLayerPath, inputPrefix)
                RefreshFromValues(inputPrefix, TxtRasterLayer.Text, TxtReclassField.Text)
                RefreshNewValues()
            End If
        Next

        ' Initialize buttons
        PnlButtons.BringToFront()
    End Sub

    Private Sub TxtIterations_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtIterations.TextChanged
        If IsNumeric(TxtIterations.Text) Then
            If CInt(TxtIterations.Text) < 1 Then
                TxtFilterHeight.Text = 0
                TxtFilterHeight.Enabled = False
                TxtFilterWidth.Text = 0
                TxtFilterWidth.Enabled = False
                m_disableFilter = True
            Else
                If m_disableFilter = True Then
                    TxtFilterHeight.Enabled = True
                    TxtFilterWidth.Enabled = True
                    TxtFilterWidth.Text = 5
                    TxtFilterHeight.Text = 5
                    m_disableFilter = False
                End If
            End If
        End If
    End Sub

    Private Shared Function GetOptionOne() As ReclassItem()
        Dim reclassItems(16) As ReclassItem
        Dim item0 As ReclassItem = New ReclassItem(11, "Open Water", 11, 41, "Lakes and Reservoirs")
        reclassItems(0) = item0
        Dim item1 As ReclassItem = New ReclassItem(12, "Perennial Ice/Snow", 12, 6, "Perennial Ice")
        reclassItems(1) = item1
        Dim item2 As ReclassItem = New ReclassItem(21, "Developed, Open Space", 21, 5, "Rangeland")
        reclassItems(2) = item2
        Dim item3 As ReclassItem = New ReclassItem(22, "Developed, Low Intensity", 22, 3, "Urban")
        reclassItems(3) = item3
        Dim item4 As ReclassItem = New ReclassItem(23, "Developed, Medium Intensity", 23, 3, "Urban")
        reclassItems(4) = item4
        Dim item5 As ReclassItem = New ReclassItem(24, "Developed, High Intensity", 24, 3, "Urban")
        reclassItems(5) = item5
        Dim item6 As ReclassItem = New ReclassItem(31, "Barren Land (Rock/Sand,Clay)", 31, 2, "Agriculture")
        reclassItems(6) = item6
        Dim item7 As ReclassItem = New ReclassItem(41, "Deciduous Forest", 41, 1, "Forest")
        reclassItems(7) = item7
        Dim item8 As ReclassItem = New ReclassItem(42, "Evergreen Forest", 42, 1, "Forest")
        reclassItems(8) = item8
        Dim item9 As ReclassItem = New ReclassItem(43, "Mixed Forest", 43, 1, "Forest")
        reclassItems(9) = item9
        Dim item10 As ReclassItem = New ReclassItem(52, "Shrub/Scrub", 52, 5, "Rangeland")
        reclassItems(10) = item10
        Dim item11 As ReclassItem = New ReclassItem(71, "Grassland/Herbaceous", 71, 2, "Agriculture")
        reclassItems(11) = item11
        Dim item12 As ReclassItem = New ReclassItem(81, "Pasture/Hay", 81, 2, "Agriculture")
        reclassItems(12) = item12
        Dim item13 As ReclassItem = New ReclassItem(82, "Cultivated Crops", 82, 2, "Agriculture")
        reclassItems(13) = item13
        Dim item14 As ReclassItem = New ReclassItem(90, "Woody Wetlands", 90, 4, "Wetlands")
        reclassItems(14) = item14
        Dim item15 As ReclassItem = New ReclassItem(95, "Emergent Herbaceous Wetlands", 95, 4, "Wetlands")
        reclassItems(15) = item15
        Dim item16 As ReclassItem = New ReclassItem(127, "Ocean", 127, 41, "Lakes and Reservoirs")
        reclassItems(16) = item16
        Return reclassItems
    End Function

    Private Sub RefreshFromValues(ByVal inputPath As String, ByVal inputName As String, _
                                  ByVal selField As String)
        Dim pGeoDataset As IGeoDataset = Nothing
        Dim pRasterBandCollection As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pTable As ITable = Nothing
        Dim pCursor As ICursor = Nothing
        Dim pData As IDataStatistics = New DataStatistics
        Dim pEnumVar As IEnumerator = Nothing

        Try
            GrdLandUse.Rows.Clear()
            pGeoDataset = BA_OpenRasterFromGDB(inputPath, inputName)
            pRasterBandCollection = CType(pGeoDataset, IRasterBandCollection)
            pRasterBand = pRasterBandCollection.Item(0)
            If pRasterBand IsNot Nothing Then
                ' Populate from values from raster
                pTable = pRasterBand.AttributeTable
                If pTable IsNot Nothing Then
                    pCursor = pTable.Search(Nothing, False)
                    Dim fieldName As String = CStr(selField)
                    pData.Field = fieldName
                    pData.Cursor = pCursor
                    pEnumVar = pData.UniqueValues
                    Dim valueCount As Integer = pData.UniqueValueCount
                    Dim maxValues As Integer = 300
                    If valueCount > maxValues Then
                        MessageBox.Show("Cannot process this raster. Number of unique values exceeds " & CStr(maxValues) & ".")
                        Exit Sub
                    End If
                    pEnumVar.MoveNext()
                    Dim pObj As Object = pEnumVar.Current
                    Dim iRow As Integer = 0
                    While pObj IsNot Nothing
                        ' Assumes old value is a number
                        Dim pDouble As Double = CDbl(pObj)
                        Dim pArray As Object() = {pDouble, Nothing, Nothing, Nothing}
                        GrdLandUse.Rows.Add(pArray)
                        If m_readOnly = True Then
                            'set read-only cell format
                            Dim fromCell As DataGridViewCell = GrdLandUse.Item(idxFromValue, iRow)
                            fromCell.Style = GetReadOnlyCell.Style
                        End If
                        iRow += 1
                        pEnumVar.MoveNext()
                        pObj = pEnumVar.Current
                    End While
                Else
                    MessageBox.Show("Attribute table is missing from this raster. It cannot be reclassified.")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            MsgBox("RefreshGrid Exception: " & ex.Message)
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

    Private Sub RefreshValuesFromTemplate(ByVal reclassItems As ReclassItem())
        GrdLandUse.Rows.Clear()
        For Each item In reclassItems
            Dim pArray As Object() = {item.FromValue, item.FromDescr, item.OutputValue, item.OutputDescr}
            GrdLandUse.Rows.Add(pArray)
        Next
    End Sub

    Private Sub RefreshNewValues()
        For i = 0 To GrdLandUse.Rows.Count - 1
            Dim nextRow As DataGridViewRow = GrdLandUse.Rows(i)
            Dim fromValue As Integer = CInt(nextRow.Cells(idxFromValue).Value)
            For Each pItem In m_reclassItems
                If pItem.FromValue = fromValue Then
                    nextRow.Cells(idxFromDescr).Value = pItem.FromDescr
                    nextRow.Cells(idxFromDescr).Style = GetReadOnlyCell.Style
                    nextRow.Cells(idxOutputValue).Value = pItem.OutputValue
                    nextRow.Cells(idxOutputValue).Style = GetReadOnlyCell.Style
                    nextRow.Cells(idxOutputDescr).Value = pItem.OutputDescr
                    nextRow.Cells(idxOutputDescr).Style = GetReadOnlyCell.Style
                    Exit For
                End If
            Next
        Next
    End Sub

    Private Function CheckForMissingValues(ByVal missingValue As Integer) As Integer
        Dim rowCount As Integer = GrdLandUse.Rows.Count
        Dim missingCount As Integer
        ' Define style for missing values
        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        style.ForeColor = Drawing.Color.Red
        style.BackColor = Drawing.Color.Yellow
        Dim font As Drawing.Font = New Drawing.Font(GrdLandUse.Font, Drawing.FontStyle.Bold)
        style.Font = font

        For i = 0 To rowCount - 1
            Dim nextRow As DataGridViewRow = GrdLandUse.Rows(i)
            If nextRow.Cells(idxOutputValue).Value Is Nothing Then
                missingCount += 1
                Dim valueCell As DataGridViewCell = nextRow.Cells(idxOutputValue)
                valueCell.Style = style
                valueCell.Value = missingValue
                Dim descrCell As DataGridViewCell = nextRow.Cells(idxOutputDescr)
                descrCell.Style = style
                descrCell.Value = "Missing value"
                'GrdLandUse.CurrentCell = GrdLandUse.Rows(i).Cells(idxOutputValue)
                'GrdLandUse.Rows(i).Selected = True
            End If
        Next

        Return missingCount

    End Function

    Public Shared Function GetDefaultTemplate() As TemplateRule
        Dim tRule As New TemplateRule
        '*** Populate rule parameters ***
        tRule.RuleId = 1
        tRule.TemplateName = m_DefaultTemplateName
        tRule.RuleType = HruRuleType.LandUse
        tRule.IsDefault = True
        tRule.MissingValue = -999
        Dim templateActions As New List(Of TemplateAction)

        '*** reclass action ***'
        Dim reclassAction As New TemplateAction(1)
        reclassAction.actionType = ActionType.ReclDisc
        reclassAction.addParameter(ActionParameter.ReclassItems, GetOptionOne)
        reclassAction.addParameter(ActionParameter.InputLayer, "\" & BA_EnumDescription(GeodatabaseNames.Layers) & "\" & m_DefaultSelectedLayer)
        reclassAction.addParameter(ActionParameter.ReclassField, m_DefaultSelectedReclassField)
        templateActions.Add(reclassAction)

        '*** Post-filter (Majority) action **
        Dim filterAction As New TemplateAction(2)
        filterAction.actionType = ActionType.MajorityFilter
        filterAction.addParameter(ActionParameter.RectangleWidth, m_DefaultFilterHeightWidth)
        filterAction.addParameter(ActionParameter.RectangleHeight, m_DefaultFilterHeightWidth)
        filterAction.addParameter(ActionParameter.IterationCount, m_DefaultFilterIterations)
        templateActions.Add(filterAction)

        tRule.TemplateActions = templateActions
        tRule.InputLayerName = m_DefaultSelectedLayer
        Return tRule
    End Function

    Private Sub InitReadOnly()
        ' Hide fields used by editor
        CkDefault.Visible = False
        CboOptions.DropDownStyle = ComboBoxStyle.DropDownList
        BtnSelectLyr.Visible = False
        BtnOldValues.Visible = False
        BtnDeleteTemplate.Visible = False
        BtnClear.Visible = False
        TxtMissingValue.ReadOnly = True
        LblTemplatesFile.Visible = False
        TxtTemplatesFile.Visible = False
        Dim startingY As Integer = LblToolTip.Location.Y
        Dim pnlOptionsPoint As Drawing.Point = PnlOptions.Location
        Dim pnlGridPoint As Drawing.Point = PnlGrid.Location
        PnlOptions.Height = 50
        pnlOptionsPoint.Y = startingY + 25
        PnlOptions.Location = pnlOptionsPoint
        pnlGridPoint.Y = startingY + 75
        PnlGrid.Location = pnlGridPoint
        GrdLandUse.ReadOnly = True
        GrdLandUse.AllowUserToAddRows = False
        GrdLandUse.AllowUserToDeleteRows = False
        Me.Height = readOnlyHeight
    End Sub

    Private Sub CboOptions_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CboOptions.SelectedValueChanged
        For Each pRule In m_templateList
            If pRule.TemplateName = CboOptions.SelectedItem Then
                If m_readOnly = True Then
                    Dim missingItemCount As Integer = LoadReadOnlyFormFromTemplate(pRule)
                    Dim missingValue As Integer = CInt(TxtMissingValue.Text)
                    If missingItemCount > 0 Then
                        MessageBox.Show("One or more values from the input raster is missing from the template. Missing values will be set to " & missingValue & ".", "Missing values", _
                        MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If

                Else
                    LoadFormFromTemplate(pRule)
                End If
            End If
        Next
    End Sub

    Private Sub BtnSelectLyr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSelectLyr.Click
        Dim frmSelect As New FrmSelectTemplateLayer(m_aoi)
        frmSelect.ShowDialog()
        If frmSelect.InputLayerPath IsNot Nothing And _
           frmSelect.InputField IsNot Nothing Then
            Dim layerName As String = BA_GetBareName(frmSelect.InputLayerPath)
            TxtRasterLayer.Text = layerName
            TxtReclassField.Text = frmSelect.InputField
            m_inputLayerPath = frmSelect.InputLayerPath
            m_inputLayerFolder = frmSelect.InputLayerFolder
            m_aoi = frmSelect.Aoi
            GrdLandUse.Rows.Clear()
        Else
            'Do nothing. The cancel button was clicked
            'MessageBox.Show("New layer and/or field were not selected. The from values will not be updated.", "Missing input", _
            'MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub BtnOldValues_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnOldValues.Click
        Dim inputFolder As String = "PleaseReturn"
        Dim inputFile As String = BA_GetBareName(m_inputLayerPath, inputFolder)
        RefreshFromValues(inputFolder, inputFile, TxtReclassField.Text)
        GrdLandUse.ReadOnly = False
        TxtFilterHeight.Text = m_DefaultFilterHeightWidth
        TxtFilterWidth.Text = m_DefaultFilterHeightWidth
        TxtIterations.Text = m_DefaultFilterIterations
    End Sub

    Private Sub BtnDeleteTemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDeleteTemplate.Click
        Dim templateName As String = CStr(CboOptions.SelectedItem)
        If CboOptions.SelectedItem IsNot Nothing And CboOptions.Items.Count < 2 Then
            MessageBox.Show("Unable to delete the last template. You must leave at least one template.", "Delete Template", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        If templateName IsNot Nothing Then
            Dim result As DialogResult = MessageBox.Show("About to permanently delete template '" & templateName & "'. This action cannot be undone. Do you wish to continue ?", "Delete Template", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If result = DialogResult.Yes Then
                BA_DeleteTemplateRule(HruRuleType.LandUse, templateName)
                'Reset form
                m_templateList.Clear()
                m_templateList.AddRange(BA_GetTemplates(HruRuleType.LandUse))
                CboOptions.Items.Clear()
                For Each pRule In m_templateList
                    CboOptions.Items.Add(pRule.TemplateName)
                Next
                ClearTemplateEditor()
            Else
                Exit Sub
            End If
        Else
            MessageBox.Show("You must select a template to delete. ", "Delete Template", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub BtnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClear.Click
        Dim pCollection As DataGridViewSelectedCellCollection = GrdLandUse.SelectedCells
        If pCollection.Count > 0 Then
            'Clear only selected rows
            For Each nextCell As DataGridViewCell In pCollection
                Dim nextRow As DataGridViewRow = nextCell.OwningRow
                nextRow.Cells(idxFromDescr).Value = Nothing
                nextRow.Cells(idxOutputValue).Value = Nothing
                nextRow.Cells(idxOutputDescr).Value = Nothing
                nextRow.Selected = False
            Next
        Else
            'Clear everything
            For i = 0 To GrdLandUse.Rows.Count - 1
                Dim nextRow As DataGridViewRow = GrdLandUse.Rows(i)
                nextRow.Cells(idxFromDescr).Value = Nothing
                nextRow.Cells(idxOutputValue).Value = Nothing
                nextRow.Cells(idxOutputDescr).Value = Nothing
            Next
        End If
    End Sub

    Private Sub ClearTemplateEditor()
        CboOptions.ResetText()
        TxtRasterLayer.Text = Nothing
        TxtReclassField.Text = Nothing
        CkDefault.Checked = False
        GrdLandUse.Rows.Clear()
        TxtFilterHeight.Text = m_DefaultFilterHeightWidth
        TxtFilterWidth.Text = m_DefaultFilterHeightWidth
        TxtIterations.Text = m_DefaultFilterIterations
        TxtMissingValue.Text = Nothing
    End Sub

    Private Sub BtnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAbout.Click
        Dim toolHelpForm As FrmHelp = New FrmHelp(BA_HelpTopics.LULCTemplate)
        toolHelpForm.ShowDialog()
    End Sub

    Private Sub TxtRasterLayer_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtRasterLayer.TextChanged
        If Not String.IsNullOrEmpty(TxtRasterLayer.Text) And _
           Not String.IsNullOrEmpty(TxtReclassField.Text) Then
            BtnApply.Enabled = True
        Else
            BtnApply.Enabled = False
        End If
    End Sub

    Private Sub TxtReclassField_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtReclassField.TextChanged
        If Not String.IsNullOrEmpty(TxtRasterLayer.Text) And _
           Not String.IsNullOrEmpty(TxtReclassField.Text) Then
            BtnApply.Enabled = True
        Else
            BtnApply.Enabled = False
        End If
    End Sub

    Private Function GetReadOnlyCell() As DataGridViewCell
        Dim cell As DataGridViewCell = New DataGridViewTextBoxCell()
        cell.Style.BackColor = TxtTemplatesFile.BackColor
        cell.Style.ForeColor = TxtTemplatesFile.ForeColor
        Return cell
    End Function

    Private Sub GrdLandUse_CellValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) _
Handles GrdLandUse.CellValidating

        Dim cell As DataGridViewCell = GrdLandUse.Item(e.ColumnIndex, e.RowIndex)
        Dim maxValue As Integer = 9999
        Dim maxLength As Integer = 255
        If cell.IsInEditMode Then
            Dim c As Control = GrdLandUse.EditingControl
            Select Case e.ColumnIndex
                Case idxFromValue
                    If Not BA_ValidInteger(e.FormattedValue, maxValue) Then
                        MessageBox.Show("From value must be a positive integer < " & maxValue)
                        c.Text = Nothing
                        e.Cancel = True
                    Else
                        c.Text = CInt(e.FormattedValue)
                    End If
                Case idxOutputValue
                    If Not BA_ValidInteger(e.FormattedValue, maxValue) Then
                        MessageBox.Show("Output value must be a positive integer < " & maxValue)
                        c.Text = Nothing
                        e.Cancel = True
                    Else
                        c.Text = CInt(e.FormattedValue)
                    End If
            End Select
        End If
    End Sub
End Class