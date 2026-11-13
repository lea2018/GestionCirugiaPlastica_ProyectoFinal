using Aplicada1.Core;
using GestionCirugiaPlastica.Infrastructure.Data;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GestionCirugiaPlastica.Application.Services;

public class RolesServices(
    IDbContextFactory<GestionCirugiaPlasticaContext> contextFactory
) : IServiceResult<Role, int>
{
    private const string RolAdministrador = "Administrador";

    // 1. Guardar o actualizar un rol
    public async Task<Result> Guardar(Role rol)
    {
        if (rol == null ||
            string.IsNullOrWhiteSpace(rol.Nombre) ||
            rol.Nombre.Trim().Length > 250 ||
            rol.Descripcion?.Length > 250)
        {
            return Result.Success(false);
        }

        await using var contexto = await contextFactory.CreateDbContextAsync();

        string nombre = rol.Nombre.Trim();

        bool nombreExiste = await contexto.Roles
            .AnyAsync(r =>
                r.Nombre.ToLower() == nombre.ToLower() &&
                r.RolId != rol.RolId);

        if (nombreExiste)
            return Result.Success(false);

        if (rol.RolId == 0)
        {
            var nuevoRol = new Role
            {
                Nombre = nombre,
                Descripcion = rol.Descripcion?.Trim()
            };

            contexto.Roles.Add(nuevoRol);
        }
        else
        {
            var existente = await contexto.Roles
                .FirstOrDefaultAsync(r => r.RolId == rol.RolId);

            if (existente == null)
                return Result.Success(false);

            // No permitir cambiar el nombre del último rol administrador
            if (existente.Nombre.Equals(
                RolAdministrador,
                StringComparison.OrdinalIgnoreCase) &&
                !nombre.Equals(
                RolAdministrador,
                StringComparison.OrdinalIgnoreCase))
            {
                bool existeOtroAdministrador = await contexto.Roles
                    .AnyAsync(r =>
                        r.RolId != rol.RolId &&
                        r.Nombre.ToLower() == "administrador");

                if (!existeOtroAdministrador)
                    return Result.Success(false);
            }

            existente.Nombre = nombre;
            existente.Descripcion = rol.Descripcion?.Trim();
        }

        return Result.Success(await contexto.SaveChangesAsync() > 0);
    }

    // 2. Buscar rol por ID
    public async Task<Result<Role?>> Buscar(int rolId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var rol = await contexto.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.RolId == rolId);

        return Result<Role?>.Success(rol);
    }

    // 3. Obtener roles según un criterio
    public async Task<Result<List<Role>>> GetList(
        Expression<Func<Role, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var roles = await contexto.Roles
            .AsNoTracking()
            .Where(criterio)
            .OrderBy(r => r.Nombre)
            .ToListAsync();

        return Result<List<Role>>.Success(roles);
    }

    // 4. Buscar roles por nombre
    public async Task<Result<List<Role>>> BuscarPorNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return Result<List<Role>>.Success(new List<Role>());

        await using var contexto = await contextFactory.CreateDbContextAsync();

        nombre = nombre.Trim();

        var roles = await contexto.Roles
            .AsNoTracking()
            .Where(r => r.Nombre.Contains(nombre))
            .OrderBy(r => r.Nombre)
            .ToListAsync();

        return Result<List<Role>>.Success(roles);
    }

    // 5. Buscar el rol asignado a un usuario
    public async Task<Result<Role?>> BuscarPorUsuario(int usuarioId)
    {
        if (usuarioId <= 0)
            return Result<Role?>.Success((Role?)null);

        await using var contexto = await contextFactory.CreateDbContextAsync();

        var rol = await contexto.Usuarios
            .AsNoTracking()
            .Where(u => u.UsuarioId == usuarioId)
            .Select(u => u.Rol)
            .FirstOrDefaultAsync();

        return Result<Role?>.Success(rol);
    }

    // 6. Buscar usuarios pertenecientes a un rol
    public async Task<Result<List<Usuario>>> BuscarUsuarios(int rolId)
    {
        if (rolId <= 0)
            return Result<List<Usuario>>.Success(new List<Usuario>());

        await using var contexto = await contextFactory.CreateDbContextAsync();

        var usuarios = await contexto.Usuarios
            .AsNoTracking()
            .Where(u => u.RolId == rolId)
            .OrderBy(u => u.Nombre)
            .ThenBy(u => u.Apellido)
            .Select(u => new Usuario
            {
                UsuarioId = u.UsuarioId,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Email = u.Email,
                RolId = u.RolId,
                Activo = u.Activo,
                FechaRegistro = u.FechaRegistro
            })
            .ToListAsync();

        return Result<List<Usuario>>.Success(usuarios);
    }

    // 7. Comprobar si un rol tiene usuarios asignados
    public async Task<Result<bool>> TieneUsuarios(int rolId)
    {
        if (rolId <= 0)
            return Result<bool>.Success(false);

        await using var contexto = await contextFactory.CreateDbContextAsync();

        bool tieneUsuarios = await contexto.Usuarios
            .AnyAsync(u => u.RolId == rolId);

        return Result<bool>.Success(tieneUsuarios);
    }

    // 8. Contar usuarios asignados a un rol
    public async Task<Result<int>> ContarUsuarios(int rolId)
    {
        if (rolId <= 0)
            return Result<int>.Success(0);

        await using var contexto = await contextFactory.CreateDbContextAsync();

        int cantidad = await contexto.Usuarios
            .CountAsync(u => u.RolId == rolId);

        return Result<int>.Success(cantidad);
    }

    // 9. Comprobar si existe otro administrador activo
    public async Task<Result<bool>> ExisteAdministradorActivo()
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        bool existe = await contexto.Usuarios
            .AnyAsync(u =>
                u.Activo &&
                u.Rol.Nombre.ToLower() == "administrador");

        return Result<bool>.Success(existe);
    }

    // 10. No permitir eliminar roles asignados o el último administrador
    public async Task<Result> Eliminar(int rolId)
    {
        if (rolId <= 0)
            return Result.Success(false);

        await using var contexto = await contextFactory.CreateDbContextAsync();

        var rol = await contexto.Roles
            .FirstOrDefaultAsync(r => r.RolId == rolId);

        if (rol == null)
            return Result.Success(false);

        bool tieneUsuarios = await contexto.Usuarios
            .AnyAsync(u => u.RolId == rolId);

        if (tieneUsuarios)
            return Result.Success(false);

        // Proteger el último rol administrador
        if (rol.Nombre.Equals(
            RolAdministrador,
            StringComparison.OrdinalIgnoreCase))
        {
            bool existeOtroAdministrador = await contexto.Usuarios
                .AnyAsync(u =>
                    u.Activo &&
                    u.RolId != rolId &&
                    u.Rol.Nombre.ToLower() == "administrador");

            if (!existeOtroAdministrador)
                return Result.Success(false);
        }

        contexto.Roles.Remove(rol);

        return Result.Success(await contexto.SaveChangesAsync() > 0);
    }
}