Imports System.Collections.Generic
Imports System.Data.Entity
Imports System.Linq
Imports System.Web.Http
Imports System.Web.Http.Controllers
Imports System.Net.Http
Imports System.Text
Imports FrameWork
Imports FrameWork.DAO
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Namespace DAO
    <Table("Idiomas")>
    Public Class DAO_Idioma
        <Required>
        Property Id As Int64
        <MaxLength(100)>
        Property Grupo As String
        <MaxLength(100)>
        Property Clave As String
        Property Valor As String
        <MaxLength(50)>
        Property Idioma As String
    End Class
End Namespace