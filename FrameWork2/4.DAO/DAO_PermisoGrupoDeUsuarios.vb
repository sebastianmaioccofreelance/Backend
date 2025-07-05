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
    <Table("PermisosGruposDeUsuarios")>
    Public Class DAO_PermisoGrupoDeUsuarios
        <Required>
        Property Id As Int64
        Property IdPermiso As Int64
        Property IdGrupoDeUsuarios As Int64
    End Class


End Namespace

