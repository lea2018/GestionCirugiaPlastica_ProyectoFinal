using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GestionCirugiaPlastica.API.Models;

[Index("Fecha", Name = "IX_Pagos_Fecha")]
public partial class Pago
{
    [Key]
    public int PagoId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Fecha { get; set; }

    public int? CirugiaId { get; set; }

    public int? CitaId { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    public decimal Monto { get; set; }

    [StringLength(250)]
    public string? MetodoPago { get; set; }

    [StringLength(250)]
    public string? Concepto { get; set; }

    [ForeignKey("CirugiaId")]
    [InverseProperty("Pagos")]
    public virtual Cirugia? Cirugia { get; set; }

    [ForeignKey("CitaId")]
    [InverseProperty("Pagos")]
    public virtual Cita? Cita { get; set; }
}
