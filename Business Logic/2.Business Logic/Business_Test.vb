Imports FrameWork.Bases
Imports FrameWork.DAO
Imports FrameWork.Extensiones
Imports FrameWork.Gestores
Imports FrameWork.Trace
Imports BusinessLogic
Imports FrameWork.Modulos
Imports FrameWork.Security
Imports FrameWork.Bases.DTORequests
Imports FrameWork.Bases.DTOResponses
Imports FrameWork.FW_ResponseService

Public Class Business_Test
    Inherits ClasesBases.Business.BL_BaseBusiness
    Public Sub New(Transaction As FW_BaseTransaction)
        MyBase.New(Transaction)
    End Sub

    Friend Sub Test()
        Dim Request As FW_BaseRequestVoid = RequestDTO
        Dim Response As New FW_BaseResponseVoid
        SetResponse(Response)
    End Sub
    Friend Sub ObtenerSocialLogin()
        Dim Request As FW_BaseRequestVoid = RequestDTO
        Dim Response As New FW_BaseResponseObject
        Response.SetData(Gestores.Seguridad.ObtenerListaSocialLoginsPublico().Data)
        SetResponse(Response)
    End Sub
    Friend Sub AutenticarConSocialLogin()
        Dim Request As FW_BaseRequestDictionary = RequestDTO
        Dim Response As New FW_BaseResponseTokenInformation
        SetResponse(Response)
        Dim Provider As String = Request.Values("state").Split("_").First
        Dim State As String = Request.Values("state").Split("_").Last
        Response.SetData(Gestores.Seguridad.AutenticarConSocialLogin(Provider, Request.Values, State).Data)
    End Sub
    Friend Sub AutenticacionBasica()

    End Sub
End Class
