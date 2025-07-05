Imports System.Linq
Imports System.Collections.Generic
Imports System.Web.Http
Imports System.Web.Http.Controllers
Imports System.Net.Http
Imports System.Text
Imports FrameWork.Extensiones
Imports FrameWork
Imports FrameWork.DAO
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Namespace DAO
    <Table("SocialLogin")>
    Public Class DAO_SocialLogin
        Inherits Bases.FW_BaseDAO
        Property ProviderName As String
        Property ClientId As String
        Property ClientSecret As String
        Property TenantId As String
        Property URLGetRequestToken As String
        Property URLGetAuthorizationCode As String
        Property URLGetAccessToken As String
        Property URLGetInformation As String
        Property VersionOAuth As Int32
    End Class
End Namespace

