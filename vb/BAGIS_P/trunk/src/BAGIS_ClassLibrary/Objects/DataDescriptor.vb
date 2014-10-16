Imports System.Xml.Serialization

Public Class DataDescriptor
    Inherits SerializableData

    Dim m_layerName As MapsLayerName
    Dim m_units As MeasurementUnit
    Dim m_version As String

    <XmlIgnore()> Public Property LayerName() As MapsLayerName
        Get
            Return m_layerName
        End Get
        Set(ByVal value As MapsLayerName)
            m_layerName = value
        End Set

    End Property

    Public Property LayerNameText() As String
        Get
            Return BA_EnumDescription(m_layerName)
        End Get
        Set(ByVal value As String)
            m_layerName = BA_GetMapsLayerName(value)
        End Set
    End Property

    <XmlIgnore()> Public Property Units() As MeasurementUnit
        Get
            Return m_units
        End Get
        Set(ByVal value As MeasurementUnit)
            m_units = value
        End Set
    End Property

    Public Property UnitsText() As String
        Get
            Return BA_EnumDescription(m_units)
        End Get
        Set(ByVal value As String)
            m_units = BA_GetMeasurementUnit(value)
        End Set
    End Property

    Property Version() As String
        Get
            Return m_version
        End Get
        Set(ByVal value As String)
            m_version = value
        End Set
    End Property

End Class
