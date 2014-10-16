Imports System.Windows.Forms
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.SpatialAnalyst
Imports ESRI.ArcGIS.Geoprocessing
Imports ESRI.ArcGIS.GeoAnalyst
Imports BAGIS_ClassLibrary
Imports System.IO

Public Class FrmHruContribAreaRule
    Private m_aoiPath As String
    Private m_threshold As Double 'threshold for creating stream links
    Private m_areatype As Integer '1, 2, or 3
    Private m_mean As Double
    Private Const BA_AOIStreamShapefile As String = "CA_Streamlinks"

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Dim shapefpath As String = BA_GetPath(m_aoiPath, PublicPath.Layers)
        Dim response As Integer

        'check if previous output exist
        If BA_Shapefile_Exists(shapefpath & "\" & BA_AOIStreamShapefile) Then
            response = MessageBox.Show("Do you want to keep the preview streamlinks shapefile?", "Contributing Area", MessageBoxButtons.YesNo)
            If response = vbNo Then
                RemovePreviewShapefile()
            End If
        End If

        Me.Close()
    End Sub

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Dim hruExt As HruExtension = HruExtension.GetExtension
        Dim aoi As Aoi = hruExt.aoi
        If aoi IsNot Nothing Then m_aoiPath = aoi.FilePath
        Dim RasterRes As Double
        Dim DemFilePath As String = BA_GeodatabasePath(aoi.FilePath, GeodatabaseNames.Surfaces, True) & BA_EnumDescription(MapsFileName.flow_accumulation_gdb)
        'Dim DemFilePath As String = BA_GetPath(m_aoiPath, PublicPath.FlowAccumulation)
        Dim pRasterStats As ESRI.ArcGIS.DataSourcesRaster.IRasterStatistics
        'pRasterStats = BA_GetRasterStats(DemFilePath & "\" & BA_EnumDescription(MapsFileName.flow_direction), RasterRes)
        pRasterStats = BA_GetRasterStatsGDB(DemFilePath, RasterRes)
        If pRasterStats IsNot Nothing Then
            LblMinFlowValue.Text = Format(pRasterStats.Minimum, "#0")
            LblMaxFlowValue.Text = Format(pRasterStats.Maximum, "#0")
            LblMeanValue.Text = Format(pRasterStats.Mean, "#0.000")
            LblStDevValue.Text = Format(pRasterStats.StandardDeviation, "#0.000")
            m_mean = pRasterStats.Mean
            TxtThreshold.Text = Math.Round(pRasterStats.StandardDeviation).ToString
        End If

    End Sub

    Public Sub LoadForm(ByVal pRule As BAGIS_ClassLibrary.IRule)
        Dim ContributingAreaRule As ContributingAreasRule = CType(pRule, ContributingAreasRule)
        TxtThreshold.Text = ContributingAreaRule.FACCThresholdValue
        Select Case ContributingAreaRule.NumberofArea
            Case 2
                RdoTwoArea.Checked = True
            Case 3
                RdoThreeArea.Checked = True
            Case Else '1
                RdoOneArea.Checked = True
        End Select

        TxtRuleID.Text = CStr(ContributingAreaRule.RuleId)
        ChkKeepTempFiles.Checked = ContributingAreaRule.KeepTemporaryFiles
    End Sub

    Private Sub BtnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAbout.Click
        Dim toolHelpForm As FrmHelp = New FrmHelp(BA_HelpTopics.ContributingArea)
        toolHelpForm.ShowDialog()
    End Sub

    Private Sub BtnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSave.Click
        'remove the temporary streamlink preview shapefile
        'save output shapefile
        'Dim shapefpath As String = BA_GetPath(m_aoiPath, PublicPath.Layers)
        Dim shapefpath As String = BA_GeodatabasePath(m_aoiPath, GeodatabaseNames.Layers, True)
        Dim response As Integer

        'check if previous output exist
        'If BA_Shapefile_Exists(shapefpath & "\" & BA_AOIStreamShapefile) Then
        If BA_File_Exists(shapefpath, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            response = MessageBox.Show("Do you want to keep the preview streamlinks shapefile?", "Contributing Area", MessageBoxButtons.YesNo)
            If response = vbNo Then
                RemovePreviewShapefile()
            End If
        End If

        'validate input threshold value
        Dim FACCThreshold As Double = Val(TxtThreshold.Text) 'unit in stdev
        If FACCThreshold <= 0 Then
            MessageBox.Show("Invalid threshold value. Please re-enter.")
            Exit Sub
        End If

        ' Increment ruleId before using
        Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of frmHruZone.AddinImpl)(My.ThisAddIn.IDs.frmHruZone)
        Dim hruZoneForm As frmHruZone = dockWindowAddIn.UI

        Dim ruleId As Integer
        If String.IsNullOrEmpty(TxtRuleID.Text) Then
            ruleId = hruZoneForm.GetNextRuleId
        Else
            ruleId = CInt(TxtRuleID.Text)
        End If

        Dim numberofArea As Integer = 1
        If RdoTwoArea.Checked Then
            numberofArea = 2
        ElseIf RdoThreeArea.Checked Then
            numberofArea = 3
        Else
            numberofArea = 1
        End If

        'Commenting out. Don't see this variable used anywhere below. LCB
        'Dim inputLayerPath As String = m_aoiPath & "\" & BA_EnumDescription(AOIClipFile.AOIExtentCoverage)
        'Dim inputLayerPath As String = BA_GeodatabasePath(m_aoiPath, GeodatabaseNames.Aoi, True) & BA_EnumDescription(AOIClipFile.AOIExtentCoverage)

        'Flow accumulation layer is the input layer
        Dim thisContributingAreasRule = _
            New ContributingAreasRule(m_aoiPath, _
            FACCThreshold, m_mean, numberofArea, ChkKeepTempFiles.Checked, ruleId)

        hruZoneForm.AddPendingRule(thisContributingAreasRule)
        BtnCancel_Click(sender, e)
    End Sub

    Private Sub BtnPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnPreview.Click
        'determine threshold value
        Dim FACCThreshold As Double = Val(TxtThreshold.Text)
        If FACCThreshold <= 0 Then
            MessageBox.Show("Invalid threshold value. Please re-enter.")
            Exit Sub
        End If

        Dim response As Integer
        Dim pDEUtility As IDEUtilities = New DEUtilities

        'Use the AOI extent to limit the process extent
        Dim pAOIRaster As IGeoDataset
        'Open AOI Polygon to set the analysis mask
        Dim rasterFileName As String = BA_EnumDescription(PublicPath.AoiGrid)
        rasterFileName = BA_StandardizeShapefileName(rasterFileName, False, False)
        'pAOIRaster = BA_OpenRasterFromFile(m_aoiPath, rasterFileName)
        pAOIRaster = BA_OpenRasterFromGDB(BA_GeodatabasePath(m_aoiPath, GeodatabaseNames.Aoi), rasterFileName)
        If pAOIRaster Is Nothing Then
            MsgBox("Cannot locate AOI boundary raster in the AOI.  Please re-run AOI Tool.")
            Exit Sub
        End If

        'open flow_accum and flow direction raster
        Dim rasterfilepath As String
        rasterFileName = BA_EnumDescription(MapsFileName.flow_accumulation_gdb)
        'rasterFileName = BA_StandardizeShapefileName(rasterFileName, False, False)
        'rasterfilepath = BA_GetPath(m_aoiPath, PublicPath.FlowAccumulation)
        rasterfilepath = BA_GeodatabasePath(m_aoiPath, GeodatabaseNames.Surfaces)

        'Dim pFAccGDS As IGeoDataset = BA_OpenRasterFromFile(rasterfilepath, rasterFileName)
        Dim pFAccGDS As IGeoDataset = BA_OpenRasterFromGDB(rasterfilepath, rasterFileName)
        Dim pFAccRasterDS As IRasterDataset = pFAccGDS 'qi
        Dim pFAccRaster As IRaster = pFAccRasterDS.CreateDefaultRaster

        rasterFileName = BA_EnumDescription(MapsFileName.flow_direction_gdb)
        'rasterFileName = BA_StandardizeShapefileName(rasterFileName, False, False)
        'rasterfilepath = BA_GetPath(m_aoiPath, PublicPath.FlowDirection)
        'Dim pFDirGDS As IGeoDataset = BA_OpenRasterFromFile(rasterfilepath, rasterFileName)
        Dim pFDirGDS As IGeoDataset = BA_OpenRasterFromGDB(rasterfilepath, rasterFileName)

        'Run Extraction Operation
        Dim pExtractOp As IExtractionOp2 = New RasterExtractionOp
        Dim pTempRaster As IGeoDataset = pExtractOp.Raster(pFAccRaster, pAOIRaster)
        pAOIRaster = Nothing

        'Query the Raster and Create a new dataset
        Dim pQFilter As IQueryFilter = New QueryFilter
        pQFilter.WhereClause = "Value > " & FACCThreshold

        Dim pRasDes As IRasterDescriptor = New RasterDescriptor
        pRasDes.Create(pTempRaster, pQFilter, "Value")

        Dim pStreamGDS As IGeoDataset = pExtractOp.Attribute(pRasDes)

        Dim pHydrologyOp As IHydrologyOp = New RasterHydrologyOp
        Dim pStreamlinkGDS As IGeoDataset = pHydrologyOp.StreamLink(pStreamGDS, pFDirGDS)

        'save output shapefile
        'Dim shapefpath As String = BA_GetPath(m_aoiPath, PublicPath.Layers)
        Dim shapefpath As String = BA_GeodatabasePath(m_aoiPath, GeodatabaseNames.Layers)
        'check if output exist
        RemovePreviewShapefile()
        response = BA_Raster2LineShapefile(pStreamlinkGDS, shapefpath, BA_AOIStreamShapefile, False)

        pHydrologyOp = Nothing
        pRasDes = Nothing
        pQFilter = Nothing
        pExtractOp = Nothing
        pFAccGDS = Nothing
        pFAccRasterDS = Nothing
        pFAccRaster = Nothing
        pFDirGDS = Nothing
        pStreamGDS = Nothing
        pStreamlinkGDS = Nothing
        pTempRaster = Nothing

        If response <> BA_ReturnCode.Success Then
            MsgBox("Unable to create the output stream shapefile.")
            Exit Sub
        End If

        'Add stream to Map
        'set line color
        Dim pLColor As IColor
        pLColor = New RgbColor
        pLColor.RGB = RGB(0, 0, 255)
        response = BA_AddLineLayer(shapefpath & "\" & BA_AOIStreamShapefile, BA_AOIStreamShapefile, pLColor, 0)
        pLColor = Nothing

        pDEUtility.ReleaseInternals()
        Exit Sub
    End Sub

    'Private Sub RdoTwoArea_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RdoTwoArea.CheckedChanged
    '    If RdoOneArea.Checked = False Then
    '        MsgBox("Only 1 area option works at this point!")
    '    End If
    'End Sub

    Private Sub RemovePreviewShapefile()
        Dim shapefpath As String = BA_GeodatabasePath(m_aoiPath, GeodatabaseNames.Layers)
        'If BA_Shapefile_Exists(shapefpath & "\" & BA_AOIStreamShapefile) Then
        If BA_File_Exists(shapefpath & "\" & BA_AOIStreamShapefile, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            Dim response As Integer = BA_RemoveLayersInFolder(My.Document, shapefpath)
            'response = BA_Remove_Shapefile(shapefpath, BA_AOIStreamShapefile)
            response = BA_Remove_ShapefileFromGDB(shapefpath, BA_AOIStreamShapefile)
        End If
    End Sub

    Private Sub BtnTL_LUT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnTL_LUT.Click
        Dim LUT_Version_Keywords As String = "LUT FILE 1.1"
        Dim ThresholdLink_LUTFilename As String = "threshold_link_LUT.txt"
        'Dim LUTFile As String = BA_GetPath(m_aoiPath, PublicPath.FlowAccumulation) & "\" & ThresholdLink_LUTFilename
        Dim LUTFile As String = BA_GeodatabasePath(m_aoiPath, GeodatabaseNames.Surfaces, True) & ThresholdLink_LUTFilename
        Dim arrMean() As Double = Nothing
        Dim arrThreshold() As Long = Nothing
        Dim arrLinks() As Long = Nothing

        If BA_LoadStreamLinkCountLUT(m_aoiPath, arrMean, arrThreshold, arrLinks, LUT_Version_Keywords) <> BA_ReturnCode.Success Then

            If BA_StreamLinkCountLUT(m_aoiPath, LUT_Version_Keywords) = BA_ReturnCode.Success Then
                BA_LoadStreamLinkCountLUT(m_aoiPath, arrMean, arrThreshold, arrLinks, LUT_Version_Keywords)

            Else

                MsgBox("Unable to create the threshold look-up table!")
                Exit Sub

            End If

        End If

        Dim cellValues As Object() = Nothing
        Dim nrecords As Long = UBound(arrMean)
        If DataGridViewLUT.RowCount <= 0 Then
            For i As Long = 0 To nrecords
                cellValues = {arrThreshold(i), arrLinks(i)}

                DataGridViewLUT.Rows.Add(cellValues)
            Next
            BtnTL_LUT.Enabled = False
        End If
    End Sub

    Private Sub DataGridViewLUT_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridViewLUT.CellClick
        If e.RowIndex >= 0 Then TxtThreshold.Text = DataGridViewLUT.Item(0, e.RowIndex).Value
    End Sub
End Class