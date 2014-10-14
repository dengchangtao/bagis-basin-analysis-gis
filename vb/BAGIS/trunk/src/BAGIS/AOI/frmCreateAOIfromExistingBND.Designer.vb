<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCreateAOIfromExistingBND
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Opt10Meters = New System.Windows.Forms.RadioButton()
        Me.Opt30Meters = New System.Windows.Forms.RadioButton()
        Me.lblWhy = New System.Windows.Forms.Label()
        Me.GrpBoxFilter = New System.Windows.Forms.GroupBox()
        Me.txtWidth = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtHeight = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ChkSmoothDEM = New System.Windows.Forms.CheckBox()
        Me.txtZFactor = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.grpboxPRISMUnit = New System.Windows.Forms.GroupBox()
        Me.rbtnDepthInch = New System.Windows.Forms.RadioButton()
        Me.rbtnDepthMM = New System.Windows.Forms.RadioButton()
        Me.lblSlopeUnit = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblDEMUnit = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmbRun = New System.Windows.Forms.Button()
        Me.lblBufferUnit = New System.Windows.Forms.Label()
        Me.txtBufferD = New System.Windows.Forms.TextBox()
        Me.lblBufferD = New System.Windows.Forms.Label()
        Me.ChkAOIBuffer = New System.Windows.Forms.CheckBox()
        Me.btnSelectWorkspace = New System.Windows.Forms.Button()
        Me.txtOutputName = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtOutputWorkspace = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtSourceData = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbSelectAll = New System.Windows.Forms.Button()
        Me.cmbSelectNone = New System.Windows.Forms.Button()
        Me.ChkDEMExtent = New System.Windows.Forms.CheckBox()
        Me.ChkFilledDEM = New System.Windows.Forms.CheckBox()
        Me.ChkFlowDir = New System.Windows.Forms.CheckBox()
        Me.ChkFlowAccum = New System.Windows.Forms.CheckBox()
        Me.ChkSlope = New System.Windows.Forms.CheckBox()
        Me.ChkAspect = New System.Windows.Forms.CheckBox()
        Me.ChkHillshade = New System.Windows.Forms.CheckBox()
        Me.GroupBox1.SuspendLayout()
        Me.GrpBoxFilter.SuspendLayout()
        Me.grpboxPRISMUnit.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Opt10Meters)
        Me.GroupBox1.Controls.Add(Me.Opt30Meters)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(35, 122)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(200, 76)
        Me.GroupBox1.TabIndex = 12
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Source DEM:"
        '
        'Opt10Meters
        '
        Me.Opt10Meters.AutoSize = True
        Me.Opt10Meters.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Opt10Meters.Location = New System.Drawing.Point(47, 25)
        Me.Opt10Meters.Name = "Opt10Meters"
        Me.Opt10Meters.Size = New System.Drawing.Size(98, 24)
        Me.Opt10Meters.TabIndex = 1
        Me.Opt10Meters.Text = "10 meters"
        Me.Opt10Meters.UseVisualStyleBackColor = True
        '
        'Opt30Meters
        '
        Me.Opt30Meters.AutoSize = True
        Me.Opt30Meters.Checked = True
        Me.Opt30Meters.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Opt30Meters.Location = New System.Drawing.Point(47, 47)
        Me.Opt30Meters.Name = "Opt30Meters"
        Me.Opt30Meters.Size = New System.Drawing.Size(98, 24)
        Me.Opt30Meters.TabIndex = 1
        Me.Opt30Meters.TabStop = True
        Me.Opt30Meters.Text = "30 meters"
        Me.Opt30Meters.UseVisualStyleBackColor = True
        '
        'lblWhy
        '
        Me.lblWhy.AutoSize = True
        Me.lblWhy.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWhy.ForeColor = System.Drawing.Color.Blue
        Me.lblWhy.Location = New System.Drawing.Point(403, 123)
        Me.lblWhy.Name = "lblWhy"
        Me.lblWhy.Size = New System.Drawing.Size(56, 16)
        Me.lblWhy.TabIndex = 30
        Me.lblWhy.Text = "(Why?)"
        '
        'GrpBoxFilter
        '
        Me.GrpBoxFilter.Controls.Add(Me.txtWidth)
        Me.GrpBoxFilter.Controls.Add(Me.Label4)
        Me.GrpBoxFilter.Controls.Add(Me.txtHeight)
        Me.GrpBoxFilter.Controls.Add(Me.Label2)
        Me.GrpBoxFilter.Enabled = False
        Me.GrpBoxFilter.Location = New System.Drawing.Point(301, 148)
        Me.GrpBoxFilter.Name = "GrpBoxFilter"
        Me.GrpBoxFilter.Size = New System.Drawing.Size(213, 48)
        Me.GrpBoxFilter.TabIndex = 29
        Me.GrpBoxFilter.TabStop = False
        Me.GrpBoxFilter.Text = "Filter Size (pixels)"
        '
        'txtWidth
        '
        Me.txtWidth.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtWidth.Location = New System.Drawing.Point(150, 20)
        Me.txtWidth.Name = "txtWidth"
        Me.txtWidth.Size = New System.Drawing.Size(38, 22)
        Me.txtWidth.TabIndex = 3
        Me.txtWidth.Text = "7"
        Me.txtWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(20, 25)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(38, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Height"
        '
        'txtHeight
        '
        Me.txtHeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHeight.Location = New System.Drawing.Point(59, 20)
        Me.txtHeight.Name = "txtHeight"
        Me.txtHeight.Size = New System.Drawing.Size(38, 22)
        Me.txtHeight.TabIndex = 1
        Me.txtHeight.Text = "3"
        Me.txtHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(109, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(35, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Width"
        '
        'ChkSmoothDEM
        '
        Me.ChkSmoothDEM.AutoSize = True
        Me.ChkSmoothDEM.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkSmoothDEM.Location = New System.Drawing.Point(301, 122)
        Me.ChkSmoothDEM.Name = "ChkSmoothDEM"
        Me.ChkSmoothDEM.Size = New System.Drawing.Size(106, 20)
        Me.ChkSmoothDEM.TabIndex = 28
        Me.ChkSmoothDEM.Text = "Smooth DEM"
        Me.ChkSmoothDEM.UseVisualStyleBackColor = True
        '
        'txtZFactor
        '
        Me.txtZFactor.Location = New System.Drawing.Point(170, 457)
        Me.txtZFactor.Name = "txtZFactor"
        Me.txtZFactor.Size = New System.Drawing.Size(67, 20)
        Me.txtZFactor.TabIndex = 27
        Me.txtZFactor.Text = "1"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(35, 457)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(115, 16)
        Me.Label3.TabIndex = 16
        Me.Label3.Text = "Hillshade Z factor:"
        '
        'grpboxPRISMUnit
        '
        Me.grpboxPRISMUnit.Controls.Add(Me.rbtnDepthInch)
        Me.grpboxPRISMUnit.Controls.Add(Me.rbtnDepthMM)
        Me.grpboxPRISMUnit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpboxPRISMUnit.Location = New System.Drawing.Point(311, 350)
        Me.grpboxPRISMUnit.Name = "grpboxPRISMUnit"
        Me.grpboxPRISMUnit.Size = New System.Drawing.Size(238, 54)
        Me.grpboxPRISMUnit.TabIndex = 40
        Me.grpboxPRISMUnit.TabStop = False
        Me.grpboxPRISMUnit.Text = "PRISM Depth Unit:"
        '
        'rbtnDepthInch
        '
        Me.rbtnDepthInch.AutoSize = True
        Me.rbtnDepthInch.Checked = True
        Me.rbtnDepthInch.ForeColor = System.Drawing.Color.Blue
        Me.rbtnDepthInch.Location = New System.Drawing.Point(57, 25)
        Me.rbtnDepthInch.Name = "rbtnDepthInch"
        Me.rbtnDepthInch.Size = New System.Drawing.Size(65, 20)
        Me.rbtnDepthInch.TabIndex = 1
        Me.rbtnDepthInch.TabStop = True
        Me.rbtnDepthInch.Text = "Inches"
        Me.rbtnDepthInch.UseVisualStyleBackColor = True
        '
        'rbtnDepthMM
        '
        Me.rbtnDepthMM.AutoSize = True
        Me.rbtnDepthMM.ForeColor = System.Drawing.Color.Blue
        Me.rbtnDepthMM.Location = New System.Drawing.Point(138, 25)
        Me.rbtnDepthMM.Name = "rbtnDepthMM"
        Me.rbtnDepthMM.Size = New System.Drawing.Size(90, 20)
        Me.rbtnDepthMM.TabIndex = 0
        Me.rbtnDepthMM.Text = "Millimeters"
        Me.rbtnDepthMM.UseVisualStyleBackColor = True
        '
        'lblSlopeUnit
        '
        Me.lblSlopeUnit.AutoSize = True
        Me.lblSlopeUnit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSlopeUnit.ForeColor = System.Drawing.Color.Black
        Me.lblSlopeUnit.Location = New System.Drawing.Point(440, 327)
        Me.lblSlopeUnit.Name = "lblSlopeUnit"
        Me.lblSlopeUnit.Size = New System.Drawing.Size(63, 16)
        Me.lblSlopeUnit.TabIndex = 39
        Me.lblSlopeUnit.Text = "Unknown"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(361, 327)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(73, 16)
        Me.Label5.TabIndex = 38
        Me.Label5.Text = "Slope Unit:"
        '
        'lblDEMUnit
        '
        Me.lblDEMUnit.AutoSize = True
        Me.lblDEMUnit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDEMUnit.ForeColor = System.Drawing.Color.Black
        Me.lblDEMUnit.Location = New System.Drawing.Point(440, 303)
        Me.lblDEMUnit.Name = "lblDEMUnit"
        Me.lblDEMUnit.Size = New System.Drawing.Size(63, 16)
        Me.lblDEMUnit.TabIndex = 37
        Me.lblDEMUnit.Text = "Unknown"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(308, 303)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(126, 16)
        Me.Label6.TabIndex = 36
        Me.Label6.Text = "DEM Elevation Unit:"
        '
        'cmbRun
        '
        Me.cmbRun.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbRun.Location = New System.Drawing.Point(350, 430)
        Me.cmbRun.Name = "cmbRun"
        Me.cmbRun.Size = New System.Drawing.Size(139, 31)
        Me.cmbRun.TabIndex = 35
        Me.cmbRun.Text = "Generate AOI"
        Me.cmbRun.UseVisualStyleBackColor = True
        '
        'lblBufferUnit
        '
        Me.lblBufferUnit.AutoSize = True
        Me.lblBufferUnit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBufferUnit.Location = New System.Drawing.Point(493, 261)
        Me.lblBufferUnit.Name = "lblBufferUnit"
        Me.lblBufferUnit.Size = New System.Drawing.Size(49, 16)
        Me.lblBufferUnit.TabIndex = 32
        Me.lblBufferUnit.Text = "Meters"
        '
        'txtBufferD
        '
        Me.txtBufferD.Location = New System.Drawing.Point(419, 257)
        Me.txtBufferD.Name = "txtBufferD"
        Me.txtBufferD.Size = New System.Drawing.Size(68, 20)
        Me.txtBufferD.TabIndex = 34
        Me.txtBufferD.Text = "100"
        Me.txtBufferD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblBufferD
        '
        Me.lblBufferD.AutoSize = True
        Me.lblBufferD.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBufferD.Location = New System.Drawing.Point(317, 261)
        Me.lblBufferD.Name = "lblBufferD"
        Me.lblBufferD.Size = New System.Drawing.Size(101, 16)
        Me.lblBufferD.TabIndex = 33
        Me.lblBufferD.Text = "Buffer Distance:"
        '
        'ChkAOIBuffer
        '
        Me.ChkAOIBuffer.AutoSize = True
        Me.ChkAOIBuffer.Checked = True
        Me.ChkAOIBuffer.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkAOIBuffer.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkAOIBuffer.Location = New System.Drawing.Point(301, 228)
        Me.ChkAOIBuffer.Name = "ChkAOIBuffer"
        Me.ChkAOIBuffer.Size = New System.Drawing.Size(164, 20)
        Me.ChkAOIBuffer.TabIndex = 31
        Me.ChkAOIBuffer.Text = "Buffer AOI to clip layers"
        Me.ChkAOIBuffer.UseVisualStyleBackColor = True
        '
        'btnSelectWorkspace
        '
        Me.btnSelectWorkspace.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSelectWorkspace.Location = New System.Drawing.Point(523, 44)
        Me.btnSelectWorkspace.Name = "btnSelectWorkspace"
        Me.btnSelectWorkspace.Size = New System.Drawing.Size(56, 23)
        Me.btnSelectWorkspace.TabIndex = 41
        Me.btnSelectWorkspace.Text = "Open"
        Me.btnSelectWorkspace.UseVisualStyleBackColor = True
        '
        'txtOutputName
        '
        Me.txtOutputName.Location = New System.Drawing.Point(133, 77)
        Me.txtOutputName.Name = "txtOutputName"
        Me.txtOutputName.Size = New System.Drawing.Size(100, 20)
        Me.txtOutputName.TabIndex = 42
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(15, 78)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(111, 16)
        Me.Label7.TabIndex = 43
        Me.Label7.Text = "Output AOI name:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(8, 44)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(118, 16)
        Me.Label8.TabIndex = 44
        Me.Label8.Text = "Output workspace:"
        '
        'txtOutputWorkspace
        '
        Me.txtOutputWorkspace.Location = New System.Drawing.Point(132, 46)
        Me.txtOutputWorkspace.Name = "txtOutputWorkspace"
        Me.txtOutputWorkspace.Size = New System.Drawing.Size(385, 20)
        Me.txtOutputWorkspace.TabIndex = 45
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(52, 17)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(74, 16)
        Me.Label9.TabIndex = 46
        Me.Label9.Text = "Source file:"
        '
        'txtSourceData
        '
        Me.txtSourceData.BackColor = System.Drawing.SystemColors.Menu
        Me.txtSourceData.Location = New System.Drawing.Point(132, 16)
        Me.txtSourceData.Name = "txtSourceData"
        Me.txtSourceData.Size = New System.Drawing.Size(447, 20)
        Me.txtSourceData.TabIndex = 47
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(32, 215)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(201, 16)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Select the output layer(s) to view:"
        '
        'cmbSelectAll
        '
        Me.cmbSelectAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbSelectAll.Location = New System.Drawing.Point(32, 238)
        Me.cmbSelectAll.Name = "cmbSelectAll"
        Me.cmbSelectAll.Size = New System.Drawing.Size(100, 24)
        Me.cmbSelectAll.TabIndex = 18
        Me.cmbSelectAll.Text = "Select All"
        Me.cmbSelectAll.UseVisualStyleBackColor = True
        '
        'cmbSelectNone
        '
        Me.cmbSelectNone.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbSelectNone.Location = New System.Drawing.Point(137, 238)
        Me.cmbSelectNone.Name = "cmbSelectNone"
        Me.cmbSelectNone.Size = New System.Drawing.Size(100, 24)
        Me.cmbSelectNone.TabIndex = 17
        Me.cmbSelectNone.Text = "Select None"
        Me.cmbSelectNone.UseVisualStyleBackColor = True
        '
        'ChkDEMExtent
        '
        Me.ChkDEMExtent.AutoSize = True
        Me.ChkDEMExtent.Checked = True
        Me.ChkDEMExtent.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkDEMExtent.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkDEMExtent.Location = New System.Drawing.Point(35, 280)
        Me.ChkDEMExtent.Name = "ChkDEMExtent"
        Me.ChkDEMExtent.Size = New System.Drawing.Size(91, 19)
        Me.ChkDEMExtent.TabIndex = 21
        Me.ChkDEMExtent.Text = "DEM Extent"
        Me.ChkDEMExtent.UseVisualStyleBackColor = True
        '
        'ChkFilledDEM
        '
        Me.ChkFilledDEM.AutoSize = True
        Me.ChkFilledDEM.Checked = True
        Me.ChkFilledDEM.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkFilledDEM.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkFilledDEM.Location = New System.Drawing.Point(35, 305)
        Me.ChkFilledDEM.Name = "ChkFilledDEM"
        Me.ChkFilledDEM.Size = New System.Drawing.Size(87, 19)
        Me.ChkFilledDEM.TabIndex = 22
        Me.ChkFilledDEM.Text = "Filled DEM"
        Me.ChkFilledDEM.UseVisualStyleBackColor = True
        '
        'ChkFlowDir
        '
        Me.ChkFlowDir.AutoSize = True
        Me.ChkFlowDir.Checked = True
        Me.ChkFlowDir.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkFlowDir.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkFlowDir.Location = New System.Drawing.Point(35, 330)
        Me.ChkFlowDir.Name = "ChkFlowDir"
        Me.ChkFlowDir.Size = New System.Drawing.Size(104, 19)
        Me.ChkFlowDir.TabIndex = 19
        Me.ChkFlowDir.Text = "Flow Direction"
        Me.ChkFlowDir.UseVisualStyleBackColor = True
        '
        'ChkFlowAccum
        '
        Me.ChkFlowAccum.AutoSize = True
        Me.ChkFlowAccum.Checked = True
        Me.ChkFlowAccum.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkFlowAccum.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkFlowAccum.Location = New System.Drawing.Point(35, 355)
        Me.ChkFlowAccum.Name = "ChkFlowAccum"
        Me.ChkFlowAccum.Size = New System.Drawing.Size(129, 19)
        Me.ChkFlowAccum.TabIndex = 20
        Me.ChkFlowAccum.Text = "Flow Accumulation"
        Me.ChkFlowAccum.UseVisualStyleBackColor = True
        '
        'ChkSlope
        '
        Me.ChkSlope.AutoSize = True
        Me.ChkSlope.Checked = True
        Me.ChkSlope.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkSlope.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkSlope.Location = New System.Drawing.Point(35, 380)
        Me.ChkSlope.Name = "ChkSlope"
        Me.ChkSlope.Size = New System.Drawing.Size(58, 19)
        Me.ChkSlope.TabIndex = 25
        Me.ChkSlope.Text = "Slope"
        Me.ChkSlope.UseVisualStyleBackColor = True
        '
        'ChkAspect
        '
        Me.ChkAspect.AutoSize = True
        Me.ChkAspect.Checked = True
        Me.ChkAspect.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkAspect.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkAspect.Location = New System.Drawing.Point(35, 405)
        Me.ChkAspect.Name = "ChkAspect"
        Me.ChkAspect.Size = New System.Drawing.Size(62, 19)
        Me.ChkAspect.TabIndex = 24
        Me.ChkAspect.Text = "Aspect"
        Me.ChkAspect.UseVisualStyleBackColor = True
        '
        'ChkHillshade
        '
        Me.ChkHillshade.AutoSize = True
        Me.ChkHillshade.Checked = True
        Me.ChkHillshade.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkHillshade.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkHillshade.Location = New System.Drawing.Point(35, 430)
        Me.ChkHillshade.Name = "ChkHillshade"
        Me.ChkHillshade.Size = New System.Drawing.Size(78, 19)
        Me.ChkHillshade.TabIndex = 23
        Me.ChkHillshade.Text = "Hillshade"
        Me.ChkHillshade.UseVisualStyleBackColor = True
        '
        'frmCreateAOIfromExistingBND
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(593, 491)
        Me.Controls.Add(Me.txtSourceData)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.txtOutputWorkspace)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtOutputName)
        Me.Controls.Add(Me.btnSelectWorkspace)
        Me.Controls.Add(Me.grpboxPRISMUnit)
        Me.Controls.Add(Me.lblSlopeUnit)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.lblDEMUnit)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.cmbRun)
        Me.Controls.Add(Me.lblBufferUnit)
        Me.Controls.Add(Me.txtBufferD)
        Me.Controls.Add(Me.lblBufferD)
        Me.Controls.Add(Me.ChkAOIBuffer)
        Me.Controls.Add(Me.lblWhy)
        Me.Controls.Add(Me.GrpBoxFilter)
        Me.Controls.Add(Me.ChkSmoothDEM)
        Me.Controls.Add(Me.txtZFactor)
        Me.Controls.Add(Me.ChkHillshade)
        Me.Controls.Add(Me.ChkAspect)
        Me.Controls.Add(Me.ChkSlope)
        Me.Controls.Add(Me.ChkFlowAccum)
        Me.Controls.Add(Me.ChkFlowDir)
        Me.Controls.Add(Me.ChkFilledDEM)
        Me.Controls.Add(Me.ChkDEMExtent)
        Me.Controls.Add(Me.cmbSelectNone)
        Me.Controls.Add(Me.cmbSelectAll)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "frmCreateAOIfromExistingBND"
        Me.ShowIcon = False
        Me.Text = "Create AOI from a shapefile"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GrpBoxFilter.ResumeLayout(False)
        Me.GrpBoxFilter.PerformLayout()
        Me.grpboxPRISMUnit.ResumeLayout(False)
        Me.grpboxPRISMUnit.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Opt10Meters As System.Windows.Forms.RadioButton
    Friend WithEvents Opt30Meters As System.Windows.Forms.RadioButton
    Friend WithEvents lblWhy As System.Windows.Forms.Label
    Friend WithEvents GrpBoxFilter As System.Windows.Forms.GroupBox
    Friend WithEvents txtWidth As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtHeight As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ChkSmoothDEM As System.Windows.Forms.CheckBox
    Friend WithEvents txtZFactor As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents grpboxPRISMUnit As System.Windows.Forms.GroupBox
    Friend WithEvents rbtnDepthInch As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnDepthMM As System.Windows.Forms.RadioButton
    Friend WithEvents lblSlopeUnit As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lblDEMUnit As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cmbRun As System.Windows.Forms.Button
    Friend WithEvents lblBufferUnit As System.Windows.Forms.Label
    Friend WithEvents txtBufferD As System.Windows.Forms.TextBox
    Friend WithEvents lblBufferD As System.Windows.Forms.Label
    Friend WithEvents ChkAOIBuffer As System.Windows.Forms.CheckBox
    Friend WithEvents btnSelectWorkspace As System.Windows.Forms.Button
    Friend WithEvents txtOutputName As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtOutputWorkspace As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtSourceData As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmbSelectAll As System.Windows.Forms.Button
    Friend WithEvents cmbSelectNone As System.Windows.Forms.Button
    Friend WithEvents ChkDEMExtent As System.Windows.Forms.CheckBox
    Friend WithEvents ChkFilledDEM As System.Windows.Forms.CheckBox
    Friend WithEvents ChkFlowDir As System.Windows.Forms.CheckBox
    Friend WithEvents ChkFlowAccum As System.Windows.Forms.CheckBox
    Friend WithEvents ChkSlope As System.Windows.Forms.CheckBox
    Friend WithEvents ChkAspect As System.Windows.Forms.CheckBox
    Friend WithEvents ChkHillshade As System.Windows.Forms.CheckBox
End Class
