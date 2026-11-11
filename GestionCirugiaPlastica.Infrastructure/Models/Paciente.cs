using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GestionCirugiaPlastica.Infrastructure.Models;

[Index("Cedula", Name = "UQ__Paciente__B4ADFE385F036FD8", IsUnique = true)]
public partial class Paciente
{
    [Key]
    public int PacienteId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaRegistro { get; set; }

    [StringLength(250)]
    public string Nombre { get; set; } = null!;

    [StringLength(250)]
    public string Apellido { get; set; } = null!;

    [StringLength(20)]
    public string? Cedula { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    [StringLength(250)]
    public string? Telefono { get; set; }

    [StringLength(250)]
    public string? Email { get; set; }

    [StringLength(250)]
    public string? Direccion { get; set; }

    [InverseProperty("Paciente")]
    public virtual ICollection<Cirugia> Cirugia { get; set; } = new List<Cirugia>();

    [InverseProperty("Paciente")]
    public virtual ICollection<Cita> Cita { get; set; } = new List<Cita>();

    [InverseProperty("Paciente")]
    public virtual ICollection<Consulta> Consulta { get; set; } = new List<Consulta>();

    [InverseProperty("Paciente")]
    public virtual HistorialClinico? HistorialClinico { get; set; }

    [InverseProperty("Paciente")]
    public virtual ICollection<PacienteSeguro> PacienteSeguros { get; set; } = new List<PacienteSeguro>();

    [InverseProperty("Paciente")]
    public virtual ICollection<Receta> Receta { get; set; } = new List<Receta>();
}
