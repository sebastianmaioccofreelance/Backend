



Imports FrameWork.Bases

Imports FrameWork.DAO
Imports System.Linq
Imports System.Collections.Generic

Namespace Gestores
    Partial Public Class Ges_Seguridad

#Region "1 a 1"

#Region "Asignar (1 a 1)"



        Public Function AsignarPermisoAGrupoDeUsuarios(PermisoId As Int64, GrupoDeUsuariosId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            If PermisoGrupoDeUsuariosNoExiste(PermisoId, GrupoDeUsuariosId).IsTrue Then
                Dim NuevoPermisoGrupoDeUsuarios As New DAO_PermisoGrupoDeUsuarios
                NuevoPermisoGrupoDeUsuarios.IdPermiso = PermisoId
                NuevoPermisoGrupoDeUsuarios.IdGrupoDeUsuarios = GrupoDeUsuariosId
                ContextDB.PermisosGruposDeUsuarios.Add(NuevoPermisoGrupoDeUsuarios)
                Save()
            End If
            Return ResponseService
        End Function

        Public Function GuardarPermisoGrupoDeUsuarios(PermisoGrupoDeUsuarios As DAO_PermisoGrupoDeUsuarios) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            If PermisoGrupoDeUsuariosNoExiste(PermisoGrupoDeUsuarios.IdPermiso, PermisoGrupoDeUsuarios.IdGrupoDeUsuarios).IsTrue Then
                ContextDB.PermisosGruposDeUsuarios.Add(PermisoGrupoDeUsuarios)
            End If
            Save()
            Return ResponseService
        End Function


        Public Function AsignarPermisoAGrupoDeUsuarios(Permiso As DAO_Permiso, GrupoDeUsuariosId As Int64) As FW_ResponseService
            Return AsignarPermisoAGrupoDeUsuarios(Permiso.Id, GrupoDeUsuariosId)
        End Function

        Public Function AsignarPermisoAGrupoDeUsuarios(PermisoId As Int64, GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService
            Return AsignarPermisoAGrupoDeUsuarios(PermisoId, GrupoDeUsuarios.Id)
        End Function

        Public Function AsignarPermisoAGrupoDeUsuarios(Permiso As DAO_Permiso, GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService
            Return AsignarPermisoAGrupoDeUsuarios(Permiso.Id, GrupoDeUsuarios.Id)
        End Function



        Public Function AsignarGrupoDeUsuariosAPermiso(GrupoDeUsuariosId As Int64, Permiso As DAO_Permiso) As FW_ResponseService
            Return AsignarPermisoAGrupoDeUsuarios(Permiso.Id, GrupoDeUsuariosId)
        End Function

        Public Function AsignarGrupoDeUsuariosAPermiso(GrupoDeUsuarios As DAO_GrupoDeUsuarios, PermisoId As Int64) As FW_ResponseService
            Return AsignarPermisoAGrupoDeUsuarios(PermisoId, GrupoDeUsuarios.Id)
        End Function

        Public Function AsignarGrupoDeUsuariosAPermiso(GrupoDeUsuarios As DAO_GrupoDeUsuarios, Permiso As DAO_Permiso) As FW_ResponseService
            Return AsignarPermisoAGrupoDeUsuarios(Permiso.Id, GrupoDeUsuarios.Id)
        End Function

        Public Function AsignarGrupoDeUsuariosAPermiso(GrupoDeUsuariosId As Int64, PermisoId As Int64) As FW_ResponseService
            Return AsignarPermisoAGrupoDeUsuarios(PermisoId, GrupoDeUsuariosId)
        End Function

#End Region

#Region "Desasignar (1 a 1)"


        Public Function DesasignarPermisoDeGrupoDeUsuarios(PermisoId As Int64, GrupoDeUsuariosId As Int64) As FW_ResponseService
            Dim ServiciosSeguridad As New Ges_Seguridad(Me)
            Dim ResponseService As New FW_ResponseService(Transaction)
            ResponseService = ServiciosSeguridad.PermisoGrupoDeUsuariosExiste(PermisoId, GrupoDeUsuariosId)
            If ResponseService.IsTrue Then
                DesasignarGrupoDeUsuariosDePermiso(ResponseService.Data)
            End If
            Return ResponseService
        End Function

        Public Function DesasignarPermisoDeGrupoDeUsuarios(Permiso As DAO_Permiso, GrupoDeUsuariosId As Int64) As FW_ResponseService
            Return DesasignarPermisoDeGrupoDeUsuarios(Permiso.Id, GrupoDeUsuariosId)
        End Function

        Public Function DesasignarPermisoDeGrupoDeUsuarios(PermisoId As Int64, GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService
            Return DesasignarPermisoDeGrupoDeUsuarios(PermisoId, GrupoDeUsuarios.Id)
        End Function

        Public Function DesasignarPermisoDeGrupoDeUsuarios(Permiso As DAO_Permiso, GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService
            Return DesasignarPermisoDeGrupoDeUsuarios(Permiso.Id, GrupoDeUsuarios.Id)
        End Function



        Public Function DesasignarGrupoDeUsuariosDePermiso(GrupoDeUsuariosId As Int64, PermisoId As Int64) As FW_ResponseService
            Return DesasignarPermisoDeGrupoDeUsuarios(PermisoId, GrupoDeUsuariosId)
        End Function

        Public Function DesasignarGrupoDeUsuariosDePermiso(GrupoDeUsuarios As DAO_GrupoDeUsuarios, PermisoId As Int64) As FW_ResponseService
            Return DesasignarPermisoDeGrupoDeUsuarios(PermisoId, GrupoDeUsuarios.Id)
        End Function

        Public Function DesasignarGrupoDeUsuariosDePermiso(GrupoDeUsuariosId As Int64, Permiso As DAO_Permiso) As FW_ResponseService
            Return DesasignarPermisoDeGrupoDeUsuarios(Permiso.Id, GrupoDeUsuariosId)
        End Function

        Public Function DesasignarGrupoDeUsuariosDePermiso(GrupoDeUsuarios As DAO_GrupoDeUsuarios, Permiso As DAO_Permiso) As FW_ResponseService
            Return DesasignarPermisoDeGrupoDeUsuarios(Permiso.Id, GrupoDeUsuarios.Id)
        End Function



        Public Function DesasignarPermisoDeGrupoDeUsuarios(PermisoGrupoDeUsuarios As DAO_PermisoGrupoDeUsuarios) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Transaction)
            ResponseService = ObtenerPermisoGrupoDeUsuarios(PermisoGrupoDeUsuarios.Id)
            If ResponseService.IsTrue Then
                ContextDB.PermisosGruposDeUsuarios.Remove(ResponseService.Data)
                Save()
            End If
            Return ResponseService
        End Function

        Public Function DesasignarGrupoDeUsuariosDePermiso(PermisoGrupoDeUsuarios As DAO_PermisoGrupoDeUsuarios) As FW_ResponseService
            Return DesasignarPermisoDeGrupoDeUsuarios(PermisoGrupoDeUsuarios)
        End Function

        Public Function EliminarPermisoGrupoDeUsuarios(PermisoGrupoDeUsuarios As DAO_PermisoGrupoDeUsuarios) As FW_ResponseService
            Return DesasignarPermisoDeGrupoDeUsuarios(PermisoGrupoDeUsuarios)
        End Function

        Public Function DesasignarGrupoDeUsuariosDePermiso(PermisoGruposDeUsuariosId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Transaction)
            Dim ServiciosSeguridad As New Ges_Seguridad(Me)
            ResponseService = ServiciosSeguridad.PermisoGrupoDeUsuariosExiste(PermisoGruposDeUsuariosId)
            If ResponseService.IsTrue Then
                DesasignarGrupoDeUsuariosDePermiso(ResponseService.Data)
            End If
            Return ResponseService
        End Function

        Public Function DesasignarPermisoDeGrupoDeUsuarios(PermisoGruposDeUsuariosId As Int64) As FW_ResponseService
            Return DesasignarGrupoDeUsuariosDePermiso(PermisoGruposDeUsuariosId)
        End Function

#End Region

#End Region

#Region "1 a muchos"

#Region "Asignar (1 a muchos)"


        Public Function GuardarPermisosGruposDeUsuarios(PermisosGruposDeUsuarios() As DAO_PermisoGrupoDeUsuarios) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Transaction)
            For Each PermisoGrupoDeUsuarios As DAO_PermisoGrupoDeUsuarios In PermisosGruposDeUsuarios
                GuardarPermisoGrupoDeUsuarios(PermisoGrupoDeUsuarios)
            Next
            Return ResponseService
        End Function

        Public Function AsignarPermisosAGrupoDeUsuarios(PermisoIds() As Int64, GrupoDeUsuariosId As Int64) As FW_ResponseService '1
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdPermiso As Int64 In PermisoIds
                AsignarPermisoAGrupoDeUsuarios(IdPermiso, GrupoDeUsuariosId)
            Next
            Return ResponseService
        End Function

        Public Function AsignarPermisosAGrupoDeUsuarios(PermisoIds() As Int64, GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService '3
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdPermiso As Int64 In PermisoIds
                AsignarPermisoAGrupoDeUsuarios(IdPermiso, GrupoDeUsuarios.Id)
            Next
            Return ResponseService
        End Function

        Public Function AsignarPermisosAGrupoDeUsuarios(Permisos() As DAO_Permiso, GrupoDeUsuariosId As Int64) As FW_ResponseService '5
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Permiso As DAO_Permiso In Permisos
                AsignarPermisoAGrupoDeUsuarios(Permiso.Id, GrupoDeUsuariosId)
            Next
            Return ResponseService
        End Function

        Public Function AsignarPermisosAGrupoDeUsuarios(Permisos() As DAO_Permiso, GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService '7
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Permiso As DAO_Permiso In Permisos
                AsignarPermisoAGrupoDeUsuarios(Permiso.Id, GrupoDeUsuarios.Id)
            Next
            Return ResponseService
        End Function



        Public Function AsignarGruposDeUsuariosAPermiso(GruposDeUsuariosIds() As Int64, PermisoId As Int64) As FW_ResponseService '2
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdGrupoDeUsuarios As Int64 In GruposDeUsuariosIds
                AsignarPermisoAGrupoDeUsuarios(PermisoId, IdGrupoDeUsuarios)
            Next
            Return ResponseService
        End Function

        Public Function AsignarGruposDeUsuariosAPermiso(GruposDeUsuarios() As DAO_GrupoDeUsuarios, PermisoId As Int64) As FW_ResponseService '4
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each GrupoDeUsuarios As DAO_GrupoDeUsuarios In GruposDeUsuarios
                AsignarPermisoAGrupoDeUsuarios(PermisoId, GrupoDeUsuarios.Id)
            Next
            Return ResponseService
        End Function

        Public Function AsignarGruposDeUsuariosAPermiso(GruposDeUsuariosId() As Int64, Permiso As DAO_Permiso) As FW_ResponseService '6
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each GrupoDeUsuariosId As Int64 In GruposDeUsuariosId
                AsignarPermisoAGrupoDeUsuarios(Permiso.Id, GrupoDeUsuariosId)
            Next
            Return ResponseService
        End Function

        Public Function AsignarGruposDeUsuariosAPermiso(GruposDeUsuarios() As DAO_GrupoDeUsuarios, Permiso As DAO_Permiso) As FW_ResponseService '8
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each GrupoDeUsuarios As DAO_GrupoDeUsuarios In GruposDeUsuarios
                AsignarPermisoAGrupoDeUsuarios(Permiso.Id, GrupoDeUsuarios.Id)
            Next
            Return ResponseService
        End Function

#End Region

#Region "Desasignar ( 1 a muchos)"



        Public Function DesasignarPermisosDeGrupoDeUsuarios(PermisoIds() As Int64, GrupoDeUsuariosId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdPermiso As Int64 In PermisoIds
                DesasignarPermisoDeGrupoDeUsuarios(IdPermiso, GrupoDeUsuariosId)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarPermisosDeGrupoDeUsuarios(PermisoIds() As Int64, GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdPermiso As Int64 In PermisoIds
                DesasignarPermisoDeGrupoDeUsuarios(IdPermiso, GrupoDeUsuarios.Id)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarPermisosDeGrupoDeUsuarios(Permisos() As DAO_Permiso, GrupoDeUsuariosId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Permiso As DAO_Permiso In Permisos
                DesasignarPermisoDeGrupoDeUsuarios(Permiso.Id, GrupoDeUsuariosId)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarPermisosDeGrupoDeUsuarios(Permisos() As DAO_Permiso, GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Permiso As DAO_Permiso In Permisos
                DesasignarPermisoDeGrupoDeUsuarios(Permiso.Id, GrupoDeUsuarios.Id)
            Next
            Return ResponseService
        End Function




        Public Function DesasignarGruposDeUsuariosDePermiso(GruposDeUsuariosIds() As Int64, PermisoId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdGrupoDeUsuarios As Int64 In GruposDeUsuariosIds
                DesasignarPermisoDeGrupoDeUsuarios(PermisoId, IdGrupoDeUsuarios)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarGruposDeUsuariosDePermiso(GrupoDeUsuarios() As DAO_GrupoDeUsuarios, PermisoId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each GrupoPermiso As DAO_GrupoDeUsuarios In GrupoDeUsuarios
                DesasignarPermisoDeGrupoDeUsuarios(PermisoId, GrupoPermiso.Id)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarGruposDeUsuariosDePermiso(GrupoDeUsuariosIds() As Int64, Permiso As DAO_Permiso) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdGrupoPermiso As Int64 In GrupoDeUsuariosIds
                DesasignarPermisoDeGrupoDeUsuarios(Permiso.Id, IdGrupoPermiso)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarGruposDeUsuariosDePermiso(GruposDeUsuarios() As DAO_GrupoDeUsuarios, Permiso As DAO_Permiso) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each GrupoDeUsuarios As DAO_GrupoDeUsuarios In GruposDeUsuarios
                DesasignarPermisoDeGrupoDeUsuarios(Permiso.Id, GrupoDeUsuarios.Id)
            Next
            Return ResponseService
        End Function

        Public Function EliminarGruposDeUsuariosPermisos(PermisosGruposDeUsuarios() As DAO_PermisoGrupoDeUsuarios) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each PermisoGrupoDeUsuarios As DAO_PermisoGrupoDeUsuarios In PermisosGruposDeUsuarios
                EliminarPermisoGrupoDeUsuarios(PermisoGrupoDeUsuarios)
            Next
            Return ResponseService
        End Function



#End Region

#End Region

#Region "Obtener / Existe / No existe / Pertenece /  No pertenece "

        Public Function ObtenerPermisoGrupoDeUsuarios(PermisoPermidoId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaPermisosGruposDeUsuarios As List(Of DAO_PermisoGrupoDeUsuarios)
            ListaPermisosGruposDeUsuarios = ContextDB.PermisosGruposDeUsuarios.Where(Function(n) n.Id = PermisoPermidoId).ToList
            If ListaPermisosGruposDeUsuarios.Count = 1 Then
                ResponseService.SetData(ListaPermisosGruposDeUsuarios.First)
                ResponseService.SetTrue()
            Else
                ResponseService.SetFalse()
            End If
            Return ResponseService
        End Function

        Public Function PermisoGrupoDeUsuariosExiste(PermisoId As Int64, GrupoDeUsuariosId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaPermisosGruposDeUsuarios As List(Of DAO_PermisoGrupoDeUsuarios)
            ListaPermisosGruposDeUsuarios = ContextDB.PermisosGruposDeUsuarios.Where(Function(n) n.IdGrupoDeUsuarios = GrupoDeUsuariosId And n.IdPermiso = PermisoId).ToList
            If ListaPermisosGruposDeUsuarios.Count = 1 Then
                ResponseService.SetData(ListaPermisosGruposDeUsuarios.First)
                ResponseService.SetTrue()
            Else
                ResponseService.SetFalse()
            End If
            Return ResponseService
        End Function

        Public Function PermisoGrupoDeUsuariosExiste(PermisoGrupoDeUsuariosId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaPermisosGruposDeUsuarios As List(Of DAO_PermisoGrupoDeUsuarios)
            ListaPermisosGruposDeUsuarios = ContextDB.PermisosGruposDeUsuarios.Where(Function(n) n.Id = PermisoGrupoDeUsuariosId).ToList
            If ListaPermisosGruposDeUsuarios.Count = 1 Then
                ResponseService.SetData(ListaPermisosGruposDeUsuarios.First)
                ResponseService.SetTrue()
            Else
                ResponseService.SetFalse()
            End If
            Return ResponseService
        End Function

        Public Function PermisoGrupoDeUsuariosNoExiste(PermisoId As Int64, GrupoDeUsuariosId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService.SetResultsByBooleanValue(PermisoGrupoDeUsuariosExiste(PermisoId, GrupoDeUsuariosId).IsFalse)
            Return ResponseService
        End Function

        Public Function PermisoGrupoDeUsuariosNoExiste(PermisoGrupoDeUsuariosId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService.SetResultsByBooleanValue(PermisoGrupoDeUsuariosExiste(PermisoGrupoDeUsuariosId).IsFalse)
            Return ResponseService
        End Function

        Public Function GrupoDeUsuariosPerteneceAPermiso(GrupoDeUsuariosId As Int64, PermisoId As Int64) As FW_ResponseService
            Return PermisoGrupoDeUsuariosExiste(PermisoId, GrupoDeUsuariosId)
        End Function

        Public Function PermisoPerteneceAGrupoDeUsuarios(PermisoId As Int64, GrupoDeUsuariosId As Int64) As FW_ResponseService
            Return PermisoGrupoDeUsuariosExiste(PermisoId, GrupoDeUsuariosId)
        End Function

#End Region

#Region "Listar"

        Public Function ListarPermisosDeUnGrupoDeUsuarios(GrupoDeUsuariosId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaPermisos As List(Of DAO_Permiso)
            ListaPermisos = (From t_PermisoGrupoDeUsuarios In ContextDB.PermisosGruposDeUsuarios
                             Join t_Permisos In ContextDB.Permisos On t_Permisos.Id Equals t_PermisoGrupoDeUsuarios.IdPermiso
                             Where t_PermisoGrupoDeUsuarios.IdGrupoDeUsuarios = GrupoDeUsuariosId
                             Select t_Permisos).ToList
            ResponseService.SetData(ListaPermisos)
            Return ResponseService
        End Function

        Public Function ListarGruposDeUsuariosDePermiso(PermisoId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaGruposDeUsuarios As List(Of DAO_GrupoDeUsuarios)
            ListaGruposDeUsuarios = (From t_PermisoGrupoDeUsuarios In ContextDB.PermisosGruposDeUsuarios
                                     Join t_GruposDeUsuarios In ContextDB.GruposDeUsuarios On t_GruposDeUsuarios.Id Equals t_PermisoGrupoDeUsuarios.IdGrupoDeUsuarios
                                     Where t_PermisoGrupoDeUsuarios.IdPermiso = PermisoId
                                     Select t_GruposDeUsuarios).ToList
            ResponseService.SetData(ListaGruposDeUsuarios)
            Return ResponseService
        End Function

        Public Function ListarPermisosDeGrupoDeUsuarios(GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService
            Return ListarPermisosDeUnGrupoDeUsuarios(GrupoDeUsuarios.Id)
        End Function

        Public Function ListarGruposDeUsuariosDePermiso(Permiso As DAO_Permiso) As FW_ResponseService
            Return ListarGruposDeUsuariosDePermiso(Permiso.Id)
        End Function
#End Region

    End Class
End Namespace
