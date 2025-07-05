



Imports FrameWork.Bases

Imports FrameWork.DAO
Imports System.Linq
Imports System.Collections.Generic

Namespace Gestores
    Partial Public Class Ges_Seguridad

#Region "1 a 1"

#Region "Asignar (1 a 1)"



        Public Function AsignarUsuarioARol(UsuarioId As Int64, RolId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            If UsuarioRolNoExiste(UsuarioId, RolId).IsTrue Then
                Dim NuevoUsuarioRol As New DAO_UsuarioRol
                NuevoUsuarioRol.IdUsuario = UsuarioId
                NuevoUsuarioRol.IdRol = RolId
                ContextDB.UsuariosRoles.Add(NuevoUsuarioRol)
                Save()
            End If
            Return ResponseService
        End Function

        Public Function GuardarUsuarioRol(UsuarioRol As DAO_UsuarioRol) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            If UsuarioRolNoExiste(UsuarioRol.IdUsuario, UsuarioRol.IdRol).IsTrue Then
                ContextDB.UsuariosRoles.Add(UsuarioRol)
            End If
            Save()
            Return ResponseService
        End Function


        Public Function AsignarUsuarioARol(Usuario As DAO_Usuario, RolId As Int64) As FW_ResponseService
            Return AsignarUsuarioARol(Usuario.Id, RolId)
        End Function

        Public Function AsignarUsuarioARol(UsuarioId As Int64, Rol As DAO_Rol) As FW_ResponseService
            Return AsignarUsuarioARol(UsuarioId, Rol.Id)
        End Function

        Public Function AsignarUsuarioARol(Usuario As DAO_Usuario, Rol As DAO_Rol) As FW_ResponseService
            Return AsignarUsuarioARol(Usuario.Id, Rol.Id)
        End Function



        Public Function AsignarRolAUsuario(RolId As Int64, Usuario As DAO_Usuario) As FW_ResponseService
            Return AsignarUsuarioARol(Usuario.Id, RolId)
        End Function

        Public Function AsignarRolAUsuario(Rol As DAO_Rol, UsuarioId As Int64) As FW_ResponseService
            Return AsignarUsuarioARol(UsuarioId, Rol.Id)
        End Function

        Public Function AsignarRolAUsuario(Rol As DAO_Rol, Usuario As DAO_Usuario) As FW_ResponseService
            Return AsignarUsuarioARol(Usuario.Id, Rol.Id)
        End Function

        Public Function AsignarRolAUsuario(RolId As Int64, UsuarioId As Int64) As FW_ResponseService
            Return AsignarUsuarioARol(UsuarioId, RolId)
        End Function

#End Region

#Region "Desasignar (1 a 1)"


        Public Function DesasignarUsuarioDeRol(UsuarioId As Int64, RolId As Int64) As FW_ResponseService
            Dim ServiciosSeguridad As New Ges_Seguridad(Me)
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService = ServiciosSeguridad.UsuarioRolExiste(UsuarioId, RolId)
            If ResponseService.IsTrue Then
                DesasignarRolDeUsuario(ResponseService.Data)
            End If
            Return ResponseService
        End Function

        Public Function DesasignarUsuarioDeRol(Usuario As DAO_Usuario, RolId As Int64) As FW_ResponseService
            Return DesasignarUsuarioDeRol(Usuario.Id, RolId)
        End Function

        Public Function DesasignarUsuarioDeRol(UsuarioId As Int64, Rol As DAO_Rol) As FW_ResponseService
            Return DesasignarUsuarioDeRol(UsuarioId, Rol.Id)
        End Function

        Public Function DesasignarUsuarioDeRol(Usuario As DAO_Usuario, Rol As DAO_Rol) As FW_ResponseService
            Return DesasignarUsuarioDeRol(Usuario.Id, Rol.Id)
        End Function



        Public Function DesasignarRolDeUsuario(RolId As Int64, UsuarioId As Int64) As FW_ResponseService
            Return DesasignarUsuarioDeRol(UsuarioId, RolId)
        End Function

        Public Function DesasignarRolDeUsuario(Rol As DAO_Rol, UsuarioId As Int64) As FW_ResponseService
            Return DesasignarUsuarioDeRol(UsuarioId, Rol.Id)
        End Function

        Public Function DesasignarRolDeUsuario(RolId As Int64, Usuario As DAO_Usuario) As FW_ResponseService
            Return DesasignarUsuarioDeRol(Usuario.Id, RolId)
        End Function

        Public Function DesasignarRolDeUsuario(Rol As DAO_Rol, Usuario As DAO_Usuario) As FW_ResponseService
            Return DesasignarUsuarioDeRol(Usuario.Id, Rol.Id)
        End Function



        Public Function DesasignarUsuarioDeRol(UsuarioRol As DAO_UsuarioRol) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService = ObtenerUsuarioRol(UsuarioRol.Id)
            If ResponseService.IsTrue Then
                ContextDB.UsuariosRoles.Remove(ResponseService.Data)
                Save()
            End If
            Return ResponseService
        End Function

        Public Function DesasignarRolDeUsuario(UsuarioRol As DAO_UsuarioRol) As FW_ResponseService
            Return DesasignarUsuarioDeRol(UsuarioRol)
        End Function

        Public Function EliminarUsuarioRol(UsuarioRol As DAO_UsuarioRol) As FW_ResponseService
            Return DesasignarUsuarioDeRol(UsuarioRol)
        End Function

        Public Function DesasignarRolDeUsuario(UsuarioRolesId As Int64) As FW_ResponseService
            Dim ServiciosSeguridad As New Ges_Seguridad(Me)
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService = ServiciosSeguridad.UsuarioRolExiste(UsuarioRolesId)
            If ResponseService.IsTrue Then
                DesasignarRolDeUsuario(ResponseService.Data)
            End If
            Return ResponseService
        End Function

        Public Function DesasignarUsuarioDeRol(UsuarioRolesId As Int64) As FW_ResponseService
            Return DesasignarRolDeUsuario(UsuarioRolesId)
        End Function

#End Region

#End Region

#Region "1 a muchos"

#Region "Asignar (1 a muchos)"


        Public Function GuardarUsuariosRoles(UsuariosRoles() As DAO_UsuarioRol) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each UsuarioRol As DAO_UsuarioRol In UsuariosRoles
                GuardarUsuarioRol(UsuarioRol)
            Next
            Return ResponseService
        End Function

        Public Function AsignarUsuariosARol(UsuarioIds() As Int64, RolId As Int64) As FW_ResponseService '1
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdUsuario As Int64 In UsuarioIds
                AsignarUsuarioARol(IdUsuario, RolId)
            Next
            Return ResponseService
        End Function

        Public Function AsignarUsuariosARol(UsuarioIds() As Int64, Rol As DAO_Rol) As FW_ResponseService '3
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdUsuario As Int64 In UsuarioIds
                AsignarUsuarioARol(IdUsuario, Rol.Id)
            Next
            Return ResponseService
        End Function

        Public Function AsignarUsuariosARol(Usuarios() As DAO_Usuario, RolId As Int64) As FW_ResponseService '5
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Usuario As DAO_Usuario In Usuarios
                AsignarUsuarioARol(Usuario.Id, RolId)
            Next
            Return ResponseService
        End Function

        Public Function AsignarUsuariosARol(Usuarios() As DAO_Usuario, Rol As DAO_Rol) As FW_ResponseService '7
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Usuario As DAO_Usuario In Usuarios
                AsignarUsuarioARol(Usuario.Id, Rol.Id)
            Next
            Return ResponseService
        End Function



        Public Function AsignarRolesAUsuario(RolesIds() As Int64, UsuarioId As Int64) As FW_ResponseService '2
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdRol As Int64 In RolesIds
                AsignarUsuarioARol(UsuarioId, IdRol)
            Next
            Return ResponseService
        End Function

        Public Function AsignarRolesAUsuario(Roles() As DAO_Rol, UsuarioId As Int64) As FW_ResponseService '4
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Rol As DAO_Rol In Roles
                AsignarUsuarioARol(UsuarioId, Rol.Id)
            Next
            Return ResponseService
        End Function

        Public Function AsignarRolesAUsuario(RolesId() As Int64, Usuario As DAO_Usuario) As FW_ResponseService '6
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each RolId As Int64 In RolesId
                AsignarUsuarioARol(Usuario.Id, RolId)
            Next
            Return ResponseService
        End Function

        Public Function AsignarRolesAUsuario(Roles() As DAO_Rol, Usuario As DAO_Usuario) As FW_ResponseService '8
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Rol As DAO_Rol In Roles
                AsignarUsuarioARol(Usuario.Id, Rol.Id)
            Next
            Return ResponseService
        End Function

#End Region

#Region "Desasignar ( 1 a muchos)"



        Public Function DesasignarUsuariosDeRol(UsuarioIds() As Int64, RolId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdUsuario As Int64 In UsuarioIds
                DesasignarUsuarioDeRol(IdUsuario, RolId)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarUsuariosDeRol(UsuarioIds() As Int64, Rol As DAO_Rol) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdUsuario As Int64 In UsuarioIds
                DesasignarUsuarioDeRol(IdUsuario, Rol.Id)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarUsuariosDeRol(Usuarios() As DAO_Usuario, RolId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Usuario As DAO_Usuario In Usuarios
                DesasignarUsuarioDeRol(Usuario.Id, RolId)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarUsuariosDeRol(Usuarios() As DAO_Usuario, Rol As DAO_Rol) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Usuario As DAO_Usuario In Usuarios
                DesasignarUsuarioDeRol(Usuario.Id, Rol.Id)
            Next
            Return ResponseService
        End Function




        Public Function DesasignarRolesDeUsuario(RolesIds() As Int64, UsuarioId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdRol As Int64 In RolesIds
                DesasignarUsuarioDeRol(UsuarioId, IdRol)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarRolesDeUsuario(Rol() As DAO_Rol, UsuarioId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each GrupoUsuario As DAO_Rol In Rol
                DesasignarUsuarioDeRol(UsuarioId, GrupoUsuario.Id)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarRolesDeUsuario(RolIds() As Int64, Usuario As DAO_Usuario) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdGrupoUsuario As Int64 In RolIds
                DesasignarUsuarioDeRol(Usuario.Id, IdGrupoUsuario)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarRolesDeUsuario(Roles() As DAO_Rol, Usuario As DAO_Usuario) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Rol As DAO_Rol In Roles
                DesasignarUsuarioDeRol(Usuario.Id, Rol.Id)
            Next
            Return ResponseService
        End Function

        Public Function EliminarRolesUsuarios(UsuariosRoles() As DAO_UsuarioRol) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each UsuarioRol As DAO_UsuarioRol In UsuariosRoles
                EliminarUsuarioRol(UsuarioRol)
            Next
            Return ResponseService
        End Function



#End Region

#End Region

#Region "Obtener / Existe / No existe / Pertenece /  No pertenece "

        Public Function ObtenerUsuarioRol(UsuarioPermidoId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaUsuariosRoles As List(Of DAO_UsuarioRol)
            ListaUsuariosRoles = ContextDB.UsuariosRoles.Where(Function(n) n.Id = UsuarioPermidoId).ToList
            If ListaUsuariosRoles.Count = 1 Then
                ResponseService.SetData(ListaUsuariosRoles.First)
                ResponseService.SetTrue()
            Else
                ResponseService.SetFalse()
            End If
            Return ResponseService
        End Function

        Public Function UsuarioRolExiste(UsuarioId As Int64, RolId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaUsuariosRoles As List(Of DAO_UsuarioRol)
            ListaUsuariosRoles = ContextDB.UsuariosRoles.Where(Function(n) n.IdRol = RolId And n.IdUsuario = UsuarioId).ToList
            If ListaUsuariosRoles.Count = 1 Then
                ResponseService.SetData(ListaUsuariosRoles.First)
                ResponseService.SetTrue()
            Else
                ResponseService.SetFalse()
            End If
            Return ResponseService
        End Function

        Public Function UsuarioRolExiste(UsuarioRolId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaUsuariosRoles As List(Of DAO_UsuarioRol)
            ListaUsuariosRoles = ContextDB.UsuariosRoles.Where(Function(n) n.Id = UsuarioRolId).ToList
            If ListaUsuariosRoles.Count = 1 Then
                ResponseService.SetData(ListaUsuariosRoles.First)
                ResponseService.SetTrue()
            Else
                ResponseService.SetFalse()
            End If
            Return ResponseService
        End Function

        Public Function UsuarioRolNoExiste(UsuarioId As Int64, RolId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService.SetResultsByBooleanValue(UsuarioRolExiste(UsuarioId, RolId).IsFalse)
            Return ResponseService
        End Function

        Public Function UsuarioRolNoExiste(UsuarioRolId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService.SetResultsByBooleanValue(UsuarioRolExiste(UsuarioRolId).IsFalse)
            Return ResponseService
        End Function

        Public Function RolPerteneceAUsuario(RolId As Int64, UsuarioId As Int64) As FW_ResponseService
            Return UsuarioRolExiste(UsuarioId, RolId)
        End Function

        Public Function UsuarioPerteneceARol(UsuarioId As Int64, RolId As Int64) As FW_ResponseService
            Return UsuarioRolExiste(UsuarioId, RolId)
        End Function

#End Region

#Region "Listar"

        Public Function ListarUsuariosDeUnRol(RolId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaUsuarios As List(Of DAO_Usuario)
            ListaUsuarios = (From t_UsuarioRol In ContextDB.UsuariosRoles
                             Join t_Usuarios In ContextDB.Usuarios On t_Usuarios.Id Equals t_UsuarioRol.IdUsuario
                             Where t_UsuarioRol.IdRol = RolId
                             Select t_Usuarios).ToList
            ResponseService.SetData(ListaUsuarios)
            Return ResponseService
        End Function

        Public Function ListarRolesDeUsuario(UsuarioId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaRoles As List(Of DAO_Rol)
            ListaRoles = (From t_UsuarioRol In ContextDB.UsuariosRoles
                          Join t_Roles In ContextDB.Roles On t_Roles.Id Equals t_UsuarioRol.IdRol
                          Where t_UsuarioRol.IdUsuario = UsuarioId
                          Select t_Roles).ToList
            ResponseService.SetData(ListaRoles)
            Return ResponseService
        End Function

        Public Function ListarUsuariosDeRol(Rol As DAO_Rol) As FW_ResponseService
            Return ListarUsuariosDeUnRol(Rol.Id)
        End Function

        Public Function ListarRolesDeUsuario(Usuario As DAO_Usuario) As FW_ResponseService
            Return ListarRolesDeUsuario(Usuario.Id)
        End Function
#End Region

    End Class
End Namespace
