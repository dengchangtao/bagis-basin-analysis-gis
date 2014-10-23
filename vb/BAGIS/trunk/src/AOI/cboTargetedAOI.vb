Public Class cboTargetedAOI
    Inherits ESRI.ArcGIS.Desktop.AddIns.ComboBox

    Public Sub New()
        Me.Enabled = False
    End Sub

    Protected Overrides Sub OnUpdate()

    End Sub
    Public WriteOnly Property selectedProperty As Boolean
        Set(ByVal value As Boolean)
            Me.Enabled = value
        End Set
    End Property

    Public Function getValue() As String
        Dim s1 As String = Nothing
        If Me.items.Count > 0 Then
            s1 = Me.items.Item(0).Caption
        End If

        'If String.IsNullOrEmpty(s1) Then
        '    Return "Nothing"
        'End If
        Return s1
    End Function

    Public Sub setValue(ByVal value As String)
        Me.Clear()
        Dim c1 As Integer = Me.Add(value)
        Me.Select(c1)
        Me.Value = value
    End Sub
End Class
