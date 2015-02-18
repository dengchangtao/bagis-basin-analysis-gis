<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmDownloadAoiMenu
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.AoiGrid = New System.Windows.Forms.DataGridView()
        Me.AoiName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Uploaded = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Author = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Download = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.Remarks = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.BtnDownloadAoi = New System.Windows.Forms.Button()
        Me.LblHruLayers = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtNewHruName = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.CboRuleType = New System.Windows.Forms.ComboBox()
        Me.BtnSearch = New System.Windows.Forms.Button()
        Me.TxtAoiPath = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.BtnSelectAoi = New System.Windows.Forms.Button()
        Me.TxtBasinsDb = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        CType(Me.AoiGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AoiGrid
        '
        Me.AoiGrid.AllowUserToAddRows = False
        Me.AoiGrid.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.AoiGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.AoiGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.AoiGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.AoiName, Me.Uploaded, Me.Author, Me.Download, Me.Remarks})
        Me.AoiGrid.Location = New System.Drawing.Point(12, 151)
        Me.AoiGrid.Name = "AoiGrid"
        Me.AoiGrid.ReadOnly = True
        Me.AoiGrid.Size = New System.Drawing.Size(851, 208)
        Me.AoiGrid.TabIndex = 0
        Me.AoiGrid.TabStop = False
        '
        'AoiName
        '
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AoiName.DefaultCellStyle = DataGridViewCellStyle2
        Me.AoiName.HeaderText = "AOI Name"
        Me.AoiName.Name = "AoiName"
        Me.AoiName.ReadOnly = True
        Me.AoiName.Width = 250
        '
        'Uploaded
        '
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Uploaded.DefaultCellStyle = DataGridViewCellStyle3
        Me.Uploaded.HeaderText = "Uploaded"
        Me.Uploaded.Name = "Uploaded"
        Me.Uploaded.ReadOnly = True
        '
        'Author
        '
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Author.DefaultCellStyle = DataGridViewCellStyle4
        Me.Author.HeaderText = "Author"
        Me.Author.Name = "Author"
        Me.Author.ReadOnly = True
        '
        'Download
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.NullValue = False
        Me.Download.DefaultCellStyle = DataGridViewCellStyle5
        Me.Download.HeaderText = "Download ?"
        Me.Download.Name = "Download"
        Me.Download.ReadOnly = True
        '
        'Remarks
        '
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Remarks.DefaultCellStyle = DataGridViewCellStyle6
        Me.Remarks.HeaderText = "Remarks"
        Me.Remarks.Name = "Remarks"
        Me.Remarks.ReadOnly = True
        Me.Remarks.Width = 250
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(614, 377)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(108, 30)
        Me.BtnCancel.TabIndex = 57
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'BtnDownloadAoi
        '
        Me.BtnDownloadAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDownloadAoi.Location = New System.Drawing.Point(738, 377)
        Me.BtnDownloadAoi.Name = "BtnDownloadAoi"
        Me.BtnDownloadAoi.Size = New System.Drawing.Size(125, 30)
        Me.BtnDownloadAoi.TabIndex = 56
        Me.BtnDownloadAoi.Text = "Download"
        Me.BtnDownloadAoi.UseVisualStyleBackColor = True
        '
        'LblHruLayers
        '
        Me.LblHruLayers.AutoSize = True
        Me.LblHruLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblHruLayers.Location = New System.Drawing.Point(9, 40)
        Me.LblHruLayers.Name = "LblHruLayers"
        Me.LblHruLayers.Size = New System.Drawing.Size(104, 16)
        Me.LblHruLayers.TabIndex = 58
        Me.LblHruLayers.Text = "Search Filters"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(141, 40)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(82, 16)
        Me.Label1.TabIndex = 59
        Me.Label1.Text = "AOI Name:"
        '
        'TxtNewHruName
        '
        Me.TxtNewHruName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtNewHruName.Location = New System.Drawing.Point(218, 38)
        Me.TxtNewHruName.Name = "TxtNewHruName"
        Me.TxtNewHruName.Size = New System.Drawing.Size(174, 22)
        Me.TxtNewHruName.TabIndex = 60
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(219, 63)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(94, 13)
        Me.Label2.TabIndex = 61
        Me.Label2.Text = "Use % for wildcard"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(607, 40)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(131, 16)
        Me.Label3.TabIndex = 62
        Me.Label3.Text = "Uploaded Before:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(403, 76)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(117, 16)
        Me.Label4.TabIndex = 63
        Me.Label4.Text = "Uploaded After:"
        '
        'TextBox1
        '
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(736, 38)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(75, 22)
        Me.TextBox1.TabIndex = 64
        '
        'TextBox2
        '
        Me.TextBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(517, 37)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(75, 22)
        Me.TextBox2.TabIndex = 65
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(464, 40)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(56, 16)
        Me.Label5.TabIndex = 66
        Me.Label5.Text = "Author:"
        '
        'CboRuleType
        '
        Me.CboRuleType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboRuleType.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboRuleType.FormattingEnabled = True
        Me.CboRuleType.Items.AddRange(New Object() {"Contributing Area", "DAFlow-Type Zones", "PRISM Precipitation", "Raster Reclass (Continuous data)", "Raster Reclass (Discrete data)", "Raster Slice", "Template - Aspect", "Template - Canopy", "Template - Land Use", "Template - Slope"})
        Me.CboRuleType.Location = New System.Drawing.Point(518, 72)
        Me.CboRuleType.Name = "CboRuleType"
        Me.CboRuleType.Size = New System.Drawing.Size(150, 24)
        Me.CboRuleType.TabIndex = 67
        '
        'BtnSearch
        '
        Me.BtnSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSearch.Location = New System.Drawing.Point(711, 69)
        Me.BtnSearch.Name = "BtnSearch"
        Me.BtnSearch.Size = New System.Drawing.Size(100, 30)
        Me.BtnSearch.TabIndex = 68
        Me.BtnSearch.Text = "Search"
        Me.BtnSearch.UseVisualStyleBackColor = True
        '
        'TxtAoiPath
        '
        Me.TxtAoiPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtAoiPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtAoiPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtAoiPath.Location = New System.Drawing.Point(109, 108)
        Me.TxtAoiPath.Name = "TxtAoiPath"
        Me.TxtAoiPath.ReadOnly = True
        Me.TxtAoiPath.Size = New System.Drawing.Size(559, 22)
        Me.TxtAoiPath.TabIndex = 71
        Me.TxtAoiPath.Text = "C:\Users\Lesley\AllMyAOIsAreStoredHere"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(9, 111)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(98, 16)
        Me.Label6.TabIndex = 70
        Me.Label6.Text = "Download to:"
        '
        'BtnSelectAoi
        '
        Me.BtnSelectAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSelectAoi.Location = New System.Drawing.Point(711, 106)
        Me.BtnSelectAoi.Name = "BtnSelectAoi"
        Me.BtnSelectAoi.Size = New System.Drawing.Size(100, 30)
        Me.BtnSelectAoi.TabIndex = 69
        Me.BtnSelectAoi.Text = "Select"
        Me.BtnSelectAoi.UseVisualStyleBackColor = True
        '
        'TxtBasinsDb
        '
        Me.TxtBasinsDb.Enabled = False
        Me.TxtBasinsDb.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtBasinsDb.Location = New System.Drawing.Point(143, 6)
        Me.TxtBasinsDb.Name = "TxtBasinsDb"
        Me.TxtBasinsDb.Size = New System.Drawing.Size(350, 22)
        Me.TxtBasinsDb.TabIndex = 73
        Me.TxtBasinsDb.Text = "https://webservices.geog.pdx.edu:8000/api/rest/"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(9, 9)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(131, 16)
        Me.Label7.TabIndex = 72
        Me.Label7.Text = "Basins Database:"
        '
        'FrmDownloadAoiMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(874, 417)
        Me.Controls.Add(Me.TxtBasinsDb)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.TxtAoiPath)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.BtnSelectAoi)
        Me.Controls.Add(Me.BtnSearch)
        Me.Controls.Add(Me.CboRuleType)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TxtNewHruName)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LblHruLayers)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnDownloadAoi)
        Me.Controls.Add(Me.AoiGrid)
        Me.Name = "FrmDownloadAoiMenu"
        Me.ShowIcon = False
        Me.Text = "Download AOI"
        CType(Me.AoiGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents AoiGrid As System.Windows.Forms.DataGridView
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents BtnDownloadAoi As System.Windows.Forms.Button
    Friend WithEvents AoiName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Uploaded As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Author As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Download As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Remarks As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LblHruLayers As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TxtNewHruName As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents CboRuleType As System.Windows.Forms.ComboBox
    Friend WithEvents BtnSearch As System.Windows.Forms.Button
    Friend WithEvents TxtAoiPath As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents BtnSelectAoi As System.Windows.Forms.Button
    Friend WithEvents TxtBasinsDb As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
End Class
