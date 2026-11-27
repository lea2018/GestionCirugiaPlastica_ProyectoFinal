/* ============================================================================
   GestionCirugiaPlastica (Tanger Clinic) — Script Maestro Idempotente
   Archivo: Scrip/BaseDatosCompleta_Azure.sql
   Compatibilidad: Azure SQL Database y SQL Server local (2019 / 2022 / Express)

   IMPORTANTE PARA AZURE SQL DATABASE:
   - NO incluye comandos USE [NombreBD] ni CREATE DATABASE, ya que Azure SQL
     Database no permite cambiar el contexto de base de datos con USE.
   - Conéctese directamente a la base de datos destino (vacía o existente)
     desde Azure Portal Query Editor, SSMS o Azure Data Studio y ejecute todo.
   - Es 100% idempotente: puede ejecutarse múltiples veces sin errores ni
     duplicación de tablas, restricciones, índices o datos semilla.

   Esquema sincronizado con GestionCirugiaPlasticaContext.cs (22 tablas):
     Roles, Pacientes, Seguros, Procedimientos, Usuarios, PacienteSeguros,
     HistorialClinico, Citas, HistorialEstadosCita, Consultas, Cirugias,
     Preoperatorios, ControlesPostoperatorios, Fotografias, Recetas,
     DetallesReceta, Pagos, Gastos, DocumentosClinicos, AuditoriaExpediente,
     Notificaciones, Noticias.
   ============================================================================ */

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
SET NOCOUNT ON;
SET XACT_ABORT ON;
GO

/* ============================================================================
   SECCIÓN 1: CREACIÓN DE TABLAS (22 ENTIDADES DEL CONTEXTO EF CORE)
   ============================================================================ */

-- 1. Roles
IF OBJECT_ID(N'dbo.Roles', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Roles
    (
        RolId        int IDENTITY(1,1) NOT NULL,
        Nombre       nvarchar(250)     NOT NULL,
        Descripcion  nvarchar(250)     NULL,
        CONSTRAINT PK_Roles PRIMARY KEY CLUSTERED (RolId ASC)
    );
END;
GO

-- 2. Pacientes (incluye TieneSeguro y Sexo)
IF OBJECT_ID(N'dbo.Pacientes', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Pacientes
    (
        PacienteId       int IDENTITY(1,1) NOT NULL,
        FechaRegistro    datetime          NOT NULL CONSTRAINT DF_Pacientes_FechaRegistro DEFAULT (GETDATE()),
        Nombre           nvarchar(250)     NOT NULL,
        Apellido         nvarchar(250)     NOT NULL,
        Cedula           nvarchar(20)      NULL,
        FechaNacimiento  date              NULL,
        TieneSeguro      bit               NOT NULL CONSTRAINT DF_Pacientes_TieneSeguro DEFAULT (0),
        Sexo             nvarchar(max)     NULL,
        Telefono         nvarchar(250)     NULL,
        Email            nvarchar(250)     NULL,
        Direccion        nvarchar(250)     NULL,
        CONSTRAINT PK_Pacientes PRIMARY KEY CLUSTERED (PacienteId ASC)
    );
END;
GO

-- 3. Seguros (Aseguradoras / ARS)
IF OBJECT_ID(N'dbo.Seguros', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Seguros
    (
        SeguroId       int IDENTITY(1,1) NOT NULL,
        FechaRegistro  datetime          NOT NULL CONSTRAINT DF_Seguros_FechaRegistro DEFAULT (GETDATE()),
        Nombre         nvarchar(250)     NOT NULL,
        Telefono       nvarchar(250)     NULL,
        Email          nvarchar(250)     NULL,
        Direccion      nvarchar(250)     NULL,
        Activo         bit               NOT NULL CONSTRAINT DF_Seguros_Activo DEFAULT (1),
        CONSTRAINT PK_Seguros PRIMARY KEY CLUSTERED (SeguroId ASC)
    );
END;
GO

-- 4. Procedimientos (Catálogo de procedimientos quirúrgicos/estéticos)
IF OBJECT_ID(N'dbo.Procedimientos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Procedimientos
    (
        ProcedimientoId  int IDENTITY(1,1) NOT NULL,
        FechaRegistro    datetime          NOT NULL CONSTRAINT DF_Procedimientos_FechaRegistro DEFAULT (GETDATE()),
        Nombre           nvarchar(250)     NOT NULL,
        Descripcion      nvarchar(max)     NULL,
        PrecioBase       decimal(12, 2)    NOT NULL,
        CONSTRAINT PK_Procedimientos PRIMARY KEY CLUSTERED (ProcedimientoId ASC)
    );
END;
GO

-- 5. Usuarios
IF OBJECT_ID(N'dbo.Usuarios', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Usuarios
    (
        UsuarioId      int IDENTITY(1,1) NOT NULL,
        FechaRegistro  datetime          NOT NULL CONSTRAINT DF_Usuarios_FechaRegistro DEFAULT (GETDATE()),
        Nombre         nvarchar(250)     NOT NULL,
        Apellido       nvarchar(250)     NOT NULL,
        Email          nvarchar(250)     NULL,
        FotoUrl        nvarchar(500)     NULL,
        NombreUsuario  nvarchar(100)     NULL,
        PacienteId     int               NULL,
        PasswordHash   nvarchar(250)     NOT NULL,
        RolId          int               NOT NULL,
        Activo         bit               NOT NULL CONSTRAINT DF_Usuarios_Activo DEFAULT (1),
        CONSTRAINT PK_Usuarios PRIMARY KEY CLUSTERED (UsuarioId ASC)
    );
END;
GO

-- 6. PacienteSeguros (Pólizas vinculadas a cada paciente)
IF OBJECT_ID(N'dbo.PacienteSeguros', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.PacienteSeguros
    (
        PacienteSeguroId  int IDENTITY(1,1) NOT NULL,
        FechaRegistro     datetime          NOT NULL CONSTRAINT DF_PacienteSeguros_FechaRegistro DEFAULT (GETDATE()),
        PacienteId        int               NOT NULL,
        SeguroId          int               NOT NULL,
        NumeroPoliza      nvarchar(250)     NULL,
        Titular           nvarchar(250)     NULL,
        FechaVencimiento  date              NULL,
        Observaciones     nvarchar(250)     NULL,
        CONSTRAINT PK_PacienteSeguros PRIMARY KEY CLUSTERED (PacienteSeguroId ASC)
    );
END;
GO

-- 7. HistorialClinico (relación 1:1 con Pacientes)
IF OBJECT_ID(N'dbo.HistorialClinico', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.HistorialClinico
    (
        HistoriaId              int IDENTITY(1,1) NOT NULL,
        FechaActualizacion      datetime          NOT NULL CONSTRAINT DF_HistorialClinico_FechaActualizacion DEFAULT (GETDATE()),
        PacienteId              int               NOT NULL,
        Antecedentes            nvarchar(max)     NULL,
        AntecedentesPersonales  nvarchar(max)     NULL,
        AntecedentesFamiliares  nvarchar(max)     NULL,
        Alergias                nvarchar(max)     NULL,
        Medicamentos            nvarchar(max)     NULL,
        NotasMedicas            nvarchar(max)     NULL,
        CONSTRAINT PK_HistorialClinico PRIMARY KEY CLUSTERED (HistoriaId ASC)
    );
END;
GO

-- 8. Citas
IF OBJECT_ID(N'dbo.Citas', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Citas
    (
        CitaId          int IDENTITY(1,1) NOT NULL,
        Fecha           datetime          NOT NULL,
        PacienteId      int               NOT NULL,
        TipoCita        nvarchar(250)     NULL,
        Costo           decimal(12, 2)    NOT NULL,
        Estado          nvarchar(250)     NOT NULL CONSTRAINT DF_Citas_Estado DEFAULT (N'Pendiente'),
        EstadoPago      nvarchar(250)     NOT NULL CONSTRAINT DF_Citas_EstadoPago DEFAULT (N'Pendiente'),
        CodigoPublico   nvarchar(32)      NULL,
        Observaciones   nvarchar(max)     NULL,
        CitaAnteriorId  int               NULL,
        CONSTRAINT PK_Citas PRIMARY KEY CLUSTERED (CitaId ASC)
    );
END;
GO

-- 9. HistorialEstadosCita (nombre físico exacto mapeado por EF Core)
IF OBJECT_ID(N'dbo.HistorialEstadosCita', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.HistorialEstadosCita
    (
        HistorialEstadoCitaId  int IDENTITY(1,1) NOT NULL,
        FechaCambio            datetime          NOT NULL CONSTRAINT DF_HistorialEstadosCita_FechaCambio DEFAULT (GETDATE()),
        CitaId                 int               NOT NULL,
        EstadoAnterior         nvarchar(250)     NULL,
        EstadoNuevo            nvarchar(250)     NOT NULL,
        UsuarioId              int               NULL,
        Observaciones          nvarchar(250)     NULL,
        CONSTRAINT PK_HistorialEstadosCita PRIMARY KEY CLUSTERED (HistorialEstadoCitaId ASC)
    );
END;
GO

-- 10. Consultas
IF OBJECT_ID(N'dbo.Consultas', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Consultas
    (
        ConsultaId     int IDENTITY(1,1) NOT NULL,
        Fecha          datetime          NOT NULL,
        PacienteId     int               NOT NULL,
        CitaId         int               NULL,
        Motivo         nvarchar(250)     NULL,
        Diagnostico    nvarchar(max)     NULL,
        Observaciones  nvarchar(max)     NULL,
        CONSTRAINT PK_Consultas PRIMARY KEY CLUSTERED (ConsultaId ASC)
    );
END;
GO

-- 11. Cirugias
IF OBJECT_ID(N'dbo.Cirugias', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Cirugias
    (
        CirugiaId                 int IDENTITY(1,1) NOT NULL,
        FechaCirugia              datetime          NOT NULL,
        PacienteId                int               NOT NULL,
        ProcedimientoId           int               NOT NULL,
        DiagnosticoPreoperatorio  nvarchar(max)     NULL,
        Observaciones             nvarchar(max)     NULL,
        Estado                    nvarchar(250)     NOT NULL CONSTRAINT DF_Cirugias_Estado DEFAULT (N'Pendiente'),
        CONSTRAINT PK_Cirugias PRIMARY KEY CLUSTERED (CirugiaId ASC)
    );
END;
GO

-- 12. Preoperatorios (Evaluaciones preoperatorias)
IF OBJECT_ID(N'dbo.Preoperatorios', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Preoperatorios
    (
        PreoperatorioId         int IDENTITY(1,1) NOT NULL,
        FechaEvaluacion         datetime          NOT NULL CONSTRAINT DF_Preoperatorios_FechaEvaluacion DEFAULT (GETDATE()),
        CirugiaId               int               NOT NULL,
        AnalisisLaboratorio     nvarchar(max)     NULL,
        EvaluacionCardiologica  nvarchar(max)     NULL,
        EvaluacionAnestesica    nvarchar(max)     NULL,
        Autorizado              bit               NOT NULL CONSTRAINT DF_Preoperatorios_Autorizado DEFAULT (0),
        Observaciones           nvarchar(max)     NULL,
        CONSTRAINT PK_Preoperatorios PRIMARY KEY CLUSTERED (PreoperatorioId ASC)
    );
END;
GO

-- 13. ControlesPostoperatorios
IF OBJECT_ID(N'dbo.ControlesPostoperatorios', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ControlesPostoperatorios
    (
        ControlId           int IDENTITY(1,1) NOT NULL,
        Fecha               datetime          NOT NULL,
        CirugiaId           int               NOT NULL,
        DiasPostoperatorio  int               NULL,
        Evolucion           nvarchar(max)     NULL,
        Dolor               nvarchar(250)     NULL,
        Inflamacion         nvarchar(250)     NULL,
        Observaciones       nvarchar(max)     NULL,
        ProximoControl      datetime          NULL,
        CONSTRAINT PK_ControlesPostoperatorios PRIMARY KEY CLUSTERED (ControlId ASC)
    );
END;
GO

-- 14. Fotografias (Fotografías clínicas asociadas a cirugías)
IF OBJECT_ID(N'dbo.Fotografias', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Fotografias
    (
        FotografiaId  int IDENTITY(1,1) NOT NULL,
        Fecha         datetime          NOT NULL CONSTRAINT DF_Fotografias_Fecha DEFAULT (GETDATE()),
        CirugiaId     int               NOT NULL,
        Tipo          nvarchar(250)     NULL,
        RutaArchivo   nvarchar(250)     NOT NULL,
        Descripcion   nvarchar(250)     NULL,
        CONSTRAINT PK_Fotografias PRIMARY KEY CLUSTERED (FotografiaId ASC)
    );
END;
GO

-- 15. Recetas
IF OBJECT_ID(N'dbo.Recetas', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Recetas
    (
        RecetaId      int IDENTITY(1,1) NOT NULL,
        Fecha         datetime          NOT NULL CONSTRAINT DF_Recetas_Fecha DEFAULT (GETDATE()),
        PacienteId    int               NOT NULL,
        ConsultaId    int               NULL,
        CirugiaId     int               NULL,
        UsuarioId     int               NULL,
        Indicaciones  nvarchar(max)     NULL,
        CONSTRAINT PK_Recetas PRIMARY KEY CLUSTERED (RecetaId ASC)
    );
END;
GO

-- 16. DetallesReceta
IF OBJECT_ID(N'dbo.DetallesReceta', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.DetallesReceta
    (
        DetalleRecetaId  int IDENTITY(1,1) NOT NULL,
        RecetaId         int               NOT NULL,
        Medicamento      nvarchar(250)     NOT NULL,
        Dosis            nvarchar(250)     NULL,
        Frecuencia       nvarchar(250)     NULL,
        Duracion         nvarchar(250)     NULL,
        Instrucciones    nvarchar(250)     NULL,
        CONSTRAINT PK_DetallesReceta PRIMARY KEY CLUSTERED (DetalleRecetaId ASC)
    );
END;
GO

-- 17. Pagos
IF OBJECT_ID(N'dbo.Pagos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Pagos
    (
        PagoId       int IDENTITY(1,1) NOT NULL,
        Fecha        datetime          NOT NULL CONSTRAINT DF_Pagos_Fecha DEFAULT (GETDATE()),
        CirugiaId    int               NULL,
        CitaId       int               NULL,
        PacienteId   int               NULL,
        SeguroId     int               NULL,
        Monto        decimal(12, 2)    NOT NULL,
        MontoSeguro  decimal(12, 2)    NOT NULL CONSTRAINT DF_Pagos_MontoSeguro DEFAULT (0.00),
        Estado       nvarchar(250)     NOT NULL CONSTRAINT DF_Pagos_Estado DEFAULT (N'Pagado'),
        MetodoPago   nvarchar(250)     NULL,
        Concepto     nvarchar(250)     NULL,
        CONSTRAINT PK_Pagos PRIMARY KEY CLUSTERED (PagoId ASC)
    );
END;
GO

-- 18. Gastos
IF OBJECT_ID(N'dbo.Gastos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Gastos
    (
        GastoId        int IDENTITY(1,1) NOT NULL,
        Fecha          datetime          NOT NULL CONSTRAINT DF_Gastos_Fecha DEFAULT (GETDATE()),
        Concepto       nvarchar(250)     NOT NULL,
        Categoria      nvarchar(250)     NULL,
        Monto          decimal(12, 2)    NOT NULL,
        MetodoPago     nvarchar(250)     NULL,
        Observaciones  nvarchar(max)     NULL,
        UsuarioId      int               NULL,
        CONSTRAINT PK_Gastos PRIMARY KEY CLUSTERED (GastoId ASC)
    );
END;
GO

-- 19. DocumentosClinicos
IF OBJECT_ID(N'dbo.DocumentosClinicos', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.DocumentosClinicos
    (
        DocumentoClinicoId  int IDENTITY(1,1) NOT NULL,
        PacienteId          int               NOT NULL,
        CirugiaId           int               NULL,
        ConsultaId          int               NULL,
        Categoria           nvarchar(40)      NOT NULL CONSTRAINT DF_DocumentosClinicos_Categoria DEFAULT (N'Otro'),
        NombreArchivo       nvarchar(250)     NOT NULL,
        ContentType         nvarchar(120)     NOT NULL,
        RutaArchivo         nvarchar(500)     NOT NULL,
        Descripcion         nvarchar(500)     NULL,
        FechaCarga          datetime2         NOT NULL CONSTRAINT DF_DocumentosClinicos_FechaCarga DEFAULT (SYSUTCDATETIME()),
        UsuarioId           int               NOT NULL,
        CONSTRAINT PK_DocumentosClinicos PRIMARY KEY CLUSTERED (DocumentoClinicoId ASC)
    );
END;
GO

-- 20. AuditoriaExpediente
IF OBJECT_ID(N'dbo.AuditoriaExpediente', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.AuditoriaExpediente
    (
        AuditoriaExpedienteId  bigint IDENTITY(1,1) NOT NULL,
        PacienteId             int                  NOT NULL,
        UsuarioId              int                  NOT NULL,
        Fecha                  datetime2            NOT NULL CONSTRAINT DF_AuditoriaExpediente_Fecha DEFAULT (SYSUTCDATETIME()),
        Entidad                nvarchar(80)         NOT NULL,
        RegistroId             int                  NULL,
        Accion                 nvarchar(40)         NOT NULL,
        Detalle                nvarchar(500)        NULL,
        CONSTRAINT PK_AuditoriaExpediente PRIMARY KEY CLUSTERED (AuditoriaExpedienteId ASC)
    );
END;
GO

-- 21. Notificaciones
IF OBJECT_ID(N'dbo.Notificaciones', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Notificaciones
    (
        NotificacionId  int IDENTITY(1,1) NOT NULL,
        Tipo            nvarchar(50)      NOT NULL,
        Titulo          nvarchar(200)     NOT NULL,
        Mensaje         nvarchar(1000)    NOT NULL,
        CitaId          int               NULL,
        FechaCreacion   datetime2         NOT NULL CONSTRAINT DF_Notificaciones_FechaCreacion DEFAULT (GETDATE()),
        Leida           bit               NOT NULL CONSTRAINT DF_Notificaciones_Leida DEFAULT (0),
        CONSTRAINT PK_Notificaciones PRIMARY KEY CLUSTERED (NotificacionId ASC)
    );
END;
GO

-- 22. Noticias
IF OBJECT_ID(N'dbo.Noticias', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Noticias
    (
        NoticiaId           int IDENTITY(1,1) NOT NULL,
        Titulo              nvarchar(200)     NOT NULL,
        Resumen             nvarchar(500)     NULL,
        Contenido           nvarchar(max)     NOT NULL,
        FechaPublicacion    datetime2         NOT NULL CONSTRAINT DF_Noticias_FechaPublicacion DEFAULT (GETDATE()),
        Publicada           bit               NOT NULL CONSTRAINT DF_Noticias_Publicada DEFAULT (0),
        CreadaPorUsuarioId  int               NULL,
        CONSTRAINT PK_Noticias PRIMARY KEY CLUSTERED (NoticiaId ASC)
    );
END;
GO

/* ============================================================================
   SECCIÓN 2: SINCRONIZACIÓN ADITIVA DE COLUMNAS (SI LAS TABLAS YA EXISTÍAN)
   ============================================================================ */

IF COL_LENGTH(N'dbo.Pacientes', N'TieneSeguro') IS NULL
    ALTER TABLE dbo.Pacientes ADD TieneSeguro bit NOT NULL CONSTRAINT DF_Pacientes_TieneSeguro_Sync DEFAULT (0) WITH VALUES;
IF COL_LENGTH(N'dbo.Pacientes', N'Sexo') IS NULL
    ALTER TABLE dbo.Pacientes ADD Sexo nvarchar(max) NULL;

IF COL_LENGTH(N'dbo.HistorialClinico', N'AntecedentesPersonales') IS NULL
    ALTER TABLE dbo.HistorialClinico ADD AntecedentesPersonales nvarchar(max) NULL;
IF COL_LENGTH(N'dbo.HistorialClinico', N'AntecedentesFamiliares') IS NULL
    ALTER TABLE dbo.HistorialClinico ADD AntecedentesFamiliares nvarchar(max) NULL;

IF COL_LENGTH(N'dbo.Citas', N'CodigoPublico') IS NULL
    ALTER TABLE dbo.Citas ADD CodigoPublico nvarchar(32) NULL;

IF COL_LENGTH(N'dbo.Usuarios', N'NombreUsuario') IS NULL
    ALTER TABLE dbo.Usuarios ADD NombreUsuario nvarchar(100) NULL;
IF COL_LENGTH(N'dbo.Usuarios', N'PacienteId') IS NULL
    ALTER TABLE dbo.Usuarios ADD PacienteId int NULL;
IF COL_LENGTH(N'dbo.Usuarios', N'FotoUrl') IS NULL
    ALTER TABLE dbo.Usuarios ADD FotoUrl nvarchar(500) NULL;
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.Usuarios') AND name = N'Email' AND is_nullable = 0)
BEGIN
    IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Usuarios') AND name = N'UX_Usuarios_Email_NotNull')
        DROP INDEX UX_Usuarios_Email_NotNull ON dbo.Usuarios;
    ALTER TABLE dbo.Usuarios ALTER COLUMN Email nvarchar(250) NULL;
END;

IF COL_LENGTH(N'dbo.Pagos', N'PacienteId') IS NULL
    ALTER TABLE dbo.Pagos ADD PacienteId int NULL;
IF COL_LENGTH(N'dbo.Pagos', N'SeguroId') IS NULL
    ALTER TABLE dbo.Pagos ADD SeguroId int NULL;
IF COL_LENGTH(N'dbo.Pagos', N'MontoSeguro') IS NULL
    ALTER TABLE dbo.Pagos ADD MontoSeguro decimal(12, 2) NOT NULL CONSTRAINT DF_Pagos_MontoSeguro_Sync DEFAULT (0.00) WITH VALUES;
IF COL_LENGTH(N'dbo.Pagos', N'Estado') IS NULL
    ALTER TABLE dbo.Pagos ADD Estado nvarchar(250) NOT NULL CONSTRAINT DF_Pagos_Estado_Sync DEFAULT (N'Pagado') WITH VALUES;
GO

/* ============================================================================
   SECCIÓN 3: FOREIGN KEYS Y CHECK CONSTRAINTS (IDEMPOTENTES)
   ============================================================================ */

-- Usuarios -> Roles, Pacientes
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Usuarios_Roles')
    ALTER TABLE dbo.Usuarios WITH CHECK ADD CONSTRAINT FK_Usuarios_Roles
        FOREIGN KEY (RolId) REFERENCES dbo.Roles (RolId);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Usuarios_Pacientes')
    ALTER TABLE dbo.Usuarios WITH CHECK ADD CONSTRAINT FK_Usuarios_Pacientes
        FOREIGN KEY (PacienteId) REFERENCES dbo.Pacientes (PacienteId) ON DELETE NO ACTION;

-- PacienteSeguros -> Pacientes, Seguros
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_PacienteSeguros_Pacientes')
    ALTER TABLE dbo.PacienteSeguros WITH CHECK ADD CONSTRAINT FK_PacienteSeguros_Pacientes
        FOREIGN KEY (PacienteId) REFERENCES dbo.Pacientes (PacienteId);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_PacienteSeguros_Seguros')
    ALTER TABLE dbo.PacienteSeguros WITH CHECK ADD CONSTRAINT FK_PacienteSeguros_Seguros
        FOREIGN KEY (SeguroId) REFERENCES dbo.Seguros (SeguroId);

-- HistorialClinico -> Pacientes
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_HistorialClinico_Pacientes')
    ALTER TABLE dbo.HistorialClinico WITH CHECK ADD CONSTRAINT FK_HistorialClinico_Pacientes
        FOREIGN KEY (PacienteId) REFERENCES dbo.Pacientes (PacienteId);

-- Citas -> Pacientes, CitaAnterior
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Citas_Pacientes')
    ALTER TABLE dbo.Citas WITH CHECK ADD CONSTRAINT FK_Citas_Pacientes
        FOREIGN KEY (PacienteId) REFERENCES dbo.Pacientes (PacienteId);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Citas_CitaAnterior')
    ALTER TABLE dbo.Citas WITH CHECK ADD CONSTRAINT FK_Citas_CitaAnterior
        FOREIGN KEY (CitaAnteriorId) REFERENCES dbo.Citas (CitaId);

-- HistorialEstadosCita -> Citas, Usuarios
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_HistorialEstadosCita_Citas')
    ALTER TABLE dbo.HistorialEstadosCita WITH CHECK ADD CONSTRAINT FK_HistorialEstadosCita_Citas
        FOREIGN KEY (CitaId) REFERENCES dbo.Citas (CitaId);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_HistorialEstadosCita_Usuarios')
    ALTER TABLE dbo.HistorialEstadosCita WITH CHECK ADD CONSTRAINT FK_HistorialEstadosCita_Usuarios
        FOREIGN KEY (UsuarioId) REFERENCES dbo.Usuarios (UsuarioId);

-- Consultas -> Pacientes, Citas
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Consultas_Pacientes')
    ALTER TABLE dbo.Consultas WITH CHECK ADD CONSTRAINT FK_Consultas_Pacientes
        FOREIGN KEY (PacienteId) REFERENCES dbo.Pacientes (PacienteId);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Consultas_Citas')
    ALTER TABLE dbo.Consultas WITH CHECK ADD CONSTRAINT FK_Consultas_Citas
        FOREIGN KEY (CitaId) REFERENCES dbo.Citas (CitaId);

-- Cirugias -> Pacientes, Procedimientos
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Cirugias_Pacientes')
    ALTER TABLE dbo.Cirugias WITH CHECK ADD CONSTRAINT FK_Cirugias_Pacientes
        FOREIGN KEY (PacienteId) REFERENCES dbo.Pacientes (PacienteId);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Cirugias_Procedimientos')
    ALTER TABLE dbo.Cirugias WITH CHECK ADD CONSTRAINT FK_Cirugias_Procedimientos
        FOREIGN KEY (ProcedimientoId) REFERENCES dbo.Procedimientos (ProcedimientoId);

-- Preoperatorios -> Cirugias
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Preoperatorios_Cirugias')
    ALTER TABLE dbo.Preoperatorios WITH CHECK ADD CONSTRAINT FK_Preoperatorios_Cirugias
        FOREIGN KEY (CirugiaId) REFERENCES dbo.Cirugias (CirugiaId);

-- ControlesPostoperatorios -> Cirugias
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_ControlesPostoperatorios_Cirugias')
    ALTER TABLE dbo.ControlesPostoperatorios WITH CHECK ADD CONSTRAINT FK_ControlesPostoperatorios_Cirugias
        FOREIGN KEY (CirugiaId) REFERENCES dbo.Cirugias (CirugiaId);

-- Fotografias -> Cirugias
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Fotografias_Cirugias')
    ALTER TABLE dbo.Fotografias WITH CHECK ADD CONSTRAINT FK_Fotografias_Cirugias
        FOREIGN KEY (CirugiaId) REFERENCES dbo.Cirugias (CirugiaId);

-- Recetas -> Pacientes, Consultas, Cirugias, Usuarios
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Recetas_Pacientes')
    ALTER TABLE dbo.Recetas WITH CHECK ADD CONSTRAINT FK_Recetas_Pacientes
        FOREIGN KEY (PacienteId) REFERENCES dbo.Pacientes (PacienteId);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Recetas_Consultas')
    ALTER TABLE dbo.Recetas WITH CHECK ADD CONSTRAINT FK_Recetas_Consultas
        FOREIGN KEY (ConsultaId) REFERENCES dbo.Consultas (ConsultaId);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Recetas_Cirugias')
    ALTER TABLE dbo.Recetas WITH CHECK ADD CONSTRAINT FK_Recetas_Cirugias
        FOREIGN KEY (CirugiaId) REFERENCES dbo.Cirugias (CirugiaId);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Recetas_Usuarios')
    ALTER TABLE dbo.Recetas WITH CHECK ADD CONSTRAINT FK_Recetas_Usuarios
        FOREIGN KEY (UsuarioId) REFERENCES dbo.Usuarios (UsuarioId);

-- DetallesReceta -> Recetas
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_DetallesReceta_Recetas')
    ALTER TABLE dbo.DetallesReceta WITH CHECK ADD CONSTRAINT FK_DetallesReceta_Recetas
        FOREIGN KEY (RecetaId) REFERENCES dbo.Recetas (RecetaId);

-- Pagos -> Cirugias, Citas, Pacientes, Seguros
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Pagos_Cirugias')
    ALTER TABLE dbo.Pagos WITH CHECK ADD CONSTRAINT FK_Pagos_Cirugias
        FOREIGN KEY (CirugiaId) REFERENCES dbo.Cirugias (CirugiaId);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Pagos_Citas')
    ALTER TABLE dbo.Pagos WITH CHECK ADD CONSTRAINT FK_Pagos_Citas
        FOREIGN KEY (CitaId) REFERENCES dbo.Citas (CitaId);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Pagos_Pacientes')
    ALTER TABLE dbo.Pagos WITH CHECK ADD CONSTRAINT FK_Pagos_Pacientes
        FOREIGN KEY (PacienteId) REFERENCES dbo.Pacientes (PacienteId);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Pagos_Seguros')
    ALTER TABLE dbo.Pagos WITH CHECK ADD CONSTRAINT FK_Pagos_Seguros
        FOREIGN KEY (SeguroId) REFERENCES dbo.Seguros (SeguroId);

-- Check constraints de Pagos (validan reglas de negocio de PagosServices)
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = N'CK_Pagos_MontoSeguro')
    ALTER TABLE dbo.Pagos WITH CHECK ADD CONSTRAINT CK_Pagos_MontoSeguro
        CHECK (MontoSeguro >= 0 AND MontoSeguro <= Monto);

IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = N'CK_Pagos_Estado')
    ALTER TABLE dbo.Pagos WITH CHECK ADD CONSTRAINT CK_Pagos_Estado
        CHECK (Estado IN (N'Pagado', N'Pendiente'));

-- Gastos -> Usuarios
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Gastos_Usuarios')
    ALTER TABLE dbo.Gastos WITH CHECK ADD CONSTRAINT FK_Gastos_Usuarios
        FOREIGN KEY (UsuarioId) REFERENCES dbo.Usuarios (UsuarioId);

-- DocumentosClinicos -> Pacientes, Cirugias, Consultas, Usuarios
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_DocumentosClinicos_Pacientes')
    ALTER TABLE dbo.DocumentosClinicos WITH CHECK ADD CONSTRAINT FK_DocumentosClinicos_Pacientes
        FOREIGN KEY (PacienteId) REFERENCES dbo.Pacientes (PacienteId);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_DocumentosClinicos_Cirugias')
    ALTER TABLE dbo.DocumentosClinicos WITH CHECK ADD CONSTRAINT FK_DocumentosClinicos_Cirugias
        FOREIGN KEY (CirugiaId) REFERENCES dbo.Cirugias (CirugiaId);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_DocumentosClinicos_Consultas')
    ALTER TABLE dbo.DocumentosClinicos WITH CHECK ADD CONSTRAINT FK_DocumentosClinicos_Consultas
        FOREIGN KEY (ConsultaId) REFERENCES dbo.Consultas (ConsultaId);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_DocumentosClinicos_Usuarios')
    ALTER TABLE dbo.DocumentosClinicos WITH CHECK ADD CONSTRAINT FK_DocumentosClinicos_Usuarios
        FOREIGN KEY (UsuarioId) REFERENCES dbo.Usuarios (UsuarioId);

-- AuditoriaExpediente -> Pacientes, Usuarios
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_AuditoriaExpediente_Pacientes')
    ALTER TABLE dbo.AuditoriaExpediente WITH CHECK ADD CONSTRAINT FK_AuditoriaExpediente_Pacientes
        FOREIGN KEY (PacienteId) REFERENCES dbo.Pacientes (PacienteId);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_AuditoriaExpediente_Usuarios')
    ALTER TABLE dbo.AuditoriaExpediente WITH CHECK ADD CONSTRAINT FK_AuditoriaExpediente_Usuarios
        FOREIGN KEY (UsuarioId) REFERENCES dbo.Usuarios (UsuarioId);

-- Notificaciones -> Citas (SET NULL al eliminar cita)
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Notificaciones_Citas')
    ALTER TABLE dbo.Notificaciones WITH CHECK ADD CONSTRAINT FK_Notificaciones_Citas
        FOREIGN KEY (CitaId) REFERENCES dbo.Citas (CitaId) ON DELETE SET NULL;

-- Noticias -> Usuarios (SET NULL al eliminar usuario)
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Noticias_Usuarios')
    ALTER TABLE dbo.Noticias WITH CHECK ADD CONSTRAINT FK_Noticias_Usuarios
        FOREIGN KEY (CreadaPorUsuarioId) REFERENCES dbo.Usuarios (UsuarioId) ON DELETE SET NULL;
GO

/* ============================================================================
   SECCIÓN 4: ÍNDICES ÚNICOS Y DE RENDIMIENTO (IDEMPOTENTES)
   ============================================================================ */

-- Roles
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Roles') AND name = N'UX_Roles_Nombre')
    CREATE UNIQUE INDEX UX_Roles_Nombre ON dbo.Roles (Nombre);

-- Pacientes (cédula única solo cuando no es NULL)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Pacientes') AND name = N'UX_Pacientes_Cedula_NotNull')
    CREATE UNIQUE INDEX UX_Pacientes_Cedula_NotNull ON dbo.Pacientes (Cedula) WHERE Cedula IS NOT NULL;

-- Usuarios (email y nombre de usuario únicos cuando no son NULL)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Usuarios') AND name = N'UX_Usuarios_Email_NotNull')
    CREATE UNIQUE INDEX UX_Usuarios_Email_NotNull ON dbo.Usuarios (Email) WHERE Email IS NOT NULL;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Usuarios') AND name = N'UX_Usuarios_NombreUsuario_NotNull')
    CREATE UNIQUE INDEX UX_Usuarios_NombreUsuario_NotNull ON dbo.Usuarios (NombreUsuario) WHERE NombreUsuario IS NOT NULL;

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Usuarios') AND name = N'IX_Usuarios_RolId')
    CREATE INDEX IX_Usuarios_RolId ON dbo.Usuarios (RolId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Usuarios') AND name = N'IX_Usuarios_PacienteId')
    CREATE INDEX IX_Usuarios_PacienteId ON dbo.Usuarios (PacienteId);

-- PacienteSeguros
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.PacienteSeguros') AND name = N'IX_PacienteSeguros_PacienteId')
    CREATE INDEX IX_PacienteSeguros_PacienteId ON dbo.PacienteSeguros (PacienteId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.PacienteSeguros') AND name = N'IX_PacienteSeguros_SeguroId')
    CREATE INDEX IX_PacienteSeguros_SeguroId ON dbo.PacienteSeguros (SeguroId);

-- HistorialClinico (1:1 con Pacientes)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.HistorialClinico') AND name = N'UX_HistorialClinico_PacienteId')
    CREATE UNIQUE INDEX UX_HistorialClinico_PacienteId ON dbo.HistorialClinico (PacienteId);

-- Citas
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Citas') AND name = N'IX_Citas_Paciente_Fecha')
    CREATE INDEX IX_Citas_Paciente_Fecha ON dbo.Citas (PacienteId, Fecha);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Citas') AND name = N'IX_Citas_CitaAnteriorId')
    CREATE INDEX IX_Citas_CitaAnteriorId ON dbo.Citas (CitaAnteriorId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Citas') AND name = N'UX_Citas_CodigoPublico')
    CREATE UNIQUE INDEX UX_Citas_CodigoPublico ON dbo.Citas (CodigoPublico) WHERE CodigoPublico IS NOT NULL;

-- HistorialEstadosCita
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.HistorialEstadosCita') AND name = N'IX_HistorialEstadosCita_CitaId')
    CREATE INDEX IX_HistorialEstadosCita_CitaId ON dbo.HistorialEstadosCita (CitaId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.HistorialEstadosCita') AND name = N'IX_HistorialEstadosCita_UsuarioId')
    CREATE INDEX IX_HistorialEstadosCita_UsuarioId ON dbo.HistorialEstadosCita (UsuarioId);

-- Consultas
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Consultas') AND name = N'IX_Consultas_Paciente_Fecha')
    CREATE INDEX IX_Consultas_Paciente_Fecha ON dbo.Consultas (PacienteId, Fecha);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Consultas') AND name = N'IX_Consultas_CitaId')
    CREATE INDEX IX_Consultas_CitaId ON dbo.Consultas (CitaId);

-- Cirugias
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Cirugias') AND name = N'IX_Cirugias_Paciente_Fecha')
    CREATE INDEX IX_Cirugias_Paciente_Fecha ON dbo.Cirugias (PacienteId, FechaCirugia);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Cirugias') AND name = N'IX_Cirugias_ProcedimientoId')
    CREATE INDEX IX_Cirugias_ProcedimientoId ON dbo.Cirugias (ProcedimientoId);

-- Preoperatorios
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Preoperatorios') AND name = N'IX_Preoperatorios_CirugiaId')
    CREATE INDEX IX_Preoperatorios_CirugiaId ON dbo.Preoperatorios (CirugiaId);

-- ControlesPostoperatorios
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.ControlesPostoperatorios') AND name = N'IX_Controles_Cirugia_Fecha')
    CREATE INDEX IX_Controles_Cirugia_Fecha ON dbo.ControlesPostoperatorios (CirugiaId, Fecha);

-- Fotografias
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Fotografias') AND name = N'IX_Fotografias_Cirugia')
    CREATE INDEX IX_Fotografias_Cirugia ON dbo.Fotografias (CirugiaId);

-- Recetas
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Recetas') AND name = N'IX_Recetas_PacienteId')
    CREATE INDEX IX_Recetas_PacienteId ON dbo.Recetas (PacienteId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Recetas') AND name = N'IX_Recetas_ConsultaId')
    CREATE INDEX IX_Recetas_ConsultaId ON dbo.Recetas (ConsultaId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Recetas') AND name = N'IX_Recetas_CirugiaId')
    CREATE INDEX IX_Recetas_CirugiaId ON dbo.Recetas (CirugiaId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Recetas') AND name = N'IX_Recetas_UsuarioId')
    CREATE INDEX IX_Recetas_UsuarioId ON dbo.Recetas (UsuarioId);

-- DetallesReceta
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.DetallesReceta') AND name = N'IX_DetallesReceta_RecetaId')
    CREATE INDEX IX_DetallesReceta_RecetaId ON dbo.DetallesReceta (RecetaId);

-- Pagos
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Pagos') AND name = N'IX_Pagos_Fecha')
    CREATE INDEX IX_Pagos_Fecha ON dbo.Pagos (Fecha);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Pagos') AND name = N'IX_Pagos_PacienteId')
    CREATE INDEX IX_Pagos_PacienteId ON dbo.Pagos (PacienteId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Pagos') AND name = N'IX_Pagos_CirugiaId')
    CREATE INDEX IX_Pagos_CirugiaId ON dbo.Pagos (CirugiaId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Pagos') AND name = N'IX_Pagos_CitaId')
    CREATE INDEX IX_Pagos_CitaId ON dbo.Pagos (CitaId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Pagos') AND name = N'IX_Pagos_SeguroId')
    CREATE INDEX IX_Pagos_SeguroId ON dbo.Pagos (SeguroId);

-- Gastos
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Gastos') AND name = N'IX_Gastos_UsuarioId')
    CREATE INDEX IX_Gastos_UsuarioId ON dbo.Gastos (UsuarioId);

-- DocumentosClinicos
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.DocumentosClinicos') AND name = N'IX_DocumentosClinicos_Paciente_Fecha')
    CREATE INDEX IX_DocumentosClinicos_Paciente_Fecha ON dbo.DocumentosClinicos (PacienteId, FechaCarga DESC);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.DocumentosClinicos') AND name = N'IX_DocumentosClinicos_CirugiaId')
    CREATE INDEX IX_DocumentosClinicos_CirugiaId ON dbo.DocumentosClinicos (CirugiaId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.DocumentosClinicos') AND name = N'IX_DocumentosClinicos_ConsultaId')
    CREATE INDEX IX_DocumentosClinicos_ConsultaId ON dbo.DocumentosClinicos (ConsultaId);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.DocumentosClinicos') AND name = N'IX_DocumentosClinicos_UsuarioId')
    CREATE INDEX IX_DocumentosClinicos_UsuarioId ON dbo.DocumentosClinicos (UsuarioId);

-- AuditoriaExpediente
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.AuditoriaExpediente') AND name = N'IX_AuditoriaExpediente_Paciente_Fecha')
    CREATE INDEX IX_AuditoriaExpediente_Paciente_Fecha ON dbo.AuditoriaExpediente (PacienteId, Fecha DESC);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.AuditoriaExpediente') AND name = N'IX_AuditoriaExpediente_UsuarioId')
    CREATE INDEX IX_AuditoriaExpediente_UsuarioId ON dbo.AuditoriaExpediente (UsuarioId);

-- Notificaciones
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Notificaciones') AND name = N'IX_Notificaciones_Leida_FechaCreacion')
    CREATE INDEX IX_Notificaciones_Leida_FechaCreacion ON dbo.Notificaciones (Leida, FechaCreacion DESC);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Notificaciones') AND name = N'IX_Notificaciones_CitaId')
    CREATE INDEX IX_Notificaciones_CitaId ON dbo.Notificaciones (CitaId);

-- Noticias
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Noticias') AND name = N'IX_Noticias_Publicada_FechaPublicacion')
    CREATE INDEX IX_Noticias_Publicada_FechaPublicacion ON dbo.Noticias (Publicada, FechaPublicacion DESC);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.Noticias') AND name = N'IX_Noticias_CreadaPorUsuarioId')
    CREATE INDEX IX_Noticias_CreadaPorUsuarioId ON dbo.Noticias (CreadaPorUsuarioId);
GO

/* ============================================================================
   SECCIÓN 5: SEMBRADO INICIAL IDEMPOTENTE (ROLES Y USUARIOS DE PRUEBA)
   ============================================================================ */

-- 5.1 Los 4 roles oficiales del sistema
IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE Nombre = N'Doctora')
    INSERT INTO dbo.Roles (Nombre, Descripcion)
    VALUES (N'Doctora', N'Jefa / Dueña; acceso total al sistema');

IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE Nombre = N'AsistenteClinico')
    INSERT INTO dbo.Roles (Nombre, Descripcion)
    VALUES (N'AsistenteClinico', N'Acceso clínico a expedientes, cirugías y recetas');

IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE Nombre = N'Secretaria')
    INSERT INTO dbo.Roles (Nombre, Descripcion)
    VALUES (N'Secretaria', N'Recepción, agenda, pacientes y cobros');

IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE Nombre = N'Paciente')
    INSERT INTO dbo.Roles (Nombre, Descripcion)
    VALUES (N'Paciente', N'Acceso al perfil y citas propias');
GO

-- 5.2 Paciente de demostración (vinculado a paciente.demo)
IF NOT EXISTS (SELECT 1 FROM dbo.Pacientes WHERE Cedula = N'00000000001')
BEGIN
    INSERT INTO dbo.Pacientes
        (FechaRegistro, Nombre, Apellido, Cedula, FechaNacimiento, TieneSeguro, Sexo, Telefono, Email, Direccion)
    VALUES
        (GETDATE(), N'Paciente', N'Demo', N'00000000001', CONVERT(date, '1994-05-15', 23), 1, N'Femenino',
         N'8090000001', N'paciente.demo@tangerclinic.com', N'Av. Abraham Lincoln, Santo Domingo');
END;
GO

-- 5.3 Usuarios de demostración con hashes reales compatibles con PasswordHasher<Usuario>
--     Credenciales:
--       doctor.demo     / DoctorTest!2026     (Rol: Doctora)
--       asistente.demo  / AsistenteTest!2026  (Rol: AsistenteClinico)
--       secretaria.demo / SecretariaTest!2026 (Rol: Secretaria)
--       paciente.demo   / PacienteTest!2026   (Rol: Paciente; también ingresa con 00000000001 o 8090000001)
DECLARE @RolDoctoraId    int = (SELECT TOP (1) RolId FROM dbo.Roles WHERE Nombre = N'Doctora');
DECLARE @RolAsistenteId  int = (SELECT TOP (1) RolId FROM dbo.Roles WHERE Nombre = N'AsistenteClinico');
DECLARE @RolSecretariaId int = (SELECT TOP (1) RolId FROM dbo.Roles WHERE Nombre = N'Secretaria');
DECLARE @RolPacienteId   int = (SELECT TOP (1) RolId FROM dbo.Roles WHERE Nombre = N'Paciente');
DECLARE @PacienteDemoId  int = (SELECT TOP (1) PacienteId FROM dbo.Pacientes WHERE Cedula = N'00000000001');

IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE NombreUsuario = N'doctor.demo')
    INSERT INTO dbo.Usuarios (FechaRegistro, Nombre, Apellido, Email, NombreUsuario, PacienteId, PasswordHash, RolId, Activo)
    VALUES (GETDATE(), N'Doctora', N'Demo', NULL, N'doctor.demo', NULL,
            N'AQAAAAEAAYagAAAAEH8ryt/hvDWmOG0PLEJVOMYc0jKeTEjS8wT2wKxQJauNnl6wUdwIW6z7DI1XgBgdJQ==',
            @RolDoctoraId, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE NombreUsuario = N'asistente.demo')
    INSERT INTO dbo.Usuarios (FechaRegistro, Nombre, Apellido, Email, NombreUsuario, PacienteId, PasswordHash, RolId, Activo)
    VALUES (GETDATE(), N'Asistente', N'Demo', NULL, N'asistente.demo', NULL,
            N'AQAAAAIAAYagAAAAEEDvb5HFjfb2RElH9GyD0SjnTJRw95YGOaiMCpQWTaWwiXCxdb+dxcayXHP/m5vqdA==',
            @RolAsistenteId, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE NombreUsuario = N'secretaria.demo')
    INSERT INTO dbo.Usuarios (FechaRegistro, Nombre, Apellido, Email, NombreUsuario, PacienteId, PasswordHash, RolId, Activo)
    VALUES (GETDATE(), N'Secretaria', N'Demo', NULL, N'secretaria.demo', NULL,
            N'AQAAAAEAAYagAAAAEM3WNoKBln0sPpISOznteHitMxa4HPKGzMbuX8Ru/dtjRJSlANjMCm+iUgOnH0iJAw==',
            @RolSecretariaId, 1);

IF NOT EXISTS (SELECT 1 FROM dbo.Usuarios WHERE NombreUsuario = N'paciente.demo')
    INSERT INTO dbo.Usuarios (FechaRegistro, Nombre, Apellido, Email, NombreUsuario, PacienteId, PasswordHash, RolId, Activo)
    VALUES (GETDATE(), N'Paciente', N'Demo', NULL, N'paciente.demo', @PacienteDemoId,
            N'AQAAAAEAAYagAAAAEI/ciALrrPI22jfj+gknvsJSqQcChgOYuD3n2s01PEJWsWVcAECVbdDGZLSX4pZQ0Q==',
            @RolPacienteId, 1);

-- Garantiza que paciente.demo quede vinculado a su ficha si ya existía sin PacienteId
UPDATE dbo.Usuarios
SET PacienteId = @PacienteDemoId
WHERE NombreUsuario = N'paciente.demo'
  AND PacienteId IS NULL
  AND @PacienteDemoId IS NOT NULL;
GO

/* ============================================================================
   SECCIÓN 6: DATOS BÁSICOS DE DEMOSTRACIÓN (IDEMPOTENTES)
   - Aseguradoras (ARS Humano, ARS SeNaSa, ARS Universal, ARS Mapfre Salud)
   - Póliza e historial clínico inicial del paciente demo
   - Catálogo de procedimientos estéticos base
   - 2 Noticias publicadas visibles desde el primer arranque (web y móvil)
   - Cita y notificación inicial de bienvenida
   ============================================================================ */

-- 6.1 Aseguradoras (dbo.Seguros)
IF NOT EXISTS (SELECT 1 FROM dbo.Seguros WHERE Nombre = N'ARS Humano')
    INSERT INTO dbo.Seguros (FechaRegistro, Nombre, Telefono, Email, Direccion, Activo)
    VALUES (GETDATE(), N'ARS Humano', N'809-476-3535', N'servicio@arshumano.com.do',
            N'Av. Lope de Vega No. 31, Santo Domingo', 1);

IF NOT EXISTS (SELECT 1 FROM dbo.Seguros WHERE Nombre = N'ARS SeNaSa')
    INSERT INTO dbo.Seguros (FechaRegistro, Nombre, Telefono, Email, Direccion, Activo)
    VALUES (GETDATE(), N'ARS SeNaSa', N'809-333-3824', N'contacto@arssenasa.gob.do',
            N'Av. 27 de Febrero No. 232, Santo Domingo', 1);

IF NOT EXISTS (SELECT 1 FROM dbo.Seguros WHERE Nombre = N'ARS Universal')
    INSERT INTO dbo.Seguros (FechaRegistro, Nombre, Telefono, Email, Direccion, Activo)
    VALUES (GETDATE(), N'ARS Universal', N'809-544-7111', N'atencion@universal.com.do',
            N'Av. Winston Churchill No. 1100, Santo Domingo', 1);

IF NOT EXISTS (SELECT 1 FROM dbo.Seguros WHERE Nombre = N'ARS Mapfre Salud')
    INSERT INTO dbo.Seguros (FechaRegistro, Nombre, Telefono, Email, Direccion, Activo)
    VALUES (GETDATE(), N'ARS Mapfre Salud', N'809-508-5000', N'salud@mapfresaludars.com.do',
            N'Av. Abraham Lincoln No. 952, Santo Domingo', 1);
GO

-- 6.2 Póliza e historial clínico inicial para Paciente Demo
DECLARE @PacienteDemoId int = (SELECT TOP (1) PacienteId FROM dbo.Pacientes WHERE Cedula = N'00000000001');
DECLARE @ArsHumanoId    int = (SELECT TOP (1) SeguroId FROM dbo.Seguros WHERE Nombre = N'ARS Humano');

IF @PacienteDemoId IS NOT NULL AND @ArsHumanoId IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM dbo.PacienteSeguros WHERE PacienteId = @PacienteDemoId AND SeguroId = @ArsHumanoId)
BEGIN
    INSERT INTO dbo.PacienteSeguros
        (FechaRegistro, PacienteId, SeguroId, NumeroPoliza, Titular, FechaVencimiento, Observaciones)
    VALUES
        (GETDATE(), @PacienteDemoId, @ArsHumanoId, N'HUM-2026-0001', N'Paciente Demo',
         DATEADD(year, 1, CAST(GETDATE() AS date)), N'Póliza activa de demostración.');
END;

IF @PacienteDemoId IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM dbo.HistorialClinico WHERE PacienteId = @PacienteDemoId)
BEGIN
    INSERT INTO dbo.HistorialClinico
        (FechaActualizacion, PacienteId, Antecedentes, AntecedentesPersonales, AntecedentesFamiliares, Alergias, Medicamentos, NotasMedicas)
    VALUES
        (GETDATE(), @PacienteDemoId,
         N'Sin cirugías previas de alto riesgo.',
         N'No fumadora. Actividad física moderada.',
         N'Sin antecedentes cardiovasculares relevantes.',
         N'Ninguna alergia medicamentosa conocida.',
         N'Ninguno actualmente.',
         N'Expediente inicial de demostración listo para evaluación estética.');
END;
GO

-- 6.3 Procedimientos estéticos base (dbo.Procedimientos)
IF NOT EXISTS (SELECT 1 FROM dbo.Procedimientos WHERE Nombre = N'Rinoplastia Estética y Funcional')
    INSERT INTO dbo.Procedimientos (FechaRegistro, Nombre, Descripcion, PrecioBase)
    VALUES (GETDATE(), N'Rinoplastia Estética y Funcional',
            N'Remodelación armónica de la estructura nasal con preservación y mejora de la función respiratoria.',
            165000.00);

IF NOT EXISTS (SELECT 1 FROM dbo.Procedimientos WHERE Nombre = N'Mamoplastia de Aumento')
    INSERT INTO dbo.Procedimientos (FechaRegistro, Nombre, Descripcion, PrecioBase)
    VALUES (GETDATE(), N'Mamoplastia de Aumento',
            N'Aumento y armonización mamaria con implantes cohesivos de última generación y planificación 3D.',
            210000.00);

IF NOT EXISTS (SELECT 1 FROM dbo.Procedimientos WHERE Nombre = N'Abdominoplastia Completa')
    INSERT INTO dbo.Procedimientos (FechaRegistro, Nombre, Descripcion, PrecioBase)
    VALUES (GETDATE(), N'Abdominoplastia Completa',
            N'Reconstrucción de la pared abdominal, plicatura muscular y retiro de exceso cutáneo.',
            245000.00);

IF NOT EXISTS (SELECT 1 FROM dbo.Procedimientos WHERE Nombre = N'Lipoescultura 360')
    INSERT INTO dbo.Procedimientos (FechaRegistro, Nombre, Descripcion, PrecioBase)
    VALUES (GETDATE(), N'Lipoescultura 360',
            N'Contorneado corporal circunferencial de alta definición asistido por tecnología ultrasónica.',
            225000.00);

IF NOT EXISTS (SELECT 1 FROM dbo.Procedimientos WHERE Nombre = N'Blefaroplastia (Cirugía de Párpados)')
    INSERT INTO dbo.Procedimientos (FechaRegistro, Nombre, Descripcion, PrecioBase)
    VALUES (GETDATE(), N'Blefaroplastia (Cirugía de Párpados)',
            N'Rejuvenecimiento de párpados superiores e inferiores mediante corrección de piel redundante y bolsas.',
            115000.00);

IF NOT EXISTS (SELECT 1 FROM dbo.Procedimientos WHERE Nombre = N'Lifting Facial (Ritidoplastia)')
    INSERT INTO dbo.Procedimientos (FechaRegistro, Nombre, Descripcion, PrecioBase)
    VALUES (GETDATE(), N'Lifting Facial (Ritidoplastia)',
            N'Reposicionamiento profundo de tejidos faciales y cervicales para resultados naturales y duraderos.',
            280000.00);
GO

-- 6.4 Dos noticias publicadas visibles desde el primer arranque (web y móvil)
DECLARE @DoctoraUsuarioId int = (SELECT TOP (1) UsuarioId FROM dbo.Usuarios WHERE NombreUsuario = N'doctor.demo');

IF NOT EXISTS (SELECT 1 FROM dbo.Noticias WHERE Titulo = N'Bienvenidos a la nueva plataforma digital de Tanger Clinic')
BEGIN
    INSERT INTO dbo.Noticias (Titulo, Resumen, Contenido, FechaPublicacion, Publicada, CreadaPorUsuarioId)
    VALUES
    (
        N'Bienvenidos a la nueva plataforma digital de Tanger Clinic',
        N'Ya puedes agendar tus citas en línea, consultar el estado de tu seguro médico y dar seguimiento a tus recetas desde la web o la app móvil.',
        N'En Tanger Clinic damos un paso adelante en la experiencia de nuestros pacientes. A través de nuestra plataforma digital puedes solicitar evaluaciones estéticas, verificar tus próximas citas, revisar tus recetas médicas y mantener una comunicación ágil con nuestro equipo clínico. Nuestro compromiso es brindarte seguridad, acompañamiento personalizado y resultados armónicos en cada etapa de tu proceso.',
        DATEADD(day, -2, GETDATE()),
        1,
        @DoctoraUsuarioId
    );
END;

IF NOT EXISTS (SELECT 1 FROM dbo.Noticias WHERE Titulo = N'Guía de preparación preoperatoria para una cirugía estética segura')
BEGIN
    INSERT INTO dbo.Noticias (Titulo, Resumen, Contenido, FechaPublicacion, Publicada, CreadaPorUsuarioId)
    VALUES
    (
        N'Guía de preparación preoperatoria para una cirugía estética segura',
        N'Conoce las evaluaciones médicas, analíticas y recomendaciones clave antes de tu procedimiento quirúrgico.',
        N'La seguridad del paciente es la prioridad en cada intervención de cirugía plástica. Antes de la fecha programada realizamos una evaluación preoperatoria integral que incluye analíticas de laboratorio, evaluación cardiológica y valoración anestésica. Te recomendamos mantener una adecuada hidratación, evitar el consumo de tabaco al menos cuatro semanas antes y notificar a la doctora cualquier medicamento o suplemento que estés tomando.',
        DATEADD(day, -1, GETDATE()),
        1,
        @DoctoraUsuarioId
    );
END;
GO

-- 6.5 Cita demostrativa próxima y notificación inicial
DECLARE @PacienteDemoId int = (SELECT TOP (1) PacienteId FROM dbo.Pacientes WHERE Cedula = N'00000000001');
DECLARE @CitaDemoId     int;

IF @PacienteDemoId IS NOT NULL
BEGIN
    SELECT @CitaDemoId = CitaId
    FROM dbo.Citas
    WHERE CodigoPublico = N'TC-DEMO-0001';

    IF @CitaDemoId IS NULL
    BEGIN
        INSERT INTO dbo.Citas
            (Fecha, PacienteId, TipoCita, Costo, Estado, EstadoPago, CodigoPublico, Observaciones)
        VALUES
            (DATEADD(hour, 10, DATEADD(day, 7, CAST(CAST(GETDATE() AS date) AS datetime))),
             @PacienteDemoId,
             N'Evaluación Estética Inicial',
             3500.00,
             N'Confirmada',
             N'Pendiente',
             N'TC-DEMO-0001',
             N'Cita inicial de demostración generada automáticamente.');

        SET @CitaDemoId = CONVERT(int, SCOPE_IDENTITY());
    END;

    IF NOT EXISTS (SELECT 1 FROM dbo.Notificaciones WHERE Tipo = N'Sistema' AND Titulo = N'Sistema inicializado correctamente')
    BEGIN
        INSERT INTO dbo.Notificaciones (Tipo, Titulo, Mensaje, CitaId, FechaCreacion, Leida)
        VALUES
        (
            N'Sistema',
            N'Sistema inicializado correctamente',
            N'La base de datos de Tanger Clinic se cargó con los roles, usuarios de prueba, aseguradoras, procedimientos y noticias iniciales.',
            @CitaDemoId,
            GETDATE(),
            0
        );
    END;
END;
GO

/* ============================================================================
   RESUMEN DE CREDENCIALES DEMO INCLUIDAS:
     doctor.demo     / DoctorTest!2026     -> Rol: Doctora
     asistente.demo  / AsistenteTest!2026  -> Rol: AsistenteClinico
     secretaria.demo / SecretariaTest!2026 -> Rol: Secretaria
     paciente.demo   / PacienteTest!2026   -> Rol: Paciente (Cédula: 00000000001 | Tel: 8090000001)
   ============================================================================ */
