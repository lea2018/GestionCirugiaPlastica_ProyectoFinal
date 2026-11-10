using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GestionCirugiaPlastica.API.Models;

[Index("Email", Name = "UQ__Usuarios__A9D10534CD4693E6", IsUnique = true)]
public partial class Usuario
{
    [Key]
    public int UsuarioId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaRegistro { get; set; }

    [StringLength(250)]
    public string Nombre { get; set; } = null!;

    [StringLength(250)]
    public string Apellido { get; set; } = null!;

    [StringLength(250)]
    public string Email { get; set; } = null!;

    [StringLength(250)]
    public string PasswordHash { get; set; } = null!;

    public int RolId { get; set; }

    public bool Activo { get; set; }

    [InverseProperty("Usuario")]
    public virtual ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();

    [InverseProperty("Usuario")]
    public virtual ICollection<HistorialEstadosCitum> HistorialEstadosCita { get; set; } = new List<HistorialEstadosCitum>();

    [InverseProperty("Usuario")]
    public virtual ICollection<Receta> Receta { get; set; } = new List<Receta>();

    [ForeignKey("RolId")]
    [InverseProperty("Usuarios")]
    public virtual Role Rol { get; set; } = null!;
}
