Imports System.Windows.Forms
Imports BAGIS_ClassLibrary

Public Class FrmHruTabLog

    Dim m_aoi As Aoi
    Dim m_hruName As String

    Public Sub New(ByVal aoi As Aoi, ByVal hruName As String, ByVal warnOfMissingLayers As Boolean)
        InitializeComponent()
        m_aoi = aoi
        m_hruName = hruName
        TxtAoiName.Text = aoi.Name
        TxtAoiPath.Text = aoi.FilePath
        Dim logHru As Hru = New Hru
        Dim phru As Hru = New Hru
        If aoi.HruList.Count = 1 Then
            phru = aoi.HruList.Item(0)
        End If
        Dim isHruParent As Boolean = True
        ' We display the log for the hru that generated the xml log file
        If String.Compare(phru.Name, hruName) = 0 Then
            logHru = phru
            isHruParent = False
        Else
            ' We are looking for a parent log; Work our way through the parents until we find the one we want
            Dim parentHru As Hru = phru.ParentHru
            Do While parentHru IsNot Nothing
                If String.Compare(parentHru.Name, hruName) = 0 Then
                    logHru = parentHru
                    Exit Do
                Else
                    parentHru = parentHru.ParentHru
                End If
            Loop
        End If
        If String.IsNullOrEmpty(logHru.Name) Then
            Windows.Forms.MessageBox.Show("Unable to display log for hru " & hruName & ".", "Missing log", _
                                          Windows.Forms.MessageBoxButtons.OK, Windows.Forms.MessageBoxIcon.Information)
            Exit Sub
        End If
        Me.Text = "Current HRU: " & logHru.Name
        TxtHruName.Text = logHru.Name
        TxtHruPath.Text = logHru.FilePath
        If logHru.ParentHru IsNot Nothing Then
            TxtParentName.Text = logHru.ParentHru.Name
            BtnViewParent.Enabled = True
        Else
            BtnViewParent.Enabled = False
        End If
        Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder
        Dim selZones As Zone() = logHru.SelectedZones
        If selZones IsNot Nothing AndAlso selZones.Length > 0 AndAlso selZones(0) IsNot Nothing Then
            For Each pZone In selZones
                sb.Append(CStr(pZone.Id))
                sb.Append(", ")
            Next
            ' Remove trailing comma
            sb.Remove(sb.Length - 2, 2)
        End If
        TxtApplyZones.Text = sb.ToString
        TxtNonContiguous.Text = NO
        If logHru.AllowNonContiguousHru = True Then TxtNonContiguous.Text = YES
        TxtPolygonCount.Text = logHru.FeatureCount
        TxtAverageSize.Text = logHru.AverageFeatureSize
        TxtMaxSize.Text = logHru.MaxFeatureSize
        If logHru.Units <> MeasurementUnit.Missing Then
            TxtUnits.Text = logHru.UnitsText
        End If
        TxtDateCreated.Text = logHru.DateCreatedValue
        TxtAppVersion.Text = aoi.ApplicationVersion
        ' Sort the rules by ruleId
        logHru.RuleList.Sort()
        For Each pRule In logHru.RuleList
            If TypeOf pRule Is RasterSliceRule Then
                Dim sliceTab As TabSliceRuleCtrl = New TabSliceRuleCtrl(logHru, pRule, isHruParent)
                TabControl1.Controls.Add(sliceTab)
            ElseIf TypeOf pRule Is RasterReclassRule Then
                Dim reclassTab As TabReclassRuleCtrl = New TabReclassRuleCtrl(logHru, pRule, isHruParent)
                TabControl1.Controls.Add(reclassTab)
            ElseIf TypeOf pRule Is ReclassContinuousRule Then
                Dim reclassTab As TabReclassContinuousCtrl = New TabReclassContinuousCtrl(logHru, pRule, isHruParent)
                TabControl1.Controls.Add(reclassTab)
            ElseIf TypeOf pRule Is PrismPrecipRule Then
                Dim prismTab As TabPrismRuleCtrl = New TabPrismRuleCtrl(logHru, pRule, isHruParent)
                TabControl1.Controls.Add(prismTab)
            ElseIf TypeOf pRule Is DAFlowTypeZonesRule Then
                Dim daFlowTab As TabDAFlowRuleCtrl = New TabDAFlowRuleCtrl(logHru, pRule, isHruParent)
                TabControl1.Controls.Add(daFlowTab)
            ElseIf TypeOf pRule Is ContributingAreasRule Then
                Dim contribTab As TabContribAreaRuleCtrl = New TabContribAreaRuleCtrl(logHru, pRule, isHruParent)
                TabControl1.Controls.Add(contribTab)
            ElseIf TypeOf pRule Is TemplateRule Then
                Dim tempRule As TemplateRule = CType(pRule, TemplateRule)
                If tempRule.RuleType = HruRuleType.LandUse Then
                    Dim lulcTab As TabTemplateLulcCtrl = New TabTemplateLulcCtrl(logHru, pRule, isHruParent)
                    TabControl1.Controls.Add(lulcTab)
                Else
                    Dim templateTab As TabTemplateRuleCtrl = New TabTemplateRuleCtrl(logHru, pRule, isHruParent)
                    TabControl1.Controls.Add(templateTab)
                End If
            End If
        Next
        If logHru.EliminateProcess IsNot Nothing Then
            Dim elimTab As TabEliminateCtrl = New TabEliminateCtrl(logHru.EliminateProcess)
            TabControl1.Controls.Add(elimTab)
        End If
        If logHru.CookieCutProcess IsNot Nothing Then
            Dim cookieTab As TabCookieCutCtrl = New TabCookieCutCtrl(logHru.CookieCutProcess)
            TabControl1.Controls.Add(cookieTab)
        End If
        If logHru.ReclassZonesRule IsNot Nothing Then
            Dim reclassTab As TabReclassZonesCtrl = New TabReclassZonesCtrl(logHru, logHru.ReclassZonesRule, isHruParent)
            TabControl1.Controls.Add(reclassTab)
        End If
        If isHruParent = True Then
            Dim showMessage As Boolean = False
            Dim logHruPath As String = BA_HruFolderPathFromGdbString(logHru.FilePath)
            ' Strip trailing "\" if exists
            If logHruPath(Len(logHruPath) - 1) = "\" Then
                logHruPath = logHruPath.Remove(Len(logHruPath) - 1, 1)
            End If
            If BA_Folder_ExistsWindowsIO(logHruPath) Then
                    Dim parentAoi As Aoi = BA_LoadHRUFromXml(logHruPath)
                    If parentAoi IsNot Nothing Then
                        Dim testHru As Hru = parentAoi.HruList.Item(0)
                        If testHru IsNot Nothing Then
                            If testHru.DateCreated <> logHru.DateCreated Then
                                showMessage = True
                            End If
                        Else
                            showMessage = True
                        End If
                    Else
                        showMessage = True
                    End If
                Else
                    showMessage = True
                End If
                If warnOfMissingLayers = True AndAlso showMessage = True Then
                    MessageBox.Show("The supporting files for this parent template hru have been modified or deleted.", "Missing files", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            End If
    End Sub

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    Private Sub BtnViewParent_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnViewParent.Click
        Dim frmHruTabLog As FrmHruTabLog = New FrmHruTabLog(m_aoi, TxtParentName.Text, True)
        If Not String.IsNullOrEmpty(frmHruTabLog.TxtHruName.Text) Then
            frmHruTabLog.ShowDialog()
        End If
    End Sub

    Public Function GetTabControl() As TabControl
        Return Me.TabControl1
    End Function
End Class