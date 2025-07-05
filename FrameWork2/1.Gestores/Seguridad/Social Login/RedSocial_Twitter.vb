Imports FrameWork.Bases
Imports FrameWork.DAO
Imports System.Linq
Imports FrameWork.Modulos
Imports System.Collections.Generic
Imports FrameWork.Gestores
Namespace Modulos
    Public Class RedSocial_Twitter
        Implements IRedSocial
        Public Property Sesion As SocialLoginSesionInfo Implements IRedSocial.Sesion
        Public Property SocialLogin As DAO_SocialLogin Implements IRedSocial.SocialLogin
        Public Property Params As Dictionary(Of String, String) Implements IRedSocial.Params
        Public Property State As String Implements IRedSocial.State
        Public Property URL_Redirect As String Implements IRedSocial.URL_Redirect
        Public Property ServiciosRedSocial As Ges_RedSocial Implements IRedSocial.ServiciosRedSocial
        Public Property WebServicesConsumer As New Ges_WebServices Implements IRedSocial.WebServicesConsumer

        Public Sub GetAccessToken() Implements IRedSocial.GetAccessToken
            WebServicesConsumer = ServiciosRedSocial.GetAccessToken(Enum_GetAccessToken.OAuth1)
            Dim Response As String = WebServicesConsumer.ResponseContentString
            Dim ColValores As New Dictionary(Of String, String)
            For Each ClaveValor As String In Response.Split("&")
                Dim Clave As String = ClaveValor.Split("=").First
                Dim Valor As String = ClaveValor.Substring(Clave.Length + 1, ClaveValor.Length - Clave.Length - 1)
                ColValores.Add(Clave, Valor)
            Next
            Sesion = New SocialLoginSesionInfo
            Sesion.oauth_token = ColValores("oauth_token")
            Sesion.access_token = ColValores("oauth_token")
            Sesion.oauth_token_secret = ColValores("oauth_token_secret")
            Sesion.User_Id = ColValores("user_id")
            Sesion.screen_name = ColValores("screen_name")
        End Sub

        Public Sub GetUserInfo() Implements IRedSocial.GetUserInfo
            WebServicesConsumer = ServiciosRedSocial.GetUserInfo(Enum_GetUserInfo.OAuth1)
            Sesion.UserInformationJson = WebServicesConsumer.ResponseContentString
            Dim Info As InfoUserTwitter
            Info = Sesion.UserInformationJson.JsonToObject(Of InfoUserTwitter)
            Sesion.UserInformationObject = Info
            With Sesion
                .ProviderName = "Twitter"
                .User_name = Info.name
                .User_Id = Info.id
                .User_Photo = New Uri(Info.profile_image_url_https)
                .screen_name = Info.screen_name
            End With

        End Sub

        Public Sub RequestToken() Implements IRedSocial.RequestToken
            Sesion = New SocialLoginSesionInfo
            WebServicesConsumer = ServiciosRedSocial.RequestToken(Enum_RequestToken.Oauth1)
            Dim SplitResponse() As String = WebServicesConsumer.ResponseContentString.Split("&")
            Sesion.oauth_token = SplitResponse(0).Split("=").Last
            Sesion.oauth_token_secret = SplitResponse(1).Split("=").Last
            Sesion.oauth_callback_confirmed = Convert.ToBoolean(SplitResponse(2).Split("=").Last)
        End Sub


        Private Class Description
            Public Property urls As List(Of Object)
        End Class

        Private Class Entities
            Public Property description As Description
        End Class

        Private Class InfoUserTwitter
            Inherits SocialLoginUserInformation
            Public Property id As Long
            Public Property id_str As String
            Public Property name As String
            Public Property screen_name As String
            Public Property location As String
            Public Property description As String
            Public Property url As Object
            Public Property entities As Entities
            Public Property [protected] As Boolean
            Public Property followers_count As Integer
            Public Property friends_count As Integer
            Public Property listed_count As Integer
            Public Property created_at As String
            Public Property favourites_count As Integer
            Public Property utc_offset As Object
            Public Property time_zone As Object
            Public Property geo_enabled As Boolean
            Public Property verified As Boolean
            Public Property statuses_count As Integer
            Public Property lang As Object
            Public Property contributors_enabled As Boolean
            Public Property is_translator As Boolean
            Public Property is_translation_enabled As Boolean
            Public Property profile_background_color As String
            Public Property profile_background_image_url As Object
            Public Property profile_background_image_url_https As Object
            Public Property profile_background_tile As Boolean
            Public Property profile_image_url As String
            Public Property profile_image_url_https As String
            Public Property profile_link_color As String
            Public Property profile_sidebar_border_color As String
            Public Property profile_sidebar_fill_color As String
            Public Property profile_text_color As String
            Public Property profile_use_background_image As Boolean
            Public Property has_extended_profile As Boolean
            Public Property default_profile As Boolean
            Public Property default_profile_image As Boolean
            Public Property following As Boolean
            Public Property follow_request_sent As Boolean
            Public Property notifications As Boolean
            Public Property translator_type As String
            Public Property withheld_in_countries As List(Of Object)
            Public Property suspended As Boolean
            Public Property needs_phone_verification As Boolean
        End Class


















































    End Class
End Namespace

