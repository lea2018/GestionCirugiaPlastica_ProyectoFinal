using Aplicada1.Core;
using GestionCirugiaPlastica.Infrastructure.Data;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GestionCirugiaPlastica.Application.Services;

public class PreoperatoriosServices(
    IDbContextFactory<GestionCirugiaPlasticaContext> contextFactory
) : IServiceResult<Preoperatorio, int>
{
    // 1. Registrar una nueva evaluación preoperatoria
    public async Task<Result> Guardar(Preoperatorio preoperatorio)
    {
        if (preoperatorio == null ||
            preoperatorio.PreoperatorioId != 0 ||
            preoperatorio.CirugiaId <= 0)
        {
            return Result.Success(false);
        }

        await using var contexto = await contextFactory.CreateDbContextAsync();

        // Comprobar que la cirugía existe
        bool cirugiaExiste = await contexto.Cirugias
            .AnyAsync(c => c.CirugiaId == preoperatorio.CirugiaId);

        if (!cirugiaExiste)
            return Result.Success(false);

        // Validar autorización médica
        if (preoperatorio.Autorizado &&
            (string.IsNullOrWhiteSpace(preoperatorio.AnalisisLaboratorio) ||
             string.IsNullOrWhiteSpace(preoperatorio.EvaluacionCardiologica) ||
             string.IsNullOrWhiteSpace(preoperatorio.EvaluacionAnestesica)))
        {
            return Result.Success(false);
        }

        // Registrar una evaluación nueva
        var nuevoPreoperatorio = new Preoperatorio
        {
            CirugiaId = preoperatorio.CirugiaId,
            AnalisisLaboratorio = preoperatorio.AnalisisLaboratorio?.Trim(),
            EvaluacionCardiologica = preoperatorio.EvaluacionCardiologica?.Trim(),
            EvaluacionAnestesica = preoperatorio.EvaluacionAnestesica?.Trim(),
            Autorizado = preoperatorio.Autorizado,
            Observaciones = preoperatorio.Observaciones?.Trim()
        };

        contexto.Preoperatorios.Add(nuevoPreoperatorio);

        return Result.Success(await contexto.SaveChangesAsync() > 0);
    }

    // 2. Buscar evaluación por ID
    public async Task<Result<Preoperatorio?>> Buscar(int preoperatorioId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var preoperatorio = await contexto.Preoperatorios
            .AsNoTracking()
            .FirstOrDefaultAsync(p =>
                p.PreoperatorioId == preoperatorioId);

        return Result<Preoperatorio?>.Success(preoperatorio);
    }

    // 3. Obtener evaluaciones según un criterio
    public async Task<Result<List<Preoperatorio>>> GetList(
        Expression<Func<Preoperatorio, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var preoperatorios = await contexto.Preoperatorios
            .AsNoTracking()
            .Where(criterio)
            .OrderByDescending(p => p.FechaEvaluacion)
            .ThenByDescending(p => p.PreoperatorioId)
            .ToListAsync();

        return Result<List<Preoperatorio>>.Success(preoperatorios);
    }

    // 4. Consultar historial de evaluaciones de una cirugía
    public async Task<Result<List<Preoperatorio>>> BuscarHistorial(int cirugiaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var historial = await contexto.Preoperatorios
            .AsNoTracking()
            .Where(p => p.CirugiaId == cirugiaId)
            .OrderByDescending(p => p.FechaEvaluacion)
            .ThenByDescending(p => p.PreoperatorioId)
            .ToListAsync();

        return Result<List<Preoperatorio>>.Success(historial);
    }

    // 5. Buscar evaluaciones por cirugía
    public async Task<Result<List<Preoperatorio>>> BuscarPorCirugia(
        int cirugiaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var evaluaciones = await contexto.Preoperatorios
            .AsNoTracking()
            .Where(p => p.CirugiaId == cirugiaId)
            .OrderByDescending(p => p.FechaEvaluacion)
            .ToListAsync();

        return Result<List<Preoperatorio>>.Success(evaluaciones);
    }

    // 6. Buscar evaluaciones según autorización
    public async Task<Result<List<Preoperatorio>>> BuscarPorAutorizacion(
        bool autorizado)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var evaluaciones = await contexto.Preoperatorios
            .AsNoTracking()
            .Where(p => p.Autorizado == autorizado)
            .OrderByDescending(p => p.FechaEvaluacion)
            .ToListAsync();

        return Result<List<Preoperatorio>>.Success(evaluaciones);
    }

    // 7. Buscar evaluaciones con requisitos pendientes
    public async Task<Result<List<Preoperatorio>>> BuscarRequisitosPendientes()
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var pendientes = await contexto.Preoperatorios
            .AsNoTracking()
            .Where(p =>
                !p.Autorizado ||
                p.AnalisisLaboratorio == null ||
                p.AnalisisLaboratorio.Trim() == "" ||
                p.EvaluacionCardiologica == null ||
                p.EvaluacionCardiologica.Trim() == "" ||
                p.EvaluacionAnestesica == null ||
                p.EvaluacionAnestesica.Trim() == "")
            .OrderByDescending(p => p.FechaEvaluacion)
            .ToListAsync();

        return Result<List<Preoperatorio>>.Success(pendientes);
    }

    // 8. Buscar evaluaciones por rango de fechas
    public async Task<Result<List<Preoperatorio>>> BuscarPorFecha(
        DateTime fechaDesde,
        DateTime fechaHasta)
    {
        if (fechaDesde.Date > fechaHasta.Date)
        {
            return Result<List<Preoperatorio>>.Success(
                new List<Preoperatorio>());
        }

        await using var contexto = await contextFactory.CreateDbContextAsync();

        DateTime desde = fechaDesde.Date;
        DateTime hasta = fechaHasta.Date.AddDays(1);

        var evaluaciones = await contexto.Preoperatorios
            .AsNoTracking()
            .Where(p =>
                p.FechaEvaluacion >= desde &&
                p.FechaEvaluacion < hasta)
            .OrderByDescending(p => p.FechaEvaluacion)
            .ToListAsync();

        return Result<List<Preoperatorio>>.Success(evaluaciones);
    }

    // 9. No permitir eliminar evaluaciones clínicas
    public Task<Result> Eliminar(int preoperatorioId)
    {
        return Task.FromResult<Result>(Result.Success(false));
    }
}