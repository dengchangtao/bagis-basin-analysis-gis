Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports System.Windows.Forms
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework
Imports System.Text
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesRaster

Public Class FrmClipDataSource

    Dim m_dataTable As Hashtable
    Dim m_selDataSources As IList(Of String)
    Const IDX_AOI As Integer = 0
    Const IDX_NAME As Integer = 1
    Const IDX_FILE_PATH As Integer = 2
    Const IDX_CLIPPED As Integer = 3
    Const IDX_ERROR_MESSAGE As Integer = 4
    Const IDX_AOI_PATH As Integer = 5

    Public Sub New(ByVal dataTable As Hashtable, ByVal selDataSources As IList(Of String))
        InitializeComponent()
        m_dataTable = dataTable
        m_selDataSources = selDataSources
    End Sub

    Private Sub BtnAddAoi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAddAoi.Click
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
                If AlreadyOnList(aoiName) = False Then
                    Dim pAoi As Aoi = New Aoi(aoiName, DataPath, Nothing, bagisPExt.version)
                    'Get the cell size of this aoi using the filled dem
                    Dim aoiCellSize As Double = BA_CellSize(BA_GeodatabasePath(pAoi.FilePath, GeodatabaseNames.Surfaces), BA_EnumDescription(MapsFileName.filled_dem_gdb))
                    Dim clipCellSize As Double
                    For Each dsName As String In m_selDataSources
                        Dim dataSource As DataSource = m_dataTable(dsName)
                        '---check the cell size of the aoi against the clip data source if the clip is a raster---
                        If dataSource.LayerType = LayerType.Raster Then
                            Dim folderPath As String = "PleaseReturn"
                            Dim fileName As String = BA_GetBareName(dataSource.Source, folderPath)
                            clipCellSize = BA_CellSize(folderPath, FileName)
                        End If
                        'If the cell size is different
                        Dim clipDataSet As Boolean = True
                        If aoiCellSize <> clipCellSize Then
                            Dim sb As StringBuilder = New StringBuilder
                            sb.Append("The default cell size of AOI '" & aoiName & "'" & vbCrLf)
                            sb.Append("does not match the cell size of data source" & vbCrLf)
                            sb.Append("'" & dataSource.Name & "'. " & "This data will be resampled" & vbCrLf)
                            sb.Append("using the 'nearest neighbor' technique to the" & vbCrLf)
                            sb.Append("AOI cell size of approximately " & Math.Round(aoiCellSize, 4) & vbCrLf)
                            sb.Append("during the clipping process." & vbCrLf & vbCrLf)
                            sb.Append("Do you still wish to clip this layer ?")
                            Dim result As DialogResult = MessageBox.Show(sb.ToString, "Cell size mismatch", MessageBoxButtons.YesNo, _
                                                                         MessageBoxIcon.Warning)
                            If result = DialogResult.No Then clipDataSet = False
                        End If
                        'If measurement unit is slope, check to see if it conflicts with aoi slope unit
                        If clipDataSet = True And dataSource.MeasurementUnitType = MeasurementUnitType.Slope Then
                            'Get the slope unit of the selected aoi
                            Dim aoiSlope As SlopeUnit = BA_GetAoiSlopeUnit(pAoi.FilePath)
                            'If the units don't match, verify that the user wants to continue with adding/conversion
                            If aoiSlope <> dataSource.SlopeUnit Then
                                Dim sb As StringBuilder = New StringBuilder
                                sb.Append("The slope units of data source" & vbCrLf)
                                sb.Append("'" & dataSource.Name & "' do not match the" & vbCrLf)
                                sb.Append("slope units of AOI '" & aoiName & "'." & vbCrLf)
                                sb.Append("This data source will be recalculated" & vbCrLf)
                                sb.Append("to use '" & BA_EnumDescription(aoiSlope) & "' units" & vbCrLf)
                                sb.Append("during the clipping process." & vbCrLf & vbCrLf)
                                sb.Append("Do you still wish to clip this layer ?")
                                Dim result As DialogResult = MessageBox.Show(sb.ToString, "Slope unit mismatch", MessageBoxButtons.YesNo, _
                                                                             MessageBoxIcon.Warning)
                                If result = DialogResult.No Then clipDataSet = False
                            End If
                        End If
                        If clipDataSet = True Then
                            '---create a row---
                            Dim pRow As New DataGridViewRow
                            pRow.CreateCells(GrdAoi)
                            With pRow
                                .Cells(IDX_AOI).Value = aoiName
                                .Cells(IDX_AOI_PATH).Value = DataPath
                                .Cells(IDX_NAME).Value = dsName
                                .Cells(IDX_FILE_PATH).Value = dataSource.Source
                                .Cells(IDX_CLIPPED).Value = NO
                            End With
                            '---add the row---
                            GrdAoi.Rows.Add(pRow)
                        End If
                    Next
                End If
            End If
            Dim sortCol As DataGridViewColumn = GrdAoi.Columns(IDX_AOI)
            GrdAoi.Sort(sortCol, System.ComponentModel.ListSortDirection.Ascending)
            For Each pRow As DataGridViewRow In GrdAoi.SelectedRows
                pRow.Selected = False
            Next
            ManageClipAndRemoveButtons()
        Catch ex As Exception
            MessageBox.Show("BtnSelectAoi_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Function AlreadyOnList(ByVal aoiName) As Boolean
        For Each pRow As DataGridViewRow In GrdAoi.Rows
            Dim nextAoi As String = CStr(pRow.Cells(IDX_AOI).Value)
            If nextAoi = aoiName Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub BtnClip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClip.Click
        ' Create/configure a step progressor
        Dim pStepProg As IStepProgressor = Nothing
        Dim progressDialog2 As IProgressDialog2 = Nothing
        BtnClip.Enabled = False
        BtnAddAoi.Enabled = False
        BtnRemove.Enabled = False
        BtnCancel.Enabled = False
        Try
            If GrdAoi.Rows.Count > 0 Then
                pStepProg = BA_GetStepProgressor(My.ArcMap.Application.hWnd, GrdAoi.Rows.Count + 4)
                progressDialog2 = BA_GetProgressDialog(pStepProg, "Clipping the data sources ", "Clipping...")
                pStepProg.Show()
                progressDialog2.ShowDialog()
                pStepProg.Step()
                Dim idxRow As Integer = 0
                For Each pRow As DataGridViewRow In GrdAoi.Rows
                    Dim dsName As String = CStr(pRow.Cells(IDX_NAME).Value)
                    Dim aoiName As String = CStr(pRow.Cells(IDX_AOI).Value)
                    Dim aoiPath As String = CStr(pRow.Cells(IDX_AOI_PATH).Value)
                    progressDialog2.Description = "Clipping " & dsName & " to AOI " & aoiName
                    pStepProg.Step()
                    'If the settings path exists, we know we have aoipath\param folder
                    Dim settingsPath As String = BA_GetLocalSettingsPath(aoiPath)
                    Dim isValid As Boolean = True
                    If Not String.IsNullOrEmpty(settingsPath) Then
                        Dim dataBinPath As String = BA_GetDataBinPath(aoiPath)
                        pStepProg.Step()
                        If dataBinPath IsNot Nothing Then
                            'For Each pRow As DataGridViewRow In pCollection
                            'Dim key As String = CStr(pRow.Cells(idx_Name).Value)
                            Dim pDS As DataSource = m_dataTable(dsName)
                            If pDS IsNot Nothing Then
                                'Check to make sure we don't have a units conflict
                                If pDS.MeasurementUnitType <> MeasurementUnitType.Missing Then
                                    Dim aoiDataTbl As Hashtable = BA_LoadSettingsFile(BA_GetLocalSettingsPath(aoiPath))
                                    BA_AppendUnitsToDataSources(aoiDataTbl, aoiPath)
                                    Dim measureDS As DataSource = BA_ValidateMeasurementUnits(aoiDataTbl, pDS.MeasurementUnitType, _
                                                                                              pDS.MeasurementUnit, pDS.SlopeUnit)
                                    If measureDS IsNot Nothing Then
                                        If measureDS.Name = pDS.Name Then
                                            'Do nothing; We are clipping an existing layer of the same name and unit
                                        Else
                                            Dim units As String
                                            Dim importUnits As String
                                            If measureDS.MeasurementUnitType = MeasurementUnitType.Slope Then
                                                units = BA_EnumDescription(measureDS.SlopeUnit)
                                                importUnits = BA_EnumDescription(pDS.SlopeUnit)
                                            Else
                                                units = BA_EnumDescription(measureDS.MeasurementUnit)
                                                importUnits = BA_EnumDescription(pDS.MeasurementUnit)
                                            End If
                                            Dim sb As StringBuilder = New StringBuilder
                                            sb.Append("Measurement unit conflict with data source ")
                                            sb.Append(measureDS.Name & " in target AOI. " & measureDS.Name)
                                            sb.Append(" uses " & units.ToString & "for " & measureDS.MeasurementUnitType.ToString & " values.")
                                            isValid = False
                                            UpdateDataSourceStatusOnGrid(aoiName, dsName, NO, sb.ToString)
                                        End If
                                    End If
                                End If
                                If isValid Then
                                    SetDatumInExtension(aoiPath)
                                    'Check to see if the layer already exists and if the user wants to overwrite
                                    Dim layerAlreadyExists As Boolean = False
                                    Dim fullPath As String = dataBinPath & "\" & BA_GetBareName(pDS.Source)
                                    If pDS.LayerType = LayerType.Raster Then
                                        If BA_File_Exists(fullPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                                            layerAlreadyExists = True
                                        End If
                                    ElseIf pDS.LayerType = LayerType.Vector Then
                                        If BA_File_Exists(fullPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                                            layerAlreadyExists = True
                                        End If
                                    End If
                                    If layerAlreadyExists = False Then
                                        Dim clipFileName As String = BA_GetBareName(pDS.Source)
                                        Dim clipFullPath As String = dataBinPath & "\" & clipFileName
                                        Dim success As BA_ReturnCode = BA_ClipLayerToAoi(aoiPath, dataBinPath, pDS)
                                        If pDS.MeasurementUnitType = MeasurementUnitType.Slope And pDS.SlopeUnit <> SlopeUnit.Missing Then
                                            'Get the slope unit of the selected aoi
                                            Dim aoiSlope As SlopeUnit = BA_GetAoiSlopeUnit(aoiPath)
                                            'If the units don't match, verify that the user wants to continue with adding/conversion
                                            If aoiSlope <> pDS.SlopeUnit And pDS.LayerType = LayerType.Raster Then
                                                Dim tempFileName As String = "tmpSlope"
                                                'The boundLayerName is used as both an input to the BA_ExecuteMapAlgebraGDB function
                                                'and as part of the map algebr expression
                                                Dim boundLayerName As String = "slpInput"
                                                Dim mapExp As String = Nothing
                                                If aoiSlope = SlopeUnit.PctSlope Then
                                                    'Degrees to Perecent
                                                    mapExp = "Tan([" & boundLayerName & "] / 57.2957795) * 100"
                                                ElseIf aoiSlope = SlopeUnit.Degree Then
                                                    'Percent to degrees
                                                    mapExp = "ATan([" & boundLayerName & "] / 100) * 180 / 3.14159265359"
                                                End If
                                                'NOTE: We are saving temporary output directly to aoi root folder due to bug
                                                'with IMapAlgebraOp
                                                success = BA_ExecuteMapAlgebraGDB(dataBinPath, clipFileName, aoiPath, tempFileName, _
                                                                                  boundLayerName, mapExp)
                                                'If we recalculated successfully, overwrite the original clip layer with 
                                                'the recalculated layer
                                                If success = BA_ReturnCode.Success Then
                                                    'Delete the existing clip layer from the param.gdb
                                                    Dim retVal As Short = BA_RemoveRasterFromGDB(dataBinPath, clipFileName)
                                                    If retVal = 1 Then
                                                        'Save the recalculated slope layer to param.gdb 
                                                        success = BA_SaveFileRasterToGDB(aoiPath, tempFileName, dataBinPath, clipFileName, BA_RASTER_FORMAT)
                                                        If success = BA_ReturnCode.Success Then
                                                            'Remove the temporary recalculated layer from the aoi root folder
                                                            retVal = BA_Remove_Raster(aoiPath, tempFileName)
                                                            If retVal = 1 Then
                                                                'Set the slope units on the recalculated layer
                                                                BA_UpdateSlopeUnits(dataBinPath, clipFileName, aoiSlope)
                                                                success = BA_ReturnCode.Success
                                                            Else
                                                                success = BA_ReturnCode.UnknownError
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If

                                        'Get the cell size of this aoi using the filled dem
                                        Dim aoiCellSize As Double = BA_CellSize(BA_GeodatabasePath(aoiPath, GeodatabaseNames.Surfaces), BA_EnumDescription(MapsFileName.filled_dem_gdb))
                                        Dim clipCellSize As Double
                                        'check the cell size of the aoi against the clip data source if the clip is a raster
                                        If pDS.LayerType = LayerType.Raster Then
                                            Dim folderPath As String = "PleaseReturn"
                                            Dim fileName As String = BA_GetBareName(pDS.Source, folderPath)
                                            clipCellSize = BA_CellSize(folderPath, fileName)
                                        End If
                                        'If the cell sizes are different, we need to resample the result of the clip
                                        If aoiCellSize <> clipCellSize Then
                                            Dim tempFileName As String = "tmpResample"
                                            Dim tempFilePath As String = dataBinPath & "\" & tempFileName
                                            Dim snapRasterPath As String = aoiPath & BA_EnumDescription(PublicPath.AoiGrid)
                                            success = BA_Resample_Raster(clipFullPath, tempFilePath, aoiCellSize, snapRasterPath, Nothing)
                                            'If we resampled successfully, overwrite the original clip layer with the resampled layer
                                            If success = BA_ReturnCode.Success Then
                                                Dim retVal As Short = BA_RemoveRasterFromGDB(dataBinPath, clipFileName)
                                                If retVal = 1 Then
                                                    success = BA_RenameRasterInGDB(dataBinPath, tempFileName, clipFileName)
                                                End If
                                            End If
                                        End If
                                        If success = BA_ReturnCode.Success Then
                                            UpdateDataSourceStatusOnGrid(aoiName, dsName, YES, Nothing)
                                        Else
                                            UpdateDataSourceStatusOnGrid(aoiName, dsName, NO, "An error occurred while trying to clip this layer to the target AOI.")
                                        End If
                                    Else
                                        Dim sb As StringBuilder = New StringBuilder
                                        sb.Append("A data source with file name " & BA_GetBareName(pDS.Source) & " already exists in target AOI.")
                                        'isValid = False
                                        UpdateDataSourceStatusOnGrid(aoiName, dsName, NO, sb.ToString)
                                    End If
                                End If
                            End If
                            'Next
                        End If
                    End If
                Next

            End If
        Catch ex As Exception
            Debug.Print("BtnClip_Click Exception: " & ex.Message)
        Finally
            'The step progressor will be undefined if the cancel button was clicked
            If pStepProg IsNot Nothing Then
                pStepProg.Hide()
                pStepProg = Nothing
                progressDialog2.HideDialog()
                progressDialog2 = Nothing
                BtnCancel.Enabled = True
                BtnAddAoi.Enabled = True
            End If
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try

    End Sub

    Private Sub ManageClipAndRemoveButtons()
        If GrdAoi.Rows.Count > 0 Then
            BtnClip.Enabled = True
            BtnRemove.Enabled = True
        Else
            BtnClip.Enabled = False
            BtnRemove.Enabled = False
        End If
    End Sub

    ' Sets the datum string from the source DEM in the bagis-p extension
    Private Sub SetDatumInExtension(ByVal aoiPath As String)
        Dim pExt As BagisPExtension = BagisPExtension.GetExtension
        Dim workspaceType As WorkspaceType = workspaceType.Geodatabase
        Dim parentPath As String = aoiPath & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces)
        Dim pGeoDataSet As IGeoDataset = BA_OpenRasterFromGDB(parentPath, BA_EnumDescription(MapsFileName.filled_dem_gdb))
        If pGeoDataSet IsNot Nothing Then
            'Spatial reference for the dataset in question
            Dim pSpRef As ESRI.ArcGIS.Geometry.ISpatialReference = pGeoDataSet.SpatialReference
            If pSpRef IsNot Nothing Then
                pExt.Datum = BA_DatumString(pSpRef)
                pExt.SpatialReference = pSpRef.Name
            End If
            pSpRef = Nothing
        End If
        pGeoDataSet = Nothing
        GC.WaitForPendingFinalizers()
        GC.Collect()
    End Sub

    Private Sub UpdateDataSourceStatusOnGrid(ByVal aoiName As String, ByVal dsName As String, ByVal clipped As String, ByVal errorMessage As String)
        For Each pRow As DataGridViewRow In GrdAoi.Rows
            'This is the row with the right method
            If pRow.Cells(IDX_AOI).Value = aoiName And pRow.Cells(IDX_NAME).Value = dsName Then
                'Update the status
                pRow.Cells(IDX_CLIPPED).Value = clipped
                pRow.Cells(IDX_ERROR_MESSAGE).Value = errorMessage
            End If
        Next
    End Sub

    Private Sub GrdAoi_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GrdAoi.SelectionChanged
        ManageClipAndRemoveButtons()
    End Sub

    Private Sub BtnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnRemove.Click
        Dim pCollection As DataGridViewSelectedRowCollection = GrdAoi.SelectedRows

        For Each pRow As DataGridViewRow In pCollection
            GrdAoi.Rows.Remove(pRow)
        Next

        For Each pRow As DataGridViewRow In GrdAoi.SelectedRows
            pRow.Selected = False
        Next
    End Sub

End Class