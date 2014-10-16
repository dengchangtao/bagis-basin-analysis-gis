<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmLandUseTemplate
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
        Me.TxtRuleId = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.TxtFilterWidth = New System.Windows.Forms.TextBox
        Me.TxtFilterHeight = New System.Windows.Forms.TextBox
        Me.TxtIterations = New System.Windows.Forms.TextBox
        Me.BtnApply = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.GrdLandUse = New System.Windows.Forms.DataGridView
        Me.OldValues = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.OldDescr = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.NewValues = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.NewDescr = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.LblRasterLayers = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.CboOptions = New System.Windows.Forms.ComboBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.PnlGrid = New System.Windows.Forms.Panel
        Me.PnlButtons = New System.Windows.Forms.Panel
        Me.BtnDeleteTemplate = New System.Windows.Forms.Button
        Me.BtnCancel = New System.Windows.Forms.Button
        Me.TxtRasterLayer = New System.Windows.Forms.TextBox
        Me.TxtReclassField = New System.Windows.Forms.TextBox
        Me.CkDefault = New System.Windows.Forms.CheckBox
        Me.BtnSelectLyr = New System.Windows.Forms.Button
        Me.TxtMissingValue = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.BtnOldValues = New System.Windows.Forms.Button
        Me.BtnClear = New System.Windows.Forms.Button
        Me.LblToolTip = New System.Windows.Forms.Label
        Me.BtnAbout = New System.Windows.Forms.Button
        Me.PnlOptions = New System.Windows.Forms.Panel
        Me.TxtTemplatesFile = New System.Windows.Forms.TextBox
        Me.LblTemplatesFile = New System.Windows.Forms.Label
        CType(Me.GrdLandUse, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlGrid.SuspendLayout()
        Me.PnlButtons.SuspendLayout()
        Me.PnlOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'TxtRuleId
        '
        Me.TxtRuleId.Enabled = False
        Me.TxtRuleId.Location = New System.Drawing.Point(273, 205)
        Me.TxtRuleId.Name = "TxtRuleId"
        Me.TxtRuleId.Size = New System.Drawing.Size(45, 20)
        Me.TxtRuleId.TabIndex = 27
        Me.TxtRuleId.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(0, 179)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(86, 16)
        Me.Label2.TabIndex = 28
        Me.Label2.Text = "Filter width:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(166, 178)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(57, 16)
        Me.Label3.TabIndex = 29
        Me.Label3.Text = "Height:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(301, 178)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(76, 16)
        Me.Label4.TabIndex = 30
        Me.Label4.Text = "Iterations:"
        '
        'TxtFilterWidth
        '
        Me.TxtFilterWidth.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtFilterWidth.Location = New System.Drawing.Point(80, 176)
        Me.TxtFilterWidth.Name = "TxtFilterWidth"
        Me.TxtFilterWidth.Size = New System.Drawing.Size(80, 22)
        Me.TxtFilterWidth.TabIndex = 31
        '
        'TxtFilterHeight
        '
        Me.TxtFilterHeight.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtFilterHeight.Location = New System.Drawing.Point(217, 176)
        Me.TxtFilterHeight.Name = "TxtFilterHeight"
        Me.TxtFilterHeight.Size = New System.Drawing.Size(80, 22)
        Me.TxtFilterHeight.TabIndex = 32
        '
        'TxtIterations
        '
        Me.TxtIterations.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtIterations.Location = New System.Drawing.Point(371, 175)
        Me.TxtIterations.Name = "TxtIterations"
        Me.TxtIterations.Size = New System.Drawing.Size(80, 22)
        Me.TxtIterations.TabIndex = 33
        '
        'BtnApply
        '
        Me.BtnApply.Enabled = False
        Me.BtnApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnApply.Location = New System.Drawing.Point(145, 6)
        Me.BtnApply.Name = "BtnApply"
        Me.BtnApply.Size = New System.Drawing.Size(60, 25)
        Me.BtnApply.TabIndex = 34
        Me.BtnApply.Text = "Apply"
        Me.BtnApply.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Red
        Me.Label5.Location = New System.Drawing.Point(0, 201)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(267, 16)
        Me.Label5.TabIndex = 36
        Me.Label5.Text = "Set filter iterations to 0 to skip filtering"
        '
        'GrdLandUse
        '
        Me.GrdLandUse.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GrdLandUse.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.OldValues, Me.OldDescr, Me.NewValues, Me.NewDescr})
        Me.GrdLandUse.Location = New System.Drawing.Point(1, 4)
        Me.GrdLandUse.Name = "GrdLandUse"
        Me.GrdLandUse.Size = New System.Drawing.Size(524, 165)
        Me.GrdLandUse.TabIndex = 37
        '
        'OldValues
        '
        Me.OldValues.FillWeight = 60.0!
        Me.OldValues.HeaderText = "Old Value"
        Me.OldValues.Name = "OldValues"
        Me.OldValues.Width = 80
        '
        'OldDescr
        '
        Me.OldDescr.FillWeight = 140.0!
        Me.OldDescr.HeaderText = "Description"
        Me.OldDescr.Name = "OldDescr"
        Me.OldDescr.Width = 150
        '
        'NewValues
        '
        Me.NewValues.FillWeight = 60.0!
        Me.NewValues.HeaderText = "New Value"
        Me.NewValues.Name = "NewValues"
        Me.NewValues.Width = 85
        '
        'NewDescr
        '
        Me.NewDescr.FillWeight = 140.0!
        Me.NewDescr.HeaderText = "Description"
        Me.NewDescr.Name = "NewDescr"
        Me.NewDescr.Width = 150
        '
        'LblRasterLayers
        '
        Me.LblRasterLayers.AutoSize = True
        Me.LblRasterLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRasterLayers.Location = New System.Drawing.Point(127, 4)
        Me.LblRasterLayers.Name = "LblRasterLayers"
        Me.LblRasterLayers.Size = New System.Drawing.Size(113, 16)
        Me.LblRasterLayers.TabIndex = 39
        Me.LblRasterLayers.Text = "Selected layer:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(256, 4)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(103, 16)
        Me.Label9.TabIndex = 40
        Me.Label9.Text = "Reclass field:"
        '
        'CboOptions
        '
        Me.CboOptions.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboOptions.FormattingEnabled = True
        Me.CboOptions.Location = New System.Drawing.Point(4, 23)
        Me.CboOptions.Name = "CboOptions"
        Me.CboOptions.Size = New System.Drawing.Size(110, 24)
        Me.CboOptions.TabIndex = 54
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(1, 4)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(65, 16)
        Me.Label13.TabIndex = 53
        Me.Label13.Text = "Options:"
        '
        'PnlGrid
        '
        Me.PnlGrid.Controls.Add(Me.PnlButtons)
        Me.PnlGrid.Controls.Add(Me.GrdLandUse)
        Me.PnlGrid.Controls.Add(Me.Label2)
        Me.PnlGrid.Controls.Add(Me.Label3)
        Me.PnlGrid.Controls.Add(Me.Label4)
        Me.PnlGrid.Controls.Add(Me.TxtFilterWidth)
        Me.PnlGrid.Controls.Add(Me.TxtFilterHeight)
        Me.PnlGrid.Controls.Add(Me.TxtRuleId)
        Me.PnlGrid.Controls.Add(Me.TxtIterations)
        Me.PnlGrid.Controls.Add(Me.Label5)
        Me.PnlGrid.Location = New System.Drawing.Point(15, 179)
        Me.PnlGrid.Name = "PnlGrid"
        Me.PnlGrid.Size = New System.Drawing.Size(537, 235)
        Me.PnlGrid.TabIndex = 56
        '
        'PnlButtons
        '
        Me.PnlButtons.Controls.Add(Me.BtnDeleteTemplate)
        Me.PnlButtons.Controls.Add(Me.BtnApply)
        Me.PnlButtons.Controls.Add(Me.BtnCancel)
        Me.PnlButtons.Location = New System.Drawing.Point(315, 199)
        Me.PnlButtons.Name = "PnlButtons"
        Me.PnlButtons.Size = New System.Drawing.Size(212, 35)
        Me.PnlButtons.TabIndex = 55
        '
        'BtnDeleteTemplate
        '
        Me.BtnDeleteTemplate.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnDeleteTemplate.Location = New System.Drawing.Point(5, 7)
        Me.BtnDeleteTemplate.Name = "BtnDeleteTemplate"
        Me.BtnDeleteTemplate.Size = New System.Drawing.Size(60, 25)
        Me.BtnDeleteTemplate.TabIndex = 36
        Me.BtnDeleteTemplate.Text = "Delete"
        Me.BtnDeleteTemplate.UseVisualStyleBackColor = True
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(75, 6)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(60, 25)
        Me.BtnCancel.TabIndex = 35
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'TxtRasterLayer
        '
        Me.TxtRasterLayer.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtRasterLayer.Location = New System.Drawing.Point(130, 25)
        Me.TxtRasterLayer.Name = "TxtRasterLayer"
        Me.TxtRasterLayer.ReadOnly = True
        Me.TxtRasterLayer.Size = New System.Drawing.Size(110, 22)
        Me.TxtRasterLayer.TabIndex = 57
        '
        'TxtReclassField
        '
        Me.TxtReclassField.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtReclassField.Location = New System.Drawing.Point(257, 25)
        Me.TxtReclassField.Name = "TxtReclassField"
        Me.TxtReclassField.ReadOnly = True
        Me.TxtReclassField.Size = New System.Drawing.Size(110, 22)
        Me.TxtReclassField.TabIndex = 58
        '
        'CkDefault
        '
        Me.CkDefault.AutoSize = True
        Me.CkDefault.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CkDefault.Location = New System.Drawing.Point(4, 53)
        Me.CkDefault.Name = "CkDefault"
        Me.CkDefault.Size = New System.Drawing.Size(108, 20)
        Me.CkDefault.TabIndex = 62
        Me.CkDefault.Text = "Set as default"
        Me.CkDefault.UseVisualStyleBackColor = True
        '
        'BtnSelectLyr
        '
        Me.BtnSelectLyr.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSelectLyr.Location = New System.Drawing.Point(4, 79)
        Me.BtnSelectLyr.Name = "BtnSelectLyr"
        Me.BtnSelectLyr.Size = New System.Drawing.Size(100, 25)
        Me.BtnSelectLyr.TabIndex = 63
        Me.BtnSelectLyr.Text = "Select Layer"
        Me.BtnSelectLyr.UseVisualStyleBackColor = True
        '
        'TxtMissingValue
        '
        Me.TxtMissingValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtMissingValue.Location = New System.Drawing.Point(378, 25)
        Me.TxtMissingValue.Name = "TxtMissingValue"
        Me.TxtMissingValue.Size = New System.Drawing.Size(110, 22)
        Me.TxtMissingValue.TabIndex = 65
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(375, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(166, 16)
        Me.Label1.TabIndex = 64
        Me.Label1.Text = "Map missing values to:"
        '
        'BtnOldValues
        '
        Me.BtnOldValues.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnOldValues.Location = New System.Drawing.Point(112, 79)
        Me.BtnOldValues.Name = "BtnOldValues"
        Me.BtnOldValues.Size = New System.Drawing.Size(100, 25)
        Me.BtnOldValues.TabIndex = 66
        Me.BtnOldValues.Text = "Old Values"
        Me.BtnOldValues.UseVisualStyleBackColor = True
        '
        'BtnClear
        '
        Me.BtnClear.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClear.Location = New System.Drawing.Point(222, 79)
        Me.BtnClear.Name = "BtnClear"
        Me.BtnClear.Size = New System.Drawing.Size(100, 25)
        Me.BtnClear.TabIndex = 67
        Me.BtnClear.Text = "Clear"
        Me.BtnClear.UseVisualStyleBackColor = True
        '
        'LblToolTip
        '
        Me.LblToolTip.AutoSize = True
        Me.LblToolTip.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblToolTip.Location = New System.Drawing.Point(15, 13)
        Me.LblToolTip.Name = "LblToolTip"
        Me.LblToolTip.Size = New System.Drawing.Size(345, 16)
        Me.LblToolTip.TabIndex = 69
        Me.LblToolTip.Text = "Use land-use/land cover classes to define zones"
        '
        'BtnAbout
        '
        Me.BtnAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAbout.Location = New System.Drawing.Point(430, 7)
        Me.BtnAbout.Name = "BtnAbout"
        Me.BtnAbout.Size = New System.Drawing.Size(105, 28)
        Me.BtnAbout.TabIndex = 68
        Me.BtnAbout.Text = "Tell me more"
        Me.BtnAbout.UseVisualStyleBackColor = True
        '
        'PnlOptions
        '
        Me.PnlOptions.Controls.Add(Me.Label13)
        Me.PnlOptions.Controls.Add(Me.LblRasterLayers)
        Me.PnlOptions.Controls.Add(Me.Label9)
        Me.PnlOptions.Controls.Add(Me.BtnClear)
        Me.PnlOptions.Controls.Add(Me.CboOptions)
        Me.PnlOptions.Controls.Add(Me.BtnOldValues)
        Me.PnlOptions.Controls.Add(Me.TxtRasterLayer)
        Me.PnlOptions.Controls.Add(Me.TxtMissingValue)
        Me.PnlOptions.Controls.Add(Me.TxtReclassField)
        Me.PnlOptions.Controls.Add(Me.Label1)
        Me.PnlOptions.Controls.Add(Me.CkDefault)
        Me.PnlOptions.Controls.Add(Me.BtnSelectLyr)
        Me.PnlOptions.Location = New System.Drawing.Point(13, 64)
        Me.PnlOptions.Name = "PnlOptions"
        Me.PnlOptions.Size = New System.Drawing.Size(539, 110)
        Me.PnlOptions.TabIndex = 70
        '
        'TxtTemplatesFile
        '
        Me.TxtTemplatesFile.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtTemplatesFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtTemplatesFile.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtTemplatesFile.Location = New System.Drawing.Point(120, 42)
        Me.TxtTemplatesFile.Name = "TxtTemplatesFile"
        Me.TxtTemplatesFile.ReadOnly = True
        Me.TxtTemplatesFile.Size = New System.Drawing.Size(415, 22)
        Me.TxtTemplatesFile.TabIndex = 73
        Me.TxtTemplatesFile.TabStop = False
        '
        'LblTemplatesFile
        '
        Me.LblTemplatesFile.AutoSize = True
        Me.LblTemplatesFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblTemplatesFile.Location = New System.Drawing.Point(14, 45)
        Me.LblTemplatesFile.Name = "LblTemplatesFile"
        Me.LblTemplatesFile.Size = New System.Drawing.Size(111, 16)
        Me.LblTemplatesFile.TabIndex = 72
        Me.LblTemplatesFile.Text = "Templates file:"
        '
        'FrmLandUseTemplate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(554, 416)
        Me.Controls.Add(Me.TxtTemplatesFile)
        Me.Controls.Add(Me.LblTemplatesFile)
        Me.Controls.Add(Me.PnlOptions)
        Me.Controls.Add(Me.LblToolTip)
        Me.Controls.Add(Me.BtnAbout)
        Me.Controls.Add(Me.PnlGrid)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "FrmLandUseTemplate"
        Me.ShowIcon = False
        Me.Text = "Template - Land Use"
        CType(Me.GrdLandUse, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlGrid.ResumeLayout(False)
        Me.PnlGrid.PerformLayout()
        Me.PnlButtons.ResumeLayout(False)
        Me.PnlOptions.ResumeLayout(False)
        Me.PnlOptions.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TxtRuleId As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TxtFilterWidth As System.Windows.Forms.TextBox
    Friend WithEvents TxtFilterHeight As System.Windows.Forms.TextBox
    Friend WithEvents TxtIterations As System.Windows.Forms.TextBox
    Friend WithEvents BtnApply As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents GrdLandUse As System.Windows.Forms.DataGridView
    Friend WithEvents LblRasterLayers As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents CboOptions As System.Windows.Forms.ComboBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents PnlGrid As System.Windows.Forms.Panel
    Friend WithEvents PnlButtons As System.Windows.Forms.Panel
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents TxtRasterLayer As System.Windows.Forms.TextBox
    Friend WithEvents TxtReclassField As System.Windows.Forms.TextBox
    Friend WithEvents CkDefault As System.Windows.Forms.CheckBox
    Friend WithEvents BtnSelectLyr As System.Windows.Forms.Button
    Friend WithEvents OldValues As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents OldDescr As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NewValues As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NewDescr As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TxtMissingValue As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents BtnOldValues As System.Windows.Forms.Button
    Friend WithEvents BtnDeleteTemplate As System.Windows.Forms.Button
    Friend WithEvents BtnClear As System.Windows.Forms.Button
    Friend WithEvents LblToolTip As System.Windows.Forms.Label
    Friend WithEvents BtnAbout As System.Windows.Forms.Button
    Friend WithEvents PnlOptions As System.Windows.Forms.Panel
    Friend WithEvents TxtTemplatesFile As System.Windows.Forms.TextBox
    Friend WithEvents LblTemplatesFile As System.Windows.Forms.Label
End Class