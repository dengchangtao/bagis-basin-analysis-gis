Option Strict Off
Option Explicit On

Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Carto
Imports System.Windows.Forms
Imports System.IO
Imports System.Text
Imports ESRI.ArcGIS.SpatialAnalyst
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.ArcMapUI

Public Module BasinModule

    ' Returns the IEnvelope of a basin. Uses the buffered AOI shapefile as the extent
    Public Function BA_GetBasinEnvelope(ByVal basinfolder As String) As IEnvelope
        'get the envelope of basin
        Dim basinGdbPath As String = BA_GeodatabasePath(basinfolder, GeodatabaseNames.Aoi)
        Dim File_Name As String = BA_EnumDescription(AOIClipFile.AOIExtentCoverage)
        Dim pFeatClass As IFeatureClass
        Dim pFLayer As IFeatureLayer = New FeatureLayer

        'if buffered AOI exists, then use the buffered shapefile
        If BA_File_Exists(basinGdbPath & "\" & BA_EnumDescription(AOIClipFile.BufferedAOIExtentCoverage), WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            File_Name = BA_EnumDescription(AOIClipFile.BufferedAOIExtentCoverage)
        End If
        Try
            pFeatClass = BA_OpenFeatureClassFromGDB(basinGdbPath, File_Name)
            'add featureclass to current data frame
            pFLayer.FeatureClass = pFeatClass
            Return pFLayer.AreaOfInterest
        Catch ex As Exception
            MessageBox.Show("BA_GetBasinEnvelope Exception: " + ex.Message)
            Return Nothing
        Finally
            pFeatClass = Nothing
            pFLayer = Nothing
        End Try
    End Function

    Public Function BA_GenerateBagisAsciiFiles(ByVal aoiPath As String, ByVal fType As FolderType) As BA_ReturnCode

        Try
            Dim deminfo As BA_DEMInfo = New BA_DEMInfo
            Dim success As BA_ReturnCode = BA_UpdateDEMInfo(aoiPath, deminfo)
            If success = BA_ReturnCode.Success AndAlso fType = FolderType.AOI Then
                '\output\source.weasel
                Dim demPath As String = BA_GetPath(aoiPath, PublicPath.SourceDEM) & "\" & GRID
                success = BA_Create_FolderType_File(aoiPath, BA_AOI_TYPE, demPath)
                If success = BA_ReturnCode.Success Then
                    '\output\window.weasel
                    demPath = " " & BA_GetPath(aoiPath, PublicPath.SourceDEM) & "\" & GRID
                    success = BA_Create_FolderType_File(aoiPath, BA_AOIWINDOW_TYPE, demPath)
                    If success = BA_ReturnCode.Success Then
                        'output\surfaces\dem\filled\flow-direction\source.weasel
                        demPath = BA_EnumDescription(PublicPath.FlowDirection) & "\" & GRID
                        'trim off leading \
                        demPath = demPath.Substring(1)
                        success = BA_Create_FolderType_File(aoiPath, BA_AOI_FLOW_DIR, demPath)
                        If success = BA_ReturnCode.Success Then
                            'output\surfaces\dem\filled\flow-direction\flow-accumulation\grid
                            demPath = BA_GetPath(aoiPath, PublicPath.FlowAccumulation) & "\" & GRID
                            BA_Create_FolderType_File(aoiPath, BA_AOI_FLOW_ACC, demPath)
                        End If
                    End If
                End If
            End If
            Return success
        Catch ex As Exception
            MsgBox("BA_GenerateBagisAsciiFiles Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        End Try
    End Function

    'create an id file in a folder to identify the type of folder
    'if the folder has a dem, then BA_Basin_Type file exists (or will be created)
    'if the folder is an AOI, then BA_AOI_Type file exists (or will be created)
    'if the folder is an AOIWindow, then BA_AOIWindow_Type file exists (or will be created)
    'text_string will be written to the file
    Public Function BA_Create_FolderType_File(ByVal path_string As String, ByVal type_key As String, _
                                              ByVal text_string As String) As BA_ReturnCode
        Dim FileName As String
        Dim myStream As StreamWriter = Nothing

        Try
            If Len(path_string) = 0 Or Len(type_key) = 0 Then Return BA_ReturnCode.ReadError
            BA_StandardizePathString(path_string, True)
            FileName = path_string & "\" & type_key

            'open file for output
            myStream = New StreamWriter(FileName)
            If myStream IsNot Nothing Then
                myStream.WriteLine(text_string)
            End If
            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_Create_FolderType_File Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            myStream.Close()
        End Try
    End Function

    'write and update the information in the deminfo.txt file associated with a basin or an AOI
    Public Function BA_UpdateDEMInfo(ByVal FolderBase As String, ByRef deminfo As BA_DEMInfo) As BA_ReturnCode
        Dim myStream As StreamWriter = Nothing
        Try
            If Len(FolderBase) = 0 Then Return BA_ReturnCode.UnknownError
            Dim fileName As String = FolderBase & "\" & BA_BASIN_TYPE
            BA_GetDEMStats(FolderBase, deminfo)

            'open file for output
            myStream = New StreamWriter(fileName)
            If myStream IsNot Nothing Then
                myStream.WriteLine(Format(deminfo.Cellsize, "######0.##") & " meters DEM")
                myStream.WriteLine(deminfo.Cellsize)
                myStream.WriteLine(deminfo.Min)
                myStream.WriteLine(deminfo.Max)
                myStream.WriteLine(deminfo.Range)
            End If

            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_UpdateDEMInfo Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            myStream.Close()
        End Try

    End Function

    'get rasterresolution
    Public Sub BA_GetDEMStats(ByVal FolderBase As String, ByRef deminfo As BA_DEMInfo)
        If Len(FolderBase) = 0 Then 'not a valid input
            deminfo.Exist = False
            Exit Sub
        End If

        'get the location and name of the dem. Using output\surfaces\dem\filled\grid
        Dim DemFilePath As String = BA_GetPath(FolderBase, PublicPath.DEM)
        Dim DemFileName As String = DemFilePath & "\" & GRID

        'check if the raster exists
        Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(DemFileName)
        If Not BA_File_Exists(DemFileName, workspaceType, esriDatasetType.esriDTRasterDataset) Then
            deminfo.Exist = False
            Exit Sub
        End If

        Dim pGeoDataset As IGeoDataset = Nothing
        Dim pRasterDataset As IRasterDataset = Nothing
        Dim pAOIRaster As IGeoDataset = Nothing
        Dim pTempRaster As IGeoDataset = Nothing
        Dim pExtractOp As IExtractionOp2 = New RasterExtractionOp
        Dim pRasterBandCollection As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pRasterP As IRasterProps = Nothing
        Dim pRStats As IRasterStatistics = Nothing
        Dim pPnt As IPnt = New DblPnt

        Try
            'Open Raster
            If workspaceType = workspaceType.Geodatabase Then
                pGeoDataset = BA_OpenRasterFromGDB(DemFilePath, GRID)
            Else
                pGeoDataset = BA_OpenRasterFromFile(DemFilePath, GRID)
            End If
            pRasterDataset = CType(pGeoDataset, IRasterDataset) ' Explicit cast

            'see if the DEM was created with a buffer
            'BASIN dem wasn't bufferred
            'AOI dem could be bufferred. If bufferred, there will be an aoib_v.shp in the AOI folder
            Dim bufferFileName As String = BA_StandardizeShapefileName(BA_EnumDescription(AOIClipFile.BufferedAOIExtentCoverage), True, True)
            If Len(Dir(FolderBase & bufferFileName, vbNormal)) <> 0 Then 'DEM is bufferred
                Dim aoiExtentRasterName As String = BA_GetBareName(BA_EnumDescription(PublicPath.AoiGrid))
                If workspaceType = workspaceType.Geodatabase Then
                    pAOIRaster = BA_OpenRasterFromGDB(FolderBase, aoiExtentRasterName)
                Else
                    pAOIRaster = BA_OpenRasterFromFile(FolderBase, aoiExtentRasterName)
                End If
                If pAOIRaster Is Nothing Then
                    deminfo.Exist = False
                    Exit Sub
                End If
                pTempRaster = pExtractOp.Raster(pRasterDataset, pAOIRaster)
                pRasterBandCollection = CType(pTempRaster, IRasterBandCollection) 'Explicit cast
            Else
                pRasterBandCollection = CType(pRasterDataset, IRasterBandCollection) 'Explicit cast
            End If

            pRasterBand = pRasterBandCollection.Item(0)
            pRStats = pRasterBand.Statistics
            pRasterP = pRasterBand
            pPnt = pRasterP.MeanCellSize
            With deminfo
                .Min = pRStats.Minimum
                .Max = pRStats.Maximum
                .Range = .Max - .Min
                .Cellsize = (pPnt.X + pPnt.Y) / 2
                .Exist = True
            End With

        Catch ex As Exception
            Debug.Print("BA_GetDEMStats Exception: " & ex.Message)
            deminfo.Exist = False
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGeoDataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterDataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pAOIRaster)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTempRaster)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pExtractOp)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBandCollection)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBand)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterP)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRStats)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pPnt)

        End Try
    End Sub

    ' Check the length of the AOI path and return an error message if appropriate
    Public Function BA_CheckAoiPathLength(ByVal aoiFilePath As String) As String
        Dim errorMsg As String = Nothing
        If Len(aoiFilePath) >= MAX_AOI_PATH_LENGTH Then
            'errorMsg = "The AOI path name is too long (" & _
            'Len(aoiFilePath) - (MAX_AOI_PATH_LENGTH - 1) & _
            '" character(s) more than the allowed length)!" & vbCrLf & _
            '"The AOI cannot be processed in Weasel GIS." & vbCrLf & _
            '"ArcInfo doesn't allow the path string to exceed 128 char and a workspace" & vbCrLf & _
            '"path to exceed 115 char."
            errorMsg = "The AOI path name is too long." & vbCrLf & _
                "The number of characters, " & Len(aoiFilePath) & "," & vbCrLf & _
                "exceeds the number permitted by ArcInfo."
        End If
        Return errorMsg
    End Function

    'Determines if a folder contains a BASIN or an AOI when provided a path string
    Public Function BA_GetWeaselFolderType(ByVal pathString As String) As FolderType
        Dim retVal As FolderType = FolderType.FOLDER
        Dim aoiPath As String = pathString & "\" & BA_AOI_TYPE
        If BA_File_ExistsWindowsIO(aoiPath) Then
            retVal = FolderType.AOI
        Else
            Dim demPath As String = pathString & "\" & BA_BASIN_TYPE
            If BA_File_ExistsWindowsIO(demPath) Then
                retVal = FolderType.BASIN
            End If
        End If
        Return retVal
    End Function

    'Determines if a folder contains a BASIN or an AOI when provided a path string
    Public Function BA_GetFGDBFolderType(ByVal pathString As String) As FolderType
        Dim retVal As FolderType = FolderType.FOLDER
        Dim gdbPath As String = pathString & "\" & BA_EnumDescription(GeodatabaseNames.Aoi)
        If BA_Folder_ExistsWindowsIO(gdbPath) Then
            Dim aoiVectorName As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.AoiVector), False, True)
            'Check for \aoi raster
            If BA_File_Exists(gdbPath & BA_EnumDescription(PublicPath.AoiGrid), WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                retVal = FolderType.AOI
            ElseIf BA_File_Exists(gdbPath & aoiVectorName, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                retVal = FolderType.BASIN
            End If
        End If
        Return retVal
    End Function

    'Add surfaces to the HashTable
    Public Sub BA_UpdateHashtableForSurfaces(ByVal aoiPath As String, ByRef pTable As Hashtable)
        Dim layerPath As String = BA_GetPath(aoiPath, PublicPath.DEM)
        Dim inRasterPath As String = Nothing
        Dim outRasterPath As String = Nothing
        Dim fileName As String = Nothing
        ' Filled DEM
        If BA_File_ExistsRaster(layerPath, BA_EnumDescription(MapsFileName.filled_dem)) Then
            inRasterPath = layerPath & "\" & BA_EnumDescription(MapsFileName.filled_dem)
            outRasterPath = aoiPath & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\" & BA_EnumDescription(MapsFileName.filled_dem_gdb)
            pTable.Add(inRasterPath, outRasterPath)
        End If
        ' Original DEM
        layerPath = BA_GetPath(aoiPath, PublicPath.SourceDEM)
        If BA_File_ExistsRaster(layerPath, BA_EnumDescription(MapsFileName.dem)) Then
            inRasterPath = layerPath & "\" & BA_EnumDescription(MapsFileName.dem)
            outRasterPath = aoiPath & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\" & BA_GetBareName(layerPath)
            pTable.Add(inRasterPath, outRasterPath)
        End If
        ' Layers in \output\surfaces\dem\filled folder
        Dim dirFilled As New DirectoryInfo(BA_GetPath(aoiPath, PublicPath.DEM))
        Dim dirSurfaces As DirectoryInfo() = Nothing
        If dirFilled.Exists Then
            dirSurfaces = dirFilled.GetDirectories
            For Each pDir In dirSurfaces
                If BA_File_ExistsRaster(pDir.FullName, BA_EnumDescription(MapsFileName.slope)) Then
                    inRasterPath = pDir.FullName & "\" & BA_EnumDescription(MapsFileName.slope)
                    outRasterPath = aoiPath & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\" & pDir.Name.Replace("-", "_")
                    pTable.Add(inRasterPath, outRasterPath)
                End If
            Next
        End If
        ' Flow accumulation layer
        layerPath = BA_GetPath(aoiPath, PublicPath.FlowAccumulation)
        If BA_File_ExistsRaster(layerPath, BA_EnumDescription(MapsFileName.flow_accumulation)) Then
            inRasterPath = layerPath & "\" & BA_EnumDescription(MapsFileName.flow_accumulation)
            outRasterPath = aoiPath & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\" & BA_GetBareName(layerPath).Replace("-", "_")
            pTable.Add(inRasterPath, outRasterPath)
        End If

    End Sub

    'Add Prism layers to the HashTable
    Public Sub BA_UpdateHashtableForPrism(ByVal aoiPath As String, ByRef pTable As Hashtable)
        Dim layerCount As Short = [Enum].GetValues(GetType(AOIPrismFolderNames)).Length
        Dim strPath As String = BA_GetPath(aoiPath, PublicPath.PRISM)
        Dim i = 1
        Do Until i > layerCount
            Dim nextFolder As String = BA_GetPrismFolderName(i)
            Dim prismPath = strPath & "\" & nextFolder & "\" & BA_EnumDescription(MapsFileName.Prism)
            If BA_File_ExistsRaster(strPath & "\" & nextFolder, BA_EnumDescription(MapsFileName.Prism)) Then
                Dim outRasterPath As String = aoiPath & "\" & BA_EnumDescription(GeodatabaseNames.Prism) & "\" & nextFolder
                pTable.Add(prismPath, outRasterPath)
            End If
            i += 1
        Loop
    End Sub

    'Checks to see if the desired AOI path contains the required FGDB files for BAGIS-H
    'If not, it offers to convert them
    'Next, check the path for spaces and length
    Public Function BA_CheckAoiStatus(ByVal aoiPath As String, ByVal pHWnd As Integer, _
                                      ByVal pDocument As IMxDocument) As BA_ReturnCode
        Dim folderType As FolderType = BA_GetFGDBFolderType(aoiPath)
        If folderType <> folderType.AOI Then
            Dim weaselFolderType As FolderType = BA_GetWeaselFolderType(aoiPath)
            If weaselFolderType <> folderType.AOI Then
                MessageBox.Show("The selected folder does not contain a valid AOI!")
                Return BA_ReturnCode.OtherError
            Else
                'NOTE: Any changes here should also be made in 
                'the BAGIS-H form: FrmExportToGdb.BtnSelectFolder_Click()
                'Get list of required raster files for the AOI
                Dim reqRasterList As List(Of String) = BA_GetListOfReqWeaselRasters(aoiPath)
                Dim missingLayerList As List(Of String) = New List(Of String)
                'Loop through required rasters; If one is missing, add it to the list so we can warn the user
                For Each rPath As String In reqRasterList
                    Dim exists As Boolean = BA_File_Exists(rPath, WorkspaceType.Raster, esriDatasetType.esriDTRasterDataset)
                    If exists = False Then
                        missingLayerList.Add(rPath)
                    End If
                Next
                'Accomodate two possible names for raster aoi boundary layer (aoibagis or aoi)
                Dim layerPath As String = aoiPath & BA_EnumDescription(PublicPath.AoiGrid)
                'First check for aoibagis
                If Not BA_File_Exists(layerPath, WorkspaceType.Raster, esriDatasetType.esriDTRasterDataset) Then
                    'If missing, check for aoi
                    Dim layerPath2 As String = aoiPath & BA_EnumDescription(PublicPath.AoiGridWeasel)
                    'If missing, add aoibagis path to the error message
                    If Not BA_File_Exists(layerPath2, WorkspaceType.Raster, esriDatasetType.esriDTRasterDataset) Then
                        missingLayerList.Add(layerPath)
                    End If
                End If
                'Get list of required vector files for the AOI
                Dim reqVectorList As List(Of String) = BA_GetListOfReqWeaselVectors(aoiPath)
                'Loop through required vectors; If one is missing, add it to the list so we can warn the user
                For Each vPath As String In reqVectorList
                    Dim exists As Boolean = BA_Shapefile_Exists(vPath)
                    If exists = False Then
                        missingLayerList.Add(vPath)
                    End If
                Next
                If missingLayerList.Count > 0 Then
                    Dim missingSb As StringBuilder = New StringBuilder
                    missingSb.Append("The selected folder cannot be used to source BAGIS." & vbCrLf)
                    missingSb.Append("The data layers listed below are missing. These files" & vbCrLf)
                    missingSb.Append("must be present before attempting the conversion to" & vbCrLf)
                    missingSb.Append("File Geodatabase format." & vbCrLf & vbCrLf)
                    'If any rasters are missing, add to error message and cease processing
                    For Each mLayer As String In missingLayerList
                        missingSb.Append(mLayer & vbCrLf)
                    Next
                    MessageBox.Show(missingSb.ToString, "Missing data layers", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return BA_ReturnCode.OtherError
                End If
                'Get list of optional raster files for the AOI
                Dim optRasterList As List(Of String) = BA_GetListOfOptWeaselRasters(aoiPath)
                Dim missingOptLayerList As List(Of String) = New List(Of String)
                'Loop through optional rasters; If one is missing, add it to the list so we can warn the user
                For Each rPath As String In optRasterList
                    Dim exists As Boolean = BA_File_Exists(rPath, WorkspaceType.Raster, esriDatasetType.esriDTRasterDataset)
                    If exists = False Then
                        missingOptLayerList.Add(rPath)
                    End If
                Next
                'Get list of optional vector files for the AOI
                Dim optVectorList As List(Of String) = BA_GetListOfOptWeaselVectors(aoiPath)
                'Loop through required vectors; If one is missing, add it to the list so we can warn the user
                For Each vPath As String In optVectorList
                    Dim exists As Boolean = BA_Shapefile_Exists(vPath)
                    If exists = False Then
                        missingOptLayerList.Add(vPath)
                    End If
                Next
                Dim sb As New StringBuilder()
                sb.Append("The selected folder contains a Weasel AOI which cannot " & vbCrLf)
                sb.Append("be used to source BAGIS. Converting this AOI to File" & vbCrLf)
                sb.Append("Geodatabase format may take several minutes." & vbCrLf)
                If missingOptLayerList.Count > 0 Then
                    sb.Append(vbCrLf)
                    sb.Append("The following files normally present in a Weasel AOI are" & vbCrLf)
                    sb.Append("missing and will not be converted: " & vbCrLf)
                    For Each oLayer As String In missingOptLayerList
                        sb.Append(oLayer & vbCrLf)
                    Next
                End If
                sb.Append(vbCrLf)
                sb.Append("Do you wish to continue ?")
                Dim result As DialogResult = MessageBox.Show(sb.ToString, "Weasel AOI", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If result = DialogResult.Yes Then
                    Dim success As BA_ReturnCode = BA_ExportToFileGdb(pHWnd, aoiPath, pDocument)
                    If success <> BA_ReturnCode.Success Then
                        MessageBox.Show("Unable to convert files to File Geodatabase format", "Conversion error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return BA_ReturnCode.OtherError
                    End If
                Else
                    Return BA_ReturnCode.OtherError
                End If
            End If
        End If
        If BA_ContainsSpace(aoiPath) Then
            MessageBox.Show("The aoi folder and name cannot contain spaces.")
            Return BA_ReturnCode.OtherError
        End If
        Dim errorMsg As String = BA_CheckAoiPathLength(aoiPath)
        If Not String.IsNullOrEmpty(errorMsg) Then
            MessageBox.Show(errorMsg, "Invalid AOI path", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return BA_ReturnCode.OtherError
        End If
        Return BA_ReturnCode.Success
    End Function

    Public Function BA_ExportToFileGdb(ByVal pHWnd As Integer, ByVal aoiPath As String, _
                                       ByVal pDocument As IMxDocument) As BA_ReturnCode
        ' Create/configure a step progressor
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(pHWnd, 50)
        pStepProg.Show()
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "", "Converting weasel layers...")
        progressDialog2.ShowDialog()

        Try
            pStepProg.Step()
            'Check to see if all 5 BAGIS geodatabases exist
            Dim existsList As List(Of String) = BA_CheckForBagisGDB(aoiPath)
            For Each gdbPath As String In existsList
                'Delete if they do
                Dim gdbSuccess As BA_ReturnCode = BA_DeleteGeodatabase(gdbPath, pDocument)
                If gdbSuccess <> BA_ReturnCode.Success Then
                    MessageBox.Show("Unable to delete folder '" & gdbPath & "'. Please restart ArcMap and try again", "Unable to delete folder", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return BA_ReturnCode.WriteError
                End If
            Next

            pStepProg.Message = "Creating file geodatabase folders"
            pStepProg.Step()
            Dim success As BA_ReturnCode = BA_CreateGeodatabaseFolders(aoiPath, FolderType.AOI)
            If success = BA_ReturnCode.Success Then
                pStepProg.Message = "Gathering information on files to copy"
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

                pStepProg.Message = "Initializing geoprocessor"
                pStepProg.Step()
                If success = BA_ReturnCode.Success Then
                    success = BA_CopyRastersToGDB(rasterNamesTable, pStepProg)
                    'Debug.Print("Raster count: " & rasterNamesTable.Count)
                    If success = BA_ReturnCode.Success Then
                        success = BA_CopyVectorsToGDB(vectorNamesTable, pStepProg)
                        'Debug.Print("Vector count: " & vectorNamesTable.Count)
                        pStepProg.Step()
                        If success = BA_ReturnCode.Success Then
                            success = BA_RenameAoiBoundary(aoiPath)
                        End If
                    End If
                End If
            End If
            Return success
        Catch ex As Exception
            Debug.Print("BA_ExportToFileGdb Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            ' Clean up step progressor
            pStepProg.Hide()
            progressDialog2.HideDialog()
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStepProg)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(progressDialog2)
        End Try

    End Function

    Public Function BA_GetListOfReqWeaselRasters(ByVal aoiPath As String) As List(Of String)
        Dim rasterList As List(Of String) = New List(Of String)
        ' 5 surfaces layers
        ' 1. Filled DEM
        Dim layerPath As String = BA_GetPath(aoiPath, PublicPath.DEM) & "\" & BA_EnumDescription(MapsFileName.filled_dem)
        rasterList.Add(layerPath)
        ' 2. Aspect
        layerPath = BA_GetPath(aoiPath, PublicPath.Aspect) & "\" & BA_EnumDescription(MapsFileName.aspect)
        rasterList.Add(layerPath)
        ' 3. Slope
        layerPath = BA_GetPath(aoiPath, PublicPath.Slope) & "\" & BA_EnumDescription(MapsFileName.slope)
        rasterList.Add(layerPath)
        ' 4. Flow accumulation
        layerPath = BA_GetPath(aoiPath, PublicPath.FlowAccumulation) & "\" & BA_EnumDescription(MapsFileName.flow_accumulation)
        rasterList.Add(layerPath)
        ' 5. Flow accumulation
        layerPath = BA_GetPath(aoiPath, PublicPath.FlowDirection) & "\" & BA_EnumDescription(MapsFileName.flow_direction)
        rasterList.Add(layerPath)

        'Prism layers
        Dim layerCount As Short = [Enum].GetValues(GetType(AOIPrismFolderNames)).Length
        Dim strPath As String = BA_GetPath(aoiPath, PublicPath.PRISM)
        Dim i = 1
        Do Until i > layerCount
            Dim nextFolder As String = BA_GetPrismFolderName(i)
            layerPath = strPath & "\" & nextFolder & "\" & BA_EnumDescription(MapsFileName.Prism)
            rasterList.Add(layerPath)
            i += 1
        Loop

        'A couple of aoi layers
        layerPath = aoiPath & BA_EnumDescription(PublicPath.AoiBufferedGrid)
        rasterList.Add(layerPath)
        layerPath = aoiPath & BA_EnumDescription(PublicPath.AoiPrismGrid)
        rasterList.Add(layerPath)

        Return rasterList
    End Function

    Public Function BA_GetListOfReqWeaselVectors(ByVal aoiPath As String) As List(Of String)
        Dim vectorList As List(Of String) = New List(Of String)
        'List of required aoi vectors
        Dim layerPath As String = aoiPath & BA_EnumDescription(PublicPath.AoiVector)
        vectorList.Add(layerPath)
        layerPath = aoiPath & BA_StandardizeShapefileName(BA_EnumDescription(AOIClipFile.BufferedAOIExtentCoverage), True, True)
        vectorList.Add(layerPath)
        layerPath = aoiPath & BA_StandardizeShapefileName(BA_EnumDescription(AOIClipFile.PrismClipAOIExtentCoverage), True, True)
        vectorList.Add(layerPath)
        layerPath = aoiPath & BA_StandardizeShapefileName(BA_EnumDescription(MapsFileName.PourPoint), True, True)
        vectorList.Add(layerPath)
        Return vectorList
    End Function

    'We warn if these rasters are missing but don't cease execution
    Public Function BA_GetListOfOptWeaselRasters(ByVal aoiPath As String) As List(Of String)
        Dim rasterList As List(Of String) = New List(Of String)
        ' Original DEM
        Dim layerPath As String = BA_GetPath(aoiPath, PublicPath.SourceDEM) & "\" & BA_EnumDescription(MapsFileName.dem)
        rasterList.Add(layerPath)
        ' Hillshade
        layerPath = BA_GetPath(aoiPath, PublicPath.Hillshade) & "\" & BA_EnumDescription(MapsFileName.hillshade)
        rasterList.Add(layerPath)

        Return rasterList
    End Function

    Public Function BA_GetListOfOptWeaselVectors(ByVal aoiPath As String) As List(Of String)
        Dim vectorList As List(Of String) = New List(Of String)
        'List of required aoi vectors
        Dim layerPath As String = aoiPath & "\" & BA_EnumDescription(MapsFileName.UnsnappedPourPoint)
        vectorList.Add(layerPath)
        Return vectorList
     End Function

    'Rename the weasel aoi boundary to aoibagis if it doesn't already exist
    Public Function BA_RenameAoiBoundary(ByVal aoiPath As String) As BA_ReturnCode
        Dim parentPath As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Aoi)
        Dim fullPath As String = parentPath & BA_EnumDescription(PublicPath.AoiGrid)
        If Not BA_File_Exists(fullPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            Return BA_RenameRasterInGDB(parentPath, BA_GetBareName(BA_EnumDescription(PublicPath.AoiGridWeasel)), BA_GetBareName(BA_EnumDescription(PublicPath.AoiGrid)))
        Else
            Return BA_ReturnCode.Success
        End If
    End Function

    Public Function BA_GetListOfReqFgdbRasters(ByVal aoiPath As String) As List(Of String)
        Dim rasterList As List(Of String) = New List(Of String)
        ' 5 surfaces layers
        Dim gdbPath As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Surfaces, True)
        ' 1. Filled DEM
        Dim layerPath As String = gdbPath & BA_EnumDescription(MapsFileName.filled_dem_gdb)
        rasterList.Add(layerPath)
        ' 2. Aspect
        Dim fileName As String = BA_GetBareName(BA_EnumDescription(PublicPath.Aspect))
        layerPath = gdbPath & fileName
        rasterList.Add(layerPath)
        ' 3. Slope
        fileName = BA_GetBareName(BA_EnumDescription(PublicPath.Slope))
        layerPath = gdbPath & fileName
        rasterList.Add(layerPath)
        ' 4. Flow accumulation
        layerPath = gdbPath & BA_EnumDescription(MapsFileName.flow_accumulation_gdb)
        rasterList.Add(layerPath)
        ' 5. Flow direction
        layerPath = gdbPath & BA_EnumDescription(MapsFileName.flow_direction_gdb)
        rasterList.Add(layerPath)

        'Prism layers
        Dim layerCount As Short = [Enum].GetValues(GetType(AOIPrismFolderNames)).Length
        Dim strPath As String = BA_GetPath(aoiPath, PublicPath.PRISM)
        gdbPath = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Prism, True)
        Dim i = 1
        Do Until i > layerCount
            Dim nextFolder As String = BA_GetPrismFolderName(i)
            layerPath = gdbPath & nextFolder
            rasterList.Add(layerPath)
            i += 1
        Loop

        '3 aoi layers
        gdbPath = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Aoi)
        layerPath = gdbPath & BA_EnumDescription(PublicPath.AoiGrid)
        rasterList.Add(layerPath)
        layerPath = gdbPath & BA_EnumDescription(PublicPath.AoiBufferedGrid)
        rasterList.Add(layerPath)
        layerPath = gdbPath & BA_EnumDescription(PublicPath.AoiPrismGrid)
        rasterList.Add(layerPath)

        Return rasterList
    End Function

    Public Function BA_GetListOfReqFgdbVectors(ByVal aoiPath As String) As List(Of String)
        Dim vectorList As List(Of String) = New List(Of String)
        'List of required aoi vectors
        Dim gdbPath As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Aoi, True)
        Dim layerPath As String = gdbPath & BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.AoiVector), False)
        vectorList.Add(layerPath)
        layerPath = gdbPath & BA_EnumDescription(AOIClipFile.BufferedAOIExtentCoverage)
        vectorList.Add(layerPath)
        layerPath = gdbPath & BA_EnumDescription(AOIClipFile.PrismClipAOIExtentCoverage)
        vectorList.Add(layerPath)
        layerPath = gdbPath & BA_EnumDescription(MapsFileName.PourPoint)
        vectorList.Add(layerPath)
        Return vectorList
    End Function

    'We warn if these rasters are missing but don't cease execution
    Public Function BA_GetListOfOptFgdbRasters(ByVal aoiPath As String) As List(Of String)
        Dim rasterList As List(Of String) = New List(Of String)
        ' Original DEM
        Dim gdbPath As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Surfaces, True)
        Dim fileName As String = BA_GetBareName(BA_EnumDescription(PublicPath.SourceDEM))
        Dim layerPath As String = gdbPath & fileName
        rasterList.Add(layerPath)
        ' Hillshade
        fileName = BA_GetBareName(BA_EnumDescription(PublicPath.Hillshade))
        layerPath = gdbPath & fileName
        rasterList.Add(layerPath)

        Return rasterList
    End Function

    Public Function BA_GetListOfOptFgdbVectors(ByVal aoiPath As String) As List(Of String)
        Dim vectorList As List(Of String) = New List(Of String)
        'List of required aoi vectors
        Dim layerPath As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Aoi, True) & BA_EnumDescription(MapsFileName.UnsnappedPourPoint)
        vectorList.Add(layerPath)
        Return vectorList
    End Function

    'Get a list of SubAOI paths inside a given AOI
    Public Function BA_GetListOfSubAoiPaths(ByVal aoiPath As String) As List(Of String)
        Dim folderList As IList(Of String) = New List(Of String)
        Dim dirInfo As DirectoryInfo = New DirectoryInfo(aoiPath)
        Dim subDirectories As DirectoryInfo() = dirInfo.GetDirectories
        For Each subDirectory As DirectoryInfo In subDirectories
            Dim folderPath As String = subDirectory.FullName
            'Ignore the file geodatabases
            If Right(folderPath, 4).ToUpper <> ".GDB" Then
                'Check to see if remaining folders are subAOI's
                Dim folderType As FolderType = BA_GetFGDBFolderType(folderPath)
                If folderType = BAGIS_ClassLibrary.FolderType.AOI Then
                    folderList.Add(folderPath)
                End If
            End If
        Next
        Return folderList
    End Function

End Module
