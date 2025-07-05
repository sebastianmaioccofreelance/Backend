Imports System.ComponentModel.DataAnnotations

Namespace Modulos
    Module ModelValidations
        Public Class FechaMayorQueHoy
            Inherits ValidationAttribute
            Public Overrides Function IsValid(ByVal value As Object) As Boolean
                Dim dateTime As DateTime = Convert.ToDateTime(value)
                Return dateTime > DateTime.Now
            End Function
        End Class
        Public Class EnteroPositivo
            Inherits ValidationAttribute
            Public Overrides Function IsValid(ByVal value As Object) As Boolean
                Dim Valor As Int64 = Convert.ToInt64(value)
                Return Valor >= 0
            End Function
        End Class
    End Module
End Namespace
