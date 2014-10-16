Imports System.IO
Imports System.Xml.Serialization

Public Class SerializableData

    Public Sub Save(ByVal fileName As String)
        Dim tempFileName As String = fileName & ".tmp"
        Dim tempFileInfo As New FileInfo(tempFileName)
        If tempFileInfo.Exists = True Then tempFileInfo.Delete()
        Dim stream As New FileStream(tempFileName, FileMode.Create)
        Save(stream)
        stream.Close()
        tempFileInfo.CopyTo(fileName, True)
        tempFileInfo.Delete()
    End Sub

    Public Sub Save(ByVal pStream As Stream)
        Dim serializer As New XmlSerializer(Me.GetType)
        serializer.Serialize(pStream, Me)
    End Sub

    Public Shared Function Load(ByVal fileName As String, ByVal newType As Type) As Object
        Dim fileInfo As New FileInfo(fileName)
        If fileInfo.Exists = False Then
            ' create a blank version of the object and return that
            Return System.Activator.CreateInstance(newType)
        End If
        Dim stream As New FileStream(fileName, FileMode.Open)
        Dim newObject As Object = Load(stream, newType)
        stream.Close()
        Return newObject
    End Function

    Public Shared Function Load(ByVal stream As Stream, ByVal newType As Type)
        Dim serializer As New XmlSerializer(newType)
        Dim newObject As Object = serializer.Deserialize(stream)
        Return newObject
    End Function

End Class
