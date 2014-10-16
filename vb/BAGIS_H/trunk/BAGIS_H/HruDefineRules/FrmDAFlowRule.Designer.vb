<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmDAFlowRule
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
        Me.RadBtnByHRUNo = New System.Windows.Forms.RadioButton
        Me.RadBtnByRowCol = New System.Windows.Forms.RadioButton
        Me.BtnCancel = New System.Windows.Forms.Button
        Me.BtnApply = New System.Windows.Forms.Button
        Me.RadBtnByHRUDimension = New System.Windows.Forms.RadioButton
        Me.PanHRUNumber = New System.Windows.Forms.Panel
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.TxtHRUNumber = New System.Windows.Forms.TextBox
        Me.PanRowCol = New System.Windows.Forms.Panel
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.TxtRow = New System.Windows.Forms.TextBox
        Me.TxtCol = New System.Windows.Forms.TextBox
        Me.PanXYSize = New System.Windows.Forms.Panel
        Me.RadBtnUnknown = New System.Windows.Forms.RadioButton
        Me.Label6 = New System.Windows.Forms.Label
        Me.RadBtnMile = New System.Windows.Forms.RadioButton
        Me.RadBtnFoot = New System.Windows.Forms.RadioButton
        Me.RadBtnMeter = New System.Windows.Forms.RadioButton
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.TxtYSize = New System.Windows.Forms.TextBox
        Me.TxtXSize = New System.Windows.Forms.TextBox
        Me.TxtRuleID = New System.Windows.Forms.TextBox
        Me.BtnTest = New System.Windows.Forms.Button
        Me.Label8 = New System.Windows.Forms.Label
        Me.BtnAbout = New System.Windows.Forms.Button
        Me.PanHRUNumber.SuspendLayout()
        Me.PanRowCol.SuspendLayout()
        Me.PanXYSize.SuspendLayout()
        Me.SuspendLayout()
        '
        'RadBtnByHRUNo
        '
        Me.RadBtnByHRUNo.AutoSize = True
        Me.RadBtnByHRUNo.Location = New System.Drawing.Point(13, 113)
        Me.RadBtnByHRUNo.Margin = New System.Windows.Forms.Padding(4)
        Me.RadBtnByHRUNo.Name = "RadBtnByHRUNo"
        Me.RadBtnByHRUNo.Size = New System.Drawing.Size(165, 20)
        Me.RadBtnByHRUNo.TabIndex = 0
        Me.RadBtnByHRUNo.Text = "By the number of HRUs"
        Me.RadBtnByHRUNo.UseVisualStyleBackColor = True
        '
        'RadBtnByRowCol
        '
        Me.RadBtnByRowCol.AutoSize = True
        Me.RadBtnByRowCol.Location = New System.Drawing.Point(13, 206)
        Me.RadBtnByRowCol.Margin = New System.Windows.Forms.Padding(4)
        Me.RadBtnByRowCol.Name = "RadBtnByRowCol"
        Me.RadBtnByRowCol.Size = New System.Drawing.Size(242, 20)
        Me.RadBtnByRowCol.TabIndex = 1
        Me.RadBtnByRowCol.Text = "By the numbers of rows and columns"
        Me.RadBtnByRowCol.UseVisualStyleBackColor = True
        '
        'BtnCancel
        '
        Me.BtnCancel.Location = New System.Drawing.Point(147, 259)
        Me.BtnCancel.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(100, 28)
        Me.BtnCancel.TabIndex = 2
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'BtnApply
        '
        Me.BtnApply.Location = New System.Drawing.Point(255, 259)
        Me.BtnApply.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnApply.Name = "BtnApply"
        Me.BtnApply.Size = New System.Drawing.Size(100, 28)
        Me.BtnApply.TabIndex = 3
        Me.BtnApply.Text = "Apply"
        Me.BtnApply.UseVisualStyleBackColor = True
        '
        'RadBtnByHRUDimension
        '
        Me.RadBtnByHRUDimension.AutoSize = True
        Me.RadBtnByHRUDimension.Checked = True
        Me.RadBtnByHRUDimension.Location = New System.Drawing.Point(13, 36)
        Me.RadBtnByHRUDimension.Margin = New System.Windows.Forms.Padding(4)
        Me.RadBtnByHRUDimension.Name = "RadBtnByHRUDimension"
        Me.RadBtnByHRUDimension.Size = New System.Drawing.Size(182, 20)
        Me.RadBtnByHRUDimension.TabIndex = 6
        Me.RadBtnByHRUDimension.TabStop = True
        Me.RadBtnByHRUDimension.Text = "By the dimensions of HRU"
        Me.RadBtnByHRUDimension.UseVisualStyleBackColor = True
        '
        'PanHRUNumber
        '
        Me.PanHRUNumber.Controls.Add(Me.Label7)
        Me.PanHRUNumber.Controls.Add(Me.Label3)
        Me.PanHRUNumber.Controls.Add(Me.TxtHRUNumber)
        Me.PanHRUNumber.Location = New System.Drawing.Point(30, 130)
        Me.PanHRUNumber.Name = "PanHRUNumber"
        Me.PanHRUNumber.Size = New System.Drawing.Size(325, 69)
        Me.PanHRUNumber.TabIndex = 8
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.ForeColor = System.Drawing.Color.Red
        Me.Label7.Location = New System.Drawing.Point(7, 29)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(305, 32)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = "This is an estimated number. The actual number is" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "affected by the shape of the A" & _
            "OI."
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(7, 7)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(135, 16)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Estimated # of HRUs:"
        '
        'TxtHRUNumber
        '
        Me.TxtHRUNumber.AcceptsReturn = True
        Me.TxtHRUNumber.Location = New System.Drawing.Point(148, 4)
        Me.TxtHRUNumber.Name = "TxtHRUNumber"
        Me.TxtHRUNumber.Size = New System.Drawing.Size(100, 22)
        Me.TxtHRUNumber.TabIndex = 3
        '
        'PanRowCol
        '
        Me.PanRowCol.Controls.Add(Me.Label2)
        Me.PanRowCol.Controls.Add(Me.Label1)
        Me.PanRowCol.Controls.Add(Me.TxtRow)
        Me.PanRowCol.Controls.Add(Me.TxtCol)
        Me.PanRowCol.Location = New System.Drawing.Point(30, 225)
        Me.PanRowCol.Name = "PanRowCol"
        Me.PanRowCol.Size = New System.Drawing.Size(325, 27)
        Me.PanRowCol.TabIndex = 9
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(170, 2)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(82, 16)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "# of Row (Y):"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(7, 2)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(74, 16)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "# of Col (X):"
        '
        'TxtRow
        '
        Me.TxtRow.Location = New System.Drawing.Point(258, 2)
        Me.TxtRow.Name = "TxtRow"
        Me.TxtRow.Size = New System.Drawing.Size(60, 22)
        Me.TxtRow.TabIndex = 5
        '
        'TxtCol
        '
        Me.TxtCol.Location = New System.Drawing.Point(84, 2)
        Me.TxtCol.Name = "TxtCol"
        Me.TxtCol.Size = New System.Drawing.Size(60, 22)
        Me.TxtCol.TabIndex = 4
        '
        'PanXYSize
        '
        Me.PanXYSize.Controls.Add(Me.RadBtnUnknown)
        Me.PanXYSize.Controls.Add(Me.Label6)
        Me.PanXYSize.Controls.Add(Me.RadBtnMile)
        Me.PanXYSize.Controls.Add(Me.RadBtnFoot)
        Me.PanXYSize.Controls.Add(Me.RadBtnMeter)
        Me.PanXYSize.Controls.Add(Me.Label5)
        Me.PanXYSize.Controls.Add(Me.Label4)
        Me.PanXYSize.Controls.Add(Me.TxtYSize)
        Me.PanXYSize.Controls.Add(Me.TxtXSize)
        Me.PanXYSize.Location = New System.Drawing.Point(30, 54)
        Me.PanXYSize.Name = "PanXYSize"
        Me.PanXYSize.Size = New System.Drawing.Size(325, 52)
        Me.PanXYSize.TabIndex = 10
        '
        'RadBtnUnknown
        '
        Me.RadBtnUnknown.AutoSize = True
        Me.RadBtnUnknown.Location = New System.Drawing.Point(232, 29)
        Me.RadBtnUnknown.Name = "RadBtnUnknown"
        Me.RadBtnUnknown.Size = New System.Drawing.Size(81, 20)
        Me.RadBtnUnknown.TabIndex = 16
        Me.RadBtnUnknown.TabStop = True
        Me.RadBtnUnknown.Text = "Unknown"
        Me.RadBtnUnknown.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(7, 31)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(34, 16)
        Me.Label6.TabIndex = 15
        Me.Label6.Text = "Unit:"
        '
        'RadBtnMile
        '
        Me.RadBtnMile.AutoSize = True
        Me.RadBtnMile.Location = New System.Drawing.Point(170, 29)
        Me.RadBtnMile.Name = "RadBtnMile"
        Me.RadBtnMile.Size = New System.Drawing.Size(58, 20)
        Me.RadBtnMile.TabIndex = 14
        Me.RadBtnMile.Text = "Miles"
        Me.RadBtnMile.UseVisualStyleBackColor = True
        '
        'RadBtnFoot
        '
        Me.RadBtnFoot.AutoSize = True
        Me.RadBtnFoot.Location = New System.Drawing.Point(112, 29)
        Me.RadBtnFoot.Name = "RadBtnFoot"
        Me.RadBtnFoot.Size = New System.Drawing.Size(53, 20)
        Me.RadBtnFoot.TabIndex = 13
        Me.RadBtnFoot.Text = "Feet"
        Me.RadBtnFoot.UseVisualStyleBackColor = True
        '
        'RadBtnMeter
        '
        Me.RadBtnMeter.AutoSize = True
        Me.RadBtnMeter.Checked = True
        Me.RadBtnMeter.Location = New System.Drawing.Point(42, 29)
        Me.RadBtnMeter.Name = "RadBtnMeter"
        Me.RadBtnMeter.Size = New System.Drawing.Size(67, 20)
        Me.RadBtnMeter.TabIndex = 12
        Me.RadBtnMeter.TabStop = True
        Me.RadBtnMeter.Text = "Meters" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.RadBtnMeter.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(168, 7)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(49, 16)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "Y Size:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(7, 7)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(48, 16)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "X Size:"
        '
        'TxtYSize
        '
        Me.TxtYSize.Location = New System.Drawing.Point(223, 4)
        Me.TxtYSize.Name = "TxtYSize"
        Me.TxtYSize.Size = New System.Drawing.Size(95, 22)
        Me.TxtYSize.TabIndex = 9
        '
        'TxtXSize
        '
        Me.TxtXSize.Location = New System.Drawing.Point(61, 4)
        Me.TxtXSize.Name = "TxtXSize"
        Me.TxtXSize.Size = New System.Drawing.Size(95, 22)
        Me.TxtXSize.TabIndex = 8
        '
        'TxtRuleID
        '
        Me.TxtRuleID.Location = New System.Drawing.Point(12, 258)
        Me.TxtRuleID.Name = "TxtRuleID"
        Me.TxtRuleID.Size = New System.Drawing.Size(46, 22)
        Me.TxtRuleID.TabIndex = 11
        Me.TxtRuleID.Visible = False
        '
        'BtnTest
        '
        Me.BtnTest.Location = New System.Drawing.Point(30, 259)
        Me.BtnTest.Name = "BtnTest"
        Me.BtnTest.Size = New System.Drawing.Size(85, 28)
        Me.BtnTest.TabIndex = 12
        Me.BtnTest.Text = "Test Rule"
        Me.BtnTest.UseVisualStyleBackColor = True
        Me.BtnTest.Visible = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(12, 13)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(249, 16)
        Me.Label8.TabIndex = 13
        Me.Label8.Text = "Generate DAFlow Type Grid Zones"
        '
        'BtnAbout
        '
        Me.BtnAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAbout.Location = New System.Drawing.Point(267, 7)
        Me.BtnAbout.Name = "BtnAbout"
        Me.BtnAbout.Size = New System.Drawing.Size(105, 28)
        Me.BtnAbout.TabIndex = 50
        Me.BtnAbout.Text = "Tell me more"
        Me.BtnAbout.UseVisualStyleBackColor = True
        '
        'FrmDAFlowRule
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(381, 299)
        Me.Controls.Add(Me.BtnAbout)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.BtnTest)
        Me.Controls.Add(Me.TxtRuleID)
        Me.Controls.Add(Me.PanXYSize)
        Me.Controls.Add(Me.PanRowCol)
        Me.Controls.Add(Me.PanHRUNumber)
        Me.Controls.Add(Me.RadBtnByHRUDimension)
        Me.Controls.Add(Me.BtnApply)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.RadBtnByRowCol)
        Me.Controls.Add(Me.RadBtnByHRUNo)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "FrmDAFlowRule"
        Me.Text = "DAFlow-Type Zones"
        Me.PanHRUNumber.ResumeLayout(False)
        Me.PanHRUNumber.PerformLayout()
        Me.PanRowCol.ResumeLayout(False)
        Me.PanRowCol.PerformLayout()
        Me.PanXYSize.ResumeLayout(False)
        Me.PanXYSize.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents RadBtnByHRUNo As System.Windows.Forms.RadioButton
    Friend WithEvents RadBtnByRowCol As System.Windows.Forms.RadioButton
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents BtnApply As System.Windows.Forms.Button
    Friend WithEvents RadBtnByHRUDimension As System.Windows.Forms.RadioButton
    Friend WithEvents PanHRUNumber As System.Windows.Forms.Panel
    Friend WithEvents PanRowCol As System.Windows.Forms.Panel
    Friend WithEvents PanXYSize As System.Windows.Forms.Panel
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TxtHRUNumber As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TxtRow As System.Windows.Forms.TextBox
    Friend WithEvents TxtCol As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents RadBtnMile As System.Windows.Forms.RadioButton
    Friend WithEvents RadBtnFoot As System.Windows.Forms.RadioButton
    Friend WithEvents RadBtnMeter As System.Windows.Forms.RadioButton
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TxtYSize As System.Windows.Forms.TextBox
    Friend WithEvents TxtXSize As System.Windows.Forms.TextBox
    Friend WithEvents RadBtnUnknown As System.Windows.Forms.RadioButton
    Friend WithEvents TxtRuleID As System.Windows.Forms.TextBox
    Friend WithEvents BtnTest As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents BtnAbout As System.Windows.Forms.Button
End Class