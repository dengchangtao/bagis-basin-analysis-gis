Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesFile
Imports System.Windows.Forms
Imports System.Text
Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.Desktop.AddIns
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework

Public Class frmSettings

    Private Sub CmdSetTerrainRef_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSetTerrainRef.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing

        Dim pFilter As IGxObjectFilter = New GxFilterLayers

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select Terrain Reference Layer"
            .ObjectFilter = pFilter
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then Exit Sub

        'get the name of the selected folder
        Dim pGxDatasetLayer As IGxLayer = pGxObject.Next
        Dim pGxFile As IGxFile
        pGxFile = pGxDatasetLayer
        If Len(Trim(pGxFile.Path)) = 0 Then Exit Sub 'user cancelled the action
        txtTerrain.Text = pGxFile.Path

        CmdUndo.Enabled = True
    End Sub
    Private Sub CmdSetDrainageRef_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSetDrainageRef.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing

        Dim pFilter As IGxObjectFilter = New GxFilterLayers

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select Drainage Reference Layer"
            .ObjectFilter = pFilter
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then Exit Sub

        'get the name of the selected folder
        Dim pGxDatasetLayer As IGxLayer = pGxObject.Next
        Dim pGxFile As IGxFile = pGxDatasetLayer
        If Len(Trim(pGxFile.Path)) = 0 Then Exit Sub 'user cancelled the action
        txtDrainage.Text = pGxFile.Path

        CmdUndo.Enabled = True
    End Sub
    Private Sub CmdSetWatershedRef_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSetWatershedRef.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing

        Dim pFilter As IGxObjectFilter = New GxFilterLayers

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select Watershed Reference Layer"
            .ObjectFilter = pFilter
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then Exit Sub

        'get the name of the selected folder
        Dim pGxDatasetLayer As IGxLayer = pGxObject.Next
        Dim pGxFile As IGxFile = pGxDatasetLayer
        If Len(Trim(pGxFile.Path)) = 0 Then Exit Sub 'user cancelled the action
        txtWatershed.Text = pGxFile.Path

        CmdUndo.Enabled = True
    End Sub

    Private Sub CmdSet10MDEM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSet10MDEM.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog
        pGxDialog = New GxDialog
        Dim Data_Path As String, Data_Name As String
        Dim data_fullname As String
        Dim pGxObject As IEnumGxObject = Nothing

        Dim pFilter As IGxObjectFilter
        pFilter = New GxFilterRasterDatasets

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select 10 Meters DEM"
            .ObjectFilter = pFilter
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then Exit Sub

        'get the name of the selected folder
        Dim pGxDataset As IGxDataset
        pGxDataset = pGxObject.Next
        Dim pDatasetName As IDatasetName
        pDatasetName = pGxDataset.DatasetName
        Data_Path = pDatasetName.WorkspaceName.PathName
        Data_Name = pDatasetName.Name
        data_fullname = Data_Path & Data_Name
        If Len(Trim(data_fullname)) = 0 Then Exit Sub 'user cancelled the action
        txtDEM10.Text = data_fullname
        CmdUndo.Enabled = True
    End Sub
    Private Sub CmdSet30MDEM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSet30MDEM.Click
        Dim bObjectSelected As Boolean = True
        Dim pGxDialog As IGxDialog = New GxDialog

        Dim Data_Path As String, Data_Name As String
        Dim data_fullname As String
        Dim pGxObject As IEnumGxObject = Nothing

        Dim pFilter As IGxObjectFilter
        pFilter = New GxFilterRasterDatasets

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select 30 Meters DEM"
            .ObjectFilter = pFilter
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then Exit Sub

        'get the name of the selected folder
        Dim pGxDataset As IGxDataset
        pGxDataset = pGxObject.Next
        Dim pDatasetName As IDatasetName
        pDatasetName = pGxDataset.DatasetName
        Data_Path = pDatasetName.WorkspaceName.PathName
        Data_Name = pDatasetName.Name
        data_fullname = Data_Path & Data_Name
        If Len(Trim(data_fullname)) = 0 Then Exit Sub 'user cancelled the action
        txtDEM30.Text = data_fullname
        CmdUndo.Enabled = True
    End Sub

    Private Sub CmdSetGadgeLayer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSetGadgeLayer.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog
        pGxDialog = New GxDialog
        Dim Data_Path As String, Data_Name As String
        Dim data_fullname As String, data_type As Integer
        Dim data_type_name As String
        Dim pGxObject As IEnumGxObject = Nothing

        Dim pFilter As IGxObjectFilter
        pFilter = New GxFilterPointFeatureClasses

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select Gadge Station Layer"
            .ObjectFilter = pFilter
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then Exit Sub

        'get the name of the selected folder
        Dim pGxDataset As IGxDataset
        pGxDataset = pGxObject.Next
        Dim pDatasetName As IDatasetName
        pDatasetName = pGxDataset.DatasetName
        Data_Path = pDatasetName.WorkspaceName.PathName
        Data_Name = pDatasetName.Name
        data_type = pDatasetName.Type

        'Set Data Type Name from Data Type
        If data_type = 4 Then
            data_type_name = " (Shapefile)"
        ElseIf data_type = 5 Then
            data_type_name = " (Shapefile)"
        ElseIf data_type = 12 Then
            data_type_name = " (Raster)"
        ElseIf data_type = 13 Then
            data_type_name = " (Raster)"
        ElseIf data_type = 14 Then
            data_type_name = " (Tin)"
        Else
            data_type_name = " (Cannot Clip)"
        End If

        'pad a backslash to the path if it doesn't have one.
        'If Right(Data_Path, 1) <> "\" Then Data_Path = Data_Path & "\"
        Data_Path = BA_StandardizePathString(Data_Path, True)

        data_fullname = Data_Path & Data_Name & data_type_name
        If Len(Trim(data_fullname)) = 0 Then Exit Sub 'user cancelled the action
        txtGaugeStation.Text = data_fullname

        'read the fields in the attribute table and add to CmboxStationAtt
        Dim pWksFactory As IWorkspaceFactory = New ShapefileWorkspaceFactory
        Dim pFeatWorkspace As IFeatureWorkspace = pWksFactory.OpenFromFile(Data_Path, 0)
        Dim pFeatClass As IFeatureClass = pFeatWorkspace.OpenFeatureClass(Data_Name)

        'get fields
        Dim pFields As IFields = pFeatClass.Fields
        Dim aField As IField
        Dim i As Integer, nfields As Integer, qType As Integer
        Dim naddedfield As Integer

        nfields = pFields.FieldCount
        CmboxStationAtt.Items.Clear()

        naddedfield = 0
        For i = 0 To nfields - 1 'Selects only integers and strings
            aField = pFields.Field(i)
            qType = aField.Type
            If qType <= 1 Or qType = 4 Then
                CmboxStationAtt.Items.Add(aField.Name)
                naddedfield = naddedfield + 1
            End If
        Next

        If naddedfield = 0 Then
            MsgBox("No valid attribute field in the attribute table! Please check data." & vbCrLf & Data_Path & Data_Name)
            GoTo AbandonSub
        End If

        'Area field
        ComboStationArea.Items.Clear()
        ComboStationArea.Items.Add("No data")
        naddedfield = 0
        For i = 0 To nfields - 1 'Selects only numerical data types
            aField = pFields.Field(i)
            qType = aField.Type
            If qType <= 3 Then 'numerical data types
                ComboStationArea.Items.Add(aField.Name)
                naddedfield = naddedfield + 1
            End If
        Next
        ComboStationArea.SelectedIndex = 0

        'set area unit to unknown
        ComboStation_Value.SelectedIndex = 0



AbandonSub:
        aField = Nothing
        pFields = Nothing
        pFeatClass = Nothing
        pFeatWorkspace = Nothing
        pWksFactory = Nothing

        pDatasetName = Nothing
        pGxDataset = Nothing
        pFilter = Nothing
        pGxObject = Nothing
        pGxDialog = Nothing

        CmboxStationAtt.SelectedIndex = 0
        CmdUndo.Enabled = True
    End Sub

    Private Sub CmdSetSNOTEL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSetSNOTEL.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog
        pGxDialog = New GxDialog
        Dim Data_Path As String, Data_Name As String, data_type As Integer
        Dim data_fullname As String
        Dim data_type_name As String
        Dim pGxObject As IEnumGxObject = Nothing

        Dim pFilter As IGxObjectFilter
        pFilter = New GxFilterPointFeatureClasses

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select SNOTEL Layer"
            .ObjectFilter = pFilter
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then Exit Sub

        'get the name of the selected folder
        Dim pGxDataset As IGxDataset
        pGxDataset = pGxObject.Next
        Dim pDatasetName As IDatasetName
        pDatasetName = pGxDataset.DatasetName
        Data_Path = pDatasetName.WorkspaceName.PathName
        Data_Name = pDatasetName.Name
        data_type = pDatasetName.Type

        'Set Data Type Name from Data Type
        If data_type = 4 Then
            data_type_name = " (Shapefile)"
        ElseIf data_type = 5 Then
            data_type_name = " (Shapefile)"
        ElseIf data_type = 12 Then
            data_type_name = " (Raster)"
        ElseIf data_type = 13 Then
            data_type_name = " (Raster)"
        ElseIf data_type = 14 Then
            data_type_name = " (Tin)"
        Else
            data_type_name = " (Cannot Clip)"
        End If

        'pad a backslash to the path if it doesn't have one.
        'If Right(Data_Path, 1) <> "\" Then Data_Path = Data_Path & "\"
        Data_Path = BA_StandardizePathString(Data_Path, True)

        data_fullname = Data_Path & Data_Name & data_type_name
        If Len(Trim(data_fullname)) = 0 Then Exit Sub 'user cancelled the action
        txtSNOTEL.Text = data_fullname

        'read the fields in the attribute table and add to CmboxStationAtt
        Dim pWksFactory As IWorkspaceFactory
        pWksFactory = New ShapefileWorkspaceFactory
        Dim pFeatWorkspace As IFeatureWorkspace

        pFeatWorkspace = pWksFactory.OpenFromFile(Data_Path, 0)
        Dim pFeatClass As IFeatureClass
        pFeatClass = pFeatWorkspace.OpenFeatureClass(Data_Name)

        'get fields
        Dim pFields As IFields
        Dim aField As IField
        Dim i As Integer, nfields As Integer, qType As Integer
        pFields = pFeatClass.Fields
        nfields = pFields.FieldCount

        'elevation field
        ComboSNOTEL_Elevation.Items.Clear()
        For i = 0 To nfields - 1 'Selects only numerical data types
            aField = pFields.Field(i)
            qType = aField.Type
            If qType <= 3 Then 'numerical data types
                ComboSNOTEL_Elevation.Items.Add(aField.Name)
            End If
        Next

        'name field
        ComboSNOTEL_Name.Items.Clear()
        ComboSNOTEL_Name.Items.Add("None")
        For i = 1 To nfields 'Selects only string data types
            aField = pFields.Field(i - 1)
            qType = aField.Type
            If qType = 4 Then 'string data types
                ComboSNOTEL_Name.Items.Add(aField.Name)
            End If
        Next

        aField = Nothing
        pFields = Nothing
        pFeatClass = Nothing
        pFeatWorkspace = Nothing
        pWksFactory = Nothing

        pDatasetName = Nothing
        pGxDataset = Nothing
        pFilter = Nothing
        pGxObject = Nothing
        pGxDialog = Nothing

        ComboSNOTEL_Name.SelectedIndex = 0
        ComboSNOTEL_Elevation.SelectedIndex = 0
        CmdUndo.Enabled = True
    End Sub

    Private Sub CmdSetSnowC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSetSnowC.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog
        pGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim Data_Path As String, Data_Name As String, data_type As Integer
        Dim data_fullname As String
        Dim data_type_name As String

        Dim pFilter As IGxObjectFilter = New GxFilterPointFeatureClasses

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select Snow Course Layer"
            .ObjectFilter = pFilter
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then Exit Sub

        'get the name of the selected folder
        Dim pGxDataset As IGxDataset
        pGxDataset = pGxObject.Next
        Dim pDatasetName As IDatasetName
        pDatasetName = pGxDataset.DatasetName
        Data_Path = pDatasetName.WorkspaceName.PathName
        Data_Name = pDatasetName.Name
        data_type = pDatasetName.Type

        'Set Data Type Name from Data Type
        If data_type = 4 Then
            data_type_name = " (Shapefile)"
        ElseIf data_type = 5 Then
            data_type_name = " (Shapefile)"
        ElseIf data_type = 12 Then
            data_type_name = " (Raster)"
        ElseIf data_type = 13 Then
            data_type_name = " (Raster)"
        ElseIf data_type = 14 Then
            data_type_name = " (Tin)"
        Else
            data_type_name = " (Cannot Clip)"
        End If

        'pad a backslash to the path if it doesn't have one.
        'If Right(Data_Path, 1) <> "\" Then Data_Path = Data_Path & "\"
        Data_Path = BA_StandardizePathString(Data_Path, True)

        data_fullname = Data_Path & Data_Name & data_type_name
        If Len(Trim(data_fullname)) = 0 Then Exit Sub 'user cancelled the action
        txtSnowCourse.Text = data_fullname

        'read the fields in the attribute table and add to CmboxStationAtt
        Dim pWksFactory As IWorkspaceFactory
        pWksFactory = New ShapefileWorkspaceFactory
        Dim pFeatWorkspace As IFeatureWorkspace
        pFeatWorkspace = pWksFactory.OpenFromFile(Data_Path, 0)
        Dim pFeatClass As IFeatureClass
        pFeatClass = pFeatWorkspace.OpenFeatureClass(Data_Name)

        'get fields
        Dim pFields As IFields
        Dim aField As IField
        Dim i As Integer, nfields As Integer, qType As Integer
        pFields = pFeatClass.Fields
        nfields = pFields.FieldCount

        'elevation field
        ComboSC_Elevation.Items.Clear()
        For i = 0 To nfields - 1 'Selects only numerical data types
            aField = pFields.Field(i)
            qType = aField.Type
            If qType <= 3 Then 'numerical data types
                ComboSC_Elevation.Items.Add(aField.Name)
            End If
        Next

        'name field
        ComboSC_Name.Items.Clear()
        ComboSC_Name.Items.Add("None")
        For i = 1 To nfields 'Selects only string data types
            aField = pFields.Field(i - 1)
            qType = aField.Type
            If qType = 4 Then 'string data types
                ComboSC_Name.Items.Add(aField.Name)
            End If
        Next

        aField = Nothing
        pFields = Nothing
        pFeatClass = Nothing
        pFeatWorkspace = Nothing
        pWksFactory = Nothing

        pDatasetName = Nothing
        pGxDataset = Nothing
        pFilter = Nothing
        pGxObject = Nothing
        pGxDialog = Nothing

        ComboSC_Name.SelectedIndex = 0
        ComboSC_Elevation.SelectedIndex = 0
        CmdUndo.Enabled = True
    End Sub

    Private Sub CmdSetPrecip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSetPrecip.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog
        pGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim Data_Path As String = ""

        Dim pFilter As IGxObjectFilter
        pFilter = New GxFilterContainers

        Try
            'initialize and open mini browser
            With pGxDialog
                .AllowMultiSelect = False
                .ButtonCaption = "Select"
                .Title = "Select PRISM Data Folder"
                .ObjectFilter = pFilter
                bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
            End With

            If bObjectSelected = False Then Exit Sub

            'get the name of the selected folder
            Dim pGxDataFolder As IGxFile
            pGxDataFolder = pGxObject.Next
            Data_Path = pGxDataFolder.Path
            If Len(Trim(Data_Path)) = 0 Then Exit Sub 'user cancelled the action

            Dim TempPathName As String
            TempPathName = Data_Path & "\Q4\grid"

            If Not BA_Workspace_Exists(TempPathName) Then
                MsgBox(Data_Path & " is not a valid PRISM data folder!")
                txtPRISM.Text = ""
            Else
                txtPRISM.Text = Data_Path
            End If

            CmdUndo.Enabled = True
        Catch
            MsgBox("Please select a folder containing PRISM data!" & vbCrLf & Data_Path & " is not a valid PRISM data folder!")
        End Try
    End Sub


    Private Sub CmdDisplayReferenceLayers_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdDisplayReferenceLayers.Click
        'check if pourpoint file exists
        Dim ppointpath As String = "Please Return"
        Dim layertype As String = ""
        Dim pourpointRef As String = BA_SystemSettings.PourPointLayer
        Dim pplayername As String = BA_GetBareNameAndExtension(pourpointRef, ppointpath, layertype)
        pourpointRef = ppointpath & pplayername
        If Len(pourpointRef) > 0 Then 'it's OK to not have a specified reference layer
            If Not BA_Shapefile_Exists(pourpointRef) Then
                MsgBox("Pourpoint layer does not exist: " & pourpointRef)
                pourpointRef = ""
            End If
        End If

        BA_LoadReferenceLayers(txtTerrain.Text, txtDrainage.Text, txtWatershed.Text, pourpointRef)
        'Dim SaveAOIMXDButton = AddIn.FromID(Of BtnSaveAOIMXD)(My.ThisAddIn.IDs.BtnSaveAOIMXD)
        'Dim BasinInfoButton = AddIn.FromID(Of BtnBasinInfo)(My.ThisAddIn.IDs.BtnBasinInfo)
        'SaveAOIMXDButton.selectedProperty = True
        'BasinInfoButton.selectedProperty = True
    End Sub
    Private Sub txtTerrain_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTerrain.DoubleClick
        txtTerrain.Text = ""
        CmdUndo.Enabled = True
    End Sub
    Private Sub txtDrainage_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDrainage.DoubleClick
        txtDrainage.Text = ""
        CmdUndo.Enabled = True
    End Sub
    Private Sub txtWatershed_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtWatershed.DoubleClick
        txtWatershed.Text = ""
        CmdUndo.Enabled = True
    End Sub


    Private Sub CmboxStationAtt_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmboxStationAtt.SelectedIndexChanged
        CmdUndo.Enabled = True
    End Sub
    Private Sub ComboStationArea_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboStationArea.SelectedIndexChanged
        CmdUndo.Enabled = True
    End Sub
    Private Sub ComboStation_Value_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboStation_Value.SelectedIndexChanged
        CmdUndo.Enabled = True
    End Sub
    Private Sub ComboSNOTEL_Elevation_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboSNOTEL_Elevation.SelectedIndexChanged
        CmdUndo.Enabled = True
    End Sub
    Private Sub ComboSNOTEL_Name_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboSNOTEL_Name.SelectedIndexChanged
        CmdUndo.Enabled = True
    End Sub
    Private Sub ComboSC_Elevation_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboSC_Elevation.SelectedIndexChanged
        CmdUndo.Enabled = True
    End Sub
    Private Sub ComboSC_Name_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboSC_Name.SelectedIndexChanged
        CmdUndo.Enabled = True
    End Sub


    Private Sub Opt10M_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Opt10M.Click
        CmdUndo.Enabled = True
    End Sub
    Private Sub Opt30M_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Opt30M.Click
        CmdUndo.Enabled = True
    End Sub
    Private Sub OptMeter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptMeter.Click
        CmdUndo.Enabled = True
    End Sub
    Private Sub OptFoot_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptFoot.Click
        CmdUndo.Enabled = True
    End Sub
    Private Sub OptSTMeter_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptSTMeter.Click
        CmdUndo.Enabled = True
    End Sub
    Private Sub OptSTFoot_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptSTFoot.Click
        CmdUndo.Enabled = True
    End Sub
    Private Sub OptSCMeter_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptSCMeter.Click
        CmdUndo.Enabled = True
    End Sub
    Private Sub OptSCFoot_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptSCFoot.CheckedChanged
        CmdUndo.Enabled = True
    End Sub

    Private Sub lstLayers_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstLayers.Click
        If lstLayers.SelectedIndex >= 0 Then CmdRemoveLayer.Enabled = True
    End Sub

    Private Sub CmdAddLayer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdAddLayer.Click
        Dim bObjectSelected As Boolean
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim Data_Path As String, Data_Name As String, data_type As Object, data_type_name As String
        Dim data_fullname As String

        Dim pFilter As IGxObjectFilter
        pFilter = New GxFilterDatasets

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select GIS Layers"
            .ObjectFilter = pFilter
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then Exit Sub

        'get the name of the selected folder
        Dim pGxDataset As IGxDataset
        pGxDataset = pGxObject.Next
        Dim pDatasetName As IDatasetName
        pDatasetName = pGxDataset.DatasetName
        Data_Path = pDatasetName.WorkspaceName.PathName
        Data_Name = pDatasetName.Name
        data_type = pDatasetName.Type

        'Set Data Type Name from Data Type
        If data_type = 4 Then
            data_type_name = " (Shapefile)"
        ElseIf data_type = 5 Then
            data_type_name = " (Shapefile)"
        ElseIf data_type = 12 Then
            data_type_name = " (Raster)"
        ElseIf data_type = 13 Then
            data_type_name = " (Raster)"
        Else
            data_type_name = " (Cannot Clip)"
        End If

        'pad a backslash to the path if it doesn't have one.
        'If Right(Data_Path, 1) <> "\" Then Data_Path = Data_Path & "\"
        Data_Path = BA_StandardizePathString(Data_Path, True)

        data_fullname = Data_Path & Data_Name & data_type_name
        If Len(Trim(data_fullname)) = 0 Then Exit Sub 'user cancelled the action
        lstLayers.Items.Add(data_fullname)
        lstLayers.SelectedIndex = lstLayers.Items.Count - 1
        CmdRemoveLayer.Enabled = True
        CmdUndo.Enabled = True
    End Sub
    Private Sub CmdRemoveLayer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdRemoveLayer.Click
        lstLayers.Items.Remove(lstLayers.SelectedItem)
        If lstLayers.Items.Count = 0 Then
            CmdRemoveLayer.Enabled = False
        Else
            lstLayers.SelectedItem = lstLayers.Items.Count - 1
            CmdUndo.Enabled = True
        End If
    End Sub
    Private Sub CmdClearAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClearAll.Click
        lstLayers.Items.Clear()
        CmdUndo.Enabled = True
    End Sub
    Private Sub CmdSaveSettings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSaveSettings.Click
        SaveSettingsButton()
    End Sub
    Public Sub SaveSettingsButton()
        Dim missingdata As Boolean = False
        Dim notForBasinAnalysis As Boolean = Me.ChkboxAOIOnly.Checked

        'check for required fields
        If Len(Me.txtDEM10.Text) = 0 Then missingdata = True
        If Len(Me.txtDEM30.Text) = 0 Then missingdata = True
        If Len(Me.txtGaugeStation.Text) = 0 Then missingdata = True

        If Not notForBasinAnalysis Then
            If Len(Me.txtSNOTEL.Text) = 0 Then missingdata = True
            If Len(Me.txtSnowCourse.Text) = 0 Then missingdata = True
            If Len(Me.txtPRISM.Text) = 0 Then missingdata = True
        End If

        'MsgBox(Len(Me.txtTerrain.Text))
        If missingdata Then
            MsgBox("Missing information on the required fields!")
            Exit Sub
        End If

        Dim demProjText As String = BA_GetProjectionString(Me.txtDEM30.Text)
        Dim projectionsToCheck = New SortedList
        projectionsToCheck.Add("Snotel", Me.txtSNOTEL.Text)
        projectionsToCheck.Add("Snow Course", Me.txtSnowCourse.Text)
        Dim mismatchList As New SortedList
        For Each key As String In projectionsToCheck.Keys
            Dim projPath As String = projectionsToCheck(key)
            Dim parentPath As String = "Please Return"
            'Although this is an optional argument, it's the only to strip the file type from the text field
            Dim tempExt As String = "tempExt"
            Dim fileName As String = BA_GetBareNameAndExtension(projPath, parentPath, tempExt)
            Dim layerType As WorkspaceType = BA_GetWorkspaceTypeFromPath(parentPath)
            If layerType = WorkspaceType.Raster And tempExt = "(Shapefile)" Then
                fileName = BA_StandardizeShapefileName(fileName, True)
            End If
            Dim projString As String = BA_GetProjectionString(parentPath & fileName)
            If demProjText <> projString Then
                mismatchList.Add(key, projString)
            End If
        Next
        ' One or more of the projections didn't match
        If mismatchList.Count > 0 Then
            Dim sb As StringBuilder = New StringBuilder
            sb.Append("Warning: One or more layers is in a different projection from the 30m DEM layer." & vbCrLf)
            sb.Append("30m DEM projection = " & demProjText & vbCrLf)
            For Each key As String In mismatchList.Keys
                sb.Append(key & " = " & mismatchList(key) & vbCrLf)
            Next
            sb.Append(vbCrLf)
            sb.Append("Do you still wish to save the settings ?")
            Dim res As DialogResult = MessageBox.Show(sb.ToString, "Projection mismatch", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If res <> Windows.Forms.DialogResult.Yes Then
                Exit Sub
            End If
        End If

        'MsgBox("Update variables")
        Update_BA_SystemSettings()

        Dim saveSuccess As BA_ReturnCode = BA_Save_Settings()
        If saveSuccess <> BA_ReturnCode.Success Then
            MsgBox("Cannot save to the basinanalyst.def file in " & BA_Settings_Filepath & "! Please contact your system administrator.")
        Else
            MsgBox("Basin Analyst settings are saved to the definition file!")
            'enable btnAddRefLayers
            CmdUndo.Enabled = False
        End If
    End Sub
    'get the name of the layers listed in the lstLayers and send them to an array for using in frmCreateAOI class
    'Private Sub UpdatelstLayersItemNames()
    '    Dim strArray(lstLayers.Items.Count - 1) As String
    '    For i = 0 To lstLayers.Items.Count - 1
    '        strArray(i) = lstLayers.Items.Item(i)
    '        frmSettingsListLayeritem = strArray
    '        'MsgBox(frmSettingsListLayeritem(i))
    '    Next
    'End Sub
    'Pass the index of comboboxes in the frmSetting into public integer variables for using in frmCreateAOI class
    'Private Sub UpdatefrmSettingsComboIndices()
    '    PourNameFieldIndex = CmboxStationAtt.SelectedIndex
    '    SNOTEL_NameFieldIndex = ComboSNOTEL_Name.SelectedIndex
    '    SCourse_NameFieldIndex = ComboSC_Name.SelectedIndex
    'End Sub

    Private Sub CmdClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdClose.Click
        Dim response As Integer
        If CmdUndo.Enabled Then 'user exits the dialog window without saving the change
            response = MsgBox("Save changes?", vbYesNo, "Exit Setting")
            If response = vbYes Then
                SaveSettingsButton()
            Else
                'BA_Read_Settings() 'reload the setting
                Me.Close()
            End If
        End If
        Me.Close()
    End Sub

    Private Sub Update_BA_SystemSettings()
        With BA_SystemSettings
            .GenerateAOIOnly = Me.ChkboxAOIOnly.Checked
            .Status = 1

            .Ref_Terrain = Trim(Me.txtTerrain.Text)
            .Ref_Drainage = Trim(Me.txtDrainage.Text)
            .Ref_Watershed = Trim(Me.txtWatershed.Text)

            'the following are required fields
            .DEM10M = Trim(Me.txtDEM10.Text)
            .DEM30M = Trim(Me.txtDEM30.Text)
            .DEM10MPreferred = Me.Opt10M.Checked
            .DEM_ZUnit_IsMeter = Me.OptMeter.Checked

            .PourPointLayer = Trim(Me.txtGaugeStation.Text)
            .PourPointField = Me.CmboxStationAtt.SelectedItem.ToString
            .PourAreaField = Me.ComboStationArea.SelectedItem.ToString
            .PourAreaUnit = Me.ComboStation_Value.SelectedItem.ToString

            .SNOTELLayer = Trim(Me.txtSNOTEL.Text)
            .SNOTEL_ElevationField = Me.ComboSNOTEL_Elevation.SelectedItem.ToString
            .SNOTEL_NameField = Me.ComboSNOTEL_Name.SelectedItem.ToString
            .SNOTEL_ZUnit_IsMeter = Me.OptSTMeter.Checked

            .SCourseLayer = Trim(Me.txtSnowCourse.Text)
            .SCourse_ElevationField = Me.ComboSC_Elevation.SelectedItem.ToString
            .SCourse_NameField = Me.ComboSC_Name.SelectedItem.ToString
            .SCourse_ZUnit_IsMeter = Me.OptSCMeter.Checked

            .PRISMFolder = Trim(Me.txtPRISM.Text)
            'If lstLayers.Items.Count = 0 Then
            '.listCount = Nothing
            ' Else
            .listCount = lstLayers.Items.Count
            ' End If

            'other layers
            Dim layerNo As Integer = lstLayers.Items.Count
            If layerNo > 0 Then
                ReDim .OtherLayers(layerNo)
                For i As Integer = 0 To layerNo - 1
                    .OtherLayers(i) = lstLayers.Items(i)
                Next
            End If

        End With
    End Sub

    'we do not need this part because we have a similar block that works the same as this
    'Private Sub Display_BA_SystemSettings()
    '    With BA_SystemSettings
    '        .Ref_Terrain = Trim(Me.txtTerrain.Text)
    '        .Ref_Drainage = Trim(Me.txtDrainage.Text)
    '        .Ref_Watershed = Trim(Me.txtWatershed.Text)

    '        'the following are required fields
    '        '.DEM10M = (BA_SystemSettings.DEM10M)
    '        '.DEM30M = (BA_SystemSettings.DEM30M)

    '        .DEM10M = Trim(Me.txtDEM10.Text)
    '        .DEM30M = Trim(Me.txtDEM30.Text)

    '        If .DEM10MPreferred Then
    '            Me.Opt10M.Checked = True
    '        Else
    '            Me.Opt30M.Checked = True
    '        End If

    '        .DEM_ZUnit_IsMeter = Me.OptMeter.Checked

    '        .PourPointLayer = Trim(Me.txtGaugeStation.Text)
    '        .PourPointField = Me.CmboxStationAtt.SelectedItem.ToString
    '        .PourAreaField = Me.ComboStationArea.SelectedItem.ToString
    '        .PourAreaUnit = Me.ComboStation_Value.SelectedItem.ToString

    '        .SNOTELLayer = Trim(Me.txtSNOTEL.Text)
    '        .SNOTEL_ElevationField = Me.ComboSNOTEL_Elevation.SelectedIndex.ToString
    '        .SNOTEL_NameField = Me.ComboSC_Name.SelectedItem.ToString
    '        '.SNOTEL_ZUnit_IsMeter = Me.OptSTMeter.Checked
    '        If .DEM_ZUnit_IsMeter Then
    '            Me.OptSTMeter.Checked = True
    '        End If

    '        .SCourseLayer = Trim(Me.txtSnowCourse.Text)
    '        .SCourse_ElevationField = Me.ComboSC_Elevation.SelectedItem.ToString
    '        .SCourse_NameField = Me.ComboSC_Name.SelectedItem.ToString
    '        '.SCourse_ZUnit_IsMeter = Me.OptSCMeter.Checked
    '        If .SCourse_ZUnit_IsMeter Then
    '            Me.OptSCMeter.Checked = True
    '        End If

    '        .PRISMFolder = Trim(Me.txtPRISM.Text)

    '        'other layers
    '        Dim layerNo As Integer = Me.lstLayers.Items.Count
    '        If layerNo > 0 Then
    '            ReDim .OtherLayers(layerNo)
    '            For i As Integer = 0 To layerNo - 1
    '                .OtherLayers(i) = Me.lstLayers.Items(i)
    '            Next
    '        End If

    '    End With
    'End Sub

    Private Sub CmdUndo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdUndo.Click
        BA_Read_Settings(Me)
        CmdUndo.Enabled = False
    End Sub

    Private Sub frmSettings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim settings_message As String
        BA_SetSettingPath() 'set the BA_Settings_Filepath global variable
        Me.ComboStation_Value.Items.Clear()
        Me.ComboStation_Value.Items.Add("Unknown")
        Me.ComboStation_Value.Items.Add("Sq. Km")
        Me.ComboStation_Value.Items.Add("Acre")
        Me.ComboStation_Value.Items.Add("Sq. Miles")
        Me.ComboStation_Value.SelectedIndex = 0

        Me.ComboSNOTEL_Name.Items.Clear()
        Me.ComboSNOTEL_Name.Items.Add("none")
        Me.ComboSNOTEL_Name.SelectedIndex = 0

        'reset Snow courses name field combobox
        Me.ComboSC_Name.Items.Clear()
        Me.ComboSC_Name.Items.Add("none")
        Me.ComboSC_Name.SelectedIndex = 0

        'set settings
        settings_message = BA_Read_Settings(Me)
        Dim cboSelectBasin = AddIn.FromID(Of cboTargetedBasin)(My.ThisAddIn.IDs.cboTargetedBasin)
        If Len(settings_message) > 0 Then
            MsgBox(settings_message)
            'SelectBasin_Flag = False                                      
            cboSelectBasin.selectedProperty = False

            If settings_message.Substring(0, 7) = "Version" Then
                MsgBox("Please update Settings file using save settings in Settings form.", vbOKOnly, "BAGIS Settings Version Error")
            End If
            If settings_message.Substring(0, 6) = "ERROR!" Then
                MsgBox("Please set and save the data layer settings first!")
            End If
        End If

        CmdUndo.Enabled = False
        Me.Text = Me.Text & " " & BA_Settings_Filepath & "\" & BA_Settings_Filename

        'enable btnAddreferencelayers 
        'Dim AddRefLayersButton = AddIn.FromID(Of BtnAddRefLayers)(My.ThisAddIn.IDs.BtnAddRefLayers)
        'If Not String.IsNullOrEmpty(BA_SystemSettings.Ref_Drainage) And _
        '    Not String.IsNullOrEmpty(BA_SystemSettings.Ref_Watershed) And _
        '    Not String.IsNullOrEmpty(BA_SystemSettings.Ref_Terrain) Then
        '    AddRefLayersButton.selectedProperty = True
        'End If

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub ChkboxAOIOnly_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles ChkboxAOIOnly.CheckedChanged
        Dim toggleStatus As Boolean = Me.ChkboxAOIOnly.Checked
        ToggleAOICreationOption(toggleStatus)
        CmdUndo.Enabled = True
    End Sub

    Private Sub ToggleAOICreationOption(ByVal status As Boolean)
        Dim not_Status As Boolean = Not status
        'if status is TRUE, then disable all none essential parameters for AOI creation
        lblSNOTEL.Enabled = not_Status
        lblSnowCourse.Enabled = not_Status
        lblPRISM.Enabled = not_Status

        Static SNOTELtxt As String
        Static SCtxt As String
        Static PRISMtxt As String

        If Trim(txtSNOTEL.Text) = "" Then
            txtSNOTEL.Text = SNOTELtxt
        Else
            SNOTELtxt = txtSNOTEL.Text
            txtSNOTEL.Text = ""
        End If

        If Trim(txtSnowCourse.Text) = "" Then
            txtSnowCourse.Text = SCtxt
        Else
            SCtxt = txtSnowCourse.Text
            txtSnowCourse.Text = ""
        End If

        If Trim(txtPRISM.Text) = "" Then
            txtPRISM.Text = PRISMtxt
        Else
            PRISMtxt = txtPRISM.Text
            txtPRISM.Text = ""
        End If

        txtSNOTEL.Enabled = not_Status
        txtSnowCourse.Enabled = not_Status
        txtPRISM.Enabled = not_Status
        CmdSetSNOTEL.Enabled = not_Status
        CmdSetSnowC.Enabled = not_Status
        CmdSetPrecip.Enabled = not_Status

        'hide the followings
        lblElevField.Visible = not_Status
        lblNameField.Visible = not_Status

        ComboSNOTEL_Elevation.Visible = not_Status
        ComboSNOTEL_Name.Visible = not_Status
        lblSNOTELUnit.Visible = not_Status
        GrpBoxSNOTELUnit.Visible = not_Status

        ComboSC_Elevation.Visible = not_Status
        ComboSC_Name.Visible = not_Status
        lblSnowCourseUnit.Visible = not_Status
        GrpBoxSnowCourseUnit.Visible = not_Status
    End Sub
End Class