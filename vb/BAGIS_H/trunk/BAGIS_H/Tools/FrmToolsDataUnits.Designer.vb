<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmToolsDataUnits
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
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.BtnApply = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.CboElevationUnits = New System.Windows.Forms.ComboBox()
        Me.LblElevationUnits = New System.Windows.Forms.Label()
        Me.CboSlopeUnits = New System.Windows.Forms.ComboBox()
        Me.LblSlopeUnits = New System.Windows.Forms.Label()
        Me.CboDepthUnits = New System.Windows.Forms.ComboBox()
        Me.LblDepthUnits = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(138, 17)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(63, 16)
        Me.Label5.TabIndex = 67
        Me.Label5.Text = "AOI Path:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TxtAoiPath
        '
        Me.TxtAoiPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtAoiPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtAoiPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtAoiPath.Location = New System.Drawing.Point(214, 15)
        Me.TxtAoiPath.Name = "TxtAoiPath"
        Me.TxtAoiPath.ReadOnly = True
        Me.TxtAoiPath.Size = New System.Drawing.Size(403, 22)
        Me.TxtAoiPath.TabIndex = 66
        Me.TxtAoiPath.TabStop = False
        '
        'BtnAOI
        '
        Me.BtnAOI.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAOI.Location = New System.Drawing.Point(7, 11)
        Me.BtnAOI.Name = "BtnAOI"
        Me.BtnAOI.Size = New System.Drawing.Size(119, 30)
        Me.BtnAOI.TabIndex = 65
        Me.BtnAOI.Text = "Select AOI"
        Me.BtnAOI.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.SystemColors.Menu
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(8, 47)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(707, 27)
        Me.TextBox1.TabIndex = 77
        Me.TextBox1.Text = "Important: If the units are incorrect, the output of BAGIS-H analysis may also be" & _
    " incorrect."
        '
        'BtnApply
        '
        Me.BtnApply.Enabled = False
        Me.BtnApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnApply.Location = New System.Drawing.Point(553, 169)
        Me.BtnApply.Name = "BtnApply"
        Me.BtnApply.Size = New System.Drawing.Size(64, 25)
        Me.BtnApply.TabIndex = 85
        Me.BtnApply.Text = "Apply"
        Me.BtnApply.UseVisualStyleBackColor = True
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(477, 169)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(64, 25)
        Me.BtnCancel.TabIndex = 84
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'CboElevationUnits
        '
        Me.CboElevationUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboElevationUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboElevationUnits.FormattingEnabled = True
        Me.CboElevationUnits.Location = New System.Drawing.Point(149, 67)
        Me.CboElevationUnits.Name = "CboElevationUnits"
        Me.CboElevationUnits.Size = New System.Drawing.Size(121, 24)
        Me.CboElevationUnits.TabIndex = 83
        '
        'LblElevationUnits
        '
        Me.LblElevationUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblElevationUnits.Location = New System.Drawing.Point(5, 70)
        Me.LblElevationUnits.Name = "LblElevationUnits"
        Me.LblElevationUnits.Size = New System.Drawing.Size(120, 16)
        Me.LblElevationUnits.TabIndex = 82
        Me.LblElevationUnits.Text = "Elevation units:"
        '
        'CboSlopeUnits
        '
        Me.CboSlopeUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboSlopeUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboSlopeUnits.FormattingEnabled = True
        Me.CboSlopeUnits.Location = New System.Drawing.Point(149, 97)
        Me.CboSlopeUnits.Name = "CboSlopeUnits"
        Me.CboSlopeUnits.Size = New System.Drawing.Size(121, 24)
        Me.CboSlopeUnits.TabIndex = 81
        '
        'LblSlopeUnits
        '
        Me.LblSlopeUnits.AutoSize = True
        Me.LblSlopeUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblSlopeUnits.Location = New System.Drawing.Point(5, 100)
        Me.LblSlopeUnits.Name = "LblSlopeUnits"
        Me.LblSlopeUnits.Size = New System.Drawing.Size(89, 16)
        Me.LblSlopeUnits.TabIndex = 80
        Me.LblSlopeUnits.Text = "Slope units:"
        '
        'CboDepthUnits
        '
        Me.CboDepthUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboDepthUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboDepthUnits.FormattingEnabled = True
        Me.CboDepthUnits.Location = New System.Drawing.Point(149, 128)
        Me.CboDepthUnits.Name = "CboDepthUnits"
        Me.CboDepthUnits.Size = New System.Drawing.Size(121, 24)
        Me.CboDepthUnits.TabIndex = 79
        '
        'LblDepthUnits
        '
        Me.LblDepthUnits.AutoSize = True
        Me.LblDepthUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblDepthUnits.Location = New System.Drawing.Point(5, 131)
        Me.LblDepthUnits.Name = "LblDepthUnits"
        Me.LblDepthUnits.Size = New System.Drawing.Size(89, 16)
        Me.LblDepthUnits.TabIndex = 78
        Me.LblDepthUnits.Text = "Depth units:"
        '
        'FrmToolsDataUnits
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(627, 206)
        Me.Controls.Add(Me.BtnApply)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.CboElevationUnits)
        Me.Controls.Add(Me.LblElevationUnits)
        Me.Controls.Add(Me.CboSlopeUnits)
        Me.Controls.Add(Me.LblSlopeUnits)
        Me.Controls.Add(Me.CboDepthUnits)
        Me.Controls.Add(Me.LblDepthUnits)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TxtAoiPath)
        Me.Controls.Add(Me.BtnAOI)
        Me.Name = "FrmToolsDataUnits"
        Me.ShowIcon = False
        Me.Text = "Data Units"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TxtAoiPath As System.Windows.Forms.TextBox
    Friend WithEvents BtnAOI As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents BtnApply As System.Windows.Forms.Button
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents CboElevationUnits As System.Windows.Forms.ComboBox
    Friend WithEvents LblElevationUnits As System.Windows.Forms.Label
    Friend WithEvents CboSlopeUnits As System.Windows.Forms.ComboBox
    Friend WithEvents LblSlopeUnits As System.Windows.Forms.Label
    Friend WithEvents CboDepthUnits As System.Windows.Forms.ComboBox
    Friend WithEvents LblDepthUnits As System.Windows.Forms.Label
End Class
