
using Aplicada1.Core;
using GestionCirugiaPlastica.Infrastructure.Data;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestionCirugiaPlastica.Application.Services;

public class CirugiasService(
    IDbContextFactory<GestionCirugiaPlasticaContext> contextFactory
) : IServiceResult<Cirugia, int>
{
    public async Task<Result> Guardar(Cirugia cirugia)
    {
        bool existe = await Existe(cirugia.CirugiaId);

        bool guardado = existe
            ? await Modificar(cirugia)
            : await Insertar(cirugia);

        return Result.Success(guardado);
    }

    private async Task<bool> Existe(int cirugiaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.Cirugias
            .AnyAsync(c => c.CirugiaId == cirugiaId);
    }

    private async Task<bool> Insertar(Cirugia cirugia)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.Cirugias.Add(cirugia);

        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Cirugia cirugia)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.Cirugias.Update(cirugia);

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result<Cirugia?>> Buscar(int cirugiaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var cirugia = await contexto.Cirugias
            .AsNoTracking()
            .Include(c => c.Paciente)
            .Include(c => c.Procedimiento)
            .FirstOrDefaultAsync(c => c.CirugiaId == cirugiaId);

        return Result<Cirugia?>.Success(cirugia);
    }

    public async Task<Result<List<Cirugia>>> GetList(
        Expression<Func<Cirugia, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var cirugias = await contexto.Cirugias
            .AsNoTracking()
            .Include(c => c.Paciente)
            .Include(c => c.Procedimiento)
            .Where(criterio)
            .ToListAsync();

        return Result<List<Cirugia>>.Success(cirugias);
    }

    public async Task<Result> Eliminar(int cirugiaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        bool eliminado = await contexto.Cirugias
            .Where(c => c.CirugiaId == cirugiaId)
            .ExecuteDeleteAsync() > 0;

        return Result.Success(eliminado);
    }
}
