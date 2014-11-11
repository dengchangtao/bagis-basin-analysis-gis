Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports BAGIS_ClassLibrary
Imports System.Windows.Forms
Imports System.Text
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework

Public Class FrmSelectProfile

    Dim m_profile As Profile
    Dim m_profileTable As Hashtable
    Dim m_localProfileTable As Hashtable
    Dim m_localMethodTable As Hashtable = New Hashtable
    Dim m_publicDataTable As Hashtable
    Dim m_aoiPath As String
    Dim m_DirtyFlag As Boolean = False

    Public Sub New(ByVal localProfileTable As Hashtable, ByVal aoiPath As String)
        InitializeComponent()
        Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        Dim settingsPath As String = bExt.SettingsPath
        Dim profilesFolder As String = BA_GetPublicProfilesPath(settingsPath)
        BA_ValidateProfileNames(profilesFolder)
        Dim profileList As List(Of Profile) = BA_LoadProfilesFromXml(profilesFolder)
        If profileList IsNot Nothing Then
            m_profileTable = New Hashtable
            For Each nextProfile In profileList
                m_profileTable.Add(nextProfile.Name, nextProfile)
                LstProfiles.Items.Add(nextProfile.Name)
            Next
        End If
        m_localProfileTable = localProfileTable
        m_aoiPath = aoiPath
        'Put all of the methods in this aoi into a hashtable so we can validate against them later
        Dim methodList As List(Of Method) = BA_LoadMethodsFromXml(BA_GetLocalMethodsDir(aoiPath))
        If methodList IsNot Nothing Then
            For Each pMethod As Method In methodList
                m_localMethodTable.Add(pMethod.Name, pMethod)
            Next
        End If
    End Sub

    Protected Friend ReadOnly Property DirtyFlag() As Boolean
        Get
            Return m_DirtyFlag
        End Get
    End Property

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Public ReadOnly Property SelectedProfile As Profile
        Get
            Return m_profile
        End Get
    End Property

    Private Sub LstAoi_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstProfiles.SelectedIndexChanged
        If LstProfiles.SelectedIndex > -1 Then
            Dim pItem As String = CType(LstProfiles.SelectedItem, String)
            BtnApply.Enabled = True
            Dim selProfile As Profile = m_profileTable(pItem)
            If selProfile IsNot Nothing Then
                TxtDescription.Text = selProfile.Description
                TxtNumMethods.Text = CStr(selProfile.MethodNames.Count)
            Else
                TxtDescription.Text = Nothing
                TxtNumMethods.Text = Nothing
            End If
        Else
            BtnApply.Enabled = False
            TxtDescription.Text = Nothing
            TxtNumMethods.Text = Nothing
        End If
    End Sub

    Private Sub BtnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Do While LstProfiles.SelectedIndex > -1
            Dim idxDelete As Int16 = LstProfiles.SelectedIndex
            LstProfiles.Items.RemoveAt(idxDelete)
        Loop
    End Sub

    Private Sub BtnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        If LstProfiles.SelectedIndex > -1 Then
            Dim profileName As String = CType(LstProfiles.SelectedItem, String)
            'Get selected group profile
            m_profile = m_profileTable(profileName)
            If m_profile IsNot Nothing Then
                ' We have a duplicate local profile and need to ask for a new name
                Dim lclProfileName As String = profileName
                Do While m_localProfileTable(lclProfileName) IsNot Nothing
                    Dim sb As StringBuilder = New StringBuilder
                    sb.Append("A profile already exists with the name" & vbCrLf)
                    sb.Append("'" & profileName & "' in this AOI. Please supply" & vbCrLf)
                    sb.Append("a unique profile name.")
                    lclProfileName = InputBox(sb.ToString, "Profile Name", "")
                    'User clicked cancel; Exit sub
                    If String.IsNullOrEmpty(lclProfileName) Then Exit Sub
                Loop
                'Create the new local profile
                Dim profileId As Int16 = GetNextProfileId()
                Dim bExt As BagisPExtension = BagisPExtension.GetExtension
                Dim lclProfile As Profile = New Profile(profileId, lclProfileName, ProfileClass.BA_Local, m_profile.Description, bExt.version)
                Dim lclMethodList As List(Of String) = New List(Of String)
                'Loop through the methods of the public profile
                Dim settingsPath As String = bExt.SettingsPath
                Dim publicMethodsPath As String = BA_GetPublicMethodsPath(settingsPath)
                Dim localMethodsPath As String = BA_GetLocalMethodsDir(m_aoiPath)
                'Create a list of existing methods so we can warn the user
                Dim existingMethods As IList(Of Method) = New List(Of Method)
                Dim keepExisting As Boolean = True

                ' Create/configure a step progressor
                Dim pStepProg As IStepProgressor = Nothing
                Dim progressDialog2 As IProgressDialog2 = Nothing
                Try
                    Dim stepCount As Integer = 5
                    If m_profile.MethodNames.Count > 0 Then
                        stepCount = stepCount + m_profile.MethodNames.Count
                    End If
                    pStepProg = BA_GetStepProgressor(My.ArcMap.Application.hWnd, stepCount)
                    progressDialog2 = BA_GetProgressDialog(pStepProg, "Importing profile ", "")
                    pStepProg.Show()
                    progressDialog2.ShowDialog()
                    For Each mName As String In m_profile.MethodNames
                        If m_localMethodTable(mName) IsNot Nothing Then
                            'Add the existing method to a list; Process later
                            existingMethods.Add(m_localMethodTable(mName))
                        End If
                    Next
                    If existingMethods.Count > 0 Then
                        'method by this name already exists
                        'check to see if the user still wants to copy the method
                        Dim sb As StringBuilder = New StringBuilder()
                        sb.Append("The following method(s) already exist in this AOI:" & vbCrLf & vbCrLf)
                        For Each nextMethod As Method In existingMethods
                            sb.Append(nextMethod.Name & vbCrLf)
                        Next
                        sb.Append(vbCrLf)
                        sb.Append("Click Yes to overwrite or No to keep the existing versions." & vbCrLf)
                        Dim result As DialogResult = MessageBox.Show(sb.ToString, "Method(s) Exist", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        If result = DialogResult.Yes Then
                            keepExisting = False
                        End If
                    End If

                    For Each mName As String In m_profile.MethodNames
                        pStepProg.Message = "Importing " & mName
                        pStepProg.Step()
                        'We have an existing local method by this name and e want to keep this method
                        If m_localMethodTable(mName) IsNot Nothing And keepExisting = True Then
                            'Still need to make sure the supporting data layer(s) are there
                            Dim srcMethod As Method = BA_LoadMethodFromXml(publicMethodsPath, mName)
                            If srcMethod IsNot Nothing AndAlso srcMethod.ModelParameters IsNot Nothing Then
                                For Each srcParam As ModelParameter In srcMethod.ModelParameters
                                    'This method clips the data source to the aoi if it doesn't already exist
                                    Dim success As BA_ReturnCode = ClipDataSource(m_aoiPath, srcParam.Name, srcParam.Value)
                                Next
                                'Still need to attach the name to the profile even if we use existing version
                                lclMethodList.Add(mName)

                            End If
                        Else
                            'Add all new methods
                            'Load the source public method from the xml
                            Dim srcMethod As Method = BA_LoadMethodFromXml(publicMethodsPath, mName)
                            If srcMethod IsNot Nothing Then
                                'Generate a new methodId for the local method
                                Dim methodId As Int16 = GetNextMethodId()
                                'Create the local method
                                Dim lclMethod As Method = New Method(methodId, mName, _
                                                                     srcMethod.Description, srcMethod.ToolboxName, _
                                                                     srcMethod.ModelName, srcMethod.ModelLabel, _
                                                                     srcMethod.ToolBoxPath)
                                'copy the parameters
                                If srcMethod.ModelParameters IsNot Nothing Then
                                    Dim lclParamList As List(Of ModelParameter) = New List(Of ModelParameter)
                                    For Each srcParam As ModelParameter In srcMethod.ModelParameters
                                        Dim lclParam As ModelParameter = New ModelParameter(srcParam.Id, _
                                                                                            srcParam.Name, srcParam.Value)
                                        lclParamList.Add(lclParam)
                                        'This method clips the data source to the aoi if it doesn't already exist
                                        'Checks first to see if it is a db_ parameter
                                        Dim success As BA_ReturnCode = ClipDataSource(m_aoiPath, srcParam.Name, srcParam.Value)
                                    Next
                                    lclMethod.ModelParameters = lclParamList
                                End If
                                'persist the method
                                lclMethod.Save(BA_BagisPXmlPath(localMethodsPath, lclMethod.Name))
                                lclMethodList.Add(mName)
                            End If
                        End If
                    Next

                    lclProfile.MethodNames = lclMethodList
                    'persist profile
                    lclProfile.Save(BA_BagisPXmlPath(BA_GetLocalProfilesDir(m_aoiPath), lclProfileName))
                    m_DirtyFlag = True
                Catch ex As Exception
                    Debug.Print("BtnApply_Click Exception: " & ex.Message)
                Finally
                    'The step progressor will be undefined if the cancel button was clicked
                    If pStepProg IsNot Nothing Then
                        pStepProg.Hide()
                        pStepProg = Nothing
                        progressDialog2.HideDialog()
                        progressDialog2 = Nothing
                    End If
                    GC.Collect()
                    GC.WaitForPendingFinalizers()
                End Try
            End If
        End If
        Me.Close()
    End Sub

    'Find the highest current id and then add 1 to get the next valid id
    Private Function GetNextProfileId() As Integer
        Dim id As Integer = 0
        For Each key As String In m_localProfileTable.Keys
            Dim nextProfile As Profile = m_localProfileTable(key)
            If nextProfile.Id >= id Then
                id = nextProfile.Id
            End If
        Next
        Return id + 1
    End Function

    'Find the highest current id and then add 1 to get the next valid id
    Private Function GetNextMethodId() As Integer
        Dim id As Integer = 0
        For Each key As String In m_localMethodTable.Keys
            Dim nextMethod As Method = m_localMethodTable(key)
            If nextMethod.Id >= id Then
                id = nextMethod.Id
            End If
        Next
        Return id + 1
    End Function

    ' paramName: db_landfire_evt
    ' paramValue: Veg_Cover
    Private Function ClipDataSource(ByVal aoiPath As String, ByVal paramName As String, ByVal paramValue As String) As BA_ReturnCode
        'Only run function if paramName has "db_" prefix
        If paramName.Substring(0, BA_DATABIN_PREFIX.Length).ToLower = BA_DATABIN_PREFIX Then
            ' Load the public data source definition if it hasn't already been loaded for
            'a previous parameter
            If m_publicDataTable Is Nothing Then
                Dim settingsPath As String = BA_GetBagisPSettingsPath()
                m_publicDataTable = BA_LoadSettingsFile(settingsPath)
                If m_publicDataTable IsNot Nothing Then
                    Dim pubDataSource As DataSource = m_publicDataTable(paramValue)
                    If pubDataSource IsNot Nothing Then
                        Dim lclDataBinPath As String = BA_GetDataBinPath(aoiPath)
                        Dim lclFileName As String = BA_GetBareName(pubDataSource.Source)
                        'If the file doesn't exist, clip it to the aoi
                        If Not BA_File_Exists(lclDataBinPath & "\" & lclFileName, WorkspaceType.Geodatabase, _
                                              ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTRasterDataset) Then
                            SetDatumInExtension(aoiPath)
                            Return BA_ClipLayerToAoi(aoiPath, lclDataBinPath, pubDataSource)
                        Else
                            'If it does, return
                            Return BA_ReturnCode.Success
                        End If
                    End If
                End If
            End If
            Return BA_ReturnCode.OtherError
        Else
            Return BA_ReturnCode.Success
        End If

    End Function

    ' Sets the datum string from the source DEM in the bagis-p extension
    Private Sub SetDatumInExtension(ByVal aoiPath As String)
        Dim pExt As BagisPExtension = BagisPExtension.GetExtension
        Dim workspaceType As WorkspaceType = workspaceType.Geodatabase
        Dim parentPath As String = aoiPath & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces)
        Dim pGeoDataSet As IGeoDataset = BA_OpenRasterFromGDB(parentPath, BA_EnumDescription(MapsFileName.filled_dem_gdb))
        If pGeoDataSet IsNot Nothing Then
            'Spatial reference for the dataset in question
            Dim pSpRef As ESRI.ArcGIS.Geometry.ISpatialReference = pGeoDataSet.SpatialReference
            If pSpRef IsNot Nothing Then
                pExt.Datum = BA_DatumString(pSpRef)
                pExt.SpatialReference = pSpRef.Name
            End If
            pSpRef = Nothing
        End If
        pGeoDataSet = Nothing
        GC.WaitForPendingFinalizers()
        GC.Collect()
    End Sub
End Class