Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity
Imports FrameWork.Security
Imports FrameWork.Trace
Imports FrameWork.DAO
Imports System.Data.Entity.Infrastructure

Namespace Bases
    Public Class MyContextFactory
        Implements IDbContextFactory(Of FW_BaseDBContext)
        Public Function Create() As FW_BaseDBContext
            Return New FW_BaseDBContext("Test")
        End Function

        Private Function IDbContextFactory_Create() As FW_BaseDBContext Implements IDbContextFactory(Of FW_BaseDBContext).Create
            Return New FW_BaseDBContext("Test")
        End Function
    End Class

    Public Class FW_BaseDBContext
        Inherits DbContext
        Public Sub New(ConnectonStringName As String)
            MyBase.New(ConnectonStringName)
            ' comando para actualizar Base de datos y Entitiy Framework
            '           Enable-Migrations -ContextTypeName FrameWork.Bases.FW_BaseDBContext -force
            Database.SetInitializer(Of FW_BaseDBContext)(Nothing)
        End Sub
        Property UsersCache As DbSet(Of DAO_UsuarioCache)
        Property Usuarios As DbSet(Of DAO_Usuario)
        Property UsuariosRoles As DbSet(Of DAO_UsuarioRol)
        Property Permisos As DbSet(Of DAO_Permiso)
        Property UsuariosPermisos As DbSet(Of DAO_UsuarioPermiso)
        Property Roles As DbSet(Of DAO_Rol)
        Property TraceAcciones As DbSet(Of DAO_TraceTransaction)
        Property TraceAccionesDetalle As DbSet(Of DAO_TraceAccionesDetalle)
        Property TraceExceptions As DbSet(Of DAO_TraceException)
        Property TraceInformation As DbSet(Of DAO_TraceInformacion)
        Property SesionesUsuario As DbSet(Of DAO_SesionUsuario)
        Property SocialLogin As DbSet(Of DAO_SocialLogin)
        Property ConfigurationsItems As DbSet(Of DAO_Configuration)
        Property UsuarioPreferencias As DbSet(Of DAO_UsuarioPreferencias)
        Property GruposDeUsuarios As DbSet(Of DAO_GrupoDeUsuarios)
        Property GruposDeUsuariosUsuarios As DbSet(Of DAO_UsuarioGrupoDeUsuarios)
        Property RolesGruposDeUsuarios As DbSet(Of DAO_RolGrupoDeUsuarios)
        Property PermisosGruposDeUsuarios As DbSet(Of DAO_PermisoGrupoDeUsuarios)
        Property RolesPermisos As DbSet(Of DAO_RolPermiso)
        Property Idiomas As DbSet(Of DAO_Idioma)
    End Class

    Public Class FW_BaseDBContextTrace
        Inherits DbContext
        Public Sub New(ConnectonStringName As String)
            MyBase.New(ConnectonStringName)
            Database.SetInitializer(Of FW_BaseDBContextTrace)(Nothing)
        End Sub

        Property TraceAcciones As DbSet(Of DAO_TraceTransaction)
        Property TraceAccionesDetalle As DbSet(Of DAO_TraceAccionesDetalle)
        Property TraceExceptions As DbSet(Of DAO_TraceException)
        Property TraceInformation As DbSet(Of DAO_TraceInformacion)
    End Class
End Namespace



