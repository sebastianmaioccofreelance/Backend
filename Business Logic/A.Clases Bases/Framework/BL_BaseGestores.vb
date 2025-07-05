Imports FrameWork.Bases
Imports FrameWork.Security
Imports System.Data.Entity
Imports System.Net
Imports FrameWork.Gestores

Namespace ClasesBases
    Public Class BL_BaseGestores
        Inherits FW_BaseGestores
        Sub New(Transaction As BL_BaseTransaction)
            MyBase.New(Transaction)
        End Sub
        ReadOnly Property DataBase As BL_BaseDBContext
            Get
                Return Transaction.ContextDB
            End Get
        End Property
    End Class
End Namespace