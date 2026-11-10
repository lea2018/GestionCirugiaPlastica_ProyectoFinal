using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GestionCirugiaPlastica.API.Models;

[Index("Nombre", Name = "UQ__Roles__75E3EFCF410496BE", IsUnique = true)]
public partial class Role
{
    [Key]
    public int RolId { get; set; }

    [StringLength(250)]
    public string Nombre { get; set; } = null!;

    [StringLength(250)]
    public string? Descripcion { get; set; }

    [InverseProperty("Rol")]
    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
