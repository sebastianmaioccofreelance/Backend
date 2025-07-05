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
    <Table("SesionesUsuario")>
    Public Class DAO_SesionUsuario
        Inherits Bases.FW_BaseDAO
        Property IdUsuario As Int64
        Property Token As String
        Property TokenRefresh As String
        Property ExpirationToken As DateTime
        Property FechaInicio As DateTime
        '<NotMapped>
        'Property ProveedorSocialLogin As 
    End Class
End Namespace

