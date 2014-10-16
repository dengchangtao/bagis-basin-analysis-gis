Imports System.IO
Imports System.Windows.Forms
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Desktop.AddIns
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesRaster
Imports BAGIS_ClassLibrary

Public Class FrmCookieCut

    Dim m_aoi As Aoi
    Dim m_hruListItems As List(Of LayerListItem)
    Dim m_rasterItems As List(Of LayerListItem)
    Dim m_cookieCutValues As Long()
    Dim m_mode As String

    Public Sub New(ByVal pMode As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Set AOI if already set
        Dim hruExt As HruExtension = HruExtension.GetExtension
        Dim aoi As Aoi = hruExt.aoi
        If aoi IsNot Nothing Then
            m_aoi = aoi
            Me.Text = "Current AOI: " & m_aoi.Name
            TxtAoiPath.Text = aoi.FilePath
            GrpLayerType.Enabled = True
            RdoHru.Checked = True
            TxtNewHruName.Enabled = True
            LoadHruLayers()
            LoadRasterLayers()
            TxtHruPath.Text = m_aoi.FilePath & BA_EnumDescription(PublicPath.HruDirectory)
        End If

        'Set form mode
        m_mode = pMode
        LblLayer.Text = m_mode & " Layer"
        LblField.Text = m_mode & " Field"
        If m_mode = BA_MODE_COOKIE_CUT Then
            CkBoundary.Visible = True
            CkRetainAttributes.Visible = True
        End If

    End Sub

    Private Sub LoadHruLayers()
        CboCookieCut.Items.Clear()
        ' Get the count of zone directories so we know how many steps to put into the StepProgressor
        ' Create a DirectoryInfo of the HRU directory.
        Dim zonesDirectory As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, Nothing)
        Dim dirZones As New DirectoryInfo(zonesDirectory)
        Dim dirZonesArr As DirectoryInfo() = Nothing
        m_hruListItems = New List(Of LayerListItem)
        If dirZones.Exists Then
            dirZonesArr = dirZones.GetDirectories
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
                        m_hruListItems.Add(item)
                        CboParentHru.Items.Add(item)
                        CboCookieCut.Items.Add(item)
                    End If
                Next dri
                CboParentHru.SelectedIndex = 0
                CboCookieCut.SelectedIndex = 0
            End If
        End If

        'Pop error message if no zones in this aoi
        If CboParentHru.Items.Count < 1 Then
            MessageBox.Show("No HRU datasets have been generated for this AOI. Use the 'Define Zones' tool to create an HRU dataset.", "Missing HRU datasets", MessageBoxButtons.OK, MessageBoxIcon.Information)
            TxtAoiPath.Text = "Function disabled. The selected AOI has no HRU."
        End If

    End Sub

    Private Sub LoadRasterLayers()
        Dim AOIVectorList() As String = Nothing
        Dim AOIRasterList() As String = Nothing
        Dim layerPath As String = m_aoi.FilePath & "\" & BA_EnumDescription(GeodatabaseNames.Layers)
        BA_ListLayersinGDB(layerPath, AOIRasterList, AOIVectorList)
        'display raster layers
        Dim rasterCount As Long = UBound(AOIRasterList)
        m_rasterItems = New List(Of LayerListItem)
        If rasterCount > 0 Then
            For i = 1 To rasterCount
                Dim fullLayerPath As String = layerPath & "\" & AOIRasterList(i)
                Dim isDiscrete As Boolean = BA_IsIntegerRasterGDB(fullLayerPath)
                Dim item As LayerListItem = New LayerListItem(AOIRasterList(i), fullLayerPath, LayerType.Raster, isDiscrete)
                m_rasterItems.Add(item)
            Next
        End If
    End Sub

    Private Sub Reset()
        LstSelectedValues.Items.Clear()
        CboCookieCut.Items.Clear()
        CboParentHru.Items.Clear()
        TxtNewHruName.Text = ""
        RdoRaster.Checked = False
        RdoHru.Checked = True
        For Each pItem In m_hruListItems
            CboCookieCut.Items.Add(pItem)
            CboParentHru.Items.Add(pItem)
        Next
        CboParentHru.SelectedIndex = 0
        CboCookieCut.SelectedIndex = 0
        TxtHruPath.Text = m_aoi.FilePath & BA_EnumDescription(PublicPath.HruDirectory)
        Enable()
    End Sub

    Private Sub BtnSelectAoi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSelectAoi.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim DataPath As String
        Dim pFilter As IGxObjectFilter = New GxFilterContainers
        Dim hruExt As HruExtension = HruExtension.GetExtension

        Try
            ' Reset form fields
            LstSelectedValues.Items.Clear()
            CboCookieCut.Items.Clear()
            CboParentHru.Items.Clear()
            TxtNewHruName.Text = ""

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
                m_aoi = New Aoi(aoiName, DataPath, Nothing, hruExt.version)
                Me.Text = "Current AOI: " & m_aoi.Name
                TxtAoiPath.Text = m_aoi.FilePath
                LoadHruLayers()
                LoadRasterLayers()
                TxtHruPath.Text = m_aoi.FilePath & BA_EnumDescription(PublicPath.HruDirectory)
                GrpLayerType.Enabled = True
                RdoHru.Checked = True
                TxtNewHruName.Enabled = True
            End If
        Catch ex As Exception
            MessageBox.Show("BtnSelectAoi_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub CboCookieCut_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboCookieCut.SelectedIndexChanged
        Dim layerListItem As LayerListItem = TryCast(CboCookieCut.SelectedItem, LayerListItem)
        If layerListItem IsNot Nothing Then
            Dim parentPath = "Nothing"
            Dim layerName = BA_GetBareName(layerListItem.Value, parentPath)
            UpdateCboField(parentPath, layerName)
        End If
    End Sub

    Private Sub CboField_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CboField.SelectedIndexChanged
        Dim queryField As String = TryCast(CboField.SelectedItem, String)
        Dim cookieCutItem As LayerListItem = TryCast(CboCookieCut.SelectedItem, LayerListItem)
        If Not String.IsNullOrEmpty(queryField) AndAlso cookieCutItem IsNot Nothing Then
            LstSelectedValues.Items.Clear()
            LoadZoneLayers(cookieCutItem.Value, queryField)
            BtnToggle.Enabled = True
        Else
            BtnToggle.Enabled = False
        End If
    End Sub

    Private Sub LoadZoneLayers(ByVal fullFilePath As String, ByVal queryField As String)
        m_cookieCutValues = BA_ReadValuesFromRaster(fullFilePath, queryField)
        For i As Integer = 0 To m_cookieCutValues.GetUpperBound(0)
            LstSelectedValues.Items.Add(m_cookieCutValues(i))
            LstSelectedValues.SelectedItems.Add(LstSelectedValues.Items(i))
        Next
    End Sub

    Private Sub TxtNewHruName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtNewHruName.TextChanged
        If TxtNewHruName.Text.Length > 0 Then
            BtnGenerateHru.Enabled = True
        Else
            BtnGenerateHru.Enabled = False
        End If
        TxtHruPath.Text = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, TxtNewHruName.Text)
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub BtnGenerateHru_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGenerateHru.Click
        'Disable form controls
        Me.Disable()

        ' Create/configure a step progressor
        Dim stepCount As Integer = 7
        Dim pStepProg As IStepProgressor = Nothing
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = Nothing

        Try
            Dim hruOutputPath2 As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, TxtNewHruName.Text)
            Dim hruFolderPath As String = BA_HruFolderPathFromGdbString(hruOutputPath2)
            If BA_Workspace_Exists(hruFolderPath) Then
                Dim result As DialogResult = MessageBox.Show("HRU directory already exists. Overwrite existing directory ?", "Folder exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If result = DialogResult.Yes Then
                    Dim success As BA_ReturnCode = BA_DeleteHRU(hruFolderPath, My.Document)
                    If success <> BA_ReturnCode.Success Then
                        MessageBox.Show("Unable to delete HRU '" & TxtNewHruName.Text & "'. Please restart ArcMap and try again", "Unable to delete HRU", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Me.Enable()
                        BtnGenerateHru.Enabled = True
                        Exit Sub
                    End If
                    pStepProg = BA_GetStepProgressor(My.ArcMap.Application.hWnd, stepCount)
                    ' Create/configure the ProgressDialog. This automatically displays the dialog
                    progressDialog2 = BA_GetProgressDialog(pStepProg, "Generating HRUs...", "Generate HRUs")
                    pStepProg.Step()
                Else
                    TxtNewHruName.Focus()
                    Exit Sub
                End If
            Else
                pStepProg = BA_GetStepProgressor(My.ArcMap.Application.hWnd, stepCount)
                ' Create/configure the ProgressDialog. This automatically displays the dialog
                progressDialog2 = BA_GetProgressDialog(pStepProg, "Generating HRUs...", "Generate HRUs")
                pStepProg.Step()
            End If

            BA_CreateHruOutputFolders(m_aoi.FilePath, TxtNewHruName.Text)
            ' Create new file GDB for HRU
            BA_CreateHruOutputGDB(m_aoi.FilePath, TxtNewHruName.Text)
            pStepProg.Step()

            Dim returnVal As BA_ReturnCode = BA_ReturnCode.UnknownError
            Dim maskName As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.AoiVector), False)
            Dim maskFilePath As String = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Aoi, True) & maskName
            Dim snapRasterPath As String = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
            Dim selectValuesArray(0) As Long
            Dim parentHru As Hru = Nothing
            Dim useSelectedZones As Boolean = True
            Dim allowNonContiguous As Boolean
            Dim cookieCutItem As LayerListItem = Nothing
            Dim selectedCollection As ListBox.SelectedObjectCollection = LstSelectedValues.SelectedItems
            Dim selectedValuesList As IList = CType(selectedCollection, IList)   ' explicit cast
            selectValuesArray = BA_ReadSelectedValues(selectedValuesList)
            cookieCutItem = CType(CboCookieCut.SelectedItem, LayerListItem)  'explicit cast
            Dim cookieCutFullPath As String = cookieCutItem.Value
            Dim parentHruItem As LayerListItem = CType(CboParentHru.SelectedItem, LayerListItem)    'explicit cast
            Dim parentFullPath As String = parentHruItem.Value
            Dim queryField As String = TryCast(CboField.SelectedItem, String)
            If Not String.IsNullOrEmpty(queryField) Then
                If m_mode = BA_MODE_STAMP Then
                    returnVal = BA_StampHru(maskFilePath, cookieCutFullPath, m_cookieCutValues, _
                                            selectValuesArray, parentFullPath, hruOutputPath2, GRID, _
                                            snapRasterPath, m_aoi.FilePath)
                ElseIf m_mode = BA_MODE_COOKIE_CUT Then
                    returnVal = BA_CookieCutterHru(maskFilePath, cookieCutFullPath, queryField, selectValuesArray, _
                                                   parentFullPath, hruOutputPath2, GRID, CkBoundary.Checked, _
                                                   snapRasterPath, m_aoi.FilePath)
                End If
            End If
            If returnVal = BA_ReturnCode.Success Then
                Dim hruInputPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, parentHruItem.Name)
                Dim aoi As Aoi = BA_LoadHRUFromXml(hruInputPath)
                For Each pHru In aoi.HruList
                    ' We found the parent hru
                    If String.Compare(pHru.Name, parentHruItem.Name) = 0 Then
                        parentHru = pHru
                    End If
                Next
                Dim cookieCutAllowNonContiguous = False
                'Execute this if using cookie cut hru
                If RdoHru.Checked = True Then
                    Dim cookieCutHru As Hru = Nothing
                    Dim cookieCutInputPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, cookieCutItem.Name)
                    Dim cookieCutAoi As Aoi = BA_LoadHRUFromXml(cookieCutInputPath)
                    For Each pHru In cookieCutAoi.HruList
                        ' We found the cookie cut hru
                        If String.Compare(pHru.Name, cookieCutItem.Name) = 0 Then
                            cookieCutHru = pHru
                            cookieCutAllowNonContiguous = cookieCutHru.AllowNonContiguousHru
                        End If
                    Next
                End If
                If parentHru.AllowNonContiguousHru = True Or cookieCutAllowNonContiguous = True Then
                    allowNonContiguous = True
                End If
            End If
            pStepProg.Step()

            If returnVal = BA_ReturnCode.Success Then
                Dim rInputPath As String = hruOutputPath2 & BA_EnumDescription(PublicPath.HruGrid)
                'Dim vOutputPath As String = hruOutputPath2 & BA_EnumDescription(PublicPath.HruVector)
                Dim vOutputPath As String = BA_StandardizePathString(hruOutputPath2, True) & BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruVector), False)
                Dim vReturnVal As Short = BA_Raster2PolygonShapefileFromPath(rInputPath, vOutputPath, False)
                ' Filled DEM Path
                Dim layerPath As String = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Surfaces, True)
                Dim fullLayerPath As String = layerPath & BA_EnumDescription(MapsFileName.filled_dem_gdb)
                'get raster resolution
                Dim cellSize As Double
                Dim rasterStat As IRasterStatistics = BA_GetRasterStatsGDB(fullLayerPath, cellSize)

                returnVal = BA_ProcessNonContiguousGrids(allowNonContiguous, vOutputPath, _
                                             hruOutputPath2, cellSize, snapRasterPath)

                If returnVal = BA_ReturnCode.Success Then
                    pStepProg.Step()

                    ' Placeholder for rules
                    Dim rules As List(Of BAGIS_ClassLibrary.IRule) = New List(Of BAGIS_ClassLibrary.IRule)
                    Dim pHru As Hru = BA_CreateHru(TxtNewHruName.Text, rInputPath, vOutputPath, Nothing, _
                                                   rules, allowNonContiguous)
                    If m_mode = BA_MODE_COOKIE_CUT Then
                        pHru.RetainSourceAttributes = CkRetainAttributes.Checked
                        If CkRetainAttributes.Checked = True And parentHru IsNot Nothing Then
                            Dim origHru As Hru = parentHru
                            Dim parentPath As String = Nothing
                            Dim ruleFilePath As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, origHru.Name)
                            If origHru IsNot Nothing AndAlso origHru.ParentHru IsNot Nothing Then
                                parentPath = origHru.ParentHru.FilePath & GRID
                            End If
                            BA_AddAttributesToHru(m_aoi.FilePath, hruOutputPath2, ruleFilePath, origHru.RuleList, parentPath)
                            pStepProg.Step()
                        End If
                    End If

                    If parentHru IsNot Nothing Then pHru.ParentHru = parentHru
                    Dim cookieCutPath As String = "PleaseReturn"
                    Dim cookieCutFileName As String = BA_GetBareName(cookieCutFullPath, cookieCutPath)
                    Dim cookieCutProcess As New CookieCutProcess(m_mode, cookieCutPath, cookieCutFileName, queryField, selectValuesArray, _
                                                                 RdoHru.Checked, CkBoundary.Checked)
                    'Set hru name in cookieCutProcess if cookieCut layer is an hru
                    If cookieCutProcess.CookieCutIsHru = True Then
                        cookieCutProcess.CookieCutHruName = cookieCutItem.Name
                    End If
                    pHru.CookieCutProcess = cookieCutProcess
                    pStepProg.Step()

                    Dim pHruList As IList(Of Hru) = New List(Of Hru)
                    pHruList.Add(pHru)
                    m_aoi.HruList = pHruList
                    Dim xmlOutputPath As String = hruFolderPath & BA_EnumDescription(PublicPath.HruXml)
                    'MessageBox.Show("7. Generating XML")
                    m_aoi.Save(xmlOutputPath)
                    progressDialog2.HideDialog()

                    MessageBox.Show("New HRU " & TxtNewHruName.Text & " successfully created", "HRU: " & TxtNewHruName.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Reset()
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("BtnGenerateHru_Click Exception: " & ex.Message)
        Finally
            ' Clean up step progressor
            If progressDialog2 IsNot Nothing Then progressDialog2.HideDialog()
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStepProg)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(progressDialog2)
        End Try

    End Sub

    Private Sub Disable()
        BtnSelectAoi.Enabled = False
        CboParentHru.Enabled = False
        CboCookieCut.Enabled = False
        CboField.Enabled = False
        LstSelectedValues.Enabled = False
        TxtNewHruName.Enabled = False
        BtnCancel.Enabled = False
        BtnGenerateHru.Enabled = False
        GrpLayerType.Enabled = False
    End Sub

    Private Sub Enable()
        BtnSelectAoi.Enabled = True
        CboParentHru.Enabled = True
        CboCookieCut.Enabled = True
        CboField.Enabled = True
        LstSelectedValues.Enabled = True
        TxtNewHruName.Enabled = True
        BtnCancel.Enabled = True
        GrpLayerType.Enabled = True
    End Sub

    Private Sub BtnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAbout.Click
        If m_mode = BA_MODE_STAMP Then
            Dim toolHelpForm As FrmHelp = New FrmHelp(BA_HelpTopics.Stamp)
            toolHelpForm.ShowDialog()
        Else 'cookie-cut
            Dim toolHelpForm As FrmHelp = New FrmHelp(BA_HelpTopics.CookieCut)
            toolHelpForm.ShowDialog()
        End If
    End Sub

    Private Sub RdoRaster_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RdoRaster.Click
        CboCookieCut.Items.Clear()
        ' load only continuous rasters
        For Each item As LayerListItem In m_rasterItems
            If item.IsDiscrete = True Then
                CboCookieCut.Items.Add(item)
            End If
        Next
        If CboCookieCut.Items.Count > 0 Then
            CboCookieCut.SelectedIndex = 0
        End If
    End Sub

    Private Sub RdoHru_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RdoHru.Click
        CboCookieCut.Items.Clear()
        ' load only continuous rasters
        For Each item As LayerListItem In m_hruListItems
                CboCookieCut.Items.Add(item)
        Next
        If CboCookieCut.Items.Count > 0 Then
            CboCookieCut.SelectedIndex = 0
        End If
    End Sub

    'This method updates the CboField list based on the selected layer
    Private Sub UpdateCboField(ByVal layerPath As String, ByVal layerName As String)
        Dim selGeoDataset As IGeoDataset = Nothing
        Dim pRasterBandCollection As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pTable As ITable = Nothing
        Dim pFields As IFields = Nothing
        Dim pField As IField = Nothing
        Try
            CboField.Items.Clear()
            Dim pWorkspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(layerPath)
            If pWorkspaceType = WorkspaceType.Raster Then
                selGeoDataset = BA_OpenRasterFromFile(layerPath, layerName)
            ElseIf pWorkspaceType = WorkspaceType.Geodatabase Then
                selGeoDataset = BA_OpenRasterFromGDB(layerPath, layerName)
            End If
            pRasterBandCollection = CType(selGeoDataset, IRasterBandCollection)
            pRasterBand = pRasterBandCollection.Item(0)

            If pRasterBand IsNot Nothing Then
                pTable = pRasterBand.AttributeTable
                pFields = pTable.Fields
                Dim uBound As Integer = pFields.FieldCount - 1
                For i = 1 To uBound
                    pField = pFields.Field(i)
                    Dim pType As esriFieldType = pField.Type
                    'Only show discrete numeric fields (no decimals)
                    Select Case pType
                        Case esriFieldType.esriFieldTypeInteger
                            CboField.Items.Add(pField.Name)
                        Case esriFieldType.esriFieldTypeSingle
                            CboField.Items.Add(pField.Name)
                        Case esriFieldType.esriFieldTypeSmallInteger
                            CboField.Items.Add(pField.Name)
                    End Select
                    'Set selected vield to VALUE if it's there
                    If pField.Name.ToUpper = BA_FIELD_VALUE.ToUpper Then
                        CboField.SelectedIndex = i - 1
                    End If
                Next
                'Otherwise, set to the first field in the list
                If CboField.SelectedIndex < 0 Then CboField.SelectedIndex = 0
            End If

        Catch ex As Exception
            MessageBox.Show("UpdateCboField Exception: " + ex.Message)
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pField)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFields)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTable)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBand)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBandCollection)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(selGeoDataset)
        End Try

    End Sub

    Private Sub BtnToggle_Click(sender As System.Object, e As System.EventArgs) Handles BtnToggle.Click
        For i = LstSelectedValues.Items.Count - 1 To 0 Step -1
            Dim newValue As Boolean = Not LstSelectedValues.GetSelected(i)
            LstSelectedValues.SetSelected(i, newValue)
        Next i
    End Sub

 End Class