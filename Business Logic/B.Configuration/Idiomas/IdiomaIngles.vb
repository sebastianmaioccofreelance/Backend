Module Ingles
    Public Class IdiomaIngles
        Inherits Dictionary(Of Enum_Messages, String)
        Public Sub New()
            With Me
                .Add(Enum_Messages.Method_Not_Allowed, "Action not allowed")
                .Add(Enum_Messages.Email_Exist, "Email yet exists")
                .Add(Enum_Messages.Client_Exists, "The client yet exists")
                .Add(Enum_Messages.FullName_Exists, "The fullname yet exists")
                .Add(Enum_Messages.Error_SaveClient, "Error while saving the client")
                .Add(Enum_Messages.Error_DeleteClient, "Error while deleting client")
            End With
        End Sub
    End Class
End Module
