Imports BAGIS_ClassLibrary

Public Class LayerListItem

    Dim m_name As String
    Dim m_value As String
    Dim m_layerType As LayerType
    Dim m_isDiscrete As Boolean

    Public Sub New(ByVal name As String, ByVal value As String, ByVal layerType As LayerType, _
                   ByVal isDiscrete As Boolean)
        m_name = name
        m_value = value
        m_layerType = layerType
        m_isDiscrete = isDiscrete
    End Sub

    Public ReadOnly Property Name() As String
        Get
            Return m_name
        End Get
    End Property

    Public ReadOnly Property Value() As String
        Get
            Return m_value
        End Get
    End Property

    Public ReadOnly Property LayerType() As LayerType
        Get
            Return m_layerType
        End Get
    End Property

    Public ReadOnly Property IsDiscrete() As Boolean
        Get
            Return m_isDiscrete
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return m_name
    End Function



End Class
