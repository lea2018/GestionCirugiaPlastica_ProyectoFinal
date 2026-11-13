using Microsoft.AspNetCore.Http;
using GestionCirugiaPlastica.Application.Services;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionCirugiaPlastica.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController(
    RolesServices rolesService
) : ControllerBase
{
    // Registrar o actualizar un rol
    [HttpPost]
    public async Task<IActionResult> Guardar(Role rol)
    {
        var resultado = await rolesService.Guardar(rol);
        return Ok(resultado);
    }

    // Buscar un rol por ID
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Buscar(int id)
    {
        var resultado = await rolesService.Buscar(id);
        return Ok(resultado);
    }

    // Obtener todos los roles
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var resultado = await rolesService.GetList(r => true);
        return Ok(resultado);
    }

    // Buscar roles por nombre
    [HttpGet("nombre")]
    public async Task<IActionResult> BuscarPorNombre(
        [FromQuery] string nombre)
    {
        var resultado = await rolesService.BuscarPorNombre(nombre);
        return Ok(resultado);
    }

    // Buscar el rol asignado a un usuario
    [HttpGet("usuario/{usuarioId:int}")]
    public async Task<IActionResult> BuscarPorUsuario(int usuarioId)
    {
        var resultado = await rolesService.BuscarPorUsuario(usuarioId);
        return Ok(resultado);
    }

    // Buscar los usuarios pertenecientes a un rol
    [HttpGet("{rolId:int}/usuarios")]
    public async Task<IActionResult> BuscarUsuarios(int rolId)
    {
        var resultado = await rolesService.BuscarUsuarios(rolId);
        return Ok(resultado);
    }

    // Comprobar si un rol tiene usuarios asignados
    [HttpGet("{rolId:int}/tiene-usuarios")]
    public async Task<IActionResult> TieneUsuarios(int rolId)
    {
        var resultado = await rolesService.TieneUsuarios(rolId);
        return Ok(resultado);
    }

    // Contar usuarios asignados a un rol
    [HttpGet("{rolId:int}/cantidad-usuarios")]
    public async Task<IActionResult> ContarUsuarios(int rolId)
    {
        var resultado = await rolesService.ContarUsuarios(rolId);
        return Ok(resultado);
    }

    // Comprobar si existe un administrador activo
    [HttpGet("existe-administrador-activo")]
    public async Task<IActionResult> ExisteAdministradorActivo()
    {
        var resultado = await rolesService.ExisteAdministradorActivo();
        return Ok(resultado);
    }

    // Eliminar un rol
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var resultado = await rolesService.Eliminar(id);
        return Ok(resultado);
    }
}
