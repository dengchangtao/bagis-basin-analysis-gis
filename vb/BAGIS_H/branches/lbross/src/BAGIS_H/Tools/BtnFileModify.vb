Imports ESRI.ArcGIS.Geodatabase
Imports BAGIS_ClassLibrary
Imports System.ComponentModel
Imports ESRI.ArcGIS.DataSourcesRaster
Imports ESRI.ArcGIS.DataSourcesGDB
Imports ESRI.ArcGIS.Catalog
Imports System.IO
Imports System.Xml

Public Class BtnFileModify
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()

    End Sub

    Protected Overrides Sub OnClick()
        GetModifyDate()
    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub

    Private Sub GetModifyDate()
        Dim pFC As IFeatureClass
        Dim pDFS As IDatasetFileStat
        Try

            Dim aoiPath As String = "D:\Momeni\AOIs\teton_aoi"
            Dim gdbPath As String = Nothing
            Dim values() As Integer = CType([Enum].GetValues(GetType(GeodatabaseNames)), Integer())
            For Each value As GeodatabaseNames In values
                'gdbPath = BA_GeodatabasePath(aoiPath, value)
                'VectorNames(gdbPath)
                'RasterNames(gdbPath)
                ReadBagisXml()
                'This code gets the modifyDate for GDB folders. Don't plan to use
                'Dim dirInfo As DirectoryInfo = New DirectoryInfo(gdbPath)
                'Dim dateModified As DateTime = dirInfo.LastWriteTime
                'Debug.Print(BA_EnumDescription(value) & vbTab & vbTab & dateModified.ToLocalTime)
            Next
            MsgBox("Done!")
        Catch ex As Exception
            Debug.Print("GetModifyDate Exception:" & ex.Message)
        Finally
            pFC = Nothing
            pDFS = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub

    Private Sub VectorNames(ByVal workspaceName As String)
        Dim pWSF As IWorkspaceFactory = New FileGDBWorkspaceFactory()
        Dim pWorkspace As IWorkspace = Nothing
        Dim pEnumDSName As IEnumDatasetName = Nothing
        Dim pDSName As IDatasetName = Nothing
        Dim pDNFS As IDatasetNameFileStat2 = Nothing

        Try
            ' Strip trailing "\" if exists
            If workspaceName(Len(workspaceName) - 1) = "\" Then
                workspaceName = workspaceName.Remove(Len(workspaceName) - 1, 1)
            End If
            pWorkspace = pWSF.OpenFromFile(workspaceName, 0)
            ' Delete rasters
            pEnumDSName = pWorkspace.DatasetNames(esriDatasetType.esriDTFeatureClass)
            pDSName = pEnumDSName.Next
            Dim pName As String = Nothing
            While Not pDSName Is Nothing
                pName = pDSName.Name
                pDNFS = CType(pDSName, IDatasetNameFileStat2)
                Dim intSeconds As Integer = pDNFS.StatTime(esriDatasetFileStatTimeMode.esriDatasetFileStatTimeLastModification)
                Dim baseTime As DateTime = New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                Dim fileTime As DateTime = baseTime.AddSeconds(intSeconds)
                Debug.Print(pName & vbTab & vbTab & fileTime.ToLocalTime())
                pDSName = pEnumDSName.Next
            End While
        Catch ex As Exception
            MsgBox("DatasetNames exception: " & ex.Message)
        Finally
            pWSF = Nothing
            pWorkspace = Nothing
            pEnumDSName = Nothing
            pDSName = Nothing
            pDNFS = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub

    Private Sub RasterNames(ByVal workspaceName As String)
        Dim pWSF As IWorkspaceFactory = New FileGDBWorkspaceFactory()
        Dim pWorkspace As IWorkspace = Nothing
        Dim pEnumDSName As IEnumDatasetName = Nothing
        Dim pDSName As IDatasetName = Nothing
        Dim pMeta As IMetadata = Nothing
        Dim pXmlPropertySet As IXmlPropertySet2 = Nothing
        Try
            ' Strip trailing "\" if exists
            If workspaceName(Len(workspaceName) - 1) = "\" Then
                workspaceName = workspaceName.Remove(Len(workspaceName) - 1, 1)
            End If
            pWorkspace = pWSF.OpenFromFile(workspaceName, 0)
            ' READING RASTER XML
            pEnumDSName = pWorkspace.DatasetNames(esriDatasetType.esriDTRasterDataset)
            pDSName = pEnumDSName.Next
            Dim pName As String = Nothing
            While Not pDSName Is Nothing
                pName = pDSName.Name
                pMeta = BA_GetMetadataForRaster(workspaceName, pName)
                'Get a reference to the IXmlPropertySet2 from the metadata
                pXmlPropertySet = pMeta.Metadata
                'Import only the ESRI node
                Dim strXml As String = pXmlPropertySet.GetXml("/metadata/Esri")
                'Create a VB .NET XmlDocument and load the schema
                Dim myXml As XmlDocument = New XmlDocument
                myXml.LoadXml(strXml)
                'Select all child nodes of Esri node
                For Each pNode As XmlNode In myXml.ChildNodes(0).ChildNodes
                    If pNode.Name = "CreaDate" Then
                        Debug.Print(pName & vbTab & vbTab & pNode.InnerText)
                    End If
                Next
                pDSName = pEnumDSName.Next
            End While
        Catch ex As Exception
            MsgBox("DatasetNames exception: " & ex.Message)
        Finally
            pWSF = Nothing
            pWorkspace = Nothing
            pEnumDSName = Nothing
            pDSName = Nothing
            pMeta = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub

    Private Sub ReadBagisXml()
        Dim pWSF As IWorkspaceFactory = New FileGDBWorkspaceFactory()
        Dim pWorkspace As IWorkspace = Nothing
        Dim pEnumDSName As IEnumDatasetName = Nothing
        Dim fEnumDSName As IEnumDatasetName = Nothing
        Dim pDSName As IDatasetName = Nothing
        Dim pMeta As IMetadata = Nothing
        Dim pXmlPropertySet As IXmlPropertySet2 = Nothing
        Dim pDNFS As IDatasetNameFileStat2 = Nothing
        Try
            Dim workspaceName As String = "D:\Momeni\AOIs\XMLTesting\XMLtesting.gdb"
            Debug.Print("workspaceName: " & workspaceName)
            ' Strip trailing "\" if exists
            If workspaceName(Len(workspaceName) - 1) = "\" Then
                workspaceName = workspaceName.Remove(Len(workspaceName) - 1, 1)
            End If
            pWorkspace = pWSF.OpenFromFile(workspaceName, 0)
            ' Read rasters
            pEnumDSName = pWorkspace.DatasetNames(esriDatasetType.esriDTRasterDataset)
            pDSName = pEnumDSName.Next
            Dim pName As String = Nothing
            While Not pDSName Is Nothing
                pName = pDSName.Name
                pMeta = BA_GetMetadataForRaster(workspaceName, pName)
                'Get a reference to the IXmlPropertySet2 from the metadata
                pXmlPropertySet = pMeta.Metadata
                'Import only the ESRI node
                Dim strXml As String = pXmlPropertySet.GetXml("/metadata/BAGIS")
                'Create a VB .NET XmlDocument and load the schema
                Dim myXml As XmlDocument = New XmlDocument
                myXml.LoadXml(strXml)
                'Select all child nodes of BAGIS node
                For Each pNode As XmlNode In myXml.ChildNodes(0).ChildNodes
                    Debug.Print("Name:" & pNode.Name & vbTab & vbTab & "InnerText: " & pNode.InnerText)
                Next
                pDSName = pEnumDSName.Next
            End While
            'READING VECTOR XML
            pWorkspace = pWSF.OpenFromFile(workspaceName, 0)
            fEnumDSName = pWorkspace.DatasetNames(esriDatasetType.esriDTFeatureClass)
            pDSName = fEnumDSName.Next
            While Not pDSName Is Nothing
                pName = pDSName.Name
                pMeta = BA_GetMetadataForVector(workspaceName, pName)
                'Get a reference to the IXmlPropertySet2 from the metadata
                pXmlPropertySet = pMeta.Metadata
                'Import only the ESRI node
                Dim strXml As String = pXmlPropertySet.GetXml("/metadata/BAGIS")
                'Create a VB .NET XmlDocument and load the schema
                Dim myXml As XmlDocument = New XmlDocument
                myXml.LoadXml(strXml)
                'Select all child nodes of BAGIS node
                For Each pNode As XmlNode In myXml.ChildNodes(0).ChildNodes
                    Debug.Print("Name:" & pNode.Name & vbTab & vbTab & "InnerText: " & pNode.InnerText)
                Next
                pDNFS = CType(pDSName, IDatasetNameFileStat2)
                Dim intSeconds As Integer = pDNFS.StatTime(esriDatasetFileStatTimeMode.esriDatasetFileStatTimeCreation)
                Dim baseTime As DateTime = New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                Dim fileTime As DateTime = baseTime.AddSeconds(intSeconds)
                Debug.Print("DateCreated:" & vbTab & vbTab & fileTime.ToLocalTime())
                intSeconds = pDNFS.StatTime(esriDatasetFileStatTimeMode.esriDatasetFileStatTimeLastModification)
                baseTime = New DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                fileTime = baseTime.AddSeconds(intSeconds)
                Debug.Print("DateModified:" & vbTab & vbTab & fileTime.ToLocalTime())
                pDSName = fEnumDSName.Next
            End While
        Catch ex As Exception
            MsgBox("DatasetNames exception: " & ex.Message)
        Finally
            pWSF = Nothing
            pWorkspace = Nothing
            pEnumDSName = Nothing
            pDSName = Nothing
            pMeta = Nothing
            pDNFS = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Sub
End Class
