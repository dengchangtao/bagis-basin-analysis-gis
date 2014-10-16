Imports ESRI.ArcGIS.esriSystem
Imports BAGIS_ClassLibrary

Public Class BtnReclassZones
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()

    End Sub

    Protected Overrides Sub OnClick()
        Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow
        Dim dockWinID As UID = New UIDClass()
        dockWinID.Value = My.ThisAddIn.IDs.FrmReclassZones
        dockWindow = My.ArcMap.DockableWindowManager.GetDockableWindow(dockWinID)
        ' Get handle to UI (form) to reload lists
        Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of FrmReclassZones.AddinImpl)(My.ThisAddIn.IDs.FrmReclassZones)
        Dim reclassForm As FrmReclassZones = dockWindowAddIn.UI
        Dim aboutToShow As Boolean = Not dockWindow.IsVisible()

        If aboutToShow Then
                'Set AOI if already set
            Dim hruExt As HruExtension = HruExtension.GetExtension
            Dim aoi As Aoi = hruExt.aoi
            If aoi IsNot Nothing Then
                dockWindow.Caption = "Current AOI: " & aoi.Name
                reclassForm.LoadForm()
            Else
                dockWindow.Caption = "Current AOI: (Not selected)"
            End If
        End If

        'Toggle dockable window
        dockWindow.Show(aboutToShow)
        ' Set dimensions of dockable window
        Dim windowPos As ESRI.ArcGIS.Framework.IWindowPosition = CType(dockWindow, ESRI.ArcGIS.Framework.IWindowPosition)
        windowPos.Height = 475
        windowPos.Width = 577

    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub
End Class
