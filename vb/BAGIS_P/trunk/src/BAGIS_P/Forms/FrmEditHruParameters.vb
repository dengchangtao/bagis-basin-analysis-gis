Imports Microsoft.VisualBasic.FileIO
Imports System.Windows.Forms
Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.Geodatabase
Imports System.ComponentModel

Public Class FrmEditHruParameters
    Dim m_parentForm As FrmExportParametersOms
    Dim m_spatialParamsTable As Hashtable
    Dim m_templatePath As String
    Dim m_token As String
    Dim m_nhru As Integer
    Dim m_reqSpatialParameters As IList(Of String)
    Dim m_missingSpatialParameters As IList(Of String)
    Dim m_missingValue As Integer

    Public Sub New(ByVal parentForm As FrmExportParametersOms, ByVal templatePath As String, ByVal token As String, _
                   ByVal reqSpatialParameters As IList(Of String), ByVal missingSpatialParameters As IList(Of String), _
                   ByVal spatialParamsTable As Hashtable, ByVal missingValue As Integer)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_templatePath = templatePath
        m_token = token
        m_parentForm = parentForm
        m_nhru = CInt(m_parentForm.TxtNHru.Text)
        m_reqSpatialParameters = reqSpatialParameters
        m_missingSpatialParameters = missingSpatialParameters
        m_spatialParamsTable = spatialParamsTable
        m_missingValue = missingValue

        LoadGrid()
    End Sub

    Private Sub LoadGrid()
        Dim paramTable As ITable = Nothing
        Dim pCursor As ICursor = Nothing
        Dim pRow As IRow = Nothing

        Try
            If m_spatialParamsTable Is Nothing Then
                m_spatialParamsTable = BA_ReadNhruParams(m_templatePath, m_token, m_nhru, m_reqSpatialParameters, m_missingSpatialParameters, m_missingValue)
            End If

            ' Open the parameters table so we can populate the ERAMS_ID and HRU_ID before we populate the parameters
            ' Sorted by HRU ID
            Dim selItem As LayerListItem = TryCast(m_parentForm.LstHruLayers.SelectedItem, LayerListItem)
            Dim hruPath As String = BA_GetHruPath(m_parentForm.TxtAoiPath.Text, PublicPath.HruDirectory, selItem.Name)
            Dim hruParamPath As String = hruPath & BA_EnumDescription(PublicPath.BagisParamGdb)
            Dim selProfile As String = TryCast(m_parentForm.LstProfiles.SelectedItem, String)
            If selProfile IsNot Nothing Then
                Dim tableName As String = selProfile & BA_PARAM_TABLE_SUFFIX
                paramTable = BA_OpenTableFromGDB(hruParamPath, tableName)
                If paramTable IsNot Nothing Then
                    'Dim idxERamsId As Integer = paramTable.FindField(BA_FIELD_ERAMS_ID)
                    Dim idxHruId As Integer = paramTable.FindField(BA_FIELD_HRU_ID)
                    pCursor = paramTable.Search(Nothing, False)
                    pRow = pCursor.NextRow
                    While pRow IsNot Nothing
                        '---create a row---
                        Dim item As New DataGridViewRow
                        item.CreateCells(GrdParam)
                        '---populate the ERAMS_ID ---
                        'Dim eRamsId As Long = CLng(pRow.Value(idxERamsId))
                        'Dim dgvColumn As DataGridViewColumn = GrdParam.Columns(BA_FIELD_ERAMS_ID)
                        'item.Cells(dgvColumn.Index).Value = eRamsId
                        '---populate the HRU_ID ---
                        Dim hruId As Long = CLng(pRow.Value(idxHruId))
                        Dim dgvColumn As DataGridViewColumn = GrdParam.Columns(BA_FIELD_HRU_ID)
                        item.Cells(dgvColumn.Index).Value = hruId
                        '---add the row---
                        GrdParam.Rows.Add(item)
                        pRow = pCursor.NextRow
                    End While
                    GrdParam.Sort(GrdParam.Columns(0), ListSortDirection.Ascending)
                    GrdParam.ClearSelection()
                    GrdParam.Visible = True
                End If
            End If

            AddColumnsToGrid()

            For Each key As String In m_spatialParamsTable.Keys
                Dim values As IList(Of String) = m_spatialParamsTable(key)
                Dim idx As Integer = 0
                For Each row As DataGridViewRow In GrdParam.Rows
                    row.Cells(key).Value = values.Item(idx)
                    idx += 1
                Next
            Next
            GrdParam.ClearSelection()
            GrdParam.Visible = True
        Catch ex As Exception
            Debug.Print("LoadGrid: " & ex.Message)

        Finally
            paramTable = Nothing
            pCursor = Nothing
            pRow = Nothing
        End Try
    End Sub

    Private Sub AddColumnsToGrid()
        Dim keys As ICollection = m_spatialParamsTable.Keys
        Dim keysA(m_spatialParamsTable.Count - 1) As String
        keys.CopyTo(keysA, 0)
        Array.Sort(keysA)

        For Each key As String In keysA
            Dim newColumn As DataGridViewTextBoxColumn = New DataGridViewTextBoxColumn()
            newColumn.Name = key
            newColumn.HeaderText = key
            newColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            Dim newColumnStyle As DataGridViewCellStyle = New DataGridViewCellStyle
            newColumnStyle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            newColumn.DefaultCellStyle = newColumnStyle
            'newColumn.ReadOnly = True
            newColumn.Width = 85
            If m_missingSpatialParameters.Contains(key) Then
                newColumn.HeaderCell.Style.ForeColor = Drawing.Color.Red
            End If
            newColumn.SortMode = DataGridViewColumnSortMode.Programmatic
            GrdParam.Columns.Add(newColumn)
        Next
    End Sub

    Private Sub BtnClose_Click(sender As System.Object, e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    Private Sub BtnApply_Click(sender As System.Object, e As System.EventArgs) Handles BtnApply.Click
        Dim hTable As Hashtable = New Hashtable
        Try
            For i As Integer = 0 To GrdParam.Columns.Count - 1
                Dim nextColumn As DataGridViewColumn = GrdParam.Columns(i)
                Dim values As IList(Of String) = New List(Of String)
                For Each nextRow As DataGridViewRow In GrdParam.Rows
                    values.Add(nextRow.Cells(i).Value)
                Next
                hTable(nextColumn.Name) = values
            Next
            m_parentForm.SpatialParamsTable = hTable
            Me.Close()
        Catch ex As Exception
            Debug.Print("BtnApply_Click: " & ex.Message)
            MessageBox.Show("An error occurred while trying to save updated parameters", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
End Class