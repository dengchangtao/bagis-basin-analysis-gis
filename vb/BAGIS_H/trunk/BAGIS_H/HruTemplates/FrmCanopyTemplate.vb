Imports System.Windows.Forms
Imports System.Text
Imports BAGIS_ClassLibrary

Public Class FrmCanopyTemplate

    Private m_ruleId As Integer
    Private m_aoiPath As String
    ' Flag to indicate if filtering function is disabled so we know to reset
    ' the default filter values when a filter is added
    Private m_disablePreFilter As Boolean = False
    Private m_disablePostFilter As Boolean = False
    Private m_reclassItems(0) As ReclassItem
    Private m_templateList As New List(Of TemplateRule)
    Private m_inputLayerPath As String
    Private m_inputLayerFolder As String
    Private Shared m_DefaultTemplateName As String = "Default canopy"
    Private Shared m_DefaultFilterHeightWidth As String = "5"
    Private Shared m_DefaultFilterIterations As String = "5"
    Private Shared m_DefaultSelectedLayer As String = "cov_den"
    Private Shared m_DefaultSelectedReclassField As String = "VALUE"
    Private Shared m_SelectedReclassField As String = m_DefaultSelectedReclassField
    Private m_readOnly As Boolean
    Private idxFromValue As Integer = 0
    Private idxToValue As Integer = 1
    Private idxOutputValue As Integer = 2
    Private m_maxValue As Double
    Private Shared m_DefaultMaxValue As Integer = 100
    Private Shared m_DefaultMinValue As Integer = 0


    Public Sub New(ByVal templateList As List(Of TemplateRule), ByVal blnReadOnly As Boolean)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Set AOI
        Dim hruExt As HruExtension = HruExtension.GetExtension
        Dim pAoi As Aoi = hruExt.aoi
        If pAoi IsNot Nothing Then m_aoiPath = pAoi.FilePath
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

        If m_readOnly = True Then
            ' Set default template for read-only GUI
            CboOptions.SelectedItem = pOption
            initReadOnly()
        Else
            TxtTemplatesFile.Text = BA_GetTemplatesFullPath()
            TxtMinValue.Text = m_DefaultMinValue
            TxtMaxValue.Text = m_DefaultMaxValue
            BtnAbout.Visible = False
        End If

    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub BtnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        Dim templateActions As New List(Of TemplateAction)

        '*** Pre-filter (Low-Pass) action **
        Dim minIteration As Integer = 0
        Dim maxIteration As Integer = 25
        If String.IsNullOrEmpty(TxtPreIterations.Text) Or IsNumeric(TxtPreIterations.Text) = False Then
            MessageBox.Show("Numeric value required for low-pass filter iterations")
            TxtPreIterations.Focus()
            Exit Sub
        ElseIf CInt(TxtPreIterations.Text) < minIteration Or CInt(TxtPreWidth.Text > maxIteration) Then
            MessageBox.Show("Value between " & minIteration & " and " & maxIteration & " required for filter iterations")
            TxtPreIterations.Focus()
            Exit Sub
        End If
        If CInt(TxtPreIterations.Text) > 0 Then
            Dim minFilterSize As Integer = 3
            Dim maxFilterSize As Integer = 25
            If String.IsNullOrEmpty(TxtPreWidth.Text) Or IsNumeric(TxtPreWidth.Text) = False Then
                MessageBox.Show("Numeric value required for low-pass filter width")
                TxtPreWidth.Focus()
                Exit Sub
            ElseIf CInt(TxtPreWidth.Text < minFilterSize) Or CInt(TxtPreWidth.Text > maxFilterSize) Then
                MessageBox.Show("Value between " & minFilterSize & " and " & maxFilterSize & " required for low-pass filter width")
                TxtPreWidth.Focus()
                Exit Sub
            End If
            If String.IsNullOrEmpty(TxtPreHeight.Text) Or IsNumeric(TxtPreHeight.Text) = False Then
                MessageBox.Show("Numeric value required for low-pass filter height")
                TxtPreHeight.Focus()
                Exit Sub
            ElseIf CInt(TxtPreHeight.Text < minFilterSize) Or CInt(TxtPreHeight.Text > maxFilterSize) Then
                MessageBox.Show("Value between " & minFilterSize & " and " & maxFilterSize & " required for low-pass filter height")
                TxtPreHeight.Focus()
                Exit Sub
            End If
            Dim filterAction As New TemplateAction(GetNextRuleId)
            filterAction.actionType = ActionType.LowPassFilter
            filterAction.addParameter(ActionParameter.RectangleWidth, TxtPreWidth.Text)
            filterAction.addParameter(ActionParameter.RectangleHeight, TxtPreHeight.Text)
            filterAction.addParameter(ActionParameter.IterationCount, TxtPreIterations.Text)
            templateActions.Add(filterAction)
        End If

        '*** reclass action ***'
        Dim reclassAction As New TemplateAction(GetNextRuleId)
        reclassAction.actionType = ActionType.ReclCont
        Dim reclassItems As ReclassItem() = BA_CopySlopeValuesFromGridToArray(GrdCanopy, idxFromValue, idxToValue, idxOutputValue)
        If m_readOnly = True Then
            Dim reclassDescr As StringBuilder = New StringBuilder()
            For Each pItem In reclassItems
                reclassDescr.Append(pItem.OutputValue & ": ")
                reclassDescr.Append(pItem.FromValue & " – ")
                reclassDescr.Append(pItem.ToValue & ", ")
            Next
            ' remove trailing comma
            reclassDescr = reclassDescr.Remove(reclassDescr.Length - 2, 2)
            reclassAction.addParameter(ActionParameter.CanopyClassifyDescr, reclassDescr.ToString)
        End If
        reclassAction.addParameter(ActionParameter.ReclassField, TxtReclassField.Text)
        reclassAction.addParameter(ActionParameter.ReclassItems, reclassItems)
        reclassAction.addParameter(ActionParameter.CanopyOptions, CStr(CboOptions.SelectedItem))
        templateActions.Add(reclassAction)

        '*** Post-filter (Majority) action **
        If String.IsNullOrEmpty(TxtPostIterations.Text) Or IsNumeric(TxtPostIterations.Text) = False Then
            MessageBox.Show("Numeric value required for majority filter iterations")
            TxtPostIterations.Focus()
            Exit Sub
        ElseIf CInt(TxtPostIterations.Text) < minIteration Or CInt(TxtPostWidth.Text > maxIteration) Then
            MessageBox.Show("Value between " & minIteration & " and " & maxIteration & " required for majority filter iterations")
            TxtPostIterations.Focus()
            Exit Sub
        End If
        If CInt(TxtPostIterations.Text) > 0 Then
            Dim minFilterSize As Integer = 3
            Dim maxFilterSize As Integer = 25
            If String.IsNullOrEmpty(TxtPostWidth.Text) Or IsNumeric(TxtPostWidth.Text) = False Then
                MessageBox.Show("Numeric value required for majority filter width")
                TxtPostWidth.Focus()
                Exit Sub
            ElseIf CInt(TxtPostWidth.Text < minFilterSize) Or CInt(TxtPostWidth.Text > maxFilterSize) Then
                MessageBox.Show("Value between " & minFilterSize & " and " & maxFilterSize & " required for majority filter width")
                TxtPostWidth.Focus()
                Exit Sub
            End If
            If String.IsNullOrEmpty(TxtPostHeight.Text) Or IsNumeric(TxtPostHeight.Text) = False Then
                MessageBox.Show("Numeric value required for majority filter height")
                TxtPostHeight.Focus()
                Exit Sub
            ElseIf CInt(TxtPostHeight.Text < minFilterSize) Or CInt(TxtPostHeight.Text > maxFilterSize) Then
                MessageBox.Show("Value between " & minFilterSize & " and " & maxFilterSize & " required for majority filter height")
                TxtPostHeight.Focus()
                Exit Sub
            End If
            Dim filterAction As New TemplateAction(GetNextRuleId)
            filterAction.actionType = ActionType.MajorityFilter
            filterAction.addParameter(ActionParameter.RectangleWidth, TxtPostWidth.Text)
            filterAction.addParameter(ActionParameter.RectangleHeight, TxtPostHeight.Text)
            filterAction.addParameter(ActionParameter.IterationCount, TxtPostIterations.Text)
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
            Dim canopyRule As New TemplateRule(HruRuleType.Canopy, templateActions, fileName, _
                                               m_inputLayerPath, ruleId)
            hruZoneForm.AddPendingRule(canopyRule)
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
            Else
                editedRule = New TemplateRule(HruRuleType.Canopy, templateActions, TxtRasterLayer.Text, _
                                              m_inputLayerPath, 1)
                editedRule.TemplateName = CboOptions.Text
                editedRule.IsDefault = CkDefault.Checked
            End If
            BA_AddUpdateTemplateRule(editedRule)

            'Reset form
            m_templateList.Clear()
            m_templateList.AddRange(BA_GetTemplates(HruRuleType.Canopy))
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

    Public Sub ReloadForm(ByVal tRule As TemplateRule)
        TxtRuleId.Text = CStr(tRule.RuleId)
        If tRule.RuleId > 0 Then
            TxtPreHeight.Text = 0
            TxtPreHeight.Enabled = False
            TxtPreWidth.Text = 0
            TxtPreWidth.Enabled = False
            TxtPreIterations.Text = 0
            TxtPostHeight.Text = 0
            TxtPostHeight.Enabled = False
            TxtPostWidth.Text = 0
            TxtPostWidth.Enabled = False
            TxtPostIterations.Text = 0
        End If
        Dim actions As List(Of TemplateAction) = tRule.TemplateActions
        For Each action In actions
            Dim params As Hashtable = action.parameters
            If action.actionType = ActionType.LowPassFilter Then
                Dim oIteration As Object = params(ActionParameter.IterationCount)
                Dim iteration As String = TryCast(oIteration, String)
                TxtPreIterations.Text = iteration
                Dim oHeight As Object = params(ActionParameter.RectangleHeight)
                TxtPreHeight.Text = TryCast(oHeight, String)
                Dim oWidth As Object = params(ActionParameter.RectangleWidth)
                TxtPreWidth.Text = TryCast(oWidth, String)
            ElseIf action.actionType = ActionType.ReclCont Then
                Dim canopyOption As String = params(ActionParameter.CanopyOptions)
                For Each pItem In CboOptions.Items
                    If String.Compare(CStr(pItem), canopyOption) = 0 Then
                        CboOptions.SelectedItem = pItem
                        Exit For
                    End If
                Next
                Dim reclassItems As ReclassItem() = params(ActionParameter.ReclassItems)
                Array.Resize(m_reclassItems, reclassItems.Length)
                Array.Copy(reclassItems, m_reclassItems, reclassItems.Length)
                RefreshReadOnlyGrid()
            ElseIf action.actionType = ActionType.MajorityFilter Then
                Dim oIteration As Object = params(ActionParameter.IterationCount)
                Dim iteration As String = TryCast(oIteration, String)
                TxtPostIterations.Text = iteration
                Dim oHeight As Object = params(ActionParameter.RectangleHeight)
                TxtPostHeight.Text = TryCast(oHeight, String)
                Dim oWidth As Object = params(ActionParameter.RectangleWidth)
                TxtPostWidth.Text = TryCast(oWidth, String)
            End If
        Next
    End Sub

    Private Sub TxtPreIterations_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtPreIterations.TextChanged
        If IsNumeric(TxtPreIterations.Text) Then
            If CInt(TxtPreIterations.Text) < 1 Then
                TxtPreHeight.Text = 0
                TxtPreHeight.Enabled = False
                TxtPreWidth.Text = 0
                TxtPreWidth.Enabled = False
                m_disablePreFilter = True
            Else
                If m_disablePreFilter = True Then
                    TxtPreHeight.Text = 5
                    TxtPreHeight.Enabled = True
                    TxtPreWidth.Enabled = True
                    TxtPreWidth.Text = 5
                    m_disablePreFilter = False
                End If
            End If
        End If
    End Sub

    Private Sub TxtPostIterations_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtPostIterations.TextChanged
        If IsNumeric(TxtPostIterations.Text) Then
            If CInt(TxtPostIterations.Text) < 1 Then
                TxtPostHeight.Text = 0
                TxtPostHeight.Enabled = False
                TxtPostWidth.Text = 0
                TxtPostWidth.Enabled = False
                m_disablePostFilter = True
            Else
                If m_disablePostFilter = True Then
                    TxtPostHeight.Text = 5
                    TxtPostHeight.Enabled = True
                    TxtPostWidth.Enabled = True
                    TxtPostWidth.Text = 5
                    m_disablePostFilter = False
                End If
            End If
        End If
    End Sub

    Private Sub BtnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAbout.Click
        Dim toolHelpForm As FrmHelp = New FrmHelp(BA_HelpTopics.CanopyTemplate)
        toolHelpForm.ShowDialog()
    End Sub

    Public Shared Function GetDefaultTemplate() As TemplateRule
        Dim tRule As New TemplateRule
        '*** Populate rule parameters ***
        tRule.RuleId = 3
        tRule.TemplateName = m_DefaultTemplateName
        tRule.RuleType = HruRuleType.Canopy
        tRule.IsDefault = True
        tRule.MissingValue = -1
        Dim templateActions As New List(Of TemplateAction)

        '*** Pre-filter (Low-Pass) action **
        Dim filterAction1 As New TemplateAction(1)
        filterAction1.actionType = ActionType.LowPassFilter
        filterAction1.addParameter(ActionParameter.RectangleWidth, m_DefaultFilterHeightWidth)
        filterAction1.addParameter(ActionParameter.RectangleHeight, m_DefaultFilterHeightWidth)
        filterAction1.addParameter(ActionParameter.IterationCount, m_DefaultFilterIterations)
        templateActions.Add(filterAction1)

        '*** reclass action ***'
        Dim reclassAction As New TemplateAction(2)
        reclassAction.actionType = ActionType.ReclCont
        reclassAction.addParameter(ActionParameter.ReclassItems, GetDefaultCanopy())
        reclassAction.addParameter(ActionParameter.InputLayer, "\" & BA_EnumDescription(GeodatabaseNames.Layers) & "\" & m_DefaultSelectedLayer)
        reclassAction.addParameter(ActionParameter.ReclassField, m_DefaultSelectedReclassField)
        templateActions.Add(reclassAction)

        '*** Post-filter (Majority) action **
        Dim filterAction2 As New TemplateAction(3)
        filterAction2.actionType = ActionType.MajorityFilter
        filterAction2.addParameter(ActionParameter.RectangleWidth, m_DefaultFilterHeightWidth)
        filterAction2.addParameter(ActionParameter.RectangleHeight, m_DefaultFilterHeightWidth)
        filterAction2.addParameter(ActionParameter.IterationCount, m_DefaultFilterIterations)
        templateActions.Add(filterAction2)

        tRule.TemplateActions = templateActions
        tRule.InputLayerName = m_DefaultSelectedLayer
        Return tRule
    End Function

    Private Shared Function GetDefaultCanopy() As ReclassItem()
        Dim items(1) As ReclassItem
        Dim item1 As ReclassItem = New ReclassItem()
        item1.FromValue = 0
        item1.ToValue = 30
        item1.OutputValue = 1
        items(0) = item1
        Dim item2 As ReclassItem = New ReclassItem()
        item2.FromValue = 30
        item2.ToValue = 101
        item2.OutputValue = 2
        items(1) = item2
        Return items
    End Function

    Public Sub LoadFormFromTemplate(ByVal templateRule As TemplateRule)

        Dim pActions As List(Of TemplateAction) = templateRule.TemplateActions
        CkDefault.Checked = templateRule.IsDefault
        TxtRasterLayer.Text = templateRule.InputLayerName
        For Each pAction In pActions
            ' Initialize filter fields
            If pAction.actionType = ActionType.LowPassFilter Then
                Dim params As Hashtable = pAction.parameters
                TxtPreHeight.Text = CStr(pAction.parameters(ActionParameter.RectangleHeight))
                TxtPreWidth.Text = CStr(pAction.parameters(ActionParameter.RectangleWidth))
                TxtPreIterations.Text = CStr(pAction.parameters(ActionParameter.IterationCount))
            ElseIf pAction.actionType = ActionType.ReclCont Then
                Dim params As Hashtable = pAction.parameters
                m_SelectedReclassField = CStr(pAction.parameters(ActionParameter.ReclassField))
                TxtReclassField.Text = m_SelectedReclassField
                Dim canopyItems As ReclassItem() = params(ActionParameter.ReclassItems)
                Array.Resize(m_reclassItems, canopyItems.Length)
                Array.Copy(canopyItems, m_reclassItems, canopyItems.Length)
                RefreshGrid()
                TxtClasses.Text = GrdCanopy.RowCount
                TxtMinValue.Text = GrdCanopy.Item(idxFromValue, 0).Value
                TxtMaxValue.Text = GrdCanopy.Item(idxToValue, GrdCanopy.RowCount - 1).Value
                m_maxValue = CInt(TxtMaxValue.Text)
                m_inputLayerFolder = CStr(pAction.parameters(ActionParameter.InputLayer))
                GrdCanopy.ReadOnly = False
            ElseIf pAction.actionType = ActionType.MajorityFilter Then
                Dim params As Hashtable = pAction.parameters
                TxtPostHeight.Text = CStr(pAction.parameters(ActionParameter.RectangleHeight))
                TxtPostWidth.Text = CStr(pAction.parameters(ActionParameter.RectangleWidth))
                TxtPostIterations.Text = CStr(pAction.parameters(ActionParameter.IterationCount))
            End If
        Next

    End Sub

    Public Sub LoadReadOnlyFormFromTemplate(ByVal templateRule As TemplateRule)
        TxtRasterLayer.Text = templateRule.InputLayerName
        Dim pActions As List(Of TemplateAction) = templateRule.TemplateActions
        'TxtMissingValue.Text = CStr(templateRule.MissingValue)
        For Each pAction In pActions
            ' Initialize filter fields
            If pAction.actionType = ActionType.LowPassFilter Then
                Dim params As Hashtable = pAction.parameters
                TxtPreHeight.Text = CStr(pAction.parameters(ActionParameter.RectangleHeight))
                TxtPreWidth.Text = CStr(pAction.parameters(ActionParameter.RectangleWidth))
                TxtPreIterations.Text = CStr(pAction.parameters(ActionParameter.IterationCount))
            ElseIf pAction.actionType = ActionType.ReclCont Then
                TxtReclassField.Text = CStr(pAction.parameters(ActionParameter.ReclassField))
                Dim inputLayer As String = CStr(pAction.parameters(ActionParameter.InputLayer))
                Dim layersPath As String = BA_GeodatabasePath(m_aoiPath, GeodatabaseNames.Layers, True)
                m_inputLayerPath = layersPath & templateRule.InputLayerName
                If Not BA_File_Exists(m_inputLayerPath, WorkspaceType.Geodatabase, ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTRasterDataset) Then
                        MessageBox.Show("The input layer (" & m_inputLayerPath & ") is missing for this template. It cannot be run.", "Missing layer", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Exit Sub
                End If
                    Dim params As Hashtable = pAction.parameters
                    m_SelectedReclassField = CStr(pAction.parameters(ActionParameter.ReclassField))
                    Dim canopyItems As ReclassItem() = params(ActionParameter.ReclassItems)
                    Array.Resize(m_reclassItems, canopyItems.Length)
                    Array.Copy(canopyItems, m_reclassItems, canopyItems.Length)
                    RefreshReadOnlyGrid()
                    m_inputLayerFolder = CStr(pAction.parameters(ActionParameter.InputLayer))
                    GrdCanopy.ReadOnly = True
                ElseIf pAction.actionType = ActionType.MajorityFilter Then
                    Dim params As Hashtable = pAction.parameters
                    TxtPostHeight.Text = CStr(pAction.parameters(ActionParameter.RectangleHeight))
                    TxtPostWidth.Text = CStr(pAction.parameters(ActionParameter.RectangleWidth))
                    TxtPostIterations.Text = CStr(pAction.parameters(ActionParameter.IterationCount))
                End If
        Next

    End Sub

    Private Sub RefreshGrid()
        GrdCanopy.Rows.Clear()
        For i As Short = 0 To m_reclassItems.GetUpperBound(0) Step 1
            Dim reclassItem As ReclassItem = m_reclassItems(i)
            Dim displayItem As Object() = {reclassItem.FromValue, reclassItem.ToValue, reclassItem.OutputValue}
            GrdCanopy.Rows.Add(displayItem)
        Next
        Dim readOnlyCell As DataGridViewCell = GrdCanopy.Item(idxToValue, GrdCanopy.RowCount - 1)
        readOnlyCell.ReadOnly = True
        readOnlyCell.Style = GetReadOnlyCell.Style
    End Sub

    Private Sub RefreshReadOnlyGrid()
        GrdCanopy.Rows.Clear()
        For i As Short = 0 To m_reclassItems.GetUpperBound(0) Step 1
            Dim reclassItem As ReclassItem = m_reclassItems(i)
            Dim displayItem As Object() = {reclassItem.FromValue, reclassItem.ToValue, reclassItem.OutputValue}
            GrdCanopy.Rows.Add(displayItem)
            Dim toCell As DataGridViewCell = GrdCanopy.Item(idxToValue, i)
            toCell.Style = GetReadOnlyCell.Style
            Dim outputCell As DataGridViewCell = GrdCanopy.Item(idxOutputValue, i)
            outputCell.Style = GetReadOnlyCell.Style
        Next
    End Sub

    Private Sub initReadOnly()
        CkDefault.Visible = False
        CboOptions.DropDownStyle = ComboBoxStyle.DropDownList
        GrdCanopy.ReadOnly = True
        GrdCanopy.AllowUserToAddRows = False
        GrdCanopy.AllowUserToDeleteRows = False
        LblTemplatesFile.Visible = False
        TxtTemplatesFile.Visible = False
        BtnSelectLyr.Visible = False
        PnlReclass.Visible = False
        Dim pointA As Drawing.Point = New Drawing.Point(4, 38)
        PnlOptions.Location = pointA
        Dim pointB As Drawing.Point = New Drawing.Point(4, 87)
        PnlMain.Location = pointB
        BtnDeleteTemplate.Visible = False
        Me.Height = 520
    End Sub

    Private Sub CboOptions_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CboOptions.SelectedValueChanged
        For Each pRule In m_templateList
            If pRule.TemplateName = CboOptions.SelectedItem Then
                If m_readOnly = True Then
                    LoadReadOnlyFormFromTemplate(pRule)
                Else
                    LoadFormFromTemplate(pRule)
                End If
            End If
        Next
    End Sub

    Private Function GetReadOnlyCell() As DataGridViewCell
        Dim cell As DataGridViewCell = New DataGridViewTextBoxCell()
        cell.Style.BackColor = TxtTemplatesFile.BackColor
        cell.Style.ForeColor = TxtTemplatesFile.ForeColor
        Return cell
    End Function

    Private Sub GrdCanopy_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GrdCanopy.CellEndEdit
        ' Convert user input to Int32 so it can be sorted with existing values
        Dim strA As String = TryCast(GrdCanopy.Item(e.ColumnIndex, e.RowIndex).Value, String)
        Dim intA As Int32 = -1
        If Not String.IsNullOrEmpty(strA) Then
            Integer.TryParse(strA, intA)
            If intA > 0 Then
                GrdCanopy.Item(e.ColumnIndex, e.RowIndex).Value = intA
            End If
        End If
    End Sub

    Private Sub Grdcanopy_CellValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs) _
      Handles GrdCanopy.CellValidating

        Try
            'Call shared cell validation function
            BA_ValidateCell(GrdCanopy, idxToValue, idxFromValue, idxOutputValue, m_maxValue, _
                            m_readOnly, sender, e)

        Catch ex As Exception
            MessageBox.Show("Grdcanopy_CellValidating Exception: " & ex.Message)
        End Try

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
                BA_DeleteTemplateRule(HruRuleType.Canopy, templateName)
                'Reset form
                m_templateList.Clear()
                m_templateList.AddRange(BA_GetTemplates(HruRuleType.Canopy))
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

    Private Sub ClearTemplateEditor()
        CboOptions.ResetText()
        CkDefault.Checked = False
        GrdCanopy.Rows.Clear()
        TxtPreHeight.Text = m_DefaultFilterHeightWidth
        TxtPreWidth.Text = m_DefaultFilterHeightWidth
        TxtPreIterations.Text = m_DefaultFilterIterations
        TxtPostHeight.Text = m_DefaultFilterHeightWidth
        TxtPostWidth.Text = m_DefaultFilterHeightWidth
        TxtPostIterations.Text = m_DefaultFilterIterations
        m_inputLayerFolder = BA_EnumDescription(PublicPath.Canopy)
        TxtMinValue.Text = m_DefaultMinValue
        TxtMaxValue.Text = m_DefaultMaxValue
    End Sub

    Private Sub BtnSelectLyr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSelectLyr.Click
        Dim frmSelect As New FrmSelectTemplateLayer(Nothing)
        frmSelect.ShowDialog()
        If frmSelect.InputLayerPath IsNot Nothing And _
           frmSelect.InputField IsNot Nothing Then
            Dim layerName As String = BA_GetBareName(frmSelect.InputLayerPath)
            TxtRasterLayer.Text = layerName
            TxtReclassField.Text = frmSelect.InputField
            m_inputLayerPath = frmSelect.InputLayerPath
            m_inputLayerFolder = frmSelect.InputLayerFolder
            m_aoiPath = frmSelect.Aoi.FilePath
            GrdCanopy.Rows.Clear()
        Else
            'Do nothing. The cancel button was clicked
        End If
    End Sub

    Private Sub TxtRasterLayer_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtRasterLayer.TextChanged
        If m_readOnly = False Then
            If Not String.IsNullOrEmpty(TxtRasterLayer.Text) And _
               Not String.IsNullOrEmpty(TxtReclassField.Text) Then
                BtnApply.Enabled = True
            Else
                BtnApply.Enabled = False
            End If
        End If
    End Sub

    Private Sub TxtReclassField_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtReclassField.TextChanged
        If m_readOnly = False Then
            If Not String.IsNullOrEmpty(TxtRasterLayer.Text) And _
               Not String.IsNullOrEmpty(TxtReclassField.Text) Then
                BtnApply.Enabled = True
            Else
                BtnApply.Enabled = False
            End If
        End If
    End Sub

    Private Sub BtnReclass_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnReclass.Click
        ' Call shared function to populate reclass table
        BA_BuildReclassTable(GrdCanopy, TxtMinValue, TxtMaxValue, TxtClasses, idxToValue, _
                             BA_GetReadOnlyCell(TxtTemplatesFile), 0)
        ' Cache max value in case the user changes it on us
        m_maxValue = CInt(TxtMaxValue.Text)

    End Sub

End Class