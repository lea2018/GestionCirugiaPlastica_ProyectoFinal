using GestionCirugiaPlastica.Application.Services;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionCirugiaPlastica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsultasController : ControllerBase
{
    private readonly ConsultasService _consultasService;

    public ConsultasController(ConsultasService consultasService)
    {
        _consultasService = consultasService;
    }

    // GET: api/Consultas
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var resultado = await _consultasService.GetList(c => true);
        return Ok(resultado);
    }

    // GET: api/Consultas/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var resultado = await _consultasService.Buscar(id);
        return Ok(resultado);
    }

    // GET: api/Consultas/buscar/nombre/María
    [HttpGet("buscar/nombre/{nombre}")]
    public async Task<IActionResult> BuscarPorNombre(string nombre)
    {
        var resultado = await _consultasService.BuscarPorNombre(nombre);
        return Ok(resultado);
    }

    // GET: api/Consultas/buscar/cedula/00000000000
    [HttpGet("buscar/cedula/{cedula}")]
    public async Task<IActionResult> BuscarPorCedula(string cedula)
    {
        var resultado = await _consultasService.BuscarPorCedula(cedula);
        return Ok(resultado);
    }

    // POST: api/Consultas
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Consulta consulta)
    {
        var resultado = await _consultasService.Guardar(consulta);
        return Ok(resultado);
    }

    // DELETE: api/Consultas/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var resultado = await _consultasService.Eliminar(id);
        return Ok(resultado);
    }
}
