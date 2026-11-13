using Aplicada1.Core;
using GestionCirugiaPlastica.Infrastructure.Data;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GestionCirugiaPlastica.Application.Services;

public class ProcedimientosServices(
    IDbContextFactory<GestionCirugiaPlasticaContext> contextFactory
) : IServiceResult<Procedimiento, int>
{
    // 1. Guardar o actualizar un procedimiento
    public async Task<Result> Guardar(Procedimiento procedimiento)
    {
        if (procedimiento == null ||
            string.IsNullOrWhiteSpace(procedimiento.Nombre) ||
            procedimiento.Nombre.Trim().Length > 250 ||
            procedimiento.PrecioBase < 0)
        {
            return Result.Success(false);
        }

        await using var contexto = await contextFactory.CreateDbContextAsync();

        string nombre = procedimiento.Nombre.Trim();

        // Evitar nombres duplicados
        bool nombreExiste = await contexto.Procedimientos
            .AnyAsync(p =>
                p.Nombre.ToLower() == nombre.ToLower() &&
                p.ProcedimientoId != procedimiento.ProcedimientoId);

        if (nombreExiste)
            return Result.Success(false);

        if (procedimiento.ProcedimientoId == 0)
        {
            var nuevoProcedimiento = new Procedimiento
            {
                Nombre = nombre,
                Descripcion = string.IsNullOrWhiteSpace(procedimiento.Descripcion)
                    ? null
                    : procedimiento.Descripcion.Trim(),
                PrecioBase = procedimiento.PrecioBase
            };

            contexto.Procedimientos.Add(nuevoProcedimiento);
        }
        else
        {
            var existente = await contexto.Procedimientos
                .FirstOrDefaultAsync(p =>
                    p.ProcedimientoId == procedimiento.ProcedimientoId);

            if (existente == null)
                return Result.Success(false);

            existente.Nombre = nombre;

            existente.Descripcion =
                string.IsNullOrWhiteSpace(procedimiento.Descripcion)
                    ? null
                    : procedimiento.Descripcion.Trim();

            existente.PrecioBase = procedimiento.PrecioBase;
        }

        return Result.Success(await contexto.SaveChangesAsync() > 0);
    }

    // 2. Buscar procedimiento por ID
    public async Task<Result<Procedimiento?>> Buscar(int procedimientoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var procedimiento = await contexto.Procedimientos
            .AsNoTracking()
            .FirstOrDefaultAsync(p =>
                p.ProcedimientoId == procedimientoId);

        return Result<Procedimiento?>.Success(procedimiento);
    }

    // 3. Obtener procedimientos según un criterio
    public async Task<Result<List<Procedimiento>>> GetList(
        Expression<Func<Procedimiento, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var procedimientos = await contexto.Procedimientos
            .AsNoTracking()
            .Where(criterio)
            .OrderBy(p => p.Nombre)
            .ToListAsync();

        return Result<List<Procedimiento>>.Success(procedimientos);
    }

    // 4. Buscar procedimientos por nombre
    public async Task<Result<List<Procedimiento>>> BuscarPorNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            return Result<List<Procedimiento>>.Success(
                new List<Procedimiento>());
        }

        await using var contexto = await contextFactory.CreateDbContextAsync();

        nombre = nombre.Trim();

        var procedimientos = await contexto.Procedimientos
            .AsNoTracking()
            .Where(p => p.Nombre.Contains(nombre))
            .OrderBy(p => p.Nombre)
            .ToListAsync();

        return Result<List<Procedimiento>>.Success(procedimientos);
    }

    // 5. Buscar procedimientos por rango de precios
    public async Task<Result<List<Procedimiento>>> BuscarPorRangoPrecio(
        decimal precioMinimo,
        decimal precioMaximo)
    {
        if (precioMinimo < 0 || precioMaximo < precioMinimo)
        {
            return Result<List<Procedimiento>>.Success(
                new List<Procedimiento>());
        }

        await using var contexto = await contextFactory.CreateDbContextAsync();

        var procedimientos = await contexto.Procedimientos
            .AsNoTracking()
            .Where(p =>
                p.PrecioBase >= precioMinimo &&
                p.PrecioBase <= precioMaximo)
            .OrderBy(p => p.PrecioBase)
            .ToListAsync();

        return Result<List<Procedimiento>>.Success(procedimientos);
    }

    // 6. Buscar procedimientos utilizados en una cirugía
    public async Task<Result<Procedimiento?>> BuscarPorCirugia(int cirugiaId)
    {
        if (cirugiaId <= 0)
            return Result<Procedimiento?>.Success((Procedimiento?)null);

        await using var contexto = await contextFactory.CreateDbContextAsync();

        var procedimiento = await contexto.Procedimientos
            .AsNoTracking()
            .FirstOrDefaultAsync(p =>
                p.Cirugia.Any(c => c.CirugiaId == cirugiaId));

        return Result<Procedimiento?>.Success(procedimiento);
    }

    // 7. Eliminar procedimiento si no tiene cirugías asociadas
    public async Task<Result> Eliminar(int procedimientoId)
    {
        if (procedimientoId <= 0)
            return Result.Success(false);

        await using var contexto = await contextFactory.CreateDbContextAsync();

        bool existe = await contexto.Procedimientos
            .AnyAsync(p => p.ProcedimientoId == procedimientoId);

        if (!existe)
            return Result.Success(false);

        bool tieneCirugias = await contexto.Cirugias
            .AnyAsync(c => c.ProcedimientoId == procedimientoId);

        if (tieneCirugias)
            return Result.Success(false);

        int eliminados = await contexto.Procedimientos
            .Where(p => p.ProcedimientoId == procedimientoId)
            .ExecuteDeleteAsync();

        return Result.Success(eliminados > 0);
    }
}
