using Microsoft.AspNetCore.Http;
using GestionCirugiaPlastica.Application.Services;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionCirugiaPlastica.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SegurosController(
    SegurosServices segurosService
) : ControllerBase
{
    // Registrar o actualizar una aseguradora
    [HttpPost]
    public async Task<IActionResult> Guardar(Seguro seguro)
    {
        var resultado = await segurosService.Guardar(seguro);
        return Ok(resultado);
    }

    // Buscar aseguradora por ID
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Buscar(int id)
    {
        var resultado = await segurosService.Buscar(id);
        return Ok(resultado);
    }

    // Obtener todas las aseguradoras
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var resultado = await segurosService.GetList(s => true);
        return Ok(resultado);
    }

    // Buscar aseguradoras por nombre
    [HttpGet("nombre")]
    public async Task<IActionResult> BuscarPorNombre(
        [FromQuery] string nombre)
    {
        var resultado = await segurosService.BuscarPorNombre(nombre);
        return Ok(resultado);
    }

    // Obtener aseguradoras activas
    [HttpGet("activos")]
    public async Task<IActionResult> BuscarActivos()
    {
        var resultado = await segurosService.BuscarActivos();
        return Ok(resultado);
    }

    // Buscar seguros asociados a un paciente
    [HttpGet("paciente/{pacienteId:int}")]
    public async Task<IActionResult> BuscarPorPaciente(int pacienteId)
    {
        var resultado = await segurosService.BuscarPorPaciente(pacienteId);
        return Ok(resultado);
    }

    // Buscar pacientes asociados a una aseguradora
    [HttpGet("{seguroId:int}/pacientes")]
    public async Task<IActionResult> BuscarPacientes(int seguroId)
    {
        var resultado = await segurosService.BuscarPacientes(seguroId);
        return Ok(resultado);
    }

    // Registrar una póliza para un paciente
    [HttpPost("polizas")]
    public async Task<IActionResult> RegistrarSeguroPaciente(
        PacienteSeguro pacienteSeguro)
    {
        var resultado = await segurosService.RegistrarSeguroPaciente(
            pacienteSeguro);

        return Ok(resultado);
    }

    // Actualizar una póliza existente
    [HttpPut("polizas")]
    public async Task<IActionResult> ActualizarSeguroPaciente(
        PacienteSeguro pacienteSeguro)
    {
        var resultado = await segurosService.ActualizarSeguroPaciente(
            pacienteSeguro);

        return Ok(resultado);
    }

    // Buscar póliza por ID
    [HttpGet("polizas/{pacienteSeguroId:int}")]
    public async Task<IActionResult> BuscarPoliza(int pacienteSeguroId)
    {
        var resultado = await segurosService.BuscarPoliza(pacienteSeguroId);
        return Ok(resultado);
    }

    // Buscar pólizas vencidas
    [HttpGet("polizas/vencidas")]
    public async Task<IActionResult> BuscarPolizasVencidas()
    {
        var resultado = await segurosService.BuscarPolizasVencidas();
        return Ok(resultado);
    }

    // Buscar pólizas próximas a vencer
    [HttpGet("polizas/por-vencer")]
    public async Task<IActionResult> BuscarPolizasPorVencer(
        [FromQuery] int diasAnticipacion = 30)
    {
        var resultado = await segurosService.BuscarPolizasPorVencer(
            diasAnticipacion);

        return Ok(resultado);
    }

    // Desactivar una aseguradora
    [HttpPatch("{id:int}/desactivar")]
    public async Task<IActionResult> Desactivar(int id)
    {
        var resultado = await segurosService.Desactivar(id);
        return Ok(resultado);
    }

    // Reactivar una aseguradora
    [HttpPatch("{id:int}/activar")]
    public async Task<IActionResult> Activar(int id)
    {
        var resultado = await segurosService.Activar(id);
        return Ok(resultado);
    }

    // El servicio no permite eliminar aseguradoras
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var resultado = await segurosService.Eliminar(id);
        return Ok(resultado);
    }
}
