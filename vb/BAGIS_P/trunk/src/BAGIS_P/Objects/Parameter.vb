Public Class Parameter

    Dim m_name As String
    'I am using an array here to accomodate parameters with an array of values
    Dim m_value As String()
    Dim m_isDimension As Boolean
    Dim m_is2Dimensional As Boolean
    Dim m_2DValue As String()()

    Public Sub New(ByVal name As String, ByVal value As String(), ByVal isDimension As Boolean)
        m_name = name
        ReDim m_value(value.GetLength(0))
        Array.Copy(value, m_value, value.GetLength(0))
        m_isDimension = isDimension
    End Sub

    Public Sub New(ByVal name As String, ByVal value As String(), ByVal is2Dimensional As Boolean, ByVal TwoDValue As String()())
        m_name = name
        m_value = value
        m_is2Dimensional = is2Dimensional
        m_2DValue = TwoDValue
    End Sub

    Public ReadOnly Property name As String
        Get
            Return m_name
        End Get
    End Property

    Public Property value As String()
        Get
            Return m_value
        End Get
        Set(value As String())
            ReDim m_value(value.GetLength(0))
            m_value = value
        End Set
    End Property

    Public Property isDimension As Boolean
        Get
            Return m_isDimension
        End Get
        Set(value As Boolean)
            m_isDimension = value
        End Set
    End Property

    Public ReadOnly Property is2Dimensional As Boolean
        Get
            Return m_is2Dimensional
        End Get
    End Property
End Class
