using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GestionCirugiaPlastica.API.Models;

[Index("PacienteId", "Fecha", Name = "IX_Consultas_Paciente_Fecha")]
public partial class Consulta
{
    [Key]
    public int ConsultaId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Fecha { get; set; }

    public int PacienteId { get; set; }

    public int? CitaId { get; set; }

    [StringLength(250)]
    public string? Motivo { get; set; }

    public string? Diagnostico { get; set; }

    public string? Observaciones { get; set; }

    [ForeignKey("CitaId")]
    [InverseProperty("Consulta")]
    public virtual Cita? Cita { get; set; }

    [ForeignKey("PacienteId")]
    [InverseProperty("Consulta")]
    public virtual Paciente Paciente { get; set; } = null!;

    [InverseProperty("Consulta")]
    public virtual ICollection<Receta> Receta { get; set; } = new List<Receta>();
}
