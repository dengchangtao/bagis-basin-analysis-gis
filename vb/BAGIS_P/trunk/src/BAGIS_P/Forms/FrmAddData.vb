Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesRaster
Imports System.Text
Imports System.Windows.Forms
Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework

Public Class FrmAddData

    Dim m_layerTable As Hashtable
    Dim m_selLayerName As String
    Dim m_selDataSource As DataSource
    Dim m_layerType As LayerType
    Dim m_DirtyFlag As Boolean = False
    Dim m_settingsPath As String = Nothing
    Dim m_aoiPath As String = Nothing

    Public Sub New(ByVal layerTable As Hashtable, ByVal aoiPath As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_layerTable = layerTable
        If String.IsNullOrEmpty(aoiPath) Then
            m_settingsPath = BA_GetBagisPSettingsPath()
        Else
            m_settingsPath = BA_GetLocalSettingsPath(aoiPath)
            m_aoiPath = aoiPath
        End If

        LoadMeasurementUnitTypes()

    End Sub

    Public Sub New(ByVal layerTable As Hashtable, ByVal selLayerName As String, ByVal aoiPath As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_layerTable = layerTable
        m_selLayerName = selLayerName
        If String.IsNullOrEmpty(aoiPath) Then
            m_settingsPath = BA_GetBagisPSettingsPath()
        Else
            m_settingsPath = BA_GetLocalSettingsPath(aoiPath)
            m_aoiPath = aoiPath
        End If

        m_selDataSource = m_layerTable(m_selLayerName)
        TxtName.Text = selLayerName
        TxtDescription.Text = m_selDataSource.Description
        TxtSource.Text = m_selDataSource.Source
        m_LayerType = m_selDataSource.LayerType

        '24-APR-2012 As of this date we aren't saving the data field
        'PopulateCboDataField(pGeoDataset, dataType, pLayer.DataField)

        LoadMeasurementUnitTypes()
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Protected Friend ReadOnly Property DirtyFlag() As Boolean
        Get
            Return m_DirtyFlag
        End Get
    End Property

    Private Sub BtnSelectSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSelectSource.Click
        'Declare ArcObjects outside of Try/Catch so we can dispose of them in Finally
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        'Only show shapefiles
        Dim pFilter As IGxObjectFilter = New GxFilterGeoDatasets
        'Collection of layers to be assigned here
        Dim pGxObjects As IEnumGxObject = Nothing
        Dim pGxDataset As IGxDataset
        Try
            'initialize and open mini browser
            With pGxDialog
                .AllowMultiSelect = False
                .ButtonCaption = "Select"
                .Title = "Select data source"
                'using shapefile filter
                .ObjectFilter = pFilter
                'open dialog passing handle to Application from AddIn
                bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObjects)
            End With
            'no file selected; exit
            If bObjectSelected = Nothing Then Exit Sub
            'get first dataset
            pGxDataset = pGxObjects.Next
            'no dataset has been selected
            If pGxDataset Is Nothing Then Exit Sub
            Dim folderPath As String = pGxDataset.Dataset.Workspace.PathName
            'Folder path may have trailing backslash if it is a grid
            folderPath = BA_StandardizePathString(folderPath)
            Dim fileName As String = pGxDataset.DatasetName.Name
            TxtSource.Text = folderPath & "\" & fileName
            '24-APR-2012 As of this date we aren't saving the data field
            'PopulateCboDataField(pGxDataset.Dataset, pGxDataset.Type, Nothing)
            SetLayerType(pGxDataset.Type)
        Catch ex As Exception
            Debug.Print("BtnSelectSource_Click Exception: " & ex.Message)
        Finally

        End Try
    End Sub

    Private Sub ManageApplyButton()
        Dim layerName As String = Trim(TxtName.Text)
        Dim source As String = Trim(TxtSource.Text)
        If String.IsNullOrEmpty(layerName) Or
            String.IsNullOrEmpty(source) Then
            BtnApply.Enabled = False
        Else
            BtnApply.Enabled = True
        End If
    End Sub

    Private Sub TxtLayerName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtName.TextChanged
        ManageApplyButton()
    End Sub

    Private Sub TxtSource_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtSource.TextChanged
        ManageApplyButton()
    End Sub

    Private Sub BtnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        '24-APR-2012 As of this date we aren't saving the data field
        'Validate that the user has selected a field
        'If String.IsNullOrEmpty(CboDataField.SelectedItem) Then
        '    Dim sb As StringBuilder = New StringBuilder
        '    sb.Append("Please select a field from the data" & vbCrLf)
        '    sb.Append("source. A data field is required.")
        '    MessageBox.Show(sb.ToString, "Missing field", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    CboDataField.Focus()
        '    Exit Sub
        'End If

        'Don't allow spaces in layer name
        Dim layerName As String = Trim(TxtName.Text)
        layerName = layerName.Replace(" ", "_")
        'Don't allow hyphens in layer name
        layerName = layerName.Replace("-", "_")
        'Get layerType enumeration
        Dim pLayerType As LayerType = m_layerType

        Dim selUnitType As MeasurementUnitType
        Dim selUnit As MeasurementUnit
        Dim selSlopeUnit As SlopeUnit
        If CkUnits.Checked Then
            If CboUnitType.SelectedIndex > -1 Then
                selUnitType = BA_GetMeasurementUnitType(CboUnitType.SelectedItem)
            End If
            If selUnitType = MeasurementUnitType.Slope And CboUnits.SelectedIndex > -1 Then
                selSlopeUnit = BA_GetSlopeUnit(CboUnits.SelectedItem)
            ElseIf CboUnits.SelectedIndex > -1 Then
                selUnit = BA_GetMeasurementUnit(CboUnits.SelectedItem)
            End If
        End If
        Dim unitSource As DataSource = BA_ValidateMeasurementUnits(m_layerTable, selUnitType, _
                                                                   selUnit, selSlopeUnit)
        'We may have a measurement unit conflict
        If unitSource IsNot Nothing Then
            If unitSource.Name = layerName Then
                'Do nothing; We are updating an existing layer of the same name and unit
            Else
                Dim units As String
                If unitSource.MeasurementUnitType = MeasurementUnitType.Slope Then
                    units = BA_EnumDescription(unitSource.SlopeUnit)
                Else
                    units = BA_EnumDescription(unitSource.MeasurementUnit)
                End If
                Dim sb As StringBuilder = New StringBuilder
                sb.Append("There is an existing data source named " & vbCrLf)
                sb.Append(unitSource.Name & " that uses unit type " & unitSource.MeasurementUnitType.ToString & vbCrLf)
                sb.Append("and units " & units & ". This is not compatible" & vbCrLf)
                sb.Append("with your current selection of " & CboUnits.SelectedItem & "." & vbCrLf)
                sb.Append("All data sources that share a measurement unit type" & vbCrLf)
                sb.Append("must use the same units.")
                MessageBox.Show(sb.ToString, "Measurement unit error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If

        'Check to see if > 1 layer in the data manager has the same file name
        'If so, warn user that it may not be able to be clipped to an aoi with another file of the same name
        Dim source As String = Trim(TxtSource.Text) 'This is the full path
        Dim newFileName As String = BA_GetBareName(source)
        Dim sb1 As StringBuilder = New StringBuilder
        For Each key As String In m_layerTable.Keys
            Dim nextDataSource As DataSource = m_layerTable(key)
            'Only need to check custom layers and layers that are NOT the selected layer
            If nextDataSource.AoiLayer = False AndAlso layerName <> m_selLayerName Then
                Dim fileName2 As String = BA_GetBareName(nextDataSource.Source)
                If fileName2.ToUpper = newFileName.ToUpper Then
                    'Add prefix to warning message; The stringbuilder hasn't been initialized yet
                    If sb1.Length < 1 Then
                        sb1.Append("The data source you are trying to add" & vbCrLf)
                        sb1.Append("has the same file name as the following" & vbCrLf)
                        sb1.Append("data source(s):" & vbCrLf & vbCrLf)
                    End If
                    sb1.Append("Name: " & nextDataSource.Name & " Path:" & nextDataSource.Source & vbCrLf)
                End If
            End If
        Next
        'Add suffix to warning message if there were any conflicts and pop message
        If sb1.Length > 0 Then
            sb1.Append(vbCrLf & "You may not clip data sources with the" & vbCrLf)
            sb1.Append("same name to an AOI. If you plan to use this data" & vbCrLf)
            sb1.Append("source with the data source(s) listed above, rename " & vbCrLf)
            sb1.Append("the file with a unique name before adding it in " & vbCrLf)
            sb1.Append("the Data Manager." & vbCrLf & vbCrLf)
            sb1.Append("Do you still wish to add this data source ?")
            Dim result As DialogResult = MessageBox.Show(sb1.ToString, "Duplicate file name", MessageBoxButtons.YesNo, _
                                                         MessageBoxIcon.Warning)
            If result <> DialogResult.Yes Then
                Exit Sub
            End If
        End If

        'Verify that we want to overwrite an existing layer if it has the same name
        'as the name on the form and is NOT the selected layer
        Dim overwriteLayer As DataSource = m_layerTable(layerName)
        If overwriteLayer IsNot Nothing AndAlso layerName <> m_selLayerName Then
            Dim sb As StringBuilder = New StringBuilder
            sb.Append("You are about to overwrite an" & vbCrLf)
            sb.Append("existing data layer '" & layerName & "'." & vbCrLf)
            sb.Append("Do you wish to continue?")
            Dim result As DialogResult = MessageBox.Show(sb.ToString, "Existing data layer", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If result = Windows.Forms.DialogResult.Yes Then
                m_layerTable.Remove(layerName)
            Else
                TxtName.Focus()
                Exit Sub
            End If
        End If

        'Update an existing layer if we have one
        If Not String.IsNullOrEmpty(m_selLayerName) Then
            Dim pLayer As DataSource = m_layerTable(m_selLayerName)
            pLayer.Name = layerName
            pLayer.Source = Trim(TxtSource.Text)
            pLayer.Description = TxtDescription.Text
            '24-APR-2012 As of this date we aren't saving the data field
            'pLayer.DataField = CStr(CboDataField.SelectedItem)
            pLayer.LayerType = pLayerType
            If CkUnits.Checked Then
                pLayer.MeasurementUnitType = selUnitType
                If pLayer.MeasurementUnitType = MeasurementUnitType.Slope Then
                    pLayer.MeasurementUnit = MeasurementUnit.Missing
                    pLayer.SlopeUnit = selSlopeUnit
                Else
                    pLayer.MeasurementUnit = selUnit
                    pLayer.SlopeUnit = SlopeUnit.Missing
                End If
            Else
                pLayer.MeasurementUnitType = MeasurementUnitType.Missing
                pLayer.MeasurementUnit = MeasurementUnit.Missing
            End If
            'Update layer in table
            m_layerTable.Item(m_selLayerName) = pLayer
            m_selDataSource = pLayer
        End If

        If String.IsNullOrEmpty(m_selLayerName) Then
            'Add new layer to list so we can persist it
            Dim id As Integer = BA_GetNextDataSourceId(m_settingsPath)
            Dim newLayer As DataSource = New DataSource(id, layerName, TxtDescription.Text, source, False, _
                                                        pLayerType)
            If CkUnits.Checked Then
                newLayer.MeasurementUnitType = selUnitType
                If newLayer.MeasurementUnitType = MeasurementUnitType.Slope Then
                    newLayer.MeasurementUnit = MeasurementUnit.Missing
                    newLayer.SlopeUnit = selSlopeUnit
                Else
                    newLayer.MeasurementUnit = selUnit
                    newLayer.SlopeUnit = SlopeUnit.Missing
                End If
            Else
                newLayer.MeasurementUnitType = MeasurementUnitType.Missing
                newLayer.MeasurementUnit = MeasurementUnit.Missing
            End If

            'This is a new source; We need to add it to the table
            m_layerTable.Item(layerName) = newLayer
            m_selDataSource = newLayer
        End If

        'If this is a local data source, we need to clip it to the aoi
        Dim success As BA_ReturnCode = BA_ReturnCode.Success
        If Not String.IsNullOrEmpty(m_aoiPath) Then
            Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 5)
            Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Clipping the data source ", "Clipping...")
            pStepProg.Show()
            progressDialog2.ShowDialog()
            pStepProg.Step()
            '1. Verify local settings (aoipath\param folder) path exists
            Dim settingsPath As String = BA_GetLocalSettingsPath(m_aoiPath)
            '2. Get dataBin path from aoi string
            Dim dataBinPath As String = BA_GetDataBinPath(m_aoiPath)
            '3. SetDatumInExtension
            SetDatumInExtension(m_aoiPath)
            '4. ClipAOI and check return code so we only save it if we were successful
            success = BA_ClipLayerToAoi(m_aoiPath, dataBinPath, m_layerTable(layerName))
            '5. Set path of new datasource to file name
            Dim aoiSource As DataSource = m_layerTable(layerName)
            Dim fileName As String = BA_GetBareName(aoiSource.Source)
            aoiSource.Source = fileName
            m_layerTable.Item(layerName) = aoiSource
            If pStepProg IsNot Nothing Then
                pStepProg.Hide()
                pStepProg = Nothing
                progressDialog2.HideDialog()
                progressDialog2 = Nothing
            End If
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End If

        If success = BA_ReturnCode.Success Then
            'Persist changes to settings.xml
            Dim srcList As List(Of DataSource) = New List(Of DataSource)
            For Each key As String In m_layerTable.Keys
                Dim pDS As DataSource = m_layerTable(key)
                srcList.Add(pDS)
            Next
            BA_SaveDataLayers(srcList, m_settingsPath)
            UpdateMeasurementUnits()
        End If
        m_DirtyFlag = True
        Me.Close()
    End Sub

    '24-APR-2012 As of this date we aren't saving the data field
    'Private Sub PopulateCboDataField(ByVal pGeoDataset As IGeoDataset, ByVal data_type As esriDatasetType, _
    '                                 ByVal selField As String)
    '    Dim pFeatureClass As IFeatureClass = Nothing
    '    Dim pRasterBandColl As IRasterBandCollection = Nothing
    '    Dim pRasterBand As IRasterBand = Nothing
    '    Dim pTable As ITable = Nothing
    '    Dim pFields As IFields = Nothing
    '    Try
    '        CboDataField.Items.Clear()
    '        Select Case data_type
    '            Case 4, 5 'shapefile
    '                pFeatureClass = CType(pGeoDataset, FeatureClass)
    '                pFields = pFeatureClass.Fields
    '                TxtLayerType.Text = LayerType.Vector.ToString
    '            Case 12, 13 'raster
    '                pRasterBandColl = CType(pGeoDataset, IRasterBandCollection)
    '                pRasterBand = pRasterBandColl.Item(0)
    '                pTable = pRasterBand.AttributeTable
    '                pFields = pTable.Fields
    '                TxtLayerType.Text = LayerType.Raster.ToString
    '            Case Else 'unsupported format
    '                Dim sb As StringBuilder = New StringBuilder
    '                sb.Append("The type of the selected data source is not supported by BAGIS-P.")
    '                MessageBox.Show(sb.ToString, "Unsupported type", MessageBoxButtons.OK, MessageBoxIcon.Information)
    '                Exit Sub
    '        End Select

    '        If pFields IsNot Nothing Then
    '            For i As Integer = 0 To pFields.FieldCount - 1
    '                Dim pField As IField = pFields.Field(i)
    '                CboDataField.Items.Add(pField.AliasName)
    '                If pField.AliasName = selField Then
    '                    CboDataField.SelectedItem = pField.AliasName
    '                End If
    '            Next
    '        End If
    '    Catch ex As Exception
    '        Debug.Print("PopulateCboDataField Exception: " & ex.Message)
    '    Finally
    '        pFeatureClass = Nothing
    '        pRasterBandColl = Nothing
    '        pRasterBand = Nothing
    '        pFields = Nothing
    '        GC.WaitForPendingFinalizers()
    '        GC.Collect()
    '    End Try

    'End Sub

    Private Sub SetLayerType(ByVal data_type As esriDatasetType)
        Select Case data_type
            Case 4, 5 'shapefile
                m_layerType = LayerType.Vector
            Case 12, 13 'raster
                m_layerType = LayerType.Raster
            Case Else 'unsupported format
                Dim sb As StringBuilder = New StringBuilder
                sb.Append("The type of the selected data source is not supported by BAGIS-P.")
                MessageBox.Show(sb.ToString, "Unsupported type", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
        End Select
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

    Private Sub CkUnits_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CkUnits.CheckedChanged
        PnlUnits.Visible = CkUnits.Checked
    End Sub

    Private Sub LoadMeasurementUnitTypes()
        CboUnitType.Items.Clear()
        Dim enumValues As System.Array = System.[Enum].GetValues(GetType(MeasurementUnitType))
        'Start adding at position 1 to exclude "Missing" value
        For i As Integer = 1 To enumValues.Length - 1
            CboUnitType.Items.Add(enumValues(i).ToString)
        Next

        'Set the measurement unit in the UI, if appropriate
        CboUnitType.SelectedIndex = 0
        If m_selDataSource IsNot Nothing AndAlso _
            m_selDataSource.MeasurementUnitType <> MeasurementUnitType.Missing Then
            CkUnits.Checked = True
            For Each strItem As String In CboUnitType.Items
                If strItem = m_selDataSource.MeasurementUnitType.ToString Then
                    CboUnitType.SelectedItem = strItem
                End If
            Next
        End If

    End Sub

    Private Sub LoadMeasurementUnits()
        CboUnits.Items.Clear()
        Dim strUnitType As String = CboUnitType.SelectedItem
        Dim measUnitType As MeasurementUnitType = BA_GetMeasurementUnitType(strUnitType)
        Select Case measUnitType
            Case MeasurementUnitType.Depth
                CboUnits.Items.Add(MeasurementUnit.Inches.ToString)
                CboUnits.Items.Add(MeasurementUnit.Millimeters.ToString)
            Case MeasurementUnitType.Elevation
                CboUnits.Items.Add(MeasurementUnit.Feet.ToString)
                CboUnits.Items.Add(MeasurementUnit.Meters.ToString)
            Case MeasurementUnitType.Temperature
                CboUnits.Items.Add(MeasurementUnit.Celsius.ToString)
                CboUnits.Items.Add(MeasurementUnit.Fahrenheit.ToString)
            Case MeasurementUnitType.Slope
                CboUnits.Items.Add(BA_EnumDescription(SlopeUnit.Degree))
                CboUnits.Items.Add(BA_EnumDescription(SlopeUnit.PctSlope))
        End Select
        If CboUnits.Items.Count > 0 Then
            CboUnits.SelectedIndex = 0
        End If
    End Sub

    Private Sub CboUnitType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboUnitType.SelectedIndexChanged
        If CboUnitType.SelectedItem IsNot Nothing Then
            LoadMeasurementUnits()
            If m_selDataSource IsNot Nothing Then
                For Each strItem As String In CboUnits.Items
                    If m_selDataSource.MeasurementUnitType = MeasurementUnitType.Slope And _
                        strItem = BA_EnumDescription(m_selDataSource.SlopeUnit) Then
                        CboUnits.SelectedItem = strItem
                    ElseIf strItem = m_selDataSource.MeasurementUnit.ToString Then
                        CboUnits.SelectedItem = strItem
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub UpdateMeasurementUnits()
        Dim inputFolder As String = Nothing
        Dim inputFile As String = Nothing
        If String.IsNullOrEmpty(m_aoiPath) Then
            'This is a public data source
            inputFolder = "PleaseReturn"
            inputFile = BA_GetBareName(m_selDataSource.Source, inputFolder)
        Else
            'Otherwise it's a local data source
            inputFolder = BA_GetDataBinPath(m_aoiPath)
            inputFile = m_selDataSource.Source
        End If
        Dim tagsList As IList(Of String) = BA_ReadMetaData(inputFolder, inputFile, _
                                                           m_selDataSource.LayerType, _
                                                           BA_XPATH_TAGS)
        'Check to see if we need to upgrade the metadata
        If tagsList IsNot Nothing AndAlso tagsList.Count = 0 Then
            Dim fgdcList As IList(Of String) = BA_ReadMetaData(inputFolder, inputFile, _
                                                           m_selDataSource.LayerType, _
                                                           BA_XPATH_METSTDN)
            If fgdcList IsNot Nothing Then
                For Each innerText As String In fgdcList
                    If innerText.IndexOf("FGDC") > -1 Then
                        'We have older FGDC metadata to upgrade
                        Dim success As BA_ReturnCode = BA_UpgradeMetadata(inputFolder, inputFile)
                        If success = BA_ReturnCode.Success Then
                            tagsList = BA_ReadMetaData(inputFolder, inputFile, _
                                                           m_selDataSource.LayerType, _
                                                           BA_XPATH_TAGS)
                        End If
                    End If
                Next
            End If
        End If
        'We have existing tags at "/metadata/dataIdInfo/searchKeys/keyword"
        If tagsList IsNot Nothing AndAlso tagsList.Count > 0 Then
            Dim updateBagisTag As Boolean = False
            For Each pInnerText As String In tagsList
                'We have an existing BAGIS tag
                If pInnerText.IndexOf(BA_BAGIS_TAG_PREFIX) = 0 Then
                    'Extract inner tags
                    Dim finalLength As Integer = pInnerText.Length - BA_BAGIS_TAG_PREFIX.Length - BA_BAGIS_TAG_SUFFIX.Length
                    Dim innerText As String = ""
                    If finalLength > 0 Then
                        innerText = pInnerText.Substring(BA_BAGIS_TAG_PREFIX.Length, finalLength)
                    End If
                    Dim pContents As String() = innerText.Split(";")
                    If CkUnits.Checked Then
                        'We need to record the units in the tag
                        Dim updateCategory As Boolean = False
                        Dim updateValue As Boolean = False
                        Dim i As Integer = 0
                        For Each pString As String In pContents
                            'The zUnit category
                            If pString.IndexOf(BA_ZUNIT_CATEGORY_TAG) > -1 Then
                                'Overwrite existing value if there is one
                                pContents(i) = BA_ZUNIT_CATEGORY_TAG & m_selDataSource.MeasurementUnitType.ToString & ";"
                                updateCategory = True
                            ElseIf pString.IndexOf(BA_ZUNIT_VALUE_TAG) > -1 Then
                                'Overwrite existing value if there is one
                                If m_selDataSource.MeasurementUnitType = MeasurementUnitType.Slope Then
                                    pContents(i) = BA_ZUNIT_VALUE_TAG & BA_EnumDescription(m_selDataSource.SlopeUnit) & ";"
                                Else
                                    pContents(i) = BA_ZUNIT_VALUE_TAG & m_selDataSource.MeasurementUnit.ToString & ";"
                                End If
                                updateValue = True
                            End If
                            i += 1
                        Next
                        If updateCategory = False Then
                            System.Array.Resize(pContents, pContents.Length + 1)
                            'Put the category in the old last position
                            pContents(pContents.GetUpperBound(0)) = BA_ZUNIT_CATEGORY_TAG & m_selDataSource.MeasurementUnitType.ToString & ";"
                        End If
                        If updateValue = False Then
                            System.Array.Resize(pContents, pContents.Length + 1)
                            If m_selDataSource.MeasurementUnitType = MeasurementUnitType.Slope Then
                                'Put the value in the old last position
                                pContents(pContents.GetUpperBound(0)) = BA_ZUNIT_VALUE_TAG & BA_EnumDescription(m_selDataSource.SlopeUnit) & ";"
                            Else
                                'Put the value in the old last position
                                pContents(pContents.GetUpperBound(0)) = BA_ZUNIT_VALUE_TAG & m_selDataSource.MeasurementUnit.ToString & ";"
                            End If
                        End If
                    Else
                        'We need to remove the units from the tag if they are there
                        'Check first to make sure there is something to remove
                        If pContents.Length > 0 Then
                            Dim contentsList As List(Of String) = New List(Of String)
                            For Each pString As String In pContents
                                'Only add the innerText if it isn't unit-related
                                If pString.IndexOf(BA_ZUNIT_CATEGORY_TAG) < 0 And _
                                    pString.IndexOf(BA_ZUNIT_VALUE_TAG) < 0 Then
                                    contentsList.Add(pString)
                                End If
                            Next
                            pContents = contentsList.ToArray
                        End If
                    End If
                    'Reassemble the updated innerText
                    Dim sb As StringBuilder = New StringBuilder()
                    sb.Append(BA_BAGIS_TAG_PREFIX)
                    For Each pString As String In pContents
                        If Not String.IsNullOrEmpty(pString) Then
                            sb.Append(pString & " ")
                        End If
                    Next
                    'Trim off trailing space
                    Dim newInnerText As String = sb.ToString
                    newInnerText = newInnerText.Remove(Len(newInnerText) - 1, 1)
                    newInnerText = newInnerText & BA_BAGIS_TAG_SUFFIX
                    BA_UpdateMetadata(inputFolder, inputFile, m_selDataSource.LayerType, _
                                      BA_XPATH_TAGS, newInnerText, BA_BAGIS_TAG_PREFIX.Length)
                    updateBagisTag = True
                End If
            Next
            'We had existing "keyword" tags but no BAGIS tag; Need to add
            If updateBagisTag = False Then
            Dim bagisTag As String = CreateBagisTag()
                BA_UpdateMetadata(inputFolder, inputFile, m_selDataSource.LayerType, _
                    BA_XPATH_TAGS, bagisTag, BA_BAGIS_TAG_PREFIX.Length)
            End If
        Else
            'We need to add a new tag at "/metadata/dataIdInfo/searchKeys/keyword"
            Dim bagisTag As String = CreateBagisTag()
            BA_UpdateMetadata(inputFolder, inputFile, m_selDataSource.LayerType, _
                BA_XPATH_TAGS, bagisTag, BA_BAGIS_TAG_PREFIX.Length)
        End If

    End Sub

    Private Function CreateBagisTag() As String
        Dim sb As StringBuilder = New StringBuilder
        sb.Append(BA_BAGIS_TAG_PREFIX)
        sb.Append(BA_ZUNIT_CATEGORY_TAG & m_selDataSource.MeasurementUnitType.ToString & "; ")
        If m_selDataSource.MeasurementUnitType = MeasurementUnitType.Slope Then
            sb.Append(BA_ZUNIT_VALUE_TAG & BA_EnumDescription(m_selDataSource.SlopeUnit) & ";")
        Else
            sb.Append(BA_ZUNIT_VALUE_TAG & m_selDataSource.MeasurementUnit.ToString & ";")
        End If
        sb.Append(BA_BAGIS_TAG_SUFFIX)
        Return sb.ToString
    End Function
End Class