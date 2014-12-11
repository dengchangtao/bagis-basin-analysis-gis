Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports BAGIS_ClassLibrary
Imports System.IO
Imports System.Windows.Forms
Imports System.Text
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.esriSystem
Imports Microsoft.VisualBasic.FileIO

Public Class FrmExportParametersOms

    Dim m_aoi As Aoi
    Dim m_paramsTable As Hashtable
    Dim m_tablesTable As Hashtable
    Dim m_spatialParamsTable As Hashtable
    Dim m_reqSpatialParameters As IList(Of String)
    Dim m_missingSpatialParameters As IList(Of String)
    Dim m_radplSpatialParameters As IList(Of String)
    Dim m_bagisParameterFilePath As String

    Private Sub BtnSelectAoi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnSelectAoi.Click
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
                m_aoi = New Aoi(aoiName, DataPath, Nothing, bagisPExt.version)
                TxtAoiPath.Text = m_aoi.FilePath
                'ResetForm()
                Me.Text = "Export Parameters (AOI: " & aoiName & m_aoi.ApplicationVersion & " )"
                bagisPExt.aoi = m_aoi

                'Load layer lists
                ' Create a DirectoryInfo of the HRU directory.
                Dim zonesDirectory As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, Nothing)
                Dim dirZones As New DirectoryInfo(zonesDirectory)
                Dim dirZonesArr As DirectoryInfo() = Nothing
                If dirZones.Exists Then
                    dirZonesArr = dirZones.GetDirectories
                    LoadHruLayers(dirZonesArr)
                End If

            End If
        Catch ex As Exception
            MessageBox.Show("BtnSelectAoi_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadHruLayers(ByVal dirZonesArr As DirectoryInfo())
        LstHruLayers.Items.Clear()
        If dirZonesArr IsNot Nothing Then
            Dim item As LayerListItem
            For Each dri In dirZonesArr
                Dim hruFilePath As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, dri.Name) & BA_EnumDescription(PublicPath.HruGrid)
                Dim hruXmlFilePath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, dri.Name) & BA_EnumDescription(PublicPath.HruXml)
                ' Add hru to the list if the grid exists
                If BA_File_Exists(hruFilePath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) And _
                   BA_File_ExistsWindowsIO(hruXmlFilePath) Then
                    'Assume hru is discrete raster since we create it to be so
                    item = New LayerListItem(dri.Name, hruFilePath, LayerType.Raster, True)
                    LstHruLayers.Items.Add(item)
                End If
            Next dri
        End If
    End Sub

    Private Sub LoadProfileList(ByVal hruName As String)
        LstProfiles.Items.Clear()
        Dim hruPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, hruName)
        Dim hruParamPath As String = hruPath & BA_EnumDescription(PublicPath.BagisParamGdb)
        If BA_Folder_ExistsWindowsIO(hruParamPath) Then
            Dim tNames As IList(Of String) = BA_ListTablesInGDB(hruParamPath)
            If tNames IsNot Nothing Then
                For Each pName As String In tNames
                    LstProfiles.Items.Add(pName)
                Next
            End If
        End If
    End Sub

    Private Sub BtnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    Private Sub LstHruLayers_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles LstHruLayers.SelectedIndexChanged
        If LstHruLayers.SelectedIndex > -1 Then
            'Derive the file path for the HRU vector to be displayed
            Dim selItem As LayerListItem = TryCast(LstHruLayers.SelectedItem, LayerListItem)
            Dim hruGdbName As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, selItem.Name)
            Dim vName As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruZonesVector), False)
            Dim hruCount As Integer = BA_CountPolygons(hruGdbName, vName)
            If hruCount > 0 Then
                TxtNHru.Text = CStr(hruCount)
            Else
                TxtNHru.Text = "0"
            End If
            LoadProfileList(selItem.Name)
        End If
    End Sub

    Private Sub BtnSetTemplate_Click(sender As System.Object, e As System.EventArgs) Handles BtnSetTemplate.Click
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            SetTemplate(OpenFileDialog1.FileName)
        End If
    End Sub

    Private Sub BtnDefaultTemplate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDefaultTemplate.Click
        Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        Dim settingsPath As String = bExt.SettingsPath
        Dim profilesFolder As String = BA_GetPublicProfilesPath(settingsPath)
        Dim templatepath As String = profilesFolder & BA_EnumDescription(PublicPath.DefaultParameterTemplate)
        If Not BA_File_ExistsWindowsIO(templatepath) Then
            Dim errMsg As String = "The default parameter template could not be located. "
            errMsg += "BAGIS-P is looking for the template at " & templatepath & "."
            MessageBox.Show(errMsg, "Default template not found", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            SetTemplate(templatepath)
        End If
    End Sub

    Private Function SetTemplate(ByVal pathToFile As String) As BA_ReturnCode
        Dim validParamFile As Boolean = False
        Try
            Using sr As StreamReader = File.OpenText(pathToFile)
                If sr.Peek <> 0 Then
                    Dim firstLine As String = sr.ReadLine
                    If Not String.IsNullOrEmpty(firstLine) Then
                        Dim firstChars As String = firstLine.Substring(0, 2)
                        If firstChars = SECTION_FLAG Then
                            validParamFile = True
                        End If
                    End If
                End If
            End Using
        Catch ex As Exception
            Debug.Print("SetTemplate Exception: " & ex.Message)
        End Try
        If validParamFile = True Then
            TxtParameterTemplate.Text = pathToFile
            'TxtDescription.Text = "Descr: East Fork Carson River, CA" & vbCrLf & "Modified: Wed Aug 29 17:41:47 MDT 2012 " & vbCrLf _
            '                    & "Version: 1.7 " & vbCrLf & "Created: Tue Aug 21 16:02:12 MDT 2012"
            GetTemplateDetails()
            Return BA_ReturnCode.Success
        Else
            MessageBox.Show("The file you selected is not a valid template.", "Invalid template", MessageBoxButtons.OK, MessageBoxIcon.Information)
            TxtParameterTemplate.Text = Nothing
        End If
        Return BA_ReturnCode.UnknownError
    End Function

    Private Sub BtnSetOutput_Click(sender As System.Object, e As System.EventArgs) Handles BtnSetOutput.Click
        Try
            If SaveFileDialog1.ShowDialog = DialogResult.OK Then
                Dim fName As String = SaveFileDialog1.FileName
                'Append .csv extension if not there already
                If Microsoft.VisualBasic.Strings.Right(fName, 4).ToLower = ".csv" Then
                    fName = fName & ".csv"
                End If
                TxtOutputFolder.Text = SaveFileDialog1.FileName
            Else
                TxtOutputFolder.Text = Nothing
            End If
        Catch ex As Exception
            Debug.Print("BtnSetOutput_Click Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub LstProfiles_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles LstProfiles.SelectedIndexChanged
        TxtNumParameters.Text = Nothing
        Dim paramTable As ITable = Nothing
        Dim pFields As IFields = Nothing
        Dim selItem As LayerListItem = TryCast(LstHruLayers.SelectedItem, LayerListItem)
        Try
            If LstProfiles.SelectedIndex > -1 Then
                Dim hruPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, selItem.Name)
                Dim hruParamPath As String = hruPath & BA_EnumDescription(PublicPath.BagisParamGdb)
                If BA_Folder_ExistsWindowsIO(hruParamPath) Then
                    Dim selProfile As String = TryCast(LstProfiles.SelectedItem, String)
                    If selProfile IsNot Nothing Then
                        Dim tableName As String = selProfile & BA_PARAM_TABLE_SUFFIX
                        paramTable = BA_OpenTableFromGDB(hruParamPath, tableName)
                        If paramTable IsNot Nothing Then
                            pFields = paramTable.Fields
                            If pFields IsNot Nothing Then
                                'Subtract the id, the hru_id, and the oms_id columns
                                TxtNumParameters.Text = pFields.FieldCount - 3
                            End If
                        End If
                    End If
                End If
            End If
            ManageEditParametersButton()
            ManageExportButton()
        Catch ex As Exception
            Debug.Print("LstProfiles_SelectedIndexChanged Exception: " & ex.Message)
        Finally
            pFields = Nothing
            paramTable = Nothing
        End Try
    End Sub

    Private Sub BtnEditParameters_Click(sender As System.Object, e As System.EventArgs) Handles BtnEditParameters.Click
        If TxtParameterTemplate.Text.Length > 1 Then
            Try
                Dim frmEditParameters As FrmEditParameters = New FrmEditParameters(Me, m_paramsTable, m_tablesTable, TxtParameterTemplate.Text)
                frmEditParameters.ShowDialog()
            Catch ex As Exception
                Debug.Print("FrmEditParameters initialization Exception: " & ex.Message)
                MessageBox.Show("The selected parameter template could not be read by BAGIS-P. Please select another template.", "Invalid Template", _
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Else
            MessageBox.Show("Please select a template to edit.", "No template selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub GetTemplateDetails()
        Dim parser As TextFieldParser = Nothing

        Try
            parser = New TextFieldParser(TxtParameterTemplate.Text)
            parser.SetDelimiters({","})
            parser.HasFieldsEnclosedInQuotes = True
            Dim fields As String() = parser.ReadFields
            If fields(0) = SECTION_FLAG Then
                ' Get the modified date
                If Not parser.EndOfData Then
                    fields = parser.ReadFields
                    If fields(0) = "Descr" Then
                        TxtDescription.Text = fields(1)
                    End If
                End If
                Do Until parser.EndOfData
                    fields = parser.ReadFields
                    If fields(0) = PARAM_FLAG Then
                        Exit Do
                    ElseIf fields(0) = DESCR Then
                        'Description
                        TxtDescription.Text = fields(1)
                    ElseIf fields(0) = MODIFIED_AT Then
                        'Modified date
                        TxtModified.Text = fields(1)
                        'Date format: Wed Aug 29 17:41:47 MDT 
                    ElseIf fields(0) = VERSION Then
                        'Version
                        TxtVersion.Text = fields(1)
                    ElseIf fields(0) = "CreatedAt" Then
                        'Version
                        TxtCreated.Text = fields(1)
                    End If
                Loop
            End If
        Catch ex As Exception
            Debug.Print("GetTemplateDetails Exception: " & ex.Message)
        Finally
            parser.Close()
        End Try
    End Sub

    Private Sub ManageExportButton()
        If String.IsNullOrEmpty(TxtParameterTemplate.Text) Or String.IsNullOrEmpty(TxtOutputFolder.Text) Or _
            LstProfiles.SelectedIndex < 0 Then
            BtnExport.Enabled = False
        Else
            BtnExport.Enabled = True
        End If
    End Sub

    Private Sub ManageEditParametersButton()
        If String.IsNullOrEmpty(TxtParameterTemplate.Text) Or String.IsNullOrEmpty(TxtNHru.Text) Then
            BtnEditParameters.Enabled = False
            BtnEditHruParameters.Enabled = False
        Else
            BtnEditParameters.Enabled = True
            If LstProfiles.SelectedIndex > -1 Then
                BtnEditHruParameters.Enabled = True
            Else
                BtnEditHruParameters.Enabled = False
            End If
        End If
    End Sub

    Private Sub TxtParameterTemplate_TextChanged(sender As System.Object, e As System.EventArgs) Handles TxtParameterTemplate.TextChanged
        ManageExportButton()
        ManageEditParametersButton()
    End Sub

    Private Sub TxtOutputFolder_TextChanged(sender As System.Object, e As System.EventArgs) Handles TxtOutputFolder.TextChanged
        ManageExportButton()
    End Sub

    Private Sub TxtNHru_TextChanged(sender As Object, e As System.EventArgs) Handles TxtNHru.TextChanged
        ManageEditParametersButton()
    End Sub

    Private Sub BtnExport_Click(sender As System.Object, e As System.EventArgs) Handles BtnExport.Click
        'If we did not edit the paramsTable, it will be nothing and we need to initialize it from the template
        If m_paramsTable Is Nothing Then
            m_paramsTable = BA_GetParameterMap(TxtParameterTemplate.Text, ",", CInt(TxtNHru.Text), TxtAoiPath.Text)
        End If
        'Replace nhru parameter with correct value
        Dim newValue() As String = {TxtNHru.Text}
        Dim nHruParam As Parameter = m_paramsTable(NHRU)
        If nHruParam Is Nothing Then
            nHruParam = New Parameter(NHRU, newValue, True)
        Else
            nHruParam.value = newValue
        End If
        m_paramsTable(NHRU) = nHruParam
        'Replace nradpl parameter with correct value
        Dim nRadplParam As Parameter = m_paramsTable(NRADPL)
        If nRadplParam Is Nothing Then
            nRadplParam = New Parameter(NRADPL, newValue, True)
        Else
            nRadplParam.value = newValue
        End If
        m_paramsTable(NRADPL) = nRadplParam

        'Read log to extract missing data value
        Dim selItem As LayerListItem = CType(LstHruLayers.SelectedItem, LayerListItem)
        Dim hruPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, selItem.Name)
        Dim missingData As String = Nothing
        Dim logProfile As LogProfile = BA_LoadLogProfileFromXml(hruPath & "\" & TryCast(LstProfiles.SelectedItem, String) & "_params_log.xml")
        If logProfile IsNot Nothing Then
            missingData = logProfile.NoDataValue
        End If

        If m_tablesTable Is Nothing Then
            'If we did not edit the paramsTable, it will be nothing and we need to initialize it from the template
            m_tablesTable = BA_GetTableMap(TxtParameterTemplate.Text, ",", m_paramsTable)
        End If

        If m_reqSpatialParameters Is Nothing Then
            'Read the list of BAGIS-P parameter names to exclude
            ReadBagisParameterNames()
        End If

        Dim hruParamPath As String = hruPath & BA_EnumDescription(PublicPath.BagisParamGdb)
        Dim tableName As String = CStr(LstProfiles.SelectedItem) & BA_PARAM_TABLE_SUFFIX
        Dim success As Boolean = VerifyParameterValuesInTable(hruParamPath, tableName, True)
        Dim retVal As BA_ReturnCode = BA_ReturnCode.Success

        If success = True Then
            If m_spatialParamsTable Is Nothing Then
                'If we did not edit the spatial params table, it will be nothing and we need to initialize it from the template
                m_spatialParamsTable = BA_ReadNhruParams(TxtParameterTemplate.Text, ",", TxtNHru.Text, m_reqSpatialParameters, _
                                                         m_missingSpatialParameters, missingData)
            End If
            BA_ExportParameterFile(TxtOutputFolder.Text, TxtDescription.Text, TxtVersion.Text, m_paramsTable, m_tablesTable, hruParamPath, _
                                   tableName, CInt(TxtNHru.Text), m_spatialParamsTable, missingData, m_radplSpatialParameters)
            Dim zipFolder As String = Nothing
            If CkExportZipped.Checked = True Then
                'Export Geodatabase file to shapefile
                Dim targetFolder As String = "Please Return"
                Dim targetFile As String = BA_GetBareName(TxtOutputFolder.Text, targetFolder)
                Dim tempBagisFolder As String = "BagisZip"
                'Delete tempBagisFolder if it exists to make sure we don't have old data
                If BA_Folder_ExistsWindowsIO(targetFolder & tempBagisFolder) Then
                    BA_Remove_Folder(targetFolder & tempBagisFolder)
                End If
                zipFolder = BA_CreateFolder(targetFolder, tempBagisFolder)
                'Strip extension from parameter file name so we can add the .shp
                If Not String.IsNullOrEmpty(targetFile) Then
                    Dim idxExtension As Integer = targetFile.IndexOf(".")
                    If idxExtension > 0 Then
                        targetFile = targetFile.Substring(0, idxExtension)
                    End If
                End If
                Dim hruGdbName As String = BA_GetHruPathGDB(m_aoi.FilePath, PublicPath.HruDirectory, selItem.Name)
                Dim vName As String = BA_StandardizeShapefileName(BA_EnumDescription(PublicPath.HruZonesVector), False)
                retVal = BA_ConvertGDBToShapefile(hruGdbName, vName, zipFolder, targetFile)
                'Copy the parameter file into the tempBagisFolder
                File.Copy(TxtOutputFolder.Text, zipFolder & "\" & BA_GetBareName(TxtOutputFolder.Text), True)
                'Zip up the folder
                Dim zipFileName As String = BA_StandardizeShapefileName(targetFile, False) & ".zip"
                retVal = BA_ZipFolder(zipFolder, zipFileName)
            End If

            If success = True And retVal = BA_ReturnCode.Success Then
                BA_Remove_Folder(zipFolder)
                MessageBox.Show("Parameter file export complete !", _
                                "File export", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub

    Public WriteOnly Property ParamsTable As Hashtable
        Set(value As Hashtable)
            m_paramsTable = value
        End Set
    End Property

    Public WriteOnly Property TablesTable As Hashtable
        Set(value As Hashtable)
            m_tablesTable = value
        End Set
    End Property

    Public WriteOnly Property SpatialParamsTable As Hashtable
        Set(value As Hashtable)
            m_spatialParamsTable = value
        End Set
    End Property

    Private Sub BtnEditHruParameters_Click(sender As System.Object, e As System.EventArgs) Handles BtnEditHruParameters.Click
        Dim nhru As Integer = CInt(TxtNHru.Text)
        If m_reqSpatialParameters Is Nothing Then
            ReadBagisParameterNames()
        End If
        Dim selItem As LayerListItem = CType(LstHruLayers.SelectedItem, LayerListItem)
        Dim hruPath As String = BA_GetHruPath(m_aoi.FilePath, PublicPath.HruDirectory, selItem.Name)
        Dim hruParamPath As String = hruPath & BA_EnumDescription(PublicPath.BagisParamGdb)
        Dim tableName As String = CStr(LstProfiles.SelectedItem) & BA_PARAM_TABLE_SUFFIX
        VerifyParameterValuesInTable(hruParamPath, tableName, False)
        'Read log to extract missing data value
        Dim missingData As String = Nothing
        Dim logProfile As LogProfile = BA_LoadLogProfileFromXml(hruPath & "\" & TryCast(LstProfiles.SelectedItem, String) & "_params_log.xml")
        If logProfile IsNot Nothing Then
            missingData = logProfile.NoDataValue
        End If
        Dim frmEditHruParameters As FrmEditHruParameters = New FrmEditHruParameters(Me, TxtParameterTemplate.Text, ",", _
                                                                                    m_reqSpatialParameters, m_missingSpatialParameters, _
                                                                                    m_spatialParamsTable, missingData)
        frmEditHruParameters.ShowDialog()
    End Sub

    Private Sub ReadBagisParameterNames()
        Dim bExt As BagisPExtension = BagisPExtension.GetExtension
        Dim settingsPath As String = bExt.SettingsPath
        Dim methodsPath As String = BA_GetPublicMethodsPath(settingsPath)
        Dim sr As StreamReader = Nothing
        m_reqSpatialParameters = New List(Of String)
        m_radplSpatialParameters = New List(Of String)
        Try
            'open file for input
            If BA_File_ExistsWindowsIO(methodsPath & BA_EnumDescription(PublicPath.BagisParameters)) Then
                m_bagisParameterFilePath = methodsPath & BA_EnumDescription(PublicPath.BagisParameters)
                sr = File.OpenText(methodsPath & BA_EnumDescription(PublicPath.BagisParameters))
                Dim line As String
                ' Read and display the lines from the file until the end 
                ' of the file is reached.
                Do
                    line = sr.ReadLine()
                    If Not String.IsNullOrEmpty(line) Then
                        Dim chrComment As Char = "#"
                        line = Trim(line)
                        If line(0) <> chrComment Then
                            Dim lineArr As String() = line.Split(",")
                            Dim pName As String = lineArr(0)
                            Dim pDimension As String = lineArr(1)
                            If pDimension = NRADPL Then
                                m_radplSpatialParameters.Add(pName)
                            End If
                            m_reqSpatialParameters.Add(pName)
                        End If
                    End If
                Loop Until line Is Nothing
            End If
        Catch ex As Exception
            Debug.Print("ReadBagisParameterNames Exception: " & ex.Message)
        Finally
            'Don't forget to close the file handle
            If sr IsNot Nothing Then sr.Close()
        End Try
    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        '@ToDo: Default for testing
        'TxtParameterTemplate.Text = "C:\Docs\Lesley\NRCS_Code_Migration\BAGIS_P\params.csv"

    End Sub

    Private Function VerifyParameterValuesInTable(ByVal hruParamFolder As String, ByVal hruParamFile As String, _
                                                  ByVal showWarningMessage As Boolean) As Boolean
        'm_bagisParameterNames
        Dim pTable As ITable = Nothing
        Dim pFields As IFields = Nothing
        m_missingSpatialParameters = New List(Of String)
        Try
            pTable = BA_OpenTableFromGDB(hruParamFolder, hruParamFile)
            If pTable IsNot Nothing Then
                pFields = pTable.Fields
                For Each reqParam As String In m_reqSpatialParameters
                    Dim foundIt As Boolean = False
                    For i As Integer = 0 To pFields.FieldCount - 1
                        Dim pField As Field = pFields.Field(i)
                        Dim fieldName As String = pField.Name
                        If fieldName = reqParam Then
                            foundIt = True
                            Exit For
                        End If
                    Next
                    If foundIt = False Then
                        m_missingSpatialParameters.Add(reqParam)
                    End If
                Next

                If m_missingSpatialParameters.Count > 0 And showWarningMessage = True Then
                    Dim sb As StringBuilder = New StringBuilder
                    sb.Append("The following parameters were missing from the profile you selected: " & vbCrLf)
                    For Each missing As String In m_missingSpatialParameters
                        sb.Append(missing & vbCrLf)
                    Next
                    sb.Append(vbCrLf)
                    sb.Append("You can manually set the missing parameters using the " & vbCrLf)
                    sb.Append("'Edit Hru Parameters' screen." & vbCrLf & vbCrLf)
                    sb.Append("If these parameters are not required, please update the BAGIS-P" & vbCrLf)
                    sb.Append("configuration file located at: " & vbCrLf)
                    sb.Append(m_bagisParameterFilePath & vbCrLf & vbCrLf)
                    sb.Append("Do you still wish to export the parameters ?")
                    Dim res As DialogResult = MessageBox.Show(sb.ToString, "Missing parameters", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If res = Windows.Forms.DialogResult.Yes Then
                        Return True
                    Else
                        Return False
                    End If
                End If
                Return True
            Else
                MessageBox.Show("The parameters could not be located for the profile you selected", "Missing parameters", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return False
            End If
        Catch ex As Exception
            Debug.Print("VerifyParameterValuesInTable Exception: " & ex.Message)
            Return False
        End Try
    End Function

    Private Sub LblParameterTemplate_Click(sender As Object, e As System.EventArgs) Handles LblParameterTemplate.Click
        Dim mText = "BAGIS-P's export function uses an input parameter file template to"
        mText = mText & " produce an output parameter file. It stores non-spatial parameters,"
        mText = mText & " spatial parameters that have a dimension of ""nhru"" (i.e., the number"
        mText = mText & " of HRUs in an AOI) that were calculated by BAGIS-P, and spatial"
        mText = mText & " parameters that have a dimension of nhru but were not calculated"
        mText = mText & " by BAGIS-P." & vbCrLf & vbCrLf
        mText = mText & "During the export process, BAGIS-P retrieves parameter values from"
        mText = mText & " the input template file and copies them to the export file except"
        mText = mText & " for spatial parameters calculated by BAGIS-P. Parameters not "
        mText = mText & " calculated by BAGIS-P can be edited using buttons on the Export Parameters form."
        MessageBox.Show(mText, "Parameter Template", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
End Class