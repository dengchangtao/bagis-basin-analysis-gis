Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Desktop.AddIns
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.CatalogUI

Public Class frmAOI_Tool

    Private Sub frmAOI_Tool_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim cboSelectAOI = AddIn.FromID(Of cboTargetedAOI)(My.ThisAddIn.IDs.cboTargetedAOI)
        Dim aoiName As String = ""
        If Not String.IsNullOrEmpty(cboSelectAOI.getValue) Then
            aoiName = cboSelectAOI.getValue
            If AOIFolderBase Is Nothing Then
                AOIFolderBase = BasinFolderBase & "\" & aoiName
            End If
        End If
        If Not String.IsNullOrEmpty(aoiName) Then
            lstAOIList.Items.Add(aoiName)
            lstAOIList.SelectedIndex = 0
        ElseIf Not String.IsNullOrEmpty(AOIFolderBase) Then
            aoiName = BA_GetBareName(AOIFolderBase)
            lstAOIList.Items.Add(aoiName)
            lstAOIList.SelectedIndex = 0
        End If
        If Not String.IsNullOrEmpty(BasinFolderBase) Then
            lblBasin.Text = BA_GetBareName(BasinFolderBase)
        End If
        If lstAOIList.Items.Count > 0 Then
            lstAOIList.SelectedIndex = 0
            'lblAOISelected.Text = lstAOIList.SelectedItem
        End If
        If lstAOIList.Items.Count = 0 Then
            CmdAOIView.Enabled = False
            CmdDeleteAOI.Enabled = False
            CmdSelectAOI.Enabled = False
            CmdViewLayers.Enabled = False
        End If
        Dim pMxDoc As IMxDocument = My.ArcMap.Document
        BA_ToggleView(pMxDoc, True) 'switch to data viewMy.ArcMap.DockableWindowManager
        Refresh_AOIForm()
    End Sub
    Protected Overrides Sub Finalize()
        MyBase.Finalize()
        Call CoFreeUnusedLibraries()
    End Sub
    Private Sub CmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdExit.Click
        Me.Close()
    End Sub

    Private Sub CmdNewAOI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdNewAOI.Click
        Me.Hide()
        Dim myform As New frmPourPoint
        myform.ShowDialog()
    End Sub

    Public myListItem As String

    Public WriteOnly Property myProperty As String
        Set(ByVal value As String)
            value = lstAOIList.SelectedItem
        End Set
    End Property

    Private Sub CmdAOIView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdAOIView.Click
        Dim response As Integer
        Dim strPathandName As String
        'I added this line
        'strPathandName = BasinFolderBase & "\" & lstAOIList.List(lstAOIList.ListIndex) & "\" & BA_AOIExtentCoverage
        'Me.Hide()
        strPathandName = BasinFolderBase & "\" & lstAOIList.SelectedItem & "\" & _
            BA_EnumDescription(GeodatabaseNames.Aoi) & "\" & BA_AOIExtentCoverage
        Dim pColor As IRgbColor = pDisplayColor
        response = BA_AddExtentLayer(My.ArcMap.Document, strPathandName, pColor, "AOI " & lstAOIList.SelectedItem, 0, 2)
        pColor = Nothing
        'Me.Show()
    End Sub

    Private Sub CmdViewLayers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdViewLayers.Click
        Me.Close()
        Dim frmViewDEMLayers As frmViewDEMLayers = New frmViewDEMLayers(lstAOIList.SelectedItem.ToString, BasinFolderBase & "\" & lstAOIList.SelectedItem.ToString)
        frmViewDEMLayers.ShowDialog()
    End Sub

    Private Sub CmdSelectAOI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSelectAOI.Click

        Dim AOIName As String = lstAOIList.SelectedItem.ToString

        'If Len(AOIName) >= 80 Then
        '    MsgBox("The name of the AOI is too long (exceeds 80 characters). Please rename the AOI folder.")
        '    Exit Sub
        'End If
        BA_SetAOI(AOIName)
        Me.Close()
    End Sub

    Private Sub CmdDeleteAOI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdDeleteAOI.Click
        Dim response As Integer
        Dim AOIFilePath As String
        Dim stringpos As Object
        Dim naoi As Integer

        'get the aoi folder path
        AOIFilePath = BasinFolderBase & "\" & lstAOIList.SelectedItem

        'check if the map document file is in the AOI folder, if so, the AOI cannot be removed
        'save the mxd to the AOI folder using the name of the aoi
        response = MsgBox("Delete " & lstAOIList.SelectedItem & "?", vbYesNo)

        naoi = lstAOIList.Items.Count

        If response = vbYes Then 'delete the AOI and its subfolders
            ' Create/configure the ProgressDialog. This automatically displays the dialog
            Dim pProgD As IProgressDialog2 = BA_GetAnimationProgressor(My.ArcMap.Application.hWnd, "Deleting AOI folder...", "AOI Tool: Delete")

            'check if the aoi to be deleted contains the selected aoi
            If Len(AOIFolderBase) > 0 Then 'i.e., there is a selected aoi
                stringpos = InStr(1, AOIFolderBase, AOIFilePath, vbTextCompare)
                If stringpos > 0 Then 'the selected aoi is contained by the aoi to be deleted
                    'reset aoi
                    BA_ResetAOI()
                End If
            End If

            'remove layers from map that are from the AOI folder
            lstAOIList.Items.Clear()

            Dim returnValue As BA_ReturnCode = BA_ReturnCode.NotSupportedOperation
            returnValue = BA_DeleteGeodatabase(AOIFilePath, My.ArcMap.Document)
            'MsgBox(returnValue.ToString)
            'LayerRemoved = BA_RemoveLayersInFolder(My.ArcMap.Document, AOIFilePath)
            'DeleteStatus = BA_Remove_FolderGP(AOIFilePath)

            pProgD.HideDialog()
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pProgD)

            If BA_Workspace_Exists(AOIFilePath) Then 'cannot delete the folder
                MsgBox("Cannot delete the AOI! It is probably caused by a file lock in the Geodatabase. Please restart ArcGIS and repeat the process.")
            Else
                naoi = naoi - 1 'the AOI was successfully removed
            End If

        Else
            Exit Sub
        End If

        'System.Windows.Forms.Application.DoEvents()

        Try
            'if the aoi is the last one, then don't refresh the list
            If naoi > 0 Then
                Refresh_AOIForm() 'refresh the listbox
            Else 'disable command buttons
                lstAOIList.Items.Clear()
                CmdAOIView.Enabled = False
                CmdViewLayers.Enabled = False
                CmdSelectAOI.Enabled = False
                CmdDeleteAOI.Enabled = False
            End If

        Catch ex As Exception
            MsgBox("Exception: ", ex.Message)

        Finally
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
        Me.Activate() 'regain focus
    End Sub

    Private Sub Refresh_AOIForm()
        Dim nSubFolder As Integer
        Dim SubFlist() As BA_SubFolderList = Nothing

        nSubFolder = BA_Dir(BasinFolderBase, SubFlist)
        lstAOIList.Items.Clear()

        For i = 0 To nSubFolder - 1
            Dim fName As String = SubFlist(i).Name
            Dim fPath As String = BasinFolderBase & "\" & fName
            Dim ftype As FolderType = BA_GetFGDBFolderType(fPath)
            If ftype = FolderType.AOI Then
                lstAOIList.Items.Add(BA_GetBareName(fPath))
            End If
        Next

        If lstAOIList.Items.Count = 0 Then 'no AOI in the basin folder
            CmdDeleteAOI.Enabled = False
            CmdSelectAOI.Enabled = False
            CmdAOIView.Enabled = False
            CmdViewLayers.Enabled = False
        Else
            lstAOIList.SelectedIndex = 0
            CmdDeleteAOI.Enabled = True
            CmdSelectAOI.Enabled = True
            CmdAOIView.Enabled = True
            CmdViewLayers.Enabled = True
        End If
    End Sub
End Class