Module Espaniol
    Public Class IdiomaEspaniol
        Inherits Dictionary(Of Enum_Messages, String)
        Public Sub New()
            With Me
                .Add(Enum_Messages.Void, "")
                .Add(Enum_Messages.Method_Not_Allowed, "Accion no permitida")
                .Add(Enum_Messages.Email_Exist, "El email ya existe")
                .Add(Enum_Messages.Client_Exists, "El cliente ya existe")
                .Add(Enum_Messages.FullName_Exists, "El nombre ya existe")
                .Add(Enum_Messages.Error_SaveClient, "Error al guardar el cliente")
                .Add(Enum_Messages.Error_GetCliente, "Error al obtener el cliente")
                .Add(Enum_Messages.Error_DeleteClient, "Error al eliminar el cliente")
                .Add(Enum_Messages.Error_LockUser, "Error al bloquear el usuario")
                .Add(Enum_Messages.Error_Test, "Error al testar modulo")
                .Add(Enum_Messages.Error_SetUserRol, "Error al asignar usuario a rol")
            End With
        End Sub
    End Class
End Module
