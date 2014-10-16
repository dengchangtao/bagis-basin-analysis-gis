Imports ESRI.ArcGIS.esriSystem

Public Class BtnTimberlineTool
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()
        'Me.Enabled = False
    End Sub

    Protected Overrides Sub OnClick()
        Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow
        Dim dockWinID As UID = New UID()
        dockWinID.Value = My.ThisAddIn.IDs.FrmTimberlineTool
        dockWindow = My.ArcMap.DockableWindowManager.GetDockableWindow(dockWinID)
        ' Get handle to UI (form) to reload lists
        Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of FrmTimberlineTool.AddinImpl)(My.ThisAddIn.IDs.FrmTimberlineTool)
        Dim reclassForm As FrmTimberlineTool = dockWindowAddIn.UI
        Dim aboutToShow As Boolean = Not dockWindow.IsVisible()

        If aboutToShow Then
            dockWindow.Caption = "Current AOI: (Not selected)"
        End If

        'Toggle dockable window
        dockWindow.Show(aboutToShow)
        ' Set dimensions of dockable window
        Dim windowPos As ESRI.ArcGIS.Framework.IWindowPosition = CType(dockWindow, ESRI.ArcGIS.Framework.IWindowPosition)
        windowPos.Height = 485
        windowPos.Width = 735
    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub
End Class
