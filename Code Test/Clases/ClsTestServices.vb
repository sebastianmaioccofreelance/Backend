Imports System.Collections.Generic
Imports System.Linq
Imports FrameWork
Imports FrameWork.Modulos
Imports FrameWork.Bases
Imports FrameWork.Bases.DTORequests
Imports FrameWork.Bases.DTOResponses
Namespace Testing
    Public Class ClsTestServices
        Public Property WebServices As New FrameWork.Gestores.Ges_WebServices
        Public Property Mapeos As New ClsMapeos()
        Public Property Sesion As New ClsSesion

        Public Sub MostrarJson(JSON As String)
            Dim Frm As New Frm_JsonViewer
            Frm.MostrarJson(JSON)
        End Sub
        Public Sub MostrarJson(JSON As Object)
            Dim Frm As New Frm_JsonViewer
            Frm.MostrarJson(JSON)
        End Sub
        Public Sub MostrarSQLGrilla(SQL As String)
            Dim tablas As New Frm_VisorTablas
            tablas.MostrarConsulta(SQL)
        End Sub
        Public Sub MostrarSQLGrilla(SQL As List(Of String))
            Dim tablas As New Frm_VisorTablas
            tablas.MostrarGillas(SQL)
            WebServices.ExecutePost()
        End Sub
        Public Sub MostrarJsonDesdeSQL(StrSql As String)
            Dim f As New Frm_SQLToJSon
            f.MostrarJson(StrSql)
            f.ShowDialog()
        End Sub
    End Class

End Namespace
