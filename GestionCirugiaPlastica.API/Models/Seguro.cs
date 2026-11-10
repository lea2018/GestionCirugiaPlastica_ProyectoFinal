using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GestionCirugiaPlastica.API.Models;

public partial class Seguro
{
    [Key]
    public int SeguroId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaRegistro { get; set; }

    [StringLength(250)]
    public string Nombre { get; set; } = null!;

    [StringLength(250)]
    public string? Telefono { get; set; }

    [StringLength(250)]
    public string? Email { get; set; }

    [StringLength(250)]
    public string? Direccion { get; set; }

    public bool Activo { get; set; }

    [InverseProperty("Seguro")]
    public virtual ICollection<PacienteSeguro> PacienteSeguros { get; set; } = new List<PacienteSeguro>();
}
