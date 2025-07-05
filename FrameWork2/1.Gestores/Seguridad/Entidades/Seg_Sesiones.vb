Imports FrameWork.Bases
Imports FrameWork.DAO
Imports System.Linq
Imports System.Collections.Generic
Imports System.Data.Entity.Migrations
Imports System.Web.Http
Imports System.Web.Http.Controllers
Imports System.Net.Http
Imports System.Text
Imports FrameWork.Gestores
Imports FrameWork.Extensiones
Imports FrameWork
Imports FrameWork.Modulos
Imports System.ComponentModel.DataAnnotations
Imports System.Web
Imports FrameWork.FW_ResponseService
Imports System.Web.Http.Filters

Namespace Gestores


    Public Class ControlRequestResponseWebApi
        Inherits ActionFilterAttribute

        Public Overrides Sub OnActionExecuting(actionContext As HttpActionContext)
            ' Lógica a ejecutar antes de que la acción del controlador se ejecute
            ' Puedes acceder al contexto de la acción (actionContext) para obtener información sobre la solicitud actual, etc.
        End Sub

        Public Overrides Sub OnActionExecuted(actionExecutedContext As HttpActionExecutedContext)
            ' Lógica a ejecutar después de que la acción del controlador se ha ejecutado
            ' Puedes acceder al resultado de la acción, información sobre la respuesta, etc. en actionExecutedContext
        End Sub
    End Class
    Public Class AuthorizeWebApiAttribute
        Inherits AuthorizeAttribute
        Public Sub New()
            MyBase.New
        End Sub
        Public Property Permisos As String
        Public Property Grupos As String = ""
        Public Property LogicEvaluationAndOr As AuthorizeLogicEvaluation = AuthorizeLogicEvaluation.LogicOr
        Public Overrides Sub OnAuthorization(ByVal actionContext As HttpActionContext)
            If Not Authorize(actionContext) Then
                MyBase.HandleUnauthorizedRequest(actionContext)
            End If
        End Sub

        Protected Overrides Sub HandleUnauthorizedRequest(ByVal actionContext As HttpActionContext)
            MyBase.HandleUnauthorizedRequest(actionContext)
        End Sub

        Private Function Authorize(ByVal actionContext As HttpActionContext) As Boolean
            Dim UsuarioAutorizado As Boolean = False
            Dim Context As FW_BaseDBContext
            Dim TransactionID As String = Utilities.GenerarTransactionID
            Dim Trace As New Ges_Trace(TransactionID)
            Context = New FW_BaseDBContext(AppConfig("ConnectionString"))
            Try
                Dim SesionUsuario As New InfoUserSession(Context)
                SesionUsuario.CargarSesionYPermisos(actionContext.Request)
                If SesionUsuario.SesionEsAutentica Then
                    If SesionUsuario.Usuario.Locked Or Not SesionUsuario.Usuario.Enabled Then
                        Dim ServicioSeguridad As New Gestores.Ges_Seguridad(Context)
                        ServicioSeguridad.CerrarSesion(SesionUsuario.Sesion.Id)
                        Throw New ExceptionSecurity("El usuario se encuentra inhabilitado o bloqueado")
                    End If
                End If
                UsuarioAutorizado = SesionUsuario.SesionEsAutentica
                If UsuarioAutorizado Then
                    If Not (String.IsNullOrEmpty(Me.Roles) And String.IsNullOrEmpty(Me.Permisos) And String.IsNullOrEmpty(Me.Grupos)) Then
                        Dim PerteneceAAlgunRol As Boolean = PerteneceA("Roles", SesionUsuario, Me.Roles)
                        Dim PerteneceAAlgunGrupo As Boolean = PerteneceA("Grupos", SesionUsuario, Me.Grupos)
                        Dim TienePermisos As Boolean = PerteneceA("Permisos", SesionUsuario, Me.Permisos)
                        Dim PerteneceAUsuariosAutorizados As Boolean = PerteneceA("Usuarios", SesionUsuario, Me.Users)
                        If LogicEvaluationAndOr = AuthorizeLogicEvaluation.LogicOr Then
                            UsuarioAutorizado = PerteneceAAlgunGrupo Or PerteneceAAlgunRol Or TienePermisos Or PerteneceAUsuariosAutorizados
                        Else
                            UsuarioAutorizado = PerteneceAAlgunGrupo And PerteneceAAlgunRol And TienePermisos And PerteneceAUsuariosAutorizados
                        End If
                    End If
                End If
                Context.Database.Connection.Close()
                If UsuarioAutorizado Then
                    Sesion.SesionActual = SesionUsuario
                End If
            Catch ex As Exception
                Trace.TraceException(ex)
                UsuarioAutorizado = False
                Context.Database.Connection.Close()
            End Try
            If Not UsuarioAutorizado Then
                Trace.TraceInformation("Autorizacion denegada", actionContext.Request.Headers.Authorization.ToJson)
            End If
            Return UsuarioAutorizado
        End Function

        Private Function PerteneceA(Tipo As String, Sesion As InfoUserSession, Lista As String) As Boolean
            If String.IsNullOrEmpty(Lista) Then
                Return Me.LogicEvaluationAndOr = AuthorizeLogicEvaluation.LogicAnd
            End If
            Dim ArrLista() As String = Lista.Split(",")
            For Each Item In ArrLista
                Select Case Tipo
                    Case "Roles"
                        If Sesion.PerteneceARol(Item) Then
                            Return True
                        End If
                    Case "Usuarios"
                        If Sesion.Usuario.Username.ToLower = Item And Sesion.Usuario.ProveedorSocialLoginId = 0 Then
                            Return True
                        End If
                    Case "Grupos"
                        If Sesion.PerteneceAGrupo(Item) Then
                            Return True
                        End If
                    Case "Permisos"
                        If Sesion.TienePermisos(Item) Then
                            Return True
                        End If
                End Select
            Next
            Return False
        End Function
        Public Enum AuthorizeLogicEvaluation
            LogicAnd = 0
            LogicOr = 1
        End Enum
    End Class

    Public Class WebApiRequestResponse
        Public Sub AgregarCabeceras(ByRef Response As System.Web.HttpResponse)
            'Response.Headers.Add("Access-Control-Allow-Origin", "https://localhost:3000")
            'Response.Headers.Add("Access-Control-Allow-Origin", "*")
            ''' Response.Headers.Add("Access-Control-Allow-Methods", "POST, GET, OPTIONS")
            'Response.Headers.Add("Access-Control-Allow-Headers", "Origin, Content-Type, X-Auth-Token")
            ''' Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With")
        End Sub
        Public Function ValidarRequest(ByRef Request As System.Web.HttpRequest, ByRef Response As System.Web.HttpResponse) As Boolean
            Dim RequestValido As Boolean = True
            If Not RequestValido Then
                Response.StatusCode = 401
            End If
        End Function
    End Class

    Partial Public Class Ges_Seguridad
        Public Function AutenticacionBasica(Username As String, Password As String) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ResponseToken As New TokenInformation
            Dim PasswordEncriptada As String = Password.Encriptar
            Dim UsuarioLogin As DAO_Usuario = ContextDB.Usuarios.Where(Function(n) n.Username = Username And n.Password = PasswordEncriptada).FirstOrDefault
            If Not UsuarioLogin Is Nothing Then
                If UsuarioLogin.Locked Then
                    Throw New ExceptionUserAuthenticationAuthorization("Usuario bloqueado")
                End If
                If Not UsuarioLogin.Enabled Then
                    Throw New ExceptionUserAuthenticationAuthorization("Usuario deshabilitado")
                End If
                Dim GuidSession As String = ""
                Dim GenerateGUID As Guid
                For i = 1 To 3
                    GenerateGUID = Guid.NewGuid()
                    GuidSession += GenerateGUID.ToString
                Next
                Dim InfoToken As String = UsuarioLogin.Id.ToString + "%" + UsuarioLogin.Username + "%" & UsuarioLogin.Password + "%" & GuidSession
                Dim Sesion As DAO_SesionUsuario
                Sesion = CrearSesionUsuario(UsuarioLogin.Id, InfoToken).Data
                ResponseToken.UserID = UsuarioLogin.Id
                ResponseToken.Token = Sesion.Token
                ResponseToken.Username = Username
                ResponseToken.DisplayName = UsuarioLogin.NombreCompleto
                ResponseToken.ExpirationDate = Sesion.ExpirationToken
                ResponseService.SetData(ResponseToken)
                ResponseService.SetTrue()
            Else
                ResponseService.SetFalse()
                Throw New ExceptionUserAuthenticationAuthorization("Usuario o password no valido")
            End If
            Return ResponseService
        End Function


        Public Function CrearSesionUsuario(IdUsuario As Int64, Token As String, Optional IdProveedor As Int64 = 0, Optional ExpirationToken As Int64 = 120) As FW_ResponseService
            Dim NuevaSesion As New DAO_SesionUsuario
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            With NuevaSesion
                .ExpirationToken = Now.AddSeconds(ExpirationToken)
                .FechaInicio = Now
                .Token = (IdProveedor.ToString + "%" + Token).Encriptar
                .IdUsuario = IdUsuario
            End With
            ContextDB.SesionesUsuario.AddOrUpdate(NuevaSesion)
            ResponseService.SetData(NuevaSesion)
            Save()
            Return ResponseService
        End Function

        Public Function CerrarSesionUsuario(IdUsuario As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim SesionesCerrar As List(Of DAO_SesionUsuario)
            SesionesCerrar = ObtenerSesionesPorUserID(IdUsuario).Data
            ContextDB.SesionesUsuario.RemoveRange(SesionesCerrar)
            Save()
            Return ResponseService
        End Function
        Public Function CerrarSesionUsuario(Usuario As DAO_Usuario) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim SesionesCerrar As List(Of DAO_SesionUsuario)
            SesionesCerrar = ObtenerSesionesPorUserID(Usuario.Id).Data
            ContextDB.SesionesUsuario.RemoveRange(SesionesCerrar)
            Save()
            Return ResponseService
        End Function

        Public Function CerrarTodasLasSesiones() As FW_ResponseService
            Transaction.ContextDB.Database.ExecuteSqlCommand("truncate table [SesionesUsuario]")
            Save()
        End Function

        Public Function CerrarSesion(SesionId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            CerrarSesion(ObtenerSesionPorID(SesionId).Data)
            Return ResponseService
        End Function

        Public Function CerrarSesion(Sesion As DAO_SesionUsuario) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ContextDB.SesionesUsuario.Remove(Sesion)
            Save()
            Return ResponseService
        End Function

        Public Function ObtenerSesionPorID(SesionId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim SesionObtener As DAO_SesionUsuario
            SesionObtener = (From Sesion
                         In Me.ContextDB.SesionesUsuario
                             Where Sesion.Id = SesionId
                             Select Sesion
        ).FirstOrDefault
            If SesionObtener Is Nothing Then
                ResponseService.SetResults(Enum_ResultsServices.Void)
            Else
                ResponseService.SetResults(Enum_ResultsServices.Single)
            End If
            Return ResponseService.SetData(SesionObtener)
        End Function

        Public Function ObtenerSesionesPorUserID(UserId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim SesionesUsuario As List(Of DAO_SesionUsuario)
            SesionesUsuario = (From Sesion In Me.ContextDB.SesionesUsuario Where Sesion.IdUsuario = UserId Select Sesion).ToList
            Return ResponseService.SetDataAndResults(SesionesUsuario, SesionesUsuario.Count)
        End Function
    End Class

    Public Class TokenInformation
        Property UserID As Int64
        Property Username As String
        Property DisplayName As String
        Property Token As String
        Property ExpirationDate As DateTime
        Property URLPhoto As String
    End Class

    Public Class InfoUserSession
        Property Usuario As DAO_Usuario
        Property Sesion As DAO_SesionUsuario
        Property Roles As New List(Of DAO_Rol)
        Property Grupos As New List(Of DAO_GrupoDeUsuarios)
        Property Permisos As New List(Of DAO_Permiso)
        Dim Context As FW_BaseDBContext

        Public Sub New()
            Dim Context As New FW_BaseDBContext(AppConfig("ConnectionString"))
        End Sub
        Public Sub New(Context As FW_BaseDBContext)
            Me.Context = Context
        End Sub
        Public ReadOnly Property SesionEsAnonima As Boolean
            Get
                Return (Sesion Is Nothing)
            End Get
        End Property
        Public ReadOnly Property SesionEsAutentica As Boolean
            Get
                Return Not SesionEsAnonima
            End Get
        End Property
        Public Sub CargarSesionYPermisos(Request As HttpRequestMessage)
            CargarSesion(Request)
            CargarPermisos()
        End Sub

        Public Function PerteneceARol(Rol As String) As Boolean
            Return Roles.Any(Function(n) n.NombreRol = Rol)
        End Function

        Public Function NoPerteneceARol(Rol As String) As Boolean
            Return Not PerteneceARol(Rol)
        End Function

        Public Function TienePermisos(Permiso As String) As Boolean
            Return Permisos.Any(Function(n) n.Nombre = Permiso)
        End Function

        Public Function NoTienePermisos(Permiso As String) As Boolean
            Return Not TienePermisos(Permiso)
        End Function

        Public Function PerteneceAGrupo(Grupo As String) As Boolean
            Return Grupos.Any(Function(n) n.NombreGrupoDeUsuarios = Grupo)
        End Function

        Public Function NoPerteneceAGrupo(Grupo As String) As Boolean
            Return Not PerteneceAGrupo(Grupo)
        End Function

        Public Function PerteneceARol(Rol As Int64) As Boolean
            Return Roles.Any(Function(n) n.Id = Rol)
        End Function

        Public Function NoPerteneceARol(Rol As Int64) As Boolean
            Return Not PerteneceARol(Rol)
        End Function

        Public Function TienePermisos(Permiso As Int64) As Boolean
            Return Permisos.Any(Function(n) n.Id = Permiso)
        End Function

        Public Function NoTienePermisos(Permiso As Int64) As Boolean
            Return Not TienePermisos(Permiso)
        End Function

        Public Function PerteneceAGrupo(Grupo As Int64) As Boolean
            Return Grupos.Any(Function(n) n.Id = Grupo)
        End Function

        Public Function NoPerteneceAGrupo(Grupo As Int64) As Boolean
            Return Not PerteneceAGrupo(Grupo)
        End Function

        Public Sub CargarPermisos()
            If SesionEsAnonima Then Exit Sub
            Dim UserId As Int64 = Me.Usuario.Id
            'UserId = 26

            Dim GruposUsuario As List(Of DAO_GrupoDeUsuarios)
            Dim RolesUsuario As List(Of DAO_Rol)
            Dim RolesDeGrupos As List(Of DAO_Rol)
            Dim PermisosUsuario As List(Of DAO_Permiso)
            Dim PermisosRoles As List(Of DAO_Permiso)
            Dim PermisosGrupos As List(Of DAO_Permiso)

            GruposUsuario = (From g In Context.GruposDeUsuarios
                             Join gu In Context.GruposDeUsuariosUsuarios
                                     On gu.IdGrupoDeUsuarios Equals g.Id
                             Where gu.IdUsuario = UserId
                             Select g).ToList

            RolesUsuario = (From r In Context.Roles
                            Join ur In Context.UsuariosRoles
                                On r.Id Equals ur.IdRol
                            Where ur.IdUsuario = UserId
                            Select r).ToList

            Dim ArrGrupos() As Int64 = GruposUsuario.Select(Function(n) n.Id).ToArray

            RolesDeGrupos = (From r In Context.Roles
                             Join rg In Context.RolesGruposDeUsuarios
                                     On r.Id Equals rg.IdRol
                             Where ArrGrupos.Contains(rg.IdGrupoDeUsuarios)
                             Select r).ToList

            RolesUsuario.AddRange(RolesDeGrupos)

            Dim ArrRoles() As Int64 = RolesUsuario.Select(Function(n) n.Id).ToArray

            PermisosUsuario = (From up In Context.UsuariosPermisos
                               Join p In Context.Permisos On p.Id Equals up.IdPermiso
                               Where up.IdUsuario = UserId
                               Select p).ToList

            PermisosRoles = (From rp In Context.RolesPermisos
                             Join p In Context.Permisos On p.Id Equals rp.IdPermiso
                             Where ArrRoles.Contains(rp.IdRol)
                             Select p).ToList

            PermisosGrupos = (From pg In Context.PermisosGruposDeUsuarios
                              Join p In Context.Permisos On p.Id Equals pg.IdPermiso
                              Where ArrGrupos.Contains(pg.IdGrupoDeUsuarios)
                              Select p).ToList

            Me.Roles.AddRange(RolesUsuario.Distinct)
            Me.Grupos.AddRange(GruposUsuario)
            Me.Permisos.AddRange(PermisosUsuario)
            Me.Permisos.AddRange(PermisosRoles)
            Me.Permisos.AddRange(PermisosGrupos)
            Me.Permisos = Me.Permisos.Distinct.ToList
        End Sub

        Public Sub CargarSesion(Request As HttpRequestMessage)
            Dim ServicioUsuarios As New Ges_Seguridad(Context)
            Dim UsuarioAnonimo As New DAO_Usuario
            With UsuarioAnonimo
                .Id = 0
                .NombreCompleto = "Anonimo"
                .Enabled = True
                .Locked = False
                .ProveedorSocialLoginId = 0
                .Username = "Anonimo"
                Me.Usuario = UsuarioAnonimo
            End With
            If Request Is Nothing Then Exit Sub
            If Not Request.Headers.Authorization Is Nothing Then

                If Request.Headers.Authorization.Scheme = "Basic" Then
                    Dim Authorization As String = Request.Headers.Authorization.Parameter.Base64ToString
                    Dim Username As String = Authorization.Split(":").First
                    Dim Password As String = Authorization.Split(":").Last.Encriptar
                    Dim ObtenerUsuario As DAO_Usuario =
                            (From Usuarios
                            In Context.Usuarios
                             Where
                                Usuarios.Username = Username And
                                Usuarios.Password = Password And
                                Not Usuarios.Locked And
                                Usuarios.Enabled).FirstOrDefault
                    If Not ObtenerUsuario Is Nothing Then
                        Me.Sesion = New DAO_SesionUsuario
                        Me.Sesion.Id = 0
                        Me.Sesion.IdUsuario = 0
                        Me.Sesion.Token = ""
                        Me.Sesion.FechaInicio = Now
                        Me.Sesion.ExpirationToken = Now
                    End If
                End If

                If Request.Headers.Authorization.Scheme = "Bearer" Then
                    Dim Token As String = Request.Headers.Authorization.Parameter
                    Dim SplitAuthorization As List(Of String) = Token.Desencriptar().Split("%").ToList()
                    If SplitAuthorization.Count <> 2 And SplitAuthorization.Count <> 5 Then
                        Exit Sub
                    End If
                    If SplitAuthorization.Count = 5 Then ' autenticacion original
                        Dim IdUsuario As Int64 = Convert.ToInt64(SplitAuthorization.Item(1))
                        Dim Username As String = SplitAuthorization.Item(2)
                        Dim Password As String = SplitAuthorization.Item(3)
                        Dim ObtenerUsuario As DAO_Usuario =
                            (From Usuarios
                            In Context.Usuarios
                             Where
                                Usuarios.Id = IdUsuario And
                                Usuarios.Username = Username And
                                Usuarios.Password = Password And
                                Not Usuarios.Locked And
                                Usuarios.Enabled).FirstOrDefault

                        If Not ObtenerUsuario Is Nothing Then
                            Me.Sesion = Context.SesionesUsuario.Where(Function(n) n.Token = Token And n.IdUsuario = IdUsuario And n.ExpirationToken > Now).FirstOrDefault
                            If Not IsNothing(Me.Sesion) Then
                                Me.Usuario = ObtenerUsuario
                            End If
                        End If
                    Else
                        Dim IdProveedor As Int64
                        Dim BearerToken As String
                        IdProveedor = Convert.ToInt64(SplitAuthorization(0))
                        BearerToken = SplitAuthorization(1)
                        Me.Sesion = (From Sesiones
                                    In Context.SesionesUsuario
                                     Where Sesiones.Token = Token).FirstOrDefault
                        Me.Usuario = Context.Usuarios.Where(Function(n) n.Id = Sesion.IdUsuario).FirstOrDefault
                    End If

                End If
            End If
            Context.Database.Connection.Close()
        End Sub
    End Class
End Namespace