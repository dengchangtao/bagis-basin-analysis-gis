<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmClipDataSource
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
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.BtnAddAoi = New System.Windows.Forms.Button()
        Me.GrdAoi = New System.Windows.Forms.DataGridView()
        Me.BtnRemove = New System.Windows.Forms.Button()
        Me.BtnClip = New System.Windows.Forms.Button()
        Me.BtnCancel = New System.Windows.Forms.Button()
        Me.Aoi = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SourceName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.FilePath = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Clipped = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ErrorMessage = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.aoiPath = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.GrdAoi, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BtnAddAoi
        '
        Me.BtnAddAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAddAoi.Location = New System.Drawing.Point(9, 9)
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
        Me.GrdAoi.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Aoi, Me.SourceName, Me.FilePath, Me.Clipped, Me.ErrorMessage, Me.aoiPath})
        Me.GrdAoi.Location = New System.Drawing.Point(9, 49)
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
        Me.BtnRemove.Location = New System.Drawing.Point(571, 245)
        Me.BtnRemove.Name = "BtnRemove"
        Me.BtnRemove.Size = New System.Drawing.Size(70, 25)
        Me.BtnRemove.TabIndex = 67
        Me.BtnRemove.Text = "Remove"
        Me.BtnRemove.UseVisualStyleBackColor = True
        '
        'BtnClip
        '
        Me.BtnClip.Enabled = False
        Me.BtnClip.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClip.Location = New System.Drawing.Point(717, 245)
        Me.BtnClip.Name = "BtnClip"
        Me.BtnClip.Size = New System.Drawing.Size(64, 25)
        Me.BtnClip.TabIndex = 66
        Me.BtnClip.Text = "Clip"
        Me.BtnClip.UseVisualStyleBackColor = True
        '
        'BtnCancel
        '
        Me.BtnCancel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCancel.Location = New System.Drawing.Point(647, 245)
        Me.BtnCancel.Name = "BtnCancel"
        Me.BtnCancel.Size = New System.Drawing.Size(64, 25)
        Me.BtnCancel.TabIndex = 65
        Me.BtnCancel.Text = "Cancel"
        Me.BtnCancel.UseVisualStyleBackColor = True
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
        'SourceName
        '
        Me.SourceName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black
        Me.SourceName.DefaultCellStyle = DataGridViewCellStyle3
        Me.SourceName.HeaderText = "Name"
        Me.SourceName.Name = "SourceName"
        Me.SourceName.ReadOnly = True
        Me.SourceName.Width = 74
        '
        'FilePath
        '
        Me.FilePath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FilePath.DefaultCellStyle = DataGridViewCellStyle4
        Me.FilePath.HeaderText = "File Path"
        Me.FilePath.Name = "FilePath"
        Me.FilePath.ReadOnly = True
        Me.FilePath.Width = 94
        '
        'Clipped
        '
        Me.Clipped.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Clipped.DefaultCellStyle = DataGridViewCellStyle5
        Me.Clipped.HeaderText = "Clipped?"
        Me.Clipped.Name = "Clipped"
        Me.Clipped.ReadOnly = True
        Me.Clipped.Width = 95
        '
        'ErrorMessage
        '
        Me.ErrorMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ErrorMessage.DefaultCellStyle = DataGridViewCellStyle6
        Me.ErrorMessage.HeaderText = "Error Message"
        Me.ErrorMessage.Name = "ErrorMessage"
        Me.ErrorMessage.ReadOnly = True
        Me.ErrorMessage.Width = 124
        '
        'aoiPath
        '
        Me.aoiPath.HeaderText = "aoiPath"
        Me.aoiPath.Name = "aoiPath"
        Me.aoiPath.ReadOnly = True
        Me.aoiPath.Visible = False
        Me.aoiPath.Width = 5
        '
        'FrmClipDataSource
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(793, 273)
        Me.Controls.Add(Me.BtnRemove)
        Me.Controls.Add(Me.BtnClip)
        Me.Controls.Add(Me.BtnCancel)
        Me.Controls.Add(Me.GrdAoi)
        Me.Controls.Add(Me.BtnAddAoi)
        Me.Name = "FrmClipDataSource"
        Me.ShowIcon = False
        Me.Text = "Clip data source(s)"
        CType(Me.GrdAoi, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BtnAddAoi As System.Windows.Forms.Button
    Friend WithEvents GrdAoi As System.Windows.Forms.DataGridView
    Friend WithEvents BtnRemove As System.Windows.Forms.Button
    Friend WithEvents BtnClip As System.Windows.Forms.Button
    Friend WithEvents BtnCancel As System.Windows.Forms.Button
    Friend WithEvents Aoi As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SourceName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents FilePath As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Clipped As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ErrorMessage As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents aoiPath As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
