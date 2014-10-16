Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Desktop.AddIns
Imports BAGIS_ClassLibrary

Public Class HruExtension
    Inherits ESRI.ArcGIS.Desktop.AddIns.Extension

    Private Shared s_extension As HruExtension
    Private m_version As String = " 1.4.0"
    Private m_templateVersion As String = "3.0"
    Private m_aoi As Aoi
    Private m_currentTool As ESRI.ArcGIS.Framework.ICommandItem
    Private m_settingsPath As String
    Private m_datum As String
    Private m_spatialReferenceName As String
    Private m_classLibraryVersion As String = BA_CLASS_LIBRARY_VERSION

    Public Sub New()
        s_extension = Me
        'Load settings path
        m_settingsPath = BA_GetSettingsPath()
    End Sub

    Friend Shared Function GetExtension() As HruExtension
        ' Extension loads just in time, call FindExtension to load it.
        If s_extension Is Nothing Then
            Dim extID As UID = New UIDClass()
            extID.Value = My.AddInID
            My.ArcMap.Application.FindExtensionByCLSID(extID)
        End If
        Return s_extension
    End Function

    Protected Overrides Sub OnStartup()

    End Sub

    Protected Overrides Sub OnShutdown()

    End Sub

    Friend ReadOnly Property version() As String
        Get
            Return m_version
        End Get
    End Property

    Friend Property aoi() As Aoi
        Get
            Return m_aoi
        End Get
        Set(ByVal value As Aoi)
            m_aoi = value
        End Set
    End Property

    Friend Property CurrentTool() As ESRI.ArcGIS.Framework.ICommandItem
        Get
            Return m_currentTool
        End Get
        Set(ByVal value As ESRI.ArcGIS.Framework.ICommandItem)
            m_currentTool = value
        End Set
    End Property

    Friend Property SettingsPath() As String
        Get
            Return m_settingsPath
        End Get
        Set(ByVal value As String)
            m_settingsPath = value
        End Set
    End Property

    Friend ReadOnly Property TemplateVersion() As String
        Get
            Return m_templateVersion
        End Get
    End Property

    Friend ReadOnly Property ClassLibraryVersion() As String
        Get
            Return m_classLibraryVersion
        End Get
    End Property

    Friend Property Datum() As String
        Get
            Return m_datum
        End Get
        Set(ByVal value As String)
            m_datum = value
        End Set

    End Property

    Friend Property SpatialReference() As String
        Get
            Return m_spatialReferenceName
        End Get
        Set(ByVal value As String)
            m_spatialReferenceName = value
        End Set

    End Property


End Class
