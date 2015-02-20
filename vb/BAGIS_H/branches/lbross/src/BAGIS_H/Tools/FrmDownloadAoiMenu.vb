Imports System.Windows.Forms
Imports System.Net
Imports System.Text

Public Class FrmDownloadAoiMenu

    Private m_token As BagisToken = New BagisToken
    'In practice the user name/password will be provided by the user
    Private m_userName As String = "testUser"
    Private m_password As String = "CSARBasins2015"

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        '---create a row---
        Dim item As New DataGridViewRow
        item.CreateCells(AoiGrid)
        With item
            .Cells(0).Value = "Price_R_at_Woodside_01232014"
            .Cells(1).Value = "23-JAN-2014"
            .Cells(2).Value = "G. Duh"
            .Cells(3).Value = False
            .Cells(4).Value = "Updated AOI with new gauge station"
        End With
        Dim item2 As New DataGridViewRow
        item2.CreateCells(AoiGrid)
        With item2
            .Cells(0).Value = "Santa_Fe_R_nr_Santa_Fe_11302012"
            .Cells(1).Value = "11-NOV-2012"
            .Cells(2).Value = "D. Garen"
            .Cells(3).Value = False
            .Cells(4).Value = "Initial upload of AOI; Includes HRU definition"
        End With
        '---add the row---
        '@ToDo: Temporarily stop populating form
        'AoiGrid.Rows.Add(item)
        'AoiGrid.Rows.Add(item2)
        AoiGrid.ClearSelection()
        AoiGrid.CurrentCell = Nothing

        'Check for token
        m_token.token = SecurityModule.GetStoredToken
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub BtnDownloadAoi_Click(sender As System.Object, e As System.EventArgs) Handles BtnDownloadAoi.Click
        If String.IsNullOrEmpty(m_token.token) Then
            Dim strToken As String = SecurityModule.GetServerToken(m_userName, m_password, TxtBasinsDb.Text & "api-token-auth/")
            m_token.token = strToken
            If String.IsNullOrEmpty(strToken) Then
                MessageBox.Show("Invalid user name or password. Failed to connect to database.", "Failed Connection", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
        End If

        Dim reqT As HttpWebRequest
        Dim resT As HttpWebResponse
        'The end point for getting a token for the web service
        reqT = WebRequest.Create(TxtBasinsDb.Text & "AOI/")
        'This is a GET request
        reqT.Method = "GET"

        'Retrieve the token and format it for the header; Token comes from caller
        Dim cred As String = String.Format("{0} {1}", "Token", m_token.token)
        'Put token in header
        reqT.Headers(HttpRequestHeader.Authorization) = cred

        Try
            resT = CType(reqT.GetResponse(), HttpWebResponse)
            'Printing the response to the Console for testing
            Using SReader As System.IO.StreamReader = New System.IO.StreamReader(resT.GetResponseStream)
                Debug.Print(SReader.ReadToEnd())
            End Using
        Catch ex As WebException
            Debug.Print("Exception: " & ex.Message)
        End Try
    End Sub

    'Testing how to connect to an unsecured connection
    Protected Sub TestConnection()
        Dim reqT As HttpWebRequest
        Dim resT As HttpWebResponse
        reqT = WebRequest.Create("http://basins.geog.pdx.edu/bagis_test/test.html")
        reqT.Method = "GET"
        Try
            'Dim dataStreamT As System.IO.Stream = reqT.GetRequestStream()
            resT = CType(reqT.GetResponse(), HttpWebResponse)
            Dim responseStream As New System.IO.StreamReader(resT.GetResponseStream(), Encoding.UTF8)
            Dim result As String = responseStream.ReadToEnd()
            Debug.Print(result)
        Catch ex As WebException
            MsgBox(ex.InnerException)
        End Try
    End Sub

End Class