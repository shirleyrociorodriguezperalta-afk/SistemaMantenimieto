using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaMantenimiento.Models;
using SistemaMantenimiento.Services;

namespace SistemaMantenimiento.Controllers
{
    [Authorize(Roles = "Administrador")] // Solo el Administrador gestiona equipos
    public class EquipoController : Controller
    {
        private readonly IEquipoService _equipoService;
        private readonly IMantenimientoService _mantenimientoService;

        // Inyección de dependencias para ambos servicios
        public EquipoController(IEquipoService equipoService, IMantenimientoService mantenimientoService)
        {
            _equipoService = equipoService;
            _mantenimientoService = mantenimientoService;
        }

        // GET: /Equipo (Listar Equipos)
        public IActionResult Index()
        {
            var equipos = _equipoService.ObtenerTodos();
            return View(equipos);
        }

        // GET: /Equipo/Historial/5 (NUEVO: Muestra el historial de mantenimientos del equipo)
        [HttpGet]
        public IActionResult Historial(int id)
        {
            var equipo = _equipoService.ObtenerPorId(id);
            if (equipo == null)
            {
                return NotFound();
            }

            var historial = _mantenimientoService.ObtenerHistorialPorEquipo(id);

            ViewBag.Equipo = equipo;
            return View(historial);
        }

        // GET: /Equipo/Crear
        public IActionResult Crear()
        {
            var model = new EquipoViewModel
            {
                TiposEquipo = ObtenerTiposSelectList()
            };
            return View(model);
        }

        // POST: /Equipo/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(EquipoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.TiposEquipo = ObtenerTiposSelectList();
                return View(model);
            }

            try
            {
                _equipoService.Crear(model);
                TempData["MensajeExito"] = "Equipo registrado correctamente en el sistema.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                model.TiposEquipo = ObtenerTiposSelectList();
                return View(model);
            }
        }

        // GET: /Equipo/Editar/5
        public IActionResult Editar(int id)
        {
            var equipo = _equipoService.ObtenerPorId(id);
            if (equipo == null) return NotFound();

            var model = new EquipoViewModel
            {
                IdEquipo = equipo.IdEquipo,
                CodigoActivo = equipo.CodigoActivo,
                Nombre = equipo.Nombre,
                IdTipoEquipo = equipo.IdTipoEquipo,
                Ubicacion = equipo.Ubicacion,
                EstadoOperativo = equipo.EstadoOperativo,
                TiposEquipo = ObtenerTiposSelectList()
            };

            return View(model);
        }

        // POST: /Equipo/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(EquipoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.TiposEquipo = ObtenerTiposSelectList();
                return View(model);
            }

            try
            {
                _equipoService.Actualizar(model);
                TempData["MensajeExito"] = "Información del equipo actualizada con éxito.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                model.TiposEquipo = ObtenerTiposSelectList();
                return View(model);
            }
        }

        // POST: /Equipo/Eliminar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(int id)
        {
            try
            {
                _equipoService.Eliminar(id);
                TempData["MensajeExito"] = "El equipo fue eliminado correctamente del sistema.";
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "No se puede eliminar el equipo. Es posible que tenga registros o mantenimientos asociados.";
            }

            return RedirectToAction(nameof(Index));
        }

        private IEnumerable<SelectListItem> ObtenerTiposSelectList()
        {
            return _equipoService.ObtenerTiposEquipo().Select(t => new SelectListItem
            {
                Value = t.IdTipoEquipo.ToString(),
                Text = t.Nombre
            });
        }
    }
}