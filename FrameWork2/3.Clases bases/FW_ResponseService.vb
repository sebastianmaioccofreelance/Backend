Imports System.Collections.Generic
Imports System.Data.Entity
Imports FrameWork.Security
Imports FrameWork.DAO
Imports FrameWork.Modulos
Imports FrameWork.Gestores.Extensiones
Imports FrameWork.Gestores
Imports FrameWork.Bases

Public Class FW_ResponseService
#Region "Propiedades"
    Public Sub New()
        ' Dim a As Object
    End Sub
    Property Data As Object
    Property Transaction As FW_BaseTransaction
    Property Issues As New Dictionary(Of String, String)
    Property Exception As Exception
    Property Results As Enum_ResultsServices = 0

    Public Sub New(Transaction As FW_BaseTransaction)
        Me.Transaction = Transaction
    End Sub
    Public Sub ThrowException(ex As Exception, Optional BreakProcess As Boolean = True)

    End Sub
    Public Sub AddIssue(Codigo As String, Valor As String)
        Issues.Add(Codigo, Valor)
    End Sub

    Public Sub ClearIssues()
        Issues.Clear()
    End Sub

    Public Function HasIssue(Codigo As String) As Boolean
        Return Issues.ContainsKey(Codigo)
    End Function
    Public ReadOnly Property HasIssues As Boolean
        Get
            Return Issues.Count > 0
        End Get
    End Property

    Public ReadOnly Property HasData As Boolean
        Get
            Return Not IsNothing(Data)
        End Get
    End Property

    Public ReadOnly Property HasNotData As Boolean
        Get
            Return IsNothing(Data)
        End Get
    End Property

    ReadOnly Property IsVoid As Boolean
        Get
            Return InResults(Enum_ResultsServices.Void)
        End Get
    End Property

    ReadOnly Property IsSingle As Boolean
        Get
            Return InResults(Enum_ResultsServices.Single)
        End Get
    End Property

    ReadOnly Property IsTrue As Boolean
        Get
            Return InResults(Enum_ResultsServices.IsTrue)
        End Get
    End Property

    ReadOnly Property IsFalse As Boolean
        Get
            Return InResults(Enum_ResultsServices.IsFalse)
        End Get
    End Property

#End Region

#Region "Set results"

    Public Function SetData(Data As Object) As FW_ResponseService
        Me.Data = Data
        Return Me
    End Function

    Public Function SetData(Data As Object, RecordsCount As Int64) As FW_ResponseService
        Me.Data = Data
        SetResultsByCountRecords(RecordsCount)
        Return Me
    End Function
    Public Function SetDataAndResults(Data As Object, RecordsCount As Int64) As FW_ResponseService
        SetData(Data, RecordsCount)
        Return Me
    End Function

    Public Function SetResults(Results As Enum_ResultsServices) As FW_ResponseService
        Me.Results = Results
        Return Me
    End Function
    Public Function SetRestulsBroke() As FW_ResponseService
        Me.Results = Enum_ResultsServices.Broke
        Return Me
    End Function
    Public Function SetRestulsFound() As FW_ResponseService
        Me.Results = Enum_ResultsServices.Found
        Return Me
    End Function
    Public Function SetRestulsNotFound() As FW_ResponseService
        Me.Results = Enum_ResultsServices.NotFound
        Return Me
    End Function
    Public Function SetTrue() As FW_ResponseService
        Return SetResultsTrue()
    End Function
    Public Function SetFalse() As FW_ResponseService
        Return SetResultsFalse()
    End Function
    Public Function SetResultsTrue() As FW_ResponseService
        Me.Results = Enum_ResultsServices.IsTrue
        Return Me
    End Function

    Public Function SetResultsFalse() As FW_ResponseService
        Me.Results = Enum_ResultsServices.IsFalse
        Return Me
    End Function

    Public Function SetResultsVoid() As FW_ResponseService
        Me.Results = Enum_ResultsServices.Void
        Return Me
    End Function

    Public Function SetResultsSingle() As FW_ResponseService
        Me.Results = Enum_ResultsServices.Single
        Return Me
    End Function

    Public Function SetResultsMultiple() As FW_ResponseService
        Me.Results = Enum_ResultsServices.Multiple
        Return Me
    End Function

    Public Function SetResultsByCountRecords(TotalRecords As Int64) As FW_ResponseService
        If TotalRecords = 0 Then
            Me.Results = Enum_ResultsServices.Void
        ElseIf TotalRecords = 1 Then
            Me.Results = Enum_ResultsServices.Single
        Else
            Me.Results = Enum_ResultsServices.Multiple
        End If
        Return Me
    End Function

    Public Function SetResultsByBooleanValue(Value As Boolean) As FW_ResponseService
        If Value Then
            Me.Results = Enum_ResultsServices.IsTrue
        Else
            Me.Results = Enum_ResultsServices.IsFalse
        End If
        Return Me
    End Function
#End Region

#Region "Errores"

    Public Function UserError(Results As Enum_ResultsServices, Mensaje As String) As FW_ResponseService
        If InResults(Results) Then
            Transaction.Evento(Mensaje, TiposDeEvento.ErrorUsuario)
        End If
        Return Me
    End Function

    Public Function Unsuccessfully(Results As Enum_ResultsServices, Mensaje As String) As FW_ResponseService
        If InResults(Results) Then
            Throw New ExceptionUnsuccessfully(Mensaje)
        End If
        Return Me
    End Function

    Public Function AccessViolation(Results As Enum_ResultsServices) As FW_ResponseService
        If InResults(Results) Then
            Throw New ExceptionSecurity("Violacion de la seguridad")
        End If
        Return Me
    End Function

    Function InResults(Valor As Enum_ResultsServices) As Boolean
        Dim ret As Boolean = Convert.ToInt64(Valor).ValorEnSumaDeConstantes(Results)
        Return ret
    End Function
#End Region
    Public Enum Enum_ResultsServices
        IsTrue = 1
        IsFalse = 2
        Void = 4
        [Single] = 8
        Multiple = 16
        Broke = 32
        Found = 64
        NotFound = 128
    End Enum
End Class
