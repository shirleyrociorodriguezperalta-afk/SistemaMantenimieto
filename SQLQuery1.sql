-- 1. Tabla TIPO_EQUIPO
CREATE TABLE Tipo_Equipo (
    IdTipoEquipo INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(255) NULL
);

-- 2. Tabla EQUIPO
CREATE TABLE Equipo (
    IdEquipo INT IDENTITY(1,1) PRIMARY KEY,
    CodigoActivo VARCHAR(50) NOT NULL UNIQUE,
    Nombre VARCHAR(100) NOT NULL,
    IdTipoEquipo INT NOT NULL,
    Ubicacion VARCHAR(150) NOT NULL,
    EstadoOperativo VARCHAR(50) DEFAULT 'Operativo' NOT NULL,
    CONSTRAINT FK_Equipo_TipoEquipo FOREIGN KEY (IdTipoEquipo) REFERENCES Tipo_Equipo(IdTipoEquipo)
);

-- 3. Tabla USUARIO (Autenticación y Roles)
CREATE TABLE Usuario (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario VARCHAR(50) NOT NULL UNIQUE,
    Clave VARCHAR(255) NOT NULL,
    NombreCompleto VARCHAR(100) NOT NULL,
    Rol VARCHAR(30) NOT NULL, -- 'Administrador' o 'Tecnico'
    Activo INT DEFAULT 1 NOT NULL
);

-- 4. Tabla TECNICO
CREATE TABLE Tecnico (
    IdTecnico INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Especialidad VARCHAR(100) NOT NULL,
    Activo INT DEFAULT 1 NOT NULL
);

-- 5. Tabla PLAN_MANTENIMIENTO
CREATE TABLE PlanMantenimiento (
    IdPlan INT IDENTITY(1,1) PRIMARY KEY,
    IdEquipo INT NOT NULL,
    DescripcionTarea VARCHAR(255) NOT NULL,
    FrecuenciaDias INT NOT NULL,
    Activo INT DEFAULT 1 NOT NULL,
    CONSTRAINT FK_Plan_Equipo FOREIGN KEY (IdEquipo) REFERENCES Equipo(IdEquipo)
);

-- 6. Tabla MANTENIMIENTO
CREATE TABLE Mantenimiento (
    IdMantenimiento INT IDENTITY(1,1) PRIMARY KEY,
    IdPlan INT NOT NULL,
    IdTecnico INT NULL,
    FechaProgramada DATE NOT NULL,
    FechaRealizada DATE NULL,
    Estado VARCHAR(30) DEFAULT 'Vigente' NOT NULL,
    Observaciones VARCHAR(500) NULL,
    CONSTRAINT FK_Mantenimiento_Plan FOREIGN KEY (IdPlan) REFERENCES PlanMantenimiento(IdPlan),
    CONSTRAINT FK_Mantenimiento_Tecnico FOREIGN KEY (IdTecnico) REFERENCES Tecnico(IdTecnico)
);
GO

-- =========================================================
-- DATOS INICIALES DE PRUEBA (SEMILLA)
-- =========================================================

-- Tipos de Equipo base
INSERT INTO Tipo_Equipo (Nombre, Descripcion) VALUES 
('Servidor', 'Equipos de cómputo de alto rendimiento para red'),
('Computadora de Escritorio', 'Equipos para laboratorios y personal administrativo'),
('Impresora Industrial', 'Impresoras y multifuncionales de área');

-- Usuario Administrador por defecto
INSERT INTO Usuario (NombreUsuario, Clave, NombreCompleto, Rol, Activo) VALUES 
('admin', 'admin123', 'Administrador del Sistema', 'Administrador', 1),
('tecnico', 'tecnico123', 'Técnico de Soporte', 'Tecnico', 1);

-- Técnico inicial
INSERT INTO Tecnico (Nombre, Especialidad, Activo) VALUES 
('Juan Pérez', 'Mantenimiento Preventivo y Redes', 1);
GO