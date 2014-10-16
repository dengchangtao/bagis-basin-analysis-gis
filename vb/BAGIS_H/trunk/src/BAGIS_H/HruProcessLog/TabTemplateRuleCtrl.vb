Imports System.Windows.Forms
Imports BAGIS_ClassLibrary

Public Class TabTemplateRuleCtrl

    Private Sub BtnViewRule_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnViewRule.Click
        Dim filePath As String = LblHruPath.Text & TxtOutputLayer.Text
        Dim displayName As String = TxtOutputLayer.Text
        BA_DisplayRaster(My.ArcMap.Application, filePath, displayName)
    End Sub

    Private Sub BtnViewInput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnViewInput.Click
        Dim filePath As String = TxtInputLayerPath.Text
        Dim displayName As String = TxtInputLayerName.Text
        BA_DisplayRaster(My.ArcMap.Application, filePath, displayName)
    End Sub

    Private Sub cboActions_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboActions.SelectedIndexChanged
        ' Add 1 to the 0-based comboBox index to get the selected rule id
        Dim actionId As Integer = cboActions.SelectedIndex + 1
        'Dim gridColumn As DataGridViewColumn = DataGridView1.Columns("ActionId")
        'Dim idx As Integer = DataGridView1.Columns.IndexOf(gridColumn)
        For Each pRow As DataGridViewRow In DataGridView1.Rows
            If pRow.Cells.Item("ActionId").Value = actionId Then
                pRow.Visible = True
            Else
                pRow.Visible = False
            End If
        Next
    End Sub
End Class
