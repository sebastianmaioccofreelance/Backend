Imports FrameWork.Bases
Imports FrameWork.DAO
Imports System.Linq
Imports System.Collections.Generic
Imports System.Data.Entity.Migrations


Namespace Gestores
    Public Class Ges_ApplicationConfiguration
        Inherits FW_BaseGestor
        Public Sub New(Transaction As FW_BaseTransaction)
            MyBase.New(Transaction)
        End Sub

        Public Sub New(ContextDB As Bases.FW_BaseDBContext)
            MyBase.New(ContextDB)
        End Sub

        Public Function ListarTodos() As List(Of DAO_Configuration)
            Dim ItemsConfiguracionObtener As List(Of DAO_Configuration)
            ItemsConfiguracionObtener = ContextDB.ConfigurationsItems.ToList
            Return ItemsConfiguracionObtener
        End Function

        Public Function ListarCategoria(Categoria As String) As List(Of DAO_Configuration)
            Dim ItemsConfiguracionObtener As List(Of DAO_Configuration)
            ItemsConfiguracionObtener = (From ItemsConfiguration
                              In ContextDB.ConfigurationsItems
                                         Where ItemsConfiguration.Categoria = Categoria
                                         Select ItemsConfiguration).ToList
            Return ItemsConfiguracionObtener
        End Function

        Public Function Obtener(Key As String) As DAO_Configuration
            Dim ItemConfiguracionObtener As DAO_Configuration
            ItemConfiguracionObtener = (From ItemsConfiguration
                              In ContextDB.ConfigurationsItems
                                        Where ItemsConfiguration.Key = Key
                                        Select ItemsConfiguration).FirstOrDefault
            Return ItemConfiguracionObtener
        End Function

        Public Function Guardar(ItemConfiguration As DAO_Configuration) As Boolean
            ContextDB.ConfigurationsItems.AddOrUpdate(ItemConfiguration)
            Return True
        End Function

        Public Function Eliminar(Key As String) As Boolean
            Dim ServicioConfiguration As New Ges_ApplicationConfiguration(Me.Transaction)
            Dim ItemConfiguration As DAO_Configuration
            ItemConfiguration = ServicioConfiguration.Obtener(Key)
            If Not IsNothing(ItemConfiguration) Then
                ContextDB.ConfigurationsItems.Remove(ItemConfiguration)
            End If
            Return True
        End Function
    End Class
End Namespace

