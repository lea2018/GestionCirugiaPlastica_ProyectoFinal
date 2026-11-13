using Aplicada1.Core;
using GestionCirugiaPlastica.Application.Services;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionCirugiaPlastica.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PacientesController(
    IServiceResult<Paciente, int> pacientesService
) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Guardar(Paciente paciente)
    {
        var resultado = await pacientesService.Guardar(paciente);
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Buscar(int id)
    {
        var resultado = await pacientesService.Buscar(id);
        return Ok(resultado);
    }

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var resultado = await pacientesService.GetList(p => true);
        return Ok(resultado);
    }

    [HttpGet("buscar")]
    public async Task<IActionResult> BuscarPorNombre([FromQuery] string nombre)
    {
        var resultado = await pacientesService.GetList(
            p => p.Nombre.Contains(nombre) ||
                 p.Apellido.Contains(nombre)
        );

        return Ok(resultado);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var resultado = await pacientesService.Eliminar(id);
        return Ok(resultado);
    }
}