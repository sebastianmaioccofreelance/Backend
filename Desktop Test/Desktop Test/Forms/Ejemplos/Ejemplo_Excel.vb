Imports System
Imports System.Collections
Imports System.Collections.Specialized
Imports System.Diagnostics
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Text
Imports System.Threading
Imports FrameWork
Imports FrameWork.Bases
Imports FrameWork.Gestores
Imports Newtonsoft.Json.Linq

Imports Test_Services
Imports FrameWork.Bases.DTORequests
Imports FrameWork.Bases.DTOResponses
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.Collections.Generic
Imports System.Data
Imports System.Linq

'Imports Microsoft.Office.Interop.Excel
Public Class Ejemplo_Excel
    Private Sub Frm_Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        EjemploExcel()
    End Sub
    Private Sub EjemploExcel()
        Dim NuevoExcel As New Ges_Excel
        'etiqueta 1
        Dim Noviembre As New Ges_Excel.ComponenteEtiqueta
        Noviembre.Nombre = "Noviembre"
        Noviembre.Valor = "Mes de noviembre"
        Noviembre.Ancho = 2
        Noviembre.Alto = 1
        Noviembre.Atributos.ForeColor = System.Drawing.Color.Red
        Noviembre.Atributos.Backcolor = System.Drawing.Color.Green
        NuevoExcel.Etiquetas.Add(Noviembre)
        ' Tabla 
        Dim dt As New System.Data.DataTable
        dt.Columns.Add("nombre")
        dt.Columns.Add("apellido")
        dt.Columns.Add("Fecha_nacimiento", System.Type.GetType("System.DateTime"))
        dt.Columns.Add("Puntos")
        Dim TablaNueva As New Ges_Excel.ComponenteTabla
        TablaNueva.Tabla = dt
        TablaNueva.Nombre = "Proveedores"
        Dim AtributosTabla As New Ges_Excel.AtributosTabla
        TablaNueva.Atributos = AtributosTabla
        AtributosTabla.ConCabecera = True
        With AtributosTabla
            AtributosTabla.Cabecera = New Ges_Excel.AtributosCelda With
                {
            .Backcolor = Color.Red,
            .ForeColor = Color.White,
            .HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter,
            .Bold = True,
            .Underline = False
            }

            AtributosTabla.Celdas = New Ges_Excel.AtributosCelda With
                {
            .ForeColor = Color.Gray
            }

            AtributosTabla.Tipo = Ges_Excel.Tipos.Basico

            AtributosTabla.FormatosColumnas.Add(New Ges_Excel.FormatoColumna With
               {
                   .NombreColumna = "Fecha_nacimiento",
                   .Formato = "dd-MM-yyyy",
                   .Visible = True,
                   .NombreTextual = "Fecha de nacimiento"
               }
            )
        End With
        Dim PintarApellido As New Ges_Excel.CondicionCelda
        With PintarApellido
            .Condicion = "Nombre=""Sebastian"""
            .Atributos = New Ges_Excel.AtributosCelda With
                {.Backcolor = Color.Green,
                .ForeColor = Color.White}
            .AplicarFilaCompleta = True
        End With

        Dim NuevaFila As DataRow = dt.NewRow()
        NuevaFila("Nombre") = "Sebastian"
        NuevaFila("Apellido") = "Maiocco"
        NuevaFila("Fecha_nacimiento") = New DateTime(1981, 10, 9)
        NuevaFila("Puntos") = 20
        dt.Rows.Add(NuevaFila)

        NuevaFila = dt.NewRow
        NuevaFila("Nombre") = "Yamila"
        NuevaFila("Apellido") = "Olguin"
        NuevaFila("Fecha_nacimiento") = New DateTime(1981, 2, 23)
        NuevaFila("Puntos") = 15
        dt.Rows.Add(NuevaFila)

        NuevaFila = dt.NewRow
        NuevaFila("Nombre") = "Pablo"
        NuevaFila("Apellido") = "Maiocco"
        NuevaFila("Fecha_nacimiento") = New DateTime(1980, 7, 26)
        NuevaFila("Puntos") = 11
        dt.Rows.Add(NuevaFila)

        NuevaFila = dt.NewRow
        NuevaFila("Nombre") = "Julio"
        NuevaFila("Apellido") = "Maiocco"
        NuevaFila("Fecha_nacimiento") = New DateTime(1954, 3, 10)
        NuevaFila("Puntos") = 2
        dt.Rows.Add(NuevaFila)

        NuevoExcel.Tablas.Add(TablaNueva)
        ' crear excel

        NuevoExcel.Pestanias.Add("Noviembre")
        NuevoExcel.Pestanias.Add("Noviembre2")
        NuevoExcel.AbrirArchivo("e:\gestor de excel ejemplo.xlsx")
        'NuevoExcel.NuevoArchivo()
        NuevoExcel.Etiqueta("Noviembre", Ges_Excel.Enum_SeleccionComponente.Pegar)
        NuevoExcel.Abajo(1)
        NuevoExcel.Derecha(3)
        NuevoExcel.Tabla("Proveedores", Ges_Excel.Enum_SeleccionComponente.Pegar)
        NuevoExcel.AsignarDatosAGrafico("Puntos", "Proveedores")
        NuevoExcel.TerminarHojas()
        NuevoExcel.ExportarPDF("e:\mipdf.pdf", True)
        NuevoExcel.FinalizarSesion()
        System.Windows.Forms.Application.Exit()
    End Sub
    Private Sub Frm_Main_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        Hide()
    End Sub
End Class