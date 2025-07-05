Imports System.IO
Imports System.Net
Imports System.Web.Http
'Imports System.Web.Script.Serialization
Imports Newtonsoft.Json
Imports System.Reflection
Imports System.Text
Imports FrameWork.Bases

Namespace Gestores
    Public Class Ges_PushNotification

        Public Sub New()
            Dim TipoObjeto As Type = Me.GetType
            For Each ObjProperty As PropertyInfo In TipoObjeto.GetProperties()
                ObjProperty.SetValue(Me, Nothing)
            Next
        End Sub

#Region "Atributos"
        ' Keys
        Dim RestApiKey As String
        Public Property app_id As String
        ' Segmentos
        Public Property included_segments As String()
        Public Property excluded_segments As String()
        ' Dispositivos
        Public Property include_player_ids As String()
        Public Property include_external_user_ids As String()
        Public Property include_email_tokens As String()
        Public Property include_phone_numbers As String()
        Public Property include_ios_tokens As String()
        Public Property include_wp_wns_uris As String()
        Public Property include_amazon_reg_ids As String()
        Public Property include_chrome_reg_ids As String()
        Public Property include_chrome_web_reg_ids As String()
        Public Property include_android_reg_ids As String()
        ' Idempotencia
        Public Property external_id As String
        ' Contenidos 
        Public Property headings As PushNotification_AtributoTextoIdiomas
        Public Property subtitle As PushNotification_AtributoTextoIdiomas
        Public Property template_id As String
        Public Property content_available As Boolean?
        Public Property mutable_content As Boolean?
        Public Property target_content_identifier As String
        ' Email
        Public Property email_subject As String
        Public Property email_body As String
        Public Property email_from_name As String
        Public Property email_from_address As String
        ' SMS
        Public Property name As String
        Public Property disable_email_click_tracking As Boolean?
        Public Property sms_from As String
        Public Property contents As PushNotification_AtributoTextoIdiomas
        Public Property sms_media_urls As String()
        ' Adjuntos
        Public Property data As Object
        Public Property huawei_msg_type As String
        Public Property url As String
        Public Property web_url As String
        Public Property app_url As String
        Public Property ios_attachments As Object
        Public Property big_picture As String
        Public Property huawei_big_picture As String
        Public Property adm_big_picture As String
        Public Property chrome_big_picture As String
        ' Botones
        Public Property buttons As PushNotification_AtributoButtons()
        Public Property web_buttons As PushNotification_AtributoButtons()
        Public Property ios_category As String
        Public Property icon_type As String
        ' Apariencia
        Public Property android_channel_id As String
        Public Property huawei_channel_id As String
        Public Property existing_android_channel_id As String
        Public Property huawei_existing_channel_id As String
        Public Property android_background_layout As PushNotification_AndroidBackgroundLayout
        Public Property small_icon As String
        Public Property huawei_small_icon As String
        Public Property large_icon As String
        Public Property huawei_large_icon As String
        Public Property adm_small_icon As String
        Public Property adm_large_icon As String
        Public Property chrome_web_icon As String
        Public Property chrome_web_image As String
        Public Property chrome_web_badge As String
        Public Property firefox_icon As String
        Public Property chrome_icon As String
        Public Property ios_sound As String
        Public Property android_sound As String
        Public Property huawei_sound As String
        Public Property adm_sound As String
        Public Property wp_wns_sound As String
        Public Property android_led_color As String
        Public Property huawei_led_color As String
        Public Property android_accent_color As String
        Public Property huawei_accent_color As String
        Public Property android_visibility As Integer?
        Public Property huawei_visibility As Integer?
        Public Property ios_badgeType As String
        Public Property ios_badgeCount As String
        Public Property collapse_id As String
        Public Property web_push_topic As String
        Public Property apns_alert As Object
        ' Delivery
        Public Property send_after As String
        Public Property delayed_option As String
        Public Property delivery_time_of_day As String
        Public Property ttl As Integer?
        Public Property priority As Integer?
        Public Property apns_push_type_override As String
        Public Property throttle_rate_per_minute As Integer?
        ' Agrupamiento y colapsamiento
        Public Property android_group As String
        Public Property android_group_message As PushNotification_AtributoTextoIdiomas
        Public Property adm_group As String
        Public Property adm_group_message As PushNotification_AtributoTextoIdiomas
        Public Property thread_id As String
        Public Property summary_arg As String
        Public Property summary_arg_count As Decimal?
        Public Property ios_relevance_score As Decimal?
        Public Property ios_interruption_level As String
        ' Plataformas
        Public Property isIos As Boolean?
        Public Property isAndroid As Boolean?
        Public Property isHuawei As Boolean?
        Public Property isAnyWeb As Boolean?
        Public Property isChromeWeb As Boolean?
        Public Property isFirefox As Boolean?
        Public Property isSafari As Boolean?
        Public Property isWP_WNS As Boolean?
        Public Property isAdm As Boolean?
        Public Property isChrome As Boolean?
        Public Property channel_for_external_user_ids As String
        ' Filtros
        Public Property filters As PushNotification_Filters()

#End Region

#Region "Atributos clases"
        Public Class PushNotification_AtributoTextoIdiomas
            Public Property en As String
            Public Property es As String
        End Class

        Public Class PushNotification_AtributoIOSAttachment
            Public Property id1 As String
        End Class

        Public Class PushNotification_AtributoButtons
            Public Property id As String
            Public Property text As String
            Public Property icon As String
            Public Property url As String
        End Class

        Public Class PushNotification_AndroidBackgroundLayout
            Property image As String
            Property headings_color As String
            Property contents_color As String
        End Class

        Public Class PushNotification_Filters
            Public Property field As String
            Public Property key As String
            Public Property relation As String
            Public Property value As String
            Public Property [operator] As String
        End Class
#End Region

#Region "Servicios"

        Public Class PushNotificationBaseServicio
            Inherits FW_BaseGestor
            Public Sub New(Transaction As FW_BaseTransaction)
                MyBase.New(Transaction)
                Me.Gestor = New Ges_PushNotification
            End Sub

            Public Sub New(Context As FW_BaseDBContext)
                MyBase.New(Context)
                Me.Gestor = New Ges_PushNotification
            End Sub

            Public Property Gestor As Ges_PushNotification


            Public Property atributo_app_id
                Get
                    Return Gestor.app_id
                End Get
                Set(value)
                    Gestor.app_id = value
                End Set
            End Property

            Public Function Enviar() As String
                Return Gestor.Enviar()
            End Function
            Public Sub SetRestApiKey(Key As String)
                Gestor.SetRestApiKey(Key)
            End Sub
        End Class

        Public Class PushNotificationServiceToWeb
            Inherits PushNotificationBaseServicio
            Public Sub New(Transaction As FW_BaseTransaction)
                MyBase.New(Transaction)
                Me.Gestor = New Ges_PushNotification
            End Sub

            Public Sub New(Context As FW_BaseDBContext)
                MyBase.New(Context)
                Me.Gestor = New Ges_PushNotification
            End Sub
            Public Sub atributo_contenido(Español As String, Optional Ingles As String = Nothing)
                Gestor.contents = New Ges_PushNotification.PushNotification_AtributoTextoIdiomas With {.en = Ingles, .es = Español}
            End Sub

            Public Sub atributo_titulo(Español As String, Optional Ingles As String = Nothing)
                Gestor.headings = New Ges_PushNotification.PushNotification_AtributoTextoIdiomas With {.en = Ingles, .es = Español}
            End Sub

            Public Property atributo_usuarios As String()
                Get
                    Return Gestor.include_player_ids
                End Get
                Set(value As String())
                    Gestor.include_player_ids = value
                End Set
            End Property

            Public Property atributo_urlLink() As String
                Get
                    Return Gestor.url
                End Get
                Set(value As String)
                    Gestor.url = value
                End Set
            End Property

            Public Property atributo_urlImagen() As String
                Get
                    Return Gestor.chrome_web_image
                End Get
                Set(value As String)
                    Gestor.chrome_web_image = value
                End Set
            End Property
        End Class

        Public Class PushNotificationServiceToSMS
            Inherits PushNotificationBaseServicio
            Public Sub New(Transaction As FW_BaseTransaction)
                MyBase.New(Transaction)
            End Sub

            Public Sub New(ContextDB As FW_BaseDBContext)
                MyBase.New(ContextDB)
            End Sub
            Public Property atributo_nombre
                Get
                    Return Gestor.name
                End Get
                Set(value)
                    Gestor.name = value
                End Set
            End Property

            Public Property atributo_desde
                Get
                    Return Gestor.sms_from
                End Get
                Set(value)
                    Gestor.sms_from = value
                End Set
            End Property

            Public Property atributo_mensaje
                Get
                    Return Gestor.contents
                End Get
                Set(value)
                    Gestor.contents = value
                End Set
            End Property
            Public Property atributo_urlMensaje
                Get
                    Return Gestor.sms_media_urls
                End Get
                Set(value)
                    Gestor.sms_media_urls = value
                End Set
            End Property
            Public Property atributo_nrosTelefonicos
                Get
                    Return Gestor.include_phone_numbers
                End Get
                Set(value)
                    Gestor.include_phone_numbers = value
                End Set
            End Property
        End Class

#End Region

#Region "Metodos"
        Public Sub SetPlayerId(UserID As Int64, PlayerID As String)

        End Sub

        Public Sub SetRestApiKey(Key As String)
            RestApiKey = Key
        End Sub

        Public Function Enviar() As String
            Dim WebServices As New Gestores.Ges_WebServices()
            WebServices.Url = "https://onesignal.com/api/v1/notifications"
            WebServices.SetBasicAuthorizationInHeader(RestApiKey)
            WebServices.SetBody(ObjectBody:=Me, IgnoreNull:=True)
            WebServices.ExecutePost()
            Return WebServices.ResponseContentString
        End Function

#End Region

#Region "Documentacion (NUNCA BORRAR)"
        ' ¡¡¡NO BORRAR!!! Script para el frontend :
        'Documentacion: https://documentation.onesignal.com/reference/create-notification
        ' Javascript:
        '<!--<script src="../../Scripts/Push Notification/OneSignalSDK.js"></script>--> <script src="Scripts/jquery-3.4.1.js"></script> <script src="https://cdn.onesignal.com/sdks/OneSignalSDK.js" async=""></script> <!--<script src="../../Scripts/Push Notification/PushNotification.js"></script>--> <div class="jumbotron"> <input type="hidden" id="hdnPushNotification_player_id" value="" /> <input type="hidden" id="hdnPushNotification_app_id" value="839937fa-4816-4b65-a2fb-5cbf2498be78" /> </div> <script> // pasar a un javascript var push_notification_app_id = ""; //var ZambaWebRestApiURL = "https://localhost:44390/"; var ZambaWebRestApiURL = "https://localhost:44366/WebApi/"; function getUserIdExtended() { OneSignal.push(function () { OneSignal.on('subscriptionChange', function (isSubscribed) { if (isSubscribed) { OneSignal.getUserId(function (userId) { }); } }); }); } function getUserID() { OneSignal.getUserId(function (userId) { var valoractual = $("#hdnPushNotification_player_id").val(); if (userId != valoractual) { SetPlayerID(26, userId); $("#hdnPushNotification_player_id").val(userId); } }); } function importarScript(nombre) { var s = document.createElement("script"); s.src = nombre; document.querySelector("head").appendChild(s); } function registrarEnNotificaciones() { var OneSignal = window.OneSignal || []; OneSignal.push(function () { OneSignal.init({ appId: push_notification_app_id }); }); } function SetPlayerID(userid, playerid) { var serviceBasePushNotification = ZambaWebRestApiURL + "PushNotification/"; var data = { Values: { player_id: playerid, user_id: userid } }; var ret; $.ajax({ url: serviceBasePushNotification + "SetPlayerId", type: "POST", data: JSON.stringify(data), //data: data, dataType: 'json', contentType: "application/json; charset=utf-8", async: false, success: function (response) { ret = response; }, error: function (error) { ret = error } }); return ret; } window.onload = function () { push_notification_app_id = $("#hdnPushNotification_app_id").val() if (push_notification_app_id == "") return; registrarEnNotificaciones(); getUserID(); }; </script>
#End Region
    End Class
End Namespace
