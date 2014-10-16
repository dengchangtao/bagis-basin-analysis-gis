<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmTimberlineTool
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.PnlTimberline = New System.Windows.Forms.Panel()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.RdoFeet = New System.Windows.Forms.RadioButton()
        Me.RdoMeters = New System.Windows.Forms.RadioButton()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.TxtElevMean = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.TxtElevMax = New System.Windows.Forms.TextBox()
        Me.TxtElevMin = New System.Windows.Forms.TextBox()
        Me.TxtAoiPath = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.BtnSelectAoi = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CboParentHru = New System.Windows.Forms.ComboBox()
        Me.BtnViewHru = New System.Windows.Forms.Button()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.HruId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TimberlineElevation = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TxtElev = New System.Windows.Forms.TextBox()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.BtnSave = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TxtHruId = New System.Windows.Forms.TextBox()
        Me.BtnAbout = New System.Windows.Forms.Button()
        Me.BtnIdentify = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Pnl1 = New System.Windows.Forms.Panel()
        Me.BtnUpdate = New System.Windows.Forms.Button()
        Me.TxtSelElev = New System.Windows.Forms.TextBox()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.TxtRdoMean = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.BtnClearSelection = New System.Windows.Forms.Button()
        Me.RdoSelect = New System.Windows.Forms.RadioButton()
        Me.RdoMouseClick = New System.Windows.Forms.RadioButton()
        Me.BtnCustom = New System.Windows.Forms.Button()
        Me.BtnSatellite = New System.Windows.Forms.Button()
        Me.PnlTimberline.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Pnl1.SuspendLayout()
        Me.SuspendLayout()
        '
        'PnlTimberline
        '
        Me.PnlTimberline.Controls.Add(Me.TextBox2)
        Me.PnlTimberline.Controls.Add(Me.RdoFeet)
        Me.PnlTimberline.Controls.Add(Me.RdoMeters)
        Me.PnlTimberline.Controls.Add(Me.TextBox1)
        Me.PnlTimberline.Controls.Add(Me.Label9)
        Me.PnlTimberline.Controls.Add(Me.Label10)
        Me.PnlTimberline.Controls.Add(Me.TxtElevMean)
        Me.PnlTimberline.Controls.Add(Me.Label11)
        Me.PnlTimberline.Controls.Add(Me.TxtElevMax)
        Me.PnlTimberline.Controls.Add(Me.TxtElevMin)
        Me.PnlTimberline.Location = New System.Drawing.Point(1, 104)
        Me.PnlTimberline.Name = "PnlTimberline"
        Me.PnlTimberline.Size = New System.Drawing.Size(527, 79)
        Me.PnlTimberline.TabIndex = 88
        '
        'TextBox2
        '
        Me.TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(4, 4)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(182, 15)
        Me.TextBox2.TabIndex = 121
        Me.TextBox2.Text = "Elevation for selected AOI"
        '
        'RdoFeet
        '
        Me.RdoFeet.AutoSize = True
        Me.RdoFeet.Enabled = False
        Me.RdoFeet.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RdoFeet.Location = New System.Drawing.Point(183, 26)
        Me.RdoFeet.Name = "RdoFeet"
        Me.RdoFeet.Size = New System.Drawing.Size(53, 20)
        Me.RdoFeet.TabIndex = 120
        Me.RdoFeet.Text = "Feet"
        Me.RdoFeet.UseVisualStyleBackColor = True
        '
        'RdoMeters
        '
        Me.RdoMeters.AutoSize = True
        Me.RdoMeters.Enabled = False
        Me.RdoMeters.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RdoMeters.Location = New System.Drawing.Point(117, 26)
        Me.RdoMeters.Name = "RdoMeters"
        Me.RdoMeters.Size = New System.Drawing.Size(67, 20)
        Me.RdoMeters.TabIndex = 119
        Me.RdoMeters.Text = "Meters"
        Me.RdoMeters.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(4, 28)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(117, 15)
        Me.TextBox1.TabIndex = 94
        Me.TextBox1.Text = "Elevation Units:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(3, 53)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(64, 16)
        Me.Label9.TabIndex = 89
        Me.Label9.Text = "Minimum:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(174, 53)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(68, 16)
        Me.Label10.TabIndex = 90
        Me.Label10.Text = "Maximum:"
        '
        'TxtElevMean
        '
        Me.TxtElevMean.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtElevMean.Location = New System.Drawing.Point(404, 50)
        Me.TxtElevMean.Name = "TxtElevMean"
        Me.TxtElevMean.ReadOnly = True
        Me.TxtElevMean.Size = New System.Drawing.Size(100, 22)
        Me.TxtElevMean.TabIndex = 93
        Me.TxtElevMean.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(359, 53)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(45, 16)
        Me.Label11.TabIndex = 91
        Me.Label11.Text = "Mean:"
        '
        'TxtElevMax
        '
        Me.TxtElevMax.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtElevMax.Location = New System.Drawing.Point(247, 50)
        Me.TxtElevMax.Name = "TxtElevMax"
        Me.TxtElevMax.ReadOnly = True
        Me.TxtElevMax.Size = New System.Drawing.Size(100, 22)
        Me.TxtElevMax.TabIndex = 92
        Me.TxtElevMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TxtElevMin
        '
        Me.TxtElevMin.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtElevMin.Location = New System.Drawing.Point(66, 50)
        Me.TxtElevMin.Name = "TxtElevMin"
        Me.TxtElevMin.ReadOnly = True
        Me.TxtElevMin.Size = New System.Drawing.Size(100, 22)
        Me.TxtElevMin.TabIndex = 87
        Me.TxtElevMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TxtAoiPath
        '
        Me.TxtAoiPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtAoiPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtAoiPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtAoiPath.Location = New System.Drawing.Point(183, 44)
        Me.TxtAoiPath.Name = "TxtAoiPath"
        Me.TxtAoiPath.ReadOnly = True
        Me.TxtAoiPath.Size = New System.Drawing.Size(369, 22)
        Me.TxtAoiPath.TabIndex = 91
        Me.TxtAoiPath.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(114, 47)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 16)
        Me.Label2.TabIndex = 90
        Me.Label2.Text = "AOI Path:"
        '
        'BtnSelectAoi
        '
        Me.BtnSelectAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSelectAoi.Location = New System.Drawing.Point(3, 40)
        Me.BtnSelectAoi.Name = "BtnSelectAoi"
        Me.BtnSelectAoi.Size = New System.Drawing.Size(105, 30)
        Me.BtnSelectAoi.TabIndex = 89
        Me.BtnSelectAoi.Text = "Select AOI"
        Me.BtnSelectAoi.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(3, 80)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(137, 16)
        Me.Label1.TabIndex = 97
        Me.Label1.Text = "HRU Layers in AOI"
        '
        'CboParentHru
        '
        Me.CboParentHru.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboParentHru.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboParentHru.FormattingEnabled = True
        Me.CboParentHru.Location = New System.Drawing.Point(144, 75)
        Me.CboParentHru.Name = "CboParentHru"
        Me.CboParentHru.Size = New System.Drawing.Size(275, 24)
        Me.CboParentHru.TabIndex = 98
        '
        'BtnViewHru
        '
        Me.BtnViewHru.Enabled = False
        Me.BtnViewHru.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnViewHru.Location = New System.Drawing.Point(429, 73)
        Me.BtnViewHru.Name = "BtnViewHru"
        Me.BtnViewHru.Size = New System.Drawing.Size(115, 28)
        Me.BtnViewHru.TabIndex = 99
        Me.BtnViewHru.Text = "View HRU Layer"
        Me.BtnViewHru.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.HruId, Me.TimberlineElevation})
        Me.DataGridView1.Location = New System.Drawing.Point(3, 245)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(321, 197)
        Me.DataGridView1.TabIndex = 103
        '
        'HruId
        '
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.HruId.DefaultCellStyle = DataGridViewCellStyle2
        Me.HruId.HeaderText = "HRU ID"
        Me.HruId.Name = "HruId"
        Me.HruId.ReadOnly = True
        '
        'TimberlineElevation
        '
        Me.TimberlineElevation.HeaderText = "Timberline Elevation"
        Me.TimberlineElevation.Name = "TimberlineElevation"
        Me.TimberlineElevation.ReadOnly = True
        Me.TimberlineElevation.Width = 150
        '
        'TxtElev
        '
        Me.TxtElev.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtElev.Location = New System.Drawing.Point(253, 25)
        Me.TxtElev.Name = "TxtElev"
        Me.TxtElev.ReadOnly = True
        Me.TxtElev.Size = New System.Drawing.Size(61, 22)
        Me.TxtElev.TabIndex = 105
        '
        'BtnClose
        '
        Me.BtnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClose.Location = New System.Drawing.Point(649, 417)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(65, 25)
        Me.BtnClose.TabIndex = 109
        Me.BtnClose.Text = "Close"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'BtnSave
        '
        Me.BtnSave.Enabled = False
        Me.BtnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSave.Location = New System.Drawing.Point(578, 417)
        Me.BtnSave.Name = "BtnSave"
        Me.BtnSave.Size = New System.Drawing.Size(65, 25)
        Me.BtnSave.TabIndex = 110
        Me.BtnSave.Text = "Save"
        Me.BtnSave.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(2, 189)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(209, 16)
        Me.Label5.TabIndex = 96
        Me.Label5.Text = "Timberline Reference Layers"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(188, 28)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(67, 16)
        Me.Label6.TabIndex = 96
        Me.Label6.Text = "Elevation:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(61, 28)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(57, 16)
        Me.Label7.TabIndex = 111
        Me.Label7.Text = "HRU ID:"
        '
        'TxtHruId
        '
        Me.TxtHruId.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtHruId.Location = New System.Drawing.Point(115, 25)
        Me.TxtHruId.Name = "TxtHruId"
        Me.TxtHruId.ReadOnly = True
        Me.TxtHruId.Size = New System.Drawing.Size(61, 22)
        Me.TxtHruId.TabIndex = 112
        '
        'BtnAbout
        '
        Me.BtnAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAbout.Location = New System.Drawing.Point(345, 5)
        Me.BtnAbout.Name = "BtnAbout"
        Me.BtnAbout.Size = New System.Drawing.Size(105, 28)
        Me.BtnAbout.TabIndex = 113
        Me.BtnAbout.Text = "Tell me more"
        Me.BtnAbout.UseVisualStyleBackColor = True
        '
        'BtnIdentify
        '
        Me.BtnIdentify.Enabled = False
        Me.BtnIdentify.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnIdentify.Image = Global.BAGIS_P.My.Resources.Resources.IdentifyTool16
        Me.BtnIdentify.Location = New System.Drawing.Point(32, 22)
        Me.BtnIdentify.Name = "BtnIdentify"
        Me.BtnIdentify.Size = New System.Drawing.Size(26, 28)
        Me.BtnIdentify.TabIndex = 104
        Me.BtnIdentify.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(2, 9)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(310, 16)
        Me.Label8.TabIndex = 114
        Me.Label8.Text = "Specify a timberline elevation for each HRU"
        '
        'Pnl1
        '
        Me.Pnl1.Controls.Add(Me.BtnUpdate)
        Me.Pnl1.Controls.Add(Me.TxtSelElev)
        Me.Pnl1.Controls.Add(Me.TextBox4)
        Me.Pnl1.Controls.Add(Me.TxtRdoMean)
        Me.Pnl1.Controls.Add(Me.TextBox3)
        Me.Pnl1.Controls.Add(Me.BtnClearSelection)
        Me.Pnl1.Controls.Add(Me.RdoSelect)
        Me.Pnl1.Controls.Add(Me.RdoMouseClick)
        Me.Pnl1.Controls.Add(Me.BtnIdentify)
        Me.Pnl1.Controls.Add(Me.TxtHruId)
        Me.Pnl1.Controls.Add(Me.TxtElev)
        Me.Pnl1.Controls.Add(Me.Label7)
        Me.Pnl1.Controls.Add(Me.Label6)
        Me.Pnl1.Location = New System.Drawing.Point(330, 246)
        Me.Pnl1.Name = "Pnl1"
        Me.Pnl1.Size = New System.Drawing.Size(390, 143)
        Me.Pnl1.TabIndex = 115
        '
        'BtnUpdate
        '
        Me.BtnUpdate.Enabled = False
        Me.BtnUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnUpdate.Location = New System.Drawing.Point(293, 49)
        Me.BtnUpdate.Name = "BtnUpdate"
        Me.BtnUpdate.Size = New System.Drawing.Size(65, 25)
        Me.BtnUpdate.TabIndex = 118
        Me.BtnUpdate.Text = "Update"
        Me.BtnUpdate.UseVisualStyleBackColor = True
        '
        'TxtSelElev
        '
        Me.TxtSelElev.Enabled = False
        Me.TxtSelElev.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtSelElev.Location = New System.Drawing.Point(224, 50)
        Me.TxtSelElev.Name = "TxtSelElev"
        Me.TxtSelElev.Size = New System.Drawing.Size(61, 22)
        Me.TxtSelElev.TabIndex = 125
        Me.TxtSelElev.Text = "0"
        '
        'TextBox4
        '
        Me.TextBox4.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox4.Location = New System.Drawing.Point(27, 74)
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.ReadOnly = True
        Me.TextBox4.Size = New System.Drawing.Size(360, 15)
        Me.TextBox4.TabIndex = 124
        Me.TextBox4.Text = "Note: an elevation of 0 indicates the HRU is below timberline"
        '
        'TxtRdoMean
        '
        Me.TxtRdoMean.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtRdoMean.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtRdoMean.Location = New System.Drawing.Point(27, 53)
        Me.TxtRdoMean.Name = "TxtRdoMean"
        Me.TxtRdoMean.ReadOnly = True
        Me.TxtRdoMean.Size = New System.Drawing.Size(311, 15)
        Me.TxtRdoMean.TabIndex = 123
        Me.TxtRdoMean.Text = "Set selected HRU values to:"
        '
        'TextBox3
        '
        Me.TextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox3.Location = New System.Drawing.Point(27, 3)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ReadOnly = True
        Me.TextBox3.Size = New System.Drawing.Size(287, 15)
        Me.TextBox3.TabIndex = 122
        Me.TextBox3.Text = "Update elevation value on mouse-click"
        '
        'BtnClearSelection
        '
        Me.BtnClearSelection.Enabled = False
        Me.BtnClearSelection.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClearSelection.Location = New System.Drawing.Point(7, 100)
        Me.BtnClearSelection.Name = "BtnClearSelection"
        Me.BtnClearSelection.Size = New System.Drawing.Size(110, 25)
        Me.BtnClearSelection.TabIndex = 118
        Me.BtnClearSelection.Text = "Clear Selection"
        Me.BtnClearSelection.UseVisualStyleBackColor = True
        '
        'RdoSelect
        '
        Me.RdoSelect.AutoSize = True
        Me.RdoSelect.Enabled = False
        Me.RdoSelect.Location = New System.Drawing.Point(7, 53)
        Me.RdoSelect.Name = "RdoSelect"
        Me.RdoSelect.Size = New System.Drawing.Size(14, 13)
        Me.RdoSelect.TabIndex = 1
        Me.RdoSelect.UseVisualStyleBackColor = True
        '
        'RdoMouseClick
        '
        Me.RdoMouseClick.AutoSize = True
        Me.RdoMouseClick.Enabled = False
        Me.RdoMouseClick.Location = New System.Drawing.Point(7, 3)
        Me.RdoMouseClick.Name = "RdoMouseClick"
        Me.RdoMouseClick.Size = New System.Drawing.Size(14, 13)
        Me.RdoMouseClick.TabIndex = 0
        Me.RdoMouseClick.UseVisualStyleBackColor = True
        '
        'BtnCustom
        '
        Me.BtnCustom.Enabled = False
        Me.BtnCustom.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCustom.Location = New System.Drawing.Point(179, 208)
        Me.BtnCustom.Name = "BtnCustom"
        Me.BtnCustom.Size = New System.Drawing.Size(169, 28)
        Me.BtnCustom.TabIndex = 116
        Me.BtnCustom.Text = "Custom Reference Layer"
        Me.BtnCustom.UseVisualStyleBackColor = True
        '
        'BtnSatellite
        '
        Me.BtnSatellite.Enabled = False
        Me.BtnSatellite.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSatellite.Location = New System.Drawing.Point(3, 208)
        Me.BtnSatellite.Name = "BtnSatellite"
        Me.BtnSatellite.Size = New System.Drawing.Size(169, 28)
        Me.BtnSatellite.TabIndex = 117
        Me.BtnSatellite.Text = "Satellite Landcover"
        Me.BtnSatellite.UseVisualStyleBackColor = True
        '
        'FrmTimberlineTool
        '
        Me.Controls.Add(Me.BtnSatellite)
        Me.Controls.Add(Me.BtnCustom)
        Me.Controls.Add(Me.Pnl1)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.BtnAbout)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.BtnSave)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.BtnViewHru)
        Me.Controls.Add(Me.CboParentHru)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TxtAoiPath)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.BtnSelectAoi)
        Me.Controls.Add(Me.PnlTimberline)
        Me.Name = "FrmTimberlineTool"
        Me.Size = New System.Drawing.Size(730, 456)
        Me.PnlTimberline.ResumeLayout(False)
        Me.PnlTimberline.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Pnl1.ResumeLayout(False)
        Me.Pnl1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PnlTimberline As System.Windows.Forms.Panel
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents TxtElevMean As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents TxtElevMax As System.Windows.Forms.TextBox
    Friend WithEvents TxtElevMin As System.Windows.Forms.TextBox
    Friend WithEvents TxtAoiPath As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents BtnSelectAoi As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CboParentHru As System.Windows.Forms.ComboBox
    Friend WithEvents BtnViewHru As System.Windows.Forms.Button
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents TxtElev As System.Windows.Forms.TextBox
    Friend WithEvents BtnIdentify As System.Windows.Forms.Button
    Friend WithEvents BtnClose As System.Windows.Forms.Button
    Friend WithEvents BtnSave As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TxtHruId As System.Windows.Forms.TextBox
    Friend WithEvents BtnAbout As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Pnl1 As System.Windows.Forms.Panel
    Friend WithEvents RdoSelect As System.Windows.Forms.RadioButton
    Friend WithEvents RdoMouseClick As System.Windows.Forms.RadioButton
    Friend WithEvents BtnCustom As System.Windows.Forms.Button
    Friend WithEvents BtnSatellite As System.Windows.Forms.Button
    Friend WithEvents BtnClearSelection As System.Windows.Forms.Button
    Friend WithEvents RdoFeet As System.Windows.Forms.RadioButton
    Friend WithEvents RdoMeters As System.Windows.Forms.RadioButton
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TxtRdoMean As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents TxtSelElev As System.Windows.Forms.TextBox
    Friend WithEvents HruId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TimberlineElevation As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents BtnUpdate As System.Windows.Forms.Button

End Class
