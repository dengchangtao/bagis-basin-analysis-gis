Imports System.IO
Imports BAGIS_ClassLibrary



Public Class BtnSaveAOIMXD
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public WriteOnly Property selectedProperty As Boolean
        Set(ByVal value As Boolean)
            Me.Enabled = value
        End Set
    End Property
    Public Sub New()
        Me.Enabled = False
    End Sub

    Protected Overrides Sub OnClick()
        'save the mxd to the AOI folder using the name of the aoi
        Dim pApp As ESRI.ArcGIS.Framework.IApplication = My.ArcMap.Application
        Dim response As Integer
        Dim SaveMxd As Boolean
        Dim mxdPathName As String
        If Len(AOIFolderBase) = 0 Or Len(BA_GetBareName(AOIFolderBase)) = 0 Then
            MsgBox("AOI folder was not set correctly! Please report to the system administrator.")
            Exit Sub
        End If
        Dim mapPath As String = AOIFolderBase & "\maps"
        If Not BA_Workspace_Exists(mapPath) Then
            mapPath = BA_CreateFolder(AOIFolderBase, "maps")
        End If
        mxdPathName = mapPath & "\" & BA_GetBareName(AOIFolderBase) & ".mxd"

        'check if the AOI exist
        SaveMxd = True
        'If Len(Dir(mxdPathName, vbNormal)) <> 0 Then 'i.e., output file already exists
        If File.Exists(mxdPathName) Then
            response = MsgBox(mxdPathName & " already exists! Overwrite?", vbOKCancel)
            If response = vbCancel Then SaveMxd = False
        End If
        If SaveMxd Then
            pApp.SaveAsDocument(mxdPathName)
            If Not pApp Is Nothing Then
                MsgBox(BA_GetBareName(mxdPathName) & " was saved!")
            End If
        End If
    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub
End Class
