Imports FrameWork.Bases
Imports FrameWork.DAO
Imports System.Linq
Imports System.Collections.Generic
Imports FrameWork.Modulos
Imports FrameWork.Gestores
Namespace Modulos
    Public Class RedSocial_Yahoo
        Implements IRedSocial
        Public Property Sesion As SocialLoginSesionInfo Implements IRedSocial.Sesion
        Public Property SocialLogin As DAO_SocialLogin Implements IRedSocial.SocialLogin
        Public Property Params As Dictionary(Of String, String) Implements IRedSocial.Params
        Public Property State As String Implements IRedSocial.State
        Public Property URL_Redirect As String Implements IRedSocial.URL_Redirect
        Public Property ServiciosRedSocial As Ges_RedSocial Implements IRedSocial.ServiciosRedSocial
        Public Property WebServicesConsumer As New Ges_WebServices Implements IRedSocial.WebServicesConsumer

        Public Sub GetAccessToken() Implements IRedSocial.GetAccessToken
            WebServicesConsumer = ServiciosRedSocial.GetAccessToken(Enum_GetAccessToken.BasicAuthorization)
            Sesion = WebServicesConsumer.ResponseToObject(Of SocialLoginSesionInfo)
        End Sub

        Public Sub GetUserInfo() Implements IRedSocial.GetUserInfo
            WebServicesConsumer = ServiciosRedSocial.GetUserInfo(Enum_GetUserInfo.Oauth2)
            Sesion.UserInformationJson = WebServicesConsumer.ResponseContentString
            Dim Info As InfoUserYahoo
            Info = Sesion.UserInformationJson.JsonToObject(Of InfoUserYahoo)
            Sesion.UserInformationObject = Info
            With Sesion
                .ProviderName = "Yahoo"
                .User_Email = Info.email
                .User_Id = Info.sub
                .User_Photo = New Uri(Info.picture)
                .User_name = Info.name
                .screen_name = Info.nickname
            End With
        End Sub

        Public Sub RequestToken() Implements IRedSocial.RequestToken
            Throw New NotImplementedException()
        End Sub
        Private Class ProfileImages
            Public Property image32 As String
            Public Property image64 As String
            Public Property image128 As String
            Public Property image192 As String
        End Class

        Private Class InfoUserYahoo
            Inherits SocialLoginUserInformation
            Public Property [sub] As String
            Public Property name As String
            Public Property given_name As String
            Public Property family_name As String
            Public Property locale As String
            Public Property email As String
            Public Property email_verified As Boolean
            Public Property profile_images As ProfileImages
            Public Property nickname As String
            Public Property gender As String
            Public Property picture As String
        End Class
    End Class
End Namespace

