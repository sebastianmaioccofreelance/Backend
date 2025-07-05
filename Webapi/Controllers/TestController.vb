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

'<AuthorizeWebApi(Roles:="a", Users:="b", Permisos:="permiso1,permiso2", Grupos:="GrupoA,GrupoB")>
'<AuthorizeWebApi(Grupos:="Admin", Roles:="Admins")>

'<EnableCors("http://localhost:44366", "*", "*")>
'<EnableCors("*", "*", "*")>

Public Class TestController
    Inherits ApiController
    <OverrideAuthorization>
    <AuthorizeWebApi>
    <Route("Actions/Test")>
    <HttpPost, HttpGet>
    Public Function Test() As IHttpActionResult
        'Public Function Test(RequestDTO As FW_BaseRequestDictionary) As IHttpActionResult
        Dim Transaction As New Transaction_Test(New FW_BaseRequestVoid, Request)
        Transaction.Test()
        Return Ok(Transaction.ResponseDTO)
    End Function

    <OverrideAuthorization>
    <Route("Actions/Prueba")>
    <HttpPost, HttpGet>
    Public Function Prueba(RequestDTO As FW_BaseRequestDictionary) As IHttpActionResult
        Dim Transaction As New Transaction_Test(RequestDTO, Request)
        Transaction.Test()
        Return Ok(Transaction.ResponseDTO)
    End Function

    <OverrideAuthorization>
    <AuthorizeWebApi(Roles:="Admin")>
    <Route("Actions/ObtenerSocialLogin")>
        <HttpPost, HttpGet>
    Public Function ObtenerSocialLogin() As IHttpActionResult
        Dim RequestDTO As New FW_BaseRequestVoid
        Dim Transaction As New Transaction_Test(RequestDTO, Request)
        Transaction.ObtenerSocialLogin()
        Return Ok(Transaction.ResponseDTO)
    End Function

    <OverrideAuthorization>
                <Route("Actions/LoginWithSocialLogin")>
                    <System.Web.Http.AcceptVerbs("POST")>
                        <HttpPost>
    Public Function LoginWithSocialLogin(RequestDTO As FW_BaseRequestDictionary) As IHttpActionResult
        Dim Transaction As New Transaction_Test(RequestDTO, Request)
        Transaction.AutenticarConSocialLogin()
        Return Ok(Transaction.ResponseDTO)
    End Function

    <Route("Actions/Publicidad")>
                                <HttpPut>
    Public Function Publicidad_Modificar(RequestDTO As FW_BaseRequestDictionary) As IHttpActionResult

    End Function
    <Route("Actions/Publicidad")>
                                        <HttpDelete>
    Public Function Publicidad_Eliminar(RequestDTO As FW_BaseRequestDictionary) As IHttpActionResult

    End Function
    <Route("Actions/Publicidad")>
                                                <HttpGet>
    Public Function Publicidad_Obtener(RequestDTO As FW_BaseRequestDictionary) As IHttpActionResult

    End Function



End Class


