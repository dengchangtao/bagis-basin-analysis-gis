Imports System.Xml.Serialization
Imports ESRI.ArcGIS.esriSystem
Imports BAGIS_ClassLibrary

Public Class Analysis
    Inherits SerializableData

    Dim m_aoiName As String
    Dim m_aoiFolderPath As String
    Dim m_dateCreated As DateTime
    Dim m_shapeAreaKm As Double
    Dim m_minElev As String
    Dim m_maxElev As String
    Dim m_useBufferDistance As Boolean
    Dim m_bufferDistance As Double
    'xml ignore
    Dim m_bufferUnits As esriUnits
    Dim m_useUpperRange As Boolean
    Dim m_upperRange As Double
    Dim m_upperRangeText As String
    Dim m_useLowerRange As Boolean
    Dim m_lowerRange As Double
    Dim m_lowerRangeText As String
    Dim m_elevUnits As esriUnits
    Dim m_reportTitle As String
    Dim m_scenario1Title As String
    Dim m_scenario1Sites As Site()
    Dim m_scenario2Title As String
    Dim m_scenario2Sites As Site()
    Dim m_areaStatistics As AreaStatistics

    ' Required for de-serialization. Do not use.
    Sub New()
        MyBase.New()
    End Sub

    Sub New(ByVal aoiName As String, ByVal aoiFolderPath As String, ByVal shapeAreaKm As Double, _
            ByVal minElev As String, ByVal maxElev As String, ByVal useBufferDistance As Boolean, ByVal bufferDistance As Double, ByVal bufferUnits As esriUnits, _
            ByVal useUpperRange As Boolean, ByVal upperRange As Double, ByVal upperRangeText As String, ByVal useLowerRange As Boolean, _
            ByVal lowerRange As Double, ByVal lowerRangeText As String, ByVal elevUnits As esriUnits, ByVal reportTitle As String,
            ByVal scenario1Title As String, ByVal scenario2Title As String)
        m_aoiName = aoiName
        m_aoiFolderPath = aoiFolderPath
        m_shapeAreaKm = shapeAreaKm
        m_minElev = minElev
        m_maxElev = maxElev
        m_useBufferDistance = useBufferDistance
        m_bufferDistance = bufferDistance
        m_bufferUnits = bufferUnits
        m_useUpperRange = useUpperRange
        m_upperRange = upperRange
        m_upperRangeText = upperRangeText
        m_useLowerRange = useLowerRange
        m_lowerRange = lowerRange
        m_lowerRangeText = lowerRangeText
        m_elevUnits = ElevUnits
        m_reportTitle = reportTitle
        m_scenario1Title = scenario1Title
        m_scenario2Title = scenario2Title
    End Sub

    Public Property AoiName() As String
        Get
            Return m_aoiName
        End Get
        Set(ByVal value As String)
            m_aoiName = value
        End Set
    End Property

    Public Property AoiFolderPath() As String
        Get
            Return m_aoiFolderPath
        End Get
        Set(ByVal value As String)
            m_aoiFolderPath = value
        End Set
    End Property

    Public Property DateCreated() As DateTime
        Get
            Return m_dateCreated
        End Get
        Set(ByVal value As DateTime)
            m_dateCreated = value
        End Set
    End Property

    Public Property DateCreatedText() As String
        Get
            Dim zone As System.TimeZoneInfo = System.TimeZoneInfo.Local
            Dim strDate As String = m_dateCreated.ToString("d-MMM-yyyy h:m tt ")
            Return strDate & zone.DisplayName
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property ShapeAreaKm() As Double
        Get
            Return m_shapeAreaKm
        End Get
        Set(value As Double)
            m_shapeAreaKm = value
        End Set
    End Property

    Public Property ShapeAreaKmText() As String
        Get
            '0.361 Square KM
            Dim strKm As String = Format(Math.Round(m_shapeAreaKm, 3), "#0.000")
            Return strKm & " Square KM"
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property ShapeAreaAcresText() As String
        Get
            '89.24 Acres
            Dim dblAcres As Double = m_shapeAreaKm * BA_SQKm_To_ACRE
            Return Format(Math.Round(dblAcres, 2), "#0.00") & " Acres"
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property ShapeAreaHectaresText() As String
        Get
            '36.11 Hectares
            Dim dblHectares As Double = m_shapeAreaKm * BA_SQKm_To_HECTARE
            Return Format(Math.Round(dblHectares, 2), "#0.00") & " Hectares"
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property ShapeAreaMiText() As String
        Get
            '0.139 Square Miles
            Dim dblSqMiles As Double = m_shapeAreaKm * BA_SQKm_To_SQMile
            Return Format(Math.Round(dblSqMiles, 3), "#0.000") & " Square Miles"
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    Public Property MinElevText() As String
        Get
            Return m_minElev
        End Get
        Set(value As String)
            m_minElev = value
        End Set
    End Property

    Public Property MaxElevText() As String
        Get
            Return m_maxElev
        End Get
        Set(value As String)
            m_maxElev = value
        End Set
    End Property

    Public Property UseBufferDistance() As Boolean
        Get
            Return m_useBufferDistance
        End Get
        Set(value As Boolean)
            m_useBufferDistance = value
        End Set
    End Property

    Public Property BufferDistance() As Double
        Get
            Return m_bufferDistance
        End Get
        Set(value As Double)
            m_bufferDistance = value
        End Set
    End Property

    Public Property BufferDistanceText() As String
        Get
            Return Format(Math.Round(m_bufferDistance, 2), "#0.00")
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property

    <XmlIgnore()> Public Property BufferUnits() As esriUnits
        Get
            Return m_bufferUnits
        End Get
        Set(value As esriUnits)
            m_bufferUnits = value
        End Set
    End Property

    Public Property BufferUnitsText() As String
        Get
            Dim unitsText As String = m_bufferUnits.ToString
            If Left(unitsText, 4).ToLower = "esri" Then
                unitsText = unitsText.Remove(0, Len("esri"))
            End If
            Return unitsText
        End Get
        Set(ByVal value As String)
            m_bufferUnits = BA_GetEsriUnits(value)
        End Set
    End Property

    Public Property UseUpperRange() As Boolean
        Get
            Return m_useUpperRange
        End Get
        Set(value As Boolean)
            m_useUpperRange = value
        End Set
    End Property

    Public Property UpperRange As Double
        Get
            Return m_upperRange
        End Get
        Set(value As Double)
            m_upperRange = value
        End Set
    End Property

    Public Property UpperRangeText() As String
        Get
            Return m_upperRangeText
        End Get
        Set(value As String)
            m_upperRangeText = value
        End Set
    End Property

    <XmlIgnore()> Public Property ElevUnits() As esriUnits
        Get
            Return m_elevUnits
        End Get
        Set(value As esriUnits)
            m_elevUnits = value
        End Set
    End Property

    Public Property ElevUnitsText() As String
        Get
            Dim unitsText As String = m_elevUnits.ToString
            If Left(unitsText, 4).ToLower = "esri" Then
                unitsText = unitsText.Remove(0, Len("esri"))
            End If
            Return unitsText
        End Get
        Set(ByVal value As String)
            m_elevUnits = BA_GetEsriUnits(value)
        End Set
    End Property

    Public Property UseLowerRange() As Boolean
        Get
            Return m_useLowerRange
        End Get
        Set(value As Boolean)
            m_useLowerRange = value
        End Set
    End Property

    Public Property LowerRange As Double
        Get
            Return m_lowerRange
        End Get
        Set(value As Double)
            m_lowerRange = value
        End Set
    End Property

    Public Property LowerRangeText() As String
        Get
            Return m_lowerRangeText
        End Get
        Set(value As String)
            m_lowerRangeText = value
        End Set
    End Property

    Public Property ReportTitle() As String
        Get
            Return m_reportTitle
        End Get
        Set(ByVal value As String)
            m_reportTitle = value
        End Set
    End Property

    Public Property Scenario1Title() As String
        Get
            Return m_scenario1Title
        End Get
        Set(ByVal value As String)
            m_scenario1Title = value
        End Set
    End Property

    Public Property Scenario1Sites() As Site()
        Get
            Return m_scenario1Sites
        End Get
        Set(ByVal value As Site())
            ReDim m_scenario1Sites(value.GetUpperBound(0))
            For i As Integer = 0 To value.GetUpperBound(0)
                m_scenario1Sites(i) = value(i)
            Next
        End Set
    End Property

    Public Property Scenario2Title() As String
        Get
            Return m_scenario2Title
        End Get
        Set(ByVal value As String)
            m_scenario2Title = value
        End Set
    End Property

    Public Property Scenario2Sites() As Site()
        Get
            Return m_scenario2Sites
        End Get
        Set(ByVal value As Site())
            ReDim m_scenario2Sites(value.GetUpperBound(0))
            For i As Integer = 0 To value.GetUpperBound(0)
                m_scenario2Sites(i) = value(i)
            Next
        End Set
    End Property

    Public Property AreaStatistics As AreaStatistics
        Get
            Return m_areaStatistics
        End Get
        Set(value As AreaStatistics)
            m_areaStatistics = value
        End Set
    End Property

End Class
