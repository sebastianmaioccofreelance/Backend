Imports System.Collections.Generic
Imports System.Linq
Imports System.Net.Http

Namespace Bases.DTOResponses
    Public Class FW_BaseResponseDTO
        Property Summary As New FW_ResponseSummary

        Public Sub Validate(IsValid As Boolean, Message As String)
            If IsValid Then
                UserError(Message)
            End If
        End Sub

        Public Sub SetHasValues(Optional Value As Boolean = True)
            Summary.HasValues = Value
        End Sub

        Public Sub SetUnsuccessfully(Ex As ExceptionUnsuccessfully, Optional Value As Boolean = True)
            Summary.Messages.Information.Add(Ex.Message)
            Summary.Unsuccessfully = Value
        End Sub

        Public Sub SetSystemError(Optional Value As Boolean = True)
            Summary.HasSystemError = Value
        End Sub

        Public Sub SetUnAuthorized(Optional Value As Boolean = True)
            Summary.UnAuthorized = Value
        End Sub

        Public Sub SetWarningSecurity(Optional Value As Boolean = True)
            Summary.HasSecurityWarning = Value
        End Sub

        Public Sub UserError(Mensaje As String)
            Summary.Messages.UserErrors.Add(Mensaje)
        End Sub

        Public Sub SetInformation(Mensaje As String)
            Summary.Messages.Information.Add(Mensaje)
        End Sub
    End Class

    Public Class FW_BaseResponseVoid
        Inherits FW_BaseResponseDTO
    End Class
    Public Class FW_BaseResponseFile
        Inherits FW_BaseResponseDTO
        Property ResponseMessage As HttpResponseMessage
    End Class

    Public Class FW_BaseResponseId
        Inherits FW_BaseResponseDTO
        Property Id As Int64
        Public Sub SetId(Id As Int64)
            Me.Id = Id
        End Sub
    End Class

    Public Class FW_BaseResponseBoolean
        Inherits FW_BaseResponseDTO
        Property Response As Boolean
    End Class
    Public Class FW_BaseResponseDictionary
        Inherits FW_BaseResponseDTO
        Property Data As Dictionary(Of String, String)
        Public Sub SetData(Data As Dictionary(Of String, String))
            Me.Data = Data
        End Sub
    End Class

    Public Class FW_BaseResponseObject
        Inherits FW_BaseResponseDTO
        Property Data As New Object
        Public Sub SetData(Data As Object)
            Me.Data = Data
        End Sub
    End Class

    Public Class FW_BaseResponseString
        Inherits FW_BaseResponseDTO
        Property Response As String
        Public Sub SetResponse(Response As String)
            Me.Response = Response
        End Sub
    End Class

    Public Class FW_BaseResponseSingleData
        Inherits FW_BaseResponseDTO
        Property Data As New FW_BaseDAO
        Public Sub SetData(Data As Object)
            Me.Data = Data
        End Sub
    End Class
    Public Class FW_BaseResponseTokenInformation
        Inherits FW_BaseResponseDTO
        Property Data As New Gestores.TokenInformation
        Public Sub SetData(Data As Gestores.TokenInformation)
            Me.Data = Data
        End Sub
    End Class
    Public Class FW_BaseResponseTable
        Inherits FW_BaseResponseDTO
        Private _data As New List(Of FW_BaseDAO)
        Property Data As List(Of FW_BaseDAO)
            Get
                Return _data
            End Get
            Set(value As List(Of FW_BaseDAO))
                _data.Clear()
                _data.AddRange(value)
                SetHasValues(value.Count > 0)
            End Set
        End Property

        Public Sub SetData(Data As IEnumerable(Of FW_BaseDAO))
            _data.Clear()
            _data.AddRange(Data)
            SetHasValues(_data.Count > 0)
        End Sub
    End Class

    Public Class FW_ResponseSummary
        Property Messages As New FW_Messages
        Property HasValues As Boolean
        Property HasSystemError As Boolean
        Property UnAuthorized As Boolean
        Property HasSecurityWarning As Boolean
        Property ErrorId As Long
        Property Unsuccessfully As Boolean
        Property TransactionID As String

        Public ReadOnly Property Ok As Boolean
            Get
                Return (Not HasUserErrors) And (Not HasSystemError And (Not HasSecurityWarning) And (Not Unsuccessfully) And (Not UnAuthorized))
            End Get
        End Property

        Public ReadOnly Property HasUserErrors
            Get
                Return Messages.UserErrors.Count > 0
            End Get
        End Property

        Public ReadOnly Property HasInformation
            Get
                Return Messages.Information.Count > 0
            End Get
        End Property
    End Class

    Public Class FW_Messages
        Property UserErrors As New List(Of String)
        Property Information As New List(Of String)
    End Class

End Namespace

