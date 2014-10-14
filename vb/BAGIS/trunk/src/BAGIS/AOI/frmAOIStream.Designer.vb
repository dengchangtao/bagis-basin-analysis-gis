<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAOIStream
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
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.OptStdv = New System.Windows.Forms.RadioButton()
        Me.OptFlowAcc = New System.Windows.Forms.RadioButton()
        Me.txtMax = New System.Windows.Forms.TextBox()
        Me.txtSTDV = New System.Windows.Forms.TextBox()
        Me.txtThresholdSTDV = New System.Windows.Forms.TextBox()
        Me.txtThresholdFAcc = New System.Windows.Forms.TextBox()
        Me.CmdCreateStream = New System.Windows.Forms.Button()
        Me.CmdCancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(60, 24)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(174, 15)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Max Flow Accumulation Value:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(0, 50)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(233, 15)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Standard Deviation of Flow Accumulation:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(73, 97)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(87, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Threshold Value:"
        '
        'OptStdv
        '
        Me.OptStdv.AutoSize = True
        Me.OptStdv.Location = New System.Drawing.Point(180, 95)
        Me.OptStdv.Name = "OptStdv"
        Me.OptStdv.Size = New System.Drawing.Size(116, 17)
        Me.OptStdv.TabIndex = 1
        Me.OptStdv.TabStop = True
        Me.OptStdv.Text = "Standard Deviation"
        Me.OptStdv.UseVisualStyleBackColor = True
        '
        'OptFlowAcc
        '
        Me.OptFlowAcc.AutoSize = True
        Me.OptFlowAcc.Location = New System.Drawing.Point(180, 118)
        Me.OptFlowAcc.Name = "OptFlowAcc"
        Me.OptFlowAcc.Size = New System.Drawing.Size(114, 17)
        Me.OptFlowAcc.TabIndex = 1
        Me.OptFlowAcc.TabStop = True
        Me.OptFlowAcc.Text = "Flow Accumulation"
        Me.OptFlowAcc.UseVisualStyleBackColor = True
        '
        'txtMax
        '
        Me.txtMax.BackColor = System.Drawing.SystemColors.Menu
        Me.txtMax.Location = New System.Drawing.Point(236, 22)
        Me.txtMax.Name = "txtMax"
        Me.txtMax.Size = New System.Drawing.Size(226, 20)
        Me.txtMax.TabIndex = 2
        Me.txtMax.Text = "0"
        Me.txtMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtSTDV
        '
        Me.txtSTDV.BackColor = System.Drawing.SystemColors.Menu
        Me.txtSTDV.Location = New System.Drawing.Point(236, 50)
        Me.txtSTDV.Name = "txtSTDV"
        Me.txtSTDV.Size = New System.Drawing.Size(226, 20)
        Me.txtSTDV.TabIndex = 2
        Me.txtSTDV.Text = "0"
        Me.txtSTDV.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtThresholdSTDV
        '
        Me.txtThresholdSTDV.BackColor = System.Drawing.SystemColors.Window
        Me.txtThresholdSTDV.Location = New System.Drawing.Point(303, 92)
        Me.txtThresholdSTDV.Name = "txtThresholdSTDV"
        Me.txtThresholdSTDV.Size = New System.Drawing.Size(159, 20)
        Me.txtThresholdSTDV.TabIndex = 2
        Me.txtThresholdSTDV.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtThresholdFAcc
        '
        Me.txtThresholdFAcc.BackColor = System.Drawing.SystemColors.Window
        Me.txtThresholdFAcc.Location = New System.Drawing.Point(303, 118)
        Me.txtThresholdFAcc.Name = "txtThresholdFAcc"
        Me.txtThresholdFAcc.Size = New System.Drawing.Size(159, 20)
        Me.txtThresholdFAcc.TabIndex = 2
        Me.txtThresholdFAcc.Text = "0"
        Me.txtThresholdFAcc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'CmdCreateStream
        '
        Me.CmdCreateStream.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdCreateStream.Location = New System.Drawing.Point(259, 152)
        Me.CmdCreateStream.Name = "CmdCreateStream"
        Me.CmdCreateStream.Size = New System.Drawing.Size(95, 29)
        Me.CmdCreateStream.TabIndex = 3
        Me.CmdCreateStream.Text = "Create"
        Me.CmdCreateStream.UseVisualStyleBackColor = True
        '
        'CmdCancel
        '
        Me.CmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdCancel.Location = New System.Drawing.Point(366, 152)
        Me.CmdCancel.Name = "CmdCancel"
        Me.CmdCancel.Size = New System.Drawing.Size(95, 29)
        Me.CmdCancel.TabIndex = 3
        Me.CmdCancel.Text = "Cancel"
        Me.CmdCancel.UseVisualStyleBackColor = True
        '
        'frmAOIStream
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(474, 193)
        Me.Controls.Add(Me.CmdCancel)
        Me.Controls.Add(Me.CmdCreateStream)
        Me.Controls.Add(Me.txtThresholdFAcc)
        Me.Controls.Add(Me.txtThresholdSTDV)
        Me.Controls.Add(Me.txtSTDV)
        Me.Controls.Add(Me.txtMax)
        Me.Controls.Add(Me.OptFlowAcc)
        Me.Controls.Add(Me.OptStdv)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Name = "frmAOIStream"
        Me.ShowIcon = False
        Me.Text = "AOI Flow Accumulation to Streams"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents OptStdv As System.Windows.Forms.RadioButton
    Friend WithEvents OptFlowAcc As System.Windows.Forms.RadioButton
    Friend WithEvents txtMax As System.Windows.Forms.TextBox
    Friend WithEvents txtSTDV As System.Windows.Forms.TextBox
    Friend WithEvents txtThresholdSTDV As System.Windows.Forms.TextBox
    Friend WithEvents txtThresholdFAcc As System.Windows.Forms.TextBox
    Friend WithEvents CmdCreateStream As System.Windows.Forms.Button
    Friend WithEvents CmdCancel As System.Windows.Forms.Button
End Class
