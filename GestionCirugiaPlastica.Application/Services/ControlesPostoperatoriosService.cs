using Aplicada1.Core;
using GestionCirugiaPlastica.Infrastructure.Data;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestionCirugiaPlastica.Application.Services;

public class ControlesPostoperatoriosService(
    IDbContextFactory<GestionCirugiaPlasticaContext> contextFactory
) : IServiceResult<ControlesPostoperatorio, int>
{
    public async Task<Result> Guardar(ControlesPostoperatorio control)
    {
        bool existe = await Existe(control.ControlId);

        bool guardado = existe
            ? await Modificar(control)
            : await Insertar(control);

        return Result.Success(guardado);
    }

    private async Task<bool> Existe(int controlId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.ControlesPostoperatorios
            .AnyAsync(c => c.ControlId == controlId);
    }

    private async Task<bool> Insertar(ControlesPostoperatorio control)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.ControlesPostoperatorios.Add(control);

        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(ControlesPostoperatorio control)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.ControlesPostoperatorios.Update(control);

        return await contexto.SaveChangesAsync() > 0;
    }

    // Buscar por ID
    public async Task<Result<ControlesPostoperatorio?>> Buscar(int controlId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var control = await contexto.ControlesPostoperatorios
            .AsNoTracking()
            .Include(c => c.Cirugia)
                .ThenInclude(c => c.Paciente)
            .Include(c => c.Cirugia)
                .ThenInclude(c => c.Procedimiento)
            .FirstOrDefaultAsync(c => c.ControlId == controlId);

        return Result<ControlesPostoperatorio?>.Success(control);
    }

    // Buscar por nombre o apellido del paciente
    public async Task<Result<List<ControlesPostoperatorio>>> BuscarPorNombre(
        string nombre)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var controles = await contexto.ControlesPostoperatorios
            .AsNoTracking()
            .Include(c => c.Cirugia)
                .ThenInclude(c => c.Paciente)
            .Include(c => c.Cirugia)
                .ThenInclude(c => c.Procedimiento)
            .Where(c =>
                c.Cirugia.Paciente.Nombre.Contains(nombre) ||
                c.Cirugia.Paciente.Apellido.Contains(nombre))
            .ToListAsync();

        return Result<List<ControlesPostoperatorio>>.Success(controles);
    }

    // Buscar por cédula del paciente
    public async Task<Result<List<ControlesPostoperatorio>>> BuscarPorCedula(
        string cedula)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var controles = await contexto.ControlesPostoperatorios
            .AsNoTracking()
            .Include(c => c.Cirugia)
                .ThenInclude(c => c.Paciente)
            .Include(c => c.Cirugia)
                .ThenInclude(c => c.Procedimiento)
            .Where(c => c.Cirugia.Paciente.Cedula == cedula)
            .ToListAsync();

        return Result<List<ControlesPostoperatorio>>.Success(controles);
    }

    // Listar mediante un criterio personalizado
    public async Task<Result<List<ControlesPostoperatorio>>> GetList(
        Expression<Func<ControlesPostoperatorio, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var controles = await contexto.ControlesPostoperatorios
            .AsNoTracking()
            .Include(c => c.Cirugia)
                .ThenInclude(c => c.Paciente)
            .Include(c => c.Cirugia)
                .ThenInclude(c => c.Procedimiento)
            .Where(criterio)
            .ToListAsync();

        return Result<List<ControlesPostoperatorio>>.Success(controles);
    }

    // Eliminar por ID
    public async Task<Result> Eliminar(int controlId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        bool eliminado = await contexto.ControlesPostoperatorios
            .Where(c => c.ControlId == controlId)
            .ExecuteDeleteAsync() > 0;

        return Result.Success(eliminado);
    }
}
