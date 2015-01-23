Imports ESRI.ArcGIS.esriSystem
Imports BAGIS_ClassLibrary

Public Class BtnZonalStats
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()
        'Me.Enabled = False
    End Sub

    Protected Overrides Sub OnClick()

        Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow
        Dim dockWinID As UID = New UIDClass()
        dockWinID.Value = My.ThisAddIn.IDs.FrmZonalStats
        dockWindow = My.ArcMap.DockableWindowManager.GetDockableWindow(dockWinID)
        ' Get handle to UI (form) to reload lists
        Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of FrmZonalStats.AddinImpl)(My.ThisAddIn.IDs.FrmZonalStats)
        Dim zonalForm As FrmZonalStats = dockWindowAddIn.UI
        dockWindow.Show((Not dockWindow.IsVisible()))
        ' Set dimensions of dockable window
        Dim windowPos As ESRI.ArcGIS.Framework.IWindowPosition = CType(dockWindow, ESRI.ArcGIS.Framework.IWindowPosition)
        windowPos.Height = 510
        windowPos.Width = 630
        dockWindow.Caption = "Post Processing : Zonal Statistics"

        If dockWindow.IsVisible Then
            ' Set AOI if already set
            Dim hruExt As HruExtension = HruExtension.GetExtension
            Dim aoi As Aoi = hruExt.aoi
            If aoi IsNot Nothing Then
                zonalForm.TxtAoiPath.Text = aoi.FilePath
                zonalForm.LoadHruLayers(aoi)
                zonalForm.LoadLstLayers(aoi)
            Else
                'Reset the Eliminate Poly Form
                zonalForm.ResetForm()
            End If
        End If
    End Sub


    Protected Overrides Sub OnUpdate()

    End Sub
End Class
