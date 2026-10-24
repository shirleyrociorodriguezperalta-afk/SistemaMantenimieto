using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaMantenimiento.Models;
using SistemaMantenimiento.Services;

namespace SistemaMantenimiento.Controllers
{
    [Authorize(Roles = "Administrador")] // OBLIGATORIO: Solo el Administrador puede gestionar técnicos
    public class TecnicoController : Controller
    {
        private readonly ITecnicoService _tecnicoService;

        public TecnicoController(ITecnicoService tecnicoService)
        {
            _tecnicoService = tecnicoService;
        }

        // GET: /Tecnico
        public IActionResult Index()
        {
            var tecnicos = _tecnicoService.ObtenerTodos();
            return View(tecnicos);
        }

        // GET: /Tecnico/Crear
        public IActionResult Crear()
        {
            return View(new TecnicoViewModel());
        }

        // POST: /Tecnico/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(TecnicoViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                _tecnicoService.Crear(model);
                TempData["MensajeExito"] = "Técnico registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: /Tecnico/Editar/5
        public IActionResult Editar(int id)
        {
            var tecnico = _tecnicoService.ObtenerPorId(id);
            if (tecnico == null) return NotFound();

            var model = new TecnicoViewModel
            {
                IdTecnico = tecnico.IdTecnico,
                Nombre = tecnico.Nombre,
                Especialidad = tecnico.Especialidad,
                Correo = tecnico.Correo,
                Activo = (int)tecnico.Activo // Cast de short (Entidad) a int (ViewModel)
            };

            return View(model);
        }

        // POST: /Tecnico/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(TecnicoViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                _tecnicoService.Actualizar(model);
                TempData["MensajeExito"] = "Datos del técnico actualizados correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // POST: /Tecnico/Eliminar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(int id)
        {
            try
            {
                _tecnicoService.Eliminar(id);
                TempData["MensajeExito"] = "Técnico eliminado correctamente del sistema.";
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "No se puede eliminar el técnico. Es posible que tenga mantenimientos asignados.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}