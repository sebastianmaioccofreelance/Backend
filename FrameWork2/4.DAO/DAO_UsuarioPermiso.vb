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
    <Table("UsuariosPermisos")>
    Public Class DAO_UsuarioPermiso
        Inherits Bases.FW_BaseDAO
        Public Sub New()

        End Sub
        Public Sub New(IdUsuario As Int64, IdPermiso As Int64)
            Me.IdUsuario = IdUsuario
            Me.IdPermiso = IdPermiso
        End Sub
        Property IdUsuario As Int64
        Property IdPermiso As Int64
    End Class
End Namespace


