Imports BAGIS_ClassLibrary

Module BAGIS_MetadataModule

    Public Function BA_GetElevationUnitsForAOI(ByVal aoiPath As String) As MeasurementUnit
        Dim inputFolder As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Surfaces)
        Dim inputFile As String = BA_EnumDescription(MapsFileName.filled_dem_gdb)
        Dim tagsList As IList(Of String) = BA_ReadMetaData(inputFolder, inputFile, _
                                                           LayerType.Raster, BA_XPATH_TAGS)
        Dim elevUnit As MeasurementUnit
        If tagsList IsNot Nothing Then
            For Each pInnerText As String In tagsList
                'This is our BAGIS tag
                If pInnerText.IndexOf(BA_BAGIS_TAG_PREFIX) = 0 Then
                    Dim strUnits As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_VALUE_TAG)
                    If strUnits IsNot Nothing Then
                        elevUnit = BA_GetMeasurementUnit(strUnits)
                    End If
                    Exit For
                End If
            Next
        End If
        Return elevUnit
    End Function

End Module
