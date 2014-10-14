Imports ESRI.ArcGIS.Desktop.AddIns
Imports System.IO
Imports BAGIS_ClassLibrary
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.DataSourcesFile
Imports ESRI.ArcGIS.Carto

Public Class BtnAddRefLayers
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button
    Public settingsform As frmSettings = New frmSettings
    Public Sub New()
        BA_SetSettingPath()
        If Len(BA_Settings_Filepath) = 0 Then
            MsgBox("ERROR! BA_Read_Settings: Cannot retrieve the file path and name of the definition file.")
        End If
        Dim settings_message As String = BA_Read_Settings(settingsform)
        BA_SystemSettings.listCount = settingsform.lstLayers.Items.Count
    End Sub
    Public WriteOnly Property selectedProperty As Boolean
        Set(ByVal value As Boolean)
            Me.Enabled = value
        End Set
    End Property

    Protected Overrides Sub OnClick()
        Dim terrainRef As String = BA_SystemSettings.Ref_Terrain
        Dim DrainageRef As String = BA_SystemSettings.Ref_Drainage
        Dim watershedRef As String = BA_SystemSettings.Ref_Watershed
        Dim pourpointRef As String = BA_SystemSettings.PourPointLayer
        'check if pourpoint file exists
        Dim ppointpath As String = "Please Return"
        Dim layertype As String = ""
        Dim pplayername As String = BA_GetBareNameAndExtension(pourpointRef, ppointpath, layertype)
        pourpointRef = ppointpath & pplayername
        If Len(pourpointRef) > 0 Then 'it's OK to not have a specified reference layer
            If Not BA_Shapefile_Exists(pourpointRef) Then
                MsgBox("Pourpoint layer does not exist: " & pourpointRef)
                pourpointRef = ""
            End If
        End If
        BA_LoadReferenceLayers(terrainRef, DrainageRef, watershedRef, pourpointRef)
        If String.IsNullOrEmpty(DrainageRef) And Not String.IsNullOrEmpty(watershedRef) And Not String.IsNullOrEmpty(terrainRef) Then
            MsgBox("No reference layer is specified in the settings")
        End If
        'Dim SaveAOIMXDButton = AddIn.FromID(Of BtnSaveAOIMXD)(My.ThisAddIn.IDs.BtnSaveAOIMXD)
        'SaveAOIMXDButton.selectedProperty = True
    End Sub
    
    Protected Overrides Sub OnUpdate()
    End Sub

End Class
