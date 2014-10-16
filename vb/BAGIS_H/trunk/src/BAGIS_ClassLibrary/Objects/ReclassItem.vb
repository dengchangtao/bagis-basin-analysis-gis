Imports System.Xml.Serialization

Public Class ReclassItem
    Inherits SerializableData

    Dim m_fromValue As Double
    Dim m_fromDescr As String
    Dim m_toValue As Double
    Dim m_outputValue As Long
    Dim m_outputDescr As String

    Sub New()
        MyBase.New()
    End Sub

    Sub New(ByVal fromValue As Double, ByVal toValue As Double, ByVal outputValue As Double)
        MyBase.New()
        m_fromValue = fromValue
        m_toValue = toValue
        m_outputValue = outputValue
    End Sub

    Sub New(ByVal fromValue As Double, ByVal fromDescr As String, ByVal toValue As Double, _
            ByVal outputValue As Double, ByVal outputDescr As String)
        MyBase.New()
        m_fromValue = fromValue
        m_fromDescr = fromDescr
        m_toValue = toValue
        m_outputValue = outputValue
        m_outputDescr = outputDescr
    End Sub

    Public Property FromValue() As Double
        Get
            Return m_fromValue
        End Get
        Set(ByVal value As Double)
            m_fromValue = value
        End Set
    End Property

    Public Property FromDescr() As String
        Get
            Return m_fromDescr
        End Get
        Set(ByVal value As String)
            m_fromDescr = value
        End Set
    End Property

    Public Property ToValue() As Double
        Get
            Return m_toValue
        End Get
        Set(ByVal value As Double)
            m_toValue = value
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

    Public Property OutputDescr() As String
        Get
            Return m_outputDescr
        End Get
        Set(ByVal value As String)
            m_outputDescr = value
        End Set
    End Property

End Class
