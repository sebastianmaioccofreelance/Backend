Imports Excel = Microsoft.Office.Interop.Excel
Imports System.Collections.Generic
Imports System.Data
Imports System.Linq
Imports SautinSoft
Imports FrameWork.Bases

Namespace Gestores
    Public Class Ges_Excel
        Inherits FW_BaseGestor
        Public Sub New()
            MyBase.New
        End Sub
        Public Sub New(Transaction As FW_BaseTransaction)
            MyBase.New(Transaction)
        End Sub

        Public Sub New(ContextDB As FW_BaseDBContext)
            MyBase.New(ContextDB)
        End Sub
        Property Pestanias As New List(Of String)
        Property PestaniasExcel As New List(Of Excel.Worksheet)
        Property Tablas As New List(Of ComponenteTabla)
        Property Etiquetas As New List(Of ComponenteEtiqueta)
        Property Graficos As New List(Of ComponenteGrafico)
        Property ComponenteSeleccionado As Componente
        Property IndiceColumnaActiva As Int64
        Property IndiceFilaActiva As Int64
        Dim AplicacionExcel As Excel.Application
        Dim LibroExcel As Excel.Workbook
        Dim RangoActivo As Excel.Range
        Dim PestaniaActiva As Excel.Worksheet
        Dim EsNuevo As Boolean = False

        Public Sub NuevoArchivo()
            AplicacionExcel = CreateObject("Excel.Application")
            AplicacionExcel.Visible = False
            LibroExcel = AplicacionExcel.Workbooks.Add
            AplicacionExcel.ScreenUpdating = False
            CrearPestanias()
            EsNuevo = True
        End Sub
        Public Sub ExportarPDF(Archivo As String, Optional AbirArchivo As Boolean = False)
            AplicacionExcel.ActiveWorkbook.ExportAsFixedFormat(Excel.XlFixedFormatType.xlTypePDF, Archivo, Excel.XlFixedFormatQuality.xlQualityStandard, , , , , AbirArchivo)
        End Sub
        Public Sub Guardar()
            AplicacionExcel.ActiveWorkbook.Save()
        End Sub
        Public Sub GuardarComo(Archivo As String)
            AplicacionExcel.ActiveWorkbook.SaveAs(Archivo)
        End Sub
        Public Sub AbrirArchivo(Archivo As String)
            AplicacionExcel = CreateObject("Excel.Application")
            AplicacionExcel.Visible = False
            AplicacionExcel.ScreenUpdating = False
            LibroExcel = AplicacionExcel.Workbooks.Open(Archivo)
            CrearPestanias()
        End Sub

        Private Sub CrearPestanias()
            For Each Pestania As String In Pestanias
                Dim xlSheets = TryCast(LibroExcel.Sheets, Excel.Sheets)
                Dim YaExiste As Boolean = False
                For Each ObjPestania As Object In LibroExcel.Sheets
                    If ObjPestania.Name = Pestania Then
                        PestaniasExcel.Add(ObjPestania)
                        YaExiste = True
                    End If
                Next
                If Not YaExiste Then
                    Dim xlNewSheet = CType(xlSheets.Add(xlSheets(1), Type.Missing, Type.Missing, Type.Missing), Excel.Worksheet)
                    xlNewSheet.Name = Pestania
                    PestaniasExcel.Add(xlNewSheet)
                End If
            Next
            ActivarPestania(Pestanias.First)
            Posicionar(1, 1)
        End Sub

        Public Sub TerminarHojas()
            For Each Pestania As Excel.Worksheet In PestaniasExcel
                RangoActivo = Pestania.Range("A1", "CA1")
                RangoActivo.EntireColumn.AutoFit()
            Next
            If EsNuevo Then
                LibroExcel.Worksheets(Pestanias.Count + 1).Delete()
            End If
            AplicacionExcel.ScreenUpdating = True
        End Sub

        Public Sub FinalizarSesion(Optional Terminar As Boolean = False)
            If Terminar Then
                TerminarHojas()
            End If
            AplicacionExcel.UserControl = True
            RangoActivo = Nothing
            LibroExcel = Nothing
            AplicacionExcel.DisplayAlerts = False
            AplicacionExcel.Quit()
            AplicacionExcel = Nothing
        End Sub

        Public Sub Arriba(n As Int64)
            Me.IndiceFilaActiva -= n
        End Sub

        Public Sub Abajo(n As Int64)
            Me.IndiceFilaActiva += n
        End Sub

        Public Sub Izquierda(n As Int64)
            IndiceColumnaActiva -= n
        End Sub

        Public Sub Derecha(n As Int64)
            IndiceColumnaActiva += n
        End Sub

        Public Sub Desplazar(CorrerFila As Int64, CorrerColumna As Int64)
            Me.IndiceColumnaActiva += CorrerColumna
            Me.IndiceFilaActiva += CorrerFila
        End Sub

        Public Sub Posicionar(Fila As Int64, Columna As Int64)
            Me.IndiceColumnaActiva = Columna
            Me.IndiceFilaActiva = Fila
        End Sub

        Public Sub Tabla(Nombre As String, Accion As Enum_SeleccionComponente)
            Dim Tabla As ComponenteTabla = Me.Tablas.Where(Function(n) n.Nombre = Nombre).First
            Dim ObjDataTable As DataTable = Tabla.Tabla
            ComponenteSeleccionado = Tabla
            If Accion = Enum_SeleccionComponente.Posicionar Then
                Me.IndiceColumnaActiva = Tabla.IndiceColumnaDesde
                Me.IndiceFilaActiva = Tabla.IndiceFilaDesde
            ElseIf Accion = Enum_SeleccionComponente.Pegar Then
                Tabla.IndiceColumnaDesde = Me.IndiceColumnaActiva
                Tabla.IndiceFilaDesde = Me.IndiceFilaActiva
                Tabla.IndiceColumnaHasta = Tabla.IndiceColumnaDesde
                Tabla.IndiceFilaHasta = Tabla.IndiceFilaDesde

                ' genero las columnas
                Dim PunteroColumna As Int64 = 0
                If Tabla.Atributos.ConCabecera Then
                    For i = 0 To ObjDataTable.Columns.Count - 1
                        Dim Visible As Boolean = True
                        Dim TextoColumna As String = ObjDataTable.Columns.Item(i).ColumnName
                        For Each Formato As FormatoColumna In Tabla.Atributos.FormatosColumnas
                            If Formato.NombreColumna.ToLower = ObjDataTable.Columns.Item(i).ColumnName.ToLower Then
                                Visible = Formato.Visible
                                If Not String.IsNullOrEmpty(Formato.NombreTextual) Then
                                    TextoColumna = Formato.NombreTextual
                                End If
                            End If
                        Next
                        If Visible Then
                            RangoActivo = PestaniaActiva.Cells(Tabla.IndiceFilaHasta, Tabla.IndiceColumnaHasta + PunteroColumna)
                            RangoActivo.Value = TextoColumna
                            If Tabla.Atributos.Grilla Then
                                RangoActivo.Borders.LineStyle = Excel.XlLineStyle.xlContinuous
                            End If
                            Tabla.Atributos.Cabecera.AplicarAtributos(RangoActivo)
                            PunteroColumna += 1
                        End If
                    Next
                End If
                For i = 0 To ObjDataTable.Rows.Count - 1
                    Tabla.IndiceFilaHasta += 1
                    PunteroColumna = 0
                    For j = 0 To ObjDataTable.Columns.Count - 1
                        Dim CeldaAtributos As New AtributosCelda
                        Dim ValorCelda As Object
                        Dim NombreColumna As String
                        ValorCelda = ObjDataTable.Rows(i).Item(j)
                        NombreColumna = ObjDataTable.Columns(j).ColumnName
                        Dim Visible As Boolean = True
                        Dim FormatoColumna As String = "0"
                        For Each Formato As FormatoColumna In Tabla.Atributos.FormatosColumnas
                            If Formato.NombreColumna.ToLower = ObjDataTable.Columns.Item(j).ColumnName.ToLower Then
                                Visible = Formato.Visible
                                FormatoColumna = Formato.Formato
                            End If
                        Next
                        If Visible Then
                            RangoActivo = PestaniaActiva.Cells(Tabla.IndiceFilaHasta, Tabla.IndiceColumnaDesde + PunteroColumna)
                            If Not String.IsNullOrEmpty(FormatoColumna) Then
                                RangoActivo.NumberFormat = FormatoColumna
                            End If
                            RangoActivo.Value = ValorCelda
                            Select Case Tabla.Atributos.Tipo
                                Case Tipos.Basico
                                    CeldaAtributos = Tabla.Atributos.Celdas
                                Case Tipos.Columnas_Alternadas
                                    If PunteroColumna Mod 2 = 0 Then
                                        CeldaAtributos = Tabla.Atributos.Pares
                                    Else
                                        CeldaAtributos = Tabla.Atributos.Impares
                                    End If
                                Case Tipos.Filas_Alternadas
                                    If i Mod 2 = 0 Then
                                        CeldaAtributos = Tabla.Atributos.Pares
                                    Else
                                        CeldaAtributos = Tabla.Atributos.Impares
                                    End If
                            End Select
                            PunteroColumna += 1
                            Dim CondicionesParaColumna As New List(Of CondicionCelda)
                            CondicionesParaColumna = Tabla.Atributos.CeldasCondicionadas.Where(Function(n) n.ColumnaAplicar.ToLower = NombreColumna.ToLower Or n.AplicarFilaCompleta).ToList
                            For Each ObjCondicion As CondicionCelda In CondicionesParaColumna
                                If FilaCumpleCondicion(ObjDataTable.Rows(i), ObjCondicion.Condicion, ObjCondicion.EsCondicionSimple) Then
                                    CeldaAtributos = ObjCondicion.Atributos
                                End If
                            Next
                            If Tabla.Atributos.Tipo <> Tipos.SinEstilos Then
                                CeldaAtributos.AplicarAtributos(RangoActivo)
                            End If
                            If Tabla.Atributos.Grilla Then
                                RangoActivo.Borders.LineStyle = Excel.XlLineStyle.xlContinuous
                            End If
                        End If
                    Next j
                Next i
                Tabla.IndiceColumnaHasta = +Tabla.IndiceColumnaDesde + PunteroColumna
                Tabla.Alto = Tabla.IndiceFilaHasta - Tabla.IndiceFilaDesde
                Tabla.Ancho = Tabla.IndiceColumnaHasta - Tabla.IndiceColumnaDesde + 1
                Tabla.IndiceFilaHasta = Tabla.IndiceFilaDesde + ObjDataTable.Rows.Count

                If Tabla.Atributos.BordesCabecera Then
                    RangoActivo = PestaniaActiva.Range(PestaniaActiva.Cells(Tabla.IndiceFilaDesde, Tabla.IndiceColumnaDesde), PestaniaActiva.Cells(Tabla.IndiceFilaDesde, Tabla.IndiceColumnaHasta))
                End If
                PosicionarSobreVertice(Enum_Vertices.BottomLeft)
            End If
        End Sub

        Private Function EvaluarExpresion(ByVal Expresion As String) As Boolean
            Try
                Dim vbcp As New Microsoft.VisualBasic.VBCodeProvider
                Dim vbc As System.CodeDom.Compiler.ICodeCompiler = vbcp.CreateCompiler
                Dim cpar As New System.CodeDom.Compiler.CompilerParameters
                Dim res As System.CodeDom.Compiler.CompilerResults
                cpar.GenerateExecutable = False
                cpar.GenerateInMemory = True
                cpar.IncludeDebugInformation = True
                cpar.ReferencedAssemblies.Add("Microsoft.VisualBasic.dll")
                res = vbc.CompileAssemblyFromSource(cpar,
            "Imports Microsoft.VisualBasic" & vbCrLf &
            "Namespace EvaluateExpresion" & vbCrLf &
            " Public Class clsEval" & vbCrLf &
            "  Public Shared Function Eval() As Boolean " & vbCrLf &
             Expresion & vbCrLf &
            "  End Function" & vbCrLf &
            " End Class" & vbCrLf &
            "End Namespace")
                If res.Errors.Count = 0 Then
                    Dim miclase As System.Type
                    miclase = res.CompiledAssembly.GetType("EvaluateExpresion.clsEval")
                    Dim funceval As System.Reflection.MethodInfo
                    funceval = miclase.GetMethod("Eval")
                    Return funceval.Invoke(Nothing, Nothing)
                Else
                    Return False
                End If
            Catch ex As Exception
                Return False
            End Try
        End Function

        Private Function FilaCumpleCondicion(Row As DataRow, Script As String, EsCondicionSimple As Boolean) As Boolean
            Dim Expresion As String = ""
            For i = 0 To Row.Table.Columns.Count - 1
                Dim Columna As DataColumn = Row.Table.Columns(i)
                Dim CeldaValor As Object = Row.Item(i)
                Dim TipoColumna As String = Columna.DataType.Name
                Select Case TipoColumna.ToLower
                    Case "string"
                        Expresion &= "Dim " & Columna.ColumnName & " As " & TipoColumna & " = """ & CeldaValor.ToString & """" & vbCrLf
                    Case "decimal", "integer", "int64", "int32", "int", "long", "double", "float", "byte", "ushort", "uint", "ulong"
                        Expresion &= "Dim " & Columna.ColumnName & " As Decimal = " & CeldaValor.ToString.Replace(",", ".") & vbCrLf
                    Case "boolean,bool"
                        If CeldaValor Then
                            Expresion &= "Dim " & Columna.ColumnName & " As " & TipoColumna & " = True" & vbCrLf
                        Else
                            Expresion &= "Dim " & Columna.ColumnName & " As " & TipoColumna & " = False" & vbCrLf
                        End If
                    Case "datetime", "date"
                        Dim Dia As Long = Convert.ToDateTime(CeldaValor).Day
                        Dim Anio As Long = Convert.ToDateTime(CeldaValor).Year
                        Dim Mes As Long = Convert.ToDateTime(CeldaValor).Month
                        'Dim a As New DateTime(2002, 1, 1)
                        Expresion &= "Dim " & Columna.ColumnName & " As New Date(" & Anio.ToString & "," & Mes.ToString & "," & Dia.ToString & ")" & vbCrLf
                End Select
            Next
            If EsCondicionSimple Then
                Expresion &= "return (" & Script & ")" & vbCrLf
            Else
                Expresion &= Script & vbCrLf
            End If
            Return EvaluarExpresion(Expresion)
        End Function

        Public Sub AsignarDatosAGrafico(NombreGrafico As String, NombreTabla As String)
            ' https://www.youtube.com/watch?v=tvR6vaBBMnI
            Dim Graficos As Excel.ChartObjects
            Dim Tabla As ComponenteTabla
            Tabla = Tablas.Where(Function(n) n.Nombre = NombreTabla).First
            Dim RangoSourceData As Excel.Range
            RangoSourceData = PestaniaActiva.Range(PestaniaActiva.Cells(Tabla.IndiceFilaDesde + 1, Tabla.IndiceColumnaDesde), PestaniaActiva.Cells(Tabla.IndiceFilaHasta, Tabla.IndiceColumnaHasta))
            Graficos = PestaniaActiva.ChartObjects
            Dim ObjChart As Excel.Chart
            For i = 1 To Graficos.Count
                Dim Grafico = Graficos.Item(i)
                If Grafico.Name = NombreGrafico Then
                    ObjChart = Grafico.Chart
                End If
            Next
            ObjChart.SetSourceData(Source:=RangoSourceData)
        End Sub

        Public Sub Etiqueta(Nombre As String, Accion As Enum_SeleccionComponente)
            Dim Etiqueta As ComponenteEtiqueta = Me.Etiquetas.Where(Function(n) n.Nombre = Nombre).First
            ComponenteSeleccionado = Etiqueta
            If Accion = Enum_SeleccionComponente.Posicionar Then
                Me.IndiceColumnaActiva = Etiqueta.IndiceColumnaDesde
                Me.IndiceFilaActiva = Etiqueta.IndiceFilaDesde
            ElseIf Accion = Enum_SeleccionComponente.Pegar Then
                Etiqueta.IndiceColumnaDesde = Me.IndiceColumnaActiva
                Etiqueta.IndiceFilaDesde = Me.IndiceFilaActiva
                Etiqueta.IndiceColumnaHasta = Etiqueta.IndiceColumnaDesde + Etiqueta.Ancho - 1
                Etiqueta.IndiceFilaHasta = Etiqueta.IndiceFilaDesde + Etiqueta.Alto - 1
                RangoActivo = PestaniaActiva.Range(PestaniaActiva.Cells(Etiqueta.IndiceFilaDesde, Etiqueta.IndiceColumnaDesde), PestaniaActiva.Cells(Etiqueta.IndiceFilaHasta, Etiqueta.IndiceColumnaHasta))
                RangoActivo.Select()
                RangoActivo.Merge()
                RangoActivo.Value = Etiqueta.Valor
                RangoActivo.HorizontalAlignment = -4108
                RangoActivo.VerticalAlignment = -4108
                Etiqueta.Atributos.AplicarAtributos(RangoActivo)
            End If
        End Sub

        Public Sub PosicionarSobreVertice(Vertice As Enum_Vertices)
            Select Case Vertice
                Case Enum_Vertices.BottomLeft
                    Me.IndiceColumnaActiva = ComponenteSeleccionado.IndiceColumnaDesde
                    Me.IndiceFilaActiva = ComponenteSeleccionado.IndiceFilaHasta
                Case Enum_Vertices.BottomRight
                    Me.IndiceColumnaActiva = ComponenteSeleccionado.IndiceColumnaHasta
                    Me.IndiceFilaActiva = ComponenteSeleccionado.IndiceFilaHasta
                Case Enum_Vertices.TopLeft
                    Me.IndiceColumnaActiva = ComponenteSeleccionado.IndiceColumnaDesde
                    Me.IndiceFilaActiva = ComponenteSeleccionado.IndiceFilaDesde
                Case Enum_Vertices.TopRight
                    Me.IndiceColumnaActiva = ComponenteSeleccionado.IndiceColumnaHasta
                    Me.IndiceFilaActiva = ComponenteSeleccionado.IndiceFilaDesde
            End Select
        End Sub

        Public Sub ActivarPestania(Nombre As String)
            PestaniaActiva = PestaniasExcel.Where(Function(n) n.Name = Nombre).First
            PestaniaActiva.Activate()
        End Sub

        Enum Enum_SeleccionComponente
            Posicionar
            Pegar
        End Enum

        Enum Enum_Vertices
            TopLeft
            TopRight
            BottomLeft
            BottomRight
        End Enum

        Public Enum Tipos
            SinEstilos
            Basico
            Columnas_Alternadas
            Filas_Alternadas
        End Enum

        Public Class Componente
            Property Nombre As String
            Property IndiceColumnaDesde As Int64
            Property IndiceFilaDesde As Int64
            Property IndiceColumnaHasta As Int64
            Property IndiceFilaHasta As Int64
            Property Ancho As Int64
            Property Alto As Int64
        End Class

        Public Class ComponenteTabla
            Inherits Componente
            Dim _tabla As DataTable

            Property Tabla As DataTable
                Get
                    Return _tabla
                End Get
                Set(value As DataTable)
                    Me._tabla = value
                    Me.Ancho = value.Columns.Count
                    Me.Alto = value.Rows.Count
                End Set
            End Property

            Property Atributos As New AtributosTabla
        End Class

        Public Class ComponenteEtiqueta
            Inherits Componente
            Property Valor As String
            Property Atributos As New AtributosCelda
        End Class

        Public Class ComponenteGrafico
            Inherits Componente

        End Class

        Public Class AtributosTabla
            Public Sub New()
                Cabecera.Backcolor = Drawing.Color.Red
                Cabecera.ForeColor = Drawing.Color.White
                Cabecera.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter
                Grilla = True
                BordesCabecera = True
                Tipo = Tipos.Basico
            End Sub
            Property Tipo As Tipos
            Property Celdas As New AtributosCelda
            Property Pares As New AtributosCelda
            Property Impares As New AtributosCelda
            Property Cabecera As New AtributosCelda
            Property CeldasCondicionadas As New List(Of CondicionCelda)
            Property FormatosColumnas As New List(Of FormatoColumna)
            Property ConCabecera As Boolean = True
            Property Grilla As Boolean
            Property BordesCabecera As Boolean
        End Class

        Public Class CondicionCelda
            Property ColumnaAplicar As String = ""
            Property Condicion As String = ""
            Property Atributos As New AtributosCelda
            Property EsCondicionSimple As Boolean = True
            Property AplicarFilaCompleta As Boolean = False

            Public Sub New()

            End Sub
            Public Sub New(Condicion As String, Columna As String, Atributos As AtributosCelda)
                Me.ColumnaAplicar = Columna
                Me.Condicion = Condicion
                Me.Atributos = Atributos
            End Sub
            Public Sub New(Condicion As String, Atributos As AtributosCelda)
                Me.Condicion = Condicion
                Me.Atributos = Atributos
            End Sub

        End Class

        Public Class FormatoColumna
            Property NombreColumna As String
            Property NombreTextual As String
            Property Formato As String
            'ver opciones: https://www.it-swarm-es.com/es/excel/cuales-son-las-opciones-de-.numberformat-en-excel-vba/1044304345/
            Property Visible As Boolean = True
        End Class
        Public Class AtributosCelda
            Private Apply_FontName As Boolean = False
            Private Apply_FontSize As Boolean = False
            Private Apply_Italic As Boolean = False
            Private Apply_Bold As Boolean = False
            Private Apply_Underline As Boolean = False
            Private Apply_Backcolor As Boolean = False
            Private Apply_ForeColor As Boolean = False
            Private Apply_StrikeThrough As Boolean = False
            Private Apply_HorizontalAlignment As Boolean = False

            Private _Fontname As String
            Private _FontSize As Int16
            Private _Italic As Boolean
            Private _Bold As Boolean
            Private _Underline As Boolean
            Private _StrikeThrough As Boolean
            Private _ForeColor As Drawing.Color
            Private _BackColor As Drawing.Color
            Private _HorizontalAlignment As Excel.XlHAlign

            Public Property FontName() As String
                Get
                    Return _Fontname
                End Get
                Set
                    Apply_FontName = True
                    _Fontname = Value
                End Set
            End Property

            Public Property HorizontalAlignment() As Excel.XlHAlign
                Get
                    Return _HorizontalAlignment
                End Get
                Set
                    Apply_HorizontalAlignment = True
                    _HorizontalAlignment = Value
                End Set
            End Property

            Public Property FontSize() As Int16
                Get
                    Return _FontSize
                End Get
                Set
                    Apply_FontSize = True
                    _FontSize = Value
                End Set
            End Property

            Public Property Italic() As String
                Get
                    Return _Italic
                End Get
                Set
                    Apply_Italic = True
                    _Italic = Value
                End Set
            End Property


            Public Property Bold() As String
                Get
                    Return _Bold
                End Get
                Set
                    Apply_Bold = True
                    _Bold = Value
                End Set
            End Property
            Public Property Underline() As Boolean
                Get
                    Return _Underline
                End Get
                Set
                    _Underline = Value
                    Apply_Underline = True
                End Set
            End Property
            Public Property Backcolor() As Drawing.Color
                Get
                    Return _BackColor
                End Get
                Set
                    Apply_Backcolor = True
                    _BackColor = Value
                End Set
            End Property
            Public Property ForeColor() As Drawing.Color
                Get
                    Return _ForeColor
                End Get
                Set
                    Apply_ForeColor = True
                    _ForeColor = Value
                End Set
            End Property
            Public Property StrikeThrough() As String
                Get
                    Return _StrikeThrough
                End Get
                Set
                    Apply_StrikeThrough = True
                    _StrikeThrough = Value
                End Set
            End Property

            Public Sub AplicarAtributos(Celda As Excel.Range)
                If Apply_Backcolor Then
                    Celda.Interior.Color = _BackColor
                End If
                If Apply_ForeColor Then
                    Celda.Font.Color = _ForeColor
                End If
                If Apply_Italic Then
                    Celda.Font.IsItalic = _Italic
                End If
                If Apply_StrikeThrough Then
                    Celda.Font.Strikethrough = _StrikeThrough
                End If
                If Apply_Underline Then
                    Celda.Font.Underline = _Underline
                End If
                If Apply_FontSize Then
                    Celda.Font.Size = _FontSize
                End If
                If Apply_Bold Then
                    Celda.Font.Bold = _Bold
                End If
                If Apply_FontName Then
                    Celda.Font.FontName = _Fontname
                End If
                If Apply_HorizontalAlignment Then
                    Celda.HorizontalAlignment = _HorizontalAlignment
                End If
            End Sub
        End Class
    End Class
End Namespace


