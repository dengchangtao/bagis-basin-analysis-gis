Imports System.Windows.Forms

Public Class FrmParameterTable

    Dim m_paramTable As ParameterTable
    Dim m_parentForm As FrmEditParameters

    Public Sub New(ByVal parentForm As FrmEditParameters, ByVal paramTable As ParameterTable, ByVal dimension1Value As String, _
                   ByVal dimension2Value As String, ByVal paramDescription As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Text = paramTable.Name
        AddColumnsToTable(paramTable.Headers)
        AddRowsToTable(paramTable.Values)
        TxtDimension1.Text = paramTable.Dimension1
        TxtDimension2.Text = paramTable.Dimension2
        TxtDim1Value.Text = dimension1Value
        TxtDim2Value.Text = dimension2Value
        TxtDescription.Text = paramDescription
        m_paramTable = paramTable
        m_parentForm = parentForm
    End Sub

    Private Sub AddColumnsToTable(ByVal pHeaders As String())
        For i As Integer = DataGridView1.Columns.Count - 1 To 0 Step -1
            Dim pCol As DataGridViewColumn = DataGridView1.Columns(i)
            DataGridView1.Columns.Remove(pCol)
        Next

        For i As Integer = 0 To pHeaders.Length - 1
            Dim nextHeader As String = pHeaders(i)
            Dim newColumn As DataGridViewTextBoxColumn = New DataGridViewTextBoxColumn()
            newColumn.Name = nextHeader
            newColumn.HeaderText = nextHeader
            newColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            Dim newColumnStyle As DataGridViewCellStyle = New DataGridViewCellStyle
            newColumnStyle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            newColumn.DefaultCellStyle = newColumnStyle
            newColumn.ReadOnly = False
            newColumn.Width = 85
            newColumn.SortMode = DataGridViewColumnSortMode.Programmatic
            DataGridView1.Columns.Add(newColumn)
        Next
    End Sub

    Private Sub AddRowsToTable(ByVal paramValues As String(,))
        For i As Integer = 0 To paramValues.GetUpperBound(0)
            '---create a row---
            Dim item As New DataGridViewRow
            item.CreateCells(DataGridView1)
            For j As Integer = 0 To paramValues.GetUpperBound(1)
                item.Cells(j).Value = paramValues(i, j)
            Next
            DataGridView1.Rows.Add(item)
        Next
    End Sub

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    Private Sub BtnApply_Click(sender As System.Object, e As System.EventArgs) Handles BtnApply.Click
        Dim values(DataGridView1.RowCount - 1, DataGridView1.ColumnCount - 1) As String
        For i As Integer = 0 To DataGridView1.RowCount - 1
            For j As Integer = 0 To DataGridView1.ColumnCount - 1
                values(i, j) = DataGridView1.Rows(i).Cells(j).Value
            Next
        Next
        Dim newParamTable As ParameterTable = New ParameterTable(m_paramTable.Name, m_paramTable.Dimension1, m_paramTable.Dimension2, _
                                                                 values, m_paramTable.Headers)
        m_parentForm.ModifiedParameterTable = newParamTable
        Me.Close()
    End Sub
End Class