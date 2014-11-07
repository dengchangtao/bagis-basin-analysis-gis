Imports BAGIS_ClassLibrary
Imports System.Windows.Forms
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.Geoprocessing
Imports ESRI.ArcGIS.GeoprocessingUI
Imports System.Text

Public Class FrmEditMethod

    Dim m_profile As Profile
    Dim m_method As Method
    Dim m_methodTable As Hashtable
    Dim m_paramTable As Hashtable
    Dim m_aoiPath As String
    Dim m_parameterLabels As List(Of String)
    Dim m_parameterInputControls As List(Of Control)
    Dim m_parentForm As FrmProfileBuilder = Nothing
    Const TXT_PREFIX As String = "txt"
    Const SELECT_ONE As String = "Select one"
    Const CBO_PREFIX As String = "cbo"

    Public Sub New(ByVal frmProfileBuilder As FrmProfileBuilder, ByVal selProfile As Profile, _
                   ByVal selMethod As Method, ByVal aoiPath As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_profile = selProfile
        m_aoiPath = aoiPath
        m_parentForm = frmProfileBuilder

        If frmProfileBuilder.MethodTable IsNot Nothing Then
            m_methodTable = New Hashtable
            For Each pKey In frmProfileBuilder.MethodTable.Keys
                m_methodTable.Add(pKey, frmProfileBuilder.MethodTable(pKey))
            Next
        End If

        LoadMethodList(selMethod)
    End Sub

    Public Sub New(ByVal frmProfileBuilder As FrmProfileBuilder, ByVal aoiPath As String, ByVal selProfile As Profile)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_parentForm = frmProfileBuilder
        m_profile = selProfile

        If m_parentForm.MethodTable IsNot Nothing Then
            m_methodTable = New Hashtable
            For Each pKey In m_parentForm.MethodTable.Keys
                m_methodTable.Add(pKey, m_parentForm.MethodTable(pKey))
            Next
        End If

        m_aoiPath = aoiPath
        LoadMethodList(Nothing)
        PositionCloseAndSaveButtons()
    End Sub

    'This constructor is called from BtnPublicMethodEditor; There is no parent form
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'Load the method list from the public settings file
        Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        Dim settingsPath As String = bExt.SettingsPath
        Dim methodsFolder As String = BAGIS_ClassLibrary.BA_GetPublicMethodsPath(settingsPath)
        If Not String.IsNullOrEmpty(methodsFolder) Then
            m_methodTable = New Hashtable
            Dim methodList As List(Of Method) = BA_LoadMethodsFromXml(methodsFolder)
            If methodList IsNot Nothing Then
                For Each nextMethod In methodList
                    m_methodTable.Add(nextMethod.Name, nextMethod)
                Next
            End If
        End If

        LoadMethodList(Nothing)
        PositionCloseAndSaveButtons()
    End Sub

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    Private Sub BtnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        If ValidateParameters() <> BA_ReturnCode.Success Then
            Exit Sub
        End If
        BtnApply.Enabled = False
        Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        Dim settingsPath As String = bExt.SettingsPath
        Dim methodsPath As String = BA_GetPublicMethodsPath(settingsPath)
        If Not String.IsNullOrEmpty(m_aoiPath) Then
            methodsPath = BA_GetLocalMethodsDir(m_aoiPath)
        End If
        Dim pName = Trim(TxtModelName.Text)
        'We have a folder to save the method
        If methodsPath IsNot Nothing Then
            'If name duplication, be sure user wants to overwrite existing method
            Dim oldMethod As Method = m_methodTable(pName)
            If oldMethod IsNot Nothing Then
                If m_method IsNot Nothing Then
                    'We are editing an existing method and the id doesn't match a method with the same name
                    If m_method.Id <> oldMethod.Id Then
                        Dim result As DialogResult = MessageBox.Show("You are about to overwrite an existing method '" & pName & "'. Do you wish to continue?", "Existing method", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                        If result = DialogResult.No Then
                            TxtMethod.Focus()
                            BtnApply.Enabled = True
                            Exit Sub
                        End If
                    End If
                Else
                    'We are creating a new method but we already have one with the same name
                    Dim result As DialogResult = MessageBox.Show("You are about to overwrite an existing method '" & pName & "'. Do you wish to continue?", "Existing method", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                    If result = DialogResult.No Then
                        TxtMethod.Focus()
                        BtnApply.Enabled = True
                        Exit Sub
                    End If
                End If
            End If
            SaveParametersFromForm()
            Dim modelParams As List(Of ModelParameter) = New List(Of ModelParameter)
            Dim paramId As Int16 = 1
            For Each key As String In m_paramTable.Keys
                Dim pModel As ModelParameter = m_paramTable(key)
                pModel.Id = paramId
                modelParams.Add(pModel)
                paramId += 1
            Next
            If m_method IsNot Nothing Then
                'We are updating an existing method
                'If the name changed, delete the old method and xml
                If m_method.Name <> Trim(TxtModelName.Text) Then
                    'Delete the xml file if it exists
                    Dim success As BA_ReturnCode = BA_ReturnCode.Success
                    If BA_File_ExistsWindowsIO(BA_BagisPXmlPath(methodsPath, m_method.Name)) Then
                        success = BA_Remove_File(BA_BagisPXmlPath(methodsPath, m_method.Name))
                    End If
                    m_methodTable.Remove(m_method.Name)
                End If
                m_method.Name = Trim(TxtModelName.Text)
                m_method.ToolboxName = Trim(TxtToolboxName.Text)
                m_method.ModelLabel = Trim(TxtModelName.Text)
                m_method.ModelName = LblModelName.Text
                m_method.ToolBoxPath = Trim(TxtToolboxPath.Text)
                m_method.Description = TxtDescription.Text
                m_method.ModelParameters = modelParams
                m_method.Save(BA_BagisPXmlPath(methodsPath, m_method.Name))
            Else
                'We are creating a new method
                Dim id As Integer = GetNextMethodId()
                m_method = New Method(id, pName, TxtDescription.Text, Trim(TxtToolboxName.Text), _
                                      LblModelName.Text, TxtModelName.Text, Trim(TxtToolboxPath.Text))
                m_method.ModelParameters = modelParams
                m_method.Save(BA_BagisPXmlPath(methodsPath, m_method.Name))
            End If
            m_methodTable.Item(m_method.Name) = m_method
            'Set updated methodTable in parent form, if applicable
            If m_parentForm IsNot Nothing Then
                m_parentForm.MethodTable = m_methodTable
            End If
            If m_profile IsNot Nothing Then
                'Attach method name to profile if it isn't already
                If m_profile.MethodNames IsNot Nothing Then
                    Dim foundIt As Boolean = False
                    For Each mName As String In m_profile.MethodNames
                        If mName = pName Then
                            foundIt = True
                            Exit For
                        End If
                    Next
                    If foundIt = False Then
                        m_profile.MethodNames.Add(pName)
                    End If
                End If
                'Persist changes to xml
                Dim profilesPath As String = BA_GetPublicProfilesPath(settingsPath)
                If Not String.IsNullOrEmpty(m_aoiPath) Then
                    profilesPath = BA_GetLocalProfilesDir(m_aoiPath)
                End If
                m_profile.Save(BA_BagisPXmlPath(profilesPath, m_profile.Name))
            End If
        End If
        ResetForm()
    End Sub

    'Find the highest current id and then add 1 to get the next valid id
    Private Function GetNextMethodId() As Integer
        Dim id As Integer = 0
        Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        Dim settingsPath As String = bExt.SettingsPath
        Dim publicMethodsPath As String = BA_GetPublicMethodsPath(settingsPath)
        Dim allMethods As List(Of Method) = BA_LoadMethodsFromXml(publicMethodsPath)
        If allMethods IsNot Nothing Then
            For Each nextMethod In allMethods
                If nextMethod.Id > id Then
                    id = nextMethod.Id
                End If
            Next
        End If
        Return id + 1
    End Function

    Private Sub BtnToolbox_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnModel.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        'Dim pGxCatalog As IGxCatalog = New GxCatalogClass()
        Dim pGxCatalog As IGxCatalog = New GxCatalog
        Dim DataPath As String
        Dim pFilter As IGxObjectFilter = New GxFilterTools
        Dim model As IGPTool = Nothing

        Try

            ' Find location of methods folder to set as default for dialog
            Dim bExt As BagisPExtension = BagisPExtension.GetExtension
            Dim settingsPath As String = bExt.SettingsPath
            Dim longPathName As String = Nothing
            Dim methodsFolder As String = BA_GetPublicMethodsPath(settingsPath)
            'Note: ArcCatalog cannot handle using the old DOS path names as the starting folder
            If Not String.IsNullOrEmpty(methodsFolder) Then
                longPathName = BA_GetLongName(methodsFolder)
            End If
            pGxCatalog.Location = longPathName

            'initialize and open mini browser
            With pGxDialog
                .AllowMultiSelect = False
                .ButtonCaption = "Select"
                .Title = "Select Model"
                .ObjectFilter = pFilter
                .StartingLocation = longPathName
                bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
            End With

            If bObjectSelected = False Then Exit Sub

            'get the name of the selected tool; Underlying object is a GxTool
            Dim pGxTool As IGxObject = pGxObject.Next
            DataPath = pGxTool.FullName
            If String.IsNullOrEmpty(DataPath) Then Exit Sub 'user cancelled the action
            Dim pGxToolbox As IGxObject = pGxTool.Parent

            Dim toolboxPath As String = ""
            Dim toolboxName As String = BA_GetBareName(pGxToolbox.FullName, toolboxPath)
            toolboxPath = BA_StandardizePathString(toolboxPath)
            Dim toolLabel As String = BA_GetBareName(DataPath)
            Dim nameString As String = pGxTool.InternalObjectName.NameString
            Dim toolName As String = BA_GetBareName(nameString)

            TxtToolboxPath.Text = toolboxPath
            TxtModelName.Text = toolLabel
            TxtMethod.Text = toolLabel
            TxtToolboxName.Text = toolboxName
            LblModelName.Text = toolName
            RemoveAllLabelsAndControls()

            model = BA_OpenModel(toolboxPath, toolboxName, toolName)
            If model IsNot Nothing Then
                AddParametersToForm(model)
                TxtDescription.Text = model.Description
            End If
            PositionCloseAndSaveButtons()
        Catch ex As Exception
            Debug.Print("BtnToolbox_Click Exception: " & ex.Message)
        Finally
            pGxDialog = Nothing
            pGxObject = Nothing
            pGxCatalog = Nothing
            pFilter = Nothing
            model = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub

    'Private Sub AddParametersToGrid(ByVal pModel As IGPTool)
    '    DataGridView1.Rows.Clear()
    '    If pModel IsNot Nothing Then
    '        Dim paramList As List(Of ModelParameter) = BA_GetModelParameters(pModel)
    '        If paramList IsNot Nothing Then
    '            For Each mParam In paramList
    '                '---create a row---
    '                Dim item As New DataGridViewRow
    '                item.CreateCells(DataGridView1)
    '                With item
    '                    .Cells(0).Value = mParam.Name
    '                    .Cells(1).Value = ""
    '                    .Cells(2).Value = ""
    '                End With
    '                '---add the row---
    '                DataGridView1.Rows.Add(item)
    '            Next
    '            If m_method IsNot Nothing AndAlso m_method.ModelParameters IsNot Nothing Then
    '                For Each mParam As ModelParameter In m_method.ModelParameters
    '                    For i As Integer = 0 To DataGridView1.Rows.Count - 1
    '                        Dim paramName As String = CStr(DataGridView1.Rows(i).Cells(0).Value)
    '                        'Overwrite paramValue if it exists in method being loaded
    '                        If paramName = mParam.Name Then
    '                            DataGridView1.Rows(i).Cells(1).Value = mParam.Value
    '                        End If
    '                    Next
    '                Next
    '            End If
    '        End If
    '    End If
    'End Sub

    'Private Function CopyGridToModelParameters() As List(Of ModelParameter)
    '    Dim paramList As List(Of ModelParameter) = Nothing
    '    If DataGridView1.Rows.Count > 0 Then
    '        paramList = New List(Of ModelParameter)
    '        For i As Integer = 0 To DataGridView1.Rows.Count - 1
    '            Dim item As DataGridViewRow = DataGridView1.Rows(i)
    '            Dim id As Int16 = i + 1
    '            Dim modelParameter = New ModelParameter(id, CStr(item.Cells(0).Value), CStr(item.Cells(1).Value))
    '            paramList.Add(modelParameter)
    '        Next
    '    End If
    '    Return paramList
    'End Function

    Private Sub AddParametersToForm(ByVal pModel As IGPTool)
        If pModel IsNot Nothing Then
            Dim pos As Int16 = 1
            m_paramTable = New Hashtable
            Dim paramList As List(Of ModelParameter) = BA_GetModelParameters(pModel)
            Dim hasUserParams As Boolean = False    'Indicates if we should display the Model Parameters: header
            Dim xValueLabel As Integer = 277
            Dim yValueLabel As Integer = 207
            Dim xValueInput As Integer = 470
            Dim yValueInput As Integer = 207

            'Dim testDbParam As New ModelParameter(9, "db_blah", Nothing)
            'paramList.Add(testDbParam)
            If paramList IsNot Nothing Then
                'Copy the set of parameters to a form-level Hashtable
                For Each mParam In paramList
                    m_paramTable.Add(mParam.Name, mParam)
                    'Screen out the system params; They are not displayed on the form
                    If mParam.Name.Substring(0, BA_PARAM_PREFIX.Length).ToLower <> BA_PARAM_PREFIX Then
                        If mParam.Name.Substring(0, BA_DATABIN_PREFIX.Length).ToLower = BA_DATABIN_PREFIX Then
                            'This is a data-bin param
                            hasUserParams = True
                        ElseIf mParam.Name.Substring(0, BA_FIELD_PREFIX.Length).ToLower = BA_FIELD_PREFIX Then
                            'Do nothing; This is a field name and will be displayed programmatically
                        Else
                            'This is a user param
                            hasUserParams = True
                        End If
                    End If
                Next
                'Check parameters in the selected method if there is one
                If m_method IsNot Nothing Then
                    'Replace the parameter in the Hashtable with one from the selected method
                    'if it exists
                    If m_method.ModelParameters IsNot Nothing AndAlso m_method.ModelParameters IsNot Nothing Then
                        For Each mParam In m_method.ModelParameters
                            Dim key As String = mParam.Name
                            If m_paramTable(key) IsNot Nothing Then
                                m_paramTable(key) = mParam
                            End If
                        Next
                    End If
                End If
                'Add Model Parameters label if we have parameters for the user
                If hasUserParams = True Then
                    CreateLabel(xValueLabel, yValueInput, "Model Parameters")
                    yValueLabel = yValueLabel + 30
                    yValueInput = yValueInput + 30
                End If
                Dim sb As StringBuilder = New StringBuilder
                For Each key As String In m_paramTable.Keys
                    If key.Substring(0, BA_PARAM_PREFIX.Length).ToLower <> BA_PARAM_PREFIX Then
                        If key.Substring(0, BA_DATABIN_PREFIX.Length).ToLower = BA_DATABIN_PREFIX Then
                            'This is a data bin parameter; We add a comboBox to the form
                            Dim dataParam As ModelParameter = m_paramTable(key)
                            CreateLabel(xValueLabel, yValueLabel, key)
                            'Move the position down for the next potential parameter
                            yValueLabel = yValueLabel + 30
                            CreateCboBoxForDataBin(xValueInput, yValueInput, key, dataParam.Value)
                            'Move the position down for the next potential parameter
                            yValueInput = yValueInput + 30
                        ElseIf key.Substring(0, BA_FIELD_PREFIX.Length).ToLower = BA_FIELD_PREFIX Then
                            'This is a field name parameter, we append the field name to the field name display
                            Dim dataParam As ModelParameter = m_paramTable(key)
                            sb.Append(dataParam.Value)
                            sb.Append(", ")
                        Else
                            'Otherwise we assume it is a string parameter and create a textBox
                            Dim txtParam As ModelParameter = m_paramTable(key)
                            CreateLabel(xValueLabel, yValueLabel, key)
                            'Move the position down for the next potential parameter
                            yValueLabel = yValueLabel + 30
                            CreateTextBoxForParam(xValueInput, yValueInput, key, txtParam.Value)
                            'Move the position down for the next potential parameter
                            yValueInput = yValueInput + 30
                        End If
                    End If
                Next
                'Trim trailing , and space from StringBuilder
                Dim strFieldNames As String = sb.ToString
                If strFieldNames.Length > 0 Then
                    strFieldNames = strFieldNames.Substring(0, strFieldNames.Length - 2)
                    TxtFieldNames.Text = strFieldNames
                End If
            End If
        End If
    End Sub

    Private Sub SaveParametersFromForm()
        If m_parameterInputControls IsNot Nothing Then
            For Each ctrl As Control In m_parameterInputControls
                If TypeOf ctrl Is ComboBox Then
                    'db_ params
                    Dim cboDataLayer As ComboBox = CType(ctrl, ComboBox)    'Explicit cast
                    Dim paramName As String = cboDataLayer.Name.Substring(CBO_PREFIX.Length)
                    Dim mParam As ModelParameter = m_paramTable(paramName)
                    Dim layerName As String = CStr(cboDataLayer.SelectedItem)
                    mParam.Value = layerName
                ElseIf TypeOf ctrl Is TextBox Then
                    Dim paramText As TextBox = CType(ctrl, TextBox) 'Explicit cast
                    Dim paramName As String = paramText.Name.Substring(TXT_PREFIX.Length)
                    Dim mParam As ModelParameter = m_paramTable(paramName)
                    mParam.Value = Trim(paramText.Text)
                End If
            Next
        End If
    End Sub

    Private Sub LoadCboDataLayer(ByVal pComboBox As ComboBox, ByVal layerName As String)
        pComboBox.Items.Clear()
        pComboBox.Items.Add(SELECT_ONE)
        Dim settingsPath As String = BA_GetBagisPSettingsPath()
        If Not String.IsNullOrEmpty(m_aoiPath) Then
            settingsPath = BA_GetLocalSettingsPath(m_aoiPath)
        End If
        Dim layerTable As Hashtable = BA_LoadSettingsFile(settingsPath)
        Dim sortedKeys(layerTable.Count - 1) As String
        layerTable.Keys.CopyTo(sortedKeys, 0)
        Array.Sort(sortedKeys)
        For Each key As String In sortedKeys
            pComboBox.Items.Add(key)
            If key = layerName Then
                pComboBox.SelectedItem = key
            End If
        Next
        'Set default data layer if we don't have one
        If pComboBox.SelectedItem = Nothing Then
            pComboBox.SelectedItem = pComboBox.Items(0)
        End If
    End Sub

    Private Sub LoadMethodList(ByVal selMethod As Method)
        LstMethods.Items.Clear()
        If m_methodTable IsNot Nothing Then
            For Each key As String In m_methodTable.Keys
                LstMethods.Items.Add(key)
                If selMethod IsNot Nothing Then
                    If key = selMethod.Name Then
                        LstMethods.SelectedItem = key
                    End If
                End If
            Next
        End If

    End Sub

    Private Sub LstMethods_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstMethods.SelectedIndexChanged
        RemoveAllLabelsAndControls()
        If LstMethods.SelectedItems.Count > 0 Then
            'If an item is selected
            m_method = m_methodTable(CStr(LstMethods.SelectedItem))
            If m_method IsNot Nothing Then
                Dim model As IGPTool = BA_OpenModel(m_method.ToolBoxPath, m_method.ToolboxName, m_method.ModelName)
                TxtMethod.Text = m_method.Name
                TxtDescription.Text = m_method.Description
                BtnDelete.Enabled = True
                If model Is Nothing Then
                    TxtToolboxName.Text = Nothing & vbCrLf
                    TxtModelName.Text = Nothing
                    TxtToolboxPath.Text = Nothing
                    LblModelName.Text = Nothing
                    Dim sb As StringBuilder = New StringBuilder
                    sb.Append("The model associated with this method could not be found at" & vbCrLf)
                    sb.Append(m_method.ToolBoxPath & "\" & m_method.ToolboxName & "." & vbCrLf)
                    sb.Append("Please select a new model for this method." & vbCrLf & vbCrLf)
                    sb.Append("You may also update the toolbox path for all methods using" & vbCrLf)
                    sb.Append("the 'Reset toolbox path(s)' button on the 'Edit Method' screen.")
                    MessageBox.Show(sb.ToString, "Missing model", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    TxtToolboxName.Text = m_method.ToolboxName
                    TxtModelName.Text = m_method.ModelLabel
                    TxtToolboxPath.Text = m_method.ToolBoxPath
                    LblModelName.Text = m_method.ModelName
                    AddParametersToForm(model)
                End If
            Else
                TxtMethod.Text = Nothing
                TxtToolboxName.Text = Nothing
                TxtModelName.Text = Nothing
                TxtToolboxPath.Text = Nothing
                LblModelName.Text = Nothing
                TxtDescription.Text = Nothing
            End If
        Else
            'Otherwise clear everything out
            TxtMethod.Text = Nothing
            TxtToolboxName.Text = Nothing
            TxtModelName.Text = Nothing
            TxtToolboxPath.Text = Nothing
            LblModelName.Text = Nothing
            TxtDescription.Text = Nothing
            m_method = Nothing
            BtnDelete.Enabled = False
        End If
        PositionCloseAndSaveButtons()
    End Sub

    Private Sub CreateLabel(ByVal posX As Integer, ByVal posY As Integer, ByVal paramName As String)
        Dim newLabel As Label = New System.Windows.Forms.Label()
        newLabel.AutoSize = True
        newLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        newLabel.Location = New System.Drawing.Point(posX, posY)
        newLabel.Name = "Lbl" & paramName
        newLabel.Size = New System.Drawing.Size(101, 16)
        newLabel.TabIndex = 40
        newLabel.Text = paramName & ":"
        newLabel.Visible = True
        Me.Controls.Add(newLabel)
        If m_parameterLabels Is Nothing Then
            m_parameterLabels = New List(Of String)
        End If
        m_parameterLabels.Add(newLabel.Name)
    End Sub

    Private Sub CreateCboBoxForDataBin(ByVal posX As Integer, ByVal posY As Integer, ByVal paramName As String, _
                                       ByVal paramValue As String)
        Dim newCombo As ComboBox = New System.Windows.Forms.ComboBox
        newCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        newCombo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        newCombo.FormattingEnabled = True
        newCombo.Location = New System.Drawing.Point(posX, posY)
        newCombo.Name = CBO_PREFIX & paramName
        newCombo.Size = New System.Drawing.Size(288, 24)
        newCombo.TabIndex = 49
        newCombo.Visible = True
        Me.Controls.Add(newCombo)
        LoadCboDataLayer(newCombo, paramValue)
        If m_parameterInputControls Is Nothing Then
            m_parameterInputControls = New List(Of Control)
        End If
        m_parameterInputControls.Add(newCombo)
    End Sub

    Private Sub CreateTextBoxForParam(ByVal posX As Integer, ByVal posY As Integer, ByVal paramName As String, _
                                      ByVal paramValue As String)
        Dim newText As TextBox = New System.Windows.Forms.TextBox
        newText.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        newText.Location = New System.Drawing.Point(posX, posY)
        newText.Name = TXT_PREFIX & paramName
        newText.Size = New System.Drawing.Size(288, 22)
        newText.TabIndex = 41
        newText.Visible = True
        Me.Controls.Add(newText)
        newText.Text = paramValue
        If m_parameterInputControls Is Nothing Then
            m_parameterInputControls = New List(Of Control)
        End If
        m_parameterInputControls.Add(newText)
    End Sub

    Private Sub RemoveAllLabelsAndControls()
        If m_parameterLabels IsNot Nothing Then
            For Each strLabel As String In m_parameterLabels
                'Find the label by name
                Dim oldLabel As Control = Me.Controls.Find(strLabel, False)(0)
                Me.Controls.Remove(oldLabel)
            Next
            m_parameterLabels.RemoveRange(0, m_parameterLabels.Count)
        End If
        If m_parameterInputControls IsNot Nothing Then
            For Each pControl As Control In m_parameterInputControls
                Me.Controls.Remove(pControl)
            Next
            m_parameterInputControls.RemoveRange(0, m_parameterInputControls.Count)
        End If
    End Sub

    Public Sub PositionCloseAndSaveButtons()
        Dim yPos As Integer = 250
        If m_parameterLabels IsNot Nothing AndAlso m_parameterLabels.Count > 0 Then
            Dim lastLabel As String = m_parameterLabels(m_parameterLabels.Count - 1)
            Dim ctrlLabel As Control = Me.Controls.Find(lastLabel, False)(0)
            yPos = ctrlLabel.Location.Y
            yPos = yPos + 35
            BtnClose.Location = New System.Drawing.Point(463, yPos)
            BtnDelete.Location = New System.Drawing.Point(536, yPos)
            BtnEditModel.Location = New System.Drawing.Point(608, yPos)
            BtnApply.Location = New System.Drawing.Point(696, yPos)
            BtnResetToolboxPath.Location = New System.Drawing.Point(8, yPos)
        Else
            BtnClose.Location = New System.Drawing.Point(463, yPos)
            BtnDelete.Location = New System.Drawing.Point(536, yPos)
            BtnEditModel.Location = New System.Drawing.Point(608, yPos)
            BtnApply.Location = New System.Drawing.Point(696, yPos)
            BtnResetToolboxPath.Location = New System.Drawing.Point(8, yPos)
        End If
        Me.Height = yPos + 70
    End Sub

    Private Function ValidateParameters() As BA_ReturnCode
        Dim missing As Boolean = False
        Dim paramName As String = Nothing
        If m_parameterInputControls IsNot Nothing Then
            For Each ctrl As Control In m_parameterInputControls
                If TypeOf ctrl Is ComboBox Then
                    Dim cboDataLayer As ComboBox = CType(ctrl, ComboBox)    'Explicit cast
                    paramName = cboDataLayer.Name.Substring(CBO_PREFIX.Length)
                    If cboDataLayer.SelectedIndex < 1 Then
                        missing = True
                    End If
                ElseIf TypeOf ctrl Is TextBox Then
                    Dim txtParam As TextBox = CType(ctrl, TextBox)  'Explicit cast
                    paramName = txtParam.Name.Substring(TXT_PREFIX.Length)
                    If String.IsNullOrEmpty(txtParam.Text) Then
                        missing = True
                    End If
                End If
                If missing = True Then
                    Dim sb As StringBuilder = New StringBuilder
                    sb.Append("Please supply a value for the " & paramName & vbCrLf)
                    sb.Append("model parameter. Values are required for all" & vbCrLf)
                    sb.Append("model parameters." & vbCrLf)
                    MessageBox.Show(sb.ToString, "Missing parameter", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    ctrl.Focus()
                    Return BA_ReturnCode.OtherError
                End If
            Next
        End If
        Return BA_ReturnCode.Success
    End Function

    Public Sub ResetForm()
        RemoveAllLabelsAndControls()
        TxtToolboxPath.Text = Nothing
        TxtModelName.Text = Nothing
        TxtMethod.Text = Nothing
        TxtToolboxName.Text = Nothing
        LblModelName.Text = Nothing
        TxtDescription.Text = Nothing
        TxtFieldNames.Text = Nothing
        m_method = Nothing
        LoadMethodList(m_method)
    End Sub

    Private Sub TxtToolboxPath_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtToolboxPath.TextChanged
        'Enable the apply button when we have a toolbox loaded
        If String.IsNullOrEmpty(TxtToolboxPath.Text) Then
            BtnApply.Enabled = False
            BtnEditModel.Enabled = False
        Else
            BtnApply.Enabled = True
            BtnEditModel.Enabled = True
        End If
    End Sub

    Private Sub BtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDelete.Click
        If LstMethods.SelectedIndex > -1 Then
            Dim methodName As String = CStr(LstMethods.SelectedItem)
            Dim bExt As BagisPExtension = BagisPExtension.GetExtension
            Dim settingsPath As String = bExt.SettingsPath
            Dim methodsPath As String = BA_GetPublicMethodsPath(settingsPath)
            If Not String.IsNullOrEmpty(m_aoiPath) Then
                methodsPath = BA_GetLocalMethodsDir(m_aoiPath)
            End If
            Dim methodFullPath As String = BA_BagisPXmlPath(methodsPath, methodName)
            'Delete the xml file if it exists
            Dim success As BA_ReturnCode = BA_ReturnCode.Success
            If BA_File_ExistsWindowsIO(methodFullPath) Then
                success = BA_Remove_File(methodFullPath)
            End If
            If success = BA_ReturnCode.Success Then
                m_methodTable.Remove(methodName)
                'Set updated methodTable in parent form
                If m_parentForm IsNot Nothing Then
                    m_parentForm.MethodTable = m_methodTable
                End If
                ResetForm()
            End If
        End If
    End Sub

    Private Sub BtnEditModel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnEditModel.Click
        Dim arcToolBoxExtension As IArcToolboxExtension = Nothing
        Dim arcToolBox As IArcToolbox = Nothing
        Dim pTool As IGPTool = Nothing

        Try
            arcToolBoxExtension = My.ArcMap.Application.FindExtensionByName("ESRI ArcToolbox")
            If arcToolBoxExtension IsNot Nothing Then
                arcToolBox = arcToolBoxExtension.ArcToolbox
                If arcToolBox IsNot Nothing Then
                    'pTool = arcToolBox.GetToolbyNameString(TxtModelName.Text)
                    pTool = BA_OpenModel(TxtToolboxPath.Text, TxtToolboxName.Text, LblModelName.Text)
                    If pTool IsNot Nothing Then
                        arcToolBox.EditToolSource(pTool)
                    End If
                End If
            End If
        Catch ex As Exception
            Debug.Print("BtnEditModel_Click() Exception: " & ex.Message)
            MessageBox.Show("Unable to open the selected model")
        Finally
            pTool = Nothing
            arcToolBoxExtension = Nothing
            arcToolBox = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub

    Private Sub BtnResetToolboxPath_Click(sender As System.Object, e As System.EventArgs) Handles BtnResetToolboxPath.Click
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("The toolbox path will be updated for ALL" & vbCrLf)
        sb.Append("methods listed in the method editor." & vbCrLf)
        sb.Append("Do you wish to continue?" & vbCrLf & vbCrLf)
        Dim result As DialogResult = MessageBox.Show(sb.ToString, "Reset toolbox path(s)", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = Windows.Forms.DialogResult.Yes Then
            Dim bExt As BagisPExtension = BagisPExtension.GetExtension
            Dim settingsPath As String = bExt.SettingsPath
            Dim methodsPath As String = BA_GetPublicMethodsPath(settingsPath)
            If Not String.IsNullOrEmpty(m_aoiPath) Then
                methodsPath = BA_GetLocalMethodsDir(m_aoiPath)
            End If

            Dim frmReset As FrmResetToolboxPath = New FrmResetToolboxPath(m_methodTable, methodsPath)
            frmReset.ShowDialog()
            RefreshMethodData()
            'Set updated methodTable in parent form, if applicable
            If m_parentForm IsNot Nothing Then
                m_parentForm.MethodTable = m_methodTable
            End If
            ResetForm()
        Else
            Exit Sub
        End If
    End Sub

    'Loads the method data from disk
    Private Sub RefreshMethodData()
        Dim methodsFolder As String = Nothing
        'Assume if there isn't an aoi path, we are in public mode
        If m_aoiPath IsNot Nothing Then
            methodsFolder = BA_GetLocalMethodsDir(m_aoiPath)
        Else
            Dim bExt As BagisPExtension = BagisPExtension.GetExtension
            Dim settingsPath As String = bExt.SettingsPath
            methodsFolder = BA_GetPublicMethodsPath(settingsPath)
        End If
        If Not String.IsNullOrEmpty(methodsFolder) Then
            Dim methodList As List(Of Method) = BA_LoadMethodsFromXml(methodsFolder)
            If methodList IsNot Nothing Then
                m_methodTable = New Hashtable
                For Each nextMethod In methodList
                    m_methodTable.Add(nextMethod.Name, nextMethod)
                Next
            End If
        End If
    End Sub
End Class