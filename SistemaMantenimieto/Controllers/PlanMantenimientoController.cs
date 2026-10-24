using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaMantenimiento.Entities;
using SistemaMantenimiento.Models;
using SistemaMantenimiento.Repositories;
using SistemaMantenimiento.Services;

namespace SistemaMantenimiento.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class PlanMantenimientoController : Controller
    {
        private readonly IRepository<PlanMantenimiento> _planRepo;
        private readonly IMantenimientoRepository _mantenimientoRepo;
        private readonly IEquipoService _equipoService;

        public PlanMantenimientoController(
            IRepository<PlanMantenimiento> planRepo,
            IMantenimientoRepository mantenimientoRepo,
            IEquipoService equipoService)
        {
            _planRepo = planRepo;
            _mantenimientoRepo = mantenimientoRepo;
            _equipoService = equipoService;
        }

        // GET: /PlanMantenimiento/Crear
        [HttpGet]
        public IActionResult Crear(int? idEquipo = null)
        {
            var equipos = _equipoService.ObtenerTodos().Where(e => e.EstadoOperativo == "Operativo");
            ViewBag.IdEquipo = new SelectList(equipos, "IdEquipo", "Nombre", idEquipo);

            var model = new PlanMantenimientoViewModel
            {
                IdEquipo = idEquipo ?? 0,
                FechaPrimerMantenimiento = DateTime.Today.AddDays(7)
            };

            return View(model);
        }

        // POST: /PlanMantenimiento/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(PlanMantenimientoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var equipos = _equipoService.ObtenerTodos().Where(e => e.EstadoOperativo == "Operativo");
                ViewBag.IdEquipo = new SelectList(equipos, "IdEquipo", "Nombre", model.IdEquipo);
                return View(model);
            }

            try
            {
                // 1. Guardar el Plan de Mantenimiento
                var plan = new PlanMantenimiento
                {
                    IdEquipo = model.IdEquipo,
                    DescripcionTarea = model.DescripcionTarea,
                    FrecuenciaDias = model.FrecuenciaDias,
                    Activo = 1
                };

                _planRepo.Add(plan);
                _planRepo.Save(); // Guarda para obtener el IdPlan generado por la secuencia de Oracle

                // 2. Generar el primer mantenimiento programado para el tablero
                var primerMantenimiento = new Mantenimiento
                {
                    IdPlan = plan.IdPlan,
                    FechaProgramada = model.FechaPrimerMantenimiento,
                    Estado = model.FechaPrimerMantenimiento <= DateTime.Today.AddDays(7) ? "Proximo" : "Vigente"
                };

                _mantenimientoRepo.Add(primerMantenimiento);
                _mantenimientoRepo.Save();

                TempData["MensajeExito"] = "¡Plan de mantenimiento creado y primera fecha programada en el tablero!";
                return RedirectToAction("Index", "Mantenimiento");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al guardar en Oracle: " + ex.Message);
                var equipos = _equipoService.ObtenerTodos().Where(e => e.EstadoOperativo == "Operativo");
                ViewBag.IdEquipo = new SelectList(equipos, "IdEquipo", "Nombre", model.IdEquipo);
                return View(model);
            }
        }
    }
}