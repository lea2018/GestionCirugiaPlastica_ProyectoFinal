using Aplicada1.Core;
using GestionCirugiaPlastica.Infrastructure.Data;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestionCirugiaPlastica.Application.Services;

public class HistorialClinicoService(
    IDbContextFactory<GestionCirugiaPlasticaContext> contextFactory
) : IServiceResult<HistorialClinico, int>
{
    // Registrar o modificar un historial clínico
    public async Task<Result> Guardar(HistorialClinico historial)
    {
        bool existe = await Existe(historial.HistoriaId);

        bool guardado = existe
            ? await Modificar(historial)
            : await Insertar(historial);

        return Result.Success(guardado);
    }

    // Verificar si el historial existe
    private async Task<bool> Existe(int historiaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.HistorialClinicos
            .AnyAsync(h => h.HistoriaId == historiaId);
    }

    // Insertar historial clínico
    private async Task<bool> Insertar(HistorialClinico historial)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.HistorialClinicos.Add(historial);

        return await contexto.SaveChangesAsync() > 0;
    }

    // Modificar historial clínico
    private async Task<bool> Modificar(HistorialClinico historial)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.HistorialClinicos.Update(historial);

        return await contexto.SaveChangesAsync() > 0;
    }

    // Buscar historial por ID
    public async Task<Result<HistorialClinico?>> Buscar(int historiaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var historial = await contexto.HistorialClinicos
            .AsNoTracking()
            .Include(h => h.Paciente)
            .FirstOrDefaultAsync(h => h.HistoriaId == historiaId);

        return Result<HistorialClinico?>.Success(historial);
    }

    // Buscar historial por nombre o apellido del paciente
    public async Task<Result<List<HistorialClinico>>> BuscarPorNombre(
        string nombre)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var historiales = await contexto.HistorialClinicos
            .AsNoTracking()
            .Include(h => h.Paciente)
            .Where(h =>
                h.Paciente.Nombre.Contains(nombre) ||
                h.Paciente.Apellido.Contains(nombre))
            .ToListAsync();

        return Result<List<HistorialClinico>>.Success(historiales);
    }

    // Buscar historial por cédula del paciente
    public async Task<Result<List<HistorialClinico>>> BuscarPorCedula(
        string cedula)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var historiales = await contexto.HistorialClinicos
            .AsNoTracking()
            .Include(h => h.Paciente)
            .Where(h => h.Paciente.Cedula == cedula)
            .ToListAsync();

        return Result<List<HistorialClinico>>.Success(historiales);
    }

    // Listar historiales mediante criterios personalizados
    public async Task<Result<List<HistorialClinico>>> GetList(
        Expression<Func<HistorialClinico, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var historiales = await contexto.HistorialClinicos
            .AsNoTracking()
            .Include(h => h.Paciente)
            .Where(criterio)
            .ToListAsync();

        return Result<List<HistorialClinico>>.Success(historiales);
    }

    // Eliminar historial por ID
    public async Task<Result> Eliminar(int historiaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        bool eliminado = await contexto.HistorialClinicos
            .Where(h => h.HistoriaId == historiaId)
            .ExecuteDeleteAsync() > 0;

        return Result.Success(eliminado);
    }
}
