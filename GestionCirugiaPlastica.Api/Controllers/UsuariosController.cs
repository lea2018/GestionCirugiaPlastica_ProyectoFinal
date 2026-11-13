using Microsoft.AspNetCore.Http;
using GestionCirugiaPlastica.Application.Services;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionCirugiaPlastica.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController(
    UsuariosServices usuariosService
) : ControllerBase
{
    // Registrar o actualizar usuario
    [HttpPost]
    public async Task<IActionResult> Guardar(Usuario usuario)
    {
        var resultado = await usuariosService.Guardar(usuario);
        return Ok(resultado);
    }

    // Buscar usuario por ID
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Buscar(int id)
    {
        var resultado = await usuariosService.Buscar(id);
        return Ok(resultado);
    }

    // Obtener todos los usuarios
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var resultado = await usuariosService.GetList(u => true);
        return Ok(resultado);
    }

    // Buscar usuarios por nombre o apellido
    [HttpGet("nombre")]
    public async Task<IActionResult> BuscarPorNombre(
        [FromQuery] string nombre)
    {
        var resultado = await usuariosService.BuscarPorNombre(nombre);
        return Ok(resultado);
    }

    // Buscar usuario por correo electrónico
    [HttpGet("email")]
    public async Task<IActionResult> BuscarPorEmail(
        [FromQuery] string email)
    {
        var resultado = await usuariosService.BuscarPorEmail(email);
        return Ok(resultado);
    }

    // Buscar usuarios por rol
    [HttpGet("rol/{rolId:int}")]
    public async Task<IActionResult> BuscarPorRol(int rolId)
    {
        var resultado = await usuariosService.BuscarPorRol(rolId);
        return Ok(resultado);
    }

    // Obtener usuarios activos
    [HttpGet("activos")]
    public async Task<IActionResult> BuscarActivos()
    {
        var resultado = await usuariosService.BuscarActivos();
        return Ok(resultado);
    }

    // Cambiar rol de un usuario
    [HttpPatch("{usuarioId:int}/rol")]
    public async Task<IActionResult> CambiarRol(
        int usuarioId,
        [FromQuery] int nuevoRolId)
    {
        var resultado = await usuariosService.CambiarRol(
            usuarioId,
            nuevoRolId);

        return Ok(resultado);
    }

    // Cambiar contraseña
    [HttpPatch("{usuarioId:int}/password")]
    public async Task<IActionResult> CambiarPassword(
        int usuarioId,
        [FromBody] CambiarPasswordRequest solicitud)
    {
        var resultado = await usuariosService.CambiarPassword(
            usuarioId,
            solicitud.PasswordActual,
            solicitud.PasswordNueva);

        return Ok(resultado);
    }

    // Desactivar usuario
    [HttpPatch("{usuarioId:int}/desactivar")]
    public async Task<IActionResult> Desactivar(int usuarioId)
    {
        var resultado = await usuariosService.Desactivar(usuarioId);
        return Ok(resultado);
    }

    // Activar usuario
    [HttpPatch("{usuarioId:int}/activar")]
    public async Task<IActionResult> Activar(int usuarioId)
    {
        var resultado = await usuariosService.Activar(usuarioId);
        return Ok(resultado);
    }

    // Verificar si un correo ya está registrado
    [HttpGet("verificar-email")]
    public async Task<IActionResult> VerificarEmail(
        [FromQuery] string email,
        [FromQuery] int usuarioIdExcluir = 0)
    {
        var resultado = await usuariosService.VerificarEmail(
            email,
            usuarioIdExcluir);

        return Ok(resultado);
    }
}

// Modelo para cambiar contraseña
public class CambiarPasswordRequest
{
    public string PasswordActual { get; set; } = string.Empty;

    public string PasswordNueva { get; set; } = string.Empty;
}
