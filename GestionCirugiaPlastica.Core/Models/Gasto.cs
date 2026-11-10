using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GestionCirugiaPlastica.Core.Models;

public partial class Gasto
{
    [Key]
    public int GastoId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Fecha { get; set; }

    [StringLength(250)]
    public string Concepto { get; set; } = null!;

    [StringLength(250)]
    public string? Categoria { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    public decimal Monto { get; set; }

    [StringLength(250)]
    public string? MetodoPago { get; set; }

    public string? Observaciones { get; set; }

    public int? UsuarioId { get; set; }

    [ForeignKey("UsuarioId")]
    [InverseProperty("Gastos")]
    public virtual Usuario? Usuario { get; set; }
}
