Imports System.Windows.Forms
Imports System.ComponentModel
Imports System.Text
Imports ESRI.ArcGIS.GeoAnalyst
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports BAGIS_ClassLibrary

Public Class FrmPrismPrecipRule

    'Store current aoiPath
    Private m_aoiPath As String
    Private m_listPrismLayers = New ListBox
    Private idxFromValue As Integer = 0
    Private idxToValue As Integer = 1
    Private idxOutputValue As Integer = 2

    Private m_maxValue As Double
    Private m_displayUnits As MeasurementUnit
    Private m_dataUnits As MeasurementUnit


    Public Sub New(ByVal aoiPath As String, ByVal lstPrismLayers As ListBox)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        m_aoiPath = aoiPath
        m_listPrismLayers.Items.AddRange(lstPrismLayers.Items)

        Try
            'Set m_dataUnits from annual prism layer xml data
            m_dataUnits = GetDepthUnits()
            If m_dataUnits = MeasurementUnit.Missing Then
                ' If meta data is missing,
                ' frmUnits will update the m_dataUnits class level variable in this form with user's choice
                'Dim frmUnits As New FrmPrismPrecipUnits(Me, m_aoiPath)
                'frmUnits.ShowDialog()
                Dim sb1 As StringBuilder = New StringBuilder
                sb1.Append("The prism depth units have not been defined" & vbCrLf)
                sb1.Append("for this AOI. The depth units must be" & vbCrLf)
                sb1.Append("defined before using the prism rule." & vbCrLf)
                sb1.Append("BAGIS-H will prompt you to define the units" & vbCrLf)
                sb1.Append("when you select an AOI on the 'Define Zones'" & vbCrLf)
                sb1.Append("screen.")
                MessageBox.Show(sb1.ToString, "Missing Prism Units", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            ' Set default display units to be same as data units
            m_displayUnits = m_dataUnits
        Catch ex As Exception
            Dim sb As New StringBuilder
            sb.Append("The system is unable to determine the precipitation units." & vbCrLf)
            sb.Append("Please contact your System Administrator with this message.")
            MessageBox.Show(sb.ToString, "Precipitation units error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

            '    Debug.Print("Missing precipitation units. Do not show form")
            '    ' If user fails to set data units, bail out of showing prism precip rule form
            '    Exit Sub
            'Else
            ' Otherwise set the radio buttons for the default display unit from the data
            If m_displayUnits = MeasurementUnit.Inches Then
                RdoInches.Checked = True
                RdoMillimeters.Checked = False
            ElseIf m_displayUnits = MeasurementUnit.Millimeters Then
                RdoInches.Checked = False
                RdoMillimeters.Checked = True
            End If


        ' Populate CboPrecipType from PrismDataRange enum
        For Each dRange In [Enum].GetValues(GetType(PrismDataRange))
            Dim EnumConstant As [Enum] = dRange
            Dim fi As Reflection.FieldInfo = EnumConstant.GetType().GetField(EnumConstant.ToString())
            Dim aattr() As DescriptionAttribute = DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
            CboPrecipType.Items.Add(aattr(0).Description)
        Next

        ' Add any initialization after the InitializeComponent() call.
        CboEnd.SelectedIndex = 11
        CboBegin.SelectedIndex = 0
        CboPrecipType.SelectedIndex = 0
    End Sub

    Private Sub CboPrecipType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboPrecipType.SelectedIndexChanged
        If CboPrecipType.SelectedIndex = 5 Then
            CboBegin.Enabled = True
            CboEnd.Enabled = True
            LblFrom.Enabled = True
            LblTo.Enabled = True
        Else
            CboBegin.Enabled = False
            CboBegin.SelectedIndex = 0
            CboEnd.Enabled = False
            CboEnd.SelectedIndex = 11
            LblFrom.Enabled = False
            LblTo.Enabled = False
        End If
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub BtnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        Dim prismPrecipRule As BAGIS_ClassLibrary.IRule = Nothing

        ' Increment ruleId before using
        Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of frmHruZone.AddinImpl)(My.ThisAddIn.IDs.frmHruZone)
        Dim hruZoneForm As frmHruZone = dockWindowAddIn.UI
        Dim ruleId As Integer
        If String.IsNullOrEmpty(TxtRuleId.Text) Then
            ruleId = hruZoneForm.GetNextRuleId
        Else
            ruleId = CInt(TxtRuleId.Text)
        End If
        Dim ruleIdx As Short = CboPrecipType.SelectedIndex
        Dim dataRange As PrismDataRange = CType(ruleIdx, PrismDataRange)
        Dim fMonth As Integer = -1
        Dim lMonth As Integer = -1
        Dim strPath As String = BA_GeodatabasePath(m_aoiPath, GeodatabaseNames.Prism, True)
        Dim aoiPrismFolder As AOIPrismFolderNames
        If dataRange <> PrismDataRange.Custom Then
            Select Case dataRange
                Case PrismDataRange.Annual
                    aoiPrismFolder = AOIPrismFolderNames.annual
                Case PrismDataRange.Q1
                    aoiPrismFolder = AOIPrismFolderNames.q1
                Case PrismDataRange.Q2
                    aoiPrismFolder = AOIPrismFolderNames.q2
                Case PrismDataRange.Q3
                    aoiPrismFolder = AOIPrismFolderNames.q3
                Case PrismDataRange.Q4
                    aoiPrismFolder = AOIPrismFolderNames.q4
            End Select
            If BA_LayerInList(aoiPrismFolder.ToString, m_listPrismLayers) <> True Then
                MessageBox.Show("One or more required Prism layers are missing from this AOI. This Prism Precip rule cannot be run as configured.", "Missing input", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If
        Dim nextFolder As String
        Dim layerName As String
        If dataRange = PrismDataRange.Custom Then
            'handle custom
            nextFolder = Nothing
            layerName = "Custom_PRISM_calculation"
            fMonth = CboBegin.SelectedIndex
            lMonth = CboEnd.SelectedIndex
        Else
            'Add 1 to index for backward compatibility
            nextFolder = BA_GetPrismFolderName(CInt(aoiPrismFolder) + 1)
            layerName = aoiPrismFolder.ToString
        End If

        Dim prismPath = strPath & nextFolder

        Dim displayUnits As MeasurementUnit = MeasurementUnit.Inches
        If RdoMillimeters.Checked = True Then
            displayUnits = MeasurementUnit.Millimeters
        End If
        Dim sliceType As esriGeoAnalysisSliceEnum = esriGeoAnalysisSliceEnum.esriGeoAnalysisSliceEqualInterval
        If RdoEqArea.Checked = True Then
            sliceType = esriGeoAnalysisSliceEnum.esriGeoAnalysisSliceEqualArea
        End If
        If DataGridView1.Rows.Count > 0 Then
            Dim pReclassItems As ReclassItem() = BA_CopySlopeValuesFromGridToArray(DataGridView1, idxFromValue, idxToValue, _
                                                                                   idxOutputValue)
            If pReclassItems IsNot Nothing AndAlso pReclassItems.GetLength(0) > 0 Then
                prismPrecipRule = New PrismPrecipRule(dataRange, fMonth, lMonth, displayUnits, _
                                                      m_dataUnits, sliceType, pReclassItems, layerName, prismPath, ruleId)
            End If
        End If

        'Validate we have supporting data for custom data range
        If dataRange = PrismDataRange.Custom Then
            Dim monthList As List(Of Integer) = BA_PrismMonthList(prismPrecipRule)
            For Each intMonth In monthList
                Dim folderName As AOIPrismFolderNames = CType(intMonth, AOIPrismFolderNames)
                If BA_LayerInList(folderName.ToString, m_listPrismLayers) <> True Then
                    MessageBox.Show("One or more required Prism layers are missing from this AOI. This Prism Precip rule cannot be run as configured.", "Missing input", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            Next
        End If
        hruZoneForm.AddPendingRule(prismPrecipRule)
        BtnCancel_Click(sender, e)
    End Sub

    Public Sub LoadForm(ByVal pRule As BAGIS_ClassLibrary.IRule)
        Dim prismRule As PrismPrecipRule = CType(pRule, PrismPrecipRule)
        CboPrecipType.SelectedIndex = CType(prismRule.DataRange, Integer)
        CboBegin.SelectedIndex = prismRule.fMonthIdx
        CboEnd.SelectedIndex = prismRule.lMonthIdx
        RdoInches.Enabled = True
        RdoMillimeters.Enabled = True
        RdoInches.Checked = True
        m_displayUnits = MeasurementUnit.Inches
        m_dataUnits = prismRule.DataUnits
        If prismRule.DisplayUnits = MeasurementUnit.Millimeters Then
            RdoInches.Checked = False
            RdoMillimeters.Checked = True
            m_displayUnits = MeasurementUnit.Millimeters
        End If

        RdoEqArea.Enabled = True
        RdoEqInterval.Enabled = True
        RdoEqInterval.Checked = True

        DataGridView1.Rows.Clear()
        Dim reclassItems As ReclassItem() = prismRule.ReclassItems
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
        TxtRuleId.Text = CStr(prismRule.RuleId)

        ManageGridButtons()
        ManageReclassButton(False)

    End Sub

    Private Sub BtnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAbout.Click
        Dim toolHelpForm As FrmHelp = New FrmHelp(BA_HelpTopics.PRISM)
        toolHelpForm.ShowDialog()
    End Sub

    Private Sub BtnStats_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnStats.Click
        Dim ruleIdx As Short = CboPrecipType.SelectedIndex
        Dim dataRange As PrismDataRange = CType(ruleIdx, PrismDataRange)
        Dim aoiPrismFolder As AOIPrismFolderNames
        Dim prismFile As String = Nothing
        Dim prismParentPath As String = Nothing
        Dim tempFileName As String = "tmpPrismSum"

        'Reset GUI
        TxtMaxValue.Text = ""
        TxtMinValue.Text = ""
        RdoEqArea.Enabled = False
        RdoEqInterval.Enabled = False
        DataGridView1.Rows.Clear()

        If dataRange <> PrismDataRange.Custom Then
            aoiPrismFolder = GetAoiPrismFolder(dataRange, prismParentPath, prismFile)
            If BA_LayerInList(aoiPrismFolder.ToString, m_listPrismLayers) <> True Then
                MessageBox.Show("One or more required Prism layers are missing from this AOI. This Prism Precip rule cannot be run as configured.", "Missing input", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
        Else
            'code for custom data range
            prismFile = GetCustomAoiPrismFolder(aoiPrismFolder, tempFileName, prismParentPath)
        End If

        'Load statistics for single layers
        Dim selGeoDataset As IGeoDataset = Nothing
        Dim pRasterBandCollection As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pRasterStats As IRasterStatistics = Nothing
        Try
            Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(prismParentPath)
            If workspaceType = workspaceType.Geodatabase Then
                selGeoDataset = BA_OpenRasterFromGDB(prismParentPath, prismFile)
            ElseIf workspaceType = workspaceType.Raster Then
                selGeoDataset = BA_OpenRasterFromFile(prismParentPath, prismFile)
            End If
            pRasterBandCollection = CType(selGeoDataset, IRasterBandCollection)
            pRasterBand = pRasterBandCollection.Item(0)
            Dim validVAT As Boolean = False
            pRasterBand.HasTable(validVAT)
            If validVAT = False Then
                pRasterStats = pRasterBand.Statistics
                If pRasterStats IsNot Nothing Then
                    Dim minimum As Double = pRasterStats.Minimum
                    ' Increment max value in case it is rounded down to catch actual max value
                    Dim maximum As Double = pRasterStats.Maximum + 0.01
                    If m_displayUnits = m_dataUnits Then
                        TxtMinValue.Text = Format(minimum, "######0.##")
                        TxtMaxValue.Text = Format(maximum, "######0.##")
                    ElseIf m_displayUnits = MeasurementUnit.Millimeters And m_dataUnits = MeasurementUnit.Inches Then
                        ' Convert inches to millimeters
                        TxtMinValue.Text = Format(minimum * BA_Inches_To_Millimeters, "######0")
                        TxtMaxValue.Text = Format(maximum * BA_Inches_To_Millimeters, "######0")
                    ElseIf m_displayUnits = MeasurementUnit.Inches And m_dataUnits = MeasurementUnit.Millimeters Then
                        ' Convert millimeters to inches
                        TxtMinValue.Text = Format(minimum / BA_Inches_To_Millimeters, "######0.##")
                        TxtMaxValue.Text = Format(maximum / BA_Inches_To_Millimeters, "######0.##")
                    End If
                Else
                    TxtMinValue.Text = "Not available"
                    TxtMaxValue.Text = "Not available"
                End If
            Else
                TxtMinValue.Text = "Not available"
                TxtMaxValue.Text = "Not available"
            End If
            RdoEqArea.Enabled = True
            RdoEqInterval.Enabled = True
            RdoInches.Enabled = True
            RdoMillimeters.Enabled = True
            If BA_File_Exists(prismParentPath & "\" & tempFileName, workspaceType.Raster, esriDatasetType.esriDTRasterDataset) Then
                BA_Remove_Raster(prismParentPath, tempFileName)
            End If
        Catch Ex As Exception
            MessageBox.Show("BtnStats_Click Exception: " + Ex.Message)
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterStats)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBand)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBandCollection)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(selGeoDataset)
        End Try

    End Sub

    'This code is used by BtnStats and BtnReclass
    Private Function GetAoiPrismFolder(ByVal dataRange As PrismDataRange, ByRef prismParentPath As String, ByRef prismFile As String) As AOIPrismFolderNames
        Dim aoiPrismFolder As AOIPrismFolderNames
        Select Case dataRange
            Case PrismDataRange.Annual
                aoiPrismFolder = AOIPrismFolderNames.annual
            Case PrismDataRange.Q1
                aoiPrismFolder = AOIPrismFolderNames.q1
            Case PrismDataRange.Q2
                aoiPrismFolder = AOIPrismFolderNames.q2
            Case PrismDataRange.Q3
                aoiPrismFolder = AOIPrismFolderNames.q3
            Case PrismDataRange.Q4
                aoiPrismFolder = AOIPrismFolderNames.q4
        End Select
        prismParentPath = m_aoiPath & "\" & BA_EnumDescription(GeodatabaseNames.Prism)
        prismFile = BA_GetPrismFolderName(CInt(aoiPrismFolder) + 1)
    End Function

    Private Function GetCustomAoiPrismFolder(ByVal aoiPrismFolder As AOIPrismFolderNames, ByVal tempFileName As String, ByRef prismParentPath As String) As String
        'code for custom data range
        'temporary rule to feed BA_PrismMonthList function
        Dim tmpPrismRule As PrismPrecipRule = New PrismPrecipRule(aoiPrismFolder, CboBegin.SelectedIndex, CboEnd.SelectedIndex, _
                                                                    MeasurementUnit.Inches, MeasurementUnit.Inches, _
                                                                    esriGeoAnalysisSliceEnum.esriGeoAnalysisSliceEqualInterval, _
                                                                    Nothing, Nothing, Nothing, 0)
        Dim monthList As List(Of Integer) = BA_PrismMonthList(tmpPrismRule)
        'Dim strPath As String = BA_GetPath(m_aoiPath, PublicPath.PRISM)
        Dim strPath As String = m_aoiPath & "\" & BA_EnumDescription(GeodatabaseNames.Prism)
        Dim nextFolder As String
        ' Build list of filePaths from list of months
        Dim filePaths As New List(Of String)
        For Each pMonth In monthList
            nextFolder = BA_GetPrismFolderName(pMonth + 1)
            Dim prismPath = strPath & "\" & nextFolder
            filePaths.Add(prismPath)
        Next
        ' Call function to sum the raster bands together
        Dim success As BA_ReturnCode = BA_SumRasterBands(filePaths, m_aoiPath, m_aoiPath, tempFileName)
        If success = BA_ReturnCode.Success Then
            prismParentPath = m_aoiPath
        End If
        Return tempFileName
    End Function

    Private Sub TxtMaxValue_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TxtMaxValue.Validating
        Dim isValid As Boolean = ValidateReclassText(DirectCast(sender, TextBox))
        ManageReclassButton(isValid)
    End Sub

    Private Sub TxtMinValue_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TxtMinValue.Validating
        Dim isValid As Boolean = ValidateReclassText(DirectCast(sender, TextBox))
        ManageReclassButton(isValid)
    End Sub

    Private Sub TxtClasses_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TxtClasses.Validating
        Dim isValid As Boolean = ValidateReclassText(DirectCast(sender, TextBox))
        ManageReclassButton(isValid)
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

    Private Function ValidateReclassText(ByVal text_box As TextBox) As Boolean
        If String.IsNullOrEmpty(text_box.Text) Then
            Return True
        ElseIf Not IsNumeric(text_box.Text) Then
            MessageBox.Show("Value must be numeric", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error)
            text_box.Focus()
            Return False
        End If
    End Function

    Private Sub ManageReclassButton(ByVal fieldErrors As Boolean)
        If fieldErrors = False Then
            If Not String.IsNullOrEmpty(TxtMaxValue.Text) _
                    And Not String.IsNullOrEmpty(TxtMinValue.Text) _
                    And Not String.IsNullOrEmpty(TxtClasses.Text) Then
                BtnReclass.Enabled = True
            Else
                BtnReclass.Enabled = False
            End If
        Else
            BtnReclass.Enabled = False
        End If
    End Sub

    Private Sub ManageGridButtons()
        If DataGridView1.Rows.Count > 0 Then
            BtnSave.Enabled = True
            BtnClear.Enabled = True
        End If
    End Sub

    Private Sub BtnReclass_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnReclass.Click
        Dim decimalPlaces As Short = 2
        If m_displayUnits = MeasurementUnit.Millimeters Then
            decimalPlaces = 0
        End If
        If RdoEqInterval.Checked = True Then
            ' Call shared function to populate reclass table
            BA_BuildReclassTable(DataGridView1, TxtMinValue, TxtMaxValue, TxtClasses, idxToValue, _
                                 BA_GetReadOnlyCell(TxtTemplatesFile), decimalPlaces)
        Else
            Dim dataRange As PrismDataRange = CType(CboPrecipType.SelectedIndex, PrismDataRange)
            Dim aoiPrismFolder As AOIPrismFolderNames
            Dim tempFileName As String = "tmpPrismSum"
            Dim prismParentPath As String = Nothing
            Dim prismFile As String = Nothing
            If dataRange <> PrismDataRange.Custom Then
                aoiPrismFolder = GetAoiPrismFolder(dataRange, prismParentPath, prismFile)
            Else
                'code for custom data range
                prismFile = GetCustomAoiPrismFolder(aoiPrismFolder, tempFileName, prismParentPath)
            End If
            BA_BuildEqualAreaReclassTable(DataGridView1, m_aoiPath, prismParentPath, prismFile, TxtClasses, idxToValue, _
                                          BA_GetReadOnlyCell(TxtTemplatesFile), decimalPlaces, m_displayUnits, _
                                          m_dataUnits)
            ' Clean up temp file for custom calculations
            If BA_File_ExistsIDEUtil(m_aoiPath & "\" & tempFileName) Then
                BA_Remove_Raster(m_aoiPath, tempFileName)
            End If
        End If
        ' Cache max value in case the user changes it on us
        m_maxValue = CInt(TxtMaxValue.Text)
        ManageGridButtons()
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
            ManageGridButtons()

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

    Private Sub RdoMillimeters_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RdoMillimeters.CheckedChanged
        If m_displayUnits = MeasurementUnit.Inches And RdoMillimeters.Checked = True Then
            If DataGridView1.Rows.Count > 0 Then
                Dim rowCount As Integer = DataGridView1.Rows.Count
                For i = 0 To rowCount - 1
                    Dim nextRow As DataGridViewRow = DataGridView1.Rows(i)
                    Dim fromValue As Double = CDbl(nextRow.Cells(idxFromValue).Value)
                    Dim strFromValue As String = Format(fromValue * BA_Inches_To_Millimeters, "######0")
                    Dim toValue As Double = CDbl(nextRow.Cells(idxToValue).Value)
                    Dim strToValue As String = Format(toValue * BA_Inches_To_Millimeters, "######0")
                    nextRow.Cells(idxFromValue).Value = strFromValue
                    nextRow.Cells(idxToValue).Value = strToValue
                Next
            End If
            If Not String.IsNullOrEmpty(TxtMaxValue.Text) Then
                If IsNumeric(TxtMaxValue.Text) Then
                    Dim maxValue As Double = CDbl(TxtMaxValue.Text)
                    Dim strMaxValue As String = Format(maxValue * BA_Inches_To_Millimeters, "######0")
                    TxtMaxValue.Text = strMaxValue
                End If
            End If
            If Not String.IsNullOrEmpty(TxtMinValue.Text) Then
                If IsNumeric(TxtMinValue.Text) Then
                    Dim maxValue As Double = CDbl(TxtMinValue.Text)
                    Dim strMaxValue As String = Format(maxValue * BA_Inches_To_Millimeters, "######0")
                    TxtMinValue.Text = strMaxValue
                End If
            End If
            m_displayUnits = MeasurementUnit.Millimeters
        End If
    End Sub

    Private Sub RdoInches_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RdoInches.CheckedChanged
        If m_displayUnits = MeasurementUnit.Millimeters And RdoInches.Checked = True Then
            If DataGridView1.Rows.Count > 0 Then
                Dim rowCount As Integer = DataGridView1.Rows.Count
                For i = 0 To rowCount - 1
                    Dim nextRow As DataGridViewRow = DataGridView1.Rows(i)
                    Dim fromValue As Double = CDbl(nextRow.Cells(idxFromValue).Value)
                    Dim strFromValue As String = Format(fromValue / BA_Inches_To_Millimeters, "######0.##")
                    Dim toValue As Double = CDbl(nextRow.Cells(idxToValue).Value)
                    Dim strToValue As String = Format(toValue / BA_Inches_To_Millimeters, "######0.##")
                    nextRow.Cells(idxFromValue).Value = strFromValue
                    nextRow.Cells(idxToValue).Value = strToValue
                Next
            End If
            If Not String.IsNullOrEmpty(TxtMaxValue.Text) Then
                If IsNumeric(TxtMaxValue.Text) Then
                    Dim maxValue As Double = CDbl(TxtMaxValue.Text)
                    Dim strMaxValue As String = Format(maxValue / BA_Inches_To_Millimeters, "######0.##")
                    TxtMaxValue.Text = strMaxValue
                End If
            End If
            If Not String.IsNullOrEmpty(TxtMinValue.Text) Then
                If IsNumeric(TxtMinValue.Text) Then
                    Dim maxValue As Double = CDbl(TxtMinValue.Text)
                    Dim strMaxValue As String = Format(maxValue / BA_Inches_To_Millimeters, "######0.##")
                    TxtMinValue.Text = strMaxValue
                End If
            End If
            m_displayUnits = MeasurementUnit.Inches
        End If
    End Sub

    Public Property DataUnits As MeasurementUnit
        Get
            Return m_dataUnits
        End Get
        Set(ByVal value As MeasurementUnit)
            m_dataUnits = value
        End Set
    End Property

    'Note: The prism units are always stored in the annual prism layer
    Private Function GetDepthUnits() As MeasurementUnit
        Dim pUnits As MeasurementUnit = MeasurementUnit.Missing
        Dim inputFolder As String = BA_GeodatabasePath(m_aoiPath, GeodatabaseNames.Prism)
        Dim inputFile As String = AOIPrismFolderNames.annual.ToString
        Dim tagsList As IList(Of String) = BA_ReadMetaData(inputFolder, inputFile, _
                                           LayerType.Raster, BA_XPATH_TAGS)
        If tagsList IsNot Nothing Then
            For Each pInnerText As String In tagsList
                'This is our BAGIS tag
                If pInnerText.IndexOf(BA_BAGIS_TAG_PREFIX) = 0 Then
                    Dim strUnits As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_VALUE_TAG)
                    If strUnits IsNot Nothing Then
                        pUnits = BA_GetMeasurementUnit(strUnits)
                        Return pUnits
                    End If
                End If
            Next
        End If
        Return pUnits
    End Function

    Private Sub DataGridView1_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles DataGridView1.RowsAdded
        If DataGridView1.RowCount > 0 Then
            BtnApply.Enabled = True
        Else
            BtnApply.Enabled = False
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