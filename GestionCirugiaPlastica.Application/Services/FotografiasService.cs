
using Aplicada1.Core;
using GestionCirugiaPlastica.Infrastructure.Data;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestionCirugiaPlastica.Application.Services;

public class FotografiasService(
    IDbContextFactory<GestionCirugiaPlasticaContext> contextFactory
) : IServiceResult<Fotografia, int>
{
    public async Task<Result> Guardar(Fotografia fotografia)
    {
        bool existe = await Existe(fotografia.FotografiaId);

        bool guardado = existe
            ? await Modificar(fotografia)
            : await Insertar(fotografia);

        return Result.Success(guardado);
    }

    private async Task<bool> Existe(int fotografiaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.Fotografias
            .AnyAsync(f => f.FotografiaId == fotografiaId);
    }

    private async Task<bool> Insertar(Fotografia fotografia)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.Fotografias.Add(fotografia);

        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Fotografia fotografia)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.Fotografias.Update(fotografia);

        return await contexto.SaveChangesAsync() > 0;
    }

    // Buscar por ID
    public async Task<Result<Fotografia?>> Buscar(int fotografiaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var fotografia = await contexto.Fotografias
            .AsNoTracking()
            .Include(f => f.Cirugia)
                .ThenInclude(c => c.Paciente)
            .Include(f => f.Cirugia)
                .ThenInclude(c => c.Procedimiento)
            .FirstOrDefaultAsync(f => f.FotografiaId == fotografiaId);

        return Result<Fotografia?>.Success(fotografia);
    }

    // Buscar por nombre o apellido del paciente
    public async Task<Result<List<Fotografia>>> BuscarPorNombre(
        string nombre)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var fotografias = await contexto.Fotografias
            .AsNoTracking()
            .Include(f => f.Cirugia)
                .ThenInclude(c => c.Paciente)
            .Include(f => f.Cirugia)
                .ThenInclude(c => c.Procedimiento)
            .Where(f =>
                f.Cirugia.Paciente.Nombre.Contains(nombre) ||
                f.Cirugia.Paciente.Apellido.Contains(nombre))
            .ToListAsync();

        return Result<List<Fotografia>>.Success(fotografias);
    }

    // Buscar por cédula del paciente
    public async Task<Result<List<Fotografia>>> BuscarPorCedula(
        string cedula)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var fotografias = await contexto.Fotografias
            .AsNoTracking()
            .Include(f => f.Cirugia)
                .ThenInclude(c => c.Paciente)
            .Include(f => f.Cirugia)
                .ThenInclude(c => c.Procedimiento)
            .Where(f => f.Cirugia.Paciente.Cedula == cedula)
            .ToListAsync();

        return Result<List<Fotografia>>.Success(fotografias);
    }

    // Listar mediante un criterio personalizado
    public async Task<Result<List<Fotografia>>> GetList(
        Expression<Func<Fotografia, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var fotografias = await contexto.Fotografias
            .AsNoTracking()
            .Include(f => f.Cirugia)
                .ThenInclude(c => c.Paciente)
            .Include(f => f.Cirugia)
                .ThenInclude(c => c.Procedimiento)
            .Where(criterio)
            .ToListAsync();

        return Result<List<Fotografia>>.Success(fotografias);
    }

    // Eliminar por ID
    public async Task<Result> Eliminar(int fotografiaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        bool eliminado = await contexto.Fotografias
            .Where(f => f.FotografiaId == fotografiaId)
            .ExecuteDeleteAsync() > 0;

        return Result.Success(eliminado);
    }
}
