Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports BAGIS_ClassLibrary
Imports System.Windows.Forms
Imports System.IO
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesFile
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Desktop.AddIns
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Display
Imports System.Text
Imports ESRI.ArcGIS.ArcMapUI
Imports System.ComponentModel

Public Class frmBasin_Tool
    Private ParentFolderFGDBBasinStatus As String = "-"
    Private ParentFolderFGDBAOIStatus As String = "-"
    Private ParentFolderWeaselBasinStatus As String = "-"
    Private ParentFolderWeaselAOIStatus As String = "-"

    Private Sub frmBasin_Tool_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim response As Integer = 0
        Dim nSubFolder As Long
        Dim SubFlist() As BA_SubFolderList = Nothing
        txtBasinFolder.Text = BasinFolderBase
        txtBasinFolder.Tag = BasinFolderBase

        Try
            'If lstvBasinDEM.Items.Count= 0 then
            If Len(txtBasinFolder.Text) = 0 Then
                CmdSelectBasin.Enabled = False
                CmdViewDEMExtent.Enabled = False
                lstvBasinDEM.Items.Clear()
                CmdViewRasters.Enabled = False

            ElseIf Len(txtBasinFolder.Text) > 0 Then 'basin is already set, reload the basin folder to achieve consistency
                CmdSelectBasin.Enabled = True
                CmdViewDEMExtent.Enabled = True
                CmdViewRasters.Enabled = True

                BA_SetSettingPath() 'set the BA_Settings_Filepath global variable
                'global variables: pSelectColor and pDisplayColor
                'check DEM and AOI status
                CheckGDBFolderStatus(txtBasinFolder.Text)
                'list subfolders
                nSubFolder = BA_Dir(txtBasinFolder.Text, SubFlist)
                Display_SubFolderList(SubFlist)
            End If

            pSelectColor = New RgbColor
            ''pSelectColor determines the color for AOI and Pourpoint graphics
            pSelectColor.RGB = RGB(0, 255, 0)
            ''pDisplayColor determines the color for AOI and Pourpoint shapfiles
            pDisplayColor = New RgbColor
            pDisplayColor.RGB = RGB(255, 0, 0)
            'type Error
        Catch ex As Exception
            'OtherError:
            MsgBox("Error occurred when selecting a basin folder!" & vbCrLf & ex.Message)

        End Try

    End Sub

    Private Sub CmdSelectBasinFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSelectBasinFolder.Click
        Dim bObjectSelected As Boolean = True
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim basinfoldertext As String = ""

        Dim pFilter As IGxObjectFilter = New GxFilterContainers

        Dim nSubFolder As Long = 0
        Dim SubFlist() As BA_SubFolderList = Nothing

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select Basin Folder"
            .ObjectFilter = pFilter
            'bObjectSelected = .DoModalOpen(0, pGxObject)
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then 'user canceled the action
            Exit Sub
        End If

        Dim pGxDataFolder As IGxFile
        pGxDataFolder = pGxObject.Next
        basinfoldertext = pGxDataFolder.Path

        If String.IsNullOrEmpty(basinfoldertext) Then
            pGxDataFolder = Nothing
            pGxDataFolder = Nothing
            pGxObject = Nothing
            Exit Sub 'user cancelled the action
        End If

        Try
            'set the name of the selected folder
            txtBasinFolder.Text = basinfoldertext
            txtBasinFolder.Tag = txtBasinFolder.Text

            If Len(txtBasinFolder.Text) > 0 Then
                'list subfolders
                nSubFolder = BA_Dir(txtBasinFolder.Text, SubFlist)
                Display_SubFolderList(SubFlist)

                'check DEM and AOI status
                'check if the selected folder is a weasel AOI folder it should be converted to gdb basin folder
                CheckGDBFolderStatus(txtBasinFolder.Text)
                CmdSelectBasin.Enabled = True
            Else
                CmdSelectBasin.Enabled = False
            End If

            'type Error
        Catch ex As Exception
            'OtherError:
            MsgBox("Error occurred when selecting a basin folder!" & vbCrLf & ex.Message)

        Finally
            pGxDataFolder = Nothing
            pGxDataFolder = Nothing
            pGxObject = Nothing

        End Try
    End Sub

    Private Sub CheckGDBFolderStatus(ByVal folderPath As String)
        Dim demPath As String = folderPath & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\" & BA_EnumDescription(MapsFileName.dem)
        Dim demFilledPath As String = folderPath & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\" & BA_EnumDescription(MapsFileName.filled_dem_gdb)
        Dim aoiPath As String = folderPath & "\" & BA_EnumDescription(GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiVector)

        Dim rasterResolution As Double = 0

        Dim folderType As FolderType = BA_GetFGDBFolderType(folderPath)
        If folderType <> folderType.AOI Then
            'then check if dem_filled exist
            If BA_File_Exists(demFilledPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then 'The folder has filled dem

                BA_GetRasterStatsGDB(demFilledPath, rasterResolution)
                lblDEMStatus.Text = Math.Round(rasterResolution, 2) & " meters resolution"
                CmdViewRasters.Enabled = True

            Else
                lblDEMStatus.Text = "No"
                CmdViewRasters.Enabled = False
            End If
            lblAOIStatus.Text = "No"
        Else
            If BA_GetRasterStatsGDB(demPath, rasterResolution) Is Nothing Then
                lblDEMStatus.Text = "No"
                CmdViewRasters.Enabled = False
                lblAOIStatus.Text = "No"
            Else
                lblDEMStatus.Text = Math.Round(rasterResolution, 2) & " meters resolution"
                CmdViewRasters.Enabled = True
                lblAOIStatus.Text = "Yes"
            End If
        End If

        'allow user to view the DEM extent when the selected folder is a BASIN or an AOI
        If lblDEMStatus.Text = "No" And lblAOIStatus.Text = "No" Then
            CmdViewDEMExtent.Enabled = False
        Else
            CmdViewDEMExtent.Enabled = True
        End If

        'check weasel dem status
        Dim return_string As String
        return_string = BA_Check_Folder_Type(folderPath, BA_Basin_Type)
        If Not String.IsNullOrEmpty(return_string) Then 'the folder has DEM
            lblWeaselDEMStatus.Text = "Yes"
        Else
            lblWeaselDEMStatus.Text = "No"
        End If

        'check weasel AOI status
        return_string = BA_Check_Folder_Type(folderPath, BA_AOI_Type)
        If Not String.IsNullOrEmpty(return_string) Then 'the folder is an AOI
            lblWeaselAOIStatus.Text = "Yes"
        Else
            lblWeaselAOIStatus.Text = "No"
        End If

        ''set global variable
        'With BASIN_Folder_Types
        '    .Name = folderPath
        '    .gdbAOI = lblAOIStatus.Text
        '    .gdbdem = lblDEMStatus.Text
        '    .weaselAOI = lblWeaselAOIStatus.Text
        '    .weaseldem = lblWeaselDEMStatus.Text
        'End With
    End Sub

    ''set the DEM and AOI status of the current folder listed in the textbox
    'Private Sub Set_Folder_Status(ByVal folder_string As String)
    '    'check DEM status
    '    If Len(folder_string) = 0 Then Exit Sub 'no input string was provided

    '    Dim return_string As String
    '    Dim demPath As String = folder_string & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\" & BA_EnumDescription(MapsFileName.dem)
    '    Dim demFilledPath As String = folder_string & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\" & BA_EnumDescription(MapsFileName.filled_dem_gdb)

    '    'at first check if dem exists
    '    'then check if dem_filled exist
    '    If BA_File_Exists(demPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then 'The folder has dem
    '        Dim rasterResolution As Double
    '        BA_GetRasterStatsGDB(demPath, rasterResolution)
    '        lblDEMStatus.Text = Math.Round(rasterResolution, 2) & " meters resolution"
    '        CmdViewRasters.Enabled = True
    '    ElseIf BA_File_Exists(demFilledPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then 'The folder has dem_filled
    '        Dim rasterResolution As Double
    '        BA_GetRasterStatsGDB(demFilledPath, rasterResolution)
    '        lblDEMStatus.Text = Math.Round(rasterResolution, 2) & " meters resolution"
    '        CmdViewRasters.Enabled = True
    '    Else
    '        lblDEMStatus.Text = "No"
    '        CmdViewRasters.Enabled = False
    '    End If

    '    Dim folderType As FolderType = BA_GetFGDBFolderType(folder_string)
    '    If folderType <> folderType.AOI Then
    '        lblAOIStatus.Text = "No"
    '    Else
    '        lblAOIStatus.Text = "Yes"
    '    End If

    '    'check weasel dem status
    '    return_string = Check_Folder_Type(txtBasinFolder.Text, BA_Basin_Type)
    '    If Not String.IsNullOrEmpty(return_string) Then 'the folder has DEM
    '        lblWeaselDEMStatus.Text = "Yes"
    '    Else
    '        lblWeaselDEMStatus.Text = "No"
    '    End If

    '    'check weasel AOI status
    '    return_string = Check_Folder_Type(txtBasinFolder.Text, BA_AOI_Type)
    '    If Not String.IsNullOrEmpty(return_string) Then 'the folder is an AOI
    '        lblWeaselAOIStatus.Text = "Yes"
    '    Else
    '        lblWeaselAOIStatus.Text = "No"
    '    End If

    '    'set global variable
    '    With BASIN_Folder_Types
    '        .Name = folder_string
    '        .gdbAOI = lblAOIStatus.Text
    '        .gdbdem = lblDEMStatus.Text
    '        .weaselAOI = lblWeaselAOIStatus.Text
    '        .weaseldem = lblWeaselDEMStatus.Text
    '    End With
    'End Sub

    'Display the BA_SubFolderList in the listbox
    'sublist() array is 1-based
    'the first entry will always be the parent directory (i.e., "..")
    Private Sub Display_SubFolderList(ByVal sublist() As BA_SubFolderList, Optional ByVal Display_ParentDir As Boolean = True)

        Dim nSubFolder As Long
        Dim i As Long
        'Dim Row_OffSet As Long

        lstvBasinDEM.Items.Clear()

        'display the first entry
        Dim strParent As String = ".. <Parent Folder>"
        Dim strRoot As String = "..<Root Folder - No Parent folder>"
        Dim parentitem As New ListViewItem(strParent)
        Dim rootitem As New ListViewItem(strRoot)

        Try
            'number of subfolders
            If sublist Is Nothing Then
                'If nSubFolder = 0 Then
                If Display_ParentDir Then
                    lstvBasinDEM.Items.AddRange(New ListViewItem() {parentitem})
                    'Exit Sub    'no subfolder
                End If
            Else
                'Dim item As New ListViewItem(sublist(i).Name)
                nSubFolder = sublist.Length
                If Display_ParentDir Then
                    lstvBasinDEM.Items.AddRange(New ListViewItem() {parentitem})
                Else
                    lstvBasinDEM.Items.AddRange(New ListViewItem() {rootitem})
                End If

                For i = 0 To nSubFolder - 1
                    Dim folderitem As New ListViewItem(sublist(i).Name)
                    'lstvBasinDEM.Items.AddRange(New ListViewItem() {folderitem})

                    'gdb DEM/AOI status
                    folderitem.SubItems.Add(sublist(i).gdbdem)
                    Dim gdbaoiString As String
                    If sublist(i).gdbAOI Then
                        gdbaoiString = "Yes"
                    Else
                        gdbaoiString = "No"
                    End If
                    folderitem.SubItems.Add(gdbaoiString)

                    'weasel DEM/AOI status
                    'folderitem.SubItems.Add(sublist(i).weaseldem)
                    Dim weaselDEMstring As String
                    'intentionally not displaying the DEM resolution here
                    If UCase(sublist(i).weaseldem) <> "NO" Then
                        weaselDEMstring = "Yes"
                    Else
                        weaselDEMstring = "No"
                    End If
                    folderitem.SubItems.Add(weaselDEMstring)

                    Dim weaselaoistring As String
                    If sublist(i).weaselAOI Then
                        weaselaoistring = "Yes"
                    Else
                        weaselaoistring = "No"
                    End If
                    folderitem.SubItems.Add(weaselaoistring)
                    lstvBasinDEM.Items.Add(folderitem)
                Next
            End If

        Catch ex As Exception
            MsgBox("Unknown Error: " & ex.Message)

        End Try
    End Sub

    Private Sub CmdSelectBasin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSelectBasin.Click
        Dim response As Integer, confirmresponse As Integer
        Dim NeedtoClipDEM As Boolean
        Dim pBasinEnvelope As IEnvelope
        Dim tempname As String = ""
        Dim cboSelectedBasin = AddIn.FromID(Of cboTargetedBasin)(My.ThisAddIn.IDs.cboTargetedBasin)
        cboSelectedBasin.setValue("")
        NeedtoClipDEM = True

        If BA_Enable_SAExtension(My.ArcMap.Application) <> ESRI.ArcGIS.esriSystem.esriExtensionState.esriESEnabled Then
            Windows.Forms.MessageBox.Show("Spatial Analyst is required for BAGIS and is not available. Program stopped.")
            Exit Sub
        End If

        Dim hasAOIinFolder As Boolean = False
        Dim SubFlist() As BA_SubFolderList = Nothing
        BA_Dir(txtBasinFolder.Text, SubFlist, hasAOIinFolder) 'check if the folder contains any AOI in it.

        'declare the property value of buttons in the main toolbar
        Dim setDEMExtenttool = AddIn.FromID(Of setDEMExtenttool)(My.ThisAddIn.IDs.setDEMExtenttool)
        'Dim clipDEMButton = AddIn.FromID(Of BtnClipDEM)(My.ThisAddIn.IDs.BtnClipDEM)
        Dim selectAOIButton = AddIn.FromID(Of BtnAOI_Tool)(My.ThisAddIn.IDs.BtnAOI_Tool)
        Dim basinInfoButton = AddIn.FromID(Of BtnBasinInfo)(My.ThisAddIn.IDs.BtnBasinInfo)
        Me.Hide()

        Dim weaselDemStatus As String = lblWeaselDEMStatus.Text
        Dim weaselAOIStatus As String = lblWeaselAOIStatus.Text

        Dim result As DialogResult = Windows.Forms.DialogResult.No
        Dim sb As New StringBuilder()

        If UCase(lblDEMStatus.Text) = "NO" Then 'FGDB doesn't exist
            If Not UCase(weaselDemStatus) = "NO" Then 'weasel DEM exists in the folder
                result = DialogResult.No
                If UCase(weaselAOIStatus) = "YES" Then 'the folder is a weasel AOI, users can choose to convert it to gdb aoi, otherwise cannot change it
                    sb.Remove(0, sb.Length) 'reset the stringbuilder
                    sb.Append("The selected folder is an AOI. You cannot alter its DEM data." & vbCrLf)
                    sb.Append("The DEM is in Weasel format. You must convert it to File GDB format first." & vbCrLf)
                    sb.Append("Do you wish to convert the BASIN?" & vbCrLf)
                    sb.Append("WARNING!!! The conversion may take several minutes.")
                    result = MessageBox.Show(sb.ToString, "Basin Tool", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                ElseIf hasAOIinFolder Then
                    sb.Remove(0, sb.Length) 'reset the stringbuilder
                    sb.Append("The selected folder contains AOIs. You cannot alter its DEM data." & vbCrLf)
                    sb.Append("The DEM is in Weasel format. You must convert it to File GDB format first." & vbCrLf)
                    sb.Append("Do you wish to convert the BASIN?" & vbCrLf)
                    sb.Append("WARNING!!! The conversion may take several minutes.")
                    result = MessageBox.Show(sb.ToString, "Basin Tool", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                Else 'weasel DEM exists and can be used for further analysis
                    'the folder is a weasel basin folder. User can select to convert it to a gdb basin folder and then continue the process
                    sb.Remove(0, sb.Length) 'reset the stringbuilder
                    sb.Append("The selected folder is a Weasel basin folder. Do you want to reuse its DEM data?" & vbCrLf)
                    sb.Append("Select YES to convert the Weasel basin to a File GDB basin." & vbCrLf)
                    sb.Append("Select NO to delete the existing DEM." & vbCrLf)
                    sb.Append("Select CANCEL to abort the process." & vbCrLf)
                    sb.Append("WARNING!!! If YES is selected, the conversion may take a few minutes. " & vbCrLf)

                    result = MessageBox.Show(sb.ToString, "Weasel Basin", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)

                    If result = DialogResult.No Then
                        'remove layers in the DEM folder from the active map
                        response = BA_RemoveLayersInFolder(My.ArcMap.Document, txtBasinFolder.Text)
                        'delete the output folder
                        response = BA_Remove_Folder(txtBasinFolder.Text & "\output")
                        'delete the "aoi_v" shapefile
                        response = BA_Remove_Shapefile(txtBasinFolder.Text, BA_DEMExtentShapefile)
                        NeedtoClipDEM = True
                    ElseIf result = DialogResult.Cancel Then 'user selected cancel
                        Exit Sub
                    Else
                        NeedtoClipDEM = False 'user selected to reuse the dem, a weseal to FGDB conversion is needed
                    End If
                End If

                If result = DialogResult.Yes Then 'user choose to convert the weasel DEM to FGDB DEM
                    Dim success As BA_ReturnCode = BA_ExportToFileGdb(My.ArcMap.Application.hWnd, txtBasinFolder.Text, My.ArcMap.Document)
                    If success <> BA_ReturnCode.Success Then
                        sb.Remove(0, sb.Length) 'reset the stringbuilder
                        sb.Append("Unable to convert files to File Geodatabase format." & vbCrLf)
                        sb.Append("Please select a different folder.")
                        MessageBox.Show(sb.ToString, "Conversion error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                    MsgBox("The BASIN is converted to File Geodatabase format!")
                    NeedtoClipDEM = False

                ElseIf result = DialogResult.Cancel Then
                    MessageBox.Show("Please select another valid BASIN folder.", "Weasel AOI", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub

                Else 'user select NO to abort the process
                    If Not NeedtoClipDEM Then 'the NO was to abort the process
                        MessageBox.Show("Please select another valid BASIN folder.", "Weasel AOI", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    Else
                        'do nothing, the NO was to not reuse the DEM
                    End If
                End If

            ElseIf hasAOIinFolder Then 'dem doesn't exist but the folder contains AOIs
                sb.Remove(0, sb.Length) 'reset the stringbuilder
                sb.Append("The selected folder contains AOIs but doesn't have a valid DEM." & vbCrLf)
                sb.Append("You cannot add new DEM to it. Please select a different folder.")
                MessageBox.Show(sb.ToString, "Weasel AOI", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub

            Else 'dem doesn't exist, a new DEM must be created
                NeedtoClipDEM = True
            End If

        Else 'the basin already has a FGDB dem
            If UCase(lblAOIStatus.Text) = "YES" Or hasAOIinFolder Then 'the folder is also an AOI or contains AOIs, users cannot change its DEM
                NeedtoClipDEM = False
                MsgBox("The selected folder is an AOI or contains AOIs. You cannot alter its DEM data.")
            Else
                response = MsgBox("Reuse existing DEM?", MessageBoxButtons.YesNoCancel)
                Select Case response
                    Case vbCancel 'user cancel this operation
                        Exit Sub
                    Case vbYes 'user wants to reuse the dem
                        NeedtoClipDEM = False
                    Case vbNo 'user wants to overwrite the existing dem
                        confirmresponse = MsgBox("Are you sure?" & vbCrLf & "WARNING!!! Existing DEM layers will be deleted if you continue. Click NO to cancel the action.", MessageBoxButtons.YesNo)
                        If confirmresponse = vbNo Then
                            Exit Sub
                        End If

                        'remove layers in the DEM folder from the active map
                        Dim surfacesgdbpath As String = txtBasinFolder.Text & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces)
                        Dim aoigdbPath As String = txtBasinFolder.Text & "\" & BA_EnumDescription(GeodatabaseNames.Aoi)

                        'delete the Surfaces Geodatabase
                        If BA_Workspace_Exists(surfacesgdbpath) Then
                            response = BA_RemoveLayersInFolder(My.ArcMap.Document, surfacesgdbpath)
                            Dim gdbSuccess As BA_ReturnCode = BA_Remove_WorkspaceGP(surfacesgdbpath)
                            'If gdbSuccess <> BA_ReturnCode.Success Then
                            '    MessageBox.Show("Unable to delete Geodatabase '" & surfacesgdbpath & "'. Please restart ArcMap and try again", "Unable to delete GDB", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            '    Exit Sub
                            'End If
                        End If

                        'delete the "aoi" gdb
                        If BA_Workspace_Exists(aoigdbPath) Then
                            response = BA_RemoveLayersInFolder(My.ArcMap.Document, aoigdbPath)
                            Dim gdbSuccess As BA_ReturnCode = BA_Remove_WorkspaceGP(aoigdbPath)
                            'If gdbSuccess <> BA_ReturnCode.Success Then
                            '    MessageBox.Show("Unable to delete Geodatabase '" & surfacesgdbpath & "'. Please restart ArcMap and try again", "Unable to delete GDB", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            '    Exit Sub
                            'End If
                        End If

                        'verify the gdbs were removed
                        If BA_Workspace_Exists(surfacesgdbpath) Or BA_Workspace_Exists(aoigdbPath) Then
                            MsgBox("Unable to clear BASIN's internal file geodatabase. Please restart ArcMap and try again.")
                            Exit Sub
                        End If
                        NeedtoClipDEM = True
                End Select
            End If
        End If

        BasinFolderBase = txtBasinFolder.Text
        BA_SetSettingPath()

        If Not File.Exists(BA_Settings_Filepath & "\" & BA_Settings_Filename) Then
            MessageBox.Show("Settings file does not exist!" & vbCrLf & "Please go to Options to set settings files", "Missing file!")
            Exit Sub
        End If

        response = BA_ReadBAGISSettings(BA_Settings_Filepath) 'set the system setting parameters

        If response <> 1 Then
            MsgBox("Unable to get critical system settings information. System stopped!")
            Exit Sub
        End If

        BA_SetDefaultProjection(My.ArcMap.Application)
        'set mapframe name to default name
        response = BA_SetDefaultMapFrameName(BA_DefaultMapName)
        Dim response1 As Integer = BA_SetMapFrameDimension(BA_DefaultMapName, 1, 2, 7.5, 9, True)

        Try
            If NeedtoClipDEM Then
                If BA_DEMDimension.X_CellSize * BA_DEMDimension.Y_CellSize = 0 Then
                    Dim strDEMDataSet As String
                    If BA_SystemSettings.DEM10MPreferred Then
                        strDEMDataSet = BA_SystemSettings.DEM10M
                    Else
                        strDEMDataSet = BA_SystemSettings.DEM30M
                    End If

                    MsgBox("The Basin DEM extent boundaries will be snapped to the DEM raster cells." & vbCrLf & "The activation might take a few seconds.")
                    BA_GetRasterDimension(strDEMDataSet, BA_DEMDimension)
                    MsgBox("Snapping to raster has been activated!")
                End If
                setDEMExtenttool.selectedProperty = True
                'clipDEMButton.selectedProperty = True
                cboSelectedBasin.setValue(BA_GetBareName(BasinFolderBase))
                MsgBox("Please select and clip the DEM to the basin folder!")
                'need to set pBasinEnvelope when the DEM extent is determined

            Else
                pBasinEnvelope = BA_GetBasinEnvelope(BasinFolderBase)

                If pBasinEnvelope Is Nothing Then 'unable to read basin aoi_v correctly
                    MsgBox("The Basin folder file structure is corrupted! Unable to read its boundary file - aoi_v.")
                    Exit Sub
                End If

                pBasinEnvelope = Nothing
                cboSelectedBasin.setValue(BA_GetBareName(BasinFolderBase))
                'reset aoi information
                BA_Reset_AOIFlags()
                BA_ResetAOI()

                setDEMExtenttool.selectedProperty = False
                'clipDEMButton.selectedProperty = False
                selectAOIButton.selectedProperty = True
                basinInfoButton.selectedProperty = True
            End If
        Catch ex As Exception
            MsgBox("Unknown error")
            Exit Sub
        Finally
            'pStepProg = Nothing
            'progressDialog2.HideDialog()
            'progressDialog2 = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
        Me.Close()
    End Sub

    Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdExit.Click
        Me.Close()
    End Sub

    '' '' '' '' ''    Private Sub CmdCreateSubFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdCreateSubFolder.Click
    '' '' '' '' ''        'check if the process is for creating AOI folders
    '' '' '' '' ''        Dim commandstr As String
    '' '' '' '' ''        Dim comspec_string As String
    '' '' '' '' ''        Dim foldername As String
    '' '' '' '' ''        Dim folder_string As String

    '' '' '' '' ''ReEnter:
    '' '' '' '' ''        foldername = InputBox("Please enter subfolder name:", "Subfolder Name", foldername)

    '' '' '' '' ''        If InStr(foldername, " ") Then
    '' '' '' '' ''            MsgBox("Space not allowed in the folder name!")
    '' '' '' '' ''            GoTo ReEnter
    '' '' '' '' ''        End If

    '' '' '' '' ''        folder_string = txtBasinFolder.Text & "\" & foldername

    '' '' '' '' ''        If Len(Dir(folder_string, vbDirectory)) <> 0 Then 'i.e., folder already exists
    '' '' '' '' ''            MsgBox("The subfolder already exists!")
    '' '' '' '' ''            Exit Sub
    '' '' '' '' ''        Else
    '' '' '' '' ''            'create the folder and its subfolders
    '' '' '' '' ''            commandstr = Environ("COMSPEC") & " /c mkdir " & Chr(34) & folder_string & Chr(34)
    '' '' '' '' ''            'If Not bShellAndWait(commandstr, vbMinimizedFocus) Then Err.Raise(9999)
    '' '' '' '' ''        End If

    '' '' '' '' ''        Dim nSubFolder As Long
    '' '' '' '' ''        Dim SubFlist() As BA_SubFolderList

    '' '' '' '' ''        'refresh the subfolder list
    '' '' '' '' ''        If Len(txtBasinFolder.Text) > 0 Then
    '' '' '' '' ''            'check DEM and AOI status
    '' '' '' '' ''            Set_Folder_Status(txtBasinFolder.Text)

    '' '' '' '' ''            CmdSelectBasin.Enabled = True
    '' '' '' '' ''            'list subfolders
    '' '' '' '' ''            nSubFolder = BA_Dir(txtBasinFolder.Text, SubFlist)
    '' '' '' '' ''            Display_SubFolderList(SubFlist)
    '' '' '' '' ''        Else
    '' '' '' '' ''            CmdSelectBasin.Enabled = False
    '' '' '' '' ''        End If
    '' '' '' '' ''    End Sub

    Private Sub CmdViewDEMExtent_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdViewDEMExtent.Click
        'Dim response As Integer
        'Dim strPathandName As String
        'strPathandName = txtBasinFolder.Text '& "\" & BA_EnumDescription(GeodatabaseNames.Aoi)
        'If BA_File_Exists(strPathandName, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
        'strPathandName = txtBasinFolder.Text & "\" & BA_DEMExtentFolder & "\" & BA_DEMExtentShapefile
        'response = BA_AddExtentLayer(strPathandName, "Basin DEM Extent", 0, 2)

        Dim strPathname As String = Nothing
        strPathname = txtBasinFolder.Text & "\" & BA_EnumDescription(GeodatabaseNames.Aoi)
        Dim actionCode As Short = 0
        'Get the name of aoi_v feature class
        Dim vectorName As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.AoiVector), False)
        ' check if the specified dem extent exists
        Dim RgbColor As IRgbColor = New RgbColor()
        RgbColor.RGB = RGB(255, 0, 0)

        If BA_File_Exists(strPathname & "\" & vectorName, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            Dim success As BA_ReturnCode = BA_AddExtentLayer(My.ArcMap.Document, strPathname & "\" & vectorName, _
                                                            RgbColor, vectorName, actionCode, 1.5)
        Else
            Exit Sub
        End If

    End Sub

    Private Sub CmdViewRasters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdViewRasters.Click
        Try
            Dim BasinFolderPath As String = txtBasinFolder.Text
            Dim aoiLabel As String = lblAOIStatus.Text
            Dim frmViewDEMLayers As frmViewDEMLayers = New frmViewDEMLayers(aoiLabel, BasinFolderPath)
            frmViewDEMLayers.ShowDialog()
        Catch ex As Exception
            MsgBox("Error", ex.Message)
        End Try
    End Sub

    Private Sub lstvBasinDEM_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstvBasinDEM.Click
        If lstvBasinDEM.SelectedItems.Count > 0 Then
            Dim lVItem As ListViewItem = lstvBasinDEM.SelectedItems(0)
            Dim tempname As String = lVItem.Text

            If tempname(0) = "." Then
                txtBasinFolder.Text = txtBasinFolder.Tag
                'CmdSelectBasin.Enabled = False

                lblDEMStatus.Text = ParentFolderFGDBBasinStatus
                lblAOIStatus.Text = ParentFolderFGDBAOIStatus
                lblWeaselDEMStatus.Text = ParentFolderWeaselBasinStatus
                lblWeaselAOIStatus.Text = ParentFolderWeaselAOIStatus
            Else
                txtBasinFolder.Text = txtBasinFolder.Tag & "\" & tempname
                CmdSelectBasin.Enabled = True
                lblDEMStatus.Text = lVItem.SubItems(1).Text
                lblAOIStatus.Text = lVItem.SubItems(2).Text
                lblWeaselDEMStatus.Text = lVItem.SubItems(3).Text
                lblWeaselAOIStatus.Text = lVItem.SubItems(4).Text
            End If

            If UCase(lblDEMStatus.Text) <> "NO" Then
                CmdViewRasters.Enabled = True
            Else
                CmdViewRasters.Enabled = False
            End If

            Dim demextentpath As String = txtBasinFolder.Text & "\" & BA_EnumDescription(GeodatabaseNames.Aoi) & "\aoi_v"
            If BA_File_Exists(demextentpath, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                CmdViewDEMExtent.Enabled = True
            Else
                CmdViewDEMExtent.Enabled = False
            End If
        End If
    End Sub

    Private Sub lstvBasinDEM_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstvBasinDEM.DoubleClick
        Dim newpath As String = "" 'a temp path string that stores the text to be set to the txtBasinFolder.tag
        Dim BareName As String
        Dim nSubFolder As Long
        Dim SubFlist() As BA_SubFolderList = Nothing
        Dim NotTheRootDir As Boolean = True

        If lstvBasinDEM.SelectedItems.Count > 0 Then
            Dim lVItem As ListViewItem = lstvBasinDEM.SelectedItems(0)
            Dim index As Short = lVItem.Index

            If index = 0 Then 'up to the parent folder
                BareName = BA_GetBareName(txtBasinFolder.Text, newpath)

                If Len(newpath) = 0 Then 'reaching the top of a folder structure
                    newpath = txtBasinFolder.Tag
                    NotTheRootDir = False
                Else
                    'remove the last character if it's a backslash
                    If Microsoft.VisualBasic.Right(newpath, 1) = "\" Then newpath = Microsoft.VisualBasic.Left(newpath, Len(newpath) - 1)
                End If

                'reset the parentfolder's status data when navigate upward
                ParentFolderFGDBAOIStatus = "-"
                ParentFolderFGDBBasinStatus = "-"
                ParentFolderWeaselAOIStatus = "-"
                ParentFolderWeaselBasinStatus = "-"

                txtBasinFolder.Text = newpath
                txtBasinFolder.Tag = txtBasinFolder.Text

            Else
                newpath = txtBasinFolder.Tag & "\" & lVItem.Text

                'remember the parentfolder's status data when navigate downward
                ParentFolderFGDBAOIStatus = lblAOIStatus.Text
                ParentFolderFGDBBasinStatus = lblDEMStatus.Text
                ParentFolderWeaselAOIStatus = lblWeaselAOIStatus.Text
                ParentFolderWeaselBasinStatus = lblWeaselDEMStatus.Text

                txtBasinFolder.Text = newpath
                txtBasinFolder.Tag = txtBasinFolder.Text

            End If

            'Checks if the folder has dem and aoi to set the lblDEMstatus and lblAOIstatus labels in the upper path
            CheckGDBFolderStatus(txtBasinFolder.Text)

            'Checks if the folder has dem and aoi to set the lblDEMstatus and lblAOIstatus labels in the upper path
            If Len(txtBasinFolder.Text) > 0 Then
                CmdSelectBasin.Enabled = True
                nSubFolder = BA_Dir(txtBasinFolder.Text, SubFlist)
                Display_SubFolderList(SubFlist, NotTheRootDir)
            Else
                CmdSelectBasin.Enabled = False
            End If
        End If

    End Sub

End Class