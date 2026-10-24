using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaMantenimiento.Entities;
using SistemaMantenimiento.Models;
using SistemaMantenimiento.Services;

namespace SistemaMantenimiento.Controllers
{
    [Authorize(Roles = "Administrador,Tecnico")]
    public class MantenimientoController : Controller
    {
        private readonly IMantenimientoService _mantenimientoService;
        private readonly IEquipoService _equipoService;
        private readonly ITecnicoService _tecnicoService;

        public MantenimientoController(
            IMantenimientoService mantenimientoService,
            IEquipoService equipoService,
            ITecnicoService tecnicoService)
        {
            _mantenimientoService = mantenimientoService;
            _equipoService = equipoService;
            _tecnicoService = tecnicoService;
        }

        // ==========================================
        // 1. GET: /Mantenimiento/Index
        // ==========================================
        [HttpGet]
        public IActionResult Index()
        {
            var mantenimientos = _mantenimientoService.ObtenerTodos();

            var equipos = _equipoService.ObtenerTodos();
            var tecnicos = _tecnicoService.ObtenerTodos();

            ViewBag.TotalEquipos = equipos.Count();
            ViewBag.EquiposOperativos = equipos.Count(e => e.EstadoOperativo == "Operativo");
            ViewBag.EquiposEnMantenimiento = equipos.Count(e => e.EstadoOperativo != "Operativo");
            ViewBag.TecnicosTotales = tecnicos.Count();

            return View(mantenimientos);
        }

        // ==========================================
        // 2. GET: /Mantenimiento/Registrar/5 (Ejecutar / Finalizar Mantenimiento)
        // ==========================================
        [HttpGet]
        public IActionResult Registrar(int id)
        {
            CargarTecnicosDropDownList();

            var model = new RegistrarMantenimientoViewModel
            {
                IdMantenimiento = id,
                FechaRealizada = DateTime.Today
            };

            return View(model);
        }

        // ==========================================
        // 3. POST: /Mantenimiento/Registrar
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registrar(RegistrarMantenimientoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                CargarTecnicosDropDownList(model.IdTecnico);
                return View(model);
            }

            try
            {
                _mantenimientoService.RegistrarMantenimientoRealizado(
                    model.IdMantenimiento,
                    model.FechaRealizada,
                    model.IdTecnico,
                    model.Observaciones ?? ""
                );

                TempData["MensajeExito"] = "¡Mantenimiento registrado con éxito! Se ha calculado automáticamente la siguiente fecha programada.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                CargarTecnicosDropDownList(model.IdTecnico);
                return View(model);
            }
        }

        // ==========================================
        // 4. GET: /Mantenimiento/Editar/5
        // ==========================================
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var mantenimiento = _mantenimientoService.ObtenerPorId(id);
            if (mantenimiento == null)
            {
                return NotFound();
            }

            CargarTecnicosDropDownList(mantenimiento.IdTecnico);

            return View(mantenimiento);
        }

        // ==========================================
        // 5. POST: /Mantenimiento/Editar/5
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Mantenimiento model)
        {
            // Remover la validación del campo 'Estado' calculada para evitar falsos negativos en ModelState
            ModelState.Remove("Estado");

            if (!ModelState.IsValid)
            {
                CargarTecnicosDropDownList(model.IdTecnico);
                return View(model);
            }

            try
            {
                _mantenimientoService.Actualizar(model);
                TempData["MensajeExito"] = "Mantenimiento y asignación de técnico actualizados correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                CargarTecnicosDropDownList(model.IdTecnico);
                return View(model);
            }
        }

        // ==========================================
        // Método auxiliar para rellenar el DropDown de Técnicos
        // ==========================================
        private void CargarTecnicosDropDownList(int? idTecnicoSeleccionado = null)
        {
            var tecnicos = _tecnicoService.ObtenerTodos()
                .Where(t => t.Activo == 1)
                .OrderBy(t => t.Nombre);

            ViewBag.Tecnicos = new SelectList(tecnicos, "IdTecnico", "Nombre", idTecnicoSeleccionado);
            ViewBag.IdTecnico = ViewBag.Tecnicos;
        }
    }
}