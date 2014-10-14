Option Explicit On

Imports ESRI.ArcGIS.Catalog

' Copyright 1995-2004 ESRI

' All rights reserved under the copyright laws of the United States.

' You may freely redistribute and use this sample code, with or without modification.

' Disclaimer: THE SAMPLE CODE IS PROVIDED "AS IS" AND ANY EXPRESS OR IMPLIED 
' WARRANTIES, INCLUDING THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS 
' FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL ESRI OR 
' CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
' OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
' SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
' INTERRUPTION) SUSTAINED BY YOU OR A THIRD PARTY, HOWEVER CAUSED AND ON ANY 
' THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT ARISING IN ANY 
' WAY OUT OF THE USE OF THIS SAMPLE CODE, EVEN IF ADVISED OF THE POSSIBILITY OF 
' SUCH DAMAGE.

' For additional information contact: Environmental Systems Research Institute, Inc.

' Attn: Contracts Dept.

' 380 New York Street

' Redlands, California, U.S.A. 92373 

' Email: contracts@esri.com

Public Class GxFilterXml

    Implements IGxObjectFilter

    Private Function CanChooseObject(ByVal catObj As IGxObject, ByRef result As esriDoubleClickResult) As Boolean Implements IGxObjectFilter.CanChooseObject
        If ((catObj.Category = "Folder") Or (catObj.Category = "Folder Connection")) Then
            Return False
        ElseIf (catObj.Category = "XML Document") Then
            Return True
        End If
        Return False
    End Function

    Private Function CanDisplayObject(ByVal catObj As IGxObject) As Boolean Implements IGxObjectFilter.CanDisplayObject
        'If TypeOf catObj Is IGxObjectContainer Or Right$(UCase$(catObj.FullName), 4) = ".XML" Then
        '    Return True
        'End If
        If ((catObj.Category = "Folder") Or (catObj.Category = "Folder Connection") Or (catObj.Category = "XML Document")) Then
            Return True
        End If
        Return False
    End Function

    Private Function CanSaveObject(ByVal location As IGxObject, ByVal newObjectName As String, ByRef objectAlreadyExists As Boolean) As Boolean Implements IGxObjectFilter.CanSaveObject
        Return True
    End Function

    ' This appears on the filter description
    Private ReadOnly Property Description() As String Implements IGxObjectFilter.Description
        Get
            Return "XML Files (*.xml)"
        End Get
    End Property

    Private ReadOnly Property Name() As String Implements IGxObjectFilter.Name
        Get
            Return "XML"
        End Get
    End Property


End Class
