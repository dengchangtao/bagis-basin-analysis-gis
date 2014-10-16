Imports ESRI.ArcGIS.GeoAnalyst
Imports ESRI.ArcGIS.Geodatabase
Imports System.Xml.Serialization
Imports BAGIS_ClassLibrary

Public Class PrismPrecipRule
    Inherits SerializableData
    Implements IRule, IComparable

    'Unique properties of PrismPrecipRule
    Dim m_prismDataRange As PrismDataRange
    Dim m_fMonthIdx As Integer
    Dim m_lMonthIdx As Integer
    Dim m_displayUnits As MeasurementUnit
    Dim m_dataUnits As MeasurementUnit
    Dim m_sliceType As esriGeoAnalysisSliceEnum
    Dim m_reclassItems As ReclassItem()
    'IRule properties implemented
    Dim m_inputLayerName As String
    Dim m_inputFolderPath As String
    Dim m_outputDatasetName As String
    Dim m_defined As Boolean
    Dim m_status As FactorStatus
    Dim m_ruleType As HruRuleType
    Dim m_id As Integer

    Sub New(ByVal prismDataRange As PrismDataRange, ByVal fMonth As Integer, ByVal lMonth As Integer, _
            ByVal displayUnits As MeasurementUnit, ByVal dataUnits As MeasurementUnit, _
            ByVal sliceType As esriGeoAnalysisSliceEnum, ByVal reclassItems As ReclassItem(), _
            ByVal layerName As String, ByVal inputFolderPath As String, ByVal id As Integer)
        MyBase.New()
        m_prismDataRange = prismDataRange
        m_fMonthIdx = fMonth
        m_lMonthIdx = lMonth
        m_displayUnits = displayUnits
        m_dataUnits = dataUnits
        m_sliceType = sliceType
        m_reclassItems = ReclassItems
        m_inputLayerName = layerName
        m_ruleType = HruRuleType.PrismPrecipitation
        m_defined = True
        m_inputFolderPath = InputFolderPath
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

    <XmlIgnore()> Public Property DataRange() As PrismDataRange
        Get
            Return m_prismDataRange
        End Get
        Set(ByVal value As PrismDataRange)
            m_prismDataRange = value
        End Set
    End Property

    Public Property DataRangeText() As String
        Get
            Return BA_EnumDescription(m_prismDataRange)
        End Get
        Set(ByVal value As String)
            m_prismDataRange = BA_GetPrismDataRange(value)
        End Set
    End Property

    ' Unit of measure for the size fields
    <XmlIgnore()> Public Property DisplayUnits() As MeasurementUnit
        Get
            Return m_displayUnits
        End Get
        Set(ByVal value As MeasurementUnit)
            m_displayUnits = value
        End Set
    End Property

    Public Property DisplayUnitsText() As String
        Get
            Return BA_EnumDescription(m_displayUnits)
        End Get
        Set(ByVal value As String)
            m_displayUnits = BA_GetMeasurementUnit(value)
        End Set
    End Property

    ' Unit of measure for the size fields
    <XmlIgnore()> Public Property DataUnits() As MeasurementUnit
        Get
            Return m_dataUnits
        End Get
        Set(ByVal value As MeasurementUnit)
            m_dataUnits = value
        End Set
    End Property

    Public Property DataUnitsText() As String
        Get
            Return BA_EnumDescription(m_dataUnits)
        End Get
        Set(ByVal value As String)
            m_dataUnits = BA_GetMeasurementUnit(value)
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

    Public Property ReclassItems() As ReclassItem()
        Get
            Return m_reclassItems
        End Get
        Set(ByVal value As ReclassItem())
            m_reclassItems = value
        End Set
    End Property

    Public ReadOnly Property fMonthIdx() As Integer
        Get
            Return m_fMonthIdx
        End Get
    End Property

    Public ReadOnly Property lMonthIdx() As Integer
        Get
            Return m_lMonthIdx
        End Get
    End Property

    ' Property we use to display firstMonth; The code works with the index which is one-off
    Public Property firstMonth() As String
        Get
            If fMonthIdx > -1 Then
                Return CStr(fMonthIdx + 1)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As String)
            If Not String.IsNullOrEmpty(value) Then
                m_fMonthIdx = CInt(value) - 1
            Else
                m_fMonthIdx = -1
            End If
        End Set
    End Property

    ' Property we use to display lastMonth; The code works with the index which is one-off
    Public Property lastMonth() As String
        Get
            If lMonthIdx > -1 Then
                Return CStr(lMonthIdx + 1)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As String)
            If Not String.IsNullOrEmpty(value) Then
                m_lMonthIdx = CInt(value - 1)
            Else
                m_lMonthIdx = -1
            End If
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
