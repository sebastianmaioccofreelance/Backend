Imports FrameWork.DAO
Imports FrameWork.Bases
Imports System.Linq
Imports System.Data.Entity
Imports FrameWork.Gestores
Imports FrameWork.Extensiones
Imports FrameWork.Modulos
Imports Newtonsoft.Json
Imports System.Collections.Generic
' ----------------------------------------------------------------------------------------------------------------
' -------------------------------------------------- Instancia ---------------------------------------------------
' ----------------------------------------------------------------------------------------------------------------

Namespace Gestores
    Public Class Ges_Trace
        Inherits FW_BaseGestor
        Dim ContextDB As FW_BaseDBContextTrace
        Dim ArchivoTrace As String = AppConfig("RutaTraces") + "\trace.txt"
        Property ContextDBTransaction As DbContextTransaction
        Property TransactionID As String
        Property objTraceTXT As New DAO_TraceTXT

        Public Sub New()
            ContextDB = New FW_BaseDBContextTrace(AppConfig("ConnectionString"))
            Me.TransactionID = "xxxxxxxx"
        End Sub

        Public Sub New(TransactionID As String)
            ContextDB = New FW_BaseDBContextTrace(AppConfig("ConnectionString"))
            Me.TransactionID = TransactionID
        End Sub

        Public Sub New(Transaction As FW_BaseTransaction)
            MyBase.New(Transaction)
            ContextDB = New FW_BaseDBContextTrace(AppConfig("ConnectionString"))
            Me.TransactionID = Transaction.TransactionID
        End Sub

        Public Sub InitTransaction()
            ContextDBTransaction = Me.ContextDB.Database.BeginTransaction
        End Sub
        Public Sub EndTransaction()
            ContextDB.SaveChanges()
            ContextDBTransaction.Commit()
            ContextDB.Database.Connection.Close()
            ContextDBTransaction.Dispose()
        End Sub

        Public Sub TraceAction(Transaction As FW_BaseTransaction)
            Try
                InitTransaction()
                Dim objTraceAction As DAO_TraceTransaction
                objTraceAction = CreateObjectAction(Transaction)
                ContextDB.TraceAcciones.Add(objTraceAction)
                For Each AccionDetalle As DAO_TraceAccionesDetalle In objTraceAction.AccionesDetalle
                    ContextDB.TraceAccionesDetalle.Add(AccionDetalle)
                Next
                EndTransaction()
            Catch ex As Exception
                TraceException(ex)
                TraceActionTXT(Transaction)
            End Try
        End Sub

        Public Sub TraceException(Ex As Exception, Optional FirstTime As Boolean = True)
            Try
                InitTransaction()
                Dim objTraceException As DAO_TraceException
                objTraceException = CreateObjectException(Ex)
                ContextDB.TraceExceptions.Add(objTraceException)
                EndTransaction()
            Catch ex2 As Exception
                If FirstTime Then
                    TraceException(ex2, False)
                End If
                TraceExceptionTXT(Ex)
            End Try
        End Sub

        Public Sub TraceInformation(Origen As String, Information As String)
            Try
                InitTransaction()
                Dim objTraceInformation As DAO_TraceInformacion
                objTraceInformation = CreateObjectInformation(Origen, Information)
                ContextDB.TraceInformation.Add(objTraceInformation)
                EndTransaction()
            Catch ex As Exception
                TraceException(ex)
                TraceInformationTXT(Origen, Information)
            End Try
        End Sub

        Private Sub TraceActionTXT(Transaction As FW_BaseTransaction)
            Try
                Dim objTraceAction As DAO_TraceTransaction
                objTraceAction = CreateObjectAction(Transaction)
                objTraceTXT.Transaction = objTraceAction
                TraceTxt()
            Catch ex As Exception

            End Try
        End Sub
        Private Sub TraceExceptionTXT(Ex As Exception)
            Try
                Dim objTraceException As DAO_TraceException
                objTraceException = CreateObjectException(Ex)
                objTraceTXT.Excepction = objTraceException
                TraceTxt()
            Catch ex2 As Exception

            End Try
        End Sub
        Private Sub TraceInformationTXT(Origen As String, Information As String)
            Try
                Dim objTraceInformation As DAO_TraceInformacion
                objTraceInformation = CreateObjectInformation(Origen, Information)
                objTraceTXT.Information = objTraceInformation
                TraceTxt()
            Catch ex As Exception

            End Try
        End Sub

        Public Class HttpRequestExpress
            Property Uri As String
            Property Method As String
            Property Headers As String
        End Class

        Public Function GetHttpRequestExpress(Request As Net.Http.HttpRequestMessage) As HttpRequestExpress
            Dim RequestExpress As New HttpRequestExpress
            RequestExpress.Method = Request.Method.Method
            RequestExpress.Headers = Request.Headers.ToList.ToJson
            RequestExpress.Uri = Request.RequestUri.AbsoluteUri
            Return RequestExpress
        End Function

        Public Function CreateObjectAction(Transaction As FW_BaseTransaction) As DAO_TraceTransaction
            Dim TraceAction As New DAO_TraceTransaction
            With TraceAction
                .IDTransaction = Me.TransactionID
                .IdUsuario = Transaction.Sesion.Usuario.Id
                .TransactionName = Transaction.TransactionName
                .FechaFin = Transaction.HoraFin
                .OK = Transaction.ResponseDTO.Summary.Ok
                If Transaction.ResponseDTO.Summary.HasSystemError Then
                    .TipoError = "Error de sistema"
                ElseIf Transaction.ResponseDTO.Summary.HasUserErrors Then
                    .TipoError = "Error de usuario"
                ElseIf Transaction.ResponseDTO.Summary.Unsuccessfully Then
                    .TipoError = "Tarea insatisfecha"
                ElseIf Transaction.ResponseDTO.Summary.HasSecurityWarning Then
                    .TipoError = "Alerta de seguridad"
                End If
                .FechaInicio = Transaction.HoraInicio
                .ConsumoCPU = Transaction.ConsumoCPU
                .ConsumoRAMInicio = Transaction.ConsumoRAMInicio
                .ConsumoRAMFin = Transaction.ConsumoRAMFin
                .RequestDTO = Transaction.RequestDTO.ToJson
                .RequestHTTP = GetHttpRequestExpress(Transaction.RequestHttp).ToJson
                .UltimaEtapa = Transaction.Eventos.Last.Descripcion
            End With
            Dim i As Int16 = 0
            Dim FechaFin As DateTime
            For i = 0 To Transaction.Eventos.Count - 1
                Transaction.Eventos(i).NroEtapa = i + 1
                Transaction.Eventos(i).IDTransaction = TraceAction.IDTransaction
            Next
            FechaFin = TraceAction.FechaFin
            For i = Transaction.Eventos.Count - 1 To 0 Step -1
                Transaction.Eventos.Item(i).FechaFin = FechaFin
                Transaction.Eventos.Item(i).IDTransaction = Me.TransactionID
                FechaFin = Transaction.Eventos.Item(i).FechaInicio
            Next
            TraceAction.AccionesDetalle.AddRange(Transaction.Eventos)
            Return TraceAction
        End Function

        Public Function CreateObjectException(Ex As Exception) As DAO_TraceException
            Dim retTraceException As New DAO_TraceException
            With retTraceException
                .FechaInicio = DateTime.Now
                .FechaFin = DateTime.Now
                .Exception = Ex.ToJson
                .IDTransaction = Me.TransactionID
                .Mensaje = Ex.Message
            End With
            Return retTraceException
        End Function

        Public Function CreateObjectInformation(Origen As String, Information As String) As DAO_TraceInformacion
            Dim retTraceInformation As New DAO_TraceInformacion
            With retTraceInformation
                .Mensaje = Information
                .Origen = Origen
                .IDTransaction = Me.TransactionID
                .FechaInicio = DateTime.Now
                .FechaFin = DateTime.Now
            End With
            Return retTraceInformation
        End Function

        Public Sub TraceTxt()
            Dim guardar As New Ges_Files(ArchivoTrace, Enum_FileAccess.Append)
            guardar.WriteLine(JsonConvert.SerializeObject(objTraceTXT) + ",")
            guardar.Close()
        End Sub

        Public Sub UploadTraceTxtToDB()
            If IO.File.Exists(ArchivoTrace) Then
                InitTransaction()
                Dim TxtFile As New Ges_Files(ArchivoTrace, Enum_FileAccess.Read)
                Dim TxtJSON As String = "[" + TxtFile.Content.TrimEnd(",") + "]"
                Dim Lst_DAO_TraceTXT As List(Of DAO_TraceTXT)
                Lst_DAO_TraceTXT = TxtJSON.JsonToObject(Of List(Of DAO_TraceTXT))
                For Each item As DAO_TraceTXT In Lst_DAO_TraceTXT
                    If Not item.Transaction Is Nothing Then
                        ContextDB.TraceAcciones.Add(item.Transaction)
                        ContextDB.TraceAccionesDetalle.AddRange(item.Transaction.AccionesDetalle)
                    End If
                    If Not item.Excepction Is Nothing Then
                        ContextDB.TraceExceptions.Add(item.Excepction)
                    End If
                    If Not item.Information Is Nothing Then
                        ContextDB.TraceInformation.Add(item.Information)
                    End If
                Next
                EndTransaction()
                IO.File.Delete(ArchivoTrace)
            End If
        End Sub
    End Class
End Namespace

























