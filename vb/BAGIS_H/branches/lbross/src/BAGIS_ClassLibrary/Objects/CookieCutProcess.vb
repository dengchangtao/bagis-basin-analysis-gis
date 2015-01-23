Imports System.Xml.Serialization
Imports BAGIS_ClassLibrary

Public Class CookieCutProcess

    Dim m_cookieCutName As String
    Dim m_cookieCutPath As String
    Dim m_cookieCutField As String
    Dim m_selectedValues As Long()
    Dim m_cookieCutIsHru As Boolean
    Dim m_hruName As String
    Dim m_mode As String
    Dim m_preserveAoiBoundary As Boolean

    ' Constructor with arguments to create the object
    Sub New(ByVal pMode As String, ByVal cookieCutPath As String, ByVal cookieCutName As String, _
            ByVal cookieCutField As String, ByVal selectedValues As Long(), ByVal cookieCutIsHru As Boolean, _
            ByVal preserveAoiBoundary As Boolean)
        m_mode = pMode
        m_cookieCutPath = cookieCutPath
        m_cookieCutName = cookieCutName
        m_cookieCutField = cookieCutField
        m_selectedValues = selectedValues
        m_cookieCutIsHru = cookieCutIsHru
        m_preserveAoiBoundary = preserveAoiBoundary
    End Sub

    ' Required for de-serialization. Do not use.
    Sub New()
        MyBase.New()
    End Sub

    Public Property Mode() As String
        Get
            Return m_mode
        End Get
        Set(ByVal value As String)
            m_mode = value
        End Set
    End Property

    Public Property CookieCutPath() As String
        Get
            Return m_cookieCutPath
        End Get
        Set(ByVal value As String)
            m_cookieCutPath = value
        End Set
    End Property

    Public Property CookieCutName() As String
        Get
            Return m_cookieCutName
        End Get
        Set(ByVal value As String)
            m_cookieCutName = value
        End Set
    End Property

    Public Property CookieCutField() As String
        Get
            Return m_cookieCutField
        End Get
        Set(ByVal value As String)
            m_cookieCutField = value
        End Set
    End Property

    Public Property SelectedValues() As Long()
        Get
            Return m_selectedValues
        End Get
        Set(ByVal value As Long())
            m_selectedValues = value
        End Set
    End Property

    Public Property CookieCutIsHru() As Boolean
        Get
            Return m_cookieCutIsHru
        End Get
        Set(ByVal value As Boolean)
            m_cookieCutIsHru = value
        End Set
    End Property

    Public Property CookieCutHruName() As String
        Get
            Return m_hruName
        End Get
        Set(ByVal value As String)
            m_hruName = value
        End Set
    End Property

    Public Property PreserveAoiBoundary() As Boolean
        Get
            Return m_preserveAoiBoundary
        End Get
        Set(ByVal value As Boolean)
            m_preserveAoiBoundary = value
        End Set
    End Property

    Public ReadOnly Property HruPath() As String
        Get
            If m_cookieCutIsHru = False Then
                Return Nothing
            Else
                ' Remove trailing backslash
                Dim path1 As String = BA_StandardizePathString(m_cookieCutPath)
                ' Extract .gdb portion of path
                Dim path2 As String = "Nothing"
                Dim tempName As String = BA_GetBareName(path1, path2)
                ' Remove trailing backslash
                path2 = BA_StandardizePathString(path2)
                Return path2
            End If
        End Get
    End Property

End Class
