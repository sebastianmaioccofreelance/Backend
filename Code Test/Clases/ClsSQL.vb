Imports System.Collections.Generic
Imports BusinessLogic
Imports FrameWork.Gestores
Imports System.Data
Imports FrameWork.Modulos


Public Class ClsSQL
    Public Function QuerySQL(StrSql As String) As DataTable
        Mod_Configurations.InicializarAplicacion(False)
        Dim BaseDeDatos As New BL_BaseDBContext
        Dim Query As New Ges_Queries(BaseDeDatos)
        Return Query.Query(StrSql)
    End Function
End Class
