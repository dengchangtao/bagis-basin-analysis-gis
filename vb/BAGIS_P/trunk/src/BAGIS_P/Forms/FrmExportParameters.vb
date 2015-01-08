Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports BAGIS_ClassLibrary
Imports System.IO
Imports System.Windows.Forms
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.esriSystem
Imports Microsoft.VisualBasic.FileIO

Public Class FrmExportParameters

    Dim m_aoi As Aoi
    Dim m_profileList As IList(Of Profile)

    Private Sub BtnSelectAoi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSelectAoi.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim DataPath As String
        Dim pFilter As IGxObjectFilter = New GxFilterContainers
        Dim bagisPExt As BagisPExtension = BagisPExtension.GetExtension

        Try
            'initialize and open mini browser
            With pGxDialog
                .AllowMultiSelect = False
                .ButtonCaption = "Select"
                .Title = "Select AOI Folder"
                .ObjectFilter = pFilter
                bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
            End With

            If bObjectSelected = False Then Exit Sub

            'get the name of the selected folder
            Dim pGxDataFolder As IGxFile
            pGxDataFolder = pGxObject.Next
            DataPath = pGxDataFolder.Path
            If String.IsNullOrEmpty(DataPath) Then Exit Sub 'user cancelled the action

            'check AOI/BASIN status
            Dim success As BA_ReturnCode = BA_CheckAoiStatus(DataPath, My.ArcMap.Application.hWnd, My.ArcMap.Document)
            If success = BA_ReturnCode.Success Then
                BA_SetDefaultProjection(My.ArcMap.Application)
                Dim aoiName As String = BA_GetBareName(DataPath)
                m_aoi = New Aoi(aoiName, DataPath, Nothing, bagisPExt.version)
                TxtAoiPath.Text = m_aoi.FilePath
                TxtOutputFolder1.Text = m_aoi.FilePath
                'ResetForm()
                Me.Text = "Export Parameters (AOI: " & aoiName & m_aoi.ApplicationVersion & " )"
                bagisPExt.aoi = m_aoi

                'Load layer lists
                ' Create a DirectoryInfo of the HRU directory.
                Dim zonesDirectory As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, Nothing)
                Dim dirZones As New DirectoryInfo(zonesDirectory)
                Dim dirZonesArr As DirectoryInfo() = Nothing
                If dirZones.Exists Then
                    dirZonesArr = dirZones.GetDirectories
                    LoadHruLayers(dirZonesArr)
                End If

                'Cache profiles so we have access to their descriptions
                'For now we only load this when the aoi changes
                Dim profilesFolder As String = BA_GetLocalProfilesDir(DataPath)
                m_profileList = BA_LoadProfilesFromXml(profilesFolder)
            End If
        Catch ex As Exception
            MessageBox.Show("BtnSelectAoi_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadHruLayers(ByVal dirZonesArr As DirectoryInfo())
        LstHruLayers.Items.Clear()
        If dirZonesArr IsNot Nothing Then
            Dim item As LayerListItem
            For Each dri In dirZonesArr
                Dim hruFilePath As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, dri.Name) & BA_EnumDescription(PublicPath.HruGrid)
                Dim hruXmlFilePath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, dri.Name) & BA_EnumDescription(PublicPath.HruXml)
                ' Add hru to the list if the grid exists
                If BA_File_Exists(hruFilePath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) And _
                   BA_File_ExistsWindowsIO(hruXmlFilePath) Then
                    'Assume hru is discrete raster since we create it to be so
                    item = New LayerListItem(dri.Name, hruFilePath, LayerType.Raster, True)
                    LstHruLayers.Items.Add(item)
                End If
            Next dri
        End If
    End Sub

    Private Sub LoadProfileList(ByVal hruName As String)
        LstProfiles.Items.Clear()
        Dim hruPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, hruName)
        Dim hruParamPath As String = hruPath & BA_EnumDescription(PublicPath.BagisParamGdb)
        If BA_Folder_ExistsWindowsIO(hruParamPath) Then
            Dim tNames As IList(Of String) = BA_ListTablesInGDB(hruParamPath)
            If tNames IsNot Nothing Then
                For Each pName As String In tNames
                    LstProfiles.Items.Add(pName)
                Next
            End If
        End If
    End Sub

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    Private Sub LstHruLayers_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles LstHruLayers.SelectedIndexChanged
        If LstHruLayers.SelectedIndex > -1 Then
            'Derive the file path for the HRU vector to be displayed
            Dim selItem As LayerListItem = TryCast(LstHruLayers.SelectedItem, LayerListItem)
            Dim hruGdbName As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, selItem.Name)
            Dim vName As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruZonesVector), False)
                Dim hruCount As Integer = BA_CountPolygons(hruGdbName, vName)
                If hruCount > 0 Then
                    TxtNHru.Text = CStr(hruCount)
                Else
                    TxtNHru.Text = "0"
                End If 
            LoadProfileList(selItem.Name)
        End If
    End Sub

    Private Sub BtnSetOutput_Click(sender As System.Object, e As System.EventArgs) Handles BtnSetOutput.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim DataPath As String
        Dim pFilter As IGxObjectFilter = New GxFilterContainers

        Try
            'initialize and open mini browser
            With pGxDialog
                .AllowMultiSelect = False
                .ButtonCaption = "Select"
                .Title = "Select output folder"
                .ObjectFilter = pFilter
                bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
            End With

            If bObjectSelected = False Then Exit Sub

            'get the name of the selected folder
            Dim pGxDataFolder As IGxFile
            pGxDataFolder = pGxObject.Next
            DataPath = pGxDataFolder.Path
            If String.IsNullOrEmpty(DataPath) Then Exit Sub 'user cancelled the action
            TxtOutputFolder1.Text = DataPath
        Catch ex As Exception
            Debug.Print("BtnSetOutput_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub LstProfiles_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles LstProfiles.SelectedIndexChanged
        TxtNumParameters.Text = Nothing
        Dim paramTable As ITable = Nothing
        Dim pFields As IFields = Nothing
        Dim selItem As LayerListItem = TryCast(LstHruLayers.SelectedItem, LayerListItem)
        Try
            If LstProfiles.SelectedIndex > -1 Then
                TxtDescription.Text = Nothing
                Dim hruPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, selItem.Name)
                Dim hruParamPath As String = hruPath & BA_EnumDescription(PublicPath.BagisParamGdb)
                If BA_Folder_ExistsWindowsIO(hruParamPath) Then
                    Dim selProfile As String = TryCast(LstProfiles.SelectedItem, String)
                    If selProfile IsNot Nothing Then
                        Dim tableName As String = selProfile & BA_PARAM_TABLE_SUFFIX
                        paramTable = BA_OpenTableFromGDB(hruParamPath, tableName)
                        If paramTable IsNot Nothing Then
                            pFields = paramTable.Fields
                            If pFields IsNot Nothing Then
                                'Subtract the id, the hru_id, and the oms_id columns
                                TxtNumParameters.Text = pFields.FieldCount - 3
                            End If
                        End If
                        'Populate the profile description from cached list
                        For Each nextProfile As Profile In m_profileList
                            If nextProfile.Name = selProfile Then
                                TxtDescription.Text = nextProfile.Description
                                Exit For
                            End If
                        Next
                        TxtOutputName.Text = selProfile
                    End If
                End If
            End If
            ManageExportButton()
        Catch ex As Exception
            Debug.Print("LstProfiles_SelectedIndexChanged Exception: " & ex.Message)
        Finally
            pFields = Nothing
            paramTable = Nothing
        End Try
    End Sub

    Private Sub ManageExportButton()
        If CkExportShapefile.Checked Or CkExportZipped.Checked Then
            If String.IsNullOrEmpty(TxtOutputName.Text) Or LstProfiles.SelectedIndex < 0 _
                Or String.IsNullOrEmpty(TxtOutputFolder1.Text) Then
                BtnExport.Enabled = False
            Else
                BtnExport.Enabled = True
            End If
        Else
            BtnExport.Enabled = False
        End If
    End Sub

    Private Sub BtnExport_Click(sender As System.Object, e As System.EventArgs) Handles BtnExport.Click
        Dim selItem As LayerListItem = CType(LstHruLayers.SelectedItem, LayerListItem)  'Selected HRU
        Dim outputFolder As String = TxtOutputFolder1.Text
        Dim tempBagisFolder As String = "BagisZip"
        Dim outputFile As String = BA_GetBareName(TxtOutputName.Text)
        Dim zipFolder As String = Nothing
        'Delete tempBagisFolder if it exists to make sure we don't have old data
        If BA_Folder_ExistsWindowsIO(outputFolder & tempBagisFolder) Then
            BA_Remove_Folder(outputFolder & tempBagisFolder)
        End If
        zipFolder = BA_CreateFolder(outputFolder, tempBagisFolder)
        Dim success As BA_ReturnCode = BA_CreateShapefileWithParameters(m_aoi.FilePath, selItem.Name, CStr(LstProfiles.SelectedItem), _
                                                                        zipFolder, BA_StandardizeShapefileName(outputFile))
        If success = BA_ReturnCode.Success Then
            'Delete duplicated fields from shapefile
            success = BA_DeleteFieldFromFeatureClass(zipFolder, outputFile, BA_FIELD_HRU_ID & "_1")
        End If
        If success = BA_ReturnCode.Success Then
            'Create zipped output
            If CkExportZipped.Checked = True Then
                Dim zipFileName As String = BA_StandardizeShapefileName(outputFile, False) & ".zip"
                success = BA_ZipFolder(zipFolder, zipFileName)
            End If
            'Copy the feature class to the output folder
            If CkExportShapefile.Checked = True Then
                success = BA_CopyFeatures(outputFolder & "\" & tempBagisFolder & BA_StandardizeShapefileName(outputFile, True, True), _
                                          outputFolder & BA_StandardizeShapefileName(outputFile, True, True))
            End If

            If success = BA_ReturnCode.Success Then
                BA_Remove_Folder(zipFolder)
                MessageBox.Show("Parameter file export complete !", _
                                "File export", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            MessageBox.Show("An error occurred while trying to export the shapefile", _
                "File export", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub TxtOutputFolder1_TextChanged(sender As Object, e As System.EventArgs) Handles TxtOutputFolder1.TextChanged
        ManageExportButton()
    End Sub

    Private Sub TxtOutputName_Validating(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles TxtOutputName.Validating
        If Not IsValidFileName(TxtOutputName.Text) Then
            ' Cancel the event and select the text to be corrected by the user.
            e.Cancel = True
            TxtOutputName.Select(0, TxtOutputName.Text.Length)
            MessageBox.Show("Please enter a valid output name without spaces or special characters", "Invalid output name", MessageBoxButtons.OK, _
                            MessageBoxIcon.Information)
        Else
            ManageExportButton()
        End If
    End Sub

    Private Function IsValidFileName(ByVal fn As String) As Boolean
        Try
            'Don't allow spaces
            If fn.Contains(" ") Then
                Return False
            End If
            Dim fi As New IO.FileInfo(fn)
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Private Sub CkExportShapefile_CheckedChanged(sender As Object, e As System.EventArgs) Handles CkExportShapefile.CheckedChanged
        ManageExportButton()
    End Sub

    Private Sub CkExportZipped_CheckedChanged(sender As Object, e As System.EventArgs) Handles CkExportZipped.CheckedChanged
        ManageExportButton()
    End Sub
End Class