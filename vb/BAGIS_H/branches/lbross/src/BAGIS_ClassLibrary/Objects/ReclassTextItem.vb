Imports System.Xml.Serialization

Public Class ReclassTextItem
    Inherits SerializableData

    Dim m_fromValue As String
    Dim m_outputValue As Long

    Sub New()
        MyBase.New()
    End Sub

    Sub New(ByVal fromValue As String, ByVal outputValue As Double)
        MyBase.New()
        m_fromValue = fromValue
        m_outputValue = outputValue
    End Sub

    Public Property FromValue() As String
        Get
            Return m_fromValue
        End Get
        Set(ByVal value As String)
            m_fromValue = value
        End Set
    End Property

    Public Property OutputValue() As Long
        Get
            Return m_outputValue
        End Get
        Set(ByVal value As Long)
            m_outputValue = value
        End Set
    End Property

End Class
