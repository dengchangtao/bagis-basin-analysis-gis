<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmDataUnits
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
        Me.LblDepthUnits = New System.Windows.Forms.Label()
        Me.CboDepthUnits = New System.Windows.Forms.ComboBox()
        Me.CboSlopeUnits = New System.Windows.Forms.ComboBox()
        Me.LblSlopeUnits = New System.Windows.Forms.Label()
        Me.CboElevationUnits = New System.Windows.Forms.ComboBox()
        Me.LblElevationUnits = New System.Windows.Forms.Label()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.BtnApply = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'LblDepthUnits
        '
        Me.LblDepthUnits.AutoSize = True
        Me.LblDepthUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblDepthUnits.Location = New System.Drawing.Point(21, 163)
        Me.LblDepthUnits.Name = "LblDepthUnits"
        Me.LblDepthUnits.Size = New System.Drawing.Size(89, 16)
        Me.LblDepthUnits.TabIndex = 66
        Me.LblDepthUnits.Text = "Depth units:"
        Me.LblDepthUnits.Visible = False
        '
        'CboDepthUnits
        '
        Me.CboDepthUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboDepthUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboDepthUnits.FormattingEnabled = True
        Me.CboDepthUnits.Location = New System.Drawing.Point(165, 160)
        Me.CboDepthUnits.Name = "CboDepthUnits"
        Me.CboDepthUnits.Size = New System.Drawing.Size(121, 24)
        Me.CboDepthUnits.TabIndex = 67
        Me.CboDepthUnits.Visible = False
        '
        'CboSlopeUnits
        '
        Me.CboSlopeUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboSlopeUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboSlopeUnits.FormattingEnabled = True
        Me.CboSlopeUnits.Location = New System.Drawing.Point(165, 129)
        Me.CboSlopeUnits.Name = "CboSlopeUnits"
        Me.CboSlopeUnits.Size = New System.Drawing.Size(121, 24)
        Me.CboSlopeUnits.TabIndex = 69
        Me.CboSlopeUnits.Visible = False
        '
        'LblSlopeUnits
        '
        Me.LblSlopeUnits.AutoSize = True
        Me.LblSlopeUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblSlopeUnits.Location = New System.Drawing.Point(21, 132)
        Me.LblSlopeUnits.Name = "LblSlopeUnits"
        Me.LblSlopeUnits.Size = New System.Drawing.Size(89, 16)
        Me.LblSlopeUnits.TabIndex = 68
        Me.LblSlopeUnits.Text = "Slope units:"
        Me.LblSlopeUnits.Visible = False
        '
        'CboElevationUnits
        '
        Me.CboElevationUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboElevationUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboElevationUnits.FormattingEnabled = True
        Me.CboElevationUnits.Location = New System.Drawing.Point(165, 99)
        Me.CboElevationUnits.Name = "CboElevationUnits"
        Me.CboElevationUnits.Size = New System.Drawing.Size(121, 24)
        Me.CboElevationUnits.TabIndex = 71
        Me.CboElevationUnits.Visible = False
        '
        'LblElevationUnits
        '
        Me.LblElevationUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblElevationUnits.Location = New System.Drawing.Point(21, 102)
        Me.LblElevationUnits.Name = "LblElevationUnits"
        Me.LblElevationUnits.Size = New System.Drawing.Size(120, 16)
        Me.LblElevationUnits.TabIndex = 70
        Me.LblElevationUnits.Text = "Elevation units:"
        Me.LblElevationUnits.Visible = False
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(172, 197)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(64, 25)
        Me.BtnCancel.TabIndex = 74
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'BtnApply
        '
        Me.BtnApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnApply.Location = New System.Drawing.Point(248, 197)
        Me.BtnApply.Name = "BtnApply"
        Me.BtnApply.Size = New System.Drawing.Size(64, 25)
        Me.BtnApply.TabIndex = 75
        Me.BtnApply.Text = "Apply"
        Me.BtnApply.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.SystemColors.Menu
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(1, 3)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(323, 87)
        Me.TextBox1.TabIndex = 76
        Me.TextBox1.Text = "The following data units are undefined for this " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "AOI. Please select the correct " & _
            "data units." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Important: If the units are incorrect, the output " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "of BAGIS-P ca" & _
            "lculations will also be incorrect."
        '
        'FrmDataUnits
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(323, 232)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.BtnApply)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.CboElevationUnits)
        Me.Controls.Add(Me.LblElevationUnits)
        Me.Controls.Add(Me.CboSlopeUnits)
        Me.Controls.Add(Me.LblSlopeUnits)
        Me.Controls.Add(Me.CboDepthUnits)
        Me.Controls.Add(Me.LblDepthUnits)
        Me.Name = "FrmDataUnits"
        Me.ShowIcon = False
        Me.Text = "Missing data units"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LblDepthUnits As System.Windows.Forms.Label
    Friend WithEvents CboDepthUnits As System.Windows.Forms.ComboBox
    Friend WithEvents CboSlopeUnits As System.Windows.Forms.ComboBox
    Friend WithEvents LblSlopeUnits As System.Windows.Forms.Label
    Friend WithEvents CboElevationUnits As System.Windows.Forms.ComboBox
    Friend WithEvents LblElevationUnits As System.Windows.Forms.Label
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents BtnApply As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
End Class
