Imports BAGIS_ClassLibrary
Imports System.Xml.Serialization

Public Class DataSource
    Inherits SerializableData

    Dim m_id As Integer
    Dim m_name As String
    Dim m_description As String
    Dim m_source As String
    Dim m_aoiLayer As Boolean
    '24-APR-2012 As of this date we aren't saving the data field
    'Dim m_dataField As String
    Dim m_layerType As LayerType
    Dim m_measurementUnitType As MeasurementUnitType
    Dim m_measurementUnit As MeasurementUnit
    Dim m_slopeUnit As SlopeUnit

    ' Required for de-serialization. Do not use.
    Sub New()
        MyBase.new()
    End Sub

    ' Use this when instantiating a new object
    Public Sub New(ByVal id As Integer, ByVal name As String, ByVal description As String, ByVal source As String, _
                   ByVal aoiLayer As Boolean, ByVal layerType As LayerType)
        m_id = id
        m_name = name
        m_description = description
        m_source = source
        m_aoiLayer = aoiLayer
        'm_dataField = dataField
        m_layerType = layerType
    End Sub

    Public Property Id() As Integer
        Set(ByVal value As Integer)
            m_id = value
        End Set
        Get
            Return m_id
        End Get
    End Property

    Public Property Name() As String
        Set(ByVal value As String)
            m_name = value
        End Set
        Get
            Return m_name
        End Get
    End Property

    Public Property Description() As String
        Set(ByVal value As String)
            m_description = value
        End Set
        Get
            Return m_description
        End Get
    End Property

    Public Property Source() As String
        Set(ByVal value As String)
            m_source = value
        End Set
        Get
            Return m_source
        End Get
    End Property

    Public Property AoiLayer() As Boolean
        Set(ByVal value As Boolean)
            m_aoiLayer = value
        End Set
        Get
            Return m_aoiLayer
        End Get
    End Property

    '24-APR-2012 As of this date we aren't saving the data field
    'Public Property DataField() As String
    '    Set(ByVal value As String)
    '        m_dataField = value
    '    End Set
    '    Get
    '        Return m_dataField
    '    End Get
    'End Property

    Public Property LayerType As LayerType
        Set(ByVal value As LayerType)
            m_layerType = value
        End Set
        Get
            Return m_layerType
        End Get
    End Property

    'This property will not be persisted in settings.xml since it lives in the metadata of the layer
    'We will use it, however, when working with the data sources
    'This property will be retrieved from the metadata of the layer
    <XmlIgnore()>
    Public Property MeasurementUnitType As MeasurementUnitType
        Set(ByVal value As MeasurementUnitType)
            m_measurementUnitType = value
        End Set
        Get
            Return m_measurementUnitType
        End Get
    End Property

    'This property will not be persisted in settings.xml since it lives in the metadata of the layer
    'We will use it, however, when working with the data sources
    'This property will be retrieved from the metadata of the layer
    <XmlIgnore()>
    Public Property MeasurementUnit As MeasurementUnit
        Set(ByVal value As MeasurementUnit)
            m_measurementUnit = value
        End Set
        Get
            Return m_measurementUnit
        End Get
    End Property

    'This property will not be persisted in settings.xml since it lives in the metadata of the layer
    'We will use it, however, when working with the data sources
    'This property will be retrieved from the metadata of the layer
    <XmlIgnore()>
    Public Property SlopeUnit As SlopeUnit
        Set(ByVal value As SlopeUnit)
            m_slopeUnit = value
        End Set
        Get
            Return m_slopeUnit
        End Get
    End Property

End Class
