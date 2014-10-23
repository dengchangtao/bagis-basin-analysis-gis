<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmClipDEMtoAOI
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
        Me.Opt10Meters = New System.Windows.Forms.RadioButton()
        Me.Opt30Meters = New System.Windows.Forms.RadioButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CmbSelectAll = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.ChkDEMExtent = New System.Windows.Forms.CheckBox()
        Me.ChkFilledDEM = New System.Windows.Forms.CheckBox()
        Me.ChkFlowDir = New System.Windows.Forms.CheckBox()
        Me.ChkFlowAccum = New System.Windows.Forms.CheckBox()
        Me.ChkSlope = New System.Windows.Forms.CheckBox()
        Me.ChkAspect = New System.Windows.Forms.CheckBox()
        Me.ChkHillshade = New System.Windows.Forms.CheckBox()
        Me.CmbRun = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtZFactor = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ChkSmoothDEM = New System.Windows.Forms.CheckBox()
        Me.GrpBoxFilter = New System.Windows.Forms.GroupBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtHeight = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtWidth = New System.Windows.Forms.TextBox()
        Me.lblWhy = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GrpBoxFilter.SuspendLayout()
        Me.SuspendLayout()
        '
        'Opt10Meters
        '
        Me.Opt10Meters.AutoSize = True
        Me.Opt10Meters.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Opt10Meters.Location = New System.Drawing.Point(47, 22)
        Me.Opt10Meters.Name = "Opt10Meters"
        Me.Opt10Meters.Size = New System.Drawing.Size(98, 24)
        Me.Opt10Meters.TabIndex = 1
        Me.Opt10Meters.TabStop = True
        Me.Opt10Meters.Text = "10 meters"
        Me.Opt10Meters.UseVisualStyleBackColor = True
        '
        'Opt30Meters
        '
        Me.Opt30Meters.AutoSize = True
        Me.Opt30Meters.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Opt30Meters.Location = New System.Drawing.Point(47, 47)
        Me.Opt30Meters.Name = "Opt30Meters"
        Me.Opt30Meters.Size = New System.Drawing.Size(98, 24)
        Me.Opt30Meters.TabIndex = 1
        Me.Opt30Meters.TabStop = True
        Me.Opt30Meters.Text = "30 meters"
        Me.Opt30Meters.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(34, 86)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(201, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Select the output layer(s) to view:"
        '
        'CmbSelectAll
        '
        Me.CmbSelectAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmbSelectAll.Location = New System.Drawing.Point(34, 109)
        Me.CmbSelectAll.Name = "CmbSelectAll"
        Me.CmbSelectAll.Size = New System.Drawing.Size(100, 24)
        Me.CmbSelectAll.TabIndex = 2
        Me.CmbSelectAll.Text = "Select All"
        Me.CmbSelectAll.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(139, 109)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(100, 24)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Select None"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'ChkDEMExtent
        '
        Me.ChkDEMExtent.AutoSize = True
        Me.ChkDEMExtent.Checked = True
        Me.ChkDEMExtent.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkDEMExtent.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkDEMExtent.Location = New System.Drawing.Point(37, 151)
        Me.ChkDEMExtent.Name = "ChkDEMExtent"
        Me.ChkDEMExtent.Size = New System.Drawing.Size(91, 19)
        Me.ChkDEMExtent.TabIndex = 3
        Me.ChkDEMExtent.Text = "DEM Extent"
        Me.ChkDEMExtent.UseVisualStyleBackColor = True
        '
        'ChkFilledDEM
        '
        Me.ChkFilledDEM.AutoSize = True
        Me.ChkFilledDEM.Checked = True
        Me.ChkFilledDEM.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkFilledDEM.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkFilledDEM.Location = New System.Drawing.Point(37, 176)
        Me.ChkFilledDEM.Name = "ChkFilledDEM"
        Me.ChkFilledDEM.Size = New System.Drawing.Size(87, 19)
        Me.ChkFilledDEM.TabIndex = 3
        Me.ChkFilledDEM.Text = "Filled DEM"
        Me.ChkFilledDEM.UseVisualStyleBackColor = True
        '
        'ChkFlowDir
        '
        Me.ChkFlowDir.AutoSize = True
        Me.ChkFlowDir.Checked = True
        Me.ChkFlowDir.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkFlowDir.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkFlowDir.Location = New System.Drawing.Point(37, 201)
        Me.ChkFlowDir.Name = "ChkFlowDir"
        Me.ChkFlowDir.Size = New System.Drawing.Size(104, 19)
        Me.ChkFlowDir.TabIndex = 3
        Me.ChkFlowDir.Text = "Flow Direction"
        Me.ChkFlowDir.UseVisualStyleBackColor = True
        '
        'ChkFlowAccum
        '
        Me.ChkFlowAccum.AutoSize = True
        Me.ChkFlowAccum.Checked = True
        Me.ChkFlowAccum.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkFlowAccum.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkFlowAccum.Location = New System.Drawing.Point(37, 226)
        Me.ChkFlowAccum.Name = "ChkFlowAccum"
        Me.ChkFlowAccum.Size = New System.Drawing.Size(129, 19)
        Me.ChkFlowAccum.TabIndex = 3
        Me.ChkFlowAccum.Text = "Flow Accumulation"
        Me.ChkFlowAccum.UseVisualStyleBackColor = True
        '
        'ChkSlope
        '
        Me.ChkSlope.AutoSize = True
        Me.ChkSlope.Checked = True
        Me.ChkSlope.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkSlope.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkSlope.Location = New System.Drawing.Point(37, 251)
        Me.ChkSlope.Name = "ChkSlope"
        Me.ChkSlope.Size = New System.Drawing.Size(58, 19)
        Me.ChkSlope.TabIndex = 3
        Me.ChkSlope.Text = "Slope"
        Me.ChkSlope.UseVisualStyleBackColor = True
        '
        'ChkAspect
        '
        Me.ChkAspect.AutoSize = True
        Me.ChkAspect.Checked = True
        Me.ChkAspect.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkAspect.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkAspect.Location = New System.Drawing.Point(37, 276)
        Me.ChkAspect.Name = "ChkAspect"
        Me.ChkAspect.Size = New System.Drawing.Size(62, 19)
        Me.ChkAspect.TabIndex = 3
        Me.ChkAspect.Text = "Aspect"
        Me.ChkAspect.UseVisualStyleBackColor = True
        '
        'ChkHillshade
        '
        Me.ChkHillshade.AutoSize = True
        Me.ChkHillshade.Checked = True
        Me.ChkHillshade.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkHillshade.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkHillshade.Location = New System.Drawing.Point(37, 301)
        Me.ChkHillshade.Name = "ChkHillshade"
        Me.ChkHillshade.Size = New System.Drawing.Size(78, 19)
        Me.ChkHillshade.TabIndex = 3
        Me.ChkHillshade.Text = "Hillshade"
        Me.ChkHillshade.UseVisualStyleBackColor = True
        '
        'CmbRun
        '
        Me.CmbRun.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmbRun.Location = New System.Drawing.Point(86, 446)
        Me.CmbRun.Name = "CmbRun"
        Me.CmbRun.Size = New System.Drawing.Size(96, 32)
        Me.CmbRun.TabIndex = 4
        Me.CmbRun.Text = "Clip"
        Me.CmbRun.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(37, 328)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(115, 16)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Hillshade Z factor:"
        '
        'txtZFactor
        '
        Me.txtZFactor.Location = New System.Drawing.Point(172, 328)
        Me.txtZFactor.Name = "txtZFactor"
        Me.txtZFactor.Size = New System.Drawing.Size(67, 20)
        Me.txtZFactor.TabIndex = 5
        Me.txtZFactor.Text = "1"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Opt10Meters)
        Me.GroupBox1.Controls.Add(Me.Opt30Meters)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(32, 7)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(200, 76)
        Me.GroupBox1.TabIndex = 11
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Source DEM:"
        '
        'ChkSmoothDEM
        '
        Me.ChkSmoothDEM.AutoSize = True
        Me.ChkSmoothDEM.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkSmoothDEM.Location = New System.Drawing.Point(37, 366)
        Me.ChkSmoothDEM.Name = "ChkSmoothDEM"
        Me.ChkSmoothDEM.Size = New System.Drawing.Size(106, 20)
        Me.ChkSmoothDEM.TabIndex = 12
        Me.ChkSmoothDEM.Text = "Smooth DEM"
        Me.ChkSmoothDEM.UseVisualStyleBackColor = True
        '
        'GrpBoxFilter
        '
        Me.GrpBoxFilter.Controls.Add(Me.txtWidth)
        Me.GrpBoxFilter.Controls.Add(Me.Label4)
        Me.GrpBoxFilter.Controls.Add(Me.txtHeight)
        Me.GrpBoxFilter.Controls.Add(Me.Label2)
        Me.GrpBoxFilter.Enabled = False
        Me.GrpBoxFilter.Location = New System.Drawing.Point(37, 392)
        Me.GrpBoxFilter.Name = "GrpBoxFilter"
        Me.GrpBoxFilter.Size = New System.Drawing.Size(213, 48)
        Me.GrpBoxFilter.TabIndex = 13
        Me.GrpBoxFilter.TabStop = False
        Me.GrpBoxFilter.Text = "Filter Size (pixels)"
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
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(20, 25)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(38, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Height"
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
        'lblWhy
        '
        Me.lblWhy.AutoSize = True
        Me.lblWhy.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWhy.ForeColor = System.Drawing.Color.Blue
        Me.lblWhy.Location = New System.Drawing.Point(139, 367)
        Me.lblWhy.Name = "lblWhy"
        Me.lblWhy.Size = New System.Drawing.Size(56, 16)
        Me.lblWhy.TabIndex = 14
        Me.lblWhy.Text = "(Why?)"
        '
        'frmClipDEM
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(274, 488)
        Me.Controls.Add(Me.lblWhy)
        Me.Controls.Add(Me.GrpBoxFilter)
        Me.Controls.Add(Me.ChkSmoothDEM)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.txtZFactor)
        Me.Controls.Add(Me.CmbRun)
        Me.Controls.Add(Me.ChkHillshade)
        Me.Controls.Add(Me.ChkAspect)
        Me.Controls.Add(Me.ChkSlope)
        Me.Controls.Add(Me.ChkFlowAccum)
        Me.Controls.Add(Me.ChkFlowDir)
        Me.Controls.Add(Me.ChkFilledDEM)
        Me.Controls.Add(Me.ChkDEMExtent)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.CmbSelectAll)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label3)
        Me.Name = "frmClipDEM"
        Me.ShowIcon = False
        Me.Text = "DEM Clipping"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GrpBoxFilter.ResumeLayout(False)
        Me.GrpBoxFilter.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Opt10Meters As System.Windows.Forms.RadioButton
    Friend WithEvents Opt30Meters As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CmbSelectAll As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ChkDEMExtent As System.Windows.Forms.CheckBox
    Friend WithEvents ChkFilledDEM As System.Windows.Forms.CheckBox
    Friend WithEvents ChkFlowDir As System.Windows.Forms.CheckBox
    Friend WithEvents ChkFlowAccum As System.Windows.Forms.CheckBox
    Friend WithEvents ChkSlope As System.Windows.Forms.CheckBox
    Friend WithEvents ChkAspect As System.Windows.Forms.CheckBox
    Friend WithEvents ChkHillshade As System.Windows.Forms.CheckBox
    Friend WithEvents CmbRun As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtZFactor As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ChkSmoothDEM As System.Windows.Forms.CheckBox
    Friend WithEvents GrpBoxFilter As System.Windows.Forms.GroupBox
    Friend WithEvents txtWidth As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtHeight As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblWhy As System.Windows.Forms.Label
End Class
