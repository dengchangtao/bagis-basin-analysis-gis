Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.DataSourcesFile
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.DataSourcesGDB

' This module is a temporary replacement for the ArcObjects Geoprocessing fishnet tool
' There is a bug with the tool: NIM064813 that prevents it from releasing a file lock in
' ArcGIS v10.0. This bug is resolved in ArcGIS 10.1 so this module will not longer be needed
' The code in this module is based on VB6 code contributed by Robert Nicholas to the ArcScripts website
' http://arcscripts.esri.com/details.asp?dbid=12807
Public Module FishNetModule

    ' attribute field name for tracking the column/row of each polygon
    ' would normally go in ConstantsModule but this module is temporary until
    ' FishNet bug is fixed in 10.1
    Public Const BA_FIELD_COL_ROW As String = "Col_Row"

    ' Generates a fishnet shapefile. The extent and spatial reference are derived from a given shapefile.
    ' The cellsize is determined by the number of columns/rows
    Public Function BA_CustomFishnet(ByVal templateFileName As String, ByVal outputPath As String, _
                                     ByVal outputFileName As String, _
                                     ByVal LLMapX As Double, ByVal LLMapY As Double, _
                                     ByVal URMapX As Double, ByVal URMapY As Double, _
                                     ByVal numCols As Long, ByVal numRows As Long) As BA_ReturnCode
        Dim templatePath As String = "PleaseReturn"
        Dim templateFile As String = BA_GetBareName(templateFileName, templatePath)
        templateFile = BA_StandardizeShapefileName(templateFile, False, False)
        Dim templateFC As IFeatureClass = Nothing
        Dim templateGDS As IGeoDataset = Nothing
        Dim templateSpatialRef As ISpatialReference = Nothing
        Dim pWorkspaceFactory As IWorkspaceFactory = Nothing
        Dim pFWS As IFeatureWorkspace = Nothing
        Dim pOutFeatClass As IFeatureClass = Nothing

        Try
            ' extract spatial reference from template shapefile
            'templateFC = BA_OpenFeatureClassFromFile(templatePath, templateFile)
            templateFC = BA_OpenFeatureClassFromGDB(templatePath, templateFile)
            templateGDS = CType(templateFC, IGeoDataset)
            templateSpatialRef = templateGDS.SpatialReference


            ' create geometry field
            Dim pFields As IFields = New Fields
            Dim pFieldsEdit As IFieldsEdit = pFields
            Dim pField As IField = New Field
            Dim pFieldEdit As IFieldEdit = pField
            pFieldEdit.Name_2 = BA_FIELD_SHAPE
            pFieldEdit.Type_2 = esriFieldType.esriFieldTypeGeometry
            Dim pGeomDef As IGeometryDef = New GeometryDef
            Dim pGeomDefEdit As IGeometryDefEdit = pGeomDef
            ' only support polygons for now
            pGeomDefEdit.GeometryType_2 = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon
            pGeomDefEdit.SpatialReference_2 = templateSpatialRef
            pFieldEdit.GeometryDef_2 = pGeomDef

            ' add geometry field to fields collection
            pFieldsEdit.AddField(pField)

            ' create row_col field because we are using polygons
            pField = New Field
            pFieldEdit = pField
            With pFieldEdit
                .Name_2 = BA_FIELD_COL_ROW
                .Type_2 = esriFieldType.esriFieldTypeString
                .Length_2 = 20
            End With

            ' add row_col field to fields collection
            pFieldsEdit.AddField(pField)

            ' create the shapefile
            pWorkspaceFactory = New FileGDBWorkspaceFactory
            pFWS = pWorkspaceFactory.OpenFromFile(outputPath, 0)
            pOutFeatClass = pFWS.CreateFeatureClass(outputFileName, pFields, Nothing, _
                            Nothing, esriFeatureType.esriFTSimple, BA_FIELD_SHAPE, "")
            'Dim LLMapX As Double = pEnv.XMin
            'Dim LLMapY As Double = pEnv.YMin
            'Dim mapXOff As Double = pEnv.XMax - pEnv.XMin
            'Dim mapYOff As Double = pEnv.YMax - pEnv.YMin

            CreateFeatures(pOutFeatClass, LLMapX, LLMapY, URMapX, _
                           URMapY, numCols, numRows)

            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_Fishnet Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(templateFC)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(templateGDS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(templateSpatialRef)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pWorkspaceFactory)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFWS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pOutFeatClass)
        End Try

    End Function

    Private Sub CreateFeatures(ByVal pOutFeatClass As IFeatureClass, _
                               ByVal LLMapX As Double, ByVal LLMapY As Double, _
                               ByVal URMapX As Double, ByVal URMapY As Double, _
                               ByVal numCols As Long, ByVal numRows As Long)
        Dim lngTotalFeatureCount As Long, lngCurrentFeatureCount As Long
        lngTotalFeatureCount = numRows * numCols

        ' create the first polygon . it will be copied many times.
        Dim pPointColl As IPointCollection = New Polygon
        Dim pPnt1 As IPoint = New Point
        Dim pPnt2 As IPoint = New Point
        Dim pPnt3 As IPoint = New Point
        Dim pPnt4 As IPoint = New Point
        Dim pClone As IClone = Nothing
        Dim pInsertFeatureCursor As IFeatureCursor = Nothing
        Dim pInsertFeatureBuffer As IFeatureBuffer = Nothing
        Dim i As Long, j As Long
        Dim dblDeltaX As Double, dblDeltaY As Double
        Dim pTrans2D As ITransform2D

        Try
            ' How wide is the envelope?
            Dim mapXOff As Double = URMapX - LLMapX
            ' How tall is the envelope?
            Dim mapYOff As Double = URMapY - LLMapY
            ' Calculate cell width from envelope width and column count
            Dim dblWidth As Double = mapXOff / numCols
            ' Calculate cell height from envelope height and row count
            Dim dblHeight As Double = mapYOff / numRows

            pPnt1.X = LLMapX - dblWidth
            pPnt1.Y = LLMapY - dblHeight
            pPnt2.X = LLMapX - dblWidth
            pPnt2.Y = LLMapY
            pPnt3.X = LLMapX
            pPnt3.Y = LLMapY
            pPnt4.X = LLMapX
            pPnt4.Y = LLMapY - dblHeight
            pPointColl.AddPoint(pPnt1)
            pPointColl.AddPoint(pPnt2)
            pPointColl.AddPoint(pPnt3)
            pPointColl.AddPoint(pPnt4)
            ' Convert point collection to polygon
            Dim pFirstPolygon As IPolygon = pPointColl
            pFirstPolygon.Close()
            pClone = pFirstPolygon

            ' write the polygons out to the shapefile.
            pInsertFeatureCursor = pOutFeatClass.Insert(True)
            pInsertFeatureBuffer = pOutFeatClass.CreateFeatureBuffer
            Dim idxIndexPos As Long = pOutFeatClass.FindField(BA_FIELD_COL_ROW)
            Dim pPolygon As IPolygon
            For i = 1 To numCols
                dblDeltaX = (i * dblWidth)
                For j = 1 To numRows
                    dblDeltaY = (j * dblHeight)
                    pPolygon = pClone.Clone
                    pTrans2D = pPolygon
                    pTrans2D.Move(dblDeltaX, dblDeltaY)
                    pInsertFeatureBuffer.Shape = pPolygon
                    pInsertFeatureBuffer.Value(idxIndexPos) = i & " - " & (numRows - j + 1)
                    pInsertFeatureCursor.InsertFeature(pInsertFeatureBuffer)
                    lngCurrentFeatureCount = lngCurrentFeatureCount + 1
                Next
            Next

        Catch ex As Exception
            Debug.Print("CreateFeatures Exception: " & ex.Message)
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pInsertFeatureBuffer)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pInsertFeatureCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pClone)
        End Try


    End Sub

End Module
