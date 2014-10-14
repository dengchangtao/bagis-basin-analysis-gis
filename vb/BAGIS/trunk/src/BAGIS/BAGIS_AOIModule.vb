Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesFile
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.SpatialAnalystTools
Imports ESRI.ArcGIS.Geoprocessor
Imports ESRI.ArcGIS.AnalysisTools
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.DataSourcesGDB
Imports ESRI.ArcGIS.DataSourcesRaster
Imports System.Windows.Forms

Module BAGIS_AOIModule
    'return value:
    '   -1: error occurred
    ' otherwise, the value indicate the number of records updated
    'the function adds new fields to hold elevation and name data in the clipped snotel and snow course layers
    'the conversion_factor convert elevation unit to meters
    Public Sub BA_ReadPPAttributes(ByVal AOIPath As String, ByRef shapearea As Double, ByRef shapeareaunit As String, _
        ByRef refarea As Double, ByRef refareaunit As String)
        'fields to be added to the pour point shapefile
        'Public Const BA_AOIShapeAreaField = "AOISHPAREA"
        'Public Const BA_AOIShapeUnitField = "AOISHPUNIT"
        'Public Const BA_AOIRefAreaField = "AOIREFAREA"
        'Public Const BA_AOIRefUnitField = "AOIREFUNIT"
        'AOISHPAREA and AOIREFAREA - FieldTypeDouble: store area of AOI
        'AOISHPUNIT and AOIREFUNIT - esriFieldTypeString: for area unit, AOISHPAREA always has a unit in SQ KM

        Dim File_Name As String

        File_Name = "pourpoint"
        Dim filepath As String = AOIPath & "\" & BA_EnumDescription(GeodatabaseNames.Aoi)
        Debug.Print(filepath)

        shapearea = 0
        shapeareaunit = "Unknown"
        refarea = 0
        refareaunit = "Unknown"

        'check if input Geodatabase exists
        'If Not BA_Shapefile_Exists(File_Name) Then Exit Sub
        If Not BA_Workspace_Exists(filepath) Then
            Dim notExist As String = "The specified Geodatabase does not exist"
            MsgBox(notExist)
            Exit Sub
        End If

        ' Open the folder to contain the shapefile as a workspace
        Dim pFeatureClass As IFeatureClass = Nothing
        Dim pWorkspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(filepath)
        If pWorkspaceType = WorkspaceType.Geodatabase Then
            pFeatureClass = BA_OpenFeatureClassFromGDB(filepath, File_Name)
        ElseIf pWorkspaceType = WorkspaceType.Raster Then
            File_Name = BA_StandardizeShapefileName(File_Name, False, False)
            pFeatureClass = BA_OpenFeatureClassFromFile(filepath, File_Name) 'filename cannot have .shp extension, the sub appends it
        End If

        'Dim pFWS As IFeatureWorkspace
        'Dim pWorkspaceFactory As IWorkspaceFactory = New FileGDBWorkspaceFactory

        'pWks = pWorkspaceFactory.OpenFromFile(filepath, 0)
        'If pWks.NameExists(esriDatasetType.esriDTFeatureClass, "pourpoint") Then
        '    pFClass = pWks.OpenFeatureClass("pourpoint")
        'Else
        '    MsgBox("The specified file name does not exist")
        '    Exit Sub
        'End If

        Dim pFCursor As IFeatureCursor
        Dim pFeature As IFeature

        Try
            ' check if field exist
            'read value
            ' Get field index again
            Dim FI1 As Integer, FI2 As Integer, FI3 As Integer, FI4 As Integer

            FI1 = pFeatureClass.FindField(BA_AOIShapeAreaField)
            FI2 = pFeatureClass.FindField(BA_AOIShapeUnitField)
            FI3 = pFeatureClass.FindField(BA_AOIRefAreaField)
            FI4 = pFeatureClass.FindField(BA_AOIRefUnitField)

            pFCursor = pFeatureClass.Search(Nothing, False)
            pFeature = pFCursor.NextFeature

            If Not pFeature Is Nothing Then 'there is only one feature in a pour point shapefile
                shapearea = pFeature.Value(FI1) 'shape area in sq miles
                shapeareaunit = pFeature.Value(FI2) 'shape area unit, sq miles
                refarea = pFeature.Value(FI3) 'reference area
                refareaunit = pFeature.Value(FI4) 'reference area unit, specified by the user
                'Debug.Print("shapearea: " & shapearea)
                'Debug.Print("shapeareaunit: " & shapeareaunit)
                'Debug.Print("refarea: " & refarea)
                'Debug.Print("refareaunit: " & refareaunit)
            End If
        Catch ex As Exception
            MsgBox(" Exception: " & ex.Message)

        Finally
            pFCursor = Nothing
            pFeature = Nothing
            pFeatureClass = Nothing
        End Try

    End Sub

    'Public Function BA_ClipAOIRaster2(ByVal AOIFolder As String, ByVal InputRaster As String, ByVal OutputRasterName As String, _
    '                                  ByVal outputFolder As String, ByVal AOIClipKey As AOIClipFile) As Short
    '    'prepare for data clipping
    '    'get vector clipping mask, raster clipping mask is created earlier, i.e., pWaterRDS
    '    Dim return_value As Short = 0
    '    Dim Data_Path As String = ""
    '    Dim Data_Name As String
    '    Dim OutputName As String
    '    Dim ClipShapeFile As String = Nothing
    '    Dim pClipFCursor As IFeatureCursor
    '    Dim pClipFeature As IFeature
    '    Dim pGeo As IGeometry
    '    Dim pAOIEnvelope As IEnvelope
    '    Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
    '    Dim tool As ESRI.ArcGIS.DataManagementTools.Clip = New ESRI.ArcGIS.DataManagementTools.Clip()
    '    Dim buildTool As ESRI.ArcGIS.DataManagementTools.BuildRasterAttributeTable = Nothing

    '    If String.IsNullOrEmpty(InputRaster) Then
    '        Return -3
    '    End If

    '    Data_Name = BA_GetBareName(InputRaster, Data_Path)

    '    Dim pClipFeatureLayer As IFeatureLayer = New FeatureLayer
    '    Dim pClipFClass As IFeatureClass
    '    Try
    '        ClipShapeFile = BA_EnumDescription(AOIClipKey)

    '        Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(outputFolder)
    '        If Not BA_Folder_ExistsWindowsIO(outputFolder) Then
    '            Throw New Exception("Output geodatabase folder " + outputFolder + " does not exist.")
    '        End If
    '        If String.IsNullOrEmpty(OutputRasterName) Then 'user didn't specify an output name
    '            OutputName = outputFolder & "\" & Data_Name
    '        Else
    '            If workspaceType = BAGIS_Addin_ClassLibrary.WorkspaceType.Raster And OutputRasterName.Length > BA_GRID_NAME_MAX_LENGTH Then
    '                Throw New Exception("Output raster name cannot exceed " + CStr(BA_GRID_NAME_MAX_LENGTH) + " characters")
    '            End If
    '            OutputName = outputFolder & "\" & OutputRasterName
    '        End If

    '        'check if a layer of the same name exists in the AOI
    '        If BA_File_Exists(OutputName, workspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
    '            Return -2
    '        End If

    '        'checking overlap between input and clip layers
    '        Dim pClipFClassPath As String = BA_GeodatabasePath(AOIFolder, GeodatabaseNames.Aoi, True) & ClipShapeFile
    '        pClipFClass = BA_OpenFeatureClassFromGDB(BA_GeodatabasePath(AOIFolder, GeodatabaseNames.Aoi), ClipShapeFile)
    '        'retrieve IFeature from FeatureClass
    '        pClipFeatureLayer.FeatureClass = pClipFClass
    '        pClipFCursor = pClipFeatureLayer.Search(Nothing, False)
    '        pClipFeature = pClipFCursor.NextFeature
    '        pGeo = pClipFeature.Shape
    '        'copy envelope rectangle to string variable
    '        pAOIEnvelope = pGeo.Envelope
    '        Dim urx, llx, lly, ury As Double
    '        pAOIEnvelope.QueryCoords(llx, lly, urx, ury)
    '        Dim EnvBox_Text As String = llx & " " & lly & " " & urx & " " & ury

    '        ' Configure the geoprocessor tool.
    '        tool.in_raster = InputRaster ' file name for the input raster
    '        tool.rectangle = EnvBox_Text ' four coordinates defining the minimum bounding rectangle to be clipped
    '        tool.out_raster = OutputName ' file name for output raster dataset
    '        'tool.out_raster = AOIFolder & "\" & tmpOutputName  'temporarily save to aoi root as workaround
    '        tool.in_template_dataset = pClipFClassPath
    '        tool.clipping_geometry = "ClippingGeometry"  ' clip the raster to the boundary of the aoi_b vector
    '        GP.AddOutputsToMap = False
    '        Dim res As Object = GP.Execute(tool, Nothing)
    '        Return 1
    '        'If res Is Nothing Then
    '        '    'Clip did not complete successfully
    '        '    Return 0
    '        'Else
    '        '    'If source and target are both in a File GDB, need to rebuild the attribute
    '        '    'table. Otherwise it will be corrupted if source and target GDB are different
    '        '    If BA_GetWorkspaceTypeFromPath(InputRaster) = workspaceType.Geodatabase And _
    '        '        BA_GetWorkspaceTypeFromPath(OutputName) = workspaceType.Geodatabase Then
    '        '        buildTool = New ESRI.ArcGIS.DataManagementTools.BuildRasterAttributeTable
    '        '        buildTool.in_raster = OutputName
    '        '        buildTool.overwrite = False
    '        '        res = GP.Execute(buildTool, Nothing)
    '        '        If res IsNot Nothing Then
    '        '            Return 1
    '        '        End If
    '        '        Return 0
    '        '    Else
    '        '        Return 1
    '        '    End If
    '        'End If
    '    Catch ex As Exception
    '        If GP.MessageCount > 0 Then
    '            MessageBox.Show("Geoprocessor error: " + GP.GetMessages(Type.Missing))
    '        Else
    '            MessageBox.Show("Exception: " + ex.Message)
    '        End If
    '        Return -1
    '    Finally
    '        pClipFClass = Nothing
    '        pClipFeatureLayer = Nothing
    '        pClipFCursor = Nothing
    '        pClipFeature = Nothing
    '        pGeo = Nothing
    '        pAOIEnvelope = Nothing
    '        tool = Nothing
    '        GP = Nothing
    '    End Try
    'End Function

    ' '' ''Public Sub BA_GetDEMStatsGDB(ByVal AOIPath As String, ByRef DEMMin As Double, ByRef DEMMax As Double, _
    ' '' ''    ByRef DEMRange As Double)
    ' '' ''    'Dim response As Double
    ' '' ''    Dim filename As String


    ' '' ''    filename = "dem"
    ' '' ''    Dim gdbpath As String = AOIPath & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces)
    ' '' ''    If String.IsNullOrEmpty(gdbpath) Then
    ' '' ''        Dim notExist As String = "The DEM source Geodatabase does not exist"
    ' '' ''        MsgBox(notExist)
    ' '' ''        Exit Sub
    ' '' ''    End If

    ' '' ''    DEMMin = Nothing
    ' '' ''    DEMMax = Nothing
    ' '' ''    DEMRange = Nothing

    ' '' ''    'From Here
    ' '' ''    Dim pDEUtility As ESRI.ArcGIS.Geoprocessing.IDEUtilities = New ESRI.ArcGIS.Geoprocessing.DEUtilities
    ' '' ''    Dim selGeoDataset As IGeoDataset = Nothing
    ' '' ''    Dim pRasterBandCollection As IRasterBandCollection = Nothing
    ' '' ''    Dim pRasterBand As IRasterBand = Nothing
    ' '' ''    Dim pRasterDataset As IRasterDataset = New RasterDataset  'To be deleted if not used
    ' '' ''    Dim pRasterStats As IRasterStatistics = Nothing
    ' '' ''    Dim pfeaturclass As IFeatureClass = Nothing

    ' '' ''    'Try
    ' '' ''    '1. Open Raster
    ' '' ''    selGeoDataset = BA_OpenRasterFromGDB((gdbpath & "\"), filename)
    ' '' ''    If selGeoDataset IsNot Nothing Then
    ' '' ''        pfeaturclass = TryCast(selGeoDataset, IRasterStatistics)
    ' '' ''        Dim fieldname As String = "Pixel value"
    ' '' ''        pRasterStats = pfeaturclass.FindField(fieldname)

    ' '' ''    End If

    ' '' ''    '2. QI Raster to IRasterBand

    ' '' ''    'Dim validVAT As Boolean = False
    ' '' ''    'pRasterBand.HasTable(validVAT)
    ' '' ''    ' If validVAT = False Then



    ' '' ''    pRasterStats = pRasterBand.Statistics
    ' '' ''    If pRasterStats IsNot Nothing Then
    ' '' ''        MsgBox(Format(pRasterStats.Minimum, "######0.##"))
    ' '' ''        DEMMin = pRasterStats.Minimum
    ' '' ''    End If
    ' '' ''    'End If



    ' '' ''    'Dim pFeatureClass As IFeatureClass = Nothing
    ' '' ''    'Dim pWorkspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(gdbpath)
    ' '' ''    'If pWorkspaceType = WorkspaceType.Raster Then
    ' '' ''    '    pFeatureClass = BA_OpenFeatureClassFromGDB(gdbpath, filename)
    ' '' ''    'End If


    ' '' ''    'Try
    ' '' ''    '    '1. Open Raster
    ' '' ''    '    'pWS = pWSF.OpenFromFile(gdbpath, 0)
    ' '' ''    '    Dim FieldName As String = "Pixel value"
    ' '' ''    '    pRasterStat = pFeatureClass.FindField(FieldName)
    ' '' ''    '    DEMMin = pRasterStat.Minimum(filename)

    ' '' ''    '    '2. QI Raster to IRasterBand

    ' '' ''    '    pRasterBandCollection = pRasterDataset
    ' '' ''    '    pRasterBand = pRasterBandCollection.Item(0)

    ' '' ''    '    'QI IRasterProps
    ' '' ''    '    Dim pRasterP As IRasterProps = pRasterBand
    ' '' ''    '    Dim pPnt As IPnt = New DblPnt
    ' '' ''    '    pPnt = pRasterP.MeanCellSize
    ' '' ''    '    rasterresolution = (pPnt.X + pPnt.Y) / 2
    ' '' ''    '    Return pRasterBand.Statistics
    ' '' ''    'Catch ex As Exception
    ' '' ''    '    rasterresolution = 0
    ' '' ''    '    MessageBox.Show("Exception: " + ex.Message)
    ' '' ''    '    Return Nothing
    ' '' ''    'Finally
    ' '' ''    '    pRasterBand = Nothing
    ' '' ''    '    pRasterBandCollection = Nothing
    ' '' ''    '    pRasterDataset = Nothing
    ' '' ''    '    pWS = Nothing
    ' '' ''    '    pWSF = Nothing
    ' '' ''    '    pDEUtility.ReleaseInternals()
    ' '' ''    'End Try

    ' '' ''    'To Here

    ' '' ''    ' Open the Geodatabase containing the DEM raster as a workspace

    ' '' ''End Sub
    'return value:
    '   -1: error occurred
    ' otherwise, the value indicate the number of records updated
    'the function adds new fields to hold elevation and name data in the clipped snotel and snow course layers
    'the conversion_factor convert elevation unit to meters

    Public Function BA_UpdatePPAttributes(ByVal AOIPath As String) As Integer
        'fields to be added to the pour point shapefile
        'Public Const BA_AOIShapeAreaField = "AOISHPAREA"
        'Public Const BA_AOIShapeUnitField = "AOISHPUNIT"
        'Public Const BA_AOIRefAreaField = "AOIREFAREA"
        'Public Const BA_AOIRefUnitField = "AOIREFUNIT"

        'AOISHPAREA and AOIREFAREA - FieldTypeDouble: store area of AOI
        'AOISHPUNIT and AOIREFUNIT - esriFieldTypeString: for area unit, AOISHPAREA always has a unit in SQ KM

        Dim comReleaser As ESRI.ArcGIS.ADF.ComReleaser = New ESRI.ArcGIS.ADF.ComReleaser
        Dim return_value As Integer = 0
        Dim File_Name As String = AOIPath & "\" & BA_POURPOINTCoverage

        If Not BA_File_Exists(File_Name, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            MsgBox(BA_POURPOINTCoverage & " does not exist")
            Return return_value
        End If

        Dim pFClass As IFeatureClass = BA_OpenFeatureClassFromGDB(AOIPath, BA_POURPOINTCoverage)
        comReleaser.ManageLifetime(pFClass)

        'add fields
        Dim FieldIndex As Integer = 0
        Dim pFld As IFieldEdit2 = Nothing

        Dim FName(7) As String
        FName(1) = BA_AOIShapeAreaField
        FName(2) = BA_AOIShapeUnitField
        FName(3) = BA_AOIRefAreaField
        FName(4) = BA_AOIRefUnitField
        FName(5) = "AOI_SQMI"
        FName(6) = "BASIN"
        FName(7) = BA_AOI_IDField 'Ver1E update - this module was modified to handle the extra att field

        Dim pFCursor As IFeatureCursor = Nothing
        Dim pFeature As IFeature = Nothing

        Try
            For i As Integer = 1 To 7
                'check if field exist
                FieldIndex = pFClass.FindField(FName(i))

                ' Define field type
                If FieldIndex < 0 Then 'add field
                    'Define field name
                    pFld = New Field
                    pFld.Name_2 = FName(i)
                    Select Case i
                        Case 1, 3, 5 'area field
                            pFld.Type_2 = esriFieldType.esriFieldTypeDouble
                            pFld.Length_2 = 24
                            pFld.Required_2 = 8
                        Case 2, 4 'area unit field
                            pFld.Type_2 = esriFieldType.esriFieldTypeString
                            pFld.Length_2 = 15
                            pFld.Required_2 = 0
                        Case 6, 7 'basin name, aoi_id
                            pFld.Type_2 = esriFieldType.esriFieldTypeString
                            pFld.Length_2 = 30
                            pFld.Required_2 = 0
                    End Select

                    ' Add field
                    pFClass.AddField(pFld)
                End If
            Next

            'update value
            ' Get field index again
            Dim FI1 As Integer, FI2 As Integer, FI3 As Integer
            Dim FI4 As Integer, FI5 As Integer, FI6 As Integer, FI7 As Integer

            FI1 = pFClass.FindField(FName(1))
            FI2 = pFClass.FindField(FName(2))
            FI3 = pFClass.FindField(FName(3))
            FI4 = pFClass.FindField(FName(4))
            FI5 = pFClass.FindField(FName(5))
            FI6 = pFClass.FindField(FName(6))
            FI7 = pFClass.FindField(FName(7))

            pFCursor = pFClass.Update(Nothing, False)
            comReleaser.ManageLifetime(pFCursor)
            pFeature = pFCursor.NextFeature

            Do While Not pFeature Is Nothing
                pFeature.Value(FI1) = AOI_ShapeArea 'shape area in sq miles
                pFeature.Value(FI2) = AOI_ShapeUnit 'shape area unit, sq miles
                pFeature.Value(FI3) = AOI_ReferenceArea 'reference area
                pFeature.Value(FI4) = AOI_ReferenceUnit 'reference area unit, specified by the user   
                pFeature.Value(FI5) = 0 'no value is provided
                pFeature.Value(FI6) = BA_GetBareName(BasinFolderBase)
                pFeature.Value(FI7) = BA_AOI_Forecast_ID 'the unique id of the forecast point 
                pFCursor.UpdateFeature(pFeature)
                pFeature = pFCursor.NextFeature
                return_value = return_value + 1
            Loop

        Catch ex As Exception

        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeature)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFClass)

        End Try
        Return return_value
    End Function

    'return value
    '-1: unknown error
    '-2: output exists
    '-3: missing parameters
    '-4: no input shapefile
    '0: no intersect between the input and the clip layers
    '1: clipping is done successfully
    Public Function BA_ClipAOISNOTEL(ByVal AOIFolder As String, ByVal InputShapefile_Path_Name As String, ByVal Is_SNOTEL As Boolean) As Integer
        Dim comReleaser As ESRI.ArcGIS.ADF.ComReleaser = New ESRI.ArcGIS.ADF.ComReleaser
        Dim InData_Path As String = "", InData_Name As String
        Dim InputName As String, ClipName As String, ClipShapeFile As String
        Dim OutputName As String, OutputKey As String
        Dim return_value As Integer, response As Integer

        return_value = -3

        If Len(InputShapefile_Path_Name) = 0 Then
            BA_ClipAOISNOTEL = return_value
            Exit Function
        End If

        'set input file name
        InData_Name = BA_GetBareName(InputShapefile_Path_Name, InData_Path)

        If Len(Trim(InData_Name)) = 0 Or Len(Trim(InData_Path)) = 0 Then
            Return return_value
        End If

        'check if the input shapefile has a .shp file extension
        'If UCase(Right(InData_Name, 4)) <> ".SHP" Then InData_Name = InData_Name & ".shp"
        InputName = InData_Path & InData_Name 'GP parameter
        If Not BA_Shapefile_Exists(InputName) Then
            Return -4
        End If

        'set output file name - GP parameter
        If Is_SNOTEL Then
            OutputName = AOIFolder & "\" & BA_EnumDescription(GeodatabaseNames.Layers) & "\" & BA_SNOTELSites
            OutputKey = BA_SNOTELSites
        Else
            OutputName = AOIFolder & "\" & BA_EnumDescription(GeodatabaseNames.Layers) & "\" & BA_SnowCourseSites
            OutputKey = BA_SnowCourseSites
        End If

        'If UCase(Right(OutputName, 4)) <> ".SHP" Then OutputName = OutputName & ".shp"

        ClipShapeFile = BA_BufferedAOIExtentCoverage
        ClipName = AOIFolder & "\" & BA_EnumDescription(GeodatabaseNames.Aoi) & "\" & BA_BufferedAOIExtentCoverage  'GP parameter

        'check if a layer of the same name exists in the AOI
        If BA_File_Exists(OutputName, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            Return -2
        End If

        'clip the input layer
        'prepare for data clipping
        'get vector clipping mask

        Dim pClipFClass As IFeatureClass = BA_OpenFeatureClassFromGDB(AOIFolder & "\" & BA_EnumDescription(GeodatabaseNames.Aoi), ClipShapeFile)
        comReleaser.ManageLifetime(pClipFClass)
        Dim pClipFeatureLayer As IFeatureLayer = New FeatureLayer
        pClipFeatureLayer.FeatureClass = pClipFClass
        'Dim pClipTable As ITable = pClipFeatureLayer
        Dim pClipFCursor As IFeatureCursor = pClipFeatureLayer.Search(Nothing, False)
        comReleaser.ManageLifetime(pClipFCursor)
        Dim pGeo As IGeometry = Nothing
        Dim pSFilter As ISpatialFilter
        Dim pClipFeature As IFeature = Nothing

        Dim pInputFClass As IFeatureClass = BA_OpenFeatureClassFromFile(InData_Path, BA_StandardizeShapefileName(InData_Name, False, False))
        comReleaser.ManageLifetime(pInputFClass)
        Dim pGP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
        Dim pResult As ESRI.ArcGIS.Geoprocessing.IGeoProcessorResult = Nothing
        Dim cliptool As Clip = New Clip

        Try
            return_value = -1
            'get the clip geometry
            pClipFeature = pClipFCursor.NextFeature
            pGeo = pClipFeature.Shape

            'create a spatial filter for checking if the clipped layers are within the clip bnd
            pSFilter = New SpatialFilter
            With pSFilter
                .Geometry = pGeo
                .GeometryField = "SHAPE"
                .SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects
            End With

            'Open input Shapefile
            Dim pTempFCursor As IFeatureCursor = pInputFClass.Search(pSFilter, False)
            comReleaser.ManageLifetime(pTempFCursor)
            Dim pTempFeature As IFeature = New Feature
            pTempFeature = pTempFCursor.NextFeature

            Dim clipInput As Boolean = False

            If Not pTempFeature Is Nothing Then 'at least one input feature can be clipped
                clipInput = True
            End If

            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTempFCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pInputFClass)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGeo)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pClipFeatureLayer)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pClipFClass)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pClipFCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pClipFeature)

            If clipInput Then
                'Clear Memory
                pGP.OverwriteOutput = True
                pGP.AddOutputsToMap = False
                With cliptool
                    .in_features = InputShapefile_Path_Name & ".shp"
                    .clip_features = ClipName
                    .out_feature_class = OutputName
                    .cluster_tolerance = 0
                End With
                pResult = pGP.Execute(cliptool, Nothing)

                'update snotel attribute table to contain standard attribute info
                Dim ElevFieldName As String
                Dim NameFieldName As String
                Dim unit_factor As Double

                'read the definition file to get the parameter
                BA_SetSettingPath()
                response = BA_ReadBAGISSettings(BA_Settings_Filepath)

                If Is_SNOTEL Then
                    ElevFieldName = BA_SystemSettings.SNOTEL_ElevationField
                    If UCase(BA_SystemSettings.SNOTEL_NameField) <> "NONE" Then
                        NameFieldName = BA_SystemSettings.SNOTEL_NameField
                    Else
                        NameFieldName = ""
                    End If
                    unit_factor = BA_SetConversionFactor(True, BA_SystemSettings.SNOTEL_ZUnit_IsMeter)

                Else
                    ElevFieldName = BA_SystemSettings.SCourse_ElevationField
                    If UCase(BA_SystemSettings.SCourse_NameField) <> "NONE" Then
                        NameFieldName = BA_SystemSettings.SCourse_NameField
                    Else
                        NameFieldName = ""
                    End If
                    unit_factor = BA_SetConversionFactor(True, BA_SystemSettings.SCourse_ZUnit_IsMeter)

                End If

                response = BA_UpdateSiteAttributes(AOIFolder & "\" & BA_EnumDescription(GeodatabaseNames.Layers), OutputKey, ElevFieldName, NameFieldName, unit_factor)

                return_value = 1
            Else
                return_value = 0
            End If

        Catch ex As Exception
            MessageBox.Show("BA_ClipAOISNOTEL() Exception: " & ex.Message)

        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGP)
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
        Return return_value
    End Function

    ''return a conversion factor based on reference and data units
    ''unit value 1: meters, 2: feet
    ''if ref_unit equals data_unit, then conversion factor = 1
    ''if ref_unit = 1 and data_unit = 2 then conversion factor = 0.3048 (i.e., feet to meters)
    ''if ref_unit = 2 and data_unit = 1 then conversion factor = 3.2808399 (i.e., meters to feet)
    'Public Function BA_SetConversionFactor(ByVal ref_unit_in_meter As Boolean, ByVal data_unit_in_meter As Boolean) As Double
    '    If ref_unit_in_meter Then 'dem is in meters
    '        If data_unit_in_meter Then 'display unit matches dem unit
    '            BA_SetConversionFactor = 1
    '        Else 'display unit is feet and dem unit is meters (convert feet to meters)
    '            BA_SetConversionFactor = 0.3048
    '        End If
    '    Else 'dem unit is in feet
    '        If Not data_unit_in_meter Then 'display unit matches dem unit
    '            BA_SetConversionFactor = 1
    '        Else 'display unit is feet and dem unit is meters (convert feet to meters)
    '            BA_SetConversionFactor = 3.2808399
    '        End If
    '    End If
    'End Function

    'return value:
    '   -1: error occurred
    ' otherwise, the value indicate the number of records updated
    'the function adds new fields to hold elevation and name data in the clipped snotel and snow course layers
    'the conversion_factor convert elevation unit to meters
    Public Function BA_UpdateSiteAttributes(ByVal File_Path As String, ByVal File_Name As String, ByVal Elev_Field As String, ByVal Name_Field As String, ByVal Conversion_Factor As Double) As Integer
        'fields to be added
        'AOI snotel and snow course elevation and name field
        'Public Const BA_SiteNameField = "BA_SNAME"
        'Public Const BA_SiteElevField = "BA_SELEV"

        'BA_SNAME - esriFieldTypeString: for labeling purpose
        'BA_SELEV - esriFieldTypeDouble: for elevation unit in meters
        Dim comReleaser As ESRI.ArcGIS.ADF.ComReleaser = New ESRI.ArcGIS.ADF.ComReleaser
        Dim return_value As Integer
        Dim response As Integer = Nothing
        return_value = 0

        'check if input file exists
        If Not BA_File_Exists(File_Path & "\" & File_Name, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            Return return_value
        End If

        ' Open the folder to contain the shapefile as a workspace
        Dim pFClass As IFeatureClass = BA_OpenFeatureClassFromGDB(File_Path, File_Name)
        comReleaser.ManageLifetime(pFClass)

        'add Name field
        ' check if field exist
        Dim FieldIndex As Integer
        Dim pFld As IFieldEdit
        Dim FName(2) As String

        FName(1) = BA_SiteElevField
        FName(2) = BA_SiteNameField
        Dim i As Integer

        For i = 1 To 2
            FieldIndex = pFClass.FindField(FName(i))

            ' Define field type
            If FieldIndex < 0 Then 'add field
                'Define field name
                pFld = New Field
                pFld.Name_2 = FName(i)
                Select Case i
                    Case 1 'Elevation field
                        pFld.Type_2 = esriFieldType.esriFieldTypeDouble
                        pFld.Length_2 = 24
                        pFld.Required_2 = 8
                    Case 2 'Name field
                        pFld.Type_2 = esriFieldType.esriFieldTypeString
                        pFld.Length_2 = 50
                        pFld.Required_2 = 0
                End Select

                ' Add field
                pFClass.AddField(pFld)
            End If
        Next

        'update value
        ' Get field index again
        Dim FI1 As Integer, FI2 As Integer
        Dim FIInput1 As Integer, FIInput2 As Integer

        FI1 = pFClass.FindField(FName(1))
        FI2 = pFClass.FindField(FName(2))
        FIInput1 = pFClass.FindField(Elev_Field)
        If Len(Name_Field) > 0 Then FIInput2 = pFClass.FindField(Name_Field)

        'Ver1E update - error handling
        If FI1 < 0 Or FI2 < 0 Or FIInput1 < 0 Or FIInput2 < 0 Then
            MsgBox("Attribute field(s) were not found in the input!")
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFClass)
            Return -1
        End If

        Dim pFCursor As IFeatureCursor = pFClass.Update(Nothing, False)
        comReleaser.ManageLifetime(pFCursor)
        Dim pFeature As IFeature = pFCursor.NextFeature

        Try
            Do While Not pFeature Is Nothing
                pFeature.Value(FI1) = pFeature.Value(FIInput1) * Conversion_Factor 'elevation conversion
                If Len(Name_Field) > 0 Then
                    pFeature.Value(FI2) = pFeature.Value(FIInput2)
                Else
                    pFeature.Value(FI2) = "Unknown"
                End If
                pFCursor.UpdateFeature(pFeature)
                pFeature = pFCursor.NextFeature
                return_value = return_value + 1
            Loop
            Return return_value

        Catch ex As Exception
            MsgBox("BA_UpdateSiteAttributes Exception: " & ex.Message)
            Return -1

        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeature)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFClass)
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    'extract a value from an integer raster and save the result to the output as a value specified by the user
    Public Function BA_ExtractRasterbyValue(ByVal inputRasterPathName As String, ByVal cellValue As Long, ByVal outputPathName As String, outputValue As Integer) As BA_ReturnCode
        Dim contool As Con = New Con
        Dim pGP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
        Dim pResult As ESRI.ArcGIS.Geoprocessing.IGeoProcessorResult = Nothing

        Try
            pGP.OverwriteOutput = True
            pGP.AddOutputsToMap = False
            With contool
                .in_conditional_raster = inputRasterPathName
                .in_true_raster_or_constant = outputValue
                .out_raster = outputPathName
                .where_clause = "Value = " & cellValue
            End With
            pResult = pGP.Execute(contool, Nothing)
            If pResult Is Nothing Then
                Return BA_ReturnCode.OtherError
            Else
                Return BA_ReturnCode.Success
            End If
        Catch ex As Exception
            MsgBox("BA_ExtractRasterbyValue error: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            pGP = Nothing
            contool = Nothing
        End Try
    End Function
End Module

