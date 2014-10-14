<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSiteRepresentations
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
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.GrdExistingSites = New System.Windows.Forms.DataGridView()
        Me.Selected = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.ObjectId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Site_Type = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Site_Name = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.RasterName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Elevation = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Upper_Elev = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Lower_Elev = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DefaultElevation = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LblLowerRange = New System.Windows.Forms.Label()
        Me.LblUpperRange = New System.Windows.Forms.Label()
        Me.LblBufferDistance = New System.Windows.Forms.Label()
        Me.TxtLowerRange = New System.Windows.Forms.TextBox()
        Me.TxtUpperRange = New System.Windows.Forms.TextBox()
        Me.TxtBufferDistance = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ChkAutoZoom = New System.Windows.Forms.CheckBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ChkOnlyOne = New System.Windows.Forms.CheckBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.LblDistanceUnit = New System.Windows.Forms.Label()
        Me.LblZUnit = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TxtRasterFolder = New System.Windows.Forms.TextBox()
        CType(Me.GrdExistingSites, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BtnClose
        '
        Me.BtnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClose.Location = New System.Drawing.Point(410, 94)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(96, 37)
        Me.BtnClose.TabIndex = 0
        Me.BtnClose.Text = "Close"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'GrdExistingSites
        '
        Me.GrdExistingSites.AllowUserToAddRows = False
        Me.GrdExistingSites.AllowUserToDeleteRows = False
        Me.GrdExistingSites.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GrdExistingSites.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Selected, Me.ObjectId, Me.Site_Type, Me.Site_Name, Me.RasterName, Me.Elevation, Me.Upper_Elev, Me.Lower_Elev, Me.DefaultElevation})
        Me.GrdExistingSites.Location = New System.Drawing.Point(8, 199)
        Me.GrdExistingSites.Name = "GrdExistingSites"
        Me.GrdExistingSites.Size = New System.Drawing.Size(527, 387)
        Me.GrdExistingSites.TabIndex = 1
        '
        'Selected
        '
        Me.Selected.HeaderText = "□"
        Me.Selected.Name = "Selected"
        Me.Selected.ReadOnly = True
        Me.Selected.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Selected.Width = 30
        '
        'ObjectId
        '
        Me.ObjectId.HeaderText = "ObjectId"
        Me.ObjectId.Name = "ObjectId"
        Me.ObjectId.Visible = False
        '
        'Site_Type
        '
        Me.Site_Type.HeaderText = "Type"
        Me.Site_Type.Name = "Site_Type"
        Me.Site_Type.ReadOnly = True
        Me.Site_Type.Width = 70
        '
        'Site_Name
        '
        Me.Site_Name.HeaderText = "Name"
        Me.Site_Name.Name = "Site_Name"
        Me.Site_Name.ReadOnly = True
        Me.Site_Name.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'RasterName
        '
        Me.RasterName.HeaderText = "Raster Name"
        Me.RasterName.Name = "RasterName"
        Me.RasterName.ReadOnly = True
        Me.RasterName.Width = 75
        '
        'Elevation
        '
        Me.Elevation.HeaderText = "Elevation"
        Me.Elevation.Name = "Elevation"
        Me.Elevation.ReadOnly = True
        Me.Elevation.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Elevation.Width = 70
        '
        'Upper_Elev
        '
        Me.Upper_Elev.HeaderText = "Upper Elevation"
        Me.Upper_Elev.Name = "Upper_Elev"
        Me.Upper_Elev.ReadOnly = True
        Me.Upper_Elev.Width = 70
        '
        'Lower_Elev
        '
        Me.Lower_Elev.HeaderText = "Lower Elevation"
        Me.Lower_Elev.Name = "Lower_Elev"
        Me.Lower_Elev.ReadOnly = True
        Me.Lower_Elev.Width = 70
        '
        'DefaultElevation
        '
        Me.DefaultElevation.HeaderText = "Default Elevation"
        Me.DefaultElevation.Name = "DefaultElevation"
        Me.DefaultElevation.Visible = False
        '
        'LblLowerRange
        '
        Me.LblLowerRange.AutoSize = True
        Me.LblLowerRange.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLowerRange.ForeColor = System.Drawing.Color.Black
        Me.LblLowerRange.Location = New System.Drawing.Point(19, 92)
        Me.LblLowerRange.Name = "LblLowerRange"
        Me.LblLowerRange.Size = New System.Drawing.Size(118, 16)
        Me.LblLowerRange.TabIndex = 76
        Me.LblLowerRange.Text = "Elev Lower Range"
        '
        'LblUpperRange
        '
        Me.LblUpperRange.AutoSize = True
        Me.LblUpperRange.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblUpperRange.ForeColor = System.Drawing.Color.Black
        Me.LblUpperRange.Location = New System.Drawing.Point(17, 65)
        Me.LblUpperRange.Name = "LblUpperRange"
        Me.LblUpperRange.Size = New System.Drawing.Size(120, 16)
        Me.LblUpperRange.TabIndex = 75
        Me.LblUpperRange.Text = "Elev Upper Range"
        '
        'LblBufferDistance
        '
        Me.LblBufferDistance.AutoSize = True
        Me.LblBufferDistance.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblBufferDistance.ForeColor = System.Drawing.Color.Black
        Me.LblBufferDistance.Location = New System.Drawing.Point(39, 40)
        Me.LblBufferDistance.Name = "LblBufferDistance"
        Me.LblBufferDistance.Size = New System.Drawing.Size(98, 16)
        Me.LblBufferDistance.TabIndex = 74
        Me.LblBufferDistance.Text = "Buffer Distance"
        '
        'TxtLowerRange
        '
        Me.TxtLowerRange.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtLowerRange.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtLowerRange.Location = New System.Drawing.Point(143, 91)
        Me.TxtLowerRange.Name = "TxtLowerRange"
        Me.TxtLowerRange.ReadOnly = True
        Me.TxtLowerRange.Size = New System.Drawing.Size(78, 20)
        Me.TxtLowerRange.TabIndex = 73
        Me.TxtLowerRange.Text = "61"
        Me.TxtLowerRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TxtUpperRange
        '
        Me.TxtUpperRange.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtUpperRange.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtUpperRange.Location = New System.Drawing.Point(143, 65)
        Me.TxtUpperRange.Name = "TxtUpperRange"
        Me.TxtUpperRange.ReadOnly = True
        Me.TxtUpperRange.Size = New System.Drawing.Size(78, 20)
        Me.TxtUpperRange.TabIndex = 72
        Me.TxtUpperRange.Text = "-1"
        Me.TxtUpperRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TxtBufferDistance
        '
        Me.TxtBufferDistance.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtBufferDistance.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtBufferDistance.Location = New System.Drawing.Point(143, 39)
        Me.TxtBufferDistance.Name = "TxtBufferDistance"
        Me.TxtBufferDistance.ReadOnly = True
        Me.TxtBufferDistance.Size = New System.Drawing.Size(78, 20)
        Me.TxtBufferDistance.TabIndex = 71
        Me.TxtBufferDistance.Text = "10"
        Me.TxtBufferDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(45, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(190, 20)
        Me.Label1.TabIndex = 70
        Me.Label1.Text = "Representation Definition"
        '
        'ChkAutoZoom
        '
        Me.ChkAutoZoom.AutoSize = True
        Me.ChkAutoZoom.Checked = True
        Me.ChkAutoZoom.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkAutoZoom.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkAutoZoom.Location = New System.Drawing.Point(312, 32)
        Me.ChkAutoZoom.Name = "ChkAutoZoom"
        Me.ChkAutoZoom.Size = New System.Drawing.Size(107, 24)
        Me.ChkAutoZoom.TabIndex = 79
        Me.ChkAutoZoom.Text = "Auto Zoom"
        Me.ChkAutoZoom.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(13, 146)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(335, 16)
        Me.Label2.TabIndex = 81
        Me.Label2.Text = "Double-click on a site to display its representation map."
        '
        'ChkOnlyOne
        '
        Me.ChkOnlyOne.AutoSize = True
        Me.ChkOnlyOne.Checked = True
        Me.ChkOnlyOne.CheckState = System.Windows.Forms.CheckState.Checked
        Me.ChkOnlyOne.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkOnlyOne.Location = New System.Drawing.Point(312, 57)
        Me.ChkOnlyOne.Name = "ChkOnlyOne"
        Me.ChkOnlyOne.Size = New System.Drawing.Size(204, 24)
        Me.ChkOnlyOne.TabIndex = 82
        Me.ChkOnlyOne.Text = "Clear maps of other sites"
        Me.ChkOnlyOne.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(13, 126)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(374, 16)
        Me.Label3.TabIndex = 83
        Me.Label3.Text = "Site representation maps are available for selected sites only."
        '
        'LblDistanceUnit
        '
        Me.LblDistanceUnit.AutoSize = True
        Me.LblDistanceUnit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblDistanceUnit.Location = New System.Drawing.Point(227, 40)
        Me.LblDistanceUnit.Name = "LblDistanceUnit"
        Me.LblDistanceUnit.Size = New System.Drawing.Size(27, 16)
        Me.LblDistanceUnit.TabIndex = 84
        Me.LblDistanceUnit.Text = "Km"
        '
        'LblZUnit
        '
        Me.LblZUnit.AutoSize = True
        Me.LblZUnit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblZUnit.Location = New System.Drawing.Point(227, 79)
        Me.LblZUnit.Name = "LblZUnit"
        Me.LblZUnit.Size = New System.Drawing.Size(49, 16)
        Me.LblZUnit.TabIndex = 85
        Me.LblZUnit.Text = "Meters"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(14, 173)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(93, 16)
        Me.Label4.TabIndex = 86
        Me.Label4.Text = "Raster Folder:"
        '
        'TxtRasterFolder
        '
        Me.TxtRasterFolder.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtRasterFolder.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtRasterFolder.ForeColor = System.Drawing.Color.Blue
        Me.TxtRasterFolder.Location = New System.Drawing.Point(113, 170)
        Me.TxtRasterFolder.Name = "TxtRasterFolder"
        Me.TxtRasterFolder.Size = New System.Drawing.Size(422, 22)
        Me.TxtRasterFolder.TabIndex = 87
        Me.TxtRasterFolder.Text = "file path"
        '
        'frmSiteRepresentations
        '
        Me.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.Controls.Add(Me.TxtRasterFolder)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.LblZUnit)
        Me.Controls.Add(Me.LblDistanceUnit)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ChkOnlyOne)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ChkAutoZoom)
        Me.Controls.Add(Me.LblLowerRange)
        Me.Controls.Add(Me.LblUpperRange)
        Me.Controls.Add(Me.LblBufferDistance)
        Me.Controls.Add(Me.TxtLowerRange)
        Me.Controls.Add(Me.TxtUpperRange)
        Me.Controls.Add(Me.TxtBufferDistance)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.GrdExistingSites)
        Me.Controls.Add(Me.BtnClose)
        Me.Name = "frmSiteRepresentations"
        Me.Size = New System.Drawing.Size(543, 599)
        CType(Me.GrdExistingSites, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnClose As System.Windows.Forms.Button
    Friend WithEvents GrdExistingSites As System.Windows.Forms.DataGridView
    Friend WithEvents LblLowerRange As System.Windows.Forms.Label
    Friend WithEvents LblUpperRange As System.Windows.Forms.Label
    Friend WithEvents LblBufferDistance As System.Windows.Forms.Label
    Friend WithEvents TxtLowerRange As System.Windows.Forms.TextBox
    Friend WithEvents TxtUpperRange As System.Windows.Forms.TextBox
    Friend WithEvents TxtBufferDistance As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ChkAutoZoom As System.Windows.Forms.CheckBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ChkOnlyOne As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents LblDistanceUnit As System.Windows.Forms.Label
    Friend WithEvents LblZUnit As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TxtRasterFolder As System.Windows.Forms.TextBox
    Friend WithEvents Selected As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents ObjectId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Site_Type As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Site_Name As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents RasterName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Elevation As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Upper_Elev As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Lower_Elev As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DefaultElevation As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
