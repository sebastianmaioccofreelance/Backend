Imports System.Data.Entity

Public Class BL_BaseDBContext
    Inherits FrameWork.Bases.FW_BaseDBContext
    Public Sub New()
        MyBase.New(AppConfig("ConnectionString"))
    End Sub
End Class
