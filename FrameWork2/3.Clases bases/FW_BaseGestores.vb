Imports FrameWork.Gestores
Imports FrameWork.Bases
Namespace Bases
    Public Class FW_BaseGestores
        Public Property ApplicationConfiguration As Ges_ApplicationConfiguration
        Public Property Excel As Ges_Excel
        Public Property Files As Ges_Files
        Public Property UserPreferences As Ges_UsuarioPreferencias
        Public Property Seguridad As Ges_Seguridad
        Public Property Queries As Ges_Queries
        Public Property Traces As Ges_Trace
        Public Property Utilities As New Ges_Utilities
        Public Property WebServices As New Ges_WebServices
        Public Property Transaction As FW_BaseTransaction
        Public Property UserCache As Ges_UserCache
        Public Property PushNotification As Ges_PushNotification
        Public Property Mails As Ges_Mails
        Public Property Idiomas As Ges_Idiomas

        Public Sub New(DBContext As FW_BaseDBContext)
            Me.Transaction = New FW_BaseTransaction(DBContext)
            ApplicationConfiguration = New Ges_ApplicationConfiguration(DBContext)
            UserPreferences = New Ges_UsuarioPreferencias(DBContext)
            UserCache = New Ges_UserCache(DBContext)
            Queries = New Ges_Queries(DBContext)
            Excel = New Ges_Excel(DBContext)
            Idiomas = New Ges_Idiomas(DBContext)
            Seguridad = New Ges_Seguridad(DBContext)
            PushNotification = New Ges_PushNotification
            Traces = New Ges_Trace()
            Mails = New Ges_Mails
        End Sub

        Public Sub New(Transaction As Bases.FW_BaseTransaction)
            ApplicationConfiguration = New Ges_ApplicationConfiguration(Transaction)
            UserPreferences = New Ges_UsuarioPreferencias(Transaction)
            Excel = New Ges_Excel(Transaction)
            Queries = New Ges_Queries(Transaction)
            Seguridad = New Ges_Seguridad(Transaction)
            Traces = New Ges_Trace(Transaction)
            UserCache = New Ges_UserCache(Transaction)
            Idiomas = New Ges_Idiomas(Transaction)
            Mails = New Ges_Mails
            PushNotification = New Ges_PushNotification
            Me.Transaction = Transaction
        End Sub
    End Class
End Namespace