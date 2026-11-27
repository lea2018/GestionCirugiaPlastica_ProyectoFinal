using GestionCirugiaPlastica.Application.Services;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using GestionCirugiaPlastica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GestionCirugiaPlastica.API.Controllers;

[ApiController]
[Authorize(Roles = "Doctora,AsistenteClinico")]
[Route("api/[controller]")]
public class FotografiasController(
    FotografiasService fotografiasService,
    IDbContextFactory<GestionCirugiaPlasticaContext> contextFactory,
    IWebHostEnvironment environment
) : ControllerBase
{
    private const long MaxImageBytes = 8 * 1024 * 1024;

    private static string? ResolveExtension(string? contentType)
    {
        if (string.Equals(contentType, "image/jpeg", StringComparison.OrdinalIgnoreCase)) return ".jpg";
        if (string.Equals(contentType, "image/png", StringComparison.OrdinalIgnoreCase)) return ".png";
        if (string.Equals(contentType, "image/webp", StringComparison.OrdinalIgnoreCase)) return ".webp";
        return null;
    }

    [HttpGet("cirugia/{cirugiaId:int}")]
    public async Task<IActionResult> PorCirugia(int cirugiaId)
    {
        var result = await fotografiasService.GetList(f => f.CirugiaId == cirugiaId);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost("cirugia/{cirugiaId:int}/archivo")]
    [RequestSizeLimit(9 * 1024 * 1024)]
    public async Task<IActionResult> Subir(int cirugiaId, IFormFile archivo, [FromForm] string? descripcion)
    {
        if (archivo is null || archivo.Length == 0) return BadRequest(new { message = "Selecciona una imagen." });
        if (archivo.Length > MaxImageBytes) return BadRequest(new { message = "La imagen no puede superar 8 MB." });

        var extension = ResolveExtension(archivo.ContentType);
        if (extension is null) return BadRequest(new { message = "Formato no permitido. Usa JPEG, PNG o WebP." });

        await using var db = await contextFactory.CreateDbContextAsync();
        if (!await db.Cirugias.AnyAsync(c => c.CirugiaId == cirugiaId)) return NotFound();

        var baseDirectory = Path.GetFullPath(Path.Combine(environment.ContentRootPath, "App_Data", "clinical-images"));
        if (!baseDirectory.EndsWith(Path.DirectorySeparatorChar))
            baseDirectory += Path.DirectorySeparatorChar;
        Directory.CreateDirectory(baseDirectory);
        var filename = $"{Guid.NewGuid():N}{extension}";
        var fullPath = Path.GetFullPath(Path.Combine(baseDirectory, filename));
        if (!fullPath.StartsWith(baseDirectory, StringComparison.Ordinal))
            return BadRequest(new { message = "Ruta de archivo no válida." });
        try
        {
            await using (var stream = System.IO.File.Create(fullPath))
                await archivo.CopyToAsync(stream);

            var photo = new Fotografia
            {
                Fecha = DateTime.Now,
                CirugiaId = cirugiaId,
                Tipo = archivo.ContentType,
                RutaArchivo = $"uploads/cirugias/{filename}",
                Descripcion = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion.Trim()
            };
            db.Fotografias.Add(photo);
            await db.SaveChangesAsync();
            db.AuditoriaExpedientes.Add(new AuditoriaExpediente
            {
                PacienteId = (await db.Cirugias.Where(c => c.CirugiaId == cirugiaId).Select(c => c.PacienteId).SingleAsync()),
                UsuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!),
                Fecha = DateTime.UtcNow,
                Entidad = "Fotografia",
                RegistroId = photo.FotografiaId,
                Accion = "Adjunto",
                Detalle = $"Fotografía clínica: {photo.Descripcion ?? "Imagen clínica"}"
            });
            await db.SaveChangesAsync();
            return Ok(new { photo.FotografiaId, photo.CirugiaId, photo.Fecha, photo.Tipo, photo.RutaArchivo, photo.Descripcion });
        }
        catch
        {
            if (System.IO.File.Exists(fullPath)) System.IO.File.Delete(fullPath);
            throw;
        }
    }

    [HttpGet("archivo/{id:int}")]
    public async Task<IActionResult> Archivo(int id)
    {
        var result = await fotografiasService.Buscar(id);
        if (result.IsFailure || result.Value is null) return NotFound();
        var relative = result.Value.RutaArchivo.Replace('\\', '/');
        if (!relative.StartsWith("uploads/cirugias/", StringComparison.OrdinalIgnoreCase)) return NotFound();
        var path = Path.Combine(environment.ContentRootPath, "App_Data", "clinical-images", Path.GetFileName(relative));
        if (!System.IO.File.Exists(path)) return NotFound();
        var contentType = result.Value.Tipo is "image/jpeg" or "image/png" or "image/webp" ? result.Value.Tipo : "application/octet-stream";
        return PhysicalFile(path, contentType);
    }
    // GET: api/Fotografias
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var resultado = await fotografiasService.GetList(f => true);
        return resultado.IsSuccess ? Ok(resultado) : BadRequest(resultado);
    }

    // GET: api/Fotografias/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var resultado = await fotografiasService.Buscar(id);

        if (resultado.IsFailure)
            return BadRequest(resultado);

        return resultado.Value is null ? NotFound() : Ok(resultado);
    }

    // GET: api/Fotografias/buscar/nombre/María
    [HttpGet("buscar/nombre/{nombre}")]
    public async Task<IActionResult> BuscarPorNombre(string nombre)
    {
        var resultado = await fotografiasService.BuscarPorNombre(nombre);
        return resultado.IsSuccess ? Ok(resultado) : BadRequest(resultado);
    }

    // GET: api/Fotografias/buscar/cedula/00000000000
    [HttpGet("buscar/cedula/{cedula}")]
    public async Task<IActionResult> BuscarPorCedula(string cedula)
    {
        var resultado = await fotografiasService.BuscarPorCedula(cedula);
        return resultado.IsSuccess ? Ok(resultado) : BadRequest(resultado);
    }

    // POST: api/Fotografias
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Fotografia fotografia)
    {
        var resultado = await fotografiasService.Guardar(fotografia);
        return resultado.IsSuccess ? Ok(resultado) : BadRequest(resultado);
    }

    // DELETE: api/Fotografias/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        return Conflict(new { message = "Las fotografías forman parte del expediente clínico y no se eliminan desde esta API." });
    }
}
