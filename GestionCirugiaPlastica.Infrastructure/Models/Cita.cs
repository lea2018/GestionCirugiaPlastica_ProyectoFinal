using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GestionCirugiaPlastica.Infrastructure.Models;

[Index("PacienteId", "Fecha", Name = "IX_Citas_Paciente_Fecha")]
public partial class Cita
{
    [Key]
    public int CitaId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Fecha { get; set; }

    public int PacienteId { get; set; }

    [StringLength(250)]
    public string? TipoCita { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    public decimal Costo { get; set; }

    [StringLength(250)]
    public string Estado { get; set; } = null!;

    [StringLength(250)]
    public string EstadoPago { get; set; } = null!;

    public string? Observaciones { get; set; }

    public int? CitaAnteriorId { get; set; }

    [ForeignKey("CitaAnteriorId")]
    [InverseProperty("InverseCitaAnterior")]
    public virtual Cita? CitaAnterior { get; set; }

    [InverseProperty("Cita")]
    public virtual ICollection<Consulta> Consulta { get; set; } = new List<Consulta>();

    [InverseProperty("Cita")]
    public virtual ICollection<HistorialEstadosCitas> HistorialEstadosCita { get; set; } = new List<HistorialEstadosCitas>();

    [InverseProperty("CitaAnterior")]
    public virtual ICollection<Cita> InverseCitaAnterior { get; set; } = new List<Cita>();

    [ForeignKey("PacienteId")]
    [InverseProperty("Cita")]
    public virtual Paciente Paciente { get; set; } = null!;

    [InverseProperty("Cita")]
    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}
