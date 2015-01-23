Imports System.Windows.Forms
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports System.IO
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework
Imports System.Text
Imports ESRI.ArcGIS.Geodatabase
Imports BAGIS_ClassLibrary

Public Class FrmUseTemplate

    Dim m_aoi As Aoi
    Dim m_templateAoi As Aoi
    Dim m_version As String
    Dim m_lstHruLayersItem As LayerListItem = Nothing
    Dim m_lstLabel As List(Of Label)
    Dim m_lstButton As List(Of Windows.Forms.Button)
    Dim m_lstHru As List(Of Hru)


    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Set AOI if already set
        Dim hruExt As HruExtension = HruExtension.GetExtension
        Dim aoi As Aoi = hruExt.aoi
        If aoi IsNot Nothing Then
            TxtAoiPath.Text = aoi.FilePath
            BtnTemplateAOI.Enabled = True
            m_aoi = aoi
        End If

        ' Add any initialization after the InitializeComponent() call.
        m_lstLabel = New List(Of Label)
        m_lstButton = New List(Of Windows.Forms.Button)
        m_lstHru = New List(Of Hru)
    End Sub

    Private Sub BtnAOI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAOI.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim DataPath As String
        Dim pFilter As IGxObjectFilter = New GxFilterContainers

        Dim hruExt As HruExtension = HruExtension.GetExtension
        m_version = hruExt.version

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

            'check AOI status
            Dim success As BA_ReturnCode = BA_CheckAoiStatus(DataPath, My.ArcMap.Application.hWnd, My.ArcMap.Document)
            If success = BA_ReturnCode.Success Then
                Dim aoiName As String = BA_GetBareName(DataPath)
                m_aoi = New Aoi(aoiName, DataPath, Nothing, m_version)
                TxtAoiPath.Text = m_aoi.FilePath
                BtnTemplateAOI.Enabled = True
            End If
        Catch ex As Exception
            MessageBox.Show("COMException: " & ex.Message)
        End Try
    End Sub

    Private Sub BtnTemplateAOI_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnTemplateAOI.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim DataPath As String
        Dim pFilter As IGxObjectFilter = New GxFilterContainers

        Dim hruExt As HruExtension = HruExtension.GetExtension
        m_version = hruExt.version

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
                Dim aoiName As String = BA_GetBareName(DataPath)
                m_templateAoi = New Aoi(aoiName, DataPath, Nothing, m_version)
                TxtTemplateAoiPath.Text = m_templateAoi.FilePath

                ' Create a DirectoryInfo of the HRU directory.
                Dim zonesDirectory As String = BA_GetHruPath(m_templateAoi.FilePath, PublicPath.HruDirectory, Nothing)
                Dim dirZones As New DirectoryInfo(zonesDirectory)
                Dim dirZonesArr As DirectoryInfo() = Nothing
                If dirZones.Exists Then
                    dirZonesArr = dirZones.GetDirectories
                    LoadHruLayers(dirZonesArr)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("BtnTemplateAOI_Click: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadHruLayers(ByVal dirZonesArr As DirectoryInfo())

        LstHruLayers.Items.Clear()
        If dirZonesArr IsNot Nothing Then
            Dim zoneCount As Integer = dirZonesArr.Length
            ' Create/configure a step progressor
            Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 2 + zoneCount)
            pStepProg.Show()
            ' Create/configure the ProgressDialog. This automatically displays the dialog
            Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "Loading HRU list", "Loading...")
            progressDialog2.ShowDialog()
            pStepProg.Step()

            Dim item As LayerListItem
            For Each dri In dirZonesArr
                Dim hruFilePath As String = BA_GetHruPathGDB(m_templateAoi.FilePath, PublicPath.HruDirectory, dri.Name) & BA_EnumDescription(PublicPath.HruGrid)
                Dim hruXmlFilePath As String = BA_GetHruPath(m_templateAoi.FilePath, PublicPath.HruDirectory, dri.Name) & BA_EnumDescription(PublicPath.HruXml)

                ' Add hru to the list if the grid exists
                If BA_File_Exists(hruFilePath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) And
                   BA_File_ExistsWindowsIO(hruXmlFilePath) Then
                    'Assume hru is discrete raster since we create it to be so
                    item = New LayerListItem(dri.Name, hruFilePath, LayerType.Raster, True)
                    LstHruLayers.Items.Add(item)
                End If
                pStepProg.Step()
            Next dri
            ' Clean up step progressor
            progressDialog2.HideDialog()
            pStepProg.Hide()
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(progressDialog2)
            ESRI.ArcGIS.ADF.ComReleaser.ReleaseCOMObject(pStepProg)
        End If
    End Sub

    Private Sub LstHruLayers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles LstHruLayers.SelectedIndexChanged
        m_lstHru.Clear()
        ResetHruDetails()
        ' unselect previous selected item
        If m_lstHruLayersItem IsNot Nothing Then
            LstHruLayers.SelectedItems.Remove(m_lstHruLayersItem)
        End If
        ' reset selected index to new value
        m_lstHruLayersItem = LstHruLayers.SelectedItem
        If m_lstHruLayersItem IsNot Nothing Then
            Dim hruAoi As Aoi = Nothing
            Try
                Dim hruItem As LayerListItem = CType(LstHruLayers.SelectedItem, LayerListItem)
                Dim hruInputPath As String = BA_GetHruPath(m_templateAoi.FilePath, PublicPath.HruDirectory, hruItem.Name)
                ' load xml for selected hru
                hruAoi = BA_LoadHRUFromXml(hruInputPath)
                Dim hruExt As HruExtension = HruExtension.GetExtension
                If hruAoi IsNot Nothing Then
                    ' verify application version of selected hru
                    If hruAoi.ApplicationVersion <> hruExt.version Then
                        MessageBox.Show(hruAoi.Name & " was created using an older version of BAGIS-HD. The template may not work.", "Old version", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                    Dim hru As Hru = hruAoi.HruList.Item(0)
                    ' Create temporary list of hru, we want to store them in inverse order once we have them all
                    Dim tempLstHru = New List(Of Hru)
                    ' Add selected hru to the temporary array
                    tempLstHru.Add(hru)
                    Dim parentHru As Hru = hru.ParentHru
                    ' Add all of the ancestors to the temporary array
                    Do While parentHru IsNot Nothing
                        tempLstHru.Add(parentHru)
                        parentHru = parentHru.ParentHru
                    Loop
                    ' Add these items to the current m_templatehru; Can't just copy over the 
                    ' xml hru because the aoi file path may be different if created on another computer
                    Dim templateHruList = New List(Of Hru)
                    templateHruList.AddRange(hruAoi.HruList)
                    m_templateAoi.HruList = templateHruList
                    'Add a button for each ancestor hru
                    'Work backwards through the array so we show the grandparents first
                    Dim x, y As Integer
                    x = 2
                    y = 270
                    Dim hruCount As Int32 = tempLstHru.Count - 1
                    Dim counter As Integer = 1
                    For i = hruCount To 0 Step -1
                        Dim nextHru As Hru = tempLstHru.Item(i)
                        AddNewButton(counter, nextHru, x, y)
                        ' counter used as step # on buttons
                        counter += 1
                        y += 40
                        ' add rule to global list of rules
                        m_lstHru.Add(nextHru)
                    Next
                    ' verify presence of source layers in local template directory
                    ' function returns a list of error messages (Strings)
                    Dim msgList As List(Of String) = BA_ValidateHruTemplate(m_templateAoi.FilePath, m_aoi.FilePath, _
                                                                            m_lstHru)
                    ' if we have any error messages, display them in TxtErrorMsg
                    If msgList.Count > 0 Then
                        Dim sb As New StringBuilder()
                        For Each msg In msgList
                            sb.Append(msg)
                            sb.Append(" ")
                        Next
                        TxtErrorMsg.Text = sb.ToString
                    End If

                    'Turn on generate aoi button if everything is there
                    If TxtAoiPath.Text IsNot Nothing And TxtTemplateAoiPath.Text IsNot Nothing Then
                        BtnGenerateHru.Enabled = True
                    End If
                Else
                    ' Couldn't find the xml for this hru
                    MessageBox.Show("The history is missing for this HRU. It cannot be used as a template", "Missing history", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            Catch ex As Exception
                MessageBox.Show("An unknown error occurred while trying to load the hru and it cannot be used as a template.", "Unknown error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End Try
        Else

        End If
    End Sub

    Private Sub AddNewLabel(ByVal labelText As String, ByVal x As Int32, ByVal y As Int32)
        Dim lbl As Label = New Label
        lbl.Text = labelText
        lbl.Left = x
        lbl.Top = y
        lbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Controls.Add(lbl)
        m_lstLabel.Add(lbl)
    End Sub

    Private Sub AddNewButton(ByVal idx As Int32, ByVal pHru As Hru, _
                             ByVal x As Int32, ByVal y As Int32)
        Dim btn As System.Windows.Forms.Button = New System.Windows.Forms.Button
        btn.Text = idx & ". " & pHru.Name
        btn.Tag = pHru
        btn.Name = "BtnHru"
        btn.Left = x
        btn.Top = y
        btn.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        btn.TextAlign = Drawing.ContentAlignment.MiddleLeft
        btn.Width = 119
        btn.Height = 30
        AddHandler btn.Click, AddressOf MyEventHandler 'Dynamically add the event handle
        Me.Controls.Add(btn)
        m_lstButton.Add(btn)

        Dim pictCheck As PictureBox = New PictureBox
        pictCheck.Image = My.Resources.GenericCheckMark32
        pictCheck.Location = New System.Drawing.Point(x + 122, y)
        pictCheck.Name = "PictCheck"
        pictCheck.Size = New System.Drawing.Size(28, 28)
        pictCheck.TabIndex = 76
        pictCheck.TabStop = False
        pictCheck.SizeMode = PictureBoxSizeMode.StretchImage
        pictCheck.Visible = False
        Me.Controls.Add(pictCheck)

    End Sub

    Private Sub MyEventHandler(ByVal sender As Object, ByVal e As System.EventArgs)
        ' Remove the old tab control
        Dim oldTabControls As Control() = Me.Controls.Find("TabControl1", False)
        If oldTabControls IsNot Nothing Then
            For Each ctrl In oldTabControls
                Me.Controls.Remove(ctrl)
            Next
        End If

        ' Cast the object into a Button.
        Dim btn As Windows.Forms.Button = CType(sender, Windows.Forms.Button)
        Dim pHru As Hru = CType(btn.Tag, Hru)
        Dim frmHruTabLog As FrmHruTabLog = New FrmHruTabLog(m_templateAoi, pHru.Name, False)
        Dim tabControl1 As TabControl = frmHruTabLog.TabControl1
        tabControl1.Left = 160
        tabControl1.Top = 270
        Me.Controls.Add(tabControl1)

        '' Activate the appropriate checkbox
        'Dim pictures As Control() = Me.Controls.Find("PictCheck", False)
        'If pictures IsNot Nothing Then
        '    For Each ctrl In pictures
        '        ctrl.Visible = False
        '    Next
        'End If

        'Dim buttons As Control() = Me.Controls.Find("BtnHru", False)
        'If buttons IsNot Nothing Then
        '    Dim idx As Integer
        '    For Each ctrl In buttons
        '        If ctrl.Text.IndexOf(pHru.Name) > -1 Then
        '            Dim selPicture As Control = pictures(idx)
        '            selPicture.Visible = True
        '            Exit For
        '        End If
        '        idx += 1
        '    Next
        'End If

    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub ResetHruDetails()
        ' Remove the tab control
        Dim oldTabControls As Control() = Me.Controls.Find("TabControl1", False)
        If oldTabControls IsNot Nothing Then
            For Each ctrl In oldTabControls
                Me.Controls.Remove(ctrl)
            Next
        End If

        ' Remove the buttons
        Dim oldButtons As Control() = Me.Controls.Find("BtnHru", False)
        If oldButtons IsNot Nothing Then
            For Each ctrl In oldButtons
                Me.Controls.Remove(ctrl)
            Next
        End If

        ' Remove the images
        Dim pictures As Control() = Me.Controls.Find("PictCheck", False)
        If pictures IsNot Nothing Then
            For Each ctrl In pictures
                Me.Controls.Remove(ctrl)
            Next
        End If

        ' Clear errors in textbox
        TxtErrorMsg.Text = Nothing

    End Sub

    'Private Sub TxtNewHruName_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtNewHruName.TextChanged
    '    ' Must have at least one pending rule
    '    If TxtAoiPath.Text IsNot Nothing And TxtTemplateAoiPath.Text IsNot Nothing _
    '    And m_lstHru.Count > 0 Then
    '        If TxtNewHruName.Text.Length > 0 Then
    '            BtnGenerateHru.Enabled = True
    '        Else
    '            BtnGenerateHru.Enabled = False
    '        End If
    '    End If
    'End Sub

    Private Sub BtnGenerateHru_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnGenerateHru.Click
        If TxtAoiPath.Text = TxtTemplateAoiPath.Text Then
            MessageBox.Show("Output AOI path and template AOI path cannot be the same or the template hru will be overwritten.", "Invalid AOI paths", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        Dim buttons As Control() = Me.Controls.Find("BtnHru", False)
        Dim pictures As Control() = Me.Controls.Find("PictCheck", False)
        BtnGenerateHru.Enabled = False
        Dim retVal As BA_ReturnCode = BA_GenerateHruFromTemplate(TxtTemplateAoiPath.Text, m_aoi, _
                                                                 m_lstHru, buttons, pictures)
        If retVal <> BA_ReturnCode.Success Then
            BtnGenerateHru.Enabled = True
        Else
            LstHruLayers.SelectedItems.Clear()
            'ResetHruDetails()
            BtnGenerateHru.Enabled = True
        End If
    End Sub
End Class