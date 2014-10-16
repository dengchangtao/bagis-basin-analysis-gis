Imports ESRI.ArcGIS.esriSystem

Public Class BtnHruZoneDelete
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()

    End Sub

    Protected Overrides Sub OnClick()
        ' Check to see if spatial analyst is enabled, if not throw an error
        If BAGIS_ClassLibrary.BA_Enable_SAExtension(My.ArcMap.Application) = ESRI.ArcGIS.esriSystem.esriExtensionState.esriESEnabled Then
            Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow
            Dim dockWinID As UID = New UIDClass()
            dockWinID.Value = My.ThisAddIn.IDs.frmHruZone
            dockWindow = My.ArcMap.DockableWindowManager.GetDockableWindow(dockWinID)
            ' Get handle to UI (form) to reload lists
            Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of frmHruZone.AddinImpl)(My.ThisAddIn.IDs.frmHruZone)
            Dim hruZoneForm As frmHruZone = dockWindowAddIn.UI
            hruZoneForm.ReloadListLayers()
            dockWindow.Show((Not dockWindow.IsVisible()))
            ' Set dimensions of dockable window
            Dim windowPos As ESRI.ArcGIS.Framework.IWindowPosition = CType(dockWindow, ESRI.ArcGIS.Framework.IWindowPosition)
            windowPos.Height = 605
            windowPos.Width = 675
        Else
            Windows.Forms.MessageBox.Show("Spatial Analyst is required for HRU delineation and is not available.")
        End If

    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub
End Class
