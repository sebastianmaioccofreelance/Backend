Imports BusinessLogic.ClasesBases.BL_BaseTransaction
Imports FrameWork.Bases
Imports FrameWork.Security
Imports System.Data.Entity
Imports System.Net

Namespace ClasesBases.Business
    Public Class BL_ListaBusinessLogic
        Property Test As Business_Test
        Property Login As Business_Login
        Sub New(Transaction As BL_BaseTransaction)
            Test = New Business_Test(Transaction)
            Login = New Business_Login(Transaction)
        End Sub
    End Class

    Public Class BL_BaseBusiness
        Inherits FrameWork.Bases.FW_BaseBusiness
        Property Gestores As BL_BaseGestores

        Public Sub New(Transaction As FW_BaseTransaction)
            MyBase.New(Transaction)
            Me.Transaction = Transaction
            Me.Utilidades = New BL_BaseUtilidades
            Gestores = New BL_BaseGestores(Transaction)
        End Sub
        ReadOnly Property DataBase As BL_BaseDBContext
            Get
                Return Transaction.ContextDB
            End Get
        End Property
    End Class
End Namespace
