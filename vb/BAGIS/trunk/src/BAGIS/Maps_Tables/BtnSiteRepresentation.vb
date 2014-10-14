Imports ESRI.ArcGIS.esriSystem
Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.Desktop.AddIns

Public Class BtnSiteRepresentation
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()
        Me.Enabled = False

    End Sub

    Protected Overrides Sub OnClick()

        Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow
        Dim dockWinID As UID = New UIDClass()
        dockWinID.Value = My.ThisAddIn.IDs.frmSiteRepresentations
        dockWindow = My.ArcMap.DockableWindowManager.GetDockableWindow(dockWinID)
        ' Get handle to UI (form) to reload lists
        Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of frmSiteRepresentations.AddinImpl)(My.ThisAddIn.IDs.frmSiteRepresentations)
        Dim siteRepresentationForm As frmSiteRepresentations = dockWindowAddIn.UI
        dockWindow.Show((Not dockWindow.IsVisible()))
        ' Set dimensions of dockable window
        Dim windowPos As ESRI.ArcGIS.Framework.IWindowPosition = CType(dockWindow, ESRI.ArcGIS.Framework.IWindowPosition)
        windowPos.Height = 620
        windowPos.Width = 555

        Dim comboBox = AddIn.FromID(Of cboTargetedAOI)(My.ThisAddIn.IDs.cboTargetedAOI)
        Dim aoiName As String = comboBox.getValue()

        If Len(aoiName) = 0 Then
            MsgBox("Please select an AOI!")
        Else
            If Not dockWindowAddIn.Ready(aoiName) Then
                dockWindow.Caption = "Display Site Representations (Current AOI --> " & aoiName & ")"
                siteRepresentationForm.LoadAOIInfo(AOIFolderBase)
            End If
        End If

    End Sub

    Public WriteOnly Property selectedProperty As Boolean
        Set(ByVal value As Boolean)
            Me.Enabled = value
        End Set
    End Property

    Protected Overrides Sub OnUpdate()

    End Sub
End Class
