Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports BAGIS_ClassLibrary
Imports System.Windows.Forms
Imports System.IO
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesGDB
Imports System.ComponentModel
Imports ESRI.ArcGIS.GeoprocessingUI
Imports ESRI.ArcGIS.Geoprocessing
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.ArcMapUI
Imports System.Drawing
Imports System.Text

Public Class FrmParameterViewer

    Dim m_aoi As Aoi
    Dim m_profileTable As Hashtable
    Dim m_methodTable As Hashtable
    'key: field name, value: associated method
    Dim m_fieldTable As Hashtable
    'Parallel arrays maintain image resource names and their corresponding styles
    Dim m_resImageArray(6) As Image
    Dim m_displayStyle(6) As String

    Private Sub BtnSelectAoi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSelectAoi.Click
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
                m_aoi = New Aoi(aoiName, DataPath, Nothing, bagisPExt.version)
                TxtAoiPath.Text = m_aoi.FilePath
                'ResetForm()
                Me.Text = "Parameter Viewer (AOI: " & aoiName & m_aoi.ApplicationVersion & " )"
                bagisPExt.aoi = m_aoi

                'Load layer lists
                ' Create a DirectoryInfo of the HRU directory.
                Dim zonesDirectory As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, Nothing)
                Dim dirZones As New DirectoryInfo(zonesDirectory)
                Dim dirZonesArr As DirectoryInfo() = Nothing
                If dirZones.Exists Then
                    dirZonesArr = dirZones.GetDirectories
                    LoadHruLayers(dirZonesArr)
                End If

                'Populate profile Hashtable
                RefreshProfileData()
                'Populate Method Hashtable
                RefreshMethodData()

            End If

        Catch ex As Exception
            MessageBox.Show("BtnSelectAoi_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadHruLayers(ByVal dirZonesArr As DirectoryInfo())
        LstHruLayers.Items.Clear()
        If dirZonesArr IsNot Nothing Then
            Dim item As LayerListItem
            For Each dri In dirZonesArr
                Dim hruFilePath As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, dri.Name) & BA_EnumDescription(PublicPath.HruGrid)
                Dim hruXmlFilePath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, dri.Name) & BA_EnumDescription(PublicPath.HruXml)
                ' Add hru to the list if the grid exists
                If BA_File_Exists(hruFilePath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) And _
                   BA_File_ExistsWindowsIO(hruXmlFilePath) Then
                    'Assume hru is discrete raster since we create it to be so
                    item = New LayerListItem(dri.Name, hruFilePath, LayerType.Raster, True)
                    LstHruLayers.Items.Add(item)
                End If
            Next dri
        End If
    End Sub

    Private Sub LoadProfileList(ByVal hruName As String)
        LstProfiles.Items.Clear()
        Dim hruPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, hruName)
        Dim hruParamPath As String = hruPath & BA_EnumDescription(PublicPath.BagisParamGdb)
        If BA_Folder_ExistsWindowsIO(hruParamPath) Then
            Dim tNames As IList(Of String) = BA_ListTablesInGDB(hruParamPath)
            If tNames IsNot Nothing Then
                For Each pName As String In tNames
                    LstProfiles.Items.Add(pName)
                Next
            End If
        End If

    End Sub

    Private Sub LstHruLayers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstHruLayers.SelectedIndexChanged
        Dim selItem As LayerListItem = TryCast(LstHruLayers.SelectedItem, LayerListItem)
        If selItem IsNot Nothing Then
            LoadProfileList(selItem.Name)
        End If
    End Sub

    Private Sub LstProfiles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstProfiles.SelectedIndexChanged
        Dim paramTable As ITable = Nothing
        Dim pCursor As ICursor = Nothing
        Dim pRow As IRow = Nothing
        Dim pFields As IFields = Nothing
        Dim selItem As LayerListItem = TryCast(LstHruLayers.SelectedItem, LayerListItem)

        Try
            GrdParam.Visible = False
            GrdParam.Rows.Clear()
            'Remove all columns except for the OMS_ID
            For i As Integer = GrdParam.Columns.Count - 1 To 0 Step -1
                Dim pCol As DataGridViewColumn = GrdParam.Columns(i)
                If pCol.Name.ToUpper <> BA_FIELD_HRU_ID Then
                    GrdParam.Columns.Remove(pCol)
                End If
            Next
            'Clear out any previous error messages
            LblError.Text = Nothing
            Dim hruPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, selItem.Name)
            Dim hruParamPath As String = hruPath & BA_EnumDescription(PublicPath.BagisParamGdb)
            If BA_Folder_ExistsWindowsIO(hruParamPath) Then
                Dim selProfile As String = TryCast(LstProfiles.SelectedItem, String)
                If selProfile IsNot Nothing Then
                    Dim tableName As String = selProfile & BA_PARAM_TABLE_SUFFIX
                    paramTable = BA_OpenTableFromGDB(hruParamPath, tableName)
                    If paramTable IsNot Nothing Then
                        pFields = paramTable.Fields
                        'Dim idxERamsId As Integer = paramTable.FindField(BA_FIELD_ERAMS_ID)
                        Dim idxHruId As Integer = paramTable.FindField(BA_FIELD_HRU_ID)
                        AddColumnsToTable(pFields)
                        pCursor = paramTable.Search(Nothing, False)
                        pRow = pCursor.NextRow
                        While pRow IsNot Nothing
                            '---create a row---
                            Dim item As New DataGridViewRow
                            item.CreateCells(GrdParam)
                            '---populate the HRU_ID ---
                            'Dim eRamsId As Long = CLng(pRow.Value(idxERamsId))
                            Dim hruId As Long = CLng(pRow.Value(idxHruId))
                            'Dim dgvColumn As DataGridViewColumn = GrdParam.Columns(BA_FIELD_ERAMS_ID)
                            Dim dgvColumn As DataGridViewColumn = GrdParam.Columns(BA_FIELD_HRU_ID)
                            'item.Cells(dgvColumn.Index).Value = eRamsId
                            item.Cells(dgvColumn.Index).Value = hruId
                            For j As Integer = 1 To pFields.FieldCount - 1
                                Dim nextField As IField = pRow.Fields.Field(j)
                                If nextField.Type <> esriFieldType.esriFieldTypeOID And nextField.Name <> BA_FIELD_HRU_ID Then
                                    dgvColumn = GrdParam.Columns(nextField.Name)
                                    If pRow.Value(j) IsNot DBNull.Value Then
                                        item.Cells(dgvColumn.Index).Value = CStr(pRow.Value(j))
                                    End If
                                End If
                            Next
                            '---add the row---
                            GrdParam.Rows.Add(item)
                            pRow = pCursor.NextRow
                        End While
                        GrdParam.Sort(GrdParam.Columns(0), ListSortDirection.Ascending)
                        GrdParam.ClearSelection()
                        GrdParam.Visible = True
                    End If
                End If
                RefreshFieldData(selProfile)
                ManageLblError(hruPath, selProfile)
            End If
        Catch ex As Exception
            Debug.Print("LstProfiles_SelectedIndexChanged Exception: " & ex.Message)
        Finally
            paramTable = Nothing
            pCursor = Nothing
            pRow = Nothing
            pFields = Nothing
        End Try
    End Sub

    Private Sub AddColumnsToTable(ByVal pFields As IFields)
        For i As Integer = 0 To pFields.FieldCount - 1
            Dim pField As Field = pFields.Field(i)
            Dim fieldName As String = pField.Name
            If pField.Type <> esriFieldType.esriFieldTypeOID And fieldName <> BA_FIELD_HRU_ID Then
                Dim newColumn As DataGridViewTextBoxColumn = New DataGridViewTextBoxColumn()
                newColumn.Name = fieldName
                newColumn.HeaderText = fieldName
                newColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
                Dim newColumnStyle As DataGridViewCellStyle = New DataGridViewCellStyle
                newColumnStyle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
                newColumn.DefaultCellStyle = newColumnStyle
                newColumn.ReadOnly = True
                newColumn.Width = 85
                newColumn.SortMode = DataGridViewColumnSortMode.Programmatic
                GrdParam.Columns.Add(newColumn)
            End If
        Next

    End Sub

    'Loads the profile data from disk
    Private Sub RefreshProfileData()
        Dim profilesFolder As String = Nothing
        If m_aoi IsNot Nothing Then
            profilesFolder = BA_GetLocalProfilesDir(m_aoi.FilePath)
        End If
        If Not String.IsNullOrEmpty(profilesFolder) Then
            Dim profileList As List(Of Profile) = BA_LoadProfilesFromXml(profilesFolder)
            If profileList IsNot Nothing Then
                m_profileTable = New Hashtable
                For Each nextProfile In profileList
                    m_profileTable.Add(nextProfile.Name, nextProfile)
                Next
            End If
        End If
    End Sub

    'Loads the method data from disk
    Private Sub RefreshMethodData()
        Dim methodsFolder As String = Nothing
        If m_aoi IsNot Nothing Then
            methodsFolder = BA_GetLocalMethodsDir(m_aoi.FilePath)
        End If
        If Not String.IsNullOrEmpty(methodsFolder) Then
            Dim methodList As List(Of Method) = BA_LoadMethodsFromXml(methodsFolder)
            If methodList IsNot Nothing Then
                m_methodTable = New Hashtable
                For Each nextMethod In methodList
                    If m_aoi IsNot Nothing Then
                        'Set method status for local profile builder
                        nextMethod.Status = MethodStatus.Pending
                    End If
                    m_methodTable.Add(nextMethod.Name, nextMethod)
                Next
            End If
        End If
    End Sub

    Private Sub RefreshFieldData(ByVal profileName As String)
        m_fieldTable = New Hashtable
        Dim selProfile As Profile = m_profileTable(profileName)
        If selProfile IsNot Nothing AndAlso selProfile.MethodNames IsNot Nothing Then
            For Each mName As String In selProfile.MethodNames
                Dim selMethod As Method = m_methodTable(mName)
                If selMethod IsNot Nothing AndAlso selMethod.ModelParameters IsNot Nothing Then
                    For Each selParam As ModelParameter In selMethod.ModelParameters
                        Dim paramName As String = selParam.Name
                        If paramName.Substring(0, BA_FIELD_PREFIX.Length).ToLower = BA_FIELD_PREFIX Then
                            m_fieldTable(selParam.Value) = selMethod
                        End If
                    Next
                End If
            Next
        End If
    End Sub

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    'Override default sort behavior; Cannot use default sort behavior when selection mode is set to FullColumnSelect
    Private Sub GrdParam_ColumnHeaderMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles GrdParam.ColumnHeaderMouseClick
        Dim newColumn As DataGridViewColumn = GrdParam.Columns(e.ColumnIndex)
        Dim oldColumn As DataGridViewColumn = GrdParam.SortedColumn
        Dim direction As ListSortDirection

        ' If oldColumn is null, then the DataGridView is not currently sorted.
        If oldColumn IsNot Nothing Then

            ' Sort the same column again, reversing the SortOrder.
            If oldColumn Is newColumn AndAlso GrdParam.SortOrder = SortOrder.Ascending Then
                direction = ListSortDirection.Descending
            Else

                ' Sort a new column and remove the old SortGlyph.
                direction = ListSortDirection.Ascending
                oldColumn.HeaderCell.SortGlyphDirection = SortOrder.None
            End If
        Else
            direction = ListSortDirection.Ascending
        End If

        ' Sort the selected column.
        GrdParam.Sort(newColumn, direction)
        If direction = ListSortDirection.Ascending Then
            newColumn.HeaderCell.SortGlyphDirection = SortOrder.Ascending
        Else
            newColumn.HeaderCell.SortGlyphDirection = SortOrder.Descending
        End If

    End Sub

    Private Sub GrdParam_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GrdParam.SelectionChanged
        Dim columnColl As DataGridViewSelectedColumnCollection = GrdParam.SelectedColumns
        If columnColl.Count > 0 AndAlso columnColl(0).Name <> BA_FIELD_HRU_ID Then
            BtnEditModel.Enabled = True
            BtnViewValues.Enabled = True
        Else
            BtnEditModel.Enabled = False
            BtnViewValues.Enabled = False
        End If
    End Sub

    Private Sub BtnEditModel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnEditModel.Click
        Dim arcToolBoxExtension As IArcToolboxExtension = Nothing
        Dim arcToolBox As IArcToolbox = Nothing
        Dim pTool As IGPTool = Nothing
        Dim columnColl As DataGridViewSelectedColumnCollection = GrdParam.SelectedColumns
        If columnColl.Count > 0 Then
            'Always take the first one; VB .NET should prevent you from selecting > 1
            Dim fldName As String = columnColl(0).Name
            Dim selMethod As Method = m_fieldTable(fldName)
            Try
                arcToolBoxExtension = My.ArcMap.Application.FindExtensionByName("ESRI ArcToolbox")
                If arcToolBoxExtension IsNot Nothing Then
                    arcToolBox = arcToolBoxExtension.ArcToolbox
                    If arcToolBox IsNot Nothing Then
                        'pTool = arcToolBox.GetToolbyNameString(TxtModelName.Text)
                        pTool = BA_OpenModel(selMethod.ToolBoxPath, selMethod.ToolboxName, selMethod.ModelName)
                        If pTool IsNot Nothing Then
                            arcToolBox.EditToolSource(pTool)
                        End If
                    End If
                End If
            Catch ex As Exception
                Debug.Print("BtnEditModel_Click() Exception: " & ex.Message)
                MessageBox.Show("Unable to open the selected model")
            Finally
                pTool = Nothing
                arcToolBoxExtension = Nothing
                arcToolBox = Nothing
                GC.WaitForPendingFinalizers()
                GC.Collect()
            End Try
        End If
    End Sub

    Private Sub BtnViewValues_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnViewValues.Click
        Dim selItem As LayerListItem = TryCast(LstHruLayers.SelectedItem, LayerListItem)
        Dim selProfile As String = TryCast(LstProfiles.SelectedItem, String)
        Dim hruPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, selItem.Name)
        Dim pfLayer As IFeatureLayer = Nothing
        Dim paramTable As ITable = Nothing
        Dim pColorRamp As IColorRamp = Nothing
        Dim pGFLayer As IGeoFeatureLayer = Nothing
        Dim pDisplayTable As IDisplayTable = Nothing
        Dim displayCursor As ICursor = Nothing
        Dim pDataStatistics As IDataStatistics = New DataStatistics
        Dim statisticsResults As IStatisticsResults = Nothing

        Try
            Dim columnColl As DataGridViewSelectedColumnCollection = GrdParam.SelectedColumns
            If columnColl.Count > 0 Then
                'Always take the first one; VB .NET should prevent you from selecting > 1
                Dim fldName As String = columnColl(0).Name
                Dim displayName As String = selItem.Name & " " & fldName

                'Derive the file path for the HRU vector to be displayed
                Dim hruGdbName As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, selItem.Name)
                Dim vName As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruZonesVector), False)
                'Add the vector to the map but don't refresh yet; It won't show on the display
                Dim buffer_factor As Double = 1.25
                Dim success As BA_ReturnCode = BA_DisplayVectorNoRefresh(My.Document, hruGdbName, vName, displayName, 0, buffer_factor)

                If success = BA_ReturnCode.Success Then
                    'Get a handle to the newly added vector
                    Dim pMap As IMap = My.Document.FocusMap
                    For i As Integer = 0 To pMap.LayerCount - 1
                        Dim nextLayer As ILayer2 = pMap.Layer(i)
                        If TypeOf nextLayer Is IFeatureLayer Then
                            Dim fLayer As IFeatureLayer = CType(nextLayer, IFeatureLayer)
                            If fLayer.Name = displayName Then
                                pfLayer = fLayer
                                Exit For
                            End If
                        End If
                    Next

                    'Derive the file path for the parameter table to be joined
                    Dim hruParamPath As String = hruPath & BA_EnumDescription(PublicPath.BagisParamGdb)
                    Dim tableName As String = selProfile & BA_PARAM_TABLE_SUFFIX
                    'Open the parameter table
                    paramTable = BA_OpenTableFromGDB(hruParamPath, tableName)
                    'Join the feature class to the table
                    Dim vNameJoinField As String = BA_FIELD_HRU_ID
                    Dim tableJoinField As String = BA_FIELD_HRU_ID
                    success = BA_JoinFeatureClassToTable(pfLayer, vName, vNameJoinField, paramTable, tableJoinField)

                    If success = BA_ReturnCode.Success Then
                        Dim displayField As String = tableName & "." & fldName
                        'Access the displayTable through the IGeoFeatureLayer
                        pGFLayer = CType(pfLayer, IGeoFeatureLayer) 'Explicit cast
                        pDisplayTable = CType(pGFLayer, IDisplayTable)  'Explicit cast

                        'Get a table cursor
                        displayCursor = pDisplayTable.SearchDisplayTable(Nothing, False)
                        'initialize properties for the dataStatistics interface
                        pDataStatistics.Field = displayField
                        pDataStatistics.Cursor = displayCursor
                        'Get the result statistics
                        statisticsResults = pDataStatistics.Statistics
                        Dim recordCount As Integer = statisticsResults.Count
                        'Out of memory error if number of classes exceeds 250 when calling the Classify method in BA_DisplayVectorWithSymbol
                        'GDuh says to set maximum # of records at 10
                        If recordCount > 10 Then
                            recordCount = 10
                        End If
                        Dim displayStyle As String = m_displayStyle(CboColors.SelectedIndex)
                        BA_DisplayVectorWithSymbol(My.Document, pfLayer, displayField, recordCount, displayName, displayStyle, 0, buffer_factor)
                        'Remove any existing annotation
                        'First remove any existing labels
                        For i As Integer = 0 To pMap.LayerCount - 1
                            Dim nextLayer As ILayer2 = pMap.Layer(i)
                            If TypeOf nextLayer Is IFeatureLayer Then
                                Dim fLayer As IFeatureLayer = CType(nextLayer, IFeatureLayer)
                                BA_RemoveAnnotation(fLayer)
                            End If
                        Next
                        'Next annotate the layer if desired
                        Dim pColor As IRgbColor = New RgbColor
                        pColor.RGB = RGB(0, 0, 0)   'Black
                        BA_AnnotateLayer(pfLayer, displayField, pColor, Nothing, CkDisplayLabels.Checked)
                        'refresh the activated view
                        My.Document.ActivatedView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)
                        My.Document.UpdateContents()
                    End If
                End If
            End If
        Catch ex As Exception
            Debug.Print("BtnViewValues_Click Exception: " & ex.Message)
        Finally
            pfLayer = Nothing
            paramTable = Nothing
            pColorRamp = Nothing
            pGFLayer = Nothing
            pDisplayTable = Nothing
            displayCursor = Nothing
            pDataStatistics = Nothing
            statisticsResults = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub

    Private Sub CkDisplayLabels_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CkDisplayLabels.CheckedChanged
        'First remove any existing labels
        Dim pMap As IMap = My.Document.FocusMap
        For i As Integer = 0 To pMap.LayerCount - 1
            Dim nextLayer As ILayer2 = pMap.Layer(i)
            If TypeOf nextLayer Is IFeatureLayer Then
                Dim fLayer As IFeatureLayer = CType(nextLayer, IFeatureLayer)
                BA_RemoveAnnotation(nextLayer)
            End If
        Next

        Dim columnColl As DataGridViewSelectedColumnCollection = GrdParam.SelectedColumns
        If columnColl.Count > 0 Then
            'Always take the first one; VB .NET should prevent you from selecting > 1
            Dim fldName As String = columnColl(0).Name
            Dim selProfile As String = TryCast(LstProfiles.SelectedItem, String)
            Dim tableName As String = selProfile & BA_PARAM_TABLE_SUFFIX
            Dim displayField As String = tableName & "." & fldName
            Dim selItem As LayerListItem = TryCast(LstHruLayers.SelectedItem, LayerListItem)
            Dim displayName As String = selItem.Name & " " & fldName

            'Get a handle to the correct vector by display name
            Dim pfLayer As IFeatureLayer = Nothing
            For i As Integer = 0 To pMap.LayerCount - 1
                Dim nextLayer As ILayer2 = pMap.Layer(i)
                If TypeOf nextLayer Is IFeatureLayer Then
                    Dim fLayer As IFeatureLayer = CType(nextLayer, IFeatureLayer)
                    If fLayer.Name = displayName Then
                        pfLayer = fLayer
                        Exit For
                    End If
                End If
            Next

            If pfLayer IsNot Nothing Then
                Dim pColor As IRgbColor = New RgbColor
                pColor.RGB = RGB(0, 0, 0)   'Black
                BA_AnnotateLayer(pfLayer, displayField, pColor, Nothing, CkDisplayLabels.Checked)
            End If
        End If
        'refresh the activated view
        My.Document.ActivatedView.PartialRefresh(esriViewDrawPhase.esriViewGeography, Nothing, Nothing)

    End Sub

    Private Sub FrmParameterViewer_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        'Load up arrays of resource images and their corresponding style types
        m_resImageArray(0) = My.Resources.black2white
        m_displayStyle(0) = "Black to White"
        m_resImageArray(1) = My.Resources.aspect
        m_displayStyle(1) = "ASPECT"
        m_resImageArray(2) = My.Resources.basic_random
        m_displayStyle(2) = "Range/Random"
        m_resImageArray(3) = My.Resources.elevation2
        m_displayStyle(3) = "ELEVATION"
        m_resImageArray(4) = My.Resources.precipitation
        m_displayStyle(4) = "PRECIPITATION"
        m_resImageArray(5) = My.Resources.slope
        m_displayStyle(5) = "SLOPE"

        ' Set the ImageSize property to a larger size  
        ' (the default is 16 x 16).
        Dim pImage As System.Drawing.Image = m_resImageArray(0)
        ImgListColors.ImageSize = pImage.Size
        For j As Int32 = 0 To m_resImageArray.GetUpperBound(0)
            Dim nextImage As Image = m_resImageArray(j)
            If nextImage IsNot Nothing Then
                ImgListColors.Images.Add(nextImage)
            End If
        Next

        Dim items(Me.ImgListColors.Images.Count - 1) As String
        For i As Int32 = 0 To Me.ImgListColors.Images.Count - 1
            items(i) = "Item " & i.ToString
        Next

        Me.CboColors.Items.AddRange(items)
        Me.CboColors.DropDownStyle = ComboBoxStyle.DropDownList
        Me.CboColors.DrawMode = DrawMode.OwnerDrawVariable
        Me.CboColors.ItemHeight = Me.ImgListColors.ImageSize.Height
        Me.CboColors.Width = Me.ImgListColors.ImageSize.Width + 18
        Me.CboColors.MaxDropDownItems = Me.ImgListColors.Images.Count
        CboColors.SelectedIndex = 0
    End Sub

    Private Sub CboColors_DrawItem(sender As Object, e As System.Windows.Forms.DrawItemEventArgs) Handles CboColors.DrawItem
        If e.Index <> -1 Then
            e.Graphics.DrawImage(Me.ImgListColors.Images(e.Index) _
                                 , e.Bounds.Left, e.Bounds.Top)
        End If
    End Sub

    Private Sub CboColors_MeasureItem(sender As Object, e As System.Windows.Forms.MeasureItemEventArgs) Handles CboColors.MeasureItem
        e.ItemHeight = Me.ImgListColors.ImageSize.Height
        e.ItemWidth = Me.ImgListColors.ImageSize.Width
    End Sub

    Private Sub ManageLblError(ByVal folderPath As String, ByVal selProfile As String)
        If Not String.IsNullOrEmpty(folderPath) AndAlso Not String.IsNullOrEmpty(selProfile) Then
            Dim fileName As String = TryCast(LstProfiles.SelectedItem, String) & "_params_log.xml"
            Try
                Dim logProfile As LogProfile = BA_LoadLogProfileFromXml(folderPath & "\" & fileName)
                If logProfile IsNot Nothing Then
                    Dim errorCount As Integer
                    Dim arrMethods As Method() = logProfile.HruMethods
                    For i As Integer = 0 To arrMethods.GetUpperBound(0)
                        Dim nextMethod As Method = arrMethods(i)
                        If nextMethod.Status = MethodStatus.Complete Or _
                            nextMethod.Status = MethodStatus.Pending Or _
                            nextMethod.Status = MethodStatus.Verified Then
                            'Do nothing
                        Else
                            errorCount += 1
                        End If
                    Next
                    If errorCount > 0 Then
                        'Note:1 method failed 
                        'when this profile was 
                        'executed. Click here to 
                        'view the log.
                        Dim sb As StringBuilder = New StringBuilder
                        If errorCount = 1 Then
                            sb.Append("Note:1 method failed" & vbCrLf)
                        Else
                            sb.Append("Note:" & errorCount & " methods failed" & vbCrLf)
                        End If
                        sb.Append("when this profile was" & vbCrLf)
                        sb.Append("executed. Click here to" & vbCrLf)
                        sb.Append("view the log.")
                        LblError.Text = sb.ToString
                        Dim idxLink As Int16 = LblError.Text.IndexOf("here")
                        Dim lArea As LinkArea = New LinkArea(idxLink, 4)
                        LblError.LinkArea = lArea
                    End If
                End If
            Catch ex As Exception
                Debug.Print("ManageLblError Exception: " & ex.Message)
            End Try
        End If

    End Sub

    Private Sub LblError_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LblError.LinkClicked
        Dim pExt As BagisPExtension = BagisPExtension.GetExtension
        Dim selItem As LayerListItem = TryCast(LstHruLayers.SelectedItem, LayerListItem)
        Dim folderPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, selItem.Name)
        Dim fileName As String = TryCast(LstProfiles.SelectedItem, String) & "_params_log.xml"
        Dim logProfile As LogProfile = BA_LoadLogProfileFromXml(folderPath & "\" & fileName)
        If logProfile IsNot Nothing Then
            If logProfile.Version <> pExt.version Then
                MessageBox.Show("This parameter log was created using an older version of BAGIS-P. It may not display correctly", "Old version", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            Dim fileNameArr As String() = fileName.Split(".")
            Dim filePath As String = BA_StandardizePathString(folderPath, True) & fileNameArr(0) & ".html"
            BA_WriteParameterFile(logProfile, filePath, True)
            Process.Start(filePath)
        Else
            MessageBox.Show("An error occurred while trying to read the selected log file. It cannot be displayed.", "Unknown error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub
End Class