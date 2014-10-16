Imports System.Text

Public Module DataDescriptorModule

    ' Populates an DataDescriptor object from an XML file
    Public Function BA_LoadDataDescriptorFromXml(ByVal xmlInputPath As String) As DataDescriptor
        If BA_File_ExistsWindowsIO(xmlInputPath) Then
            Dim obj As Object = SerializableData.Load(xmlInputPath, GetType(DataDescriptor))
            'Dim obj As Object = SerializableData.Load(xmlInputPath, GetType(Aoi))
            If obj IsNot Nothing Then
                Dim pDataDescriptor As DataDescriptor = CType(obj, DataDescriptor)
                Return pDataDescriptor
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If
    End Function

    ' Build path to prism data descriptor
    Public Function BA_PathToPrismDataDescriptor(ByVal aoiPath As String) As String
        Dim sb As New StringBuilder()
        sb.Append(aoiPath)
        sb.Append("\" & BA_EnumDescription(GeodatabaseNames.Prism))
        sb.Append(BA_EnumDescription(PublicPath.DataDescriptor))
        Return sb.ToString
    End Function

End Module
