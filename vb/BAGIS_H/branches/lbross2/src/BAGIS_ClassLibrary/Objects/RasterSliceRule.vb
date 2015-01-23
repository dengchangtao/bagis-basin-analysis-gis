Imports ESRI.ArcGIS.GeoAnalyst
Imports ESRI.ArcGIS.Geodatabase
Imports System.Xml.Serialization
Imports BAGIS_ClassLibrary

Public Class RasterSliceRule
    Inherits SerializableData
    Implements IRule, IComparable

    Dim m_zoneCount As Long
    Dim m_baseZone As Integer
    Dim m_sliceType As esriGeoAnalysisSliceEnum
    Dim m_inputLayerName As String
    Dim m_inputFolderPath As String
    Dim m_outputDatasetName As String
    Dim m_defined As Boolean
    Dim m_status As FactorStatus
    Dim m_ruleType As HruRuleType
    Dim m_id As Integer

    Sub New(ByVal layerName As String, ByVal zoneCount As Long, _
            ByVal baseZone As Integer, ByVal sliceType As esriGeoAnalysisSliceEnum, _
            ByVal inputFolderPath As String, ByVal id As Integer)
        MyBase.New()
        m_inputLayerName = layerName
        m_ruleType = HruRuleType.RasterSlices
        m_defined = True
        m_zoneCount = zoneCount
        m_baseZone = baseZone
        m_sliceType = sliceType
        m_inputFolderPath = inputFolderPath
        m_id = id
        m_outputDatasetName = "r" & m_id.ToString("000")
    End Sub

    ' Required for de-serialization. Do not use.
    Sub New()
        MyBase.New()
    End Sub

    Public Property RuleId() As Integer Implements IRule.RuleId
        Get
            Return m_id
        End Get
        Set(ByVal value As Integer)
            m_id = value
        End Set
    End Property

    Public Property Defined() As Boolean Implements IRule.Defined
        Get
            Return m_defined
        End Get
        Set(ByVal value As Boolean)
            m_defined = value
        End Set
    End Property

    Public Property FactorStatus() As FactorStatus Implements IRule.FactorStatus
        Get
            Return m_status
        End Get
        Set(ByVal value As FactorStatus)
            m_status = value
        End Set
    End Property

    Public Property InputLayerName() As String Implements IRule.InputLayerName
        Get
            Return m_inputLayerName
        End Get
        Set(ByVal value As String)
            m_inputLayerName = value
        End Set
    End Property

    Public Property InputFolderPath() As String Implements IRule.InputFolderPath
        Get
            Return m_inputFolderPath
        End Get
        Set(ByVal value As String)
            m_inputFolderPath = value
        End Set
    End Property

    Public Property OutputDatasetName() As String Implements IRule.OutputDatasetName
        Get
            Return m_outputDatasetName
        End Get
        Set(ByVal value As String)
            m_outputDatasetName = value
        End Set
    End Property

    <XmlIgnore()> Public Property RuleType() As HruRuleType
        Get
            Return m_ruleType
        End Get
        Set(ByVal value As HruRuleType)
            m_ruleType = value
        End Set
    End Property

    Public Property RuleTypeText() As String Implements IRule.RuleTypeText
        Get
            Return BA_EnumDescription(m_ruleType)
        End Get
        Set(ByVal value As String)
            m_ruleType = BA_GetRuleType(value)
        End Set
    End Property

    Public Property ZoneCount() As Long
        Get
            Return m_zoneCount
        End Get
        Set(ByVal value As Long)
            m_zoneCount = value
        End Set
    End Property

    Public Property BaseZone() As Integer
        Get
            Return m_baseZone
        End Get
        Set(ByVal value As Integer)
            m_baseZone = value
        End Set
    End Property

    Public ReadOnly Property SliceType() As esriGeoAnalysisSliceEnum
        Get
            Return m_sliceType
        End Get
    End Property

    Public Property SliceTypeText() As String
        Get
            Return BA_GetEsriSliceTypeText(m_sliceType)
        End Get
        Set(ByVal value As String)
            m_sliceType = BA_GetEsriSliceType(value)
        End Set
    End Property

    Public Function CompareTo(ByVal obj As Object) As Integer Implements IComparable.CompareTo
        Dim otherRule As IRule = TryCast(obj, IRule)
        If otherRule IsNot Nothing Then
            Return Me.RuleId.CompareTo(otherRule.RuleId)
        Else
            Throw New ArgumentException("Object is not a Rule")
        End If
    End Function

End Class
