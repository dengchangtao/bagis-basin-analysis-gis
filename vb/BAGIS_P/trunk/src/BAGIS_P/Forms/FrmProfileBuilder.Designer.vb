<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmProfileBuilder
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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle17 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle18 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle19 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle20 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle21 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.TxtAoiPath = New System.Windows.Forms.TextBox()
        Me.BtnSelectAoi = New System.Windows.Forms.Button()
        Me.LblHruLayers = New System.Windows.Forms.Label()
        Me.GrdProfiles = New System.Windows.Forms.DataGridView()
        Me.Profiles = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BtnEditProfile = New System.Windows.Forms.Button()
        Me.BtnProfileCopy = New System.Windows.Forms.Button()
        Me.BtnProfileDelete = New System.Windows.Forms.Button()
        Me.BtnProfileNew = New System.Windows.Forms.Button()
        Me.BtnEditMethod = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.TxtModelName = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TxtToolboxName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtSelMethod = New System.Windows.Forms.TextBox()
        Me.TxtDescription = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TxtNumMethods = New System.Windows.Forms.TextBox()
        Me.LblCurrentProfile = New System.Windows.Forms.Label()
        Me.TxtProfileName = New System.Windows.Forms.TextBox()
        Me.BtnApply = New System.Windows.Forms.Button()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.GrdMethods = New System.Windows.Forms.DataGridView()
        Me.Methods = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ColStatus = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.IncludeMethod = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.BtnAddMethod = New System.Windows.Forms.Button()
        Me.BtnMethodDelete = New System.Windows.Forms.Button()
        Me.BtnMethodNew = New System.Windows.Forms.Button()
        Me.BtnVerify = New System.Windows.Forms.Button()
        Me.BtnCalculate = New System.Windows.Forms.Button()
        Me.PnlProfile = New System.Windows.Forms.Panel()
        Me.LblStatus = New System.Windows.Forms.Label()
        Me.BtnExportProfile = New System.Windows.Forms.Button()
        Me.BtnImport = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GrdHruLayers = New System.Windows.Forms.DataGridView()
        Me.HruName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Completed = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.GrdCompleteProfiles = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.PnlSubAoi = New System.Windows.Forms.Panel()
        Me.RichTextBox1 = New System.Windows.Forms.RichTextBox()
        Me.TxtSubAoiCount = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.CboSubAoiId = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.CkAppendSubAoi = New System.Windows.Forms.CheckBox()
        Me.TxtLblPath = New System.Windows.Forms.TextBox()
        Me.TxtNoData = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.BtnViewLog = New System.Windows.Forms.Button()
        CType(Me.GrdProfiles, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GrdMethods, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlProfile.SuspendLayout()
        CType(Me.GrdHruLayers, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GrdCompleteProfiles, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PnlSubAoi.SuspendLayout()
        Me.SuspendLayout()
        '
        'TxtAoiPath
        '
        Me.TxtAoiPath.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TxtAoiPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtAoiPath.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TxtAoiPath.Location = New System.Drawing.Point(237, 11)
        Me.TxtAoiPath.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtAoiPath.Name = "TxtAoiPath"
        Me.TxtAoiPath.ReadOnly = True
        Me.TxtAoiPath.Size = New System.Drawing.Size(446, 22)
        Me.TxtAoiPath.TabIndex = 100
        '
        'BtnSelectAoi
        '
        Me.BtnSelectAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSelectAoi.Location = New System.Drawing.Point(4, 7)
        Me.BtnSelectAoi.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnSelectAoi.Name = "BtnSelectAoi"
        Me.BtnSelectAoi.Size = New System.Drawing.Size(140, 34)
        Me.BtnSelectAoi.TabIndex = 54
        Me.BtnSelectAoi.Text = "Select AOI"
        Me.BtnSelectAoi.UseVisualStyleBackColor = True
        '
        'LblHruLayers
        '
        Me.LblHruLayers.AutoSize = True
        Me.LblHruLayers.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblHruLayers.Location = New System.Drawing.Point(777, 6)
        Me.LblHruLayers.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblHruLayers.Name = "LblHruLayers"
        Me.LblHruLayers.Size = New System.Drawing.Size(137, 16)
        Me.LblHruLayers.TabIndex = 58
        Me.LblHruLayers.Text = "HRU Layers in AOI"
        '
        'GrdProfiles
        '
        Me.GrdProfiles.AllowUserToAddRows = False
        Me.GrdProfiles.AllowUserToDeleteRows = False
        Me.GrdProfiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GrdProfiles.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Profiles})
        Me.GrdProfiles.Location = New System.Drawing.Point(7, 204)
        Me.GrdProfiles.Margin = New System.Windows.Forms.Padding(4)
        Me.GrdProfiles.Name = "GrdProfiles"
        Me.GrdProfiles.ReadOnly = True
        Me.GrdProfiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.GrdProfiles.Size = New System.Drawing.Size(295, 375)
        Me.GrdProfiles.TabIndex = 59
        '
        'Profiles
        '
        DataGridViewCellStyle15.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Profiles.DefaultCellStyle = DataGridViewCellStyle15
        Me.Profiles.HeaderText = "Profiles"
        Me.Profiles.Name = "Profiles"
        Me.Profiles.ReadOnly = True
        Me.Profiles.Width = 245
        '
        'BtnEditProfile
        '
        Me.BtnEditProfile.Enabled = False
        Me.BtnEditProfile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnEditProfile.Location = New System.Drawing.Point(80, 588)
        Me.BtnEditProfile.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnEditProfile.Name = "BtnEditProfile"
        Me.BtnEditProfile.Size = New System.Drawing.Size(70, 31)
        Me.BtnEditProfile.TabIndex = 63
        Me.BtnEditProfile.Text = "Edit"
        Me.BtnEditProfile.UseVisualStyleBackColor = True
        Me.BtnEditProfile.Visible = False
        '
        'BtnProfileCopy
        '
        Me.BtnProfileCopy.Enabled = False
        Me.BtnProfileCopy.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnProfileCopy.Location = New System.Drawing.Point(230, 588)
        Me.BtnProfileCopy.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnProfileCopy.Name = "BtnProfileCopy"
        Me.BtnProfileCopy.Size = New System.Drawing.Size(70, 31)
        Me.BtnProfileCopy.TabIndex = 62
        Me.BtnProfileCopy.Text = "Copy"
        Me.BtnProfileCopy.UseVisualStyleBackColor = True
        Me.BtnProfileCopy.Visible = False
        '
        'BtnProfileDelete
        '
        Me.BtnProfileDelete.Enabled = False
        Me.BtnProfileDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnProfileDelete.Location = New System.Drawing.Point(155, 588)
        Me.BtnProfileDelete.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnProfileDelete.Name = "BtnProfileDelete"
        Me.BtnProfileDelete.Size = New System.Drawing.Size(70, 31)
        Me.BtnProfileDelete.TabIndex = 61
        Me.BtnProfileDelete.Text = "Delete"
        Me.BtnProfileDelete.UseVisualStyleBackColor = True
        Me.BtnProfileDelete.Visible = False
        '
        'BtnProfileNew
        '
        Me.BtnProfileNew.Enabled = False
        Me.BtnProfileNew.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnProfileNew.Location = New System.Drawing.Point(6, 588)
        Me.BtnProfileNew.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnProfileNew.Name = "BtnProfileNew"
        Me.BtnProfileNew.Size = New System.Drawing.Size(70, 31)
        Me.BtnProfileNew.TabIndex = 60
        Me.BtnProfileNew.Text = "New"
        Me.BtnProfileNew.UseVisualStyleBackColor = True
        Me.BtnProfileNew.Visible = False
        '
        'BtnEditMethod
        '
        Me.BtnEditMethod.Enabled = False
        Me.BtnEditMethod.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnEditMethod.Location = New System.Drawing.Point(543, 102)
        Me.BtnEditMethod.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnEditMethod.Name = "BtnEditMethod"
        Me.BtnEditMethod.Size = New System.Drawing.Size(140, 31)
        Me.BtnEditMethod.TabIndex = 78
        Me.BtnEditMethod.Text = "Edit Method"
        Me.BtnEditMethod.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(318, 69)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(97, 16)
        Me.Label8.TabIndex = 77
        Me.Label8.Text = "Model name:"
        '
        'TxtModelName
        '
        Me.TxtModelName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtModelName.Location = New System.Drawing.Point(507, 68)
        Me.TxtModelName.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtModelName.Name = "TxtModelName"
        Me.TxtModelName.ReadOnly = True
        Me.TxtModelName.Size = New System.Drawing.Size(177, 22)
        Me.TxtModelName.TabIndex = 76
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(317, 38)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(111, 16)
        Me.Label7.TabIndex = 75
        Me.Label7.Text = "Toolbox name:"
        '
        'TxtToolboxName
        '
        Me.TxtToolboxName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtToolboxName.Location = New System.Drawing.Point(470, 36)
        Me.TxtToolboxName.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtToolboxName.Name = "TxtToolboxName"
        Me.TxtToolboxName.ReadOnly = True
        Me.TxtToolboxName.Size = New System.Drawing.Size(214, 22)
        Me.TxtToolboxName.TabIndex = 74
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(317, 7)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(116, 16)
        Me.Label1.TabIndex = 71
        Me.Label1.Text = "Current method:"
        '
        'TxtSelMethod
        '
        Me.TxtSelMethod.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtSelMethod.Location = New System.Drawing.Point(470, 5)
        Me.TxtSelMethod.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtSelMethod.Name = "TxtSelMethod"
        Me.TxtSelMethod.ReadOnly = True
        Me.TxtSelMethod.Size = New System.Drawing.Size(214, 22)
        Me.TxtSelMethod.TabIndex = 70
        '
        'TxtDescription
        '
        Me.TxtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtDescription.Location = New System.Drawing.Point(118, 66)
        Me.TxtDescription.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtDescription.Multiline = True
        Me.TxtDescription.Name = "TxtDescription"
        Me.TxtDescription.ReadOnly = True
        Me.TxtDescription.Size = New System.Drawing.Size(182, 53)
        Me.TxtDescription.TabIndex = 69
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(4, 66)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(91, 16)
        Me.Label4.TabIndex = 68
        Me.Label4.Text = "Description:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(4, 38)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(146, 16)
        Me.Label3.TabIndex = 67
        Me.Label3.Text = "Number of methods:"
        '
        'TxtNumMethods
        '
        Me.TxtNumMethods.Location = New System.Drawing.Point(196, 36)
        Me.TxtNumMethods.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtNumMethods.Name = "TxtNumMethods"
        Me.TxtNumMethods.ReadOnly = True
        Me.TxtNumMethods.Size = New System.Drawing.Size(56, 22)
        Me.TxtNumMethods.TabIndex = 66
        '
        'LblCurrentProfile
        '
        Me.LblCurrentProfile.AutoSize = True
        Me.LblCurrentProfile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblCurrentProfile.Location = New System.Drawing.Point(4, 7)
        Me.LblCurrentProfile.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblCurrentProfile.Name = "LblCurrentProfile"
        Me.LblCurrentProfile.Size = New System.Drawing.Size(109, 16)
        Me.LblCurrentProfile.TabIndex = 65
        Me.LblCurrentProfile.Text = "Current profile:"
        '
        'TxtProfileName
        '
        Me.TxtProfileName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtProfileName.Location = New System.Drawing.Point(143, 5)
        Me.TxtProfileName.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtProfileName.Name = "TxtProfileName"
        Me.TxtProfileName.ReadOnly = True
        Me.TxtProfileName.Size = New System.Drawing.Size(157, 22)
        Me.TxtProfileName.TabIndex = 64
        '
        'BtnApply
        '
        Me.BtnApply.Enabled = False
        Me.BtnApply.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnApply.Location = New System.Drawing.Point(165, 127)
        Me.BtnApply.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnApply.Name = "BtnApply"
        Me.BtnApply.Size = New System.Drawing.Size(135, 31)
        Me.BtnApply.TabIndex = 80
        Me.BtnApply.Text = "Apply Changes"
        Me.BtnApply.UseVisualStyleBackColor = True
        Me.BtnApply.Visible = False
        '
        'BtnClose
        '
        Me.BtnClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnClose.Location = New System.Drawing.Point(920, 638)
        Me.BtnClose.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(87, 31)
        Me.BtnClose.TabIndex = 79
        Me.BtnClose.Text = "Close"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'GrdMethods
        '
        Me.GrdMethods.AllowUserToAddRows = False
        Me.GrdMethods.AllowUserToDeleteRows = False
        Me.GrdMethods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GrdMethods.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Methods, Me.ColStatus, Me.IncludeMethod})
        Me.GrdMethods.Location = New System.Drawing.Point(321, 204)
        Me.GrdMethods.Margin = New System.Windows.Forms.Padding(4)
        Me.GrdMethods.Name = "GrdMethods"
        Me.GrdMethods.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.GrdMethods.Size = New System.Drawing.Size(365, 375)
        Me.GrdMethods.TabIndex = 81
        '
        'Methods
        '
        DataGridViewCellStyle16.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Methods.DefaultCellStyle = DataGridViewCellStyle16
        Me.Methods.HeaderText = "Methods"
        Me.Methods.Name = "Methods"
        Me.Methods.ReadOnly = True
        Me.Methods.Width = 150
        '
        'ColStatus
        '
        DataGridViewCellStyle17.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ColStatus.DefaultCellStyle = DataGridViewCellStyle17
        Me.ColStatus.HeaderText = "Status"
        Me.ColStatus.Name = "ColStatus"
        Me.ColStatus.ReadOnly = True
        Me.ColStatus.Width = 115
        '
        'IncludeMethod
        '
        DataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle18.ForeColor = System.Drawing.Color.Silver
        DataGridViewCellStyle18.NullValue = False
        Me.IncludeMethod.DefaultCellStyle = DataGridViewCellStyle18
        Me.IncludeMethod.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.IncludeMethod.HeaderText = "Use"
        Me.IncludeMethod.Name = "IncludeMethod"
        Me.IncludeMethod.ReadOnly = True
        Me.IncludeMethod.Width = 50
        '
        'BtnAddMethod
        '
        Me.BtnAddMethod.Enabled = False
        Me.BtnAddMethod.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAddMethod.Location = New System.Drawing.Point(477, 588)
        Me.BtnAddMethod.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnAddMethod.Name = "BtnAddMethod"
        Me.BtnAddMethod.Size = New System.Drawing.Size(205, 31)
        Me.BtnAddMethod.TabIndex = 84
        Me.BtnAddMethod.Text = "Add Method(s) to Profile"
        Me.BtnAddMethod.UseVisualStyleBackColor = True
        Me.BtnAddMethod.Visible = False
        '
        'BtnMethodDelete
        '
        Me.BtnMethodDelete.Enabled = False
        Me.BtnMethodDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnMethodDelete.Location = New System.Drawing.Point(401, 588)
        Me.BtnMethodDelete.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnMethodDelete.Name = "BtnMethodDelete"
        Me.BtnMethodDelete.Size = New System.Drawing.Size(70, 31)
        Me.BtnMethodDelete.TabIndex = 83
        Me.BtnMethodDelete.Text = "Delete"
        Me.BtnMethodDelete.UseVisualStyleBackColor = True
        Me.BtnMethodDelete.Visible = False
        '
        'BtnMethodNew
        '
        Me.BtnMethodNew.Enabled = False
        Me.BtnMethodNew.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnMethodNew.Location = New System.Drawing.Point(325, 588)
        Me.BtnMethodNew.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnMethodNew.Name = "BtnMethodNew"
        Me.BtnMethodNew.Size = New System.Drawing.Size(70, 31)
        Me.BtnMethodNew.TabIndex = 82
        Me.BtnMethodNew.Text = "New"
        Me.BtnMethodNew.UseVisualStyleBackColor = True
        Me.BtnMethodNew.Visible = False
        '
        'BtnVerify
        '
        Me.BtnVerify.Enabled = False
        Me.BtnVerify.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnVerify.Location = New System.Drawing.Point(695, 161)
        Me.BtnVerify.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnVerify.Name = "BtnVerify"
        Me.BtnVerify.Size = New System.Drawing.Size(85, 31)
        Me.BtnVerify.TabIndex = 85
        Me.BtnVerify.Text = "Verify"
        Me.BtnVerify.UseVisualStyleBackColor = True
        '
        'BtnCalculate
        '
        Me.BtnCalculate.Enabled = False
        Me.BtnCalculate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnCalculate.Location = New System.Drawing.Point(784, 161)
        Me.BtnCalculate.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnCalculate.Name = "BtnCalculate"
        Me.BtnCalculate.Size = New System.Drawing.Size(225, 31)
        Me.BtnCalculate.TabIndex = 86
        Me.BtnCalculate.Text = "(Re)Calculate Parameters"
        Me.BtnCalculate.UseVisualStyleBackColor = True
        '
        'PnlProfile
        '
        Me.PnlProfile.Controls.Add(Me.LblStatus)
        Me.PnlProfile.Controls.Add(Me.BtnExportProfile)
        Me.PnlProfile.Controls.Add(Me.BtnImport)
        Me.PnlProfile.Controls.Add(Me.GrdMethods)
        Me.PnlProfile.Controls.Add(Me.GrdProfiles)
        Me.PnlProfile.Controls.Add(Me.BtnProfileNew)
        Me.PnlProfile.Controls.Add(Me.BtnAddMethod)
        Me.PnlProfile.Controls.Add(Me.BtnProfileDelete)
        Me.PnlProfile.Controls.Add(Me.BtnMethodDelete)
        Me.PnlProfile.Controls.Add(Me.BtnProfileCopy)
        Me.PnlProfile.Controls.Add(Me.BtnMethodNew)
        Me.PnlProfile.Controls.Add(Me.BtnEditProfile)
        Me.PnlProfile.Controls.Add(Me.TxtProfileName)
        Me.PnlProfile.Controls.Add(Me.BtnApply)
        Me.PnlProfile.Controls.Add(Me.LblCurrentProfile)
        Me.PnlProfile.Controls.Add(Me.TxtNumMethods)
        Me.PnlProfile.Controls.Add(Me.BtnEditMethod)
        Me.PnlProfile.Controls.Add(Me.Label3)
        Me.PnlProfile.Controls.Add(Me.Label8)
        Me.PnlProfile.Controls.Add(Me.Label4)
        Me.PnlProfile.Controls.Add(Me.TxtModelName)
        Me.PnlProfile.Controls.Add(Me.TxtDescription)
        Me.PnlProfile.Controls.Add(Me.Label7)
        Me.PnlProfile.Controls.Add(Me.TxtSelMethod)
        Me.PnlProfile.Controls.Add(Me.TxtToolboxName)
        Me.PnlProfile.Controls.Add(Me.Label1)
        Me.PnlProfile.Location = New System.Drawing.Point(-3, 50)
        Me.PnlProfile.Margin = New System.Windows.Forms.Padding(4)
        Me.PnlProfile.Name = "PnlProfile"
        Me.PnlProfile.Size = New System.Drawing.Size(692, 625)
        Me.PnlProfile.TabIndex = 87
        '
        'LblStatus
        '
        Me.LblStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LblStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblStatus.ForeColor = System.Drawing.Color.Red
        Me.LblStatus.Location = New System.Drawing.Point(322, 178)
        Me.LblStatus.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.LblStatus.Name = "LblStatus"
        Me.LblStatus.Size = New System.Drawing.Size(364, 20)
        Me.LblStatus.TabIndex = 88
        '
        'BtnExportProfile
        '
        Me.BtnExportProfile.Enabled = False
        Me.BtnExportProfile.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnExportProfile.Location = New System.Drawing.Point(184, 166)
        Me.BtnExportProfile.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnExportProfile.Name = "BtnExportProfile"
        Me.BtnExportProfile.Size = New System.Drawing.Size(125, 31)
        Me.BtnExportProfile.TabIndex = 87
        Me.BtnExportProfile.Text = "Export Profile"
        Me.BtnExportProfile.UseVisualStyleBackColor = True
        Me.BtnExportProfile.Visible = False
        '
        'BtnImport
        '
        Me.BtnImport.Enabled = False
        Me.BtnImport.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnImport.Location = New System.Drawing.Point(6, 166)
        Me.BtnImport.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnImport.Name = "BtnImport"
        Me.BtnImport.Size = New System.Drawing.Size(175, 31)
        Me.BtnImport.TabIndex = 86
        Me.BtnImport.Text = "Import Public Profile"
        Me.BtnImport.UseVisualStyleBackColor = True
        Me.BtnImport.Visible = False
        '
        'GrdHruLayers
        '
        Me.GrdHruLayers.AllowUserToAddRows = False
        Me.GrdHruLayers.AllowUserToDeleteRows = False
        Me.GrdHruLayers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GrdHruLayers.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.HruName, Me.Completed})
        Me.GrdHruLayers.Location = New System.Drawing.Point(695, 31)
        Me.GrdHruLayers.Margin = New System.Windows.Forms.Padding(4)
        Me.GrdHruLayers.Name = "GrdHruLayers"
        Me.GrdHruLayers.ReadOnly = True
        Me.GrdHruLayers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.GrdHruLayers.Size = New System.Drawing.Size(315, 123)
        Me.GrdHruLayers.TabIndex = 87
        '
        'HruName
        '
        DataGridViewCellStyle19.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HruName.DefaultCellStyle = DataGridViewCellStyle19
        Me.HruName.HeaderText = "HRU Name"
        Me.HruName.Name = "HruName"
        Me.HruName.ReadOnly = True
        Me.HruName.Width = 140
        '
        'Completed
        '
        DataGridViewCellStyle20.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Completed.DefaultCellStyle = DataGridViewCellStyle20
        Me.Completed.HeaderText = "# Complete"
        Me.Completed.Name = "Completed"
        Me.Completed.ReadOnly = True
        Me.Completed.Width = 125
        '
        'GrdCompleteProfiles
        '
        Me.GrdCompleteProfiles.AllowUserToAddRows = False
        Me.GrdCompleteProfiles.AllowUserToDeleteRows = False
        Me.GrdCompleteProfiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GrdCompleteProfiles.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2})
        Me.GrdCompleteProfiles.Location = New System.Drawing.Point(695, 254)
        Me.GrdCompleteProfiles.Margin = New System.Windows.Forms.Padding(4)
        Me.GrdCompleteProfiles.MultiSelect = False
        Me.GrdCompleteProfiles.Name = "GrdCompleteProfiles"
        Me.GrdCompleteProfiles.ReadOnly = True
        Me.GrdCompleteProfiles.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.GrdCompleteProfiles.Size = New System.Drawing.Size(315, 123)
        Me.GrdCompleteProfiles.TabIndex = 89
        '
        'DataGridViewTextBoxColumn1
        '
        DataGridViewCellStyle21.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DataGridViewTextBoxColumn1.DefaultCellStyle = DataGridViewCellStyle21
        Me.DataGridViewTextBoxColumn1.HeaderText = "Profile Name"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        Me.DataGridViewTextBoxColumn1.Width = 140
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.HeaderText = "Completed"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 130
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(778, 230)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(140, 16)
        Me.Label6.TabIndex = 88
        Me.Label6.Text = "Completed Profiles"
        '
        'PnlSubAoi
        '
        Me.PnlSubAoi.Controls.Add(Me.RichTextBox1)
        Me.PnlSubAoi.Controls.Add(Me.TxtSubAoiCount)
        Me.PnlSubAoi.Controls.Add(Me.Label10)
        Me.PnlSubAoi.Controls.Add(Me.CboSubAoiId)
        Me.PnlSubAoi.Controls.Add(Me.Label9)
        Me.PnlSubAoi.Controls.Add(Me.CkAppendSubAoi)
        Me.PnlSubAoi.Location = New System.Drawing.Point(695, 416)
        Me.PnlSubAoi.Name = "PnlSubAoi"
        Me.PnlSubAoi.Size = New System.Drawing.Size(315, 193)
        Me.PnlSubAoi.TabIndex = 94
        '
        'RichTextBox1
        '
        Me.RichTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.RichTextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RichTextBox1.Location = New System.Drawing.Point(5, 112)
        Me.RichTextBox1.Name = "RichTextBox1"
        Me.RichTextBox1.ReadOnly = True
        Me.RichTextBox1.Size = New System.Drawing.Size(307, 77)
        Me.RichTextBox1.TabIndex = 95
        Me.RichTextBox1.Text = "Please use the SubAOI ID Tool on the " & Global.Microsoft.VisualBasic.ChrW(10) & "BAGIS-P AOI Parameterization menu to " & Global.Microsoft.VisualBasic.ChrW(10) & "manag" & _
            "e the SubAOI ID Layers"
        '
        'TxtSubAoiCount
        '
        Me.TxtSubAoiCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtSubAoiCount.Location = New System.Drawing.Point(225, 94)
        Me.TxtSubAoiCount.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtSubAoiCount.Name = "TxtSubAoiCount"
        Me.TxtSubAoiCount.ReadOnly = True
        Me.TxtSubAoiCount.Size = New System.Drawing.Size(73, 21)
        Me.TxtSubAoiCount.TabIndex = 94
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(4, 94)
        Me.Label10.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(186, 15)
        Me.Label10.TabIndex = 98
        Me.Label10.Text = "# of SubAOIs in the ID layer:"
        '
        'CboSubAoiId
        '
        Me.CboSubAoiId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboSubAoiId.FormattingEnabled = True
        Me.CboSubAoiId.Location = New System.Drawing.Point(12, 60)
        Me.CboSubAoiId.Name = "CboSubAoiId"
        Me.CboSubAoiId.Size = New System.Drawing.Size(185, 24)
        Me.CboSubAoiId.Sorted = True
        Me.CboSubAoiId.TabIndex = 97
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(8, 40)
        Me.Label9.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(146, 15)
        Me.Label9.TabIndex = 96
        Me.Label9.Text = "SubAOI ID layer used:"
        '
        'CkAppendSubAoi
        '
        Me.CkAppendSubAoi.AutoSize = True
        Me.CkAppendSubAoi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CkAppendSubAoi.Location = New System.Drawing.Point(4, 16)
        Me.CkAppendSubAoi.Margin = New System.Windows.Forms.Padding(4)
        Me.CkAppendSubAoi.Name = "CkAppendSubAoi"
        Me.CkAppendSubAoi.Size = New System.Drawing.Size(258, 19)
        Me.CkAppendSubAoi.TabIndex = 95
        Me.CkAppendSubAoi.Text = "Append SubAOI ID to Parameter File"
        Me.CkAppendSubAoi.UseVisualStyleBackColor = True
        '
        'TxtLblPath
        '
        Me.TxtLblPath.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtLblPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtLblPath.Location = New System.Drawing.Point(154, 15)
        Me.TxtLblPath.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtLblPath.Name = "TxtLblPath"
        Me.TxtLblPath.ReadOnly = True
        Me.TxtLblPath.Size = New System.Drawing.Size(65, 15)
        Me.TxtLblPath.TabIndex = 87
        Me.TxtLblPath.Text = "AOI Path:"
        '
        'TxtNoData
        '
        Me.TxtNoData.Location = New System.Drawing.Point(842, 196)
        Me.TxtNoData.Margin = New System.Windows.Forms.Padding(4)
        Me.TxtNoData.Name = "TxtNoData"
        Me.TxtNoData.Size = New System.Drawing.Size(50, 22)
        Me.TxtNoData.TabIndex = 87
        Me.TxtNoData.Text = "-99"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(705, 199)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(113, 16)
        Me.Label2.TabIndex = 101
        Me.Label2.Text = "No Data Value:"
        '
        'BtnViewLog
        '
        Me.BtnViewLog.Enabled = False
        Me.BtnViewLog.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnViewLog.Location = New System.Drawing.Point(695, 382)
        Me.BtnViewLog.Margin = New System.Windows.Forms.Padding(4)
        Me.BtnViewLog.Name = "BtnViewLog"
        Me.BtnViewLog.Size = New System.Drawing.Size(100, 31)
        Me.BtnViewLog.TabIndex = 102
        Me.BtnViewLog.Text = "View Log"
        Me.BtnViewLog.UseVisualStyleBackColor = True
        '
        'FrmProfileBuilder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1014, 677)
        Me.Controls.Add(Me.BtnViewLog)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TxtNoData)
        Me.Controls.Add(Me.TxtLblPath)
        Me.Controls.Add(Me.PnlSubAoi)
        Me.Controls.Add(Me.GrdCompleteProfiles)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.GrdHruLayers)
        Me.Controls.Add(Me.PnlProfile)
        Me.Controls.Add(Me.BtnCalculate)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.BtnVerify)
        Me.Controls.Add(Me.LblHruLayers)
        Me.Controls.Add(Me.TxtAoiPath)
        Me.Controls.Add(Me.BtnSelectAoi)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "FrmProfileBuilder"
        Me.ShowIcon = False
        Me.Text = "Profile Builder"
        CType(Me.GrdProfiles, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GrdMethods, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlProfile.ResumeLayout(False)
        Me.PnlProfile.PerformLayout()
        CType(Me.GrdHruLayers, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GrdCompleteProfiles, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PnlSubAoi.ResumeLayout(False)
        Me.PnlSubAoi.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TxtAoiPath As System.Windows.Forms.TextBox
    Friend WithEvents BtnSelectAoi As System.Windows.Forms.Button
    Friend WithEvents LblHruLayers As System.Windows.Forms.Label
    Friend WithEvents GrdProfiles As System.Windows.Forms.DataGridView
    Friend WithEvents BtnEditProfile As System.Windows.Forms.Button
    Friend WithEvents BtnProfileCopy As System.Windows.Forms.Button
    Friend WithEvents BtnProfileDelete As System.Windows.Forms.Button
    Friend WithEvents BtnProfileNew As System.Windows.Forms.Button
    Friend WithEvents BtnEditMethod As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents TxtModelName As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TxtToolboxName As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TxtSelMethod As System.Windows.Forms.TextBox
    Friend WithEvents TxtDescription As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TxtNumMethods As System.Windows.Forms.TextBox
    Friend WithEvents LblCurrentProfile As System.Windows.Forms.Label
    Friend WithEvents TxtProfileName As System.Windows.Forms.TextBox
    Friend WithEvents BtnApply As System.Windows.Forms.Button
    Friend WithEvents BtnClose As System.Windows.Forms.Button
    Friend WithEvents GrdMethods As System.Windows.Forms.DataGridView
    Friend WithEvents BtnAddMethod As System.Windows.Forms.Button
    Friend WithEvents BtnMethodDelete As System.Windows.Forms.Button
    Friend WithEvents BtnMethodNew As System.Windows.Forms.Button
    Friend WithEvents BtnVerify As System.Windows.Forms.Button
    Friend WithEvents BtnCalculate As System.Windows.Forms.Button
    Friend WithEvents PnlProfile As System.Windows.Forms.Panel
    Friend WithEvents BtnImport As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Methods As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ColStatus As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents IncludeMethod As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents GrdHruLayers As System.Windows.Forms.DataGridView
    Friend WithEvents GrdCompleteProfiles As System.Windows.Forms.DataGridView
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents PnlSubAoi As System.Windows.Forms.Panel
    Friend WithEvents TxtSubAoiCount As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents CboSubAoiId As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents CkAppendSubAoi As System.Windows.Forms.CheckBox
    Friend WithEvents RichTextBox1 As System.Windows.Forms.RichTextBox
    Friend WithEvents TxtLblPath As System.Windows.Forms.TextBox
    Friend WithEvents TxtNoData As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents BtnExportProfile As System.Windows.Forms.Button
    Friend WithEvents Profiles As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents HruName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Completed As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LblStatus As System.Windows.Forms.Label
    Friend WithEvents BtnViewLog As System.Windows.Forms.Button
End Class
