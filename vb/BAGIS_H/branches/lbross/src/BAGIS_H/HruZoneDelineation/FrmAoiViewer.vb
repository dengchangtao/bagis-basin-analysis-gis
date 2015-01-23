Imports System.Windows.Forms
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.esriSystem
Imports System.IO
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Desktop.AddIns
Imports ESRI.ArcGIS.Geodatabase
Imports BAGIS_ClassLibrary

Public Class FrmAoiViewer
    Dim m_aoi As Aoi
    Dim m_version As String

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        BtnViewAoi.Enabled = False
        BtnImport.Enabled = False
        BtnClearSelected.Enabled = False

        ' Set AOI if already set
        Dim hruExt As HruExtension = HruExtension.GetExtension
        Dim aoi As Aoi = hruExt.aoi
        If aoi IsNot Nothing Then
            m_aoi = aoi
            TxtAoiPath.Text = m_aoi.FilePath

            LoadLstLayers()
            BtnImport.Enabled = True
            SetDatumInExtension()
        End If
    End Sub

    Private Sub BtnAOI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAOI.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim DataPath As String
        Dim pFilter As IGxObjectFilter = New GxFilterContainers

        Dim hruExt As HruExtension = HruExtension.GetExtension
        m_version = hruExt.version

        Try
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
                m_aoi = New Aoi(aoiName, DataPath, Nothing, m_version)
                TxtAoiPath.Text = m_aoi.FilePath
                LoadLstLayers()

                'Set current aoi in comboBox on Toolbar
                Dim comboBox = AddIn.FromID(Of CboSelectedAoi)(My.ThisAddIn.IDs.CboSelectedAoi)
                If comboBox IsNot Nothing Then
                    comboBox.setValue(aoiName)
                End If
                hruExt.aoi = m_aoi
                BtnImport.Enabled = True
                SetDatumInExtension()
                Show()
            End If
        Catch ex As Exception
            MessageBox.Show("BtnAOI_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadLstLayers()
        Dim ShapefileCount As Long, RasterCount As Long
        Dim i As Long

        ' Create/configure a step progressor
        Dim stepCount As Short = 6  'Loading 4 dropdowns
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, stepCount)
        pStepProg.Show()
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Loading layer lists", "Loading...")

        Try
            LstAoiVectorLayers.Items.Clear()
            LstAoiRasterLayers.Items.Clear()
            LstDemLayers.Items.Clear()
            LstPrismLayers.Items.Clear()
            progressDialog2.ShowDialog()
            pStepProg.Step()
            Dim AOIVectorList() As String = Nothing
            Dim AOIRasterList() As String = Nothing
            Dim layerPath As String = m_aoi.FilePath & "\" & BA_EnumDescription(GeodatabaseNames.Layers)
            BA_ListLayersinGDB(layerPath, AOIRasterList, AOIVectorList)
            pStepProg.Step()

            'display shapefiles
            ShapefileCount = UBound(AOIVectorList)
            If ShapefileCount > 0 Then
                For i = 1 To ShapefileCount
                    ' Vectors are always discrete
                    Dim isDiscrete As Boolean = True
                    Dim fullLayerPath As String = layerPath & "\" & AOIVectorList(i)
                    Dim item As LayerListItem = New LayerListItem(AOIVectorList(i), fullLayerPath, LayerType.Vector, isDiscrete)
                    LstAoiVectorLayers.Items.Add(item)
                Next
            End If

            pStepProg.Step()

            'display raster layers
            RasterCount = UBound(AOIRasterList)
            If RasterCount > 0 Then
                For i = 1 To RasterCount
                    Dim fullLayerPath As String = layerPath & "\" & AOIRasterList(i)
                    Dim isDiscrete As Boolean = BA_IsIntegerRasterGDB(fullLayerPath)
                    Dim item As LayerListItem = New LayerListItem(AOIRasterList(i), fullLayerPath, LayerType.Raster, isDiscrete)
                    LstAoiRasterLayers.Items.Add(item)
                Next
            End If
            pStepProg.Step()

            'display dem layers
            LoadDemLayers()
            pStepProg.Step()

            'display prism layers
            LoadPrismLayers()
            pStepProg.Step()

        Catch ex As Exception
            MessageBox.Show("LoadLstLayers() Exception: " & ex.Message)
        Finally
            ' Clean up step progressor
            pStepProg.Hide()
            pStepProg = Nothing
            progressDialog2.HideDialog()
            progressDialog2 = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub

    Private Sub LoadDemLayers()
        Dim layerPath As String = m_aoi.FilePath & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces)
        Dim item As LayerListItem
        ' Filled DEM
        Dim fullLayerPath As String = layerPath & "\" & BA_EnumDescription(MapsFileName.filled_dem_gdb)
        Dim isDiscrete As Boolean = BA_IsIntegerRaster(fullLayerPath)
        If BA_File_Exists(fullLayerPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            item = New LayerListItem(BA_EnumDescription(MapsLayerName.filled_dem), fullLayerPath, LayerType.Raster, isDiscrete)
            LstDemLayers.Items.Add(item)
        End If
        ' Original DEM
        Dim demFileName As String = BA_GetBareName(BA_EnumDescription(PublicPath.SourceDEM))
        fullLayerPath = layerPath & "\" & demFileName
        isDiscrete = BA_IsIntegerRaster(fullLayerPath)
        If BA_File_Exists(fullLayerPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            item = New LayerListItem(BA_EnumDescription(MapsLayerName.dem), fullLayerPath, LayerType.Raster, isDiscrete)
            LstDemLayers.Items.Add(item)
        End If
        ' Flow Direction
        fullLayerPath = layerPath & "\" & BA_EnumDescription(MapsFileName.flow_direction_gdb)
        isDiscrete = BA_IsIntegerRaster(fullLayerPath)
        If BA_File_Exists(fullLayerPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            item = New LayerListItem(BA_EnumDescription(MapsLayerName.flow_direction_gdb), fullLayerPath, LayerType.Raster, isDiscrete)
            LstDemLayers.Items.Add(item)
        End If
        ' Flow Accumulation
        fullLayerPath = layerPath & "\" & BA_EnumDescription(MapsFileName.flow_accumulation_gdb)
        isDiscrete = BA_IsIntegerRaster(fullLayerPath)
        If BA_File_Exists(fullLayerPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            item = New LayerListItem(BA_EnumDescription(MapsLayerName.flow_accumulation_gdb), fullLayerPath, LayerType.Raster, isDiscrete)
            LstDemLayers.Items.Add(item)
        End If
        ' Slope
        fullLayerPath = layerPath & "\" & BA_EnumDescription(MapsLayerName.slope)
        isDiscrete = BA_IsIntegerRaster(fullLayerPath)
        If BA_File_Exists(fullLayerPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            item = New LayerListItem(BA_EnumDescription(MapsLayerName.slope), fullLayerPath, LayerType.Raster, isDiscrete)
            LstDemLayers.Items.Add(item)
        End If
        ' Aspect
        fullLayerPath = layerPath & "\" & BA_EnumDescription(MapsLayerName.aspect)
        isDiscrete = BA_IsIntegerRaster(fullLayerPath)
        If BA_File_Exists(fullLayerPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            item = New LayerListItem(BA_EnumDescription(MapsLayerName.aspect), fullLayerPath, LayerType.Raster, isDiscrete)
            LstDemLayers.Items.Add(item)
        End If
        ' Hillshade
        fullLayerPath = layerPath & "\" & BA_EnumDescription(MapsLayerName.hillshade)
        isDiscrete = BA_IsIntegerRaster(fullLayerPath)
        If BA_File_Exists(fullLayerPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            item = New LayerListItem(BA_EnumDescription(MapsLayerName.hillshade), fullLayerPath, LayerType.Raster, isDiscrete)
            LstDemLayers.Items.Add(item)
        End If

    End Sub

    Private Sub LoadPrismLayers()
        Dim item As LayerListItem
        Dim strPath As String = m_aoi.FilePath & "\" & BA_EnumDescription(GeodatabaseNames.Prism)
        Dim layerCount As Short = [Enum].GetValues(GetType(AOIPrismFolderNames)).Length
        Dim i = 1
        Do Until i > layerCount
            Dim nextFolder As String = BA_GetPRISMFolderName(i)
            Dim prismPath = strPath & "\" & nextFolder
            'Dim isDiscrete As Boolean = BA_IsIntegerRaster(prismPath)
            'For performance, we assume that prism layers are not discrete
            If BA_File_Exists(prismPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                item = New LayerListItem(nextFolder, prismPath, LayerType.Raster, False)
                LstPrismLayers.Items.Add(item)
            End If
            i += 1
        Loop
    End Sub

    Private Sub BtnViewAoi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnViewAoi.Click
        Try
            Dim fileNamesWithStyle As List(Of String) = BA_ListOfLayerNamesWithStyles()
            ' Display raster layers
            If LstAoiRasterLayers.SelectedIndex > -1 Then
                Dim items As IList = LstAoiRasterLayers.SelectedItems
                For Each item As LayerListItem In items
                    If fileNamesWithStyle.IndexOf(item.Name) > -1 Then
                        Dim symbology As BA_Map_Symbology = BA_GetRasterMapSymbology(item.Name)
                        BA_DisplayRasterWithSymbol(My.ArcMap.Document, item.Value, symbology.DisplayName, _
                                                   symbology.DisplayStyle, symbology.Transparency, WorkspaceType.Geodatabase)
                    Else
                        BA_DisplayRaster(My.ArcMap.Application, item.Value)
                    End If
                Next
            End If
            ' Display vector layers
            If LstAoiVectorLayers.SelectedIndex > -1 Then
                Dim items As IList = LstAoiVectorLayers.SelectedItems
                For Each item As LayerListItem In items
                    Dim strFileName As String = BA_GetBareName(item.Value)
                    If fileNamesWithStyle.IndexOf(strFileName) > -1 Then
                        Dim symbology As BA_Map_Symbology = BA_GetPointMapSymbology(strFileName)
                        BA_MapDisplayPointMarkers(My.ArcMap.Application, item.Value, symbology.DisplayName, symbology.Color, symbology.MarkerType)
                    Else
                        BA_DisplayVector(My.ArcMap.Document, item.Value)
                    End If
                Next
            End If
            ' Display DEM layers
            If LstDemLayers.SelectedIndex > -1 Then
                Dim items As IList = LstDemLayers.SelectedItems
                For Each item As LayerListItem In items
                    If item.LayerType = LayerType.Raster Then
                        BA_DisplayRaster(My.ArcMap.Application, item.Value, item.Name)
                    Else
                        'Need special processing here for the pourpoint layer
                        'We assume this is a point layer because all vector styles are points at this time
                        'Dim strFileName As String = BA_GetBareName(item.Value)
                        'If fileNamesWithStyle.IndexOf(strFileName) > -1 Then
                        '    Dim symbology As BA_Map_Symbology = BA_GetPointMapSymbology(strFileName)
                        '    BA_MapDisplayPointMarkers(My.ArcMap.Application, item.Value, symbology.DisplayName, symbology.Color, symbology.MarkerType)
                        'Else
                        '    BA_DisplayVector(My.ArcMap.Document, item.Value)
                        'End If
                        MessageBox.Show("Unable to display layer '" & item.Name & "'. Only raster layers are valid DEM layers", "Invalid data type", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                Next
            End If
            'Prism layers
            If LstPrismLayers.SelectedIndex > -1 Then
                Dim items As IList = LstPrismLayers.SelectedItems
                For Each item As LayerListItem In items
                    BA_DisplayRaster(My.ArcMap.Application, item.Value, item.Name)
                Next
            End If

        Catch ex As Exception
            MessageBox.Show("An error occurred while trying to view one or more of the layers you requested.", "Error", MessageBoxButtons.OK)
            Debug.Print("AOI path: " & m_aoi.FilePath)
            Debug.Print("AOI path length: " & m_aoi.FilePath.Length)
            Debug.Print("BtnViewAoi_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub BtnClearSelected_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClearSelected.Click
        LstAoiVectorLayers.SelectedIndex = -1
        LstAoiRasterLayers.SelectedIndex = -1
        LstPrismLayers.SelectedIndex = -1
        LstDemLayers.SelectedIndex = -1
    End Sub

    Private Sub ManageAoiLayerButtons()
        Dim lCount As Integer = LstAoiRasterLayers.SelectedItems.Count + _
                                LstDemLayers.SelectedItems.Count + _
                                LstPrismLayers.SelectedItems.Count + _
                                LstAoiVectorLayers.SelectedItems.Count
        If lCount > 0 Then
            BtnViewAoi.Enabled = True
            BtnClearSelected.Enabled = True
        Else
            BtnViewAoi.Enabled = False
            BtnClearSelected.Enabled = False
        End If
    End Sub

    Private Sub LstAoiRasterLayers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstAoiRasterLayers.SelectedIndexChanged
        ManageAoiLayerButtons()
    End Sub

    Private Sub LstAoiVectorLayers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstAoiVectorLayers.SelectedIndexChanged
        ManageAoiLayerButtons()
    End Sub

    Private Sub LstDemLayers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstDemLayers.SelectedIndexChanged
        ManageAoiLayerButtons()
    End Sub

    Private Sub LstPrismLayers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstPrismLayers.SelectedIndexChanged
        ManageAoiLayerButtons()
    End Sub

    Private Sub BtnImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnImport.Click

        Dim pFilter As IGxObjectFilter = New GxFilterDatasets
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim bObjectSelected As Boolean
        Dim importDone As Boolean = False

        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select a GIS dataset to add to AOI"
            .ObjectFilter = pFilter
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With
        If bObjectSelected = False Then Exit Sub

        'get the name of the selected folder
        Dim pGxDataset As IGxDataset = pGxObject.Next
        Dim pDatasetName As IDatasetName = pGxDataset.DatasetName
        Dim Data_Path As String = pDatasetName.WorkspaceName.PathName
        Dim Data_Name As String = pDatasetName.Name
        Dim data_type As Object = pDatasetName.Type
        Dim data_type_code As Integer '1. shapefile, 2. Raster, 0. Unsupported format

        'Set Data Type Name from Data Type
        Select Case data_type
            Case 4, 5 'shapefile
                data_type_code = 1

            Case 12, 13 'raster
                data_type_code = 2

            Case Else 'unsupported format
                data_type_code = 0
        End Select

        'pad a backslash to the path if it doesn't have one.
        If Data_Path(Len(Data_Path) - 1) <> "\" Then Data_Path = Data_Path & "\"
        Dim data_fullname As String = Data_Path & Data_Name
        If Len(Trim(data_fullname)) = 0 Then Exit Sub 'user cancelled the action

        'allow user to specify a different output name
        Dim outlayername As String = InputBox("Set output layer name (please don't use any space in the name):", "Clip Layer to AOI", Data_Name)
        If Len(Trim(outlayername)) = 0 Then Exit Sub

        ' Create/configure a step progressor
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 4)
        Dim progressDialog2 As IProgressDialog2 = Nothing
        If data_type_code = 1 Then
            progressDialog2 = BA_GetProgressDialog(pStepProg, "Importing the vector file ", "Importing...")
        ElseIf data_type_code = 2 Then
            progressDialog2 = BA_GetProgressDialog(pStepProg, "Importing the raster file ", "Importing...")
        End If
        pStepProg.Show()
        progressDialog2.ShowDialog()
        pStepProg.Step()
        Try
            'check if a layer is in the correct projection
            Dim validDatum As Boolean
            Dim hruExt As HruExtension = HruExtension.GetExtension
            If data_type_code = 1 Then 'shapefile
                validDatum = BA_VectorDatumMatch(data_fullname, hruExt.Datum)
            ElseIf data_type_code = 2 Then
                validDatum = BA_DatumMatch(data_fullname, hruExt.Datum)
            End If
            If validDatum = False Then
                MessageBox.Show("The selected layer '" & Data_Name & "' cannot be imported because the datum does not match the AOI DEM. Please reproject to " & hruExt.SpatialReference & " and try again.", "Invalid datum", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            'check if a layer of the same name exist
            Dim Layer_Exist As Boolean = False
            If data_type_code = 1 Then 'shapefile
                For idx As Integer = 0 To LstAoiVectorLayers.Items.Count - 1
                    If LstAoiVectorLayers.Items(idx).name = outlayername Then
                        Layer_Exist = True
                        Exit For
                    End If
                Next
            ElseIf data_type_code = 2 Then
                For idx As Integer = 0 To LstAoiRasterLayers.Items.Count - 1
                    If LstAoiRasterLayers.Items(idx).name = outlayername Then
                        Layer_Exist = True
                        Exit For
                    End If
                Next
            Else
                MsgBox("The data type of " & Data_Name & " is not supported. No layer was added to the AOI.")
                Exit Sub
            End If

            If Layer_Exist Then
                MsgBox(outlayername & " already exists in the AOI! The action is aborted.")
                Exit Sub
            End If
            pStepProg.Step()
            'confirm the selection
            'response = MsgBox("Clip " & data_name & " to the AOI?" & vbCrLf & "Output: " & outlayername, vbYesNo)
            'If response = vbNo Then Exit Sub

            'prepare for data clipping
            Dim response As Integer
            Dim outputFolder As String = m_aoi.FilePath & "/" & BA_EnumDescription(GeodatabaseNames.Layers)
            If data_type_code = 1 Then 'clip shapefile
                response = BA_ClipAOIVector(m_aoi.FilePath, data_fullname, outlayername, _
                                            outputFolder, True)
                If response <= 0 Then
                    MsgBox("Import failed! Layer is out of range of AOI.")
                    Exit Sub
                End If
                importDone = True
                pStepProg.Step()
                'Add vector to the list for display
                Dim fullLayerPath As String = outputFolder & "\" & outlayername
                Dim item As LayerListItem = New LayerListItem(outlayername, fullLayerPath, LayerType.Vector, True)
                LstAoiVectorLayers.Items.Add(item)
            ElseIf data_type_code = 2 Then 'raster clip

                Dim clipKey As AOIClipFile = 1
                response = BA_ClipAOIRaster(m_aoi.FilePath, data_fullname, outlayername, outputFolder, clipKey)
                If response <= 0 Then
                    MsgBox("Import Failed! Layer is out of range of AOI.")
                    Exit Sub
                End If
                importDone = True
                pStepProg.Step()
                'Add Raster to the list for display
                Dim fullLayerPath As String = outputFolder & "\" & outlayername
                Dim isDiscrete As Boolean = BA_IsIntegerRaster(fullLayerPath)
                Dim item As LayerListItem = New LayerListItem(outlayername, fullLayerPath, LayerType.Raster, isDiscrete)
                LstAoiRasterLayers.Items.Add(item)
            End If

            'Get handle to UI (form) to reload user layer lists
            Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of frmHruZone.AddinImpl)(My.ThisAddIn.IDs.frmHruZone)
            If dockWindowAddIn IsNot Nothing Then
                Dim hruZoneForm As frmHruZone = dockWindowAddIn.UI
                If hruZoneForm IsNot Nothing Then
                    hruZoneForm.BA_ReloadUserLayers()
                End If
            End If

        Catch ex As Exception
            MessageBox.Show("BtnImport_Click Exception: " & ex.Message)
        Finally
            pStepProg.Hide()
            pStepProg = Nothing
            progressDialog2.HideDialog()
            progressDialog2 = Nothing
            If importDone Then MessageBox.Show("Import is completed.")
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFilter)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGxDialog)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGxObject)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGxDataset)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pDatasetName)
        End Try

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    ' Sets the datum string from the source DEM in the hru extension
    Private Sub SetDatumInExtension()
        Dim hruExt As HruExtension = HruExtension.GetExtension
        Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(m_aoi.FilePath)
        Dim parentPath As String = m_aoi.FilePath & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces)
        Dim pGeoDataSet As IGeoDataset = BA_OpenRasterFromGDB(parentPath, BA_EnumDescription(MapsFileName.filled_dem_gdb))
        If pGeoDataSet IsNot Nothing Then
            'Spatial reference for the dataset in question
            Dim pSpRef As ESRI.ArcGIS.Geometry.ISpatialReference = pGeoDataSet.SpatialReference
            If pSpRef IsNot Nothing Then
                hruExt.Datum = BA_DatumString(pSpRef)
                hruExt.SpatialReference = pSpRef.Name
            End If
            pSpRef = Nothing
        End If
        pGeoDataSet = Nothing
        GC.WaitForPendingFinalizers()
        GC.Collect()
    End Sub

End Class