using System.Security.Claims;
using GestionCirugiaPlastica.Infrastructure.Data;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionCirugiaPlastica.Api.Controllers;

[ApiController]
[Authorize(Roles = "Doctora")]
[Route("api/DocumentosClinicos")]
public sealed class DocumentosClinicosController(IDbContextFactory<GestionCirugiaPlasticaContext> factory, IWebHostEnvironment environment) : ControllerBase
{
    private const long MaxBytes = 20 * 1024 * 1024;

    [HttpPost("paciente/{pacienteId:int}")]
    [RequestSizeLimit(21 * 1024 * 1024)]
    public async Task<IActionResult> Subir(int pacienteId, IFormFile archivo, [FromForm] string categoria, [FromForm] string? descripcion, [FromForm] int? cirugiaId, [FromForm] int? consultaId)
    {
        if (archivo is null || archivo.Length == 0) return BadRequest(new { message = "Selecciona un documento." });
        if (archivo.Length > MaxBytes) return BadRequest(new { message = "El documento no puede superar 20 MB." });
        var fileType = archivo.ContentType?.ToLowerInvariant() switch
        {
            "application/pdf" => 0,
            "image/jpeg" => 1,
            "image/png" => 2,
            "image/webp" => 3,
            _ => -1
        };
        if (fileType < 0) return BadRequest(new { message = "Formato permitido: PDF, JPEG, PNG o WebP." });
        var extension = fileType switch
        {
            0 => ".pdf",
            1 => ".jpg",
            2 => ".png",
            _ => ".webp"
        };
        var categorias = new[] { "Laboratorio", "Estudio", "Consentimiento", "Otro" };
        if (!categorias.Contains(categoria, StringComparer.OrdinalIgnoreCase)) return BadRequest(new { message = "Categoría de documento no válida." });

        await using var db = await factory.CreateDbContextAsync();
        if (!await db.Pacientes.AnyAsync(p => p.PacienteId == pacienteId)) return NotFound(new { message = "Paciente no encontrado." });
        if (cirugiaId.HasValue && !await db.Cirugias.AnyAsync(c => c.CirugiaId == cirugiaId && c.PacienteId == pacienteId)) return BadRequest(new { message = "La cirugía no corresponde al paciente." });
        if (consultaId.HasValue && !await db.Consultas.AnyAsync(c => c.ConsultaId == consultaId && c.PacienteId == pacienteId)) return BadRequest(new { message = "La consulta no corresponde al paciente." });

        var baseDirectory = Path.GetFullPath(Path.Combine(environment.ContentRootPath, "App_Data", "clinical-documents"));
        if (!baseDirectory.EndsWith(Path.DirectorySeparatorChar))
            baseDirectory += Path.DirectorySeparatorChar;
        Directory.CreateDirectory(baseDirectory);
        var filename = $"{Guid.NewGuid():N}{extension}";
        var path = Path.GetFullPath(Path.Combine(baseDirectory, filename));
        if (!path.StartsWith(baseDirectory, StringComparison.Ordinal))
            return BadRequest(new { message = "Ruta de archivo no válida." });
        try
        {
            await using (var output = System.IO.File.Create(path)) await archivo.CopyToAsync(output);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var doc = new DocumentoClinico
            {
                PacienteId = pacienteId, CirugiaId = cirugiaId, ConsultaId = consultaId,
                Categoria = categorias.First(x => x.Equals(categoria, StringComparison.OrdinalIgnoreCase)),
                NombreArchivo = Path.GetFileName(archivo.FileName), ContentType = archivo.ContentType,
                RutaArchivo = filename, Descripcion = string.IsNullOrWhiteSpace(descripcion) ? null : descripcion.Trim(),
                FechaCarga = DateTime.UtcNow, UsuarioId = userId
            };
            db.DocumentosClinicos.Add(doc);
            await db.SaveChangesAsync();
            db.AuditoriaExpedientes.Add(new AuditoriaExpediente { PacienteId = pacienteId, UsuarioId = userId, Fecha = DateTime.UtcNow, Entidad = "DocumentoClinico", RegistroId = doc.DocumentoClinicoId, Accion = "Adjunto", Detalle = $"{doc.Categoria}: {doc.NombreArchivo}" });
            await db.SaveChangesAsync();
            return Ok(new { doc.DocumentoClinicoId, doc.Categoria, doc.NombreArchivo, doc.ContentType, doc.Descripcion, doc.FechaCarga, doc.CirugiaId, doc.ConsultaId });
        }
        catch
        {
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            throw;
        }
    }

    [HttpGet("{id:int}/archivo")]
    public async Task<IActionResult> Descargar(int id)
    {
        await using var db = await factory.CreateDbContextAsync();
        var doc = await db.DocumentosClinicos.AsNoTracking().FirstOrDefaultAsync(d => d.DocumentoClinicoId == id);
        if (doc is null) return NotFound();
        var path = Path.Combine(environment.ContentRootPath, "App_Data", "clinical-documents", Path.GetFileName(doc.RutaArchivo));
        if (!System.IO.File.Exists(path)) return NotFound();
        return PhysicalFile(path, doc.ContentType, doc.NombreArchivo);
    }
}
