<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmPrismPrecipRule
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CboPrecipType = New System.Windows.Forms.ComboBox()
        Me.LblFrom = New System.Windows.Forms.Label()
        Me.CboBegin = New System.Windows.Forms.ComboBox()
        Me.CboEnd = New System.Windows.Forms.ComboBox()
        Me.LblTo = New System.Windows.Forms.Label()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.BtnApply = New System.Windows.Forms.Button()
        Me.TxtRuleId = New System.Windows.Forms.TextBox()
        Me.BtnAbout = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GrpReclass = New System.Windows.Forms.GroupBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.RdoMillimeters = New System.Windows.Forms.RadioButton()
        Me.RdoInches = New System.Windows.Forms.RadioButton()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.PnlReclass = New System.Windows.Forms.Panel()
        Me.RdoEqArea = New System.Windows.Forms.RadioButton()
        Me.RdoEqInterval = New System.Windows.Forms.RadioButton()
        Me.BtnReclass = New System.Windows.Forms.Button()
        Me.TxtClasses = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.TxtMaxValue = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.TxtMinValue = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.From = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ToValues = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NewValue = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BtnSave = New System.Windows.Forms.Button()
        Me.BtnClear = New System.Windows.Forms.Button()
        Me.BtnLoad = New System.Windows.Forms.Button()
        Me.BtnStats = New System.Windows.Forms.Button()
        Me.TxtTemplatesFile = New System.Windows.Forms.TextBox()
        Me.GrpReclass.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.PnlReclass.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(11, 53)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(94, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "PRISM data:"
        '
        'CboPrecipType
        '
        Me.CboPrecipType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboPrecipType.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboPrecipType.FormattingEnabled = True
        Me.CboPrecipType.Location = New System.Drawing.Point(104, 49)
        Me.CboPrecipType.Name = "CboPrecipType"
        Me.CboPrecipType.Size = New System.Drawing.Size(175, 24)
        Me.CboPrecipType.TabIndex = 1
        '
        'LblFrom
        '
        Me.LblFrom.AutoSize = True
        Me.LblFrom.Enabled = False
        Me.LblFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblFrom.Location = New System.Drawing.Point(302, 54)
        Me.LblFrom.Name = "LblFrom"
        Me.LblFrom.Size = New System.Drawing.Size(47, 16)
        Me.LblFrom.TabIndex = 2
        Me.LblFrom.Text = "From:"
        '
        'CboBegin
        '
        Me.CboBegin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboBegin.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboBegin.FormattingEnabled = True
        Me.CboBegin.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"})
        Me.CboBegin.Location = New System.Drawing.Point(346, 49)
        Me.CboBegin.Name = "CboBegin"
        Me.CboBegin.Size = New System.Drawing.Size(50, 24)
        Me.CboBegin.TabIndex = 3
        '
        'CboEnd
        '
        Me.CboEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboEnd.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboEnd.FormattingEnabled = True
        Me.CboEnd.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"})
        Me.CboEnd.Location = New System.Drawing.Point(433, 50)
        Me.CboEnd.Name = "CboEnd"
        Me.CboEnd.Size = New System.Drawing.Size(50, 24)
        Me.CboEnd.TabIndex = 4
        '
        'LblTo
        '
        Me.LblTo.AutoSize = True
        Me.LblTo.Enabled = False
        Me.LblTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblTo.Location = New System.Drawing.Point(396, 54)
        Me.LblTo.Name = "LblTo"
        Me.LblTo.Size = New System.Drawing.Size(31, 16)
        Me.LblTo.TabIndex = 5
        Me.LblTo.Text = "To:"
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(345, 383)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(60, 25)
        Me.BtnCancel.TabIndex = 18
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'BtnApply
        '
        Me.BtnApply.Enabled = False
        Me.BtnApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnApply.Location = New System.Drawing.Point(422, 383)
        Me.BtnApply.Name = "BtnApply"
        Me.BtnApply.Size = New System.Drawing.Size(60, 25)
        Me.BtnApply.TabIndex = 17
        Me.BtnApply.Text = "Apply"
        Me.BtnApply.UseVisualStyleBackColor = True
        '
        'TxtRuleId
        '
        Me.TxtRuleId.Enabled = False
        Me.TxtRuleId.Location = New System.Drawing.Point(12, 394)
        Me.TxtRuleId.Name = "TxtRuleId"
        Me.TxtRuleId.Size = New System.Drawing.Size(45, 20)
        Me.TxtRuleId.TabIndex = 26
        Me.TxtRuleId.Visible = False
        '
        'BtnAbout
        '
        Me.BtnAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAbout.Location = New System.Drawing.Point(378, 8)
        Me.BtnAbout.Name = "BtnAbout"
        Me.BtnAbout.Size = New System.Drawing.Size(105, 28)
        Me.BtnAbout.TabIndex = 51
        Me.BtnAbout.Text = "Tell me more"
        Me.BtnAbout.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(11, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(235, 16)
        Me.Label2.TabIndex = 52
        Me.Label2.Text = "Use precipitation to define zones"
        '
        'GrpReclass
        '
        Me.GrpReclass.Controls.Add(Me.Panel1)
        Me.GrpReclass.Controls.Add(Me.PnlReclass)
        Me.GrpReclass.Controls.Add(Me.DataGridView1)
        Me.GrpReclass.Controls.Add(Me.BtnSave)
        Me.GrpReclass.Controls.Add(Me.BtnClear)
        Me.GrpReclass.Controls.Add(Me.BtnLoad)
        Me.GrpReclass.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpReclass.Location = New System.Drawing.Point(12, 113)
        Me.GrpReclass.Name = "GrpReclass"
        Me.GrpReclass.Size = New System.Drawing.Size(473, 262)
        Me.GrpReclass.TabIndex = 61
        Me.GrpReclass.TabStop = False
        Me.GrpReclass.Text = "Reclass data"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.RdoMillimeters)
        Me.Panel1.Controls.Add(Me.RdoInches)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Location = New System.Drawing.Point(3, 18)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(465, 25)
        Me.Panel1.TabIndex = 83
        '
        'RdoMillimeters
        '
        Me.RdoMillimeters.AutoSize = True
        Me.RdoMillimeters.Enabled = False
        Me.RdoMillimeters.Location = New System.Drawing.Point(204, 2)
        Me.RdoMillimeters.Name = "RdoMillimeters"
        Me.RdoMillimeters.Size = New System.Drawing.Size(90, 20)
        Me.RdoMillimeters.TabIndex = 83
        Me.RdoMillimeters.Text = "Millimeters"
        Me.RdoMillimeters.UseVisualStyleBackColor = True
        '
        'RdoInches
        '
        Me.RdoInches.AutoSize = True
        Me.RdoInches.Enabled = False
        Me.RdoInches.Location = New System.Drawing.Point(137, 3)
        Me.RdoInches.Name = "RdoInches"
        Me.RdoInches.Size = New System.Drawing.Size(65, 20)
        Me.RdoInches.TabIndex = 58
        Me.RdoInches.Text = "Inches"
        Me.RdoInches.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(1, 3)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(138, 16)
        Me.Label3.TabIndex = 82
        Me.Label3.Text = "Precipitation Units:"
        '
        'PnlReclass
        '
        Me.PnlReclass.Controls.Add(Me.RdoEqArea)
        Me.PnlReclass.Controls.Add(Me.RdoEqInterval)
        Me.PnlReclass.Controls.Add(Me.BtnReclass)
        Me.PnlReclass.Controls.Add(Me.TxtClasses)
        Me.PnlReclass.Controls.Add(Me.Label15)
        Me.PnlReclass.Controls.Add(Me.TxtMaxValue)
        Me.PnlReclass.Controls.Add(Me.Label14)
        Me.PnlReclass.Controls.Add(Me.TxtMinValue)
        Me.PnlReclass.Controls.Add(Me.Label13)
        Me.PnlReclass.Location = New System.Drawing.Point(2, 46)
        Me.PnlReclass.Name = "PnlReclass"
        Me.PnlReclass.Size = New System.Drawing.Size(200, 150)
        Me.PnlReclass.TabIndex = 57
        '
        'RdoEqArea
        '
        Me.RdoEqArea.AutoSize = True
        Me.RdoEqArea.Enabled = False
        Me.RdoEqArea.Location = New System.Drawing.Point(105, 78)
        Me.RdoEqArea.Name = "RdoEqArea"
        Me.RdoEqArea.Size = New System.Drawing.Size(93, 20)
        Me.RdoEqArea.TabIndex = 83
        Me.RdoEqArea.Text = "Equal Area"
        Me.RdoEqArea.UseVisualStyleBackColor = True
        '
        'RdoEqInterval
        '
        Me.RdoEqInterval.AutoSize = True
        Me.RdoEqInterval.Checked = True
        Me.RdoEqInterval.Enabled = False
        Me.RdoEqInterval.Location = New System.Drawing.Point(3, 78)
        Me.RdoEqInterval.Name = "RdoEqInterval"
        Me.RdoEqInterval.Size = New System.Drawing.Size(107, 20)
        Me.RdoEqInterval.TabIndex = 82
        Me.RdoEqInterval.TabStop = True
        Me.RdoEqInterval.Text = "Equal Interval"
        Me.RdoEqInterval.UseVisualStyleBackColor = True
        '
        'BtnReclass
        '
        Me.BtnReclass.Enabled = False
        Me.BtnReclass.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnReclass.Location = New System.Drawing.Point(59, 104)
        Me.BtnReclass.Name = "BtnReclass"
        Me.BtnReclass.Size = New System.Drawing.Size(75, 25)
        Me.BtnReclass.TabIndex = 81
        Me.BtnReclass.Text = "Reclass"
        Me.BtnReclass.UseVisualStyleBackColor = True
        '
        'TxtClasses
        '
        Me.TxtClasses.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtClasses.Location = New System.Drawing.Point(79, 50)
        Me.TxtClasses.Name = "TxtClasses"
        Me.TxtClasses.Size = New System.Drawing.Size(57, 22)
        Me.TxtClasses.TabIndex = 59
        Me.TxtClasses.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(3, 53)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(80, 16)
        Me.Label15.TabIndex = 58
        Me.Label15.Text = "# Classes:"
        '
        'TxtMaxValue
        '
        Me.TxtMaxValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtMaxValue.Location = New System.Drawing.Point(79, 26)
        Me.TxtMaxValue.Name = "TxtMaxValue"
        Me.TxtMaxValue.Size = New System.Drawing.Size(57, 22)
        Me.TxtMaxValue.TabIndex = 57
        Me.TxtMaxValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(3, 29)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(82, 16)
        Me.Label14.TabIndex = 56
        Me.Label14.Text = "Max value:"
        '
        'TxtMinValue
        '
        Me.TxtMinValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtMinValue.Location = New System.Drawing.Point(79, 4)
        Me.TxtMinValue.Name = "TxtMinValue"
        Me.TxtMinValue.Size = New System.Drawing.Size(57, 22)
        Me.TxtMinValue.TabIndex = 55
        Me.TxtMinValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(3, 4)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(78, 16)
        Me.Label13.TabIndex = 54
        Me.Label13.Text = "Min value:"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.From, Me.ToValues, Me.NewValue})
        Me.DataGridView1.Location = New System.Drawing.Point(203, 46)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(265, 165)
        Me.DataGridView1.TabIndex = 5
        '
        'From
        '
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.From.DefaultCellStyle = DataGridViewCellStyle1
        Me.From.HeaderText = "From"
        Me.From.Name = "From"
        Me.From.ReadOnly = True
        Me.From.Width = 60
        '
        'ToValues
        '
        Me.ToValues.HeaderText = "To"
        Me.ToValues.Name = "ToValues"
        Me.ToValues.Width = 60
        '
        'NewValue
        '
        Me.NewValue.HeaderText = "New Value"
        Me.NewValue.Name = "NewValue"
        '
        'BtnSave
        '
        Me.BtnSave.Enabled = False
        Me.BtnSave.Location = New System.Drawing.Point(340, 217)
        Me.BtnSave.Name = "BtnSave"
        Me.BtnSave.Size = New System.Drawing.Size(60, 25)
        Me.BtnSave.TabIndex = 4
        Me.BtnSave.Text = "Save..."
        Me.BtnSave.UseVisualStyleBackColor = True
        '
        'BtnClear
        '
        Me.BtnClear.Enabled = False
        Me.BtnClear.Location = New System.Drawing.Point(408, 217)
        Me.BtnClear.Name = "BtnClear"
        Me.BtnClear.Size = New System.Drawing.Size(60, 25)
        Me.BtnClear.TabIndex = 3
        Me.BtnClear.Text = "Clear"
        Me.BtnClear.UseVisualStyleBackColor = True
        '
        'BtnLoad
        '
        Me.BtnLoad.Location = New System.Drawing.Point(272, 217)
        Me.BtnLoad.Name = "BtnLoad"
        Me.BtnLoad.Size = New System.Drawing.Size(60, 25)
        Me.BtnLoad.TabIndex = 2
        Me.BtnLoad.Text = "Load..."
        Me.BtnLoad.UseVisualStyleBackColor = True
        '
        'BtnStats
        '
        Me.BtnStats.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnStats.Location = New System.Drawing.Point(347, 83)
        Me.BtnStats.Name = "BtnStats"
        Me.BtnStats.Size = New System.Drawing.Size(136, 28)
        Me.BtnStats.TabIndex = 62
        Me.BtnStats.Text = "Calculate statistics"
        Me.BtnStats.UseVisualStyleBackColor = True
        '
        'TxtTemplatesFile
        '
        Me.TxtTemplatesFile.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtTemplatesFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtTemplatesFile.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtTemplatesFile.Location = New System.Drawing.Point(63, 392)
        Me.TxtTemplatesFile.Name = "TxtTemplatesFile"
        Me.TxtTemplatesFile.ReadOnly = True
        Me.TxtTemplatesFile.Size = New System.Drawing.Size(45, 22)
        Me.TxtTemplatesFile.TabIndex = 82
        Me.TxtTemplatesFile.TabStop = False
        Me.TxtTemplatesFile.Visible = False
        '
        'FrmPrismPrecipRule
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(494, 416)
        Me.Controls.Add(Me.TxtTemplatesFile)
        Me.Controls.Add(Me.BtnStats)
        Me.Controls.Add(Me.GrpReclass)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.BtnAbout)
        Me.Controls.Add(Me.TxtRuleId)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnApply)
        Me.Controls.Add(Me.LblTo)
        Me.Controls.Add(Me.CboEnd)
        Me.Controls.Add(Me.CboBegin)
        Me.Controls.Add(Me.LblFrom)
        Me.Controls.Add(Me.CboPrecipType)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Location = New System.Drawing.Point(200, 200)
        Me.Name = "FrmPrismPrecipRule"
        Me.ShowIcon = False
        Me.Text = "PRISM Precipitation"
        Me.GrpReclass.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.PnlReclass.ResumeLayout(False)
        Me.PnlReclass.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CboPrecipType As System.Windows.Forms.ComboBox
    Friend WithEvents LblFrom As System.Windows.Forms.Label
    Friend WithEvents CboBegin As System.Windows.Forms.ComboBox
    Friend WithEvents CboEnd As System.Windows.Forms.ComboBox
    Friend WithEvents LblTo As System.Windows.Forms.Label
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents BtnApply As System.Windows.Forms.Button
    Friend WithEvents TxtRuleId As System.Windows.Forms.TextBox
    Friend WithEvents BtnAbout As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents GrpReclass As System.Windows.Forms.GroupBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents RdoMillimeters As System.Windows.Forms.RadioButton
    Friend WithEvents RdoInches As System.Windows.Forms.RadioButton
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents PnlReclass As System.Windows.Forms.Panel
    Friend WithEvents BtnReclass As System.Windows.Forms.Button
    Friend WithEvents TxtClasses As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents TxtMaxValue As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents TxtMinValue As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents BtnSave As System.Windows.Forms.Button
    Friend WithEvents BtnClear As System.Windows.Forms.Button
    Friend WithEvents BtnLoad As System.Windows.Forms.Button
    Friend WithEvents BtnStats As System.Windows.Forms.Button
    Friend WithEvents TxtTemplatesFile As System.Windows.Forms.TextBox
    Friend WithEvents From As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ToValues As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NewValue As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents RdoEqArea As System.Windows.Forms.RadioButton
    Friend WithEvents RdoEqInterval As System.Windows.Forms.RadioButton
End Class