use Test
--------------------------- Entidad -------------------------

SELECT 'usuario' as tabla,* FROM Usuarios
SELECT 'rol' as tabla,* FROM [Test].[dbo].[Roles]
SELECT 'grupo' as tabla,* FROM [Test].[dbo].GruposUsuarios
SELECT 'permiso' as tabla,* FROM [Test].[dbo].Permisos

--------------------------- Relaciones -------------------------

select 
	r.NombreRol as 'rol'
	,p.Nombre   as 'permiso'
	from RolesPermisos pr 
	inner join Roles r on r.Id=Pr.RolId 
	inner join Permisos p on P.Id = Pr.IdPermiso 

select 
	r.NombreRol as 'rol',
	u.NombreCompleto as 'usuario'
	from RolesUsuarios ru
	inner join roles r on r.Id = ru.IdRol 
	inner join Usuarios u on ru.IdUsuario = u.Id


select 
	u.NombreCompleto as 'usuario'
	,p.Nombre   as 'permiso'
	from UsuariosPermisos  up 
	inner join Usuarios  u on u.Id=up.IdUsuario 
	inner join Permisos p on P.Id = up.IdPermiso 




