using System;
using System.Collections.Generic;
using System.Text;
using Aplicada1.Core;
using GestionCirugiaPlastica.Infrastructure.Data;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestionCirugiaPlastica.Application.Services;

public class PagosServices(
    IDbContextFactory<GestionCirugiaPlasticaContext> contextFactory
) : IServiceResult<Pago, int>
{
    // 1. Registrar o modificar un pago
    public async Task<Result> Guardar(Pago pago)
    {
        if (pago == null ||
            pago.Monto <= 0 ||
            (pago.CirugiaId == null && pago.CitaId == null) ||
            (pago.CirugiaId.HasValue && pago.CitaId.HasValue))
        {
            return Result.Success(false);
        }

        await using var contexto = await contextFactory.CreateDbContextAsync();

        // Verificar que exista la cirugía asociada
        if (pago.CirugiaId.HasValue)
        {
            bool cirugiaExiste = await contexto.Cirugias
                .AnyAsync(c => c.CirugiaId == pago.CirugiaId.Value);

            if (!cirugiaExiste)
                return Result.Success(false);
        }

        // Verificar que exista la cita asociada
        if (pago.CitaId.HasValue)
        {
            bool citaExiste = await contexto.Citas
                .AnyAsync(c => c.CitaId == pago.CitaId.Value);

            if (!citaExiste)
                return Result.Success(false);
        }

        // Registrar un pago nuevo
        if (pago.PagoId == 0)
        {
            var nuevoPago = new Pago
            {
                CirugiaId = pago.CirugiaId,
                CitaId = pago.CitaId,
                Monto = pago.Monto,
                MetodoPago = pago.MetodoPago?.Trim(),
                Concepto = pago.Concepto?.Trim()
            };

            contexto.Pagos.Add(nuevoPago);
        }
        else
        {
            // Buscar pago existente
            var existente = await contexto.Pagos
                .FirstOrDefaultAsync(p => p.PagoId == pago.PagoId);

            if (existente == null)
                return Result.Success(false);

            existente.CirugiaId = pago.CirugiaId;
            existente.CitaId = pago.CitaId;
            existente.Monto = pago.Monto;
            existente.MetodoPago = pago.MetodoPago?.Trim();
            existente.Concepto = pago.Concepto?.Trim();
        }

        return Result.Success(await contexto.SaveChangesAsync() > 0);
    }

    // 2. Buscar pago por ID
    public async Task<Result<Pago?>> Buscar(int pagoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var pago = await contexto.Pagos
            .AsNoTracking()
            .Include(p => p.Cirugia)
            .Include(p => p.Cita)
            .FirstOrDefaultAsync(p => p.PagoId == pagoId);

        return Result<Pago?>.Success(pago);
    }

    // 3. Listar pagos mediante un criterio personalizado
    public async Task<Result<List<Pago>>> GetList(
        Expression<Func<Pago, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var pagos = await contexto.Pagos
            .AsNoTracking()
            .Include(p => p.Cirugia)
            .Include(p => p.Cita)
            .Where(criterio)
            .OrderByDescending(p => p.Fecha)
            .ToListAsync();

        return Result<List<Pago>>.Success(pagos);
    }

    // 4. Buscar pagos por cirugía
    public async Task<Result<List<Pago>>> BuscarPorCirugia(int cirugiaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var pagos = await contexto.Pagos
            .AsNoTracking()
            .Include(p => p.Cirugia)
            .Include(p => p.Cita)
            .Where(p => p.CirugiaId == cirugiaId)
            .OrderByDescending(p => p.Fecha)
            .ToListAsync();

        return Result<List<Pago>>.Success(pagos);
    }

    // 5. Buscar pagos por cita
    public async Task<Result<List<Pago>>> BuscarPorCita(int citaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var pagos = await contexto.Pagos
            .AsNoTracking()
            .Include(p => p.Cirugia)
            .Include(p => p.Cita)
            .Where(p => p.CitaId == citaId)
            .OrderByDescending(p => p.Fecha)
            .ToListAsync();

        return Result<List<Pago>>.Success(pagos);
    }

    // 6. Buscar pagos de una fecha específica
    public async Task<Result<List<Pago>>> BuscarPorFecha(DateTime fecha)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var pagos = await contexto.Pagos
            .AsNoTracking()
            .Include(p => p.Cirugia)
            .Include(p => p.Cita)
            .Where(p => p.Fecha >= fecha.Date &&
                        p.Fecha < fecha.Date.AddDays(1))
            .OrderByDescending(p => p.Fecha)
            .ToListAsync();

        return Result<List<Pago>>.Success(pagos);
    }

    // 7. Buscar pagos entre dos fechas
    public async Task<Result<List<Pago>>> BuscarPorRangoFechas(
        DateTime fechaInicio,
        DateTime fechaFin)
    {
        if (fechaInicio.Date > fechaFin.Date)
        {
            return Result<List<Pago>>.Success(new List<Pago>());
        }

        await using var contexto = await contextFactory.CreateDbContextAsync();

        DateTime inicio = fechaInicio.Date;
        DateTime fin = fechaFin.Date.AddDays(1);

        var pagos = await contexto.Pagos
            .AsNoTracking()
            .Include(p => p.Cirugia)
            .Include(p => p.Cita)
            .Where(p => p.Fecha >= inicio && p.Fecha < fin)
            .OrderByDescending(p => p.Fecha)
            .ToListAsync();

        return Result<List<Pago>>.Success(pagos);
    }

    // 8. Eliminar pago por ID
    public async Task<Result> Eliminar(int pagoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        bool eliminado = await contexto.Pagos
            .Where(p => p.PagoId == pagoId)
            .ExecuteDeleteAsync() > 0;

        return Result.Success(eliminado);
    }
}
