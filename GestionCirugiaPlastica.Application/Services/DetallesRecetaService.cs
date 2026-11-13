
using Aplicada1.Core;
using GestionCirugiaPlastica.Infrastructure.Data;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestionCirugiaPlastica.Application.Services;

public class DetallesRecetaService(
    IDbContextFactory<GestionCirugiaPlasticaContext> contextFactory
) : IServiceResult<DetallesReceta, int>
{
    public async Task<Result> Guardar(DetallesReceta detalle)
    {
        bool existe = await Existe(detalle.DetalleRecetaId);

        bool guardado = existe
            ? await Modificar(detalle)
            : await Insertar(detalle);

        return Result.Success(guardado);
    }

    private async Task<bool> Existe(int detalleRecetaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.DetallesReceta
            .AnyAsync(d => d.DetalleRecetaId == detalleRecetaId);
    }

    private async Task<bool> Insertar(DetallesReceta detalle)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.DetallesReceta.Add(detalle);

        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(DetallesReceta detalle)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.DetallesReceta.Update(detalle);

        return await contexto.SaveChangesAsync() > 0;
    }

    // Buscar por ID
    public async Task<Result<DetallesReceta?>> Buscar(int detalleRecetaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var detalle = await contexto.DetallesReceta
            .AsNoTracking()
            .Include(d => d.Receta)
                .ThenInclude(r => r.Paciente)
            .FirstOrDefaultAsync(d => d.DetalleRecetaId == detalleRecetaId);

        return Result<DetallesReceta?>.Success(detalle);
    }

    // Buscar por nombre o apellido del paciente
    public async Task<Result<List<DetallesReceta>>> BuscarPorNombre(
        string nombre)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var detalles = await contexto.DetallesReceta
            .AsNoTracking()
            .Include(d => d.Receta)
                .ThenInclude(r => r.Paciente)
            .Where(d =>
                d.Receta.Paciente.Nombre.Contains(nombre) ||
                d.Receta.Paciente.Apellido.Contains(nombre))
            .ToListAsync();

        return Result<List<DetallesReceta>>.Success(detalles);
    }

    // Buscar por cédula del paciente
    public async Task<Result<List<DetallesReceta>>> BuscarPorCedula(
        string cedula)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var detalles = await contexto.DetallesReceta
            .AsNoTracking()
            .Include(d => d.Receta)
                .ThenInclude(r => r.Paciente)
            .Where(d => d.Receta.Paciente.Cedula == cedula)
            .ToListAsync();

        return Result<List<DetallesReceta>>.Success(detalles);
    }

    // Listar mediante un criterio personalizado
    public async Task<Result<List<DetallesReceta>>> GetList(
        Expression<Func<DetallesReceta, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var detalles = await contexto.DetallesReceta
            .AsNoTracking()
            .Include(d => d.Receta)
                .ThenInclude(r => r.Paciente)
            .Where(criterio)
            .ToListAsync();

        return Result<List<DetallesReceta>>.Success(detalles);
    }

    // Eliminar por ID
    public async Task<Result> Eliminar(int detalleRecetaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        bool eliminado = await contexto.DetallesReceta
            .Where(d => d.DetalleRecetaId == detalleRecetaId)
            .ExecuteDeleteAsync() > 0;

        return Result.Success(eliminado);
    }
}
