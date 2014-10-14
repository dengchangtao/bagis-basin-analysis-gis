Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesGDB
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Carto
Imports System.Windows.Forms
Imports ESRI.ArcGIS.GeoAnalyst
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Geometry
Imports System.Windows.Forms.ListBox
Imports ESRI.ArcGIS.DataManagementTools
Imports ESRI.ArcGIS.ArcMapUI
Imports System.ComponentModel
Imports System.Text
Imports ESRI.ArcGIS.SpatialAnalyst

Public Module GeodatabaseModule

    Dim m_messagePrefix As String = "Converting "

    Public Function BA_CreateFileGdb(ByVal Path As String, ByVal strName As String) As BA_ReturnCode
        ' Instantiate a file geodatabase workspace factory and create a file geodatabase.
        ' The Create method returns a workspace name object.
        Dim workspaceFactory As IWorkspaceFactory = New FileGDBWorkspaceFactory
        Dim workspaceName As IWorkspaceName = Nothing
        Dim name As IName = Nothing
        Dim workspace As IWorkspace = Nothing
        Try
            ' Check for existence of gdb by trapping the error generated when it doesn't exist
            Dim gdbExists As Boolean
            Try
                Dim gdbPath As String = Path & "\" & strName
                workspace = workspaceFactory.OpenFromFile(gdbPath, 0)
                gdbExists = True
            Catch ex As Exception
                'Do nothing; gdbExists is initialized to false by VB
            End Try
            If Not gdbExists Then
                workspaceName = workspaceFactory.Create(Path, strName, Nothing, 0)
                ' Cast the workspace name object to the IName interface and open the workspace.
                name = CType(workspaceName, IName)
                workspace = CType(name.Open(), IWorkspace)
            End If
            Return BA_ReturnCode.Success
        Catch ex As Exception
            MsgBox("BA_CreateFileGdb Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(workspaceFactory)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(workspaceName)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(name)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(workspace)
        End Try

    End Function

    Public Function BA_SaveRasterDatasetGDB(ByVal rasterDataset As IGeoDataset, ByRef strPath As String, ByVal sFormat As String, ByVal sName As String) As Short

        Dim workspaceFactory As IWorkspaceFactory = New FileGDBWorkspaceFactory
        Dim workspace As IWorkspace = Nothing
        Dim rasterWorkspaceEx As IRasterWorkspaceEx = Nothing
        Try
            ' Strip trailing "\" if exists
            If strPath(Len(strPath) - 1) = "\" Then
                strPath = strPath.Remove(Len(strPath) - 1, 1)
            End If
            workspace = workspaceFactory.OpenFromFile(strPath, 0)
            rasterWorkspaceEx = CType(workspace, IRasterWorkspaceEx)
            'Using ISaveAs2, specify the storage property for the output raster, such as tile size, compression,
            'and pyramid building, etc., for geodatabase raster, some also applies to output as a file format.
            Dim saveAs As ISaveAs2 = TryCast(rasterDataset, ISaveAs2)
            saveAs.SaveAs(sName, rasterWorkspaceEx, sFormat)
            Return 1
        Catch ex As Exception
            MessageBox.Show("BA_SaveRasterDatasetGDB Exception: " + ex.Message)
            Return 0
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(rasterWorkspaceEx)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(workspace)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(workspaceFactory)
        End Try

    End Function

    'Workaround because can't save output from pNeighborhoodOp directly to .gdb
    Public Function BA_SaveRasterDatasetGDB2(ByVal rasterDataset As IGeoDataset, ByVal aoiPath As String, _
                                             ByRef strPath As String, ByVal sFormat As String, _
                                             ByVal sName As String) As Short

        Dim tmpGDS As IGeoDataset = Nothing
        Try
            'strPath: C:\Docs\Lesley\ochoco_FGDB\zones\aspect1\aspect1.gdb
            If BA_SaveRasterDataset(rasterDataset, aoiPath, sName) = 1 Then
                tmpGDS = BA_OpenRasterFromFile(aoiPath, sName)
                BA_SaveRasterDatasetGDB(tmpGDS, strPath, sFormat, sName)
            End If
            BA_Remove_Raster(aoiPath, sName)
            Return 1
        Catch ex As Exception
            MessageBox.Show("BA_SaveRasterDatasetGDB2 Exception: " + ex.Message)
            Return 0
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(tmpGDS)
        End Try

    End Function

    'Saves a file from workspace format to file gdb
    Public Function BA_SaveFileRasterToGDB(ByVal inputPath As String, ByVal inputFile As String, _
                                           ByVal outputPath As String, ByVal outputFile As String, _
                                           ByVal sFormat As String) As BA_ReturnCode
        Dim inputGDS As IGeoDataset = Nothing
        Try
            inputGDS = BA_OpenRasterFromFile(inputPath, inputFile)
            If inputGDS IsNot Nothing Then
                Dim success As Short = BA_SaveRasterDatasetGDB(inputGDS, outputPath, sFormat, outputFile)
                If success = 1 Then
                    Return BA_ReturnCode.Success
                End If
            End If
            Return BA_ReturnCode.UnknownError
        Catch ex As Exception
            MessageBox.Show("BA_SaveRasterDatasetGDB2 Exception: " + ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            inputGDS = Nothing
        End Try
    End Function

    'Uses IRasterWorkspace to check if a file exists; Parses full path into file path and name before starting
    Public Function BA_File_Exists(ByVal fullPath As String, ByVal wksType As WorkspaceType, _
                                    ByVal datasetType As esriDatasetType) As Boolean
        If String.IsNullOrEmpty(fullPath) Then
            Return False
        End If
        Dim filePath As String = "PleaseReturn"
        Dim fileName As String = BA_GetBareName(fullPath, filePath)
        If wksType = WorkspaceType.Raster Then
            Return BA_File_ExistsRaster(filePath, fileName)
        ElseIf wksType = WorkspaceType.Geodatabase Then
            Dim pWSFactory As IWorkspaceFactory = New FileGDBWorkspaceFactory
            Dim pWS As IWorkspace2 = Nothing
            Try
                ' Strip trailing "\" if exists
                If filePath(Len(filePath) - 1) = "\" Then
                    filePath = filePath.Remove(Len(filePath) - 1, 1)
                End If
                pWS = pWSFactory.OpenFromFile(filePath, 0)
                If pWS.NameExists(datasetType, fileName) Then
                    Return True
                End If
                Return False
            Catch ex As Exception
                ' An exception was thrown while trying to open the dataset, return false
                Return False
            Finally
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWS)
            End Try
        End If
        Return False
    End Function

    'Opens and returns a IGeoDataset (raster) object
    Public Function BA_OpenRasterFromGDB(ByRef gdbPath As String, ByRef fileName As String) As IGeoDataset
        Dim pWSFactory As IWorkspaceFactory = New FileGDBWorkspaceFactory()
        Dim pWS As IWorkspace2 = Nothing
        Dim pRasterWs As IRasterWorkspaceEx = Nothing
        Try
            ' Strip trailing "\" if exists
            If gdbPath(Len(gdbPath) - 1) = "\" Then
                gdbPath = gdbPath.Remove(Len(gdbPath) - 1, 1)
            End If
            pWS = pWSFactory.OpenFromFile(gdbPath, 0)
            'Valid raster dataset
            If pWS.NameExists(esriDatasetType.esriDTRasterDataset, fileName) Then
                pRasterWs = CType(pWS, IRasterWorkspaceEx)  'Explicit cast
                Dim rasterDataset As IRasterDataset = pRasterWs.OpenRasterDataset(fileName)
                Dim geoDataset As IGeoDataset = CType(rasterDataset, IGeoDataset) ' Explicit Cast
                Return geoDataset
            Else
                'Not a valid raster dataset
                Return Nothing
            End If
        Catch ex As Exception
            'MessageBox.Show("Exception: " + ex.Message)
            Return Nothing
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterWs)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWS)
            pRasterWs = Nothing
            'Can't release factory or you get an error later on when you try to use the factory
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWSFactory)
        End Try
    End Function

    ' Opens and returns an IFeatureClass object
    Public Function BA_OpenFeatureClassFromGDB(ByRef gdbPath As String, ByRef FileName As String) As IFeatureClass
        Dim pWSFactory As IWorkspaceFactory = New FileGDBWorkspaceFactory()
        Dim pWS As IWorkspace2 = Nothing
        Dim pFeatWSpace As IFeatureWorkspace = Nothing
        Try
            ' Strip trailing "\" if exists
            If gdbPath(Len(gdbPath) - 1) = "\" Then
                gdbPath = gdbPath.Remove(Len(gdbPath) - 1, 1)
            End If
            pWS = pWSFactory.OpenFromFile(gdbPath, 0)
            'Valid feature class
            If pWS.NameExists(esriDatasetType.esriDTFeatureClass, FileName) Then
                pFeatWSpace = CType(pWS, IFeatureWorkspace)  'Explicit cast
                Return pFeatWSpace.OpenFeatureClass(FileName)
            Else
                'Not a valid feature class
                Return Nothing
            End If
        Catch ex As Exception
            'MessageBox.Show("Exception: " + ex.Message)
            Return Nothing
        Finally
            'Avoid RCW separations when workspace is accessed later in a calling sub
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeatWSpace)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWS)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWSFactory)
        End Try
    End Function

    'Delete a raster file using IRasterWorkspaceEx object
    Public Function BA_RemoveRasterFromGDB(ByVal gdbPath As String, ByVal FileName As String) As Short
        Dim pWSFactory As IWorkspaceFactory = New FileGDBWorkspaceFactory()
        Dim pWS As IWorkspace2 = Nothing
        Dim pRasterWs As IRasterWorkspaceEx = Nothing
        Dim pRasterData As IRasterDataset = Nothing
        Dim pDataset As IDataset = Nothing
        Try
            ' Strip trailing "\" if exists
            If gdbPath(Len(gdbPath) - 1) = "\" Then
                gdbPath = gdbPath.Remove(Len(gdbPath) - 1, 1)
            End If
            pWS = pWSFactory.OpenFromFile(gdbPath, 0)
            'Valid raster dataset
            If pWS.NameExists(esriDatasetType.esriDTRasterDataset, FileName) Then
                pRasterWs = CType(pWS, IRasterWorkspaceEx)  'Explicit cast
                pRasterData = pRasterWs.OpenRasterDataset(FileName)
                pDataset = CType(pRasterData, IDataset) 'Explicit cast
                pDataset.Delete()
                Return 1
            Else
                'Not a valid raster dataset
                Return 0
            End If
        Catch ex As Exception
            MessageBox.Show("BARemoveRasterFromGdb: " + ex.Message)
            Return 0
        Finally
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWSFactory)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterWs)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterData)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDataset)
        End Try
    End Function

    Public Function BA_Remove_ShapefileFromGDB(ByRef folder_string As String, ByRef shapefilename As String) As Short
        Dim pDataset As IDataset = Nothing
        Dim fc As IFeatureClass = Nothing

        Try
            fc = BA_OpenFeatureClassFromGDB(folder_string, shapefilename)
            If fc Is Nothing Then
                Return 0
            Else
                pDataset = CType(fc, IDataset)
                pDataset.Delete()
                Return 1
            End If
        Catch ex As Exception
            MessageBox.Show("BA_Remove_ShapefileFromGDB Exception: " + ex.Message)
            Return 0
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(fc)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDataset)
        End Try
    End Function

    Public Sub BA_Feature2RasterGDB(ByVal featClassDescr As IFeatureClassDescriptor, ByVal gdbPath As String, _
                                    ByVal FileName As String, ByVal Cellsize As Object, _
                                    ByVal valueField As String, ByVal snapRasterPath As String)
        Dim pWS As IWorkspace = Nothing
        Dim pWSFactory As IWorkspaceFactory = New RasterWorkspaceFactory
        Dim pConversionOp As IConversionOp = New RasterConversionOp
        Dim pEnv As IRasterAnalysisEnvironment = Nothing
        Dim pRDS As IRasterDataset = Nothing
        Dim snapGDS As IGeoDataset = Nothing
        Dim envelope As IEnvelope = Nothing

        Try
            'Commenting out check for integer values since we may also convert text values; Field values should be checked
            'before this is called from either BA_Feature2RasterInteger or BA_Feature2RasterDouble
            'If BA_IsIntegerField(featClassDescr, valueField) Then
            Dim hruPath As String = ""
            Dim tmpName As String = BA_GetBareName(gdbPath, hruPath)
            pWS = pWSFactory.OpenFromFile(hruPath, 0)
            pEnv = pConversionOp
            pEnv.SetCellSize(esriRasterEnvSettingEnum.esriRasterEnvValue, Cellsize)
            If Not String.IsNullOrEmpty(snapRasterPath) Then
                Dim snapPath As String = "PleaseReturn"
                Dim snapName As String = BA_GetBareName(snapRasterPath, snapPath)

                Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(snapPath)
                If WorkspaceType = WorkspaceType.Geodatabase Then
                    snapGDS = BA_OpenRasterFromGDB(snapPath, snapName)
                ElseIf workspaceType = workspaceType.Raster Then 'input is a GRID
                    snapGDS = BA_OpenRasterFromFile(snapPath, snapName)
                End If

                envelope = snapGDS.Extent
                Dim object_Envelope As System.Object = CType(envelope, System.Object) ' Explicit Cast
                pEnv.SetExtent(esriRasterEnvSettingEnum.esriRasterEnvValue, object_Envelope, snapRasterPath)
            End If
            pRDS = pConversionOp.ToRasterDataset(featClassDescr, "GRID", pWS, FileName)
            Dim success As Integer = BA_SaveRasterDatasetGDB(pRDS, gdbPath, BA_RASTER_FORMAT, FileName)
            If success = 1 Then
                BA_Remove_Raster(hruPath, FileName)
            End If
            'Else
            'Throw New Exception("Invalid values found in value field. Values must be whole numbers.")
            'End If
        Catch ex As Exception
            MessageBox.Show("BA_Feature2RasterGDB Exception: " + ex.Message)
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWS)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWSFactory)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pConversionOp)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(snapGDS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(envelope)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pEnv)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRDS)
        End Try
    End Sub

    Public Function BA_GetRasterStatsGDB(ByRef rasterfile_pathname As String, ByRef rasterresolution As Double) As IRasterStatistics
        Dim filepath As String = ""
        Dim FileName As String

        If String.IsNullOrEmpty(rasterfile_pathname) Then 'not a valid input
            Return Nothing
        End If

        Dim pDEUtility As ESRI.ArcGIS.Geoprocessing.IDEUtilities = New ESRI.ArcGIS.Geoprocessing.DEUtilities
        Dim pGeoDataset As IGeoDataset = Nothing
        Dim pRasterDataset As IRasterDataset = New RasterDataset
        Dim pRasterBand As IRasterBand = Nothing
        Dim pRasterBandCollection As IRasterBandCollection = Nothing

        Try
            '1. Open Raster
            FileName = BA_GetBareName(rasterfile_pathname, filepath)
            pGeoDataset = BA_OpenRasterFromGDB(filepath, FileName)
            pRasterDataset = CType(pGeoDataset, IRasterDataset) ' Explicit cast

            '2. QI Raster to IRasterBand
            pRasterBandCollection = pRasterDataset
            pRasterBand = pRasterBandCollection.Item(0)

            'QI IRasterProps
            Dim pRasterP As IRasterProps = pRasterBand
            Dim pPnt As IPnt = New DblPnt
            pPnt = pRasterP.MeanCellSize
            rasterresolution = (pPnt.X + pPnt.Y) / 2
            'Compute statistics if not already calculated
            pRasterBand.ComputeStatsAndHist()
            Return pRasterBand.Statistics
        Catch ex As Exception
            rasterresolution = 0
            Debug.Print("BA_GetRasterStatsGDB Exception: " + ex.Message)
            Return Nothing
        Finally
            pDEUtility.ReleaseInternals()
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBand)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBandCollection)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterDataset)
        End Try
    End Function

    ' Generates a new raster replacing NoData cells with a selected value. The replacement
    ' extent may be masked if a mask file path is provided
    Public Function BA_ReplaceNoDataCellsGDB(ByVal inputFolder As String, ByVal inputFile As String, _
                                          ByVal outputFolder As String, ByVal outputFile As String, _
                                          ByVal replaceValue As Integer, ByVal maskFolder As String, _
                                          ByVal maskFile As String) As BA_ReturnCode
        Dim mapAlgebraOp As ESRI.ArcGIS.SpatialAnalyst.IMapAlgebraOp = New ESRI.ArcGIS.SpatialAnalyst.RasterMapAlgebraOp
        Dim inputGeodataset As IGeoDataset = Nothing
        Dim pRasterDataset As IRasterDataset3 = Nothing
        Dim isNullGeodataset As IGeoDataset = Nothing
        Dim outputGeodataset As IGeoDataset = Nothing
        Dim maskFeatureClass As IFeatureClass = Nothing
        Dim maskGeodataset As IGeoDataset = Nothing
        Dim workspaceFactory As IWorkspaceFactory = New FileGDBWorkspaceFactory()
        Dim workspace As IWorkspace = Nothing
        Dim pEnv As IRasterAnalysisEnvironment = Nothing
        Dim retVal As BA_ReturnCode = BA_ReturnCode.UnknownError

        Try
            inputGeodataset = BA_OpenRasterFromGDB(inputFolder, inputFile)
            If inputGeodataset IsNot Nothing Then
                pRasterDataset = CType(inputGeodataset, IRasterDataset3) ' Explicit cast
                'Set environment
                If Not String.IsNullOrEmpty(maskFolder) AndAlso Not String.IsNullOrEmpty(maskFolder) Then
                    pEnv = CType(mapAlgebraOp, IRasterAnalysisEnvironment)  ' Explicit cast
                    maskFeatureClass = BA_OpenFeatureClassFromGDB(maskFolder, maskFile)
                    maskGeodataset = CType(maskFeatureClass, IGeoDataset)
                    pEnv.Mask = maskGeodataset
                    ' Set the analysis extent to match the mask
                    Dim extentProvider As Object = CType(maskGeodataset.Extent, Object)
                    pEnv.SetExtent(esriRasterEnvSettingEnum.esriRasterEnvValue, extentProvider)
                    workspace = workspaceFactory.OpenFromFile(outputFolder, 0)
                    pEnv.OutWorkspace = workspace
                End If
                mapAlgebraOp.BindRaster(pRasterDataset, "noData1")
                isNullGeodataset = mapAlgebraOp.Execute("Con(IsNull([noData1]), " & replaceValue & ", [noData1])")
                If isNullGeodataset IsNot Nothing Then
                    BA_SaveRasterDatasetGDB(isNullGeodataset, outputFolder, BA_RASTER_FORMAT, outputFile)
                    retVal = BA_ReturnCode.Success
                End If
            End If
            Return retVal
        Catch ex As Exception
            MsgBox("BA_ReplaceNoDataCellsGDB Exception: " & ex.Message)
            Return retVal
        Finally
            mapAlgebraOp.UnbindRaster("noData1")
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pEnv)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(maskGeodataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(maskFeatureClass)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(outputGeodataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(isNullGeodataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(inputGeodataset)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(workspaceFactory)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(workspace)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(mapAlgebraOp)
        End Try

    End Function

    'Opens a raster and checks the properties to see if it is an integer raster
    Public Function BA_IsIntegerRasterGDB(ByVal fullLayerFilePath As String) As Boolean
        Dim filePath As String = "blank"
        Dim fileName As String = BA_GetBareName(fullLayerFilePath, filePath)
        Dim pGeoDS As IGeoDataset = Nothing
        Dim pRasterBandColl As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pRasterP As IRasterProps = Nothing
        Try
            pGeoDS = BA_OpenRasterFromGDB(filePath, fileName)
            If pGeoDS Is Nothing Then
                Return False
            End If
            pRasterBandColl = CType(pGeoDS, IRasterBandCollection)
            pRasterBand = pRasterBandColl.Item(0)
            pRasterP = pRasterBand
            Return pRasterP.IsInteger
        Catch ex As Exception
            MessageBox.Show("BA_IsIntegerRaster Exception: " + ex.Message)
            Return False
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterP)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBand)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBandColl)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGeoDS)
        End Try

    End Function

    Public Function BA_RemoveFilesByPrefix(ByVal workspaceName As String, ByVal tempPrefix As String) As BA_ReturnCode
        Dim pWSF As IWorkspaceFactory = New FileGDBWorkspaceFactory()
        Dim pWorkspace As IWorkspace = Nothing
        Dim pEnumDSName As IEnumDatasetName = Nothing
        Dim pDSName As IDatasetName = Nothing
        Dim pRasterWs As IRasterWorkspaceEx = Nothing
        Dim pFeatWSpace As IFeatureWorkspace = Nothing
        Dim pDataset As IDataset = Nothing

        Try
            ' Strip trailing "\" if exists
            If workspaceName(Len(workspaceName) - 1) = "\" Then
                workspaceName = workspaceName.Remove(Len(workspaceName) - 1, 1)
            End If
            pWorkspace = pWSF.OpenFromFile(workspaceName, 0)
            ' Delete rasters
            pEnumDSName = pWorkspace.DatasetNames(esriDatasetType.esriDTRasterDataset)
            pDSName = pEnumDSName.Next
            Dim pName As String = Nothing
            While Not pDSName Is Nothing
                pName = pDSName.Name
                If pName.Substring(0, tempPrefix.Length) = tempPrefix Then
                    pRasterWs = CType(pWorkspace, IRasterWorkspaceEx)
                    pDataset = pRasterWs.OpenRasterDataset(pName)
                    pDataset.Delete()
                End If
                pDSName = pEnumDSName.Next
            End While

            'Delete vectors
            pEnumDSName = pWorkspace.DatasetNames(esriDatasetType.esriDTFeatureDataset)
            pDSName = pEnumDSName.Next
            pName = Nothing
            While Not pDSName Is Nothing
                pName = pDSName.Name
                If pName.Substring(0, tempPrefix.Length) = tempPrefix Then
                    pFeatWSpace = CType(pWorkspace, IFeatureWorkspace)
                    pDataset = pFeatWSpace.OpenFeatureDataset(pName)
                    pDataset.Delete()
                End If
                pDSName = pEnumDSName.Next
            End While

            Return BA_ReturnCode.Success
        Catch ex As Exception
            MsgBox("BA_RemoveTemporaryRasters & Exception: " & ex.Message)
            Return BA_ReturnCode.OtherError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterWs)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeatWSpace)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDSName)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pEnumDSName)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWorkspace)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWSF)
        End Try
    End Function

    Public Function BA_RenameFilesByPrefix(ByVal workspaceName As String, ByVal oldPrefix As String, _
                                           ByVal newPrefix As String) As BA_ReturnCode
        Dim pWSF As IWorkspaceFactory = New FileGDBWorkspaceFactory()
        Dim pWorkspace As IWorkspace = Nothing
        Dim pEnumDSName As IEnumDatasetName = Nothing
        Dim pDSName As IDatasetName = Nothing
        Dim pRasterWs As IRasterWorkspaceEx = Nothing
        Dim pFeatWSpace As IFeatureWorkspace = Nothing
        Dim pDataset As IDataset = Nothing

        Try
            ' Strip trailing "\" if exists
            If workspaceName(Len(workspaceName) - 1) = "\" Then
                workspaceName = workspaceName.Remove(Len(workspaceName) - 1, 1)
            End If
            pWorkspace = pWSF.OpenFromFile(workspaceName, 0)
            ' Delete rasters
            pEnumDSName = pWorkspace.DatasetNames(esriDatasetType.esriDTRasterDataset)
            pDSName = pEnumDSName.Next
            Dim pName As String = Nothing
            While Not pDSName Is Nothing
                pName = pDSName.Name
                If pName.Substring(0, oldPrefix.Length) = oldPrefix Then
                    pRasterWs = CType(pWorkspace, IRasterWorkspaceEx)
                    pDataset = pRasterWs.OpenRasterDataset(pName)
                    pDataset.Rename(pName.Replace(oldPrefix, newPrefix))
                End If
                pDSName = pEnumDSName.Next
            End While

            'Delete vectors
            pEnumDSName = pWorkspace.DatasetNames(esriDatasetType.esriDTFeatureDataset)
            pDSName = pEnumDSName.Next
            pName = Nothing
            While Not pDSName Is Nothing
                pName = pDSName.Name
                If pName.Substring(0, oldPrefix.Length) = oldPrefix Then
                    pFeatWSpace = CType(pWorkspace, IFeatureWorkspace)
                    pDataset = pFeatWSpace.OpenFeatureDataset(pName)
                    pDataset.Rename(pName.Replace(oldPrefix, newPrefix))
                End If
                pDSName = pEnumDSName.Next
            End While

            Return BA_ReturnCode.Success
        Catch ex As Exception
            MsgBox("BA_RemoveTemporaryRasters & Exception: " & ex.Message)
            Return BA_ReturnCode.OtherError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterWs)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeatWSpace)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDSName)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pEnumDSName)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWorkspace)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWSF)
        End Try
    End Function

    ' Return the folder path that contains the gdb
    Public Function BA_HruFolderPathFromGdbString(ByVal gdbPath As String) As String
        Dim retVal As String = "PleaseReturn"
        Dim strTemp As String = Nothing
        Dim pos As Short = gdbPath.IndexOf(".gdb")
        If pos > 1 Then
            gdbPath = gdbPath.Substring(0, pos)
            strTemp = BA_GetBareName(gdbPath, retVal)
        End If
        Return retVal
    End Function

    Public Function BA_GetWorkspaceTypeFromPath(ByVal inputPath As String) As WorkspaceType
        If inputPath.IndexOf(".gdb") > -1 Then
            Return WorkspaceType.Geodatabase
        Else
            Return WorkspaceType.Raster
        End If
    End Function

    Public Function BA_RenameRasterInGDB(ByVal inputFolder As String, ByVal inputFile As String, _
                                         ByVal outputFile As String) As BA_ReturnCode
        Dim pInputDataset As IGeoDataset = Nothing
        Dim pDataset As IDataset = Nothing
        Try
            pInputDataset = BA_OpenRasterFromGDB(inputFolder, inputFile)
            If pInputDataset IsNot Nothing Then
                pDataset = CType(pInputDataset, IDataset)
                pDataset.Rename(outputFile)
            End If
            Return BA_ReturnCode.Success
        Catch ex As Exception
            MessageBox.Show("BA_RenameRasterInGDB() Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pInputDataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDataset)
        End Try

    End Function

    Public Function BA_RenameFeatureClassInGDB(ByVal inputFolder As String, ByVal inputFile As String, _
                                               ByVal outputFile As String) As BA_ReturnCode
        Dim pInputDataset As IGeoDataset = Nothing
        Dim pDataset As IDataset = Nothing
        Try
            pInputDataset = BA_OpenFeatureClassFromGDB(inputFolder, inputFile)
            If pInputDataset IsNot Nothing Then
                pDataset = CType(pInputDataset, IDataset)
                pDataset.Rename(outputFile)
            End If
            Return BA_ReturnCode.Success
        Catch ex As Exception
            MessageBox.Show("BA_RenameFeatureClassInGDB() Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pInputDataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDataset)
        End Try

    End Function

    'Copies an array of rasters to the file geodatabase; Intended for use in converting data from Weasel to BAGIS format
    Public Function BA_CopyRastersToGDB(ByVal rasterTable As Hashtable, ByVal stepProgressor As IStepProgressor) As BA_ReturnCode
        Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
        Dim pResult As ESRI.ArcGIS.Geoprocessing.IGeoProcessorResult = Nothing
        Dim copyRaster As CopyRaster = New CopyRaster
        Dim inRasterPath As String = Nothing
        Dim outRasterPath As String = Nothing
        Try
            Dim RasterCount As Integer = rasterTable.Count
            If RasterCount > 0 Then
                GP.OverwriteOutput = True
                GP.SetEnvironmentValue("cellsize", "MINOF")
                GP.AddOutputsToMap = False
                For Each de As DictionaryEntry In rasterTable
                    inRasterPath = TryCast(de.Key, String)
                    outRasterPath = TryCast(de.Value, String)
                    If Not String.IsNullOrEmpty(inRasterPath) AndAlso Not String.IsNullOrEmpty(outRasterPath) Then
                        copyRaster.in_raster = inRasterPath
                        copyRaster.out_rasterdataset = outRasterPath
                        Dim progressMessage As String = m_messagePrefix & inRasterPath
                        If progressMessage.Length > MAX_PROGRESSOR_MSG_PATH_LENGTH Then
                            Dim messageSuffixLength As Int16 = MAX_PROGRESSOR_MSG_PATH_LENGTH - m_messagePrefix.Length
                            ' Display as much of the left-hand side of the filepath as we can with the message length limitation
                            progressMessage = m_messagePrefix & inRasterPath.Substring(inRasterPath.Length - messageSuffixLength)
                        End If
                        stepProgressor.Message = progressMessage
                        stepProgressor.Step()
                        pResult = GP.Execute(copyRaster, Nothing)
                    End If
                Next
            End If
            Return BA_ReturnCode.Success
        Catch ex As Exception
            MessageBox.Show("BA_CopyRastersToGDB Exception: " & ex.Message)
            Debug.Print(inRasterPath)
            Debug.Print(outRasterPath)
            Return BA_ReturnCode.UnknownError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(copyRaster)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pResult)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(GP)
        End Try
    End Function

    'Copies an array of vectors to the file geodatabase; Intended for use in converting data from Weasel to BAGIS format
    Public Function BA_CopyVectorsToGDB(ByVal vectorTable As Hashtable, ByVal stepProgressor As IStepProgressor) As BA_ReturnCode
        Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
        Dim pResult As ESRI.ArcGIS.Geoprocessing.IGeoProcessorResult = Nothing
        Dim copyFeatures As CopyFeatures = New CopyFeatures
        Try
            Dim vectorCount As Integer = vectorTable.Count
            If vectorCount > 0 Then
                GP.OverwriteOutput = True
                GP.AddOutputsToMap = False
                For Each de As DictionaryEntry In vectorTable
                    Dim inVectorPath As String = TryCast(de.Key, String)
                    Dim outVectorPath As String = TryCast(de.Value, String)
                    If Not String.IsNullOrEmpty(inVectorPath) AndAlso Not String.IsNullOrEmpty(outVectorPath) Then
                        copyFeatures.in_features = inVectorPath
                        copyFeatures.out_feature_class = outVectorPath
                        Dim progressMessage As String = m_messagePrefix & inVectorPath
                        If progressMessage.Length > MAX_PROGRESSOR_MSG_PATH_LENGTH Then
                            Dim messageSuffixLength As Int16 = MAX_PROGRESSOR_MSG_PATH_LENGTH - m_messagePrefix.Length
                            ' Display as much of the left-hand side of the filepath as we can with the message length limitation
                            progressMessage = m_messagePrefix & inVectorPath.Substring(inVectorPath.Length - messageSuffixLength)
                        End If
                        stepProgressor.Message = progressMessage
                        stepProgressor.Step()
                        pResult = GP.Execute(copyFeatures, Nothing)
                    End If
                Next
            End If
            Return BA_ReturnCode.Success
        Catch ex As Exception
            MessageBox.Show("BA_CopyVectorsToGDB Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(copyFeatures)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pResult)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(GP)
        End Try
    End Function

    'Copies an array of rasters to the file geodatabase; Intended for use in converting data from Weasel to BAGIS format
    Public Function BA_CopyPropertiesToGDB(ByVal propertyTable As Hashtable) As BA_ReturnCode
        Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
        Dim pResult As ESRI.ArcGIS.Geoprocessing.IGeoProcessorResult = Nothing
        Dim copyTool As Copy = New Copy
        Dim inPath As String = Nothing
        Dim outPath As String = Nothing
        Try
            Dim RasterCount As Integer = propertyTable.Count
            If RasterCount > 0 Then
                GP.OverwriteOutput = True
                GP.AddOutputsToMap = False
                For Each de As DictionaryEntry In propertyTable
                    inPath = TryCast(de.Key, String)
                    outPath = TryCast(de.Value, String)
                    If Not String.IsNullOrEmpty(inPath) AndAlso Not String.IsNullOrEmpty(outPath) Then
                        copyTool.in_data = inPath
                        copyTool.out_data = outPath
                        pResult = GP.Execute(copyTool, Nothing)
                    End If
                Next
            End If
            Return BA_ReturnCode.Success
        Catch ex As Exception
            MessageBox.Show("BA_CopyRastersToGDB Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(copyTool)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pResult)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(GP)
        End Try
    End Function

    Public Sub BA_UpdateHashtableForRasterCopy(ByRef pTable As Hashtable, ByVal inPath As String, ByVal outPath As String, _
                                               ByVal layerNames() As String)
        Dim layerCount As Integer = UBound(layerNames)
        If layerCount > 0 Then
            For i = 0 To layerCount
                If Not String.IsNullOrEmpty(layerNames(i)) Then
                    Dim inRasterPath As String = inPath & "\" & layerNames(i)
                    Dim outRasterPath As String = outPath & "\" & layerNames(i)
                    pTable.Add(inRasterPath, outRasterPath)
                End If
            Next
        End If
    End Sub

    Public Sub BA_UpdateHashtableForVectorCopy(ByRef pTable As Hashtable, ByVal inPath As String, ByVal outPath As String, _
                                               ByVal layerNames() As String)

        Dim layerCount As Integer = UBound(layerNames)
        If layerCount > 0 Then
            For i = 0 To layerCount
                If Not String.IsNullOrEmpty(layerNames(i)) Then
                    Dim inRasterPath As String = inPath & BA_StandardizeShapefileName(layerNames(i), True, True)
                    Dim outRasterPath As String = outPath & "\" & layerNames(i)
                    pTable.Add(inRasterPath, outRasterPath)
                End If
            Next
        End If
    End Sub

    ' Deletes a file geodatabase. Checks first to see if any layers in target gdb folder are in the
    ' current map document. If so, these layers are removed before the gdb is deleted
    Public Function BA_DeleteGeodatabase(ByVal gdbPath As String, ByVal pmxDoc As IMxDocument) As BA_ReturnCode
        ' Remove any layers from ArcMap in the hru folder
        BA_RemoveLayersInFolder(pmxDoc, gdbPath)
        ' Delete the gdb; Note that BA_Remove_Workspace did not work with the gdb folder
        'BA_Remove_Folder(gdbPath)
        BA_Remove_WorkspaceGP(gdbPath)
        If BA_Workspace_Exists(gdbPath) Then
            Return BA_ReturnCode.UnknownError
        End If
        Return BA_ReturnCode.Success
    End Function

    ' Create the gdb folders for a given aoi when converting from Weasel format
    Public Function BA_CreateGeodatabaseFolders(ByVal aoiPath As String, ByVal folderType As FolderType) As BA_ReturnCode
        Dim success As BA_ReturnCode = BA_ReturnCode.UnknownError
        If folderType = folderType.BASIN Then
            ' If we are working with a basin, we only need the aoi.gdb, surfaces.gdb
            Dim gdbName As String = BA_EnumDescription(GeodatabaseNames.Aoi)
            success = BA_CreateFileGdb(aoiPath, gdbName)
            gdbName = BA_EnumDescription(GeodatabaseNames.Surfaces)
            success = BA_CreateFileGdb(aoiPath, gdbName)
        ElseIf folderType = folderType.AOI Then
            ' Otherwise we need them all
            For Each pName In [Enum].GetValues(GetType(GeodatabaseNames))
                Dim EnumConstant As [Enum] = pName
                Dim fi As Reflection.FieldInfo = EnumConstant.GetType().GetField(EnumConstant.ToString())
                Dim aattr() As DescriptionAttribute = DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
                Dim gdbName As String = aattr(0).Description
                success = BA_CreateFileGdb(aoiPath, gdbName)
                If success <> BA_ReturnCode.Success Then
                    Return success
                End If
            Next
        End If
        Return success
    End Function

    Public Function BA_CheckForBagisGDB(ByVal aoiPath As String) As List(Of String)
        Dim existsList As List(Of String) = New List(Of String)
        For Each pName In [Enum].GetValues(GetType(GeodatabaseNames))
            Dim EnumConstant As [Enum] = pName
            Dim fi As Reflection.FieldInfo = EnumConstant.GetType().GetField(EnumConstant.ToString())
            Dim aattr() As DescriptionAttribute = DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
            Dim gdbName As String = aattr(0).Description
            Dim gdbPath As String = aoiPath & "\" & gdbName
            'If geodatabase already exists add it to a list; We will warn the user below
            If BA_Folder_ExistsWindowsIO(gdbPath) Then
                existsList.Add(gdbPath)
            End If
        Next
        Return existsList
    End Function

    ' Returns arrays of raster and vector files in a given workspace (folder) 
    Public Sub BA_ListLayersinGDB(ByVal gdbPath As String, ByRef RasterList() As String, ByRef VectorList() As String)
        Dim nshapefile, nraster, i As Integer
        Dim pWSF As IWorkspaceFactory = New FileGDBWorkspaceFactory()
        Dim pWorkspace As IWorkspace = Nothing
        Dim pEnumDSName As IEnumDatasetName = Nothing
        Dim pDSName As IDatasetName = Nothing

        Try
            ' Strip trailing "\" if exists
            If gdbPath(Len(gdbPath) - 1) = "\" Then
                gdbPath = gdbPath.Remove(Len(gdbPath) - 1, 1)
            End If
            pWorkspace = pWSF.OpenFromFile(gdbPath, 0)

            'list shapefiles
            pEnumDSName = pWorkspace.DatasetNames(esriDatasetType.esriDTFeatureClass)
            pDSName = pEnumDSName.Next

            While Not pDSName Is Nothing
                nshapefile = nshapefile + 1
                pDSName = pEnumDSName.Next
            End While

            If nshapefile > 0 Then
                ReDim VectorList(nshapefile)
            Else
                ReDim VectorList(0)
            End If

            pEnumDSName.Reset()
            For i = 1 To nshapefile
                pDSName = pEnumDSName.Next
                VectorList(i) = pDSName.Name
            Next

            pDSName = Nothing

            'list raster files
            pEnumDSName = pWorkspace.DatasetNames(esriDatasetType.esriDTRasterDataset)
            pDSName = pEnumDSName.Next

            While Not pDSName Is Nothing
                If Not IsCpgFile(pDSName.Name) Then nraster = nraster + 1
                pDSName = pEnumDSName.Next
            End While

            If nraster > 0 Then
                ReDim RasterList(nraster)
            Else
                ReDim RasterList(0)
            End If

            pEnumDSName.Reset()
            pDSName = pEnumDSName.Next
            Dim pos As Short = 1
            While Not pDSName Is Nothing
                If Not IsCpgFile(pDSName.Name) Then
                    RasterList(pos) = pDSName.Name
                    pos += 1
                End If
                pDSName = pEnumDSName.Next
            End While

        Catch ex As Exception
            MessageBox.Show("BA_ListLayersinGDB Exception: " + ex.Message)
        Finally
            ' release COM references
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDSName)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pEnumDSName)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWorkspace)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWSF)
        End Try
    End Sub

    Private Function IsCpgFile(ByVal strFileName As String) As Boolean
        If Strings.Right(strFileName, 4).ToUpper = ".CPG" Then
            Return True
        End If
        Return False
    End Function

    ' Check to see if Weasel files exist in AOI 
    Public Function BA_CheckForWeaselFiles(ByVal aoiPath As String) As Boolean
        Dim aoiVectorList(0) As String
        Dim aoiRasterList(0) As String

        BA_ListLayersinAOI(aoiPath, aoiRasterList, aoiVectorList)
        'This relies on BA_ListLayersinAOI returning the first entry in position 1 which was inherited from VBA version
        If aoiVectorList.Length > 1 Or aoiRasterList.Length > 1 Then
            Return True
        End If
        Return False
    End Function

    Public Sub BA_UpdateHashtableForRasterToWeasel(ByRef pTable As Hashtable, ByVal inPath As String, ByVal outPath As String, _
                                                   ByVal layerNames() As String)
        Dim layerCount As Integer = UBound(layerNames)
        If layerCount > 0 Then
            For i = 0 To layerCount
                If Not String.IsNullOrEmpty(layerNames(i)) Then
                    Dim inRasterPath As String = inPath & "\" & layerNames(i)
                    'Dim outRasterPath As String = outPath & "\" & layerNames(i) & "\" & GRID
                    Dim outRasterPath As String = outPath & "\" & layerNames(i)
                    pTable.Add(inRasterPath, outRasterPath)
                End If
            Next
        End If
    End Sub

    Public Sub BA_UpdateHashtableForVectorToWeasel(ByRef pTable As Hashtable, ByVal inPath As String, ByVal outPath As String, _
                                                   ByVal layerNames() As String)

        Dim layerCount As Integer = UBound(layerNames)
        If layerCount > 0 Then
            For i = 0 To layerCount
                If Not String.IsNullOrEmpty(layerNames(i)) Then
                    Dim inRasterPath As String = inPath & "\" & layerNames(i)
                    Dim outRasterPath As String = outPath & BA_StandardizeShapefileName(layerNames(i), True, True)
                    pTable.Add(inRasterPath, outRasterPath)
                End If
            Next
        End If
    End Sub

    Public Function BA_GeodatabasePath(ByVal aoiPath As String, ByVal gdbEnum As GeodatabaseNames, _
                                          Optional ByVal hasTrailingBackSlash As Boolean = False) As String
        Dim sb As StringBuilder = New StringBuilder
        sb.Append(aoiPath)
        sb.Append("\")
        sb.Append(BA_EnumDescription(gdbEnum))
        If hasTrailingBackSlash = True Then sb.Append("\")
        Return sb.ToString
    End Function

    '13-JUL-2011 This function works but commenting out because I don't plan to use
    'raster catalog functionality
    'Public Sub BA_CreateRasterCatalog(ByVal filePath As String, ByVal fileName As String, _
    '                                  ByVal gdbPath As String, ByVal cName As String)

    '    Dim geoDataset As IGeoDataset = BA_OpenFeatureClassFromFile(filePath, fileName)
    '    Dim spatialRef As ISpatialReference = geoDataset.SpatialReference
    '    Dim success As BA_ReturnCode = BA_CreateRasterCatalog_GP(gdbPath, cName, spatialRef)
    'End Sub

    '13-JUL-2011 Giving up on trying to use raster catalog functionality. Unable to save
    'raster to gdb. After further research, it appears this isn't the appropriate use for this
    'technology anyway. Should be used for grouping tiled rasters rather than as a folder 
    'to organize rasters covering the same area
    'Public Function BA_SaveRasterDatasetToCatalog(ByVal gdbPath As String, ByVal cName As String, _
    '                                              ByVal pRasterDataset As IGeoDataset) As Short

    '    Dim pRasterCatalog As IRasterCatalog = Nothing
    '    Dim fClass As IFeatureClass = Nothing
    '    Dim feature As IFeature = Nothing
    '    Dim rasterValue As IRasterValue = New RasterValueClass()
    '    Dim pWSFactory As IWorkspaceFactory = New FileGDBWorkspaceFactory()
    '    Dim pWS As IWorkspace2 = Nothing
    '    Dim pRasterWs As IRasterWorkspaceEx = Nothing

    '    Try
    '        If gdbPath(Len(gdbPath) - 1) = "\" Then
    '            gdbPath = gdbPath.Remove(Len(gdbPath) - 1, 1)
    '        End If
    '        pWS = pWSFactory.OpenFromFile(gdbPath, 0)
    '        'Valid raster dataset
    '        pRasterWs = CType(pWS, IRasterWorkspaceEx)  'Explicit cast
    '        pRasterCatalog = pRasterWs.OpenRasterCatalog(cName)

    '        fClass = CType(pRasterCatalog, IFeatureClass)
    '        feature = fClass.CreateFeature()

    '        'Create a raster value.
    '        rasterValue.RasterDataset = pRasterDataset

    '        feature.set_Value(pRasterCatalog.NameFieldIndex, rasterValue)
    '        'Throws exception when you try to store the feature
    '        feature.Store()

    '        Return 1
    '    Catch ex As Exception
    '        MsgBox("BA_SaveRasterDatasetToCatalog() Exception: " & ex.Message)
    '        Return 0
    '    Finally
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterCatalog)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(fClass)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(feature)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(rasterValue)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWS)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterWs)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWSFactory)
    '    End Try

    'End Function

    ' Opens and returns an IFeatureClass object
    Public Function BA_OpenTableFromGDB(ByRef gdbPath As String, ByRef FileName As String) As ITable
        Dim pWSFactory As IWorkspaceFactory = New FileGDBWorkspaceFactory()
        Dim pWS As IWorkspace2 = Nothing
        Dim pFeatWSpace As IFeatureWorkspace = Nothing
        Try
            ' Strip trailing "\" if exists
            If gdbPath(Len(gdbPath) - 1) = "\" Then
                gdbPath = gdbPath.Remove(Len(gdbPath) - 1, 1)
            End If
            pWS = pWSFactory.OpenFromFile(gdbPath, 0)
            'Valid feature class
            If pWS.NameExists(esriDatasetType.esriDTTable, FileName) Then
                pFeatWSpace = CType(pWS, IFeatureWorkspace)  'Explicit cast
                Return pFeatWSpace.OpenTable(FileName)
            Else
                'Not a valid feature class
                Return Nothing
            End If
        Catch ex As Exception
            'MessageBox.Show("Exception: " + ex.Message)
            Return Nothing
        Finally
            pWSFactory = Nothing
            pWS = Nothing
            pFeatWSpace = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    Public Function BA_Remove_TableFromGDB(ByRef folder_string As String, ByRef file_name As String) As BA_ReturnCode
        Dim pDataset As IDataset
        Dim fTable As ITable

        Try
            fTable = BA_OpenTableFromGDB(folder_string, file_name)
            If fTable Is Nothing Then
                Return BA_ReturnCode.ReadError
            Else
                pDataset = CType(fTable, IDataset)
                pDataset.Delete()
                Return BA_ReturnCode.Success
            End If
        Catch ex As Exception
            Debug.Print("BA_Remove_TableFromGDB Exception: " + ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            fTable = Nothing
            pDataset = Nothing
        End Try
    End Function

    'Executes a map algebra function based on a layer stored in a gdb; You need to provide the expression
    'and the name of the layer to bind to. The name of the bound layer is usually part of the expression
    'IMPORTANT: output folder must be a workspace rather than Geodatabase. RasterMapAlgebraOp rasters cannot
    'be saved directly to a Geodatabase due to an ArcMap bug
    Public Function BA_ExecuteMapAlgebraGDB(ByVal inputFolder As String, ByVal inputFile As String, _
                                            ByVal outputFolder As String, ByVal outputFile As String, _
                                            ByVal bindInputName As String, _
                                            ByVal mapExpression As String) As BA_ReturnCode
        Dim mapAlgebraOp As IMapAlgebraOp = New RasterMapAlgebraOp
        Dim inputGeodataset As IGeoDataset = Nothing
        Dim pRasterDataset As IRasterDataset3 = Nothing
        Dim outputGeodataset As IGeoDataset = Nothing
        Try
            inputGeodataset = BA_OpenRasterFromGDB(inputFolder, inputFile)
            If inputGeodataset IsNot Nothing Then
                pRasterDataset = CType(inputGeodataset, IRasterDataset3) ' Explicit cast
                mapAlgebraOp.BindRaster(pRasterDataset, bindInputName)
                outputGeodataset = mapAlgebraOp.Execute(mapExpression)
                If outputGeodataset IsNot Nothing Then
                    Dim success As Short = BA_SaveRasterDataset(outputGeodataset, outputFolder, outputFile)
                    If success = 1 Then
                        Return BA_ReturnCode.Success
                    End If
                End If
            End If
            Return BA_ReturnCode.UnknownError
        Catch ex As Exception
            MsgBox("BA_ExecuteMapAlgebra() Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            inputGeodataset = Nothing
            pRasterDataset = Nothing
            outputGeodataset = Nothing
            mapAlgebraOp = Nothing
        End Try

    End Function

    'Converts a shapefile to a feature class
    'Copied from ESRI Help: Converting simple data 
    Public Function BA_ConvertShapeFileToGDB(ByVal sourcePath As String, ByVal sourceFile As String, ByVal targetGDB As String, ByVal targetFile As String) As BA_ReturnCode
        Try

            ' Validate source path
            Dim pType As WorkspaceType = BA_GetWorkspaceTypeFromPath(sourcePath)
            If pType = WorkspaceType.Geodatabase Then
                MessageBox.Show("Source folder cannot be a geodatabase", "Source folder cannot be a geodatabase", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return BA_ReturnCode.ReadError
            End If
            ' Validate target path
            pType = BA_GetWorkspaceTypeFromPath(targetGDB)
            If pType <> WorkspaceType.Geodatabase Then
                MessageBox.Show("Source folder must be a geodatabase", "Source folder must be a geodatabase", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return BA_ReturnCode.ReadError
            End If
            'Be sure that sourceFile has .shp extension
            sourceFile = BA_StandardizeShapefileName(sourceFile, True)

            ' Create a name object for the source workspace and open it.
            Dim sourceWorkspaceName As IWorkspaceName = New WorkspaceName With _
                                                        { _
                                                        .WorkspaceFactoryProgID = "esriDataSourcesFile.ShapefileWorkspaceFactory", _
                                                        .PathName = sourcePath _
                                                        }
            Dim sourceWorkspaceIName As IName = CType(sourceWorkspaceName, IName)
            Dim sourceWorkspace As IWorkspace = CType(sourceWorkspaceIName.Open(), IWorkspace)

            ' Create a name object for the target workspace and open it.
            Dim targetWorkspaceName As IWorkspaceName = New WorkspaceName With _
                                                        { _
                                                        .WorkspaceFactoryProgID = "esriDataSourcesGDB.FileGDBWorkspaceFactory", _
                                                        .PathName = targetGDB _
                                                        }
            Dim targetWorkspaceIName As IName = CType(targetWorkspaceName, IName)
            Dim targetWorkspace As IWorkspace = CType(targetWorkspaceIName.Open(), IWorkspace)

            ' Create a name object for the source dataset.
            Dim sourceFeatureClassName As IFeatureClassName = New FeatureClassName
            Dim sourceDatasetName As IDatasetName = CType(sourceFeatureClassName, IDatasetName)
            sourceDatasetName.Name = sourceFile
            sourceDatasetName.WorkspaceName = sourceWorkspaceName

            ' Create a name object for the target dataset.
            Dim targetFeatureClassName As IFeatureClassName = New FeatureClassName
            Dim targetDatasetName As IDatasetName = CType(targetFeatureClassName, IDatasetName)
            targetDatasetName.Name = targetFile
            targetDatasetName.WorkspaceName = targetWorkspaceName

            ' Open source feature class to get field definitions.
            Dim sourceName As IName = CType(sourceFeatureClassName, IName)
            Dim sourceFeatureClass As IFeatureClass = CType(sourceName.Open(), IFeatureClass)

            ' Create the objects and references necessary for field validation.
            Dim fieldChecker As IFieldChecker = New FieldChecker
            Dim sourceFields As IFields = sourceFeatureClass.Fields
            Dim targetFields As IFields = Nothing
            Dim enumFieldError As IEnumFieldError = Nothing

            ' Set the required properties for the IFieldChecker interface.
            fieldChecker.InputWorkspace = sourceWorkspace
            fieldChecker.ValidateWorkspace = targetWorkspace

            ' Validate the fields and check for errors.
            fieldChecker.Validate(sourceFields, enumFieldError, targetFields)
            If Not enumFieldError Is Nothing Then
                ' Handle the errors in a way appropriate to your application.
                Console.WriteLine("Errors were encountered during field validation.")
            End If

            ' Find the shape field.
            Dim shapeFieldName As String = sourceFeatureClass.ShapeFieldName
            Dim shapeFieldIndex As Integer = sourceFeatureClass.FindField(shapeFieldName)
            Dim shapeField As IField = sourceFields.Field(shapeFieldIndex)

            ' Get the geometry definition from the shape field and clone it.
            Dim geometryDef As IGeometryDef = shapeField.GeometryDef
            Dim geometryDefClone As IClone = CType(geometryDef, IClone)
            Dim targetGeometryDefClone As IClone = geometryDefClone.Clone()
            Dim targetGeometryDef As IGeometryDef = CType(targetGeometryDefClone, IGeometryDef)

            ' Cast the IGeometryDef to the IGeometryDefEdit interface.
            Dim targetGeometryDefEdit As IGeometryDefEdit = CType(targetGeometryDef, IGeometryDefEdit)

            ' Set the IGeometryDefEdit properties.
            targetGeometryDefEdit.GridCount_2 = 1
            targetGeometryDefEdit.GridSize_2(0) = 0.75

            ' Create a query filter to only select cities with a province (PROV) value of 'NS.'
            'Dim queryFilter As IQueryFilter = New QueryFilterClass()
            'queryFilter.WhereClause = "PROV = 'NS'"
            'queryFilter.SubFields = "Shape, NAME, TERM, Pop1996"

            ' Create the converter and run the conversion.
            Dim featureDataConverter As IFeatureDataConverter = New FeatureDataConverter
            Dim enumInvalidObject As IEnumInvalidObject = featureDataConverter.ConvertFeatureClass(sourceFeatureClassName, Nothing, Nothing, targetFeatureClassName, targetGeometryDef, targetFields, "", 1000, 0)

            ' Check for errors.
            Dim invalidObjectInfo As IInvalidObjectInfo = Nothing
            enumInvalidObject.Reset()

            Do While Not (invalidObjectInfo) Is Nothing
                ' Handle the errors in a way appropriate to the application.
                Console.WriteLine("Errors occurred for the following feature: {0}", invalidObjectInfo.InvalidObjectID)
                invalidObjectInfo = enumInvalidObject.Next()
            Loop
            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_ConvertShapeFileToGDB Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        End Try

    End Function

    'Converts a shapefile to a feature class
    'Copied from ESRI Help: Converting simple data 
    Public Function BA_ConvertGDBToShapefile(ByVal sourceGDB As String, ByVal sourceFile As String, ByVal targetFolder As String, _
                                             ByVal targetFile As String) As BA_ReturnCode
        Try

            ' Validate source path
            Dim pType As WorkspaceType = BA_GetWorkspaceTypeFromPath(sourceGDB)
            If pType <> WorkspaceType.Geodatabase Then
                MessageBox.Show("Source folder must be a geodatabase", "Source folder cannot be a geodatabase", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return BA_ReturnCode.ReadError
            End If
            ' Validate target path
            pType = BA_GetWorkspaceTypeFromPath(targetFolder)
            If pType <> WorkspaceType.Raster Then
                MessageBox.Show("Target folder cannot be a geodatabase", "Target folder cannot be a geodatabase", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return BA_ReturnCode.ReadError
            End If
            'Be sure that sourceFile doesn't have .shp extension
            sourceFile = BA_StandardizeShapefileName(sourceFile, False)
            'Be sure that targetFile does have .shp extension
            targetFile = BA_StandardizeShapefileName(targetFile, True)

            ' Create a name object for the source workspace and open it.
            Dim sourceWorkspaceName As IWorkspaceName = New WorkspaceName With _
                                                        { _
                                                        .WorkspaceFactoryProgID = "esriDataSourcesFile.FileGDBWorkspaceFactory", _
                                                        .PathName = sourceGDB _
                                                        }
            Dim sourceWorkspaceIName As IName = CType(sourceWorkspaceName, IName)
            Dim sourceWorkspace As IWorkspace = CType(sourceWorkspaceIName.Open(), IWorkspace)

            ' Create a name object for the target workspace and open it.
            Dim targetWorkspaceName As IWorkspaceName = New WorkspaceName With _
                                                        { _
                                                        .WorkspaceFactoryProgID = "esriDataSourcesGDB.ShapefileWorkspaceFactory", _
                                                        .PathName = targetFolder _
                                                        }
            Dim targetWorkspaceIName As IName = CType(targetWorkspaceName, IName)
            Dim targetWorkspace As IWorkspace = CType(targetWorkspaceIName.Open(), IWorkspace)

            ' Create a name object for the source dataset.
            Dim sourceFeatureClassName As IFeatureClassName = New FeatureClassName
            Dim sourceDatasetName As IDatasetName = CType(sourceFeatureClassName, IDatasetName)
            sourceDatasetName.Name = sourceFile
            sourceDatasetName.WorkspaceName = sourceWorkspaceName

            ' Create a name object for the target dataset.
            Dim targetFeatureClassName As IFeatureClassName = New FeatureClassName
            Dim targetDatasetName As IDatasetName = CType(targetFeatureClassName, IDatasetName)
            targetDatasetName.Name = targetFile
            targetDatasetName.WorkspaceName = targetWorkspaceName

            ' Open source feature class to get field definitions.
            Dim sourceName As IName = CType(sourceFeatureClassName, IName)
            Dim sourceFeatureClass As IFeatureClass = CType(sourceName.Open(), IFeatureClass)

            ' Create the objects and references necessary for field validation.
            Dim fieldChecker As IFieldChecker = New FieldChecker
            Dim sourceFields As IFields = sourceFeatureClass.Fields
            Dim targetFields As IFields = Nothing
            Dim enumFieldError As IEnumFieldError = Nothing

            ' Set the required properties for the IFieldChecker interface.
            fieldChecker.InputWorkspace = sourceWorkspace
            fieldChecker.ValidateWorkspace = targetWorkspace

            ' Validate the fields and check for errors.
            fieldChecker.Validate(sourceFields, enumFieldError, targetFields)
            If Not enumFieldError Is Nothing Then
                ' Handle the errors in a way appropriate to your application.
                Console.WriteLine("Errors were encountered during field validation.")
            End If

            ' Find the shape field.
            Dim shapeFieldName As String = sourceFeatureClass.ShapeFieldName
            Dim shapeFieldIndex As Integer = sourceFeatureClass.FindField(shapeFieldName)
            Dim shapeField As IField = sourceFields.Field(shapeFieldIndex)

            ' Get the geometry definition from the shape field and clone it.
            Dim geometryDef As IGeometryDef = shapeField.GeometryDef
            Dim geometryDefClone As IClone = CType(geometryDef, IClone)
            Dim targetGeometryDefClone As IClone = geometryDefClone.Clone()
            Dim targetGeometryDef As IGeometryDef = CType(targetGeometryDefClone, IGeometryDef)

            ' Cast the IGeometryDef to the IGeometryDefEdit interface.
            Dim targetGeometryDefEdit As IGeometryDefEdit = CType(targetGeometryDef, IGeometryDefEdit)

            ' Set the IGeometryDefEdit properties.
            targetGeometryDefEdit.GridCount_2 = 1
            targetGeometryDefEdit.GridSize_2(0) = 0.75

            ' Create the converter and run the conversion.
            Dim featureDataConverter As IFeatureDataConverter = New FeatureDataConverter
            Dim enumInvalidObject As IEnumInvalidObject = featureDataConverter.ConvertFeatureClass(sourceFeatureClassName, Nothing, Nothing, targetFeatureClassName, targetGeometryDef, targetFields, "", 1000, 0)

            ' Check for errors.
            Dim invalidObjectInfo As IInvalidObjectInfo = Nothing
            enumInvalidObject.Reset()

            Do While Not (invalidObjectInfo) Is Nothing
                ' Handle the errors in a way appropriate to the application.
                Console.WriteLine("Errors occurred for the following feature: {0}", invalidObjectInfo.InvalidObjectID)
                invalidObjectInfo = enumInvalidObject.Next()
            Loop
            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_ConvertGDBToShapefile Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        End Try

    End Function

    Public Function BA_GetShapeAreaGDB(ByVal shapefile_pathname As String) As Double
        Dim filepath As String = "Return"
        Dim FileName As String
        Dim shapearea As Double

        If Len(shapefile_pathname) = 0 Then 'not a valide input
            Return -1
        End If

        FileName = BA_GetBareName(shapefile_pathname, filepath)
        Dim pFeatureClass As IFeatureClass
        Dim pCursor As IFeatureCursor
        Dim pFeature As IFeature
        Dim pQueryFilter As IQueryFilter = Nothing
        Dim pGeometry As IGeometry
        Dim pArea As IArea

        Try
            'Get shape FeatureClass
            pFeatureClass = BA_OpenFeatureClassFromGDB(filepath, FileName)

            'Extract Area
            pCursor = pFeatureClass.Search(pQueryFilter, False)
            pFeature = pCursor.NextFeature

            shapearea = 0
            Do While Not pFeature Is Nothing
                pGeometry = pFeature.Shape
                pArea = pGeometry
                pFeature = pCursor.NextFeature
                shapearea = shapearea + pArea.Area
            Loop

            Return shapearea
        Catch ex As Exception
            Debug.Print("BA_GetShapeAreaGDB Exception: " & ex.Message)
            MsgBox("Unable to get the area of " & shapefile_pathname & "!")
            Return 0
        Finally
            pFeatureClass = Nothing
            pFeature = Nothing
            pCursor = Nothing
            pFeature = Nothing
            pGeometry = Nothing
            pArea = Nothing
        End Try
    End Function

    Public Function BA_GetDemStatsGDB(ByVal aoiPath As String) As IRasterStatistics
        Dim pRasterDataset As IRasterDataset = Nothing
        Dim pAOIRaster As IGeoDataset = Nothing
        Dim pTempRaster As IGeoDataset = Nothing
        Dim pRasterBandCollection As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand
        Dim pRStats As IRasterStatistics
        Try
            'get the location and name of the dem.
            Dim DemFilePath As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Surfaces)
            Dim DemFileName As String = BA_EnumDescription(MapsFileName.filled_dem_gdb)
            pRasterDataset = BA_OpenRasterFromGDB(DemFilePath, DemFileName)
            'see if the DEM was created with a buffer
            'BASIN dem wasn't bufferred
            'AOI dem could be bufferred. If bufferred, there will be an aoib_v.shp in the AOI folder
            Dim bufferedVector As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Aoi, True)
            bufferedVector = bufferedVector & BA_EnumDescription(AOIClipFile.BufferedAOIExtentCoverage)
            If BA_File_Exists(bufferedVector, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                'use the unbufferred aoi to mask out the dem
                'Use the AOI extent for analysis
                'Open AOI Polygon to set the analysis mask
                pAOIRaster = BA_OpenRasterFromGDB(BA_GeodatabasePath(aoiPath, GeodatabaseNames.Aoi), BA_GetBareName(BA_EnumDescription(PublicPath.AoiGrid)))
                Dim pExtractOp As IExtractionOp2 = New RasterExtractionOp
                pTempRaster = pExtractOp.Raster(pRasterDataset, pAOIRaster)
                pRasterBandCollection = CType(pTempRaster, IRasterBandCollection)   'Explict cast
                pExtractOp = Nothing
            Else 'DEM is not bufferred
                pRasterBandCollection = CType(pRasterDataset, IRasterBandCollection)
            End If
            pRasterBand = pRasterBandCollection.Item(0)
            pRStats = pRasterBand.Statistics
            Return pRStats
        Catch ex As Exception
            Debug.Print("BA_GetDemStatsGDB Exception: " & ex.Message)
            Return Nothing
        Finally
            pRasterDataset = Nothing
            pAOIRaster = Nothing
            pTempRaster = Nothing
            pRasterBandCollection = Nothing
            pRasterBand = Nothing
            pRStats = Nothing
        End Try

    End Function

    Public Function BA_ListTablesInGDB(ByVal gdbPath As String) As IList(Of String)
        Dim pWSF As IWorkspaceFactory = New FileGDBWorkspaceFactory()
        Dim pWorkspace As IWorkspace = Nothing
        Dim pEnumDSName As IEnumDatasetName = Nothing
        Dim pDSName As IDatasetName = Nothing
        Dim returnList As New List(Of String)

        Try
            ' Strip trailing "\" if exists
            If gdbPath(Len(gdbPath) - 1) = "\" Then
                gdbPath = gdbPath.Remove(Len(gdbPath) - 1, 1)
            End If
            pWorkspace = pWSF.OpenFromFile(gdbPath, 0)
            'list tables
            pEnumDSName = pWorkspace.DatasetNames(esriDatasetType.esriDTTable)
            pDSName = pEnumDSName.Next

            While Not pDSName Is Nothing
                Dim nextItem As String = pDSName.Name
                Dim idxSuffix As Integer = pDSName.Name.IndexOf(BA_PARAM_TABLE_SUFFIX)
                If idxSuffix > 0 Then
                    nextItem = nextItem.Substring(0, idxSuffix)
                End If
                returnList.Add(nextItem)
                pDSName = pEnumDSName.Next
            End While

            Return returnList
        Catch ex As Exception
            MessageBox.Show("BA_ListTablesInGDB Exception: " + ex.Message)
            Return Nothing
        Finally
            pWSF = Nothing
            pWorkspace = Nothing
            pEnumDSName = Nothing
            pDSName = Nothing
        End Try
    End Function

    'Set selected cells to null
    'Uses Raster Calculator SetNull syntax
    Public Function BA_SetNullSelectedCellsGDB(ByVal inputFolder As String, ByVal inputFile As String,
                                              ByVal outputFolder As String, ByVal outputFile As String, _
                                              ByVal maskFolder As String, ByVal maskFile As String, _
                                              ByVal whereClause As String) As BA_ReturnCode
        Dim mapAlgebraOp As ESRI.ArcGIS.SpatialAnalyst.IMapAlgebraOp = New ESRI.ArcGIS.SpatialAnalyst.RasterMapAlgebraOp
        Dim inputGeodataset As IGeoDataset = Nothing
        Dim pRasterDataset As IRasterDataset3 = Nothing
        Dim isNullGeodataset As IGeoDataset = Nothing
        Dim outputGeodataset As IGeoDataset = Nothing
        Dim maskFeatureClass As IFeatureClass = Nothing
        Dim maskGeodataset As IGeoDataset = Nothing
        Dim workspaceFactory As IWorkspaceFactory = New FileGDBWorkspaceFactory()
        Dim workspace As IWorkspace = Nothing
        Dim pEnv As IRasterAnalysisEnvironment = Nothing
        Dim retVal As BA_ReturnCode = BA_ReturnCode.UnknownError

        Try
            inputGeodataset = BA_OpenRasterFromGDB(inputFolder, inputFile)
            If inputGeodataset IsNot Nothing Then
                pRasterDataset = CType(inputGeodataset, IRasterDataset3) ' Explicit cast
                'Set environment
                If Not String.IsNullOrEmpty(maskFolder) AndAlso Not String.IsNullOrEmpty(maskFolder) Then
                    pEnv = CType(mapAlgebraOp, IRasterAnalysisEnvironment)  ' Explicit cast
                    maskFeatureClass = BA_OpenFeatureClassFromGDB(maskFolder, maskFile)
                    maskGeodataset = CType(maskFeatureClass, IGeoDataset)
                    pEnv.Mask = maskGeodataset
                    ' Set the analysis extent to match the mask
                    Dim extentProvider As Object = CType(maskGeodataset.Extent, Object)
                    pEnv.SetExtent(esriRasterEnvSettingEnum.esriRasterEnvValue, extentProvider)
                    workspace = workspaceFactory.OpenFromFile(outputFolder, 0)
                    pEnv.OutWorkspace = workspace
                End If
                mapAlgebraOp.BindRaster(pRasterDataset, "noData1")
                'sample where clause:  == 3
                isNullGeodataset = mapAlgebraOp.Execute("SetNull([noData1]" & whereClause & ",[noData1])")
                If isNullGeodataset IsNot Nothing Then
                    BA_SaveRasterDatasetGDB(isNullGeodataset, outputFolder, BA_RASTER_FORMAT, outputFile)
                    retVal = BA_ReturnCode.Success
                End If
            End If
            Return retVal
        Catch ex As Exception
            MsgBox("BA_ReplaceNoDataCellsGDB Exception: " & ex.Message)
            Return retVal
        Finally
            mapAlgebraOp.UnbindRaster("noData1")
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pEnv)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(maskGeodataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(maskFeatureClass)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(outputGeodataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(isNullGeodataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(inputGeodataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(workspace)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(mapAlgebraOp)
        End Try

    End Function

    'Assumes there is only one row and one value to return
    Public Function BA_QueryStringFieldFromVector(ByVal filePath As String, ByVal fileName As String, _
                                                  ByVal fieldName As String) As String
        Dim fClass As IFeatureClass = Nothing
        Dim pFeature As IFeature = Nothing
        Dim pCursor As IFeatureCursor = Nothing
        Dim pRow As IRow = Nothing
        Try
            fClass = BA_OpenFeatureClassFromGDB(filePath, fileName)
            If fClass IsNot Nothing Then
                Dim idxField As Integer = fClass.FindField(fieldName)
                If idxField < 0 Then
                    Return Nothing
                Else
                    pCursor = fClass.Search(Nothing, False)
                    pFeature = pCursor.NextFeature
                    Return Convert.ToString(pFeature.Value(idxField))
                End If
            End If
            Return Nothing
        Catch ex As Exception
            Debug.Print("BA_QueryStringFieldFromVector Exception: " & ex.Message)
            Return Nothing
        Finally
            fClass = Nothing
            pFeature = Nothing
            pCursor = Nothing
            pRow = Nothing
        End Try
    End Function

    'Return the unique values from a GDB raster in an IEnumerator
    Public Function BA_QueryUniqueValuesFromRasterGDB(ByVal folderName As String, ByVal fileName As String, ByVal queryField As String) As IEnumerator
        Dim pGeodataset As IGeoDataset = Nothing
        Dim pRasterBandCollection As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pTable As ITable = Nothing
        Dim valuesCursor As ICursor = Nothing
        Dim pDataStatistics As IDataStatistics = New DataStatistics
        Try
            pGeodataset = BA_OpenRasterFromGDB(folderName, fileName)
            If pGeodataset IsNot Nothing Then
                pRasterBandCollection = CType(pGeodataset, IRasterBandCollection)
                pRasterBand = pRasterBandCollection.Item(0)
                pTable = pRasterBand.AttributeTable
                If pTable IsNot Nothing Then
                    valuesCursor = pTable.Search(Nothing, False)
                    'initialize properties for the dataStatistics interface
                    pDataStatistics.Field = queryField
                    pDataStatistics.Cursor = valuesCursor
                    Return pDataStatistics.UniqueValues
                End If
            End If
            Return Nothing
        Catch ex As Exception
            Debug.Print("BA_QueryUniqueValuesFromRasterGDB Exception: " & ex.Message)
            Return Nothing
        Finally
            pGeodataset = Nothing
            pRasterBandCollection = Nothing
            pRasterBand = Nothing
            pTable = Nothing
            valuesCursor = Nothing
            pDataStatistics = Nothing
        End Try

    End Function

    Public Function BA_ComputeStatsRasterDatasetGDB(ByRef strPath As String, ByVal sName As String) As BA_ReturnCode
        Dim return_value As BA_ReturnCode = BA_ReturnCode.UnknownError
        Dim pRasterDataset As IRasterDataset = New RasterDataset
        Dim pRasterBand As IRasterBand = Nothing
        Dim pRasterBandCollection As IRasterBandCollection = Nothing
        Dim pInputRaster As IGeoDataset2 = Nothing

        Try
            pInputRaster = BA_OpenRasterFromGDB(strPath, sName)
            pRasterDataset = CType(pInputRaster, IRasterDataset2) ' Explicit cast
            pRasterBandCollection = CType(pInputRaster, IRasterBandCollection)
            pRasterBand = pRasterBandCollection.Item(0)
            pRasterBand.ComputeStatsAndHist()
            return_value = BA_ReturnCode.Success
        Catch ex As Exception
            MessageBox.Show("BA_ComputeStatsRasterDatasetGDB Exception: " + ex.Message)
            return_value = BA_ReturnCode.UnknownError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBand)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBandCollection)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterDataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pInputRaster)
        End Try
        Return return_value
    End Function

    'Returns the ILinearUnit of a raster in a file gdb if the spatial reference is projected
    Public Function BA_GetLinearUnitOfProjectedRaster(ByVal filePath As String, ByVal fileName As String) As ILinearUnit
        Dim pGeoDataset As IGeoDataset
        Dim spatialRef As ISpatialReference
        Dim projCoord As IProjectedCoordinateSystem
        Try
            pGeoDataset = BA_OpenRasterFromGDB(filePath, fileName)
            spatialRef = pGeoDataset.SpatialReference
            projCoord = TryCast(spatialRef, IProjectedCoordinateSystem)
            If projCoord IsNot Nothing Then
                Return projCoord.CoordinateUnit
            End If
            Return Nothing
        Catch ex As Exception
            Debug.Print("BA_GetLinearUnitOfProjectedRaster Exception: " & ex.Message)
            Return Nothing
        Finally
            pGeoDataset = Nothing
            spatialRef = Nothing
            projCoord = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    'The name of the grid_code field may change depending on the ArcGIS version used and if the
    'attribute table is in a FGDB. This function checks for both field names and returns the field name
    'that is found. This function can work with both folders and geodatabases
    Function BA_FindGridCodeFieldNameForFC(ByVal inputFolder As String, ByVal inputFile As String) As String
        Dim fc As IFeatureClass = Nothing
        Dim gridField As String = Nothing

        Try
            Dim inputPath As String = inputFolder & "\" & inputFile
            Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(inputPath)
            If wType = WorkspaceType.Geodatabase Then
                fc = BA_OpenFeatureClassFromGDB(inputFolder, inputFile)
            Else
                fc = BA_OpenFeatureClassFromFile(inputFolder, inputFile)
            End If
            If fc IsNot Nothing Then
                Dim idxFind As Long = fc.FindField(BA_FIELD_GRIDCODE_GDB)
                If idxFind > 0 Then
                    gridField = BA_FIELD_GRIDCODE_GDB
                Else
                    idxFind = fc.FindField(BA_FIELD_GRIDCODE)
                    If idxFind > 0 Then
                        gridField = BA_FIELD_GRIDCODE
                    End If
                End If
            End If
            Return gridField
        Catch ex As Exception
            Debug.Print("BA_FindGridCodeFieldName" & ex.Message)
            Return gridField
        Finally

        End Try

    End Function

End Module
