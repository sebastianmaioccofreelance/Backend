Imports System.Web.Http
Imports System.Web.Optimization
Imports System.Data.Entity

Public Class WebApiApplication
    Inherits System.Web.HttpApplication

    Protected Sub Application_Start()
        AreaRegistration.RegisterAllAreas()
        GlobalConfiguration.Configure(AddressOf WebApiConfig.Register)
        'FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters)
        'Database.SetInitializer(Of MyContext)(Nothing) ' inicializa contexto de entity framework
        BusinessLogic.Mod_Configurations.InicializarAplicacion(False)
    End Sub
    Sub Application_BeginRequest(sender As Object, e As EventArgs)
        Dim SeguridadWebApi As New FrameWork.Gestores.WebApiRequestResponse
        If Request.HttpMethod = "OPTIONS" Then
            'Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:3000/")
            Response.Headers.Add("Access-Control-Allow-Headers", "authorization")
            Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST")
            Response.End()
        End If
        SeguridadWebApi.ValidarRequest(Request, Response)
        SeguridadWebApi.AgregarCabeceras(Response)
    End Sub

    Protected Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        Dim ex As Exception = Server.GetLastError()
        Dim path As String = "/"
        If TypeOf sender Is HttpApplication Then
            path = (CType(sender, HttpApplication)).Request.Url.PathAndQuery
        End If
        Dim TransactionID As String
        TransactionID = FrameWork.Gestores.Utilities.GenerarTransactionID()
        Dim Trace As New FrameWork.Gestores.Ges_Trace(TransactionID)
        Trace.TraceInformation("Global asax", "Error")
        Trace.TraceException(ex)
    End Sub

    'Private Sub WebApiApplication_BeginRequest(sender As Object, e As EventArgs) Handles Me.BeginRequest
    '    Dim r As HttpRequest = Request
    'End Sub

    Private Sub WebApiApplication_Error(sender As Object, e As EventArgs) Handles Me.[Error]
        Stop
    End Sub

    Private Sub WebApiApplication_PreRequestHandlerExecute(sender As Object, e As EventArgs) Handles Me.PreRequestHandlerExecute

    End Sub

    Private Sub WebApiApplication_LogRequest(sender As Object, e As EventArgs) Handles Me.LogRequest

    End Sub

    Private Sub WebApiApplication_RequestCompleted(sender As Object, e As EventArgs) Handles Me.RequestCompleted

    End Sub
End Class
