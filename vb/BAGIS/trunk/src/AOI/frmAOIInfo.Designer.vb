<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAOIInfo
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
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txtDEMFolder = New System.Windows.Forms.TextBox()
        Me.txtPRISMFolder = New System.Windows.Forms.TextBox()
        Me.txtLayersFolder = New System.Windows.Forms.TextBox()
        Me.txtMinElev = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtMaxElev = New System.Windows.Forms.TextBox()
        Me.txtRangeElev = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtArea = New System.Windows.Forms.TextBox()
        Me.txtAreaAcre = New System.Windows.Forms.TextBox()
        Me.txtAreaSQMile = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.lblRefUnit = New System.Windows.Forms.Label()
        Me.txtRefArea = New System.Windows.Forms.TextBox()
        Me.CmdSetAOI = New System.Windows.Forms.Button()
        Me.FrameUserLayers = New System.Windows.Forms.GroupBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.LstVectors = New System.Windows.Forms.ListBox()
        Me.LstRasters = New System.Windows.Forms.ListBox()
        Me.CmbAddSelectionsToMap = New System.Windows.Forms.Button()
        Me.CmdClearSelected = New System.Windows.Forms.Button()
        Me.FrameBAGISLayers = New System.Windows.Forms.GroupBox()
        Me.CmdReClip = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ChkSnowCourseExist = New System.Windows.Forms.CheckBox()
        Me.ChkSnowCourseSelected = New System.Windows.Forms.CheckBox()
        Me.ChkSNOTELSelected = New System.Windows.Forms.CheckBox()
        Me.ChkPRISMSelected = New System.Windows.Forms.CheckBox()
        Me.ChkSNOTELExist = New System.Windows.Forms.CheckBox()
        Me.ChkPRISMExist = New System.Windows.Forms.CheckBox()
        Me.CmdAddLayer = New System.Windows.Forms.Button()
        Me.CmdUpdateWeasel = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.FrameUserLayers.SuspendLayout()
        Me.FrameBAGISLayers.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(37, 30)
        Me.Label17.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(71, 16)
        Me.Label17.TabIndex = 0
        Me.Label17.Text = "DEM Path:"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(25, 60)
        Me.Label18.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(83, 16)
        Me.Label18.TabIndex = 0
        Me.Label18.Text = "PRISM Path:"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(26, 91)
        Me.Label19.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(82, 16)
        Me.Label19.TabIndex = 0
        Me.Label19.Text = "Layers Path:"
        '
        'txtDEMFolder
        '
        Me.txtDEMFolder.Location = New System.Drawing.Point(115, 27)
        Me.txtDEMFolder.Name = "txtDEMFolder"
        Me.txtDEMFolder.ReadOnly = True
        Me.txtDEMFolder.Size = New System.Drawing.Size(589, 22)
        Me.txtDEMFolder.TabIndex = 1
        '
        'txtPRISMFolder
        '
        Me.txtPRISMFolder.Location = New System.Drawing.Point(115, 58)
        Me.txtPRISMFolder.Name = "txtPRISMFolder"
        Me.txtPRISMFolder.ReadOnly = True
        Me.txtPRISMFolder.Size = New System.Drawing.Size(589, 22)
        Me.txtPRISMFolder.TabIndex = 1
        '
        'txtLayersFolder
        '
        Me.txtLayersFolder.Location = New System.Drawing.Point(115, 88)
        Me.txtLayersFolder.Name = "txtLayersFolder"
        Me.txtLayersFolder.ReadOnly = True
        Me.txtLayersFolder.Size = New System.Drawing.Size(589, 22)
        Me.txtLayersFolder.TabIndex = 1
        '
        'txtMinElev
        '
        Me.txtMinElev.BackColor = System.Drawing.SystemColors.Menu
        Me.txtMinElev.ForeColor = System.Drawing.Color.Blue
        Me.txtMinElev.Location = New System.Drawing.Point(61, 157)
        Me.txtMinElev.Name = "txtMinElev"
        Me.txtMinElev.ReadOnly = True
        Me.txtMinElev.Size = New System.Drawing.Size(149, 22)
        Me.txtMinElev.TabIndex = 2
        Me.txtMinElev.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(26, 159)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(32, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Min:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(9, 214)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(52, 16)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Range:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(23, 187)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(36, 16)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Max:"
        '
        'txtMaxElev
        '
        Me.txtMaxElev.BackColor = System.Drawing.SystemColors.Menu
        Me.txtMaxElev.ForeColor = System.Drawing.Color.Blue
        Me.txtMaxElev.Location = New System.Drawing.Point(61, 183)
        Me.txtMaxElev.Name = "txtMaxElev"
        Me.txtMaxElev.ReadOnly = True
        Me.txtMaxElev.Size = New System.Drawing.Size(149, 22)
        Me.txtMaxElev.TabIndex = 2
        Me.txtMaxElev.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtRangeElev
        '
        Me.txtRangeElev.BackColor = System.Drawing.SystemColors.Menu
        Me.txtRangeElev.ForeColor = System.Drawing.Color.Blue
        Me.txtRangeElev.Location = New System.Drawing.Point(61, 211)
        Me.txtRangeElev.Name = "txtRangeElev"
        Me.txtRangeElev.ReadOnly = True
        Me.txtRangeElev.Size = New System.Drawing.Size(149, 22)
        Me.txtRangeElev.TabIndex = 2
        Me.txtRangeElev.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(58, 138)
        Me.Label14.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(100, 16)
        Me.Label14.TabIndex = 0
        Me.Label14.Text = "Elevation Stats:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(217, 159)
        Me.Label9.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(49, 16)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Meters"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(217, 187)
        Me.Label10.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(49, 16)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "Meters"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(217, 214)
        Me.Label11.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(49, 16)
        Me.Label11.TabIndex = 0
        Me.Label11.Text = "Meters"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(296, 138)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(83, 16)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "Shape Area:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(455, 159)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(74, 16)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Square Km"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(455, 187)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(36, 16)
        Me.Label5.TabIndex = 0
        Me.Label5.Text = "Acre"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(455, 214)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(80, 16)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Square Mile"
        '
        'txtArea
        '
        Me.txtArea.BackColor = System.Drawing.SystemColors.Menu
        Me.txtArea.ForeColor = System.Drawing.Color.Blue
        Me.txtArea.Location = New System.Drawing.Point(299, 157)
        Me.txtArea.Name = "txtArea"
        Me.txtArea.ReadOnly = True
        Me.txtArea.Size = New System.Drawing.Size(149, 22)
        Me.txtArea.TabIndex = 2
        Me.txtArea.Tag = ""
        Me.txtArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAreaAcre
        '
        Me.txtAreaAcre.BackColor = System.Drawing.SystemColors.Menu
        Me.txtAreaAcre.ForeColor = System.Drawing.Color.Blue
        Me.txtAreaAcre.Location = New System.Drawing.Point(299, 183)
        Me.txtAreaAcre.Name = "txtAreaAcre"
        Me.txtAreaAcre.ReadOnly = True
        Me.txtAreaAcre.Size = New System.Drawing.Size(149, 22)
        Me.txtAreaAcre.TabIndex = 2
        Me.txtAreaAcre.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtAreaSQMile
        '
        Me.txtAreaSQMile.BackColor = System.Drawing.SystemColors.Menu
        Me.txtAreaSQMile.ForeColor = System.Drawing.Color.Blue
        Me.txtAreaSQMile.Location = New System.Drawing.Point(299, 211)
        Me.txtAreaSQMile.Name = "txtAreaSQMile"
        Me.txtAreaSQMile.ReadOnly = True
        Me.txtAreaSQMile.Size = New System.Drawing.Size(149, 22)
        Me.txtAreaSQMile.TabIndex = 2
        Me.txtAreaSQMile.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(557, 138)
        Me.Label12.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(106, 16)
        Me.Label12.TabIndex = 0
        Me.Label12.Text = "Reference Area:"
        '
        'lblRefUnit
        '
        Me.lblRefUnit.AutoSize = True
        Me.lblRefUnit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRefUnit.Location = New System.Drawing.Point(699, 159)
        Me.lblRefUnit.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblRefUnit.Name = "lblRefUnit"
        Me.lblRefUnit.Size = New System.Drawing.Size(12, 16)
        Me.lblRefUnit.TabIndex = 0
        Me.lblRefUnit.Text = "-"
        '
        'txtRefArea
        '
        Me.txtRefArea.BackColor = System.Drawing.SystemColors.Menu
        Me.txtRefArea.ForeColor = System.Drawing.Color.Blue
        Me.txtRefArea.Location = New System.Drawing.Point(560, 157)
        Me.txtRefArea.Name = "txtRefArea"
        Me.txtRefArea.ReadOnly = True
        Me.txtRefArea.Size = New System.Drawing.Size(134, 22)
        Me.txtRefArea.TabIndex = 2
        Me.txtRefArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'CmdSetAOI
        '
        Me.CmdSetAOI.Location = New System.Drawing.Point(598, 203)
        Me.CmdSetAOI.Name = "CmdSetAOI"
        Me.CmdSetAOI.Size = New System.Drawing.Size(127, 30)
        Me.CmdSetAOI.TabIndex = 3
        Me.CmdSetAOI.Text = "Set AOI"
        Me.CmdSetAOI.UseVisualStyleBackColor = True
        '
        'FrameUserLayers
        '
        Me.FrameUserLayers.Controls.Add(Me.Label16)
        Me.FrameUserLayers.Controls.Add(Me.Label15)
        Me.FrameUserLayers.Controls.Add(Me.LstVectors)
        Me.FrameUserLayers.Controls.Add(Me.LstRasters)
        Me.FrameUserLayers.Controls.Add(Me.CmbAddSelectionsToMap)
        Me.FrameUserLayers.Controls.Add(Me.CmdClearSelected)
        Me.FrameUserLayers.Location = New System.Drawing.Point(26, 250)
        Me.FrameUserLayers.Name = "FrameUserLayers"
        Me.FrameUserLayers.Size = New System.Drawing.Size(438, 259)
        Me.FrameUserLayers.TabIndex = 4
        Me.FrameUserLayers.TabStop = False
        Me.FrameUserLayers.Text = "Presence of User's Layers in AOI"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(222, 68)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(91, 16)
        Me.Label16.TabIndex = 2
        Me.Label16.Text = "Vector Layers"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(11, 68)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(92, 16)
        Me.Label15.TabIndex = 2
        Me.Label15.Text = "Raster Layers"
        '
        'LstVectors
        '
        Me.LstVectors.FormattingEnabled = True
        Me.LstVectors.ItemHeight = 16
        Me.LstVectors.Location = New System.Drawing.Point(225, 89)
        Me.LstVectors.Name = "LstVectors"
        Me.LstVectors.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.LstVectors.Size = New System.Drawing.Size(197, 164)
        Me.LstVectors.TabIndex = 1
        '
        'LstRasters
        '
        Me.LstRasters.FormattingEnabled = True
        Me.LstRasters.ItemHeight = 16
        Me.LstRasters.Location = New System.Drawing.Point(14, 89)
        Me.LstRasters.Name = "LstRasters"
        Me.LstRasters.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended
        Me.LstRasters.Size = New System.Drawing.Size(197, 164)
        Me.LstRasters.TabIndex = 1
        '
        'CmbAddSelectionsToMap
        '
        Me.CmbAddSelectionsToMap.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmbAddSelectionsToMap.Location = New System.Drawing.Point(236, 27)
        Me.CmbAddSelectionsToMap.Name = "CmbAddSelectionsToMap"
        Me.CmbAddSelectionsToMap.Size = New System.Drawing.Size(160, 29)
        Me.CmbAddSelectionsToMap.TabIndex = 0
        Me.CmbAddSelectionsToMap.Text = "Add Selections To Map"
        Me.CmbAddSelectionsToMap.UseVisualStyleBackColor = True
        '
        'CmdClearSelected
        '
        Me.CmdClearSelected.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdClearSelected.Location = New System.Drawing.Point(52, 27)
        Me.CmdClearSelected.Name = "CmdClearSelected"
        Me.CmdClearSelected.Size = New System.Drawing.Size(132, 29)
        Me.CmdClearSelected.TabIndex = 0
        Me.CmdClearSelected.Text = "Clear Selection"
        Me.CmdClearSelected.UseVisualStyleBackColor = True
        '
        'FrameBAGISLayers
        '
        Me.FrameBAGISLayers.Controls.Add(Me.CmdReClip)
        Me.FrameBAGISLayers.Controls.Add(Me.Label3)
        Me.FrameBAGISLayers.Controls.Add(Me.ChkSnowCourseExist)
        Me.FrameBAGISLayers.Controls.Add(Me.ChkSnowCourseSelected)
        Me.FrameBAGISLayers.Controls.Add(Me.ChkSNOTELSelected)
        Me.FrameBAGISLayers.Controls.Add(Me.ChkPRISMSelected)
        Me.FrameBAGISLayers.Controls.Add(Me.ChkSNOTELExist)
        Me.FrameBAGISLayers.Controls.Add(Me.ChkPRISMExist)
        Me.FrameBAGISLayers.Location = New System.Drawing.Point(502, 250)
        Me.FrameBAGISLayers.Name = "FrameBAGISLayers"
        Me.FrameBAGISLayers.Size = New System.Drawing.Size(241, 167)
        Me.FrameBAGISLayers.TabIndex = 5
        Me.FrameBAGISLayers.TabStop = False
        Me.FrameBAGISLayers.Text = "Presence of BAGIS Layers"
        '
        'CmdReClip
        '
        Me.CmdReClip.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdReClip.Location = New System.Drawing.Point(45, 132)
        Me.CmdReClip.Name = "CmdReClip"
        Me.CmdReClip.Size = New System.Drawing.Size(147, 27)
        Me.CmdReClip.TabIndex = 3
        Me.CmdReClip.Text = "Re-clip Selected Layers"
        Me.CmdReClip.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(161, 27)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(62, 16)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Selected"
        '
        'ChkSnowCourseExist
        '
        Me.ChkSnowCourseExist.Enabled = False
        Me.ChkSnowCourseExist.Location = New System.Drawing.Point(15, 99)
        Me.ChkSnowCourseExist.Name = "ChkSnowCourseExist"
        Me.ChkSnowCourseExist.Size = New System.Drawing.Size(106, 20)
        Me.ChkSnowCourseExist.TabIndex = 0
        Me.ChkSnowCourseExist.Text = "Snow Course"
        Me.ChkSnowCourseExist.UseVisualStyleBackColor = True
        '
        'ChkSnowCourseSelected
        '
        Me.ChkSnowCourseSelected.AutoSize = True
        Me.ChkSnowCourseSelected.Location = New System.Drawing.Point(177, 102)
        Me.ChkSnowCourseSelected.Name = "ChkSnowCourseSelected"
        Me.ChkSnowCourseSelected.Size = New System.Drawing.Size(15, 14)
        Me.ChkSnowCourseSelected.TabIndex = 0
        Me.ChkSnowCourseSelected.UseVisualStyleBackColor = True
        '
        'ChkSNOTELSelected
        '
        Me.ChkSNOTELSelected.AutoSize = True
        Me.ChkSNOTELSelected.Location = New System.Drawing.Point(177, 76)
        Me.ChkSNOTELSelected.Name = "ChkSNOTELSelected"
        Me.ChkSNOTELSelected.Size = New System.Drawing.Size(15, 14)
        Me.ChkSNOTELSelected.TabIndex = 0
        Me.ChkSNOTELSelected.UseVisualStyleBackColor = True
        '
        'ChkPRISMSelected
        '
        Me.ChkPRISMSelected.AutoSize = True
        Me.ChkPRISMSelected.Location = New System.Drawing.Point(177, 50)
        Me.ChkPRISMSelected.Name = "ChkPRISMSelected"
        Me.ChkPRISMSelected.Size = New System.Drawing.Size(15, 14)
        Me.ChkPRISMSelected.TabIndex = 0
        Me.ChkPRISMSelected.UseVisualStyleBackColor = True
        '
        'ChkSNOTELExist
        '
        Me.ChkSNOTELExist.Enabled = False
        Me.ChkSNOTELExist.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.7!)
        Me.ChkSNOTELExist.Location = New System.Drawing.Point(15, 73)
        Me.ChkSNOTELExist.Name = "ChkSNOTELExist"
        Me.ChkSNOTELExist.Size = New System.Drawing.Size(81, 20)
        Me.ChkSNOTELExist.TabIndex = 0
        Me.ChkSNOTELExist.Text = "SNOTEL"
        Me.ChkSNOTELExist.UseVisualStyleBackColor = True
        '
        'ChkPRISMExist
        '
        Me.ChkPRISMExist.Enabled = False
        Me.ChkPRISMExist.Location = New System.Drawing.Point(15, 47)
        Me.ChkPRISMExist.Name = "ChkPRISMExist"
        Me.ChkPRISMExist.Size = New System.Drawing.Size(113, 20)
        Me.ChkPRISMExist.TabIndex = 0
        Me.ChkPRISMExist.Text = "PRISM Layers"
        Me.ChkPRISMExist.UseVisualStyleBackColor = True
        '
        'CmdAddLayer
        '
        Me.CmdAddLayer.Location = New System.Drawing.Point(547, 423)
        Me.CmdAddLayer.Name = "CmdAddLayer"
        Me.CmdAddLayer.Size = New System.Drawing.Size(147, 27)
        Me.CmdAddLayer.TabIndex = 3
        Me.CmdAddLayer.Text = "Add A New Layer"
        Me.CmdAddLayer.UseVisualStyleBackColor = True
        '
        'CmdUpdateWeasel
        '
        Me.CmdUpdateWeasel.Location = New System.Drawing.Point(547, 456)
        Me.CmdUpdateWeasel.Name = "CmdUpdateWeasel"
        Me.CmdUpdateWeasel.Size = New System.Drawing.Size(147, 27)
        Me.CmdUpdateWeasel.TabIndex = 3
        Me.CmdUpdateWeasel.Text = "Update Weasel Info"
        Me.CmdUpdateWeasel.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Location = New System.Drawing.Point(547, 489)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(147, 27)
        Me.cmdOK.TabIndex = 3
        Me.cmdOK.Text = "Close"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'frmAOIInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(792, 525)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.CmdUpdateWeasel)
        Me.Controls.Add(Me.CmdAddLayer)
        Me.Controls.Add(Me.FrameBAGISLayers)
        Me.Controls.Add(Me.FrameUserLayers)
        Me.Controls.Add(Me.CmdSetAOI)
        Me.Controls.Add(Me.txtAreaSQMile)
        Me.Controls.Add(Me.txtAreaAcre)
        Me.Controls.Add(Me.txtRangeElev)
        Me.Controls.Add(Me.txtRefArea)
        Me.Controls.Add(Me.txtArea)
        Me.Controls.Add(Me.txtMaxElev)
        Me.Controls.Add(Me.txtMinElev)
        Me.Controls.Add(Me.txtLayersFolder)
        Me.Controls.Add(Me.txtPRISMFolder)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.txtDEMFolder)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.lblRefUnit)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label19)
        Me.Controls.Add(Me.Label18)
        Me.Controls.Add(Me.Label17)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "frmAOIInfo"
        Me.ShowIcon = False
        Me.Tag = ""
        Me.FrameUserLayers.ResumeLayout(False)
        Me.FrameUserLayers.PerformLayout()
        Me.FrameBAGISLayers.ResumeLayout(False)
        Me.FrameBAGISLayers.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents txtDEMFolder As System.Windows.Forms.TextBox
    Friend WithEvents txtPRISMFolder As System.Windows.Forms.TextBox
    Friend WithEvents txtLayersFolder As System.Windows.Forms.TextBox
    Friend WithEvents txtMinElev As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents txtMaxElev As System.Windows.Forms.TextBox
    Friend WithEvents txtRangeElev As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtArea As System.Windows.Forms.TextBox
    Friend WithEvents txtAreaAcre As System.Windows.Forms.TextBox
    Friend WithEvents txtAreaSQMile As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents lblRefUnit As System.Windows.Forms.Label
    Friend WithEvents txtRefArea As System.Windows.Forms.TextBox
    Friend WithEvents CmdSetAOI As System.Windows.Forms.Button
    Friend WithEvents FrameUserLayers As System.Windows.Forms.GroupBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents LstVectors As System.Windows.Forms.ListBox
    Friend WithEvents LstRasters As System.Windows.Forms.ListBox
    Friend WithEvents CmbAddSelectionsToMap As System.Windows.Forms.Button
    Friend WithEvents CmdClearSelected As System.Windows.Forms.Button
    Friend WithEvents FrameBAGISLayers As System.Windows.Forms.GroupBox
    Friend WithEvents ChkSnowCourseExist As System.Windows.Forms.CheckBox
    Friend WithEvents ChkSNOTELExist As System.Windows.Forms.CheckBox
    Friend WithEvents ChkPRISMExist As System.Windows.Forms.CheckBox
    Friend WithEvents CmdReClip As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ChkSnowCourseSelected As System.Windows.Forms.CheckBox
    Friend WithEvents ChkSNOTELSelected As System.Windows.Forms.CheckBox
    Friend WithEvents ChkPRISMSelected As System.Windows.Forms.CheckBox
    Friend WithEvents CmdAddLayer As System.Windows.Forms.Button
    Friend WithEvents CmdUpdateWeasel As System.Windows.Forms.Button
    Friend WithEvents cmdOK As System.Windows.Forms.Button
End Class
