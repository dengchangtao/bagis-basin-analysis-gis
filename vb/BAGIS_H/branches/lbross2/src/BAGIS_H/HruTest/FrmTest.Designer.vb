<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmTest
  Inherits System.Windows.Forms.UserControl

  'Form overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    If disposing AndAlso components IsNot Nothing Then
      components.Dispose()
    End If
    MyBase.Dispose(disposing)
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
        Me.BtnSubmit = New System.Windows.Forms.Button
        Me.TxtSlices = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'BtnSubmit
        '
        Me.BtnSubmit.Location = New System.Drawing.Point(103, 231)
        Me.BtnSubmit.Name = "BtnSubmit"
        Me.BtnSubmit.Size = New System.Drawing.Size(75, 23)
        Me.BtnSubmit.TabIndex = 0
        Me.BtnSubmit.Text = "Submit"
        Me.BtnSubmit.UseVisualStyleBackColor = True
        '
        'TxtSlices
        '
        Me.TxtSlices.Location = New System.Drawing.Point(126, 104)
        Me.TxtSlices.Name = "TxtSlices"
        Me.TxtSlices.Size = New System.Drawing.Size(100, 20)
        Me.TxtSlices.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(35, 107)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(85, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Number of slices"
        '
        'FrmTest
        '
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TxtSlices)
        Me.Controls.Add(Me.BtnSubmit)
        Me.Name = "FrmTest"
        Me.Size = New System.Drawing.Size(300, 300)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnSubmit As System.Windows.Forms.Button
    Friend WithEvents TxtSlices As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label

End Class
