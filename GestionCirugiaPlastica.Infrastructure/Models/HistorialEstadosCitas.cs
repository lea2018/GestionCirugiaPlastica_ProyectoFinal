using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GestionCirugiaPlastica.Infrastructure.Models;

public partial class HistorialEstadosCitas
{
    [Key]
    public int HistorialEstadoCitaId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaCambio { get; set; }

    public int CitaId { get; set; }

    [StringLength(250)]
    public string? EstadoAnterior { get; set; }

    [StringLength(250)]
    public string EstadoNuevo { get; set; } = null!;

    public int? UsuarioId { get; set; }

    [StringLength(250)]
    public string? Observaciones { get; set; }

    [ForeignKey("CitaId")]
    [InverseProperty("HistorialEstadosCita")]
    public virtual Cita Cita { get; set; } = null!;

    [ForeignKey("UsuarioId")]
    [InverseProperty("HistorialEstadosCita")]
    public virtual Usuario? Usuario { get; set; }
}
