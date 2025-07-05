Imports FrameWork.Bases
Imports FrameWork.DAO
Imports System.Linq
Imports System.Collections.Generic
Imports System.Data.Entity.Migrations
Imports FrameWork.FW_ResponseService

Namespace Gestores
    Partial Public Class Ges_Seguridad

        Public Function ObtenerListaGruposUsuarios(Optional SoloHabilitados As Boolean = False) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaGruposUsuarios As List(Of DAO_GrupoDeUsuarios)
            If SoloHabilitados Then
                ListaGruposUsuarios = ContextDB.GruposDeUsuarios.Where(Function(n) n.Habilitado).OrderBy(Function(n) n.Descripcion).ToList
            Else
                ListaGruposUsuarios = ContextDB.GruposDeUsuarios.ToList.OrderBy(Function(n) n.Descripcion).ToList
            End If
            Return ResponseService.SetDataAndResults(ListaGruposUsuarios, ListaGruposUsuarios.Count)
        End Function

        Public Function ObtenerGrupoUsuarios(IdGrupoUsuarios As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim GrupoUsuariosObtener As DAO_GrupoDeUsuarios
            GrupoUsuariosObtener = (From GrupoUsuarios
                         In Me.ContextDB.GruposDeUsuarios
                                    Where GrupoUsuarios.Id = IdGrupoUsuarios
                                    Select GrupoUsuarios
        ).FirstOrDefault
            If GrupoUsuariosObtener Is Nothing Then
                ResponseService.SetResults(Enum_ResultsServices.Void)
            Else
                ResponseService.SetResults(Enum_ResultsServices.Single)
            End If
            Return ResponseService.SetData(GrupoUsuariosObtener)
        End Function

        Public Function GuardarGrupoUsuarios(GrupoUsuarios As DAO_GrupoDeUsuarios) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ContextDB.GruposDeUsuarios.AddOrUpdate(GrupoUsuarios)
            Save()
            Return ResponseService.SetData(GrupoUsuarios)
        End Function

        Public Function EliminarGrupoUsuarios(IdGrupoUsuarios As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim GrupoUsuariosEliminar As DAO_GrupoDeUsuarios
            GrupoUsuariosEliminar = ObtenerGrupoUsuarios(IdGrupoUsuarios).
                          Unsuccessfully(Enum_ResultsServices.Void, "No se encontro el GrupoUsuarios a eliminar").
                          Data
            ContextDB.GruposDeUsuarios.Remove(GrupoUsuariosEliminar)
            Save()
            Return ResponseService
        End Function

        Public Function EliminarGruposUsuarios(IdsGruposUsuarios() As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each IdGrupoUsuarios As Int64 In IdsGruposUsuarios
                EliminarGrupoUsuarios(IdGrupoUsuarios)
            Next
            Return ResponseService
        End Function

        Public Function HabilitarGrupoUsuarios(IdGrupoUsuarios As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim GrupoUsuariosHabilitar As DAO_GrupoDeUsuarios
            GrupoUsuariosHabilitar = ObtenerGrupoUsuarios(IdGrupoUsuarios).
                        Unsuccessfully(Enum_ResultsServices.Void, "No se encontro el GrupoUsuarios a habilitar").
                        Data
            GrupoUsuariosHabilitar.Habilitado = True
            Save()
            Return ResponseService
        End Function

        Public Function HabilitarGruposUsuarios(IdGruposUsuarios() As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Id As Int64 In IdGruposUsuarios
                HabilitarGrupoUsuarios(Id)
            Next
            Return ResponseService
        End Function

        Public Function InhabilitarGruposUsuarios(IdGruposUsuarios() As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            For Each Id As Int64 In IdGruposUsuarios
                InhabilitarGrupoUsuarios(Id)
            Next
            Save()
            Return ResponseService
        End Function

        Public Function InhabilitarGrupoUsuarios(IdGrupoUsuarios As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim GrupoUsuariosDeshabilitar As DAO_GrupoDeUsuarios
            GrupoUsuariosDeshabilitar = ObtenerGrupoUsuarios(IdGrupoUsuarios).
                        Unsuccessfully(Enum_ResultsServices.Void, "No se encontro el GrupoUsuarios a deshabilitar").
                        Data
            GrupoUsuariosDeshabilitar.Habilitado = False
            Save()
            Return ResponseService
        End Function

        Public Function GrupoUsuariosExiste(IdGrupoUsuarios As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim Result As Boolean = ContextDB.GruposDeUsuarios.Any(Function(n) n.Id = IdGrupoUsuarios)
            ResponseService.SetResultsByBooleanValue(Result)
            Return ResponseService
        End Function

        Public Function GrupoUsuariosNoExiste(IdGrupoUsuarios As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            ResponseService.SetResultsByBooleanValue(GrupoUsuariosExiste(IdGrupoUsuarios).IsFalse)
            Return ResponseService
        End Function

        Public Function BusquedaGruposUsuarios(Buscar As String) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaGruposUsuarios As List(Of DAO_GrupoDeUsuarios)
            ListaGruposUsuarios = ContextDB.GruposDeUsuarios.AsEnumerable.Where(Function(n) n.NombreGrupoDeUsuarios.ToLower Like "*" + Buscar.ToLower + "*").ToList
            ResponseService.SetDataAndResults(ListaGruposUsuarios, ListaGruposUsuarios.Count)
            Return ResponseService
        End Function
    End Class
End Namespace

