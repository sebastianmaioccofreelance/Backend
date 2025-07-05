Imports FrameWork.DAO
Imports System.Configuration
Imports System.Data.Entity
Imports System.Web

Public Module Sesion
    Public CurrentUser As DAO_Usuario
End Module

Public Module Mod_Configurations
    Public Sub InicializarAplicacion(Optional ReiniciarBaseDeDatos As Boolean = False)
        IniciarBaseDeDatos(ReiniciarBaseDeDatos)
        CargarAppConfig()
    End Sub
    Private Sub IniciarBaseDeDatos(Optional ReiniciarBaseDeDatos As Boolean = False)
        ConfigurarIdioma(Enum_Idiomas.Espaniol)
        'instancia el mapeo tablas de las base de datos
        Database.SetInitializer(Of BL_BaseDBContext)(Nothing)
        'crear tablas de las base de datos
        If ReiniciarBaseDeDatos Then
            Database.SetInitializer(New DropCreateDatabaseAlways(Of BL_BaseDBContext))
        End If
    End Sub

    Private Sub CargarAppConfig()
        Dim RutaEjecucion As String = ""
        If Not AppDomain.CurrentDomain.SetupInformation.PrivateBinPath Is Nothing Then
            RutaEjecucion = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath
        Else
            If Not Application.StartupPath Is Nothing Then
                RutaEjecucion = Application.StartupPath
            End If
        End If
        With AppConfig
            .Add("ConnectionString", ConfigurationManager.AppSettings("ConnectionString"))
            .Add("HashEncriptacion", ConfigurationManager.AppSettings("HashEncriptacion"))
            .Add("OverrideAuthorization", Convert.ToBoolean(ConfigurationManager.AppSettings("OverrideAuthorization")))
            .Add("SocialLoginURLCallBack", ConfigurationManager.AppSettings("SocialLoginURLCallBack"))
            .Add("RutaTraces", RutaEjecucion + "\" + ConfigurationManager.AppSettings("RutaTraces"))
            .Add("ActivarContadoresPerformance", Convert.ToBoolean(ConfigurationManager.AppSettings("ActivarContadoresPerformance")))
            .Add("RutaTemporales", ConfigurationManager.AppSettings("RutaTemporales"))
            .Add("ActivarTrace", Convert.ToBoolean(ConfigurationManager.AppSettings("ActivarTrace")))
        End With
        FrameWork.Modulos.Mod_ApplicationConfiguration.Setear()
    End Sub
    Public ReadOnly Property AppConfig As Dictionary(Of String, Object)
        Get
            Return FrameWork.Modulos.Mod_ApplicationConfiguration.AppConfig
        End Get
    End Property
End Module








