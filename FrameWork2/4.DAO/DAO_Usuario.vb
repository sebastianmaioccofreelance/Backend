Imports System.Linq
Imports System.Collections.Generic
Imports System.Web.Http
Imports System.Web.Http.Controllers
Imports System.Net.Http
Imports System.Text
Imports FrameWork.Extensiones
Imports FrameWork.Modulos
Imports FrameWork
Imports FrameWork.DAO
Imports FrameWork.Gestores.Extensiones
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Namespace DAO
    <Table("Usuarios")>
    Public Class DAO_Usuario
        Inherits Bases.FW_BaseDAO
        Property ProveedorSocialLoginId As Int64
        Property SocialLoginUserId As String
        Property Foto As String
        Property Username As String
        Property Password As String
        Property NroDocumento As String
        Property NombreCompleto As String
        Property Nacionalidad As String
        Property FechaNacimiento As DateTime?
        <EmailAddress>
        Property Mail As String
        Property Pais As String
        Property ProvinciaEstado As String
        Property Ciudad As String
        Property Direccion As String
        Property CodigoPostal As String
        Property Piso As String
        Property Departamento As String
        Property Telefono As String
        Property NroCelular As String
        Property Enabled As Boolean
        Property Locked As Boolean
        Property PushNotificationPlayerID As String
    End Class
End Namespace

