using Aplicada1.Core;
using GestionCirugiaPlastica.Infrastructure.Data;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestionCirugiaPlastica.Application.Services;

public class PacientesService(
    IDbContextFactory<GestionCirugiaPlasticaContext> contextFactory
) : IServiceResult<Paciente, int>
{
    public async Task<Result> Guardar(Paciente paciente)
    {
        bool existe = await Existe(paciente.PacienteId);

        bool guardado = existe
            ? await Modificar(paciente)
            : await Insertar(paciente);

        return Result.Success(guardado);
    }

    private async Task<bool> Existe(int pacienteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.Pacientes
            .AnyAsync(p => p.PacienteId == pacienteId);
    }

    private async Task<bool> Insertar(Paciente paciente)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.Pacientes.Add(paciente);

        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Paciente paciente)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.Pacientes.Update(paciente);

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<Result<Paciente?>> Buscar(int pacienteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var paciente = await contexto.Pacientes
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PacienteId == pacienteId);

        return Result<Paciente?>.Success(paciente);
    }

    public async Task<Result<List<Paciente>>> GetList(
        Expression<Func<Paciente, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var pacientes = await contexto.Pacientes
            .AsNoTracking()
            .Where(criterio)
            .ToListAsync();

        return Result<List<Paciente>>.Success(pacientes);
    }

    public async Task<Result> Eliminar(int pacienteId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        bool eliminado = await contexto.Pacientes
            .Where(p => p.PacienteId == pacienteId)
            .ExecuteDeleteAsync() > 0;

        return Result.Success(eliminado);
    }
}
