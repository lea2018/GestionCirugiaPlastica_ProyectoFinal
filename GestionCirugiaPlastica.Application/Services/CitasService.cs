using Aplicada1.Core;
using GestionCirugiaPlastica.Infrastructure.Data;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestionCirugiaPlastica.Application.Services;

public class CitasService(
    IDbContextFactory<GestionCirugiaPlasticaContext> contextFactory
) : IServiceResult<Cita, int>
{
    public async Task<Result> Guardar(Cita cita)
    {
        bool existe = await Existe(cita.CitaId);

        bool guardado = existe
            ? await Modificar(cita)
            : await Insertar(cita);

        return Result.Success(guardado);
    }

    private async Task<bool> Existe(int citaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.Citas
            .AnyAsync(c => c.CitaId == citaId);
    }

    private async Task<bool> Insertar(Cita cita)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.Citas.Add(cita);

        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Cita cita)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.Citas.Update(cita);

        return await contexto.SaveChangesAsync() > 0;
    }

    // Buscar por ID
    public async Task<Result<Cita?>> Buscar(int citaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var cita = await contexto.Citas
            .AsNoTracking()
            .Include(c => c.Paciente)
            .FirstOrDefaultAsync(c => c.CitaId == citaId);

        return Result<Cita?>.Success(cita);
    }

    // Buscar citas por nombre o apellido del paciente
    public async Task<Result<List<Cita>>> BuscarPorNombre(string nombre)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var citas = await contexto.Citas
            .AsNoTracking()
            .Include(c => c.Paciente)
            .Where(c =>
                c.Paciente.Nombre.Contains(nombre) ||
                c.Paciente.Apellido.Contains(nombre))
            .ToListAsync();

        return Result<List<Cita>>.Success(citas);
    }

    // Buscar citas por cédula del paciente
    public async Task<Result<List<Cita>>> BuscarPorCedula(string cedula)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var citas = await contexto.Citas
            .AsNoTracking()
            .Include(c => c.Paciente)
            .Where(c => c.Paciente.Cedula == cedula)
            .ToListAsync();

        return Result<List<Cita>>.Success(citas);
    }

    // Listar citas mediante un criterio personalizado
    public async Task<Result<List<Cita>>> GetList(
        Expression<Func<Cita, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var citas = await contexto.Citas
            .AsNoTracking()
            .Include(c => c.Paciente)
            .Where(criterio)
            .ToListAsync();

        return Result<List<Cita>>.Success(citas);
    }

    // Eliminar cita
    public async Task<Result> Eliminar(int citaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        bool eliminado = await contexto.Citas
            .Where(c => c.CitaId == citaId)
            .ExecuteDeleteAsync() > 0;

        return Result.Success(eliminado);
    }
}
