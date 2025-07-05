Imports System.Collections.Generic
Imports System.Data
Imports System.Data.Entity.Infrastructure
Imports System.Data.SqlClient
Imports FrameWork.Gestores
Imports FrameWork.Modulos
Imports System.Linq
Imports FrameWork.Bases

Namespace Gestores
    Public Class Ges_Queries
        Inherits FW_BaseGestor
        Public Sub New(ContextDB As Bases.FW_BaseDBContext)
            MyBase.New(ContextDB, InstanceGestorQueries:=False)
            Me.ContextDB = ContextDB
            CargarQueries()
        End Sub

        Public Sub New(Transaction As Bases.FW_BaseTransaction)
            MyBase.New(Transaction)
            Me.ContextDB = Transaction.ContextDB
            CargarQueries()
        End Sub

        Dim Queries As List(Of ClsQuery)
        Dim Parametros As New List(Of SqlParameter)

        Public Sub Clear()
            Parametros.Clear()
        End Sub

        Public Sub AddParameter(Name As String, Value As String)
            AddParameter(Name, Value, SqlDbType.VarChar, 8000)
        End Sub
        Public Sub AddParameter(Name As String, Value As Int64)
            AddParameter(Name, Value, SqlDbType.BigInt)
        End Sub

        Public Sub AddParameter(Name As String, Value As Int16)
            AddParameter(Name, Value, SqlDbType.SmallInt)
        End Sub

        Public Sub AddParameter(Name As String, Value As Int32)
            AddParameter(Name, Value, SqlDbType.Int)
        End Sub

        Public Sub AddParameter(Name As String, Value As DateTime)
            AddParameter(Name, Value, SqlDbType.DateTime)
        End Sub

        Public Sub AddParameter(Name As String, Value As Decimal)
            AddParameter(Name, Value, SqlDbType.Decimal, 20, 20)
        End Sub
        Public Sub AddParameter(Name As String, Value As Double)
            AddParameter(Name, Value, SqlDbType.Float, 20, 20)
        End Sub
        Public Sub AddParameter(Name As String, Value As Byte())
            AddParameter(Name, Value, SqlDbType.VarBinary, 20, 20)
        End Sub

        Public Sub AddParameter(Name As String, Value As Boolean)
            AddParameter(Name, Value, SqlDbType.Bit)
        End Sub

        Private Sub AddParameter(Name As String, Value As Object, Tipo As SqlDbType, Optional Size As Int64 = 0, Optional Precision As Int64 = 0)
            Dim Parametro As New SqlParameter()
            Parametro.ParameterName = Name
            Parametro.Value = Value
            Parametro.SqlDbType = Tipo
            If Precision > 0 Then
                Parametro.Precision = Precision
            End If
            If Size > 0 Then
                Parametro.Size = Size
            End If
            Parametros.Add(Parametro)
        End Sub



        Public Sub CargarQueries()
            Dim JsonQueries As String
            Dim FileName As String
            If Not AppDomain.CurrentDomain.SetupInformation.PrivateBinPath Is Nothing Then
                FileName = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath + "\5.Queries\Queries.enc"
            ElseIf Not AppDomain.CurrentDomain.SetupInformation.ApplicationBase Is Nothing Then
                FileName = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\5.Queries\Queries.enc"
            End If
            JsonQueries = (New Ges_Files(FileName, Enum_FileAccess.Read)).Content
            Queries = JsonQueries.JsonToObject(Of List(Of ClsQuery))
        End Sub

        Sub Execute(QueryName As String)
            Dim StrQuery As String = GetScriptQuery(QueryName)
            If Parametros.Count = 0 Then
                ContextDB.Database.ExecuteSqlCommand(StrQuery)
            Else
                ContextDB.Database.ExecuteSqlCommand(StrQuery, Parametros)
            End If
        End Sub

        Function QueryFromFile(QueryName As String) As DataTable
            Dim FileContent As String = GetScriptQuery(QueryName)
            Return Query(FileContent)
        End Function

        Function Query(SQLQuery) As DataTable
            Dim Conneccion As SqlConnection = ContextDB.Database.Connection
            Dim adapter As SqlDataAdapter
            Dim ds As DataSet = New DataSet()
            Dim dt As DataTable
            Conneccion.Open()
            adapter = New SqlDataAdapter(SQLQuery, Conneccion)
            adapter.SelectCommand.Parameters.AddRange(Parametros.ToArray)
            adapter.Fill(ds)
            dt = ds.Tables(0)
            Conneccion.Close()
            Return dt
        End Function

        Function Query(Of t)(QueryName As String) As List(Of t)
            Dim StrQuery As String = GetScriptQuery(QueryName)
            Dim ret As List(Of t)
            Dim Consulta As DbRawSqlQuery(Of t)
            If Parametros.Count = 0 Then
                Consulta = ContextDB.Database.SqlQuery(Of t)(StrQuery)
            Else
                Consulta = ContextDB.Database.SqlQuery(Of t)(StrQuery, Parametros.ToArray)
            End If
            ret = Consulta.ToList()
            Return ret
        End Function

        Private Function GetScriptQuery(Name As String) As String
            Return Queries.Where(Function(n) n.Name = Name + ".sql").First.Query
        End Function

        Private Class ClsQuery
            Property Name As String
            Property Query As String
        End Class
    End Class
End Namespace
