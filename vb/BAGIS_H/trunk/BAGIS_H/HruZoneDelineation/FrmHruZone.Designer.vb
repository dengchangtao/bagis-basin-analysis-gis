<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmHruZone
  Inherits System.Windows.Forms.UserControl

  'Form overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    If disposing AndAlso components IsNot Nothing Then
      components.Dispose()
    End If
    MyBase.Dispose(disposing)
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.BtnSelectAoi = New System.Windows.Forms.Button()
        Me.LblHruLayers = New System.Windows.Forms.Label()
        Me.LstHruLayers = New System.Windows.Forms.ListBox()
        Me.BtnViewLayerLog = New System.Windows.Forms.Button()
        Me.BtnViewLayer = New System.Windows.Forms.Button()
        Me.LblTemplateHru = New System.Windows.Forms.Label()
        Me.LstSelectedZones = New System.Windows.Forms.ListBox()
        Me.GrpRules = New System.Windows.Forms.GroupBox()
        Me.CkRetainAttributes = New System.Windows.Forms.CheckBox()
        Me.TxtLayerLocation = New System.Windows.Forms.TextBox()
        Me.CkNonContiguous = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GridViewRules = New System.Windows.Forms.DataGridView()
        Me.RuleType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LayerUsed = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.OutputFolder = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Status = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.View = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.RuleId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BtnRunRule = New System.Windows.Forms.Button()
        Me.LblLayerNameValue = New System.Windows.Forms.Label()
        Me.LblLayerTypeValue = New System.Windows.Forms.Label()
        Me.CboRuleType = New System.Windows.Forms.ComboBox()
        Me.LblOutputHruName = New System.Windows.Forms.Label()
        Me.TxtNewHruName = New System.Windows.Forms.TextBox()
        Me.BtnDeleteRule = New System.Windows.Forms.Button()
        Me.BtnDefineRule = New System.Windows.Forms.Button()
        Me.BtnUpdateRule = New System.Windows.Forms.Button()
        Me.LblRuleType = New System.Windows.Forms.Label()
        Me.RadApplyRulesAll = New System.Windows.Forms.RadioButton()
        Me.PnlApplyRules = New System.Windows.Forms.Panel()
        Me.BtnToggle = New System.Windows.Forms.Button()
        Me.CboParentHru = New System.Windows.Forms.ComboBox()
        Me.RadApplyRulesSelect = New System.Windows.Forms.RadioButton()
        Me.ToolTipZones = New System.Windows.Forms.ToolTip(Me.components)
        Me.BtnGenerateHru = New System.Windows.Forms.Button()
        Me.BtnDeleteLayer = New System.Windows.Forms.Button()
        Me.TxtAoiPath = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.BtnRenameLayer = New System.Windows.Forms.Button()
        Me.GrpRules.SuspendLayout()
        CType(Me.GridViewRules, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlApplyRules.SuspendLayout()
        Me.SuspendLayout()
        '
        'BtnSelectAoi
        '
        Me.BtnSelectAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSelectAoi.Location = New System.Drawing.Point(12, 8)
        Me.BtnSelectAoi.Name = "BtnSelectAoi"
        Me.BtnSelectAoi.Size = New System.Drawing.Size(105, 28)
        Me.BtnSelectAoi.TabIndex = 0
        Me.BtnSelectAoi.Text = "Select AOI"
        Me.BtnSelectAoi.UseVisualStyleBackColor = True
        '
        'LblHruLayers
        '
        Me.LblHruLayers.AutoSize = True
        Me.LblHruLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblHruLayers.Location = New System.Drawing.Point(49, 51)
        Me.LblHruLayers.Name = "LblHruLayers"
        Me.LblHruLayers.Size = New System.Drawing.Size(137, 16)
        Me.LblHruLayers.TabIndex = 7
        Me.LblHruLayers.Text = "HRU Layers in AOI"
        '
        'LstHruLayers
        '
        Me.LstHruLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstHruLayers.FormattingEnabled = True
        Me.LstHruLayers.ItemHeight = 16
        Me.LstHruLayers.Location = New System.Drawing.Point(192, 39)
        Me.LstHruLayers.Name = "LstHruLayers"
        Me.LstHruLayers.Size = New System.Drawing.Size(226, 100)
        Me.LstHruLayers.TabIndex = 6
        '
        'BtnViewLayerLog
        '
        Me.BtnViewLayerLog.Enabled = False
        Me.BtnViewLayerLog.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnViewLayerLog.Location = New System.Drawing.Point(424, 73)
        Me.BtnViewLayerLog.Name = "BtnViewLayerLog"
        Me.BtnViewLayerLog.Size = New System.Drawing.Size(110, 28)
        Me.BtnViewLayerLog.TabIndex = 8
        Me.BtnViewLayerLog.Text = "View Layer Log"
        Me.BtnViewLayerLog.UseVisualStyleBackColor = True
        '
        'BtnViewLayer
        '
        Me.BtnViewLayer.Enabled = False
        Me.BtnViewLayer.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnViewLayer.Location = New System.Drawing.Point(424, 39)
        Me.BtnViewLayer.Name = "BtnViewLayer"
        Me.BtnViewLayer.Size = New System.Drawing.Size(110, 28)
        Me.BtnViewLayer.TabIndex = 9
        Me.BtnViewLayer.Text = "View Layer"
        Me.BtnViewLayer.UseVisualStyleBackColor = True
        '
        'LblTemplateHru
        '
        Me.LblTemplateHru.AutoSize = True
        Me.LblTemplateHru.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblTemplateHru.Location = New System.Drawing.Point(15, 3)
        Me.LblTemplateHru.Name = "LblTemplateHru"
        Me.LblTemplateHru.Size = New System.Drawing.Size(129, 16)
        Me.LblTemplateHru.TabIndex = 11
        Me.LblTemplateHru.Text = "Parent HRU layer"
        '
        'LstSelectedZones
        '
        Me.LstSelectedZones.Enabled = False
        Me.LstSelectedZones.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstSelectedZones.FormattingEnabled = True
        Me.LstSelectedZones.IntegralHeight = False
        Me.LstSelectedZones.ItemHeight = 16
        Me.LstSelectedZones.Location = New System.Drawing.Point(423, 3)
        Me.LstSelectedZones.Name = "LstSelectedZones"
        Me.LstSelectedZones.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.LstSelectedZones.Size = New System.Drawing.Size(68, 64)
        Me.LstSelectedZones.TabIndex = 14
        '
        'GrpRules
        '
        Me.GrpRules.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.GrpRules.Controls.Add(Me.CkRetainAttributes)
        Me.GrpRules.Controls.Add(Me.TxtLayerLocation)
        Me.GrpRules.Controls.Add(Me.CkNonContiguous)
        Me.GrpRules.Controls.Add(Me.Label1)
        Me.GrpRules.Controls.Add(Me.GridViewRules)
        Me.GrpRules.Controls.Add(Me.BtnRunRule)
        Me.GrpRules.Controls.Add(Me.LblLayerNameValue)
        Me.GrpRules.Controls.Add(Me.LblLayerTypeValue)
        Me.GrpRules.Controls.Add(Me.CboRuleType)
        Me.GrpRules.Controls.Add(Me.LblOutputHruName)
        Me.GrpRules.Controls.Add(Me.TxtNewHruName)
        Me.GrpRules.Controls.Add(Me.BtnDeleteRule)
        Me.GrpRules.Controls.Add(Me.BtnDefineRule)
        Me.GrpRules.Controls.Add(Me.BtnUpdateRule)
        Me.GrpRules.Controls.Add(Me.LblRuleType)
        Me.GrpRules.Location = New System.Drawing.Point(12, 221)
        Me.GrpRules.Name = "GrpRules"
        Me.GrpRules.Size = New System.Drawing.Size(640, 317)
        Me.GrpRules.TabIndex = 15
        Me.GrpRules.TabStop = False
        '
        'CkRetainAttributes
        '
        Me.CkRetainAttributes.AutoSize = True
        Me.CkRetainAttributes.Checked = True
        Me.CkRetainAttributes.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CkRetainAttributes.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CkRetainAttributes.Location = New System.Drawing.Point(351, 28)
        Me.CkRetainAttributes.Name = "CkRetainAttributes"
        Me.CkRetainAttributes.Size = New System.Drawing.Size(191, 20)
        Me.CkRetainAttributes.TabIndex = 35
        Me.CkRetainAttributes.Text = "Retain source attributes"
        Me.CkRetainAttributes.UseVisualStyleBackColor = True
        '
        'TxtLayerLocation
        '
        Me.TxtLayerLocation.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtLayerLocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtLayerLocation.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtLayerLocation.Location = New System.Drawing.Point(165, 51)
        Me.TxtLayerLocation.Name = "TxtLayerLocation"
        Me.TxtLayerLocation.ReadOnly = True
        Me.TxtLayerLocation.Size = New System.Drawing.Size(470, 22)
        Me.TxtLayerLocation.TabIndex = 32
        '
        'CkNonContiguous
        '
        Me.CkNonContiguous.AutoSize = True
        Me.CkNonContiguous.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CkNonContiguous.Location = New System.Drawing.Point(351, 9)
        Me.CkNonContiguous.Name = "CkNonContiguous"
        Me.CkNonContiguous.Size = New System.Drawing.Size(218, 20)
        Me.CkNonContiguous.TabIndex = 33
        Me.CkNonContiguous.Text = "Allow non-contiguous HRUs"
        Me.CkNonContiguous.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(69, 54)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(99, 16)
        Me.Label1.TabIndex = 31
        Me.Label1.Text = "Layer Location:"
        '
        'GridViewRules
        '
        Me.GridViewRules.AllowUserToAddRows = False
        Me.GridViewRules.AllowUserToDeleteRows = False
        DataGridViewCellStyle11.BackColor = System.Drawing.Color.Silver
        Me.GridViewRules.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle11
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.ControlDarkDark
        DataGridViewCellStyle12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.Info
        DataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.GridViewRules.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle12
        Me.GridViewRules.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.RuleType, Me.LayerUsed, Me.OutputFolder, Me.Status, Me.View, Me.RuleId})
        DataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle14.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.GridViewRules.DefaultCellStyle = DataGridViewCellStyle14
        Me.GridViewRules.Location = New System.Drawing.Point(6, 144)
        Me.GridViewRules.Name = "GridViewRules"
        DataGridViewCellStyle15.BackColor = System.Drawing.Color.White
        Me.GridViewRules.RowsDefaultCellStyle = DataGridViewCellStyle15
        Me.GridViewRules.Size = New System.Drawing.Size(630, 164)
        Me.GridViewRules.TabIndex = 30
        '
        'RuleType
        '
        Me.RuleType.HeaderText = "RuleType"
        Me.RuleType.Name = "RuleType"
        Me.RuleType.ReadOnly = True
        Me.RuleType.Width = 172
        '
        'LayerUsed
        '
        Me.LayerUsed.HeaderText = "Layer Used"
        Me.LayerUsed.Name = "LayerUsed"
        Me.LayerUsed.ReadOnly = True
        Me.LayerUsed.Width = 140
        '
        'OutputFolder
        '
        Me.OutputFolder.HeaderText = "Output Folder"
        Me.OutputFolder.Name = "OutputFolder"
        Me.OutputFolder.Width = 120
        '
        'Status
        '
        Me.Status.HeaderText = "Status"
        Me.Status.Name = "Status"
        Me.Status.ReadOnly = True
        Me.Status.Width = 75
        '
        'View
        '
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle13.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        DataGridViewCellStyle13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.View.DefaultCellStyle = DataGridViewCellStyle13
        Me.View.HeaderText = "View"
        Me.View.Name = "View"
        Me.View.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.View.ToolTipText = "View"
        Me.View.UseColumnTextForButtonValue = True
        Me.View.Width = 80
        '
        'RuleId
        '
        Me.RuleId.HeaderText = "RuleId"
        Me.RuleId.Name = "RuleId"
        Me.RuleId.ReadOnly = True
        Me.RuleId.Visible = False
        '
        'BtnRunRule
        '
        Me.BtnRunRule.Enabled = False
        Me.BtnRunRule.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnRunRule.Location = New System.Drawing.Point(146, 111)
        Me.BtnRunRule.Name = "BtnRunRule"
        Me.BtnRunRule.Size = New System.Drawing.Size(104, 27)
        Me.BtnRunRule.TabIndex = 34
        Me.BtnRunRule.Text = "Run Rule"
        Me.BtnRunRule.UseVisualStyleBackColor = True
        '
        'LblLayerNameValue
        '
        Me.LblLayerNameValue.AutoSize = True
        Me.LblLayerNameValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLayerNameValue.Location = New System.Drawing.Point(245, 97)
        Me.LblLayerNameValue.Name = "LblLayerNameValue"
        Me.LblLayerNameValue.Size = New System.Drawing.Size(0, 16)
        Me.LblLayerNameValue.TabIndex = 29
        '
        'LblLayerTypeValue
        '
        Me.LblLayerTypeValue.AutoSize = True
        Me.LblLayerTypeValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLayerTypeValue.Location = New System.Drawing.Point(393, 99)
        Me.LblLayerTypeValue.Name = "LblLayerTypeValue"
        Me.LblLayerTypeValue.Size = New System.Drawing.Size(0, 16)
        Me.LblLayerTypeValue.TabIndex = 28
        '
        'CboRuleType
        '
        Me.CboRuleType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboRuleType.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboRuleType.FormattingEnabled = True
        Me.CboRuleType.Items.AddRange(New Object() {"Contributing Area", "DAFlow-Type Zones", "PRISM Precipitation", "Raster Reclass (Continuous data)", "Raster Reclass (Discrete data)", "Raster Slice", "Template - Aspect", "Template - Canopy", "Template - Land Use", "Template - Slope"})
        Me.CboRuleType.Location = New System.Drawing.Point(146, 79)
        Me.CboRuleType.Name = "CboRuleType"
        Me.CboRuleType.Size = New System.Drawing.Size(220, 24)
        Me.CboRuleType.TabIndex = 28
        '
        'LblOutputHruName
        '
        Me.LblOutputHruName.AutoSize = True
        Me.LblOutputHruName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblOutputHruName.Location = New System.Drawing.Point(36, 16)
        Me.LblOutputHruName.Name = "LblOutputHruName"
        Me.LblOutputHruName.Size = New System.Drawing.Size(140, 16)
        Me.LblOutputHruName.TabIndex = 26
        Me.LblOutputHruName.Text = "Output Layer Name"
        '
        'TxtNewHruName
        '
        Me.TxtNewHruName.Enabled = False
        Me.TxtNewHruName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtNewHruName.Location = New System.Drawing.Point(180, 13)
        Me.TxtNewHruName.Name = "TxtNewHruName"
        Me.TxtNewHruName.Size = New System.Drawing.Size(143, 22)
        Me.TxtNewHruName.TabIndex = 25
        '
        'BtnDeleteRule
        '
        Me.BtnDeleteRule.Enabled = False
        Me.BtnDeleteRule.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDeleteRule.Location = New System.Drawing.Point(262, 111)
        Me.BtnDeleteRule.Name = "BtnDeleteRule"
        Me.BtnDeleteRule.Size = New System.Drawing.Size(100, 27)
        Me.BtnDeleteRule.TabIndex = 23
        Me.BtnDeleteRule.Text = "Delete/Clear"
        Me.BtnDeleteRule.UseVisualStyleBackColor = True
        '
        'BtnDefineRule
        '
        Me.BtnDefineRule.Enabled = False
        Me.BtnDefineRule.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDefineRule.Location = New System.Drawing.Point(374, 77)
        Me.BtnDefineRule.Name = "BtnDefineRule"
        Me.BtnDefineRule.Size = New System.Drawing.Size(100, 27)
        Me.BtnDefineRule.TabIndex = 22
        Me.BtnDefineRule.Text = "Define Rule"
        Me.BtnDefineRule.UseVisualStyleBackColor = True
        '
        'BtnUpdateRule
        '
        Me.BtnUpdateRule.Enabled = False
        Me.BtnUpdateRule.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnUpdateRule.Location = New System.Drawing.Point(374, 111)
        Me.BtnUpdateRule.Name = "BtnUpdateRule"
        Me.BtnUpdateRule.Size = New System.Drawing.Size(100, 27)
        Me.BtnUpdateRule.TabIndex = 17
        Me.BtnUpdateRule.Text = "Update Rule"
        Me.BtnUpdateRule.UseVisualStyleBackColor = True
        '
        'LblRuleType
        '
        Me.LblRuleType.AutoSize = True
        Me.LblRuleType.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRuleType.Location = New System.Drawing.Point(69, 82)
        Me.LblRuleType.Name = "LblRuleType"
        Me.LblRuleType.Size = New System.Drawing.Size(74, 16)
        Me.LblRuleType.TabIndex = 17
        Me.LblRuleType.Text = "Rule Type:"
        '
        'RadApplyRulesAll
        '
        Me.RadApplyRulesAll.AutoSize = True
        Me.RadApplyRulesAll.Enabled = False
        Me.RadApplyRulesAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadApplyRulesAll.Location = New System.Drawing.Point(179, 16)
        Me.RadApplyRulesAll.Name = "RadApplyRulesAll"
        Me.RadApplyRulesAll.Size = New System.Drawing.Size(248, 20)
        Me.RadApplyRulesAll.TabIndex = 0
        Me.RadApplyRulesAll.TabStop = True
        Me.RadApplyRulesAll.Text = "Apply rules on all existing zones"
        Me.RadApplyRulesAll.UseVisualStyleBackColor = True
        '
        'PnlApplyRules
        '
        Me.PnlApplyRules.Controls.Add(Me.BtnToggle)
        Me.PnlApplyRules.Controls.Add(Me.CboParentHru)
        Me.PnlApplyRules.Controls.Add(Me.RadApplyRulesSelect)
        Me.PnlApplyRules.Controls.Add(Me.RadApplyRulesAll)
        Me.PnlApplyRules.Controls.Add(Me.LstSelectedZones)
        Me.PnlApplyRules.Controls.Add(Me.LblTemplateHru)
        Me.PnlApplyRules.Location = New System.Drawing.Point(31, 145)
        Me.PnlApplyRules.Name = "PnlApplyRules"
        Me.PnlApplyRules.Size = New System.Drawing.Size(618, 70)
        Me.PnlApplyRules.TabIndex = 28
        '
        'BtnToggle
        '
        Me.BtnToggle.Enabled = False
        Me.BtnToggle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnToggle.Location = New System.Drawing.Point(499, 3)
        Me.BtnToggle.Name = "BtnToggle"
        Me.BtnToggle.Size = New System.Drawing.Size(119, 28)
        Me.BtnToggle.TabIndex = 57
        Me.BtnToggle.Text = "Toggle Selection"
        Me.BtnToggle.UseVisualStyleBackColor = True
        '
        'CboParentHru
        '
        Me.CboParentHru.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboParentHru.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboParentHru.FormattingEnabled = True
        Me.CboParentHru.Location = New System.Drawing.Point(18, 24)
        Me.CboParentHru.Name = "CboParentHru"
        Me.CboParentHru.Size = New System.Drawing.Size(144, 24)
        Me.CboParentHru.TabIndex = 15
        '
        'RadApplyRulesSelect
        '
        Me.RadApplyRulesSelect.AutoSize = True
        Me.RadApplyRulesSelect.Enabled = False
        Me.RadApplyRulesSelect.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadApplyRulesSelect.Location = New System.Drawing.Point(179, 39)
        Me.RadApplyRulesSelect.Name = "RadApplyRulesSelect"
        Me.RadApplyRulesSelect.Size = New System.Drawing.Size(234, 20)
        Me.RadApplyRulesSelect.TabIndex = 1
        Me.RadApplyRulesSelect.TabStop = True
        Me.RadApplyRulesSelect.Text = "Apply rules on selected zones"
        Me.RadApplyRulesSelect.UseVisualStyleBackColor = True
        '
        'BtnGenerateHru
        '
        Me.BtnGenerateHru.Enabled = False
        Me.BtnGenerateHru.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnGenerateHru.Location = New System.Drawing.Point(510, 541)
        Me.BtnGenerateHru.Name = "BtnGenerateHru"
        Me.BtnGenerateHru.Size = New System.Drawing.Size(125, 30)
        Me.BtnGenerateHru.TabIndex = 34
        Me.BtnGenerateHru.Text = "Generate HRUs"
        Me.BtnGenerateHru.UseVisualStyleBackColor = True
        '
        'BtnDeleteLayer
        '
        Me.BtnDeleteLayer.Enabled = False
        Me.BtnDeleteLayer.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDeleteLayer.Location = New System.Drawing.Point(541, 73)
        Me.BtnDeleteLayer.Name = "BtnDeleteLayer"
        Me.BtnDeleteLayer.Size = New System.Drawing.Size(110, 28)
        Me.BtnDeleteLayer.TabIndex = 35
        Me.BtnDeleteLayer.Text = "Delete Layer"
        Me.BtnDeleteLayer.UseVisualStyleBackColor = True
        '
        'TxtAoiPath
        '
        Me.TxtAoiPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtAoiPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtAoiPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtAoiPath.Location = New System.Drawing.Point(192, 11)
        Me.TxtAoiPath.Name = "TxtAoiPath"
        Me.TxtAoiPath.ReadOnly = True
        Me.TxtAoiPath.Size = New System.Drawing.Size(459, 22)
        Me.TxtAoiPath.TabIndex = 53
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(123, 14)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(63, 16)
        Me.Label5.TabIndex = 52
        Me.Label5.Text = "AOI Path:"
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(386, 541)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(108, 30)
        Me.BtnCancel.TabIndex = 55
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'BtnRenameLayer
        '
        Me.BtnRenameLayer.Enabled = False
        Me.BtnRenameLayer.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnRenameLayer.Location = New System.Drawing.Point(541, 39)
        Me.BtnRenameLayer.Name = "BtnRenameLayer"
        Me.BtnRenameLayer.Size = New System.Drawing.Size(110, 28)
        Me.BtnRenameLayer.TabIndex = 56
        Me.BtnRenameLayer.Text = "Rename Layer"
        Me.BtnRenameLayer.UseVisualStyleBackColor = True
        '
        'frmHruZone
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.Controls.Add(Me.BtnRenameLayer)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.TxtAoiPath)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.BtnDeleteLayer)
        Me.Controls.Add(Me.BtnGenerateHru)
        Me.Controls.Add(Me.PnlApplyRules)
        Me.Controls.Add(Me.GrpRules)
        Me.Controls.Add(Me.BtnViewLayer)
        Me.Controls.Add(Me.BtnViewLayerLog)
        Me.Controls.Add(Me.LblHruLayers)
        Me.Controls.Add(Me.LstHruLayers)
        Me.Controls.Add(Me.BtnSelectAoi)
        Me.Margin = New System.Windows.Forms.Padding(0)
        Me.Name = "frmHruZone"
        Me.Size = New System.Drawing.Size(665, 575)
        Me.GrpRules.ResumeLayout(False)
        Me.GrpRules.PerformLayout()
        CType(Me.GridViewRules, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlApplyRules.ResumeLayout(False)
        Me.PnlApplyRules.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnSelectAoi As System.Windows.Forms.Button
    Friend WithEvents LblHruLayers As System.Windows.Forms.Label
    Friend WithEvents LstHruLayers As System.Windows.Forms.ListBox
    Friend WithEvents BtnViewLayerLog As System.Windows.Forms.Button
    Friend WithEvents BtnViewLayer As System.Windows.Forms.Button
    Friend WithEvents LblTemplateHru As System.Windows.Forms.Label
    Friend WithEvents LstSelectedZones As System.Windows.Forms.ListBox
    Friend WithEvents GrpRules As System.Windows.Forms.GroupBox
    Friend WithEvents LblRuleType As System.Windows.Forms.Label
    Friend WithEvents BtnDefineRule As System.Windows.Forms.Button
    Friend WithEvents BtnUpdateRule As System.Windows.Forms.Button
    Friend WithEvents BtnDeleteRule As System.Windows.Forms.Button
    Friend WithEvents LblOutputHruName As System.Windows.Forms.Label
    Friend WithEvents TxtNewHruName As System.Windows.Forms.TextBox
    Friend WithEvents CboRuleType As System.Windows.Forms.ComboBox
    Friend WithEvents LblLayerNameValue As System.Windows.Forms.Label
    Friend WithEvents LblLayerTypeValue As System.Windows.Forms.Label
    Friend WithEvents GridViewRules As System.Windows.Forms.DataGridView
    Friend WithEvents RadApplyRulesAll As System.Windows.Forms.RadioButton
    Friend WithEvents PnlApplyRules As System.Windows.Forms.Panel
    Friend WithEvents RadApplyRulesSelect As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TxtLayerLocation As System.Windows.Forms.TextBox
    Friend WithEvents ToolTipZones As System.Windows.Forms.ToolTip
    Friend WithEvents BtnGenerateHru As System.Windows.Forms.Button
    Friend WithEvents CkNonContiguous As System.Windows.Forms.CheckBox
    Friend WithEvents BtnDeleteLayer As System.Windows.Forms.Button
    Friend WithEvents BtnRunRule As System.Windows.Forms.Button
    Friend WithEvents RuleType As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LayerUsed As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents OutputFolder As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Status As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents View As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents RuleId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CboParentHru As System.Windows.Forms.ComboBox
    Friend WithEvents TxtAoiPath As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents BtnRenameLayer As System.Windows.Forms.Button
    Friend WithEvents CkRetainAttributes As System.Windows.Forms.CheckBox
    Friend WithEvents BtnToggle As System.Windows.Forms.Button

End Class