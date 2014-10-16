Imports System.Windows.Forms
Imports BAGIS_ClassLibrary

Public Class FrmHelp

    Private Sub BtnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnOK.Click
        Me.Close()
    End Sub

    Public Sub New(ByVal topic As BA_HelpTopics)
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        SetForm(topic)
    End Sub

    Private Sub SetForm(ByVal topic As BA_HelpTopics)
        Dim description As String = ""
        Dim illustration As System.Drawing.Image = Nothing

        Try
            Select Case topic
                Case BA_HelpTopics.TimberlineTool
                    description = "This tool allows users to specify a timberline elevation for each HRU zone in the AOI. The timberline values can be used to assign the Snowmelt Depletion Curve (hru_deplcrv) parameter values used in PRMS. This tool saves the timberline elevation values in the attribute table of the vector HRU featureclass (grid_zones_v). The timberline elevation will be recorded only in the attribute table of this featureclass and not be added to the exported OMS parameter files. A timberline elevation value of 0 indicates that the HRU is a regular HRU (not above timberline). The actual calculation of the hru_deplcrv is done by the hru_deplcrv_treeline parameter models built in ArcGIS ModelBuilder. Users can either use the timberline elevation extracted in this tool or the Landfire vegetation layer to set the values of hru_deplcrv. If the area of an HRU is more then 50% above timberline elevation or not covered by canopy vegetation, then the hru_deplcrv is set to 2, otherwise 1."
            End Select
            LblDescription.Text = description
            If illustration IsNot Nothing Then
                PictureBox.Image = illustration
                PictureBox.SizeMode = PictureBoxSizeMode.Normal
            End If
        Catch ex As Exception
            MsgBox("An error has occurred" & Chr(13) & Chr(13) & ex.Message)
        End Try
    End Sub
End Class