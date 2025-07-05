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
    <Table("UsuariosRoles")>
    Public Class DAO_UsuarioRol
        Inherits Bases.FW_BaseDAO
        Public Sub New()

        End Sub
        Public Sub New(IdRol, IdUsuario)
            Me.IdRol = IdRol
            Me.IdUsuario = IdUsuario
        End Sub
        Property IdRol As Int64
        Property IdUsuario As Int64
    End Class
End Namespace

