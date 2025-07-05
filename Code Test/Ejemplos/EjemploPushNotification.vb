
Namespace Ejemplos
    Module EjemploPushNotification
        Public Sub PushNotification()
            'Dim EnviarMensaje As New Ges_PushNotification.PushNotificationServiceToWeb(Transaction)
            'Dim Claves As List(Of DAO_Configuration)
            'Claves = Gestores.ApplicationConfiguration.ListarCategoria("Push notification Web")
            'Dim app_id = Claves.Where(Function(n) n.Key = "app_id").First.Value
            'With EnviarMensaje
            '    .SetRestApiKey(Claves.Where(Function(n) n.Key = "api_key").First.Value)
            '    .atributo_app_id = app_id
            '    .atributo_contenido("Este es un mensaje para ti", "This is a message")
            '    .atributo_titulo("Titulo", "Title")
            '    .atributo_urlImagen = "https://cdn.autobild.es/sites/navi.axelspringer.es/public/styles/480/public/media/image/2015/08/446727-minion-gigante-que-esta-aterrorizando-conductores.jpg?itok=nnw0a0vB"
            '    .atributo_urlLink = "https://www.google.com"
            '    .atributo_usuarios = {Request.Values.Item("player_id")}
            '    Dim ResponseText As String = .Enviar()
            '    Response.Response = True
            'End With
        End Sub
    End Module

End Namespace
