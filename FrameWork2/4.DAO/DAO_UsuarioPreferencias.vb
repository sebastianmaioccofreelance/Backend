
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
    <Table("UsuarioPreferencias")>
    Public Class DAO_UsuarioPreferencias
        Inherits Bases.FW_BaseDAO
        Property UserId As Int64
        Property Categoria As String
        Property Key As String
        Property Value As String
    End Class
End Namespace