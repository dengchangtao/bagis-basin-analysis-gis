<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmSubAoiId
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
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.TxtAoiPath = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.BtnSelectAoi = New System.Windows.Forms.Button()
        Me.GrdSubAoi = New System.Windows.Forms.DataGridView()
        Me.BtnDisplaySubAoi = New System.Windows.Forms.Button()
        Me.BtnCreateSubAoi = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BtnDelete = New System.Windows.Forms.Button()
        Me.BtnDisplayId = New System.Windows.Forms.Button()
        Me.GrdId = New System.Windows.Forms.DataGridView()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.BtnRename = New System.Windows.Forms.Button()
        Me.sName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Include = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewCheckBoxColumn1 = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        CType(Me.GrdSubAoi, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GrdId, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TxtAoiPath
        '
        Me.TxtAoiPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtAoiPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtAoiPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtAoiPath.Location = New System.Drawing.Point(174, 10)
        Me.TxtAoiPath.Name = "TxtAoiPath"
        Me.TxtAoiPath.ReadOnly = True
        Me.TxtAoiPath.Size = New System.Drawing.Size(512, 22)
        Me.TxtAoiPath.TabIndex = 65
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(113, 13)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(63, 16)
        Me.Label5.TabIndex = 64
        Me.Label5.Text = "AOI Path:"
        '
        'BtnSelectAoi
        '
        Me.BtnSelectAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSelectAoi.Location = New System.Drawing.Point(4, 6)
        Me.BtnSelectAoi.Name = "BtnSelectAoi"
        Me.BtnSelectAoi.Size = New System.Drawing.Size(105, 28)
        Me.BtnSelectAoi.TabIndex = 63
        Me.BtnSelectAoi.Text = "Select AOI"
        Me.BtnSelectAoi.UseVisualStyleBackColor = True
        '
        'GrdSubAoi
        '
        Me.GrdSubAoi.AllowUserToAddRows = False
        Me.GrdSubAoi.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.GrdSubAoi.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.GrdSubAoi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GrdSubAoi.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.sName, Me.Include})
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.GrdSubAoi.DefaultCellStyle = DataGridViewCellStyle3
        Me.GrdSubAoi.Location = New System.Drawing.Point(5, 60)
        Me.GrdSubAoi.Name = "GrdSubAoi"
        Me.GrdSubAoi.Size = New System.Drawing.Size(527, 150)
        Me.GrdSubAoi.TabIndex = 66
        '
        'BtnDisplaySubAoi
        '
        Me.BtnDisplaySubAoi.Enabled = False
        Me.BtnDisplaySubAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDisplaySubAoi.Location = New System.Drawing.Point(5, 216)
        Me.BtnDisplaySubAoi.Name = "BtnDisplaySubAoi"
        Me.BtnDisplaySubAoi.Size = New System.Drawing.Size(175, 28)
        Me.BtnDisplaySubAoi.TabIndex = 67
        Me.BtnDisplaySubAoi.Text = "Display Selected Layer(s)"
        Me.BtnDisplaySubAoi.UseVisualStyleBackColor = True
        '
        'BtnCreateSubAoi
        '
        Me.BtnCreateSubAoi.Enabled = False
        Me.BtnCreateSubAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCreateSubAoi.Location = New System.Drawing.Point(185, 216)
        Me.BtnCreateSubAoi.Name = "BtnCreateSubAoi"
        Me.BtnCreateSubAoi.Size = New System.Drawing.Size(320, 28)
        Me.BtnCreateSubAoi.TabIndex = 69
        Me.BtnCreateSubAoi.Text = "Create SubAOI ID Layer Using Selected SubAOI(s)"
        Me.BtnCreateSubAoi.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(2, 39)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(166, 16)
        Me.Label2.TabIndex = 70
        Me.Label2.Text = "SubAOIs in Current AOI"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(2, 259)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(226, 16)
        Me.Label1.TabIndex = 75
        Me.Label1.Text = "SubAOI Id Layers in Current AOI"
        '
        'BtnDelete
        '
        Me.BtnDelete.Enabled = False
        Me.BtnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDelete.Location = New System.Drawing.Point(183, 476)
        Me.BtnDelete.Name = "BtnDelete"
        Me.BtnDelete.Size = New System.Drawing.Size(165, 28)
        Me.BtnDelete.TabIndex = 73
        Me.BtnDelete.Text = "Delete Selected Layer(s)"
        Me.BtnDelete.UseVisualStyleBackColor = True
        '
        'BtnDisplayId
        '
        Me.BtnDisplayId.Enabled = False
        Me.BtnDisplayId.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDisplayId.Location = New System.Drawing.Point(4, 476)
        Me.BtnDisplayId.Name = "BtnDisplayId"
        Me.BtnDisplayId.Size = New System.Drawing.Size(175, 28)
        Me.BtnDisplayId.TabIndex = 72
        Me.BtnDisplayId.Text = "Display Selected Layer(s)"
        Me.BtnDisplayId.UseVisualStyleBackColor = True
        '
        'GrdId
        '
        Me.GrdId.AllowUserToAddRows = False
        Me.GrdId.AllowUserToDeleteRows = False
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.GrdId.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.GrdId.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GrdId.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewCheckBoxColumn1})
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.GrdId.DefaultCellStyle = DataGridViewCellStyle6
        Me.GrdId.Location = New System.Drawing.Point(5, 281)
        Me.GrdId.Name = "GrdId"
        Me.GrdId.Size = New System.Drawing.Size(527, 150)
        Me.GrdId.TabIndex = 71
        '
        'BtnClose
        '
        Me.BtnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClose.Location = New System.Drawing.Point(537, 478)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(65, 25)
        Me.BtnClose.TabIndex = 82
        Me.BtnClose.Text = "Close"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(2, 435)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(462, 32)
        Me.Label3.TabIndex = 83
        Me.Label3.Text = "Note: The SubAOI IDs in these layers may be exported to the parameter files " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "of " & _
    "any HRU in the associated AOI"
        '
        'BtnRename
        '
        Me.BtnRename.Enabled = False
        Me.BtnRename.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnRename.Location = New System.Drawing.Point(352, 476)
        Me.BtnRename.Name = "BtnRename"
        Me.BtnRename.Size = New System.Drawing.Size(180, 28)
        Me.BtnRename.TabIndex = 84
        Me.BtnRename.Text = "Rename Selected Layer(s)"
        Me.BtnRename.UseVisualStyleBackColor = True
        '
        'sName
        '
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.sName.DefaultCellStyle = DataGridViewCellStyle2
        Me.sName.HeaderText = "SubAOI Name"
        Me.sName.Name = "sName"
        Me.sName.ReadOnly = True
        Me.sName.Width = 400
        '
        'Include
        '
        Me.Include.HeaderText = "Selected"
        Me.Include.Name = "Include"
        Me.Include.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Include.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Include.Width = 75
        '
        'DataGridViewTextBoxColumn1
        '
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGridViewTextBoxColumn1.DefaultCellStyle = DataGridViewCellStyle5
        Me.DataGridViewTextBoxColumn1.HeaderText = "SubAOI Id Layer Name"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Width = 400
        '
        'DataGridViewCheckBoxColumn1
        '
        Me.DataGridViewCheckBoxColumn1.HeaderText = "Selected"
        Me.DataGridViewCheckBoxColumn1.Name = "DataGridViewCheckBoxColumn1"
        Me.DataGridViewCheckBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridViewCheckBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.DataGridViewCheckBoxColumn1.Width = 75
        '
        'FrmSubAoiId
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(612, 516)
        Me.Controls.Add(Me.BtnRename)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.BtnDelete)
        Me.Controls.Add(Me.BtnDisplayId)
        Me.Controls.Add(Me.GrdId)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.BtnCreateSubAoi)
        Me.Controls.Add(Me.BtnDisplaySubAoi)
        Me.Controls.Add(Me.GrdSubAoi)
        Me.Controls.Add(Me.TxtAoiPath)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.BtnSelectAoi)
        Me.Name = "FrmSubAoiId"
        Me.ShowIcon = False
        Me.Text = "Manage SubAOI Id Layers"
        CType(Me.GrdSubAoi, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GrdId, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TxtAoiPath As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents BtnSelectAoi As System.Windows.Forms.Button
    Friend WithEvents GrdSubAoi As System.Windows.Forms.DataGridView
    Friend WithEvents BtnDisplaySubAoi As System.Windows.Forms.Button
    Friend WithEvents BtnCreateSubAoi As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents BtnDelete As System.Windows.Forms.Button
    Friend WithEvents BtnDisplayId As System.Windows.Forms.Button
    Friend WithEvents GrdId As System.Windows.Forms.DataGridView
    Friend WithEvents BtnClose As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents BtnRename As System.Windows.Forms.Button
    Friend WithEvents sName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Include As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewCheckBoxColumn1 As System.Windows.Forms.DataGridViewCheckBoxColumn
End Class
