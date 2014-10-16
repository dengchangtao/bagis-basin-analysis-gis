Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports System.Windows.Forms


Public Class BtnViewLog
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()

    End Sub

    Protected Overrides Sub OnClick()
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim DataPath As String
        Dim pFilter As IGxObjectFilter = New GxFilterXml

        Try
            'initialize and open mini browser
            With pGxDialog
                .AllowMultiSelect = False
                .ButtonCaption = "Select"
                .Title = "Select an HRU log file to view"
                .ObjectFilter = pFilter
                bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
            End With

            If bObjectSelected = False Then Exit Sub

            'get the name of the selected folder
            Dim pGxDataFolder As IGxFile
            pGxDataFolder = pGxObject.Next
            DataPath = pGxDataFolder.Path
            If String.IsNullOrEmpty(DataPath) Then Exit Sub 'user cancelled the action

            Dim folderPath As String = "Return"
            Dim tempFile As String = BA_GetBareName(DataPath, folderPath)
            Dim pExt As BagisPExtension = BagisPExtension.GetExtension
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

    Protected Overrides Sub OnUpdate()

    End Sub
End Class
