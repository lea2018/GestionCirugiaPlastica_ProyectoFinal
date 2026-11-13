using Microsoft.AspNetCore.Http;
using GestionCirugiaPlastica.Application.Services;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionCirugiaPlastica.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PacienteSeguroController(
    PacientesSegurosServices pacientesSegurosService
) : ControllerBase
{
    // Registrar o actualizar
    [HttpPost]
    public async Task<IActionResult> Guardar(PacienteSeguro pacienteSeguro)
    {
        var resultado = await pacientesSegurosService.Guardar(pacienteSeguro);
        return Ok(resultado);
    }

    // Buscar por ID
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Buscar(int id)
    {
        var resultado = await pacientesSegurosService.Buscar(id);
        return Ok(resultado);
    }

    // Listar todos
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var resultado = await pacientesSegurosService.GetList(p => true);
        return Ok(resultado);
    }

    // Buscar seguros de un paciente
    [HttpGet("paciente/{pacienteId:int}")]
    public async Task<IActionResult> BuscarPorPaciente(int pacienteId)
    {
        var resultado = await pacientesSegurosService.BuscarPorPaciente(pacienteId);
        return Ok(resultado);
    }

    // Eliminar
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var resultado = await pacientesSegurosService.Eliminar(id);
        return Ok(resultado);
    }
}
