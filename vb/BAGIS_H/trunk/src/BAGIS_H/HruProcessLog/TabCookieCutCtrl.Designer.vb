<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TabCookieCutCtrl
    Inherits System.Windows.Forms.TabPage
    'Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.TxtCookieCutName = New System.Windows.Forms.TextBox()
        Me.LblLayerName = New System.Windows.Forms.Label()
        Me.LblApplyZones = New System.Windows.Forms.Label()
        Me.TxtApplyZones = New System.Windows.Forms.TextBox()
        Me.TxtPreserveAoiBoundary = New System.Windows.Forms.TextBox()
        Me.LblPreserveAoiBoundary = New System.Windows.Forms.Label()
        Me.TxtCookieCutPath = New System.Windows.Forms.TextBox()
        Me.LblLayerPath = New System.Windows.Forms.Label()
        Me.TxtField = New System.Windows.Forms.TextBox()
        Me.LblField = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'TxtCookieCutName
        '
        Me.TxtCookieCutName.Location = New System.Drawing.Point(136, 30)
        Me.TxtCookieCutName.Name = "TxtCookieCutName"
        Me.TxtCookieCutName.ReadOnly = True
        Me.TxtCookieCutName.Size = New System.Drawing.Size(233, 20)
        Me.TxtCookieCutName.TabIndex = 44
        '
        'LblLayerName
        '
        Me.LblLayerName.AutoSize = True
        Me.LblLayerName.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLayerName.Location = New System.Drawing.Point(2, 33)
        Me.LblLayerName.Name = "LblLayerName"
        Me.LblLayerName.Size = New System.Drawing.Size(87, 13)
        Me.LblLayerName.TabIndex = 43
        Me.LblLayerName.Text = "LblLayerName"
        '
        'LblApplyZones
        '
        Me.LblApplyZones.AutoSize = True
        Me.LblApplyZones.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblApplyZones.Location = New System.Drawing.Point(2, 82)
        Me.LblApplyZones.Name = "LblApplyZones"
        Me.LblApplyZones.Size = New System.Drawing.Size(126, 13)
        Me.LblApplyZones.TabIndex = 66
        Me.LblApplyZones.Text = "Apply to field values:"
        '
        'TxtApplyZones
        '
        Me.TxtApplyZones.Location = New System.Drawing.Point(136, 79)
        Me.TxtApplyZones.Name = "TxtApplyZones"
        Me.TxtApplyZones.ReadOnly = True
        Me.TxtApplyZones.Size = New System.Drawing.Size(181, 20)
        Me.TxtApplyZones.TabIndex = 67
        '
        'TxtPreserveAoiBoundary
        '
        Me.TxtPreserveAoiBoundary.Location = New System.Drawing.Point(142, 104)
        Me.TxtPreserveAoiBoundary.Name = "TxtPreserveAoiBoundary"
        Me.TxtPreserveAoiBoundary.ReadOnly = True
        Me.TxtPreserveAoiBoundary.Size = New System.Drawing.Size(50, 20)
        Me.TxtPreserveAoiBoundary.TabIndex = 69
        Me.TxtPreserveAoiBoundary.Visible = False
        '
        'LblPreserveAoiBoundary
        '
        Me.LblPreserveAoiBoundary.AutoSize = True
        Me.LblPreserveAoiBoundary.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblPreserveAoiBoundary.Location = New System.Drawing.Point(2, 107)
        Me.LblPreserveAoiBoundary.Name = "LblPreserveAoiBoundary"
        Me.LblPreserveAoiBoundary.Size = New System.Drawing.Size(142, 13)
        Me.LblPreserveAoiBoundary.TabIndex = 68
        Me.LblPreserveAoiBoundary.Text = "Preserve AOI boundary:"
        Me.LblPreserveAoiBoundary.Visible = False
        '
        'TxtCookieCutPath
        '
        Me.TxtCookieCutPath.Location = New System.Drawing.Point(136, 6)
        Me.TxtCookieCutPath.Name = "TxtCookieCutPath"
        Me.TxtCookieCutPath.ReadOnly = True
        Me.TxtCookieCutPath.Size = New System.Drawing.Size(233, 20)
        Me.TxtCookieCutPath.TabIndex = 71
        '
        'LblLayerPath
        '
        Me.LblLayerPath.AutoSize = True
        Me.LblLayerPath.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblLayerPath.Location = New System.Drawing.Point(2, 9)
        Me.LblLayerPath.Name = "LblLayerPath"
        Me.LblLayerPath.Size = New System.Drawing.Size(81, 13)
        Me.LblLayerPath.TabIndex = 70
        Me.LblLayerPath.Text = "LblLayerPath"
        '
        'TxtField
        '
        Me.TxtField.Location = New System.Drawing.Point(136, 55)
        Me.TxtField.Name = "TxtField"
        Me.TxtField.ReadOnly = True
        Me.TxtField.Size = New System.Drawing.Size(233, 20)
        Me.TxtField.TabIndex = 73
        '
        'LblField
        '
        Me.LblField.AutoSize = True
        Me.LblField.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblField.Location = New System.Drawing.Point(2, 58)
        Me.LblField.Name = "LblField"
        Me.LblField.Size = New System.Drawing.Size(51, 13)
        Me.LblField.TabIndex = 72
        Me.LblField.Text = "LblField"
        '
        'TabCookieCutCtrl
        '
        Me.Controls.Add(Me.TxtField)
        Me.Controls.Add(Me.LblField)
        Me.Controls.Add(Me.TxtCookieCutPath)
        Me.Controls.Add(Me.LblLayerPath)
        Me.Controls.Add(Me.TxtPreserveAoiBoundary)
        Me.Controls.Add(Me.LblPreserveAoiBoundary)
        Me.Controls.Add(Me.TxtApplyZones)
        Me.Controls.Add(Me.LblApplyZones)
        Me.Controls.Add(Me.TxtCookieCutName)
        Me.Controls.Add(Me.LblLayerName)
        Me.Name = "TabCookieCutCtrl"
        Me.Size = New System.Drawing.Size(650, 375)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TxtCookieCutName As System.Windows.Forms.TextBox
    Friend WithEvents LblLayerName As System.Windows.Forms.Label

    Public Sub New(ByVal cookieCutProcess As BAGIS_ClassLibrary.CookieCutProcess)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Me.Text = cookieCutProcess.Mode
        LblLayerPath.Text = cookieCutProcess.Mode & " layer path:"
        TxtCookieCutPath.Text = cookieCutProcess.CookieCutPath
        LblLayerName.Text = cookieCutProcess.Mode & " layer name:"
        TxtCookieCutName.Text = cookieCutProcess.CookieCutName
        LblField.Text = cookieCutProcess.Mode & " field name:"
        TxtField.Text = cookieCutProcess.CookieCutField
        If cookieCutProcess.Mode = BAGIS_ClassLibrary.BA_MODE_COOKIE_CUT Then
            LblApplyZones.Text = "Apply to cookie-cut values:"
            LblPreserveAoiBoundary.Visible = True
            TxtPreserveAoiBoundary.Visible = True
            TxtPreserveAoiBoundary.Text = cookieCutProcess.PreserveAoiBoundary
        ElseIf cookieCutProcess.Mode = BAGIS_ClassLibrary.BA_MODE_STAMP Then
            LblApplyZones.Text = "Apply to stamp values:"
        End If

        Dim selValues As Long() = cookieCutProcess.SelectedValues
        Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder
        If selValues IsNot Nothing AndAlso selValues.Length > 0 AndAlso selValues(0) <> 0 Then
            For Each pLong In selValues
                sb.Append(CStr(pLong))
                sb.Append(", ")
            Next
            ' Remove trailing comma
            sb.Remove(sb.Length - 2, 2)
        End If
        TxtApplyZones.Text = sb.ToString

    End Sub
    Friend WithEvents LblApplyZones As System.Windows.Forms.Label
    Friend WithEvents TxtApplyZones As System.Windows.Forms.TextBox
    Friend WithEvents TxtPreserveAoiBoundary As System.Windows.Forms.TextBox
    Friend WithEvents LblPreserveAoiBoundary As System.Windows.Forms.Label
    Friend WithEvents TxtCookieCutPath As System.Windows.Forms.TextBox
    Friend WithEvents LblLayerPath As System.Windows.Forms.Label
    Friend WithEvents TxtField As System.Windows.Forms.TextBox
    Friend WithEvents LblField As System.Windows.Forms.Label
End Class
