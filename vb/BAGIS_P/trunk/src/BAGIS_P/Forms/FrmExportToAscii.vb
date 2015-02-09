Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports BAGIS_ClassLibrary
Imports System.IO
Imports System.Windows.Forms
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.GeoAnalyst
Imports Microsoft.VisualBasic.FileIO

Public Class FrmExportToAscii

    Dim m_aoi As Aoi
    Dim m_profileList As IList(Of Profile)

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add items to CboResampleHru
        CboResampleHru.Items.Add(BA_Resample_Majority)
        CboResampleHru.Items.Add(BA_Resample_Nearest)
        CboResampleHru.SelectedItem = BA_Resample_Majority
        ' Add items to CboResampleDem
        CboResampleDem.Items.Add(BA_Resample_Bilinear)
        CboResampleDem.Items.Add(BA_Resample_Nearest)
        CboResampleDem.Items.Add(BA_Resample_Cubic)
        CboResampleDem.SelectedItem = BA_Resample_Bilinear
    End Sub

    Private Sub BtnSelectAoi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSelectAoi.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim DataPath As String
        Dim pFilter As IGxObjectFilter = New GxFilterContainers
        Dim bagisPExt As BagisPExtension = BagisPExtension.GetExtension

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
                m_aoi = New Aoi(aoiName, DataPath, Nothing, bagisPExt.version)
                TxtAoiPath.Text = m_aoi.FilePath
                'ResetForm()
                Me.Text = "Export To ASCII (AOI: " & aoiName & m_aoi.ApplicationVersion & " )"
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

                'Cache profiles so we have access to their descriptions
                'For now we only load this when the aoi changes
                Dim profilesFolder As String = BA_GetLocalProfilesDir(DataPath)
                m_profileList = BA_LoadProfilesFromXml(profilesFolder)

                Dim surfacesFolder As String = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Surfaces)
                Dim cellSize As Double = BA_CellSize(surfacesFolder, BA_EnumDescription(MapsFileName.filled_dem_gdb))
                Dim linearUnit As ESRI.ArcGIS.Geometry.ILinearUnit = BA_GetLinearUnitOfProjectedRaster(surfacesFolder, BA_EnumDescription(MapsFileName.filled_dem_gdb))
                Dim unitLabel As String = "Unknown"
                If linearUnit.Name = "Meter" Then
                    unitLabel = "Meters"
                ElseIf linearUnit.Name = "Foot" Then
                    unitLabel = "Feet"
                End If
                TxtDemOutputName.Text = BA_EnumDescription(MapsFileName.filled_dem_gdb) & BA_FILE_EXT_TEXT
                If linearUnit IsNot Nothing Then
                    TxtDemResolution.Text = Math.Round(cellSize, 2) & " " & unitLabel
                    TxtDemUnits.Text = unitLabel
                Else
                    TxtDemResolution.Text = Math.Round(cellSize, 2)
                End If
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

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    Private Sub LstHruLayers_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles LstHruLayers.SelectedIndexChanged
        If LstHruLayers.SelectedIndex > -1 Then
            'Derive the file path for the HRU vector to be displayed
            Dim selItem As LayerListItem = TryCast(LstHruLayers.SelectedItem, LayerListItem)
            Dim hruGdbName As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, selItem.Name)
            Dim vName As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruZonesVector), False)
            Dim hruCount As Integer = BA_CountPolygons(hruGdbName, vName)
            If hruCount > 0 Then
                TxtNHru.Text = CStr(hruCount)
            Else
                TxtNHru.Text = "0"
            End If
            TxtOutputFolder1.Text = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, selItem.Name)
            TxtHruOutputName.Text = selItem.Name & BA_FILE_EXT_TEXT

            Dim hruGdbPath As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, selItem.Name)
            Dim cellSize As Double = BA_CellSize(hruGdbPath, GRID)
            Dim linearUnit As ESRI.ArcGIS.Geometry.ILinearUnit = BA_GetLinearUnitOfProjectedRaster(hruGdbPath, GRID)
            Dim unitLabel As String = "Unknown"
            If linearUnit.Name = "Meter" Then
                unitLabel = "Meters"
            ElseIf linearUnit.Name = "Foot" Then
                unitLabel = "Feet"
            End If
            If linearUnit IsNot Nothing Then
                TxtHruResolution.Text = Math.Round(cellSize, 2) & " " & unitLabel
                TxtHruUnits.Text = unitLabel
            Else
                TxtHruResolution.Text = Math.Round(cellSize, 2)
            End If
            ManageExportButton()
        End If
    End Sub

    Private Sub BtnSetOutput_Click(sender As System.Object, e As System.EventArgs) Handles BtnSetOutput.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim DataPath As String
        Dim pFilter As IGxObjectFilter = New GxFilterContainers

        Try
            'initialize and open mini browser
            With pGxDialog
                .AllowMultiSelect = False
                .ButtonCaption = "Select"
                .Title = "Select output folder"
                .ObjectFilter = pFilter
                bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
            End With

            If bObjectSelected = False Then Exit Sub

            'get the name of the selected folder
            Dim pGxDataFolder As IGxFile
            pGxDataFolder = pGxObject.Next
            DataPath = pGxDataFolder.Path
            If String.IsNullOrEmpty(DataPath) Then Exit Sub 'user cancelled the action
            TxtOutputFolder1.Text = DataPath
        Catch ex As Exception
            Debug.Print("BtnSetOutput_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub ManageExportButton()
        If String.IsNullOrEmpty(TxtHruOutputName.Text) Or String.IsNullOrEmpty(TxtOutputFolder1.Text) _
            Or String.IsNullOrEmpty(TxtDemOutputName.Text) Then
            BtnExport.Enabled = False
        Else
            BtnExport.Enabled = True
        End If
    End Sub

    Private Sub BtnExport_Click(sender As System.Object, e As System.EventArgs) Handles BtnExport.Click
        Dim selItem As LayerListItem = CType(LstHruLayers.SelectedItem, LayerListItem)  'Selected HRU
        Dim hruGdbName As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, selItem.Name)
        Dim outputFolder As String = Trim(TxtOutputFolder1.Text)
        Dim outputFile As String = BA_GetBareName(TxtHruOutputName.Text)
        Dim transformOp As ITransformationOp = New RasterTransformationOp
        Dim exportOp As IRasterExportOp = New RasterConversionOp
        Dim hruDataSet As IGeoDataset = Nothing
        Dim inputDataSet As IGeoDataset = Nothing
        Dim demDataSet As IGeoDataset = Nothing
        Dim hruCellSize As Double = 0
        Dim demCellSize As Double = 0
        Try
            Dim hruFileName As String = Trim(TxtHruOutputName.Text)
            If BA_File_ExistsWindowsIO(outputFolder & "\" & hruFileName) Then
                Dim res As DialogResult = MessageBox.Show("The HRU ASCII file name you specified already exists. Do you want to overwrite it ?", "File exists", _
                                                          MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If res <> Windows.Forms.DialogResult.Yes Then
                    TxtHruOutputName.Focus()
                    Exit Sub
                End If
                BA_Remove_File(outputFolder & "\" & hruFileName)
            End If
            Dim demFileName As String = Trim(TxtDemOutputName.Text)
            If BA_File_ExistsWindowsIO(outputFolder & "\" & demFileName) Then
                Dim res As DialogResult = MessageBox.Show("The DEM ASCII file name you specified already exists. Do you want to overwrite it ?", "File exists", _
                                                          MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If res <> Windows.Forms.DialogResult.Yes Then
                    TxtDemOutputName.Focus()
                    Exit Sub
                End If
                BA_Remove_File(outputFolder & "\" & demFileName)
            End If
            BtnSelectAoi.Focus()
            BtnExport.Enabled = False

            If Not String.IsNullOrEmpty(TxtHruResample.Text) Then
                hruCellSize = CDbl(TxtHruResample.Text)
            End If
            Dim tempFileName As String = "tmpResample"
            If hruCellSize > 0 Then
                'Need to resample
                'inputDataSet = BA_OpenRasterFromGDB(hruGdbName, GRID)
                Dim outputRaster As String = hruGdbName & "\" & tempFileName
                Dim snapRasterPath As String = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Aoi, True) & BA_EnumDescription(AOIClipFile.AOIExtentCoverage)
                Dim success As BA_ReturnCode = BA_Resample_Raster(hruGdbName & "\" & GRID, outputRaster, hruCellSize, snapRasterPath, CboResampleHru.SelectedItem.ToString)
                If success = BA_ReturnCode.Success Then
                    hruDataSet = BA_OpenRasterFromGDB(hruGdbName, tempFileName)
                Else
                    Throw New System.Exception("Unable to resample hru raster dataset.")
                End If
            Else
                'Open the dataset directly
                hruDataSet = BA_OpenRasterFromGDB(hruGdbName, GRID)
            End If
            If hruDataSet IsNot Nothing Then
                exportOp.ExportToASCII(hruDataSet, outputFolder & "\" & hruFileName)
            End If
            'Delete the temporary hru dataset if it exists
            If BA_File_Exists(hruGdbName & "\" & tempFileName, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                BA_RemoveRasterFromGDB(hruGdbName, tempFileName)
            End If

            If Not String.IsNullOrEmpty(TxtDemResample.Text) Then
                demCellSize = CDbl(TxtDemResample.Text)
            End If
            Dim surfacesFolder As String = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Surfaces)
            If demCellSize > 0 Then
                'Need to resample
                inputDataSet = BA_OpenRasterFromGDB(surfacesFolder, BA_EnumDescription(MapsFileName.filled_dem_gdb))
                'Get resample method from form
                Dim resampleEnum As esriGeoAnalysisResampleEnum = GetResampleEnum(CboResampleDem.SelectedItem.ToString)
                demDataSet = transformOp.Resample(inputDataSet, demCellSize, resampleEnum)
            Else
                'Open the dataset directly
                demDataSet = BA_OpenRasterFromGDB(surfacesFolder, BA_EnumDescription(MapsFileName.filled_dem_gdb))
            End If
            inputDataSet = Nothing
            If demDataSet IsNot Nothing Then
                exportOp.ExportToASCII(demDataSet, outputFolder & "\" & demFileName)
            End If

            BtnExport.Enabled = True
            MessageBox.Show("Files successfully exported!", "File export", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            Debug.Print("BtnExport_Click Exception: " & ex.Message)
            MessageBox.Show("An error occurred while trying to export the ASCII information", _
                "File export", MessageBoxButtons.OK, MessageBoxIcon.Information)
            BtnExport.Enabled = True
        Finally
            hruDataSet = Nothing
            demDataSet = Nothing
            transformOp = Nothing
            exportOp = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try

    End Sub

    Private Sub TxtOutputFolder1_TextChanged(sender As Object, e As System.EventArgs) Handles TxtOutputFolder1.TextChanged
        ManageExportButton()
    End Sub

    Private Sub TxtHruOutputName_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles TxtHruOutputName.Validating
        If Not IsValidFileName(TxtHruOutputName.Text) Then
            ' Cancel the event and select the text to be corrected by the user.
            e.Cancel = True
            TxtHruOutputName.Select(0, TxtHruOutputName.Text.Length)
            MessageBox.Show("Please enter a valid file name without spaces or special characters", "Invalid file name", MessageBoxButtons.OK, _
                            MessageBoxIcon.Information)
        Else
            Dim extension As String = Path.GetExtension(TxtHruOutputName.Text)
            If extension <> BA_FILE_EXT_TEXT Then
                TxtHruOutputName.Text = TxtHruOutputName.Text & BA_FILE_EXT_TEXT
            End If
            ManageExportButton()
        End If
    End Sub

    Private Sub TxtDemOutputName_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles TxtDemOutputName.Validating
        If Not IsValidFileName(TxtDemOutputName.Text) Then
            ' Cancel the event and select the text to be corrected by the user.
            e.Cancel = True
            TxtDemOutputName.Select(0, TxtDemOutputName.Text.Length)
            MessageBox.Show("Please enter a valid file name without spaces or special characters", "Invalid file name", MessageBoxButtons.OK, _
                            MessageBoxIcon.Information)
        Else
            Dim extension As String = Path.GetExtension(TxtDemOutputName.Text)
            If extension <> BA_FILE_EXT_TEXT Then
                TxtDemOutputName.Text = TxtDemOutputName.Text & BA_FILE_EXT_TEXT
            End If
            ManageExportButton()
        End If
    End Sub

    Private Function IsValidFileName(ByVal fn As String) As Boolean
        Try
            'Don't allow spaces
            If fn.Contains(" ") Then
                Return False
            End If
            Dim fi As New IO.FileInfo(fn)
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Private Function GetResampleEnum(ByVal txtResample As String) As esriGeoAnalysisResampleEnum
        Select Case txtResample
            Case BA_Resample_Nearest
                Return esriGeoAnalysisResampleEnum.esriGeoAnalysisResampleNearest
            Case BA_Resample_Cubic
                Return esriGeoAnalysisResampleEnum.esriGeoAnalysisResampleCubic
            Case Else
                'Per design, bilinear is the default for DEM
                Return esriGeoAnalysisResampleEnum.esriGeoAnalysisResampleBilinear
        End Select
    End Function

    Private Sub TxtDemResample_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TxtDemResample.Validating
        If Not String.IsNullOrEmpty(TxtDemResample.Text) Then
            If Not IsNumeric(TxtDemResample.Text) Then
                ' Cancel the event and select the text to be corrected by the user.
                e.Cancel = True
                TxtDemResample.Select(0, TxtDemResample.Text.Length)
                MessageBox.Show("Please enter a number without spaces or special characters to resample", "Invalid value", MessageBoxButtons.OK, _
                                MessageBoxIcon.Information)
            ElseIf CInt(TxtDemResample.Text) < 1 Then
                ' Cancel the event and select the text to be corrected by the user.
                e.Cancel = True
                TxtDemResample.Select(0, TxtDemResample.Text.Length)
                MessageBox.Show("Please enter a number without spaces or special characters to resample", "Invalid value", MessageBoxButtons.OK, _
                                MessageBoxIcon.Information)
            End If
        End If
    End Sub

    Private Sub TxtHruResample_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TxtHruResample.Validating
        If Not String.IsNullOrEmpty(TxtHruResample.Text) Then
            If Not IsNumeric(TxtHruResample.Text) Then
                ' Cancel the event and select the text to be corrected by the user.
                e.Cancel = True
                TxtHruResample.Select(0, TxtHruResample.Text.Length)
                MessageBox.Show("Please enter a number without spaces or special characters to resample", "Invalid value", MessageBoxButtons.OK, _
                                MessageBoxIcon.Information)
            ElseIf CInt(TxtHruResample.Text) < 1 Then
                ' Cancel the event and select the text to be corrected by the user.
                e.Cancel = True
                TxtHruResample.Select(0, TxtHruResample.Text.Length)
                MessageBox.Show("Please enter a number without spaces or special characters to resample", "Invalid value", MessageBoxButtons.OK, _
                                MessageBoxIcon.Information)
            End If
        End If
    End Sub
End Class