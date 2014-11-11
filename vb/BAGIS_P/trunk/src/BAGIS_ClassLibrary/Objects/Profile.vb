Imports BAGIS_ClassLibrary

Public Class Profile
    Inherits SerializableData
    Implements IComparable

    'Note that we use LogProfile object to save the results of a parameter calculation
    'If a new field is added that we want to add to the log, we also need to add it to the
    'LogProfile object in the BAGIS_ClassLibrary
    Dim m_id As Integer
    Dim m_name As String
    Dim m_profileClass As ProfileClass
    Dim m_description As String
    Dim m_version As String
    Dim m_xmlFileName As String
    Dim m_methodNames As List(Of String)

    ' Required for de-serialization. Do not use.
    Sub New()
        MyBase.new()
    End Sub

    Sub New(ByVal id As Integer, ByVal name As String, ByVal profileClass As ProfileClass, _
            ByVal description As String, ByVal version As String)
        m_id = id
        m_name = name
        m_profileClass = profileClass
        m_description = description
        m_version = version
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

    Public Property ProfileClass() As ProfileClass
        Get
            Return m_profileClass
        End Get
        Set(ByVal value As ProfileClass)
            m_profileClass = value
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

    'XML file name: profile source
    Public Property XmlFileName() As String
        Get
            Return m_xmlFileName
        End Get
        Set(ByVal value As String)
            m_xmlFileName = value
        End Set
    End Property

    ' List of model parameters related to the method
    Public Property MethodNames() As List(Of String)
        Get
            Return m_methodNames
        End Get
        Set(ByVal value As List(Of String))
            m_methodNames = New List(Of String)
            If value IsNot Nothing Then
                If value.Count > 0 Then
                    m_methodNames.AddRange(value)
                End If
            End If
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
