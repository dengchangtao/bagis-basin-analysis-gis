Imports BAGIS_ClassLibrary
Imports System.Windows.Forms

'MAY 22 2012 - THIS FORM IS NO LONGER USED LCB
Public Class FrmGroupProfile

    Dim m_profileList As List(Of Profile)
    Dim m_selProfile As Profile
    Dim m_selMethod As Method
    Dim m_methodTable As Hashtable

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'Dim profile1 As Profile = New Profile(1, "All_Methods", ProfileClass.BA_Public, "Contains all available methods")
        'Dim profile2 As Profile = New Profile(2, "Profile1", ProfileClass.BA_Public, "")
        'Dim profile3 As Profile = New Profile(3, "Profile2", ProfileClass.BA_Public, "NWCC standard profile for PRMS/OMS")
        'Dim profile4 As Profile = New Profile(4, "Profile3", ProfileClass.BA_Public, "")

        'Dim method1 As Method = New Method(1, "Aspect", "", "", "", "", "")
        'Dim method2 As Method = New Method(2, "Area", "", "", "", "", "")
        'Dim method3 As Method = New Method(3, "Elevation", "", "", "", "", "")
        'Dim method4 As Method = New Method(4, "VegCov_Density", "Cov_Den", "", "VPC", "VegCov_Density", "C:\Documents and Settings\Lesley\Application Data\ESRI\Desktop10.0\ArcToolbox\My Toolboxes\VPC.tbx")
        'Dim method5 As Method = New Method(5, "Profile4", "", "", "", "", "")
        'Dim method6 As Method = New Method(6, "Root-depth", "", "", "", "", "")
        'Dim method7 As Method = New Method(7, "Soilmoist", "", "", "", "", "")
        'Dim methods As List(Of Method) = New List(Of Method)
        'methods.Add(method1)
        'methods.Add(method2)
        'methods.Add(method3)
        'methods.Add(method4)
        'methods.Add(method5)
        'methods.Add(method6)
        'methods.Add(method7)
        'profile3.Methods = methods

        'Dim profiles As List(Of Profile) = New List(Of Profile)
        'profiles.Add(profile1)
        'profiles.Add(profile2)
        'profiles.Add(profile3)
        'profiles.Add(profile4)

        'LoadProfilesList(profiles)

        Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        Dim settingsPath As String = bExt.SettingsPath
        Dim publicProfilesPath As String = BA_GetPublicProfilesPath(settingsPath)
        Try
            If publicProfilesPath IsNot Nothing Then
                Dim profileList As List(Of Profile) = BA_LoadProfilesFromXml(publicProfilesPath)
                If profileList IsNot Nothing Then
                    m_profileList = New List(Of Profile)
                    m_profileList.AddRange(profileList)
                    For Each nextProfile In m_profileList
                        LstProfiles.Items.Add(nextProfile.Name)
                    Next
                End If
            End If

            Dim allMethods As List(Of Method) = BA_LoadMethodsFromXml(BA_GetPublicMethodsPath(settingsPath))
            m_methodTable = New Hashtable
            For Each pMethod In allMethods
                m_methodTable.Add(pMethod.Name, pMethod)
            Next
        Catch ex As Exception
            Debug.Print("BtnApply_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub LstProfiles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstProfiles.SelectedIndexChanged
        ClearProfileFields()
        m_selProfile = Nothing
        If LstProfiles.SelectedItem IsNot Nothing Then
            Dim pName As String = CStr(LstProfiles.SelectedItem)
            For Each profile In m_profileList
                If profile.Name = pName Then
                    m_selProfile = profile
                    Exit For
                End If
            Next
            If m_selProfile IsNot Nothing Then
                LoadMethodNamesList()
                TxtProfileName.Text = m_selProfile.Name
                TxtNumMethods.Text = LstMethods.Items.Count
                TxtDescription.Text = m_selProfile.Description
            End If
            BtnEditProfile.Enabled = True
            BtnProfileDelete.Enabled = True
            BtnAddMethod.Enabled = True
        Else
            BtnEditProfile.Enabled = False
            BtnProfileDelete.Enabled = False
            BtnAddMethod.Enabled = False
        End If

    End Sub

    Private Sub LoadMethodNamesList()
        Dim mList As List(Of String) = m_selProfile.MethodNames
        If mList IsNot Nothing AndAlso mList.Count > 0 Then
            For Each pName In mList
                'Make sure method exists in the hashtable before adding in case it has been deleted
                If m_methodTable(pName) IsNot Nothing Then
                    LstMethods.Items.Add(pName)
                End If
            Next
        End If
    End Sub

    Private Sub LstMethods_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstMethods.SelectedIndexChanged
        ClearMethodFields()
        m_selMethod = Nothing
        If LstMethods.SelectedItem IsNot Nothing Then
            Dim methodName As String = CStr(LstMethods.SelectedItem)
            If m_methodTable IsNot Nothing Then
                m_selMethod = m_methodTable(methodName)
            End If
            If m_selMethod IsNot Nothing Then
                TxtSelMethod.Text = m_selMethod.Name
                'This is now a parameter from the model
                'TxtParamName.Text = m_selMethod.ParamName
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

    Private Sub ClearProfileFields()
        LstMethods.Items.Clear()
        TxtProfileName.Text = Nothing
        TxtProfileName.ReadOnly = True
        TxtNumMethods.Text = Nothing
        TxtDescription.Text = Nothing
        TxtDescription.ReadOnly = True
        ClearMethodFields()
    End Sub

    Private Sub ClearMethodFields()
        TxtSelMethod.Text = Nothing
        TxtParamName.Text = Nothing
        TxtToolboxName.Text = Nothing
        TxtModelName.Text = Nothing
        BtnEditMethod.Enabled = False
    End Sub

    Private Sub BtnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        'Validate that we at least have a name
        If String.IsNullOrEmpty(TxtProfileName.Text) Then
            MessageBox.Show("You must supply a profile name before creating or updating a profile.")
            Exit Sub
        End If
        Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        Dim settingsPath As String = bExt.SettingsPath
        Dim profilesPath As String = BA_GetPublicProfilesPath(settingsPath)
        Try
            If profilesPath IsNot Nothing Then
                'We have a folder to save the profile
                'Trim leading/trailing spaces from profile name
                Dim pName As String = Trim(TxtProfileName.Text)
                'Replace remaining spaces with "_" in name
                pName = pName.Replace(" ", "_")
                If m_selProfile IsNot Nothing Then
                    'We are updating an existing profile
                    'Delete old xml record
                    BA_Remove_File(BA_BagisPXmlPath(profilesPath, m_selProfile.Name))
                    m_profileList.Remove(m_selProfile)
                    m_selProfile.Name = pName
                    m_selProfile.Description = Trim(TxtDescription.Text)
                    m_selProfile.Save(BA_BagisPXmlPath(profilesPath, m_selProfile.Name))
                    m_profileList.Add(m_selProfile)
                Else
                    'We are creating a new profile
                    Dim id As Integer = GetNextProfileId()
                    'Because this is the public profile screen, the class is always public
                    Dim newProfile As Profile = New Profile(id, pName, ProfileClass.BA_Public, bExt.version, Trim(TxtDescription.Text))
                    m_selProfile = newProfile
                    newProfile.Save(BA_BagisPXmlPath(profilesPath, m_selProfile.Name))
                    m_profileList.Add(newProfile)
                End If
                ClearMethodFields()
                ClearProfileFields()
                LstProfiles.Items.Clear()
                m_profileList.Sort()
                For Each nextProfile In m_profileList
                    LstProfiles.Items.Add(nextProfile.Name)
                Next
            End If
        Catch ex As Exception
            Debug.Print("BtnApply_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub BtnEditProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnEditProfile.Click
        TxtProfileName.ReadOnly = False
        TxtDescription.ReadOnly = False
    End Sub

    Private Sub BtnProfileNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnProfileNew.Click
        ClearProfileFields()
        m_selProfile = Nothing
        TxtProfileName.ReadOnly = False
        TxtDescription.ReadOnly = False
        BtnProfileDelete.Enabled = False
    End Sub

    'Find the highest current id and then add 1 to get the next valid id
    Private Function GetNextProfileId() As Integer
        Dim id As Integer = 0
        For Each nextProfile In m_profileList
            If nextProfile.Id > id Then
                id = nextProfile.Id
            End If
        Next
        Return id + 1
    End Function

    Private Sub BtnProfileDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnProfileDelete.Click
        Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        Dim settingsPath As String = bExt.SettingsPath
        Dim profilesPath As String = BA_GetPublicProfilesPath(settingsPath)
        m_profileList.Remove(m_selProfile)
        'Delete old xml record
        BA_Remove_File(BA_BagisPXmlPath(profilesPath, m_selProfile.Name))
        m_selProfile = Nothing
        ClearMethodFields()
        ClearProfileFields()
        LstProfiles.Items.Clear()
        For Each nextProfile In m_profileList
            LstProfiles.Items.Add(nextProfile.Name)
        Next
    End Sub

    'Private Sub BtnEditMethod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnEditMethod.Click
    '    Dim frmMethod As New FrmEditMethod(Me, m_selProfile, m_selMethod, Nothing)
    '    frmMethod.ShowDialog()
    'End Sub

    Private Sub BtnMethodNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnMethodNew.Click
        'Dim frmMethod As New FrmEditMethod(m_methodTable, Nothing)
        'frmMethod.ShowDialog()
        'If frmMethod.DirtyFlag = True Then
        '    'reload method table
        '    Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        '    Dim settingsPath As String = bExt.SettingsPath
        '    Dim allMethods As List(Of Method) = BA_LoadMethodsFromXml(BA_GetPublicMethodsPath(settingsPath))
        '    m_methodTable = New Hashtable
        '    For Each pMethod In allMethods
        '        m_methodTable.Add(pMethod.Name, pMethod)
        '    Next
        'End If
    End Sub

    Private Sub BtnAddMethod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAddMethod.Click
        Dim frmAddMethod As New FrmAddMethod(m_selProfile, m_methodTable, Nothing)
        frmAddMethod.ShowDialog()
        ClearMethodFields()
        LstMethods.Items.Clear()
        LoadMethodNamesList()
    End Sub

    Private Sub BtnMethodDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnMethodDelete.Click
        If LstMethods.SelectedItem IsNot Nothing Then
            Dim methodName As String = CStr(LstMethods.SelectedItem)
            Dim profileMethodNames As List(Of String) = m_selProfile.MethodNames
            For Each pName As String In profileMethodNames
                If pName = methodName Then
                    profileMethodNames.Remove(pName)
                    Exit For
                End If
            Next
            ClearMethodFields()
            LstMethods.Items.Clear()
            Dim bExt As BagisPExtension = BagisPExtension.GetExtension
            Dim settingsPath As String = bExt.SettingsPath
            Dim profilesPath As String = BA_GetPublicProfilesPath(settingsPath)
            m_selProfile.Save(BA_BagisPXmlPath(profilesPath, m_selProfile.Name))
            LoadMethodNamesList()
        End If
    End Sub

End Class