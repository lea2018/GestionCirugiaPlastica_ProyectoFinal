using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GestionCirugiaPlastica.Core.Models;

[Index("PacienteId", "FechaCirugia", Name = "IX_Cirugias_Paciente_Fecha")]
public partial class Cirugia
{
    [Key]
    public int CirugiaId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaCirugia { get; set; }

    public int PacienteId { get; set; }

    public int ProcedimientoId { get; set; }

    public string? DiagnosticoPreoperatorio { get; set; }

    public string? Observaciones { get; set; }

    [StringLength(250)]
    public string Estado { get; set; } = null!;

    [InverseProperty("Cirugia")]
    public virtual ICollection<ControlesPostoperatorio> ControlesPostoperatorios { get; set; } = new List<ControlesPostoperatorio>();

    [InverseProperty("Cirugia")]
    public virtual ICollection<Fotografia> Fotografia { get; set; } = new List<Fotografia>();

    [ForeignKey("PacienteId")]
    [InverseProperty("Cirugia")]
    public virtual Paciente Paciente { get; set; } = null!;

    [InverseProperty("Cirugia")]
    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    [InverseProperty("Cirugia")]
    public virtual ICollection<Preoperatorio> Preoperatorios { get; set; } = new List<Preoperatorio>();

    [ForeignKey("ProcedimientoId")]
    [InverseProperty("Cirugia")]
    public virtual Procedimiento Procedimiento { get; set; } = null!;

    [InverseProperty("Cirugia")]
    public virtual ICollection<Receta> Receta { get; set; } = new List<Receta>();
}
