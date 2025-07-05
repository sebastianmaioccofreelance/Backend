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
    <Table("RolesPermisos")>
    Public Class DAO_RolPermiso
        Inherits Bases.FW_BaseDAO
        Public Sub New()

        End Sub
        Public Sub New(IdRol As Int64, IdPermiso As Int64)
            Me.IdRol = IdRol
            Me.IdPermiso = IdPermiso
        End Sub
        Property IdPermiso As Int64
        Property IdRol As Int64
    End Class
End Namespace

