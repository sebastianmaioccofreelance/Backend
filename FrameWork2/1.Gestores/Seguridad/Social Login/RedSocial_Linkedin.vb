Imports FrameWork.Bases
Imports FrameWork.DAO
Imports System.Linq
Imports System.Collections.Generic
Imports Newtonsoft.Json
Imports FrameWork.Modulos
Imports FrameWork.Gestores
Namespace Modulos
    Public Class RedSocial_Linkedin
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
            Dim Info As infoUserLinkedin
            Info = Sesion.UserInformationJson.JsonToObject(Of infoUserLinkedin)()
            Sesion.UserInformationObject = Info
            With Sesion
                .ProviderName = "Linkedin"
                .User_Photo = New Uri(Info.profilePicture.DisplayImage.elements(Info.profilePicture.DisplayImage.elements.Count - 1).identifiers.First.identifier)
                .User_Id = Info.id
                .User_name = Info.localizedLastName + ", " + Info.localizedFirstName
                .screen_name = .User_name
            End With
        End Sub

        Public Sub RequestToken() Implements IRedSocial.RequestToken
            Throw New NotImplementedException()
        End Sub

        Private Class Localized
            Public Property es_ES As String
        End Class

        Private Class PreferredLocale
            Public Property country As String
            Public Property language As String
        End Class

        Private Class FirstName
            Public Property localized As Localized
            Public Property preferredLocale As PreferredLocale
        End Class

        Private Class LastName
            Public Property localized As Localized
            Public Property preferredLocale As PreferredLocale
        End Class

        Private Class Paging
            Public Property count As Integer
            Public Property start As Integer
            Public Property links As List(Of Object)
        End Class

        Private Class RawCodecSpec
            Public Property name As String
            Public Property type As String
        End Class

        Private Class DisplaySize
            Public Property width As Double
            Public Property uom As String
            Public Property height As Double
        End Class

        Private Class StorageSize
            Public Property width As Integer
            Public Property height As Integer
        End Class

        Private Class StorageAspectRatio
            Public Property widthAspect As Double
            Public Property heightAspect As Double
            Public Property formatted As String
        End Class

        Private Class DisplayAspectRatio
            Public Property widthAspect As Double
            Public Property heightAspect As Double
            Public Property formatted As String
        End Class

        Private Class ComLinkedinDigitalmediaMediaartifactStillImage
            Public Property mediaType As String
            Public Property rawCodecSpec As RawCodecSpec
            Public Property displaySize As DisplaySize
            Public Property storageSize As StorageSize
            Public Property storageAspectRatio As StorageAspectRatio
            Public Property displayAspectRatio As DisplayAspectRatio
        End Class

        Private Class Data
            <JsonProperty("com.linkedin.digitalmedia.mediaartifact.StillImage")>
            Public Property ComLinkedinDigitalmediaMediaartifactStillImage As ComLinkedinDigitalmediaMediaartifactStillImage
        End Class

        Private Class Identifier
            Public Property identifier As String
            Public Property index As Integer
            Public Property mediaType As String
            Public Property file As String
            Public Property identifierType As String
            Public Property identifierExpiresInSeconds As Integer
        End Class

        Private Class Element
            Public Property artifact As String
            Public Property authorizationMethod As String
            Public Property data As Data
            Public Property identifiers As List(Of Identifier)
        End Class

        Private Class DisplayImage
            Public Property paging As Paging
            Public Property elements As List(Of Element)
        End Class

        Private Class ProfilePicture
            <JsonProperty("displayImage~")>
            Public Property DisplayImage As DisplayImage
        End Class

        Private Class infoUserLinkedin
            Inherits SocialLoginUserInformation
            Public Property localizedLastName As String
            Public Property firstName As FirstName
            Public Property lastName As LastName
            Public Property profilePicture As ProfilePicture
            Public Property id As String
            Public Property localizedFirstName As String
        End Class
    End Class
End Namespace