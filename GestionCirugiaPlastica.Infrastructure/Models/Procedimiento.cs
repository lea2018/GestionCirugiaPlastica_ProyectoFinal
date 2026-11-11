using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GestionCirugiaPlastica.Infrastructure.Models;

public partial class Procedimiento
{
    [Key]
    public int ProcedimientoId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaRegistro { get; set; }

    [StringLength(250)]
    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    public decimal PrecioBase { get; set; }

    [InverseProperty("Procedimiento")]
    public virtual ICollection<Cirugia> Cirugia { get; set; } = new List<Cirugia>();
}
