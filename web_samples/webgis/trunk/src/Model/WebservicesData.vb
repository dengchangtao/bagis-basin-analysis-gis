Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data

Public Class WebservicesData

    Private _connectionString As String
    Private Shared DELETED As Int16 = 1

    Public Sub New()
        Initialize()
    End Sub


    Public Sub Initialize()
        ' Initialize data source. Use "wgis" connection string from configuration.

        If ConfigurationManager.ConnectionStrings("wgis") Is Nothing OrElse _
            ConfigurationManager.ConnectionStrings("wgis").ConnectionString.Trim() = "" Then
            Throw New Exception("A connection string named 'wgis' with a valid connection string " & _
                                "must exist in the <connectionStrings> configuration section for the application.")
        End If

        _connectionString = _
          ConfigurationManager.ConnectionStrings("wgis").ConnectionString
    End Sub

    Public Function GetAllServices(sortColumns As String, startRecord As Integer, maxRecords As Integer) As List(Of Webservice)

        VerifySortColumns(sortColumns)

        'Dim sqlCmd As String = "SELECT ID, DisplayName, MapServiceUrl, IndexColumn FROM webservices where Deleted <> 1 "
        Dim sqlCmd As String = "SELECT * FROM webservices where Deleted <> " & DELETED

        If sortColumns.Trim() = "" Then
            sqlCmd &= "ORDER BY DisplayName"
        Else
            sqlCmd &= "ORDER BY " & sortColumns
        End If

        Dim conn As SqlConnection = New SqlConnection(_connectionString)
        Dim cmd As SqlCommand = New SqlCommand(sqlCmd, conn)
        'Dim da As SqlDataAdapter = New SqlDataAdapter(sqlCmd, conn)

        Dim reader As SqlDataReader = Nothing
        Dim webservices As List(Of Webservice) = New List(Of Webservice)()
        Dim count As Integer = 0
        'Reset maxRecords if it was not supplied
        If maxRecords < 1 Then maxRecords = +2147483647

        'Dim ds As DataSet = New DataSet()

        Try
            conn.Open()
            reader = cmd.ExecuteReader()

            Do While reader.Read()
                If count >= startRecord Then
                    If webservices.Count < maxRecords Then
                        webservices.Add(GetWebserviceFromReader(reader))
                    Else
                        cmd.Cancel()
                    End If
                End If

                count += 1
            Loop

            'da.Fill(ds, startRecord, maxRecords, "webservices")
        Catch e As SqlException
            ' Handle exception.
        Finally
            conn.Close()
        End Try

        'Return ds.Tables("webservices")
        Return webservices
    End Function

    Private Function GetWebserviceFromReader(reader As SqlDataReader) As Webservice
        Dim ws As Webservice = New Webservice()

        ws.Id = reader.GetInt32(0)
        ws.DisplayName = Trim(reader.GetString(1))
        ws.MapServiceUrl = Trim(reader.GetString(2))
        ws.IndexColumn = Trim(reader.GetString(3))
        ws.DateCreated = reader.GetDateTime(4)
        ws.DateModified = reader.GetDateTime(5)
        'Username and password values may be null
        If Not IsDBNull(reader.GetValue(7)) Then
            ws.UserName = reader.GetString(7)
        Else
            ws.UserName = Nothing
        End If
        If Not IsDBNull(reader.GetValue(8)) Then
            ws.Password = reader.GetString(8)
        Else
            ws.Password = Nothing
        End If
        If Not IsDBNull(reader.GetValue(10)) Then
            ws.Shape = reader.GetString(10)
        Else
            ws.Shape = Nothing
        End If
        If Not IsDBNull(reader.GetValue(11)) Then
            ws.ServiceType = reader.GetString(11)
        Else
            ws.ServiceType = Nothing
        End If
        Return ws
    End Function

    Public Function GetAllServicesJson(sortColumns As String, startRecord As Integer, maxRecords As Integer) As String
        'Dim dTable As DataTable = GetAllServices(sortColumns, startRecord, maxRecords)
        Dim wsList As IList(Of Webservice) = GetAllServices(sortColumns, startRecord, maxRecords)
        Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim rows As New List(Of Dictionary(Of String, Object))()
        Dim row As Dictionary(Of String, Object) = Nothing
        For Each ws As Webservice In wsList
            row = New Dictionary(Of String, Object)()
            row.Add("ID", ws.Id)
            row.Add("DisplayName", ws.DisplayName)
            row.Add("MapServiceUrl", ws.MapServiceUrl)
            row.Add("IndexColumn", ws.IndexColumn)
            If Not String.IsNullOrEmpty(ws.UserName) Then
                row.Add("UserName", Trim(ws.UserName))
                row.Add("Password", Trim(ws.Password))
            End If
            row.Add("Shape", Trim(ws.Shape))
            row.Add("ServiceType", Trim(ws.ServiceType))
            rows.Add(row)
        Next
        Return serializer.Serialize(rows)
    End Function

    Public Function SelectCount() As Integer

        Dim conn As SqlConnection = New SqlConnection(_connectionString)
        Dim cmd As SqlCommand = New SqlCommand("SELECT COUNT(*) FROM Webservices where Deleted <> " & DELETED, conn)

        Dim result As Integer = 0

        Try
            conn.Open()

            result = CInt(cmd.ExecuteScalar())
        Catch e As SqlException
            ' Handle exception.
        Finally

            conn.Close()
        End Try

        Return result
    End Function

    '''''
    ' Verify that only valid columns are specified in the sort expression to aSub a SQL Injection attack.

    Private Sub VerifySortColumns(sortColumns As String)
        If String.IsNullOrEmpty(sortColumns) Then
            'No sort columns were provided
            Exit Sub
        End If
        If sortColumns.ToLowerInvariant().EndsWith(" desc") Then _
          sortColumns = sortColumns.Substring(0, sortColumns.Length - 5)

        Dim columnNames() As String = sortColumns.Split(",")

        For Each columnName As String In columnNames
            Select Case columnName.Trim().ToLowerInvariant()
                Case "ID"
                Case "DisplayName"
                Case "MapServiceUrl"
                Case "IndexColumn"
                Case Else
                    Throw New ArgumentException("SortColumns contains an invalid column name.")
            End Select
        Next
    End Sub

    Public Function GetWebservice(ByVal id As Int32) As Webservice
        Dim conn As SqlConnection = New SqlConnection(_connectionString)
        Dim cmd As SqlCommand = _
          New SqlCommand("SELECT * " & _
                         "  FROM webservices where Deleted <> " & DELETED & " AND Id = @Id", conn)
        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id

        Dim reader As SqlDataReader = Nothing
        Dim webservice As Webservice = New Webservice()

        Try
            conn.Open()

            reader = cmd.ExecuteReader(CommandBehavior.SingleRow)
            If reader.Read = True Then
                webservice = GetWebserviceFromReader(reader)
            End If
        Catch e As SqlException
            ' Handle exception.
            Debug.Print("Exception: " & e.Message)
        Finally
            If reader IsNot Nothing Then reader.Close()
            conn.Close()
        End Try

        Return webservice

    End Function

    '
    ' Update the Webservice by ID.
    '   This method assumes that ConflictDetection is Set to OverwriteValues.
    Public Function UpdateWebservice(ByVal Id As Int32, DisplayName As String,
                                     ByVal MapServiceUrl As String, ByVal IndexColumn As String, _
                                     ByVal UserName As String, ByVal Password As String, _
                                     ByVal HasPassword As Boolean, ByVal Shape As String, ByVal ServiceType As String) As Integer

        If String.IsNullOrEmpty(DisplayName) Then _
          Throw New ArgumentException("DisplayName cannot be null or an empty string.")
        If String.IsNullOrEmpty(MapServiceUrl) Then _
          Throw New ArgumentException("MapServiceUrl cannot be null or an empty string.")

        If IndexColumn Is Nothing Then IndexColumn = String.Empty
        Dim DateModified As DateTime = DateTime.Now

        Dim conn As SqlConnection = New SqlConnection(_connectionString)
        Dim cmd As SqlCommand = New SqlCommand("UPDATE Webservices " & _
                                            "  SET DisplayName=@DisplayName, MapServiceUrl=@MapServiceUrl, " & _
                                            "  IndexColumn=@IndexColumn, Username=@Username, Password=@Password, " & _
                                            "  Shape=@Shape, ServiceType=@ServiceType, DateModified=@DateModified WHERE Id=@Id", conn)

        cmd.Parameters.Add("@DisplayName", SqlDbType.VarChar).Value = DisplayName
        cmd.Parameters.Add("@MapServiceUrl", SqlDbType.VarChar).Value = MapServiceUrl
        cmd.Parameters.Add("@IndexColumn", SqlDbType.VarChar).Value = IndexColumn
        If String.IsNullOrEmpty(UserName) Then
            cmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = DBNull.Value
        Else
            cmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = UserName.Trim
        End If
        If HasPassword = True Then
            If String.IsNullOrEmpty(Password) Then
                'Get the old password before updating the record
                Dim wsCopy As Webservice = GetWebservice(Id)
                If wsCopy IsNot Nothing Then
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = wsCopy.Password
                    wsCopy = Nothing
                End If
            Else
                cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = Password
            End If
        Else
            cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = DBNull.Value
        End If
        cmd.Parameters.Add("@Shape", SqlDbType.VarChar).Value = Shape
        cmd.Parameters.Add("@ServiceType", SqlDbType.VarChar).Value = ServiceType
        cmd.Parameters.Add("@DateModified", SqlDbType.VarChar).Value = DateModified
        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Id

        Dim result As Integer = 0

        Try
            conn.Open()

            result = cmd.ExecuteNonQuery()
        Catch e As SqlException
            ' Handle exception.
            Debug.Print("SqlException: " & e.Message)
            result = 0
        Finally
            conn.Close()
        End Try

        Return result
    End Function

    '
    ' Delete the Webservice by ID.
    ' This method assumes that ConflictDetection is Set to OverwriteValues.
    Public Function DeleteWebservice(ByVal Id As Int32) As Integer
        Dim DateModified As DateTime = DateTime.Now

        Dim conn As SqlConnection = New SqlConnection(_connectionString)
        Dim cmd As SqlCommand = New SqlCommand("UPDATE Webservices " & _
                                            "  SET Deleted=@Deleted, DateModified=@DateModified " & _
                                            "  WHERE Id=@Id", conn)

        cmd.Parameters.Add("@Deleted", SqlDbType.Int).Value = DELETED
        cmd.Parameters.Add("@DateModified", SqlDbType.VarChar).Value = DateModified
        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = Id

        Dim result As Integer = 0

        Try
            conn.Open()

            result = cmd.ExecuteNonQuery()
        Catch e As SqlException
            ' Handle exception.
            Debug.Print("SqlException: " & e.Message)
            result = 0
        Finally
            conn.Close()
        End Try

        Return result
    End Function

    'Note: For a production application, the id should be generated by the database
    'http://www.w3schools.com/sql/sql_autoincrement.asp
    Private Function GetNextId() As Integer
        Dim conn As SqlConnection = New SqlConnection(_connectionString)
        Dim cmd As SqlCommand = New SqlCommand("SELECT MAX(ID) FROM webservices", conn)

        Dim reader As SqlDataReader = Nothing
        Dim nextId As Integer = -1

        Try
            conn.Open()

            reader = cmd.ExecuteReader(CommandBehavior.SingleRow)
            If reader.Read = True Then
                nextId = reader.GetInt32(0)
            End If
            nextId += 1
        Catch e As SqlException
            ' Handle exception.
            Debug.Print("SqlException: " & e.Message)
        Finally
            If reader IsNot Nothing Then reader.Close()
            conn.Close()
        End Try

        Return nextId
    End Function

    Public Function InsertWebService(ByVal DisplayName As String, ByVal MapServiceUrl As String, _
                                     ByVal IndexColumn As String, ByVal UserName As String, _
                                     ByVal Password As String, ByVal Shape As String, _
                                     ByVal ServiceType As String) As Integer
        If String.IsNullOrEmpty(DisplayName) Then _
            Throw New ArgumentException("DisplayName cannot be null or an empty string.")
        If String.IsNullOrEmpty(MapServiceUrl) Then _
            Throw New ArgumentException("MapServiceUrl cannot be null or an empty string.")

        If IndexColumn Is Nothing Then IndexColumn = String.Empty

        Dim conn As SqlConnection = New SqlConnection(_connectionString)
        Dim cmd As SqlCommand = New SqlCommand("INSERT INTO Webservices " & _
                                            "  (ID, DisplayName, MapServiceUrl, IndexColumn, Username, Password, Shape, ServiceType, DateCreated, DateModified) " & _
                                            "  Values(@ID, @DisplayName, @MapServiceUrl, @IndexColumn, @Username, @Password, @Shape, @ServiceType," & _
                                            " @DateCreated, @DateModified) ", conn)

        Dim ID As Integer = GetNextId()
        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ID
        cmd.Parameters.Add("@DisplayName", SqlDbType.VarChar).Value = DisplayName
        cmd.Parameters.Add("@MapServiceUrl", SqlDbType.VarChar).Value = MapServiceUrl
        cmd.Parameters.Add("@IndexColumn", SqlDbType.VarChar).Value = IndexColumn
        If String.IsNullOrEmpty(UserName) Then
            cmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = DBNull.Value
            'Set password to null so we don't save a password with no user name
            Password = Nothing
        Else
            cmd.Parameters.Add("@Username", SqlDbType.VarChar).Value = UserName.Trim
        End If
        If String.IsNullOrEmpty(Password) Then
            cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = DBNull.Value
        Else
            cmd.Parameters.Add("@Password", SqlDbType.VarChar).Value = Password
        End If
        If String.IsNullOrEmpty(Shape) Then
            cmd.Parameters.Add("@Shape", SqlDbType.VarChar).Value = DBNull.Value
        Else
            cmd.Parameters.Add("@Shape", SqlDbType.VarChar).Value = Shape
        End If
        If String.IsNullOrEmpty(ServiceType) Then
            cmd.Parameters.Add("@ServiceType", SqlDbType.VarChar).Value = DBNull.Value
        Else
            cmd.Parameters.Add("@ServiceType", SqlDbType.VarChar).Value = ServiceType
        End If

        Dim rightNow As DateTime = DateTime.Now
        cmd.Parameters.Add("@DateCreated", SqlDbType.DateTime).Value = rightNow
        cmd.Parameters.Add("@DateModified", SqlDbType.DateTime).Value = rightNow

        Try
            conn.Open()
            cmd.ExecuteNonQuery()

        Catch e As SqlException
            ' Handle exception.
            Debug.Print("SqlException: " & e.Message)
            ID = 0
        Finally
            conn.Close()
        End Try

        Return ID

    End Function

End Class

