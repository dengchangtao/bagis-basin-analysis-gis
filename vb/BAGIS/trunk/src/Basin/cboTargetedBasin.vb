Public Class cboTargetedBasin
    Inherits ESRI.ArcGIS.Desktop.AddIns.ComboBox

    Public Sub New()
        Me.Enabled = False
    End Sub

    Public Sub setValue(ByVal value As String)
        Me.Clear()
        Dim c1 As Integer = Me.Add(value)
        Me.Select(c1)
        Me.Value = value
        myvalue(value)
    End Sub
    Public Function myvalue(ByVal value As String)
        'Dim cboSelectedBasinValue As String = value
        Return value
    End Function
    Public Function getValue() As String
        Dim s1 As String = ""
        If Me.items.Count > 0 Then
            s1 = Me.items.Item(0).Caption
        End If
        Return s1
    End Function
    Public WriteOnly Property selectedProperty As Boolean
        Set(ByVal value As Boolean)
            Me.Enabled = value
        End Set
    End Property

    'Private Sub GetManagedFromID()
    '    ' Extension1 is an add-in type within (or referenced by) this project. ' No need to call FindExtension again. 
    '    Dim ext = AddIn.FromID(Of Extension1)(ThisAddin.IDs.Extension1)
    '    ext.DoSomething()
    '    'early bound. ' An onDemand loading tool will be created here even OnClick hasn't been called
    '    Dim tool = AddIn.FromID(Of Tool1)(ThisAddin.IDs.Tool1)
    '    tool.DoSomething(ArcMap.Application.Caption)
    'End Sub
End Class


