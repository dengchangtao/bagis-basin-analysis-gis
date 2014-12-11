Imports BAGIS_ClassLibrary
Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Geoprocessing
Imports ESRI.ArcGIS.DataSourcesRaster

Module ProfileModule

    Public Function BA_GetBagisPSettingsPath() As String
        Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        Dim settingsPath As String = bExt.SettingsPath
        If settingsPath IsNot Nothing Then
            settingsPath = settingsPath & BA_EnumDescription(PublicPath.BagisPSettings)
            Dim parentFolder As String = ""
            Dim fileName As String = BA_GetBareName(settingsPath, parentFolder)
            If Not BA_Folder_ExistsWindowsIO(parentFolder) Then
                fileName = BA_GetBareName(parentFolder)
                Dim newPath As String = BA_CreateFolder(bExt.SettingsPath, fileName)
                If newPath IsNot Nothing Then
                    Return settingsPath
                End If
            Else
                Return settingsPath
            End If
        End If
        Return Nothing
    End Function

    ' Populates the profiles object
    Public Function BA_LoadProfilesFromXml(ByVal myPath As String) As List(Of Profile)
        If BA_Folder_ExistsWindowsIO(myPath) Then
            Dim dirInfo As New DirectoryInfo(myPath)
            Dim allDirectories As FileInfo() = dirInfo.GetFiles
            Dim allProfiles As List(Of Profile) = New List(Of Profile)
            For Each pFile As FileInfo In allDirectories
                If Path.GetExtension(pFile.FullName) = BA_FILE_EXT_XML Then
                    Dim obj As Object = SerializableData.Load(pFile.FullName, GetType(Profile))
                    If obj IsNot Nothing Then
                        Dim pProfile As Profile = CType(obj, Profile)
                        pProfile.XmlFileName = pFile.Name
                        allProfiles.Add(pProfile)
                    End If
                End If
            Next
            Return allProfiles
        Else
            Return Nothing
        End If
    End Function

    Public Function BA_BagisPXmlPath(ByVal parentPath As String, ByVal objName As String) As String
        Return parentPath & "\" & objName & ".xml"
    End Function

    Public Function BA_SaveDataLayers(ByVal layerList As List(Of DataSource), ByVal settingsPath As String) As BA_ReturnCode
        Try
            Dim settings As New Settings
            settings.DataSources = layerList
            settings.Save(settingsPath)
            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_SaveDataLayers exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        End Try
    End Function

    Public Function BA_SaveNewDataSource(ByVal dataSource As DataSource, ByVal settingsPath As String) As BA_ReturnCode
        Try
            Dim srcTable As Hashtable = BA_LoadSettingsFile(settingsPath)
            Dim srcList As List(Of DataSource) = New List(Of DataSource)
            If srcTable IsNot Nothing Then
                For Each key As String In srcTable.Keys
                    Dim pDS As DataSource = srcTable(key)
                    'Overwrite existing but do not add to collection yet
                    If pDS.Name = dataSource.Name Then
                        dataSource.Id = pDS.Id
                    Else
                        srcList.Add(pDS)
                    End If
                Next
            End If
            srcList.Add(dataSource)
            Dim settings As New Settings
            settings.DataSources = srcList
            settings.Save(settingsPath)
            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_SaveNewDataSource: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        End Try
    End Function

    Public Function BA_GetLocalProfilesDir(ByVal aoiPath As String) As String
        'trim trailing backslash from aoiPath
        aoiPath = BA_StandardizePathString(aoiPath)
        Dim sb As StringBuilder = New StringBuilder
        sb.Append(aoiPath)
        sb.Append(BA_EnumDescription(PublicPath.BagisLocalProfiles))
        'return the profiles folder if it exists
        If BA_Folder_ExistsWindowsIO(sb.ToString) Then
            Return sb.ToString
        Else
            'Otherwise create it before returning
            Dim profilesFolder As String = BA_GetBareName(BA_EnumDescription(PublicPath.BagisLocalProfiles))
            Dim paramFolder As String = BA_EnumDescription(PublicPath.BagisParamFolder)
            'Trim leading backslash
            If paramFolder(0) = "\" Then
                paramFolder = paramFolder.Remove(0, 1)
            End If
            Dim newFolder As String = BA_CreateFolder(aoiPath, paramFolder)
            If Not String.IsNullOrEmpty(newFolder) Then
                Dim retFolder As String = BA_CreateFolder(newFolder, profilesFolder)
                If Not String.IsNullOrEmpty(retFolder) Then
                    Return retFolder
                End If
            End If
        End If
        Return Nothing
    End Function

    Public Function BA_GetLocalSettingsPath(ByVal aoiPath As String) As String
        Dim settingsPath As String = aoiPath & BA_EnumDescription(PublicPath.BagisLocalSettings)
        If Not BA_Folder_ExistsWindowsIO(settingsPath) Then
            Dim newPath As String = BA_CreateFolder(aoiPath, BA_EnumDescription(PublicPath.BagisParamFolder))
            If newPath IsNot Nothing Then
                Return settingsPath
            End If
        Else
            Return settingsPath
        End If
        Return Nothing
    End Function

    Public Function BA_ClipLayerToAoi(ByVal aoiPath As String, ByVal dataBinPath As String, _
                                      ByVal dataSource As DataSource) As BA_ReturnCode
        'check if a layer is in the correct projection
        'assumes SetDatumInExtension has been run to set the datum in the extension
        Dim validDatum As Boolean
        Dim pExt As BagisPExtension = BagisPExtension.GetExtension
        If dataSource.LayerType = LayerType.Vector Then 'shapefile
            validDatum = BA_VectorDatumMatch(dataSource.Source, pExt.Datum)
        ElseIf dataSource.LayerType = LayerType.Raster Then
            validDatum = BA_DatumMatch(dataSource.Source, pExt.Datum)
        End If
        If validDatum = False Then
            Dim sb As StringBuilder = New StringBuilder()
            sb.Append("The selected data source '" & dataSource.Name & "'" & vbCrLf)
            sb.Append("cannot be imported because the datum does not" & vbCrLf)
            sb.Append("match the AOI DEM. Please reproject to" & vbCrLf)
            sb.Append(pExt.SpatialReference & " and try again.")
            MessageBox.Show(sb.ToString, "Invalid datum", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return BA_ReturnCode.UnknownError
        End If
        Dim response As Integer = 1
        Dim outputFileName As String = BA_GetBareName(dataSource.Source)
        Dim msg As StringBuilder = New StringBuilder()
        Dim strAoi As String = BA_GetBareName(aoiPath)
        msg.Append("An error occurred while trying to clip" & vbCrLf)
        msg.Append("the data source: '" & dataSource.Name & "' to" & vbCrLf)
        msg.Append("AOI: '" & strAoi & "'")
        Dim outputFullPath As String = dataBinPath & "\" & outputFileName
        If dataSource.LayerType = LayerType.Vector Then
            If BA_File_Exists(outputFullPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                response = BA_Remove_ShapefileFromGDB(dataBinPath, outputFileName)
            End If
            If response > 0 Then
                response = BA_ClipAOIVector(aoiPath, dataSource.Source, outputFileName, dataBinPath, True)
                If response <= 0 Then
                    MessageBox.Show(msg.ToString, "Clipping error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Function
                End If
            End If
        ElseIf dataSource.LayerType = LayerType.Raster Then
            If BA_File_Exists(outputFullPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                response = BA_RemoveRasterFromGDB(dataBinPath, outputFileName)
            End If
            If response > 0 Then
                Dim clipKey As AOIClipFile = 1
                response = BA_ClipAOIRaster(aoiPath, dataSource.Source, outputFileName, dataBinPath, clipKey)
                If response <= 0 Then
                    MessageBox.Show(msg.ToString, "Clipping error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Function
                End If
            End If
        End If

        'Create new data source object
        Dim id As Integer = BA_GetNextDataSourceId(BA_GetLocalSettingsPath(aoiPath))
        Dim localDS As DataSource = New DataSource(id, dataSource.Name, dataSource.Description, outputFileName, _
                                                   dataSource.AoiLayer, dataSource.LayerType)
        Dim success As BA_ReturnCode = BA_SaveNewDataSource(localDS, BA_GetLocalSettingsPath(aoiPath))
        Return success
    End Function

    Public Function BA_GetDataBinPath(ByVal aoiPath As String) As String
        Dim dataBinPath As String = aoiPath & BA_EnumDescription(PublicPath.BagisParamFolder) & BA_EnumDescription(PublicPath.BagisDataBinGdb)
        'Create paramdata.gdb in aoi if it doesn't exist
        Dim success As BA_ReturnCode = BA_ReturnCode.Success
        Dim gdbName As String = Nothing
        If Not BA_File_ExistsWindowsIO(dataBinPath) Then
            gdbName = BA_GetBareName(dataBinPath)
            success = BA_CreateFileGdb(aoiPath & BA_EnumDescription(PublicPath.BagisParamFolder), gdbName)
        End If
        If success = BA_ReturnCode.Success Then
            Return dataBinPath
        Else
            Return Nothing
        End If
    End Function

    'Get the next available data source id from a settings file
    Public Function BA_GetNextDataSourceId(ByVal settingsPath As String) As Integer
        Dim sourceTable As Hashtable = BA_LoadSettingsFile(settingsPath)
        Dim id As Integer
        For Each pKey As String In sourceTable.Keys
            Dim existingSrc As DataSource = sourceTable(pKey)
            'Calculate id for new layer
            If existingSrc.Id >= id Then
                id = existingSrc.Id + 1
            End If
        Next
        Return id
    End Function

    Public Function BA_ValidateUnitsForProfile(ByVal aProfile As Profile, ByVal aMethodTable As Hashtable) As String
        Dim pModel As IGPTool = Nothing
        Dim sb As StringBuilder = New StringBuilder
        Dim params As List(Of ModelParameter) = Nothing
        Try
            Dim unitTypeList As IList(Of MeasurementUnitType) = New List(Of MeasurementUnitType)
            For Each mName As String In aProfile.MethodNames
                Dim pMethod As Method = aMethodTable(mName)
                pModel = BA_OpenModel(pMethod.ToolBoxPath, pMethod.ToolboxName, pMethod.ModelName)
                If pModel IsNot Nothing Then
                    params = BA_GetModelParameters(pModel)
                    If params IsNot Nothing Then
                        For Each pParam As ModelParameter In params
                            If pParam.Name.ToLower = SystemModelParameterName.sys_units_elevation.ToString.ToLower Or
                               pParam.Name.ToLower = SystemModelParameterName.sys_units_slope.ToString.ToLower Or
                               pParam.Name.ToLower = SystemModelParameterName.sys_units_depth.ToString.ToLower Or
                               pParam.Name.ToLower = SystemModelParameterName.sys_units_temperature.ToString.ToLower Then
                                unitTypeList.Add(BA_GetMeasurementUnit(pParam.Name))
                            End If
                        Next
                    End If
                End If
            Next
            If unitTypeList.Count > 0 Then

            End If

            Return sb.ToString
        Catch ex As Exception
            Debug.Print("BA_ValidateUnitsForProfile Exception: " & ex.Message)
            Return sb.ToString
        Finally


        End Try
    End Function

    Public Function BA_SaveHruProfileStatus(ByVal filePath As String, ByVal profileName As String, _
                                            ByVal dateCompleted As Date) As BA_ReturnCode
        Dim pHruProfileStatus As HruProfileStatus = Nothing
        Dim obj As Object = SerializableData.Load(filePath, GetType(HruProfileStatus))
        If obj IsNot Nothing Then
            pHruProfileStatus = CType(obj, HruProfileStatus)
            Dim profileNames As List(Of String) = pHruProfileStatus.ProfileNames
            Dim validProfileNames As List(Of String) = New List(Of String)
            If profileNames Is Nothing Then profileNames = New List(Of String)
            Dim completionDates As List(Of Date) = pHruProfileStatus.CompletionDates
            Dim validCompletionDates As List(Of Date) = New List(Of Date)
            If completionDates Is Nothing Then completionDates = New List(Of Date)
            Dim idx As Integer = 0
            Dim foundIt As Boolean = False
            Dim hruParamsGdbPath As String = "PleaseReturn"
            Dim hruPath As String = "PleaseReturn"
            Dim fileName As String = BA_GetBareName(filePath, hruParamsGdbPath)
            fileName = BA_GetBareName(hruParamsGdbPath)
            For Each pName As String In profileNames
                Dim tableName As String = BA_GetBareName(BA_CalculateSystemParameter(SystemModelParameterName.sys_param_table.ToString, _
                                                                      hruPath, pName, Nothing))
                Dim paramTable As ITable = BA_OpenTableFromGDB(hruParamsGdbPath, tableName)
                If paramTable IsNot Nothing Then
                    validProfileNames.Add(pName)
                    If pName = profileName Then
                        'We are updating an existing entry
                        validCompletionDates.Add(Date.Now)
                        foundIt = True
                    Else
                        validCompletionDates.Add(completionDates.Item(idx))
                    End If
                End If
                idx += 1
            Next
            'We are creating a new entry
            If foundIt = False Then
                validProfileNames.Add(profileName)
                validCompletionDates.Add(Date.Now)
            End If
            pHruProfileStatus.ProfileNames = validProfileNames
            pHruProfileStatus.CompletionDates = validCompletionDates
            pHruProfileStatus.Save(filePath)
            Return BA_ReturnCode.Success
        End If
        Return BA_ReturnCode.UnknownError
    End Function

    Public Function BA_GetLastCompletedDateForHru(ByVal filePath As String) As Date
        Dim pHruProfileStatus As HruProfileStatus = Nothing
        Dim obj As Object = SerializableData.Load(filePath, GetType(HruProfileStatus))
        Dim retVal As Date = Nothing
        If obj IsNot Nothing Then
            pHruProfileStatus = CType(obj, HruProfileStatus)
            Dim completionDates As List(Of Date) = pHruProfileStatus.CompletionDates
            If completionDates IsNot Nothing Then
                For Each pDate As Date In completionDates
                    If retVal = Nothing Then
                        retVal = pDate
                    ElseIf pDate > retVal Then
                        retVal = pDate
                    End If
                Next
            End If
        End If
        Return retVal
    End Function

    Public Function BA_GetCompletedProfileCountForHru(ByVal filePath As String) As Integer
        Dim pHruProfileStatus As HruProfileStatus = Nothing
        Dim obj As Object = SerializableData.Load(filePath, GetType(HruProfileStatus))
        If obj IsNot Nothing Then
            pHruProfileStatus = CType(obj, HruProfileStatus)
            Return pHruProfileStatus.CompletedCount
        End If
        Return 0
    End Function

    Public Function BA_SaveProfileLog(ByVal hruPath As String, ByVal myProfile As Profile, ByVal noDataValue As String, _
                                      ByVal methodTable As Hashtable, ByVal hruMethodTable As Hashtable) As BA_ReturnCode
        Try
            Dim bExt As BagisPExtension = BagisPExtension.GetExtension
            Dim logProfile As LogProfile = New LogProfile(myProfile.Id, myProfile.Name, myProfile.Description, bExt.version, hruPath, _
                                                          DateTime.Now, noDataValue)
            Dim methodArr(myProfile.MethodNames.Count - 1) As Method
            Dim hruMethodArr(myProfile.MethodNames.Count - 1) As Method
            Dim i As Integer = 0
            For Each mName As String In myProfile.MethodNames
                Dim nextMethod As Method = methodTable(mName)
                methodArr(i) = nextMethod
                Dim hruMethod As Method = hruMethodTable(mName)
                hruMethodArr(i) = hruMethod
                i += 1
            Next
            logProfile.Methods = methodArr
            logProfile.HruMethods = hruMethodArr
            Dim logPath As String = hruPath & "\" & logProfile.Name & "_params_log.xml"
            logProfile.Save(logPath)
            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_SaveProfileLog Exception: " & ex.Message)
            Return BA_ReturnCode.WriteError
        End Try
    End Function

    Public Function BA_LoadLogProfileFromXml(ByVal logPath As String) As LogProfile
        If BA_File_ExistsWindowsIO(logPath) Then
            Dim obj As Object = SerializableData.Load(logPath, GetType(LogProfile))
            If obj IsNot Nothing Then
                Dim logProfile As LogProfile = CType(obj, LogProfile)
                Return logProfile
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If
    End Function

    Public Function BA_GetButtonErrorMessage(ByVal formName As String, ByVal e As Exception) As String
        Dim sb As StringBuilder = New StringBuilder
        sb.Append("An error occurred while trying to open the '" & formName & "' form." & vbCrLf)
        If e IsNot Nothing Then
            sb.Append("Error message: " & e.Message)
        End If
        Return sb.ToString
    End Function

    Public Function BA_ValidateProfileNames(ByVal profilesFolder As String) As BA_ReturnCode
        Dim profileList As List(Of Profile) = BA_LoadProfilesFromXml(profilesFolder)
        If profileList IsNot Nothing Then
            ' Put all profiles in a Hashtable with profileName as the key
            Dim profileTable As Hashtable = New Hashtable
            For Each nextProfile In profileList
                profileTable(nextProfile.Name) = nextProfile
            Next
            ' If the hashtable is smaller than the list, we have duplicates
            If profileList.Count > profileTable.Keys.Count Then
                'Rename each profile to match its xml file name
                For Each nextProfile In profileList
                    Dim pName As String = Path.GetFileNameWithoutExtension(nextProfile.XmlFileName)
                    nextProfile.Name = pName
                    nextProfile.Save(BA_BagisPXmlPath(profilesFolder, pName))
                Next
            End If
        End If
        Return BA_ReturnCode.Success
    End Function

End Module
