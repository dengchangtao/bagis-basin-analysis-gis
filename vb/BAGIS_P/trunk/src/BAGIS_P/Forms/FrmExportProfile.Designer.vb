<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmExportProfile
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.BtnAddAoi = New System.Windows.Forms.Button()
        Me.GrdAoi = New System.Windows.Forms.DataGridView()
        Me.BtnRemove = New System.Windows.Forms.Button()
        Me.BtnExport = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.TxtProfileName = New System.Windows.Forms.TextBox()
        Me.LblCurrentProfile = New System.Windows.Forms.Label()
        Me.Aoi = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FilePath = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Copied = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.aoiPath = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.GrdAoi, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BtnAddAoi
        '
        Me.BtnAddAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAddAoi.Location = New System.Drawing.Point(9, 32)
        Me.BtnAddAoi.Name = "BtnAddAoi"
        Me.BtnAddAoi.Size = New System.Drawing.Size(105, 28)
        Me.BtnAddAoi.TabIndex = 58
        Me.BtnAddAoi.Text = "Add AOI"
        Me.BtnAddAoi.UseVisualStyleBackColor = True
        '
        'GrdAoi
        '
        Me.GrdAoi.AllowUserToAddRows = False
        Me.GrdAoi.AllowUserToDeleteRows = False
        Me.GrdAoi.AllowUserToOrderColumns = True
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.GrdAoi.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.GrdAoi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GrdAoi.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Aoi, Me.FilePath, Me.Copied, Me.aoiPath})
        Me.GrdAoi.Location = New System.Drawing.Point(9, 72)
        Me.GrdAoi.Name = "GrdAoi"
        Me.GrdAoi.ReadOnly = True
        Me.GrdAoi.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.GrdAoi.Size = New System.Drawing.Size(772, 180)
        Me.GrdAoi.TabIndex = 59
        '
        'BtnRemove
        '
        Me.BtnRemove.Enabled = False
        Me.BtnRemove.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnRemove.Location = New System.Drawing.Point(571, 268)
        Me.BtnRemove.Name = "BtnRemove"
        Me.BtnRemove.Size = New System.Drawing.Size(70, 25)
        Me.BtnRemove.TabIndex = 67
        Me.BtnRemove.Text = "Remove"
        Me.BtnRemove.UseVisualStyleBackColor = True
        '
        'BtnExport
        '
        Me.BtnExport.Enabled = False
        Me.BtnExport.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnExport.Location = New System.Drawing.Point(717, 268)
        Me.BtnExport.Name = "BtnExport"
        Me.BtnExport.Size = New System.Drawing.Size(64, 25)
        Me.BtnExport.TabIndex = 66
        Me.BtnExport.Text = "Export"
        Me.BtnExport.UseVisualStyleBackColor = True
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(647, 268)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(64, 25)
        Me.BtnCancel.TabIndex = 65
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
        '
        'TxtProfileName
        '
        Me.TxtProfileName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtProfileName.Location = New System.Drawing.Point(115, 7)
        Me.TxtProfileName.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtProfileName.Name = "TxtProfileName"
        Me.TxtProfileName.ReadOnly = True
        Me.TxtProfileName.Size = New System.Drawing.Size(285, 22)
        Me.TxtProfileName.TabIndex = 68
        '
        'LblCurrentProfile
        '
        Me.LblCurrentProfile.AutoSize = True
        Me.LblCurrentProfile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCurrentProfile.Location = New System.Drawing.Point(6, 9)
        Me.LblCurrentProfile.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblCurrentProfile.Name = "LblCurrentProfile"
        Me.LblCurrentProfile.Size = New System.Drawing.Size(109, 16)
        Me.LblCurrentProfile.TabIndex = 69
        Me.LblCurrentProfile.Text = "Current profile:"
        '
        'Aoi
        '
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Aoi.DefaultCellStyle = DataGridViewCellStyle2
        Me.Aoi.HeaderText = "Aoi"
        Me.Aoi.Name = "Aoi"
        Me.Aoi.ReadOnly = True
        Me.Aoi.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Aoi.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Aoi.Width = 125
        '
        'FilePath
        '
        Me.FilePath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FilePath.DefaultCellStyle = DataGridViewCellStyle3
        Me.FilePath.HeaderText = "File Path"
        Me.FilePath.Name = "FilePath"
        Me.FilePath.ReadOnly = True
        Me.FilePath.Width = 94
        '
        'Copied
        '
        Me.Copied.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Copied.DefaultCellStyle = DataGridViewCellStyle4
        Me.Copied.HeaderText = "Exported ?"
        Me.Copied.Name = "Copied"
        Me.Copied.ReadOnly = True
        Me.Copied.Width = 107
        '
        'aoiPath
        '
        Me.aoiPath.HeaderText = "aoiPath"
        Me.aoiPath.Name = "aoiPath"
        Me.aoiPath.ReadOnly = True
        Me.aoiPath.Visible = False
        Me.aoiPath.Width = 5
        '
        'FrmExportProfile
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(793, 304)
        Me.Controls.Add(Me.TxtProfileName)
        Me.Controls.Add(Me.LblCurrentProfile)
        Me.Controls.Add(Me.BtnRemove)
        Me.Controls.Add(Me.BtnExport)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.GrdAoi)
        Me.Controls.Add(Me.BtnAddAoi)
        Me.Name = "FrmExportProfile"
        Me.ShowIcon = False
        Me.Text = "Export profile"
        CType(Me.GrdAoi, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnAddAoi As System.Windows.Forms.Button
    Friend WithEvents GrdAoi As System.Windows.Forms.DataGridView
    Friend WithEvents BtnRemove As System.Windows.Forms.Button
    Friend WithEvents BtnExport As System.Windows.Forms.Button
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents TxtProfileName As System.Windows.Forms.TextBox
    Friend WithEvents LblCurrentProfile As System.Windows.Forms.Label
    Friend WithEvents Aoi As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents FilePath As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Copied As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents aoiPath As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
