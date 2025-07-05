Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports FrameWork.Gestores.Extensiones
Imports System.Windows.Forms
Imports System.Collections
Imports System.Linq


Public Class Frm_JsonViewer
    Dim JsonString As String
    Private Sub Frm_JsonViewer_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Public Sub MostrarJson(Objeto As Object, Optional root As String = "JSON")
        MostrarJson(JsonConvert.SerializeObject(Objeto), root)
    End Sub

    Public Sub MostrarJson(json As String, Optional root As String = "JSON")
        JsonString = json
        Dim reader As JsonTextReader = New JsonTextReader(New StringReader(json))
        Dim ActiveNode As TreeNode
        Dim NewNode As TreeNode
        ActiveNode = Tree_JSon.Nodes.Add("root", root)
        ActiveNode.ImageKey = "Json"
        ActiveNode.SelectedImageKey = "Json"
        Dim Consola As String = ""
        Dim Pila As Stack = New Stack()
        While reader.Read()
            Consola += reader.TokenType.ToString()
            If Not reader.Value Is Nothing Then
                Consola += ":" + reader.Value.ToString + vbCrLf
            End If
            Consola += vbCrLf
            Select Case reader.TokenType
                Case JsonToken.StartObject
                    If PilaRecorriendoArray(Pila) Then
                        ActiveNode = ActiveNode.Nodes.Add("{ ... }")
                        ActiveNode.ImageKey = "Object"
                        ActiveNode.SelectedImageKey = "Object"
                    End If
                    Pila.Push("Object")
                Case JsonToken.EndObject
                    Pila.Pop()
                    ActiveNode = ActiveNode.Parent
                Case JsonToken.StartArray
                    ActiveNode.Text = ActiveNode.Text
                    ActiveNode.ImageKey = "Array"
                    ActiveNode.SelectedImageKey = "Array"
                    Pila.Push("Array")
                Case JsonToken.EndArray
                    ActiveNode.Text = ActiveNode.Text + " [" + ActiveNode.Nodes.Count.ToString + "]"
                    ActiveNode = ActiveNode.Parent
                    Pila.Pop()
                Case JsonToken.StartConstructor
                    MsgBox("Corrija el codigo, Start Constructuctor no esta contemplado")
                Case JsonToken.EndConstructor
                    MsgBox("Corrija el codigo, End Constructuctor no esta contemplado")
                Case JsonToken.PropertyName
                    ActiveNode = ActiveNode.Nodes.Add(reader.Value)
                    ActiveNode.ImageKey = "Object"
                    ActiveNode.SelectedImageKey = "Object"
                Case JsonToken.String
                    If PilaRecorriendoArray(Pila) Then
                        Dim NodoArray = ActiveNode.Nodes.Add("""" + reader.Value + """")
                        NodoArray.ImageKey = "String"
                        NodoArray.SelectedImageKey = "String"
                    Else
                        ActiveNode.Text = ActiveNode.Text + ": """ + reader.Value + """"
                        ActiveNode.ImageKey = "String"
                        ActiveNode.SelectedImageKey = "String"
                        ActiveNode = ActiveNode.Parent
                    End If
                Case JsonToken.Boolean
                    If PilaRecorriendoArray(Pila) Then
                        Dim NodoArray = ActiveNode.Nodes.Add(reader.Value.ToString)
                        NodoArray.ImageKey = "Boolean"
                        NodoArray.SelectedImageKey = "Boolean"
                    Else
                        ActiveNode.Text = ActiveNode.Text + ": " + reader.Value.ToString
                        ActiveNode.ImageKey = "Boolean"
                        ActiveNode.SelectedImageKey = "Boolean"
                        ActiveNode = ActiveNode.Parent
                    End If
                Case JsonToken.Date
                    If PilaRecorriendoArray(Pila) Then
                        Dim NodoArray = ActiveNode.Nodes.Add(Convert.ToDateTime(reader.Value).ToString("dd/MM/yyyy hh:mm:ss"))
                        NodoArray.ImageKey = "Date"
                        NodoArray.SelectedImageKey = "Date"
                    Else
                        ActiveNode.Text = ActiveNode.Text + ": " + Convert.ToDateTime(reader.Value).ToString("dd/MM/yyyy hh:mm:ss")
                        ActiveNode.ImageKey = "Date"
                        ActiveNode.SelectedImageKey = "Date"
                        ActiveNode = ActiveNode.Parent
                    End If
                Case JsonToken.Float, JsonToken.Integer
                    If PilaRecorriendoArray(Pila) Then
                        Dim NodoArray = ActiveNode.Nodes.Add(reader.Value.ToString)
                        NodoArray.ImageKey = "Number"
                        NodoArray.SelectedImageKey = "Number"
                    Else
                        ActiveNode.Text = ActiveNode.Text + ": " + reader.Value.ToString
                        ActiveNode.ImageKey = "Number"
                        ActiveNode.SelectedImageKey = "Number"
                        ActiveNode = ActiveNode.Parent
                    End If
                Case JsonToken.Null
                    If PilaRecorriendoArray(Pila) Then
                        Dim NodoArray = ActiveNode.Nodes.Add("Null")
                        NodoArray.ImageKey = "Null"
                        NodoArray.SelectedImageKey = "Null"
                    Else
                        ActiveNode.Text = ActiveNode.Text + " Null"
                        ActiveNode.ImageKey = "Null"
                        ActiveNode.SelectedImageKey = "Null"
                        ActiveNode = ActiveNode.Parent
                    End If
                Case JsonToken.Undefined
                    If PilaRecorriendoArray(Pila) Then
                        Dim NodoArray = ActiveNode.Nodes.Add("Undefined")
                        NodoArray.ImageKey = "Null"
                        NodoArray.SelectedImageKey = "Null"
                    Else
                        ActiveNode.Text = ActiveNode.Text + " Undefined"
                        ActiveNode.ImageKey = "Null"
                        ActiveNode.SelectedImageKey = "Null"
                        ActiveNode = ActiveNode.Parent
                    End If
                Case JsonToken.Raw
                    MsgBox("Corrija el codigo, Raw no esta contemplado")
                Case JsonToken.Bytes
                    MsgBox("Corrija el codigo, Bytes no esta contemplado")
            End Select
        End While
        Tree_JSon.ExpandAll()
        Me.Show()
    End Sub

    Public Function PilaRecorriendoArray(Pila As Stack)
        If Pila.Count = 0 Then
            Return False
        End If
        Return Pila.Peek = "Array"
    End Function
    Public Function PilaRecorriendoObjeto(Pila As Stack)
        If Pila.Count = 0 Then
            Return False
        End If
        Return Pila.Peek = "Object"
    End Function

    Private Sub Tree_JSon_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles Tree_JSon.AfterSelect

    End Sub

    Private Sub Mnu_CopiarValor_Click(sender As Object, e As EventArgs) Handles Mnu_CopiarValor.Click
        Dim Valor As String = ""
        Select Case Tree_JSon.SelectedNode.ImageKey
            Case "Object", "Array", "Json"
                CopiarArbol(Tree_JSon.SelectedNode, Valor, Tree_JSon.SelectedNode.Level)
                Clipboard.SetText(Valor)
            Case Else
                Valor = Trim(Join(Tree_JSon.SelectedNode.Text.Split(":").ToList.Where(Function(n, index) index > 0).ToArray, ":")).Trim("""")
                Clipboard.SetText(Valor)
        End Select

    End Sub

    Private Sub Mnu_CopiarClaveValor_Click(sender As Object, e As EventArgs) Handles Mnu_CopiarClaveValor.Click
        Dim Valor As String = ""
        Select Case Tree_JSon.SelectedNode.ImageKey
            Case "Object", "Array", "Json"
                CopiarArbol(Tree_JSon.SelectedNode, Valor, Tree_JSon.SelectedNode.Level)
                Clipboard.SetText(Valor)
            Case Else
                Clipboard.SetText(Tree_JSon.SelectedNode.Text)
        End Select

    End Sub
    Private Sub CopiarArbol(Nodo As TreeNode, ByRef Texto As String, NivelInferior As Long)
        For i = NivelInferior To Nodo.Level
            Texto = Texto + vbTab
        Next
        Texto = Texto + Nodo.Text + vbCrLf
        For Each SubNodo As TreeNode In Nodo.Nodes
            CopiarArbol(SubNodo, Texto, NivelInferior)
        Next
    End Sub

    Private Sub Mnu_CopiarJson_Click(sender As Object, e As EventArgs) Handles Mnu_CopiarJson.Click
        Clipboard.SetText(JsonString)
    End Sub
End Class