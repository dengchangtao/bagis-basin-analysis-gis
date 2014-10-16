Public Class Zone
    Implements IComparable

    Dim m_id As Long

    Sub New(ByVal id As Long)
        MyBase.New()
        m_id = id
    End Sub

    ' Required for de-serialization. Do not use.
    Sub New()
        MyBase.New()
    End Sub

    ' Returns unique id of the zone
    Public Property Id() As Long
        Get
            Return m_id
        End Get
        Set(ByVal value As Long)
            m_id = value
        End Set
    End Property

    Public Function CompareTo(ByVal obj As Object) As Integer Implements IComparable.CompareTo
        Dim otherZone As Zone = TryCast(obj, Zone)
        If otherZone IsNot Nothing Then
            Return Me.Id.CompareTo(otherZone.Id)
        Else
            Throw New ArgumentException("Object is not a Zone")
        End If
    End Function
End Class
