================================================================================
  GUÍA COMPLETA DE INSTALACIÓN Y EJECUCIÓN - SISTEMA DE MANTENIMIENTO (.NET 8)
================================================================================
--------------------------------------------------------------------------------
1. REQUISITOS PREVIOS (REQUISITOS DEL ENTORNO)
--------------------------------------------------------------------------------
Antes de abrir el proyecto, asegúrese de tener instalado en su equipo:

1. Visual Studio 2022 (Community, Professional o Enterprise).
2. Durante la instalación de Visual Studio, asegurarse de tener seleccionadas:
   - Carga de trabajo: "Desarrollo de ASP.NET y web".
   - Componente individual: "SQL Server Express LocalDB" (viene por defecto con la carga web).
3. SDK de .NET 8.0.

--------------------------------------------------------------------------------
2. PASO A PASO 
--------------------------------------------------------------------------------

PASO 1: EXTRAER EL PROYECTO
- Descomprima el archivo '.ZIP' en una carpeta local de su preferencia (ej: C:\Proyectos\SistemaMantenimiento).
- IMPORTANTE: Evite carpetas temporales o comprimidas directas sin extraer.

PASO 2: ABRIR LA SOLUCIÓN EN VISUAL STUDIO
- Inicie Visual Studio 2022.
- Seleccione "Abrir una solución o un proyecto".
- Navegue a la carpeta extraída y abra el archivo: 'SistemaMantenimiento.sln'.

PASO 3: RESTAURAR PAQUETES NUGET
- En la parte superior de Visual Studio, haga clic en:
  'Compilar' -> 'Restaurar paquetes NuGet' (o Simplemente compile con Ctrl + Shift + B).
- Esto descargará las librerías necesarias de Entity Framework Core.

PASO 4: CREAR Y POBLAR LA BASE DE DATOS LOCAL (MIGRACIONES)
Dado que el proyecto utiliza Entity Framework Core Code-First, debe generar la 
base de datos en su instancia local mediante la Consola de NuGet:

1. En el menú superior de Visual Studio, vaya a:
   Herramientas -> Administrador de paquetes NuGet -> Consola del Administrador de paquetes.
   
2. En la consola desplegada en la parte inferior, verifique que el "Proyecto predeterminado"
   sea 'SistemaMantenimiento'.

3. Escriba el siguiente comando y presione ENTER:

   Update-Database

4. Esperar el mensaje "Done!". Entity Framework creará automáticamente la base de datos 
   'SistemaMantenimientoDB' en su LocalDB con todas sus tablas e integridad relacional.

PASO 5: EJECUTAR LA APLICACIÓN
- Presione la tecla 'F5' (o el botón verde de reproducción "IIS Express / SistemaMantenimiento").
- El navegador web predeterminado se abrirá automáticamente en la vista principal (Home/Index).

--------------------------------------------------------------------------------
3. DATOS DE CONEXIÓN Y NOTAS TÉCNICAS
--------------------------------------------------------------------------------
- Cadena de conexión empleada ('appsettings.json'):
  "Server=(localdb)\\mssqllocaldb;Database=SistemaMantenimientoDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"

- No se requieren configuraciones de contraseñas ni instancias de SQL Server externas, 
  ya que hace uso de la autenticación Windows nativa en LocalDB.
- En caso de requerir reajustar los datos de prueba, la base de datos se puede consultar 
  en Visual Studio desde: Ver -> Explorador de objetos de SQL Server -> (localdb)\MSSQLLocalDB.

================================================================================
