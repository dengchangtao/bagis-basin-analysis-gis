Public Class Webservice

    Private _id As Integer
    Private _displayName As String
    Private _mapServiceUrl As String
    Private _indexColumn As String
    Private _userName As String
    Private _password As String
    Private _shape As String
    Private _serviceType As String
    Private _dateCreated As Date
    Private _dateModified As Nullable(Of Date)
    Private _deleted As Boolean

    Public Property Id As Integer
        Get
            Return _id
        End Get
        Set(value As Integer)
            _id = value
        End Set
    End Property

    Public Property DisplayName As String
        Get
            Return _displayName
        End Get
        Set(value As String)
            _displayName = value
        End Set
    End Property

    Public Property MapServiceUrl As String
        Get
            Return _mapServiceUrl
        End Get
        Set(value As String)
            _mapServiceUrl = value
        End Set
    End Property

    Public Property IndexColumn As String
        Get
            Return _indexColumn
        End Get
        Set(value As String)
            _indexColumn = value
        End Set
    End Property

    'How to use secured resources with ArcGIS Javascript API
    'https://developers.arcgis.com/javascript/jshelp/ags_secureservices.html
    Public Property UserName As String
        Get
            Return _userName
        End Get
        Set(value As String)
            _userName = value
        End Set
    End Property

    Public Property Password As String
        Get
            Return _password
        End Get
        Set(value As String)
            _password = value
        End Set
    End Property

    Public Property Shape As String
        Get
            Return _shape
        End Get
        Set(value As String)
            _shape = value
        End Set
    End Property

    Public Property ServiceType As String
        Get
            Return _serviceType
        End Get
        Set(value As String)
            _serviceType = value
        End Set
    End Property

    Public Property DateCreated As Date
        Get
            Return _dateCreated
        End Get
        Set(value As Date)
            _dateCreated = value
        End Set
    End Property

    Public ReadOnly Property DateCreatedText As String
        Get
            If _dateCreated > New Date Then
                Return _dateCreated.ToString("g", System.Globalization.CultureInfo.CreateSpecificCulture("en-us"))
            Else
                Return ""
            End If
        End Get
        'Set(value As Date)
        '    _dateCreated = value
        'End Set
    End Property

    Public Property DateModified As Date
        Get
            Return _dateModified
        End Get
        Set(value As Date)
            _dateModified = value
        End Set
    End Property

    Public ReadOnly Property DateModifiedText As String
        Get
            If _dateModified IsNot Nothing Then
                'Resolve compile error when trying to convert a Nullable to String
                Dim mDate As Date = CType(_dateModified, Date)
                Return mDate.ToString("g", System.Globalization.CultureInfo.CreateSpecificCulture("en-us"))
            Else
                Return ""
            End If
        End Get
        'Set(value As Date)
        '    _dateCreated = value
        'End Set
    End Property

    Public Property Deleted As Boolean
        Get
            Return _deleted
        End Get
        Set(value As Boolean)
            _deleted = value
        End Set
    End Property
End Class
