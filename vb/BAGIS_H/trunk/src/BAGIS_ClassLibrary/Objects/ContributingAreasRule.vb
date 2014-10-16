Imports BAGIS_ClassLibrary

Public Class ContributingAreasRule
    Inherits SerializableData
    Implements IRule, IComparable

    Dim m_FACCThresholdValue As Double
    Dim m_FACCStandardDeviation As Double
    Dim m_NumberofArea As Integer
    Dim m_inputLayerName As String
    Dim m_inputFolderPath As String
    Dim m_outputDatasetName As String
    Dim m_defined As Boolean
    Dim m_status As FactorStatus
    Dim m_ruleType As HruRuleType
    Dim m_id As Integer
    Dim m_keepTempFiles As Boolean

    Sub New(ByVal AOIPath As String, ByVal FACCThresholdValue As Double, ByVal FACCStDev As Double, _
            ByVal numberofArea As Integer, ByVal keepTempFiles As Boolean, ByVal id As Integer)
        MyBase.New()
        m_inputLayerName = BA_EnumDescription(MapsLayerName.flow_accumulation) 'points to flowacc raster
        m_FACCThresholdValue = FACCThresholdValue
        m_FACCStandardDeviation = FACCStDev
        m_NumberofArea = numberofArea
        m_ruleType = HruRuleType.ContributingArea
        m_defined = True
        'input folder is the aoifolder, sub needs to get the facc and fdir path strings
        'm_inputFolderPath = AOIPath & BA_EnumDescription(PublicPath.FlowAccumulation) & "\" & BA_EnumDescription(MapsFileName.flow_accumulation)
        m_inputFolderPath = BA_GeodatabasePath(AOIPath, GeodatabaseNames.Surfaces, True) & BA_EnumDescription(MapsFileName.flow_accumulation_gdb)
        m_keepTempFiles = keepTempFiles
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

    Public Property RuleType() As HruRuleType
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

    Public Property FACCThresholdValue() As Double
        Get
            Return m_FACCThresholdValue
        End Get
        Set(ByVal value As Double)
            m_FACCThresholdValue = value
        End Set
    End Property

    Public Property FACCStandardDeviation() As Double
        Get
            Return m_FACCStandardDeviation
        End Get
        Set(ByVal value As Double)
            m_FACCStandardDeviation = value
        End Set
    End Property

    Public Property NumberofArea() As Integer
        Get
            Return m_NumberofArea
        End Get
        Set(ByVal value As Integer)
            m_NumberofArea = value
        End Set
    End Property

    Public Property KeepTemporaryFiles() As Boolean
        Get
            Return m_keepTempFiles
        End Get
        Set(ByVal value As Boolean)
            m_keepTempFiles = value
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
