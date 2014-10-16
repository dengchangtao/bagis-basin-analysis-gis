Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.esriSystem
Imports System.Text

Public Class FrmDataUnits

    Private m_aoi As Aoi

    Public Sub New(ByVal pAoi As Aoi, ByVal slopeUnits As SlopeUnit, ByVal elevUnits As MeasurementUnit, _
                   ByVal depthUnits As MeasurementUnit)

        ' This call is required by the designer.
        InitializeComponent()

        m_aoi = pAoi
        Me.Text = "Data Units (AOI: " & m_aoi.Name & " )"

        Dim labelPoint As System.Drawing.Point = New System.Drawing.Point(21, 102)
        Dim cboPoint As System.Drawing.Point = New System.Drawing.Point(165, 99)
        If elevUnits = MeasurementUnit.Missing Then
            LoadElevationUnits()
            LblElevationUnits.Location = labelPoint
            LblElevationUnits.Visible = True
            labelPoint.Y += 30
            CboElevationUnits.Location = cboPoint
            CboElevationUnits.Visible = True
            cboPoint.Y += 30
        End If
        If slopeUnits = SlopeUnit.Missing Then
            LoadSlopeUnits()
            LblSlopeUnits.Location = labelPoint
            LblSlopeUnits.Visible = True
            labelPoint.Y += 30
            CboSlopeUnits.Location = cboPoint
            CboSlopeUnits.Visible = True
            cboPoint.Y += 30
        End If
        If depthUnits = MeasurementUnit.Missing Then
            LoadPrismUnits()
            LblDepthUnits.Location = labelPoint
            LblDepthUnits.Visible = True
            CboDepthUnits.Location = cboPoint
            CboDepthUnits.Visible = True
        End If

        Dim filePath As String = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Surfaces)
        'Dim fileName As String = BA_GetBareName(BA_EnumDescription(PublicPath.Slope))
        Dim fileName As String = "slope"
        'Dim success As BA_ReturnCode = BA_UpdateMetadata(filePath, fileName, BA_XPATH_TAGS, BA_GetZUnitsText(MeasurementUnit.Acres), BA_ZUNITS_TAG_PREFIX.Length)
        'Dim myList As IList(Of String) = BA_ReadMetaData(filePath, fileName, propertyName)

    End Sub

    Private Sub LoadPrismUnits()
        CboDepthUnits.Items.Clear()
        CboDepthUnits.Items.Add(BA_EnumDescription(MeasurementUnit.Inches))
        CboDepthUnits.Items.Add(BA_EnumDescription(MeasurementUnit.Millimeters))
        CboDepthUnits.SelectedItem = BA_EnumDescription(MeasurementUnit.Inches)
    End Sub

    Private Sub LoadSlopeUnits()
        CboSlopeUnits.Items.Clear()
        CboSlopeUnits.Items.Add(BA_EnumDescription(SlopeUnit.Degree))
        CboSlopeUnits.Items.Add(BA_EnumDescription(SlopeUnit.PctSlope))
        CboSlopeUnits.SelectedItem = BA_EnumDescription(SlopeUnit.PctSlope)
    End Sub

    Private Sub LoadElevationUnits()
        CboElevationUnits.Items.Clear()
        CboElevationUnits.Items.Add(BA_EnumDescription(MeasurementUnit.Feet))
        CboElevationUnits.Items.Add(BA_EnumDescription(MeasurementUnit.Meters))
        CboElevationUnits.SelectedItem = BA_EnumDescription(MeasurementUnit.Meters)
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub BtnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        Dim inputFolder As String
        Dim inputFile As String

        'We need to update the elevation units
        If CboElevationUnits.Visible = True And CboElevationUnits.SelectedIndex > -1 Then
            inputFolder = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Surfaces)
            inputFile = BA_EnumDescription(MapsFileName.filled_dem_gdb)
            Dim sb As StringBuilder = New StringBuilder
            sb.Append(BA_BAGIS_TAG_PREFIX)
            sb.Append(BA_ZUNIT_CATEGORY_TAG & MeasurementUnitType.Elevation.ToString & "; ")
            sb.Append(BA_ZUNIT_VALUE_TAG & CboElevationUnits.SelectedItem & ";")
            sb.Append(BA_BAGIS_TAG_SUFFIX)
            BA_UpdateMetadata(inputFolder, inputFile, LayerType.Raster, BA_XPATH_TAGS, _
                              sb.ToString, BA_BAGIS_TAG_PREFIX.Length)
        End If

        'We need to update the depth units
        If CboDepthUnits.Visible = True And CboDepthUnits.SelectedIndex > -1 Then
            inputFolder = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Prism)
            inputFile = AOIPrismFolderNames.annual.ToString
            Dim sb As StringBuilder = New StringBuilder
            sb.Append(BA_BAGIS_TAG_PREFIX)
            sb.Append(BA_ZUNIT_CATEGORY_TAG & MeasurementUnitType.Depth.ToString & "; ")
            sb.Append(BA_ZUNIT_VALUE_TAG & CboDepthUnits.SelectedItem & ";")
            sb.Append(BA_BAGIS_TAG_SUFFIX)
            BA_UpdateMetadata(inputFolder, inputFile, LayerType.Raster, BA_XPATH_TAGS, _
                              sb.ToString, BA_BAGIS_TAG_PREFIX.Length)
        End If

        'We need to update the slope units
        If CboSlopeUnits.Visible = True And CboSlopeUnits.SelectedIndex > -1 Then
            inputFolder = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Surfaces)
            inputFile = BA_GetBareName(BA_EnumDescription(PublicPath.Slope))
            Dim sb As StringBuilder = New StringBuilder
            sb.Append(BA_BAGIS_TAG_PREFIX)
            sb.Append(BA_ZUNIT_CATEGORY_TAG & MeasurementUnitType.Slope.ToString & "; ")
            sb.Append(BA_ZUNIT_VALUE_TAG & CboSlopeUnits.SelectedItem & ";")
            sb.Append(BA_BAGIS_TAG_SUFFIX)
            BA_UpdateMetadata(inputFolder, inputFile, LayerType.Raster, BA_XPATH_TAGS, _
                              sb.ToString, BA_BAGIS_TAG_PREFIX.Length)
        End If

        Me.Close()
    End Sub
End Class

