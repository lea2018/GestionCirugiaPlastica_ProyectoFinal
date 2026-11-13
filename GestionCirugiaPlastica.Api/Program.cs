using Aplicada1.Core;
using GestionCirugiaPlastica.Infrastructure.Models;
using GestionCirugiaPlastica.Application.Services;
using GestionCirugiaPlastica.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la conexión con SQL Server
builder.Services.AddDbContextFactory<GestionCirugiaPlasticaContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Registro de los servicios
builder.Services.AddScoped<IServiceResult<Paciente, int>, PacientesService>();
builder.Services.AddScoped<CirugiasService>();
builder.Services.AddScoped<CitasService>();
builder.Services.AddScoped<ConsultasService>();
builder.Services.AddScoped<ControlesPostoperatoriosService>();
builder.Services.AddScoped<DetallesRecetaService>();
builder.Services.AddScoped<FotografiasService>();
builder.Services.AddScoped<GastosService>();
builder.Services.AddScoped<HistorialClinicoService>();

// Configuración de Controllers y OpenAPI
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configuración de OpenAPI
app.MapOpenApi();

// Configuración de Scalar
app.MapScalarApiReference(options =>
{
    options.WithOpenApiRoutePattern("/openapi/v1.json");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
