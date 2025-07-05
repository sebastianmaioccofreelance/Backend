Imports FrameWork.Bases
Imports FrameWork.DAO
Imports FrameWork.Modulos
Imports System.Linq
Imports System.Collections.Generic
Imports FrameWork.Gestores

Namespace Modulos
    Public Class RedSocial_Google
        Implements IRedSocial
        Public Property Sesion As SocialLoginSesionInfo Implements IRedSocial.Sesion
        Public Property SocialLogin As DAO_SocialLogin Implements IRedSocial.SocialLogin
        Public Property Params As Dictionary(Of String, String) Implements IRedSocial.Params
        Public Property State As String Implements IRedSocial.State
        Public Property URL_Redirect As String Implements IRedSocial.URL_Redirect
        Public Property ServiciosRedSocial As Ges_RedSocial Implements IRedSocial.ServiciosRedSocial
        Public Property WebServicesConsumer As New Ges_WebServices Implements IRedSocial.WebServicesConsumer

        Public Sub GetAccessToken() Implements IRedSocial.GetAccessToken
            WebServicesConsumer = ServiciosRedSocial.GetAccessToken(Enum_GetAccessToken.ParamsOnQueryString)
            Sesion = WebServicesConsumer.ResponseToObject(Of SocialLoginSesionInfo)
        End Sub

        Public Sub GetUserInfo() Implements IRedSocial.GetUserInfo
            WebServicesConsumer = ServiciosRedSocial.GetUserInfo(Enum_GetUserInfo.Oauth2)
            Sesion.UserInformationJson = WebServicesConsumer.ResponseContentString
            Dim Info As InfoUserGoogle
            Info = Sesion.UserInformationJson.JsonToObject(Of InfoUserGoogle)
            Sesion.UserInformationObject = Info
            With Sesion
                .ProviderName = "Google"
                .User_Email = Info.email
                .User_Id = Info.id
                .User_Photo = New Uri(Info.picture)
                .User_name = Info.email
                .screen_name = Info.email
            End With
        End Sub

        Public Sub RequestToken() Implements IRedSocial.RequestToken
            Throw New NotImplementedException()
        End Sub

        Private Class InfoUserGoogle
            Inherits SocialLoginUserInformation
            Property id As String
            Property email As String
            Property verified_email As Boolean
            Property picture As String
        End Class
    End Class
End Namespace

