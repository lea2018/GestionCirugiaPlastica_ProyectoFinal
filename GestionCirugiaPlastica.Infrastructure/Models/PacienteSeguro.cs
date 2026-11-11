using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GestionCirugiaPlastica.Infrastructure.Models;

public partial class PacienteSeguro
{
    [Key]
    public int PacienteSeguroId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaRegistro { get; set; }

    public int PacienteId { get; set; }

    public int SeguroId { get; set; }

    [StringLength(250)]
    public string? NumeroPoliza { get; set; }

    [StringLength(250)]
    public string? Titular { get; set; }

    public DateOnly? FechaVencimiento { get; set; }

    [StringLength(250)]
    public string? Observaciones { get; set; }

    [ForeignKey("PacienteId")]
    [InverseProperty("PacienteSeguros")]
    public virtual Paciente Paciente { get; set; } = null!;

    [ForeignKey("SeguroId")]
    [InverseProperty("PacienteSeguros")]
    public virtual Seguro Seguro { get; set; } = null!;
}
