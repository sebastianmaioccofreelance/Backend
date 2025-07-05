Imports FrameWork.Bases
Imports FrameWork.Gestores.Extensiones
Imports Newtonsoft.Json
Imports System.IO
Imports System.Reflection
Imports System.Data
Imports System.Data.OleDb
Imports System.Globalization
Imports FrameWork.Modulos
Imports System.Linq
Imports System.Windows.Forms
Imports System.Collections.Generic

Public Class ClsMapeos

    Public Sub New()

    End Sub
    Public Sub New(CarpetaDeTrabajo As String)
        Me.CarpetaDeTrabajo = CarpetaDeTrabajo
    End Sub

    Property CarpetaDeTrabajo As String
    Property SeparadorCSV As String = ";"

    Public Sub FormularioAObjeto(Formulario As Form, ByRef Destino As Object)
        Dim properties As PropertyInfo() = Destino.GetType.GetProperties()
        Dim Ctrls As New List(Of Control)
        ListarControles(Ctrls, Formulario)
        For Each ObjProperty As PropertyInfo In properties
            For Each ObjControl As Control In Ctrls
                If ObjProperty.Name.ToLower = ObjControl.Name.Split("_").Last.ToLower Then
                    Select Case True
                        Case TypeOf ObjControl Is TextBox
                            ObjProperty.SetValue(Destino, DirectCast(ObjControl, TextBox).Text)
                        Case TypeOf ObjControl Is RichTextBox
                            ObjProperty.SetValue(Destino, DirectCast(ObjControl, RichTextBox).Text)
                        Case TypeOf ObjControl Is ComboBox
                            ObjProperty.SetValue(Destino, DirectCast(ObjControl, ComboBox).Text)
                        Case TypeOf ObjControl Is ListBox
                            ObjProperty.SetValue(Destino, DirectCast(ObjControl, ListBox).SelectedValue)
                        Case TypeOf ObjControl Is CheckBox
                            ObjProperty.SetValue(Destino, DirectCast(ObjControl, CheckBox).Checked)
                        Case TypeOf ObjControl Is MaskedTextBox
                            ObjProperty.SetValue(Destino, DirectCast(ObjControl, MaskedTextBox).Text)
                        Case TypeOf ObjControl Is DateTimePicker
                            ObjProperty.SetValue(Destino, DirectCast(ObjControl, DateTimePicker).Value)
                            'Case TypeOf ObjControl Is RadioButton
                            '    If DirectCast(ObjControl, RadioButton).Checked Then
                            '        ObjProperty.SetValue(Destino, DirectCast(ObjControl, RadioButton).Text)
                            '    End If
                    End Select
                End If
            Next
        Next
    End Sub

    Public Sub ObjetoAFormulario(ByRef Objeto As Object, Formulario As Form)
        Dim properties As PropertyInfo() = Objeto.GetType.GetProperties()
        Dim Ctrls As New List(Of Control)
        ListarControles(Ctrls, Formulario)
        For Each ObjProperty As PropertyInfo In properties
            For Each ObjControl As Control In Ctrls
                If ObjProperty.Name.ToLower = ObjControl.Name.Split("_").Last.ToLower Then
                    Select Case True
                        Case TypeOf ObjControl Is TextBox
                            DirectCast(ObjControl, TextBox).Text = ObjProperty.GetValue(Objeto)

                        Case TypeOf ObjControl Is RichTextBox
                            DirectCast(ObjControl, RichTextBox).Text = ObjProperty.GetValue(Objeto)

                        Case TypeOf ObjControl Is ComboBox
                            DirectCast(ObjControl, ComboBox).SelectedItem = ObjProperty.GetValue(Objeto)

                        Case TypeOf ObjControl Is ListBox
                            DirectCast(ObjControl, ListBox).SelectedItem = ObjProperty.GetValue(Objeto)

                        Case TypeOf ObjControl Is CheckBox
                            DirectCast(ObjControl, CheckBox).Checked = ObjProperty.GetValue(Objeto)

                        Case TypeOf ObjControl Is MaskedTextBox
                            DirectCast(ObjControl, MaskedTextBox).Text = ObjProperty.GetValue(Objeto)

                        Case TypeOf ObjControl Is DateTimePicker
                            DirectCast(ObjControl, DateTimePicker).Value = ObjProperty.GetValue(Objeto)

                            'Case TypeOf ObjControl Is RadioButton
                            '    If DirectCast(ObjControl, RadioButton).Checked Then
                            '        DirectCast(ObjControl, RadioButton).Text = ObjProperty.GetValue(Destino)
                            '        ObjProperty.SetValue(Destino, DirectCast(ObjControl, RadioButton).Text)
                            '    End If
                    End Select
                End If
            Next
        Next
    End Sub

    Public Function CargarArchivoJson(Archivo As String, Optional RutaRelativa As Boolean = True)
        Dim Ruta As String
        If RutaRelativa Then
            Ruta = CarpetaDeTrabajo + "\" + Archivo + ".json"
        Else
            Ruta = Archivo
        End If
        Using sr As StreamReader = New StreamReader(Ruta)
            Return sr.ReadToEnd()
        End Using
    End Function

    Public Function ArchivoJsonAObjeto(Of T)(NombreArchivo As String, Optional RutaRelativa As Boolean = True) As T
        Dim Json As String = CargarArchivoJson(NombreArchivo, RutaRelativa)
        Return JsonConvert.DeserializeObject(Of T)(Json)
    End Function

    Public Sub ArchivoJsonAFormulario(Of T)(NombreArchivo As String, ByRef Formulario As Form)
        Dim Json As String = CargarArchivoJson(NombreArchivo)
        Dim Objeto As T
        Objeto = Json.JsonToObject(Of T)
        ObjetoAFormulario(Objeto, Formulario)
    End Sub

    Public Sub JsonStringAFormulario(Of T)(Json As String, ByRef Formulario As Form)
        Dim Objeto As T
        Objeto = Json.JsonToObject(Of T)
        ObjetoAFormulario(Objeto, Formulario)
    End Sub

    Public Function FormularioAJsonString(Of T)(Formulario As Form)
        Dim Objeto As T
        FormularioAObjeto(Formulario, Objeto)
        Return Objeto.ToJson
    End Function

    Private Sub ListarControles(ByRef Controles As List(Of Control), Ctrl As Control)
        For Each ObjControl As Control In Ctrl.Controls
            Controles.Add(ObjControl)
            ListarControles(Controles, ObjControl)
        Next
    End Sub

    Public Sub JsonStringAArchivoJson(NombreArchivo As String, ContenidoJson As String, ByVal Optional Carpeta As String = "", Optional RutaRelativa As Boolean = True)
        Dim RutaConArchivo As String
        If RutaRelativa Then
            Carpeta = CarpetaDeTrabajo + "\" + Carpeta
        End If
        Dim Contador As Int64 = 1
        RecrearCarpeta(Carpeta)
        RutaConArchivo = Carpeta + "\" + NombreArchivo + ".json"
        While File.Exists(RutaConArchivo)
            RutaConArchivo = Carpeta + "\" + NombreArchivo + "(" + Contador.ToString + ").json"
            Contador += 1
        End While
        Dim ManejoArchivo As New ClsFiles(FullPath:=RutaConArchivo, TipoAcceso:=Enum_FileAccess.Write)
        ManejoArchivo.Write(ContenidoJson)
        ManejoArchivo.Close()
    End Sub

    Public Sub CSVStringAArchivoCSV(NombreArchivoCSV As String, CSVString As String, Optional Carpeta As String = "", Optional RutaRelativa As Boolean = True)
        Dim RutaConArchivo As String
        If RutaRelativa Then
            Carpeta = CarpetaDeTrabajo + "\" + Carpeta
        Else

        End If
        Carpeta = Carpeta.TrimEnd("\")
        RecrearCarpeta(Carpeta)
        Dim Contador As Int64 = 1
        RutaConArchivo = Carpeta + "\" + NombreArchivoCSV + ".csv"
        While File.Exists(RutaConArchivo)
            RutaConArchivo = Carpeta + "\" + NombreArchivoCSV + "(" + Contador.ToString + ").csv"
            Contador += 1
        End While
        Dim ManejoArchivo As New ClsFiles(FullPath:=RutaConArchivo, TipoAcceso:=Enum_FileAccess.Write)
        ManejoArchivo.Write(CSVString)
        ManejoArchivo.Close()
    End Sub

    Public Sub ListaDeObjetosAArchivosCSV(Of t)(Lista As List(Of t), ArchivoCSV As String, Optional CarpetaCSV As String = "", Optional RutaRelativaCarpeta As Boolean = True)
        Dim dt As DataTable = ListaDeObjetosADatatable(Of t)(Lista)
        DataTableACSV(dt, ArchivoCSV, CarpetaCSV, RutaRelativaCarpeta)
    End Sub

    Public Function ListaDeObjetosADatatable(Of t)(Lista As List(Of t)) As DataTable
        Dim dt As New DataTable
        If Lista.Count = 0 Then
            Return dt
        End If
        Dim properties As PropertyInfo() = Lista.First.GetType.GetProperties()
        dt.Columns.AddRange((From n In properties Select New DataColumn With {.ColumnName = n.Name}).ToArray)
        For Each Fila As t In Lista
            Dim row As DataRow = dt.NewRow
            For Each ObjProperty As PropertyInfo In properties
                row(ObjProperty.Name) = ObjProperty.GetValue(Fila)
            Next
            dt.Rows.Add(row)
        Next
        Return dt
    End Function

    Public Sub DataTableACSV(dt As DataTable, ArchivoCSV As String, Optional CarpetaCSV As String = "", Optional RutaRelativaCSV As Boolean = True)
        Dim ContenidoCSV As String = ""

        For Each Columna As DataColumn In dt.Columns
            ContenidoCSV += Columna.ColumnName + SeparadorCSV
        Next
        ContenidoCSV = ContenidoCSV.TrimEnd(",") + vbCrLf
        For Each Fila As DataRow In dt.Rows
            For Each Columna As DataColumn In dt.Columns
                ContenidoCSV += """" + Fila(Columna.ColumnName).ToString.Replace("""", """""") + """" + SeparadorCSV
            Next
            ContenidoCSV = ContenidoCSV.TrimEnd(",") + vbCrLf
        Next
        CSVStringAArchivoCSV(ArchivoCSV, ContenidoCSV, CarpetaCSV, RutaRelativaCSV)
    End Sub

    Public Sub ObjetoAArchivoJson(Objeto As Object, NombreArchivo As String, Optional Carpeta As String = "", Optional RutaRelativa As Boolean = True)
        Dim Json As String
        Json = JsonConvert.SerializeObject(Objeto)
        JsonStringAArchivoJson(NombreArchivo, Json, Carpeta, RutaRelativa)
    End Sub

    Public Sub FormularioAArchivoJson(Of t As New)(Formulario As Form, NombreArchivo As String, Optional Carpeta As String = "", Optional RutaRelativa As Boolean = True)
        Dim Objeto As New t
        FormularioAObjeto(Formulario, Objeto)
        ObjetoAArchivoJson(Objeto, NombreArchivo, Carpeta, RutaRelativa)
    End Sub

    Public Function CsvADataTable(Archivo As String, Optional RutaRelativa As Boolean = True) As DataTable
        If Not Archivo.EndsWith(".csv") Then
            Archivo += ".csv"
        End If
        If RutaRelativa Then
            Archivo = CarpetaDeTrabajo + "\" + Archivo
        End If
        Dim abrir As New ClsFiles(Archivo, Enum_FileAccess.Read)
        Dim Contenido As String = abrir.Content
        Contenido = Replace(Contenido, """""", Chr(1))
        Contenido = Replace(Contenido, """", Chr(2))
        Dim Separar() As String = Contenido.Split(vbCrLf)
        Dim Cabecera As Boolean = True
        Dim CaracterSeparador As String = ""
        Dim Celdas As String()
        Dim dt As New DataTable
        For Each Linea As String In Separar
            If Cabecera Then
                Dim CantidadPuntoYComas As Int64
                Dim CantidadComas As Int64
                CantidadComas = Split(Linea, ",").Count
                CantidadPuntoYComas = Split(Linea, ";").Count
                If CantidadComas > CantidadPuntoYComas Then
                    CaracterSeparador = ","
                Else
                    CaracterSeparador = ";"
                End If
                Linea = Linea.Replace(vbCr, "")
                Linea = Linea.Replace(vbLf, "")
                Celdas = SepararLineaCSV(Linea, CaracterSeparador)
                dt.Clear()
                For Each Columna In Celdas
                    If Not String.IsNullOrEmpty(Columna) Then
                        dt.Columns.Add(Columna)
                    End If

                Next
                Cabecera = False
            Else
                Linea = Linea.Replace(vbCr, "")
                Linea = Linea.Replace(vbLf, "")
                Celdas = SepararLineaCSV(Linea, CaracterSeparador)
                Dim row As DataRow
                row = dt.NewRow
                Dim i As Integer = 0
                For Each Celda As String In Celdas
                    If Not String.IsNullOrEmpty(Celda) Then
                        row(dt.Columns.Item(i).ColumnName) = IIf(Celda = Chr(34), "", Celda)
                    End If
                    i += 1
                Next
                If Linea <> "" Then
                    dt.Rows.Add(row)
                End If
            End If
        Next
        Return dt
    End Function

    Private Function SepararLineaCSV(Linea As String, Separador As String) As String()
        Dim retLista As New List(Of String)
        Dim EsCadena As Boolean = False
        Dim CadenaTransformada As String = ""
        For i = 0 To Linea.Length - 1
            Dim Caracter = Linea.Substring(i, 1)
            If Caracter = Chr(2) Then
                EsCadena = Not EsCadena
            End If
            If Caracter = Separador And EsCadena Then
                Caracter = Chr(3)
            End If
            CadenaTransformada += Caracter
        Next
        CadenaTransformada = CadenaTransformada.Replace(Chr(1), """")
        CadenaTransformada = CadenaTransformada.Replace(Chr(2), "")
        retLista = CadenaTransformada.Split(Separador).Select(Function(n) n.Replace(Chr(3), Separador)).ToList
        Return retLista.ToArray
    End Function

    Public Function CsvAListaDeObjetos(Of t As New)(CSVFilename As String, Optional RutaRelativa As Boolean = True) As List(Of t)
        Dim dt As DataTable = CsvADataTable(CSVFilename, RutaRelativa)
        Return DataTableAListaDeObjetos(Of t)(dt)
    End Function

    Public Sub DataTableACarpetaJson(Of t As New)(Tabla As DataTable, NomreArchivoJson As String, Optional CarpetaJson As String = "", Optional RutaRelativaCarpetaJson As Boolean = True)
        Dim ListaObjetos As List(Of t)
        ListaObjetos = DataTableAListaDeObjetos(Of t)(Tabla)
        ListaDeObjetosACarpetaJson(Of t)(ListaObjetos, NomreArchivoJson, CarpetaJson, RutaRelativaCarpetaJson)
    End Sub

    Public Function DataTableAListaDeObjetos(Of t As New)(Tabla As DataTable) As List(Of t)
        Dim LstObjects As New List(Of t)
        Dim properties As PropertyInfo() = GetType(t).GetProperties()
        For Each fila As DataRow In Tabla.Rows
            Dim ItemObjeto As New t
            For Each Columna As DataColumn In Tabla.Columns
                For Each ObjProperty As PropertyInfo In properties
                    If ObjProperty.Name.ToLower = Columna.ColumnName.ToLower Then
                        If ObjProperty.CanWrite Then
                            ObjProperty.SetValue(ItemObjeto, ConvertCellToObject(ObjProperty.PropertyType.Name, fila(Columna.ColumnName)))
                        End If
                    End If
                Next
            Next
            LstObjects.Add(ItemObjeto)
        Next
        Return LstObjects
    End Function

    Public Function SqlServerTypeToSqlDBType(ByVal Tipo As String) As SqlDbType
        Select Case Tipo.Split("(").First.ToLower
            Case "bigint" : Return SqlDbType.BigInt
            Case "binary" : Return SqlDbType.Binary
            Case "bit" : Return SqlDbType.Bit
            Case "date" : Return SqlDbType.Date
            Case "datetime" : Return SqlDbType.DateTime
            Case "datetimee2" : Return SqlDbType.DateTime2
            Case "datetimeoffset" : Return SqlDbType.DateTimeOffset
            Case "decimal" : Return SqlDbType.Decimal
            Case "float" : Return SqlDbType.Float
            Case "image" : Return SqlDbType.Image
            Case "int" : Return SqlDbType.Int
            Case "money" : Return SqlDbType.Money
            Case "nchar" : Return SqlDbType.NChar
            Case "ntext" : Return SqlDbType.NText
            Case "nvarchar" : Return SqlDbType.NVarChar
            Case "real" : Return SqlDbType.Real
            Case "smalldatetime" : Return SqlDbType.SmallDateTime
            Case "smallint" : Return SqlDbType.SmallInt
            Case "smallmoney" : Return SqlDbType.SmallMoney
            Case "text" : Return SqlDbType.Text
            Case "time" : Return SqlDbType.Time
            Case "timestamp" : Return SqlDbType.Timestamp
            Case "tinyint" : Return SqlDbType.TinyInt
            Case "uniqueidentifier" : Return SqlDbType.UniqueIdentifier
            Case "varbinary" : Return SqlDbType.VarBinary
            Case "varchar" : Return SqlDbType.VarChar
            Case "xml" : Return SqlDbType.Xml
        End Select
    End Function


    Public Function ConvertCellToObject(Tipo As String, Valor As Object) As Object
        Select Case Tipo.ToLower
            Case "string"
                Return Valor.ToString
            Case "decimal"
                Return Convert.ToDecimal(Valor)
            Case "integer", "int", "int32", "uint"
                Return Convert.ToInt32(Valor)
            Case "int64", "long", "ulong"
                Return Convert.ToInt64(Valor)
            Case "double"
                Return Convert.ToDouble(Valor)
            Case "float"
                Return Convert.ToDecimal(Valor)
            Case "byte"

            Case "ushort"
                Return Convert.ToInt16(Valor)
            Case "boolean,bool"
                Return Convert.ToBoolean(Valor)
            Case "datetime", "date"
                Return Convert.ToDateTime(Valor)
        End Select
    End Function

    Public Function CarpetaJsonADataTable(Of t)(Optional Ruta As String = "", Optional RutaRelativa As Boolean = True) As DataTable
        Dim Lista As List(Of t)
        Dim dt As DataTable
        Lista = CarpetaJsonAListaDeObjetos(Of t)(Ruta, RutaRelativa)
        dt = ListaDeObjetosADatatable(Of t)(Lista)
        Return dt
    End Function

    Public Function CarpetaJsonAListaDeObjetos(Of t)(Optional Ruta As String = "", Optional RutaRelativa As Boolean = True) As List(Of t)
        Dim Carpeta As String = CarpetaDeTrabajo
        If RutaRelativa Then
            Carpeta = CarpetaDeTrabajo + "\" + Ruta
        Else
            Carpeta = Ruta
        End If
        Dim ret As New List(Of t)
        For Each ArchivoJson As String In System.IO.Directory.EnumerateFiles(Carpeta)
            If ArchivoJson.ToLower.EndsWith(".json") Then
                ret.Add(ArchivoJsonAObjeto(Of t)(ArchivoJson, False))
            End If
        Next
        Return ret
    End Function

    Public Function CarpetaJsonACSV(Of t As New)(NombreArchivoCSV As String, Optional CarpetaCSV As String = "", Optional CarpetaJson As String = "", Optional RutaRelativaCSV As Boolean = True, Optional RutaRelativaCarpetaJson As Boolean = True)
        Dim dt As DataTable = CarpetaJsonADataTable(Of t)(CarpetaJson, RutaRelativaCarpetaJson)
        DataTableACSV(dt, NombreArchivoCSV, CarpetaCSV, RutaRelativaCSV)
    End Function

    Public Sub CSVACarpetaJson(Of t As New)(ArchivoCSV As String, NombreArchivoJson As String, Optional CarpetaJson As String = "", Optional RutaRelativaCSV As Boolean = True, Optional RutaRelativaJson As Boolean = True)
        Dim ListaObjetos As New List(Of t)
        ListaObjetos = CsvAListaDeObjetos(Of t)(ArchivoCSV, RutaRelativaCSV)
        ListaDeObjetosACarpetaJson(Of t)(ListaObjetos, NombreArchivoJson, CarpetaJson, RutaRelativaJson)
    End Sub

    Public Sub ListaDeObjetosACarpetaJson(Of t As New)(ListaObjetos As List(Of t), NombreArchivoJson As String, Optional CarpetaJson As String = "", Optional RutaRelativaJson As Boolean = True)
        For Each Objeto As t In ListaObjetos
            JsonStringAArchivoJson(NombreArchivoJson, Objeto.ToJson, CarpetaJson, RutaRelativaJson)
        Next
    End Sub

    Public Sub RecrearCarpeta(Carpeta As String)
        Dim SubRutas() As String = Carpeta.Split("\")
        Dim Arbol As String = SubRutas(0)
        For i = 1 To SubRutas.Length - 1
            Arbol += "\" + SubRutas(i)
            If Not IO.Directory.Exists(Arbol) Then
                IO.Directory.CreateDirectory(Arbol)
            End If
        Next
    End Sub
End Class







