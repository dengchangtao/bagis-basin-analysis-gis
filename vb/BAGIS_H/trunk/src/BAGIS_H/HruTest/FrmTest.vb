Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.GeoAnalyst

''' <summary>
''' Designer class of the dockable window add-in. It contains user interfaces that
''' make up the dockable window.
''' </summary>
Public Class FrmTest

    Public Sub New(ByVal hook As Object)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Hook = hook
    End Sub


    Private m_hook As Object
    ''' <summary>
    ''' Host object of the dockable window
    ''' </summary> 
    Public Property Hook() As Object
        Get
            Return m_hook
        End Get
        Set(ByVal value As Object)
            m_hook = value
        End Set
    End Property

    ''' <summary>
    ''' Implementation class of the dockable window add-in. It is responsible for
    ''' creating and disposing the user interface class for the dockable window.
    ''' </summary>
    Public Class AddinImpl
        Inherits ESRI.ArcGIS.Desktop.AddIns.DockableWindow

        Private m_windowUI As FrmTest

        Protected Overrides Function OnCreateChild() As System.IntPtr
            m_windowUI = New FrmTest(Me.Hook)
            Return m_windowUI.Handle
        End Function

        Protected Overrides Sub Dispose(ByVal Param As Boolean)
            If m_windowUI IsNot Nothing Then
                m_windowUI.Dispose(Param)
            End If

            MyBase.Dispose(Param)
        End Sub

    End Class

    Private Sub BtnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnSubmit.Click

        Dim inputFolderPath As String = "C:\Docs\Lesley\NRCS_Code_Migration\GIS\UCO_RioG_SantaFe_R_nr_SantaFe_092010\layers\west_covtype"
        Dim pOutputRaster As IGeoDataset = Nothing
        Dim sliceType As esriGeoAnalysisSliceEnum = esriGeoAnalysisSliceEnum.esriGeoAnalysisSliceEqualArea
        If String.IsNullOrEmpty(TxtSlices.Text) Then
            MsgBox("Number of slices required")
            Exit Sub
        End If
        'pOutputRaster = BA_SliceRaster(inputFolderPath, sliceType, 5, 1, WorkspaceType.Raster)
        MsgBox("Done!")
    End Sub
End Class