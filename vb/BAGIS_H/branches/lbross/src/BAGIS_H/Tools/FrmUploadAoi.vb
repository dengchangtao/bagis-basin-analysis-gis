Imports System.Windows.Forms

Public Class FrmUploadAoi

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        '---create a row---
        Dim item As New DataGridViewRow
        item.CreateCells(LayerGrid)
        With item
            .Cells(0).Value = "Pourpoint"
            .Cells(1).Value = "aoi"
            .Cells(2).Value = "update"
            .Cells(3).Value = True
        End With
        Dim item2 As New DataGridViewRow
        item2.CreateCells(LayerGrid)
        With item2
            .Cells(0).Value = "state_boundaries"
            .Cells(1).Value = "optional"
            .Cells(2).Value = "add"
            .Cells(3).Value = True
        End With
        Dim item3 As New DataGridViewRow
        item3.CreateCells(LayerGrid)
        With item3
            .Cells(0).Value = "gopher_holes"
            .Cells(1).Value = "optional"
            .Cells(2).Value = "delete"
            .Cells(3).Value = True
        End With
        Dim item4 As New DataGridViewRow
        item4.CreateCells(LayerGrid)
        With item4
            .Cells(0).Value = "cov_den"
            .Cells(1).Value = "optional"
            .Cells(2).Value = "update"
            .Cells(3).Value = True
        End With
        Dim item5 As New DataGridViewRow
        item5.CreateCells(LayerGrid)
        With item5
            .Cells(0).Value = "slope_pct"
            .Cells(1).Value = "surfaces"
            .Cells(2).Value = "add"
            .Cells(3).Value = True
        End With
        '---add the rows---
        LayerGrid.Rows.Add(item)
        LayerGrid.Rows.Add(item2)
        LayerGrid.Rows.Add(item3)
        LayerGrid.Rows.Add(item4)
        LayerGrid.Rows.Add(item5)
        LayerGrid.Sort(LayerGrid.Columns(2), System.ComponentModel.ListSortDirection.Descending)
    End Sub

End Class