Imports System.Windows.Forms
Imports System.Text
Imports BAGIS_ClassLibrary

Public Class FrmEditParameters

    Dim m_parentForm As FrmExportParametersOms
    Dim m_paramsTable As Hashtable
    Dim m_tablesTable As Hashtable
    'key: dimension name, value: List of ParameterTables
    Dim m_dimensionTables As Hashtable
    Dim m_modifiedParameterTable As ParameterTable

    Public Sub New(ByVal parentForm As FrmExportParametersOms, ByVal paramsTable As Hashtable, ByVal tablesTable As Hashtable, _
                   ByVal parameterPath As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        If paramsTable Is Nothing Then
            m_paramsTable = BA_GetParameterMap(parameterPath, ",", CInt(parentForm.TxtNHru.Text), parentForm.TxtAoiPath.Text)
        Else
            'The params table will exist if we previously edited it
            m_paramsTable = paramsTable
        End If
        If tablesTable Is Nothing Then
            m_tablesTable = BA_GetTableMap(parameterPath, ",", m_paramsTable)
        Else
            m_tablesTable = tablesTable
        End If
        Dim paramDescrPath As String = BA_GetParameterDescriptionPath()
        Dim descrHash As Hashtable = Nothing
        If BA_File_ExistsWindowsIO(paramDescrPath) Then
            descrHash = BA_GetParameterDescriptionHash(paramDescrPath, ",")
        Else
            'Geoffrey does not want a warning
            'MessageBox.Show("Unable to locate parameter description file at " & paramDescrPath & ".", "Missing configuration file", _
            '                MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        If m_paramsTable.Keys.Count > 0 Or m_tablesTable.Keys.Count > 0 Then
            RefreshGrid(descrHash)
        End If
        m_dimensionTables = BA_GetTablesByDimension(m_tablesTable)
        m_parentForm = parentForm

        'RefreshComboBox()
        'Test
        'Dim rectArray(,) As String = {{1, 2, 3}, {3, 4, 5}, {5, 6, 7}, {7, 8, 9}, {0, 1, 2}}
        'Dim headArray() As String = {"a", "b", "c", "d", "e"}
        'Dim pTable As ParameterTable = New ParameterTable("tableName", "dim1", rectArray, headArray)

    End Sub

    Private Sub RefreshGrid(ByVal descrTable As Hashtable)
        Dim sortCol As DataGridViewColumn = DataGridView1.Columns(0)
        DataGridView1.Rows.Clear()
        If m_paramsTable IsNot Nothing Then
            Dim keys As ICollection = m_paramsTable.Keys
            Dim param As Parameter = Nothing
            For Each pKey As String In keys
                param = CType(m_paramsTable(pKey), Parameter)
                Dim pRow As New DataGridViewRow
                pRow.CreateCells(DataGridView1)
                '---parameter name
                pRow.Cells(0).Value = pKey
                '---populate value
                Dim pValue As String = ""
                If param.value IsNot Nothing Then
                    If param.value.GetUpperBound(0) = 0 Then
                        pValue = param.value(0)
                    Else
                        Dim sb As StringBuilder = New StringBuilder
                        For i As Integer = 0 To param.value.GetUpperBound(0)
                            sb.Append(param.value(i))
                            sb.Append(",")
                        Next
                        pValue = sb.ToString
                        If pValue.Length > 1 Then
                            pValue = pValue.TrimEnd(",")
                        End If
                    End If
                End If
                pRow.Cells(1).Value = pValue
                '---add the description, if available---
                Dim paramDescr As String = "No description available"
                If descrTable IsNot Nothing AndAlso descrTable(pKey) IsNot Nothing Then
                    If String.IsNullOrEmpty(descrTable(pKey)) Then
                        paramDescr = "No description available (null)"
                    Else
                        paramDescr = descrTable(pKey)
                    End If
                End If
                pRow.Cells(2).Value = paramDescr
                pRow.Cells(3).Value = param.isDimension
                '---add the row---
                DataGridView1.Rows.Add(pRow)
            Next
        End If
        Dim tableStyle As DataGridViewCellStyle = New DataGridViewCellStyle
        tableStyle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        tableStyle.ForeColor = Drawing.Color.Blue

        If m_tablesTable IsNot Nothing Then
            Dim keys As ICollection = m_tablesTable.Keys
            Dim pTable As ParameterTable = Nothing
            For Each pKey As String In keys
                pTable = CType(m_tablesTable(pKey), ParameterTable)
                Dim pRow As New DataGridViewRow
                pRow.CreateCells(DataGridView1)
                '---parameter name
                pRow.Cells(0).Value = pKey
                Dim nextLine As String = "Table"
                pRow.Cells(1).Value = nextLine
                pRow.Cells(1).ReadOnly = True
                pRow.Cells(1).Style = tableStyle
                '---add the description, if available---
                Dim paramDescr As String = "No description available"
                If descrTable IsNot Nothing AndAlso descrTable(pKey) IsNot Nothing Then
                    If String.IsNullOrEmpty(descrTable(pKey)) Then
                        paramDescr = "No description available (null)"
                    Else
                        paramDescr = descrTable(pKey)
                    End If
                End If
                pRow.Cells(2).Value = paramDescr
                pRow.Cells(3).Value = False
                '---add the row---
                DataGridView1.Rows.Add(pRow)
            Next
        End If
        DataGridView1.Sort(sortCol, System.ComponentModel.ListSortDirection.Ascending)
    End Sub

    'Private Sub RefreshComboBox()
    '    CboDimensions.Items.Clear()
    '    CboDimensions.Items.Add("")
    '    Dim keys As ICollection = m_dimensionTables.Keys
    '    Dim paramTable As ParameterTable = Nothing
    '    For Each pKey As String In keys
    '        CboDimensions.Items.Add(pKey)
    '    Next
    '    CboDimensions.SelectedIndex = 0
    'End Sub

    'Private Sub CboDimensions_SelectedIndexChanged(sender As System.Object, e As System.EventArgs)
    '    Dim selValue As String = CStr(CboDimensions.SelectedItem)
    '    If Not String.IsNullOrEmpty(selValue) Then
    '        Dim paramTables As IList(Of ParameterTable) = m_dimensionTables(selValue)
    '        If paramTables IsNot Nothing Then
    '            'Add headers to display table
    '            Dim firstTable As ParameterTable = paramTables(0)
    '            AddColumnsToTable(firstTable.Headers)
    '            AddRowsToTable(paramTables)
    '        End If
    '    Else
    '        DataGridView2.Visible = False
    '        DataGridView1.Visible = True
    '    End If
    'End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Dim colIdx As Integer = e.ColumnIndex
        Dim rowIdx As Integer = e.RowIndex
        If colIdx = 1 Then
            Dim pValue As String = DataGridView1.Rows(rowIdx).Cells(colIdx).Value
            Dim paramName As String = DataGridView1.Rows(rowIdx).Cells(0).Value
            'We are editing a table
            If pValue = "Table" Then
                Dim paramTable As ParameterTable = m_tablesTable(paramName)
                Dim dimension1Value As String = ""
                Dim dimension2Value As String = ""
                If Not String.IsNullOrEmpty(paramTable.Dimension1) Then
                    dimension1Value = BA_GetValueForDimension(m_paramsTable, paramTable.Dimension1)
                End If
                If Not String.IsNullOrEmpty(paramTable.Dimension2) Then
                    dimension2Value = BA_GetValueForDimension(m_paramsTable, paramTable.Dimension2)
                End If
                Dim paramDescr As String = DataGridView1.Rows(rowIdx).Cells(2).Value
                Dim frmParamTable As FrmParameterTable = New FrmParameterTable(Me, paramTable, dimension1Value, dimension2Value, paramDescr)
                frmParamTable.ShowDialog()
                If m_modifiedParameterTable IsNot Nothing Then
                    'Replace table in map with edited table
                    m_tablesTable(m_modifiedParameterTable.Name) = m_modifiedParameterTable
                    'Reset variable
                    m_modifiedParameterTable = Nothing
                End If
            Else
                'We are editing a parameter
                Dim selParam As Parameter = m_paramsTable(paramName)
                If selParam IsNot Nothing Then
                    selParam.value(0) = pValue
                    selParam.isDimension = DataGridView1.Rows(rowIdx).Cells(1).Value
                Else
                    Dim arrValue As String() = {pValue}
                    selParam = New Parameter(paramName, arrValue, DataGridView1.Rows(rowIdx).Cells(1).Value)
                End If
                m_paramsTable(paramName) = selParam
            End If
        End If
    End Sub

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    Public WriteOnly Property ModifiedParameterTable As ParameterTable
        Set(value As ParameterTable)
            m_modifiedParameterTable = value
        End Set
    End Property

    Private Sub BtnApply_Click(sender As System.Object, e As System.EventArgs) Handles BtnApply.Click
        'Clear the parameters table so we only have parameters that were on the grid
        'This handles deletes
        m_paramsTable.Clear()
        Dim listGridKeys As IList(Of String) = New List(Of String)
        For i As Integer = 0 To DataGridView1.RowCount - 1
            Dim pValue As String = DataGridView1.Rows(i).Cells(1).Value
            Dim paramName As String = DataGridView1.Rows(i).Cells(0).Value
            If paramName <> "" AndAlso pValue <> "Table" Then
                'Only allow numbers for the value
                If pValue <> "" AndAlso Not IsNumeric(pValue) Then
                    MessageBox.Show("Only numeric values are allowed.")
                    DataGridView1.Rows(i).Cells(1).Selected = True
                    DataGridView1.CurrentCell = DataGridView1.Rows(i).Cells(1)
                    Exit Sub
                End If
                'We are editing a parameter
                Dim selParam As Parameter = m_paramsTable(paramName)
                If selParam IsNot Nothing Then
                    selParam.value(0) = pValue
                    selParam.isDimension = DataGridView1.Rows(i).Cells(1).Value
                Else
                    Dim arrValue As String() = {pValue}
                    selParam = New Parameter(paramName, arrValue, DataGridView1.Rows(i).Cells(1).Value)
                End If
                m_paramsTable(paramName) = selParam
            ElseIf paramName <> "" AndAlso pValue = "Table" Then
                'Compile a list of the table keys that are on the grid
                listGridKeys.Add(paramName)
            End If
        Next
        m_parentForm.ParamsTable = m_paramsTable
        'Check for deletes of tables
        Dim tableKeys As ICollection = m_tablesTable.Keys
        Dim listKeys As IList(Of String) = New List(Of String)
        For Each pKey As String In tableKeys
            listKeys.Add(pKey)
        Next
        For Each lKey As String In listKeys
            'If grid doesn't contain table key, remove it from the hashtable
            If Not listGridKeys.Contains(lKey) Then
                m_tablesTable.Remove(lKey)
            End If
        Next
        m_parentForm.TablesTable = m_tablesTable
        Me.Close()
    End Sub
End Class