<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TabPrismRuleCtrl
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
        Me.Label9 = New System.Windows.Forms.Label()
        Me.TxtDataRange = New System.Windows.Forms.TextBox()
        Me.TxtFirstMonth = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.TxtLastMonth = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.BtnViewInput = New System.Windows.Forms.Button()
        Me.LblHruPath = New System.Windows.Forms.Label()
        Me.TxtUnits = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.GridReclassValues = New System.Windows.Forms.DataGridView()
        Me.FromValues = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ToValues = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NewValues = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.GridReclassValues, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.TxtInputLayerPath.Location = New System.Drawing.Point(99, 65)
        Me.TxtInputLayerPath.Name = "TxtInputLayerPath"
        Me.TxtInputLayerPath.ReadOnly = True
        Me.TxtInputLayerPath.Size = New System.Drawing.Size(538, 20)
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
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(2, 144)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(74, 13)
        Me.Label9.TabIndex = 66
        Me.Label9.Text = "Data range:"
        '
        'TxtDataRange
        '
        Me.TxtDataRange.Location = New System.Drawing.Point(73, 141)
        Me.TxtDataRange.Name = "TxtDataRange"
        Me.TxtDataRange.ReadOnly = True
        Me.TxtDataRange.Size = New System.Drawing.Size(564, 20)
        Me.TxtDataRange.TabIndex = 67
        '
        'TxtFirstMonth
        '
        Me.TxtFirstMonth.Location = New System.Drawing.Point(38, 165)
        Me.TxtFirstMonth.Name = "TxtFirstMonth"
        Me.TxtFirstMonth.ReadOnly = True
        Me.TxtFirstMonth.Size = New System.Drawing.Size(77, 20)
        Me.TxtFirstMonth.TabIndex = 69
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(3, 168)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(38, 13)
        Me.Label10.TabIndex = 68
        Me.Label10.Text = "From:"
        '
        'TxtLastMonth
        '
        Me.TxtLastMonth.Location = New System.Drawing.Point(139, 165)
        Me.TxtLastMonth.Name = "TxtLastMonth"
        Me.TxtLastMonth.ReadOnly = True
        Me.TxtLastMonth.Size = New System.Drawing.Size(77, 20)
        Me.TxtLastMonth.TabIndex = 71
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(116, 168)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(26, 13)
        Me.Label11.TabIndex = 70
        Me.Label11.Text = "To:"
        '
        'BtnViewInput
        '
        Me.BtnViewInput.Location = New System.Drawing.Point(560, 36)
        Me.BtnViewInput.Name = "BtnViewInput"
        Me.BtnViewInput.Size = New System.Drawing.Size(75, 23)
        Me.BtnViewInput.TabIndex = 72
        Me.BtnViewInput.Text = "View Layer"
        Me.BtnViewInput.UseVisualStyleBackColor = True
        '
        'LblHruPath
        '
        Me.LblHruPath.AutoSize = True
        Me.LblHruPath.Location = New System.Drawing.Point(7, 337)
        Me.LblHruPath.Name = "LblHruPath"
        Me.LblHruPath.Size = New System.Drawing.Size(60, 13)
        Me.LblHruPath.TabIndex = 73
        Me.LblHruPath.Text = "LblHruPath"
        Me.LblHruPath.Visible = False
        '
        'TxtUnits
        '
        Me.TxtUnits.Location = New System.Drawing.Point(116, 190)
        Me.TxtUnits.Name = "TxtUnits"
        Me.TxtUnits.ReadOnly = True
        Me.TxtUnits.Size = New System.Drawing.Size(100, 20)
        Me.TxtUnits.TabIndex = 75
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(2, 193)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(113, 13)
        Me.Label6.TabIndex = 74
        Me.Label6.Text = "Precipitation units:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(2, 216)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(97, 13)
        Me.Label7.TabIndex = 77
        Me.Label7.Text = "Reclass values:"
        '
        'GridReclassValues
        '
        Me.GridReclassValues.AllowUserToAddRows = False
        Me.GridReclassValues.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.GridReclassValues.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.GridReclassValues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridReclassValues.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.FromValues, Me.ToValues, Me.NewValues})
        Me.GridReclassValues.Location = New System.Drawing.Point(116, 215)
        Me.GridReclassValues.Name = "GridReclassValues"
        Me.GridReclassValues.ReadOnly = True
        Me.GridReclassValues.Size = New System.Drawing.Size(339, 185)
        Me.GridReclassValues.TabIndex = 76
        '
        'FromValues
        '
        Me.FromValues.HeaderText = "From"
        Me.FromValues.Name = "FromValues"
        Me.FromValues.ReadOnly = True
        Me.FromValues.Width = 95
        '
        'ToValues
        '
        Me.ToValues.HeaderText = "To"
        Me.ToValues.Name = "ToValues"
        Me.ToValues.ReadOnly = True
        '
        'NewValues
        '
        Me.NewValues.HeaderText = "New Value"
        Me.NewValues.Name = "NewValues"
        Me.NewValues.ReadOnly = True
        Me.NewValues.Width = 95
        '
        'TabPrismRuleCtrl
        '
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.GridReclassValues)
        Me.Controls.Add(Me.TxtUnits)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.LblHruPath)
        Me.Controls.Add(Me.BtnViewInput)
        Me.Controls.Add(Me.TxtLastMonth)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.TxtFirstMonth)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.TxtDataRange)
        Me.Controls.Add(Me.Label9)
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
        Me.Name = "TabPrismRuleCtrl"
        Me.Size = New System.Drawing.Size(650, 412)
        CType(Me.GridReclassValues, System.ComponentModel.ISupportInitialize).EndInit()
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
        TxtOutputLayer.Text = rule.OutputDatasetName
        LblHruPath.Text = Hru.FilePath
        TxtStatus.Text = rule.FactorStatus.ToString
        If isParent = True Then
            BtnViewInput.Enabled = False
            BtnViewRule.Enabled = False
        End If

        ' slice specific fields
        Dim prismRule As BAGIS_ClassLibrary.PrismPrecipRule = CType(rule, BAGIS_ClassLibrary.PrismPrecipRule)

        ' prism specific fields
        TxtDataRange.Text = prismRule.DataRangeText
        If prismRule.DataRange = BAGIS_ClassLibrary.PrismDataRange.Custom Then
            TxtFirstMonth.Text = prismRule.firstMonth
            TxtLastMonth.Text = prismRule.lastMonth
        Else
            TxtFirstMonth.Text = Nothing
            TxtLastMonth.Text = Nothing
        End If
        TxtUnits.Text = prismRule.DisplayUnitsText
        Dim reclassItems As BAGIS_ClassLibrary.ReclassItem() = prismRule.ReclassItems
        For i As Short = 0 To reclassItems.GetUpperBound(0) Step 1
            Dim reclassItem As BAGIS_ClassLibrary.ReclassItem = reclassItems(i)
            Dim displayItem As String() = {reclassItem.FromValue, reclassItem.ToValue, reclassItem.OutputValue}
            Me.GridReclassValues.Rows.Add(displayItem)
        Next

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
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TxtDataRange As System.Windows.Forms.TextBox
    Friend WithEvents TxtFirstMonth As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents TxtLastMonth As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents BtnViewInput As System.Windows.Forms.Button
    Friend WithEvents LblHruPath As System.Windows.Forms.Label
    Friend WithEvents TxtUnits As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents GridReclassValues As System.Windows.Forms.DataGridView
    Friend WithEvents FromValues As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ToValues As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NewValues As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
