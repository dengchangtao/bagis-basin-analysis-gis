Imports ESRI.ArcGIS.esriSystem
Imports ESRI.ArcGIS.Framework

Public Module ProgressIndicatorModule

    'the progress animation type to be used
    Private Const PROG_ANIMATION_TYPE As ESRI.ArcGIS.Framework.esriProgressAnimationTypes = ESRI.ArcGIS.Framework.esriProgressAnimationTypes.esriDownloadFile

    ' Creates and configures an IStepProgressor object
    Public Function BA_GetStepProgressor(ByVal int32_hWnd As System.Int32, ByVal maxRange As System.Int32) As IStepProgressor
        ' Create a CancelTracker
        Dim trackCancel As ITrackCancel = New ESRI.ArcGIS.Display.CancelTracker
        Dim progressDialogFactory As IProgressDialogFactory = New ProgressDialogFactory
        Dim pStepProg As IStepProgressor

        Try
            ' Create a step progressor
            pStepProg = progressDialogFactory.Create(trackCancel, int32_hWnd)
            ' Set the properties of the step progressor
            With pStepProg
                .Position = 1
                .MaxRange = maxRange
                '.Message = "Loading lists:"
                .StepValue = 1
            End With
            Return pStepProg
        Catch ex As Exception
            MsgBox("BA_GetStepProgressor() Exception: " & ex.Message)
            Return Nothing
        Finally
            trackCancel = Nothing
            progressDialogFactory = Nothing
            pStepProg = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try

    End Function

    'Creates and configures an IProgressDialog2 object from an IStepProgressor
    Public Function BA_GetProgressDialog(ByVal pStepProg As IStepProgressor, _
                                         ByVal pDescription As String, _
                                         ByVal pTitle As String) As IProgressDialog2
        Dim progressDialog2 As IProgressDialog2
        Try
            progressDialog2 = CType(pStepProg, IProgressDialog2) ' Explicit Cast
            ' Set the properties of the ProgressDialog
            With progressDialog2
                .CancelEnabled = True
                .Description = pDescription
                .Title = pTitle
                .Animation = PROG_ANIMATION_TYPE
            End With
            Return progressDialog2
        Catch ex As Exception
            MsgBox("BA_GetProgressDialog Exception: " & ex.Message)
            Return Nothing
        Finally
            progressDialog2 = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

    'Creates an IProgressDialog2 object without a progressor
    Public Function BA_GetAnimationProgressor(ByVal int32_hWnd As System.Int32, ByVal pDescription As String, _
                                              ByVal pTitle As String) As IProgressDialog2
        Dim trackCancel As ITrackCancel = New ESRI.ArcGIS.Display.CancelTracker
        Dim pProgDFact As IProgressDialogFactory = New ProgressDialogFactory
        ' Create/configure the ProgressDialog. This automatically displays the dialog
        Dim progressDialog2 As IProgressDialog2 = pProgDFact.Create(trackCancel, int32_hWnd)
        Dim pProg As IProgressor = CType(progressDialog2, IProgressor)

        Try
            ' Set the properties of the ProgressDialog
            pProg.Hide()
            With progressDialog2
                .CancelEnabled = True
                .Description = pDescription
                .Title = pTitle
                .Animation = ESRI.ArcGIS.Framework.esriProgressAnimationTypes.esriProgressSpiral
            End With
            Return progressDialog2
        Catch ex As Exception
            MsgBox("BA_GetAnimationProgressor Exception: " & ex.Message)
            Return Nothing
        Finally
            progressDialog2 = Nothing
            trackCancel = Nothing
            pProgDFact = Nothing
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try
    End Function

End Module
