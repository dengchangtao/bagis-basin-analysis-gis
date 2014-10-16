Imports BAGIS_ClassLibrary

Public Class FrmAddMethod

    Dim m_profile As Profile
    Dim m_methodTable As Hashtable
    Dim m_allNames As List(Of String)
    Dim m_profileNames As List(Of String)
    Dim m_aoiPath As String

    Public Sub New(ByRef selProfile As Profile, ByVal pMethodTable As Hashtable, _
                   ByVal aoiPath As String)

        ' This call is required by the designer.
        InitializeComponent()

        m_profile = selProfile
        TxtProfileName.Text = m_profile.Name
        TxtProfileDescription.Text = m_profile.Description

        m_allNames = New List(Of String)
        m_profileNames = New List(Of String)
        m_aoiPath = aoiPath

        If pMethodTable IsNot Nothing Then
            m_methodTable = New Hashtable
            For Each pKey In pMethodTable.Keys
                m_methodTable.Add(pKey, pMethodTable(pKey))
                Dim foundIt As Boolean = False
                If selProfile.MethodNames Is Nothing Then
                    foundIt = False
                Else
                    'Only add a method to the list if it isn't already attached
                    For Each methodName In selProfile.MethodNames
                        If pKey = methodName Then
                            foundIt = True
                            Exit For
                        End If
                    Next
                End If
                If foundIt = False Then
                    m_allNames.Add(pKey)
                End If
            Next
        End If
        If selProfile.MethodNames IsNot Nothing Then
            For Each pName In selProfile.MethodNames
                If m_methodTable(pName) IsNot Nothing Then
                    ' Only add the name if the method xml file exists
                    m_profileNames.Add(pName)
                End If
            Next
        End If
        LoadLists()
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub LstNewMethods_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstNewMethods.SelectedIndexChanged
        If LstNewMethods.SelectedItem IsNot Nothing Then
            Dim pItem As String = CStr(LstNewMethods.SelectedItem)
            Dim pMethod As Method = m_methodTable(pItem)
            If pMethod IsNot Nothing Then
                TxtMethodDescription.Text = pMethod.Description
            End If
            BtnApply.Enabled = True
        Else
            BtnApply.Enabled = False
        End If
    End Sub

    Private Sub LstProfileMethods_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles LstProfileMethods.SelectedIndexChanged
        If LstProfileMethods.SelectedItem IsNot Nothing Then
            Dim pItem As String = CStr(LstProfileMethods.SelectedItem)
            Dim pMethod As Method = m_methodTable(pItem)
            If pMethod IsNot Nothing Then
                TxtMethodDescription.Text = pMethod.Description
            End If
            BtnApply.Enabled = True
        Else
            BtnApply.Enabled = False
        End If
    End Sub

    Private Sub LoadLists()
        LoadNewMethodsList()
        LstProfileMethods.Items.Clear()
        If m_profileNames IsNot Nothing Then
            m_profileNames.Sort()
            For Each pName In m_profileNames
                LstProfileMethods.Items.Add(pName)
            Next
        End If
        If LstProfileMethods.Items.Count > 0 Then
            BtnRemoveMethod.Enabled = True
        Else
            BtnRemoveMethod.Enabled = False
        End If
    End Sub

    Private Sub LoadNewMethodsList()
        Dim strSearch = TxtSearchFilter.Text.ToUpper
        LstNewMethods.Items.Clear()
        If m_allNames IsNot Nothing Then
            m_allNames.Sort()
            For Each pName In m_allNames
                If Not String.IsNullOrEmpty(strSearch) Then
                    Dim uName As String = pName.ToUpper
                    Dim SearchString As String = "XXpXXpXXPXXP"
                    Dim searchChar As String = "P"
                    Dim pos1 As Integer = InStr(4, SearchString, SearchChar, CompareMethod.Text)
                    Dim myPos As Integer = InStr(1, uName, strSearch, CompareMethod.Text)
                    If myPos > 0 Then
                        LstNewMethods.Items.Add(pName)
                    End If
                Else
                    LstNewMethods.Items.Add(pName)
                End If
            Next
        End If
        If LstNewMethods.Items.Count > 0 Then
            BtnAddMethod.Enabled = True
        Else
            BtnAddMethod.Enabled = False
        End If
    End Sub

    Private Sub BtnRemoveMethod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnRemoveMethod.Click
        If LstProfileMethods.SelectedItem IsNot Nothing Then
            Dim mName As String = CStr(LstProfileMethods.SelectedItem)
            m_profileNames.Remove(mName)
            m_allNames.Add(mName)
            LoadLists()
        End If
    End Sub

    Private Sub BtnAddMethod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAddMethod.Click
        If LstNewMethods.SelectedItem IsNot Nothing Then
            Dim mName As String = CStr(LstNewMethods.SelectedItem)
            m_allNames.Remove(mName)
            m_profileNames.Add(mName)
            LoadLists()
        End If
    End Sub

    Private Sub BtnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        TxtSearchFilter.ReadOnly = True
        Dim pathToSave As String = Nothing
        If String.IsNullOrEmpty(m_aoiPath) Then
            Dim bExt As BagisPExtension = BagisPExtension.GetExtension
            Dim settingsPath As String = bExt.SettingsPath
            pathToSave = BA_GetPublicProfilesPath(settingsPath)
        Else
            pathToSave = BA_GetLocalProfilesDir(m_aoiPath)
        End If
        m_profile.MethodNames = m_profileNames
        m_profile.Save(BA_BagisPXmlPath(pathToSave, m_profile.Name))
        Me.Close()
    End Sub


    Private Sub TxtSearchFilter_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TxtSearchFilter.TextChanged
        LoadNewMethodsList()
    End Sub
End Class