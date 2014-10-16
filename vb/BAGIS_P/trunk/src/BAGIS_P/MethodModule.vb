Imports BAGIS_ClassLibrary
Imports System.IO
Imports System.Text

Module MethodModule

    Public Function BA_LoadMethodFromXml(ByVal myPath As String, ByVal methodName As String) As Method
        Dim xmlPath As String = BA_BagisPXmlPath(myPath, methodName)
        If BA_File_ExistsWindowsIO(xmlPath) Then
            Dim obj As Object = SerializableData.Load(xmlPath, GetType(Method))
            If obj IsNot Nothing Then
                Dim pMethod As Method = CType(obj, Method)
                Return pMethod
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If
    End Function

    ' Loads a list of all the methods
    Public Function BA_LoadMethodsFromXml(ByVal myPath As String) As List(Of Method)
        If BA_Folder_ExistsWindowsIO(myPath) Then
            Dim dirInfo As New DirectoryInfo(myPath)
            Dim allDirectories As FileInfo() = dirInfo.GetFiles
            Dim allMethods As List(Of Method) = New List(Of Method)
            For Each pFile As FileInfo In allDirectories
                Dim fileExt As String = pFile.Extension
                'Make sure we are trying to serialize an xml file
                If fileExt.ToLower = BA_FILE_EXT_XML Then
                    Dim obj As Object = SerializableData.Load(pFile.FullName, GetType(Method))
                    If obj IsNot Nothing Then
                        Dim pMethod As Method = CType(obj, Method)
                        allMethods.Add(pMethod)
                    End If
                End If
            Next
            Return allMethods
        Else
            Return Nothing
        End If
    End Function

    'Calculates and returns a model system parameter
    Public Function BA_CalculateSystemParameter(ByVal paramName As String, ByVal hruPath As String, _
                                                ByVal profileName As String, ByVal pDataTable As Hashtable) As String
        Dim modelParam As SystemModelParameterName = BA_GetSystemModelParameterName(paramName)
        Dim aoiPath As String = ""
        Dim pos As Integer = hruPath.IndexOf(BA_EnumDescription(PublicPath.HruDirectory))
        If pos > -1 Then
            aoiPath = hruPath.Substring(0, pos)
        End If

        Select Case modelParam
            Case SystemModelParameterName.sys_param_table
                'C:\Docs\Lesley\ochoco_FGDB\zones\hru_ca\param.gdb\Profile1_params
                Dim sb As StringBuilder = New StringBuilder()
                sb.Append(hruPath)
                sb.Append(BA_EnumDescription(PublicPath.BagisParamGdb))
                sb.Append("\" & profileName & BA_PARAM_TABLE_SUFFIX)
                Return sb.ToString
            Case SystemModelParameterName.sys_hru_folder
                'C:\Docs\Lesley\ochoco_FGDB\zones\hru_ca
                Return BA_StandardizePathString(hruPath)
            Case SystemModelParameterName.sys_aoi_path
                'C:\Docs\Lesley\ochoco_FGDB
                Return aoiPath
            Case SystemModelParameterName.sys_hru_name
                'hru_ca
                Return BA_GetBareName(hruPath)
            Case SystemModelParameterName.sys_slope_path
                'C:\Docs\Lesley\ochoco_FGDB\surfaces.gdb\slope
                Dim sb As StringBuilder = New StringBuilder()
                sb.Append(BA_GeodatabasePath(aoiPath, GeodatabaseNames.Surfaces, True))
                sb.Append(BA_GetBareName(BA_EnumDescription(PublicPath.Slope)))
                Return sb.ToString
            Case SystemModelParameterName.sys_units_elevation
                'Get the elevation units from filled_dem
                Dim pUnit As MeasurementUnit = MeasurementUnit.Missing
                Dim inputFolder As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Surfaces)
                Dim inputFile As String = BA_EnumDescription(MapsFileName.filled_dem_gdb)
                Dim tagsList As IList(Of String) = BA_ReadMetaData(inputFolder, inputFile, _
                                                           LayerType.Raster, BA_XPATH_TAGS)
                If tagsList IsNot Nothing Then
                    For Each pInnerText As String In tagsList
                        'This is our BAGIS tag
                        If pInnerText.IndexOf(BA_BAGIS_TAG_PREFIX) = 0 Then
                            Dim strUnits As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_VALUE_TAG)
                            If strUnits IsNot Nothing Then
                                pUnit = BA_GetMeasurementUnit(strUnits)
                            End If
                            Exit For
                        End If
                    Next
                End If
                Return ControlChars.Quote & BA_EnumDescription(pUnit) & ControlChars.Quote
            Case SystemModelParameterName.sys_units_temperature
                Dim pUnit As MeasurementUnit = MeasurementUnit.Missing
                For Each pKey As String In pDataTable.Keys
                    Dim pDataSource As DataSource = pDataTable(pKey)
                    If pDataSource.MeasurementUnitType = MeasurementUnitType.Temperature Then
                        pUnit = pDataSource.MeasurementUnit
                        Exit For
                    End If
                Next
                Return ControlChars.Quote & BA_EnumDescription(pUnit) & ControlChars.Quote
            Case SystemModelParameterName.sys_units_depth
                Dim pUnit As MeasurementUnit = MeasurementUnit.Missing
                Dim inputFolder As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Prism)
                Dim inputFile As String = AOIPrismFolderNames.annual.ToString
                Dim tagsList As IList(Of String) = BA_ReadMetaData(inputFolder, inputFile, _
                                                           LayerType.Raster, BA_XPATH_TAGS)
                If tagsList IsNot Nothing Then
                    For Each pInnerText As String In tagsList
                        'This is our BAGIS tag
                        If pInnerText.IndexOf(BA_BAGIS_TAG_PREFIX) = 0 Then
                            Dim strUnits As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_VALUE_TAG)
                            If strUnits IsNot Nothing Then
                                pUnit = BA_GetMeasurementUnit(strUnits)
                            End If
                            Exit For
                        End If
                    Next
                End If
                Return ControlChars.Quote & BA_EnumDescription(pUnit) & ControlChars.Quote
            Case SystemModelParameterName.sys_units_slope
                Dim pUnit As SlopeUnit = SlopeUnit.Missing
                Dim inputFolder As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Surfaces)
                Dim inputFile As String = BA_GetBareName(BA_EnumDescription(PublicPath.Slope))
                Dim tagsList As IList(Of String) = BA_ReadMetaData(inputFolder, inputFile, _
                                                           LayerType.Raster, BA_XPATH_TAGS)
                If tagsList IsNot Nothing Then
                    For Each pInnerText As String In tagsList
                        'This is our BAGIS tag
                        If pInnerText.IndexOf(BA_BAGIS_TAG_PREFIX) = 0 Then
                            Dim strUnits As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_VALUE_TAG)
                            If strUnits IsNot Nothing Then
                                pUnit = BA_GetMeasurementUnit(strUnits)
                            End If
                            Exit For
                        End If
                    Next
                End If
                Return ControlChars.Quote & BA_EnumDescription(pUnit) & ControlChars.Quote
            Case Else
                Return Nothing
        End Select
    End Function

    'Returns the file path to the selected db source when supplied with the db source name
    Public Function BA_CalculateDbParameter(ByVal aoiPath As String, ByVal pDataTable As Hashtable, _
                                            ByVal paramName As String, ByVal pMethod As Method) As String
        'Only run function if paramName has "db_" prefix
        If paramName.Substring(0, BA_DATABIN_PREFIX.Length).ToLower = BA_DATABIN_PREFIX Then
            'Get the data source alias (name) out of the BAGIS-P method
            Dim dataSourceName As String = BA_GetParamValueFromMethod(pMethod, paramName)
            If Not String.IsNullOrEmpty(dataSourceName) Then
                Dim selDataSource As DataSource = pDataTable(dataSourceName)
                If selDataSource IsNot Nothing Then
                    Dim fullSourcePath As String = BA_GetDataBinPath(aoiPath) & "\" & selDataSource.Source
                    Return fullSourcePath
                End If
            End If
        End If
        Return Nothing
    End Function

    Public Function BA_GetSystemModelParameterName(ByVal paramName As String) As SystemModelParameterName
        For Each param In [Enum].GetValues(GetType(SystemModelParameterName))
            If param.ToString = paramName Then
                Return param
            End If
        Next
        Return Nothing
    End Function

    Public Function BA_LoadSettingsFile(ByVal settingsPath As String) As Hashtable
        Dim pSettings As Settings = Nothing
        Dim layerTable As Hashtable = New Hashtable
        'First try to load an existing settings file
        If BA_File_ExistsWindowsIO(settingsPath) Then
            Dim obj As Object = SerializableData.Load(settingsPath, GetType(Settings))
            If obj IsNot Nothing Then
                pSettings = CType(obj, Settings)
            End If
        End If
        'If settings file doesn't exist then build/save a new one
        If pSettings Is Nothing Then
            Dim dataLayerList As List(Of DataSource) = New List(Of DataSource)
            'Aspect layer
            Dim slopeLayer As DataSource = New DataSource(1, "Aspect", "Slope aspect", "AOI aspect layer", True, LayerType.Raster)
            dataLayerList.Add(slopeLayer)
            'Elevation layer
            Dim elevLayer As DataSource = New DataSource(2, "Elevation", "Filled DEM", "AOI filled DEM layer", True, LayerType.Raster)
            dataLayerList.Add(elevLayer)
            'Flow accumulation layer
            Dim flowLayer As DataSource = New DataSource(3, "FlowAcc", "Flow accumulation", "AOI flow accumulation layer", True, LayerType.Raster)
            dataLayerList.Add(flowLayer)
            pSettings = New Settings()
            pSettings.DataSources = dataLayerList
            pSettings.Save(settingsPath)
        End If
        If pSettings.DataSources IsNot Nothing AndAlso pSettings.DataSources.Count > 0 Then
            Dim paramIdx = settingsPath.IndexOf(BA_EnumDescription(PublicPath.BagisParamFolder))
            For Each dLayer As DataSource In pSettings.DataSources
                Dim sourcePath As String = dLayer.Source
                If paramIdx > -1 Then
                    Dim parentPath = "Please Return"
                    Dim tempFileName = BA_GetBareName(settingsPath, parentPath)
                    'Trim trailing \
                    parentPath = parentPath.Substring(0, parentPath.Length - 1)
                    sourcePath = parentPath & BA_EnumDescription(PublicPath.BagisDataBinGdb) & "\" & dLayer.Source
                End If
                If dLayer.AoiLayer Then
                    layerTable.Add(dLayer.Name, dLayer)
                ElseIf dLayer.LayerType = LayerType.Raster Then
                    Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(sourcePath)
                    If BA_File_Exists(sourcePath, wType, ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTRasterDataset) Then
                        layerTable.Add(dLayer.Name, dLayer)
                    End If
                Else
                    Dim wType As WorkspaceType = BA_GetWorkspaceTypeFromPath(sourcePath)
                    If BA_File_Exists(sourcePath, wType, ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureClass) Then
                        layerTable.Add(dLayer.Name, dLayer)
                    End If
                End If
            Next
        End If
        Return layerTable
    End Function

    Public Function BA_GetLocalMethodsDir(ByVal aoiPath As String) As String
        'trim trailing backslash from aoiPath
        aoiPath = BA_StandardizePathString(aoiPath)
        Dim sb As StringBuilder = New StringBuilder
        sb.Append(aoiPath)
        sb.Append(BA_EnumDescription(PublicPath.BagisLocalMethods))
        'return the profiles folder if it exists
        If BA_Folder_ExistsWindowsIO(sb.ToString) Then
            Return sb.ToString
        Else
            'Otherwise create it before returning
            Dim methodsFolder As String = BA_GetBareName(BA_EnumDescription(PublicPath.BagisLocalMethods))
            Dim paramFolder As String = BA_EnumDescription(PublicPath.BagisParamFolder)
            'Trim leading backslash
            If paramFolder(0) = "\" Then
                paramFolder = paramFolder.Remove(0, 1)
            End If
            Dim newFolder As String = BA_CreateFolder(aoiPath, paramFolder)
            If Not String.IsNullOrEmpty(newFolder) Then
                Dim retFolder As String = BA_CreateFolder(newFolder, methodsFolder)
                If Not String.IsNullOrEmpty(retFolder) Then
                    Return retFolder
                End If
            End If
        End If
        Return Nothing
    End Function

    'Pass in an array of data sources to check their units and populate, if appropriate
    Public Function BA_AppendUnitsToDataSources(ByRef pDataTable As Hashtable, ByVal aoiPath As String) As BA_ReturnCode
        If pDataTable IsNot Nothing AndAlso pDataTable.Keys.Count > 0 Then
            For Each pKey As String In pDataTable.Keys
                Dim pDataSource As DataSource = pDataTable(pKey)
                Dim inputFolder As String = Nothing
                Dim inputFile As String = Nothing
                If pDataSource.AoiLayer = False Then
                    If String.IsNullOrEmpty(aoiPath) Then
                        'This is a public data source
                        inputFolder = "PleaseReturn"
                        inputFile = BA_GetBareName(pDataSource.Source, inputFolder)
                    Else
                        'Otherwise it's a local data source
                        inputFolder = BA_GetDataBinPath(aoiPath)
                        inputFile = pDataSource.Source
                    End If
                    Dim tagsList As IList(Of String) = BA_ReadMetaData(inputFolder, inputFile, _
                                                                       pDataSource.LayerType, _
                                                                       BA_XPATH_TAGS)
                    If tagsList IsNot Nothing AndAlso tagsList.Count > 0 Then
                        For Each pInnerText As String In tagsList
                            'This is our BAGIS tag
                            If pInnerText.IndexOf(BA_BAGIS_TAG_PREFIX) = 0 Then
                                Dim strCategory As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_CATEGORY_TAG)
                                If Not String.IsNullOrEmpty(strCategory) Then
                                    Dim pUnitType As MeasurementUnitType = BA_GetMeasurementUnitType(strCategory)
                                    pDataSource.MeasurementUnitType = pUnitType
                                End If
                                Dim strUnits As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_VALUE_TAG)
                                If Not String.IsNullOrEmpty(strUnits) Then
                                    Dim pUnits As MeasurementUnit = BA_GetMeasurementUnit(strUnits)
                                    If pUnits <> MeasurementUnit.Missing Then
                                        pDataSource.MeasurementUnit = pUnits
                                    Else
                                        'Some special treatment here in case it is a slope unit
                                        Dim slopeUnits As SlopeUnit = BA_GetSlopeUnit(strUnits)
                                        If slopeUnits <> SlopeUnit.Missing Then
                                            pDataSource.SlopeUnit = slopeUnits
                                        End If
                                    End If
                                End If
                            End If
                        Next
                    End If
                End If
            Next
        End If
    End Function

    Public Sub BA_SetMeasurementUnitsForAoi(ByVal aoiPath As String, ByVal aDataTable As Hashtable, _
                                         ByRef slopeUnit As SlopeUnit, ByRef elevUnit As MeasurementUnit, _
                                         ByRef depthUnit As MeasurementUnit, ByRef degreeUnit As MeasurementUnit)
        'Slope units
        Dim inputFolder As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Surfaces)
        Dim inputFile As String = BA_GetBareName(BA_EnumDescription(PublicPath.Slope))
        Dim tagsList As IList(Of String) = BA_ReadMetaData(inputFolder, inputFile, _
                                                   LayerType.Raster, BA_XPATH_TAGS)
        If tagsList IsNot Nothing Then
            For Each pInnerText As String In tagsList
                'This is our BAGIS tag
                If pInnerText.IndexOf(BA_BAGIS_TAG_PREFIX) = 0 Then
                    Dim strUnits As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_VALUE_TAG)
                    If strUnits IsNot Nothing Then
                        slopeUnit = BA_GetSlopeUnit(strUnits)
                    End If
                    Exit For
                End If
            Next
        End If

        'Elevation units
        inputFile = BA_EnumDescription(MapsFileName.filled_dem_gdb)
        tagsList = BA_ReadMetaData(inputFolder, inputFile, _
                                   LayerType.Raster, BA_XPATH_TAGS)
        If tagsList IsNot Nothing Then
            For Each pInnerText As String In tagsList
                'This is our BAGIS tag
                If pInnerText.IndexOf(BA_BAGIS_TAG_PREFIX) = 0 Then
                    Dim strUnits As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_VALUE_TAG)
                    If strUnits IsNot Nothing Then
                        elevUnit = BA_GetMeasurementUnit(strUnits)
                    End If
                    Exit For
                End If
            Next
        End If

        'Depth units
        inputFolder = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Prism)
        inputFile = AOIPrismFolderNames.annual.ToString
        tagsList = BA_ReadMetaData(inputFolder, inputFile, _
                           LayerType.Raster, BA_XPATH_TAGS)
        If tagsList IsNot Nothing Then
            For Each pInnerText As String In tagsList
                'This is our BAGIS tag
                If pInnerText.IndexOf(BA_BAGIS_TAG_PREFIX) = 0 Then
                    Dim strUnits As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_VALUE_TAG)
                    If strUnits IsNot Nothing Then
                        depthUnit = BA_GetMeasurementUnit(strUnits)
                    End If
                    Exit For
                End If
            Next
        End If

        'Degree units
        If aDataTable IsNot Nothing Then
            For Each key In aDataTable.Keys
                Dim pSource As DataSource = aDataTable(key)
                If Not pSource.AoiLayer Then
                    inputFolder = BA_GetDataBinPath(aoiPath)
                    inputFile = pSource.Source
                    tagsList = BA_ReadMetaData(inputFolder, inputFile, pSource.LayerType, BA_XPATH_TAGS)
                    If tagsList IsNot Nothing Then
                        For Each pInnerText As String In tagsList
                            'This is our BAGIS tag
                            If pInnerText.IndexOf(BA_BAGIS_TAG_PREFIX) = 0 Then
                                Dim strCategory As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_CATEGORY_TAG)
                                Dim unitCategory As MeasurementUnitType = BA_GetMeasurementUnitType(strCategory)
                                If unitCategory = MeasurementUnitType.Temperature Then
                                    Dim strUnits As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_VALUE_TAG)
                                    If strUnits IsNot Nothing Then
                                        degreeUnit = BA_GetMeasurementUnit(strUnits)
                                    End If
                                End If
                                Exit For
                            End If
                        Next

                    End If
                End If
            Next
        End If

    End Sub

    Public Function BA_ValidateMeasurementUnitsForAoi(ByVal aDataTable As Hashtable, ByVal depthUnit As MeasurementUnit, _
                                                      ByVal elevUnit As MeasurementUnit, ByVal slopeUnit As SlopeUnit, _
                                                      ByVal degreeUnit As MeasurementUnit) As String
        Dim sb As StringBuilder = New StringBuilder
        If aDataTable IsNot Nothing Then
            For Each key In aDataTable.Keys
                Dim pSource As DataSource = aDataTable(key)
                If Not pSource.AoiLayer Then
                    Dim unitType As MeasurementUnitType = pSource.MeasurementUnitType
                    Select Case unitType
                        Case MeasurementUnitType.Depth
                            Dim unit As MeasurementUnit = pSource.MeasurementUnit
                            If unit <> depthUnit Then
                                sb.AppendLine("The depth measurement units for data source " & pSource.Name)
                                sb.AppendLine("do not match those of the Prism data in this AOI.")
                                sb.AppendLine("The depth measurement units for this AOI are " & BA_EnumDescription(depthUnit))
                                sb.AppendLine("Redefine this data source with the correct units using the")
                                sb.AppendLine("Public Data Manager and reclip the data source to this AOI")
                                sb.AppendLine()
                            End If
                        Case MeasurementUnitType.Elevation
                            Dim unit As MeasurementUnit = pSource.MeasurementUnit
                            If unit <> elevUnit Then
                                sb.AppendLine("The elevation measurement units for data source " & pSource.Name)
                                sb.AppendLine("do not match those of the filled DEM in this AOI.")
                                sb.AppendLine("The elevation measurement units for this AOI are " & BA_EnumDescription(elevUnit))
                                sb.AppendLine("Redefine this data source with the correct units using the")
                                sb.AppendLine("Public Data Manager and reclip the data source to this AOI")
                                sb.AppendLine()
                            End If
                        Case MeasurementUnitType.Slope
                            Dim unit As SlopeUnit = pSource.SlopeUnit
                            If unit <> slopeUnit Then
                                sb.AppendLine("The slope measurement units for data source " & pSource.Name)
                                sb.AppendLine("do not match those of the slope layer in this AOI.")
                                sb.AppendLine("The slope measurement units for this AOI are " & BA_EnumDescription(slopeUnit))
                                sb.AppendLine("Redefine this data source with the correct units using the")
                                sb.AppendLine("Public Data Manager and reclip the data source to this AOI")
                                sb.AppendLine()
                            End If
                        Case MeasurementUnitType.Temperature
                            Dim unit As MeasurementUnit = pSource.MeasurementUnit
                            If unit <> degreeUnit Then
                                sb.AppendLine("The degree measurement units for data source " & pSource.Name)
                                sb.AppendLine("do not match those of the another temperature layer in this AOI.")
                                sb.AppendLine("The degree measurement units for this AOI are " & BA_EnumDescription(degreeUnit))
                                sb.AppendLine("Redefine this data source with the correct units using the")
                                sb.AppendLine("Public Data Manager and reclip the data source to this AOI")
                                sb.AppendLine()
                            End If
                    End Select
                End If
            Next
        End If
        Return sb.ToString
    End Function

    'Validate units to make sure we don't have data sources with different units
    'for the same unit type
    Public Function BA_ValidateMeasurementUnits(ByVal pLayerTable As Hashtable, ByVal pUnitType As MeasurementUnitType, _
                                                ByVal pUnit As MeasurementUnit, ByVal pSlopeUnit As SlopeUnit) As DataSource
        Dim dataSource As DataSource = Nothing
        If pLayerTable IsNot Nothing And pLayerTable.Keys.Count > 0 Then
            For Each strKey As String In pLayerTable.Keys
                Dim nextDataSource As DataSource = pLayerTable(strKey)
                If nextDataSource.MeasurementUnitType <> MeasurementUnitType.Missing Then
                    If nextDataSource.MeasurementUnitType = pUnitType Then
                        If nextDataSource.MeasurementUnitType = MeasurementUnitType.Slope Then
                            If nextDataSource.SlopeUnit <> pSlopeUnit Then
                                Return nextDataSource
                            End If
                        Else
                            If nextDataSource.MeasurementUnit <> pUnit Then
                                Return nextDataSource
                            End If
                        End If
                    End If
                End If
            Next
        End If
        Return dataSource
    End Function

    'This method gets the value for a given parameter name out of a method. Sometimes we store parameter names
    'at the method level. For example a custom data source name such as 'db_landfire_evt'
    Public Function BA_GetParamValueFromMethod(ByVal pMethod As Method, ByVal paramName As String) As String
        Dim strValue As String = Nothing
        Dim methodParams As List(Of ModelParameter) = pMethod.ModelParameters
        If methodParams IsNot Nothing Then
            For Each methodParam As ModelParameter In methodParams
                If methodParam.Name = paramName Then
                    strValue = methodParam.Value
                    Return strValue
                End If
            Next
        End If
        Return strValue
    End Function

End Module
