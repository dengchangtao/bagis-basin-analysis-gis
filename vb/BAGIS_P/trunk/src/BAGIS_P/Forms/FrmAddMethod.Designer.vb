<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmAddMethod
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
        Me.Label2 = New System.Windows.Forms.Label()
        Me.TxtProfileName = New System.Windows.Forms.TextBox()
        Me.LblMethods = New System.Windows.Forms.Label()
        Me.LstNewMethods = New System.Windows.Forms.ListBox()
        Me.BtnApply = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LstProfileMethods = New System.Windows.Forms.ListBox()
        Me.BtnRemoveMethod = New System.Windows.Forms.Button()
        Me.BtnAddMethod = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TxtMethodDescription = New System.Windows.Forms.TextBox()
        Me.TxtProfileDescription = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TxtSearchFilter = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(0, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(105, 16)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Current profile"
        '
        'TxtProfileName
        '
        Me.TxtProfileName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtProfileName.Location = New System.Drawing.Point(106, 4)
        Me.TxtProfileName.Name = "TxtProfileName"
        Me.TxtProfileName.ReadOnly = True
        Me.TxtProfileName.Size = New System.Drawing.Size(142, 22)
        Me.TxtProfileName.TabIndex = 13
        '
        'LblMethods
        '
        Me.LblMethods.AutoSize = True
        Me.LblMethods.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMethods.Location = New System.Drawing.Point(6, 154)
        Me.LblMethods.Name = "LblMethods"
        Me.LblMethods.Size = New System.Drawing.Size(137, 16)
        Me.LblMethods.TabIndex = 16
        Me.LblMethods.Text = "Available methods"
        '
        'LstNewMethods
        '
        Me.LstNewMethods.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstNewMethods.FormattingEnabled = True
        Me.LstNewMethods.ItemHeight = 16
        Me.LstNewMethods.Location = New System.Drawing.Point(4, 175)
        Me.LstNewMethods.Name = "LstNewMethods"
        Me.LstNewMethods.Size = New System.Drawing.Size(142, 196)
        Me.LstNewMethods.TabIndex = 15
        '
        'BtnApply
        '
        Me.BtnApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnApply.Location = New System.Drawing.Point(363, 455)
        Me.BtnApply.Name = "BtnApply"
        Me.BtnApply.Size = New System.Drawing.Size(64, 25)
        Me.BtnApply.TabIndex = 28
        Me.BtnApply.Text = "Apply"
        Me.BtnApply.UseVisualStyleBackColor = True
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(293, 455)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(64, 25)
        Me.BtnCancel.TabIndex = 27
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(206, 154)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(116, 16)
        Me.Label1.TabIndex = 34
        Me.Label1.Text = "Profile methods"
        '
        'LstProfileMethods
        '
        Me.LstProfileMethods.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstProfileMethods.FormattingEnabled = True
        Me.LstProfileMethods.ItemHeight = 16
        Me.LstProfileMethods.Location = New System.Drawing.Point(204, 175)
        Me.LstProfileMethods.Name = "LstProfileMethods"
        Me.LstProfileMethods.Size = New System.Drawing.Size(142, 196)
        Me.LstProfileMethods.TabIndex = 33
        '
        'BtnRemoveMethod
        '
        Me.BtnRemoveMethod.Image = Global.BAGIS_P.My.Resources.Resources.GenericYellowLeftArrow32
        Me.BtnRemoveMethod.Location = New System.Drawing.Point(157, 216)
        Me.BtnRemoveMethod.Name = "BtnRemoveMethod"
        Me.BtnRemoveMethod.Size = New System.Drawing.Size(39, 31)
        Me.BtnRemoveMethod.TabIndex = 35
        Me.BtnRemoveMethod.UseVisualStyleBackColor = True
        '
        'BtnAddMethod
        '
        Me.BtnAddMethod.Image = Global.BAGIS_P.My.Resources.Resources.GenericYellowRightArrow32
        Me.BtnAddMethod.Location = New System.Drawing.Point(157, 260)
        Me.BtnAddMethod.Name = "BtnAddMethod"
        Me.BtnAddMethod.Size = New System.Drawing.Size(39, 31)
        Me.BtnAddMethod.TabIndex = 36
        Me.BtnAddMethod.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(1, 379)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(140, 16)
        Me.Label4.TabIndex = 31
        Me.Label4.Text = "Method description"
        '
        'TxtMethodDescription
        '
        Me.TxtMethodDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtMethodDescription.Location = New System.Drawing.Point(139, 379)
        Me.TxtMethodDescription.Multiline = True
        Me.TxtMethodDescription.Name = "TxtMethodDescription"
        Me.TxtMethodDescription.ReadOnly = True
        Me.TxtMethodDescription.Size = New System.Drawing.Size(288, 58)
        Me.TxtMethodDescription.TabIndex = 32
        '
        'TxtProfileDescription
        '
        Me.TxtProfileDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtProfileDescription.Location = New System.Drawing.Point(139, 33)
        Me.TxtProfileDescription.Multiline = True
        Me.TxtProfileDescription.Name = "TxtProfileDescription"
        Me.TxtProfileDescription.ReadOnly = True
        Me.TxtProfileDescription.Size = New System.Drawing.Size(288, 58)
        Me.TxtProfileDescription.TabIndex = 38
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(0, 35)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(134, 16)
        Me.Label3.TabIndex = 37
        Me.Label3.Text = "Profile description"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(0, 105)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(91, 16)
        Me.Label5.TabIndex = 40
        Me.Label5.Text = "Search filter"
        '
        'TxtSearchFilter
        '
        Me.TxtSearchFilter.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtSearchFilter.Location = New System.Drawing.Point(94, 102)
        Me.TxtSearchFilter.Name = "TxtSearchFilter"
        Me.TxtSearchFilter.Size = New System.Drawing.Size(333, 22)
        Me.TxtSearchFilter.TabIndex = 39
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(4, 128)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(267, 16)
        Me.Label6.TabIndex = 41
        Me.Label6.Text = "Filters the list of available methods by name" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'FrmAddMethod
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(441, 495)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TxtSearchFilter)
        Me.Controls.Add(Me.TxtProfileDescription)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.BtnAddMethod)
        Me.Controls.Add(Me.BtnRemoveMethod)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LstProfileMethods)
        Me.Controls.Add(Me.TxtMethodDescription)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.BtnApply)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.LblMethods)
        Me.Controls.Add(Me.LstNewMethods)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TxtProfileName)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "FrmAddMethod"
        Me.ShowIcon = False
        Me.Text = "Add method(s) to profile"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TxtProfileName As System.Windows.Forms.TextBox
    Friend WithEvents LblMethods As System.Windows.Forms.Label
    Friend WithEvents LstNewMethods As System.Windows.Forms.ListBox
    Friend WithEvents BtnApply As System.Windows.Forms.Button
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents LstProfileMethods As System.Windows.Forms.ListBox
    Friend WithEvents BtnRemoveMethod As System.Windows.Forms.Button
    Friend WithEvents BtnAddMethod As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TxtMethodDescription As System.Windows.Forms.TextBox
    Friend WithEvents TxtProfileDescription As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TxtSearchFilter As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
End Class
