<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Frm_SQLToJSon
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
        Me.Txt_Json = New System.Windows.Forms.TextBox()
        Me.Txt_Query = New System.Windows.Forms.TextBox()
        Me.Lbl_SQL = New System.Windows.Forms.Label()
        Me.Lbl_Json = New System.Windows.Forms.Label()
        Me.But_Ejecutar = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Txt_Json
        '
        Me.Txt_Json.Location = New System.Drawing.Point(83, 115)
        Me.Txt_Json.Multiline = True
        Me.Txt_Json.Name = "Txt_Json"
        Me.Txt_Json.Size = New System.Drawing.Size(653, 255)
        Me.Txt_Json.TabIndex = 0
        '
        'Txt_Query
        '
        Me.Txt_Query.Location = New System.Drawing.Point(83, 12)
        Me.Txt_Query.Multiline = True
        Me.Txt_Query.Name = "Txt_Query"
        Me.Txt_Query.Size = New System.Drawing.Size(653, 97)
        Me.Txt_Query.TabIndex = 1
        '
        'Lbl_SQL
        '
        Me.Lbl_SQL.AutoSize = True
        Me.Lbl_SQL.Location = New System.Drawing.Point(16, 15)
        Me.Lbl_SQL.Name = "Lbl_SQL"
        Me.Lbl_SQL.Size = New System.Drawing.Size(26, 13)
        Me.Lbl_SQL.TabIndex = 2
        Me.Lbl_SQL.Text = "SQL"
        '
        'Lbl_Json
        '
        Me.Lbl_Json.AutoSize = True
        Me.Lbl_Json.Location = New System.Drawing.Point(16, 115)
        Me.Lbl_Json.Name = "Lbl_Json"
        Me.Lbl_Json.Size = New System.Drawing.Size(30, 13)
        Me.Lbl_Json.TabIndex = 3
        Me.Lbl_Json.Text = "Json"
        '
        'But_Ejecutar
        '
        Me.But_Ejecutar.Location = New System.Drawing.Point(742, 15)
        Me.But_Ejecutar.Name = "But_Ejecutar"
        Me.But_Ejecutar.Size = New System.Drawing.Size(119, 360)
        Me.But_Ejecutar.TabIndex = 4
        Me.But_Ejecutar.Text = "Ejecutar"
        Me.But_Ejecutar.UseVisualStyleBackColor = True
        '
        'Frm_SQLToJSon
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(867, 376)
        Me.Controls.Add(Me.But_Ejecutar)
        Me.Controls.Add(Me.Lbl_Json)
        Me.Controls.Add(Me.Lbl_SQL)
        Me.Controls.Add(Me.Txt_Query)
        Me.Controls.Add(Me.Txt_Json)
        Me.Name = "Frm_SQLToJSon"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Sql To Json"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Txt_Json As Windows.Forms.TextBox
    Friend WithEvents Txt_Query As Windows.Forms.TextBox
    Friend WithEvents Lbl_SQL As Windows.Forms.Label
    Friend WithEvents Lbl_Json As Windows.Forms.Label
    Friend WithEvents But_Ejecutar As Windows.Forms.Button
End Class
