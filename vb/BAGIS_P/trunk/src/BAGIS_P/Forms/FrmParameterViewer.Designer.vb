<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmParameterViewer
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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.TxtAoiPath = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.BtnSelectAoi = New System.Windows.Forms.Button()
        Me.LstHruLayers = New System.Windows.Forms.ListBox()
        Me.LblHruLayers = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LstProfiles = New System.Windows.Forms.ListBox()
        Me.GrdParam = New System.Windows.Forms.DataGridView()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.BtnEditModel = New System.Windows.Forms.Button()
        Me.BtnViewValues = New System.Windows.Forms.Button()
        Me.CkDisplayLabels = New System.Windows.Forms.CheckBox()
        Me.ImgListColors = New System.Windows.Forms.ImageList(Me.components)
        Me.CboColors = New System.Windows.Forms.ComboBox()
        Me.LblError = New System.Windows.Forms.LinkLabel()
        Me.HRU_ID = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.GrdParam, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TxtAoiPath
        '
        Me.TxtAoiPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtAoiPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtAoiPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtAoiPath.Location = New System.Drawing.Point(183, 9)
        Me.TxtAoiPath.Name = "TxtAoiPath"
        Me.TxtAoiPath.ReadOnly = True
        Me.TxtAoiPath.Size = New System.Drawing.Size(603, 22)
        Me.TxtAoiPath.TabIndex = 59
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(114, 12)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(63, 16)
        Me.Label5.TabIndex = 58
        Me.Label5.Text = "AOI Path:"
        '
        'BtnSelectAoi
        '
        Me.BtnSelectAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSelectAoi.Location = New System.Drawing.Point(3, 6)
        Me.BtnSelectAoi.Name = "BtnSelectAoi"
        Me.BtnSelectAoi.Size = New System.Drawing.Size(105, 28)
        Me.BtnSelectAoi.TabIndex = 57
        Me.BtnSelectAoi.Text = "Select AOI"
        Me.BtnSelectAoi.UseVisualStyleBackColor = True
        '
        'LstHruLayers
        '
        Me.LstHruLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstHruLayers.FormattingEnabled = True
        Me.LstHruLayers.ItemHeight = 16
        Me.LstHruLayers.Location = New System.Drawing.Point(3, 65)
        Me.LstHruLayers.Name = "LstHruLayers"
        Me.LstHruLayers.Size = New System.Drawing.Size(150, 100)
        Me.LstHruLayers.TabIndex = 60
        '
        'LblHruLayers
        '
        Me.LblHruLayers.AutoSize = True
        Me.LblHruLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblHruLayers.Location = New System.Drawing.Point(0, 45)
        Me.LblHruLayers.Name = "LblHruLayers"
        Me.LblHruLayers.Size = New System.Drawing.Size(137, 16)
        Me.LblHruLayers.TabIndex = 61
        Me.LblHruLayers.Text = "HRU Layers in AOI"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(0, 170)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 16)
        Me.Label1.TabIndex = 62
        Me.Label1.Text = "Profiles"
        '
        'LstProfiles
        '
        Me.LstProfiles.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstProfiles.FormattingEnabled = True
        Me.LstProfiles.ItemHeight = 16
        Me.LstProfiles.Location = New System.Drawing.Point(3, 189)
        Me.LstProfiles.Name = "LstProfiles"
        Me.LstProfiles.Size = New System.Drawing.Size(150, 100)
        Me.LstProfiles.Sorted = True
        Me.LstProfiles.TabIndex = 63
        '
        'GrdParam
        '
        Me.GrdParam.AllowUserToAddRows = False
        Me.GrdParam.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.GrdParam.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.GrdParam.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GrdParam.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.HRU_ID})
        Me.GrdParam.Location = New System.Drawing.Point(168, 45)
        Me.GrdParam.MultiSelect = False
        Me.GrdParam.Name = "GrdParam"
        Me.GrdParam.ReadOnly = True
        Me.GrdParam.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullColumnSelect
        Me.GrdParam.Size = New System.Drawing.Size(618, 244)
        Me.GrdParam.TabIndex = 64
        Me.GrdParam.Visible = False
        '
        'BtnClose
        '
        Me.BtnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClose.Location = New System.Drawing.Point(721, 295)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(65, 25)
        Me.BtnClose.TabIndex = 80
        Me.BtnClose.Text = "Close"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'BtnEditModel
        '
        Me.BtnEditModel.Enabled = False
        Me.BtnEditModel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnEditModel.Location = New System.Drawing.Point(542, 295)
        Me.BtnEditModel.Name = "BtnEditModel"
        Me.BtnEditModel.Size = New System.Drawing.Size(81, 25)
        Me.BtnEditModel.TabIndex = 81
        Me.BtnEditModel.Text = "Edit Model"
        Me.BtnEditModel.UseVisualStyleBackColor = True
        '
        'BtnViewValues
        '
        Me.BtnViewValues.Enabled = False
        Me.BtnViewValues.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnViewValues.Location = New System.Drawing.Point(386, 295)
        Me.BtnViewValues.Name = "BtnViewValues"
        Me.BtnViewValues.Size = New System.Drawing.Size(150, 25)
        Me.BtnViewValues.TabIndex = 83
        Me.BtnViewValues.Text = "View Value(s) On Map"
        Me.BtnViewValues.UseVisualStyleBackColor = True
        '
        'CkDisplayLabels
        '
        Me.CkDisplayLabels.AutoSize = True
        Me.CkDisplayLabels.Checked = True
        Me.CkDisplayLabels.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CkDisplayLabels.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CkDisplayLabels.Location = New System.Drawing.Point(168, 298)
        Me.CkDisplayLabels.Name = "CkDisplayLabels"
        Me.CkDisplayLabels.Size = New System.Drawing.Size(117, 20)
        Me.CkDisplayLabels.TabIndex = 84
        Me.CkDisplayLabels.Text = "Display Labels"
        Me.CkDisplayLabels.UseVisualStyleBackColor = True
        '
        'ImgListColors
        '
        Me.ImgListColors.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.ImgListColors.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImgListColors.TransparentColor = System.Drawing.Color.Transparent
        '
        'CboColors
        '
        Me.CboColors.FormattingEnabled = True
        Me.CboColors.Location = New System.Drawing.Point(290, 297)
        Me.CboColors.Name = "CboColors"
        Me.CboColors.Size = New System.Drawing.Size(90, 21)
        Me.CboColors.TabIndex = 85
        '
        'LblError
        '
        Me.LblError.AutoSize = True
        Me.LblError.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblError.ForeColor = System.Drawing.Color.Red
        Me.LblError.LinkArea = New System.Windows.Forms.LinkArea(63, 4)
        Me.LblError.Location = New System.Drawing.Point(3, 294)
        Me.LblError.Name = "LblError"
        Me.LblError.Size = New System.Drawing.Size(0, 19)
        Me.LblError.TabIndex = 86
        Me.LblError.UseCompatibleTextRendering = True
        '
        'HRU_ID
        '
        Me.HRU_ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HRU_ID.DefaultCellStyle = DataGridViewCellStyle2
        Me.HRU_ID.HeaderText = "HRU ID"
        Me.HRU_ID.Name = "HRU_ID"
        Me.HRU_ID.ReadOnly = True
        Me.HRU_ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic
        Me.HRU_ID.Width = 85
        '
        'FrmParameterViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(802, 360)
        Me.Controls.Add(Me.LblError)
        Me.Controls.Add(Me.CboColors)
        Me.Controls.Add(Me.CkDisplayLabels)
        Me.Controls.Add(Me.BtnViewValues)
        Me.Controls.Add(Me.BtnEditModel)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.GrdParam)
        Me.Controls.Add(Me.LstProfiles)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LblHruLayers)
        Me.Controls.Add(Me.LstHruLayers)
        Me.Controls.Add(Me.TxtAoiPath)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.BtnSelectAoi)
        Me.Name = "FrmParameterViewer"
        Me.ShowIcon = False
        Me.Text = "Parameter Viewer (AOI: Not selected)"
        CType(Me.GrdParam, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TxtAoiPath As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents BtnSelectAoi As System.Windows.Forms.Button
    Friend WithEvents LstHruLayers As System.Windows.Forms.ListBox
    Friend WithEvents LblHruLayers As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents LstProfiles As System.Windows.Forms.ListBox
    Friend WithEvents GrdParam As System.Windows.Forms.DataGridView
    Friend WithEvents BtnClose As System.Windows.Forms.Button
    Friend WithEvents BtnEditModel As System.Windows.Forms.Button
    Friend WithEvents BtnViewValues As System.Windows.Forms.Button
    Friend WithEvents CkDisplayLabels As System.Windows.Forms.CheckBox
    Friend WithEvents ImgListColors As System.Windows.Forms.ImageList
    Friend WithEvents CboColors As System.Windows.Forms.ComboBox
    Friend WithEvents LblError As System.Windows.Forms.LinkLabel
    Friend WithEvents HRU_ID As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
