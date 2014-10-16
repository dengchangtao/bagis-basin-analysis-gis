<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmHruContribAreaRule
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.TxtThreshold = New System.Windows.Forms.TextBox()
        Me.RdoThreeArea = New System.Windows.Forms.RadioButton()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.RdoTwoArea = New System.Windows.Forms.RadioButton()
        Me.LblMeanValue = New System.Windows.Forms.Label()
        Me.RdoOneArea = New System.Windows.Forms.RadioButton()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.BtnSave = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.LblMaxFlowValue = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.LblMinFlowValue = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.BtnAbout = New System.Windows.Forms.Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.BtnPreview = New System.Windows.Forms.Button()
        Me.TxtRuleID = New System.Windows.Forms.TextBox()
        Me.ChkKeepTempFiles = New System.Windows.Forms.CheckBox()
        Me.BtnTL_LUT = New System.Windows.Forms.Button()
        Me.DataGridViewLUT = New System.Windows.Forms.DataGridView()
        Me.FAccThreshold = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.StreamLinkCount = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.LblStDevValue = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        CType(Me.DataGridViewLUT, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TxtThreshold
        '
        Me.TxtThreshold.Location = New System.Drawing.Point(263, 159)
        Me.TxtThreshold.Name = "TxtThreshold"
        Me.TxtThreshold.Size = New System.Drawing.Size(109, 20)
        Me.TxtThreshold.TabIndex = 25
        Me.TxtThreshold.Text = "1"
        Me.TxtThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'RdoThreeArea
        '
        Me.RdoThreeArea.AutoSize = True
        Me.RdoThreeArea.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RdoThreeArea.Location = New System.Drawing.Point(88, 4)
        Me.RdoThreeArea.Name = "RdoThreeArea"
        Me.RdoThreeArea.Size = New System.Drawing.Size(34, 20)
        Me.RdoThreeArea.TabIndex = 2
        Me.RdoThreeArea.Text = "3"
        Me.RdoThreeArea.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(12, 160)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(250, 16)
        Me.Label3.TabIndex = 24
        Me.Label3.Text = "Flow Accumulation threshold value:"
        '
        'RdoTwoArea
        '
        Me.RdoTwoArea.AutoSize = True
        Me.RdoTwoArea.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RdoTwoArea.Location = New System.Drawing.Point(46, 4)
        Me.RdoTwoArea.Name = "RdoTwoArea"
        Me.RdoTwoArea.Size = New System.Drawing.Size(34, 20)
        Me.RdoTwoArea.TabIndex = 1
        Me.RdoTwoArea.Text = "2"
        Me.RdoTwoArea.UseVisualStyleBackColor = True
        '
        'LblMeanValue
        '
        Me.LblMeanValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMeanValue.ForeColor = System.Drawing.Color.Blue
        Me.LblMeanValue.Location = New System.Drawing.Point(92, 130)
        Me.LblMeanValue.Name = "LblMeanValue"
        Me.LblMeanValue.Size = New System.Drawing.Size(76, 16)
        Me.LblMeanValue.TabIndex = 23
        Me.LblMeanValue.Text = "Label3"
        '
        'RdoOneArea
        '
        Me.RdoOneArea.AutoSize = True
        Me.RdoOneArea.Checked = True
        Me.RdoOneArea.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RdoOneArea.Location = New System.Drawing.Point(4, 4)
        Me.RdoOneArea.Name = "RdoOneArea"
        Me.RdoOneArea.Size = New System.Drawing.Size(34, 20)
        Me.RdoOneArea.TabIndex = 0
        Me.RdoOneArea.TabStop = True
        Me.RdoOneArea.Text = "1"
        Me.RdoOneArea.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.RdoThreeArea)
        Me.Panel1.Controls.Add(Me.RdoTwoArea)
        Me.Panel1.Controls.Add(Me.RdoOneArea)
        Me.Panel1.Location = New System.Drawing.Point(159, 43)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(136, 30)
        Me.Panel1.TabIndex = 26
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(207, 192)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(75, 31)
        Me.BtnCancel.TabIndex = 22
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'BtnSave
        '
        Me.BtnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSave.Location = New System.Drawing.Point(297, 192)
        Me.BtnSave.Name = "BtnSave"
        Me.BtnSave.Size = New System.Drawing.Size(75, 31)
        Me.BtnSave.TabIndex = 21
        Me.BtnSave.Text = "Apply"
        Me.BtnSave.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(12, 48)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(141, 16)
        Me.Label6.TabIndex = 20
        Me.Label6.Text = "Contributing Zones:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(41, 130)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(45, 16)
        Me.Label5.TabIndex = 19
        Me.Label5.Text = "Mean:"
        '
        'LblMaxFlowValue
        '
        Me.LblMaxFlowValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMaxFlowValue.ForeColor = System.Drawing.Color.Blue
        Me.LblMaxFlowValue.Location = New System.Drawing.Point(234, 107)
        Me.LblMaxFlowValue.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.LblMaxFlowValue.Name = "LblMaxFlowValue"
        Me.LblMaxFlowValue.Size = New System.Drawing.Size(94, 16)
        Me.LblMaxFlowValue.TabIndex = 18
        Me.LblMaxFlowValue.Text = "Label3"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(192, 107)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(36, 16)
        Me.Label4.TabIndex = 17
        Me.Label4.Text = "Max:"
        '
        'LblMinFlowValue
        '
        Me.LblMinFlowValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMinFlowValue.ForeColor = System.Drawing.Color.Blue
        Me.LblMinFlowValue.Location = New System.Drawing.Point(92, 107)
        Me.LblMinFlowValue.Margin = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.LblMinFlowValue.Name = "LblMinFlowValue"
        Me.LblMinFlowValue.Size = New System.Drawing.Size(55, 16)
        Me.LblMinFlowValue.TabIndex = 16
        Me.LblMinFlowValue.Text = "Label2"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 83)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(207, 16)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Flow Accumulation Statistics:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(54, 107)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(32, 16)
        Me.Label8.TabIndex = 29
        Me.Label8.Text = "Min:"
        '
        'BtnAbout
        '
        Me.BtnAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAbout.Location = New System.Drawing.Point(283, 10)
        Me.BtnAbout.Name = "BtnAbout"
        Me.BtnAbout.Size = New System.Drawing.Size(102, 23)
        Me.BtnAbout.TabIndex = 30
        Me.BtnAbout.Text = "Tell me more"
        Me.BtnAbout.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(13, 13)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(264, 16)
        Me.Label7.TabIndex = 31
        Me.Label7.Text = "Generate contributing areas of stream links."
        '
        'BtnPreview
        '
        Me.BtnPreview.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnPreview.Location = New System.Drawing.Point(205, 238)
        Me.BtnPreview.Name = "BtnPreview"
        Me.BtnPreview.Size = New System.Drawing.Size(152, 23)
        Me.BtnPreview.TabIndex = 32
        Me.BtnPreview.Text = "Preview Stream Links"
        Me.BtnPreview.UseVisualStyleBackColor = True
        '
        'TxtRuleID
        '
        Me.TxtRuleID.Location = New System.Drawing.Point(16, 213)
        Me.TxtRuleID.Name = "TxtRuleID"
        Me.TxtRuleID.Size = New System.Drawing.Size(100, 20)
        Me.TxtRuleID.TabIndex = 33
        Me.TxtRuleID.Visible = False
        '
        'ChkKeepTempFiles
        '
        Me.ChkKeepTempFiles.AutoSize = True
        Me.ChkKeepTempFiles.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkKeepTempFiles.Location = New System.Drawing.Point(29, 200)
        Me.ChkKeepTempFiles.Name = "ChkKeepTempFiles"
        Me.ChkKeepTempFiles.Size = New System.Drawing.Size(168, 20)
        Me.ChkKeepTempFiles.TabIndex = 34
        Me.ChkKeepTempFiles.Text = "Keep Intermediate Files"
        Me.ChkKeepTempFiles.UseVisualStyleBackColor = True
        '
        'BtnTL_LUT
        '
        Me.BtnTL_LUT.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnTL_LUT.Location = New System.Drawing.Point(39, 238)
        Me.BtnTL_LUT.Name = "BtnTL_LUT"
        Me.BtnTL_LUT.Size = New System.Drawing.Size(152, 23)
        Me.BtnTL_LUT.TabIndex = 35
        Me.BtnTL_LUT.Text = "Threshold-Link LUT"
        Me.BtnTL_LUT.UseVisualStyleBackColor = True
        '
        'DataGridViewLUT
        '
        Me.DataGridViewLUT.AllowUserToAddRows = False
        Me.DataGridViewLUT.AllowUserToDeleteRows = False
        Me.DataGridViewLUT.AllowUserToOrderColumns = True
        Me.DataGridViewLUT.AllowUserToResizeRows = False
        Me.DataGridViewLUT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewLUT.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.FAccThreshold, Me.StreamLinkCount})
        Me.DataGridViewLUT.Location = New System.Drawing.Point(12, 273)
        Me.DataGridViewLUT.Name = "DataGridViewLUT"
        Me.DataGridViewLUT.ReadOnly = True
        Me.DataGridViewLUT.Size = New System.Drawing.Size(372, 230)
        Me.DataGridViewLUT.TabIndex = 37
        '
        'FAccThreshold
        '
        Me.FAccThreshold.HeaderText = "Flow Acc Values"
        Me.FAccThreshold.Name = "FAccThreshold"
        Me.FAccThreshold.ReadOnly = True
        Me.FAccThreshold.Width = 140
        '
        'StreamLinkCount
        '
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StreamLinkCount.DefaultCellStyle = DataGridViewCellStyle1
        Me.StreamLinkCount.HeaderText = "Est. Streamlinks Number"
        Me.StreamLinkCount.Name = "StreamLinkCount"
        Me.StreamLinkCount.ReadOnly = True
        Me.StreamLinkCount.Width = 160
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(174, 130)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(54, 16)
        Me.Label2.TabIndex = 38
        Me.Label2.Text = "St Dev.:"
        '
        'LblStDevValue
        '
        Me.LblStDevValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblStDevValue.ForeColor = System.Drawing.Color.Blue
        Me.LblStDevValue.Location = New System.Drawing.Point(234, 130)
        Me.LblStDevValue.Name = "LblStDevValue"
        Me.LblStDevValue.Size = New System.Drawing.Size(104, 16)
        Me.LblStDevValue.TabIndex = 39
        Me.LblStDevValue.Text = "St Dev Value"
        '
        'FrmHruContribAreaRule
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(396, 515)
        Me.Controls.Add(Me.LblStDevValue)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.DataGridViewLUT)
        Me.Controls.Add(Me.BtnTL_LUT)
        Me.Controls.Add(Me.ChkKeepTempFiles)
        Me.Controls.Add(Me.TxtRuleID)
        Me.Controls.Add(Me.BtnPreview)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.BtnAbout)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.TxtThreshold)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.LblMeanValue)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnSave)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.LblMaxFlowValue)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.LblMinFlowValue)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "FrmHruContribAreaRule"
        Me.ShowIcon = False
        Me.Text = "Contributing Area"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.DataGridViewLUT, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TxtThreshold As System.Windows.Forms.TextBox
    Friend WithEvents RdoThreeArea As System.Windows.Forms.RadioButton
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents RdoTwoArea As System.Windows.Forms.RadioButton
    Friend WithEvents LblMeanValue As System.Windows.Forms.Label
    Friend WithEvents RdoOneArea As System.Windows.Forms.RadioButton
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents BtnSave As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents LblMaxFlowValue As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents LblMinFlowValue As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents BtnAbout As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents BtnPreview As System.Windows.Forms.Button
    Friend WithEvents TxtRuleID As System.Windows.Forms.TextBox
    Friend WithEvents ChkKeepTempFiles As System.Windows.Forms.CheckBox
    Friend WithEvents BtnTL_LUT As System.Windows.Forms.Button
    Friend WithEvents DataGridViewLUT As System.Windows.Forms.DataGridView
    Friend WithEvents FAccThreshold As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents StreamLinkCount As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents LblStDevValue As System.Windows.Forms.Label
End Class