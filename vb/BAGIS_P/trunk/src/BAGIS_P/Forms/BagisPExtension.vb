Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Desktop.AddIns
Imports BAGIS_ClassLibrary

Public Class BagisPExtension
    Inherits ESRI.ArcGIS.Desktop.AddIns.Extension

    Private Shared s_extension As BagisPExtension
    Private m_version As String = " 1.3.0"
    Private m_classLibraryVersion As String = BA_CLASS_LIBRARY_VERSION
    Private m_aoi As Aoi
    Private m_settingsPath As String
    Private m_datum As String
    Private m_spatialReferenceName As String
    Private m_arcMapVersion As String
    Private m_noDataValue As String
    Private m_profileAdministrator As Boolean

    Public Sub New()
        s_extension = Me
        'Load settings path
        m_settingsPath = BA_GetSettingsPath()
        'Get the version of ArcMap from the runtime manager
        'http://gis.stackexchange.com/questions/48260/how-can-you-find-arcgis-version-programatically
        Dim success As Boolean = ESRI.ArcGIS.RuntimeManager.Bind(ESRI.ArcGIS.ProductCode.Desktop)
        If success = True Then
            Dim activeRuntimeInfo As ESRI.ArcGIS.RuntimeInfo = ESRI.ArcGIS.RuntimeManager.ActiveRuntime
            m_arcMapVersion = activeRuntimeInfo.Version
        End If
    End Sub

    Friend Shared Function GetExtension() As BagisPExtension
        ' Extension loads just in time, call FindExtension to load it.
        If s_extension Is Nothing Then
            Dim extID As UID = New UID()
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

    Friend ReadOnly Property ClassLibraryVersion() As String
        Get
            Return m_classLibraryVersion
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

    Friend Property SettingsPath() As String
        Get
            Return m_settingsPath
        End Get
        Set(ByVal value As String)
            m_settingsPath = value
        End Set
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

    Friend ReadOnly Property ArcMapVersion() As String
        Get
            Return m_arcMapVersion
        End Get
    End Property

    Friend Property ProfileAdministrator() As Boolean
        Get
            Return m_profileAdministrator
        End Get
        Set(ByVal value As Boolean)
            m_profileAdministrator = value
        End Set
    End Property

End Class
