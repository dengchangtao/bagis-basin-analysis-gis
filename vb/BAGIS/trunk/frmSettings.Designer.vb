<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSettings
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSettings))
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtTerrain = New System.Windows.Forms.TextBox()
        Me.txtDrainage = New System.Windows.Forms.TextBox()
        Me.txtWatershed = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtDEM10 = New System.Windows.Forms.TextBox()
        Me.txtDEM30 = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtGaugeStation = New System.Windows.Forms.TextBox()
        Me.lblSNOTEL = New System.Windows.Forms.Label()
        Me.txtSNOTEL = New System.Windows.Forms.TextBox()
        Me.lblSnowCourse = New System.Windows.Forms.Label()
        Me.txtSnowCourse = New System.Windows.Forms.TextBox()
        Me.lblPRISM = New System.Windows.Forms.Label()
        Me.txtPRISM = New System.Windows.Forms.TextBox()
        Me.lstLayers = New System.Windows.Forms.ListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CmdSetTerrainRef = New System.Windows.Forms.Button()
        Me.CmdSetDrainageRef = New System.Windows.Forms.Button()
        Me.CmdSetWatershedRef = New System.Windows.Forms.Button()
        Me.CmdSet10MDEM = New System.Windows.Forms.Button()
        Me.CmdSet30MDEM = New System.Windows.Forms.Button()
        Me.CmdSetGadgeLayer = New System.Windows.Forms.Button()
        Me.CmdSetSNOTEL = New System.Windows.Forms.Button()
        Me.CmdSetPrecip = New System.Windows.Forms.Button()
        Me.CmdSetSnowC = New System.Windows.Forms.Button()
        Me.lblDoubleClick = New System.Windows.Forms.Label()
        Me.CmdDisplayReferenceLayers = New System.Windows.Forms.Button()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Opt10M = New System.Windows.Forms.RadioButton()
        Me.Opt30M = New System.Windows.Forms.RadioButton()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label23 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.CmboxStationAtt = New System.Windows.Forms.ComboBox()
        Me.ComboStationArea = New System.Windows.Forms.ComboBox()
        Me.ComboStation_Value = New System.Windows.Forms.ComboBox()
        Me.lblElevField = New System.Windows.Forms.Label()
        Me.lblNameField = New System.Windows.Forms.Label()
        Me.ComboSNOTEL_Elevation = New System.Windows.Forms.ComboBox()
        Me.ComboSNOTEL_Name = New System.Windows.Forms.ComboBox()
        Me.lblSNOTELUnit = New System.Windows.Forms.Label()
        Me.OptSTMeter = New System.Windows.Forms.RadioButton()
        Me.OptSTFoot = New System.Windows.Forms.RadioButton()
        Me.ComboSC_Elevation = New System.Windows.Forms.ComboBox()
        Me.ComboSC_Name = New System.Windows.Forms.ComboBox()
        Me.lblSnowCourseUnit = New System.Windows.Forms.Label()
        Me.OptSCMeter = New System.Windows.Forms.RadioButton()
        Me.OptSCFoot = New System.Windows.Forms.RadioButton()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.CmdAddLayer = New System.Windows.Forms.Button()
        Me.CmdRemoveLayer = New System.Windows.Forms.Button()
        Me.CmdClearAll = New System.Windows.Forms.Button()
        Me.CmdSaveSettings = New System.Windows.Forms.Button()
        Me.CmdUndo = New System.Windows.Forms.Button()
        Me.CmdClose = New System.Windows.Forms.Button()
        Me.FrameDEM = New System.Windows.Forms.GroupBox()
        Me.OptFoot = New System.Windows.Forms.RadioButton()
        Me.OptMeter = New System.Windows.Forms.RadioButton()
        Me.FrameUnit = New System.Windows.Forms.GroupBox()
        Me.GrpBoxSnowCourseUnit = New System.Windows.Forms.GroupBox()
        Me.GrpBoxSNOTELUnit = New System.Windows.Forms.GroupBox()
        Me.ChkboxAOIOnly = New System.Windows.Forms.CheckBox()
        Me.FrameDEM.SuspendLayout()
        Me.FrameUnit.SuspendLayout()
        Me.GrpBoxSnowCourseUnit.SuspendLayout()
        Me.GrpBoxSNOTELUnit.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label9
        '
        resources.ApplyResources(Me.Label9, "Label9")
        Me.Label9.Name = "Label9"
        '
        'Label10
        '
        resources.ApplyResources(Me.Label10, "Label10")
        Me.Label10.Name = "Label10"
        '
        'Label11
        '
        resources.ApplyResources(Me.Label11, "Label11")
        Me.Label11.Name = "Label11"
        '
        'txtTerrain
        '
        Me.txtTerrain.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.txtTerrain.ForeColor = System.Drawing.Color.Navy
        resources.ApplyResources(Me.txtTerrain, "txtTerrain")
        Me.txtTerrain.Name = "txtTerrain"
        Me.txtTerrain.ReadOnly = True
        '
        'txtDrainage
        '
        Me.txtDrainage.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.txtDrainage.ForeColor = System.Drawing.Color.Navy
        resources.ApplyResources(Me.txtDrainage, "txtDrainage")
        Me.txtDrainage.Name = "txtDrainage"
        Me.txtDrainage.ReadOnly = True
        '
        'txtWatershed
        '
        Me.txtWatershed.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.txtWatershed.ForeColor = System.Drawing.Color.Navy
        resources.ApplyResources(Me.txtWatershed, "txtWatershed")
        Me.txtWatershed.Name = "txtWatershed"
        Me.txtWatershed.ReadOnly = True
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'txtDEM10
        '
        Me.txtDEM10.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDEM10.ForeColor = System.Drawing.Color.Navy
        resources.ApplyResources(Me.txtDEM10, "txtDEM10")
        Me.txtDEM10.Name = "txtDEM10"
        Me.txtDEM10.ReadOnly = True
        '
        'txtDEM30
        '
        Me.txtDEM30.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtDEM30.ForeColor = System.Drawing.Color.Navy
        resources.ApplyResources(Me.txtDEM30, "txtDEM30")
        Me.txtDEM30.Name = "txtDEM30"
        Me.txtDEM30.ReadOnly = True
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'txtGaugeStation
        '
        Me.txtGaugeStation.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtGaugeStation.ForeColor = System.Drawing.Color.Navy
        resources.ApplyResources(Me.txtGaugeStation, "txtGaugeStation")
        Me.txtGaugeStation.Name = "txtGaugeStation"
        Me.txtGaugeStation.ReadOnly = True
        '
        'lblSNOTEL
        '
        resources.ApplyResources(Me.lblSNOTEL, "lblSNOTEL")
        Me.lblSNOTEL.Name = "lblSNOTEL"
        '
        'txtSNOTEL
        '
        Me.txtSNOTEL.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtSNOTEL.ForeColor = System.Drawing.Color.Navy
        resources.ApplyResources(Me.txtSNOTEL, "txtSNOTEL")
        Me.txtSNOTEL.Name = "txtSNOTEL"
        Me.txtSNOTEL.ReadOnly = True
        '
        'lblSnowCourse
        '
        resources.ApplyResources(Me.lblSnowCourse, "lblSnowCourse")
        Me.lblSnowCourse.Name = "lblSnowCourse"
        '
        'txtSnowCourse
        '
        Me.txtSnowCourse.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtSnowCourse.ForeColor = System.Drawing.Color.Navy
        resources.ApplyResources(Me.txtSnowCourse, "txtSnowCourse")
        Me.txtSnowCourse.Name = "txtSnowCourse"
        Me.txtSnowCourse.ReadOnly = True
        '
        'lblPRISM
        '
        resources.ApplyResources(Me.lblPRISM, "lblPRISM")
        Me.lblPRISM.Name = "lblPRISM"
        '
        'txtPRISM
        '
        Me.txtPRISM.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtPRISM.ForeColor = System.Drawing.Color.Navy
        resources.ApplyResources(Me.txtPRISM, "txtPRISM")
        Me.txtPRISM.Name = "txtPRISM"
        Me.txtPRISM.ReadOnly = True
        '
        'lstLayers
        '
        Me.lstLayers.FormattingEnabled = True
        resources.ApplyResources(Me.lstLayers, "lstLayers")
        Me.lstLayers.Name = "lstLayers"
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'CmdSetTerrainRef
        '
        resources.ApplyResources(Me.CmdSetTerrainRef, "CmdSetTerrainRef")
        Me.CmdSetTerrainRef.Name = "CmdSetTerrainRef"
        Me.CmdSetTerrainRef.UseVisualStyleBackColor = True
        '
        'CmdSetDrainageRef
        '
        resources.ApplyResources(Me.CmdSetDrainageRef, "CmdSetDrainageRef")
        Me.CmdSetDrainageRef.Name = "CmdSetDrainageRef"
        Me.CmdSetDrainageRef.UseVisualStyleBackColor = True
        '
        'CmdSetWatershedRef
        '
        resources.ApplyResources(Me.CmdSetWatershedRef, "CmdSetWatershedRef")
        Me.CmdSetWatershedRef.Name = "CmdSetWatershedRef"
        Me.CmdSetWatershedRef.UseVisualStyleBackColor = True
        '
        'CmdSet10MDEM
        '
        resources.ApplyResources(Me.CmdSet10MDEM, "CmdSet10MDEM")
        Me.CmdSet10MDEM.Name = "CmdSet10MDEM"
        Me.CmdSet10MDEM.UseVisualStyleBackColor = True
        '
        'CmdSet30MDEM
        '
        resources.ApplyResources(Me.CmdSet30MDEM, "CmdSet30MDEM")
        Me.CmdSet30MDEM.Name = "CmdSet30MDEM"
        Me.CmdSet30MDEM.UseVisualStyleBackColor = True
        '
        'CmdSetGadgeLayer
        '
        resources.ApplyResources(Me.CmdSetGadgeLayer, "CmdSetGadgeLayer")
        Me.CmdSetGadgeLayer.Name = "CmdSetGadgeLayer"
        Me.CmdSetGadgeLayer.UseVisualStyleBackColor = True
        '
        'CmdSetSNOTEL
        '
        resources.ApplyResources(Me.CmdSetSNOTEL, "CmdSetSNOTEL")
        Me.CmdSetSNOTEL.Name = "CmdSetSNOTEL"
        Me.CmdSetSNOTEL.UseVisualStyleBackColor = True
        '
        'CmdSetPrecip
        '
        resources.ApplyResources(Me.CmdSetPrecip, "CmdSetPrecip")
        Me.CmdSetPrecip.Name = "CmdSetPrecip"
        Me.CmdSetPrecip.UseVisualStyleBackColor = True
        '
        'CmdSetSnowC
        '
        resources.ApplyResources(Me.CmdSetSnowC, "CmdSetSnowC")
        Me.CmdSetSnowC.Name = "CmdSetSnowC"
        Me.CmdSetSnowC.UseVisualStyleBackColor = True
        '
        'lblDoubleClick
        '
        resources.ApplyResources(Me.lblDoubleClick, "lblDoubleClick")
        Me.lblDoubleClick.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.lblDoubleClick.Name = "lblDoubleClick"
        Me.lblDoubleClick.UseWaitCursor = True
        '
        'CmdDisplayReferenceLayers
        '
        resources.ApplyResources(Me.CmdDisplayReferenceLayers, "CmdDisplayReferenceLayers")
        Me.CmdDisplayReferenceLayers.Name = "CmdDisplayReferenceLayers"
        Me.CmdDisplayReferenceLayers.UseVisualStyleBackColor = True
        '
        'Label12
        '
        resources.ApplyResources(Me.Label12, "Label12")
        Me.Label12.Name = "Label12"
        '
        'Label13
        '
        resources.ApplyResources(Me.Label13, "Label13")
        Me.Label13.Name = "Label13"
        '
        'Opt10M
        '
        resources.ApplyResources(Me.Opt10M, "Opt10M")
        Me.Opt10M.Checked = True
        Me.Opt10M.Name = "Opt10M"
        Me.Opt10M.TabStop = True
        Me.Opt10M.UseVisualStyleBackColor = True
        '
        'Opt30M
        '
        resources.ApplyResources(Me.Opt30M, "Opt30M")
        Me.Opt30M.Name = "Opt30M"
        Me.Opt30M.UseVisualStyleBackColor = True
        '
        'Label8
        '
        resources.ApplyResources(Me.Label8, "Label8")
        Me.Label8.Name = "Label8"
        '
        'Label23
        '
        resources.ApplyResources(Me.Label23, "Label23")
        Me.Label23.Name = "Label23"
        '
        'Label22
        '
        resources.ApplyResources(Me.Label22, "Label22")
        Me.Label22.Name = "Label22"
        '
        'CmboxStationAtt
        '
        Me.CmboxStationAtt.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.CmboxStationAtt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CmboxStationAtt.FormattingEnabled = True
        resources.ApplyResources(Me.CmboxStationAtt, "CmboxStationAtt")
        Me.CmboxStationAtt.Name = "CmboxStationAtt"
        '
        'ComboStationArea
        '
        Me.ComboStationArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboStationArea.FormattingEnabled = True
        resources.ApplyResources(Me.ComboStationArea, "ComboStationArea")
        Me.ComboStationArea.Name = "ComboStationArea"
        '
        'ComboStation_Value
        '
        Me.ComboStation_Value.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboStation_Value.FormattingEnabled = True
        resources.ApplyResources(Me.ComboStation_Value, "ComboStation_Value")
        Me.ComboStation_Value.Name = "ComboStation_Value"
        '
        'lblElevField
        '
        resources.ApplyResources(Me.lblElevField, "lblElevField")
        Me.lblElevField.Name = "lblElevField"
        '
        'lblNameField
        '
        resources.ApplyResources(Me.lblNameField, "lblNameField")
        Me.lblNameField.Name = "lblNameField"
        '
        'ComboSNOTEL_Elevation
        '
        Me.ComboSNOTEL_Elevation.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ComboSNOTEL_Elevation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboSNOTEL_Elevation.FormattingEnabled = True
        resources.ApplyResources(Me.ComboSNOTEL_Elevation, "ComboSNOTEL_Elevation")
        Me.ComboSNOTEL_Elevation.Name = "ComboSNOTEL_Elevation"
        '
        'ComboSNOTEL_Name
        '
        Me.ComboSNOTEL_Name.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboSNOTEL_Name.FormattingEnabled = True
        resources.ApplyResources(Me.ComboSNOTEL_Name, "ComboSNOTEL_Name")
        Me.ComboSNOTEL_Name.Name = "ComboSNOTEL_Name"
        '
        'lblSNOTELUnit
        '
        resources.ApplyResources(Me.lblSNOTELUnit, "lblSNOTELUnit")
        Me.lblSNOTELUnit.Name = "lblSNOTELUnit"
        '
        'OptSTMeter
        '
        resources.ApplyResources(Me.OptSTMeter, "OptSTMeter")
        Me.OptSTMeter.Checked = True
        Me.OptSTMeter.Name = "OptSTMeter"
        Me.OptSTMeter.TabStop = True
        Me.OptSTMeter.UseVisualStyleBackColor = True
        '
        'OptSTFoot
        '
        resources.ApplyResources(Me.OptSTFoot, "OptSTFoot")
        Me.OptSTFoot.Name = "OptSTFoot"
        Me.OptSTFoot.TabStop = True
        Me.OptSTFoot.UseVisualStyleBackColor = True
        '
        'ComboSC_Elevation
        '
        Me.ComboSC_Elevation.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ComboSC_Elevation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboSC_Elevation.FormattingEnabled = True
        resources.ApplyResources(Me.ComboSC_Elevation, "ComboSC_Elevation")
        Me.ComboSC_Elevation.Name = "ComboSC_Elevation"
        '
        'ComboSC_Name
        '
        Me.ComboSC_Name.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboSC_Name.FormattingEnabled = True
        resources.ApplyResources(Me.ComboSC_Name, "ComboSC_Name")
        Me.ComboSC_Name.Name = "ComboSC_Name"
        '
        'lblSnowCourseUnit
        '
        resources.ApplyResources(Me.lblSnowCourseUnit, "lblSnowCourseUnit")
        Me.lblSnowCourseUnit.Name = "lblSnowCourseUnit"
        '
        'OptSCMeter
        '
        resources.ApplyResources(Me.OptSCMeter, "OptSCMeter")
        Me.OptSCMeter.Checked = True
        Me.OptSCMeter.Name = "OptSCMeter"
        Me.OptSCMeter.TabStop = True
        Me.OptSCMeter.UseVisualStyleBackColor = True
        '
        'OptSCFoot
        '
        resources.ApplyResources(Me.OptSCFoot, "OptSCFoot")
        Me.OptSCFoot.Name = "OptSCFoot"
        Me.OptSCFoot.TabStop = True
        Me.OptSCFoot.UseVisualStyleBackColor = True
        '
        'Label15
        '
        resources.ApplyResources(Me.Label15, "Label15")
        Me.Label15.ForeColor = System.Drawing.Color.Blue
        Me.Label15.Name = "Label15"
        '
        'CmdAddLayer
        '
        resources.ApplyResources(Me.CmdAddLayer, "CmdAddLayer")
        Me.CmdAddLayer.Name = "CmdAddLayer"
        Me.CmdAddLayer.UseVisualStyleBackColor = True
        '
        'CmdRemoveLayer
        '
        resources.ApplyResources(Me.CmdRemoveLayer, "CmdRemoveLayer")
        Me.CmdRemoveLayer.Name = "CmdRemoveLayer"
        Me.CmdRemoveLayer.UseVisualStyleBackColor = True
        '
        'CmdClearAll
        '
        resources.ApplyResources(Me.CmdClearAll, "CmdClearAll")
        Me.CmdClearAll.Name = "CmdClearAll"
        Me.CmdClearAll.UseVisualStyleBackColor = True
        '
        'CmdSaveSettings
        '
        resources.ApplyResources(Me.CmdSaveSettings, "CmdSaveSettings")
        Me.CmdSaveSettings.Name = "CmdSaveSettings"
        Me.CmdSaveSettings.UseVisualStyleBackColor = True
        '
        'CmdUndo
        '
        resources.ApplyResources(Me.CmdUndo, "CmdUndo")
        Me.CmdUndo.Name = "CmdUndo"
        Me.CmdUndo.UseVisualStyleBackColor = True
        '
        'CmdClose
        '
        resources.ApplyResources(Me.CmdClose, "CmdClose")
        Me.CmdClose.Name = "CmdClose"
        Me.CmdClose.UseVisualStyleBackColor = True
        '
        'FrameDEM
        '
        Me.FrameDEM.CausesValidation = False
        Me.FrameDEM.Controls.Add(Me.Opt10M)
        Me.FrameDEM.Controls.Add(Me.Opt30M)
        resources.ApplyResources(Me.FrameDEM, "FrameDEM")
        Me.FrameDEM.Name = "FrameDEM"
        Me.FrameDEM.TabStop = False
        '
        'OptFoot
        '
        resources.ApplyResources(Me.OptFoot, "OptFoot")
        Me.OptFoot.Name = "OptFoot"
        Me.OptFoot.UseVisualStyleBackColor = True
        '
        'OptMeter
        '
        resources.ApplyResources(Me.OptMeter, "OptMeter")
        Me.OptMeter.Checked = True
        Me.OptMeter.Name = "OptMeter"
        Me.OptMeter.TabStop = True
        Me.OptMeter.UseVisualStyleBackColor = True
        '
        'FrameUnit
        '
        Me.FrameUnit.CausesValidation = False
        Me.FrameUnit.Controls.Add(Me.OptMeter)
        Me.FrameUnit.Controls.Add(Me.OptFoot)
        resources.ApplyResources(Me.FrameUnit, "FrameUnit")
        Me.FrameUnit.Name = "FrameUnit"
        Me.FrameUnit.TabStop = False
        '
        'GrpBoxSnowCourseUnit
        '
        Me.GrpBoxSnowCourseUnit.Controls.Add(Me.OptSCMeter)
        Me.GrpBoxSnowCourseUnit.Controls.Add(Me.OptSCFoot)
        resources.ApplyResources(Me.GrpBoxSnowCourseUnit, "GrpBoxSnowCourseUnit")
        Me.GrpBoxSnowCourseUnit.Name = "GrpBoxSnowCourseUnit"
        Me.GrpBoxSnowCourseUnit.TabStop = False
        '
        'GrpBoxSNOTELUnit
        '
        Me.GrpBoxSNOTELUnit.Controls.Add(Me.OptSTMeter)
        Me.GrpBoxSNOTELUnit.Controls.Add(Me.OptSTFoot)
        resources.ApplyResources(Me.GrpBoxSNOTELUnit, "GrpBoxSNOTELUnit")
        Me.GrpBoxSNOTELUnit.Name = "GrpBoxSNOTELUnit"
        Me.GrpBoxSNOTELUnit.TabStop = False
        '
        'ChkboxAOIOnly
        '
        resources.ApplyResources(Me.ChkboxAOIOnly, "ChkboxAOIOnly")
        Me.ChkboxAOIOnly.Name = "ChkboxAOIOnly"
        Me.ChkboxAOIOnly.UseVisualStyleBackColor = True
        '
        'frmSettings
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.ChkboxAOIOnly)
        Me.Controls.Add(Me.lstLayers)
        Me.Controls.Add(Me.GrpBoxSNOTELUnit)
        Me.Controls.Add(Me.GrpBoxSnowCourseUnit)
        Me.Controls.Add(Me.FrameUnit)
        Me.Controls.Add(Me.FrameDEM)
        Me.Controls.Add(Me.ComboStation_Value)
        Me.Controls.Add(Me.ComboSC_Name)
        Me.Controls.Add(Me.ComboSNOTEL_Name)
        Me.Controls.Add(Me.ComboStationArea)
        Me.Controls.Add(Me.ComboSC_Elevation)
        Me.Controls.Add(Me.ComboSNOTEL_Elevation)
        Me.Controls.Add(Me.CmboxStationAtt)
        Me.Controls.Add(Me.CmdDisplayReferenceLayers)
        Me.Controls.Add(Me.CmdSetSnowC)
        Me.Controls.Add(Me.CmdClearAll)
        Me.Controls.Add(Me.CmdRemoveLayer)
        Me.Controls.Add(Me.CmdClose)
        Me.Controls.Add(Me.CmdUndo)
        Me.Controls.Add(Me.CmdSaveSettings)
        Me.Controls.Add(Me.CmdAddLayer)
        Me.Controls.Add(Me.CmdSetPrecip)
        Me.Controls.Add(Me.CmdSetSNOTEL)
        Me.Controls.Add(Me.CmdSetGadgeLayer)
        Me.Controls.Add(Me.CmdSet30MDEM)
        Me.Controls.Add(Me.CmdSet10MDEM)
        Me.Controls.Add(Me.CmdSetWatershedRef)
        Me.Controls.Add(Me.CmdSetDrainageRef)
        Me.Controls.Add(Me.CmdSetTerrainRef)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtPRISM)
        Me.Controls.Add(Me.txtSnowCourse)
        Me.Controls.Add(Me.txtSNOTEL)
        Me.Controls.Add(Me.txtGaugeStation)
        Me.Controls.Add(Me.txtDEM30)
        Me.Controls.Add(Me.txtDEM10)
        Me.Controls.Add(Me.txtWatershed)
        Me.Controls.Add(Me.lblPRISM)
        Me.Controls.Add(Me.lblSnowCourse)
        Me.Controls.Add(Me.lblSNOTEL)
        Me.Controls.Add(Me.txtDrainage)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtTerrain)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lblNameField)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label23)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.lblSnowCourseUnit)
        Me.Controls.Add(Me.lblSNOTELUnit)
        Me.Controls.Add(Me.lblElevField)
        Me.Controls.Add(Me.Label22)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.lblDoubleClick)
        Me.Controls.Add(Me.Label9)
        Me.ForeColor = System.Drawing.Color.Black
        Me.Name = "frmSettings"
        Me.ShowIcon = False
        Me.FrameDEM.ResumeLayout(False)
        Me.FrameDEM.PerformLayout()
        Me.FrameUnit.ResumeLayout(False)
        Me.FrameUnit.PerformLayout()
        Me.GrpBoxSnowCourseUnit.ResumeLayout(False)
        Me.GrpBoxSnowCourseUnit.PerformLayout()
        Me.GrpBoxSNOTELUnit.ResumeLayout(False)
        Me.GrpBoxSNOTELUnit.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents txtTerrain As System.Windows.Forms.TextBox
    Friend WithEvents txtDrainage As System.Windows.Forms.TextBox
    Friend WithEvents txtWatershed As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtDEM10 As System.Windows.Forms.TextBox
    Friend WithEvents txtDEM30 As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtGaugeStation As System.Windows.Forms.TextBox
    Friend WithEvents lblSNOTEL As System.Windows.Forms.Label
    Friend WithEvents txtSNOTEL As System.Windows.Forms.TextBox
    Friend WithEvents lblSnowCourse As System.Windows.Forms.Label
    Friend WithEvents txtSnowCourse As System.Windows.Forms.TextBox
    Friend WithEvents lblPRISM As System.Windows.Forms.Label
    Friend WithEvents txtPRISM As System.Windows.Forms.TextBox
    Friend WithEvents lstLayers As System.Windows.Forms.ListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CmdSetTerrainRef As System.Windows.Forms.Button
    Friend WithEvents CmdSetDrainageRef As System.Windows.Forms.Button
    Friend WithEvents CmdSetWatershedRef As System.Windows.Forms.Button
    Friend WithEvents CmdSet10MDEM As System.Windows.Forms.Button
    Friend WithEvents CmdSet30MDEM As System.Windows.Forms.Button
    Friend WithEvents CmdSetGadgeLayer As System.Windows.Forms.Button
    Friend WithEvents CmdSetSNOTEL As System.Windows.Forms.Button
    Friend WithEvents CmdSetPrecip As System.Windows.Forms.Button
    Friend WithEvents CmdSetSnowC As System.Windows.Forms.Button
    Friend WithEvents lblDoubleClick As System.Windows.Forms.Label
    Friend WithEvents CmdDisplayReferenceLayers As System.Windows.Forms.Button
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Opt10M As System.Windows.Forms.RadioButton
    Friend WithEvents Opt30M As System.Windows.Forms.RadioButton
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents CmboxStationAtt As System.Windows.Forms.ComboBox
    Friend WithEvents ComboStationArea As System.Windows.Forms.ComboBox
    Friend WithEvents ComboStation_Value As System.Windows.Forms.ComboBox
    Friend WithEvents lblElevField As System.Windows.Forms.Label
    Friend WithEvents lblNameField As System.Windows.Forms.Label
    Friend WithEvents ComboSNOTEL_Elevation As System.Windows.Forms.ComboBox
    Friend WithEvents ComboSNOTEL_Name As System.Windows.Forms.ComboBox
    Friend WithEvents lblSNOTELUnit As System.Windows.Forms.Label
    Friend WithEvents OptSTMeter As System.Windows.Forms.RadioButton
    Friend WithEvents OptSTFoot As System.Windows.Forms.RadioButton
    Friend WithEvents ComboSC_Elevation As System.Windows.Forms.ComboBox
    Friend WithEvents ComboSC_Name As System.Windows.Forms.ComboBox
    Friend WithEvents lblSnowCourseUnit As System.Windows.Forms.Label
    Friend WithEvents OptSCMeter As System.Windows.Forms.RadioButton
    Friend WithEvents OptSCFoot As System.Windows.Forms.RadioButton
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents CmdAddLayer As System.Windows.Forms.Button
    Friend WithEvents CmdRemoveLayer As System.Windows.Forms.Button
    Friend WithEvents CmdClearAll As System.Windows.Forms.Button
    Friend WithEvents CmdSaveSettings As System.Windows.Forms.Button
    Friend WithEvents CmdUndo As System.Windows.Forms.Button
    Friend WithEvents CmdClose As System.Windows.Forms.Button
    Friend WithEvents FrameDEM As System.Windows.Forms.GroupBox
    Friend WithEvents OptFoot As System.Windows.Forms.RadioButton
    Friend WithEvents OptMeter As System.Windows.Forms.RadioButton
    Friend WithEvents FrameUnit As System.Windows.Forms.GroupBox
    Friend WithEvents GrpBoxSnowCourseUnit As System.Windows.Forms.GroupBox
    Friend WithEvents GrpBoxSNOTELUnit As System.Windows.Forms.GroupBox
    Friend WithEvents ChkboxAOIOnly As System.Windows.Forms.CheckBox
End Class
