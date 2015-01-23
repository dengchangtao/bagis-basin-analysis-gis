Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports ESRI.ArcGIS.GeoAnalyst
Imports ESRI.ArcGIS.esriSystem

Public Module EnumHelperModule

    ''This procedure gets the <Description> attribute of an enum constant, if any.
    ''Otherwise it gets the string name of the enum member.
    Public Function BA_EnumDescription(ByVal EnumConstant As [Enum]) As String
        Dim fi As Reflection.FieldInfo = EnumConstant.GetType().GetField(EnumConstant.ToString())
        Dim aattr() As DescriptionAttribute = DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
        If aattr.Length > 0 Then
            Return aattr(0).Description
        Else
            Return Nothing
        End If
    End Function

    'Returns ArcObjects generated field name when provided with an esriGeoAnalysisStatisticsEnum. 
    'The source enum only contains integers.
    Public Function BA_FieldNameFromStatisticEnum(ByVal esriEnum As esriGeoAnalysisStatisticsEnum) As String
        Dim ordinal As Short = CType(esriEnum, Short)
        Dim field As StatisticsFieldName = CType(ordinal, StatisticsFieldName)
        Return field.ToString
    End Function

    'Returns the ArcObjects generated field name for a StatisticsTypeString
    Public Function BA_FieldNameFromTypeString(ByVal statTypeEnum As StatisticsTypeString) As String
        Dim ordinal As Short = CType(statTypeEnum, Short)
        Dim typeString As StatisticsFieldName = CType(ordinal, StatisticsFieldName)
        Return typeString.ToString
    End Function

    'Returns a list of file names that have that have style names associated with them.
    'As of June 2011 not sure this is still true as MapsFileName has been used for other purposes.
    'Default style used if style not found
    Public Function BA_ListOfLayerNamesWithStyles() As List(Of String)
        Dim layers As New List(Of String)
        For Each FileName In [Enum].GetValues(GetType(MapsFileName))
            Dim EnumConstant As [Enum] = FileName
            Dim fi As Reflection.FieldInfo = EnumConstant.GetType().GetField(EnumConstant.ToString())
            Dim aattr() As DescriptionAttribute = DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
            Dim strFileName As String = aattr(0).Description
            layers.Add(strFileName)
        Next
        Return layers
    End Function

    ' Returns the MapsLayerName enum if fileLayerText is present; If not, returns nothing
    Public Function BA_GetMapsLayerName(ByVal fileLayerText As String) As MapsLayerName
        For Each fLayer In [Enum].GetValues(GetType(MapsLayerName))
            Dim fi As Reflection.FieldInfo = fLayer.GetType().GetField(fLayer.ToString())
            Dim aattr() As DescriptionAttribute = DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
            Dim compareText As String = aattr(0).Description
            If String.Compare(compareText, fileLayerText) = 0 Then
                Return fLayer
            End If
        Next
        Return MapsLayerName.no_name
    End Function

    ''Returns the String <Description> associated with an AOIPrismFolderNames entry corresponding
    ''to the provided index. Translates index to folder name in Weasel file structure
    Public Function BA_GetPrismFolderName(ByVal index As Short) As String
        Dim i As Short = 1
        For Each folder As AOIPrismFolderNames In [Enum].GetValues(GetType(AOIPrismFolderNames))
            If i = index Then
                Return folder.ToString
            End If
            i = i + 1
        Next
        Return Nothing
    End Function

    ' Returns the HruRuleType enum associated with String <Description> that is passed in
    Public Function BA_GetRuleType(ByVal ruleTypeText As String) As HruRuleType
        For Each rule In [Enum].GetValues(GetType(HruRuleType))
            Dim fi As Reflection.FieldInfo = rule.GetType().GetField(rule.ToString())
            Dim aattr() As DescriptionAttribute = DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
            Dim compareText As String = aattr(0).Description
            If String.Compare(compareText, ruleTypeText) = 0 Then
                Return rule
            End If
        Next
        Return Nothing
    End Function

    ' Returns the esriGeoAnalysisSliceEnum entry associated with a String description of the
    ' slice type. The source enum only contains integers.
    Public Function BA_GetEsriSliceType(ByVal sliceTypeText As String) As esriGeoAnalysisSliceEnum
        Select Case sliceTypeText
            Case Is = "Equal Interval Slice"
                Return esriGeoAnalysisSliceEnum.esriGeoAnalysisSliceEqualInterval
            Case Is = "Equal Area Slice"
                Return esriGeoAnalysisSliceEnum.esriGeoAnalysisSliceEqualArea
            Case Is = "Natural Breaks Slice"
                Return esriGeoAnalysisSliceEnum.esriGeoAnalysisSliceNatualBreaks
            Case Is = "Standard Deviation Slice"
                Return esriGeoAnalysisSliceEnum.esriGeoAnalysisSliceStandardDeviation
        End Select
        Return Nothing
    End Function

    ' Inverse of BA_GetEsriSliceType. Returns a String description of the slice type
    ' when provided with an esriGeoAnalysisSliceEnum. The source enum only contains integers.
    Public Function BA_GetEsriSliceTypeText(ByVal sliceEnum As esriGeoAnalysisSliceEnum) As String
        Select Case sliceEnum
            Case Is = esriGeoAnalysisSliceEnum.esriGeoAnalysisSliceEqualInterval
                Return "Equal Interval Slice"
            Case Is = esriGeoAnalysisSliceEnum.esriGeoAnalysisSliceEqualArea
                Return "Equal Area Slice"
            Case Is = esriGeoAnalysisSliceEnum.esriGeoAnalysisSliceNatualBreaks
                Return "Natural Breaks Slice"
            Case Is = esriGeoAnalysisSliceEnum.esriGeoAnalysisSliceStandardDeviation
                Return "Standard Deviation Slice"
            Case Else
                Return Nothing
        End Select
    End Function

    ''Returns the PrismDataRange entry associated with the PrismDataRange's String <Description>
    Public Function BA_GetPrismDataRange(ByVal dataRangeText As String) As PrismDataRange
        For Each range In [Enum].GetValues(GetType(PrismDataRange))
            Dim fi As Reflection.FieldInfo = range.GetType().GetField(range.ToString())
            Dim aattr() As DescriptionAttribute = DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
            Dim compareText As String = aattr(0).Description
            If String.Compare(compareText, dataRangeText) = 0 Then
                Return range
            End If
        Next
        Return Nothing
    End Function

    ''Returns the ActionParameter enum associated with the ActionParameter's String <Description>
    Public Function BA_GetActionParameter(ByVal strActionParam As String) As ActionParameter
        For Each aParam In [Enum].GetValues(GetType(ActionParameter))
            Dim compareText As String = aParam.ToString
            If String.Compare(compareText, strActionParam) = 0 Then
                Return aParam
            End If
        Next
        Return Nothing
    End Function

    ''Returns the MeasurementUnit entry associated with the MeasurementUnit's String <Description>
    Public Function BA_GetMeasurementUnit(ByVal unitText As String) As MeasurementUnit
        If Not String.IsNullOrEmpty(unitText) Then
            'Remove leading and trailing " Chr(34) if present
            Dim charsToTrim() As Char = {Chr(34)}
            unitText = unitText.Trim(charsToTrim)
        End If
        For Each unit In [Enum].GetValues(GetType(MeasurementUnit))
            Dim fi As Reflection.FieldInfo = unit.GetType().GetField(unit.ToString())
            Dim aattr() As DescriptionAttribute = DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
            Dim compareText As String = aattr(0).Description
            If String.Compare(compareText, unitText) = 0 Then
                Return unit
            End If
        Next
        Return Nothing
    End Function

    ''Returns the SlopeUnit entry associated with the SlopeUnit's String <Description> that is passed in
    Public Function BA_GetSlopeUnit(ByVal unitText As String) As SlopeUnit
        For Each unit In [Enum].GetValues(GetType(SlopeUnit))
            Dim fi As Reflection.FieldInfo = unit.GetType().GetField(unit.ToString())
            Dim aattr() As DescriptionAttribute = DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
            Dim compareText As String = aattr(0).Description
            If String.Compare(compareText, unitText) = 0 Then
                Return unit
            End If
        Next
        Return Nothing
    End Function

    'Generate the entry for the dropdown list that contains the folder type
    Public Function BA_GetFolderTypeForList(ByVal fType As FolderType) As String
        Dim strType As String = fType.ToString
        Return "[" & strType & "] "
    End Function

    'Return the FolderType based on what is in the dropdown list entry
    Public Function BA_GetFolderTypeFromString(ByVal strType As String) As FolderType
        Dim strAoi As String = "[" & FolderType.AOI.ToString & "] "
        Dim strBasin As String = "[" & FolderType.BASIN.ToString & "] "
        If strType.IndexOf(strAoi) > -1 Then
            Return FolderType.AOI
        ElseIf strType.IndexOf(strBasin) > -1 Then
            Return FolderType.BASIN
        End If
        Return FolderType.FOLDER
    End Function

    ''Returns the MeasurementUnitType entry associated with the MeasurementUnitType's String <Description>
    Public Function BA_GetMeasurementUnitType(ByVal unitText As String) As MeasurementUnitType
        For Each unit In [Enum].GetValues(GetType(MeasurementUnitType))
            Dim fi As Reflection.FieldInfo = unit.GetType().GetField(unit.ToString())
            Dim aattr() As DescriptionAttribute = DirectCast(fi.GetCustomAttributes(GetType(DescriptionAttribute), False), DescriptionAttribute())
            Dim compareText As String = aattr(0).Description
            If String.Compare(compareText, unitText) = 0 Then
                Return unit
            End If
        Next
        Return Nothing
    End Function

    ''Returns the MethodStatus entry associated with the MethodStatus's String representation
    Public Function BA_GetMethodStatus(ByVal methodText As String) As MethodStatus
        For Each pStatus As MethodStatus In [Enum].GetValues(GetType(MethodStatus))
            If pStatus.ToString = methodText Then
                Return pStatus
            End If
        Next
        Return Nothing
    End Function

    'Returns the esriUnits object from the String for the esri units that we use in BAGIS
    Public Function BA_GetEsriUnits(ByVal unitsText) As esriUnits
        Select Case unitsText
            Case "Feet"
                Return esriUnits.esriFeet
            Case "Meters"
                Return esriUnits.esriMeters
            Case "Miles"
                Return esriUnits.esriMiles
            Case "Kilometers"
                Return esriUnits.esriKilometers
            Case Else
                Return esriUnits.esriUnknownUnits
        End Select
    End Function

End Module
