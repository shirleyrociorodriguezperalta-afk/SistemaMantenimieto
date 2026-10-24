using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaMantenimiento.Data;
using SistemaMantenimiento.Entities;
using SistemaMantenimiento.Models;

namespace SistemaMantenimiento.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Usuario/Index (Lista de Usuarios)
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios.ToListAsync();
            return View(usuarios);
        }

        // GET: /Usuario/Crear
        public IActionResult Crear()
        {
            return View();
        }

        // POST: /Usuario/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(CrearUsuarioViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                string usernameLimpio = model.Username.Trim();

                // 1. Verificación compatible con Oracle (evita ORA-00904)
                var usuarioExistente = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Username.ToUpper() == usernameLimpio.ToUpper());

                if (usuarioExistente != null)
                {
                    ModelState.AddModelError("Username", $"El usuario '{usernameLimpio}' ya se encuentra registrado.");
                    return View(model);
                }

                // 2. Crear nueva entidad con Hash de BCrypt
                var nuevoUsuario = new Usuario
                {
                    Username = usernameLimpio,
                    NombreCompleto = model.NombreCompleto.Trim(),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    Rol = model.Rol,
                    Activo = 1
                };

                _context.Usuarios.Add(nuevoUsuario);

                // 3. Persistir en la base de datos Oracle
                await _context.SaveChangesAsync();

                TempData["MensajeExito"] = $"Usuario '{nuevoUsuario.Username}' creado y guardado exitosamente en Oracle.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al guardar en Oracle: {ex.InnerException?.Message ?? ex.Message}");
                return View(model);
            }
        }

        // POST: /Usuario/CambiarEstado (Activar / Desactivar acceso)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            // Cambiar entre 1 (Activo) y 0 (Inactivo)
            usuario.Activo = usuario.Activo == 1 ? 0 : 1;
            await _context.SaveChangesAsync();

            TempData["MensajeExito"] = $"Estado de '{usuario.Username}' actualizado.";
            return RedirectToAction(nameof(Index));
        }
    }
}