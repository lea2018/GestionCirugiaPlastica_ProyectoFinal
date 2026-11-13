using Microsoft.AspNetCore.Http;
using GestionCirugiaPlastica.Application.Services;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionCirugiaPlastica.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProcedimientosController(
    ProcedimientosServices procedimientosService
) : ControllerBase
{
    // Registrar o actualizar un procedimiento
    [HttpPost]
    public async Task<IActionResult> Guardar(Procedimiento procedimiento)
    {
        var resultado = await procedimientosService.Guardar(procedimiento);
        return Ok(resultado);
    }

    // Buscar procedimiento por ID
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Buscar(int id)
    {
        var resultado = await procedimientosService.Buscar(id);
        return Ok(resultado);
    }

    // Obtener todos los procedimientos
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var resultado = await procedimientosService.GetList(p => true);
        return Ok(resultado);
    }

    // Buscar procedimientos por nombre
    [HttpGet("nombre")]
    public async Task<IActionResult> BuscarPorNombre(
        [FromQuery] string nombre)
    {
        var resultado = await procedimientosService.BuscarPorNombre(nombre);
        return Ok(resultado);
    }

    // Buscar procedimientos por rango de precios
    [HttpGet("rango-precio")]
    public async Task<IActionResult> BuscarPorRangoPrecio(
        [FromQuery] decimal precioMinimo,
        [FromQuery] decimal precioMaximo)
    {
        var resultado = await procedimientosService.BuscarPorRangoPrecio(
            precioMinimo,
            precioMaximo);

        return Ok(resultado);
    }

    // Buscar procedimiento utilizado en una cirugía
    [HttpGet("cirugia/{cirugiaId:int}")]
    public async Task<IActionResult> BuscarPorCirugia(int cirugiaId)
    {
        var resultado = await procedimientosService.BuscarPorCirugia(cirugiaId);
        return Ok(resultado);
    }

    // Eliminar procedimiento
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var resultado = await procedimientosService.Eliminar(id);
        return Ok(resultado);
    }
}

