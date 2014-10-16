<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmUseTemplate
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
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TxtAoiPath = New System.Windows.Forms.TextBox()
        Me.BtnAOI = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtTemplateAoiPath = New System.Windows.Forms.TextBox()
        Me.BtnTemplateAOI = New System.Windows.Forms.Button()
        Me.LblHruLayers = New System.Windows.Forms.Label()
        Me.LstHruLayers = New System.Windows.Forms.ListBox()
        Me.BtnGenerateHru = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.TxtErrorMsg = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(121, 10)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(101, 16)
        Me.Label5.TabIndex = 64
        Me.Label5.Text = "Output AOI Path"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TxtAoiPath
        '
        Me.TxtAoiPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtAoiPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtAoiPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtAoiPath.Location = New System.Drawing.Point(246, 7)
        Me.TxtAoiPath.Name = "TxtAoiPath"
        Me.TxtAoiPath.ReadOnly = True
        Me.TxtAoiPath.Size = New System.Drawing.Size(571, 22)
        Me.TxtAoiPath.TabIndex = 63
        Me.TxtAoiPath.TabStop = False
        Me.TxtAoiPath.Text = "Output AOI is not specified"
        '
        'BtnAOI
        '
        Me.BtnAOI.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAOI.Location = New System.Drawing.Point(2, 3)
        Me.BtnAOI.Name = "BtnAOI"
        Me.BtnAOI.Size = New System.Drawing.Size(119, 30)
        Me.BtnAOI.TabIndex = 62
        Me.BtnAOI.Text = "Select AOI"
        Me.BtnAOI.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(125, 47)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(121, 16)
        Me.Label1.TabIndex = 67
        Me.Label1.Text = "Template AOI Path"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TxtTemplateAoiPath
        '
        Me.TxtTemplateAoiPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtTemplateAoiPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtTemplateAoiPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtTemplateAoiPath.Location = New System.Drawing.Point(246, 45)
        Me.TxtTemplateAoiPath.Name = "TxtTemplateAoiPath"
        Me.TxtTemplateAoiPath.ReadOnly = True
        Me.TxtTemplateAoiPath.Size = New System.Drawing.Size(571, 22)
        Me.TxtTemplateAoiPath.TabIndex = 66
        Me.TxtTemplateAoiPath.TabStop = False
        Me.TxtTemplateAoiPath.Text = "Template AOI is not specified"
        '
        'BtnTemplateAOI
        '
        Me.BtnTemplateAOI.Enabled = False
        Me.BtnTemplateAOI.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnTemplateAOI.Location = New System.Drawing.Point(2, 40)
        Me.BtnTemplateAOI.Name = "BtnTemplateAOI"
        Me.BtnTemplateAOI.Size = New System.Drawing.Size(119, 30)
        Me.BtnTemplateAOI.TabIndex = 65
        Me.BtnTemplateAOI.Text = "Select AOI"
        Me.BtnTemplateAOI.UseVisualStyleBackColor = True
        '
        'LblHruLayers
        '
        Me.LblHruLayers.AutoSize = True
        Me.LblHruLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblHruLayers.Location = New System.Drawing.Point(-1, 87)
        Me.LblHruLayers.Name = "LblHruLayers"
        Me.LblHruLayers.Size = New System.Drawing.Size(166, 16)
        Me.LblHruLayers.TabIndex = 69
        Me.LblHruLayers.Text = "Select a template HRU"
        '
        'LstHruLayers
        '
        Me.LstHruLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstHruLayers.FormattingEnabled = True
        Me.LstHruLayers.ItemHeight = 16
        Me.LstHruLayers.Location = New System.Drawing.Point(165, 75)
        Me.LstHruLayers.Name = "LstHruLayers"
        Me.LstHruLayers.Size = New System.Drawing.Size(143, 100)
        Me.LstHruLayers.TabIndex = 68
        '
        'BtnGenerateHru
        '
        Me.BtnGenerateHru.Enabled = False
        Me.BtnGenerateHru.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnGenerateHru.Location = New System.Drawing.Point(687, 646)
        Me.BtnGenerateHru.Name = "BtnGenerateHru"
        Me.BtnGenerateHru.Size = New System.Drawing.Size(125, 30)
        Me.BtnGenerateHru.TabIndex = 72
        Me.BtnGenerateHru.Text = "Generate HRUs"
        Me.BtnGenerateHru.UseVisualStyleBackColor = True
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(565, 646)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(108, 30)
        Me.BtnCancel.TabIndex = 73
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'TxtErrorMsg
        '
        Me.TxtErrorMsg.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtErrorMsg.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtErrorMsg.ForeColor = System.Drawing.Color.Red
        Me.TxtErrorMsg.Location = New System.Drawing.Point(130, 182)
        Me.TxtErrorMsg.Multiline = True
        Me.TxtErrorMsg.Name = "TxtErrorMsg"
        Me.TxtErrorMsg.ReadOnly = True
        Me.TxtErrorMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TxtErrorMsg.Size = New System.Drawing.Size(686, 50)
        Me.TxtErrorMsg.TabIndex = 74
        Me.TxtErrorMsg.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(-1, 180)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(119, 16)
        Me.Label3.TabIndex = 75
        Me.Label3.Text = "Template errors"
        '
        'FrmUseTemplate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(822, 688)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TxtErrorMsg)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnGenerateHru)
        Me.Controls.Add(Me.LblHruLayers)
        Me.Controls.Add(Me.LstHruLayers)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TxtTemplateAoiPath)
        Me.Controls.Add(Me.BtnTemplateAOI)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TxtAoiPath)
        Me.Controls.Add(Me.BtnAOI)
        Me.Name = "FrmUseTemplate"
        Me.ShowIcon = False
        Me.Text = "Use Template"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TxtAoiPath As System.Windows.Forms.TextBox
    Friend WithEvents BtnAOI As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TxtTemplateAoiPath As System.Windows.Forms.TextBox
    Friend WithEvents BtnTemplateAOI As System.Windows.Forms.Button
    Friend WithEvents LblHruLayers As System.Windows.Forms.Label
    Friend WithEvents LstHruLayers As System.Windows.Forms.ListBox
    'Friend WithEvents LblOutputHruName As System.Windows.Forms.Label
    'Friend WithEvents TxtNewHruName As System.Windows.Forms.TextBox
    Friend WithEvents BtnGenerateHru As System.Windows.Forms.Button
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents TxtErrorMsg As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
