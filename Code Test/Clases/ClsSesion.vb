Imports System.Collections.Generic
Imports System.Linq
Imports FrameWork
Imports FrameWork.Modulos
Imports FrameWork.Bases
Imports FrameWork.Bases.DTORequests
Imports FrameWork.Bases.DTOResponses

Public Class ClsSesion
    Public Property WebServices As New FrameWork.Gestores.Ges_WebServices
    Public Property DatosSocialLogin As List(Of PublicInfoSocialLogin)
    Public Property EndpointAutenticacionBasica As String = "Actions/login"
    Public Property EndPointSocialLogin As String = ""
    Public Property EndPointInformationSocialLogin As String = "Actions/GetInformationSocialLogin"
    Public Property EndPointURLRedirect As String = "https://localhost:44366/Web/Forms/Principal.html"
    Public Property EndPointBaseURL As String = "https://localhost:44366/WebApi/"
    Public Property EndPointRequestToken As String = "Actions/RequestToken"
    Public Property EndPointGetAccessToken As String = "Actions/LoginWithSocialLogin"
    Public Property EndPointDownload As String = "Actions/Descargar"
    Public Property AuthorizationCode As String

    Public Sub New()
        WebServices.BaseUrl = EndPointBaseURL
    End Sub
    Public Property Token As String
        Get
            Return WebServices.BearerToken
        End Get
        Set(value As String)
            WebServices.BearerToken = value
        End Set
    End Property

    Public Sub New(URLBase As String)
        BaseURL(URLBase)
    End Sub

    Public Sub BaseURL(URL As String)
        WebServices.Url = URL
    End Sub

    Public Sub AutenticacionBasica(Username As String, Password As String)
        Dim IniciarSesion As New FW_BaseRequestBasicAuthentication
        With IniciarSesion
            .Username = Username
            .Password = Password
        End With
        With WebServices
            .NextRequest()
            .SetBody(IniciarSesion)
            .Endpoint = EndpointAutenticacionBasica
            .ExecutePost()
        End With
        Dim DatosSesion As FW_BaseResponseTokenInformation
        DatosSesion = WebServices.ResponseToObject(Of FW_BaseResponseTokenInformation)
        Me.Token = DatosSesion.Data.Token
    End Sub
    Public Sub ObtenerInfoSocialLogin()
        WebServices.Endpoint = EndPointInformationSocialLogin
        WebServices.ExecuteGet()
        DatosSocialLogin = WebServices.ResponseContentString.JsonToObject(Of ClsSocialLogin).Data
    End Sub

    Public Sub AutenticacionSocialLogin(Proveedor As String, Version As Int16)
        Select Case Version
            Case 1
                AutenticarVersion1(Proveedor)
            Case 2
                AutenticarVersion2(Proveedor)
        End Select
    End Sub

    Public Sub AutenticarVersion2(Proveedor As String)
        Try
            Dim Item As PublicInfoSocialLogin = DatosSocialLogin.Where(Function(n) n.ProviderName = Proveedor).First
            'https://localhost:44366/Web/Forms/Principal.html
            'https%3A%2F%2Flocalhost%3A44366%2FWeb%2FForms%2FPrincipal.html
            Dim Url As String = Item.URLRedirect
            If Not String.IsNullOrEmpty(Item.TenantId) Then
                Url = Replace(Url, "{{tenant_id}}", Item.TenantId)
            End If
            If Item.URLRedirect.Contains("?") Then
                Url += "&"
            Else
                Url += "?"
            End If
            Dim UrlRedirect As String = Url + "client_id=" + Item.ClientId + "&redirect_uri=" + System.Web.HttpUtility.UrlEncode(Me.EndPointURLRedirect) + "&state=" + Item.ProviderName + "_" + "8968d0ac-98bf-d23b-3b0d-1055fcff72be"
            'Dim UrlRedirect As String = Url + "client_id=" + Item.ClientId + "&redirect_uri=https%3A%2F%2Flocalhost%3A44366%2FWeb%2FForms%2FPrincipal.html&state=" + Item.ProviderName + "_" + "8968d0ac-98bf-d23b-3b0d-1055fcff72be"
            Dim respuestaURL As String = InputBox(Proveedor, "Copiar y pegar", UrlRedirect)
            ObtenerToken(respuestaURL, 2)
        Catch ex As Exception

        End Try
    End Sub

    Public Sub AutenticarVersion1(Proveedor As String)
        Dim Endpoint As String = EndPointRequestToken
        Dim Data As New FW_BaseRequestDictionary
        Data.Values.Add("Proveedor", Proveedor)
        WebServices.Endpoint = Endpoint
        WebServices.SetBody(Data)
        WebServices.ExecutePost()
        Dim Response As ResponseRequestToken
        Response = WebServices.ResponseContentString.JsonToObject(Of ResponseRequestToken)
        Dim Item As PublicInfoSocialLogin = DatosSocialLogin.Where(Function(n) n.ProviderName = Proveedor).First
        Dim UrlRedirect As String = Replace(Item.URLRedirect, "{{oauth_token}}", Response.Data.oauth_token)
        UrlRedirect += "&state=" + Item.ProviderName + "_" + "8968d0ac-98bf-d23b-3b0d-1055fcff72be"
        Dim respuestaURL As String = InputBox(Proveedor, "Copiar y pegar", UrlRedirect)
        ObtenerToken(respuestaURL, 1)
    End Sub

    Private Sub ObtenerToken(URL As String, Version As Int16)
        Dim Endpoint As String = EndPointGetAccessToken
        Dim state As String = ObtenerParamRequest("state", URL)
        Dim Data As New FW_BaseRequestDictionary
        If Version = 1 Then
            Dim oauth_token As String = ObtenerParamRequest("oauth_token", URL)
            Dim oauth_verifier As String = ObtenerParamRequest("oauth_verifier", URL)
            Data.Values.Add("oauth_token", oauth_token)
            Data.Values.Add("oauth_verifier", oauth_verifier)
        Else
            Dim code As String = ObtenerParamRequest("code", URL)
            Data.Values.Add("code", code)
        End If
        Data.Values.Add("state", state)
        WebServices.Endpoint = Endpoint
        WebServices.SetBody(Data)
        WebServices.ExecutePost()
        Dim DatosSesion As FW_BaseResponseTokenInformation
        DatosSesion = WebServices.ResponseToObject(Of FW_BaseResponseTokenInformation)
        Token = DatosSesion.Data.Token
    End Sub

    Private Function ObtenerParamRequest(Parametro As String, uri As String) As String
        Dim Value As String
        Dim Params() As String = uri.Split("?").Last.Split("&")
        For Each Param As String In Params
            If Param.Split("=").First = Parametro Then
                Return Replace(Param, Parametro + "=", "")
            End If
        Next
        Return ""
    End Function

    Public Sub Descargar(Path As String)
        WebServices.NextRequest()
        WebServices.Endpoint = EndPointDownload
        Dim Diccionario As New Dictionary(Of String, String)
        Dim Req As New FW_BaseRequestStringValue
        Req.Value = Path
        WebServices.SetBody(Req)
        WebServices.ExecutePost()
    End Sub
End Class

Public Class ClsSocialLogin
    Inherits FW_BaseResponseDTO
    Property Data As List(Of FrameWork.Modulos.PublicInfoSocialLogin)
End Class

Public Class ResponseRequestToken
    Public Property Data As Data
    Public Property Summary As Summary
End Class

Public Class Data
    Public Property oauth_token As String
    Public Property oauth_token_secret As String
    Public Property oauth_callback_confirmed As String
End Class

Public Class Summary
    Public Property Messages As Messages
    Public Property HasValues As Boolean
    Public Property HasSystemError As Boolean
    Public Property UnAuthorized As Boolean
    Public Property HasSecurityWarning As Boolean
    Public Property ErrorId As Integer
    Public Property Unsuccessfully As Boolean
    Public Property TransactionID As String
    Public Property Ok As Boolean
    Public Property HasUserErrors As Boolean
    Public Property HasInformation As Boolean
End Class

Public Class Messages
    Public Property UserErrors As List(Of Object)
    Public Property Information As List(Of Object)
End Class