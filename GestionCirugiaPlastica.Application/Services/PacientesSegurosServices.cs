using Aplicada1.Core;
using GestionCirugiaPlastica.Infrastructure.Data;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GestionCirugiaPlastica.Application.Services;

public class PacientesSegurosServices(
    IDbContextFactory<GestionCirugiaPlasticaContext> contextFactory
) : IServiceResult<PacienteSeguro, int>
{
    
    public async Task<Result> Guardar(PacienteSeguro pacienteSeguro)
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

        await using var contexto = await contextFactory.CreateDbContextAsync();

        
        bool pacienteExiste = await contexto.Pacientes
            .AnyAsync(p => p.PacienteId == pacienteSeguro.PacienteId);

        if (!pacienteExiste)
            return Result.Success(false);

        // Comprobar que el seguro existe
        var seguro = await contexto.Seguros
            .FirstOrDefaultAsync(s => s.SeguroId == pacienteSeguro.SeguroId);

        if (seguro == null)
            return Result.Success(false);

        if (pacienteSeguro.PacienteSeguroId == 0)
        {
            
            if (!seguro.Activo)
                return Result.Success(false);

            var nuevoRegistro = new PacienteSeguro
            {
                PacienteId = pacienteSeguro.PacienteId,
                SeguroId = pacienteSeguro.SeguroId,
                NumeroPoliza = pacienteSeguro.NumeroPoliza?.Trim(),
                Titular = pacienteSeguro.Titular?.Trim(),
                FechaVencimiento = pacienteSeguro.FechaVencimiento,
                Observaciones = pacienteSeguro.Observaciones?.Trim()
            };

            contexto.PacienteSeguros.Add(nuevoRegistro);

            return Result.Success(
                await contexto.SaveChangesAsync() > 0
            );
        }

        
        var existente = await contexto.PacienteSeguros
            .FirstOrDefaultAsync(p =>
                p.PacienteSeguroId == pacienteSeguro.PacienteSeguroId);

        if (existente == null)
            return Result.Success(false);

        
        if (existente.PacienteId != pacienteSeguro.PacienteId ||
            existente.SeguroId != pacienteSeguro.SeguroId)
        {
            return Result.Success(false);
        }

       
        if (!seguro.Activo)
            return Result.Success(false);

        
        existente.NumeroPoliza = pacienteSeguro.NumeroPoliza?.Trim();
        existente.Titular = pacienteSeguro.Titular?.Trim();
        existente.FechaVencimiento = pacienteSeguro.FechaVencimiento;
        existente.Observaciones = pacienteSeguro.Observaciones?.Trim();

        await contexto.SaveChangesAsync();

        return Result.Success(true);
    }

    
    public async Task<Result<PacienteSeguro?>> Buscar(int pacienteSeguroId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var pacienteSeguro = await contexto.PacienteSeguros
            .AsNoTracking()
            .Include(p => p.Paciente)
            .Include(p => p.Seguro)
            .FirstOrDefaultAsync(p =>
                p.PacienteSeguroId == pacienteSeguroId);

        return Result<PacienteSeguro?>.Success(pacienteSeguro);
    }

    
    public async Task<Result<List<PacienteSeguro>>> GetList(
        Expression<Func<PacienteSeguro, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var pacientesSeguros = await contexto.PacienteSeguros
            .AsNoTracking()
            .Include(p => p.Paciente)
            .Include(p => p.Seguro)
            .Where(criterio)
            .ToListAsync();

        return Result<List<PacienteSeguro>>.Success(pacientesSeguros);
    }

    
    public async Task<Result<List<PacienteSeguro>>> BuscarPorPaciente(
        int pacienteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var pacientesSeguros = await contexto.PacienteSeguros
            .AsNoTracking()
            .Include(p => p.Seguro)
            .Where(p => p.PacienteId == pacienteId)
            .OrderByDescending(p => p.FechaRegistro)
            .ToListAsync();

        return Result<List<PacienteSeguro>>.Success(pacientesSeguros);
    }

    
    public async Task<Result> Eliminar(int pacienteSeguroId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        bool eliminado = await contexto.PacienteSeguros
            .Where(p => p.PacienteSeguroId == pacienteSeguroId)
            .ExecuteDeleteAsync() > 0;

        return Result.Success(eliminado);
    }
}