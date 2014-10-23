<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCreateAOI
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
        Me.ChkSnapPP = New System.Windows.Forms.CheckBox()
        Me.lblSnapD = New System.Windows.Forms.Label()
        Me.txtSnapD = New System.Windows.Forms.TextBox()
        Me.lblSnapUnit = New System.Windows.Forms.Label()
        Me.ChkAOIBuffer = New System.Windows.Forms.CheckBox()
        Me.lblBufferD = New System.Windows.Forms.Label()
        Me.txtBufferD = New System.Windows.Forms.TextBox()
        Me.lblBufferUnit = New System.Windows.Forms.Label()
        Me.CmbRun = New System.Windows.Forms.Button()
        Me.grpboxPRISMUnit = New System.Windows.Forms.GroupBox()
        Me.rbtnDepthInch = New System.Windows.Forms.RadioButton()
        Me.rbtnDepthMM = New System.Windows.Forms.RadioButton()
        Me.lblSlopeUnit = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblDEMUnit = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblWhyBuffer = New System.Windows.Forms.Label()
        Me.grpboxPRISMUnit.SuspendLayout()
        Me.SuspendLayout()
        '
        'ChkSnapPP
        '
        Me.ChkSnapPP.AutoSize = True
        Me.ChkSnapPP.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkSnapPP.Location = New System.Drawing.Point(31, 12)
        Me.ChkSnapPP.Name = "ChkSnapPP"
        Me.ChkSnapPP.Size = New System.Drawing.Size(199, 20)
        Me.ChkSnapPP.TabIndex = 0
        Me.ChkSnapPP.Text = "Automatically snap pourpoint"
        Me.ChkSnapPP.UseVisualStyleBackColor = True
        '
        'lblSnapD
        '
        Me.lblSnapD.AutoSize = True
        Me.lblSnapD.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSnapD.Location = New System.Drawing.Point(47, 41)
        Me.lblSnapD.Name = "lblSnapD"
        Me.lblSnapD.Size = New System.Drawing.Size(99, 16)
        Me.lblSnapD.TabIndex = 1
        Me.lblSnapD.Text = "Snap Distance:"
        '
        'txtSnapD
        '
        Me.txtSnapD.Location = New System.Drawing.Point(149, 38)
        Me.txtSnapD.Name = "txtSnapD"
        Me.txtSnapD.Size = New System.Drawing.Size(68, 20)
        Me.txtSnapD.TabIndex = 2
        '
        'lblSnapUnit
        '
        Me.lblSnapUnit.AutoSize = True
        Me.lblSnapUnit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSnapUnit.Location = New System.Drawing.Point(223, 42)
        Me.lblSnapUnit.Name = "lblSnapUnit"
        Me.lblSnapUnit.Size = New System.Drawing.Size(49, 16)
        Me.lblSnapUnit.TabIndex = 1
        Me.lblSnapUnit.Text = "Meters"
        '
        'ChkAOIBuffer
        '
        Me.ChkAOIBuffer.AutoSize = True
        Me.ChkAOIBuffer.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkAOIBuffer.Location = New System.Drawing.Point(31, 86)
        Me.ChkAOIBuffer.Name = "ChkAOIBuffer"
        Me.ChkAOIBuffer.Size = New System.Drawing.Size(164, 20)
        Me.ChkAOIBuffer.TabIndex = 0
        Me.ChkAOIBuffer.Text = "Buffer AOI to clip layers"
        Me.ChkAOIBuffer.UseVisualStyleBackColor = True
        '
        'lblBufferD
        '
        Me.lblBufferD.AutoSize = True
        Me.lblBufferD.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBufferD.Location = New System.Drawing.Point(47, 119)
        Me.lblBufferD.Name = "lblBufferD"
        Me.lblBufferD.Size = New System.Drawing.Size(101, 16)
        Me.lblBufferD.TabIndex = 1
        Me.lblBufferD.Text = "Buffer Distance:"
        '
        'txtBufferD
        '
        Me.txtBufferD.Location = New System.Drawing.Point(149, 115)
        Me.txtBufferD.Name = "txtBufferD"
        Me.txtBufferD.Size = New System.Drawing.Size(68, 20)
        Me.txtBufferD.TabIndex = 2
        '
        'lblBufferUnit
        '
        Me.lblBufferUnit.AutoSize = True
        Me.lblBufferUnit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBufferUnit.Location = New System.Drawing.Point(223, 119)
        Me.lblBufferUnit.Name = "lblBufferUnit"
        Me.lblBufferUnit.Size = New System.Drawing.Size(49, 16)
        Me.lblBufferUnit.TabIndex = 1
        Me.lblBufferUnit.Text = "Meters"
        '
        'CmbRun
        '
        Me.CmbRun.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmbRun.Location = New System.Drawing.Point(75, 298)
        Me.CmbRun.Name = "CmbRun"
        Me.CmbRun.Size = New System.Drawing.Size(139, 31)
        Me.CmbRun.TabIndex = 3
        Me.CmbRun.Text = "Generate AOI"
        Me.CmbRun.UseVisualStyleBackColor = True
        '
        'grpboxPRISMUnit
        '
        Me.grpboxPRISMUnit.Controls.Add(Me.rbtnDepthInch)
        Me.grpboxPRISMUnit.Controls.Add(Me.rbtnDepthMM)
        Me.grpboxPRISMUnit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpboxPRISMUnit.Location = New System.Drawing.Point(38, 238)
        Me.grpboxPRISMUnit.Name = "grpboxPRISMUnit"
        Me.grpboxPRISMUnit.Size = New System.Drawing.Size(238, 54)
        Me.grpboxPRISMUnit.TabIndex = 17
        Me.grpboxPRISMUnit.TabStop = False
        Me.grpboxPRISMUnit.Text = "PRISM Depth Unit:"
        '
        'rbtnDepthInch
        '
        Me.rbtnDepthInch.AutoSize = True
        Me.rbtnDepthInch.Checked = True
        Me.rbtnDepthInch.ForeColor = System.Drawing.Color.Blue
        Me.rbtnDepthInch.Location = New System.Drawing.Point(57, 25)
        Me.rbtnDepthInch.Name = "rbtnDepthInch"
        Me.rbtnDepthInch.Size = New System.Drawing.Size(65, 20)
        Me.rbtnDepthInch.TabIndex = 1
        Me.rbtnDepthInch.TabStop = True
        Me.rbtnDepthInch.Text = "Inches"
        Me.rbtnDepthInch.UseVisualStyleBackColor = True
        '
        'rbtnDepthMM
        '
        Me.rbtnDepthMM.AutoSize = True
        Me.rbtnDepthMM.ForeColor = System.Drawing.Color.Blue
        Me.rbtnDepthMM.Location = New System.Drawing.Point(138, 25)
        Me.rbtnDepthMM.Name = "rbtnDepthMM"
        Me.rbtnDepthMM.Size = New System.Drawing.Size(90, 20)
        Me.rbtnDepthMM.TabIndex = 0
        Me.rbtnDepthMM.Text = "Millimeters"
        Me.rbtnDepthMM.UseVisualStyleBackColor = True
        '
        'lblSlopeUnit
        '
        Me.lblSlopeUnit.AutoSize = True
        Me.lblSlopeUnit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSlopeUnit.ForeColor = System.Drawing.Color.Black
        Me.lblSlopeUnit.Location = New System.Drawing.Point(167, 215)
        Me.lblSlopeUnit.Name = "lblSlopeUnit"
        Me.lblSlopeUnit.Size = New System.Drawing.Size(63, 16)
        Me.lblSlopeUnit.TabIndex = 16
        Me.lblSlopeUnit.Text = "Unknown"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(88, 215)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(73, 16)
        Me.Label5.TabIndex = 15
        Me.Label5.Text = "Slope Unit:"
        '
        'lblDEMUnit
        '
        Me.lblDEMUnit.AutoSize = True
        Me.lblDEMUnit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDEMUnit.ForeColor = System.Drawing.Color.Black
        Me.lblDEMUnit.Location = New System.Drawing.Point(167, 191)
        Me.lblDEMUnit.Name = "lblDEMUnit"
        Me.lblDEMUnit.Size = New System.Drawing.Size(63, 16)
        Me.lblDEMUnit.TabIndex = 14
        Me.lblDEMUnit.Text = "Unknown"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(35, 191)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(126, 16)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "DEM Elevation Unit:"
        '
        'lblWhyBuffer
        '
        Me.lblWhyBuffer.AutoSize = True
        Me.lblWhyBuffer.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWhyBuffer.ForeColor = System.Drawing.Color.Blue
        Me.lblWhyBuffer.Location = New System.Drawing.Point(118, 147)
        Me.lblWhyBuffer.Name = "lblWhyBuffer"
        Me.lblWhyBuffer.Size = New System.Drawing.Size(148, 20)
        Me.lblWhyBuffer.TabIndex = 18
        Me.lblWhyBuffer.Text = "(Why use a buffer?)"
        '
        'frmCreateAOI
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(291, 343)
        Me.Controls.Add(Me.lblWhyBuffer)
        Me.Controls.Add(Me.grpboxPRISMUnit)
        Me.Controls.Add(Me.lblSlopeUnit)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.lblDEMUnit)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.CmbRun)
        Me.Controls.Add(Me.lblBufferUnit)
        Me.Controls.Add(Me.lblSnapUnit)
        Me.Controls.Add(Me.txtBufferD)
        Me.Controls.Add(Me.txtSnapD)
        Me.Controls.Add(Me.lblBufferD)
        Me.Controls.Add(Me.lblSnapD)
        Me.Controls.Add(Me.ChkAOIBuffer)
        Me.Controls.Add(Me.ChkSnapPP)
        Me.Name = "frmCreateAOI"
        Me.ShowIcon = False
        Me.Text = "Create AOI"
        Me.grpboxPRISMUnit.ResumeLayout(False)
        Me.grpboxPRISMUnit.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ChkSnapPP As System.Windows.Forms.CheckBox
    Friend WithEvents lblSnapD As System.Windows.Forms.Label
    Friend WithEvents txtSnapD As System.Windows.Forms.TextBox
    Friend WithEvents lblSnapUnit As System.Windows.Forms.Label
    Friend WithEvents ChkAOIBuffer As System.Windows.Forms.CheckBox
    Friend WithEvents lblBufferD As System.Windows.Forms.Label
    Friend WithEvents txtBufferD As System.Windows.Forms.TextBox
    Friend WithEvents lblBufferUnit As System.Windows.Forms.Label
    Friend WithEvents CmbRun As System.Windows.Forms.Button
    Friend WithEvents grpboxPRISMUnit As System.Windows.Forms.GroupBox
    Friend WithEvents rbtnDepthInch As System.Windows.Forms.RadioButton
    Friend WithEvents rbtnDepthMM As System.Windows.Forms.RadioButton
    Friend WithEvents lblSlopeUnit As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents lblDEMUnit As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblWhyBuffer As System.Windows.Forms.Label
End Class
