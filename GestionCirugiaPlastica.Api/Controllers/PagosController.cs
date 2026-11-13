using Microsoft.AspNetCore.Http;
using GestionCirugiaPlastica.Application.Services;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionCirugiaPlastica.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PagosController(
    PagosServices pagosService
) : ControllerBase
{
    // Registrar o actualizar un pago
    [HttpPost]
    public async Task<IActionResult> Guardar(Pago pago)
    {
        var resultado = await pagosService.Guardar(pago);
        return Ok(resultado);
    }

    // Buscar un pago por ID
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Buscar(int id)
    {
        var resultado = await pagosService.Buscar(id);
        return Ok(resultado);
    }

    // Obtener todos los pagos
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var resultado = await pagosService.GetList(p => true);
        return Ok(resultado);
    }

    // Buscar pagos asociados a una cirugía
    [HttpGet("cirugia/{cirugiaId:int}")]
    public async Task<IActionResult> BuscarPorCirugia(int cirugiaId)
    {
        var resultado = await pagosService.BuscarPorCirugia(cirugiaId);
        return Ok(resultado);
    }

    // Buscar pagos asociados a una cita
    [HttpGet("cita/{citaId:int}")]
    public async Task<IActionResult> BuscarPorCita(int citaId)
    {
        var resultado = await pagosService.BuscarPorCita(citaId);
        return Ok(resultado);
    }

    // Buscar pagos de una fecha específica
    [HttpGet("fecha")]
    public async Task<IActionResult> BuscarPorFecha([FromQuery] DateTime fecha)
    {
        var resultado = await pagosService.BuscarPorFecha(fecha);
        return Ok(resultado);
    }

    // Buscar pagos entre dos fechas
    [HttpGet("rango-fechas")]
    public async Task<IActionResult> BuscarPorRangoFechas(
        [FromQuery] DateTime fechaInicio,
        [FromQuery] DateTime fechaFin)
    {
        var resultado = await pagosService.BuscarPorRangoFechas(
            fechaInicio,
            fechaFin);

        return Ok(resultado);
    }

    // Eliminar un pago
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var resultado = await pagosService.Eliminar(id);
        return Ok(resultado);
    }
}
