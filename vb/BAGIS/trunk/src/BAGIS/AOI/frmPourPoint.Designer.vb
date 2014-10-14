<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmPourPoint
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
        Me.lstPPoints = New System.Windows.Forms.ListBox()
        Me.CmdOverview = New System.Windows.Forms.Button()
        Me.CmdNewPoint = New System.Windows.Forms.Button()
        Me.CmdSelect = New System.Windows.Forms.Button()
        Me.CmdExit = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lstPPoints
        '
        Me.lstPPoints.FormattingEnabled = True
        Me.lstPPoints.Location = New System.Drawing.Point(12, 11)
        Me.lstPPoints.Name = "lstPPoints"
        Me.lstPPoints.Size = New System.Drawing.Size(386, 199)
        Me.lstPPoints.TabIndex = 0
        '
        'CmdOverview
        '
        Me.CmdOverview.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdOverview.Location = New System.Drawing.Point(408, 11)
        Me.CmdOverview.Name = "CmdOverview"
        Me.CmdOverview.Size = New System.Drawing.Size(95, 30)
        Me.CmdOverview.TabIndex = 1
        Me.CmdOverview.Text = "View Basin"
        Me.CmdOverview.UseVisualStyleBackColor = True
        '
        'CmdNewPoint
        '
        Me.CmdNewPoint.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdNewPoint.Location = New System.Drawing.Point(408, 55)
        Me.CmdNewPoint.Name = "CmdNewPoint"
        Me.CmdNewPoint.Size = New System.Drawing.Size(95, 30)
        Me.CmdNewPoint.TabIndex = 1
        Me.CmdNewPoint.Text = "New"
        Me.CmdNewPoint.UseVisualStyleBackColor = True
        '
        'CmdSelect
        '
        Me.CmdSelect.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdSelect.Location = New System.Drawing.Point(408, 102)
        Me.CmdSelect.Name = "CmdSelect"
        Me.CmdSelect.Size = New System.Drawing.Size(95, 30)
        Me.CmdSelect.TabIndex = 1
        Me.CmdSelect.Text = "Select"
        Me.CmdSelect.UseVisualStyleBackColor = True
        '
        'CmdExit
        '
        Me.CmdExit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdExit.Location = New System.Drawing.Point(408, 180)
        Me.CmdExit.Name = "CmdExit"
        Me.CmdExit.Size = New System.Drawing.Size(95, 30)
        Me.CmdExit.TabIndex = 1
        Me.CmdExit.Text = "Cancel"
        Me.CmdExit.UseVisualStyleBackColor = True
        '
        'frmPourPoint
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(511, 219)
        Me.Controls.Add(Me.CmdExit)
        Me.Controls.Add(Me.CmdSelect)
        Me.Controls.Add(Me.CmdNewPoint)
        Me.Controls.Add(Me.CmdOverview)
        Me.Controls.Add(Me.lstPPoints)
        Me.Name = "frmPourPoint"
        Me.ShowIcon = False
        Me.Text = "Pour Points"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lstPPoints As System.Windows.Forms.ListBox
    Friend WithEvents CmdOverview As System.Windows.Forms.Button
    Friend WithEvents CmdNewPoint As System.Windows.Forms.Button
    Friend WithEvents CmdSelect As System.Windows.Forms.Button
    Friend WithEvents CmdExit As System.Windows.Forms.Button
End Class
