Imports BAGIS_ClassLibrary
Imports System.Xml.Serialization

Public Class Method
    Inherits SerializableData

    Dim m_id As Integer
    Dim m_name As String
    Dim m_description As String
    Dim m_toolBoxName As String
    Dim m_modelName As String
    Dim m_modelLabel As String
    Dim m_toolBoxPath As String
    Dim m_parameters As List(Of ModelParameter)
    Dim m_status As MethodStatus
    Dim m_errorMessage As String
    Dim m_filledParameters As List(Of ModelParameter)
    Dim m_useMethod As Boolean = True
    Dim m_validationMessages As List(Of String)
    Dim m_validated As String

    ' Required for de-serialization. Do not use.
    Sub New()
        MyBase.new()
    End Sub

    ' This is the constructor to use
    Sub New(ByVal id As Integer, ByVal name As String, ByVal description As String, _
            ByVal toolboxName As String, ByVal modelName As String, ByVal modelLabel As String, ByVal toolBoxPath As String)
        m_id = id
        m_name = name
        m_description = description
        m_toolBoxName = toolboxName
        m_modelName = modelName
        m_modelLabel = modelLabel
        m_toolBoxPath = toolBoxPath
    End Sub

    ' Unique parameter id
    Public Property Id() As Integer
        Get
            Return m_id
        End Get
        Set(ByVal value As Integer)
            m_id = value
        End Set
    End Property

    ' Name of method
    Public Property Name() As String
        Get
            Return m_name
        End Get
        Set(ByVal value As String)
            m_name = value
        End Set
    End Property

    'Description of method
    Public Property Description() As String
        Get
            Return m_description
        End Get
        Set(ByVal value As String)
            m_description = value
        End Set
    End Property

    Public Property ToolboxName As String
        Get
            Return m_toolBoxName
        End Get
        Set(ByVal value As String)
            m_toolBoxName = value
        End Set
    End Property

    Public Property ModelName As String
        Get
            Return m_modelName
        End Get
        Set(ByVal value As String)
            m_modelName = value
        End Set
    End Property

    Public Property ModelLabel As String
        Get
            Return m_modelLabel
        End Get
        Set(ByVal value As String)
            m_modelLabel = value
        End Set
    End Property

    Public Property ToolBoxPath As String
        Get
            Return m_toolBoxPath
        End Get
        Set(ByVal value As String)
            m_toolBoxPath = value
        End Set
    End Property

    ' List of model parameters related to the method
    Public Property ModelParameters() As List(Of ModelParameter)
        Get
            Return m_parameters
        End Get
        Set(ByVal value As List(Of ModelParameter))
            m_parameters = New List(Of ModelParameter)
            If value IsNot Nothing Then
                If value.Count > 0 Then
                    m_parameters.AddRange(value)
                End If
            End If
        End Set
    End Property

    <XmlIgnore()>
    Public Property Status As MethodStatus
        Get
            Return m_status
        End Get
        Set(ByVal value As MethodStatus)
            m_status = value
        End Set
    End Property

    Public Property StatusText As String
        Get
            Return m_status.ToString
        End Get
        Set(ByVal value As String)
            m_status = BA_GetMethodStatus(value)
        End Set
    End Property

    Public Property ErrorMessage As String
        Get
            Return m_errorMessage
        End Get
        Set(ByVal value As String)
            m_errorMessage = value
        End Set
    End Property

    ' List of model parameters related to the method with their values filled (calculated)
    Public Property FilledModelParameters() As List(Of ModelParameter)
        Get
            Return m_filledParameters
        End Get
        Set(ByVal value As List(Of ModelParameter))
            m_filledParameters = New List(Of ModelParameter)
            If value IsNot Nothing Then
                If value.Count > 0 Then
                    m_filledParameters.AddRange(value)
                End If
            End If
        End Set
    End Property

    Public Property UseMethod() As Boolean
        Get
            Return m_useMethod
        End Get
        Set(ByVal value As Boolean)
            m_useMethod = value
        End Set
    End Property

    ' Validation messages; Used when exporting across AOI's
    Public Property ValidationMessages() As List(Of String)
        Get
            Return m_validationMessages
        End Get
        Set(ByVal value As List(Of String))
            m_validationMessages = New List(Of String)
            If value IsNot Nothing Then
                If value.Count > 0 Then
                    m_validationMessages.AddRange(value)
                End If
            End If
        End Set
    End Property

    Public Property Validated() As String
        Get
            If m_validationMessages Is Nothing Then
                Return "True"
            ElseIf m_validationMessages.Count = 0 Then
                Return "True"
            Else
                Return "False"
            End If
        End Get
        Set(ByVal value As String)
            'Do Nothing; Depends on validationMessages list
        End Set
    End Property

End Class
