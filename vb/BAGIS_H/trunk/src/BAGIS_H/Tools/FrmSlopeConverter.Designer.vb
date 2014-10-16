<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmSlopeConverter
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
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TxtAoiPath = New System.Windows.Forms.TextBox()
        Me.BtnAOI = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtSlopePath = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TxtCurrentSlopeUnits = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.CboSlopeUnits = New System.Windows.Forms.ComboBox()
        Me.TxtRecalcMessage = New System.Windows.Forms.TextBox()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.BtnApply = New System.Windows.Forms.Button()
        Me.CkRecalculate = New System.Windows.Forms.CheckBox()
        Me.LblWeaselSlopePath = New System.Windows.Forms.Label()
        Me.TxtWeaselSlopePath = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(142, 13)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(63, 16)
        Me.Label5.TabIndex = 64
        Me.Label5.Text = "AOI Path:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TxtAoiPath
        '
        Me.TxtAoiPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtAoiPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtAoiPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtAoiPath.Location = New System.Drawing.Point(218, 11)
        Me.TxtAoiPath.Name = "TxtAoiPath"
        Me.TxtAoiPath.ReadOnly = True
        Me.TxtAoiPath.Size = New System.Drawing.Size(500, 22)
        Me.TxtAoiPath.TabIndex = 63
        Me.TxtAoiPath.TabStop = False
        '
        'BtnAOI
        '
        Me.BtnAOI.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAOI.Location = New System.Drawing.Point(11, 7)
        Me.BtnAOI.Name = "BtnAOI"
        Me.BtnAOI.Size = New System.Drawing.Size(119, 30)
        Me.BtnAOI.TabIndex = 62
        Me.BtnAOI.Text = "Select AOI"
        Me.BtnAOI.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(140, 41)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(77, 16)
        Me.Label1.TabIndex = 66
        Me.Label1.Text = "Slope Path:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TxtSlopePath
        '
        Me.TxtSlopePath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtSlopePath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtSlopePath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtSlopePath.Location = New System.Drawing.Point(218, 38)
        Me.TxtSlopePath.Name = "TxtSlopePath"
        Me.TxtSlopePath.ReadOnly = True
        Me.TxtSlopePath.Size = New System.Drawing.Size(500, 22)
        Me.TxtSlopePath.TabIndex = 65
        Me.TxtSlopePath.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(133, 66)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(210, 16)
        Me.Label2.TabIndex = 68
        Me.Label2.Text = "Current Slope Measurement Units:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TxtCurrentSlopeUnits
        '
        Me.TxtCurrentSlopeUnits.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtCurrentSlopeUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtCurrentSlopeUnits.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtCurrentSlopeUnits.Location = New System.Drawing.Point(345, 65)
        Me.TxtCurrentSlopeUnits.Name = "TxtCurrentSlopeUnits"
        Me.TxtCurrentSlopeUnits.ReadOnly = True
        Me.TxtCurrentSlopeUnits.Size = New System.Drawing.Size(77, 22)
        Me.TxtCurrentSlopeUnits.TabIndex = 67
        Me.TxtCurrentSlopeUnits.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(134, 122)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(195, 16)
        Me.Label3.TabIndex = 69
        Me.Label3.Text = "New Slope Measurement Units:"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'CboSlopeUnits
        '
        Me.CboSlopeUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboSlopeUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboSlopeUnits.FormattingEnabled = True
        Me.CboSlopeUnits.Location = New System.Drawing.Point(331, 119)
        Me.CboSlopeUnits.Name = "CboSlopeUnits"
        Me.CboSlopeUnits.Size = New System.Drawing.Size(121, 24)
        Me.CboSlopeUnits.TabIndex = 70
        '
        'TxtRecalcMessage
        '
        Me.TxtRecalcMessage.BackColor = System.Drawing.SystemColors.Menu
        Me.TxtRecalcMessage.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtRecalcMessage.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtRecalcMessage.Location = New System.Drawing.Point(142, 176)
        Me.TxtRecalcMessage.Multiline = True
        Me.TxtRecalcMessage.Name = "TxtRecalcMessage"
        Me.TxtRecalcMessage.Size = New System.Drawing.Size(336, 70)
        Me.TxtRecalcMessage.TabIndex = 77
        Me.TxtRecalcMessage.Text = "Note: The recalculated slope layer will replace the existing slope layer(s)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(507, 235)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(100, 31)
        Me.BtnCancel.TabIndex = 79
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'BtnApply
        '
        Me.BtnApply.Enabled = False
        Me.BtnApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnApply.Location = New System.Drawing.Point(616, 235)
        Me.BtnApply.Name = "BtnApply"
        Me.BtnApply.Size = New System.Drawing.Size(100, 31)
        Me.BtnApply.TabIndex = 78
        Me.BtnApply.Text = "Apply"
        Me.BtnApply.UseVisualStyleBackColor = True
        '
        'CkRecalculate
        '
        Me.CkRecalculate.AutoSize = True
        Me.CkRecalculate.Checked = True
        Me.CkRecalculate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CkRecalculate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CkRecalculate.Location = New System.Drawing.Point(146, 149)
        Me.CkRecalculate.Name = "CkRecalculate"
        Me.CkRecalculate.Size = New System.Drawing.Size(231, 20)
        Me.CkRecalculate.TabIndex = 80
        Me.CkRecalculate.Text = "Recalculate slope layer from DEM"
        Me.CkRecalculate.UseVisualStyleBackColor = True
        '
        'LblWeaselSlopePath
        '
        Me.LblWeaselSlopePath.AutoSize = True
        Me.LblWeaselSlopePath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblWeaselSlopePath.Location = New System.Drawing.Point(140, 95)
        Me.LblWeaselSlopePath.Name = "LblWeaselSlopePath"
        Me.LblWeaselSlopePath.Size = New System.Drawing.Size(127, 16)
        Me.LblWeaselSlopePath.TabIndex = 82
        Me.LblWeaselSlopePath.Text = "Weasel Slope Path:"
        Me.LblWeaselSlopePath.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.LblWeaselSlopePath.Visible = False
        '
        'TxtWeaselSlopePath
        '
        Me.TxtWeaselSlopePath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtWeaselSlopePath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtWeaselSlopePath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtWeaselSlopePath.Location = New System.Drawing.Point(268, 92)
        Me.TxtWeaselSlopePath.Name = "TxtWeaselSlopePath"
        Me.TxtWeaselSlopePath.ReadOnly = True
        Me.TxtWeaselSlopePath.Size = New System.Drawing.Size(450, 22)
        Me.TxtWeaselSlopePath.TabIndex = 81
        Me.TxtWeaselSlopePath.TabStop = False
        Me.TxtWeaselSlopePath.Visible = False
        '
        'FrmSlopeConverter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(728, 273)
        Me.Controls.Add(Me.LblWeaselSlopePath)
        Me.Controls.Add(Me.TxtWeaselSlopePath)
        Me.Controls.Add(Me.CkRecalculate)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnApply)
        Me.Controls.Add(Me.TxtRecalcMessage)
        Me.Controls.Add(Me.CboSlopeUnits)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TxtCurrentSlopeUnits)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TxtSlopePath)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TxtAoiPath)
        Me.Controls.Add(Me.BtnAOI)
        Me.Name = "FrmSlopeConverter"
        Me.ShowIcon = False
        Me.Text = "AOI Slope"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TxtAoiPath As System.Windows.Forms.TextBox
    Friend WithEvents BtnAOI As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TxtSlopePath As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TxtCurrentSlopeUnits As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents CboSlopeUnits As System.Windows.Forms.ComboBox
    Friend WithEvents TxtRecalcMessage As System.Windows.Forms.TextBox
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents BtnApply As System.Windows.Forms.Button
    Friend WithEvents CkRecalculate As System.Windows.Forms.CheckBox
    Friend WithEvents LblWeaselSlopePath As System.Windows.Forms.Label
    Friend WithEvents TxtWeaselSlopePath As System.Windows.Forms.TextBox
End Class
