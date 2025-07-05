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
    <Table("UserCache")>
    Public Class DAO_UsuarioCache
        Inherits Bases.FW_BaseDAO
        Property UserID As Int64
        Property Group As String
        Property Name As String
        Property Value As String
        Property ExpirationDateTime As DateTime
    End Class
End Namespace
