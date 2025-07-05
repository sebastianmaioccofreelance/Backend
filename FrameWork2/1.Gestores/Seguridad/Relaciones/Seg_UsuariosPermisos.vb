



Imports FrameWork.Bases

Imports FrameWork.DAO
Imports System.Linq
Imports System.Collections.Generic

Namespace Gestores
    Partial Public Class Ges_Seguridad

#Region "1 a 1"

#Region "Asignar (1 a 1)"



        Public Function AsignarUsuarioAPermiso(UsuarioId As Int64, PermisoId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            If UsuarioPermisoNoExiste(UsuarioId, PermisoId).IsTrue Then
                Dim NuevoUsuarioPermiso As New DAO_UsuarioPermiso
                NuevoUsuarioPermiso.IdUsuario = UsuarioId
                NuevoUsuarioPermiso.IdPermiso = PermisoId
                ContextDB.UsuariosPermisos.Add(NuevoUsuarioPermiso)
                Save()
            End If
            Return ResponseService
        End Function

        Public Function GuardarUsuarioPermiso(UsuarioPermiso As DAO_UsuarioPermiso) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            If UsuarioPermisoNoExiste(UsuarioPermiso.IdUsuario, UsuarioPermiso.IdPermiso).IsTrue Then
                ContextDB.UsuariosPermisos.Add(UsuarioPermiso)
            End If
            Save()
            Return ResponseService
        End Function


        Public Function AsignarUsuarioAPermiso(Usuario As DAO_Usuario, PermisoId As Int64) As FW_ResponseService
            Return AsignarUsuarioAPermiso(Usuario.Id, PermisoId)
        End Function

        Public Function AsignarUsuarioAPermiso(UsuarioId As Int64, Permiso As DAO_Permiso) As FW_ResponseService
            Return AsignarUsuarioAPermiso(UsuarioId, Permiso.Id)
        End Function

        Public Function AsignarUsuarioAPermiso(Usuario As DAO_Usuario, Permiso As DAO_Permiso) As FW_ResponseService
            Return AsignarUsuarioAPermiso(Usuario.Id, Permiso.Id)
        End Function



        Public Function AsignarPermisoAUsuario(PermisoId As Int64, Usuario As DAO_Usuario) As FW_ResponseService
            Return AsignarUsuarioAPermiso(Usuario.Id, PermisoId)
        End Function

        Public Function AsignarPermisoAUsuario(Permiso As DAO_Permiso, UsuarioId As Int64) As FW_ResponseService
            Return AsignarUsuarioAPermiso(UsuarioId, Permiso.Id)
        End Function

        Public Function AsignarPermisoAUsuario(Permiso As DAO_Permiso, Usuario As DAO_Usuario) As FW_ResponseService
            Return AsignarUsuarioAPermiso(Usuario.Id, Permiso.Id)
        End Function

        Public Function AsignarPermisoAUsuario(PermisoId As Int64, UsuarioId As Int64) As FW_ResponseService
            Return AsignarUsuarioAPermiso(UsuarioId, PermisoId)
        End Function

#End Region

#Region "Desasignar (1 a 1)"


        Public Function DesasignarUsuarioDePermiso(UsuarioId As Int64, PermisoId As Int64) As FW_ResponseService
            Dim ServiciosSeguridad As New Ges_Seguridad(Me)
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService = ServiciosSeguridad.UsuarioPermisoExiste(UsuarioId, PermisoId)
            If ResponseService.IsTrue Then
                DesasignarPermisoDeUsuario(ResponseService.Data)
            End If
            Return ResponseService
        End Function

        Public Function DesasignarUsuarioDePermiso(Usuario As DAO_Usuario, PermisoId As Int64) As FW_ResponseService
            Return DesasignarUsuarioDePermiso(Usuario.Id, PermisoId)
        End Function

        Public Function DesasignarUsuarioDePermiso(UsuarioId As Int64, Permiso As DAO_Permiso) As FW_ResponseService
            Return DesasignarUsuarioDePermiso(UsuarioId, Permiso.Id)
        End Function

        Public Function DesasignarUsuarioDePermiso(Usuario As DAO_Usuario, Permiso As DAO_Permiso) As FW_ResponseService
            Return DesasignarUsuarioDePermiso(Usuario.Id, Permiso.Id)
        End Function



        Public Function DesasignarPermisoDeUsuario(PermisoId As Int64, UsuarioId As Int64) As FW_ResponseService
            Return DesasignarUsuarioDePermiso(UsuarioId, PermisoId)
        End Function

        Public Function DesasignarPermisoDeUsuario(Permiso As DAO_Permiso, UsuarioId As Int64) As FW_ResponseService
            Return DesasignarUsuarioDePermiso(UsuarioId, Permiso.Id)
        End Function

        Public Function DesasignarPermisoDeUsuario(PermisoId As Int64, Usuario As DAO_Usuario) As FW_ResponseService
            Return DesasignarUsuarioDePermiso(Usuario.Id, PermisoId)
        End Function

        Public Function DesasignarPermisoDeUsuario(Permiso As DAO_Permiso, Usuario As DAO_Usuario) As FW_ResponseService
            Return DesasignarUsuarioDePermiso(Usuario.Id, Permiso.Id)
        End Function



        Public Function DesasignarUsuarioDePermiso(UsuarioPermiso As DAO_UsuarioPermiso) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService = ObtenerUsuarioPermiso(UsuarioPermiso.Id)
            If ResponseService.IsTrue Then
                ContextDB.UsuariosPermisos.Remove(ResponseService.Data)
                Save()
            End If
            Return ResponseService
        End Function

        Public Function DesasignarPermisoDeUsuario(UsuarioPermiso As DAO_UsuarioPermiso) As FW_ResponseService
            Return DesasignarUsuarioDePermiso(UsuarioPermiso)
        End Function

        Public Function EliminarUsuarioPermiso(UsuarioPermiso As DAO_UsuarioPermiso) As FW_ResponseService
            Return DesasignarUsuarioDePermiso(UsuarioPermiso)
        End Function

        Public Function DesasignarPermisoDeUsuario(UsuarioPermisosId As Int64) As FW_ResponseService
            Dim ServiciosSeguridad As New Ges_Seguridad(Me)
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService = ServiciosSeguridad.UsuarioPermisoExiste(UsuarioPermisosId)
            If ResponseService.IsTrue Then
                DesasignarPermisoDeUsuario(ResponseService.Data)
            End If
            Return ResponseService
        End Function

        Public Function DesasignarUsuarioDePermiso(UsuarioPermisosId As Int64) As FW_ResponseService
            Return DesasignarPermisoDeUsuario(UsuarioPermisosId)
        End Function

#End Region

#End Region

#Region "1 a muchos"

#Region "Asignar (1 a muchos)"


        Public Function GuardarUsuariosPermisos(UsuariosPermisos() As DAO_UsuarioPermiso) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each UsuarioPermiso As DAO_UsuarioPermiso In UsuariosPermisos
                GuardarUsuarioPermiso(UsuarioPermiso)
            Next
            Return ResponseService
        End Function

        Public Function AsignarUsuariosAPermiso(UsuarioIds() As Int64, PermisoId As Int64) As FW_ResponseService '1
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdUsuario As Int64 In UsuarioIds
                AsignarUsuarioAPermiso(IdUsuario, PermisoId)
            Next
            Return ResponseService
        End Function

        Public Function AsignarUsuariosAPermiso(UsuarioIds() As Int64, Permiso As DAO_Permiso) As FW_ResponseService '3
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdUsuario As Int64 In UsuarioIds
                AsignarUsuarioAPermiso(IdUsuario, Permiso.Id)
            Next
            Return ResponseService
        End Function

        Public Function AsignarUsuariosAPermiso(Usuarios() As DAO_Usuario, PermisoId As Int64) As FW_ResponseService '5
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Usuario As DAO_Usuario In Usuarios
                AsignarUsuarioAPermiso(Usuario.Id, PermisoId)
            Next
            Return ResponseService
        End Function

        Public Function AsignarUsuariosAPermiso(Usuarios() As DAO_Usuario, Permiso As DAO_Permiso) As FW_ResponseService '7
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Usuario As DAO_Usuario In Usuarios
                AsignarUsuarioAPermiso(Usuario.Id, Permiso.Id)
            Next
            Return ResponseService
        End Function



        Public Function AsignarPermisosAUsuario(PermisosIds() As Int64, UsuarioId As Int64) As FW_ResponseService '2
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdPermiso As Int64 In PermisosIds
                AsignarUsuarioAPermiso(UsuarioId, IdPermiso)
            Next
            Return ResponseService
        End Function

        Public Function AsignarPermisosAUsuario(Permisos() As DAO_Permiso, UsuarioId As Int64) As FW_ResponseService '4
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Permiso As DAO_Permiso In Permisos
                AsignarUsuarioAPermiso(UsuarioId, Permiso.Id)
            Next
            Return ResponseService
        End Function

        Public Function AsignarPermisosAUsuario(PermisosId() As Int64, Usuario As DAO_Usuario) As FW_ResponseService '6
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each PermisoId As Int64 In PermisosId
                AsignarUsuarioAPermiso(Usuario.Id, PermisoId)
            Next
            Return ResponseService
        End Function

        Public Function AsignarPermisosAUsuario(Permisos() As DAO_Permiso, Usuario As DAO_Usuario) As FW_ResponseService '8
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Permiso As DAO_Permiso In Permisos
                AsignarUsuarioAPermiso(Usuario.Id, Permiso.Id)
            Next
            Return ResponseService
        End Function

#End Region

#Region "Desasignar ( 1 a muchos)"



        Public Function DesasignarUsuariosDePermiso(UsuarioIds() As Int64, PermisoId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdUsuario As Int64 In UsuarioIds
                DesasignarUsuarioDePermiso(IdUsuario, PermisoId)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarUsuariosDePermiso(UsuarioIds() As Int64, Permiso As DAO_Permiso) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdUsuario As Int64 In UsuarioIds
                DesasignarUsuarioDePermiso(IdUsuario, Permiso.Id)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarUsuariosDePermiso(Usuarios() As DAO_Usuario, PermisoId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Usuario As DAO_Usuario In Usuarios
                DesasignarUsuarioDePermiso(Usuario.Id, PermisoId)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarUsuariosDePermiso(Usuarios() As DAO_Usuario, Permiso As DAO_Permiso) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Usuario As DAO_Usuario In Usuarios
                DesasignarUsuarioDePermiso(Usuario.Id, Permiso.Id)
            Next
            Return ResponseService
        End Function




        Public Function DesasignarPermisosDeUsuario(PermisosIds() As Int64, UsuarioId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdPermiso As Int64 In PermisosIds
                DesasignarUsuarioDePermiso(UsuarioId, IdPermiso)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarPermisosDeUsuario(Permiso() As DAO_Permiso, UsuarioId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each GrupoUsuario As DAO_Permiso In Permiso
                DesasignarUsuarioDePermiso(UsuarioId, GrupoUsuario.Id)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarPermisosDeUsuario(PermisoIds() As Int64, Usuario As DAO_Usuario) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdGrupoUsuario As Int64 In PermisoIds
                DesasignarUsuarioDePermiso(Usuario.Id, IdGrupoUsuario)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarPermisosDeUsuario(Permisos() As DAO_Permiso, Usuario As DAO_Usuario) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Permiso As DAO_Permiso In Permisos
                DesasignarUsuarioDePermiso(Usuario.Id, Permiso.Id)
            Next
            Return ResponseService
        End Function

        Public Function EliminarPermisosUsuarios(UsuariosPermisos() As DAO_UsuarioPermiso) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each UsuarioPermiso As DAO_UsuarioPermiso In UsuariosPermisos
                EliminarUsuarioPermiso(UsuarioPermiso)
            Next
            Return ResponseService
        End Function



#End Region

#End Region

#Region "Obtener / Existe / No existe / Pertenece /  No pertenece "

        Public Function ObtenerUsuarioPermiso(UsuarioPermidoId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaUsuariosPermisos As List(Of DAO_UsuarioPermiso)
            ListaUsuariosPermisos = ContextDB.UsuariosPermisos.Where(Function(n) n.Id = UsuarioPermidoId).ToList
            If ListaUsuariosPermisos.Count = 1 Then
                ResponseService.SetData(ListaUsuariosPermisos.First)
                ResponseService.SetTrue()
            Else
                ResponseService.SetFalse()
            End If
            Return ResponseService
        End Function

        Public Function UsuarioPermisoExiste(UsuarioId As Int64, PermisoId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaUsuariosPermisos As List(Of DAO_UsuarioPermiso)
            ListaUsuariosPermisos = ContextDB.UsuariosPermisos.Where(Function(n) n.IdPermiso = PermisoId And n.IdUsuario = UsuarioId).ToList
            If ListaUsuariosPermisos.Count = 1 Then
                ResponseService.SetData(ListaUsuariosPermisos.First)
                ResponseService.SetTrue()
            Else
                ResponseService.SetFalse()
            End If
            Return ResponseService
        End Function

        Public Function UsuarioPermisoExiste(UsuarioPermisoId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaUsuariosPermisos As List(Of DAO_UsuarioPermiso)
            ListaUsuariosPermisos = ContextDB.UsuariosPermisos.Where(Function(n) n.Id = UsuarioPermisoId).ToList
            If ListaUsuariosPermisos.Count = 1 Then
                ResponseService.SetData(ListaUsuariosPermisos.First)
                ResponseService.SetTrue()
            Else
                ResponseService.SetFalse()
            End If
            Return ResponseService
        End Function

        Public Function UsuarioPermisoNoExiste(UsuarioId As Int64, PermisoId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService.SetResultsByBooleanValue(UsuarioPermisoExiste(UsuarioId, PermisoId).IsFalse)
            Return ResponseService
        End Function

        Public Function UsuarioPermisoNoExiste(UsuarioPermisoId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService.SetResultsByBooleanValue(UsuarioPermisoExiste(UsuarioPermisoId).IsFalse)
            Return ResponseService
        End Function

        Public Function PermisoPerteneceAUsuario(PermisoId As Int64, UsuarioId As Int64) As FW_ResponseService
            Return UsuarioPermisoExiste(UsuarioId, PermisoId)
        End Function

        Public Function UsuarioPerteneceAPermiso(UsuarioId As Int64, PermisoId As Int64) As FW_ResponseService
            Return UsuarioPermisoExiste(UsuarioId, PermisoId)
        End Function

#End Region

#Region "Listar"

        Public Function ListarUsuariosDeUnPermiso(PermisoId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaUsuarios As List(Of DAO_Usuario)
            ListaUsuarios = (From t_UsuarioPermiso In ContextDB.UsuariosPermisos
                             Join t_Usuarios In ContextDB.Usuarios On t_Usuarios.Id Equals t_UsuarioPermiso.IdUsuario
                             Where t_UsuarioPermiso.IdPermiso = PermisoId
                             Select t_Usuarios).ToList
            ResponseService.SetData(ListaUsuarios)
            Return ResponseService
        End Function

        Public Function ListarPermisosDeUsuario(UsuarioId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaPermisos As List(Of DAO_Permiso)
            ListaPermisos = (From t_UsuarioPermiso In ContextDB.UsuariosPermisos
                             Join t_Permisos In ContextDB.Permisos On t_Permisos.Id Equals t_UsuarioPermiso.IdPermiso
                             Where t_UsuarioPermiso.IdUsuario = UsuarioId
                             Select t_Permisos).ToList
            ResponseService.SetData(ListaPermisos)
            Return ResponseService
        End Function

        Public Function ListarUsuariosDePermiso(Permiso As DAO_Permiso) As FW_ResponseService
            Return ListarUsuariosDeUnPermiso(Permiso.Id)
        End Function

        Public Function ListarPermisosDeUsuario(Usuario As DAO_Usuario) As FW_ResponseService
            Return ListarPermisosDeUsuario(Usuario.Id)
        End Function
#End Region

    End Class
End Namespace
