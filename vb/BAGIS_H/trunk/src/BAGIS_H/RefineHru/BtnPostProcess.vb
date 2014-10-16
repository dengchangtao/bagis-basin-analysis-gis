Imports ESRI.ArcGIS.esriSystem
Imports BAGIS_ClassLibrary

Public Class BtnPostProcess
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()
        'Me.Enabled = False
    End Sub

    Protected Overrides Sub OnClick()
        Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow
        Dim dockWinID As UID = New UIDClass()
        dockWinID.Value = My.ThisAddIn.IDs.FrmEliminatePoly
        dockWindow = My.ArcMap.DockableWindowManager.GetDockableWindow(dockWinID)
        ' Get handle to UI (form) to reload lists
        Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of FrmEliminatePoly.AddinImpl)(My.ThisAddIn.IDs.FrmEliminatePoly)
        Dim eliminateForm As FrmEliminatePoly = dockWindowAddIn.UI
        dockWindow.Show((Not dockWindow.IsVisible()))
        ' Set dimensions of dockable window
        Dim windowPos As ESRI.ArcGIS.Framework.IWindowPosition = CType(dockWindow, ESRI.ArcGIS.Framework.IWindowPosition)
        windowPos.Height = 490
        windowPos.Width = 530
        dockWindow.Caption = "Post Processing : Eliminate"

        If dockWindow.IsVisible Then
            ' Set AOI if already set
            Dim hruExt As HruExtension = HruExtension.GetExtension
            Dim aoi As Aoi = hruExt.aoi
            If aoi IsNot Nothing Then
                eliminateForm.TxtAOIPath.Text = aoi.FilePath
                eliminateForm.LoadHruLayers(aoi)
            Else
                'Reset the Eliminate Poly Form
                eliminateForm.ResetEliminateFrm()
            End If
        End If
    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub

End Class