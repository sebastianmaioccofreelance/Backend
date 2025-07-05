Imports FrameWork.Bases
Imports FrameWork.DAO
Imports System.Linq
Imports System.Collections.Generic
Imports FrameWork.Modulos
Imports System.Data.Entity.Migrations
Imports FrameWork.FW_ResponseService

Namespace Gestores

    Partial Public Class Ges_Seguridad
        Inherits FW_BaseGestor

        Public Sub New(Transaction As FW_BaseTransaction)
            MyBase.New(Transaction)
        End Sub

        Public Sub New(Context As FW_BaseDBContext)
            MyBase.New(Context)
        End Sub
        Public Sub New(Servicio As FW_BaseGestor)
            MyBase.New(Servicio.Transaction)
            Me.ContextDB = Servicio.ContextDB
        End Sub

        Public Function UsuarioExiste(IdUsuario As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim Result As Boolean = ContextDB.Usuarios.Any(Function(n) n.Id = IdUsuario)
            ResponseService.SetResultsByBooleanValue(Result)
            Return ResponseService
        End Function

        Public Function UsuarioNoExiste(IdUsuario As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ServiciosSeguridad As New Ges_Seguridad(Me.Transaction)
            ResponseService.SetResultsByBooleanValue(ServiciosSeguridad.UsuarioExiste(IdUsuario).IsFalse)
            Return ResponseService
        End Function

        Public Function UsuarioExisteUsername(Username As String) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim Result As Boolean = ContextDB.Usuarios.Any(Function(n) n.Username = Username And n.ProveedorSocialLoginId = 0)
            ResponseService.SetResultsByBooleanValue(Result)
            Return ResponseService
        End Function

        Public Function UsuarioNoExisteUsername(Username As String) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ServiciosSeguridad As New Ges_Seguridad(Me.Transaction)
            ResponseService.SetResultsByBooleanValue(ServiciosSeguridad.UsuarioExisteUsername(Username).IsFalse)
            Return ResponseService
        End Function

        Public Function UsuarioNoExisteSocialLogin(IdUsuarioSocialLogin As String, ProveedorId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ServiciosSeguridad As New Ges_Seguridad(Me.Transaction)
            ResponseService.SetResultsByBooleanValue(ServiciosSeguridad.ObtenerUsuario(IdUsuarioSocialLogin, ProveedorId).IsVoid)
            Return ResponseService
        End Function

        Public Function UsuarioExisteSocialLogin(IdUsuarioSocialLogin As String, ProveedorId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ServiciosSeguridad As New Ges_Seguridad(Me.Transaction)
            ResponseService.SetResultsByBooleanValue(ServiciosSeguridad.UsuarioNoExisteSocialLogin(IdUsuarioSocialLogin, ProveedorId).IsFalse)
            Return ResponseService
        End Function

        Public Function ObtenerUsuario(IdUsuarioSocialLogin As String, ProveedorId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim UsuarioObtener As DAO_Usuario
            UsuarioObtener = (From Usuario
                              In ContextDB.Usuarios
                              Where Usuario.SocialLoginUserId = IdUsuarioSocialLogin And ProveedorId = Usuario.ProveedorSocialLoginId
                              Select Usuario).
                              FirstOrDefault
            If UsuarioObtener Is Nothing Then
                ResponseService.SetResultsVoid()
            Else
                ResponseService.SetResultsSingle()
                ResponseService.SetData(UsuarioObtener)
            End If
            Return ResponseService
        End Function

        Public Function ObtenerUsuarioConMail(Mail As String) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim UsuarioObtener As DAO_Usuario
            UsuarioObtener = (From Usuario
                              In ContextDB.Usuarios
                              Where Usuario.Mail = Mail
                              Select Usuario).
                              FirstOrDefault
            If UsuarioObtener Is Nothing Then
                ResponseService.SetResultsVoid()
            Else
                ResponseService.SetResultsSingle()
            End If
            ResponseService.SetData(UsuarioObtener)
            Return ResponseService
        End Function

        Public Function ObtenerUsuario(IdUsuario As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim UsuarioObtener As DAO_Usuario
            UsuarioObtener = (From Usuario
                              In ContextDB.Usuarios
                              Where Usuario.Id = IdUsuario
                              Select Usuario).
                              FirstOrDefault
            If UsuarioObtener Is Nothing Then
                ResponseService.SetResultsVoid()
            Else
                ResponseService.SetResultsSingle()
            End If
            ResponseService.SetData(UsuarioObtener)
            Return ResponseService
        End Function

        Public Function ObtenerListaUsuarios(Optional SoloHabilitados As Boolean = False) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaUsuarios As List(Of DAO_Usuario)
            If SoloHabilitados Then
                ListaUsuarios = ContextDB.Usuarios.
                Where(Function(n) n.Enabled).
                OrderBy(Function(n) n.NombreCompleto).ToList
            Else
                ListaUsuarios = ContextDB.Usuarios.
                    OrderBy(Function(n) n.NombreCompleto).
                    ToList
            End If
            ResponseService.SetDataAndResults(ListaUsuarios, ListaUsuarios.Count)
            Return ResponseService
        End Function

        Public Function GuardarUsuario(Usuario As DAO_Usuario) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Usuario.EncriptarPassword
            ContextDB.Usuarios.AddOrUpdate(Usuario)
            Save()
            ResponseService.SetData(Usuario)
            Return ResponseService
        End Function

        Public Function EliminarUsuario(IdUsuario As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ServiciosSeguridad As New Ges_Seguridad(Me.Transaction)
            Dim UsuarioEliminar As DAO_Usuario
            UsuarioEliminar = ServiciosSeguridad.ObtenerUsuario(IdUsuario).
                          Unsuccessfully(Enum_ResultsServices.Void, "No se encontro el usuario a eliminar").
                          Data
            ContextDB.Usuarios.Remove(UsuarioEliminar)
            Save()
            Return ResponseService
        End Function

        Public Function EliminarUsuarios(IdsUsuario() As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each UserId As Int64 In IdsUsuario
                EliminarUsuario(UserId)
            Next
            Return ResponseService
        End Function

        Public Function HabilitarUsuario(IdUsuario As Int64) As FW_ResponseService
            Dim ServiciosSeguridad As New Ges_Seguridad(Me)
            Return ServiciosSeguridad.CambiarAtributoUsuario(IdUsuario, "Enabled", True, ServiciosSeguridad)
        End Function

        Public Function InhabilitarUsuario(IdUsuario As Int64) As FW_ResponseService
            Dim ServiciosSeguridad As New Ges_Seguridad(Me)
            Return ServiciosSeguridad.CambiarAtributoUsuario(IdUsuario, "Enabled", False, ServiciosSeguridad)
        End Function

        Public Function HabilitarUsuarios(IdsUsuario() As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each UserId As Int64 In IdsUsuario
                HabilitarUsuario(UserId)
            Next
            Return ResponseService
        End Function

        Public Function InhabilitarUsuarios(IdsUsuario() As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each UserId As Int64 In IdsUsuario
                InhabilitarUsuario(UserId)
            Next
            Return ResponseService
        End Function

        Public Function BloquearUsuario(IdUsuario As Int64) As FW_ResponseService
            Dim ServiciosSeguridad As New Ges_Seguridad(Me)
            Return ServiciosSeguridad.CambiarAtributoUsuario(IdUsuario, "Locked", True, ServiciosSeguridad)
        End Function

        Public Function BloquearUsuarios(IdsUsuario() As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each UserId As Int64 In IdsUsuario
                BloquearUsuario(UserId)
            Next
            Return ResponseService
        End Function

        Public Function DesbloquearUsuario(IdUsuario As Int64) As FW_ResponseService
            Dim ServiciosSeguridad As New Ges_Seguridad(Me)
            Return ServiciosSeguridad.CambiarAtributoUsuario(IdUsuario, "Locked", False, ServiciosSeguridad)
        End Function

        Public Function DesbloquearUsuarios(IdsUsuario() As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each UserId As Int64 In IdsUsuario
                DesbloquearUsuario(UserId)
            Next
            Return ResponseService
        End Function

        Private Function CambiarAtributoUsuario(IdUsuario As Int64, Atributo As String, Valor As Object, ServiciosSeguridad As Ges_Seguridad) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim UsuarioModificar As DAO_Usuario
            UsuarioModificar = ServiciosSeguridad.ObtenerUsuario(IdUsuario).
                        Unsuccessfully(Enum_ResultsServices.Void, "No se encontro el usuario.").
                        Data
            Select Case Atributo
                Case "Enabled"
                    UsuarioModificar.Enabled = Valor
                Case "Locked"
                    UsuarioModificar.Locked = Valor
                Case "push_notification_player_id"
                    UsuarioModificar.PushNotificationPlayerID = Valor
            End Select
            Save()
            Return ResponseService
        End Function
        Public Function AsignarPlayerIdAUsuario(IdUsuario As Int64, PlayerID As String)
            Dim ServiciosSeguridad As New Ges_Seguridad(Me)
            CambiarAtributoUsuario(IdUsuario, "push_notification_player_id", PlayerID, ServiciosSeguridad)
        End Function

        Public Function AsignarPlayerIdAUsuario(Usuario As DAO_Usuario, PlayerID As String)
            AsignarPlayerIdAUsuario(Usuario.Id, PlayerID)
        End Function
        Public Function GenerarPasswordTemporal() As String
            Dim NuevaPassword As String
        End Function

    End Class
End Namespace