Imports System.IO
Imports System.Web.UI
Imports System.Text
Imports BAGIS_ClassLibrary

Public Module HtmlModule

    Public Sub BA_WriteSampleHtmlFile(ByVal filePath As String, ByVal overwrite As Boolean)

        If overwrite = True Then
            If File.Exists(filePath) Then
                File.Delete(filePath)
            End If
        End If
        Dim sb As StringBuilder = New StringBuilder()
        Dim hWriter As HtmlTextWriter = New HtmlTextWriter(New StringWriter(sb))

        'start the table
        hWriter.RenderBeginTag(HtmlTextWriterTag.Table)

        'start the row
        hWriter.RenderBeginTag(HtmlTextWriterTag.Tr)

        'add data
        hWriter.RenderBeginTag(HtmlTextWriterTag.Td)
        hWriter.Write("Name: ")
        hWriter.RenderEndTag()
        hWriter.RenderBeginTag(HtmlTextWriterTag.Td)
        hWriter.Write("Geoffrey")
        hWriter.RenderEndTag()

        'end the row
        hWriter.RenderEndTag()

        'start the row
        hWriter.RenderBeginTag(HtmlTextWriterTag.Tr)

        'add data
        hWriter.RenderBeginTag(HtmlTextWriterTag.Td)
        hWriter.Write("Name: ")
        hWriter.RenderEndTag()
        hWriter.RenderBeginTag(HtmlTextWriterTag.Td)
        hWriter.Write("Lesley")
        hWriter.RenderEndTag()

        'end the row
        hWriter.RenderEndTag()

        'start the row
        hWriter.RenderBeginTag(HtmlTextWriterTag.Tr)

        'add data
        hWriter.RenderBeginTag(HtmlTextWriterTag.Td)
        hWriter.Write("Name: ")
        hWriter.RenderEndTag()
        hWriter.RenderBeginTag(HtmlTextWriterTag.Td)
        hWriter.Write("Masoud")
        hWriter.RenderEndTag()

        'end the row
        hWriter.RenderEndTag()

        'end the table
        hWriter.RenderEndTag()


        Using outfile As New StreamWriter(filePath)
            outfile.Write(sb.ToString())
        End Using


    End Sub

    Public Sub BA_WriteHtmlFile(ByVal pAoi As Aoi, ByVal filePath As String, ByVal overwrite As Boolean)
        Dim aoiName As String = pAoi.Name
        If overwrite = True Then
            If File.Exists(filePath) Then
                File.Delete(filePath)
            End If
        End If
        Dim sb As StringBuilder = New StringBuilder()
        Dim hWriter As HtmlTextWriter = New HtmlTextWriter(New StringWriter(sb))

        Dim hruList As IList(Of Hru) = pAoi.HruList
        Dim pHru As Hru = Nothing
        If hruList IsNot Nothing And hruList.Count > 0 Then
            pHru = hruList.Item(0)
        End If

        'start the page
        hWriter.RenderBeginTag(HtmlTextWriterTag.Html)
        'start the header
        hWriter.RenderBeginTag(HtmlTextWriterTag.Head)
        'start the title
        hWriter.RenderBeginTag(HtmlTextWriterTag.Title)
        hWriter.Write("BAGIS-H HRU Delineation Rule Summary for HRU: " & pHru.Name)
        hWriter.RenderEndTag()
        AddMetaTags(hWriter)
        hWriter.RenderEndTag()
        'start the body
        hWriter.RenderBeginTag(HtmlTextWriterTag.Body)
        'start the high-level div tag and accompanying style
        hWriter.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "14px")
        hWriter.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, "Arial")
        hWriter.AddStyleAttribute(HtmlTextWriterStyle.PaddingLeft, "5px")
        hWriter.RenderBeginTag(HtmlTextWriterTag.Div)
        'start the div tag for the title
        hWriter.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, "Bold")
        hWriter.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "center")
        hWriter.RenderBeginTag(HtmlTextWriterTag.Div)
        hWriter.Write("BAGIS-H HRU Delineation Rule Summary for HRU: " & pHru.Name)
        hWriter.RenderEndTag()

        'start the aoi summary section
        hWriter.RenderBeginTag(HtmlTextWriterTag.P)
        hWriter.Write("AOI: " & aoiName)
        hWriter.WriteBreak()
        hWriter.Write("AOI file path: " & pAoi.FilePath)
        hWriter.WriteBreak()
        hWriter.Write("Application version: " & pAoi.ApplicationVersion)
        hWriter.RenderEndTag()

        'start the main hru section
        AddHruSummary(hWriter, pHru)

        'print main hru section for each parent
        Dim parentHru As Hru = pHru.ParentHru
        While parentHru IsNot Nothing
            AddHruSummary(hWriter, parentHru)
            parentHru = parentHru.ParentHru
        End While

        hWriter.RenderEndTag()
        hWriter.RenderEndTag()
        hWriter.RenderEndTag()
        Using outfile As New StreamWriter(filePath)
            outfile.Write(sb.ToString())
        End Using
    End Sub

    ' Add the summary section for an Hru to the htmlWriter
    Private Sub AddHruSummary(ByRef hwriter As HtmlTextWriter, ByVal pHru As Hru)
        hwriter.RenderBeginTag(HtmlTextWriterTag.P)
        hwriter.RenderBeginTag(HtmlTextWriterTag.B)
        hwriter.Write("HRU: " & pHru.Name)
        hwriter.RenderEndTag()
        hwriter.WriteBreak()
        If pHru.ParentHru IsNot Nothing Then
            hwriter.Write("Parent HRU: " & pHru.ParentHru.Name)
            hwriter.WriteBreak()
            'Selected zones
            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder
            Dim selZones As Zone() = pHru.SelectedZones
            If selZones IsNot Nothing AndAlso selZones.Length > 0 AndAlso selZones(0) IsNot Nothing Then
                For Each pZone In selZones
                    sb.Append(CStr(pZone.Id))
                    sb.Append(", ")
                Next
                ' Remove trailing comma
                sb.Remove(sb.Length - 2, 2)
                hwriter.Write("Apply to parent HRU zones: " & sb.ToString)
                hwriter.WriteBreak()
            End If
        End If
        hwriter.Write("HRU file path: " & pHru.FilePath)
        hwriter.WriteBreak()
        hwriter.Write("Allow non-contiguous HRUs: ")
        If pHru.AllowNonContiguousHru = True Then
            hwriter.Write(YES)
        Else
            hwriter.Write(NO)
        End If
        hwriter.WriteBreak()
        hwriter.Write("Feature count: " & pHru.FeatureCount)
        hwriter.WriteBreak()
        hwriter.Write("Average feature size: " & pHru.AverageFeatureSize & "&nbsp;")
        If pHru.Units <> MeasurementUnit.Missing Then
            hwriter.Write(pHru.UnitsText)
        End If
        hwriter.WriteBreak()
        hwriter.Write("Maximum feature size: " & pHru.MaxFeatureSize & "&nbsp;")
        If pHru.Units <> MeasurementUnit.Missing Then
            hwriter.Write(pHru.UnitsText)
        End If
        hwriter.WriteBreak()
        hwriter.Write("Date created: " & pHru.DateCreatedValue)
        hwriter.RenderEndTag()

        AddRules(hwriter, pHru.RuleList)
        If pHru.CookieCutProcess IsNot Nothing Then
            AddCookieCut(hwriter, pHru)
        End If
        If pHru.EliminateProcess IsNot Nothing Then
            AddEliminate(hwriter, pHru)
        End If
        If pHru.ReclassZonesRule IsNot Nothing Then
            AddReclassZones(hwriter, pHru)
        End If
    End Sub

    'Append the meta tags to the header
    Private Sub AddMetaTags(ByRef hwriter As HtmlTextWriter)
        'author
        hwriter.AddAttribute(HtmlTextWriterAttribute.Name, "author")
        hwriter.AddAttribute(HtmlTextWriterAttribute.Content, "Center for Spatial Analysis and Research, " & _
                             "Geography, Portland State University")
        hwriter.RenderBeginTag(HtmlTextWriterTag.Meta)
        hwriter.RenderEndTag()
        'webpage
        hwriter.AddAttribute(HtmlTextWriterAttribute.Name, "webpage")
        hwriter.AddAttribute(HtmlTextWriterAttribute.Content, "http://www.geog.pdx.edu/CSAR/BAGIS/")
        hwriter.RenderBeginTag(HtmlTextWriterTag.Meta)
        hwriter.RenderEndTag()
        'keywords
        hwriter.AddAttribute(HtmlTextWriterAttribute.Name, "keywords")
        hwriter.AddAttribute(HtmlTextWriterAttribute.Content, "Basin Analysis, BAGIS, GIS, ArcGIS, NRCS, " & _
                             "water forecast, National Water and Climate Center")
        hwriter.RenderBeginTag(HtmlTextWriterTag.Meta)
        hwriter.RenderEndTag()
    End Sub

    Private Sub AddRules(ByRef hwriter As HtmlTextWriter, ByVal pRuleList As List(Of IRule))
        'Start processing the rules
        pRuleList.Sort()
        For Each pRule In pRuleList
            'Rule header
            hwriter.AddStyleAttribute(HtmlTextWriterStyle.PaddingLeft, "10px")
            hwriter.RenderBeginTag(HtmlTextWriterTag.Div)
            hwriter.RenderBeginTag(HtmlTextWriterTag.B)
            hwriter.AddStyleAttribute(HtmlTextWriterStyle.Color, "Blue")
            hwriter.RenderBeginTag(HtmlTextWriterTag.Font)
            hwriter.Write(pRule.RuleTypeText & " rule")
            hwriter.RenderEndTag()
            hwriter.RenderEndTag()
            hwriter.WriteBreak()

            'Common rule attributes
            hwriter.Write("Input layer name: " & pRule.InputLayerName)
            hwriter.WriteBreak()
            hwriter.Write("Input folder path: " & pRule.InputFolderPath)
            hwriter.WriteBreak()
            hwriter.Write("Output dataset name: " & pRule.OutputDatasetName)
            hwriter.WriteBreak()

            If TypeOf pRule Is RasterSliceRule Then
                Dim sliceRule As RasterSliceRule = CType(pRule, RasterSliceRule)
                hwriter.Write("Slice method: " & sliceRule.SliceTypeText)
                hwriter.WriteBreak()
                hwriter.Write("Number of zones: " & sliceRule.ZoneCount)
                hwriter.WriteBreak()
                hwriter.Write("Base zone value: " & sliceRule.BaseZone)
                hwriter.WriteBreak()
            ElseIf TypeOf pRule Is RasterReclassRule Then
                Dim reclassRule As RasterReclassRule = CType(pRule, RasterReclassRule)
                hwriter.Write("Reclass field: " & reclassRule.ReclassField)
                hwriter.WriteBreak()
                hwriter.Write("Reclass values: ")
                'start the table
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.Width, "400px")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "14px")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, "Arial")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, "black")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderCollapse, "collapse")
                hwriter.RenderBeginTag(HtmlTextWriterTag.Table)
                Dim reclassItems As BAGIS_ClassLibrary.ReclassItem() = reclassRule.ReclassItems
                If reclassItems IsNot Nothing AndAlso reclassItems.Length > 0 Then
                    'Render the headers
                    'start the row
                    hwriter.RenderBeginTag(HtmlTextWriterTag.Tr)
                    'add data
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                    hwriter.RenderBeginTag(HtmlTextWriterTag.Th)
                    hwriter.Write("Old Values")
                    hwriter.RenderEndTag()
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                    hwriter.RenderBeginTag(HtmlTextWriterTag.Th)
                    hwriter.Write("New Values")
                    hwriter.RenderEndTag()
                    'end the row
                    hwriter.RenderEndTag()
                    For i As Short = 0 To reclassItems.GetUpperBound(0) Step 1
                        Dim reclassItem As BAGIS_ClassLibrary.ReclassItem = reclassItems(i)
                        'start the row
                        hwriter.RenderBeginTag(HtmlTextWriterTag.Tr)
                        'add data
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                        hwriter.RenderBeginTag(HtmlTextWriterTag.Td)
                        hwriter.Write(reclassItem.FromValue)
                        hwriter.RenderEndTag()
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                        hwriter.RenderBeginTag(HtmlTextWriterTag.Td)
                        hwriter.Write(reclassItem.OutputValue)
                        hwriter.RenderEndTag()
                        'end the row
                        hwriter.RenderEndTag()
                    Next
                End If

                Dim reclassTextItems As BAGIS_ClassLibrary.ReclassTextItem() = reclassRule.ReclassTextItems
                If reclassTextItems IsNot Nothing AndAlso reclassTextItems.Length > 0 Then
                    'Render the headers
                    'start the row
                    hwriter.RenderBeginTag(HtmlTextWriterTag.Tr)
                    'add data
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                    hwriter.RenderBeginTag(HtmlTextWriterTag.Th)
                    hwriter.Write("Old Values")
                    hwriter.RenderEndTag()
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                    hwriter.RenderBeginTag(HtmlTextWriterTag.Th)
                    hwriter.Write("New Values")
                    hwriter.RenderEndTag()
                    'end the row
                    hwriter.RenderEndTag()
                    For i As Short = 0 To reclassTextItems.GetUpperBound(0) Step 1
                        Dim reclassTextItem As BAGIS_ClassLibrary.ReclassTextItem = reclassTextItems(i)
                        'start the row
                        hwriter.RenderBeginTag(HtmlTextWriterTag.Tr)
                        'add data
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                        hwriter.RenderBeginTag(HtmlTextWriterTag.Td)
                        hwriter.Write(reclassTextItem.FromValue)
                        hwriter.RenderEndTag()
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                        hwriter.RenderBeginTag(HtmlTextWriterTag.Td)
                        hwriter.Write(reclassTextItem.OutputValue)
                        hwriter.RenderEndTag()
                        'end the row
                        hwriter.RenderEndTag()
                    Next
                End If
                'Finish the table
                hwriter.RenderEndTag()
            ElseIf TypeOf pRule Is ReclassContinuousRule Then
                Dim reclassRule As ReclassContinuousRule = CType(pRule, ReclassContinuousRule)
                hwriter.Write("Reclass field: " & reclassRule.ReclassField)
                hwriter.WriteBreak()
                hwriter.Write("Reclass values: ")
                'start the table
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.Width, "400px")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "14px")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, "Arial")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, "black")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderCollapse, "collapse")
                hwriter.RenderBeginTag(HtmlTextWriterTag.Table)
                Dim reclassItems As BAGIS_ClassLibrary.ReclassItem() = reclassRule.ReclassItems
                If reclassItems IsNot Nothing AndAlso reclassItems.Length > 0 Then
                    'Render the headers
                    'start the row
                    hwriter.RenderBeginTag(HtmlTextWriterTag.Tr)
                    'add data
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                    hwriter.RenderBeginTag(HtmlTextWriterTag.Th)
                    hwriter.Write("From")
                    hwriter.RenderEndTag()
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                    hwriter.RenderBeginTag(HtmlTextWriterTag.Th)
                    hwriter.Write("To")
                    hwriter.RenderEndTag()
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                    hwriter.RenderBeginTag(HtmlTextWriterTag.Th)
                    hwriter.Write("New Value")
                    hwriter.RenderEndTag()
                    'end the row
                    hwriter.RenderEndTag()
                    For i As Short = 0 To reclassItems.GetUpperBound(0) Step 1
                        Dim reclassItem As BAGIS_ClassLibrary.ReclassItem = reclassItems(i)
                        'start the row
                        hwriter.RenderBeginTag(HtmlTextWriterTag.Tr)
                        'add data
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                        hwriter.RenderBeginTag(HtmlTextWriterTag.Td)
                        hwriter.Write(reclassItem.FromValue)
                        hwriter.RenderEndTag()
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                        hwriter.RenderBeginTag(HtmlTextWriterTag.Td)
                        hwriter.Write(reclassItem.ToValue)
                        hwriter.RenderEndTag()
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                        hwriter.RenderBeginTag(HtmlTextWriterTag.Td)
                        hwriter.Write(reclassItem.OutputValue)
                        hwriter.RenderEndTag()
                        'end the row
                        hwriter.RenderEndTag()
                    Next
                    'Finish the table
                    hwriter.RenderEndTag()
                End If
            ElseIf TypeOf pRule Is PrismPrecipRule Then
                Dim prismRule As PrismPrecipRule = CType(pRule, PrismPrecipRule)
                hwriter.Write("Data range: " & prismRule.DataRangeText)
                hwriter.WriteBreak()
                If prismRule.DataRange = BAGIS_ClassLibrary.PrismDataRange.Custom Then
                    hwriter.Write("From: " & prismRule.firstMonth & " To: " & prismRule.lastMonth)
                    hwriter.WriteBreak()
                End If
                hwriter.Write("Precipitation units: " & prismRule.DisplayUnitsText)
                hwriter.WriteBreak()
                hwriter.Write("Reclass values: ")
                'start the table
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.Width, "400px")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "14px")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, "Arial")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, "black")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderCollapse, "collapse")
                hwriter.RenderBeginTag(HtmlTextWriterTag.Table)
                Dim reclassItems As BAGIS_ClassLibrary.ReclassItem() = prismRule.ReclassItems
                If reclassItems IsNot Nothing AndAlso reclassItems.Length > 0 Then
                    'Render the headers
                    'start the row
                    hwriter.RenderBeginTag(HtmlTextWriterTag.Tr)
                    'add data
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                    hwriter.RenderBeginTag(HtmlTextWriterTag.Th)
                    hwriter.Write("From")
                    hwriter.RenderEndTag()
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                    hwriter.RenderBeginTag(HtmlTextWriterTag.Th)
                    hwriter.Write("To")
                    hwriter.RenderEndTag()
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                    hwriter.RenderBeginTag(HtmlTextWriterTag.Th)
                    hwriter.Write("New Value")
                    hwriter.RenderEndTag()
                    'end the row
                    hwriter.RenderEndTag()
                    For i As Short = 0 To reclassItems.GetUpperBound(0) Step 1
                        Dim reclassItem As BAGIS_ClassLibrary.ReclassItem = reclassItems(i)
                        'start the row
                        hwriter.RenderBeginTag(HtmlTextWriterTag.Tr)
                        'add data
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                        hwriter.RenderBeginTag(HtmlTextWriterTag.Td)
                        hwriter.Write(reclassItem.FromValue)
                        hwriter.RenderEndTag()
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                        hwriter.RenderBeginTag(HtmlTextWriterTag.Td)
                        hwriter.Write(reclassItem.ToValue)
                        hwriter.RenderEndTag()
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                        hwriter.RenderBeginTag(HtmlTextWriterTag.Td)
                        hwriter.Write(reclassItem.OutputValue)
                        hwriter.RenderEndTag()
                        'end the row
                        hwriter.RenderEndTag()
                    Next
                    'Finish the table
                    hwriter.RenderEndTag()
                End If
            ElseIf TypeOf pRule Is DAFlowTypeZonesRule Then
                Dim flowRule As DAFlowTypeZonesRule = CType(pRule, DAFlowTypeZonesRule)
                Select Case flowRule.ByParameter
                    Case BAGIS_ClassLibrary.DAFlowByParam.BA_DAFlowByXYDimension
                        hwriter.Write("Flow type: By HRU dimensions")
                        hwriter.WriteBreak()
                        hwriter.Write("X size: " & flowRule.HruXSize & " Y size: " & flowRule.HruYSize)
                        hwriter.WriteBreak()
                        hwriter.Write("Units: " & flowRule.MeasurementUnitsText)
                        hwriter.WriteBreak()
                    Case BAGIS_ClassLibrary.DAFlowByParam.BA_DAFlowByHRUNumber
                        hwriter.Write("Flow type: By # of HRUs")
                        hwriter.WriteBreak()
                        hwriter.Write("Estimated # of HRUs: " & flowRule.HruNumber)
                        hwriter.WriteBreak()
                    Case BAGIS_ClassLibrary.DAFlowByParam.BA_DAFlowByRowCol
                        hwriter.Write("Flow type: By # of rows/columns")
                        hwriter.WriteBreak()
                        hwriter.Write("# of Col (X): " & flowRule.HruCol)
                        hwriter.WriteBreak()
                        hwriter.Write("# of Row (Y): " & flowRule.HruRow)
                        hwriter.WriteBreak()
                End Select
            ElseIf TypeOf pRule Is ContributingAreasRule Then
                Dim contribRule As ContributingAreasRule = CType(pRule, ContributingAreasRule)
                hwriter.Write("Contributing zones: " & contribRule.NumberofArea)
                hwriter.WriteBreak()
                hwriter.Write("Mean flow accumulation: " & contribRule.FACCStandardDeviation)
                hwriter.WriteBreak()
                hwriter.Write("Stream link threshold value: " & contribRule.FACCThresholdValue & " Cells")
                hwriter.WriteBreak()
            ElseIf TypeOf pRule Is TemplateRule Then
                Dim templateRule As TemplateRule = CType(pRule, TemplateRule)
                Dim templateActions As List(Of TemplateAction) = templateRule.TemplateActions
                If templateRule.RuleType <> HruRuleType.LandUse Then
                    'start the table
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.Width, "600px")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "14px")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, "Arial")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, "black")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderCollapse, "collapse")
                    hwriter.RenderBeginTag(HtmlTextWriterTag.Table)
                    'Render the headers
                    'start the row
                    hwriter.RenderBeginTag(HtmlTextWriterTag.Tr)
                    'add data
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.Width, "200px")
                    hwriter.RenderBeginTag(HtmlTextWriterTag.Th)
                    hwriter.Write("Action")
                    hwriter.RenderEndTag()
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.Width, "200px")
                    hwriter.RenderBeginTag(HtmlTextWriterTag.Th)
                    hwriter.Write("Parameter name")
                    hwriter.RenderEndTag()
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.Width, "200px")
                    hwriter.RenderBeginTag(HtmlTextWriterTag.Th)
                    hwriter.Write("Parameter value")
                    hwriter.RenderEndTag()
                    'end the row
                    hwriter.RenderEndTag()
                    For Each pAction In templateActions
                        Dim params As Hashtable = pAction.parameters
                        Dim keyCollection As ICollection = params.Keys
                        ' Variable to track if we need to populate the action cell; Only first parameter per action
                        Dim actionCell As Boolean = True
                        For Each objKey In keyCollection
                            Dim key As BAGIS_ClassLibrary.ActionParameter = CType(objKey, BAGIS_ClassLibrary.ActionParameter)
                            Dim value As Object = params(objKey)
                            If TypeOf value Is String Then            '---create a row---
                                'start the row
                                hwriter.RenderBeginTag(HtmlTextWriterTag.Tr)
                                'add data
                                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                                hwriter.RenderBeginTag(HtmlTextWriterTag.Td)
                                If actionCell = True Then
                                    hwriter.Write(BA_EnumDescription(pAction.actionType))
                                    actionCell = False
                                End If
                                hwriter.RenderEndTag()
                                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                                hwriter.RenderBeginTag(HtmlTextWriterTag.Td)
                                hwriter.Write(key.ToString)
                                hwriter.RenderEndTag()
                                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                                hwriter.RenderBeginTag(HtmlTextWriterTag.Td)
                                Dim val1 As String = CStr(value)
                                Dim val2 As String = val1.Replace("–", "&#45;")
                                hwriter.Write(val2)
                                hwriter.RenderEndTag()
                                'end the row
                                hwriter.RenderEndTag()
                            End If
                        Next
                        actionCell = True
                    Next
                    'Finish the table
                    hwriter.RenderEndTag()
                Else
                    Dim templateName As String = Nothing
                    Dim reclassField As String = Nothing
                    Dim reclassItems As ReclassItem() = Nothing
                    Dim filterWidth As String = Nothing
                    Dim filterHeight As String = Nothing
                    Dim iterations As String = Nothing
                    For Each pAction In templateActions
                        Dim params As Hashtable = pAction.parameters
                        If pAction.actionType = ActionType.ReclDisc Then
                            reclassField = CStr(params(ActionParameter.ReclassField))
                            templateName = CStr(params(ActionParameter.LandUseOptions))
                            reclassItems = CType(params(BAGIS_ClassLibrary.ActionParameter.ReclassItems), ReclassItem())
                        ElseIf pAction.actionType = ActionType.MajorityFilter Then
                            filterWidth = CStr(params(ActionParameter.RectangleWidth))
                            filterHeight = CStr(params(ActionParameter.RectangleHeight))
                            iterations = CStr(params(ActionParameter.IterationCount))
                        End If
                    Next
                    hwriter.Write("Template name: " & templateName)
                    hwriter.WriteBreak()
                    hwriter.Write("Reclass field: " & reclassField)
                    hwriter.WriteBreak()
                    If reclassItems IsNot Nothing And reclassItems.Length > 0 Then
                        hwriter.Write("Reclass values: ")
                        'start the table
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.Width, "600px")
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "14px")
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, "Arial")
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, "black")
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderCollapse, "collapse")
                        hwriter.RenderBeginTag(HtmlTextWriterTag.Table)
                        'Render the headers
                        'start the row
                        hwriter.RenderBeginTag(HtmlTextWriterTag.Tr)
                        'add data
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                        hwriter.RenderBeginTag(HtmlTextWriterTag.Th)
                        hwriter.Write("Old Value")
                        hwriter.RenderEndTag()
                        hwriter.RenderBeginTag(HtmlTextWriterTag.Th)
                        hwriter.Write("Description")
                        hwriter.RenderEndTag()
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                        hwriter.RenderBeginTag(HtmlTextWriterTag.Th)
                        hwriter.Write("New Value")
                        hwriter.RenderEndTag()
                        hwriter.RenderBeginTag(HtmlTextWriterTag.Th)
                        hwriter.Write("Description")
                        hwriter.RenderEndTag()
                        'end the row
                        hwriter.RenderEndTag()
                        For i As Short = 0 To reclassItems.GetUpperBound(0) Step 1
                            Dim reclassItem As BAGIS_ClassLibrary.ReclassItem = reclassItems(i)
                            'start the row
                            hwriter.RenderBeginTag(HtmlTextWriterTag.Tr)
                            'add data
                            hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                            hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                            hwriter.RenderBeginTag(HtmlTextWriterTag.Td)
                            hwriter.Write(reclassItem.FromValue)
                            hwriter.RenderEndTag()
                            hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                            hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                            hwriter.RenderBeginTag(HtmlTextWriterTag.Td)
                            hwriter.Write(reclassItem.FromDescr)
                            hwriter.RenderEndTag()
                            hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                            hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                            hwriter.RenderBeginTag(HtmlTextWriterTag.Td)
                            hwriter.Write(reclassItem.OutputValue)
                            hwriter.RenderEndTag()
                            hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                            hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                            hwriter.RenderBeginTag(HtmlTextWriterTag.Td)
                            hwriter.Write(reclassItem.OutputDescr)
                            hwriter.RenderEndTag()
                            'end the row
                            hwriter.RenderEndTag()
                        Next
                        'end the table
                        hwriter.RenderEndTag()
                        hwriter.WriteBreak()
                    End If
                    hwriter.Write("Filter width: " & filterWidth)
                    hwriter.WriteBreak()
                    hwriter.Write("Filter height: " & filterHeight)
                    hwriter.WriteBreak()
                    hwriter.Write("Iterations: " & iterations)
                    hwriter.WriteBreak()
                End If
            End If
            hwriter.RenderEndTag()
            hwriter.WriteBreak()
        Next
    End Sub

    Private Sub AddCookieCut(ByRef hwriter As HtmlTextWriter, ByVal pHru As Hru)
        Dim process As CookieCutProcess = pHru.CookieCutProcess
        'Cookie-cut/stamp header
        hwriter.AddStyleAttribute(HtmlTextWriterStyle.PaddingLeft, "10px")
        hwriter.RenderBeginTag(HtmlTextWriterTag.Div)
        hwriter.RenderBeginTag(HtmlTextWriterTag.B)
        hwriter.AddStyleAttribute(HtmlTextWriterStyle.Color, "Blue")
        hwriter.RenderBeginTag(HtmlTextWriterTag.Font)
        hwriter.Write(process.Mode)
        hwriter.RenderEndTag()
        hwriter.RenderEndTag()
        hwriter.WriteBreak()
        hwriter.Write(process.Mode & " layer name: " & process.CookieCutName)
        hwriter.WriteBreak()
        hwriter.Write(process.Mode & " layer path: " & process.CookieCutPath)
        hwriter.WriteBreak()
        hwriter.Write(process.Mode & " layer field: " & process.CookieCutField)
        hwriter.WriteBreak()
        'Assemble selected values string
        Dim selValues As Long() = process.SelectedValues
        Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder
        If selValues IsNot Nothing AndAlso selValues.Length > 0 AndAlso selValues(0) <> 0 Then
            For Each pLong In selValues
                sb.Append(CStr(pLong))
                sb.Append(", ")
            Next
            ' Remove trailing comma
            sb.Remove(sb.Length - 2, 2)
        End If
        hwriter.Write("Selected values: " & sb.ToString)
        hwriter.WriteBreak()
        If process.Mode = BA_MODE_COOKIE_CUT Then
            hwriter.Write("Preserve AOI boundary: " & process.PreserveAoiBoundary)
            hwriter.WriteBreak()
        End If
        hwriter.RenderEndTag()
    End Sub

    Private Sub AddEliminate(ByRef hwriter As HtmlTextWriter, ByVal pHru As Hru)
        Dim process As EliminateProcess = pHru.EliminateProcess
        'Cookie-cut/stamp header
        hwriter.AddStyleAttribute(HtmlTextWriterStyle.PaddingLeft, "10px")
        hwriter.RenderBeginTag(HtmlTextWriterTag.Div)
        hwriter.RenderBeginTag(HtmlTextWriterTag.B)
        hwriter.AddStyleAttribute(HtmlTextWriterStyle.Color, "Blue")
        hwriter.RenderBeginTag(HtmlTextWriterTag.Font)
        hwriter.Write("Eliminate")
        hwriter.RenderEndTag()
        hwriter.RenderEndTag()
        hwriter.WriteBreak()
        If process.SelectByPolyArea = True Then
            hwriter.Write("Eliminate threshold: Area of zone")
        Else
            hwriter.Write("Eliminate threshold: Percentile")
        End If
        hwriter.WriteBreak()
        hwriter.Write("Merge polygons by: " & process.SelectionMethod)
        hwriter.WriteBreak()
        hwriter.Write("Threshold polygon area: " & process.PolygonArea)
        hwriter.WriteBreak()
        hwriter.Write("Area unit: " & process.PolygonAreaUnitsText)
        hwriter.WriteBreak()
        If process.SelectByPercentile = True Then
            hwriter.Write("Threshold percentile: " & Format(process.AreaPercent, "##0"))
            hwriter.WriteBreak()
        End If
        hwriter.Write("Zones removed: " & process.PolygonsEliminated)
        hwriter.WriteBreak()
        hwriter.RenderEndTag()
    End Sub

    Private Sub AddReclassZones(ByRef hwriter As HtmlTextWriter, ByVal pHru As Hru)
        Dim pRule As RasterReclassRule = pHru.ReclassZonesRule
        'Reclass zones header
        hwriter.AddStyleAttribute(HtmlTextWriterStyle.PaddingLeft, "10px")
        hwriter.RenderBeginTag(HtmlTextWriterTag.Div)
        hwriter.RenderBeginTag(HtmlTextWriterTag.B)
        hwriter.AddStyleAttribute(HtmlTextWriterStyle.Color, "Blue")
        hwriter.RenderBeginTag(HtmlTextWriterTag.Font)
        hwriter.Write("Reclass Zones")
        hwriter.RenderEndTag()
        hwriter.RenderEndTag()
        hwriter.WriteBreak()

        hwriter.Write("Input layer name: " & pRule.InputLayerName)
        hwriter.WriteBreak()
        hwriter.Write("Input folder path: " & pRule.InputFolderPath)
        hwriter.WriteBreak()

        hwriter.Write("Reclass field: " & pRule.ReclassField)
        hwriter.WriteBreak()
        hwriter.Write("Reclass values: ")
        'start the table
        hwriter.AddStyleAttribute(HtmlTextWriterStyle.Width, "400px")
        hwriter.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "14px")
        hwriter.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, "Arial")
        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, "black")
        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
        hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderCollapse, "collapse")
        hwriter.RenderBeginTag(HtmlTextWriterTag.Table)
        Dim reclassItems As BAGIS_ClassLibrary.ReclassItem() = pRule.ReclassItems
        If reclassItems IsNot Nothing AndAlso reclassItems.Length > 0 Then
            'Render the headers
            'start the row
            hwriter.RenderBeginTag(HtmlTextWriterTag.Tr)
            'add data
            hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
            hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
            hwriter.RenderBeginTag(HtmlTextWriterTag.Th)
            hwriter.Write("Old Values")
            hwriter.RenderEndTag()
            hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
            hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
            hwriter.RenderBeginTag(HtmlTextWriterTag.Th)
            hwriter.Write("New Values")
            hwriter.RenderEndTag()
            'end the row
            hwriter.RenderEndTag()
            For i As Short = 0 To reclassItems.GetUpperBound(0) Step 1
                Dim reclassItem As BAGIS_ClassLibrary.ReclassItem = reclassItems(i)
                'start the row
                hwriter.RenderBeginTag(HtmlTextWriterTag.Tr)
                'add data
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                hwriter.RenderBeginTag(HtmlTextWriterTag.Td)
                hwriter.Write(reclassItem.FromValue)
                hwriter.RenderEndTag()
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                hwriter.RenderBeginTag(HtmlTextWriterTag.Td)
                Dim outputValue As String = CStr(reclassItem.OutputValue)
                If outputValue = CStr(BA_9999) Then outputValue = BA_NODATA
                hwriter.Write(outputValue)
                hwriter.RenderEndTag()
                'end the row
                hwriter.RenderEndTag()
            Next
        End If
        hwriter.RenderEndTag()
    End Sub

    Public Sub BA_WriteParameterFile(ByVal logProfile As LogProfile, ByVal filePath As String, ByVal overwrite As Boolean)
        Dim profileName As String = logProfile.Name
        If overwrite = True Then
            If File.Exists(filePath) Then
                File.Delete(filePath)
            End If
        End If
        Dim sb As StringBuilder = New StringBuilder()
        Dim hWriter As HtmlTextWriter = New HtmlTextWriter(New StringWriter(sb))

        Dim hruName As String = BA_GetBareName(logProfile.HruPath)
        'start the page
        hWriter.RenderBeginTag(HtmlTextWriterTag.Html)
        'start the header
        hWriter.RenderBeginTag(HtmlTextWriterTag.Head)
        'start the title
        hWriter.RenderBeginTag(HtmlTextWriterTag.Title)
        hWriter.Write("BAGIS-P Parameter Calculation Method Summary for Profile '" & logProfile.Name & "' in HRU '" & hruName & "'")
        hWriter.RenderEndTag()
        AddMetaTags(hWriter)
        hWriter.RenderEndTag()
        'start the body
        hWriter.RenderBeginTag(HtmlTextWriterTag.Body)
        'start the high-level div tag and accompanying style
        hWriter.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "14px")
        hWriter.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, "Arial")
        hWriter.AddStyleAttribute(HtmlTextWriterStyle.PaddingLeft, "5px")
        hWriter.RenderBeginTag(HtmlTextWriterTag.Div)
        'start the div tag for the title
        hWriter.AddStyleAttribute(HtmlTextWriterStyle.FontWeight, "Bold")
        hWriter.AddStyleAttribute(HtmlTextWriterStyle.TextAlign, "center")
        hWriter.RenderBeginTag(HtmlTextWriterTag.Div)
        hWriter.Write("BAGIS-P Parameter Calculation Method Summary for Profile '" & logProfile.Name & "' in HRU '" & hruName & "'")
        hWriter.RenderEndTag()

        'start the hru summary section
        hWriter.RenderBeginTag(HtmlTextWriterTag.P)
        hWriter.RenderBeginTag(HtmlTextWriterTag.B)
        hWriter.Write("HRU name: " & hruName)
        hWriter.RenderEndTag()
        hWriter.WriteBreak()
        hWriter.Write("HRU file path: " & logProfile.HruPath)
        hWriter.WriteBreak()
        hWriter.Write("Application version: " & logProfile.Version)
        hWriter.RenderEndTag()


        'start the profile summary section
        hWriter.RenderBeginTag(HtmlTextWriterTag.P)
        hWriter.RenderBeginTag(HtmlTextWriterTag.B)
        hWriter.Write("Profile name: " & logProfile.Name)
        hWriter.RenderEndTag()
        hWriter.WriteBreak()
        hWriter.Write("Description: " & logProfile.Description)
        hWriter.WriteBreak()
        hWriter.Write("Date calculations completed: " & logProfile.DateCompleted.ToString("g", Globalization.DateTimeFormatInfo.InvariantInfo))
        hWriter.WriteBreak()

        'print section for each method: subroutine
        Dim methodArr() As Method = logProfile.Methods
        Dim hruMethodArr() As Method = logProfile.HruMethods
        Dim i As Integer = 0
        If methodArr IsNot Nothing Then
            For Each nextMethod In methodArr
                Dim hruMethod As Method = hruMethodArr(i)
                hWriter.WriteBreak()
                AddMethodSummary(hWriter, nextMethod, hruMethod)
                i += 1
            Next
        End If

        hWriter.RenderEndTag()
        hWriter.RenderEndTag()
        hWriter.RenderEndTag()
        Using outfile As New StreamWriter(filePath)
            outfile.Write(sb.ToString())
        End Using
    End Sub

    ' Add the summary section for an Hru to the htmlWriter
    Private Sub AddMethodSummary(ByRef hwriter As HtmlTextWriter, ByVal pMethod As Method, ByVal hruMethod As Method)
        Try

            hwriter.AddStyleAttribute(HtmlTextWriterStyle.PaddingLeft, "10px")
            hwriter.RenderBeginTag(HtmlTextWriterTag.Div)
            hwriter.RenderBeginTag(HtmlTextWriterTag.B)
            hwriter.Write("Method name: " & pMethod.Name)
            hwriter.RenderEndTag()
            hwriter.WriteBreak()
            hwriter.Write("Description: " & pMethod.Description)
            hwriter.WriteBreak()
            hwriter.Write("Toolbox path: " & pMethod.ToolBoxPath)
            hwriter.WriteBreak()
            hwriter.Write("Toolbox name: " & pMethod.ToolboxName)
            hwriter.WriteBreak()
            hwriter.Write("Model name: " & pMethod.ModelLabel)
            hwriter.WriteBreak()
            hwriter.Write("Include method: ")
            If hruMethod.UseMethod = True Then
                hwriter.Write(YES)
            Else
                hwriter.Write(NO)
            End If
            hwriter.WriteBreak()
            If hruMethod.Status = MethodStatus.Complete Or _
                hruMethod.Status = MethodStatus.Pending Or _
                hruMethod.Status = MethodStatus.Verified Then
                hwriter.Write("Method status: " & hruMethod.StatusText)
            Else
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.Color, "red")
                hwriter.RenderBeginTag(HtmlTextWriterTag.Span)
                hwriter.Write("Method status: " & hruMethod.StatusText)
                hwriter.RenderEndTag()
            End If
            hwriter.WriteBreak()
            If Not String.IsNullOrEmpty(hruMethod.ErrorMessage) Then
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.Color, "red")
                hwriter.RenderBeginTag(HtmlTextWriterTag.Span)
                hwriter.Write("Error message: " & hruMethod.ErrorMessage)
                hwriter.RenderEndTag()
                hwriter.WriteBreak()
            End If
            If pMethod.FilledModelParameters IsNot Nothing AndAlso pMethod.FilledModelParameters.Count > 0 Then
                hwriter.Write("Model parameters: ")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.PaddingLeft, "10px")
                hwriter.RenderBeginTag(HtmlTextWriterTag.Div)
                'start the table
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.Width, "700px")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "14px")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.FontFamily, "Arial")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderColor, "black")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderCollapse, "collapse")
                hwriter.RenderBeginTag(HtmlTextWriterTag.Table)
                Dim modelParams As IList(Of ModelParameter) = pMethod.FilledModelParameters
                'Render the headers
                'start the row
                hwriter.RenderBeginTag(HtmlTextWriterTag.Tr)
                'add data
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                hwriter.RenderBeginTag(HtmlTextWriterTag.Th)
                hwriter.Write("Parameter name")
                hwriter.RenderEndTag()
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                hwriter.RenderBeginTag(HtmlTextWriterTag.Th)
                hwriter.Write("Parameter value")
                hwriter.RenderEndTag()
                'end the row
                hwriter.RenderEndTag()
                For Each mParam As ModelParameter In modelParams
                    'start the row
                    hwriter.RenderBeginTag(HtmlTextWriterTag.Tr)
                    'add data
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.PaddingLeft, "3px")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.PaddingRight, "3px")
                    hwriter.RenderBeginTag(HtmlTextWriterTag.Td)
                    hwriter.Write(mParam.Name)
                    hwriter.RenderEndTag()
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderWidth, "1px")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.BorderStyle, "solid")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.PaddingLeft, "3px")
                    hwriter.AddStyleAttribute(HtmlTextWriterStyle.PaddingRight, "3px")
                    hwriter.RenderBeginTag(HtmlTextWriterTag.Td)
                    hwriter.Write(mParam.Value)
                    hwriter.RenderEndTag()
                    'end the row
                    hwriter.RenderEndTag()
                Next
                hwriter.RenderEndTag()
                hwriter.RenderEndTag()
            End If
            hwriter.RenderEndTag()
        Catch ex As Exception
            Debug.Print("AddMethodSummary " & ex.Message)
        End Try
    End Sub

End Module
