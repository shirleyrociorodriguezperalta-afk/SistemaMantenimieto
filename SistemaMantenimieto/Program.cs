using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SistemaMantenimiento.Data;
using SistemaMantenimiento.Entities;
using SistemaMantenimiento.Repositories;
using SistemaMantenimiento.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de DbContext con SQL Server LocalDB
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("La cadena 'DefaultConnection' no fue encontrada en appsettings.json.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Registro de Repositorios (Inyección de Dependencias)
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IMantenimientoRepository, MantenimientoRepository>();

// 3. Registro de Servicios de Negocio
builder.Services.AddScoped<IEquipoService, EquipoService>();
builder.Services.AddScoped<ITecnicoService, TecnicoService>();
builder.Services.AddScoped<IMantenimientoService, MantenimientoService>();

// 4. Agregar controladores y vistas MVC
builder.Services.AddControllersWithViews();

// 5. Configurar Autenticación basada en Cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

// ¡CONSTRUCCIÓN DE LA APLICACIÓN!
var app = builder.Build();

// 6. Pipeline de Peticiones HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// 7. Middlewares de Autenticación y Autorización
app.UseAuthentication();
app.UseAuthorization();

// 8. Definición de la Ruta Predeterminada
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();