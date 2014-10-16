Imports System.Text
Imports System.Runtime.InteropServices

Public Module PathsModule

    Public Function BA_GetPublicProfilesPath(ByVal settingsPath As String) As String
        Dim publicProfilesPath As String = Nothing
        If settingsPath IsNot Nothing Then
            publicProfilesPath = settingsPath & BA_EnumDescription(PublicPath.BagisPProfiles)
            If Not BA_Folder_ExistsWindowsIO(publicProfilesPath) Then
                'Split publicProfilesPath into its' components so we can check to make sure all folders exist
                Dim folders As String() = publicProfilesPath.Split("\")
                'If this doesn't work, try a slash the other direction
                If folders.GetUpperBound(0) < 1 Then
                    folders = publicProfilesPath.Split("/")
                End If
                Dim parentPath As String = ""
                'If we have more folders than the root
                If folders.GetUpperBound(0) > 1 Then
                    parentPath = folders(0)
                    For i As Integer = 1 To folders.GetUpperBound(0)
                        'Set the file name to the next item in the array
                        Dim fileName As String = folders(i)
                        'Check to see if parentPath/fileName combination exists
                        If Not BA_Folder_ExistsWindowsIO(parentPath & "\" & fileName) Then
                            'If not, create the folder and update parentPath
                            parentPath = BA_CreateFolder(parentPath, fileName)
                        Else
                            'If so just update parentPath
                            parentPath = parentPath & "\" & fileName
                        End If
                    Next
                End If
                'Return the final output as the folder name
                Return parentPath
            Else
                Return publicProfilesPath
            End If
        End If
        Return Nothing
    End Function

     Public Function BA_GetPublicMethodsPath(ByVal settingsPath As String) As String
        'Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        'Dim settingsPath As String = bExt.SettingsPath
        Dim publicMethodsPath As String = Nothing
        If settingsPath IsNot Nothing Then
            'We assume that settingsPath exists because we created it when getting the profiles path
            publicMethodsPath = settingsPath & BA_EnumDescription(PublicPath.BagisPMethods)
            If Not BA_Folder_ExistsWindowsIO(publicMethodsPath) Then
                Dim parentFolder As String = ""
                Dim fileName As String = BA_GetBareName(publicMethodsPath, parentFolder)
                parentFolder = BA_StandardizePathString(parentFolder)
                Dim newPath As String = BA_CreateFolder(parentFolder, fileName)
                If newPath IsNot Nothing Then
                    Return newPath
                End If
            Else
                Return publicMethodsPath
            End If
        End If
        Return Nothing
    End Function

    'http://support.microsoft.com/kb/q154822/
    'The Dir() function can be used to return a long filename but it does not include path information. 
    'By parsing a given short path/filename into its constituent directories, you can use the Dir() 
    'function to build a long path/filename. This article demonstrates how to accomplish this behavior. 
    Public Function BA_GetLongName(ByVal sShortName As String) As String
        Dim sLongName As String = Nothing
        Dim sTemp As String
        Dim iSlashPos As Integer
        Dim longName As String = Nothing

        'Add \ to short name to prevent Instr from failing
        sShortName = sShortName & "\"

        'Start from 4 to ignore the "[Drive Letter]:\" characters
        iSlashPos = InStr(4, sShortName, "\")

        'Pull out each string between \ character for conversion
        While iSlashPos
            sTemp = Dir(Left$(sShortName, iSlashPos - 1), _
              vbNormal + vbHidden + vbSystem + vbDirectory)
            If sTemp = "" Then
                'Error 52 - Bad File Name or Number
                Return ""
            End If
            sLongName = sLongName & "\" & sTemp
            iSlashPos = InStr(iSlashPos + 1, sShortName, "\")
        End While

        'Prefix with the drive letter
        longName = Left$(sShortName, 2) & sLongName
        Return longName

    End Function

End Module
