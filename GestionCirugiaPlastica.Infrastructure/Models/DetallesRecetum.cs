using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GestionCirugiaPlastica.Infrastructure.Models;

public partial class DetallesRecetum
{
    [Key]
    public int DetalleRecetaId { get; set; }

    public int RecetaId { get; set; }

    [StringLength(250)]
    public string Medicamento { get; set; } = null!;

    [StringLength(250)]
    public string? Dosis { get; set; }

    [StringLength(250)]
    public string? Frecuencia { get; set; }

    [StringLength(250)]
    public string? Duracion { get; set; }

    [StringLength(250)]
    public string? Instrucciones { get; set; }

    [ForeignKey("RecetaId")]
    [InverseProperty("DetallesReceta")]
    public virtual Receta Receta { get; set; } = null!;
}
