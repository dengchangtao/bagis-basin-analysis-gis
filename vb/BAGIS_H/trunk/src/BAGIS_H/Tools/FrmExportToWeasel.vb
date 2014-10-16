Imports System.IO
Imports System.Windows.Forms
Imports System.Text
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.DataSourcesGDB
Imports ESRI.ArcGIS.DataSourcesFile
Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework

Public Class FrmExportToWeasel

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

    End Sub

    Private Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub BtnSelectFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSelectFolder.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim DataPath As String
        Dim pFilter As IGxObjectFilter = New GxFilterContainers
        Dim hruExt As HruExtension = HruExtension.GetExtension

        Try
            'initialize and open mini browser
            With pGxDialog
                .AllowMultiSelect = False
                .ButtonCaption = "Select"
                .Title = "Select Folder"
                .ObjectFilter = pFilter
                bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
            End With

            If bObjectSelected = False Then Exit Sub

            'get the name of the selected folder
            Dim pGxDataFolder As IGxFile
            pGxDataFolder = pGxObject.Next
            DataPath = pGxDataFolder.Path
            If String.IsNullOrEmpty(DataPath) Then Exit Sub 'user cancelled the action

            'check AOI/BASIN status
            Dim folderType As FolderType = BA_GetFGDBFolderType(DataPath)
            If folderType = folderType.FOLDER Then
                MessageBox.Show("The selected folder does not contain a valid BASIN or AOI!")
                Exit Sub
            Else
                Dim ReturnPath As String = "PleaseReturn"
                'Choose to use supplied DataPath here rather than ReturnString generated from source.weasel
                Dim aoiName As String = BA_GetBareName(DataPath, ReturnPath)
                Dim aoiFolder As String = DataPath
                If BA_ContainsSpace(aoiFolder) Or BA_ContainsSpace(aoiName) Then
                    MessageBox.Show("The folder and name cannot contain spaces.")
                    Exit Sub
                End If

                Dim errorMsg As String = BA_CheckAoiPathLength(DataPath)
                If Not String.IsNullOrEmpty(errorMsg) Then
                    MessageBox.Show(errorMsg, "Path length is too long.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If

                'Check to see if files already exist
                If BA_CheckForWeaselFiles(DataPath) = True Then
                    Dim sb As StringBuilder = New StringBuilder
                    sb.Append("Data in Weasel format exists in folder: " & BA_GetBareName(DataPath) & "." & vbCrLf)
                    sb.Append("Do you wish to overwrite the existing data ?" & vbCrLf)
                    sb.Append("This action cannot be undone." & vbCrLf)
                    Dim result As DialogResult = MessageBox.Show(sb.ToString, "Weasel data exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    'If user wants to overwrite, add to list
                    If result = DialogResult.No Then
                        Exit Sub
                    End If
                End If
            End If

            'Get list of required raster files for the AOI
            Dim reqRasterList As List(Of String) = BA_GetListOfReqFgdbRasters(DataPath)
            Dim missingLayerList As List(Of String) = New List(Of String)
            'Loop through required rasters; If one is missing, add it to the list so we can warn the user
            For Each rPath As String In reqRasterList
                Dim exists As Boolean = BA_File_Exists(rPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset)
                If exists = False Then
                    missingLayerList.Add(rPath)
                End If
            Next
            'Get list of required vector files for the AOI
            Dim reqVectorList As List(Of String) = BA_GetListOfReqFgdbVectors(DataPath)
            'Loop through required vectors; If one is missing, add it to the list so we can warn the user
            For Each vPath As String In reqVectorList
                Dim exists As Boolean = BA_File_Exists(vPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass)
                If exists = False Then
                    missingLayerList.Add(vPath)
                End If
            Next

            If missingLayerList.Count > 0 Then
                Dim missingSb As StringBuilder = New StringBuilder
                missingSb.Append("The selected folder cannot be exported to Weasel format." & vbCrLf)
                missingSb.Append("The data layers listed below are missing. These files" & vbCrLf)
                missingSb.Append("must be present before attempting the conversion." & vbCrLf)
                'If any rasters are missing, add to error message and cease processing
                For Each mLayer As String In missingLayerList
                    missingSb.Append(mLayer & vbCrLf)
                Next
                MessageBox.Show(missingSb.ToString, "Missing data layers", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            'Get list of optional raster files for the AOI
            Dim optRasterList As List(Of String) = BA_GetListOfOptFgdbRasters(DataPath)
            Dim missingOptLayerList As List(Of String) = New List(Of String)
            'Loop through optional rasters; If one is missing, add it to the list so we can warn the user
            For Each rPath As String In optRasterList
                Dim exists As Boolean = BA_File_Exists(rPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset)
                If exists = False Then
                    missingOptLayerList.Add(rPath)
                End If
            Next
            'Get list of optional vector files for the AOI
            Dim optVectorList As List(Of String) = BA_GetListOfOptFgdbVectors(DataPath)
            'Loop through required vectors; If one is missing, add it to the list so we can warn the user
            For Each vPath As String In optVectorList
                Dim exists As Boolean = BA_File_Exists(vPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass)
                If exists = False Then
                    missingOptLayerList.Add(vPath)
                End If
            Next
            If missingOptLayerList.Count > 0 Then
                Dim sb As StringBuilder = New StringBuilder
                sb.Append("The following files normally present in File Geodatabase" & vbCrLf)
                sb.Append("AOI are missing and will not be converted: " & vbCrLf & vbCrLf)
                For Each oLayer As String In missingOptLayerList
                    sb.Append(oLayer & vbCrLf)
                Next
                MessageBox.Show(sb.ToString, "Missing optional data layers", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

            Dim prefix As String = BA_GetFolderTypeForList(folderType)
            Dim listItem As LayerListItem = New LayerListItem(prefix & BA_GetBareName(DataPath), DataPath, LayerType.Raster, True)
            Dim itemExists As Boolean = False
            For Each pItem In LstSelectAoi.Items
                Dim nextItem As LayerListItem = CType(pItem, LayerListItem)
                If nextItem.Value = listItem.Value Then
                    itemExists = True
                    Exit For
                End If
            Next
            If itemExists = False Then
                LstSelectAoi.Items.Add(listItem)
                BtnApply.Enabled = True
                BtnRemove.Enabled = True
            End If

        Catch ex As Exception
            MessageBox.Show("BtnSelectFolder_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub BtnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAbout.Click

        Dim toolHelpForm As FrmHelp = New FrmHelp(BA_HelpTopics.ExportToWeasel)
        toolHelpForm.ShowDialog()
    End Sub

    Private Sub BtnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnRemove.Click
        While LstSelectAoi.SelectedItems.Count > 0
            LstSelectAoi.Items.Remove(LstSelectAoi.SelectedItem)
        End While
        If LstSelectAoi.Items.Count < 1 Then
            BtnApply.Enabled = False
            BtnRemove.Enabled = False
        End If
    End Sub

    Private Sub BtnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        ' Create/configure a step progressor
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 3 * LstSelectAoi.Items.Count)
        pStepProg.Show()
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Converting geodatabase layers to Weasel", "Converting...")
        progressDialog2.ShowDialog()

        Try
            For Each pItem In LstSelectAoi.Items
                Dim nextItem As LayerListItem = CType(pItem, LayerListItem)
                Dim aoiPath As String = nextItem.Value

                'Delete Weasel AOI files at root
                BA_DeleteWeaselFiles(aoiPath, My.Document)
                pStepProg.Step()

                'Create weasel file structure
                Dim folderType As FolderType = BA_GetFolderTypeFromString(nextItem.Name)
                BA_Create_Output_Folders(aoiPath, My.Document, folderType)

                'Copy Weasel AOI files from aoi.gdb to aoi root
                Dim rasterNamesTable As Hashtable = New Hashtable
                Dim vectorNamesTable As Hashtable = New Hashtable
                Dim aoiVectorList(0) As String
                Dim aoiRasterList(0) As String

                Dim inGdbPath As String = aoiPath & "\" & BA_EnumDescription(GeodatabaseNames.Aoi)
                BA_ListLayersinGDB(inGdbPath, aoiRasterList, aoiVectorList)
                BA_UpdateHashtableForRasterToWeasel(rasterNamesTable, inGdbPath, aoiPath, aoiRasterList)
                BA_UpdateHashtableForVectorToWeasel(vectorNamesTable, inGdbPath, aoiPath, aoiVectorList)

                'Now the surfaces
                UpdateHashtableForSurfaces(aoiPath, rasterNamesTable)

                If folderType = folderType.AOI Then
                    'PRISM raster layers
                    UpdateHashtableForPrism(aoiPath, rasterNamesTable)

                    'Now we copy the user layers
                    Dim layerVectorList() As String = Nothing
                    Dim layerRasterList() As String = Nothing
                    inGdbPath = aoiPath & "\" & BA_EnumDescription(GeodatabaseNames.Layers)
                    Dim layerPath As String = BA_GetPath(aoiPath, PublicPath.Layers)
                    BA_ListLayersinGDB(inGdbPath, layerRasterList, layerVectorList)
                    BA_UpdateHashtableForRasterToWeasel(rasterNamesTable, inGdbPath, layerPath, layerRasterList)
                    BA_UpdateHashtableForVectorToWeasel(vectorNamesTable, inGdbPath, layerPath, layerVectorList)

                    'Now we copy the analysis layers
                    Dim analysisVectorList(0) As String
                    Dim analysisRasterList(0) As String
                    inGdbPath = aoiPath & "\" & BA_EnumDescription(GeodatabaseNames.Analysis)
                    layerPath = BA_GetPath(aoiPath, PublicPath.Analysis)
                    BA_ListLayersinGDB(inGdbPath, analysisRasterList, analysisVectorList)
                    BA_UpdateHashtableForRasterToWeasel(rasterNamesTable, inGdbPath, layerPath, analysisRasterList)
                    BA_UpdateHashtableForVectorToWeasel(vectorNamesTable, inGdbPath, layerPath, analysisVectorList)

                    'And finally the zones
                    UpdateHashtableForZones(aoiPath, rasterNamesTable, vectorNamesTable)
                    pStepProg.Step()
                End If

                Dim success As BA_ReturnCode = BA_CopyRastersToGDB(rasterNamesTable, pStepProg)
                pStepProg.Step()
                If success = BA_ReturnCode.Success Then
                    success = BA_CopyVectorsToGDB(vectorNamesTable, pStepProg)
                    pStepProg.Step()
                    If success = BA_ReturnCode.Success Then
                        'Create ascii text files required by Weasel and BAGIS
                        success = BA_GenerateBagisAsciiFiles(aoiPath, folderType)
                    End If
                End If
            Next
        Catch ex As Exception
            MsgBox("BtnApply_Click Exception: " & ex.Message)
        Finally
            ' Clean up step progressor
            pStepProg.Hide()
            progressDialog2.HideDialog()
        End Try
        Me.Close()
    End Sub

    'Add Prism layers to the HashTable
    Private Sub UpdateHashtableForPrism(ByVal aoiPath As String, ByRef pTable As Hashtable)
        Dim prismVectorList(0) As String
        Dim prismRasterList(0) As String
        Dim inGdbPath As String = aoiPath & "\" & BA_EnumDescription(GeodatabaseNames.Prism)
        BA_ListLayersinGDB(inGdbPath, prismRasterList, prismVectorList)

        Dim layerCount As Short = prismRasterList.Length
        Dim strPath As String = BA_GetPath(aoiPath, PublicPath.PRISM)
        Dim i = 1
        Do Until i > layerCount - 1
            Dim nextFolder As String = prismRasterList(i)
            Dim inRasterPath As String = inGdbPath & "\" & nextFolder
            Dim outRasterPath = strPath & "\" & nextFolder & "\" & BA_EnumDescription(MapsFileName.Prism)
            pTable.Add(inRasterPath, outRasterPath)
            i += 1
        Loop
    End Sub

    'Add surfaces to the HashTable
    Private Sub UpdateHashtableForSurfaces(ByVal aoiPath As String, ByRef pTable As Hashtable)
        Dim layerPath As String = BA_GetPath(aoiPath, PublicPath.DEM)
        Dim gdbPath As String = aoiPath & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces)
        Dim inRasterPath As String = Nothing
        Dim outRasterPath As String = Nothing
        Dim fileName As String = Nothing
        ' Filled DEM
        If BA_File_Exists(gdbPath & "\" & BA_EnumDescription(MapsFileName.filled_dem_gdb), WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            inRasterPath = gdbPath & "\" & BA_EnumDescription(MapsFileName.filled_dem_gdb)
            outRasterPath = layerPath & "\" & BA_EnumDescription(MapsFileName.filled_dem)
            pTable.Add(inRasterPath, outRasterPath)
        End If
        ' Original DEM
        layerPath = BA_GetPath(aoiPath, PublicPath.SourceDEM)
        If BA_File_Exists(gdbPath & "\" & BA_GetBareName(layerPath), WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            inRasterPath = gdbPath & "\" & BA_GetBareName(layerPath)
            outRasterPath = layerPath & "\" & BA_EnumDescription(MapsFileName.dem)
            pTable.Add(inRasterPath, outRasterPath)
        End If
        ' Layers in \output\surfaces\dem\filled folder (includes flow accumulation)
        Dim values As PublicPath() = [Enum].GetValues(GetType(PublicPath))
        For i = PublicPath.FlowDirection To PublicPath.Aspect
            Dim nextPath As PublicPath = values(i)
            layerPath = BA_GetPath(aoiPath, nextPath)
            Dim tempLayerName As String = BA_GetBareName(BA_EnumDescription(nextPath))
            tempLayerName = tempLayerName.Replace("-", "_")
            If BA_File_Exists(gdbPath & "\" & tempLayerName, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                inRasterPath = gdbPath & "\" & tempLayerName
                outRasterPath = layerPath & "\" & BA_EnumDescription(MapsFileName.slope)
                pTable.Add(inRasterPath, outRasterPath)
            End If
        Next
    End Sub

    'Add zone layers to the HashTable
    Private Sub UpdateHashtableForZones(ByVal aoiPath As String, ByRef pRasterTable As Hashtable, ByRef pVectorTable As Hashtable)
        Dim inputZonesPath As String = BA_GetPath(aoiPath, PublicPath.HruDirectory)
        Dim outputZonesPath As String = BA_GetPath(aoiPath, PublicPath.HruWeaselDirectory)
        Dim dirHruDirectory As New DirectoryInfo(inputZonesPath)
        Dim dirZones() As DirectoryInfo = Nothing
        Dim vectorName As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruVector), False, True)
        If dirHruDirectory.Exists Then
            dirZones = dirHruDirectory.GetDirectories
            For Each pDir In dirZones
                'Assemble layer paths
                Dim inRasterPath As String = inputZonesPath & "\" & pDir.Name & "\" & pDir.Name & ".gdb" & BA_EnumDescription(PublicPath.HruGrid)
                Dim outRasterPath As String = outputZonesPath & "\" & pDir.Name & BA_EnumDescription(PublicPath.HruGrid)
                Dim inVectorPath As String = inputZonesPath & "\" & pDir.Name & "\" & pDir.Name & ".gdb" & vectorName
                Dim outVectorPath As String = outputZonesPath & "\" & pDir.Name & BA_EnumDescription(PublicPath.HruVector)
                'Check for existence of layers before creating folders
                If BA_File_Exists(inRasterPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Or
                    BA_File_Exists(inVectorPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                    BA_CreateFolder(outputZonesPath, pDir.Name)
                End If
                'Check for existence of layers before adding them to the hashtable
                If BA_File_Exists(inRasterPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                    pRasterTable.Add(inRasterPath, outRasterPath)
                End If
                If BA_File_Exists(inVectorPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                    pVectorTable.Add(inVectorPath, outVectorPath)
                End If
            Next
        End If
    End Sub

End Class