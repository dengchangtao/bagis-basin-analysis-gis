Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports BAGIS_ClassLibrary
Imports System.Windows.Forms
Imports ESRI.ArcGIS.Geodatabase
Imports System.IO
Imports System.Drawing
Imports System.Text
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.Geoprocessing
Imports ESRI.ArcGIS.DataSourcesRaster
Imports System.Globalization

Public Class FrmProfileBuilder

    Dim m_mode As String
    Dim m_aoi As Aoi
    Dim m_profileTable As Hashtable
    Dim m_selProfile As Profile
    Dim m_methodTable As Hashtable
    Dim m_dataTable As Hashtable
    'This table keeps track of hru/profile combinations that have been validated/executed
    'The key is a composite of the hru name and the profile name: hru_ca|profile1
    Dim m_methodStatusTable As Hashtable
    Dim m_selMethod As Method
    Dim m_slopeUnit As SlopeUnit
    Dim m_elevUnit As MeasurementUnit
    Dim m_depthUnit As MeasurementUnit
    Dim m_degreeUnit As MeasurementUnit

    Public Sub New(ByVal pMode As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        m_mode = pMode
        If m_mode = BA_BAGISP_MODE_LOCAL Then
            Me.Text = "Profile Builder (AOI: Not selected)"
        Else
            Me.Text = "Profile Builder (Public)"
            'Reposition panel with profile and methods
            'Dim pntPnlProfile As Point = New Point(3, 30)
            'PnlProfile.Location = pntPnlProfile
            'Hide select AOI Button
            BtnSelectAoi.Visible = False
            'Display settings folder
            Dim bExt As BagisPExtension = BagisPExtension.GetExtension
            Dim settingsPath As String = bExt.SettingsPath
            Dim bagisFolder As String = "PleaseReturn"
            Dim tmpFile As String = BA_GetBareName(BA_EnumDescription(PublicPath.BagisPProfiles), bagisFolder)
            TxtAoiPath.Text = settingsPath & BA_StandardizePathString(bagisFolder)
            Dim pntLblPath As Point = New Point(9, 9)
            TxtLblPath.Location = pntLblPath
            TxtLblPath.Width = 125
            TxtLblPath.Height = 35
            TxtLblPath.Font = LblCurrentProfile.Font
            TxtLblPath.Text = "Settings folder:"
            Dim pntTxtPath As Point = New Point(140, 7)
            TxtAoiPath.Location = pntTxtPath
            TxtAoiPath.ForeColor = TxtProfileName.ForeColor
            TxtAoiPath.Height = 25
            PnlProfile.TabIndex = 1
            'Reposition close button
            Dim pntBtnClose As Point = New Point(615, 625)
            PnlProfile.Controls.Add(BtnClose)
            BtnClose.Location = pntBtnClose
            BtnClose.Width = 65
            'Hide import public profile button
            BtnImport.Visible = False
            'Hide export local profile button
            BtnExportProfile.Visible = False
            'Reposition 'Apply Changes' and 'Verify Profile' buttons
            Dim pntBtnApply As Point = New Point(167, 128)
            BtnApply.Location = pntBtnApply
            PnlProfile.Controls.Add(BtnVerify)
            BtnVerify.Width = 109
            Dim pntVerify As Point = New Point(193, 166)
            BtnVerify.Location = pntVerify
            'Hide controls on right side of form
            LblHruLayers.Visible = False
            GrdHruLayers.Visible = False
            GrdCompleteProfiles.Visible = False
            BtnCalculate.Visible = False
            'Hide subAoi panel
            PnlSubAoi.Visible = False
            'Resize form
            Me.Height = Me.Height + 40
            Me.Width = Me.Width - 325
            PnlProfile.Height = PnlProfile.Height + 40
            'Format profiles grid
            RefreshProfileData()
            LoadProfileGrid()
            'Format methods grid
            RefreshMethodData()
            GrdMethods.Columns("IncludeMethod").Visible = False
            GrdMethods.Width = 336
            LblStatus.Width = 335
            'Re-Initialize m_methodStatusTable
            m_methodStatusTable = New Hashtable
        End If

    End Sub

    Private Sub FrmProfileBuilder_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        GrdProfiles.ClearSelection()
        GrdProfiles.Refresh()
        m_selProfile = Nothing
        GrdMethods.Rows.Clear()
    End Sub

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
                LblStatus.Text = "Gathering HRU information"

                BA_SetDefaultProjection(My.ArcMap.Application)
                Dim aoiName As String = BA_GetBareName(DataPath)
                m_aoi = New Aoi(aoiName, DataPath, Nothing, bagisPExt.version)
                TxtAoiPath.Text = m_aoi.FilePath
                'ResetForm()
                Me.Text = "Profile Builder (AOI: " & aoiName & m_aoi.ApplicationVersion & " )"
                bagisPExt.aoi = m_aoi
                SetDatumInExtension()

                ' Get the count of zone directories so we know how many steps to put into the StepProgressor
                ' Create a DirectoryInfo of the HRU directory.
                Dim zonesDirectory As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, Nothing)
                Dim dirZones As New DirectoryInfo(zonesDirectory)
                Dim dirZonesArr As DirectoryInfo() = Nothing
                If dirZones.Exists Then
                    dirZonesArr = dirZones.GetDirectories
                    LoadHruGrid(dirZonesArr)
                Else
                    'Reset HRU layers from a previous AOI
                    GrdHruLayers.Rows.Clear()
                End If

                'Populate data table
                LblStatus.Text = "Loading data sources"
                m_dataTable = BA_LoadSettingsFile(BA_GetLocalSettingsPath(m_aoi.FilePath))
                BA_AppendUnitsToDataSources(m_dataTable, m_aoi.FilePath)
                BA_SetMeasurementUnitsForAoi(m_aoi.FilePath, m_dataTable, m_slopeUnit, m_elevUnit, _
                     m_depthUnit, m_degreeUnit)

                If m_slopeUnit = SlopeUnit.Missing Or _
                    m_elevUnit = MeasurementUnit.Missing Or _
                    m_depthUnit = MeasurementUnit.Missing Then
                    Dim frmDataUnits As FrmDataUnits = New FrmDataUnits(m_aoi, m_slopeUnit, m_elevUnit, m_depthUnit)
                    frmDataUnits.ShowDialog()
                    'Update with changes
                    BA_SetMeasurementUnitsForAoi(m_aoi.FilePath, m_dataTable, m_slopeUnit, m_elevUnit, _
                                                 m_depthUnit, m_degreeUnit)
                End If

                RefreshProfileData()
                RefreshMethodData()
                LoadSubAOIPanel()

                'Re-Initialize m_methodStatusTable
                m_methodStatusTable = New Hashtable
                LblStatus.Text = "Refreshing view"
                LoadProfileGrid()
                LblStatus.Text = Nothing
            End If
        Catch ex As Exception
            LblStatus.Text = "Unable to load AOI"
            Debug.Print("BtnSelectAoi_Click Exception: " & ex.Message)
        End Try
    End Sub

    ' Sets the datum string from the source DEM in the hru extension
    Private Sub SetDatumInExtension()
        Dim bagisPExt As BagisPExtension = BagisPExtension.GetExtension
        Dim parentPath As String = m_aoi.FilePath & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces)
        Dim pGeoDataSet As IGeoDataset = BA_OpenRasterFromGDB(parentPath, BA_EnumDescription(MapsFileName.filled_dem_gdb))
        If pGeoDataSet IsNot Nothing Then
            'Spatial reference for the dataset in question
            Dim pSpRef As ESRI.ArcGIS.Geometry.ISpatialReference = pGeoDataSet.SpatialReference
            If pSpRef IsNot Nothing Then
                bagisPExt.Datum = BA_DatumString(pSpRef)
                bagisPExt.SpatialReference = pSpRef.Name
            End If
            pSpRef = Nothing
        End If
        pGeoDataSet = Nothing
        GC.WaitForPendingFinalizers()
        GC.Collect()
    End Sub

    Private Sub LoadHruGrid(ByVal dirZonesArr As DirectoryInfo())
        GrdHruLayers.Rows.Clear()
        If dirZonesArr IsNot Nothing Then
            For Each dri In dirZonesArr
                Dim hruFilePath As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, dri.Name) & BA_EnumDescription(PublicPath.HruGrid)
                Dim hruXmlFilePath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, dri.Name) & BA_EnumDescription(PublicPath.HruXml)
                ' Add hru to the list if the grid exists
                If BA_File_Exists(hruFilePath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) And _
                   BA_File_ExistsWindowsIO(hruXmlFilePath) Then
                    'Assume hru is discrete raster since we create it to be so
                    'item = New LayerListItem(dri.Name, hruFilePath, LayerType.Raster, True)
                    'LstHruLayers.Items.Add(item)
                    'pStepProg.Step()
                    '---create a row---
                    Dim pRow As New DataGridViewRow
                    pRow.CreateCells(GrdHruLayers)
                    pRow.Cells(0).Value = dri.Name
                    '---populate completion date if it exists
                    Dim hruPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, dri.Name)
                    Dim hruParamPath As String = hruPath & BA_EnumDescription(PublicPath.BagisParamGdb) & BA_EnumDescription(PublicPath.HruProfileStatus)
                    pRow.Cells(1).Value = BA_GetCompletedProfileCountForHru(hruParamPath)
                '---add the row---
                GrdHruLayers.Rows.Add(pRow)
                End If
            Next dri
            Dim sortCol As DataGridViewColumn = GrdHruLayers.Columns(0)
            GrdHruLayers.Sort(sortCol, System.ComponentModel.ListSortDirection.Ascending)
            GrdHruLayers.ClearSelection()
            GrdHruLayers.Refresh()
        End If
    End Sub



    Private Sub LoadProfileGrid()
        GrdProfiles.Rows.Clear()
        If m_profileTable IsNot Nothing Then
            For Each key As String In m_profileTable.Keys
                '---create a row---
                Dim item As New DataGridViewRow
                item.CreateCells(GrdProfiles)
                With item
                    .Cells(0).Value = key
                End With
                '---add the row---
                GrdProfiles.Rows.Add(item)
            Next
            Dim sortCol As DataGridViewColumn = GrdProfiles.Columns(0)
            GrdProfiles.Sort(sortCol, System.ComponentModel.ListSortDirection.Ascending)
            GrdProfiles.ClearSelection()
            GrdProfiles.Refresh()
            GrdMethods.Rows.Clear()
            If m_profileTable.Keys.Count = 0 Then
                'This would otherwise be called when a row is added to the grid
                ManageProfileButtons()
            End If
        End If
    End Sub

    'Loads the profile data from disk
    Private Sub LoadMethodGrid()
        GrdMethods.Rows.Clear()
        If m_selProfile IsNot Nothing Then
            Dim mList As List(Of String) = m_selProfile.MethodNames
            If mList IsNot Nothing AndAlso mList.Count > 0 Then
                Dim sourceTable As Hashtable = m_methodTable
                'If an aoi and hru are selected
                If m_aoi IsNot Nothing And GrdHruLayers.SelectedRows.Count > 0 Then
                    'Check to see if there is a cached method status table from an earlier run
                    Dim statusMethodTable As Hashtable = m_methodStatusTable(GetMethodStatusKey(GetSelectedHruName(), m_selProfile.Name))
                    If statusMethodTable IsNot Nothing Then
                        'If so, use that to source the grid
                        sourceTable = statusMethodTable
                    End If
                ElseIf m_mode = BA_BAGISP_MODE_PUBLIC Then
                    Dim statusMethodTable As Hashtable = m_methodStatusTable(m_selProfile.Name)
                    If statusMethodTable IsNot Nothing Then
                        sourceTable = statusMethodTable
                    End If
                End If
                For Each pName In mList
                    'Make sure method exists in the hashtable before adding in case it has been deleted
                    If sourceTable(pName) IsNot Nothing Then
                        '---create a row---
                        Dim aMethod As Method = sourceTable(pName)
                        Dim item As New DataGridViewRow
                        item.CreateCells(GrdMethods)
                        With item
                            .Cells(0).Value = aMethod.Name
                            If aMethod.Status <> MethodStatus.Missing Then
                                .Cells(1).Value = aMethod.Status.ToString
                            Else
                                .Cells(1).Value = MethodStatus.Pending.ToString
                            End If
                            If m_mode = BA_BAGISP_MODE_LOCAL Then
                                .Cells(2).Value = aMethod.UseMethod
                            End If
                        End With
                        '---add the row---
                        GrdMethods.Rows.Add(item)
                    End If
                Next
                Dim sortCol As DataGridViewColumn = GrdMethods.Columns(0)
                GrdMethods.Sort(sortCol, System.ComponentModel.ListSortDirection.Ascending)
                GrdMethods.ClearSelection()
                GrdMethods.Refresh()
                TxtNumMethods.Text = GrdMethods.RowCount
            End If
        End If
    End Sub

    Private Sub BtnProfileNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnProfileNew.Click
        ClearProfileFields()
        m_selProfile = Nothing
        TxtProfileName.ReadOnly = False
        TxtDescription.ReadOnly = False
        BtnApply.Enabled = True
        BtnProfileDelete.Enabled = False
    End Sub

    Private Sub ClearProfileFields()
        'LstMethods.Items.Clear()
        TxtProfileName.Text = Nothing
        TxtProfileName.ReadOnly = True
        TxtNumMethods.Text = Nothing
        TxtDescription.Text = Nothing
        TxtDescription.ReadOnly = True
        ClearMethodFields()
    End Sub

    Private Sub BtnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        'Validate that we at least have a name
        If String.IsNullOrEmpty(TxtProfileName.Text) Then
            MessageBox.Show("You must supply a profile name before creating or updating a profile.")
            Exit Sub
        End If
        Dim profilesPath As String = Nothing
        Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        If m_mode = BA_BAGISP_MODE_PUBLIC Then
            Dim settingsPath As String = bExt.SettingsPath
            profilesPath = BA_GetPublicProfilesPath(settingsPath)
        ElseIf m_aoi IsNot Nothing Then
            profilesPath = BA_GetLocalProfilesDir(m_aoi.FilePath)
        End If
        Try
            If profilesPath IsNot Nothing Then
                'We have a folder to save the profile
                'Trim leading/trailing spaces from profile name
                Dim pName As String = Trim(TxtProfileName.Text)
                'Replace remaining spaces with "_" in name
                pName = pName.Replace(" ", "_")
                'We are updating an existing profile
                If m_selProfile IsNot Nothing Then
                    If m_selProfile.Name <> pName Then
                        'Delete xml record
                        BA_Remove_File(BA_BagisPXmlPath(profilesPath, m_selProfile.Name))
                        m_profileTable.Remove(m_selProfile.Name)
                    End If
                    m_selProfile.Name = pName
                    m_selProfile.Description = Trim(TxtDescription.Text)
                    m_selProfile.Save(BA_BagisPXmlPath(profilesPath, m_selProfile.Name))
                    m_profileTable.Item(pName) = m_selProfile
                Else
                    'We are creating a new profile
                    Dim existingProfile As Profile = m_profileTable(pName)
                    If existingProfile IsNot Nothing Then
                        Dim errMsg As String = "A profile already exists with this name. Please enter a unique name."
                        MessageBox.Show(errMsg, "Duplicate name", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        TxtProfileName.Focus()
                        Exit Sub
                    End If
                    'We are creating a new profile
                    Dim id As Integer = GetNextProfileId()
                    'Determine the profile class based on the form
                    Dim newProfileClass As ProfileClass = ProfileClass.BA_Public
                    If m_mode = BA_BAGISP_MODE_LOCAL Then
                        newProfileClass = ProfileClass.BA_Local
                    End If
                    Dim newProfile As Profile = New Profile(id, pName, newProfileClass, Trim(TxtDescription.Text), bExt.version)
                    m_selProfile = newProfile
                    newProfile.Save(BA_BagisPXmlPath(profilesPath, m_selProfile.Name))
                    m_profileTable.Add(pName, newProfile)
                End If
                ClearMethodFields()
                ClearProfileFields()
                LoadProfileGrid()
            End If
        Catch ex As Exception
            Debug.Print("BtnApply_Click Exception: " & ex.Message)
        End Try
    End Sub

    'Find the highest current id and then add 1 to get the next valid id
    Private Function GetNextProfileId() As Integer
        Dim id As Integer = 0
        For Each key As String In m_profileTable.Keys
            Dim nextProfile As Profile = m_profileTable(key)
            If nextProfile.Id >= id Then
                id = nextProfile.Id
            End If
        Next
        Return id + 1
    End Function

    'Find the highest current id and then add 1 to get the next valid id
    Private Function GetNextMethodId(ByVal methodsFolder) As Integer
        Dim id As Integer = 0
        Dim allMethods As List(Of Method) = BA_LoadMethodsFromXml(methodsFolder)
        If allMethods IsNot Nothing Then
            For Each nextMethod In allMethods
                If nextMethod.Id > id Then
                    id = nextMethod.Id
                End If
            Next
        End If
        Return id + 1
    End Function

    'Find the highest current id and then add 1 to get the next valid id
    Private Function GetNextLocalProfileId(ByVal profilesFolder As String) As Integer
        Dim id As Integer = 0
        Dim profileList As List(Of Profile) = BA_LoadProfilesFromXml(profilesFolder)
        For Each pProfile As Profile In profileList
            If pProfile.Id = id Then
                id = pProfile.Id
            End If
        Next
        Return id + 1
    End Function

    'Loads the profile data from disk
    Private Sub RefreshProfileData()
        Dim profilesFolder As String = Nothing
        m_profileTable = New Hashtable
        If m_aoi IsNot Nothing Then
            profilesFolder = BA_GetLocalProfilesDir(m_aoi.FilePath)
        ElseIf m_mode = BA_BAGISP_MODE_PUBLIC Then
            Dim bExt As BagisPExtension = BagisPExtension.GetExtension
            Dim settingsPath As String = bExt.SettingsPath
            profilesFolder = BA_GetPublicProfilesPath(settingsPath)
        End If
        If Not String.IsNullOrEmpty(profilesFolder) Then
            BA_ValidateProfileNames(profilesFolder)
            Dim profileList As List(Of Profile) = BA_LoadProfilesFromXml(profilesFolder)
            If profileList IsNot Nothing Then
                For Each nextProfile In profileList
                    m_profileTable.Add(nextProfile.Name, nextProfile)
                Next
            End If
        End If
    End Sub

    'Loads the method data from disk
    Private Sub RefreshMethodData()
        Dim methodsFolder As String = Nothing
        If m_aoi IsNot Nothing Then
            methodsFolder = BA_GetLocalMethodsDir(m_aoi.FilePath)
        ElseIf m_mode = BA_BAGISP_MODE_PUBLIC Then
            Dim bExt As BagisPExtension = BagisPExtension.GetExtension
            Dim settingsPath As String = bExt.SettingsPath
            methodsFolder = BA_GetPublicMethodsPath(settingsPath)
        End If
        If Not String.IsNullOrEmpty(methodsFolder) Then
            Dim methodList As List(Of Method) = BA_LoadMethodsFromXml(methodsFolder)
            If methodList IsNot Nothing Then
                m_methodTable = New Hashtable
                For Each nextMethod In methodList
                    If m_aoi IsNot Nothing Then
                        'Set method status for local profile builder
                        nextMethod.Status = MethodStatus.Pending
                    End If
                    m_methodTable.Add(nextMethod.Name, nextMethod)
                Next
            End If
        End If
    End Sub

    Private Sub ManageProfileButtons()
        Dim pCollection As DataGridViewSelectedRowCollection = GrdProfiles.SelectedRows
        If pCollection.Count > 0 Then
            BtnEditProfile.Enabled = True
            BtnProfileDelete.Enabled = True
            BtnProfileCopy.Enabled = True
            BtnAddMethod.Enabled = True
            BtnExportProfile.Enabled = True
        Else
            BtnEditProfile.Enabled = False
            BtnProfileDelete.Enabled = False
            BtnProfileCopy.Enabled = False
            BtnAddMethod.Enabled = False
            BtnApply.Enabled = False
            BtnExportProfile.Enabled = False
        End If
        If m_mode = BA_BAGISP_MODE_PUBLIC Then
            BtnProfileNew.Enabled = True
            BtnMethodNew.Enabled = True
        ElseIf m_aoi Is Nothing Then
            BtnProfileNew.Enabled = False
            BtnMethodNew.Enabled = False
        Else
            BtnProfileNew.Enabled = True
            BtnMethodNew.Enabled = True
            BtnImport.Enabled = True
        End If
    End Sub

    Private Sub GrdProfiles_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GrdProfiles.SelectionChanged
        ClearProfileFields()
        m_selProfile = Nothing
        'A profile is selected
        'Only display the details if only one is selected
        If GrdProfiles.SelectedRows.Count = 1 Then
            Dim pCollection As DataGridViewSelectedRowCollection = GrdProfiles.SelectedRows
            'Always grab the first one to display
            Dim pRow As DataGridViewRow = pCollection.Item(0)
            Dim profileName As String = CStr(pRow.Cells(0).Value)
            m_selProfile = m_profileTable(profileName)
            If m_selProfile IsNot Nothing Then
                LoadMethodGrid()
                TxtProfileName.Text = m_selProfile.Name
                TxtDescription.Text = m_selProfile.Description
            End If
        End If
        ManageProfileButtons()
        ManageVerifyButton()
        ManageRecalculateButton()
    End Sub

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    Private Sub BtnEditProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnEditProfile.Click
        TxtProfileName.ReadOnly = False
        TxtDescription.ReadOnly = False
        BtnApply.Enabled = True
    End Sub

    Private Sub BtnProfileDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnProfileDelete.Click
        Dim profilesPath As String = Nothing
        If m_mode = BA_BAGISP_MODE_PUBLIC Then
            Dim bExt As BagisPExtension = BagisPExtension.GetExtension
            Dim settingsPath As String = bExt.SettingsPath
            profilesPath = BA_GetPublicProfilesPath(settingsPath)
        ElseIf m_aoi IsNot Nothing Then
            profilesPath = BA_GetLocalProfilesDir(m_aoi.FilePath)
        End If
        Dim pCollection As DataGridViewSelectedRowCollection = GrdProfiles.SelectedRows
        For Each pRow As DataGridViewRow In pCollection
            Dim pName As String = CStr(pRow.Cells(0).Value)
            m_profileTable.Remove(pName)
            'Delete xml record
            BA_Remove_File(BA_BagisPXmlPath(profilesPath, pName))
        Next
        m_selProfile = Nothing
        ClearMethodFields()
        ClearProfileFields()
        LoadProfileGrid()
    End Sub

    Private Sub GrdMethods_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GrdMethods.CellContentClick
        'Update the useMethod value in the cached method/hru collection
        Dim colInclude As DataGridViewColumn = GrdMethods.Columns("IncludeMethod")
        Dim colMethod As DataGridViewColumn = GrdMethods.Columns("Methods")
        If e.ColumnIndex = colInclude.Index Then
            'If an aoi and hru are selected
            If m_aoi IsNot Nothing And GrdHruLayers.SelectedRows.Count > 0 Then
                'Check to see if there is a cached method status table from an earlier run
                Dim statusMethodTable As Hashtable = m_methodStatusTable(GetMethodStatusKey(GetSelectedHruName(), m_selProfile.Name))
                If statusMethodTable Is Nothing Then
                    statusMethodTable = New Hashtable
                End If
                For Each pRow As DataGridViewRow In GrdMethods.Rows
                    'Make sure method exists in the hashtable before adding in case it has been deleted
                    Dim methodName As String = pRow.Cells(colMethod.Index).Value
                    Dim hruMethod As Method = statusMethodTable(methodName)
                    If hruMethod Is Nothing Then
                        hruMethod = New Method
                        hruMethod.Name = methodName
                        hruMethod.Status = MethodStatus.Pending
                    End If
                    hruMethod.UseMethod = pRow.Cells(colInclude.Index).EditedFormattedValue
                    If hruMethod.UseMethod = False Then
                        hruMethod.Status = MethodStatus.Pending
                    End If
                    statusMethodTable(methodName) = hruMethod
                Next
                m_methodStatusTable(GetMethodStatusKey(GetSelectedHruName(), m_selProfile.Name)) = statusMethodTable
            End If
            ManageVerifyButton()
            ManageRecalculateButton()
        End If
    End Sub

    Private Sub GrdMethods_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GrdMethods.SelectionChanged
        ClearMethodFields()
        m_selMethod = Nothing

        'Only display the details if only one is selected
        If GrdMethods.SelectedRows.Count = 1 Then
            Dim pCollection As DataGridViewSelectedRowCollection = GrdMethods.SelectedRows
            'Always grab the first one to display
            Dim pRow As DataGridViewRow = pCollection.Item(0)
            Dim methodName As String = CStr(pRow.Cells(0).Value)
            m_selMethod = m_methodTable(methodName)
            If m_selMethod IsNot Nothing Then
                TxtSelMethod.Text = m_selMethod.Name
                TxtToolboxName.Text = m_selMethod.ToolboxName
                TxtModelName.Text = m_selMethod.ModelLabel
            End If
            BtnEditMethod.Enabled = True
            BtnMethodDelete.Enabled = True
        Else
            BtnEditMethod.Enabled = False
            BtnMethodDelete.Enabled = False
        End If
    End Sub

    Private Sub ClearMethodFields()
        TxtSelMethod.Text = Nothing
        TxtToolboxName.Text = Nothing
        TxtModelName.Text = Nothing
        BtnEditMethod.Enabled = False
    End Sub

    Private Sub BtnAddMethod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAddMethod.Click
        Dim frmAddMethod As FrmAddMethod = Nothing
        If m_aoi IsNot Nothing Then
            frmAddMethod = New FrmAddMethod(m_selProfile, m_methodTable, m_aoi.FilePath)
        Else
            frmAddMethod = New FrmAddMethod(m_selProfile, m_methodTable, Nothing)
        End If
        frmAddMethod.ShowDialog()
        ClearMethodFields()
        'If this hru/profile has a method status table associated with it, we need to update it
        If m_aoi IsNot Nothing And GrdHruLayers.SelectedRows.Count > 0 Then
            'Check to see if there is a cached method status table from an earlier run
            Dim statusMethodTable As Hashtable = m_methodStatusTable(GetMethodStatusKey(GetSelectedHruName(), m_selProfile.Name))
            If statusMethodTable IsNot Nothing Then
                If m_selProfile.MethodNames IsNot Nothing Then
                    For Each strName As String In m_selProfile.MethodNames
                        Dim hruMethod As Method = statusMethodTable(strName)
                        'Method not found in cached method status table
                        If hruMethod Is Nothing Then
                            'Get method from main method table
                            Dim pMethod As Method = m_methodTable(strName)
                            'If found, add a copy of the method to the cached method table
                            If pMethod IsNot Nothing Then
                                hruMethod = New Method()
                                With hruMethod
                                    .Name = pMethod.Name
                                    .Status = MethodStatus.Pending
                                End With
                            End If
                            'Set the method in the status method table
                            statusMethodTable(hruMethod.Name) = hruMethod
                        End If
                    Next
                End If
            ElseIf m_selProfile.MethodNames IsNot Nothing Then
                statusMethodTable = New Hashtable
                For Each strName As String In m_selProfile.MethodNames
                    'Get method from main method table
                    Dim pMethod As Method = m_methodTable(strName)
                    Dim hruMethod As Method = New Method()
                    'If found, add a copy of the method to the cached method table
                    If pMethod IsNot Nothing Then
                        With hruMethod
                            .Name = pMethod.Name
                            .Status = MethodStatus.Pending
                        End With
                    End If
                    'Set the method in the status method table
                    statusMethodTable(hruMethod.Name) = hruMethod
                Next
                m_methodStatusTable(GetMethodStatusKey(GetSelectedHruName(), m_selProfile.Name)) = statusMethodTable
            End If
        End If

        LoadMethodGrid()
    End Sub

    Private Sub BtnMethodNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnMethodNew.Click
        Dim frmMethod As FrmEditMethod = Nothing
        If m_aoi IsNot Nothing Then
            frmMethod = New FrmEditMethod(Me, m_aoi.FilePath, m_selProfile)
        Else
            frmMethod = New FrmEditMethod(Me, Nothing, m_selProfile)
        End If
        frmMethod.ShowDialog()
        LoadMethodGrid()
    End Sub

    Private Sub BtnMethodDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnMethodDelete.Click
        Dim pCollection As DataGridViewSelectedRowCollection = GrdMethods.SelectedRows
        If pCollection.Count > 0 Then
            For Each pRow As DataGridViewRow In pCollection
                Dim methodName As String = CStr(pRow.Cells(0).Value)
                Dim profileMethodNames As List(Of String) = m_selProfile.MethodNames
                For Each pName As String In profileMethodNames
                    If pName = methodName Then
                        profileMethodNames.Remove(pName)
                        Exit For
                    End If
                Next
            Next
            ClearMethodFields()
            Dim profilesPath As String = Nothing
            If m_mode = BA_BAGISP_MODE_PUBLIC Then
                Dim bExt As BagisPExtension = BagisPExtension.GetExtension
                Dim settingsPath As String = bExt.SettingsPath
                profilesPath = BA_GetPublicProfilesPath(settingsPath)
            ElseIf m_aoi IsNot Nothing Then
                profilesPath = BA_GetLocalProfilesDir(m_aoi.FilePath)
            End If
            m_selProfile.Save(BA_BagisPXmlPath(profilesPath, m_selProfile.Name))
            LoadMethodGrid()
        End If
    End Sub

    Private Sub BtnEditMethod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnEditMethod.Click
        Dim frmMethod As FrmEditMethod = Nothing
        If m_aoi IsNot Nothing Then
            frmMethod = New FrmEditMethod(Me, m_selProfile, m_selMethod, m_aoi.FilePath)
        Else
            frmMethod = New FrmEditMethod(Me, m_selProfile, m_selMethod, Nothing)
        End If
        frmMethod.ShowDialog()
        'If frmMethod.DirtyFlag = True Then
        '    RefreshMethodData()
        If m_selMethod IsNot Nothing Then
            Dim newMethod As Method = m_methodTable(m_selMethod.Name)
            If newMethod IsNot Nothing Then
                m_selMethod = newMethod
                TxtSelMethod.Text = m_selMethod.Name
                TxtToolboxName.Text = m_selMethod.ToolboxName
                TxtModelName.Text = m_selMethod.ModelLabel
                TxtDescription.Text = m_selMethod.Description
            Else
                'The method was deleted
                m_selMethod = Nothing
                ClearMethodFields()
            End If
            'End If
            LoadMethodGrid()
        End If
    End Sub

    Private Sub ManageRecalculateButton()
        Dim pCollection As DataGridViewSelectedRowCollection = GrdProfiles.SelectedRows
        If pCollection.Count > 0 AndAlso GrdHruLayers.SelectedRows.Count > 0 Then
            If GrdMethods.Rows IsNot Nothing Then
                Dim methodsToRun As Integer = 0
                For Each pRow As DataGridViewRow In GrdMethods.Rows
                    Dim useMethod As Boolean = pRow.Cells("IncludeMethod").EditedFormattedValue
                    Dim strStatus As String = pRow.Cells("ColStatus").Value.ToString
                    If useMethod = True Then
                        Select Case strStatus
                            Case MethodStatus.Verified.ToString
                                methodsToRun += 1
                            Case MethodStatus.Complete.ToString
                                methodsToRun += 1
                            Case Else
                                'Do nothing; This method not ready to run
                        End Select
                    End If
                Next
                If methodsToRun > 0 Then
                    BtnCalculate.Enabled = True
                Else
                    BtnCalculate.Enabled = False
                End If
            End If
        Else
            BtnCalculate.Enabled = False
        End If
    End Sub

    Private Sub ManageVerifyButton()
        If m_mode = BA_BAGISP_MODE_LOCAL Then
            Dim colIncludeMethod As DataGridViewCheckBoxColumn = CType(GrdMethods.Columns("IncludeMethod"), DataGridViewCheckBoxColumn)
            Dim methodCount As Integer
            For Each pRow As DataGridViewRow In GrdMethods.Rows
                Dim pCell As DataGridViewCell = pRow.Cells(colIncludeMethod.Index)
                If pCell.EditedFormattedValue = True Then
                    methodCount += 1
                End If
            Next
            If GrdHruLayers.SelectedRows.Count > 0 AndAlso methodCount > 0 Then
                BtnVerify.Enabled = True
            Else
                BtnVerify.Enabled = False
            End If
            ManageIncludeColumn()
        Else
            If GrdMethods.Rows.Count > 0 Then
                BtnVerify.Enabled = True
            Else
                BtnVerify.Enabled = False
            End If
        End If
    End Sub

    Private Sub ManageIncludeColumn()
        Dim colIncludeMethod As DataGridViewCheckBoxColumn = CType(GrdMethods.Columns("IncludeMethod"), DataGridViewCheckBoxColumn)
        If GrdProfiles.SelectedCells.Count > 0 AndAlso GrdHruLayers.SelectedRows.Count > 0 Then
            colIncludeMethod.ReadOnly = False
            colIncludeMethod.FlatStyle = FlatStyle.Standard
            colIncludeMethod.DefaultCellStyle.ForeColor = GrdMethods.Columns("Methods").DefaultCellStyle.ForeColor
        Else
            colIncludeMethod.ReadOnly = True
            colIncludeMethod.FlatStyle = FlatStyle.Flat
            colIncludeMethod.DefaultCellStyle.ForeColor = Color.Silver
        End If
    End Sub

    Private Sub GrdHruLayers_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GrdHruLayers.SelectionChanged
        If GrdProfiles.SelectedRows.Count > 0 And GrdHruLayers.SelectedRows.Count > 0 Then
            LoadMethodGrid()
        End If
        LoadCompletedProfileGrid()
        ManageVerifyButton()
        ManageRecalculateButton()
    End Sub

    Private Sub BtnCalculate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCalculate.Click
        BtnCalculate.Enabled = False
        BtnVerify.Enabled = False
        Try
            If GrdHruLayers.SelectedRows.Count > 0 And m_selProfile IsNot Nothing Then
                If m_selProfile.MethodNames IsNot Nothing And m_selProfile.MethodNames.Count > 0 Then
                    Dim success As BA_ReturnCode = PopulateModelParameters()
                    If success <> BA_ReturnCode.Success Then
                        Exit Sub
                    End If
                    LblStatus.Text = "Checking target geodatabase"
                    Dim selHruName As String = GetSelectedHruName()
                    'Append OMS ID to zone shapefile if it doesn't exist
                    Dim hruGdbName As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, selHruName)
                    Dim vName As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruZonesVector), False)
                    'Dim fClassHasERamsId As Boolean = BA_FeatureClassHasERamsId(hruGdbName, vName)
                    'If fClassHasERamsId = False Then
                    '    Dim tempTableName As String = "eRamsTable"
                    '    Dim snapRasterPath As String = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Aoi, True) & BA_EnumDescription(AOIClipFile.AOIExtentCoverage)
                    '    Dim flowAccumPath As String = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Surfaces, True) & BA_EnumDescription(MapsFileName.flow_accumulation_gdb)
                    '    success = BA_ZonalStatisticsAsTable(hruGdbName, vName, BA_FIELD_HRU_ID, flowAccumPath, _
                    '                                                             hruGdbName, tempTableName, snapRasterPath, StatisticsTypeString.MAXIMUM)
                    '    If success = BA_ReturnCode.Success Then
                    '        BA_AppendERamsIdToFeatureClass(hruGdbName, vName, tempTableName)
                    '        'Delete temporary statistics table
                    '        BA_Remove_TableFromGDB(hruGdbName, tempTableName)
                    '    End If
                    'End If
                    Dim paramTable As String = m_selProfile.Name & BA_PARAM_TABLE_SUFFIX
                    'Append ERAMS_ID to parameter table
                    'BA_AppendERamsIdToParameterTable(hruGdbName, paramTable)
                    If success = BA_ReturnCode.Success Then
                        For Each mName As String In m_selProfile.MethodNames
                            Dim pMethod As Method = m_methodTable(mName)
                            Dim hruMethodTable As Hashtable = m_methodStatusTable(GetMethodStatusKey(selHruName, m_selProfile.Name))
                            Dim hruMethod As Method = hruMethodTable(mName)
                            If pMethod IsNot Nothing AndAlso hruMethod.Status = MethodStatus.Verified AndAlso hruMethod.UseMethod = True Then
                                LblStatus.Text = "Calculating " & mName
                                Dim errorMessage As String = Nothing
                                Dim scratchDir As String = m_aoi.FilePath & BA_EnumDescription(PublicPath.BagisPDefaultWorkspace)
                                success = BA_RunModelFromMethodFilledParameters(pMethod, scratchDir, errorMessage)
                                'Create and populate a new method for this profile/hru combination
                                With hruMethod
                                    .Name = pMethod.Name
                                    .Status = pMethod.Status
                                End With
                                If success = BA_ReturnCode.Success Then
                                    hruMethod.Status = MethodStatus.Complete
                                Else
                                    hruMethod.Status = MethodStatus.Failed
                                End If
                                hruMethod.ErrorMessage = errorMessage
                                UpdateMethodStatusOnGrid(hruMethod)
                                If hruMethodTable Is Nothing Then
                                    hruMethodTable = New Hashtable
                                End If
                                hruMethodTable(hruMethod.Name) = hruMethod
                                m_methodStatusTable(GetMethodStatusKey(selHruName, m_selProfile.Name)) = hruMethodTable
                            End If
                        Next
                    End If
                    'If success = BA_ReturnCode.Success Then
                    Dim hruPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, selHruName)
                    Dim hruParamPath As String = hruPath & BA_EnumDescription(PublicPath.BagisParamGdb) & BA_EnumDescription(PublicPath.HruProfileStatus)
                    'Append SUB_AOI_ID to parameter table if desired
                    If CkAppendSubAoi.Checked = True Then
                        If CboSubAoiId.SelectedIndex > -1 Then
                            Dim subAOIIdLayer As String = CStr(CboSubAoiId.SelectedItem)
                            LblStatus.Text = "Appending SubAOI ID to parameter table"
                            Dim snapRasterPath As String = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Aoi, True) & BA_EnumDescription(AOIClipFile.AOIExtentCoverage)
                            BA_AppendSubAOIIdToParameterTable(m_aoi.FilePath, selHruName, subAOIIdLayer, paramTable, snapRasterPath)
                        End If
                    End If
                    'Replace no data values with -99 because OMS-PRMS cannot handle null values
                    success = BA_ReplaceNoDataValuesInTable(hruPath & BA_EnumDescription(PublicPath.BagisParamGdb), paramTable, TxtNoData.Text)
                    Dim dateCompleted As Date = Date.Now
                    'Save completion timestamp to disk at aoi|profile level
                    success = BA_SaveHruProfileStatus(hruParamPath, m_selProfile.Name, dateCompleted)
                    'Update timestamp on UI (after completion timestamp is persisted)
                    LoadCompletedProfileGrid()
                    'Save settings to XML
                    Dim saveTable As Hashtable = m_methodStatusTable(GetMethodStatusKey(selHruName, m_selProfile.Name))
                    success = BA_SaveProfileLog(hruPath, m_selProfile, TxtNoData.Text, m_methodTable, saveTable)
                    ManageVerifyButton()
                    ManageRecalculateButton()
                    LblStatus.Text = "Parameter calculations complete"
                End If
            End If

        Catch ex As Exception
            LblStatus.Text = "Parameter calculations failed"
            Debug.Print("BtnCalculate_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub BtnExportToAoi_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim formText As String = "Select an AOI or multiple AOIs to receive profiles and methods."
        Dim frmSelectAoi As New FrmSelectAoi(formText)
        frmSelectAoi.ShowDialog()
        Dim aoiPathList As List(Of String) = frmSelectAoi.AoiPathList
        If aoiPathList IsNot Nothing AndAlso aoiPathList.Count > 0 Then
            Dim pCollection As DataGridViewSelectedRowCollection = GrdProfiles.SelectedRows
            If pCollection.Count > 0 Then
                For Each aoiPath As String In aoiPathList
                    '1. Get a selected profile out of m_profileTable
                    For Each pRow As DataGridViewRow In pCollection
                        Dim pName As String = CStr(pRow.Cells(0).Value)
                        Dim copyProfile As Profile = m_profileTable(pName)
                        If copyProfile IsNot Nothing Then
                            '2. Create a new profile from the original; Change properties that are specific
                            'to the local profile
                            Dim profilePath As String = BA_GetLocalProfilesDir(aoiPath)
                            Dim profileId As Integer = GetNextLocalProfileId(profilePath)
                            Dim pExt As BagisPExtension = BagisPExtension.GetExtension
                            Dim exProfile As Profile = New Profile(profileId, copyProfile.Name, _
                                                                   ProfileClass.BA_Local, copyProfile.Description, pExt.version)
                            Dim exMethodList As List(Of String) = New List(Of String)
                            '3. Get the method path for the aoi
                            Dim methodPath As String = BA_GetLocalMethodsDir(aoiPath)
                            '4. For each associated method in m_methodTable
                            For Each mName As String In copyProfile.MethodNames
                                Dim copyMethod As Method = m_methodTable(mName)
                                If copyMethod IsNot Nothing Then
                                    Dim methodId As Integer = GetNextMethodId(methodPath)
                                    '5. Create a copy of the method so we don't mutate the original
                                    Dim exMethod As Method = New Method(methodId, copyMethod.Name, _
                                                                        copyMethod.Description, copyMethod.ToolboxName, _
                                                                        copyMethod.ModelName, copyMethod.ModelLabel, copyMethod.ToolBoxPath)
                                    exMethod.ModelParameters = copyMethod.ModelParameters
                                    '6. Save the method to the aoi
                                    exMethod.Save(BA_BagisPXmlPath(methodPath, exMethod.Name))
                                    exMethodList.Add(exMethod.Name)
                                End If
                            Next
                            '7. Save the profile to the aoi
                            exProfile.MethodNames = exMethodList
                            exProfile.Save(BA_BagisPXmlPath(profilePath, exProfile.Name))
                        End If
                    Next
                Next
                MessageBox.Show("Profile export is complete!")
                Exit Sub
            End If
        End If
        MessageBox.Show("Profile export failed.")
    End Sub

    Private Sub BtnImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnImport.Click
        Dim frmSelectProfile As FrmSelectProfile = New FrmSelectProfile(m_profileTable, m_aoi.FilePath)
        frmSelectProfile.ShowDialog()
        If frmSelectProfile.DirtyFlag = True Then
            RefreshProfileData()
            RefreshMethodData()
            ClearMethodFields()
            ClearProfileFields()
            LoadProfileGrid()
        End If
    End Sub

    Private Sub BtnProfileCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnProfileCopy.Click
        Dim profilesPath As String = Nothing
        Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        If m_mode = BA_BAGISP_MODE_PUBLIC Then
            Dim settingsPath As String = bExt.SettingsPath
            profilesPath = BA_GetPublicProfilesPath(settingsPath)
        ElseIf m_aoi IsNot Nothing Then
            profilesPath = BA_GetLocalProfilesDir(m_aoi.FilePath)
        End If
        Dim pCollection As DataGridViewSelectedRowCollection = GrdProfiles.SelectedRows
        For Each pRow As DataGridViewRow In pCollection
            Dim pName As String = CStr(pRow.Cells(0).Value)
            Dim srcProfile As Profile = m_profileTable(pName)
            If srcProfile IsNot Nothing Then
                Dim copyProfileName As String = Trim(InputBox("Please supply a unique profile name:" & vbCrLf, "Profile Name", ""))
                'Replace remaining spaces with "_" in name
                copyProfileName = copyProfileName.Replace(" ", "_")
                Dim copyProfile As Profile = m_profileTable(copyProfileName)
                Do While copyProfile IsNot Nothing
                    'Keep prompting the user until they pick a unique profile name
                    Dim sb As StringBuilder = New StringBuilder
                    sb.Append("A profile named " & copyProfileName & " already exists." & vbCrLf)
                    sb.Append("Please supply a unique profile name:" & vbCrLf)
                    copyProfileName = Trim(InputBox(sb.ToString, "Profile Name", ""))
                    copyProfileName = copyProfileName.Replace(" ", "_")
                    copyProfile = m_profileTable(copyProfileName)
                Loop
                'Get unique key
                Dim id As Integer = GetNextProfileId()
                'Because this is the public profile screen, the class is always public
                copyProfile = New Profile(id, copyProfileName, srcProfile.ProfileClass, srcProfile.Description, bExt.version)
                If srcProfile.MethodNames IsNot Nothing Then
                    Dim copyMethods As List(Of String) = New List(Of String)
                    For Each srcMethod As String In srcProfile.MethodNames
                        copyMethods.Add(srcMethod)
                    Next
                    copyProfile.MethodNames = copyMethods
                End If
                copyProfile.Save(BA_BagisPXmlPath(profilesPath, copyProfileName))
                m_profileTable.Add(copyProfileName, copyProfile)
            End If
        Next
        m_selProfile = Nothing
        ClearMethodFields()
        ClearProfileFields()
        LoadProfileGrid()
    End Sub

    Friend Property MethodTable As Hashtable
        Get
            Return m_methodTable
        End Get
        Set(ByVal value As Hashtable)
            m_methodTable.Clear()
            If value IsNot Nothing Then
                For Each pKey In value.Keys
                    m_methodTable.Add(pKey, value(pKey))
                Next
            End If
        End Set
    End Property

    'Populates the parameters in the model
    Private Function PopulateModelParameters() As BA_ReturnCode
        Dim pModel As IGPTool = Nothing
        Dim params As List(Of ModelParameter) = Nothing
        Dim selHruName As String = GetSelectedHruName()
        Dim hruPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, selHruName)
        Dim pMethod As Method = Nothing
        Try
            Dim success As BA_ReturnCode = BA_ReturnCode.Success
            Dim hruMethodTable As Hashtable = m_methodStatusTable(GetMethodStatusKey(selHruName, m_selProfile.Name))
            For Each mName As String In m_selProfile.MethodNames
                Dim hruMethod As Method = hruMethodTable(mName)
                pMethod = m_methodTable(mName)
                If pMethod IsNot Nothing AndAlso hruMethod IsNot Nothing AndAlso _
                    hruMethod.Status = MethodStatus.Verified AndAlso hruMethod.UseMethod = True Then
                    Dim pFilledParameters As List(Of ModelParameter) = New List(Of ModelParameter)
                    pModel = BA_OpenModel(pMethod.ToolBoxPath, pMethod.ToolboxName, pMethod.ModelName)
                    If pModel IsNot Nothing Then
                        params = BA_GetModelParameters(pModel)
                        If params IsNot Nothing Then
                            'Re-enable for debugging purposes
                            'Dim sb As New StringBuilder
                            For Each pParam As ModelParameter In params
                                Dim pValue As String = BA_CalculateSystemParameter(pParam.Name, hruPath, m_selProfile.Name, m_dataTable)
                                If pValue Is Nothing Then
                                    'The parameter was not a system parameter
                                    pValue = BA_CalculateDbParameter(m_aoi.FilePath, m_dataTable, pParam.Name, pMethod)
                                    If pValue = Nothing Then
                                        'The parameter was not a databin parameter either; Pass value through
                                        pValue = pParam.Value
                                    End If
                                End If
                                'sb.Append(pParam.Name & ": " & pValue & vbCrLf)
                                pParam.Value = pValue
                                pFilledParameters.Add(pParam)
                            Next
                            'MsgBox(sb.ToString)
                        End If
                        'Set filled parameters on Method
                        pMethod.FilledModelParameters = pFilledParameters
                        'm_methodTable(pMethod.Name) = pMethod
                    Else
                        UpdateMethodStatus(pMethod, selHruName, "Unable to open model")
                        success = BA_ReturnCode.ReadError
                    End If
                End If
            Next
            Return success
        Catch ex As Exception
            Debug.Print("PopulateModelParameters Exception: " & ex.Message)
            UpdateMethodStatus(pMethod, selHruName, "Unable to populate model parameters")
            Return BA_ReturnCode.UnknownError
        Finally
            pModel = Nothing
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Function

    Private Sub UpdateMethodStatus(ByVal pMethod As Method, ByVal hruName As String, ByVal errorMessage As String)
        'Create and populate a new method for this profile/hru combination
        Dim hruMethod As Method = New Method()
        With hruMethod
            .Name = pMethod.Name
            .Status = MethodStatus.Failed
            .ErrorMessage = errorMessage
        End With
        UpdateMethodStatusOnGrid(hruMethod)
        Dim hruMethodTable As Hashtable = m_methodStatusTable(GetMethodStatusKey(hruName, m_selProfile.Name))
        If hruMethodTable Is Nothing Then
            hruMethodTable = New Hashtable
        End If
        hruMethodTable(hruMethod.Name) = hruMethod
        m_methodStatusTable(GetMethodStatusKey(hruName, m_selProfile.Name)) = hruMethodTable
    End Sub

    Private Sub UpdateMethodStatusOnGrid(ByVal aMethod As Method)
        Dim idxName As Integer = -1
        Dim idxStatus As Integer = -1
        Dim idxIncMethod As Integer = -1
        Dim i As Integer
        'Get the column indexes to update
        For Each pCol As DataGridViewColumn In GrdMethods.Columns
            If pCol.Name = "Methods" Then
                idxName = i
            ElseIf pCol.Name = "ColStatus" Then
                idxStatus = i
            ElseIf pCol.Name = "IncludeMethod" Then
                idxIncMethod = i
            End If
            i += 1
        Next
        For Each pRow As DataGridViewRow In GrdMethods.Rows
            'This is the row with the right method
            If pRow.Cells(idxName).Value = aMethod.Name Then
                'Update the status
                pRow.Cells(idxStatus).Value = aMethod.Status.ToString
                pRow.Cells(idxStatus).ToolTipText = aMethod.ErrorMessage
                pRow.Cells(idxIncMethod).Value = aMethod.UseMethod
            End If
        Next
    End Sub

    Private Function GetMethodStatusKey(ByVal hruName As String, ByVal profileName As String) As String
        Return hruName & "|" & profileName
    End Function

    Private Sub BtnVerify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnVerify.Click
        If m_mode = BA_BAGISP_MODE_PUBLIC Then
            VerifyPublicProfile()
        Else
            VerifyLocalProfile()
        End If
    End Sub

    Private Sub VerifyLocalProfile()
        ' Create/configure a step progressor
        'Dim pStepProg As IStepProgressor = Nothing
        'Dim progressDialog2 As IProgressDialog2 = Nothing
        Dim pModel As IGPTool
        Try
            If m_selProfile IsNot Nothing Then
                BtnVerify.Enabled = False
                'pStepProg = BA_GetStepProgressor(My.ArcMap.Application.hWnd, m_selProfile.MethodNames.Count + 4)
                'progressDialog2 = BA_GetProgressDialog(pStepProg, "Checking target geodatabase...", "Verifying profile " & m_selProfile.Name)
                'pStepProg.Show()
                'progressDialog2.ShowDialog()
                'pStepProg.Step()
                LblStatus.Text = "Verifying attribute table for AOI"
                Dim success As BA_ReturnCode = BA_ReturnCode.UnknownError
                success = VerifyAOIAttributeTable()
                If success <> BA_ReturnCode.Success Then
                    MessageBox.Show("Unable to verify attribute for AOI: " & m_aoi.Name, "Bad attribute table", MessageBoxButtons.OK, _
                        MessageBoxIcon.Error)
                    Exit Sub
                End If

                LblStatus.Text = "Checking target geodatabase..."
                Dim mList As List(Of String) = m_selProfile.MethodNames
                If mList IsNot Nothing AndAlso mList.Count > 0 Then
                    'If an aoi and hru are selected
                    Dim selHruName As String = GetSelectedHruName()

                    'Create params.gdb in hru if it doesn't exist; This is where we store the analysis data layers
                    Dim hruPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, selHruName)
                    Dim hruParamPath As String = hruPath & BA_EnumDescription(PublicPath.BagisParamGdb)
                    If Not BA_Folder_ExistsWindowsIO(hruParamPath) Then
                        Dim gdbName As String = BA_GetBareName(hruParamPath)
                        success = BA_CreateFileGdb(hruPath, gdbName)
                    Else
                        success = BA_ReturnCode.Success
                    End If
                    'If we were able to create the params.gdb, we create the Default.gdb in the AOI
                    'This is where ModelBuilder will store temporary data layers
                    If success = BA_ReturnCode.Success Then
                        'Create Default.gdb in aoi if it doesn't exist
                        success = BA_CheckDefaultWorkspace(m_aoi.FilePath)
                        If success <> BA_ReturnCode.Success Then
                            MessageBox.Show("Unable to create default.gdb for AOI: " & m_aoi.Name, "Write error", MessageBoxButtons.OK, _
                                            MessageBoxIcon.Error)
                            Exit Sub
                        End If
                    Else
                        MessageBox.Show("Unable to create params.gdb for AOI: " & m_aoi.Name, "Write error", MessageBoxButtons.OK, _
                                        MessageBoxIcon.Error)
                        Exit Sub
                    End If

                    Dim methodsToRun As Integer = 0
                    Dim validMethodsTable As Hashtable = New Hashtable
                    For Each pName In mList
                        'Make sure method exists in the hashtable before adding in case it has been deleted
                        If m_methodTable(pName) IsNot Nothing Then
                            'progressDialog2.Description = "Validating method " & pName
                            'pStepProg.Step()
                            LblStatus.Text = "Validating method " & pName
                            Dim pMethod As Method = m_methodTable(pName)

                            'Track the "use Method" value from the grid
                            Dim hruMethodTable As Hashtable = m_methodStatusTable(GetMethodStatusKey(selHruName, m_selProfile.Name))
                            If hruMethodTable Is Nothing Then
                                hruMethodTable = New Hashtable
                            End If
                            Dim hruMethod As Method = hruMethodTable(pName)
                            If hruMethod Is Nothing Then
                                hruMethod = New Method
                                hruMethod.Name = pName
                            End If
                            hruMethod.Status = MethodStatus.Pending

                            'First check to see if the user wants to use the method
                            For Each pRow As DataGridViewRow In GrdMethods.Rows
                                'This is the row with the right method
                                If pRow.Cells("Methods").Value = pName Then
                                    hruMethod.UseMethod = pRow.Cells("IncludeMethod").EditedFormattedValue
                                    Exit For
                                End If
                            Next

                            Dim sb As StringBuilder = New StringBuilder
                            '************************************************
                            '* Any new validations added here should also be added to FrmExportProfile
                            '************************************************
                            If hruMethod.UseMethod = True Then
                                'Validate existence of model on disk
                                pModel = BA_OpenModel(pMethod.ToolBoxPath, pMethod.ToolboxName, pMethod.ModelName)
                                If pModel IsNot Nothing Then
                                    hruMethod.Status = MethodStatus.Verified
                                Else
                                    hruMethod.Status = MethodStatus.Invalid
                                    hruMethod.UseMethod = False
                                    sb.Append("Unable to open model")
                                End If
                                If pModel IsNot Nothing Then
                                    'Validate local data source presence
                                    Dim params As List(Of ModelParameter) = BA_GetModelParameters(pModel)
                                    If params IsNot Nothing Then
                                        Dim sourceExists As Boolean = True
                                        Dim dsName As String = Nothing
                                        For Each pParam As ModelParameter In params
                                            'Only run function if paramName has "db_" prefix
                                            If pParam.Name.Substring(0, BA_DATABIN_PREFIX.Length).ToLower = BA_DATABIN_PREFIX Then
                                                Dim pValue As String = BA_CalculateDbParameter(m_aoi.FilePath, m_dataTable, pParam.Name, pMethod)
                                                If Not String.IsNullOrEmpty(pValue) Then
                                                    'Get the name of the data source so we can access it from the data source manager
                                                    dsName = BA_GetParamValueFromMethod(pMethod, pParam.Name)
                                                    Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(pValue)
                                                    'First try to open it as a raster
                                                    sourceExists = BA_File_Exists(pValue, wType, ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTRasterDataset)
                                                    'Follow-up by trying to open it as a vector
                                                    If sourceExists = False Then
                                                        sourceExists = BA_File_Exists(pValue, wType, ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureClass)
                                                    End If
                                                    If sourceExists = False Then
                                                        hruMethod.Status = MethodStatus.Invalid
                                                        hruMethod.UseMethod = False
                                                        sb.Append("Unable to locate associated data source")
                                                    End If
                                                Else
                                                    hruMethod.Status = MethodStatus.Invalid
                                                    hruMethod.UseMethod = False
                                                    sb.Append("Unable to locate associated data source")
                                                End If
                                                'Check for unit conflict if units are present with custom DS
                                                If Not String.IsNullOrEmpty(dsName) Then
                                                    Dim pDataSource As DataSource = m_dataTable(dsName)
                                                    For Each dataParam As ModelParameter In params
                                                        Dim modelParam As SystemModelParameterName = BA_GetSystemModelParameterName(dataParam.Name)
                                                        Select Case modelParam
                                                            Case SystemModelParameterName.sys_units_elevation
                                                                If pDataSource.MeasurementUnitType = MeasurementUnitType.Elevation Then
                                                                    If pDataSource.MeasurementUnit <> m_elevUnit Then
                                                                        hruMethod.Status = MethodStatus.Invalid
                                                                        hruMethod.UseMethod = False
                                                                        sb.Append("Elevation units of data source " & dsName & " don't match AOI elevation units")
                                                                    End If
                                                                End If
                                                            Case SystemModelParameterName.sys_units_slope
                                                                If pDataSource.MeasurementUnitType = MeasurementUnitType.Slope Then
                                                                    If pDataSource.SlopeUnit <> m_slopeUnit Then
                                                                        hruMethod.Status = MethodStatus.Invalid
                                                                        hruMethod.UseMethod = False
                                                                        sb.Append("Slope units of data source " & dsName & " don't match AOI elevation units")
                                                                    End If
                                                                End If
                                                            Case SystemModelParameterName.sys_units_depth
                                                                If pDataSource.MeasurementUnitType = MeasurementUnitType.Depth Then
                                                                    If pDataSource.MeasurementUnit <> m_depthUnit Then
                                                                        hruMethod.Status = MethodStatus.Invalid
                                                                        hruMethod.UseMethod = False
                                                                        sb.Append("Depth units of data source " & dsName & " don't match AOI elevation units")
                                                                    End If
                                                                End If
                                                        End Select
                                                    Next
                                                End If
                                            End If
                                        Next
                                        'Convert the slope layer if we need to
                                        'Also make sure the temperature units are present if used. This is also a system model parameter
                                        'Other units are verified at the AOI level when the form loads
                                        If params IsNot Nothing Then
                                            For Each pParam As ModelParameter In params
                                                Dim modelParam As SystemModelParameterName = BA_GetSystemModelParameterName(pParam.Name)
                                                If modelParam = SystemModelParameterName.sys_slope_path AndAlso _
                                                    m_slopeUnit = SlopeUnit.Degree Then
                                                    'progressDialog2.Description = "Recalculating slope layer to use percent"
                                                    'pStepProg.Step()
                                                    LblStatus.Text = "Recalculating slope layer to use percent"
                                                    Dim surfacesFolder As String = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Surfaces)
                                                    Dim demPath As String = surfacesFolder & "\" & BA_EnumDescription(MapsFileName.filled_dem_gdb)
                                                    Dim slopePath As String = surfacesFolder & "\" & BA_GetBareName(BA_EnumDescription(PublicPath.Slope))
                                                    Dim aoiFolder = BA_GeodatabasePath(m_aoi.FilePath, GeodatabaseNames.Aoi)
                                                    Dim snapRasterPath As String = aoiFolder & BA_EnumDescription(PublicPath.AoiGrid)
                                                    Dim newSlopeUnit As SlopeUnit = SlopeUnit.PctSlope
                                                    success = BA_Calculate_Slope(demPath, slopePath, newSlopeUnit, snapRasterPath)
                                                    If success = BA_ReturnCode.Success Then
                                                        'We successfully create the new slope layer; Now we need to update the metadata
                                                        m_slopeUnit = newSlopeUnit
                                                        Dim sTag As StringBuilder = New StringBuilder
                                                        sTag.Append(BA_BAGIS_TAG_PREFIX)
                                                        sTag.Append(BA_ZUNIT_CATEGORY_TAG & MeasurementUnitType.Slope.ToString & "; ")
                                                        sTag.Append(BA_ZUNIT_VALUE_TAG & BA_EnumDescription(newSlopeUnit) & ";")
                                                        sTag.Append(BA_BAGIS_TAG_SUFFIX)
                                                        success = BA_UpdateMetadata(surfacesFolder, BA_GetBareName(BA_EnumDescription(PublicPath.Slope)), _
                                                                          LayerType.Raster, BA_XPATH_TAGS, _
                                                                          sTag.ToString, BA_BAGIS_TAG_PREFIX.Length)
                                                        If success <> BA_ReturnCode.Success Then
                                                            hruMethod.Status = MethodStatus.Invalid
                                                            hruMethod.UseMethod = False
                                                            sb.Append("Unable to set units on new slope layer")
                                                        End If
                                                    Else
                                                        hruMethod.Status = MethodStatus.Invalid
                                                        hruMethod.UseMethod = False
                                                        sb.Append("Unable to convert slope layer to percent")
                                                    End If
                                                ElseIf modelParam = SystemModelParameterName.sys_units_temperature Then
                                                    Dim pValue As String = BA_CalculateSystemParameter(pParam.Name, hruPath, m_selProfile.Name, m_dataTable)
                                                    Dim pUnits As MeasurementUnit = BA_GetMeasurementUnit(pValue)
                                                    If pUnits = MeasurementUnit.Missing Then
                                                        hruMethod.Status = MethodStatus.Invalid
                                                        hruMethod.UseMethod = False
                                                        sb.Append("No temperature units have been defined")
                                                    End If
                                                End If
                                            Next
                                        End If
                                    End If
                                End If
                            End If
                            hruMethod.ErrorMessage = sb.ToString
                            UpdateMethodStatusOnGrid(hruMethod)
                            If hruMethod.Status = MethodStatus.Verified Then
                                methodsToRun += 1
                                validMethodsTable(pMethod.Name) = pMethod
                            End If
                            hruMethodTable(hruMethod.Name) = hruMethod
                            m_methodStatusTable(GetMethodStatusKey(selHruName, m_selProfile.Name)) = hruMethodTable
                        End If
                    Next
                    If methodsToRun > 0 Then
                        'Check to see if 'grid_zones_v' shapefile is present. This is a vector representation of the hru zones grid used by BAGIS-P
                        'If it doesn't exist, create it
                        'progressDialog2.Description = "Creating vector copy of grid for BAGIS-P"
                        'pStepProg.Step()
                        LblStatus.Text = "Creating vector copy of grid for BAGIS-P"
                        Dim hruGdbPath As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, selHruName)
                        If Not BA_File_Exists(hruGdbPath & BA_EnumDescription(PublicPath.HruZonesVector), WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                            'Convert raster grid to 'grid_zones_v' shapefile
                            Dim retVal As Short = BA_Raster2PolygonShapefileFromPath(hruGdbPath & BA_EnumDescription(PublicPath.HruGrid), hruGdbPath & BA_EnumDescription(PublicPath.HruZonesVector), False)
                            If retVal = 1 Then
                                Dim zonesVectorName As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruZonesVector), False)
                                Dim tempVectorName As String = "dissolve_v"
                                'If successful, add HRU_ID column and copy values from 'grid_code' column
                                success = BA_CreateHruIdField(hruGdbPath, zonesVectorName)
                                'If this is a non-contiguous HRU, we need to dissolve on HRU ID
                                If success = BA_ReturnCode.Success AndAlso BA_IsNonContiguousHru(hruGdbPath, zonesVectorName, BA_GetBareName(BA_EnumDescription(PublicPath.HruGrid))) = True Then
                                    success = BA_Dissolve(hruGdbPath & "\" & zonesVectorName, BA_FIELD_HRU_ID, hruGdbPath & "\" & tempVectorName)
                                    If success = BA_ReturnCode.Success Then
                                        'After dissolving, remove the original shapefile
                                        retVal = BA_Remove_ShapefileFromGDB(hruGdbPath, zonesVectorName)
                                        If retVal = 1 Then
                                            'And rename the dissolved file to grid_zones_v
                                            BA_RenameFeatureClassInGDB(hruGdbPath, tempVectorName, zonesVectorName)
                                        End If
                                    End If
                                End If
                            End If
                        End If
                        ''Check to see if params table exists for selected profile
                        'If it does drop and recreate so we only have columns from this run
                        'progressDialog2.Description = "Verifying output parameter table"
                        'pStepProg.Step()
                        LblStatus.Text = "Verifying output parameter table"
                        If success = BA_ReturnCode.Success Then
                            success = BA_CheckParamsTable(hruPath, m_selProfile.Name, True)
                        Else
                            MessageBox.Show("Unable to create parameter output table for HRU: " & selHruName, "Write error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Exit Sub
                        End If
                    Else
                        MessageBox.Show("No valid methods are associated with this HRU", "No valid methods", MessageBoxButtons.OK, _
                                        MessageBoxIcon.Error)
                        Exit Sub
                    End If
                    ManageVerifyButton()
                    ManageRecalculateButton()
                    LblStatus.Text = "Verification complete"
                    Dim errorMsg As String = CheckForDuplicateFieldNames(validMethodsTable)
                    'We have duplicate field names; Pop a warning message
                    If Not String.IsNullOrEmpty(errorMsg) Then
                        MessageBox.Show(errorMsg, "Duplicate field names", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                End If
            End If
        Catch ex As Exception
            Debug.Print("VerifyLocalProfile: " & ex.Message)
            LblStatus.Text = "Verification failed"
        Finally
            'The step progressor will be undefined if the cancel button was clicked
            'If pStepProg IsNot Nothing Then
            '    pStepProg.Hide()
            '    pStepProg = Nothing
            '    progressDialog2.HideDialog()
            '    progressDialog2 = Nothing
            'End If
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Sub

    Private Sub VerifyPublicProfile()
        ' Create/configure a step progressor
        Dim pModel As IGPTool
        Try
            If m_selProfile IsNot Nothing Then
                BtnVerify.Enabled = False
                LblStatus.Text = "Verifying profile " & m_selProfile.Name
                Dim mList As List(Of String) = m_selProfile.MethodNames
                If mList IsNot Nothing AndAlso mList.Count > 0 Then
                    'Populate data table
                    m_dataTable = BA_LoadSettingsFile(BA_GetBagisPSettingsPath())

                    'Get the status method from the Hashtable
                    Dim verifyMethodTable As Hashtable = m_methodStatusTable(m_selProfile.Name)
                    If verifyMethodTable Is Nothing Then
                        verifyMethodTable = New Hashtable
                    End If

                    For Each pName In mList

                        'Make sure method exists in the hashtable before adding in case it has been deleted
                        If m_methodTable(pName) IsNot Nothing Then
                            LblStatus.Text = "Validating method " & pName
                            Dim pMethod As Method = m_methodTable(pName)
                            'Get verify method from the status hashtable
                            Dim vMethod As Method = verifyMethodTable(pName)
                            If vMethod Is Nothing Then
                                vMethod = New Method
                                vMethod.Name = pName
                            End If
                            vMethod.Status = MethodStatus.Pending

                            Dim sb As StringBuilder = New StringBuilder
                            'Validate existence of model on disk
                            pModel = BA_OpenModel(pMethod.ToolBoxPath, pMethod.ToolboxName, pMethod.ModelName)
                            If pModel IsNot Nothing Then
                                vMethod.Status = MethodStatus.Verified
                            Else
                                vMethod.Status = MethodStatus.Invalid
                                sb.Append("Unable to open model")
                            End If

                            'Validate local data source presence
                            If pModel IsNot Nothing Then
                                Dim params As List(Of ModelParameter) = BA_GetModelParameters(pModel)
                                If params IsNot Nothing Then
                                    Dim sourceExists As Boolean = True
                                    Dim dsName As String = Nothing
                                    For Each pParam As ModelParameter In params
                                        'Only run function if paramName has "db_" prefix
                                        If pParam.Name.Substring(0, BA_DATABIN_PREFIX.Length).ToLower = BA_DATABIN_PREFIX Then
                                            'Get the name of the data source so we can access it from the data source manager
                                            dsName = BA_GetParamValueFromMethod(pMethod, pParam.Name)
                                            If Not String.IsNullOrEmpty(dsName) Then
                                                Dim dSource As DataSource = m_dataTable(dsName)
                                                If dSource IsNot Nothing Then
                                                    Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(dSource.Source)
                                                    'First try to open it as a raster
                                                    sourceExists = BA_File_Exists(dSource.Source, wType, ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTRasterDataset)
                                                    'Follow-up by trying to open it as a vector
                                                    If sourceExists = False Then
                                                        sourceExists = BA_File_Exists(dSource.Source, wType, ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureClass)
                                                    End If
                                                    If sourceExists = False Then
                                                        vMethod.Status = MethodStatus.Invalid
                                                        sb.Append("Unable to locate associated data source")
                                                    End If
                                                Else
                                                    vMethod.Status = MethodStatus.Invalid
                                                    sb.Append("Unable to locate associated data source")
                                                End If
                                            Else
                                                vMethod.Status = MethodStatus.Invalid
                                                sb.Append("Unable to locate associated data source")
                                            End If
                                        End If
                                        vMethod.ErrorMessage = sb.ToString
                                        UpdateMethodStatusOnGrid(vMethod)
                                        verifyMethodTable(vMethod.Name) = vMethod
                                        m_methodStatusTable(m_selProfile.Name) = verifyMethodTable
                                    Next
                                End If
                            End If
                        End If
                    Next
                End If
                ManageVerifyButton()
                LblStatus.Text = "Verification complete"
            End If
        Catch ex As Exception
            Debug.Print("VerifyPublicProfile: " & ex.Message)
            LblStatus.Text = "Verification failed"
        Finally
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try
    End Sub

    Private Function GetSelectedHruName() As String
        If GrdHruLayers.SelectedRows.Count = 1 Then
            Dim pCollection As DataGridViewSelectedRowCollection = GrdHruLayers.SelectedRows
            'Always grab the first one
            Dim pRow As DataGridViewRow = pCollection.Item(0)
            Return CStr(pRow.Cells(0).Value)
        Else
            Return Nothing
        End If
    End Function

    Private Sub LoadCompletedProfileGrid()
        GrdCompleteProfiles.Rows.Clear()
        Dim selHruName As String = GetSelectedHruName()
        If Not String.IsNullOrEmpty(selHruName) Then
            Try
                Dim hruPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, selHruName)
                Dim hruParamPath As String = hruPath & BA_EnumDescription(PublicPath.BagisParamGdb) & BA_EnumDescription(PublicPath.HruProfileStatus)
                Dim pHruProfileStatus As HruProfileStatus = Nothing
                If BA_File_ExistsWindowsIO(hruParamPath) Then
                    Dim obj As Object = SerializableData.Load(hruParamPath, GetType(HruProfileStatus))
                    If obj IsNot Nothing Then
                        pHruProfileStatus = CType(obj, HruProfileStatus)
                        Dim profileNames As List(Of String) = pHruProfileStatus.ProfileNames
                        Dim completionDates As List(Of Date) = pHruProfileStatus.CompletionDates
                        Dim pos As Integer = 0
                        For Each pName In profileNames
                            '---create a row---
                            Dim pRow As New DataGridViewRow
                            pRow.CreateCells(GrdHruLayers)
                            pRow.Cells(0).Value = pName
                            Dim ts1 As Date = completionDates.Item(pos)
                            If ts1 <> Nothing Then
                                pRow.Cells(1).Value = ts1.ToString("g", DateTimeFormatInfo.InvariantInfo)
                            End If
                            '---add the row---
                            GrdCompleteProfiles.Rows.Add(pRow)
                            pos += 1
                        Next
                        GrdCompleteProfiles.ClearSelection()
                        GrdCompleteProfiles.Refresh()
                    End If
                End If
                'Update the count of completed profiles for the selected hru after we update the grid
                For i As Integer = 0 To GrdHruLayers.RowCount - 1
                    Dim pCell As DataGridViewCell = GrdHruLayers.Rows.Item(i).Cells.Item(0)
                    Dim hruName As String = CStr(pCell.Value)
                    If hruName = selHruName Then
                        GrdHruLayers.Rows.Item(i).Cells.Item(1).Value = GrdCompleteProfiles.RowCount
                    End If
                Next
            Catch ex As Exception
                Debug.Print("LoadCompletedProfileGrid Exception: " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub LoadSubAOIPanel()
        CboSubAoiId.Items.Clear()
        Dim subAoiGdbPath As String = TxtAoiPath.Text & BA_EnumDescription(PublicPath.BagisSubAoiGdb)
        If BA_Folder_ExistsWindowsIO(subAoiGdbPath) Then
            Dim rasterList As String() = Nothing
            Dim vectorList As String() = Nothing
            BA_ListLayersinGDB(subAoiGdbPath, rasterList, vectorList)
            'rasterList has an empty first position as a holdover from VBA
            For i As Integer = 1 To rasterList.GetUpperBound(0)
                CboSubAoiId.Items.Add(rasterList(i))
            Next
            If CboSubAoiId.Items.Count > 0 Then
                CboSubAoiId.SelectedIndex = 0
                CkAppendSubAoi.Checked = True
            End If
        End If
    End Sub

    Private Sub CboSubAoiId_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles CboSubAoiId.SelectedIndexChanged
        If CboSubAoiId.SelectedIndex > -1 Then
            Dim subAoiGdbPath As String = TxtAoiPath.Text & BA_EnumDescription(PublicPath.BagisSubAoiGdb)
            Dim valuesEnum As IEnumerator = BA_QueryUniqueValuesFromRasterGDB(subAoiGdbPath, CStr(CboSubAoiId.SelectedItem), BA_FIELD_VALUE)
            Dim i As Integer = 0
            Do Until valuesEnum.MoveNext = False
                i += 1
            Loop
            TxtSubAoiCount.Text = i
            valuesEnum = Nothing
        End If
    End Sub

    Private Function CheckForDuplicateFieldNames(ByVal validMethodsTable As Hashtable) As String
        Dim sb As StringBuilder = New StringBuilder
        'Key: fieldName, Value: List of method names
        Dim fieldNameTable As Hashtable = New Hashtable
        For Each key As String In validMethodsTable.Keys
            Dim pMethod As Method = validMethodsTable(key)
            'Open the model so we can get the parameter information
            Dim pModel As IGPTool = BA_OpenModel(pMethod.ToolBoxPath, pMethod.ToolboxName, pMethod.ModelName)
            If pModel IsNot Nothing Then
                Dim paramList As List(Of ModelParameter) = BA_GetModelParameters(pModel)
                For Each param As ModelParameter In paramList
                    Dim pName As String = param.Name
                    'If the parameter name starts with fld_ we have a field name
                    If pName.Substring(0, BA_FIELD_PREFIX.Length).ToLower = BA_FIELD_PREFIX Then
                        Dim mNameList As IList(Of String) = Nothing
                        'This is the first occurrence of this field name
                        If fieldNameTable(pName) Is Nothing Then
                            mNameList = New List(Of String)
                            mNameList.Add(pMethod.Name)
                            fieldNameTable.Add(pName, mNameList)
                            'This is not the first occurrence of this field name
                        Else
                            mNameList = fieldNameTable(pName)
                            mNameList.Add(pMethod.Name)
                            fieldNameTable(pName) = mNameList
                        End If
                    End If
                Next
            End If
        Next
        For Each fName As String In fieldNameTable.Keys
            Dim mList As IList(Of String) = fieldNameTable(fName)
            If mList.Count > 1 Then
                For Each mName As String In mList
                    sb.Append("Method: " & mName & " --> Field: " & fName & vbCrLf)
                Next
            End If
        Next
        Dim errMsg As String = Nothing
        If sb.Length > 0 Then
            errMsg = "Two or more of the methods you have selected to run share the same field name. "
            errMsg += "The last method that uses the field name will overwrite any previous calculations. "
            errMsg += "The methods and field name(s) are listed below:" & vbCrLf & vbCrLf
            errMsg += sb.ToString
        End If
        Return errMsg
    End Function

    Private Sub BtnCopyProfile_Click(sender As System.Object, e As System.EventArgs) Handles BtnExportProfile.Click
        Dim frmCopyProfile As New FrmExportProfile(m_aoi, m_selProfile)
        frmCopyProfile.ShowDialog()
    End Sub

    Private Sub GrdCompleteProfiles_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GrdCompleteProfiles.SelectionChanged
        If GrdCompleteProfiles.SelectedRows.Count > 0 Then
            BtnViewLog.Enabled = True
        Else
            BtnViewLog.Enabled = False
        End If
    End Sub

    Private Sub BtnViewLog_Click(sender As System.Object, e As System.EventArgs) Handles BtnViewLog.Click
        Try
            Dim pExt As BagisPExtension = BagisPExtension.GetExtension
            Dim selProfile As String = Nothing
            If GrdCompleteProfiles.SelectedRows.Count = 1 Then
                Dim pCollection As DataGridViewSelectedRowCollection = GrdCompleteProfiles.SelectedRows
                'Always grab the first one
                Dim pRow As DataGridViewRow = pCollection.Item(0)
                selProfile = CStr(pRow.Cells(0).Value)
            End If
            Dim selHruName As String = GetSelectedHruName()
            Dim folderPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, selHruName)
            Dim tempFile As String = selProfile & "_params_log.xml"
            Dim logProfile As LogProfile = BA_LoadLogProfileFromXml(folderPath & "\" & tempFile)
            If logProfile IsNot Nothing Then
                If logProfile.Version <> pExt.version Then
                    MessageBox.Show("This parameter log was created using an older version of BAGIS-P. It may not display correctly", "Old version", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                Dim fileNameArr As String() = tempFile.Split(".")
                Dim filePath As String = BA_StandardizePathString(folderPath, True) & fileNameArr(0) & ".html"
                BA_WriteParameterFile(logProfile, filePath, True)
                Process.Start(filePath)
            Else
                MessageBox.Show("An error occurred while trying to read the selected log file. It cannot be displayed.", "Unknown error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show("An unknown error occurred while trying to load the log file and it cannot be viewed.", "Unknown error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Debug.Print(ex.Message)
        End Try
    End Sub

    Private Function VerifyAOIAttributeTable() As BA_ReturnCode
        Dim pBandCollection As IRasterBandCollection = Nothing
        Dim pRasterBand As IRasterBand = Nothing
        Dim returnVal As BA_ReturnCode = BA_ReturnCode.UnknownError
        Try
            'Check to be sure the target is a single-band thematic raster; Cannot build an attribute table otherwise
            Dim inputFolder As String = BA_GeodatabasePath(TxtAoiPath.Text, GeodatabaseNames.Aoi)
            Dim inputFile As String = BA_GetBareName(BA_EnumDescription(PublicPath.AoiGrid))
            pBandCollection = CType(BA_OpenRasterFromGDB(inputFolder, inputFile), IRasterBandCollection)
            pRasterBand = pBandCollection.Item(0)
            Dim inputATT As Boolean = False
            pRasterBand.HasTable(inputATT)
            If inputATT = False Then
                Dim inputName = inputFolder & "\" & inputFile
                returnVal = BA_BuildRasterAttributeTable(inputName, True)
            Else
                returnVal = BA_ReturnCode.Success
            End If
            Return returnVal
        Catch ex As Exception
            Debug.Print("VerifyAOIAttributeTable Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            pBandCollection = Nothing
            pRasterBand = Nothing
        End Try
    End Function
End Class