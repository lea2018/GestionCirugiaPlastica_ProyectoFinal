using GestionCirugiaPlastica.Application.Services;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionCirugiaPlastica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HistorialClinicoController : ControllerBase
{
    private readonly HistorialClinicoService _historialService;

    public HistorialClinicoController(
        HistorialClinicoService historialService)
    {
        _historialService = historialService;
    }

    // GET: api/HistorialClinico
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var resultado = await _historialService.GetList(h => true);
        return Ok(resultado);
    }

    // GET: api/HistorialClinico/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var resultado = await _historialService.Buscar(id);
        return Ok(resultado);
    }

    // GET: api/HistorialClinico/buscar/nombre/María
    [HttpGet("buscar/nombre/{nombre}")]
    public async Task<IActionResult> BuscarPorNombre(string nombre)
    {
        var resultado = await _historialService.BuscarPorNombre(nombre);
        return Ok(resultado);
    }

    // GET: api/HistorialClinico/buscar/cedula/00000000000
    [HttpGet("buscar/cedula/{cedula}")]
    public async Task<IActionResult> BuscarPorCedula(string cedula)
    {
        var resultado = await _historialService.BuscarPorCedula(cedula);
        return Ok(resultado);
    }

    // POST: api/HistorialClinico
    [HttpPost]
    public async Task<IActionResult> Post(
        [FromBody] HistorialClinico historial)
    {
        var resultado = await _historialService.Guardar(historial);
        return Ok(resultado);
    }

    // DELETE: api/HistorialClinico/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var resultado = await _historialService.Eliminar(id);
        return Ok(resultado);
    }
}
