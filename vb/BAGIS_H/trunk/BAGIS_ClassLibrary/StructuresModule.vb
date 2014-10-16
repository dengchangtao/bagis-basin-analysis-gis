Public Module StructuresModule

    ' Inherited structure from BAGIS. Used for classifying/symbolizing maps
    Public Structure BA_IntervalList
        Dim Value As Object
        Dim LowerBound As Object
        Dim UpperBound As Object
        Dim Name As Object
        Dim SNOTEL As Object
        Dim SnowCourse As Object
        Dim Area As Object
        Dim Blank As Object
    End Structure

    'Public Structure BA_UserConfig
    '    Dim AoiFolder As String
    '    Dim AoiName As String
    '    Dim HruFolder As String
    'End Structure

    ' Stores properties that are combined to symbolize layers
    Public Structure BA_Map_Symbology
        Dim DisplayName As String
        Dim DisplayStyle As String
        Dim Transparency As Short
        Dim Color As ESRI.ArcGIS.Display.IRgbColor
        Dim MarkerType As String
    End Structure

    ' Stores layer statistics derived from IDataStatistics object. These statistics are then
    ' passed between subroutines
    Structure BA_DataStatistics
        Dim Count As Integer
        Dim Maximum As Double
        Dim Mean As Double
        Dim Minimum As Double
        Dim StandardDeviation As Double
        Dim Sum As Double
    End Structure

    Structure BA_DEMInfo
        Dim IDText As String
        Dim Cellsize As Double
        Dim Min As Double
        Dim Max As Double
        Dim Range As Double
        Dim Exist As Boolean
    End Structure


End Module
