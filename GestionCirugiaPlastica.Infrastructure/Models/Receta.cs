using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GestionCirugiaPlastica.Infrastructure.Models;

public partial class Receta
{
    [Key]
    public int RecetaId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Fecha { get; set; }

    public int PacienteId { get; set; }

    public int? ConsultaId { get; set; }

    public int? CirugiaId { get; set; }

    public int? UsuarioId { get; set; }

    public string? Indicaciones { get; set; }

    [ForeignKey("CirugiaId")]
    [InverseProperty("Receta")]
    public virtual Cirugia? Cirugia { get; set; }

    [ForeignKey("ConsultaId")]
    [InverseProperty("Receta")]
    public virtual Consulta? Consulta { get; set; }

    [InverseProperty("Receta")]
    public virtual ICollection<DetallesRecetum> DetallesReceta { get; set; } = new List<DetallesRecetum>();

    [ForeignKey("PacienteId")]
    [InverseProperty("Receta")]
    public virtual Paciente Paciente { get; set; } = null!;

    [ForeignKey("UsuarioId")]
    [InverseProperty("Receta")]
    public virtual Usuario? Usuario { get; set; }
}
