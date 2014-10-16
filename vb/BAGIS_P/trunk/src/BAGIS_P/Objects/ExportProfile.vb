Imports System.Xml.Serialization
Imports BAGIS_ClassLibrary

Public Class ExportProfile
    Inherits SerializableData

    Dim m_profile As Profile
    Dim m_aoiName As String
    Dim m_aoiPath As String
    Dim m_methodList As List(Of Method)
    Dim m_dateCopied As DateTime

    ' Required for de-serialization. Do not use.
    Sub New()
        MyBase.New()
    End Sub

    Sub New(ByVal aoiName As String, ByVal aoiPath As String, dateCopied As DateTime)
        m_aoiName = aoiName
        m_aoiPath = aoiPath
        m_dateCopied = dateCopied
    End Sub

    Public Property AoiName() As String
        Get
            Return m_aoiName
        End Get
        Set(ByVal value As String)
            m_aoiName = value
        End Set
    End Property

    Public Property AoiPath() As String
        Get
            Return m_aoiPath
        End Get
        Set(ByVal value As String)
            m_aoiPath = value
        End Set
    End Property

    Public Property Profile() As Profile
        Get
            Return m_profile
        End Get
        Set(ByVal value As Profile)
            m_profile = value
        End Set
    End Property

    Public Property MethodList() As List(Of Method)
        Get
            Return m_methodList
        End Get
        Set(ByVal value As List(Of Method))
            m_methodList = New List(Of Method)
            If value IsNot Nothing Then
                If value.Count > 0 Then
                    m_methodList.AddRange(value)
                End If
            End If
        End Set
    End Property

    Public Property DateCopied() As DateTime
        Get
            Return m_dateCopied
        End Get
        Set(ByVal value As DateTime)
            m_dateCopied = value
        End Set
    End Property

    Public Property DateCopiedText() As String
        Get
            Dim zone As System.TimeZoneInfo = System.TimeZoneInfo.Local
            Dim strDate As String = m_dateCopied.ToString("d-MMM-yyyy h:m tt ")
            Return strDate & zone.DisplayName
        End Get
        Set(value As String)
            'Do nothing; This is only for XML serialization
        End Set
    End Property
End Class
