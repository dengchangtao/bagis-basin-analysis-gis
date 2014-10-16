<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TabReclassRuleCtrl
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
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
        Me.Label10 = New System.Windows.Forms.Label
        Me.GridReclassValues = New System.Windows.Forms.DataGridView
        Me.OldValues = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.NewValues = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TxtReclass = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.BtnViewRule = New System.Windows.Forms.Button
        Me.BtnViewInput = New System.Windows.Forms.Button
        Me.LblHruPath = New System.Windows.Forms.Label
        CType(Me.GridReclassValues, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TxtRuleType
        '
        Me.TxtRuleType.Location = New System.Drawing.Point(61, 9)
        Me.TxtRuleType.Name = "TxtRuleType"
        Me.TxtRuleType.ReadOnly = True
        Me.TxtRuleType.Size = New System.Drawing.Size(494, 20)
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
        Me.TxtInputLayerName.Size = New System.Drawing.Size(451, 20)
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
        Me.TxtStatus.Location = New System.Drawing.Point(45, 117)
        Me.TxtStatus.Name = "TxtStatus"
        Me.TxtStatus.ReadOnly = True
        Me.TxtStatus.Size = New System.Drawing.Size(100, 20)
        Me.TxtStatus.TabIndex = 56
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(2, 120)
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
        Me.TxtOutputLayer.Size = New System.Drawing.Size(556, 20)
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
        Me.TxtInputLayerPath.Size = New System.Drawing.Size(539, 20)
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
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(2, 167)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(97, 13)
        Me.Label10.TabIndex = 62
        Me.Label10.Text = "Reclass values:"
        '
        'GridReclassValues
        '
        Me.GridReclassValues.AllowUserToAddRows = False
        Me.GridReclassValues.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.GridReclassValues.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.GridReclassValues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridReclassValues.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.OldValues, Me.NewValues})
        Me.GridReclassValues.Location = New System.Drawing.Point(96, 167)
        Me.GridReclassValues.Name = "GridReclassValues"
        Me.GridReclassValues.ReadOnly = True
        Me.GridReclassValues.Size = New System.Drawing.Size(255, 185)
        Me.GridReclassValues.TabIndex = 61
        '
        'OldValues
        '
        Me.OldValues.HeaderText = "Old Values"
        Me.OldValues.Name = "OldValues"
        Me.OldValues.ReadOnly = True
        Me.OldValues.Width = 95
        '
        'NewValues
        '
        Me.NewValues.HeaderText = "New Values"
        Me.NewValues.Name = "NewValues"
        Me.NewValues.ReadOnly = True
        Me.NewValues.Width = 95
        '
        'TxtReclass
        '
        Me.TxtReclass.Location = New System.Drawing.Point(83, 142)
        Me.TxtReclass.Name = "TxtReclass"
        Me.TxtReclass.ReadOnly = True
        Me.TxtReclass.Size = New System.Drawing.Size(100, 20)
        Me.TxtReclass.TabIndex = 60
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(2, 145)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(84, 13)
        Me.Label9.TabIndex = 59
        Me.Label9.Text = "Reclass field:"
        '
        'BtnViewRule
        '
        Me.BtnViewRule.Location = New System.Drawing.Point(561, 6)
        Me.BtnViewRule.Name = "BtnViewRule"
        Me.BtnViewRule.Size = New System.Drawing.Size(75, 23)
        Me.BtnViewRule.TabIndex = 63
        Me.BtnViewRule.Text = "View Layer"
        Me.BtnViewRule.UseVisualStyleBackColor = True
        '
        'BtnViewInput
        '
        Me.BtnViewInput.Location = New System.Drawing.Point(561, 36)
        Me.BtnViewInput.Name = "BtnViewInput"
        Me.BtnViewInput.Size = New System.Drawing.Size(75, 23)
        Me.BtnViewInput.TabIndex = 64
        Me.BtnViewInput.Text = "View Layer"
        Me.BtnViewInput.UseVisualStyleBackColor = True
        '
        'LblHruPath
        '
        Me.LblHruPath.AutoSize = True
        Me.LblHruPath.Location = New System.Drawing.Point(453, 258)
        Me.LblHruPath.Name = "LblHruPath"
        Me.LblHruPath.Size = New System.Drawing.Size(60, 13)
        Me.LblHruPath.TabIndex = 65
        Me.LblHruPath.Text = "LblHruPath"
        Me.LblHruPath.Visible = False
        '
        'TabReclassRuleCtrl
        '
        Me.Controls.Add(Me.LblHruPath)
        Me.Controls.Add(Me.BtnViewInput)
        Me.Controls.Add(Me.BtnViewRule)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.GridReclassValues)
        Me.Controls.Add(Me.TxtReclass)
        Me.Controls.Add(Me.Label9)
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
        Me.Name = "TabReclassRuleCtrl"
        Me.Size = New System.Drawing.Size(650, 375)
        CType(Me.GridReclassValues, System.ComponentModel.ISupportInitialize).EndInit()
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

        ' Reclass specific fields
        Dim reclassRule As BAGIS_ClassLibrary.RasterReclassRule = CType(rule, BAGIS_ClassLibrary.RasterReclassRule)
        TxtReclass.Text = reclassRule.ReclassField
        Dim reclassItems As BAGIS_ClassLibrary.ReclassItem() = reclassRule.ReclassItems
        If reclassItems IsNot Nothing AndAlso reclassItems.Length > 0 Then
            For i As Short = 0 To reclassItems.GetUpperBound(0) Step 1
                Dim reclassItem As BAGIS_ClassLibrary.ReclassItem = reclassItems(i)
                Dim displayItem As String() = {CStr(reclassItem.FromValue), CStr(reclassItem.OutputValue)}
                Me.GridReclassValues.Rows.Add(displayItem)
            Next
        End If
        Dim reclassTextItems As BAGIS_ClassLibrary.ReclassTextItem() = reclassRule.ReclassTextItems
        If reclassTextItems IsNot Nothing AndAlso reclassTextItems.Length > 0 Then
            For i As Short = 0 To reclassTextItems.GetUpperBound(0) Step 1
                Dim reclassItem As BAGIS_ClassLibrary.ReclassTextItem = reclassTextItems(i)
                Dim displayItem As String() = {reclassItem.FromValue, CStr(reclassItem.OutputValue)}
                Me.GridReclassValues.Rows.Add(displayItem)
            Next
        End If


    End Sub

    Friend WithEvents TxtInputLayerName As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TxtStatus As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TxtOutputLayer As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TxtInputLayerPath As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents GridReclassValues As System.Windows.Forms.DataGridView
    Friend WithEvents OldValues As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NewValues As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TxtReclass As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents BtnViewRule As System.Windows.Forms.Button
    Friend WithEvents BtnViewInput As System.Windows.Forms.Button
    Friend WithEvents LblHruPath As System.Windows.Forms.Label
End Class
