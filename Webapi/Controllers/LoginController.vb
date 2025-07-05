Imports System.Net
Imports FrameWork.Extensiones
Imports System.Web.Http
Imports FrameWork.Security
Imports FrameWork
Imports BusinessLogic
Imports FrameWork.Gestores.Extensiones
Imports FrameWork.Bases
Imports FrameWork.Modulos
Imports FrameWork.Gestores
Imports FrameWork.Bases.DTORequests
Imports FrameWork.Bases.DTOResponses
Imports System.Web.Http.Cors
<RoutePrefix("Login")>
Public Class LoginController
    Inherits ApiController
    <OverrideAuthorization>
    <Route("ObtenerSocialLogin")>
    <HttpPost, HttpGet>
    Public Function ObtenerSocialLogin() As IHttpActionResult
        Dim RequestDTO As New FW_BaseRequestVoid
        Dim Transaction As New Transaction_Login(RequestDTO, Request)
        Transaction.ObtenerSocialLogin()
        Return Ok(Transaction.ResponseDTO)
    End Function

    <OverrideAuthorization>
    <Route("LoginWithSocialLogin")>
    <HttpPost>
    Public Function LoginWithSocialLogin(RequestDTO As FW_BaseRequestDictionary) As IHttpActionResult
        Dim Transaction As New Transaction_Test(RequestDTO, Request)
        Transaction.AutenticarConSocialLogin()
        Return Ok(Transaction.ResponseDTO)
    End Function

    <OverrideAuthorization>
    <Route("Login")>
    <HttpPost>
    Public Function Login(RequestDTO As FW_BaseRequestBasicAuthentication) As IHttpActionResult
        Dim Transaction As New Transaction_Login(RequestDTO, Request)
        Transaction.Login()
        If Not Transaction.ResponseDTO.Summary.Ok Then
            Return Unauthorized()
        End If
        Return Ok(Transaction.ResponseDTO)
    End Function
    <OverrideAuthorization>
    <Route("Test")>
    <HttpPost, HttpGet>
    Public Function Test(RequestDTO As FW_BaseRequestBasicAuthentication) As IHttpActionResult
        Return Ok("ok")
    End Function

End Class
