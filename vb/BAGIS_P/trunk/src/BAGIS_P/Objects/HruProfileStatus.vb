Imports BAGIS_ClassLibrary

Public Class HruProfileStatus
    Inherits SerializableData

    'These are parallel lists since you can't serialize a HashTable
    'They should always have the same number of elements
    'One completion date for each profile name
    Dim m_profileNames As List(Of String)
    Dim m_completionDates As List(Of Date)

    ' Required for de-serialization. Do not use.
    Sub New()
        MyBase.new()
    End Sub

    ' List of profile names where parameters have been calculated for the HRU
    Public Property ProfileNames() As List(Of String)
        Get
            Return m_profileNames
        End Get
        Set(ByVal value As List(Of String))
            m_profileNames = New List(Of String)
            If value IsNot Nothing Then
                If value.Count > 0 Then
                    m_profileNames.AddRange(value)
                End If
            End If
        End Set
    End Property

    ' List of last date profile was calculated for the HRU
    Public Property CompletionDates() As List(Of Date)
        Get
            Return m_completionDates
        End Get
        Set(ByVal value As List(Of Date))
            m_completionDates = New List(Of Date)
            If value IsNot Nothing Then
                If value.Count > 0 Then
                    m_completionDates.AddRange(value)
                End If
            End If
        End Set
    End Property

    'Returns the number of completed profiles for this Hru
    Public ReadOnly Property CompletedCount As Integer
        Get
            If m_profileNames Is Nothing Then
                Return 0
            Else
                Return m_profileNames.Count
            End If
        End Get
    End Property
End Class
