Imports System.Windows.Forms
Imports System.IO
Imports System.Text
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Geodatabase
Imports BAGIS_ClassLibrary

Module TemplateModule

    ' If there is no local copy of the templates, gets default template from each form
    ' And saves to BAGIS directory as XML
    Private Function SaveDefaultTemplates() As TemplateCollection
        Try
            Dim hruExt As HruExtension = HruExtension.GetExtension
            Dim templatesFullPath As String = BA_GetTemplatesFullPath()
            Dim templateColl As TemplateCollection = Nothing
            If templatesFullPath IsNot Nothing Then
                If BA_File_ExistsWindowsIO(templatesFullPath) Then
                    ' Check template version
                    Dim currentVersion As String = hruExt.TemplateVersion
                    templateColl = LoadTemplatesFromXml()
                        If templateColl.Version = currentVersion Then
                            Return templateColl
                        Else
                            Dim settingsPath As String = hruExt.SettingsPath
                            Dim oldTemplatesFullPath As String = settingsPath & BA_EnumDescription(PublicPath.OldTemplates)
                            MessageBox.Show("Your existing templates file is obsolete. A copy will be saved to " _
                                            & oldTemplatesFullPath & ". An updated file with default template definitions " _
                                            & "will be generated and used.", "Obsolete templates file", MessageBoxButtons.OK, _
                                            MessageBoxIcon.Warning)
                            BackupSettingsFile(oldTemplatesFullPath)
                        End If
                    End If
                    Dim templateCollection As New TemplateCollection
                    Dim templateArray(2) As TemplateRule
                    Dim lulcTemplate As TemplateRule = FrmLandUseTemplate.GetDefaultTemplate
                    templateArray(0) = lulcTemplate
                    Dim slopeTemplate As TemplateRule = FrmSlopeTemplate.GetDefaultTemplate
                    templateArray(1) = slopeTemplate
                    Dim canopyTemplate As TemplateRule = FrmCanopyTemplate.GetDefaultTemplate
                    templateArray(2) = canopyTemplate
                    templateCollection.TemplateArray = templateArray
                    templateCollection.Version = hruExt.TemplateVersion
                    templateCollection.Save(templatesFullPath)
                    Return templateCollection
                Else
                    Return Nothing
                End If
        Catch ex As Exception
            MsgBox(ex.Message)
            Return Nothing
        End Try
    End Function

    ' Loop through the templates and return a list of those with desired rule type
    Public Function BA_GetTemplates(ByVal ruleType As HruRuleType) As List(Of TemplateRule)
        Dim templateColl As TemplateCollection = SaveDefaultTemplates()
        If templateColl IsNot Nothing Then
            Dim templateList As New List(Of TemplateRule)
            For Each pRule In templateColl.TemplateArray
                If pRule.RuleType = ruleType Then
                    templateList.Add(pRule)
                End If
            Next
            Return templateList
        Else
            Return Nothing
        End If
    End Function

    Private Function LoadTemplatesFromXml() As TemplateCollection
        Dim hruExt As HruExtension = HruExtension.GetExtension
        Dim settingsPath As String = hruExt.SettingsPath
        If settingsPath IsNot Nothing Then
            Dim templatesFullPath As String = settingsPath & BA_EnumDescription(PublicPath.Templates)
            If BA_File_ExistsWindowsIO(templatesFullPath) Then
                Dim obj As Object = SerializableData.Load(templatesFullPath, GetType(TemplateCollection))
                If obj IsNot Nothing Then
                    Dim pCollect As TemplateCollection = CType(obj, TemplateCollection)
                    Return pCollect
                Else
                    Return Nothing
                End If
            Else
                Return Nothing
            End If
        End If
        Return Nothing
    End Function

    Public Sub BA_AddUpdateTemplateRule(ByVal templateRule As TemplateRule)
        Dim templateColl As TemplateCollection = LoadTemplatesFromXml()
        Dim i As Integer
        Dim updateTemplate As Boolean = False
        Dim lastId As Integer = 0
        Dim allTemplates As TemplateRule() = Nothing
        If templateColl IsNot Nothing Then
            allTemplates = templateColl.TemplateArray
            For Each item In allTemplates
                If item.RuleType = templateRule.RuleType Then
                    If item.TemplateName = templateRule.TemplateName Then
                        allTemplates(i) = templateRule
                        updateTemplate = True
                        Exit For
                    End If
                End If
                lastId = item.RuleId
                i += 1
            Next
            If updateTemplate = False Then
                Array.Resize(allTemplates, i + 1)
                templateRule.RuleId = lastId + 1
                allTemplates(i) = templateRule
            End If
        End If
        Dim defaultIndexes As New List(Of Integer)
        Dim j As Integer
        For Each item In allTemplates
            If item.RuleType = templateRule.RuleType And item.IsDefault = True Then
                defaultIndexes.Add(j)
            End If
            j += 1
        Next
        If defaultIndexes.Count = 1 Then
            ' Do Nothing; We have one default
        ElseIf defaultIndexes.Count = 0 Then
            ' The last update must have deleted the default; switch it back
            Dim defaultTemplate As TemplateRule = allTemplates(i)
            defaultTemplate.IsDefault = True
            allTemplates(i) = defaultTemplate
        Else
            ' We have > 1 default template; Only the updated one s/b default
            For Each idx In defaultIndexes
                If idx <> i Then
                    Dim notDefaultTemplate As TemplateRule = allTemplates(idx)
                    notDefaultTemplate.IsDefault = False
                    allTemplates(idx) = notDefaultTemplate
                End If
            Next
        End If

        ' Persist changes to xml
        If allTemplates IsNot Nothing Then
            templateColl.TemplateArray = allTemplates
        End If
        Dim hruExt As HruExtension = HruExtension.GetExtension
        Dim settingsPath As String = hruExt.SettingsPath
        If settingsPath IsNot Nothing Then
            Dim templatesFullPath As String = settingsPath & BA_EnumDescription(PublicPath.Templates)
            templateColl.Save(templatesFullPath)
        End If
    End Sub

    Public Sub BA_DeleteTemplateRule(ByVal templateRuleType As String, _
                                     ByVal templateName As String)
        Dim templateColl As TemplateCollection = LoadTemplatesFromXml()
        Dim deleteTemplate As Boolean = False
        Dim i As Integer
        Dim newTemplates(0) As TemplateRule
        Dim deleteDefault As Boolean
        If templateColl IsNot Nothing Then
            Array.Resize(newTemplates, templateColl.TemplateArray.Length)
            For Each item In templateColl.TemplateArray
                If item.RuleType = templateRuleType Then
                    If item.TemplateName <> templateName Then
                        ' Keep the item in the template collection; It is not the 'deleted' template
                        newTemplates(i) = item
                    Else
                        If item.IsDefault = True Then
                            deleteDefault = True
                        End If
                    End If
                Else
                    ' Keep the item in the template collection; It is a different type than the default template
                    newTemplates(i) = item
                End If
                i += 1
            Next
        End If
        'Pick another default template if default is deleted
        If deleteDefault = True Then
            For Each item In newTemplates
                If item.RuleType = templateRuleType Then
                    ' Pick the first item of that type you find
                    item.IsDefault = True
                    Exit For
                End If
            Next
        End If
        Array.Resize(newTemplates, i - 1)
        ' Persist changes to xml
        If newTemplates IsNot Nothing Then
            templateColl.TemplateArray = newTemplates
        End If
        Dim hruExt As HruExtension = HruExtension.GetExtension
        Dim settingsPath As String = hruExt.SettingsPath
        If settingsPath IsNot Nothing Then
            Dim templatesFullPath As String = settingsPath & BA_EnumDescription(PublicPath.Templates)
            templateColl.Save(templatesFullPath)
        End If
    End Sub

    Public Function BA_GetTemplatesFullPath() As String
        Dim hruExt As HruExtension = HruExtension.GetExtension
        Dim settingsPath As String = hruExt.SettingsPath
        If settingsPath IsNot Nothing Then
            Return settingsPath & BA_EnumDescription(PublicPath.Templates)
        Else
            Return Nothing
        End If
    End Function

    Private Sub BackupSettingsFile(ByVal backupPath As String)
        Try
            ' Ensure that the target does not exist.
            If File.Exists(backupPath) Then
                File.Delete(backupPath)
            End If

            ' Move the file.
            Dim newPath As String = BA_GetTemplatesFullPath()
            File.Move(newPath, backupPath)

            ' See if the original file exists now. It shouldn't
            If File.Exists(newPath) Then
                Throw New Exception("The original settings file still exists, which is unexpected.")
            End If

        Catch ex As Exception
            MessageBox.Show("BackupSettingsFile: " & ex.Message)
        End Try
    End Sub

    ' Validate cells on continuous reclass tables: canopy, slope, continuous reclass
    Public Sub BA_ValidateCell(ByVal grid As DataGridView, ByVal idxToValue As Integer, _
                               ByVal idxFromValue As Integer, ByVal idxOutputValue As Integer, _
                               ByVal maxValue As Integer, ByVal blnReadOnly As Boolean, _
                               ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellValidatingEventArgs)
        ' Validating to values
        If e.ColumnIndex = idxToValue And blnReadOnly = False Then
            Dim toValue As Double
            If Not Double.TryParse(e.FormattedValue, toValue) Then
                MessageBox.Show("Value must be numeric")
                If grid.EditingControl IsNot Nothing Then grid.EditingControl.Text = Nothing
                e.Cancel = True
            Else
                Dim fromValue As Double = 0
                Double.TryParse(grid.Item(idxFromValue, e.RowIndex).Value, fromValue)
                If toValue < fromValue Then
                    MessageBox.Show("From value must be greater than to value.")
                    If grid.EditingControl IsNot Nothing Then grid.EditingControl.Text = Nothing
                    e.Cancel = True
                    Exit Sub
                End If
                ' Validate To value unless last row. Last row is read-only
                If toValue > (maxValue - 1) And e.RowIndex < grid.RowCount - 1 Then
                    MessageBox.Show("To value cannot exceed maximum value for reclass.")
                    If grid.EditingControl IsNot Nothing Then grid.EditingControl.Text = Nothing
                    e.Cancel = True
                    Exit Sub
                End If
                If e.RowIndex < (grid.RowCount - 1) Then
                    grid.Item(idxFromValue, e.RowIndex + 1).Value = e.FormattedValue
                End If
            End If
        ElseIf e.ColumnIndex = idxOutputValue Then
            If Not Integer.TryParse(e.FormattedValue, Nothing) Then
                MessageBox.Show("Value must be an integer")
                If grid.EditingControl IsNot Nothing Then grid.EditingControl.Text = Nothing
                e.Cancel = True
            End If
        End If


    End Sub

    ' Populate dataGridView object when provided with min, max values and number of classes
    ' from UI. Used by editable continuous reclass grids like slope and canopy
    Public Sub BA_BuildReclassTable(ByVal grid As DataGridView, ByVal txtMinValue As TextBox, _
                                    ByVal txtMaxValue As TextBox, ByVal txtClasses As TextBox, _
                                    ByVal idxToValue As Integer, ByVal styleCell As DataGridViewCell, _
                                    ByVal decimalPlaces As Int32)
        ' Ensure input values are numeric
        If Not IsNumeric(txtMinValue.Text) Then
            MessageBox.Show("Numeric value required for minimum")
            txtMinValue.Focus()
            Exit Sub
        End If
        If Not IsNumeric(txtMaxValue.Text) Then
            MessageBox.Show("Numeric value required for maximum")
            txtMaxValue.Focus()
            Exit Sub
        End If
        If Not IsNumeric(txtClasses.Text) Then
            MessageBox.Show("Numeric value required for # classes")
            txtClasses.Focus()
            Exit Sub
        End If

        ' Convert input values to integers
        ' Add 1/100 to catch outliers with rounding
        Dim minValue As Double = CDbl(txtMinValue.Text) - 0.01
        Dim maxValue As Double = CDbl(txtMaxValue.Text) + 0.01
        Dim numClasses As Integer = CInt(txtClasses.Text)

        ' Calculate class interval
        Dim interval As Double = (maxValue - minValue) / numClasses

        ' Populate table
        grid.Rows.Clear()
        Dim fromVal As Double = minValue
        Dim toVal As Double = 0
        For i = 1 To (numClasses)
            If i < numClasses Then
                toVal = Math.Round(fromVal + interval, decimalPlaces)
            Else
                toVal = maxValue
            End If
            Dim nextLine As Object() = {fromVal, toVal, i}
            grid.Rows.Add(nextLine)
            fromVal = toVal
        Next

        ' Set read-only formats
        Dim readOnlyCell As DataGridViewCell = grid.Item(idxToValue, grid.RowCount - 1)
        readOnlyCell.ReadOnly = True
        readOnlyCell.Style = styleCell.Style
    End Sub

    ' Return a cell to use as template for formatting
    Public Function BA_GetReadOnlyCell(ByVal copyTextBox As TextBox) As DataGridViewCell
        Dim cell As DataGridViewCell = New DataGridViewTextBoxCell()
        cell.Style.BackColor = copyTextBox.BackColor
        cell.Style.ForeColor = copyTextBox.ForeColor
        Return cell
    End Function

    '@ToDo: Verify parent hru exists if zones are selected for rules
    Public Function BA_ValidateHruTemplate(ByVal templateAoiPath As String, ByVal outputAoiPath As String, _
                                           ByVal hruList As List(Of Hru)) As List(Of String)
        Dim returnList = New List(Of String)
        Dim hruCount As Int32 = hruList.Count - 1
        For i = hruCount To 0 Step -1
            Dim nextHru As Hru = hruList.Item(i)
            If nextHru.RuleList IsNot Nothing And nextHru.RuleList.Count > 0 Then
                ' This Hru was created by a ruleset
                ' Sort the rules by ruleId
                nextHru.RuleList.Sort()
                ' Validate presence of input layer for hru rules
                For Each pRule In nextHru.RuleList
                    Dim inputFolderPath As String = pRule.InputFolderPath
                    Dim strippedFolderPath As String = inputFolderPath.Substring(templateAoiPath.Length)
                    Dim newFolderPath As String = outputAoiPath & strippedFolderPath
                    Dim datasetType As esriDatasetType = esriDatasetType.esriDTRasterDataset
                    ' Make an exception for DAFlow because it is based on a vector
                    If TypeOf pRule Is DAFlowTypeZonesRule Then
                        datasetType = esriDatasetType.esriDTFeatureClass
                    End If
                    If Not BA_File_Exists(newFolderPath, WorkspaceType.Geodatabase, datasetType) Then
                        Dim sb As New StringBuilder()
                        sb.Append("Unable to locate input layer for ")
                        sb.Append(nextHru.Name & " ")
                        sb.Append(pRule.RuleTypeText)
                        sb.Append(" rule: ")
                        sb.Append(newFolderPath)
                        sb.Append(" in output AOI path. This HRU template cannot be used.")
                        returnList.Add(sb.ToString)
                    End If
                Next
            ElseIf nextHru.CookieCutProcess IsNot Nothing Then
                ' This hru was created by a cookie cut/stamp process
                Dim cookieCut As CookieCutProcess = nextHru.CookieCutProcess
                Dim inputFolderPath As String = cookieCut.CookieCutPath
                Dim strippedFolderPath As String = inputFolderPath.Substring(templateAoiPath.Length)
                Dim newFolderPath As String = outputAoiPath & strippedFolderPath & cookieCut.CookieCutName
                'Debug.Print(cookieCutFullPath)
                Dim pWorkspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(newFolderPath)
                If Not BA_File_Exists(newFolderPath, pWorkspaceType, esriDatasetType.esriDTRasterDataset) Then
                    Dim sb As New StringBuilder
                    sb.Append("Unable to locate input layer for hru: ")
                    sb.Append(nextHru.Name & ", ")
                    sb.Append(cookieCut.Mode)
                    sb.Append(" process in output AOI path ")
                    sb.Append(newFolderPath)
                    sb.Append(". If this input layer is not created")
                    sb.Append(" by an earlier template step, the process will fail.")
                    returnList.Add(sb.ToString)
                End If
            ElseIf nextHru.EliminateProcess IsNot Nothing Then
                ' This hru was created by the eliminate tool
                Dim elimProcess As EliminateProcess = nextHru.EliminateProcess
                Dim elimName As String = nextHru.ParentHru.Name
                Dim elimFullPath As String = BA_GetHruPathGDB(outputAoiPath, PublicPath.HruDirectory, elimName)
                elimFullPath = elimFullPath & BA_EnumDescription(PublicPath.HruGrid)
                'Debug.Print(elimCutFullPath)
                Dim pWorkspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(elimFullPath)
                If Not BA_File_Exists(elimFullPath, pWorkspaceType, esriDatasetType.esriDTRasterDataset) Then
                    Dim sb As New StringBuilder
                    sb.Append("Unable to locate input layer for hru: ")
                    sb.Append(nextHru.Name & ", ")
                    sb.Append("Eliminate process in output AOI path ")
                    sb.Append(elimFullPath)
                    sb.Append(". If this input layer is not created")
                    sb.Append(" by an earlier template step, the process will fail.")
                    returnList.Add(sb.ToString)
                End If
            ElseIf nextHru.ReclassZonesRule IsNot Nothing Then
                ' This hru was created by the reclass zones tool
                Dim reclassRule As RasterReclassRule = nextHru.ReclassZonesRule
                Dim parentName As String = nextHru.ParentHru.Name
                Dim parentFullPath As String = BA_GetHruPathGDB(outputAoiPath, PublicPath.HruDirectory, parentName)
                parentFullPath = parentFullPath & BA_EnumDescription(PublicPath.HruGrid)
                Dim pWorkspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(parentFullPath)
                If Not BA_File_Exists(parentFullPath, pWorkspaceType, esriDatasetType.esriDTRasterDataset) Then
                    Dim sb As New StringBuilder
                    sb.Append("Unable to locate input layer for hru: ")
                    sb.Append(nextHru.Name & ", ")
                    sb.Append("ReclassZones process in output AOI path ")
                    sb.Append(parentFullPath)
                    sb.Append(". If this input layer is not created")
                    sb.Append(" by an earlier template step, the process will fail.")
                    returnList.Add(sb.ToString)
                End If
            End If
        Next

        Return returnList
    End Function

    Public Function BA_GenerateHruFromTemplate(ByVal templateAoiPath As String, ByVal pAoi As Aoi, _
                                               ByVal lstHru As List(Of Hru), ByVal buttons As Control(), _
                                               ByVal pictures As Control()) As BA_ReturnCode
        ' Create/configure a step progressor
        Dim pStepProg As ESRI.ArcGIS.esriSystem.IStepProgressor = Nothing
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As ESRI.ArcGIS.Framework.IProgressDialog2 = Nothing

        ' The mask file path will be used by all tasks for this template hru
        Dim maskFilePath As String = BA_GeodatabasePath(pAoi.FilePath, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiVector)
        Dim snapRasterPath As String = BA_GeodatabasePath(pAoi.FilePath, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
        ' Filled DEM Path
        Dim fullLayerPath As String = BA_GeodatabasePath(pAoi.FilePath, GeodatabaseNames.Surfaces, True) & BA_EnumDescription(MapsFileName.filled_dem_gdb)

        Try
            'Ensure it is okay to delete any existing hru folders before we begin. If not
            'Bail out of the process
            'Also configure number of steps in step counter in this process
            Dim stepCount As Integer = 5
            For Each tempHru In lstHru
                Dim hruOutputPath As String = BA_GetHruPath(pAoi.FilePath, PublicPath.HruDirectory, tempHru.Name)
                If BA_Workspace_Exists(hruOutputPath) Then
                    Dim result As DialogResult = MessageBox.Show("HRU directory " & tempHru.Name & " already exists. Overwrite existing directory ?", "Folder exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If result = DialogResult.Yes Then
                        Dim success As BA_ReturnCode = BA_DeleteHRU(hruOutputPath, My.Document)
                        If success <> BA_ReturnCode.Success Then
                            MessageBox.Show("Unable to delete HRU '" & tempHru.Name & "'. Please restart ArcMap and try again", "Unable to delete HRU", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            Exit Function
                        End If
                    Else
                        Return BA_ReturnCode.NotSupportedOperation
                    End If
                End If
                If tempHru.RuleList IsNot Nothing AndAlso tempHru.RuleList.Count > 0 Then
                    stepCount = stepCount + tempHru.RuleList.Count
                End If
                If tempHru.CookieCutProcess IsNot Nothing Then
                    stepCount += 2
                End If
                If tempHru.EliminateProcess IsNot Nothing Then
                    stepCount += 4
                End If
                If tempHru.ReclassZonesRule IsNot Nothing Then
                    stepCount += 4
                End If
                stepCount += 1
            Next

            pStepProg = BA_GetStepProgressor(My.ArcMap.Application.hWnd, stepCount)
            ' Create/configure the ProgressDialog. This automatically displays the dialog
            progressDialog2 = BA_GetProgressDialog(pStepProg, "Generating HRUs...", "Generate HRUs")

            'get raster resolution
            Dim cellSize As Double
            Dim rasterStat As IRasterStatistics = BA_GetRasterStatsGDB(fullLayerPath, cellSize)

            For Each tempHru In lstHru
                Dim hruOutputPath As String = BA_GetHruPath(pAoi.FilePath, PublicPath.HruDirectory, tempHru.Name)
                Dim hruOutputPath2 As String = BA_GetHruPathGDB(pAoi.FilePath, PublicPath.HruDirectory, tempHru.Name)
                BA_CreateHruOutputFolders(pAoi.FilePath, tempHru.Name)
                ' Create new file GDB for HRU
                BA_CreateHruOutputGDB(pAoi.FilePath, tempHru.Name)
                pStepProg.Step()

                '*** Execute Hru rules if present ***
                Dim pendingRules As New List(Of BAGIS_ClassLibrary.IRule)
                If tempHru.RuleList IsNot Nothing AndAlso tempHru.RuleList.Count > 0 Then
                    ' List of paths to rule layers to combine
                    pendingRules.AddRange(tempHru.RuleList)
                    Dim layerPathList As New List(Of String)
                    For Each pRule As BAGIS_ClassLibrary.IRule In pendingRules
                        'modify input file path so it points to target hru
                        Dim inputFolderPath As String = pRule.InputFolderPath
                        Dim strippedFolderPath As String = inputFolderPath.Substring(templateAoiPath.Length)
                        Dim newFolderPath As String = pAoi.FilePath & strippedFolderPath
                        pRule.InputFolderPath = newFolderPath
                        Dim success As BA_ReturnCode = BA_RunRule(pAoi.FilePath, hruOutputPath2, pRule)
                        If success = BA_ReturnCode.Success Then
                            Dim ruleFilePath As String = hruOutputPath2 & "\" & pRule.OutputDatasetName
                            layerPathList.Add(ruleFilePath)
                        End If
                        pStepProg.Step()
                    Next
                    Dim returnVal As BA_ReturnCode = BA_ReturnCode.UnknownError
                    Dim hruInputPath As String
                    Dim parentHru As Hru = Nothing
                    Dim selectZonesArray As Zone() = Nothing
                    If tempHru.ParentHru IsNot Nothing Then
                        selectZonesArray = tempHru.SelectedZones
                        hruInputPath = BA_GetHruPath(pAoi.FilePath, PublicPath.HruDirectory, tempHru.ParentHru.Name)
                        Dim zoneHruFullPath As String = BA_GetHruPathGDB(pAoi.FilePath, PublicPath.HruDirectory, tempHru.ParentHru.Name) & BA_EnumDescription(PublicPath.HruGrid)
                        returnVal = BA_CreateHruFromZones(maskFilePath, zoneHruFullPath, _
                                                          layerPathList, tempHru.ApplyToSelectedZones, selectZonesArray, _
                                                          hruOutputPath2, GRID, snapRasterPath)
                        If returnVal = BA_ReturnCode.Success Then
                            Dim aoi As Aoi = BA_LoadHRUFromXml(hruInputPath)
                            For Each pHru In aoi.HruList
                                ' We found the hru the user selected
                                If String.Compare(pHru.Name, tempHru.ParentHru.Name) = 0 Then
                                    parentHru = pHru
                                End If
                            Next
                        End If
                    Else
                        returnVal = BA_ZoneOverlay(maskFilePath, layerPathList, hruOutputPath2, GRID, False, _
                                                   True, snapRasterPath, WorkspaceType.Geodatabase)
                    End If
                    pStepProg.Step()

                    If returnVal = BA_ReturnCode.Success Then
                        Dim rInputPath As String = hruOutputPath2 & BA_EnumDescription(PublicPath.HruGrid)
                        Dim vOutputPath As String = hruOutputPath2 & BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruVector), False, True)
                        Dim vReturnVal As Short = BA_Raster2PolygonShapefileFromPath(rInputPath, vOutputPath, False)

                        returnVal = BA_ProcessNonContiguousGrids(tempHru.AllowNonContiguousHru, vOutputPath, _
                                                     hruOutputPath2, cellSize, snapRasterPath)
                        If returnVal = BA_ReturnCode.Success Then
                            pStepProg.Step()

                            If tempHru.RetainSourceAttributes = True Then
                                Dim parentPath As String = Nothing
                                If parentHru IsNot Nothing Then
                                    parentPath = parentHru.FilePath & GRID
                                End If
                                returnVal = BA_AddAttributesToHru(pAoi.FilePath, hruOutputPath2, hruOutputPath2, _
                                                                  pendingRules, parentPath)
                                pStepProg.Step()
                            End If

                            Dim pHru As Hru = BA_CreateHru(tempHru.Name, rInputPath, vOutputPath, selectZonesArray, _
                                                           pendingRules, tempHru.AllowNonContiguousHru)
                            pHru.RetainSourceAttributes = tempHru.RetainSourceAttributes
                            If parentHru IsNot Nothing Then pHru.ParentHru = parentHru
                            pStepProg.Step()

                            PersistHru(pAoi, pHru, hruOutputPath)
                            UpdateCheckmark(pHru.Name, buttons, pictures)
                        End If
                        'MessageBox.Show("New HRU " & tempHru.Name & " successfully created", "HRU: " & tempHru.Name, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If

                '*** EXECUTE COOKIE-CUT/STAMP IF PRESENT ***
                If tempHru.CookieCutProcess IsNot Nothing Then
                    Dim returnVal As BA_ReturnCode = BA_ReturnCode.UnknownError
                    Dim parentFullPath As String = BA_GetHruPath(pAoi.FilePath, PublicPath.HruDirectory, tempHru.ParentHru.Name)
                    Dim parentFullPath2 As String = BA_GetHruPathGDB(pAoi.FilePath, PublicPath.HruDirectory, tempHru.ParentHru.Name)
                    Dim cookieCut As CookieCutProcess = tempHru.CookieCutProcess
                    'Trim file extension
                    If maskFilePath.Contains(".shp") Then
                        maskFilePath = Strings.Left(maskFilePath, Len(maskFilePath) - Len(".shp"))
                    End If
                    Dim selectValuesArray() As Long = cookieCut.SelectedValues
                    Dim parentHru As Hru = Nothing
                    Dim cookieCutHru As Hru = Nothing
                    Dim useSelectedZones As Boolean = True
                    Dim allowNonContiguous As Boolean
                    Dim inputFolderPath As String = cookieCut.CookieCutPath
                    Dim strippedFolderPath As String = inputFolderPath.Substring(templateAoiPath.Length)
                    Dim newFolderPath As String = pAoi.FilePath & strippedFolderPath & cookieCut.CookieCutName
                    If cookieCut.Mode = BA_MODE_STAMP Then
                        Dim tempCookieCutValues As Long() = BA_ReadValuesFromRaster(newFolderPath, cookieCut.CookieCutField)
                        returnVal = BA_StampHru(maskFilePath, newFolderPath, tempCookieCutValues, _
                                                selectValuesArray, parentFullPath2 & BA_EnumDescription(PublicPath.HruGrid), _
                                                hruOutputPath2, GRID, snapRasterPath, pAoi.FilePath)
                    ElseIf cookieCut.Mode = BA_MODE_COOKIE_CUT Then
                        returnVal = BA_CookieCutterHru(maskFilePath, newFolderPath, cookieCut.CookieCutField, _
                                                       selectValuesArray, _
                                                       parentFullPath2 & BA_EnumDescription(PublicPath.HruGrid), _
                                                       hruOutputPath2, GRID, cookieCut.PreserveAoiBoundary, _
                                                       snapRasterPath, pAoi.FilePath)
                    End If

                    If returnVal = BA_ReturnCode.Success Then
                        Dim aoi As Aoi = BA_LoadHRUFromXml(parentFullPath)
                        For Each pHru In aoi.HruList
                            ' We found the parent hru
                            If String.Compare(pHru.Name, tempHru.ParentHru.Name) = 0 Then
                                parentHru = pHru
                            End If
                        Next

                        Dim cookieCutAllowNonContiguous = False
                        'Execute this if using cookie cut hru
                        If Not String.IsNullOrEmpty(cookieCut.HruPath) Then
                            Dim cookieCutAoi As Aoi = BA_LoadHRUFromXml(cookieCut.HruPath)
                            If cookieCutAoi IsNot Nothing Then
                                For Each pHru In cookieCutAoi.HruList
                                    ' We found the cookie cut hru
                                    If String.Compare(pHru.Name, cookieCut.CookieCutHruName) = 0 Then
                                        cookieCutHru = pHru
                                        cookieCutAllowNonContiguous = cookieCutHru.AllowNonContiguousHru
                                    End If
                                Next
                            End If
                        End If
                        If parentHru.AllowNonContiguousHru = True Or cookieCutAllowNonContiguous = True Then
                            allowNonContiguous = True
                        End If
                        pStepProg.Step()

                        Dim rInputPath As String = hruOutputPath2 & BA_EnumDescription(PublicPath.HruGrid)
                        Dim vOutputPath As String = hruOutputPath2 & BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruVector), False, True)
                        Dim vReturnVal As Short = BA_Raster2PolygonShapefileFromPath(rInputPath, vOutputPath, False)

                        returnVal = BA_ProcessNonContiguousGrids(tempHru.AllowNonContiguousHru, vOutputPath, _
                                                     hruOutputPath2, cellSize, snapRasterPath)

                        If returnVal = BA_ReturnCode.Success Then
                            ' Placeholder for rules
                            Dim rules As List(Of BAGIS_ClassLibrary.IRule) = New List(Of BAGIS_ClassLibrary.IRule)
                            Dim newHru As Hru = BA_CreateHru(tempHru.Name, rInputPath, vOutputPath, Nothing, _
                                                           rules, allowNonContiguous)
                            If cookieCut.Mode = BA_MODE_COOKIE_CUT Then
                                newHru.RetainSourceAttributes = tempHru.RetainSourceAttributes
                                If newHru.RetainSourceAttributes = True And parentHru IsNot Nothing Then
                                    Dim origHru As Hru = parentHru
                                    Dim parentPath As String = Nothing
                                    Dim ruleFilePath As String = BA_GetHruPathGDB(pAoi.FilePath, PublicPath.HruDirectory, origHru.Name)
                                    If origHru IsNot Nothing AndAlso origHru.ParentHru IsNot Nothing Then
                                        parentPath = origHru.ParentHru.FilePath & GRID
                                    End If
                                    BA_AddAttributesToHru(pAoi.FilePath, hruOutputPath2, ruleFilePath, origHru.RuleList, parentPath)
                                    pStepProg.Step()
                                End If
                            End If

                            If parentHru IsNot Nothing Then newHru.ParentHru = parentHru
                            Dim cookieCutProcess As New CookieCutProcess(cookieCut.Mode, pAoi.FilePath & strippedFolderPath, cookieCut.CookieCutName, cookieCut.CookieCutField, _
                                                                         selectValuesArray, cookieCut.CookieCutIsHru, cookieCut.PreserveAoiBoundary)
                            'Set hru name in cookieCutProcess if cookieCut layer is an hru
                            If cookieCutProcess.CookieCutIsHru = True Then
                                cookieCutProcess.CookieCutHruName = cookieCut.CookieCutHruName
                            End If
                            newHru.CookieCutProcess = cookieCutProcess
                            pStepProg.Step()

                            PersistHru(pAoi, newHru, hruOutputPath)
                            UpdateCheckmark(newHru.Name, buttons, pictures)
                        End If
                    End If
                End If

                '*** Execute Eliminate process if present ***
                If tempHru.EliminateProcess IsNot Nothing Then
                    Dim elimProcess1 As EliminateProcess = tempHru.EliminateProcess

                    Dim vName As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruVector), False, True)
                    Dim featurePath As String = BA_GetHruPath(pAoi.FilePath, PublicPath.HruDirectory, tempHru.ParentHru.Name)
                    Dim featurePath2 As String = BA_GetHruPathGDB(pAoi.FilePath, PublicPath.HruDirectory, tempHru.ParentHru.Name)
                    featurePath2 = BA_StandardizePathString(featurePath2)
                    Dim rInputPath As String = hruOutputPath2 & BA_EnumDescription(PublicPath.HruGrid)
                    Dim vOutputPath As String = hruOutputPath2 & vName

                    If vName(0) = "\" Then
                        vName = vName.Remove(0, 1)
                    End If

                    'Calculate inputs for BA_Eliminate method
                    Dim area As Double
                    If elimProcess1.SelectByPercentile = True Then
                        Dim areaPct As Double = elimProcess1.AreaPercent
                        area = BA_GetPercentileValueFromFeatureClass(vName, featurePath2, BA_FIELD_AREA_SQKM, areaPct)
                    Else
                        area = elimProcess1.AreaInSqKm
                    End If

                    BA_EliminatePoly(featurePath2, vName, hruOutputPath2, vName, elimProcess1.SelectionMethod, area, BA_FIELD_AREA_SQKM)
                    pStepProg.Step()

                    'add HRUID_CO and HRUID_NC fields to the Vector file
                    If BA_AddCTAndNonCTToAttrib(vOutputPath) <> BA_ReturnCode.Success Then
                        Throw New Exception("HRU Template: Error adding CT and NonCT to Shape file.")
                    End If
                    pStepProg.Step()

                    If cellSize <= 0 Then cellSize = 30

                    'create raster HRU
                    Dim rasName As String = BA_EnumDescription(PublicPath.HruGrid)
                    If rasName(0) = "\" Then
                        rasName = rasName.Remove(0, 1)
                    End If

                    'Dim vFeatureClass As IFeatureClass = BA_OpenFeatureClassFromGDB(hruOutputPath2, vName)
                    'If vFeatureClass IsNot Nothing Then
                    '    If tempHru.AllowNonContiguousHru Then
                    '        BA_Feature2RasterInteger(vFeatureClass, hruOutputPath2, rasName, cellSize, BA_FIELD_HRUID_NC, snapRasterPath)
                    '    Else
                    '        BA_Feature2RasterInteger(vFeatureClass, hruOutputPath2, rasName, cellSize, BA_FIELD_HRUID_CO, snapRasterPath)
                    '    End If
                    '    vFeatureClass = Nothing
                    'Else
                    '    Throw New Exception("HRU Template: Error converting HRU vector to raster.")
                    'End If

                    'This code is copied from FrmEliminatePoly
                    Dim inFeaturesPath As String = hruOutputPath2 & "\" & vName

                    Dim outRasterPath As String = hruOutputPath2 & BA_EnumDescription(PublicPath.HruGrid)
                    If tempHru.AllowNonContiguousHru Then
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
                    Dim pHru As Hru = BA_CreateHru(tempHru.Name, rInputPath, vOutputPath, Nothing, _
                                                   Nothing, tempHru.AllowNonContiguousHru)
                    'add eliminate details to log
                    'the maximum area of the polygons to be deleted in units selected on the form
                    Dim tempArea As Double = area
                    Dim newElimProcess As EliminateProcess = Nothing
                    Dim polyAreaUnits As MeasurementUnit = elimProcess1.PolygonAreaUnits
                    'update tempElimPolygons value
                    Dim originalCount As Long = BA_GetFeatureCount(featurePath2, vName)
                    Dim newcount As Long = BA_GetFeatureCount(hruOutputPath2, vName)
                    'the number of polygons that were eliminated
                    Dim tempElimPolygons As Long = originalCount - newcount

                    ' Calculate area to store depending on units of measure
                    Select Case polyAreaUnits
                        Case MeasurementUnit.Acres
                            tempArea = area * BA_SQKm_To_ACRE
                        Case MeasurementUnit.SquareKilometers
                            'Do nothing; area is already in sq km
                        Case MeasurementUnit.SquareMiles
                            tempArea = area * BA_SQKm_To_SQMile
                    End Select

                    If elimProcess1.SelectByPercentile = True Then
                        newElimProcess = New EliminateProcess(elimProcess1.SelectionMethod, True, tempArea, elimProcess1.AreaPercent, polyAreaUnits, tempElimPolygons)
                    Else
                        newElimProcess = New EliminateProcess(elimProcess1.SelectionMethod, True, tempArea, polyAreaUnits, tempElimPolygons)
                    End If
                    pHru.EliminateProcess = newElimProcess
                    pHru.RetainSourceAttributes = tempHru.RetainSourceAttributes

                    Dim aoi As Aoi = BA_LoadHRUFromXml(featurePath)
                    For Each parentHru As Hru In aoi.HruList
                        ' We found the hru the user selected
                        If String.Compare(parentHru.Name, tempHru.ParentHru.Name) = 0 Then
                            pHru.ParentHru = parentHru
                        End If
                    Next
                    pStepProg.Step()
                    If pHru.RetainSourceAttributes = True And pHru.ParentHru IsNot Nothing Then
                        Dim origHru As Hru = pHru.ParentHru
                        Dim parentPath As String = Nothing
                        Dim ruleFilePath As String = BA_GetHruPathGDB(pAoi.FilePath, PublicPath.HruDirectory, origHru.Name)
                        If origHru IsNot Nothing AndAlso origHru.ParentHru IsNot Nothing Then
                            parentPath = origHru.ParentHru.FilePath & GRID
                        End If
                        BA_AddAttributesToHru(pAoi.FilePath, hruOutputPath2, ruleFilePath, origHru.RuleList, parentPath)
                        pStepProg.Step()
                    End If

                    PersistHru(pAoi, pHru, hruOutputPath)
                    UpdateCheckmark(pHru.Name, buttons, pictures)
                End If

                '*** Execute ReclassZones process if present ***
                If tempHru.ReclassZonesRule IsNot Nothing Then
                    Dim reclassRule As RasterReclassRule = tempHru.ReclassZonesRule
                    Dim reclassItems As ReclassItem() = reclassRule.ReclassItems
                    If reclassItems IsNot Nothing AndAlso reclassItems.Length > 0 Then
                        Dim outputFolderPath As String = hruOutputPath2 & BA_EnumDescription(PublicPath.HruGrid)
                        Dim inputFolderPath As String = reclassRule.InputFolderPath
                        Dim strippedFolderPath As String = inputFolderPath.Substring(templateAoiPath.Length)
                        Dim newFolderPath As String = pAoi.FilePath & strippedFolderPath
                        Dim success As BA_ReturnCode = BA_ReclassifyRasterFromTableWithNoData(newFolderPath, BA_FIELD_VALUE, _
                                                                                    reclassItems, outputFolderPath, _
                                                                                    snapRasterPath)
                        If success = BA_ReturnCode.Success Then
                            reclassRule = New RasterReclassRule(reclassRule.InputLayerName, BA_FIELD_VALUE, newFolderPath, 0)
                            reclassRule.ReclassItems = reclassItems
                            Dim hruInputPath As String = BA_GetHruPath(pAoi.FilePath, PublicPath.HruDirectory, tempHru.ParentHru.Name)
                            Dim aoi As Aoi = BA_LoadHRUFromXml(hruInputPath)
                            Dim parentHru As Hru = Nothing
                            For Each pHru In aoi.HruList
                                ' We found the parent hru
                                If String.Compare(pHru.Name, reclassRule.InputLayerName) = 0 Then
                                    parentHru = pHru
                                End If
                            Next
                            pStepProg.Step()

                            Dim rInputPath As String = hruOutputPath2 & BA_EnumDescription(PublicPath.HruGrid)
                            Dim vOutputPath As String = BA_StandardizePathString(hruOutputPath2, True) & BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruVector), False)
                            Dim vReturnVal As Short = BA_Raster2PolygonShapefileFromPath(rInputPath, vOutputPath, False)
                            success = BA_ProcessNonContiguousGrids(parentHru.AllowNonContiguousHru, vOutputPath, _
                                                                   hruOutputPath2, cellSize, snapRasterPath)
                            If success = BA_ReturnCode.Success Then
                                pStepProg.Step()

                                ' Placeholder for rules
                                Dim rules As List(Of BAGIS_ClassLibrary.IRule) = New List(Of BAGIS_ClassLibrary.IRule)
                                Dim pHru As Hru = BA_CreateHru(tempHru.Name, rInputPath, vOutputPath, Nothing, _
                                                               rules, parentHru.AllowNonContiguousHru)
                                pHru.RetainSourceAttributes = tempHru.RetainSourceAttributes
                                If parentHru IsNot Nothing Then pHru.ParentHru = parentHru
                                pHru.ReclassZonesRule = reclassRule

                                If pHru.RetainSourceAttributes = True And pHru.ParentHru IsNot Nothing Then
                                    Dim origHru As Hru = pHru.ParentHru
                                    Dim parentPath As String = Nothing
                                    Dim ruleFilePath As String = BA_GetHruPathGDB(pAoi.FilePath, PublicPath.HruDirectory, origHru.Name)
                                    If origHru IsNot Nothing AndAlso origHru.ParentHru IsNot Nothing Then
                                        parentPath = origHru.ParentHru.FilePath & GRID
                                    End If
                                    BA_AddAttributesToHru(pAoi.FilePath, hruOutputPath2, ruleFilePath, origHru.RuleList, parentPath)
                                    pStepProg.Step()
                                End If


                                Dim pHruList As IList(Of Hru) = New List(Of Hru)
                                pHruList.Add(pHru)
                                PersistHru(pAoi, pHru, hruOutputPath)
                                UpdateCheckmark(pHru.Name, buttons, pictures)
                                pStepProg.Step()
                            End If
                        End If
                    End If
                End If
            Next
            Dim finalHru As Hru = lstHru.Item(lstHru.Count - 1)
            progressDialog2.HideDialog()
            MessageBox.Show("HRU's successfully created from Template HRU " & finalHru.Name, "HRU: " & finalHru.Name, MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("BA_GenerateHruFromTemplate() Exception: " & ex.Message)
        Finally
            ' Clean up step progressor
            If progressDialog2 IsNot Nothing Then
                progressDialog2.HideDialog()
            End If
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStepProg)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(progressDialog2)
        End Try
    End Function

    ' Turn on the checkmark picture next to the button with the name of the hru that
    ' just completed processing
    Private Sub UpdateCheckmark(ByVal hruName As String, ByVal buttons As Control(), _
                                ByVal pictures As Control())
        If buttons IsNot Nothing Then
            Dim idx As Integer
            For Each ctrl In buttons
                If ctrl.Text.IndexOf(hruName) > -1 Then
                    Dim selPicture As Control = pictures(idx)
                    selPicture.Visible = True
                    Exit For
                End If
                idx += 1
            Next
        End If
    End Sub

    ' Persist the new hru in an XML file
    Private Sub PersistHru(ByVal pAoi As Aoi, ByVal newHru As Hru, ByVal hruOutputPath As String)
        Dim pHruList As IList(Of Hru) = New List(Of Hru)
        pHruList.Add(newHru)
        pAoi.HruList = pHruList
        Dim xmlOutputPath As String = hruOutputPath & BA_EnumDescription(PublicPath.HruXml)
        'MessageBox.Show("7. Generating XML")
        pAoi.Save(xmlOutputPath)
    End Sub

    ' Validate that the input string is a valid integer > 0
    Public Function BA_ValidInteger(ByVal objValue As Object, ByVal maxValue As Int32) As Boolean
        If objValue IsNot Nothing Then
            If IsNumeric(objValue) AndAlso (System.Math.Abs(Val(objValue) Mod 1) = 0) Then
                If CInt(objValue) > -1 And CInt(objValue) <= maxValue Then
                    Return True
                End If
            End If
        End If
        Return False
    End Function

End Module
