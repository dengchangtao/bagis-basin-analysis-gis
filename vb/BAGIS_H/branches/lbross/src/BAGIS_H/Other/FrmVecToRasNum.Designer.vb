<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmVecToRasNum
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
        Me.TxtOutRas = New System.Windows.Forms.TextBox
        Me.CboAttField = New System.Windows.Forms.ComboBox
        Me.BtnApply = New System.Windows.Forms.Button
        Me.BtnCancel = New System.Windows.Forms.Button
        Me.LblAttribute = New System.Windows.Forms.Label
        Me.LblRaster = New System.Windows.Forms.Label
        Me.TxtCellSize = New System.Windows.Forms.TextBox
        Me.LblDftCellSize = New System.Windows.Forms.Label
        Me.LstVectorLayers = New System.Windows.Forms.ListBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.BtnAOISelect = New System.Windows.Forms.Button
        Me.LblCellUnit = New System.Windows.Forms.Label
        Me.TxtAoiPath = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'TxtOutRas
        '
        Me.TxtOutRas.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtOutRas.Location = New System.Drawing.Point(380, 131)
        Me.TxtOutRas.Name = "TxtOutRas"
        Me.TxtOutRas.Size = New System.Drawing.Size(151, 22)
        Me.TxtOutRas.TabIndex = 7
        '
        'CboAttField
        '
        Me.CboAttField.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CboAttField.FormattingEnabled = True
        Me.CboAttField.Location = New System.Drawing.Point(380, 69)
        Me.CboAttField.Name = "CboAttField"
        Me.CboAttField.Size = New System.Drawing.Size(167, 24)
        Me.CboAttField.TabIndex = 5
        '
        'BtnApply
        '
        Me.BtnApply.Enabled = False
        Me.BtnApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnApply.Location = New System.Drawing.Point(449, 171)
        Me.BtnApply.Name = "BtnApply"
        Me.BtnApply.Size = New System.Drawing.Size(97, 29)
        Me.BtnApply.TabIndex = 2
        Me.BtnApply.Text = "Apply"
        Me.BtnApply.UseVisualStyleBackColor = True
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(331, 171)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(88, 29)
        Me.BtnCancel.TabIndex = 3
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'LblAttribute
        '
        Me.LblAttribute.AutoSize = True
        Me.LblAttribute.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAttribute.Location = New System.Drawing.Point(240, 72)
        Me.LblAttribute.Name = "LblAttribute"
        Me.LblAttribute.Size = New System.Drawing.Size(134, 16)
        Me.LblAttribute.TabIndex = 6
        Me.LblAttribute.Text = "Vector Attribute Field:"
        '
        'LblRaster
        '
        Me.LblRaster.AutoSize = True
        Me.LblRaster.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRaster.Location = New System.Drawing.Point(282, 134)
        Me.LblRaster.Name = "LblRaster"
        Me.LblRaster.Size = New System.Drawing.Size(92, 16)
        Me.LblRaster.TabIndex = 7
        Me.LblRaster.Text = "Output Raster:"
        '
        'TxtCellSize
        '
        Me.TxtCellSize.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtCellSize.Location = New System.Drawing.Point(380, 101)
        Me.TxtCellSize.Name = "TxtCellSize"
        Me.TxtCellSize.Size = New System.Drawing.Size(94, 22)
        Me.TxtCellSize.TabIndex = 6
        '
        'LblDftCellSize
        '
        Me.LblDftCellSize.AutoSize = True
        Me.LblDftCellSize.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblDftCellSize.Location = New System.Drawing.Point(270, 104)
        Me.LblDftCellSize.Name = "LblDftCellSize"
        Me.LblDftCellSize.Size = New System.Drawing.Size(104, 16)
        Me.LblDftCellSize.TabIndex = 9
        Me.LblDftCellSize.Text = "Output Cell Size:"
        '
        'LstVectorLayers
        '
        Me.LstVectorLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstVectorLayers.FormattingEnabled = True
        Me.LstVectorLayers.ItemHeight = 16
        Me.LstVectorLayers.Location = New System.Drawing.Point(51, 70)
        Me.LstVectorLayers.Name = "LstVectorLayers"
        Me.LstVectorLayers.Size = New System.Drawing.Size(168, 132)
        Me.LstVectorLayers.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(48, 50)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(125, 16)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Vector layers in AOI"
        '
        'BtnAOISelect
        '
        Me.BtnAOISelect.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAOISelect.Location = New System.Drawing.Point(12, 12)
        Me.BtnAOISelect.Name = "BtnAOISelect"
        Me.BtnAOISelect.Size = New System.Drawing.Size(114, 30)
        Me.BtnAOISelect.TabIndex = 1
        Me.BtnAOISelect.Text = "Select AOI"
        Me.BtnAOISelect.UseVisualStyleBackColor = True
        '
        'LblCellUnit
        '
        Me.LblCellUnit.AutoSize = True
        Me.LblCellUnit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCellUnit.Location = New System.Drawing.Point(483, 104)
        Me.LblCellUnit.Name = "LblCellUnit"
        Me.LblCellUnit.Size = New System.Drawing.Size(63, 16)
        Me.LblCellUnit.TabIndex = 13
        Me.LblCellUnit.Text = "Unknown"
        '
        'TxtAoiPath
        '
        Me.TxtAoiPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtAoiPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtAoiPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtAoiPath.Location = New System.Drawing.Point(201, 16)
        Me.TxtAoiPath.Name = "TxtAoiPath"
        Me.TxtAoiPath.ReadOnly = True
        Me.TxtAoiPath.Size = New System.Drawing.Size(371, 22)
        Me.TxtAoiPath.TabIndex = 49
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(132, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 16)
        Me.Label2.TabIndex = 48
        Me.Label2.Text = "AOI Path:"
        '
        'FrmVecToRasNum
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(585, 218)
        Me.Controls.Add(Me.TxtAoiPath)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.LblCellUnit)
        Me.Controls.Add(Me.BtnAOISelect)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.LstVectorLayers)
        Me.Controls.Add(Me.LblDftCellSize)
        Me.Controls.Add(Me.TxtCellSize)
        Me.Controls.Add(Me.LblRaster)
        Me.Controls.Add(Me.LblAttribute)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.BtnApply)
        Me.Controls.Add(Me.CboAttField)
        Me.Controls.Add(Me.TxtOutRas)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "FrmVecToRasNum"
        Me.ShowIcon = False
        Me.Text = "Vector To Raster Conversion"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TxtOutRas As System.Windows.Forms.TextBox
    Friend WithEvents CboAttField As System.Windows.Forms.ComboBox
    Friend WithEvents BtnApply As System.Windows.Forms.Button
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents LblAttribute As System.Windows.Forms.Label
    Friend WithEvents LblRaster As System.Windows.Forms.Label
    Friend WithEvents TxtCellSize As System.Windows.Forms.TextBox
    Friend WithEvents LblDftCellSize As System.Windows.Forms.Label
    Friend WithEvents LstVectorLayers As System.Windows.Forms.ListBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents BtnAOISelect As System.Windows.Forms.Button
    Friend WithEvents LblCellUnit As System.Windows.Forms.Label
    Friend WithEvents TxtAoiPath As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class