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
    End Sub

    Private Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub BtnDownloadAoi_Click(sender As System.Object, e As System.EventArgs) Handles BtnDownloadAoi.Click
        GetToken()
        QueryRestRoot()
    End Sub

    Protected Sub GetToken()
        Dim reqT As HttpWebRequest
        Dim resT As HttpWebResponse
        'The end point for getting a token from the web service
        Dim url As String = TxtBasinsDb.Text & "api-token-auth/"
        reqT = WebRequest.Create(url)
        'Needs to be a POST request to get a token
        reqT.Method = "POST"

        'These are the field/value pairs that would be on an html form
        Dim Data As String = "username=" & m_userName & "&password=" & m_password
        'Encode them to Byte format to include with the request
        Dim credArray As Byte() = Encoding.UTF8.GetBytes(Data)
        'We are sending a form
        reqT.ContentType = "application/x-www-form-urlencoded"
        reqT.ContentLength = credArray.Length
        Try
            '@ToDo: Workaround for certificate error; This should come out when the certificate issue is fixed
            ServicePointManager.ServerCertificateValidationCallback = New System.Net.Security.RemoteCertificateValidationCallback(AddressOf AcceptAllCertifications)
            'Intercept the httpRequest so we can add the user name/password
            Dim dataStreamT As System.IO.Stream = reqT.GetRequestStream()
            dataStreamT.Write(credArray, 0, credArray.Length)
            dataStreamT.Close()
            'Send the request and wait for response
            resT = CType(reqT.GetResponse(), HttpWebResponse)
            'Convert the JSON response to a BagisTokenn object
            Dim ser As System.Runtime.Serialization.Json.DataContractJsonSerializer = New System.Runtime.Serialization.Json.DataContractJsonSerializer(m_token.[GetType]())
            '@ToDo: Storing the token in the form but it should also be saved on disk and in the extension
            m_token = CType(ser.ReadObject(resT.GetResponseStream), BagisToken)
        Catch ex As WebException
            MsgBox(ex.InnerException)
        End Try
    End Sub

    Public Sub QueryRestRoot()
        Dim reqT As HttpWebRequest
        Dim resT As HttpWebResponse
        'The end point for getting a token for the web service
        reqT = WebRequest.Create(TxtBasinsDb.Text)
        'This is a GET request
        reqT.Method = "GET"

        '@ToDo: Retrieve the token and format it from the header; Token may come from file system or extension
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
            MsgBox(ex.InnerException)
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

    Protected Function AcceptAllCertifications(ByVal sender As Object, ByVal certification As System.Security.Cryptography.X509Certificates.X509Certificate, ByVal chain As System.Security.Cryptography.X509Certificates.X509Chain, ByVal sslPolicyErrors As System.Net.Security.SslPolicyErrors) As Boolean
        Return True
    End Function
End Class