Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry
Imports System.Windows.Forms
Imports BAGIS_ClassLibrary

Module EliminateModule
    'count features in a shapefile
    Public Function BA_GetFeatureCount(ByVal filePath As String, ByVal fileName As String) As Long
        Dim returnValue As Long = 0

        Dim featName As String = BA_StandardizeShapefileName(fileName, False)
        Dim featPath As String = BA_StandardizePathString(filePath)

        'Get shape FeatureClass
        'Dim pFeatureClass As IFeatureClass = BA_OpenFeatureClassFromFile(featPath, featName)
        Dim pFeatureClass As IFeatureClass = BA_OpenFeatureClassFromGDB(featPath, featName)

        If pFeatureClass Is Nothing Then
            MessageBox.Show("Can't find Feature Class")
            Return 0
            Exit Function
        End If

        returnValue = pFeatureClass.FeatureCount(Nothing)

        pFeatureClass = Nothing
        GC.Collect()
        GC.WaitForPendingFinalizers()

        Return returnValue
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

    Public Function BA_GetLayerAttributebyPoint(ByRef pFLayer As IFeatureLayer, ByRef pPoint As IPoint, ByVal fieldName As String) As Double
        Dim pMxDoc As IMxDocument = My.ArcMap.Document
        Dim pMap As IMap = pMxDoc.FocusMap
        Dim attributeValue As Double = -9999

        If pPoint IsNot Nothing And pFLayer IsNot Nothing Then
            Dim pFSele As IFeatureSelection = _
                TryCast(pFLayer, IFeatureSelection)

            'define selection queryfilter by attribute
            Dim pSFilter As ISpatialFilter = New SpatialFilter

            pSFilter.Geometry = pPoint
            pSFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelWithin

            pFSele.SelectFeatures(pSFilter, esriSelectionResultEnum.esriSelectionResultNew, False)
            pMxDoc.ActivatedView.Refresh()  'refresh the view to see the selected features

            Dim fIndex As Long = pFLayer.FeatureClass.FindField(fieldName)

            Dim pSeleSet As ISelectionSet = pFSele.SelectionSet
            Dim pFCursor As IFeatureCursor = Nothing
            Dim pFeature As IFeature

            Try
                If fIndex >= 0 Then 'field with name "fieldName" found in the FLayer
                    'retrieve selected features using a FeatureCursor
                    pSeleSet.Search(Nothing, False, pFCursor)

                    Dim nf As Long = pSeleSet.Count
                    If nf = 1 Then
                        pFeature = pFCursor.NextFeature
                        If pFeature IsNot Nothing Then
                            attributeValue = pFeature.Value(fIndex)
                        End If
                    ElseIf nf > 1 Then
                        MsgBox("More than one polygon are selected. Please select only one polygon.")
                    Else
                        MsgBox("No polygon is selected. Please select one polygon.")
                    End If
                End If

            Catch ex As Exception
                MsgBox("AreaForSelectedFeature Exception: " & ex.Message)

            Finally
                pFeature = Nothing
                pFCursor = Nothing
                pSeleSet = Nothing
                pSFilter = Nothing
                pFLayer = Nothing
                GC.WaitForPendingFinalizers()
                GC.Collect()
            End Try
        End If

        Return attributeValue
    End Function

    Public Function BA_CountFeaturesSmallerThanOrEqualTo(ByVal featName As String, ByVal featPath As String, ByVal fieldName As String, ByVal fieldValue As Double) As Integer
        Dim returnValue As Integer = 0

        featName = BA_StandardizeShapefileName(featName, False)
        featPath = BA_StandardizePathString(featPath)

        'Get shape FeatureClass
        Dim pFeatureClass As IFeatureClass = BA_OpenFeatureClassFromGDB(featPath, featName)

        If pFeatureClass Is Nothing Then
            MessageBox.Show("Can't find Feature Class")
            Return 0
            Exit Function
        End If

        Dim queryFilter As IQueryFilter = New QueryFilter()
        Try
            queryFilter.WhereClause = """" & fieldName & """" & " <= " & Str(fieldValue)
            returnValue = pFeatureClass.FeatureCount(queryFilter)

        Catch ex As Exception
            MessageBox.Show("Exception: " + ex.Message)

        Finally
            queryFilter = Nothing
            pFeatureClass = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try

        Return returnValue
    End Function

    Public Function BA_GetPercentileValueFromFeatureClass(ByVal featName As String, ByVal featPath As String, ByVal fieldName As String, ByVal fieldValue As Double) As Double
        Dim returnValue As Double = 0
        Dim numberFeatures As Long

        featName = BA_StandardizeShapefileName(featName, False)
        featPath = BA_StandardizePathString(featPath)

        'Get shape FeatureClass
        Dim pFeatureClass As IFeatureClass = Nothing
        Dim inputWorkspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(featPath)
        If inputWorkspaceType = WorkspaceType.Raster Then
            pFeatureClass = BA_OpenFeatureClassFromFile(featPath, featName)
        ElseIf inputWorkspaceType = WorkspaceType.Geodatabase Then
            pFeatureClass = BA_OpenFeatureClassFromGDB(featPath, featName)
        End If

        If pFeatureClass Is Nothing Then
            MessageBox.Show("Can't find Feature Class")
            Return 0
            Exit Function
        End If

        Dim tblSort As ITableSort = New TableSort()
        Try
            numberFeatures = pFeatureClass.FeatureCount(Nothing)
            tblSort.Table = pFeatureClass
            tblSort.Fields = fieldName
            tblSort.Ascending(fieldName) = True
            tblSort.Sort(Nothing)

            'fieldValue is in percentile without decimal point. ie: 20% = 20
            'find the row id at the percentile selected
            Dim valueIdx As Integer = Math.Round((fieldValue / 100) * numberFeatures + 0.5)
            Dim areaIdx As Integer = pFeatureClass.FindField(BA_FIELD_AREA_SQKM)

            'get the id of the item in the valueId position
            Dim id As Integer = tblSort.IDByIndex(valueIdx)
            'get the area of that item to populate the form
            returnValue = pFeatureClass.GetFeature(id).Value(areaIdx)

        Catch ex As Exception
            MessageBox.Show("Exception: " + ex.Message)
            Return 0

        Finally
            tblSort = Nothing
            pFeatureClass = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try

        Return returnValue
    End Function
End Module
