Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports BAGIS_ClassLibrary
Imports System.Windows.Forms

Public Class FrmSelectAoi

    Dim m_aoiPathList As List(Of String)

    Public Sub New(ByVal formText As String)
        InitializeComponent()
        LblText.Text = formText
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Public ReadOnly Property AoiPathList As List(Of String)
        Get
            Return m_aoiPathList
        End Get
    End Property


    Private Sub BtnAddAoi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAddAoi.Click
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
                If AlreadyOnList(DataPath) = False Then
                    Dim pAoi As Aoi = New Aoi(aoiName, DataPath, Nothing, bagisPExt.version)
                    Dim pItem As LayerListItem = New LayerListItem(pAoi.FilePath, pAoi.FilePath, LayerType.Raster, True)
                    LstAoi.Items.Add(pItem)
                    LstAoi.Sorted = True
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("BtnSelectAoi_Click Exception: " & ex.Message)
        End Try
    End Sub

    'Check to see if the aoiPath is already in the list
    Private Function AlreadyOnList(ByVal aoiPath As String) As Boolean
        If LstAoi.Items.Count = 0 Then
            Return False
        Else
            For Each pItem As LayerListItem In LstAoi.Items
                If pItem.Value = aoiPath Then
                    Return True
                End If
            Next
            Return False
        End If
    End Function

    Private Sub LstAoi_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LstAoi.SelectedIndexChanged
        If LstAoi.SelectedIndex > -1 Then
            Dim pItem As LayerListItem = CType(LstAoi.SelectedItem, LayerListItem)
            BtnRemove.Enabled = True
        Else
            BtnRemove.Enabled = False
        End If
    End Sub

    Private Sub BtnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnRemove.Click
        Do While LstAoi.SelectedIndex > -1
            Dim idxDelete As Int16 = LstAoi.SelectedIndex
            LstAoi.Items.RemoveAt(idxDelete)
        Loop
    End Sub

    Private Sub BtnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        m_aoiPathList = New List(Of String)
        If LstAoi.Items.Count > 0 Then
            For Each pItem As LayerListItem In LstAoi.Items
                m_aoiPathList.Add(pItem.Value)
            Next
        End If
        Me.Close()
    End Sub
End Class