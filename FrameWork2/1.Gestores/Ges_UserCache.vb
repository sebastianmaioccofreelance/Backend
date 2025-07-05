Imports FrameWork.Bases
Imports FrameWork.DAO
Imports System.Linq
Imports System.Collections.Generic
Imports System.Data.Entity.Migrations
Imports FrameWork.FW_ResponseService

Namespace Gestores
    Public Class Ges_UserCache
        Inherits FW_BaseGestor

        Public Sub New(Transaction As FW_BaseTransaction)
            MyBase.New(Transaction)
        End Sub

        Public Sub New(Context As FW_BaseDBContext)
            MyBase.New(Context)
        End Sub

        Public Function ObtenerPorId(Id As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ItemUsuarioCache As DAO_UsuarioCache
            ItemUsuarioCache = (From ItemsCache
                              In ContextDB.UsersCache
                                Where ItemsCache.Id = Id
                                Select ItemsCache).FirstOrDefault
            ResponseService.SetData(ItemUsuarioCache)
            Return ResponseService
        End Function

        Public Function ObtenerPorClave(UserId As Int64, Group As String, Key As String) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ItemUsuarioCache As DAO_UsuarioCache
            ItemUsuarioCache = (From ItemsCache
                              In ContextDB.UsersCache
                                Where ItemsCache.Name = Key And ItemsCache.UserID = UserId And ItemsCache.Group = Group
                                Select ItemsCache).FirstOrDefault
            ResponseService.SetData(ItemUsuarioCache)
            Return ResponseService
        End Function

        Public Function Guardar(UserId As Int64, Group As String, Key As String, Value As String, ExpirationDateTime As DateTime) As FW_ResponseService
            Dim UsuarioCache As New DAO_UsuarioCache
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            With UsuarioCache
                .Name = Key
                .Value = Value
                .Group = Group
                .UserID = UserId
                .ExpirationDateTime = ExpirationDateTime
            End With
            Guardar(UsuarioCache)
            Return ResponseService
        End Function

        Public Function Guardar(ItemUsuarioCache As DAO_UsuarioCache) As FW_ResponseService
            EliminarPorNombre(ItemUsuarioCache.UserID, ItemUsuarioCache.Group, ItemUsuarioCache.Name)
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ContextDB.UsersCache.AddOrUpdate(ItemUsuarioCache)
            Save()
            ResponseService.SetData(ItemUsuarioCache)
            EliminarExpirados()
            Return ResponseService
        End Function

        Public Function EliminarPorNombre(UserId As Int64, Group As String, Key As String) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim Gestor_UserCache As New Ges_UserCache(Me.Transaction)
            Dim ItemUsuarioCache As DAO_UsuarioCache
            Dim ServiceUserCache As FW_ResponseService
            ServiceUserCache = Gestor_UserCache.ObtenerPorClave(UserId, Group, Key)
            If ServiceUserCache.IsSingle Then
                ItemUsuarioCache = ServiceUserCache.Data
                ContextDB.UsersCache.Remove(ItemUsuarioCache)
                Save()
            End If
            Return ResponseService
        End Function

        Public Function Eliminar(ItemUsuarioCache As DAO_UsuarioCache)
            ContextDB.UsersCache.Remove(ItemUsuarioCache)
            Save()
        End Function

        Public Function EliminarPorId(Id As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim Gestor_UserCache As New Ges_UserCache(Me.Transaction)
            Dim ItemUsuarioCache As DAO_UsuarioCache
            ItemUsuarioCache = Gestor_UserCache.ObtenerPorId(Id).
                          Unsuccessfully(Enum_ResultsServices.Void, "No se encontro el item de cache de usuario").
                          Data
            Eliminar(ItemUsuarioCache)
            Return ResponseService
        End Function

        Public Sub EliminarExpirados()
            Dim ItemsExpirados As List(Of DAO_UsuarioCache)
            ItemsExpirados = (From items
                              In ContextDB.UsersCache
                              Where items.ExpirationDateTime < DateTime.Now
                              Select items
                                  ).ToList
            If ItemsExpirados.Count > 0 Then
                ContextDB.UsersCache.RemoveRange(ItemsExpirados.AsEnumerable)
            End If
            Save()
        End Sub
    End Class
End Namespace
