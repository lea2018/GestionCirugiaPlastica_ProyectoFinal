using Aplicada1.Core;
using GestionCirugiaPlastica.Infrastructure.Data;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestionCirugiaPlastica.Application.Services;

public class GastosService(
    IDbContextFactory<GestionCirugiaPlasticaContext> contextFactory
) : IServiceResult<Gasto, int>
{
    // Registrar o modificar un gasto
    public async Task<Result> Guardar(Gasto gasto)
    {
        bool existe = await Existe(gasto.GastoId);

        bool guardado = existe
            ? await Modificar(gasto)
            : await Insertar(gasto);

        return Result.Success(guardado);
    }

    // Verificar si el gasto existe
    private async Task<bool> Existe(int gastoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.Gastos
            .AnyAsync(g => g.GastoId == gastoId);
    }

    // Insertar un gasto
    private async Task<bool> Insertar(Gasto gasto)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.Gastos.Add(gasto);

        return await contexto.SaveChangesAsync() > 0;
    }

    // Modificar un gasto
    private async Task<bool> Modificar(Gasto gasto)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.Gastos.Update(gasto);

        return await contexto.SaveChangesAsync() > 0;
    }

    // Buscar gasto por ID
    public async Task<Result<Gasto?>> Buscar(int gastoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var gasto = await contexto.Gastos
            .AsNoTracking()
            .Include(g => g.Usuario)
            .FirstOrDefaultAsync(g => g.GastoId == gastoId);

        return Result<Gasto?>.Success(gasto);
    }

    // Buscar gastos de una fecha específica
    public async Task<Result<List<Gasto>>> BuscarPorFecha(
        DateTime fecha)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var gastos = await contexto.Gastos
            .AsNoTracking()
            .Include(g => g.Usuario)
            .Where(g => g.Fecha.Date == fecha.Date)
            .ToListAsync();

        return Result<List<Gasto>>.Success(gastos);
    }

    // Buscar gastos entre dos fechas
    public async Task<Result<List<Gasto>>> BuscarPorRangoFechas(
        DateTime fechaInicio,
        DateTime fechaFin)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        DateTime inicio = fechaInicio.Date;
        DateTime fin = fechaFin.Date.AddDays(1);

        var gastos = await contexto.Gastos
            .AsNoTracking()
            .Include(g => g.Usuario)
            .Where(g => g.Fecha >= inicio && g.Fecha < fin)
            .ToListAsync();

        return Result<List<Gasto>>.Success(gastos);
    }

    // Listar gastos mediante un criterio personalizado
    public async Task<Result<List<Gasto>>> GetList(
        Expression<Func<Gasto, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var gastos = await contexto.Gastos
            .AsNoTracking()
            .Include(g => g.Usuario)
            .Where(criterio)
            .ToListAsync();

        return Result<List<Gasto>>.Success(gastos);
    }

    // Eliminar gasto por ID
    public async Task<Result> Eliminar(int gastoId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        bool eliminado = await contexto.Gastos
            .Where(g => g.GastoId == gastoId)
            .ExecuteDeleteAsync() > 0;

        return Result.Success(eliminado);
    }
}
