using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GestionCirugiaPlastica.Api.Services;
using GestionCirugiaPlastica.Infrastructure.Data;
using GestionCirugiaPlastica.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionCirugiaPlastica.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class PerfilController(
    IDbContextFactory<GestionCirugiaPlasticaContext> contextFactory,
    IWebHostEnvironment environment,
    JwtTokenService tokenService
) : ControllerBase
{
    private const long MaxPhotoBytes = 5 * 1024 * 1024;
    private const string PhotoUrlPrefix = "/uploads/perfiles/";

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var user = await FindCurrentUserAsync();
        if (user is null || !user.Activo)
            return Unauthorized(new { message = "La cuenta no está disponible." });

        return Ok(ToResponse(user));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateProfileRequest request)
    {
        var user = await FindCurrentUserAsync();
        if (user is null || !user.Activo || user.Rol is null)
            return Unauthorized(new { message = "La cuenta no está disponible." });

        var fullName = request.NombreCompleto?.Trim();
        if (string.IsNullOrWhiteSpace(fullName) || fullName.Length > 250)
            return BadRequest(new { message = "Ingresa tu nombre completo (máximo 250 caracteres)." });

        var nameParts = fullName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        if (nameParts[0].Length > 250 || (nameParts.Length > 1 && nameParts[1].Length > 250))
            return BadRequest(new { message = "El nombre o apellido superan 250 caracteres." });

        var wantsPasswordChange = !string.IsNullOrWhiteSpace(request.PasswordActual) ||
                                  !string.IsNullOrWhiteSpace(request.PasswordNueva);
        if (wantsPasswordChange)
        {
            if (string.IsNullOrWhiteSpace(request.PasswordActual) ||
                string.IsNullOrWhiteSpace(request.PasswordNueva) ||
                request.PasswordNueva.Length < 12 ||
                request.PasswordActual == request.PasswordNueva)
                return BadRequest(new { message = "Para cambiar la contraseña, ingresa la actual y una nueva distinta de al menos 12 caracteres." });

            var hasher = new PasswordHasher<Usuario>();
            if (hasher.VerifyHashedPassword(user, user.PasswordHash, request.PasswordActual) == PasswordVerificationResult.Failed)
                return BadRequest(new { message = "La contraseña actual no es correcta." });

            user.PasswordHash = hasher.HashPassword(user, request.PasswordNueva);
        }

        user.Nombre = nameParts[0];
        user.Apellido = nameParts.Length > 1 ? nameParts[1] : "";
        var roleName = user.Rol.Nombre;
        await using var db = await contextFactory.CreateDbContextAsync();
        db.Entry(user).State = EntityState.Modified;
        await db.SaveChangesAsync();

        return Ok(ToResponse(user, tokenService.Create(user, roleName).AccessToken));
    }

    [HttpPost("foto")]
    [RequestSizeLimit(MaxPhotoBytes + 64 * 1024)]
    public async Task<IActionResult> UploadPhoto([FromForm] IFormFile? file)
    {
        var user = await FindCurrentUserAsync();
        if (user is null || !user.Activo)
            return Unauthorized(new { message = "La cuenta no está disponible." });
        if (file is null || file.Length == 0)
            return BadRequest(new { message = "Selecciona una imagen." });
        if (file.Length > MaxPhotoBytes)
            return BadRequest(new { message = "La imagen no puede superar 5 MB." });

        await using var imageStream = new MemoryStream();
        await file.CopyToAsync(imageStream);
        var imageBytes = imageStream.ToArray();
        var imageType = GetImageType(imageBytes);
        if (imageType < 0)
            return BadRequest(new { message = "Usa una imagen JPEG, PNG o WebP válida." });
        var extension = imageType switch
        {
            0 => ".jpg",
            1 => ".png",
            _ => ".webp"
        };

        var webRoot = GetWebRoot();
        var photoDirectory = Path.GetFullPath(Path.Combine(webRoot, "uploads", "perfiles"));
        if (!photoDirectory.EndsWith(Path.DirectorySeparatorChar))
            photoDirectory += Path.DirectorySeparatorChar;
        Directory.CreateDirectory(photoDirectory);

        var fileName = $"{Guid.NewGuid():N}{extension}";
        var fullPath = Path.GetFullPath(Path.Combine(photoDirectory, fileName));
        if (!fullPath.StartsWith(photoDirectory, StringComparison.Ordinal))
            return BadRequest(new { message = "Ruta de archivo no válida." });
        await System.IO.File.WriteAllBytesAsync(fullPath, imageBytes);

        var previousPhoto = user.FotoUrl;
        user.FotoUrl = $"{PhotoUrlPrefix}{fileName}";
        await using var db = await contextFactory.CreateDbContextAsync();
        db.Entry(user).State = EntityState.Modified;
        try
        {
            await db.SaveChangesAsync();
        }
        catch
        {
            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);
            throw;
        }

        DeleteStoredPhoto(previousPhoto, webRoot);
        return Ok(ToResponse(user));
    }

    private async Task<Usuario?> FindCurrentUserAsync()
    {
        var idValue = User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                      User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (!int.TryParse(idValue, out var userId))
            return null;

        await using var db = await contextFactory.CreateDbContextAsync();
        return await db.Usuarios.Include(u => u.Rol)
            .FirstOrDefaultAsync(u => u.UsuarioId == userId);
    }

    private static PerfilResponse ToResponse(Usuario user, string? accessToken = null) => new(
        user.UsuarioId,
        user.Nombre,
        user.Apellido,
        $"{user.Nombre} {user.Apellido}".Trim(),
        user.NombreUsuario,
        user.Email,
        user.FotoUrl,
        user.PacienteId,
        user.Rol?.Nombre ?? "",
        accessToken);

    private string GetWebRoot() => environment.WebRootPath ?? Path.Combine(environment.ContentRootPath, "wwwroot");

    private static int GetImageType(byte[] bytes)
    {
        if (bytes.Length >= 3 && bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF)
            return 0;
        if (bytes.Length >= 8 && bytes.AsSpan(0, 8).SequenceEqual(new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }))
            return 1;
        if (bytes.Length >= 12 &&
            System.Text.Encoding.ASCII.GetString(bytes, 0, 4) == "RIFF" &&
            System.Text.Encoding.ASCII.GetString(bytes, 8, 4) == "WEBP")
            return 2;
        return -1;
    }

    private static void DeleteStoredPhoto(string? photoUrl, string webRoot)
    {
        if (string.IsNullOrWhiteSpace(photoUrl) || !photoUrl.StartsWith(PhotoUrlPrefix, StringComparison.OrdinalIgnoreCase))
            return;

        var fileName = Path.GetFileName(photoUrl);
        if (string.IsNullOrWhiteSpace(fileName))
            return;

        var baseDirectory = Path.GetFullPath(Path.Combine(webRoot, "uploads", "perfiles"));
        if (!baseDirectory.EndsWith(Path.DirectorySeparatorChar))
            baseDirectory += Path.DirectorySeparatorChar;
        var path = Path.GetFullPath(Path.Combine(baseDirectory, fileName));
        if (path.StartsWith(baseDirectory, StringComparison.Ordinal) && System.IO.File.Exists(path))
            System.IO.File.Delete(path);
    }
}

public sealed record UpdateProfileRequest(string NombreCompleto, string? PasswordActual, string? PasswordNueva);
public sealed record PerfilResponse(
    int UsuarioId,
    string Nombre,
    string Apellido,
    string NombreCompleto,
    string? NombreUsuario,
    string? Email,
    string? FotoUrl,
    int? PacienteId,
    string Rol,
    string? AccessToken);
