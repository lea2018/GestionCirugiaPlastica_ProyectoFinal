using GestionCirugiaPlastica.Application.Services;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionCirugiaPlastica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FotografiasController : ControllerBase
{
    private readonly FotografiasService _fotografiasService;

    public FotografiasController(FotografiasService fotografiasService)
    {
        _fotografiasService = fotografiasService;
    }

    // GET: api/Fotografias
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var resultado = await _fotografiasService.GetList(f => true);
        return Ok(resultado);
    }

    // GET: api/Fotografias/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var resultado = await _fotografiasService.Buscar(id);
        return Ok(resultado);
    }

    // GET: api/Fotografias/buscar/nombre/María
    [HttpGet("buscar/nombre/{nombre}")]
    public async Task<IActionResult> BuscarPorNombre(string nombre)
    {
        var resultado = await _fotografiasService.BuscarPorNombre(nombre);
        return Ok(resultado);
    }

    // GET: api/Fotografias/buscar/cedula/00000000000
    [HttpGet("buscar/cedula/{cedula}")]
    public async Task<IActionResult> BuscarPorCedula(string cedula)
    {
        var resultado = await _fotografiasService.BuscarPorCedula(cedula);
        return Ok(resultado);
    }

    // POST: api/Fotografias
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Fotografia fotografia)
    {
        var resultado = await _fotografiasService.Guardar(fotografia);
        return Ok(resultado);
    }

    // DELETE: api/Fotografias/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var resultado = await _fotografiasService.Eliminar(id);
        return Ok(resultado);
    }
}
