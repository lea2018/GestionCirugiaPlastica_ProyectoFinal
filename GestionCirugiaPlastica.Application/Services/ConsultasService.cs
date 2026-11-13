
using Aplicada1.Core;
using GestionCirugiaPlastica.Infrastructure.Data;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestionCirugiaPlastica.Application.Services;

public class ConsultasService(
    IDbContextFactory<GestionCirugiaPlasticaContext> contextFactory
) : IServiceResult<Consulta, int>
{
    public async Task<Result> Guardar(Consulta consulta)
    {
        bool existe = await Existe(consulta.ConsultaId);

        bool guardado = existe
            ? await Modificar(consulta)
            : await Insertar(consulta);

        return Result.Success(guardado);
    }

    private async Task<bool> Existe(int consultaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        return await contexto.Consultas
            .AnyAsync(c => c.ConsultaId == consultaId);
    }

    private async Task<bool> Insertar(Consulta consulta)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.Consultas.Add(consulta);

        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Consulta consulta)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        contexto.Consultas.Update(consulta);

        return await contexto.SaveChangesAsync() > 0;
    }

    // Buscar por ID
    public async Task<Result<Consulta?>> Buscar(int consultaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var consulta = await contexto.Consultas
            .AsNoTracking()
            .Include(c => c.Paciente)
            .Include(c => c.Cita)
            .FirstOrDefaultAsync(c => c.ConsultaId == consultaId);

        return Result<Consulta?>.Success(consulta);
    }

    // Buscar consultas por nombre o apellido del paciente
    public async Task<Result<List<Consulta>>> BuscarPorNombre(string nombre)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var consultas = await contexto.Consultas
            .AsNoTracking()
            .Include(c => c.Paciente)
            .Include(c => c.Cita)
            .Where(c =>
                c.Paciente.Nombre.Contains(nombre) ||
                c.Paciente.Apellido.Contains(nombre))
            .ToListAsync();

        return Result<List<Consulta>>.Success(consultas);
    }

    // Buscar consultas por cédula del paciente
    public async Task<Result<List<Consulta>>> BuscarPorCedula(string cedula)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var consultas = await contexto.Consultas
            .AsNoTracking()
            .Include(c => c.Paciente)
            .Include(c => c.Cita)
            .Where(c => c.Paciente.Cedula == cedula)
            .ToListAsync();

        return Result<List<Consulta>>.Success(consultas);
    }

    // Listar consultas mediante un criterio personalizado
    public async Task<Result<List<Consulta>>> GetList(
        Expression<Func<Consulta, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var consultas = await contexto.Consultas
            .AsNoTracking()
            .Include(c => c.Paciente)
            .Include(c => c.Cita)
            .Where(criterio)
            .ToListAsync();

        return Result<List<Consulta>>.Success(consultas);
    }

    // Eliminar consulta
    public async Task<Result> Eliminar(int consultaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        bool eliminado = await contexto.Consultas
            .Where(c => c.ConsultaId == consultaId)
            .ExecuteDeleteAsync() > 0;

        return Result.Success(eliminado);
    }
}
