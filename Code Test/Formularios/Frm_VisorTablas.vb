Imports System.Collections.Generic
Imports BusinessLogic
Imports FrameWork.Gestores


Public Class Frm_VisorTablas
    Public Sub MostrarGillas(Queries As List(Of String))
        Mod_Configurations.InicializarAplicacion(False)
        Dim BaseDeDatos As New BL_BaseDBContext
        Dim Query As New Ges_Queries(BaseDeDatos)
        Datos_1.DataSource = Query.Query(Queries(0))
        If Queries.Count = 2 Then
            Datos_2.DataSource = Query.Query(Queries(1))
        End If
        If Queries.Count = 3 Then
            Datos_3.DataSource = Query.Query(Queries(2))
        End If
        Show()
    End Sub
    Public Sub MostrarConsulta(StrSql As String)
        Mod_Configurations.InicializarAplicacion(False)
        Dim BaseDeDatos As New BL_BaseDBContext
        Dim Query As New Ges_Queries(BaseDeDatos)
        Datos_1.DataSource = Query.Query(StrSql)
        Me.ShowDialog()
    End Sub

    Private Sub Datos_3_CellContentClick(sender As Object, e As Windows.Forms.DataGridViewCellEventArgs) Handles Datos_3.CellContentClick

    End Sub

    Private Sub Datos_2_CellContentClick(sender As Object, e As Windows.Forms.DataGridViewCellEventArgs) Handles Datos_2.CellContentClick

    End Sub

    Private Sub Datos_1_CellContentClick(sender As Object, e As Windows.Forms.DataGridViewCellEventArgs) Handles Datos_1.CellContentClick

    End Sub
End Class