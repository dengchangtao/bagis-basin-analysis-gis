<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBasin_Tool
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
        Me.CmdSelectBasinFolder = New System.Windows.Forms.Button()
        Me.txtBasinFolder = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.CmdViewDEMExtent = New System.Windows.Forms.Button()
        Me.CmdViewRasters = New System.Windows.Forms.Button()
        Me.CmdSelectBasin = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblAOIStatus = New System.Windows.Forms.TextBox()
        Me.lblDEMStatus = New System.Windows.Forms.TextBox()
        Me.CmdExit = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lstvBasinDEM = New System.Windows.Forms.ListView()
        Me.Subfolders = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.gdbDEMStatus = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.gdbAOIStatus = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.weaselDEMStatus = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.weaselAOIStatus = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblWeaselDEMStatus = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblWeaselAOIStatus = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'CmdSelectBasinFolder
        '
        Me.CmdSelectBasinFolder.Location = New System.Drawing.Point(21, 12)
        Me.CmdSelectBasinFolder.Name = "CmdSelectBasinFolder"
        Me.CmdSelectBasinFolder.Size = New System.Drawing.Size(75, 23)
        Me.CmdSelectBasinFolder.TabIndex = 0
        Me.CmdSelectBasinFolder.Text = "Open"
        Me.CmdSelectBasinFolder.UseVisualStyleBackColor = True
        '
        'txtBasinFolder
        '
        Me.txtBasinFolder.BackColor = System.Drawing.SystemColors.Menu
        Me.txtBasinFolder.Location = New System.Drawing.Point(106, 15)
        Me.txtBasinFolder.Name = "txtBasinFolder"
        Me.txtBasinFolder.Size = New System.Drawing.Size(473, 20)
        Me.txtBasinFolder.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(19, 51)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(108, 16)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "File GDB BASIN:"
        '
        'CmdViewDEMExtent
        '
        Me.CmdViewDEMExtent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdViewDEMExtent.Location = New System.Drawing.Point(467, 43)
        Me.CmdViewDEMExtent.Name = "CmdViewDEMExtent"
        Me.CmdViewDEMExtent.Size = New System.Drawing.Size(112, 29)
        Me.CmdViewDEMExtent.TabIndex = 0
        Me.CmdViewDEMExtent.Text = "View DEM Extent"
        Me.CmdViewDEMExtent.UseVisualStyleBackColor = True
        '
        'CmdViewRasters
        '
        Me.CmdViewRasters.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdViewRasters.Location = New System.Drawing.Point(467, 78)
        Me.CmdViewRasters.Name = "CmdViewRasters"
        Me.CmdViewRasters.Size = New System.Drawing.Size(112, 29)
        Me.CmdViewRasters.TabIndex = 0
        Me.CmdViewRasters.Text = "View Layers"
        Me.CmdViewRasters.UseVisualStyleBackColor = True
        '
        'CmdSelectBasin
        '
        Me.CmdSelectBasin.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdSelectBasin.Location = New System.Drawing.Point(592, 7)
        Me.CmdSelectBasin.Name = "CmdSelectBasin"
        Me.CmdSelectBasin.Size = New System.Drawing.Size(75, 31)
        Me.CmdSelectBasin.TabIndex = 0
        Me.CmdSelectBasin.Text = "Select"
        Me.CmdSelectBasin.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(321, 52)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(33, 16)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "AOI:"
        '
        'lblAOIStatus
        '
        Me.lblAOIStatus.BackColor = System.Drawing.SystemColors.Menu
        Me.lblAOIStatus.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblAOIStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAOIStatus.ForeColor = System.Drawing.Color.Blue
        Me.lblAOIStatus.Location = New System.Drawing.Point(360, 54)
        Me.lblAOIStatus.Name = "lblAOIStatus"
        Me.lblAOIStatus.ReadOnly = True
        Me.lblAOIStatus.Size = New System.Drawing.Size(69, 13)
        Me.lblAOIStatus.TabIndex = 4
        Me.lblAOIStatus.Text = "No"
        '
        'lblDEMStatus
        '
        Me.lblDEMStatus.BackColor = System.Drawing.SystemColors.Menu
        Me.lblDEMStatus.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblDEMStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDEMStatus.ForeColor = System.Drawing.Color.Blue
        Me.lblDEMStatus.Location = New System.Drawing.Point(127, 54)
        Me.lblDEMStatus.Name = "lblDEMStatus"
        Me.lblDEMStatus.ReadOnly = True
        Me.lblDEMStatus.Size = New System.Drawing.Size(188, 13)
        Me.lblDEMStatus.TabIndex = 4
        Me.lblDEMStatus.Text = "No"
        '
        'CmdExit
        '
        Me.CmdExit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdExit.Location = New System.Drawing.Point(592, 65)
        Me.CmdExit.Name = "CmdExit"
        Me.CmdExit.Size = New System.Drawing.Size(75, 29)
        Me.CmdExit.TabIndex = 0
        Me.CmdExit.Text = "Close"
        Me.CmdExit.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.MediumBlue
        Me.Label6.Location = New System.Drawing.Point(21, 387)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(215, 16)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "Double-click to change folder."
        '
        'lstvBasinDEM
        '
        Me.lstvBasinDEM.BackColor = System.Drawing.Color.White
        Me.lstvBasinDEM.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Subfolders, Me.gdbDEMStatus, Me.gdbAOIStatus, Me.weaselDEMStatus, Me.weaselAOIStatus})
        Me.lstvBasinDEM.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lstvBasinDEM.FullRowSelect = True
        Me.lstvBasinDEM.Location = New System.Drawing.Point(24, 116)
        Me.lstvBasinDEM.MultiSelect = False
        Me.lstvBasinDEM.Name = "lstvBasinDEM"
        Me.lstvBasinDEM.Size = New System.Drawing.Size(641, 268)
        Me.lstvBasinDEM.TabIndex = 6
        Me.lstvBasinDEM.UseCompatibleStateImageBehavior = False
        Me.lstvBasinDEM.View = System.Windows.Forms.View.Details
        '
        'Subfolders
        '
        Me.Subfolders.Text = "Subfolders"
        Me.Subfolders.Width = 125
        '
        'gdbDEMStatus
        '
        Me.gdbDEMStatus.Text = "FGDB BASIN Status"
        Me.gdbDEMStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.gdbDEMStatus.Width = 157
        '
        'gdbAOIStatus
        '
        Me.gdbAOIStatus.Text = "FGDB AOI Status"
        Me.gdbAOIStatus.Width = 110
        '
        'weaselDEMStatus
        '
        Me.weaselDEMStatus.Text = "Weasel BASIN Status"
        Me.weaselDEMStatus.Width = 127
        '
        'weaselAOIStatus
        '
        Me.weaselAOIStatus.Text = "Weasel AOI Status"
        Me.weaselAOIStatus.Width = 108
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(26, 81)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(101, 16)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Weasel BASIN:"
        '
        'lblWeaselDEMStatus
        '
        Me.lblWeaselDEMStatus.BackColor = System.Drawing.SystemColors.Menu
        Me.lblWeaselDEMStatus.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblWeaselDEMStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWeaselDEMStatus.ForeColor = System.Drawing.Color.Blue
        Me.lblWeaselDEMStatus.Location = New System.Drawing.Point(127, 81)
        Me.lblWeaselDEMStatus.Name = "lblWeaselDEMStatus"
        Me.lblWeaselDEMStatus.Size = New System.Drawing.Size(188, 13)
        Me.lblWeaselDEMStatus.TabIndex = 8
        Me.lblWeaselDEMStatus.Text = "No"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(321, 81)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(33, 16)
        Me.Label4.TabIndex = 9
        Me.Label4.Text = "AOI:"
        '
        'lblWeaselAOIStatus
        '
        Me.lblWeaselAOIStatus.BackColor = System.Drawing.SystemColors.Menu
        Me.lblWeaselAOIStatus.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblWeaselAOIStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWeaselAOIStatus.ForeColor = System.Drawing.Color.Blue
        Me.lblWeaselAOIStatus.Location = New System.Drawing.Point(360, 83)
        Me.lblWeaselAOIStatus.Name = "lblWeaselAOIStatus"
        Me.lblWeaselAOIStatus.Size = New System.Drawing.Size(69, 13)
        Me.lblWeaselAOIStatus.TabIndex = 10
        Me.lblWeaselAOIStatus.Text = "No"
        '
        'frmBasin_Tool
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(675, 412)
        Me.Controls.Add(Me.lblWeaselAOIStatus)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblWeaselDEMStatus)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lstvBasinDEM)
        Me.Controls.Add(Me.lblDEMStatus)
        Me.Controls.Add(Me.lblAOIStatus)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtBasinFolder)
        Me.Controls.Add(Me.CmdExit)
        Me.Controls.Add(Me.CmdViewRasters)
        Me.Controls.Add(Me.CmdViewDEMExtent)
        Me.Controls.Add(Me.CmdSelectBasin)
        Me.Controls.Add(Me.CmdSelectBasinFolder)
        Me.Name = "frmBasin_Tool"
        Me.ShowIcon = False
        Me.Text = "Basin Tool"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents CmdSelectBasinFolder As System.Windows.Forms.Button
    Friend WithEvents txtBasinFolder As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CmdViewDEMExtent As System.Windows.Forms.Button
    Friend WithEvents CmdViewRasters As System.Windows.Forms.Button
    Friend WithEvents CmdSelectBasin As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblAOIStatus As System.Windows.Forms.TextBox
    Friend WithEvents lblDEMStatus As System.Windows.Forms.TextBox
    Friend WithEvents CmdExit As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lstvBasinDEM As System.Windows.Forms.ListView
    Friend WithEvents Subfolders As System.Windows.Forms.ColumnHeader
    Friend WithEvents gdbDEMStatus As System.Windows.Forms.ColumnHeader
    Friend WithEvents gdbAOIStatus As System.Windows.Forms.ColumnHeader
    Friend WithEvents weaselDEMStatus As System.Windows.Forms.ColumnHeader
    Friend WithEvents weaselAOIStatus As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lblWeaselDEMStatus As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblWeaselAOIStatus As System.Windows.Forms.TextBox
End Class
