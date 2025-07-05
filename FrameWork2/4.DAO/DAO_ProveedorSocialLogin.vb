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
    <Table("ProveedoresSocialLogin")>
    Public Class DAO_ProveedorSocialLogin
        Inherits Bases.FW_BaseDAO
        Property ProviderName As String
        Property URLGetAuthorizationCode As String
        Property URLGetAccessToken As String
        Property URLGetUserInformation As String
        Property URLLogout As String
        Property BaseURL As String
        Property ClientId As String
        Property ClientSecret As String
        Property Token As String
        Property ParamsRequest As String
    End Class
End Namespace
