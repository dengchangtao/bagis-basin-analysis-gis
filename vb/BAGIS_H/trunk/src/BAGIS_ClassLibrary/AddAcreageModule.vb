Imports System.Windows.Forms
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.DataSourcesRaster

Public Module AddAcreageModule

    'Return Value
    '0 - Success
    '-1 - file name or path not found
    '-2 - Unknown Error
    'get shape area and write to the attribute table
    Public Function BA_AddShapeAreaToAttrib(ByVal shapefile_pathname As String) As Integer
        Dim filepath As String = ""
        Dim filename As String
        Dim shapearea As Double = 0

        If String.IsNullOrEmpty(shapefile_pathname) Then 'not a valid input
            Return -1
            Exit Function
        End If

        filename = BA_GetBareName(shapefile_pathname, filepath)

        If filename Is "" Then
            Return -1
            Exit Function
        End If

        'remove the last character if it's a backslash
        If Right(filepath, 1) = "\" Then filepath = Left(filepath, Len(filepath) - 1)

        'Get shape FeatureClass
        Dim pFeatureClass As IFeatureClass = Nothing
        Dim pWorkspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(filepath)
        If pWorkspaceType = WorkspaceType.Geodatabase Then
            pFeatureClass = BA_OpenFeatureClassFromGDB(filepath, filename)
        ElseIf pWorkspaceType = WorkspaceType.Raster Then
            filename = BA_StandardizeShapefileName(filename, False, False)
            pFeatureClass = BA_OpenFeatureClassFromFile(filepath, filename) 'filename cannot have .shp extension, the sub appends it
        End If

        If pFeatureClass Is Nothing Then
            Return -2
            Exit Function
        End If

        Dim indexArea As Long = pFeatureClass.FindField(BA_FIELD_AREA_SQKM)
        Dim indexAcre As Long = pFeatureClass.FindField(BA_FIELD_AREA_ACRE)

        ' Check for Area-Sq.Km. field - if don't exist then add it
        If indexArea < 0 Then
            Dim pFieldArea As IFieldEdit = New Field
            With pFieldArea
                .Type_2 = 3 'esriFieldTypeDouble
                .Name_2 = BA_FIELD_AREA_SQKM
            End With
            pFeatureClass.AddField(pFieldArea)
            indexArea = pFeatureClass.FindField(BA_FIELD_AREA_SQKM)
        End If

        ' Check for Area-Acre field - if don't exist then add it
        If indexAcre < 0 Then
            Dim pFieldAcre As IFieldEdit = New Field
            With pFieldAcre
                .Type_2 = 3 'esriFieldTypeDouble
                .Name_2 = BA_FIELD_AREA_ACRE
            End With
            pFeatureClass.AddField(pFieldAcre)
            indexAcre = pFeatureClass.FindField(BA_FIELD_AREA_ACRE)
        End If

        'Extract Area
        Dim pCursor As IFeatureCursor
        Dim pFeature As IFeature
        Dim pGeometry As IGeometry
        Dim pArea As IArea
        Dim areaValue As Double
        Dim MetersPerUnit As Double

        'First get the unit from the projected coordinate system
        Dim geoDataSet As IGeoDataset = pFeatureClass
        Dim pSpRef As ISpatialReference = geoDataSet.SpatialReference
        Dim projCoordSys As IProjectedCoordinateSystem = pSpRef
        Dim pLinearUnit As ILinearUnit = projCoordSys.CoordinateUnit

        MetersPerUnit = pLinearUnit.MetersPerUnit
        'MsgBox("MetersPerUnit = " & MetersPerUnit)

        Try
            pCursor = pFeatureClass.Update(Nothing, False)
            pFeature = pCursor.NextFeature
            Do While Not pFeature Is Nothing
                pGeometry = pFeature.Shape
                pArea = pGeometry
                areaValue = pArea.Area
                pFeature.Value(indexArea) = areaValue * (MetersPerUnit ^ 2) / (1000 ^ 2)  'Convert SqMeters to Sq.Km.
                pFeature.Value(indexAcre) = areaValue * (MetersPerUnit ^ 2) / BA_SQ_METERS_PER_ACRE
                'MsgBox("Shape Area = " & pFeature.Value(indexArea) & " Sq.Km., " & pFeature.Value(indexAcre) & " Acre")
                pCursor.UpdateFeature(pFeature)
                pFeature = pCursor.NextFeature
            Loop
        Catch ex As Exception
            MessageBox.Show("Unable to get the area!")
            MessageBox.Show("Exception: " + ex.Message)
            Return -2
        Finally
            pFeature = Nothing
            pCursor = Nothing
            pArea = Nothing
            pGeometry = Nothing
            pFeatureClass = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
        Return 0
    End Function

    'Return Value
    '0 - Success
    '-1 - file name or path not found
    '-2 - field name not found

    Public Function BA_GetDataStatistics(ByVal filePath As String, ByVal fieldName As String, ByRef statResults As BA_DataStatistics)

        Dim fPath As String = ""
        Dim fileName As String = BA_GetBareName(filePath, fPath)

        If fileName Is "" Then
            Return -1
        End If

        If Right(fPath, 1) = "\" Then
            fPath = Left(fPath, Len(fPath) - 1)
        End If
        If fileName.Contains(".shp") Then
            fileName = Left(fileName, Len(fileName) - Len(".shp"))
        End If
        'Get shape FeatureClass
        Dim pFeatureClass As IFeatureClass = Nothing
        Dim inputWorkspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(filePath)
        If inputWorkspaceType = WorkspaceType.Raster Then
            pFeatureClass = BA_OpenFeatureClassFromFile(fPath, fileName)
        ElseIf inputWorkspaceType = WorkspaceType.Geodatabase Then
            pFeatureClass = BA_OpenFeatureClassFromGDB(fPath, fileName)
        End If

        If pFeatureClass Is Nothing Then
            Return -1
        End If

        Dim idxField As Long = pFeatureClass.FindField(fieldName)

        If idxField < 0 Then
            Return -2
        End If

        Dim pCursor As ICursor = pFeatureClass.Search(Nothing, False)

        Dim pData As IDataStatistics = New DataStatistics
        Dim pResults As ESRI.ArcGIS.esriSystem.IStatisticsResults

        pData.Field = fieldName
        pData.Cursor = pCursor
        pResults = pData.Statistics
        statResults.Count = pResults.Count
        statResults.Maximum = pResults.Maximum
        statResults.Mean = pResults.Mean
        statResults.Minimum = pResults.Minimum
        statResults.StandardDeviation = pResults.StandardDeviation
        statResults.Sum = pResults.Sum

        Return 0
    End Function

    'get shape area and write to the attribute table
    Public Function BA_AddCTAndNonCTToAttrib(ByVal shpFilenameWithPath As String) As BA_ReturnCode
        Dim filepath As String = ""
        Dim filename As String

        If String.IsNullOrEmpty(shpFilenameWithPath) Then 'not a valid input
            Return BA_ReturnCode.UnknownError
            Exit Function
        End If

        filename = BA_GetBareName(shpFilenameWithPath, filepath)

        If filename Is "" Then
            Return BA_ReturnCode.UnknownError
            Exit Function
        End If

        'remove the last character if it's a backslash
        If Right(filepath, 1) = "\" Then filepath = Left(filepath, Len(filepath) - 1)

        If filename.Contains(".shp") Then
            filename = filename.Remove(Len(filename) - Len(".shp"), Len(".shp"))
        End If

        'Get shape FeatureClass
        'Dim pFeatureClass As IFeatureClass = BA_OpenFeatureClassFromFile(filepath, filename)
        Dim pFeatureClass As IFeatureClass = BA_OpenFeatureClassFromGDB(filepath, filename)
        If pFeatureClass Is Nothing Then
            Return BA_ReturnCode.UnknownError
            Exit Function
        End If

        Dim idxCt As Integer = pFeatureClass.FindField(BA_FIELD_HRUID_CO)
        Dim idxNonCt As Integer = pFeatureClass.FindField(BA_FIELD_HRUID_NC)
        Dim idxGridCd As Integer = pFeatureClass.FindField(BA_FIELD_GRIDCODE_GDB)
        'ESRI changed the field name of the grid code in 10.1 or later so we have to check for both
        If idxGridCd = -1 Then
            idxGridCd = pFeatureClass.FindField(BA_FIELD_GRIDCODE)
        End If

        'check if GRIDCODE field exist - if don't exist then raise error
        If idxGridCd < 0 Then
            Return BA_ReturnCode.UnknownError
            Exit Function
        End If

        ' Check for HRUID_CO field - if don't exist then add it
        If idxCt < 0 Then
            Dim pFieldCont As IFieldEdit = New Field
            With pFieldCont
                .Type_2 = esriFieldType.esriFieldTypeInteger
                .Name_2 = BA_FIELD_HRUID_CO
            End With
            pFeatureClass.AddField(pFieldCont)
            idxCt = pFeatureClass.FindField(BA_FIELD_HRUID_CO)
        End If

        ' Check for HRUID_NC field - if don't exist then add it
        If idxNonCt < 0 Then
            Dim pFieldNCont As IFieldEdit = New Field
            With pFieldNCont
                .Type_2 = esriFieldType.esriFieldTypeInteger
                .Name_2 = BA_FIELD_HRUID_NC
            End With
            pFeatureClass.AddField(pFieldNCont)
            idxNonCt = pFeatureClass.FindField(BA_FIELD_HRUID_NC)
        End If

        'Extract Area
        Dim pCursor As IFeatureCursor
        Dim pFeature As IFeature
        Dim counter As Integer = 1
        Try
            pCursor = pFeatureClass.Update(Nothing, False)
            pFeature = pCursor.NextFeature
            Do While Not pFeature Is Nothing
                pFeature.Value(idxNonCt) = pFeature.Value(idxGridCd)
                pFeature.Value(idxCt) = counter
                counter = counter + 1
                pCursor.UpdateFeature(pFeature)
                pFeature = pCursor.NextFeature
            Loop
        Catch ex As Exception
            MessageBox.Show("BA_AddCTAndNonCTToAttrib Exception: " + ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            pFeature = Nothing
            pCursor = Nothing
            pFeatureClass = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
        Return BA_ReturnCode.Success

    End Function
    'Copy attributes from source Raster to destination vector or Raster
    Public Function BA_CopyAttribRasterToVector(ByVal srcRasPath As String, _
                                                ByVal srcRasName As String, _
                                                ByVal srcField1 As String, _
                                                ByVal srcField2 As String, _
                                                ByVal dstPath As String, _
                                                ByVal dstName As String, _
                                                ByVal dstField1 As String, _
                                                ByVal dstField2 As String) As BA_ReturnCode

        Try
            Dim geoDataSet As IGeoDataset = BA_OpenRasterFromFile(srcRasPath, srcRasName)
            If geoDataSet Is Nothing Then
                Return BA_ReturnCode.UnknownError
                Exit Function
            End If

            Dim rasDataset As IRasterDataset = CType(geoDataSet, IRasterDataset)
            Dim pBandCol As IRasterBandCollection = rasDataset
            Dim pRasterBand As IRasterBand = pBandCol.Item(0)
            Dim pSrcTable As ITable = pRasterBand.AttributeTable

            Dim idxSrcFld1 As Integer = pSrcTable.FindField(srcField1)
            Dim idxSrcFld2 As Integer = pSrcTable.FindField(srcField2)

            'If source fields don't exist then return error
            If idxSrcFld1 < 0 Or idxSrcFld2 < 0 Then
                Return BA_ReturnCode.UnknownError
                Exit Function
            End If

            'Open dstPath/dstName to check layer type
            Dim dstIsRaster As Boolean

            Dim pZoneDataset As IGeoDataset = BA_GetDataSet(dstPath, dstName)

            If TypeOf pZoneDataset Is IFeatureClass Then
                dstIsRaster = False
            ElseIf TypeOf pZoneDataset Is IRasterDataset Then
                dstIsRaster = True
            End If

            Dim pFeatureClass As IFeatureClass = Nothing
            Dim pDstTable As ITable = Nothing
            Dim idxDstFld1 As Integer
            Dim idxDstFld2 As Integer
            If Not dstIsRaster Then
                pFeatureClass = BA_OpenFeatureClassFromFile(dstPath, dstName)
                If pFeatureClass Is Nothing Then
                    Return BA_ReturnCode.UnknownError
                    Exit Function
                End If

                idxDstFld1 = pFeatureClass.FindField(dstField1)
                idxDstFld2 = pFeatureClass.FindField(dstField2)

                ' If the destination fields don't exist then add them
                If idxDstFld1 < 0 Then
                    Dim pField1 As IFieldEdit = New Field
                    pField1.Type_2 = esriFieldType.esriFieldTypeInteger
                    pField1.Name_2 = dstField1
                    pFeatureClass.AddField(pField1)
                    idxDstFld1 = pFeatureClass.FindField(dstField1)
                End If

                If idxDstFld2 < 0 Then
                    Dim pField2 As IFieldEdit = New Field
                    pField2.Type_2 = esriFieldType.esriFieldTypeInteger
                    pField2.Name_2 = dstField2
                    pFeatureClass.AddField(pField2)
                    idxDstFld2 = pFeatureClass.FindField(dstField2)
                End If
            Else
                Dim tmpDataset As IRasterDataset = CType(pZoneDataset, IRasterDataset)
                Dim pTmpBandCol As IRasterBandCollection = tmpDataset
                Dim pTmpRasterBand As IRasterBand = pTmpBandCol.Item(0)
                pDstTable = pTmpRasterBand.AttributeTable

                idxDstFld1 = pDstTable.FindField(dstField1)
                idxDstFld2 = pDstTable.FindField(dstField2)

                ' If the destination fields don't exist then add them
                If idxDstFld1 < 0 Then
                    Dim pField1 As IFieldEdit = New Field
                    pField1.Type_2 = esriFieldType.esriFieldTypeInteger
                    pField1.Name_2 = dstField1
                    pDstTable.AddField(pField1)
                    idxDstFld1 = pDstTable.FindField(dstField1)
                End If

                If idxDstFld2 < 0 Then
                    Dim pField2 As IFieldEdit = New Field
                    pField2.Type_2 = esriFieldType.esriFieldTypeInteger
                    pField2.Name_2 = dstField2
                    pDstTable.AddField(pField2)
                    idxDstFld2 = pDstTable.FindField(dstField2)
                End If
            End If

            'Copy attributes to the destination vector
            Dim pSrcCursor As ICursor = Nothing
            Dim pSrcRow As IRow = Nothing
            Dim pDstCursor As ICursor = Nothing
            Dim pDstRow As IRow = Nothing

            pSrcCursor = pSrcTable.Search(Nothing, False)
            pSrcRow = pSrcCursor.NextRow

            If dstIsRaster Then
                pDstCursor = pDstTable.Update(Nothing, False)
                pDstRow = pDstCursor.NextRow
            Else
                pDstCursor = pFeatureClass.Update(Nothing, False)
                pDstRow = pDstCursor.NextRow
            End If

            Do While (Not pSrcRow Is Nothing) And (Not pDstRow Is Nothing)
                pDstRow.Value(idxDstFld1) = pSrcRow.Value(idxSrcFld1)
                pDstRow.Value(idxDstFld2) = pSrcRow.Value(idxSrcFld2)
                pDstCursor.UpdateRow(pDstRow)
                pDstRow = pDstCursor.NextRow
                pSrcRow = pSrcCursor.NextRow
            Loop

            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(geoDataSet)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(rasDataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBand)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pSrcTable)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pSrcCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pSrcRow)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDstTable)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDstCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeatureClass)

        Catch ex As Exception
            MessageBox.Show("Exception: " + ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
        Return BA_ReturnCode.Success
    End Function

    'Copy attributes from source Raster to destination Raster
    Public Function BA_CopyAttribRasterToRaster(ByVal srcRasPath As String, _
                                                ByVal srcRasName As String, _
                                                ByVal dstPath As String, _
                                                ByVal dstName As String) As BA_ReturnCode

        Dim geoDataSet As IGeoDataset = Nothing
        Dim rasDataset As IRasterDataset = Nothing
        Dim pBandCol As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pSrcTable As ITable = Nothing
        Dim pSrcCursor As ICursor = Nothing
        Dim pSrcRow As IRow = Nothing
        Dim pDstQueryFilter As IQueryFilter = New QueryFilter
        Dim pDstTable As ITable = Nothing
        Dim pDstCursor As ICursor = Nothing
        Dim pDstRow As IRow = Nothing
        Dim pFeatureClass As IFeatureClass = Nothing
        Dim pZoneDataset As IGeoDataset = Nothing
        Dim tmpDataset As IRasterDataset = Nothing
        Dim pTmpBandCol As IRasterBandCollection = Nothing
        Dim pTmpRasterBand As IRasterBand = Nothing
        Dim pField As IFieldEdit = New Field
        Dim dstValueFieldName As String = BA_FIELD_PRMS_MAJORITY
        Dim dstSlpFieldName As String = BA_FIELD_RADP_SLP
        Dim dstAspFieldName As String = BA_FIELD_RADP_ASP
        Dim srcValueFieldName As String = BA_FIELD_VALUE
        Dim srcSlpFieldName As String = BA_FIELD_TEMP_SLOPE
        Dim srcAspFieldName As String = BA_FIELD_TEMP_ASPECT

        Try
            'Open combined raster (SlpAsp)
            geoDataSet = BA_OpenRasterFromGDB(srcRasPath, srcRasName)
            If geoDataSet Is Nothing Then
                Return BA_ReturnCode.UnknownError
                Exit Function
            End If

            rasDataset = CType(geoDataSet, IRasterDataset)
            pBandCol = rasDataset
            pRasterBand = pBandCol.Item(0)
            pSrcTable = pRasterBand.AttributeTable

            If pSrcTable Is Nothing Then
                Return BA_ReturnCode.ReadError
            End If

            'Value
            Dim idxSrcFld As Integer = pSrcTable.FindField(srcValueFieldName)
            'TempSlope
            Dim idxSrcSlpFld = pSrcTable.FindField(srcSlpFieldName)
            'TempAspect
            Dim idxSrcAspFld = pSrcTable.FindField(srcAspFieldName)

            'If source field don't exist then return error
            If idxSrcFld < 0 Or idxSrcSlpFld < 0 Or idxSrcAspFld < 0 Then
                Debug.print("Unable to locate required fields on source file: " & srcRasName)
                Return BA_ReturnCode.UnknownError
                Exit Function
            End If

            'Open dstPath/dstName to check layer type
            Dim dstIsRaster As Boolean
            pZoneDataset = BA_GetDataSet(dstPath, dstName)

            If TypeOf pZoneDataset Is IFeatureClass Then
                dstIsRaster = False
            ElseIf TypeOf pZoneDataset Is IRasterDataset Then
                dstIsRaster = True
            End If

            Dim idxDstSlpFld As Integer
            Dim idxDstAspFld As Integer
            If Not dstIsRaster Then
                pFeatureClass = BA_OpenFeatureClassFromGDB(dstPath, dstName)
                If pFeatureClass Is Nothing Then
                    Return BA_ReturnCode.UnknownError
                    Exit Function
                End If

                idxDstSlpFld = pFeatureClass.FindField(dstSlpFieldName)
                idxDstAspFld = pFeatureClass.FindField(dstAspFieldName)

                ' If the slope field doesn't exist then add it
                If idxDstSlpFld < 0 Then
                    pField = New Field
                    pField.Type_2 = esriFieldType.esriFieldTypeInteger
                    pField.Name_2 = dstSlpFieldName
                    pFeatureClass.AddField(pField)
                    idxDstSlpFld = pFeatureClass.FindField(dstSlpFieldName)
                End If

                ' If the aspect field doesn't exist then add it
                If idxDstAspFld < 0 Then
                    pField = New Field
                    pField.Type_2 = esriFieldType.esriFieldTypeInteger
                    pField.Name_2 = dstAspFieldName
                    pFeatureClass.AddField(pField)
                    idxDstAspFld = pFeatureClass.FindField(dstAspFieldName)
                End If

            Else
                tmpDataset = CType(pZoneDataset, IRasterDataset)
                pTmpBandCol = tmpDataset
                pTmpRasterBand = pTmpBandCol.Item(0)
                pDstTable = pTmpRasterBand.AttributeTable

                idxDstSlpFld = pDstTable.FindField(dstSlpFieldName)
                idxDstAspFld = pDstTable.FindField(dstAspFieldName)

                ' If the slope field doesn't exist then add it
                If idxDstSlpFld < 0 Then
                    pField = New Field
                    pField.Type_2 = esriFieldType.esriFieldTypeInteger
                    pField.Name_2 = dstSlpFieldName
                    pDstTable.AddField(pField)
                    idxDstSlpFld = pDstTable.FindField(dstSlpFieldName)
                End If

                ' If the aspect field doesn't exist then add it
                If idxDstAspFld < 0 Then
                    pField = New Field
                    pField.Type_2 = esriFieldType.esriFieldTypeInteger
                    pField.Name_2 = dstAspFieldName
                    pDstTable.AddField(pField)
                    idxDstAspFld = pDstTable.FindField(dstAspFieldName)
                End If

            End If

            'Copy attributes to the destination layer
            'Move to first position in pDstCursor
            pSrcCursor = pSrcTable.Search(Nothing, False)
            pSrcRow = pSrcCursor.NextRow

            'Loop through destination table
            While pSrcRow IsNot Nothing
                ' Get the combined PRMS value
                Dim pValue As Object = pSrcRow.Value(idxSrcFld)
                pDstQueryFilter.WhereClause = dstValueFieldName & " = " & pValue
                Debug.Print(pDstQueryFilter.WhereClause)
                If Not dstIsRaster Then
                    pDstCursor = pFeatureClass.Update(pDstQueryFilter, False)
                Else
                    pDstCursor = pDstTable.Update(pDstQueryFilter, False)
                End If
                ' Get first row returned from query
                pDstRow = pDstCursor.NextRow
                While pDstRow IsNot Nothing
                    pDstRow.Value(idxDstSlpFld) = pSrcRow.Value(idxSrcSlpFld)
                    pDstRow.Value(idxDstAspFld) = pSrcRow.Value(idxSrcAspFld)
                    pDstCursor.UpdateRow(pDstRow)
                    pDstRow = pDstCursor.NextRow
                End While
                pSrcRow = pSrcCursor.NextRow
            End While
            pDstCursor.Flush()

            Return BA_ReturnCode.Success
        Catch ex As Exception
            MessageBox.Show("BA_CopyAttribRasterToRaster Exception: " + ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(geoDataSet)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(rasDataset)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pBandCol)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBand)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pSrcTable)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pSrcCursor)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pSrcRow)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDstQueryFilter)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDstTable)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDstCursor)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDstRow)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeatureClass)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pZoneDataset)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(tmpDataset)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTmpRasterBand)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTmpBandCol)
            'ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pField)
            geoDataSet = Nothing
            rasDataset = Nothing
            pBandCol = Nothing
            pRasterBand = Nothing
            pSrcTable = Nothing
            pSrcCursor = Nothing
            pSrcRow = Nothing
            pDstQueryFilter = Nothing
            pDstTable = Nothing
            pDstCursor = Nothing
            pDstRow = Nothing
            pFeatureClass = Nothing
            pZoneDataset = Nothing
            tmpDataset = Nothing
            pTmpRasterBand = Nothing
            pTmpBandCol = Nothing
            pField = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    Public Function BA_AddReclassAttribToRas(ByVal rasterPath As String, ByVal rasterName As String, _
                                             ByVal reclassItems As ReclassItem()) As BA_ReturnCode

        Dim pCursor As ICursor = Nothing
        Dim geoDataSet As IGeoDataset = Nothing
        Dim pTable As ITable = Nothing

        Try
            'Dim rasterpath As String = ""
            'Dim rastername As String = BA_GetBareName(inputRasPath, rasterpath)

            If rasterName Is "" Then
                Return BA_ReturnCode.UnknownError
                Exit Function
            End If

            BA_StandardizePathString(rasterPath)

            geoDataSet = BA_OpenRasterFromGDB(rasterPath, rasterName)
            If geoDataSet Is Nothing Then
                Return BA_ReturnCode.UnknownError
                Exit Function
            End If

            Dim rasDataset As IRasterDataset = CType(geoDataSet, IRasterDataset)
            Dim pBandCol As IRasterBandCollection = rasDataset
            Dim pRasterBand As IRasterBand = pBandCol.Item(0)
            pTable = pRasterBand.AttributeTable

            Dim idxValFld As Integer = pTable.FindField(BA_FIELD_VALUE)

            'If field doesn't exist then return error
            If idxValFld < 0 Then
                Return BA_ReturnCode.UnknownError
                Exit Function
            End If

            'Dim idxAddFld1 As Integer = pTable.FindField("NewValue")
            Dim idxAddFld2 As Integer = pTable.FindField("OutputDescr")

            ' If the destination fields don't exist then add them
            'If idxAddFld1 < 0 Then
            '    Dim pField As IFieldEdit = New Field
            '    pField.Type_2 = esriFieldType.esriFieldTypeInteger
            '    pField.Name_2 = "NewValue"
            '    pTable.AddField(pField)
            '    idxAddFld1 = pTable.FindField("NewValue")
            'End If
            If idxAddFld2 < 0 Then
                Dim pField As IFieldEdit = New Field
                pField.Type_2 = esriFieldType.esriFieldTypeString
                pField.Name_2 = "OutputDescr"
                pTable.AddField(pField)
                idxAddFld2 = pTable.FindField("OutputDescr")
            End If

            'Copy attributes from reclass table to the raster
            Dim pRow As IRow = Nothing

            pCursor = pTable.Update(Nothing, False)
            pRow = pCursor.NextRow
            Do While pRow IsNot Nothing
                For Each item In reclassItems
                    If pRow.Value(idxValFld) = item.OutputValue Then
                        'pRow.Value(idxAddFld1) = item.OutputValue
                        pRow.Value(idxAddFld2) = item.OutputDescr
                        pCursor.UpdateRow(pRow)
                        Exit For
                    End If
                Next
                pRow = pCursor.NextRow
            Loop
            Return BA_ReturnCode.Success
        Catch ex As Exception
            MessageBox.Show("BA_AddReclassAttribToRas Exception: " + ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(geoDataSet)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTable)
        End Try
    End Function
End Module
