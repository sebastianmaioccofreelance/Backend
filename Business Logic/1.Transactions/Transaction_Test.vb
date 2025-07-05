Imports FrameWork.Bases
Imports FrameWork.Security
Imports FrameWork.Extensiones
Imports System.Web
Imports System.Net
Imports BusinessLogic.Configuration
Imports ClasesBases
Imports FrameWork.Bases.DTORequests
Imports FrameWork.Bases.DTOResponses

Public Class Transaction_Test
    Inherits ClasesBases.BL_BaseTransaction
    Public Sub New(Request As FW_BaseRequestDTO, Optional RequestHttp As Http.HttpRequestMessage = Nothing)
        MyBase.New(Request, New BL_BaseDBContext, RequestHttp)
    End Sub


    Public Sub Test()
        Try
            InicializarTransaccion("Prueba")
            Business.Test.Test()
            FinalizarTransaccion()
        Catch ex As Exception
            Exception(ex, DiccionarioMensajes(Enum_Messages.Error_GetCliente))
        End Try
    End Sub
    Public Sub ObtenerSocialLogin()
        'Try
        InicializarTransaccion("Obtener social login")
            Business.Test.ObtenerSocialLogin()
            FinalizarTransaccion()
        'Catch ex As Exception

        ' End Try
    End Sub
    Public Sub AutenticarConSocialLogin()
        Try
            InicializarTransaccion("Autenticar con codigo de autorizacion")
            Business.Test.AutenticarConSocialLogin()
            FinalizarTransaccion()
        Catch ex As Exception
            Exception(ex, DiccionarioMensajes(Enum_Messages.Error_GetCliente))
        End Try
    End Sub
End Class
