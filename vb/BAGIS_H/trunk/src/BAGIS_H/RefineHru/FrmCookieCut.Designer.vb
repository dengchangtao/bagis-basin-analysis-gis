<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmCookieCut
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
        Me.LblHruLayers = New System.Windows.Forms.Label()
        Me.LstSelectedValues = New System.Windows.Forms.ListBox()
        Me.LblValues = New System.Windows.Forms.Label()
        Me.BtnSelectAoi = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.BtnGenerateHru = New System.Windows.Forms.Button()
        Me.CboParentHru = New System.Windows.Forms.ComboBox()
        Me.CboCookieCut = New System.Windows.Forms.ComboBox()
        Me.LblLayer = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TxtHruPath = New System.Windows.Forms.TextBox()
        Me.LblOutputHruName = New System.Windows.Forms.Label()
        Me.TxtNewHruName = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TxtAoiPath = New System.Windows.Forms.TextBox()
        Me.LblToolTip = New System.Windows.Forms.Label()
        Me.BtnAbout = New System.Windows.Forms.Button()
        Me.CkBoundary = New System.Windows.Forms.CheckBox()
        Me.GrpLayerType = New System.Windows.Forms.GroupBox()
        Me.RdoHru = New System.Windows.Forms.RadioButton()
        Me.RdoRaster = New System.Windows.Forms.RadioButton()
        Me.CboField = New System.Windows.Forms.ComboBox()
        Me.LblField = New System.Windows.Forms.Label()
        Me.CkRetainAttributes = New System.Windows.Forms.CheckBox()
        Me.BtnToggle = New System.Windows.Forms.Button()
        Me.GrpLayerType.SuspendLayout()
        Me.SuspendLayout()
        '
        'LblHruLayers
        '
        Me.LblHruLayers.AutoSize = True
        Me.LblHruLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblHruLayers.Location = New System.Drawing.Point(18, 88)
        Me.LblHruLayers.Name = "LblHruLayers"
        Me.LblHruLayers.Size = New System.Drawing.Size(154, 16)
        Me.LblHruLayers.TabIndex = 9
        Me.LblHruLayers.Text = "Parent template HRU"
        '
        'LstSelectedValues
        '
        Me.LstSelectedValues.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstSelectedValues.FormattingEnabled = True
        Me.LstSelectedValues.IntegralHeight = False
        Me.LstSelectedValues.ItemHeight = 16
        Me.LstSelectedValues.Location = New System.Drawing.Point(488, 106)
        Me.LstSelectedValues.Name = "LstSelectedValues"
        Me.LstSelectedValues.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.LstSelectedValues.Size = New System.Drawing.Size(74, 103)
        Me.LstSelectedValues.TabIndex = 5
        '
        'LblValues
        '
        Me.LblValues.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LblValues.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblValues.Location = New System.Drawing.Point(486, 88)
        Me.LblValues.Name = "LblValues"
        Me.LblValues.Size = New System.Drawing.Size(72, 16)
        Me.LblValues.TabIndex = 16
        Me.LblValues.Text = "Value(s)"
        Me.LblValues.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'BtnSelectAoi
        '
        Me.BtnSelectAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSelectAoi.Location = New System.Drawing.Point(14, 40)
        Me.BtnSelectAoi.Name = "BtnSelectAoi"
        Me.BtnSelectAoi.Size = New System.Drawing.Size(105, 30)
        Me.BtnSelectAoi.TabIndex = 1
        Me.BtnSelectAoi.Text = "Select AOI"
        Me.BtnSelectAoi.UseVisualStyleBackColor = True
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(292, 279)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(94, 30)
        Me.BtnCancel.TabIndex = 7
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'BtnGenerateHru
        '
        Me.BtnGenerateHru.Enabled = False
        Me.BtnGenerateHru.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnGenerateHru.Location = New System.Drawing.Point(404, 279)
        Me.BtnGenerateHru.Name = "BtnGenerateHru"
        Me.BtnGenerateHru.Size = New System.Drawing.Size(157, 30)
        Me.BtnGenerateHru.TabIndex = 6
        Me.BtnGenerateHru.Text = "Generate HRUs"
        Me.BtnGenerateHru.UseVisualStyleBackColor = True
        '
        'CboParentHru
        '
        Me.CboParentHru.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboParentHru.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboParentHru.FormattingEnabled = True
        Me.CboParentHru.Location = New System.Drawing.Point(26, 110)
        Me.CboParentHru.Name = "CboParentHru"
        Me.CboParentHru.Size = New System.Drawing.Size(160, 24)
        Me.CboParentHru.TabIndex = 2
        '
        'CboCookieCut
        '
        Me.CboCookieCut.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboCookieCut.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboCookieCut.FormattingEnabled = True
        Me.CboCookieCut.Location = New System.Drawing.Point(319, 110)
        Me.CboCookieCut.Name = "CboCookieCut"
        Me.CboCookieCut.Size = New System.Drawing.Size(160, 24)
        Me.CboCookieCut.TabIndex = 4
        '
        'LblLayer
        '
        Me.LblLayer.AutoSize = True
        Me.LblLayer.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLayer.Location = New System.Drawing.Point(315, 88)
        Me.LblLayer.Name = "LblLayer"
        Me.LblLayer.Size = New System.Drawing.Size(68, 16)
        Me.LblLayer.TabIndex = 40
        Me.LblLayer.Text = "LblLayer"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(14, 251)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(71, 16)
        Me.Label3.TabIndex = 41
        Me.Label3.Text = "HRU Path:"
        '
        'TxtHruPath
        '
        Me.TxtHruPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtHruPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtHruPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtHruPath.Location = New System.Drawing.Point(84, 248)
        Me.TxtHruPath.Name = "TxtHruPath"
        Me.TxtHruPath.ReadOnly = True
        Me.TxtHruPath.Size = New System.Drawing.Size(479, 22)
        Me.TxtHruPath.TabIndex = 7
        Me.TxtHruPath.TabStop = False
        Me.TxtHruPath.Text = "HRU is not specified"
        '
        'LblOutputHruName
        '
        Me.LblOutputHruName.AutoSize = True
        Me.LblOutputHruName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblOutputHruName.Location = New System.Drawing.Point(14, 192)
        Me.LblOutputHruName.Name = "LblOutputHruName"
        Me.LblOutputHruName.Size = New System.Drawing.Size(138, 16)
        Me.LblOutputHruName.TabIndex = 44
        Me.LblOutputHruName.Text = "Output HRU Name:"
        '
        'TxtNewHruName
        '
        Me.TxtNewHruName.Enabled = False
        Me.TxtNewHruName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtNewHruName.Location = New System.Drawing.Point(154, 189)
        Me.TxtNewHruName.Name = "TxtNewHruName"
        Me.TxtNewHruName.Size = New System.Drawing.Size(159, 22)
        Me.TxtNewHruName.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(125, 47)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 16)
        Me.Label2.TabIndex = 45
        Me.Label2.Text = "AOI Path:"
        '
        'TxtAoiPath
        '
        Me.TxtAoiPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtAoiPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtAoiPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtAoiPath.Location = New System.Drawing.Point(194, 44)
        Me.TxtAoiPath.Name = "TxtAoiPath"
        Me.TxtAoiPath.ReadOnly = True
        Me.TxtAoiPath.Size = New System.Drawing.Size(369, 22)
        Me.TxtAoiPath.TabIndex = 47
        Me.TxtAoiPath.TabStop = False
        Me.TxtAoiPath.Text = "AOI is not specified"
        '
        'LblToolTip
        '
        Me.LblToolTip.AutoSize = True
        Me.LblToolTip.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblToolTip.Location = New System.Drawing.Point(14, 12)
        Me.LblToolTip.Name = "LblToolTip"
        Me.LblToolTip.Size = New System.Drawing.Size(61, 16)
        Me.LblToolTip.TabIndex = 48
        Me.LblToolTip.Text = "Tool tip"
        '
        'BtnAbout
        '
        Me.BtnAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAbout.Location = New System.Drawing.Point(458, 6)
        Me.BtnAbout.Name = "BtnAbout"
        Me.BtnAbout.Size = New System.Drawing.Size(105, 28)
        Me.BtnAbout.TabIndex = 49
        Me.BtnAbout.Text = "Tell me more"
        Me.BtnAbout.UseVisualStyleBackColor = True
        '
        'CkBoundary
        '
        Me.CkBoundary.AutoSize = True
        Me.CkBoundary.Checked = True
        Me.CkBoundary.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CkBoundary.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CkBoundary.Location = New System.Drawing.Point(18, 147)
        Me.CkBoundary.Name = "CkBoundary"
        Me.CkBoundary.Size = New System.Drawing.Size(188, 20)
        Me.CkBoundary.TabIndex = 3
        Me.CkBoundary.Text = "Preserve AOI boundary"
        Me.CkBoundary.UseVisualStyleBackColor = True
        Me.CkBoundary.Visible = False
        '
        'GrpLayerType
        '
        Me.GrpLayerType.Controls.Add(Me.RdoHru)
        Me.GrpLayerType.Controls.Add(Me.RdoRaster)
        Me.GrpLayerType.Enabled = False
        Me.GrpLayerType.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GrpLayerType.Location = New System.Drawing.Point(227, 88)
        Me.GrpLayerType.Name = "GrpLayerType"
        Me.GrpLayerType.Size = New System.Drawing.Size(86, 65)
        Me.GrpLayerType.TabIndex = 50
        Me.GrpLayerType.TabStop = False
        Me.GrpLayerType.Text = "Layer type"
        '
        'RdoHru
        '
        Me.RdoHru.AutoSize = True
        Me.RdoHru.Location = New System.Drawing.Point(10, 20)
        Me.RdoHru.Name = "RdoHru"
        Me.RdoHru.Size = New System.Drawing.Size(56, 20)
        Me.RdoHru.TabIndex = 3
        Me.RdoHru.TabStop = True
        Me.RdoHru.Text = "HRU"
        Me.RdoHru.UseVisualStyleBackColor = True
        '
        'RdoRaster
        '
        Me.RdoRaster.AutoSize = True
        Me.RdoRaster.Location = New System.Drawing.Point(10, 41)
        Me.RdoRaster.Name = "RdoRaster"
        Me.RdoRaster.Size = New System.Drawing.Size(66, 20)
        Me.RdoRaster.TabIndex = 0
        Me.RdoRaster.TabStop = True
        Me.RdoRaster.Text = "Raster"
        Me.RdoRaster.UseVisualStyleBackColor = True
        '
        'CboField
        '
        Me.CboField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboField.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboField.FormattingEnabled = True
        Me.CboField.Location = New System.Drawing.Point(319, 158)
        Me.CboField.Name = "CboField"
        Me.CboField.Size = New System.Drawing.Size(160, 24)
        Me.CboField.TabIndex = 51
        '
        'LblField
        '
        Me.LblField.AutoSize = True
        Me.LblField.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblField.Location = New System.Drawing.Point(315, 139)
        Me.LblField.Name = "LblField"
        Me.LblField.Size = New System.Drawing.Size(64, 16)
        Me.LblField.TabIndex = 52
        Me.LblField.Text = "LblField"
        '
        'CkRetainAttributes
        '
        Me.CkRetainAttributes.AutoSize = True
        Me.CkRetainAttributes.Checked = True
        Me.CkRetainAttributes.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CkRetainAttributes.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CkRetainAttributes.Location = New System.Drawing.Point(18, 166)
        Me.CkRetainAttributes.Name = "CkRetainAttributes"
        Me.CkRetainAttributes.Size = New System.Drawing.Size(191, 20)
        Me.CkRetainAttributes.TabIndex = 53
        Me.CkRetainAttributes.Text = "Retain source attributes"
        Me.CkRetainAttributes.UseVisualStyleBackColor = True
        Me.CkRetainAttributes.Visible = False
        '
        'BtnToggle
        '
        Me.BtnToggle.Enabled = False
        Me.BtnToggle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnToggle.Location = New System.Drawing.Point(444, 215)
        Me.BtnToggle.Name = "BtnToggle"
        Me.BtnToggle.Size = New System.Drawing.Size(119, 28)
        Me.BtnToggle.TabIndex = 58
        Me.BtnToggle.Text = "Toggle Selection"
        Me.BtnToggle.UseVisualStyleBackColor = True
        '
        'FrmCookieCut
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(575, 318)
        Me.Controls.Add(Me.BtnToggle)
        Me.Controls.Add(Me.CkRetainAttributes)
        Me.Controls.Add(Me.LblField)
        Me.Controls.Add(Me.CboField)
        Me.Controls.Add(Me.GrpLayerType)
        Me.Controls.Add(Me.CkBoundary)
        Me.Controls.Add(Me.BtnAbout)
        Me.Controls.Add(Me.LblToolTip)
        Me.Controls.Add(Me.TxtAoiPath)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.LblOutputHruName)
        Me.Controls.Add(Me.TxtNewHruName)
        Me.Controls.Add(Me.TxtHruPath)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.LblLayer)
        Me.Controls.Add(Me.CboCookieCut)
        Me.Controls.Add(Me.CboParentHru)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnGenerateHru)
        Me.Controls.Add(Me.BtnSelectAoi)
        Me.Controls.Add(Me.LblValues)
        Me.Controls.Add(Me.LstSelectedValues)
        Me.Controls.Add(Me.LblHruLayers)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "FrmCookieCut"
        Me.ShowIcon = False
        Me.Text = "Current AOI: (Not selected)"
        Me.GrpLayerType.ResumeLayout(False)
        Me.GrpLayerType.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LblHruLayers As System.Windows.Forms.Label
    Friend WithEvents LstSelectedValues As System.Windows.Forms.ListBox
    Friend WithEvents LblValues As System.Windows.Forms.Label
    Friend WithEvents BtnSelectAoi As System.Windows.Forms.Button
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents BtnGenerateHru As System.Windows.Forms.Button
    Friend WithEvents CboParentHru As System.Windows.Forms.ComboBox
    Friend WithEvents CboCookieCut As System.Windows.Forms.ComboBox
    Friend WithEvents LblLayer As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TxtHruPath As System.Windows.Forms.TextBox
    Friend WithEvents LblOutputHruName As System.Windows.Forms.Label
    Friend WithEvents TxtNewHruName As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TxtAoiPath As System.Windows.Forms.TextBox
    Friend WithEvents LblToolTip As System.Windows.Forms.Label
    Friend WithEvents BtnAbout As System.Windows.Forms.Button
    Friend WithEvents CkBoundary As System.Windows.Forms.CheckBox
    Friend WithEvents GrpLayerType As System.Windows.Forms.GroupBox
    Friend WithEvents RdoHru As System.Windows.Forms.RadioButton
    Friend WithEvents RdoRaster As System.Windows.Forms.RadioButton
    Friend WithEvents CboField As System.Windows.Forms.ComboBox
    Friend WithEvents LblField As System.Windows.Forms.Label
    Friend WithEvents CkRetainAttributes As System.Windows.Forms.CheckBox
    Friend WithEvents BtnToggle As System.Windows.Forms.Button
End Class