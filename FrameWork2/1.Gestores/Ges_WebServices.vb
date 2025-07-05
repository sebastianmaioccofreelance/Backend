Imports FrameWork.Modulos
Imports Newtonsoft.Json
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Net
Imports System.Text
Imports System.Xml.Linq

Namespace Gestores
    Public Class Ges_WebServices
        Property BaseUrl As String
        Property Endpoint As String
        Property BearerToken As String
        Property OAuth As String
        Property User As String = ""
        Property Password As String = ""
        Property Method As String = "GET"
        Property ContentType As String = "application/json"
        Property Accept As String = "application/json"
        Property ResponseContentString As String
        Property ResponseContentBytes As Byte()
        Property ResponseWeb As WebResponse
        Property Body As String = ""
        Property Headers As New Dictionary(Of String, String)
        Property Params As New Dictionary(Of String, String)
        Property Browser As HttpWebRequest

        ReadOnly Property URLFullPath As String
            Get
                Return (Url + QueryString).TrimEnd("?")
            End Get
        End Property

        Property QueryString As String
            Get
                Dim ret As String = "?"
                For Each KeyValue As KeyValuePair(Of String, String) In Params
                    ret += KeyValue.Key + "=" + KeyValue.Value + "&"
                Next
                ret = ret.TrimEnd("&").TrimEnd("?")
                Return ret
            End Get
            Set(value As String)
                Dim SplitParams As String()
                Params.Clear()
                value = value.TrimStart("?")
                SplitParams = value.Split("&")
                For Each Param As String In SplitParams
                    Dim SplitKeyValue As String()
                    SplitKeyValue = Param.Split("=")
                    Params.Add(SplitKeyValue.First, SplitKeyValue.Last)
                Next
            End Set
        End Property

        Public ReadOnly Property ResponseValues As Dictionary(Of String, String)
            Get
                Return ResponseContentString.JsonToObject(Of Dictionary(Of String, String))
            End Get
        End Property

        Public Sub Reset()
            BearerToken = ""
            BasicAuthorization = ""
            User = ""
            Password = ""
            BaseUrl = ""
            NextRequest()
        End Sub

        Public Sub NextRequest(Optional Method As String = "GET")
            Me.Method = Method
            Body = ""
            Headers.Clear()
            Params.Clear()
            ResponseContentString = ""
            Endpoint = ""
            ContentType = "application/json"
        End Sub

        Public Sub AddParam(Key As String, Value As String)
            Params.Add(Key, Value)
        End Sub
        Public Sub AddParamEncode(Key As String, value As String)
            Params.Add(Key, Web.HttpUtility.UrlEncode(value))
        End Sub
        Public Sub AddHeader(Key As String, Value As String)
            Headers.Add(Key, Value)
        End Sub

        Public Function ResponseToObject(Of t)() As t
            Return ResponseContentString.JsonToObject(Of t)
        End Function
        Public Sub SetBasicAuthorizationInHeader(Key As String)
            Headers.Add("Authorization", "Basic " + Key)
        End Sub
        Property BasicAuthorization As String
            Get
                If User <> "" And Password <> "" Then
                    Return "Basic " + (User + ":" + Password).ToBase64
                End If
                Return ""
            End Get
            Set(value As String)
                Dim UserPassword As String
                UserPassword = value.Base64ToString
                User = UserPassword.Split(":").First
                Password = UserPassword.Split(":").Last
            End Set
        End Property

        Property Url As String
            Get
                Return BaseUrl.TrimEnd("/") + "/" + Endpoint.TrimStart("/")
            End Get
            Set(value As String)
                BaseUrl = Join(value.Split("/").Where(Function(n, index) index < 3).ToArray, "/")
                Endpoint = Join(value.Split("/").Where(Function(n, index) index >= 3).ToArray, "/")
                If Endpoint.Split("?").Count > 1 Then
                    QueryString = Endpoint.Split("?").Last
                    Endpoint = Endpoint.Split("?").First
                End If
            End Set
        End Property


        Sub GetBearerToken(FieldName As String)
            Method = "POST"
            RequestURL(FieldName)
        End Sub

        Public Sub SetBody(ObjectBody As Object, Optional IgnoreNull As Boolean = False)
            Body = Utilities.ToJson(ObjectBody, IgnoreNull)
        End Sub

        Function ExecuteGet(PathURL As String, Optional IsFullURL As Boolean = False) As String
            If IsFullURL Then
                Url = PathURL
            Else
                Endpoint = PathURL
            End If
            Return ExecuteGet()
        End Function

        Function ExecuteGet() As String
            Method = "GET"
            Return RequestURL()
        End Function

        Function ExecutePost() As String
            Method = "POST"
            Return RequestURL()
        End Function

        Function ExecutePost(Objeto As Object) As String
            Body = JsonConvert.SerializeObject(Objeto)
            Return RequestURL()
        End Function

        Function ExecutePostJson(EndPoint As String, Json As String, Optional IsFullURL As Boolean = False) As String
            If IsFullURL Then
                Url = EndPoint
            Else
                EndPoint = EndPoint
            End If
            Body = Json
            Return ExecutePost()
        End Function

        Function ExecutePost(EndPoint As String, Objeto As Object, Optional IsFullURL As Boolean = False) As String
            If IsFullURL Then
                Url = EndPoint
            Else
                Me.Endpoint = EndPoint
            End If
            Body = Utilities.ToJson(Objeto)
            Return ExecutePost()
        End Function

        Function ExecuteMethod(Method As String) As String
            Me.Method = Method
            Return RequestURL()
        End Function

        Private Function RequestURL(Optional FieldBearerToken As String = "") As String
            Browser = CType(WebRequest.Create(New Uri(URLFullPath)), HttpWebRequest)
            Browser.KeepAlive = True
            Me.ResponseContentString = ""
            For Each Atributo As KeyValuePair(Of String, String) In Headers
                Browser.Headers.Add(Atributo.Key, Atributo.Value)
            Next
            If Not String.IsNullOrEmpty(BasicAuthorization) Then
                Browser.Headers.Add("Authorization", BasicAuthorization)
            End If
            If Not String.IsNullOrEmpty(BearerToken) Then
                Browser.Headers.Add("Authorization", "Bearer " + BearerToken)
            End If
            If Not String.IsNullOrEmpty(OAuth) Then
                Browser.Headers.Add("Authorization", "OAuth " + OAuth)
            End If
            Browser.Accept = Accept
            Browser.ContentType = ContentType
            Browser.Method = Method
            ' Browser.Timeout = 10000
            If Method = "POST" Then
                Dim encoding As ASCIIEncoding = New ASCIIEncoding()
                Dim bytes As Byte() = encoding.GetBytes(Me.Body)
                Dim newStream As Stream = Browser.GetRequestStream()
                newStream.Write(bytes, 0, bytes.Length)
                newStream.Close()
            End If
            Me.ResponseWeb = Browser.GetResponse()
            Dim stream As Stream = ResponseWeb.GetResponseStream()
            Dim sr As StreamReader = New StreamReader(stream)
            Dim ms As New MemoryStream()
            stream.CopyTo(ms)
            Me.ResponseContentBytes = ms.ToArray
            Me.ResponseContentString = Encoding.UTF8.GetString(ResponseContentBytes)
            Return Me.ResponseContentString
        End Function
    End Class
End Namespace

