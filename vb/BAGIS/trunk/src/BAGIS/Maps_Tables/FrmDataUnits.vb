Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.esriSystem
Imports System.Text
Imports System.IO
Imports System.Windows.Forms

Public Class FrmDataUnits

    Private m_aoi As Aoi
    Private m_newElevUnit As MeasurementUnit

    Public Sub New(ByVal pAoi As Aoi)

        ' This call is required by the designer.
        InitializeComponent()

        m_aoi = pAoi
        Me.Text = "Data Units (AOI: " & m_aoi.Name & " )"

        LoadElevationUnits()
        Dim settingsUnit As MeasurementUnit = GetElevationUnitsFromSettings()
        CboElevationUnits.SelectedItem = BA_EnumDescription(settingsUnit)
    End Sub

    Private Sub LoadElevationUnits()
        CboElevationUnits.Items.Clear()
        CboElevationUnits.Items.Add(BA_EnumDescription(MeasurementUnit.Feet))
        CboElevationUnits.Items.Add(BA_EnumDescription(MeasurementUnit.Meters))
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub BtnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        Dim inputFolder As String
        Dim inputFile As String

        'We need to update the elevation units
        inputFolder = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Surfaces)
        inputFile = BA_EnumDescription(MapsFileName.filled_dem_gdb)
        Dim sb As StringBuilder = New StringBuilder
        sb.Append(BA_BAGIS_TAG_PREFIX)
        sb.Append(BA_ZUNIT_CATEGORY_TAG & MeasurementUnitType.Elevation.ToString & "; ")
        sb.Append(BA_ZUNIT_VALUE_TAG & CboElevationUnits.SelectedItem & ";")
        sb.Append(BA_BAGIS_TAG_SUFFIX)
        BA_UpdateMetadata(inputFolder, inputFile, LayerType.Raster, BA_XPATH_TAGS, _
                          sb.ToString, BA_BAGIS_TAG_PREFIX.Length)
        m_newElevUnit = BA_GetMeasurementUnit(CboElevationUnits.SelectedItem)
        Me.Close()
    End Sub

    Private Function GetElevationUnitsFromSettings() As MeasurementUnit
        Dim response As Integer = 0
        Dim elevationUnit As MeasurementUnit = MeasurementUnit.Meters
        BA_SetSettingPath() 'this Sub finds the path for bagisanalyst.def file and sets BA_Settings_Filepath.
        If Not File.Exists(BA_Settings_Filepath & "\" & BA_Settings_Filename) Then
            MessageBox.Show("Settings file does not exist!" & vbCrLf & "Please go to Options to set settings files", "Missing file!")
            Exit Function
        End If

        response = BA_ReadBAGISSettings(BA_Settings_Filepath) 'set the system setting parameters
        If response <> 1 Then
            MsgBox("Unable to get elevation from settings information. Using defaults.")
            Return elevationUnit
        End If

        If BA_SystemSettings.DEM_ZUnit_IsMeter Then
            Return elevationUnit
        Else
            Return MeasurementUnit.Feet
        End If
    End Function

    Public ReadOnly Property NewElevationUnit()
        Get
            Return m_newElevUnit
        End Get
    End Property
End Class

