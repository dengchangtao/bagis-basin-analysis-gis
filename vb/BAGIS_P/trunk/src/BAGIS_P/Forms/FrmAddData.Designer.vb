<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmAddData
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
        Me.TxtName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtDescription = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TxtSource = New System.Windows.Forms.TextBox()
        Me.BtnSelectSource = New System.Windows.Forms.Button()
        Me.BtnApply = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.CkUnits = New System.Windows.Forms.CheckBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.CboUnitType = New System.Windows.Forms.ComboBox()
        Me.PnlUnits = New System.Windows.Forms.Panel()
        Me.CboUnits = New System.Windows.Forms.ComboBox()
        Me.PnlUnits.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(2, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(53, 16)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Name:"
        '
        'TxtName
        '
        Me.TxtName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtName.Location = New System.Drawing.Point(95, 7)
        Me.TxtName.Name = "TxtName"
        Me.TxtName.Size = New System.Drawing.Size(288, 22)
        Me.TxtName.TabIndex = 13
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(2, 41)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(91, 16)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Description:"
        '
        'TxtDescription
        '
        Me.TxtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtDescription.Location = New System.Drawing.Point(95, 41)
        Me.TxtDescription.Multiline = True
        Me.TxtDescription.Name = "TxtDescription"
        Me.TxtDescription.Size = New System.Drawing.Size(288, 58)
        Me.TxtDescription.TabIndex = 31
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(2, 112)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(61, 16)
        Me.Label3.TabIndex = 33
        Me.Label3.Text = "Source:"
        '
        'TxtSource
        '
        Me.TxtSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtSource.Location = New System.Drawing.Point(95, 110)
        Me.TxtSource.Name = "TxtSource"
        Me.TxtSource.ReadOnly = True
        Me.TxtSource.Size = New System.Drawing.Size(288, 22)
        Me.TxtSource.TabIndex = 32
        '
        'BtnSelectSource
        '
        Me.BtnSelectSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSelectSource.Location = New System.Drawing.Point(392, 109)
        Me.BtnSelectSource.Name = "BtnSelectSource"
        Me.BtnSelectSource.Size = New System.Drawing.Size(64, 25)
        Me.BtnSelectSource.TabIndex = 34
        Me.BtnSelectSource.Text = "Select"
        Me.BtnSelectSource.UseVisualStyleBackColor = True
        '
        'BtnApply
        '
        Me.BtnApply.Enabled = False
        Me.BtnApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnApply.Location = New System.Drawing.Point(392, 216)
        Me.BtnApply.Name = "BtnApply"
        Me.BtnApply.Size = New System.Drawing.Size(64, 25)
        Me.BtnApply.TabIndex = 36
        Me.BtnApply.Text = "Apply"
        Me.BtnApply.UseVisualStyleBackColor = True
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(322, 216)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(64, 25)
        Me.BtnCancel.TabIndex = 35
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'CkUnits
        '
        Me.CkUnits.AutoSize = True
        Me.CkUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CkUnits.Location = New System.Drawing.Point(4, 138)
        Me.CkUnits.Name = "CkUnits"
        Me.CkUnits.Size = New System.Drawing.Size(251, 20)
        Me.CkUnits.TabIndex = 41
        Me.CkUnits.Text = "Define the unit of the field values"
        Me.CkUnits.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CkUnits.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(1, 4)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(73, 16)
        Me.Label4.TabIndex = 42
        Me.Label4.Text = "Unit type:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(1, 35)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(39, 16)
        Me.Label5.TabIndex = 43
        Me.Label5.Text = "Unit:"
        '
        'CboUnitType
        '
        Me.CboUnitType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboUnitType.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboUnitType.FormattingEnabled = True
        Me.CboUnitType.Location = New System.Drawing.Point(70, 2)
        Me.CboUnitType.Name = "CboUnitType"
        Me.CboUnitType.Size = New System.Drawing.Size(121, 24)
        Me.CboUnitType.TabIndex = 44
        '
        'PnlUnits
        '
        Me.PnlUnits.Controls.Add(Me.CboUnits)
        Me.PnlUnits.Controls.Add(Me.Label5)
        Me.PnlUnits.Controls.Add(Me.CboUnitType)
        Me.PnlUnits.Controls.Add(Me.Label4)
        Me.PnlUnits.Location = New System.Drawing.Point(0, 163)
        Me.PnlUnits.Name = "PnlUnits"
        Me.PnlUnits.Size = New System.Drawing.Size(311, 61)
        Me.PnlUnits.TabIndex = 45
        Me.PnlUnits.Visible = False
        '
        'CboUnits
        '
        Me.CboUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboUnits.FormattingEnabled = True
        Me.CboUnits.Location = New System.Drawing.Point(70, 32)
        Me.CboUnits.Name = "CboUnits"
        Me.CboUnits.Size = New System.Drawing.Size(121, 24)
        Me.CboUnits.TabIndex = 45
        '
        'FrmAddData
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(465, 253)
        Me.Controls.Add(Me.PnlUnits)
        Me.Controls.Add(Me.CkUnits)
        Me.Controls.Add(Me.BtnApply)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnSelectSource)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TxtSource)
        Me.Controls.Add(Me.TxtDescription)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TxtName)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "FrmAddData"
        Me.ShowIcon = False
        Me.Text = "Data Source Editor"
        Me.PnlUnits.ResumeLayout(False)
        Me.PnlUnits.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TxtName As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TxtDescription As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TxtSource As System.Windows.Forms.TextBox
    Friend WithEvents BtnSelectSource As System.Windows.Forms.Button
    Friend WithEvents BtnApply As System.Windows.Forms.Button
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents CkUnits As System.Windows.Forms.CheckBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents CboUnitType As System.Windows.Forms.ComboBox
    Friend WithEvents PnlUnits As System.Windows.Forms.Panel
    Friend WithEvents CboUnits As System.Windows.Forms.ComboBox
End Class
