using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GestionCirugiaPlastica.Infrastructure.Models;

[Index("CirugiaId", "Fecha", Name = "IX_Controles_Cirugia_Fecha")]
public partial class ControlesPostoperatorio
{
    [Key]
    public int ControlId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Fecha { get; set; }

    public int CirugiaId { get; set; }

    public int? DiasPostoperatorio { get; set; }

    public string? Evolucion { get; set; }

    [StringLength(250)]
    public string? Dolor { get; set; }

    [StringLength(250)]
    public string? Inflamacion { get; set; }

    public string? Observaciones { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? ProximoControl { get; set; }

    [ForeignKey("CirugiaId")]
    [InverseProperty("ControlesPostoperatorios")]
    public virtual Cirugia Cirugia { get; set; } = null!;
}
