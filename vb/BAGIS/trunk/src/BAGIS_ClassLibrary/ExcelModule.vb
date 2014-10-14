Option Strict Off
Option Explicit On

Imports Microsoft.Office.Interop.Excel
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.SpatialAnalyst
Imports Microsoft.Office.Interop
Imports Microsoft.Office.Core
Imports ESRI.ArcGIS.GeoAnalyst

Public Module ExcelModule

    Public Chart_YMaxScale As Double
    Public Chart_YMinScale As Double
    Public Chart_YMapUnit As Double

    'count the number of records in a worksheet based on the values on the first column
    'aspect, slope, snotel, and snow course tables have a beginning_row value of 2
    'other tables have a value of 3.
    Public Function BA_Excel_CountRecords(ByVal pworksheet As Worksheet, ByVal beginning_row As Integer) As Long
        Dim count As Long = pworksheet.UsedRange.Rows.Count
        Dim validRow As Long = 0
        For i = beginning_row To count
            Dim cell As Range = pworksheet.UsedRange.Cells(i, 1)
            Dim strCell As String = cell.ToString
            If Not String.IsNullOrEmpty(strCell) Then
                validRow = validRow + 1
            End If
        Next
        Return validRow
    End Function

    'copy the cell values in the source worksheet to the target worksheet
    Public Function BA_Excel_CopyCells(pSourceWS As Worksheet, SourceCol As Integer, pTargetWS As Worksheet, TargetCol As Integer) As Long
        Dim pRange As Range
        Dim returnvalue As Long
        Dim row_index As Long
        row_index = 3
        returnvalue = 0
        pRange = pSourceWS.Cells(row_index, SourceCol)

        Do While Len(pRange.Text.ToString) > 0
            Dim targetRange As Range = pTargetWS.Cells(row_index, TargetCol)
            targetRange.Value = pRange.Value
            row_index = row_index + 1
            returnvalue = returnvalue + 1
            pRange = pSourceWS.Cells(row_index, SourceCol)
        Loop
        Return returnvalue
    End Function

    'copy the cell values in the source worksheet to the target worksheet within a specified range
    'return the number of cells copied
    Public Function BA_Excel_CopyRangeCells(ByRef pSourceWS As Worksheet, ByRef SourceCol As Short, ByRef pTargetWS As Worksheet, ByRef TargetCol As Short, ByRef Begin_Cell As Short, ByRef End_Cell As Short, ByRef Row_OffSet As Short) As Integer
        Dim usedRange As Range = pSourceWS.UsedRange
        Dim returnvalue As Integer = 0
        Dim i As Integer
        Dim ncell As Integer

        If End_Cell < Begin_Cell Then
            Return 0
        End If

        ncell = End_Cell - Begin_Cell + 1

        For i = 1 To ncell
            Dim sourceCell As Range = usedRange.Cells(Begin_Cell + i - 1, SourceCol)
            pTargetWS.Cells(i + Row_OffSet - 1, TargetCol) = sourceCell
            returnvalue = returnvalue + 1
        Next
        BA_Excel_CopyRangeCells = returnvalue
    End Function

    Public Function BA_Excel_Installed() As Boolean
        Dim objKey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.ClassesRoot
        Dim objSubKey As Microsoft.Win32.RegistryKey = objKey.OpenSubKey("Excel.Application")
        Dim retVal As Boolean = Not objSubKey Is Nothing
        objKey.Close()
        Return retVal
    End Function

    'set the y-axis based on the elevation range and interval values
    Public Sub BA_Excel_SetYAxis(ByVal minvalue As Double, ByVal maxvalue As Double, ByVal interval As Double)
        Dim quotient As Integer
        Dim modvalue As Integer

        Chart_YMapUnit = interval
        quotient = Int(minvalue / interval)
        Chart_YMinScale = quotient * interval

        modvalue = maxvalue Mod interval
        If modvalue = 0 Then
            quotient = Int(maxvalue / interval)
        Else
            quotient = Int(maxvalue / interval) + 1
        End If

        Chart_YMaxScale = quotient * interval
    End Sub

    Public Function BA_Excel_CreateElevationTable(ByVal aoiPath As String, ByRef pworksheet As Worksheet, ByVal conversionFactor As Double, ByVal aoiDemMin As Double, _
                                                  ByVal optZMetersChecked As Boolean) As Integer

        If Len(aoiPath) <= 0 Then
            MsgBox("Please select an AOI!")
            Return 0
        End If

        Dim ZRasterName As String, VRasterName As String
        Dim ZInputPath As String, VInputPath As String
        Dim i As Integer
        Dim IntervalList() As BA_IntervalList = Nothing

        ZRasterName = BA_EnumDescription(MapsFileName.ElevationZone)
        ZInputPath = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Analysis)
        'read class definition for chart and table labelling
        Dim success As BA_ReturnCode = BA_ReadReclassRasterAttributeGDB(IntervalList, ZInputPath, ZRasterName)
        VRasterName = BA_EnumDescription(MapsFileName.filled_dem_gdb)
        VInputPath = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Surfaces)

        Dim pZoneRaster As IGeoDataset = BA_OpenRasterFromGDB(ZInputPath, ZRasterName)
        Dim pValueRaster As IGeoDataset = BA_OpenRasterFromGDB(VInputPath, VRasterName)

        'Use the AOI extent for analysis
        Dim pAOIRaster As IGeoDataset
        'Open AOI Polygon to set the analysis mask
        Dim aoiGdbPath As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Aoi)
        Dim aoiFileName As String = BA_GetBareName(BA_EnumDescription(PublicPath.AoiGrid))
        pAOIRaster = BA_OpenRasterFromGDB(aoiGdbPath, aoiFileName)
        If pAOIRaster Is Nothing Then
            MsgBox("Cannot locate AOI boundary raster in the AOI.  Please re-run AOI Tool.")
            Return 0
        End If

        Dim pTempRaster As IGeoDataset
        Dim pExtractOp As IExtractionOp2 = New RasterExtractionOp
        pTempRaster = pExtractOp.Raster(pZoneRaster, pAOIRaster)
        pExtractOp = Nothing
        pZoneRaster = Nothing
        pAOIRaster = Nothing

        '===========================
        'Zonal Statistics
        '===========================
        Dim ZonalOp As IZonalOp
        Dim ZonalTable As ITable
        ZonalOp = New RasterZonalOp
        ZonalTable = ZonalOp.ZonalStatisticsAsTable(pTempRaster, pValueRaster, True)
        ZonalOp = Nothing
        pZoneRaster = Nothing
        pValueRaster = Nothing
        pTempRaster = Nothing

        'Create Field Indexes
        Dim MinIndex As Integer, MaxIndex As Integer, MeanIndex As Integer
        Dim RangeIndex As Integer, SumIndex As Integer, STDIndex As Integer
        Dim AreaIndex As Integer, CountIndex As Integer, ValueIndex As Integer

        MinIndex = ZonalTable.FindField("MIN")
        MaxIndex = ZonalTable.FindField("MAX")
        MeanIndex = ZonalTable.FindField("MEAN")
        RangeIndex = ZonalTable.FindField("RANGE")
        SumIndex = ZonalTable.FindField("SUM")
        STDIndex = ZonalTable.FindField("STD")
        AreaIndex = ZonalTable.FindField("AREA")
        CountIndex = ZonalTable.FindField("COUNT")
        ValueIndex = ZonalTable.FindField("VALUE")

        Dim RasterValueCount As Long = ZonalTable.RowCount(Nothing)

        'Get Sum of count and QueryCount, count total number of cells in raster
        Dim pReclassRow As IRow
        Dim SumOfCount As Long
        SumOfCount = 0
        For i = 1 To RasterValueCount
            pReclassRow = ZonalTable.GetRow(i)
            SumOfCount = SumOfCount + pReclassRow.Value(CountIndex)
        Next
        pReclassRow = Nothing

        '=============================================
        'Create Field Titles
        '=============================================
        pworksheet.Cells(1, 1) = "VALUE"
        pworksheet.Cells(1, 2) = "COUNT"
        pworksheet.Cells(1, 3) = "AREA"
        pworksheet.Cells(1, 4) = "MIN"
        pworksheet.Cells(1, 5) = "MAX"
        pworksheet.Cells(1, 6) = "RANGE"
        pworksheet.Cells(1, 7) = "MEAN"
        pworksheet.Cells(1, 8) = "STD"
        pworksheet.Cells(1, 9) = "SUM"
        pworksheet.Cells(1, 10) = "%_AREA"
        pworksheet.Cells(1, 11) = "%_AREA_ELV"
        pworksheet.Cells(1, 12) = "LABEL"

        '====================================================
        'Elevation Titles
        '====================================================

        '============================================
        'Populate Elevation and Percent Area Rows
        '============================================
        Dim pZonalRow As IRow
        Dim PercentArea As Double
        PercentArea = 0
        For i = 0 To RasterValueCount - 1
            'Target Rows
            pZonalRow = ZonalTable.GetRow(i + 1)
            'Determine PercentArea
            PercentArea = PercentArea + ((pZonalRow.Value(CountIndex) / SumOfCount) * 100)
            'Populate Excel Table
            Debug.Print(pZonalRow.Value(ValueIndex))
            Debug.Print(IntervalList(pZonalRow.Value(ValueIndex)).UpperBound)
            pworksheet.Cells(i + 3, 1) = IntervalList(pZonalRow.Value(ValueIndex)).UpperBound * conversionFactor      'Value
            pworksheet.Cells(i + 3, 2) = pZonalRow.Value(CountIndex)                                'Count
            pworksheet.Cells(i + 3, 3) = pZonalRow.Value(AreaIndex)                                 'Area
            pworksheet.Cells(i + 3, 4) = pZonalRow.Value(MinIndex)                                  'MIN
            pworksheet.Cells(i + 3, 5) = pZonalRow.Value(MaxIndex)                                  'MAX
            pworksheet.Cells(i + 3, 6) = pZonalRow.Value(RangeIndex)                                'Range
            pworksheet.Cells(i + 3, 7) = pZonalRow.Value(MeanIndex)                                 'Mean
            pworksheet.Cells(i + 3, 8) = pZonalRow.Value(STDIndex)                                  'STD
            pworksheet.Cells(i + 3, 9) = pZonalRow.Value(SumIndex)                                  'Sum
            pworksheet.Cells(i + 3, 10) = (pZonalRow.Value(CountIndex) / SumOfCount) * 100          'PERCENT_AREA
            pworksheet.Cells(i + 3, 11) = PercentArea                                               'PERCENT_AREA_ELEVATION
            pworksheet.Cells(i + 3, 12) = IntervalList(pZonalRow.Value(ValueIndex)).Name            'Label
        Next

        'AOI_DEMMin is always in meters
        'Make First Elevation Interval the Min of AOI DEM
        pworksheet.Cells(2, 1) = aoiDemMin * BA_SetConversionFactor(optZMetersChecked, True)   'Value
        pworksheet.Cells(2, 11) = 0 'PERCENT_AREA_ELEVATION

        pZonalRow = Nothing
        ZonalTable = Nothing

        Return 1
    End Function

    Public Function BA_Excel_CreateSubElevationTable(ByVal aoiPath As String, ByRef pworksheet As Worksheet, ByVal conversionFactor As Double, ByVal aoiDemMin As Double, _
                                                     ByVal optZMetersChecked As Boolean) As Integer

        If Len(aoiPath) <= 0 Then
            MsgBox("Please select an AOI!")
            Return 0
        End If

        Dim ZRasterName As String, VRasterName As String
        Dim ZInputPath As String, VInputPath As String
        Dim i As Integer
        Dim IntervalList() As BA_IntervalList = Nothing

        ZRasterName = BA_EnumDescription(MapsFileName.SubElevationZone)
        ZInputPath = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Analysis)
        'read class definition for chart and table labelling
        Dim succcess As BA_ReturnCode = BA_ReadReclassRasterAttributeGDB(IntervalList, ZInputPath, ZRasterName)
        VRasterName = BA_EnumDescription(MapsFileName.filled_dem_gdb)
        VInputPath = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Surfaces)


        Dim pZoneRaster As IGeoDataset = BA_OpenRasterFromGDB(ZInputPath, ZRasterName)
        Dim pValueRaster As IGeoDataset = BA_OpenRasterFromGDB(VInputPath, VRasterName)

        'Use the AOI extent for analysis
        Dim pAOIRaster As IGeoDataset
        'Open AOI Polygon to set the analysis mask
        Dim aoiGdbPath As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Aoi)
        Dim aoiFileName As String = BA_GetBareName(BA_EnumDescription(PublicPath.AoiGrid))
        pAOIRaster = BA_OpenRasterFromGDB(aoiGdbPath, aoiFileName)

        If pAOIRaster Is Nothing Then
            MsgBox("Cannot locate AOI boundary raster in the AOI.  Please re-run AOI Tool.")
            Return 0
        End If

        Dim pTempRaster As IGeoDataset
        Dim pExtractOp As IExtractionOp2 = New RasterExtractionOp
        pTempRaster = pExtractOp.Raster(pZoneRaster, pAOIRaster)
        pExtractOp = Nothing
        pZoneRaster = Nothing
        pAOIRaster = Nothing

        '===========================
        'Zonal Statistics
        '===========================
        Dim ZonalOp As IZonalOp = New RasterZonalOp
        Dim ZonalTable As ITable
        ZonalTable = ZonalOp.ZonalStatisticsAsTable(pTempRaster, pValueRaster, True)
        ZonalOp = Nothing
        pZoneRaster = Nothing
        pValueRaster = Nothing
        pTempRaster = Nothing

        'Create Field Indexes
        Dim MinIndex As Integer, MaxIndex As Integer, MeanIndex As Integer
        Dim RangeIndex As Integer, SumIndex As Integer, STDIndex As Integer
        Dim AreaIndex As Integer, CountIndex As Integer, ValueIndex As Integer

        MinIndex = ZonalTable.FindField("MIN")
        MaxIndex = ZonalTable.FindField("MAX")
        MeanIndex = ZonalTable.FindField("MEAN")
        RangeIndex = ZonalTable.FindField("RANGE")
        SumIndex = ZonalTable.FindField("SUM")
        STDIndex = ZonalTable.FindField("STD")
        AreaIndex = ZonalTable.FindField("AREA")
        CountIndex = ZonalTable.FindField("COUNT")
        ValueIndex = ZonalTable.FindField("VALUE")

        Dim RasterValueCount As Long
        RasterValueCount = ZonalTable.RowCount(Nothing)

        'Get Sum of count and QueryCount, count total number of cells in raster
        Dim pReclassRow As IRow
        Dim SumOfCount As Long
        SumOfCount = 0
        For i = 1 To RasterValueCount
            pReclassRow = ZonalTable.GetRow(i)
            SumOfCount = SumOfCount + pReclassRow.Value(CountIndex)
        Next
        pReclassRow = Nothing

        '=============================================
        'Create Field Titles
        '=============================================
        pworksheet.Cells(1, 1) = "VALUE"
        pworksheet.Cells(1, 2) = "COUNT"
        pworksheet.Cells(1, 3) = "AREA"
        pworksheet.Cells(1, 4) = "MIN"
        pworksheet.Cells(1, 5) = "MAX"
        pworksheet.Cells(1, 6) = "RANGE"
        pworksheet.Cells(1, 7) = "MEAN"
        pworksheet.Cells(1, 8) = "STD"
        pworksheet.Cells(1, 9) = "SUM"
        pworksheet.Cells(1, 10) = "%_AREA"
        pworksheet.Cells(1, 11) = "%_AREA_ELV"
        pworksheet.Cells(1, 12) = "LABEL"

        '============================================
        'Populate Elevation and Percent Area Rows
        '============================================
        Dim pZonalRow As IRow
        Dim PercentArea As Double
        PercentArea = 0
        For i = 0 To RasterValueCount - 1
            'Target Rows
            pZonalRow = ZonalTable.GetRow(i + 1)
            'Determine PercentArea
            PercentArea = PercentArea + ((pZonalRow.Value(CountIndex) / SumOfCount) * 100)
            'Populate Excel Table
            pworksheet.Cells(i + 3, 1) = IntervalList(pZonalRow.Value(ValueIndex)).UpperBound * conversionFactor      'Value
            pworksheet.Cells(i + 3, 2) = pZonalRow.Value(CountIndex)                                'Count
            pworksheet.Cells(i + 3, 3) = pZonalRow.Value(AreaIndex)                                 'Area
            pworksheet.Cells(i + 3, 4) = pZonalRow.Value(MinIndex)                                  'MIN
            pworksheet.Cells(i + 3, 5) = pZonalRow.Value(MaxIndex)                                  'MAX
            pworksheet.Cells(i + 3, 6) = pZonalRow.Value(RangeIndex)                                'Range
            pworksheet.Cells(i + 3, 7) = pZonalRow.Value(MeanIndex)                                 'Mean
            pworksheet.Cells(i + 3, 8) = pZonalRow.Value(STDIndex)                                  'STD
            pworksheet.Cells(i + 3, 9) = pZonalRow.Value(SumIndex)                                  'Sum
            pworksheet.Cells(i + 3, 10) = (pZonalRow.Value(CountIndex) / SumOfCount) * 100          'PERCENT_AREA
            pworksheet.Cells(i + 3, 11) = PercentArea                                               'PERCENT_AREA_ELEVATION
            pworksheet.Cells(i + 3, 12) = IntervalList(pZonalRow.Value(ValueIndex)).Name            'Label
        Next

        'AOI_DEMMin is always in meters
        'Make First Elevation Interval the Min of AOI DEM
        pworksheet.Cells(2, 1) = aoiDemMin * BA_SetConversionFactor(optZMetersChecked, True)   'Value
        pworksheet.Cells(2, 11) = 0 'PERCENT_AREA_ELEVATION

        pZonalRow = Nothing
        ZonalTable = Nothing

        Return 1
    End Function

    Public Function BA_Excel_CreateElevationChart(ByRef pworksheet As Worksheet, ByRef pChartsWorksheet As Worksheet, _
                                                  ByVal Position_Left As Double, ByVal Position_Top As Double, ByVal Y_Min As Double, _
                                                  ByVal Y_Max As Double, ByVal Y_Unit As Double, ByVal optZMeters As Boolean, _
                                                  ByVal optZFeet As Boolean) As Integer
        Dim nrecords As Long, LastRow As Long
        Dim returnvalue As Integer
        returnvalue = -1

        nrecords = BA_Excel_CountRecords(pworksheet, 2)
        LastRow = nrecords + 2

        '======================================================================================================
        'Make Chart
        '======================================================================================================
        Dim myChart As Chart
        myChart = pChartsWorksheet.Shapes.AddChart.Chart

        'Determine Z Mapping Unit and Create Value Axis Title
        Dim AxisTitleUnit As String = Nothing
        If optZMeters = True Then
            AxisTitleUnit = " (Meters)"
        ElseIf optZFeet = True Then
            AxisTitleUnit = " (Feet)"
        End If

        'Set Range Cells
        Dim ValueRange As String
        Dim xValueRange As String
        Dim sValueRange As String
        Dim maxvalue As String
        Dim minvalue As String
        sValueRange = "A2:A" & LastRow & "," & "K2:K" & LastRow
        xValueRange = "A2:A" & LastRow
        ValueRange = "K2:K" & LastRow
        maxvalue = "A" & LastRow
        minvalue = "A2"

        With myChart
            'Clear Style
            .ClearToMatchStyle()
            'Create and Set Title
            .HasTitle = True
            .ChartTitle.Caption = "Area-Elevation Distribution"
            'Set Chart Type and Value Range
            .ChartType = Excel.XlChartType.xlXYScatterLines
            .SetSourceData(pworksheet.Range(sValueRange))
            'Set Axis Parameters
            Dim categoryAxis As Excel.Axis = CType(.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary), Excel.Axis)
            categoryAxis.HasTitle = True
            categoryAxis.AxisTitle.Characters.Text = "% AOI Area below Elevation"
            categoryAxis.MaximumScale = "100.1"
            categoryAxis.MinimumScale = "0"
            Dim valueAxis As Excel.Axis = CType(.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary), Excel.Axis)
            valueAxis.HasTitle = True
            valueAxis.AxisTitle.Characters.Text = "Elevation" & AxisTitleUnit
            valueAxis.MaximumScale = Y_Max
            valueAxis.MinimumScale = Y_Min
            valueAxis.MajorUnit = Y_Unit

            'Set Element Locations
            .SetElement(MsoChartElementType.msoElementChartTitleAboveChart)
            .SetElement(MsoChartElementType.msoElementLegendNone)
            'Set Chart Location and Size
            .Parent.Left = Position_Left
            .Parent.Width = BA_ChartWidth
            .Parent.Top = Position_Top
            .Parent.Height = BA_ChartHeight
        End With

        'Clear Chart Series
        With myChart
            Do Until .SeriesCollection.Count = 0
                .SeriesCollection(1).Delete()
            Loop
        End With

        'Create Elevation Series
        Dim Elevation As Series
        Elevation = myChart.SeriesCollection.NewSeries
        With Elevation
            .Name = "Elevation"
            'Set Values
            .Values = pworksheet.Range(xValueRange)
            .XValues = pworksheet.Range(ValueRange)
            .Smooth = False
            .MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleNone
        End With

        'Clear Chart and Return Value
        myChart = Nothing
        Return 1
    End Function

    'calculate zonal stats using SNOTEL or Snow Course Elevation zones and DEM raster
    Public Function BA_Excel_CreateSNOTELTable(ByVal aoiPath As String, ByRef pworksheet As Worksheet, ByRef pElevWorkSheet As Worksheet, _
                                               ByVal ZoneRasterName As String, ByVal conversionFactor As Double) As Integer

        Dim nrecords As Long
        Dim ElevReturn As Long

        nrecords = BA_Excel_CountRecords(pElevWorkSheet, 2)
        ElevReturn = nrecords + 2

        Dim VRasterName As String
        Dim ZInputPath As String, VInputPath As String
        Dim i As Integer
        Dim IntervalList() As BA_IntervalList = Nothing

        ZInputPath = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Analysis)

        'read class definition for chart and table labelling
        Dim success As BA_ReturnCode = BA_ReadReclassRasterAttributeGDB(IntervalList, ZInputPath, ZoneRasterName)
        VRasterName = BA_EnumDescription(MapsFileName.filled_dem_gdb)
        VInputPath = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Surfaces)

        Dim pZoneRaster As IGeoDataset = BA_OpenRasterFromGDB(ZInputPath, ZoneRasterName)
        Dim pValueRaster As IGeoDataset = BA_OpenRasterFromGDB(VInputPath, VRasterName)

        'Use the AOI extent for analysis
        Dim pAOIRaster As IGeoDataset = BA_OpenRasterFromGDB(BA_GeodatabasePath(aoiPath, GeodatabaseNames.Aoi), BA_GetBareName(BA_EnumDescription(PublicPath.AoiGrid)))
        If pAOIRaster Is Nothing Then
            MsgBox("Cannot locate AOI boundary raster in the AOI.  Please re-run AOI Tool.")
            Return 0
        End If

        Dim pTempRaster As IGeoDataset
        Dim pExtractOp As IExtractionOp2 = New RasterExtractionOp
        pTempRaster = pExtractOp.Raster(pZoneRaster, pAOIRaster)
        pExtractOp = Nothing
        pZoneRaster = Nothing
        pAOIRaster = Nothing

        '===========================
        'Zonal Statistics
        '===========================
        Dim ZonalOp As IZonalOp = New RasterZonalOp
        Dim ZonalTable As ITable = ZonalOp.ZonalStatisticsAsTable(pTempRaster, pValueRaster, True)
        ZonalOp = Nothing
        pTempRaster = Nothing
        pValueRaster = Nothing

        'Create Field Indexes
        Dim MinIndex As Integer, MaxIndex As Integer, MeanIndex As Integer
        Dim RangeIndex As Integer, SumIndex As Integer, STDIndex As Integer
        Dim AreaIndex As Integer, CountIndex As Integer, ValueIndex As Integer

        MinIndex = ZonalTable.FindField("MIN")
        MaxIndex = ZonalTable.FindField("MAX")
        MeanIndex = ZonalTable.FindField("MEAN")
        RangeIndex = ZonalTable.FindField("RANGE")
        SumIndex = ZonalTable.FindField("SUM")
        STDIndex = ZonalTable.FindField("STD")
        AreaIndex = ZonalTable.FindField("AREA")
        CountIndex = ZonalTable.FindField("COUNT")
        ValueIndex = ZonalTable.FindField("VALUE")

        Dim RasterValueCount As Long = ZonalTable.RowCount(Nothing)

        'Get Sum of count and QueryCount, count total number of cells in raster
        Dim pReclassRow As IRow
        Dim SumOfCount As Long
        SumOfCount = 0
        For i = 1 To RasterValueCount
            pReclassRow = ZonalTable.GetRow(i)
            SumOfCount = SumOfCount + pReclassRow.Value(CountIndex)
        Next
        pReclassRow = Nothing

        '===================================
        'Create Field Titles
        '===================================
        pworksheet.Cells(1, 1) = "VALUE"
        pworksheet.Cells(1, 2) = "COUNT"
        pworksheet.Cells(1, 3) = "AREA"
        pworksheet.Cells(1, 4) = "MIN"
        pworksheet.Cells(1, 5) = "MAX"
        pworksheet.Cells(1, 6) = "RANGE"
        pworksheet.Cells(1, 7) = "MEAN"
        pworksheet.Cells(1, 8) = "STD"
        pworksheet.Cells(1, 9) = "SUM"
        pworksheet.Cells(1, 10) = "%_AREA"
        pworksheet.Cells(1, 11) = "%_AREA_ELV"
        pworksheet.Cells(1, 12) = "Label"

        '==============================================
        'Populate Elevation and Percent Area Rows
        '==============================================
        Dim pZonalRow As IRow
        Dim PercentArea As Double
        PercentArea = 0
        'Create Feature Cursor and Feature
        For i = 1 To RasterValueCount
            'Target Rows
            pZonalRow = ZonalTable.GetRow(i)
            'Determine PercentArea
            PercentArea = PercentArea + ((pZonalRow.Value(CountIndex) / SumOfCount) * 100)
            'Populate Excel Table
            pworksheet.Cells(i + 1, 1) = IntervalList(pZonalRow.Value(ValueIndex)).UpperBound * conversionFactor 'Value
            pworksheet.Cells(i + 1, 2) = pZonalRow.Value(CountIndex)                                    'Count
            pworksheet.Cells(i + 1, 3) = pZonalRow.Value(AreaIndex)                                     'Area
            pworksheet.Cells(i + 1, 4) = pZonalRow.Value(MinIndex)                                      'MIN
            pworksheet.Cells(i + 1, 5) = pZonalRow.Value(MaxIndex)                                      'MAX
            pworksheet.Cells(i + 1, 6) = pZonalRow.Value(RangeIndex)                                    'Range
            pworksheet.Cells(i + 1, 7) = pZonalRow.Value(MeanIndex)                                     'Mean
            pworksheet.Cells(i + 1, 8) = pZonalRow.Value(STDIndex)                                      'STD
            pworksheet.Cells(i + 1, 9) = pZonalRow.Value(SumIndex)                                      'Sum
            pworksheet.Cells(i + 1, 10) = (pZonalRow.Value(CountIndex) / SumOfCount) * 100        'PERCENT_AREA
            pworksheet.Cells(i + 1, 11) = PercentArea                                           'PERCENT_AREA_ELVATION
            pworksheet.Cells(i + 1, 12) = IntervalList(pZonalRow.Value(ValueIndex)).Name                 'label
        Next

        pZonalRow = Nothing
        ZonalTable = Nothing

        Return 1
    End Function

    '@ToDo: fix this function
    Public Function BA_Excel_CreateSNOTELChart(ByRef pSiteWorkSheet As Worksheet, ByRef pElevWorkSheet As Worksheet, ByRef pChartsWorksheet As Worksheet, _
                                            ByVal Is_SNOTEL As Boolean, ByVal Position_Left As Double, ByVal Position_Top As Double, ByVal Y_Min As Double, _
                                            ByVal Y_Max As Double, ByVal Y_Unit As Double, ByVal optZMeters As Boolean, _
                                            ByVal optZFeet As Boolean) As Integer
        Dim nrecords As Long, SiteLastRow As Long, ElevLastRow As Long
        Dim returnvalue As Integer
        returnvalue = -1

        nrecords = BA_Excel_CountRecords(pSiteWorkSheet, 1)
        SiteLastRow = nrecords - 1 'last point not represented

        nrecords = BA_Excel_CountRecords(pElevWorkSheet, 2)
        ElevLastRow = nrecords + 2

        Dim ChartTitle As String
        Dim SeriesName As String
        Dim MarkerColor As Long

        If Is_SNOTEL Then
            ChartTitle = "Area-Elevation and SNOTEL Distribution"
            SeriesName = "SNOTEL"
            MarkerColor = RGB(0, 0, 0)
        Else
            ChartTitle = "Area-Elevation and Snow Course Distribution"
            SeriesName = "Snow Course"
            MarkerColor = RGB(246, 32, 10)
        End If

        Dim myChart As Chart = pChartsWorksheet.Shapes.AddChart.Chart

        'Determine Z Mapping Unit and Create Value Axis Title
        Dim AxisTitleUnit As String = Nothing
        If optZMeters = True Then
            AxisTitleUnit = " (Meters)"
        ElseIf optZFeet = True Then
            AxisTitleUnit = " (Feet)"
        End If

        'Set SNOTEL/Snow Course Ranges
        Dim ValueRange As String
        Dim xValueRange As String
        Dim sValueRange As String
        sValueRange = "A2:A" & SiteLastRow & "," & "K2:K" & SiteLastRow
        xValueRange = "A2:A" & SiteLastRow 'last point not represented
        ValueRange = "K2:K" & SiteLastRow  'last point not represented

        'Set Elevation Ranges
        Dim sElevRange As String
        Dim sElevValueRange As String
        sElevRange = "K2:K" & ElevLastRow
        sElevValueRange = "A2:A" & ElevLastRow

        With myChart
            'Clear Styles
            .ClearToMatchStyle()
            'Insert Title and Legend
            .HasTitle = True
            .HasLegend = True
            .ChartTitle.Caption = ChartTitle
            'Set Chart Type and Data Source
            .ChartType = Excel.XlChartType.xlXYScatter
            .SetSourceData(pElevWorkSheet.Range(sValueRange))
            'Set x Axis Parameters
            Dim categoryAxis As Excel.Axis = CType(.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary), Excel.Axis)
            categoryAxis.HasTitle = True
            categoryAxis.AxisTitle.Characters.Text = "% AOI Area below Elevation"
            categoryAxis.MaximumScale = "100.1"
            categoryAxis.MinimumScale = "0"
            Dim valueAxis As Excel.Axis = CType(.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary), Excel.Axis)
            valueAxis.HasTitle = True
            valueAxis.AxisTitle.Characters.Text = "Elevation" & AxisTitleUnit
            valueAxis.MaximumScale = Y_Max
            valueAxis.MinimumScale = Y_Min
            valueAxis.MajorUnit = Y_Unit

            'Set Element Parameters
            .SetElement(MsoChartElementType.msoElementChartTitleAboveChart)
            .SetElement(MsoChartElementType.msoElementLegendBottom)
            'Set Position and Chart Dimensions
            .Parent.Left = Position_Left
            .Parent.Width = BA_ChartWidth
            .Parent.Top = Position_Top
            .Parent.Height = BA_ChartHeight
        End With

        'Clear All Series
        With myChart
            Do Until .SeriesCollection.Count = 0
                .SeriesCollection(1).Delete()
            Loop
        End With

        'Snow Course Series as Dots (No Lines)
        Dim scSeries As Series
        scSeries = myChart.SeriesCollection.NewSeries
        With scSeries
            .Name = SeriesName
            'Set Series Values
            .Values = pSiteWorkSheet.Range(xValueRange)
            .XValues = pSiteWorkSheet.Range(ValueRange)
            .MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleTriangle
            'Set Marker Colors
            .MarkerForegroundColor = MarkerColor
            .MarkerBackgroundColor = MarkerColor
        End With

        'Elevation Series as Lines (No Dots)
        Dim Elevation As Series = myChart.SeriesCollection.NewSeries
        With Elevation
            .Name = "Elevation"
            'XY Values
            .Values = pElevWorkSheet.Range(sElevValueRange)
            .XValues = pElevWorkSheet.Range(sElevRange)
            'Line Formats
            .Smooth = False
            .Format.Line.DashStyle = MsoLineDashStyle.msoLineSolid
            .Format.Line.ForeColor.RGB = RGB(74, 126, 187)
            .MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleNone
            .Format.Line.BeginArrowheadWidth = MsoArrowheadWidth.msoArrowheadNarrow
            .Format.Line.EndArrowheadWidth = MsoArrowheadWidth.msoArrowheadNarrow
            'Makes sure its plotted behind Snow Course Markers
            .PlotOrder = 1
        End With

        'Clear Memory and Return Value
        myChart = Nothing
        Return 1
    End Function

    'calculate zonal stats using aspect zones (16 directions plus flat) and aspect raster
    Public Function BA_Excel_CreateAspectTable(ByVal aoiPath As String, ByRef pworksheet As Worksheet) As Integer

        Dim ZRasterName As String, VRasterName As String
        Dim ZInputPath As String, VInputPath As String
        Dim i As Integer
        Dim IntervalList() As BA_IntervalList = Nothing

        ZRasterName = BA_EnumDescription(MapsFileName.AspectZone)
        ZInputPath = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Analysis)
        'read class definition for chart and table labelling
        Dim success As BA_ReturnCode = BA_ReadReclassRasterAttributeGDB(IntervalList, ZInputPath, ZRasterName)
        VRasterName = BA_GetBareName(BA_EnumDescription(PublicPath.Aspect))
        VInputPath = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Surfaces)

        Dim pZoneRaster As IGeoDataset = BA_OpenRasterFromGDB(ZInputPath, ZRasterName)
        Dim pValueRaster As IGeoDataset = BA_OpenRasterFromGDB(VInputPath, VRasterName)

        'Use the AOI extent for analysis
        Dim pAOIRaster As IGeoDataset = BA_OpenRasterFromGDB(BA_GeodatabasePath(aoiPath, GeodatabaseNames.Aoi), BA_GetBareName(BA_EnumDescription(PublicPath.AoiGrid)))
        If pAOIRaster Is Nothing Then
            MsgBox("Cannot locate AOI boundary raster in the AOI.  Please re-run AOI Tool.")
            Return 0
        End If

        Dim pExtractOp As IExtractionOp2 = New RasterExtractionOp
        Dim pTempRaster As IGeoDataset = pExtractOp.Raster(pZoneRaster, pAOIRaster)
        pExtractOp = Nothing
        pZoneRaster = Nothing
        pAOIRaster = Nothing

        '===========================
        'Zonal Statistics
        '===========================
        Dim ZonalOp As IZonalOp = New RasterZonalOp
        Dim ZonalTable As ITable = ZonalOp.ZonalStatisticsAsTable(pTempRaster, pValueRaster, True)
        ZonalOp = Nothing
        pTempRaster = Nothing
        pValueRaster = Nothing

        'Create Field Indexes
        Dim MinIndex As Integer, MaxIndex As Integer, MeanIndex As Integer
        Dim RangeIndex As Integer, SumIndex As Integer, STDIndex As Integer
        Dim AreaIndex As Integer, CountIndex As Integer, ValueIndex As Integer

        MinIndex = ZonalTable.FindField("MIN")
        MaxIndex = ZonalTable.FindField("MAX")
        MeanIndex = ZonalTable.FindField("MEAN")
        RangeIndex = ZonalTable.FindField("RANGE")
        SumIndex = ZonalTable.FindField("SUM")
        STDIndex = ZonalTable.FindField("STD")
        AreaIndex = ZonalTable.FindField("AREA")
        CountIndex = ZonalTable.FindField("COUNT")
        ValueIndex = ZonalTable.FindField("VALUE")

        Dim RasterValueCount As Long = ZonalTable.RowCount(Nothing)

        'Get Sum of count and QueryCount, count total number of cells in raster
        Dim pReclassRow As IRow
        Dim SumOfCount As Long
        SumOfCount = 0
        For i = 1 To RasterValueCount
            pReclassRow = ZonalTable.GetRow(i)
            SumOfCount = SumOfCount + pReclassRow.Value(CountIndex)
        Next
        pReclassRow = Nothing

        '============================
        'Create Field Titles
        '============================
        pworksheet.Cells(1, 1) = "DIRECTION"
        pworksheet.Cells(1, 2) = "DIRECTION"
        pworksheet.Cells(1, 3) = "COUNT"
        pworksheet.Cells(1, 4) = "AREA"
        pworksheet.Cells(1, 5) = "MIN"
        pworksheet.Cells(1, 6) = "MAX"
        pworksheet.Cells(1, 7) = "RANGE"
        pworksheet.Cells(1, 8) = "MEAN"
        pworksheet.Cells(1, 9) = "STD"
        pworksheet.Cells(1, 10) = "SUM"
        pworksheet.Cells(1, 11) = "%_AREA"

        '===============================
        'Populate Elevation and Percent Area Rows
        '===============================
        Dim pZonalRow As IRow
        Dim PercentArea As Double
        PercentArea = 0
        'Create Feature Cursor and Feature
        For i = 1 To RasterValueCount
            'Target Rows
            pZonalRow = ZonalTable.GetRow(i)
            'Populate Excel Table
            pworksheet.Cells(i + 1, 1) = pZonalRow.Value(ValueIndex)
            pworksheet.Cells(i + 1, 2) = IntervalList(i).Name                                       'Direction
            pworksheet.Cells(i + 1, 3) = pZonalRow.Value(CountIndex)                                'Count
            pworksheet.Cells(i + 1, 4) = pZonalRow.Value(AreaIndex)                                 'Area
            pworksheet.Cells(i + 1, 5) = pZonalRow.Value(MinIndex)                                  'MIN
            pworksheet.Cells(i + 1, 6) = pZonalRow.Value(MaxIndex)                                  'MAX
            pworksheet.Cells(i + 1, 7) = pZonalRow.Value(RangeIndex)                                'Range
            pworksheet.Cells(i + 1, 8) = pZonalRow.Value(MeanIndex)                                 'Mean
            pworksheet.Cells(i + 1, 9) = pZonalRow.Value(STDIndex)                                  'STD
            pworksheet.Cells(i + 1, 10) = pZonalRow.Value(SumIndex)                                  'Sum
            pworksheet.Cells(i + 1, 11) = (pZonalRow.Value(CountIndex) / SumOfCount) * 100          'PERCENT_AREA
        Next
        ZonalTable = Nothing
        pZonalRow = Nothing
        Return 1
    End Function

    'calculate zonal stats using aspect zones (16 directions plus flat) and aspect raster
    Public Function BA_Excel_CreateAspectChart(ByRef pworksheet As Worksheet, ByRef pChartsWorksheet As Worksheet) As Integer
        Dim nrecords As Long
        nrecords = BA_Excel_CountRecords(pworksheet, 2)

        '===========================
        'Make Chart
        '===========================
        Dim myChart As Chart
        myChart = pChartsWorksheet.Shapes.AddChart.Chart

        'Set Cell Value Ranges
        Dim ValueRange As String
        ValueRange = "B2:B" & nrecords + 1 & "," & "K2:K" & nrecords + 1

        With myChart
            'Clear Chart Styles
            .ClearToMatchStyle() 'not supported in Office 2003
            'Set Chart Title
            .HasTitle = True
            .ChartTitle.Caption = "Aspect Distribution"
            'Set Chart Size and Location
            .Parent.Left = BA_ChartSpacing
            .Parent.Width = BA_ChartWidth
            .Parent.Top = (BA_ChartHeight + BA_ChartSpacing) * 3 + BA_ChartSpacing
            .Parent.Height = BA_ChartHeight
            'Set Chart Type and Range
            .ChartType = Excel.XlChartType.xlColumnClustered
            .SetSourceData(pworksheet.Range(ValueRange))
            'Set Axis Parameters
            Dim categoryAxis As Excel.Axis = CType(.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary), Excel.Axis)
            categoryAxis.HasTitle = True
            categoryAxis.AxisTitle.Characters.Text = "Aspect"
            Dim valueAxis As Excel.Axis = CType(.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary), Excel.Axis)
            valueAxis.HasTitle = True
            valueAxis.AxisTitle.Characters.Text = "% AOI Area"
            valueAxis.MinimumScale = 0
            'Set Element Locations
            .SetElement(MsoChartElementType.msoElementChartTitleAboveChart)
            .SetElement(MsoChartElementType.msoElementLegendNone)
        End With

        'Clear Memory and Return Value
        myChart = Nothing
        Return 1
    End Function

    'calculate zonal stats using slope zones (7 zones) and percent slope raster
    Public Function BA_Excel_CreateSlopeTable(ByVal aoiPath As String, ByRef pworksheet As Worksheet) As Integer

        Dim response As Integer
        Dim ZRasterName As String, VRasterName As String
        Dim ZInputPath As String, VInputPath As String
        Dim i As Integer
        Dim IntervalList() As BA_IntervalList = Nothing

        ZRasterName = BA_EnumDescription(MapsFileName.SlopeZone)
        ZInputPath = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Analysis)
        'read class definition for chart and table labelling
        response = BA_ReadReclassRasterAttributeGDB(IntervalList, ZInputPath, ZRasterName)
        VRasterName = BA_GetBareName(BA_EnumDescription(PublicPath.Slope))
        VInputPath = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Surfaces)

        Dim pZoneRaster As IGeoDataset = BA_OpenRasterFromGDB(ZInputPath, ZRasterName)
        Dim pValueRaster As IGeoDataset = BA_OpenRasterFromGDB(VInputPath, VRasterName)

        'Use the AOI extent for analysis
        Dim pAOIRaster As IGeoDataset = BA_OpenRasterFromGDB(BA_GeodatabasePath(aoiPath, GeodatabaseNames.Aoi), BA_GetBareName(BA_EnumDescription(PublicPath.AoiGrid)))
        If pAOIRaster Is Nothing Then
            MsgBox("Cannot locate AOI boundary raster in the AOI.  Please re-run AOI Tool.")
            Return 0
        End If

        Dim pExtractOp As IExtractionOp2 = New RasterExtractionOp
        Dim pTempRaster As IGeoDataset = pExtractOp.Raster(pZoneRaster, pAOIRaster)
        pExtractOp = Nothing
        pZoneRaster = Nothing
        pAOIRaster = Nothing

        '===========================
        'Zonal Statistics
        '===========================
        Dim ZonalOp As IZonalOp
        Dim ZonalTable As ITable
        ZonalOp = New RasterZonalOp
        ZonalTable = ZonalOp.ZonalStatisticsAsTable(pTempRaster, pValueRaster, True)
        ZonalOp = Nothing
        pTempRaster = Nothing
        pValueRaster = Nothing

        'Create Field Indexes
        Dim MinIndex As Integer, MaxIndex As Integer, MeanIndex As Integer
        Dim RangeIndex As Integer, SumIndex As Integer, STDIndex As Integer
        Dim AreaIndex As Integer, CountIndex As Integer, ValueIndex As Integer

        MinIndex = ZonalTable.FindField("MIN")
        MaxIndex = ZonalTable.FindField("MAX")
        MeanIndex = ZonalTable.FindField("MEAN")
        RangeIndex = ZonalTable.FindField("RANGE")
        SumIndex = ZonalTable.FindField("SUM")
        STDIndex = ZonalTable.FindField("STD")
        AreaIndex = ZonalTable.FindField("AREA")
        CountIndex = ZonalTable.FindField("COUNT")
        ValueIndex = ZonalTable.FindField("VALUE")

        Dim RasterValueCount As Long = ZonalTable.RowCount(Nothing)

        'Get Sum of count and QueryCount, count total number of cells in raster
        Dim pReclassRow As IRow
        Dim SumOfCount As Long
        SumOfCount = 0
        For i = 1 To RasterValueCount
            pReclassRow = ZonalTable.GetRow(i)
            SumOfCount = SumOfCount + pReclassRow.Value(CountIndex)
        Next
        pReclassRow = Nothing

        '====================================
        'Create Field Titles
        '====================================
        pworksheet.Cells(1, 1) = "SLOPE"
        pworksheet.Cells(1, 2) = "COUNT"
        pworksheet.Cells(1, 3) = "AREA"
        pworksheet.Cells(1, 4) = "MIN"
        pworksheet.Cells(1, 5) = "MAX"
        pworksheet.Cells(1, 6) = "RANGE"
        pworksheet.Cells(1, 7) = "MEAN"
        pworksheet.Cells(1, 8) = "STD"
        pworksheet.Cells(1, 9) = "SUM"
        pworksheet.Cells(1, 10) = "%_AREA"

        '===========================================
        'Populate Elevation and Percent Area Rows
        '===========================================
        Dim pZonalRow As IRow
        Dim PercentArea As Double = 0
        For i = 1 To RasterValueCount
            'Target Rows
            pZonalRow = ZonalTable.GetRow(i)
            'Populate Excel Table
            pworksheet.Cells(i + 1, 1) = IntervalList(pZonalRow.Value(ValueIndex)).Name             'Slope
            pworksheet.Cells(i + 1, 2) = pZonalRow.Value(CountIndex)                                'Count
            pworksheet.Cells(i + 1, 3) = pZonalRow.Value(AreaIndex)                                 'Area
            pworksheet.Cells(i + 1, 4) = pZonalRow.Value(MinIndex)                                  'MIN
            pworksheet.Cells(i + 1, 5) = pZonalRow.Value(MaxIndex)                                  'MAX
            pworksheet.Cells(i + 1, 6) = pZonalRow.Value(RangeIndex)                                'Range
            pworksheet.Cells(i + 1, 7) = pZonalRow.Value(MeanIndex)                                 'Mean
            pworksheet.Cells(i + 1, 8) = pZonalRow.Value(STDIndex)                                  'STD
            pworksheet.Cells(i + 1, 9) = pZonalRow.Value(SumIndex)                                  'Sum
            pworksheet.Cells(i + 1, 10) = (pZonalRow.Value(CountIndex) / SumOfCount) * 100          'PERCENT_AREA
        Next

        pZonalRow = Nothing
        ZonalTable = Nothing
        Return 1
    End Function

    'calculate zonal stats using slope zones and percent slope raster
    Public Function BA_Excel_CreateSlopeChart(ByRef pworksheet As Worksheet, ByRef pChartsWorksheet As Worksheet) As Integer
        Dim nrecords As Long = BA_Excel_CountRecords(pworksheet, 2)

        '===========================
        'Make Chart
        '===========================
        Dim myChart As Chart
        myChart = pChartsWorksheet.Shapes.AddChart.Chart
        'Set myChart = pChartsWorksheet.ChartObjects.Add(0, 330, BA_ChartWidth, BA_ChartHeight) 'pre-2007

        Dim ValueRange As String
        ValueRange = "A2:A" & nrecords + 1 & "," & "J2:J" & nrecords + 1

        With myChart
            'Clear Styles
            .ClearToMatchStyle()
            'Set Title
            .HasTitle = True
            .ChartTitle.Caption = "Slope Distribution"
            'Set Position and Location
            .Parent.Left = BA_ChartSpacing
            .Parent.Width = BA_ChartWidth
            .Parent.Top = (BA_ChartHeight + BA_ChartSpacing) * 2 + BA_ChartSpacing
            .Parent.Height = BA_ChartHeight
            'Set Chart Type and Value Range
            .ChartType = Excel.XlChartType.xlColumnClustered
            .SetSourceData(pworksheet.Range(ValueRange))
            'Set Axis Properties
            Dim categoryAxis As Excel.Axis = CType(.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary), Excel.Axis)
            Dim valueAxis As Excel.Axis = CType(.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary), Excel.Axis)
            categoryAxis.HasTitle = True
            categoryAxis.AxisTitle.Characters.Text = "Percent Slope"
            'For some reason, setting the following 2 properties causes some of the charts not to display
            'Seems to work okay without them - LCB
            'categoryAxis.MaximumScale = 100.1
            'categoryAxis.MinimumScale = 0
            valueAxis.HasTitle = True
            valueAxis.AxisTitle.Characters.Text = "% AOI Area"
            valueAxis.MinimumScale = 0
            'Set Element Positions
            .SetElement(MsoChartElementType.msoElementChartTitleAboveChart)
            .SetElement(MsoChartElementType.msoElementLegendNone)
        End With
        myChart = Nothing
        Return 1
    End Function

    'summarize PRISM precipitation by elevation zones
    Public Function BA_Excel_CreatePRISMTable(ByVal aoiPath As String, ByRef pworksheet As Worksheet, ByRef pElevWorkSheet As Worksheet, _
                                              ByRef MaxPRISMValue As Double, ByVal prismPath As String, ByVal aoiDemMin As Double, _
                                              ByVal conversionFactor As Double, ByVal optZValue As Boolean) As Integer

        Dim ElevReturn As Long
        Dim nrecords As Long = BA_Excel_CountRecords(pElevWorkSheet, 2)
        ElevReturn = nrecords + 2

        Dim response As Integer
        Dim ZRasterName As String, VRasterName As String
        Dim ZInputPath As String, VInputPath As String
        Dim i As Integer
        Dim IntervalList() As BA_IntervalList = Nothing

        ZRasterName = BA_EnumDescription(MapsFileName.ElevationZone)
        ZInputPath = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Analysis)
        'read class definition for chart and table labelling
        response = BA_ReadReclassRasterAttributeGDB(IntervalList, ZInputPath, ZRasterName)
        'prism data location must be passed from the form
        VInputPath = "PleaseReturn"
        VRasterName = BA_GetBareName(prismPath, VInputPath)

        Dim pZoneRaster As IGeoDataset = BA_OpenRasterFromGDB(ZInputPath, ZRasterName)
        Dim pValueRaster As IGeoDataset = BA_OpenRasterFromGDB(VInputPath, VRasterName)

        'Use the AOI extent for analysis
        Dim pAOIRaster As IGeoDataset = BA_OpenRasterFromGDB(BA_GeodatabasePath(aoiPath, GeodatabaseNames.Aoi), BA_GetBareName(BA_EnumDescription(PublicPath.AoiGrid)))
        If pAOIRaster Is Nothing Then
            MsgBox("Cannot locate AOI boundary raster in the AOI.  Please re-run AOI Tool.")
            Return 0
        End If

        Dim pExtractOp As IExtractionOp2 = New RasterExtractionOp
        Dim pTempRaster As IGeoDataset = pExtractOp.Raster(pZoneRaster, pAOIRaster)
        pExtractOp = Nothing
        pZoneRaster = Nothing
        pAOIRaster = Nothing

        '===========================
        'Zonal Statistics
        '===========================
        Dim pZonalOpEnv As IRasterAnalysisEnvironment
        Dim ZonalTable As ITable
        Dim ZonalOp As IZonalOp = New RasterZonalOp
        pZonalOpEnv = ZonalOp 'QI
        pZonalOpEnv.SetCellSize(esriRasterEnvSettingEnum.esriRasterEnvMinOf)    'set the analysis cellzie to prevent missing counts in the zonal stats
        ZonalTable = ZonalOp.ZonalStatisticsAsTable(pTempRaster, pValueRaster, True)
        ZonalOp = Nothing
        pZoneRaster = Nothing
        pValueRaster = Nothing
        pTempRaster = Nothing

        'Create Field Indexes
        Dim MinIndex As Integer, MaxIndex As Integer, MeanIndex As Integer
        Dim RangeIndex As Integer, SumIndex As Integer, STDIndex As Integer
        Dim AreaIndex As Integer, CountIndex As Integer, ValueIndex As Integer

        MinIndex = ZonalTable.FindField("MIN")
        MaxIndex = ZonalTable.FindField("MAX")
        MeanIndex = ZonalTable.FindField("MEAN")
        RangeIndex = ZonalTable.FindField("RANGE")
        SumIndex = ZonalTable.FindField("SUM")
        STDIndex = ZonalTable.FindField("STD")
        AreaIndex = ZonalTable.FindField("AREA")
        CountIndex = ZonalTable.FindField("COUNT")
        ValueIndex = ZonalTable.FindField("VALUE")

        Dim RasterValueCount As Long
        RasterValueCount = ZonalTable.RowCount(Nothing)

        'Get Sum of count and QueryCount, count total number of cells in raster
        Dim pReclassRow As IRow
        Dim SumOfCount As Long
        SumOfCount = 0
        For i = 1 To RasterValueCount
            pReclassRow = ZonalTable.GetRow(i)
            SumOfCount = SumOfCount + pReclassRow.Value(CountIndex)
        Next
        pReclassRow = Nothing

        '============================================
        'Create Field Titles
        '============================================
        pworksheet.Cells(1, 1) = "VALUE"
        pworksheet.Cells(1, 2) = "COUNT"
        pworksheet.Cells(1, 3) = "AREA"
        pworksheet.Cells(1, 4) = "MIN"
        pworksheet.Cells(1, 5) = "MAX"
        pworksheet.Cells(1, 6) = "RANGE"
        pworksheet.Cells(1, 7) = "MEAN"
        pworksheet.Cells(1, 8) = "STD"
        pworksheet.Cells(1, 9) = "SUM"
        pworksheet.Cells(1, 10) = "%_AREA"
        pworksheet.Cells(1, 11) = "Label"
        pworksheet.Cells(1, 12) = "AREA_DEM"
        pworksheet.Cells(1, 13) = "%_AREA_DEM"
        pworksheet.Cells(1, 14) = "VOL_ACRE_FT"
        pworksheet.Cells(1, 15) = "%_VOL"

        '====================================================
        'Populate Elevation and Percent Area Rows
        '====================================================
        Dim pZonalRow As IRow
        Dim PercentArea As Double
        Dim TempMaxPRISMValue As Object
        PercentArea = 0
        'Create Feature Cursor and Feature
        For i = 1 To RasterValueCount
            'Target Rows
            pZonalRow = ZonalTable.GetRow(i)
            'Determine PercentArea
            PercentArea = PercentArea + (pZonalRow.Value(CountIndex) / SumOfCount) * 100
            'Populate Excel Table
            pworksheet.Cells(i + 2, 1) = IntervalList(pZonalRow.Value(ValueIndex)).UpperBound * conversionFactor       'Value
            pworksheet.Cells(i + 2, 2) = pZonalRow.Value(CountIndex)                                            'Count
            pworksheet.Cells(i + 2, 3) = pZonalRow.Value(AreaIndex)                                             'Area
            pworksheet.Cells(i + 2, 4) = pZonalRow.Value(MinIndex)                                              'MIN
            pworksheet.Cells(i + 2, 5) = pZonalRow.Value(MaxIndex)                                              'MAX
            pworksheet.Cells(i + 2, 6) = pZonalRow.Value(RangeIndex)                                            'Range
            pworksheet.Cells(i + 2, 7) = pZonalRow.Value(MeanIndex)                                             'Mean
            pworksheet.Cells(i + 2, 8) = pZonalRow.Value(STDIndex)                                              'STD
            pworksheet.Cells(i + 2, 9) = pZonalRow.Value(SumIndex)                                              'Sum
            pworksheet.Cells(i + 2, 10) = (pZonalRow.Value(CountIndex) / SumOfCount) * 100                'PERCENT_AREA
            pworksheet.Cells(i + 2, 11) = IntervalList(pZonalRow.Value(ValueIndex)).Name             'label
            'Determine Max PRISM Value (for Charting Purposes)
            TempMaxPRISMValue = (pZonalRow.Value(CountIndex) / SumOfCount) * 100

            'find the largest % value
            If TempMaxPRISMValue > MaxPRISMValue Then
                MaxPRISMValue = TempMaxPRISMValue
            End If
        Next

        'Make First PRISM Interval the Min of AOI DEM
        pworksheet.Cells(2, 1) = aoiDemMin * BA_SetConversionFactor(optZValue, True) 'Value
        pworksheet.Cells(2, 10) = 0 'PERCENT_AREA_ELEVATION
        pZonalRow = Nothing
        ZonalTable = Nothing

        '===============================================
        'Set MaxPRISMValue to an Even Whole Number
        '===============================================
        MaxPRISMValue = Math.Round(MaxPRISMValue * 1.05 + 0.5) 'this number is to set the x axis of the chart
        If MaxPRISMValue Mod 2 > 0 Then
            MaxPRISMValue = MaxPRISMValue + 1
        End If
        Return 1
    End Function

    Public Function BA_Excel_CreatePRISMChart(ByRef pPRISMWorkSheet As Worksheet, ByRef pElevWorkSheet As Worksheet, ByRef pChartsWorksheet As Worksheet, _
                                           ByVal Position_Left As Double, ByVal Position_Top As Double, ByVal Y_Min As Double, ByVal Y_Max As Double, _
                                           ByVal Y_Unit As Double, ByVal X_Max As Double, ByVal optZMetersValue As Boolean, ByVal optZFeetValue As Boolean) As Integer
        Dim PRISMLastRow As Long, ElevLastRow As Long
        Dim returnvalue As Integer
        returnvalue = -1

        Dim nrecords As Long = BA_Excel_CountRecords(pPRISMWorkSheet, 2)
        PRISMLastRow = nrecords + 2

        nrecords = BA_Excel_CountRecords(pElevWorkSheet, 2)
        ElevLastRow = nrecords + 2

        '======================================================================================================
        'Make Chart
        '======================================================================================================
        Dim myChart As Chart
        myChart = pChartsWorksheet.Shapes.AddChart.Chart

        'Determine Z Mapping Unit and Create Value Axis Title
        Dim AxisTitleUnit As String = Nothing
        If optZMetersValue = True Then
            AxisTitleUnit = " (Meters)"
        ElseIf optZFeetValue = True Then
            AxisTitleUnit = " (Feet)"
        End If

        'Set PRISM Data Ranges
        Dim PRISMRange As String
        Dim xPRISMValueRange As String
        Dim vPRISMValueRange As String
        PRISMRange = "J3:J" & PRISMLastRow
        xPRISMValueRange = "A3:A" & PRISMLastRow
        vPRISMValueRange = "O3:O" & PRISMLastRow '% Volume

        'Set Elevation Ranges
        Dim sElevRange As String
        Dim sElevValueRange As String
        sElevRange = "K2:K" & ElevLastRow
        sElevValueRange = "A2:A" & ElevLastRow

        With myChart
            'Clear Styles
            .ClearToMatchStyle()
            'Insert Title
            .HasTitle = True
            .HasLegend = True
            .ChartTitle.Caption = "Area-Elevation and Precipitation Distribution"
            'Set Chart Type and Data Range
            .ChartType = Excel.XlChartType.xlXYScatterLines
            .SetSourceData(pPRISMWorkSheet.Range(PRISMRange))
            'Set Element Positions
            .SetElement(MsoChartElementType.msoElementChartTitleAboveChart)
            .SetElement(MsoChartElementType.msoElementLegendBottom)
            'Set Chart Position
            .Parent.Left = Position_Left
            .Parent.Width = BA_ChartWidth
            .Parent.Top = Position_Top
            .Parent.Height = BA_ChartHeight
        End With

        'Clear Previous Series
        With myChart
            Do Until .SeriesCollection.Count = 0
                .SeriesCollection(1).Delete()
            Loop
        End With

        'Elevation Series
        Dim ElvSeries As Series
        ElvSeries = myChart.SeriesCollection.NewSeries
        With ElvSeries
            .Name = "Elevation"
            'Set Series Values
            .Values = pElevWorkSheet.Range(sElevValueRange)
            .XValues = pElevWorkSheet.Range(sElevRange)
            'Set Series Formats
            .MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleNone
            .Smooth = False
            'Set Axis Group
            .AxisGroup = Excel.XlAxisGroup.xlPrimary
        End With

        Dim PRISM As Series = myChart.SeriesCollection.NewSeries
        With PRISM
            .Name = "Precipitation"
            'Set Series Type
            '.Type = xlXYScatter: Not sure if this is right; Can't find key for type; Method just asks for integer
            'Set Series Values
            .Values = pPRISMWorkSheet.Range(xPRISMValueRange)
            .XValues = pPRISMWorkSheet.Range(vPRISMValueRange)
            'Set Series Formats
            .Format.Line.ForeColor.RGB = RGB(204, 0, 0)
            .MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleSquare
            .Smooth = False
            'Set Axis Group
            .AxisGroup = Excel.XlAxisGroup.xlSecondary
        End With

        'Set Variables Associates with each Axis
        With myChart
            'Bottom Axis
            Dim categoryAxis As Excel.Axis = CType(.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary), Excel.Axis)
            With categoryAxis
                .HasTitle = True
                .AxisTitle.Characters.Text = "% AOI Area below Elevation"
                .AxisTitle.Orientation = 0
                .MaximumScale = "100.1"
                .MinimumScale = "0"
            End With

            'Left Side Axis
            Dim valueAxis As Excel.Axis = CType(.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary), Excel.Axis)
            With valueAxis
                .HasTitle = True
                .AxisTitle.Characters.Text = "Elevation" & AxisTitleUnit
                .AxisTitle.Orientation = 90
                .MaximumScale = Y_Max
                .MinimumScale = Y_Min
                .MajorUnit = Y_Unit
            End With

            'Right Side Axis
            Dim rightAxis As Excel.Axis = CType(.Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlSecondary), Excel.Axis)
            With rightAxis
                .HasTitle = True
                .AxisTitle.Characters.Text = "Elevation" & AxisTitleUnit
                .AxisTitle.Orientation = 90
                .MaximumScale = Y_Max
                .MinimumScale = Y_Min
                .MajorUnit = Y_Unit
            End With

            'Top Axis
            Dim topAxis As Excel.Axis = CType(.Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlSecondary), Excel.Axis)
            With topAxis
                .HasTitle = True
                .AxisTitle.Characters.Text = "Precipitation Distribution (% contribution by elevation zone)"
                .AxisTitle.Orientation = "0"
                .MaximumScale = X_Max
                .MinimumScale = "0"
            End With

            'Insert Axes
            .HasAxis(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary) = True
            .HasAxis(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlSecondary) = True
            .HasAxis(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary) = True
            .HasAxis(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlSecondary) = True
        End With 'myChart

        'Clear Chart and Return Value
        myChart = Nothing
        Return 1
    End Function

    Public Function BA_Excel_CreateCombinedChart(ByRef pPRISMWorkSheet As Worksheet, ByRef pElvWorksheet As Worksheet, ByRef pChartsWorksheet As Worksheet, _
                                                 ByRef pSnowCourseWorksheet As Worksheet, ByRef pSNOTELWorksheet As Worksheet, ByVal Y_Min As Double, _
                                                 ByVal Y_Max As Double, ByVal Y_Unit As Double, ByVal MaxPRISMValue As Double, ByVal optZMetersValue As Boolean, _
                                                 ByVal optZFeetValue As Boolean, ByVal aoiHasSnotel As Boolean, ByVal aoiHasSnowCourse As Boolean)

        Dim ElevReturn As Long, PRISMReturn As Long, SNOTELReturn As Long, SnowCourseReturn As Long

        Dim nrecords As Long = BA_Excel_CountRecords(pElvWorksheet, 2)
        ElevReturn = nrecords + 2

        nrecords = BA_Excel_CountRecords(pPRISMWorkSheet, 2)
        PRISMReturn = nrecords + 2

        nrecords = BA_Excel_CountRecords(pSNOTELWorksheet, 1)
        SNOTELReturn = nrecords - 1 'not counting the last record, i.e., not presented

        nrecords = BA_Excel_CountRecords(pSnowCourseWorksheet, 1)
        SnowCourseReturn = nrecords - 1 'not counting the last record, i.e., not presented

        Dim myChart As Chart = pChartsWorksheet.Shapes.AddChart.Chart

        'Determine Z Mapping Unit and Create Value Axis Title
        Dim AxisTitleUnit As String = Nothing
        If optZMetersValue = True Then
            AxisTitleUnit = " (Meters)"
        ElseIf optZFeetValue = True Then
            AxisTitleUnit = " (Feet)"
        End If

        'Set SNOTEL Ranges
        Dim vSNOTELValueRange As String = Nothing
        Dim xSNOTELValueRange As String = Nothing
        If aoiHasSnotel Then
            xSNOTELValueRange = "A2:A" & SNOTELReturn
            vSNOTELValueRange = "K2:K" & SNOTELReturn
        End If

        'Set SnowCourse Ranges
        Dim vSnowCourseValueRange As String = Nothing
        Dim xSnowCourseValueRange As String = Nothing
        If aoiHasSnowCourse Then
            xSnowCourseValueRange = "A2:A" & SnowCourseReturn
            vSnowCourseValueRange = "K2:K" & SnowCourseReturn
        End If

        'Set PRISM Data Ranges
        Dim PRISMRange As String
        Dim xPRISMValueRange As String
        Dim vPRISMValueRange As String
        PRISMRange = "J3:J" & PRISMReturn
        xPRISMValueRange = "A3:A" & PRISMReturn
        vPRISMValueRange = "O3:O" & PRISMReturn

        'Set Elevation Ranges
        Dim sElevRange As String
        Dim sElevValueRange As String
        sElevRange = "K2:K" & ElevReturn
        sElevValueRange = "A2:A" & ElevReturn

        With myChart
            'Clear Styles
            .ClearToMatchStyle()
            'Insert Title
            .HasTitle = True
            .HasLegend = True
            .ChartTitle.Caption = "Area-Elevation, Precipitation  and Site Distribution"
            'Set Chart Type and Data Range
            .ChartType = Excel.XlChartType.xlXYScatter
            .SetSourceData(pPRISMWorkSheet.Range(PRISMRange))
            'Set Element Positions
            .SetElement(MsoChartElementType.msoElementChartTitleAboveChart)
            .SetElement(MsoChartElementType.msoElementLegendBottom)
            'Set Chart Position
            .Parent.Left = BA_ChartSpacing
            .Parent.Width = BA_ChartWidth
            .Parent.Top = BA_ChartHeight + BA_ChartSpacing + BA_ChartSpacing
            .Parent.Height = BA_ChartHeight
        End With

        'Clear Previous Series
        With myChart
            Do Until .SeriesCollection.Count = 0
                .SeriesCollection(1).Delete()
            Loop
        End With

        'Snow Course Series
        Dim scSeries As Series
        If aoiHasSnowCourse Then
            scSeries = myChart.SeriesCollection.NewSeries
            With scSeries
                .Name = "Snow Course"
                'Set Series Values
                .Values = pSnowCourseWorksheet.Range(xSnowCourseValueRange)
                .XValues = pSnowCourseWorksheet.Range(vSnowCourseValueRange)
                'Set Series Formats
                .MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleTriangle
                .MarkerForegroundColor = RGB(246, 32, 10)
                .MarkerBackgroundColor = RGB(246, 32, 10)
                'Set Axis Group
                .AxisGroup = Excel.XlAxisGroup.xlPrimary
            End With
        End If

        'SNOTEL Series
        Dim SNOTELSeries As Series
        If aoiHasSnotel Then
            SNOTELSeries = myChart.SeriesCollection.NewSeries
            With SNOTELSeries
                .Name = "SNOTEL"
                'Set Series Values
                .Values = pSNOTELWorksheet.Range(xSNOTELValueRange)
                .XValues = pSNOTELWorksheet.Range(vSNOTELValueRange)
                'Set Series Formats
                .MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleTriangle
                .MarkerForegroundColor = RGB(0, 0, 0)
                .MarkerBackgroundColor = RGB(0, 0, 0)
                'Set Axis Group
                .AxisGroup = Excel.XlAxisGroup.xlPrimary
            End With
        End If

        'Elevation Series
        Dim ElvSeries As Series
        ElvSeries = myChart.SeriesCollection.NewSeries
        With ElvSeries
            .Name = "Elevation"
            'Set Series Values
            .Values = pElvWorksheet.Range(sElevValueRange)
            .XValues = pElvWorksheet.Range(sElevRange)
            'Set Series Formats
            .Smooth = False
            .Format.Line.DashStyle = MsoLineDashStyle.msoLineSolid
            .Format.Line.ForeColor.RGB = RGB(74, 126, 187)
            .MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleNone
            'Set Axis Group
            .AxisGroup = Excel.XlAxisGroup.xlPrimary
            'Set to be first plotted series
            .PlotOrder = 1
        End With

        'PRISM Series
        Dim PRISM As Series
        PRISM = myChart.SeriesCollection.NewSeries
        With PRISM
            .Name = "Precipitation"
            'Set Series Values
            .Values = pPRISMWorkSheet.Range(xPRISMValueRange)
            .XValues = pPRISMWorkSheet.Range(vPRISMValueRange)
            'Set Series Formats
            .Smooth = False
            .Format.Line.DashStyle = MsoLineDashStyle.msoLineSolid
            .Format.Line.ForeColor.RGB = RGB(204, 0, 0)
            .Format.Line.BackColor.RGB = RGB(204, 0, 0)

            .MarkerStyle = Excel.XlMarkerStyle.xlMarkerStyleSquare
            'Set Axis Group
            .AxisGroup = Excel.XlAxisGroup.xlSecondary
            'Set to be first plotted series
            .PlotOrder = 1
        End With

        'Set Variables Associates with each Axis
        With myChart
            'Bottom Axis
            With .Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary)
                .HasTitle = True
                .AxisTitle.Characters.Text = "% AOI Area below Elevation"
                .AxisTitle.Orientation = 0
                .MaximumScale = "100.1"
                .MinimumScale = "0"
            End With

            'Left Side Axis
            With .Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary)
                .HasTitle = True
                .AxisTitle.Characters.Text = "Elevation" & AxisTitleUnit
                .AxisTitle.Orientation = 90
                .MaximumScale = Y_Max
                .MinimumScale = Y_Min
                .MajorUnit = Y_Unit
            End With

            'Right Side Axis
            With .Axes(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlSecondary)
                .HasTitle = True
                .AxisTitle.Characters.Text = "Elevation" & AxisTitleUnit
                .AxisTitle.Orientation = 90
                .MaximumScale = Y_Max
                .MinimumScale = Y_Min
                .MajorUnit = Y_Unit
            End With

            'Top Axis
            With .Axes(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlSecondary)
                .HasTitle = True
                .AxisTitle.Characters.Text = "Precipitation Distribution (% contribution by elevation zone)"
                .AxisTitle.Orientation = "0"
                .MaximumScale = MaxPRISMValue
                .MinimumScale = "0"
            End With

            'Insert Axes
            .HasAxis(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlPrimary) = True
            .HasAxis(Excel.XlAxisType.xlCategory, Excel.XlAxisGroup.xlSecondary) = True
            .HasAxis(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlPrimary) = True
            .HasAxis(Excel.XlAxisType.xlValue, Excel.XlAxisGroup.xlSecondary) = True
        End With 'myChart
        Return 1
    End Function

    'estimate the precipitation volume in each elevation zone
    'input area unit in square meters
    'input precip unit in inches
    'output volume in acre-feet
    Public Function BA_Excel_PrecipitationVolume(ByRef pPRSIMWS As Worksheet, ByVal AreaCol As Integer, ByVal PrecipCol As Integer, ByVal VolumeCol As Integer, _
                                                 ByVal PercentCol As Integer) As Long
        Dim pRange As Range
        Dim returnvalue As Long
        Dim row_index As Long
        Dim conversionfactor As Double
        Dim total_vol As Double
        Dim i As Integer

        row_index = 3
        returnvalue = 0
        pRange = pPRSIMWS.Cells(row_index, 1)

        conversionfactor = 1 / (4046.8564224 * 12) 'convert sq meter-inch to acre-foot
        '1 square meter = 2.471053814671653e-4 acre
        '1 inch = 1/12 feet

        total_vol = 0
        Do While Len(pRange.Text.ToString) > 0
            Dim areaRange As Range = pPRSIMWS.Cells(row_index, AreaCol)
            Dim precipRange As Range = pPRSIMWS.Cells(row_index, PrecipCol)
            Dim volumeRange As Range = pPRSIMWS.Cells(row_index, VolumeCol)
            volumeRange.Value = CDbl(areaRange.Value) * CDbl(precipRange.Value) * conversionfactor
            total_vol = total_vol + CDbl(volumeRange.Value)
            row_index = row_index + 1
            returnvalue = returnvalue + 1
            pRange = pPRSIMWS.Cells(row_index, 1)
        Loop

        'calculate % volume
        For i = 1 To returnvalue
            Dim pctRange As Range = pPRSIMWS.Cells(i + 2, PercentCol)
            Dim volumeRange As Range = pPRSIMWS.Cells(i + 2, VolumeCol)
            pctRange.Value = volumeRange.Value * 100 / total_vol
        Next

        Return returnvalue
    End Function

    'create a range worksheet
    'copy records in the elevation table that fall with an elevation range
    Public Function BA_Excel_CreateElevRangeTable(ByRef pworksheet As Worksheet, ByVal pSourceWS As Worksheet, ByVal Start_Value As Double, ByVal End_Value As Double) As Integer
        Dim response As Integer
        Dim i As Integer
        Dim NColumn As Integer
        Dim Start_Row As Integer
        Dim End_Row As Integer
        Dim Row_Count As Integer
        Dim cell_value As Range
        Dim Total_Area As Double
        Dim Cumu_PArea As Double

        Dim nrecord As Integer

        NColumn = 12

        '=============================================
        'Create Field Titles
        '=============================================
        pworksheet.Cells(1, 1) = "VALUE"
        pworksheet.Cells(1, 2) = "COUNT"
        pworksheet.Cells(1, 3) = "AREA"
        pworksheet.Cells(1, 4) = "MIN"
        pworksheet.Cells(1, 5) = "MAX"
        pworksheet.Cells(1, 6) = "RANGE"
        pworksheet.Cells(1, 7) = "MEAN"
        pworksheet.Cells(1, 8) = "STD"
        pworksheet.Cells(1, 9) = "SUM"
        pworksheet.Cells(1, 10) = "%_AREA"
        pworksheet.Cells(1, 11) = "C%_AREA"
        pworksheet.Cells(1, 12) = "LABEL"

        'determine the begin and ending row
        Dim col_index As Long 'the column contains elevation range text
        col_index = 1
        cell_value = pSourceWS.Cells(2, col_index)

        Start_Row = 0
        End_Row = 0
        Row_Count = 2

        'compare only the integer part of the number
        Do While Len(cell_value.Value) > 0
            If CInt(cell_value.Value) = CInt(Start_Value) Then Start_Row = Row_Count
            If CInt(cell_value.Value) = CInt(End_Value) Then End_Row = Row_Count
            Row_Count = Row_Count + 1
            cell_value = pSourceWS.Cells(Row_Count, col_index)
        Loop

        If Start_Row * End_Row = 0 Then
            MsgBox("Unable to find the specified range value(s)! The output range table is incomplete." & vbCrLf & _
            "The FROM elevation value exists in the buffered DEM but might not exist within the AOI.")
            Return 0
        End If

        'copy cell values
        'start_row contains the upper bound of the elevation range, skip to the next row to get the actual range
        For i = 1 To NColumn
            response = BA_Excel_CopyRangeCells(pSourceWS, i, pworksheet, i, Start_Row + 1, End_Row, 3)
        Next
        nrecord = response

        'recalculate %area and cumulative_%area
        Total_Area = 0
        For i = 1 To nrecord
            Dim nextRange As Range = pworksheet.Cells(i + 2, 3)
            Total_Area = Total_Area + nextRange.Value
        Next

        Cumu_PArea = 0
        For i = 1 To nrecord
            Dim pctArea As Range = pworksheet.Cells(i + 2, 10)
            Dim area As Range = pworksheet.Cells(i + 2, 3)
            Dim cumu_pctArea As Range = pworksheet.Cells(i + 2, 11)
            pctArea.Value = area.Value * 100 / Total_Area
            Cumu_PArea = Cumu_PArea + pctArea.Value
            cumu_pctArea.Value = Cumu_PArea
        Next

        'AOI_DEMMin is always in meters
        'Make First Elevation Interval the Min of AOI DEM
        pworksheet.Cells(2, 1) = Start_Value
        pworksheet.Cells(2, 11) = 0 'PERCENT_AREA_ELEVATION

        Return 1
    End Function

    'create a range worksheet
    'copy records in the precipitation table that fall with an elevation range
    Public Function BA_Excel_CreatePrecipRangeTable(ByRef pworksheet As Worksheet, ByVal pSourceWS As Worksheet, ByVal Start_Value As Double, ByVal End_Value As Double, _
                                                    ByRef Return_MaxValue As Double) As Integer
        Dim response As Integer
        Dim i As Integer
        Dim NColumn As Integer
        Dim Start_Row As Integer
        Dim End_Row As Integer
        Dim Row_Count As Integer
        Dim cell_value As Range
        Dim Total_Area As Double, Total_DEMArea As Double
        Dim Total_Volume As Double

        Dim nrecord As Integer

        NColumn = 15

        '============================================
        'Create Field Titles
        '============================================
        pworksheet.Cells(1, 1) = "VALUE"
        pworksheet.Cells(1, 2) = "COUNT"
        pworksheet.Cells(1, 3) = "AREA"
        pworksheet.Cells(1, 4) = "MIN"
        pworksheet.Cells(1, 5) = "MAX"
        pworksheet.Cells(1, 6) = "RANGE"
        pworksheet.Cells(1, 7) = "MEAN"
        pworksheet.Cells(1, 8) = "STD"
        pworksheet.Cells(1, 9) = "SUM"
        pworksheet.Cells(1, 10) = "%_AREA"
        pworksheet.Cells(1, 11) = "Label"
        pworksheet.Cells(1, 12) = "AREA_DEM"
        pworksheet.Cells(1, 13) = "%_AREA_DEM"
        pworksheet.Cells(1, 14) = "VOL_ACRE_FT"
        pworksheet.Cells(1, 15) = "%_VOL"

        'determine the begin and ending row
        Dim col_index As Long 'the column contains elevation range text
        col_index = 1
        cell_value = pSourceWS.Cells(2, col_index)

        Start_Row = 0
        End_Row = 0
        Row_Count = 2

        'compare only the integer part of the number
        Do While Len(cell_value.Value) > 0
            If CInt(cell_value.Value) = CInt(Start_Value) Then Start_Row = Row_Count
            If CInt(cell_value.Value) = CInt(End_Value) Then End_Row = Row_Count
            Row_Count = Row_Count + 1
            cell_value = pSourceWS.Cells(Row_Count, col_index)
        Loop

        If Start_Row * End_Row = 0 Then
            MsgBox("Unable to find the specified range value(s)! The output range table is incomplete." & vbCrLf & _
            "The FROM elevation value exists in the buffered DEM but might not exist within the AOI.")
            Return 0
        End If

        'copy cell values
        'start_row contains the upper bound of the elevation range, skip to the next row to get the actual range
        For i = 1 To NColumn
            response = BA_Excel_CopyRangeCells(pSourceWS, i, pworksheet, i, Start_Row + 1, End_Row, 3)
        Next
        nrecord = response

        'recalculate Area and Volume %
        Total_Area = 0
        Total_DEMArea = 0
        Total_Volume = 0
        For i = 1 To nrecord
            Dim aRange As Range = pworksheet.Cells(i + 2, 3)
            Dim dRange As Range = pworksheet.Cells(i + 2, 12)
            Dim vRange As Range = pworksheet.Cells(i + 2, 14)
            Total_Area = Total_Area + aRange.Value
            Total_DEMArea = Total_DEMArea + dRange.Value
            Total_Volume = Total_Volume + vRange.Value
        Next

        Return_MaxValue = -10
        For i = 1 To nrecord
            Dim pctAreaRange As Range = pworksheet.Cells(i + 2, 10)
            Dim areaRange As Range = pworksheet.Cells(i + 2, 3)
            pctAreaRange.Value = areaRange.Value * 100 / Total_Area
            If pctAreaRange.Value > Return_MaxValue Then Return_MaxValue = pctAreaRange.Value
            Dim dRange As Range = pworksheet.Cells(i + 2, 12)
            Dim vRange As Range = pworksheet.Cells(i + 2, 14)
            pworksheet.Cells(i + 2, 13) = dRange.Value * 100 / Total_DEMArea
            pworksheet.Cells(i + 2, 15) = vRange.Value * 100 / Total_Volume
        Next

        'AOI_DEMMin is always in meters
        'Make First Elevation Interval the Min of AOI DEM
        pworksheet.Cells(2, 1) = Start_Value
        pworksheet.Cells(2, 10) = 0 'PERCENT_AREA_ELEVATION

        '===============================================
        'Set Return_MaxValue to an Even Whole Number
        '===============================================
        Return_MaxValue = Math.Round(Return_MaxValue * 1.05 + 0.5)
        If Return_MaxValue Mod 2 > 0 Then
            Return_MaxValue = Return_MaxValue + 1
        End If

        Return 1
    End Function

    'create a range worksheet
    'copy records in the SNOTEL or Snow Course table that fall with an elevation range
    Public Function BA_Excel_CreateSNOTELRangeTable(ByRef pworksheet As Worksheet, ByVal pSourceWS As Worksheet, ByRef pElevWS As Worksheet, ByVal Start_Value As Double, _
                                                    ByVal End_Value As Double) As Integer
        Dim i As Integer
        Dim NColumn As Integer
        Dim Start_Row As Integer
        Dim End_Row As Integer
        Dim Row_Count As Integer
        Dim cell_value As Range
        Dim Total_Area As Double
        Dim Cumu_PArea As Double

        Dim nrecord As Integer

        NColumn = 12

        '===================================
        'Create Field Titles
        '===================================
        pworksheet.Cells(1, 1) = "VALUE"
        pworksheet.Cells(1, 2) = "COUNT"
        pworksheet.Cells(1, 3) = "AREA"
        pworksheet.Cells(1, 4) = "MIN"
        pworksheet.Cells(1, 5) = "MAX"
        pworksheet.Cells(1, 6) = "RANGE"
        pworksheet.Cells(1, 7) = "MEAN"
        pworksheet.Cells(1, 8) = "STD"
        pworksheet.Cells(1, 9) = "SUM"
        pworksheet.Cells(1, 10) = "%_AREA"
        pworksheet.Cells(1, 11) = "%_AREA_ELV"
        pworksheet.Cells(1, 12) = "Label"

        'determine the begin and ending row
        Dim col_index As Long 'the column contains elevation range text
        Dim Start_Cumu_Percent As Double
        Dim End_Cumu_Percent As Double

        col_index = 1
        cell_value = pElevWS.Cells(2, col_index)

        Start_Row = 0
        End_Row = 0
        Row_Count = 2
        Dim Last_Value As Integer = CInt(cell_value.Value) 'this variable keeps track of the last cell_value until the last row of source spreadsheet is passed
        'it was created to fix the "extra point" bug when the last row of the range table equals the last row of the source table
        'when the last row is within the range of the range table, the last row should not be copied to the range table.

        Do While Len(cell_value.Value) > 0

            If CInt(cell_value.Value) = CInt(Start_Value) Then
                Start_Row = Row_Count
                Dim peRange As Range = pElevWS.Cells(Row_Count, 11)
                Start_Cumu_Percent = peRange.Value
            End If

            If CInt(cell_value.Value) = CInt(End_Value) Then
                End_Row = Row_Count
                Dim peRange As Range = pElevWS.Cells(Row_Count, 11)
                End_Cumu_Percent = peRange.Value
            End If
            Last_Value = CInt(cell_value.Value)
            Row_Count = Row_Count + 1
            cell_value = pElevWS.Cells(Row_Count, col_index)
        Loop

        If Last_Value = CInt(End_Value) Then 'skip the last row if it's the last row (i.e., not represented) of the source spreadsheet
            End_Row = End_Row - 1
            End_Cumu_Percent = pElevWS.Cells(Row_Count - 2, 11).value
        End If

        If Start_Row * End_Row <= 0 Then
            MsgBox("Unable to find the specified range value(s)! The output range table is incomplete." & vbCrLf & _
            "The FROM elevation value exists in the buffered DEM but might not exist within the AOI.")
            Return 0
        End If

        'copy SNOTEL rows that have elevation values fall within the range
        cell_value = pSourceWS.Cells(2, col_index)
        Row_Count = 2
        nrecord = 0

        Do While Len(cell_value.Value) > 0
            'Dim debug As Integer = CInt(cell_value.Value)
            If CInt(cell_value.Value) >= CInt(Start_Value) And CInt(cell_value.Value) <= CInt(End_Value) Then
                If CInt(cell_value.Value) <> Last_Value Then 'skip the last row if it's the last row (i.e., not represented) of the source spreadsheet
                    nrecord = nrecord + 1
                    For i = 1 To NColumn
                        pworksheet.Cells(nrecord + 1, i) = pSourceWS.Cells(Row_Count, i)
                    Next
                End If
            End If

            Row_Count = Row_Count + 1
            cell_value = pSourceWS.Cells(Row_Count, col_index)
        Loop

        'adjust % area of the first and the last SNOTEL record
        'first record - replace the % area with the difference between the cumulative % area and the cumulative % area of the start elevation
        Dim pctAreaElevRange As Range = pworksheet.Cells(2, 11)
        pworksheet.Cells(2, 10) = pctAreaElevRange.Value - Start_Cumu_Percent
        'last record - add an additional record showing the % area between the last elevation and the highest SNOTEL site within the range
        Dim lstPctAreaElevRange As Range = pworksheet.Cells(nrecord + 1, 11)
        pworksheet.Cells(nrecord + 2, 10) = End_Cumu_Percent - lstPctAreaElevRange.Value

        'recalculate %area (col: 10) and cumulative_%area (col: 11)
        Total_Area = 0
        For i = 1 To nrecord + 1
            Dim pctAreaRange As Range = pworksheet.Cells(i + 1, 10)
            Total_Area = Total_Area + pctAreaRange.Value
        Next

        Cumu_PArea = 0
        For i = 1 To nrecord + 1
            Dim pctAreaRange As Range = pworksheet.Cells(i + 1, 10)
            pctAreaRange.Value = pctAreaRange.Value * 100 / Total_Area
            Cumu_PArea = Cumu_PArea + pctAreaRange.Value
            pworksheet.Cells(i + 1, 11) = Cumu_PArea
        Next

        Return 1
    End Function

End Module
