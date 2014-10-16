Imports System.Drawing
Imports System.Windows.Forms

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TabTemplateRuleCtrl
    Inherits System.Windows.Forms.TabPage
    'Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.TxtRuleType = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtInputLayerName = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TxtStatus = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TxtOutputLayer = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TxtInputLayerPath = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.BtnViewRule = New System.Windows.Forms.Button()
        Me.BtnViewInput = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cboActions = New System.Windows.Forms.ComboBox()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.pName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pValue = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ActionId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LblHruPath = New System.Windows.Forms.Label()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TxtRuleType
        '
        Me.TxtRuleType.Location = New System.Drawing.Point(61, 9)
        Me.TxtRuleType.Name = "TxtRuleType"
        Me.TxtRuleType.ReadOnly = True
        Me.TxtRuleType.Size = New System.Drawing.Size(492, 20)
        Me.TxtRuleType.TabIndex = 44
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(2, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(65, 13)
        Me.Label1.TabIndex = 43
        Me.Label1.Text = "Rule type:"
        '
        'TxtInputLayerName
        '
        Me.TxtInputLayerName.Location = New System.Drawing.Point(104, 37)
        Me.TxtInputLayerName.Name = "TxtInputLayerName"
        Me.TxtInputLayerName.ReadOnly = True
        Me.TxtInputLayerName.Size = New System.Drawing.Size(449, 20)
        Me.TxtInputLayerName.TabIndex = 58
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(2, 40)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(105, 13)
        Me.Label5.TabIndex = 57
        Me.Label5.Text = "Input layer name:"
        '
        'TxtStatus
        '
        Me.TxtStatus.Location = New System.Drawing.Point(45, 116)
        Me.TxtStatus.Name = "TxtStatus"
        Me.TxtStatus.ReadOnly = True
        Me.TxtStatus.Size = New System.Drawing.Size(100, 20)
        Me.TxtStatus.TabIndex = 56
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(2, 119)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(47, 13)
        Me.Label4.TabIndex = 55
        Me.Label4.Text = "Status:"
        '
        'TxtOutputLayer
        '
        Me.TxtOutputLayer.Location = New System.Drawing.Point(82, 91)
        Me.TxtOutputLayer.Name = "TxtOutputLayer"
        Me.TxtOutputLayer.ReadOnly = True
        Me.TxtOutputLayer.Size = New System.Drawing.Size(555, 20)
        Me.TxtOutputLayer.TabIndex = 54
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(2, 94)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(80, 13)
        Me.Label3.TabIndex = 53
        Me.Label3.Text = "Output layer:"
        '
        'TxtInputLayerPath
        '
        Me.TxtInputLayerPath.Location = New System.Drawing.Point(104, 65)
        Me.TxtInputLayerPath.Name = "TxtInputLayerPath"
        Me.TxtInputLayerPath.ReadOnly = True
        Me.TxtInputLayerPath.Size = New System.Drawing.Size(533, 20)
        Me.TxtInputLayerPath.TabIndex = 52
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(2, 68)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 13)
        Me.Label2.TabIndex = 51
        Me.Label2.Text = "Input layer path:"
        '
        'BtnViewRule
        '
        Me.BtnViewRule.Location = New System.Drawing.Point(560, 6)
        Me.BtnViewRule.Name = "BtnViewRule"
        Me.BtnViewRule.Size = New System.Drawing.Size(75, 23)
        Me.BtnViewRule.TabIndex = 65
        Me.BtnViewRule.Text = "View Layer"
        Me.BtnViewRule.UseVisualStyleBackColor = True
        '
        'BtnViewInput
        '
        Me.BtnViewInput.Location = New System.Drawing.Point(560, 35)
        Me.BtnViewInput.Name = "BtnViewInput"
        Me.BtnViewInput.Size = New System.Drawing.Size(75, 23)
        Me.BtnViewInput.TabIndex = 66
        Me.BtnViewInput.Text = "View Layer"
        Me.BtnViewInput.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(2, 143)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(53, 13)
        Me.Label6.TabIndex = 68
        Me.Label6.Text = "Actions:"
        '
        'cboActions
        '
        Me.cboActions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboActions.FormattingEnabled = True
        Me.cboActions.Location = New System.Drawing.Point(57, 139)
        Me.cboActions.Name = "cboActions"
        Me.cboActions.Size = New System.Drawing.Size(150, 21)
        Me.cboActions.TabIndex = 69
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.White
        Me.DataGridView1.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlDarkDark
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.Info
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.pName, Me.pValue, Me.ActionId})
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridView1.DefaultCellStyle = DataGridViewCellStyle4
        Me.DataGridView1.Location = New System.Drawing.Point(3, 178)
        Me.DataGridView1.Name = "DataGridView1"
        DataGridViewCellStyle5.BackColor = System.Drawing.Color.White
        Me.DataGridView1.RowsDefaultCellStyle = DataGridViewCellStyle5
        Me.DataGridView1.Size = New System.Drawing.Size(497, 135)
        Me.DataGridView1.TabIndex = 70
        '
        'pName
        '
        Me.pName.HeaderText = "Parameter name"
        Me.pName.Name = "pName"
        Me.pName.ReadOnly = True
        Me.pName.Width = 205
        '
        'pValue
        '
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.pValue.DefaultCellStyle = DataGridViewCellStyle3
        Me.pValue.HeaderText = "Parameter value"
        Me.pValue.Name = "pValue"
        Me.pValue.ReadOnly = True
        Me.pValue.Width = 225
        '
        'ActionId
        '
        Me.ActionId.HeaderText = "ActionId"
        Me.ActionId.Name = "ActionId"
        Me.ActionId.ReadOnly = True
        Me.ActionId.Visible = False
        '
        'LblHruPath
        '
        Me.LblHruPath.AutoSize = True
        Me.LblHruPath.Location = New System.Drawing.Point(15, 337)
        Me.LblHruPath.Name = "LblHruPath"
        Me.LblHruPath.Size = New System.Drawing.Size(60, 13)
        Me.LblHruPath.TabIndex = 71
        Me.LblHruPath.Text = "LblHruPath"
        Me.LblHruPath.Visible = False
        '
        'TabTemplateRuleCtrl
        '
        Me.Controls.Add(Me.LblHruPath)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.cboActions)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.BtnViewInput)
        Me.Controls.Add(Me.BtnViewRule)
        Me.Controls.Add(Me.TxtInputLayerName)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TxtStatus)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.TxtOutputLayer)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TxtInputLayerPath)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TxtRuleType)
        Me.Controls.Add(Me.Label1)
        Me.Name = "TabTemplateRuleCtrl"
        Me.Size = New System.Drawing.Size(650, 375)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TxtRuleType As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label

    Public Sub New(ByVal Hru As BAGIS_ClassLibrary.Hru, ByVal rule As BAGIS_ClassLibrary.IRule, ByVal isParent As Boolean)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Text = "Rule " & rule.RuleId
        TxtRuleType.Text = rule.RuleTypeText
        TxtInputLayerName.Text = rule.InputLayerName
        TxtInputLayerPath.Text = rule.InputFolderPath
        LblHruPath.Text = Hru.FilePath
        TxtOutputLayer.Text = rule.OutputDatasetName
        TxtStatus.Text = rule.FactorStatus.ToString
        If isParent = True Then
            BtnViewInput.Enabled = False
            BtnViewRule.Enabled = False
        End If

        ' template specific fields
        Dim templateRule As BAGIS_ClassLibrary.TemplateRule = CType(rule, BAGIS_ClassLibrary.TemplateRule)
        Dim templateActions As List(Of BAGIS_ClassLibrary.TemplateAction) = templateRule.TemplateActions
        'Dim yPos As Integer = 120
        'Dim xPos As Integer = 2
        'Dim xPosParam As Integer = 5
        'Dim xPosValue As Integer = 177
        'Dim headerSize As New Size(150, 15)
        'Dim size As New Size(160, 13)
        'Dim valueSize As New Size(100, 13)
        Dim idx As Integer
        Dim paramIdx As Integer
        DataGridView1.Rows.Clear()
        For Each pAction In templateActions
            'yPos = yPos + 15
            idx += 1
            cboActions.Items.Add("Action " & idx & " - " & BAGIS_ClassLibrary.BA_EnumDescription(pAction.actionType))
            'Dim point As New System.Drawing.Point(xPos, yPos)
            'Dim name As String = "LblAction" & idx
            'Dim pText As String = pAction.actionType.ToString & " action parameters"
            'Dim pFont As New Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            'Dim txtFont As New Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            'Dim newLabel As System.Windows.Forms.Label = AddNewLabel(name, pText, headerSize, point, pFont)
            'Dim newTxtBox As System.Windows.Forms.TextBox
            Dim params As Hashtable = pAction.parameters
            Dim keyCollection As ICollection = params.Keys
            For Each objKey In keyCollection
                'yPos = yPos + 20
                paramIdx += 1
                Dim key As BAGIS_ClassLibrary.ActionParameter = CType(objKey, BAGIS_ClassLibrary.ActionParameter)
                Dim value As Object = params(objKey)
                If TypeOf value Is String Then            '---create a row---
                    Dim item As New DataGridViewRow
                    item.CreateCells(DataGridView1)
                    With item
                        .Cells(0).Value = key.ToString
                        .Cells(1).Value = CStr(value)
                        .Cells(2).Value = pAction.id
                    End With
                    '---add the row---
                    DataGridView1.Rows.Add(item)

                    'Dim paramPoint As New System.Drawing.Point(xPosParam, yPos)
                    'Dim paramName As String = "LblParam" & paramIdx
                    'Dim paramText As String = "Parameter: " & key.ToString
                    'newLabel = AddNewLabel(paramName, paramText, size, paramPoint, pFont)

                    'Dim valuePoint As New System.Drawing.Point(xPosValue, yPos)
                    'Dim valueName As String = "TxtValue" & paramIdx
                    'Dim valueText As String = CStr(value)
                    'newTxtBox = AddNewTextBox(valueName, valueText, valueSize, valuePoint, txtFont)
                End If
            Next
        Next
        cboActions.SelectedIndex = 0
    End Sub
    Friend WithEvents TxtInputLayerName As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TxtStatus As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TxtOutputLayer As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TxtInputLayerPath As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents BtnViewRule As System.Windows.Forms.Button
    Friend WithEvents BtnViewInput As System.Windows.Forms.Button

    Private Function AddNewLabel(ByVal name As String, ByVal labelText As String, _
                                 ByVal size As Size, ByVal location As Point, ByVal font As Font) As System.Windows.Forms.Label
        ' Create a new instance of the Label class.  
        Dim aLabel As New System.Windows.Forms.Label
        Me.Controls.Add(aLabel)
        aLabel.Name = name
        aLabel.Size = size
        aLabel.Location = location
        aLabel.Text = labelText
        aLabel.Font = font
        Return aLabel
    End Function

    Private Function AddNewTextBox(ByVal name As String, ByVal pText As String, _
                                   ByVal size As Size, ByVal location As Point, ByVal font As Font) As System.Windows.Forms.TextBox
        ' Create a new instance of the Label class.  
        Dim aTextBox As New System.Windows.Forms.TextBox
        Me.Controls.Add(aTextBox)
        aTextBox.Name = name
        aTextBox.Size = size
        aTextBox.Location = location
        aTextBox.Text = pText
        aTextBox.Font = font
        aTextBox.ReadOnly = True
        Return aTextBox
    End Function

    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cboActions As System.Windows.Forms.ComboBox
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents pName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents pValue As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ActionId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LblHruPath As System.Windows.Forms.Label


End Class
