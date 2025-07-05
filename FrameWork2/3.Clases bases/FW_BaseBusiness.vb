Imports FrameWork.DAO
Imports FrameWork.Gestores
Imports System.ComponentModel.DataAnnotations
Imports FrameWork.Extensiones
Imports DesktopTest.Configuration.Idiomas
Imports FrameWork.Trace
Imports FrameWork.Bases.DTORequests
Imports FrameWork.Bases.DTOResponses
Imports FrameWork.Security
Imports FrameWork.Bases

Namespace Bases
    Public Class FW_BaseBusiness
        Public Sub New(Transaction As FW_BaseTransaction)
            'Me.Transaction = Action.Transaction
            Me.Transaction = Transaction
        End Sub
#Region "Propiedades"

        Public Property Utilidades As Ges_Utilities
        Public Property Transaction As FW_BaseTransaction

        Public ReadOnly Property Ok As Boolean
            Get
                Return Me.Transaction.ResponseDTO.Summary.Ok
            End Get
        End Property

        Public ReadOnly Property Sesion As InfoUserSession
            Get
                Return Transaction.Sesion
            End Get
        End Property
        Public ReadOnly Property Usuario As DAO_Usuario
            Get
                Return Transaction.Sesion.Usuario
            End Get
        End Property

        Public ReadOnly Property RequestDTO As FW_BaseRequestDTO
            Get
                Return Transaction.RequestDTO
            End Get
        End Property
        Public ReadOnly Property Gestores As FW_BaseGestores
            Get
                Return Transaction.Gestores
            End Get
        End Property
#End Region

#Region "Respuestas, validaciones y errores"
        Public Sub EndUnsuccessfully(Mensaje)
            Throw New ExceptionUnsuccessfully(Mensaje)
        End Sub

        Public Sub SetResponse(Response As FW_BaseResponseDTO)
            Etapa("retorna respuesta")
            Response.Summary.TransactionID = Transaction.TransactionID
            Transaction.ResponseDTO = Response
        End Sub

        Public Function TienePermisos(Permiso As String) As Boolean
            Return Transaction.Sesion.TienePermisos(Permiso)
        End Function

        Public Function NoTienePermisos(Permiso As String) As Boolean
            Return Transaction.Sesion.NoTienePermisos(Permiso)
        End Function

        Public Function PerteneceARol(Rol As String) As Boolean
            Return Transaction.Sesion.PerteneceARol(Rol)
        End Function

        Public Function NoPerteneceARol(Rol As String) As Boolean
            Return Transaction.Sesion.NoPerteneceARol(Rol)
        End Function

        Public Sub ExitIfUserErrors()
            Evento("Valida que no existan datos mal cargados por el usuario", TiposDeEvento.Etapa)
            If Transaction.ResponseDTO.Summary.HasUserErrors Then
                Evento("Finaliza por error del usuario", TiposDeEvento.ErrorUsuario)
                Throw New ExceptionUserAuthenticationAuthorization("El usuario no cargo correctamente los datos")
            End If
        End Sub

        Public Sub ValidateModel()
            Evento("Valida la solicitud", TiposDeEvento.Etapa)
            Dim RequestTable As FW_BaseRequestData
            RequestTable = Transaction.RequestDTO
            For Each ItemError As ValidationResult In RequestTable.ModelErrors
                Transaction.ResponseDTO.UserError(ItemError.ErrorMessage)
            Next
        End Sub

        Public Sub ValidateInputUser(IsValid As Boolean, Message As String)
            Transaction.ResponseDTO.Validate(IsValid, Message)
        End Sub

        Public Sub ValidateSecurity(IsValid As Boolean, Message As String)
            If Not IsValid Then
                Throw New ExceptionSecurity(Message)
            End If
        End Sub

        Public Sub ErrorUsuario(Mensaje As String)
            Transaction.Evento(Mensaje, TiposDeEvento.ErrorUsuario)
        End Sub

        Public Sub SetHasValues(Optional Value As Boolean = True)
            Transaction.ResponseDTO.Summary.HasValues = Value
        End Sub
#End Region

#Region "Trazabilidad"
        Public Sub Evento(Mensaje As String, TipoEvento As TiposDeEvento)
            Transaction.Evento(Mensaje, TipoEvento)
        End Sub

        Public Sub Etapa(Mensaje)
            Transaction.Evento(Mensaje, TiposDeEvento.Etapa)
        End Sub

        Public Sub Accion(Mensaje)
            Transaction.Evento(Mensaje, TiposDeEvento.Accion)
        End Sub

        Public Sub Informacion(Mensaje)
            Transaction.Evento(Mensaje, TiposDeEvento.Informacion)
        End Sub
#End Region

    End Class
End Namespace

