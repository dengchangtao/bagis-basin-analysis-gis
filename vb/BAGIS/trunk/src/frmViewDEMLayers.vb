Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.Display
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework
Imports ESRI.ArcGIS.ArcMapUI
Imports System.Windows.Forms
Imports ESRI.ArcGIS.Carto

Public Class frmViewDEMLayers

    Private _aoiLabel As String

    Private Sub CmdSelectAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSelectAll.Click
        SetAllCheckBoxes(True)
    End Sub

    Private Sub SetAllCheckBoxes(ByVal Value As Boolean)
        ChkFilledDEM.Checked = Value
        ChkFlowDir.Checked = Value
        ChkFlowAccum.Checked = Value
        ChkSlope.Checked = Value
        ChkAspect.Checked = Value
        ChkHillshade.Checked = Value
        If ChkPourPoint.Enabled Then ChkPourPoint.Checked = Value
    End Sub

    Private Sub CmdSelectNone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdSelectNone.Click
        SetAllCheckBoxes(False)
    End Sub

    Private Sub CmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdCancel.Click
        Me.Close()
    End Sub

    Dim m_labelText As String
    Dim myBasinPath As String

    Sub New(ByVal labelText As String, ByVal FolderPath As String)

        MyBase.New()
        InitializeComponent()
        Try
            'Set the form level variable to the labelText from the caller
            m_labelText = labelText
            myBasinPath = FolderPath
        Catch ex As Exception
            MsgBox("Unknown Error", ex.Message)
        End Try
    End Sub

    ' Private frmBasinTool As frmBasin_Tool
    Private Sub CmdContinue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CmdContinue.Click
        'Dim IsAOI As Boolean = False
        'Dim hasPourPoint As Boolean = False

        'check if the selected folder is an AOI folder, only an AOI has the pourpoint layer
        'If m_labelText = "Yes" Then
        '    hasPourPoint = True
        'End If

        'Me.Tag = myBasinPath
        'If Len(Trim(Me.Tag)) = 0 Then 'the form wasn't opened correctly by the parent process
        '    MsgBox("No path is correctly specified!")
        '    Exit Sub
        'End If

        Dim response As Integer
        Dim strParentName As String = Nothing
        'Dim baseFolderPath As String = Me.Tag
        Me.Hide()
        'display filled DEM
        Dim strOutputPath As String
        Dim folderBase As String = myBasinPath

        strOutputPath = folderBase & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\" & BA_EnumDescription(MapsFileName.filled_dem_gdb)
        If ChkFilledDEM.Checked Then
            If BA_File_Exists(strOutputPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                response = BA_DisplayRaster(My.ArcMap.Application, strOutputPath)
                If response < 0 Then 'give warning if error encountered
                    MsgBox("Cannot open the DEM raster!")
                End If
            End If
        End If

        'display Slope
        strOutputPath = folderBase & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\" & BA_EnumDescription(MapsFileName.slope_gdb)
        If ChkSlope.Checked Then
            If BA_File_Exists(strOutputPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                response = BA_DisplayRaster(My.ArcMap.Application, strOutputPath)
                If response < 0 Then 'give warning if error encountered
                    MsgBox("Cannot open the Slope raster!")
                End If
            End If
        End If

        'display Aspect
        strOutputPath = folderBase & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\" & BA_EnumDescription(MapsFileName.aspect_gdb)
        If ChkAspect.Checked Then
            If BA_File_Exists(strOutputPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                response = BA_DisplayRaster(My.ArcMap.Application, strOutputPath)
                If response < 0 Then 'give warning if error encountered
                    MsgBox("Cannot open the Aspect raster!")
                End If
            End If
        End If

        'display Flow Direction
        strOutputPath = folderBase & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\" & BA_EnumDescription(MapsFileName.flow_direction_gdb)
        If ChkFlowDir.Checked Then
            If BA_File_Exists(strOutputPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                response = BA_DisplayRaster(My.ArcMap.Application, strOutputPath)
                If response < 0 Then 'give warning if error encountered
                    MsgBox("Cannot open the Flow Direction raster!")
                End If
            End If
        End If

        'display Flow Accumulation
        strOutputPath = folderBase & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\" & BA_EnumDescription(MapsFileName.flow_accumulation_gdb)
        If ChkFlowAccum.Checked Then
            If BA_File_Exists(strOutputPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                response = BA_DisplayRaster(My.ArcMap.Application, strOutputPath) ' & "\grid", BasinLayerDisplayNames(6))
                If response < 0 Then 'give warning if error encountered
                    MsgBox("Cannot open the Flow Accumulation raster!")
                End If
            End If
        End If

        'display Hillshade
        strOutputPath = folderBase & "\" & BA_EnumDescription(GeodatabaseNames.Surfaces) & "\" & BA_EnumDescription(MapsFileName.hillshade_gdb)
        If ChkHillshade.Checked Then
            If BA_File_Exists(strOutputPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTRasterDataset) Then
                response = BA_DisplayRaster(My.ArcMap.Application, strOutputPath) '   "\grid", BasinLayerDisplayNames(7))
                If response < 0 Then 'give warning if error encountered
                    MsgBox("Cannot open the Hillshade raster!")
                End If
            End If
        End If

        'display pourpoint of an AOI
        If ChkPourPoint.Checked = True Then
            Dim strPPointPath As String = folderBase & "\" & BA_EnumDescription(GeodatabaseNames.Aoi) & "\" & BA_POURPOINTCoverage
            If BA_File_Exists(strPPointPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
                Dim pMColor As IRgbColor
                pMColor = New RgbColor
                pMColor.RGB = RGB(0, 255, 255)
                Dim symbology As BA_Map_Symbology = BA_GetPointMapSymbology(BA_POURPOINTCoverage)
                response = BA_MapDisplayPointMarkers(My.ArcMap.Application, strPPointPath, symbology.DisplayName, symbology.Color, symbology.MarkerType)
                'response = BA_DisplayPointMarkers(strOutputPath, pourPoint, pourPoint, pMColor)
                If response < 0 Then 'give warning if error encountered
                    MsgBox("Cannot open the pourpoint shapefile!")
                End If
            End If
        End If

        Dim pMxDoc As IMxDocument = My.ArcMap.Application.Document
        Dim pMap As IMap = pMxDoc.FocusMap
        pMxDoc.ActiveView.Refresh()

        GC.WaitForPendingFinalizers()
        GC.Collect()
        Me.Close()
    End Sub

    Private Sub frmViewDEMLayers_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CmdContinue.Enabled = False
        Dim haspourpoint As Boolean = False
        Dim folderBase As String = ""

        If Not String.IsNullOrEmpty(myBasinPath) Then
            folderBase = myBasinPath
        Else
            MsgBox("The BASIN or AOI folder was not set correctly!")
        End If

        Dim strPPointPath As String = folderBase & "\" & BA_EnumDescription(GeodatabaseNames.Aoi) & "\" & BA_POURPOINTCoverage
        If BA_File_Exists(strPPointPath, WorkspaceType.Geodatabase, esriDatasetType.esriDTFeatureClass) Then
            haspourpoint = True
        End If

        If haspourpoint Then
            ChkPourPoint.Enabled = True
        Else
            ChkPourPoint.Enabled = False
        End If
    End Sub

    Private Sub ChkFilledDEM_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkFilledDEM.CheckedChanged
        If ChkAspect.Checked Or ChkFilledDEM.Checked Or ChkFlowAccum.Checked Or ChkFlowDir.Checked Or ChkHillshade.Checked Or ChkPourPoint.Checked Or ChkSlope.Checked = True Then
            CmdContinue.Enabled = True
        Else
            CmdContinue.Enabled = False
        End If
    End Sub

    Private Sub ChkFlowDir_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkFlowDir.CheckedChanged
        If ChkAspect.Checked Or ChkFilledDEM.Checked Or ChkFlowAccum.Checked Or ChkFlowDir.Checked Or ChkHillshade.Checked Or ChkPourPoint.Checked Or ChkSlope.Checked = True Then
            CmdContinue.Enabled = True
        Else
            CmdContinue.Enabled = False
        End If
    End Sub

    Private Sub ChkFlowAccum_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkFlowAccum.CheckedChanged
        If ChkAspect.Checked Or ChkFilledDEM.Checked Or ChkFlowAccum.Checked Or ChkFlowDir.Checked Or ChkHillshade.Checked Or ChkPourPoint.Checked Or ChkSlope.Checked = True Then
            CmdContinue.Enabled = True
        Else
            CmdContinue.Enabled = False
        End If
    End Sub

    Private Sub ChkSlope_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkSlope.CheckedChanged
        If ChkAspect.Checked Or ChkFilledDEM.Checked Or ChkFlowAccum.Checked Or ChkFlowDir.Checked Or ChkHillshade.Checked Or ChkPourPoint.Checked Or ChkSlope.Checked = True Then
            CmdContinue.Enabled = True
        Else
            CmdContinue.Enabled = False
        End If
    End Sub

    Private Sub ChkAspect_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkAspect.CheckedChanged
        If ChkAspect.Checked Or ChkFilledDEM.Checked Or ChkFlowAccum.Checked Or ChkFlowDir.Checked Or ChkHillshade.Checked Or ChkPourPoint.Checked Or ChkSlope.Checked = True Then
            CmdContinue.Enabled = True
        Else
            CmdContinue.Enabled = False
        End If
    End Sub

    Private Sub ChkHillshade_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkHillshade.CheckedChanged
        If ChkAspect.Checked Or ChkFilledDEM.Checked Or ChkFlowAccum.Checked Or ChkFlowDir.Checked Or ChkHillshade.Checked Or ChkPourPoint.Checked Or ChkSlope.Checked = True Then
            CmdContinue.Enabled = True
        Else
            CmdContinue.Enabled = False
        End If
    End Sub

    Private Sub ChkPourPoint_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChkPourPoint.CheckedChanged
        If ChkAspect.Checked Or ChkFilledDEM.Checked Or ChkFlowAccum.Checked Or ChkFlowDir.Checked Or ChkHillshade.Checked Or ChkPourPoint.Checked Or ChkSlope.Checked = True Then
            CmdContinue.Enabled = True
        Else
            CmdContinue.Enabled = False
        End If
    End Sub
End Class