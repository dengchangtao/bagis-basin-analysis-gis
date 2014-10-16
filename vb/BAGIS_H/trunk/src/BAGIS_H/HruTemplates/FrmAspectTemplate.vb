Imports System.Windows.Forms
Imports BAGIS_ClassLibrary

Public Class FrmAspectTemplate

    Private m_ruleId As Integer
    Private m_aoiPath As String
    ' Flag to indicate if filtering function is disabled so we know to reset
    ' the default filter values when a filter is added
    Private m_disableFilter As Boolean = False
    Private fourDirections As Double() = {-1, -0.01, 45, 135, 225, 315, 360}
    Private eightDirections As Double() = {-1, -0.01, 22.5, 67.5, 112.5, 157.5, 202.5, _
                                           247.5, 292.5, 337.5, 360}
    Private sixteenDirections As Double() = {-1, -0.01, 11.25, 33.75, 56.25, 78.75, 101.25, 123.75, _
                                             146.25, 168.75, 191.25, 213.75, 236.25, 258.75, _
                                             281.25, 303.75, 326.25, 348.75, 360}
    Private m_DefaultSelectedReclassField As String = "VALUE"


    Public Sub New(ByVal aoiPath As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        m_aoiPath = aoiPath
        CboDirections.SelectedIndex = 0
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub BtnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnApply.Click

        Dim templateActions As New List(Of TemplateAction)
        Dim reclassAction As New TemplateAction(GetNextRuleId)
        reclassAction.actionType = ActionType.ReclCont
        Dim reclassItem As ReclassItem
        Dim reclassItems(1) As ReclassItem
        Dim selectedDirections(1) As Double
        Select Case CboDirections.SelectedItem
            Case "4"
                Array.Resize(selectedDirections, fourDirections.Length)
                Array.Copy(fourDirections, selectedDirections, fourDirections.Length)
            Case "8"
                Array.Resize(selectedDirections, eightDirections.Length)
                Array.Copy(eightDirections, selectedDirections, eightDirections.Length)
            Case "16"
                Array.Resize(selectedDirections, sixteenDirections.Length)
                Array.Copy(sixteenDirections, selectedDirections, sixteenDirections.Length)
        End Select
        Array.Resize(reclassItems, selectedDirections.Length - 1)
        For i As Integer = 0 To selectedDirections.GetUpperBound(0) - 1
            reclassItem = New ReclassItem()
            reclassItem.FromValue = selectedDirections(i)
            reclassItem.ToValue = selectedDirections(i + 1)
            If i + 1 = selectedDirections.GetUpperBound(0) Then
                reclassItem.OutputValue = 1
            ElseIf reclassItem.ToValue < 0 Then
                reclassItem.OutputValue = -1
            Else
                reclassItem.OutputValue = i
            End If
            reclassItems(i) = reclassItem
        Next
        reclassAction.addParameter(ActionParameter.Directions, CStr(CboDirections.SelectedItem))
        reclassAction.addParameter(ActionParameter.ReclassField, m_DefaultSelectedReclassField)
        reclassAction.addParameter(ActionParameter.ReclassItems, reclassItems)
        templateActions.Add(reclassAction)
        Dim minIteration As Integer = 0
        Dim maxIteration As Integer = 25
        If String.IsNullOrEmpty(TxtIterations.Text) Or IsNumeric(TxtIterations.Text) = False Then
            MessageBox.Show("Numeric value required for filter iterations")
            TxtIterations.Focus()
            Exit Sub
        ElseIf CInt(TxtIterations.Text) < minIteration Or CInt(TxtFilterWidth.Text > maxIteration) Then
            MessageBox.Show("Value between " & minIteration & " and " & maxIteration & " required for filter iterations")
            TxtIterations.Focus()
            Exit Sub
        End If
        If CInt(TxtIterations.Text) > 0 Then
            Dim minFilterSize As Integer = 3
            Dim maxFilterSize As Integer = 25
            If String.IsNullOrEmpty(TxtFilterWidth.Text) Or IsNumeric(TxtFilterWidth.Text) = False Then
                MessageBox.Show("Numeric value required for filter width")
                TxtFilterWidth.Focus()
                Exit Sub
            ElseIf CInt(TxtFilterWidth.Text < minFilterSize) Or CInt(TxtFilterWidth.Text > maxFilterSize) Then
                MessageBox.Show("Value between " & minFilterSize & " and " & maxFilterSize & " required for filter width")
                TxtFilterWidth.Focus()
                Exit Sub
            End If
            If String.IsNullOrEmpty(TxtFilterHeight.Text) Or IsNumeric(TxtFilterHeight.Text) = False Then
                MessageBox.Show("Numeric value required for filter height")
                TxtFilterHeight.Focus()
                Exit Sub
            ElseIf CInt(TxtFilterHeight.Text < minFilterSize) Or CInt(TxtFilterHeight.Text > maxFilterSize) Then
                MessageBox.Show("Value between " & minFilterSize & " and " & maxFilterSize & " required for filter height")
                TxtFilterHeight.Focus()
                Exit Sub
            End If
            Dim filterAction As New TemplateAction(GetNextRuleId)
            filterAction.actionType = ActionType.MajorityFilter
            filterAction.addParameter(ActionParameter.RectangleWidth, TxtFilterWidth.Text)
            filterAction.addParameter(ActionParameter.RectangleHeight, TxtFilterHeight.Text)
            filterAction.addParameter(ActionParameter.IterationCount, TxtIterations.Text)
            templateActions.Add(filterAction)
        End If

        ' Increment ruleId before using
        Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of frmHruZone.AddinImpl)(My.ThisAddIn.IDs.frmHruZone)
        Dim hruZoneForm As frmHruZone = dockWindowAddIn.UI
        Dim ruleId As Integer
        If String.IsNullOrEmpty(TxtRuleId.Text) Then
            ruleId = hruZoneForm.GetNextRuleId
        Else
            ruleId = CInt(TxtRuleId.Text)
        End If
        Dim layerPath As String = BA_GeodatabasePath(m_aoiPath, GeodatabaseNames.Surfaces, True)
        Dim fullLayerPath As String = layerPath & BA_EnumDescription(MapsLayerName.aspect)
        Dim aspectRule As New TemplateRule(HruRuleType.Aspect, templateActions, BA_EnumDescription(MapsLayerName.aspect), _
                                           fullLayerPath, ruleId)
        hruZoneForm.AddPendingRule(aspectRule)
        BtnCancel_Click(sender, e)
    End Sub

    Private Function GetNextRuleId() As Integer
        m_ruleId = m_ruleId + 1
        Return m_ruleId
    End Function

    Public Sub LoadForm(ByVal tRule As TemplateRule)
        TxtRuleId.Text = CStr(tRule.RuleId)
        If tRule.RuleId > 0 Then
            TxtFilterHeight.Text = 0
            TxtFilterHeight.Enabled = False
            TxtFilterWidth.Text = 0
            TxtFilterWidth.Enabled = False
            TxtIterations.Text = 0
        End If
        Dim actions As List(Of TemplateAction) = tRule.TemplateActions
        For Each action In actions
            Dim params As Hashtable = action.parameters
            If action.actionType = ActionType.MajorityFilter Then
                Dim oIteration As Object = params(ActionParameter.IterationCount)
                Dim iteration As String = TryCast(oIteration, String)
                TxtIterations.Text = iteration
                Dim oHeight As Object = params(ActionParameter.RectangleHeight)
                TxtFilterHeight.Enabled = True
                TxtFilterHeight.Text = TryCast(oHeight, String)
                Dim oWidth As Object = params(ActionParameter.RectangleWidth)
                TxtFilterWidth.Enabled = True
                TxtFilterWidth.Text = TryCast(oWidth, String)
            ElseIf action.actionType = ActionType.ReclCont Then
                Dim oDirections As Object = params(ActionParameter.Directions)
                Dim directions As String = TryCast(oDirections, String)
                CboDirections.SelectedItem = directions
            End If
        Next
    End Sub

    Private Sub TxtIterations_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtIterations.TextChanged
        If IsNumeric(TxtIterations.Text) Then
            If CInt(TxtIterations.Text) < 1 Then
                TxtFilterHeight.Text = 0
                TxtFilterHeight.Enabled = False
                TxtFilterWidth.Text = 0
                TxtFilterWidth.Enabled = False
                m_disableFilter = True
            Else
                If m_disableFilter = True Then
                    TxtFilterHeight.Enabled = True
                    TxtFilterWidth.Enabled = True
                    TxtFilterWidth.Text = 5
                    TxtFilterHeight.Text = 5
                    m_disableFilter = False
                End If
            End If
        End If
    End Sub

    Private Sub BtnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAbout.Click
        Dim toolHelpForm As FrmHelp = New FrmHelp(BA_HelpTopics.AspectTemplate)
        toolHelpForm.ShowDialog()
    End Sub

End Class