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
    <Table("Roles")>
    Public Class DAO_Rol
        Inherits Bases.FW_BaseDAO
        Property NombreRol As String
        Property Descripcion As String
        Property Habilitado As Boolean
    End Class
End Namespace