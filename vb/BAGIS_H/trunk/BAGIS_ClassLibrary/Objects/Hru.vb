Imports System.Xml.Serialization

Imports BAGIS_ClassLibrary

Public Class Hru
    Inherits SerializableData

    Dim m_parentHru As Hru
    Dim m_selectedZones As Zone()
    Dim m_name As String
    Dim m_filePath As String
    Dim m_allowNontiguousHru As Boolean
    Dim m_rules As List(Of IRule)
    Dim m_eliminateProcess As EliminateProcess
    Dim m_cookieCutProcess As CookieCutProcess
    Dim m_reclassZonesRule As RasterReclassRule
    Dim m_featureCount As Long
    Dim m_avgFeatureSize As Double
    Dim m_maxFeatureSize As Double
    Dim m_units As MeasurementUnit
    Dim m_dateCreated As DateTime
    Dim m_applicationVersion As String
    Dim m_applyToAllZones As Boolean
    Dim m_applyToSelectedZones As Boolean
    Dim m_retainSrcAttrib As Boolean

    Sub New(ByVal name As String, ByVal filePath As String, _
            ByVal selectedZoneList As Zone(), ByVal allowNonContiguousHru As Boolean, _
            ByVal featureCount As Long, ByVal avgFeatureSize As Double, ByVal maxFeatureSize As Double, _
            ByVal measureUnits As MeasurementUnit, ByVal dateCreated As Date)

        MyBase.New()
        m_name = name
        m_filePath = filePath
        SelectedZones = selectedZoneList
        m_allowNontiguousHru = allowNonContiguousHru
        m_featureCount = featureCount
        m_avgFeatureSize = avgFeatureSize
        m_maxFeatureSize = maxFeatureSize
        m_units = measureUnits
        m_dateCreated = dateCreated
    End Sub

    ' Required for de-serialization. Do not use.
    Sub New()
        MyBase.New()
    End Sub

    ' Parent HRU
    Public Property ParentHru() As Hru
        Get
            Return m_parentHru
        End Get
        Set(ByVal value As Hru)
            m_parentHru = value
        End Set
    End Property

    ' Collection of hrus related to the aoi
    Public Property SelectedZones() As Zone()
        Get
            Return m_selectedZones
        End Get
        Set(ByVal value As Zone())
            If value IsNot Nothing Then
                ReDim m_selectedZones(value.GetUpperBound(0))
                If value IsNot Nothing Then
                    If value.GetLength(0) > 0 Then
                        value.CopyTo(m_selectedZones, 0)
                    End If
                End If
            End If
        End Set
    End Property

    ' Display name of the hru
    Public Property Name() As String
        Get
            Return m_name
        End Get
        Set(ByVal value As String)
            m_name = value
        End Set
    End Property

    ' File path where the hru was created
    Public Property FilePath() As String
        Get
            Return m_filePath
        End Get
        Set(ByVal value As String)
            m_filePath = value
        End Set
    End Property

    ' Allow NonContiguous Hrus?
    Public Property AllowNonContiguousHru() As Boolean
        Get
            Return m_allowNontiguousHru
        End Get
        Set(ByVal value As Boolean)
            m_allowNontiguousHru = value
        End Set
    End Property

    ' Collection of factors used to generate the hru
    ' Cannot be xml-serialized because it is an interface. Use rule arrays instead
    <XmlIgnore()> Public Property RuleList() As List(Of IRule)
        Get
            Return m_rules
        End Get
        Set(ByVal value As List(Of IRule))
            m_rules = New List(Of IRule)
            If value IsNot Nothing Then
                If value.Count > 0 Then
                    m_rules.AddRange(value)
                End If
            End If
        End Set
    End Property

    Public Property RasterSliceRuleArray() As RasterSliceRule()
        Get
            Dim sliceList As New List(Of IRule)
            Dim pArray(0) As RasterSliceRule
            If m_rules IsNot Nothing Then
                For Each rule In m_rules
                    If TypeOf rule Is RasterSliceRule Then sliceList.Add(rule)
                Next
                If sliceList.Count > 0 Then
                    Array.Resize(pArray, sliceList.Count)
                    sliceList.CopyTo(pArray)
                End If
            End If
            Return pArray
        End Get
        Set(ByVal value As RasterSliceRule())
            If m_rules Is Nothing Then m_rules = New List(Of IRule)
            If Not value Is Nothing Then
                Dim sliceRule As RasterSliceRule
                For Each sliceRule In value
                    If sliceRule IsNot Nothing Then m_rules.Add(sliceRule)
                Next
            End If
        End Set
    End Property

    Public Property RasterReclassRuleArray() As RasterReclassRule()
        Get
            Dim reclassList As New List(Of IRule)
            Dim pArray(0) As RasterReclassRule
            If m_rules IsNot Nothing Then
                For Each rule In m_rules
                    If TypeOf rule Is RasterReclassRule Then reclassList.Add(rule)
                Next
                If reclassList.Count > 0 Then
                    Array.Resize(pArray, reclassList.Count)
                    reclassList.CopyTo(pArray)

                End If
            End If
            Return pArray
        End Get
        Set(ByVal value As RasterReclassRule())
            If m_rules Is Nothing Then m_rules = New List(Of IRule)
            If Not value Is Nothing Then
                Dim reclassRule As RasterReclassRule
                For Each reclassRule In value
                    If reclassRule IsNot Nothing Then m_rules.Add(reclassRule)
                Next
            End If
        End Set
    End Property

    Public Property ReclassContinuousRuleArray() As ReclassContinuousRule()
        Get
            Dim reclassList As New List(Of IRule)
            Dim pArray(0) As ReclassContinuousRule
            If m_rules IsNot Nothing Then
                For Each rule In m_rules
                    If TypeOf rule Is ReclassContinuousRule Then reclassList.Add(rule)
                Next
                If reclassList.Count > 0 Then
                    Array.Resize(pArray, reclassList.Count)
                    reclassList.CopyTo(pArray)

                End If
            End If
            Return pArray
        End Get
        Set(ByVal value As ReclassContinuousRule())
            If m_rules Is Nothing Then m_rules = New List(Of IRule)
            If Not value Is Nothing Then
                Dim reclassRule As ReclassContinuousRule
                For Each reclassRule In value
                    If reclassRule IsNot Nothing Then m_rules.Add(reclassRule)
                Next
            End If
        End Set
    End Property

    Public Property PrismPrecipRuleArray() As PrismPrecipRule()
        Get
            Dim prismList As New List(Of IRule)
            Dim pArray(0) As PrismPrecipRule
            If m_rules IsNot Nothing Then
                For Each rule In m_rules
                    If TypeOf rule Is PrismPrecipRule Then prismList.Add(rule)
                Next
                If prismList.Count > 0 Then
                    Array.Resize(pArray, prismList.Count)
                    prismList.CopyTo(pArray)
                End If
            End If
            Return pArray
        End Get
        Set(ByVal value As PrismPrecipRule())
            If m_rules Is Nothing Then m_rules = New List(Of IRule)
            If Not value Is Nothing Then
                Dim prismRule As PrismPrecipRule
                For Each prismRule In value
                    If prismRule IsNot Nothing Then m_rules.Add(prismRule)
                Next
            End If
        End Set
    End Property

    Public Property DAFlowTypeRuleArray() As DAFlowTypeZonesRule()
        Get
            Dim daFlowList As New List(Of IRule)
            Dim pArray(0) As DAFlowTypeZonesRule
            If m_rules IsNot Nothing Then
                For Each rule In m_rules
                    If TypeOf rule Is DAFlowTypeZonesRule Then daFlowList.Add(rule)
                Next
                If daFlowList.Count > 0 Then
                    Array.Resize(pArray, daFlowList.Count)
                    daFlowList.CopyTo(pArray)
                End If
            End If
            Return pArray
        End Get
        Set(ByVal value As DAFlowTypeZonesRule())
            If m_rules Is Nothing Then m_rules = New List(Of IRule)
            If Not value Is Nothing Then
                Dim daFlowRule As DAFlowTypeZonesRule
                For Each daFlowRule In value
                    If daFlowRule IsNot Nothing Then m_rules.Add(daFlowRule)
                Next
            End If
        End Set
    End Property

    Public Property ContributingAreasRuleArray() As ContributingAreasRule()
        Get
            Dim contribAreaList As New List(Of IRule)
            Dim pArray(0) As ContributingAreasRule
            If m_rules IsNot Nothing Then
                For Each rule In m_rules
                    If TypeOf rule Is ContributingAreasRule Then contribAreaList.Add(rule)
                Next
                If contribAreaList.Count > 0 Then
                    Array.Resize(pArray, contribAreaList.Count)
                    contribAreaList.CopyTo(pArray)
                End If
            End If
            Return pArray
        End Get
        Set(ByVal value As ContributingAreasRule())
            If m_rules Is Nothing Then m_rules = New List(Of IRule)
            If Not value Is Nothing Then
                Dim contribAreaRule As ContributingAreasRule
                For Each contribAreaRule In value
                    If contribAreaRule IsNot Nothing Then m_rules.Add(contribAreaRule)
                Next
            End If
        End Set
    End Property

    Public Property TemplateRuleArray() As TemplateRule()
        Get
            Dim templateList As New List(Of IRule)
            Dim pArray(0) As TemplateRule
            If m_rules IsNot Nothing Then

                For Each rule In m_rules
                    If TypeOf rule Is TemplateRule Then templateList.Add(rule)
                Next
                If templateList.Count > 0 Then
                    Array.Resize(pArray, templateList.Count)
                    templateList.CopyTo(pArray)
                End If
            End If
            Return pArray
        End Get
        Set(ByVal value As TemplateRule())
            If m_rules Is Nothing Then m_rules = New List(Of IRule)
            If Not value Is Nothing Then
                For Each templateRule In value
                    If templateRule IsNot Nothing Then m_rules.Add(templateRule)
                Next
            End If
        End Set
    End Property

    Public Property EliminateProcess() As EliminateProcess
        Get
            Return m_eliminateProcess
        End Get
        Set(ByVal value As EliminateProcess)
            m_eliminateProcess = value
        End Set
    End Property

    Public Property CookieCutProcess() As CookieCutProcess
        Get
            Return m_cookieCutProcess
        End Get
        Set(ByVal value As CookieCutProcess)
            m_cookieCutProcess = value
        End Set
    End Property

    Public Property ReclassZonesRule() As RasterReclassRule
        Get
            Return m_reclassZonesRule
        End Get
        Set(ByVal value As RasterReclassRule)
            m_reclassZonesRule = value
        End Set
    End Property

    ' Number of features in the HRU
    Public Property FeatureCount() As Long
        Get
            Return m_featureCount
        End Get
        Set(ByVal value As Long)
            m_featureCount = value
        End Set
    End Property

    ' Average feature size in hru
    Public Property AverageFeatureSize() As Double
        Get
            Return m_avgFeatureSize
        End Get
        Set(ByVal value As Double)
            m_avgFeatureSize = value
        End Set
    End Property

    ' Maximum feature size in hru
    Public Property MaxFeatureSize() As Double
        Get
            Return m_maxFeatureSize
        End Get
        Set(ByVal value As Double)
            m_maxFeatureSize = value
        End Set
    End Property

    ' Unit of measure for the size fields
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

    ' Date the HRU was created
    ' Not serialized as it is read-only
    Public ReadOnly Property DateCreated() As Date
        Get
            Return m_dateCreated
        End Get
    End Property

    Public Property DateCreatedValue() As String
        Get
            Return m_dateCreated.ToShortDateString & " " & m_dateCreated.ToShortTimeString
        End Get
        Set(ByVal value As String)
            m_dateCreated = Convert.ToDateTime(value)
        End Set
    End Property

    ' Returns application version this hru was created under
    Public Property ApplicationVersion() As String
        Get
            Return m_applicationVersion
        End Get
        Set(ByVal value As String)
            m_applicationVersion = value
        End Set
    End Property

    Public Property ApplyToAllZones() As Boolean
        Get
            Return m_applyToAllZones
        End Get
        Set(ByVal value As Boolean)
            m_applyToAllZones = value
        End Set
    End Property

    Public Property ApplyToSelectedZones() As Boolean
        Get
            Return m_ApplyToSelectedZones
        End Get
        Set(ByVal value As Boolean)
            m_ApplyToSelectedZones = value
        End Set
    End Property

    Public Property RetainSourceAttributes() As Boolean
        Get
            Return m_retainSrcAttrib
        End Get
        Set(ByVal value As Boolean)
            m_retainSrcAttrib = value
        End Set
    End Property

End Class
