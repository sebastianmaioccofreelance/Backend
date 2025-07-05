Imports FrameWork.Bases
Imports FrameWork.Security
Imports System.Data.Entity
Imports System.Net
Imports FrameWork.DAO
Imports BusinessLogic.ClasesBases.Business
Imports FrameWork.Bases.DTORequests
Imports FrameWork.Bases.DTOResponses

Namespace ClasesBases
    Public Class BL_BaseTransaction
        Inherits FW_BaseTransaction
        Sub New(Request As FW_BaseRequestDTO, ContextDB As DbContext, Optional RequestHttp As Http.HttpRequestMessage = Nothing)
            MyBase.New(Request, ContextDB, RequestHttp)
            Business = New BL_ListaBusinessLogic(Me)
            Database = ContextDB
        End Sub
        Property Business As BL_ListaBusinessLogic
        Property Database As BL_BaseDBContext
    End Class
End Namespace
