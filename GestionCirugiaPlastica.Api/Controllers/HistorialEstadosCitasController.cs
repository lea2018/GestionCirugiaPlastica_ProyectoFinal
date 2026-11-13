using Microsoft.AspNetCore.Http;
using GestionCirugiaPlastica.Application.Services;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionCirugiaPlastica.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HistorialEstadosCitasController(
    HistorialEstadosCitasServices historialService
) : ControllerBase
{
    // Registrar un nuevo cambio de estado
    [HttpPost]
    public async Task<IActionResult> Guardar(HistorialEstadosCitas historial)
    {
        var resultado = await historialService.Guardar(historial);
        return Ok(resultado);
    }

    // Buscar un historial por su ID
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Buscar(int id)
    {
        var resultado = await historialService.Buscar(id);
        return Ok(resultado);
    }

    // Obtener todos los registros del historial
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var resultado = await historialService.GetList(h => true);
        return Ok(resultado);
    }

    // Buscar historial de una cita específica
    [HttpGet("cita/{citaId:int}")]
    public async Task<IActionResult> BuscarPorCita(int citaId)
    {
        var resultado = await historialService.BuscarPorCita(citaId);
        return Ok(resultado);
    }

    // Buscar historial realizado por un usuario
    [HttpGet("usuario/{usuarioId:int}")]
    public async Task<IActionResult> BuscarPorUsuario(int usuarioId)
    {
        var resultado = await historialService.BuscarPorUsuario(usuarioId);
        return Ok(resultado);
    }
}

