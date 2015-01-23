<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmReclassZones
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
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.BtnAbout = New System.Windows.Forms.Button()
        Me.LblToolTip = New System.Windows.Forms.Label()
        Me.TxtAoiPath = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.LblOutputHruName = New System.Windows.Forms.Label()
        Me.TxtNewHruName = New System.Windows.Forms.TextBox()
        Me.TxtHruPath = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.CboParentHru = New System.Windows.Forms.ComboBox()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.BtnGenerateHru = New System.Windows.Forms.Button()
        Me.BtnSelectAoi = New System.Windows.Forms.Button()
        Me.LblHruLayers = New System.Windows.Forms.Label()
        Me.BtnViewLayer = New System.Windows.Forms.Button()
        Me.BtnDelete = New System.Windows.Forms.Button()
        Me.TxtSelZone = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BtnAdd = New System.Windows.Forms.Button()
        Me.BtnUpdate = New System.Windows.Forms.Button()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.OldValue = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NewValues = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BtnSelectZones = New System.Windows.Forms.Button()
        Me.CkRetainAttributes = New System.Windows.Forms.CheckBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BtnAbout
        '
        Me.BtnAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAbout.Location = New System.Drawing.Point(456, 8)
        Me.BtnAbout.Name = "BtnAbout"
        Me.BtnAbout.Size = New System.Drawing.Size(105, 28)
        Me.BtnAbout.TabIndex = 62
        Me.BtnAbout.Text = "Tell me more"
        Me.BtnAbout.UseVisualStyleBackColor = True
        '
        'LblToolTip
        '
        Me.LblToolTip.AutoSize = True
        Me.LblToolTip.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblToolTip.Location = New System.Drawing.Point(12, 14)
        Me.LblToolTip.Name = "LblToolTip"
        Me.LblToolTip.Size = New System.Drawing.Size(152, 16)
        Me.LblToolTip.TabIndex = 61
        Me.LblToolTip.Text = "Reclass zone values"
        '
        'TxtAoiPath
        '
        Me.TxtAoiPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtAoiPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtAoiPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtAoiPath.Location = New System.Drawing.Point(192, 42)
        Me.TxtAoiPath.Name = "TxtAoiPath"
        Me.TxtAoiPath.ReadOnly = True
        Me.TxtAoiPath.Size = New System.Drawing.Size(369, 22)
        Me.TxtAoiPath.TabIndex = 60
        Me.TxtAoiPath.TabStop = False
        Me.TxtAoiPath.Text = "AOI is not specified"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(123, 45)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 16)
        Me.Label2.TabIndex = 59
        Me.Label2.Text = "AOI Path:"
        '
        'LblOutputHruName
        '
        Me.LblOutputHruName.AutoSize = True
        Me.LblOutputHruName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblOutputHruName.Location = New System.Drawing.Point(12, 348)
        Me.LblOutputHruName.Name = "LblOutputHruName"
        Me.LblOutputHruName.Size = New System.Drawing.Size(138, 16)
        Me.LblOutputHruName.TabIndex = 58
        Me.LblOutputHruName.Text = "Output HRU Name:"
        '
        'TxtNewHruName
        '
        Me.TxtNewHruName.Enabled = False
        Me.TxtNewHruName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtNewHruName.Location = New System.Drawing.Point(152, 345)
        Me.TxtNewHruName.Name = "TxtNewHruName"
        Me.TxtNewHruName.Size = New System.Drawing.Size(159, 22)
        Me.TxtNewHruName.TabIndex = 52
        '
        'TxtHruPath
        '
        Me.TxtHruPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtHruPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtHruPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtHruPath.Location = New System.Drawing.Point(93, 386)
        Me.TxtHruPath.Name = "TxtHruPath"
        Me.TxtHruPath.ReadOnly = True
        Me.TxtHruPath.Size = New System.Drawing.Size(468, 22)
        Me.TxtHruPath.TabIndex = 54
        Me.TxtHruPath.TabStop = False
        Me.TxtHruPath.Text = "HRU is not specified"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(16, 389)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(71, 16)
        Me.Label3.TabIndex = 57
        Me.Label3.Text = "HRU Path:"
        '
        'CboParentHru
        '
        Me.CboParentHru.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboParentHru.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboParentHru.FormattingEnabled = True
        Me.CboParentHru.Location = New System.Drawing.Point(12, 97)
        Me.CboParentHru.Name = "CboParentHru"
        Me.CboParentHru.Size = New System.Drawing.Size(160, 24)
        Me.CboParentHru.TabIndex = 51
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(290, 417)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(94, 30)
        Me.BtnCancel.TabIndex = 55
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'BtnGenerateHru
        '
        Me.BtnGenerateHru.Enabled = False
        Me.BtnGenerateHru.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnGenerateHru.Location = New System.Drawing.Point(402, 417)
        Me.BtnGenerateHru.Name = "BtnGenerateHru"
        Me.BtnGenerateHru.Size = New System.Drawing.Size(157, 30)
        Me.BtnGenerateHru.TabIndex = 53
        Me.BtnGenerateHru.Text = "Generate HRUs"
        Me.BtnGenerateHru.UseVisualStyleBackColor = True
        '
        'BtnSelectAoi
        '
        Me.BtnSelectAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSelectAoi.Location = New System.Drawing.Point(12, 38)
        Me.BtnSelectAoi.Name = "BtnSelectAoi"
        Me.BtnSelectAoi.Size = New System.Drawing.Size(105, 30)
        Me.BtnSelectAoi.TabIndex = 50
        Me.BtnSelectAoi.Text = "Select AOI"
        Me.BtnSelectAoi.UseVisualStyleBackColor = True
        '
        'LblHruLayers
        '
        Me.LblHruLayers.AutoSize = True
        Me.LblHruLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblHruLayers.Location = New System.Drawing.Point(16, 75)
        Me.LblHruLayers.Name = "LblHruLayers"
        Me.LblHruLayers.Size = New System.Drawing.Size(154, 16)
        Me.LblHruLayers.TabIndex = 56
        Me.LblHruLayers.Text = "Zone layer to reclass"
        '
        'BtnViewLayer
        '
        Me.BtnViewLayer.Enabled = False
        Me.BtnViewLayer.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnViewLayer.Location = New System.Drawing.Point(178, 95)
        Me.BtnViewLayer.Name = "BtnViewLayer"
        Me.BtnViewLayer.Size = New System.Drawing.Size(135, 28)
        Me.BtnViewLayer.TabIndex = 63
        Me.BtnViewLayer.Text = "Add Zone Polygons"
        Me.BtnViewLayer.UseVisualStyleBackColor = True
        '
        'BtnDelete
        '
        Me.BtnDelete.Enabled = False
        Me.BtnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDelete.Location = New System.Drawing.Point(323, 304)
        Me.BtnDelete.Name = "BtnDelete"
        Me.BtnDelete.Size = New System.Drawing.Size(114, 28)
        Me.BtnDelete.TabIndex = 67
        Me.BtnDelete.Text = "Delete Row(s)"
        Me.BtnDelete.UseVisualStyleBackColor = True
        '
        'TxtSelZone
        '
        Me.TxtSelZone.Enabled = False
        Me.TxtSelZone.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtSelZone.Location = New System.Drawing.Point(378, 121)
        Me.TxtSelZone.Name = "TxtSelZone"
        Me.TxtSelZone.Size = New System.Drawing.Size(114, 22)
        Me.TxtSelZone.TabIndex = 68
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(319, 99)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(253, 16)
        Me.Label1.TabIndex = 70
        Me.Label1.Text = "Indentify zone value on mouse-click"
        '
        'BtnAdd
        '
        Me.BtnAdd.Enabled = False
        Me.BtnAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAdd.Location = New System.Drawing.Point(323, 160)
        Me.BtnAdd.Name = "BtnAdd"
        Me.BtnAdd.Size = New System.Drawing.Size(114, 28)
        Me.BtnAdd.TabIndex = 71
        Me.BtnAdd.Text = "Add Value"
        Me.BtnAdd.UseVisualStyleBackColor = True
        '
        'BtnUpdate
        '
        Me.BtnUpdate.Enabled = False
        Me.BtnUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnUpdate.Location = New System.Drawing.Point(323, 225)
        Me.BtnUpdate.Name = "BtnUpdate"
        Me.BtnUpdate.Size = New System.Drawing.Size(114, 28)
        Me.BtnUpdate.TabIndex = 72
        Me.BtnUpdate.Text = "Update Value(s)"
        Me.BtnUpdate.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.OldValue, Me.NewValues})
        Me.DataGridView1.Location = New System.Drawing.Point(12, 135)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(300, 197)
        Me.DataGridView1.TabIndex = 73
        '
        'OldValue
        '
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.OldValue.DefaultCellStyle = DataGridViewCellStyle3
        Me.OldValue.HeaderText = "Old Values"
        Me.OldValue.Name = "OldValue"
        Me.OldValue.ReadOnly = True
        Me.OldValue.Width = 120
        '
        'NewValues
        '
        Me.NewValues.HeaderText = "New Values"
        Me.NewValues.Name = "NewValues"
        Me.NewValues.Width = 120
        '
        'BtnSelectZones
        '
        Me.BtnSelectZones.Enabled = False
        Me.BtnSelectZones.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSelectZones.Image = Global.BAGIS_H.My.Resources.Resources.IdentifyTool16
        Me.BtnSelectZones.Location = New System.Drawing.Point(346, 118)
        Me.BtnSelectZones.Name = "BtnSelectZones"
        Me.BtnSelectZones.Size = New System.Drawing.Size(26, 28)
        Me.BtnSelectZones.TabIndex = 64
        Me.BtnSelectZones.UseVisualStyleBackColor = True
        '
        'CkRetainAttributes
        '
        Me.CkRetainAttributes.AutoSize = True
        Me.CkRetainAttributes.Checked = True
        Me.CkRetainAttributes.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CkRetainAttributes.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CkRetainAttributes.Location = New System.Drawing.Point(315, 346)
        Me.CkRetainAttributes.Name = "CkRetainAttributes"
        Me.CkRetainAttributes.Size = New System.Drawing.Size(191, 20)
        Me.CkRetainAttributes.TabIndex = 74
        Me.CkRetainAttributes.Text = "Retain source attributes"
        Me.CkRetainAttributes.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Blue
        Me.Label4.Location = New System.Drawing.Point(333, 256)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(231, 30)
        Me.Label4.TabIndex = 75
        Me.Label4.Text = "Select New Values cell(s) on the left table" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "before clicking on the Update Value(" & _
    "s)!"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Blue
        Me.Label5.Location = New System.Drawing.Point(333, 191)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(182, 15)
        Me.Label5.TabIndex = 76
        Me.Label5.Text = "Add Old Value to be reclassified."
        '
        'FrmReclassZones
        '
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.CkRetainAttributes)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.BtnUpdate)
        Me.Controls.Add(Me.BtnAdd)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TxtSelZone)
        Me.Controls.Add(Me.BtnDelete)
        Me.Controls.Add(Me.BtnSelectZones)
        Me.Controls.Add(Me.BtnViewLayer)
        Me.Controls.Add(Me.BtnAbout)
        Me.Controls.Add(Me.LblToolTip)
        Me.Controls.Add(Me.TxtAoiPath)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.LblOutputHruName)
        Me.Controls.Add(Me.TxtNewHruName)
        Me.Controls.Add(Me.TxtHruPath)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.CboParentHru)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnGenerateHru)
        Me.Controls.Add(Me.BtnSelectAoi)
        Me.Controls.Add(Me.LblHruLayers)
        Me.Name = "FrmReclassZones"
        Me.Size = New System.Drawing.Size(571, 475)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnAbout As System.Windows.Forms.Button
    Friend WithEvents LblToolTip As System.Windows.Forms.Label
    Friend WithEvents TxtAoiPath As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents LblOutputHruName As System.Windows.Forms.Label
    Friend WithEvents TxtNewHruName As System.Windows.Forms.TextBox
    Friend WithEvents TxtHruPath As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents CboParentHru As System.Windows.Forms.ComboBox
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents BtnGenerateHru As System.Windows.Forms.Button
    Friend WithEvents BtnSelectAoi As System.Windows.Forms.Button
    Friend WithEvents LblHruLayers As System.Windows.Forms.Label
    Friend WithEvents BtnViewLayer As System.Windows.Forms.Button
    Friend WithEvents BtnSelectZones As System.Windows.Forms.Button
    Friend WithEvents BtnDelete As System.Windows.Forms.Button
    Friend WithEvents TxtSelZone As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents BtnAdd As System.Windows.Forms.Button
    Friend WithEvents BtnUpdate As System.Windows.Forms.Button
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents CkRetainAttributes As System.Windows.Forms.CheckBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents OldValue As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NewValues As System.Windows.Forms.DataGridViewTextBoxColumn

End Class