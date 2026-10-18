/* =====================================================
   GymControl - Script de creación de base de datos
   Curso: ISB-13 Programación II
   Motor: Microsoft SQL Server
   ===================================================== */

IF DB_ID('GymControl') IS NULL
    CREATE DATABASE GymControl;
GO

USE GymControl;
GO

/* ---------- Limpieza (permite re-ejecutar el script) ---------- */
IF OBJECT_ID('dbo.Asistencia')    IS NOT NULL DROP TABLE dbo.Asistencia;
IF OBJECT_ID('dbo.Pago')          IS NOT NULL DROP TABLE dbo.Pago;
IF OBJECT_ID('dbo.Membresia')     IS NOT NULL DROP TABLE dbo.Membresia;
IF OBJECT_ID('dbo.Entrenador')    IS NOT NULL DROP TABLE dbo.Entrenador;
IF OBJECT_ID('dbo.Miembro')       IS NOT NULL DROP TABLE dbo.Miembro;
IF OBJECT_ID('dbo.TipoMembresia') IS NOT NULL DROP TABLE dbo.TipoMembresia;
GO

/* ---------- Tablas ---------- */

CREATE TABLE dbo.TipoMembresia (
    IdTipoMembresia INT IDENTITY(1,1) CONSTRAINT PK_TipoMembresia PRIMARY KEY,
    Nombre          VARCHAR(50)   NOT NULL CONSTRAINT UQ_TipoMembresia_Nombre UNIQUE,
    DuracionDias    INT           NOT NULL CONSTRAINT CK_TipoMembresia_Duracion CHECK (DuracionDias > 0),
    Precio          DECIMAL(10,2) NOT NULL CONSTRAINT CK_TipoMembresia_Precio CHECK (Precio > 0)
);

CREATE TABLE dbo.Miembro (
    IdMiembro       INT IDENTITY(1,1) CONSTRAINT PK_Miembro PRIMARY KEY,
    Cedula          VARCHAR(20)  NOT NULL CONSTRAINT UQ_Miembro_Cedula UNIQUE,
    Nombre          VARCHAR(50)  NOT NULL,
    Apellidos       VARCHAR(80)  NOT NULL,
    FechaNacimiento DATE         NOT NULL,
    Telefono        VARCHAR(15)  NOT NULL,
    Correo          VARCHAR(100) NULL,
    FechaRegistro   DATETIME     NOT NULL CONSTRAINT DF_Miembro_FechaRegistro DEFAULT (GETDATE()),
    Activo          BIT          NOT NULL CONSTRAINT DF_Miembro_Activo DEFAULT (1)
);

CREATE TABLE dbo.Entrenador (
    IdEntrenador INT IDENTITY(1,1) CONSTRAINT PK_Entrenador PRIMARY KEY,
    Cedula       VARCHAR(20) NOT NULL CONSTRAINT UQ_Entrenador_Cedula UNIQUE,
    Nombre       VARCHAR(50) NOT NULL,
    Apellidos    VARCHAR(80) NOT NULL,
    Telefono     VARCHAR(15) NOT NULL,
    Especialidad VARCHAR(60) NOT NULL
);

CREATE TABLE dbo.Membresia (
    IdMembresia      INT IDENTITY(1,1) CONSTRAINT PK_Membresia PRIMARY KEY,
    IdMiembro        INT           NOT NULL CONSTRAINT FK_Membresia_Miembro
                         FOREIGN KEY REFERENCES dbo.Miembro(IdMiembro),
    IdTipoMembresia  INT           NOT NULL CONSTRAINT FK_Membresia_Tipo
                         FOREIGN KEY REFERENCES dbo.TipoMembresia(IdTipoMembresia),
    IdEntrenador     INT           NULL CONSTRAINT FK_Membresia_Entrenador
                         FOREIGN KEY REFERENCES dbo.Entrenador(IdEntrenador),
    FechaInicio      DATE          NOT NULL,
    FechaVencimiento DATE          NOT NULL,
    PrecioAcordado   DECIMAL(10,2) NOT NULL CONSTRAINT CK_Membresia_Precio CHECK (PrecioAcordado > 0),
    CONSTRAINT CK_Membresia_Fechas CHECK (FechaVencimiento >= FechaInicio)
);

CREATE TABLE dbo.Pago (
    IdPago      INT IDENTITY(1,1) CONSTRAINT PK_Pago PRIMARY KEY,
    IdMembresia INT           NOT NULL CONSTRAINT FK_Pago_Membresia
                    FOREIGN KEY REFERENCES dbo.Membresia(IdMembresia),
    FechaPago   DATETIME      NOT NULL CONSTRAINT DF_Pago_Fecha DEFAULT (GETDATE()),
    Monto       DECIMAL(10,2) NOT NULL CONSTRAINT CK_Pago_Monto CHECK (Monto > 0),
    MetodoPago  VARCHAR(20)   NOT NULL CONSTRAINT CK_Pago_Metodo
                    CHECK (MetodoPago IN ('Efectivo','Tarjeta','SINPE'))
);

CREATE TABLE dbo.Asistencia (
    IdAsistencia       INT IDENTITY(1,1) CONSTRAINT PK_Asistencia PRIMARY KEY,
    IdMiembro          INT      NOT NULL CONSTRAINT FK_Asistencia_Miembro
                           FOREIGN KEY REFERENCES dbo.Miembro(IdMiembro),
    FechaHoraEntrada   DATETIME NOT NULL CONSTRAINT DF_Asistencia_Fecha DEFAULT (GETDATE())
);
GO

/* ---------- Datos de prueba ---------- */

INSERT INTO dbo.TipoMembresia (Nombre, DuracionDias, Precio) VALUES
('Mensual',    30,  25000),
('Trimestral', 90,  65000),
('Semestral',  180, 120000),
('Anual',      365, 210000);

INSERT INTO dbo.Miembro (Cedula, Nombre, Apellidos, FechaNacimiento, Telefono, Correo) VALUES
('1-1234-0567', 'Andrea',  'Mora Solís',     '1995-03-14', '8888-1111', 'andrea.mora@correo.com'),
('2-0456-0789', 'Carlos',  'Jiménez Rojas',  '1988-11-02', '8777-2222', 'carlos.jimenez@correo.com'),
('1-0987-0321', 'Daniela', 'Vargas Castro',  '2001-07-25', '8666-3333', NULL),
('3-0222-0444', 'Luis',    'Quesada Araya',  '1979-01-30', '8555-4444', 'luis.quesada@correo.com'),
('1-1500-0900', 'María',   'Fallas Núñez',   '1999-09-09', '8444-5555', 'maria.fallas@correo.com');

INSERT INTO dbo.Entrenador (Cedula, Nombre, Apellidos, Telefono, Especialidad) VALUES
('1-0800-0100', 'Jorge',  'Salazar Pérez',  '8333-6666', 'Musculación'),
('4-0150-0200', 'Karla',  'Brenes Ugalde',  '8222-7777', 'Crossfit'),
('1-0900-0300', 'Esteban','Rodríguez León', '8111-8888', 'Cardio y resistencia');

/* Fechas relativas a hoy, para que haya membresías activas, por vencer y vencidas */
DECLARE @hoy DATE = CAST(GETDATE() AS DATE);

INSERT INTO dbo.Membresia (IdMiembro, IdTipoMembresia, IdEntrenador, FechaInicio, FechaVencimiento, PrecioAcordado) VALUES
(1, 1, 1,    DATEADD(DAY, -10,  @hoy), DATEADD(DAY, 20,  @hoy), 25000),   -- Activa
(2, 2, NULL, DATEADD(DAY, -80,  @hoy), DATEADD(DAY, 10,  @hoy), 65000),   -- Activa
(3, 1, 2,    DATEADD(DAY, -45,  @hoy), DATEADD(DAY, -15, @hoy), 25000),   -- Vencida
(4, 4, 3,    DATEADD(DAY, -100, @hoy), DATEADD(DAY, 265, @hoy), 210000),  -- Activa, saldo pendiente
(5, 1, NULL, DATEADD(DAY, -25,  @hoy), DATEADD(DAY, 5,   @hoy), 25000);   -- Por vencer, saldo pendiente

INSERT INTO dbo.Pago (IdMembresia, FechaPago, Monto, MetodoPago) VALUES
(1, DATEADD(DAY, -10,  GETDATE()), 25000,  'Efectivo'),
(2, DATEADD(DAY, -80,  GETDATE()), 40000,  'SINPE'),
(3, DATEADD(DAY, -45,  GETDATE()), 25000,  'Tarjeta'),
(4, DATEADD(DAY, -100, GETDATE()), 100000, 'Tarjeta'),
(4, DATEADD(DAY, -40,  GETDATE()), 50000,  'SINPE'),
(5, DATEADD(DAY, -25,  GETDATE()), 10000,  'Efectivo');

INSERT INTO dbo.Asistencia (IdMiembro, FechaHoraEntrada) VALUES
(1, DATEADD(HOUR, -5,   GETDATE())),
(1, DATEADD(DAY,  -2,   GETDATE())),
(2, DATEADD(DAY,  -1,   GETDATE())),
(4, DATEADD(HOUR, -3,   GETDATE())),
(4, DATEADD(DAY,  -3,   GETDATE())),
(5, DATEADD(DAY,  -1,   GETDATE())),
(3, DATEADD(DAY,  -20,  GETDATE()));
GO

/* ---------- Verificación rápida ---------- */
SELECT 'TipoMembresia' AS Tabla, COUNT(*) AS Registros FROM dbo.TipoMembresia UNION ALL
SELECT 'Miembro',      COUNT(*) FROM dbo.Miembro      UNION ALL
SELECT 'Entrenador',   COUNT(*) FROM dbo.Entrenador   UNION ALL
SELECT 'Membresia',    COUNT(*) FROM dbo.Membresia    UNION ALL
SELECT 'Pago',         COUNT(*) FROM dbo.Pago         UNION ALL
SELECT 'Asistencia',   COUNT(*) FROM dbo.Asistencia;
GO