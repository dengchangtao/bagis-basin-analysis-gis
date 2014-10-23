Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Geodatabase
Imports System.IO
Imports System.Text
Imports System.Windows.Forms

Public Class frmElevScenario

    Private DEMConversion_Factor As Double
    Private STConversion_Factor As Double
    Private SCConversion_Factor As Double
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
    Private m_demInMeters As Boolean

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        If Len(AOIFolderBase) = 0 Then
            MsgBox("Please select an AOI first!")
            Exit Sub
        End If

        Dim filepath As String, FileName As String, filenamepath As String
        Dim response As Integer, previous_result As Integer

        filepath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True)
        FileName = BA_ScenarioResult
        filenamepath = filepath & FileName

        'convert unit to internal system unit, i.e., meters
        Dim elevUnit As MeasurementUnit = BA_GetElevationUnitsForAOI(AOIFolderBase)
        If elevUnit = MeasurementUnit.Missing Then
            Dim aoiName As String = BA_GetBareName(AOIFolderBase)
            Dim pAoi As Aoi = New Aoi(aoiName, AOIFolderBase, "", "")
            Dim frmDataUnits As FrmDataUnits = New FrmDataUnits(pAoi)
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
        If elevUnit = MeasurementUnit.Meters Then
            m_demInMeters = True
        End If

        'check if the result file exists
        If BA_File_ExistsWindowsIO(filenamepath) Then previous_result = S_ReadResults()

        'determine internal conversion factors
        STConversion_Factor = BA_SetConversionFactor(m_demInMeters, True) 'internal ST and SC elevation unit was converted to meters when clipped to AOI
        SCConversion_Factor = BA_SetConversionFactor(m_demInMeters, True)

        'determine display conversion factor
        DEMConversion_Factor = BA_SetConversionFactor(m_demInMeters, OptZMeters.Checked)

        If previous_result > 0 Then 'note: all ES_ variables are in the same unit as DEM
            ES_DEMMin = Val(txtMinElev.Text) * DEMConversion_Factor
            ES_DEMMax = Val(txtMaxElev.Text) * DEMConversion_Factor
            If Val(txtSNOTELMin.Text) > 0 Then
                ES_STMin = Val(txtSNOTELMin.Text) * DEMConversion_Factor
                ES_STMax = Val(txtSNOTELMax.Text) * DEMConversion_Factor
            Else
                ES_STMin = 0
                ES_STMax = 0
            End If

            If Val(txtSNOTELMin.Text) > 0 Then
                ES_SCMin = Val(txtSCourseMin.Text) * DEMConversion_Factor
                ES_SCMax = Val(txtSCourseMax.Text) * DEMConversion_Factor
            Else
                ES_SCMin = 0
                ES_SCMax = 0
            End If

            ShowOutput()
            Exit Sub
        End If

        'no pre-existing results
        If m_demInMeters Then 'meters
            Set_LabelUnit("Meters")
        Else
            Set_LabelUnit("Feet")
        End If
        'DEMConversion_Factor = 1

        'get min and max of AOI DEM
        'display dem elevation stats
        Dim OutputPath As String

        Dim demRasterStats As IRasterStatistics = BA_GetDemStatsGDB(AOIFolderBase)
        ES_DEMMin = demRasterStats.Minimum
        ES_DEMMax = demRasterStats.Maximum

        'Populate Boxes
        txtMinElev.Text = Math.Round(ES_DEMMin - 0.005, 2)
        txtMaxElev.Text = Math.Round(ES_DEMMax + 0.005, 2)

        'check zone rasters
        'BA_RasterSNOTELZones = "stelzone"
        'BA_RasterSnowCourseZones = "scoszone"
        OutputPath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis)
        Dim AOILayerPath As String
        Dim nclass As Integer
        Dim IntervalList() As BA_IntervalList = Nothing

        'check if snotel and snow course layers exist
        Dim Has_SNOTELLayer As Boolean, Has_SnowCourseLayer As Boolean
        AOILayerPath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers) 'clipped SNOTEL and SnowCourse are in this folder

        'Check to see if SNOTEL Shapefile Exist
        If BA_File_Exists(AOILayerPath & "\" & BA_SNOTELSites, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            Has_SNOTELLayer = True
        Else
            Has_SNOTELLayer = False
        End If

        'Check to see if SnowCourse Shapefile Exist
        If BA_File_Exists(AOILayerPath & "\" & BA_SnowCourseSites, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            Has_SnowCourseLayer = True
        Else
            Has_SnowCourseLayer = False
        End If

        If Has_SNOTELLayer Then
            'read snotel
            response = BA_GetUniqueSortedValues(AOILayerPath, BA_SNOTELSites, "", BA_SiteElevField, _
                                                ES_DEMMin / STConversion_Factor, ES_DEMMax / STConversion_Factor, IntervalList)
            If response > 0 Then
                nclass = UBound(IntervalList)
                ES_STMin = IntervalList(1).UpperBound * STConversion_Factor
                ES_STMax = IntervalList(nclass).LowerBound * STConversion_Factor
                txtSNOTELMin.Text = Math.Round(ES_STMin, 2)
                txtSNOTELMax.Text = Math.Round(ES_STMax, 2)
            Else
                txtSNOTELMin.Text = "Error!"
                txtSNOTELMax.Text = "Error!"
                MsgBox("Error detected in the SNOTEL elevation data!")
            End If
        Else
            txtSNOTELMin.Text = "No data"
            txtSNOTELMax.Text = "No data"
        End If

        If Has_SnowCourseLayer Then 'the zone rasters exist
            'MsgBox "Snow Course Conversion: " & SCConversion_Factor
            'read snow course
            response = BA_GetUniqueSortedValues(AOILayerPath, BA_SnowCourseSites, "", BA_SiteElevField, _
                                                ES_DEMMin / SCConversion_Factor, ES_DEMMax / SCConversion_Factor, IntervalList)
            If response > 0 Then
                nclass = UBound(IntervalList)
                ES_SCMin = IntervalList(1).UpperBound * SCConversion_Factor
                ES_SCMax = IntervalList(nclass).LowerBound * SCConversion_Factor
                txtSCourseMin.Text = Math.Round(ES_SCMin, 2)
                txtSCourseMax.Text = Math.Round(ES_SCMax, 2)
            Else
                txtSCourseMin.Text = "Error!"
                txtSCourseMax.Text = "Error!"
                MsgBox("Error detected in the Snow Course elevation data!")
            End If
        Else
            txtSCourseMin.Text = "No data"
            txtSCourseMax.Text = "No data"
        End If
    End Sub

    Private Sub cmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClose.Click
        Me.Close()
    End Sub

    Private Function S_ReadResults() As Integer
        Dim FileName As String, filepath As String
        Dim linestring As String
        Dim filepathname As String
        Dim sr As StreamReader = Nothing

        If Len(AOIFolderBase) = 0 Then
            S_ReadResults = 0
            Exit Function
        End If

        Try
            filepath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True)
            FileName = BA_ScenarioResult
            filepathname = filepath & FileName

            'open file for input
            If BA_File_ExistsWindowsIO(filepathname) Then
                sr = File.OpenText(filepathname)
            Else
                'MsgBox("File " & filepathname & " does not exist and cannot be opened.")
                Return -1
            End If

            'read the version text
            linestring = sr.ReadLine
            'read the file title
            linestring = sr.ReadLine

            'Z unit
            linestring = sr.ReadLine    'heading "*** Elevation Z Unit ***"
            linestring = sr.ReadLine    'read Z unit text

            If linestring = "Meters" Then
                Set_LabelUnit("Meters")
            Else
                Set_LabelUnit("Feet")
            End If

            'DEM min and max
            linestring = sr.ReadLine    'heading "*** AOI Elevation min and max ***"
            linestring = sr.ReadLine
            txtMinElev.Text = linestring
            linestring = sr.ReadLine
            txtMaxElev.Text = linestring


            'SNOTEL sites min and max
            linestring = sr.ReadLine    'heading "*** SNOTEL Elevation min and max ***"
            linestring = sr.ReadLine
            txtSNOTELMin.Text = linestring
            linestring = sr.ReadLine
            txtSNOTELMax.Text = linestring

            'Snow course sites min and max
            linestring = sr.ReadLine    'heading "*** Snow Course Elevation min and max ***"
            linestring = sr.ReadLine
            txtSCourseMin.Text = linestring
            linestring = sr.ReadLine
            txtSCourseMax.Text = linestring

            'Lower range elevation buffer
            linestring = sr.ReadLine    'heading "*** Lower Range Extended Buffer ***"
            linestring = sr.ReadLine
            txtLowerBuffer.Text = linestring

            'Actual Elevation lower and upper bounds
            linestring = sr.ReadLine    'heading "*** Actual Elevation lowerbnd and upperbnd ***"
            linestring = sr.ReadLine
            txtActualLow.Text = linestring
            linestring = sr.ReadLine
            txtActualHigh.Text = linestring

            'Actual Elevation non-represented area in acres
            linestring = sr.ReadLine    'heading "*** Actual Non-represented Below, Above, and Total in Acres ***"
            linestring = sr.ReadLine
            txtA_BelowArea1.Text = linestring
            linestring = sr.ReadLine
            txtA_AboveArea1.Text = linestring
            linestring = sr.ReadLine
            txtA_TotalArea1.Text = linestring

            'Actual Elevation non-represented area in Ha
            linestring = sr.ReadLine    'heading "*** Actual Non-represented Below, Above, and Total in Hectares ***"
            linestring = sr.ReadLine
            txtA_BelowArea2.Text = linestring
            linestring = sr.ReadLine
            txtA_AboveArea2.Text = linestring
            linestring = sr.ReadLine
            txtA_TotalArea2.Text = linestring

            'Actual Elevation non-represented area in %
            linestring = sr.ReadLine    'heading "*** Actual Non-represented Below, Above, and Total in Percentage ***"
            linestring = sr.ReadLine
            txtA_BelowPercent.Text = linestring
            linestring = sr.ReadLine
            txtA_AbovePercent.Text = linestring
            linestring = sr.ReadLine
            txtA_TotalPercent.Text = linestring

            'Pseudo Elevation lower and upper bounds
            linestring = sr.ReadLine    'heading "*** Pseudo Elevation lowerbnd and upperbnd ***"
            linestring = sr.ReadLine
            txtNewLow.Text = linestring
            linestring = sr.ReadLine
            txtNewHigh.Text = linestring

            'Pseudo Elevation non-represented area in acres
            linestring = sr.ReadLine    'heading "*** Pseudo Non-represented Below, Above, and Total in Acres ***"
            linestring = sr.ReadLine
            txtP_BelowArea1.Text = linestring
            linestring = sr.ReadLine
            txtP_AboveArea1.Text = linestring
            linestring = sr.ReadLine
            txtP_TotalArea1.Text = linestring

            'Pseudo Elevation non-represented area in Ha
            linestring = sr.ReadLine    'heading "*** Pseudo Non-represented Below, Above, and Total in Hectares ***"
            linestring = sr.ReadLine
            txtP_BelowArea2.Text = linestring
            linestring = sr.ReadLine
            txtP_AboveArea2.Text = linestring
            linestring = sr.ReadLine
            txtP_TotalArea2.Text = linestring

            'Pseudo Elevation non-represented area in %
            linestring = sr.ReadLine    'heading "*** Pseudo Non-represented Below, Above, and Total in Percentage ***"
            linestring = sr.ReadLine
            txtP_BelowPercent.Text = linestring
            linestring = sr.ReadLine
            txtP_AbovePercent.Text = linestring
            linestring = sr.ReadLine
            txtP_TotalPercent.Text = linestring
            Return 1
        Catch ex As Exception
            Debug.Print("S_ReadResults Exception: " & ex.Message)
            Return -1
        Finally
            'Don't forget to close the file handle
            If sr IsNot Nothing Then sr.Close()
        End Try
    End Function

    Private Function S_SaveResults() As Integer
        Dim FileName As String, filepath As String
        Dim filepathname As String
        Dim linestring As String
        Dim sw As StreamWriter = Nothing

        If Len(AOIFolderBase) = 0 Then
            Return -1
        End If

        Try
            filepath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis)
            FileName = BA_ScenarioResult
            filepathname = filepath & "\" & FileName

            'open file for output
            sw = New StreamWriter(filepathname)
            sw.WriteLine(BA_VersionText)
            sw.WriteLine("*** Elevation Scenario Output ***")

            'Z unit
            sw.WriteLine("*** Elevation Z Unit ***")
            If OptZMeters.Checked = True Then
                linestring = "Meters"
            Else
                linestring = "Feet"
            End If
            sw.WriteLine(linestring)

            'DEM min and max
            sw.WriteLine("*** AOI Elevation min and max ***")
            sw.WriteLine(txtMinElev.Text)
            sw.WriteLine(txtMaxElev.Text)

            'SNOTEL sites min and max
            sw.WriteLine("*** SNOTEL Elevation min and max ***")
            sw.WriteLine(txtSNOTELMin.Text)
            sw.WriteLine(txtSNOTELMax.Text)

            'Snow course sites min and max
            sw.WriteLine("*** Snow Course Elevation min and max ***")
            sw.WriteLine(txtSCourseMin.Text)
            sw.WriteLine(txtSCourseMax.Text)

            'Lower range elevation buffer
            sw.WriteLine("*** Lower Range Extended Buffer ***")
            sw.WriteLine(txtLowerBuffer.Text)

            'Actual Elevation lower and upper bounds
            sw.WriteLine("***  Actual Elevation lowerbnd and upperbnd ***")
            sw.WriteLine(txtActualLow.Text)
            sw.WriteLine(txtActualHigh.Text)

            'Actual Elevation non-represented area in acres
            sw.WriteLine("***  Actual Non-represented Below, Above, and Total in Acres ***")
            sw.WriteLine(txtA_BelowArea1.Text)
            sw.WriteLine(txtA_AboveArea1.Text)
            sw.WriteLine(txtA_TotalArea1.Text)

            'Actual Elevation non-represented area in Ha
            sw.WriteLine("***  Actual Non-represented Below, Above, and Total in Hectares ***")
            sw.WriteLine(txtA_BelowArea2.Text)
            sw.WriteLine(txtA_AboveArea2.Text)
            sw.WriteLine(txtA_TotalArea2.Text)

            'Actual Elevation non-represented area in %
            sw.WriteLine("***  Actual Non-represented Below, Above, and Total in Percentage ***")
            sw.WriteLine(txtA_BelowPercent.Text)
            sw.WriteLine(txtA_AbovePercent.Text)
            sw.WriteLine(txtA_TotalPercent.Text)

            'Pseudo Elevation lower and upper bounds
            sw.WriteLine("***  Pseudo Elevation lowerbnd and upperbnd ***")
            sw.WriteLine(txtNewLow.Text)
            sw.WriteLine(txtNewHigh.Text)

            'Pseudo Elevation non-represented area in acres
            sw.WriteLine("***  Pseudo Non-represented Below, Above, and Total in Acres ***")
            sw.WriteLine(txtP_BelowArea1.Text)
            sw.WriteLine(txtP_AboveArea1.Text)
            sw.WriteLine(txtP_TotalArea1.Text)

            'Pseudo Elevation non-represented area in Ha
            sw.WriteLine("***  Pseudo Non-represented Below, Above, and Total in Hectares ***")
            sw.WriteLine(txtP_BelowArea2.Text)
            sw.WriteLine(txtP_AboveArea2.Text)
            sw.WriteLine(txtP_TotalArea2.Text)

            'Pseudo Elevation non-represented area in %
            sw.WriteLine("***  Pseudo Non-represented Below, Above, and Total in Percentage***")
            sw.WriteLine(txtP_BelowPercent.Text)
            sw.WriteLine(txtP_AbovePercent.Text)
            sw.WriteLine(txtP_TotalPercent.Text)

            Return 1
        Catch ex As Exception
            Debug.Print("S_SaveResults Exception: " & ex.Message)
            Return -1
        Finally
            If sw IsNot Nothing Then
                sw.Close()
            End If
        End Try
    End Function

    Private Sub S_DisplayActualArea(ByVal IntervalList() As BA_IntervalList)
        Dim Area_NP_Below As Double
        Dim Area_NP_Above As Double
        Dim Area_P As Double
        Dim aoi_area As Double
        Dim ninterval As Integer
        Dim i As Integer

        'find areas not represented
        aoi_area = 0
        ninterval = UBound(IntervalList)

        If ninterval = 1 Then
            'acre
            txtA_BelowArea1.Text = 0
            txtA_AboveArea1.Text = 0
            txtA_TotalArea1.Text = 0

            'hectare
            txtA_BelowArea2.Text = 0
            txtA_AboveArea2.Text = 0
            txtA_TotalArea2.Text = 0

            'percentage
            txtA_BelowPercent.Text = 0
            txtA_AbovePercent.Text = 0
            txtA_TotalPercent.Text = 0
            Exit Sub
        End If

        For i = 1 To UBound(IntervalList)
            Select Case Val(IntervalList(i).Value)
                Case 1 'area below not represented
                    Area_NP_Below = Val(IntervalList(i).Area)
                Case 2 'represented area
                    Area_P = Val(IntervalList(i).Area)
                Case 3 'area above not represented
                    Area_NP_Above = Val(IntervalList(i).Area)
            End Select
            aoi_area = aoi_area + IntervalList(i).Area
        Next

        'input area unit is in square meters
        'display unit is in Acres and Hectare
        Dim ToAcre As Double
        Dim ToHectare As Double
        ToAcre = 0.0002471044
        ToHectare = 0.0001

        If aoi_area > 0 Then
            'acre
            txtA_BelowArea1.Text = Format(Area_NP_Below * ToAcre, "#0.00")
            txtA_AboveArea1.Text = Format(Area_NP_Above * ToAcre, "#0.00")
            txtA_TotalArea1.Text = Format((Area_NP_Below + Area_NP_Above) * ToAcre, "#0.00")

            'hectare
            txtA_BelowArea2.Text = Format(Area_NP_Below * ToHectare, "#0.00")
            txtA_AboveArea2.Text = Format(Area_NP_Above * ToHectare, "#0.00")
            txtA_TotalArea2.Text = Format((Area_NP_Below + Area_NP_Above) * ToHectare, "#0.00")

            'percentage
            txtA_BelowPercent.Text = Format(Area_NP_Below / aoi_area * 100, "#0.00")
            txtA_AbovePercent.Text = Format(Area_NP_Above / aoi_area * 100, "#0.00")
            txtA_TotalPercent.Text = Format((Area_NP_Below + Area_NP_Above) / aoi_area * 100, "#0.00")
        Else
            S_ResetResults()
        End If
    End Sub

    Private Sub S_DisplayPseudoArea(IntervalList() As BA_IntervalList)
        Dim Area_NP_Below As Double
        Dim Area_NP_Above As Double
        Dim Area_P As Double
        Dim aoi_area As Double
        Dim ninterval As Integer
        Dim i As Integer

        'find areas not represented
        aoi_area = 0
        ninterval = UBound(IntervalList)

        If ninterval = 1 Then
            'acre
            txtP_BelowArea1.Text = 0
            txtP_AboveArea1.Text = 0
            txtP_TotalArea1.Text = 0

            'hectare
            txtP_BelowArea2.Text = 0
            txtP_AboveArea2.Text = 0
            txtP_TotalArea2.Text = 0

            'percentage
            txtP_BelowPercent.Text = 0
            txtP_AbovePercent.Text = 0
            txtP_TotalPercent.Text = 0
            Exit Sub
        End If

        For i = 1 To UBound(IntervalList)
            Select Case Val(IntervalList(i).Value)
                Case 1 'area below not represented
                    Area_NP_Below = Val(IntervalList(i).Area)
                Case 2 'represented area
                    Area_P = Val(IntervalList(i).Area)
                Case 3 'area above not represented
                    Area_NP_Above = Val(IntervalList(i).Area)
            End Select
            aoi_area = aoi_area + IntervalList(i).Area
        Next

        'input area unit is in square meters
        'display unit is in Acres and Hectare
        Dim ToAcre As Double
        Dim ToHectare As Double
        ToAcre = 0.0002471044
        ToHectare = 0.0001

        If aoi_area > 0 Then
            'acre
            txtP_BelowArea1.Text = Format(Area_NP_Below * ToAcre, "#0.00")
            txtP_AboveArea1.Text = Format(Area_NP_Above * ToAcre, "#0.00")
            txtP_TotalArea1.Text = Format((Area_NP_Below + Area_NP_Above) * ToAcre, "#0.00")

            'hectare
            txtP_BelowArea2.Text = Format(Area_NP_Below * ToHectare, "#0.00")
            txtP_AboveArea2.Text = Format(Area_NP_Above * ToHectare, "#0.00")
            txtP_TotalArea2.Text = Format((Area_NP_Below + Area_NP_Above) * ToHectare, "#0.00")

            'percentage
            txtP_BelowPercent.Text = Format(Area_NP_Below / aoi_area * 100, "#0.00")
            txtP_AbovePercent.Text = Format(Area_NP_Above / aoi_area * 100, "#0.00")
            txtP_TotalPercent.Text = Format((Area_NP_Below + Area_NP_Above) / aoi_area * 100, "#0.00")
        Else
            S_ResetResults()
        End If

    End Sub

    Private Sub S_ResetResults()
        'acre
        txtA_BelowArea1.Text = 0
        txtA_AboveArea1.Text = 0
        txtA_TotalArea1.Text = 0

        'hectare
        txtA_BelowArea2.Text = 0
        txtA_AboveArea2.Text = 0
        txtA_TotalArea2.Text = 0

        'percentage
        txtA_BelowPercent.Text = 0
        txtA_AbovePercent.Text = 0
        txtA_TotalPercent.Text = 0

        'acre
        txtP_BelowArea1.Text = 0
        txtP_AboveArea1.Text = 0
        txtP_TotalArea1.Text = 0

        'hectare
        txtP_BelowArea2.Text = 0
        txtP_AboveArea2.Text = 0
        txtP_TotalArea2.Text = 0

        'percentage
        txtP_BelowPercent.Text = 0
        txtP_AbovePercent.Text = 0
        txtP_TotalPercent.Text = 0
    End Sub

    Private Sub ShowOutput()
        Dim filepath As String
        Dim FileName As String
        Dim filepathname As String
        Dim response As Integer

        'add non-represented zone maps
        filepath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True)
        FileName = BA_EnumDescription(MapsFileName.ActualRepresentedArea)
        filepathname = filepath & FileName
        response = BA_DisplayRasterWithSymbol(My.Document, filepathname, BA_MAP_ACTUAL_REPRESENTATION, _
                                              MapsDisplayStyle.Elevation, 30, WorkspaceType.Geodatabase)
        If response < 0 Then
            MsgBox("Cannot display " & FileName & "!")
            Scenario1Map_Flag = False
        Else
            If Maps_Are_Generated Then Scenario1Map_Flag = True
        End If

        FileName = BA_EnumDescription(MapsFileName.PseudoRepresentedArea)
        filepathname = filepath & FileName
        response = BA_DisplayRasterWithSymbol(My.Document, filepathname, BA_MAP_PSEUDO_REPRESENTATION, _
                                      MapsDisplayStyle.Elevation, 30, WorkspaceType.Geodatabase)
        If response < 0 Then
            MsgBox("Cannot display " & FileName & "!")
            Scenario2Map_Flag = False
        Else
            If Maps_Are_Generated Then Scenario2Map_Flag = True
        End If

        If Not Maps_Are_Generated Then
            MsgBox("You need to generate basin analysis maps first before you can use the menu items to show the representation maps!")
        Else
            Dim Basin_Name As String
            Dim cboSelectedBasin = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of cboTargetedBasin)(My.ThisAddIn.IDs.cboTargetedBasin)
            Dim cboSelectedAoi = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of cboTargetedAOI)(My.ThisAddIn.IDs.cboTargetedAOI)
            If Len(Trim(cboSelectedBasin.getValue)) = 0 Then
                Basin_Name = ""
            Else
                Basin_Name = " at " & cboSelectedBasin.getValue
            End If
            If Scenario1Map_Flag Then
                BA_DisplayMap(My.Document, 7, Basin_Name, cboSelectedAoi.getValue, Map_Display_Elevation_in_Meters, _
                              "SCENARIO 1 SITE REPRESENTATION")
            Else
                BA_DisplayMap(My.Document, 8, Basin_Name, cboSelectedAoi.getValue, Map_Display_Elevation_in_Meters, _
                              "SCENARIO 2 SITE REPRESENTATION")
            End If
        End If
    End Sub

    Private Sub Set_LabelUnit(Zunit As String)
        Dim zunitstring As String
        Select Case UCase(Zunit)
            Case "M", "METER", "METERS"
                zunitstring = "Meters"
                OptZMeters.Checked = True

            Case "F", "FEET", "FOOT"
                zunitstring = "Feet"
                OptZFeet.Checked = True

            Case Else
                zunitstring = "Unknown"

        End Select

        txtZUnit1.Text = zunitstring
        txtZUnit2.Text = zunitstring
        txtZUnit3.Text = zunitstring
        txtZUnit4.Text = zunitstring
        txtZUnit5.Text = zunitstring
    End Sub

    Private Sub OptZMeters_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles OptZMeters.CheckedChanged
        If OptZMeters.Checked = True Then
            Set_LabelUnit("Meters")

            If m_demInMeters Then 'meters
                DEMConversion_Factor = 1
            Else
                DEMConversion_Factor = 0.3048 'feet to meters
            End If

            txtMinElev.Text = Math.Round(ES_DEMMin * DEMConversion_Factor - 0.005, 2)
            txtMaxElev.Text = Math.Round(ES_DEMMax * DEMConversion_Factor + 0.005, 2)

            If Val(txtSNOTELMin.Text) > 0 Then 'i.e., data values present
                txtSNOTELMin.Text = Math.Round(ES_STMin * DEMConversion_Factor, 2)
                txtSNOTELMax.Text = Math.Round(ES_STMax * DEMConversion_Factor, 2)
            End If

            If Val(txtSCourseMin.Text) > 0 Then
                txtSCourseMin.Text = Math.Round(ES_SCMin * DEMConversion_Factor, 2)
                txtSCourseMax.Text = Math.Round(ES_SCMax * DEMConversion_Factor, 2)
            End If

            txtActualLow.Text = Math.Round(Val(txtActualLow.Text) * 0.3048, 2)
            txtActualHigh.Text = Math.Round(Val(txtActualHigh.Text) * 0.3048, 2)
            txtNewLow.Text = Math.Round(Val(txtNewLow.Text) * 0.3048, 2)
            txtNewHigh.Text = Math.Round(Val(txtNewHigh.Text) * 0.3048, 2)
            txtLowerBuffer.Text = Math.Round(Val(txtLowerBuffer.Text) * 0.3048, 2)
        End If
    End Sub

    Private Sub OptZFeet_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles OptZFeet.CheckedChanged
        If OptZFeet.Checked = True Then
            Set_LabelUnit("feet")

            If m_demInMeters Then 'meters
                DEMConversion_Factor = 3.2808399 'meters to feet
            Else
                DEMConversion_Factor = 1
            End If

            txtMinElev.Text = Math.Round(ES_DEMMin * DEMConversion_Factor - 0.005, 2)
            txtMaxElev.Text = Math.Round(ES_DEMMax * DEMConversion_Factor + 0.005, 2)

            If Val(txtSNOTELMin.Text) > 0 Then 'i.e., data values present
                txtSNOTELMin.Text = Math.Round(ES_STMin * DEMConversion_Factor, 2)
                txtSNOTELMax.Text = Math.Round(ES_STMax * DEMConversion_Factor, 2)
            End If

            If Val(txtSCourseMin.Text) > 0 Then
                txtSCourseMin.Text = Math.Round(ES_SCMin * DEMConversion_Factor, 2)
                txtSCourseMax.Text = Math.Round(ES_SCMax * DEMConversion_Factor, 2)
            End If

            txtActualLow.Text = Math.Round(Val(txtActualLow.Text) * 3.2808399, 2)
            txtActualHigh.Text = Math.Round(Val(txtActualHigh.Text) * 3.2808399, 2)
            txtNewLow.Text = Math.Round(Val(txtNewLow.Text) * 3.2808399, 2)
            txtNewHigh.Text = Math.Round(Val(txtNewHigh.Text) * 3.2808399, 2)
            txtLowerBuffer.Text = Math.Round(Val(txtLowerBuffer.Text) * 3.2808399, 2)
        End If
    End Sub

    Private Sub CmdCalculate_Click(sender As System.Object, e As System.EventArgs) Handles CmdCalculate.Click
        Dim dAOI_Area As Double
        Dim arrIntervalList() As BA_IntervalList = Nothing
        Dim dMinElev As Double, dMaxElev As Double
        Dim dMinActual As Double, dMaxActual As Double
        Dim dMinPseudo As Double, dMaxPseudo As Double
        Dim dLowerBuffer As Double
        Dim bInvalidInput As Boolean
        Dim sFilePath As String

        'all data must be in the same Z unit as the DEM
        'get the elevations from the bufferred DEM
        Dim raster_res As Double
        Dim filledDemPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces, True) & BA_EnumDescription(MapsFileName.filled_dem_gdb)
        Dim pRasterStats As IRasterStatistics = BA_GetRasterStatsGDB(filledDemPath, raster_res)
        dMinElev = pRasterStats.Minimum
        dMaxElev = pRasterStats.Maximum
        pRasterStats = Nothing

        dMinActual = Val(txtActualLow.Text) / DEMConversion_Factor
        dMaxActual = Val(txtActualHigh.Text) / DEMConversion_Factor
        dMinPseudo = Val(txtNewLow.Text) / DEMConversion_Factor
        dMaxPseudo = Val(txtNewHigh.Text) / DEMConversion_Factor
        dLowerBuffer = Val(txtLowerBuffer.Text) / DEMConversion_Factor

        'verify inputs
        bInvalidInput = False
        If dMinElev >= dMaxElev Or dMinActual >= dMaxActual Or dMinPseudo >= dMaxPseudo Then bInvalidInput = True
        If dMinActual < dMinElev Or dMaxActual > dMaxElev Then bInvalidInput = True
        If dMinPseudo < dMinElev Or dMaxPseudo > dMaxElev Then bInvalidInput = True

        If bInvalidInput Then
            MsgBox("Inconsistent input values. Please check the input.")
            Exit Sub
        End If

        sFilePath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis)

        'calculate actual scenario
        dAOI_Area = BA_Map_CalculateNonRepresented(My.Document, AOIFolderBase, dMinElev, dMaxElev, dLowerBuffer, _
                                                   dMinActual, dMaxActual, arrIntervalList, sFilePath, BA_ActualNP)
        If dAOI_Area < 0 Then
            MsgBox("An error occurred when calculating actual non-represented area!")
            Exit Sub
        End If

        S_DisplayActualArea(arrIntervalList)

        'calculate pseduo scenario
        dAOI_Area = BA_Map_CalculateNonRepresented(My.Document, AOIFolderBase, dMinElev, dMaxElev, dLowerBuffer, _
                                                   dMinPseudo, dMaxPseudo, arrIntervalList, sFilePath, BA_PseudoNP)

        If dAOI_Area < 0 Then
            MsgBox("An error occurred when calculating pesudo non-represented area!")
            Exit Sub
        End If

        S_DisplayPseudoArea(arrIntervalList)
        S_SaveResults()

        ElevDistMap_Flag = False
        ElevSNOTELMap_Flag = False
        ElevSnowCourseMap_Flag = False
        PrecipDistMap_Flag = False
        SlopeDistMap_Flag = False
        AspectDistMap_Flag = False
        Scenario1Map_Flag = False
        Scenario2Map_Flag = False
        Maps_Are_Generated = False

        ShowOutput()
        MsgBox("Calculation is completed! Please use the map menu to view results.")
    End Sub

    Private Sub frmElevScenario_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class