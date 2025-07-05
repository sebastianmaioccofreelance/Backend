Imports FrameWork.Bases
Imports FrameWork.DAO
Imports System.Linq
Imports System.Collections.Generic
Imports System.Data.Entity.Migrations
Imports FrameWork.FW_ResponseService

Namespace Gestores
    Partial Public Class Ges_Seguridad
        Public Function ObtenerListaPermisos() As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaPermisos As List(Of DAO_Permiso)
            ListaPermisos = ContextDB.Permisos.ToList.OrderBy(Function(n) n.Nombre).ToList
            Return ResponseService.SetDataAndResults(ListaPermisos, ListaPermisos.Count)
        End Function

        Public Function ObtenerPermiso(IdPermiso As Int64) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim PermisoObtener As DAO_Permiso
            PermisoObtener = (From Permiso
                         In Me.ContextDB.Permisos
                              Where Permiso.Id = IdPermiso
                              Select Permiso
        ).FirstOrDefault
            If PermisoObtener Is Nothing Then
                ResponseService.SetResults(Enum_ResultsServices.Void)
            Else
                ResponseService.SetResults(Enum_ResultsServices.Single)
            End If
            Return ResponseService.SetData(PermisoObtener)
        End Function


        Public Function BusquedaPermisos(Buscar As String) As FW_ResponseService
            Dim ResponseService As New FW_ResponseService(Me.Transaction)
            Dim ListaPermisos As List(Of DAO_Permiso)
            ListaPermisos = ContextDB.Permisos.AsEnumerable.Where(Function(n) n.Nombre.ToLower Like "*" + Buscar.ToLower + "*").ToList
            ResponseService.SetDataAndResults(ListaPermisos, ListaPermisos.Count)
            Return ResponseService
        End Function
    End Class
End Namespace

