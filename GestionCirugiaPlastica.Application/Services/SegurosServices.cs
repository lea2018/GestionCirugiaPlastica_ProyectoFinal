using Aplicada1.Core;
using GestionCirugiaPlastica.Infrastructure.Data;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GestionCirugiaPlastica.Application.Services;

public class SegurosServices(
    IDbContextFactory<GestionCirugiaPlasticaContext> contextFactory
) : IServiceResult<Seguro, int>
{
    // 1. Guardar o actualizar una aseguradora
    public async Task<Result> Guardar(Seguro seguro)
    {
        if (seguro == null ||
            string.IsNullOrWhiteSpace(seguro.Nombre) ||
            seguro.Nombre.Trim().Length > 250 ||
            seguro.Telefono?.Length > 250 ||
            seguro.Email?.Length > 250 ||
            seguro.Direccion?.Length > 250)
        {
            return Result.Success(false);
        }

        await using var contexto = await contextFactory.CreateDbContextAsync();

        string nombre = seguro.Nombre.Trim();

        bool nombreExiste = await contexto.Seguros
            .AnyAsync(s =>
                s.Nombre.ToLower() == nombre.ToLower() &&
                s.SeguroId != seguro.SeguroId);

        if (nombreExiste)
            return Result.Success(false);

        if (seguro.SeguroId == 0)
        {
            var nuevoSeguro = new Seguro
            {
                FechaRegistro = DateTime.Now,
                Nombre = nombre,
                Telefono = seguro.Telefono?.Trim(),
                Email = seguro.Email?.Trim(),
                Direccion = seguro.Direccion?.Trim(),
                Activo = true
            };

            contexto.Seguros.Add(nuevoSeguro);
        }
        else
        {
            var existente = await contexto.Seguros
                .FirstOrDefaultAsync(s => s.SeguroId == seguro.SeguroId);

            if (existente == null)
                return Result.Success(false);

            existente.Nombre = nombre;
            existente.Telefono = seguro.Telefono?.Trim();
            existente.Email = seguro.Email?.Trim();
            existente.Direccion = seguro.Direccion?.Trim();

            // El estado Activo se modifica mediante Desactivar o Activar.
        }

        return Result.Success(await contexto.SaveChangesAsync() > 0);
    }

    // 2. Buscar aseguradora por ID
    public async Task<Result<Seguro?>> Buscar(int seguroId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var seguro = await contexto.Seguros
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.SeguroId == seguroId);

        return Result<Seguro?>.Success(seguro);
    }

    // 3. Obtener aseguradoras según un criterio
    public async Task<Result<List<Seguro>>> GetList(
        Expression<Func<Seguro, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var seguros = await contexto.Seguros
            .AsNoTracking()
            .Where(criterio)
            .OrderBy(s => s.Nombre)
            .ToListAsync();

        return Result<List<Seguro>>.Success(seguros);
    }

    // 4. Buscar aseguradoras por nombre
    public async Task<Result<List<Seguro>>> BuscarPorNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return Result<List<Seguro>>.Success(new List<Seguro>());

        await using var contexto = await contextFactory.CreateDbContextAsync();

        nombre = nombre.Trim();

        var seguros = await contexto.Seguros
            .AsNoTracking()
            .Where(s => s.Nombre.Contains(nombre))
            .OrderBy(s => s.Nombre)
            .ToListAsync();

        return Result<List<Seguro>>.Success(seguros);
    }

    // 5. Buscar aseguradoras activas
    public async Task<Result<List<Seguro>>> BuscarActivos()
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var seguros = await contexto.Seguros
            .AsNoTracking()
            .Where(s => s.Activo)
            .OrderBy(s => s.Nombre)
            .ToListAsync();

        return Result<List<Seguro>>.Success(seguros);
    }

    // 6. Buscar aseguradoras de un paciente
    public async Task<Result<List<PacienteSeguro>>> BuscarPorPaciente(
        int pacienteId)
    {
        if (pacienteId <= 0)
            return Result<List<PacienteSeguro>>.Success(
                new List<PacienteSeguro>());

        await using var contexto = await contextFactory.CreateDbContextAsync();

        var polizas = await contexto.PacienteSeguros
            .AsNoTracking()
            .Include(ps => ps.Seguro)
            .Where(ps => ps.PacienteId == pacienteId)
            .OrderByDescending(ps => ps.FechaRegistro)
            .ToListAsync();

        return Result<List<PacienteSeguro>>.Success(polizas);
    }

    // 7. Buscar pólizas y pacientes de una aseguradora
    public async Task<Result<List<PacienteSeguro>>> BuscarPacientes(
        int seguroId)
    {
        if (seguroId <= 0)
            return Result<List<PacienteSeguro>>.Success(
                new List<PacienteSeguro>());

        await using var contexto = await contextFactory.CreateDbContextAsync();

        var polizas = await contexto.PacienteSeguros
            .AsNoTracking()
            .Include(ps => ps.Paciente)
            .Where(ps => ps.SeguroId == seguroId)
            .OrderBy(ps => ps.Paciente.Nombre)
            .ThenBy(ps => ps.Paciente.Apellido)
            .ToListAsync();

        return Result<List<PacienteSeguro>>.Success(polizas);
    }

    // 8. Registrar una póliza para un paciente
    public async Task<Result> RegistrarSeguroPaciente(
        PacienteSeguro pacienteSeguro)
    {
        if (pacienteSeguro == null ||
            pacienteSeguro.PacienteId <= 0 ||
            pacienteSeguro.SeguroId <= 0 ||
            pacienteSeguro.NumeroPoliza?.Length > 250 ||
            pacienteSeguro.Titular?.Length > 250 ||
            pacienteSeguro.Observaciones?.Length > 250)
        {
            return Result.Success(false);
        }

        if (pacienteSeguro.FechaVencimiento.HasValue &&
            pacienteSeguro.FechaVencimiento.Value < DateOnly.FromDateTime(DateTime.Today))
        {
            return Result.Success(false);
        }

        await using var contexto = await contextFactory.CreateDbContextAsync();

        bool pacienteExiste = await contexto.Pacientes
            .AnyAsync(p => p.PacienteId == pacienteSeguro.PacienteId);

        if (!pacienteExiste)
            return Result.Success(false);

        bool seguroValido = await contexto.Seguros
            .AnyAsync(s =>
                s.SeguroId == pacienteSeguro.SeguroId &&
                s.Activo);

        if (!seguroValido)
            return Result.Success(false);

        // Evitar registrar dos veces la misma póliza
        if (!string.IsNullOrWhiteSpace(pacienteSeguro.NumeroPoliza))
        {
            string numero = pacienteSeguro.NumeroPoliza.Trim();

            bool duplicada = await contexto.PacienteSeguros
                .AnyAsync(ps =>
                    ps.PacienteId == pacienteSeguro.PacienteId &&
                    ps.SeguroId == pacienteSeguro.SeguroId &&
                    ps.NumeroPoliza == numero);

            if (duplicada)
                return Result.Success(false);
        }

        var nuevaPoliza = new PacienteSeguro
        {
            FechaRegistro = DateTime.Now,
            PacienteId = pacienteSeguro.PacienteId,
            SeguroId = pacienteSeguro.SeguroId,
            NumeroPoliza = pacienteSeguro.NumeroPoliza?.Trim(),
            Titular = pacienteSeguro.Titular?.Trim(),
            FechaVencimiento = pacienteSeguro.FechaVencimiento,
            Observaciones = pacienteSeguro.Observaciones?.Trim()
        };

        contexto.PacienteSeguros.Add(nuevaPoliza);

        return Result.Success(await contexto.SaveChangesAsync() > 0);
    }

    // 9. Actualizar una póliza existente
    public async Task<Result> ActualizarSeguroPaciente(
        PacienteSeguro pacienteSeguro)
    {
        if (pacienteSeguro == null ||
            pacienteSeguro.PacienteSeguroId <= 0 ||
            pacienteSeguro.PacienteId <= 0 ||
            pacienteSeguro.SeguroId <= 0 ||
            pacienteSeguro.NumeroPoliza?.Length > 250 ||
            pacienteSeguro.Titular?.Length > 250 ||
            pacienteSeguro.Observaciones?.Length > 250)
        {
            return Result.Success(false);
        }

        if (pacienteSeguro.FechaVencimiento.HasValue &&
            pacienteSeguro.FechaVencimiento.Value < DateOnly.FromDateTime(DateTime.Today))
        {
            return Result.Success(false);
        }

        await using var contexto = await contextFactory.CreateDbContextAsync();

        var existente = await contexto.PacienteSeguros
            .FirstOrDefaultAsync(ps =>
                ps.PacienteSeguroId == pacienteSeguro.PacienteSeguroId);

        if (existente == null)
            return Result.Success(false);

        // No permitir cambiar el paciente ni la aseguradora
        if (existente.PacienteId != pacienteSeguro.PacienteId ||
            existente.SeguroId != pacienteSeguro.SeguroId)
        {
            return Result.Success(false);
        }

        if (!string.IsNullOrWhiteSpace(pacienteSeguro.NumeroPoliza))
        {
            string numero = pacienteSeguro.NumeroPoliza.Trim();

            bool duplicada = await contexto.PacienteSeguros
                .AnyAsync(ps =>
                    ps.PacienteSeguroId != pacienteSeguro.PacienteSeguroId &&
                    ps.PacienteId == pacienteSeguro.PacienteId &&
                    ps.SeguroId == pacienteSeguro.SeguroId &&
                    ps.NumeroPoliza == numero);

            if (duplicada)
                return Result.Success(false);
        }

        existente.NumeroPoliza = pacienteSeguro.NumeroPoliza?.Trim();
        existente.Titular = pacienteSeguro.Titular?.Trim();
        existente.FechaVencimiento = pacienteSeguro.FechaVencimiento;
        existente.Observaciones = pacienteSeguro.Observaciones?.Trim();

        return Result.Success(await contexto.SaveChangesAsync() > 0);
    }

    // 10. Buscar póliza por ID
    public async Task<Result<PacienteSeguro?>> BuscarPoliza(int pacienteSeguroId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var poliza = await contexto.PacienteSeguros
            .AsNoTracking()
            .Include(ps => ps.Paciente)
            .Include(ps => ps.Seguro)
            .FirstOrDefaultAsync(ps =>
                ps.PacienteSeguroId == pacienteSeguroId);

        return Result<PacienteSeguro?>.Success(poliza);
    }

    // 11. Buscar pólizas vencidas
    public async Task<Result<List<PacienteSeguro>>> BuscarPolizasVencidas()
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        DateOnly hoy = DateOnly.FromDateTime(DateTime.Today);

        var polizas = await contexto.PacienteSeguros
            .AsNoTracking()
            .Include(ps => ps.Paciente)
            .Include(ps => ps.Seguro)
            .Where(ps =>
                ps.FechaVencimiento.HasValue &&
                ps.FechaVencimiento.Value < hoy)
            .OrderBy(ps => ps.FechaVencimiento)
            .ToListAsync();

        return Result<List<PacienteSeguro>>.Success(polizas);
    }

    // 12. Buscar pólizas próximas a vencer
    public async Task<Result<List<PacienteSeguro>>> BuscarPolizasPorVencer(
        int diasAnticipacion = 30)
    {
        if (diasAnticipacion < 0 || diasAnticipacion > 365)
            return Result<List<PacienteSeguro>>.Success(
                new List<PacienteSeguro>());

        await using var contexto = await contextFactory.CreateDbContextAsync();

        DateOnly hoy = DateOnly.FromDateTime(DateTime.Today);
        DateOnly limite = hoy.AddDays(diasAnticipacion);

        var polizas = await contexto.PacienteSeguros
            .AsNoTracking()
            .Include(ps => ps.Paciente)
            .Include(ps => ps.Seguro)
            .Where(ps =>
                ps.FechaVencimiento.HasValue &&
                ps.FechaVencimiento.Value >= hoy &&
                ps.FechaVencimiento.Value <= limite)
            .OrderBy(ps => ps.FechaVencimiento)
            .ToListAsync();

        return Result<List<PacienteSeguro>>.Success(polizas);
    }

    // 13. Desactivar una aseguradora
    public async Task<Result> Desactivar(int seguroId)
    {
        if (seguroId <= 0)
            return Result.Success(false);

        await using var contexto = await contextFactory.CreateDbContextAsync();

        var seguro = await contexto.Seguros
            .FirstOrDefaultAsync(s => s.SeguroId == seguroId);

        if (seguro == null || !seguro.Activo)
            return Result.Success(false);

        seguro.Activo = false;

        return Result.Success(await contexto.SaveChangesAsync() > 0);
    }

    // 14. Reactivar una aseguradora
    public async Task<Result> Activar(int seguroId)
    {
        if (seguroId <= 0)
            return Result.Success(false);

        await using var contexto = await contextFactory.CreateDbContextAsync();

        var seguro = await contexto.Seguros
            .FirstOrDefaultAsync(s => s.SeguroId == seguroId);

        if (seguro == null || seguro.Activo)
            return Result.Success(false);

        seguro.Activo = true;

        return Result.Success(await contexto.SaveChangesAsync() > 0);
    }

    // 15. No permitir eliminar aseguradoras
    // para conservar el historial de pólizas
    public Task<Result> Eliminar(int seguroId)
    {
        return Task.FromResult<Result>(Result.Success(false));
    }
}
