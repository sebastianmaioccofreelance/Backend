Imports System.Collections.Generic
Imports FrameWork.DAO
Imports FrameWork.Gestores

Namespace Modulos
    Public Interface IRedSocial
        Property Sesion As SocialLoginSesionInfo
        Property SocialLogin As DAO_SocialLogin
        Property Params As Dictionary(Of String, String)
        Property ServiciosRedSocial As Ges_RedSocial
        Property WebServicesConsumer As Ges_WebServices
        Property State As String
        Property URL_Redirect As String
        Sub RequestToken()
        Sub GetAccessToken()
        Sub GetUserInfo()
    End Interface

    Public Class PublicInfoSocialLogin
        Property Id As Int64
        Property ProviderName As String
        Property URLRedirect As String
        Property ClientId As String
        Property State As String
        Property ParamsRequest As String
        Property TenantId As String
        Property VersionOAuth As Int32
    End Class

    Public Class SocialLoginSesionInfo
        Property ProviderName As String
        Property access_token As String
        Property oauth_token As String
        Property oauth_token_secret As String
        Property oauth_callback_confirmed As Boolean
        Property refresh_token As String
        Property scope As String
        Property token_type As String
        Property expires_in As Long
        Property id_token As String
        Property UserInformationJson As String
        Property UserInformationObject As SocialLoginUserInformation
        Property User_Id As String
        Property User_name As String
        Property User_Email As String
        Property User_Photo As Uri
        Property screen_name As String
    End Class
    Public Class SocialLoginUserInformation

    End Class
End Namespace
