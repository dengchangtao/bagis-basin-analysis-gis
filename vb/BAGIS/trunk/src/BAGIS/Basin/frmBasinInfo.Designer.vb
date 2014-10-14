<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBasinInfo
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtMinElev = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtMaxElev = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtArea = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtAreaAcre = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtAreaSQMile = New System.Windows.Forms.TextBox()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(14, 17)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(119, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Lowest Elevation:"
        '
        'txtMinElev
        '
        Me.txtMinElev.BackColor = System.Drawing.SystemColors.MenuBar
        Me.txtMinElev.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtMinElev.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMinElev.ForeColor = System.Drawing.Color.Blue
        Me.txtMinElev.Location = New System.Drawing.Point(143, 19)
        Me.txtMinElev.Name = "txtMinElev"
        Me.txtMinElev.ReadOnly = True
        Me.txtMinElev.Size = New System.Drawing.Size(135, 14)
        Me.txtMinElev.TabIndex = 1
        Me.txtMinElev.Text = "0"
        Me.txtMinElev.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(296, 16)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(44, 15)
        Me.Label9.TabIndex = 2
        Me.Label9.Text = "Meter"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(13, 43)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(123, 15)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Highest Elevation:"
        '
        'txtMaxElev
        '
        Me.txtMaxElev.BackColor = System.Drawing.SystemColors.Menu
        Me.txtMaxElev.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtMaxElev.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMaxElev.ForeColor = System.Drawing.Color.Blue
        Me.txtMaxElev.Location = New System.Drawing.Point(143, 44)
        Me.txtMaxElev.Name = "txtMaxElev"
        Me.txtMaxElev.ReadOnly = True
        Me.txtMaxElev.Size = New System.Drawing.Size(135, 14)
        Me.txtMaxElev.TabIndex = 1
        Me.txtMaxElev.Text = "0"
        Me.txtMaxElev.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(296, 43)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(44, 15)
        Me.Label10.TabIndex = 2
        Me.Label10.Text = "Meter"
        '
        'txtArea
        '
        Me.txtArea.BackColor = System.Drawing.SystemColors.Menu
        Me.txtArea.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtArea.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtArea.ForeColor = System.Drawing.Color.Blue
        Me.txtArea.Location = New System.Drawing.Point(25, 18)
        Me.txtArea.Name = "txtArea"
        Me.txtArea.ReadOnly = True
        Me.txtArea.Size = New System.Drawing.Size(131, 15)
        Me.txtArea.TabIndex = 3
        Me.txtArea.Text = "0"
        Me.txtArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(185, 18)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(78, 15)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Square Km"
        '
        'txtAreaAcre
        '
        Me.txtAreaAcre.BackColor = System.Drawing.SystemColors.Menu
        Me.txtAreaAcre.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtAreaAcre.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAreaAcre.ForeColor = System.Drawing.Color.Blue
        Me.txtAreaAcre.Location = New System.Drawing.Point(25, 43)
        Me.txtAreaAcre.Name = "txtAreaAcre"
        Me.txtAreaAcre.ReadOnly = True
        Me.txtAreaAcre.Size = New System.Drawing.Size(131, 15)
        Me.txtAreaAcre.TabIndex = 3
        Me.txtAreaAcre.Text = "0"
        Me.txtAreaAcre.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(185, 43)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(35, 15)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Acre"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(185, 68)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(85, 15)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "Square Mile"
        '
        'txtAreaSQMile
        '
        Me.txtAreaSQMile.BackColor = System.Drawing.SystemColors.Menu
        Me.txtAreaSQMile.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtAreaSQMile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAreaSQMile.ForeColor = System.Drawing.Color.Blue
        Me.txtAreaSQMile.Location = New System.Drawing.Point(25, 68)
        Me.txtAreaSQMile.Name = "txtAreaSQMile"
        Me.txtAreaSQMile.ReadOnly = True
        Me.txtAreaSQMile.Size = New System.Drawing.Size(131, 15)
        Me.txtAreaSQMile.TabIndex = 3
        Me.txtAreaSQMile.Text = "0"
        Me.txtAreaSQMile.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cmdOK
        '
        Me.cmdOK.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOK.Location = New System.Drawing.Point(112, 163)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(115, 29)
        Me.cmdOK.TabIndex = 4
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtAreaSQMile)
        Me.GroupBox1.Controls.Add(Me.txtAreaAcre)
        Me.GroupBox1.Controls.Add(Me.txtArea)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(17, 68)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(322, 94)
        Me.GroupBox1.TabIndex = 5
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Area"
        '
        'frmBasinInfo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(351, 201)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.txtMaxElev)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtMinElev)
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmBasinInfo"
        Me.ShowIcon = False
        Me.Text = "Basin Information"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtMinElev As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtMaxElev As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtArea As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtAreaAcre As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtAreaSQMile As System.Windows.Forms.TextBox
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
End Class
