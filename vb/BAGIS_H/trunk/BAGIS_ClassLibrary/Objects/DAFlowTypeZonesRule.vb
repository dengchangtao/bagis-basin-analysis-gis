Imports ESRI.ArcGIS.GeoAnalyst
Imports ESRI.ArcGIS.Geodatabase
Imports System.Xml.Serialization
Imports BAGIS_ClassLibrary

Public Class DAFlowTypeZonesRule
    Inherits SerializableData
    Implements IRule, IComparable

    Dim m_byParameter As DAFlowByParam
    Dim m_hruNumber As Long
    Dim m_hruRow As Long
    Dim m_hruCol As Long
    Dim m_hruXSize As Double
    Dim m_hruYSize As Double
    Dim m_sizeUnit As Double
    Dim m_units As MeasurementUnit
    Dim m_inputLayerName As String
    Dim m_inputFolderPath As String
    Dim m_outputDatasetName As String
    Dim m_defined As Boolean
    Dim m_status As FactorStatus
    Dim m_ruleType As HruRuleType
    Dim m_id As Integer

    Sub New(ByVal layerName As String, ByVal flowParam As DAFlowByParam, _
            ByVal hruNumber As Long, ByVal hruRow As Long, ByVal hruCol As Long, _
            ByVal hruXSize As Double, ByVal hruYSize As Double, ByVal sizeUnit As Double, _
            ByVal units As MeasurementUnit, ByVal inputFolderPath As String, ByVal id As Integer)
        MyBase.New()
        m_inputLayerName = layerName
        m_byParameter = flowParam
        m_hruNumber = hruNumber
        m_hruRow = hruRow
        m_hruCol = hruCol
        m_hruXSize = hruXSize
        m_hruYSize = hruYSize
        m_units = units
        m_sizeUnit = sizeUnit
        m_ruleType = HruRuleType.DAFlowTypeZones
        m_defined = True
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

    Public Property ByParameter() As DAFlowByParam
        Get
            Return m_byParameter
        End Get
        Set(ByVal value As DAFlowByParam)
            m_byParameter = value
        End Set
    End Property

    Public Property HruNumber() As Long
        Get
            Return m_hruNumber
        End Get
        Set(ByVal value As Long)
            m_hruNumber = value
        End Set
    End Property

    Public Property HruRow() As Long
        Get
            Return m_hruRow
        End Get
        Set(ByVal value As Long)
            m_hruRow = value
        End Set
    End Property

    Public Property HruCol() As Long
        Get
            Return m_hruCol
        End Get
        Set(ByVal value As Long)
            m_hruCol = value
        End Set
    End Property

    Public Property HruXSize() As Double
        Get
            Return m_hruXSize
        End Get
        Set(ByVal value As Double)
            m_hruXSize = value
        End Set
    End Property

    Public Property HruYSize() As Double
        Get
            Return m_hruYSize
        End Get
        Set(ByVal value As Double)
            m_hruYSize = value
        End Set
    End Property

    <XmlIgnore()> Property MeasurementUnits() As MeasurementUnit
        Get
            Return m_units
        End Get
        Set(ByVal value As MeasurementUnit)
            m_units = value
        End Set
    End Property

    Public Property MeasurementUnitsText() As String
        Get
            Return BA_EnumDescription(m_units)
        End Get
        Set(ByVal value As String)
            m_units = BA_GetMeasurementUnit(value)
        End Set
    End Property

    Public Property SizeUnit() As Double
        Get
            Return m_sizeUnit
        End Get
        Set(ByVal value As Double)
            m_sizeUnit = value
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
