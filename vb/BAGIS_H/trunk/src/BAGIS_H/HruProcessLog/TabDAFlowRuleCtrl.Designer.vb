<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TabDAFlowRuleCtrl
    Inherits System.Windows.Forms.TabPage
    'Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.TxtRuleType = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.TxtInputLayerName = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.TxtStatus = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.TxtOutputLayer = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.TxtInputLayerPath = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.BtnViewRule = New System.Windows.Forms.Button
        Me.BtnViewInput = New System.Windows.Forms.Button
        Me.Label6 = New System.Windows.Forms.Label
        Me.RdoHruDimension = New System.Windows.Forms.RadioButton
        Me.RdoHruNumber = New System.Windows.Forms.RadioButton
        Me.RdoRowCol = New System.Windows.Forms.RadioButton
        Me.PnlHruDimension = New System.Windows.Forms.Panel
        Me.TxtXSize = New System.Windows.Forms.TextBox
        Me.RadBtnUnknown = New System.Windows.Forms.RadioButton
        Me.TxtYSize = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.RadBtnMile = New System.Windows.Forms.RadioButton
        Me.Label8 = New System.Windows.Forms.Label
        Me.RadBtnFoot = New System.Windows.Forms.RadioButton
        Me.RadBtnMeter = New System.Windows.Forms.RadioButton
        Me.PnlHruNumber = New System.Windows.Forms.Panel
        Me.TxtNumberHru = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.PnlRowCol = New System.Windows.Forms.Panel
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.TxtRow = New System.Windows.Forms.TextBox
        Me.TxtCol = New System.Windows.Forms.TextBox
        Me.LblHruPath = New System.Windows.Forms.Label
        Me.PnlHruDimension.SuspendLayout()
        Me.PnlHruNumber.SuspendLayout()
        Me.PnlRowCol.SuspendLayout()
        Me.SuspendLayout()
        '
        'TxtRuleType
        '
        Me.TxtRuleType.Location = New System.Drawing.Point(61, 9)
        Me.TxtRuleType.Name = "TxtRuleType"
        Me.TxtRuleType.ReadOnly = True
        Me.TxtRuleType.Size = New System.Drawing.Size(492, 20)
        Me.TxtRuleType.TabIndex = 44
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(2, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(65, 13)
        Me.Label1.TabIndex = 43
        Me.Label1.Text = "Rule type:"
        '
        'TxtInputLayerName
        '
        Me.TxtInputLayerName.Location = New System.Drawing.Point(104, 37)
        Me.TxtInputLayerName.Name = "TxtInputLayerName"
        Me.TxtInputLayerName.ReadOnly = True
        Me.TxtInputLayerName.Size = New System.Drawing.Size(449, 20)
        Me.TxtInputLayerName.TabIndex = 58
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(2, 40)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(105, 13)
        Me.Label5.TabIndex = 57
        Me.Label5.Text = "Input layer name:"
        '
        'TxtStatus
        '
        Me.TxtStatus.Location = New System.Drawing.Point(45, 116)
        Me.TxtStatus.Name = "TxtStatus"
        Me.TxtStatus.ReadOnly = True
        Me.TxtStatus.Size = New System.Drawing.Size(100, 20)
        Me.TxtStatus.TabIndex = 56
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(2, 119)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(47, 13)
        Me.Label4.TabIndex = 55
        Me.Label4.Text = "Status:"
        '
        'TxtOutputLayer
        '
        Me.TxtOutputLayer.Location = New System.Drawing.Point(82, 91)
        Me.TxtOutputLayer.Name = "TxtOutputLayer"
        Me.TxtOutputLayer.ReadOnly = True
        Me.TxtOutputLayer.Size = New System.Drawing.Size(555, 20)
        Me.TxtOutputLayer.TabIndex = 54
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(2, 94)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(80, 13)
        Me.Label3.TabIndex = 53
        Me.Label3.Text = "Output layer:"
        '
        'TxtInputLayerPath
        '
        Me.TxtInputLayerPath.Location = New System.Drawing.Point(99, 65)
        Me.TxtInputLayerPath.Name = "TxtInputLayerPath"
        Me.TxtInputLayerPath.ReadOnly = True
        Me.TxtInputLayerPath.Size = New System.Drawing.Size(538, 20)
        Me.TxtInputLayerPath.TabIndex = 52
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(2, 68)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 13)
        Me.Label2.TabIndex = 51
        Me.Label2.Text = "Input layer path:"
        '
        'BtnViewRule
        '
        Me.BtnViewRule.Location = New System.Drawing.Point(560, 6)
        Me.BtnViewRule.Name = "BtnViewRule"
        Me.BtnViewRule.Size = New System.Drawing.Size(75, 23)
        Me.BtnViewRule.TabIndex = 65
        Me.BtnViewRule.Text = "View Layer"
        Me.BtnViewRule.UseVisualStyleBackColor = True
        '
        'BtnViewInput
        '
        Me.BtnViewInput.Location = New System.Drawing.Point(560, 36)
        Me.BtnViewInput.Name = "BtnViewInput"
        Me.BtnViewInput.Size = New System.Drawing.Size(75, 23)
        Me.BtnViewInput.TabIndex = 72
        Me.BtnViewInput.Text = "View Layer"
        Me.BtnViewInput.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(2, 142)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(65, 13)
        Me.Label6.TabIndex = 73
        Me.Label6.Text = "Flow type:"
        '
        'RdoHruDimension
        '
        Me.RdoHruDimension.AutoCheck = False
        Me.RdoHruDimension.AutoSize = True
        Me.RdoHruDimension.Location = New System.Drawing.Point(67, 140)
        Me.RdoHruDimension.Name = "RdoHruDimension"
        Me.RdoHruDimension.Size = New System.Drawing.Size(119, 17)
        Me.RdoHruDimension.TabIndex = 74
        Me.RdoHruDimension.Text = "By HRU dimensions"
        Me.RdoHruDimension.UseVisualStyleBackColor = True
        '
        'RdoHruNumber
        '
        Me.RdoHruNumber.AutoCheck = False
        Me.RdoHruNumber.AutoSize = True
        Me.RdoHruNumber.Location = New System.Drawing.Point(185, 141)
        Me.RdoHruNumber.Name = "RdoHruNumber"
        Me.RdoHruNumber.Size = New System.Drawing.Size(91, 17)
        Me.RdoHruNumber.TabIndex = 75
        Me.RdoHruNumber.Text = "By # of HRUs"
        Me.RdoHruNumber.UseVisualStyleBackColor = True
        '
        'RdoRowCol
        '
        Me.RdoRowCol.AutoCheck = False
        Me.RdoRowCol.AutoSize = True
        Me.RdoRowCol.Location = New System.Drawing.Point(274, 141)
        Me.RdoRowCol.Name = "RdoRowCol"
        Me.RdoRowCol.Size = New System.Drawing.Size(128, 17)
        Me.RdoRowCol.TabIndex = 76
        Me.RdoRowCol.Text = "By # of rows/columns"
        Me.RdoRowCol.UseVisualStyleBackColor = True
        '
        'PnlHruDimension
        '
        Me.PnlHruDimension.Controls.Add(Me.TxtXSize)
        Me.PnlHruDimension.Controls.Add(Me.RadBtnUnknown)
        Me.PnlHruDimension.Controls.Add(Me.TxtYSize)
        Me.PnlHruDimension.Controls.Add(Me.Label7)
        Me.PnlHruDimension.Controls.Add(Me.Label9)
        Me.PnlHruDimension.Controls.Add(Me.RadBtnMile)
        Me.PnlHruDimension.Controls.Add(Me.Label8)
        Me.PnlHruDimension.Controls.Add(Me.RadBtnFoot)
        Me.PnlHruDimension.Controls.Add(Me.RadBtnMeter)
        Me.PnlHruDimension.Location = New System.Drawing.Point(1, 160)
        Me.PnlHruDimension.Name = "PnlHruDimension"
        Me.PnlHruDimension.Size = New System.Drawing.Size(302, 44)
        Me.PnlHruDimension.TabIndex = 86
        '
        'TxtXSize
        '
        Me.TxtXSize.BackColor = System.Drawing.SystemColors.Control
        Me.TxtXSize.Location = New System.Drawing.Point(48, 0)
        Me.TxtXSize.Name = "TxtXSize"
        Me.TxtXSize.ReadOnly = True
        Me.TxtXSize.Size = New System.Drawing.Size(96, 20)
        Me.TxtXSize.TabIndex = 77
        '
        'RadBtnUnknown
        '
        Me.RadBtnUnknown.AutoCheck = False
        Me.RadBtnUnknown.AutoSize = True
        Me.RadBtnUnknown.Location = New System.Drawing.Point(194, 26)
        Me.RadBtnUnknown.Name = "RadBtnUnknown"
        Me.RadBtnUnknown.Size = New System.Drawing.Size(71, 17)
        Me.RadBtnUnknown.TabIndex = 85
        Me.RadBtnUnknown.Text = "Unknown"
        Me.RadBtnUnknown.UseVisualStyleBackColor = True
        '
        'TxtYSize
        '
        Me.TxtYSize.BackColor = System.Drawing.SystemColors.Control
        Me.TxtYSize.Location = New System.Drawing.Point(197, 1)
        Me.TxtYSize.Name = "TxtYSize"
        Me.TxtYSize.ReadOnly = True
        Me.TxtYSize.Size = New System.Drawing.Size(95, 20)
        Me.TxtYSize.TabIndex = 78
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(2, 27)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(34, 13)
        Me.Label7.TabIndex = 84
        Me.Label7.Text = "Unit:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(2, 3)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(47, 13)
        Me.Label9.TabIndex = 79
        Me.Label9.Text = "X Size:"
        '
        'RadBtnMile
        '
        Me.RadBtnMile.AutoCheck = False
        Me.RadBtnMile.AutoSize = True
        Me.RadBtnMile.Location = New System.Drawing.Point(145, 25)
        Me.RadBtnMile.Name = "RadBtnMile"
        Me.RadBtnMile.Size = New System.Drawing.Size(49, 17)
        Me.RadBtnMile.TabIndex = 83
        Me.RadBtnMile.Text = "Miles"
        Me.RadBtnMile.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(151, 4)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(47, 13)
        Me.Label8.TabIndex = 80
        Me.Label8.Text = "Y Size:"
        '
        'RadBtnFoot
        '
        Me.RadBtnFoot.AutoCheck = False
        Me.RadBtnFoot.AutoSize = True
        Me.RadBtnFoot.Location = New System.Drawing.Point(97, 25)
        Me.RadBtnFoot.Name = "RadBtnFoot"
        Me.RadBtnFoot.Size = New System.Drawing.Size(46, 17)
        Me.RadBtnFoot.TabIndex = 82
        Me.RadBtnFoot.Text = "Feet"
        Me.RadBtnFoot.UseVisualStyleBackColor = True
        '
        'RadBtnMeter
        '
        Me.RadBtnMeter.AutoCheck = False
        Me.RadBtnMeter.AutoSize = True
        Me.RadBtnMeter.Location = New System.Drawing.Point(41, 25)
        Me.RadBtnMeter.Name = "RadBtnMeter"
        Me.RadBtnMeter.Size = New System.Drawing.Size(57, 17)
        Me.RadBtnMeter.TabIndex = 81
        Me.RadBtnMeter.Text = "Meters" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.RadBtnMeter.UseVisualStyleBackColor = True
        '
        'PnlHruNumber
        '
        Me.PnlHruNumber.Controls.Add(Me.TxtNumberHru)
        Me.PnlHruNumber.Controls.Add(Me.Label10)
        Me.PnlHruNumber.Location = New System.Drawing.Point(0, 211)
        Me.PnlHruNumber.Name = "PnlHruNumber"
        Me.PnlHruNumber.Size = New System.Drawing.Size(303, 26)
        Me.PnlHruNumber.TabIndex = 87
        '
        'TxtNumberHru
        '
        Me.TxtNumberHru.BackColor = System.Drawing.SystemColors.Control
        Me.TxtNumberHru.Location = New System.Drawing.Point(128, 1)
        Me.TxtNumberHru.Name = "TxtNumberHru"
        Me.TxtNumberHru.ReadOnly = True
        Me.TxtNumberHru.Size = New System.Drawing.Size(95, 20)
        Me.TxtNumberHru.TabIndex = 79
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(1, 4)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(130, 13)
        Me.Label10.TabIndex = 6
        Me.Label10.Text = "Estimated # of HRUs:"
        '
        'PnlRowCol
        '
        Me.PnlRowCol.Controls.Add(Me.Label11)
        Me.PnlRowCol.Controls.Add(Me.Label12)
        Me.PnlRowCol.Controls.Add(Me.TxtRow)
        Me.PnlRowCol.Controls.Add(Me.TxtCol)
        Me.PnlRowCol.Location = New System.Drawing.Point(2, 245)
        Me.PnlRowCol.Name = "PnlRowCol"
        Me.PnlRowCol.Size = New System.Drawing.Size(300, 27)
        Me.PnlRowCol.TabIndex = 88
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(149, 4)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(83, 13)
        Me.Label11.TabIndex = 7
        Me.Label11.Text = "# of Row (Y):"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(0, 4)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(76, 13)
        Me.Label12.TabIndex = 6
        Me.Label12.Text = "# of Col (X):"
        '
        'TxtRow
        '
        Me.TxtRow.BackColor = System.Drawing.SystemColors.Control
        Me.TxtRow.Location = New System.Drawing.Point(227, 1)
        Me.TxtRow.Name = "TxtRow"
        Me.TxtRow.ReadOnly = True
        Me.TxtRow.Size = New System.Drawing.Size(60, 20)
        Me.TxtRow.TabIndex = 5
        '
        'TxtCol
        '
        Me.TxtCol.BackColor = System.Drawing.SystemColors.Control
        Me.TxtCol.Location = New System.Drawing.Point(71, 1)
        Me.TxtCol.Name = "TxtCol"
        Me.TxtCol.ReadOnly = True
        Me.TxtCol.Size = New System.Drawing.Size(60, 20)
        Me.TxtCol.TabIndex = 4
        '
        'LblHruPath
        '
        Me.LblHruPath.AutoSize = True
        Me.LblHruPath.Location = New System.Drawing.Point(444, 141)
        Me.LblHruPath.Name = "LblHruPath"
        Me.LblHruPath.Size = New System.Drawing.Size(60, 13)
        Me.LblHruPath.TabIndex = 89
        Me.LblHruPath.Text = "LblHruPath"
        Me.LblHruPath.Visible = False
        '
        'TabDAFlowRuleCtrl
        '
        Me.Controls.Add(Me.LblHruPath)
        Me.Controls.Add(Me.PnlRowCol)
        Me.Controls.Add(Me.PnlHruNumber)
        Me.Controls.Add(Me.PnlHruDimension)
        Me.Controls.Add(Me.RdoRowCol)
        Me.Controls.Add(Me.RdoHruNumber)
        Me.Controls.Add(Me.RdoHruDimension)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.BtnViewInput)
        Me.Controls.Add(Me.BtnViewRule)
        Me.Controls.Add(Me.TxtInputLayerName)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TxtStatus)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.TxtOutputLayer)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TxtInputLayerPath)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TxtRuleType)
        Me.Controls.Add(Me.Label1)
        Me.Name = "TabDAFlowRuleCtrl"
        Me.Size = New System.Drawing.Size(650, 375)
        Me.PnlHruDimension.ResumeLayout(False)
        Me.PnlHruDimension.PerformLayout()
        Me.PnlHruNumber.ResumeLayout(False)
        Me.PnlHruNumber.PerformLayout()
        Me.PnlRowCol.ResumeLayout(False)
        Me.PnlRowCol.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TxtRuleType As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label

    Public Sub New(ByVal Hru As BAGIS_ClassLibrary.Hru, ByVal rule As BAGIS_ClassLibrary.IRule, ByVal isParent As Boolean)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Text = "Rule " & rule.RuleId
        TxtRuleType.Text = rule.RuleTypeText
        TxtInputLayerName.Text = rule.InputLayerName
        TxtInputLayerPath.Text = rule.InputFolderPath
        TxtOutputLayer.Text = rule.OutputDatasetName
        LblHruPath.Text = Hru.FilePath
        TxtStatus.Text = rule.FactorStatus.ToString
        If isParent = True Then
            BtnViewInput.Enabled = False
            BtnViewRule.Enabled = False
        End If

        ' DAFlow specific fields
        Dim daFlowRule As BAGIS_ClassLibrary.DAFlowTypeZonesRule = CType(rule, BAGIS_ClassLibrary.DAFlowTypeZonesRule)
        Select Case daFlowRule.ByParameter
            Case BAGIS_ClassLibrary.DAFlowByParam.BA_DAFlowByXYDimension
                RdoHruDimension.Checked = True
                PnlHruDimension.Visible = True
                PnlHruDimension.Location = New System.Drawing.Point(1, 160)
                PnlHruNumber.Visible = False
                PnlRowCol.Visible = False
                TxtXSize.Text = Format(daFlowRule.HruXSize, "###,###,##0.###")
                TxtYSize.Text = Format(daFlowRule.HruYSize, "###,###,##0.###")
                Select Case daFlowRule.MeasurementUnits
                    Case BAGIS_ClassLibrary.MeasurementUnit.Meters
                        RadBtnMeter.Checked = True
                    Case BAGIS_ClassLibrary.MeasurementUnit.Feet
                        RadBtnFoot.Checked = True
                    Case BAGIS_ClassLibrary.MeasurementUnit.Miles
                        RadBtnMile.Checked = True
                    Case Else
                        RadBtnUnknown.Checked = True
                End Select
            Case BAGIS_ClassLibrary.DAFlowByParam.BA_DAFlowByHRUNumber
                RdoHruNumber.Checked = True
                PnlHruDimension.Visible = False
                PnlHruNumber.Visible = True
                PnlHruNumber.Location = PnlHruDimension.Location
                PnlRowCol.Visible = False
                TxtNumberHru.Text = CStr(daFlowRule.HruNumber)
            Case BAGIS_ClassLibrary.DAFlowByParam.BA_DAFlowByRowCol
                RdoRowCol.Checked = True
                PnlHruDimension.Visible = False
                PnlHruNumber.Visible = False
                PnlRowCol.Visible = True
                PnlRowCol.Location = PnlHruDimension.Location
                TxtCol.Text = daFlowRule.HruCol
                TxtRow.Text = daFlowRule.HruRow
        End Select

    End Sub
    Friend WithEvents TxtInputLayerName As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TxtStatus As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TxtOutputLayer As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TxtInputLayerPath As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents BtnViewRule As System.Windows.Forms.Button
    Friend WithEvents BtnViewInput As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents RdoHruDimension As System.Windows.Forms.RadioButton
    Friend WithEvents RdoHruNumber As System.Windows.Forms.RadioButton
    Friend WithEvents RdoRowCol As System.Windows.Forms.RadioButton
    Friend WithEvents PnlHruDimension As System.Windows.Forms.Panel
    Friend WithEvents TxtXSize As System.Windows.Forms.TextBox
    Friend WithEvents RadBtnUnknown As System.Windows.Forms.RadioButton
    Friend WithEvents TxtYSize As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents RadBtnMile As System.Windows.Forms.RadioButton
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents RadBtnFoot As System.Windows.Forms.RadioButton
    Friend WithEvents RadBtnMeter As System.Windows.Forms.RadioButton
    Friend WithEvents PnlHruNumber As System.Windows.Forms.Panel
    Friend WithEvents TxtNumberHru As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents PnlRowCol As System.Windows.Forms.Panel
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents TxtRow As System.Windows.Forms.TextBox
    Friend WithEvents TxtCol As System.Windows.Forms.TextBox
    Friend WithEvents LblHruPath As System.Windows.Forms.Label
End Class
