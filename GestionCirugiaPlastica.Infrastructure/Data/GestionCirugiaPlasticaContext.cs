using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using GestionCirugiaPlastica.Infrastructure.Models;

namespace GestionCirugiaPlastica.Infrastructure.Data;

public partial class GestionCirugiaPlasticaContext : DbContext
{
    public GestionCirugiaPlasticaContext(DbContextOptions<GestionCirugiaPlasticaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cirugia> Cirugias { get; set; }

    public virtual DbSet<Cita> Citas { get; set; }

    public virtual DbSet<Consulta> Consultas { get; set; }

    public virtual DbSet<ControlesPostoperatorio> ControlesPostoperatorios { get; set; }

    public virtual DbSet<DetallesReceta> DetallesReceta { get; set; }

    public virtual DbSet<Fotografia> Fotografias { get; set; }

    public virtual DbSet<Gasto> Gastos { get; set; }

    public virtual DbSet<HistorialClinico> HistorialClinicos { get; set; }

    public virtual DbSet<HistorialEstadosCitum> HistorialEstadosCita { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    public virtual DbSet<PacienteSeguro> PacienteSeguros { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Preoperatorio> Preoperatorios { get; set; }

    public virtual DbSet<Procedimiento> Procedimientos { get; set; }

    public virtual DbSet<Receta> Recetas { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Seguro> Seguros { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cirugia>(entity =>
        {
            entity.HasKey(e => e.CirugiaId).HasName("PK__Cirugias__85A3D64EDC84E935");

            entity.Property(e => e.Estado).HasDefaultValue("Pendiente");

            entity.HasOne(d => d.Paciente).WithMany(p => p.Cirugia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cirugias_Pacientes");

            entity.HasOne(d => d.Procedimiento).WithMany(p => p.Cirugia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cirugias_Procedimientos");
        });

        modelBuilder.Entity<Cita>(entity =>
        {
            entity.HasKey(e => e.CitaId).HasName("PK__Citas__F0E2D9D2FFD4C917");

            entity.Property(e => e.Estado).HasDefaultValue("Pendiente");
            entity.Property(e => e.EstadoPago).HasDefaultValue("Pendiente");

            entity.HasOne(d => d.CitaAnterior).WithMany(p => p.InverseCitaAnterior).HasConstraintName("FK_Citas_CitaAnterior");

            entity.HasOne(d => d.Paciente).WithMany(p => p.Cita)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Citas_Pacientes");
        });

        modelBuilder.Entity<Consulta>(entity =>
        {
            entity.HasKey(e => e.ConsultaId).HasName("PK__Consulta__7D0B7DCC7AF23D61");

            entity.HasOne(d => d.Cita).WithMany(p => p.Consulta).HasConstraintName("FK_Consultas_Citas");

            entity.HasOne(d => d.Paciente).WithMany(p => p.Consulta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Consultas_Pacientes");
        });

        modelBuilder.Entity<ControlesPostoperatorio>(entity =>
        {
            entity.HasKey(e => e.ControlId).HasName("PK__Controle__091D9B355B120EA9");

            entity.HasOne(d => d.Cirugia).WithMany(p => p.ControlesPostoperatorios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ControlesPostoperatorios_Cirugias");
        });

        modelBuilder.Entity<DetallesReceta>(entity =>
        {
            entity.HasKey(e => e.DetalleRecetaId).HasName("PK__Detalles__DE1447049D073B39");

            entity.HasOne(d => d.Receta).WithMany(p => p.DetallesReceta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetallesReceta_Recetas");
        });

        modelBuilder.Entity<Fotografia>(entity =>
        {
            entity.HasKey(e => e.FotografiaId).HasName("PK__Fotograf__D2BA5284DCB8C0E3");

            entity.Property(e => e.Fecha).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Cirugia).WithMany(p => p.Fotografia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Fotografias_Cirugias");
        });

        modelBuilder.Entity<Gasto>(entity =>
        {
            entity.HasKey(e => e.GastoId).HasName("PK__Gastos__815BB0F09D1CA9BC");

            entity.Property(e => e.Fecha).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Gastos).HasConstraintName("FK_Gastos_Usuarios");
        });

        modelBuilder.Entity<HistorialClinico>(entity =>
        {
            entity.HasKey(e => e.HistoriaId).HasName("PK__Historia__0F6B1131FAF405DA");

            entity.Property(e => e.FechaActualizacion).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Paciente).WithOne(p => p.HistorialClinico)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistorialClinico_Pacientes");
        });

        modelBuilder.Entity<HistorialEstadosCitum>(entity =>
        {
            entity.HasKey(e => e.HistorialEstadoCitaId).HasName("PK__Historia__1C1561E562400B64");

            entity.Property(e => e.FechaCambio).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Cita).WithMany(p => p.HistorialEstadosCita)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HistorialEstadosCita_Citas");

            entity.HasOne(d => d.Usuario).WithMany(p => p.HistorialEstadosCita).HasConstraintName("FK_HistorialEstadosCita_Usuarios");
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.PacienteId).HasName("PK__Paciente__9353C01FF2D94C07");

            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<PacienteSeguro>(entity =>
        {
            entity.HasKey(e => e.PacienteSeguroId).HasName("PK__Paciente__E7A4EF8803C69A3D");

            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Paciente).WithMany(p => p.PacienteSeguros)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PacienteSeguros_Pacientes");

            entity.HasOne(d => d.Seguro).WithMany(p => p.PacienteSeguros)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PacienteSeguros_Seguros");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.PagoId).HasName("PK__Pagos__F00B61381094ED38");

            entity.Property(e => e.Fecha).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Cirugia).WithMany(p => p.Pagos).HasConstraintName("FK_Pagos_Cirugias");

            entity.HasOne(d => d.Cita).WithMany(p => p.Pagos).HasConstraintName("FK_Pagos_Citas");
        });

        modelBuilder.Entity<Preoperatorio>(entity =>
        {
            entity.HasKey(e => e.PreoperatorioId).HasName("PK__Preopera__4BEE3285C37D3D45");

            entity.Property(e => e.FechaEvaluacion).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Cirugia).WithMany(p => p.Preoperatorios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Preoperatorios_Cirugias");
        });

        modelBuilder.Entity<Procedimiento>(entity =>
        {
            entity.HasKey(e => e.ProcedimientoId).HasName("PK__Procedim__2CE1CED2FDAE77B4");

            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Receta>(entity =>
        {
            entity.HasKey(e => e.RecetaId).HasName("PK__Recetas__03D077D88935695F");

            entity.Property(e => e.Fecha).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Cirugia).WithMany(p => p.Receta).HasConstraintName("FK_Recetas_Cirugias");

            entity.HasOne(d => d.Consulta).WithMany(p => p.Receta).HasConstraintName("FK_Recetas_Consultas");

            entity.HasOne(d => d.Paciente).WithMany(p => p.Receta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Recetas_Pacientes");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Receta).HasConstraintName("FK_Recetas_Usuarios");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("PK__Roles__F92302F1E2B74607");
        });

        modelBuilder.Entity<Seguro>(entity =>
        {
            entity.HasKey(e => e.SeguroId).HasName("PK__Seguros__8B87D00A30431600");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE7B801F00105");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.FechaRegistro).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
