Imports BAGIS_ClassLibrary

Public Class ModelParameter
    Inherits SerializableData

    Dim m_id As Integer
    Dim m_name As String
    Dim m_value As Object

    ' Required for de-serialization. Do not use.
    Sub New()
        MyBase.new()
    End Sub

    ' This is the constructor to use
    Sub New(ByVal id As Integer, ByVal name As String, ByVal value As Object)
        m_id = id
        m_name = name
        m_value = value
    End Sub

    ' Use when extracting from IArray
    Sub New(ByVal name As String)
        m_name = name
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

    ' Display name of the parameter
    Public Property Name() As String
        Get
            Return m_name
        End Get
        Set(ByVal value As String)
            m_name = value
        End Set
    End Property

    ' Value of the parameter
    Public Property Value() As Object
        Get
            Return m_value
        End Get
        Set(ByVal value As Object)
            m_value = value
        End Set
    End Property

End Class
