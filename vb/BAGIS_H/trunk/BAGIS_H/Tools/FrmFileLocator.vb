Imports System.Windows.Forms
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Display
Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.ArcMapUI
Imports System.IO


Public Class frmFileLocator
    Private Sub frmFileLocator_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim datapath(5) As String
        Dim hruExt As HruExtension = HruExtension.GetExtension
        Dim temppath As String = hruExt.SettingsPath

        BA_GetSettingsPath()
        datapath(0) = BA_GetSettingsPath() & BA_EnumDescription(PublicPath.BagisSettingFilename)
        datapath(1) = temppath & BA_EnumDescription(PublicPath.Templates)
        datapath(2) = "TBD"
        'Masoud: Here is the code to generate the path to the BAGIS-P profile folder.
        'You can do something similar for the Method folder. See the new PathsModule I added
        'to ClassLibrary -- Lesley
        Dim BagisPProfilePath As String = BA_GetPublicProfilesPath(hruExt.SettingsPath)
        datapath(3) = "TBD"
        datapath(4) = "TBD"

        'display File paths
        txtDefFile.Text = datapath(0)
        txtHRUTemplate.Text = datapath(1)
        txtPubParRulTemp.Text = datapath(2)
        txtProfFolder.Text = datapath(3)
        txtMethodsFolder.Text = datapath(4)

    End Sub


    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub btnSetAOI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetAOI.Click
        Dim bObjectSelected As Boolean = True
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim Data_Path As String
        'Dim response As Integer = 0

        Dim pFilter As IGxObjectFilter = New GxFilterContainers

        'set the PRISM folder variables: PRISMLayer()
        'BA_SetPRISMFolderNames()

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select AOI Folder"
            .ObjectFilter = pFilter
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then Exit Sub

        'get the name of the selected folder
        Dim pGxDataFolder As IGxFile
        pGxDataFolder = pGxObject.Next
        Data_Path = pGxDataFolder.Path
        If Len(Trim(Data_Path)) = 0 Then Exit Sub 'user cancelled the action

        Dim return_string As String
        'check AOI status
        'return_string = Check_Folder_Type(Data_Path, BA_AOI_TYPE)
        return_string = BA_Check_Folder_Type(Data_Path, BA_AOI_TYPE)

        If Len(return_string) <= 0 Then
            MsgBox("The selected folder does not contain a valid AOI!")
            Exit Sub
        Else 'it's an AOI
            Me.Text = "" 'reset Basin assignment
        End If


        'check if selected folder is a weasely type folder or not
        Dim fType As FolderType = BA_GetWeaselFolderType(Data_Path)
        If fType <> FolderType.AOI Then
            txtPathTextBox.Text = "The selected folder does not contain weasel files!"
        Else
            Show_AOI_filepath(Data_Path)
        End If

        ' ''If BA_CheckForWeaselFiles(Data_Path) Then
        ' ''    Show_AOI_filepath(Data_Path)
        ' ''Else
        ' ''    txtPathTextBox.Text = "The SELECTED files do not exist"
        ' ''End If

        'Check if GDB files exist
        Dim GDBfType As FolderType = BA_GetFGDBFolderType(Data_Path)
        If GDBfType <> FolderType.AOI Then
            txtGDBFilePath.Text = "the selected folder does not contain GDB data layers"
        Else
            Show_AOI_GDB_FilePath(Data_Path)
        End If
        'Check to see if all 5 BAGIS geodatabases exist
        ' ''BA_CheckForBagisGDB(Data_Path)

        ' ''If Not BA_File_Exists(Data_Path, WorkspaceType.Raster, esriDatasetType.esriDTFeatureClass) Then
        ' ''    Show_AOI_GDB_FilePath(Data_Path)
        ' ''Else
        ' ''    txtGDBFilePath.Text = "The files do not exist"
        ' ''End If

    End Sub
    Public Const missfile As String = " (Missing File)"
    Public Const missfolder As String = " (Missing Folder)"
    Private Sub Show_AOI_filepath(ByVal AOIPath As String)
        txtAOIPath.Text = AOIPath

        'check if each file exists or not
        Dim pathstring(12) As String
        'Aspect
        pathstring(0) = AOIPath & BA_EnumDescription(PublicPath.Aspect) & "\grid"
        If Not BA_File_ExistsRaster(AOIPath, (BA_EnumDescription(PublicPath.Aspect) & "\grid")) Then
            pathstring(0) = pathstring(0) & missfile
        End If
        'Flowdirection
        pathstring(1) = AOIPath & BA_EnumDescription(PublicPath.FlowDirection) & "\grid"
        If Not BA_File_ExistsRaster(AOIPath, (BA_EnumDescription(PublicPath.FlowDirection) & "\grid")) Then
            pathstring(1) = pathstring(1) & missfile
        End If
        'Flow accumulation
        pathstring(2) = AOIPath & BA_EnumDescription(PublicPath.FlowAccumulation) & "\grid"
        If Not BA_File_ExistsRaster(AOIPath, (BA_EnumDescription(PublicPath.FlowAccumulation) & "\grid")) Then
            pathstring(2) = pathstring(2) & missfile
        End If
        'DEM
        pathstring(3) = AOIPath & BA_EnumDescription(PublicPath.DEM) & "\grid"
        If Not BA_File_ExistsRaster(AOIPath, (BA_EnumDescription(PublicPath.DEM) & "\grid")) Then
            pathstring(3) = pathstring(3) & missfile
        End If
        'Hillshade
        pathstring(4) = AOIPath & BA_EnumDescription(PublicPath.Hillshade) & "\grid"
        If Not BA_File_ExistsRaster(AOIPath, (BA_EnumDescription(PublicPath.Hillshade) & "\grid")) Then
            pathstring(4) = pathstring(4) & missfile
        End If
        'Slope
        pathstring(5) = AOIPath & BA_EnumDescription(PublicPath.Slope) & "\grid"
        If Not BA_File_ExistsRaster(AOIPath, (BA_EnumDescription(PublicPath.Slope) & "\grid")) Then
            pathstring(5) = pathstring(5) & missfile
        End If
        'Source DEM
        pathstring(6) = AOIPath & BA_EnumDescription(PublicPath.SourceDEM) & "\grid"
        If Not BA_File_ExistsRaster(AOIPath, (BA_EnumDescription(PublicPath.SourceDEM) & "\grid")) Then
            pathstring(6) = pathstring(6) & missfile
        End If
        'AOI Grid
        pathstring(7) = AOIPath & BA_EnumDescription(PublicPath.AoiGrid)
        If Not BA_File_ExistsRaster(AOIPath, BA_EnumDescription(PublicPath.AoiGrid)) Then
            pathstring(7) = pathstring(7) & missfile
        End If
        'Analysis
        pathstring(8) = AOIPath & BA_EnumDescription(PublicPath.Analysis)
        If Not BA_Workspace_Exists(pathstring(8)) Then
            pathstring(8) = pathstring(8) & missfolder
        End If
        'Layers
        pathstring(9) = AOIPath & BA_EnumDescription(PublicPath.Layers)
        If Not BA_Workspace_Exists(pathstring(9)) Then
            pathstring(9) = pathstring(9) & missfolder
        End If
        'PRISM
        pathstring(10) = AOIPath & BA_EnumDescription(PublicPath.PRISM)
        If Not BA_Workspace_Exists(pathstring(10)) Then
            pathstring(10) = pathstring(10) & missfolder
        End If
        'HRU Directory
        pathstring(11) = AOIPath & BA_EnumDescription(PublicPath.HruDirectory)
        If Not BA_Workspace_Exists(pathstring(11)) Then
            pathstring(11) = pathstring(11) & missfolder
        End If

        Dim txtString As String = "Analysis Folder: " & pathstring(8)             '1
        txtString = txtString & vbCrLf & "AOI: " & pathstring(7)                  '2
        txtString = txtString & vbCrLf & "Aspect: " & pathstring(0)               '3
        txtString = txtString & vbCrLf & "DEM (Original): " & pathstring(6)         '4
        txtString = txtString & vbCrLf & "Filled_DEM: " & pathstring(3)           '5
        txtString = txtString & vbCrLf & "Flow_Accumulation: " & pathstring(2)    '6
        txtString = txtString & vbCrLf & "Flow_direction: " & pathstring(1)       '7
        txtString = txtString & vbCrLf & "Hillshade: " & pathstring(4)            '8
        txtString = txtString & vbCrLf & "HRU Folder: " & pathstring(11)          '9
        txtString = txtString & vbCrLf & "Layers Folder: " & pathstring(9)        '10
        txtString = txtString & vbCrLf & "PRISM Folder: " & pathstring(10)        '11
        txtString = txtString & vbCrLf & "Slope: " & pathstring(5)                '12
        txtPathTextBox.Text = txtString

    End Sub
    Private Sub Show_AOI_GDB_FilePath(ByVal AOIPath As String)
        Dim gdbpathstring(12) As String
        'Aspect
        gdbpathstring(0) = AOIPath & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\aspect"

        If Not BA_File_Exists(gdbpathstring(0), WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            gdbpathstring(0) = gdbpathstring(0) & missfile
        End If
        'Flowdirection
        gdbpathstring(1) = AOIPath & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\flow_direction"
        If Not BA_File_Exists(gdbpathstring(1), WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            gdbpathstring(1) = gdbpathstring(1) & missfile
        End If
        'Flow accumulation
        gdbpathstring(2) = AOIPath & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\flow_accumulation"
        If Not BA_File_Exists(gdbpathstring(2), WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            gdbpathstring(2) = gdbpathstring(2) & missfile
        End If
        'Filled DEM
        gdbpathstring(3) = AOIPath & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\dem"
        If Not BA_File_Exists(gdbpathstring(3), WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            gdbpathstring(3) = gdbpathstring(3) & missfile
        End If
        'Hillshade
        gdbpathstring(4) = AOIPath & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\hillshade"
        If Not BA_File_Exists(gdbpathstring(4), WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            gdbpathstring(4) = gdbpathstring(4) & missfile
        End If
        'Slope
        gdbpathstring(5) = AOIPath & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\slope"
        If Not BA_File_Exists(gdbpathstring(5), WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            gdbpathstring(5) = gdbpathstring(5) & missfile
        End If
        'Source DEM
        gdbpathstring(6) = AOIPath & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\dem"
        If Not BA_File_Exists(gdbpathstring(6), WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            gdbpathstring(6) = gdbpathstring(6) & missfile
        End If
        'AOI Grid
        gdbpathstring(7) = AOIPath & "\" & BA_EnumDescription(GeodatabaseNames.Aoi) & "\aoi"
        If Not BA_File_Exists(gdbpathstring(7), WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            gdbpathstring(7) = gdbpathstring(7) & missfile
        End If
        'Analysis Geodatabase
        gdbpathstring(8) = AOIPath & "\" & BA_EnumDescription(GeodatabaseNames.Analysis)
        If Not BA_Workspace_Exists(gdbpathstring(8)) Then
            gdbpathstring(8) = gdbpathstring(8) & missfolder
        End If
        'Layers Geodatabase
        gdbpathstring(9) = AOIPath & "\" & BA_EnumDescription(GeodatabaseNames.Layers)
        If Not BA_Workspace_Exists(gdbpathstring(9)) Then
            gdbpathstring(9) = gdbpathstring(9) & missfolder
        End If
        'PRISM Geodatabase
        gdbpathstring(10) = AOIPath & "\" & BA_EnumDescription(GeodatabaseNames.Prism)
        If Not BA_Workspace_Exists(gdbpathstring(10)) Then
            gdbpathstring(10) = gdbpathstring(10) & missfolder
        End If
        'HRU Geodatabase (Zones)
        gdbpathstring(11) = AOIPath & "\" & BA_EnumDescription(GeodatabaseNames.Layers)
        If Not BA_Workspace_Exists(gdbpathstring(11)) Then
            gdbpathstring(11) = gdbpathstring(11) & missfolder
        End If
        Dim txtGDBpath As String = "Analysis Geodatabase: " & gdbpathstring(8)              '1
        txtGDBpath = txtGDBpath & vbCrLf & "AOI: " & gdbpathstring(7)                       '2
        txtGDBpath = txtGDBpath & vbCrLf & "Aspect: " & gdbpathstring(0)                     '3
        txtGDBpath = txtGDBpath & vbCrLf & "DEM (Original): " & gdbpathstring(6)            '4
        txtGDBpath = txtGDBpath & vbCrLf & "DEM_Filled: " & gdbpathstring(3)                '5
        txtGDBpath = txtGDBpath & vbCrLf & "Flow_Accumulation: " & gdbpathstring(2)         '6
        txtGDBpath = txtGDBpath & vbCrLf & "Flow_direction: " & gdbpathstring(1)            '7
        txtGDBpath = txtGDBpath & vbCrLf & "Hillshade: " & gdbpathstring(4)                 '8
        txtGDBpath = txtGDBpath & vbCrLf & "HRU Geodatabase: " & gdbpathstring(11)          '9
        txtGDBpath = txtGDBpath & vbCrLf & "Layers Geodatabase: " & gdbpathstring(9)        '10
        txtGDBpath = txtGDBpath & vbCrLf & "PRISM Geodatabase: " & gdbpathstring(10)        '11
        txtGDBpath = txtGDBpath & vbCrLf & "Slope: " & gdbpathstring(5)                     '12
        txtGDBFilePath.Text = txtGDBpath
    End Sub

    Private Sub txtDefFile_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDefFile.TextChanged

    End Sub
End Class