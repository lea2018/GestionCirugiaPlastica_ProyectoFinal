using GestionCirugiaPlastica.Application.Services;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionCirugiaPlastica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GastosController : ControllerBase
{
    private readonly GastosService _gastosService;

    public GastosController(GastosService gastosService)
    {
        _gastosService = gastosService;
    }

    // GET: api/Gastos
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var resultado = await _gastosService.GetList(g => true);
        return Ok(resultado);
    }

    // GET: api/Gastos/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var resultado = await _gastosService.Buscar(id);
        return Ok(resultado);
    }

    // GET: api/Gastos/fecha/2026-11-12
    [HttpGet("fecha/{fecha}")]
    public async Task<IActionResult> BuscarPorFecha(DateTime fecha)
    {
        var resultado = await _gastosService.BuscarPorFecha(fecha);
        return Ok(resultado);
    }

    // GET: api/Gastos/rango/2026-11-01/2026-11-12
    [HttpGet("rango/{fechaInicio}/{fechaFin}")]
    public async Task<IActionResult> BuscarPorRangoFechas(
        DateTime fechaInicio,
        DateTime fechaFin)
    {
        var resultado = await _gastosService.BuscarPorRangoFechas(
            fechaInicio, fechaFin);

        return Ok(resultado);
    }

    // POST: api/Gastos
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Gasto gasto)
    {
        var resultado = await _gastosService.Guardar(gasto);
        return Ok(resultado);
    }

    // DELETE: api/Gastos/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var resultado = await _gastosService.Eliminar(id);
        return Ok(resultado);
    }
}
