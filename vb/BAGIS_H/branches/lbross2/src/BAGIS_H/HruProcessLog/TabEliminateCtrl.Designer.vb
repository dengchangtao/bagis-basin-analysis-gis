<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TabEliminateCtrl
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
        Me.TxtSelectionMethod = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.TxtPercent = New System.Windows.Forms.TextBox()
        Me.LblPercent = New System.Windows.Forms.Label()
        Me.TxtUnits = New System.Windows.Forms.TextBox()
        Me.LblPolyUnits = New System.Windows.Forms.Label()
        Me.TxtPolyArea = New System.Windows.Forms.TextBox()
        Me.LblPolyThreshold = New System.Windows.Forms.Label()
        Me.LblZonesRemoved = New System.Windows.Forms.Label()
        Me.TxtZonesRemoved = New System.Windows.Forms.TextBox()
        Me.RdoArea = New System.Windows.Forms.RadioButton()
        Me.RdoPercent = New System.Windows.Forms.RadioButton()
        Me.SuspendLayout()
        '
        'TxtSelectionMethod
        '
        Me.TxtSelectionMethod.Location = New System.Drawing.Point(119, 28)
        Me.TxtSelectionMethod.Name = "TxtSelectionMethod"
        Me.TxtSelectionMethod.ReadOnly = True
        Me.TxtSelectionMethod.Size = New System.Drawing.Size(139, 20)
        Me.TxtSelectionMethod.TabIndex = 44
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(1, 31)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(117, 13)
        Me.Label1.TabIndex = 43
        Me.Label1.Text = "Merge polygons by:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(2, 8)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(118, 13)
        Me.Label5.TabIndex = 57
        Me.Label5.Text = "Eliminate threshold:"
        '
        'TxtPercent
        '
        Me.TxtPercent.Location = New System.Drawing.Point(124, 102)
        Me.TxtPercent.Name = "TxtPercent"
        Me.TxtPercent.ReadOnly = True
        Me.TxtPercent.Size = New System.Drawing.Size(134, 20)
        Me.TxtPercent.TabIndex = 56
        '
        'LblPercent
        '
        Me.LblPercent.AutoSize = True
        Me.LblPercent.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblPercent.Location = New System.Drawing.Point(1, 105)
        Me.LblPercent.Name = "LblPercent"
        Me.LblPercent.Size = New System.Drawing.Size(127, 13)
        Me.LblPercent.TabIndex = 55
        Me.LblPercent.Text = "Threshold percentile:"
        '
        'TxtUnits
        '
        Me.TxtUnits.Location = New System.Drawing.Point(62, 78)
        Me.TxtUnits.Name = "TxtUnits"
        Me.TxtUnits.ReadOnly = True
        Me.TxtUnits.Size = New System.Drawing.Size(105, 20)
        Me.TxtUnits.TabIndex = 54
        '
        'LblPolyUnits
        '
        Me.LblPolyUnits.AutoSize = True
        Me.LblPolyUnits.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblPolyUnits.Location = New System.Drawing.Point(2, 82)
        Me.LblPolyUnits.Name = "LblPolyUnits"
        Me.LblPolyUnits.Size = New System.Drawing.Size(62, 13)
        Me.LblPolyUnits.TabIndex = 53
        Me.LblPolyUnits.Text = "Area unit:"
        '
        'TxtPolyArea
        '
        Me.TxtPolyArea.Location = New System.Drawing.Point(147, 54)
        Me.TxtPolyArea.Name = "TxtPolyArea"
        Me.TxtPolyArea.ReadOnly = True
        Me.TxtPolyArea.Size = New System.Drawing.Size(111, 20)
        Me.TxtPolyArea.TabIndex = 52
        '
        'LblPolyThreshold
        '
        Me.LblPolyThreshold.AutoSize = True
        Me.LblPolyThreshold.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblPolyThreshold.Location = New System.Drawing.Point(1, 57)
        Me.LblPolyThreshold.Name = "LblPolyThreshold"
        Me.LblPolyThreshold.Size = New System.Drawing.Size(144, 13)
        Me.LblPolyThreshold.TabIndex = 51
        Me.LblPolyThreshold.Text = "Threshold polygon area:"
        '
        'LblZonesRemoved
        '
        Me.LblZonesRemoved.AutoSize = True
        Me.LblZonesRemoved.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblZonesRemoved.Location = New System.Drawing.Point(3, 130)
        Me.LblZonesRemoved.Name = "LblZonesRemoved"
        Me.LblZonesRemoved.Size = New System.Drawing.Size(98, 13)
        Me.LblZonesRemoved.TabIndex = 66
        Me.LblZonesRemoved.Text = "Zones removed:"
        '
        'TxtZonesRemoved
        '
        Me.TxtZonesRemoved.Location = New System.Drawing.Point(100, 127)
        Me.TxtZonesRemoved.Name = "TxtZonesRemoved"
        Me.TxtZonesRemoved.ReadOnly = True
        Me.TxtZonesRemoved.Size = New System.Drawing.Size(158, 20)
        Me.TxtZonesRemoved.TabIndex = 67
        '
        'RdoArea
        '
        Me.RdoArea.AutoCheck = False
        Me.RdoArea.AutoSize = True
        Me.RdoArea.Location = New System.Drawing.Point(191, 6)
        Me.RdoArea.Name = "RdoArea"
        Me.RdoArea.Size = New System.Drawing.Size(85, 17)
        Me.RdoArea.TabIndex = 72
        Me.RdoArea.TabStop = True
        Me.RdoArea.Text = "Area of zone"
        Me.RdoArea.UseVisualStyleBackColor = True
        '
        'RdoPercent
        '
        Me.RdoPercent.AutoCheck = False
        Me.RdoPercent.AutoSize = True
        Me.RdoPercent.Location = New System.Drawing.Point(118, 6)
        Me.RdoPercent.Name = "RdoPercent"
        Me.RdoPercent.Size = New System.Drawing.Size(72, 17)
        Me.RdoPercent.TabIndex = 73
        Me.RdoPercent.TabStop = True
        Me.RdoPercent.Text = "Percentile"
        Me.RdoPercent.UseVisualStyleBackColor = True
        '
        'TabEliminateCtrl
        '
        Me.Controls.Add(Me.RdoPercent)
        Me.Controls.Add(Me.RdoArea)
        Me.Controls.Add(Me.TxtZonesRemoved)
        Me.Controls.Add(Me.LblZonesRemoved)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TxtPercent)
        Me.Controls.Add(Me.LblPercent)
        Me.Controls.Add(Me.TxtUnits)
        Me.Controls.Add(Me.LblPolyUnits)
        Me.Controls.Add(Me.TxtPolyArea)
        Me.Controls.Add(Me.LblPolyThreshold)
        Me.Controls.Add(Me.TxtSelectionMethod)
        Me.Controls.Add(Me.Label1)
        Me.Name = "TabEliminateCtrl"
        Me.Size = New System.Drawing.Size(650, 375)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TxtSelectionMethod As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label

    Public Sub New(ByVal elimProcess As BAGIS_ClassLibrary.EliminateProcess)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Me.Text = "Eliminate"
        TxtSelectionMethod.Text = elimProcess.SelectionMethod
        TxtZonesRemoved.Text = Format(elimProcess.PolygonsEliminated, "###,###,##0")
        TxtPolyArea.Text = Format(elimProcess.PolygonArea, "###,###,##0.#####")
        TxtUnits.Text = elimProcess.PolygonAreaUnitsText
        If elimProcess.SelectByPolyArea = True Then
            RdoArea.Checked = True
            LblPercent.Visible = False
            TxtPercent.Visible = False
            LblZonesRemoved.Location = New System.Drawing.Point(1, 105)
            TxtZonesRemoved.Location = New System.Drawing.Point(98, 102)
        Else
            RdoPercent.Checked = True
            TxtPercent.Text = Format(elimProcess.AreaPercent, "##0")
        End If

    End Sub
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TxtPercent As System.Windows.Forms.TextBox
    Friend WithEvents LblPercent As System.Windows.Forms.Label
    Friend WithEvents TxtUnits As System.Windows.Forms.TextBox
    Friend WithEvents LblPolyUnits As System.Windows.Forms.Label
    Friend WithEvents TxtPolyArea As System.Windows.Forms.TextBox
    Friend WithEvents LblPolyThreshold As System.Windows.Forms.Label
    Friend WithEvents LblZonesRemoved As System.Windows.Forms.Label
    Friend WithEvents TxtZonesRemoved As System.Windows.Forms.TextBox
    Friend WithEvents RdoArea As System.Windows.Forms.RadioButton
    Friend WithEvents RdoPercent As System.Windows.Forms.RadioButton
End Class
