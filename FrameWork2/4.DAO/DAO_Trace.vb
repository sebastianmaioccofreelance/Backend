Imports System.Linq
Imports System.Collections.Generic
Imports System.Web.Http
Imports System.Web.Http.Controllers
Imports System.Net.Http
Imports System.Text
Imports FrameWork.Extensiones
Imports FrameWork
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports FrameWork.Gestores
Imports FrameWork.Modulos

Namespace DAO

    Public Class DAO_BaseTrace
        Property Id As Int64
        Property IDTransaction As String
        Property FechaInicio As DateTime
        Property FechaFin As DateTime
        ReadOnly Property Tiempo As Double
            Get
                Return FechaFin.MilisegundosTranscurridos(FechaInicio)
            End Get
        End Property
    End Class

    Public Class DAO_TraceTXT
        Property Transaction As DAO_TraceTransaction
        Property Excepction As DAO_TraceException
        Property Information As DAO_TraceInformacion
    End Class

    <Table("TraceAccionesDetalle")>
    Public Class DAO_TraceAccionesDetalle
        Inherits DAO_BaseTrace
        Property TipoEvento As String
        Property NroEtapa As Int32
        Property Descripcion As String
    End Class

    <Table("TraceTransaction")>
    Public Class DAO_TraceTransaction
        Inherits DAO_BaseTrace
        Property TransactionName As String
        Property IdUsuario As Int64
        Property RequestDTO As String
        Property RequestHTTP As String
        Property OK As Boolean
        Property TipoError As String
        Property UltimaEtapa As String
        Property ConsumoCPU As Decimal
        Property ConsumoRAMInicio As Decimal
        Property ConsumoRAMFin As Decimal
        <NotMapped>
        Property AccionesDetalle As New List(Of DAO_TraceAccionesDetalle)
    End Class

    <Table("TraceException")>
    Public Class DAO_TraceException
        Inherits DAO_BaseTrace
        Property Mensaje As String
        Property Exception As String
    End Class

    <Table("TraceInformation")>
    Public Class DAO_TraceInformacion
        Inherits DAO_BaseTrace
        Property Mensaje As String
        Property Origen As String
    End Class
End Namespace
