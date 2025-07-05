Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports FrameWork.Gestores
Imports FrameWork.Modulos
Namespace Bases.DTORequests
    Public Class FW_BaseRequestDTO

    End Class
    Public Class FW_BaseRequestBasicAuthentication
        Inherits FW_BaseRequestDTO
        Property Username As String
        Property Password As String
    End Class
    Public Class FW_BaseRequestRegistracionBasica
        Inherits FW_BaseRequestDTO
        Property Username As String
        Property Password As String
        Property FullName As String
        Property Email As String
    End Class
    Public Class FW_BaseRequestRegistracionCompleta
        Inherits FW_BaseRequestDTO
    End Class

    Public Class FW_BaseRequestId
        Inherits FW_BaseRequestDTO
        Property Id As Int64
    End Class

    Public Class FW_BaseRequestDictionary
        Inherits FW_BaseRequestDTO
        Property Values As New Dictionary(Of String, String)
    End Class

    Public Class FW_BaseRequestIds
        Inherits FW_BaseRequestDTO
        Property Ids As Int64()
    End Class

    Public Class FW_BaseRequestIdIds
        Inherits FW_BaseRequestDTO
        Property Id As Int64
        Property Ids As Int64()
    End Class

    Public Class FW_BaseRequestIdId
        Inherits FW_BaseRequestDTO
        <Required>
        <EnteroPositivo>
        Property Id_Who As Int64
        <Required>
        <EnteroPositivo>
        Property Id_To As Int64
    End Class

    Public Class FW_BaseRequestParameters
        Inherits FW_BaseRequestDTO
        Property Values As Dictionary(Of String, String)
    End Class

    Public Class FW_BaseRequestData
        Inherits FW_BaseRequestDTO
        Property Data As FW_BaseDAO
        Public ReadOnly Property ModelErrors As List(Of ValidationResult)
            Get
                Dim context = New ValidationContext(Me.Data, Nothing, Nothing)
                Dim results = New List(Of ValidationResult)
                Validator.TryValidateObject(Me.Data, context, results, True)
                Return results
            End Get
        End Property

        Public ReadOnly Property ModelIsValid As Boolean
            Get
                Dim context = New ValidationContext(Me.Data, Nothing, Nothing)
                Dim results = New List(Of ValidationResult)
                Dim Valid As Boolean = Validator.TryValidateObject(Me.Data, context, results, True)
                Return Valid
            End Get
        End Property
    End Class

    Public Class FW_BaseRequestVoid
        Inherits FW_BaseRequestDTO

    End Class

    Public Class FW_BaseRequestSearchText
        Inherits FW_BaseRequestDTO
        Property TextSearch As String
    End Class

    Public Class FW_BaseRequestStringValue
        Inherits FW_BaseRequestDTO
        Property Value As String
    End Class

    Public Class FW_BaseRequestSearchFilter
        Inherits FW_BaseRequestDTO
        Property TextSearch As Dictionary(Of String, String)
    End Class
End Namespace

