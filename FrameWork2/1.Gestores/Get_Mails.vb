Imports FrameWork.Bases
Imports FrameWork.DAO
Imports System.Linq
Imports System.Collections.Generic
Imports System.Data.Entity.Migrations
Imports System.Net.Mail
Imports System.Net

Namespace Gestores
    Public Class Ges_Mails

        Public Property Client As New SmtpClient
        Public Property Credentials As New NetworkCredential
        Public Property MailMessage As MailMessage = New MailMessage()

        Public Sub New()
            '"smtp.gmail.com", 587
            Client.EnableSsl = True
            Client.DeliveryMethod = SmtpDeliveryMethod.Network
        End Sub

        Public Sub New(Cliente As String, Puerto As Int32, Username As String, Password As String)
            Me.Cliente = Cliente
            Me.Password = Password
            Me.Puerto = Puerto
            Me.Username = Username
            Me.SetFrom(Username, Username)
            Client.EnableSsl = True
            Client.DeliveryMethod = SmtpDeliveryMethod.Network
        End Sub

        Public Property Cliente As String
            Get
                Return Client.Host
            End Get
            Set(value As String)
                Client.Host = value
            End Set
        End Property

        Public Property Username As String
            Get
                Return Credentials.UserName
            End Get
            Set(value As String)
                Credentials.UserName = value
            End Set
        End Property

        Public Property Password As String
            Get
                Return Credentials.Password
            End Get
            Set(value As String)
                Credentials.Password = value
            End Set
        End Property

        Public Property Puerto As Integer
            Get
                Return Client.Port
            End Get
            Set(value As Integer)
                Client.Port = value
            End Set
        End Property

        Public Property EnableSSL As Boolean
            Get
                Return Client.EnableSsl
            End Get
            Set(value As Boolean)
                Client.EnableSsl = value
            End Set
        End Property

        Public Property IsHtml As Boolean
            Get
                Return MailMessage.IsBodyHtml
            End Get
            Set(value As Boolean)
                MailMessage.IsBodyHtml = value
            End Set
        End Property

        Public Sub EnviarCorreo([To] As String, Body As String, Subject As String)
            AddTo([To])
            Me.Body = Body
            Me.Subject = Subject
            EnviarCorreo()
        End Sub
        Public Sub EnviarCorreo([To] As String(), Body As String, Subject As String)
            AddTo([To])
            Me.Body = Body
            Me.Subject = Subject
            EnviarCorreo()
        End Sub

        Public Sub EnviarCorreo()
            Client.Credentials = Credentials
            Client.Send(MailMessage)
        End Sub

        Public Sub SetFrom(Mail As String, DisplayName As String)
            MailMessage.From = New MailAddress(Mail, DisplayName)
        End Sub
        Public Sub SetFrom(Mail As String)
            MailMessage.From = New MailAddress(Mail)
        End Sub

        Public Sub Credenciales(Username As String, Password As String)
            Me.Password = Password
            Me.Username = Username
        End Sub

        Public Sub Conectar(Cliente As String, Optional Puerto As Integer = 587)
            Client.Host = Cliente
            Client.Port = Puerto
        End Sub

        Public Property Body As String
            Get
                Return MailMessage.Body
            End Get
            Set(value As String)
                MailMessage.Body = value
            End Set
        End Property

        Public Property Subject As String
            Get
                Return MailMessage.Subject
            End Get
            Set(value As String)
                MailMessage.Subject = value
            End Set
        End Property

        Public Sub AddTo(Mails As String)
            For Each Mail As String In Mails.Split(";")
                MailMessage.To.Add(Mail)
            Next
        End Sub

        Public Sub AddTo(Mails As List(Of String))
            For Each Mail As String In Mails
                MailMessage.To.Add(Mail)
            Next
        End Sub

        Public Sub AddTo(Mails() As String)
            For Each Mail As String In Mails
                MailMessage.To.Add(Mail)
            Next
        End Sub

        Public Sub AddCC(Mails As String)
            For Each Mail As String In Mails.Split(";")
                MailMessage.CC.Add(Mail)
            Next
        End Sub

        Public Sub AddCC(Mails As List(Of String))
            For Each Mail As String In Mails
                MailMessage.CC.Add(Mail)
            Next
        End Sub

        Public Sub AddCC(Mails() As String)
            For Each Mail As String In Mails
                MailMessage.CC.Add(Mail)
            Next
        End Sub

        Public Sub AddCCo(Mails As String)
            For Each Mail As String In Mails.Split(";")
                MailMessage.Bcc.Add(Mail)
            Next
        End Sub

        Public Sub AddCCo(Mails As List(Of String))
            For Each Mail As String In Mails
                MailMessage.Bcc.Add(Mail)
            Next
        End Sub

        Public Sub AddCCo(Mails() As String)
            For Each Mail As String In Mails
                MailMessage.Bcc.Add(Mail)
            Next
        End Sub

        Public Sub AddReplyTo(Mails As String)
            For Each Mail As String In Mails.Split(";")
                MailMessage.ReplyToList.Add(Mail)
            Next
        End Sub

        Public Sub AddReplyTo(Mails As List(Of String))
            For Each Mail As String In Mails
                MailMessage.ReplyToList.Add(Mail)
            Next
        End Sub

        Public Sub AddReplyTo(Mails() As String)
            For Each Mail As String In Mails
                MailMessage.ReplyToList.Add(Mail)
            Next
        End Sub

        Public Sub AgregarArchivo(FileName As String)
            MailMessage.Attachments.Add(New Attachment(FileName))
        End Sub

        Public Sub Prioridad(Prioridad As MailPriority)
            MailMessage.Priority = Prioridad
        End Sub
    End Class
End Namespace

