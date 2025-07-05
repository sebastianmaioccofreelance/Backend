Imports System.Windows.Forms
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Frm_JsonViewer
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Frm_JsonViewer))
        Me.Tree_JSon = New System.Windows.Forms.TreeView()
        Me.Mnu_Context = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.Mnu_CopiarValor = New System.Windows.Forms.ToolStripMenuItem()
        Me.Mnu_CopiarClaveValor = New System.Windows.Forms.ToolStripMenuItem()
        Me.Mnu_CopiarJson = New System.Windows.Forms.ToolStripMenuItem()
        Me.Iml_Iconos = New System.Windows.Forms.ImageList(Me.components)
        Me.Mnu_Context.SuspendLayout()
        Me.SuspendLayout()
        '
        'Tree_JSon
        '
        Me.Tree_JSon.ContextMenuStrip = Me.Mnu_Context
        Me.Tree_JSon.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Tree_JSon.ImageIndex = 2
        Me.Tree_JSon.ImageList = Me.Iml_Iconos
        Me.Tree_JSon.Location = New System.Drawing.Point(0, 0)
        Me.Tree_JSon.Name = "Tree_JSon"
        Me.Tree_JSon.SelectedImageIndex = 0
        Me.Tree_JSon.Size = New System.Drawing.Size(357, 348)
        Me.Tree_JSon.TabIndex = 0
        '
        'Mnu_Context
        '
        Me.Mnu_Context.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.Mnu_CopiarValor, Me.Mnu_CopiarClaveValor, Me.Mnu_CopiarJson})
        Me.Mnu_Context.Name = "Mnu_Context"
        Me.Mnu_Context.Size = New System.Drawing.Size(178, 70)
        Me.Mnu_Context.Text = "Mnu_Context"
        '
        'Mnu_CopiarValor
        '
        Me.Mnu_CopiarValor.Name = "Mnu_CopiarValor"
        Me.Mnu_CopiarValor.Size = New System.Drawing.Size(177, 22)
        Me.Mnu_CopiarValor.Text = "Copiar valor"
        '
        'Mnu_CopiarClaveValor
        '
        Me.Mnu_CopiarClaveValor.Name = "Mnu_CopiarClaveValor"
        Me.Mnu_CopiarClaveValor.Size = New System.Drawing.Size(177, 22)
        Me.Mnu_CopiarClaveValor.Text = "Copiar clave y valor"
        '
        'Mnu_CopiarJson
        '
        Me.Mnu_CopiarJson.Name = "Mnu_CopiarJson"
        Me.Mnu_CopiarJson.Size = New System.Drawing.Size(177, 22)
        Me.Mnu_CopiarJson.Text = "Copiar Json"
        '
        'Iml_Iconos
        '
        Me.Iml_Iconos.ImageStream = CType(resources.GetObject("Iml_Iconos.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.Iml_Iconos.TransparentColor = System.Drawing.Color.Transparent
        Me.Iml_Iconos.Images.SetKeyName(0, "String")
        Me.Iml_Iconos.Images.SetKeyName(1, "Json")
        Me.Iml_Iconos.Images.SetKeyName(2, "Boolean")
        Me.Iml_Iconos.Images.SetKeyName(3, "Object")
        Me.Iml_Iconos.Images.SetKeyName(4, "Array")
        Me.Iml_Iconos.Images.SetKeyName(5, "Number")
        Me.Iml_Iconos.Images.SetKeyName(6, "Null")
        Me.Iml_Iconos.Images.SetKeyName(7, "Date")
        '
        'Frm_JsonViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(357, 348)
        Me.Controls.Add(Me.Tree_JSon)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Frm_JsonViewer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Visor JSON"
        Me.Mnu_Context.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Tree_JSon As TreeView
    Friend WithEvents Iml_Iconos As ImageList
    Friend WithEvents Mnu_Context As ContextMenuStrip
    Friend WithEvents Mnu_CopiarValor As ToolStripMenuItem
    Friend WithEvents Mnu_CopiarClaveValor As ToolStripMenuItem
    Friend WithEvents Mnu_CopiarJson As ToolStripMenuItem
End Class
