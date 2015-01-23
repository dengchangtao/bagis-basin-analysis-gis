<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmZonalStats
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
        Me.BtnAOI = New System.Windows.Forms.Button()
        Me.LstSelectHruLayers = New System.Windows.Forms.ListBox()
        Me.LstAttributeField = New System.Windows.Forms.ListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LstAttribute = New System.Windows.Forms.Label()
        Me.LblPrismLayers = New System.Windows.Forms.Label()
        Me.LstPrismLayers = New System.Windows.Forms.ListBox()
        Me.LblDemLayers = New System.Windows.Forms.Label()
        Me.LstDemLayers = New System.Windows.Forms.ListBox()
        Me.LblRasterLayers = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.LstAoiRasterLayers = New System.Windows.Forms.ListBox()
        Me.BtnClearSelected = New System.Windows.Forms.Button()
        Me.BtnViewAoi = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.BtnStats = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ChkMin = New System.Windows.Forms.CheckBox()
        Me.ChkMedian = New System.Windows.Forms.CheckBox()
        Me.ChkMax = New System.Windows.Forms.CheckBox()
        Me.ChkMajority = New System.Windows.Forms.CheckBox()
        Me.ChkMinority = New System.Windows.Forms.CheckBox()
        Me.ChkVariety = New System.Windows.Forms.CheckBox()
        Me.ChkSum = New System.Windows.Forms.CheckBox()
        Me.ChkSTD = New System.Windows.Forms.CheckBox()
        Me.ChkRange = New System.Windows.Forms.CheckBox()
        Me.ChkMean = New System.Windows.Forms.CheckBox()
        Me.LblStatT = New System.Windows.Forms.Label()
        Me.Btnclear = New System.Windows.Forms.Button()
        Me.TxtAoiPath = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.BtnAbout = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'BtnAOI
        '
        Me.BtnAOI.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAOI.Location = New System.Drawing.Point(11, 38)
        Me.BtnAOI.Name = "BtnAOI"
        Me.BtnAOI.Size = New System.Drawing.Size(105, 28)
        Me.BtnAOI.TabIndex = 0
        Me.BtnAOI.Text = "Select AOI"
        Me.BtnAOI.UseVisualStyleBackColor = True
        '
        'LstSelectHruLayers
        '
        Me.LstSelectHruLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstSelectHruLayers.FormattingEnabled = True
        Me.LstSelectHruLayers.ItemHeight = 16
        Me.LstSelectHruLayers.Location = New System.Drawing.Point(138, 100)
        Me.LstSelectHruLayers.Name = "LstSelectHruLayers"
        Me.LstSelectHruLayers.Size = New System.Drawing.Size(140, 100)
        Me.LstSelectHruLayers.TabIndex = 2
        '
        'LstAttributeField
        '
        Me.LstAttributeField.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstAttributeField.FormattingEnabled = True
        Me.LstAttributeField.ItemHeight = 16
        Me.LstAttributeField.Location = New System.Drawing.Point(291, 100)
        Me.LstAttributeField.Name = "LstAttributeField"
        Me.LstAttributeField.SelectionMode = System.Windows.Forms.SelectionMode.None
        Me.LstAttributeField.Size = New System.Drawing.Size(140, 100)
        Me.LstAttributeField.TabIndex = 8
        Me.LstAttributeField.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(141, 80)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(116, 16)
        Me.Label1.TabIndex = 9
        Me.Label1.Text = "HRU Zone Layers"
        '
        'LstAttribute
        '
        Me.LstAttribute.AutoSize = True
        Me.LstAttribute.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstAttribute.Location = New System.Drawing.Point(294, 80)
        Me.LstAttribute.Name = "LstAttribute"
        Me.LstAttribute.Size = New System.Drawing.Size(200, 16)
        Me.LstAttribute.TabIndex = 10
        Me.LstAttribute.Text = "Attribute Fields of Selected HRU"
        '
        'LblPrismLayers
        '
        Me.LblPrismLayers.AutoSize = True
        Me.LblPrismLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblPrismLayers.Location = New System.Drawing.Point(448, 212)
        Me.LblPrismLayers.Name = "LblPrismLayers"
        Me.LblPrismLayers.Size = New System.Drawing.Size(92, 16)
        Me.LblPrismLayers.TabIndex = 39
        Me.LblPrismLayers.Text = "PRISM Precip"
        '
        'LstPrismLayers
        '
        Me.LstPrismLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstPrismLayers.FormattingEnabled = True
        Me.LstPrismLayers.ItemHeight = 16
        Me.LstPrismLayers.Location = New System.Drawing.Point(445, 232)
        Me.LstPrismLayers.Name = "LstPrismLayers"
        Me.LstPrismLayers.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.LstPrismLayers.Size = New System.Drawing.Size(140, 100)
        Me.LstPrismLayers.TabIndex = 5
        '
        'LblDemLayers
        '
        Me.LblDemLayers.AutoSize = True
        Me.LblDemLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblDemLayers.Location = New System.Drawing.Point(295, 212)
        Me.LblDemLayers.Name = "LblDemLayers"
        Me.LblDemLayers.Size = New System.Drawing.Size(38, 16)
        Me.LblDemLayers.TabIndex = 37
        Me.LblDemLayers.Text = "DEM"
        '
        'LstDemLayers
        '
        Me.LstDemLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstDemLayers.FormattingEnabled = True
        Me.LstDemLayers.ItemHeight = 16
        Me.LstDemLayers.Location = New System.Drawing.Point(292, 232)
        Me.LstDemLayers.Name = "LstDemLayers"
        Me.LstDemLayers.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.LstDemLayers.Size = New System.Drawing.Size(140, 100)
        Me.LstDemLayers.TabIndex = 4
        '
        'LblRasterLayers
        '
        Me.LblRasterLayers.AutoSize = True
        Me.LblRasterLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRasterLayers.Location = New System.Drawing.Point(-127, 144)
        Me.LblRasterLayers.Name = "LblRasterLayers"
        Me.LblRasterLayers.Size = New System.Drawing.Size(105, 16)
        Me.LblRasterLayers.TabIndex = 35
        Me.LblRasterLayers.Text = "Raster Layers"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(142, 212)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 16)
        Me.Label2.TabIndex = 41
        Me.Label2.Text = "Raster"
        '
        'LstAoiRasterLayers
        '
        Me.LstAoiRasterLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstAoiRasterLayers.FormattingEnabled = True
        Me.LstAoiRasterLayers.ItemHeight = 16
        Me.LstAoiRasterLayers.Location = New System.Drawing.Point(139, 232)
        Me.LstAoiRasterLayers.Name = "LstAoiRasterLayers"
        Me.LstAoiRasterLayers.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.LstAoiRasterLayers.Size = New System.Drawing.Size(140, 100)
        Me.LstAoiRasterLayers.TabIndex = 3
        '
        'BtnClearSelected
        '
        Me.BtnClearSelected.Enabled = False
        Me.BtnClearSelected.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClearSelected.Location = New System.Drawing.Point(11, 275)
        Me.BtnClearSelected.Name = "BtnClearSelected"
        Me.BtnClearSelected.Size = New System.Drawing.Size(105, 28)
        Me.BtnClearSelected.TabIndex = 7
        Me.BtnClearSelected.Text = "Clear Selected"
        Me.BtnClearSelected.UseVisualStyleBackColor = True
        '
        'BtnViewAoi
        '
        Me.BtnViewAoi.Enabled = False
        Me.BtnViewAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnViewAoi.Location = New System.Drawing.Point(11, 241)
        Me.BtnViewAoi.Name = "BtnViewAoi"
        Me.BtnViewAoi.Size = New System.Drawing.Size(105, 28)
        Me.BtnViewAoi.TabIndex = 6
        Me.BtnViewAoi.Text = "View Selected"
        Me.BtnViewAoi.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(13, 213)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(118, 20)
        Me.Label4.TabIndex = 45
        Me.Label4.Text = "Value Layers:"
        '
        'BtnStats
        '
        Me.BtnStats.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnStats.Location = New System.Drawing.Point(477, 434)
        Me.BtnStats.Name = "BtnStats"
        Me.BtnStats.Size = New System.Drawing.Size(108, 33)
        Me.BtnStats.TabIndex = 1
        Me.BtnStats.Text = "Calculate"
        Me.BtnStats.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.ChkMin)
        Me.Panel1.Controls.Add(Me.ChkMedian)
        Me.Panel1.Controls.Add(Me.ChkMax)
        Me.Panel1.Controls.Add(Me.ChkMajority)
        Me.Panel1.Controls.Add(Me.ChkMinority)
        Me.Panel1.Controls.Add(Me.ChkVariety)
        Me.Panel1.Controls.Add(Me.ChkSum)
        Me.Panel1.Controls.Add(Me.ChkSTD)
        Me.Panel1.Controls.Add(Me.ChkRange)
        Me.Panel1.Controls.Add(Me.ChkMean)
        Me.Panel1.Location = New System.Drawing.Point(17, 358)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(587, 61)
        Me.Panel1.TabIndex = 48
        '
        'ChkMin
        '
        Me.ChkMin.AutoSize = True
        Me.ChkMin.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkMin.Location = New System.Drawing.Point(3, 33)
        Me.ChkMin.Name = "ChkMin"
        Me.ChkMin.Size = New System.Drawing.Size(73, 20)
        Me.ChkMin.TabIndex = 13
        Me.ChkMin.Text = "Min (MI)"
        Me.ChkMin.UseVisualStyleBackColor = True
        '
        'ChkMedian
        '
        Me.ChkMedian.AutoSize = True
        Me.ChkMedian.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkMedian.Location = New System.Drawing.Point(108, 7)
        Me.ChkMedian.Name = "ChkMedian"
        Me.ChkMedian.Size = New System.Drawing.Size(104, 20)
        Me.ChkMedian.TabIndex = 9
        Me.ChkMedian.Text = "Median (MD)"
        Me.ChkMedian.UseVisualStyleBackColor = True
        '
        'ChkMax
        '
        Me.ChkMax.AutoSize = True
        Me.ChkMax.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkMax.Location = New System.Drawing.Point(108, 33)
        Me.ChkMax.Name = "ChkMax"
        Me.ChkMax.Size = New System.Drawing.Size(82, 20)
        Me.ChkMax.TabIndex = 14
        Me.ChkMax.Text = "Max (MX)"
        Me.ChkMax.UseVisualStyleBackColor = True
        '
        'ChkMajority
        '
        Me.ChkMajority.AutoSize = True
        Me.ChkMajority.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkMajority.Location = New System.Drawing.Point(334, 7)
        Me.ChkMajority.Name = "ChkMajority"
        Me.ChkMajority.Size = New System.Drawing.Size(103, 20)
        Me.ChkMajority.TabIndex = 11
        Me.ChkMajority.Text = "Majority (MJ)"
        Me.ChkMajority.UseVisualStyleBackColor = True
        '
        'ChkMinority
        '
        Me.ChkMinority.AutoSize = True
        Me.ChkMinority.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkMinority.Location = New System.Drawing.Point(219, 7)
        Me.ChkMinority.Name = "ChkMinority"
        Me.ChkMinority.Size = New System.Drawing.Size(104, 20)
        Me.ChkMinority.TabIndex = 10
        Me.ChkMinority.Text = "Minority (MT)"
        Me.ChkMinority.UseVisualStyleBackColor = True
        '
        'ChkVariety
        '
        Me.ChkVariety.AutoSize = True
        Me.ChkVariety.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkVariety.Location = New System.Drawing.Point(452, 7)
        Me.ChkVariety.Name = "ChkVariety"
        Me.ChkVariety.Size = New System.Drawing.Size(99, 20)
        Me.ChkVariety.TabIndex = 12
        Me.ChkVariety.Text = "Variety (VR)"
        Me.ChkVariety.UseVisualStyleBackColor = True
        '
        'ChkSum
        '
        Me.ChkSum.AutoSize = True
        Me.ChkSum.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkSum.Location = New System.Drawing.Point(452, 33)
        Me.ChkSum.Name = "ChkSum"
        Me.ChkSum.Size = New System.Drawing.Size(84, 20)
        Me.ChkSum.TabIndex = 17
        Me.ChkSum.Text = "Sum (SU)"
        Me.ChkSum.UseVisualStyleBackColor = True
        '
        'ChkSTD
        '
        Me.ChkSTD.AutoSize = True
        Me.ChkSTD.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkSTD.Location = New System.Drawing.Point(334, 33)
        Me.ChkSTD.Name = "ChkSTD"
        Me.ChkSTD.Size = New System.Drawing.Size(84, 20)
        Me.ChkSTD.TabIndex = 16
        Me.ChkSTD.Text = "STD (ST)"
        Me.ChkSTD.UseVisualStyleBackColor = True
        '
        'ChkRange
        '
        Me.ChkRange.AutoSize = True
        Me.ChkRange.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkRange.Location = New System.Drawing.Point(219, 33)
        Me.ChkRange.Name = "ChkRange"
        Me.ChkRange.Size = New System.Drawing.Size(99, 20)
        Me.ChkRange.TabIndex = 15
        Me.ChkRange.Text = "Range (RG)"
        Me.ChkRange.UseVisualStyleBackColor = True
        '
        'ChkMean
        '
        Me.ChkMean.AutoSize = True
        Me.ChkMean.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkMean.Location = New System.Drawing.Point(3, 7)
        Me.ChkMean.Name = "ChkMean"
        Me.ChkMean.Size = New System.Drawing.Size(93, 20)
        Me.ChkMean.TabIndex = 8
        Me.ChkMean.Text = "Mean (MN)"
        Me.ChkMean.UseVisualStyleBackColor = True
        '
        'LblStatT
        '
        Me.LblStatT.AutoSize = True
        Me.LblStatT.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblStatT.Location = New System.Drawing.Point(13, 335)
        Me.LblStatT.Name = "LblStatT"
        Me.LblStatT.Size = New System.Drawing.Size(132, 20)
        Me.LblStatT.TabIndex = 1
        Me.LblStatT.Text = "Statistics Type:"
        '
        'Btnclear
        '
        Me.Btnclear.Enabled = False
        Me.Btnclear.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Btnclear.Location = New System.Drawing.Point(11, 110)
        Me.Btnclear.Name = "Btnclear"
        Me.Btnclear.Size = New System.Drawing.Size(105, 28)
        Me.Btnclear.TabIndex = 49
        Me.Btnclear.Text = "Clear Selected"
        Me.Btnclear.UseVisualStyleBackColor = True
        '
        'TxtAoiPath
        '
        Me.TxtAoiPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtAoiPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtAoiPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtAoiPath.Location = New System.Drawing.Point(191, 41)
        Me.TxtAoiPath.Name = "TxtAoiPath"
        Me.TxtAoiPath.ReadOnly = True
        Me.TxtAoiPath.Size = New System.Drawing.Size(413, 22)
        Me.TxtAoiPath.TabIndex = 51
        Me.TxtAoiPath.TabStop = False
        Me.TxtAoiPath.Text = "AOI is not specified"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(122, 44)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(63, 16)
        Me.Label5.TabIndex = 50
        Me.Label5.Text = "AOI Path:"
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(346, 436)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(108, 31)
        Me.BtnCancel.TabIndex = 19
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(12, 9)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(481, 20)
        Me.Label6.TabIndex = 54
        Me.Label6.Text = "Calculate zonal statistics to the HRU zone attribute tables."
        '
        'BtnAbout
        '
        Me.BtnAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAbout.Location = New System.Drawing.Point(499, 6)
        Me.BtnAbout.Name = "BtnAbout"
        Me.BtnAbout.Size = New System.Drawing.Size(105, 28)
        Me.BtnAbout.TabIndex = 20
        Me.BtnAbout.Text = "Tell me more"
        Me.BtnAbout.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(13, 81)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(104, 20)
        Me.Label3.TabIndex = 56
        Me.Label3.Text = "Zone Layer:"
        '
        'FrmZonalStats
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.BtnAbout)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.TxtAoiPath)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Btnclear)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.BtnStats)
        Me.Controls.Add(Me.LblStatT)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.BtnClearSelected)
        Me.Controls.Add(Me.BtnViewAoi)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.LstAoiRasterLayers)
        Me.Controls.Add(Me.LblPrismLayers)
        Me.Controls.Add(Me.LstPrismLayers)
        Me.Controls.Add(Me.LblDemLayers)
        Me.Controls.Add(Me.LstDemLayers)
        Me.Controls.Add(Me.LblRasterLayers)
        Me.Controls.Add(Me.LstAttribute)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LstAttributeField)
        Me.Controls.Add(Me.LstSelectHruLayers)
        Me.Controls.Add(Me.BtnAOI)
        Me.Name = "FrmZonalStats"
        Me.Size = New System.Drawing.Size(620, 480)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnAOI As System.Windows.Forms.Button
    Friend WithEvents LstSelectHruLayers As System.Windows.Forms.ListBox
    Friend WithEvents LstAttributeField As System.Windows.Forms.ListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents LstAttribute As System.Windows.Forms.Label
    Friend WithEvents LblPrismLayers As System.Windows.Forms.Label
    Friend WithEvents LstPrismLayers As System.Windows.Forms.ListBox
    Friend WithEvents LblDemLayers As System.Windows.Forms.Label
    Friend WithEvents LstDemLayers As System.Windows.Forms.ListBox
    Friend WithEvents LblRasterLayers As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents LstAoiRasterLayers As System.Windows.Forms.ListBox
    Friend WithEvents BtnClearSelected As System.Windows.Forms.Button
    Friend WithEvents BtnViewAoi As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents BtnStats As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ChkMean As System.Windows.Forms.CheckBox
    Friend WithEvents LblStatT As System.Windows.Forms.Label
    Friend WithEvents Btnclear As System.Windows.Forms.Button
    Friend WithEvents ChkMin As System.Windows.Forms.CheckBox
    Friend WithEvents ChkMedian As System.Windows.Forms.CheckBox
    Friend WithEvents ChkMax As System.Windows.Forms.CheckBox
    Friend WithEvents ChkMajority As System.Windows.Forms.CheckBox
    Friend WithEvents ChkMinority As System.Windows.Forms.CheckBox
    Friend WithEvents ChkVariety As System.Windows.Forms.CheckBox
    Friend WithEvents ChkSum As System.Windows.Forms.CheckBox
    Friend WithEvents ChkSTD As System.Windows.Forms.CheckBox
    Friend WithEvents ChkRange As System.Windows.Forms.CheckBox
    Friend WithEvents TxtAoiPath As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents BtnAbout As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label

End Class