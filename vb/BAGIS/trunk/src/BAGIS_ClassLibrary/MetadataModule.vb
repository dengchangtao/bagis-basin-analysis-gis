Imports ESRI.ArcGIS.Geodatabase
Imports ESRI.ArcGIS.esriSystem
Imports System.Xml
Imports System.Text

Public Module MetadataModule

    'This function returns a iList of the innerValues for the set of tags that exist at the
    'propertyPath XPath for the layer defined by the input folder and input file.
    'An example XPath is: /metadata/dataIdInfo/searchKeys/keyword
    Public Function BA_ReadMetaData(ByVal inputFolder As String, ByVal inputFile As String, _
                                    ByVal layerType As LayerType, ByVal propertyPath As String) As IList(Of String)
        Dim pMeta As IMetadata = Nothing
        Dim pXmlPropertySet As IXmlPropertySet2
        Try
            If layerType = layerType.Raster Then
                pMeta = BA_GetMetadataForRaster(inputFolder, inputFile)
            ElseIf layerType = layerType.Vector Then
                pMeta = BA_GetMetadataForVector(inputFolder, inputFile)
            End If
            'Get a reference to the IXmlPropertySet2 from the metadata
            pXmlPropertySet = pMeta.Metadata
            'Import the full schema
            Dim strXml As String = pXmlPropertySet.GetXml("")
            'Create a VB .NET XmlDocument and load the schema
            Dim myXml As XmlDocument = New XmlDocument
            myXml.LoadXml(strXml)
            'Select the nodes from the fully qualified XPath
            Dim propertyNodes As XmlNodeList = myXml.SelectNodes(propertyPath)
            Dim propertyList As IList(Of String) = New List(Of String)
            'Place each innerText into a list to return
            For Each pNode As XmlNode In propertyNodes
                propertyList.Add(pNode.InnerText)
            Next
            Return propertyList
        Catch ex As Exception
            Debug.Print("BA_ReadMetaData Exception: " & ex.Message)
            Return Nothing
        Finally
            pMeta = Nothing
            pXmlPropertySet = Nothing
        End Try

    End Function

    'This function adds a node to an xml document; The node is identified by its XPath
    'The separator is usually a "/"
    'An example XPath is: /metadata/dataIdInfo/searchKeys/keyword
    Public Sub BA_AddNode(ByRef pXmlDoc As XmlDocument, ByVal nodePath As String, _
                          ByVal sep As String)
        'Parse the propertyPath into its components
        Dim propNames As String() = nodePath.Split(sep)
        Dim parentXPath As String = Nothing
        Dim parentNode As XmlNode = Nothing
        For Each pName In propNames
            If Not String.IsNullOrEmpty(pName) Then
                parentXPath = parentXPath & sep & pName
                'Select the nodes from the fully qualified XPath
                Dim propertyNodes As XmlNodeList = pXmlDoc.SelectNodes(parentXPath)
                If propertyNodes.Count > 0 Then
                    'parentXPath exists
                    'assume the node we want is the first one
                    parentNode = propertyNodes(0)
                Else
                    'Create the new child node
                    Dim newChildNode As XmlNode = pXmlDoc.CreateNode(XmlNodeType.Element, pName, Nothing)
                    'append it to the parent
                    parentNode.AppendChild(newChildNode)
                    parentNode = newChildNode
                End If
            End If
        Next
    End Sub

    'This function updates the innerText of a node identified by a fully qualified XPath
    'An example XPath is: /metadata/dataIdInfo/searchKeys/keyword
    'The function also allows you to specify the length of the prefix of the String to be matched
    'against the innerText
    Public Function BA_UpdateMetadata(ByVal inputFolder As String, ByVal inputFile As String, _
                                      ByVal layerType As LayerType, ByVal propertyPath As String, _
                                      ByVal innerText As String, ByVal matchLength As Integer) As BA_ReturnCode
        Dim pMeta As IMetadata
        Dim pXmlPropertySet As IXmlPropertySet2
        Try
            If layerType = BAGIS_ClassLibrary.LayerType.Raster Then
                pMeta = BA_GetMetadataForRaster(inputFolder, inputFile)
            Else
                pMeta = BA_GetMetadataForVector(inputFolder, inputFile)
            End If
            'Get a reference to the IXmlPropertySet2 from the metadata
            pXmlPropertySet = pMeta.Metadata
            'Import the full schema
            Dim strXml As String = pXmlPropertySet.GetXml("")
            'Create a VB .NET XmlDocument and load the schema
            Dim myXml As XmlDocument = New XmlDocument
            myXml.LoadXml(strXml)
            'Check to see if the parent node exists
            Dim sep As String = "/"
            Dim lastSep As Integer = propertyPath.LastIndexOf(sep)
            Dim parentNodePath = propertyPath.Substring(0, lastSep)
            Dim parentNodeList As XmlNodeList = myXml.SelectNodes(parentNodePath)
            If parentNodeList.Count < 1 Then
                BA_AddNode(myXml, parentNodePath, sep)
                parentNodeList = myXml.SelectNodes(parentNodePath)
            End If
            'Assume we want the first one
            Dim parentNode As XmlNode = parentNodeList(0)
            Dim childNodeName As String = propertyPath.Substring(lastSep + 1)
            Dim propertyNodeList As XmlNodeList = parentNode.ChildNodes
            Dim matchPrefix As String = innerText.Substring(0, matchLength)
            Dim foundIt As Boolean = False
            For Each pNode As XmlNode In propertyNodeList
                'Is the node the same node name we need to update?
                If pNode.Name = childNodeName Then
                    'Is the first part of the innerText the same as what we want to update 
                    If pNode.InnerText.Length > matchLength AndAlso _
                        pNode.InnerText.Substring(0, matchLength) = matchPrefix Then
                        'If so, update the innerText
                        pNode.InnerText = innerText
                        foundIt = True
                    End If
                End If
            Next
            'If it didn't exist, we need to create a new node
            If foundIt = False Then
                'Create the child node
                Dim childNode As XmlNode = myXml.CreateNode(XmlNodeType.Element, childNodeName, Nothing)
                childNode.InnerText = innerText
                'Attach the child to the parent
                parentNode.AppendChild(childNode)
            End If
            'Persist changes
            pXmlPropertySet.SetXml(myXml.OuterXml)
            pMeta.Metadata = pXmlPropertySet
            Return BA_ReturnCode.Success
        Catch ex As Exception
            Debug.Print("BA_UpdateMetadata Exception: " & ex.Message)
            Return BA_ReturnCode.UnknownError
        Finally
            pMeta = Nothing
            pXmlPropertySet = Nothing
        End Try
    End Function

    Public Function BA_GetValueForKey(ByVal innerText As String, ByVal keyText As String) As String
        Dim strValue As String = Nothing
        Dim pContents As String() = innerText.Split(";")
        For Each pValue As String In pContents
            'This tag contains the zUnitCategory
            If pValue.IndexOf(keyText) > -1 Then
                'ZUnitCategory|Depth
                strValue = pValue.Substring(pValue.IndexOf("|") + 1)
                ' Strip trailing ";" if exists
                If strValue(Len(strValue) - 1) = ";" Then
                    'Elevation
                    strValue = strValue.Remove(Len(strValue) - 1, 1)
                End If
            End If
        Next
        Return strValue
    End Function

    ' Returns the metadata object for a raster dataset
    ' Works with both grid and file gdb inputs
    Public Function BA_GetMetadataForRaster(ByVal inputFolder As String, ByVal inputFile As String)
        Dim pRasterGDS As IGeoDataset = Nothing
        Dim pRasterDS As IRasterDataset
        Dim pMeta As IMetadata

        Try
            Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(inputFolder)
            If workspaceType = workspaceType.Geodatabase Then
                pRasterGDS = BA_OpenRasterFromGDB(inputFolder, inputFile)
            Else
                pRasterGDS = BA_OpenRasterFromFile(inputFolder, inputFile)
            End If
            pRasterDS = CType(pRasterGDS, IRasterDataset)   'Explicit Cast
            pMeta = CType(pRasterDS, IMetadata)
            Return pMeta
        Catch ex As Exception
            Debug.Print("BA_GetMetadataForRaster: " & ex.Message)
            Return Nothing
        Finally
            pRasterDS = Nothing
            pRasterGDS = Nothing
        End Try

    End Function

    ' Returns the metadata object for a vector dataset
    ' Works with both grid and file gdb inputs
    Public Function BA_GetMetadataForVector(ByVal inputFolder As String, ByVal inputFile As String)
        Dim pFeatGDS As IGeoDataset = Nothing
        Dim pFeatClass As IFeatureClass
        Dim pDataset As IDataset
        Dim pFeatureClassName As FeatureClassName
        Dim pMeta As IMetadata

        Try
            Dim workspaceType As WorkspaceType = BA_GetWorkspaceTypeFromPath(inputFolder)
            If workspaceType = workspaceType.Geodatabase Then
                pFeatGDS = BA_OpenFeatureClassFromGDB(inputFolder, inputFile)
            Else
                pFeatGDS = BA_OpenFeatureClassFromFile(inputFolder, inputFile)
            End If
            pFeatClass = CType(pFeatGDS, IFeatureClass) 'Explicit Cast
            pDataset = CType(pFeatClass, IDataset)  'Explicit Cast
            pFeatureClassName = CType(pDataset.FullName, FeatureClassName) 'Explicit Cast
            pMeta = CType(pFeatureClassName, IMetadata)
            Return pMeta
        Catch ex As Exception
            Debug.Print("BA_GetMetadataForVector: " & ex.Message)
            Return Nothing
        Finally
            pFeatGDS = Nothing
            pFeatClass = Nothing
            pDataset = Nothing
            pFeatureClassName = Nothing
        End Try

    End Function

    Public Function BA_GetAoiSlopeUnit(ByVal aoiPath As String) As SlopeUnit
        'Slope units
        Dim inputFolder As String = BA_GeodatabasePath(aoiPath, GeodatabaseNames.Surfaces)
        Dim inputFile As String = BA_GetBareName(BA_EnumDescription(PublicPath.Slope))
        Dim tagsList As IList(Of String) = BA_ReadMetaData(inputFolder, inputFile, _
                                                           LayerType.Raster, BA_XPATH_TAGS)
        Dim retVal As SlopeUnit
        If tagsList IsNot Nothing Then
            For Each pInnerText As String In tagsList
                'This is our BAGIS tag
                If pInnerText.IndexOf(BA_BAGIS_TAG_PREFIX) = 0 Then
                    Dim strUnits As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_VALUE_TAG)
                    If strUnits IsNot Nothing Then
                        retVal = BA_GetSlopeUnit(strUnits)
                    End If
                    Exit For
                End If
            Next
        End If
        Return retVal
    End Function

    Public Function BA_GetAoiWeaselSlopeUnit(ByVal aoiPath As String) As SlopeUnit
        'Slope units
        Dim inputFolder As String = BA_GetPath(aoiPath, PublicPath.Slope)
        Dim inputFile As String = BA_EnumDescription(MapsFileName.slope)
        Dim tagsList As IList(Of String) = BA_ReadMetaData(inputFolder, inputFile, _
                                                           LayerType.Raster, BA_XPATH_TAGS)
        Dim retVal As SlopeUnit
        If tagsList IsNot Nothing Then
            For Each pInnerText As String In tagsList
                'This is our BAGIS tag
                If pInnerText.IndexOf(BA_BAGIS_TAG_PREFIX) = 0 Then
                    Dim strUnits As String = BA_GetValueForKey(pInnerText, BA_ZUNIT_VALUE_TAG)
                    If strUnits IsNot Nothing Then
                        retVal = BA_GetSlopeUnit(strUnits)
                    End If
                    Exit For
                End If
            Next
        End If
        Return retVal
    End Function

    'This is a simplistic updating of the BAGIS tag for a recalculated slope units layer
    'Because we create the new slope units layer, we assume there is no existing metadata
    Public Sub BA_UpdateSlopeUnits(ByVal inputFolder As String, ByVal inputFile As String, ByVal slopeUnit As SlopeUnit)
        'We need to add a new tag at "/metadata/dataIdInfo/searchKeys/keyword"
        Dim sb As StringBuilder = New StringBuilder
        sb.Append(BA_BAGIS_TAG_PREFIX)
        sb.Append(BA_ZUNIT_CATEGORY_TAG & MeasurementUnitType.Slope.ToString & "; ")

        sb.Append(BA_ZUNIT_VALUE_TAG & BA_EnumDescription(slopeUnit) & ";")
        sb.Append(BA_BAGIS_TAG_SUFFIX)
        BA_UpdateMetadata(inputFolder, inputFile, LayerType.Raster, _
            BA_XPATH_TAGS, sb.ToString, BA_BAGIS_TAG_PREFIX.Length)
    End Sub

End Module
