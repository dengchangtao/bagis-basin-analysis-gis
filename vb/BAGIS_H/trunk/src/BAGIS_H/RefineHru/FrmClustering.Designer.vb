<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmClustering
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
        Me.BtnViewAoi = New System.Windows.Forms.Button
        Me.BtnClearSelected = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.BtnAOI = New System.Windows.Forms.Button
        Me.TxtAoiPath = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.LstAoiRasterLayers = New System.Windows.Forms.ListBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.LstDemLayers = New System.Windows.Forms.ListBox
        Me.LblPrismLayers = New System.Windows.Forms.Label
        Me.LstPrismLayers = New System.Windows.Forms.ListBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.TxtClass = New System.Windows.Forms.TextBox
        Me.TxtIterate = New System.Windows.Forms.TextBox
        Me.TxtSize = New System.Windows.Forms.TextBox
        Me.TxtSample = New System.Windows.Forms.TextBox
        Me.TxtSignature = New System.Windows.Forms.TextBox
        Me.BtnCalIsoCluster = New System.Windows.Forms.Button
        Me.BtnCancel = New System.Windows.Forms.Button
        Me.Label10 = New System.Windows.Forms.Label
        Me.LblToolTip = New System.Windows.Forms.Label
        Me.BtnAbout = New System.Windows.Forms.Button
        Me.TxtOutputPath = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'BtnViewAoi
        '
        Me.BtnViewAoi.Enabled = False
        Me.BtnViewAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnViewAoi.Location = New System.Drawing.Point(15, 159)
        Me.BtnViewAoi.Name = "BtnViewAoi"
        Me.BtnViewAoi.Size = New System.Drawing.Size(119, 30)
        Me.BtnViewAoi.TabIndex = 3
        Me.BtnViewAoi.Text = "View Selected"
        Me.BtnViewAoi.UseVisualStyleBackColor = True
        '
        'BtnClearSelected
        '
        Me.BtnClearSelected.Enabled = False
        Me.BtnClearSelected.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClearSelected.Location = New System.Drawing.Point(15, 212)
        Me.BtnClearSelected.Name = "BtnClearSelected"
        Me.BtnClearSelected.Size = New System.Drawing.Size(119, 30)
        Me.BtnClearSelected.TabIndex = 64
        Me.BtnClearSelected.Text = "Clear Selected"
        Me.BtnClearSelected.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(143, 59)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(63, 16)
        Me.Label5.TabIndex = 67
        Me.Label5.Text = "AOI Path:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'BtnAOI
        '
        Me.BtnAOI.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAOI.Location = New System.Drawing.Point(11, 52)
        Me.BtnAOI.Name = "BtnAOI"
        Me.BtnAOI.Size = New System.Drawing.Size(119, 30)
        Me.BtnAOI.TabIndex = 66
        Me.BtnAOI.Text = "Select AOI"
        Me.BtnAOI.UseVisualStyleBackColor = True
        '
        'TxtAoiPath
        '
        Me.TxtAoiPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtAoiPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtAoiPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtAoiPath.Location = New System.Drawing.Point(212, 56)
        Me.TxtAoiPath.Name = "TxtAoiPath"
        Me.TxtAoiPath.ReadOnly = True
        Me.TxtAoiPath.Size = New System.Drawing.Size(376, 22)
        Me.TxtAoiPath.TabIndex = 68
        Me.TxtAoiPath.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(1, 110)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(152, 20)
        Me.Label1.TabIndex = 69
        Me.Label1.Text = "Input Raster Bands:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(177, 91)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(95, 16)
        Me.Label2.TabIndex = 71
        Me.Label2.Text = "Raster Grids"
        '
        'LstAoiRasterLayers
        '
        Me.LstAoiRasterLayers.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.LstAoiRasterLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstAoiRasterLayers.FormattingEnabled = True
        Me.LstAoiRasterLayers.ItemHeight = 16
        Me.LstAoiRasterLayers.Location = New System.Drawing.Point(153, 110)
        Me.LstAoiRasterLayers.Name = "LstAoiRasterLayers"
        Me.LstAoiRasterLayers.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.LstAoiRasterLayers.Size = New System.Drawing.Size(140, 132)
        Me.LstAoiRasterLayers.TabIndex = 70
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(331, 91)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 16)
        Me.Label4.TabIndex = 73
        Me.Label4.Text = "DEM Grids"
        '
        'LstDemLayers
        '
        Me.LstDemLayers.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.LstDemLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstDemLayers.FormattingEnabled = True
        Me.LstDemLayers.ItemHeight = 16
        Me.LstDemLayers.Location = New System.Drawing.Point(302, 110)
        Me.LstDemLayers.Name = "LstDemLayers"
        Me.LstDemLayers.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.LstDemLayers.Size = New System.Drawing.Size(140, 132)
        Me.LstDemLayers.TabIndex = 72
        '
        'LblPrismLayers
        '
        Me.LblPrismLayers.AutoSize = True
        Me.LblPrismLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblPrismLayers.Location = New System.Drawing.Point(445, 91)
        Me.LblPrismLayers.Name = "LblPrismLayers"
        Me.LblPrismLayers.Size = New System.Drawing.Size(145, 16)
        Me.LblPrismLayers.TabIndex = 75
        Me.LblPrismLayers.Text = "PRISM Precip Grids"
        '
        'LstPrismLayers
        '
        Me.LstPrismLayers.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.LstPrismLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstPrismLayers.FormattingEnabled = True
        Me.LstPrismLayers.ItemHeight = 16
        Me.LstPrismLayers.Location = New System.Drawing.Point(448, 110)
        Me.LstPrismLayers.Name = "LstPrismLayers"
        Me.LstPrismLayers.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.LstPrismLayers.Size = New System.Drawing.Size(140, 132)
        Me.LstPrismLayers.TabIndex = 74
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(8, 263)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(111, 16)
        Me.Label3.TabIndex = 76
        Me.Label3.Text = "No. Of Classes"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(359, 263)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(149, 16)
        Me.Label6.TabIndex = 77
        Me.Label6.Text = "Minimum Class Size "
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(180, 263)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(119, 16)
        Me.Label7.TabIndex = 78
        Me.Label7.Text = "No. Of Iterations"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(8, 300)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(128, 16)
        Me.Label8.TabIndex = 79
        Me.Label8.Text = "Sampling Interval"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(205, 300)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(152, 16)
        Me.Label9.TabIndex = 80
        Me.Label9.Text = "Output Signature File"
        '
        'TxtClass
        '
        Me.TxtClass.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtClass.Location = New System.Drawing.Point(120, 260)
        Me.TxtClass.Name = "TxtClass"
        Me.TxtClass.Size = New System.Drawing.Size(54, 22)
        Me.TxtClass.TabIndex = 81
        '
        'TxtIterate
        '
        Me.TxtIterate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtIterate.Location = New System.Drawing.Point(302, 257)
        Me.TxtIterate.Name = "TxtIterate"
        Me.TxtIterate.Size = New System.Drawing.Size(51, 22)
        Me.TxtIterate.TabIndex = 82
        '
        'TxtSize
        '
        Me.TxtSize.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtSize.Location = New System.Drawing.Point(505, 257)
        Me.TxtSize.Name = "TxtSize"
        Me.TxtSize.Size = New System.Drawing.Size(58, 22)
        Me.TxtSize.TabIndex = 83
        '
        'TxtSample
        '
        Me.TxtSample.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtSample.Location = New System.Drawing.Point(142, 297)
        Me.TxtSample.Name = "TxtSample"
        Me.TxtSample.Size = New System.Drawing.Size(47, 22)
        Me.TxtSample.TabIndex = 84
        '
        'TxtSignature
        '
        Me.TxtSignature.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtSignature.Location = New System.Drawing.Point(358, 297)
        Me.TxtSignature.Name = "TxtSignature"
        Me.TxtSignature.Size = New System.Drawing.Size(146, 22)
        Me.TxtSignature.TabIndex = 85
        '
        'BtnCalIsoCluster
        '
        Me.BtnCalIsoCluster.Enabled = False
        Me.BtnCalIsoCluster.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCalIsoCluster.Location = New System.Drawing.Point(407, 373)
        Me.BtnCalIsoCluster.Name = "BtnCalIsoCluster"
        Me.BtnCalIsoCluster.Size = New System.Drawing.Size(183, 30)
        Me.BtnCalIsoCluster.TabIndex = 86
        Me.BtnCalIsoCluster.Text = "Calculate Isocluster"
        Me.BtnCalIsoCluster.UseVisualStyleBackColor = True
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(277, 373)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(108, 30)
        Me.BtnCancel.TabIndex = 87
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.Red
        Me.Label10.Location = New System.Drawing.Point(205, 322)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(294, 16)
        Me.Label10.TabIndex = 88
        Me.Label10.Text = "Output Signature file need .gsg extension "
        '
        'LblToolTip
        '
        Me.LblToolTip.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblToolTip.Location = New System.Drawing.Point(12, 18)
        Me.LblToolTip.Name = "LblToolTip"
        Me.LblToolTip.Size = New System.Drawing.Size(281, 16)
        Me.LblToolTip.TabIndex = 90
        Me.LblToolTip.Text = "Dissolve adjacent HRU's of same type"
        '
        'BtnAbout
        '
        Me.BtnAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAbout.Location = New System.Drawing.Point(293, 10)
        Me.BtnAbout.Name = "BtnAbout"
        Me.BtnAbout.Size = New System.Drawing.Size(105, 28)
        Me.BtnAbout.TabIndex = 89
        Me.BtnAbout.Text = "Tell me more"
        Me.BtnAbout.UseVisualStyleBackColor = True
        '
        'TxtOutputPath
        '
        Me.TxtOutputPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtOutputPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtOutputPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtOutputPath.Location = New System.Drawing.Point(85, 341)
        Me.TxtOutputPath.Name = "TxtOutputPath"
        Me.TxtOutputPath.ReadOnly = True
        Me.TxtOutputPath.Size = New System.Drawing.Size(503, 22)
        Me.TxtOutputPath.TabIndex = 92
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(8, 344)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(79, 16)
        Me.Label11.TabIndex = 91
        Me.Label11.Text = "Output Path:"
        '
        'FrmClustering
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(602, 411)
        Me.Controls.Add(Me.TxtOutputPath)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.LblToolTip)
        Me.Controls.Add(Me.BtnAbout)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnCalIsoCluster)
        Me.Controls.Add(Me.TxtSignature)
        Me.Controls.Add(Me.TxtSample)
        Me.Controls.Add(Me.TxtSize)
        Me.Controls.Add(Me.TxtIterate)
        Me.Controls.Add(Me.TxtClass)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.LblPrismLayers)
        Me.Controls.Add(Me.LstPrismLayers)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.LstDemLayers)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.LstAoiRasterLayers)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TxtAoiPath)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.BtnAOI)
        Me.Controls.Add(Me.BtnClearSelected)
        Me.Controls.Add(Me.BtnViewAoi)
        Me.Name = "FrmClustering"
        Me.ShowIcon = False
        Me.Text = "Iso Cluster"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnViewAoi As System.Windows.Forms.Button
    Friend WithEvents BtnClearSelected As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents BtnAOI As System.Windows.Forms.Button
    Friend WithEvents TxtAoiPath As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents LstAoiRasterLayers As System.Windows.Forms.ListBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents LstDemLayers As System.Windows.Forms.ListBox
    Friend WithEvents LblPrismLayers As System.Windows.Forms.Label
    Friend WithEvents LstPrismLayers As System.Windows.Forms.ListBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TxtClass As System.Windows.Forms.TextBox
    Friend WithEvents TxtIterate As System.Windows.Forms.TextBox
    Friend WithEvents TxtSize As System.Windows.Forms.TextBox
    Friend WithEvents TxtSample As System.Windows.Forms.TextBox
    Friend WithEvents TxtSignature As System.Windows.Forms.TextBox
    Friend WithEvents BtnCalIsoCluster As System.Windows.Forms.Button
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents LblToolTip As System.Windows.Forms.Label
    Friend WithEvents BtnAbout As System.Windows.Forms.Button
    Friend WithEvents TxtOutputPath As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
End Class