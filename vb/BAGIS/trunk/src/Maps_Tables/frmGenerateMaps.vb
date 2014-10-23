Imports BAGIS_ClassLibrary
Imports System.IO
Imports System.Windows.Forms
Imports System.Text
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.SpatialAnalyst
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Desktop.AddIns
Imports Microsoft.Office.Interop.Excel

Public Class frmGenerateMaps

    'silence the message
    Private Set_Silent_Mode As Boolean = True
    'flag to control the execution of analysis
    Private Flag_ElevationZone As Boolean
    Private Flag_PrecipitationZone As Boolean
    Private Flag_BasinTables As Boolean
    Private Flag_BasinMaps As Boolean
    Private Flag_ElevOrPrecipChange As Boolean
    'variables
    Private Elev_Interval As Integer
    Private Elev_Subdivision As Integer

    Private AnalysisPath As String
    Private PRISMPath As String
    Private ElvPath As String
    Private PrecipPath As String
    Private PRISMRasterName As String
    Private m_demInMeters As Boolean
    Private m_formInit As Boolean = False

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Dim filepath As String, FileName As String
        Dim response As Integer

        If Len(Trim(AOIFolderBase)) = 0 Then
            MsgBox("AOI hasn't been specified! Please use the AOI Tools to set target AOI.")
            Me.Close()
        End If

        'Initialize the controls on the form
        InitForm()

        'read dem min, max everytime the form is activated
        'display dem elevation stats
        Dim pRasterStats As IRasterStatistics = BA_GetDemStatsGDB(AOIFolderBase)

        Dim DataConversion_Factor As Double
        Dim DisplayConversion_Factor As Double

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
        DataConversion_Factor = BA_SetConversionFactor(True, m_demInMeters)
        AOI_DEMMin = Math.Round(pRasterStats.Minimum * DataConversion_Factor - 0.005, 2)
        AOI_DEMMax = Math.Round(pRasterStats.Maximum * DataConversion_Factor + 0.005, 2)

        'Get AOI area
        Dim aoiGdbPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi, True)
        Dim AOIArea As Double = BA_GetShapeAreaGDB(aoiGdbPath & BA_AOIExtentCoverage) / 1000000 'the shape unit is in sq meters, converted to sq km
        txtArea.Text = Format(AOIArea, "#0.00")
        txtAreaAcre.Text = Format(AOIArea * 247.1044, "#0.00")
        txtAreaSQMile.Text = Format(AOIArea * 0.3861022, "#0.00")

        'check to see if maps folder exists
        filepath = BA_GetPath(AOIFolderBase, PublicPath.Maps)
        If Not BA_Folder_ExistsWindowsIO(filepath) Then
            Dim newFolder As String = BA_CreateFolder(AOIFolderBase, BA_GetBareName(BA_EnumDescription(PublicPath.Maps)))
            If String.IsNullOrEmpty(newFolder) Then
                MessageBox.Show("Could not create maps folder in AOI. The Generate Maps screen is unavailable.", _
                                "Failed to create folder", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If

        'check if map_parameters.txt file exists
        Set_Silent_Mode = True
        FileName = BA_MapParameterFile 'i.e., map_parameters.txt
        response = ReadMapParameters(filepath, FileName)

        Dim rvalues_arr() As BA_IntervalList = Nothing
        Dim subrvalues_arr() As BA_IntervalList = Nothing

        Dim ninterval As Integer
        Dim nsubinterval As Integer
        If response < 1 Then 'cannot read the parameter file correctly
            If m_demInMeters = True Then
                OptZMeters.Checked = True
                lblElevUnit.Text = "Elevation (m):"
            Else
                OptZFeet.Checked = True
                lblElevUnit.Text = "Elevation (ft):"
            End If

            'Populate Boxes
            DisplayConversion_Factor = BA_SetConversionFactor(OptZMeters.Checked, True)

            txtMinElev.Text = Math.Round(AOI_DEMMin * DisplayConversion_Factor - 0.005, 2)  'adjust value to include the actual min, max
            txtMaxElev.Text = Math.Round(AOI_DEMMax * DisplayConversion_Factor + 0.005, 2)
            txtRangeElev.Text = Val(txtMaxElev.Text) - Val(txtMinElev.Text)

            Elev_Interval = Val(CmboxElevInterval.SelectedItem)
            Elev_Subdivision = Val(ComboxSubDivide.SelectedItem)

            ninterval = BA_CreateRangeArray(Val(txtMinElev.Text), Val(txtMaxElev.Text), Elev_Interval, rvalues_arr)
            txtElevClassNumber.Text = ninterval
            Display_IntervalList(rvalues_arr)
            nsubinterval = Subdivide_IntervalList(rvalues_arr, Elev_Interval, _
                subrvalues_arr, Elev_Subdivision)
            Display_ElevationRange(rvalues_arr)
        Else
            'Populate elevation Boxes
            DisplayConversion_Factor = BA_SetConversionFactor(OptZMeters.Checked, True)
            txtMinElev.Text = Math.Round(AOI_DEMMin * DisplayConversion_Factor - 0.005, 2)  'adjust value to include the actual min, max
            txtMaxElev.Text = Math.Round(AOI_DEMMax * DisplayConversion_Factor + 0.005, 2)
            txtRangeElev.Text = Val(txtMaxElev.Text) - Val(txtMinElev.Text)
            Elev_Interval = Val(CmboxElevInterval.SelectedItem)
            Elev_Subdivision = Val(ComboxSubDivide.SelectedItem)

            ninterval = BA_CreateRangeArray(Val(txtMinElev.Text), Val(txtMaxElev.Text), Elev_Interval, rvalues_arr)
            'txtElevClassNumber.Text = ninterval
            nsubinterval = Subdivide_IntervalList(rvalues_arr, Elev_Interval, _
                subrvalues_arr, Elev_Subdivision)
            Display_ElevationRange(rvalues_arr)
        End If

        If AOI_DEMMax > 30000 Then 'elevation range value error
            MsgBox("DEM elevation value out of normal bound! Please check the DEM data.")
            Me.Close()
        End If

        AnalysisPath = BA_GetPath(AOIFolderBase, PublicPath.Analysis)
        PRISMPath = AnalysisPath & "\" & BA_RasterPrecipitationZones
        ElvPath = AnalysisPath & "\" & BA_RasterElevationZones

        Display_DataStatus()
        m_formInit = True
        Set_Silent_Mode = False
    End Sub

    Private Sub CmbClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmbClose.Click
        Me.Close()
    End Sub

    Private Function ReadMapParameters(ByVal filepath As String, ByVal FileName As String) As Integer

        Dim filepathname As String
        Dim linestring As String
        Dim errormessage As String
        Dim listcount As Integer
        Dim listitem(0 To 3) As String
        Dim i As Integer, j As Integer
        Dim position As Integer

        If Len(Trim(filepath)) * Len(Trim(FileName)) = 0 Then Return -1

        filepathname = filepath & "\" & FileName

        Dim sr As StreamReader = Nothing


        Try
            'open file for input
            If BA_File_ExistsWindowsIO(filepathname) Then
                sr = File.OpenText(filepathname)
            Else
                'MsgBox("File " & filepathname & " does not exist and cannot be opened.")
                Return -1
            End If

            'read the version text
            linestring = sr.ReadLine

            'check version
            If Trim(linestring) <> BA_VersionText And Trim(linestring) <> BA_CompatibleVersion1Text Then
                sr.Close()
                errormessage = "The map parameter file's version doesn't match the version of the model!" & vbCrLf & "Please delete or rename the file and restart the model" & vbCrLf
                errormessage = errormessage & FileName
                MsgBox(errormessage)
                Return -1
            End If

            'read the map unit text
            linestring = sr.ReadLine
            If Trim(linestring) = "True" Then
                OptZMeters.Checked = True
                Map_Display_Elevation_in_Meters = True
            Else
                OptZFeet.Checked = True
                Map_Display_Elevation_in_Meters = False
            End If

            'prepare the elevation interval list
            linestring = sr.ReadLine
            CmboxElevInterval.SelectedIndex = Val(Trim(linestring))
            linestring = sr.ReadLine
            listcount = Val(Trim(linestring))
            txtElevClassNumber.Text = listcount
            lstintervals.Items.Clear()

            If listcount > 0 Then
                For i = 0 To listcount - 1
                    linestring = sr.ReadLine  'comma delimited
                    j = 0
                    Do While Len(linestring) > 0
                        position = InStr(1, linestring, ",", vbTextCompare)
                        If position > 0 Then
                            listitem(j) = Microsoft.VisualBasic.Left(linestring, position - 1)
                            linestring = Microsoft.VisualBasic.Right(linestring, Len(linestring) - position)
                        Else
                            listitem(j) = linestring
                            linestring = ""
                        End If
                        j = j + 1
                    Loop

                    With lstintervals
                        Dim pitem As New ListViewItem(listitem(0))
                        pitem.SubItems.Add(listitem(1))
                        pitem.SubItems.Add(listitem(2))
                        pitem.SubItems.Add(listitem(3))
                        .Items.Add(pitem)
                    End With
                Next
                'disable the apply button if the zone raster has been created
                cmdApplyElevInterval.Enabled = False
            Else
                'enable the apply button
                cmdApplyElevInterval.Enabled = True
            End If

            'prepare the PRISM list
            linestring = sr.ReadLine
            CmboxPrecipType.SelectedIndex = Val(linestring)
            linestring = sr.ReadLine
            CmboxBegin.SelectedIndex = Val(linestring)
            linestring = sr.ReadLine
            CmboxEnd.SelectedIndex = Val(linestring)
            linestring = sr.ReadLine
            txtMinPrecip.Text = Val(linestring)
            linestring = sr.ReadLine
            txtMaxPrecip.Text = Val(linestring)
            linestring = sr.ReadLine
            txtRangePrecip.Text = Val(linestring)

            linestring = sr.ReadLine
            If Val(linestring) <= 0 Then
                txtPrecipMapZoneInt.Text = 1
            Else
                txtPrecipMapZoneInt.Text = Val(linestring)
            End If

            linestring = sr.ReadLine
            txtPrecipMapZoneNo.Text = Val(linestring)

            'lstPrecipZones list box
            linestring = sr.ReadLine
            listcount = Val(Trim(linestring))
            lstPrecipZones.Items.Clear()
            If listcount > 0 Then
                For i = 1 To listcount
                    linestring = sr.ReadLine
                    lstPrecipZones.Items.Add(Trim(linestring))
                Next
                'disable the apply button if the zone raster has been created
                cmdPRISM.Enabled = False
                cmdApplyPRISMInterval.Enabled = False
            Else
                'enable the apply button
                cmdPRISM.Enabled = True
                cmdApplyPRISMInterval.Enabled = False
            End If

            linestring = sr.ReadLine 'number of subdivision
            ComboxSubDivide.SelectedIndex = Val(linestring) - 1     'Backwards compatibility; First index was one in VBA

            linestring = sr.ReadLine 'whether subrange analysis
            chkUseRange.Checked = linestring

            linestring = sr.ReadLine 'from elevation
            txtFromElev.Text = linestring

            linestring = sr.ReadLine 'to elevation
            txtToElev.Text = linestring

            If sr.Peek > -1 Then 'check if additional parameters were added after BAGIS Ver 1. Aspect was added in version 2
                linestring = sr.ReadLine 'skip the REVISION text
                linestring = sr.ReadLine 'aspect
                Dim tokenstring() As String = linestring.Split(New Char(), " "c)
                If tokenstring(0).ToUpper = "ASPECT" Then
                    Select Case tokenstring(1)
                        Case "4"
                            CmboxAspect.SelectedIndex = 0
                        Case "8"
                            CmboxAspect.SelectedIndex = 1
                        Case Else
                            CmboxAspect.SelectedIndex = 2
                    End Select
                Else
                    CmboxAspect.SelectedIndex = 2 'default value is set to 16 aspect classes
                End If
            Else
                CmboxAspect.SelectedIndex = 2 'default value is set to 16 aspect classes
            End If

            If sr.Peek > -1 Then 'check to see if we were partway through an analysis and need to set generate to true
                linestring = sr.ReadLine
                Dim tokenstring() As String = linestring.Split(New Char(), " "c)
                If tokenstring(0).ToUpper = "ENABLE_GENERATE" Then
                    If tokenstring(1) = "True" Then
                        Flag_BasinMaps = False
                        Flag_BasinTables = False
                        Flag_ElevOrPrecipChange = True
                    Else
                        Flag_BasinMaps = True
                        Flag_BasinTables = True
                        Flag_ElevOrPrecipChange = False
                    End If
                End If
            End If

            'Flag_PrecipitationZone = True
            'Flag_ElevationZone = True
            CmdGenerate.Enabled = False
            Return 1
        Catch ex As Exception
            Debug.Print("ReadMapParameters Exception: " & ex.Message)
            Return -1
        Finally
            'Don't forget to close the file handle
            If sr IsNot Nothing Then sr.Close()
        End Try
    End Function

    Private Function SaveMapParameters(filepath As String, FileName As String) As Integer
        Dim filepathname As String
        Dim i As Integer
        Dim tempstring As String
        Dim listcount As Integer

        If Len(Trim(filepath)) * Len(Trim(FileName)) = 0 Then Return -1

        filepathname = filepath & "\" & FileName
        Dim sw As StreamWriter = Nothing

        Try
            'open file for output
            sw = New StreamWriter(filepathname)
            sw.WriteLine(BA_VersionText)
            'map unit
            sw.WriteLine(OptZMeters.Checked)
            'elevation dist frame
            sw.WriteLine(CmboxElevInterval.SelectedIndex)
            listcount = lstintervals.Items.Count
            sw.WriteLine(listcount)

            'lstintervals list box
            If listcount > 0 Then
                For i = 0 To listcount - 1
                    With lstintervals
                        Dim pItem As ListViewItem = .Items(i)
                        tempstring = pItem.Text & "," & pItem.SubItems(1).Text & "," & pItem.SubItems(2).Text & "," & pItem.SubItems(3).Text
                        sw.WriteLine(tempstring)  'comma delimited
                    End With
                Next
            End If

            'precipitation dist frame
            sw.WriteLine(CmboxPrecipType.SelectedIndex)
            sw.WriteLine(CmboxBegin.SelectedIndex)
            sw.WriteLine(CmboxEnd.SelectedIndex)
            sw.WriteLine(txtMinPrecip.Text)
            sw.WriteLine(txtMaxPrecip.Text)
            sw.WriteLine(txtRangePrecip.Text)
            sw.WriteLine(txtPrecipMapZoneInt.Text)
            sw.WriteLine(txtPrecipMapZoneNo.Text)

            listcount = lstPrecipZones.Items.Count
            sw.WriteLine(listcount)

            'lstPrecipZones list box
            If listcount > 0 Then
                For i = 0 To listcount - 1
                    sw.WriteLine(lstPrecipZones.Items(i))
                Next
            End If

            sw.WriteLine(ComboxSubDivide.SelectedIndex + 1) 'number of subdivision 1 to 5; Backwards compatibility
            sw.WriteLine(chkUseRange.Checked) 'whether user enable the subrange analysis
            sw.WriteLine(txtFromElev.Text)  'from elevation
            sw.WriteLine(txtToElev.Text) 'to elevation

            'BAGIS V2 new parameter for aspect class
            sw.WriteLine("REVISION for BAGIS 2")
            sw.WriteLine("ASPECT " & 2 ^ (CmboxAspect.SelectedIndex + 2))
            sw.WriteLine("ENABLE_GENERATE " & Flag_ElevOrPrecipChange)

            sw.Flush()
            Return 1
        Catch ex As Exception
            Debug.Print("SaveMapParameters Exception: " & ex.Message)
            Return -1
        Finally
            If sw IsNot Nothing Then
                sw.Close()
            End If
        End Try
    End Function

    'This subroutine updates only the Elevation zone information in the map parameter file
    'return value:
    ' 0: fail
    ' 1: success
    Public Function UpdateMapParameters(ByVal filepath As String, ByVal FileName As String) As Integer
        Dim filepathname As String
        Dim linearray() As String
        Dim i As Integer
        Dim linestring As String, tempstring As String
        Dim listcount As Integer
        Dim nlines As Long, oldVersonLines As Long

        If Len(Trim(filepath)) * Len(Trim(FileName)) = 0 Then Return -1
        filepathname = filepath & "\" & FileName
        Dim sr As StreamReader = Nothing
        Dim sw As StreamWriter = Nothing

        Try
            'read parameters into memory
            'open file for input
            sr = New StreamReader(filepathname)
            linestring = sr.ReadLine 'read the version text

            'check version
            If Trim(linestring) <> BA_VersionText And Trim(linestring) <> BA_CompatibleVersion1Text Then
                sr.Close()
                'Delete the text file: map_parameters.txt
                BA_Remove_File(filepathname)
                Return 0
            End If

            nlines = 1
            oldVersonLines = 1
            Dim endofOldVersion As Boolean = False
            Do While linestring IsNot Nothing
                linestring = sr.ReadLine
                If linestring.Length > 8 Then
                    If linestring.Substring(0, 8) = "REVISION" Then
                        endofOldVersion = True
                    End If
                End If
                nlines = nlines + 1 'count the number of lines in the file
                If Not endofOldVersion Then oldVersonLines = oldVersonLines + 1
            Loop

            sr.Close() 'close the file and reopen to read
            sr = New StreamReader(filepathname)

            ReDim linearray(0 To nlines)
            For i = 0 To nlines - 1
                linestring = sr.ReadLine
                linearray(i) = linestring 'read the whole parameter file
            Next

            sr.Close() 'close the file and reopen for output

            'open file for output
            sw = New StreamWriter(filepathname)

            sw.WriteLine(linearray(0))
            sw.WriteLine(OptZMeters.Checked)

            'elevation dist frame
            Dim OldListCount As Long = Val(linearray(3))
            listcount = lstintervals.Items.Count
            sw.WriteLine(CmboxElevInterval.SelectedIndex)
            sw.WriteLine(listcount)

            'lstintervals list box
            If listcount > 0 Then
                For i = 0 To listcount - 1
                    With lstintervals
                        Dim pItem As ListViewItem = .Items(i)
                        tempstring = pItem.Text & "," & pItem.SubItems(1).Text & "," & pItem.SubItems(2).Text & "," & pItem.SubItems(3).Text
                        sw.WriteLine(tempstring)  'comma delimited
                    End With
                Next
            End If

            For i = OldListCount + 4 To oldVersonLines - 6
                sw.WriteLine(linearray(i))
            Next

            sw.WriteLine(ComboxSubDivide.SelectedIndex + 1) 'number of subdivision 1 to 5

            If chkUseRange.Checked = True Then
                'last 3 lines are for user-specified range analysis
                sw.WriteLine(chkUseRange.Checked) 'whether user enable the subrange analysis
                sw.WriteLine(txtFromElev.Text) 'from elevation
                sw.WriteLine(txtToElev.Text) 'to elevation
            Else
                sw.WriteLine(linearray(oldVersonLines - 4))
                sw.WriteLine(linearray(oldVersonLines - 3))
                sw.WriteLine(linearray(oldVersonLines - 2))
            End If

            If oldVersonLines < nlines Then
                For i = oldVersonLines To nlines
                    sw.WriteLine(linearray(i - 1))
                Next
            End If

            Return 1
        Catch ex As Exception
            Debug.Print("UpdateMapParameters Exception " & ex.Message)
            Return 0
        Finally
            'Close the file(s)
            If sw IsNot Nothing Then
                sw.Close()
            End If
            If sr IsNot Nothing Then
                sr.Close()
            End If
        End Try
    End Function

    'This subroutine updates only the Precipitation zone information in the map parameter file
    'return value:
    ' 0: fail
    ' 1: success
    Private Function UpdateMapParametersPRISM(ByVal filepath As String, ByVal FileName As String) As Integer
        Dim filepathname As String
        Dim linearray() As String
        Dim i As Integer
        Dim linestring As String
        Dim listcount As Integer
        Dim nlines As Long, oldVersonLines As Long

        If Len(Trim(filepath)) * Len(Trim(FileName)) = 0 Then Return 0
        Dim sr As StreamReader = Nothing
        Dim sw As StreamWriter = Nothing

        filepathname = filepath & "\" & FileName
        Try
            'read parameters into memory
            'open file for input
            sr = New StreamReader(filepathname)
            linestring = sr.ReadLine 'read the version text

            'check version
            If Trim(linestring) <> BA_VersionText And Trim(linestring) <> BA_CompatibleVersion1Text Then
                sr.Close()
                'Delete the text file: map_parameters.txt
                BA_Remove_File(filepathname)
                Return 0
            End If

            nlines = 1
            oldVersonLines = 1
            Dim endofOldVersion As Boolean = False
            Do While linestring IsNot Nothing
                linestring = sr.ReadLine
                If linestring.Length > 8 Then
                    If linestring.Substring(0, 8) = "REVISION" Then
                        endofOldVersion = True
                    End If
                End If
                nlines = nlines + 1 'count the number of lines in the file
                If Not endofOldVersion Then oldVersonLines = oldVersonLines + 1
            Loop
            sr.Close() 'close the file and reopen to read

            ReDim linearray(0 To nlines)

            'Open filepathname For Input
            sr = New StreamReader(filepathname)

            For i = 0 To nlines - 1
                linestring = sr.ReadLine
                linearray(i) = linestring 'read the whole parameter file
            Next
            sr.Close() 'close the file and reopen for output

            'open file for output
            sw = New StreamWriter(filepathname)
            'reproduce the first three lines
            For i = 0 To 2
                sw.WriteLine(linearray(i))
            Next

            'reproduce elevation dist frame
            listcount = Val(linearray(3))
            For i = 0 To listcount
                sw.WriteLine(linearray(i + 3))
            Next

            'update the precipitation settings and frame
            sw.WriteLine(CmboxPrecipType.SelectedIndex)
            sw.WriteLine(CmboxBegin.SelectedIndex)
            sw.WriteLine(CmboxEnd.SelectedIndex)
            sw.WriteLine(txtMinPrecip.Text)
            sw.WriteLine(txtMaxPrecip.Text)
            sw.WriteLine(txtRangePrecip.Text)
            sw.WriteLine(txtPrecipMapZoneInt.Text)
            sw.WriteLine(txtPrecipMapZoneNo.Text)

            listcount = lstPrecipZones.Items.Count
            sw.WriteLine(listcount)

            'lstPrecipZones list box
            If listcount > 0 Then
                For i = 0 To listcount - 1
                    sw.WriteLine(lstPrecipZones.Items(i))
                Next
            End If

            'last 4 lines are for user-specified range analysis
            sw.WriteLine(linearray(oldVersonLines - 5)) 'subdivision number
            sw.WriteLine(linearray(oldVersonLines - 4)) 'whether range analysis activated
            sw.WriteLine(linearray(oldVersonLines - 3)) 'from elev
            sw.WriteLine(linearray(oldVersonLines - 2)) 'to elev

            If oldVersonLines < nlines Then
                For i = oldVersonLines To nlines
                    sw.WriteLine(linearray(i - 1))
                Next
            End If

            sw.Close()    'Close the file
            Return 1
        Catch ex As Exception
            Debug.Print("UpdateMapParametersPRISM Exception: " & ex.Message)
            Return 0
        Finally
            If sr IsNot Nothing Then
                sr.Close()
            End If
            If sw IsNot Nothing Then
                sw.Close()
            End If
        End Try
    End Function

    Private Function Subdivide_IntervalList(ByVal In_List() As BA_IntervalList, ByVal In_Interval As Integer, _
                                            ByRef Out_List() As BA_IntervalList, ByVal SubdivideNo As Integer) As Long
        Dim new_interval As Double
        Dim ninterval As Long
        Dim minval As Double
        Dim maxval As Double
        Dim i As Long

        ninterval = UBound(In_List)

        If SubdivideNo = 1 Then  'duplicate the interval list
            ReDim Out_List(0 To ninterval)

            For i = 1 To ninterval
                Out_List(i).Value = In_List(i).Value
                Out_List(i).LowerBound = In_List(i).LowerBound
                Out_List(i).UpperBound = In_List(i).UpperBound
                Out_List(i).Name = In_List(i).Name
            Next

            Subdivide_IntervalList = ninterval
            Exit Function
        End If

        new_interval = Math.Round(CDbl(In_Interval / SubdivideNo), 4)
        minval = In_List(1).LowerBound
        maxval = In_List(ninterval).UpperBound

        Subdivide_IntervalList = BA_CreateRangeArray(minval, maxval, new_interval, Out_List)

        'round the out_list values
        For i = 1 To Subdivide_IntervalList
            Out_List(i).LowerBound = Math.Round(Out_List(i).LowerBound)
            Out_List(i).UpperBound = Math.Round(Out_List(i).UpperBound)
        Next

        Out_List(1).LowerBound = minval
        Out_List(Subdivide_IntervalList).UpperBound = maxval
    End Function

    Private Sub Display_IntervalList(ByVal Interval_List() As BA_IntervalList)
        Dim i As Long
        Dim ninterval As Long = UBound(Interval_List)

        lstintervals.Items.Clear()

        Try
            Dim rangestring As String
            Dim pcntstring As String
            If ninterval = 0 Then
                Exit Sub    'no class interval
            Else
                For i = 0 To ninterval - 1
                    rangestring = Interval_List(i + 1).Name

                    'align the percentage number on the decimal point
                    pcntstring = Format(Interval_List(i + 1).Area, "000.00")
                    If Microsoft.VisualBasic.Left(pcntstring, 1) = "0" Then pcntstring = " " & Microsoft.VisualBasic.Right(pcntstring, Len(pcntstring) - 1)
                    If Mid(pcntstring, 2, 1) = "0" And Microsoft.VisualBasic.Left(pcntstring, 1) <> "1" Then pcntstring = "  " & Microsoft.VisualBasic.Right(pcntstring, Len(pcntstring) - 2)

                    With lstintervals
                        Dim pitem As New ListViewItem(rangestring)
                        If Not String.IsNullOrEmpty(pcntstring) Then
                            pitem.SubItems.Add(pcntstring & " %")
                        Else
                            pitem.SubItems.Add("?")
                        End If
                        If Not String.IsNullOrEmpty(Interval_List(i + 1).SNOTEL) Then
                            pitem.SubItems.Add(Interval_List(i + 1).SNOTEL)
                        Else
                            pitem.SubItems.Add("?")
                        End If
                        If Not String.IsNullOrEmpty(Interval_List(i + 1).SnowCourse) Then
                            pitem.SubItems.Add(Interval_List(i + 1).SnowCourse)
                        Else
                            pitem.SubItems.Add("?")
                        End If
                        .Items.Add(pitem)
                    End With
                Next
            End If

        Catch ex As Exception
            Debug.Print("Display_IntervalList Exception: " & ex.Message)
        End Try
    End Sub

    'BA_IntervalList array is 1-based
    Private Sub Display_PreciptIntervalList(ByVal Interval_List() As BA_IntervalList)
        Dim i As Long
        Dim ninterval As Long = UBound(Interval_List)

        lstPrecipZones.Items.Clear()

        Dim rangestring As String

        If ninterval = 0 Then
            Exit Sub    'no class interval
        Else
            For i = 0 To ninterval - 1
                rangestring = Interval_List(i + 1).Name
                lstPrecipZones.Items.Add(rangestring)
            Next
        End If
    End Sub

    Private Sub Display_ElevationRange(ByVal Interval_List() As BA_IntervalList)
        Dim i As Long
        Dim ninterval As Long

        ninterval = UBound(Interval_List)
        If ninterval <= 1 Then
            Exit Sub    'no class interval
        Else
            lstElevRange.Items.Clear()
            For i = 0 To ninterval - 1
                lstElevRange.Items.Add(Interval_List(i + 1).LowerBound)
            Next
            lstElevRange.Items.Add(Interval_List(ninterval).UpperBound)
            'lstElevRange.SelectedIndex = 0
        End If
    End Sub

    Private Sub Display_DataStatus()

        'number of data layers, determined by the program developer
        'layers to be checked
        Dim ndata As Integer = 8

        'prepare list
        Dim datastatus(ndata) As String
        Dim datadesc(ndata) As String
        Dim DataName(ndata) As String

        Dim DataCount As Integer

        datadesc(0) = BA_MapStream
        DataName(0) = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.AoiStreamsVector), False)

        datadesc(1) = BA_MapPRISMElevation
        DataName(1) = BA_RasterElevationZones

        datadesc(2) = BA_MapElevationZone
        DataName(2) = BA_SubElevationZones

        datadesc(3) = BA_MapPrecipZone
        DataName(3) = BA_RasterPrecipitationZones

        datadesc(4) = BA_MapSNOTELZone
        DataName(4) = BA_RasterSNOTELZones

        datadesc(5) = BA_MapSnowCourseZone
        DataName(5) = BA_RasterSnowCourseZones

        datadesc(6) = BA_MapAspect
        DataName(6) = BA_RasterAspectZones

        datadesc(7) = BA_MapSlope
        DataName(7) = BA_RasterSlopeZones

        Dim i As Long

        lstDataStatus.Items.Clear()

        Try
            'check AOI stream
            Dim gdbPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers, True)
            If BA_File_Exists(gdbPath & DataName(0), WorkspaceType.Geodatabase, ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureClass) Then
                datastatus(0) = "Ready"
            Else
                datastatus(0) = " ?"
            End If

            'check zone rasters
            Flag_PrecipitationZone = False
            Flag_ElevationZone = False

            gdbPath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True)
            DataCount = 0
            For i = 1 To ndata - 1
                If BA_File_Exists(gdbPath & DataName(i), WorkspaceType.Geodatabase, ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTRasterDataset) Then
                    datastatus(i) = "Ready"
                    DataCount = DataCount + 1
                    Select Case i
                        Case 1 'Flag_ElevationZone
                            Flag_ElevationZone = True
                        Case 2 'Flag_PrecipitationZone
                            Flag_PrecipitationZone = True
                    End Select
                Else
                    datastatus(i) = " ?"
                End If
            Next

            For i = 0 To ndata - 1
                With lstDataStatus
                    Dim pitem As New ListViewItem(datastatus(i))
                    pitem.SubItems.Add(datadesc(i))
                    pitem.SubItems.Add(DataName(i))
                    .Items.Add(pitem)
                End With
            Next

            'check snotel and snow course data, tables and maps are still allowed
            'to be generated without the presence of these layers
            If datastatus(4) = " ?" Then 'snotel
                AOI_HasSNOTEL = False
                ndata = ndata - 1
            Else
                AOI_HasSNOTEL = True
            End If

            If datastatus(5) = " ?" Then 'snow course
                AOI_HasSnowCourse = False
                ndata = ndata - 1
            Else
                AOI_HasSnowCourse = True
            End If

            'disable elevation zone apply button if the zone exist
            If datastatus(2) = "Ready" Then
                cmdApplyElevInterval.Enabled = False
            Else
                cmdApplyElevInterval.Enabled = True
            End If

            'set UI control
            If DataCount = ndata - 1 AndAlso Flag_ElevOrPrecipChange = False Then 'not counting the AOI stream layer
                CmdMaps.Enabled = True
                If BA_Excel_Available Then
                    CmdTables.Enabled = True
                Else
                    CmdTables.Enabled = False
                End If
            Else
                'if PRISM zone intervals are populated but precipitation zones don't exist, enable CMdApplyPrism
                If datastatus(2) = "Ready" And datastatus(3) = " ?" And lstPrecipZones.Items.Count > 0 Then cmdApplyPRISMInterval.Enabled = True
                'enable generate zones button when elevation and precipitation zones exist
                If datastatus(2) = "Ready" And datastatus(3) = "Ready" Then CmdGenerate.Enabled = True
                CmdMaps.Enabled = False
                CmdTables.Enabled = False
            End If

        Catch ex As Exception
            Debug.Print("Display_DataStatus Exception: " & ex.Message)
        End Try

    End Sub

    Private Sub OptZMeters_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles OptZMeters.CheckedChanged
        If OptZMeters.Checked = True Then
            'Determine if Display ZUnit is the same as DEM ZUnit
            'AOI_DEMMin and AOI_DEMMax use internal system unit, i.e., meters
            Dim Conversion_Factor As Double = BA_SetConversionFactor(True, True) 'i.e., meters to meters

            lblElevUnit.Text = "Elevation (m):"

            'Populate Boxes
            txtMinElev.Text = Math.Round(AOI_DEMMin * Conversion_Factor - 0.005, 2) 'adjust value to include the actual min, max
            txtMaxElev.Text = Math.Round(AOI_DEMMax * Conversion_Factor + 0.005, 2)
            txtRangeElev.Text = Val(txtMaxElev.Text) - Val(txtMinElev.Text)

            Dim rvalues_arr() As BA_IntervalList = Nothing
            Dim subrvalues_arr() As BA_IntervalList = Nothing
            Dim ninterval As Integer, nsubinterval As Long

            ninterval = BA_CreateRangeArray(Val(txtMinElev.Text), Val(txtMaxElev.Text), Val(CmboxElevInterval.SelectedItem), rvalues_arr)
            txtElevClassNumber.Text = ninterval

            Display_IntervalList(rvalues_arr)
            nsubinterval = Subdivide_IntervalList(rvalues_arr, Val(CmboxElevInterval.Items(CmboxElevInterval.SelectedIndex)), _
                                                  subrvalues_arr, Val(ComboxSubDivide.Items(ComboxSubDivide.SelectedIndex)))
            Display_ElevationRange(rvalues_arr)
            ResetElevationRange()

            Map_Display_Elevation_in_Meters = True
            If Not Set_Silent_Mode Then
                MsgBox("You must reapply the change (i.e., click the 1. Apply button) on Elevation Distribution Map to update the change!" & _
                vbCrLf & "Or, reopen this dialog window to load the previous result.")
            End If

        End If
    End Sub

    Private Sub OptZFeet_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles OptZFeet.CheckedChanged
        If OptZFeet.Checked = True Then
            'Determine if Display ZUnit is the same as DEM ZUnit
            'AOI_DEMMin and AOI_DEMMax use internal system unit, i.e., meters
            Dim Conversion_Factor As Double = BA_SetConversionFactor(False, True) 'i.e., meters to feet

            lblElevUnit.Text = "Elevation (ft):"

            'Populate Boxes
            txtMinElev.Text = Math.Round(AOI_DEMMin * Conversion_Factor - 0.005, 2) 'adjust value to include the actual min, max
            txtMaxElev.Text = Math.Round(AOI_DEMMax * Conversion_Factor + 0.005, 2)
            txtRangeElev.Text = Val(txtMaxElev.Text) - Val(txtMinElev.Text)

            Dim rvalues_arr() As BA_IntervalList = Nothing
            Dim subrvalues_arr() As BA_IntervalList = Nothing
            Dim ninterval As Integer, nsubinterval As Long
            ninterval = BA_CreateRangeArray(Val(txtMinElev.Text), Val(txtMaxElev.Text), Val(CmboxElevInterval.SelectedItem), rvalues_arr)
            txtElevClassNumber.Text = ninterval

            Display_IntervalList(rvalues_arr)
            nsubinterval = Subdivide_IntervalList(rvalues_arr, Val(CmboxElevInterval.Items(CmboxElevInterval.SelectedIndex)), _
                subrvalues_arr, Val(ComboxSubDivide.Items(ComboxSubDivide.SelectedIndex)))
            Display_ElevationRange(rvalues_arr)
            ResetElevationRange()

            Map_Display_Elevation_in_Meters = False
            If Not Set_Silent_Mode Then
                MsgBox("You must reapply the change (i.e., click the 1. Apply button) on Elevation Distribution Map to update the change!" & _
                vbCrLf & "Or, reopen this dialog window to load the previous result.")
            End If
        End If
    End Sub

    'disable elevation range
    Private Sub ResetElevationRange()
        chkUseRange.Checked = False
        txtFromElev.Text = txtMinElev.Text
        txtToElev.Text = txtMaxElev.Text
    End Sub

    Private Sub InitForm()
        Elev_Interval = 200
        Elev_Subdivision = 1

        ComboxSubDivide.Items.Clear()
        With ComboxSubDivide
            .Items.Add("1")
            .Items.Add("2")
            .Items.Add("3")
            .Items.Add("4")
            .Items.Add("5")
            .SelectedIndex = 0
        End With

        CmboxPrecipType.Items.Clear()
        With CmboxPrecipType
            .Items.Add("Annual Precipitation")
            .Items.Add("Jan - Mar Precipitation")
            .Items.Add("Apr - Jun Precipitation")
            .Items.Add("Jul - Sep Precipitation")
            .Items.Add("Oct - Dec Precipitation")
            .Items.Add("Custom")
            .SelectedIndex = 0
        End With

        CmboxElevInterval.Items.Clear()
        With CmboxElevInterval
            .Items.Add("50")
            .Items.Add("100")
            .Items.Add("200")
            .Items.Add("250")
            .Items.Add("500")
            .Items.Add("1000")
            .Items.Add("2500")
            .Items.Add("5000")
            .SelectedIndex = 2
        End With

        CmboxBegin.Items.Clear()
        With CmboxBegin
            .Items.Add("1")
            .Items.Add("2")
            .Items.Add("3")
            .Items.Add("4")
            .Items.Add("5")
            .Items.Add("6")
            .Items.Add("7")
            .Items.Add("8")
            .Items.Add("9")
            .Items.Add("10")
            .Items.Add("11")
            .Items.Add("12")
            .SelectedIndex = 0
        End With

        CmboxEnd.Items.Clear()
        With CmboxEnd
            .Items.Add("1")
            .Items.Add("2")
            .Items.Add("3")
            .Items.Add("4")
            .Items.Add("5")
            .Items.Add("6")
            .Items.Add("7")
            .Items.Add("8")
            .Items.Add("9")
            .Items.Add("10")
            .Items.Add("11")
            .Items.Add("12")
            .SelectedIndex = 11
        End With

        CmboxAspect.Items.Clear()
        With CmboxAspect
            .Items.Add("4")
            .Items.Add("8")
            .Items.Add("16")
            .SelectedIndex = 2 'default to 16 aspect classes
        End With

        'set the value of BA_Excel_Available
        BA_Excel_Available = BA_Excel_Installed()

        Flag_BasinTables = False
        Flag_BasinMaps = False
    End Sub

    Private Sub chkUseRange_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkUseRange.CheckedChanged
        lblFromElev.Enabled = chkUseRange.Checked
        lblToElev.Enabled = chkUseRange.Checked
        txtFromElev.Enabled = chkUseRange.Checked
        txtToElev.Enabled = chkUseRange.Checked
        OptSelFrom.Enabled = chkUseRange.Checked
        OptSelTo.Enabled = chkUseRange.Checked
        lblSelectType.Enabled = chkUseRange.Checked
        lblSelNote.Enabled = chkUseRange.Checked
        lstElevRange.Enabled = chkUseRange.Checked
    End Sub

    Private Sub CmboxElevInterval_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles CmboxElevInterval.SelectedIndexChanged
        Elev_Interval = Val(CmboxElevInterval.SelectedItem)
        Dim rvalues_arr() As BA_IntervalList = Nothing
        Dim subrvalues_arr() As BA_IntervalList = Nothing
        Dim ninterval As Integer, nsubinterval As Long
        lstintervals.Items.Clear()
        ninterval = BA_CreateRangeArray(Val(txtMinElev.Text), Val(txtMaxElev.Text), Elev_Interval, rvalues_arr)
        txtElevClassNumber.Text = ninterval
        Display_IntervalList(rvalues_arr)

        nsubinterval = Subdivide_IntervalList(rvalues_arr, Elev_Interval, subrvalues_arr, Elev_Subdivision)
        Display_ElevationRange(rvalues_arr)
        ResetElevationRange()
        If m_formInit = True Then
            cmdApplyElevInterval.Enabled = True
            CmdGenerate.Enabled = False
            CmdTables.Enabled = False
            CmdMaps.Enabled = False
        End If
    End Sub

    Private Sub cmdApplyElevInterval_Click(sender As System.Object, e As System.EventArgs) Handles cmdApplyElevInterval.Click
        Dim response As Integer
        Dim MessageKey As String
        Dim LayerRemoved As Integer
        Dim DeleteStatus As Integer
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 15)
        Dim progressDialog2 As IProgressDialog2 = Nothing
        Dim pDEMGeoDataset As IGeoDataset = Nothing
        Dim pZoneRaster As IGeoDataset = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim pRasterBandCollection As IRasterBandCollection = Nothing
        Dim pTable As ITable
        Dim pCursor As ICursor
        Dim pRow As IRow
        Dim pZoneFeatureCursor As IFeatureCursor
        Dim pQueryFilter As IQueryFilter
        Dim pZoneFeature As IFeature
        Dim pExtractOp As IExtractionOp2 = New RasterExtractionOp
        Dim pAOIRaster As IGeoDataset = Nothing
        Dim pTempRaster As IGeoDataset = Nothing
        'Declarations for Spatial Filter
        Dim pGeo As IGeometry = Nothing
        Dim pSFilter As ISpatialFilter = Nothing
        Dim SnowCourseFeatureClass As IFeatureClass = Nothing
        Dim SNOTELFeatureClass As IFeatureClass = Nothing
        Dim pFeatureClass As IFeatureClass = Nothing

        '=====================================================================================
        '1. Get Min and Max Raster Value
        '=====================================================================================
        'Get Min and Max DEM Raster Value
        Dim interval As Object
        Dim IntervalList() As BA_IntervalList = Nothing
        Dim SubIntervalList() As BA_IntervalList = Nothing
        Dim ninterval As Integer, nsubinterval As Long
        Dim DisplayConversionFact As Double
        Dim DataConversionFact As Double
        Dim i As Integer, j As Integer

        Try
            interval = Val(CmboxElevInterval.SelectedItem)
            DisplayConversionFact = BA_SetConversionFactor(OptZMeters.Checked, True)

            'calculate range values for reclass in Display ZUnits
            ninterval = BA_CreateRangeArray(Math.Round(AOI_DEMMin * DisplayConversionFact - 0.005, 2), _
                                            Math.Round(AOI_DEMMax * DisplayConversionFact + 0.005, 2), interval, IntervalList)

            Elev_Interval = interval
            Elev_Subdivision = Val(ComboxSubDivide.SelectedItem)

            'calculate the subelevation list
            nsubinterval = Subdivide_IntervalList(IntervalList, Elev_Interval, SubIntervalList, Elev_Subdivision)

            'convert the range values to the DEM Zunit
            DataConversionFact = BA_SetConversionFactor(m_demInMeters, OptZMeters.Checked)

            If DataConversionFact <> 1 Then
                For i = 1 To ninterval
                    IntervalList(i).LowerBound = IntervalList(i).LowerBound * DataConversionFact
                    IntervalList(i).UpperBound = IntervalList(i).UpperBound * DataConversionFact
                Next

                For i = 1 To nsubinterval
                    SubIntervalList(i).LowerBound = SubIntervalList(i).LowerBound * DataConversionFact
                    SubIntervalList(i).UpperBound = SubIntervalList(i).UpperBound * DataConversionFact
                Next
            End If

            '=====================================================================================
            '2. Reclass Rasters (prism raster and sub_elevation raster)
            'prism raster: for elevation map and for prism precipitation analysis
            'sub_elevation raster: for creating elevation curve in excel charts
            '=====================================================================================
            '========================================
            'create elevation zones for prism summary
            '========================================
            'prism elevation zone is the same as the elevation mapping zone
            'use the prism elevation zone for elevation map

            'set parameters
            'Open the DEM of the AOI
            Dim strSavePath As String

            If Len(AOIFolderBase) > 0 Then
                Dim surfacesGdbPath As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces)
                pDEMGeoDataset = BA_OpenRasterFromGDB(surfacesGdbPath, BA_EnumDescription(MapsFileName.filled_dem_gdb))
            Else
                MsgBox("Please select an AOI!")
                Exit Sub
            End If

            cmdApplyElevInterval.Enabled = False
            progressDialog2 = BA_GetProgressDialog(pStepProg, "Processing Elevation Data", "Running...")
            pStepProg.Show()
            progressDialog2.ShowDialog()
            pStepProg.Step()

            'check and remove the output folder if it already exists
            strSavePath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis)  'analysis folder
            LayerRemoved = BA_RemoveLayersInFolder(My.Document, strSavePath) 'remove layers from map

            'remove elevation zone
            If BA_File_Exists(strSavePath & "\" & BA_RasterElevationZones, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                'delete raster
                DeleteStatus = BA_RemoveRasterFromGDB(strSavePath, BA_RasterElevationZones)
                If DeleteStatus = 0 Then 'unable to delete the folder
                    MsgBox("Unable to remove the folder " & strSavePath & "\" & BA_RasterElevationZones & ". Program stopped.")
                    Exit Sub
                End If
            End If

            'remove sub elevation zone
            If BA_File_Exists(strSavePath & "\" & BA_SubElevationZones, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                'delete raster
                DeleteStatus = BA_RemoveRasterFromGDB(strSavePath, BA_SubElevationZones)
                If DeleteStatus = 0 Then 'unable to delete the folder
                    MsgBox("Unable to remove the folder " & strSavePath & "\" & BA_SubElevationZones & ". Program stopped.")
                    Exit Sub
                End If
            End If

            MessageKey = "PRISM Elevation"
            pStepProg.Message = "Creating " & MessageKey & " Zones ..."
            pStepProg.Step()

            'reclassify and save the reclassified raster
            response = BA_ReclassRasterFromIntervalList(IntervalList, pDEMGeoDataset, strSavePath, BA_RasterElevationZones)
            'propogate interval list data to the attribute table of the grid
            response = BA_UpdateReclassRasterAttributes(IntervalList, strSavePath, BA_RasterElevationZones)
            'Try to enable Site Scenario tool after elevation zone is created
            Dim SiteScenarioButton = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of BtnSiteScenario)(My.ThisAddIn.IDs.BtnSiteScenario)
            SiteScenarioButton.selectedProperty = True


                MessageKey = "Subdivided-Elevation"
                pStepProg.Message = "Creating " & MessageKey & " Zones ..."
                pStepProg.Step()

                'repeat the same procedures for sub elevation raster
                response = BA_ReclassRasterFromIntervalList(SubIntervalList, pDEMGeoDataset, strSavePath, BA_SubElevationZones)
                response = BA_UpdateReclassRasterAttributes(SubIntervalList, strSavePath, BA_SubElevationZones)

                'open the raster elevation zone dataset for use
                pZoneRaster = BA_OpenRasterFromGDB(strSavePath, BA_RasterElevationZones)

                '=====================================================================================
                '3. Get Percent Area
                '=====================================================================================
                'Get zone Raster attribute
                pRasterBandCollection = pZoneRaster
                pRasterBand = pRasterBandCollection.Item(0)

                'Get Total Count of Cells
                Dim AreaSUM As Long
                Dim classarea() As Long
                Dim piField As Integer

                pTable = pRasterBand.AttributeTable
                pCursor = pTable.Search(Nothing, True)
                pRow = pCursor.NextRow

                AreaSUM = 0
                piField = pCursor.FindField(BA_FIELD_COUNT)
                ReDim classarea(0 To ninterval)

                For j = 1 To ninterval
                    classarea(j) = pRow.Value(piField)
                    AreaSUM = AreaSUM + classarea(j)
                    pRow = pCursor.NextRow
                Next

                'calculate Percent Area
                For j = 1 To ninterval
                    IntervalList(j).Area = (classarea(j) / AreaSUM) * 100
                Next

                '=====================================================================================
                '4. Convert Raster to Vector
                '=====================================================================================
                'Silently Delete Shapefile if Exists
                If BA_File_Exists(strSavePath & "\" & BA_VectorElevationZones, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                    'Remove Layer
                    response = BA_RemoveLayersInFolder(My.Document, strSavePath)
                    'Delete Dataset
                    response = BA_Remove_ShapefileFromGDB(strSavePath, BA_VectorElevationZones)
                End If

                'set mask on the pZoneRaster so that the vector version doesn't include the buffer
                'Use the AOI extent for analysis
                'Open AOI Polygon to set the analysis mask
                pAOIRaster = BA_OpenRasterFromGDB(BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Aoi), BA_AOIExtentRaster)
                If pAOIRaster Is Nothing Then
                    MsgBox("Cannot locate AOI boundary raster in the AOI.  Please re-run AOI Tool.")
                    Exit Sub
                End If

                pTempRaster = pExtractOp.Raster(pZoneRaster, pAOIRaster)
                response = BA_Raster2PolygonShapefile(strSavePath, BA_VectorElevationZones, pTempRaster)

                If response = 0 Then
                    MsgBox("Unable to convert the elevation zone raster to vector! Program stopped.")
                    Exit Sub
                End If

                'propogate interval list data to the attribute table of the shapefile
                response = BA_UpdateReclassVectorAttributes(IntervalList, strSavePath, BA_VectorElevationZones)

                '=====================================================================================
                '5. SNOTEL and Snow Course Analysis
                '=====================================================================================

                'Snow Course
                Dim SnowCourseBareName As String
                Dim SnowCourseParentName As String

                'Declarations for Opening SNOTEL File
                Dim SNOTELBareName As String
                Dim SNOTELParentName As String

                'Declarations for Within Array
                Dim nSTSite As Long
                Dim nSCSite As Long

                'Get BareName, ParentDirectory, and Extension
                SNOTELBareName = BA_SNOTELSites 'BA_GetBareNameAndExtension(frmSettings.txtSNOTEL.Text, SNOTELParentName, SNOTELExtension)
                SnowCourseBareName = BA_SnowCourseSites 'BA_GetBareNameAndExtension(frmSettings.txtSnowCourse.Text, SnowCourseParentName, SnowCourseExtension)
                SNOTELParentName = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers)
                SnowCourseParentName = SNOTELParentName

                Dim Has_SNOTELLayer As Boolean, Has_SnowCourseLayer As Boolean

                'Check to see if SNOTEL Shapefile Exist
                If BA_File_Exists(SNOTELParentName & "\" & SNOTELBareName, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                    Has_SNOTELLayer = True
                Else
                    Has_SNOTELLayer = False
                End If

                'Check to see if SnowCourse Shapefile Exist
                If BA_File_Exists(SnowCourseParentName & "\" & SnowCourseBareName, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                    Has_SnowCourseLayer = True
                Else
                    Has_SnowCourseLayer = False
                End If

                'Open Elevation zone file Files
                pFeatureClass = BA_OpenFeatureClassFromGDB(strSavePath, BA_VectorElevationZones)
                If Has_SNOTELLayer Then
                    'Open SNOTEL, Snowcourse, and Zone vector Shapefiles
                    SNOTELFeatureClass = BA_OpenFeatureClassFromGDB(SNOTELParentName, SNOTELBareName)

                    'Determine Number of Features Within Each Class
                    pQueryFilter = New QueryFilter
                    pZoneFeature = New Feature
                    pSFilter = New SpatialFilter

                    'Run Analysis
                    For j = 1 To ninterval
                        pQueryFilter.WhereClause = BA_FIELD_GRIDCODE & " = " & j
                        pZoneFeatureCursor = pFeatureClass.Search(pQueryFilter, False)
                        pZoneFeature = pZoneFeatureCursor.NextFeature

                        'Build Filter
                        nSTSite = 0

                        Do Until pZoneFeature Is Nothing
                            'Create Spatial Filter
                            pGeo = pZoneFeature.Shape

                            With pSFilter
                                .Geometry = pGeo
                                .GeometryField = BA_FIELD_SHAPE
                                .SpatialRel = esriSpatialRelEnum.esriSpatialRelContains
                            End With

                            'Get Number of sites within Filter
                            nSTSite = nSTSite + SNOTELFeatureClass.FeatureCount(pSFilter)
                            pZoneFeature = pZoneFeatureCursor.NextFeature
                        Loop

                        'Call next SNOTEL Feature
                        IntervalList(j).SNOTEL = nSTSite
                    Next
                Else
                    'reset count to zero
                    For j = 1 To ninterval
                        IntervalList(j).SNOTEL = 0
                    Next
                End If

                If Has_SnowCourseLayer Then
                    'Open Snowcourse, and Zone vector Shapefiles
                    SnowCourseFeatureClass = BA_OpenFeatureClassFromGDB(SnowCourseParentName, SnowCourseBareName)

                    'Determine Number of Features Within Each Class
                    pQueryFilter = New QueryFilter
                    pZoneFeature = New Feature
                    pSFilter = New SpatialFilter

                    'Run Analysis
                    For j = 1 To ninterval
                        pQueryFilter.WhereClause = BA_FIELD_GRIDCODE & " = " & j
                        pZoneFeatureCursor = pFeatureClass.Search(pQueryFilter, False)
                        pZoneFeature = pZoneFeatureCursor.NextFeature

                        'Build Filter
                        nSCSite = 0

                        Do Until pZoneFeature Is Nothing
                            'Create Spatial Filter
                            pGeo = pZoneFeature.Shape

                            With pSFilter
                                .Geometry = pGeo
                                .GeometryField = BA_FIELD_SHAPE
                                .SpatialRel = esriSpatialRelEnum.esriSpatialRelContains
                            End With

                            'Get Number of sites within Filter
                            nSCSite = nSCSite + SnowCourseFeatureClass.FeatureCount(pSFilter)
                            pZoneFeature = pZoneFeatureCursor.NextFeature
                        Loop

                        'Call next SNOTEL Feature
                        IntervalList(j).SnowCourse = nSCSite
                    Next
                Else
                    'reset count to zero
                    For j = 1 To ninterval
                        IntervalList(j).SnowCourse = 0
                    Next
                End If

                '=======================================================================================================================
                '7. Add Values to Form
                '=======================================================================================================================
                Display_IntervalList(IntervalList)

                '=================================
                'Update map parameters file
                '=================================
                Dim filepath As String, FileName As String


            Flag_ElevOrPrecipChange = True
            'check if map_parameters.txt file exists
                filepath = BA_GetPath(AOIFolderBase, PublicPath.Maps)
                FileName = BA_MapParameterFile 'i.e., map_parameters.txt
                response = UpdateMapParameters(filepath, FileName)

                If response <= 0 Then
                    response = SaveMapParameters(filepath, FileName)
                    '    MsgBox "Error! Unable to update map parameter file. Please report the error to the developer."
                End If

                'set flags
            Flag_ElevationZone = True
                Flag_BasinTables = False
                Flag_BasinMaps = False

                If Flag_PrecipitationZone Then
                    CmdGenerate.Enabled = True
                Else
                    CmdGenerate.Enabled = False
            End If

            'We only want to enable button #2 after this completes if there is not a previous analysis
            'If there is a previous analysis, there will be items in lstPrecipZones
            If lstPrecipZones.Items.Count = 0 Then
                cmdPRISM.Enabled = True
            End If

        Catch ex As Exception
            Debug.Print("cmdApplyElevInterval_Click Exception: " & ex.Message)
        Finally
            If pStepProg IsNot Nothing Then
                pStepProg.Hide()
                pStepProg = Nothing
            End If
            If progressDialog2 IsNot Nothing Then
                progressDialog2.HideDialog()
                progressDialog2 = Nothing
            End If

            pGeo = Nothing
            pSFilter = Nothing
            pZoneRaster = Nothing
            pZoneFeatureCursor = Nothing
            pZoneFeature = Nothing
            pQueryFilter = Nothing
            pFeatureClass = Nothing
            pZoneRaster = Nothing
            pTable = Nothing
            pCursor = Nothing
            pRow = Nothing
            pRasterBand = Nothing
            pRasterBandCollection = Nothing
            pDEMGeoDataset = Nothing
            pAOIRaster = Nothing
            SnowCourseFeatureClass = Nothing
            SNOTELFeatureClass = Nothing
        End Try
    End Sub

    Private Sub ComboxSubDivide_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboxSubDivide.SelectedIndexChanged
        Elev_Subdivision = Val(ComboxSubDivide.SelectedItem)
        Dim rvalues_arr() As BA_IntervalList = Nothing
        Dim subrvalues_arr() As BA_IntervalList = Nothing
        Dim ninterval As Integer, nsubinterval As Long
        ninterval = BA_CreateRangeArray(Val(txtMinElev.Text), Val(txtMaxElev.Text), Elev_Interval, rvalues_arr)
        nsubinterval = Subdivide_IntervalList(rvalues_arr, Elev_Interval, subrvalues_arr, Elev_Subdivision)
        Display_ElevationRange(rvalues_arr)
        ResetElevationRange()
        'Manage the form buttons if the form has already loaded
        If m_formInit = True Then
            cmdApplyElevInterval.Enabled = True
            CmdGenerate.Enabled = False
            CmdTables.Enabled = False 'disable the table button, users need to regenerate the elev zones
        End If
    End Sub

    Private Sub lstElevRange_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles lstElevRange.SelectedIndexChanged
        If chkUseRange.Checked = True Then
            If OptSelFrom.Checked = True Then
                txtFromElev.Text = lstElevRange.SelectedItem
            Else
                txtToElev.Text = lstElevRange.SelectedItem
            End If
        End If
    End Sub

    Private Sub CmboxPrecipType_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles CmboxPrecipType.SelectedIndexChanged
        If m_formInit = True Then
            cmdPRISM.Enabled = True
            CmdGenerate.Enabled = False
            CmdGenerate.Enabled = False
            CmdTables.Enabled = False
            CmdMaps.Enabled = False
        End If

        If CmboxPrecipType.SelectedIndex = 5 Then
            lblBeginMonth.Enabled = True
            CmboxBegin.Enabled = True
            lblEndMonth.Enabled = True
            CmboxEnd.Enabled = True
        Else
            lblBeginMonth.Enabled = False
            CmboxBegin.Enabled = False
            lblEndMonth.Enabled = False
            CmboxEnd.Enabled = False
        End If

        'reset the PRISM Dialog window
        lstPrecipZones.Items.Clear()
        txtMinPrecip.Text = ""
        txtMaxPrecip.Text = ""
        txtRangePrecip.Text = ""
        txtPrecipMapZoneInt.Text = "0"
        If m_formInit = True Then
            cmdApplyPRISMInterval.Enabled = False
        End If
    End Sub

    Private Sub cmdPRISM_Click(sender As System.Object, e As System.EventArgs) Handles cmdPRISM.Click
        'Get Number of Precipitation Zones
        Dim response As Integer
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 10)
        Dim progressDialog2 As IProgressDialog2 = Nothing
        Dim pRasterStats As IRasterStatistics

        'check for the presense of PRISM data before continue.
        Dim temppathname As String = AOIFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Prism) & "\Q4"
        If Not BA_File_Exists(temppathname, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            MsgBox("No PRISM data in the AOI! Please clip the data using the AOI Utility dialog.")
            Exit Sub
        End If

        Try
            progressDialog2 = BA_GetProgressDialog(pStepProg, "Compiling PRISM Precipitation Data", "Running...")
            pStepProg.Show()
            progressDialog2.ShowDialog()
            pStepProg.Step()

            PrecipPath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Prism, True)
            AnalysisPath = BA_GetPath(AOIFolderBase, PublicPath.Analysis)

            'Determine Precipitation Path
            '    PRISMLayer(1) = "Jan"
            '    PRISMLayer(2) = "Feb"
            '    PRISMLayer(3) = "Mar"
            '    PRISMLayer(4) = "Apr"
            '    PRISMLayer(5) = "May"
            '    PRISMLayer(6) = "Jun"
            '    PRISMLayer(7) = "Jul"
            '    PRISMLayer(8) = "Aug"
            '    PRISMLayer(9) = "Sep"
            '    PRISMLayer(10) = "Oct"
            '    PRISMLayer(11) = "Nov"
            '    PRISMLayer(12) = "Dec"
            '    PRISMLayer(13) = "Q1"
            '    PRISMLayer(14) = "Q2"
            '    PRISMLayer(15) = "Q3"
            '    PRISMLayer(16) = "Q4"
            '    PRISMLayer(17) = "Annual"

            If CmboxPrecipType.SelectedIndex = 0 Then  'read direct Annual PRISM raster
                PRISMRasterName = AOIPrismFolderNames.annual.ToString
            ElseIf CmboxPrecipType.SelectedIndex > 0 And CmboxPrecipType.SelectedIndex < 5 Then 'read directly Quarterly PRISM raster
                PRISMRasterName = BA_GetPrismFolderName(CmboxPrecipType.SelectedIndex + 12)
            Else 'sum individual monthly PRISM rasters
                response = BA_PRISMCustom(My.Document, AOIFolderBase, Val(CmboxBegin.SelectedItem), Val(CmboxEnd.SelectedItem))
                If response = 0 Then
                    MsgBox("Unable to generate custom PRISM layer! Program stopped.")
                    Exit Sub
                End If
                PrecipPath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True)
                PRISMRasterName = BA_TEMP_PRISM
            End If

            Dim raster_res As Double
            pRasterStats = BA_GetRasterStatsGDB(PrecipPath & PRISMRasterName, raster_res)

            'Populate Boxes
            txtMinPrecip.Text = Math.Round(pRasterStats.Minimum - 0.005, 2)
            txtMaxPrecip.Text = Math.Round(pRasterStats.Maximum + 0.005, 2)
            txtRangePrecip.Text = Val(txtMaxPrecip.Text) - Val(txtMinPrecip.Text)

            'create precipitation zones
            Dim minvalue As Object
            Dim maxvalue As Object
            Dim interval As Object
            Dim IntervalList() As BA_IntervalList = Nothing
            Dim ninterval As Integer

            minvalue = Val(txtMinPrecip.Text)
            maxvalue = Val(txtMaxPrecip.Text)

            'determine interval number based on map class #
            interval = (maxvalue - minvalue) / Val(txtPrecipMapZoneNo.Text)
            'round the number to 2 decimal place
            interval = Math.Round(interval, 2)

            'calculate range values for reclass
            ninterval = BA_CreateRangeArray(minvalue, maxvalue, interval, IntervalList)

            'display range information
            Display_PreciptIntervalList(IntervalList)
            txtPrecipMapZoneNo.Text = ninterval
            txtPrecipMapZoneInt.Text = interval

            '=================================
            'Update map parameters file
            '=================================
            Dim filepath As String, FileName As String

            'check if map_parameters.txt file exists
            filepath = BA_GetPath(AOIFolderBase, PublicPath.Maps)
            FileName = BA_MapParameterFile 'i.e., map_parameters.txt
            response = UpdateMapParametersPRISM(filepath, FileName)

            If response <= 0 Then
                response = SaveMapParameters(filepath, FileName)
            End If

            'set flags
            Flag_BasinTables = False
            Flag_BasinMaps = False

            cmdPRISM.Enabled = False
            cmdApplyPRISMInterval.Enabled = True
            'MsgBox("Change the Precipitation Zone Interval value to activate the Apply button!")
        Catch ex As Exception
            Debug.Print("cmdPRISM_Click Exception: " & ex.Message)
        Finally
            If pStepProg IsNot Nothing Then
                pStepProg.Hide()
                pStepProg = Nothing
            End If
            If progressDialog2 IsNot Nothing Then
                progressDialog2.HideDialog()
                progressDialog2 = Nothing
            End If
            pRasterStats = Nothing
        End Try
    End Sub

    Private Sub CmboxBegin_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles CmboxBegin.SelectedIndexChanged
        If m_formInit = True Then cmdPRISM.Enabled = True
    End Sub

    Private Sub CmboxEnd_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles CmboxEnd.SelectedIndexChanged
        If m_formInit = True Then cmdPRISM.Enabled = True
    End Sub

    Private Sub txtPrecipMapZoneInt_Validated(sender As Object, e As System.EventArgs) Handles txtPrecipMapZoneInt.Validated
        Dim minValue As Double = Val(txtMinPrecip.Text)
        Dim maxValue As Double = Val(txtMaxPrecip.Text)
        'determine interval number based on map class #
        Dim interval As Double = Val(txtPrecipMapZoneInt.Text)

        'Only run sub if min, max, interval values have been initialized
        If minValue + maxValue + interval <> 0 Then
            'create precipitation zones
            Dim IntervalList() As BA_IntervalList = Nothing
            Dim ninterval As Integer

            If interval <= 0 Or maxValue < minValue Then
                MsgBox("Invalid interval number or range value!")
                Exit Sub 'invalid parameters
            End If

            'calculate range values for reclass
            ninterval = BA_CreateRangeArray(minValue, maxValue, interval, IntervalList)

            'display range information
            Display_PreciptIntervalList(IntervalList)
            txtPrecipMapZoneNo.Text = ninterval
            cmdApplyPRISMInterval.Enabled = True
            CmdGenerate.Enabled = False
            CmdTables.Enabled = False
            CmdMaps.Enabled = False
        End If
    End Sub

    'Private Sub txtPrecipMapZoneInt_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtPrecipMapZoneInt.TextChanged
    '    Dim minValue As Double = Val(txtMinPrecip.Text)
    '    Dim maxValue As Double = Val(txtMaxPrecip.Text)
    '    'determine interval number based on map class #
    '    Dim interval As Double = Val(txtPrecipMapZoneInt.Text)

    '    'Only run sub if min, max, interval values have been initialized
    '    If minValue + maxValue + interval <> 0 Then
    '        'create precipitation zones
    '        Dim IntervalList() As BA_IntervalList = Nothing
    '        Dim ninterval As Integer

    '        If interval <= 0 Or maxValue < minValue Then
    '            MsgBox("Invalid interval number or range value!")
    '            Exit Sub 'invalid parameters
    '        End If

    '        'calculate range values for reclass
    '        ninterval = BA_CreateRangeArray(minValue, maxValue, interval, IntervalList)

    '        'display range information
    '        Display_PreciptIntervalList(IntervalList)
    '        txtPrecipMapZoneNo.Text = ninterval
    '        cmdApplyPRISMInterval.Enabled = True
    '    End If
    'End Sub

    Private Sub cmdApplyPRISMInterval_Click(sender As System.Object, e As System.EventArgs) Handles cmdApplyPRISMInterval.Click

        Dim response As Integer
        Dim PMinValue As Double
        Dim PMaxValue As Double
        Dim interval As Object
        Dim IntervalList() As BA_IntervalList = Nothing
        Dim ninterval As Integer
        Dim InputPath As String
        Dim InputName As String
        'Dim VectorName As String
        Const NO_VECTOR_NAME As String = ""
        Dim RasterName As String
        Dim strSavePath As String
        Dim pInputRaster As IGeoDataset
        Dim MessageKey As AOIMessageKey
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 10)
        Dim progressDialog2 As IProgressDialog2 = Nothing

        Try

            progressDialog2 = BA_GetProgressDialog(pStepProg, "Preparing Precipitation Zone Dataset", "Running...")
            pStepProg.Show()
            progressDialog2.ShowDialog()
            pStepProg.Step()
            cmdApplyPRISMInterval.Enabled = False

            '=================================
            'create precipitation zones
            '=================================
            'set parameters
            If CmboxPrecipType.SelectedIndex = 0 Then  'read direct Annual PRISM raster
                PrecipPath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Prism)
                PRISMRasterName = AOIPrismFolderNames.annual.ToString
            ElseIf CmboxPrecipType.SelectedIndex > 0 And CmboxPrecipType.SelectedIndex < 5 Then 'read directly Quarterly PRISM raster
                PrecipPath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Prism)
                PRISMRasterName = BA_GetPrismFolderName(CmboxPrecipType.SelectedIndex + 12)
            Else 'sum individual monthly PRISM rasters
                PrecipPath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis)
                PRISMRasterName = BA_TEMP_PRISM
            End If

            InputPath = PrecipPath
            InputName = PRISMRasterName
            'VectorName = BA_VectorPrecipitationZones
            RasterName = BA_EnumDescription(MapsFileName.PrecipZone)
            strSavePath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis)  'analysis fgdb
            MessageKey = AOIMessageKey.Precipitation

            pStepProg.Message = "Creating " & BA_EnumDescription(MessageKey) & " Zones ..."
            pStepProg.Step()

            'calculate range values for reclass
            PMinValue = Val(txtMinPrecip.Text)
            PMaxValue = Val(txtMaxPrecip.Text)
            interval = Val(txtPrecipMapZoneInt.Text)
            ninterval = BA_CreateRangeArray(PMinValue, PMaxValue, interval, IntervalList)

            'Open Input Raster and create the zone raster and vector
            pInputRaster = BA_OpenRasterFromGDB(InputPath, InputName)
            response = BA_MakeZoneDatasets(My.Document, pInputRaster, IntervalList, strSavePath, RasterName, NO_VECTOR_NAME, MessageKey)

            '=================================
            'Update map parameters file
            '=================================
            Dim filepath As String, FileName As String

            Flag_ElevOrPrecipChange = True

            'check if map_parameters.txt file exists
            filepath = BA_GetPath(AOIFolderBase, PublicPath.Maps)
            FileName = BA_MapParameterFile 'i.e., map_parameters.txt
            response = UpdateMapParametersPRISM(filepath, FileName)

            If response <= 0 Then
                response = SaveMapParameters(filepath, FileName)
                '    MsgBox "Error! Unable to update map parameter file. Please report the error to the developer."
            End If

            Flag_PrecipitationZone = True
            Flag_BasinTables = False
            Flag_BasinMaps = False

            If Flag_ElevationZone Then
                CmdGenerate.Enabled = True
            Else
                CmdGenerate.Enabled = False
            End If
        Catch ex As Exception
            Debug.Print(" Exception" & ex.Message)
            CmdGenerate.Enabled = True
            Display_DataStatus()
        Finally
            If pStepProg IsNot Nothing Then
                pStepProg.Hide()
                pStepProg = Nothing
            End If
            If progressDialog2 IsNot Nothing Then
                progressDialog2.HideDialog()
                progressDialog2 = Nothing
            End If
            pInputRaster = Nothing
        End Try
    End Sub

    Private Sub CmdGenerate_Click(sender As System.Object, e As System.EventArgs) Handles CmdGenerate.Click
        Dim response As Integer

        'generate all needed raster and vector datasets before generating Excel book
        'the layers to be generated are:
        'precipitation zone
        'elevation zone for precipitation distribution analysis
        'slope zones
        'aspect zones
        Dim i As Integer
        Dim IntervalList() As BA_IntervalList = Nothing
        Dim InputPath As String
        Dim InputName As String
        'Dim VectorName As String
        Const NO_VECTOR_NAME As String = ""
        Dim RasterName As String
        Dim strSavePath As String
        Dim pInputRaster As IGeoDataset
        Dim MessageKey As AOIMessageKey
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 10)
        Dim progressDialog2 As IProgressDialog2 = Nothing

        Try
            progressDialog2 = BA_GetProgressDialog(pStepProg, "Preparing Zone Datasets", "Running...")
            pStepProg.Show()
            progressDialog2.ShowDialog()
            pStepProg.Step()

            '=================================
            'create aspect zones
            '=================================
            'set parameters
            InputPath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Surfaces)
            InputName = BA_GetBareName(BA_EnumDescription(PublicPath.Aspect))
            'VectorName = BA_VectorAspectZones
            RasterName = BA_EnumDescription(MapsFileName.AspectZone)
            strSavePath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis)
            MessageKey = AOIMessageKey.Aspect

            pStepProg.Message = "Creating " & BA_EnumDescription(MessageKey) & " Zones ..."
            pStepProg.Step()

            'set reclass
            Dim AspectDirectionsNumber As Short = 2 ^ (CmboxAspect.SelectedIndex + 2) 'either 4, 8, or 16
            BA_SetAspectClasses(IntervalList, AspectDirectionsNumber)

            'Open Input Raster and create the zone raster and vector
            pInputRaster = BA_OpenRasterFromGDB(InputPath, InputName)
            response = BA_MakeZoneDatasets(My.Document, pInputRaster, IntervalList, strSavePath, RasterName, NO_VECTOR_NAME, MessageKey)
            pInputRaster = Nothing

            '=================================
            'create slope zones
            '=================================
            'set parameters
            InputName = BA_GetBareName(BA_EnumDescription(PublicPath.Slope))
            'VectorName = BA_VectorSlopeZones
            RasterName = BA_EnumDescription(MapsFileName.SlopeZone)
            MessageKey = AOIMessageKey.Slope

            pStepProg.Message = "Creating " & BA_EnumDescription(MessageKey) & " Zones ..."
            pStepProg.Step()

            'set reclass
            BA_SetSlopeClasses(IntervalList)

            'Open Input Raster and create the zone raster and vector
            pInputRaster = BA_OpenRasterFromGDB(InputPath, InputName)
            response = BA_MakeZoneDatasets(My.Document, pInputRaster, IntervalList, strSavePath, RasterName, NO_VECTOR_NAME, MessageKey)
            pInputRaster = Nothing

            Dim DataConversionFact As Double = BA_SetConversionFactor(m_demInMeters, True)

            ' Open SNOTEL and Snow Course Files
            Dim SNOTELNamePath As String
            Dim SNOTELParentPath As String
            Dim SNOTELBareName As String
            Dim AOILayerPath As String
            Dim SnowCourseNamePath As String
            Dim SnowCourseParentPath As String
            Dim SnowCourseBareName As String

            AOILayerPath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers) 'clipped SNOTEL and SnowCourse are in this folder
            SNOTELNamePath = BA_SystemSettings.SNOTELLayer
            SNOTELBareName = BA_EnumDescription(MapsFileName.Snotel)
            SNOTELParentPath = AOILayerPath

            SnowCourseNamePath = BA_SystemSettings.SCourseLayer
            SnowCourseBareName = BA_EnumDescription(MapsFileName.SnowCourse)
            SnowCourseParentPath = AOILayerPath

            '=================================
            'create snotel elevation zones
            '=================================
            'set parameters
            InputName = BA_EnumDescription(MapsFileName.filled_dem_gdb)
            'VectorName = BA_VectorSNOTELZones
            RasterName = BA_EnumDescription(MapsFileName.SnotelZone)
            MessageKey = AOIMessageKey.Snotel

            pStepProg.Message = "Creating " & BA_EnumDescription(MessageKey) & " Zones ..."
            pStepProg.Step()

            'SNOTEL and Snow Course elevation internal data units are always in meters
            'AOI_DEMMin and Max have the same unit as SNOTEL's ZUnit, i.e., in meters.
            response = BA_GetUniqueSortedValues(SNOTELParentPath, SNOTELBareName, BA_FIELD_SITE_NAME, BA_SiteElevField, AOI_DEMMin, AOI_DEMMax, IntervalList)

            'Converts SNOTEL integer into DEM ZUnit
            Dim ListMax As Object
            ListMax = UBound(IntervalList)
            For i = 1 To ListMax
                IntervalList(i).LowerBound = IntervalList(i).LowerBound * DataConversionFact
                IntervalList(i).UpperBound = IntervalList(i).UpperBound * DataConversionFact
            Next

            If response = 0 Then 'input attribute out of the elevation bounds
                MsgBox("Elevation data in the SNOTEL layer are out of the DEM elevation range!")
            ElseIf response > 0 Then
                'Open Input Raster and create the zone raster and vector
                pInputRaster = BA_OpenRasterFromGDB(InputPath, InputName)
                response = BA_MakeZoneDatasets(My.Document, pInputRaster, IntervalList, strSavePath, RasterName, NO_VECTOR_NAME, MessageKey)
                pInputRaster = Nothing
            End If

            '===================================
            'create snow course elevation zones
            '===================================
            'set parameters
            'VectorName = BA_VectorSnowCourseZones
            RasterName = BA_EnumDescription(MapsFileName.SnowCourseZone)
            MessageKey = AOIMessageKey.SnowCourse

            pStepProg.Message = "Creating " & BA_EnumDescription(MessageKey) & " Zones ..."
            pStepProg.Step()

            'Converts Display Range to SNOTEL ZUnit
            response = BA_GetUniqueSortedValues(SnowCourseParentPath, SnowCourseBareName, BA_FIELD_SITE_NAME, BA_SiteElevField, AOI_DEMMin, AOI_DEMMax, IntervalList)

            'Converts Snow course integer into DEM ZUnit
            ListMax = UBound(IntervalList)
            For i = 1 To ListMax
                IntervalList(i).LowerBound = IntervalList(i).LowerBound * DataConversionFact
                IntervalList(i).UpperBound = IntervalList(i).UpperBound * DataConversionFact
            Next

            If response = 0 Then 'input attribute out of the elevation bounds
                MsgBox("Elevation data in the snow course layer are out of the DEM elevation range!")
            ElseIf response > 0 Then
                'Open Input Raster and create the zone raster and vector
                pInputRaster = BA_OpenRasterFromGDB(InputPath, InputName)
                response = BA_MakeZoneDatasets(My.Document, pInputRaster, IntervalList, strSavePath, RasterName, NO_VECTOR_NAME, MessageKey)
                pInputRaster = Nothing
            End If

            '=================================
            'Update map parameters file
            '=================================
            Dim filepath As String, FileName As String

            Flag_ElevOrPrecipChange = False

            'check if map_parameters.txt file exists
            filepath = BA_GetPath(AOIFolderBase, PublicPath.Maps)
            FileName = BA_MapParameterFile 'i.e., map_parameters.txt
            response = SaveMapParameters(filepath, FileName)
            Display_DataStatus()
            'set flags
            Flag_BasinMaps = True
            Flag_BasinTables = True

            CmdGenerate.Enabled = False
            If response <= 0 Then MsgBox("Error! Unable to update map parameter file. Please report the error to the developer.")
        Catch ex As Exception
            Debug.Print("CmdGenerate_Click Exception: " & ex.Message)
            Display_DataStatus()
            CmdGenerate.Enabled = False
        Finally
            If pStepProg IsNot Nothing Then
                pStepProg.Hide()
                pStepProg = Nothing
            End If
            If progressDialog2 IsNot Nothing Then
                progressDialog2.HideDialog()
                progressDialog2 = Nothing
            End If
        End Try
    End Sub

    Private Sub AddLayersToMap()
        Dim Basin_Name As String
        Dim cboSelectedBasin = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of cboTargetedBasin)(My.ThisAddIn.IDs.cboTargetedBasin)
        Dim cboSelectedAoi = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of cboTargetedAOI)(My.ThisAddIn.IDs.cboTargetedAOI)

        If Len(Trim(cboSelectedBasin.getValue)) = 0 Then
            Basin_Name = ""
        Else
            Basin_Name = cboSelectedBasin.getValue
        End If

        'BA_ActivateMapFrame BA_DefaultMapName
        Dim response As Integer = BA_AddLayerstoMapFrame(My.ThisApplication, My.Document, AOIFolderBase, AOI_HasSNOTEL, AOI_HasSnowCourse, Scenario1Map_Flag, Scenario2Map_Flag)
        BA_AddMapElements(My.Document, cboSelectedAoi.getValue & Basin_Name, "Subtitle BAGIS")
        response = BA_DisplayMap(My.Document, 1, Basin_Name, cboSelectedAoi.getValue, Map_Display_Elevation_in_Meters, _
                                 "Elevation Distribution")
        BA_RemoveLayersfromLegend(My.Document)
    End Sub

    Private Sub CmdMaps_Click(sender As System.Object, e As System.EventArgs) Handles CmdMaps.Click
        AddLayersToMap()
        Call BA_Enable_MapFlags(True)
        MsgBox("Please use the menu items to view maps!")
        Me.Close()
    End Sub

    Private Sub CmdTables_Click(sender As System.Object, e As System.EventArgs) Handles CmdTables.Click
        Dim response As Integer
        Dim SNOTELParentPath As String
        Dim SNOTELBareName As String
        Dim AOILayerPath As String
        Dim SnowCourseParentPath As String
        Dim SnowCourseBareName As String
        Dim EMinValue As Object
        Dim EMaxValue As Object

        'verify elevation range values if used
        If chkUseRange.Checked = True Then
            If Val(txtFromElev.Text) < Val(txtMinElev.Text) Or _
                Val(txtToElev.Text) > Val(txtMaxElev.Text) Or _
                Val(txtFromElev.Text) >= Val(txtToElev.Text) Then
                MsgBox("Invalid elevation range specified for localized analysis!")
                Exit Sub
            End If
        End If

        If OptZMeters.Checked = True Then
            BA_ElevationUnitString = "Meters"
        Else
            BA_ElevationUnitString = "Feet"
        End If

        AOILayerPath = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Layers)   'clipped SNOTEL and SnowCourse are in this folder
        SNOTELParentPath = AOILayerPath
        SNOTELBareName = BA_EnumDescription(MapsFileName.Snotel)
        SnowCourseParentPath = AOILayerPath
        SnowCourseBareName = BA_EnumDescription(MapsFileName.SnowCourse)

        EMinValue = Val(txtMinElev.Text)
        EMaxValue = Val(txtMaxElev.Text)

        'set the y axis values of the excel charts
        BA_Excel_SetYAxis(CDbl(EMinValue), CDbl(EMaxValue), CDbl(CmboxElevInterval.SelectedItem))

        'Declare Excel object variables
        Dim objExcel As New Microsoft.Office.Interop.Excel.Application
        Dim bkWorkBook As Workbook 'a file in excel
        bkWorkBook = objExcel.Workbooks.Add

        'Declare progress indicator variables
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 15)
        Dim progressDialog2 As IProgressDialog2 = Nothing

        Try
            progressDialog2 = BA_GetProgressDialog(pStepProg, "Generating Basin Analysis Tables", "Running...")
            pStepProg.Show()
            progressDialog2.ShowDialog()
            pStepProg.Step()


            '=============================================
            ' Create Excel WorkSheets
            '=============================================
            pStepProg.Message = "Preparing Excel Spreadsheets..."
            pStepProg.Step()

            'Create Subdivided Elevation Distribution Worksheet for plotting curve
            Dim pSubElvWorksheet As Worksheet = bkWorkBook.ActiveSheet
            pSubElvWorksheet.Name = "Elevation Curve"

            'Create SNOTEL Distribution Worksheet
            Dim pSNOTELWorksheet As Worksheet = bkWorkBook.Sheets.Add
            pSNOTELWorksheet.Name = "SNOTEL"

            'Create Snow Course Distribution Worksheet
            Dim pSnowCourseWorksheet As Worksheet = bkWorkBook.Sheets.Add
            pSnowCourseWorksheet.Name = "Snow Course"

            'Create Snow Course Distribution Worksheet
            Dim pAspectWorksheet As Worksheet = bkWorkBook.Sheets.Add
            pAspectWorksheet.Name = "Aspect"

            'Create Snow Course Distribution Worksheet
            Dim pSlopeWorksheet As Worksheet = bkWorkBook.Sheets.Add
            pSlopeWorksheet.Name = "Slope"

            'Create Snow Course Distribution Worksheet
            Dim pPRISMWorkSheet As Worksheet = bkWorkBook.Sheets.Add
            pPRISMWorkSheet.Name = "PRISM"

            'Create Elevation Distribution Worksheet
            Dim pAreaElvWorksheet As Worksheet = bkWorkBook.Sheets.Add
            pAreaElvWorksheet.Name = "Area Elevations"

            'Dim variables for the range worksheets in case we need them later
            Dim pSCRangeWorksheet As Worksheet = Nothing
            Dim pElevationRangeWorksheet As Worksheet = Nothing
            Dim pPrecipitationRangeWorksheet As Worksheet = Nothing
            Dim pRangeChartWorksheet As Worksheet = Nothing
            Dim pChartsWorksheet As Worksheet = Nothing
            Dim pSTRangeWorksheet As Worksheet = Nothing
            If chkUseRange.Checked = True Then
                If AOI_HasSnowCourse Then
                    pSCRangeWorksheet = bkWorkBook.Sheets.Add
                    pSCRangeWorksheet.Name = "Snow Course Range"
                End If

                If AOI_HasSNOTEL Then
                    pSTRangeWorksheet = bkWorkBook.Sheets.Add
                    pSTRangeWorksheet.Name = "SNOTEL Range"
                End If

                pPrecipitationRangeWorksheet = bkWorkBook.Sheets.Add
                pPrecipitationRangeWorksheet.Name = "PRISM Range"

                pElevationRangeWorksheet = bkWorkBook.Sheets.Add
                pElevationRangeWorksheet.Name = "Elevation Range"

                pRangeChartWorksheet = bkWorkBook.Sheets.Add
                pRangeChartWorksheet.Name = "Range Charts"

                '=================================
                'Update map parameters file
                '=================================
                Dim filepath As String, FileName As String

                'check if map_parameters.txt file exists
                filepath = BA_GetPath(AOIFolderBase, PublicPath.Maps)
                FileName = BA_MapParameterFile 'i.e., map_parameters.txt
                response = SaveMapParameters(filepath, FileName)
                If response <= 0 Then MsgBox("Error! Unable to update map parameter file. Please report the error to the developer.")
            End If

            'Create Snow Course Distribution Worksheet
            pChartsWorksheet = bkWorkBook.Sheets.Add
            pChartsWorksheet.Name = "Charts"

            '============================================
            '4. Create Elevation Excel Table
            '============================================
            Dim MaxPRISMValue As Double

            pStepProg.Message = "Creating Elevation Tables and Charts..."
            pStepProg.Step()

            'create elevation table for summary statistics
            Dim conversionFactor As Double
            If OptZFeet.Checked = True Then 'Display = Feet
                If m_demInMeters = False Then 'DEM = Feet
                    conversionFactor = 1
                Else 'DEM = METERS
                    conversionFactor = 3.2808399 'Meters to Foot
                End If
            Else 'Display = Meters
                If m_demInMeters = False Then 'DEM = Feet
                    conversionFactor = 0.3048 'Foot to Meters
                Else 'DEM = Meters
                    conversionFactor = 1
                End If
            End If
            response = BA_Excel_CreateElevationTable(AOIFolderBase, pAreaElvWorksheet, conversionFactor, AOI_DEMMin, OptZMeters.Checked)

            'create subdivided elevation table for plotting the curve
            response = BA_Excel_CreateSubElevationTable(AOIFolderBase, pSubElvWorksheet, conversionFactor, AOI_DEMMin, OptZMeters.Checked)
            response = BA_Excel_CreateElevationChart(pSubElvWorksheet, pChartsWorksheet, BA_ChartSpacing, BA_ChartSpacing, Chart_YMinScale, Chart_YMaxScale, _
                                                     Chart_YMapUnit, OptZMeters.Checked, OptZFeet.Checked)

            If AOI_HasSNOTEL Then
                pStepProg.Message = "Creating SNOTEL Table and Chart..."
                pStepProg.Step()
                response = BA_Excel_CreateSNOTELTable(AOIFolderBase, pSNOTELWorksheet, pSubElvWorksheet, BA_EnumDescription(MapsFileName.SnotelZone), conversionFactor)
                response = BA_Excel_CreateSNOTELChart(pSNOTELWorksheet, pSubElvWorksheet, pChartsWorksheet, True, _
                    BA_ChartWidth + BA_ChartSpacing + BA_ChartSpacing, BA_ChartHeight + BA_ChartSpacing + BA_ChartSpacing, _
                    Chart_YMinScale, Chart_YMaxScale, Chart_YMapUnit, OptZMeters.Checked, OptZFeet.Checked)
            End If

            If AOI_HasSnowCourse Then
                pStepProg.Message = "Creating Snow Course Table and Chart..."
                pStepProg.Step()

                response = BA_Excel_CreateSNOTELTable(AOIFolderBase, pSnowCourseWorksheet, pSubElvWorksheet, BA_EnumDescription(MapsFileName.SnowCourseZone), conversionFactor)
                response = BA_Excel_CreateSNOTELChart(pSnowCourseWorksheet, pSubElvWorksheet, pChartsWorksheet, False, _
                        BA_ChartWidth + BA_ChartSpacing + BA_ChartSpacing, (BA_ChartHeight + BA_ChartSpacing) * 2 + BA_ChartSpacing, _
                        Chart_YMinScale, Chart_YMaxScale, Chart_YMapUnit, OptZMeters.Checked, OptZFeet.Checked)
            End If

            pStepProg.Message = "Creating ASPECT Table and Chart..."
            pStepProg.Step()

            response = BA_Excel_CreateAspectTable(AOIFolderBase, pAspectWorksheet)
            response = BA_Excel_CreateAspectChart(pAspectWorksheet, pChartsWorksheet)

            pStepProg.Message = "Creating SLOPE Table and Chart..."
            pStepProg.Step()

            response = BA_Excel_CreateSlopeTable(AOIFolderBase, pSlopeWorksheet)
            response = BA_Excel_CreateSlopeChart(pSlopeWorksheet, pChartsWorksheet)

            pStepProg.Message = "Creating Precipitation Table and Chart..."
            pStepProg.Step()

            'Calculate file path for prism based on the form
            Dim PRISMFolderName As String = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Prism, True)
            If CmboxPrecipType.SelectedIndex = 0 Then  'read direct Annual PRISM raster
                PRISMRasterName = AOIPrismFolderNames.annual.ToString
            ElseIf CmboxPrecipType.SelectedIndex > 0 And CmboxPrecipType.SelectedIndex < 5 Then 'read directly Quarterly PRISM raster
                PRISMRasterName = BA_GetPrismFolderName(CmboxPrecipType.SelectedIndex + 12)
            Else 'sum individual monthly PRISM rasters
                PRISMFolderName = BA_GeodatabasePath(AOIFolderBase, GeodatabaseNames.Analysis, True)
                PRISMRasterName = BA_TEMP_PRISM
            End If
            response = BA_Excel_CreatePRISMTable(AOIFolderBase, pPRISMWorkSheet, pSubElvWorksheet, MaxPRISMValue, _
                                                 PRISMFolderName & PRISMRasterName, AOI_DEMMin, conversionFactor, OptZMeters.Checked)
            response = BA_Excel_CreatePRISMChart(pPRISMWorkSheet, pSubElvWorksheet, pChartsWorksheet, _
                                                 BA_ChartWidth + BA_ChartSpacing + BA_ChartSpacing, BA_ChartSpacing, _
                                                 Chart_YMinScale, Chart_YMaxScale, Chart_YMapUnit, MaxPRISMValue, OptZMeters.Checked, _
                                                 OptZFeet.Checked)

            pStepProg.Message = "Creating Combined Charts..."
            pStepProg.Step()

            response = BA_Excel_CreateCombinedChart(pPRISMWorkSheet, pSubElvWorksheet, pChartsWorksheet, pSnowCourseWorksheet, _
                                                    pSNOTELWorksheet, Chart_YMinScale, Chart_YMaxScale, Chart_YMapUnit, MaxPRISMValue, _
                                                    OptZMeters.Checked, OptZFeet.Checked, AOI_HasSNOTEL, AOI_HasSnowCourse)

            'copy DEM area and %_area to the PRISM table
            'response = Excel_CopyCells(pAreaElvWorksheet, 3, pPRISMWorkSheet, 12)
            response = BA_Excel_CopyCells(pAreaElvWorksheet, 3, pPRISMWorkSheet, 12)
            'response = Excel_CopyCells(pAreaElvWorksheet, 10, pPRISMWorkSheet, 13)
            response = BA_Excel_CopyCells(pAreaElvWorksheet, 10, pPRISMWorkSheet, 13)
            response = BA_Excel_PrecipitationVolume(pPRISMWorkSheet, 12, 7, 14, 15)

            If chkUseRange.Checked = True Then
                pStepProg.Message = "Creating Elevation Range Tables and Charts..."
                pStepProg.Step()

                response = BA_Excel_CreateElevRangeTable(pElevationRangeWorksheet, pSubElvWorksheet, CDbl(txtFromElev.Text), CDbl(txtToElev.Text))
                response = BA_Excel_CreatePrecipRangeTable(pPrecipitationRangeWorksheet, pPRISMWorkSheet, CDbl(txtFromElev.Text), CDbl(txtToElev.Text), MaxPRISMValue)

                response = BA_Excel_CreateElevationChart(pElevationRangeWorksheet, pRangeChartWorksheet, BA_ChartSpacing, BA_ChartSpacing, _
                    CDbl(txtFromElev.Text), CDbl(txtToElev.Text), Chart_YMapUnit, OptZMeters.Checked, OptZFeet.Checked)

                If AOI_HasSNOTEL Then
                    response = BA_Excel_CreateSNOTELRangeTable(pSTRangeWorksheet, pSNOTELWorksheet, pSubElvWorksheet, CDbl(txtFromElev.Text), CDbl(txtToElev.Text))
                    response = BA_Excel_CreateSNOTELChart(pSTRangeWorksheet, pElevationRangeWorksheet, pRangeChartWorksheet, True, _
                                                          BA_ChartWidth + BA_ChartSpacing + BA_ChartSpacing, BA_ChartHeight + BA_ChartSpacing + BA_ChartSpacing, _
                                                          CDbl(txtFromElev.Text), CDbl(txtToElev.Text), Chart_YMapUnit, OptZMeters.Checked, OptZFeet.Checked)
                End If

                If AOI_HasSnowCourse Then
                    response = BA_Excel_CreateSNOTELRangeTable(pSCRangeWorksheet, pSnowCourseWorksheet, pSubElvWorksheet, CDbl(txtFromElev.Text), CDbl(txtToElev.Text))
                    response = BA_Excel_CreateSNOTELChart(pSCRangeWorksheet, pElevationRangeWorksheet, pRangeChartWorksheet, False, _
                        BA_ChartWidth + BA_ChartSpacing + BA_ChartSpacing, (BA_ChartHeight + BA_ChartSpacing) * 2 + BA_ChartSpacing, _
                        CDbl(txtFromElev.Text), CDbl(txtToElev.Text), Chart_YMapUnit, OptZMeters.Checked, OptZFeet.Checked)
                End If

                response = BA_Excel_CreatePRISMChart(pPrecipitationRangeWorksheet, pElevationRangeWorksheet, pRangeChartWorksheet, _
                                                     BA_ChartWidth + BA_ChartSpacing + BA_ChartSpacing, BA_ChartSpacing, _
                                                     CDbl(txtFromElev.Text), CDbl(txtToElev.Text), Chart_YMapUnit, MaxPRISMValue, _
                                                     OptZMeters.Checked, OptZFeet.Checked)

                response = BA_Excel_CreateCombinedChart(pPrecipitationRangeWorksheet, pElevationRangeWorksheet, pRangeChartWorksheet, pSCRangeWorksheet, _
                                                        pSTRangeWorksheet, CDbl(txtFromElev.Text), CDbl(txtToElev.Text), Chart_YMapUnit, MaxPRISMValue, _
                                                        OptZMeters.Checked, OptZFeet.Checked, AOI_HasSNOTEL, AOI_HasSnowCourse)
            End If

        Catch ex As Exception
            Debug.Print("CmdTables_Click Exception: " & ex.Message)
        Finally
            objExcel.Visible = True
            If pStepProg IsNot Nothing Then
                pStepProg.Hide()
                pStepProg = Nothing
            End If
            If progressDialog2 IsNot Nothing Then
                progressDialog2.HideDialog()
                progressDialog2 = Nothing
            End If
        End Try
    End Sub

    Private Sub CmboxAspect_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles CmboxAspect.SelectedIndexChanged
        If Flag_ElevationZone And Flag_PrecipitationZone Then
            CmdGenerate.Enabled = True
        Else
            CmdGenerate.Enabled = False
        End If
        CmdMaps.Enabled = False
        CmdTables.Enabled = False
    End Sub

End Class