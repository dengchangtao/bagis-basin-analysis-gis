Imports System.Windows.Forms
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Desktop.AddIns
Imports ESRI.ArcGIS.esriSystem
Imports System.IO
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.SpatialAnalyst
Imports BAGIS_ClassLibrary

Public Class FrmClustering
    Dim m_aoi As Aoi
    Dim m_version As String

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        BtnViewAoi.Enabled = False
        BtnClearSelected.Enabled = False
        BtnCalIsoCluster.Enabled = False

        ' Set AOI if already set
        Dim hruExt As HruExtension = HruExtension.GetExtension
        Dim aoi As Aoi = hruExt.aoi
        If aoi IsNot Nothing Then
            m_aoi = aoi
            TxtAoiPath.Text = m_aoi.FilePath
            LoadLstLayers()
        End If
        TxtSample.Text = 10
        TxtSize.Text = 20
        TxtIterate.Text = 20
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

            'check AOI status
            Dim ReturnString As String = BA_Check_Folder_Type(DataPath, BA_AOI_TYPE)
            If ReturnString Is Nothing Then
                MessageBox.Show("The selected folder does not contain a valid AOI!")
                Exit Sub
            Else
                'BA_SetDefaultProjection(My.ArcMap.Application)
                Dim ReturnPath As String = "PleaseReturn"
                'Choose to use supplied DataPath here rather than ReturnString generated from source.weasel
                Dim aoiName As String = BA_GetBareName(DataPath, ReturnPath)
                Dim aoiFolder As String = DataPath
                If BA_ContainsSpace(aoiFolder) Or BA_ContainsSpace(aoiName) Then
                    MessageBox.Show("The aoi folder and name cannot contain spaces.")
                    Exit Sub
                End If
                m_aoi = New Aoi(aoiName, aoiFolder, Nothing, m_version)
                TxtAoiPath.Text = m_aoi.FilePath


                Dim comboBox = AddIn.FromID(Of CboSelectedAoi)(My.ThisAddIn.IDs.CboSelectedAoi)
                If comboBox IsNot Nothing Then
                    comboBox.setValue(aoiName)
                    LoadLstLayers()

                    'Set current aoi in comboBox on Toolbarme)
                End If
                hruExt.aoi = m_aoi

                Show()
            End If
        Catch ex As Exception
            MessageBox.Show("COMException: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadLstLayers()
        Dim RasterCount As Long
        Dim i As Long

        ' Get the count of zone directories so we know how many steps to put into the StepProgressor
        ' Create a DirectoryInfo of the HRU directory.
        Dim zonesDirectory As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, Nothing)
        Dim dirZones As New DirectoryInfo(zonesDirectory)
        Dim dirZonesArr As DirectoryInfo() = Nothing
        Dim zoneCount As Integer
        If dirZones.Exists Then
            dirZonesArr = dirZones.GetDirectories
            If dirZonesArr IsNot Nothing Then zoneCount = dirZonesArr.Length
        End If

        ' Create/configure a step progressor
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 8 + zoneCount)
        pStepProg.Show()
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Loading layer lists", "Loading...")

        Try
            Dim bTime As DateTime = DateTime.Now
            LstAoiRasterLayers.Items.Clear()
            LstDemLayers.Items.Clear()
            LstPrismLayers.Items.Clear()
            'LstHruLayers.Items.Clear()
            'LstSelectedZones.Items.Clear()
            progressDialog2.ShowDialog()
            pStepProg.Step()
            Dim AOIVectorList() As String = Nothing
            Dim AOIRasterList() As String = Nothing
            Dim layerPath As String = BA_GetPath(m_aoi.FilePath, PublicPath.Layers)
            BA_ListLayersinAOI(layerPath, AOIRasterList, AOIVectorList)
            Dim ts As TimeSpan = DateTime.Now.Subtract(bTime)
            Debug.Print("ts1: " & ts.Milliseconds)
            bTime = DateTime.Now
            pStepProg.Step()

            'display raster layers
            RasterCount = UBound(AOIRasterList)
            If RasterCount > 0 Then
                For i = 1 To RasterCount
                    Dim fullLayerPath As String = layerPath & "\" & AOIRasterList(i)
                    Dim isDiscrete As Boolean = BA_IsIntegerRaster(fullLayerPath)
                    Dim item As LayerListItem = New LayerListItem(AOIRasterList(i), fullLayerPath, LayerType.Raster, isDiscrete)
                    LstAoiRasterLayers.Items.Add(item)
                Next
            End If
            ts = DateTime.Now.Subtract(bTime)
            Debug.Print("ts3: " & ts.Milliseconds)
            bTime = DateTime.Now
            pStepProg.Step()

            'display dem layers
            LoadDemLayers()
            ts = DateTime.Now.Subtract(bTime)
            Debug.Print("ts4: " & ts.Milliseconds)
            bTime = DateTime.Now
            pStepProg.Step()

            'display prism layers
            LoadPrismLayers()
            ts = DateTime.Now.Subtract(bTime)
            Debug.Print("ts5: " & ts.Milliseconds)
            bTime = DateTime.Now
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
        Dim layerPath As String = BA_GetPath(m_aoi.FilePath, PublicPath.DEM)
        Dim item As LayerListItem
        ' Filled DEM
        Dim fullLayerPath As String = layerPath & "\" & BA_EnumDescription(MapsFileName.filled_dem)
        Dim isDiscrete As Boolean = BA_IsIntegerRaster(fullLayerPath)
        If BA_File_ExistsIDEUtil(fullLayerPath) Then
            item = New LayerListItem(BA_EnumDescription(MapsLayerName.filled_dem), fullLayerPath, LayerType.Raster, isDiscrete)
            LstDemLayers.Items.Add(item)
        End If
        ' Original DEM
        layerPath = BA_GetPath(m_aoi.FilePath, PublicPath.SourceDEM)
        fullLayerPath = layerPath & "\" & BA_EnumDescription(MapsFileName.dem)
        isDiscrete = BA_IsIntegerRaster(fullLayerPath)
        If BA_File_ExistsIDEUtil(fullLayerPath) Then
            item = New LayerListItem(BA_EnumDescription(MapsLayerName.dem), fullLayerPath, LayerType.Raster, isDiscrete)
            LstDemLayers.Items.Add(item)
        End If
        ' Flow Direction
        layerPath = BA_GetPath(m_aoi.FilePath, PublicPath.FlowDirection)
        fullLayerPath = layerPath & "\" & BA_EnumDescription(MapsFileName.flow_direction)
        isDiscrete = BA_IsIntegerRaster(fullLayerPath)
        If BA_File_ExistsIDEUtil(fullLayerPath) Then
            item = New LayerListItem(BA_EnumDescription(MapsLayerName.flow_direction), fullLayerPath, LayerType.Raster, isDiscrete)
            LstDemLayers.Items.Add(item)
        End If
        ' Flow Accumulation
        layerPath = BA_GetPath(m_aoi.FilePath, PublicPath.FlowAccumulation)
        fullLayerPath = layerPath & "\" & BA_EnumDescription(MapsFileName.flow_accumulation)
        isDiscrete = BA_IsIntegerRaster(fullLayerPath)
        If BA_File_ExistsIDEUtil(fullLayerPath) Then
            item = New LayerListItem(BA_EnumDescription(MapsLayerName.flow_accumulation), fullLayerPath, LayerType.Raster, isDiscrete)
            LstDemLayers.Items.Add(item)
        End If
        ' Slope
        layerPath = BA_GetPath(m_aoi.FilePath, PublicPath.Slope)
        fullLayerPath = layerPath & "\" & BA_EnumDescription(MapsFileName.slope)
        isDiscrete = BA_IsIntegerRaster(fullLayerPath)
        If BA_File_ExistsIDEUtil(fullLayerPath) Then
            item = New LayerListItem(BA_EnumDescription(MapsLayerName.slope), fullLayerPath, LayerType.Raster, isDiscrete)
            LstDemLayers.Items.Add(item)
        End If
        ' Aspect
        layerPath = BA_GetPath(m_aoi.FilePath, PublicPath.Aspect)
        fullLayerPath = layerPath & "\" & BA_EnumDescription(MapsFileName.aspect)
        isDiscrete = BA_IsIntegerRaster(fullLayerPath)
        If BA_File_ExistsIDEUtil(fullLayerPath) Then
            item = New LayerListItem(BA_EnumDescription(MapsLayerName.aspect), fullLayerPath, LayerType.Raster, isDiscrete)
            LstDemLayers.Items.Add(item)
        End If
        ' Hillshade
        layerPath = BA_GetPath(m_aoi.FilePath, PublicPath.Hillshade)
        fullLayerPath = layerPath & "\" & BA_EnumDescription(MapsFileName.hillshade)
        isDiscrete = BA_IsIntegerRaster(fullLayerPath)
        If BA_File_ExistsIDEUtil(fullLayerPath) Then
            item = New LayerListItem(BA_EnumDescription(MapsLayerName.hillshade), fullLayerPath, LayerType.Raster, isDiscrete)
            LstDemLayers.Items.Add(item)
        End If
        ' Pourpoint
        fullLayerPath = m_aoi.FilePath & "\" & BA_EnumDescription(MapsFileName.PourPoint)
        isDiscrete = True
        If BA_File_ExistsIDEUtil(fullLayerPath & ".shp") Then
            item = New LayerListItem(BA_EnumDescription(MapsLayerName.Pourpoint), fullLayerPath, LayerType.Vector, isDiscrete)
            LstDemLayers.Items.Add(item)
        End If

    End Sub

    Private Sub LoadPrismLayers()
        Dim item As LayerListItem
        Dim strPath As String = BA_GetPath(m_aoi.FilePath, PublicPath.PRISM)
        Dim layerCount As Short = [Enum].GetValues(GetType(AOIPrismFolderNames)).Length
        Dim i = 1
        Do Until i > layerCount
            Dim nextFolder As String = BA_GetPrismFolderName(i)
            Dim prismPath = strPath & "\" & nextFolder & "\" & BA_EnumDescription(MapsFileName.Prism)
            'Dim isDiscrete As Boolean = BA_IsIntegerRaster(prismPath)
            'For performance, we assume that prism layers are not discrete
            If BA_File_ExistsIDEUtil(prismPath) Then
                item = New LayerListItem(nextFolder, prismPath, LayerType.Raster, False)
                LstPrismLayers.Items.Add(item)
            End If
            i += 1
        Loop
    End Sub

    Private Sub BtnCalIsoCluster_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCalIsoCluster.Click
        If ValidateTxtSignature(TxtSignature.Text) = False Then
            MessageBox.Show("Invalid output signature file name. The file name must end in .gsg.", "Invalid file name", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            TxtSignature.Focus()
            Exit Sub
        End If

        ' Create/configure a step progressor
        Dim stepCount As Integer = 5
        Dim pStepProg As IStepProgressor = Nothing
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = Nothing

        BtnCalIsoCluster.Enabled = False
        Dim GP As ESRI.ArcGIS.Geoprocessor.Geoprocessor = New ESRI.ArcGIS.Geoprocessor.Geoprocessor()
        Dim tool As ESRI.ArcGIS.SpatialAnalystTools.IsoCluster = New ESRI.ArcGIS.SpatialAnalystTools.IsoCluster()
        Dim rasterBands As String = ""
        Try
            pStepProg = BA_GetStepProgressor(My.ArcMap.Application.hWnd, stepCount)
            ' Create/configure the ProgressDialog. This automatically displays the dialog
            progressDialog2 = BA_GetProgressDialog(pStepProg, "Running isocluster algorithm...", Me.Text)
            pStepProg.Step()

            For idx = 0 To LstAoiRasterLayers.SelectedItems.Count - 1
                rasterBands = rasterBands & LstAoiRasterLayers.SelectedItems(idx).value
                rasterBands = rasterBands & "; "
            Next
            For idx = 0 To LstDemLayers.SelectedItems.Count - 1
                rasterBands = rasterBands & LstDemLayers.SelectedItems(idx).value
                rasterBands = rasterBands & "; "
            Next
            For idx = 0 To LstPrismLayers.SelectedItems.Count - 1
                rasterBands = rasterBands & LstPrismLayers.SelectedItems(idx).value
                rasterBands = rasterBands & "; "
            Next
            ' Set default workspace
            GP.SetEnvironmentValue("workspace", m_aoi.FilePath)
            GP.OverwriteOutput = True

            tool.in_raster_bands = rasterBands
            tool.out_signature_file = TxtSignature.Text
            tool.number_classes = TxtClass.Text
            tool.number_iterations = TxtIterate.Text
            tool.min_class_size = TxtSize.Text
            tool.sample_interval = TxtSample.Text
            pStepProg.Step()

            GP.Execute(tool, Nothing)
            progressDialog2.HideDialog()

            MessageBox.Show(TxtSignature.Text & " has been created.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
            ResetForm(sender, e)
        Catch ex As Exception
            If GP.MessageCount > 0 Then
                MessageBox.Show("Geoprocessor error: " + GP.GetMessages(2))
            Else
                MessageBox.Show("Exception: " + ex.Message)
            End If
        Finally
            ' Clean up step progressor
            If progressDialog2 IsNot Nothing Then
                progressDialog2.HideDialog()
            End If
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStepProg)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(progressDialog2)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(tool)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(GP)
        End Try
    End Sub

    Private Sub BtnViewAoi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnViewAoi.Click
        Dim fileNamesWithStyle As List(Of String) = BA_ListOfLayerNamesWithStyles()
        ' Display raster layers
        If LstAoiRasterLayers.SelectedIndex > -1 Then
            Dim items As IList = LstAoiRasterLayers.SelectedItems
            For Each item As LayerListItem In items
                If fileNamesWithStyle.IndexOf(item.Name) > -1 Then
                    Dim symbology As BA_Map_Symbology = BA_GetRasterMapSymbology(item.Name)
                    BA_DisplayRasterWithSymbol(My.ArcMap.Document, item.Value, symbology.DisplayName, _
                                               symbology.DisplayStyle, symbology.Transparency, WorkspaceType.Raster)
                Else
                    BA_DisplayRaster(My.ArcMap.Application, item.Value)
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
                    Dim strFileName As String = BA_GetBareName(item.Value)
                    If fileNamesWithStyle.IndexOf(strFileName) > -1 Then
                        Dim symbology As BA_Map_Symbology = BA_GetPointMapSymbology(strFileName)
                        BA_MapDisplayPointMarkers(My.ArcMap.Application, item.Value, symbology.DisplayName, symbology.Color, symbology.MarkerType)
                    Else
                        BA_DisplayVector(My.ArcMap.Document, item.Value)
                    End If
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

    End Sub

    Private Sub BtnClearSelected_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClearSelected.Click
        LstAoiRasterLayers.SelectedIndex = -1
        LstPrismLayers.SelectedIndex = -1
        LstDemLayers.SelectedIndex = -1
    End Sub

    Private Sub ManageAoiLayerButtons()
        Dim lCount As Integer = LstAoiRasterLayers.SelectedItems.Count + _
                                LstDemLayers.SelectedItems.Count + _
                                LstPrismLayers.SelectedItems.Count

        If lCount > 0 Then
            BtnViewAoi.Enabled = True
            BtnClearSelected.Enabled = True
        Else
            BtnViewAoi.Enabled = False
            BtnClearSelected.Enabled = False
            BtnCalIsoCluster.Enabled = False
        End If

        If TxtClass.Text <> "" And _
           TxtIterate.Text <> "" And _
           TxtSample.Text <> "" And _
           TxtSignature.Text <> "" And _
           TxtSize.Text <> "" Then
            If lCount > 0 Then
                BtnCalIsoCluster.Enabled = True
            End If
        Else
            BtnCalIsoCluster.Enabled = False
        End If

    End Sub

    Private Sub LstAoiRasterLayers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstAoiRasterLayers.SelectedIndexChanged
        ManageAoiLayerButtons()
    End Sub

    Private Sub LstDemLayers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstDemLayers.SelectedIndexChanged
        ManageAoiLayerButtons()
    End Sub

    Private Sub LstPrismLayers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstPrismLayers.SelectedIndexChanged
        ManageAoiLayerButtons()
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub TxtClass_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtClass.TextChanged
        ManageAoiLayerButtons()
    End Sub

    Private Sub TxtIterate_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtIterate.TextChanged
        ManageAoiLayerButtons()
    End Sub

    Private Sub TxtSize_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtSize.TextChanged
        ManageAoiLayerButtons()
    End Sub

    Private Sub TxtSample_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtSample.TextChanged
        ManageAoiLayerButtons()
    End Sub

    Private Sub TxtSignature_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtSignature.TextChanged
        TxtOutputPath.Text = m_aoi.FilePath & "\" & TxtSignature.Text
        ManageAoiLayerButtons()
    End Sub

    Private Sub BtnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAbout.Click
        Dim toolHelpForm As FrmHelp = New FrmHelp(BA_HelpTopics.IsoClustering)
        toolHelpForm.ShowDialog()
    End Sub

    ' Validate that the extension of the txtSignature entry is '.gsg'
    Private Function ValidateTxtSignature(ByRef strSignature As String) As Boolean
        strSignature = strSignature.Trim
        Dim strValid As String = BA_GetBareName(strSignature)
        If Microsoft.VisualBasic.Strings.Right(strValid, 4).ToLower = ".gsg" Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub ResetForm(ByVal sender As System.Object, ByVal e As System.EventArgs)
        BtnClearSelected_Click(sender, e)
        TxtClass.Text = Nothing
        TxtSignature.Text = Nothing
        TxtOutputPath.Text = Nothing
        ManageAoiLayerButtons()
    End Sub
End Class