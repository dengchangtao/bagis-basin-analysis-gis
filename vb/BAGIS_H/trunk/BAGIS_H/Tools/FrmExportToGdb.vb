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

Public Class FrmExportToGdb

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
            Dim folderType As FolderType = BA_GetWeaselFolderType(DataPath)
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
                    MessageBox.Show(errorMsg, "Path length is too long", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If

                'NOTE: Any changes here should also be made in 
                'BasinModule.BA_CheckAoiStatus
                'Check to make sure we have the source files we need before proceeding
                'Get list of required raster files for the AOI
                Dim reqRasterList As List(Of String) = BA_GetListOfReqWeaselRasters(DataPath)
                Dim missingLayerList As List(Of String) = New List(Of String)
                'Loop through required rasters; If one is missing, add it to the list so we can warn the user
                For Each rPath As String In reqRasterList
                    Dim exists As Boolean = BA_File_Exists(rPath, WorkspaceType.Raster, esriDatasetType.esriDTRasterDataset)
                    If exists = False Then
                        missingLayerList.Add(rPath)
                    End If
                Next
                'Accomodate two possible names for raster aoi boundary layer (aoibagis or aoi)
                Dim layerPath As String = DataPath & BA_EnumDescription(PublicPath.AoiGrid)
                'First check for aoibagis
                If Not BA_File_Exists(layerPath, WorkspaceType.Raster, esriDatasetType.esriDTRasterDataset) Then
                    'If missing, check for aoi
                    Dim layerPath2 As String = DataPath & BA_EnumDescription(PublicPath.AoiGridWeasel)
                    'If missing, add aoibagis path to the error message
                    If Not BA_File_Exists(layerPath2, WorkspaceType.Raster, esriDatasetType.esriDTRasterDataset) Then
                        missingLayerList.Add(layerPath)
                    End If
                End If
                'Get list of required raster files for the AOI
                Dim reqVectorList As List(Of String) = BA_GetListOfReqWeaselVectors(DataPath)
                Dim missingVectorList As List(Of String) = New List(Of String)
                'Loop through required vectors; If one is missing, add it to the list so we can warn the user
                For Each vPath As String In reqVectorList
                    Dim exists As Boolean = BA_Shapefile_Exists(vPath)
                    If exists = False Then
                        missingLayerList.Add(vPath)
                    End If
                Next
                If missingLayerList.Count > 0 Then
                    Dim missingSb As StringBuilder = New StringBuilder
                    missingSb.Append("The selected folder cannot be exported to File Geodatabase format." & vbCrLf)
                    missingSb.Append("The data layers listed below are missing. These files" & vbCrLf)
                    missingSb.Append("must be present before attempting the conversion.")
                    'If any rasters are missing, add to error message and cease processing
                    For Each mLayer As String In missingLayerList
                        missingSb.Append(mLayer & vbCrLf)
                    Next
                    MessageBox.Show(missingSb.ToString, "Missing data layers", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If

                'Get list of optional raster files for the AOI
                Dim optRasterList As List(Of String) = BA_GetListOfOptWeaselRasters(DataPath)
                Dim missingOptLayerList As List(Of String) = New List(Of String)
                'Loop through optional rasters; If one is missing, add it to the list so we can warn the user
                For Each rPath As String In optRasterList
                    Dim exists As Boolean = BA_File_Exists(rPath, WorkspaceType.Raster, esriDatasetType.esriDTRasterDataset)
                    If exists = False Then
                        missingOptLayerList.Add(rPath)
                    End If
                Next
                'Get list of optional vector files for the AOI
                Dim optVectorList As List(Of String) = BA_GetListOfOptWeaselVectors(DataPath)
                'Loop through required vectors; If one is missing, add it to the list so we can warn the user
                For Each vPath As String In optVectorList
                    Dim exists As Boolean = BA_Shapefile_Exists(vPath)
                    If exists = False Then
                        missingOptLayerList.Add(vPath)
                    End If
                Next
                If missingOptLayerList.Count > 0 Then
                    Dim optSb As StringBuilder = New StringBuilder
                    optSb.Append("The following files normally present in a Weasel AOI are" & vbCrLf)
                    optSb.Append("missing and will not be converted: " & vbCrLf)
                    For Each oLayer As String In missingOptLayerList
                        optSb.Append(oLayer & vbCrLf)
                    Next
                    MessageBox.Show(optSb.ToString, "Missing files", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If

                'Check to see if all 6 BAGIS geodatabases exist
                Dim existsList As List(Of String) = BA_CheckForBagisGDB(DataPath)
                'If any items exists, ask user if they want to overwrite
                If existsList.Count > 0 Then
                    'Assemble user message
                    Dim sb As New StringBuilder
                    sb.Append("The following folders (geodatabases) already exist:" & vbCrLf & vbCrLf)
                    For Each gdbPath As String In existsList
                        sb.Append(gdbPath & vbCrLf)
                    Next
                    sb.Append(vbCrLf & "Do you wish to overwrite them? All existing data will be lost." & vbCrLf)
                    Dim result As DialogResult = MessageBox.Show(sb.ToString, "Folder exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If result = DialogResult.No Then
                        Exit Sub
                    End If
                End If
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
        Dim toolHelpForm As FrmHelp = New FrmHelp(BA_HelpTopics.ExportToGdb)
        toolHelpForm.ShowDialog()
    End Sub

    Private Sub BtnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        ' Create/configure a step progressor
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 50 * LstSelectAoi.Items.Count)
        pStepProg.Show()
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "", "Converting weasel layers...")
        progressDialog2.ShowDialog()

        Try
            For Each pItem In LstSelectAoi.Items
                Dim nextItem As LayerListItem = CType(pItem, LayerListItem)
                Dim aoiPath As String = nextItem.Value
                Dim folderType As FolderType = BA_GetFolderTypeFromString(nextItem.Name)
                'Check to see if all 5 BAGIS geodatabases exist
                Dim existsList As List(Of String) = BA_CheckForBagisGDB(aoiPath)
                For Each gdbPath As String In existsList
                    Dim gdbSuccess As BA_ReturnCode = BA_DeleteGeodatabase(gdbPath, My.ArcMap.Document)
                    If gdbSuccess <> BA_ReturnCode.Success Then
                        MessageBox.Show("Unable to delete folder '" & gdbPath & "'. Please restart ArcMap and try again", "Unable to delete folder", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Exit Sub
                    End If
                Next

                Dim aoiName = BA_GetBareName(aoiPath)
                pStepProg.Message = "Creating file geodatabase folders for " & aoiName
                pStepProg.Step()
                Dim success As BA_ReturnCode = BA_CreateGeodatabaseFolders(aoiPath, folderType)
                If success = BA_ReturnCode.Success Then
                    pStepProg.Message = "Gathering information on files to copy for " & aoiName
                    pStepProg.Step()
                    Dim bTime As DateTime = DateTime.Now
                    Dim rasterNamesTable As Hashtable = New Hashtable
                    Dim vectorNamesTable As Hashtable = New Hashtable

                    'Add surfaces to the rasterNamesTable
                    BA_UpdateHashtableForSurfaces(aoiPath, rasterNamesTable)

                    'Here we copy over all the aoi layers at the top level
                    Dim AOIVectorList() As String = Nothing
                    Dim AOIRasterList() As String = Nothing
                    BA_ListLayersinAOI(aoiPath, AOIRasterList, AOIVectorList)
                    BA_UpdateHashtableForRasterCopy(rasterNamesTable, aoiPath, aoiPath & "\" & BA_EnumDescription(GeodatabaseNames.Aoi), AOIRasterList)
                    BA_UpdateHashtableForVectorCopy(vectorNamesTable, aoiPath, aoiPath & "\" & BA_EnumDescription(GeodatabaseNames.Aoi), AOIVectorList)

                    If folderType = folderType.AOI Then
                        'Moving on to the Prism layers
                        BA_UpdateHashtableForPrism(aoiPath, rasterNamesTable)

                        'Now we copy the user layers
                        Dim layerVectorList() As String = Nothing
                        Dim layerRasterList() As String = Nothing
                        Dim layerPath As String = BA_GetPath(aoiPath, PublicPath.Layers)
                        BA_ListLayersinAOI(layerPath, layerRasterList, layerVectorList)
                        BA_UpdateHashtableForRasterCopy(rasterNamesTable, layerPath, aoiPath & "\" & BA_EnumDescription(GeodatabaseNames.Layers), layerRasterList)
                        BA_UpdateHashtableForVectorCopy(vectorNamesTable, layerPath, aoiPath & "\" & BA_EnumDescription(GeodatabaseNames.Layers), layerVectorList)

                        'Next the analysis folder
                        Dim analysisVectorList(0) As String
                        Dim analysisRasterList(0) As String
                        layerPath = BA_GetPath(aoiPath, PublicPath.Analysis)
                        BA_ListLayersinAOI(layerPath, analysisRasterList, analysisVectorList)
                        BA_UpdateHashtableForRasterCopy(rasterNamesTable, layerPath, aoiPath & "\" & BA_EnumDescription(GeodatabaseNames.Analysis), analysisRasterList)
                        BA_UpdateHashtableForVectorCopy(vectorNamesTable, layerPath, aoiPath & "\" & BA_EnumDescription(GeodatabaseNames.Analysis), analysisVectorList)
                    End If

                    pStepProg.Message = "Initializing geoprocessor"
                    pStepProg.Step()
                    If success = BA_ReturnCode.Success Then
                        success = BA_CopyRastersToGDB(rasterNamesTable, pStepProg)
                        If success = BA_ReturnCode.Success Then
                            success = BA_CopyVectorsToGDB(vectorNamesTable, pStepProg)
                            pStepProg.Step()
                            If success = BA_ReturnCode.Success Then
                                success = BA_RenameAoiBoundary(aoiPath)
                            End If
                        End If
                        pStepProg.Step()
                    End If

                    Dim ts As TimeSpan = DateTime.Now.Subtract(bTime)
                    Debug.Print("ts1: " & ts.Milliseconds)
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

    Private Sub BtnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnRemove.Click
        While LstSelectAoi.SelectedItems.Count > 0
            LstSelectAoi.Items.Remove(LstSelectAoi.SelectedItem)
        End While
        If LstSelectAoi.Items.Count < 1 Then
            BtnApply.Enabled = False
            BtnRemove.Enabled = False
        End If
    End Sub

End Class