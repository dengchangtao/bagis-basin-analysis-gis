Public Class ParameterTable

    Dim m_name As String
    Dim m_dimension1 As String
    Dim m_dimension2 As String
    Dim m_values(0, 0) As String
    Dim m_headers(0) As String


    Public Sub New(ByVal pName As String, ByVal dimension1 As String, ByVal values(,) As String, ByVal headers() As String)
        m_name = pName
        m_dimension1 = dimension1
        If values IsNot Nothing Then
            ReDim m_values(values.GetUpperBound(0), values.GetUpperBound(1))
            Array.Copy(values, m_values, values.Length)
        End If
        If headers IsNot Nothing Then
            ReDim m_headers(headers.GetUpperBound(0))
            Array.Copy(headers, m_headers, headers.Length)
        End If
    End Sub

    Public Sub New(ByVal pName As String, ByVal dimension1 As String, ByVal dimension2 As String, ByVal values(,) As String, ByVal headers() As String)
        m_name = pName
        m_dimension1 = dimension1
        m_dimension2 = dimension2
        If values IsNot Nothing Then
            ReDim m_values(values.GetUpperBound(0), values.GetUpperBound(1))
            Array.Copy(values, m_values, values.Length)
        End If
        If headers IsNot Nothing Then
            ReDim m_headers(headers.GetUpperBound(0))
            Array.Copy(headers, m_headers, headers.Length)
        End If
    End Sub

    Public ReadOnly Property Name As String
        Get
            Return m_name
        End Get
    End Property

    Public ReadOnly Property Dimension1 As String
        Get
            Return m_dimension1
        End Get
    End Property

    Public ReadOnly Property Dimension2 As String
        Get
            Return m_dimension2
        End Get
    End Property

    Public ReadOnly Property Values As String(,)
        Get
            Return m_values
        End Get
    End Property

    Public ReadOnly Property Headers As String()
        Get
            Return m_headers
        End Get
    End Property
End Class
