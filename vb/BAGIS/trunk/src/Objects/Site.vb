Imports System.Xml.Serialization

Public Class Site
    Dim m_objectId As Integer
    Dim m_includeInAnalysis As Boolean
    Dim m_name As String
    Dim m_siteType As SiteType
    Dim m_elevation As Double
    Dim m_elevText As String
    Dim m_upperElevText As String
    Dim m_lowerElevText As String

    ' Required for de-serialization. Do not use.
    Sub New()
        MyBase.New()
    End Sub

    Sub New(ByVal objectId As Integer, ByVal name As String, ByVal siteType As SiteType, ByVal elevation As Double, _
            ByVal includeInAnalysis As Boolean)
        m_objectId = objectId
        m_name = name
        m_siteType = siteType
        m_elevation = elevation
        m_includeInAnalysis = includeInAnalysis
    End Sub

    Public Property ObjectId As Integer
        Get
            Return m_objectId
        End Get
        Set(value As Integer)
            m_objectId = value
        End Set
    End Property

    Public Property IncludeInAnalysis As Boolean
        Get
            Return m_includeInAnalysis
        End Get
        Set(value As Boolean)
            m_includeInAnalysis = value
        End Set
    End Property

    Public Property Name As String
        Get
            Return m_name
        End Get
        Set(value As String)
            m_name = value
        End Set
    End Property

    <XmlIgnore()> Public ReadOnly Property SiteType As SiteType
        Get
            Return m_siteType
        End Get
    End Property

    Public Property SiteTypeText As String
        Get
            Return m_siteType.ToString
        End Get
        Set(value As String)
            m_siteType = BA_GetSiteType(value)
        End Set
    End Property

    Public Property Elevation As Double
        Get
            Return m_elevation
        End Get
        Set(value As Double)
            m_elevation = value
        End Set
    End Property

    'This property is only used to display on the html report
    'The elevation values displayed on the grid are calculated as part of the
    'application to keep the units straight
    Public Property ElevationText As String
        Get
            Return m_elevText
        End Get
        Set(value As String)
            m_elevText = value
        End Set
    End Property

    'This property is only used to display on the html report
    'The elevation values displayed on the grid are calculated as part of the
    'application to keep the units straight
    Public Property UpperElevText As String
        Get
            Return m_upperElevText
        End Get
        Set(value As String)
            m_upperElevText = value
        End Set
    End Property

    'This property is only used to display on the html report
    'The elevation values displayed on the grid are calculated as part of the
    'application to keep the units straight
    Public Property LowerElevText As String
        Get
            Return m_lowerElevText
        End Get
        Set(value As String)
            m_lowerElevText = value
        End Set
    End Property

    'Override the default equals function
    Public Overloads Overrides Function Equals(obj As Object) As Boolean

        If obj Is Nothing OrElse Not Me.GetType() Is obj.GetType() Then
            Return False
        End If

        Dim s As Site = CType(obj, Site)
        If s.SiteType = Me.SiteType Then
            If s.ObjectId = Me.ObjectId Then
                Return True
            End If
        End If
        Return False
    End Function


End Class
