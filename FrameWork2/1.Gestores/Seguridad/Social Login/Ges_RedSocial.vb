Imports System
Imports System.Security.Cryptography
Imports System.Collections.Generic
Imports System.Text
Imports System.Runtime.InteropServices
Imports FrameWork.Modulos

Namespace Gestores
    Public Class Ges_RedSocial
        Property RedSocial As IRedSocial
        Dim ServicioExterno As New FrameWork.Gestores.Ges_WebServices
        Public Sub New(RedSocial As IRedSocial)
            Me.RedSocial = RedSocial
        End Sub
        Function GetAccessToken(Tipo As Enum_GetAccessToken) As Ges_WebServices
            ServicioExterno = New Ges_WebServices
            With ServicioExterno
                Select Case Tipo
                    Case Enum_GetAccessToken.ParamsOnBody
                        .ContentType = "application/x-www-form-urlencoded"
                        Dim Body As New System.Text.StringBuilder
                        With Body
                            .Append("client_id=" + RedSocial.SocialLogin.ClientId + "&")
                            .Append("client_secret=" + RedSocial.SocialLogin.ClientSecret + "&")
                            .Append("redirect_uri=" + Web.HttpUtility.UrlEncode(RedSocial.URL_Redirect) + "&")
                            .Append("grant_type=authorization_code&")
                            .Append("state=" + RedSocial.State + "&")
                            .Append("code=" + RedSocial.Params("code"))
                        End With
                        ServicioExterno.Body = Body.ToString
                    Case Enum_GetAccessToken.BasicAuthorization
                        .ContentType = "application/x-www-form-urlencoded"
                        .User = RedSocial.SocialLogin.ClientId
                        .Password = RedSocial.SocialLogin.ClientSecret
                        Dim Body As New System.Text.StringBuilder
                        With Body
                            .Append("client_id=" + RedSocial.SocialLogin.ClientId + "&")
                            .Append("redirect_uri=" + Web.HttpUtility.UrlEncode(RedSocial.URL_Redirect) + "&")
                            .Append("grant_type=authorization_code&")
                            .Append("code=" + RedSocial.Params("code"))
                        End With
                        ServicioExterno.Body = Body.ToString
                    Case Enum_GetAccessToken.ParamsOnQueryString
                        .AddParam("client_id", RedSocial.SocialLogin.ClientId)
                        .AddParam("client_secret", RedSocial.SocialLogin.ClientSecret)
                        .AddParamEncode("redirect_uri", RedSocial.URL_Redirect)
                        .AddParam("grant_type", "authorization_code")
                        .AddParam("state", RedSocial.State)
                        .AddParam("code", RedSocial.Params("code"))
                    Case Enum_GetAccessToken.OAuth1
                        .AddParam("oauth_token", RedSocial.Params("oauth_token"))
                        .AddParam("oauth_verifier", RedSocial.Params("oauth_verifier"))
                End Select
                .Url = RedSocial.SocialLogin.URLGetAccessToken
                .ExecutePost()
            End With
            Return ServicioExterno
        End Function

        Function GetUserInfo(Tipo As Enum_GetUserInfo) As Ges_WebServices
            ServicioExterno = New Ges_WebServices
            Select Case Tipo
                Case Enum_GetUserInfo.OAuth1
                    Dim URL As String = RedSocial.SocialLogin.URLGetInformation + "?screen_name=" + RedSocial.Sesion.screen_name
                    Dim baseuristr As String = URL
                    Dim baseuri As Uri = New Uri(baseuristr)
                    Dim OAuth1 = New Srv_OAuth1()
                    Dim ConsumerKey As String = RedSocial.SocialLogin.ClientId
                    Dim SecretKey As String = RedSocial.SocialLogin.ClientSecret
                    Dim oAuthToken As String = RedSocial.Sesion.oauth_token
                    Dim oAuthTokenSecret As String = RedSocial.Sesion.oauth_token_secret
                    Dim timeStamp = OAuth1.GenerateTimeStamp()
                    Dim nonce = OAuth1.GenerateNonce()
                    Dim QStr = String.Format("?oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method=HMAC-SHA1&oauth_timestamp={2}&oauth_version=1.0", "YFPvHjYuuPuGuk1vo6QROBWJB", nonce, timeStamp)
                    Dim baseUrlwithQStr = baseuristr & QStr
                    Dim oauth_signature = OAuth1.GenerateSignature(baseuri, "", ConsumerKey, SecretKey, oAuthToken, oAuthTokenSecret, "GET", timeStamp, "", nonce, baseuristr, QStr)
                    oauth_signature = OAuth1.UrlEncode(oauth_signature)
                    Dim finalUrl = String.Format("{0}&oauth_signature={1}", baseUrlwithQStr, oauth_signature)
                    Dim Authorization As String = ""
                    Authorization += "oauth_consumer_key=""" + ConsumerKey + ""","
                    Authorization += "oauth_token=""" + oAuthToken + ""","
                    Authorization += "oauth_signature_method=""HMAC-SHA1"","
                    Authorization += "oauth_timestamp=""" + timeStamp + ""","
                    Authorization += "oauth_nonce=""" + nonce + ""","
                    Authorization += "oauth_version=""1.0"","
                    Authorization += "oauth_signature=""" + oauth_signature + """"
                    ServicioExterno.OAuth = Authorization
                    ServicioExterno.Url = URL
                Case Enum_GetUserInfo.Oauth2
                    ServicioExterno.Url = RedSocial.SocialLogin.URLGetInformation
                    ServicioExterno.BearerToken = RedSocial.Sesion.access_token
            End Select
            ServicioExterno.ExecuteGet()
            Return ServicioExterno
        End Function

        Function RequestToken(Tipo As Enum_RequestToken) As Ges_WebServices
            ServicioExterno = New Ges_WebServices
            Select Case Tipo
                Case Enum_RequestToken.Oauth1
                    Dim baseuristr As String = RedSocial.SocialLogin.URLGetRequestToken
                    Dim baseUri As Uri = New Uri(baseuristr)
                    Dim OAuth1 = New Srv_OAuth1()
                    Dim timeStamp = OAuth1.GenerateTimeStamp()
                    Dim nonce = OAuth1.GenerateNonce()
                    Dim QStr = String.Format("?oauth_consumer_key={0}&oauth_nonce={1}&oauth_signature_method=HMAC-SHA1&oauth_timestamp={2}&oauth_version=1.0", RedSocial.SocialLogin.ClientId, nonce, timeStamp)
                    Dim baseUrlwithQStr = baseuristr & QStr
                    Dim oauth_signature = OAuth1.GenerateSignature(baseUri, "", RedSocial.SocialLogin.ClientId, RedSocial.SocialLogin.ClientSecret, "", "", "GET", timeStamp, "", nonce, baseuristr, QStr)
                    Dim finalUrl = String.Format("{0}&oauth_signature={1}", baseUrlwithQStr, OAuth1.UrlEncode(oauth_signature))
                    ServicioExterno.Url = finalUrl
                    ServicioExterno.ExecuteGet()
            End Select
            Return ServicioExterno
        End Function
    End Class


    Public Enum Enum_RequestToken
        Oauth1 = 0
    End Enum


    Public Enum Enum_GetAccessToken
        ParamsOnBody = 0
        ParamsOnQueryString = 1
        OAuth1 = 2
        BasicAuthorization = 3
    End Enum
    Public Enum Enum_GetUserInfo
        OAuth1 = 0
        Oauth2 = 1
    End Enum

    ' ***************************************************************************************************************************
    ' ***************************************************************************************************************************
    ' ***************************************************************************************************************************
    ' ************************************************************* OAuth1 ******************************************************
    ' ************************************************************* OAuth1 ******************************************************
    ' ************************************************************* OAuth1 ******************************************************
    ' ***************************************************************************************************************************
    ' ***************************************************************************************************************************
    ' ***************************************************************************************************************************

    Public Class Srv_OAuth1
        Public Enum SignatureTypes
            HMACSHA1
            PLAINTEXT
            RSASHA1
        End Enum

        Protected Class QueryParameter
            Private _name As String = Nothing
            Private _value As String = Nothing

            Public Sub New(ByVal name As String, ByVal value As String)
                Me._name = name
                Me._value = value
            End Sub

            Public ReadOnly Property Name As String
                Get
                    Return _name
                End Get
            End Property

            Public ReadOnly Property Value As String
                Get
                    Return _value
                End Get
            End Property
        End Class

        Protected Class QueryParameterComparer
            Implements IComparer(Of QueryParameter)
            Public Function Compare(ByVal x As QueryParameter, ByVal y As QueryParameter) As Integer Implements IComparer(Of QueryParameter).Compare
                If x.Name = y.Name Then
                    Return String.Compare(x.Value, y.Value)
                Else
                    Return String.Compare(x.Name, y.Name)
                End If
            End Function
        End Class

        Protected Const OAuthVersion As String = "1.0"
        Protected Const OAuthParameterPrefix As String = "oauth_"
        Protected Const OAuthConsumerKeyKey As String = "oauth_consumer_key"
        Protected Const OAuthCallbackKey As String = "oauth_callback"
        Protected Const OAuthVersionKey As String = "oauth_version"
        Protected Const OAuthSignatureMethodKey As String = "oauth_signature_method"
        Protected Const OAuthSignatureKey As String = "oauth_signature"
        Protected Const OAuthTimestampKey As String = "oauth_timestamp"
        Protected Const OAuthNonceKey As String = "oauth_nonce"
        Protected Const OAuthTokenKey As String = "oauth_token"
        Protected Const OAuthTokenSecretKey As String = "oauth_token_secret"
        Protected Const OAuthVerifier As String = "oauth_verifier"
        Protected Const HMACSHA1SignatureType As String = "HMAC-SHA1"
        Protected Const PlainTextSignatureType As String = "PLAINTEXT"
        Protected Const RSASHA1SignatureType As String = "RSA-SHA1"
        Protected Shared unreservedChars As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~"

        Private Function ComputeHash(ByVal hashAlgorithm As HashAlgorithm, ByVal data As String) As String
            If hashAlgorithm Is Nothing Then
                Throw New ArgumentNullException("hashAlgorithm")
            End If

            If String.IsNullOrEmpty(data) Then
                Throw New ArgumentNullException("data")
            End If

            Dim dataBuffer As Byte() = System.Text.Encoding.ASCII.GetBytes(data)
            Dim hashBytes As Byte() = hashAlgorithm.ComputeHash(dataBuffer)
            Return Convert.ToBase64String(hashBytes)
        End Function

        Private Function GetQueryParameters(ByVal parameters As String) As List(Of QueryParameter)
            If parameters.StartsWith("?") Then
                parameters = parameters.Remove(0, 1)
            End If
            Dim result As List(Of QueryParameter) = New List(Of QueryParameter)()
            If Not String.IsNullOrEmpty(parameters) Then
                Dim p As String() = parameters.Split("&"c)
                For Each s As String In p
                    If Not String.IsNullOrEmpty(s) AndAlso Not s.StartsWith(OAuthParameterPrefix) Then
                        If s.IndexOf("="c) > -1 Then
                            Dim temp As String() = s.Split("="c)
                            result.Add(New QueryParameter(temp(0), temp(1)))
                        Else
                            result.Add(New QueryParameter(s, String.Empty))
                        End If
                    End If
                Next
            End If
            Return result
        End Function

        Public Function UrlEncode(ByVal value As String) As String
            Dim result As StringBuilder = New StringBuilder()
            For Each symbol As Char In value
                If unreservedChars.IndexOf(symbol) <> -1 Then
                    result.Append(symbol)
                Else
                    result.Append("%"c & String.Format("{0:X2}", Convert.ToByte(symbol)))
                End If
            Next

            Return result.ToString()
        End Function

        Protected Function NormalizeRequestParameters(ByVal parameters As IList(Of QueryParameter)) As String
            Dim sb As StringBuilder = New StringBuilder()
            Dim p As QueryParameter = Nothing
            For i As Integer = 0 To parameters.Count - 1
                p = parameters(i)
                sb.AppendFormat("{0}={1}", p.Name, p.Value)

                If i < parameters.Count - 1 Then
                    sb.Append("&")
                End If
            Next
            Return sb.ToString()
        End Function

        Public Function GenerateSignatureBase(ByVal url As Uri, ByVal callback As String, ByVal consumerKey As String, ByVal token As String, ByVal tokenSecret As String, ByVal httpMethod As String, ByVal timeStamp As String, ByVal verifier As String, ByVal nonce As String, ByVal signatureType As String, <Out> ByRef normalizedUrl As String, <Out> ByRef normalizedRequestParameters As String) As String
            If callback Is Nothing Then
                callback = String.Empty
            End If
            If token Is Nothing Then
                token = String.Empty
            End If
            If tokenSecret Is Nothing Then
                tokenSecret = String.Empty
            End If
            If verifier Is Nothing Then
                verifier = String.Empty
            End If
            If String.IsNullOrEmpty(consumerKey) Then
                Throw New ArgumentNullException("consumerKey")
            End If
            If String.IsNullOrEmpty(httpMethod) Then
                Throw New ArgumentNullException("httpMethod")
            End If
            If String.IsNullOrEmpty(signatureType) Then
                Throw New ArgumentNullException("signatureType")
            End If
            normalizedUrl = Nothing
            normalizedRequestParameters = Nothing
            Dim parameters As List(Of QueryParameter) = GetQueryParameters(url.Query)
            parameters.Add(New QueryParameter(OAuthVersionKey, OAuthVersion))
            parameters.Add(New QueryParameter(OAuthNonceKey, nonce))
            parameters.Add(New QueryParameter(OAuthTimestampKey, timeStamp))
            parameters.Add(New QueryParameter(OAuthSignatureMethodKey, signatureType))
            parameters.Add(New QueryParameter(OAuthConsumerKeyKey, consumerKey))
            If Not String.IsNullOrEmpty(token) Then
                parameters.Add(New QueryParameter(OAuthTokenKey, token))
            End If
            If Not String.IsNullOrEmpty(callback) Then
                parameters.Add(New QueryParameter(OAuthCallbackKey, UrlEncode(callback)))
            End If
            If Not String.IsNullOrEmpty(verifier) Then
                parameters.Add(New QueryParameter(OAuthVerifier, verifier))
            End If
            parameters.Sort(New QueryParameterComparer())
            normalizedUrl = String.Format("{0}://{1}", url.Scheme, url.Host)
            If Not ((url.Scheme = "http" AndAlso url.Port = 80) OrElse (url.Scheme = "https" AndAlso url.Port = 443)) Then
                normalizedUrl += ":" & url.Port
            End If
            normalizedUrl += url.AbsolutePath
            normalizedRequestParameters = NormalizeRequestParameters(parameters)
            Dim signatureBase As StringBuilder = New StringBuilder()
            signatureBase.AppendFormat("{0}&", httpMethod.ToUpper())
            signatureBase.AppendFormat("{0}&", UrlEncode(normalizedUrl))
            signatureBase.AppendFormat("{0}", UrlEncode(normalizedRequestParameters))
            Return signatureBase.ToString()
        End Function

        Public Function GenerateSignatureUsingHash(ByVal signatureBase As String, ByVal hash As HashAlgorithm) As String
            Return ComputeHash(hash, signatureBase)
        End Function

        Public Function GenerateSignature(ByVal url As Uri, ByVal callback As String, ByVal consumerKey As String, ByVal consumerSecret As String, ByVal token As String, ByVal tokenSecret As String, ByVal httpMethod As String, ByVal timeStamp As String, ByVal verifier As String, ByVal nonce As String, <Out> ByRef normalizedUrl As String, <Out> ByRef normalizedRequestParameters As String) As String
            Return GenerateSignature(url, callback, consumerKey, consumerSecret, token, tokenSecret, httpMethod, timeStamp, verifier, nonce, SignatureTypes.HMACSHA1, normalizedUrl, normalizedRequestParameters)
        End Function

        Public Function GenerateSignature(ByVal url As Uri, ByVal callback As String, ByVal consumerKey As String, ByVal consumerSecret As String, ByVal token As String, ByVal tokenSecret As String, ByVal httpMethod As String, ByVal timeStamp As String, ByVal verifier As String, ByVal nonce As String, ByVal signatureType As SignatureTypes, <Out> ByRef normalizedUrl As String, <Out> ByRef normalizedRequestParameters As String) As String
            normalizedUrl = Nothing
            normalizedRequestParameters = Nothing
            Select Case signatureType
                Case SignatureTypes.PLAINTEXT
                    Return UrlEncode(String.Format("{0}&{1}", consumerSecret, tokenSecret))
                Case SignatureTypes.HMACSHA1
                    Dim signatureBase As String = GenerateSignatureBase(url, callback, consumerKey, token, tokenSecret, httpMethod, timeStamp, verifier, nonce, HMACSHA1SignatureType, normalizedUrl, normalizedRequestParameters)
                    Dim hmacsha1 As HMACSHA1 = New HMACSHA1()
                    hmacsha1.Key = Encoding.ASCII.GetBytes(String.Format("{0}&{1}", UrlEncode(consumerSecret), If(String.IsNullOrEmpty(tokenSecret), "", UrlEncode(tokenSecret))))
                    Return GenerateSignatureUsingHash(signatureBase, hmacsha1)
                Case SignatureTypes.RSASHA1
                    Throw New NotImplementedException()
                Case Else
                    Throw New ArgumentException("Unknown signature type", "signatureType")
            End Select
        End Function

        Public Overridable Function GenerateTimeStamp() As String
            Dim ts As TimeSpan = DateTime.UtcNow - New DateTime(1970, 1, 1, 0, 0, 0, 0)
            Return Convert.ToInt64(ts.TotalSeconds).ToString()
        End Function

        Public Overridable Function GenerateNonce() As String
            Return Guid.NewGuid().ToString().Replace("-", "")
        End Function
    End Class

End Namespace

