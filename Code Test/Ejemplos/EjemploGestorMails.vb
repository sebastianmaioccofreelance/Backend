Imports System.Net
Imports System.Net.Mail
Namespace Ejemplos
    Module EjemploGestorMails
        Public Sub TestMails()
            Dim Mail As New FrameWork.Gestores.Ges_Mails("smtp.gmail.com", 587, "naiosoft.compania@gmail.com", "Korgm3expanded")
            Mail.SetFrom("naiosoft.compania@gmail.com", "Sebastian Maiocco Compania")
            Mail.EnviarCorreo("naiosoft@hotmail.com", "hola como estas?", "Mail de prueba")
            Dim client As SmtpClient = New SmtpClient("smtp.gmail.com", 587)
            client.Credentials = New NetworkCredential("naiosoft.compania@gmail.com", "Korgm3expanded")
            Dim mailMessage As MailMessage = New MailMessage()
            mailMessage.From = New MailAddress("naiosoft.compania@gmail.com")
            mailMessage.[To].Add("naiosoft@hotmail.com")
            mailMessage.Body = "Esto es un mail de prueba"
            mailMessage.Subject = "Titulo"
            client.EnableSsl = True
            client.DeliveryMethod = SmtpDeliveryMethod.Network
            mailMessage.IsBodyHtml = False
            client.Send(mailMessage)
        End Sub
    End Module
End Namespace

