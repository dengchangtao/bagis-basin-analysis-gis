Imports System.Net
Imports System.Text

Public Class SecurityHelper

    Public Shared Function GetStoredToken() As String
        Return My.Settings.GoldenTicket
    End Function

    Private Shared Sub StoreToken(ByVal aToken As BagisToken)
        My.Settings.GoldenTicket = aToken.token
        My.Settings.Save()
    End Sub

    Public Shared Function GetServerToken(ByVal userName, ByVal password, ByVal url) As String
        Dim reqT As HttpWebRequest
        Dim resT As HttpWebResponse
        reqT = WebRequest.Create(url)
        'Needs to be a POST request to get a token
        reqT.Method = "POST"

        'These are the field/value pairs that would be on an html form
        Dim Data As String = "username=" & userName & "&password=" & password
        'Encode them to Byte format to include with the request
        Dim credArray As Byte() = Encoding.UTF8.GetBytes(Data)
        'We are sending a form
        reqT.ContentType = "application/x-www-form-urlencoded"
        reqT.ContentLength = credArray.Length

        '@ToDo: Workaround for certificate error; This should come out when the certificate issue is fixed
        ServicePointManager.ServerCertificateValidationCallback = New System.Net.Security.RemoteCertificateValidationCallback(AddressOf AcceptAllCertifications)

        Try
            'Intercept the httpRequest so we can add the user name/password
            Dim dataStreamT As System.IO.Stream = reqT.GetRequestStream()
            dataStreamT.Write(credArray, 0, credArray.Length)
            dataStreamT.Close()
            'Send the request and wait for response
            resT = CType(reqT.GetResponse(), HttpWebResponse)
            'Convert the JSON response to a BagisTokenn object
            Dim aToken As BagisToken = New BagisToken
            Dim ser As System.Runtime.Serialization.Json.DataContractJsonSerializer = New System.Runtime.Serialization.Json.DataContractJsonSerializer(aToken.[GetType]())
            'Store token in user settings
            aToken = CType(ser.ReadObject(resT.GetResponseStream), BagisToken)
            StoreToken(aToken)
            Return aToken.token
        Catch ex As WebException
            Debug.Print("GetServerToken Exception" & ex.Message)
            Return Nothing
        End Try
    End Function

    Public Shared Function IsTokenValid(ByVal testUrl, ByVal strToken) As Boolean
        Dim reqT As HttpWebRequest
        Dim resT As HttpWebResponse
        'The end point for getting a token for the web service
        reqT = WebRequest.Create(testUrl)
        'This is a GET request
        reqT.Method = "GET"

        'Retrieve the token and format it for the header; Token comes from caller
        Dim cred As String = String.Format("{0} {1}", "Token", strToken)
        'Put token in header
        reqT.Headers(HttpRequestHeader.Authorization) = cred

        '@ToDo: Workaround for certificate error; This should come out when the certificate issue is fixed
        ServicePointManager.ServerCertificateValidationCallback = New System.Net.Security.RemoteCertificateValidationCallback(AddressOf AcceptAllCertifications)

        Try
            resT = CType(reqT.GetResponse(), HttpWebResponse)
            'Printing the response to the Console for testing
            'Using SReader As System.IO.StreamReader = New System.IO.StreamReader(resT.GetResponseStream)
            '    Debug.Print(SReader.ReadToEnd())
            'End Using
            'If we didn't get an exception, the token is valid
            Return True
        Catch ex As WebException
            'Catch exception and return false; Token is not valid
            Return False
        End Try
    End Function

    Protected Shared Function AcceptAllCertifications(ByVal sender As Object, ByVal certification As System.Security.Cryptography.X509Certificates.X509Certificate, ByVal chain As System.Security.Cryptography.X509Certificates.X509Chain, ByVal sslPolicyErrors As System.Net.Security.SslPolicyErrors) As Boolean
        Return True
    End Function
End Class
