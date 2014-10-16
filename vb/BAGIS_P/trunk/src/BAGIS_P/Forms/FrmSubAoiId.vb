Imports System.Windows.Forms
Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports System.Text

Public Class FrmSubAoiId



    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub RefreshSubAoiGrid()
        GrdSubAoi.Rows.Clear()
        Dim subAoiPaths As IList(Of String) = BA_GetListOfSubAoiPaths(TxtAoiPath.Text)
        For Each subAoi As String In subAoiPaths
            Dim sName As String = BA_GetBareName(subAoi)
            '---create a row---
            Dim item As New DataGridViewRow
            item.CreateCells(GrdSubAoi)
            item.Cells(0).Value = sName
            GrdSubAoi.Rows.Add(item)
        Next
        'De-select any rows
        If GrdSubAoi.Rows.Count > 0 Then
            GrdSubAoi.CurrentCell = Nothing
            GrdSubAoi.ClearSelection()
            GrdSubAoi.Refresh()
        End If
        'Manage display button
        If GrdSubAoi.Rows.Count > 0 Then
            BtnDisplaySubAoi.Enabled = True
        Else
            BtnDisplaySubAoi.Enabled = False
        End If
    End Sub

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    Private Sub BtnDisplaySubAoi_Click(sender As System.Object, e As System.EventArgs) Handles BtnDisplaySubAoi.Click
        Dim pFSymbol As ISimpleFillSymbol = New SimpleFillSymbol
        Dim pColorRamp As IColorRamp = New PresetColorRamp
        Dim pStyleGallery As IStyleGallery = Nothing
        Dim pEnumStyleGallery As IEnumStyleGalleryItem = Nothing
        Dim pStyleItem As IStyleGalleryItem = Nothing
        Dim pRasterDS As IRasterDataset = Nothing
        Dim pRaster As IRaster = Nothing
        Dim pLayerEffects As ILayerEffects = Nothing
        Dim pTempLayer As ILayer = Nothing

        Try
            pStyleGallery = My.Document.StyleGallery
            pEnumStyleGallery = pStyleGallery.Items("Color Ramps", "ESRI.style", "Default Schemes")
            pEnumStyleGallery.Reset()

            pStyleItem = pEnumStyleGallery.Next
            Do Until pStyleItem Is Nothing
                If pStyleItem.Name = "Cool Tones" Then
                    pColorRamp = pStyleItem.Item
                    Exit Do
                End If
                pStyleItem = pEnumStyleGallery.Next
            Loop

            'assign value to the colorramp
            With pColorRamp
                .Size = GrdSubAoi.Rows.Count
                .CreateRamp(True)
            End With

            Dim i As Integer = 0
            For Each pRow As DataGridViewRow In GrdSubAoi.Rows
                Dim includeSub As Boolean = pRow.Cells(1).Value
                Dim displayName As String = pRow.Cells(0).Value
                If includeSub = True Then
                    Dim aoiGdbPath As String = BA_GeodatabasePath(displayName, GeodatabaseNames.Aoi)
                    Dim layerPathName As String = TxtAoiPath.Text & "\" & aoiGdbPath & BA_EnumDescription(PublicPath.AoiGrid)

                    Dim pUVRen As IRasterUniqueValueRenderer = New RasterUniqueValueRenderer
                    Dim pRasRen As IRasterRenderer = pUVRen

                    ' Connect renderer and raster 
                    pRasterDS = BA_OpenRasterFromGDB(TxtAoiPath.Text & "\" & aoiGdbPath, BA_GetBareName(BA_EnumDescription(PublicPath.AoiGrid)))
                    pRaster = pRasterDS.CreateDefaultRaster
                    pRasRen.Raster = pRaster
                    pRasRen.Update()

                    ' Configure UniqueValue renderer
                    pUVRen.HeadingCount = 1   ' Use one heading 
                    pUVRen.ClassCount(0) = 1
                    pUVRen.Field = BA_FIELD_VALUE

                    Dim Value As Object = 1
                    pUVRen.AddValue(0, 0, Value)
                    pUVRen.Label(0, 0) = CStr(Value)
                    pFSymbol.Color = pColorRamp.Color(i)
                    pUVRen.Symbol(0, 0) = pFSymbol

                    'add raster to current data frame
                    Dim pRLayer As IRasterLayer = New RasterLayer
                    pRLayer.CreateFromDataset(pRasterDS)
                    pRLayer.Name = displayName

                    ' Update render and refresh layer
                    pRasRen.Update()
                    pRLayer.Renderer = pUVRen

                    'set layer transparency
                    pLayerEffects = pRLayer
                    If pLayerEffects.SupportsTransparency Then
                        pLayerEffects.Transparency = 50
                    End If

                    'check if a layer with the assigned name exists
                    'search layer of the specified name, if found
                    Dim pMap As IMap = My.Document.FocusMap
                    Dim nlayers As Integer = pMap.LayerCount
                    For i = nlayers To 1 Step -1
                        pTempLayer = CType(pMap.Layer(i - 1), ILayer)   'Explicit cast
                        If displayName = pTempLayer.Name Then 'remove the layer
                            pMap.DeleteLayer(pTempLayer)
                        End If
                    Next

                    My.Document.AddLayer(pRLayer)
                    My.Document.UpdateContents()

                    i += 1
                    pUVRen = Nothing
                    pRasRen = Nothing
                    pRLayer = Nothing
                Else
                    'If the layer is not checked, remove it if it is displayed
                    Dim pMap As IMap = My.Document.FocusMap
                    Dim nlayers As Integer = pMap.LayerCount
                    For i = nlayers To 1 Step -1
                        pTempLayer = CType(pMap.Layer(i - 1), ILayer)   'Explicit cast
                        If displayName = pTempLayer.Name Then 'remove the layer
                            pMap.DeleteLayer(pTempLayer)
                        End If
                    Next
                End If
            Next
            'refresh the active view
            My.Document.ActivatedView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
        Catch ex As Exception
            Debug.Print("BtnDisplaySubAoi_Click " & ex.Message)
        Finally
            pFSymbol = Nothing
            pColorRamp = Nothing
            pRasterDS = Nothing
            pRaster = Nothing
            pLayerEffects = Nothing
            pTempLayer = Nothing
        End Try
    End Sub

    Private Sub BtnCreateSubAoi_Click(sender As System.Object, e As System.EventArgs) Handles BtnCreateSubAoi.Click
        ' Create/configure a step progressor
        Dim pStepProg As IStepProgressor = Nothing
        Dim progressDialog2 As IProgressDialog2 = Nothing
        Dim pGeodataset As IGeoDataset = Nothing
        Dim pRasterBandCollection As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pTable As ITable = Nothing
        Dim pQF As IQueryFilter = New QueryFilter
        Try
            Dim message, title As String
            Dim defaultValue As String = Nothing
            Dim outputFile As String = Nothing
            Dim subAoiPath As String = TxtAoiPath.Text & BA_EnumDescription(PublicPath.BagisSubAoiGdb)

            ' Set prompt.
            message = "Enter a name for the SubAOI Id layer:"
            ' Set title.
            title = "Create SubAOI Id Layer"

            ' Display message, title, and default value.
            outputFile = InputBox(message, title, defaultValue, 150, 150)
            ' If user has clicked Cancel, set myValue to defaultValue 
            If String.IsNullOrEmpty(outputFile) Then
                MessageBox.Show("You did not enter a name for the SubAOI Id layer. No layer will be created.", title, _
                                MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Else
                If BA_File_Exists(subAoiPath & "\" & outputFile, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                    Dim returnValue As DialogResult = MessageBox.Show("A file exists with that name. Do you wish to overwrite it?" & vbCrLf & "This action cannot be undone.", title, _
                                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    If returnValue <> Windows.Forms.DialogResult.Yes Then
                        Exit Sub
                    Else
                        BA_RemoveRasterFromGDB(subAoiPath, outputFile)
                    End If
                End If
            End If

            pStepProg = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 20)
            progressDialog2 = BA_GetProgressDialog(pStepProg, "Checking target geodatabase...", "Creating geodatabase")
            pStepProg.Show()
            progressDialog2.ShowDialog()
            pStepProg.Step()
            'Create subaoi.gdb in aoi if it doesn't exist; This is where we store the subaoi layers
            Dim success As BA_ReturnCode = BA_ReturnCode.UnknownError
            If Not BA_Folder_ExistsWindowsIO(subAoiPath) Then
                Dim gdbName As String = BA_GetBareName(subAoiPath)
                success = BA_CreateFileGdb(TxtAoiPath.Text, gdbName)
            Else
                success = BA_ReturnCode.Success
            End If
            If success = BA_ReturnCode.Success Then
                'The list of the grid values for the combined subAOI layer; In same order as subAoiList
                Dim subAoiTable As Hashtable = New Hashtable
                Dim fileBase As String = "subTemp"
                Dim fileCombine As String = "combTemp"
                Dim maskFolder As String = BA_GeodatabasePath(TxtAoiPath.Text, GeodatabaseNames.Aoi)
                Dim i As Integer = 1
                For Each pRow As DataGridViewRow In GrdSubAoi.Rows
                    Dim includeSub As Boolean = pRow.Cells(1).Value
                    If includeSub = True Then
                        progressDialog2.Description = "Replacing NoData cells in subAOI layers"
                        pStepProg.Step()
                        Dim displayName As String = pRow.Cells(0).Value
                        Dim pSubAoi As SubAOI = New SubAOI(displayName)
                        pSubAoi.TempLayerName = fileBase & i
                        subAoiTable(displayName) = pSubAoi
                        Dim aoiGdbPath As String = BA_GeodatabasePath(displayName, GeodatabaseNames.Aoi)
                        Dim inputFolder As String = TxtAoiPath.Text & "\" & aoiGdbPath
                        'Delete raster if it exists before creating new one
                        If BA_File_Exists(subAoiPath & "\" & fileBase & i, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                            BA_RemoveRasterFromGDB(subAoiPath, fileBase & i)
                        End If
                        'Query and set max accumulation value on subAoi object
                        Dim accumValue As Integer = BA_QueryRasterFromPoint(inputFolder, BA_EnumDescription(MapsFileName.PourPoint), _
                                                                       BA_GeodatabasePath(TxtAoiPath.Text, GeodatabaseNames.Surfaces),
                                                                       BA_EnumDescription(MapsFileName.flow_accumulation_gdb))
                        pSubAoi.MaxAccumValue = accumValue
                        Dim gaugeNumber As String = BA_QueryStringFieldFromVector(inputFolder, BA_EnumDescription(MapsFileName.PourPoint), _
                                                                                  BA_FIELD_AWDB_ID)
                        pSubAoi.GaugeNumber = gaugeNumber
                        subAoiTable(displayName) = pSubAoi
                        success = BA_ReplaceNoDataCellsGDB(inputFolder, BA_GetBareName(BA_EnumDescription(PublicPath.AoiGrid)), _
                                                           subAoiPath, pSubAoi.TempLayerName, -1, maskFolder, _
                                                           BA_EnumDescription(AOIClipFile.AOIExtentCoverage))
                        If success = BA_ReturnCode.Success Then
                            pSubAoi.TempFilePath = subAoiPath & "\" & pSubAoi.TempLayerName
                            subAoiTable(displayName) = pSubAoi
                        End If
                        i += 1
                    End If
                Next
                If success = BA_ReturnCode.Success Then
                    progressDialog2.Description = "Calculating subAOI ID values"
                    pStepProg.Step()
                    BA_CalculateSubAoiId(subAoiTable)
                    progressDialog2.Description = "Generating combined subAOI layer"
                    pStepProg.Step()
                    Dim maskFilePath As String = maskFolder & "\" & BA_EnumDescription(AOIClipFile.AOIExtentCoverage)
                    Dim combineList As IList(Of String) = New List(Of String)
                    For Each pKey As String In subAoiTable.Keys
                        Dim sAoi As SubAOI = subAoiTable(pKey)
                        combineList.Add(sAoi.TempFilePath)
                    Next
                    success = BA_ZoneOverlay(maskFilePath, combineList, subAoiPath, fileCombine, _
                                             False, True, maskFilePath, WorkspaceType.Geodatabase)
                    If success = BA_ReturnCode.Success Then
                        progressDialog2.Description = "Appending attributes to subAOI layer"
                        pStepProg.Step()
                        'Delete temporary SubAOI layers
                        For Each pKey As String In subAoiTable.Keys
                            Dim sAoi As SubAOI = subAoiTable(pKey)
                            BA_RemoveRasterFromGDB(subAoiPath, sAoi.TempLayerName)
                        Next
                        pGeodataset = BA_OpenRasterFromGDB(subAoiPath, fileCombine)
                        If pGeodataset IsNot Nothing Then
                            pRasterBandCollection = CType(pGeodataset, IRasterBandCollection)
                            pRasterBand = pRasterBandCollection.Item(0)
                            pTable = pRasterBand.AttributeTable
                            Dim idxValue As Integer = pTable.FindField(BA_FIELD_VALUE)
                            Dim sortedArray(subAoiTable.Keys.Count - 1) As SubAOI
                            Dim j As Integer = 0
                            For Each sName As String In subAoiTable.Keys
                                Dim sAoi As SubAOI = subAoiTable(sName)
                                sortedArray(j) = sAoi
                                j += 1
                            Next
                            'Sort the subAOI's so we get the lowest ID's first
                            System.Array.Sort(sortedArray, SubAOI.maxAccumAscending)
                            'Keep track of which combine codes have been assigned to a SubAOI
                            Dim usedCombineCodes As IList(Of Integer) = New List(Of Integer)
                            'Loop through the sorted sub AOI's
                            For k As Integer = 0 To sortedArray.GetUpperBound(0)
                                'Get all the rows assigned to that SubAOI
                                Dim sAoi As SubAOI = sortedArray(k)
                                pQF.WhereClause = """" & sAoi.TempLayerName & """ = 1"
                                Dim pCursor As ICursor = pTable.Search(pQF, False)
                                Dim pRow As IRow = pCursor.NextRow
                                Dim combineValList As IList(Of Integer) = New List(Of Integer)
                                Do Until pRow Is Nothing
                                    Dim pValue As Integer = pRow.Value(idxValue)
                                    'If that combine code hasn't been assigned to an upstream subAOI
                                    'Assign it to this one
                                    If Not usedCombineCodes.Contains(pValue) Then
                                        combineValList.Add(pValue)
                                    End If
                                    'Save the collection of Combine codes to the subAoi
                                    usedCombineCodes.Add(pValue)
                                    pRow = pCursor.NextRow
                                Loop
                                sAoi.CombineValueList = combineValList
                                subAoiTable(sAoi.Name) = sAoi
                                pRow = Nothing
                                pCursor = Nothing
                            Next
                            Dim valuesCursor As ICursor = Nothing
                            Dim pDataStatistics As IDataStatistics = New DataStatistics
                            valuesCursor = pTable.Search(Nothing, False)
                            'initialize properties for the dataStatistics interface
                            pDataStatistics.Field = BA_FIELD_VALUE
                            pDataStatistics.Cursor = valuesCursor
                            Dim valuesEnum As IEnumerator = pDataStatistics.UniqueValues
                            Dim whereClause As String = ""
                            valuesEnum.MoveNext()
                            Dim nextValue As Integer = Convert.ToInt32(valuesEnum.Current)
                            Do While nextValue > 0
                                If usedCombineCodes.Contains(nextValue) Then
                                    'Do nothing; the Value is used
                                Else
                                    whereClause = " == " & nextValue
                                    Exit Do
                                End If
                                'Debug.WriteLine(nextValue)
                                valuesEnum.MoveNext()
                                nextValue = Convert.ToInt32(valuesEnum.Current)
                            Loop
                            valuesCursor = Nothing
                            pDataStatistics = Nothing
                            valuesEnum = Nothing
                            'Recode noData value to noData
                            If Not String.IsNullOrEmpty(whereClause) Then
                                'Delete raster if it exists before creating new one
                                'If BA_File_Exists(subAoiPath & "\" & outputFile, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                                '    BA_RemoveRasterFromGDB(subAoiPath, outputFile)
                                'End If
                                success = BA_SetNullSelectedCellsGDB(subAoiPath, fileCombine, subAoiPath, outputFile, _
                                                                    maskFolder, BA_EnumDescription(AOIClipFile.AOIExtentCoverage), _
                                                                    whereClause)
                                If success = BA_ReturnCode.Success Then
                                    'Delete temporary combine file
                                    BA_RemoveRasterFromGDB(subAoiPath, fileCombine)
                                    BA_UpdateSubAoiAttributeTable(subAoiPath, outputFile, subAoiTable)
                                End If
                            End If
                        End If
                    End If
                End If
                RefreshGrdId()
            End If

        Catch ex As Exception
            Debug.Print("BtnCreateSubAoi_Click Exception: " & ex.Message)
        Finally
            If pStepProg IsNot Nothing Then
                pStepProg.Hide()
                pStepProg = Nothing
                progressDialog2.HideDialog()
                progressDialog2 = Nothing
            End If
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try

    End Sub

    Private Sub BtnSelectAoi_Click(sender As System.Object, e As System.EventArgs) Handles BtnSelectAoi.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim DataPath As String
        Dim pFilter As IGxObjectFilter = New GxFilterContainers
        Dim bagisPExt As BagisPExtension = BagisPExtension.GetExtension

        Try
            'TestSort()
            'initialize and open mini browser
            With pGxDialog
                .AllowMultiSelect = False
                .ButtonCaption = "Select"
                .Title = "Select AOI Folder"
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
            Dim success As BA_ReturnCode = BA_CheckAoiStatus(DataPath, My.ArcMap.Application.hWnd, My.ArcMap.Document)
            If success = BA_ReturnCode.Success Then
                BA_SetDefaultProjection(My.ArcMap.Application)
                Dim aoiName As String = BA_GetBareName(DataPath)
                Dim pAoi As Aoi = New Aoi(aoiName, DataPath, Nothing, bagisPExt.version)
                TxtAoiPath.Text = pAoi.FilePath
                'ResetForm()
                Me.Text = "Manage SubAOI Id Layers (AOI: " & aoiName & pAoi.ApplicationVersion & " )"
                bagisPExt.aoi = pAoi

                RefreshSubAoiGrid()
                RefreshGrdId()
                'Bring the window to the front
                Me.WindowState = FormWindowState.Normal
                Me.BringToFront()
            End If
        Catch ex As Exception
            MessageBox.Show("BtnSelectAoi_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub RefreshGrdId()
        GrdId.Rows.Clear()
        Dim subAoiGdbPath As String = TxtAoiPath.Text & BA_EnumDescription(PublicPath.BagisSubAoiGdb)
        If BA_Folder_ExistsWindowsIO(subAoiGdbPath) Then
            Dim rasterList As String() = Nothing
            Dim vectorList As String() = Nothing
            BA_ListLayersinGDB(subAoiGdbPath, rasterList, vectorList)
            'rasterList has an empty first position as a holdover from VBA
            For i As Integer = 1 To rasterList.GetUpperBound(0)
                '---create a row---
                Dim item As New DataGridViewRow
                item.CreateCells(GrdId)
                item.Cells(0).Value = rasterList(i)
                item.Cells(1).Value = False
                GrdId.Rows.Add(item)
            Next
        End If
        'De-select any rows
        If GrdId.Rows.Count > 0 Then
            GrdId.CurrentCell = Nothing
            GrdId.ClearSelection()
            GrdId.Refresh()
        End If
        'Manage display button
        If GrdId.Rows.Count > 0 Then
            BtnDisplayId.Enabled = True
        Else
            BtnDisplayId.Enabled = False
        End If
    End Sub

    Private Sub GrdSubAoi_CellContentClick(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GrdSubAoi.CellContentClick
        Dim idxColumn As Integer = e.ColumnIndex
        'We changed one of the checkboxes
        If idxColumn = 1 Then
            Dim selectedLayers As Integer = 0
            For Each pRow As DataGridViewRow In GrdSubAoi.Rows
                Dim selected As Boolean = Convert.ToBoolean(pRow.Cells(idxColumn).EditedFormattedValue)
                If selected = True Then
                    selectedLayers += 1
                    Exit For
                End If
            Next
            If selectedLayers > 0 Then
                BtnCreateSubAoi.Enabled = True
            Else
                BtnCreateSubAoi.Enabled = False
            End If
        End If
    End Sub

    Private Sub GrdId_CellContentClick(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GrdId.CellContentClick
        Dim idxColumn As Integer = e.ColumnIndex
        'We changed one of the checkboxes
        If idxColumn = 1 Then
            Dim selectedLayers As Integer = 0
            For Each pRow As DataGridViewRow In GrdId.Rows
                Dim selected As Boolean = Convert.ToBoolean(pRow.Cells(idxColumn).EditedFormattedValue)
                If selected = True Then
                    selectedLayers += 1
                    Exit For
                End If
            Next
            If selectedLayers > 0 Then
                BtnDelete.Enabled = True
                BtnRename.Enabled = True
            Else
                BtnDelete.Enabled = False
                BtnRename.Enabled = False
            End If
        End If
    End Sub

    Private Sub BtnDisplayId_Click(sender As Object, e As System.EventArgs) Handles BtnDisplayId.Click
        Dim subAoiPath As String = TxtAoiPath.Text & BA_EnumDescription(PublicPath.BagisSubAoiGdb)
        Dim pTempLayer As ILayer = Nothing
        For Each pRow As DataGridViewRow In GrdId.Rows
            Dim includeSub As Boolean = pRow.Cells(1).Value
            Dim displayName As String = pRow.Cells(0).Value
            If includeSub = True Then
                BA_DisplayRasterWithSymbol(My.Document, subAoiPath & "\" & displayName, _
                                           displayName, MapsDisplayStyle.Cool_Tones, 50, _
                                           WorkspaceType.Geodatabase)
            Else
                'If the layer is not checked, remove it if it is displayed
                Dim pMap As IMap = My.Document.FocusMap
                Dim nlayers As Integer = pMap.LayerCount
                For i = nlayers To 1 Step -1
                    pTempLayer = CType(pMap.Layer(i - 1), ILayer)   'Explicit cast
                    If displayName = pTempLayer.Name Then 'remove the layer
                        pMap.DeleteLayer(pTempLayer)
                    End If
                Next
                pTempLayer = Nothing
            End If
        Next

    End Sub

    Private Sub BtnDelete_Click(sender As System.Object, e As System.EventArgs) Handles BtnDelete.Click
        Dim sb As StringBuilder = New StringBuilder()
        sb.Append("The selected layers will be permanently deleted." & vbCrLf)
        sb.Append("This action cannot be undone. Do you wish to continue ?" & vbCrLf)
        Dim returnValue As DialogResult = MessageBox.Show(sb.ToString, "Delete layer(s)", _
                                                          MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If returnValue = Windows.Forms.DialogResult.Yes Then
            Dim subAoiPath As String = TxtAoiPath.Text & BA_EnumDescription(PublicPath.BagisSubAoiGdb)
            Dim pTempLayer As ILayer = Nothing
            Dim pMap As IMap = My.Document.FocusMap
            Dim nlayers As Integer = pMap.LayerCount
            For Each pRow As DataGridViewRow In GrdId.Rows
                Dim deleteSub As Boolean = pRow.Cells(1).Value
                Dim layerName As String = pRow.Cells(0).Value
                If deleteSub = True Then
                    'Remove layer from map before we try to delete it
                    For i = nlayers To 1 Step -1
                        pTempLayer = CType(pMap.Layer(i - 1), ILayer)   'Explicit cast
                        If layerName = pTempLayer.Name Then 'remove the layer
                            pMap.DeleteLayer(pTempLayer)
                        End If
                    Next
                    pTempLayer = Nothing
                    BA_RemoveRasterFromGDB(subAoiPath, layerName)
                End If
            Next
            RefreshGrdId()
        End If
    End Sub

    Private Sub BtnRename_Click(sender As System.Object, e As System.EventArgs) Handles BtnRename.Click
        Dim subAoiPath As String = TxtAoiPath.Text & BA_EnumDescription(PublicPath.BagisSubAoiGdb)
        Dim pTempLayer As ILayer = Nothing
        Dim pMap As IMap = My.Document.FocusMap
        Dim nlayers As Integer = pMap.LayerCount
        For Each pRow As DataGridViewRow In GrdId.Rows
            Dim renameSub As Boolean = pRow.Cells(1).Value
            Dim layerName As String = pRow.Cells(0).Value
            If renameSub = True Then
                'Remove layer from map before we try to rename it
                Dim layerRemoved As Boolean = False
                For i = nlayers To 1 Step -1
                    pTempLayer = CType(pMap.Layer(i - 1), ILayer)   'Explicit cast
                    If layerName = pTempLayer.Name Then 'remove the layer
                        pMap.DeleteLayer(pTempLayer)
                        layerRemoved = True
                    End If
                Next
                pTempLayer = Nothing
                ' Set prompt.
                Dim message As String = "Enter a new name for the " & layerName & " layer:"
                ' Set title.
                Dim title As String = "Rename SubAOI Id Layer"
                ' Display message, title, and default value.
                Dim outputFile As String = InputBox(message, title, "", 150, 150)
                ' If user has clicked Cancel, set myValue to defaultValue 
                If String.IsNullOrEmpty(outputFile) Then
                    MessageBox.Show("You did not enter a new name. This layer will not be renamed.", title, _
                                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                Else
                    If BA_File_Exists(subAoiPath & "\" & outputFile, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                        Dim returnValue As DialogResult = MessageBox.Show("A file exists with that name. Do you wish to overwrite it?" & vbCrLf & "This action cannot be undone.", title, _
                                                                          MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                        If returnValue <> Windows.Forms.DialogResult.Yes Then
                            Exit Sub
                        Else
                            BA_RemoveRasterFromGDB(subAoiPath, outputFile)
                        End If
                    End If
                End If
                BA_RenameRasterInGDB(subAoiPath, layerName, outputFile)
                If layerRemoved Then
                    BA_DisplayRasterWithSymbol(My.Document, subAoiPath & "\" & outputFile, _
                           outputFile, MapsDisplayStyle.Cool_Tones, 50, _
                           WorkspaceType.Geodatabase)
                End If
            End If
        Next
        RefreshGrdId()

    End Sub
End Class