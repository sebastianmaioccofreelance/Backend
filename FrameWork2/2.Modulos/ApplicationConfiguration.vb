Imports System.Collections.Generic
Imports FrameWork.Gestores

Namespace Modulos
    Public Module Sesion
        Public SesionActual As New InfoUserSession()
    End Module

    Public Module Mod_ApplicationConfiguration

        Public AppConfig As New Dictionary(Of String, Object)
        Public Sub Setear()
            If Not AppConfig("RutaTraces") Is Nothing Then
                If Not IO.Directory.Exists(AppConfig("RutaTraces")) Then
                    IO.Directory.CreateDirectory(AppConfig("RutaTraces"))
                End If
            End If
            If Not AppConfig("RutaTemporales") Is Nothing Then
                If Not IO.Directory.Exists(AppConfig("RutaTemporales")) Then
                    IO.Directory.CreateDirectory(AppConfig("RutaTemporales"))
                End If
            End If
        End Sub
    End Module
End Namespace