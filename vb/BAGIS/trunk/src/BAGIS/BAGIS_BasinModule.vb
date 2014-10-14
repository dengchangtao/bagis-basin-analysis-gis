Imports BAGIS_ClassLibrary
Imports System.ComponentModel
Imports ESRI.ArcGIS.ArcMapUI
Imports ESRI.ArcGIS.Framework
Imports System.Windows.Forms
Imports ESRI.ArcGIS.esriSystem

Module BAGIS_BasinModule
    Public Function BA_ExportBasinToFileGdb(ByVal pHWnd As Integer, ByVal BainPath As String, _
                                       ByVal pDocument As IMxDocument) As BA_ReturnCode
        ' Create/configure a step progressor
        Dim pStepProg As IStepProgressor = BA_GetStepProgressor(pHWnd, 50)
        pStepProg.Show()
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = BA_GetProgressDialog(pStepProg, "", "Converting weasel layers...")
        progressDialog2.ShowDialog()

        Try
            pStepProg.Step()
            'Check to see if all 5 BAGIS geodatabases exist
            Dim existsList As List(Of String) = BA_CheckForBagisGDB(BainPath)
            For Each gdbPath As String In existsList
                'Delete if they do
                Dim gdbSuccess As BA_ReturnCode = BA_DeleteGeodatabase(gdbPath, pDocument)
                If gdbSuccess <> BA_ReturnCode.Success Then
                    MessageBox.Show("Unable to delete folder '" & gdbPath & "'. Please restart ArcMap and try again", "Unable to delete folder", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return BA_ReturnCode.WriteError
                End If
            Next

            pStepProg.Message = "Creating file geodatabase folders"
            pStepProg.Step()
            Dim success As BA_ReturnCode = BA_CreateGeodatabaseFolders(BainPath, FolderType.BASIN)
            If success = BA_ReturnCode.Success Then
                pStepProg.Message = "Gathering information on files to copy"
                pStepProg.Step()
                Dim bTime As DateTime = DateTime.Now
                Dim rasterNamesTable As Hashtable = New Hashtable
                Dim vectorNamesTable As Hashtable = New Hashtable

                'Add surfaces to the rasterNamesTable
                BA_UpdateHashtableForSurfaces(BainPath, rasterNamesTable)

                'Here we copy over all the aoi layers at the top level
                Dim AOIVectorList() As String = Nothing
                Dim AOIRasterList() As String = Nothing
                BA_ListLayersinAOI(BainPath, AOIRasterList, AOIVectorList)
                BA_UpdateHashtableForRasterCopy(rasterNamesTable, BainPath, BainPath & "\" & BA_EnumDescription(GeodatabaseNames.Aoi), AOIRasterList)
                BA_UpdateHashtableForVectorCopy(vectorNamesTable, BainPath, BainPath & "\" & BA_EnumDescription(GeodatabaseNames.Aoi), AOIVectorList)

                pStepProg.Message = "Initializing geoprocessor"
                pStepProg.Step()
                If success = BA_ReturnCode.Success Then
                    success = BA_CopyRastersToGDB(rasterNamesTable, pStepProg)
                    'Debug.Print("Raster count: " & rasterNamesTable.Count)
                    If success = BA_ReturnCode.Success Then
                        success = BA_CopyVectorsToGDB(vectorNamesTable, pStepProg)
                        'Debug.Print("Vector count: " & vectorNamesTable.Count)
                        pStepProg.Step()
                        If success = BA_ReturnCode.Success Then
                            success = BA_RenameAoiBoundary(BainPath)
                        End If
                    End If
                End If
            End If
            Return success
        Catch ex As Exception
            Debug.Print("BA_ExportBasinToFileGdb Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            ' Clean up step progressor
            pStepProg.Hide()
            progressDialog2.HideDialog()
            pStepProg = Nothing
            progressDialog2 = Nothing
        End Try

    End Function
End Module
