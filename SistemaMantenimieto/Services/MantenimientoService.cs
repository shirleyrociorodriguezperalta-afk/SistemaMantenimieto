using System;
using System.Collections.Generic;
using SistemaMantenimiento.Entities;
using SistemaMantenimiento.Repositories;

namespace SistemaMantenimiento.Services
{
    public class MantenimientoService : IMantenimientoService
    {
        private readonly IMantenimientoRepository _mantenimientoRepo;
        private readonly IRepository<Tecnico> _tecnicoRepo;
        private readonly IRepository<PlanMantenimiento> _planRepo;
        private readonly IRepository<Equipo> _equipoRepo;

        public MantenimientoService(
            IMantenimientoRepository mantenimientoRepo,
            IRepository<Tecnico> tecnicoRepo,
            IRepository<PlanMantenimiento> planRepo,
            IRepository<Equipo> equipoRepo)
        {
            _mantenimientoRepo = mantenimientoRepo;
            _tecnicoRepo = tecnicoRepo;
            _planRepo = planRepo;
            _equipoRepo = equipoRepo;
        }

        // ==========================================
        // 1. Obtener TODOS los mantenimientos
        // ==========================================
        public IEnumerable<Mantenimiento> ObtenerTodos()
        {
            // Se eliminó la llamada a ActualizarEstadosDinamicos() ya que Estado se calcula al vuelo en C#
            return _mantenimientoRepo.GetAll();
        }

        // ==========================================
        // 2. Obtener un mantenimiento por ID
        // ==========================================
        public Mantenimiento? ObtenerPorId(int id)
        {
            return _mantenimientoRepo.GetById(id);
        }

        // ==========================================
        // 3. Obtener Historial de Mantenimientos por Equipo
        // ==========================================
        public IEnumerable<Mantenimiento> ObtenerHistorialPorEquipo(int idEquipo)
        {
            return _mantenimientoRepo.ObtenerHistorialPorEquipo(idEquipo);
        }

        // ==========================================
        // 4. Actualizar datos de un mantenimiento
        // ==========================================
        public void Actualizar(Mantenimiento mantenimiento)
        {
            var mantenimientoExistente = _mantenimientoRepo.GetById(mantenimiento.IdMantenimiento);

            if (mantenimientoExistente != null)
            {
                mantenimientoExistente.IdTecnico = mantenimiento.IdTecnico;
                mantenimientoExistente.FechaProgramada = mantenimiento.FechaProgramada;
                mantenimientoExistente.Observaciones = mantenimiento.Observaciones;
                // Se removió: mantenimientoExistente.Estado = mantenimiento.Estado;

                _mantenimientoRepo.Update(mantenimientoExistente);
                _mantenimientoRepo.Save();
            }
        }

        // ==========================================
        // 5. Métodos para Flujo de Trabajo y Negocio
        // ==========================================
        public DateTime CalcularProximaFecha(DateTime fechaRealizada, int frecuenciaDias)
        {
            return fechaRealizada.AddDays(frecuenciaDias);
        }

        public IEnumerable<Tecnico> ObtenerTecnicosActivos()
        {
            return _tecnicoRepo.Find(t => t.Activo == 1);
        }

        public IEnumerable<Mantenimiento> ObtenerPendientesYVencidos()
        {
            // Se realiza la consulta en SQL sobre la columna física FechaRealizada
            return _mantenimientoRepo.Find(m => m.FechaRealizada == null);
        }

        public void RegistrarMantenimientoRealizado(int idMantenimiento, DateTime fechaRealizada, int? idTecnico, string observaciones)
        {
            var mantenimientoActual = _mantenimientoRepo.GetById(idMantenimiento);
            if (mantenimientoActual == null)
                throw new InvalidOperationException("El mantenimiento especificado no existe.");

            var plan = _planRepo.GetById(mantenimientoActual.IdPlan);
            if (plan == null)
                throw new InvalidOperationException("El plan de mantenimiento asociado no existe.");

            var equipo = _equipoRepo.GetById(plan.IdEquipo);
            if (equipo == null)
                throw new InvalidOperationException("El equipo asociado no existe.");

            // REGLA DE NEGOCIO 1: No procesar mantenimientos de equipos fuera de servicio
            if (equipo.EstadoOperativo == "Fuera de Servicio")
            {
                throw new InvalidOperationException("No se puede registrar mantenimiento a un equipo en estado 'Fuera de Servicio'.");
            }

            // REGLA DE NEGOCIO 2: Evitar registrar fechas anteriores al último servicio
            var ultimoRealizado = _mantenimientoRepo.ObtenerUltimoRealizadoPorEquipo(equipo.IdEquipo);
            if (ultimoRealizado != null && ultimoRealizado.FechaRealizada.HasValue)
            {
                if (fechaRealizada < ultimoRealizado.FechaRealizada.Value)
                {
                    throw new InvalidOperationException("La fecha realizada no puede ser anterior al último mantenimiento completado.");
                }
            }

            // 1. Finalizar y cerrar el mantenimiento actual
            // Al asignar FechaRealizada, el getter de Estado devolverá automáticamente "Realizado"
            mantenimientoActual.FechaRealizada = fechaRealizada;
            mantenimientoActual.IdTecnico = idTecnico;
            mantenimientoActual.Observaciones = observaciones;
            _mantenimientoRepo.Update(mantenimientoActual);

            // 2. Calcular la siguiente fecha programada automáticamente
            DateTime proximaFecha = CalcularProximaFecha(fechaRealizada, plan.FrecuenciaDias);

            // 3. Crear y programar el próximo mantenimiento (sin asignar 'Estado' manualmente)
            var nuevoMantenimiento = new Mantenimiento
            {
                IdPlan = plan.IdPlan,
                FechaProgramada = proximaFecha
            };

            _mantenimientoRepo.Add(nuevoMantenimiento);
            _mantenimientoRepo.Save();
        }
    }
}