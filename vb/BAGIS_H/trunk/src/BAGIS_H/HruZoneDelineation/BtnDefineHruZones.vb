Imports ESRI.ArcGIS.esriSystem
Imports BAGIS_ClassLibrary

Public Class BtnDefineHruZones
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public Sub New()

    End Sub

    Protected Overrides Sub OnClick()
        ' Check to see if spatial analyst is enabled, if not throw an error
        If BA_Enable_SAExtension(My.ArcMap.Application) = ESRI.ArcGIS.esriSystem.esriExtensionState.esriESEnabled Then
            Dim dockWindow As ESRI.ArcGIS.Framework.IDockableWindow
            Dim dockWinID As UID = New UIDClass()
            dockWinID.Value = My.ThisAddIn.IDs.frmHruZone
            dockWindow = My.ArcMap.DockableWindowManager.GetDockableWindow(dockWinID)
            ' Get handle to UI (form) to reload lists
            Dim dockWindowAddIn = ESRI.ArcGIS.Desktop.AddIns.AddIn.FromID(Of frmHruZone.AddinImpl)(My.ThisAddIn.IDs.frmHruZone)
            Dim hruZoneForm As frmHruZone = dockWindowAddIn.UI
            Dim aboutToShow As Boolean = Not dockWindow.IsVisible()
            If aboutToShow Then
                'Set AOI if already set
                Dim hruExt As HruExtension = HruExtension.GetExtension
                Dim aoi As Aoi = hruExt.aoi
                If aoi IsNot Nothing Then
                    'Look for saved rules file; Load rule info from xml if it exists in aoi
                    Dim xmlOutputPath As String = aoi.FilePath & BA_EnumDescription(PublicPath.RulesXml)
                    Dim pHru As Hru = Nothing
                    ' Open old rules file if there is one
                    If BA_File_ExistsWindowsIO(xmlOutputPath) Then
                        pHru = BA_LoadRulesFromXml(aoi.FilePath)
                        'Only use the rules if the version hasn't changed
                        If pHru IsNot Nothing Then
                            If pHru.ApplicationVersion <> hruExt.version Then
                                pHru = Nothing
                            Else
                                'Delete the rule file if version has changed
                                BA_Remove_File(xmlOutputPath)
                            End If
                        End If
                    End If

                    If aoi.Name = hruZoneForm.Name Then
                        If pHru IsNot Nothing Then
                            'Pre-set AOI is the same as form AOI, user saved the rules rules still exist on form
                            hruZoneForm.ReloadListLayers()
                            hruZoneForm.ReloadParentTemplate(pHru)
                        Else
                            hruZoneForm.ResetForm()
                        End If
                    Else
                        'Pre-set AOI is different from form AOI
                        hruZoneForm.ResetForm(aoi)
                        dockWindow.Caption = "Current AOI: " & aoi.Name & aoi.ApplicationVersion
                        hruZoneForm.TxtAoiPath.Text = aoi.FilePath
                        'Load rules file if it exists and is valid
                        If pHru IsNot Nothing Then
                            hruZoneForm.ReloadRules(pHru)
                        End If
                    End If

                    'Check to make sure the units are set in the metadata before proceeding
                    Dim slopeUnit As SlopeUnit
                    Dim elevUnit As MeasurementUnit
                    Dim depthUnit As MeasurementUnit    'prism data
                    BA_GetMeasurementUnitsForAoi(aoi.FilePath, slopeUnit, elevUnit, depthUnit)
                    If slopeUnit = slopeUnit.Missing Or _
                       elevUnit = MeasurementUnit.Missing Or _
                       depthUnit = MeasurementUnit.Missing Then
                        Dim frmDataUnits As FrmDataUnits = New FrmDataUnits(aoi, slopeUnit, elevUnit, depthUnit)
                        frmDataUnits.ShowDialog()
                    End If

                End If
            End If

            'Toggle dockable window
            dockWindow.Show(aboutToShow)
            ' Set dimensions of dockable window
            Dim windowPos As ESRI.ArcGIS.Framework.IWindowPosition = CType(dockWindow, ESRI.ArcGIS.Framework.IWindowPosition)
            windowPos.Height = 605
            windowPos.Width = 675
        Else
            Windows.Forms.MessageBox.Show("Spatial Analyst is required for HRU delineation and is not available.")
        End If

    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub
End Class
