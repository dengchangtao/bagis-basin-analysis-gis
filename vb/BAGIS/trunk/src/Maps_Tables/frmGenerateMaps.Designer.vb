<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGenerateMaps
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
        Me.FrameInfo = New System.Windows.Forms.GroupBox()
        Me.txtRangeElev = New System.Windows.Forms.TextBox()
        Me.txtAreaSQMile = New System.Windows.Forms.TextBox()
        Me.txtMaxElev = New System.Windows.Forms.TextBox()
        Me.txtMinElev = New System.Windows.Forms.TextBox()
        Me.txtAreaAcre = New System.Windows.Forms.TextBox()
        Me.txtArea = New System.Windows.Forms.TextBox()
        Me.lblElevUnit = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.OptZFeet = New System.Windows.Forms.RadioButton()
        Me.OptZMeters = New System.Windows.Forms.RadioButton()
        Me.FrameElevationDist = New System.Windows.Forms.GroupBox()
        Me.lstintervals = New System.Windows.Forms.ListView()
        Me.Intervals = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Pct_Area = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Snotel = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.SnowCourse = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmdApplyElevInterval = New System.Windows.Forms.Button()
        Me.txtElevClassNumber = New System.Windows.Forms.TextBox()
        Me.CmboxElevInterval = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ComboxSubDivide = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.chkUseRange = New System.Windows.Forms.CheckBox()
        Me.FrameElevationRange = New System.Windows.Forms.GroupBox()
        Me.OptSelTo = New System.Windows.Forms.RadioButton()
        Me.OptSelFrom = New System.Windows.Forms.RadioButton()
        Me.lstElevRange = New System.Windows.Forms.ListBox()
        Me.lblSelNote = New System.Windows.Forms.Label()
        Me.lblSelectType = New System.Windows.Forms.Label()
        Me.lblToElev = New System.Windows.Forms.Label()
        Me.lblFromElev = New System.Windows.Forms.Label()
        Me.txtToElev = New System.Windows.Forms.TextBox()
        Me.txtFromElev = New System.Windows.Forms.TextBox()
        Me.FramePrecipitationDist = New System.Windows.Forms.GroupBox()
        Me.txtRangePrecip = New System.Windows.Forms.TextBox()
        Me.txtMaxPrecip = New System.Windows.Forms.TextBox()
        Me.txtPrecipMapZoneInt = New System.Windows.Forms.TextBox()
        Me.txtPrecipMapZoneNo = New System.Windows.Forms.TextBox()
        Me.txtMinPrecip = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.lstPrecipZones = New System.Windows.Forms.ListBox()
        Me.cmdApplyPRISMInterval = New System.Windows.Forms.Button()
        Me.cmdPRISM = New System.Windows.Forms.Button()
        Me.CmboxEnd = New System.Windows.Forms.ComboBox()
        Me.CmboxBegin = New System.Windows.Forms.ComboBox()
        Me.CmboxPrecipType = New System.Windows.Forms.ComboBox()
        Me.lblEndMonth = New System.Windows.Forms.Label()
        Me.lblBeginMonth = New System.Windows.Forms.Label()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.CmdGenerate = New System.Windows.Forms.Button()
        Me.CmdTables = New System.Windows.Forms.Button()
        Me.CmbClose = New System.Windows.Forms.Button()
        Me.CmdMaps = New System.Windows.Forms.Button()
        Me.lstDataStatus = New System.Windows.Forms.ListView()
        Me.DataStatus = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.DataDescription = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.RasterName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.CmboxAspect = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.FrameInfo.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.FrameElevationDist.SuspendLayout()
        Me.FrameElevationRange.SuspendLayout()
        Me.FramePrecipitationDist.SuspendLayout()
        Me.SuspendLayout()
        '
        'FrameInfo
        '
        Me.FrameInfo.Controls.Add(Me.txtRangeElev)
        Me.FrameInfo.Controls.Add(Me.txtAreaSQMile)
        Me.FrameInfo.Controls.Add(Me.txtMaxElev)
        Me.FrameInfo.Controls.Add(Me.txtMinElev)
        Me.FrameInfo.Controls.Add(Me.txtAreaAcre)
        Me.FrameInfo.Controls.Add(Me.txtArea)
        Me.FrameInfo.Controls.Add(Me.lblElevUnit)
        Me.FrameInfo.Controls.Add(Me.Label3)
        Me.FrameInfo.Controls.Add(Me.Label2)
        Me.FrameInfo.Controls.Add(Me.Label15)
        Me.FrameInfo.Controls.Add(Me.Label1)
        Me.FrameInfo.Controls.Add(Me.Label14)
        Me.FrameInfo.Controls.Add(Me.Label13)
        Me.FrameInfo.Controls.Add(Me.Label4)
        Me.FrameInfo.Location = New System.Drawing.Point(7, 6)
        Me.FrameInfo.Name = "FrameInfo"
        Me.FrameInfo.Size = New System.Drawing.Size(357, 110)
        Me.FrameInfo.TabIndex = 0
        Me.FrameInfo.TabStop = False
        '
        'txtRangeElev
        '
        Me.txtRangeElev.BackColor = System.Drawing.SystemColors.Menu
        Me.txtRangeElev.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtRangeElev.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.txtRangeElev.ForeColor = System.Drawing.Color.Blue
        Me.txtRangeElev.Location = New System.Drawing.Point(247, 78)
        Me.txtRangeElev.Name = "txtRangeElev"
        Me.txtRangeElev.Size = New System.Drawing.Size(100, 16)
        Me.txtRangeElev.TabIndex = 2
        Me.txtRangeElev.Text = "0"
        Me.txtRangeElev.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAreaSQMile
        '
        Me.txtAreaSQMile.BackColor = System.Drawing.SystemColors.Menu
        Me.txtAreaSQMile.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtAreaSQMile.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.txtAreaSQMile.ForeColor = System.Drawing.Color.Blue
        Me.txtAreaSQMile.Location = New System.Drawing.Point(10, 79)
        Me.txtAreaSQMile.Name = "txtAreaSQMile"
        Me.txtAreaSQMile.Size = New System.Drawing.Size(100, 16)
        Me.txtAreaSQMile.TabIndex = 2
        Me.txtAreaSQMile.Text = "0"
        Me.txtAreaSQMile.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMaxElev
        '
        Me.txtMaxElev.BackColor = System.Drawing.SystemColors.Menu
        Me.txtMaxElev.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtMaxElev.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.txtMaxElev.ForeColor = System.Drawing.Color.Blue
        Me.txtMaxElev.Location = New System.Drawing.Point(247, 56)
        Me.txtMaxElev.Name = "txtMaxElev"
        Me.txtMaxElev.Size = New System.Drawing.Size(100, 16)
        Me.txtMaxElev.TabIndex = 2
        Me.txtMaxElev.Text = "0"
        Me.txtMaxElev.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtMinElev
        '
        Me.txtMinElev.BackColor = System.Drawing.SystemColors.Menu
        Me.txtMinElev.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtMinElev.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.txtMinElev.ForeColor = System.Drawing.Color.Blue
        Me.txtMinElev.Location = New System.Drawing.Point(247, 32)
        Me.txtMinElev.Name = "txtMinElev"
        Me.txtMinElev.Size = New System.Drawing.Size(100, 16)
        Me.txtMinElev.TabIndex = 2
        Me.txtMinElev.Text = "0"
        Me.txtMinElev.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAreaAcre
        '
        Me.txtAreaAcre.BackColor = System.Drawing.SystemColors.Menu
        Me.txtAreaAcre.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtAreaAcre.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.txtAreaAcre.ForeColor = System.Drawing.Color.Blue
        Me.txtAreaAcre.Location = New System.Drawing.Point(10, 57)
        Me.txtAreaAcre.Name = "txtAreaAcre"
        Me.txtAreaAcre.Size = New System.Drawing.Size(100, 16)
        Me.txtAreaAcre.TabIndex = 2
        Me.txtAreaAcre.Text = "0"
        Me.txtAreaAcre.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtArea
        '
        Me.txtArea.BackColor = System.Drawing.SystemColors.Menu
        Me.txtArea.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtArea.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.txtArea.ForeColor = System.Drawing.Color.Blue
        Me.txtArea.Location = New System.Drawing.Point(10, 33)
        Me.txtArea.Name = "txtArea"
        Me.txtArea.Size = New System.Drawing.Size(100, 16)
        Me.txtArea.TabIndex = 2
        Me.txtArea.Text = "0"
        Me.txtArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblElevUnit
        '
        Me.lblElevUnit.AutoSize = True
        Me.lblElevUnit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblElevUnit.Location = New System.Drawing.Point(205, 12)
        Me.lblElevUnit.Name = "lblElevUnit"
        Me.lblElevUnit.Size = New System.Drawing.Size(99, 16)
        Me.lblElevUnit.TabIndex = 1
        Me.lblElevUnit.Text = "Elevation (ft):"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(199, 79)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(44, 15)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Range"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(212, 58)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(31, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Max"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(122, 79)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(52, 15)
        Me.Label15.TabIndex = 1
        Me.Label15.Text = "Sq. Mile"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(215, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(28, 15)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Min"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(122, 57)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(31, 15)
        Me.Label14.TabIndex = 1
        Me.Label14.Text = "Acre"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(122, 33)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(47, 15)
        Me.Label13.TabIndex = 1
        Me.Label13.Text = "Sq. Km"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(26, 12)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(74, 16)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "AOI Area:"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.OptZFeet)
        Me.GroupBox1.Controls.Add(Me.OptZMeters)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(370, 22)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(121, 79)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Elevation Unit"
        '
        'OptZFeet
        '
        Me.OptZFeet.AutoSize = True
        Me.OptZFeet.Location = New System.Drawing.Point(26, 47)
        Me.OptZFeet.Name = "OptZFeet"
        Me.OptZFeet.Size = New System.Drawing.Size(55, 22)
        Me.OptZFeet.TabIndex = 2
        Me.OptZFeet.TabStop = True
        Me.OptZFeet.Text = "Feet"
        Me.OptZFeet.UseVisualStyleBackColor = True
        '
        'OptZMeters
        '
        Me.OptZMeters.AutoSize = True
        Me.OptZMeters.Location = New System.Drawing.Point(26, 21)
        Me.OptZMeters.Name = "OptZMeters"
        Me.OptZMeters.Size = New System.Drawing.Size(72, 22)
        Me.OptZMeters.TabIndex = 2
        Me.OptZMeters.TabStop = True
        Me.OptZMeters.Text = "Meters"
        Me.OptZMeters.UseVisualStyleBackColor = True
        '
        'FrameElevationDist
        '
        Me.FrameElevationDist.Controls.Add(Me.lstintervals)
        Me.FrameElevationDist.Controls.Add(Me.cmdApplyElevInterval)
        Me.FrameElevationDist.Controls.Add(Me.txtElevClassNumber)
        Me.FrameElevationDist.Controls.Add(Me.CmboxElevInterval)
        Me.FrameElevationDist.Controls.Add(Me.Label6)
        Me.FrameElevationDist.Controls.Add(Me.Label5)
        Me.FrameElevationDist.Controls.Add(Me.ComboxSubDivide)
        Me.FrameElevationDist.Controls.Add(Me.Label8)
        Me.FrameElevationDist.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!)
        Me.FrameElevationDist.Location = New System.Drawing.Point(12, 122)
        Me.FrameElevationDist.Name = "FrameElevationDist"
        Me.FrameElevationDist.Size = New System.Drawing.Size(478, 249)
        Me.FrameElevationDist.TabIndex = 3
        Me.FrameElevationDist.TabStop = False
        Me.FrameElevationDist.Text = "Elevation Zones for Precipitation Analysis"
        '
        'lstintervals
        '
        Me.lstintervals.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Intervals, Me.Pct_Area, Me.Snotel, Me.SnowCourse})
        Me.lstintervals.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstintervals.Location = New System.Drawing.Point(5, 57)
        Me.lstintervals.Name = "lstintervals"
        Me.lstintervals.Size = New System.Drawing.Size(459, 152)
        Me.lstintervals.TabIndex = 11
        Me.lstintervals.UseCompatibleStateImageBehavior = False
        Me.lstintervals.View = System.Windows.Forms.View.Details
        '
        'Intervals
        '
        Me.Intervals.Text = "Intervals"
        Me.Intervals.Width = 110
        '
        'Pct_Area
        '
        Me.Pct_Area.Text = "% Area"
        Me.Pct_Area.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.Pct_Area.Width = 100
        '
        'Snotel
        '
        Me.Snotel.Text = "# SNOTEL"
        Me.Snotel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.Snotel.Width = 100
        '
        'SnowCourse
        '
        Me.SnowCourse.Text = "# Snow Course"
        Me.SnowCourse.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.SnowCourse.Width = 125
        '
        'cmdApplyElevInterval
        '
        Me.cmdApplyElevInterval.Enabled = False
        Me.cmdApplyElevInterval.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApplyElevInterval.Location = New System.Drawing.Point(347, 23)
        Me.cmdApplyElevInterval.Name = "cmdApplyElevInterval"
        Me.cmdApplyElevInterval.Size = New System.Drawing.Size(109, 28)
        Me.cmdApplyElevInterval.TabIndex = 10
        Me.cmdApplyElevInterval.Text = "1. Apply"
        Me.cmdApplyElevInterval.UseVisualStyleBackColor = True
        '
        'txtElevClassNumber
        '
        Me.txtElevClassNumber.BackColor = System.Drawing.SystemColors.Menu
        Me.txtElevClassNumber.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtElevClassNumber.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtElevClassNumber.ForeColor = System.Drawing.Color.Blue
        Me.txtElevClassNumber.Location = New System.Drawing.Point(264, 28)
        Me.txtElevClassNumber.Name = "txtElevClassNumber"
        Me.txtElevClassNumber.Size = New System.Drawing.Size(63, 15)
        Me.txtElevClassNumber.TabIndex = 9
        Me.txtElevClassNumber.Text = "0"
        Me.txtElevClassNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'CmboxElevInterval
        '
        Me.CmboxElevInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmboxElevInterval.FormattingEnabled = True
        Me.CmboxElevInterval.Location = New System.Drawing.Point(70, 23)
        Me.CmboxElevInterval.Name = "CmboxElevInterval"
        Me.CmboxElevInterval.Size = New System.Drawing.Size(109, 26)
        Me.CmboxElevInterval.TabIndex = 8
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(187, 26)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(70, 16)
        Me.Label6.TabIndex = 7
        Me.Label6.Text = "# Classes:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(7, 25)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(58, 18)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Interval:"
        '
        'ComboxSubDivide
        '
        Me.ComboxSubDivide.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboxSubDivide.FormattingEnabled = True
        Me.ComboxSubDivide.Location = New System.Drawing.Point(318, 217)
        Me.ComboxSubDivide.Name = "ComboxSubDivide"
        Me.ComboxSubDivide.Size = New System.Drawing.Size(91, 26)
        Me.ComboxSubDivide.TabIndex = 1
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(50, 221)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(262, 16)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Elevation Subdivisions on Elevation Curve:"
        '
        'chkUseRange
        '
        Me.chkUseRange.AutoSize = True
        Me.chkUseRange.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkUseRange.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkUseRange.Location = New System.Drawing.Point(12, 377)
        Me.chkUseRange.Name = "chkUseRange"
        Me.chkUseRange.Size = New System.Drawing.Size(377, 20)
        Me.chkUseRange.TabIndex = 4
        Me.chkUseRange.Text = "Generate Tables and Charts for Specified Elevation Range"
        Me.chkUseRange.UseVisualStyleBackColor = True
        '
        'FrameElevationRange
        '
        Me.FrameElevationRange.Controls.Add(Me.OptSelTo)
        Me.FrameElevationRange.Controls.Add(Me.OptSelFrom)
        Me.FrameElevationRange.Controls.Add(Me.lstElevRange)
        Me.FrameElevationRange.Controls.Add(Me.lblSelNote)
        Me.FrameElevationRange.Controls.Add(Me.lblSelectType)
        Me.FrameElevationRange.Controls.Add(Me.lblToElev)
        Me.FrameElevationRange.Controls.Add(Me.lblFromElev)
        Me.FrameElevationRange.Controls.Add(Me.txtToElev)
        Me.FrameElevationRange.Controls.Add(Me.txtFromElev)
        Me.FrameElevationRange.Location = New System.Drawing.Point(12, 396)
        Me.FrameElevationRange.Name = "FrameElevationRange"
        Me.FrameElevationRange.Size = New System.Drawing.Size(479, 133)
        Me.FrameElevationRange.TabIndex = 5
        Me.FrameElevationRange.TabStop = False
        '
        'OptSelTo
        '
        Me.OptSelTo.AutoSize = True
        Me.OptSelTo.Enabled = False
        Me.OptSelTo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptSelTo.Location = New System.Drawing.Point(173, 106)
        Me.OptSelTo.Name = "OptSelTo"
        Me.OptSelTo.Size = New System.Drawing.Size(43, 20)
        Me.OptSelTo.TabIndex = 14
        Me.OptSelTo.TabStop = True
        Me.OptSelTo.Text = "To"
        Me.OptSelTo.UseVisualStyleBackColor = True
        '
        'OptSelFrom
        '
        Me.OptSelFrom.AutoSize = True
        Me.OptSelFrom.Checked = True
        Me.OptSelFrom.Enabled = False
        Me.OptSelFrom.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OptSelFrom.Location = New System.Drawing.Point(97, 106)
        Me.OptSelFrom.Name = "OptSelFrom"
        Me.OptSelFrom.Size = New System.Drawing.Size(57, 20)
        Me.OptSelFrom.TabIndex = 14
        Me.OptSelFrom.TabStop = True
        Me.OptSelFrom.Text = "From"
        Me.OptSelFrom.UseVisualStyleBackColor = True
        '
        'lstElevRange
        '
        Me.lstElevRange.Enabled = False
        Me.lstElevRange.FormattingEnabled = True
        Me.lstElevRange.Location = New System.Drawing.Point(293, 15)
        Me.lstElevRange.Name = "lstElevRange"
        Me.lstElevRange.Size = New System.Drawing.Size(171, 108)
        Me.lstElevRange.TabIndex = 13
        '
        'lblSelNote
        '
        Me.lblSelNote.AutoSize = True
        Me.lblSelNote.Enabled = False
        Me.lblSelNote.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSelNote.Location = New System.Drawing.Point(191, 19)
        Me.lblSelNote.Name = "lblSelNote"
        Me.lblSelNote.Size = New System.Drawing.Size(100, 15)
        Me.lblSelNote.TabIndex = 7
        Me.lblSelNote.Text = "Click to set value:"
        '
        'lblSelectType
        '
        Me.lblSelectType.AutoSize = True
        Me.lblSelectType.Enabled = False
        Me.lblSelectType.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSelectType.Location = New System.Drawing.Point(17, 108)
        Me.lblSelectType.Name = "lblSelectType"
        Me.lblSelectType.Size = New System.Drawing.Size(67, 16)
        Me.lblSelectType.TabIndex = 7
        Me.lblSelectType.Text = "Set value:"
        '
        'lblToElev
        '
        Me.lblToElev.AutoSize = True
        Me.lblToElev.Enabled = False
        Me.lblToElev.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblToElev.Location = New System.Drawing.Point(15, 48)
        Me.lblToElev.Name = "lblToElev"
        Me.lblToElev.Size = New System.Drawing.Size(76, 15)
        Me.lblToElev.TabIndex = 7
        Me.lblToElev.Text = "To elevation:"
        '
        'lblFromElev
        '
        Me.lblFromElev.AutoSize = True
        Me.lblFromElev.Enabled = False
        Me.lblFromElev.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFromElev.Location = New System.Drawing.Point(9, 16)
        Me.lblFromElev.Name = "lblFromElev"
        Me.lblFromElev.Size = New System.Drawing.Size(91, 15)
        Me.lblFromElev.TabIndex = 7
        Me.lblFromElev.Text = "From elevation:"
        '
        'txtToElev
        '
        Me.txtToElev.BackColor = System.Drawing.SystemColors.Window
        Me.txtToElev.Enabled = False
        Me.txtToElev.Location = New System.Drawing.Point(94, 46)
        Me.txtToElev.Name = "txtToElev"
        Me.txtToElev.Size = New System.Drawing.Size(82, 20)
        Me.txtToElev.TabIndex = 12
        Me.txtToElev.Text = "0"
        Me.txtToElev.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtFromElev
        '
        Me.txtFromElev.BackColor = System.Drawing.SystemColors.Window
        Me.txtFromElev.Enabled = False
        Me.txtFromElev.Location = New System.Drawing.Point(94, 15)
        Me.txtFromElev.Name = "txtFromElev"
        Me.txtFromElev.Size = New System.Drawing.Size(82, 20)
        Me.txtFromElev.TabIndex = 12
        Me.txtFromElev.Text = "0"
        Me.txtFromElev.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'FramePrecipitationDist
        '
        Me.FramePrecipitationDist.Controls.Add(Me.txtRangePrecip)
        Me.FramePrecipitationDist.Controls.Add(Me.txtMaxPrecip)
        Me.FramePrecipitationDist.Controls.Add(Me.txtPrecipMapZoneInt)
        Me.FramePrecipitationDist.Controls.Add(Me.txtPrecipMapZoneNo)
        Me.FramePrecipitationDist.Controls.Add(Me.txtMinPrecip)
        Me.FramePrecipitationDist.Controls.Add(Me.Label18)
        Me.FramePrecipitationDist.Controls.Add(Me.Label17)
        Me.FramePrecipitationDist.Controls.Add(Me.Label20)
        Me.FramePrecipitationDist.Controls.Add(Me.lstPrecipZones)
        Me.FramePrecipitationDist.Controls.Add(Me.cmdApplyPRISMInterval)
        Me.FramePrecipitationDist.Controls.Add(Me.cmdPRISM)
        Me.FramePrecipitationDist.Controls.Add(Me.CmboxEnd)
        Me.FramePrecipitationDist.Controls.Add(Me.CmboxBegin)
        Me.FramePrecipitationDist.Controls.Add(Me.CmboxPrecipType)
        Me.FramePrecipitationDist.Controls.Add(Me.lblEndMonth)
        Me.FramePrecipitationDist.Controls.Add(Me.lblBeginMonth)
        Me.FramePrecipitationDist.Controls.Add(Me.Label26)
        Me.FramePrecipitationDist.Controls.Add(Me.Label19)
        Me.FramePrecipitationDist.Controls.Add(Me.Label21)
        Me.FramePrecipitationDist.Controls.Add(Me.Label22)
        Me.FramePrecipitationDist.Controls.Add(Me.Label16)
        Me.FramePrecipitationDist.Controls.Add(Me.Label7)
        Me.FramePrecipitationDist.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FramePrecipitationDist.Location = New System.Drawing.Point(497, 197)
        Me.FramePrecipitationDist.Name = "FramePrecipitationDist"
        Me.FramePrecipitationDist.Size = New System.Drawing.Size(443, 216)
        Me.FramePrecipitationDist.TabIndex = 0
        Me.FramePrecipitationDist.TabStop = False
        Me.FramePrecipitationDist.Text = "Precipitation Distribution Map"
        '
        'txtRangePrecip
        '
        Me.txtRangePrecip.BackColor = System.Drawing.SystemColors.Menu
        Me.txtRangePrecip.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtRangePrecip.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRangePrecip.ForeColor = System.Drawing.Color.Blue
        Me.txtRangePrecip.Location = New System.Drawing.Point(50, 179)
        Me.txtRangePrecip.Name = "txtRangePrecip"
        Me.txtRangePrecip.Size = New System.Drawing.Size(69, 15)
        Me.txtRangePrecip.TabIndex = 12
        Me.txtRangePrecip.Text = "0"
        '
        'txtMaxPrecip
        '
        Me.txtMaxPrecip.BackColor = System.Drawing.SystemColors.Menu
        Me.txtMaxPrecip.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtMaxPrecip.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMaxPrecip.ForeColor = System.Drawing.Color.Blue
        Me.txtMaxPrecip.Location = New System.Drawing.Point(50, 146)
        Me.txtMaxPrecip.Name = "txtMaxPrecip"
        Me.txtMaxPrecip.Size = New System.Drawing.Size(69, 15)
        Me.txtMaxPrecip.TabIndex = 12
        Me.txtMaxPrecip.Text = "0"
        '
        'txtPrecipMapZoneInt
        '
        Me.txtPrecipMapZoneInt.BackColor = System.Drawing.SystemColors.Window
        Me.txtPrecipMapZoneInt.Location = New System.Drawing.Point(308, 110)
        Me.txtPrecipMapZoneInt.Name = "txtPrecipMapZoneInt"
        Me.txtPrecipMapZoneInt.Size = New System.Drawing.Size(69, 24)
        Me.txtPrecipMapZoneInt.TabIndex = 12
        Me.txtPrecipMapZoneInt.Text = "0"
        Me.txtPrecipMapZoneInt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtPrecipMapZoneNo
        '
        Me.txtPrecipMapZoneNo.BackColor = System.Drawing.SystemColors.Menu
        Me.txtPrecipMapZoneNo.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtPrecipMapZoneNo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPrecipMapZoneNo.ForeColor = System.Drawing.Color.Blue
        Me.txtPrecipMapZoneNo.Location = New System.Drawing.Point(389, 144)
        Me.txtPrecipMapZoneNo.Name = "txtPrecipMapZoneNo"
        Me.txtPrecipMapZoneNo.Size = New System.Drawing.Size(48, 15)
        Me.txtPrecipMapZoneNo.TabIndex = 12
        Me.txtPrecipMapZoneNo.Text = "10"
        '
        'txtMinPrecip
        '
        Me.txtMinPrecip.BackColor = System.Drawing.SystemColors.Menu
        Me.txtMinPrecip.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtMinPrecip.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMinPrecip.ForeColor = System.Drawing.Color.Blue
        Me.txtMinPrecip.Location = New System.Drawing.Point(50, 112)
        Me.txtMinPrecip.Name = "txtMinPrecip"
        Me.txtMinPrecip.Size = New System.Drawing.Size(69, 15)
        Me.txtMinPrecip.TabIndex = 12
        Me.txtMinPrecip.Text = "0"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(3, 180)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(39, 13)
        Me.Label18.TabIndex = 8
        Me.Label18.Text = "Range"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(14, 147)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(27, 13)
        Me.Label17.TabIndex = 8
        Me.Label17.Text = "Max"
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(17, 116)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(24, 13)
        Me.Label20.TabIndex = 8
        Me.Label20.Text = "Min"
        '
        'lstPrecipZones
        '
        Me.lstPrecipZones.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstPrecipZones.FormattingEnabled = True
        Me.lstPrecipZones.ItemHeight = 15
        Me.lstPrecipZones.Location = New System.Drawing.Point(125, 110)
        Me.lstPrecipZones.Name = "lstPrecipZones"
        Me.lstPrecipZones.Size = New System.Drawing.Size(153, 94)
        Me.lstPrecipZones.TabIndex = 11
        '
        'cmdApplyPRISMInterval
        '
        Me.cmdApplyPRISMInterval.Enabled = False
        Me.cmdApplyPRISMInterval.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdApplyPRISMInterval.Location = New System.Drawing.Point(308, 174)
        Me.cmdApplyPRISMInterval.Name = "cmdApplyPRISMInterval"
        Me.cmdApplyPRISMInterval.Size = New System.Drawing.Size(122, 28)
        Me.cmdApplyPRISMInterval.TabIndex = 10
        Me.cmdApplyPRISMInterval.Text = "3. Apply"
        Me.cmdApplyPRISMInterval.UseVisualStyleBackColor = True
        '
        'cmdPRISM
        '
        Me.cmdPRISM.Enabled = False
        Me.cmdPRISM.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPRISM.Location = New System.Drawing.Point(308, 21)
        Me.cmdPRISM.Name = "cmdPRISM"
        Me.cmdPRISM.Size = New System.Drawing.Size(122, 28)
        Me.cmdPRISM.TabIndex = 10
        Me.cmdPRISM.Text = "2. Get Range"
        Me.cmdPRISM.UseVisualStyleBackColor = True
        '
        'CmboxEnd
        '
        Me.CmboxEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmboxEnd.Enabled = False
        Me.CmboxEnd.FormattingEnabled = True
        Me.CmboxEnd.Location = New System.Drawing.Point(222, 55)
        Me.CmboxEnd.Name = "CmboxEnd"
        Me.CmboxEnd.Size = New System.Drawing.Size(67, 26)
        Me.CmboxEnd.TabIndex = 8
        '
        'CmboxBegin
        '
        Me.CmboxBegin.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmboxBegin.Enabled = False
        Me.CmboxBegin.FormattingEnabled = True
        Me.CmboxBegin.Location = New System.Drawing.Point(94, 55)
        Me.CmboxBegin.Name = "CmboxBegin"
        Me.CmboxBegin.Size = New System.Drawing.Size(67, 26)
        Me.CmboxBegin.TabIndex = 8
        '
        'CmboxPrecipType
        '
        Me.CmboxPrecipType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmboxPrecipType.FormattingEnabled = True
        Me.CmboxPrecipType.Location = New System.Drawing.Point(94, 23)
        Me.CmboxPrecipType.Name = "CmboxPrecipType"
        Me.CmboxPrecipType.Size = New System.Drawing.Size(195, 26)
        Me.CmboxPrecipType.TabIndex = 8
        '
        'lblEndMonth
        '
        Me.lblEndMonth.AutoSize = True
        Me.lblEndMonth.Enabled = False
        Me.lblEndMonth.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEndMonth.Location = New System.Drawing.Point(189, 60)
        Me.lblEndMonth.Name = "lblEndMonth"
        Me.lblEndMonth.Size = New System.Drawing.Size(28, 16)
        Me.lblEndMonth.TabIndex = 7
        Me.lblEndMonth.Text = "To:"
        '
        'lblBeginMonth
        '
        Me.lblBeginMonth.AutoSize = True
        Me.lblBeginMonth.Enabled = False
        Me.lblBeginMonth.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBeginMonth.Location = New System.Drawing.Point(46, 60)
        Me.lblBeginMonth.Name = "lblBeginMonth"
        Me.lblBeginMonth.Size = New System.Drawing.Size(42, 16)
        Me.lblBeginMonth.TabIndex = 7
        Me.lblBeginMonth.Text = "From:"
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label26.Location = New System.Drawing.Point(383, 114)
        Me.Label26.Name = "Label26"
        Me.Label26.Size = New System.Drawing.Size(47, 16)
        Me.Label26.TabIndex = 7
        Me.Label26.Text = "Inches"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(283, 144)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(94, 16)
        Me.Label19.TabIndex = 7
        Me.Label19.Text = "Precip Zone #:"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(305, 94)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(78, 13)
        Me.Label21.TabIndex = 7
        Me.Label21.Text = "Precip Interval:"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.Location = New System.Drawing.Point(145, 94)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(98, 13)
        Me.Label22.TabIndex = 7
        Me.Label22.Text = "Precipitation Zones"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(6, 94)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(105, 13)
        Me.Label16.TabIndex = 7
        Me.Label16.Text = "Precipitation (inches)"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(6, 25)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(82, 16)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "PRISM Data"
        '
        'CmdGenerate
        '
        Me.CmdGenerate.Enabled = False
        Me.CmdGenerate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdGenerate.Location = New System.Drawing.Point(528, 434)
        Me.CmdGenerate.Name = "CmdGenerate"
        Me.CmdGenerate.Size = New System.Drawing.Size(177, 28)
        Me.CmdGenerate.TabIndex = 10
        Me.CmdGenerate.Text = "4. Generate Zones"
        Me.CmdGenerate.UseVisualStyleBackColor = True
        '
        'CmdTables
        '
        Me.CmdTables.Enabled = False
        Me.CmdTables.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdTables.Location = New System.Drawing.Point(528, 480)
        Me.CmdTables.Name = "CmdTables"
        Me.CmdTables.Size = New System.Drawing.Size(77, 28)
        Me.CmdTables.TabIndex = 10
        Me.CmdTables.Text = "Tables"
        Me.CmdTables.UseVisualStyleBackColor = True
        '
        'CmbClose
        '
        Me.CmbClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmbClose.Location = New System.Drawing.Point(827, 480)
        Me.CmbClose.Name = "CmbClose"
        Me.CmbClose.Size = New System.Drawing.Size(100, 28)
        Me.CmbClose.TabIndex = 10
        Me.CmbClose.Text = "Close"
        Me.CmbClose.UseVisualStyleBackColor = True
        '
        'CmdMaps
        '
        Me.CmdMaps.Enabled = False
        Me.CmdMaps.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdMaps.Location = New System.Drawing.Point(628, 480)
        Me.CmdMaps.Name = "CmdMaps"
        Me.CmdMaps.Size = New System.Drawing.Size(77, 28)
        Me.CmdMaps.TabIndex = 10
        Me.CmdMaps.Text = "Maps"
        Me.CmdMaps.UseVisualStyleBackColor = True
        '
        'lstDataStatus
        '
        Me.lstDataStatus.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.DataStatus, Me.DataDescription, Me.RasterName})
        Me.lstDataStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lstDataStatus.Location = New System.Drawing.Point(506, 18)
        Me.lstDataStatus.Name = "lstDataStatus"
        Me.lstDataStatus.Size = New System.Drawing.Size(434, 173)
        Me.lstDataStatus.TabIndex = 11
        Me.lstDataStatus.UseCompatibleStateImageBehavior = False
        Me.lstDataStatus.View = System.Windows.Forms.View.Details
        '
        'DataStatus
        '
        Me.DataStatus.Text = "Data Status"
        Me.DataStatus.Width = 100
        '
        'DataDescription
        '
        Me.DataDescription.Text = "Data Description"
        Me.DataDescription.Width = 200
        '
        'RasterName
        '
        Me.RasterName.Text = "Raster Name"
        Me.RasterName.Width = 130
        '
        'CmboxAspect
        '
        Me.CmboxAspect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmboxAspect.FormattingEnabled = True
        Me.CmboxAspect.Location = New System.Drawing.Point(860, 438)
        Me.CmboxAspect.Name = "CmboxAspect"
        Me.CmboxAspect.Size = New System.Drawing.Size(67, 21)
        Me.CmboxAspect.TabIndex = 12
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(728, 439)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(126, 16)
        Me.Label9.TabIndex = 13
        Me.Label9.Text = "Aspect Directions #:"
        '
        'frmGenerateMaps
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(952, 534)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.CmboxAspect)
        Me.Controls.Add(Me.lstDataStatus)
        Me.Controls.Add(Me.FramePrecipitationDist)
        Me.Controls.Add(Me.FrameElevationRange)
        Me.Controls.Add(Me.chkUseRange)
        Me.Controls.Add(Me.FrameElevationDist)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.CmdMaps)
        Me.Controls.Add(Me.CmdTables)
        Me.Controls.Add(Me.CmbClose)
        Me.Controls.Add(Me.CmdGenerate)
        Me.Controls.Add(Me.FrameInfo)
        Me.Name = "frmGenerateMaps"
        Me.ShowIcon = False
        Me.Text = "Map Settings"
        Me.FrameInfo.ResumeLayout(False)
        Me.FrameInfo.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.FrameElevationDist.ResumeLayout(False)
        Me.FrameElevationDist.PerformLayout()
        Me.FrameElevationRange.ResumeLayout(False)
        Me.FrameElevationRange.PerformLayout()
        Me.FramePrecipitationDist.ResumeLayout(False)
        Me.FramePrecipitationDist.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents FrameInfo As System.Windows.Forms.GroupBox
    Friend WithEvents lblElevUnit As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtRangeElev As System.Windows.Forms.TextBox
    Friend WithEvents txtAreaSQMile As System.Windows.Forms.TextBox
    Friend WithEvents txtMaxElev As System.Windows.Forms.TextBox
    Friend WithEvents txtMinElev As System.Windows.Forms.TextBox
    Friend WithEvents txtAreaAcre As System.Windows.Forms.TextBox
    Friend WithEvents txtArea As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents OptZFeet As System.Windows.Forms.RadioButton
    Friend WithEvents OptZMeters As System.Windows.Forms.RadioButton
    Friend WithEvents FrameElevationDist As System.Windows.Forms.GroupBox
    Friend WithEvents chkUseRange As System.Windows.Forms.CheckBox
    Friend WithEvents FrameElevationRange As System.Windows.Forms.GroupBox
    Friend WithEvents FramePrecipitationDist As System.Windows.Forms.GroupBox
    Friend WithEvents ComboxSubDivide As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents cmdApplyElevInterval As System.Windows.Forms.Button
    Friend WithEvents txtElevClassNumber As System.Windows.Forms.TextBox
    Friend WithEvents CmboxElevInterval As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cmdPRISM As System.Windows.Forms.Button
    Friend WithEvents CmboxEnd As System.Windows.Forms.ComboBox
    Friend WithEvents CmboxBegin As System.Windows.Forms.ComboBox
    Friend WithEvents CmboxPrecipType As System.Windows.Forms.ComboBox
    Friend WithEvents lblEndMonth As System.Windows.Forms.Label
    Friend WithEvents lblBeginMonth As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents txtRangePrecip As System.Windows.Forms.TextBox
    Friend WithEvents txtMaxPrecip As System.Windows.Forms.TextBox
    Friend WithEvents txtPrecipMapZoneInt As System.Windows.Forms.TextBox
    Friend WithEvents txtPrecipMapZoneNo As System.Windows.Forms.TextBox
    Friend WithEvents txtMinPrecip As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents lstPrecipZones As System.Windows.Forms.ListBox
    Friend WithEvents Label26 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents cmdApplyPRISMInterval As System.Windows.Forms.Button
    Friend WithEvents lblSelNote As System.Windows.Forms.Label
    Friend WithEvents lblToElev As System.Windows.Forms.Label
    Friend WithEvents lblFromElev As System.Windows.Forms.Label
    Friend WithEvents txtToElev As System.Windows.Forms.TextBox
    Friend WithEvents txtFromElev As System.Windows.Forms.TextBox
    Friend WithEvents OptSelTo As System.Windows.Forms.RadioButton
    Friend WithEvents OptSelFrom As System.Windows.Forms.RadioButton
    Friend WithEvents lstElevRange As System.Windows.Forms.ListBox
    Friend WithEvents lblSelectType As System.Windows.Forms.Label
    Friend WithEvents CmdGenerate As System.Windows.Forms.Button
    Friend WithEvents CmdTables As System.Windows.Forms.Button
    Friend WithEvents CmbClose As System.Windows.Forms.Button
    Friend WithEvents CmdMaps As System.Windows.Forms.Button
    Friend WithEvents lstintervals As System.Windows.Forms.ListView
    Friend WithEvents Intervals As System.Windows.Forms.ColumnHeader
    Friend WithEvents Pct_Area As System.Windows.Forms.ColumnHeader
    Friend WithEvents Snotel As System.Windows.Forms.ColumnHeader
    Friend WithEvents SnowCourse As System.Windows.Forms.ColumnHeader
    Friend WithEvents lstDataStatus As System.Windows.Forms.ListView
    Friend WithEvents DataStatus As System.Windows.Forms.ColumnHeader
    Friend WithEvents DataDescription As System.Windows.Forms.ColumnHeader
    Friend WithEvents RasterName As System.Windows.Forms.ColumnHeader
    Friend WithEvents CmboxAspect As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
End Class
