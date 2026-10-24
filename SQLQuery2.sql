USE SistemaMantenimientoDB;
GO

-- Limpiar datos de prueba anteriores respetando llaves foráneas
DELETE FROM Mantenimiento;
DELETE FROM Plan_Mantenimiento;
DELETE FROM Tecnico;
DELETE FROM Usuario;
DELETE FROM Equipo;
DELETE FROM Tipo_Equipo;

DBCC CHECKIDENT ('Mantenimiento', RESEED, 0);
DBCC CHECKIDENT ('Plan_Mantenimiento', RESEED, 0);
DBCC CHECKIDENT ('Tecnico', RESEED, 0);
DBCC CHECKIDENT ('Usuario', RESEED, 0);
DBCC CHECKIDENT ('Equipo', RESEED, 0);
DBCC CHECKIDENT ('Tipo_Equipo', RESEED, 0);
GO

-- 1. TIPOS DE EQUIPO DIVERSIFICADOS
INSERT INTO Tipo_Equipo (Nombre, Descripcion) VALUES
('Servidores y Datacenter', 'Servidores Blade, Datacenters y Switches de Core para infraestructura central'),
('Cómputo de Alto Rendimiento', 'Estaciones de trabajo para diseño, simulación 3D y renderizado'),
('Instrumental Científico y Médico', 'Microscopios electrónicos, espectrómetros y centrífugas'),
('Fabricación Digital y Prototipado', 'Impresoras 3D industriales, cortadoras láser y ruteadoras CNC'),
('Equipo Audiovisual e Interactivo', 'Proyectores de red, pantallas interactivas y sistemas de audio para Aulas Magna'),
('Redes y Telecomunicaciones', 'Routers Cisco, Access Points empresariales y racks de telecomunicaciones'),
('Impresión y Digitalización Masiva', 'Fotocopiadoras industriales y escáneres de alta velocidad para oficinas');

-- 2. EQUIPOS DISTRIBUIDOS EN EDIFICIOS, LABORATORIOS Y OFICINAS
INSERT INTO Equipo (CodigoActivo, Nombre, IdTipoEquipo, Ubicacion, EstadoOperativo, FechaIngreso) VALUES
-- Datacenter y Redes
('EQ-UNI-001', 'Servidor Dell PowerEdge R750 (Blade 01)', 1, 'Datacenter Central - Edificio de Tecnologías (Piso 2)', 'Operativo', '2022-01-15'),
('EQ-UNI-002', 'Switch Core Cisco Catalyst 9500', 6, 'Rack Principal - Edificio de Administración (Piso 1)', 'Operativo', '2021-11-10'),

-- Laboratorios de Cómputo y Diseño
('EQ-UNI-003', 'Workstation HP Z4 G4 (i9/64GB/RTX)', 2, 'Lab. Cómputo 301 - Edificio de Ingeniería', 'Operativo', '2023-02-01'),
('EQ-UNI-004', 'Workstation Mac Studio M2 Max', 2, 'Lab. Edición Digital - Edificio de Arquitectura y Diseño', 'Operativo', '2023-08-20'),

-- Instrumental Científico y Médico
('EQ-UNI-005', 'Microscopio Electrónico Zeiss Evo 10', 3, 'Lab. Microscopía - Edificio de Ciencias Biomédicas', 'En Mantenimiento', '2020-05-12'),
('EQ-UNI-006', 'Espectrómetro FT-IR Thermo Scientific', 3, 'Lab. Química Orgánica - Edificio de Química y Farmacia', 'Operativo', '2021-03-30'),

-- Prototipado y Robótica
('EQ-UNI-007', 'Impresora 3D Formlabs Form 3L (SLA)', 4, 'Lab. Mecatrónica y Prototipado - Edificio de Innovación', 'Operativo', '2022-09-14'),
('EQ-UNI-008', 'Cortadora Láser CNC Epilog Fusion', 4, 'Taller de Prototipos - Edificio de Ingeniería Mecánica', 'Operativo', '2021-07-22'),

-- Audiovisual en Aulas y Auditorios
('EQ-UNI-009', 'Proyector Laser Epson EB-PU2213B (13k Lum)', 5, 'Auditorio Principal - Edificio de Rectoría', 'Operativo', '2023-01-10'),
('EQ-UNI-010', 'Pantalla Interactiva SMART Board 85"', 5, 'Aula Magna 102 - Edificio de Derecho y Humanidades', 'Operativo', '2022-04-18'),

-- Oficinas Administrativas
('EQ-UNI-011', 'Multifuncional Ricoh Aficio MP 6055', 7, 'Oficina de Admisiones y Registro - Edificio Central', 'Operativo', '2021-02-05'),
('EQ-UNI-012', 'Escáner Fujitsu fi-7700 de Alta Velocidad', 7, 'Oficina de Archivo Central - Edificio de Finanzas', 'Operativo', '2022-10-01');

-- 3. USUARIOS DEL SISTEMA (ADMINISTRADORES Y TÉCNICOS POR ÁREA)
-- Clave por defecto para pruebas: admin123 para admin, tecnico123 para el resto
INSERT INTO Usuario (NombreUsuario, Clave, NombreCompleto, Rol, Activo) VALUES
('admin', 'admin123', 'Dra. Elena Rostova (Directora de Servicios Generales)', 'Administrador', 1),
('jgarcia', 'tecnico123', 'Ing. Jorge García (Especialista en Redes y Servidores)', 'Tecnico', 1),
('mlopez', 'tecnico123', 'Lic. María López (Coordinadora de Hardware y Cómputo)', 'Tecnico', 1),
('rgomez', 'tecnico123', 'Dr. Roberto Gómez (Especialista en Instrumental Científico)', 'Tecnico', 1),
('cmorales', 'tecnico123', 'Ing. Carlos Morales (Técnico Audiovisual y Multimedia)', 'Tecnico', 1);

-- 4. TÉCNICOS REGISTRADOS CON SUS RESPECTIVOS CORREOS Y ÁREAS
INSERT INTO Tecnico (Nombre, Correo, Especialidad, Activo) VALUES
('Ing. Jorge García', 'jgarcia@universidad.edu', 'Servidores, Cloud y Redes Cisco', 1),
('Lic. María López', 'mlopez@universidad.edu', 'Cómputo, Impresión 3D y Periféricos', 1),
('Dr. Roberto Gómez', 'rgomez@universidad.edu', 'Instrumental Médico, Óptico y Científico', 1),
('Ing. Carlos Morales', 'cmorales@universidad.edu', 'Sistemas Audiovisuales, Domótica y Proyección', 1);

-- 5. PLANES DE MANTENIMIENTO PREVENTIVO ESTRUCTURADOS
INSERT INTO Plan_Mantenimiento (IdEquipo, DescripcionTarea, FrecuenciaDias, Activo) VALUES
(1, 'Mantenimiento Preventivo Servidor: Limpieza de polvo, reemplazo de pasta térmica, verificación de arreglos RAID y actualización de parches Firmware', 90, 1),
(2, 'Revisión de Core de Red: Inspección de fibra óptica, limpieza de puertos SFP, respaldo de configuración Running-Config y prueba de redundancia', 180, 1),
(3, 'Mantenimiento Lab. Cómputo: Soplado interno de PCs, actualización de BIOS, pruebas de rendimiento GPU y recalibración de monitores', 60, 1),
(5, 'Calibración Óptica: Ajuste de vacío de columna electrónica, alineación de filamento de tungsteno y calibración de detectores SE/BSE', 120, 1),
(7, 'Mantenimiento Impresora 3D SLA: Limpieza de tanques de resina, calibración del sistema galvo láser y nivelación de plataforma de construcción', 30, 1),
(9, 'Mantenimiento Audiovisual: Limpieza de filtros de aire, verificación del bloque óptico láser, ajuste de convergencia y prueba de audio', 60, 1),
(11, 'Servicio Fotocopiadora Industrial: Reemplazo de rodillos de arrastre, limpieza de óptica de escáner y recarga/revisión de unidad de revelado', 45, 1);

-- 6. HISTORIAL Y PROGRAMACIÓN DE MANTENIMIENTOS CON DIFERENTES ESTADOS
INSERT INTO Mantenimiento (IdPlan, IdTecnico, FechaProgramada, FechaRealizada, Estado, Observaciones) VALUES
-- Mantenimientos Completados
(1, 1, '2024-01-10', '2024-01-10', 'Completado', 'Se realizó la limpieza física completa en el Datacenter. Discos RAID sin novedades.'),
(3, 2, '2024-01-15', '2024-01-16', 'Completado', 'Mantenimiento preventivo en Lab 301 ejecutado sin contratiempos. 30 estaciones listas.'),
(5, 2, '2024-02-01', '2024-02-02', 'Completado', 'Calibración de la impresora 3D Formlabs finalizada. Muestra de prueba aprobada.'),

-- Mantenimientos En Proceso / Pendientes
(4, 3, '2024-03-01', NULL, 'Pendiente', 'Pendiente de llegada del técnico especialista del fabricante Zeiss para calibración de columna.'),
(7, 2, '2024-03-05', NULL, 'En Proceso', 'En revisión de la fotocopiadora de Admisiones. Se solicitó cambio de kit de mantenimiento.'),

-- Mantenimientos Vigentes / Futuros
(2, 1, '2024-04-01', NULL, 'Vigente', 'Programado para la ventana de mantenimiento del periodo vacacional de semana santa.'),
(6, 4, '2024-04-15', NULL, 'Vigente', 'Revisión técnica de proyectores del Auditorio previo a la semana de conferencias.');

GO