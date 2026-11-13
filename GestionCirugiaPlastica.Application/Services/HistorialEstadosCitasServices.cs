using GestionCirugiaPlastica.Infrastructure.Data;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Aplicada1.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;


namespace GestionCirugiaPlastica.Application.Services;

public class HistorialEstadosCitasServices(
    IDbContextFactory<GestionCirugiaPlasticaContext> contextFactory
) : IServiceResult<HistorialEstadosCitas, int>
{
   
    public async Task<Result> Guardar(HistorialEstadosCitas historial)
    {
        if (historial == null ||
            historial.HistorialEstadoCitaId != 0 ||
            historial.CitaId <= 0 ||
            string.IsNullOrWhiteSpace(historial.EstadoNuevo) ||
            historial.EstadoNuevo.Length > 250 ||
            historial.EstadoAnterior?.Length > 250 ||
            historial.Observaciones?.Length > 250)
        {
            return Result.Success(false);
        }

        await using var contexto = await contextFactory.CreateDbContextAsync();

        bool citaExiste = await contexto.Citas
            .AnyAsync(c => c.CitaId == historial.CitaId);

        if (!citaExiste)
            return Result.Success(false);

        if (historial.UsuarioId.HasValue)
        {
            bool usuarioExiste = await contexto.Usuarios
                .AnyAsync(u =>
                    u.UsuarioId == historial.UsuarioId.Value &&
                    u.Activo);

            if (!usuarioExiste)
                return Result.Success(false);
        }

        var nuevoHistorial = new HistorialEstadosCitas
        {
            FechaCambio = historial.FechaCambio,
            CitaId = historial.CitaId,
            EstadoAnterior = historial.EstadoAnterior?.Trim(),
            EstadoNuevo = historial.EstadoNuevo.Trim(),
            UsuarioId = historial.UsuarioId,
            Observaciones = historial.Observaciones?.Trim()
        };

        contexto.HistorialEstadosCita.Add(nuevoHistorial);

        return Result.Success(await contexto.SaveChangesAsync() > 0);
    }

   
    public async Task<Result<HistorialEstadosCitas?>> Buscar(int historialEstadoCitaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var historial = await contexto.HistorialEstadosCita
            .AsNoTracking()
            .FirstOrDefaultAsync(h =>
                h.HistorialEstadoCitaId == historialEstadoCitaId);

        return Result<HistorialEstadosCitas?>.Success(historial);
    }

   
    public async Task<Result<List<HistorialEstadosCitas>>> GetList(
        Expression<Func<HistorialEstadosCitas, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var historial = await contexto.HistorialEstadosCita
            .AsNoTracking()
            .Where(criterio)
            .OrderByDescending(h => h.FechaCambio)
            .ThenByDescending(h => h.HistorialEstadoCitaId)
            .ToListAsync();

        return Result<List<HistorialEstadosCitas>>.Success(historial);
    }

   
    public async Task<Result<List<HistorialEstadosCitas>>> BuscarPorCita(int citaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var historial = await contexto.HistorialEstadosCita
            .AsNoTracking()
            .Where(h => h.CitaId == citaId)
            .OrderByDescending(h => h.FechaCambio)
            .ThenByDescending(h => h.HistorialEstadoCitaId)
            .ToListAsync();

        return Result<List<HistorialEstadosCitas>>.Success(historial);
    }


    public async Task<Result<List<HistorialEstadosCitas>>> BuscarPorUsuario(int usuarioId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var historial = await contexto.HistorialEstadosCita
            .AsNoTracking()
            .Where(h => h.UsuarioId == usuarioId)
            .OrderByDescending(h => h.FechaCambio)
            .ThenByDescending(h => h.HistorialEstadoCitaId)
            .ToListAsync();

        return Result<List<HistorialEstadosCitas>>.Success(historial);
    }


    public Task<Result> Eliminar(int historialEstadoCitaId)
    {
        throw new NotImplementedException("No se permite eliminar registros del historial de citas.");
    }
}