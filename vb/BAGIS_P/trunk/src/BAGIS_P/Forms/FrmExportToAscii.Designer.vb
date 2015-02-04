<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmExportToAscii
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
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.BtnSetOutput = New System.Windows.Forms.Button()
        Me.BtnExport = New System.Windows.Forms.Button()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.TxtHruOutputName = New System.Windows.Forms.TextBox()
        Me.TxtOutputFolder1 = New System.Windows.Forms.TextBox()
        Me.TxtHruResolution = New System.Windows.Forms.TextBox()
        Me.TextBox7 = New System.Windows.Forms.TextBox()
        Me.TxtHruUnits = New System.Windows.Forms.TextBox()
        Me.TextBox8 = New System.Windows.Forms.TextBox()
        Me.TxtHruResample = New System.Windows.Forms.TextBox()
        Me.TxtDemResample = New System.Windows.Forms.TextBox()
        Me.TxtDemUnits = New System.Windows.Forms.TextBox()
        Me.TextBox9 = New System.Windows.Forms.TextBox()
        Me.TxtDemResolution = New System.Windows.Forms.TextBox()
        Me.TextBox11 = New System.Windows.Forms.TextBox()
        Me.TxtDemOutputName = New System.Windows.Forms.TextBox()
        Me.TextBox13 = New System.Windows.Forms.TextBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.CboResampleHru = New System.Windows.Forms.ComboBox()
        Me.CboResampleDem = New System.Windows.Forms.ComboBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
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
        Me.Label1.Location = New System.Drawing.Point(239, 67)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(124, 16)
        Me.Label1.TabIndex = 65
        Me.Label1.Text = "Number of HRUs"
        '
        'TxtNHru
        '
        Me.TxtNHru.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtNHru.Location = New System.Drawing.Point(364, 64)
        Me.TxtNHru.Name = "TxtNHru"
        Me.TxtNHru.ReadOnly = True
        Me.TxtNHru.Size = New System.Drawing.Size(60, 22)
        Me.TxtNHru.TabIndex = 66
        '
        'BtnClose
        '
        Me.BtnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClose.Location = New System.Drawing.Point(625, 323)
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
        Me.BtnSetOutput.Location = New System.Drawing.Point(584, 174)
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
        Me.BtnExport.Location = New System.Drawing.Point(554, 323)
        Me.BtnExport.Name = "BtnExport"
        Me.BtnExport.Size = New System.Drawing.Size(65, 25)
        Me.BtnExport.TabIndex = 91
        Me.BtnExport.Text = "Export"
        Me.BtnExport.UseVisualStyleBackColor = True
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.Filter = "csv files (*.csv)|*.csv"
        Me.SaveFileDialog1.RestoreDirectory = True
        Me.SaveFileDialog1.Title = "Output Path"
        '
        'TextBox3
        '
        Me.TextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox3.Location = New System.Drawing.Point(3, 203)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ReadOnly = True
        Me.TextBox3.Size = New System.Drawing.Size(121, 15)
        Me.TextBox3.TabIndex = 99
        Me.TextBox3.Text = "HRU ASCII name"
        '
        'TextBox5
        '
        Me.TextBox5.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox5.Location = New System.Drawing.Point(3, 174)
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.ReadOnly = True
        Me.TextBox5.Size = New System.Drawing.Size(121, 15)
        Me.TextBox5.TabIndex = 102
        Me.TextBox5.Text = "Output folder"
        '
        'TxtHruOutputName
        '
        Me.TxtHruOutputName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtHruOutputName.Location = New System.Drawing.Point(130, 200)
        Me.TxtHruOutputName.Name = "TxtHruOutputName"
        Me.TxtHruOutputName.Size = New System.Drawing.Size(444, 22)
        Me.TxtHruOutputName.TabIndex = 103
        '
        'TxtOutputFolder1
        '
        Me.TxtOutputFolder1.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtOutputFolder1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtOutputFolder1.ForeColor = System.Drawing.Color.Black
        Me.TxtOutputFolder1.Location = New System.Drawing.Point(106, 172)
        Me.TxtOutputFolder1.Name = "TxtOutputFolder1"
        Me.TxtOutputFolder1.ReadOnly = True
        Me.TxtOutputFolder1.Size = New System.Drawing.Size(468, 22)
        Me.TxtOutputFolder1.TabIndex = 104
        '
        'TxtHruResolution
        '
        Me.TxtHruResolution.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtHruResolution.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtHruResolution.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtHruResolution.ForeColor = System.Drawing.Color.Black
        Me.TxtHruResolution.Location = New System.Drawing.Point(112, 232)
        Me.TxtHruResolution.Name = "TxtHruResolution"
        Me.TxtHruResolution.ReadOnly = True
        Me.TxtHruResolution.Size = New System.Drawing.Size(75, 15)
        Me.TxtHruResolution.TabIndex = 106
        '
        'TextBox7
        '
        Me.TextBox7.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox7.Location = New System.Drawing.Point(3, 232)
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.ReadOnly = True
        Me.TextBox7.Size = New System.Drawing.Size(121, 15)
        Me.TextBox7.TabIndex = 105
        Me.TextBox7.Text = "HRU resolution"
        '
        'TxtHruUnits
        '
        Me.TxtHruUnits.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtHruUnits.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtHruUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtHruUnits.ForeColor = System.Drawing.Color.Black
        Me.TxtHruUnits.Location = New System.Drawing.Point(418, 233)
        Me.TxtHruUnits.Name = "TxtHruUnits"
        Me.TxtHruUnits.ReadOnly = True
        Me.TxtHruUnits.Size = New System.Drawing.Size(70, 15)
        Me.TxtHruUnits.TabIndex = 108
        '
        'TextBox8
        '
        Me.TextBox8.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox8.Location = New System.Drawing.Point(194, 232)
        Me.TextBox8.Name = "TextBox8"
        Me.TextBox8.ReadOnly = True
        Me.TextBox8.Size = New System.Drawing.Size(90, 15)
        Me.TextBox8.TabIndex = 107
        Me.TextBox8.Text = "Resample to"
        '
        'TxtHruResample
        '
        Me.TxtHruResample.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtHruResample.Location = New System.Drawing.Point(287, 230)
        Me.TxtHruResample.Name = "TxtHruResample"
        Me.TxtHruResample.Size = New System.Drawing.Size(67, 22)
        Me.TxtHruResample.TabIndex = 109
        '
        'TxtDemResample
        '
        Me.TxtDemResample.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtDemResample.Location = New System.Drawing.Point(287, 288)
        Me.TxtDemResample.Name = "TxtDemResample"
        Me.TxtDemResample.Size = New System.Drawing.Size(67, 22)
        Me.TxtDemResample.TabIndex = 116
        '
        'TxtDemUnits
        '
        Me.TxtDemUnits.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtDemUnits.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtDemUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtDemUnits.ForeColor = System.Drawing.Color.Black
        Me.TxtDemUnits.Location = New System.Drawing.Point(418, 293)
        Me.TxtDemUnits.Name = "TxtDemUnits"
        Me.TxtDemUnits.ReadOnly = True
        Me.TxtDemUnits.Size = New System.Drawing.Size(70, 15)
        Me.TxtDemUnits.TabIndex = 115
        '
        'TextBox9
        '
        Me.TextBox9.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox9.Location = New System.Drawing.Point(194, 292)
        Me.TextBox9.Name = "TextBox9"
        Me.TextBox9.ReadOnly = True
        Me.TextBox9.Size = New System.Drawing.Size(90, 15)
        Me.TextBox9.TabIndex = 114
        Me.TextBox9.Text = "Resample to"
        '
        'TxtDemResolution
        '
        Me.TxtDemResolution.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtDemResolution.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtDemResolution.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtDemResolution.ForeColor = System.Drawing.Color.Black
        Me.TxtDemResolution.Location = New System.Drawing.Point(112, 292)
        Me.TxtDemResolution.Name = "TxtDemResolution"
        Me.TxtDemResolution.ReadOnly = True
        Me.TxtDemResolution.Size = New System.Drawing.Size(75, 15)
        Me.TxtDemResolution.TabIndex = 113
        '
        'TextBox11
        '
        Me.TextBox11.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox11.Location = New System.Drawing.Point(3, 292)
        Me.TextBox11.Name = "TextBox11"
        Me.TextBox11.ReadOnly = True
        Me.TextBox11.Size = New System.Drawing.Size(121, 15)
        Me.TextBox11.TabIndex = 112
        Me.TextBox11.Text = "DEM resolution"
        '
        'TxtDemOutputName
        '
        Me.TxtDemOutputName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtDemOutputName.Location = New System.Drawing.Point(130, 260)
        Me.TxtDemOutputName.Name = "TxtDemOutputName"
        Me.TxtDemOutputName.Size = New System.Drawing.Size(499, 22)
        Me.TxtDemOutputName.TabIndex = 111
        '
        'TextBox13
        '
        Me.TextBox13.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox13.Location = New System.Drawing.Point(3, 263)
        Me.TextBox13.Name = "TextBox13"
        Me.TextBox13.ReadOnly = True
        Me.TextBox13.Size = New System.Drawing.Size(121, 15)
        Me.TextBox13.TabIndex = 110
        Me.TextBox13.Text = "DEM ASCII name"
        '
        'TextBox1
        '
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(368, 232)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(166, 15)
        Me.TextBox1.TabIndex = 117
        Me.TextBox1.Text = "Resampling technique"
        '
        'CboResampleHru
        '
        Me.CboResampleHru.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboResampleHru.FormattingEnabled = True
        Me.CboResampleHru.Location = New System.Drawing.Point(529, 230)
        Me.CboResampleHru.Name = "CboResampleHru"
        Me.CboResampleHru.Size = New System.Drawing.Size(100, 24)
        Me.CboResampleHru.TabIndex = 118
        '
        'CboResampleDem
        '
        Me.CboResampleDem.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboResampleDem.FormattingEnabled = True
        Me.CboResampleDem.Location = New System.Drawing.Point(529, 289)
        Me.CboResampleDem.Name = "CboResampleDem"
        Me.CboResampleDem.Size = New System.Drawing.Size(100, 24)
        Me.CboResampleDem.TabIndex = 120
        '
        'TextBox2
        '
        Me.TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(368, 291)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(166, 15)
        Me.TextBox2.TabIndex = 119
        Me.TextBox2.Text = "Resampling technique"
        '
        'FrmExportToAscii
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(704, 355)
        Me.Controls.Add(Me.CboResampleDem)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.CboResampleHru)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.TxtDemResample)
        Me.Controls.Add(Me.TxtDemUnits)
        Me.Controls.Add(Me.TextBox9)
        Me.Controls.Add(Me.TxtDemResolution)
        Me.Controls.Add(Me.TextBox11)
        Me.Controls.Add(Me.TxtDemOutputName)
        Me.Controls.Add(Me.TextBox13)
        Me.Controls.Add(Me.TxtHruResample)
        Me.Controls.Add(Me.TxtHruUnits)
        Me.Controls.Add(Me.TextBox8)
        Me.Controls.Add(Me.TxtHruResolution)
        Me.Controls.Add(Me.TextBox7)
        Me.Controls.Add(Me.TxtOutputFolder1)
        Me.Controls.Add(Me.TxtHruOutputName)
        Me.Controls.Add(Me.TextBox5)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.BtnExport)
        Me.Controls.Add(Me.BtnSetOutput)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.TxtNHru)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LblHruLayers)
        Me.Controls.Add(Me.LstHruLayers)
        Me.Controls.Add(Me.TxtAoiPath)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.BtnSelectAoi)
        Me.Name = "FrmExportToAscii"
        Me.ShowIcon = False
        Me.Text = "Export To ASCII"
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
    Friend WithEvents BtnClose As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents BtnSetOutput As System.Windows.Forms.Button
    Friend WithEvents BtnExport As System.Windows.Forms.Button
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox5 As System.Windows.Forms.TextBox
    Friend WithEvents TxtHruOutputName As System.Windows.Forms.TextBox
    Friend WithEvents TxtOutputFolder1 As System.Windows.Forms.TextBox
    Friend WithEvents TxtHruResolution As System.Windows.Forms.TextBox
    Friend WithEvents TextBox7 As System.Windows.Forms.TextBox
    Friend WithEvents TxtHruUnits As System.Windows.Forms.TextBox
    Friend WithEvents TextBox8 As System.Windows.Forms.TextBox
    Friend WithEvents TxtHruResample As System.Windows.Forms.TextBox
    Friend WithEvents TxtDemResample As System.Windows.Forms.TextBox
    Friend WithEvents TxtDemUnits As System.Windows.Forms.TextBox
    Friend WithEvents TextBox9 As System.Windows.Forms.TextBox
    Friend WithEvents TxtDemResolution As System.Windows.Forms.TextBox
    Friend WithEvents TextBox11 As System.Windows.Forms.TextBox
    Friend WithEvents TxtDemOutputName As System.Windows.Forms.TextBox
    Friend WithEvents TextBox13 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents CboResampleHru As System.Windows.Forms.ComboBox
    Friend WithEvents CboResampleDem As System.Windows.Forms.ComboBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
End Class
