Imports System.Linq
Imports System.Text
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Net.Http
Imports System.Net
Imports System.Security.Cryptography
Imports FrameWork.Modulos.Mod_ApplicationConfiguration
Imports FrameWork.Bases

Namespace Gestores
    Public Class Ges_Files
        Inherits FW_BaseGestor
        Public Sub New(Transaction As FW_BaseTransaction)
            MyBase.New(Transaction)
        End Sub

        Public Sub New(ContextDB As FW_BaseDBContext)
            MyBase.New(ContextDB)
        End Sub
        Private _base64 As String
        Private _bytes As Byte()
        Private _content As String
        Private _fullpath As String
        Private _fileWriter As StreamWriter
        Private _fileStream As FileStream
        Private _fileAccess As Enum_FileAccess
        Private _responseDownloadFile As HttpResponseMessage
        Public ReadOnly Property Base64 As String
            Get
                Return _base64
            End Get
        End Property
        Public ReadOnly Property Bytes As Byte()
            Get
                Return _bytes
            End Get
        End Property
        Public ReadOnly Property Content As String
            Get
                Return _content
            End Get
        End Property
        Public ReadOnly Property FileName As String
            Get
                Return FullPath.Split("\").Last
            End Get
        End Property
        Public ReadOnly Property FullPath As String
            Get
                Return _fullpath
            End Get
        End Property
        Public ReadOnly Property ResponseDownloadFile As HttpResponseMessage
            Get
                Return _responseDownloadFile
            End Get
        End Property

        Public Sub New(FullPath As String, TipoAcceso As Enum_FileAccess, Optional RequestHttp As HttpRequestMessage = Nothing)
            _fileAccess = TipoAcceso
            _fullpath = FullPath
            Select Case TipoAcceso
                Case Enum_FileAccess.Write
                    _fileStream = New FileStream(FullPath, FileMode.CreateNew)
                    _fileWriter = New StreamWriter(_fileStream)
                Case Enum_FileAccess.Append
                    _fileStream = New FileStream(FullPath, FileMode.Append)
                    _fileWriter = New StreamWriter(_fileStream)
                Case Enum_FileAccess.Read
                    _bytes = File.ReadAllBytes(FullPath)
                    _content = File.ReadAllText(FullPath)
                    _base64 = Convert.ToBase64String(_bytes)
                    _fullpath = FullPath
                Case Enum_FileAccess.ResponseDownloadFile
                    _bytes = File.ReadAllBytes(FullPath)
                    _base64 = Convert.ToBase64String(_bytes)
                    _responseDownloadFile = RequestHttp.CreateResponse(HttpStatusCode.OK)
                    _responseDownloadFile.Content = New ByteArrayContent(_bytes)
                    _responseDownloadFile.Content.Headers.ContentDisposition = New Headers.ContentDispositionHeaderValue("attachment")
                    _responseDownloadFile.Content.Headers.ContentDisposition.FileName = FileName
            End Select
        End Sub

        Public Sub Encriptar(Optional Llave As String = "")
            If String.IsNullOrEmpty(Llave) Then
                Llave = AppConfig("HashEncriptacion")
            End If
            Dim Keys As Byte()
            Dim md5 As New MD5CryptoServiceProvider
            Keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(Llave))
            Dim tripDes As New TripleDESCryptoServiceProvider With {.Key = Keys, .Mode = CipherMode.ECB, .Padding = PaddingMode.PKCS7}
            Dim Transform As ICryptoTransform = tripDes.CreateEncryptor
            _bytes = Transform.TransformFinalBlock(_bytes, 0, _bytes.Length)
        End Sub

        Public Function Desencriptar(Optional Llave As String = "") As String
            If String.IsNullOrEmpty(Llave) Then
                Llave = AppConfig("HashEncriptacion")
            End If
            Dim Keys As Byte()
            Dim md5 As New MD5CryptoServiceProvider
            Keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(Llave))
            Dim tripDes As New TripleDESCryptoServiceProvider With {.Key = Keys, .Mode = CipherMode.ECB, .Padding = PaddingMode.PKCS7}
            Dim Transform As ICryptoTransform = tripDes.CreateDecryptor
            _bytes = Transform.TransformFinalBlock(_bytes, 0, _bytes.Length)
        End Function

        Public Sub WriteLine(Text As String)
            _fileWriter.WriteLine(Text)
        End Sub
        Public Sub Write(Text As String)
            _fileWriter.Write(Text)
        End Sub
        Public Sub Save(Optional Encripted As Boolean = False)
            If Encripted Then
                Encriptar()
            End If
            File.WriteAllBytes(FullPath, _bytes)
        End Sub
        Public Sub SaveAs(NewPath As String, Optional Encripted As Boolean = False)
            If Encripted Then
                Encriptar()
            End If
            _fullpath = NewPath
            File.WriteAllBytes(NewPath, _bytes)
        End Sub

        Public Sub Close()
            _fileWriter.Close()
        End Sub

        Public Function CreateResponseDownloadFile(RequestHttp As HttpRequestMessage) As HttpResponseMessage
            Dim Result As HttpResponseMessage
            Result = RequestHttp.CreateResponse(HttpStatusCode.OK)
            Result.Content = New ByteArrayContent(_bytes)
            Result.Content.Headers.ContentDisposition = New Headers.ContentDispositionHeaderValue("attachment")
            Result.Content.Headers.ContentDisposition.FileName = FileName
            Return Result
        End Function

        Public Sub New(FilePath As String, Base64 As String)
            _base64 = Base64
            _bytes = Convert.FromBase64CharArray(Base64.ToCharArray(), 0, Base64.Length)
            _fullpath = FilePath
        End Sub
        Public Sub New(FilePath As String, Bytes As Byte())
            _bytes = Bytes
            _base64 = Convert.ToBase64String(Bytes)
            _fullpath = FilePath
        End Sub
        Public Sub New(Bytes As Byte())
            _bytes = Bytes
            _base64 = Convert.ToBase64String(Bytes)
        End Sub
    End Class
    Public Enum Enum_FileAccess
        Append
        Write
        Read
        ResponseDownloadFile
    End Enum
End Namespace
