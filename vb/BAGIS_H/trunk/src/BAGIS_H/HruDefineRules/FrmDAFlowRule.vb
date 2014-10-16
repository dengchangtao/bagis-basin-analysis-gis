Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.Carto
Imports System.Windows.Forms
Imports ESRI.ArcGIS.Geometry
Imports System.IO
Imports ESRI.ArcGIS.DataSourcesRaster
Imports BAGIS_ClassLibrary

Public Class FrmDAFlowRule
    Private m_aoiPath As String
    Private m_mapXOff, m_mapYOff As Double 'extend offset
    Private m_mapX0, m_mapY0 As Double 'extend origin (lower left)
    Private m_MetersPerUnit As Double
    Private m_MapUnitName As String
    Private m_AOIShapeArea As Double

    Public Sub New(ByVal aoiPath As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        m_aoiPath = aoiPath

        ' Add any initialization after the InitializeComponent() call.
        'get and set the envelope values of the AOI
        Dim fileName As String
        Dim pFeatClass As IFeatureClass
        Dim pEnv As IEnvelope
        Dim pFLayer As IFeatureLayer = New FeatureLayer
        Dim geoDataSet As IGeoDataset = Nothing
        Dim pSpRef As ISpatialReference = Nothing
        Dim projCoordSys As IProjectedCoordinateSystem = Nothing
        Dim pLinearUnit As ILinearUnit

        Dim gdbPath As String = aoiPath & "\" & BA_EnumDescription(GeodatabaseNames.Aoi)
        If BA_File_Exists(gdbPath & "\" & BA_EnumDescription(AOIClipFile.AOIExtentCoverage), _
                          WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            fileName = BA_EnumDescription(AOIClipFile.AOIExtentCoverage)
            Try
                m_AOIShapeArea = BA_GetShapeArea(gdbPath & "\" & fileName)
                pFeatClass = BA_OpenFeatureClassFromGDB(gdbPath, fileName)
                pFLayer.FeatureClass = pFeatClass
                pEnv = pFLayer.AreaOfInterest
                m_mapX0 = pEnv.XMin
                m_mapY0 = pEnv.YMin
                m_mapXOff = pEnv.XMax - pEnv.XMin
                m_mapYOff = pEnv.YMax - pEnv.YMin

                'First get the unit from the projected coordinate system
                geoDataSet = CType(pFeatClass, IGeoDataset)    'Explicit cast
                pSpRef = geoDataSet.SpatialReference
                projCoordSys = CType(pSpRef, IProjectedCoordinateSystem)  'Explicit cast
                pLinearUnit = projCoordSys.CoordinateUnit

                m_MetersPerUnit = pLinearUnit.MetersPerUnit
                m_MapUnitName = pLinearUnit.Name
                If m_MapUnitName = "Foot" Then
                    RadBtnMeter.Visible = True
                    RadBtnFoot.Visible = True
                    RadBtnMile.Visible = True
                    RadBtnFoot.Checked = True
                    RadBtnUnknown.Visible = False
                ElseIf m_MapUnitName = "Meter" Then
                    RadBtnMeter.Visible = True
                    RadBtnFoot.Visible = True
                    RadBtnMile.Visible = True
                    RadBtnMeter.Checked = True
                    RadBtnUnknown.Visible = False
                Else
                    MsgBox("Unknown map unit for the selected AOI! Please check data.")
                    RadBtnUnknown.Visible = True
                    RadBtnUnknown.Checked = True
                    RadBtnMeter.Visible = False
                    RadBtnFoot.Visible = False
                    RadBtnMile.Visible = False
                End If
                'MsgBox("MetersPerUnit = " & m_MetersPerUnit & " Abbrev = " & pLinearUnit.Name)

                'set default value using zone row and col
                Dim nCol As Long = 10
                Dim xSize As Double = m_mapXOff / nCol
                Dim nRow As Long = Math.Ceiling(m_mapYOff / xSize)
                TxtHRUNumber.Text = Math.Ceiling(m_AOIShapeArea / (xSize * xSize))
                TxtCol.Text = nCol
                TxtRow.Text = nRow
                TxtXSize.Text = xSize
                TxtYSize.Text = xSize

            Catch ex As Exception
                MessageBox.Show("Exception: " + ex.Message)
            Finally
                pFeatClass = Nothing
                pFLayer = Nothing
                pEnv = Nothing
                geoDataSet = Nothing
                pSpRef = Nothing
                projCoordSys = Nothing
                pLinearUnit = Nothing
                GC.WaitForPendingFinalizers()
                GC.Collect()
            End Try
        Else
            MessageBox.Show("Error: Cannot open AOI vector boundary shapefile. The AOI folder is invalid.")
        End If
    End Sub

    Public Sub LoadForm(ByVal pRule As BAGIS_ClassLibrary.IRule)
        Dim DAFlowRule As DAFlowTypeZonesRule = CType(pRule, DAFlowTypeZonesRule)
        TxtHRUNumber.Text = DAFlowRule.HruNumber
        TxtRow.Text = DAFlowRule.HruRow
        TxtCol.Text = DAFlowRule.HruCol
        TxtXSize.Text = DAFlowRule.HruXSize
        TxtYSize.Text = DAFlowRule.HruYSize
        If DAFlowRule.ByParameter = DAFlowByParam.BA_DAFlowByHRUNumber Then
            RadBtnByHRUNo.Checked = True
        ElseIf DAFlowRule.ByParameter = DAFlowByParam.BA_DAFlowByRowCol Then
            RadBtnByRowCol.Checked = True
        Else
            RadBtnByHRUDimension.Checked = True
        End If

        TxtRuleID.Text = CStr(DAFlowRule.RuleId)
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub RadBtnByHRUNo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadBtnByHRUNo.CheckedChanged
        If RadBtnByHRUNo.Checked Then 'disable other group controls 
            PanHRUNumber.Enabled = True
            PanRowCol.Enabled = False
            PanXYSize.Enabled = False
        End If
    End Sub

    Private Sub RadBtnByRowCol_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadBtnByRowCol.CheckedChanged
        If RadBtnByRowCol.Checked Then 'disable other group controls 
            PanHRUNumber.Enabled = False
            PanRowCol.Enabled = True
            PanXYSize.Enabled = False
        End If
    End Sub

    Private Sub RadBtnByHRUDimension_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadBtnByHRUDimension.CheckedChanged
        If RadBtnByHRUDimension.Checked Then 'disable other group controls 
            PanHRUNumber.Enabled = False
            PanRowCol.Enabled = False
            PanXYSize.Enabled = True
        End If
    End Sub

    Private Sub TxtHRUNumber_Validated(ByVal sender As Object, ByVal e As System.EventArgs)
        'validate input value
        If Val(TxtHRUNumber.Text) > 1 Then
            'use aoi shape area to estimate the row and column numbers 
            Dim cellSize As Double = _
                Math.Sqrt(m_AOIShapeArea / Val(TxtHRUNumber.Text))
            TxtXSize.Text = cellSize
            TxtYSize.Text = cellSize
            TxtCol.Text = CType(m_mapXOff / cellSize, Integer)
            TxtRow.Text = CType(m_mapYOff / cellSize, Integer)

            'update the HRU number by Row * Col
            'TxtHRUNumber.Text = Val(TxtCol.Text) * Val(TxtRow.Text)
            BtnApply.Enabled = True
        Else
            BtnApply.Enabled = False
            MsgBox("Please enter a number larger than 1.")
        End If
    End Sub

    Private Sub TxtHRUNumber_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TxtHRUNumber.KeyDown
        If e.KeyCode = Keys.Enter Then
            '   this is the call on the routine that does the password checking
            TxtHRUNumber_Validated(Nothing, Nothing)
        End If
    End Sub

    Private Sub BtnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        'adjust DAFlow parameters based on user selected parameter type
        Dim units As MeasurementUnit
        If RadBtnByRowCol.Enabled Then 'row and column numbers are fixed
            TxtXSize.Text = m_mapXOff / Val(TxtCol.Text)
            TxtYSize.Text = m_mapYOff / Val(TxtRow.Text)
            If RadBtnFoot.Checked = True Then
                units = MeasurementUnit.Feet
            ElseIf RadBtnMeter.Checked = True Then
                units = MeasurementUnit.Meters
            ElseIf RadBtnMile.Checked = True Then
                units = MeasurementUnit.Miles
            End If
        ElseIf RadBtnByHRUDimension.Enabled Then 'xsize and ysize values are fixed, modify row and column numbers
            TxtCol.Text = Math.Ceiling(m_mapXOff / Val(TxtXSize.Text))
            TxtRow.Text = Math.Ceiling(m_mapYOff / Val(TxtYSize.Text))
        Else 'by HRU number
            Dim cellSize As Double = _
                Math.Sqrt(m_AOIShapeArea / Val(TxtHRUNumber.Text))
            TxtXSize.Text = cellSize
            TxtYSize.Text = cellSize
            TxtCol.Text = Math.Ceiling(m_mapXOff / Val(TxtXSize.Text))
            TxtRow.Text = Math.Ceiling(m_mapYOff / Val(TxtYSize.Text))
        End If

        ' Increment ruleId before using
        Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of frmHruZone.AddinImpl)(My.ThisAddIn.IDs.frmHruZone)
        Dim hruZoneForm As frmHruZone = dockWindowAddIn.UI

        Dim ruleId As Integer
        If String.IsNullOrEmpty(TxtRuleId.Text) Then
            ruleId = hruZoneForm.GetNextRuleId
        Else
            ruleId = CInt(TxtRuleId.Text)
        End If

        Dim ruleByType As DAFlowByParam
        If RadBtnByHRUDimension.Checked Then
            ruleByType = DAFlowByParam.BA_DAFlowByXYDimension
        ElseIf RadBtnByHRUNo.Checked Then
            ruleByType = DAFlowByParam.BA_DAFlowByHRUNumber
        ElseIf RadBtnByRowCol.Checked Then
            ruleByType = DAFlowByParam.BA_DAFlowByRowCol
        End If

        Dim sizeUnit As Double = m_MetersPerUnit
        Dim inputLayerPath As String = m_aoiPath & "\" & BA_EnumDescription(GeodatabaseNames.Aoi) & "\" & BA_EnumDescription(AOIClipFile.AOIExtentCoverage)

        'DAFlowzones are generated by parameters, no inputfolder or file is associated with the rule
        Dim thisDAFlowRule = New DAFlowTypeZonesRule(BA_EnumDescription(AOIClipFile.AOIExtentCoverage), ruleByType, CLng(TxtHRUNumber.Text), _
            CInt(TxtRow.Text), CInt(TxtCol.Text), CDbl(TxtXSize.Text), CDbl(TxtYSize.Text), _
            sizeUnit, units, inputLayerPath, ruleId)

        'Debug.Print(thisDAFlowRule.InputFolderPath)
        'Debug.Print(thisDAFlowRule.OutputFolderName)

        hruZoneForm.AddPendingRule(thisDAFlowRule)
        BtnCancel_Click(sender, e)
    End Sub

    Private Sub TxtCol_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TxtCol.KeyDown
        If e.KeyCode = Keys.Enter Then
            '   this is the call on the routine that does the password checking
            TxtCol_Validated(Nothing, Nothing)
        End If
    End Sub

    Private Sub TxtCol_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtCol.Validated
        'validate input value
        If Val(TxtCol.Text) >= 1 Then
            If Val(TxtRow.Text) >= 1 Then
                'use aoi shape area to estimate the row and column numbers 
                Dim xCellSize As Double = m_mapXOff / Val(TxtCol.Text)
                Dim yCellSize As Double = m_mapYOff / Val(TxtRow.Text)
                TxtXSize.Text = xCellSize
                TxtYSize.Text = yCellSize

                'update the HRU number by Row * Col
                TxtHRUNumber.Text = Math.Ceiling(m_AOIShapeArea / (xCellSize * yCellSize))
                BtnApply.Enabled = True
            Else
                BtnApply.Enabled = False
            End If
        Else
            BtnApply.Enabled = False
            MsgBox("Please enter a number larger than 0.")
        End If
    End Sub

    Private Sub TxtRow_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TxtRow.KeyDown
        If e.KeyCode = Keys.Enter Then
            '   this is the call on the routine that does the password checking
            TxtRow_Validated(Nothing, Nothing)
        End If
    End Sub

    Private Sub TxtRow_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtRow.Validated
        'validate input value
        If Val(TxtRow.Text) >= 1 Then
            If Val(TxtCol.Text) >= 1 Then
                'use aoi shape area to estimate the row and column numbers 
                Dim xCellSize As Double = m_mapXOff / Val(TxtCol.Text)
                Dim yCellSize As Double = m_mapYOff / Val(TxtRow.Text)
                TxtXSize.Text = xCellSize
                TxtYSize.Text = yCellSize

                'update the HRU number by Row * Col
                TxtHRUNumber.Text = Math.Ceiling(m_AOIShapeArea / (xCellSize * yCellSize))
                BtnApply.Enabled = True
            Else
                BtnApply.Enabled = False
            End If
        Else
            BtnApply.Enabled = False
            MsgBox("Please enter a number larger than 0.")
        End If
    End Sub

    Private Sub TxtXSize_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TxtXSize.KeyDown
        If e.KeyCode = Keys.Enter Then
            '   this is the call on the routine that does the password checking
            TxtXSize_Validated(Nothing, Nothing)
        End If
    End Sub

    Private Sub TxtXSize_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtXSize.Validated
        'validate input value
        If Val(TxtXSize.Text) > 0 Then
            If Val(TxtYSize.Text) > 0 Then
                'use aoi envelope dimensions to estimate the row and column numbers 
                TxtCol.Text = Math.Ceiling(m_mapXOff / Val(TxtXSize.Text))
                TxtRow.Text = Math.Ceiling(m_mapYOff / Val(TxtYSize.Text))

                'update the HRU number by Row * Col
                TxtHRUNumber.Text = Math.Ceiling(m_AOIShapeArea / (Val(TxtXSize.Text) * Val(TxtYSize.Text)))
                BtnApply.Enabled = True
            Else
                BtnApply.Enabled = False
            End If
        Else
            BtnApply.Enabled = False
            MsgBox("Please enter a number larger than 0.")
        End If
    End Sub

    Private Sub TxtYSize_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TxtYSize.KeyDown
        If e.KeyCode = Keys.Enter Then
            '   this is the call on the routine that does the password checking
            TxtYSize_Validated(Nothing, Nothing)
        End If
    End Sub

    Private Sub TxtYSize_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles TxtYSize.Validated
        'validate input value
        If Val(TxtYSize.Text) > 0 Then
            If Val(TxtXSize.Text) > 0 Then
                'use aoi envelope dimensions to estimate the row and column numbers 
                TxtCol.Text = Math.Ceiling(m_mapXOff / Val(TxtXSize.Text))
                TxtRow.Text = Math.Ceiling(m_mapYOff / Val(TxtYSize.Text))

                'update the HRU number by Row * Col
                TxtHRUNumber.Text = Math.Ceiling(m_AOIShapeArea / (Val(TxtXSize.Text) * Val(TxtYSize.Text)))
                BtnApply.Enabled = True
            Else
                BtnApply.Enabled = False
            End If
        Else
            BtnApply.Enabled = False
            MsgBox("Please enter a number larger than 0.")
        End If
    End Sub

    Private Sub BtnTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnTest.Click
        'set argument values based on the ByType
        Dim returnCode As Long
        If RadBtnByHRUDimension.Enabled = True Then
            returnCode = BA_CreateFishnet(m_aoiPath, m_aoiPath, "DAFlow", m_mapX0, m_mapY0, _
                m_mapX0 + Val(TxtXSize.Text) * Val(TxtCol.Text), _
                m_mapY0 + Val(TxtYSize.Text) * Val(TxtRow.Text), Val(TxtCol.Text), Val(TxtRow.Text))
        Else
            returnCode = BA_CreateFishnet(m_aoiPath, m_aoiPath, "DAFlow", m_mapX0, m_mapY0, _
                m_mapX0 + m_mapXOff, m_mapY0 + m_mapYOff, Val(TxtCol.Text), Val(TxtRow.Text))
        End If

        If returnCode > 0 Then
            MsgBox(returnCode & " DAFlow Zones were created.")
        Else
            MsgBox("Error in DAFlow Zones Rule. Error Code: " & returnCode.ToString)
        End If

    End Sub

    Private Sub BtnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAbout.Click
        Dim toolHelpForm As FrmHelp = New FrmHelp(BA_HelpTopics.DAFlow)
        toolHelpForm.ShowDialog()
    End Sub

End Class