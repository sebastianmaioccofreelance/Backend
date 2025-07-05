Imports FrameWork.Bases
Imports FrameWork.DAO
Imports System.Linq
Imports System.Collections.Generic
Imports System.Data.Entity.Migrations
Imports FrameWork.FW_ResponseService

Namespace Gestores
    Public Class Ges_UsuarioPreferencias
        Inherits FW_BaseGestor
        Public Sub New(Transaction As FW_BaseTransaction)
            MyBase.New(Transaction)
        End Sub

        Public Sub New(Context As FW_BaseDBContext)
            MyBase.New(Context)
        End Sub

        Public Function ListarTodos() As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ItemsUsuarioPreferencias As List(Of DAO_UsuarioPreferencias)
            ItemsUsuarioPreferencias = ContextDB.UsuarioPreferencias.ToList
            ResponseService.SetData(ItemsUsuarioPreferencias)
            ResponseService.SetResultsByCountRecords(ItemsUsuarioPreferencias.Count)
            Return ResponseService
        End Function

        Public Function ListarCategoria(Categoria As String) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim Categorias As List(Of DAO_UsuarioPreferencias)
            Categorias = (From UsuarioPreferencias
                              In ContextDB.UsuarioPreferencias
                          Where UsuarioPreferencias.Categoria = Categoria
                          Select UsuarioPreferencias).ToList

            ResponseService.SetData(Categorias)
            ResponseService.SetResultsByCountRecords(Categorias.Count)
            Return ResponseService
        End Function

        Public Function Obtener(UserId As Int64, Key As String) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ItemUsuarioPreferencias As DAO_UsuarioPreferencias
            ItemUsuarioPreferencias = (From ItemsConfiguration
                              In ContextDB.UsuarioPreferencias
                                       Where ItemsConfiguration.Key = Key And ItemsConfiguration.UserId = UserId
                                       Select ItemsConfiguration).FirstOrDefault

            ResponseService.SetData(ItemUsuarioPreferencias)
            Return ResponseService
        End Function
        Public Function Guardar(ItemUsuarioPreferencias As DAO_UsuarioPreferencias) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ContextDB.UsuarioPreferencias.AddOrUpdate((ItemUsuarioPreferencias))
            Save()
            ResponseService.SetData(ItemUsuarioPreferencias)
            Return ResponseService
        End Function

        Public Function Eliminar(UserId As Int64, Key As String) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ServicioUsuarioPreferencias As New Ges_UsuarioPreferencias(Me.Transaction)
            Dim ItemUsuarioPreferencias As DAO_UsuarioPreferencias
            ItemUsuarioPreferencias = ServicioUsuarioPreferencias.Obtener(UserId, Key).
                          Unsuccessfully(Enum_ResultsServices.Void, "No se encontro el item de preferencias de usuario").
                          Data
            ContextDB.UsuarioPreferencias.Remove(ItemUsuarioPreferencias)
            Save()
            Return ResponseService
        End Function
    End Class
End Namespace