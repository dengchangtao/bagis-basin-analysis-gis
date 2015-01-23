<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmAoiViewer
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
        Me.BtnAOI = New System.Windows.Forms.Button
        Me.LstAoiRasterLayers = New System.Windows.Forms.ListBox
        Me.LstAoiVectorLayers = New System.Windows.Forms.ListBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.LstDemLayers = New System.Windows.Forms.ListBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.LblPrismLayers = New System.Windows.Forms.Label
        Me.LstPrismLayers = New System.Windows.Forms.ListBox
        Me.BtnClearSelected = New System.Windows.Forms.Button
        Me.BtnViewAoi = New System.Windows.Forms.Button
        Me.TxtAoiPath = New System.Windows.Forms.TextBox
        Me.BtnImport = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Button2 = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'BtnAOI
        '
        Me.BtnAOI.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAOI.Location = New System.Drawing.Point(11, 7)
        Me.BtnAOI.Name = "BtnAOI"
        Me.BtnAOI.Size = New System.Drawing.Size(119, 30)
        Me.BtnAOI.TabIndex = 1
        Me.BtnAOI.Text = "Select AOI"
        Me.BtnAOI.UseVisualStyleBackColor = True
        '
        'LstAoiRasterLayers
        '
        Me.LstAoiRasterLayers.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.LstAoiRasterLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstAoiRasterLayers.FormattingEnabled = True
        Me.LstAoiRasterLayers.ItemHeight = 16
        Me.LstAoiRasterLayers.Location = New System.Drawing.Point(138, 96)
        Me.LstAoiRasterLayers.Name = "LstAoiRasterLayers"
        Me.LstAoiRasterLayers.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.LstAoiRasterLayers.Size = New System.Drawing.Size(140, 132)
        Me.LstAoiRasterLayers.TabIndex = 5
        '
        'LstAoiVectorLayers
        '
        Me.LstAoiVectorLayers.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.LstAoiVectorLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstAoiVectorLayers.FormattingEnabled = True
        Me.LstAoiVectorLayers.ItemHeight = 16
        Me.LstAoiVectorLayers.Location = New System.Drawing.Point(284, 96)
        Me.LstAoiVectorLayers.Name = "LstAoiVectorLayers"
        Me.LstAoiVectorLayers.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.LstAoiVectorLayers.Size = New System.Drawing.Size(140, 132)
        Me.LstAoiVectorLayers.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(162, 77)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(95, 16)
        Me.Label2.TabIndex = 51
        Me.Label2.Text = "Raster Grids"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(281, 77)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(131, 16)
        Me.Label3.TabIndex = 52
        Me.Label3.Text = "Vector Shapefiles"
        '
        'LstDemLayers
        '
        Me.LstDemLayers.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.LstDemLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstDemLayers.FormattingEnabled = True
        Me.LstDemLayers.ItemHeight = 16
        Me.LstDemLayers.Location = New System.Drawing.Point(430, 96)
        Me.LstDemLayers.Name = "LstDemLayers"
        Me.LstDemLayers.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.LstDemLayers.Size = New System.Drawing.Size(140, 132)
        Me.LstDemLayers.TabIndex = 7
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(459, 77)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 16)
        Me.Label4.TabIndex = 54
        Me.Label4.Text = "DEM Grids"
        '
        'LblPrismLayers
        '
        Me.LblPrismLayers.AutoSize = True
        Me.LblPrismLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblPrismLayers.Location = New System.Drawing.Point(573, 79)
        Me.LblPrismLayers.Name = "LblPrismLayers"
        Me.LblPrismLayers.Size = New System.Drawing.Size(145, 16)
        Me.LblPrismLayers.TabIndex = 56
        Me.LblPrismLayers.Text = "PRISM Precip Grids"
        '
        'LstPrismLayers
        '
        Me.LstPrismLayers.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.LstPrismLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LstPrismLayers.FormattingEnabled = True
        Me.LstPrismLayers.ItemHeight = 16
        Me.LstPrismLayers.Location = New System.Drawing.Point(576, 98)
        Me.LstPrismLayers.Name = "LstPrismLayers"
        Me.LstPrismLayers.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.LstPrismLayers.Size = New System.Drawing.Size(140, 132)
        Me.LstPrismLayers.TabIndex = 8
        '
        'BtnClearSelected
        '
        Me.BtnClearSelected.Enabled = False
        Me.BtnClearSelected.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClearSelected.Location = New System.Drawing.Point(11, 164)
        Me.BtnClearSelected.Name = "BtnClearSelected"
        Me.BtnClearSelected.Size = New System.Drawing.Size(119, 30)
        Me.BtnClearSelected.TabIndex = 3
        Me.BtnClearSelected.Text = "Clear Selected"
        Me.BtnClearSelected.UseVisualStyleBackColor = True
        '
        'BtnViewAoi
        '
        Me.BtnViewAoi.Enabled = False
        Me.BtnViewAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnViewAoi.Location = New System.Drawing.Point(11, 128)
        Me.BtnViewAoi.Name = "BtnViewAoi"
        Me.BtnViewAoi.Size = New System.Drawing.Size(119, 30)
        Me.BtnViewAoi.TabIndex = 2
        Me.BtnViewAoi.Text = "View Selected"
        Me.BtnViewAoi.UseVisualStyleBackColor = True
        '
        'TxtAoiPath
        '
        Me.TxtAoiPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtAoiPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtAoiPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtAoiPath.Location = New System.Drawing.Point(210, 11)
        Me.TxtAoiPath.Name = "TxtAoiPath"
        Me.TxtAoiPath.ReadOnly = True
        Me.TxtAoiPath.Size = New System.Drawing.Size(508, 22)
        Me.TxtAoiPath.TabIndex = 59
        Me.TxtAoiPath.TabStop = False
        '
        'BtnImport
        '
        Me.BtnImport.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnImport.Location = New System.Drawing.Point(12, 54)
        Me.BtnImport.Name = "BtnImport"
        Me.BtnImport.Size = New System.Drawing.Size(119, 30)
        Me.BtnImport.TabIndex = 4
        Me.BtnImport.Text = "Import"
        Me.BtnImport.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(143, 14)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(63, 16)
        Me.Label5.TabIndex = 61
        Me.Label5.Text = "AOI Path:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 96)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(108, 20)
        Me.Label1.TabIndex = 62
        Me.Label1.Text = "Layers in AOI:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Button2
        '
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.Location = New System.Drawing.Point(12, 200)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(119, 30)
        Me.Button2.TabIndex = 63
        Me.Button2.Text = "Close"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'FrmAoiViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(730, 240)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.BtnImport)
        Me.Controls.Add(Me.TxtAoiPath)
        Me.Controls.Add(Me.BtnClearSelected)
        Me.Controls.Add(Me.BtnViewAoi)
        Me.Controls.Add(Me.LblPrismLayers)
        Me.Controls.Add(Me.LstPrismLayers)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.LstDemLayers)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.LstAoiVectorLayers)
        Me.Controls.Add(Me.LstAoiRasterLayers)
        Me.Controls.Add(Me.BtnAOI)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "FrmAoiViewer"
        Me.ShowIcon = False
        Me.Text = "AOI Layers Viewer"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnAOI As System.Windows.Forms.Button
    Friend WithEvents LstAoiRasterLayers As System.Windows.Forms.ListBox
    Friend WithEvents LstAoiVectorLayers As System.Windows.Forms.ListBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents LstDemLayers As System.Windows.Forms.ListBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents LblPrismLayers As System.Windows.Forms.Label
    Friend WithEvents LstPrismLayers As System.Windows.Forms.ListBox
    Friend WithEvents BtnClearSelected As System.Windows.Forms.Button
    Friend WithEvents BtnViewAoi As System.Windows.Forms.Button
    Friend WithEvents TxtAoiPath As System.Windows.Forms.TextBox
    Friend WithEvents BtnImport As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
End Class