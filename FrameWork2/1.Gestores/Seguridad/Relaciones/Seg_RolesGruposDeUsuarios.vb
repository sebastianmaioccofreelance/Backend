



Imports FrameWork.Bases

Imports FrameWork.DAO
Imports System.Linq
Imports System.Collections.Generic

Namespace Gestores
    Partial Public Class Ges_Seguridad

#Region "1 a 1"

#Region "Asignar (1 a 1)"



        Public Function AsignarRolAGrupoDeUsuarios(RolId As Int64, GrupoDeUsuariosId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            If RolGrupoDeUsuariosNoExiste(RolId, GrupoDeUsuariosId).IsTrue Then
                Dim NuevoRolGrupoDeUsuarios As New DAO_RolGrupoDeUsuarios
                NuevoRolGrupoDeUsuarios.IdRol = RolId
                NuevoRolGrupoDeUsuarios.IdGrupoDeUsuarios = GrupoDeUsuariosId
                ContextDB.RolesGruposDeUsuarios.Add(NuevoRolGrupoDeUsuarios)
                Save()
            End If
            Return ResponseService
        End Function

        Public Function GuardarRolGrupoDeUsuarios(RolGrupoDeUsuarios As DAO_RolGrupoDeUsuarios) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            If RolGrupoDeUsuariosNoExiste(RolGrupoDeUsuarios.IdRol, RolGrupoDeUsuarios.IdGrupoDeUsuarios).IsTrue Then
                ContextDB.RolesGruposDeUsuarios.Add(RolGrupoDeUsuarios)
            End If
            Save()
            Return ResponseService
        End Function


        Public Function AsignarRolAGrupoDeUsuarios(Rol As DAO_Rol, GrupoDeUsuariosId As Int64) As FW_ResponseService
            Return AsignarRolAGrupoDeUsuarios(Rol.Id, GrupoDeUsuariosId)
        End Function

        Public Function AsignarRolAGrupoDeUsuarios(RolId As Int64, GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService
            Return AsignarRolAGrupoDeUsuarios(RolId, GrupoDeUsuarios.Id)
        End Function

        Public Function AsignarRolAGrupoDeUsuarios(Rol As DAO_Rol, GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService
            Return AsignarRolAGrupoDeUsuarios(Rol.Id, GrupoDeUsuarios.Id)
        End Function



        Public Function AsignarGrupoDeUsuariosARol(GrupoDeUsuariosId As Int64, Rol As DAO_Rol) As FW_ResponseService
            Return AsignarRolAGrupoDeUsuarios(Rol.Id, GrupoDeUsuariosId)
        End Function

        Public Function AsignarGrupoDeUsuariosARol(GrupoDeUsuarios As DAO_GrupoDeUsuarios, RolId As Int64) As FW_ResponseService
            Return AsignarRolAGrupoDeUsuarios(RolId, GrupoDeUsuarios.Id)
        End Function

        Public Function AsignarGrupoDeUsuariosARol(GrupoDeUsuarios As DAO_GrupoDeUsuarios, Rol As DAO_Rol) As FW_ResponseService
            Return AsignarRolAGrupoDeUsuarios(Rol.Id, GrupoDeUsuarios.Id)
        End Function

        Public Function AsignarGrupoDeUsuariosARol(GrupoDeUsuariosId As Int64, RolId As Int64) As FW_ResponseService
            Return AsignarRolAGrupoDeUsuarios(RolId, GrupoDeUsuariosId)
        End Function

#End Region

#Region "Desasignar (1 a 1)"


        Public Function DesasignarRolDeGrupoDeUsuarios(RolId As Int64, GrupoDeUsuariosId As Int64) As FW_ResponseService
            Dim ServiciosSeguridad As New Ges_Seguridad(Me)
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService = ServiciosSeguridad.RolGrupoDeUsuariosExiste(RolId, GrupoDeUsuariosId)
            If ResponseService.IsTrue Then
                DesasignarGrupoDeUsuariosDeRol(ResponseService.Data)
            End If
            Return ResponseService
        End Function

        Public Function DesasignarRolDeGrupoDeUsuarios(Rol As DAO_Rol, GrupoDeUsuariosId As Int64) As FW_ResponseService
            Return DesasignarRolDeGrupoDeUsuarios(Rol.Id, GrupoDeUsuariosId)
        End Function

        Public Function DesasignarRolDeGrupoDeUsuarios(RolId As Int64, GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService
            Return DesasignarRolDeGrupoDeUsuarios(RolId, GrupoDeUsuarios.Id)
        End Function

        Public Function DesasignarRolDeGrupoDeUsuarios(Rol As DAO_Rol, GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService
            Return DesasignarRolDeGrupoDeUsuarios(Rol.Id, GrupoDeUsuarios.Id)
        End Function



        Public Function DesasignarGrupoDeUsuariosDeRol(GrupoDeUsuariosId As Int64, RolId As Int64) As FW_ResponseService
            Return DesasignarRolDeGrupoDeUsuarios(RolId, GrupoDeUsuariosId)
        End Function

        Public Function DesasignarGrupoDeUsuariosDeRol(GrupoDeUsuarios As DAO_GrupoDeUsuarios, RolId As Int64) As FW_ResponseService
            Return DesasignarRolDeGrupoDeUsuarios(RolId, GrupoDeUsuarios.Id)
        End Function

        Public Function DesasignarGrupoDeUsuariosDeRol(GrupoDeUsuariosId As Int64, Rol As DAO_Rol) As FW_ResponseService
            Return DesasignarRolDeGrupoDeUsuarios(Rol.Id, GrupoDeUsuariosId)
        End Function

        Public Function DesasignarGrupoDeUsuariosDeRol(GrupoDeUsuarios As DAO_GrupoDeUsuarios, Rol As DAO_Rol) As FW_ResponseService
            Return DesasignarRolDeGrupoDeUsuarios(Rol.Id, GrupoDeUsuarios.Id)
        End Function



        Public Function DesasignarRolDeGrupoDeUsuarios(RolGrupoDeUsuarios As DAO_RolGrupoDeUsuarios) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService = ObtenerRolGrupoDeUsuarios(RolGrupoDeUsuarios.Id)
            If ResponseService.IsTrue Then
                ContextDB.RolesGruposDeUsuarios.Remove(ResponseService.Data)
                Save()
            End If
            Return ResponseService
        End Function

        Public Function DesasignarGrupoDeUsuariosDeRol(RolGrupoDeUsuarios As DAO_RolGrupoDeUsuarios) As FW_ResponseService
            Return DesasignarRolDeGrupoDeUsuarios(RolGrupoDeUsuarios)
        End Function

        Public Function EliminarRolGrupoDeUsuarios(RolGrupoDeUsuarios As DAO_RolGrupoDeUsuarios) As FW_ResponseService
            Return DesasignarRolDeGrupoDeUsuarios(RolGrupoDeUsuarios)
        End Function

        Public Function DesasignarGrupoDeUsuariosDeRol(RolGruposDeUsuariosId As Int64) As FW_ResponseService
            Dim ServiciosSeguridad As New Ges_Seguridad(Me)
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService = ServiciosSeguridad.RolGrupoDeUsuariosExiste(RolGruposDeUsuariosId)
            If ResponseService.IsTrue Then
                DesasignarGrupoDeUsuariosDeRol(ResponseService.Data)
            End If
            Return ResponseService
        End Function

        Public Function DesasignarRolDeGrupoDeUsuarios(RolGruposDeUsuariosId As Int64) As FW_ResponseService
            Return DesasignarGrupoDeUsuariosDeRol(RolGruposDeUsuariosId)
        End Function

#End Region

#End Region

#Region "1 a muchos"

#Region "Asignar (1 a muchos)"


        Public Function GuardarRolesGruposDeUsuarios(RolesGruposDeUsuarios() As DAO_RolGrupoDeUsuarios) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each RolGrupoDeUsuarios As DAO_RolGrupoDeUsuarios In RolesGruposDeUsuarios
                GuardarRolGrupoDeUsuarios(RolGrupoDeUsuarios)
            Next
            Return ResponseService
        End Function

        Public Function AsignarRolesAGrupoDeUsuarios(RolIds() As Int64, GrupoDeUsuariosId As Int64) As FW_ResponseService '1
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdRol As Int64 In RolIds
                AsignarRolAGrupoDeUsuarios(IdRol, GrupoDeUsuariosId)
            Next
            Return ResponseService
        End Function

        Public Function AsignarRolesAGrupoDeUsuarios(RolIds() As Int64, GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService '3
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdRol As Int64 In RolIds
                AsignarRolAGrupoDeUsuarios(IdRol, GrupoDeUsuarios.Id)
            Next
            Return ResponseService
        End Function

        Public Function AsignarRolesAGrupoDeUsuarios(Roles() As DAO_Rol, GrupoDeUsuariosId As Int64) As FW_ResponseService '5
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Rol As DAO_Rol In Roles
                AsignarRolAGrupoDeUsuarios(Rol.Id, GrupoDeUsuariosId)
            Next
            Return ResponseService
        End Function

        Public Function AsignarRolesAGrupoDeUsuarios(Roles() As DAO_Rol, GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService '7
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Rol As DAO_Rol In Roles
                AsignarRolAGrupoDeUsuarios(Rol.Id, GrupoDeUsuarios.Id)
            Next
            Return ResponseService
        End Function



        Public Function AsignarGruposDeUsuariosARol(GruposDeUsuariosIds() As Int64, RolId As Int64) As FW_ResponseService '2
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdGrupoDeUsuarios As Int64 In GruposDeUsuariosIds
                AsignarRolAGrupoDeUsuarios(RolId, IdGrupoDeUsuarios)
            Next
            Return ResponseService
        End Function

        Public Function AsignarGruposDeUsuariosARol(GruposDeUsuarios() As DAO_GrupoDeUsuarios, RolId As Int64) As FW_ResponseService '4
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each GrupoDeUsuarios As DAO_GrupoDeUsuarios In GruposDeUsuarios
                AsignarRolAGrupoDeUsuarios(RolId, GrupoDeUsuarios.Id)
            Next
            Return ResponseService
        End Function

        Public Function AsignarGruposDeUsuariosARol(GruposDeUsuariosId() As Int64, Rol As DAO_Rol) As FW_ResponseService '6
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each GrupoDeUsuariosId As Int64 In GruposDeUsuariosId
                AsignarRolAGrupoDeUsuarios(Rol.Id, GrupoDeUsuariosId)
            Next
            Return ResponseService
        End Function

        Public Function AsignarGruposDeUsuariosARol(GruposDeUsuarios() As DAO_GrupoDeUsuarios, Rol As DAO_Rol) As FW_ResponseService '8
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each GrupoDeUsuarios As DAO_GrupoDeUsuarios In GruposDeUsuarios
                AsignarRolAGrupoDeUsuarios(Rol.Id, GrupoDeUsuarios.Id)
            Next
            Return ResponseService
        End Function

#End Region

#Region "Desasignar ( 1 a muchos)"



        Public Function DesasignarRolesDeGrupoDeUsuarios(RolIds() As Int64, GrupoDeUsuariosId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdRol As Int64 In RolIds
                DesasignarRolDeGrupoDeUsuarios(IdRol, GrupoDeUsuariosId)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarRolesDeGrupoDeUsuarios(RolIds() As Int64, GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdRol As Int64 In RolIds
                DesasignarRolDeGrupoDeUsuarios(IdRol, GrupoDeUsuarios.Id)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarRolesDeGrupoDeUsuarios(Roles() As DAO_Rol, GrupoDeUsuariosId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Rol As DAO_Rol In Roles
                DesasignarRolDeGrupoDeUsuarios(Rol.Id, GrupoDeUsuariosId)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarRolesDeGrupoDeUsuarios(Roles() As DAO_Rol, GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Rol As DAO_Rol In Roles
                DesasignarRolDeGrupoDeUsuarios(Rol.Id, GrupoDeUsuarios.Id)
            Next
            Return ResponseService
        End Function




        Public Function DesasignarGruposDeUsuariosDeRol(GruposDeUsuariosIds() As Int64, RolId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdGrupoDeUsuarios As Int64 In GruposDeUsuariosIds
                DesasignarRolDeGrupoDeUsuarios(RolId, IdGrupoDeUsuarios)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarGruposDeUsuariosDeRol(GrupoDeUsuarios() As DAO_GrupoDeUsuarios, RolId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each GrupoRol As DAO_GrupoDeUsuarios In GrupoDeUsuarios
                DesasignarRolDeGrupoDeUsuarios(RolId, GrupoRol.Id)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarGruposDeUsuariosDeRol(GrupoDeUsuariosIds() As Int64, Rol As DAO_Rol) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdGrupoRol As Int64 In GrupoDeUsuariosIds
                DesasignarRolDeGrupoDeUsuarios(Rol.Id, IdGrupoRol)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarGruposDeUsuariosDeRol(GruposDeUsuarios() As DAO_GrupoDeUsuarios, Rol As DAO_Rol) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each GrupoDeUsuarios As DAO_GrupoDeUsuarios In GruposDeUsuarios
                DesasignarRolDeGrupoDeUsuarios(Rol.Id, GrupoDeUsuarios.Id)
            Next
            Return ResponseService
        End Function

        Public Function EliminarGruposDeUsuariosRoles(RolesGruposDeUsuarios() As DAO_RolGrupoDeUsuarios) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each RolGrupoDeUsuarios As DAO_RolGrupoDeUsuarios In RolesGruposDeUsuarios
                EliminarRolGrupoDeUsuarios(RolGrupoDeUsuarios)
            Next
            Return ResponseService
        End Function



#End Region

#End Region

#Region "Obtener / Existe / No existe / Pertenece /  No pertenece "

        Public Function ObtenerRolGrupoDeUsuarios(RolPermidoId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaRolesGruposDeUsuarios As List(Of DAO_RolGrupoDeUsuarios)
            ListaRolesGruposDeUsuarios = ContextDB.RolesGruposDeUsuarios.Where(Function(n) n.Id = RolPermidoId).ToList
            If ListaRolesGruposDeUsuarios.Count = 1 Then
                ResponseService.SetData(ListaRolesGruposDeUsuarios.First)
                ResponseService.SetTrue()
            Else
                ResponseService.SetFalse()
            End If
            Return ResponseService
        End Function

        Public Function RolGrupoDeUsuariosExiste(RolId As Int64, GrupoDeUsuariosId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaRolesGruposDeUsuarios As List(Of DAO_RolGrupoDeUsuarios)
            ListaRolesGruposDeUsuarios = ContextDB.RolesGruposDeUsuarios.Where(Function(n) n.IdGrupoDeUsuarios = GrupoDeUsuariosId And n.IdRol = RolId).ToList
            If ListaRolesGruposDeUsuarios.Count = 1 Then
                ResponseService.SetData(ListaRolesGruposDeUsuarios.First)
                ResponseService.SetTrue()
            Else
                ResponseService.SetFalse()
            End If
            Return ResponseService
        End Function

        Public Function RolGrupoDeUsuariosExiste(RolGrupoDeUsuariosId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaRolesGruposDeUsuarios As List(Of DAO_RolGrupoDeUsuarios)
            ListaRolesGruposDeUsuarios = ContextDB.RolesGruposDeUsuarios.Where(Function(n) n.Id = RolGrupoDeUsuariosId).ToList
            If ListaRolesGruposDeUsuarios.Count = 1 Then
                ResponseService.SetData(ListaRolesGruposDeUsuarios.First)
                ResponseService.SetTrue()
            Else
                ResponseService.SetFalse()
            End If
            Return ResponseService
        End Function

        Public Function RolGrupoDeUsuariosNoExiste(RolId As Int64, GrupoDeUsuariosId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService.SetResultsByBooleanValue(RolGrupoDeUsuariosExiste(RolId, GrupoDeUsuariosId).IsFalse)
            Return ResponseService
        End Function

        Public Function RolGrupoDeUsuariosNoExiste(RolGrupoDeUsuariosId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService.SetResultsByBooleanValue(RolGrupoDeUsuariosExiste(RolGrupoDeUsuariosId).IsFalse)
            Return ResponseService
        End Function

        Public Function GrupoDeUsuariosPerteneceARol(GrupoDeUsuariosId As Int64, RolId As Int64) As FW_ResponseService
            Return RolGrupoDeUsuariosExiste(RolId, GrupoDeUsuariosId)
        End Function

        Public Function RolPerteneceAGrupoDeUsuarios(RolId As Int64, GrupoDeUsuariosId As Int64) As FW_ResponseService
            Return RolGrupoDeUsuariosExiste(RolId, GrupoDeUsuariosId)
        End Function

#End Region

#Region "Listar"

        Public Function ListarRolesDeUnGrupoDeUsuarios(GrupoDeUsuariosId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaRoles As List(Of DAO_Rol)
            ListaRoles = (From t_RolGrupoDeUsuarios In ContextDB.RolesGruposDeUsuarios
                          Join t_Roles In ContextDB.Roles On t_Roles.Id Equals t_RolGrupoDeUsuarios.IdRol
                          Where t_RolGrupoDeUsuarios.IdGrupoDeUsuarios = GrupoDeUsuariosId
                          Select t_Roles).ToList
            ResponseService.SetData(ListaRoles)
            Return ResponseService
        End Function

        Public Function ListarGruposDeUsuariosDeRol(RolId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaGruposDeUsuarios As List(Of DAO_GrupoDeUsuarios)
            ListaGruposDeUsuarios = (From t_RolGrupoDeUsuarios In ContextDB.RolesGruposDeUsuarios
                                     Join t_GruposDeUsuarios In ContextDB.GruposDeUsuarios On t_GruposDeUsuarios.Id Equals t_RolGrupoDeUsuarios.IdGrupoDeUsuarios
                                     Where t_RolGrupoDeUsuarios.IdRol = RolId
                                     Select t_GruposDeUsuarios).ToList
            ResponseService.SetData(ListaGruposDeUsuarios)
            Return ResponseService
        End Function

        Public Function ListarRolesDeGrupoDeUsuarios(GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService
            Return ListarRolesDeUnGrupoDeUsuarios(GrupoDeUsuarios.Id)
        End Function

        Public Function ListarGruposDeUsuariosDeRol(Rol As DAO_Rol) As FW_ResponseService
            Return ListarGruposDeUsuariosDeRol(Rol.Id)
        End Function
#End Region

    End Class
End Namespace
