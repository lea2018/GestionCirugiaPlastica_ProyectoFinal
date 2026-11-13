using GestionCirugiaPlastica.Application.Services;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionCirugiaPlastica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitasController : ControllerBase
{
    private readonly CitasService _citasService;

    public CitasController(CitasService citasService)
    {
        _citasService = citasService;
    }

    // GET: api/Citas
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var resultado = await _citasService.GetList(c => true);
        return Ok(resultado);
    }

    // GET: api/Citas/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var resultado = await _citasService.Buscar(id);
        return Ok(resultado);
    }

    // GET: api/Citas/buscar/nombre/María
    [HttpGet("buscar/nombre/{nombre}")]
    public async Task<IActionResult> BuscarPorNombre(string nombre)
    {
        var resultado = await _citasService.BuscarPorNombre(nombre);
        return Ok(resultado);
    }

    // GET: api/Citas/buscar/cedula/00000000000
    [HttpGet("buscar/cedula/{cedula}")]
    public async Task<IActionResult> BuscarPorCedula(string cedula)
    {
        var resultado = await _citasService.BuscarPorCedula(cedula);
        return Ok(resultado);
    }

    // POST: api/Citas
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Cita cita)
    {
        var resultado = await _citasService.Guardar(cita);
        return Ok(resultado);
    }

    // DELETE: api/Citas/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var resultado = await _citasService.Eliminar(id);
        return Ok(resultado);
    }
}
