<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmExportParameters
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
        Me.TxtAoiPath = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.BtnSelectAoi = New System.Windows.Forms.Button()
        Me.LblHruLayers = New System.Windows.Forms.Label()
        Me.LstHruLayers = New System.Windows.Forms.ListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtNHru = New System.Windows.Forms.TextBox()
        Me.TxtNumParameters = New System.Windows.Forms.TextBox()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.BtnSetOutput = New System.Windows.Forms.Button()
        Me.BtnExport = New System.Windows.Forms.Button()
        Me.LstProfiles = New System.Windows.Forms.ListBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.TxtDescription = New System.Windows.Forms.TextBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.TxtOutputName = New System.Windows.Forms.TextBox()
        Me.TxtOutputFolder1 = New System.Windows.Forms.TextBox()
        Me.CkExportShapefile = New System.Windows.Forms.CheckBox()
        Me.CkExportZipped = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'TxtAoiPath
        '
        Me.TxtAoiPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtAoiPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtAoiPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtAoiPath.Location = New System.Drawing.Point(176, 9)
        Me.TxtAoiPath.Name = "TxtAoiPath"
        Me.TxtAoiPath.ReadOnly = True
        Me.TxtAoiPath.Size = New System.Drawing.Size(512, 22)
        Me.TxtAoiPath.TabIndex = 62
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(115, 12)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(63, 16)
        Me.Label5.TabIndex = 61
        Me.Label5.Text = "AOI Path:"
        '
        'BtnSelectAoi
        '
        Me.BtnSelectAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSelectAoi.Location = New System.Drawing.Point(4, 6)
        Me.BtnSelectAoi.Name = "BtnSelectAoi"
        Me.BtnSelectAoi.Size = New System.Drawing.Size(105, 28)
        Me.BtnSelectAoi.TabIndex = 60
        Me.BtnSelectAoi.Text = "Select AOI"
        Me.BtnSelectAoi.UseVisualStyleBackColor = True
        '
        'LblHruLayers
        '
        Me.LblHruLayers.AutoSize = True
        Me.LblHruLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblHruLayers.Location = New System.Drawing.Point(1, 44)
        Me.LblHruLayers.Name = "LblHruLayers"
        Me.LblHruLayers.Size = New System.Drawing.Size(137, 16)
        Me.LblHruLayers.TabIndex = 64
        Me.LblHruLayers.Text = "HRU Layers in AOI"
        '
        'LstHruLayers
        '
        Me.LstHruLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstHruLayers.FormattingEnabled = True
        Me.LstHruLayers.ItemHeight = 16
        Me.LstHruLayers.Location = New System.Drawing.Point(4, 64)
        Me.LstHruLayers.Name = "LstHruLayers"
        Me.LstHruLayers.Size = New System.Drawing.Size(219, 100)
        Me.LstHruLayers.TabIndex = 63
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(1, 174)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(124, 16)
        Me.Label1.TabIndex = 65
        Me.Label1.Text = "Number of HRUs"
        '
        'TxtNHru
        '
        Me.TxtNHru.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtNHru.Location = New System.Drawing.Point(126, 171)
        Me.TxtNHru.Name = "TxtNHru"
        Me.TxtNHru.ReadOnly = True
        Me.TxtNHru.Size = New System.Drawing.Size(60, 22)
        Me.TxtNHru.TabIndex = 66
        '
        'TxtNumParameters
        '
        Me.TxtNumParameters.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtNumParameters.Location = New System.Drawing.Point(400, 171)
        Me.TxtNumParameters.Name = "TxtNumParameters"
        Me.TxtNumParameters.ReadOnly = True
        Me.TxtNumParameters.Size = New System.Drawing.Size(60, 22)
        Me.TxtNumParameters.TabIndex = 68
        '
        'BtnClose
        '
        Me.BtnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClose.Location = New System.Drawing.Point(626, 295)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(65, 25)
        Me.BtnClose.TabIndex = 81
        Me.BtnClose.Text = "Close"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        Me.OpenFileDialog1.Filter = "csv files (*.csv)|*.csv|text files (*.txt)|*.txt*"
        Me.OpenFileDialog1.RestoreDirectory = True
        Me.OpenFileDialog1.Title = "Parameter Template"
        '
        'BtnSetOutput
        '
        Me.BtnSetOutput.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSetOutput.Location = New System.Drawing.Point(585, 224)
        Me.BtnSetOutput.Name = "BtnSetOutput"
        Me.BtnSetOutput.Size = New System.Drawing.Size(106, 25)
        Me.BtnSetOutput.TabIndex = 90
        Me.BtnSetOutput.Text = "Set Output"
        Me.BtnSetOutput.UseVisualStyleBackColor = True
        '
        'BtnExport
        '
        Me.BtnExport.Enabled = False
        Me.BtnExport.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnExport.Location = New System.Drawing.Point(555, 295)
        Me.BtnExport.Name = "BtnExport"
        Me.BtnExport.Size = New System.Drawing.Size(65, 25)
        Me.BtnExport.TabIndex = 91
        Me.BtnExport.Text = "Export"
        Me.BtnExport.UseVisualStyleBackColor = True
        '
        'LstProfiles
        '
        Me.LstProfiles.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstProfiles.FormattingEnabled = True
        Me.LstProfiles.ItemHeight = 16
        Me.LstProfiles.Location = New System.Drawing.Point(241, 64)
        Me.LstProfiles.Name = "LstProfiles"
        Me.LstProfiles.Size = New System.Drawing.Size(219, 100)
        Me.LstProfiles.Sorted = True
        Me.LstProfiles.TabIndex = 94
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(238, 44)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(61, 16)
        Me.Label8.TabIndex = 93
        Me.Label8.Text = "Profiles"
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.Filter = "csv files (*.csv)|*.csv"
        Me.SaveFileDialog1.RestoreDirectory = True
        Me.SaveFileDialog1.Title = "Output Path"
        '
        'TxtDescription
        '
        Me.TxtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtDescription.Location = New System.Drawing.Point(478, 64)
        Me.TxtDescription.Multiline = True
        Me.TxtDescription.Name = "TxtDescription"
        Me.TxtDescription.ReadOnly = True
        Me.TxtDescription.Size = New System.Drawing.Size(215, 100)
        Me.TxtDescription.TabIndex = 96
        '
        'TextBox1
        '
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(478, 44)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(137, 15)
        Me.TextBox1.TabIndex = 97
        Me.TextBox1.Text = "Profile description"
        '
        'TextBox2
        '
        Me.TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(241, 174)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(162, 15)
        Me.TextBox2.TabIndex = 98
        Me.TextBox2.Text = "Number of parameters"
        '
        'TextBox3
        '
        Me.TextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox3.Location = New System.Drawing.Point(4, 200)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ReadOnly = True
        Me.TextBox3.Size = New System.Drawing.Size(121, 15)
        Me.TextBox3.TabIndex = 99
        Me.TextBox3.Text = "Output name"
        '
        'TextBox4
        '
        Me.TextBox4.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox4.Location = New System.Drawing.Point(5, 278)
        Me.TextBox4.Multiline = True
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.ReadOnly = True
        Me.TextBox4.Size = New System.Drawing.Size(546, 42)
        Me.TextBox4.TabIndex = 100
        Me.TextBox4.Text = "The export output is a shapefile with the parameter values appended to the attrib" & _
    "ute table."
        '
        'TextBox5
        '
        Me.TextBox5.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox5.Location = New System.Drawing.Point(4, 227)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.ReadOnly = True
        Me.TextBox5.Size = New System.Drawing.Size(121, 15)
        Me.TextBox5.TabIndex = 102
        Me.TextBox5.Text = "Output folder"
        '
        'TxtOutputName
        '
        Me.TxtOutputName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtOutputName.Location = New System.Drawing.Point(107, 197)
        Me.TxtOutputName.Name = "TxtOutputName"
        Me.TxtOutputName.Size = New System.Drawing.Size(468, 22)
        Me.TxtOutputName.TabIndex = 103
        '
        'TxtOutputFolder1
        '
        Me.TxtOutputFolder1.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtOutputFolder1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtOutputFolder1.ForeColor = System.Drawing.Color.Black
        Me.TxtOutputFolder1.Location = New System.Drawing.Point(107, 225)
        Me.TxtOutputFolder1.Name = "TxtOutputFolder1"
        Me.TxtOutputFolder1.ReadOnly = True
        Me.TxtOutputFolder1.Size = New System.Drawing.Size(468, 22)
        Me.TxtOutputFolder1.TabIndex = 104
        '
        'CkExportShapefile
        '
        Me.CkExportShapefile.AutoSize = True
        Me.CkExportShapefile.Checked = True
        Me.CkExportShapefile.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CkExportShapefile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CkExportShapefile.Location = New System.Drawing.Point(12, 249)
        Me.CkExportShapefile.Name = "CkExportShapefile"
        Me.CkExportShapefile.Size = New System.Drawing.Size(152, 20)
        Me.CkExportShapefile.TabIndex = 105
        Me.CkExportShapefile.Text = "Export as a shapefile"
        Me.CkExportShapefile.UseVisualStyleBackColor = True
        '
        'CkExportZipped
        '
        Me.CkExportZipped.AutoSize = True
        Me.CkExportZipped.Checked = True
        Me.CkExportZipped.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CkExportZipped.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CkExportZipped.Location = New System.Drawing.Point(171, 249)
        Me.CkExportZipped.Name = "CkExportZipped"
        Me.CkExportZipped.Size = New System.Drawing.Size(196, 20)
        Me.CkExportZipped.TabIndex = 106
        Me.CkExportZipped.Text = "Export as a zipped shapefile"
        Me.CkExportZipped.UseVisualStyleBackColor = True
        '
        'FrmExportParameters
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(709, 333)
        Me.Controls.Add(Me.CkExportZipped)
        Me.Controls.Add(Me.CkExportShapefile)
        Me.Controls.Add(Me.TxtOutputFolder1)
        Me.Controls.Add(Me.TxtOutputName)
        Me.Controls.Add(Me.TextBox5)
        Me.Controls.Add(Me.TextBox4)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.TxtDescription)
        Me.Controls.Add(Me.LstProfiles)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.BtnExport)
        Me.Controls.Add(Me.BtnSetOutput)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.TxtNumParameters)
        Me.Controls.Add(Me.TxtNHru)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LblHruLayers)
        Me.Controls.Add(Me.LstHruLayers)
        Me.Controls.Add(Me.TxtAoiPath)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.BtnSelectAoi)
        Me.Name = "FrmExportParameters"
        Me.ShowIcon = False
        Me.Text = "Export Parameters As Shapefile"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TxtAoiPath As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents BtnSelectAoi As System.Windows.Forms.Button
    Friend WithEvents LblHruLayers As System.Windows.Forms.Label
    Friend WithEvents LstHruLayers As System.Windows.Forms.ListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TxtNHru As System.Windows.Forms.TextBox
    Friend WithEvents TxtNumParameters As System.Windows.Forms.TextBox
    Friend WithEvents BtnClose As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents BtnSetOutput As System.Windows.Forms.Button
    Friend WithEvents BtnExport As System.Windows.Forms.Button
    Friend WithEvents LstProfiles As System.Windows.Forms.ListBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents TxtDescription As System.Windows.Forms.TextBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox4 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents TxtOutputName As System.Windows.Forms.TextBox
    Friend WithEvents TxtOutputFolder1 As System.Windows.Forms.TextBox
    Friend WithEvents CkExportShapefile As System.Windows.Forms.CheckBox
    Friend WithEvents CkExportZipped As System.Windows.Forms.CheckBox
End Class
