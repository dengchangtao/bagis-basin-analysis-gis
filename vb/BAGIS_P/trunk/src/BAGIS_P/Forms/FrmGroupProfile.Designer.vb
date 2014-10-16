<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmGroupProfile
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
        Me.LstProfiles = New System.Windows.Forms.ListBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.LblMethods = New System.Windows.Forms.Label()
        Me.LstMethods = New System.Windows.Forms.ListBox()
        Me.BtnProfileNew = New System.Windows.Forms.Button()
        Me.BtnProfileDelete = New System.Windows.Forms.Button()
        Me.BtnProfileImport = New System.Windows.Forms.Button()
        Me.BtnMethodDelete = New System.Windows.Forms.Button()
        Me.BtnMethodNew = New System.Windows.Forms.Button()
        Me.TxtProfileName = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TxtNumMethods = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TxtDescription = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TxtSelMethod = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.TxtParamName = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TxtToolboxName = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.TxtModelName = New System.Windows.Forms.TextBox()
        Me.BtnApplyProfile = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.BtnEditProfile = New System.Windows.Forms.Button()
        Me.BtnApply = New System.Windows.Forms.Button()
        Me.BtnEditMethod = New System.Windows.Forms.Button()
        Me.BtnAddMethod = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'LstProfiles
        '
        Me.LstProfiles.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstProfiles.FormattingEnabled = True
        Me.LstProfiles.ItemHeight = 16
        Me.LstProfiles.Location = New System.Drawing.Point(6, 30)
        Me.LstProfiles.Name = "LstProfiles"
        Me.LstProfiles.Size = New System.Drawing.Size(186, 196)
        Me.LstProfiles.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 16)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Profiles"
        '
        'LblMethods
        '
        Me.LblMethods.AutoSize = True
        Me.LblMethods.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblMethods.Location = New System.Drawing.Point(212, 9)
        Me.LblMethods.Name = "LblMethods"
        Me.LblMethods.Size = New System.Drawing.Size(67, 16)
        Me.LblMethods.TabIndex = 3
        Me.LblMethods.Text = "Methods"
        '
        'LstMethods
        '
        Me.LstMethods.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstMethods.FormattingEnabled = True
        Me.LstMethods.ItemHeight = 16
        Me.LstMethods.Location = New System.Drawing.Point(210, 30)
        Me.LstMethods.Name = "LstMethods"
        Me.LstMethods.Size = New System.Drawing.Size(142, 196)
        Me.LstMethods.TabIndex = 2
        '
        'BtnProfileNew
        '
        Me.BtnProfileNew.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnProfileNew.Location = New System.Drawing.Point(8, 235)
        Me.BtnProfileNew.Name = "BtnProfileNew"
        Me.BtnProfileNew.Size = New System.Drawing.Size(75, 25)
        Me.BtnProfileNew.TabIndex = 4
        Me.BtnProfileNew.Text = "New"
        Me.BtnProfileNew.UseVisualStyleBackColor = True
        '
        'BtnProfileDelete
        '
        Me.BtnProfileDelete.Enabled = False
        Me.BtnProfileDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnProfileDelete.Location = New System.Drawing.Point(108, 265)
        Me.BtnProfileDelete.Name = "BtnProfileDelete"
        Me.BtnProfileDelete.Size = New System.Drawing.Size(75, 25)
        Me.BtnProfileDelete.TabIndex = 5
        Me.BtnProfileDelete.Text = "Delete"
        Me.BtnProfileDelete.UseVisualStyleBackColor = True
        '
        'BtnProfileImport
        '
        Me.BtnProfileImport.Enabled = False
        Me.BtnProfileImport.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnProfileImport.Location = New System.Drawing.Point(8, 265)
        Me.BtnProfileImport.Name = "BtnProfileImport"
        Me.BtnProfileImport.Size = New System.Drawing.Size(91, 25)
        Me.BtnProfileImport.TabIndex = 6
        Me.BtnProfileImport.Text = "Import/Copy"
        Me.BtnProfileImport.UseVisualStyleBackColor = True
        '
        'BtnMethodDelete
        '
        Me.BtnMethodDelete.Enabled = False
        Me.BtnMethodDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnMethodDelete.Location = New System.Drawing.Point(210, 265)
        Me.BtnMethodDelete.Name = "BtnMethodDelete"
        Me.BtnMethodDelete.Size = New System.Drawing.Size(75, 25)
        Me.BtnMethodDelete.TabIndex = 8
        Me.BtnMethodDelete.Text = "Delete"
        Me.BtnMethodDelete.UseVisualStyleBackColor = True
        '
        'BtnMethodNew
        '
        Me.BtnMethodNew.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnMethodNew.Location = New System.Drawing.Point(210, 235)
        Me.BtnMethodNew.Name = "BtnMethodNew"
        Me.BtnMethodNew.Size = New System.Drawing.Size(75, 25)
        Me.BtnMethodNew.TabIndex = 7
        Me.BtnMethodNew.Text = "New"
        Me.BtnMethodNew.UseVisualStyleBackColor = True
        '
        'TxtProfileName
        '
        Me.TxtProfileName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtProfileName.Location = New System.Drawing.Point(472, 8)
        Me.TxtProfileName.Name = "TxtProfileName"
        Me.TxtProfileName.ReadOnly = True
        Me.TxtProfileName.Size = New System.Drawing.Size(215, 22)
        Me.TxtProfileName.TabIndex = 9
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(363, 10)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(109, 16)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Current profile:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(363, 35)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(146, 16)
        Me.Label3.TabIndex = 12
        Me.Label3.Text = "Number of methods:"
        '
        'TxtNumMethods
        '
        Me.TxtNumMethods.Location = New System.Drawing.Point(515, 33)
        Me.TxtNumMethods.Name = "TxtNumMethods"
        Me.TxtNumMethods.ReadOnly = True
        Me.TxtNumMethods.Size = New System.Drawing.Size(56, 20)
        Me.TxtNumMethods.TabIndex = 11
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(363, 58)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(91, 16)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "Description:"
        '
        'TxtDescription
        '
        Me.TxtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtDescription.Location = New System.Drawing.Point(472, 58)
        Me.TxtDescription.Multiline = True
        Me.TxtDescription.Name = "TxtDescription"
        Me.TxtDescription.ReadOnly = True
        Me.TxtDescription.Size = New System.Drawing.Size(215, 44)
        Me.TxtDescription.TabIndex = 14
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(363, 112)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(116, 16)
        Me.Label5.TabIndex = 16
        Me.Label5.Text = "Current method:"
        '
        'TxtSelMethod
        '
        Me.TxtSelMethod.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtSelMethod.Location = New System.Drawing.Point(495, 108)
        Me.TxtSelMethod.Name = "TxtSelMethod"
        Me.TxtSelMethod.ReadOnly = True
        Me.TxtSelMethod.Size = New System.Drawing.Size(192, 22)
        Me.TxtSelMethod.TabIndex = 15
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(363, 137)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(126, 16)
        Me.Label6.TabIndex = 18
        Me.Label6.Text = "Parameter name:"
        '
        'TxtParamName
        '
        Me.TxtParamName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtParamName.Location = New System.Drawing.Point(495, 133)
        Me.TxtParamName.Name = "TxtParamName"
        Me.TxtParamName.ReadOnly = True
        Me.TxtParamName.Size = New System.Drawing.Size(192, 22)
        Me.TxtParamName.TabIndex = 17
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(363, 162)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(111, 16)
        Me.Label7.TabIndex = 20
        Me.Label7.Text = "Toolbox name:"
        '
        'TxtToolboxName
        '
        Me.TxtToolboxName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtToolboxName.Location = New System.Drawing.Point(495, 158)
        Me.TxtToolboxName.Name = "TxtToolboxName"
        Me.TxtToolboxName.ReadOnly = True
        Me.TxtToolboxName.Size = New System.Drawing.Size(192, 22)
        Me.TxtToolboxName.TabIndex = 19
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(364, 185)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(97, 16)
        Me.Label8.TabIndex = 22
        Me.Label8.Text = "Model name:"
        '
        'TxtModelName
        '
        Me.TxtModelName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtModelName.Location = New System.Drawing.Point(495, 184)
        Me.TxtModelName.Name = "TxtModelName"
        Me.TxtModelName.ReadOnly = True
        Me.TxtModelName.Size = New System.Drawing.Size(192, 22)
        Me.TxtModelName.TabIndex = 21
        '
        'BtnApplyProfile
        '
        Me.BtnApplyProfile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnApplyProfile.Location = New System.Drawing.Point(416, 265)
        Me.BtnApplyProfile.Name = "BtnApplyProfile"
        Me.BtnApplyProfile.Size = New System.Drawing.Size(131, 25)
        Me.BtnApplyProfile.TabIndex = 23
        Me.BtnApplyProfile.Text = "Apply profile to AOI"
        Me.BtnApplyProfile.UseVisualStyleBackColor = True
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(553, 265)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(64, 25)
        Me.BtnCancel.TabIndex = 24
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'BtnEditProfile
        '
        Me.BtnEditProfile.Enabled = False
        Me.BtnEditProfile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnEditProfile.Location = New System.Drawing.Point(108, 235)
        Me.BtnEditProfile.Name = "BtnEditProfile"
        Me.BtnEditProfile.Size = New System.Drawing.Size(75, 25)
        Me.BtnEditProfile.TabIndex = 25
        Me.BtnEditProfile.Text = "Edit"
        Me.BtnEditProfile.UseVisualStyleBackColor = True
        '
        'BtnApply
        '
        Me.BtnApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnApply.Location = New System.Drawing.Point(623, 265)
        Me.BtnApply.Name = "BtnApply"
        Me.BtnApply.Size = New System.Drawing.Size(64, 25)
        Me.BtnApply.TabIndex = 26
        Me.BtnApply.Text = "Apply"
        Me.BtnApply.UseVisualStyleBackColor = True
        '
        'BtnEditMethod
        '
        Me.BtnEditMethod.Enabled = False
        Me.BtnEditMethod.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnEditMethod.Location = New System.Drawing.Point(595, 212)
        Me.BtnEditMethod.Name = "BtnEditMethod"
        Me.BtnEditMethod.Size = New System.Drawing.Size(92, 25)
        Me.BtnEditMethod.TabIndex = 27
        Me.BtnEditMethod.Text = "Edit Method"
        Me.BtnEditMethod.UseVisualStyleBackColor = True
        '
        'BtnAddMethod
        '
        Me.BtnAddMethod.Enabled = False
        Me.BtnAddMethod.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAddMethod.Location = New System.Drawing.Point(291, 235)
        Me.BtnAddMethod.Name = "BtnAddMethod"
        Me.BtnAddMethod.Size = New System.Drawing.Size(75, 25)
        Me.BtnAddMethod.TabIndex = 28
        Me.BtnAddMethod.Text = "Add"
        Me.BtnAddMethod.UseVisualStyleBackColor = True
        '
        'FrmPublicProfile
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(695, 303)
        Me.Controls.Add(Me.BtnAddMethod)
        Me.Controls.Add(Me.BtnEditMethod)
        Me.Controls.Add(Me.BtnApply)
        Me.Controls.Add(Me.BtnEditProfile)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnApplyProfile)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.TxtModelName)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.TxtToolboxName)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.TxtParamName)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TxtSelMethod)
        Me.Controls.Add(Me.TxtDescription)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TxtNumMethods)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TxtProfileName)
        Me.Controls.Add(Me.BtnMethodDelete)
        Me.Controls.Add(Me.BtnMethodNew)
        Me.Controls.Add(Me.BtnProfileImport)
        Me.Controls.Add(Me.BtnProfileDelete)
        Me.Controls.Add(Me.BtnProfileNew)
        Me.Controls.Add(Me.LblMethods)
        Me.Controls.Add(Me.LstMethods)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LstProfiles)
        Me.Name = "FrmPublicProfile"
        Me.ShowIcon = False
        Me.Text = "Group profile builder"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LstProfiles As System.Windows.Forms.ListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents LblMethods As System.Windows.Forms.Label
    Friend WithEvents LstMethods As System.Windows.Forms.ListBox
    Friend WithEvents BtnProfileNew As System.Windows.Forms.Button
    Friend WithEvents BtnProfileDelete As System.Windows.Forms.Button
    Friend WithEvents BtnProfileImport As System.Windows.Forms.Button
    Friend WithEvents BtnMethodDelete As System.Windows.Forms.Button
    Friend WithEvents BtnMethodNew As System.Windows.Forms.Button
    Friend WithEvents TxtProfileName As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TxtNumMethods As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TxtDescription As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TxtSelMethod As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TxtParamName As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TxtToolboxName As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents TxtModelName As System.Windows.Forms.TextBox
    Friend WithEvents BtnApplyProfile As System.Windows.Forms.Button
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents BtnEditProfile As System.Windows.Forms.Button
    Friend WithEvents BtnApply As System.Windows.Forms.Button
    Friend WithEvents BtnEditMethod As System.Windows.Forms.Button
    Friend WithEvents BtnAddMethod As System.Windows.Forms.Button
End Class
