<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmElevScenario
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
        Me.lblElevUnit = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.txtMinElev = New System.Windows.Forms.TextBox()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.txtMaxElev = New System.Windows.Forms.TextBox()
        Me.txtZUnit4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.txtSNOTELMin = New System.Windows.Forms.TextBox()
        Me.txtSCourseMin = New System.Windows.Forms.TextBox()
        Me.txtActualLow = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtSNOTELMax = New System.Windows.Forms.TextBox()
        Me.txtSCourseMax = New System.Windows.Forms.TextBox()
        Me.txtActualHigh = New System.Windows.Forms.TextBox()
        Me.txtZUnit1 = New System.Windows.Forms.Label()
        Me.txtZUnit2 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.txtA_BelowArea1 = New System.Windows.Forms.TextBox()
        Me.txtA_BelowArea2 = New System.Windows.Forms.TextBox()
        Me.txtA_BelowPercent = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txtA_AboveArea1 = New System.Windows.Forms.TextBox()
        Me.txtA_AboveArea2 = New System.Windows.Forms.TextBox()
        Me.txtA_AbovePercent = New System.Windows.Forms.TextBox()
        Me.txtA_TotalArea1 = New System.Windows.Forms.TextBox()
        Me.txtA_TotalArea2 = New System.Windows.Forms.TextBox()
        Me.txtA_TotalPercent = New System.Windows.Forms.TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtP_BelowArea1 = New System.Windows.Forms.TextBox()
        Me.txtP_AboveArea1 = New System.Windows.Forms.TextBox()
        Me.txtP_BelowArea2 = New System.Windows.Forms.TextBox()
        Me.txtP_TotalArea1 = New System.Windows.Forms.TextBox()
        Me.txtP_AboveArea2 = New System.Windows.Forms.TextBox()
        Me.txtP_TotalArea2 = New System.Windows.Forms.TextBox()
        Me.txtP_BelowPercent = New System.Windows.Forms.TextBox()
        Me.txtP_AbovePercent = New System.Windows.Forms.TextBox()
        Me.txtP_TotalPercent = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtNewLow = New System.Windows.Forms.TextBox()
        Me.txtNewHigh = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtZUnit3 = New System.Windows.Forms.Label()
        Me.CmdCalculate = New System.Windows.Forms.Button()
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.txtLowerBuffer = New System.Windows.Forms.TextBox()
        Me.txtZUnit5 = New System.Windows.Forms.Label()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.OptZMeters = New System.Windows.Forms.RadioButton()
        Me.OptZFeet = New System.Windows.Forms.RadioButton()
        Me.SuspendLayout()
        '
        'lblElevUnit
        '
        Me.lblElevUnit.AutoSize = True
        Me.lblElevUnit.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblElevUnit.Location = New System.Drawing.Point(17, 43)
        Me.lblElevUnit.Name = "lblElevUnit"
        Me.lblElevUnit.Size = New System.Drawing.Size(109, 18)
        Me.lblElevUnit.TabIndex = 0
        Me.lblElevUnit.Text = "AOI Elevation"
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.Location = New System.Drawing.Point(59, 71)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(29, 16)
        Me.Label23.TabIndex = 0
        Me.Label23.Text = "Min"
        '
        'txtMinElev
        '
        Me.txtMinElev.BackColor = System.Drawing.SystemColors.Menu
        Me.txtMinElev.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtMinElev.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMinElev.ForeColor = System.Drawing.Color.Blue
        Me.txtMinElev.Location = New System.Drawing.Point(94, 71)
        Me.txtMinElev.Name = "txtMinElev"
        Me.txtMinElev.ReadOnly = True
        Me.txtMinElev.Size = New System.Drawing.Size(100, 15)
        Me.txtMinElev.TabIndex = 1
        Me.txtMinElev.Text = "0"
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.Location = New System.Drawing.Point(232, 71)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(33, 16)
        Me.Label24.TabIndex = 0
        Me.Label24.Text = "Max"
        '
        'txtMaxElev
        '
        Me.txtMaxElev.BackColor = System.Drawing.SystemColors.Menu
        Me.txtMaxElev.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtMaxElev.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMaxElev.ForeColor = System.Drawing.Color.Blue
        Me.txtMaxElev.Location = New System.Drawing.Point(267, 71)
        Me.txtMaxElev.Name = "txtMaxElev"
        Me.txtMaxElev.ReadOnly = True
        Me.txtMaxElev.Size = New System.Drawing.Size(100, 15)
        Me.txtMaxElev.TabIndex = 1
        Me.txtMaxElev.Text = "0"
        '
        'txtZUnit4
        '
        Me.txtZUnit4.AutoSize = True
        Me.txtZUnit4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtZUnit4.Location = New System.Drawing.Point(367, 71)
        Me.txtZUnit4.Name = "txtZUnit4"
        Me.txtZUnit4.Size = New System.Drawing.Size(49, 16)
        Me.txtZUnit4.TabIndex = 0
        Me.txtZUnit4.Text = "Meters"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(16, 116)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(110, 18)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Existing Sites"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(71, 146)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(65, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "SNOTEL:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(46, 172)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(90, 16)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Snow Course:"
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.Location = New System.Drawing.Point(33, 198)
        Me.Label28.Name = "Label28"
        Me.Label28.Size = New System.Drawing.Size(103, 16)
        Me.Label28.TabIndex = 0
        Me.Label28.Text = "Elevation Used:"
        '
        'txtSNOTELMin
        '
        Me.txtSNOTELMin.BackColor = System.Drawing.SystemColors.Menu
        Me.txtSNOTELMin.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSNOTELMin.ForeColor = System.Drawing.Color.Blue
        Me.txtSNOTELMin.Location = New System.Drawing.Point(142, 142)
        Me.txtSNOTELMin.Name = "txtSNOTELMin"
        Me.txtSNOTELMin.ReadOnly = True
        Me.txtSNOTELMin.Size = New System.Drawing.Size(100, 22)
        Me.txtSNOTELMin.TabIndex = 1
        Me.txtSNOTELMin.Text = "0"
        '
        'txtSCourseMin
        '
        Me.txtSCourseMin.BackColor = System.Drawing.SystemColors.Menu
        Me.txtSCourseMin.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSCourseMin.ForeColor = System.Drawing.Color.Blue
        Me.txtSCourseMin.Location = New System.Drawing.Point(142, 169)
        Me.txtSCourseMin.Name = "txtSCourseMin"
        Me.txtSCourseMin.ReadOnly = True
        Me.txtSCourseMin.Size = New System.Drawing.Size(100, 22)
        Me.txtSCourseMin.TabIndex = 1
        Me.txtSCourseMin.Text = "0"
        '
        'txtActualLow
        '
        Me.txtActualLow.BackColor = System.Drawing.SystemColors.Window
        Me.txtActualLow.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtActualLow.ForeColor = System.Drawing.Color.Blue
        Me.txtActualLow.Location = New System.Drawing.Point(142, 197)
        Me.txtActualLow.Name = "txtActualLow"
        Me.txtActualLow.Size = New System.Drawing.Size(100, 22)
        Me.txtActualLow.TabIndex = 1
        Me.txtActualLow.Text = "0"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(160, 126)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(65, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Lowest Elev"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(272, 126)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(67, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Highest Elev"
        '
        'txtSNOTELMax
        '
        Me.txtSNOTELMax.BackColor = System.Drawing.SystemColors.Menu
        Me.txtSNOTELMax.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSNOTELMax.ForeColor = System.Drawing.Color.Blue
        Me.txtSNOTELMax.Location = New System.Drawing.Point(254, 142)
        Me.txtSNOTELMax.Name = "txtSNOTELMax"
        Me.txtSNOTELMax.ReadOnly = True
        Me.txtSNOTELMax.Size = New System.Drawing.Size(100, 22)
        Me.txtSNOTELMax.TabIndex = 1
        Me.txtSNOTELMax.Text = "0"
        '
        'txtSCourseMax
        '
        Me.txtSCourseMax.BackColor = System.Drawing.SystemColors.Menu
        Me.txtSCourseMax.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSCourseMax.ForeColor = System.Drawing.Color.Blue
        Me.txtSCourseMax.Location = New System.Drawing.Point(254, 169)
        Me.txtSCourseMax.Name = "txtSCourseMax"
        Me.txtSCourseMax.ReadOnly = True
        Me.txtSCourseMax.Size = New System.Drawing.Size(100, 22)
        Me.txtSCourseMax.TabIndex = 1
        Me.txtSCourseMax.Text = "0"
        '
        'txtActualHigh
        '
        Me.txtActualHigh.BackColor = System.Drawing.SystemColors.Window
        Me.txtActualHigh.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtActualHigh.ForeColor = System.Drawing.Color.Blue
        Me.txtActualHigh.Location = New System.Drawing.Point(254, 197)
        Me.txtActualHigh.Name = "txtActualHigh"
        Me.txtActualHigh.Size = New System.Drawing.Size(100, 22)
        Me.txtActualHigh.TabIndex = 1
        Me.txtActualHigh.Text = "0"
        '
        'txtZUnit1
        '
        Me.txtZUnit1.AutoSize = True
        Me.txtZUnit1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtZUnit1.Location = New System.Drawing.Point(367, 143)
        Me.txtZUnit1.Name = "txtZUnit1"
        Me.txtZUnit1.Size = New System.Drawing.Size(35, 16)
        Me.txtZUnit1.TabIndex = 0
        Me.txtZUnit1.Text = "Feet"
        '
        'txtZUnit2
        '
        Me.txtZUnit2.AutoSize = True
        Me.txtZUnit2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtZUnit2.Location = New System.Drawing.Point(367, 172)
        Me.txtZUnit2.Name = "txtZUnit2"
        Me.txtZUnit2.Size = New System.Drawing.Size(35, 16)
        Me.txtZUnit2.TabIndex = 0
        Me.txtZUnit2.Text = "Feet"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(171, 246)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(180, 13)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "Actual Non-represented* Area in AOI"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(109, 271)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(36, 13)
        Me.Label17.TabIndex = 0
        Me.Label17.Text = "Below"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(29, 291)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(46, 16)
        Me.Label20.TabIndex = 0
        Me.Label20.Text = "Acres:"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(242, 271)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(38, 13)
        Me.Label18.TabIndex = 0
        Me.Label18.Text = "Above"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(9, 316)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(66, 16)
        Me.Label21.TabIndex = 0
        Me.Label21.Text = "Hectares:"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.Location = New System.Drawing.Point(13, 342)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(62, 16)
        Me.Label22.TabIndex = 0
        Me.Label22.Text = "% of AOI:"
        '
        'txtA_BelowArea1
        '
        Me.txtA_BelowArea1.BackColor = System.Drawing.SystemColors.Menu
        Me.txtA_BelowArea1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtA_BelowArea1.ForeColor = System.Drawing.Color.Blue
        Me.txtA_BelowArea1.Location = New System.Drawing.Point(79, 287)
        Me.txtA_BelowArea1.Name = "txtA_BelowArea1"
        Me.txtA_BelowArea1.ReadOnly = True
        Me.txtA_BelowArea1.Size = New System.Drawing.Size(116, 22)
        Me.txtA_BelowArea1.TabIndex = 1
        Me.txtA_BelowArea1.Text = "0"
        '
        'txtA_BelowArea2
        '
        Me.txtA_BelowArea2.BackColor = System.Drawing.SystemColors.Menu
        Me.txtA_BelowArea2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtA_BelowArea2.ForeColor = System.Drawing.Color.Blue
        Me.txtA_BelowArea2.Location = New System.Drawing.Point(79, 314)
        Me.txtA_BelowArea2.Name = "txtA_BelowArea2"
        Me.txtA_BelowArea2.ReadOnly = True
        Me.txtA_BelowArea2.Size = New System.Drawing.Size(116, 22)
        Me.txtA_BelowArea2.TabIndex = 1
        Me.txtA_BelowArea2.Text = "0"
        '
        'txtA_BelowPercent
        '
        Me.txtA_BelowPercent.BackColor = System.Drawing.SystemColors.Menu
        Me.txtA_BelowPercent.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtA_BelowPercent.ForeColor = System.Drawing.Color.Blue
        Me.txtA_BelowPercent.Location = New System.Drawing.Point(79, 342)
        Me.txtA_BelowPercent.Name = "txtA_BelowPercent"
        Me.txtA_BelowPercent.ReadOnly = True
        Me.txtA_BelowPercent.Size = New System.Drawing.Size(116, 22)
        Me.txtA_BelowPercent.TabIndex = 1
        Me.txtA_BelowPercent.Text = "0"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(372, 271)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(31, 13)
        Me.Label19.TabIndex = 0
        Me.Label19.Text = "Total"
        '
        'txtA_AboveArea1
        '
        Me.txtA_AboveArea1.BackColor = System.Drawing.SystemColors.Menu
        Me.txtA_AboveArea1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtA_AboveArea1.ForeColor = System.Drawing.Color.Blue
        Me.txtA_AboveArea1.Location = New System.Drawing.Point(208, 287)
        Me.txtA_AboveArea1.Name = "txtA_AboveArea1"
        Me.txtA_AboveArea1.ReadOnly = True
        Me.txtA_AboveArea1.Size = New System.Drawing.Size(116, 22)
        Me.txtA_AboveArea1.TabIndex = 1
        Me.txtA_AboveArea1.Text = "0"
        '
        'txtA_AboveArea2
        '
        Me.txtA_AboveArea2.BackColor = System.Drawing.SystemColors.Menu
        Me.txtA_AboveArea2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtA_AboveArea2.ForeColor = System.Drawing.Color.Blue
        Me.txtA_AboveArea2.Location = New System.Drawing.Point(208, 314)
        Me.txtA_AboveArea2.Name = "txtA_AboveArea2"
        Me.txtA_AboveArea2.ReadOnly = True
        Me.txtA_AboveArea2.Size = New System.Drawing.Size(116, 22)
        Me.txtA_AboveArea2.TabIndex = 1
        Me.txtA_AboveArea2.Text = "0"
        '
        'txtA_AbovePercent
        '
        Me.txtA_AbovePercent.BackColor = System.Drawing.SystemColors.Menu
        Me.txtA_AbovePercent.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtA_AbovePercent.ForeColor = System.Drawing.Color.Blue
        Me.txtA_AbovePercent.Location = New System.Drawing.Point(208, 342)
        Me.txtA_AbovePercent.Name = "txtA_AbovePercent"
        Me.txtA_AbovePercent.ReadOnly = True
        Me.txtA_AbovePercent.Size = New System.Drawing.Size(116, 22)
        Me.txtA_AbovePercent.TabIndex = 1
        Me.txtA_AbovePercent.Text = "0"
        '
        'txtA_TotalArea1
        '
        Me.txtA_TotalArea1.BackColor = System.Drawing.SystemColors.Menu
        Me.txtA_TotalArea1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtA_TotalArea1.ForeColor = System.Drawing.Color.Blue
        Me.txtA_TotalArea1.Location = New System.Drawing.Point(339, 287)
        Me.txtA_TotalArea1.Name = "txtA_TotalArea1"
        Me.txtA_TotalArea1.ReadOnly = True
        Me.txtA_TotalArea1.Size = New System.Drawing.Size(116, 22)
        Me.txtA_TotalArea1.TabIndex = 1
        Me.txtA_TotalArea1.Text = "0"
        '
        'txtA_TotalArea2
        '
        Me.txtA_TotalArea2.BackColor = System.Drawing.SystemColors.Menu
        Me.txtA_TotalArea2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtA_TotalArea2.ForeColor = System.Drawing.Color.Blue
        Me.txtA_TotalArea2.Location = New System.Drawing.Point(339, 314)
        Me.txtA_TotalArea2.Name = "txtA_TotalArea2"
        Me.txtA_TotalArea2.ReadOnly = True
        Me.txtA_TotalArea2.Size = New System.Drawing.Size(116, 22)
        Me.txtA_TotalArea2.TabIndex = 1
        Me.txtA_TotalArea2.Text = "0"
        '
        'txtA_TotalPercent
        '
        Me.txtA_TotalPercent.BackColor = System.Drawing.SystemColors.Menu
        Me.txtA_TotalPercent.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtA_TotalPercent.ForeColor = System.Drawing.Color.Blue
        Me.txtA_TotalPercent.Location = New System.Drawing.Point(339, 342)
        Me.txtA_TotalPercent.Name = "txtA_TotalPercent"
        Me.txtA_TotalPercent.ReadOnly = True
        Me.txtA_TotalPercent.Size = New System.Drawing.Size(116, 22)
        Me.txtA_TotalPercent.TabIndex = 1
        Me.txtA_TotalPercent.Text = "0"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(644, 246)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(186, 13)
        Me.Label13.TabIndex = 0
        Me.Label13.Text = "Pseudo Non-represented* Area in AOI"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(592, 271)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(36, 13)
        Me.Label14.TabIndex = 0
        Me.Label14.Text = "Below"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(510, 291)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(46, 16)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Acres:"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(725, 271)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(38, 13)
        Me.Label15.TabIndex = 0
        Me.Label15.Text = "Above"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(855, 271)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(31, 13)
        Me.Label16.TabIndex = 0
        Me.Label16.Text = "Total"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(490, 316)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(66, 16)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "Hectares:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(494, 342)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(62, 16)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "% of AOI:"
        '
        'txtP_BelowArea1
        '
        Me.txtP_BelowArea1.BackColor = System.Drawing.SystemColors.Menu
        Me.txtP_BelowArea1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtP_BelowArea1.ForeColor = System.Drawing.Color.Red
        Me.txtP_BelowArea1.Location = New System.Drawing.Point(562, 287)
        Me.txtP_BelowArea1.Name = "txtP_BelowArea1"
        Me.txtP_BelowArea1.ReadOnly = True
        Me.txtP_BelowArea1.Size = New System.Drawing.Size(116, 22)
        Me.txtP_BelowArea1.TabIndex = 1
        Me.txtP_BelowArea1.Text = "0"
        '
        'txtP_AboveArea1
        '
        Me.txtP_AboveArea1.BackColor = System.Drawing.SystemColors.Menu
        Me.txtP_AboveArea1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtP_AboveArea1.ForeColor = System.Drawing.Color.Red
        Me.txtP_AboveArea1.Location = New System.Drawing.Point(691, 287)
        Me.txtP_AboveArea1.Name = "txtP_AboveArea1"
        Me.txtP_AboveArea1.ReadOnly = True
        Me.txtP_AboveArea1.Size = New System.Drawing.Size(116, 22)
        Me.txtP_AboveArea1.TabIndex = 1
        Me.txtP_AboveArea1.Text = "0"
        '
        'txtP_BelowArea2
        '
        Me.txtP_BelowArea2.BackColor = System.Drawing.SystemColors.Menu
        Me.txtP_BelowArea2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtP_BelowArea2.ForeColor = System.Drawing.Color.Red
        Me.txtP_BelowArea2.Location = New System.Drawing.Point(562, 314)
        Me.txtP_BelowArea2.Name = "txtP_BelowArea2"
        Me.txtP_BelowArea2.ReadOnly = True
        Me.txtP_BelowArea2.Size = New System.Drawing.Size(116, 22)
        Me.txtP_BelowArea2.TabIndex = 1
        Me.txtP_BelowArea2.Text = "0"
        '
        'txtP_TotalArea1
        '
        Me.txtP_TotalArea1.BackColor = System.Drawing.SystemColors.Menu
        Me.txtP_TotalArea1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtP_TotalArea1.ForeColor = System.Drawing.Color.Red
        Me.txtP_TotalArea1.Location = New System.Drawing.Point(822, 287)
        Me.txtP_TotalArea1.Name = "txtP_TotalArea1"
        Me.txtP_TotalArea1.ReadOnly = True
        Me.txtP_TotalArea1.Size = New System.Drawing.Size(116, 22)
        Me.txtP_TotalArea1.TabIndex = 1
        Me.txtP_TotalArea1.Text = "0"
        '
        'txtP_AboveArea2
        '
        Me.txtP_AboveArea2.BackColor = System.Drawing.SystemColors.Menu
        Me.txtP_AboveArea2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtP_AboveArea2.ForeColor = System.Drawing.Color.Red
        Me.txtP_AboveArea2.Location = New System.Drawing.Point(691, 314)
        Me.txtP_AboveArea2.Name = "txtP_AboveArea2"
        Me.txtP_AboveArea2.ReadOnly = True
        Me.txtP_AboveArea2.Size = New System.Drawing.Size(116, 22)
        Me.txtP_AboveArea2.TabIndex = 1
        Me.txtP_AboveArea2.Text = "0"
        '
        'txtP_TotalArea2
        '
        Me.txtP_TotalArea2.BackColor = System.Drawing.SystemColors.Menu
        Me.txtP_TotalArea2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtP_TotalArea2.ForeColor = System.Drawing.Color.Red
        Me.txtP_TotalArea2.Location = New System.Drawing.Point(822, 314)
        Me.txtP_TotalArea2.Name = "txtP_TotalArea2"
        Me.txtP_TotalArea2.ReadOnly = True
        Me.txtP_TotalArea2.Size = New System.Drawing.Size(116, 22)
        Me.txtP_TotalArea2.TabIndex = 1
        Me.txtP_TotalArea2.Text = "0"
        '
        'txtP_BelowPercent
        '
        Me.txtP_BelowPercent.BackColor = System.Drawing.SystemColors.Menu
        Me.txtP_BelowPercent.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtP_BelowPercent.ForeColor = System.Drawing.Color.Red
        Me.txtP_BelowPercent.Location = New System.Drawing.Point(562, 342)
        Me.txtP_BelowPercent.Name = "txtP_BelowPercent"
        Me.txtP_BelowPercent.ReadOnly = True
        Me.txtP_BelowPercent.Size = New System.Drawing.Size(116, 22)
        Me.txtP_BelowPercent.TabIndex = 1
        Me.txtP_BelowPercent.Text = "0"
        '
        'txtP_AbovePercent
        '
        Me.txtP_AbovePercent.BackColor = System.Drawing.SystemColors.Menu
        Me.txtP_AbovePercent.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtP_AbovePercent.ForeColor = System.Drawing.Color.Red
        Me.txtP_AbovePercent.Location = New System.Drawing.Point(691, 342)
        Me.txtP_AbovePercent.Name = "txtP_AbovePercent"
        Me.txtP_AbovePercent.ReadOnly = True
        Me.txtP_AbovePercent.Size = New System.Drawing.Size(116, 22)
        Me.txtP_AbovePercent.TabIndex = 1
        Me.txtP_AbovePercent.Text = "0"
        '
        'txtP_TotalPercent
        '
        Me.txtP_TotalPercent.BackColor = System.Drawing.SystemColors.Menu
        Me.txtP_TotalPercent.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtP_TotalPercent.ForeColor = System.Drawing.Color.Red
        Me.txtP_TotalPercent.Location = New System.Drawing.Point(822, 342)
        Me.txtP_TotalPercent.Name = "txtP_TotalPercent"
        Me.txtP_TotalPercent.ReadOnly = True
        Me.txtP_TotalPercent.Size = New System.Drawing.Size(116, 22)
        Me.txtP_TotalPercent.TabIndex = 1
        Me.txtP_TotalPercent.Text = "0"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(508, 201)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(108, 18)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Pseudo Sites"
        '
        'txtNewLow
        '
        Me.txtNewLow.BackColor = System.Drawing.SystemColors.Window
        Me.txtNewLow.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNewLow.ForeColor = System.Drawing.Color.Red
        Me.txtNewLow.Location = New System.Drawing.Point(645, 198)
        Me.txtNewLow.Name = "txtNewLow"
        Me.txtNewLow.Size = New System.Drawing.Size(100, 22)
        Me.txtNewLow.TabIndex = 1
        Me.txtNewLow.Text = "0"
        '
        'txtNewHigh
        '
        Me.txtNewHigh.BackColor = System.Drawing.SystemColors.Window
        Me.txtNewHigh.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNewHigh.ForeColor = System.Drawing.Color.Red
        Me.txtNewHigh.Location = New System.Drawing.Point(757, 198)
        Me.txtNewHigh.Name = "txtNewHigh"
        Me.txtNewHigh.Size = New System.Drawing.Size(100, 22)
        Me.txtNewHigh.TabIndex = 1
        Me.txtNewHigh.Text = "0"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(650, 182)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(90, 13)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "New Lowest Elev"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(761, 182)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(92, 13)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "New Highest Elev"
        '
        'txtZUnit3
        '
        Me.txtZUnit3.AutoSize = True
        Me.txtZUnit3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtZUnit3.Location = New System.Drawing.Point(875, 200)
        Me.txtZUnit3.Name = "txtZUnit3"
        Me.txtZUnit3.Size = New System.Drawing.Size(35, 16)
        Me.txtZUnit3.TabIndex = 0
        Me.txtZUnit3.Text = "Feet"
        '
        'CmdCalculate
        '
        Me.CmdCalculate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdCalculate.Location = New System.Drawing.Point(645, 143)
        Me.CmdCalculate.Name = "CmdCalculate"
        Me.CmdCalculate.Size = New System.Drawing.Size(100, 26)
        Me.CmdCalculate.TabIndex = 2
        Me.CmdCalculate.Text = "Calculate"
        Me.CmdCalculate.UseVisualStyleBackColor = True
        '
        'cmdClose
        '
        Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClose.Location = New System.Drawing.Point(764, 143)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(100, 26)
        Me.cmdClose.TabIndex = 2
        Me.cmdClose.Text = "Close"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.Location = New System.Drawing.Point(517, 107)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(64, 15)
        Me.Label29.TabIndex = 0
        Me.Label29.Text = "and below"
        '
        'txtLowerBuffer
        '
        Me.txtLowerBuffer.Location = New System.Drawing.Point(587, 102)
        Me.txtLowerBuffer.Name = "txtLowerBuffer"
        Me.txtLowerBuffer.Size = New System.Drawing.Size(71, 21)
        Me.txtLowerBuffer.TabIndex = 1
        Me.txtLowerBuffer.Text = "200"
        '
        'txtZUnit5
        '
        Me.txtZUnit5.AutoSize = True
        Me.txtZUnit5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtZUnit5.Location = New System.Drawing.Point(664, 107)
        Me.txtZUnit5.Name = "txtZUnit5"
        Me.txtZUnit5.Size = New System.Drawing.Size(49, 16)
        Me.txtZUnit5.TabIndex = 0
        Me.txtZUnit5.Text = "Meters"
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label27.Location = New System.Drawing.Point(709, 107)
        Me.Label27.Name = "Label27"
        Me.Label27.Size = New System.Drawing.Size(133, 16)
        Me.Label27.TabIndex = 0
        Me.Label27.Text = "below the lowest site."
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label25.Location = New System.Drawing.Point(511, 72)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(416, 16)
        Me.Label25.TabIndex = 0
        Me.Label25.Text = "* Non-represented area is defined as the area above the highest site "
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.Font = New System.Drawing.Font("Tahoma", 10.0!)
        Me.Label30.Location = New System.Drawing.Point(584, 19)
        Me.Label30.Name = "Label30"
        Me.Label30.Size = New System.Drawing.Size(96, 17)
        Me.Label30.TabIndex = 3
        Me.Label30.Text = "Elevation Unit:"
        '
        'OptZMeters
        '
        Me.OptZMeters.AutoSize = True
        Me.OptZMeters.Location = New System.Drawing.Point(712, 19)
        Me.OptZMeters.Name = "OptZMeters"
        Me.OptZMeters.Size = New System.Drawing.Size(58, 17)
        Me.OptZMeters.TabIndex = 4
        Me.OptZMeters.TabStop = True
        Me.OptZMeters.Text = "Meters"
        Me.OptZMeters.UseVisualStyleBackColor = True
        '
        'OptZFeet
        '
        Me.OptZFeet.AutoSize = True
        Me.OptZFeet.Location = New System.Drawing.Point(782, 19)
        Me.OptZFeet.Name = "OptZFeet"
        Me.OptZFeet.Size = New System.Drawing.Size(47, 17)
        Me.OptZFeet.TabIndex = 4
        Me.OptZFeet.TabStop = True
        Me.OptZFeet.Text = "Feet"
        Me.OptZFeet.UseVisualStyleBackColor = True
        '
        'frmElevScenario
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(943, 374)
        Me.Controls.Add(Me.OptZFeet)
        Me.Controls.Add(Me.OptZMeters)
        Me.Controls.Add(Me.Label30)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.CmdCalculate)
        Me.Controls.Add(Me.txtLowerBuffer)
        Me.Controls.Add(Me.txtMaxElev)
        Me.Controls.Add(Me.txtNewHigh)
        Me.Controls.Add(Me.txtActualHigh)
        Me.Controls.Add(Me.txtP_TotalPercent)
        Me.Controls.Add(Me.txtA_TotalPercent)
        Me.Controls.Add(Me.txtP_AbovePercent)
        Me.Controls.Add(Me.txtA_AbovePercent)
        Me.Controls.Add(Me.txtP_BelowPercent)
        Me.Controls.Add(Me.txtNewLow)
        Me.Controls.Add(Me.txtA_BelowPercent)
        Me.Controls.Add(Me.txtActualLow)
        Me.Controls.Add(Me.txtSCourseMax)
        Me.Controls.Add(Me.txtP_TotalArea2)
        Me.Controls.Add(Me.txtA_TotalArea2)
        Me.Controls.Add(Me.txtP_AboveArea2)
        Me.Controls.Add(Me.txtA_AboveArea2)
        Me.Controls.Add(Me.txtP_TotalArea1)
        Me.Controls.Add(Me.txtA_TotalArea1)
        Me.Controls.Add(Me.txtP_BelowArea2)
        Me.Controls.Add(Me.txtA_BelowArea2)
        Me.Controls.Add(Me.txtP_AboveArea1)
        Me.Controls.Add(Me.txtA_AboveArea1)
        Me.Controls.Add(Me.txtP_BelowArea1)
        Me.Controls.Add(Me.txtSNOTELMax)
        Me.Controls.Add(Me.txtA_BelowArea1)
        Me.Controls.Add(Me.txtSCourseMin)
        Me.Controls.Add(Me.txtSNOTELMin)
        Me.Controls.Add(Me.txtMinElev)
        Me.Controls.Add(Me.txtZUnit2)
        Me.Controls.Add(Me.txtZUnit3)
        Me.Controls.Add(Me.txtZUnit1)
        Me.Controls.Add(Me.Label25)
        Me.Controls.Add(Me.Label27)
        Me.Controls.Add(Me.txtZUnit5)
        Me.Controls.Add(Me.txtZUnit4)
        Me.Controls.Add(Me.Label24)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label23)
        Me.Controls.Add(Me.Label22)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label21)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.Label28)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label20)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.Label29)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lblElevUnit)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmElevScenario"
        Me.ShowIcon = False
        Me.Text = "Elevation Representation Analysis"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblElevUnit As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents txtMinElev As System.Windows.Forms.TextBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents txtMaxElev As System.Windows.Forms.TextBox
    Friend WithEvents txtZUnit4 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label28 As System.Windows.Forms.Label
    Friend WithEvents txtSNOTELMin As System.Windows.Forms.TextBox
    Friend WithEvents txtSCourseMin As System.Windows.Forms.TextBox
    Friend WithEvents txtActualLow As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtSNOTELMax As System.Windows.Forms.TextBox
    Friend WithEvents txtSCourseMax As System.Windows.Forms.TextBox
    Friend WithEvents txtActualHigh As System.Windows.Forms.TextBox
    Friend WithEvents txtZUnit1 As System.Windows.Forms.Label
    Friend WithEvents txtZUnit2 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents txtA_BelowArea1 As System.Windows.Forms.TextBox
    Friend WithEvents txtA_BelowArea2 As System.Windows.Forms.TextBox
    Friend WithEvents txtA_BelowPercent As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents txtA_AboveArea1 As System.Windows.Forms.TextBox
    Friend WithEvents txtA_AboveArea2 As System.Windows.Forms.TextBox
    Friend WithEvents txtA_AbovePercent As System.Windows.Forms.TextBox
    Friend WithEvents txtA_TotalArea1 As System.Windows.Forms.TextBox
    Friend WithEvents txtA_TotalArea2 As System.Windows.Forms.TextBox
    Friend WithEvents txtA_TotalPercent As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtP_BelowArea1 As System.Windows.Forms.TextBox
    Friend WithEvents txtP_AboveArea1 As System.Windows.Forms.TextBox
    Friend WithEvents txtP_BelowArea2 As System.Windows.Forms.TextBox
    Friend WithEvents txtP_TotalArea1 As System.Windows.Forms.TextBox
    Friend WithEvents txtP_AboveArea2 As System.Windows.Forms.TextBox
    Friend WithEvents txtP_TotalArea2 As System.Windows.Forms.TextBox
    Friend WithEvents txtP_BelowPercent As System.Windows.Forms.TextBox
    Friend WithEvents txtP_AbovePercent As System.Windows.Forms.TextBox
    Friend WithEvents txtP_TotalPercent As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtNewLow As System.Windows.Forms.TextBox
    Friend WithEvents txtNewHigh As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtZUnit3 As System.Windows.Forms.Label
    Friend WithEvents CmdCalculate As System.Windows.Forms.Button
    Friend WithEvents cmdClose As System.Windows.Forms.Button
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents txtLowerBuffer As System.Windows.Forms.TextBox
    Friend WithEvents txtZUnit5 As System.Windows.Forms.Label
    Friend WithEvents Label27 As System.Windows.Forms.Label
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents Label30 As System.Windows.Forms.Label
    Friend WithEvents OptZMeters As System.Windows.Forms.RadioButton
    Friend WithEvents OptZFeet As System.Windows.Forms.RadioButton
End Class
