Imports BAGIS_ClassLibrary

Public Class Settings
    Inherits SerializableData

    Dim m_dataSourceList As List(Of DataSource)


    ' Required for de-serialization. Do not use.
    Sub New()
        MyBase.new()
    End Sub

    ' List of data sources
    Public Property DataSources() As List(Of DataSource)
        Get
            Return m_dataSourceList
        End Get
        Set(ByVal value As List(Of DataSource))
            m_dataSourceList = New List(Of DataSource)
            If value IsNot Nothing Then
                If value.Count > 0 Then
                    m_dataSourceList.AddRange(value)
                End If
            End If
        End Set
    End Property

End Class
