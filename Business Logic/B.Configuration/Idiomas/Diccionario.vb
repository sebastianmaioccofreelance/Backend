Module Diccionario

    Public DiccionarioMensajes As Dictionary(Of Enum_Messages, String)
    Public Sub ConfigurarIdioma(Idioma As Enum_Idiomas)
        Select Case Idioma
            Case Enum_Idiomas.Espaniol
                DiccionarioMensajes = New IdiomaEspaniol
            Case Enum_Idiomas.Ingles
                DiccionarioMensajes = New IdiomaIngles
        End Select
    End Sub
    Public Enum Enum_Idiomas
        Espaniol
        Ingles
    End Enum
    Public Enum Enum_Messages
        Method_Not_Allowed
        Email_Exist
        Client_Exists
        FullName_Exists
        Error_SaveClient
        Error_GetCliente
        Error_DeleteClient
        Error_SetUserRol
        Error_LockUser
        Error_Test
        Void
    End Enum
End Module
