using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GestionCirugiaPlastica.Core.Models;

[Table("HistorialClinico")]
[Index("PacienteId", Name = "UQ__Historia__9353C01E9DC9A381", IsUnique = true)]
public partial class HistorialClinico
{
    [Key]
    public int HistoriaId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaActualizacion { get; set; }

    public int PacienteId { get; set; }

    public string? Antecedentes { get; set; }

    public string? Alergias { get; set; }

    public string? Medicamentos { get; set; }

    public string? NotasMedicas { get; set; }

    [ForeignKey("PacienteId")]
    [InverseProperty("HistorialClinico")]
    public virtual Paciente Paciente { get; set; } = null!;
}
