Imports FrameWork.Bases
Imports FrameWork.DAO
Imports FrameWork.Modulos
Imports System.Linq
Imports System.Collections.Generic
Imports FrameWork.Gestores
Namespace Modulos
    Public Class RedSocial_Instagram
        Implements IRedSocial
        Public Property Sesion As SocialLoginSesionInfo Implements IRedSocial.Sesion
        Public Property SocialLogin As DAO_SocialLogin Implements IRedSocial.SocialLogin
        Public Property Params As Dictionary(Of String, String) Implements IRedSocial.Params
        Public Property State As String Implements IRedSocial.State
        Public Property URL_Redirect As String Implements IRedSocial.URL_Redirect
        Public Property ServiciosRedSocial As Ges_RedSocial Implements IRedSocial.ServiciosRedSocial
        Public Property WebServicesConsumer As Ges_WebServices Implements IRedSocial.WebServicesConsumer

        Public Sub GetAccessToken() Implements IRedSocial.GetAccessToken
            WebServicesConsumer = ServiciosRedSocial.GetAccessToken(Enum_GetAccessToken.ParamsOnBody)
            Sesion = WebServicesConsumer.ResponseToObject(Of SocialLoginSesionInfo)
        End Sub

        Public Sub GetUserInfo() Implements IRedSocial.GetUserInfo
            WebServicesConsumer = ServiciosRedSocial.GetUserInfo(Enum_GetUserInfo.Oauth2)
            Sesion.UserInformationJson = WebServicesConsumer.ResponseContentString
            Dim Info As InfoUserInstagram
            Info = Sesion.UserInformationJson.JsonToObject(Of InfoUserInstagram)
            Sesion.UserInformationObject = Info
            With Sesion
                .ProviderName = "Instagram"
                .User_Id = Info.id
                .User_name = Info.username
                .screen_name = Info.username
            End With
        End Sub

        Public Sub RequestToken() Implements IRedSocial.RequestToken
            Throw New NotImplementedException()
        End Sub

        Private Class InfoUserInstagram
            Inherits SocialLoginUserInformation
            Property id As String = ""
            Property username As String = ""
        End Class
    End Class
End Namespace

