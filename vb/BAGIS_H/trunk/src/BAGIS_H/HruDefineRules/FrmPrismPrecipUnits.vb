Imports System.Text
Imports BAGIS_ClassLibrary

Public Class FrmPrismPrecipUnits

    Private m_units As MeasurementUnit
    Private m_parentForm As FrmPrismPrecipRule
    Private m_aoiPath As String

    Private Sub FrmPrismPrecipUnits_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sb As New StringBuilder
        sb.Append("The PRISM precipitation units have not been defined for this" & vbCrLf)
        sb.Append("AOI. Please make a selection from the choices below:")
        LblDescr.Text = sb.ToString

        m_units = MeasurementUnit.Missing

    End Sub

    Public Sub New(ByVal parentForm As FrmPrismPrecipRule, ByVal aoiPath As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        m_parentForm = parentForm
        m_parentForm.DataUnits = m_units
        m_aoiPath = aoiPath
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub BtnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnApply.Click
        m_units = MeasurementUnit.Inches
        If RdoMillimeters.Checked = True Then
            m_units = MeasurementUnit.Millimeters
        End If

        Dim prismParentPath As String = m_aoiPath & "\" & BA_EnumDescription(GeodatabaseNames.Prism)
        Dim prismFile As String = BA_GetPrismFolderName(CInt(AOIPrismFolderNames.annual) + 1)
        Dim propertyList As IList(Of String) = BA_ReadMetaData(prismParentPath, prismFile, LayerType.Raster, BA_XPATH_TAGS)
        If propertyList IsNot Nothing AndAlso propertyList.Count > 0 Then
            For Each pItem As String In propertyList
                'Need to work with a pre-existing BAGIS tag
                If pItem.IndexOf(BA_BAGIS_TAG_PREFIX) = 0 And pItem.IndexOf(BA_BAGIS_TAG_SUFFIX) > 0 Then
                    'Extract inner tags
                    Dim finalLength As Integer = pItem.Length - BA_BAGIS_TAG_PREFIX.Length - BA_BAGIS_TAG_SUFFIX.Length
                    Dim innerText As String = ""
                    If finalLength > 0 Then
                        innerText = pItem.Substring(BA_BAGIS_TAG_PREFIX.Length, finalLength)
                    End If
                    Dim pContents As String() = innerText.Split(";")
                    Dim updateCategory As Boolean = False
                    Dim updateValue As Boolean = False
                    Dim i As Integer = 0
                    For Each pString As String In pContents
                        'The zUnit category
                        If pString.IndexOf(BA_ZUNIT_CATEGORY_TAG) > -1 Then
                            'Overwrite existing value if there is one
                            pContents(i) = BA_ZUNIT_CATEGORY_TAG & MeasurementUnitType.Depth.ToString
                            updateCategory = True
                        ElseIf pString.IndexOf(BA_ZUNIT_VALUE_TAG) > -1 Then
                            'Overwrite existing value if there is one
                            pContents(i) = BA_ZUNIT_VALUE_TAG & m_units.ToString
                            updateValue = True
                        End If
                        i += 1
                    Next
                    If updateCategory = False Then
                        System.Array.Resize(pContents, pContents.Length + 1)
                        'Put the category in the old last position
                        pContents(pContents.GetUpperBound(0)) = BA_ZUNIT_CATEGORY_TAG & MeasurementUnitType.Depth.ToString & ";"
                     End If
                    If updateValue = False Then
                        System.Array.Resize(pContents, pContents.Length + 1)
                        'Put the value in the old last position
                        pContents(pContents.GetUpperBound(0)) = BA_ZUNIT_VALUE_TAG & m_units.ToString & ";"
                     End If
                    'Reassemble the updated innerText
                    Dim sb As StringBuilder = New StringBuilder()
                    sb.Append(BA_BAGIS_TAG_PREFIX)
                    For Each pString As String In pContents
                        If Not String.IsNullOrEmpty(pString) Then
                            sb.Append(pString & " ")
                        End If
                    Next
                    'Trim off trailing space
                    Dim newInnerText As String = sb.ToString
                    newInnerText = newInnerText.Remove(Len(newInnerText) - 1, 1)
                    newInnerText = newInnerText & BA_BAGIS_TAG_SUFFIX
                    BA_UpdateMetadata(prismParentPath, prismFile, LayerType.Raster, BA_XPATH_TAGS, newInnerText, BA_BAGIS_TAG_PREFIX.Length)
                End If
            Next
        Else
            'Build new innerText
            Dim sb As StringBuilder = New StringBuilder
            sb.Append(BA_BAGIS_TAG_PREFIX)
            sb.Append(BA_ZUNIT_CATEGORY_TAG & MeasurementUnitType.Depth.ToString & "; ")
            sb.Append(BA_ZUNIT_VALUE_TAG & m_units.ToString & "; ")
            sb.Append(BA_BAGIS_TAG_SUFFIX)
            BA_UpdateMetadata(prismParentPath, prismFile, LayerType.Raster, BA_XPATH_TAGS, sb.ToString, BA_BAGIS_TAG_PREFIX.Length)
        End If

        m_parentForm.DataUnits = m_units

        BtnCancel_Click(sender, e)
    End Sub
End Class