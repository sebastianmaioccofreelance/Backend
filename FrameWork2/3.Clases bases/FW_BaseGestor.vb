Imports System.Collections.Generic
Imports System.Data.Entity
Imports FrameWork.Security
Imports FrameWork.DAO
Imports FrameWork.Modulos
Imports FrameWork.Gestores.Extensiones
Imports FrameWork.Gestores


Namespace Bases
    Public Class FW_BaseGestor
        Public Sub New(Transaction As FW_BaseTransaction)
            Me.Transaction = Transaction
            Me.ContextDB = Transaction.ContextDB
            RunQueries = New Ges_Queries(Transaction.ContextDB)
        End Sub

        Public Sub New(Context As FW_BaseDBContext, Optional InstanceGestorQueries As Boolean = True)
            Me.ContextDB = Context
            Me.Transaction = New FW_BaseTransaction(Context)
            If InstanceGestorQueries Then
                RunQueries = New Ges_Queries(Context)
            End If
        End Sub
        Public Sub New()

        End Sub

        Property RunQueries As Ges_Queries
        Property ContextDB As FW_BaseDBContext
        Property Transaction As FrameWork.Bases.FW_BaseTransaction

        Public ReadOnly Property Sesion As InfoUserSession
            Get
                Return Transaction.Sesion
            End Get
        End Property
        Public ReadOnly Property Usuario As DAO_Usuario
            Get
                Return Transaction.Sesion.Usuario
            End Get
        End Property

        Public Sub Save()
            ContextDB.SaveChanges()
        End Sub

        Function Query_Execute(Of t)(QueryName As String) As List(Of t)
            Return RunQueries.Query(Of t)(QueryName)
        End Function

        Sub Query_Execute(QueryName As String)
            RunQueries.Execute(QueryName)
        End Sub

        Sub Query_New()
            RunQueries.Clear()
        End Sub

        Sub Query_AddParameter(Name As String, Value As Object)
            RunQueries.AddParameter(Name, Value)
        End Sub
    End Class
End Namespace

