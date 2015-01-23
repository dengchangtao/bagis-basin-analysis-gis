Imports ESRI.ArcGIS.Geodatabase
Imports System.Windows.Forms
Imports System.IO
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Carto
Imports BAGIS_ClassLibrary

Public Class FrmVecToRasNum
    Dim m_aoi As Aoi
    Dim m_lstVectorLayersItem As LayerListItem = Nothing
    Dim vectorName As String
    Dim vectorPath As String

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Set AOI if already set
        Dim hruExt As HruExtension = HruExtension.GetExtension
        Dim aoi As Aoi = hruExt.aoi
        If aoi IsNot Nothing Then
            TxtAoiPath.Text = aoi.FilePath
            ResetVecToRasForm(aoi)
        End If

    End Sub

    Private Sub BtnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnApply.Click

        Dim vrFeatureClass As IFeatureClass = BA_OpenFeatureClassFromGDB(vectorPath, vectorName)
        If vrFeatureClass Is Nothing Then
            Throw New Exception("Feature Class Not Found.")
            Exit Sub
        End If

        'Validate the datum
        Dim hruExt As HruExtension = HruExtension.GetExtension
        If hruExt IsNot Nothing Then
            Dim fullVectorPath As String = vectorPath & "\" & vectorName
            Dim validDatum As Boolean = BA_VectorDatumMatch(fullVectorPath, hruExt.Datum)
            If validDatum = False Then
                MessageBox.Show("The selected layer '" & vectorName & "' cannot be converted because the datum does not match the AOI DEM.", "Invalid datum", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If

        'Get cell size value from the form
        Dim cellSize As Double = TxtCellSize.Text

        Dim exists As Boolean = BA_File_Exists(vectorPath & "\" & TxtOutRas.Text, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset)
        If exists = True Then
            Dim result As DialogResult = MessageBox.Show("Raster file already exists. Overwrite existing raster file ?", "Folder exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.Yes Then
                BA_RemoveRasterFromGDB(vectorPath, TxtOutRas.Text)
            Else
                TxtOutRas.Focus()
                Exit Sub
            End If
        End If

        Dim snapRasterPath As String = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
        Feature2Raster(vrFeatureClass, vectorPath, TxtOutRas.Text, cellSize, CboAttField.SelectedItem, snapRasterPath)
        vrFeatureClass = Nothing

        'Get handle to UI (form) to reload user layer lists
        Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of frmHruZone.AddinImpl)(My.ThisAddIn.IDs.frmHruZone)
        If dockWindowAddIn IsNot Nothing Then
            Dim hruZoneForm As frmHruZone = dockWindowAddIn.UI
            If hruZoneForm IsNot Nothing Then
                hruZoneForm.BA_ReloadUserLayers()
            End If
        End If

        MsgBox(TxtOutRas.Text & " raster is added to the AOI.")
        'BtnCancel_Click(sender, e)

    End Sub

    'Performs feature2Raster conversion
    Private Sub Feature2Raster(ByVal InputFeatureClass As IFeatureClass, ByVal filepath As String, _
                               ByVal FileName As String, ByVal Cellsize As Object, ByVal valueField As String, _
                               ByVal snapRasterPath As String)

        Dim allvaluesint As Boolean = True
        Dim pCursor As IFeatureCursor = Nothing
        Dim pFeature As IFeature = Nothing

        Try
            Dim pIndex As Short = InputFeatureClass.FindField(valueField)
            If pIndex < 1 Then
                Throw New Exception("The selected field does not exist on input feature class.")
            End If

            Dim pField As IField = InputFeatureClass.Fields.Field(pIndex)
            If pField.Type = esriFieldType.esriFieldTypeString Then
                ' Conversion field is a text field so we don't need to check for decimal places
                BA_Feature2RasterInteger(InputFeatureClass, filepath, FileName, Cellsize, valueField, snapRasterPath)
            Else
                'Conversion field is a number field so we need to make sure there are no double values
                'Cycle through all feature values to see if there is any double numerical values
                pCursor = InputFeatureClass.Update(Nothing, False)
                pFeature = pCursor.NextFeature
                While Not pFeature Is Nothing
                    Dim objValue As Object = pFeature.Value(pIndex)
                    If IsNumeric(objValue) Then
                        If InStr(CStr(objValue), ".") > 0 Then
                            allvaluesint = False
                        End If
                    Else
                        Throw New Exception("Value is not numeric.")
                    End If
                    pFeature = pCursor.NextFeature()
                End While

                If allvaluesint = True Then
                    BA_Feature2RasterInteger(InputFeatureClass, filepath, FileName, Cellsize, valueField, snapRasterPath)
                Else
                    BA_Feature2RasterDouble(InputFeatureClass, filepath, FileName, Cellsize, valueField, snapRasterPath)
                End If
            End If

        Catch ex As Exception
            MsgBox("Feature2Raster Exception: " & ex.Message)
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pCursor)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pFeature)
        End Try
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub LstVectorLayers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstVectorLayers.SelectedIndexChanged
        BtnApply.Enabled = False

        ' unselect previous selected item
        If m_lstVectorLayersItem IsNot Nothing Then
            LstVectorLayers.SelectedItems.Remove(m_lstVectorLayersItem)
            CboAttField.Items.Clear()
        End If
        ' reset selected index to new value
        m_lstVectorLayersItem = LstVectorLayers.SelectedItem

        If m_lstVectorLayersItem Is Nothing Then
            MessageBox.Show("No Vector Layer is selected.")
            Exit Sub
        End If

        vectorPath = ""
        vectorName = BA_GetBareName(m_lstVectorLayersItem.Value, vectorPath)

        Dim vrFeatureClass As IFeatureClass = BA_OpenFeatureClassFromGDB(vectorPath, vectorName)
        If vrFeatureClass Is Nothing Then
            Throw New Exception("Feature Class Not Found.")
        End If
        Dim strList As List(Of String) = BA_FieldsListFromFeatureClass(vrFeatureClass)

        For Each str As String In strList
            CboAttField.Items.Add(str)
        Next
        CboAttField.SelectedIndex = 0

        TxtOutRas.Text = vectorName.Substring(0, 6) & "_g"

        'First get the unit from the projected coordinate system
        Dim geoDataSet As IGeoDataset = vrFeatureClass
        Dim pSpRef As ISpatialReference = geoDataSet.SpatialReference
        Dim projCoordSys As IProjectedCoordinateSystem = pSpRef
        Dim pLinearUnit As ILinearUnit = projCoordSys.CoordinateUnit
        LblCellUnit.Text = pLinearUnit.Name

        geoDataSet = Nothing
        vrFeatureClass = Nothing

        BtnApply.Enabled = True
    End Sub

    Private Sub BtnAOISelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAOISelect.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim DataPath As String
        Dim pFilter As IGxObjectFilter = New GxFilterContainers
        Dim hruExt As HruExtension = HruExtension.GetExtension

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
                m_aoi = New Aoi(aoiName, DataPath, Nothing, hruExt.version)
                TxtAoiPath.Text = m_aoi.FilePath
                ResetVecToRasForm(Nothing)
            End If
        Catch ex As Exception
            MessageBox.Show("BtnAOISelect_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub ResetVecToRasForm(ByVal aoi As Aoi)

        If aoi IsNot Nothing Then m_aoi = aoi

        Try
            LstVectorLayers.Items.Clear()

            Dim AOIVectorList() As String = Nothing
            Dim AOIRasterList() As String = Nothing
            Dim layerPath As String = m_aoi.FilePath & "\" & BA_EnumDescription(GeodatabaseNames.Layers)
            BA_ListLayersinGDB(layerPath, AOIRasterList, AOIVectorList)
            Dim fullLayerPath As String

            'display shapefiles
            Dim ShapefileCount As Long = UBound(AOIVectorList)
            If ShapefileCount > 0 Then
                For i = 1 To ShapefileCount
                    ' Vectors are always discrete
                    Dim isDiscrete As Boolean = True
                    fullLayerPath = layerPath & "\" & AOIVectorList(i)
                    Dim item As LayerListItem = New LayerListItem(AOIVectorList(i), fullLayerPath, LayerType.Vector, isDiscrete)
                    LstVectorLayers.Items.Add(item)
                Next

                ' Filled DEM Path
                Dim DEMlayerPath As String = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Surfaces, True)
                fullLayerPath = DEMlayerPath & BA_EnumDescription(MapsFileName.filled_dem_gdb)

                'get rasterresolution
                Dim cellSize As Double
                Dim rasterStat As IRasterStatistics = BA_GetRasterStatsGDB(fullLayerPath, cellSize)
                TxtCellSize.Text = cellSize
            Else
                MessageBox.Show("No vector dataset is found in this AOI. Please use the AOI Utilities tool on the Basin Analyst menu of BAGIS to add vector layers to the AOI.", "No vector dataset", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            SetDatumInExtension()
        Catch ex As Exception
            MessageBox.Show("Exception: " + ex.Message)
        Finally
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Sub

    Private Sub FrmVecToRasNum_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.VisibleChanged
        If Me.Visible Then
            Dim hruExt As HruExtension = HruExtension.GetExtension
            Dim aoi As Aoi = hruExt.aoi
            If aoi IsNot Nothing Then
                ResetVecToRasForm(aoi)
            End If
        End If
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