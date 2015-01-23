Imports System.Windows.Forms
Imports BAGIS_ClassLibrary

Public Class FrmHruProcessLog

    Dim m_hru As Hru

    Public Sub New(ByVal aoi As Aoi, ByVal hruName As String)
        InitializeComponent()
        Me.Text = "Current HRU: " & hruName
        TxtAoiName.Text = aoi.Name
        TxtAoiPath.Text = aoi.FilePath
        For Each pHru In aoi.HruList
            ' We found the hru the user selected
            If String.Compare(pHru.Name, hruName) = 0 Then
                m_hru = pHru
                TxtHruName.Text = pHru.Name
                TxtHruPath.Text = pHru.FilePath
                Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder
                'If pHru.HruZones IsNot Nothing Then
                '    For Each pZone In pHru.HruZones
                '        sb.Append(CStr(pZone.Id))
                '        sb.Append(", ")
                '    Next
                '    ' Remove trailing comma
                '    sb.Remove(sb.Length - 2, 2)
                'End If
                TxtApplyZones.Text = sb.ToString
                TxtNonContiguous.Text = NO
                If pHru.AllowNonContiguousHru = True Then TxtNonContiguous.Text = YES
                TxtPolygonCount.Text = pHru.FeatureCount
                TxtDateCreated.Text = pHru.DateCreatedValue
                If pHru.RuleList IsNot Nothing Then
                    For Each pRule In pHru.RuleList
                        Dim ruleType As String = pRule.RuleTypeText
                        Dim layer As String = pRule.InputLayerName
                        Dim ruleId As String = CStr(pRule.RuleId)
                        Dim pArray As String() = {ruleType, layer, ruleId}
                        Me.DataGridView1.Rows.Add(pArray)
                    Next
                End If
            End If
        Next
    End Sub

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        If IsANonHeaderLinkCell(e) Then
            Dim idColumn As DataGridViewColumn = DataGridView1.Columns.Item("RuleId")
            Dim idColumnIdx As Integer = idColumn.Index
            Dim value As Object = DataGridView1.Rows(e.RowIndex).Cells(idColumnIdx).Value
            Dim ruleId As Integer = CStr(value)
            'For Each pRule In m_hru.RuleList
            '    If pRule.RuleId = ruleId Then
            '        Dim frmRuleLog As New FrmRuleProcessLog(m_hru, pRule)
            '        frmRuleLog.ShowDialog()
            '    End If
            'Next
            'Dim ruleTabs As FrmRuleTabLog = New FrmRuleTabLog(m_hru, ruleId)
            'ruleTabs.Show()
        End If
    End Sub

    Private Function IsANonHeaderLinkCell(ByVal cellEvent As DataGridViewCellEventArgs) As Boolean
        If TypeOf DataGridView1.Columns(cellEvent.ColumnIndex) _
            Is DataGridViewLinkColumn _
            AndAlso Not cellEvent.RowIndex = -1 Then _
            Return True Else Return False
    End Function

End Class