Imports System.Windows.Forms
Imports System.IO
Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Carto
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Desktop.AddIns
Imports ESRI.ArcGIS.DataSourcesGDB
Imports ESRI.ArcGIS.DataSourcesFile
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework


Public Class frmPourPoint

    Private Sub CmdOverview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdOverview.Click
        'zoom to basin extent
        Dim pMxDoc As IMxDocument = My.ArcMap.Document
        Dim pMap As IMap = pMxDoc.FocusMap

        Dim Buffer_Factor As Double
        Buffer_Factor = 1.2
        'create a buffer around the AOI
        Dim pEnv As IEnvelope = New Envelope

        Dim llx As Double, lly As Double, urx As Double, ury As Double
        Dim xrange As Double, yrange As Double
        Dim xoffset As Double, yoffset As Double

        Dim pBasinEnvelope As IEnvelope
        If Len(BasinFolderBase) = 0 Then
            MsgBox("Please select a basin before you create an AOI!")
            Exit Sub
        End If

        pBasinEnvelope = BA_GetBasinEnvelope(BasinFolderBase)

        pBasinEnvelope.QueryCoords(llx, lly, urx, ury)
        xrange = urx - llx
        yrange = ury - lly
        xoffset = xrange * (Buffer_Factor - 1) / 2
        yoffset = yrange * (Buffer_Factor - 1) / 2
        llx = llx - xoffset
        lly = lly - yoffset
        urx = urx + xoffset
        ury = ury + yoffset
        pEnv.PutCoords(llx, lly, urx, ury)

        'pFLayer.AreaOfInterest.PutCoords llx, lly, urx, ury
        Dim pActiveView As IActiveView = pMxDoc.ActiveView
        pActiveView.Extent = pEnv

        'refresh the active view
        pMxDoc.ActiveView.Refresh()
    End Sub
    Private Has_TempPourpoint As Boolean
    Private Sub CmdNewPoint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdNewPoint.Click
        Dim messagetext As String
        Dim nametext As String
        Dim npoints As Long

        messagetext = "Please enter a temporary name of the pourpoint." & vbCrLf & "No update will be made on the gauge station layer!"
        nametext = InputBox(messagetext)

        'verify the entries in the list
        npoints = lstPPoints.Items.Count
        If npoints = 0 Then Has_TempPourpoint = False

        'temp name is always the last entry on the list
        If Len(Trim(nametext)) > 0 Then
            If Has_TempPourpoint Then 'remove the temp pourpoint name from the list
                lstPPoints.Items.RemoveAt(npoints - 1)
            End If
            Has_TempPourpoint = True
            'DoEvents()

            lstPPoints.Items.Add(nametext & " (Temporary)")
            'lstPPoints.ListIndex = lstPPoints.listcount - 1 'point to the newly added point name
            lstPPoints.SelectedIndex = lstPPoints.Items.Count - 1 'point to the newly added point name
            CmdSelect.Enabled = True
        End If
    End Sub

    Private Sub CmdSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSelect.Click
        'get today's date string in mmddyy format, e.g., 113009
        Dim DateString As String
        Dim MMString As String, DDString As String, YYString As String
        Dim MyDate
        Dim response As Integer

        If lstPPoints.SelectedItems.Count <= 0 Then
            MsgBox("Please add a new pourpoint!")
            Exit Sub
        End If

        MyDate = Date.Now

        MMString = DatePart("m", MyDate)
        If Len(Trim(MMString)) < 2 Then MMString = "0" & MMString 'pad 0 to the single digit month

        DDString = DatePart("d", MyDate)
        If Len(Trim(DDString)) < 2 Then DDString = "0" & DDString 'pad 0 to the single digit day

        YYString = DatePart("yyyy", MyDate) 'use four digit year

        DateString = MMString & DDString & YYString

        Dim AOIName As String = Nothing
        Dim i As Integer = 0, spos As Integer = 0

        'AOIName = Trim(lstPPoints.List(lstPPoints.ListIndex))
        For Each listitem In lstPPoints.SelectedItems
            'For i = 0 To lstPPoints.Items.Count - 1
            AOIName = Trim(lstPPoints.SelectedItem.ToString)
        Next
        Dim File_Path As String = "Please Return"
        Dim File_Name As String = Nothing
        Dim fileType As String = "please return"
        File_Name = BA_GetBareNameAndExtension(BA_SystemSettings.PourPointLayer, File_Path, fileType)

        'Ver1E Update - reset the BA_AOI_Forecast_ID value with the forecast point id
        BA_AOI_Forecast_ID = "Unknown"

        'remove the temporary keyword from the name
        If Microsoft.VisualBasic.Right(AOIName, 11) = "(Temporary)" Then
            AOIName = Trim(Microsoft.VisualBasic.Left(AOIName, Len(AOIName) - 11))
            AOI_ReferenceArea = 0
        Else
            If UCase(BA_SystemSettings.PourAreaField) = "NO DATA" Then
                AOI_ReferenceArea = 0
            Else
                'get the reference area from the attribute table
                AOI_ReferenceArea = CDbl(BA_QueryAttributeTable(File_Path, File_Name, _
                                                                BA_SystemSettings.PourPointField, _
                                                                AOIName, BA_STRING_ATTRIBUTE, BA_SystemSettings.PourAreaField))
                BA_AOI_Forecast_ID = CStr(BA_QueryAttributeTable(File_Path, File_Name, _
                                                                 BA_SystemSettings.PourPointField, _
                                                                 AOIName, BA_STRING_ATTRIBUTE, BA_AOI_IDField))
                If BA_AOI_Forecast_ID = "0" Or BA_AOI_Forecast_ID = "" Then 'invalid id
                    MsgBox("WARNING!!!" & vbCrLf & "The attribute table of the forecast point layer does not contain valid data in " & BA_AOI_IDField & ".")
                End If
            End If
        End If

        'Dim AOINameOut As String = "please return"
        'replace all spaces in the text with underbar
        response = BA_ReplaceCharacters(AOIName, " ", "_", AOIName) 'replace space with under bar
        response = BA_ReplaceCharacters(AOIName, ",", "-", AOIName) 'replace comma with dash
        response = BA_ReplaceCharacters(AOIName, "'", "-", AOIName) 'replace apostrophe with dash
        'AOIName = AOINameOut

        'check the length of aoi folder name, it cannot exceed 80 characters
        If Len(AOIName) >= 71 Then 'the base name is too long
            AOIName = Microsoft.VisualBasic.Left(AOIName, 50)
            MsgBox("AOI folder name is too long and is truncated by the program." & vbCrLf & "ArcInfo doesn't allow the name of any folder to exceed 80 characters.")
        End If

        'pad an underbar before the Datestring if it doesn't already have one
        If Len(DateString) > 0 Then
            If Microsoft.VisualBasic.Right(AOIName, 1) <> "_" And Microsoft.VisualBasic.Left(DateString, 1) <> "_" Then
                AOIName = AOIName & "_" & DateString
            Else
                AOIName = AOIName & DateString
            End If
        End If

ReEnter:
        AOIName = Trim(InputBox("Please enter the name of the AOI:", "AOI Name", AOIName))

        If Len(AOIName) = 0 Then 'user select cancel
            Exit Sub
        End If

        If InStr(AOIName, " ") Then
            MsgBox("Space not allowed in the AOI name!")
            GoTo ReEnter
        End If

        'check the length of aoi folder name, it cannot exceed 80 characters
        If Len(AOIName) >= 80 Then 'the base name is too long
            MsgBox("Name is too long! Please shorten it to less than 80 characters.")
            GoTo ReEnter
        End If

        'check the length of the flow_accumulation path. If the length exceeds 115 char,
        'then Arcinfo (i.e., weasel) cannot use the folder as a workspace.
        'flow_acc path = AOIFolderbase & "\output\surfaces\dem\filled\flow-direction\flow-accumulation"
        'i.e., len(AOIFolderbase) + 60 <= 115 (len(AOIFolderbase) should be shorter than 55
        'also, the total path + filename cannot exceed 128 char
        'to correctly read "\output\surfaces\dem\filled\flow-direction\flow-accumulation\source.weasel", the
        'aoifolderbase cannot exceed 128-73 = 55
        If Len(BasinFolderBase & "\" & AOIName) >= 100 Then 'the aoi folder base name is too long for ArcInfo
            response = MsgBox("WARNING!" & vbCrLf & "The AOI path name is too long (" & Len(BasinFolderBase & "\" & _
            AOIName) - 54 & " character(s) more than the allowed length)!" & vbCrLf & _
            "To make the AOI compatible with Weasel GIS, please make the AOI name shorter." & vbCrLf & _
            "ArcInfo doesn't allow the path string to exceed 128 char and a workspace path to exceed 115 char." & vbCrLf & " " & vbCrLf & _
            "Do you want to re-enter the AOI name?")
            'If response = vbYes Then
            GoTo ReEnter
            'If response = vbNo Then
            'GoTo ReEnter
            'End If
            'End If
        End If

        Me.Hide()
        'check if the aoi folder already exists
        Dim TempAOIFolderName As String
        TempAOIFolderName = BasinFolderBase & "\" & AOIName
        ''''TempAOIFolderName = BasinFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Aoi) & "\" & AOIName

        If BA_Workspace_Exists(TempAOIFolderName) Then
            ''''If BA_File_Exists(TempAOIFolderName, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
            response = MsgBox(AOIName & " folder already exists. Overwrite?", vbYesNo)
            If response = vbYes Then
                'delete the aoi folder
                Dim LayerRemoved As Short = Nothing
                Dim DeleteStatus As Integer
                'clean up the stuff that was created
                'remove layers from the map that are in the AOI folder
                LayerRemoved = BA_RemoveLayersInFolder(My.ArcMap.Document, TempAOIFolderName)
                'LayerRemoved = BA_Remove_ShapefileFromGDB(BasinFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Aoi), AOIName)

                'delete aoi folder
                DeleteStatus = BA_Remove_Folder(TempAOIFolderName)
                If DeleteStatus = 0 Then 'unable to delete the folder
                    MsgBox("Unable to remove the folder. Program stopped.")
                    Exit Sub
                End If
                BA_CreateFolder(BasinFolderBase, AOIName)
            Else
                GoTo ReEnter
            End If
        Else
            BA_CreateFolder(BasinFolderBase, AOIName)
        End If

        'reset aoi tools when a new aoi is selected
        BA_Reset_AOIFlags()

        'SelectAOI_Flag = True 'still allow user to select a different AOI
        Dim selectAOIButton = AddIn.FromID(Of BtnAOI_Tool)(My.ThisAddIn.IDs.BtnAOI_Tool)
        selectAOIButton.selectedProperty = True

        Dim cboSelectedAOI = AddIn.FromID(Of cboTargetedAOI)(My.ThisAddIn.IDs.cboTargetedAOI)
        cboSelectedAOI.selectedProperty = False

        'SaveAOIMXD_Flag = False
        Dim saveAOIMXDButton = AddIn.FromID(Of BtnSaveAOIMXD)(My.ThisAddIn.IDs.BtnSaveAOIMXD)
        saveAOIMXDButton.selectedProperty = False 'I changed it to false 11/16/2012

        'ThisDocument.SelectedAOI.Text = AOIName
        cboSelectedAOI.setValue(AOIName)

        AOIFolderBase = "" 'the selected AOI hasn't been created. This disables all the other AOI related functions.

        'SetPourPoint_Flag = True
        'GenerateAOI_Flag = True
        Dim SetPourPointtool = AddIn.FromID(Of setPourPointtool)(My.ThisAddIn.IDs.setPourPointtool)
        Dim CreatAOIButton = AddIn.FromID(Of BtnCreateAOI)(My.ThisAddIn.IDs.BtnCreateAOI)
        SetPourPointtool.selectedProperty = True
        CreatAOIButton.selectedProperty = True
        MsgBox("Please select a pourpoint location and then create the AOI! ID: " & BA_AOI_Forecast_ID)
    End Sub

    Private Sub CmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdExit.Click
        Me.Close()
    End Sub
    Private Sub Show_PourPoint()
        'zoom to the selected pourpoint location
        'the view extent is determined by the size of the basin, i.e., 1/10 of the basin's long dimension
        Dim pMxDoc As IMxDocument = My.ArcMap.Document
        Dim pMap As IMap = pMxDoc.FocusMap
        Dim GaugeStationLayerName As String
        GaugeStationLayerName = "Gauge Stations"

        Dim GaugeLayerExists As Boolean

        'search gauge layer
        Dim nlayers As Long
        Dim i As Long
        Dim pTempLayer As ILayer
        Dim pFLayer As IFeatureLayer = Nothing
        Dim pFeatClass As IFeatureClass
        Dim pSelectionSet As ISelectionSet

        nlayers = pMap.LayerCount

        GaugeLayerExists = False
        For i = nlayers To 1 Step -1
            pTempLayer = pMap.Layer(i - 1)
            If GaugeStationLayerName = pTempLayer.Name Then
                GaugeLayerExists = True
                pFLayer = pTempLayer
                pFLayer.Visible = True
            End If
        Next

        If Not GaugeLayerExists Then Exit Sub 'the gauge layer is not available

        'use the attribute query to find the selected gauge station
        'the quage station name has to be unique
        Dim pQFilter As IQueryFilter
        pQFilter = New QueryFilter

        Dim QueryStatement As String = Nothing
        Dim StationName As String
        Dim NameField As String

        Dim pScratchWorkspace As IWorkspace
        Dim pScratchWorkspaceFactory As IScratchWorkspaceFactory
        pScratchWorkspaceFactory = New ScratchWorkspaceFactory
        pScratchWorkspace = pScratchWorkspaceFactory.DefaultScratchWorkspace

        ''''StationName = lstPPoints.List(lstPPoints.ListIndex)     p
        StationName = lstPPoints.SelectedItem
        ' NameField = frmSettings.CmboxStationAtt.List(frmSettings.CmboxStationAtt.ListIndex)
        NameField = BA_SystemSettings.PourPointField

        pFeatClass = pFLayer.FeatureClass

        Dim FieldSelected As Integer
        FieldSelected = pFeatClass.FindField(NameField)
        Dim pSFields As IFields
        pSFields = pFeatClass.Fields
        Dim pSField As IField
        pSField = pSFields.Field(FieldSelected)
        Dim pType As Integer
        pType = pSField.Type

        Dim Datatype_Supported As Boolean

        Datatype_Supported = True

        'check StationName for special character - apostrophe
        Dim NewString As String = Nothing, response As Integer
        If InStr(1, StationName, "'", vbTextCompare) > 0 Then 'apostrophe exists in the text
            response = BA_ReplaceCharacters(StationName, "'", "=", NewString) 'replace space with under bar
            response = BA_ReplaceCharacters(NewString, "=", "''", StationName) 'replace space with under bar
        End If

        If pType <= 3 Then 'numerical type
            QueryStatement = NameField & " = " & StationName
        ElseIf pType = 4 Then
            QueryStatement = NameField & " = '" & StationName & "'"
        Else 'unrecognized field type
            Datatype_Supported = False
        End If

        Dim pFSelection As IFeatureSelection

        If Datatype_Supported Then
            pQFilter.WhereClause = QueryStatement

            'create a selection set
            pSelectionSet = pFeatClass.Select(pQFilter, esriSelectionType.esriSelectionTypeHybrid, esriSelectionOption.esriSelectionOptionOnlyOne, pScratchWorkspace)
            pFSelection = pFLayer
            pFSelection.SelectionSet = pSelectionSet

            'pMxDoc.ActiveView.PartialRefresh esriViewGeoSelection + esriViewGeography, Nothing, Nothing
            pMxDoc.ActiveView.Refresh()
        End If

        pScratchWorkspaceFactory = Nothing
        pScratchWorkspace = Nothing
        pFeatClass = Nothing
        pFSelection = Nothing
        pSelectionSet = Nothing
        pFLayer = Nothing
        pSField = Nothing
        pSFields = Nothing
        pQFilter = Nothing

ErrorHandler:
    End Sub
    Private Sub lstPPoints_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstPPoints.SelectedIndexChanged
        'DoEvents()
        If lstPPoints.SelectedIndex <> lstPPoints.Items.Count - 1 Then
            Show_PourPoint()
        Else
            If Not Has_TempPourpoint Then
                Show_PourPoint()
            End If
        End If
    End Sub

    Private Sub frmPourPoint_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Has_TempPourpoint = False
        CmdSelect.Enabled = False

        'load flow accumulation raster as a reference
        Dim response As Integer
        'Dim filePath As String = "Please Return"
        'Dim LayerName As String = BA_GetPath(BasinFolderBase & "\" & _
        '                                     BA_EnumDescription(GeodatabaseNames.Surfaces) _
        '                                     & "\" & BA_EnumDescription(MapsFileName.flow_accumulation_gdb), filePath)
        response = BA_DisplayRaster(My.ArcMap.Application, BasinFolderBase & "\" & _
                                    BA_EnumDescription(GeodatabaseNames.Surfaces) & "\" & _
                                    BA_EnumDescription(MapsFileName.flow_accumulation_gdb))

        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(My.ArcMap.Application.hWnd, 20)
        Dim progressDialog2 As IProgressDialog2 = Nothing
        progressDialog2 = BA_GetProgressDialog(pStepProg, "Displaying Flow Accumulation", "Displaying...")
        pStepProg.Show()
        progressDialog2.ShowDialog()
        pStepProg.Step()

        'load gauge station layer if it doesn't already exist
        Dim File_Path As String = "Please Return"
        Dim File_Name As String
        Dim GaugeStationLayerName As String
        Dim messagetext As String
        Dim nametext As String
        Dim layertype As String = "please return"

        Dim pFeature As IFeature
        Dim LoadGaugeLayer As Boolean
        GaugeStationLayerName = "Gauge Stations"
        Dim pMxDoc As IMxDocument = My.ArcMap.Document
        Dim pMap As IMap = pMxDoc.FocusMap

        'if a layer with the assigned name exists, then don't add the gauge station layer
        'search layer of the specified name, if found
        Dim nlayers As Short
        Dim i As Short
        Dim pTempLayer As ILayer
        Dim pFLayer As IFeatureLayer
        Dim pGeo As IGeometry
        Dim pFeatCursor As IFeatureCursor
        Dim pFilter As ISpatialFilter
        Dim pField As IFields
        Dim pWksFactory As IWorkspaceFactory
        Dim pFeatWorkspace As IFeatureWorkspace
        Dim pFeatClass As IFeatureClass

        nlayers = pMap.LayerCount
        LoadGaugeLayer = True

        On Error GoTo ErrorHandlerGaugeLayerInvalid
        For i = nlayers To 1 Step -1
            pTempLayer = pMap.Layer(i - 1)
            If GaugeStationLayerName = pTempLayer.Name Then
                LoadGaugeLayer = False
                pFLayer = pTempLayer
                pFLayer.Visible = True
                'refresh the active view
                pMxDoc.ActiveView.PartialRefresh(2, pFLayer, Nothing) 'esriViewGeography
                pMxDoc.UpdateContents()
            End If
        Next

        ''''' File_Name = BA_GetBareNameAndExtension(frmSettings.txtGaugeStation.Text, File_Path, layertype)
        File_Name = BA_GetBareNameAndExtension(BA_SystemSettings.PourPointLayer, File_Path, layertype)

        'exit if file_name is null
        If Len(File_Name) = 0 Then GoTo ErrorHandlerGaugeLayerInvalid

        'open the gauge station layer
        pWksFactory = New ShapefileWorkspaceFactory
        pFeatWorkspace = pWksFactory.OpenFromFile(File_Path, 0)
        pFeatClass = pFeatWorkspace.OpenFeatureClass(File_Name)

        'add featureclass to current data frame
        If LoadGaugeLayer Then 'gauge station layer hasn't been added to the map frame
            pFLayer = New FeatureLayer
            pFLayer.FeatureClass = pFeatClass
            pFLayer.Name = GaugeStationLayerName

            'check feature geometry type, only polygon layers can be used as an extent layer
            If pFLayer.FeatureClass.ShapeType <> esriGeometryType.esriGeometryPoint Then
                GoTo ErrorHandlerGaugeLayerInvalid
            End If

            'add layer
            'set layer symbology - hollow with red outline
            'Set marker symbol
            Dim pStyleGallery As IStyleGallery = pMxDoc.StyleGallery

            Dim pEnumMarkers As IEnumStyleGalleryItem = pStyleGallery.Items("Marker Symbols", "ESRI.style", "default")
            pEnumMarkers.Reset()

            Dim pStyleItem As IStyleGalleryItem
            pStyleItem = pEnumMarkers.Next

            On Error GoTo ErrorHandlerBasinNotSet
            Dim pMSymbol As IMarkerSymbol = Nothing
            Do Until pStyleItem Is Nothing
                If pStyleItem.Name = "Circle 8" Then
                    pMSymbol = pStyleItem.Item
                    pMSymbol.Size = 18
                    pMSymbol.Color = pDisplayColor
                    Exit Do
                End If
                pStyleItem = pEnumMarkers.Next
            Loop
            Dim pGFLayer As IGeoFeatureLayer
            Dim pRenderer As ISimpleRenderer

            pRenderer = New SimpleRenderer
            pRenderer.Symbol = pMSymbol
            pGFLayer = pFLayer
            pGFLayer.Renderer = pRenderer
            pMap.AddLayer(pFLayer)

            'refresh the active view
            pMxDoc.ActiveView.PartialRefresh(2, Nothing, Nothing) 'esriViewGeography
            pMxDoc.UpdateContents()
        End If
        'create a spatial filter to select only the gauge stations in the AOI boundary
        pGeo = BA_GetShapeGeometry(BasinFolderBase & "\" & BA_EnumDescription(GeodatabaseNames.Aoi) & "\" & BA_AOIExtentCoverage)
        pFilter = New SpatialFilter
        With pFilter
            .Geometry = pGeo
            .GeometryField = "SHAPE"
            .SpatialRel = esriSpatialRelEnum.esriSpatialRelContains
        End With

        pFeatCursor = pFeatClass.Search(pFilter, False)
        pField = pFeatCursor.Fields

        'search for the field that's to be used
        Dim FieldIndex As Long
        Dim TargetFieldName As String
        Dim nFeatures As Long

        'TargetFieldName = frmSettings.CmboxStationAtt.List(frmSettings.CmboxStationAtt.ListIndex)
        TargetFieldName = BA_SystemSettings.PourPointField

        FieldIndex = pField.FindField(TargetFieldName)
        If FieldIndex < 0 Then 'cannot find the specified field
            MsgBox("Error occurred when access the attribute table of the gauge station layer!")
            Exit Sub
        End If

        pFeature = pFeatCursor.NextFeature
        nFeatures = 0

        lstPPoints.Items.Clear()
        Do While Not pFeature Is Nothing
            lstPPoints.Items.Add(pFeature.Value(FieldIndex))
            nFeatures = nFeatures + 1
            pFeature = pFeatCursor.NextFeature
        Loop

        If nFeatures > 0 Then 'at least one gauge station is within the basin
            ''''lstPPoints.ListIndex = 0     p
            lstPPoints.SelectedIndex = 0
            CmdSelect.Enabled = True
        End If
        pStepProg.Hide()
        pStepProg = Nothing
        progressDialog2.HideDialog()
        progressDialog2 = Nothing

        pGeo = Nothing
        pFeature = Nothing
        pFeatClass = Nothing
        pFeatCursor = Nothing
        pFilter = Nothing
        pFeatWorkspace = Nothing
        pWksFactory = Nothing
        pTempLayer = Nothing
        pFLayer = Nothing
        pMxDoc.ActiveView.Refresh()
        Exit Sub

ErrorHandlerGaugeLayerInvalid:
        pGeo = Nothing
        pFeature = Nothing
        pFeatClass = Nothing
        pFeatCursor = Nothing
        pFilter = Nothing
        pFeatWorkspace = Nothing
        pWksFactory = Nothing
        pTempLayer = Nothing
        pFLayer = Nothing
        MsgBox("Gauge station layer specified in the settings is invalid!")
        Me.Close()
        Exit Sub

ErrorHandlerBasinNotSet:
        MsgBox("Please select a basin first!")
        Me.Close()
    End Sub
End Class