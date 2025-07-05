Imports System.Web.Optimization

Public Class Global_asax
    Inherits HttpApplication

    Public Sub New()

    End Sub

    Protected Sub Application_Start(sender As Object, e As EventArgs)

    End Sub
    Protected Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        Dim ex As Exception = Server.GetLastError()
        Dim path As String = "/"
        If TypeOf sender Is HttpApplication Then
            path = (CType(sender, HttpApplication)).Request.Url.PathAndQuery
        End If

    End Sub

    Private Sub Global_asax_BeginRequest(sender As Object, e As EventArgs) Handles Me.BeginRequest
        Dim req As HttpRequest = Request

    End Sub
End Class