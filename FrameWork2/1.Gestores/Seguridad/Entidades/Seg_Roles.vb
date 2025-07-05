Imports FrameWork.Bases
Imports FrameWork.DAO
Imports System.Linq
Imports System.Collections.Generic
Imports System.Data.Entity.Migrations
Imports FrameWork.FW_ResponseService

Namespace Gestores
    Partial Public Class Ges_Seguridad

        Public Function ObtenerListaRoles(Optional SoloHabilitados As Boolean = False) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaRoles As List(Of DAO_Rol)
            If SoloHabilitados Then
                ListaRoles = ContextDB.Roles.Where(Function(n) n.Habilitado).OrderBy(Function(n) n.Descripcion).ToList
            Else
                ListaRoles = ContextDB.Roles.ToList.OrderBy(Function(n) n.Descripcion).ToList
            End If
            Return ResponseService.SetDataAndResults(ListaRoles, ListaRoles.Count)
        End Function

        Public Function ObtenerRol(IdRol As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim RolObtener As DAO_Rol
            RolObtener = (From Rol
                         In Me.ContextDB.Roles
                          Where Rol.Id = IdRol
                          Select Rol
        ).FirstOrDefault
            If RolObtener Is Nothing Then
                ResponseService.SetResults(Enum_ResultsServices.Void)
            Else
                ResponseService.SetResults(Enum_ResultsServices.Single)
            End If
            Return ResponseService.SetData(RolObtener)
        End Function

        Public Function GuardarRol(Rol As DAO_Rol) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ContextDB.Roles.AddOrUpdate(Rol)
            Save()
            Return ResponseService.SetData(Rol)
        End Function

        Public Function EliminarRol(IdRol As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim RolEliminar As DAO_Rol
            RolEliminar = ObtenerRol(IdRol).
                          Unsuccessfully(Enum_ResultsServices.Void, "No se encontro el rol a eliminar").
                          Data
            ContextDB.Roles.Remove(RolEliminar)
            Save()
            Return ResponseService
        End Function

        Public Function EliminarRoles(IdsRoles() As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdRol As Int64 In IdsRoles
                EliminarRol(IdRol)
            Next
            Return ResponseService
        End Function

        Public Function HabilitarRol(IdRol As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim RolHabilitar As DAO_Rol
            RolHabilitar = ObtenerRol(IdRol).
                        Unsuccessfully(Enum_ResultsServices.Void, "No se encontro el rol a habilitar").
                        Data
            RolHabilitar.Habilitado = True
            Save()
            Return ResponseService
        End Function

        Public Function HabilitarRoles(IdRoles() As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Id As Int64 In IdRoles
                HabilitarRol(Id)
            Next
            Return ResponseService
        End Function

        Public Function InhabilitarRoles(IdRoles() As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Id As Int64 In IdRoles
                InhabilitarRol(Id)
            Next
            Save()
            Return ResponseService
        End Function

        Public Function InhabilitarRol(IdRol As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim RolDeshabilitar As DAO_Rol
            RolDeshabilitar = ObtenerRol(IdRol).
                        Unsuccessfully(Enum_ResultsServices.Void, "No se encontro el rol a deshabilitar").
                        Data
            RolDeshabilitar.Habilitado = False
            Save()
            Return ResponseService
        End Function

        Public Function RolExiste(IdRol As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim Result As Boolean = ContextDB.Roles.Any(Function(n) n.Id = IdRol)
            ResponseService.SetResultsByBooleanValue(Result)
            Return ResponseService
        End Function

        Public Function RolNoExiste(IdRol As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService.SetResultsByBooleanValue(RolExiste(IdRol).IsFalse)
            Return ResponseService
        End Function



        Public Function BusquedaRoles(Buscar As String) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaRoles As List(Of DAO_Rol)
            ListaRoles = ContextDB.Roles.AsEnumerable.Where(Function(n) n.NombreRol.ToLower Like "*" + Buscar.ToLower + "*").ToList
            ResponseService.SetDataAndResults(ListaRoles, ListaRoles.Count)
            Return ResponseService
        End Function
    End Class
End Namespace

