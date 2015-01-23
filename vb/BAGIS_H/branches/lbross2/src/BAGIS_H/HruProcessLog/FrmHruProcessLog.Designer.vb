<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmHruProcessLog
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.TxtPolygonCount = New System.Windows.Forms.TextBox
        Me.TxtNonContiguous = New System.Windows.Forms.TextBox
        Me.TxtApplyZones = New System.Windows.Forms.TextBox
        Me.TxtParentName = New System.Windows.Forms.TextBox
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.TxtHruPath = New System.Windows.Forms.TextBox
        Me.TxtHruName = New System.Windows.Forms.TextBox
        Me.TxtUnits = New System.Windows.Forms.TextBox
        Me.TxtAoiPath = New System.Windows.Forms.TextBox
        Me.TxtAoiName = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.TxtAverageSize = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.TxtMaxSize = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.RuleType = New System.Windows.Forms.DataGridViewLinkColumn
        Me.DataLayer = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.RuleId = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.BtnClose = New System.Windows.Forms.Button
        Me.TxtDateCreated = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TxtPolygonCount
        '
        Me.TxtPolygonCount.Location = New System.Drawing.Point(387, 55)
        Me.TxtPolygonCount.Name = "TxtPolygonCount"
        Me.TxtPolygonCount.ReadOnly = True
        Me.TxtPolygonCount.Size = New System.Drawing.Size(50, 20)
        Me.TxtPolygonCount.TabIndex = 49
        '
        'TxtNonContiguous
        '
        Me.TxtNonContiguous.Location = New System.Drawing.Point(458, 2)
        Me.TxtNonContiguous.Name = "TxtNonContiguous"
        Me.TxtNonContiguous.ReadOnly = True
        Me.TxtNonContiguous.Size = New System.Drawing.Size(50, 20)
        Me.TxtNonContiguous.TabIndex = 48
        '
        'TxtApplyZones
        '
        Me.TxtApplyZones.Location = New System.Drawing.Point(125, 152)
        Me.TxtApplyZones.Name = "TxtApplyZones"
        Me.TxtApplyZones.ReadOnly = True
        Me.TxtApplyZones.Size = New System.Drawing.Size(158, 20)
        Me.TxtApplyZones.TabIndex = 47
        '
        'TxtParentName
        '
        Me.TxtParentName.Location = New System.Drawing.Point(83, 103)
        Me.TxtParentName.Name = "TxtParentName"
        Me.TxtParentName.ReadOnly = True
        Me.TxtParentName.Size = New System.Drawing.Size(200, 20)
        Me.TxtParentName.TabIndex = 46
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(78, 128)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(205, 20)
        Me.TextBox1.TabIndex = 45
        '
        'TxtHruPath
        '
        Me.TxtHruPath.Location = New System.Drawing.Point(69, 79)
        Me.TxtHruPath.Name = "TxtHruPath"
        Me.TxtHruPath.ReadOnly = True
        Me.TxtHruPath.Size = New System.Drawing.Size(214, 20)
        Me.TxtHruPath.TabIndex = 44
        '
        'TxtHruName
        '
        Me.TxtHruName.Location = New System.Drawing.Point(74, 55)
        Me.TxtHruName.Name = "TxtHruName"
        Me.TxtHruName.ReadOnly = True
        Me.TxtHruName.Size = New System.Drawing.Size(209, 20)
        Me.TxtHruName.TabIndex = 43
        '
        'TxtUnits
        '
        Me.TxtUnits.Location = New System.Drawing.Point(335, 26)
        Me.TxtUnits.Name = "TxtUnits"
        Me.TxtUnits.ReadOnly = True
        Me.TxtUnits.Size = New System.Drawing.Size(50, 20)
        Me.TxtUnits.TabIndex = 42
        '
        'TxtAoiPath
        '
        Me.TxtAoiPath.Location = New System.Drawing.Point(58, 30)
        Me.TxtAoiPath.Name = "TxtAoiPath"
        Me.TxtAoiPath.ReadOnly = True
        Me.TxtAoiPath.Size = New System.Drawing.Size(225, 20)
        Me.TxtAoiPath.TabIndex = 41
        '
        'TxtAoiName
        '
        Me.TxtAoiName.Location = New System.Drawing.Point(63, 5)
        Me.TxtAoiName.Name = "TxtAoiName"
        Me.TxtAoiName.ReadOnly = True
        Me.TxtAoiName.Size = New System.Drawing.Size(220, 20)
        Me.TxtAoiName.TabIndex = 40
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(299, 58)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(92, 13)
        Me.Label11.TabIndex = 39
        Me.Label11.Text = "Polygon count:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(299, 5)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(163, 13)
        Me.Label10.TabIndex = 38
        Me.Label10.Text = "Allow non-contiguous HRU:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(4, 155)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(125, 13)
        Me.Label8.TabIndex = 37
        Me.Label8.Text = "Apply to HRU zones:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(4, 131)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(77, 13)
        Me.Label9.TabIndex = 36
        Me.Label9.Text = "Parent path:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(4, 108)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(82, 13)
        Me.Label7.TabIndex = 35
        Me.Label7.Text = "Parent name:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(4, 82)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(67, 13)
        Me.Label6.TabIndex = 34
        Me.Label6.Text = "HRU path:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(4, 58)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(72, 13)
        Me.Label5.TabIndex = 33
        Me.Label5.Text = "HRU name:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(299, 31)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 13)
        Me.Label4.TabIndex = 32
        Me.Label4.Text = "Units:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(4, 33)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(58, 13)
        Me.Label3.TabIndex = 31
        Me.Label3.Text = "Aoi path:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(4, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 13)
        Me.Label1.TabIndex = 30
        Me.Label1.Text = "Aoi name:"
        '
        'TxtAverageSize
        '
        Me.TxtAverageSize.Location = New System.Drawing.Point(380, 79)
        Me.TxtAverageSize.Name = "TxtAverageSize"
        Me.TxtAverageSize.ReadOnly = True
        Me.TxtAverageSize.Size = New System.Drawing.Size(50, 20)
        Me.TxtAverageSize.TabIndex = 51
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(299, 82)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 13)
        Me.Label2.TabIndex = 50
        Me.Label2.Text = "Average size:"
        '
        'TxtMaxSize
        '
        Me.TxtMaxSize.Location = New System.Drawing.Point(383, 103)
        Me.TxtMaxSize.Name = "TxtMaxSize"
        Me.TxtMaxSize.ReadOnly = True
        Me.TxtMaxSize.Size = New System.Drawing.Size(50, 20)
        Me.TxtMaxSize.TabIndex = 53
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(299, 106)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(88, 13)
        Me.Label12.TabIndex = 52
        Me.Label12.Text = "Maximum size:"
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataGridView1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.RuleType, Me.DataLayer, Me.RuleId})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridView1.DefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridView1.Location = New System.Drawing.Point(7, 180)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.Size = New System.Drawing.Size(450, 150)
        Me.DataGridView1.TabIndex = 54
        '
        'RuleType
        '
        Me.RuleType.HeaderText = "Rule Type"
        Me.RuleType.Name = "RuleType"
        Me.RuleType.ReadOnly = True
        Me.RuleType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.RuleType.Width = 200
        '
        'DataLayer
        '
        Me.DataLayer.HeaderText = "Data Layer"
        Me.DataLayer.Name = "DataLayer"
        Me.DataLayer.ReadOnly = True
        Me.DataLayer.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.DataLayer.Width = 200
        '
        'RuleId
        '
        Me.RuleId.HeaderText = "Rule Id"
        Me.RuleId.Name = "RuleId"
        Me.RuleId.ReadOnly = True
        Me.RuleId.Visible = False
        '
        'BtnClose
        '
        Me.BtnClose.Location = New System.Drawing.Point(382, 345)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(75, 23)
        Me.BtnClose.TabIndex = 55
        Me.BtnClose.Text = "Close"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'TxtDateCreated
        '
        Me.TxtDateCreated.Location = New System.Drawing.Point(383, 127)
        Me.TxtDateCreated.Name = "TxtDateCreated"
        Me.TxtDateCreated.ReadOnly = True
        Me.TxtDateCreated.Size = New System.Drawing.Size(110, 20)
        Me.TxtDateCreated.TabIndex = 57
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(299, 130)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(85, 13)
        Me.Label13.TabIndex = 56
        Me.Label13.Text = "Date created:"
        '
        'FrmHruProcessLog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(515, 373)
        Me.Controls.Add(Me.TxtDateCreated)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.TxtMaxSize)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.TxtAverageSize)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TxtPolygonCount)
        Me.Controls.Add(Me.TxtNonContiguous)
        Me.Controls.Add(Me.TxtApplyZones)
        Me.Controls.Add(Me.TxtParentName)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.TxtHruPath)
        Me.Controls.Add(Me.TxtHruName)
        Me.Controls.Add(Me.TxtUnits)
        Me.Controls.Add(Me.TxtAoiPath)
        Me.Controls.Add(Me.TxtAoiName)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Name = "FrmHruProcessLog"
        Me.ShowIcon = False
        Me.Text = "FrmHelloWorld"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TxtPolygonCount As System.Windows.Forms.TextBox
    Friend WithEvents TxtNonContiguous As System.Windows.Forms.TextBox
    Friend WithEvents TxtApplyZones As System.Windows.Forms.TextBox
    Friend WithEvents TxtParentName As System.Windows.Forms.TextBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TxtHruPath As System.Windows.Forms.TextBox
    Friend WithEvents TxtHruName As System.Windows.Forms.TextBox
    Friend WithEvents TxtUnits As System.Windows.Forms.TextBox
    Friend WithEvents TxtAoiPath As System.Windows.Forms.TextBox
    Friend WithEvents TxtAoiName As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TxtAverageSize As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TxtMaxSize As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents BtnClose As System.Windows.Forms.Button
    Friend WithEvents TxtDateCreated As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents RuleType As System.Windows.Forms.DataGridViewLinkColumn
    Friend WithEvents DataLayer As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents RuleId As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
