Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.CatalogUI
Imports BAGIS_ClassLibrary
Imports System.Windows.Forms
Imports System.Text

Public Class FrmToolsDataUnits

    Dim m_aoi As Aoi
    Dim m_version As String
    Dim SLOPE_UNIT As SlopeUnit = SlopeUnit.PctSlope
    Dim ELEV_UNIT As MeasurementUnit = MeasurementUnit.Meters
    Dim DEPTH_UNIT As MeasurementUnit = MeasurementUnit.Inches

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        LoadElevationUnits()
        LoadPrismUnits()
        LoadSlopeUnits()

    End Sub

    Private Sub BtnAOI_Click(sender As System.Object, e As System.EventArgs) Handles BtnAOI.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim DataPath As String
        Dim pFilter As IGxObjectFilter = New GxFilterContainers
        Dim hruExt As HruExtension = HruExtension.GetExtension
        m_version = hruExt.version

        Try
            'initialize and open mini browser
            With pGxDialog
                .AllowMultiSelect = False
                .ButtonCaption = "Select"
                .Title = "Select Folder"
                .ObjectFilter = pFilter
                bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
            End With

            If bObjectSelected = False Then Exit Sub

            'get the name of the selected folder
            Dim pGxDataFolder As IGxFile
            pGxDataFolder = pGxObject.Next
            DataPath = pGxDataFolder.Path
            If String.IsNullOrEmpty(DataPath) Then Exit Sub 'user cancelled the action

            'check FGDB AOI/BASIN status
            Dim fgdbType As FolderType = BA_GetFGDBFolderType(DataPath)
            If fgdbType <> FolderType.FOLDER Then
                BA_SetDefaultProjection(My.ArcMap.Application)
                Dim aoiName As String = BA_GetBareName(DataPath)
                m_aoi = New Aoi(aoiName, DataPath, Nothing, m_version)
                TxtAoiPath.Text = m_aoi.FilePath

                'Check to make sure the units are set in the metadata before proceeding
                Dim slopeUnit As SlopeUnit
                Dim elevUnit As MeasurementUnit
                Dim depthUnit As MeasurementUnit    'prism data
                BA_GetMeasurementUnitsForAoi(DataPath, slopeUnit, elevUnit, depthUnit)
                'This List tracks any missing units so we can warn the user
                Dim missingList As IList(Of String) = New List(Of String)
                If elevUnit = BAGIS_ClassLibrary.MeasurementUnit.Missing Then
                    missingList.Add("Elevation units")
                    CboElevationUnits.SelectedItem = BA_EnumDescription(ELEV_UNIT)
                Else
                    CboElevationUnits.SelectedItem = BA_EnumDescription(elevUnit)
                End If
                If slopeUnit = BAGIS_ClassLibrary.SlopeUnit.Missing Then
                    missingList.Add("Slope units")
                    CboSlopeUnits.SelectedItem = BA_EnumDescription(SLOPE_UNIT)
                Else
                    CboSlopeUnits.SelectedItem = BA_EnumDescription(slopeUnit)
                End If
                If depthUnit = BAGIS_ClassLibrary.MeasurementUnit.Missing Then
                    missingList.Add("Depth units")
                    CboDepthUnits.SelectedItem = BA_EnumDescription(DEPTH_UNIT)
                Else
                    CboDepthUnits.SelectedItem = BA_EnumDescription(depthUnit)
                End If

                'Warning message that units are missing
                If missingList.Count > 0 Then
                    Dim strWarning As String = "Warning: The following units were not set for this AOI." & vbCrLf
                    strWarning = strWarning & "They will be set to the default value for BAGIS-H unless you change them." & vbCrLf
                    For Each strMiss In missingList
                        strWarning = strWarning & strMiss & vbCrLf
                    Next
                    MessageBox.Show(strWarning, "Missing units", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If

                BtnApply.Enabled = True
            End If

            'the folder selected did not contain an FGDB or Weasel AOI
            If String.IsNullOrEmpty(TxtAoiPath.Text) Then
                MessageBox.Show("The selected folder does not contain a valid BASIN or AOI!")
                Exit Sub
            End If

        Catch ex As Exception
            Debug.Print("BtnAOI_Click Exception: " & ex.Message)
        Finally
            pGxDialog = Nothing
            pGxObject = Nothing
            pFilter = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub

    Private Sub LoadPrismUnits()
        CboDepthUnits.Items.Clear()
        CboDepthUnits.Items.Add(BA_EnumDescription(MeasurementUnit.Inches))
        CboDepthUnits.Items.Add(BA_EnumDescription(MeasurementUnit.Millimeters))
        CboDepthUnits.SelectedItem = BA_EnumDescription(DEPTH_UNIT)
    End Sub

    Private Sub LoadSlopeUnits()
        CboSlopeUnits.Items.Clear()
        CboSlopeUnits.Items.Add(BA_EnumDescription(SlopeUnit.Degree))
        CboSlopeUnits.Items.Add(BA_EnumDescription(SlopeUnit.PctSlope))
        CboSlopeUnits.SelectedItem = BA_EnumDescription(SLOPE_UNIT)
    End Sub

    Private Sub LoadElevationUnits()
        CboElevationUnits.Items.Clear()
        CboElevationUnits.Items.Add(BA_EnumDescription(MeasurementUnit.Feet))
        CboElevationUnits.Items.Add(BA_EnumDescription(MeasurementUnit.Meters))
        CboElevationUnits.SelectedItem = BA_EnumDescription(ELEV_UNIT)
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub BtnApply_Click(sender As System.Object, e As System.EventArgs) Handles BtnApply.Click
        Dim inputFolder As String
        Dim inputFile As String

        'We need to update the elevation units
        If CboElevationUnits.SelectedIndex > -1 Then
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
        If CboDepthUnits.SelectedIndex > -1 Then
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
        If CboSlopeUnits.SelectedIndex > -1 Then
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

        MessageBox.Show("Any changes you made to the units have been saved.", "Saved successfully", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
End Class