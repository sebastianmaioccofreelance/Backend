Imports System.Linq
Imports System.Text
Imports System
Imports System.Collections.Generic
Imports System.IO


Public Class ClsFiles
    Private _base64 As String
    Private _bytes As Byte()
    Private _content As String
    Private _fullpath As String
    Private _fileWriter As StreamWriter
    Private _fileStream As FileStream
    Private _fileAccess As Enum_FileAccess
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
    Property FileName As String
    Public ReadOnly Property FullPath
        Get
            Return _fullpath
        End Get
    End Property

    Public Sub New(FullPath As String, TipoAcceso As Enum_FileAccess)
        _fileAccess = TipoAcceso
        Select Case TipoAcceso
            Case Enum_FileAccess.Write
                If File.Exists(FullPath) Then
                    File.Delete(FullPath)
                End If
                _fileStream = New FileStream(FullPath, FileMode.Create)
                _fileWriter = New StreamWriter(_fileStream)
            Case Enum_FileAccess.Append
                _fileStream = New FileStream(FullPath, FileMode.Append)
                _fileWriter = New StreamWriter(_fileStream)
            Case Enum_FileAccess.Read
                _bytes = File.ReadAllBytes(FullPath)
                _content = File.ReadAllText(FullPath)
                _base64 = Convert.ToBase64String(_bytes)
                FileName = FullPath.Split("\").Last
                _fullpath = FullPath
        End Select
    End Sub

    Public Sub WriteLine(Text As String)
        _fileWriter.WriteLine(Text)
    End Sub
    Public Sub Write(Text As String)
        _fileWriter.Write(Text)
    End Sub

    Public Sub Close()
        _fileWriter.Close()
    End Sub
    Public Sub New(FileName As String, Base64 As String)
        _base64 = Base64
        _bytes = Convert.FromBase64CharArray(Base64.ToCharArray(), 0, Base64.Length)
        FileName = FileName
    End Sub
    Public Sub New(FileName As String, Bytes As Byte())
        _bytes = Bytes
        _base64 = Convert.ToBase64String(Bytes)
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
End Enum
