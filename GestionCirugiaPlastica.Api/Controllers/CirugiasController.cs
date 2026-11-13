using Aplicada1.Core;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionCirugiaPlastica.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CirugiasController : ControllerBase
{
    private readonly IServiceResult<Cirugia, int> _cirugiasService;

    public CirugiasController(
        IServiceResult<Cirugia, int> cirugiasService)
    {
        _cirugiasService = cirugiasService;
    }

    // GET: api/Cirugias
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var resultado = await _cirugiasService.GetList(c => true);
        return Ok(resultado);
    }

    // GET: api/Cirugias/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var resultado = await _cirugiasService.Buscar(id);
        return Ok(resultado);
    }

    // POST: api/Cirugias
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Cirugia cirugia)
    {
        var resultado = await _cirugiasService.Guardar(cirugia);
        return Ok(resultado);
    }

    // DELETE: api/Cirugias/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var resultado = await _cirugiasService.Eliminar(id);
        return Ok(resultado);
    }
}
