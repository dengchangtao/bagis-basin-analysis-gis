<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFileLocator
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtMethodsFolder = New System.Windows.Forms.TextBox()
        Me.txtProfFolder = New System.Windows.Forms.TextBox()
        Me.txtPubParRulTemp = New System.Windows.Forms.TextBox()
        Me.txtHRUTemplate = New System.Windows.Forms.TextBox()
        Me.txtDefFile = New System.Windows.Forms.TextBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.txtPathTextBox = New System.Windows.Forms.TextBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.txtGDBFilePath = New System.Windows.Forms.TextBox()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.btnSetAOI = New System.Windows.Forms.Button()
        Me.txtAOIPath = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.txtMethodsFolder)
        Me.GroupBox1.Controls.Add(Me.txtProfFolder)
        Me.GroupBox1.Controls.Add(Me.txtPubParRulTemp)
        Me.GroupBox1.Controls.Add(Me.txtHRUTemplate)
        Me.GroupBox1.Controls.Add(Me.txtDefFile)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(12, 22)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(894, 185)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "BAGIS System Files"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(99, 152)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(102, 16)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "Methods Folder"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(113, 123)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(88, 16)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "Profile Folder"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(64, 91)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(141, 16)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Public Parametrization"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(7, 60)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(198, 16)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "HRU Delineation RuleTemplate"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(70, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(131, 16)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "BAGIS Definition File"
        '
        'txtMethodsFolder
        '
        Me.txtMethodsFolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtMethodsFolder.Location = New System.Drawing.Point(207, 151)
        Me.txtMethodsFolder.Name = "txtMethodsFolder"
        Me.txtMethodsFolder.ReadOnly = True
        Me.txtMethodsFolder.Size = New System.Drawing.Size(670, 24)
        Me.txtMethodsFolder.TabIndex = 0
        '
        'txtProfFolder
        '
        Me.txtProfFolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtProfFolder.Location = New System.Drawing.Point(207, 122)
        Me.txtProfFolder.Name = "txtProfFolder"
        Me.txtProfFolder.ReadOnly = True
        Me.txtProfFolder.Size = New System.Drawing.Size(670, 24)
        Me.txtProfFolder.TabIndex = 0
        '
        'txtPubParRulTemp
        '
        Me.txtPubParRulTemp.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPubParRulTemp.Location = New System.Drawing.Point(207, 91)
        Me.txtPubParRulTemp.Name = "txtPubParRulTemp"
        Me.txtPubParRulTemp.ReadOnly = True
        Me.txtPubParRulTemp.Size = New System.Drawing.Size(670, 24)
        Me.txtPubParRulTemp.TabIndex = 0
        '
        'txtHRUTemplate
        '
        Me.txtHRUTemplate.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtHRUTemplate.Location = New System.Drawing.Point(207, 60)
        Me.txtHRUTemplate.Name = "txtHRUTemplate"
        Me.txtHRUTemplate.ReadOnly = True
        Me.txtHRUTemplate.Size = New System.Drawing.Size(670, 24)
        Me.txtHRUTemplate.TabIndex = 0
        '
        'txtDefFile
        '
        Me.txtDefFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDefFile.Location = New System.Drawing.Point(207, 29)
        Me.txtDefFile.Name = "txtDefFile"
        Me.txtDefFile.ReadOnly = True
        Me.txtDefFile.Size = New System.Drawing.Size(670, 24)
        Me.txtDefFile.TabIndex = 0
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtPathTextBox)
        Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(12, 263)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(894, 142)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Weasel Files & Folders"
        '
        'txtPathTextBox
        '
        Me.txtPathTextBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPathTextBox.Location = New System.Drawing.Point(10, 19)
        Me.txtPathTextBox.Multiline = True
        Me.txtPathTextBox.Name = "txtPathTextBox"
        Me.txtPathTextBox.ReadOnly = True
        Me.txtPathTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtPathTextBox.Size = New System.Drawing.Size(867, 115)
        Me.txtPathTextBox.TabIndex = 0
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.txtGDBFilePath)
        Me.GroupBox3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox3.Location = New System.Drawing.Point(12, 411)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(894, 142)
        Me.GroupBox3.TabIndex = 2
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "FGDB Files & Folders"
        '
        'txtGDBFilePath
        '
        Me.txtGDBFilePath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGDBFilePath.Location = New System.Drawing.Point(10, 19)
        Me.txtGDBFilePath.Multiline = True
        Me.txtGDBFilePath.Name = "txtGDBFilePath"
        Me.txtGDBFilePath.ReadOnly = True
        Me.txtGDBFilePath.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtGDBFilePath.Size = New System.Drawing.Size(867, 115)
        Me.txtGDBFilePath.TabIndex = 0
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnClose.Location = New System.Drawing.Point(784, 559)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(122, 38)
        Me.btnClose.TabIndex = 3
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnSetAOI
        '
        Me.btnSetAOI.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSetAOI.Location = New System.Drawing.Point(46, 221)
        Me.btnSetAOI.Name = "btnSetAOI"
        Me.btnSetAOI.Size = New System.Drawing.Size(108, 36)
        Me.btnSetAOI.TabIndex = 4
        Me.btnSetAOI.Text = "Set AOI"
        Me.btnSetAOI.UseVisualStyleBackColor = True
        '
        'txtAOIPath
        '
        Me.txtAOIPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAOIPath.Location = New System.Drawing.Point(320, 233)
        Me.txtAOIPath.Name = "txtAOIPath"
        Me.txtAOIPath.ReadOnly = True
        Me.txtAOIPath.Size = New System.Drawing.Size(559, 24)
        Me.txtAOIPath.TabIndex = 0
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(244, 234)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(60, 16)
        Me.Label12.TabIndex = 5
        Me.Label12.Text = "AOI Path"
        '
        'frmFileLocator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(918, 603)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.btnSetAOI)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.txtAOIPath)
        Me.Name = "frmFileLocator"
        Me.Text = "File Locator"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents txtMethodsFolder As System.Windows.Forms.TextBox
    Friend WithEvents txtProfFolder As System.Windows.Forms.TextBox
    Friend WithEvents txtPubParRulTemp As System.Windows.Forms.TextBox
    Friend WithEvents txtHRUTemplate As System.Windows.Forms.TextBox
    Friend WithEvents txtDefFile As System.Windows.Forms.TextBox
    Friend WithEvents txtPathTextBox As System.Windows.Forms.TextBox
    Friend WithEvents txtGDBFilePath As System.Windows.Forms.TextBox
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnSetAOI As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtAOIPath As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
End Class