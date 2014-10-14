<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmViewDEMLayers
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ChkFilledDEM = New System.Windows.Forms.CheckBox()
        Me.ChkFlowDir = New System.Windows.Forms.CheckBox()
        Me.ChkFlowAccum = New System.Windows.Forms.CheckBox()
        Me.ChkSlope = New System.Windows.Forms.CheckBox()
        Me.ChkAspect = New System.Windows.Forms.CheckBox()
        Me.ChkHillshade = New System.Windows.Forms.CheckBox()
        Me.ChkPourPoint = New System.Windows.Forms.CheckBox()
        Me.CmdSelectAll = New System.Windows.Forms.Button()
        Me.CmdSelectNone = New System.Windows.Forms.Button()
        Me.CmdContinue = New System.Windows.Forms.Button()
        Me.CmdCancel = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.Label1.Location = New System.Drawing.Point(7, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(174, 17)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Select the layer(s) to view:"
        '
        'ChkFilledDEM
        '
        Me.ChkFilledDEM.AutoSize = True
        Me.ChkFilledDEM.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.ChkFilledDEM.Location = New System.Drawing.Point(26, 38)
        Me.ChkFilledDEM.Name = "ChkFilledDEM"
        Me.ChkFilledDEM.Size = New System.Drawing.Size(94, 21)
        Me.ChkFilledDEM.TabIndex = 1
        Me.ChkFilledDEM.Text = "Filled DEM"
        Me.ChkFilledDEM.UseVisualStyleBackColor = True
        '
        'ChkFlowDir
        '
        Me.ChkFlowDir.AutoSize = True
        Me.ChkFlowDir.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.ChkFlowDir.Location = New System.Drawing.Point(26, 65)
        Me.ChkFlowDir.Name = "ChkFlowDir"
        Me.ChkFlowDir.Size = New System.Drawing.Size(115, 21)
        Me.ChkFlowDir.TabIndex = 1
        Me.ChkFlowDir.Text = "Flow Direction"
        Me.ChkFlowDir.UseVisualStyleBackColor = True
        '
        'ChkFlowAccum
        '
        Me.ChkFlowAccum.AutoSize = True
        Me.ChkFlowAccum.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.ChkFlowAccum.Location = New System.Drawing.Point(26, 92)
        Me.ChkFlowAccum.Name = "ChkFlowAccum"
        Me.ChkFlowAccum.Size = New System.Drawing.Size(143, 21)
        Me.ChkFlowAccum.TabIndex = 1
        Me.ChkFlowAccum.Text = "Flow Accumulation"
        Me.ChkFlowAccum.UseVisualStyleBackColor = True
        '
        'ChkSlope
        '
        Me.ChkSlope.AutoSize = True
        Me.ChkSlope.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.ChkSlope.Location = New System.Drawing.Point(26, 119)
        Me.ChkSlope.Name = "ChkSlope"
        Me.ChkSlope.Size = New System.Drawing.Size(63, 21)
        Me.ChkSlope.TabIndex = 1
        Me.ChkSlope.Text = "Slope"
        Me.ChkSlope.UseVisualStyleBackColor = True
        '
        'ChkAspect
        '
        Me.ChkAspect.AutoSize = True
        Me.ChkAspect.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.ChkAspect.Location = New System.Drawing.Point(26, 146)
        Me.ChkAspect.Name = "ChkAspect"
        Me.ChkAspect.Size = New System.Drawing.Size(70, 21)
        Me.ChkAspect.TabIndex = 1
        Me.ChkAspect.Text = "Aspect"
        Me.ChkAspect.UseVisualStyleBackColor = True
        '
        'ChkHillshade
        '
        Me.ChkHillshade.AutoSize = True
        Me.ChkHillshade.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.ChkHillshade.Location = New System.Drawing.Point(26, 173)
        Me.ChkHillshade.Name = "ChkHillshade"
        Me.ChkHillshade.Size = New System.Drawing.Size(85, 21)
        Me.ChkHillshade.TabIndex = 1
        Me.ChkHillshade.Text = "Hillshade"
        Me.ChkHillshade.UseVisualStyleBackColor = True
        '
        'ChkPourPoint
        '
        Me.ChkPourPoint.AutoSize = True
        Me.ChkPourPoint.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.ChkPourPoint.Location = New System.Drawing.Point(26, 200)
        Me.ChkPourPoint.Name = "ChkPourPoint"
        Me.ChkPourPoint.Size = New System.Drawing.Size(93, 21)
        Me.ChkPourPoint.TabIndex = 1
        Me.ChkPourPoint.Text = "Pour Point"
        Me.ChkPourPoint.UseVisualStyleBackColor = True
        '
        'CmdSelectAll
        '
        Me.CmdSelectAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdSelectAll.Location = New System.Drawing.Point(12, 227)
        Me.CmdSelectAll.Name = "CmdSelectAll"
        Me.CmdSelectAll.Size = New System.Drawing.Size(87, 28)
        Me.CmdSelectAll.TabIndex = 2
        Me.CmdSelectAll.Text = "All"
        Me.CmdSelectAll.UseVisualStyleBackColor = True
        '
        'CmdSelectNone
        '
        Me.CmdSelectNone.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdSelectNone.Location = New System.Drawing.Point(108, 227)
        Me.CmdSelectNone.Name = "CmdSelectNone"
        Me.CmdSelectNone.Size = New System.Drawing.Size(87, 28)
        Me.CmdSelectNone.TabIndex = 2
        Me.CmdSelectNone.Text = "None"
        Me.CmdSelectNone.UseVisualStyleBackColor = True
        '
        'CmdContinue
        '
        Me.CmdContinue.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdContinue.Location = New System.Drawing.Point(12, 261)
        Me.CmdContinue.Name = "CmdContinue"
        Me.CmdContinue.Size = New System.Drawing.Size(87, 28)
        Me.CmdContinue.TabIndex = 2
        Me.CmdContinue.Text = "Display"
        Me.CmdContinue.UseVisualStyleBackColor = True
        '
        'CmdCancel
        '
        Me.CmdCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdCancel.Location = New System.Drawing.Point(108, 261)
        Me.CmdCancel.Name = "CmdCancel"
        Me.CmdCancel.Size = New System.Drawing.Size(87, 28)
        Me.CmdCancel.TabIndex = 2
        Me.CmdCancel.Text = "Cancel"
        Me.CmdCancel.UseVisualStyleBackColor = True
        '
        'frmViewDEMLayers
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(205, 296)
        Me.Controls.Add(Me.CmdCancel)
        Me.Controls.Add(Me.CmdContinue)
        Me.Controls.Add(Me.CmdSelectNone)
        Me.Controls.Add(Me.CmdSelectAll)
        Me.Controls.Add(Me.ChkPourPoint)
        Me.Controls.Add(Me.ChkHillshade)
        Me.Controls.Add(Me.ChkAspect)
        Me.Controls.Add(Me.ChkSlope)
        Me.Controls.Add(Me.ChkFlowAccum)
        Me.Controls.Add(Me.ChkFlowDir)
        Me.Controls.Add(Me.ChkFilledDEM)
        Me.Controls.Add(Me.Label1)
        Me.Name = "frmViewDEMLayers"
        Me.ShowIcon = False
        Me.Text = "Load DEM Layers"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ChkFilledDEM As System.Windows.Forms.CheckBox
    Friend WithEvents ChkFlowDir As System.Windows.Forms.CheckBox
    Friend WithEvents ChkFlowAccum As System.Windows.Forms.CheckBox
    Friend WithEvents ChkSlope As System.Windows.Forms.CheckBox
    Friend WithEvents ChkAspect As System.Windows.Forms.CheckBox
    Friend WithEvents ChkHillshade As System.Windows.Forms.CheckBox
    Friend WithEvents ChkPourPoint As System.Windows.Forms.CheckBox
    Friend WithEvents CmdSelectAll As System.Windows.Forms.Button
    Friend WithEvents CmdSelectNone As System.Windows.Forms.Button
    Friend WithEvents CmdContinue As System.Windows.Forms.Button
    Friend WithEvents CmdCancel As System.Windows.Forms.Button
End Class
