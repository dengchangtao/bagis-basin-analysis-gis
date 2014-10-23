<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAOI_Tool
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
        Me.lblBasin = New System.Windows.Forms.TextBox()
        Me.lstAOIList = New System.Windows.Forms.ListBox()
        Me.CmdNewAOI = New System.Windows.Forms.Button()
        Me.CmdAOIView = New System.Windows.Forms.Button()
        Me.CmdViewLayers = New System.Windows.Forms.Button()
        Me.CmdSelectAOI = New System.Windows.Forms.Button()
        Me.CmdDeleteAOI = New System.Windows.Forms.Button()
        Me.CmdExit = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(13, 18)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(88, 16)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Basin Name: "
        '
        'lblBasin
        '
        Me.lblBasin.BackColor = System.Drawing.SystemColors.Menu
        Me.lblBasin.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblBasin.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBasin.Location = New System.Drawing.Point(99, 19)
        Me.lblBasin.Name = "lblBasin"
        Me.lblBasin.ReadOnly = True
        Me.lblBasin.Size = New System.Drawing.Size(303, 15)
        Me.lblBasin.TabIndex = 1
        '
        'lstAOIList
        '
        Me.lstAOIList.FormattingEnabled = True
        Me.lstAOIList.Location = New System.Drawing.Point(12, 52)
        Me.lstAOIList.Name = "lstAOIList"
        Me.lstAOIList.Size = New System.Drawing.Size(316, 186)
        Me.lstAOIList.TabIndex = 2
        '
        'CmdNewAOI
        '
        Me.CmdNewAOI.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdNewAOI.Location = New System.Drawing.Point(334, 53)
        Me.CmdNewAOI.Name = "CmdNewAOI"
        Me.CmdNewAOI.Size = New System.Drawing.Size(100, 29)
        Me.CmdNewAOI.TabIndex = 3
        Me.CmdNewAOI.Text = "New"
        Me.CmdNewAOI.UseVisualStyleBackColor = True
        '
        'CmdAOIView
        '
        Me.CmdAOIView.Font = New System.Drawing.Font("Wingdings", 13.0!)
        Me.CmdAOIView.Location = New System.Drawing.Point(334, 87)
        Me.CmdAOIView.Name = "CmdAOIView"
        Me.CmdAOIView.Size = New System.Drawing.Size(35, 29)
        Me.CmdAOIView.TabIndex = 3
        Me.CmdAOIView.Text = "q"
        Me.CmdAOIView.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.CmdAOIView.UseVisualStyleBackColor = True
        '
        'CmdViewLayers
        '
        Me.CmdViewLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdViewLayers.Location = New System.Drawing.Point(369, 87)
        Me.CmdViewLayers.Name = "CmdViewLayers"
        Me.CmdViewLayers.Size = New System.Drawing.Size(65, 29)
        Me.CmdViewLayers.TabIndex = 3
        Me.CmdViewLayers.Text = "Layers"
        Me.CmdViewLayers.UseVisualStyleBackColor = True
        '
        'CmdSelectAOI
        '
        Me.CmdSelectAOI.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdSelectAOI.Location = New System.Drawing.Point(334, 126)
        Me.CmdSelectAOI.Name = "CmdSelectAOI"
        Me.CmdSelectAOI.Size = New System.Drawing.Size(100, 29)
        Me.CmdSelectAOI.TabIndex = 3
        Me.CmdSelectAOI.Text = "Select"
        Me.CmdSelectAOI.UseVisualStyleBackColor = True
        '
        'CmdDeleteAOI
        '
        Me.CmdDeleteAOI.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdDeleteAOI.Location = New System.Drawing.Point(334, 163)
        Me.CmdDeleteAOI.Name = "CmdDeleteAOI"
        Me.CmdDeleteAOI.Size = New System.Drawing.Size(100, 29)
        Me.CmdDeleteAOI.TabIndex = 3
        Me.CmdDeleteAOI.Text = "Delete"
        Me.CmdDeleteAOI.UseVisualStyleBackColor = True
        '
        'CmdExit
        '
        Me.CmdExit.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdExit.Location = New System.Drawing.Point(334, 202)
        Me.CmdExit.Name = "CmdExit"
        Me.CmdExit.Size = New System.Drawing.Size(100, 29)
        Me.CmdExit.TabIndex = 3
        Me.CmdExit.Text = "Close"
        Me.CmdExit.UseVisualStyleBackColor = True
        '
        'frmAOI_Tool
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(445, 243)
        Me.Controls.Add(Me.CmdAOIView)
        Me.Controls.Add(Me.CmdViewLayers)
        Me.Controls.Add(Me.CmdExit)
        Me.Controls.Add(Me.CmdDeleteAOI)
        Me.Controls.Add(Me.CmdSelectAOI)
        Me.Controls.Add(Me.CmdNewAOI)
        Me.Controls.Add(Me.lstAOIList)
        Me.Controls.Add(Me.lblBasin)
        Me.Controls.Add(Me.Label2)
        Me.Name = "frmAOI_Tool"
        Me.ShowIcon = False
        Me.Text = "AOI Tool"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblBasin As System.Windows.Forms.TextBox
    Friend WithEvents lstAOIList As System.Windows.Forms.ListBox
    Friend WithEvents CmdNewAOI As System.Windows.Forms.Button
    Friend WithEvents CmdAOIView As System.Windows.Forms.Button
    Friend WithEvents CmdViewLayers As System.Windows.Forms.Button
    Friend WithEvents CmdSelectAOI As System.Windows.Forms.Button
    Friend WithEvents CmdDeleteAOI As System.Windows.Forms.Button
    Friend WithEvents CmdExit As System.Windows.Forms.Button
End Class
