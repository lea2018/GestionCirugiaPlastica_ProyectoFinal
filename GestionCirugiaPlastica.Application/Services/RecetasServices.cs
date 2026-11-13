using Aplicada1.Core;
using GestionCirugiaPlastica.Infrastructure.Data;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace GestionCirugiaPlastica.Application.Services;

public class RecetasServices(
    IDbContextFactory<GestionCirugiaPlasticaContext> contextFactory
) : IServiceResult<Receta, int>
{
    // 1. Registrar o actualizar una receta
    public async Task<Result> Guardar(Receta receta)
    {
        if (receta == null ||
            receta.PacienteId <= 0 ||
            receta.Indicaciones?.Length > 4000)
        {
            return Result.Success(false);
        }

        if (receta.DetallesReceta == null ||
            !receta.DetallesReceta.Any() ||
            receta.DetallesReceta.Any(d =>
                string.IsNullOrWhiteSpace(d.Medicamento) ||
                d.Medicamento.Trim().Length > 250 ||
                d.Dosis?.Length > 250 ||
                d.Frecuencia?.Length > 250 ||
                d.Duracion?.Length > 250 ||
                d.Instrucciones?.Length > 250))
        {
            return Result.Success(false);
        }

        // Una receta no debe estar vinculada simultáneamente
        // a una consulta y a una cirugía.
        if (receta.ConsultaId.HasValue &&
            receta.CirugiaId.HasValue)
        {
            return Result.Success(false);
        }

        await using var contexto = await contextFactory.CreateDbContextAsync();

        // Comprobar que el paciente existe
        bool pacienteExiste = await contexto.Pacientes
            .AnyAsync(p => p.PacienteId == receta.PacienteId);

        if (!pacienteExiste)
            return Result.Success(false);

        // Comprobar que la consulta pertenece al paciente
        if (receta.ConsultaId.HasValue)
        {
            bool consultaValida = await contexto.Consultas
                .AnyAsync(c =>
                    c.ConsultaId == receta.ConsultaId.Value &&
                    c.PacienteId == receta.PacienteId);

            if (!consultaValida)
                return Result.Success(false);
        }

        // Comprobar que la cirugía pertenece al paciente
        if (receta.CirugiaId.HasValue)
        {
            bool cirugiaValida = await contexto.Cirugias
                .AnyAsync(c =>
                    c.CirugiaId == receta.CirugiaId.Value &&
                    c.PacienteId == receta.PacienteId);

            if (!cirugiaValida)
                return Result.Success(false);
        }

        // Comprobar que el usuario existe
        if (receta.UsuarioId.HasValue)
        {
            bool usuarioExiste = await contexto.Usuarios
                .AnyAsync(u => u.UsuarioId == receta.UsuarioId.Value);

            if (!usuarioExiste)
                return Result.Success(false);
        }

        if (receta.RecetaId == 0)
        {
            // Crear una receta nueva
            var nuevaReceta = new Receta
            {
                Fecha = DateTime.Now,
                PacienteId = receta.PacienteId,
                ConsultaId = receta.ConsultaId,
                CirugiaId = receta.CirugiaId,
                UsuarioId = receta.UsuarioId,
                Indicaciones = receta.Indicaciones?.Trim(),

                DetallesReceta = receta.DetallesReceta
                    .Select(d => new DetallesReceta
                    {
                        Medicamento = d.Medicamento.Trim(),
                        Dosis = d.Dosis?.Trim(),
                        Frecuencia = d.Frecuencia?.Trim(),
                        Duracion = d.Duracion?.Trim(),
                        Instrucciones = d.Instrucciones?.Trim()
                    }).ToList()
            };

            contexto.Recetas.Add(nuevaReceta);
        }
        else
        {
            // Buscar receta existente
            var existente = await contexto.Recetas
                .Include(r => r.DetallesReceta)
                .FirstOrDefaultAsync(r =>
                    r.RecetaId == receta.RecetaId);

            if (existente == null)
                return Result.Success(false);

            // No permitir cambiar el paciente ni el origen de la receta
            if (existente.PacienteId != receta.PacienteId ||
                existente.ConsultaId != receta.ConsultaId ||
                existente.CirugiaId != receta.CirugiaId)
            {
                return Result.Success(false);
            }

            existente.Indicaciones = receta.Indicaciones?.Trim();

            // Actualizar o agregar medicamentos sin eliminar
            // los detalles históricos que no vengan en la solicitud.
            foreach (var detalle in receta.DetallesReceta)
            {
                if (detalle.DetalleRecetaId == 0)
                {
                    existente.DetallesReceta.Add(new DetallesReceta
                    {
                        Medicamento = detalle.Medicamento.Trim(),
                        Dosis = detalle.Dosis?.Trim(),
                        Frecuencia = detalle.Frecuencia?.Trim(),
                        Duracion = detalle.Duracion?.Trim(),
                        Instrucciones = detalle.Instrucciones?.Trim()
                    });
                }
                else
                {
                    var detalleExistente = existente.DetallesReceta
                        .FirstOrDefault(d =>
                            d.DetalleRecetaId == detalle.DetalleRecetaId);

                    if (detalleExistente == null)
                        return Result.Success(false);

                    detalleExistente.Medicamento = detalle.Medicamento.Trim();
                    detalleExistente.Dosis = detalle.Dosis?.Trim();
                    detalleExistente.Frecuencia = detalle.Frecuencia?.Trim();
                    detalleExistente.Duracion = detalle.Duracion?.Trim();
                    detalleExistente.Instrucciones = detalle.Instrucciones?.Trim();
                }
            }
        }

        return Result.Success(await contexto.SaveChangesAsync() > 0);
    }

    // 2. Buscar receta por ID, incluyendo sus medicamentos
    public async Task<Result<Receta?>> Buscar(int recetaId)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var receta = await contexto.Recetas
            .AsNoTracking()
            .Include(r => r.DetallesReceta)
            .FirstOrDefaultAsync(r => r.RecetaId == recetaId);

        return Result<Receta?>.Success((Receta?)receta);
    }

    // 3. Obtener recetas según un criterio
    public async Task<Result<List<Receta>>> GetList(
        Expression<Func<Receta, bool>> criterio)
    {
        await using var contexto = await contextFactory.CreateDbContextAsync();

        var recetas = await contexto.Recetas
            .AsNoTracking()
            .Include(r => r.DetallesReceta)
            .Where(criterio)
            .OrderByDescending(r => r.Fecha)
            .ThenByDescending(r => r.RecetaId)
            .ToListAsync();

        return Result<List<Receta>>.Success(recetas);
    }

    // 4. Buscar recetas por paciente
    public async Task<Result<List<Receta>>> BuscarPorPaciente(int pacienteId)
    {
        if (pacienteId <= 0)
            return Result<List<Receta>>.Success(new List<Receta>());

        await using var contexto = await contextFactory.CreateDbContextAsync();

        var recetas = await contexto.Recetas
            .AsNoTracking()
            .Include(r => r.DetallesReceta)
            .Where(r => r.PacienteId == pacienteId)
            .OrderByDescending(r => r.Fecha)
            .ToListAsync();

        return Result<List<Receta>>.Success(recetas);
    }

    // 5. Buscar recetas por consulta
    public async Task<Result<List<Receta>>> BuscarPorConsulta(int consultaId)
    {
        if (consultaId <= 0)
            return Result<List<Receta>>.Success(new List<Receta>());

        await using var contexto = await contextFactory.CreateDbContextAsync();

        var recetas = await contexto.Recetas
            .AsNoTracking()
            .Include(r => r.DetallesReceta)
            .Where(r => r.ConsultaId == consultaId)
            .OrderByDescending(r => r.Fecha)
            .ToListAsync();

        return Result<List<Receta>>.Success(recetas);
    }

    // 6. Buscar recetas por cirugía
    public async Task<Result<List<Receta>>> BuscarPorCirugia(int cirugiaId)
    {
        if (cirugiaId <= 0)
            return Result<List<Receta>>.Success(new List<Receta>());

        await using var contexto = await contextFactory.CreateDbContextAsync();

        var recetas = await contexto.Recetas
            .AsNoTracking()
            .Include(r => r.DetallesReceta)
            .Where(r => r.CirugiaId == cirugiaId)
            .OrderByDescending(r => r.Fecha)
            .ToListAsync();

        return Result<List<Receta>>.Success(recetas);
    }

    // 7. Buscar recetas por usuario que las emitió
    public async Task<Result<List<Receta>>> BuscarPorUsuario(int usuarioId)
    {
        if (usuarioId <= 0)
            return Result<List<Receta>>.Success(new List<Receta>());

        await using var contexto = await contextFactory.CreateDbContextAsync();

        var recetas = await contexto.Recetas
            .AsNoTracking()
            .Include(r => r.DetallesReceta)
            .Where(r => r.UsuarioId == usuarioId)
            .OrderByDescending(r => r.Fecha)
            .ToListAsync();

        return Result<List<Receta>>.Success(recetas);
    }

    // 8. Buscar recetas por rango de fechas
    public async Task<Result<List<Receta>>> BuscarPorFecha(
        DateTime fechaDesde,
        DateTime fechaHasta)
    {
        if (fechaDesde.Date > fechaHasta.Date)
            return Result<List<Receta>>.Success(new List<Receta>());

        await using var contexto = await contextFactory.CreateDbContextAsync();

        DateTime desde = fechaDesde.Date;
        DateTime hasta = fechaHasta.Date.AddDays(1);

        var recetas = await contexto.Recetas
            .AsNoTracking()
            .Include(r => r.DetallesReceta)
            .Where(r => r.Fecha >= desde && r.Fecha < hasta)
            .OrderByDescending(r => r.Fecha)
            .ToListAsync();

        return Result<List<Receta>>.Success(recetas);
    }

    // 9. Buscar recetas por medicamento
    public async Task<Result<List<Receta>>> BuscarPorMedicamento(
        string medicamento)
    {
        if (string.IsNullOrWhiteSpace(medicamento))
            return Result<List<Receta>>.Success(new List<Receta>());

        await using var contexto = await contextFactory.CreateDbContextAsync();

        medicamento = medicamento.Trim();

        var recetas = await contexto.Recetas
            .AsNoTracking()
            .Include(r => r.DetallesReceta)
            .Where(r => r.DetallesReceta.Any(d =>
                d.Medicamento.Contains(medicamento)))
            .OrderByDescending(r => r.Fecha)
            .ToListAsync();

        return Result<List<Receta>>.Success(recetas);
    }

    // 10. No permitir eliminar recetas médicas
    public Task<Result> Eliminar(int recetaId)
    {
        return Task.FromResult<Result>(Result.Success(false));
    }
}
