<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmNewSite
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
        Me.TxtMessage = New System.Windows.Forms.TextBox()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.BtnSave = New System.Windows.Forms.Button()
        Me.TxtSiteName = New System.Windows.Forms.TextBox()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.TxtFCPath = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'TxtMessage
        '
        Me.TxtMessage.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtMessage.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtMessage.Location = New System.Drawing.Point(12, 12)
        Me.TxtMessage.Multiline = True
        Me.TxtMessage.Name = "TxtMessage"
        Me.TxtMessage.ReadOnly = True
        Me.TxtMessage.Size = New System.Drawing.Size(536, 33)
        Me.TxtMessage.TabIndex = 0
        Me.TxtMessage.Text = "BAGIS"
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(376, 134)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(82, 33)
        Me.BtnCancel.TabIndex = 62
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'BtnSave
        '
        Me.BtnSave.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSave.Location = New System.Drawing.Point(466, 134)
        Me.BtnSave.Name = "BtnSave"
        Me.BtnSave.Size = New System.Drawing.Size(82, 33)
        Me.BtnSave.TabIndex = 61
        Me.BtnSave.Text = "Save"
        Me.BtnSave.UseVisualStyleBackColor = True
        '
        'TxtSiteName
        '
        Me.TxtSiteName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtSiteName.Location = New System.Drawing.Point(80, 85)
        Me.TxtSiteName.MaxLength = 50
        Me.TxtSiteName.Name = "TxtSiteName"
        Me.TxtSiteName.Size = New System.Drawing.Size(150, 22)
        Me.TxtSiteName.TabIndex = 0
        '
        'TextBox2
        '
        Me.TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox2.Location = New System.Drawing.Point(12, 87)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.Size = New System.Drawing.Size(70, 15)
        Me.TextBox2.TabIndex = 64
        Me.TextBox2.Text = "Site name:"
        '
        'TextBox1
        '
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(12, 59)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.Size = New System.Drawing.Size(115, 15)
        Me.TextBox1.TabIndex = 65
        Me.TextBox1.Text = "Feature class path:"
        '
        'TxtFCPath
        '
        Me.TxtFCPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtFCPath.Location = New System.Drawing.Point(128, 57)
        Me.TxtFCPath.MaxLength = 50
        Me.TxtFCPath.Name = "TxtFCPath"
        Me.TxtFCPath.ReadOnly = True
        Me.TxtFCPath.Size = New System.Drawing.Size(420, 22)
        Me.TxtFCPath.TabIndex = 66
        '
        'FrmNewSite
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(560, 178)
        Me.Controls.Add(Me.TxtFCPath)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.TextBox2)
        Me.Controls.Add(Me.TxtSiteName)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnSave)
        Me.Controls.Add(Me.TxtMessage)
        Me.Name = "FrmNewSite"
        Me.ShowIcon = False
        Me.Text = "New pseudo site"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TxtMessage As System.Windows.Forms.TextBox
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents BtnSave As System.Windows.Forms.Button
    Friend WithEvents TxtSiteName As System.Windows.Forms.TextBox
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents TxtFCPath As System.Windows.Forms.TextBox
End Class
