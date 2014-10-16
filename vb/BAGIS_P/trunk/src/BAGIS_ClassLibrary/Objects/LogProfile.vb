Imports BAGIS_ClassLibrary

Public Class LogProfile
    Inherits SerializableData
    Implements IComparable

    Dim m_id As Integer
    Dim m_name As String
    Dim m_description As String
    Dim m_version As String
    Dim m_hruPath As String
    Dim m_dateCompleted As Date
    Dim m_noDataValue As String
    Dim m_methods As Method()
    Dim m_hruMethods As Method()

    ' Required for de-serialization. Do not use.
    Sub New()
        MyBase.new()
    End Sub

    Sub New(ByVal id As Integer, ByVal name As String, ByVal description As String, ByVal version As String, _
            ByVal hruPath As String, ByVal dateCompleted As Date, ByVal noDataValue As String)
        m_id = id
        m_name = name
        m_description = description
        m_version = version
        m_hruPath = hruPath
        m_dateCompleted = dateCompleted
        m_noDataValue = noDataValue
    End Sub

    ' Unique parameter id
    Public Property Id() As Integer
        Get
            Return m_id
        End Get
        Set(ByVal value As Integer)
            m_id = value
        End Set
    End Property

    ' Name of profile
    Public Property Name() As String
        Get
            Return m_name
        End Get
        Set(ByVal value As String)
            m_name = value
        End Set
    End Property

    'Description of profile
    Public Property Description() As String
        Get
            Return m_description
        End Get
        Set(ByVal value As String)
            m_description = value
        End Set
    End Property

    'Version of application profile was created under
    Public Property Version() As String
        Get
            Return m_version
        End Get
        Set(ByVal value As String)
            m_version = value
        End Set
    End Property

    'Name of HRU used to generate these parameters
    Public Property HruPath() As String
        Get
            Return m_hruPath
        End Get
        Set(ByVal value As String)
            m_hruPath = value
        End Set
    End Property

    ' List of methods used by the profile (contains configuration information)
    Public Property Methods() As Method()
        Get
            Return m_methods
        End Get
        Set(ByVal value As Method())
            m_methods = value
        End Set
    End Property

    ' List of methods used by the profile (contains status information)
    Public Property HruMethods() As Method()
        Get
            Return m_hruMethods
        End Get
        Set(ByVal value As Method())
            m_hruMethods = value
        End Set
    End Property

    ' Timestamp for when the parameters were calculated
    Public Property DateCompleted As Date
        Get
            Return m_dateCompleted
        End Get
        Set(ByVal value As Date)
            m_dateCompleted = value
        End Set
    End Property

    Public Property NoDataValue As String
        Get
            Return m_noDataValue
        End Get
        Set(ByVal value As String)
            m_noDataValue = value
        End Set
    End Property

    Public Function CompareTo(ByVal obj As Object) As Integer Implements IComparable.CompareTo
        Dim otherProfile As Profile = TryCast(obj, Profile)
        If otherProfile IsNot Nothing Then
            Return Me.Name.CompareTo(otherProfile.Name)
        Else
            Throw New ArgumentException("Object is not a Profile")
        End If
    End Function

End Class
