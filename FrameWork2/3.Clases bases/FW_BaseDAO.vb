Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports FrameWork.Gestores
Imports FrameWork.Modulos
Namespace Bases
    Public Class FW_BaseDAO
        <EnteroPositivo(ErrorMessage:="El id debe ser >=0")>
        Property Id As Int64

        Public Sub New(Id As Int64)
            Me.Id = Id
        End Sub
        Public Sub New()

        End Sub
        <NotMapped>
        Public ReadOnly Property IsNew As Boolean
            Get
                Return Id = 0
            End Get
        End Property
    End Class
End Namespace

