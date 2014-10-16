Imports ESRI.ArcGIS.GeoAnalyst
Imports ESRI.ArcGIS.Geodatabase
Imports System.Xml.Serialization
Imports BAGIS_ClassLibrary

Public Class TemplateRule
    Inherits SerializableData
    Implements IRule, IComparable

    'Unique properties of AspectTemplateRule
    Dim m_templateActions As List(Of TemplateAction)
    Dim m_templateName As String
    Dim m_isDefault As Boolean
    Dim m_missingValue As Integer
    'IRule properties implemented
    Dim m_inputLayerName As String
    Dim m_inputFolderPath As String
    Dim m_outputDatasetName As String
    Dim m_defined As Boolean
    Dim m_status As FactorStatus
    Dim m_ruleType As HruRuleType
    Dim m_id As Integer

    Sub New(ByVal ruleType As HruRuleType, ByVal templateActions As List(Of TemplateAction), _
            ByVal layerName As String, ByVal inputFolderPath As String, ByVal id As Integer)
        MyBase.New()
        m_templateActions = templateActions
        m_inputLayerName = layerName
        m_ruleType = ruleType
        m_defined = True
        m_inputFolderPath = InputFolderPath
        m_id = id
        m_outputDatasetName = "r" & m_id.ToString("000")
    End Sub

    ' Required for de-serialization. Do not use.
    Sub New()
        MyBase.New()
    End Sub

    Public Property TemplateActions() As List(Of TemplateAction)
        Get
            Return m_templateActions
        End Get
        Set(ByVal value As List(Of TemplateAction))
            m_templateActions = value
        End Set
    End Property

    Public Property TemplateName() As String
        Get
            Return m_templateName
        End Get
        Set(ByVal value As String)
            m_templateName = value
        End Set
    End Property

    Public Property IsDefault() As Boolean
        Get
            Return m_isDefault
        End Get
        Set(ByVal value As Boolean)
            m_isDefault = value
        End Set
    End Property

    Public Property MissingValue() As Integer
        Get
            Return m_missingValue
        End Get
        Set(ByVal value As Integer)
            m_missingValue = value
        End Set
    End Property

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

    Public Function CompareTo(ByVal obj As Object) As Integer Implements IComparable.CompareTo
        Dim otherRule As IRule = TryCast(obj, IRule)
        If otherRule IsNot Nothing Then
            Return Me.RuleId.CompareTo(otherRule.RuleId)
        Else
            Throw New ArgumentException("Object is not a Rule")
        End If
    End Function

End Class
