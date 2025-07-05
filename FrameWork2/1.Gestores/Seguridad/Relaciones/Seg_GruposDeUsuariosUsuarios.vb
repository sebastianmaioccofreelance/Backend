



Imports FrameWork.Bases

Imports FrameWork.DAO
Imports System.Linq
Imports System.Collections.Generic

Namespace Gestores
    Partial Public Class Ges_Seguridad

#Region "1 a 1"

#Region "Asignar (1 a 1)"

        Public Function AsignarGrupoDeUsuariosAUsuario(GrupoDeUsuariosId As Int64, UsuarioId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            If GrupoDeUsuariosUsuarioNoExiste(GrupoDeUsuariosId, UsuarioId).IsTrue Then
                Dim NuevoGrupoDeUsuariosUsuario As New DAO_UsuarioGrupoDeUsuarios
                NuevoGrupoDeUsuariosUsuario.IdGrupoDeUsuarios = GrupoDeUsuariosId
                NuevoGrupoDeUsuariosUsuario.IdUsuario = UsuarioId
                ContextDB.GruposDeUsuariosUsuarios.Add(NuevoGrupoDeUsuariosUsuario)
                Save()
            End If
            Return ResponseService
        End Function

        Public Function GuardarGrupoDeUsuariosUsuario(GrupoDeUsuariosUsuario As DAO_UsuarioGrupoDeUsuarios) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            If GrupoDeUsuariosUsuarioNoExiste(GrupoDeUsuariosUsuario.IdGrupoDeUsuarios, GrupoDeUsuariosUsuario.IdUsuario).IsTrue Then
                ContextDB.GruposDeUsuariosUsuarios.Add(GrupoDeUsuariosUsuario)
            End If
            Save()
            Return ResponseService
        End Function


        Public Function AsignarGrupoDeUsuariosAUsuario(GrupoDeUsuarios As DAO_GrupoDeUsuarios, UsuarioId As Int64) As FW_ResponseService
            Return AsignarGrupoDeUsuariosAUsuario(GrupoDeUsuarios.Id, UsuarioId)
        End Function

        Public Function AsignarGrupoDeUsuariosAUsuario(GrupoDeUsuariosId As Int64, Usuario As DAO_Usuario) As FW_ResponseService
            Return AsignarGrupoDeUsuariosAUsuario(GrupoDeUsuariosId, Usuario.Id)
        End Function

        Public Function AsignarGrupoDeUsuariosAUsuario(GrupoDeUsuarios As DAO_GrupoDeUsuarios, Usuario As DAO_Usuario) As FW_ResponseService
            Return AsignarGrupoDeUsuariosAUsuario(GrupoDeUsuarios.Id, Usuario.Id)
        End Function

        Public Function AsignarUsuarioAGrupoDeUsuarios(UsuarioId As Int64, GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService
            Return AsignarGrupoDeUsuariosAUsuario(GrupoDeUsuarios.Id, UsuarioId)
        End Function

        Public Function AsignarUsuarioAGrupoDeUsuarios(Usuario As DAO_Usuario, GrupoDeUsuariosId As Int64) As FW_ResponseService
            Return AsignarGrupoDeUsuariosAUsuario(GrupoDeUsuariosId, Usuario.Id)
        End Function

        Public Function AsignarUsuarioAGrupoDeUsuarios(Usuario As DAO_Usuario, GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService
            Return AsignarGrupoDeUsuariosAUsuario(GrupoDeUsuarios.Id, Usuario.Id)
        End Function

        Public Function AsignarUsuarioAGrupoDeUsuarios(UsuarioId As Int64, GrupoDeUsuariosId As Int64) As FW_ResponseService
            Return AsignarGrupoDeUsuariosAUsuario(GrupoDeUsuariosId, UsuarioId)
        End Function

#End Region

#Region "Desasignar (1 a 1)"


        Public Function DesasignarGrupoDeUsuariosDeUsuario(GrupoDeUsuariosId As Int64, UsuarioId As Int64) As FW_ResponseService
            Dim ServiciosSeguridad As New Ges_Seguridad(Me)
            Dim ResponseService As New FW_ResponseService(Transaction)
            ResponseService = ServiciosSeguridad.GrupoDeUsuariosUsuarioExiste(GrupoDeUsuariosId, UsuarioId)
            If ResponseService.IsTrue Then
                DesasignarUsuarioDeGrupoDeUsuarios(ResponseService.Data)
            End If
            Return ResponseService
        End Function

        Public Function DesasignarGrupoDeUsuariosDeUsuario(GrupoDeUsuarios As DAO_GrupoDeUsuarios, UsuarioId As Int64) As FW_ResponseService
            Return DesasignarGrupoDeUsuariosDeUsuario(GrupoDeUsuarios.Id, UsuarioId)
        End Function

        Public Function DesasignarGrupoDeUsuariosDeUsuario(GrupoDeUsuariosId As Int64, Usuario As DAO_Usuario) As FW_ResponseService
            Return DesasignarGrupoDeUsuariosDeUsuario(GrupoDeUsuariosId, Usuario.Id)
        End Function

        Public Function DesasignarGrupoDeUsuariosDeUsuario(GrupoDeUsuarios As DAO_GrupoDeUsuarios, Usuario As DAO_Usuario) As FW_ResponseService
            Return DesasignarGrupoDeUsuariosDeUsuario(GrupoDeUsuarios.Id, Usuario.Id)
        End Function



        Public Function DesasignarUsuarioDeGrupoDeUsuarios(UsuarioId As Int64, GrupoDeUsuariosId As Int64) As FW_ResponseService
            Return DesasignarGrupoDeUsuariosDeUsuario(GrupoDeUsuariosId, UsuarioId)
        End Function

        Public Function DesasignarUsuarioDeGrupoDeUsuarios(Usuario As DAO_Usuario, GrupoDeUsuariosId As Int64) As FW_ResponseService
            Return DesasignarGrupoDeUsuariosDeUsuario(GrupoDeUsuariosId, Usuario.Id)
        End Function

        Public Function DesasignarUsuarioDeGrupoDeUsuarios(UsuarioId As Int64, GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService
            Return DesasignarGrupoDeUsuariosDeUsuario(GrupoDeUsuarios.Id, UsuarioId)
        End Function

        Public Function DesasignarUsuarioDeGrupoDeUsuarios(Usuario As DAO_Usuario, GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService
            Return DesasignarGrupoDeUsuariosDeUsuario(GrupoDeUsuarios.Id, Usuario.Id)
        End Function



        Public Function DesasignarGrupoDeUsuariosDeUsuario(GrupoDeUsuariosUsuario As DAO_UsuarioGrupoDeUsuarios) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService = ObtenerGrupoDeUsuariosUsuario(GrupoDeUsuariosUsuario.Id)
            If ResponseService.IsTrue Then
                ContextDB.GruposDeUsuariosUsuarios.Remove(ResponseService.Data)
                Save()
            End If
            Return ResponseService
        End Function

        Public Function DesasignarUsuarioDeGrupoDeUsuarios(GrupoDeUsuariosUsuario As DAO_UsuarioGrupoDeUsuarios) As FW_ResponseService
            Return DesasignarGrupoDeUsuariosDeUsuario(GrupoDeUsuariosUsuario)
        End Function

        Public Function EliminarGrupoDeUsuariosUsuario(GrupoDeUsuariosUsuario As DAO_UsuarioGrupoDeUsuarios) As FW_ResponseService
            Return DesasignarGrupoDeUsuariosDeUsuario(GrupoDeUsuariosUsuario)
        End Function

        Public Function DesasignarUsuarioDeGrupoDeUsuarios(GrupoDeUsuariosUsuariosId As Int64) As FW_ResponseService
            Dim ServiciosSeguridad As New Ges_Seguridad(Me)
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService = ServiciosSeguridad.GrupoDeUsuariosUsuarioExiste(GrupoDeUsuariosUsuariosId)
            If ResponseService.IsTrue Then
                DesasignarUsuarioDeGrupoDeUsuarios(ResponseService.Data)
            End If
            Return ResponseService
        End Function

        Public Function DesasignarGrupoDeUsuariosDeUsuario(GrupoDeUsuariosUsuariosId As Int64) As FW_ResponseService
            Return DesasignarUsuarioDeGrupoDeUsuarios(GrupoDeUsuariosUsuariosId)
        End Function

#End Region

#End Region

#Region "1 a muchos"

#Region "Asignar (1 a muchos)"


        Public Function GuardarGruposDeUsuariosUsuarios(GruposDeUsuariosUsuarios() As DAO_UsuarioGrupoDeUsuarios) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each GrupoDeUsuariosUsuario As DAO_UsuarioGrupoDeUsuarios In GruposDeUsuariosUsuarios
                GuardarGrupoDeUsuariosUsuario(GrupoDeUsuariosUsuario)
            Next
            Return ResponseService
        End Function

        Public Function AsignarGruposDeUsuariosAUsuario(GrupoDeUsuariosIds() As Int64, UsuarioId As Int64) As FW_ResponseService '1

            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdGrupoDeUsuarios As Int64 In GrupoDeUsuariosIds
                AsignarGrupoDeUsuariosAUsuario(IdGrupoDeUsuarios, UsuarioId)
            Next
            Return ResponseService
        End Function

        Public Function AsignarGruposDeUsuariosAUsuario(GrupoDeUsuariosIds() As Int64, Usuario As DAO_Usuario) As FW_ResponseService '3
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdGrupoDeUsuarios As Int64 In GrupoDeUsuariosIds
                AsignarGrupoDeUsuariosAUsuario(IdGrupoDeUsuarios, Usuario.Id)
            Next
            Return ResponseService
        End Function

        Public Function AsignarGruposDeUsuariosAUsuario(GruposDeUsuarios() As DAO_GrupoDeUsuarios, UsuarioId As Int64) As FW_ResponseService '5
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each GrupoDeUsuarios As DAO_GrupoDeUsuarios In GruposDeUsuarios
                AsignarGrupoDeUsuariosAUsuario(GrupoDeUsuarios.Id, UsuarioId)
            Next
            Return ResponseService
        End Function

        Public Function AsignarGruposDeUsuariosAUsuario(GruposDeUsuarios() As DAO_GrupoDeUsuarios, Usuario As DAO_Usuario) As FW_ResponseService '7
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each GrupoDeUsuarios As DAO_GrupoDeUsuarios In GruposDeUsuarios
                AsignarGrupoDeUsuariosAUsuario(GrupoDeUsuarios.Id, Usuario.Id)
            Next
            Return ResponseService
        End Function



        Public Function AsignarUsuariosAGrupoDeUsuarios(UsuariosIds() As Int64, GrupoDeUsuariosId As Int64) As FW_ResponseService '2
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdUsuario As Int64 In UsuariosIds
                AsignarGrupoDeUsuariosAUsuario(GrupoDeUsuariosId, IdUsuario)
            Next
            Return ResponseService
        End Function

        Public Function AsignarUsuariosAGrupoDeUsuarios(Usuarios() As DAO_Usuario, GrupoDeUsuariosId As Int64) As FW_ResponseService '4
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Usuario As DAO_Usuario In Usuarios
                AsignarGrupoDeUsuariosAUsuario(GrupoDeUsuariosId, Usuario.Id)
            Next
            Return ResponseService
        End Function

        Public Function AsignarUsuariosAGrupoDeUsuarios(UsuariosId() As Int64, GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService '6
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each UsuarioId As Int64 In UsuariosId
                AsignarGrupoDeUsuariosAUsuario(GrupoDeUsuarios.Id, UsuarioId)
            Next
            Return ResponseService
        End Function

        Public Function AsignarUsuariosAGrupoDeUsuarios(Usuarios() As DAO_Usuario, GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService '8
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Usuario As DAO_Usuario In Usuarios
                AsignarGrupoDeUsuariosAUsuario(GrupoDeUsuarios.Id, Usuario.Id)
            Next
            Return ResponseService
        End Function

#End Region

#Region "Desasignar ( 1 a muchos)"



        Public Function DesasignarGruposDeUsuariosDeUsuario(GrupoDeUsuariosIds() As Int64, UsuarioId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdGrupoDeUsuarios As Int64 In GrupoDeUsuariosIds
                DesasignarGrupoDeUsuariosDeUsuario(IdGrupoDeUsuarios, UsuarioId)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarGruposDeUsuariosDeUsuario(GrupoDeUsuariosIds() As Int64, Usuario As DAO_Usuario) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdGrupoDeUsuarios As Int64 In GrupoDeUsuariosIds
                DesasignarGrupoDeUsuariosDeUsuario(IdGrupoDeUsuarios, Usuario.Id)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarGruposDeUsuariosDeUsuario(GruposDeUsuarios() As DAO_GrupoDeUsuarios, UsuarioId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each GrupoDeUsuarios As DAO_GrupoDeUsuarios In GruposDeUsuarios
                DesasignarGrupoDeUsuariosDeUsuario(GrupoDeUsuarios.Id, UsuarioId)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarGruposDeUsuariosDeUsuario(GruposDeUsuarios() As DAO_GrupoDeUsuarios, Usuario As DAO_Usuario) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each GrupoDeUsuarios As DAO_GrupoDeUsuarios In GruposDeUsuarios
                DesasignarGrupoDeUsuariosDeUsuario(GrupoDeUsuarios.Id, Usuario.Id)
            Next
            Return ResponseService
        End Function




        Public Function DesasignarUsuariosDeGrupoDeUsuarios(UsuariosIds() As Int64, GrupoDeUsuariosId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdUsuario As Int64 In UsuariosIds
                DesasignarGrupoDeUsuariosDeUsuario(GrupoDeUsuariosId, IdUsuario)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarUsuariosDeGrupoDeUsuarios(Usuario() As DAO_Usuario, GrupoDeUsuariosId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each GrupoGrupoDeUsuarios As DAO_Usuario In Usuario
                DesasignarGrupoDeUsuariosDeUsuario(GrupoDeUsuariosId, GrupoGrupoDeUsuarios.Id)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarUsuariosDeGrupoDeUsuarios(UsuarioIds() As Int64, GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdGrupoGrupoDeUsuarios As Int64 In UsuarioIds
                DesasignarGrupoDeUsuariosDeUsuario(GrupoDeUsuarios.Id, IdGrupoGrupoDeUsuarios)
            Next
            Return ResponseService
        End Function

        Public Function DesasignarUsuariosDeGrupoDeUsuarios(Usuarios() As DAO_Usuario, GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Usuario As DAO_Usuario In Usuarios
                DesasignarGrupoDeUsuariosDeUsuario(GrupoDeUsuarios.Id, Usuario.Id)
            Next
            Return ResponseService
        End Function

        Public Function EliminarUsuariosGruposDeUsuarios(GruposDeUsuariosUsuarios() As DAO_UsuarioGrupoDeUsuarios) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each GrupoDeUsuariosUsuario As DAO_UsuarioGrupoDeUsuarios In GruposDeUsuariosUsuarios
                EliminarGrupoDeUsuariosUsuario(GrupoDeUsuariosUsuario)
            Next
            Return ResponseService
        End Function



#End Region

#End Region

#Region "Obtener / Existe / No existe / Pertenece /  No pertenece "

        Public Function ObtenerGrupoDeUsuariosUsuario(GrupoDeUsuariosPermidoId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaGruposDeUsuariosUsuarios As List(Of DAO_UsuarioGrupoDeUsuarios)
            ListaGruposDeUsuariosUsuarios = ContextDB.GruposDeUsuariosUsuarios.Where(Function(n) n.Id = GrupoDeUsuariosPermidoId).ToList
            If ListaGruposDeUsuariosUsuarios.Count = 1 Then
                ResponseService.SetData(ListaGruposDeUsuariosUsuarios.First)
                ResponseService.SetTrue()
            Else
                ResponseService.SetFalse()
            End If
            Return ResponseService
        End Function

        Public Function GrupoDeUsuariosUsuarioExiste(GrupoDeUsuariosId As Int64, UsuarioId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaGruposDeUsuariosUsuarios As List(Of DAO_UsuarioGrupoDeUsuarios)
            ListaGruposDeUsuariosUsuarios = ContextDB.GruposDeUsuariosUsuarios.Where(Function(n) n.IdUsuario = UsuarioId And n.IdGrupoDeUsuarios = GrupoDeUsuariosId).ToList
            If ListaGruposDeUsuariosUsuarios.Count = 1 Then
                ResponseService.SetData(ListaGruposDeUsuariosUsuarios.First)
                ResponseService.SetTrue()
            Else
                ResponseService.SetFalse()
            End If
            Return ResponseService
        End Function

        Public Function GrupoDeUsuariosUsuarioExiste(GrupoDeUsuariosUsuarioId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaGruposDeUsuariosUsuarios As List(Of DAO_UsuarioGrupoDeUsuarios)
            ListaGruposDeUsuariosUsuarios = ContextDB.GruposDeUsuariosUsuarios.Where(Function(n) n.Id = GrupoDeUsuariosUsuarioId).ToList
            If ListaGruposDeUsuariosUsuarios.Count = 1 Then
                ResponseService.SetData(ListaGruposDeUsuariosUsuarios.First)
                ResponseService.SetTrue()
            Else
                ResponseService.SetFalse()
            End If
            Return ResponseService
        End Function

        Public Function GrupoDeUsuariosUsuarioNoExiste(GrupoDeUsuariosId As Int64, UsuarioId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService.SetResultsByBooleanValue(GrupoDeUsuariosUsuarioExiste(GrupoDeUsuariosId, UsuarioId).IsFalse)
            Return ResponseService
        End Function

        Public Function GrupoDeUsuariosUsuarioNoExiste(GrupoDeUsuariosUsuarioId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService.SetResultsByBooleanValue(GrupoDeUsuariosUsuarioExiste(GrupoDeUsuariosUsuarioId).IsFalse)
            Return ResponseService
        End Function

        Public Function UsuarioPerteneceAGrupoDeUsuarios(UsuarioId As Int64, GrupoDeUsuariosId As Int64) As FW_ResponseService
            Return GrupoDeUsuariosUsuarioExiste(GrupoDeUsuariosId, UsuarioId)
        End Function

        Public Function GrupoDeUsuariosPerteneceAUsuario(GrupoDeUsuariosId As Int64, UsuarioId As Int64) As FW_ResponseService
            Return GrupoDeUsuariosUsuarioExiste(GrupoDeUsuariosId, UsuarioId)
        End Function

#End Region

#Region "Listar"

        Public Function ListarGruposDeUsuariosDeUnUsuario(UsuarioId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaGruposDeUsuarios As List(Of DAO_GrupoDeUsuarios)
            ListaGruposDeUsuarios = (From t_GrupoDeUsuariosUsuario In ContextDB.GruposDeUsuariosUsuarios
                                     Join t_GruposDeUsuarios In ContextDB.GruposDeUsuarios On t_GruposDeUsuarios.Id Equals t_GrupoDeUsuariosUsuario.IdGrupoDeUsuarios
                                     Where t_GrupoDeUsuariosUsuario.IdUsuario = UsuarioId
                                     Select t_GruposDeUsuarios).ToList
            ResponseService.SetData(ListaGruposDeUsuarios)
            Return ResponseService
        End Function

        Public Function ListarUsuariosDeGrupoDeUsuarios(GrupoDeUsuariosId As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaUsuarios As List(Of DAO_Usuario)
            ListaUsuarios = (From t_GrupoDeUsuariosUsuario In ContextDB.GruposDeUsuariosUsuarios
                             Join t_Usuarios In ContextDB.Usuarios On t_Usuarios.Id Equals t_GrupoDeUsuariosUsuario.IdUsuario
                             Where t_GrupoDeUsuariosUsuario.IdGrupoDeUsuarios = GrupoDeUsuariosId
                             Select t_Usuarios).ToList
            ResponseService.SetData(ListaUsuarios)
            Return ResponseService
        End Function

        Public Function ListarGruposDeUsuariosDeUsuario(Usuario As DAO_Usuario) As FW_ResponseService
            Return ListarGruposDeUsuariosDeUnUsuario(Usuario.Id)
        End Function

        Public Function ListarUsuariosDeGrupoDeUsuarios(GrupoDeUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService
            Return ListarUsuariosDeGrupoDeUsuarios(GrupoDeUsuarios.Id)
        End Function
#End Region

    End Class
End Namespace
