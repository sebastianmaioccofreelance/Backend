Imports System.Collections.Generic
Imports FrameWork.Security
Imports FrameWork.Gestores.ModUtilities
Imports FrameWork.Gestores
Imports System.Data.Entity.Migrations
Imports System.Xml.Linq
Imports System.Web
Imports System.Data.Entity
Imports FrameWork.Modulos
Imports System.Net
Imports FrameWork.DAO
Imports System.Linq
Imports System.Diagnostics
Imports FrameWork.Bases.DTORequests
Imports FrameWork.Bases.DTOResponses

Namespace Bases
    Public Class FW_BaseTransaction
        Property Failed As Boolean = False
        Property TransactionName As String
        Property Sesion As New InfoUserSession
        Property RequestHttp As Http.HttpRequestMessage
        Property RequestDTO As FW_BaseRequestDTO
        Property ResponseDTO As FW_BaseResponseDTO
        Property ContextDB As FW_BaseDBContext
        Property ContextDBTransaction As DbContextTransaction
        Property TransactionID As String

        Property Eventos As New List(Of DAO_TraceAccionesDetalle)
        Property ProcesoEjecucion As Process
        Property ContadorPerformanceRAM As PerformanceCounter
        Property ContadorPerformanceCPU As PerformanceCounter
        Property HoraInicio As DateTime
        Property Gestores As FW_BaseGestores
        Property HoraFin As DateTime
        Property ConsumoCPU As Single
        Property ConsumoRAMInicio As Single
        Property ConsumoRAMFin As Single
        Property Trace As Ges_Trace
        Property [Shared] As New Dictionary(Of String, Object)


        'Property Servicios As FW_BaseListServices
        Public ReadOnly Property Usuario As DAO_Usuario
            Get
                Return Sesion.Usuario
            End Get
        End Property

        Public ReadOnly Property ResponseJSON As String
            Get
                Return ResponseDTO.ToJson
            End Get
        End Property

        Public ReadOnly Property ResponseXML As XNode
            Get
                Return Utilities.ToXml(ResponseJSON)
            End Get
        End Property
        Public ReadOnly Property ActivarContadoresPerformance As Boolean
            Get
                If AppConfig.ContainsKey("ActivarContadoresPerformance") Then
                    Return Convert.ToBoolean(AppConfig("ActivarContadoresPerformance"))
                Else
                    Return False
                End If
            End Get
        End Property
        Public ReadOnly Property ActivarTrace As Boolean
            Get
                If AppConfig.ContainsKey("ActivarTrace") Then
                    Return Convert.ToBoolean(AppConfig("ActivarTrace"))
                Else
                    Return False
                End If
            End Get
        End Property
        Public ReadOnly Property OnDebugging As Boolean
            Get
                If AppConfig.ContainsKey("OnDebugging") Then
                    Return Convert.ToBoolean(AppConfig("OnDebugging"))
                Else
                    Return False
                End If
            End Get
        End Property

        Public Sub New(ContextDB As DbContext)
            Me.ContextDB = ContextDB
            Me.RequestDTO = New FW_BaseRequestVoid
        End Sub

        Public Sub New(Request As FW_BaseRequestDTO, ContextDB As DbContext, Optional RequestHttp As Http.HttpRequestMessage = Nothing)
            Me.RequestDTO = Request
            Me.RequestHttp = RequestHttp
            Me.ContextDB = ContextDB
        End Sub

        Public Sub InicializarTransaccion(ActionName As String)
            Me.Gestores = New FW_BaseGestores(Me)
            Me.TransactionName = ActionName
            Me.TransactionID = Gestores.Utilities.GenerarTransactionID()
            Me.Sesion = New InfoUserSession(ContextDB)
            If Not SesionActual.Usuario Is Nothing Then
                Me.Sesion = SesionActual
            Else
                Me.Sesion.CargarSesionYPermisos(RequestHttp)
            End If
            Me.HoraInicio = Now
            Me.ContextDBTransaction = Me.ContextDB.Database.BeginTransaction
            ResponseDTO = New FW_BaseResponseVoid
            If ActivarTrace Then
                Trace = New Ges_Trace(Me)
            End If
            If ActivarContadoresPerformance Then
                ProcesoEjecucion = Process.GetCurrentProcess()
                'ContadorPerformanceRAM = New PerformanceCounter("Process", "Working Set", ProcesoEjecucion.ProcessName)
                'ContadorPerformanceCPU = New PerformanceCounter("Process", "% Processor Time", ProcesoEjecucion.ProcessName)
                ContadorPerformanceRAM = New PerformanceCounter("Process", "Working Set", ProcesoEjecucion.ProcessName)
                ContadorPerformanceCPU = New PerformanceCounter("Process", "% Processor Time", "_Total")
                ConsumoRAMInicio = ContadorPerformanceRAM.NextValue() / (2 ^ 20)
                ContadorPerformanceCPU.NextValue()
            End If
        End Sub

        Public Sub FinalizarTransaccion(Optional GuardarEventos As Boolean = True)
            ContextDB.SaveChanges()
            If ActivarContadoresPerformance Then
                ConsumoRAMFin = ContadorPerformanceRAM.NextValue() / (2 ^ 20)
                ConsumoCPU = ContadorPerformanceCPU.NextValue
            End If
            Me.HoraFin = Now
            If ActivarTrace Then
                Trace.UploadTraceTxtToDB()
                Trace.TraceAction(Me)
            End If
            If OnDebugging Then
                ContextDBTransaction.Rollback()
            Else
                ContextDBTransaction.Commit()
            End If
            ContextDB.Database.Connection.Close()
            ContextDBTransaction.Dispose()
        End Sub

        Public Sub Evento(Mensaje As String, TipoEvento As TiposDeEvento)
            Dim NuevoEvento As New DAO_TraceAccionesDetalle
            With NuevoEvento
                .FechaInicio = Now
                .Descripcion = Mensaje
                Select Case TipoEvento
                    Case TiposDeEvento.ErrorUsuario
                        .TipoEvento = "Error del usuario"
                    Case TiposDeEvento.ErrorServicio
                        .TipoEvento = "Error de servicio"
                    Case TiposDeEvento.Etapa
                        .TipoEvento = "Etapa"
                    Case TiposDeEvento.Informacion
                        .TipoEvento = "Informacion"
                    Case TiposDeEvento.Testing
                        .TipoEvento = "Testing"
                    Case TiposDeEvento.Accion
                        .TipoEvento = "Accion"
                        Me.TransactionName = Mensaje
                End Select
            End With
            Eventos.Add(NuevoEvento)
        End Sub

        Public Sub Exception(Ex As Exception, Mensaje As String)
            'Stop
            Try
                ContextDBTransaction.Rollback()
                Evento(Mensaje, TiposDeEvento.ErrorServicio)
                If Ex.[GetType]() = GetType(ExceptionSecurity) Then
                    Me.ResponseDTO.SetWarningSecurity()
                    'Stop
                ElseIf Ex.[GetType]() = GetType(ExceptionUnsuccessfully) Then
                    Me.ResponseDTO.SetUnsuccessfully(Ex)
                    'Stop
                ElseIf Ex.[GetType]() = GetType(ExceptionUserAuthenticationAuthorization) Then
                    Me.ResponseDTO.SetUnAuthorized()
                Else
                    Me.ResponseDTO.SetSystemError()
                    'Stop
                End If
                ContextDB.Database.Connection.Close()
                ContextDBTransaction.Dispose()
                HoraFin = DateTime.Now
                Trace.TraceException(Ex)
                Trace.TraceAction(Me)
            Catch ex2 As Exception
                Trace.TraceException(ex2)
            End Try
        End Sub
    End Class

    Public Class ExceptionSecurity
        Inherits Exception
        Public Sub New(Message As String)
            MyBase.New(Message)
        End Sub
    End Class

    Public Class ExceptionUserAuthenticationAuthorization
        Inherits Exception
        Public Sub New(Message As String)
            MyBase.New(Message)
        End Sub
    End Class

    Public Class ExceptionUnsuccessfully
        Inherits Exception
        Public Sub New(Message As String)
            MyBase.New(Message)
        End Sub
    End Class

    Public Enum TiposDeEvento
        Accion
        Etapa
        Informacion
        ErrorServicio
        Testing
        ErrorUsuario
    End Enum
End Namespace


