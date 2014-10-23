Imports ESRI.ArcGIS.esriSystem
Imports BAGIS_ClassLibrary
Imports System.Text
Imports System.Windows.Forms
Imports System.Drawing
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Carto

''' <summary>
''' Designer class of the dockable window add-in. It contains user interfaces that
''' make up the dockable window.
''' </summary>
Public Class frmSiteScenario
    Implements IDisposable

    Private DEMConversion_Factor As Double
    'this vaiable convert the Z unit between feet and meters
    'if DEM is in meters and user select meters as the displayed unit, then Conversion_Factor = 1
    'if DEM is in feet and user select feet as the displayed unit, then Conversion_Factor = 1
    'if DEM is in meters and user select feet as the displayed unit, then Conversion_Factor = 3.2808399
    'if DEM is in feet and user select meters as the displayed unit, then Conversion_Factor = 0.3048

    Private ES_DEMMin As Double
    Private ES_DEMMax As Double
    Private ES_STMin As Double
    Private ES_STMax As Double
    Private ES_SCMin As Double
    Private ES_SCMax As Double
    ' These default values were provided by Gus Goodbody in July 2013
    ' this is equal to 100 square mile station density recommendation for mountainous areas by World Meterological Organization)
    Private ES_Buffer As Double = 5.641895835
    Private m_buffer As Double
    Private ES_BufferUnits As esriUnits = esriUnits.esriMiles
    Private ES_ElevRange As Double = 500
    Private m_upperElevRange As Double
    Private m_lowerElevRange As Double
    Private ES_ElevRangeUnits As esriUnits = esriUnits.esriFeet
    Private m_demInMeters As Boolean
    Private m_demXYUnits As esriUnits = esriUnits.esriMeters
    Private m_oldBufferUnits As esriUnits
    Private m_oldElevUnits As esriUnits
    Private m_changingUnits As Boolean = False
    Protected Shared m_CurrentAOI As String = ""
    Private m_baseLayers As Boolean = False
    Private m_btnCalculateMessage As Boolean = True
    'Identify dgv column indexes
    Friend idxSelected As Integer = 0
    Friend idxObjectId As Integer = 1
    Friend idxSiteType As Integer = 2
    Friend idxSiteName As Integer = 3
    Friend idxElevation As Integer = 4
    Friend idxUpper As Integer = 5
    Friend idxLower As Integer = 6
    Friend idxDefaultElevation As Integer = 7

    Public Sub New(ByVal hook As Object)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Hook = hook

        If String.IsNullOrEmpty(AOIFolderBase) Then
            Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow
            Dim dockWinID As UID = New UIDClass()
            dockWinID.Value = My.ThisAddIn.IDs.frmSiteScenario
            dockWindow = My.ArcMap.DockableWindowManager.GetDockableWindow(dockWinID)
            dockWindow.Show(False)
        End If
    End Sub

    Private m_hook As Object
    ''' <summary>
    ''' Host object of the dockable window
    ''' </summary> 
    Public Property Hook() As Object
        Get
            Return m_hook
        End Get
        Set(ByVal value As Object)
            m_hook = value
        End Set
    End Property


    ''' <summary>
    ''' Implementation class of the dockable window add-in. It is responsible for
    ''' creating and disposing the user interface class for the dockable window.
    ''' </summary>
    Public Class AddinImpl
        Inherits ESRI.ArcGIS.Desktop.AddIns.DockableWindow

        Private m_windowUI As frmSiteScenario

        Protected Overrides Function OnCreateChild() As System.IntPtr
            m_windowUI = New frmSiteScenario(Me.Hook)
            Return m_windowUI.Handle
        End Function

        Protected Overrides Sub Dispose(ByVal Param As Boolean)
            If m_windowUI IsNot Nothing Then
                m_windowUI.Dispose(Param)
            End If

            MyBase.Dispose(Param)
        End Sub

        Protected Friend ReadOnly Property UI() As frmSiteScenario
            Get
                Return m_windowUI
            End Get
        End Property

        Protected Friend ReadOnly Property Ready(ByVal matchString As String) As Boolean
            Get
                If matchString = m_CurrentAOI Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property
    End Class

    Private Sub BtnAddAll_Click(sender As System.Object, e As System.EventArgs) Handles BtnAddAll.Click
        Try
            For Each nextRow As DataGridViewRow In GrdScenario1.Rows
                Dim oid As Integer = Convert.ToInt32(nextRow.Cells(idxObjectId).Value)
                Dim siteType As String = Convert.ToString(nextRow.Cells(idxSiteType).Value)
                If Not AlreadyInGrid(siteType, oid) Then
                    Dim item As New DataGridViewRow
                    item.CreateCells(GrdScenario2)
                    With item
                        .Cells(idxObjectId - 1).Value = oid
                        .Cells(idxSiteType - 1).Value = siteType
                        .Cells(idxSiteName - 1).Value = nextRow.Cells(idxSiteName).Value
                        .Cells(idxElevation - 1).Value = nextRow.Cells(idxElevation).Value
                        .Cells(idxUpper - 1).Value = nextRow.Cells(idxUpper).Value
                        .Cells(idxLower - 1).Value = nextRow.Cells(idxLower).Value
                        .Cells(idxDefaultElevation - 1).Value = nextRow.Cells(idxDefaultElevation).Value
                    End With
                    '---add the row---
                    GrdScenario2.Rows.Add(item)
                End If
            Next
        Catch ex As Exception
            Debug.Print("BtnAddAll_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub BtnClose_Click(sender As System.Object, e As System.EventArgs) Handles BtnClose.Click
        Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow
        Dim dockWinID As UID = New UIDClass()
        dockWinID.Value = My.ThisAddIn.IDs.frmSiteScenario
        dockWindow = My.ArcMap.DockableWindowManager.GetDockableWindow(dockWinID)
        dockWindow.Show(False)
    End Sub

    Public Sub LoadAOIInfo(ByVal aoiPath As String)
        ' Add any initialization after the InitializeComponent() call.
        If Len(AOIFolderBase) = 0 Then
            MsgBox("Please select an AOI first!")
            Exit Sub
        End If

        CmboxDistanceUnit.Items.Clear()
        CmboxDistanceUnit.Items.Add("Feet")
        CmboxDistanceUnit.Items.Add("Meters")
        CmboxDistanceUnit.Items.Add("Miles")
        CmboxDistanceUnit.Items.Add("Km")

        Dim filepath As String, FileName As String, filenamepath As String

        filepath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True)
        FileName = BA_ScenarioResult
        filenamepath = filepath & FileName

        'get internal system unit for the selected AOI, i.e., meters
        Dim elevUnit As MeasurementUnit = BA_GetElevationUnitsForAOI(AOIFolderBase)
        Dim aoiName As String = BA_GetBareName(AOIFolderBase)
        Dim pAoi As Aoi = New Aoi(aoiName, AOIFolderBase, "", "")
        Dim frmDataUnits As FrmDataUnits = New FrmDataUnits(pAoi)

        'if the elevation unit is not defined, pop a form to define it
        If elevUnit = MeasurementUnit.Missing Then
            If frmDataUnits.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                elevUnit = frmDataUnits.NewElevationUnit
            Else
                'Set elevUnit to system default and warn the user
                elevUnit = MeasurementUnit.Meters
                Dim sb As StringBuilder = New StringBuilder
                sb.Append("You did not define the elevation units for this" & vbCrLf)
                sb.Append("AOI. BAGIS assumes the elevation units are" & vbCrLf)
                sb.Append(BA_EnumDescription(MeasurementUnit.Meters) & ". If the elevation units are not " & BA_EnumDescription(MeasurementUnit.Meters) & vbCrLf)
                sb.Append("the results will be incorrect." & vbCrLf)
                MessageBox.Show(sb.ToString, "Elevation units not defined", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        End If

        Dim converter As IUnitConverter = New UnitConverter
        m_changingUnits = True
        If elevUnit = MeasurementUnit.Meters Then
            m_demInMeters = True
            m_oldElevUnits = esriUnits.esriMeters
            OptZMeters.Checked = True
            'Default value is currently in feet 01-AUG-2013
            If ES_ElevRangeUnits = esriUnits.esriFeet Then
                m_upperElevRange = converter.ConvertUnits(ES_ElevRange, ES_ElevRangeUnits, esriUnits.esriMeters)
                m_lowerElevRange = converter.ConvertUnits(ES_ElevRange, ES_ElevRangeUnits, esriUnits.esriMeters)
            Else
                m_upperElevRange = ES_ElevRange
                m_lowerElevRange = ES_ElevRange
            End If
            TxtUpperRange.Text = Math.Round(m_upperElevRange)
            TxtLowerRange.Text = Math.Round(m_lowerElevRange)
        Else
            m_oldElevUnits = esriUnits.esriFeet
            OptZFeet.Checked = True
            m_upperElevRange = ES_ElevRange
            m_lowerElevRange = ES_ElevRange
            TxtUpperRange.Text = m_upperElevRange
            TxtLowerRange.Text = m_lowerElevRange
        End If
        m_changingUnits = False

        'Dim demRasterStats As IRasterStatistics = BA_GetDemStatsGDB(AOIFolderBase)
        Dim demRasterStats As IRasterStatistics = BA_GetRasterStatsGDB(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces, True) & _
                                                                                          BA_EnumDescription(MapsFileName.filled_dem_gdb), 0)
        ES_DEMMin = demRasterStats.Minimum
        ES_DEMMax = demRasterStats.Maximum

        'Populate Boxes
        txtMinElev.Text = Math.Round(ES_DEMMin - 0.005, 2)
        txtMaxElev.Text = Math.Round(ES_DEMMax + 0.005, 2)

        'Retrieving the XY units from the filled_dem
        InitBufferDistance()
        Select Case m_demXYUnits
            Case esriUnits.esriMeters
                m_buffer = converter.ConvertUnits(ES_Buffer, ES_BufferUnits, esriUnits.esriMeters)
                TxtBufferDistance.Text = Math.Round(m_buffer, 3)
                m_oldBufferUnits = esriUnits.esriMeters
                CmboxDistanceUnit.SelectedIndex = 1 'set Meters as the default Buffer Distance
            Case esriUnits.esriFeet
                m_buffer = converter.ConvertUnits(ES_Buffer, ES_BufferUnits, esriUnits.esriFeet)
                TxtBufferDistance.Text = Math.Round(m_buffer, 3)
                m_oldBufferUnits = esriUnits.esriFeet
                CmboxDistanceUnit.SelectedIndex = 0 'set Feet as the default Buffer Distance
        End Select
        m_CurrentAOI = aoiName

        'Check for previously saved scenario and load those values as defaults
        Dim xmlOutputPath As String = BA_GetPath(AOIFolderBase, PublicPath.Maps) & BA_EnumDescription(PublicPath.AnalysisXml)
        Dim lastAnalysis As Analysis = Nothing
        ' Open analysis file if there is one
        If BA_File_ExistsWindowsIO(xmlOutputPath) Then
            lastAnalysis = BA_LoadAnalysisFromXml(AOIFolderBase)
            ReloadLastAnalysis(lastAnalysis)
            BtnViewResult.Enabled = True
        Else
            BtnViewResult.Enabled = False
        End If

        Dim elevUnits As esriUnits = esriUnits.esriMeters
        If DemInMeters = False Then
            elevUnits = esriUnits.esriFeet
        End If
        LoadScenarioSites(lastAnalysis, elevUnits, m_oldElevUnits)
        ManageCalculateButton()
        m_baseLayers = False    'Reset base layers flag so we know we need to reload when a psuedo-site is added
        ManageMapsButton()

    End Sub

    Private Sub LblBufferDistance_Click(sender As System.Object, e As System.EventArgs) Handles LblBufferDistance.Click
        Dim msgString As String = "Buffer Distance of Sites:" & vbCrLf & vbCrLf
        msgString &= "Indicates the planar distance beyond which the site is not reliable for supporting forecast models." & vbCrLf
        msgString &= "Deselect the check box to ignore the buffer distance of the site so that only elevation ranges are "
        msgString &= "used to define the represented areas. If the checkbox is deselected, any value in this field will "
        msgString &= "be ignored." & vbCrLf & vbCrLf
        msgString &= "The default distance of 5.641895835 Miles is equal to the 100 square mile station density "
        msgString &= "recommendation for mountainous areas by the World Meterological Organization."
        MsgBox(msgString, MsgBoxStyle.MsgBoxHelp, "Buffer Distance")
    End Sub

    Private Sub LblUpperRange_Click(sender As System.Object, e As System.EventArgs) Handles LblUpperRange.Click
        Dim msgString As String = "Elevation Upper Range of Sites:" & vbCrLf & vbCrLf
        msgString &= "Indicates the additional elevation range above which the site is not reliable for supporting forecast models." & vbCrLf
        msgString &= "Upper range value cannot be negative. A value of zero indicates that no areas higher than the site are represented."
        msgString &= "Deselect the check box to include all the areas higher than the elevation of the site as represented by the site."
        MsgBox(msgString, MsgBoxStyle.MsgBoxHelp, "Elevation Upper Range")
    End Sub

    Private Sub LblLowerRange_Click(sender As System.Object, e As System.EventArgs) Handles LblLowerRange.Click
        Dim msgString As String = "Elevation Lower Range of Sites:" & vbCrLf & vbCrLf
        msgString &= "Indicates the additional elevation range below which the site is not reliable for supporting forecast models." & vbCrLf
        msgString &= "Lower range value cannot be negative. A value of zero indicates that no areas lower than the site are represented."
        msgString &= "Deselect the check box to include all the areas lower than the elevation of the site as represented by the site."
        MsgBox(msgString, MsgBoxStyle.MsgBoxHelp, "Elevation Lower Range")
    End Sub

    Private Sub BtnAbout_Click(sender As System.Object, e As System.EventArgs) Handles BtnAbout.Click
        Dim msgString As String = "Site Scenario Analysis Tool:" & vbCrLf & vbCrLf
        msgString &= "The tool generates reports of the areas represented/nonrepresented by two scenarios defined in an AOI." & vbCrLf
        msgString &= "Represented areas are defined using Buffer Distance, Upper and Lower Range parameters of the sites."
        msgString &= " Areas within the buffer distance, upper AND lower ranges of the sites are represented by the monitoring data collected on these sites."
        MsgBox(msgString, MsgBoxStyle.MsgBoxHelp, "About this tool")
    End Sub

    Private Sub ChkBufferDistance_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ChkBufferDistance.CheckedChanged
        LblBufferDistance.Enabled = ChkBufferDistance.Checked
        TxtBufferDistance.Enabled = ChkBufferDistance.Checked
        CmboxDistanceUnit.Enabled = ChkBufferDistance.Checked
        BtnPreview.Enabled = ChkBufferDistance.Checked
    End Sub

    Private Sub ChkUpperRange_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ChkUpperRange.CheckedChanged
        LblUpperRange.Enabled = ChkUpperRange.Checked
        TxtUpperRange.Enabled = ChkUpperRange.Checked
        ManageUpperRange()
    End Sub

    Private Sub ChkLowerRange_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ChkLowerRange.CheckedChanged
        LblLowerRange.Enabled = ChkLowerRange.Checked
        TxtLowerRange.Enabled = ChkLowerRange.Checked
        ManageLowerRange()
    End Sub

    Private Sub BtnCalculate_Click(sender As System.Object, e As System.EventArgs) Handles BtnCalculate.Click
        '' Create/configure a step progressor
        'Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 15)
        'pStepProg.Show()
        '' Create/configure the ProgressDialog. This automatically displays the dialog
        'Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Calculating represented areas for AOI", "Calculating...")
        'progressDialog2.ShowDialog()

        'Try
        '    Dim layersFolder As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers)
        '    Dim analysisFolder As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis)
        '    Dim bufferDistance As Double = Convert.ToDouble(TxtBufferDistance.Text)
        '    '--- Delete any old _rep files ---
        '    '--- First, remove them from the map ---
        '    Dim pMap As IMap = My.Document.FocusMap
        '    Dim pTempLayer As ILayer
        '    For i = pMap.LayerCount To 1 Step -1
        '        pTempLayer = pMap.Layer(i - 1)
        '        Dim suffix As String = Microsoft.VisualBasic.Strings.Right(pTempLayer.Name, 4)
        '        If suffix = "_Rep" Then 'remove the layer
        '            If TypeOf pTempLayer Is IRasterLayer Then 'disconnect a rasterlayer before removing it
        '                Dim pDLayer As IDataLayer2 = CType(pTempLayer, IDataLayer2)
        '                pDLayer.Disconnect()
        '            End If
        '            pMap.DeleteLayer(pTempLayer)
        '        End If
        '    Next
        '    '--- Then delete them ---
        '    Dim success As BA_ReturnCode = BA_RemoveFilesBySuffix(analysisFolder, "_Rep")

        '    '--- Delete any old _elev files ---
        '    '--- First, remove them from the map ---
        '    For i = pMap.LayerCount To 1 Step -1
        '        pTempLayer = pMap.Layer(i - 1)
        '        Dim suffix As String = Microsoft.VisualBasic.Strings.Right(pTempLayer.Name, 5)
        '        If suffix = "_Elev" Then 'remove the layer
        '            If TypeOf pTempLayer Is IRasterLayer Then 'disconnect a rasterlayer before removing it
        '                Dim pDLayer As IDataLayer2 = CType(pTempLayer, IDataLayer2)
        '                pDLayer.Disconnect()
        '            End If
        '            pMap.DeleteLayer(pTempLayer)
        '        End If
        '    Next
        '    pTempLayer = Nothing
        '    '--- Then delete them ---
        '    success = BA_RemoveFilesBySuffix(analysisFolder, "_Elev")

        '    '--- Calculate correct buffer distance based on XY units ---
        '    Dim converter As IUnitConverter = New UnitConverter
        '    If ChkBufferDistance.Checked = True Then
        '        If m_oldBufferUnits <> m_demXYUnits Then
        '            bufferDistance = converter.ConvertUnits(bufferDistance, m_oldBufferUnits, m_demXYUnits)
        '        End If
        '    End If
        '    '--- Calculate correct upper and lower elevation thresholds based on DEM units ---
        '    Dim upperElev As Double = BA_9999
        '    Dim lowerElev As Double = BA_9999
        '    If ChkUpperRange.Checked Then
        '        upperElev = Convert.ToDouble(TxtUpperRange.Text)
        '        If m_demInMeters = False AndAlso OptZMeters.Checked = True Then
        '            upperElev = converter.ConvertUnits(upperElev, esriUnits.esriMeters, esriUnits.esriFeet)
        '        ElseIf m_demInMeters = True AndAlso OptZMeters.Checked = False Then
        '            upperElev = converter.ConvertUnits(upperElev, esriUnits.esriFeet, esriUnits.esriMeters)
        '        End If
        '    End If
        '    If ChkLowerRange.Checked Then
        '        lowerElev = Convert.ToDouble(TxtLowerRange.Text)
        '        If m_demInMeters = False AndAlso OptZMeters.Checked = True Then
        '            lowerElev = converter.ConvertUnits(lowerElev, esriUnits.esriMeters, esriUnits.esriFeet)
        '        ElseIf m_demInMeters = True AndAlso OptZMeters.Checked = False Then
        '            lowerElev = converter.ConvertUnits(lowerElev, esriUnits.esriFeet, esriUnits.esriMeters)
        '        End If
        '    End If
        '    '--- Actual Representation ---
        '    success = BA_ReturnCode.UnknownError
        '    Dim siteList As IList(Of Site) = New List(Of Site)
        '    '--- Use buffer distance ---
        '    If ChkBufferDistance.Checked = True Then
        '        Dim sb As StringBuilder = New StringBuilder 'StringBuffer to hold names of rasters to combine
        '        For Each aRow As DataGridViewRow In GrdScenario1.Rows
        '            Dim selected As Boolean = Convert.ToBoolean(aRow.Cells(idxSelected).Value)
        '            If selected = True Then
        '                Dim aSite As Site = CreateSiteFromRow(aRow, False)
        '                pStepProg.Message = "Create area map for scenario 1 site " & aSite.Name
        '                pStepProg.Step()
        '                System.Windows.Forms.Application.DoEvents()
        '                Dim pointFileName As String = Nothing
        '                Select Case aSite.SiteType
        '                    Case SiteType.Pseudo
        '                        pointFileName = BA_EnumDescription(MapsFileName.Pseudo)
        '                    Case SiteType.Snotel
        '                        pointFileName = BA_EnumDescription(MapsFileName.Snotel)
        '                    Case SiteType.SnowCourse
        '                        pointFileName = BA_EnumDescription(MapsFileName.SnowCourse)
        '                End Select
        '                'buffer_v.shp
        '                'Do NOT delete buffer_v.shp; It is re-used when the user uses the Site Representation menu
        '                success = BA_CreateBufferPolygonFile(layersFolder, pointFileName, aSite.ObjectId, bufferDistance, _
        '                                                     AOIFolderBase, BA_BufferDistanceFile, _
        '                                                     My.Document.ActiveView.FocusMap.SpatialReference)
        '                If success = BA_ReturnCode.Success Then
        '                    Dim surfacesFolder As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces)
        '                    Dim elevFileName As String = BA_GetSiteScenarioFileName(aSite, SiteScenarioLayerType.Elevation)
        '                    'sitename_elev
        '                    success = BA_ExtractRepresentedArea(AOIFolderBase, BA_BufferDistanceFile, surfacesFolder, BA_EnumDescription(MapsFileName.filled_dem_gdb), _
        '                                                        analysisFolder, elevFileName, aSite, upperElev, lowerElev, ES_DEMMax, ES_DEMMin)
        '                    If success = BA_ReturnCode.Success Then
        '                        sb.Append(analysisFolder & "\" & elevFileName)
        '                        sb.Append(";")
        '                        siteList.Add(aSite)
        '                    End If
        '                End If
        '            End If
        '        Next
        '        If sb.Length > 1 Then
        '            Dim inRasters As String = sb.ToString.TrimEnd(";")
        '            Dim snapRasterPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
        '            pStepProg.Message = "Create represented area map for scenario 1"
        '            pStepProg.Step()
        '            System.Windows.Forms.Application.DoEvents()
        '            Dim statsFileName As String = "tmpStat"
        '            'Combine _elev files using cellStatistics to create represented area
        '            success = BA_GetCellStatistics(inRasters, snapRasterPath, StatisticsTypeString.MINIMUM.ToString, _
        '                                                                analysisFolder & "\" & statsFileName)
        '            If success = BA_ReturnCode.Success Then
        '                'Recode raster to 2 values: 1-Represented, 5-non-represented
        '                success = BA_ReclassNonRepresentedArea(analysisFolder, statsFileName, BA_EnumDescription(MapsFileName.ActualRepresentedArea), _
        '                                                       snapRasterPath)
        '                If success = BA_ReturnCode.Success Then
        '                    'Update descriptions on recoded raster
        '                    success = BA_UpdateAreaNames(analysisFolder, BA_EnumDescription(MapsFileName.ActualRepresentedArea), True)
        '                    If success = BA_ReturnCode.Success Then
        '                        For Each aSite As Site In siteList
        '                            Dim inputPath As String = analysisFolder & "\" & BA_GetSiteScenarioFileName(aSite, SiteScenarioLayerType.Elevation)
        '                            Dim outputPath As String = analysisFolder & "\" & BA_GetSiteScenarioFileName(aSite, SiteScenarioLayerType.Representation)
        '                            success = BA_SetValueToNull(inputPath, outputPath, BA_ValNonRepresentedOutside)
        '                            'Delete the input file if it exists
        '                            If success = BA_ReturnCode.Success Then
        '                                'Add and populate Name column in raster
        '                                'This is where we get the sitename_rep file to display
        '                                success = BA_UpdateSiteNames(analysisFolder, BA_GetSiteScenarioFileName(aSite, SiteScenarioLayerType.Representation))
        '                                ' _elev files are deleted later after scenario 2 has finished processing. We re-use them
        '                            End If
        '                        Next
        '                    End If
        '                End If
        '            End If
        '        End If
        '    Else
        '        pStepProg.Message = "Create elevation only map for scenario 1"
        '        pStepProg.Step()
        '        System.Windows.Forms.Application.DoEvents()
        '        'Use elevation only
        '        Dim siteMin As Double = ES_DEMMax
        '        Dim siteMax As Double = ES_DEMMin
        '        'Get the lowest/highest elevation from all the selected sites 
        '        For Each aRow As DataGridViewRow In GrdScenario1.Rows
        '            Dim selected As Boolean = Convert.ToBoolean(aRow.Cells(idxSelected).Value)
        '            If selected = True Then
        '                Dim aSite As Site = CreateSiteFromRow(aRow, False)
        '                If aSite.Elevation < siteMin Then
        '                    siteMin = aSite.Elevation
        '                End If
        '                If aSite.Elevation > siteMax Then
        '                    siteMax = aSite.Elevation
        '                End If
        '            End If
        '        Next
        '        'Reclass site according to lowest/highest elevation and upper/lower thresholds
        '        Dim demFilePath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces, True) & BA_EnumDescription(MapsFileName.filled_dem_gdb)
        '        Dim snapRasterPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
        '        Dim outputPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True) & BA_EnumDescription(MapsFileName.ActualRepresentedArea)
        '        success = BA_ReclassByElevationOnly(demFilePath, outputPath, snapRasterPath, siteMax, siteMin, upperElev, lowerElev, _
        '                                            ES_DEMMax, ES_DEMMin)
        '        If success = BA_ReturnCode.Success Then
        '            success = BA_UpdateAreaNames(analysisFolder, BA_EnumDescription(MapsFileName.ActualRepresentedArea), False)
        '        End If
        '    End If
        '    '--- Pseudo Representation ---
        '    If GrdScenario2.Rows.Count > 0 Then
        '        '--- Use buffer distance ---
        '        If ChkBufferDistance.Checked = True Then
        '            Dim sb As StringBuilder = New StringBuilder 'StringBuffer to hold names of rasters to combine
        '            Dim s2List As IList(Of Site) = New List(Of Site)
        '            For Each aRow As DataGridViewRow In GrdScenario2.Rows
        '                Dim aSite As Site = CreateSiteFromRow(aRow, True)
        '                pStepProg.Message = "Create area map for scenario 2 site " & aSite.Name
        '                pStepProg.Step()
        '                System.Windows.Forms.Application.DoEvents()
        '                Dim elevFileName As String = BA_GetSiteScenarioFileName(aSite, SiteScenarioLayerType.Elevation)
        '                'Check to see if the buffered file already exists from the actual analysis
        '                If Not BA_File_Exists(analysisFolder & "\" & elevFileName, WorkspaceType.Geodatabase, ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTRasterDataset) Then
        '                    Dim pointFileName As String = Nothing
        '                    Select Case aSite.SiteType
        '                        Case SiteType.Pseudo
        '                            pointFileName = BA_EnumDescription(MapsFileName.Pseudo)
        '                        Case SiteType.Snotel
        '                            pointFileName = BA_EnumDescription(MapsFileName.Snotel)
        '                        Case SiteType.SnowCourse
        '                            pointFileName = BA_EnumDescription(MapsFileName.SnowCourse)
        '                    End Select
        '                    success = BA_CreateBufferPolygonFile(layersFolder, pointFileName, aSite.ObjectId, bufferDistance, _
        '                                                         AOIFolderBase, BA_BufferDistanceFile, _
        '                                                         My.Document.ActiveView.FocusMap.SpatialReference)
        '                    If success = BA_ReturnCode.Success Then
        '                        Dim surfacesFolder As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces)
        '                        success = BA_ExtractRepresentedArea(AOIFolderBase, BA_BufferDistanceFile, surfacesFolder, BA_EnumDescription(MapsFileName.filled_dem_gdb), _
        '                                                            analysisFolder, elevFileName, aSite, upperElev, lowerElev, ES_DEMMax, ES_DEMMin)
        '                        If success = BA_ReturnCode.Success Then
        '                            sb.Append(analysisFolder & "\" & elevFileName)
        '                            sb.Append(";")
        '                            s2List.Add(aSite)
        '                        End If
        '                    End If
        '                Else
        '                    'The buffered file already exists from actual analysis so we just need to append the name to the sb
        '                    sb.Append(analysisFolder & "\" & elevFileName)
        '                    sb.Append(";")
        '                End If
        '            Next
        '            If sb.Length > 1 Then
        '                Dim inRasters As String = sb.ToString.TrimEnd(";")
        '                Dim snapRasterPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
        '                pStepProg.Message = "Create represented area map for scenario 2"
        '                pStepProg.Step()
        '                System.Windows.Forms.Application.DoEvents()
        '                Dim statsFileName As String = "tmpStat"
        '                success = BA_GetCellStatistics(inRasters, snapRasterPath, StatisticsTypeString.MINIMUM.ToString, _
        '                                                                    analysisFolder & "\" & statsFileName)
        '                If success = BA_ReturnCode.Success Then
        '                    'Recode raster to 3 values: 1-Represented, 5-Not Represented
        '                    success = BA_ReclassNonRepresentedArea(analysisFolder, statsFileName, BA_EnumDescription(MapsFileName.PseudoRepresentedArea), _
        '                                                           snapRasterPath)
        '                    If success = BA_ReturnCode.Success Then
        '                        success = BA_UpdateAreaNames(analysisFolder, BA_EnumDescription(MapsFileName.PseudoRepresentedArea), True)
        '                        If success = BA_ReturnCode.Success Then
        '                            For Each aSite As Site In s2List
        '                                Dim inputPath As String = analysisFolder & "\" & BA_GetSiteScenarioFileName(aSite, SiteScenarioLayerType.Elevation)
        '                                Dim outputPath As String = analysisFolder & "\" & BA_GetSiteScenarioFileName(aSite, SiteScenarioLayerType.Representation)
        '                                success = BA_SetValueToNull(inputPath, outputPath, BA_ValNonRepresentedOutside)
        '                                'Delete the input file if it exists
        '                                If success = BA_ReturnCode.Success Then
        '                                    'Add and populate Name column in raster
        '                                    success = BA_UpdateSiteNames(analysisFolder, BA_GetSiteScenarioFileName(aSite, SiteScenarioLayerType.Representation))
        '                                    If BA_File_Exists(inputPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
        '                                        BA_RemoveRasterFromGDB(analysisFolder, BA_GetSiteScenarioFileName(aSite, SiteScenarioLayerType.Elevation))
        '                                    End If
        '                                End If
        '                            Next
        '                            'Delete elev files for scenario 1 sites
        '                            For Each aSite As Site In siteList
        '                                Dim inputPath As String = analysisFolder & "\" & BA_GetSiteScenarioFileName(aSite, SiteScenarioLayerType.Elevation)
        '                                If BA_File_Exists(inputPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
        '                                    BA_RemoveRasterFromGDB(analysisFolder, BA_GetSiteScenarioFileName(aSite, SiteScenarioLayerType.Elevation))
        '                                End If
        '                            Next
        '                            Scenario2Map_Flag = True
        '                        End If
        '                    End If

        '                End If
        '            End If
        '        Else
        '            pStepProg.Message = "Create elevation only map for scenario 2"
        '            pStepProg.Step()
        '            System.Windows.Forms.Application.DoEvents()
        '            'Use elevation only
        '            Dim siteMin As Double = ES_DEMMax
        '            Dim siteMax As Double = ES_DEMMin
        '            'Get the lowest/highest elevation from all the selected sites 
        '            For Each aRow As DataGridViewRow In GrdScenario2.Rows
        '                Dim aSite As Site = CreateSiteFromRow(aRow, True)
        '                If aSite.Elevation < siteMin Then
        '                    siteMin = aSite.Elevation
        '                End If
        '                If aSite.Elevation > siteMax Then
        '                    siteMax = aSite.Elevation
        '                End If
        '            Next
        '            'Reclass site according to lowest/highest elevation and upper/lower thresholds
        '            Dim demFilePath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces, True) & BA_EnumDescription(MapsFileName.filled_dem_gdb)
        '            Dim snapRasterPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
        '            Dim outputPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True) & BA_EnumDescription(MapsFileName.PseudoRepresentedArea)
        '            success = BA_ReclassByElevationOnly(demFilePath, outputPath, snapRasterPath, siteMax, siteMin, upperElev, lowerElev, _
        '                                                ES_DEMMax, ES_DEMMin)
        '            If success = BA_ReturnCode.Success Then
        '                success = BA_UpdateAreaNames(analysisFolder, BA_EnumDescription(MapsFileName.PseudoRepresentedArea), False)
        '                If success = BA_ReturnCode.Success Then
        '                    Scenario2Map_Flag = True
        '                End If
        '            End If
        '        End If
        '    Else
        '        'Delete old scenario sites layer to avoid confusion
        '        If BA_File_Exists(analysisFolder & "\" & BA_EnumDescription(MapsFileName.PseudoRepresentedArea), WorkspaceType.Geodatabase, _
        '                          ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTRasterDataset) Then
        '            BA_RemoveRasterFromGDB(analysisFolder, BA_EnumDescription(MapsFileName.PseudoRepresentedArea))
        '        End If
        '        'Delete old difference layer to avoid confusion
        '        If BA_File_Exists(analysisFolder & "\" & BA_EnumDescription(MapsFileName.DifferenceRepresentedArea), WorkspaceType.Geodatabase, _
        '                          ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTRasterDataset) Then
        '            BA_RemoveRasterFromGDB(analysisFolder, BA_EnumDescription(MapsFileName.DifferenceRepresentedArea))
        '        End If
        '        Scenario2Map_Flag = False
        '    End If
        '    RepDifferenceMap_Flag = False
        '    If success = BA_ReturnCode.Success Then
        '        'We have a second scenario and need to combine the maps
        '        If Scenario2Map_Flag = True Then
        '            pStepProg.Message = "Create difference in representation map"
        '            pStepProg.Step()
        '            Dim layerList As IList(Of String) = New List(Of String)
        '            layerList.Add(analysisFolder & "\" & BA_EnumDescription(MapsFileName.ActualRepresentedArea))
        '            layerList.Add(analysisFolder & "\" & BA_EnumDescription(MapsFileName.PseudoRepresentedArea))
        '            'Try using this for mask also
        '            Dim snapRasterPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi) & "\" & BA_EnumDescription(AOIClipFile.BufferedAOIExtentCoverage)
        '            success = BA_CreateRepresentationDifference(snapRasterPath, layerList, analysisFolder, _
        '                                                        BA_EnumDescription(MapsFileName.DifferenceRepresentedArea))
        '            If success = BA_ReturnCode.Success Then
        '                RepDifferenceMap_Flag = True
        '            End If
        '        End If

        '        pStepProg.Message = "Saving analysis settings"
        '        pStepProg.Step()
        '        SaveAnalysisLog(pStepProg, progressDialog2)

        '        'Add the site scenario layers to the map frame
        '        BA_AddScenarioLayersToMapFrame(My.ThisApplication, My.Document, AOIFolderBase)
        '        SymbolizeSelectedSites()
        '        Scenario1Map_Flag = True
        '        'Display scenario 1 map as default
        '        Dim Basin_Name As String
        '        Dim cboSelectedBasin = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of cboTargetedBasin)(My.ThisAddIn.IDs.cboTargetedBasin)
        '        If Len(Trim(cboSelectedBasin.getValue)) = 0 Then
        '            Basin_Name = ""
        '        Else
        '            Basin_Name = cboSelectedBasin.getValue
        '        End If
        '        BA_AddMapElements(My.Document, m_CurrentAOI & Basin_Name, "Subtitle BAGIS")
        '        Dim retVal As Integer = BA_DisplayMap(My.Document, 7, Basin_Name, m_CurrentAOI, Map_Display_Elevation_in_Meters, _
        '                                Trim(TxtScenario1.Text))
        '        BA_RemoveLayersfromLegend(My.Document)
        '        Dim sb As StringBuilder = New StringBuilder
        '        sb.Append("Please use the menu items to view maps!")
        '        SiteRepresentationMap_Flag = ChkBufferDistance.Checked
        '        If SiteRepresentationMap_Flag = False Then
        '            sb.Append(vbCrLf & "The Site Representation button is disabled because a buffer distance was not used.")
        '        End If
        '        MessageBox.Show(sb.ToString, "Site Scenario", MessageBoxButtons.OK, MessageBoxIcon.Information)
        '        Call BA_Enable_ScenarioMapFlags(True)
        '    End If
        '    ManageMapsButton()
        'Catch ex As Exception
        '    Debug.Print("BtnCalculate_Click Exception: " & ex.Message)
        'Finally
        '    If progressDialog2 IsNot Nothing Then
        '        progressDialog2.HideDialog()
        '    End If
        '    progressDialog2 = Nothing
        '    pStepProg = Nothing
        'End Try
        CalculateRepresentedArea()
    End Sub

    Private Sub LoadScenarioSites(ByVal lastAnalysis As Analysis, ByVal fromElevUnits As esriUnits, ByVal toElevUnits As esriUnits)
        'Remove old rows
        GrdScenario1.Rows.Clear()
        'Reset btnCalculateMessage flag
        m_btnCalculateMessage = True
        'Initialize converter in case we need it
        Dim converter As IUnitConverter = New UnitConverter
        Dim DEMMax As Double = ES_DEMMax + 0.005
        Dim DEMMin As Double = ES_DEMMin - 0.005
        'Loading Snotel sites
        Dim snotelList As IList(Of Site) = BA_ReadSiteAttributes(SiteType.Snotel)
        If snotelList.Count > 0 Then AOI_HasSNOTEL = True
        For Each snotel As Site In snotelList
            '---create a row---
            Dim item As DataGridViewRow = CreateRowFromSite(snotel, True)
            If lastAnalysis Is Nothing Then
                '---Check the box to include in the analysis by default---
                item.Cells(idxSelected).Value = True
            Else
                item.Cells(idxSelected).Value = SitePreviouslySelected(snotel, lastAnalysis.Scenario1Sites)
                If fromElevUnits <> toElevUnits Then
                    Dim dblElevation As Double = converter.ConvertUnits(Convert.ToDouble(item.Cells(idxDefaultElevation).Value), fromElevUnits, toElevUnits)
                    item.Cells(idxElevation).Value = Math.Round(dblElevation)
                 End If
            End If
            '---add the row---
            Dim idxNewRow As Integer = GrdScenario1.Rows.Add(item)
            '---mark the row as read-only if the elevation is out of range, and warn the user
            Dim sb As StringBuilder = New StringBuilder
            Dim defaultElevation As Double = Convert.ToDouble(item.Cells(idxDefaultElevation).Value)
            Dim direction1 As String = Nothing
            Dim direction2 As String = Nothing
            If defaultElevation > DEMMax Then
                direction1 = "higher"
                direction2 = "maximum"
            ElseIf defaultElevation < DEMMin Then
                direction1 = "lower"
                direction2 = "minimum"
            End If
            If direction1 IsNot Nothing Then
                GrdScenario1.Rows(idxNewRow).ReadOnly = True
                m_btnCalculateMessage = False
                item.Cells(idxSelected).Value = False
                GrdScenario1.Rows(idxNewRow).DefaultCellStyle.BackColor = SystemColors.Control
                GrdScenario1.Rows(idxNewRow).DefaultCellStyle.ForeColor = SystemColors.GrayText
                sb.Append("The elevation of snotel site " & snotel.Name & " is " & direction1 & " than ")
                sb.Append("the " & direction2 & " DEM elevation. This site has been disabled and cannot ")
                sb.Append("be included in the analysis. The calculate button may also be disabled ")
                sb.Append("if this was the only site selected.")
                MessageBox.Show(sb.ToString, "Invalid elevation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Next
        'Loading Snow Course sites
        Dim snowCourseList As IList(Of Site) = BA_ReadSiteAttributes(SiteType.SnowCourse)
        If snowCourseList.Count > 0 Then AOI_HasSnowCourse = True
        For Each snowCourse As Site In snowCourseList
            '---create a row---
            Dim item As DataGridViewRow = CreateRowFromSite(snowCourse, True)
            If lastAnalysis Is Nothing Then
                '---Check the box to include in the analysis by default---
                item.Cells(idxSelected).Value = True
            Else
                item.Cells(idxSelected).Value = SitePreviouslySelected(snowCourse, lastAnalysis.Scenario1Sites)
                If fromElevUnits <> toElevUnits Then
                    Dim dblElevation As Double = converter.ConvertUnits(Convert.ToDouble(item.Cells(idxDefaultElevation).Value), fromElevUnits, toElevUnits)
                    item.Cells(idxElevation).Value = Math.Round(dblElevation)
                End If
            End If
            '---add the row---
            Dim idxNewRow As Integer = GrdScenario1.Rows.Add(item)
            '---mark the row as read-only if the elevation is out of range, and warn the user
            Dim sb As StringBuilder = New StringBuilder
            Dim defaultElevation As Double = Convert.ToDouble(item.Cells(idxDefaultElevation).Value)
            Dim direction1 As String = Nothing
            Dim direction2 As String = Nothing
            If defaultElevation > DEMMax Then
                direction1 = "higher"
                direction2 = "maximum"
            ElseIf defaultElevation < DEMMin Then
                direction1 = "lower"
                direction2 = "minimum"
            End If
            If direction1 IsNot Nothing Then
                GrdScenario1.Rows(idxNewRow).ReadOnly = True
                m_btnCalculateMessage = False
                item.Cells(idxSelected).Value = False
                GrdScenario1.Rows(idxNewRow).DefaultCellStyle.BackColor = SystemColors.Control
                GrdScenario1.Rows(idxNewRow).DefaultCellStyle.ForeColor = SystemColors.GrayText
                sb.Append("The elevation of snow course site " & snowCourse.Name & " is " & direction1 & " than ")
                sb.Append("the " & direction2 & " DEM elevation. This site has been disabled and cannot ")
                sb.Append("be included in the analysis. The calculate button may also be disabled ")
                sb.Append("if this was the only site selected.")
                MessageBox.Show(sb.ToString, "Invalid elevation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Next
        'Loading Pseudo sites
        Dim psuedoList As IList(Of Site) = BA_ReadSiteAttributes(SiteType.Pseudo)
        If psuedoList.Count > 0 Then AOI_HasPseudoSite = True
        For Each pSite As Site In psuedoList
            '---create a row---
            Dim item As DataGridViewRow = CreateRowFromSite(pSite, True)
            If lastAnalysis Is Nothing Then
                '---Check the box to include in the analysis by default---
                item.Cells(idxSelected).Value = False
            Else
                item.Cells(idxSelected).Value = SitePreviouslySelected(pSite, lastAnalysis.Scenario1Sites)
                If fromElevUnits <> toElevUnits Then
                    Dim dblElevation As Double = converter.ConvertUnits(Convert.ToDouble(item.Cells(idxDefaultElevation).Value), fromElevUnits, toElevUnits)
                    item.Cells(idxElevation).Value = Math.Round(dblElevation)
                End If
            End If
            '---add the row---
            GrdScenario1.Rows.Add(item)
        Next
        'Remove old rows
        GrdScenario2.Rows.Clear()
        'Load scenario 2 sites if there is a previous analysis
        If lastAnalysis IsNot Nothing AndAlso lastAnalysis.Scenario2Sites IsNot Nothing Then
            For i As Integer = 0 To lastAnalysis.Scenario2Sites.GetUpperBound(0)
                Dim aSite As Site = lastAnalysis.Scenario2Sites(i)
                'Make sure the site exists in the scenario 1 grid before adding it to scenario 2
                For Each nextRow As DataGridViewRow In GrdScenario1.Rows
                    Dim siteType As String = Convert.ToString(nextRow.Cells(idxSiteType).Value)
                    Dim siteId As Integer = Convert.ToInt32(nextRow.Cells(idxObjectId).Value)
                    If siteType = aSite.SiteTypeText AndAlso siteId = aSite.ObjectId AndAlso nextRow.ReadOnly = False Then
                        Dim newRow As New DataGridViewRow
                        newRow.CreateCells(GrdScenario2)
                        With newRow
                            .Cells(idxObjectId - 1).Value = siteId
                            .Cells(idxSiteType - 1).Value = siteType
                            .Cells(idxSiteName - 1).Value = nextRow.Cells(idxSiteName).Value
                            .Cells(idxElevation - 1).Value = nextRow.Cells(idxElevation).Value
                            .Cells(idxUpper - 1).Value = nextRow.Cells(idxUpper).Value
                            .Cells(idxLower - 1).Value = nextRow.Cells(idxLower).Value
                            .Cells(idxDefaultElevation - 1).Value = nextRow.Cells(idxDefaultElevation).Value
                        End With
                        '---add the row---
                        GrdScenario2.Rows.Add(newRow)
                    End If
                Next
            Next
            GrdScenario2.CurrentCell = Nothing
        End If
        GrdScenario1.CurrentCell = Nothing
        ManageUpperRange()
        ManageLowerRange()
    End Sub

    Private Sub BtnNewSite_Click(sender As System.Object, e As System.EventArgs) Handles BtnNewSite.Click
        'Toggle to data view
        BA_ToggleView(My.Document, True)
        'Display base layers
        If m_baseLayers = False Then
            AddBaseLayers()
        End If

        'Assemble list of layers that should be visible in final output
        Dim layerNamesList As IList(Of String) = New List(Of String)
        layerNamesList.Add(BA_MAPS_FILLED_DEM)
        layerNamesList.Add(BA_MAPS_AOI_BOUNDARY)
        layerNamesList.Add(BA_MAPS_STREAMS)
        If AOI_HasSNOTEL Then
            layerNamesList.Add(BA_MAPS_SNOTEL_SITES)
        End If
        If AOI_HasSnowCourse Then
            layerNamesList.Add(BA_MAPS_SNOW_COURSE_SITES)
        End If
        If AOI_HasPseudoSite Then
            layerNamesList.Add(BA_MAPS_PSEUDO_SITES)
        End If
        Dim LayerNames(layerNamesList.Count + 1) As String
        For i As Int32 = 0 To layerNamesList.Count - 1
            LayerNames(i + 1) = layerNamesList.Item(i)
        Next
        Dim retVal As Integer = BA_ToggleLayersinMapFrame(My.Document, LayerNames)

        Dim UIDCls As ESRI.ArcGIS.esriSystem.UID = New ESRI.ArcGIS.esriSystem.UIDClass()

        ' id property of menu from Config.esriaddinx document
        UIDCls.Value = "Microsoft_BAGIS_SelectSiteTool"

        Dim document As ESRI.ArcGIS.Framework.IDocument = My.ArcMap.Document
        Dim commandItem As ESRI.ArcGIS.Framework.ICommandItem = TryCast(document.CommandBars.Find(UIDCls), ESRI.ArcGIS.Framework.ICommandItem)
        If commandItem Is Nothing Then
            Exit Sub
        End If
        My.ArcMap.Application.CurrentTool = commandItem
    End Sub

    Friend Sub AddNewPseudoSite(ByVal pSite As Site)
        '---create a row---
        Dim item As New DataGridViewRow
        item.CreateCells(GrdScenario1)
        Dim displayElevation As Double
        Dim converter As IUnitConverter = New UnitConverter
        If m_demInMeters = True Then
            If m_oldElevUnits = esriUnits.esriMeters Then
                displayElevation = pSite.Elevation
            Else
                displayElevation = converter.ConvertUnits(pSite.Elevation, esriUnits.esriMeters, esriUnits.esriFeet)
            End If
        Else
            If m_oldElevUnits = esriUnits.esriFeet Then
                displayElevation = pSite.Elevation
            Else
                displayElevation = converter.ConvertUnits(pSite.Elevation, esriUnits.esriFeet, esriUnits.esriMeters)
            End If
        End If
        With item
            .Cells(idxObjectId).Value = pSite.ObjectId
            .Cells(idxSiteType).Value = pSite.SiteType.ToString
            .Cells(idxSiteName).Value = pSite.Name
            .Cells(idxElevation).Value = Math.Round(displayElevation)
            .Cells(idxUpper).Value = 0
            .Cells(idxLower).Value = 0
            .Cells(idxDefaultElevation).Value = pSite.Elevation
        End With
        '---add the row---
        GrdScenario1.Rows.Add(item)
        ManageUpperRange()
        ManageLowerRange()
    End Sub

    Friend ReadOnly Property DemInMeters As Boolean
        Get
            Return m_demInMeters
        End Get
    End Property

    Friend ReadOnly Property ElevationUnits As esriUnits
        Get
            Return m_oldElevUnits
        End Get
    End Property

    Friend ReadOnly Property BufferUnits As esriUnits
        Get
            Return m_oldBufferUnits
        End Get
    End Property

    Private Sub BtnDeleteSite_Click(sender As System.Object, e As System.EventArgs) Handles BtnDeleteSite.Click
        Dim deleteList As IList(Of Site) = New List(Of Site)
        Try
            For Each pCell As DataGridViewCell In GrdScenario1.SelectedCells
                Dim nextRow As DataGridViewRow = GrdScenario1.Rows.Item(pCell.RowIndex)
                Dim oid As Integer = Convert.ToInt32(nextRow.Cells(idxObjectId).Value)
                Dim strType As String = Convert.ToString(nextRow.Cells(idxSiteType).Value)
                Dim sType As SiteType = BA_GetSiteType(strType)
                Dim name As String = Convert.ToString(nextRow.Cells(idxSiteName).Value)
                Dim elev As Double = Convert.ToDouble(nextRow.Cells(idxDefaultElevation).Value)
                Dim useSite As Site = New Site(oid, name, sType, elev, False)
                If Not deleteList.Contains(useSite) Then
                    deleteList.Add(useSite)
                End If
            Next
            If deleteList.Count > 0 Then
                Dim sb As StringBuilder = New StringBuilder

                sb.Append("Are you sure you want to delete the following sites?")
                sb.Append(" This action is permanent and cannot be undone." & vbCrLf)
                Dim converter As IUnitConverter = New UnitConverter
                For Each pSite As Site In deleteList
                    Dim displayElev As Double
                    Dim strUnits As String = BA_StandardizeEsriUnits(esriUnits.esriMeters)
                    If m_demInMeters = True Then
                        'The dem is in meters and the display units are in meters; No conversion required
                        If m_oldElevUnits = esriUnits.esriMeters Then
                            displayElev = pSite.Elevation
                        Else
                            displayElev = converter.ConvertUnits(pSite.Elevation, esriUnits.esriMeters, esriUnits.esriFeet)
                            strUnits = BA_StandardizeEsriUnits(esriUnits.esriFeet)
                        End If
                    Else
                        If m_oldElevUnits = esriUnits.esriFeet Then
                            displayElev = pSite.Elevation
                            strUnits = BA_StandardizeEsriUnits(esriUnits.esriFeet)
                        Else
                            displayElev = converter.ConvertUnits(pSite.Elevation, esriUnits.esriFeet, esriUnits.esriMeters)
                        End If
                    End If
                    sb.Append(pSite.SiteType.ToString & " site ")
                    sb.Append("'" & pSite.Name & "' at elevation " & Math.Round(displayElev))
                    sb.Append(" " & strUnits)
                    sb.Append(vbCrLf)
                Next
                Dim res As DialogResult = MessageBox.Show(sb.ToString, "Delete site(s)", MessageBoxButtons.YesNo, _
                                                          MessageBoxIcon.Question)
                If res = DialogResult.Yes Then
                    Dim layersGdbPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers)
                    Dim success As BA_ReturnCode
                    For Each pSite As Site In deleteList
                        'Remove the site from the feature class
                        Select Case pSite.SiteType
                            Case SiteType.Snotel
                                success = BA_DeleteSite(layersGdbPath, BA_EnumDescription(MapsFileName.Snotel), pSite)
                            Case SiteType.SnowCourse
                                success = BA_DeleteSite(layersGdbPath, BA_EnumDescription(MapsFileName.SnowCourse), pSite)
                            Case SiteType.Pseudo
                                success = BA_DeleteSite(layersGdbPath, BA_EnumDescription(MapsFileName.Pseudo), pSite)
                        End Select
                        If success = BA_ReturnCode.Success Then
                            'Remove the site from Scenario 1 DataGridView
                            Dim s1DeleteRow As DataGridViewRow = Nothing
                            For Each nextRow As DataGridViewRow In GrdScenario1.Rows
                                Dim oid As Integer = Convert.ToInt32(nextRow.Cells(idxObjectId).Value)
                                Dim siteText As String = Convert.ToString(nextRow.Cells(idxSiteType).Value)
                                Dim nextSiteType As SiteType = BA_GetSiteType(siteText)
                                If oid = pSite.ObjectId AndAlso nextSiteType = pSite.SiteType Then
                                    s1DeleteRow = nextRow
                                    Exit For
                                End If
                            Next
                            'Remove the site from Scenario 2 DataGridView
                            Dim s2DeleteRow As DataGridViewRow = Nothing
                            For Each nextRow As DataGridViewRow In GrdScenario2.Rows
                                Dim oid As Integer = Convert.ToInt32(nextRow.Cells(idxObjectId - 1).Value)
                                Dim siteText As String = Convert.ToString(nextRow.Cells(idxSiteType - 1).Value)
                                Dim nextSiteType As SiteType = BA_GetSiteType(siteText)
                                If oid = pSite.ObjectId AndAlso nextSiteType = pSite.SiteType Then
                                    s2DeleteRow = nextRow
                                    Exit For
                                End If
                            Next
                            If s2DeleteRow IsNot Nothing Then
                                GrdScenario2.Rows.Remove(s2DeleteRow)
                            End If
                            'Remove the site from Site Representation DataGridView
                            'Get a handle to the parent form
                            Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of frmSiteRepresentations.AddinImpl)(My.ThisAddIn.IDs.frmSiteRepresentations)
                            Dim siteRepForm As frmSiteRepresentations = dockWindowAddIn.UI
                            Dim sRepDeleteRow As DataGridViewRow = Nothing
                            For Each nextRow As DataGridViewRow In siteRepForm.GrdExistingSites.Rows
                                Dim oid As Integer = Convert.ToInt32(nextRow.Cells(idxObjectId).Value)
                                Dim siteText As String = Convert.ToString(nextRow.Cells(idxSiteType).Value)
                                Dim nextSiteType As SiteType = BA_GetSiteType(siteText)
                                If oid = pSite.ObjectId AndAlso nextSiteType = pSite.SiteType Then
                                    sRepDeleteRow = nextRow
                                    Exit For
                                End If
                            Next
                            If sRepDeleteRow IsNot Nothing Then
                                siteRepForm.GrdExistingSites.Rows.Remove(sRepDeleteRow)
                            End If
                            If s1DeleteRow IsNot Nothing Then
                                GrdScenario1.Rows.Remove(s1DeleteRow)
                                'Reload sites layers if they are present
                                Dim filepath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers)
                                Dim FileName As String = BA_GetBareName(MapsFileName.Snotel)
                                Dim idxLayer As Integer = BA_GetLayerIndexByFilePath(My.ArcMap.Document, filepath, FileName)
                                Dim retVal As Integer
                                Dim pColor As IColor = New RgbColor
                                If AOI_HasSNOTEL AndAlso idxLayer > -1 Then
                                    pColor.RGB = RGB(0, 0, 255)
                                    retVal = BA_MapDisplayPointMarkers(My.ThisApplication, filepath & "\" & FileName, MapsLayerName.Snotel, pColor, MapsMarkerType.Snotel)
                                End If
                                FileName = BA_GetBareName(MapsFileName.SnowCourse)
                                idxLayer = BA_GetLayerIndexByFilePath(My.ArcMap.Document, filepath, FileName)
                                If AOI_HasSnowCourse AndAlso idxLayer > -1 Then
                                    pColor.RGB = RGB(0, 255, 255) 'cyan
                                    retVal = BA_MapDisplayPointMarkers(My.ThisApplication, filepath & "\" & FileName, MapsLayerName.SnowCourse, pColor, MapsMarkerType.SnowCourse)
                                End If
                                FileName = BA_EnumDescription(MapsFileName.Pseudo)
                                idxLayer = BA_GetLayerIndexByFilePath(My.ArcMap.Document, filepath, FileName)
                                If AOI_HasPseudoSite AndAlso idxLayer > -1 Then
                                    pColor.RGB = RGB(255, 170, 0) 'electron gold
                                    retVal = BA_MapDisplayPointMarkers(My.ThisApplication, filepath & "\" & FileName, MapsLayerName.pseudo_sites, pColor, MapsMarkerType.PseudoSite)
                                End If
                                'Check to see if there are any more pseudo-sites left so we can correctly set the global variable
                                Dim hasPseudoSites As Boolean = False
                                For Each nextRow As DataGridViewRow In siteRepForm.GrdExistingSites.Rows
                                    Dim siteText As String = Convert.ToString(nextRow.Cells(idxSiteType).Value)
                                    Dim nextSiteType As SiteType = BA_GetSiteType(siteText)
                                    If nextSiteType = SiteType.Pseudo Then
                                        hasPseudoSites = True
                                    End If
                                Next
                                AOI_HasPseudoSite = hasPseudoSites
                                My.Document.ActivatedView.Refresh()
                            End If
                        End If
                    Next
                End If
            Else
                MessageBox.Show("You have not selected any sites to delete.", "Delete site(s)", MessageBoxButtons.OK, _
                                MessageBoxIcon.Information)
                Exit Sub
            End If
        Catch ex As Exception
            Debug.Print("BtnDeleteSite_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub TxtBufferDistance_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles TxtBufferDistance.Validating
        If Not IsValidInteger(TxtBufferDistance.Text) Then
            ' Cancel the event and select the text to be corrected by the user.
            e.Cancel = True
            TxtBufferDistance.Select(0, TxtBufferDistance.Text.Length)
            MessageBox.Show("Please use a positive number when setting the buffer distance. Deselect the checkbox " & _
                            "to exclude the buffer distance from your calculation.", "Invalid buffer distance", MessageBoxButtons.OK, _
                            MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub TxtUpperRange_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles TxtUpperRange.Validating
        If Not IsValidInteger(TxtUpperRange.Text) Then
            ' Cancel the event and select the text to be corrected by the user.
            e.Cancel = True
            TxtUpperRange.Select(0, TxtUpperRange.Text.Length)
            MessageBox.Show("Please use a positive number when setting the elevation upper range. Deselect the checkbox " & _
                            "to exclude the elevation upper range from your calculation.", "Invalid elevation upper range", MessageBoxButtons.OK, _
                            MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub TxtLowerRange_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles TxtLowerRange.Validating
        If Not IsValidInteger(TxtLowerRange.Text) Then
            ' Cancel the event and select the text to be corrected by the user.
            e.Cancel = True
            TxtLowerRange.Select(0, TxtLowerRange.Text.Length)
            MessageBox.Show("Please use a positive number when setting the elevation lower range. Deselect the checkbox " & _
                            "to exclude the elevation lower range from your calculation.", "Invalid elevation lower range", MessageBoxButtons.OK, _
                            MessageBoxIcon.Information)
        End If
    End Sub

    Private Function IsValidInteger(ByVal fn As String) As Boolean
        ' Check for any value
        If String.IsNullOrEmpty(fn) Then Return False
        ' Check for numeric value
        If Not IsNumeric(fn) Then Return False
        ' Try to convert to Integer
        Dim iVal As Integer
        Try
            iVal = CInt(fn)
        Catch ex As Exception
            ' Could not convert to Integer, return false
            Return False
        End Try
        ' Don't allow negative numbers
        If iVal >= 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub ManageUpperRange()
        Dim DEMMax As Double = CDbl(txtMaxElev.Text) + 0.005
        If ChkUpperRange.Checked = True Then
            Dim upperRange As Double = Convert.ToDouble(TxtUpperRange.Text)
            For Each sRow As DataGridViewRow In GrdScenario1.Rows
                Dim sElevation As Double = Convert.ToDouble(sRow.Cells(idxElevation).Value)
                Dim newValue = sElevation + upperRange
                'Set the cell value to the calculated upper range unless it is higher than the DEM Max
                If newValue < DEMMax Then
                    sRow.Cells(idxUpper).Value = Math.Round(newValue)
                Else
                    sRow.Cells(idxUpper).Value = Math.Round(DEMMax)
                End If
            Next
            For Each sRow As DataGridViewRow In GrdScenario2.Rows
                Dim sElevation As Double = Convert.ToDouble(sRow.Cells(idxElevation - 1).Value)
                Dim newValue = sElevation + upperRange
                'Set the cell value to the calculated upper range unless it is higher than the DEM Max
                If newValue < DEMMax Then
                    sRow.Cells(idxUpper - 1).Value = Math.Round(newValue)
                Else
                    sRow.Cells(idxUpper - 1).Value = Math.Round(DEMMax)
                End If
            Next

        Else
            For Each sRow As DataGridViewRow In GrdScenario1.Rows
                sRow.Cells(idxUpper).Value = Math.Round(DEMMax)
            Next
            For Each sRow As DataGridViewRow In GrdScenario2.Rows
                sRow.Cells(idxUpper - 1).Value = Math.Round(DEMMax)
            Next

        End If
    End Sub

    Private Sub ManageLowerRange()
        Dim DEMMin As Double = CDbl(txtMinElev.Text) - 0.005
        If ChkLowerRange.Checked = True Then
            Dim lowerRange As Double = Convert.ToDouble(TxtLowerRange.Text)
            For Each sRow As DataGridViewRow In GrdScenario1.Rows
                Dim sElevation As Double = Convert.ToDouble(sRow.Cells(idxElevation).Value)
                Dim newValue = sElevation - lowerRange
                'Set the cell value to the calculated lower range unless it is less than the DEM Min
                If newValue > DEMMin Then
                    sRow.Cells(idxLower).Value = Math.Round(newValue)
                Else
                    sRow.Cells(idxLower).Value = Math.Round(DEMMin)
                End If
            Next
            For Each sRow As DataGridViewRow In GrdScenario2.Rows
                Dim sElevation As Double = Convert.ToDouble(sRow.Cells(idxElevation - 1).Value)
                Dim newValue = sElevation - lowerRange
                'Set the cell value to the calculated lower range unless it is less than the DEM Min
                If newValue > DEMMin Then
                    sRow.Cells(idxLower - 1).Value = Math.Round(newValue)
                Else
                    sRow.Cells(idxLower - 1).Value = Math.Round(DEMMin)
                End If
            Next
        Else
            For Each sRow As DataGridViewRow In GrdScenario1.Rows
                sRow.Cells(idxLower).Value = Math.Round(DEMMin)
            Next
            For Each sRow As DataGridViewRow In GrdScenario2.Rows
                sRow.Cells(idxLower - 1).Value = Math.Round(DEMMin)
            Next
        End If
    End Sub

    Private Sub TxtUpperRange_TextChanged(sender As System.Object, e As System.EventArgs) Handles TxtUpperRange.TextChanged
        If m_changingUnits = False Then
            Dim upperVal As Double = CDbl(TxtUpperRange.Text)
            Dim converter As IUnitConverter = New UnitConverter
            If m_demInMeters = True Then
                'The dem is in meters and the display units are in meters; No conversion required
                If m_oldElevUnits = esriUnits.esriMeters Then
                    m_upperElevRange = upperVal
                Else
                    'The dem is in meters and the display units are in feet; Convert to meters for calculation
                    m_upperElevRange = converter.ConvertUnits(upperVal, esriUnits.esriFeet, esriUnits.esriMeters)
                End If
            Else
                If m_oldElevUnits = esriUnits.esriFeet Then
                    m_upperElevRange = upperVal
                Else
                    m_upperElevRange = converter.ConvertUnits(upperVal, esriUnits.esriMeters, esriUnits.esriFeet)
                End If
            End If
            'Manage the display on the grid
            ManageUpperRange()
        End If
    End Sub

    Private Sub TxtLowerRange_TextChanged(sender As Object, e As System.EventArgs) Handles TxtLowerRange.TextChanged
        If m_changingUnits = False Then
            Dim lowerVal As Double = CDbl(TxtLowerRange.Text)
            Dim converter As IUnitConverter = New UnitConverter
            If m_demInMeters = True Then
                'The dem is in meters and the display units are in meters; No conversion required
                If m_oldElevUnits = esriUnits.esriMeters Then
                    m_lowerElevRange = lowerVal
                Else
                    'The dem is in meters and the display units are in feet; Convert to meters for calculation
                    m_lowerElevRange = converter.ConvertUnits(lowerVal, esriUnits.esriFeet, esriUnits.esriMeters)
                End If
            Else
                If m_oldElevUnits = esriUnits.esriFeet Then
                    m_lowerElevRange = lowerVal
                Else
                    m_lowerElevRange = converter.ConvertUnits(lowerVal, esriUnits.esriMeters, esriUnits.esriFeet)
                End If
            End If
            'Manage the display on the grid
            ManageLowerRange()
        End If
    End Sub

    Private Sub InitBufferDistance()
        Dim filePath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces)
        Dim linearUnit As ILinearUnit = BA_GetLinearUnitOfProjectedRaster(filePath, BA_EnumDescription(MapsFileName.filled_dem_gdb))
        If linearUnit IsNot Nothing Then
            Dim strUnit As String = linearUnit.Name
            If strUnit = "Meter" Then
                m_demXYUnits = esriUnits.esriMeters
            ElseIf strUnit = "Feet" Then
                m_demXYUnits = esriUnits.esriFeet
            Else
                Dim msg As StringBuilder = New StringBuilder
                msg.Append("The linear units of the filled_dem for this AOI are not compatible with BAGIS. ")
                msg.Append("Please convert the linear unit of the layers in this AOI to either meters or feet ")
                msg.Append("to ensure that your results are accurate.")
                MessageBox.Show(msg.ToString, "Invalid linear units", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        End If
    End Sub

    Private Sub CmboxDistanceUnit_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles CmboxDistanceUnit.SelectedIndexChanged
        Dim newBufferUnits As esriUnits
        Select Case CmboxDistanceUnit.SelectedIndex
            Case 0
                newBufferUnits = esriUnits.esriFeet
            Case 1
                newBufferUnits = esriUnits.esriMeters
            Case 2
                newBufferUnits = esriUnits.esriMiles
            Case 3
                newBufferUnits = esriUnits.esriKilometers
        End Select
        'If the units didn't change, we don't need to do anything
        If newBufferUnits = m_oldBufferUnits Then Exit Sub
        Dim converter As IUnitConverter = New UnitConverter
        Dim dVal As Double = CDbl(TxtBufferDistance.Text)
        Dim newBuffer As Double = converter.ConvertUnits(dVal, m_oldBufferUnits, newBufferUnits)
        TxtBufferDistance.Text = Math.Round(newBuffer, 3)
        m_oldBufferUnits = newBufferUnits
    End Sub

    Private Sub OptZMeters_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles OptZMeters.CheckedChanged
        'Changing from Meters to Feet
        If OptZMeters.Checked = True AndAlso m_oldElevUnits <> esriUnits.esriMeters Then
            m_changingUnits = True
            'DEM Elevations
            If m_demInMeters = True Then
                txtMinElev.Text = Math.Round(ES_DEMMin - 0.005, 2)
                txtMaxElev.Text = Math.Round(ES_DEMMax + 0.005, 2)
                TxtUpperRange.Text = Math.Round(m_upperElevRange)
                TxtLowerRange.Text = Math.Round(m_lowerElevRange)
                For Each sRow As DataGridViewRow In GrdScenario1.Rows
                    Dim elev As Double = Convert.ToDouble(sRow.Cells(idxDefaultElevation).Value)
                    sRow.Cells(idxElevation).Value = Math.Round(elev)
                Next
                For Each sRow As DataGridViewRow In GrdScenario2.Rows
                    Dim elev As Double = Convert.ToDouble(sRow.Cells(idxDefaultElevation - 1).Value)
                    sRow.Cells(idxElevation - 1).Value = Math.Round(elev)
                Next
            Else
                Dim converter As IUnitConverter = New UnitConverter
                Dim tmpMin As Double = converter.ConvertUnits(ES_DEMMin - 0.005, esriUnits.esriFeet, esriUnits.esriMeters)
                Dim tmpMax As Double = converter.ConvertUnits(ES_DEMMax + 0.005, esriUnits.esriFeet, esriUnits.esriMeters)
                txtMinElev.Text = Math.Round(tmpMin, 2)
                txtMaxElev.Text = Math.Round(tmpMax, 2)
                Dim tmpUpper As Double = converter.ConvertUnits(m_upperElevRange, esriUnits.esriFeet, esriUnits.esriMeters)
                Dim tmpLower As Double = converter.ConvertUnits(m_lowerElevRange, esriUnits.esriFeet, esriUnits.esriMeters)
                TxtUpperRange.Text = Math.Round(tmpUpper)
                TxtLowerRange.Text = Math.Round(tmpLower)
                For Each sRow As DataGridViewRow In GrdScenario1.Rows
                    Dim elev As Double = Convert.ToDouble(sRow.Cells(idxDefaultElevation).Value)
                    sRow.Cells(idxElevation).Value = Math.Round(converter.ConvertUnits(elev, esriUnits.esriFeet, esriUnits.esriMeters))
                Next
                For Each sRow As DataGridViewRow In GrdScenario2.Rows
                    Dim elev As Double = Convert.ToDouble(sRow.Cells(idxDefaultElevation - 1).Value)
                    sRow.Cells(idxElevation - 1).Value = Math.Round(converter.ConvertUnits(elev, esriUnits.esriFeet, esriUnits.esriMeters))
                Next
            End If
            ManageUpperRange()
            ManageLowerRange()
            m_oldElevUnits = esriUnits.esriMeters
            m_changingUnits = False
        End If
    End Sub

    Private Sub OptZFeet_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles OptZFeet.CheckedChanged
        'Changing from Feet to Meters
        If OptZFeet.Checked = True AndAlso m_oldElevUnits <> esriUnits.esriFeet Then
            m_changingUnits = True
            'DEM Elevations
            If m_demInMeters = False Then
                txtMinElev.Text = Math.Round(ES_DEMMin - 0.005, 2)
                txtMaxElev.Text = Math.Round(ES_DEMMax + 0.005, 2)
                TxtUpperRange.Text = Math.Round(m_upperElevRange)
                TxtLowerRange.Text = Math.Round(m_lowerElevRange)
                For Each sRow As DataGridViewRow In GrdScenario1.Rows
                    Dim elev As Double = Convert.ToDouble(sRow.Cells(idxDefaultElevation).Value)
                    sRow.Cells(idxElevation).Value = Math.Round(elev)
                Next
                For Each sRow As DataGridViewRow In GrdScenario2.Rows
                    Dim elev As Double = Convert.ToDouble(sRow.Cells(idxDefaultElevation - 1).Value)
                    sRow.Cells(idxElevation - 1).Value = Math.Round(elev)
                Next
            Else
                Dim converter As IUnitConverter = New UnitConverter
                Dim tmpMin As Double = converter.ConvertUnits(ES_DEMMin - 0.005, esriUnits.esriMeters, esriUnits.esriFeet)
                Dim tmpMax As Double = converter.ConvertUnits(ES_DEMMax + 0.005, esriUnits.esriMeters, esriUnits.esriFeet)
                txtMinElev.Text = Math.Round(tmpMin, 2)
                txtMaxElev.Text = Math.Round(tmpMax, 2)
                Dim tmpUpper As Double = converter.ConvertUnits(m_upperElevRange, esriUnits.esriMeters, esriUnits.esriFeet)
                Dim tmpLower As Double = converter.ConvertUnits(m_lowerElevRange, esriUnits.esriMeters, esriUnits.esriFeet)
                TxtUpperRange.Text = Math.Round(tmpUpper)
                TxtLowerRange.Text = Math.Round(tmpLower)
                For Each sRow As DataGridViewRow In GrdScenario1.Rows
                    Dim elev As Double = Convert.ToDouble(sRow.Cells(idxDefaultElevation).Value)
                    sRow.Cells(idxElevation).Value = Math.Round(converter.ConvertUnits(elev, esriUnits.esriMeters, esriUnits.esriFeet))
                    Debug.Print(Math.Round(converter.ConvertUnits(elev, esriUnits.esriMeters, esriUnits.esriFeet)))
                Next
                For Each sRow As DataGridViewRow In GrdScenario2.Rows
                    Dim elev As Double = Convert.ToDouble(sRow.Cells(idxDefaultElevation - 1).Value)
                    sRow.Cells(idxElevation - 1).Value = Math.Round(converter.ConvertUnits(elev, esriUnits.esriMeters, esriUnits.esriFeet))
                    Debug.Print(Math.Round(converter.ConvertUnits(elev, esriUnits.esriMeters, esriUnits.esriFeet)))
                Next
            End If
            ManageUpperRange()
            ManageLowerRange()
            m_oldElevUnits = esriUnits.esriFeet
            m_changingUnits = False
        End If
    End Sub

    'Checks to see if the site is already in GrdScenario2
    Private Function AlreadyInGrid(ByVal sType As String, ByVal oid As Integer) As Boolean
        For Each nextRow As DataGridViewRow In GrdScenario2.Rows
            Dim siteType As String = Convert.ToString(nextRow.Cells(idxSiteType - 1).Value)
            Dim siteId As Integer = Convert.ToInt32(nextRow.Cells(idxObjectId - 1).Value)
            If siteType = sType AndAlso siteId = oid Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub BtnAddSite_Click(sender As System.Object, e As System.EventArgs) Handles BtnAddSite.Click

        Dim idxList As IList(Of Integer) = New List(Of Integer)
        Try
            For Each pCell As DataGridViewCell In GrdScenario1.SelectedCells
                If Not idxList.Contains(pCell.RowIndex) Then
                    Dim nextRow As DataGridViewRow = GrdScenario1.Rows.Item(pCell.RowIndex)
                    Dim oid As Integer = Convert.ToInt32(nextRow.Cells(idxObjectId).Value)
                    Dim siteType As String = Convert.ToString(nextRow.Cells(idxSiteType).Value)
                    If Not AlreadyInGrid(siteType, oid) Then
                        Dim item As New DataGridViewRow
                        item.CreateCells(GrdScenario2)
                        With item
                            .Cells(idxObjectId - 1).Value = oid
                            .Cells(idxSiteType - 1).Value = siteType
                            .Cells(idxSiteName - 1).Value = nextRow.Cells(idxSiteName).Value
                            .Cells(idxElevation - 1).Value = nextRow.Cells(idxElevation).Value
                            .Cells(idxUpper - 1).Value = nextRow.Cells(idxUpper).Value
                            .Cells(idxLower - 1).Value = nextRow.Cells(idxLower).Value
                            .Cells(idxDefaultElevation - 1).Value = nextRow.Cells(idxDefaultElevation).Value
                        End With
                        '---add the row---
                        GrdScenario2.Rows.Add(item)
                    End If
                    idxList.Add(pCell.RowIndex)
                End If
            Next
        Catch ex As Exception
            Debug.Print("BtnAddSite_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub BtnRemoveSite_Click(sender As System.Object, e As System.EventArgs) Handles BtnRemoveSite.Click
        Dim deleteList As IList(Of DataGridViewRow) = New List(Of DataGridViewRow)
        Try
            'Build list of rows to be deleted
            For Each pCell As DataGridViewCell In GrdScenario2.SelectedCells
                Dim nextRow As DataGridViewRow = GrdScenario2.Rows.Item(pCell.RowIndex)
                If Not deleteList.Contains(nextRow) Then
                    deleteList.Add(nextRow)
                End If
            Next
            'Delete the rows
            For Each dRow As DataGridViewRow In deleteList
                GrdScenario2.Rows.Remove(dRow)
            Next
        Catch ex As Exception
            Debug.Print("BtnRemoveSite Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub BtnRemoveAll_Click(sender As System.Object, e As System.EventArgs) Handles BtnRemoveAll.Click
        GrdScenario2.Rows.Clear()
    End Sub

    Private Sub BtnToggleSel_Click(sender As System.Object, e As System.EventArgs) Handles BtnToggleSel.Click
        Dim sList As IList(Of Integer) = New List(Of Integer)   'sites to be selected
        Dim dList As IList(Of Integer) = New List(Of Integer)   'sites to be de-selected
        Dim idx As Integer = 0
        For Each nextRow As DataGridViewRow In GrdScenario1.Rows
            Dim selected As Boolean = Convert.ToBoolean(nextRow.Cells(idxSelected).Value)
            Dim rowId As Integer = idx
            If selected = True Then
                dList.Add(rowId)
            Else
                sList.Add(rowId)
            End If
            idx += 1
        Next
        ' Set the new selected rows first so we don't trigger the validation for having no selected sites
        For Each rowId As Integer In sList
            Dim selRow As DataGridViewRow = GrdScenario1.Rows(rowId)
            selRow.Cells(idxSelected).Value = True
        Next
        ' Deselect the formerly selected rows
        For Each rowId As Integer In dList
            Dim selRow As DataGridViewRow = GrdScenario1.Rows(rowId)
            selRow.Cells(idxSelected).Value = False
        Next
    End Sub

    Private Function CreateSiteFromRow(ByVal aRow As DataGridViewRow, ByVal IsScenarioSite2 As Boolean) As Site
        If Not IsScenarioSite2 Then
            Dim oid As Integer = Convert.ToInt32(aRow.Cells(idxObjectId).Value)
            Dim include As Boolean = Convert.ToBoolean(aRow.Cells(idxSelected).Value)
            Dim name As String = Convert.ToString(aRow.Cells(idxSiteName).Value)
            Dim strType As String = Convert.ToString(aRow.Cells(idxSiteType).Value)
            Dim siteType As SiteType = BA_GetSiteType(strType)
            Dim elev As Double = Convert.ToDouble(aRow.Cells(idxDefaultElevation).Value)
            Dim aSite As Site = New Site(oid, name, siteType, elev, include)
            aSite.ElevationText = Convert.ToString(aRow.Cells(idxElevation).Value)
            aSite.UpperElevText = Convert.ToString(aRow.Cells(idxUpper).Value)
            aSite.LowerElevText = Convert.ToString(aRow.Cells(idxLower).Value)
            Return aSite
        Else
            Dim oid As Integer = Convert.ToInt32(aRow.Cells(idxObjectId - 1).Value)
            Dim name As String = Convert.ToString(aRow.Cells(idxSiteName - 1).Value)
            Dim strType As String = Convert.ToString(aRow.Cells(idxSiteType - 1).Value)
            Dim siteType As SiteType = BA_GetSiteType(strType)
            Dim elev As Double = Convert.ToDouble(aRow.Cells(idxDefaultElevation - 1).Value)
            Dim aSite As Site = New Site(oid, name, siteType, elev, True)
            aSite.ElevationText = Convert.ToString(aRow.Cells(idxElevation - 1).Value)
            aSite.UpperElevText = Convert.ToString(aRow.Cells(idxUpper - 1).Value)
            aSite.LowerElevText = Convert.ToString(aRow.Cells(idxLower - 1).Value)
            Return aSite
        End If
    End Function

    Private Function CreateRowFromSite(ByVal aSite As Site, ByVal isScenario1Site As Boolean) As DataGridViewRow
        If isScenario1Site Then
            Dim item As New DataGridViewRow
            item.CreateCells(GrdScenario1)
            With item
                'Check the box to include in the analysis by default
                .Cells(idxObjectId).Value = aSite.ObjectId
                .Cells(idxSiteType).Value = aSite.SiteType.ToString
                .Cells(idxSiteName).Value = aSite.Name
                .Cells(idxElevation).Value = Math.Round(aSite.Elevation)
                .Cells(idxUpper).Value = 0
                .Cells(idxLower).Value = 0
                .Cells(idxDefaultElevation).Value = aSite.Elevation
            End With
            Return item
        Else
            Dim item As New DataGridViewRow
            item.CreateCells(GrdScenario2)
            With item
                .Cells(idxObjectId - 1).Value = aSite.ObjectId
                .Cells(idxSiteType - 1).Value = aSite.SiteTypeText
                .Cells(idxSiteName - 1).Value = aSite.Name
                .Cells(idxElevation - 1).Value = Math.Round(aSite.Elevation)
                .Cells(idxUpper - 1).Value = 0
                .Cells(idxLower - 1).Value = 0
                .Cells(idxDefaultElevation - 1).Value = aSite.Elevation
            End With
            Return item
        End If
    End Function

    Private Sub SymbolizeSelectedSites()
        Dim lstActualSnotelId As IList(Of Int32) = New List(Of Int32)
        Dim lstPseudoSnotelId As IList(Of Int32) = New List(Of Int32)
        Dim lstActualSnowCourseId As IList(Of Int32) = New List(Of Int32)
        Dim lstPseudoSnowCourseId As IList(Of Int32) = New List(Of Int32)
        Dim lstActualPseudoId As IList(Of Int32) = New List(Of Int32)
        Dim lstPseudoPseudoId As IList(Of Int32) = New List(Of Int32)
        Dim snotelCopy As IFeatureLayer = Nothing
        Dim snowCourseCopy As IFeatureLayer = Nothing
        Dim pseudoCopy As IFeatureLayer = Nothing
        Dim sitesLayerName As String = ""
        Dim pMap As IMap = My.Document.FocusMap
        Dim nlayers As Integer = pMap.LayerCount
        Dim fLayerDef As IFeatureLayerDefinition
        Dim pFSele As IFeatureSelection = Nothing
        Dim pQFilter As IQueryFilter = New QueryFilter
        Try
            'Remove selected site layers if pre-existing
            For i = nlayers To 1 Step -1
                Dim tempLayer As ILayer
                tempLayer = CType(pMap.Layer(i - 1), ILayer)   'Explicit cast
                If TypeOf tempLayer Is FeatureLayer Then
                    If tempLayer.Name = BA_MAPS_SNOTEL_SCENARIO1 Then
                        pMap.DeleteLayer(tempLayer)
                    ElseIf tempLayer.Name = BA_MAPS_SNOTEL_SCENARIO2 Then
                        pMap.DeleteLayer(tempLayer)
                    ElseIf tempLayer.Name = BA_MAPS_SNOW_COURSE_SCENARIO1 Then
                        pMap.DeleteLayer(tempLayer)
                    ElseIf tempLayer.Name = BA_MAPS_SNOW_COURSE_SCENARIO2 Then
                        pMap.DeleteLayer(tempLayer)
                    ElseIf tempLayer.Name = BA_MAPS_PSEUDO_SCENARIO1 Then
                        pMap.DeleteLayer(tempLayer)
                    ElseIf tempLayer.Name = BA_MAPS_PSEUDO_SCENARIO2 Then
                        pMap.DeleteLayer(tempLayer)
                    End If
                End If
            Next

            'Reset layer count in case layers were removed
            nlayers = pMap.LayerCount

            Dim pActualColor As IColor = New RgbColor
            pActualColor.RGB = RGB(0, 0, 0)    'Black
            Dim actualRenderer As ISimpleRenderer = BuildRendererForPoints(pActualColor, 25)
            Dim pPseudoColor As IColor = New RgbColor
            pPseudoColor.RGB = RGB(255, 170, 0) 'Electron gold
            Dim pseudoRenderer As ISimpleRenderer = BuildRendererForPoints(pPseudoColor, 35)
            For Each nextRow As DataGridViewRow In GrdScenario1.Rows
                If Convert.ToBoolean(nextRow.Cells(idxSelected).Value) = True Then
                    Dim strType As String = nextRow.Cells(idxSiteType).Value
                    Dim siteType As SiteType = BA_GetSiteType(strType)
                    Select Case siteType
                        Case BAGIS.SiteType.Snotel
                            lstActualSnotelId.Add(Convert.ToString(nextRow.Cells(idxObjectId).Value))
                        Case BAGIS.SiteType.SnowCourse
                            lstActualSnowCourseId.Add(Convert.ToString(nextRow.Cells(idxObjectId).Value))
                        Case BAGIS.SiteType.Pseudo
                            lstActualPseudoId.Add(Convert.ToString(nextRow.Cells(idxObjectId).Value))
                    End Select
                End If
            Next
            For Each nextRow As DataGridViewRow In GrdScenario2.Rows
                Dim strType As String = nextRow.Cells(idxSiteType - 1).Value
                Dim siteType As SiteType = BA_GetSiteType(strType)
                Select Case siteType
                    Case BAGIS.SiteType.Snotel
                        'Not checking to see if the id is already in the list because the query result will be the same
                        lstPseudoSnotelId.Add(Convert.ToString(nextRow.Cells(idxObjectId - 1).Value))
                    Case BAGIS.SiteType.SnowCourse
                        lstPseudoSnowCourseId.Add(Convert.ToString(nextRow.Cells(idxObjectId - 1).Value))
                    Case BAGIS.SiteType.Pseudo
                        lstPseudoPseudoId.Add(Convert.ToString(nextRow.Cells(idxObjectId - 1).Value))
                End Select
            Next
            'Symbolize the selected Snotel sites
            If lstActualSnotelId.Count > 0 Then
                Dim query As String = BuildQueryFromOid(lstActualSnotelId)
                sitesLayerName = BA_EnumDescription(MapsLayerName.Snotel)
                Dim tempLayer As ILayer
                Dim snotelSrc As IFeatureLayer = Nothing
                For i = nlayers To 1 Step -1
                    tempLayer = CType(pMap.Layer(i - 1), ILayer)   'Explicit cast
                    If TypeOf tempLayer Is FeatureLayer AndAlso tempLayer.Name = sitesLayerName Then
                        snotelSrc = CType(tempLayer, IFeatureLayer)
                        Exit For
                    End If
                Next
                If snotelSrc IsNot Nothing Then
                    pFSele = TryCast(snotelSrc, IFeatureSelection)
                    pQFilter.WhereClause = query
                    pFSele.SelectFeatures(pQFilter, esriSelectionResultEnum.esriSelectionResultNew, False)
                    fLayerDef = CType(snotelSrc, IFeatureLayerDefinition)
                    snotelCopy = fLayerDef.CreateSelectionLayer(BA_MAPS_SNOTEL_SCENARIO1, True, Nothing, Nothing)
                    Dim pGFLayer As IGeoFeatureLayer = CType(snotelCopy, IGeoFeatureLayer)
                    pGFLayer.Renderer = actualRenderer
                    My.Document.FocusMap.AddLayer(pGFLayer)
                    pFSele.Clear()
                End If
            End If
            If lstPseudoSnotelId.Count > 0 Then
                Dim query As String = BuildQueryFromOid(lstPseudoSnotelId)
                sitesLayerName = BA_EnumDescription(MapsLayerName.Snotel)
                Dim tempLayer As ILayer
                Dim snotelSrc As IFeatureLayer = Nothing
                For i = nlayers To 1 Step -1
                    tempLayer = CType(pMap.Layer(i - 1), ILayer)   'Explicit cast
                    If TypeOf tempLayer Is FeatureLayer AndAlso tempLayer.Name = sitesLayerName Then
                        snotelSrc = CType(tempLayer, IFeatureLayer)
                        Exit For
                    End If
                Next
                If snotelSrc IsNot Nothing Then
                    pFSele = TryCast(snotelSrc, IFeatureSelection)
                    pQFilter.WhereClause = query
                    pFSele.SelectFeatures(pQFilter, esriSelectionResultEnum.esriSelectionResultNew, False)
                    fLayerDef = CType(snotelSrc, IFeatureLayerDefinition)
                    snotelCopy = fLayerDef.CreateSelectionLayer(BA_MAPS_SNOTEL_SCENARIO2, True, Nothing, Nothing)
                    Dim pGFLayer As IGeoFeatureLayer = CType(snotelCopy, IGeoFeatureLayer)
                    pGFLayer.Renderer = pseudoRenderer
                    My.Document.FocusMap.AddLayer(pGFLayer)
                    pFSele.Clear()
                End If
            End If

            'Symbolize the selected snow course sites
            If lstActualSnowCourseId.Count > 0 Then
                Dim query As String = BuildQueryFromOid(lstActualSnowCourseId)
                sitesLayerName = BA_EnumDescription(MapsLayerName.SnowCourse)
                Dim tempLayer As ILayer
                Dim snowCourseSrc As IFeatureLayer = Nothing
                For i = nlayers To 1 Step -1
                    tempLayer = CType(pMap.Layer(i - 1), ILayer)   'Explicit cast
                    If TypeOf tempLayer Is FeatureLayer AndAlso tempLayer.Name = sitesLayerName Then
                        snowCourseSrc = CType(tempLayer, IFeatureLayer)
                        Exit For
                    End If
                Next
                If snowCourseSrc IsNot Nothing Then
                    pFSele = TryCast(snowCourseSrc, IFeatureSelection)
                    pQFilter.WhereClause = query
                    pFSele.SelectFeatures(pQFilter, esriSelectionResultEnum.esriSelectionResultNew, False)
                    fLayerDef = CType(snowCourseSrc, IFeatureLayerDefinition)
                    snowCourseCopy = fLayerDef.CreateSelectionLayer(BA_MAPS_SNOW_COURSE_SCENARIO1, True, Nothing, Nothing)
                    Dim pGFLayer As IGeoFeatureLayer = CType(snowCourseCopy, IGeoFeatureLayer)
                    pGFLayer.Renderer = actualRenderer
                    My.Document.FocusMap.AddLayer(pGFLayer)
                    pFSele.Clear()
                End If
            End If
            If lstPseudoSnowCourseId.Count > 0 Then
                Dim query As String = BuildQueryFromOid(lstPseudoSnowCourseId)
                sitesLayerName = BA_EnumDescription(MapsLayerName.SnowCourse)
                Dim tempLayer As ILayer
                Dim snowCourseSrc As IFeatureLayer = Nothing
                For i = nlayers To 1 Step -1
                    tempLayer = CType(pMap.Layer(i - 1), ILayer)   'Explicit cast
                    If TypeOf tempLayer Is FeatureLayer AndAlso tempLayer.Name = sitesLayerName Then
                        snowCourseSrc = CType(tempLayer, IFeatureLayer)
                        Exit For
                    End If
                Next
                If snowCourseSrc IsNot Nothing Then
                    pFSele = TryCast(snowCourseSrc, IFeatureSelection)
                    pQFilter.WhereClause = query
                    pFSele.SelectFeatures(pQFilter, esriSelectionResultEnum.esriSelectionResultNew, False)
                    fLayerDef = CType(snowCourseSrc, IFeatureLayerDefinition)
                    snowCourseCopy = fLayerDef.CreateSelectionLayer(BA_MAPS_SNOW_COURSE_SCENARIO2, True, Nothing, Nothing)
                    Dim pGFLayer As IGeoFeatureLayer = CType(snowCourseCopy, IGeoFeatureLayer)
                    pGFLayer.Renderer = pseudoRenderer
                    My.Document.FocusMap.AddLayer(pGFLayer)
                    pFSele.Clear()
                End If
            End If

            'Symbolize the selected pseudo sites
            If lstActualPseudoId.Count > 0 Then
                Dim query As String = BuildQueryFromOid(lstActualPseudoId)
                sitesLayerName = BA_EnumDescription(MapsLayerName.pseudo_sites)
                Dim tempLayer As ILayer
                Dim pseudoSrc As IFeatureLayer = Nothing
                For i = nlayers To 1 Step -1
                    tempLayer = CType(pMap.Layer(i - 1), ILayer)   'Explicit cast
                    If TypeOf tempLayer Is FeatureLayer AndAlso tempLayer.Name = sitesLayerName Then
                        pseudoSrc = CType(tempLayer, IFeatureLayer)
                        Exit For
                    End If
                Next
                If pseudoSrc IsNot Nothing Then
                    pFSele = TryCast(pseudoSrc, IFeatureSelection)
                    pQFilter.WhereClause = query
                    pFSele.SelectFeatures(pQFilter, esriSelectionResultEnum.esriSelectionResultNew, False)
                    fLayerDef = CType(pseudoSrc, IFeatureLayerDefinition)
                    pseudoCopy = fLayerDef.CreateSelectionLayer(BA_MAPS_PSEUDO_SCENARIO1, True, Nothing, Nothing)
                    Dim pGFLayer As IGeoFeatureLayer = CType(pseudoCopy, IGeoFeatureLayer)
                    pGFLayer.Renderer = actualRenderer
                    My.Document.FocusMap.AddLayer(pGFLayer)
                    pFSele.Clear()
                End If
            End If
            If lstPseudoPseudoId.Count > 0 Then
                Dim query As String = BuildQueryFromOid(lstPseudoPseudoId)
                sitesLayerName = BA_EnumDescription(MapsLayerName.pseudo_sites)
                Dim tempLayer As ILayer
                Dim pseudoSrc As IFeatureLayer = Nothing
                For i = nlayers To 1 Step -1
                    tempLayer = CType(pMap.Layer(i - 1), ILayer)   'Explicit cast
                    If TypeOf tempLayer Is FeatureLayer AndAlso tempLayer.Name = sitesLayerName Then
                        pseudoSrc = CType(tempLayer, IFeatureLayer)
                        Exit For
                    End If
                Next
                If pseudoSrc IsNot Nothing Then
                    pFSele = TryCast(pseudoSrc, IFeatureSelection)
                    pQFilter.WhereClause = query
                    pFSele.SelectFeatures(pQFilter, esriSelectionResultEnum.esriSelectionResultNew, False)
                    fLayerDef = CType(pseudoSrc, IFeatureLayerDefinition)
                    pseudoCopy = fLayerDef.CreateSelectionLayer(BA_MAPS_PSEUDO_SCENARIO2, True, Nothing, Nothing)
                    Dim pGFLayer As IGeoFeatureLayer = CType(pseudoCopy, IGeoFeatureLayer)
                    pGFLayer.Renderer = pseudoRenderer
                    My.Document.FocusMap.AddLayer(pGFLayer)
                    pFSele.Clear()
                End If
            End If

        Catch ex As Exception
            Debug.Print("SymbolizeSelectedSites Exception: " & ex.Message)
        Finally
            pQFilter = Nothing
            pFSele = Nothing
            fLayerDef = Nothing
        End Try
    End Sub

    Private Function BuildRendererForPoints(ByVal markerColor As IColor, ByVal size As Integer) As ISimpleRenderer
        Dim pFillColor As IColor = New RgbColor
        'Dim pMSymbol As IMarkerSymbol = Nothing
        Dim pMSymbol As ISimpleMarkerSymbol = New SimpleMarkerSymbol
        Dim pMask As IMask = Nothing
        Dim pRenderer As ISimpleRenderer = New SimpleRenderer

        Try
            pMSymbol.Style = esriSimpleMarkerStyle.esriSMSCircle
            pMSymbol.Size = size
            pMSymbol.OutlineColor = markerColor
            pMSymbol.Outline = True
            pMSymbol.OutlineSize = 3.0
            pFillColor.NullColor = True
            pMSymbol.Color = pFillColor
            pRenderer.Symbol = pMSymbol
            Return pRenderer
        Catch ex As Exception
            Debug.Print("BuildRendererForPoints Exception: " & ex.Message)
            Return Nothing
        Finally
            pRenderer = Nothing
            pMSymbol = Nothing
        End Try
    End Function

    Private Function BuildQueryFromOid(ByVal oidList As IList(Of Int32)) As String
        Dim sb As StringBuilder = New StringBuilder
        sb.Append(BA_FIELD_OBJECT_ID & " IN ( ")
        For Each oid As String In oidList
            sb.Append(oid & ", ")
        Next
        Dim query As String = sb.ToString.Trim
        query = query.TrimEnd(",")
        query = query & " )"
        Return query
    End Function

    Private Sub SaveAnalysisLog(ByVal pStepProg As IStepProgressor, ByVal progressDialog2 As IProgressDialog2)
        'We assume the AOI_ShapeArea is stored in KM because this assumption is made on frmAOIInfo
        Dim thisAnalysis As Analysis = New Analysis(m_CurrentAOI, AOIFolderBase, AOI_ShapeArea, txtMinElev.Text, txtMaxElev.Text, _
                                                    ChkBufferDistance.Checked, Convert.ToDouble(TxtBufferDistance.Text), m_oldBufferUnits, _
                                                    ChkUpperRange.Checked, m_upperElevRange, TxtUpperRange.Text, ChkLowerRange.Checked, _
                                                    m_lowerElevRange, TxtLowerRange.Text, m_oldElevUnits, TxtReportTitle.Text, _
                                                    TxtScenario1.Text, TxtScenario2.Text)
        'Set date created
        thisAnalysis.DateCreated = DateAndTime.Now
        'Create scenario 1 sites array
        Dim sitesArr(GrdScenario1.Rows.Count - 1) As Site
        Dim i As Integer = 0
        For Each pRow As DataGridViewRow In GrdScenario1.Rows
            Dim nextSite As Site = CreateSiteFromRow(pRow, False)
            sitesArr(i) = nextSite
            i += 1
        Next
        thisAnalysis.Scenario1Sites = sitesArr
        'Create scenario 1 sites array
        Dim sitesArr2(GrdScenario2.Rows.Count - 1) As Site
        i = 0
        For Each pRow As DataGridViewRow In GrdScenario2.Rows
            Dim nextSite As Site = CreateSiteFromRow(pRow, True)
            sitesArr2(i) = nextSite
            i += 1
        Next
        thisAnalysis.Scenario2Sites = sitesArr2
        Dim elevUnits As esriUnits = esriUnits.esriMeters
        If DemInMeters = False Then
            elevUnits = esriUnits.esriFeet
        End If
        thisAnalysis.AreaStatistics = BA_CalculateAreaStatistics(elevUnits, GrdScenario2.RowCount, pStepProg)
        Dim xmlOutputPath As String = BA_GetPath(AOIFolderBase, PublicPath.Maps) & BA_EnumDescription(PublicPath.AnalysisXml)
        thisAnalysis.Save(xmlOutputPath)
        If BA_File_ExistsWindowsIO(xmlOutputPath) Then
            BtnViewResult.Enabled = True
        Else
            BtnViewResult.Enabled = False
        End If

        'Hide progress dialog
        If progressDialog2 IsNot Nothing Then
            progressDialog2.HideDialog()
        End If
        progressDialog2 = Nothing
        pStepProg = Nothing
    End Sub

    Private Function SitePreviouslySelected(ByVal aSite As Site, ByVal sitesArr As Site()) As Boolean
        For i As Integer = 0 To sitesArr.GetUpperBound(0)
            Dim sSite As Site = sitesArr(i)
            If sSite.SiteType = aSite.SiteType AndAlso sSite.ObjectId = aSite.ObjectId Then
                Return sSite.IncludeInAnalysis
            End If
        Next
        Return False
    End Function

    'Reload all fields except the grids from the previous analysis
    Private Sub ReloadLastAnalysis(ByVal lastAnalysis As Analysis)
        If lastAnalysis IsNot Nothing Then
            ChkBufferDistance.Checked = lastAnalysis.UseBufferDistance
            Select Case lastAnalysis.BufferUnits
                Case esriUnits.esriMeters
                    CmboxDistanceUnit.SelectedIndex = 1
                Case esriUnits.esriFeet
                    CmboxDistanceUnit.SelectedIndex = 0
                Case esriUnits.esriKilometers
                    CmboxDistanceUnit.SelectedIndex = 3
                Case esriUnits.esriMiles
                    CmboxDistanceUnit.SelectedIndex = 2
            End Select
            m_oldBufferUnits = lastAnalysis.BufferUnits
            TxtBufferDistance.Text = Math.Round(lastAnalysis.BufferDistance, 3)
            ChkUpperRange.Checked = lastAnalysis.UseUpperRange
            ChkLowerRange.Checked = lastAnalysis.UseLowerRange
            Select Case lastAnalysis.ElevUnits
                Case esriUnits.esriMeters
                    OptZMeters.Checked = True
                Case esriUnits.esriFeet
                    OptZFeet.Checked = True
            End Select
            m_oldElevUnits = lastAnalysis.ElevUnits
            m_changingUnits = True
            m_upperElevRange = lastAnalysis.UpperRange
            m_lowerElevRange = lastAnalysis.LowerRange
            TxtUpperRange.Text = lastAnalysis.UpperRangeText
            TxtLowerRange.Text = lastAnalysis.LowerRangeText
            m_changingUnits = False
            TxtReportTitle.Text = lastAnalysis.ReportTitle
            TxtScenario1.Text = lastAnalysis.Scenario1Title
            TxtScenario2.Text = lastAnalysis.Scenario2Title
        End If
    End Sub

    Private Sub BtnReload_Click(sender As Object, e As System.EventArgs) Handles BtnReload.Click
        'Check for previously saved scenario and load those values as defaults
        Dim xmlOutputPath As String = BA_GetPath(AOIFolderBase, PublicPath.Maps) & BA_EnumDescription(PublicPath.AnalysisXml)
        Dim lastAnalysis As Analysis = Nothing
        ' Open analysis file if there is one
        If BA_File_ExistsWindowsIO(xmlOutputPath) Then
            lastAnalysis = BA_LoadAnalysisFromXml(AOIFolderBase)
            ReloadLastAnalysis(lastAnalysis)
        End If

        Dim elevUnits As esriUnits = esriUnits.esriMeters
        If DemInMeters = False Then
            elevUnits = esriUnits.esriFeet
        End If
        LoadScenarioSites(lastAnalysis, elevUnits, m_oldElevUnits)
    End Sub

    Private Sub BtnViewResult_Click(sender As Object, e As System.EventArgs) Handles BtnViewResult.Click
        Dim xslTemplate As String = BA_GetAddInDirectory() & BA_EnumDescription(PublicPath.AnalysisXsl)
        Dim xslFileExists As Boolean = BA_File_ExistsWindowsIO(xslTemplate)
        If xslFileExists Then
            Dim inputFile As String = BA_GetPath(AOIFolderBase, PublicPath.Maps) & BA_EnumDescription(PublicPath.AnalysisXml)
            Dim outputFile As String = BA_GetPath(AOIFolderBase, PublicPath.Maps) & BA_EnumDescription(PublicPath.ScenarioReportHtml)
            Dim success As BA_ReturnCode = BA_XSLTransformToHtml(inputFile, xslTemplate, outputFile)
            Dim sb As StringBuilder = New StringBuilder
            If success = BA_ReturnCode.Success Then
                Process.Start(outputFile)
            Else
                sb.Append("An error occcurred while trying to generate the html report at " & outputFile)
                sb.Append(". Please contact your system administrator.")
                MessageBox.Show(sb.ToString, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub

    'Enable the Calculate button only if at least one site is selected in scenario 1
    Public Sub ManageCalculateButton()
        Dim siteCount As Integer
        For Each aRow As DataGridViewRow In GrdScenario1.Rows
            Dim include As Boolean = Convert.ToBoolean(aRow.Cells(idxSelected).Value)
            If include = True Then
                siteCount += 1
            End If
        Next
        If siteCount < 1 Then
            BtnCalculate.Enabled = False
        Else
            BtnCalculate.Enabled = True
        End If
    End Sub

    Private Sub GrdScenario1_CellValueChanged(sender As Object, e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GrdScenario1.CellValueChanged
        'We only act if a checkbox cell was clicked
        If e.ColumnIndex = idxSelected AndAlso e.RowIndex > -1 Then
            ManageCalculateButton()
            If BtnCalculate.Enabled = False AndAlso m_btnCalculateMessage = True Then
                'Warn the user that the calculate button has been disabled
                Dim sb As StringBuilder = New StringBuilder
                sb.Append("The calculate button has been disabled. At least one site" & vbCrLf)
                sb.Append("from Scenario 1 must be selected to perform a calculation.")
                MessageBox.Show(sb.ToString, "No sites selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            m_btnCalculateMessage = True
        End If
    End Sub

    'Used to immediately commit changes when user checks the checkbox cell. We
    'do this so the CellValueChanged method fires and we can warn the user if he
    'has unchecked all the sites in Scenario 1
    Private Sub GrdScenario1_CurrentCellDirtyStateChanged(sender As Object, e As System.EventArgs) Handles GrdScenario1.CurrentCellDirtyStateChanged
        'We only act if a checkbox cell was clicked
        Dim pCell As DataGridViewCell = GrdScenario1.CurrentCell
        If pCell.ColumnIndex = idxSelected Then
            If GrdScenario1.IsCurrentCellDirty Then
                'If checkbox cell of grid is dirty then commit contents
                GrdScenario1.CommitEdit(DataGridViewDataErrorContexts.Commit)
            End If
        End If
    End Sub

    Private Sub ManageMapsButton()
        BtnMaps.Enabled = False
        Dim analysisFolder As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True)
        Dim FileName As String = BA_EnumDescription(MapsFileName.ActualRepresentedArea)
        If BA_File_Exists(analysisFolder & FileName, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            BtnMaps.Enabled = True
        End If
    End Sub

    Private Sub BtnMaps_Click(sender As System.Object, e As System.EventArgs) Handles BtnMaps.Click
        Try
            BA_AddScenarioLayersToMapFrame(My.ThisApplication, My.Document, AOIFolderBase)
            SymbolizeSelectedSites()
            Scenario1Map_Flag = True
            'Display scenario 1 map as default
            Dim Basin_Name As String
            Dim cboSelectedBasin = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of cboTargetedBasin)(My.ThisAddIn.IDs.cboTargetedBasin)
            If Len(Trim(cboSelectedBasin.getValue)) = 0 Then
                Basin_Name = ""
            Else
                Basin_Name = cboSelectedBasin.getValue
            End If
            BA_AddMapElements(My.Document, m_CurrentAOI & Basin_Name, "Subtitle BAGIS")
            Dim retVal As Integer = BA_DisplayMap(My.Document, 7, Basin_Name, m_CurrentAOI, Map_Display_Elevation_in_Meters, _
                                    Trim(TxtScenario1.Text))
            BA_RemoveLayersfromLegend(My.Document)
            Dim sb As StringBuilder = New StringBuilder
            sb.Append("Please use the menu items to view maps!")
            Dim analysisFolder As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True)
            Dim FileName As String = BA_EnumDescription(MapsFileName.PseudoRepresentedArea)
            If BA_File_Exists(analysisFolder & FileName, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                Scenario2Map_Flag = True
            Else
                Scenario2Map_Flag = False
            End If
            FileName = BA_EnumDescription(MapsFileName.DifferenceRepresentedArea)
            If BA_File_Exists(analysisFolder & FileName, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                RepDifferenceMap_Flag = True
            Else
                RepDifferenceMap_Flag = False
            End If
            SiteRepresentationMap_Flag = False
            If ChkBufferDistance.Checked = True Then
                'Assume there will be at least on selected site in scen 1 that should have a representation file
                For Each row As DataGridViewRow In GrdScenario1.Rows
                    Dim selected As Boolean = Convert.ToBoolean(row.Cells(idxSelected).Value)
                    If selected = True Then
                        Dim pSite As Site = CreateSiteFromRow(row, False)
                        FileName = BA_GetSiteScenarioFileName(analysisFolder, pSite)
                        If BA_File_Exists(analysisFolder & FileName, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                            SiteRepresentationMap_Flag = True
                            Exit For
                        End If
                    End If
                Next
            End If
            If SiteRepresentationMap_Flag = False Then
                sb.Append(vbCrLf & "The Site Representation button is disabled because a buffer distance was not used ")
                sb.Append("or because the site representation maps are no longer present.")
            End If

            Call BA_Enable_ScenarioMapFlags(True)
            MessageBox.Show(sb.ToString, "Site Scenario", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            Dim sb As StringBuilder = New StringBuilder
            sb.Append("An error occurred while trying to display the site scenario maps." & vbCrLf)
            sb.Append("Exception: " & ex.Message)
            MessageBox.Show(sb.ToString, "Site Scenario", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnPreview_Click(sender As System.Object, e As System.EventArgs) Handles BtnPreview.Click
        ' Create/configure a step progressor
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 10)
        pStepProg.Show()
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Generating preview buffers...", "Preview site buffers")
        progressDialog2.ShowDialog()
        Try

            '--- Calculate correct buffer distance based on XY units ---
            Dim bufferDistance As Double = Convert.ToDouble(TxtBufferDistance.Text)
            Dim converter As IUnitConverter = New UnitConverter
            If ChkBufferDistance.Checked = True Then
                If m_oldBufferUnits <> m_demXYUnits Then
                    bufferDistance = converter.ConvertUnits(bufferDistance, m_oldBufferUnits, m_demXYUnits)
                End If
            End If

            'Loop through the scenario 1 sites and oids for each selected site to the appropriate list
            Dim snotelList As IList(Of Int32) = New List(Of Int32)
            Dim snowCourseList As IList(Of Int32) = New List(Of Int32)
            Dim pseudoList As IList(Of Int32) = New List(Of Int32)
            For Each pRow As DataGridViewRow In GrdScenario1.Rows
                Dim selected As Boolean = Convert.ToBoolean(pRow.Cells(idxSelected).Value)
                If selected = True Then
                    Dim siteType As SiteType = BA_GetSiteType(Convert.ToString(pRow.Cells(idxSiteType).Value))
                    Dim oid As Int32 = Convert.ToInt32(pRow.Cells(idxObjectId).Value)
                    Select Case siteType
                        Case BAGIS.SiteType.Snotel
                            snotelList.Add(oid)
                        Case BAGIS.SiteType.SnowCourse
                            snowCourseList.Add(oid)
                        Case BAGIS.SiteType.Pseudo
                            pseudoList.Add(oid)
                    End Select
                End If
            Next

            'Initialize path values
            Dim layersFolder As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers)
            Dim analysisFolder As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis)
            Dim inputFile As String = Nothing
            Dim outputFile As String = Nothing

            '--- Remove any pre-existing layers ---
            Dim bufferFile(6) As String
            bufferFile(0) = BA_EnumDescription(MapsFileName.S1SnotelBuffers)
            bufferFile(1) = BA_EnumDescription(MapsFileName.S1SnowCourseBuffers)
            bufferFile(2) = BA_EnumDescription(MapsFileName.S1PseudoBuffers)
            bufferFile(3) = BA_EnumDescription(MapsFileName.S2SnotelBuffers)
            bufferFile(4) = BA_EnumDescription(MapsFileName.S2SnowCourseBuffers)
            bufferFile(5) = BA_EnumDescription(MapsFileName.S2PseudoBuffers)
            Dim layerName(6) As String
            layerName(0) = BA_EnumDescription(MapsLayerName.S1SnotelBuffers)
            layerName(1) = BA_EnumDescription(MapsLayerName.S1SnowCourseBuffers)
            layerName(2) = BA_EnumDescription(MapsLayerName.S1PseudoBuffers)
            layerName(3) = BA_EnumDescription(MapsLayerName.S2SnotelBuffers)
            layerName(4) = BA_EnumDescription(MapsLayerName.S2SnowCourseBuffers)
            layerName(5) = BA_EnumDescription(MapsLayerName.S2PseudoBuffers)

            'Toggle to data view
            BA_ToggleView(My.Document, True)

            For i As Integer = 0 To bufferFile.GetUpperBound(0)
                If BA_File_Exists(analysisFolder & "\" & bufferFile(i), WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                    BA_RemoveLayers(My.Document, layerName(i))
                    BA_Remove_ShapefileFromGDB(analysisFolder, bufferFile(i))
                End If
            Next

            'Initialize map values to be used in buffer creation
            Dim pSRef As ISpatialReference = My.Document.FocusMap.SpatialReference
            Dim pGraphicsContainer As IGraphicsContainer = CType(My.Document.FocusMap, ESRI.ArcGIS.Carto.IGraphicsContainer) ' Explicit Cast
            Dim success As BA_ReturnCode

            'Create buffer for s1 snotel sites
            Dim query As String = BuildQueryFromOid(snotelList)
            If query.Length > 0 Then
                inputFile = BA_EnumDescription(MapsFileName.Snotel)
                outputFile = bufferFile(0)
                success = BA_BufferPoints(layersFolder, inputFile, analysisFolder, outputFile, query, pSRef, _
                                          pGraphicsContainer, bufferDistance)
                pStepProg.Step()
            End If
            'Create buffer for s1 snow course sites
            query = BuildQueryFromOid(snowCourseList)
            If query.Length > 0 Then
                inputFile = BA_EnumDescription(MapsFileName.SnowCourse)
                outputFile = bufferFile(1)
                success = BA_BufferPoints(layersFolder, inputFile, analysisFolder, outputFile, query, pSRef, _
                                          pGraphicsContainer, bufferDistance)
                pStepProg.Step()
            End If
            'Create buffer for s1 pseudo sites
            query = BuildQueryFromOid(pseudoList)
            If query.Length > 0 Then
                inputFile = bufferFile(2)
                outputFile = BA_EnumDescription(MapsFileName.S1PseudoBuffers)
                success = BA_BufferPoints(layersFolder, inputFile, analysisFolder, outputFile, query, pSRef, _
                                          pGraphicsContainer, bufferDistance)
                pStepProg.Step()
            End If

            'Loop through the scenario 2 sites and oids to the appropriate list
            snotelList.Clear()
            snowCourseList.Clear()
            pseudoList.Clear()
            For Each pRow As DataGridViewRow In GrdScenario2.Rows
                Dim siteType As SiteType = BA_GetSiteType(Convert.ToString(pRow.Cells(idxSiteType - 1).Value))
                Dim oid As Int32 = Convert.ToInt32(pRow.Cells(idxObjectId - 1).Value)
                Select Case siteType
                    Case BAGIS.SiteType.Snotel
                        snotelList.Add(oid)
                    Case BAGIS.SiteType.SnowCourse
                        snowCourseList.Add(oid)
                    Case BAGIS.SiteType.Pseudo
                        pseudoList.Add(oid)
                End Select
            Next

            'Create buffer for s2 snotel sites
            query = BuildQueryFromOid(snotelList)
            If query.Length > 0 Then
                inputFile = BA_EnumDescription(MapsFileName.Snotel)
                outputFile = bufferFile(3)
                success = BA_BufferPoints(layersFolder, inputFile, analysisFolder, outputFile, query, pSRef, _
                                          pGraphicsContainer, bufferDistance)
                pStepProg.Step()
            End If
            'Create buffer for s2 snow course sites
            query = BuildQueryFromOid(snowCourseList)
            If query.Length > 0 Then
                inputFile = BA_EnumDescription(MapsFileName.SnowCourse)
                outputFile = bufferFile(4)
                success = BA_BufferPoints(layersFolder, inputFile, analysisFolder, outputFile, query, pSRef, _
                                          pGraphicsContainer, bufferDistance)
                pStepProg.Step()
            End If
            'Create buffer for s2 pseudo sites
            query = BuildQueryFromOid(pseudoList)
            If query.Length > 0 Then
                inputFile = BA_EnumDescription(MapsFileName.Pseudo)
                outputFile = bufferFile(5)
                success = BA_BufferPoints(layersFolder, inputFile, analysisFolder, outputFile, query, pSRef, _
                                          pGraphicsContainer, bufferDistance)
            End If

            progressDialog2.HideDialog()
            If m_baseLayers = False Then
                AddBaseLayers()
            End If

            'Assemble list of layers that should be visible in final output
            Dim layerNamesList As IList(Of String) = New List(Of String)
            layerNamesList.Add(BA_MAPS_FILLED_DEM)
            layerNamesList.Add(BA_MAPS_AOI_BOUNDARY)
            layerNamesList.Add(BA_MAPS_STREAMS)
            If AOI_HasSNOTEL Then
                layerNamesList.Add(BA_MAPS_SNOTEL_SITES)
            End If
            If AOI_HasSnowCourse Then
                layerNamesList.Add(BA_MAPS_SNOW_COURSE_SITES)
            End If
            If AOI_HasPseudoSite Then
                layerNamesList.Add(BA_MAPS_PSEUDO_SITES)
            End If

            'Add buffer layers
            Dim pDisplayColor As IRgbColor = New RgbColor
            Dim lineWidth As Double
            For i As Integer = 5 To 0 Step -1
                If BA_File_Exists(analysisFolder & "\" & bufferFile(i), WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                    If i < 3 Then
                        lineWidth = 2.0
                        pDisplayColor.RGB = RGB(0, 0, 0)  'Black for scenario 1
                    Else
                        lineWidth = 4.5
                        pDisplayColor.RGB = RGB(255, 0, 0)  'Red for scenario 2
                    End If
                    BA_AddExtentLayer(My.Document, analysisFolder & "\" & bufferFile(i), pDisplayColor, layerName(i), 1, 0, lineWidth)
                    layerNamesList.Add(layerName(i))
                End If
            Next
            'add aoi boundary and zoom to AOI
            Dim filepath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi, True)
            Dim FileName As String = BA_EnumDescription(AOIClipFile.AOIExtentCoverage)
            'Add the aoi boundary which zooms to the correct extent
            BA_AddExtentLayer(My.Document, filepath & FileName, Nothing, BA_MAPS_AOI_BOUNDARY, 0, 1.2, 2)
            Dim aoiLayer As ILayer = My.Document.FocusMap.Layer(0)
            'Move the layer to the right place in the stack
            My.Document.FocusMap.MoveLayer(aoiLayer, My.Document.FocusMap.LayerCount - 2)

            Dim LayerNames(layerNamesList.Count + 1) As String
            For i As Int32 = 0 To layerNamesList.Count - 1
                LayerNames(i + 1) = layerNamesList.Item(i)
            Next
            Dim retVal As Integer = BA_ToggleLayersinMapFrame(My.Document, LayerNames)
            My.Document.ActivatedView.Refresh()
        Catch ex As Exception
            Debug.Print("BtnPreview_Click Exception: " & ex.Message)
        Finally
            If progressDialog2 IsNot Nothing Then
                progressDialog2.HideDialog()
            End If
            progressDialog2 = Nothing
            pStepProg = Nothing
        End Try
    End Sub

    Private Function BuildSiteQuery(ByVal siteList As IList(Of Int32)) As String
        If siteList.Count > 0 Then
            Dim sb As StringBuilder = New StringBuilder
            If siteList.Count > 0 Then
                sb.Append("OBJECTID in (")
                For Each oid As Int32 In siteList
                    sb.Append(oid)
                    sb.Append(", ")
                Next
                ' Remove trailing comma
                sb.Remove(sb.Length - 2, 2)
                sb.Append(")")
            End If
            Return sb.ToString
        Else
            Return Nothing
        End If
    End Function

    Private Sub AddBaseLayers()
        'Display filled DEM
        Dim filepath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces, True)
        Dim FileName As String = BA_GetBareName(BA_EnumDescription(MapsFileName.filled_dem_gdb))
        Dim retVal As Integer = BA_MapDisplayRaster(My.Document, filepath & FileName, BA_MAPS_FILLED_DEM, 0)

        'add aoi streams layer
        filepath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers, True)
        FileName = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.AoiStreamsVector), False)
        Dim filepathname As String = filepath & FileName
        Dim pColor As IColor = New RgbColor
        pColor.RGB = RGB(0, 0, 255)
        Dim success As BA_ReturnCode = BA_AddLineLayer(My.Document, filepathname, BA_MAPS_STREAMS, pColor, 0)

        If AOI_HasSNOTEL Then
            filepath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers, True)
            FileName = BA_EnumDescription(MapsFileName.Snotel)
            pColor.RGB = RGB(0, 0, 255)
            retVal = BA_MapDisplayPointMarkers(My.ThisApplication, filepath & FileName, MapsLayerName.Snotel, pColor, MapsMarkerType.Snotel)
        End If
            If AOI_HasSnowCourse Then
                FileName = BA_EnumDescription(MapsFileName.SnowCourse)
                pColor.RGB = RGB(0, 255, 255) 'cyan
                retVal = BA_MapDisplayPointMarkers(My.ThisApplication, filepath & FileName, MapsLayerName.SnowCourse, pColor, MapsMarkerType.SnowCourse)
            End If
            If AOI_HasPseudoSite Then
                FileName = BA_EnumDescription(MapsFileName.Pseudo)
            pColor.RGB = RGB(255, 170, 0) 'electron gold
                retVal = BA_MapDisplayPointMarkers(My.ThisApplication, filepath & FileName, MapsLayerName.pseudo_sites, pColor, MapsMarkerType.PseudoSite)
            End If

            m_baseLayers = True
    End Sub

    Private Sub CalculateRepresentedArea()
        ' Create/configure a step progressor
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 15)
        pStepProg.Show()
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Calculating represented areas for AOI", "Calculating...")
        progressDialog2.ShowDialog()
        Try
            Dim layersFolder As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers)
            Dim analysisFolder As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis)
            Dim bufferDistance As Double = Convert.ToDouble(TxtBufferDistance.Text)
            '--- Delete any old _rep files ---
            '--- First, remove them from the map ---
            Dim pMap As IMap = My.Document.FocusMap
            Dim pTempLayer As ILayer
            For i = pMap.LayerCount To 1 Step -1
                pTempLayer = pMap.Layer(i - 1)
                Dim suffix As String = Microsoft.VisualBasic.Strings.Right(pTempLayer.Name, 4)
                If suffix = "_Rep" Then 'remove the layer
                    If TypeOf pTempLayer Is IRasterLayer Then 'disconnect a rasterlayer before removing it
                        Dim pDLayer As IDataLayer2 = CType(pTempLayer, IDataLayer2)
                        pDLayer.Disconnect()
                    End If
                    pMap.DeleteLayer(pTempLayer)
                End If
            Next
            '--- Then delete them ---
            Dim success As BA_ReturnCode = BA_RemoveFilesBySuffix(analysisFolder, "_Rep")
            '--- Delete raster versions of nppseduo, npactual, and npdiff if they were created by earlier version of BAGIS
            If BA_File_Exists(analysisFolder & "\" & BA_EnumDescription(MapsFileName.ActualRepresentedArea), WorkspaceType.Geodatabase, _
                              esriDatasetType.esriDTRasterDataset) Then
                BA_RemoveRasterFromGDB(analysisFolder, BA_EnumDescription(MapsFileName.ActualRepresentedArea))
                BA_RemoveRasterFromGDB(analysisFolder, BA_EnumDescription(MapsFileName.PseudoRepresentedArea))
                BA_RemoveRasterFromGDB(analysisFolder, BA_EnumDescription(MapsFileName.DifferenceRepresentedArea))
            End If

            '--- Calculate correct buffer distance based on XY units ---
            Dim converter As IUnitConverter = New UnitConverter
            If ChkBufferDistance.Checked = True Then
                If m_oldBufferUnits <> m_demXYUnits Then
                    bufferDistance = converter.ConvertUnits(bufferDistance, m_oldBufferUnits, m_demXYUnits)
                End If
            End If
            '--- Calculate correct upper and lower elevation thresholds based on DEM units ---
            Dim upperElev As Double = BA_9999
            Dim lowerElev As Double = BA_9999
            If ChkUpperRange.Checked Then
                upperElev = Convert.ToDouble(TxtUpperRange.Text)
                If m_demInMeters = False AndAlso OptZMeters.Checked = True Then
                    upperElev = converter.ConvertUnits(upperElev, esriUnits.esriMeters, esriUnits.esriFeet)
                ElseIf m_demInMeters = True AndAlso OptZMeters.Checked = False Then
                    upperElev = converter.ConvertUnits(upperElev, esriUnits.esriFeet, esriUnits.esriMeters)
                End If
            End If
            If ChkLowerRange.Checked Then
                lowerElev = Convert.ToDouble(TxtLowerRange.Text)
                If m_demInMeters = False AndAlso OptZMeters.Checked = True Then
                    lowerElev = converter.ConvertUnits(lowerElev, esriUnits.esriMeters, esriUnits.esriFeet)
                ElseIf m_demInMeters = True AndAlso OptZMeters.Checked = False Then
                    lowerElev = converter.ConvertUnits(lowerElev, esriUnits.esriFeet, esriUnits.esriMeters)
                End If
            End If

            '--- Actual Representation ---
            success = BA_ReturnCode.UnknownError
            Dim siteList As IList(Of Site) = New List(Of Site)
            '--- Use buffer distance ---
            If ChkBufferDistance.Checked = True Then
                Dim sb As StringBuilder = New StringBuilder 'StringBuffer to hold names of rasters to combine
                For Each aRow As DataGridViewRow In GrdScenario1.Rows
                    Dim selected As Boolean = Convert.ToBoolean(aRow.Cells(idxSelected).Value)
                    If selected = True Then
                        Dim aSite As Site = CreateSiteFromRow(aRow, False)
                        pStepProg.Message = "Create area map for scenario 1 site " & aSite.Name
                        pStepProg.Step()
                        System.Windows.Forms.Application.DoEvents()
                        Dim pointFileName As String = Nothing
                        Select Case aSite.SiteType
                            Case SiteType.Pseudo
                                pointFileName = BA_EnumDescription(MapsFileName.Pseudo)
                            Case SiteType.Snotel
                                pointFileName = BA_EnumDescription(MapsFileName.Snotel)
                            Case SiteType.SnowCourse
                                pointFileName = BA_EnumDescription(MapsFileName.SnowCourse)
                        End Select
                        success = BA_CreateBufferPolygonFile(layersFolder, pointFileName, aSite.ObjectId, bufferDistance, _
                                                             AOIFolderBase, BA_BufferDistanceFile, _
                                                             My.Document.ActiveView.FocusMap.SpatialReference)
                        If success = BA_ReturnCode.Success Then
                            Dim surfacesFolder As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces)
                            Dim siteRepName As String = BA_GetSiteScenarioFileName(analysisFolder, aSite)
                            'sitename_elev
                            success = BA_ExtractRepresentedArea(AOIFolderBase, BA_BufferDistanceFile, surfacesFolder, BA_EnumDescription(MapsFileName.filled_dem_gdb), _
                                                                analysisFolder, siteRepName, aSite, upperElev, lowerElev, ES_DEMMax, ES_DEMMin)
                            If success = BA_ReturnCode.Success Then
                                sb.Append(analysisFolder & "\" & siteRepName)
                                sb.Append(";")
                                siteList.Add(aSite)
                            End If
                        End If
                    End If
                Next
                If sb.Length > 1 Then
                    Dim inFeatures As String = sb.ToString.TrimEnd(";")
                    Dim snapRasterPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
                    pStepProg.Message = "Create represented area map for scenario 1"
                    pStepProg.Step()
                    System.Windows.Forms.Application.DoEvents()
                    'Merge features for each site into a single shapefile (note: merge doesn't work with FGDB and features with different extents)
                    Dim mergeName As String = "tmpMerge"
                    success = BA_MergeFeatures(inFeatures, AOIFolderBase & "\" & mergeName, snapRasterPath)
                    If success = BA_ReturnCode.Success Then
                        'Dissolve features to eliminate overlap
                        mergeName = BA_StandardizeShapefileName(mergeName, True, True)
                        success = BA_Dissolve(AOIFolderBase & mergeName, BA_FIELD_GRIDCODE, analysisFolder & "\" & BA_EnumDescription(MapsFileName.ActualRepresentedArea))
                        'Delete temporary merge file
                        BA_Remove_Shapefile(AOIFolderBase, BA_StandardizeShapefileName(mergeName, False))
                    End If
                End If
            Else
                pStepProg.Message = "Create elevation only map for scenario 1"
                pStepProg.Step()
                System.Windows.Forms.Application.DoEvents()
                'Use elevation only
                Dim siteMin As Double = ES_DEMMax
                Dim siteMax As Double = ES_DEMMin
                'Get the lowest/highest elevation from all the selected sites 
                For Each aRow As DataGridViewRow In GrdScenario1.Rows
                    Dim selected As Boolean = Convert.ToBoolean(aRow.Cells(idxSelected).Value)
                    If selected = True Then
                        Dim aSite As Site = CreateSiteFromRow(aRow, False)
                        If aSite.Elevation < siteMin Then
                            siteMin = aSite.Elevation
                        End If
                        If aSite.Elevation > siteMax Then
                            siteMax = aSite.Elevation
                        End If
                    End If
                Next
                'Reclass site according to lowest/highest elevation and upper/lower thresholds
                Dim demFilePath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces, True) & BA_EnumDescription(MapsFileName.filled_dem_gdb)
                Dim snapRasterPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
                Dim reclassName As String = "reclElev"
                Dim mergeName As String = "tmpMerge"
                Dim outputPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True) & BA_EnumDescription(MapsFileName.ActualRepresentedArea)
                success = BA_ReclassByElevationOnly(demFilePath, AOIFolderBase & "\" & reclassName, snapRasterPath, siteMax, _
                                                    siteMin, upperElev, lowerElev, ES_DEMMax, ES_DEMMin)
                'Check to see if output file already exists and delete if it does
                If BA_Shapefile_Exists(AOIFolderBase & "\" & mergeName) Then
                    BA_Remove_Shapefile(AOIFolderBase, mergeName)
                End If
                Dim reclRaster As IGeoDataset = BA_OpenRasterFromFile(AOIFolderBase, reclassName)
                If reclRaster IsNot Nothing Then
                    BA_Raster2PolygonShapefile(AOIFolderBase, mergeName, reclRaster)
                    success = BA_Dissolve(AOIFolderBase & BA_StandardizeShapefileName(mergeName), BA_FIELD_GRIDCODE, outputPath)
                End If
                'Remove temporary files
                reclRaster = Nothing
                BA_Remove_Raster(AOIFolderBase, reclassName)
                BA_Remove_Shapefile(AOIFolderBase, mergeName)

            End If
            '--- Pseudo Representation ---
            If GrdScenario2.Rows.Count > 0 Then
                '--- Use buffer distance ---
                If ChkBufferDistance.Checked = True Then
                    Dim sb As StringBuilder = New StringBuilder 'StringBuffer to hold names of rasters to combine
                    Dim s2List As IList(Of Site) = New List(Of Site)
                    For Each aRow As DataGridViewRow In GrdScenario2.Rows
                        Dim aSite As Site = CreateSiteFromRow(aRow, True)
                        pStepProg.Message = "Create area map for scenario 2 site " & aSite.Name
                        pStepProg.Step()
                        System.Windows.Forms.Application.DoEvents()
                        Dim siteRepName As String = BA_GetSiteScenarioFileName(analysisFolder, aSite)
                        'Check to see if the buffered file already exists from the actual analysis
                        If Not BA_File_Exists(analysisFolder & "\" & siteRepName, WorkspaceType.Geodatabase, ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureClass) Then
                            Dim pointFileName As String = Nothing
                            Select Case aSite.SiteType
                                Case SiteType.Pseudo
                                    pointFileName = BA_EnumDescription(MapsFileName.Pseudo)
                                Case SiteType.Snotel
                                    pointFileName = BA_EnumDescription(MapsFileName.Snotel)
                                Case SiteType.SnowCourse
                                    pointFileName = BA_EnumDescription(MapsFileName.SnowCourse)
                            End Select
                            success = BA_CreateBufferPolygonFile(layersFolder, pointFileName, aSite.ObjectId, bufferDistance, _
                                                                 AOIFolderBase, BA_BufferDistanceFile, _
                                                                 My.Document.ActiveView.FocusMap.SpatialReference)
                            If success = BA_ReturnCode.Success Then
                                Dim surfacesFolder As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces)
                                'sitename_elev
                                success = BA_ExtractRepresentedArea(AOIFolderBase, BA_BufferDistanceFile, surfacesFolder, BA_EnumDescription(MapsFileName.filled_dem_gdb), _
                                                                    analysisFolder, siteRepName, aSite, upperElev, lowerElev, ES_DEMMax, ES_DEMMin)
                                If success = BA_ReturnCode.Success Then
                                    sb.Append(analysisFolder & "\" & siteRepName)
                                    sb.Append(";")
                                    siteList.Add(aSite)
                                End If
                            End If
                        Else
                            'The site rep file already exists from actual analysis so we just need to append the name to the sb
                            sb.Append(analysisFolder & "\" & siteRepName)
                            sb.Append(";")
                        End If
                    Next
                    If sb.Length > 1 Then
                        Dim inFeatures As String = sb.ToString.TrimEnd(";")
                        Dim snapRasterPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
                        pStepProg.Message = "Create represented area map for scenario 2"
                        pStepProg.Step()
                        System.Windows.Forms.Application.DoEvents()

                        'Merge features for each site into a single shapefile (note: merge doesn't work with FGDB and features with different extents)
                        Dim mergeName As String = "tmpMerge"
                        success = BA_MergeFeatures(inFeatures, AOIFolderBase & "\" & mergeName, snapRasterPath)
                        If success = BA_ReturnCode.Success Then
                            'Dissolve features to eliminate overlap
                            mergeName = BA_StandardizeShapefileName(mergeName, True, True)
                            success = BA_Dissolve(AOIFolderBase & mergeName, BA_FIELD_GRIDCODE, analysisFolder & "\" & BA_EnumDescription(MapsFileName.PseudoRepresentedArea))
                            'Delete temporary merge file
                            BA_Remove_Shapefile(AOIFolderBase, BA_StandardizeShapefileName(mergeName, False))
                            Scenario2Map_Flag = True
                        End If
                    End If
                Else
                    pStepProg.Message = "Create elevation only map for scenario 2"
                    pStepProg.Step()
                    System.Windows.Forms.Application.DoEvents()

                    'Use elevation only
                    Dim siteMin As Double = ES_DEMMax
                    Dim siteMax As Double = ES_DEMMin
                    'Get the lowest/highest elevation from all the selected sites 
                    For Each aRow As DataGridViewRow In GrdScenario1.Rows
                        Dim selected As Boolean = Convert.ToBoolean(aRow.Cells(idxSelected).Value)
                        If selected = True Then
                            Dim aSite As Site = CreateSiteFromRow(aRow, False)
                            If aSite.Elevation < siteMin Then
                                siteMin = aSite.Elevation
                            End If
                            If aSite.Elevation > siteMax Then
                                siteMax = aSite.Elevation
                            End If
                        End If
                    Next
                    'Reclass site according to lowest/highest elevation and upper/lower thresholds
                    Dim demFilePath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces, True) & BA_EnumDescription(MapsFileName.filled_dem_gdb)
                    Dim snapRasterPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi) & BA_EnumDescription(PublicPath.AoiGrid)
                    Dim reclassName As String = "reclElev"
                    Dim mergeName As String = "tmpMerge"
                    Dim outputPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True) & BA_EnumDescription(MapsFileName.PseudoRepresentedArea)
                    success = BA_ReclassByElevationOnly(demFilePath, AOIFolderBase & "\" & reclassName, snapRasterPath, siteMax, _
                                                        siteMin, upperElev, lowerElev, ES_DEMMax, ES_DEMMin)
                    'Check to see if output file already exists and delete if it does
                    If BA_Shapefile_Exists(AOIFolderBase & "\" & mergeName) Then
                        BA_Remove_Shapefile(AOIFolderBase, mergeName)
                    End If
                    Dim reclRaster As IGeoDataset = BA_OpenRasterFromFile(AOIFolderBase, reclassName)
                    If reclRaster IsNot Nothing Then
                        BA_Raster2PolygonShapefile(AOIFolderBase, mergeName, reclRaster)
                        success = BA_Dissolve(AOIFolderBase & BA_StandardizeShapefileName(mergeName), BA_FIELD_GRIDCODE, outputPath)
                        Scenario2Map_Flag = True
                    End If
                    'Remove temporary files
                    reclRaster = Nothing
                    BA_Remove_Raster(AOIFolderBase, reclassName)
                    BA_Remove_Shapefile(AOIFolderBase, mergeName)
                End If
            Else
                'Delete old scenario sites layer to avoid confusion
                If BA_File_Exists(analysisFolder & "\" & BA_EnumDescription(MapsFileName.PseudoRepresentedArea), WorkspaceType.Geodatabase, _
                                  ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureClass) Then
                    BA_Remove_ShapefileFromGDB(analysisFolder, BA_EnumDescription(MapsFileName.PseudoRepresentedArea))
                End If
                'Delete old difference layer to avoid confusion
                If BA_File_Exists(analysisFolder & "\" & BA_EnumDescription(MapsFileName.DifferenceRepresentedArea), WorkspaceType.Geodatabase, _
                                  ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureClass) Then
                    BA_Remove_ShapefileFromGDB(analysisFolder, BA_EnumDescription(MapsFileName.DifferenceRepresentedArea))
                End If
                Scenario2Map_Flag = False
            End If
            RepDifferenceMap_Flag = False
            If success = BA_ReturnCode.Success Then
                'We have a second scenario and need to combine the maps
                If Scenario2Map_Flag = True Then
                    pStepProg.Message = "Create difference in representation map"
                    pStepProg.Step()
                    Dim layerList As IList(Of String) = New List(Of String)
                    layerList.Add(analysisFolder & "\" & BA_EnumDescription(MapsFileName.ActualRepresentedArea))
                    layerList.Add(analysisFolder & "\" & BA_EnumDescription(MapsFileName.PseudoRepresentedArea))
                    'Try using this for mask also
                    Dim snapRasterPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi) & "\" & BA_EnumDescription(AOIClipFile.BufferedAOIExtentCoverage)
                    success = BA_CreateRepresentationDifference(snapRasterPath, layerList, analysisFolder, _
                                                                BA_EnumDescription(MapsFileName.DifferenceRepresentedArea))
                    If success = BA_ReturnCode.Success Then
                        RepDifferenceMap_Flag = True
                    End If
                End If

                pStepProg.Message = "Saving analysis settings"
                pStepProg.Step()
                SaveAnalysisLog(pStepProg, progressDialog2)

                'Add the site scenario layers to the map frame
                BA_AddScenarioLayersToMapFrame(My.ThisApplication, My.Document, AOIFolderBase)
                SymbolizeSelectedSites()
                Scenario1Map_Flag = True
                'Display scenario 1 map as default
                Dim Basin_Name As String
                Dim cboSelectedBasin = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of cboTargetedBasin)(My.ThisAddIn.IDs.cboTargetedBasin)
                If Len(Trim(cboSelectedBasin.getValue)) = 0 Then
                    Basin_Name = ""
                Else
                    Basin_Name = cboSelectedBasin.getValue
                End If
                BA_AddMapElements(My.Document, m_CurrentAOI & Basin_Name, "Subtitle BAGIS")
                Dim retVal As Integer = BA_DisplayMap(My.Document, 7, Basin_Name, m_CurrentAOI, Map_Display_Elevation_in_Meters, _
                                        Trim(TxtScenario1.Text))
                BA_RemoveLayersfromLegend(My.Document)
                Dim sb As StringBuilder = New StringBuilder
                sb.Append("Please use the menu items to view maps!")
                SiteRepresentationMap_Flag = ChkBufferDistance.Checked
                If SiteRepresentationMap_Flag = False Then
                    sb.Append(vbCrLf & "The Site Representation button is disabled because a buffer distance was not used.")
                End If
                MessageBox.Show(sb.ToString, "Site Scenario", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Call BA_Enable_ScenarioMapFlags(True)
            End If
            ManageMapsButton()

        Catch ex As Exception
            Debug.Print("CalculateRepresentedArea Exception: " & ex.Message)
        Finally
            If progressDialog2 IsNot Nothing Then
                progressDialog2.HideDialog()
            End If
            progressDialog2 = Nothing
            pStepProg = Nothing
        End Try
    End Sub

End Class