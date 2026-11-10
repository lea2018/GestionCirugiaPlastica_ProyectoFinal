using Aplicada1.Core;
using GestionCirugiaPlastica.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace GestionCirugiaPlastica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PacientesController(
    IServiceResult<Paciente, int> pacientesService
) : ControllerBase
{
    // Obtener todos los pacientes
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var resultado = await pacientesService.GetList(p => true);
        return Ok(resultado);
    }

    // Buscar un paciente por su ID
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var resultado = await pacientesService.Buscar(id);
        return Ok(resultado);
    }

    // Registrar un paciente
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Paciente paciente)
    {
        var resultado = await pacientesService.Guardar(paciente);
        return Ok(resultado);
    }

    // Actualizar un paciente
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Paciente paciente)
    {
        if (id != paciente.PacienteId)
            return BadRequest("El ID de la URL no coincide con el ID del paciente.");

        var resultado = await pacientesService.Guardar(paciente);
        return Ok(resultado);
    }

    // Eliminar un paciente
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var resultado = await pacientesService.Eliminar(id);
        return Ok(resultado);
    }
}