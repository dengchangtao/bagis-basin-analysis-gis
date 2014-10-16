Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports System.Windows.Forms
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geoprocessing

Public Class FrmResetToolboxPath

    Dim m_methodTable As Hashtable
    Dim m_methodFolder As String


    Public Sub New(ByVal pMethodTable As Hashtable, ByVal methodFolder As String)

        InitializeComponent()

        Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        Dim settingsPath As String = bExt.SettingsPath
        Dim longPathName As String = Nothing
        Dim methodsFolder As String = BA_GetPublicMethodsPath(settingsPath)
        'Note: ArcCatalog cannot handle using the old DOS path names as the starting folder
        If Not String.IsNullOrEmpty(methodsFolder) Then
            longPathName = BA_GetLongName(methodsFolder)
        End If
        TxtToolboxPath.Text = longPathName

        If pMethodTable IsNot Nothing Then
            m_methodTable = New Hashtable
            For Each pKey In pMethodTable.Keys
                m_methodTable.Add(pKey, pMethodTable(pKey))
            Next
        End If
        m_methodFolder = methodFolder

    End Sub

    Private Sub BtnCancel_Click(sender As System.Object, e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub BtnSelectPath_Click(sender As System.Object, e As System.EventArgs) Handles BtnSelectPath.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim DataPath As String
        Dim pFilter As IGxObjectFilter = New GxFilterContainers
        Dim pGxCatalog As IGxCatalog = New GxCatalog

        Try
            pGxCatalog.Location = TxtToolboxPath.Text

            'initialize and open mini browser
            With pGxDialog
                .AllowMultiSelect = False
                .ButtonCaption = "Select"
                .Title = "Select toolbox path"
                .ObjectFilter = pFilter
                .StartingLocation = TxtToolboxPath.Text
                bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
            End With

            If bObjectSelected = False Then Exit Sub

            'get the name of the selected folder
            Dim pGxDataFolder As IGxFile
            pGxDataFolder = pGxObject.Next
            DataPath = pGxDataFolder.Path
            If String.IsNullOrEmpty(DataPath) Then Exit Sub 'user cancelled the action

            Dim pCount As Integer = GetToolboxCount(DataPath)
            If pCount = 0 Then
                MessageBox.Show("The folder you selected does not contain any toolboxes and cannot be used.", "No toolboxes", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

            TxtToolboxPath.Text = DataPath

        Catch ex As Exception
            MessageBox.Show("BtnSelectPath_Click Exception: " & ex.Message)
        End Try

    End Sub

    'Returns the number of toolboxes in a folder
    Private Function GetToolboxCount(ByVal folderPath As String) As Integer
        Dim wsf As IWorkspaceFactory = New ToolboxWorkspaceFactory
        Dim ws As IToolboxWorkspace = Nothing
        Dim pEnum As IEnumGPToolboxName = Nothing
        Dim pCount As Integer

        Try
            ws = wsf.OpenFromFile(folderPath, 0)
            If ws IsNot Nothing Then
                pEnum = ws.ToolboxNames
                Dim pName As IGPToolboxName = pEnum.Next
                Do While pName IsNot Nothing
                    pCount += 1
                    pName = pEnum.Next
                Loop
            End If
            Return pCount
        Catch ex As Exception
            Debug.Print("GetToolboxCount Exception: " & ex.Message)
            Return pCount
        Finally
            wsf = Nothing
            ws = Nothing
            pEnum = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try

    End Function

    Private Sub BtnApply_Click(sender As System.Object, e As System.EventArgs) Handles BtnApply.Click
        Dim pCount As Integer = GetToolboxCount(TxtToolboxPath.Text)
        Dim methodList As IList(Of String) = New List(Of String)
        If pCount = 0 Then
            MessageBox.Show("The folder you selected does not contain any toolboxes and cannot be used.", "No toolboxes", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        BtnApply.Enabled = False
        For Each pKey As String In m_methodTable.Keys
            Dim pMethod As Method = m_methodTable(pKey)
            pMethod.ToolBoxPath = TxtToolboxPath.Text
            pMethod.Save(BA_BagisPXmlPath(m_methodFolder, pMethod.Name))
        Next
        Me.Close()

    End Sub
End Class