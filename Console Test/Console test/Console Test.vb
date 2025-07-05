Imports System
Imports System.Collections
Imports System.Collections.Specialized
Imports System.Diagnostics
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Text
Imports System.Threading
Imports FrameWork
Imports FrameWork.Bases
Imports FrameWork.Gestores
Imports Newtonsoft.Json.Linq
Imports Test_Services
Imports FrameWork.Bases.DTORequests
Imports FrameWork.Bases.DTOResponses
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.Collections.Generic
Imports System.Data
Imports System.Linq
Imports Microsoft.Office.Interop.Excel
Imports Newtonsoft.Json

Public Class App
    Private Shared Servicio As New Ges_WebServices
    Public Shared Sub Main()
        'Dim Test As New Testing.ClsTestServices
        ' TestWebApi()
        TestQueryENJson("select top 1 username,Password from usuarios")
    End Sub
    Public Shared Sub TestQueryENJson(SQL As String)
        Dim services As New Testing.ClsTestServices
        services.MostrarJsonDesdeSQL(SQL)
    End Sub
#Region "Rest Api"
    Public Shared Sub TestDescargar()
        Dim Diccionario As New Dictionary(Of String, String)
        With Diccionario
            .Add("Id1", 2)
            .Add("Id2", 2)
        End With
        Dim test As New Test_Services.Testing.ClsTestServices
        'test.Sesion.ObtenerInfoSocialLogin()
        'test.Sesion.AutenticacionSocialLogin("Twitter", 1)
        test.Sesion.AutenticacionBasica("naiosoft@hotmail.com", "091081")
        Dim response As String = test.Sesion.WebServices.ExecutePost("Actions/Generico", Diccionario)
        test.Sesion.Descargar("C:\Users\naios\Downloads\amazon.PNG")
    End Sub

    Public Shared Sub TestWebApi()
        Servicio.BaseUrl = "https://localhost:44366/WebApi/"
        Dim Token As String
        'Token = Autenticar("naiosoft@hotmail.com", "091081")
        Token = "joPG9Y2dNlIFloba9JgP0RbRPyQ1S8NNwKBfwJnR0rkqKxw8cVOBwkceixcajHHXNkidMSGFQSqw5e8SiJakCVcLoina7A8zScmdLlpcyjYZzE/SDjr//o2NnCMeiKp8HkyZAJFI3rtPt/9ZmRI1KhYtUx92bqnm5NYXX/SEyO9Rc9jFpOakj3L/LE99qmEyHoE66Aa1T7E="
        Servicio.NextRequest()
        Servicio.Endpoint = "Actions/Test"
        Dim Diccionario As New FW_BaseRequestDictionary
        With Diccionario.Values
            .Add("PlayerID", 2)
            .Add("UserID", 2)
        End With
        Servicio.SetBody(Diccionario)
        Servicio.BearerToken = Token
        Try
            Servicio.ExecutePost()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Public Shared Function Autenticar(Usuario As String, Password As String) As String
        Dim Token As String
        Dim IniciarSesion As New FW_BaseRequestBasicAuthentication
        With IniciarSesion
            .Username = Usuario
            .Password = Password
        End With
        With Servicio
            .NextRequest()
            .SetBody(IniciarSesion)
            .Endpoint = "Actions/login"
            .ExecutePost()
        End With
        Dim DatosSesion As FW_BaseResponseTokenInformation
        DatosSesion = Servicio.ResponseToObject(Of FW_BaseResponseTokenInformation)
        Token = DatosSesion.Data.Token
        Return Token
        'ShowResponse()
    End Function
#End Region
End Class








