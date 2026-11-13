using Microsoft.AspNetCore.Http;
using GestionCirugiaPlastica.Application.Services;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionCirugiaPlastica.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PreoperatoriosController(
    PreoperatoriosServices preoperatoriosService
) : ControllerBase
{
    // Registrar una nueva evaluación preoperatoria
    [HttpPost]
    public async Task<IActionResult> Guardar(Preoperatorio preoperatorio)
    {
        var resultado = await preoperatoriosService.Guardar(preoperatorio);
        return Ok(resultado);
    }

    // Buscar evaluación por ID
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Buscar(int id)
    {
        var resultado = await preoperatoriosService.Buscar(id);
        return Ok(resultado);
    }

    // Obtener todas las evaluaciones
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var resultado = await preoperatoriosService.GetList(p => true);
        return Ok(resultado);
    }

    // Consultar historial de evaluaciones de una cirugía
    [HttpGet("historial/{cirugiaId:int}")]
    public async Task<IActionResult> BuscarHistorial(int cirugiaId)
    {
        var resultado = await preoperatoriosService.BuscarHistorial(cirugiaId);
        return Ok(resultado);
    }

    // Buscar evaluaciones por cirugía
    [HttpGet("cirugia/{cirugiaId:int}")]
    public async Task<IActionResult> BuscarPorCirugia(int cirugiaId)
    {
        var resultado = await preoperatoriosService.BuscarPorCirugia(cirugiaId);
        return Ok(resultado);
    }

    // Buscar evaluaciones según autorización
    [HttpGet("autorizacion/{autorizado:bool}")]
    public async Task<IActionResult> BuscarPorAutorizacion(bool autorizado)
    {
        var resultado = await preoperatoriosService.BuscarPorAutorizacion(autorizado);
        return Ok(resultado);
    }

    // Buscar evaluaciones con requisitos pendientes
    [HttpGet("requisitos-pendientes")]
    public async Task<IActionResult> BuscarRequisitosPendientes()
    {
        var resultado = await preoperatoriosService.BuscarRequisitosPendientes();
        return Ok(resultado);
    }

    // Buscar evaluaciones por rango de fechas
    [HttpGet("rango-fechas")]
    public async Task<IActionResult> BuscarPorFecha(
        [FromQuery] DateTime fechaDesde,
        [FromQuery] DateTime fechaHasta)
    {
        var resultado = await preoperatoriosService.BuscarPorFecha(
            fechaDesde,
            fechaHasta);

        return Ok(resultado);
    }

    // El servicio no permite eliminar evaluaciones clínicas
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var resultado = await preoperatoriosService.Eliminar(id);
        return Ok(resultado);
    }
}
