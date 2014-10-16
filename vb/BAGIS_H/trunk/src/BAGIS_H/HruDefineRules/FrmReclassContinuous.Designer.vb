<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmReclassContinuous
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.LblToolTip = New System.Windows.Forms.Label()
        Me.BtnAbout = New System.Windows.Forms.Button()
        Me.GrpLayerType = New System.Windows.Forms.GroupBox()
        Me.RdoPrism = New System.Windows.Forms.RadioButton()
        Me.RdoDem = New System.Windows.Forms.RadioButton()
        Me.RdoRaster = New System.Windows.Forms.RadioButton()
        Me.LstAoiRasterLayers = New System.Windows.Forms.ListBox()
        Me.LblRasterLayers = New System.Windows.Forms.Label()
        Me.TxtRuleId = New System.Windows.Forms.TextBox()
        Me.GrpReclass = New System.Windows.Forms.GroupBox()
        Me.PnlReclass = New System.Windows.Forms.Panel()
        Me.BtnReclass = New System.Windows.Forms.Button()
        Me.TxtClasses = New System.Windows.Forms.TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.TxtMaxValue = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.TxtMinValue = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.CboReclassField = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.From = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ToValues = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NewValue = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BtnSave = New System.Windows.Forms.Button()
        Me.BtnClear = New System.Windows.Forms.Button()
        Me.BtnLoad = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.BtnApply = New System.Windows.Forms.Button()
        Me.PnlStatistics = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LblMin = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.LblMax = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.LblSTDV = New System.Windows.Forms.Label()
        Me.LblMean = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.TxtTemplatesFile = New System.Windows.Forms.TextBox()
        Me.GrpLayerType.SuspendLayout()
        Me.GrpReclass.SuspendLayout()
        Me.PnlReclass.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlStatistics.SuspendLayout()
        Me.SuspendLayout()
        '
        'LblToolTip
        '
        Me.LblToolTip.AutoSize = True
        Me.LblToolTip.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblToolTip.Location = New System.Drawing.Point(12, 10)
        Me.LblToolTip.Name = "LblToolTip"
        Me.LblToolTip.Size = New System.Drawing.Size(287, 16)
        Me.LblToolTip.TabIndex = 55
        Me.LblToolTip.Text = "Use a reclassified raster to define zones"
        '
        'BtnAbout
        '
        Me.BtnAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAbout.Location = New System.Drawing.Point(305, 4)
        Me.BtnAbout.Name = "BtnAbout"
        Me.BtnAbout.Size = New System.Drawing.Size(105, 28)
        Me.BtnAbout.TabIndex = 54
        Me.BtnAbout.Text = "Tell me more"
        Me.BtnAbout.UseVisualStyleBackColor = True
        '
        'GrpLayerType
        '
        Me.GrpLayerType.Controls.Add(Me.RdoPrism)
        Me.GrpLayerType.Controls.Add(Me.RdoDem)
        Me.GrpLayerType.Controls.Add(Me.RdoRaster)
        Me.GrpLayerType.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpLayerType.Location = New System.Drawing.Point(18, 36)
        Me.GrpLayerType.Name = "GrpLayerType"
        Me.GrpLayerType.Size = New System.Drawing.Size(86, 101)
        Me.GrpLayerType.TabIndex = 56
        Me.GrpLayerType.TabStop = False
        Me.GrpLayerType.Text = "Layer type"
        '
        'RdoPrism
        '
        Me.RdoPrism.AutoSize = True
        Me.RdoPrism.Location = New System.Drawing.Point(10, 77)
        Me.RdoPrism.Name = "RdoPrism"
        Me.RdoPrism.Size = New System.Drawing.Size(68, 20)
        Me.RdoPrism.TabIndex = 2
        Me.RdoPrism.TabStop = True
        Me.RdoPrism.Text = "PRISM"
        Me.RdoPrism.UseVisualStyleBackColor = True
        '
        'RdoDem
        '
        Me.RdoDem.AutoSize = True
        Me.RdoDem.Location = New System.Drawing.Point(10, 51)
        Me.RdoDem.Name = "RdoDem"
        Me.RdoDem.Size = New System.Drawing.Size(56, 20)
        Me.RdoDem.TabIndex = 1
        Me.RdoDem.TabStop = True
        Me.RdoDem.Text = "DEM"
        Me.RdoDem.UseVisualStyleBackColor = True
        '
        'RdoRaster
        '
        Me.RdoRaster.AutoSize = True
        Me.RdoRaster.Location = New System.Drawing.Point(10, 25)
        Me.RdoRaster.Name = "RdoRaster"
        Me.RdoRaster.Size = New System.Drawing.Size(66, 20)
        Me.RdoRaster.TabIndex = 0
        Me.RdoRaster.TabStop = True
        Me.RdoRaster.Text = "Raster"
        Me.RdoRaster.UseVisualStyleBackColor = True
        '
        'LstAoiRasterLayers
        '
        Me.LstAoiRasterLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstAoiRasterLayers.FormattingEnabled = True
        Me.LstAoiRasterLayers.ItemHeight = 16
        Me.LstAoiRasterLayers.Location = New System.Drawing.Point(114, 55)
        Me.LstAoiRasterLayers.Name = "LstAoiRasterLayers"
        Me.LstAoiRasterLayers.Size = New System.Drawing.Size(146, 116)
        Me.LstAoiRasterLayers.TabIndex = 57
        '
        'LblRasterLayers
        '
        Me.LblRasterLayers.AutoSize = True
        Me.LblRasterLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRasterLayers.Location = New System.Drawing.Point(119, 36)
        Me.LblRasterLayers.Name = "LblRasterLayers"
        Me.LblRasterLayers.Size = New System.Drawing.Size(91, 16)
        Me.LblRasterLayers.TabIndex = 58
        Me.LblRasterLayers.Text = "Select layer"
        '
        'TxtRuleId
        '
        Me.TxtRuleId.Enabled = False
        Me.TxtRuleId.Location = New System.Drawing.Point(18, 454)
        Me.TxtRuleId.Name = "TxtRuleId"
        Me.TxtRuleId.Size = New System.Drawing.Size(45, 20)
        Me.TxtRuleId.TabIndex = 59
        Me.TxtRuleId.Visible = False
        '
        'GrpReclass
        '
        Me.GrpReclass.Controls.Add(Me.PnlReclass)
        Me.GrpReclass.Controls.Add(Me.CboReclassField)
        Me.GrpReclass.Controls.Add(Me.Label9)
        Me.GrpReclass.Controls.Add(Me.DataGridView1)
        Me.GrpReclass.Controls.Add(Me.BtnSave)
        Me.GrpReclass.Controls.Add(Me.BtnClear)
        Me.GrpReclass.Controls.Add(Me.BtnLoad)
        Me.GrpReclass.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpReclass.Location = New System.Drawing.Point(18, 181)
        Me.GrpReclass.Name = "GrpReclass"
        Me.GrpReclass.Size = New System.Drawing.Size(430, 262)
        Me.GrpReclass.TabIndex = 60
        Me.GrpReclass.TabStop = False
        Me.GrpReclass.Text = "Reclass"
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
        Me.PnlReclass.Location = New System.Drawing.Point(2, 43)
        Me.PnlReclass.Name = "PnlReclass"
        Me.PnlReclass.Size = New System.Drawing.Size(142, 124)
        Me.PnlReclass.TabIndex = 57
        '
        'BtnReclass
        '
        Me.BtnReclass.Enabled = False
        Me.BtnReclass.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnReclass.Location = New System.Drawing.Point(35, 77)
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
        'CboReclassField
        '
        Me.CboReclassField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboReclassField.FormattingEnabled = True
        Me.CboReclassField.Location = New System.Drawing.Point(105, 14)
        Me.CboReclassField.Name = "CboReclassField"
        Me.CboReclassField.Size = New System.Drawing.Size(110, 24)
        Me.CboReclassField.TabIndex = 22
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(4, 20)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(103, 16)
        Me.Label9.TabIndex = 21
        Me.Label9.Text = "Reclass field:"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.From, Me.ToValues, Me.NewValue})
        Me.DataGridView1.Location = New System.Drawing.Point(149, 43)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(275, 165)
        Me.DataGridView1.TabIndex = 5
        '
        'From
        '
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.From.DefaultCellStyle = DataGridViewCellStyle2
        Me.From.HeaderText = "From"
        Me.From.Name = "From"
        Me.From.ReadOnly = True
        Me.From.Width = 55
        '
        'ToValues
        '
        Me.ToValues.HeaderText = "To"
        Me.ToValues.Name = "ToValues"
        Me.ToValues.Width = 55
        '
        'NewValue
        '
        Me.NewValue.HeaderText = "New Value"
        Me.NewValue.Name = "NewValue"
        Me.NewValue.Width = 110
        '
        'BtnSave
        '
        Me.BtnSave.Enabled = False
        Me.BtnSave.Location = New System.Drawing.Point(296, 214)
        Me.BtnSave.Name = "BtnSave"
        Me.BtnSave.Size = New System.Drawing.Size(60, 25)
        Me.BtnSave.TabIndex = 4
        Me.BtnSave.Text = "Save..."
        Me.BtnSave.UseVisualStyleBackColor = True
        '
        'BtnClear
        '
        Me.BtnClear.Enabled = False
        Me.BtnClear.Location = New System.Drawing.Point(362, 214)
        Me.BtnClear.Name = "BtnClear"
        Me.BtnClear.Size = New System.Drawing.Size(60, 25)
        Me.BtnClear.TabIndex = 3
        Me.BtnClear.Text = "Clear"
        Me.BtnClear.UseVisualStyleBackColor = True
        '
        'BtnLoad
        '
        Me.BtnLoad.Enabled = False
        Me.BtnLoad.Location = New System.Drawing.Point(230, 214)
        Me.BtnLoad.Name = "BtnLoad"
        Me.BtnLoad.Size = New System.Drawing.Size(60, 25)
        Me.BtnLoad.TabIndex = 2
        Me.BtnLoad.Text = "Load..."
        Me.BtnLoad.UseVisualStyleBackColor = True
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(314, 449)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(60, 25)
        Me.BtnCancel.TabIndex = 33
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'BtnApply
        '
        Me.BtnApply.Enabled = False
        Me.BtnApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnApply.Location = New System.Drawing.Point(384, 449)
        Me.BtnApply.Name = "BtnApply"
        Me.BtnApply.Size = New System.Drawing.Size(60, 25)
        Me.BtnApply.TabIndex = 32
        Me.BtnApply.Text = "Apply"
        Me.BtnApply.UseVisualStyleBackColor = True
        '
        'PnlStatistics
        '
        Me.PnlStatistics.Controls.Add(Me.Label2)
        Me.PnlStatistics.Controls.Add(Me.Label1)
        Me.PnlStatistics.Controls.Add(Me.LblMin)
        Me.PnlStatistics.Controls.Add(Me.Label4)
        Me.PnlStatistics.Controls.Add(Me.LblMax)
        Me.PnlStatistics.Controls.Add(Me.Label5)
        Me.PnlStatistics.Controls.Add(Me.LblSTDV)
        Me.PnlStatistics.Controls.Add(Me.LblMean)
        Me.PnlStatistics.Controls.Add(Me.Label6)
        Me.PnlStatistics.Location = New System.Drawing.Point(269, 38)
        Me.PnlStatistics.Name = "PnlStatistics"
        Me.PnlStatistics.Size = New System.Drawing.Size(144, 135)
        Me.PnlStatistics.TabIndex = 61
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(5, 2)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(114, 16)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Layer Statistics"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(22, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(36, 16)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Min:"
        '
        'LblMin
        '
        Me.LblMin.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMin.Location = New System.Drawing.Point(52, 27)
        Me.LblMin.Name = "LblMin"
        Me.LblMin.Size = New System.Drawing.Size(70, 16)
        Me.LblMin.TabIndex = 6
        Me.LblMin.Text = "LblMin"
        Me.LblMin.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(18, 53)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 16)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Max:"
        '
        'LblMax
        '
        Me.LblMax.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMax.Location = New System.Drawing.Point(52, 51)
        Me.LblMax.Name = "LblMax"
        Me.LblMax.Size = New System.Drawing.Size(70, 16)
        Me.LblMax.TabIndex = 9
        Me.LblMax.Text = "LblMax"
        Me.LblMax.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(8, 79)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(50, 16)
        Me.Label5.TabIndex = 10
        Me.Label5.Text = "Mean:"
        '
        'LblSTDV
        '
        Me.LblSTDV.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblSTDV.Location = New System.Drawing.Point(52, 105)
        Me.LblSTDV.Name = "LblSTDV"
        Me.LblSTDV.Size = New System.Drawing.Size(70, 16)
        Me.LblSTDV.TabIndex = 13
        Me.LblSTDV.Text = "LblSTDV"
        Me.LblSTDV.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'LblMean
        '
        Me.LblMean.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMean.Location = New System.Drawing.Point(52, 79)
        Me.LblMean.Name = "LblMean"
        Me.LblMean.Size = New System.Drawing.Size(70, 16)
        Me.LblMean.TabIndex = 11
        Me.LblMean.Text = "LblMean"
        Me.LblMean.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(5, 105)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(53, 16)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "STDV:"
        '
        'TxtTemplatesFile
        '
        Me.TxtTemplatesFile.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtTemplatesFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtTemplatesFile.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtTemplatesFile.Location = New System.Drawing.Point(69, 452)
        Me.TxtTemplatesFile.Name = "TxtTemplatesFile"
        Me.TxtTemplatesFile.ReadOnly = True
        Me.TxtTemplatesFile.Size = New System.Drawing.Size(45, 22)
        Me.TxtTemplatesFile.TabIndex = 81
        Me.TxtTemplatesFile.TabStop = False
        Me.TxtTemplatesFile.Visible = False
        '
        'FrmReclassContinuous
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(457, 485)
        Me.Controls.Add(Me.TxtTemplatesFile)
        Me.Controls.Add(Me.PnlStatistics)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnApply)
        Me.Controls.Add(Me.GrpReclass)
        Me.Controls.Add(Me.TxtRuleId)
        Me.Controls.Add(Me.LstAoiRasterLayers)
        Me.Controls.Add(Me.LblRasterLayers)
        Me.Controls.Add(Me.GrpLayerType)
        Me.Controls.Add(Me.LblToolTip)
        Me.Controls.Add(Me.BtnAbout)
        Me.Name = "FrmReclassContinuous"
        Me.ShowIcon = False
        Me.Text = "Raster Reclass (Continuous data)"
        Me.GrpLayerType.ResumeLayout(False)
        Me.GrpLayerType.PerformLayout()
        Me.GrpReclass.ResumeLayout(False)
        Me.GrpReclass.PerformLayout()
        Me.PnlReclass.ResumeLayout(False)
        Me.PnlReclass.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlStatistics.ResumeLayout(False)
        Me.PnlStatistics.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LblToolTip As System.Windows.Forms.Label
    Friend WithEvents BtnAbout As System.Windows.Forms.Button
    Friend WithEvents GrpLayerType As System.Windows.Forms.GroupBox
    Friend WithEvents RdoPrism As System.Windows.Forms.RadioButton
    Friend WithEvents RdoDem As System.Windows.Forms.RadioButton
    Friend WithEvents RdoRaster As System.Windows.Forms.RadioButton
    Friend WithEvents LstAoiRasterLayers As System.Windows.Forms.ListBox
    Friend WithEvents LblRasterLayers As System.Windows.Forms.Label
    Friend WithEvents TxtRuleId As System.Windows.Forms.TextBox
    Friend WithEvents GrpReclass As System.Windows.Forms.GroupBox
    Friend WithEvents CboReclassField As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents BtnSave As System.Windows.Forms.Button
    Friend WithEvents BtnClear As System.Windows.Forms.Button
    Friend WithEvents BtnLoad As System.Windows.Forms.Button
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents BtnApply As System.Windows.Forms.Button
    Friend WithEvents PnlStatistics As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents LblMin As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents LblMax As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents LblSTDV As System.Windows.Forms.Label
    Friend WithEvents LblMean As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents PnlReclass As System.Windows.Forms.Panel
    Friend WithEvents BtnReclass As System.Windows.Forms.Button
    Friend WithEvents TxtClasses As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents TxtMaxValue As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents TxtMinValue As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents TxtTemplatesFile As System.Windows.Forms.TextBox
    Friend WithEvents From As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ToValues As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NewValue As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
