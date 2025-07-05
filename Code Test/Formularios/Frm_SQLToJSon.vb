Imports System.Collections.Generic
Imports BusinessLogic
Imports FrameWork.Gestores
Imports FrameWork.Modulos
Public Class Frm_SQLToJSon
    Dim BaseDeDatos As BL_BaseDBContext
    Dim Query As Ges_Queries
    Dim Loaded As Boolean
    Private Sub Frm_Test_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not Loaded Then
            Iniciar()
        End If
    End Sub
    Private Sub Iniciar()
        Mod_Configurations.InicializarAplicacion(False)
        BaseDeDatos = New BL_BaseDBContext
        Query = New Ges_Queries(BaseDeDatos)
        Loaded = True
    End Sub
    Public Sub MostrarJson(StrSql As String)
        If Not Loaded Then
            Iniciar()
        End If
        Txt_Query.Text = StrSql
        Me.Txt_Json.Text = Query.Query(StrSql).ToJson
    End Sub

    Private Sub But_Ejecutar_Click(sender As Object, e As EventArgs) Handles But_Ejecutar.Click
        MostrarJson(Txt_Query.Text)
    End Sub

    Private Sub Txt_Json_TextChanged(sender As Object, e As EventArgs) Handles Txt_Json.TextChanged

    End Sub
End Class