Imports System.Xml.Serialization
Imports BAGIS_ClassLibrary

' Root element for user-defined template xml
<XmlInclude(GetType(BAGIS_ClassLibrary.TemplateRule))> _
Public Class TemplateCollection
    Inherits SerializableData

    Dim m_templateArray As TemplateRule()
    Dim m_version As String

    Property TemplateArray() As TemplateRule()
        Get
            Return m_templateArray
        End Get
        Set(ByVal value As TemplateRule())
            m_templateArray = value
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
