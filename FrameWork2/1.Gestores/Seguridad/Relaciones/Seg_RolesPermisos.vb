



Imports FrameWork.Bases

Imports FrameWork.DAO
Imports System.Linq
Imports System.Collections.Generic

Namespace Gestores
    Partial Public Class Ges_Seguridad

#Region "1 a 1"

#Region "Asignar (1 a 1)"



        Public Function AsignarRolAPermiso(RolId As Int64, PermisoId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            If RolPermisoNoExiste(RolId, PermisoId).IsTrue Then
                Dim NuevoRolPermiso As New DAO_RolPermiso
                NuevoRolPermiso.IdRol = RolId
                NuevoRolPermiso.IdPermiso = PermisoId
                ContextDB.RolesPermisos.Add(NuevoRolPermiso)
                Save()
            End If
            Return ResponseService
        End Function

        Public Function GuardarRolPermiso(RolPermiso As DAO_RolPermiso) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            If RolPermisoNoExiste(RolPermiso.IdRol, RolPermiso.IdPermiso).IsTrue Then
                ContextDB.RolesPermisos.Add(RolPermiso)
            End If
            Save()
            Return ResponseService
        End Function


        Public Function AsignarRolAPermiso(Rol As DAO_Rol, PermisoId As Int64) As FW_ResponseService
            Return AsignarRolAPermiso(Rol.Id, PermisoId)
        End Function

        Public Function AsignarRolAPermiso(RolId As Int64, Permiso As DAO_Permiso) As FW_ResponseService
            Return AsignarRolAPermiso(RolId, Permiso.Id)
        End Function

        Public Function AsignarRolAPermiso(Rol As DAO_Rol, Permiso As DAO_Permiso) As FW_ResponseService
            Return AsignarRolAPermiso(Rol.Id, Permiso.Id)
        End Function



        Public Function AsignarPermisoARol(PermisoId As Int64, Rol As DAO_Rol) As FW_ResponseService
            Return AsignarRolAPermiso(Rol.Id, PermisoId)
        End Function

        Public Function AsignarPermisoARol(Permiso As DAO_Permiso, RolId As Int64) As FW_ResponseService
            Return AsignarRolAPermiso(RolId, Permiso.Id)
        End Function

        Public Function AsignarPermisoARol(Permiso As DAO_Permiso, Rol As DAO_Rol) As FW_ResponseService
            Return AsignarRolAPermiso(Rol.Id, Permiso.Id)
        End Function

        Public Function AsignarPermisoARol(PermisoId As Int64, RolId As Int64) As FW_ResponseService
            Return AsignarRolAPermiso(RolId, PermisoId)
        End Function

#End Region

#Region "Desasignar (1 a 1)"


        Public Function DesasignarRolDePermiso(RolId As Int64, PermisoId As Int64) As FW_ResponseService
            Dim ServiciosSeguridad As New Ges_Seguridad(Me)
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService = ServiciosSeguridad.RolPermisoExiste(RolId, PermisoId)
            If ResponseService.IsTrue Then
                DesasignarPermisoDeRol(ResponseService.Data)
            End If
            Return ResponseService
        End Function

        Public Function DesasignarRolDePermiso(Rol As DAO_Rol, PermisoId As Int64) As FW_ResponseService
            Return DesasignarRolDePermiso(Rol.Id, PermisoId)
        End Function

        Public Function DesasignarRolDePermiso(RolId As Int64, Permiso As DAO_Permiso) As FW_ResponseService
            Return DesasignarRolDePermiso(RolId, Permiso.Id)
        End Function

        Public Function DesasignarRolDePermiso(Rol As DAO_Rol, Permiso As DAO_Permiso) As FW_ResponseService
            Return DesasignarRolDePermiso(Rol.Id, Permiso.Id)
        End Function



        Public Function DesasignarPermisoDeRol(PermisoId As Int64, RolId As Int64) As FW_ResponseService
            Return DesasignarRolDePermiso(RolId, PermisoId)
        End Function

        Public Function DesasignarPermisoDeRol(Permiso As DAO_Permiso, RolId As Int64) As FW_ResponseService
            Return DesasignarRolDePermiso(RolId, Permiso.Id)
        End Function

        Public Function DesasignarPermisoDeRol(PermisoId As Int64, Rol As DAO_Rol) As FW_ResponseService
            Return DesasignarRolDePermiso(Rol.Id, PermisoId)
        End Function

        Public Function DesasignarPermisoDeRol(Permiso As DAO_Permiso, Rol As DAO_Rol) As FW_ResponseService
            Return DesasignarRolDePermiso(Rol.Id, Permiso.Id)
        End Function



        Public Function DesasignarRolDePermiso(RolPermiso As DAO_RolPermiso) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService = ObtenerRolPermiso(RolPermiso.Id)
            If ResponseService.IsTrue Then
                ContextDB.RolesPermisos.Remove(ResponseService.Data)
                Save()
            End If
            Return ResponseService
        End Function

        Public Function DesasignarPermisoDeRol(RolPermiso As DAO_RolPermiso) As FW_ResponseService
            Return DesasignarRolDePermiso(RolPermiso)
        End Function

        Public Function EliminarRolPermiso(RolPermiso As DAO_RolPermiso) As FW_ResponseService
            Return DesasignarRolDePermiso(RolPermiso)
        End Function

        Public Function DesasignarPermisoDeRol(RolPermisosId As Int64) As FW_ResponseService
            Dim ServiciosSeguridad As New Ges_Seguridad(Me)
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService = ServiciosSeguridad.RolPermisoExiste(RolPermisosId)
            If ResponseService.IsTrue Then
                DesasignarPermisoDeRol(ResponseService.Data)
            End If
            Return ResponseService
        End Function

        Public Function DesasignarRolDePermiso(RolPermisosId As Int64) As FW_ResponseService
            Return DesasignarPermisoDeRol(RolPermisosId)
        End Function

#End Region

#End Region

#Region "1 a muchos"

#Region "Asignar (1 a muchos)"


        Public Function GuardarRolesPermisos(RolesPermisos() As DAO_RolPermiso) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each RolPermiso As DAO_RolPermiso In RolesPermisos
                GuardarRolPermiso(RolPermiso)
            Next
            Return ResponseService
        End Function

        Public Function AsignarRolesAPermiso(RolIds() As Int64, PermisoId As Int64) As FW_ResponseService '1
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdRol As Int64 In RolIds
                AsignarRolAPermiso(IdRol, PermisoId)
            Next
            Return ResponseService
        End Function

        Public Function AsignarRolesAPermiso(RolIds() As Int64, Permiso As DAO_Permiso) As FW_ResponseService '3
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdRol As Int64 In RolIds
                AsignarRolAPermiso(IdRol, Permiso.Id)
            Next
            Return ResponseService
        End Function

        Public Function AsignarRolesAPermiso(Roles() As DAO_Rol, PermisoId As Int64) As FW_ResponseService '5
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Rol As DAO_Rol In Roles
                AsignarRolAPermiso(Rol.Id, PermisoId)
            Next
            Return ResponseService
        End Function

        Public Function AsignarRolesAPermiso(Roles() As DAO_Rol, Permiso As DAO_Permiso) As FW_ResponseService '7
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Rol As DAO_Rol In Roles
                AsignarRolAPermiso(Rol.Id, Permiso.Id)
            Next
            Return ResponseService
        End Function



        Public Function AsignarPermisosARol(PermisosIds() As Int64, RolId As Int64) As FW_ResponseService '2
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdPermiso As Int64 In PermisosIds
                AsignarRolAPermiso(RolId, IdPermiso)
            Next
            Return ResponseService
        End Function

        Public Function AsignarPermisosARol(Permisos() As DAO_Permiso, RolId As Int64) As FW_ResponseService '4
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Permiso As DAO_Permiso In Permisos
                AsignarRolAPermiso(RolId, Permiso.Id)
            Next
            Return ResponseService
        End Function

        Public Function AsignarPermisosARol(PermisosId() As Int64, Rol As DAO_Rol) As FW_ResponseService '6
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each PermisoId As Int64 In PermisosId
                AsignarRolAPermiso(Rol.Id, PermisoId)
            Next
            Return ResponseService
        End Function

        Public Function AsignarPermisosARol(Permisos() As DAO_Permiso, Rol As DAO_Rol) As FW_ResponseService '8
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Permiso As DAO_Permiso In Permisos
                AsignarRolAPermiso(Rol.Id, Permiso.Id)
            Next
            Return ResponseService
        End Function

#End Region

#Region "Desasignar ( 1 a muchos)"



        Public Function DesasignarRolesDePermiso(RolIds() As Int64, PermisoId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdRol As Int64 In RolIds
                DesasignarRolDePermiso(IdRol, PermisoId)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarRolesDePermiso(RolIds() As Int64, Permiso As DAO_Permiso) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdRol As Int64 In RolIds
                DesasignarRolDePermiso(IdRol, Permiso.Id)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarRolesDePermiso(Roles() As DAO_Rol, PermisoId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Rol As DAO_Rol In Roles
                DesasignarRolDePermiso(Rol.Id, PermisoId)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarRolesDePermiso(Roles() As DAO_Rol, Permiso As DAO_Permiso) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Rol As DAO_Rol In Roles
                DesasignarRolDePermiso(Rol.Id, Permiso.Id)
            Next
            Return ResponseService
        End Function




        Public Function DesasignarPermisosDeRol(PermisosIds() As Int64, RolId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdPermiso As Int64 In PermisosIds
                DesasignarRolDePermiso(RolId, IdPermiso)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarPermisosDeRol(Permiso() As DAO_Permiso, RolId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each GrupoRol As DAO_Permiso In Permiso
                DesasignarRolDePermiso(RolId, GrupoRol.Id)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarPermisosDeRol(PermisoIds() As Int64, Rol As DAO_Rol) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdGrupoRol As Int64 In PermisoIds
                DesasignarRolDePermiso(Rol.Id, IdGrupoRol)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarPermisosDeRol(Permisos() As DAO_Permiso, Rol As DAO_Rol) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Permiso As DAO_Permiso In Permisos
                DesasignarRolDePermiso(Rol.Id, Permiso.Id)
            Next
            Return ResponseService
        End Function

        Public Function EliminarPermisosRoles(RolesPermisos() As DAO_RolPermiso) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each RolPermiso As DAO_RolPermiso In RolesPermisos
                EliminarRolPermiso(RolPermiso)
            Next
            Return ResponseService
        End Function



#End Region

#End Region

#Region "Obtener / Existe / No existe / Pertenece /  No pertenece "

        Public Function ObtenerRolPermiso(RolPermidoId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaRolesPermisos As List(Of DAO_RolPermiso)
            ListaRolesPermisos = ContextDB.RolesPermisos.Where(Function(n) n.Id = RolPermidoId).ToList
            If ListaRolesPermisos.Count = 1 Then
                ResponseService.SetData(ListaRolesPermisos.First)
                ResponseService.SetTrue()
            Else
                ResponseService.SetFalse()
            End If
            Return ResponseService
        End Function

        Public Function RolPermisoExiste(RolId As Int64, PermisoId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaRolesPermisos As List(Of DAO_RolPermiso)
            ListaRolesPermisos = ContextDB.RolesPermisos.Where(Function(n) n.IdPermiso = PermisoId And n.IdRol = RolId).ToList
            If ListaRolesPermisos.Count = 1 Then
                ResponseService.SetData(ListaRolesPermisos.First)
                ResponseService.SetTrue()
            Else
                ResponseService.SetFalse()
            End If
            Return ResponseService
        End Function

        Public Function RolPermisoExiste(RolPermisoId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaRolesPermisos As List(Of DAO_RolPermiso)
            ListaRolesPermisos = ContextDB.RolesPermisos.Where(Function(n) n.Id = RolPermisoId).ToList
            If ListaRolesPermisos.Count = 1 Then
                ResponseService.SetData(ListaRolesPermisos.First)
                ResponseService.SetTrue()
            Else
                ResponseService.SetFalse()
            End If
            Return ResponseService
        End Function

        Public Function RolPermisoNoExiste(RolId As Int64, PermisoId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService.SetResultsByBooleanValue(RolPermisoExiste(RolId, PermisoId).IsFalse)
            Return ResponseService
        End Function

        Public Function RolPermisoNoExiste(RolPermisoId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService.SetResultsByBooleanValue(RolPermisoExiste(RolPermisoId).IsFalse)
            Return ResponseService
        End Function

        Public Function PermisoPerteneceARol(PermisoId As Int64, RolId As Int64) As FW_ResponseService
            Return RolPermisoExiste(RolId, PermisoId)
        End Function

        Public Function RolPerteneceAPermiso(RolId As Int64, PermisoId As Int64) As FW_ResponseService
            Return RolPermisoExiste(RolId, PermisoId)
        End Function

#End Region

#Region "Listar"

        Public Function ListarRolesDeUnPermiso(PermisoId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaRoles As List(Of DAO_Rol)
            ListaRoles = (From t_RolPermiso In ContextDB.RolesPermisos
                          Join t_Roles In ContextDB.Roles On t_Roles.Id Equals t_RolPermiso.IdRol
                          Where t_RolPermiso.IdPermiso = PermisoId
                          Select t_Roles).ToList
            ResponseService.SetData(ListaRoles)
            Return ResponseService
        End Function

        Public Function ListarPermisosDeRol(RolId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaPermisos As List(Of DAO_Permiso)
            ListaPermisos = (From t_RolPermiso In ContextDB.RolesPermisos
                             Join t_Permisos In ContextDB.Permisos On t_Permisos.Id Equals t_RolPermiso.IdPermiso
                             Where t_RolPermiso.IdRol = RolId
                             Select t_Permisos).ToList
            ResponseService.SetData(ListaPermisos)
            Return ResponseService
        End Function

        Public Function ListarRolesDePermiso(Permiso As DAO_Permiso) As FW_ResponseService
            Return ListarRolesDeUnPermiso(Permiso.Id)
        End Function

        Public Function ListarPermisosDeRol(Rol As DAO_Rol) As FW_ResponseService
            Return ListarPermisosDeRol(Rol.Id)
        End Function
#End Region

    End Class
End Namespace
