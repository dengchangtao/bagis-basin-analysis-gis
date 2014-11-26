Option Strict Off
Option Explicit On

Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.GeoAnalyst
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.GeoDatabaseUI
Imports ESRI.ArcGIS.Geoprocessing
Imports ESRI.ArcGIS.DataSourcesFile
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Geometry
Imports System.Windows.Forms
Imports System.IO
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.DataSourcesGDB


Public Module PublicModule

    'Extract file name and parent folder from a full file path
    Public Function BA_GetBareName(ByRef path_string As String, Optional ByRef parent_path As String = Nothing) As String
        Dim temp_string As String
        Dim bareName As String
        Dim searchstart, searchpos, lastpos As Integer
        Dim nbslash As Integer
        Dim return_parent_path As Boolean

        If parent_path Is Nothing Then
            return_parent_path = False
        Else
            return_parent_path = True
        End If

        temp_string = Trim(path_string)
        If Len(temp_string) = 0 Then
            bareName = ""
            Return bareName
        End If

        'remove the last character if it's a backslash
        If Right(temp_string, 1) = "\" Then temp_string = Left(temp_string, Len(temp_string) - 1)

        'find the last backslash
        searchstart = 1
        lastpos = 0
        nbslash = 0 'number of backslashes found in the path
        Do
            lastpos = searchpos
            searchpos = InStr(searchstart, temp_string, "\", CompareMethod.Text)
            searchstart = searchpos + 1
            nbslash = nbslash + 1
        Loop Until searchpos = 0

        If nbslash = 0 Then 'no backslash found in the path, i.e., top folder
            bareName = temp_string
            If return_parent_path Then parent_path = ""
        Else
            bareName = Right(temp_string, Len(temp_string) - lastpos)
            If return_parent_path Then parent_path = Left(temp_string, lastpos)
        End If

        Return bareName
    End Function

    'get the last folder (or filename) from a path string
    'extension is in form of string (Shapefile) or (Raster)
    'this string is appended to the settings file when it is saved
    Public Function BA_GetBareNameAndExtension(ByVal path_string As String, Optional ByRef Parent_Path As String = Nothing, Optional ByRef strType As String = Nothing) As String
        Dim temp_string As String
        Dim BareName, newBareName As String
        Dim searchstart, searchpos, lastpos As Integer
        Dim nbslash, Length As Integer
        Dim Return_ParentPath, Return_Type As Boolean

        If Parent_Path Is Nothing Then
            Return_ParentPath = False
        Else
            Return_ParentPath = True
        End If

        If strType Is Nothing Then
            Return_Type = False
        Else
            Return_Type = True
        End If

        temp_string = Trim(path_string)
        If Len(temp_string) = 0 Then
            BareName = ""
            Return Nothing
        End If

        'remove the last character if it's a backslash
        If Right(temp_string, 1) = "\" Then temp_string = Left(temp_string, Len(temp_string) - 1)

        'find the last backslash
        searchstart = 1
        lastpos = 0
        nbslash = 0 'number of backslashes found in the path
        Do
            lastpos = searchpos
            searchpos = InStr(searchstart, temp_string, "\", CompareMethod.Text)
            searchstart = searchpos + 1
            nbslash = nbslash + 1
        Loop Until searchpos = 0

        If nbslash = 0 Then 'no backslash found in the path, i.e., top folder
            BareName = temp_string
            If Return_ParentPath Then Parent_Path = ""
        Else
            BareName = Right(temp_string, Len(temp_string) - lastpos)
            If Return_ParentPath Then Parent_Path = Left(temp_string, lastpos)
        End If

        newBareName = BareName

        'find the right parenthesis
        searchstart = 1
        lastpos = 0
        nbslash = 0 'number of right parenthesis found in the barename
        Do
            lastpos = searchpos
            searchpos = InStr(searchstart, newBareName, "(", CompareMethod.Text)
            searchstart = searchpos + 1
            nbslash = nbslash + 1
        Loop Until searchpos = 0

        If Return_Type = True Then
            strType = Right(newBareName, Len(newBareName) - lastpos + 1)
            Length = Len(newBareName) - Len(strType)
            BareName = Left(newBareName, Length - 1)
            Return BareName
        Else
            Return BareName
        End If
    End Function

    'Generates the path when provided with the folder base and PublicPath enum value
    Public Function BA_GetPath(ByVal FolderBase As String, ByVal pathKey As PublicPath) As String
        'please check the length of returning string for errors, if 0, then error occurred
        Dim path_string As String

        If String.IsNullOrEmpty(FolderBase) Then
            Return ""
        End If

        'remove the last character if it's a backslash
        If Right(FolderBase, 1) = "\" Then FolderBase = Left(FolderBase, Len(FolderBase) - 1)

        path_string = FolderBase & BA_EnumDescription(pathKey)

        Return path_string
    End Function

    'Uses IDEUtilities to check if a file exists
    Public Function BA_File_ExistsIDEUtil(ByVal filepath_name As String) As Boolean
        If String.IsNullOrEmpty(filepath_name) Then
            Return False
        End If

        Dim pGPValue As IGPValue = New DEWorkspace
        Dim pDEUtil As IDEUtilities = New DEUtilities
        Try
            pGPValue.SetAsText(filepath_name)
            Return pDEUtil.Exists(pGPValue)
        Catch ex As Exception
            MessageBox.Show("BA_File_Exists Exception: " + ex.Message)
            Return False
        Finally
            pGPValue.Empty()
            pDEUtil.ReleaseInternals()
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGPValue)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDEUtil)
        End Try
    End Function

    'Uses IRasterWorkspace to check if a file exists
    Public Function BA_File_ExistsRaster(ByVal filePath As String, ByVal fileName As String) As Boolean
        If String.IsNullOrEmpty(filePath) Then
            Return False
        End If
        Dim pWSFactory As IWorkspaceFactory = New RasterWorkspaceFactory
        Dim pRWorkSpace As IRasterWorkspace = Nothing
        Dim pRLayer As IRasterLayer = New RasterLayer
        Try
            pRWorkSpace = CType(pWSFactory.OpenFromFile(filePath, 0), IRasterWorkspace)
            Dim pRDS As IRasterDataset = pRWorkSpace.OpenRasterDataset(fileName)
            pRLayer.CreateFromDataset(pRDS)
            Return True
        Catch ex As Exception
            ' An exception was thrown while trying to open the dataset, return false
            Return False
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRLayer)

        End Try
    End Function

    ' Use Windows IO to check for existence of a non-ArcGIS file
    Public Function BA_File_ExistsWindowsIO(ByVal strFilePath As String) As Boolean
        Dim fileInfo_check As FileInfo = New System.IO.FileInfo(strFilePath)
        Return fileInfo_check.Exists
    End Function

    ' Use Windows IO to check for existence of a folder
    Public Function BA_Folder_ExistsWindowsIO(ByVal strFilePath As String) As Boolean
        Dim dirInfo_check As DirectoryInfo = New System.IO.DirectoryInfo(strFilePath)
        Return dirInfo_check.Exists
    End Function


    'enable Spatial Analyst Extension if it's not already enabled
    'note: no access to IApplication = no nunit test case
    Public Function BA_Enable_SAExtension(ByVal application As ESRI.ArcGIS.Framework.IApplication) As ESRI.ArcGIS.esriSystem.esriExtensionState
        Dim response As ESRI.ArcGIS.esriSystem.esriExtensionState
        Dim pExtConfig As ESRI.ArcGIS.esriSystem.IExtensionConfig
        Dim pExt As ESRI.ArcGIS.esriSystem.IExtension
        pExt = application.FindExtensionByName("Spatial Analyst")
        pExtConfig = pExt
        response = ESRI.ArcGIS.esriSystem.esriExtensionState.esriESEnabled
        If pExtConfig.State = ESRI.ArcGIS.esriSystem.esriExtensionState.esriESDisabled Then
            pExtConfig.State = ESRI.ArcGIS.esriSystem.esriExtensionState.esriESEnabled
            response = ESRI.ArcGIS.esriSystem.esriExtensionState.esriESEnabled
        ElseIf pExtConfig.State = ESRI.ArcGIS.esriSystem.esriExtensionState.esriESUnavailable Then
            response = ESRI.ArcGIS.esriSystem.esriExtensionState.esriESUnavailable
        End If
        Return response
    End Function

    ' Opens and returns an IFeatureClass object
    Public Function BA_OpenFeatureClassFromFile(ByRef filepath As String, ByRef FileName As String) As IFeatureClass
        Dim directoryInfo_check As DirectoryInfo = New DirectoryInfo(filepath)
        If directoryInfo_check.Exists Then
            'We have a valid directory, proceed
            Dim strFileCheck As String = filepath + "\" + FileName + ".shp"
            Dim fileInfo_check As FileInfo = New System.IO.FileInfo(strFileCheck)
            If fileInfo_check.Exists Then

                'We have a valid shapefile, proceed
                Dim pWSFactory As IWorkspaceFactory
                Dim pFeatWSpace As IFeatureWorkspace
                Try
                    pWSFactory = New ShapefileWorkspaceFactory
                    pFeatWSpace = pWSFactory.OpenFromFile(filepath, 0)
                    Return pFeatWSpace.OpenFeatureClass(FileName)

                Catch ex As Exception
                    MessageBox.Show("Exception: " + ex.Message)
                    Return Nothing
                Finally
                    pWSFactory = Nothing
                    pFeatWSpace = Nothing
                    GC.Collect()
                    GC.WaitForPendingFinalizers()
                End Try
            Else
                'Not valid shapefile
                Return Nothing
            End If
        Else
            'Not a valid directory
            Return Nothing
        End If
    End Function

    'Opens and returns an ITable object
    Public Function BA_OpenTableFromFile(ByRef FilePath As String, ByRef FileName As String) As ITable
        Dim directoryInfo_check As DirectoryInfo = New DirectoryInfo(FilePath)
        If directoryInfo_check.Exists Then
            'We have a valid directory, proceed
            Dim strFileCheck As String = FilePath + "\" + FileName
            If String.Compare(".dbf", Right(strFileCheck, 4)) <> 0 Then
                strFileCheck = strFileCheck & ".dbf"
            End If
            Dim fileInfo_check As FileInfo = New System.IO.FileInfo(strFileCheck)
            If fileInfo_check.Exists Then

                'We have a valid table, proceed
                Dim pWSFactory As IWorkspaceFactory
                Dim pFeatWSpace As IFeatureWorkspace
                Try
                    pWSFactory = New ShapefileWorkspaceFactory
                    pFeatWSpace = pWSFactory.OpenFromFile(FilePath, 0)
                    Return pFeatWSpace.OpenTable(FileName)

                Catch ex As Exception
                    MessageBox.Show("Exception: " + ex.Message)
                    Return Nothing
                Finally
                    pWSFactory = Nothing
                    pFeatWSpace = Nothing
                End Try
            Else
                'Not valid shapefile
                Return Nothing
            End If
        Else
            'Not a valid directory
            Return Nothing
        End If
    End Function

    'Checks to make sure all values are integer and converts to a integer raster
    Public Sub BA_Feature2RasterInteger(ByVal InputFeatureClass As IFeatureClass, ByVal filepath As String, _
                                        ByVal FileName As String, ByVal Cellsize As Object, _
                                        ByVal valueField As String, ByVal snapRasterPath As String)

        If FileName.Length > BA_GRID_NAME_MAX_LENGTH Then
            MsgBox("Alert: " & FileName)
            Throw New Exception("Output raster name cannot exceed " & CStr(BA_GRID_NAME_MAX_LENGTH) & " characters")
        End If

        Dim pFDesc As IFeatureClassDescriptor = New FeatureClassDescriptor
        Dim pQFilter As IQueryFilter = New QueryFilter
        Try
            pFDesc.Create(InputFeatureClass, pQFilter, valueField)
            Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(filepath)
            If workspaceType = workspaceType.Raster Then
                Feature2Raster(pFDesc, filepath, FileName, Cellsize, valueField, snapRasterPath)
            ElseIf workspaceType = workspaceType.Geodatabase Then
                BA_Feature2RasterGDB(pFDesc, filepath, FileName, Cellsize, valueField, snapRasterPath)
            End If
        Catch ex As Exception
            MessageBox.Show("Exception: " + ex.Message)
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFDesc)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pQFilter)
        End Try

    End Sub

    'Converts double values from vector to integer and then creates an integer raster
    Public Sub BA_Feature2RasterDouble(ByVal InputFeatureClass As IFeatureClass, ByVal filepath As String, _
                                       ByVal FileName As String, ByVal Cellsize As Object, _
                                       ByVal valueField As String, ByVal snapRasterPath As String)

        If FileName.Length > BA_GRID_NAME_MAX_LENGTH Then
            Throw New Exception("Output raster name cannot exceed " & CStr(BA_GRID_NAME_MAX_LENGTH) & " characters")
        End If

        Dim pIndex As Short = InputFeatureClass.FindField(valueField)
        If pIndex < 1 Then
            Throw New Exception("The selected field does not exist on input feature class.")
        End If

        Dim tempFieldName As String = "ROUNDVAL"
        Dim pField As IField = New Field()
        Dim pFieldEdit As IFieldEdit2 = CType(pField, IFieldEdit2)
        Dim pCursor As IFeatureCursor = Nothing
        Dim pFeature As IFeature = Nothing
        Dim pQFilter As IQueryFilter = New QueryFilter
        Dim pFDesc As IFeatureClassDescriptor = New FeatureClassDescriptor

        Try
            pFieldEdit.Name_2 = tempFieldName
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeInteger
            InputFeatureClass.AddField(pFieldEdit)
            Dim pRoundIndex As Short = InputFeatureClass.FindField(tempFieldName)

            ' Open updatable cursor
            pCursor = InputFeatureClass.Update(Nothing, False)
            pFeature = pCursor.NextFeature
            While Not pFeature Is Nothing
                Dim objValue As Object = pFeature.Value(pIndex)
                If IsNumeric(objValue) Then
                    pFeature.Value(pRoundIndex) = CInt(objValue)
                Else
                    Throw New Exception("Value is not numeric and could not be converted to a whole number.")
                End If
                pCursor.UpdateFeature(pFeature)
                pFeature = pCursor.NextFeature()
            End While
            pFDesc.Create(InputFeatureClass, pQFilter, tempFieldName)
            Feature2Raster(pFDesc, filepath, FileName, Cellsize, tempFieldName, snapRasterPath)
        Catch ex As Exception
            MessageBox.Show("BA_Feature2RasterDouble Exception: " + ex.Message)
        Finally
            If InputFeatureClass.FindField(tempFieldName) > 0 Then
                InputFeatureClass.DeleteField(pFieldEdit)
            End If
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeature)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pQFilter)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFDesc)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pField)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFieldEdit)
        End Try

    End Sub

    Private Sub Feature2Raster(ByVal featClassDescr As IFeatureClassDescriptor, ByVal filepath As String, _
                               ByVal FileName As String, ByVal Cellsize As Object, _
                               ByVal valueField As String, ByVal snapRasterPath As String)
        Dim pWS As IWorkspace = Nothing
        Dim pWSF As IWorkspaceFactory
        Dim pConversionOp As IConversionOp = New RasterConversionOp
        Dim pEnv As IRasterAnalysisEnvironment = pConversionOp
        Dim pRDS As IRasterDataset = Nothing
        Dim snapGDS As IGeoDataset = Nothing
        Dim envelope As IEnvelope = Nothing

        Try
            'Commenting out check for integer values since we may also convert text values; Field values should be checked
            'before this is called from either BA_Feature2RasterInteger or BA_Feature2RasterDouble
            'If BA_IsIntegerField(featClassDescr, valueField) Then
            pWSF = New RasterWorkspaceFactory
            pWS = pWSF.OpenFromFile(filepath, 0)
            pEnv.SetCellSize(esriRasterEnvSettingEnum.esriRasterEnvValue, Cellsize)
            Dim snapPath As String = "PleaseReturn"
            Dim snapName As String = BA_GetBareName(snapRasterPath, snapPath)
            snapGDS = BA_OpenRasterFromFile(snapPath, snapName)
            envelope = snapGDS.Extent
            Dim object_Envelope As System.Object = CType(envelope, System.Object) ' Explicit Cast
            pEnv.SetExtent(esriRasterEnvSettingEnum.esriRasterEnvValue, object_Envelope, snapRasterPath)
            pRDS = pConversionOp.ToRasterDataset(featClassDescr, "GRID", pWS, FileName)
            'Else
            'Throw New Exception("Invalid values found in value field. Values must be whole numbers.")
            'End If
        Catch ex As Exception
            MessageBox.Show("Feature2Raster Exception: " + ex.Message)
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pConversionOp)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(envelope)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pEnv)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRDS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(snapGDS)
        End Try
    End Sub

    'Delete a raster file using IRasterWorkspace object
    Public Function BA_Remove_Raster(ByRef RasterPath As String, ByRef RasterName As String) As Short
        Dim pDataset As IDataset = Nothing
        Dim pRWS As IRasterWorkspace = Nothing
        Dim pWSF As IWorkspaceFactory = Nothing

        Try
            pWSF = New RasterWorkspaceFactory
            pRWS = pWSF.OpenFromFile(RasterPath, 0)
            pDataset = pRWS.OpenRasterDataset(RasterName)
            pDataset.Delete()
            Return 1
        Catch ex As Exception
            MessageBox.Show("BA_Remove_Raster Exception: " + ex.Message)
            Return 0
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRWS)
            'Causes RCW separation exception
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWSF)
        End Try
    End Function

    'Opens and returns a IGeoDataset (raster) object
    Public Function BA_OpenRasterFromFile(ByRef filepath As String, ByRef FileName As String) As IGeoDataset
        Dim directoryInfo_check As DirectoryInfo = New DirectoryInfo(filepath)
        If directoryInfo_check.Exists Then
            'We have a valid directory, proceed
            'Add backslash to filepath (parent folder) if needed
            If Right(filepath, 1) <> "\" Then
                filepath = filepath & "\"
            End If
            'Remove backslash prefix from filename, if it's there, so we don't get two
            If Right(FileName, 1) = "\" Then
                FileName = FileName.Remove(Len(FileName) - 1, 1)
            End If
            Dim rasterExists As Boolean = False
            'First check to see if the raster exists as a file
            Dim fileInfo_check As FileInfo = New FileInfo(filepath + FileName)
            If fileInfo_check.Exists Then
                rasterExists = True
            Else
                'Second check to see if it exists as a folder; This will be the
                'case if it is a grid
                directoryInfo_check = New DirectoryInfo(filepath + FileName)
                If directoryInfo_check.Exists Then rasterExists = True
            End If

            If rasterExists Then

                'We have a valid raster file, proceed
                Dim pWSFactory As IWorkspaceFactory = New RasterWorkspaceFactory
                Dim pRasterWS As IRasterWorkspace
                Try
                    pRasterWS = CType(pWSFactory.OpenFromFile(filepath, 0), IRasterWorkspace)
                    Dim rasterDataset As IRasterDataset = pRasterWS.OpenRasterDataset(FileName)
                    Dim geoDataset As IGeoDataset = CType(rasterDataset, IGeoDataset) ' Explicit Cast
                    Return geoDataset
                Catch ex As Exception
                    MessageBox.Show("Exception: " + ex.Message)
                    Return Nothing
                Finally
                    pWSFactory = Nothing
                    pRasterWS = Nothing
                End Try
            Else
                'Not valid shapefile
                Return Nothing
            End If
        Else
            'Not a valid directory
            Return Nothing
        End If
    End Function

    'get rasterresolution
    Public Function BA_GetRasterStats(ByRef rasterfile_pathname As String, ByRef rasterresolution As Double) As IRasterStatistics
        Dim filepath As String = ""
        Dim FileName As String

        If String.IsNullOrEmpty(rasterfile_pathname) Then 'not a valid input
            Return Nothing
        End If

        FileName = BA_GetBareName(rasterfile_pathname, filepath)
        Dim pDEUtility As IDEUtilities = New DEUtilities
        Dim pRasterDataset As IRasterDataset = New RasterDataset
        Dim pWS As IRasterWorkspace
        Dim pWSF As IWorkspaceFactory = New RasterWorkspaceFactory
        Dim pRasterBand As IRasterBand
        Dim pRasterBandCollection As IRasterBandCollection

        Try
            '1. Open Raster
            pWS = pWSF.OpenFromFile(filepath, 0)
            pRasterDataset = pWS.OpenRasterDataset(FileName)

            '2. QI Raster to IRasterBand
            pRasterBandCollection = pRasterDataset
            pRasterBand = pRasterBandCollection.Item(0)

            'QI IRasterProps
            Dim pRasterP As IRasterProps = pRasterBand
            Dim pPnt As IPnt = New DblPnt
            pPnt = pRasterP.MeanCellSize
            rasterresolution = (pPnt.X + pPnt.Y) / 2
            Return pRasterBand.Statistics
        Catch ex As Exception
            rasterresolution = 0
            MessageBox.Show("Exception: " + ex.Message)
            Return Nothing
        Finally
            pRasterBand = Nothing
            pRasterBandCollection = Nothing
            pRasterDataset = Nothing
            pWS = Nothing
            pWSF = Nothing
            pDEUtility.ReleaseInternals()
        End Try
    End Function

    'Returns the summed area of all polygons in a shapefile
    Public Function BA_GetShapeArea(ByRef shapefile_pathname As String) As Double
        Dim filepath As String = ""
        Dim FileName As String
        Dim shapearea As Double = 0

        If String.IsNullOrEmpty(shapefile_pathname) Then 'not a valid input
            Return -1
            Exit Function
        End If

        FileName = BA_GetBareName(shapefile_pathname, filepath)

        'Get shape FeatureClass
        Dim pFeatureClass As IFeatureClass = BA_OpenFeatureClassFromGDB(filepath, FileName)

        'Extract Area
        Dim pCursor As IFeatureCursor = Nothing
        Dim pFeature As IFeature = Nothing
        Dim pQueryFilter As IQueryFilter = Nothing
        Dim pGeometry As IGeometry = Nothing
        Dim pArea As IArea = Nothing
        Try
            pCursor = pFeatureClass.Search(pQueryFilter, False)
            pFeature = pCursor.NextFeature
            Do While Not pFeature Is Nothing
                pGeometry = pFeature.Shape
                pArea = pGeometry
                pFeature = pCursor.NextFeature
                shapearea = shapearea + pArea.Area
            Loop
            Return shapearea
        Catch ex As Exception
            MessageBox.Show("Unable to get the area of " & shapefile_pathname & " !")
            MessageBox.Show("Exception: " + ex.Message)
            Return -1
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGeometry)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pArea)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeature)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeatureClass)
        End Try
    End Function

    ' Convert raster file to polygon shapefile
    Public Function BA_Raster2PolygonShapefile(ByVal Shapefile_BasePath As String, ByVal shapefilename As String, ByVal RasterDS As IGeoDataset) As Short
        Dim pFClass As IFeatureClass = Nothing
        Dim pWS As IFeatureWorkspace = Nothing
        Dim pWSF As IWorkspaceFactory = Nothing
        ' Create ConversionOP
        Dim pConversionOp As IConversionOp = New RasterConversionOp

        Try
            ' Get Shapefile Final Location
            ' Declare and set workspace
            Dim inputWorkspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(Shapefile_BasePath)
            If inputWorkspaceType = WorkspaceType.Raster Then
                pWSF = New ShapefileWorkspaceFactory
                pWS = pWSF.OpenFromFile(Shapefile_BasePath, 0)
            ElseIf inputWorkspaceType = WorkspaceType.Geodatabase Then
                pWSF = New FileGDBWorkspaceFactory
                pWS = pWSF.OpenFromFile(Shapefile_BasePath, 0)
            End If

            ' Calls function to open a feature class from disk
            pFClass = pConversionOp.RasterDataToPolygonFeatureData(RasterDS, pWS, shapefilename, False)
            Return 1
        Catch ex As Exception
            MessageBox.Show("BA_Raster2PolygonShapefile Exception: " + ex.Message)
            Return 0
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFClass)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pConversionOp)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWS)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWSF)
        End Try
    End Function

    'Alternate version of BA_Raster2PolygonShapefile; Uses filepaths rather than datasets as input for the conversion
    Public Function BA_Raster2PolygonShapefileFromPath(ByVal inputRasterPath As String, ByVal outputPath As String, ByVal addOutputsToMap As Boolean) As Short
        ' Does file exist?
        'Dim directoryInfo_check As IO.DirectoryInfo = New IO.DirectoryInfo(inputRasterPath)
        If BA_File_Exists(inputRasterPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            ' Initialize the geoprocessor.
            Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
            Dim tool As ESRI.ArcGIS.ConversionTools.RasterToPolygon = New ESRI.ArcGIS.ConversionTools.RasterToPolygon()
            Try
                GP.AddOutputsToMap = addOutputsToMap
                tool.in_raster = inputRasterPath
                tool.out_polygon_features = outputPath
                tool.simplify = False
                GP.Execute(tool, Nothing)
                Return 1
            Catch ex As Exception
                If GP.MessageCount > 0 Then
                    MessageBox.Show("Geoprocessor error: " + GP.GetMessages(2))
                Else
                    MessageBox.Show("Exception: " + ex.Message)
                End If
                Return 0
            Finally
                tool = Nothing
                GP = Nothing
                GC.Collect()
                GC.WaitForPendingFinalizers()
            End Try
        Else
            Return 0
        End If
    End Function

    'remove a shapefile and its associated files
    'return values: a positive number indicating the function is executed successfully, 0 otherwise
    'folder name cannot contain spaces
    Public Function BA_Remove_Shapefile(ByRef folder_string As String, ByRef shapefilename As String) As Short
        Dim pDataset As IDataset = Nothing
        Dim fc As IFeatureClass = Nothing

        Try
            fc = BA_OpenFeatureClassFromFile(folder_string, shapefilename)
            If fc Is Nothing Then
                Return 0
            Else
                pDataset = CType(fc, IDataset)
                pDataset.Delete()
                Return 1
            End If
        Catch ex As Exception
            MessageBox.Show("BA_Remove_Shapefile Exception: " + ex.Message)
            Return 0
        Finally
            fc = Nothing
            pDataset = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(fc)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDataset)
        End Try
    End Function

    ' Remove a folder and its contents recursively using Windows.IO api
    ' Consider using BA_Remove_Workspace which uses the Geoprocessor Delete tool
    Public Function BA_Remove_Folder(ByRef folder_string As String) As Short
        If String.IsNullOrEmpty(folder_string) Then
            Return 0
        Else
            Dim directoryInfo As DirectoryInfo = New DirectoryInfo(folder_string)
            Try
                If directoryInfo.Exists Then
                    'recursive delete; Includes all subfolders
                    directoryInfo.Delete(True)
                    Return 1
                Else
                    Return 0
                End If
            Catch ex As Exception
                MessageBox.Show("Exception: " + ex.Message)
                Return 0
            Finally
                directoryInfo = Nothing
                GC.Collect()
                GC.WaitForPendingFinalizers()
            End Try
        End If
    End Function

    'Delete a table (.dbf) using IFeatureWorkspace object
    'return values: a positive number indicating the function is executed successfully, 0 otherwise
    'folder name cannot contain spaces
    Public Function BA_Remove_Table(ByRef folder_string As String, ByRef file_name As String) As Short
        Dim pDataset As IDataset
        Dim fTable As ITable

        Try
            fTable = BA_OpenTableFromFile(folder_string, file_name)
            If fTable Is Nothing Then
                Return 0
            Else
                pDataset = CType(fTable, IDataset)
                pDataset.Delete()
                Return 1
            End If
        Catch ex As Exception
            MessageBox.Show("Exception: " + ex.Message)
            Return 0
        Finally
            fTable = Nothing
            pDataset = Nothing
        End Try
    End Function

    'query the attribute table of a shapefile using the value of one field to return the value of another
    Public Function BA_QueryAttributeTable(ByRef File_Path As String, ByRef File_Name As String, ByRef KeyField_Name As String, ByRef KeyField_Value As Object, ByRef KeyField_Type As String, ByRef ValueField_Name As String) As Object
        Dim TempKey_Value As String = Nothing

        ' Open the feature class
        Dim pFClass As IFeatureClass = BA_OpenFeatureClassFromFile(File_Path, File_Name)
        Dim pFCursor As IFeatureCursor = Nothing
        Dim pFeature As IFeature = Nothing
        Dim pQFilter As IQueryFilter = New QueryFilter

        Try
            ' read value
            ' Get field index
            Dim FI_Key, FI_Value As Short

            FI_Key = pFClass.FindField(KeyField_Name)
            FI_Value = pFClass.FindField(ValueField_Name)

            'If there is any apostrophe in the KeyField_Value, each of them should be replaced by two apostrophes
            If InStr(1, CStr(KeyField_Value), "'", CompareMethod.Text) > 0 Then
                TempKey_Value = CStr(KeyField_Value).Replace("'", "''")
            Else
                TempKey_Value = CStr(KeyField_Value)
            End If

            If KeyField_Type = BA_STRING_ATTRIBUTE Then
                pQFilter.WhereClause = KeyField_Name & " = '" & TempKey_Value & "'"
            ElseIf KeyField_Type = BA_NUMBER_ATTRIBUTE Then
                pQFilter.WhereClause = KeyField_Name & " = " & TempKey_Value
            End If
            pFCursor = pFClass.Search(pQFilter, False)
            pFeature = pFCursor.NextFeature

            If Not pFeature Is Nothing Then
                Return pFeature.Value(FI_Value)
            Else
                Return 0
            End If

        Catch ex As Exception
            MessageBox.Show("Exception: " + ex.Message)
            Return 0
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeature)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFClass)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pQFilter)
        End Try
    End Function

    'Read the first line of the folder_type file
    Public Function BA_Read_FolderType_File(ByVal path_string As String, ByRef type_key As String) As String
        Dim fileno As Short
        Dim FileName As String

        If String.IsNullOrEmpty(path_string) Or String.IsNullOrEmpty(type_key) Then
            Return ""
        End If

        'pad a backslash to the path if it doesn't have one.
        If Right(path_string, 1) <> "\" Then path_string = path_string & "\"
        FileName = path_string & type_key

        Try
            'open file for input
            fileno = FreeFile()
            FileOpen(fileno, FileName, OpenMode.Input)
            Return LineInput(fileno)
        Catch ex As Exception
            MessageBox.Show("Exception: " + ex.Message)
            Return ""
        Finally
            FileClose(fileno)
        End Try
    End Function

    ' Save raster dataset to a grid in a user specified workspace
    ' The workspace needs to be created first.
    ' return value: 0 error occurred
    ' 1 successfully saved the raster
    Public Function BA_SaveRasterDataset(ByVal pRaster As IGeoDataset, ByRef strPath As String, ByRef strName As String) As Short
        Dim pWSF As IWorkspaceFactory = Nothing
        Dim pWS As IRasterWorkspace = Nothing

        Try
            pWSF = New RasterWorkspaceFactory
            pWS = pWSF.OpenFromFile(strPath, 0)
            Dim outSave As ISaveAs2 = CType(pRaster, ISaveAs2)
            outSave.SaveAs(strName, pWS, "GRID")
            Return 1
        Catch ex As Exception
            MessageBox.Show("Exception: " + ex.Message)
            Return 0
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRaster)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWS)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWSF)
        End Try
    End Function

    ' Save shapefile in a user specified folder
    ' The folder needs to be created first.
    ' return value: 0 error occurred
    ' 1 successfully saved the shapefile
    Public Function BA_SaveFeatureClass(ByVal pFClass As IFeatureClass, ByVal strPath As String, ByVal strName As String) As Short
        Dim pWSF As IWorkspaceFactory
        Dim pWS As IFeatureWorkspace
        Try
            pWSF = New ShapefileWorkspaceFactory
            pWS = pWSF.OpenFromFile(strPath, 0)
            Dim pDataset As IDataset = CType(pFClass, IDataset)
            Dim pCopyFC As IFeatureClass = pDataset.Copy(strName, pWS)
            Return 1
        Catch ex As Exception
            MessageBox.Show("Exception: " + ex.Message)
            Return 0
        Finally
            pWS = Nothing
            pWSF = Nothing
        End Try
    End Function

    ' Save table in a user specified workspace
    ' The workspace needs to be created first.
    ' return value: 0 error occurred
    ' 1 successfully saved the shapefile
    Public Function BA_SaveTable(ByVal pSourceTable As ITable, ByVal strPath As String, ByVal strFileName As String) As Short

        Dim workspaceFactory As IWorkspaceFactory = New ShapefileWorkspaceFactory
        Dim workspace As IWorkspace = workspaceFactory.OpenFromFile(strPath, 0)
        Dim featureWorkspace As IFeatureWorkspace = CType(workspace, IFeatureWorkspace)
        Dim table As IDataset = CType(pSourceTable, IDataset)

        Try
            Dim inDatasetName As IDatasetName = table.FullName
            'check if .dbf extension is specified
            If String.Compare(".dbf", Right(strFileName, 4)) <> 0 Then
                strFileName = strFileName & ".dbf"
            End If

            Dim outWorkspace As IDataset = CType(workspace, IDataset)
            Dim outDatasetName As IDatasetName = New TableName
            outDatasetName.Name = strFileName
            outDatasetName.WorkspaceName = CType(outWorkspace.FullName, IWorkspaceName)

            Dim exportOperation As IExportOperation = New ExportOperation
            exportOperation.ExportTable(inDatasetName, Nothing, Nothing, outDatasetName, 0)
            Return 1
        Catch ex As Exception
            MessageBox.Show("Exception: " + ex.Message)
            Return 0
        Finally
            workspaceFactory = Nothing
            workspace = Nothing
            featureWorkspace = Nothing
            table = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try

    End Function

    'set map frame to the default map projection, i.e., NAD_1983_Albers USGS
    'ID: 102039
    'Projected Coordinate System:    NAD_1983_Albers
    'Projection: Albers
    'False_Easting:  0.00000000
    'False_Northing: 0.00000000
    'Central_Meridian:   -96.00000000
    'Standard_Parallel_1:    29.50000000
    'Standard_Parallel_2:    45.50000000
    'Latitude_Of_Origin: 23.00000000
    'Linear Unit:    Meter
    '
    'Geographic Coordinate System:   GCS_North_American_1983
    'Datum:  D_North_American_1983
    'Prime Meridian:     Greenwich
    'Angular Unit:   Degree
    Public Sub BA_SetDefaultProjection(ByVal application As ESRI.ArcGIS.Framework.IApplication)
        Dim pMxDoc As IMxDocument = application.Document
        Dim pMap As IMap = pMxDoc.FocusMap
        Dim ApplyProjection As Boolean = False

        'check if the map projection is alreay the same as the default
        Dim pSRef As ISpatialReference = pMap.SpatialReference
        If pSRef Is Nothing Then 'map projection is not defined
            ApplyProjection = True
        Else
            If Not pSRef.Name = "USA_Contiguous_Albers_Equal_Area_Conic_USGS_version" Then
                ApplyProjection = True
            End If
        End If

        Try
            Dim pSRFactory As ISpatialReferenceFactory2
            If ApplyProjection Then
                'set default projection with a Spatial Reference Factory
                pSRFactory = New SpatialReferenceEnvironment
                Dim albersCoordSystem As ISpatialReference = pSRFactory.CreateProjectedCoordinateSystem(ESRI.ArcGIS.Geometry.esriSRProjCS4Type.esriSRProjCS_NAD1983_USGS_USA_Albers)
                'Valid factory code for layer projection; Make sure it matches Albers datum
                If pSRef IsNot Nothing AndAlso pSRef.FactoryCode > 0 Then
                    'If pSRef IsNot Nothing Then
                    If Not Datum_Match(pSRef, albersCoordSystem) Then
                        Throw New Exception("Datums do not match. Layer cannot be correctly projected without a transformation")
                    End If
                End If
                pMap.SpatialReference = albersCoordSystem
                pMxDoc.ActivatedView.Refresh()
            End If
        Catch ex As Exception
            MessageBox.Show("Exception: " + ex.Message + vbCrLf + "Could not reproject to USA Contiguous Albers Equal Area Conic USGS! Please make sure your data are in this projection.")
        End Try
    End Sub

    'check if a shapefile exists
    Public Function BA_Shapefile_Exists(ByRef shapefilepath_name As String) As Boolean
        Dim SearchString As String
        If String.IsNullOrEmpty(shapefilepath_name) Then
            Return False
        End If

        'check if .shp extension is specified
        If Right(shapefilepath_name, 4) = ".shp" Then
            SearchString = shapefilepath_name
        Else
            SearchString = shapefilepath_name & ".shp"
        End If

        Dim pGPValue As IGPValue = New DEWorkspace
        pGPValue.SetAsText(SearchString)
        Dim pDEUtil As IDEUtilities = New DEUtilities
        Return pDEUtil.Exists(pGPValue)
    End Function

    'check if a folder exists
    Public Function BA_Workspace_Exists(ByRef mypath As String) As Boolean
        If String.IsNullOrEmpty(mypath) Then
            Return False
        End If
        Dim pDEUtil As IDEUtilities = New DEUtilities
        Dim pGPValue As IGPValue = New DEWorkspace
        Try
            pGPValue.SetAsText(mypath)
            Return pDEUtil.Exists(pGPValue)
        Catch ex As Exception
            MessageBox.Show("BA_Workspace_Exists Exception: " + ex.Message)
            Return False
        Finally
            ' Clears the value object
            pGPValue.Empty()
            pDEUtil.ReleaseInternals()
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGPValue)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDEUtil)
        End Try
    End Function

    'use the min, max, and interval values to propogate values of a rangearray
    'return value: number of intervals
    Public Function BA_CreateRangeArray(ByVal minval As Double, ByVal maxval As Double, ByVal interval As Double, ByRef rangearr() As BA_IntervalList) As Short
        Dim begincnt As Integer
        Dim endcnt As Integer
        Dim ninterval As Short
        Dim i As Short
        Dim Value As Double
        Dim rightoffset As Short

        If interval <= 0 Then
            Return 0
        End If

        'check the decimal place of the interval value
        Dim intvstring As String = interval
        'determine the interval decimal place to add an increment value to the lower bound
        Dim position As Integer = InStr(1, intvstring, ".", CompareMethod.Text)
        Dim inc_value As Object
        Dim scalefactor As Short

        If position = 0 And interval > 1 Then 'interval is an integer larger than 1
            scalefactor = 1
            inc_value = 1
        ElseIf interval = 1 Then
            scalefactor = 10
            inc_value = 1 / 10
        Else
            scalefactor = 10 ^ (Len(intvstring) - position)
            inc_value = 1 / (10 ^ (Len(intvstring) - position))
        End If

        'adjust value based on the scalefactor
        If scalefactor > 1 Then
            minval = System.Math.Round(minval * scalefactor - 0.5)
            maxval = System.Math.Round(maxval * scalefactor + 0.5)
            interval = interval * scalefactor
        End If

        'calculate the number of intervals
        begincnt = Int(minval / interval)

        endcnt = Int(maxval / interval) + 1
        'rightoffset indicates if the upperbound of the last interval equals maxval
        If CLng(maxval) Mod interval = 0 Then rightoffset = 1 Else rightoffset = 0

        ninterval = endcnt - begincnt - rightoffset
        If ninterval <= 0 Then ninterval = 1
        ReDim rangearr(ninterval)

        'set the min and max range values
        rangearr(1).LowerBound = minval / scalefactor
        rangearr(ninterval).UpperBound = maxval / scalefactor

        'set intermediate range values
        Value = (begincnt + 1) * interval
        For i = 1 To ninterval - 1
            rangearr(i).Value = i
            rangearr(i).UpperBound = Value / scalefactor
            rangearr(i + 1).LowerBound = Value / scalefactor
            Value = Value + interval
            If i = 1 Then
                rangearr(i).Name = rangearr(i).LowerBound & " - " & rangearr(i).UpperBound
            Else
                rangearr(i).Name = rangearr(i).LowerBound & " - " & rangearr(i).UpperBound
            End If
        Next

        rangearr(ninterval).Value = ninterval
        If ninterval > 1 Then
            rangearr(ninterval).Name = rangearr(ninterval).LowerBound & " - " & rangearr(ninterval).UpperBound
        Else
            rangearr(ninterval).Name = rangearr(ninterval).LowerBound & " - " & rangearr(ninterval).UpperBound
        End If

        Return ninterval
    End Function

    'Compares two spatial references to be sure they are the same
    Public Function Datum_Match(ByVal spatialReference1 As ISpatialReference, ByVal spatialReference2 As ISpatialReference) As Boolean
        Dim bytes As Long = Nothing
        Dim buffer As String = Nothing
        Dim buffer2 As String = Nothing
        Dim projectedCoordinateSystem1 As ISpatialReference = spatialReference1
        Dim parameterExport1 As IESRISpatialReferenceGEN2 = projectedCoordinateSystem1
        parameterExport1.ExportToESRISpatialReference2(buffer, bytes)
        Dim datumPos As Integer = InStr(buffer, "DATUM")
        Dim primePos As Integer = InStr(buffer, "PRIMEM")
        Dim datumStr As String = buffer.Substring(datumPos - 1, primePos - datumPos - 1)
        Dim projectedCoordinateSystem2 As ISpatialReference = spatialReference2
        Dim parameterExport2 As IESRISpatialReferenceGEN2 = projectedCoordinateSystem2
        parameterExport2.ExportToESRISpatialReference2(buffer2, bytes)
        Dim datumPos2 As Integer = InStr(buffer2, "DATUM")
        Dim primePos2 As Integer = InStr(buffer2, "PRIMEM")
        Dim datumStr2 As String = buffer2.Substring(datumPos2 - 1, primePos2 - datumPos2 - 1)
        If (String.Compare(datumStr, datumStr2) = 0) Then
            Return True
        Else
            Return False
        End If
    End Function

    'Are all values in the field in the specified feature class a whole number?
    Public Function BA_IsIntegerField(ByVal featClassDescr As IFeatureClassDescriptor, ByVal inputField As String) As Boolean
        Dim inputFeatureClass As IFeatureClass = featClassDescr.FeatureClass
        Dim pIndex As Short = inputFeatureClass.FindField(inputField)
        If pIndex < 1 Then
            Throw New Exception("The selected field does not exist on input feature class.")
        End If

        ' These field types cannot possibly be an integer; Always return false
        Dim pField As IField = inputFeatureClass.Fields.Field(pIndex)
        Dim pType As esriFieldType = pField.Type
        Select Case pType
            Case esriFieldType.esriFieldTypeBlob
                Return False
            Case esriFieldType.esriFieldTypeDate
                Return False
            Case esriFieldType.esriFieldTypeGeometry
                Return False
            Case esriFieldType.esriFieldTypeRaster
                Return False
            Case esriFieldType.esriFieldTypeString
                Return False
            Case esriFieldType.esriFieldTypeXML
                Return False
        End Select

        Dim pCursor As IFeatureCursor = Nothing
        Dim pFeature As IFeature = Nothing
        Try
            ' Cycle through all values and make sure there are no decimals
            pCursor = inputFeatureClass.Search(Nothing, False)
            pFeature = pCursor.NextFeature
            While Not pFeature Is Nothing
                Dim strValue As String = CStr(pFeature.Value(pIndex))
                Dim tst As Short = InStr(strValue, ".")
                If InStr(strValue, ".") > 0 Then
                    Return False
                End If
                pFeature = pCursor.NextFeature()
            End While
            Return True
        Catch ex As Exception
            MessageBox.Show("Exception: " + ex.Message)
            Return False
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeature)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pField)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(inputFeatureClass)
        End Try
    End Function

    'check if a folder is an aoi folder, i.e., contains the BA_AOI_Type file in output
    'check if a folder is an basin folder, i.e., contains the BA_Basin_Type file
    'If true, return the content of the folder type file, else a null string
    Public Function BA_Check_Folder_Type(ByVal path_string As String, ByRef type_key As String) As String
        Dim file_string As String = ""
        Dim checkstring As String

        If String.IsNullOrEmpty(path_string) Or String.IsNullOrEmpty(type_key) Then Return Nothing

        'pad a backslash to the path if it doesn't have one.
        If Right(path_string, 1) <> "\" Then path_string = path_string & "\"
        checkstring = path_string & type_key

        If BA_File_ExistsIDEUtil(checkstring) Then
            file_string = BA_Read_FolderType_File(path_string, type_key)
            Return file_string
        Else
            Return Nothing
        End If
    End Function

    'This procedure adds the specified DEM extent layer to ArcMap
    'DisplayName if specified, is used as the layer name in the TOC in ArcMap
    'Return values: -1 unknown error occurred
    '                       -2 filename error
    '                       -3 not a raster dataset
    '                       , otherwise, the same value as the Action code
    'add a raster file to the active view
    Public Function BA_DisplayRaster(ByVal application As ESRI.ArcGIS.Framework.IApplication, ByVal LayerPathName As String, _
                                     Optional ByVal DisplayName As String = "") As Short
        Dim File_Path As String = "PleaseReturn"
        Dim File_Name As String = BA_GetBareName(LayerPathName, File_Path)

        'exit if file_name is null
        If String.IsNullOrEmpty(File_Name) Then
            Return -2
        End If

        Dim pActiveView As IActiveView
        Dim pEnv As IEnvelope
        Dim pTempLayer As ILayer
        Dim pRLayer As IRasterLayer = New RasterLayer
        Dim pRasterDS As IGeoDataset = Nothing
        Dim pMxDoc As IMxDocument = Nothing
        Dim pMap As IMap = Nothing

        Try
            Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(LayerPathName)
            If workspaceType = workspaceType.Raster Then
                'text exists for the setting of this layer
                pRasterDS = BA_OpenRasterFromFile(File_Path, File_Name)
            ElseIf workspaceType = workspaceType.Geodatabase Then
                pRasterDS = BA_OpenRasterFromGDB(File_Path, File_Name)
            End If
            pRLayer.CreateFromDataset(pRasterDS)

            'set layer name
            If Not String.IsNullOrEmpty(DisplayName) Then
                pRLayer.Name = DisplayName
            End If

            Dim Buffer_Factor As Double = 2

            'add layer
            pMxDoc = application.Document
            pMap = pMxDoc.FocusMap

            'check if a layer with the assigned name exists
            'remove layer of the specified name, if found
            Dim nlayers As Integer = pMap.LayerCount
            Dim i As Integer
            For i = nlayers To 1 Step -1
                pTempLayer = pMap.Layer(i - 1)
                If pRLayer.Name = pTempLayer.Name Then 'remove the layer
                    pMap.DeleteLayer(pTempLayer)
                End If
            Next

            pMxDoc.AddLayer(pRLayer)
            pMxDoc.UpdateContents()

            'zoom to layer
            'create a buffer around the AOI
            pEnv = pRLayer.AreaOfInterest

            Dim urx, llx, lly, ury As Double
            Dim xrange, yrange As Double
            Dim xoffset, yoffset As Double

            pEnv.QueryCoords(llx, lly, urx, ury)
            xrange = urx - llx
            yrange = ury - lly
            xoffset = xrange * (Buffer_Factor - 1) / 2
            yoffset = yrange * (Buffer_Factor - 1) / 2
            llx = llx - xoffset
            lly = lly - yoffset
            urx = urx + xoffset
            ury = ury + yoffset
            pEnv.PutCoords(llx, lly, urx, ury)

            pActiveView = pMxDoc.ActiveView
            pActiveView.Extent = pEnv

            'refresh the active view
            pMxDoc.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
            Return 0
        Catch ex As Exception
            MessageBox.Show("BA_DisplayRaster Exception: " + ex.Message)
            Return -1
        Finally
            pActiveView = Nothing
            pEnv = Nothing
            pTempLayer = Nothing
            pRLayer = Nothing
            pRasterDS = Nothing
            pMxDoc = Nothing
            pMap = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    'This procedure adds the specified DEM extent layer to ArcMap
    'DisplayName if specified, is used as the layer name in the TOC in ArcMap
    'Return values: -1 unknown error occurred
    '                       -2 filename error
    '                       -3 not a raster dataset
    '                       , otherwise, the same value as the Action code
    'add a raster file to the active view
    Public Function BA_DisplayRasterNoBuffer(ByVal application As ESRI.ArcGIS.Framework.IApplication, ByVal LayerPathName As String, _
                                             ByVal DisplayName As String, ByVal zoomToLayer As Boolean) As Short
        Dim File_Path As String = "PleaseReturn"
        Dim File_Name As String = BA_GetBareName(LayerPathName, File_Path)

        'exit if file_name is null
        If String.IsNullOrEmpty(File_Name) Then
            Return -2
        End If

        Dim pActiveView As IActiveView
        Dim pEnv As IEnvelope
        Dim pTempLayer As ILayer
        Dim pRLayer As IRasterLayer = New RasterLayer
        Dim pRasterDS As IGeoDataset = Nothing
        Dim pMxDoc As IMxDocument = Nothing
        Dim pMap As IMap = Nothing

        Try
            Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(LayerPathName)
            If workspaceType = workspaceType.Raster Then
                'text exists for the setting of this layer
                pRasterDS = BA_OpenRasterFromFile(File_Path, File_Name)
            ElseIf workspaceType = workspaceType.Geodatabase Then
                pRasterDS = BA_OpenRasterFromGDB(File_Path, File_Name)
            End If
            pRLayer.CreateFromDataset(pRasterDS)

            'set layer name
            If Not String.IsNullOrEmpty(DisplayName) Then
                pRLayer.Name = DisplayName
            End If

            Dim Buffer_Factor As Double = 2

            'add layer
            pMxDoc = application.Document
            pMap = pMxDoc.FocusMap

            'check if a layer with the assigned name exists
            'remove layer of the specified name, if found
            Dim nlayers As Integer = pMap.LayerCount
            Dim i As Integer
            For i = nlayers To 1 Step -1
                pTempLayer = pMap.Layer(i - 1)
                If pRLayer.Name = pTempLayer.Name Then 'remove the layer
                    pMap.DeleteLayer(pTempLayer)
                End If
            Next

            pMxDoc.AddLayer(pRLayer)
            pMxDoc.UpdateContents()

            'zoom to layer
            If zoomToLayer = True Then
                pEnv = pRLayer.AreaOfInterest
                pActiveView = pMxDoc.ActiveView
                pActiveView.Extent = pEnv
            End If

            'refresh the active view
            pMxDoc.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
            Return 0
        Catch ex As Exception
            MessageBox.Show("BA_DisplayRaster Exception: " + ex.Message)
            Return -1
        Finally
            pActiveView = Nothing
            pEnv = Nothing
            pTempLayer = Nothing
            pRLayer = Nothing
            pRasterDS = Nothing
            pMxDoc = Nothing
            pMap = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    'This procedure adds the specified vector layer to ArcMap
    'DisplayName if specified, is used as the layer name in the TOC in ArcMap
    'Action code: 0, default add and replace if layer of the same name already added, and zoom to layer
    '                   1, add and replace only
    '                   2, add
    'Return values: -1 unknown error occurred
    '                       -2 filename error
    '                       , otherwise, the same value as the Action code
    Public Function BA_DisplayVector(ByVal pMxDoc As IMxDocument, ByVal LayerPathName As String, _
                                     Optional ByVal DisplayName As String = "", Optional ByVal Action As Short = 0, _
                                     Optional ByVal Buffer_Factor As Double = 2) As Short
        Dim File_Path As String = "Please return"
        Dim File_Name As String

        File_Name = BA_GetBareName(LayerPathName, File_Path)
        'exit if file_name is null
        If String.IsNullOrEmpty(File_Name) Then
            Return -2
        End If

        Dim pFeatClass As IFeatureClass = Nothing
        Dim pFLayer As IFeatureLayer = Nothing
        Dim pEnv As IEnvelope = Nothing
        Dim pTempLayer As ILayer = Nothing
        Dim pMap As IMap = Nothing
        Dim pActiveView As IActiveView = Nothing
        Try

            'text exists for the setting of this layer
            Dim inputWorkspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(File_Path)
            If inputWorkspaceType = WorkspaceType.Raster Then
                pFeatClass = BA_OpenFeatureClassFromFile(File_Path, File_Name)
            ElseIf inputWorkspaceType = WorkspaceType.Geodatabase Then
                pFeatClass = BA_OpenFeatureClassFromGDB(File_Path, File_Name)
            End If

            'add featureclass to current data frame
            pFLayer = New FeatureLayer
            pFLayer.FeatureClass = pFeatClass

            'set layer name
            If String.IsNullOrEmpty(DisplayName) Then
                pFLayer.Name = pFLayer.FeatureClass.AliasName
            Else
                pFLayer.Name = DisplayName
            End If

            'add layer
            pMap = pMxDoc.FocusMap

            'check if a layer with the assigned name exists
            Dim nlayers As Integer
            Dim i As Integer
            If Action <> 2 Then
                'search layer of the specified name, if found
                nlayers = pMap.LayerCount
                If nlayers > 0 Then
                    For i = nlayers To 1 Step -1
                        pTempLayer = pMap.Layer(i - 1)
                        If pFLayer.Name = pTempLayer.Name Then 'remove the layer
                            pMap.DeleteLayer(pTempLayer)
                        End If
                    Next
                End If
            End If

            pMap.AddLayer(pFLayer)

            Dim urx, llx, lly, ury As Double
            Dim xrange, yrange As Double
            Dim xoffset, yoffset As Double
            If Action = 0 Then 'zoom to layer
                'create a buffer around the AOI
                pEnv = pFLayer.AreaOfInterest
                pEnv.QueryCoords(llx, lly, urx, ury)
                xrange = urx - llx
                yrange = ury - lly
                xoffset = xrange * (Buffer_Factor - 1) / 2
                yoffset = yrange * (Buffer_Factor - 1) / 2
                llx = llx - xoffset
                lly = lly - yoffset
                urx = urx + xoffset
                ury = ury + yoffset
                pEnv.PutCoords(llx, lly, urx, ury)

                pActiveView = pMxDoc.ActiveView
                pActiveView.Extent = pEnv
            End If

            'refresh the active view
            pMxDoc.ActivatedView.Refresh()
            pMxDoc.UpdateContents()
            Return Action
        Catch ex As Exception
            Return -1
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeatClass)
            pFeatClass = Nothing
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFLayer)
            pFLayer = Nothing
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pEnv)
            pEnv = Nothing
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTempLayer)
            pTempLayer = Nothing
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pMap)
            pMap = Nothing
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pActiveView)
            pActiveView = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    ' Checks input string for spaces
    Public Function BA_ContainsSpace(ByVal inputString As String) As Boolean
        Dim pos As Integer = inputString.IndexOfAny(" ")
        If pos > -1 Then
            Return True
        Else
            Return False
        End If
    End Function

    ' Rename a raster file
    Public Function BA_RenameRaster(ByVal inputFolder As String, ByVal inputFile As String, _
                                    ByVal outputFile As String) As BA_ReturnCode
        Dim pRDataset As IRasterDataset
        Dim pInputDataset As IGeoDataset
        Dim pDataset As IDataset
        Try
            pInputDataset = BA_OpenRasterFromFile(inputFolder, inputFile)
            If pInputDataset IsNot Nothing Then
                pDataset = CType(pInputDataset, IDataset)
                pDataset.Rename(outputFile)
            End If
            Return BA_ReturnCode.Success
        Catch ex As Exception
            MessageBox.Show("BA_RenameRaster() Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            pInputDataset = Nothing
            pDataset = Nothing
            pRDataset = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try

    End Function

    ' Rename a directory
    Public Function BA_RenameWorkspace(ByVal inputFolder As String, _
                                       ByVal outputFolder As String) As BA_ReturnCode

        Dim dirInfo As DirectoryInfo = New DirectoryInfo(inputFolder)
        If dirInfo.Exists Then
            dirInfo.MoveTo(outputFolder)
        Else
            Return BA_ReturnCode.ReadError
        End If
        Return BA_ReturnCode.Success

    End Function

    ' Calculate raster cellsize from an input GeoDataset
    Public Function BA_CellSizeFromGeoDataset(ByVal pGeoDataset As IGeoDataset) As Double

        Dim pRasterDataset As IRasterDataset = New RasterDataset
        Dim pRasterBand As IRasterBand = Nothing
        Dim pRasterBandCollection As IRasterBandCollection = Nothing
        Dim pRasterP As IRasterProps = Nothing
        Try
            ' Extract IRasterBand from input IGeoDataSet
            pRasterDataset = CType(pGeoDataset, IRasterDataset) ' Explicit cast
            pRasterBandCollection = pRasterDataset
            pRasterBand = pRasterBandCollection.Item(0)

            ' Get resolution from IRasterBand and calculate cellsize
            pRasterP = pRasterBand
            Dim pPnt As IPnt = New DblPnt
            pPnt = pRasterP.MeanCellSize
            Return (pPnt.X + pPnt.Y) / 2
        Catch ex As Exception
            MsgBox("BA_CellSizeFromGeoDataset Exception: " & ex.Message)
            Return 0
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterP)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBand)
            'Can't release following to objects without causing an RCW exception in calling sub
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBandCollection)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterDataset)
        End Try
    End Function

    Public Function BA_FieldsListFromFeatureClass(ByVal inputFeatureClass As IFeatureClass) As List(Of String)
        Dim stringList As List(Of String) = New List(Of String)

        Try
            ' Cycle through all fields and save them to the List
            For pIndex As Integer = 0 To inputFeatureClass.Fields.FieldCount - 1
                Dim pField As IField = inputFeatureClass.Fields.Field(pIndex)
                Dim pType As esriFieldType = pField.Type

                Select Case pType
                    Case esriFieldType.esriFieldTypeDouble
                        stringList.Add(CStr(pField.Name))
                    Case esriFieldType.esriFieldTypeInteger
                        stringList.Add(CStr(pField.Name))
                    Case esriFieldType.esriFieldTypeSingle
                        stringList.Add(CStr(pField.Name))
                    Case esriFieldType.esriFieldTypeSmallInteger
                        stringList.Add(CStr(pField.Name))
                    Case esriFieldType.esriFieldTypeString
                        stringList.Add(CStr(pField.Name))
                End Select
            Next
            Return stringList
        Catch ex As Exception
            MessageBox.Show("BA_FieldsListFromFeatureClass Exception: " + ex.Message)
            Return Nothing
        End Try

    End Function

    ' Verify that the raster dataset uses the same datum as the datum stored in the hruExtension
    Public Function BA_DatumMatch(ByVal layerPath As String, ByVal datumStr As String) As Boolean
        Dim parentPath As String = "PleaseReturn"
        Dim fileName As String = BA_GetBareName(layerPath, parentPath)
        Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(layerPath)
        Dim pGeoDataset As IGeoDataset = Nothing
        Dim pSpRef As ISpatialReference = Nothing
        Try
            'Open the dataset to extract the spatial reference
            If workspaceType = workspaceType.Raster Then
                pGeoDataset = BA_OpenRasterFromFile(parentPath, fileName)
            ElseIf workspaceType = workspaceType.Geodatabase Then
                pGeoDataset = BA_OpenRasterFromGDB(parentPath, fileName)
            End If
            'Spatial reference for the dataset in question
            pSpRef = pGeoDataset.SpatialReference
            'Extract datum string from spatial reference
            Dim datumStr2 As String = BA_DatumString(pSpRef)
            If (String.Compare(datumStr, datumStr2) = 0) Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            MessageBox.Show("BA_DatumMatch Exception: " & ex.Message)
            Return False
        Finally
            pGeoDataset = Nothing
            pSpRef = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    ' Verify that the vector dataset uses the same datum as the datum stored in the hruExtension
    Public Function BA_VectorDatumMatch(ByVal layerPath As String, ByVal datumStr As String) As Boolean
        Dim parentPath As String = "PleaseReturn"
        Dim fileName As String = BA_GetBareName(layerPath, parentPath)
        Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(layerPath)
        Dim pGeoDataset As IGeoDataset = Nothing
        'Open the dataset to extract the spatial reference
        If workspaceType = workspaceType.Raster Then
            pGeoDataset = BA_OpenFeatureClassFromFile(parentPath, fileName)
        ElseIf workspaceType = workspaceType.Geodatabase Then
            pGeoDataset = BA_OpenFeatureClassFromGDB(parentPath, fileName)
        End If
        'Spatial reference for the dataset in question
        Dim pSpRef As ISpatialReference = pGeoDataset.SpatialReference
        'Extract datum string from spatial reference
        Dim datumStr2 As String = BA_DatumString(pSpRef)
        If (String.Compare(datumStr, datumStr2) = 0) Then
            Return True
        Else
            Return False
        End If
    End Function

    'Extract the datum string from an IspatialReference
    Public Function BA_DatumString(ByVal pSpRef As ISpatialReference) As String
        If pSpRef IsNot Nothing Then
            ' Explicit cast
            Dim parameterExport1 As IESRISpatialReferenceGEN2 = CType(pSpRef, IESRISpatialReferenceGEN2)
            Dim buffer As String = Nothing
            Dim bytes As Long = Nothing
            parameterExport1.ExportToESRISpatialReference2(buffer, bytes)
            Dim datumPos As Integer = InStr(buffer, "DATUM")
            Dim primePos As Integer = InStr(buffer, "PRIMEM")
            Return buffer.Substring(datumPos - 1, primePos - datumPos - 1)
        End If
        Return Nothing
    End Function


    Public Function BA_StandardizePathString(ByVal inputString As String, _
Optional ByVal hasPaddingBackSlach As Boolean = False) As String

        'this function returns a path string based on the options specified by user
        Dim returnString As String = inputString.Trim

        If returnString.Length > 0 Then
            'remove or add padding backslash from/to string
            If returnString(Len(returnString) - 1) = "\" Then
                If Not hasPaddingBackSlach Then
                    returnString = returnString.Remove(Len(returnString) - 1, 1)
                End If
            Else
                If hasPaddingBackSlach Then
                    returnString = returnString & "\"
                End If
            End If
        End If

        Return returnString
    End Function

    Public Function BA_StandardizeShapefileName(ByVal inputString As String, _
    Optional ByVal hasExtensionName As Boolean = True, _
    Optional ByVal hasLeadingBackSlach As Boolean = False) As String

        'this function returns a standardized shapefile name based on the options specified by user
        Dim returnString As String = ""
        inputString = inputString.Trim

        If inputString.Length > 0 Then
            'remove path from input string
            returnString = BA_GetBareName(inputString)

            'remove or add .shp extension name from/to string
            'remove shp extension from the layerName
            If Right(returnString, 4).ToUpper = ".SHP" Then
                If Not hasExtensionName Then
                    returnString = returnString.Remove(Len(returnString) - Len(".shp"), Len(".shp"))
                End If
            Else
                If hasExtensionName Then
                    returnString = returnString & ".shp"
                End If
            End If

            'remove or add leading backslash from/to string
            If returnString(0) = "\" Then
                If Not hasLeadingBackSlach Then
                    returnString = returnString.Remove(0, 1)
                End If
            Else
                If hasLeadingBackSlach Then
                    returnString = "\" & returnString
                End If
            End If
        End If

        Return returnString
    End Function

    ' Delete a file other than an ArcGIS layer; Used for xml
    Public Function BA_Remove_File(ByRef folder_string As String) As BA_ReturnCode
        If String.IsNullOrEmpty(folder_string) Then
            Return BA_ReturnCode.NotSupportedOperation
        Else
            Dim fileInfo As FileInfo = New FileInfo(folder_string)
            Try
                If fileInfo.Exists Then
                    'Delete file
                    fileInfo.Delete()
                    Return BA_ReturnCode.Success
                Else
                    Return BA_ReturnCode.UnknownError
                End If
            Catch ex As Exception
                MessageBox.Show("BA_Remove_File Exception: " + ex.Message)
                Return BA_ReturnCode.UnknownError
            Finally
                ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(fileInfo)
            End Try
        End If
    End Function

    'The function is in the ModulePublic module
    'create the output file structure in a basin or an AOI
    'the output file structure is created under the folder indicated in the path_string
    'return values: integer, indicating the number of subfolders created
    'If is_aoi is not specified, then it's not an AOI output
    Public Function BA_Create_Output_Folders(ByVal basepath_string As String, ByVal pmxDoc As IMxDocument, _
                                             ByVal folderType As FolderType) As BA_ReturnCode
        'check if the process is for creating AOI folders
        Dim folder_string As String
        Dim response As BA_ReturnCode

        folder_string = BA_GetPath(basepath_string, PublicPath.Output)
        If Len(Dir(folder_string, vbDirectory)) <> 0 Then 'i.e., folder already exists
            'remove layers in the DEM folder from the active map
            response = BA_RemoveLayersInFolder(pmxDoc, folder_string)
            'delete the folder and its subfolders
            response = BA_Remove_Folder(folder_string)
        End If

        Dim pWksFactory As IWorkspaceFactory = New ArcInfoWorkspaceFactory
        Dim pWorkspaceName As IWorkspaceName = Nothing

        Try
            'Create the AOI folder if it doesn't exist; This is for BAGIS compatibility
            If Not BA_Folder_ExistsWindowsIO(basepath_string) Then
                Dim AOIName As String
                Dim AOIPath As String = "Return" 'this will be the basin or parent AOI's folder path
                AOIName = BA_GetBareName(basepath_string, AOIPath)
                If folderType = folderType.AOI Then 'if the file struction is for an AOI, then the AOI folder also needs to be created
                    pWorkspaceName = pWksFactory.Create(AOIPath, AOIName, Nothing, 0)
                    pWorkspaceName = Nothing
                End If
            End If

            'create supporting folder structure, including
            'output, output\surfaces, and output\surfaces\dem
            'Create output folder
            Dim newFolder As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.Output), False, False)
            pWorkspaceName = pWksFactory.Create(basepath_string, newFolder, Nothing, 0)

            '/output/surfaces
            Dim tempPath = ""
            Dim tempName = BA_GetBareName(BA_EnumDescription(PublicPath.SourceDEM), tempPath)
            newFolder = BA_GetBareName(tempPath)
            pWorkspaceName = pWksFactory.Create(BA_GetPath(basepath_string, PublicPath.Output), newFolder, Nothing, 0)

            '/output/surfaces/dem
            newFolder = BA_GetBareName(BA_EnumDescription(PublicPath.SourceDEM), tempPath)
            pWorkspaceName = pWksFactory.Create(basepath_string & tempPath, newFolder, Nothing, 0)

            '/output/surfaces/dem/filled
            newFolder = BA_GetBareName(BA_EnumDescription(PublicPath.DEM), tempPath)
            pWorkspaceName = pWksFactory.Create(basepath_string & tempPath, newFolder, Nothing, 0)

            'create dem surface folders; Get the names from the enumeration
            Dim values As PublicPath() = [Enum].GetValues(GetType(PublicPath))
            For i = PublicPath.FlowDirection To PublicPath.Aspect
                Dim nextPath As PublicPath = values(i)
                newFolder = BA_GetBareName(BA_EnumDescription(nextPath), tempPath)
                pWorkspaceName = pWksFactory.Create(basepath_string & tempPath, newFolder, Nothing, 0)
            Next

            'If AOI, create hillshade folder
            If folderType = folderType.AOI Then
                newFolder = BA_GetBareName(BA_EnumDescription(PublicPath.Hillshade), tempPath)
                pWorkspaceName = pWksFactory.Create(basepath_string & tempPath, newFolder, Nothing, 0)

                'And zones folder
                'output/zones
                newFolder = BA_GetBareName(BA_EnumDescription(PublicPath.HruDirectory))
                pWorkspaceName = pWksFactory.Create(BA_GetPath(basepath_string, PublicPath.Output), newFolder, Nothing, 0)
            End If

            'create PRISM folders
            If folderType = folderType.AOI Then 'if the file struction is for an AOI, then the PRISM folders also needs to be created
                'PRISM is a subfolder in the LAYER folder
                newFolder = BA_GetBareName(BA_EnumDescription(PublicPath.Layers))
                pWorkspaceName = pWksFactory.Create(basepath_string, newFolder, Nothing, 0)

                newFolder = BA_GetBareName(BA_EnumDescription(PublicPath.PRISM), tempPath)
                pWorkspaceName = pWksFactory.Create(basepath_string & tempPath, newFolder, Nothing, 0)

                Dim prismFolders As AOIPrismFolderNames() = [Enum].GetValues(GetType(AOIPrismFolderNames))
                For i = AOIPrismFolderNames.jan To AOIPrismFolderNames.annual
                    Dim prismFolder As AOIPrismFolderNames = prismFolders(i)
                    pWorkspaceName = pWksFactory.Create(BA_GetPath(basepath_string, PublicPath.PRISM), prismFolder.ToString, Nothing, 0)
                Next

                'Create analysis folder
                newFolder = BA_GetBareName(BA_EnumDescription(PublicPath.Analysis))
                pWorkspaceName = pWksFactory.Create(basepath_string, newFolder, Nothing, 0)

                'Create maps folder
                newFolder = BA_GetBareName(BA_EnumDescription(PublicPath.Maps))
                pWorkspaceName = pWksFactory.Create(basepath_string, newFolder, Nothing, 0)

                'Create maps folder
                newFolder = BA_GetBareName(BA_EnumDescription(PublicPath.Tables))
                pWorkspaceName = pWksFactory.Create(basepath_string, newFolder, Nothing, 0)
            End If

            Return BA_ReturnCode.Success
        Catch ex As Exception
            MessageBox.Show("BA_Create_Output_Folders exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWksFactory)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWorkspaceName)
        End Try

    End Function

    Public Function BA_DeleteWeaselFiles(ByVal aoiPath As String, ByVal pmxDoc As IMxDocument) As BA_ReturnCode

        Try
            'Delete aoi files at root directory
            Dim aoiVectorList(0) As String
            Dim aoiRasterList(0) As String
            BA_ListLayersinAOI(aoiPath, aoiRasterList, aoiVectorList)
            For Each strVector In aoiVectorList
                If strVector IsNot Nothing Then
                    BA_RemoveLayers(pmxDoc, strVector)
                    BA_Remove_Shapefile(aoiPath, strVector)
                End If
            Next
            For Each strRaster In aoiRasterList
                If strRaster IsNot Nothing Then
                    BA_RemoveLayers(pmxDoc, strRaster)
                    BA_Remove_Raster(aoiPath, strRaster)
                End If
            Next

            Return BA_ReturnCode.Success
        Catch ex As Exception
            MessageBox.Show("BA_DeleteWeaselFiles exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally

        End Try

    End Function

    'Sets the settings path in the HruExtension. Checks for BAGIS, TMP, TEMP, 
    'and AGSDESKTOPJAVA in that order. Converted VBA BAGIS function. Customized 
    'templates will be stored here.
    'Summary of Windows environment variables http://en.wikipedia.org/wiki/Environment_variable#Default_Values_on_Microsoft_Windows
    Public Function BA_GetSettingsPath() As String
        Dim settingsPath As String = Environ(BA_PATH_SETTINGS) 'try the BAGIS folder first

        'Note: this environment variable is only good on Windows 7; We use this for Windows 7 instead of the TEMP variables because
        'ArcGIS 10 deletes items from the TEMP folder under Windows u
        If Len(settingsPath) = 0 Then 'then, try the APPDATA folder
            settingsPath = Environ("APPDATA")
        End If

        If Len(settingsPath) = 0 Then 'then, try the TMP folder
            settingsPath = Environ("TMP")
        End If

        If Len(settingsPath) = 0 Then 'then, try the TEMP folder
            settingsPath = Environ("TEMP")
        End If

        'Lastly try the AGSDESKTOPJAVA folder for ArcGIS 10
        If Len(settingsPath) = 0 Then 'Lastly try the AGSDESKTOPJAVA folder for ArcGIS 10
            settingsPath = Environ("AGSDESKTOPJAVA")
        End If

        'remove the slash if it's in the string
        If Right(settingsPath, 1) = "\" Then settingsPath = Left(settingsPath, Len(settingsPath) - 1)

        Return settingsPath
    End Function

    ' Creates a new folder when provided with root path and name using ArcObjects Workspace API
    Public Function BA_CreateFolder(ByRef existingFolder As String, ByVal newFolder As String) As String
        Dim pWksFactory As IWorkspaceFactory
        Dim pWorkspaceName As IWorkspaceName
        Try
            pWksFactory = New ShapefileWorkspaceFactory
            Dim dirInfo As DirectoryInfo = New DirectoryInfo(existingFolder & newFolder)
            '\output\zones
            If Not dirInfo.Exists Then
                pWorkspaceName = pWksFactory.Create(existingFolder, newFolder, Nothing, 0)
            End If
            Return existingFolder & "\" & newFolder

        Catch ex As Exception
            MessageBox.Show("BA_CreateFolder Exception: " + ex.Message)
            Return Nothing
        Finally
            pWorkspaceName = Nothing
            pWksFactory = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Function

    ' Assembles HRU file path string when provided with AOI and HRU names
    Public Function BA_GetHruPath(ByVal folderBase As String, ByVal pathKey As PublicPath, _
                                  ByVal hruName As String)
        'please check the length of returning string for errors, if 0, then error occurred
        If String.IsNullOrEmpty(folderBase) Then
            Return ""
        End If

        'remove the last character if it's a backslash
        If Right(folderBase, 1) = "\" Then folderBase = Left(folderBase, Len(folderBase) - 1)

        Dim path_string As String = folderBase & BA_EnumDescription(pathKey)
        If Not String.IsNullOrEmpty(hruName) Then
            path_string = path_string & "\" & hruName
        End If
        Return path_string
    End Function

    ' Assembles HRU file path string for gdb when provided with AOI and HRU names
    Public Function BA_GetHruPathGDB(ByVal folderBase As String, ByVal pathKey As PublicPath, _
                                  ByVal hruName As String)
        'please check the length of returning string for errors, if 0, then error occurred
        If String.IsNullOrEmpty(folderBase) Then
            Return ""
        End If

        'remove the last character if it's a backslash
        If Right(folderBase, 1) = "\" Then folderBase = Left(folderBase, Len(folderBase) - 1)

        Dim path_string As String = folderBase & BA_EnumDescription(pathKey)
        If Not String.IsNullOrEmpty(hruName) Then
            path_string = path_string & "\" & hruName & "\" & hruName & ".gdb"
        End If
        Return path_string
    End Function

    'This procedure adds the specified vector layer to ArcMap
    'DisplayName if specified, is used as the layer name in the TOC in ArcMap
    'Action code: 0, default add and replace if layer of the same name already added, and zoom to layer
    '                   1, add and replace only
    '                   2, add
    Public Function BA_DisplayVectorNoRefresh(ByVal pMxDoc As IMxDocument, ByVal parentPath As String, ByVal vPath As String, _
                                              Optional ByVal DisplayName As String = "", Optional ByVal Action As Short = 0, _
                                              Optional ByVal Buffer_Factor As Double = 2) As BA_ReturnCode
        'exit if file_name is null
        If String.IsNullOrEmpty(parentPath) Or String.IsNullOrEmpty(vPath) Then
            Return BA_ReturnCode.ReadError
        End If

        Dim pFeatClass As IFeatureClass = Nothing
        Dim pFLayer As IFeatureLayer = Nothing
        Dim pEnv As IEnvelope = Nothing
        Dim pTempLayer As ILayer = Nothing
        Dim pMap As IMap = Nothing
        Dim pActiveView As IActiveView = Nothing
        Try
            'text exists for the setting of this layer
            Dim inputWorkspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(parentPath)
            If inputWorkspaceType = WorkspaceType.Raster Then
                pFeatClass = BA_OpenFeatureClassFromFile(parentPath, vPath)
            ElseIf inputWorkspaceType = WorkspaceType.Geodatabase Then
                pFeatClass = BA_OpenFeatureClassFromGDB(parentPath, vPath)
            End If

            'add featureclass to current data frame
            pFLayer = New FeatureLayer
            pFLayer.FeatureClass = pFeatClass

            'set layer name
            If String.IsNullOrEmpty(DisplayName) Then
                pFLayer.Name = pFLayer.FeatureClass.AliasName
            Else
                pFLayer.Name = DisplayName
            End If

            'add layer
            pMap = pMxDoc.FocusMap

            'check if a layer with the assigned name exists
            Dim nlayers As Integer
            Dim i As Integer
            If Action <> 2 Then
                'search layer of the specified name, if found
                nlayers = pMap.LayerCount
                If nlayers > 0 Then
                    For i = nlayers To 1 Step -1
                        pTempLayer = pMap.Layer(i - 1)
                        If pFLayer.Name = pTempLayer.Name Then 'remove the layer
                            pMap.DeleteLayer(pTempLayer)
                        End If
                    Next
                End If
            End If

            pMap.AddLayer(pFLayer)

            Dim urx, llx, lly, ury As Double
            Dim xrange, yrange As Double
            Dim xoffset, yoffset As Double
            If Action = 0 Then 'zoom to layer
                'create a buffer around the AOI
                pEnv = pFLayer.AreaOfInterest
                pEnv.QueryCoords(llx, lly, urx, ury)
                xrange = urx - llx
                yrange = ury - lly
                xoffset = xrange * (Buffer_Factor - 1) / 2
                yoffset = yrange * (Buffer_Factor - 1) / 2
                llx = llx - xoffset
                lly = lly - yoffset
                urx = urx + xoffset
                ury = ury + yoffset
                pEnv.PutCoords(llx, lly, urx, ury)

                pActiveView = pMxDoc.ActiveView
                pActiveView.Extent = pEnv
            End If
            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_DisplayVectorNoRefresh Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            pFeatClass = Nothing
            pFLayer = Nothing
            pEnv = Nothing
            pTempLayer = Nothing
            pMap = Nothing
            pActiveView = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    'Convert a raster dataset to vector
    'The workspace needs to be created first.
    'return value: 0 error occurred
    '              1 successfully converted the raster and saved the vector
    Public Function BA_Raster2LineShapefile(ByVal InRaster As IGeoDataset, ByVal ShapefilePath As String, ByVal shapefilename As String) As BA_ReturnCode
        Dim pFClass As IFeatureClass
        Dim pWS As IFeatureWorkspace
        Dim pWSF As IWorkspaceFactory = New ShapefileWorkspaceFactory

        ' Create ConversionOP
        Dim pConversionOp As IConversionOp = New RasterConversionOp
        Try
            'verfify input
            If InRaster Is Nothing Then Return BA_ReturnCode.ReadError
            If Len(ShapefilePath) * Len(shapefilename) = 0 Then Return BA_ReturnCode.ReadError
            Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(ShapefilePath)
            'There is a bug with the RasterConversionOp.RasterDataToLineFeatureData that it can only output a shapefile
            'so it doesn't work with file geodatabase
            If wType = WorkspaceType.Geodatabase Then
                MessageBox.Show("Unable to convert raster to line. This function does not work with the file geodatabase.", "Unable to convert raster", _
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return BA_ReturnCode.ReadError
            End If

            ' set workspace
            pWS = pWSF.OpenFromFile(ShapefilePath, 0)

            ' Calls function to open a feature class from disk
            pFClass = pConversionOp.RasterDataToLineFeatureData(InRaster, pWS, shapefilename, True, True, 0)
            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_Raster2LineShapefile Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            pFClass = Nothing
            pWS = Nothing
            pWSF = Nothing
            pConversionOp = Nothing
        End Try
    End Function

    'Deletes a field from a feature class if it exists
    'The feature class can be from a file geodatabase or a shapefile
    Public Function BA_DeleteFieldFromFeatureClass(ByVal folderPath As String, ByVal fileName As String, _
                                                   ByVal fieldName As String) As BA_ReturnCode
        Dim fClass As IFeatureClass
        Dim delField As IField
        Try
            Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(folderPath)
            If workspaceType = BAGIS_ClassLibrary.WorkspaceType.Geodatabase Then
                fClass = BA_OpenFeatureClassFromGDB(folderPath, fileName)
            Else
                fClass = BA_OpenFeatureClassFromFile(BA_StandardizePathString(folderPath), BA_StandardizeShapefileName(fileName, False))
            End If
            If fClass IsNot Nothing Then
                Dim idxDelete As Integer = fClass.FindField(fieldName)
                If idxDelete > -1 Then
                    delField = fClass.Fields.Field(idxDelete)
                    fClass.DeleteField(delField)
                    Return BA_ReturnCode.Success
                End If
            End If
            Return BA_ReturnCode.UnknownError
        Catch ex As Exception
            Debug.Print("BA_DeleteFieldFromFeatureClass Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            fClass = Nothing
            delField = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    'Creates a zip file from the contents of a specified folder
    'The name of the zip file is provided as an argument
    Public Function BA_ZipFolder(ByVal sourceFilePath As String, ByVal zipFileName As String) As BA_ReturnCode
        Dim pWSF As IWorkspaceFactory = New ShapefileWorkspaceFactory
        Dim pWorkspace As IWorkspace = Nothing
        Dim pEnumDSName As IEnumDatasetName = Nothing
        Dim pDSName As IDatasetName = Nothing

        Try
            'Create the archive
            Dim archive As ESRI.ArcGIS.esriSystem.IZipArchive = New ESRI.ArcGIS.esriSystem.ZipArchive
            Dim parentFolder As String = "PleaseReturn"
            Dim tempFileName As String = BA_GetBareName(sourceFilePath, parentFolder)
            archive.CreateArchive(parentFolder & zipFileName)
            'Add Each of the files in the directory
            Dim files() As String = Directory.GetFiles(sourceFilePath)
            For Each nFile As String In files
                archive.AddFile(nFile)
            Next
            'Close the archive
            archive.CloseArchive()
            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_ZipFolder Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        End Try
    End Function

    Public Function BA_StandardizeEsriUnits(ByVal units As esriUnits) As String
        Dim strUnits = units.ToString
        If Microsoft.VisualBasic.Left(strUnits, 4) = "esri" Then
            strUnits = strUnits.Remove(0, Len("esri"))
        End If
        Return strUnits
    End Function

    'http://forums.arcgis.com/threads/17397-Add-In-embedded-Files-and-use-it-s-path?p=327951#post327951
    Public Function BA_GetAddInDirectory() As String
        Try
            Dim codeBase As String = System.Reflection.Assembly.GetExecutingAssembly.CodeBase
            Dim uriBuilder As UriBuilder = New UriBuilder(codeBase)
            Dim path As String = Uri.UnescapeDataString(uriBuilder.Path)
            Return System.IO.Path.GetDirectoryName(path)
        Catch ex As Exception
            Debug.Print("BA_GetAddInDirectory Exception:" & ex.Message)
            Return Nothing
        End Try
    End Function

    ' Calculate raster cellsize from an input GeoDataset
    Public Function BA_CellSize(ByVal folderPath As String, ByVal fileName As String) As Double
        Dim pGeoDataset As IGeoDataset = Nothing
        Dim pRasterDataset As IRasterDataset = New RasterDataset
        Dim pRasterBand As IRasterBand = Nothing
        Dim pRasterBandCollection As IRasterBandCollection = Nothing
        Dim pRasterP As IRasterProps = Nothing
        Try
            ' Open IGeodataset
            Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(folderPath)
            If wType = WorkspaceType.Geodatabase Then
                pGeoDataset = BA_OpenRasterFromGDB(folderPath, fileName)
            Else
                pGeoDataset = BA_OpenRasterFromFile(folderPath, fileName)
            End If
            ' Extract IRasterBand from input IGeoDataSet
            pRasterDataset = CType(pGeoDataset, IRasterDataset) ' Explicit cast
            pRasterBandCollection = pRasterDataset
            pRasterBand = pRasterBandCollection.Item(0)

            ' Get resolution from IRasterBand and calculate cellsize
            pRasterP = pRasterBand
            Dim pPnt As IPnt = New DblPnt
            pPnt = pRasterP.MeanCellSize
            Return (pPnt.X + pPnt.Y) / 2
        Catch ex As Exception
            MsgBox("BA_CellSize Exception: " & ex.Message)
            Return 0
        Finally
            pGeoDataset = Nothing
            pRasterDataset = Nothing
            pRasterBand = Nothing
            pRasterBandCollection = Nothing
            pRasterP = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

End Module
