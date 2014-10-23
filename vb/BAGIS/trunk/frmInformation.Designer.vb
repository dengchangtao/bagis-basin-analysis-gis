<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInformation
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
        Me.txtMessage = New System.Windows.Forms.ListView()
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'txtMessage
        '
        Me.txtMessage.Location = New System.Drawing.Point(0, 0)
        Me.txtMessage.Name = "txtMessage"
        Me.txtMessage.Size = New System.Drawing.Size(467, 277)
        Me.txtMessage.TabIndex = 0
        Me.txtMessage.UseCompatibleStateImageBehavior = False
        '
        'cmdClose
        '
        Me.cmdClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.cmdClose.Location = New System.Drawing.Point(361, 280)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(98, 31)
        Me.cmdClose.TabIndex = 1
        Me.cmdClose.Text = "Close"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'frmInformation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(468, 318)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.txtMessage)
        Me.Name = "frmInformation"
        Me.ShowIcon = False
        Me.Text = "BAGIS Information"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtMessage As System.Windows.Forms.ListView
    Friend WithEvents cmdClose As System.Windows.Forms.Button
End Class
