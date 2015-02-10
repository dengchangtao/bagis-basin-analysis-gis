<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmUploadAoi
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
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.BtnUploadAoi = New System.Windows.Forms.Button()
        Me.TxtAoiPath = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.BtnSelectAoi = New System.Windows.Forms.Button()
        Me.LayerGrid = New System.Windows.Forms.DataGridView()
        Me.LayerName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Author = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Action = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Download = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        CType(Me.LayerGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(398, 403)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(108, 30)
        Me.BtnCancel.TabIndex = 59
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'BtnUploadAoi
        '
        Me.BtnUploadAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnUploadAoi.Location = New System.Drawing.Point(522, 403)
        Me.BtnUploadAoi.Name = "BtnUploadAoi"
        Me.BtnUploadAoi.Size = New System.Drawing.Size(125, 30)
        Me.BtnUploadAoi.TabIndex = 58
        Me.BtnUploadAoi.Text = "Upload"
        Me.BtnUploadAoi.UseVisualStyleBackColor = True
        '
        'TxtAoiPath
        '
        Me.TxtAoiPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtAoiPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtAoiPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtAoiPath.Location = New System.Drawing.Point(188, 15)
        Me.TxtAoiPath.Name = "TxtAoiPath"
        Me.TxtAoiPath.ReadOnly = True
        Me.TxtAoiPath.Size = New System.Drawing.Size(459, 22)
        Me.TxtAoiPath.TabIndex = 62
        Me.TxtAoiPath.Text = "D:\Momeni\AOIs\teton_aoi"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(119, 18)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(63, 16)
        Me.Label5.TabIndex = 61
        Me.Label5.Text = "AOI Path:"
        '
        'BtnSelectAoi
        '
        Me.BtnSelectAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSelectAoi.Location = New System.Drawing.Point(8, 12)
        Me.BtnSelectAoi.Name = "BtnSelectAoi"
        Me.BtnSelectAoi.Size = New System.Drawing.Size(105, 28)
        Me.BtnSelectAoi.TabIndex = 60
        Me.BtnSelectAoi.Text = "Select AOI"
        Me.BtnSelectAoi.UseVisualStyleBackColor = True
        '
        'LayerGrid
        '
        Me.LayerGrid.AllowUserToAddRows = False
        Me.LayerGrid.AllowUserToDeleteRows = False
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.LayerGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.LayerGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.LayerGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.LayerName, Me.Author, Me.Action, Me.Download})
        Me.LayerGrid.Location = New System.Drawing.Point(8, 72)
        Me.LayerGrid.Name = "LayerGrid"
        Me.LayerGrid.ReadOnly = True
        Me.LayerGrid.Size = New System.Drawing.Size(639, 313)
        Me.LayerGrid.TabIndex = 63
        Me.LayerGrid.TabStop = False
        '
        'LayerName
        '
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LayerName.DefaultCellStyle = DataGridViewCellStyle7
        Me.LayerName.HeaderText = "Layer Name"
        Me.LayerName.Name = "LayerName"
        Me.LayerName.ReadOnly = True
        Me.LayerName.Width = 240
        '
        'Author
        '
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Author.DefaultCellStyle = DataGridViewCellStyle8
        Me.Author.HeaderText = "Type"
        Me.Author.Name = "Author"
        Me.Author.ReadOnly = True
        '
        'Action
        '
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Action.DefaultCellStyle = DataGridViewCellStyle9
        Me.Action.HeaderText = "Action"
        Me.Action.Name = "Action"
        Me.Action.ReadOnly = True
        Me.Action.Width = 150
        '
        'Download
        '
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle10.NullValue = False
        Me.Download.DefaultCellStyle = DataGridViewCellStyle10
        Me.Download.HeaderText = "Upload ?"
        Me.Download.Name = "Download"
        Me.Download.ReadOnly = True
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.SystemColors.MenuBar
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(8, 46)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(639, 20)
        Me.TextBox1.TabIndex = 64
        Me.TextBox1.Text = "This list only contains layers that have changed since the AOI was originally dow" & _
            "nloaded"
        '
        'FrmUploadAoi
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(659, 445)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.LayerGrid)
        Me.Controls.Add(Me.TxtAoiPath)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.BtnSelectAoi)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnUploadAoi)
        Me.Name = "FrmUploadAoi"
        Me.ShowIcon = False
        Me.Text = "Upload AOI"
        CType(Me.LayerGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents BtnUploadAoi As System.Windows.Forms.Button
    Friend WithEvents TxtAoiPath As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents BtnSelectAoi As System.Windows.Forms.Button
    Friend WithEvents LayerGrid As System.Windows.Forms.DataGridView
    Friend WithEvents LayerName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Author As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Action As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Download As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
End Class
