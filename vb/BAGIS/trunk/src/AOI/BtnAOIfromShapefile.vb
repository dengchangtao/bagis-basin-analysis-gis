Imports ESRI.ArcGIS.CatalogUI
Imports ESRI.ArcGIS.Catalog
Imports ESRI.ArcGIS.Geodatabase
Imports BAGIS_ClassLibrary
Imports System.Windows.Forms
Imports ESRI.ArcGIS.Geometry
Imports ESRI.ArcGIS.DataSourcesGDB

Public Class BtnAOIfromShapefile
    Inherits ESRI.ArcGIS.Desktop.AddIns.Button

    Public WriteOnly Property selectedProperty As Boolean
        Set(ByVal value As Boolean)
            Me.Enabled = value
        End Set
    End Property

    Protected Overrides Sub OnClick()
        Dim bObjectSelected As Boolean = False
        Dim pGxDialog As IGxDialog = New GxDialog
        Dim pGxObject As IEnumGxObject = Nothing
        Dim Data_Path As String, Data_Name As String, data_type As Object
        Dim data_fullname As String = ""

        If BA_Enable_SAExtension(My.ArcMap.Application) <> ESRI.ArcGIS.esriSystem.esriExtensionState.esriESEnabled Then
            Windows.Forms.MessageBox.Show("Spatial Analyst is required for BAGIS and is not available. Program stopped.")
            Exit Sub
        End If

        Dim pFilter As IGxObjectFilter
        pFilter = New GxFilterShapefiles

        'initialize and open mini browser
        With pGxDialog
            .AllowMultiSelect = False
            .ButtonCaption = "Select"
            .Title = "Select a polygon shapefile"
            .ObjectFilter = pFilter
            bObjectSelected = .DoModalOpen(My.ArcMap.Application.hWnd, pGxObject)
        End With

        If bObjectSelected = False Then Exit Sub

        'get the name of the selected shapefile
        Dim pGxDataset As IGxDataset = pGxObject.Next
        Dim pDatasetName As IDatasetName = pGxDataset.DatasetName
        Data_Path = pDatasetName.WorkspaceName.PathName
        Data_Name = pDatasetName.Name
        data_type = pDatasetName.Type

        'Set Data Type Name from Data Type
        If data_type = ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureDataset Or _
            data_type = ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureClass Then

            'check geometrytyp, only polygon layer is allowed.
            Dim projText As String = BA_GetProjectionString(Data_Path & "\" & BA_StandardizeShapefileName(Data_Name, True, False))
            MsgBox("The input shapefile must be in the same projection as the source DEM specified in the BAGIS settings!" & _
                   vbCrLf & "The projection of the selected shapefile is: " & projText, MsgBoxStyle.Information)

            Dim pInputFClass As IFeatureClass = BA_OpenFeatureClassFromFile(Data_Path, Data_Name)
            If pInputFClass IsNot Nothing Then
                If pInputFClass.ShapeType = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon Then
                    Dim frmCreateAOIfromBND As frmCreateAOIfromExistingBND = New frmCreateAOIfromExistingBND
                    frmCreateAOIfromBND.Show()
                    frmCreateAOIfromBND.txtSourceData.Text = Data_Path & "\" & BA_StandardizeShapefileName(Data_Name, True, False)
                    frmCreateAOIfromBND.txtOutputName.Text = BA_StandardizeShapefileName(Data_Name, False, False)
                Else
                    MsgBox("Please select a polygon shapefile as the input!")
                End If
                pInputFClass = Nothing
            Else
                MsgBox("Please select a polygon shapefile as the input!")
            End If

        Else
            MsgBox("Please select a polygon shapefile as the input!")
        End If

    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub

    'Extract the datum string from an IspatialReference
    Private Function BA_ProjectionString(ByVal pSpRef As ISpatialReference) As String
        If pSpRef IsNot Nothing Then
            ' Explicit cast
            Dim parameterExport1 As IESRISpatialReferenceGEN2 = CType(pSpRef, IESRISpatialReferenceGEN2)
            Dim buffer As String = Nothing
            Dim bytes As Long = Nothing
            parameterExport1.ExportToESRISpatialReference2(buffer, bytes)
            Dim datumPos As Integer = InStr(buffer, "PROJCS")
            Dim primePos As Integer = InStr(buffer, "GEOGCS")
            Return buffer.Substring(datumPos + 7, primePos - datumPos - 10)
        End If
        Return Nothing
    End Function

    Private Function BA_HaveSameDatum(ByVal layerPathName1 As String, ByVal layerPathName2 As String) As Boolean
        Try
            Dim datumStr1 As String = BA_GetProjectionString(layerPathName1)
            Dim datumStr2 As String = BA_GetProjectionString(layerPathName2)

            MsgBox(layerPathName1 & " --> " & datumStr1 & vbCrLf & layerPathName2 & " --> " & datumStr2)

            If (String.Compare(datumStr1, datumStr2) = 0) Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            MessageBox.Show("BA_HaveSameDatum Exception: " & ex.Message)
            Return False
        End Try
    End Function

End Class
