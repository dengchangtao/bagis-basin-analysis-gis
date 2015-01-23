Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports System.Windows.Forms
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Geometry
Imports BAGIS_ClassLibrary

Public Class FrmSelectTemplateLayer

    Private m_aoi As Aoi
    Private m_inputLayerPath As String
    Private m_inputLayerFolder As String
    Private m_inputField As String

    Public Sub New(ByVal aoi As Aoi)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Set AOI
        If aoi IsNot Nothing Then
            m_aoi = aoi
        Else
            Dim hruExt As HruExtension = HruExtension.GetExtension
            m_aoi = hruExt.aoi
        End If
        If m_aoi IsNot Nothing Then
            TxtAoiPath.Text = m_aoi.FilePath
            LoadLstLayers()
            SetDatumInExtension()
        End If

    End Sub

    Private Sub BtnSelectAoi_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSelectAoi.Click
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
            Dim folderType As FolderType = BA_GetFGDBFolderType(DataPath)
            If folderType <> folderType.AOI Then
                MessageBox.Show("The selected folder does not contain a valid AOI!")
                Exit Sub
            Else
                BA_SetDefaultProjection(My.ArcMap.Application)
                Dim ReturnPath As String = "PleaseReturn"
                'Choose to use supplied DataPath here rather than ReturnString generated from source.weasel
                Dim aoiName As String = BA_GetBareName(DataPath, ReturnPath)
                Dim aoiFolder As String = DataPath
                If BA_ContainsSpace(aoiFolder) Or BA_ContainsSpace(aoiName) Then
                    MessageBox.Show("The aoi folder and name cannot contain spaces.")
                    Exit Sub
                End If
                m_aoi = New Aoi(aoiName, aoiFolder, Nothing, hruExt.version)
                Dim errorMsg As String = BA_CheckAoiPathLength(m_aoi.FilePath)
                If Not String.IsNullOrEmpty(errorMsg) Then
                    MessageBox.Show(errorMsg, "Invalid AOI path", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
                TxtAoiPath.Text = m_aoi.FilePath
                LoadLstLayers()
                SetDatumInExtension()
            End If
        Catch ex As Exception
            MessageBox.Show("BtnSelectAoi_Click Exception: " & ex.Message)
        End Try
    End Sub

    ' Populate layers listBox
    Private Sub LoadLstLayers()
        LstAoiRasterLayers.Items.Clear()
        Dim AOIVectorList() As String = Nothing
        Dim AOIRasterList() As String = Nothing
        Dim layerPath As String = m_aoi.FilePath & "\" & BA_EnumDescription(GeodatabaseNames.Layers)
        BA_ListLayersinGDB(layerPath, AOIRasterList, AOIVectorList)
        Dim rasterCount As Integer = UBound(AOIRasterList)
        If rasterCount > 0 Then
            For i = 1 To rasterCount
                Dim fullLayerPath As String = layerPath & "\" & AOIRasterList(i)
                Dim isDiscrete As Boolean = BA_IsIntegerRasterGDB(fullLayerPath)
                Dim item As LayerListItem = New LayerListItem(AOIRasterList(i), fullLayerPath, LayerType.Raster, isDiscrete)
                If item.IsDiscrete Then LstAoiRasterLayers.Items.Add(item)
            Next
        End If
    End Sub

    Private Sub LstAoiRasterLayers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstAoiRasterLayers.SelectedIndexChanged
        Dim selLayer As LayerListItem = CType(LstAoiRasterLayers.SelectedItem, LayerListItem)
        UpdateReclassList(selLayer)
        If LstAoiRasterLayers.SelectedItems.Count > 0 And _
           LstReclassField.SelectedItems.Count > 0 Then
            BtnApply.Enabled = True
        End If
    End Sub

    Private Sub UpdateReclassList(ByVal selLayer As LayerListItem)
        Dim filePath As String = "PleaseReturn"
        Dim fileName As String = BA_GetBareName(selLayer.Value, filePath)
        Dim pGeoDataset As IGeoDataset = Nothing
        Dim pRasterBandCollection As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pTable As ITable = Nothing

        Try
            LstReclassField.Items.Clear()
            pGeoDataset = BA_OpenRasterFromGDB(filePath, fileName)
            pRasterBandCollection = CType(pGeoDataset, IRasterBandCollection)
            pRasterBand = pRasterBandCollection.Item(0)
            If pRasterBand IsNot Nothing Then
                pTable = pRasterBand.AttributeTable
                Dim pFields As IFields = pTable.Fields
                Dim uBound As Integer = pFields.FieldCount - 1
                For i = 1 To uBound
                    Dim pField As IField = pFields.Field(i)
                    If pField.Type = esriFieldType.esriFieldTypeInteger Or _
                    pField.Type = esriFieldType.esriFieldTypeSmallInteger Then
                        LstReclassField.Items.Add(pField.Name)
                        'If pField.Name = m_selectedReclassField Then
                        '    LstReclassField.SelectedItem = pField.Name
                        'End If
                    End If
                Next
            End If

        Catch ex As Exception
            MsgBox("UpdateReclassList Exception: " & ex.Message)
        Finally
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pTable)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBand)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pRasterBandCollection)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pGeoDataset)
        End Try

    End Sub

    Public ReadOnly Property InputLayerPath() As String
        Get
            Return m_inputLayerPath
        End Get
    End Property

    Public ReadOnly Property InputLayerFolder() As String
        Get
            Return m_inputLayerFolder
        End Get
    End Property

    Public ReadOnly Property InputField() As String
        Get
            Return m_inputField
        End Get
    End Property

    Public ReadOnly Property Aoi() As Aoi
        Get
            Return m_aoi
        End Get
    End Property

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub BtnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        Dim selLayer As LayerListItem = CType(LstAoiRasterLayers.SelectedItem, LayerListItem)
        'Validate the datum
        Dim hruExt As HruExtension = HruExtension.GetExtension
        If hruExt IsNot Nothing Then
            Dim validDatum As Boolean = BA_DatumMatch(selLayer.Value, hruExt.Datum)
            If validDatum = False Then
                MessageBox.Show("The selected layer '" & selLayer.Name & "' cannot be used in a template because the datum does not match the AOI DEM.", "Invalid datum", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If

        m_inputLayerPath = selLayer.Value
        m_inputLayerFolder = BA_EnumDescription(PublicPath.Layers) & "\" & selLayer.Value
        m_inputField = CStr(LstReclassField.SelectedItem)
        BtnCancel_Click(sender, e)
    End Sub

    Private Sub LstReclassField_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstReclassField.SelectedIndexChanged
        If LstReclassField.SelectedItems.Count > 0 Or _
           LstAoiRasterLayers.SelectedItems.Count > 0 Then
            BtnApply.Enabled = True
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