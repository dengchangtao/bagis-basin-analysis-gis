<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAbout
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAbout))
        Me.lblVersionText = New System.Windows.Forms.Label()
        Me.LblAbout = New System.Windows.Forms.Label()
        Me.CmbDisclaimer = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblVersionText
        '
        Me.lblVersionText.AccessibleName = ""
        Me.lblVersionText.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblVersionText.Location = New System.Drawing.Point(5, 9)
        Me.lblVersionText.Name = "lblVersionText"
        Me.lblVersionText.Size = New System.Drawing.Size(511, 31)
        Me.lblVersionText.TabIndex = 2
        Me.lblVersionText.Text = "BA_VersionText"
        Me.lblVersionText.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'LblAbout
        '
        Me.LblAbout.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAbout.Location = New System.Drawing.Point(2, 40)
        Me.LblAbout.Name = "LblAbout"
        Me.LblAbout.Size = New System.Drawing.Size(524, 191)
        Me.LblAbout.TabIndex = 3
        Me.LblAbout.Text = resources.GetString("LblAbout.Text")
        Me.LblAbout.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'CmbDisclaimer
        '
        Me.CmbDisclaimer.Location = New System.Drawing.Point(423, 213)
        Me.CmbDisclaimer.Name = "CmbDisclaimer"
        Me.CmbDisclaimer.Size = New System.Drawing.Size(93, 29)
        Me.CmbDisclaimer.TabIndex = 4
        Me.CmbDisclaimer.Text = "DISCLAIMERS"
        Me.CmbDisclaimer.UseVisualStyleBackColor = True
        '
        'frmAbout
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(528, 254)
        Me.Controls.Add(Me.CmbDisclaimer)
        Me.Controls.Add(Me.LblAbout)
        Me.Controls.Add(Me.lblVersionText)
        Me.Name = "frmAbout"
        Me.ShowIcon = False
        Me.Text = "About Basin Analysis GIS (BAGIS)"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblVersionText As System.Windows.Forms.Label
    Friend WithEvents LblAbout As System.Windows.Forms.Label
    Friend WithEvents CmbDisclaimer As System.Windows.Forms.Button
End Class
