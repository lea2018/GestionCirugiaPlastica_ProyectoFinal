using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GestionCirugiaPlastica.API.Models;

[Index("CirugiaId", Name = "IX_Fotografias_Cirugia")]
public partial class Fotografia
{
    [Key]
    public int FotografiaId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Fecha { get; set; }

    public int CirugiaId { get; set; }

    [StringLength(250)]
    public string? Tipo { get; set; }

    [StringLength(250)]
    public string RutaArchivo { get; set; } = null!;

    [StringLength(250)]
    public string? Descripcion { get; set; }

    [ForeignKey("CirugiaId")]
    [InverseProperty("Fotografia")]
    public virtual Cirugia Cirugia { get; set; } = null!;
}
