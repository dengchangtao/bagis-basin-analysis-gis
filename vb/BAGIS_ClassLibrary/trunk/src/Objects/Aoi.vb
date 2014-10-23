Imports System.Xml.Serialization
Imports BAGIS_ClassLibrary

Public Class Aoi
    Inherits SerializableData

    Dim m_name As String
    Dim m_filePath As String
    Dim m_comments As String
    Dim m_applicationVersion As String
    Dim m_hrus As List(Of Hru)

    ' Use this constructor for creating new aoi objects. Because this object will
    ' be serialized/deserialized, we cannot protect the properties with private
    ' setters
    Sub New(ByVal name As String, ByVal filePath As String, ByVal comments As String, _
            ByVal applicationVersion As String)
        MyBase.New()
        m_name = name
        m_filePath = filePath
        m_comments = comments
        m_applicationVersion = applicationVersion
    End Sub

    ' Required for de-serialization. Do not use.
    Sub New()
        MyBase.new()
    End Sub

    ' Display name of the aoi
    Public Property Name() As String
        Get
            Return m_name
        End Get
        Set(ByVal value As String)
            m_name = value
        End Set
    End Property

    ' File path where the aoi was created
    Public Property FilePath() As String
        Get
            Return m_filePath
        End Get
        Set(ByVal value As String)
            m_filePath = value
        End Set
    End Property

    ' Returns comments related to this aoi
    Public Property Comments() As String
        Get
            Return m_comments
        End Get
        Set(ByVal value As String)
            m_comments = value
        End Set
    End Property

    ' Returns application version this hru was created under
    Public Property ApplicationVersion() As String
        Get
            Return m_applicationVersion
        End Get
        Set(ByVal value As String)
            m_applicationVersion = value
        End Set
    End Property

    ' List of hrus related to the aoi
    Public Property HruList() As List(Of Hru)
        Get
            Return m_hrus
        End Get
        Set(ByVal value As List(Of Hru))
            m_hrus = New List(Of Hru)
            If value IsNot Nothing Then
                If value.Count > 0 Then
                    m_hrus.AddRange(value)
                End If
            End If
        End Set
    End Property

End Class
