Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geometry
Imports System.Windows.Forms
Imports System.Text
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.GeoAnalyst

Public Module MapsModule

    'This is the original method. Not sure why it duplicates entries in the Symbology tab. Replace with method below
    '15-DEC-2011 LCB
    'Public Function BA_DisplayRasterWithSymbolOrig(ByVal pMxDoc As IMxDocument, ByRef LayerPathName As String, _
    '                                           ByRef DisplayName As String, ByRef DisplayStyle As MapsDisplayStyle, _
    '                                           ByRef Transparency As Short, ByRef workspaceType As WorkspaceType) As Short
    '    Dim File_Path As String = ""
    '    Dim File_Name As String = ""
    '    Dim i As Short
    '    Dim pRasterDS As IRasterDataset = Nothing
    '    Dim pRaster As IRaster
    '    Dim pRLayer As IRasterLayer
    '    Dim pTempLayer As ILayer
    '    Dim pLayerEffects As ILayerEffects = Nothing
    '    Dim pFSymbol As ISimpleFillSymbol = Nothing
    '    Dim pRasRenderer As IRasterRenderer = Nothing
    '    Dim pUVRenderer As IRasterUniqueValueRenderer = Nothing
    '    Dim pStyleGallery As IStyleGallery = Nothing
    '    Dim pEnumStyleGallery As IEnumStyleGalleryItem = Nothing
    '    Dim pStyleItem As IStyleGalleryItem = Nothing
    '    Dim pColorRamp As IColorRamp = Nothing
    '    Dim pTable As ITable
    '    Dim pBandCol As IRasterBandCollection
    '    Dim pBand As IRasterBand
    '    Dim returnValue As Short = -1

    '    If String.IsNullOrEmpty(LayerPathName) Or Not BA_File_Exists(LayerPathName, workspaceType, esriDatasetType.esriDTRasterDataset) Then
    '        Return -2
    '        Exit Function
    '    End If

    '    File_Name = BA_GetBareName(LayerPathName, File_Path)

    '    'exit if file_name is null
    '    'text exists for the setting of this layer
    '    If workspaceType = workspaceType.Raster Then
    '        pRasterDS = BA_OpenRasterFromFile(File_Path, File_Name)
    '    ElseIf workspaceType = workspaceType.Geodatabase Then

    '        pRasterDS = BA_OpenRasterFromGDB(File_Path, File_Name)
    '    End If

    '    If pRasterDS Is Nothing Then
    '        Return -2
    '        Exit Function
    '    End If

    '    'add featureclass to current data frame
    '    pRLayer = New RasterLayer
    '    pRLayer.CreateFromDataset(pRasterDS)
    '    pRLayer.Name = DisplayName

    '    'Count the number of unique values
    '    pRaster = pRLayer.Raster
    '    pBandCol = pRaster
    '    pBand = pBandCol.Item(0)

    '    'check attribute table
    '    pTable = pBand.AttributeTable

    '    ' Getting color ramp from Style Gallery
    '    Dim pMap As IMap = pMxDoc.FocusMap

    '    Try
    '        Dim nUValue As Short = pTable.RowCount(Nothing)
    '        Dim FieldIndex As Short = pTable.FindField("Value")

    '        'read the user-defined attributes from the raster
    '        Dim IntervalList() As BA_IntervalList = Nothing
    '        Dim response As BA_ReturnCode = BA_ReadReclassRasterAttributeGDB(IntervalList, File_Path, File_Name)

    '        pColorRamp = New PresetColorRamp
    '        pStyleGallery = pMxDoc.StyleGallery

    '        Dim StyleName As String
    '        Dim StyleCategory As String
    '        Select Case DisplayStyle
    '            Case MapsDisplayStyle.Aspect '"ASPECT"
    '                StyleName = "Aspect"
    '                StyleCategory = "Default Ramps"
    '            Case MapsDisplayStyle.Elevation '"ELEVATION"
    '                StyleName = "Elevation #2"
    '                StyleCategory = "Default Ramps"
    '            Case MapsDisplayStyle.Precipitation '"PRECIPITATION"
    '                StyleName = "Precipitation"
    '                StyleCategory = "Default Ramps"
    '            Case MapsDisplayStyle.Range_Random '"Range/Random"
    '                StyleName = "Basic Random"
    '                StyleCategory = "Default Schemes"
    '            Case MapsDisplayStyle.Slope '"SLOPE"
    '                StyleName = "Slope"
    '                StyleCategory = "Spatial Ramps"
    '            Case Else
    '                StyleName = "Black to White"
    '                StyleCategory = "Default Ramps"
    '        End Select

    '        pEnumStyleGallery = pStyleGallery.Items("Color Ramps", "ESRI.style", StyleCategory)
    '        pEnumStyleGallery.Reset()

    '        pStyleItem = pEnumStyleGallery.Next
    '        Do Until pStyleItem Is Nothing
    '            If pStyleItem.Name = StyleName Then
    '                pColorRamp = pStyleItem.Item
    '                Exit Do
    '            End If
    '            pStyleItem = pEnumStyleGallery.Next
    '        Loop

    '        'assign value to the colorramp
    '        With pColorRamp
    '            .Size = UBound(IntervalList)
    '            .CreateRamp(True)
    '        End With

    '        'create raster renderer
    '        pUVRenderer = New RasterUniqueValueRenderer
    '        pRasRenderer = pUVRenderer
    '        pRasRenderer.Raster = pRLayer.Raster
    '        pRasRenderer.Update()

    '        pFSymbol = New SimpleFillSymbol

    '        Dim Value As Object
    '        For i = 0 To nUValue - 1
    '            Value = IntervalList(i + 1).Value
    '            pUVRenderer.AddValue(0, i, Value)
    '            If String.Compare(IntervalList(i + 1).Name, BA_UNKNOWN) <> 0 Then
    '                pUVRenderer.Label(0, i) = IntervalList(i + 1).Name
    '            End If
    '            pFSymbol.Color = pColorRamp.Color(i)
    '            pUVRenderer.Symbol(0, i) = pFSymbol
    '        Next

    '        pRasRenderer.Update()
    '        'Update the renderer with new settings and plug into layer
    '        pRLayer.Renderer = pRasRenderer

    '        'set layer transparency
    '        pLayerEffects = pRLayer
    '        If pLayerEffects.SupportsTransparency Then
    '            pLayerEffects.Transparency = Transparency
    '        End If

    '        'check if a layer with the assigned name exists
    '        'search layer of the specified name, if found
    '        Dim nlayers As Integer = pMap.LayerCount
    '        For i = nlayers To 1 Step -1
    '            pTempLayer = pMap.Layer(i - 1)
    '            If DisplayName = pTempLayer.Name Then 'remove the layer
    '                pMap.DeleteLayer(pTempLayer)
    '            End If
    '        Next

    '        pMxDoc.AddLayer(pRLayer)
    '        pMxDoc.UpdateContents()
    '        'refresh the active view
    '        pMxDoc.ActivatedView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
    '        returnValue = 1
    '    Catch ex As Exception
    '        MessageBox.Show("Exception: " + ex.Message)
    '        returnValue = -1
    '    Finally
    '        pTempLayer = Nothing
    '        pMxDoc = Nothing
    '        pMap = Nothing
    '        pRLayer = Nothing

    '        'pLayerEffects = Nothing
    '        'pFSymbol = Nothing
    '        'pRasRenderer = Nothing
    '        'pUVRenderer = Nothing
    '        'pStyleItem = Nothing
    '        'pEnumStyleGallery = Nothing
    '        'pStyleGallery = Nothing
    '        'pColorRamp = Nothing
    '        'pTable = Nothing
    '        'pBand = Nothing
    '        'pBandCol = Nothing
    '        'pRaster = Nothing
    '        'pRasterDS = Nothing

    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pLayerEffects)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFSymbol)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasRenderer)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pUVRenderer)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStyleItem)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pEnumStyleGallery)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStyleGallery)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pColorRamp)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTable)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pBand)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pBandCol)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRaster)
    '        ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterDS)
    '    End Try
    '    Return returnValue
    'End Function

    'Adds the specified raster layer to ArcMap
    'DisplayName if specified, is used as the layer name in the TOC in ArcMap
    'Action code: 0, default add and replace if layer of the same name already added, and zoom to layer
    'Return values: -1 unknown error occurred
    '               -2 filename error
    '               -3 not a raster dataset
    '               , otherwise, the same value as the Action code
    'add a raster file to the active view
    Public Function BA_DisplayRasterWithSymbol(ByVal pMxDoc As IMxDocument, ByRef LayerPathName As String, _
                                               ByRef DisplayName As String, ByRef DisplayStyle As MapsDisplayStyle, _
                                               ByRef Transparency As Short, ByRef workspaceType As WorkspaceType) As Short

        Dim pRaster As IRaster = Nothing
        Dim pRasterDS As IRasterDataset = Nothing
        Dim pTable As ITable = Nothing
        Dim pBand As IRasterBand = Nothing
        Dim pBandCol As IRasterBandCollection = Nothing
        Dim pColorRamp As IColorRamp = New PresetColorRamp
        Dim pUVRen As IRasterUniqueValueRenderer = New RasterUniqueValueRenderer
        Dim pRasRen As IRasterRenderer = pUVRen
        Dim pFSymbol As ISimpleFillSymbol = New SimpleFillSymbol
        Dim File_Path As String = ""
        Dim File_Name As String = ""
        Dim pRLayer As IRasterLayer = New RasterLayer
        Dim pTempLayer As ILayer = Nothing
        Dim pLayerEffects As ILayerEffects = Nothing
        Dim pStyleGallery As IStyleGallery = Nothing
        Dim pEnumStyleGallery As IEnumStyleGalleryItem = Nothing
        Dim pStyleItem As IStyleGalleryItem = Nothing

        Try
            File_Name = BA_GetBareName(LayerPathName, File_Path)
            If workspaceType = workspaceType.Raster Then
                pRasterDS = BA_OpenRasterFromFile(File_Path, File_Name)
            ElseIf workspaceType = workspaceType.Geodatabase Then
                pRasterDS = BA_OpenRasterFromGDB(File_Path, File_Name)
            End If
            If pRasterDS IsNot Nothing Then
                ' Get the number of rows from raster table
                pBandCol = CType(pRasterDS, IRasterBandCollection)
                pBand = pBandCol.Item(0)
                Dim TableExist As Boolean
                pBand.HasTable(TableExist)
                If Not TableExist Then Return -2
                pTable = pBand.AttributeTable
                Dim NumOfValues As Integer = pTable.RowCount(Nothing)

                'add raster to current data frame
                pRLayer.CreateFromDataset(pRasterDS)
                pRLayer.Name = DisplayName

                ' Specified a field and get the field index for the specified field to be rendered.
                Dim FieldName As String = BA_FIELD_VALUE   'Value is the default field, you can specify other field here.
                Dim FieldIndex As Integer = pTable.FindField(FieldName)

                pStyleGallery = pMxDoc.StyleGallery
                Dim StyleName As String
                Dim StyleCategory As String
                Select Case DisplayStyle
                    Case MapsDisplayStyle.Aspect '"ASPECT"
                        StyleName = "Aspect"
                        StyleCategory = "Default Ramps"
                    Case MapsDisplayStyle.Elevation '"ELEVATION"
                        StyleName = "Elevation #2"
                        StyleCategory = "Default Ramps"
                    Case MapsDisplayStyle.Precipitation '"PRECIPITATION"
                        StyleName = "Precipitation"
                        StyleCategory = "Default Ramps"
                    Case MapsDisplayStyle.Range_Random '"Range/Random"
                        StyleName = "Basic Random"
                        StyleCategory = "Default Schemes"
                    Case MapsDisplayStyle.Slope '"SLOPE"
                        StyleName = "Slope"
                        StyleCategory = "Spatial Ramps"
                    Case MapsDisplayStyle.Cool_Tones 'Use for SubAOI
                        StyleName = "Cool Tones"
                        StyleCategory = "Default Schemes"
                    Case MapsDisplayStyle.Red_to_Blue_Diverging   'Site scenario - Difference in Representation
                        StyleName = "Red to Blue Diverging, Bright"
                        StyleCategory = "Dichromatic Ramps"
                    Case MapsDisplayStyle.FilledDem '"Filled DEM"
                        StyleName = "Elevation #1"
                        StyleCategory = "Default Ramps"
                    Case Else
                        StyleName = "Black to White"
                        StyleCategory = "Default Ramps"
                End Select

                pEnumStyleGallery = pStyleGallery.Items("Color Ramps", "ESRI.style", StyleCategory)
                pEnumStyleGallery.Reset()

                pStyleItem = pEnumStyleGallery.Next
                Do Until pStyleItem Is Nothing
                    If pStyleItem.Name = StyleName Then
                        pColorRamp = pStyleItem.Item
                        Exit Do
                    End If
                    pStyleItem = pEnumStyleGallery.Next
                Loop

                'assign value to the colorramp
                With pColorRamp
                    .Size = NumOfValues
                    .CreateRamp(True)
                End With

                ' Connect renderer and raster 
                pRaster = pRasterDS.CreateDefaultRaster
                pRasRen.Raster = pRaster
                pRasRen.Update()

                ' Configure UniqueValue renderer
                pUVRen.HeadingCount = 1   ' Use one heading 
                'pUVRen.Heading(0) = "All Data Values"
                pUVRen.ClassCount(0) = NumOfValues
                pUVRen.Field = FieldName

                'read the user-defined attributes from the raster
                Dim IntervalList() As BA_IntervalList = Nothing
                Dim response As BA_ReturnCode = BA_ReadReclassRasterAttributeGDB(IntervalList, File_Path, File_Name)

                Dim Value As Object
                For i = 0 To NumOfValues - 1
                    Value = IntervalList(i + 1).Value
                    pUVRen.AddValue(0, i, Value)
                    If String.Compare(IntervalList(i + 1).Name, BA_UNKNOWN) <> 0 Then
                        pUVRen.Label(0, i) = IntervalList(i + 1).Name
                    Else
                        pUVRen.Label(0, i) = CStr(Value)
                    End If
                    pFSymbol.Color = pColorRamp.Color(i)
                    pUVRen.Symbol(0, i) = pFSymbol
                Next

                ' Update render and refresh layer
                pRasRen.Update()
                pRLayer.Renderer = pUVRen

                'set layer transparency
                pLayerEffects = pRLayer
                If pLayerEffects.SupportsTransparency Then
                    pLayerEffects.Transparency = Transparency
                End If

                'check if a layer with the assigned name exists
                'search layer of the specified name, if found
                Dim pMap As IMap = pMxDoc.FocusMap
                Dim nlayers As Integer = pMap.LayerCount
                For I = nlayers To 1 Step -1
                    pTempLayer = CType(pMap.Layer(I - 1), ILayer)   'Explicit cast
                    If DisplayName = pTempLayer.Name Then 'remove the layer
                        pMap.DeleteLayer(pTempLayer)
                    End If
                Next

                pMxDoc.AddLayer(pRLayer)
                pMxDoc.UpdateContents()
                'refresh the active view
                pMxDoc.ActivatedView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
            Else
                Return -1
            End If
            Return 0
        Catch ex As Exception
            MsgBox("Exception: " & ex.Message)
            Return -1
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRaster)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterDS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTable)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pBand)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pBandCol)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pColorRamp)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pUVRen)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasRen)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFSymbol)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRLayer)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTempLayer)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pLayerEffects)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStyleGallery)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStyleItem)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pEnumStyleGallery)
        End Try
    End Function

    'Adds the specified raster layer to ArcMap
    'DisplayName if specified, is used as the layer name in the TOC in ArcMap
    'add a raster file to the active view
    Public Function BA_DisplayRasterWithSymbolByField(ByVal pMxDoc As IMxDocument, ByVal LayerPathName As String, _
                                                           ByVal DisplayName As String, ByVal DisplayStyle As MapsDisplayStyle, _
                                                           ByVal Transparency As Short, ByVal workspaceType As WorkspaceType, _
                                                           ByVal fieldName As String) As BA_ReturnCode

        Dim pRaster As IRaster = Nothing
        Dim pRasterDS As IRasterDataset = Nothing
        Dim pTable As ITable = Nothing
        Dim pCursor As ICursor = Nothing
        Dim pStats As IDataStatistics = New DataStatistics
        Dim pEnumVar As System.Collections.IEnumerator = Nothing
        Dim pBand As IRasterBand = Nothing
        Dim pBandCol As IRasterBandCollection = Nothing
        Dim pColorRamp As IColorRamp = New PresetColorRamp
        Dim pUVRen As IRasterUniqueValueRenderer = New RasterUniqueValueRenderer
        Dim pRasRen As IRasterRenderer = pUVRen
        Dim pFSymbol As ISimpleFillSymbol = New SimpleFillSymbol
        Dim File_Path As String = ""
        Dim File_Name As String = ""
        Dim pRLayer As IRasterLayer = New RasterLayer
        Dim pTempLayer As ILayer = Nothing
        Dim pLayerEffects As ILayerEffects = Nothing
        Dim pStyleGallery As IStyleGallery = Nothing
        Dim pEnumStyleGallery As IEnumStyleGalleryItem = Nothing
        Dim pStyleItem As IStyleGalleryItem = Nothing

        Try
            File_Name = BA_GetBareName(LayerPathName, File_Path)
            If workspaceType = workspaceType.Raster Then
                pRasterDS = BA_OpenRasterFromFile(File_Path, File_Name)
            ElseIf workspaceType = workspaceType.Geodatabase Then
                pRasterDS = BA_OpenRasterFromGDB(File_Path, File_Name)
            End If
            If pRasterDS IsNot Nothing Then
                ' Get the number of rows from raster table
                pBandCol = CType(pRasterDS, IRasterBandCollection)
                pBand = pBandCol.Item(0)
                Dim TableExist As Boolean
                pBand.HasTable(TableExist)
                If Not TableExist Then Return BA_ReturnCode.ReadError
                pTable = pBand.AttributeTable

                'add raster to current data frame
                pRLayer.CreateFromDataset(pRasterDS)
                pRLayer.Name = DisplayName

                ' Specified a field and get the field index for the specified field to be rendered.
                'Dim FieldName As String = BA_FIELD_VALUE   'Value is the default field, you can specify other field here.
                Dim FieldIndex As Integer = pTable.FindField(fieldName)

                pStyleGallery = pMxDoc.StyleGallery
                Dim StyleName As String
                Dim StyleCategory As String
                Select Case DisplayStyle
                    Case MapsDisplayStyle.Aspect '"ASPECT"
                        StyleName = "Aspect"
                        StyleCategory = "Default Ramps"
                    Case MapsDisplayStyle.Elevation '"ELEVATION"
                        StyleName = "Elevation #2"
                        StyleCategory = "Default Ramps"
                    Case MapsDisplayStyle.Precipitation '"PRECIPITATION"
                        StyleName = "Precipitation"
                        StyleCategory = "Default Ramps"
                    Case MapsDisplayStyle.Range_Random '"Range/Random"
                        StyleName = "Basic Random"
                        StyleCategory = "Default Schemes"
                    Case MapsDisplayStyle.Slope '"SLOPE"
                        StyleName = "Slope"
                        StyleCategory = "Spatial Ramps"
                    Case MapsDisplayStyle.Cool_Tones 'Use for SubAOI
                        StyleName = "Cool Tones"
                        StyleCategory = "Default Schemes"
                    Case MapsDisplayStyle.Red_to_Blue_Diverging   'Site scenario - Difference in Representation
                        StyleName = "Red to Blue Diverging, Bright"
                        StyleCategory = "Dichromatic Ramps"
                    Case Else
                        StyleName = "Black to White"
                        StyleCategory = "Default Ramps"
                End Select

                pEnumStyleGallery = pStyleGallery.Items("Color Ramps", "ESRI.style", StyleCategory)
                pEnumStyleGallery.Reset()

                pStyleItem = pEnumStyleGallery.Next
                Do Until pStyleItem Is Nothing
                    If pStyleItem.Name = StyleName Then
                        pColorRamp = pStyleItem.Item
                        Exit Do
                    End If
                    pStyleItem = pEnumStyleGallery.Next
                Loop

                'Get unique values
                pCursor = pTable.Search(Nothing, False)
                pStats.Field = fieldName
                pStats.Cursor = pCursor
                pEnumVar = pStats.UniqueValues
                Dim NumOfValues As Short = pStats.UniqueValueCount

                'assign value to the colorramp
                With pColorRamp
                    .Size = NumOfValues
                    .CreateRamp(True)
                End With

                ' Connect renderer and raster 
                pRaster = pRasterDS.CreateDefaultRaster
                pRasRen.Raster = pRaster
                pRasRen.Update()

                ' Configure UniqueValue renderer
                pUVRen.HeadingCount = 1   ' Use one heading 
                pUVRen.ClassCount(0) = NumOfValues
                pUVRen.Field = fieldName

                'read the user-defined attributes from the raster

                pEnumVar.Reset()
                Dim strValue As String
                Dim i As Integer
                Do Until pEnumVar.MoveNext = False
                    strValue = pEnumVar.Current.ToString
                    pUVRen.AddValue(0, i, strValue)
                    pUVRen.Label(0, i) = CStr(strValue)
                    pFSymbol.Color = pColorRamp.Color(i)
                    pUVRen.Symbol(0, i) = pFSymbol
                    i += 1
                Loop

                ' Update render and refresh layer
                pRasRen.Update()
                pRLayer.Renderer = pUVRen

                'set layer transparency
                pLayerEffects = pRLayer
                If pLayerEffects.SupportsTransparency Then
                    pLayerEffects.Transparency = Transparency
                End If

                'check if a layer with the assigned name exists
                'search layer of the specified name, if found
                Dim pMap As IMap = pMxDoc.FocusMap
                Dim nlayers As Integer = pMap.LayerCount
                For I = nlayers To 1 Step -1
                    pTempLayer = CType(pMap.Layer(I - 1), ILayer)   'Explicit cast
                    If DisplayName = pTempLayer.Name Then 'remove the layer
                        pMap.DeleteLayer(pTempLayer)
                    End If
                Next

                pMxDoc.AddLayer(pRLayer)
                pMxDoc.UpdateContents()
                'refresh the active view
                pMxDoc.ActivatedView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
            Else
                Return BA_ReturnCode.ReadError
            End If
            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_DisplayRasterWithSymbolByField Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRaster)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterDS)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTable)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pBand)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pBandCol)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pColorRamp)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pUVRen)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasRen)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFSymbol)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRLayer)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTempLayer)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pLayerEffects)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStyleGallery)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStyleItem)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pEnumStyleGallery)
        End Try
    End Function

    'This procedure adds the specified DEM extent layer to ArcMap
    'DisplayName if specified, is used as the layer name in the TOC in ArcMap
    'Action code: 0, default add and replace if layer of the same name already added, and zoom to layer
    'Return values: -1 unknown error occurred
    '                       -2 filename error
    '                       -3 not a raster dataset
    '                       , otherwise, the same value as the Action code
    'add a raster file to the active view
    Public Function BA_MapDisplayRaster(ByVal pMxDoc As IMxDocument, ByVal LayerPathName As String, ByVal DisplayName As String, ByVal Transparency As Integer) As Integer
        Dim File_Path As String = "PleaseReturn"
        Dim File_Name As String
        File_Name = BA_GetBareName(LayerPathName, File_Path)

        'exit if file_name is null
        If Len(File_Name) = 0 Then
            Return -2
        End If

        Dim pRasterDS As IRasterDataset
        Dim pRLayer As IRasterLayer = New RasterLayer
        Dim pLayerEffects As ILayerEffects

        Try
            pRasterDS = BA_OpenRasterFromGDB(File_Path, File_Name)
            If pRasterDS IsNot Nothing Then
                'add raster to current data frame
                pRLayer.CreateFromDataset(pRasterDS)

                'set layer name
                pRLayer.Name = DisplayName
                'set layer transparency
                pLayerEffects = pRLayer
                If pLayerEffects.SupportsTransparency Then
                    pLayerEffects.Transparency = Transparency
                End If

                'add layer
                Dim pMap As IMap
                pMap = pMxDoc.FocusMap

                'check if a layer with the assigned name exists
                'search layer of the specified name, if found
                BA_RemoveLayers(pMxDoc, DisplayName)

                'refresh the active view
                pMxDoc.AddLayer(pRLayer)
                pMxDoc.UpdateContents()
                pMxDoc.ActivatedView.Refresh()
                Return 0
            Else
                Return -1
            End If
        Catch ex As Exception
            Debug.Print("BA_MapDisplayRaster Exception: " & ex.Message)
            Return -1
        Finally
            pRasterDS = Nothing
            pRLayer = Nothing
            pLayerEffects = Nothing
        End Try
    End Function

    'Displays the point layer with symbology in the current document
    'Marker Types:
    '    Case 1: MarkerName = "Circle 8" 'circle with crosshair, used for pour points
    '    Case 2: MarkerName = "Bolt" 'thunderbolt, used for snotel sites
    '    Case 3: MarkerName = "Check 1" 'check, used for snow courses
    '    Case 4: MarkerName = "Circle 6" 'circle with a dot, NWS Coop Sites
    '    Case Else: MarkerName = "Pentagon 6" 'pentagon with a dot, Agrimet stations
    Public Function BA_MapDisplayPointMarkers(ByVal application As ESRI.ArcGIS.Framework.IApplication, ByVal filenamepath As String, ByVal layername As MapsLayerName, ByRef MarkerColor As IRgbColor, ByRef MarkerType As MapsMarkerType) As Short
        Dim pMarkerColor As IColor
        Dim FileName As String
        Dim filepath As String = ""
        FileName = BA_GetBareName(filenamepath, filepath)

        'exit if file_name is null
        If String.IsNullOrEmpty(FileName) Then
            Return 1
        End If

        Try
            'load gauge station layer if it doesn't already exist
            Dim pMxDoc As IMxDocument = application.Document
            Dim pMap As IMap = pMxDoc.FocusMap

            'if a layer with the assigned name exists, then remove the layer and add the new point layer
            'search layer of the specified name, if found
            Dim i As Integer
            Dim pTempLayer As ILayer
            Dim nlayers As Integer = pMap.LayerCount
            Dim strLayerName As String = BA_EnumDescription(layername)

            For i = nlayers To 1 Step -1
                pTempLayer = pMap.Layer(i - 1)
                If strLayerName = pTempLayer.Name Then
                    pMap.DeleteLayer(pTempLayer)
                End If
            Next

            'open the point layer
            Dim pFeatClass As IFeatureClass = Nothing
            Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(filepath)
            If workspaceType = workspaceType.Raster Then
                pFeatClass = BA_OpenFeatureClassFromFile(filepath, FileName)
            ElseIf workspaceType = workspaceType.Geodatabase Then
                pFeatClass = BA_OpenFeatureClassFromGDB(filepath, FileName)
            End If

            'add featureclass to current data frame
            ' Note: IFeatureLayer must be created through factory or ArcMap will crash
            ' Original code: Dim pFLayer As IFeatureLayer = New FeatureLayer
            Dim pObjFact As ESRI.ArcGIS.Framework.IObjectFactory = CType(application, ESRI.ArcGIS.Framework.IObjectFactory)
            Dim pFLayer As IFeatureLayer = CType(pObjFact.Create("esriCore.FeatureLayer"), IFeatureLayer)
            pFLayer.FeatureClass = pFeatClass
            pFLayer.Name = strLayerName

            'check feature geometry type, only point layers can be used
            If pFLayer.FeatureClass.ShapeType <> ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint Then
                Return 2
            End If

            'add layer
            'set layer symbology - hollow with red outline
            'Set marker symbol
            Dim pStyleGallery As IStyleGallery = pMxDoc.StyleGallery
            Dim pEnumMarkers As IEnumStyleGalleryItem = pStyleGallery.Items("Marker Symbols", "ESRI.style", "default")
            pEnumMarkers.Reset()
            Dim pStyleItem As IStyleGalleryItem = pEnumMarkers.Next
            pMarkerColor = New RgbColor
            pMarkerColor.RGB = MarkerColor.RGB
            Dim pMSymbol As IMarkerSymbol = Nothing
            Dim pMask As IMask

            Dim MarkerName As String
            'set marker type and color
            Select Case MarkerType
                Case MapsMarkerType.Pourpoint
                    'circle with crosshair, used for pour points
                    MarkerName = "Circle 8"
                Case MapsMarkerType.Snotel
                    'thunderbolt, used for snotel sites
                    MarkerName = "Bolt"
                Case MapsMarkerType.SnowCourse
                    'check, used for snow courses
                    MarkerName = "Asterisk 4"
                Case MapsMarkerType.NwsCoop
                    'circle with a dot, NWS Coop Sites
                    MarkerName = "Circle 6"
                Case MapsMarkerType.PseudoSite
                    'pushpin, Site Scenario tool pseudo sie
                    MarkerName = "Pushpin 3"
                Case Else
                    'pentagon with a dot, Agrimet stations
                    MarkerName = "Pentagon 6"
            End Select

            Do Until pStyleItem Is Nothing
                If pStyleItem.Name = MarkerName Then
                    pMSymbol = pStyleItem.Item
                    pMSymbol.Size = 18
                    pMSymbol.Color = pMarkerColor
                    Exit Do
                End If
                pStyleItem = pEnumMarkers.Next
            Loop

            'set halo of the marker
            pMask = pMSymbol
            pMask.MaskStyle = ESRI.ArcGIS.Display.esriMaskStyle.esriMSHalo
            pMask.MaskSize = 1

            Dim pGFLayer As IGeoFeatureLayer
            Dim pRenderer As ISimpleRenderer = New SimpleRenderer
            pRenderer.Symbol = pMSymbol

            pGFLayer = pFLayer
            pGFLayer.Renderer = pRenderer
            pMap.AddLayer(pFLayer)

            'refresh the active view
            pMxDoc.ActivatedView.PartialRefresh(esriViewDrawPhase.esriViewForeground, Nothing, Nothing)
            pMxDoc.UpdateContents()
            Return 0
        Catch ex As Exception
            MessageBox.Show("BA_MapDisplayPointMarkers Exception: " + ex.Message)
            Return 1
        End Try

    End Function

    'return value:
    ' -1: error occurred
    ' otherwise, the value indicate the number of records updated
    Public Function BA_ReclassRasterAttributes(ByRef Interval_List() As BA_IntervalList, ByRef filepath As String, ByRef FileName As String) As Short
        'fields to be added
        'NAME - esriFieldTypeString: for labeling purpose
        'LBOUND - esriFieldTypeDouble: for upper bound
        'UBOUND - esriFieldTypeDouble: for lower bound
        Dim return_value As Short = 0
        Dim cell_size As Double

        'get cell size
        Dim raster_res As Double = BA_CellSize(filepath, FileName)

        If raster_res <= 0 Then raster_res = 1
        cell_size = raster_res * raster_res

        'open raster attribute table
        Dim pRDataset As IGeoDataset = BA_OpenRasterFromGDB(filepath, FileName)
        Dim pBandCol As IRasterBandCollection = CType(pRDataset, IRasterBandCollection)
        Dim pRasterBand As IRasterBand = pBandCol.Item(0)
        Dim pTable As ITable = pRasterBand.AttributeTable
        Dim pCursor As ICursor = Nothing

        Try
            'add fields
            Dim nclass As Short = UBound(Interval_List)

            'add Name field
            'check if field exist
            Dim FieldIndex As Short
            Dim pField As IField = New Field
            Dim pFld As IFieldEdit2 = CType(pField, IFieldEdit2)

            Dim FName(3) As String

            FName(1) = BA_FIELD_NAME
            FName(2) = BA_FIELD_LBOUND
            FName(3) = BA_FIELD_UBOUND
            Dim i As Short

            For i = 1 To 3
                FieldIndex = pTable.FindField(FName(i))

                ' Define field type
                If FieldIndex < 0 Then 'add field
                    'Define field name
                    pFld.Name_2 = FName(i)
                    Select Case i
                        Case 1 'Name field
                            pFld.Type_2 = esriFieldType.esriFieldTypeString 'If the field is too short, a table update will crash ArcMap!!!!
                            pFld.Length_2 = BA_NAME_FIELD_WIDTH
                            pFld.Required_2 = False
                        Case 2, 3 'Ubound and LBound field
                            pFld.Type_2 = esriFieldType.esriFieldTypeDouble
                            pFld.Length_2 = BA_BOUND_FIELD_WIDTH
                            pFld.Required_2 = False
                    End Select
                    ' Add field
                    pTable.AddField(pFld)
                End If
            Next

            Dim FI3, FI1, FI0, FI2, FI4 As Short
            Dim classvalue As Integer
            Dim pRow As IRow

            ' Get field index again
            FI0 = pTable.FindField(BA_FIELD_VALUE)
            FI1 = pTable.FindField(FName(1))
            FI2 = pTable.FindField(FName(2))
            FI3 = pTable.FindField(FName(3))
            FI4 = pTable.FindField(BA_FIELD_COUNT)

            pCursor = pTable.Update(Nothing, True)
            pCursor.Flush()
            pRow = pCursor.NextRow

            Do Until pRow Is Nothing
                classvalue = pRow.Value(FI0)
                return_value = return_value + 1
                pRow.Value(FI1) = Interval_List(return_value).Name
                pRow.Value(FI2) = Val(Interval_List(return_value).LowerBound)
                pRow.Value(FI3) = Val(Interval_List(return_value).UpperBound)
                Interval_List(return_value).Area = pRow.Value(FI4) * cell_size
                pCursor.UpdateRow(pRow)
                pRow = pCursor.NextRow
            Loop
            Return return_value
        Catch ex As Exception
            MessageBox.Show("BA_ReclassRasterAttributes Exception: " + ex.Message)
            Return -1
        Finally
            pCursor = Nothing
            pTable = Nothing
            pBandCol = Nothing
            pRasterBand = Nothing
            pRDataset = Nothing
        End Try

    End Function

    'remove all layers with the specified name from the data frame
    Public Function BA_RemoveLayers(ByVal pmxDoc As IMxDocument, ByVal LayerName As String) As Short
        Dim pMap As IMap = pmxDoc.FocusMap

        Dim nlayers As Integer = pMap.LayerCount
        Dim pTempLayer As ILayer
        Dim n As Integer = 0
        Dim i As Integer

        For i = nlayers To 1 Step -1
            pTempLayer = pMap.Layer(i - 1)
            If LayerName = pTempLayer.Name Then 'remove the layer
                If TypeOf pTempLayer Is IRasterLayer Then 'disconnect a rasterlayer before removing it
                    Dim pDLayer As IDataLayer2 = CType(pTempLayer, IDataLayer2)
                    pDLayer.Disconnect()
                End If
                pMap.DeleteLayer(pTempLayer)
                n = n + 1
            End If
        Next
        pTempLayer = Nothing

        pmxDoc.ActivatedView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
        pmxDoc.UpdateContents()
        Return n
    End Function

    ' Remove 
    Public Function BA_RemoveLayerByFileNamePrefix(ByVal pmxDoc As IMxDocument, ByVal strHruName As String, ByVal fileNamePrefix As String) As BA_ReturnCode
        'check if a file path is provided
        If String.IsNullOrEmpty(fileNamePrefix) Then
            Return BA_ReturnCode.OtherError
        End If

        Dim pMap As IMap = pmxDoc.FocusMap

        'check if a layer with the desired file path exists
        Dim nlayers As Integer = pMap.LayerCount
        Dim i As Integer
        Dim pTempLayer As ILayer = Nothing
        Dim layerDataPath As String
        Dim pDSet As IDataset = Nothing
        Dim pDLayer As IDataLayer2 = Nothing
        Dim stringpos As Short

        Try
            For i = nlayers To 1 Step -1
                pTempLayer = pMap.Layer(i - 1)
                If TypeOf pTempLayer Is IFeatureLayer Or _
                   TypeOf pTempLayer Is IRasterLayer Then
                    If pTempLayer.Valid Then
                        pDSet = pTempLayer
                        layerDataPath = pDSet.Workspace.PathName
                        stringpos = layerDataPath.IndexOf(strHruName)
                        If stringpos > -1 Then ' the layer is in the target gdb
                            stringpos = pDSet.Name.IndexOf(fileNamePrefix)
                            If stringpos > -1 Then 'found a layer that matches the prefix, remove the layer
                                pDLayer = pTempLayer
                                pDLayer.Disconnect() 'disconnect a rasterlayer before removing it
                                pMap.DeleteLayer(pTempLayer)
                            End If
                        End If
                    Else
                        MessageBox.Show("The " & pTempLayer.Name & " layer is invalid!", "Invalid layer", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                End If
            Next

            'refresh map
            pmxDoc.UpdateContents()
            pmxDoc.ActivatedView.Refresh()
            Return BA_ReturnCode.Success
        Catch ex As Exception
            MessageBox.Show("BA_RemoveLayerByPath Exception: " + ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTempLayer)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDSet)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDLayer)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pMap)
        End Try

    End Function

    ' Toggle view to/from layout view
    Public Sub BA_ToggleView(ByVal pmxDoc As IMxDocument, ByRef To_DataView As Boolean)
        Dim pMap As IMap
        Dim pActiveView As IActiveView = pmxDoc.ActivatedView

        If To_DataView Then
            If TypeOf pActiveView Is IPageLayout Then 'switch to layout view
                pMap = pmxDoc.Maps.Item(0)
                pmxDoc.ActiveView = pMap
            End If
        Else
            If Not TypeOf pActiveView Is IPageLayout Then 'switch to layout view
                pmxDoc.ActiveView = pmxDoc.PageLayout
                pmxDoc.PageLayout.ZoomToWhole()
                pmxDoc.ActiveView.Refresh()
            End If
        End If
    End Sub

    ' Zoom map display to AOI envelope
    ' No unit tests since this interacts with user interface
    Public Sub BA_ZoomToAOI(ByVal pmxDoc As IMxDocument, ByVal aoipathname As String)
        Dim pActiveView As IActiveView
        Dim pEnv As IEnvelope

        If String.IsNullOrEmpty(aoipathname) Then Exit Sub

        Dim pMap As IMap = pmxDoc.Maps.Item(0)

        If pMap.Name = BA_MAPS_DEFAULT_MAP_NAME Then
            pmxDoc.ActiveView = pMap
        Else
            MsgBox("Cannot find the default mapframe: " & BA_MAPS_DEFAULT_MAP_NAME & "!")
            Exit Sub
        End If

        pEnv = BA_GetBasinEnvelope(aoipathname)
        pmxDoc.ActiveView.Extent = pEnv

        pActiveView = pmxDoc.ActivatedView
        If Not TypeOf pActiveView Is IPageLayout Then
            pmxDoc.ActiveView = pmxDoc.PageLayout
            pmxDoc.PageLayout.ZoomToWhole()
        End If
    End Sub

    Public Function BA_GetRasterMapSymbology(ByVal fileName As String) As BA_Map_Symbology
        Dim symbol As BA_Map_Symbology = New BA_Map_Symbology
        If fileName = BA_EnumDescription(MapsFileName.AspectZone) Then
            symbol.DisplayName = BA_EnumDescription(MapsLayerName.AspectZone)
            symbol.DisplayStyle = MapsDisplayStyle.Aspect.ToString
            symbol.Transparency = 30
        ElseIf fileName = BA_EnumDescription(MapsFileName.SlopeZone) Then
            symbol.DisplayName = BA_EnumDescription(MapsLayerName.SlopeZone)
            symbol.DisplayStyle = MapsDisplayStyle.Slope.ToString
            symbol.Transparency = 30
        ElseIf fileName = BA_EnumDescription(MapsFileName.ElevationZone) Then
            symbol.DisplayName = BA_EnumDescription(MapsLayerName.ElevationZone)
            symbol.DisplayStyle = MapsDisplayStyle.Elevation.ToString
            symbol.Transparency = 30
        ElseIf fileName = BA_EnumDescription(MapsFileName.SnotelZone) Then
            symbol.DisplayName = BA_EnumDescription(MapsLayerName.SnotelZone)
            symbol.DisplayStyle = MapsDisplayStyle.Elevation.ToString
            symbol.Transparency = 30
        ElseIf fileName = BA_EnumDescription(MapsFileName.SnowCourseZone) Then
            symbol.DisplayName = BA_EnumDescription(MapsLayerName.SnowCourseZone)
            symbol.DisplayStyle = MapsDisplayStyle.Elevation.ToString
            symbol.Transparency = 30
        ElseIf fileName = BA_EnumDescription(MapsFileName.PrecipZone) Then
            symbol.DisplayName = BA_EnumDescription(MapsLayerName.PrecipZone)
            symbol.DisplayStyle = MapsDisplayStyle.Precipitation.ToString
            symbol.Transparency = 30
        ElseIf fileName = BA_EnumDescription(MapsFileName.ActualRepresentedArea) Then
            symbol.DisplayName = BA_EnumDescription(MapsLayerName.Scenario1Rep)
            symbol.DisplayStyle = MapsDisplayStyle.Elevation.ToString
            symbol.Transparency = 30
        ElseIf fileName = BA_EnumDescription(MapsFileName.PseudoRepresentedArea) Then
            symbol.DisplayName = BA_EnumDescription(MapsLayerName.Scenario2Rep)
            symbol.DisplayStyle = MapsDisplayStyle.Elevation.ToString
            symbol.Transparency = 30
            ' Default if it isn't caught up above
        Else
            symbol.DisplayName = fileName
            symbol.DisplayStyle = MapsDisplayStyle.Unknown
            symbol.Transparency = 30
        End If

        Return symbol
    End Function

    'Returns the symbology for the supplied point dataset (Snotel, Snow Course, etc.)
    Public Function BA_GetPointMapSymbology(ByVal fileName As String) As BA_Map_Symbology
        Dim symbol As BA_Map_Symbology = New BA_Map_Symbology
        Dim pColor As IColor = New RgbColor
        If fileName = BA_EnumDescription(MapsFileName.Snotel) Then
            symbol.DisplayName = MapsLayerName.Snotel
            symbol.MarkerType = MapsMarkerType.Snotel
            pColor.RGB = RGB(0, 0, 255)
        ElseIf fileName = BA_EnumDescription(MapsFileName.SnowCourse) Then
            symbol.DisplayName = MapsLayerName.SnowCourse
            symbol.MarkerType = MapsMarkerType.SnowCourse
            pColor.RGB = RGB(0, 255, 255)
        ElseIf fileName = BA_EnumDescription(MapsFileName.PourPoint) Or fileName = BA_EnumDescription(MapsFileName.UnsnappedPourPoint) Then
            symbol.DisplayName = MapsLayerName.Pourpoint
            symbol.MarkerType = MapsMarkerType.Pourpoint
            pColor.RGB = RGB(0, 255, 255)
        Else
            symbol.MarkerType = MapsMarkerType.Agrimet
            pColor.RGB = RGB(255, 255, 255)
        End If
        symbol.Color = pColor
        Return symbol
    End Function

    'Loops through layers in map. Removes them from map if they are in the supplied workspace path
    Public Function BA_RemoveLayersInWorkspace(ByVal filePath As String, ByVal pmxDoc As IMxDocument) As Integer
        Dim iMaps As IMaps = pmxDoc.Maps
        Dim iMapsCount As Integer = iMaps.Count
        For i = iMapsCount To 1 Step -1
            Dim nextMap As IMap = iMaps.Item(i - 1)
            Dim layerCount As Integer = nextMap.LayerCount
            For j = layerCount To 1 Step -1
                Dim nextLayer As ILayer = nextMap.Layer(j - 1)
                If TypeOf nextLayer Is IFeatureLayer Or _
                   TypeOf nextLayer Is IRasterLayer Then
                    If nextLayer.Valid Then
                        Dim pDSet As IDataset = CType(nextLayer, IDataset)
                        Dim nextPath As String = pDSet.Workspace.PathName
                        Dim pos As Integer = nextPath.IndexOf(filePath)
                        If pos > -1 Then
                            BA_RemoveLayers(pmxDoc, nextLayer.Name)
                        End If
                    Else
                        MessageBox.Show("The " & nextLayer.Name & " layer is invalid!", "Invalid layer", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                End If
            Next
        Next
        Return 0
    End Function

    Public Sub BA_DisplayHruZonesRaster(ByVal pMxDoc As IMxDocument, ByRef LayerPathName As String, _
                                        ByRef DisplayName As String)
        Dim pRaster As IRaster = Nothing
        Dim pGeoDS As IGeoDataset = Nothing
        Dim pRasterDS As IRasterDataset = Nothing
        Dim pTable As ITable = Nothing
        Dim pBand As IRasterBand = Nothing
        Dim pBandCol As IRasterBandCollection = Nothing
        Dim pRamp As IRandomColorRamp = New RandomColorRamp
        Dim pUVRen As IRasterUniqueValueRenderer = New RasterUniqueValueRenderer
        Dim pRasRen As IRasterRenderer = pUVRen
        Dim pFSymbol As ISimpleFillSymbol = New SimpleFillSymbol
        Dim File_Path As String = ""
        Dim File_Name As String = ""
        Dim pRLayer As IRasterLayer = New RasterLayer
        Dim pTempLayer As ILayer = Nothing
        Dim pCursor As ICursor = Nothing
        Dim pRow As IRow = Nothing

        Try
            File_Name = BA_GetBareName(LayerPathName, File_Path)
            Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(LayerPathName)
            If workspaceType = workspaceType.Raster Then
                pGeoDS = BA_OpenRasterFromFile(File_Path, File_Name)
            ElseIf workspaceType = workspaceType.Geodatabase Then
                pGeoDS = BA_OpenRasterFromGDB(File_Path, File_Name)
            End If
            If pGeoDS IsNot Nothing Then
                'Explicit cast
                pRasterDS = CType(pGeoDS, IRasterDataset)
                ' Get the number of rows from raster table
                pBandCol = CType(pRasterDS, IRasterBandCollection)  'Explicit cast
                pBand = pBandCol.Item(0)
                Dim TableExist As Boolean
                pBand.HasTable(TableExist)
                If Not TableExist Then Exit Sub
                pTable = pBand.AttributeTable
                Dim NumOfValues As Integer = pTable.RowCount(Nothing)
                pCursor = pTable.Search(Nothing, False)

                'add featureclass to current data frame
                'Is this where pRasterDS is being cast to IRasterLayer
                pRLayer.CreateFromDataset(pRasterDS)
                pRLayer.Name = DisplayName

                ' Specified a field and get the field index for the specified field to be rendered.
                Dim FieldName As String = BA_FIELD_VALUE   'Value is the default field, you can specify other field here.
                Dim FieldIndex As Integer = pTable.FindField(FieldName)

                ' Create random color
                pRamp.Size = NumOfValues
                pRamp.Seed = 100
                pRamp.CreateRamp(True)

                ' Connect renderer and raster 
                pRaster = pRasterDS.CreateDefaultRaster
                pRasRen.Raster = pRaster
                pRasRen.Update()

                ' Set UniqueValue renderer
                pUVRen.HeadingCount = 1   ' Use one heading 
                'pUVRen.Heading(0) = "All Data Values"
                pUVRen.ClassCount(0) = NumOfValues
                pUVRen.Field = FieldName
                pRow = pCursor.NextRow
                Dim LabelValue As Object
                Dim j As Int16 = 0
                While pRow IsNot Nothing
                    LabelValue = pRow.Value(FieldIndex)  ' Get value of the given index
                    pUVRen.AddValue(0, j, LabelValue)  'Set value for the renderer
                    pUVRen.Label(0, j) = CStr(LabelValue)  'Set label
                    pFSymbol.Color = pRamp.Color(j)
                    pUVRen.Symbol(0, j) = pFSymbol  'Set symbol
                    pRow = pCursor.NextRow  'Move the cursor
                    j += 1  'Increment j
                End While

                ' Update render and refresh layer
                pRasRen.Update()
                pRLayer.Renderer = pUVRen

                'check if a layer with the assigned name exists
                'search layer of the specified name, if found
                ' Getting color ramp from Style Gallery
                Dim pMap As IMap = pMxDoc.FocusMap
                Dim nlayers As Integer = pMap.LayerCount
                For I = nlayers To 1 Step -1
                    pTempLayer = CType(pMap.Layer(I - 1), ILayer) ' Explicit cast  
                    If DisplayName = pTempLayer.Name Then 'remove the layer
                        pMap.DeleteLayer(pTempLayer)
                    End If
                Next

                pMxDoc.AddLayer(pRLayer)
                pMxDoc.UpdateContents()
                'refresh the active view
                pMxDoc.ActivatedView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
            End If
        Catch ex As Exception
            MsgBox("BA_DisplayHruZonesRaster Exception: " & ex.Message)
            'Extended error message for fixing Unable to cast COM object of type
            'System.__ComObject' to interface type 'ESRI.ArcGIS.Carto.IRasterLayer' bug
            Dim sb As StringBuilder = New StringBuilder()
            sb.Append("pGeoDS: " & pGeoDS.ToString & vbCrLf)
            sb.Append("pRasterDS: " & pRasterDS.ToString & vbCrLf)
            sb.Append("pBandCol: " & pBandCol.ToString & vbCrLf)
            sb.Append("pBand: " & pBand.ToString & vbCrLf)
            sb.Append("pTable: " & pTable.ToString & vbCrLf)
            sb.Append("pCursor: " & pCursor.ToString & vbCrLf)
            sb.Append("pRLayer: " & pRLayer.ToString & vbCrLf)
            sb.Append("pRamp.Size: " & pRamp.Size & vbCrLf)
            sb.Append("pRaster: " & pRaster.ToString & vbCrLf)
            'sb.Append("pRasRen.Raster.ToString: " & pRasRen.Raster.ToString & vbCrLf)
            sb.Append("pRasRen.ToString: " & pRasRen.ToString & vbCrLf)
            sb.Append("pUVRen.Field: " & pUVRen.Field & vbCrLf)
            sb.Append("pTempLayer: " & pTempLayer.ToString & vbCrLf)
            MessageBox.Show(sb.ToString, "ArcObjects status")

        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRaster)
            pRaster = Nothing
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGeoDS)
            pGeoDS = Nothing
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterDS)
            pRasterDS = Nothing
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTable)
            pTable = Nothing
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pBand)
            pBand = Nothing
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pBandCol)
            pBandCol = Nothing
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRamp)
            pRamp = Nothing
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pUVRen)
            pUVRen = Nothing
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasRen)
            pRasRen = Nothing
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFSymbol)
            pFSymbol = Nothing
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRLayer)
            pRLayer = Nothing
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTempLayer)
            pTempLayer = Nothing
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pCursor)
            pCursor = Nothing
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRow)
            pRow = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub

    ' Get handle to removeLayer command item
    ' Response to ESRI bug NIM065856 IDataLayer2.Disconnect doesn't release data from layer
    Public Function BA_RemoveLayerCommandItem(ByVal pMxDoc As IMxDocument) As ICommandItem
        Dim pCmdItem As ICommandItem
        Dim pUID1 As ESRI.ArcGIS.esriSystem.UID = New ESRI.ArcGIS.esriSystem.UID
        Dim pDocument As IDocument
        'Remove layer command item
        pUID1.Value = "{18DF94D9-0F8A-11D2-94B1-080009EEBECB}"
        pUID1.SubType = 3
        pDocument = CType(pMxDoc, IDocument)
        pCmdItem = pDocument.CommandBars.Find(pUID1)
        Return pCmdItem
    End Function

    'This procedure adds the specified feature class to ArcMap
    'This subroutine only supports file geodatabase format
    'DisplayName if specified, is used as the layer name in the TOC in ArcMap
    'Action code: 0, default add and replace if layer of the same name already added, and zoom to layer
    '                   1, add and replace only
    '                   2, add
    Public Function BA_AddExtentLayer(ByVal pMxDoc As IMxDocument, ByVal LayerPathName As String, _
                                      ByVal rgbColor As IRgbColor, Optional ByVal DisplayName As String = "", _
                                      Optional ByVal Action As Short = 0, Optional ByVal Buffer_Factor As Double = 2, _
                                      Optional ByVal lineSymbolWidth As Double = 1.0) As BA_ReturnCode
        Dim File_Path As String = "PleaseReturn"
        Dim File_Name As String

        File_Name = BA_GetBareName(LayerPathName, File_Path)

        'exit if file_name is null
        If String.IsNullOrEmpty(File_Name) Then
            Return BA_ReturnCode.ReadError
        End If

        If BA_GetWorkspaceTypeFromPath(LayerPathName) <> WorkspaceType.Geodatabase Then
            Return BA_ReturnCode.NotSupportedOperation
        End If

        Dim pFeatClass As IFeatureClass
        Dim pFLayer As IFeatureLayer = New FeatureLayer
        Dim pMap As IMap
        Dim pPolySym As ISimpleFillSymbol = New SimpleFillSymbol
        Dim pGFLayer As IGeoFeatureLayer
        Dim pRenderer As ISimpleRenderer = New SimpleRenderer
        Dim pLineSym As ILineSymbol = New SimpleLineSymbol
        Dim pEnv As IEnvelope
        Dim pTempLayer As ILayer

        Try
            pFeatClass = BA_OpenFeatureClassFromGDB(File_Path, File_Name)

            'add featureclass to current data frame
            pFLayer.FeatureClass = pFeatClass

            'check feature geometry type, only polygon layers can be used as an extent layer
            If pFLayer.FeatureClass.ShapeType <> esriGeometryType.esriGeometryPolygon Then
                Return BA_ReturnCode.NotSupportedOperation
            End If

            'set layer name
            If String.IsNullOrEmpty(DisplayName) Then
                pFLayer.Name = pFLayer.FeatureClass.AliasName
            Else
                pFLayer.Name = DisplayName
            End If

            'add layer
            pMap = pMxDoc.FocusMap

            'set layer symbology - hollow
            If rgbColor Is Nothing Then
                rgbColor = New RgbColor()
                rgbColor.RGB = RGB(0, 0, 0)
            End If
            pLineSym.Color = rgbColor
            pLineSym.Width = lineSymbolWidth
            pPolySym.Outline = pLineSym
            pPolySym.Style = esriSimpleFillStyle.esriSFSHollow
            pRenderer.Symbol = pPolySym

            pGFLayer = pFLayer
            pGFLayer.Renderer = pRenderer

            'check if a layer with the assigned name exists
            If Action <> 2 Then
                'search layer of the specified name, if found
                Dim nlayers As Long
                Dim i As Long
                nlayers = pMap.LayerCount
                For i = nlayers To 1 Step -1
                    pTempLayer = pMap.Layer(i - 1)
                    If DisplayName = pTempLayer.Name Then 'remove the layer
                        pMap.DeleteLayer(pTempLayer)
                    End If
                Next
            End If

            pMap.AddLayer(pFLayer)

            If Action = 0 Then 'zoom to layer
                'create a buffer around the AOI
                pEnv = pFLayer.AreaOfInterest

                Dim llx As Double, lly As Double, urx As Double, ury As Double
                Dim xrange As Double, yrange As Double
                Dim xoffset As Double, yoffset As Double

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

                'pFLayer.AreaOfInterest.PutCoords llx, lly, urx, ury
                Dim pActiveView As IActiveView
                pActiveView = pMxDoc.ActiveView
                pActiveView.Extent = pEnv
            End If

            'refresh the active view
            pMxDoc.ActivatedView.Refresh()
            pMxDoc.UpdateContents()

            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_AddExtentLayer Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            pFeatClass = Nothing
            pFLayer = Nothing
            pGFLayer = Nothing
            pEnv = Nothing
            pTempLayer = Nothing
            pRenderer = Nothing
            pMap = Nothing
            pPolySym = Nothing
            pLineSym = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try

    End Function

    'Defines a uniqueValueRender for the IGeoFeatureLayer that is passed in; Uses the colorRamp and fieldName
    'that are supplied as arguments
    Public Sub BA_DefineUniqueValueRenderer(ByVal pGeoFeatureLayer As IGeoFeatureLayer, ByVal pColorRamp As IColorRamp, _
                                            ByVal fieldName As String)

        'Make the renderer.
        Dim pUniqueValueRenderer As IUniqueValueRenderer = New UniqueValueRenderer
        Dim pSimpleFillSymbol As ISimpleFillSymbol = New SimpleFillSymbol
        Dim pDisplayTable As IDisplayTable = Nothing
        'Dim pFeatureCursor As IFeatureCursor = Nothing
        Dim pCursor As ICursor = Nothing
        'Dim pFeature As IFeature = Nothing
        Dim pRow As IRow = Nothing
        Dim pFields As IFields = Nothing
        Dim pEnumColors As IEnumColors = Nothing
        Dim pTable As ITable = Nothing

        Try
            'Configure the fill symbol
            pSimpleFillSymbol.Style = esriSimpleFillStyle.esriSFSSolid
            pSimpleFillSymbol.Outline.Width = 0.4

            'These properties should be set prior to adding values.
            pUniqueValueRenderer.FieldCount = 1
            pUniqueValueRenderer.Field(0) = fieldName
            pUniqueValueRenderer.DefaultSymbol = pSimpleFillSymbol
            'Suppress display of "Default Values" heading
            pUniqueValueRenderer.UseDefaultSymbol = False

            pDisplayTable = CType(pGeoFeatureLayer, IDisplayTable) 'Explicit cast
            pTable = CType(pDisplayTable, ITable)   'Explicit Cast
            'Create cursor on the display table
            'pFeatureCursor = pDisplayTable.SearchDisplayTable(Nothing, False)

            Dim pTableSort As ITableSort = New TableSort
            With pTableSort
                .Fields = fieldName
                .Ascending(fieldName) = True
                .Table = pTable
            End With
            pTableSort.Sort(Nothing)
            pCursor = pTableSort.Rows

            'pFeatureCursor = pTableSort.Rows

            pRow = pCursor.NextRow
            Dim hruIndex As Integer = pTable.FindField("Profile_area_params.HRU_ID")
            Dim fieldIndex As Integer = pTable.FindField(fieldName)
            'Do While Not pRow Is Nothing
            '    Debug.Print("{0}, {1}", pRow.Value(hruIndex), pRow.Value(fieldIndex))
            '    pRow = pCursor.NextRow
            'Loop
            'pFeature = pFeatureCursor.NextFeature()

            Dim ValFound As Boolean

            'pFields = pFeatureCursor.Fields
            pFields = pCursor.Fields

            While Not pRow Is Nothing
                Dim pClassSymbol As ISimpleFillSymbol = New SimpleFillSymbol
                pClassSymbol.Style = esriSimpleFillStyle.esriSFSSolid
                pClassSymbol.Outline.Width = 0.4

                Dim classValue As String
                If IsDBNull(pRow.Value(fieldIndex)) Then
                    classValue = "<Null>"
                Else
                    If IsNumeric(pRow.Value(fieldIndex)) Then
                        'Dim dblValue As Double = CType(pFeature.Value(fieldIndex), Double)
                        'classValue = dblValue.ToString("0.######E+00")
                        classValue = pRow.Value(fieldIndex)
                    Else
                        classValue = pRow.Value(fieldIndex)
                    End If

                End If


                'Test to see if this value was added
                'to the renderer. If not, add it.
                ValFound = False
                Dim i As Integer
                For i = 0 To pUniqueValueRenderer.ValueCount - 1 Step i + 1
                    If pUniqueValueRenderer.Value(i) = classValue Then
                        ValFound = True
                        Exit For 'Exit the loop if the value is found As break.
                    End If
                Next
                'If the value was not found, it is new and will be added.
                If ValFound = False Then
                    pUniqueValueRenderer.AddValue(classValue, fieldName, pClassSymbol)
                    pUniqueValueRenderer.Label(classValue) = classValue
                    pUniqueValueRenderer.Symbol(classValue) = pClassSymbol
                End If
                'pFeature = pFeatureCursor.NextFeature()
                pRow = pCursor.NextRow
            End While

            'Since the number of unique values is known,
            'the color ramp can be sized and the colors assigned.

            pColorRamp.Size = pUniqueValueRenderer.ValueCount
            Dim bOK As Boolean
            pColorRamp.CreateRamp(bOK)

            pEnumColors = pColorRamp.Colors
            pEnumColors.Reset()

            Dim j As Integer
            For j = 0 To pUniqueValueRenderer.ValueCount - 1 Step j + 1
                Dim xv As String = pUniqueValueRenderer.Value(j)
                If xv <> "" Then
                    Dim pSimpleFillColor As ISimpleFillSymbol = pUniqueValueRenderer.Symbol(xv)
                    pSimpleFillColor.Color = pEnumColors.Next()
                    pUniqueValueRenderer.Symbol(xv) = pSimpleFillColor
                End If

            Next


            ''** If you didn't use a predefined color ramp
            ''** in a style, use "Custom" here. Otherwise,
            ''** use the name of the color ramp you selected.
            pUniqueValueRenderer.ColorScheme = "Black and White"
            Dim isString As Boolean = pTable.Fields.Field(fieldIndex).Type = esriFieldType.esriFieldTypeString
            pUniqueValueRenderer.FieldType(0) = isString
            pGeoFeatureLayer.Renderer = pUniqueValueRenderer

            'This makes the layer properties symbology tab
            'show the correct interface.
            Dim pUID As ESRI.ArcGIS.esriSystem.IUID = New ESRI.ArcGIS.esriSystem.UID
            pUID.Value = "{683C994E-A17B-11D1-8816-080009EC732A}"
            pGeoFeatureLayer.RendererPropertyPageClassID = pUID

        Catch ex As Exception
            Debug.Print("BA_DefineUniqueValueRenderer Exception: " & ex.Message)
        Finally
            pUniqueValueRenderer = Nothing
            pSimpleFillSymbol = Nothing
            pDisplayTable = Nothing
            'pFeatureCursor = Nothing
            'pFeature = Nothing
            pCursor = Nothing
            pRow = Nothing
            pFields = Nothing
            pEnumColors = Nothing
            pTable = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub

    'Returns a colorRamp from the style gallery
    'Example arguments: "Color Ramps", "ESRI.style", "Default Schemes", "Black and White" returns a Black-And-White color ramp
    Public Function BA_FindColorRamp(ByVal pMxDocument As IMxDocument, ByVal pClassName As String, ByVal pStyleSet As String, _
                                     ByVal pCategory As String, ByVal pName As String) As IColorRamp

        Try
            Dim pColorRamp As IColorRamp = Nothing
            Dim pStyleGallery As IStyleGallery = pMxDocument.StyleGallery
            Dim pEnumMarkers As IEnumStyleGalleryItem = pStyleGallery.Items(pClassName, pStyleSet, pCategory)
            pEnumMarkers.Reset()
            Dim pStyleItem As IStyleGalleryItem = pEnumMarkers.Next
            Do While pStyleItem IsNot Nothing
                If pStyleItem.Name = pName Then
                    pColorRamp = pStyleItem.Item
                    Exit Do
                End If
                pStyleItem = pEnumMarkers.Next
            Loop
            Return pColorRamp
        Catch ex As Exception
            Debug.Print("BA_FindColorRamp Exception: " & ex.Message)
            Return Nothing
        End Try
    End Function

    'Join a feature layer to a table
    'pfLayer: The feature layer that has been added to a map
    'vName: The name of the feature layer on the map
    'pfJoinField: The field in the feature layer to join to
    'paramTable: the table to be joined
    'pTableJoinField: The field in the table to join to
    Public Function BA_JoinFeatureClassToTable(ByVal pfLayer As IFeatureLayer, ByVal vName As String, ByVal pfJoinField As String, _
                                               ByVal paramTable As ITable, ByVal pTableJoinField As String) As BA_ReturnCode
        Dim pFClass As IFeatureClass = Nothing
        Dim memRelClassFactoryType As Type = Nothing
        Dim memRelClassFactory As IMemoryRelationshipClassFactory = Nothing
        Dim relationshipClass As IRelationshipClass = Nothing
        Try
            pFClass = pfLayer.FeatureClass

            ' Build a memory relationship class.
            memRelClassFactoryType = Type.GetTypeFromProgID("esriGeodatabase.MemoryRelationshipClassFactory")
            memRelClassFactory = CType(Activator.CreateInstance(memRelClassFactoryType), IMemoryRelationshipClassFactory)
            relationshipClass = memRelClassFactory.Open(vName, pFClass, pfJoinField, CType(paramTable, IObjectClass), pTableJoinField, "forward", "backward", esriRelCardinality.esriRelCardinalityOneToOne)
            Dim pDispRC As IDisplayRelationshipClass = pfLayer
            pDispRC.DisplayRelationshipClass(relationshipClass, esriJoinType.esriLeftOuterJoin)

            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_JoinFeatureClassToTable Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            pFClass = Nothing
            memRelClassFactory = Nothing
            memRelClassFactoryType = Nothing
            relationshipClass = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    Public Sub BA_AnnotateLayer(ByVal pFLayer As IFeatureLayer, ByVal labelField As String, ByVal labelColor As IRgbColor, _
                                ByVal pFont As stdole.IFontDisp, ByVal displayLabels As Boolean)

        Dim geoLayer As IGeoFeatureLayer = CType(pFLayer, IGeoFeatureLayer)     'Explicit cast
        If geoLayer IsNot Nothing Then
            geoLayer.DisplayAnnotation = displayLabels
            Dim propertiesColl As IAnnotateLayerPropertiesCollection = geoLayer.AnnotationProperties
            Dim labelEngineProperties As IAnnotateLayerProperties = New LabelEngineLayerProperties
            Dim placedElements As IElementCollection = New ElementCollection
            Dim unplacedElements As IElementCollection = New ElementCollection
            propertiesColl.QueryItem(0, labelEngineProperties, placedElements, unplacedElements)
            Dim lpLabelEngine As ILabelEngineLayerProperties = CType(labelEngineProperties, ILabelEngineLayerProperties)
            lpLabelEngine.Expression = "[" & labelField & "]"
            lpLabelEngine.Symbol.Color = labelColor
            If pFont IsNot Nothing Then
                lpLabelEngine.Symbol.Font = pFont
            End If
            'labelEngineProperties.AnnotationMaximumScale = maxScale
            'labelEngineProperties.AnnotationMinimumScale = minScale
            Dim displayString As IDisplayString = CType(pFLayer, IDisplayString)    'Explicit cast
            Dim properties As IDisplayExpressionProperties = displayString.ExpressionProperties
            properties.Expression = "[" & labelField & "]"
            pFLayer.ShowTips = True
        End If
    End Sub

    Public Sub BA_RemoveAnnotation(ByVal pFLayer As IFeatureLayer)
        Dim geoLayer As IGeoFeatureLayer = CType(pFLayer, IGeoFeatureLayer)     'Explicit cast
        If geoLayer IsNot Nothing Then
            geoLayer.DisplayAnnotation = False
        End If
    End Sub

    'This procedure adds a polyline shapefile to ArcMap
    'DisplayName if specified, is used as the layer name in the TOC in ArcMap
    'Action code: 0, default add and replace if layer of the same name already added, and zoom to layer
    '                   1, add and replace only
    '                   2, add
    Public Function BA_AddLineLayer(ByVal pMxDoc As IMxDocument, ByVal LayerPathName As String, ByVal DisplayName As String, ByVal LineColor As IColor, _
                                    Optional ByVal Action As Integer = 0) As BA_ReturnCode
        Dim File_Path As String = "Please Return"
        Dim File_Name As String

        File_Name = BA_GetBareName(LayerPathName, File_Path)

        'exit if file_name is null
        If Len(File_Name) = 0 Then
            Return BA_ReturnCode.ReadError
        End If

        Dim pFeatClass As IFeatureClass = Nothing
        Dim pFLayer As IFeatureLayer = New FeatureLayer
        Dim pGFLayer As IGeoFeatureLayer
        Dim pLineSym As ILineSymbol = New SimpleLineSymbol
        Dim pRenderer As ISimpleRenderer = New SimpleRenderer

        Try
            'check if the input layer is a shapefile
            If BA_GetWorkspaceTypeFromPath(File_Path) = WorkspaceType.Geodatabase Then
                'text exists for the setting of this layer
                pFeatClass = BA_OpenFeatureClassFromGDB(File_Path, File_Name)
            Else 'the input is a shapefile
                pFeatClass = BA_OpenFeatureClassFromFile(File_Path, BA_StandardizeShapefileName(File_Name, False, False))
            End If

            'add featureclass to current data frame
            pFLayer.FeatureClass = pFeatClass
            pFLayer.Name = DisplayName

            'check feature geometry type, only polyline layers can be used as an extent layer
            If pFLayer.FeatureClass.ShapeType <> esriGeometryType.esriGeometryPolyline Then
                Return BA_ReturnCode.NotSupportedOperation
            End If

            'add layer
            Dim pMap As IMap = pMxDoc.FocusMap

            'set layer symbology - hollow with red outline
            pLineSym.Color = LineColor
            pRenderer.Symbol = pLineSym

            pGFLayer = CType(pFLayer, IGeoFeatureLayer) 'Explicit cast
            pGFLayer.Renderer = pRenderer

            'check if a layer with the assigned name exists
            If Action <> 2 Then
                'search layer of the specified name, if found
                Dim nlayers As Long
                Dim i As Long
                Dim pTempLayer As ILayer
                nlayers = pMap.LayerCount
                For i = nlayers To 1 Step -1
                    pTempLayer = pMap.Layer(i - 1)
                    If DisplayName = pTempLayer.Name Then 'remove the layer
                        pMap.DeleteLayer(pTempLayer)
                    End If
                Next
            End If

            pMap.AddLayer(pFLayer)
            Dim Buffer_Factor As Double = 2

            If Action = 0 Then 'zoom to layer
                'create a buffer around the AOI
                Dim pEnv As IEnvelope
                pEnv = pFLayer.AreaOfInterest

                Dim llx As Double, lly As Double, urx As Double, ury As Double
                Dim xrange As Double, yrange As Double
                Dim xoffset As Double, yoffset As Double

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

                'pFLayer.AreaOfInterest.PutCoords llx, lly, urx, ury
                Dim pActiveView As IActiveView
                pActiveView = pMxDoc.ActiveView
                pActiveView.Extent = pEnv
            End If

            'refresh the active view
            pMxDoc.ActiveView.PartialRefresh(2, Nothing, Nothing) 'esriViewGeography
            pMxDoc.UpdateContents()

            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_AddLineLayer Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            pFeatClass = Nothing
            pFLayer = Nothing
            pGFLayer = Nothing
            pLineSym = Nothing
            pRenderer = Nothing
        End Try
    End Function

    'return value:
    '   -1: error occurred
    ' otherwise, the value indicate the number of records updated
    Public Function BA_UpdateReclassRasterAttributes(IntervalList() As BA_IntervalList, filepath As String, FileName As String) As Integer
        'fields to be added
        'NAME - esriFieldTypeString: for labeling purpose
        'LBOUND - esriFieldTypeDouble: for upper bound
        'UBOUND - esriFieldTypeDouble: for lower bound
        Dim return_value As Integer
        return_value = 0

        'open raster attribute table
        Dim pRDataset As IRasterDataset
        Dim pTable As ITable
        Dim pRasterBand As IRasterBand
        Dim pBandCol As IRasterBandCollection
        Dim pFld As IFieldEdit
        Dim pCursor As ICursor
        Dim pRow As IRow
        Dim pQFilter As IQueryFilter = New QueryFilter


        'add fields
        Try
            pRDataset = BA_OpenRasterFromGDB(filepath, FileName)
            pBandCol = pRDataset
            pRasterBand = pBandCol.Item(0)
            pTable = pRasterBand.AttributeTable

            Dim nclass As Integer = UBound(IntervalList)

            'add Name field
            ' check if field exist
            Dim FieldIndex As Integer
            Dim FName(0 To 2) As String
            Dim i As Integer

            FName(0) = BA_FIELD_NAME
            FName(1) = BA_FIELD_LBOUND
            FName(2) = BA_FIELD_UBOUND

            For i = 0 To 2

                FieldIndex = pTable.FindField(FName(i))

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
                    pTable.AddField(pFld)
                End If
            Next

            'update value
            ' Get field index again
            Dim FI1 As Integer, FI2 As Integer, FI3 As Integer

            FI1 = pTable.FindField(FName(0))
            FI2 = pTable.FindField(FName(1))
            FI3 = pTable.FindField(FName(2))


            Dim tempname As String
            For i = 1 To nclass
                pQFilter.WhereClause = BA_FIELD_VALUE & " = " & CLng(IntervalList(i).Value)
                pCursor = pTable.Update(pQFilter, False)
                pRow = pCursor.NextRow

                Do While Not pRow Is Nothing
                    tempname = Trim(IntervalList(i).Name)
                    If Len(tempname) >= BA_NAME_FIELD_WIDTH Then 'truncate the string if it's longer than the att field width
                        tempname = Left(tempname, BA_NAME_FIELD_WIDTH - 1)
                    End If
                    pRow.Value(FI1) = tempname
                    pRow.Value(FI2) = IntervalList(i).LowerBound
                    pRow.Value(FI3) = IntervalList(i).UpperBound
                    pCursor.UpdateRow(pRow)
                    pRow = pCursor.NextRow
                    return_value = return_value + 1
                Loop
            Next
            Return return_value
        Catch ex As Exception
            Debug.Print("BA_UpdateReclassRasterAttributes Exception: " & ex.Message)
            Return -1
        Finally
            pTable = Nothing
            pRasterBand = Nothing
            pBandCol = Nothing
            pRDataset = Nothing
            pQFilter = Nothing
            pRow = Nothing
            pCursor = Nothing

        End Try
    End Function

    Public Function BA_RemoveLayersfromMapFrame(ByVal pMxDoc As IMxDocument) As Integer
        Dim LayerNames(0 To 11) As String
        LayerNames(1) = BA_MAPS_AOI_BOUNDARY
        LayerNames(2) = BA_MAPS_ELEVATION_ZONES
        LayerNames(3) = BA_MAPS_SNOTEL_ZONES
        LayerNames(4) = BA_MAPS_SNOTEL_SITES
        LayerNames(5) = BA_MAPS_SNOW_COURSE_ZONES
        LayerNames(6) = BA_MAPS_SNOW_COURSE_SITES
        LayerNames(7) = BA_MAPS_PRECIPITATION_ZONES
        LayerNames(8) = BA_MAPS_HILLSHADE
        LayerNames(9) = BA_MAPS_STREAMS
        LayerNames(10) = BA_MAPS_ASPECT
        LayerNames(11) = BA_MAPS_SLOPE

        For j = 1 To 11
            BA_RemoveLayers(pMxDoc, LayerNames(j))
        Next

        'Remove any representation layers
        Dim pMap As IMap = pMxDoc.FocusMap
        Dim pTempLayer As ILayer
        Dim nlayers As Integer = pMap.LayerCount
        For i = nlayers To 1 Step -1
            pTempLayer = pMap.Layer(i - 1)
            Dim suffix As String = Right(pTempLayer.Name, 4)
            If suffix = "_Rep" Then 'remove the layer
                If TypeOf pTempLayer Is IRasterLayer Then 'disconnect a rasterlayer before removing it
                    Dim pDLayer As IDataLayer2 = CType(pTempLayer, IDataLayer2)
                    pDLayer.Disconnect()
                End If
                pMap.DeleteLayer(pTempLayer)
            End If
        Next

        pMxDoc.ActivatedView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
        pMxDoc.UpdateContents()
        Return 1
    End Function

    'add/remove layers to the map frame specified by name
    Public Function BA_AddLayerstoMapFrame(ByVal pApplication As ESRI.ArcGIS.Framework.IApplication, ByVal pMxDoc As IMxDocument, ByVal aoiPath As String, _
                                           ByVal aoiHasSnotel As Boolean, aoiHasSnowCourse As Boolean, ByRef actualRepMap_Flag As Boolean, ByRef pseudoRepMap_Flag As Boolean) As Integer
        'the following layers are added
        'Public Const BA_MAPS_AOI_BOUNDARY = "AOI Boundary"
        'Public Const BA_MAPS_ELEVATION_ZONES = "Elevation Zones"
        'Public Const BA_MAPS_SNOTEL_ZONES = "SNOTEL Elevation Zones"
        'Public Const BA_MAPS_SNOTEL_SITES = "SNOTEL Sites"
        'Public Const BA_MAPS_SNOW_COURSE_ZONES = "Snow Course Elevation Zones"
        'Public Const BA_MAPS_SNOW_COURSE_SITES = "Snow Courses"
        'Public Const BA_MAPS_PRECIPITATION_ZONES = "Precipitation Zones"
        'Public Const BA_MAPS_HILLSHADE = "Hillshade"
        'Public Const BA_MAPS_STREAMS = "AOI Streams"
        'Public Const BA_MAPS_ASPECT = "Aspect"
        'Public Const BA_MAPS_SLOPE = "Slope"
        BA_RemoveLayersfromMapFrame(pMxDoc)

        Dim filepath As String
        Dim FileName As String
        Dim filepathname As String
        Dim response As Integer
        Dim pColor As IColor = New RgbColor

        'add vector layers first
        'add aoi boundary and zoom to AOI
        filepath = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Aoi, True)
        FileName = BA_EnumDescription(AOIClipFile.AOIExtentCoverage)
        filepathname = filepath & FileName
        response = BA_AddExtentLayer(pMxDoc, filepathname, Nothing, BA_MAPS_AOI_BOUNDARY, 0, 1.2, 2.0)

        'add aoi streams layer
        filepath = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Layers, True)
        FileName = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.AoiStreamsVector), False)
        filepathname = filepath & FileName
        pColor.RGB = RGB(0, 0, 255)
        response = BA_AddLineLayer(pMxDoc, filepathname, BA_MAPS_STREAMS, pColor, 0)

        'add snotel and snow course layers
        'add SNOTEL site layer
        If aoiHasSnotel Then
            FileName = BA_EnumDescription(MapsFileName.Snotel)
            filepathname = filepath & FileName
            pColor.RGB = RGB(0, 0, 255)
            response = BA_MapDisplayPointMarkers(pApplication, filepathname, MapsLayerName.Snotel, pColor, MapsMarkerType.Snotel)
        End If

        If aoiHasSnowCourse Then
            FileName = BA_EnumDescription(MapsFileName.SnowCourse)
            filepathname = filepath & FileName
            pColor.RGB = RGB(0, 255, 255) 'cyan
            response = BA_MapDisplayPointMarkers(pApplication, filepathname, MapsLayerName.SnowCourse, pColor, MapsMarkerType.SnowCourse)
        End If

        'add hillshade
        filepath = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Surfaces, True)
        FileName = BA_GetBareName(BA_EnumDescription(PublicPath.Hillshade))
        filepathname = filepath & FileName
        response = BA_MapDisplayRaster(pMxDoc, filepathname, BA_MAPS_HILLSHADE, 0)

        'add aspect zones
        filepath = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Analysis, True)
        FileName = BA_EnumDescription(MapsFileName.AspectZone)
        filepathname = filepath & FileName
        response = BA_DisplayRasterWithSymbol(pMxDoc, filepathname, BA_MAPS_ASPECT, MapsDisplayStyle.Aspect, 30, WorkspaceType.Geodatabase)

        'add slope zones
        FileName = BA_EnumDescription(MapsFileName.SlopeZone)
        filepathname = filepath & FileName
        response = BA_DisplayRasterWithSymbol(pMxDoc, filepathname, BA_MAPS_SLOPE, MapsDisplayStyle.Slope, 30, WorkspaceType.Geodatabase)

        'add Elevation Zones
        FileName = BA_EnumDescription(MapsFileName.ElevationZone)
        filepathname = filepath & FileName
        response = BA_DisplayRasterWithSymbol(pMxDoc, filepathname, BA_MAPS_ELEVATION_ZONES, MapsDisplayStyle.Elevation, 30, WorkspaceType.Geodatabase)

        'add snowtel zones
        If aoiHasSnotel Then
            FileName = BA_EnumDescription(MapsFileName.SnotelZone)
            filepathname = filepath & FileName
            response = BA_DisplayRasterWithSymbol(pMxDoc, filepathname, BA_MAPS_SNOTEL_ZONES, MapsDisplayStyle.Elevation, 30, WorkspaceType.Geodatabase)
        End If

        'add snow course zones
        If aoiHasSnowCourse Then
            FileName = BA_EnumDescription(MapsFileName.SnowCourseZone)
            filepathname = filepath & FileName
            response = BA_DisplayRasterWithSymbol(pMxDoc, filepathname, BA_MAPS_SNOW_COURSE_ZONES, MapsDisplayStyle.Elevation, 30, WorkspaceType.Geodatabase)
        End If

        'add precipitation zones
        FileName = BA_EnumDescription(MapsFileName.PrecipZone)
        filepathname = filepath & FileName
        response = BA_DisplayRasterWithSymbol(pMxDoc, filepathname, BA_MAPS_PRECIPITATION_ZONES, MapsDisplayStyle.Precipitation, 30, WorkspaceType.Geodatabase)

        'add elevation representation scenario outputs if they exist
        FileName = BA_EnumDescription(MapsFileName.ActualRepresentedArea)
        filepathname = filepath & FileName
        response = BA_DisplayRasterWithSymbol(pMxDoc, filepathname, BA_MAPS_SCENARIO1_REPRESENTATION, MapsDisplayStyle.Elevation, 30, WorkspaceType.Geodatabase)
        If response < 0 Then
            actualRepMap_Flag = False
        Else
            actualRepMap_Flag = True
        End If

        FileName = BA_EnumDescription(MapsFileName.PseudoRepresentedArea)
        filepathname = filepath & FileName
        response = BA_DisplayRasterWithSymbol(pMxDoc, filepathname, BA_MAPS_SCENARIO2_REPRESENTATION, MapsDisplayStyle.Elevation, 30, WorkspaceType.Geodatabase)
        If response < 0 Then
            pseudoRepMap_Flag = False
        Else
            pseudoRepMap_Flag = True
        End If

        pColor = Nothing
        'zoom to the aoi boundary layer
        BA_ZoomToAOI(pMxDoc, aoiPath)
        'response may be -1 even if processing completed successfully. Used to determine if representative area layers are present
        Return 1
    End Function

    Public Sub BA_AddMapElements(ByVal pMxDoc As IMxDocument, ByVal TitleText As String, ByVal subtitletext As String)
        'map element names
        'title: "Title"
        'subtitle: "SubTitle"
        'textbox1: "TextBox1"
        'legend: "Legend"
        'northarrow: "North Arrow"
        'scalebar: "Scale"

        Dim pPageLayout As IPageLayout
        Dim pGraphicsContainer As IGraphicsContainer
        Dim pActiveView As IActiveView
        Dim pMElem As IElement
        Dim pElemProp As IElementProperties2
        Dim pMapFrame As IMapFrame
        Dim pElement As IElement
        Dim pStyleGallery As IStyleGallery = pMxDoc.StyleGallery

        Try
            Dim Map_Title As String = "Title"
            Dim Map_SubTitle As String = "SubTitle"
            Dim Map_TextBox1 As String = "TextBox1"
            Dim Map_NorthArrow As String = "North Arrow"
            Dim Map_Scale As String = "Scale"

            'Set the page layout.
            pPageLayout = pMxDoc.PageLayout
            pGraphicsContainer = pPageLayout
            pActiveView = pPageLayout

            'check if map elements were added
            Dim i As Integer = 0
            pGraphicsContainer.Reset()
            pMElem = pGraphicsContainer.Next
            Do While Not pMElem Is Nothing
                i = i + 1
                pMElem = pGraphicsContainer.Next
            Loop
            If i > 1 Then Exit Sub

            pMapFrame = pGraphicsContainer.FindFrame(pMxDoc.FocusMap)
            pElement = pMapFrame

            '==========================================
            'Title
            '==========================================
            ' Part 2: Add the title.
            Dim pTextElement As ITextElement
            Dim pTextSymbol As IFormattedTextSymbol
            Dim pTextFont As stdole.IFontDisp
            Dim pPntElement As IElement
            Dim pPoint As IPoint
            ' Define the text font.
            pTextFont = New stdole.StdFont
            With pTextFont
                .Name = "Times New Roman"
                .Size = 24
                .Bold = True
            End With

            ' Define the text symbol.
            pTextSymbol = New TextSymbol
            With pTextSymbol
                .Font = pTextFont
                .Case = esriTextCase.esriTCAllCaps
                .HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter
            End With

            ' Define the title as a text element.
            pTextElement = New TextElement
            pTextElement.Text = TitleText
            pTextElement.Symbol = pTextSymbol

            ' Define the position to plot the title.
            pPoint = New Point
            pPoint.X = 4.0#
            pPoint.Y = 10.5
            pPntElement = pTextElement
            pElemProp = pTextElement
            pElemProp.Name = Map_Title
            pPntElement.Geometry = pPoint
            ' Add the title to the graphics container.
            pGraphicsContainer.AddElement(pTextElement, 0)

            ' Part 3: Add the subtitle.
            ' Define the text font.
            pTextFont = New stdole.StdFont
            With pTextFont
                .Name = "Times New Roman"
                .Size = 14
            End With
            ' Define the text symbol.
            pTextSymbol = New TextSymbol
            With pTextSymbol
                .Font = pTextFont
                .Case = esriTextCase.esriTCAllCaps
                .HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter
            End With

            ' Define the subtitle as a text element.
            pTextElement = New TextElement
            pTextElement.Text = subtitletext
            pTextElement.Symbol = pTextSymbol
            pElemProp = pTextElement
            pElemProp.Name = Map_SubTitle

            ' Define the position to plot the subtitle.
            pPntElement = pTextElement
            pPoint = New Point
            pPoint.X = 4.0#
            pPoint.Y = 10.1
            pPntElement.Geometry = pPoint
            ' Add the subtitle to the graphic container.
            pGraphicsContainer.AddElement(pTextElement, 0)

            ' Part 4: Add the (optional) textbox.
            ' Define the text font.
            pTextFont = New stdole.StdFont
            With pTextFont
                .Name = "Times New Roman"
                .Size = 12
            End With
            ' Define the text symbol.
            pTextSymbol = New TextSymbol
            With pTextSymbol
                .Font = pTextFont
                .Case = esriTextCase.esriTCNormal
                .HorizontalAlignment = esriTextHorizontalAlignment.esriTHACenter
            End With

            ' Define the textbox as a text element.
            pTextElement = New TextElement
            pTextElement.Text = "Text Box 1"
            pTextElement.Symbol = pTextSymbol
            pElemProp = pTextElement
            pElemProp.Name = Map_TextBox1

            ' Define the position to plot the textbox.
            pPntElement = pTextElement
            pPoint = New Point
            pPoint.X = 5.0#
            pPoint.Y = 1.0
            pPntElement.Geometry = pPoint
            ' Add the textbox to the graphic container.
            pGraphicsContainer.AddElement(pTextElement, 0)

            '==========================================
            'Legend
            '==========================================
            ' Part 1: Define the page layout.
            Dim pID As New UID
            Dim pMapSurroundFrame As IMapSurroundFrame
            Dim pMapSurround As IMapSurround = Nothing
            Dim pFrameElement As IElement
            Dim pEnvelope As IEnvelope

            ' Part 2: Create a legend map surround frame.
            ' Get the map frame.
            pMapFrame = pElement
            ' Create a legend map surround frame.
            pID.Value = "esriCore.Legend"
            pMapSurroundFrame = pMapFrame.CreateSurroundFrame(pID, pMapSurround)

            'set an outline of the legend
            Dim pEnumStyleGallery As IEnumStyleGalleryItem = pStyleGallery.Items("Borders", "ESRI.style", "Default")
            pEnumStyleGallery.Reset()

            Dim pStyleItem As IStyleGalleryItem2 = pEnumStyleGallery.Next
            Dim pBorder As IBorder = New SymbolBorder

            Do Until pStyleItem Is Nothing
                If pStyleItem.Name = "1.5 Point" Then
                    pBorder = pStyleItem.Item
                    pBorder.Gap = 3
                    pMapSurroundFrame.Border = pBorder
                    Exit Do
                End If
                pStyleItem = pEnumStyleGallery.Next
            Loop

            ' Part 3: Create the legend and add the legend to the graphics container.
            ' Define the geometry of the legend.
            pEnvelope = New Envelope
            pEnvelope.PutCoords(0.5#, 0.3#, 2.5#, 1.3#)
            pFrameElement = pMapSurroundFrame
            pFrameElement.Geometry = pEnvelope
            ' Activate the screeen display of the legend.
            pFrameElement.Activate(pActiveView.ScreenDisplay)
            ' Add the legend to the graphics container.
            pGraphicsContainer.AddElement(pFrameElement, 0)

            pStyleGallery = Nothing
            pStyleItem = Nothing
            pEnumStyleGallery = Nothing

            '==========================================
            'North Arrow
            '==========================================
            ' Part 2: Create a north arrow map surround frame.
            Dim pMarkerNorthArrow As IMarkerNorthArrow
            Dim pCharacterMarkerSymbol As ICharacterMarkerSymbol
            ' Get the map frame.
            pMapFrame = pElement
            ' Create a north arrow map surround frame.
            pID.Value = "esriCore.MarkerNorthArrow"
            ' Choose a north arrow design other than the default.
            pMarkerNorthArrow = New MarkerNorthArrow
            pCharacterMarkerSymbol = pMarkerNorthArrow.MarkerSymbol
            pCharacterMarkerSymbol.CharacterIndex = 176
            pMarkerNorthArrow.MarkerSymbol = pCharacterMarkerSymbol
            pMapSurround = pMarkerNorthArrow
            pMapSurroundFrame = pMapFrame.CreateSurroundFrame(pID, pMapSurround)

            ' Part 3: Create the north arrow and add it to the graphics container.
            ' Create a envelope for the north arrow.
            pEnvelope.PutCoords(7.4#, 0.5, 8.4#, 1.5)
            pFrameElement = pMapSurroundFrame
            pFrameElement.Geometry = pEnvelope
            pFrameElement.Activate(pActiveView.ScreenDisplay)
            ' Add the north arrow to the graphics container.
            pGraphicsContainer.AddElement(pFrameElement, 0)

            '==========================================
            'Scalebar
            '==========================================
            ' Part 2: Create a scalebar map surround frame.
            Dim pScaleMarks As IScaleMarks
            ' Get the map frame.
            pMapFrame = pElement
            ' Create a scale bar map surround frame.
            pID.Value = "esriCore.Scalebar"
            pMapSurround = New AlternatingScaleBar
            pScaleMarks = pMapSurround
            pScaleMarks.MarkFrequency = esriScaleBarFrequency.esriScaleBarMajorDivisions
            pMapSurroundFrame = pMapFrame.CreateSurroundFrame(pID, pMapSurround)

            ' Create a envelope for the scale bar.
            pEnvelope.PutCoords(3.8, 0.3#, 6.8, 0.8)
            pFrameElement = pMapSurroundFrame
            pFrameElement.Geometry = pEnvelope
            pFrameElement.Activate(pActiveView.ScreenDisplay)
            ' Add the scale bar to the graphics container.
            pGraphicsContainer.AddElement(pFrameElement, 0)
            pMxDoc.ActivatedView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, Nothing, Nothing)

        Catch ex As Exception
            Debug.Print("BA_AddMapElements Exception: " & ex.Message)
        Finally
            pPageLayout = Nothing
            'zoom to the layout extent
            pGraphicsContainer = Nothing
            pActiveView = Nothing
            pMElem = Nothing
            pElemProp = Nothing
            pMapFrame = Nothing
            pElement = Nothing
        End Try
    End Sub

    'map type:
    '1. Elevation Distribution
    '2. Elevation SNOTEL
    '3. Elevation Snow Course
    '4. Precipitation Distribution
    '5. Aspect Distribution
    Public Function BA_DisplayMap(ByVal pMxDocument As IMxDocument, ByVal maptype As Integer, ByVal selectedBasin As String, ByVal selectedAoi As String, _
                                  ByVal displayElevationInMeters As Boolean, ByVal subTitle As String) As Integer
        Dim LayerNames() As String
        Dim response As Integer
        Dim maptitle As String
        Dim KeyLayerName As String = Nothing
        Dim UnitText As String
        'Participated layers
        'Public Const BA_MapAOI = "AOI Boundary"
        'Public Const BA_MapElevationZone = "Elevation Zones"
        'Public Const BA_MapSNOTELZone = "SNOTEL Elevation Zones"
        'Public Const BA_MapSNOTELSite = "SNOTEL Sites"
        'Public Const BA_MapSnowCourseZone = "Snow Course Elevation Zones"
        'Public Const BA_MapSnowCourseSite = "Snow Courses"
        'Public Const BA_MapPrecipZone = "Precipitation Zones"
        'Public Const BA_MapHillshade = "Hillshade"
        'Public Const BA_MapStream = "AOI Streams"
        'Public Const BA_MapAspect = "Aspect"
        'Public Const BA_MapSlope = "Slope"
        Dim Basin_Name As String

        If Len(Trim(selectedBasin)) = 0 Then
            Basin_Name = ""
        Else
            Basin_Name = vbCrLf & " at " & selectedBasin
        End If

        maptitle = selectedAoi & Basin_Name

        Select Case maptype
            Case 1 'elevation distribution
                ReDim LayerNames(0 To 6)
                LayerNames(1) = BA_MAPS_AOI_BOUNDARY
                LayerNames(2) = BA_MAPS_STREAMS
                LayerNames(3) = BA_MAPS_HILLSHADE
                LayerNames(4) = BA_MAPS_ELEVATION_ZONES
                LayerNames(5) = BA_MAPS_SNOTEL_SITES
                LayerNames(6) = BA_MAPS_SNOW_COURSE_SITES
                KeyLayerName = BA_MAPS_ELEVATION_ZONES
                If displayElevationInMeters = True Then
                    UnitText = "Elevation Units = Meters"
                Else
                    UnitText = "Elevation Units = Feet"
                End If
            Case 2 'elevation snotel
                ReDim LayerNames(0 To 5)
                LayerNames(1) = BA_MAPS_AOI_BOUNDARY
                LayerNames(2) = BA_MAPS_STREAMS
                LayerNames(3) = BA_MAPS_HILLSHADE
                LayerNames(4) = BA_MAPS_SNOTEL_ZONES
                LayerNames(5) = BA_MAPS_SNOTEL_SITES
                KeyLayerName = BA_MAPS_SNOTEL_ZONES
                UnitText = "Area Represented = elevation zone" & vbCrLf & "between named site and next site below"
            Case 3 'elevation snow course
                ReDim LayerNames(0 To 5)
                LayerNames(1) = BA_MAPS_AOI_BOUNDARY
                LayerNames(2) = BA_MAPS_STREAMS
                LayerNames(3) = BA_MAPS_HILLSHADE
                LayerNames(4) = BA_MAPS_SNOW_COURSE_ZONES
                LayerNames(5) = BA_MAPS_SNOW_COURSE_SITES
                KeyLayerName = BA_MAPS_SNOW_COURSE_ZONES
                UnitText = "Area Represented = elevation zone" & vbCrLf & "between named site and next site below"
            Case 4 'precipitation distribution
                ReDim LayerNames(0 To 6)
                LayerNames(1) = BA_MAPS_AOI_BOUNDARY
                LayerNames(2) = BA_MAPS_STREAMS
                LayerNames(3) = BA_MAPS_HILLSHADE
                LayerNames(4) = BA_MAPS_SNOTEL_SITES
                LayerNames(5) = BA_MAPS_SNOW_COURSE_SITES
                LayerNames(6) = BA_MAPS_PRECIPITATION_ZONES
                KeyLayerName = BA_MAPS_PRECIPITATION_ZONES
                UnitText = "Precipitation Units = Inches"
            Case 5 'aspect
                ReDim LayerNames(0 To 6)
                LayerNames(1) = BA_MAPS_AOI_BOUNDARY
                LayerNames(2) = BA_MAPS_STREAMS
                LayerNames(3) = BA_MAPS_HILLSHADE
                LayerNames(4) = BA_MAPS_ASPECT
                LayerNames(5) = BA_MAPS_SNOTEL_SITES
                LayerNames(6) = BA_MAPS_SNOW_COURSE_SITES
                KeyLayerName = BA_MAPS_ASPECT
                UnitText = " "
            Case 6 'slope
                ReDim LayerNames(0 To 6)
                LayerNames(1) = BA_MAPS_AOI_BOUNDARY
                LayerNames(2) = BA_MAPS_STREAMS
                LayerNames(3) = BA_MAPS_HILLSHADE
                LayerNames(4) = BA_MAPS_SLOPE
                LayerNames(5) = BA_MAPS_SNOTEL_SITES
                LayerNames(6) = BA_MAPS_SNOW_COURSE_SITES
                KeyLayerName = BA_MAPS_SLOPE
                UnitText = " "
            Case 7 'Scenario 1
                ReDim LayerNames(0 To 11)
                LayerNames(1) = BA_MAPS_AOI_BOUNDARY
                LayerNames(2) = BA_MAPS_STREAMS
                LayerNames(3) = BA_MAPS_HILLSHADE
                LayerNames(4) = BA_MAPS_ELEVATION_ZONES
                LayerNames(5) = BA_MAPS_SNOTEL_SITES
                LayerNames(6) = BA_MAPS_SNOTEL_SCENARIO1
                LayerNames(7) = BA_MAPS_SNOW_COURSE_SCENARIO1
                LayerNames(8) = BA_MAPS_PSEUDO_SCENARIO1
                LayerNames(9) = BA_MAPS_SNOW_COURSE_SITES
                LayerNames(10) = BA_MAPS_PSEUDO_SITES
                LayerNames(11) = BA_MAPS_SCENARIO1_REPRESENTATION
                'KeyLayerName = BA_MAPS_SCENARIO1_REPRESENTATION
                UnitText = "Scenario 1 sites are circled in black"
            Case 8 'Scenario 2
                ReDim LayerNames(0 To 11)
                LayerNames(1) = BA_MAPS_AOI_BOUNDARY
                LayerNames(2) = BA_MAPS_STREAMS
                LayerNames(3) = BA_MAPS_HILLSHADE
                LayerNames(4) = BA_MAPS_ELEVATION_ZONES
                LayerNames(5) = BA_MAPS_SNOTEL_SITES
                LayerNames(6) = BA_MAPS_SNOTEL_SCENARIO2
                LayerNames(7) = BA_MAPS_SNOW_COURSE_SCENARIO2
                LayerNames(8) = BA_MAPS_PSEUDO_SCENARIO2
                LayerNames(9) = BA_MAPS_SNOW_COURSE_SITES
                LayerNames(10) = BA_MAPS_PSEUDO_SITES
                LayerNames(11) = BA_MAPS_SCENARIO2_REPRESENTATION
                'KeyLayerName = BA_MAPS_SCENARIO2_REPRESENTATION
                UnitText = "Scenario 2 sites are circled in gold"
            Case 9 'Difference of Representations
                ReDim LayerNames(0 To 16)
                LayerNames(1) = BA_MAPS_AOI_BOUNDARY
                LayerNames(2) = BA_MAPS_STREAMS
                LayerNames(3) = BA_MAPS_HILLSHADE
                LayerNames(4) = BA_MAPS_SNOTEL_SITES
                LayerNames(5) = BA_MAPS_SNOTEL_SCENARIO1
                LayerNames(6) = BA_MAPS_SNOTEL_SCENARIO2
                LayerNames(7) = BA_MAPS_SNOW_COURSE_SCENARIO1
                LayerNames(8) = BA_MAPS_SNOW_COURSE_SCENARIO2
                LayerNames(9) = BA_MAPS_PSEUDO_SCENARIO1
                LayerNames(10) = BA_MAPS_PSEUDO_SCENARIO2
                LayerNames(11) = BA_MAPS_SNOW_COURSE_SITES
                LayerNames(12) = BA_MAPS_PSEUDO_SITES
                LayerNames(13) = BA_MAPS_SCENARIO1_REPRESENTATION
                LayerNames(14) = BA_MAPS_SCENARIO2_REPRESENTATION
                LayerNames(15) = BA_MAPS_BOTH_REPRESENTATION
                LayerNames(16) = BA_MAPS_NOT_REPRESENTED
                KeyLayerName = Nothing
                UnitText = "Scenario 1 and scenario 2 sites are circled" & vbCrLf & "in black and gold respectively"
            Case Else
                MsgBox(maptype & " is not a valid option!")
                Return -1
        End Select
        'convert subtitle to uppercase
        BA_MapUpdateSubTitle(pMxDocument, maptitle, subTitle.ToUpper, UnitText)
        response = BA_ToggleLayersinMapFrame(pMxDocument, LayerNames)
        'BA_ZoomToAOI (AOIFolderBase) 'zoom to current AOI
        BA_ToggleView(pMxDocument, False) 'switch to the may layout view
        BA_SetLegendFormat(pMxDocument, KeyLayerName)
        Return 1
    End Function

    Public Sub BA_MapUpdateSubTitle(ByVal pMxDoc As IMxDocument, ByVal TitleText As String, ByVal subtitletext As String, ByVal textboxtext As String)
        'Set the page layout.
        Dim pPageLayout As IPageLayout
        Dim pGraphicsContainer As IGraphicsContainer
        Dim pActiveView As IActiveView
        pPageLayout = pMxDoc.PageLayout
        pGraphicsContainer = pPageLayout
        pActiveView = pPageLayout

        Dim pElem As IElement
        Dim pTextElem As ITextElement
        Dim pElemProp As IElementProperties2

        pGraphicsContainer.Reset()
        pElem = pGraphicsContainer.Next

        Do While Not pElem Is Nothing
            If TypeOf pElem Is ITextElement Then
                pTextElem = pElem
                pElemProp = pElem

                Select Case pElemProp.Name
                    Case "Title"
                        pTextElem.Text = TitleText
                        pGraphicsContainer.UpdateElement(pElem)
                    Case "SubTitle"
                        pTextElem.Text = subtitletext
                        pGraphicsContainer.UpdateElement(pElem)
                    Case "TextBox1"
                        pTextElem.Text = textboxtext
                        pGraphicsContainer.UpdateElement(pElem)
                    Case Else
                End Select
            End If

            pElem = pGraphicsContainer.Next
        Loop

        pActiveView.Refresh()
    End Sub

    'toggle layers on based on a name provided in a list, all other layers will be set to off
    Public Function BA_ToggleLayersinMapFrame(ByVal pmxDoc As IMxDocument, ByVal LayerNameList() As String) As Integer
        Dim i As Integer, j As Integer
        Dim nlayers As Integer, nvisible As Integer
        Dim checkname As String
        Dim pMap As IMap
        pMap = pmxDoc.FocusMap

        nlayers = pMap.LayerCount
        nvisible = UBound(LayerNameList)

        If nlayers * nvisible = 0 Then
            BA_ToggleLayersinMapFrame = 0
            Exit Function
        End If

        'disable all layers first
        For i = 0 To nlayers - 1
            pMap.Layer(i).Visible = False
        Next

        'check layer name to enable only the ones that match
        For j = 1 To nvisible
            checkname = LayerNameList(j)
            For i = 0 To nlayers - 1
                If pMap.Layer(i).Name = checkname Then
                    pMap.Layer(i).Visible = True
                    Exit For
                End If
            Next i
        Next j

        pmxDoc.ActivatedView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
        pmxDoc.UpdateContents()
        Return 1
    End Function

    Public Sub BA_SetLegendFormat(ByVal pmxDoc As IMxDocument, ByVal TitleText As String)
        'Set the page layout.
        Dim pPageLayout As IPageLayout
        Dim pGraphicsContainer As IGraphicsContainer
        Dim pActiveView As IActiveView

        pPageLayout = pmxDoc.PageLayout
        pGraphicsContainer = pPageLayout
        pActiveView = pPageLayout

        'check if map elements were added
        Dim i As Integer
        Dim pMElem As IElement
        Dim pElemProp As IElementProperties2

        pGraphicsContainer.Reset()
        pMElem = pGraphicsContainer.Next
        Dim IsLegend As Boolean
        IsLegend = False

        Do While Not pMElem Is Nothing
            pElemProp = pMElem
            If pElemProp.Name = "Legend" Then
                IsLegend = True
                Exit Do
            End If
            pMElem = pGraphicsContainer.Next
        Loop

        Dim pMapSurround As IMapSurround
        Dim pMapSurroundFrame As IMapSurroundFrame
        Dim pLegend As ILegend
        Dim pLegendItem As ILegendItem
        Dim pLFormat As ILegendFormat
        Dim pLClsFormat As ILegendClassFormat

        Dim pTSymbol As ITextSymbol

        If IsLegend Then
            pMapSurroundFrame = pMElem
            pMapSurround = pMapSurroundFrame.MapSurround
            pLegend = pMapSurround

            'set overall patch format
            pLFormat = pLegend.Format

            'prepare textsymbol for the title
            pTSymbol = New TextSymbol
            pTSymbol.Size = 16
            pTSymbol.HorizontalAlignment = esriTextHorizontalAlignment.esriTHALeft
            pLegend.Title = TitleText

            With pLFormat
                .ShowTitle = False 'do not show legend title
                .TitleSymbol = pTSymbol
                .DefaultPatchHeight = 15
                .DefaultPatchWidth = 25
                .VerticalPatchGap = 3
                .HorizontalPatchGap = 12
            End With

            'set individual class format
            'prepare textsymbol for item label
            pTSymbol.Size = 12
            pTSymbol.VerticalAlignment = esriTextVerticalAlignment.esriTVACenter

            For i = pLegend.ItemCount - 1 To 0 Step -1
                pLegendItem = pLegend.Item(i)
                'display only the layer name for Legend item that has the "titletext" as the layer name
                If pLegendItem.Layer.Name = TitleText Then
                    pLegendItem.LayerNameSymbol = pTSymbol
                    pLegendItem.ShowLayerName = True
                Else
                    pLegendItem.ShowLayerName = False
                End If
                pLClsFormat = pLegendItem.LegendClassFormat
                pLClsFormat.LabelSymbol = pTSymbol
            Next

            pLegend.Refresh()

            pLegend = Nothing
            pMapSurroundFrame = Nothing
            pMElem = Nothing
            pMapSurround = Nothing
            pLFormat = Nothing
            pmxDoc.ActiveView.Refresh()
        End If

        pGraphicsContainer = Nothing
        pActiveView = Nothing
        pPageLayout = Nothing
    End Sub

    'remove background layers from the map legend
    'also set the format of the legend
    Public Sub BA_RemoveLayersfromLegend(ByVal pMxDoc As IMxDocument)
        Dim LayerCount As Integer = 11
        Dim LayerNames() As String
        'layers to be removed are:
        ReDim LayerNames(0 To LayerCount)
        LayerNames(1) = BA_MAPS_AOI_BOUNDARY
        LayerNames(2) = BA_MAPS_HILLSHADE
        LayerNames(3) = BA_MAPS_STREAMS
        LayerNames(4) = BA_MAPS_SNOTEL_SCENARIO1
        LayerNames(5) = BA_MAPS_SNOTEL_SCENARIO2
        LayerNames(6) = BA_MAPS_SNOW_COURSE_SCENARIO1
        LayerNames(7) = BA_MAPS_SNOW_COURSE_SCENARIO2
        LayerNames(8) = BA_MAPS_PSEUDO_SCENARIO1
        LayerNames(9) = BA_MAPS_PSEUDO_SCENARIO2
        LayerNames(10) = BA_MAPS_FILLED_DEM
        LayerNames(11) = BA_MAPS_NOT_REPRESENTED

        'Set the page layout.
        Dim pPageLayout As IPageLayout = pMxDoc.PageLayout
        Dim pGraphicsContainer As IGraphicsContainer = CType(pPageLayout, IGraphicsContainer)   'Explicit Cast
        Dim pActiveView As IActiveView = CType(pPageLayout, IActiveView)    'Explicit Cast

        'check if map elements were added
        Dim i As Integer, j As Integer
        Dim pMElem As IElement
        Dim pElemProp As IElementProperties2

        pGraphicsContainer.Reset()
        pMElem = pGraphicsContainer.Next
        Dim IsLegend As Boolean = False

        Do While Not pMElem Is Nothing
            pElemProp = pMElem
            If pElemProp.Name = "Legend" Then
                IsLegend = True
                Exit Do
            End If
            pMElem = pGraphicsContainer.Next
        Loop

        Dim pMapSurround As IMapSurround
        Dim pMapSurroundFrame As IMapSurroundFrame
        Dim pLegend As ILegend

        If IsLegend Then
            pMapSurroundFrame = pMElem
            pMapSurround = pMapSurroundFrame.MapSurround
            pLegend = pMapSurround

            For i = pLegend.ItemCount - 1 To 0 Step -1
                For j = 1 To LayerCount
                    If pLegend.Item(i).Layer.Name = LayerNames(j) Then
                        pLegend.RemoveItem(i)
                        Exit For
                    End If
                Next
            Next

            pLegend.Refresh()

            pLegend = Nothing
            pMapSurroundFrame = Nothing
            pMElem = Nothing
            pMapSurround = Nothing

            pMxDoc.ActiveView.Refresh()
        End If

        pGraphicsContainer = Nothing
        pActiveView = Nothing
        pPageLayout = Nothing
    End Sub

    'all area values are in square meters
    Public Function BA_Map_CalculateNonRepresented(ByVal pMxDocument As IMxDocument, ByVal aoiPath As String, ByVal MinElev As Double, _
                                                   ByVal MaxElev As Double, ByVal LowerBuffer As Double, ByVal LowerLimit As Double, _
                                                   ByVal UpperLimit As Double, ByRef Interval_List() As BA_IntervalList, ByVal OutputPath As String, _
                                                   ByVal OutputName As String) As Double

        'check if there is any non-represented area
        Dim No_LowerNP As Boolean
        Dim No_UpperNP As Boolean
        Dim AOIArea As Double
        Dim ninterval As Integer
        Dim LBndValue As Double, UBndValue As Double
        Dim i As Integer, response As Integer

        Dim InputPath As String, InputName As String
        Dim RasterName As String, VectorName As String
        Dim MessageKey As AOIMessageKey

        If LowerLimit >= UpperLimit Then 'invalid input
            ReDim Interval_List(0 To 1)
            Return -1
        End If

        If LowerBuffer < 0 Then LowerBuffer = 0

        'check for upper and lower NP areas
        If UpperLimit >= MaxElev Then 'the elevation of at least one site is the same as (or higher than) the highest elevation in AOI
            No_UpperNP = True
            UBndValue = MaxElev 'this variable is used to set interval values for reclassification
        Else
            No_UpperNP = False
            UBndValue = UpperLimit
        End If

        If LowerLimit - LowerBuffer <= MinElev Then 'the elevation of at least one site is the same as (or lower than) the lowest elevation in AOI
            No_LowerNP = True
            LBndValue = MinElev 'this variable is used to set interval values for reclassification
        Else
            No_LowerNP = False
            LBndValue = LowerLimit - LowerBuffer
        End If

        If No_UpperNP And No_LowerNP Then 'all elevation range is represented
            ReDim Interval_List(0 To 1)
            Return 0
        End If

        InputPath = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Surfaces)
        InputName = BA_EnumDescription(MapsFileName.filled_dem_gdb)
        RasterName = Trim(OutputName)
        VectorName = RasterName & "_v"
        MessageKey = AOIMessageKey.NonRepresented

        'remove raster datasets if they exist
        response = BA_RemoveLayersInFolder(pMxDocument, OutputPath) 'remove layers from map

        If BA_File_Exists(OutputPath & "\" & RasterName, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            'delete raster
            response = BA_RemoveRasterFromGDB(OutputPath, RasterName)
            If response = 0 Then 'unable to delete the folder
                ReDim Interval_List(0 To 1)
                Return -1
            End If
        End If

        ReDim Interval_List(0 To 3)
        ninterval = 1
        If Not No_LowerNP Then 'has lower non-represented area
            'set lower interval
            With Interval_List(ninterval)
                .Value = 1
                .Name = "Not represented below"
                .LowerBound = MinElev
                .UpperBound = LBndValue
            End With
            ninterval = ninterval + 1
        End If

        With Interval_List(ninterval)
            .Value = 2 'represented
            .Name = "Represented"
            .LowerBound = LBndValue
            .UpperBound = UBndValue
        End With

        If Not No_UpperNP Then 'has upper non-represented area
            ninterval = ninterval + 1

            'set upper interval
            With Interval_List(ninterval)
                .Value = 3
                .Name = "Not represented above"
                .LowerBound = UBndValue
                .UpperBound = MaxElev
            End With
        End If

        If ninterval < 3 Then ReDim Preserve Interval_List(0 To ninterval)

        'reclassify raster
        Dim pInGDataset As IGeoDataset = BA_OpenRasterFromGDB(InputPath, InputName)
        If pInGDataset Is Nothing Then
            ReDim Interval_List(0 To 1)
            Return -1
        End If

        'Set Remap
        Dim pNumberReMap As INumberRemap = New NumberRemap

        For i = 1 To ninterval
            'messagestring = messagestring & vbCrLf & Interval_List(i).LowerBound & "-" & Interval_List(i).UpperBound
            pNumberReMap.MapRange(CDbl(Interval_List(i).LowerBound), CDbl(Interval_List(i).UpperBound), CLng(Interval_List(i).Value))
        Next

        'MsgBox messagestring

        'Reclass by Remap
        Dim pReclassOp As IReclassOp = New RasterReclassOp
        Dim pRasterGD As IGeoDataset = pReclassOp.ReclassByRemap(pInGDataset, pNumberReMap, True)

        'save the output raster
        response = BA_SaveRasterDatasetGDB(pRasterGD, OutputPath, BA_RASTER_FORMAT, RasterName)
        pReclassOp = Nothing
        pInGDataset = Nothing
        pNumberReMap = Nothing
        pRasterGD = Nothing

        'add attributes to raster and get area of each class
        'response = Map_ReclassRasterAttributes(Interval_List, OutputPath, RasterName)
        response = BA_ReclassRasterAttributes(Interval_List, OutputPath, OutputName)

        ninterval = UBound(Interval_List)
        If ninterval > 1 Then
            'find areas not represented
            AOIArea = 0
            For i = 1 To ninterval
                AOIArea = AOIArea + Interval_List(i).Area
            Next
        End If

        Return AOIArea
    End Function

    'add a vector file to the active view
    'First, add data to the classification using SetHistogramData.  
    'Next, set the number of classes and generate breaks using Classify (typically use the same number of 
    'classes specified here for IClassBreaksRenderer.BreakCount). 
    'Then apply the classification to the renderer by cycling through ClassBreaks and setting each IClassBreaksRenderer.Break.

    Public Function BA_DisplayVectorWithSymbol(ByVal pMxDoc As IMxDocument, ByVal pFLayer As IFeatureLayer, ByVal FieldName As String, ByVal NumberClass As Long, _
                                               ByVal DisplayName As String, ByVal DisplayStyle As String, ByVal Transparency As Integer, _
                                               ByVal buffer_Factor As Double) As BA_ReturnCode
        Dim return_code As BA_ReturnCode
        Dim i As Long

        return_code = BA_ReturnCode.UnknownError

        'add featureclass to current data frame
        pFLayer.Name = DisplayName

        'create histogram for classification
        Dim pTable As ITable = pFLayer
        Dim pTableHistogram As ITableHistogram = New BasicTableHistogram
        'these will hold the histogram value and freq arrays
        Dim DataValues As Object = Nothing
        Dim DataFrequencies As Object = Nothing

        'setup a table histogram object to point at the table and attribute field
        pTableHistogram.Field = FieldName
        pTableHistogram.Table = pTable

        Dim pHistogram As IBasicHistogram = pTableHistogram
        pHistogram.GetHistogram(DataValues, DataFrequencies)

        'use the histogram to classify the attribute into mapping classes
        Dim pClassify As IClassify = New NaturalBreaks
        pClassify.SetHistogramData(DataValues, DataFrequencies)
        pClassify.Classify(NumberClass)

        NumberClass = UBound(pClassify.ClassBreaks)
        'ClassBreaks returns an array of class breaks. The number of breaks may be different 
        'than what was specified in the Classify method, so it is essential that you check this
        'before setting the BreakCount on IClassBreaksRenderer. The reason this may be different
        'has to do with how a particular classification method handles skewness in the data being 
        'classified.

        ' Getting color ramp from Style Gallery
        Dim pMap As IMap = pMxDoc.FocusMap

        Dim pStyleGallery As IStyleGallery = pMxDoc.StyleGallery

        Dim StyleName As String
        Dim StyleCategory As String
        Dim StyleKey As String = UCase(Left(Trim(DisplayStyle), 1))

        If Len(StyleKey) > 0 Then
            Select Case StyleKey
                Case "A" '"ASPECT"
                    StyleName = "Aspect"
                    StyleCategory = "Default Ramps"
                Case "E" '"ELEVATION"
                    StyleName = "Elevation #2"
                    StyleCategory = "Default Ramps"
                Case "P" '"PRECIPITATION"
                    StyleName = "Precipitation"
                    StyleCategory = "Default Ramps"
                Case "R" '"Range/Random"
                    StyleName = "Basic Random"
                    StyleCategory = "Default Schemes"
                Case "S" '"SLOPE"
                    StyleName = "Slope"
                    StyleCategory = "Spatial Ramps"
                Case Else
                    StyleName = "Black to White"
                    StyleCategory = "Default Ramps"
            End Select
        Else
            StyleName = "Black to White"
            StyleCategory = "Default Ramps"
        End If

        Dim pEnumStyleGallery As IEnumStyleGalleryItem = pStyleGallery.Items("Color Ramps", "ESRI.style", StyleCategory)

        pEnumStyleGallery.Reset()

        Dim pStyleItem As IStyleGalleryItem
        pStyleItem = pEnumStyleGallery.Next

        Dim pColorRamp As IColorRamp = New PresetColorRamp
        Do Until pStyleItem Is Nothing
            If pStyleItem.Name = StyleName Then
                pColorRamp = pStyleItem.Item
                Exit Do
            End If
            pStyleItem = pEnumStyleGallery.Next
        Loop

        'assign value to the colorramp
        With pColorRamp
            .Size = NumberClass
            .CreateRamp(True)
        End With

        'create raster renderer
        Dim pCBRenderer As IClassBreaksRenderer = New ClassBreaksRenderer
        pCBRenderer.Field = FieldName
        pCBRenderer.BreakCount = NumberClass
        Dim pRenderer As IFeatureRenderer = pCBRenderer

        Dim pFSymbol As ISimpleFillSymbol = New SimpleFillSymbol
        'pCBRenderer.MinimumBreak = pClassify.ClassBreaks(0)
        Dim startLabel As Double = pClassify.ClassBreaks(0)
        Dim endLabel As Double
        For i = 0 To NumberClass - 1
            endLabel = Math.Round(pClassify.ClassBreaks(i + 1), 2)
            pCBRenderer.Break(i) = pClassify.ClassBreaks(i + 1)
            'pCBRenderer.Label(i) = pClassify.ClassBreaks(i + 1)
            If i > 0 Then
                pCBRenderer.Label(i) = startLabel & " - " & endLabel
            Else
                pCBRenderer.Label(i) = endLabel
            End If
            startLabel = endLabel
            pFSymbol.Color = pColorRamp.Color(i)
            pCBRenderer.Symbol(i) = pFSymbol
        Next

        'Update the renderer with new settings and plug into layer
        Dim pGFLayer As IGeoFeatureLayer = pFLayer
        pGFLayer.Renderer = pRenderer

        'set layer transparency
        Dim pLayerEffects As ILayerEffects = pFLayer
        If pLayerEffects.SupportsTransparency Then
            pLayerEffects.Transparency = Transparency
        End If

        'check if a layer with the assigned name exists
        'search layer of the specified name, if found
        Dim nlayers As Long = pMap.LayerCount
        Dim pTempLayer As ILayer

        For i = nlayers To 1 Step -1
            pTempLayer = pMap.Layer(i - 1)
            If DisplayName = pTempLayer.Name Then 'remove the layer
                pMap.DeleteLayer(pTempLayer)
            End If
        Next

        pMxDoc.AddLayer(pFLayer)
        pMxDoc.UpdateContents()

        Dim urx, llx, lly, ury As Double
        Dim xrange, yrange As Double
        Dim xoffset, yoffset As Double
        'zoom to layer
        'create a buffer around the AOI

        Dim pEnv As IEnvelope = pFLayer.AreaOfInterest
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

        Dim pActiveView As IActiveView = pMxDoc.ActiveView
        pActiveView.Extent = pEnv


        'refresh the active view - not yet; we will do this later
        'pMxDoc.ActivatedView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
        return_code = BA_ReturnCode.Success

        pTempLayer = Nothing
        pLayerEffects = Nothing
        pFSymbol = Nothing
        pRenderer = Nothing
        pCBRenderer = Nothing
        pStyleItem = Nothing
        pEnumStyleGallery = Nothing
        pStyleGallery = Nothing
        pColorRamp = Nothing
        pFLayer = Nothing
        pGFLayer = Nothing
        pActiveView = Nothing
        pEnv = Nothing

        Return return_code
    End Function

    Public Function BA_GetLayerIndexByName(ByVal pMxDoc As IMxDocument, ByVal layerName As String) As Integer
        Dim pMap As IMap = pMxDoc.FocusMap

        If layerName.Trim.Length = 0 Or pMap.LayerCount <= 0 Then
            Return -1
            Exit Function
        End If

        Dim pLayer As ILayer
        Dim i As Integer
        Try
            For i = 0 To pMap.LayerCount - 1
                pLayer = pMap.Layer(i)
                If pLayer.Name = layerName Then Exit For
            Next

            If i = pMap.LayerCount Then i = -1 'layer not found
        Catch ex As Exception
            MessageBox.Show("BA_GetLayerIndexByName Exception: " + ex.Message)
        Finally
            pLayer = Nothing
            pMap = Nothing
            pMxDoc = Nothing
        End Try
        Return i
    End Function

    Public Function BA_GetLayerIndexByFilePath(ByVal pMxDoc As IMxDocument, ByVal filePath As String, ByVal fileName As String) As Integer
        Dim pMap As IMap = pMxDoc.FocusMap

        If filePath.Trim.Length = 0 Or pMap.LayerCount <= 0 Then
            Return -1
            Exit Function
        End If

        Dim pLayer As ILayer
        Dim i As Integer
        Try
            Dim matchPath As String = filePath & BA_StandardizeShapefileName(fileName, False, True)
            For i = 0 To pMap.LayerCount - 1
                pLayer = pMap.Layer(i)
                If TypeOf pLayer Is IFeatureLayer Or _
                    TypeOf pLayer Is IRasterLayer Then
                    Dim pDSet As IDataset = CType(pLayer, IDataset)
                    Dim layerFilePath As String = pDSet.Workspace.PathName
                    Dim layerFileName As String = pDSet.BrowseName
                    layerFileName = BA_StandardizeShapefileName(layerFileName, False, True)
                    If layerFilePath & layerFileName = matchPath Then Exit For
                End If
            Next

            If i = pMap.LayerCount Then i = -1 'layer not found
        Catch ex As Exception
            MessageBox.Show("BA_GetLayerIndexByFilePath Exception: " + ex.Message)
        Finally
            pLayer = Nothing
            pMap = Nothing
            pMxDoc = Nothing
        End Try
        Return i
    End Function

    Public Function BA_BufferPoint(ByVal folderPath As String, ByVal fileName As String, ByVal objectId As Integer, _
                                   ByVal bufferDistance As Double) As IPolygon
        Dim comReleaser As ESRI.ArcGIS.ADF.ComReleaser = New ESRI.ArcGIS.ADF.ComReleaser()
        Dim fClass As IFeatureClass
        Dim fCursor As IFeatureCursor
        Dim fQueryFilter As IQueryFilter = New QueryFilter
        Dim pFeature As IFeature
        Dim topoOpr As ITopologicalOperator
        Try
            fClass = BA_OpenFeatureClassFromGDB(folderPath, fileName)
            comReleaser.ManageLifetime(fClass)
            If fClass IsNot Nothing Then
                fQueryFilter.WhereClause = "OBJECTID = " & objectId
                fCursor = fClass.Search(fQueryFilter, False)
                comReleaser.ManageLifetime(fCursor)
                pFeature = fCursor.NextFeature
                comReleaser.ManageLifetime(pFeature)
                If pFeature IsNot Nothing Then
                    topoOpr = CType(pFeature.Shape, ITopologicalOperator)
                    Return topoOpr.Buffer(bufferDistance)
                End If
            End If
            Return Nothing
        Catch ex As Exception
            Debug.Print("BA_BufferPoint Exception: " & ex.Message)
            Return Nothing
        End Try
    End Function

End Module