
using GestionCirugiaPlastica.Application.Services;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionCirugiaPlastica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ControlesPostoperatoriosController : ControllerBase
{
    private readonly ControlesPostoperatoriosService _controlesService;

    public ControlesPostoperatoriosController(
        ControlesPostoperatoriosService controlesService)
    {
        _controlesService = controlesService;
    }

    // GET: api/ControlesPostoperatorios
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var resultado = await _controlesService.GetList(c => true);
        return Ok(resultado);
    }

    // GET: api/ControlesPostoperatorios/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var resultado = await _controlesService.Buscar(id);
        return Ok(resultado);
    }

    // GET: api/ControlesPostoperatorios/buscar/nombre/María
    [HttpGet("buscar/nombre/{nombre}")]
    public async Task<IActionResult> BuscarPorNombre(string nombre)
    {
        var resultado = await _controlesService.BuscarPorNombre(nombre);
        return Ok(resultado);
    }

    // GET: api/ControlesPostoperatorios/buscar/cedula/00000000000
    [HttpGet("buscar/cedula/{cedula}")]
    public async Task<IActionResult> BuscarPorCedula(string cedula)
    {
        var resultado = await _controlesService.BuscarPorCedula(cedula);
        return Ok(resultado);
    }

    // POST: api/ControlesPostoperatorios
    [HttpPost]
    public async Task<IActionResult> Post(
        [FromBody] ControlesPostoperatorio control)
    {
        var resultado = await _controlesService.Guardar(control);
        return Ok(resultado);
    }

    // DELETE: api/ControlesPostoperatorios/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var resultado = await _controlesService.Eliminar(id);
        return Ok(resultado);
    }
}
