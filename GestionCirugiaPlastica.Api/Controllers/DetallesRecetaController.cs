using GestionCirugiaPlastica.Application.Services;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionCirugiaPlastica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DetallesRecetaController : ControllerBase
{
    private readonly DetallesRecetaService _detallesService;

    public DetallesRecetaController(
        DetallesRecetaService detallesService)
    {
        _detallesService = detallesService;
    }

    // GET: api/DetallesReceta
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var resultado = await _detallesService.GetList(d => true);
        return Ok(resultado);
    }

    // GET: api/DetallesReceta/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var resultado = await _detallesService.Buscar(id);
        return Ok(resultado);
    }

    // GET: api/DetallesReceta/buscar/nombre/María
    [HttpGet("buscar/nombre/{nombre}")]
    public async Task<IActionResult> BuscarPorNombre(string nombre)
    {
        var resultado = await _detallesService.BuscarPorNombre(nombre);
        return Ok(resultado);
    }

    // GET: api/DetallesReceta/buscar/cedula/00000000000
    [HttpGet("buscar/cedula/{cedula}")]
    public async Task<IActionResult> BuscarPorCedula(string cedula)
    {
        var resultado = await _detallesService.BuscarPorCedula(cedula);
        return Ok(resultado);
    }

    // POST: api/DetallesReceta
    [HttpPost]
    public async Task<IActionResult> Post(
        [FromBody] DetallesReceta detalle)
    {
        var resultado = await _detallesService.Guardar(detalle);
        return Ok(resultado);
    }

    // DELETE: api/DetallesReceta/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var resultado = await _detallesService.Eliminar(id);
        return Ok(resultado);
    }
}
