Imports FrameWork.Bases
Imports FrameWork.DAO
Imports System.Linq
Imports FrameWork.Modulos
Imports System.Collections.Generic
Imports FrameWork.Gestores

Namespace Modulos
    Public Class RedSocial_Facebook
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
            Dim Info As InfoUserFacebook
            Info = Sesion.UserInformationJson.JsonToObject(Of InfoUserFacebook)()
            Sesion.UserInformationObject = Info
            With Sesion
                .ProviderName = "Facebook"
                .User_Photo = New Uri(Info.picture.data.url)
                .User_name = Info.name
                .User_Email = Info.email
                .User_Id = Info.id
                .screen_name = Info.name
            End With
        End Sub

        Public Sub RequestToken() Implements IRedSocial.RequestToken
            Throw New NotImplementedException()
        End Sub

        Private Class Data
            Public Property height As Integer
            Public Property is_silhouette As Boolean
            Public Property url As String
            Public Property width As Integer
        End Class

        Private Class Photo
            Public Property data As Data
        End Class

        Private Class InfoUserFacebook
            Inherits SocialLoginUserInformation
            Public Property id As String
            Public Property email As String
            Public Property name As String
            Public Property picture As Photo
        End Class
    End Class
End Namespace