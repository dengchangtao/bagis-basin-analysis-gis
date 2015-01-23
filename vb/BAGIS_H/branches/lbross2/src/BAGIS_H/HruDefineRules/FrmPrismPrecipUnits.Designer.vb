<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmPrismPrecipUnits
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
        Me.LblDescr = New System.Windows.Forms.Label()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.BtnApply = New System.Windows.Forms.Button()
        Me.RdoMillimeters = New System.Windows.Forms.RadioButton()
        Me.RdoInches = New System.Windows.Forms.RadioButton()
        Me.SuspendLayout()
        '
        'LblDescr
        '
        Me.LblDescr.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblDescr.Location = New System.Drawing.Point(1, 9)
        Me.LblDescr.Name = "LblDescr"
        Me.LblDescr.Size = New System.Drawing.Size(365, 38)
        Me.LblDescr.TabIndex = 2
        Me.LblDescr.Text = "About"
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(214, 77)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(60, 25)
        Me.BtnCancel.TabIndex = 20
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'BtnApply
        '
        Me.BtnApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnApply.Location = New System.Drawing.Point(291, 77)
        Me.BtnApply.Name = "BtnApply"
        Me.BtnApply.Size = New System.Drawing.Size(60, 25)
        Me.BtnApply.TabIndex = 19
        Me.BtnApply.Text = "Apply"
        Me.BtnApply.UseVisualStyleBackColor = True
        '
        'RdoMillimeters
        '
        Me.RdoMillimeters.AutoSize = True
        Me.RdoMillimeters.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RdoMillimeters.Location = New System.Drawing.Point(78, 44)
        Me.RdoMillimeters.Name = "RdoMillimeters"
        Me.RdoMillimeters.Size = New System.Drawing.Size(90, 20)
        Me.RdoMillimeters.TabIndex = 85
        Me.RdoMillimeters.Text = "Millimeters"
        Me.RdoMillimeters.UseVisualStyleBackColor = True
        '
        'RdoInches
        '
        Me.RdoInches.AutoSize = True
        Me.RdoInches.Checked = True
        Me.RdoInches.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RdoInches.Location = New System.Drawing.Point(11, 44)
        Me.RdoInches.Name = "RdoInches"
        Me.RdoInches.Size = New System.Drawing.Size(65, 20)
        Me.RdoInches.TabIndex = 84
        Me.RdoInches.TabStop = True
        Me.RdoInches.Text = "Inches"
        Me.RdoInches.UseVisualStyleBackColor = True
        '
        'FrmPrismPrecipUnits
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(358, 115)
        Me.Controls.Add(Me.RdoMillimeters)
        Me.Controls.Add(Me.RdoInches)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnApply)
        Me.Controls.Add(Me.LblDescr)
        Me.Name = "FrmPrismPrecipUnits"
        Me.ShowIcon = False
        Me.Text = "Define PRISM Precipitation Units"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LblDescr As System.Windows.Forms.Label
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents BtnApply As System.Windows.Forms.Button
    Friend WithEvents RdoMillimeters As System.Windows.Forms.RadioButton
    Friend WithEvents RdoInches As System.Windows.Forms.RadioButton
End Class
