Imports ESRI.ArcGIS.Geoprocessor
Imports ESRI.ArcGIS.esriSystem

Public Class FrmSnodas

    Private Sub BtnCancel_Click(sender As System.Object, e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub BtnApply_Click(sender As System.Object, e As System.EventArgs) Handles BtnApply.Click
        ' Initialize the geoprocessor.
        Dim GP As Geoprocessor = New Geoprocessor()
        Dim parameters As IVariantArray = New VarArrayClass()
        Dim result As ESRI.ArcGIS.Geoprocessing.IGeoProcessorResult

        Try
            ' Add the BestPath toolbox.
            'GP.AddToolbox("http://flame7/arcgis/services;GP/Bestpathtoolbox")
            GP.AddToolbox("http://atlas.geog.pdx.edu/arcgis/services/SNODAS_Utilities;GPTestReplaceLayer")

            ' Inputs reference layers in a map document on the server.
            parameters.Add("02_PrismPrecip.img")

            ' Execute the server tool by reference.
            result = CType(GP.Execute("GPTestReplaceLayer", parameters, Nothing), ESRI.ArcGIS.Geoprocessing.IGeoProcessorResult)
            If result.MessageCount > 0 Then
                For Count As Integer = 0 To result.MessageCount - 1
                    Debug.Print(result.GetMessage(Count))
                Next Count
            End If
        Catch ex As Exception
            MsgBox("Exception: " & ex.Message)
        Finally
            GP = Nothing
            parameters = Nothing
            result = Nothing
        End Try

    End Sub
End Class