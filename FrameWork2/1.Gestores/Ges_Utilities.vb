Imports System.Collections.Generic
Imports System.Data
Imports System.IO
Imports System.Linq
Imports System.Net
Imports System.Reflection
Imports System.Security.Cryptography
Imports System.Text
Imports System.Xml.Linq
Imports Newtonsoft.Json
Namespace Gestores
    Public Module ModUtilities
        Public Utilities As New Ges_Utilities
    End Module

    Public Class Ges_Utilities
        Public Function ToJson(ByVal Objeto As Object, Optional IgnoreNull As Boolean = False) As String
            If IgnoreNull Then
                Return JsonConvert.SerializeObject(Objeto, Formatting.None, New JsonSerializerSettings With {.NullValueHandling = NullValueHandling.Ignore})
            End If
            Return JsonConvert.SerializeObject(Objeto)
        End Function

        Public Function JsonToObject(Of t)(StringJson As String) As t
            Return JsonConvert.DeserializeObject(Of t)(StringJson)
        End Function

        Public Function ToXml(JSON As String, Optional Root As String = "Root") As XNode
            Return JsonConvert.DeserializeXNode(JSON, Root)
        End Function

        Public Function Base64ToString(Texto As String) As String
            Dim bytes As Byte()
            bytes = Convert.FromBase64String(Texto)
            Return Encoding.Default.GetString(bytes)
        End Function

        Function DataRowToObject(Of t As New)(Fila As DataRow, Data As t) As t
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

        Function DataTableToList(Of t As New)(Tabla As DataTable) As List(Of t)
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

        Public Function GenerarTransactionID() As String
            Dim Letras As Array = {"H", "J", "W", "X", "Y", "Z", "2", "3", "4", "5", "6", "7", "8", "9"}
            Dim r As New Random()
            Dim Indice As Int16
            Dim GUID As String = ""
            For i = 1 To 10
                Indice = r.Next(0, 14)
                GUID += Letras(Indice)
                If i = 5 Then
                    GUID += "-"
                End If
            Next
            Return GUID
        End Function

        Function ToSecurityText(StrSQL As String) As String
            Dim ret As String
            ret = Replace(StrSQL, "'", "''")
            Return ret
        End Function

        Private Function SendMessageRestAPI(ByVal url As String, ByVal headers As Dictionary(Of String, String), ByVal location As String, ByVal Optional JsonMessage As String = "") As ExternalServiceResponse
            Dim baseAddress = location
            Dim http = CType(WebRequest.Create(New Uri(baseAddress)), HttpWebRequest)
            For Each Atributo As KeyValuePair(Of String, String) In headers
                http.Headers.Add(Atributo.Key, Atributo.Value)
            Next
            http.Accept = "application/json"
            http.ContentType = "application/json"
            http.Method = "GET"
            If JsonMessage <> "" Then
                http.Method = "POST"
                Dim parsedContent As String = JsonMessage
                Dim encoding As ASCIIEncoding = New ASCIIEncoding()
                Dim bytes As Byte() = encoding.GetBytes(parsedContent)
                Dim newStream As Stream = http.GetRequestStream()
                newStream.Write(bytes, 0, bytes.Length)
                newStream.Close()
            End If
            Dim response2 = http.GetResponse()
            Dim stream = response2.GetResponseStream()
            Dim sr = New StreamReader(stream)
            Dim content = sr.ReadToEnd()
            If content = "" Then
                Throw New Exception("respuesta vacia.")
            End If
            Dim response As New ExternalServiceResponse
            response.JsonResponse = content
            response.header = response2.Headers
            response.withError = False
            Return response
        End Function

        Public Function Encriptar(Texto As String, Key As String) As String
            Dim Data As Byte()
            Dim Keys As Byte()
            Data = UTF8Encoding.UTF8.GetBytes(Texto)
            Dim md5 As New MD5CryptoServiceProvider
            Keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(Key))
            Dim tripDes As New TripleDESCryptoServiceProvider With {.Key = Keys, .Mode = CipherMode.ECB, .Padding = PaddingMode.PKCS7}
            Dim Transform As ICryptoTransform = tripDes.CreateEncryptor
            Dim Results As Byte() = Transform.TransformFinalBlock(Data, 0, Data.Length)
            Return Convert.ToBase64String(Results, 0, Results.Length)
        End Function

        Public Function Desencriptar(TextoEncriptado As String, Key As String) As String
            Dim Data As Byte()
            Dim Keys As Byte()
            Data = Convert.FromBase64String(TextoEncriptado)
            Dim md5 As New MD5CryptoServiceProvider
            Keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(Key))
            Dim tripDes As New TripleDESCryptoServiceProvider With {.Key = Keys, .Mode = CipherMode.ECB, .Padding = PaddingMode.PKCS7}
            Dim Transform As ICryptoTransform = tripDes.CreateDecryptor
            Dim Results As Byte() = Transform.TransformFinalBlock(Data, 0, Data.Length)
            Return UTF8Encoding.UTF8.GetString(Results)
        End Function
    End Class

    Public Class ExternalServiceResponse
        Public JsonResponse As String
        Public header As WebHeaderCollection
        Public withError As Boolean
        Public ValidationError As String
    End Class
End Namespace

