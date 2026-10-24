using SistemaMantenimiento.Entities;
using SistemaMantenimiento.Models;
using SistemaMantenimiento.Repositories;

namespace SistemaMantenimiento.Services
{
    public class EquipoService : IEquipoService
    {
        private readonly IRepository<Equipo> _equipoRepo;
        private readonly IRepository<TipoEquipo> _tipoEquipoRepo;

        public EquipoService(IRepository<Equipo> equipoRepo, IRepository<TipoEquipo> tipoEquipoRepo)
        {
            _equipoRepo = equipoRepo;
            _tipoEquipoRepo = tipoEquipoRepo;
        }

        public IEnumerable<Equipo> ObtenerTodos()
        {
            return _equipoRepo.GetAll();
        }

        public Equipo? ObtenerPorId(int id)
        {
            return _equipoRepo.GetById(id);
        }

        public IEnumerable<TipoEquipo> ObtenerTiposEquipo()
        {
            return _tipoEquipoRepo.GetAll();
        }

        public void Crear(EquipoViewModel model)
        {
            // Regla de Negocio: Validar que el código de activo no exista previamente
            var existente = _equipoRepo.Find(e => e.CodigoActivo == model.CodigoActivo).FirstOrDefault();
            if (existente != null)
            {
                throw new InvalidOperationException($"Ya existe un equipo registrado con el código de activo {model.CodigoActivo}.");
            }

            var nuevoEquipo = new Equipo
            {
                CodigoActivo = model.CodigoActivo,
                Nombre = model.Nombre,
                IdTipoEquipo = model.IdTipoEquipo,
                Ubicacion = model.Ubicacion,
                EstadoOperativo = model.EstadoOperativo,
                FechaIngreso = DateTime.Now
            };

            _equipoRepo.Add(nuevoEquipo);
            _equipoRepo.Save();
        }

        public void Actualizar(EquipoViewModel model)
        {
            var equipo = _equipoRepo.GetById(model.IdEquipo);
            if (equipo == null)
                throw new InvalidOperationException("El equipo especificado no existe.");

            equipo.Nombre = model.Nombre;
            equipo.IdTipoEquipo = model.IdTipoEquipo;
            equipo.Ubicacion = model.Ubicacion;
            equipo.EstadoOperativo = model.EstadoOperativo;

            _equipoRepo.Update(equipo);
            _equipoRepo.Save();
        }

        public void CambiarEstado(int id, string nuevoEstado)
        {
            var equipo = _equipoRepo.GetById(id);
            if (equipo == null)
                throw new InvalidOperationException("El equipo especificado no existe.");

            equipo.EstadoOperativo = nuevoEstado;
            _equipoRepo.Update(equipo);
            _equipoRepo.Save();
        }

        public void Eliminar(int id)
        {
            var equipo = _equipoRepo.GetById(id);
            if (equipo == null)
                throw new InvalidOperationException("El equipo especificado no existe.");

            // Se pasa el 'id' entero a Delete(...)
            _equipoRepo.Delete(id);
            _equipoRepo.Save();
        }
    }
}