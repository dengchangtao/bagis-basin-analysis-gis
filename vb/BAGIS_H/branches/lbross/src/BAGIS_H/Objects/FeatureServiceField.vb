Public Class FeatureServiceField

    Dim m_name As String
    Dim m_alias As String
    Dim m_type As String


    Property name As String
        Get
            Return m_name
        End Get
        Set(value As String)
            m_name = value
        End Set
    End Property

    'Use brackets because alias is a keyword in VB .net
    Property [alias] As String
        Get
            Return m_alias
        End Get
        Set(value As String)
            m_alias = value
        End Set
    End Property

    Property type As String
        Get
            Return m_type
        End Get
        Set(value As String)
            m_type = value
        End Set
    End Property

    ReadOnly Property fieldType As ESRI.ArcGIS.Geodatabase.esriFieldType
        Get
            Select Case m_type
                Case "esriFieldTypeOID"
                    Return ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeOID
                Case "esriFieldTypeString"
                    Return ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString
                Case "esriFieldTypeDate"
                    Return ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDate
                Case "esriFieldTypeSmallInteger"
                    Return ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeSmallInteger
                Case "esriFieldTypeDouble"
                    Return ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeDouble
            End Select
        End Get

    End Property
End Class
