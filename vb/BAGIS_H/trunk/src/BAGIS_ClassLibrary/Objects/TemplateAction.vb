Imports System.Xml.Serialization
Imports BAGIS_ClassLibrary

<XmlInclude(GetType(ReclassItem))> _
<XmlInclude(GetType(ReclassItem()))> _
Public Class TemplateAction
    Inherits SerializableData
    Implements IComparable

    Private m_id As Integer
    Private m_actionType As ActionType
    Private m_parameters As Hashtable

    Public Sub New(ByVal id As Integer)
        m_id = id
    End Sub

    ' Required for de-serialization. Do not use.
    Sub New()
        MyBase.New()
    End Sub

    Public Property id() As Integer
        Get
            Return m_id
        End Get
        ' Do not use; for xml only
        Set(ByVal value As Integer)
            m_id = value
        End Set
    End Property

    Public Property actionType() As ActionType
        Get
            Return m_actionType
        End Get
        Set(ByVal value As ActionType)
            m_actionType = value
        End Set
    End Property

    <XmlIgnore()> Public Property parameters() As Hashtable
        Get
            Return m_parameters
        End Get
        ' Do not use; for xml only
        Set(ByVal value As Hashtable)
            m_parameters = value
        End Set
    End Property

    Public Property ParameterArray() As Entry()
        Get
            Dim pArray(0) As Entry
            If m_parameters IsNot Nothing Then
                Dim keyCollection As ICollection = m_parameters.Keys
                Array.Resize(pArray, keyCollection.Count)
                Dim j As Integer
                For Each objKey In keyCollection
                    Dim key As ActionParameter = CType(objKey, ActionParameter)
                    Dim value As Object = m_parameters(objKey)
                    Dim nextEntry As New Entry(key.ToString, value)
                    pArray(j) = nextEntry
                    j += 1
                Next
            End If
            Return pArray
        End Get

        Set(ByVal value As Entry())
            If value.Length > 0 Then
                For Each pEntry In value
                    Dim aParam As ActionParameter = BA_GetActionParameter(pEntry.key)
                    Me.addParameter(aParam, pEntry.value)
                Next
            End If
        End Set
    End Property

    Public Sub addParameter(ByVal key, ByVal value)
        If m_parameters Is Nothing Then
            m_parameters = New Hashtable
            m_parameters.Add(key, value)
        Else
            m_parameters.Add(key, value)
        End If
    End Sub

    Public Function CompareTo(ByVal obj As Object) As Integer Implements IComparable.CompareTo
        Dim action2 As TemplateAction = TryCast(obj, TemplateAction)
        If action2 IsNot Nothing Then
            Return Me.id.CompareTo(action2.id)
        Else
            Throw New ArgumentException("Object is not a TemplateAction")
        End If
    End Function

    Public Class Entry
        Private m_key As String
        Private m_value As Object

        Public Sub New()
            'nothing here 
        End Sub

        Public Sub New(ByVal key As String, ByVal value As Object)
            Me.m_key = key
            Me.m_value = value
        End Sub

        Public Property key() As String
            Get
                Return m_key
            End Get
            Set(ByVal value As String)
                m_key = value
            End Set
        End Property

        Public Property value()
            Get
                Return m_value
            End Get
            Set(ByVal value)
                m_value = value
            End Set
        End Property
    End Class

End Class
