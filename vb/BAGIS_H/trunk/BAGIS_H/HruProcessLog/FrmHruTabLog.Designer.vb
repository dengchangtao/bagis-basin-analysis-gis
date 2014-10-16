<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmHruTabLog
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
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.BtnViewParent = New System.Windows.Forms.Button
        Me.TxtAppVersion = New System.Windows.Forms.TextBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.TxtDateCreated = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.TxtMaxSize = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.TxtAverageSize = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.TxtPolygonCount = New System.Windows.Forms.TextBox
        Me.TxtNonContiguous = New System.Windows.Forms.TextBox
        Me.TxtUnits = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.TxtApplyZones = New System.Windows.Forms.TextBox
        Me.TxtParentName = New System.Windows.Forms.TextBox
        Me.TxtHruPath = New System.Windows.Forms.TextBox
        Me.TxtHruName = New System.Windows.Forms.TextBox
        Me.TxtAoiPath = New System.Windows.Forms.TextBox
        Me.TxtAoiName = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.BtnClose = New System.Windows.Forms.Button
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Location = New System.Drawing.Point(3, 1)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(650, 375)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.SystemColors.Control
        Me.TabPage1.Controls.Add(Me.BtnViewParent)
        Me.TabPage1.Controls.Add(Me.TxtAppVersion)
        Me.TabPage1.Controls.Add(Me.Label14)
        Me.TabPage1.Controls.Add(Me.TxtDateCreated)
        Me.TabPage1.Controls.Add(Me.Label13)
        Me.TabPage1.Controls.Add(Me.TxtMaxSize)
        Me.TabPage1.Controls.Add(Me.Label12)
        Me.TabPage1.Controls.Add(Me.TxtAverageSize)
        Me.TabPage1.Controls.Add(Me.Label2)
        Me.TabPage1.Controls.Add(Me.TxtPolygonCount)
        Me.TabPage1.Controls.Add(Me.TxtNonContiguous)
        Me.TabPage1.Controls.Add(Me.TxtUnits)
        Me.TabPage1.Controls.Add(Me.Label11)
        Me.TabPage1.Controls.Add(Me.Label10)
        Me.TabPage1.Controls.Add(Me.Label4)
        Me.TabPage1.Controls.Add(Me.TxtApplyZones)
        Me.TabPage1.Controls.Add(Me.TxtParentName)
        Me.TabPage1.Controls.Add(Me.TxtHruPath)
        Me.TabPage1.Controls.Add(Me.TxtHruName)
        Me.TabPage1.Controls.Add(Me.TxtAoiPath)
        Me.TabPage1.Controls.Add(Me.TxtAoiName)
        Me.TabPage1.Controls.Add(Me.Label8)
        Me.TabPage1.Controls.Add(Me.Label7)
        Me.TabPage1.Controls.Add(Me.Label6)
        Me.TabPage1.Controls.Add(Me.Label5)
        Me.TabPage1.Controls.Add(Me.Label3)
        Me.TabPage1.Controls.Add(Me.Label1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(642, 349)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Summary"
        '
        'BtnViewParent
        '
        Me.BtnViewParent.Location = New System.Drawing.Point(531, 107)
        Me.BtnViewParent.Name = "BtnViewParent"
        Me.BtnViewParent.Size = New System.Drawing.Size(104, 23)
        Me.BtnViewParent.TabIndex = 76
        Me.BtnViewParent.Text = "View HRU Log"
        Me.BtnViewParent.UseVisualStyleBackColor = True
        '
        'TxtAppVersion
        '
        Me.TxtAppVersion.Location = New System.Drawing.Point(331, 280)
        Me.TxtAppVersion.Name = "TxtAppVersion"
        Me.TxtAppVersion.ReadOnly = True
        Me.TxtAppVersion.Size = New System.Drawing.Size(50, 20)
        Me.TxtAppVersion.TabIndex = 75
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(216, 283)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(119, 13)
        Me.Label14.TabIndex = 74
        Me.Label14.Text = "Application version:"
        '
        'TxtDateCreated
        '
        Me.TxtDateCreated.Location = New System.Drawing.Point(84, 280)
        Me.TxtDateCreated.Name = "TxtDateCreated"
        Me.TxtDateCreated.ReadOnly = True
        Me.TxtDateCreated.Size = New System.Drawing.Size(127, 20)
        Me.TxtDateCreated.TabIndex = 73
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(2, 283)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(85, 13)
        Me.Label13.TabIndex = 72
        Me.Label13.Text = "Date created:"
        '
        'TxtMaxSize
        '
        Me.TxtMaxSize.Location = New System.Drawing.Point(89, 256)
        Me.TxtMaxSize.Name = "TxtMaxSize"
        Me.TxtMaxSize.ReadOnly = True
        Me.TxtMaxSize.Size = New System.Drawing.Size(50, 20)
        Me.TxtMaxSize.TabIndex = 71
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(2, 259)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(88, 13)
        Me.Label12.TabIndex = 70
        Me.Label12.Text = "Maximum size:"
        '
        'TxtAverageSize
        '
        Me.TxtAverageSize.Location = New System.Drawing.Point(89, 232)
        Me.TxtAverageSize.Name = "TxtAverageSize"
        Me.TxtAverageSize.ReadOnly = True
        Me.TxtAverageSize.Size = New System.Drawing.Size(50, 20)
        Me.TxtAverageSize.TabIndex = 69
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(2, 235)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 13)
        Me.Label2.TabIndex = 68
        Me.Label2.Text = "Average size:"
        '
        'TxtPolygonCount
        '
        Me.TxtPolygonCount.Location = New System.Drawing.Point(89, 208)
        Me.TxtPolygonCount.Name = "TxtPolygonCount"
        Me.TxtPolygonCount.ReadOnly = True
        Me.TxtPolygonCount.Size = New System.Drawing.Size(50, 20)
        Me.TxtPolygonCount.TabIndex = 67
        '
        'TxtNonContiguous
        '
        Me.TxtNonContiguous.Location = New System.Drawing.Point(161, 159)
        Me.TxtNonContiguous.Name = "TxtNonContiguous"
        Me.TxtNonContiguous.ReadOnly = True
        Me.TxtNonContiguous.Size = New System.Drawing.Size(50, 20)
        Me.TxtNonContiguous.TabIndex = 66
        '
        'TxtUnits
        '
        Me.TxtUnits.Location = New System.Drawing.Point(39, 181)
        Me.TxtUnits.Name = "TxtUnits"
        Me.TxtUnits.ReadOnly = True
        Me.TxtUnits.Size = New System.Drawing.Size(100, 20)
        Me.TxtUnits.TabIndex = 65
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(2, 211)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(92, 13)
        Me.Label11.TabIndex = 64
        Me.Label11.Text = "Polygon count:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(2, 162)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(163, 13)
        Me.Label10.TabIndex = 63
        Me.Label10.Text = "Allow non-contiguous HRU:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(2, 186)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 13)
        Me.Label4.TabIndex = 62
        Me.Label4.Text = "Units:"
        '
        'TxtApplyZones
        '
        Me.TxtApplyZones.Location = New System.Drawing.Point(161, 134)
        Me.TxtApplyZones.Name = "TxtApplyZones"
        Me.TxtApplyZones.ReadOnly = True
        Me.TxtApplyZones.Size = New System.Drawing.Size(475, 20)
        Me.TxtApplyZones.TabIndex = 61
        '
        'TxtParentName
        '
        Me.TxtParentName.Location = New System.Drawing.Point(130, 108)
        Me.TxtParentName.Name = "TxtParentName"
        Me.TxtParentName.ReadOnly = True
        Me.TxtParentName.Size = New System.Drawing.Size(396, 20)
        Me.TxtParentName.TabIndex = 60
        '
        'TxtHruPath
        '
        Me.TxtHruPath.Location = New System.Drawing.Point(67, 82)
        Me.TxtHruPath.Name = "TxtHruPath"
        Me.TxtHruPath.ReadOnly = True
        Me.TxtHruPath.Size = New System.Drawing.Size(569, 20)
        Me.TxtHruPath.TabIndex = 58
        '
        'TxtHruName
        '
        Me.TxtHruName.Location = New System.Drawing.Point(72, 58)
        Me.TxtHruName.Name = "TxtHruName"
        Me.TxtHruName.ReadOnly = True
        Me.TxtHruName.Size = New System.Drawing.Size(564, 20)
        Me.TxtHruName.TabIndex = 57
        '
        'TxtAoiPath
        '
        Me.TxtAoiPath.Location = New System.Drawing.Point(56, 33)
        Me.TxtAoiPath.Name = "TxtAoiPath"
        Me.TxtAoiPath.ReadOnly = True
        Me.TxtAoiPath.Size = New System.Drawing.Size(580, 20)
        Me.TxtAoiPath.TabIndex = 56
        '
        'TxtAoiName
        '
        Me.TxtAoiName.Location = New System.Drawing.Point(61, 8)
        Me.TxtAoiName.Name = "TxtAoiName"
        Me.TxtAoiName.ReadOnly = True
        Me.TxtAoiName.Size = New System.Drawing.Size(575, 20)
        Me.TxtAoiName.TabIndex = 55
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(2, 137)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(165, 13)
        Me.Label8.TabIndex = 54
        Me.Label8.Text = "Apply to parent HRU zones:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(2, 113)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(134, 13)
        Me.Label7.TabIndex = 52
        Me.Label7.Text = "Parent template name:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(2, 85)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(67, 13)
        Me.Label6.TabIndex = 51
        Me.Label6.Text = "HRU path:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(2, 61)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(72, 13)
        Me.Label5.TabIndex = 50
        Me.Label5.Text = "HRU name:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(2, 36)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(58, 13)
        Me.Label3.TabIndex = 49
        Me.Label3.Text = "Aoi path:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(2, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 13)
        Me.Label1.TabIndex = 48
        Me.Label1.Text = "Aoi name:"
        '
        'BtnClose
        '
        Me.BtnClose.Location = New System.Drawing.Point(568, 382)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(75, 23)
        Me.BtnClose.TabIndex = 1
        Me.BtnClose.Text = "Close"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'FrmHruTabLog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(667, 423)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "FrmHruTabLog"
        Me.ShowIcon = False
        Me.Text = "FrmHruTabLog"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TxtApplyZones As System.Windows.Forms.TextBox
    Friend WithEvents TxtParentName As System.Windows.Forms.TextBox
    Friend WithEvents TxtHruPath As System.Windows.Forms.TextBox
    Friend WithEvents TxtHruName As System.Windows.Forms.TextBox
    Friend WithEvents TxtAoiPath As System.Windows.Forms.TextBox
    Friend WithEvents TxtAoiName As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TxtDateCreated As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents TxtMaxSize As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents TxtAverageSize As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TxtPolygonCount As System.Windows.Forms.TextBox
    Friend WithEvents TxtNonContiguous As System.Windows.Forms.TextBox
    Friend WithEvents TxtUnits As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents BtnClose As System.Windows.Forms.Button
    Friend WithEvents TxtAppVersion As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents BtnViewParent As System.Windows.Forms.Button
End Class
