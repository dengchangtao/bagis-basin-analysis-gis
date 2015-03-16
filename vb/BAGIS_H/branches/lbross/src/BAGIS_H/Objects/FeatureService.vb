Public Class FeatureService

    Private m_name As String
    Private m_featureServiceFields As FeatureServiceField()

    Property name As String
        Get
            Return m_name
        End Get
        Set(value As String)
            m_name = value
        End Set
    End Property

    Property fields() As FeatureServiceField()
        Get
            Return m_featureServiceFields
        End Get
        Set(ByVal value As FeatureServiceField())
            m_featureServiceFields = value
        End Set
    End Property

End Class
