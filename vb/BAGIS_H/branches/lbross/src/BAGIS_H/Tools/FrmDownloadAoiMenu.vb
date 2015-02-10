Imports System.Windows.Forms

Public Class FrmDownloadAoiMenu

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        '---create a row---
        Dim item As New DataGridViewRow
        item.CreateCells(AoiGrid)
        With item
            .Cells(0).Value = "Price_R_at_Woodside_01232014"
            .Cells(1).Value = "23-JAN-2014"
            .Cells(2).Value = "G. Duh"
            .Cells(3).Value = False
            .Cells(4).Value = "Updated AOI with new gauge station"
        End With
        Dim item2 As New DataGridViewRow
        item2.CreateCells(AoiGrid)
        With item2
            .Cells(0).Value = "Santa_Fe_R_nr_Santa_Fe_11302012"
            .Cells(1).Value = "11-NOV-2012"
            .Cells(2).Value = "D. Garen"
            .Cells(3).Value = False
            .Cells(4).Value = "Initial upload of AOI; Includes HRU definition"
        End With
        '---add the row---
        AoiGrid.Rows.Add(item)
        AoiGrid.Rows.Add(item2)
        AoiGrid.ClearSelection()
        AoiGrid.CurrentCell = Nothing
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

End Class