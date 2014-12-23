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
                Case BA_HelpTopics.AspectTemplate
                    description = "Aspect Template Rule converts aspect azimuth angle values into 4, 8, or 16 directions. A customizable majority filter can be applied to the direction map to remove isolated aspects within a kernel window. The more iterations the majority filter is applied the more generalized the final map is. See figure below."
                    illustration = My.Resources.AspectTemplateIllustration
                Case BA_HelpTopics.CanopyTemplate
                    description = "Canopy Template converts canopy percentage values into canopy classes based on user-defined percentage intervals. A customizable smooth filter and a majority filter can be applied to the percentage and percentage class maps to remove isolated canopy percentage cells within a kernel window. The more iterations the smooth and majority filters are applied the more generalized the final map is."
                Case BA_HelpTopics.ContributingArea
                    description = "Under construction... Contributing Area Rule Help."
                Case BA_HelpTopics.CookieCut
                    description = "Cookie-Cut Tool uses an input layer to clip out an existing HRU layer. The input layer could be an HRU layer or a raster layer. Users need to select a set of values of the input layer to define the spatial extent of the cutter. The output extent can be set as the AOI boundary or the cookie-cutter boundary. See figure below."
                    illustration = My.Resources.CookieCutIllustration
                Case BA_HelpTopics.DAFlow
                    description = "DAFlow Rule generates DAFlow style gridded zones using parameters specified by grid size or number. See figure below."
                    illustration = My.Resources.DAFlowRuleIllustration
                Case BA_HelpTopics.Eliminate
                    description = "Eliminate Tool eliminates small HRU polygons (zones) based on either a percentage of total polygons to be removed or a maximum area value. Users can use the Get Area from Map button to get the area threshold value from the map frame. Please note that the tool needs to be used recursively to remove all the polygons that meet the specified criteria."
                    illustration = My.Resources.EliminateIllustration
                Case BA_HelpTopics.ExportToWeasel
                    description = "Export to Weasel Tool exports the selected HRU layer from BAGIS-H's ArcGIS File Geodatabase to a GIS Weasel compatible ArcInfo GRID raster. This tool is for those who plan to use GIS Weasel to extract HRU parameters for hydrological modeling."
                Case BA_HelpTopics.IsoClustering
                    description = "Under construction... IsoClustering."
                Case BA_HelpTopics.LULCTemplate
                    description = "Land-Use and Land-Cover Template Rule groups land-use/land-cover classes into user-defined classes. A customizable majority filter can be applied to the input map to remove isolated classes within a kernel window. The more iterations the majority filter is applied the more generalized the final map is."
                Case BA_HelpTopics.PRISM
                    description = "PRISM Rule has a similar function as the Slice tool to group PRISM precipitation values into precipiation zones. Users can select monthly, quarterly, or annual precipitation as input or select a range of months and use their total precipiation as the input. See figure below."
                    illustration = My.Resources.PRISMRuleIllustration
                Case BA_HelpTopics.PRMSRadiation
                    description = "Under construction... PRMS Radiation statistics."
                Case BA_HelpTopics.Reclass, BA_HelpTopics.ReclassContinuous
                    description = "Reclass Rule reclassifies an input continuous or discrete raster layer into zones based on either user-specified intervals or a value look-up table. See figure below."
                    illustration = My.Resources.ReclassRuleIllustration
                Case BA_HelpTopics.Slice
                    description = "Slice Rule reclassifies an input continuous raster layer into zones based on either equal-area intervals or equal intervals. See figure below."
                    illustration = My.Resources.SliceRuleIllustration
                Case BA_HelpTopics.SlopeTemplate
                    description = "Slope Template Rule converts slope angle values (in degree or in percentage) into slope classes based on user-defined slope intervals. A customizable smooth filter and a majority filter can be applied to the slope angle and the slope class maps to remove isolated slope within a kernel window. The more iterations the smooth and majority filters are applied the more generalized the final map is. See figure below."
                    illustration = My.Resources.SlopeTemplateIllustration
                Case BA_HelpTopics.SoilTemplate
                    description = "Soil Template Rule groups soil classes into user-defined classes. A customizable majority filter can be applied to the input map to remove isolated classes within a kernel window. The more iterations the majority filter is applied the more generalized the final map is."
                Case BA_HelpTopics.Stamp
                    description = "Stamp Rule uses an input layer to update an existing HRU layer. The input layer could be an HRU layer or a raster layer. Users need to select a set of values of the input layer to define the spatial extent of the stamp tool. See figure below."
                    illustration = My.Resources.StampIllustration
                Case BA_HelpTopics.ZonalStatistics
                    description = "Zonal Statistics Tool generates zonal statistics on a set of user-selected input layers using an HRU zone layer to define the zones. The zonal statistics are added to the HRU's attribute table."
                Case BA_HelpTopics.SequentialId
                    description = "Sequential HRU ID Tool generates sequential ID numbers for the HRU zone dataset. The ID numbers are generated descending in steam links order (the higher the stream link value, the lower the id). This tool may be used to format data for input to the PRMS model."
                Case Else
                    description = "Help information not available."
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