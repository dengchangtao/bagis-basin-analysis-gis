Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports System.Windows.Forms
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework

Public Class FrmSlopeConverter

    Dim m_aoi As Aoi
    Dim m_version As String
    Dim m_fgdbAoi As Boolean
    Dim m_weaselAoi As Boolean

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        LoadSlopeUnits()

    End Sub



    Private Sub LoadSlopeUnits()
        CboSlopeUnits.Items.Clear()
        CboSlopeUnits.Items.Add(BA_EnumDescription(SlopeUnit.Degree))
        CboSlopeUnits.Items.Add(BA_EnumDescription(SlopeUnit.PctSlope))
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub BtnAOI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAOI.Click
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

            'Re-initialize form
            TxtAoiPath.Text = ""
            TxtSlopePath.Text = ""
            TxtCurrentSlopeUnits.Text = ""
            LblWeaselSlopePath.Visible = False
            TxtWeaselSlopePath.Visible = False
            CboSlopeUnits.SelectedIndex = -1
            m_fgdbAoi = False
            m_weaselAoi = False

            'check FGDB AOI/BASIN status
            Dim fgdbType As FolderType = BA_GetFGDBFolderType(DataPath)
            If fgdbType <> FolderType.FOLDER Then
                BA_SetDefaultProjection(My.ArcMap.Application)
                Dim aoiName As String = BA_GetBareName(DataPath)
                m_aoi = New Aoi(aoiName, DataPath, Nothing, m_version)
                m_fgdbAoi = True
                TxtAoiPath.Text = m_aoi.FilePath
                Dim slopeFolder As String = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Surfaces)
                Dim slopeFile As String = BA_GetBareName(BA_EnumDescription(PublicPath.Slope))
                'Check to see if the slope file exists before writing the path to the UI
                If BA_File_Exists(slopeFolder & "\" & slopeFile, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                    TxtSlopePath.Text = slopeFolder & "\" & slopeFile
                    Dim slopeUnit = BA_GetAoiSlopeUnit(m_aoi.FilePath)
                    TxtCurrentSlopeUnits.Text = BA_EnumDescription(slopeUnit)
                End If
            End If

            'check Weasel AOI/BASIN status
            Dim wFolderType As FolderType = BA_GetWeaselFolderType(DataPath)
            If wFolderType <> FolderType.FOLDER Then
                m_weaselAoi = True
                'There was no fgdb found in the selected folder
                If m_fgdbAoi = False Then
                    BA_SetDefaultProjection(My.ArcMap.Application)
                    Dim aoiName As String = BA_GetBareName(DataPath)
                    m_aoi = New Aoi(aoiName, DataPath, Nothing, m_version)
                    TxtAoiPath.Text = m_aoi.FilePath
                End If
                'Check to see if the slope file exists before writing the path to the UI
                Dim slopeFolder As String = BA_GetPath(m_aoi.FilePath, PublicPath.Slope)
                Dim slopeFile As String = BA_EnumDescription(MapsFileName.slope)
                If BA_File_Exists(slopeFolder & "\" & slopeFile, WorkspaceType.Raster, esriDatasetType.esriDTRasterDataset) Then
                    TxtWeaselSlopePath.Text = slopeFolder & "\" & slopeFile
                    TxtWeaselSlopePath.Visible = True
                    LblWeaselSlopePath.Visible = True
                    'Only update the current slope unit if it wasn't defined by a gdb AOI
                    If String.IsNullOrEmpty(TxtCurrentSlopeUnits.Text) Then
                        Dim slopeUnit = BA_GetAoiWeaselSlopeUnit(m_aoi.FilePath)
                        TxtCurrentSlopeUnits.Text = BA_EnumDescription(slopeUnit)
                    End If
                End If

            End If

            'the folder selected did not contain an FGDB or Weasel AOI
            If String.IsNullOrEmpty(TxtAoiPath.Text) Then
                MessageBox.Show("The selected folder does not contain a valid BASIN or AOI!")
                Exit Sub
            End If
            ManageApplyButton()

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

    Private Sub ManageApplyButton()
        If Not String.IsNullOrEmpty(TxtSlopePath.Text) Or Not String.IsNullOrEmpty(TxtWeaselSlopePath.Text) Then
            If CboSlopeUnits.SelectedIndex > -1 Then
                BtnApply.Enabled = True
            Else
                BtnApply.Enabled = False
            End If
        Else
            BtnApply.Enabled = False
        End If
    End Sub

    Private Sub CboSlopeUnits_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CboSlopeUnits.SelectedIndexChanged
        ManageApplyButton()
    End Sub

    Private Sub BtnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        ' Create/configure a step progressor
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 5)
        pStepProg.Show()
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Updating slope layer(s)", "Converting slope layer(s)...")
        progressDialog2.ShowDialog()
        pStepProg.Step()

        Try
            BtnApply.Enabled = False
            Dim strSlope As String = CType(CboSlopeUnits.SelectedItem, String)
            Dim newSlopeUnit As SlopeUnit = BA_GetSlopeUnit(strSlope)
            Dim success As BA_ReturnCode
            Dim fgdbSurfacesFolder As String = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Surfaces)
            Dim weaselSlopeFolder As String = BA_GetPath(m_aoi.FilePath, PublicPath.Slope)
            If CkRecalculate.Checked = True And newSlopeUnit <> SlopeUnit.Missing Then
                'Recalculate FGDB slope layer
                If m_fgdbAoi = True Then
                    Dim demPath As String = fgdbSurfacesFolder & "\" & BA_EnumDescription(MapsFileName.filled_dem_gdb)
                    Dim aoiFolder = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Aoi)
                    Dim snapRasterPath As String = aoiFolder & BA_EnumDescription(PublicPath.AoiGrid)
                    success = BA_Calculate_Slope(demPath, TxtSlopePath.Text, newSlopeUnit, snapRasterPath)
                    If success = BA_ReturnCode.Success Then
                        progressDialog2.Description = "Updating FGDB metadata..."
                        pStepProg.Step()
                        BA_UpdateSlopeUnits(fgdbSurfacesFolder, BA_GetBareName(BA_EnumDescription(PublicPath.Slope)), newSlopeUnit)
                    End If
                End If
                'Recalculate Weasel slope layer
                If m_weaselAoi = True Then
                    Dim demPath As String = BA_GetPath(m_aoi.FilePath, PublicPath.DEM) & "\" & BA_EnumDescription(MapsFileName.filled_dem)
                    Dim snapRasterPath As String = m_aoi.FilePath & BA_EnumDescription(PublicPath.AoiGridWeasel)
                    success = BA_Calculate_Slope(demPath, TxtWeaselSlopePath.Text, newSlopeUnit, snapRasterPath)
                    If success = BA_ReturnCode.Success Then
                        progressDialog2.Description = "Updating Weasel metadata..."
                        pStepProg.Step()
                        BA_UpdateSlopeUnits(weaselSlopeFolder, BA_EnumDescription(MapsFileName.slope), newSlopeUnit)
                    End If
                End If
            ElseIf newSlopeUnit <> SlopeUnit.Missing Then
                'Updating metadata only; Recalculate box isn't checked
                'Update metadata for FGDB slope layer
                If m_fgdbAoi = True Then
                    progressDialog2.Description = "Updating FGDB metadata..."
                    pStepProg.Step()
                    BA_UpdateSlopeUnits(fgdbSurfacesFolder, BA_GetBareName(BA_EnumDescription(PublicPath.Slope)), newSlopeUnit)
                End If
                'Update metadata for Weasel slope layer
                If m_weaselAoi = True Then
                    progressDialog2.Description = "Updating Weasel metadata..."
                    pStepProg.Step()
                    BA_UpdateSlopeUnits(weaselSlopeFolder, BA_EnumDescription(MapsFileName.slope), newSlopeUnit)
                End If
            End If
            TxtCurrentSlopeUnits.Text = BA_EnumDescription(newSlopeUnit)
            BtnApply.Enabled = True
        Catch ex As Exception
            Debug.Print("BtnApply_Click Exception: " & ex.Message)
        Finally
            ' Clean up step progressor
            pStepProg.Hide()
            progressDialog2.HideDialog()
        End Try
    End Sub

    Private Sub CkRecalculate_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CkRecalculate.CheckedChanged
        Dim recalcMessage As String = "Note: The recalculated slope layer will replace the existing slope layer(s)"
        Dim metaMessage As String = "Note: Only the unit metadata will be updated. Check the box above to recalculate the slope layer"
        If CkRecalculate.Checked Then
            TxtRecalcMessage.Text = recalcMessage
        Else
            TxtRecalcMessage.Text = metaMessage
        End If
    End Sub
End Class