
Imports System.Linq
Imports System.Runtime.CompilerServices
Imports System.Reflection
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports Newtonsoft.Json
Imports System.Text
Imports System.Security.Cryptography

Namespace Modulos
    Public Module Extensiones
        <Extension>
        Public Function ToJson(ByVal Objeto As Object, Optional IgnoreNull As Boolean = False) As String
            If IgnoreNull Then
                Return JsonConvert.SerializeObject(Objeto, Formatting.None, New JsonSerializerSettings With {.NullValueHandling = NullValueHandling.Ignore})
            End If
            Return JsonConvert.SerializeObject(Objeto)
        End Function

        <Extension>
        Public Function JsonToObject(Of t)(StringJson As String) As t
            Return JsonConvert.DeserializeObject(Of t)(StringJson)
        End Function

        <Extension>
        Public Function ToList(Of t)(Consulta As System.Data.Entity.Infrastructure.DbRawSqlQuery(Of t)) As List(Of t)
            Return Consulta.ToJson.JsonToObject(Of List(Of t))
        End Function

        <Extension>
        Public Function Comparar(Texto As String, Patron As String) As Boolean
            Return Texto Like Patron
        End Function

        <Extension>
        Public Function ToBase64(Texto As String) As String
            Dim bytes As Byte()
            bytes = ASCIIEncoding.ASCII.GetBytes(Texto)
            Return Convert.ToBase64String(bytes)
        End Function

        <Extension>
        Public Function Base64ToString(Texto As String) As String
            Dim bytes As Byte()
            bytes = Convert.FromBase64String(Texto)
            Return Encoding.Default.GetString(bytes)
        End Function

        <Extension>
        Public Function Encriptar(Texto As String) As String
            Dim Data As Byte()
            Dim Keys As Byte()
            Data = UTF8Encoding.UTF8.GetBytes(Texto)
            Dim md5 As New MD5CryptoServiceProvider
            Keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(AppConfig("HashEncriptacion")))
            Dim tripDes As New TripleDESCryptoServiceProvider With {.Key = Keys, .Mode = CipherMode.ECB, .Padding = PaddingMode.PKCS7}
            Dim Transform As ICryptoTransform = tripDes.CreateEncryptor
            Dim Results As Byte() = Transform.TransformFinalBlock(Data, 0, Data.Length)
            Return Convert.ToBase64String(Results, 0, Results.Length)
        End Function

        <Extension>
        Public Function Desencriptar(TextoEncriptado As String) As String
            Try
                Dim Data As Byte()
                Dim Keys As Byte()
                Data = Convert.FromBase64String(TextoEncriptado)
                Dim md5 As New MD5CryptoServiceProvider
                Keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(AppConfig("HashEncriptacion")))
                Dim tripDes As New TripleDESCryptoServiceProvider With {.Key = Keys, .Mode = CipherMode.ECB, .Padding = PaddingMode.PKCS7}
                Dim Transform As ICryptoTransform = tripDes.CreateDecryptor
                Dim Results As Byte() = Transform.TransformFinalBlock(Data, 0, Data.Length)
                Return UTF8Encoding.UTF8.GetString(Results)
            Catch ex As Exception
                Return ""
            End Try
        End Function

        <Extension>
        Function EncriptarPassword(Usuario As DAO.DAO_Usuario) As DAO.DAO_Usuario
            Usuario.Password = Usuario.Password.Encriptar()
            Return Usuario
        End Function

        <Extension>
        Function PasswordDesencriptada(Usuario As DAO.DAO_Usuario) As DAO.DAO_Usuario
            Usuario.Password = Usuario.Password.Desencriptar(AppConfig("HashEncriptacion"))
            Return Usuario
        End Function

        <Extension>
        Function ToObject(Of t As New)(Fila As DataRow, Data As t) As t
            Dim TipoObjeto As Type = Data.GetType
            Dim ret = Activator.CreateInstance(TipoObjeto)
            For Each Columna As DataColumn In Fila.Table.Columns
                Dim properties As PropertyInfo() = Data.GetType.GetProperties()
                For Each ObjProperty As PropertyInfo In properties
                    Dim Mapear As Boolean = True
                    If ObjProperty.Name.ToLower = Columna.ColumnName.ToLower Then
                        If ObjProperty.PropertyType.Name = "String" Then
                            If IsDBNull(Fila(Columna.ColumnName)) Then
                                Mapear = False
                            End If
                        End If
                        If Mapear Then
                            ObjProperty.SetValue(ret, Fila(Columna.ColumnName), Nothing)
                        End If
                    End If
                Next
            Next
            Return ret
        End Function

        <Extension>
        Function ToObject(Of t As New)(Fila As DataRow) As t
            Dim ret As New t()
            For Each Columna As DataColumn In Fila.Table.Columns
                Dim properties As PropertyInfo() = GetType(t).GetProperties()
                For Each ObjProperty As PropertyInfo In properties
                    Dim Mapear As Boolean = True
                    If ObjProperty.Name.ToLower = Columna.ColumnName.ToLower Then
                        If IsDBNull(Fila(Columna.ColumnName)) Or Fila(Columna.ColumnName).GetType.Name = "DBNull" Then
                            Mapear = False
                        End If
                        If Mapear Then
                            ObjProperty.SetValue(ret, Fila(Columna.ColumnName), Nothing)
                        End If
                    End If
                Next
            Next
        End Function

        <Extension>
        Function ToList(Of t As New)(Tabla As DataTable, Data As t) As List(Of t)
            Dim TipoObjeto As Type = Data.GetType
            Dim lstObjetos As New List(Of t)
            Dim properties As PropertyInfo() = Data.GetType.GetProperties()
            For Each Fila As DataRow In Tabla.Rows
                Dim Objeto = Activator.CreateInstance(TipoObjeto)
                For Each Columna As DataColumn In Tabla.Columns
                    For Each ObjProperty As PropertyInfo In properties
                        Dim Mapear As Boolean = True
                        If ObjProperty.Name.ToLower = Columna.ColumnName.ToLower Then
                            If IsDBNull(Fila(Columna.ColumnName)) Or Fila(Columna.ColumnName).GetType.Name = "DBNull" Then
                                Mapear = False
                            End If
                            If Mapear Then
                                ObjProperty.SetValue(Objeto, Fila(Columna.ColumnName), Nothing)
                            End If
                        End If
                    Next
                Next
                lstObjetos.Add(Objeto)
            Next
            Return lstObjetos
        End Function

        <Extension>
        Function ToList(Of t As New)(Tabla As DataTable) As List(Of t)
            Dim lstObjetos As New List(Of t)
            For Each Fila As DataRow In Tabla.Rows
                Dim Objeto As New t()
                For Each Columna As DataColumn In Tabla.Columns
                    Dim properties As PropertyInfo() = GetType(t).GetProperties()
                    For Each ObjProperty As PropertyInfo In properties
                        Dim Mapear As Boolean = True
                        If ObjProperty.Name = Columna.ColumnName Then
                            If ObjProperty.Name.ToLower = Columna.ColumnName.ToLower Then
                                If IsDBNull(Fila(Columna.ColumnName)) Or Fila(Columna.ColumnName).GetType.Name = "DBNull" Then
                                    Mapear = False
                                End If
                                If Mapear Then
                                    ObjProperty.SetValue(Objeto, Fila(Columna.ColumnName), Nothing)
                                End If
                            End If
                        End If
                    Next
                Next
                lstObjetos.Add(Objeto)
            Next
            Return lstObjetos
        End Function

        <Extension>
        Function CastListOf(Of TOne As New, TTwo As New)(Data As List(Of TTwo)) As List(Of TOne)
            Dim ret As New List(Of TOne)
            For Each ObjetoA As TTwo In Data
                Dim TipoObjeto As Type = ObjetoA.GetType
                Dim ObjetoB As New TOne
                For Each ObjPropertyA As PropertyInfo In TipoObjeto.GetProperties()
                    If ObjPropertyA.CanWrite Then
                        For Each ObjPropertyB As PropertyInfo In GetType(TOne).GetProperties()
                            If ObjPropertyA.Name.ToLower = ObjPropertyB.Name.ToLower Then
                                ObjPropertyB.SetValue(ObjetoB, ObjPropertyA.GetValue(ObjetoA))
                            End If
                        Next
                    End If
                Next
                ret.Add(ObjetoB)
            Next
            Return ret
        End Function

        <Extension>
        Function Cast(Of TOne As New, TTwo As New)(Objeto As TTwo) As TOne
            Dim ret As New TOne
            Dim TipoObjeto As Type = Objeto.GetType
            For Each ObjPropertyA As PropertyInfo In TipoObjeto.GetProperties()
                If ObjPropertyA.CanWrite Then
                    For Each ObjPropertyB As PropertyInfo In GetType(TOne).GetProperties()
                        If ObjPropertyA.Name.ToLower = ObjPropertyB.Name.ToLower Then
                            ObjPropertyB.SetValue(ret, ObjPropertyA.GetValue(Objeto))
                        End If
                    Next
                End If
            Next
            Return ret
        End Function

        <Extension>
        Function ToSecurityText(StrSQL As String) As String
            Dim ret As String
            ret = Replace(StrSQL, "'", "''")
            Return ret
        End Function

        <Extension>
        Function ValorEnSumaDeConstantes(Valor As Int64, SumatoriaDeConstantes As Int64) As Boolean
            Dim ListaValores As New List(Of Int64)
            While SumatoriaDeConstantes > 0
                Dim ValorContenido As Int64 = 2 ^ (Math.Floor(Math.Log(SumatoriaDeConstantes) / Math.Log(2)))
                ListaValores.Add(ValorContenido)
                SumatoriaDeConstantes -= ValorContenido
            End While
            Return ListaValores.Contains(Valor)
        End Function

        <Extension>
        Function MilisegundosTranscurridos(FechaA As DateTime, FechaB As DateTime) As Double
            Dim Tiempo As TimeSpan
            Dim Milisegundos As Double
            Tiempo = FechaA - FechaB
            Milisegundos = Tiempo.Milliseconds
            Return Math.Abs(Milisegundos)
        End Function
    End Module
End Namespace