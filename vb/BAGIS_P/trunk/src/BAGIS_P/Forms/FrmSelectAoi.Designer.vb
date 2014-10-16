<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmSelectAoi
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
        Me.BtnAddAoi = New System.Windows.Forms.Button()
        Me.LstAoi = New System.Windows.Forms.ListBox()
        Me.LblText = New System.Windows.Forms.Label()
        Me.BtnApply = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.BtnRemove = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'BtnAddAoi
        '
        Me.BtnAddAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAddAoi.Location = New System.Drawing.Point(9, 9)
        Me.BtnAddAoi.Name = "BtnAddAoi"
        Me.BtnAddAoi.Size = New System.Drawing.Size(105, 28)
        Me.BtnAddAoi.TabIndex = 57
        Me.BtnAddAoi.Text = "Add AOI"
        Me.BtnAddAoi.UseVisualStyleBackColor = True
        '
        'LstAoi
        '
        Me.LstAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstAoi.FormattingEnabled = True
        Me.LstAoi.ItemHeight = 16
        Me.LstAoi.Location = New System.Drawing.Point(9, 50)
        Me.LstAoi.Name = "LstAoi"
        Me.LstAoi.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.LstAoi.Size = New System.Drawing.Size(546, 196)
        Me.LstAoi.TabIndex = 60
        '
        'LblText
        '
        Me.LblText.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblText.Location = New System.Drawing.Point(124, 14)
        Me.LblText.MaximumSize = New System.Drawing.Size(450, 15)
        Me.LblText.Name = "LblText"
        Me.LblText.Size = New System.Drawing.Size(426, 15)
        Me.LblText.TabIndex = 61
        Me.LblText.Text = "LblText"
        '
        'BtnApply
        '
        Me.BtnApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnApply.Location = New System.Drawing.Point(491, 255)
        Me.BtnApply.Name = "BtnApply"
        Me.BtnApply.Size = New System.Drawing.Size(64, 25)
        Me.BtnApply.TabIndex = 63
        Me.BtnApply.Text = "Apply"
        Me.BtnApply.UseVisualStyleBackColor = True
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(421, 255)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(64, 25)
        Me.BtnCancel.TabIndex = 62
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'BtnRemove
        '
        Me.BtnRemove.Enabled = False
        Me.BtnRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnRemove.Location = New System.Drawing.Point(345, 255)
        Me.BtnRemove.Name = "BtnRemove"
        Me.BtnRemove.Size = New System.Drawing.Size(70, 25)
        Me.BtnRemove.TabIndex = 64
        Me.BtnRemove.Text = "Remove"
        Me.BtnRemove.UseVisualStyleBackColor = True
        '
        'FrmSelectAoi
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(567, 286)
        Me.Controls.Add(Me.BtnRemove)
        Me.Controls.Add(Me.BtnApply)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.LblText)
        Me.Controls.Add(Me.LstAoi)
        Me.Controls.Add(Me.BtnAddAoi)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "FrmSelectAoi"
        Me.ShowIcon = False
        Me.Text = "Add Aoi(s)"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BtnAddAoi As System.Windows.Forms.Button
    Friend WithEvents LstAoi As System.Windows.Forms.ListBox
    Friend WithEvents LblText As System.Windows.Forms.Label
    Friend WithEvents BtnApply As System.Windows.Forms.Button
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents BtnRemove As System.Windows.Forms.Button
End Class
