using Aplicada1.Core;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionCirugiaPlastica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PacientesController : ControllerBase
{
    private readonly IServiceResult<Paciente, int> _pacientesService;

    public PacientesController(
        IServiceResult<Paciente, int> pacientesService)
    {
        _pacientesService = pacientesService;
    }

    // GET: api/Pacientes
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var resultado = await _pacientesService.GetList(p => true);
        return Ok(resultado);
    }

    // GET: api/Pacientes/5
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var resultado = await _pacientesService.Buscar(id);
        return Ok(resultado);
    }

    // POST: api/Pacientes
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Paciente paciente)
    {
        var resultado = await _pacientesService.Guardar(paciente);
        return Ok(resultado);
    }

    // DELETE: api/Pacientes/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var resultado = await _pacientesService.Eliminar(id);
        return Ok(resultado);
    }
}
