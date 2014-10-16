Public Class SubAOI

    Private m_name As String
    Private m_maxAccumValue As Double
    Private m_tempLayerName As String
    Private m_tempFilePath As String
    Private m_combineValues As IList(Of Integer)
    Private m_id As Integer
    Private m_gaugeNumber As String

    Public Sub New(ByVal name As String)
        m_name = name
    End Sub

    Public ReadOnly Property Name As String
        Get
            Return m_name
        End Get
    End Property

    Public Property MaxAccumValue() As Double
        Get
            Return m_maxAccumValue
        End Get
        Set(value As Double)
            m_maxAccumValue = value
        End Set
    End Property

    Public Property TempLayerName() As String
        Get
            Return m_tempLayerName
        End Get
        Set(value As String)
            m_tempLayerName = value
        End Set
    End Property

    Public Property TempFilePath() As String
        Get
            Return m_tempFilePath
        End Get
        Set(value As String)
            m_tempFilePath = value
        End Set
    End Property

    Public Property CombineValueList() As IList(Of Integer)
        Get
            Return m_combineValues
        End Get
        Set(value As IList(Of Integer))
            m_combineValues = value
        End Set
    End Property

    Public Property Id() As Integer
        Get
            Return m_id
        End Get
        Set(value As Integer)
            m_id = value
        End Set
    End Property

    Public Property GaugeNumber As String
        Get
            Return m_gaugeNumber
        End Get
        Set(value As String)
            m_gaugeNumber = value
        End Set
    End Property

    'Define a comparer that sorts the SubAoi by the max accumulation value
    Private Class maxAccumAscendingHelper : Implements IComparer
        Function Compare(ByVal a As Object, ByVal b As Object) As Integer Implements IComparer.Compare
            Dim s1 As SubAOI = CType(a, SubAOI)
            Dim s2 As SubAOI = CType(b, SubAOI)

            If (s1.m_maxAccumValue > s2.m_maxAccumValue) Then
                Return 1
            End If

            If (s1.m_maxAccumValue < s2.m_maxAccumValue) Then
                Return -1
            Else
                Return 0
            End If
        End Function
    End Class

    'Make the comparer publicly available for VB .NET sorting functions
    'such as Array.sort
    Public Shared Function maxAccumAscending() As IComparer
        Return CType(New maxAccumAscendingHelper(), IComparer)
    End Function

End Class
