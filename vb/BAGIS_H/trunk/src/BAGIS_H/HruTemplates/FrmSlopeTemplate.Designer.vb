<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmSlopeTemplate
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.TxtRuleId = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TxtPreWidth = New System.Windows.Forms.TextBox()
        Me.TxtPreHeight = New System.Windows.Forms.TextBox()
        Me.TxtPreIterations = New System.Windows.Forms.TextBox()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.BtnApply = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.TxtPostIterations = New System.Windows.Forms.TextBox()
        Me.TxtPostHeight = New System.Windows.Forms.TextBox()
        Me.TxtPostWidth = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.GrdSlope = New System.Windows.Forms.DataGridView()
        Me.FromValue = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ToValues = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.OutputValues = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.CboOptions = New System.Windows.Forms.ComboBox()
        Me.LblOptions = New System.Windows.Forms.Label()
        Me.RdoDegree = New System.Windows.Forms.RadioButton()
        Me.PnlUnits = New System.Windows.Forms.Panel()
        Me.LblPercent = New System.Windows.Forms.Label()
        Me.RdoPct = New System.Windows.Forms.RadioButton()
        Me.LblToolTip = New System.Windows.Forms.Label()
        Me.BtnAbout = New System.Windows.Forms.Button()
        Me.CkDefault = New System.Windows.Forms.CheckBox()
        Me.PnlMain = New System.Windows.Forms.Panel()
        Me.BtnDeleteTemplate = New System.Windows.Forms.Button()
        Me.PnlReclass = New System.Windows.Forms.Panel()
        Me.BtnReclass = New System.Windows.Forms.Button()
        Me.TxtClasses = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.TxtMaxValue = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.TxtMinValue = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.TxtReclassField = New System.Windows.Forms.TextBox()
        Me.TxtRasterLayer = New System.Windows.Forms.TextBox()
        Me.LblReclassField = New System.Windows.Forms.Label()
        Me.LblRasterLayer = New System.Windows.Forms.Label()
        Me.BtnSelectLyr = New System.Windows.Forms.Button()
        Me.PnlOptions = New System.Windows.Forms.Panel()
        Me.TxtTemplatesFile = New System.Windows.Forms.TextBox()
        Me.LblTemplatesFile = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        CType(Me.GrdSlope, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlUnits.SuspendLayout()
        Me.PnlMain.SuspendLayout()
        Me.PnlReclass.SuspendLayout()
        Me.PnlOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'TxtRuleId
        '
        Me.TxtRuleId.Enabled = False
        Me.TxtRuleId.Location = New System.Drawing.Point(9, 293)
        Me.TxtRuleId.Name = "TxtRuleId"
        Me.TxtRuleId.Size = New System.Drawing.Size(45, 20)
        Me.TxtRuleId.TabIndex = 27
        Me.TxtRuleId.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(3, 3)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(304, 16)
        Me.Label2.TabIndex = 28
        Me.Label2.Text = "Step 1. Low-pass Filtering on Slope Values"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(168, 26)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(57, 16)
        Me.Label3.TabIndex = 29
        Me.Label3.Text = "Height:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(305, 26)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(76, 16)
        Me.Label4.TabIndex = 30
        Me.Label4.Text = "Iterations:"
        '
        'TxtPreWidth
        '
        Me.TxtPreWidth.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtPreWidth.Location = New System.Drawing.Point(94, 23)
        Me.TxtPreWidth.Name = "TxtPreWidth"
        Me.TxtPreWidth.Size = New System.Drawing.Size(58, 22)
        Me.TxtPreWidth.TabIndex = 31
        '
        'TxtPreHeight
        '
        Me.TxtPreHeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtPreHeight.Location = New System.Drawing.Point(231, 23)
        Me.TxtPreHeight.Name = "TxtPreHeight"
        Me.TxtPreHeight.Size = New System.Drawing.Size(57, 22)
        Me.TxtPreHeight.TabIndex = 32
        '
        'TxtPreIterations
        '
        Me.TxtPreIterations.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtPreIterations.Location = New System.Drawing.Point(387, 23)
        Me.TxtPreIterations.Name = "TxtPreIterations"
        Me.TxtPreIterations.Size = New System.Drawing.Size(61, 22)
        Me.TxtPreIterations.TabIndex = 33
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(339, 371)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(60, 25)
        Me.BtnCancel.TabIndex = 35
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'BtnApply
        '
        Me.BtnApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnApply.Location = New System.Drawing.Point(405, 371)
        Me.BtnApply.Name = "BtnApply"
        Me.BtnApply.Size = New System.Drawing.Size(60, 25)
        Me.BtnApply.TabIndex = 34
        Me.BtnApply.Text = "Apply"
        Me.BtnApply.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Red
        Me.Label5.Location = New System.Drawing.Point(3, 371)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(267, 16)
        Me.Label5.TabIndex = 36
        Me.Label5.Text = "Set filter iterations to 0 to skip filtering"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(-3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(148, 16)
        Me.Label1.TabIndex = 37
        Me.Label1.Text = "Slope classification:"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Location = New System.Drawing.Point(4, 76)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(0, 0)
        Me.Panel1.TabIndex = 38
        '
        'TxtPostIterations
        '
        Me.TxtPostIterations.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtPostIterations.Location = New System.Drawing.Point(387, 335)
        Me.TxtPostIterations.Name = "TxtPostIterations"
        Me.TxtPostIterations.Size = New System.Drawing.Size(61, 22)
        Me.TxtPostIterations.TabIndex = 44
        '
        'TxtPostHeight
        '
        Me.TxtPostHeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtPostHeight.Location = New System.Drawing.Point(231, 335)
        Me.TxtPostHeight.Name = "TxtPostHeight"
        Me.TxtPostHeight.Size = New System.Drawing.Size(57, 22)
        Me.TxtPostHeight.TabIndex = 43
        '
        'TxtPostWidth
        '
        Me.TxtPostWidth.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtPostWidth.Location = New System.Drawing.Point(94, 335)
        Me.TxtPostWidth.Name = "TxtPostWidth"
        Me.TxtPostWidth.Size = New System.Drawing.Size(59, 22)
        Me.TxtPostWidth.TabIndex = 42
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(305, 338)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(76, 16)
        Me.Label6.TabIndex = 41
        Me.Label6.Text = "Iterations:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(168, 338)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(57, 16)
        Me.Label7.TabIndex = 40
        Me.Label7.Text = "Height:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(2, 338)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(86, 16)
        Me.Label8.TabIndex = 39
        Me.Label8.Text = "Filter width:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(2, 26)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(86, 16)
        Me.Label9.TabIndex = 45
        Me.Label9.Text = "Filter width:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(3, 56)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(285, 16)
        Me.Label10.TabIndex = 46
        Me.Label10.Text = "Step 2. Reclassification of Slope Values"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(2, 316)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(387, 16)
        Me.Label11.TabIndex = 47
        Me.Label11.Text = "Step 3. Majority Filtering on Reclassified Slope Grades"
        '
        'GrdSlope
        '
        Me.GrdSlope.AllowUserToAddRows = False
        Me.GrdSlope.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.GrdSlope.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.GrdSlope.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GrdSlope.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.FromValue, Me.ToValues, Me.OutputValues})
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.GrdSlope.DefaultCellStyle = DataGridViewCellStyle3
        Me.GrdSlope.Location = New System.Drawing.Point(186, 80)
        Me.GrdSlope.Name = "GrdSlope"
        Me.GrdSlope.Size = New System.Drawing.Size(270, 220)
        Me.GrdSlope.TabIndex = 48
        '
        'FromValue
        '
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.FromValue.DefaultCellStyle = DataGridViewCellStyle2
        Me.FromValue.HeaderText = "From"
        Me.FromValue.Name = "FromValue"
        Me.FromValue.ReadOnly = True
        Me.FromValue.Width = 55
        '
        'ToValues
        '
        Me.ToValues.HeaderText = "To"
        Me.ToValues.Name = "ToValues"
        Me.ToValues.Width = 55
        '
        'OutputValues
        '
        Me.OutputValues.HeaderText = "New Value"
        Me.OutputValues.Name = "OutputValues"
        Me.OutputValues.Width = 110
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(1, 4)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(47, 16)
        Me.Label12.TabIndex = 49
        Me.Label12.Text = "Units:"
        '
        'CboOptions
        '
        Me.CboOptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboOptions.FormattingEnabled = True
        Me.CboOptions.Location = New System.Drawing.Point(8, 47)
        Me.CboOptions.Name = "CboOptions"
        Me.CboOptions.Size = New System.Drawing.Size(130, 24)
        Me.CboOptions.TabIndex = 52
        '
        'LblOptions
        '
        Me.LblOptions.AutoSize = True
        Me.LblOptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblOptions.Location = New System.Drawing.Point(4, 27)
        Me.LblOptions.Name = "LblOptions"
        Me.LblOptions.Size = New System.Drawing.Size(65, 16)
        Me.LblOptions.TabIndex = 51
        Me.LblOptions.Text = "Options:"
        '
        'RdoDegree
        '
        Me.RdoDegree.AutoSize = True
        Me.RdoDegree.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RdoDegree.Location = New System.Drawing.Point(45, 1)
        Me.RdoDegree.Name = "RdoDegree"
        Me.RdoDegree.Size = New System.Drawing.Size(79, 20)
        Me.RdoDegree.TabIndex = 0
        Me.RdoDegree.TabStop = True
        Me.RdoDegree.Text = "Degrees"
        Me.RdoDegree.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.RdoDegree.UseVisualStyleBackColor = True
        '
        'PnlUnits
        '
        Me.PnlUnits.Controls.Add(Me.LblPercent)
        Me.PnlUnits.Controls.Add(Me.RdoPct)
        Me.PnlUnits.Controls.Add(Me.RdoDegree)
        Me.PnlUnits.Controls.Add(Me.Label12)
        Me.PnlUnits.Location = New System.Drawing.Point(2, 74)
        Me.PnlUnits.Name = "PnlUnits"
        Me.PnlUnits.Size = New System.Drawing.Size(130, 42)
        Me.PnlUnits.TabIndex = 53
        '
        'LblPercent
        '
        Me.LblPercent.AutoSize = True
        Me.LblPercent.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblPercent.Location = New System.Drawing.Point(42, 4)
        Me.LblPercent.Name = "LblPercent"
        Me.LblPercent.Size = New System.Drawing.Size(59, 16)
        Me.LblPercent.TabIndex = 73
        Me.LblPercent.Text = "% Slope"
        '
        'RdoPct
        '
        Me.RdoPct.AutoSize = True
        Me.RdoPct.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RdoPct.Location = New System.Drawing.Point(45, 21)
        Me.RdoPct.Name = "RdoPct"
        Me.RdoPct.Size = New System.Drawing.Size(77, 20)
        Me.RdoPct.TabIndex = 1
        Me.RdoPct.TabStop = True
        Me.RdoPct.Text = "% Slope"
        Me.RdoPct.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.RdoPct.UseVisualStyleBackColor = True
        '
        'LblToolTip
        '
        Me.LblToolTip.AutoSize = True
        Me.LblToolTip.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblToolTip.Location = New System.Drawing.Point(12, 11)
        Me.LblToolTip.Name = "LblToolTip"
        Me.LblToolTip.Size = New System.Drawing.Size(238, 16)
        Me.LblToolTip.TabIndex = 71
        Me.LblToolTip.Text = "Use slope values to define zones"
        '
        'BtnAbout
        '
        Me.BtnAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAbout.Location = New System.Drawing.Point(358, 5)
        Me.BtnAbout.Name = "BtnAbout"
        Me.BtnAbout.Size = New System.Drawing.Size(105, 28)
        Me.BtnAbout.TabIndex = 70
        Me.BtnAbout.Text = "Tell me more"
        Me.BtnAbout.UseVisualStyleBackColor = True
        '
        'CkDefault
        '
        Me.CkDefault.AutoSize = True
        Me.CkDefault.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CkDefault.Location = New System.Drawing.Point(9, 74)
        Me.CkDefault.Name = "CkDefault"
        Me.CkDefault.Size = New System.Drawing.Size(108, 20)
        Me.CkDefault.TabIndex = 72
        Me.CkDefault.Text = "Set as default"
        Me.CkDefault.UseVisualStyleBackColor = True
        '
        'PnlMain
        '
        Me.PnlMain.Controls.Add(Me.BtnDeleteTemplate)
        Me.PnlMain.Controls.Add(Me.PnlReclass)
        Me.PnlMain.Controls.Add(Me.Label2)
        Me.PnlMain.Controls.Add(Me.Label3)
        Me.PnlMain.Controls.Add(Me.Label4)
        Me.PnlMain.Controls.Add(Me.TxtRuleId)
        Me.PnlMain.Controls.Add(Me.TxtPreWidth)
        Me.PnlMain.Controls.Add(Me.PnlUnits)
        Me.PnlMain.Controls.Add(Me.TxtPreHeight)
        Me.PnlMain.Controls.Add(Me.TxtPreIterations)
        Me.PnlMain.Controls.Add(Me.BtnApply)
        Me.PnlMain.Controls.Add(Me.GrdSlope)
        Me.PnlMain.Controls.Add(Me.BtnCancel)
        Me.PnlMain.Controls.Add(Me.Label11)
        Me.PnlMain.Controls.Add(Me.Label5)
        Me.PnlMain.Controls.Add(Me.Label10)
        Me.PnlMain.Controls.Add(Me.Label8)
        Me.PnlMain.Controls.Add(Me.Label9)
        Me.PnlMain.Controls.Add(Me.Label7)
        Me.PnlMain.Controls.Add(Me.TxtPostIterations)
        Me.PnlMain.Controls.Add(Me.Label6)
        Me.PnlMain.Controls.Add(Me.TxtPostHeight)
        Me.PnlMain.Controls.Add(Me.TxtPostWidth)
        Me.PnlMain.Location = New System.Drawing.Point(2, 133)
        Me.PnlMain.Name = "PnlMain"
        Me.PnlMain.Size = New System.Drawing.Size(475, 405)
        Me.PnlMain.TabIndex = 73
        '
        'BtnDeleteTemplate
        '
        Me.BtnDeleteTemplate.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDeleteTemplate.Location = New System.Drawing.Point(272, 371)
        Me.BtnDeleteTemplate.Name = "BtnDeleteTemplate"
        Me.BtnDeleteTemplate.Size = New System.Drawing.Size(60, 25)
        Me.BtnDeleteTemplate.TabIndex = 57
        Me.BtnDeleteTemplate.Text = "Delete"
        Me.BtnDeleteTemplate.UseVisualStyleBackColor = True
        '
        'PnlReclass
        '
        Me.PnlReclass.Controls.Add(Me.BtnReclass)
        Me.PnlReclass.Controls.Add(Me.TxtClasses)
        Me.PnlReclass.Controls.Add(Me.Label15)
        Me.PnlReclass.Controls.Add(Me.TxtMaxValue)
        Me.PnlReclass.Controls.Add(Me.Label14)
        Me.PnlReclass.Controls.Add(Me.TxtMinValue)
        Me.PnlReclass.Controls.Add(Me.Label13)
        Me.PnlReclass.Location = New System.Drawing.Point(2, 117)
        Me.PnlReclass.Name = "PnlReclass"
        Me.PnlReclass.Size = New System.Drawing.Size(180, 124)
        Me.PnlReclass.TabIndex = 56
        '
        'BtnReclass
        '
        Me.BtnReclass.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnReclass.Location = New System.Drawing.Point(59, 77)
        Me.BtnReclass.Name = "BtnReclass"
        Me.BtnReclass.Size = New System.Drawing.Size(75, 25)
        Me.BtnReclass.TabIndex = 81
        Me.BtnReclass.Text = "Reclass"
        Me.BtnReclass.UseVisualStyleBackColor = True
        '
        'TxtClasses
        '
        Me.TxtClasses.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtClasses.Location = New System.Drawing.Point(77, 49)
        Me.TxtClasses.Name = "TxtClasses"
        Me.TxtClasses.Size = New System.Drawing.Size(57, 22)
        Me.TxtClasses.TabIndex = 59
        Me.TxtClasses.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(1, 52)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(80, 16)
        Me.Label15.TabIndex = 58
        Me.Label15.Text = "# Classes:"
        '
        'TxtMaxValue
        '
        Me.TxtMaxValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtMaxValue.Location = New System.Drawing.Point(77, 25)
        Me.TxtMaxValue.Name = "TxtMaxValue"
        Me.TxtMaxValue.Size = New System.Drawing.Size(57, 22)
        Me.TxtMaxValue.TabIndex = 57
        Me.TxtMaxValue.Text = "90"
        Me.TxtMaxValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(1, 28)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(82, 16)
        Me.Label14.TabIndex = 56
        Me.Label14.Text = "Max value:"
        '
        'TxtMinValue
        '
        Me.TxtMinValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtMinValue.Location = New System.Drawing.Point(77, 0)
        Me.TxtMinValue.Name = "TxtMinValue"
        Me.TxtMinValue.Size = New System.Drawing.Size(57, 22)
        Me.TxtMinValue.TabIndex = 55
        Me.TxtMinValue.Text = "0"
        Me.TxtMinValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(1, 3)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(78, 16)
        Me.Label13.TabIndex = 54
        Me.Label13.Text = "Min value:"
        '
        'TxtReclassField
        '
        Me.TxtReclassField.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtReclassField.Location = New System.Drawing.Point(262, 49)
        Me.TxtReclassField.Name = "TxtReclassField"
        Me.TxtReclassField.ReadOnly = True
        Me.TxtReclassField.Size = New System.Drawing.Size(110, 22)
        Me.TxtReclassField.TabIndex = 77
        '
        'TxtRasterLayer
        '
        Me.TxtRasterLayer.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtRasterLayer.Location = New System.Drawing.Point(144, 49)
        Me.TxtRasterLayer.Name = "TxtRasterLayer"
        Me.TxtRasterLayer.ReadOnly = True
        Me.TxtRasterLayer.Size = New System.Drawing.Size(110, 22)
        Me.TxtRasterLayer.TabIndex = 76
        '
        'LblReclassField
        '
        Me.LblReclassField.AutoSize = True
        Me.LblReclassField.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblReclassField.Location = New System.Drawing.Point(261, 27)
        Me.LblReclassField.Name = "LblReclassField"
        Me.LblReclassField.Size = New System.Drawing.Size(103, 16)
        Me.LblReclassField.TabIndex = 75
        Me.LblReclassField.Text = "Reclass field:"
        '
        'LblRasterLayer
        '
        Me.LblRasterLayer.AutoSize = True
        Me.LblRasterLayer.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRasterLayer.Location = New System.Drawing.Point(141, 26)
        Me.LblRasterLayer.Name = "LblRasterLayer"
        Me.LblRasterLayer.Size = New System.Drawing.Size(113, 16)
        Me.LblRasterLayer.TabIndex = 74
        Me.LblRasterLayer.Text = "Selected layer:"
        '
        'BtnSelectLyr
        '
        Me.BtnSelectLyr.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSelectLyr.Location = New System.Drawing.Point(379, 47)
        Me.BtnSelectLyr.Name = "BtnSelectLyr"
        Me.BtnSelectLyr.Size = New System.Drawing.Size(100, 25)
        Me.BtnSelectLyr.TabIndex = 78
        Me.BtnSelectLyr.Text = "Select Layer"
        Me.BtnSelectLyr.UseVisualStyleBackColor = True
        Me.BtnSelectLyr.Visible = False
        '
        'PnlOptions
        '
        Me.PnlOptions.Controls.Add(Me.TxtTemplatesFile)
        Me.PnlOptions.Controls.Add(Me.CkDefault)
        Me.PnlOptions.Controls.Add(Me.LblTemplatesFile)
        Me.PnlOptions.Controls.Add(Me.LblReclassField)
        Me.PnlOptions.Controls.Add(Me.BtnSelectLyr)
        Me.PnlOptions.Controls.Add(Me.LblRasterLayer)
        Me.PnlOptions.Controls.Add(Me.TxtReclassField)
        Me.PnlOptions.Controls.Add(Me.TxtRasterLayer)
        Me.PnlOptions.Controls.Add(Me.LblOptions)
        Me.PnlOptions.Controls.Add(Me.CboOptions)
        Me.PnlOptions.Location = New System.Drawing.Point(2, 37)
        Me.PnlOptions.Name = "PnlOptions"
        Me.PnlOptions.Size = New System.Drawing.Size(485, 97)
        Me.PnlOptions.TabIndex = 79
        '
        'TxtTemplatesFile
        '
        Me.TxtTemplatesFile.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtTemplatesFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtTemplatesFile.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtTemplatesFile.Location = New System.Drawing.Point(108, 3)
        Me.TxtTemplatesFile.Name = "TxtTemplatesFile"
        Me.TxtTemplatesFile.ReadOnly = True
        Me.TxtTemplatesFile.Size = New System.Drawing.Size(371, 22)
        Me.TxtTemplatesFile.TabIndex = 80
        Me.TxtTemplatesFile.TabStop = False
        '
        'LblTemplatesFile
        '
        Me.LblTemplatesFile.AutoSize = True
        Me.LblTemplatesFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblTemplatesFile.Location = New System.Drawing.Point(2, 6)
        Me.LblTemplatesFile.Name = "LblTemplatesFile"
        Me.LblTemplatesFile.Size = New System.Drawing.Size(111, 16)
        Me.LblTemplatesFile.TabIndex = 79
        Me.LblTemplatesFile.Text = "Templates file:"
        '
        'FrmSlopeTemplate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(489, 545)
        Me.Controls.Add(Me.PnlOptions)
        Me.Controls.Add(Me.PnlMain)
        Me.Controls.Add(Me.LblToolTip)
        Me.Controls.Add(Me.BtnAbout)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "FrmSlopeTemplate"
        Me.ShowIcon = False
        Me.Text = "Template - Slope"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.GrdSlope, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlUnits.ResumeLayout(False)
        Me.PnlUnits.PerformLayout()
        Me.PnlMain.ResumeLayout(False)
        Me.PnlMain.PerformLayout()
        Me.PnlReclass.ResumeLayout(False)
        Me.PnlReclass.PerformLayout()
        Me.PnlOptions.ResumeLayout(False)
        Me.PnlOptions.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TxtRuleId As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TxtPreWidth As System.Windows.Forms.TextBox
    Friend WithEvents TxtPreHeight As System.Windows.Forms.TextBox
    Friend WithEvents TxtPreIterations As System.Windows.Forms.TextBox
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents BtnApply As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents TxtPostIterations As System.Windows.Forms.TextBox
    Friend WithEvents TxtPostHeight As System.Windows.Forms.TextBox
    Friend WithEvents TxtPostWidth As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents GrdSlope As System.Windows.Forms.DataGridView
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents CboOptions As System.Windows.Forms.ComboBox
    Friend WithEvents LblOptions As System.Windows.Forms.Label
    Friend WithEvents RdoDegree As System.Windows.Forms.RadioButton
    Friend WithEvents PnlUnits As System.Windows.Forms.Panel
    Friend WithEvents RdoPct As System.Windows.Forms.RadioButton
    Friend WithEvents LblToolTip As System.Windows.Forms.Label
    Friend WithEvents BtnAbout As System.Windows.Forms.Button
    Friend WithEvents CkDefault As System.Windows.Forms.CheckBox
    Friend WithEvents LblPercent As System.Windows.Forms.Label
    Friend WithEvents PnlMain As System.Windows.Forms.Panel
    Friend WithEvents TxtReclassField As System.Windows.Forms.TextBox
    Friend WithEvents TxtRasterLayer As System.Windows.Forms.TextBox
    Friend WithEvents LblReclassField As System.Windows.Forms.Label
    Friend WithEvents LblRasterLayer As System.Windows.Forms.Label
    Friend WithEvents BtnSelectLyr As System.Windows.Forms.Button
    Friend WithEvents PnlOptions As System.Windows.Forms.Panel
    Friend WithEvents TxtTemplatesFile As System.Windows.Forms.TextBox
    Friend WithEvents LblTemplatesFile As System.Windows.Forms.Label
    Friend WithEvents PnlReclass As System.Windows.Forms.Panel
    Friend WithEvents TxtMinValue As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents BtnReclass As System.Windows.Forms.Button
    Friend WithEvents TxtClasses As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents TxtMaxValue As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents BtnDeleteTemplate As System.Windows.Forms.Button
    Friend WithEvents FromValue As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ToValues As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents OutputValues As System.Windows.Forms.DataGridViewTextBoxColumn
End Class