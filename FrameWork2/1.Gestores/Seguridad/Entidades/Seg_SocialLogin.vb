Imports FrameWork.Bases
Imports FrameWork.DAO
Imports System.Linq
Imports System.Collections.Generic
Imports System.Web
Imports FrameWork.Modulos
Imports FrameWork.FW_ResponseService

Namespace Gestores

#Region "Servicio"
    Partial Public Class Ges_Seguridad
        Public Function ObtenerListaSocialLoginsPublico() As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListSocialLogin As List(Of DAO_SocialLogin)
            ListSocialLogin = ContextDB.SocialLogin.ToList
            Dim InfoSocialLogin As New List(Of PublicInfoSocialLogin)
            For Each Item As DAO_SocialLogin In ListSocialLogin
                Dim ItemInfoSocialLogin As New PublicInfoSocialLogin
                With ItemInfoSocialLogin
                    .Id = Item.Id
                    .ProviderName = Item.ProviderName
                    .URLRedirect = Item.URLGetAuthorizationCode
                    .ClientId = Item.ClientId
                    .TenantId = Item.TenantId
                    .VersionOAuth = Item.VersionOAuth
                    InfoSocialLogin.Add(ItemInfoSocialLogin)
                End With
            Next
            ResponseService.SetData(InfoSocialLogin)
            ResponseService.SetResultsByCountRecords(InfoSocialLogin.Count)
            Return ResponseService
        End Function

        Public Function ObtenerSocialLoginPorCodigo(ProveedorSocialLogin As String) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim SocialLoginObtener As DAO_SocialLogin
            SocialLoginObtener = (From SocialLogin
                                  In Me.ContextDB.SocialLogin
                                  Where SocialLogin.ProviderName = ProveedorSocialLogin
                                  Select SocialLogin
        ).FirstOrDefault
            If SocialLoginObtener Is Nothing Then
                ResponseService.SetResults(Enum_ResultsServices.Void)
            Else
                ResponseService.SetResults(Enum_ResultsServices.Single)
            End If
            ResponseService.SetData(SocialLoginObtener)
            Return ResponseService
        End Function

        Public Function SocialLoginRequestToken(Proveedor As String) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim Response As New Dictionary(Of String, String)
            Dim RedSocial As IRedSocial
            RedSocial = ObtenerRedSocial(Proveedor)
            RedSocial.RequestToken()
            Response.Add("oauth_token", RedSocial.Sesion.oauth_token)
            Response.Add("oauth_token_secret", RedSocial.Sesion.oauth_token_secret)
            Response.Add("oauth_callback_confirmed", RedSocial.Sesion.oauth_callback_confirmed)
            If Not RedSocial.Sesion.oauth_callback_confirmed Then
                Throw New ExceptionUnsuccessfully("Ocurrio un problema al solicitar el token")
            End If
            ResponseService.SetData(Response)
            Return ResponseService
        End Function

        Public Function AutenticarConSocialLogin(Proveedor As String, Params As Dictionary(Of String, String), State As String) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim DatosUsuario As New DAO_Usuario
            Dim SesionNueva As New DAO_SesionUsuario
            Dim ServiciosSeguridad As New Ges_Seguridad(Me.Transaction)
            Dim RedSocial As IRedSocial
            RedSocial = ObtenerRedSocial(Proveedor)
            With RedSocial
                .State = State
                .Params = Params
                .URL_Redirect = Modulos.AppConfig("SocialLoginURLCallBack")
                .GetAccessToken()
                .GetUserInfo()
                If ServiciosSeguridad.UsuarioExisteSocialLogin(.Sesion.User_Id, .SocialLogin.Id).IsTrue Then
                    DatosUsuario = ServiciosSeguridad.ObtenerUsuario(.Sesion.User_Id, .SocialLogin.Id).Data
                End If
                With DatosUsuario
                    .ProveedorSocialLoginId = RedSocial.SocialLogin.Id
                    .NombreCompleto = RedSocial.Sesion.screen_name
                    .Username = RedSocial.Sesion.User_name
                    .SocialLoginUserId = RedSocial.Sesion.User_Id
                    .Mail = RedSocial.Sesion.User_Email
                    .Foto = RedSocial.Sesion.User_Photo.AbsoluteUri
                    .Enabled = True
                    .Locked = False
                    .Password = ""
                    DatosUsuario = ServiciosSeguridad.GuardarUsuario(DatosUsuario).Data
                End With
                SesionNueva = ServiciosSeguridad.CrearSesionUsuario(DatosUsuario.Id, .Sesion.access_token, .SocialLogin.Id, .Sesion.expires_in).Data
                Dim Response As New TokenInformation
                Response.UserID = DatosUsuario.Id
                Response.Token = SesionNueva.Token
                Response.DisplayName = DatosUsuario.NombreCompleto
                Response.ExpirationDate = SesionNueva.ExpirationToken
                Response.URLPhoto = DatosUsuario.Foto
                ResponseService.SetData(Response)
            End With
            Return ResponseService
        End Function
#End Region

#Region "Clases de redes sociales"
        Public Function ObtenerRedSocial(Proveedor As String) As IRedSocial
            Dim RedSocial As IRedSocial
            Dim SocialLogin As DAO_SocialLogin
            SocialLogin = ObtenerSocialLoginPorCodigo(Proveedor).Data
            RedSocial = Activator.CreateInstance(Type.GetType("FrameWork.Modulos.RedSocial_" + Proveedor))
            RedSocial.ServiciosRedSocial = New Ges_RedSocial(RedSocial)
            RedSocial.SocialLogin = SocialLogin
            Return RedSocial
        End Function
    End Class
#End Region
End Namespace