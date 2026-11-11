using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GestionCirugiaPlastica.Infrastructure.Models;

public partial class Preoperatorio
{
    [Key]
    public int PreoperatorioId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime FechaEvaluacion { get; set; }

    public int CirugiaId { get; set; }

    public string? AnalisisLaboratorio { get; set; }

    public string? EvaluacionCardiologica { get; set; }

    public string? EvaluacionAnestesica { get; set; }

    public bool Autorizado { get; set; }

    public string? Observaciones { get; set; }

    [ForeignKey("CirugiaId")]
    [InverseProperty("Preoperatorios")]
    public virtual Cirugia Cirugia { get; set; } = null!;
}
