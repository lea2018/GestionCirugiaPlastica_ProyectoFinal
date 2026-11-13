using Microsoft.AspNetCore.Http;
using GestionCirugiaPlastica.Application.Services;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionCirugiaPlastica.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RecetasController(
    RecetasServices recetasService
) : ControllerBase
{
    // Registrar o actualizar una receta
    [HttpPost]
    public async Task<IActionResult> Guardar(Receta receta)
    {
        var resultado = await recetasService.Guardar(receta);
        return Ok(resultado);
    }

    // Buscar receta por ID
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Buscar(int id)
    {
        var resultado = await recetasService.Buscar(id);
        return Ok(resultado);
    }

    // Obtener todas las recetas
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var resultado = await recetasService.GetList(r => true);
        return Ok(resultado);
    }

    // Buscar recetas por paciente
    [HttpGet("paciente/{pacienteId:int}")]
    public async Task<IActionResult> BuscarPorPaciente(int pacienteId)
    {
        var resultado = await recetasService.BuscarPorPaciente(pacienteId);
        return Ok(resultado);
    }

    // Buscar recetas por consulta
    [HttpGet("consulta/{consultaId:int}")]
    public async Task<IActionResult> BuscarPorConsulta(int consultaId)
    {
        var resultado = await recetasService.BuscarPorConsulta(consultaId);
        return Ok(resultado);
    }

    // Buscar recetas por cirugía
    [HttpGet("cirugia/{cirugiaId:int}")]
    public async Task<IActionResult> BuscarPorCirugia(int cirugiaId)
    {
        var resultado = await recetasService.BuscarPorCirugia(cirugiaId);
        return Ok(resultado);
    }

    // Buscar recetas por usuario
    [HttpGet("usuario/{usuarioId:int}")]
    public async Task<IActionResult> BuscarPorUsuario(int usuarioId)
    {
        var resultado = await recetasService.BuscarPorUsuario(usuarioId);
        return Ok(resultado);
    }

    // Buscar recetas por rango de fechas
    [HttpGet("rango-fechas")]
    public async Task<IActionResult> BuscarPorFecha(
        [FromQuery] DateTime fechaDesde,
        [FromQuery] DateTime fechaHasta)
    {
        var resultado = await recetasService.BuscarPorFecha(
            fechaDesde,
            fechaHasta);

        return Ok(resultado);
    }

    // Buscar recetas por medicamento
    [HttpGet("medicamento")]
    public async Task<IActionResult> BuscarPorMedicamento(
        [FromQuery] string medicamento)
    {
        var resultado = await recetasService.BuscarPorMedicamento(medicamento);
        return Ok(resultado);
    }
}

