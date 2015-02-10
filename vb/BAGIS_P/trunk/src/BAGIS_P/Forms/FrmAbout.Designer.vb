<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmAbout
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
        Me.LblVersionText = New System.Windows.Forms.Label()
        Me.LblAbout = New System.Windows.Forms.Label()
        Me.BtnDisclaimers = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'LblVersionText
        '
        Me.LblVersionText.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblVersionText.Location = New System.Drawing.Point(90, 9)
        Me.LblVersionText.Name = "LblVersionText"
        Me.LblVersionText.Size = New System.Drawing.Size(285, 20)
        Me.LblVersionText.TabIndex = 0
        Me.LblVersionText.Text = "Version Text"
        Me.LblVersionText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblAbout
        '
        Me.LblAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAbout.Location = New System.Drawing.Point(24, 40)
        Me.LblAbout.Name = "LblAbout"
        Me.LblAbout.Size = New System.Drawing.Size(450, 245)
        Me.LblAbout.TabIndex = 1
        Me.LblAbout.Text = "About"
        Me.LblAbout.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'BtnDisclaimers
        '
        Me.BtnDisclaimers.Location = New System.Drawing.Point(391, 6)
        Me.BtnDisclaimers.Name = "BtnDisclaimers"
        Me.BtnDisclaimers.Size = New System.Drawing.Size(99, 23)
        Me.BtnDisclaimers.TabIndex = 2
        Me.BtnDisclaimers.Text = "DISCLAIMERS"
        Me.BtnDisclaimers.UseVisualStyleBackColor = True
        '
        'FrmAbout
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(502, 297)
        Me.Controls.Add(Me.BtnDisclaimers)
        Me.Controls.Add(Me.LblAbout)
        Me.Controls.Add(Me.LblVersionText)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "FrmAbout"
        Me.ShowIcon = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "About BAGIS-P"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LblVersionText As System.Windows.Forms.Label
    Friend WithEvents LblAbout As System.Windows.Forms.Label
    Friend WithEvents BtnDisclaimers As System.Windows.Forms.Button
End Class