Option Strict Off
Option Explicit On

Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.DataSourcesFile
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geoprocessing
Imports ESRI.ArcGIS.GeoAnalyst
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework

Imports System.Windows.Forms
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.CartoUI
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.SpatialAnalyst
Imports ESRI.ArcGIS.DataSourcesGDB

Public Module AOIModule

    ' Add name, lbound, ubound attributes to raster. These attributes are used when displaying/symbolizing the raster
    Public Function BA_ReadReclassRasterAttributeGDB(ByRef IntervalList() As BA_IntervalList, ByRef filepath As String, _
                                                     ByRef FileName As String) As BA_ReturnCode
        'fields to be read:
        'NAME - esriFieldTypeString: for labeling purpose
        'LBOUND - esriFieldTypeDouble: for upper bound
        'UBOUND - esriFieldTypeDouble: for lower bound

        Dim raster_res As Double
        Dim pRDataset As IGeoDataset = Nothing
        Dim pBandCol As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pTable As ITable = Nothing
        Dim pCursor As ICursor = Nothing
        Dim pRow As IRow = Nothing

        Try
            'call to get cellsize;
            raster_res = BA_CellSize(filepath, FileName)

            If raster_res <= 0 Then raster_res = 1
            Dim cell_size As Double = raster_res * raster_res

            'open raster attribute table
            'pRDataset = BA_OpenRasterFromFile(filepath, FileName)
            pRDataset = BA_OpenRasterFromGDB(filepath, FileName)
            pBandCol = CType(pRDataset, IRasterBandCollection)
            pRasterBand = pBandCol.Item(0)
            pTable = pRasterBand.AttributeTable

            'count the number of classes in raster
            pCursor = pTable.Search(Nothing, False)
            Dim nclass As Short = pTable.RowCount(Nothing)
            ReDim IntervalList(nclass)

            ' check if field exist
            Dim i As Short

            'read value
            ' Get field index again
            Dim FI3, FI1, FI0, FI2, FI4 As Short
            FI0 = pTable.FindField(BA_FIELD_VALUE)
            FI1 = pTable.FindField(BA_FIELD_NAME)
            FI2 = pTable.FindField(BA_FIELD_LBOUND)
            FI3 = pTable.FindField(BA_FIELD_UBOUND)
            FI4 = pTable.FindField(BA_FIELD_COUNT)

            pRow = pCursor.NextRow

            For i = 1 To nclass
                IntervalList(i).Value = pRow.Value(FI0)
                Dim strDebug As String = ""
                If FI1 > 0 Then 'Name
                    IntervalList(i).Name = pRow.Value(FI1)
                    strDebug = pRow.Value(FI1) + " | "
                Else
                    IntervalList(i).Name = BA_UNKNOWN
                End If
                If FI2 > 0 Then
                    IntervalList(i).LowerBound = pRow.Value(FI2)
                    strDebug = CStr(pRow.Value(FI2)) + " | "
                End If
                If FI3 > 0 Then
                    IntervalList(i).UpperBound = pRow.Value(FI3)
                    strDebug = CStr(pRow.Value(FI3)) + " | "
                End If
                If FI4 > 0 Then
                    IntervalList(i).Area = pRow.Value(FI4) * cell_size
                    strDebug = CStr(IntervalList(i).Area) + vbCrLf
                End If
                Console.WriteLine(strDebug)
                pRow = pCursor.NextRow
            Next
            Return BA_ReturnCode.Success
        Catch ex As Exception
            MessageBox.Show("BA_ReadReclassRasterAttribute Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRow)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTable)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pBandCol)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBand)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRDataset)

        End Try


    End Function

    'Not used as of June 15, 2011 LCB
    'this version could prevent the problem of not able to delete a raster
    'check if the save target location exists first. If it exists, remove it
    'return value: 0 error occurred
    '              1 successfully saved the raster
    'Public Function BA_ReClassRaster_Save(ByVal IntervalList() As BA_IntervalList, ByVal RasterToReclass As IGeoDataset, ByVal SavePath As String, ByVal SaveName As String) As Short
    '    Dim NumOfClasses As Integer = UBound(IntervalList)
    '    Dim i As Short
    '    Dim pNumberReMap As INumberRemap = New NumberRemap
    '    Dim pRasterGD As IGeoDataset

    '    Try
    '        'Configure Remap
    '        For i = 1 To NumOfClasses
    '            pNumberReMap.MapRange(CDbl(IntervalList(i).LowerBound), CDbl(IntervalList(i).UpperBound), CInt(IntervalList(i).Value))
    '        Next

    '        'Reclass by Remap
    '        Dim pReclassOp As IReclassOp = New RasterReclassOp
    '        pRasterGD = pReclassOp.ReclassByRemap(RasterToReclass, pNumberReMap, False)

    '        'Persist reclassed raster
    '        Return BA_SaveRasterDataset(pRasterGD, SavePath, SaveName)
    '    Catch ex As Exception
    '        Dim messagetext As String = "INTERVAL RANGE ERROR!"
    '        For i = 1 To NumOfClasses
    '            messagetext = messagetext & vbCrLf & IntervalList(i).Value & ": " & IntervalList(i).LowerBound & " - " & IntervalList(i).UpperBound
    '        Next
    '        MessageBox.Show(messagetext)
    '        MessageBox.Show("Exception: " + ex.Message)
    '        Return -1
    '    Finally
    '        pRasterGD = Nothing
    '        pNumberReMap = Nothing
    '    End Try
    'End Function

    ' Not used as of June 15, 2011
    'return value:
    ' -1: error occurred
    ' otherwise, the value indicate the number of records updated
    'Public Function BA_UpdateReclassRasterAttributes(ByRef IntervalList() As BA_IntervalList, ByRef filepath As String, ByRef FileName As String) As Short
    '    'fields to be added
    '    'NAME - esriFieldTypeString: for labeling purpose
    '    'LBOUND - esriFieldTypeDouble: for upper bound
    '    'UBOUND - esriFieldTypeDouble: for lower bound
    '    Dim return_value As Short = 0

    '    'open raster attribute table
    '    Dim pRDataset As IRasterDataset = BA_OpenRasterFromFile(filepath, FileName)
    '    Dim pBandCol As IRasterBandCollection = pRDataset
    '    Dim pRasterBand As IRasterBand = pBandCol.Item(0)
    '    Dim pTable As ITable = pRasterBand.AttributeTable
    '    Dim pQFilter As IQueryFilter = New QueryFilter

    '    'add fields
    '    Try
    '        Dim nclass As Short = UBound(IntervalList)

    '        'add Name field
    '        ' check if field exist
    '        Dim FieldIndex As Short
    '        Dim pField As IField = New FieldClass()
    '        Dim pFld As IFieldEdit2 = CType(pField, IFieldEdit2)
    '        Dim FName(3) As String
    '        Dim i As Short

    '        FName(1) = BA_FIELD_NAME
    '        FName(2) = BA_FIELD_LBOUND
    '        FName(3) = BA_FIELD_UBOUND
    '        For i = 1 To 3
    '            FieldIndex = pTable.FindField(FName(i))

    '            ' Define field type
    '            If FieldIndex < 0 Then 'add field
    '                'Define field name
    '                pFld.Name_2 = FName(i)
    '                Select Case i
    '                    Case 1 'Name field
    '                        pFld.Type_2 = esriFieldType.esriFieldTypeString
    '                        pFld.Length_2 = BA_NAME_FIELD_WIDTH
    '                        pFld.Required_2 = False
    '                    Case 2, 3 'Ubound and LBound field
    '                        pFld.Type_2 = esriFieldType.esriFieldTypeDouble
    '                        pFld.Length_2 = BA_BOUND_FIELD_WIDTH
    '                        pFld.Required_2 = False
    '                End Select
    '                ' Add field
    '                pTable.AddField(pFld)
    '            End If
    '        Next

    '        'update value
    '        ' Get field index again
    '        Dim FI2, FI1, FI3 As Short

    '        FI1 = pTable.FindField(FName(1))
    '        FI2 = pTable.FindField(FName(2))
    '        FI3 = pTable.FindField(FName(3))

    '        Dim pCursor As ICursor
    '        Dim pRow As IRow
    '        Dim tempname As String

    '        For i = 1 To nclass
    '            pQFilter.WhereClause = BA_FIELD_VALUE & " = " & CInt(IntervalList(i).Value)
    '            pCursor = pTable.Update(pQFilter, False)
    '            pRow = pCursor.NextRow

    '            Do While Not pRow Is Nothing
    '                tempname = Trim(IntervalList(i).Name)
    '                If Len(tempname) >= BA_NAME_FIELD_WIDTH Then 'truncate the string if it's longer than the att field width
    '                    tempname = Left(tempname, BA_NAME_FIELD_WIDTH - 1)
    '                End If
    '                pRow.Value(FI1) = tempname
    '                pRow.Value(FI2) = IntervalList(i).LowerBound
    '                pRow.Value(FI3) = IntervalList(i).UpperBound
    '                pCursor.UpdateRow(pRow)
    '                pRow = pCursor.NextRow
    '                return_value = return_value + 1
    '            Loop

    '            pRow = Nothing
    '            pCursor = Nothing
    '        Next
    '        Return return_value
    '    Catch ex As Exception
    '        MessageBox.Show("Exception: " + ex.Message)
    '        return_value = -1
    '        Return return_value
    '    Finally
    '        pTable = Nothing
    '        pRasterBand = Nothing
    '        pBandCol = Nothing
    '        pRDataset = Nothing
    '        pQFilter = Nothing
    '    End Try
    'End Function

    'Not used as of June 15, 2011
    'return a conversion factor based on reference and data units
    'unit value 1: meters, 2: feet
    'if ref_unit equals data_unit, then conversion factor = 1
    'if ref_unit = 1 and data_unit = 2 then conversion factor = 0.3048 (i.e., feet to meters)
    'if ref_unit = 2 and data_unit = 1 then conversion factor = 3.2808399 (i.e., meters to feet)
    Public Function BA_SetConversionFactor(ByRef ref_unit_in_meter As Boolean, ByRef data_unit_in_meter As Boolean) As Double
        If ref_unit_in_meter Then 'dem is in meters
            If data_unit_in_meter Then 'display unit matches dem unit
                Return 1
            Else 'data unit is feet and dem unit is meters (convert feet to meters)
                Return BA_FEET_TO_METERS
            End If
        Else 'dem unit is in feet
            If Not data_unit_in_meter Then 'display unit matches dem unit
                Return 1
            Else 'data unit is meters and dem unit is feet (convert meters to feet)
                Return BA_METERS_TO_FEET
            End If
        End If
    End Function

    'return value
    '-1: unknown error
    '-2: output exists
    '-3: missing parameters
    '0: no intersect between the input and the clip layers
    '1: clipping is done successfully
    '
    'BufferType:
    '0: no buffer - i.e., AOI_V
    '1: standard buffer - i.e., AOIB_V
    '2: PRISM buffer - i.e., P_AOI_V

    ' Clip raster to an AOI vector. Options are no buffer, standard buffer, and PRISM buffer
    Public Function BA_ClipAOIRaster(ByVal AOIFolder As String, ByVal InputRaster As String, ByVal OutputRasterName As String, _
                                     ByVal outputFolder As String, ByVal AOIClipKey As AOIClipFile, Optional ByVal RebuildATT As Boolean = True) As Short
        'prepare for data clipping
        'get vector clipping mask, raster clipping mask is created earlier, i.e., pWaterRDS
        Dim return_value As Short = 0
        Dim Data_Path As String = ""
        Dim Data_Name As String
        Dim OutputName As String
        Dim ClipShapeFile As String = Nothing
        Dim pClipFCursor As IFeatureCursor
        Dim pClipFeature As IFeature
        Dim pGeo As IGeometry
        Dim pAOIEnvelope As IEnvelope
        Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
        Dim tool As ESRI.ArcGIS.DataManagementTools.Clip = New ESRI.ArcGIS.DataManagementTools.Clip()
        Dim buildTool As ESRI.ArcGIS.DataManagementTools.BuildRasterAttributeTable = New ESRI.ArcGIS.DataManagementTools.BuildRasterAttributeTable
        Dim pBandCollection As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing

        If String.IsNullOrEmpty(InputRaster) Then
            Return -3
        End If

        Data_Name = BA_GetBareName(InputRaster, Data_Path)
        Dim pClipFeatureLayer As IFeatureLayer = New FeatureLayer
        Dim pClipFClass As IFeatureClass
        Try
            ClipShapeFile = BA_EnumDescription(AOIClipKey)
            Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(outputFolder)
            If Not BA_Folder_ExistsWindowsIO(outputFolder) Then
                Throw New Exception("Output geodatabase folder " + outputFolder + " does not exist.")
            End If

            If String.IsNullOrEmpty(OutputRasterName) Then 'user didn't specify an output name
                OutputName = outputFolder & "\" & Data_Name
            Else
                'Took out full qualification to solve error message
                If workspaceType = workspaceType.Raster And OutputRasterName.Length > BA_GRID_NAME_MAX_LENGTH Then
                    Throw New Exception("Output raster name cannot exceed " + CStr(BA_GRID_NAME_MAX_LENGTH) + " characters")
                End If
                OutputName = outputFolder & "\" & OutputRasterName
            End If

            'check if a layer of the same name exists in the AOI
            If BA_File_Exists(OutputName, workspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                Return -2
            End If

            'checking overlap between input and clip layers
            Dim pClipFClassPath As String = BA_GeodatabasePath(AOIFolder, GeodatabaseNames.Aoi, True) & ClipShapeFile
            pClipFClass = BA_OpenFeatureClassFromGDB(BA_GeodatabasePath(AOIFolder, GeodatabaseNames.Aoi), ClipShapeFile)
            'retrieve IFeature from FeatureClass
            pClipFeatureLayer.FeatureClass = pClipFClass
            pClipFCursor = pClipFeatureLayer.Search(Nothing, False)
            pClipFeature = pClipFCursor.NextFeature
            pGeo = pClipFeature.Shape
            'copy envelope rectangle to string variable
            pAOIEnvelope = pGeo.Envelope
            Dim urx, llx, lly, ury As Double
            pAOIEnvelope.QueryCoords(llx, lly, urx, ury)
            Dim EnvBox_Text As String = llx & " " & lly & " " & urx & " " & ury

            ' Configure the geoprocessor tool.
            tool.in_raster = InputRaster ' file name for the input raster
            tool.rectangle = EnvBox_Text ' four coordinates defining the minimum bounding rectangle to be clipped
            tool.out_raster = OutputName ' file name for output raster dataset
            'tool.out_raster = AOIFolder & "\" & tmpOutputName  'temporarily save to aoi root as workaround
            tool.in_template_dataset = pClipFClassPath
            tool.clipping_geometry = "ClippingGeometry"  ' clip the raster to the boundary of the aoi_b vector
            GP.AddOutputsToMap = False
            Dim res As Object = GP.Execute(tool, Nothing)

            If res Is Nothing Then
                'Clip did not complete successfully
                Return 0
            Else
                'If source and target are both in a File GDB, need to rebuild the attribute
                'table. Otherwise it will be corrupted if source and target GDB are different
                If RebuildATT Then
                    If BA_GetWorkspaceTypeFromPath(InputRaster) = workspaceType.Geodatabase And _
                        BA_GetWorkspaceTypeFromPath(OutputName) = workspaceType.Geodatabase Then
                        'Check to be sure the target is a single-band thematic raster; Cannot build an attribute table otherwise
                        Dim inputFolder As String = "PleaseReturn"
                        Dim inputFile As String = BA_GetBareName(InputRaster, inputFolder)
                        pBandCollection = CType(BA_OpenRasterFromGDB(inputFolder, inputFile), IRasterBandCollection)
                        pRasterBand = pBandCollection.Item(0)
                        Dim inputATT As Boolean = False
                        pRasterBand.HasTable(inputATT)
                        If inputATT = True Then
                            buildTool.in_raster = OutputName
                            buildTool.overwrite = False
                            res = GP.Execute(buildTool, Nothing)
                            If res IsNot Nothing Then
                                Return 1
                            Else
                                Return 0
                            End If
                        Else
                            Return 1
                        End If
                    Else
                        Return 1
                    End If
                Else
                    Return 1
                End If
                End If
        Catch ex As Exception
            If GP.MessageCount > 0 Then
                MessageBox.Show("Geoprocessor error: " + GP.GetMessages(Type.Missing))
            Else
                MessageBox.Show("Exception: " + ex.Message)
            End If
            Return -1
        Finally
            pClipFClass = Nothing
            pClipFeatureLayer = Nothing
            pClipFCursor = Nothing
            pClipFeature = Nothing
            pGeo = Nothing
            pAOIEnvelope = Nothing
            pBandCollection = Nothing
            pRasterBand = Nothing
            tool = Nothing
            GP = Nothing
        End Try

    End Function

    'return value
    '-1: unknown error
    '-2: output exists
    '-3: missing parameters
    '0: no intersect between the input and the clip layers
    '1: clipping is done successfully
    'Clip vector to an AOI vector. Options are no buffer or standard buffer buffer
    Public Function BA_ClipAOIVector(ByVal AOIFolder As String, ByVal InputShapefile As String, ByVal OutputVectorName As String, _
                                     ByVal outputFolder As String, ByVal WithBuffer As Boolean) As Short
        Dim Data_Path As String = ""
        Dim Data_Name As String = ""
        Dim ClipName As String = ""
        Dim ClipShapeFile As String
        Dim OutputName As String = ""
        Dim pClipFClass As IFeatureClass
        Dim pClipFeatureLayer As IFeatureLayer
        Dim pClipFCursor As IFeatureCursor
        Dim pClipFeature As IFeature
        Dim pGeo As IGeometry
        Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
        Dim tool As ESRI.ArcGIS.AnalysisTools.Clip = New ESRI.ArcGIS.AnalysisTools.Clip()

        If String.IsNullOrEmpty(InputShapefile) Then
            Return -3
        End If

        Data_Name = BA_GetBareName(InputShapefile, Data_Path)

        If String.IsNullOrEmpty(Data_Name) Or String.IsNullOrEmpty(Data_Path) Then
            Return -3
        End If

        Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(Data_Path)

        If WithBuffer Then 'use bufferred AOI
            ClipShapeFile = BA_EnumDescription(AOIClipFile.BufferedAOIExtentCoverage)
        Else 'use un-bufferred AOI
            ClipShapeFile = BA_EnumDescription(AOIClipFile.AOIExtentCoverage)
        End If
        ClipName = AOIFolder & "\" & BA_EnumDescription(GeodatabaseNames.Aoi) & "\" & ClipShapeFile

        If Not BA_Workspace_Exists(outputFolder) Then
            Throw New Exception("Output folder " + outputFolder + " does not exist.")
        End If
        If String.IsNullOrEmpty(OutputVectorName) Then 'user didn't specify an output name
            OutputName = outputFolder & "\" & Data_Name
        Else
            OutputName = outputFolder & "\" & OutputVectorName
        End If

        'check if the input shapefile has a .shp file extension
        Dim InputName As String = InputShapefile
        If workspaceType = workspaceType.Raster Then
            Dim tmpPath As String = ""
            Dim tmpName As String = ""
            tmpName = BA_GetBareName(InputShapefile, tmpPath)
            tmpName = BA_StandardizeShapefileName(InputShapefile, True, False)
            InputName = tmpPath & tmpName
        End If

        'check if a layer of the same name exists in the AOI
        If BA_File_Exists(OutputName, workspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            Return -2
        End If

        'clip the input layer
        Try
            'prepare for data clipping
            'get vector clipping mask, raster clipping mask is created earlier, i.e., pWaterRDS
            Dim pInputFClass As IFeatureClass = Nothing
            Dim pTempFCursor As IFeatureCursor
            Dim pTempFeature As IFeature
            Dim pSFilter As ISpatialFilter

            'get the clip geometry
            pClipFClass = BA_OpenFeatureClassFromGDB(AOIFolder & "\" & BA_EnumDescription(GeodatabaseNames.Aoi), ClipShapeFile)
            pClipFeatureLayer = New FeatureLayer
            pClipFeatureLayer.FeatureClass = pClipFClass

            pClipFCursor = pClipFeatureLayer.Search(Nothing, False)
            pClipFeature = pClipFCursor.NextFeature
            pGeo = pClipFeature.Shape

            'create a spatial filter for checking if the clipped layers are within the clip bnd
            pSFilter = New SpatialFilter
            With pSFilter
                .Geometry = pGeo
                .GeometryField = "SHAPE"
                .SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects
            End With

            If workspaceType = workspaceType.Geodatabase Then
                pInputFClass = BA_OpenFeatureClassFromGDB(Data_Path, Data_Name)
            ElseIf workspaceType = workspaceType.Raster Then
                pInputFClass = BA_OpenFeatureClassFromFile(Data_Path, Data_Name)
            End If

            pTempFCursor = pInputFClass.Search(pSFilter, False)
            pTempFeature = pTempFCursor.NextFeature

            pInputFClass = Nothing
            pTempFCursor = Nothing

            'use geoprocessing object to clip data
            If Not pTempFeature Is Nothing Then 'at least one input feature can be clipped
                tool.in_features = InputName    'the features to be clipped
                tool.clip_features = ClipName   'features used to clip the input features
                tool.out_feature_class = OutputName 'feature class to be created
                GP.AddOutputsToMap = False
                GP.Execute(tool, Nothing)
                Return 1
            Else
                Return 0
            End If
        Catch ex As Exception
            If GP.MessageCount > 0 Then
                MessageBox.Show("Geoprocessor error: " + GP.GetMessages(Type.Missing))
            Else
                MessageBox.Show("Exception: " + ex.Message)
            End If
            Return -1
        Finally
            pClipFClass = Nothing
            pClipFeatureLayer = Nothing
            pClipFCursor = Nothing
            pClipFeature = Nothing
            pGeo = Nothing
            tool = Nothing
            GP = Nothing
        End Try

    End Function

    'prism_type is the listindex of the PRISM data listbox
    ' 0: Annual
    '1,2,3,4: quarterly PRISM
    ' 5: custom setting
    'Generates the path and file name of the desired PRISM layer
    Public Function BA_GetPRISMName(ByVal AOI_folder As String, ByVal prism_type As Short, ByRef prism_path As String) As String
        Dim PRISMName As String
        If prism_type = 0 Then 'read direct Annual PRISM raster
            prism_path = BA_GetPath(AOI_folder, PublicPath.PRISM) & "\" & BA_GetPrismFolderName(17)
            PRISMName = "grid"
        ElseIf prism_type > 0 And prism_type < 5 Then  'read directly Quarterly PRISM raster
            prism_path = BA_GetPath(AOI_folder, PublicPath.PRISM) & "\" & BA_GetPrismFolderName(prism_type + 12)
            PRISMName = "grid"
        Else 'sum individual monthly PRISM rasters
            prism_path = BA_GetPath(AOI_folder, PublicPath.Analysis)
            PRISMName = BA_TEMP_PRISM
        End If

        Return PRISMName
    End Function

    'sort by the values in the valuefield, upper and lowerbnd are also included in the list
    'negative values are excluded from the list
    'return value:
    'n: the number of intervals
    '-1: error
    '0: input attribute out of bound
    Public Function BA_GetUniqueSortedValues(ByRef FilePath As String, ByRef shapefilename As String, ByRef NameFieldName As String, _
                                             ByRef valuefieldname As String, ByRef lowerbnd As Double, ByRef upperbnd As Double, _
                                             ByRef IntervalList() As BA_IntervalList) As Short
        Dim return_value As Short = -1
        Dim pFClass As IFeatureClass
        Dim HasNameField As Boolean
        Dim valuelist() As Object
        Dim namelist() As Object
        Dim nuniquevalue As Integer
        Dim pData As IDataStatistics
        Dim pCursor As ICursor
        Dim pEnumVar As System.Collections.IEnumerator

        If String.IsNullOrEmpty(NameFieldName) Then HasNameField = False Else HasNameField = True

        Try
            Dim pQFilter As IQueryFilter
            Dim pFCursor As IFeatureCursor
            Dim ClassNumber As Double
            Dim pFeature As IFeature
            Dim FIName As Short

            'open shapefile
            pFClass = BA_OpenFeatureClassFromGDB(FilePath, shapefilename)

            If pFClass Is Nothing Then
                Return -1
            End If

            'get unique list of gridcode from the vector att
            pCursor = pFClass.Search(Nothing, False)

            pData = New DataStatistics
            pData.Field = valuefieldname
            pData.Cursor = pCursor
            pEnumVar = pData.UniqueValues
            nuniquevalue = pData.UniqueValueCount

            ReDim valuelist(nuniquevalue + 2)

            'keep a list of unique value and find the largest value
            Dim i As Short
            Dim ncount As Short
            Dim Value As Double

            i = 0 'i keeps track of the actual numbers in the list, ignore negative values
            Do While i < nuniquevalue
                pEnumVar.MoveNext()
                Value = pEnumVar.Current
                If Int(Value - 0.5) < Int(upperbnd) And Int(Value + 0.5) > Int(lowerbnd) Then
                    i = i + 1
                    valuelist(i) = Val(CStr(Value))
                Else
                    If Value > upperbnd Or Value < lowerbnd Then 'invalid data in the attribute field, out of bound
                        MsgBox("WARNING!!" & vbCrLf & "A monitoring site is ignored in the analysis!" & vbCrLf & "The site's elevation (" & Value & ") is outside the DEM range (" & lowerbnd & ", " & upperbnd & ")!")
                    End If
                    nuniquevalue = nuniquevalue - 1
                End If
            Loop

            ncount = i + 2
            ReDim Preserve valuelist(ncount)
            ReDim namelist(ncount)
            ReDim IntervalList(ncount - 1)

            'add upper and lower bnds to the list
            valuelist(ncount) = Val(CStr(upperbnd))
            namelist(ncount) = "Not represented" '"Max Value"
            valuelist(ncount - 1) = Val(CStr(lowerbnd))
            namelist(ncount - 1) = "Min Value"

            'read value
            ' Get field index again
            If Not String.IsNullOrEmpty(NameFieldName) Then
                FIName = pFClass.FindField(NameFieldName)
                If FIName <= 0 Then HasNameField = False
            End If

            pQFilter = New QueryFilter

            'read name field data
            Dim delimiter As String

            For i = 1 To ncount - 2
                If HasNameField Then
                    ClassNumber = valuelist(i)
                    pQFilter.WhereClause = valuefieldname & " = " & ClassNumber
                    pFCursor = pFClass.Search(pQFilter, False)
                    pFeature = pFCursor.NextFeature

                    If Not pFeature Is Nothing Then
                        namelist(i) = ""
                        delimiter = ""
                        Do While Not pFeature Is Nothing 'get all the names of the features that are of the same value
                            If Not pFeature.Value(FIName) Is DBNull.Value Then
                                If Len(Trim(pFeature.Value(FIName))) > 0 Then
                                    namelist(i) = namelist(i) & delimiter & pFeature.Value(FIName)
                                Else
                                    namelist(i) = namelist(i) & delimiter & "Name missing"
                                End If
                            Else
                                namelist(i) = namelist(i) & delimiter & "Name missing"
                            End If
                            delimiter = ", "
                            pFeature = pFCursor.NextFeature
                        Loop
                    Else
                        namelist(i) = valuelist(i)
                    End If

                    pFCursor = Nothing
                    pFeature = Nothing
                Else
                    namelist(i) = valuelist(i)
                End If
            Next
            'sort the list ascendingly
            QuickSort(valuelist, namelist)

            'create the intervallist
            For i = 1 To ncount - 1
                IntervalList(i).Value = i
                IntervalList(i).LowerBound = valuelist(i)
                IntervalList(i).UpperBound = valuelist(i + 1)
                IntervalList(i).Name = namelist(i + 1) 'use the upperbnd name to represent the interval
            Next
            Return ncount - 1 'number of intervals
        Catch ex As Exception
            MessageBox.Show("BA_GetUniqueSortedValues Exception: " + ex.Message)
            Return -1
        Finally
            pCursor = Nothing
            pData = Nothing
            pFClass = Nothing
            pEnumVar = Nothing
        End Try
    End Function

    '--------------------------------------------------------------------------
    ' Quicksort
    '--------------------------------------------------------------------------
    Public Sub QuickSort(ByRef keyarr() As Object, ByRef dataarr() As Object, Optional ByVal numEls As Object = Nothing, Optional ByVal descending As Object = Nothing)

        'data are sorted based on keyarr values, dataarr elememts follow keyarr elements
        Dim tempkey, Value, tempdata As Object
        Dim sp As Integer
        Dim leftStk(32) As Integer
        Dim rightStk(32) As Integer
        Dim leftNdx, rightNdx As Integer
        Dim inverseOrder As Boolean
        Dim i, j As Integer

        ' account for optional arguments
        If IsNothing(numEls) Then numEls = UBound(keyarr)
        If IsNothing(descending) Then descending = False

        inverseOrder = (descending <> False)
        ' init pointers
        leftNdx = LBound(keyarr)
        rightNdx = numEls
        ' init stack
        sp = 1
        leftStk(sp) = leftNdx
        rightStk(sp) = rightNdx

        Do
            If rightNdx > leftNdx Then
                Value = keyarr(rightNdx)
                i = leftNdx - 1
                j = rightNdx
                ' find the pivot item
                If descending Then
                    Do
                        Do : i = i + 1
                        Loop Until keyarr(i) <= Value
                        Do
                            j = j - 1
                        Loop Until j = leftNdx Or keyarr(j) >= Value
                        tempkey = keyarr(i)
                        keyarr(i) = keyarr(j)
                        keyarr(j) = tempkey
                        tempdata = dataarr(i)
                        dataarr(i) = dataarr(j)
                        dataarr(j) = tempdata
                    Loop Until j <= i
                Else
                    Do
                        Do : i = i + 1
                        Loop Until keyarr(i) >= Value
                        Do
                            j = j - 1
                        Loop Until j = leftNdx Or keyarr(j) <= Value
                        tempkey = keyarr(i)
                        keyarr(i) = keyarr(j)
                        keyarr(j) = tempkey
                        tempdata = dataarr(i)
                        dataarr(i) = dataarr(j)
                        dataarr(j) = tempdata
                    Loop Until j <= i
                End If

                ' swap found items
                tempkey = keyarr(j)
                keyarr(j) = keyarr(i)
                keyarr(i) = keyarr(rightNdx)
                keyarr(rightNdx) = tempkey
                tempdata = dataarr(j)
                dataarr(j) = dataarr(i)
                dataarr(i) = dataarr(rightNdx)
                dataarr(rightNdx) = tempdata
                ' push on the stack the pair of pointers
                ' that differ most
                sp = sp + 1
                If (i - leftNdx) > (rightNdx - i) Then
                    leftStk(sp) = leftNdx
                    rightStk(sp) = i - 1
                    leftNdx = i + 1
                Else
                    leftStk(sp) = i + 1
                    rightStk(sp) = rightNdx
                    rightNdx = i - 1
                End If
            Else
                ' pop a new pair of pointers off the stacks
                leftNdx = leftStk(sp)
                rightNdx = rightStk(sp)
                sp = sp - 1
                If sp = 0 Then Exit Do
            End If
        Loop
    End Sub

    '--------------------------------------------------------------------------
    ' Quicksort
    '--------------------------------------------------------------------------
    Public Sub BA_SortHistogram(ByRef keyarr() As Double, ByRef dataarr() As Integer)

        'data are sorted based on keyarr values, dataarr elememts follow keyarr elements
        Dim tempkey, Value, tempdata As Object
        Dim sp As Integer
        Dim leftStk(32) As Integer
        Dim rightStk(32) As Integer
        Dim leftNdx, rightNdx As Integer
        Dim inverseOrder As Boolean
        Dim i, j As Integer

        Dim numEls As Integer = keyarr.GetUpperBound(0)
        Dim descending As Boolean = False

        inverseOrder = (descending <> False)
        ' init pointers
        leftNdx = LBound(keyarr)
        rightNdx = numEls
        ' init stack
        sp = 1
        leftStk(sp) = leftNdx
        rightStk(sp) = rightNdx

        Do
            If rightNdx > leftNdx Then
                Value = keyarr(rightNdx)
                i = leftNdx - 1
                j = rightNdx
                ' find the pivot item
                If descending Then
                    Do
                        Do : i = i + 1
                        Loop Until keyarr(i) <= Value
                        Do
                            j = j - 1
                        Loop Until j = leftNdx Or keyarr(j) >= Value
                        tempkey = keyarr(i)
                        keyarr(i) = keyarr(j)
                        keyarr(j) = tempkey
                        tempdata = dataarr(i)
                        dataarr(i) = dataarr(j)
                        dataarr(j) = tempdata
                    Loop Until j <= i
                Else
                    Do
                        Do : i = i + 1
                        Loop Until keyarr(i) >= Value
                        Do
                            j = j - 1
                        Loop Until j = leftNdx Or keyarr(j) <= Value
                        tempkey = keyarr(i)
                        keyarr(i) = keyarr(j)
                        keyarr(j) = tempkey
                        tempdata = dataarr(i)
                        dataarr(i) = dataarr(j)
                        dataarr(j) = tempdata
                    Loop Until j <= i
                End If

                ' swap found items
                tempkey = keyarr(j)
                keyarr(j) = keyarr(i)
                keyarr(i) = keyarr(rightNdx)
                keyarr(rightNdx) = tempkey
                tempdata = dataarr(j)
                dataarr(j) = dataarr(i)
                dataarr(i) = dataarr(rightNdx)
                dataarr(rightNdx) = tempdata
                ' push on the stack the pair of pointers
                ' that differ most
                sp = sp + 1
                If (i - leftNdx) > (rightNdx - i) Then
                    leftStk(sp) = leftNdx
                    rightStk(sp) = i - 1
                    leftNdx = i + 1
                Else
                    leftStk(sp) = i + 1
                    rightStk(sp) = rightNdx
                    rightNdx = i - 1
                End If
            Else
                ' pop a new pair of pointers off the stacks
                leftNdx = leftStk(sp)
                rightNdx = rightStk(sp)
                sp = sp - 1
                If sp = 0 Then Exit Do
            End If
        Loop
    End Sub

    ' Returns arrays of raster and vector files in a given workspace (folder) 
    Public Sub BA_ListLayersinAOI(ByVal WorkspacePath As String, ByRef RasterList() As String, ByRef VectorList() As String)
        Dim nshapefile, nraster, i As Integer
        Dim pWSF As IWorkspaceFactory = New ShapefileWorkspaceFactory
        Dim pWorkspace As IWorkspace = Nothing
        Dim pEnumDSName As IEnumDatasetName = Nothing
        Dim pDSName As IDatasetName = Nothing

        Try

            If BA_Folder_ExistsWindowsIO(WorkspacePath) Then

                pWorkspace = pWSF.OpenFromFile(WorkspacePath, 0)
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
                pWSF = New RasterWorkspaceFactory
                pWorkspace = pWSF.OpenFromFile(WorkspacePath, 0)
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
            Else
                'The folder doesn't exist so we return 0-length arrays
                ReDim VectorList(0)
                ReDim RasterList(0)
            End If

        Catch ex As Exception
            MessageBox.Show("BA_ListLayersinAOI Exception: " + ex.Message)
        Finally
            ' release COM references
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDSName)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pEnumDSName)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWorkspace)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWSF)
        End Try
    End Sub

    Private Function IsCpgFile(ByVal strFileName As String) As Boolean
        If Right(strFileName, 4).ToUpper = ".CPG" Then
            Return True
        End If
        Return False
    End Function

    'Remove all the layers from the map document that are in the specified folder
    'no unit test created as it interacts with user interface
    Public Function BA_RemoveLayersInFolder(ByVal pMxDoc As ESRI.ArcGIS.ArcMapUI.IMxDocument, ByRef FolderPath As String) As Short
        'check if a valid folder path is provided
        If String.IsNullOrEmpty(FolderPath) Then
            Return 0
        End If

        Dim pMap As IMap = pMxDoc.FocusMap

        'check if a layer with the assigned name exists
        'search layer of the specified name, if found
        Dim nlayers As Integer = pMap.LayerCount
        Dim i As Integer
        Dim pTempLayer As ILayer = Nothing
        Dim layerDataPath As String
        Dim pDSet As IDataset = Nothing
        Dim pDLayer As IDataLayer2 = Nothing
        Dim stringpos As Short
        'Dim pCmdItem As ICommandItem = BA_RemoveLayerCommandItem(pMxDoc)

        Try
            Dim layers_removed As Short = 0
            For i = nlayers To 1 Step -1
                pTempLayer = pMap.Layer(i - 1)
                If TypeOf pTempLayer Is IFeatureLayer Or _
                   TypeOf pTempLayer Is IRasterLayer Then
                    If pTempLayer.Valid Then
                        pDSet = pTempLayer
                        layerDataPath = pDSet.Workspace.PathName
                        stringpos = layerDataPath.IndexOf(FolderPath)
                        If stringpos > -1 Then 'found a layer that comes from the folder, remove the layer
                            pDLayer = pTempLayer
                            pDLayer.Disconnect() 'disconnect a rasterlayer before removing it
                            'NIM065856 IDataLayer2.Disconnect doesn't release data from layer
                            'pMxDoc.ContextItem = pTempLayer
                            'pCmdItem.Execute()
                            'pMxDoc.ContextItem = Nothing
                            pMap.DeleteLayer(pTempLayer)
                            layers_removed = layers_removed + 1
                        End If
                    Else
                        MessageBox.Show("The " & pTempLayer.Name & " layer is invalid!", "Invalid layer", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                End If
            Next

            'refresh map
            pMxDoc.UpdateContents()
            pMxDoc.ActivatedView.Refresh()
            Return layers_removed
        Catch ex As Exception
            MessageBox.Show("Exception: " + ex.Message)
            Return 0
        Finally
            pTempLayer = Nothing
            pDSet = Nothing
            pDLayer = Nothing
            pMap = Nothing
            pMxDoc = Nothing
            'pCmdItem = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try

    End Function

    'Not used as of June 15, 2011 LCB
    'return value:
    '   -1: error occurred
    ' otherwise, the value indicate the number of records updated
    'Public Function BA_UpdateReclassVectorAttributes(ByRef IntervalList() As BA_IntervalList, ByRef FilePath As String, ByRef FileName As String) As Short
    '    'fields to be added
    '    'NAME - esriFieldTypeString: for labeling purpose
    '    'LBOUND - esriFieldTypeDouble: for upper bound
    '    'UBOUND - esriFieldTypeDouble: for lower bound
    '    Dim return_value As Short = 0

    '    ' Open the folder to contain the shapefile as a workspace
    '    Dim pFClass As IFeatureClass = BA_OpenFeatureClassFromFile(FilePath, FileName)
    '    Dim pQFilter As IQueryFilter = New QueryFilter
    '    Dim pField As IField = New FieldClass()
    '    Dim pFld As IFieldEdit2 = CType(pField, IFieldEdit2)
    '    Dim pFCursor As IFeatureCursor = Nothing
    '    Dim pFeature As IFeature = Nothing

    '    Dim nclass As Short = UBound(IntervalList)

    '    'add fields
    '    Try
    '        'add Name field
    '        ' check if field exist
    '        Dim FieldIndex As Short
    '        Dim FName(3) As String
    '        FName(1) = BA_FIELD_NAME
    '        FName(2) = BA_FIELD_LBOUND
    '        FName(3) = BA_FIELD_UBOUND
    '        Dim i As Short

    '        For i = 1 To 3
    '            FieldIndex = pFClass.FindField(FName(i))
    '            ' Define field type
    '            If FieldIndex < 0 Then 'add field
    '                'Define field name
    '                pFld.Name_2 = FName(i)
    '                Select Case i
    '                    Case 1 'Name field
    '                        pFld.Type_2 = esriFieldType.esriFieldTypeString
    '                        pFld.Length_2 = BA_NAME_FIELD_WIDTH
    '                        pFld.Required_2 = False
    '                    Case 2, 3 'Ubound and LBound field
    '                        pFld.Type_2 = esriFieldType.esriFieldTypeDouble
    '                        pFld.Length_2 = BA_BOUND_FIELD_WIDTH
    '                        pFld.Required_2 = False
    '                End Select

    '                ' Add field
    '                pFClass.AddField(pFld)
    '            End If
    '        Next

    '        'update value
    '        ' Get field index again
    '        Dim FI2, FI1, FI3 As Short

    '        FI1 = pFClass.FindField(FName(1))
    '        FI2 = pFClass.FindField(FName(2))
    '        FI3 = pFClass.FindField(FName(3))

    '        Dim tempname As String

    '        For i = 1 To nclass

    '            pQFilter.WhereClause = BA_FIELD_GRIDCODE & " = " & CInt(IntervalList(i).Value)
    '            pFCursor = pFClass.Update(pQFilter, False)
    '            pFeature = pFCursor.NextFeature

    '            Do While Not pFeature Is Nothing
    '                tempname = Trim(IntervalList(i).Name)
    '                If Len(tempname) >= BA_NAME_FIELD_WIDTH Then 'truncate the string if it's longer than the att field width
    '                    tempname = Left(tempname, BA_NAME_FIELD_WIDTH - 1)
    '                End If
    '                pFeature.Value(FI1) = tempname
    '                pFeature.Value(FI2) = IntervalList(i).LowerBound
    '                pFeature.Value(FI3) = IntervalList(i).UpperBound
    '                pFCursor.UpdateFeature(pFeature)
    '                pFeature = pFCursor.NextFeature
    '                return_value = return_value + 1
    '            Loop

    '            pFCursor = Nothing
    '            pFeature = Nothing
    '        Next
    '        Return return_value
    '    Catch ex As Exception
    '        MessageBox.Show("Exception: " + ex.Message)
    '        Return -1
    '    Finally
    '        ' release COM references
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pQFilter)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFClass)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pField)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFld)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeature)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFCursor)
    '    End Try


    'End Function

    'this function creates and save raster reclassified zone data using an IntervalList structure
    'Return value:
    ' 0: error occurred
    ' 1: process completed without any error
    ' pMxDoc: Reference to current document so layers can be removed
    ' pInputRaster: Source raster that the zones are based from ex:DEM for elevation
    ' IntervalList: structure with unique values; Filled before this method is called
    ' OutputPath: Folder to write output
    ' Vector name: file name for output
    ' Message key: 
    Public Function BA_MakeZoneDatasets(ByVal pMxDoc As ESRI.ArcGIS.ArcMapUI.IMxDocument, ByVal pInputRaster As IGeoDataset, _
                                        ByRef IntervalList() As BA_IntervalList, ByVal OutputPath As String, ByVal RasterName As String, _
                                        ByVal VectorName As String, ByVal MessageKey As AOIMessageKey) As Short
        Dim response As Short
        Dim pZoneRaster As String

        'check and remove the output folder if it already exists
        response = BA_RemoveLayersInFolder(pMxDoc, OutputPath) 'remove layers from map

        If BA_File_Exists(OutputPath & "\" & RasterName, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            'delete raster
            response = BA_RemoveRasterFromGDB(OutputPath, RasterName)
            If response = 0 Then 'unable to delete the folder
                MessageBox.Show("Unable to remove the folder " & OutputPath & "\" & RasterName & ". Program stopped.")
                Return 0
            End If
        End If

        'reclassify and save the reclassified raster and
        'propogate interval list data to the attribute table of the grid
        response = BA_ReclassRasterFromIntervalList(IntervalList, pInputRaster, OutputPath, RasterName)
        'Set pInputRaster = Nothing
        response = BA_UpdateReclassRasterAttributes(IntervalList, OutputPath, RasterName)

        'convert the raster to vector
        If Not String.IsNullOrEmpty(VectorName) Then 'convert to vector only when a valid name is provided
            'Delete Shapefile if Exists
            If BA_File_Exists(OutputPath & "\" & VectorName, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                'Remove Layer
                response = BA_RemoveLayersInFolder(pMxDoc, OutputPath)
                'Delete Dataset
                response = BA_Remove_ShapefileFromGDB(OutputPath, VectorName)
            End If

            'open the zone raster dataset for conversion
            pZoneRaster = OutputPath + "\" + RasterName
            Dim pVectorPath = OutputPath + "\" + VectorName
            response = BA_Raster2PolygonShapefileFromPath(pZoneRaster, pVectorPath, False)

            If response = 0 Then
                MessageBox.Show("Unable to convert the " & BA_EnumDescription(MessageKey) & " zone raster to vector! Program stopped.")
                Return 0
            End If

            'propogate interval list data to the attribute table of the shapefile
            response = BA_UpdateReclassVectorAttributes(IntervalList, OutputPath, VectorName)
        End If

        Return 1
    End Function

    ' Not used as of June 15, 2011 LCB
    'return value:
    '   -1: error occurred
    ' otherwise, the value indicate the number of records read
    'Public Function BA_ReadReclassVectorAttribute(ByRef IntervalList() As BA_IntervalList, ByRef FilePath As String, ByRef FileName As String) As Object
    '    'fields to be read
    '    'NAME - esriFieldTypeString: for labeling purpose
    '    'LBOUND - esriFieldTypeDouble: for upper bound
    '    'UBOUND - esriFieldTypeDouble: for lower bound
    '    Dim nclass As Short = -1
    '    Dim nuniquevalue As Short
    '    Dim pFClass As IFeatureClass = Nothing
    '    Dim pData As IDataStatistics = New DataStatistics
    '    Dim pEnumVar As System.Collections.IEnumerator = Nothing
    '    Dim pCursor As ICursor = Nothing
    '    Dim pQFilter As IQueryFilter = New QueryFilter
    '    Dim pFCursor As IFeatureCursor = Nothing
    '    Dim pFeature As IFeature = Nothing

    '    Try
    '        ' Open the folder to contain the shapefile as a workspace
    '        pFClass = BA_OpenFeatureClassFromFile(FilePath, FileName)

    '        'get unique list of gridcode from the vector att
    '        pCursor = pFClass.Search(Nothing, False)

    '        pData.Field = BA_FIELD_GRIDCODE
    '        pData.Cursor = pCursor
    '        pEnumVar = pData.UniqueValues
    '        nuniquevalue = pData.UniqueValueCount

    '        If nuniquevalue <= 0 Then 'no feature in the feature class
    '            Return -1
    '        End If

    '        'keep a list of unique value and find the largest value
    '        Dim uarray() As Object
    '        Dim i As Short
    '        Dim Value As Object
    '        ReDim uarray(nuniquevalue)

    '        For i = 1 To nuniquevalue
    '            pEnumVar.MoveNext()
    '            Value = pEnumVar.Current
    '            If Value > nclass Then nclass = Value 'find max in the EnumVar collection
    '            uarray(i) = Value
    '        Next

    '        ReDim IntervalList(nclass)
    '        'read value
    '        ' Get field index again
    '        Dim FI2, FI1, FI3 As Short
    '        FI1 = pFClass.FindField(BA_FIELD_NAME)
    '        FI2 = pFClass.FindField(BA_FIELD_LBOUND)
    '        FI3 = pFClass.FindField(BA_FIELD_UBOUND)

    '        Dim ClassNumber As Short

    '        For i = 1 To nuniquevalue
    '            ClassNumber = uarray(i)
    '            pQFilter.WhereClause = BA_FIELD_GRIDCODE & " = " & ClassNumber
    '            pFCursor = pFClass.Search(pQFilter, False)
    '            pFeature = pFCursor.NextFeature

    '            If Not pFeature Is Nothing Then
    '                IntervalList(i).Value = ClassNumber
    '                IntervalList(i).Name = pFeature.Value(FI1)
    '                IntervalList(i).LowerBound = pFeature.Value(FI2)
    '                IntervalList(i).UpperBound = pFeature.Value(FI3)
    '            End If

    '            pFCursor = Nothing
    '            pFeature = Nothing
    '        Next

    '        Return nclass
    '    Catch ex As Exception
    '        MessageBox.Show("BA_ReadReclassVectorAttribute Exception: " & ex.Message)
    '        Return -1
    '    Finally
    '        ' release COM references
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pQFilter)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFClass)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pData)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pEnumVar)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pCursor)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeature)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFCursor)
    '    End Try

    'End Function

    'set the interval list of ASPECT
    Public Sub BA_SetAspectClasses(ByRef IntervalList() As BA_IntervalList, Optional ByVal AspectDirectionsNo As Integer = 16)
        Dim ASPECT_COUNT As Short = AspectDirectionsNo + 2
        ReDim IntervalList(ASPECT_COUNT)
        Dim aspectname(ASPECT_COUNT) As String
        Dim i As Short
        Dim interval As Double = 0

        Select Case AspectDirectionsNo
            Case 4
                interval = 90 'i.e., 360 / 4
                aspectname(1) = "Flat"
                aspectname(2) = "N"
                aspectname(3) = "E"
                aspectname(4) = "S"
                aspectname(5) = "W"
                aspectname(6) = "N"
            Case 8
                interval = 45 'i.e., 360 / 8
                aspectname(1) = "Flat"
                aspectname(2) = "N"
                aspectname(3) = "NE"
                aspectname(4) = "E"
                aspectname(5) = "SE"
                aspectname(6) = "S"
                aspectname(7) = "SW"
                aspectname(8) = "W"
                aspectname(9) = "NW"
                aspectname(10) = "N"
            Case Else 'i.e., 16 aspect directions
                interval = 22.5 'i.e., 360 / 16
                aspectname(1) = "Flat"
                aspectname(2) = "N"
                aspectname(3) = "NNE"
                aspectname(4) = "NE"
                aspectname(5) = "ENE"
                aspectname(6) = "E"
                aspectname(7) = "ESE"
                aspectname(8) = "SE"
                aspectname(9) = "SSE"
                aspectname(10) = "S"
                aspectname(11) = "SSW"
                aspectname(12) = "SW"
                aspectname(13) = "WSW"
                aspectname(14) = "W"
                aspectname(15) = "WNW"
                aspectname(16) = "NW"
                aspectname(17) = "NNW"
                aspectname(18) = "N"
        End Select

        'flat
        With IntervalList(1)
            .Value = -1
            .Name = "Flat"
            .LowerBound = -2
            '.UpperBound = 0
            .UpperBound = -0.01
        End With

        'north
        With IntervalList(2)
            .Value = 1
            .Name = "N"
            'Assign 0 azimuth (north-facing direction) was assigned a value of 1
            '.LowerBound = 0
            .LowerBound = -0.01
            .UpperBound = interval / 2
        End With

        For i = 3 To AspectDirectionsNo + 1
            With IntervalList(i)
                .Value = i - 1
                .Name = aspectname(i)
                .LowerBound = IntervalList(i - 1).UpperBound
                .UpperBound = .LowerBound + interval
            End With
        Next

        'north again
        With IntervalList(AspectDirectionsNo + 2)
            .Value = 1
            .Name = "N"
            .LowerBound = IntervalList(AspectDirectionsNo + 1).UpperBound
            .UpperBound = 360
        End With
    End Sub

    Public Sub BA_SetSlopeClasses(ByRef IntervalList() As BA_IntervalList)
        ReDim IntervalList(0 To 7)

        'flat - 2
        With IntervalList(1)
            .Value = 1
            .Name = "Flat - 2%"
            .LowerBound = 0
            .UpperBound = 2
        End With

        '2 - 5
        With IntervalList(2)
            .Value = 2
            .Name = "2% - 5%"
            .LowerBound = 2
            .UpperBound = 5
        End With

        '5 - 7
        With IntervalList(3)
            .Value = 3
            .Name = "5% - 7%"
            .LowerBound = 5
            .UpperBound = 7
        End With

        '7 - 10
        With IntervalList(4)
            .Value = 4
            .Name = "7% - 10%"
            .LowerBound = 7
            .UpperBound = 10
        End With

        '10 - 15
        With IntervalList(5)
            .Value = 5
            .Name = "10% - 15%"
            .LowerBound = 10
            .UpperBound = 15
        End With

        '15 - 20
        With IntervalList(6)
            .Value = 6
            .Name = "15% - 20%"
            .LowerBound = 15
            .UpperBound = 20
        End With

        '> 20
        With IntervalList(7)
            .Value = 7
            .Name = "> 20%"
            .LowerBound = 20
            .UpperBound = 999
        End With

    End Sub

    ' Uses Geoanalyst IReclassOp tool to slice a raster
    Public Function BA_SliceRaster(ByVal inputFolderPath As String, ByVal outputFolderPath As String, _
                                   ByVal outputLayerName As String, ByVal sliceType As esriGeoAnalysisSliceEnum, _
                                   ByVal zoneCount As Long, ByVal baseZone As Integer) As BA_ReturnCode
        Dim pGeoDataset As IGeoDataset = Nothing
        Dim pReclassOp As IReclassOp = New RasterReclassOp
        Dim pInputRaster As IGeoDataset = Nothing
        Try
            Dim filePath As String = "PleaseReturn"
            Dim fileName As String = BA_GetBareName(inputFolderPath, filePath)
            pInputRaster = BA_OpenRasterFromGDB(filePath, fileName)
            pGeoDataset = pReclassOp.Slice(pInputRaster, sliceType, zoneCount, baseZone)
            BA_SaveRasterDatasetGDB(pGeoDataset, outputFolderPath, BA_RASTER_FORMAT, outputLayerName)
            Return BA_ReturnCode.Success
        Catch ex As Exception
            MessageBox.Show("Exception: " + ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            pReclassOp = Nothing
            pInputRaster = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Function

    Public Function BA_GetEqualAreaIntervals(ByVal inputPath As String, ByVal inputRaster As String, _
                                             ByVal numClasses As Integer, Optional ByVal aoiFolder As String = "") As ReclassItem()
        Dim pGeoDS As IGeoDataset = Nothing
        Dim pRasterDataset As IRasterDataset3 = Nothing
        Dim pAOIRasterDataset As IRasterDataset3 = Nothing
        Dim pInputRaster As IRaster = Nothing
        Dim pUV As IUniqueValues2 = New UniqueValues
        Dim pRCUV As IRasterCalcUniqueValues2 = New RasterCalcUniqueValues
        Dim quantile As IClassify = New Quantile
        Dim pExtractOp As IExtractionOp2 = New RasterExtractionOp

        Try
            Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(inputPath)
            If workspaceType = workspaceType.Geodatabase Then
                pGeoDS = BA_OpenRasterFromGDB(inputPath, inputRaster)
            ElseIf workspaceType = workspaceType.Raster Then
                pGeoDS = BA_OpenRasterFromFile(inputPath, inputRaster)
            End If

            'get AOI raster to set the analysis mask
            Dim AOIFileName As String = BA_EnumDescription(PublicPath.AoiGrid)
            AOIFileName = BA_StandardizeShapefileName(AOIFileName, False, False)
            Dim aoiGdbPath = aoiFolder & "\" & BA_EnumDescription(GeodatabaseNames.Aoi)

            If aoiFolder <> "" Then pAOIRasterDataset = BA_OpenRasterFromGDB(aoiGdbPath, AOIFileName)

            'extract input raster using AOI mask
            If pAOIRasterDataset Is Nothing Then 'no AOI raster is found or not specified by the user, ignore the AOI raster
                pRasterDataset = pExtractOp.Raster(pGeoDS, pAOIRasterDataset)
            Else
                pRasterDataset = CType(pGeoDS, RasterDataset)  'Explicit cast
            End If

            ' Create IRaster from IGeoDataset
            pInputRaster = pRasterDataset.CreateDefaultRaster()
            pRCUV.AddFromRaster(pInputRaster, 0, pUV)

            'Get the array of values and counts from the raster
            Dim dataValues() As Double = Nothing
            Dim datafreq() As Integer = Nothing
            pUV.GetHistogram(dataValues, datafreq)
            BA_SortHistogram(dataValues, datafreq)
            quantile.SetHistogramData(dataValues, datafreq)
            quantile.Classify(numClasses)
            Dim classBreaks() As Double = quantile.ClassBreaks()

            Dim arrReclassItems(numClasses - 1) As ReclassItem
            For i As Integer = 0 To classBreaks.Length - 2
                Dim fromValue As Double = classBreaks(i)
                Debug.Print(i + 1)
                Dim toValue As Double = classBreaks(i + 1)
                Dim newItem As ReclassItem = New ReclassItem(fromValue, toValue, i)
                arrReclassItems(i) = newItem
            Next
            Return arrReclassItems
        Catch ex As Exception
            MsgBox("BA_GetEqualAreaIntervals Exception: " & ex.Message)
            Return Nothing
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGeoDS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterDataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pAOIRasterDataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pInputRaster)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pExtractOp)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pUV)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRCUV)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(quantile)
        End Try

    End Function

    Public Sub BA_BuildEqualAreaReclassTable(ByVal grid As DataGridView, ByVal aoiFolder As String, ByVal inputPath As String, _
                                             ByVal inputRaster As String, ByVal txtClasses As TextBox, _
                                             ByVal idxToValue As Integer, ByVal styleCell As DataGridViewCell, _
                                             ByVal decimalPlaces As Int32, ByVal displayUnits As MeasurementUnit, _
                                             ByVal dataUnits As MeasurementUnit)
        ' Ensure # classes is numeric
        If Not IsNumeric(txtClasses.Text) Then
            MessageBox.Show("Numeric value required for # classes")
            txtClasses.Focus()
            Exit Sub
        End If

        ' Convert input value to integers
        Dim numClasses As Integer = CInt(txtClasses.Text)

        ' Calculate class interval
        Dim intItems As ReclassItem() = BA_GetEqualAreaIntervals(inputPath, inputRaster, numClasses, aoiFolder)

        ' Populate table
        grid.Rows.Clear()
        If intItems IsNot Nothing Then
            Dim fromVal As Double
            Dim toVal As Double
            Dim outputValue As Integer = 1
            For Each item In intItems
                If displayUnits = dataUnits Then
                    fromVal = item.FromValue
                    toVal = item.ToValue
                ElseIf displayUnits = MeasurementUnit.Millimeters And dataUnits = MeasurementUnit.Inches Then
                    ' Convert inches to millimeters
                    fromVal = item.FromValue * BA_Inches_To_Millimeters
                    toVal = item.ToValue * BA_Inches_To_Millimeters
                ElseIf displayUnits = MeasurementUnit.Inches And dataUnits = MeasurementUnit.Millimeters Then
                    ' Convert millimeters to inches
                    fromVal = item.FromValue / BA_Inches_To_Millimeters
                    toVal = item.ToValue / BA_Inches_To_Millimeters
                End If
                If outputValue = numClasses Then
                    ' Increment max value in case it is rounded down to catch actual max value for the last entry
                    toVal = toVal + 0.01
                End If
                fromVal = Math.Round(fromVal, decimalPlaces)
                toVal = Math.Round(toVal, decimalPlaces)
                Dim nextLine As Object() = {fromVal, toVal, outputValue}
                grid.Rows.Add(nextLine)
                outputValue += 1
            Next

            ' Set read-only formats
            Dim readOnlyCell As DataGridViewCell = grid.Item(idxToValue, grid.RowCount - 1)
            readOnlyCell.ReadOnly = True
            readOnlyCell.Style = styleCell.Style
        End If
    End Sub

    'Join two rasters and save to an output file
    'Rows that don't exist in the target table aren't added
    'Subroutine works with File GDB only
    Public Function BA_JoinRasters(ByVal targetRasterPath As String, ByVal sourceRasterPath As String, _
                                   ByVal combinedRasterPath As String) As BA_ReturnCode
        'Need to check ane make sure both are from a geodatabase
        Dim pTargetRasterDS As IRasterDataset = Nothing
        Dim pTargetRaster As IRaster2 = Nothing
        Dim pTargetAttTable As ITable = Nothing
        Dim pSourceRasterDS As IRasterDataset = Nothing
        Dim pSourceRaster As IRaster2 = Nothing
        Dim pSourceAttTable As ITable = Nothing
        Dim pMemRelClassFact As IMemoryRelationshipClassFactory = New MemoryRelationshipClassFactory
        Dim pRelClass As IRelationshipClass = Nothing
        Dim pRelQueryTableFact As IRelQueryTableFactory = New RelQueryTableFactory
        Dim pRelQueryTab As ITable = Nothing
        Dim pSaveAs As ISaveAs = Nothing
        Dim workspaceFactory As IWorkspaceFactory = New FileGDBWorkspaceFactory
        Dim workspace As IWorkspace = Nothing

        Try
            'This only works for file geodatabase layers; Check to ensure all inputs are FGDB
            If BA_GetWorkspaceTypeFromPath(targetRasterPath) = WorkspaceType.Geodatabase And
                BA_GetWorkspaceTypeFromPath(sourceRasterPath) = WorkspaceType.Geodatabase And
                BA_GetWorkspaceTypeFromPath(combinedRasterPath) = WorkspaceType.Geodatabase Then
                Dim inputFolder As String = "PleaseReturn"
                Dim inputFile As String = BA_GetBareName(targetRasterPath, inputFolder)
                pTargetRasterDS = CType(BA_OpenRasterFromGDB(inputFolder, inputFile), IRasterDataset)
                pTargetRaster = pTargetRasterDS.CreateDefaultRaster
                inputFile = BA_GetBareName(sourceRasterPath, inputFolder)
                pSourceRasterDS = CType(BA_OpenRasterFromGDB(inputFolder, inputFile), IRasterDataset)
                pSourceRaster = pSourceRasterDS.CreateDefaultRaster

                ' ++ Get raster attribute tables
                pTargetAttTable = pTargetRaster.AttributeTable
                pSourceAttTable = pSourceRaster.AttributeTable

                If pTargetAttTable IsNot Nothing And pSourceAttTable IsNot Nothing Then

                    ' ++ Create the MemoryRelationshipClass that defines what is to be joined
                    pRelClass = pMemRelClassFact.Open("join_vat", pTargetAttTable, _
                            "value", pSourceAttTable, "value", "forward", "backward", esriRelCardinality.esriRelCardinalityOneToOne)

                    ' ++ Perform the join
                    pRelQueryTab = pRelQueryTableFact.Open(pRelClass, True, Nothing, Nothing, "", True, False)

                    ' ++ Save the raster to a new raster dataset
                    pSaveAs = pTargetRaster
                    pTargetRaster.AttributeTable = pRelQueryTab

                    inputFile = BA_GetBareName(combinedRasterPath, inputFolder)
                    ' Strip trailing "\" if exists
                    If inputFolder(Len(inputFolder) - 1) = "\" Then
                        inputFolder = inputFolder.Remove(Len(inputFolder) - 1, 1)
                    End If
                    workspace = workspaceFactory.OpenFromFile(inputFolder, 0)
                    pSaveAs.SaveAs(inputFile, workspace, BA_RASTER_FORMAT)
                    Return BA_ReturnCode.Success
                Else
                    Return BA_ReturnCode.NotSupportedOperation
                End If
            Else
                Return BA_ReturnCode.NotSupportedOperation
            End If
        Catch ex As Exception
            Debug.Print("BA_JoinRasters Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            pTargetRaster = Nothing
            pTargetRasterDS = Nothing
            pTargetAttTable = Nothing
            pSourceRaster = Nothing
            pSourceRasterDS = Nothing
            pSourceAttTable = Nothing
            pRelClass = Nothing
            pRelQueryTab = Nothing
            pSaveAs = Nothing
            workspace = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Function

    Public Function BA_RemoveDupAttribFields(ByVal targetRasterPath As String, ByVal fieldSuffix As String) As BA_ReturnCode
        Dim pTargetRasterDS As IRasterDataset = Nothing
        Dim pTargetRaster As IRaster2 = Nothing
        Dim pTargetAttTable As ITable = Nothing
        Dim pField As IField = Nothing

        Try
            'Open the raster and get the attribute table
            Dim inputFolder As String = "PleaseReturn"
            Dim inputFile As String = BA_GetBareName(targetRasterPath, inputFolder)
            pTargetRasterDS = CType(BA_OpenRasterFromGDB(inputFolder, inputFile), IRasterDataset)
            pTargetRaster = pTargetRasterDS.CreateDefaultRaster
            pTargetAttTable = pTargetRaster.AttributeTable

            'Get the fields and store the names in a list
            Dim fieldList As IList(Of String) = New List(Of String)
            For i As Integer = 0 To pTargetAttTable.Fields.FieldCount - 1
                fieldList.Add(pTargetAttTable.Fields.Field(i).Name)
            Next

            For Each fieldName As String In fieldList
                Dim idxField As Integer = pTargetAttTable.Fields.FindField(fieldName & fieldSuffix)
                If idxField > 0 Then
                    pField = pTargetAttTable.Fields.Field(idxField)
                    pTargetAttTable.DeleteField(pField)
                End If
            Next
            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_RemoveDupAttribFields Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            pTargetAttTable = Nothing
            pTargetRaster = Nothing
            pTargetRasterDS = Nothing
            pField = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try

    End Function

    'this version could prevent the problem of not able to delete a raster
    'check if the save target location exists first. If it exists, remove it
    'return value: 0 error occurred
    '              1 successfully saved the raster
    Public Function BA_ReclassRasterFromIntervalList(ByVal IntervalList() As BA_IntervalList, ByVal RasterToReclass As IGeoDataset, _
                                                     ByVal SavePath As String, ByVal SaveName As String) As Integer
        Dim NumOfClasses As Long
        Dim i As Integer
        NumOfClasses = UBound(IntervalList)

        'Set Remap
        Dim pNumberReMap As INumberRemap = New NumberRemap
        Dim pReclassOp As IReclassOp = New RasterReclassOp
        Dim pRasterGD As IGeoDataset

        Try
            For i = 1 To NumOfClasses
                pNumberReMap.MapRange(CDbl(IntervalList(i).LowerBound), CDbl(IntervalList(i).UpperBound), CLng(IntervalList(i).Value))
            Next

            'Reclass by Remap
            pRasterGD = pReclassOp.ReclassByRemap(RasterToReclass, pNumberReMap, False)

            'save the output raster
            Return BA_SaveRasterDatasetGDB(pRasterGD, SavePath, BA_RASTER_FORMAT, SaveName)

        Catch ex As Exception
            Dim messagetext As String
            messagetext = "INTERVAL RANGE ERROR!"
            For i = 1 To NumOfClasses
                messagetext = messagetext & vbCrLf & IntervalList(i).Value & ": " & IntervalList(i).LowerBound & " - " & IntervalList(i).UpperBound
            Next
            MsgBox(messagetext)
            Debug.Print("BA_ReclassRasterFromIntervalList Exception: " & ex.Message)
            Return -1
        Finally
            pRasterGD = Nothing
            pReclassOp = Nothing
            pNumberReMap = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    'return value:
    '   -1: error occurred
    ' otherwise, the value indicate the number of records updated
    Public Function BA_UpdateReclassVectorAttributes(IntervalList() As BA_IntervalList, filepath As String, FileName As String) As Integer
        'fields to be added
        'NAME - esriFieldTypeString: for labeling purpose
        'LBOUND - esriFieldTypeDouble: for upper bound
        'UBOUND - esriFieldTypeDouble: for lower bound

        Dim pFClass As IFeatureClass = Nothing
        Dim pFld As IFieldEdit = Nothing
        Dim pFCursor As IFeatureCursor
        Dim pFeature As IFeature
        Dim pQFilter As IQueryFilter = New QueryFilter

        Try

            ' Open the featureClass
            pFClass = BA_OpenFeatureClassFromGDB(filepath, FileName)
            Dim return_value As Integer = 0

            Dim nclass As Integer = UBound(IntervalList)

            'add fields
            'add Name field
            'check if field exist
            Dim FieldIndex As Integer

            Dim FName(0 To 2) As String
            FName(0) = BA_FIELD_NAME
            FName(1) = BA_FIELD_LBOUND
            FName(2) = BA_FIELD_UBOUND
            Dim i As Integer

            For i = 0 To 2
                FieldIndex = pFClass.FindField(FName(i))

                ' Define field type
                If FieldIndex < 0 Then 'add field
                    'Define field name
                    pFld = New Field
                    pFld.Name_2 = FName(i)
                    Select Case i
                        Case 0 'Name field
                            pFld.Type_2 = esriFieldType.esriFieldTypeString
                            pFld.Length_2 = BA_NAME_FIELD_WIDTH
                            pFld.Required_2 = True
                        Case 1, 2 'Ubound and LBound field
                            pFld.Type_2 = esriFieldType.esriFieldTypeDouble
                            pFld.Length_2 = BA_BOUND_FIELD_WIDTH
                            pFld.Required_2 = False
                    End Select

                    ' Add field
                    pFClass.AddField(pFld)
                End If
            Next

            'update value
            ' Get field index again
            Dim FI1 As Integer, FI2 As Integer, FI3 As Integer

            FI1 = pFClass.FindField(FName(0))
            FI2 = pFClass.FindField(FName(1))
            FI3 = pFClass.FindField(FName(2))

            Dim tempname As String
            'Allow for different field names for gridCode value
            Dim queryField As String = BA_FIELD_GRIDCODE
            Dim idxGrid As Integer = pFClass.FindField(queryField)
            If idxGrid < 0 Then
                queryField = BA_FIELD_GRIDCODE_GDB
                idxGrid = pFClass.FindField(queryField)
            End If

            If idxGrid > 0 Then
                For i = 1 To nclass

                    pQFilter.WhereClause = queryField & " = " & CLng(IntervalList(i).Value)
                    pFCursor = pFClass.Update(pQFilter, False)
                    pFeature = pFCursor.NextFeature

                    Do While Not pFeature Is Nothing
                        tempname = Trim(IntervalList(i).Name)
                        If Len(tempname) >= BA_NAME_FIELD_WIDTH Then 'truncate the string if it's longer than the att field width
                            tempname = Left(tempname, BA_NAME_FIELD_WIDTH - 1)
                        End If
                        pFeature.Value(FI1) = tempname
                        pFeature.Value(FI2) = IntervalList(i).LowerBound
                        pFeature.Value(FI3) = IntervalList(i).UpperBound
                        pFCursor.UpdateFeature(pFeature)
                        pFeature = pFCursor.NextFeature
                        return_value = return_value + 1
                    Loop
                Next
            Else
                Return -1
            End If
            Return return_value
        Catch ex As Exception
            Debug.Print("BA_UpdateReclassVectorAttributes Exception: " & ex.Message)
            Return -1
        Finally
            pQFilter = Nothing
            pFClass = Nothing
            pFCursor = Nothing
            pFeature = Nothing
            pFld = Nothing
        End Try
    End Function

    'Developer tool to dump contents of an interval list to the debugger
    Public Sub BA_IntervalList_Debug(ByVal iList() As BA_IntervalList)
        For i As Integer = 0 To iList.GetUpperBound(0)
            Dim aList As BA_IntervalList = iList(i)
            Debug.Print("Value ->" & aList.Value)
            Debug.Print("LowerBound ->" & aList.LowerBound)
            Debug.Print("UpperBound ->" & aList.UpperBound)
            Debug.Print("Name ->" & aList.Name)
            Debug.Print("SNOTEL ->" & aList.SNOTEL)
            Debug.Print("SnowCourse ->" & aList.SnowCourse)
            Debug.Print("Area ->" & aList.Area)
            Debug.Print("Blank ->" & aList.Blank)
        Next
    End Sub

    Public Function BA_PRISMCustom(ByVal pDoc As IMxDocument, ByVal aoiFolderBase As String, ByVal FirstMonth As Integer, ByVal LastMonth As Integer) As Integer
        '=========================================
        'Get Month List(Of Integer)
        '=========================================
        Dim MonthDiff As Short
        Dim i As Short
        Dim CombinedPRISMRaster As IGeoDataset

        Try
            Dim monthList As New List(Of Integer)
            If FirstMonth = LastMonth Then 'only one month is selected
                monthList.Add(FirstMonth)
            ElseIf FirstMonth < LastMonth Then 'single calendar year span
                MonthDiff = LastMonth - FirstMonth
                For i = 0 To MonthDiff
                    monthList.Add(FirstMonth + i)
                Next
            ElseIf FirstMonth > LastMonth Then 'cross-calendar year span
                'MonthDiff = (12 - FirstMonth) + LastMonth + 1
                For i = 1 To LastMonth
                    monthList.Add(i)
                Next
                For i = FirstMonth To 12
                    monthList.Add(i)
                Next
            End If

            '======================================================
            'Open and Consolidate PRISM Data
            'Save results to temporary file
            '======================================================

            Dim PRISMBasePath As String = BA_GeodatabasePath(aoiFolderBase, GeodatabaseNames.Prism)
            Dim PRISMPath As String
            Dim AnalysisPath As String = BA_GeodatabasePath(aoiFolderBase, GeodatabaseNames.Analysis) 'remove layers from map
            Dim response As Integer = BA_RemoveLayersInFolder(pDoc, AnalysisPath)
            If BA_File_Exists(AnalysisPath & "\" & BA_TEMP_PRISM, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                'delete raster folder
                BA_RemoveRasterFromGDB(AnalysisPath, BA_TEMP_PRISM)
            End If
            If monthList.Count = 1 Then
                CombinedPRISMRaster = BA_OpenRasterFromGDB(PRISMBasePath, BA_GetPrismFolderName(monthList.Item(0)))
                BA_SaveRasterDatasetGDB(CombinedPRISMRaster, AnalysisPath, BA_RASTER_FORMAT, BA_TEMP_PRISM)
            ElseIf monthList.Count > 1 Then
                Dim nextFolder As String
                ' Build list of filePaths from list of months
                Dim filePaths As New List(Of String)
                For Each pMonth In monthList
                    nextFolder = BA_GetPrismFolderName(pMonth)
                    PRISMPath = PRISMBasePath & "\" & nextFolder
                    filePaths.Add(PRISMPath)
                Next
                ' Call function to sum the raster bands together
                Dim success As BA_ReturnCode = BA_SumRasterBands(filePaths, aoiFolderBase, AnalysisPath, BA_TEMP_PRISM)
            End If
            Return 1
        Catch ex As Exception
            Debug.Print("BA_PRISMCustom Exception: " & ex.Message)
            Return -1
        Finally
            CombinedPRISMRaster = Nothing
        End Try
    End Function

    ' Accepts a List of raster filepaths as input; Add the raster bands together and persist
    ' in the output path/file provided by the caller
    Public Function BA_SumRasterBands(ByVal filePaths As List(Of String), ByVal aoiPath As String, _
                                      ByVal pOutPath As String, ByVal pOutFile As String) As BA_ReturnCode
        Dim pRasterDataset As IGeoDataset = Nothing
        Dim pTempBandCol As IRasterBandCollection = Nothing
        Dim pBandCol As IRasterBandCollection = New Raster
        Dim pRasterBand As IRasterBand = Nothing
        Dim pLocalOp As ILocalOp = New RasterLocalOp
        Dim pOutRaster As IGeoDataset = Nothing
        Dim tempGeodataset As IGeoDataset = Nothing
        Try
            If BA_File_ExistsRaster(aoiPath, pOutFile) Then
                BA_Remove_Raster(aoiPath, pOutFile)
            End If

            For Each filePath In filePaths
                Dim parentPath As String = "Please return"
                Dim fName As String = BA_GetBareName(filePath, parentPath)
                'pRasterDataset = BA_OpenRasterFromFile(parentPath, fName)
                pRasterDataset = BA_OpenRasterFromGDB(parentPath, fName)
                pTempBandCol = CType(pRasterDataset, IRasterBandCollection)
                pRasterBand = pTempBandCol.Item(0)
                pBandCol.AppendBand(pRasterBand)
            Next
            pOutRaster = pLocalOp.LocalStatistics(pBandCol, esriGeoAnalysisStatisticsEnum.esriGeoAnalysisStatsSum)
            'Save raster temporarily to a GRID at the AOI root
            'Geoanalyst tool unable to save to file GDB
            BA_SaveRasterDataset(pOutRaster, aoiPath, pOutFile)

            Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(pOutPath)
            If workspaceType = workspaceType.Geodatabase Then
                pOutRaster = BA_OpenRasterFromFile(aoiPath, pOutFile)
                BA_SaveRasterDatasetGDB(pOutRaster, pOutPath, BA_RASTER_FORMAT, pOutFile)
                BA_Remove_Raster(aoiPath, pOutFile)
            End If

            Return BA_ReturnCode.Success
        Catch ex As Exception
            MessageBox.Show("BA_SumRasterBands Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(tempGeodataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pOutRaster)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pBandCol)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBand)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTempBandCol)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterDataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pLocalOp)
        End Try

    End Function

End Module
