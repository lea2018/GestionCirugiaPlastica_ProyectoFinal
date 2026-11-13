using Aplicada1.Core;
using GestionCirugiaPlastica.Infrastructure.Data;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GestionCirugiaPlastica.Application.Services;

public class UsuariosServices(
    IDbContextFactory<GestionCirugiaPlasticaContext> contextFactory
) : IServiceResult<Usuario, int>
{
    private readonly PasswordHasher<Usuario> passwordHasher = new();

    // 1. Registrar o actualizar usuario
    public async Task<Result> Guardar(Usuario usuario)
    {
        if (usuario == null ||
            string.IsNullOrWhiteSpace(usuario.Nombre) ||
            string.IsNullOrWhiteSpace(usuario.Apellido) ||
            string.IsNullOrWhiteSpace(usuario.Email) ||
            usuario.Nombre.Trim().Length > 250 ||
            usuario.Apellido.Trim().Length > 250 ||
            usuario.Email.Trim().Length > 250)
        {
            return Result.Success(false);
        }

        await using var contexto = await contextFactory.CreateDbContextAsync();

        string email = usuario.Email.Trim().ToLowerInvariant();

        bool emailExiste = await contexto.Usuarios
            .AnyAsync(u =>
                u.Email.ToLower() == email &&
                u.UsuarioId != usuario.UsuarioId);

        if (emailExiste)
            return Result.Success(false);

        bool rolExiste = await contexto.Roles
            .AnyAsync(r => r.RolId == usuario.RolId);

        if (!rolExiste)
            return Result.Success(false);

        if (usuario.UsuarioId == 0)
        {
            // Al registrar, PasswordHash debe contener
            // la contraseña original que se desea proteger.
            if (string.IsNullOrWhiteSpace(usuario.PasswordHash) ||
                usuario.PasswordHash.Length < 12)
            {
                return Result.Success(false);
            }

            var nuevoUsuario = new Usuario
            {
                FechaRegistro = DateTime.Now,
                Nombre = usuario.Nombre.Trim(),
                Apellido = usuario.Apellido.Trim(),
                Email = email,
                RolId = usuario.RolId,
                Activo = true
            };

            nuevoUsuario.PasswordHash =
                passwordHasher.HashPassword(
                    nuevoUsuario,
                    usuario.PasswordHash);

            contexto.Usuarios.Add(nuevoUsuario);
        }
        else
        {
            var existente = await contexto.Usuarios
                .FirstOrDefaultAsync(u =>
                    u.UsuarioId == usuario.UsuarioId);

            if (existente == null)
                return Result.Success(false);

            existente.Nombre = usuario.Nombre.Trim();
            existente.Apellido = usuario.Apellido.Trim();
            existente.Email = email;
            existente.RolId = usuario.RolId;

            // La contraseña se modifica únicamente
            // mediante CambiarPassword.
        }

        return Result.Success(
            await contexto.SaveChangesAsync() > 0);
    }

    // 2. Buscar usuario por ID
    public async Task<Result<Usuario?>> Buscar(int usuarioId)
    {
        if (usuarioId <= 0)
            return Result<Usuario?>.Success((Usuario?)null);

        await using var contexto = await contextFactory.CreateDbContextAsync();

        var usuario = await contexto.Usuarios
            .AsNoTracking()
            .Include(u => u.Rol)
            .Where(u => u.UsuarioId == usuarioId)
            .Select(u => new Usuario
            {
                UsuarioId = u.UsuarioId,
                FechaRegistro = u.FechaRegistro,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Email = u.Email,
                RolId = u.RolId,
                Rol = u.Rol,
                Activo = u.Activo,
                PasswordHash = string.Empty
            })
            .FirstOrDefaultAsync();

        return Result<Usuario?>.Success(usuario);
    }

    // 3. Obtener usuarios según un criterio
    public async Task<Result<List<Usuario>>> GetList(
        Expression<Func<Usuario, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var usuarios = await contexto.Usuarios
            .AsNoTracking()
            .Where(criterio)
            .OrderBy(u => u.Nombre)
            .ThenBy(u => u.Apellido)
            .Select(u => new Usuario
            {
                UsuarioId = u.UsuarioId,
                FechaRegistro = u.FechaRegistro,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Email = u.Email,
                RolId = u.RolId,
                Rol = u.Rol,
                Activo = u.Activo,
                PasswordHash = string.Empty
            })
            .ToListAsync();

        return Result<List<Usuario>>.Success(usuarios);
    }

    // 4. Buscar usuarios por nombre o apellido
    public async Task<Result<List<Usuario>>> BuscarPorNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return Result<List<Usuario>>.Success(new List<Usuario>());

        await using var contexto = await contextFactory.CreateDbContextAsync();

        nombre = nombre.Trim();

        var usuarios = await contexto.Usuarios
            .AsNoTracking()
            .Where(u =>
                u.Nombre.Contains(nombre) ||
                u.Apellido.Contains(nombre) ||
                (u.Nombre + " " + u.Apellido).Contains(nombre))
            .OrderBy(u => u.Nombre)
            .ThenBy(u => u.Apellido)
            .Select(u => new Usuario
            {
                UsuarioId = u.UsuarioId,
                FechaRegistro = u.FechaRegistro,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Email = u.Email,
                RolId = u.RolId,
                Rol = u.Rol,
                Activo = u.Activo,
                PasswordHash = string.Empty
            })
            .ToListAsync();

        return Result<List<Usuario>>.Success(usuarios);
    }

    // 5. Buscar usuario por email
    public async Task<Result<Usuario?>> BuscarPorEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result<Usuario?>.Success((Usuario?)null);

        await using var contexto = await contextFactory.CreateDbContextAsync();

        email = email.Trim().ToLowerInvariant();

        var usuario = await contexto.Usuarios
            .AsNoTracking()
            .Include(u => u.Rol)
            .Where(u => u.Email.ToLower() == email)
            .Select(u => new Usuario
            {
                UsuarioId = u.UsuarioId,
                FechaRegistro = u.FechaRegistro,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Email = u.Email,
                RolId = u.RolId,
                Rol = u.Rol,
                Activo = u.Activo,
                PasswordHash = string.Empty
            })
            .FirstOrDefaultAsync();

        return Result<Usuario?>.Success(usuario);
    }

    // 6. Buscar usuarios por rol
    public async Task<Result<List<Usuario>>> BuscarPorRol(int rolId)
    {
        if (rolId <= 0)
            return Result<List<Usuario>>.Success(new List<Usuario>());

        await using var contexto = await contextFactory.CreateDbContextAsync();

        var usuarios = await contexto.Usuarios
            .AsNoTracking()
            .Where(u => u.RolId == rolId)
            .OrderBy(u => u.Nombre)
            .Select(u => new Usuario
            {
                UsuarioId = u.UsuarioId,
                FechaRegistro = u.FechaRegistro,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Email = u.Email,
                RolId = u.RolId,
                Rol = u.Rol,
                Activo = u.Activo,
                PasswordHash = string.Empty
            })
            .ToListAsync();

        return Result<List<Usuario>>.Success(usuarios);
    }

    // 7. Buscar usuarios activos
    public async Task<Result<List<Usuario>>> BuscarActivos()
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var usuarios = await contexto.Usuarios
            .AsNoTracking()
            .Where(u => u.Activo)
            .OrderBy(u => u.Nombre)
            .Select(u => new Usuario
            {
                UsuarioId = u.UsuarioId,
                FechaRegistro = u.FechaRegistro,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Email = u.Email,
                RolId = u.RolId,
                Rol = u.Rol,
                Activo = u.Activo,
                PasswordHash = string.Empty
            })
            .ToListAsync();

        return Result<List<Usuario>>.Success(usuarios);
    }

    // 8. Cambiar rol de un usuario
    public async Task<Result> CambiarRol(int usuarioId, int nuevoRolId)
    {
        if (usuarioId <= 0 || nuevoRolId <= 0)
            return Result.Success(false);

        await using var contexto = await contextFactory.CreateDbContextAsync();

        var usuario = await contexto.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);

        if (usuario == null)
            return Result.Success(false);

        bool rolExiste = await contexto.Roles
            .AnyAsync(r => r.RolId == nuevoRolId);

        if (!rolExiste)
            return Result.Success(false);

        // Proteger el último administrador activo
        if (usuario.Activo &&
            usuario.Rol.Nombre.Equals(
                "Administrador",
                StringComparison.OrdinalIgnoreCase))
        {
            bool otroAdministrador = await contexto.Usuarios
                .AnyAsync(u =>
                    u.UsuarioId != usuarioId &&
                    u.Activo &&
                    u.Rol.Nombre.ToLower() == "administrador");

            var nuevoRol = await contexto.Roles
                .FirstOrDefaultAsync(r => r.RolId == nuevoRolId);

            if (!otroAdministrador &&
                nuevoRol != null &&
                !nuevoRol.Nombre.Equals(
                    "Administrador",
                    StringComparison.OrdinalIgnoreCase))
            {
                return Result.Success(false);
            }
        }

        usuario.RolId = nuevoRolId;

        return Result.Success(
            await contexto.SaveChangesAsync() > 0);
    }

    // 9. Cambiar contraseña
    public async Task<Result> CambiarPassword(
        int usuarioId,
        string passwordActual,
        string passwordNueva)
    {
        if (usuarioId <= 0 ||
            string.IsNullOrWhiteSpace(passwordActual) ||
            string.IsNullOrWhiteSpace(passwordNueva) ||
            passwordNueva.Length < 12 ||
            passwordActual == passwordNueva)
        {
            return Result.Success(false);
        }

        await using var contexto = await contextFactory.CreateDbContextAsync();

        var usuario = await contexto.Usuarios
            .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);

        if (usuario == null || !usuario.Activo)
            return Result.Success(false);

        var resultado = passwordHasher.VerifyHashedPassword(
            usuario,
            usuario.PasswordHash,
            passwordActual);

        if (resultado == PasswordVerificationResult.Failed)
            return Result.Success(false);

        usuario.PasswordHash = passwordHasher.HashPassword(
            usuario,
            passwordNueva);

        return Result.Success(
            await contexto.SaveChangesAsync() > 0);
    }

    // 10. Desactivar usuario
    public async Task<Result> Desactivar(int usuarioId)
    {
        if (usuarioId <= 0)
            return Result.Success(false);

        await using var contexto = await contextFactory.CreateDbContextAsync();

        var usuario = await contexto.Usuarios
            .Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);

        if (usuario == null || !usuario.Activo)
            return Result.Success(false);

        if (usuario.Rol.Nombre.Equals(
            "Administrador",
            StringComparison.OrdinalIgnoreCase))
        {
            bool otroAdministrador = await contexto.Usuarios
                .AnyAsync(u =>
                    u.UsuarioId != usuarioId &&
                    u.Activo &&
                    u.Rol.Nombre.ToLower() == "administrador");

            if (!otroAdministrador)
                return Result.Success(false);
        }

        usuario.Activo = false;

        return Result.Success(
            await contexto.SaveChangesAsync() > 0);
    }

    // 11. Activar usuario
    public async Task<Result> Activar(int usuarioId)
    {
        if (usuarioId <= 0)
            return Result.Success(false);

        await using var contexto = await contextFactory.CreateDbContextAsync();

        var usuario = await contexto.Usuarios
            .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);

        if (usuario == null || usuario.Activo)
            return Result.Success(false);

        usuario.Activo = true;

        return Result.Success(
            await contexto.SaveChangesAsync() > 0);
    }

    // 12. Verificar si un email está registrado
    public async Task<Result<bool>> VerificarEmail(
        string email,
        int usuarioIdExcluir = 0)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result<bool>.Success(false);

        await using var contexto = await contextFactory.CreateDbContextAsync();

        email = email.Trim().ToLowerInvariant();

        bool existe = await contexto.Usuarios
            .AnyAsync(u =>
                u.Email.ToLower() == email &&
                u.UsuarioId != usuarioIdExcluir);

        return Result<bool>.Success(existe);
    }

    // 13. No permitir eliminar usuarios
    // Se conservan los registros históricos de la clínica.
    public Task<Result> Eliminar(int usuarioId)
    {
        return Task.FromResult<Result>(Result.Success(false));
    }
}