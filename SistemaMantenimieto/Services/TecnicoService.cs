using SistemaMantenimiento.Entities;
using SistemaMantenimiento.Models;
using SistemaMantenimiento.Repositories;

namespace SistemaMantenimiento.Services
{
    public class TecnicoService : ITecnicoService
    {
        private readonly IRepository<Tecnico> _tecnicoRepo;

        public TecnicoService(IRepository<Tecnico> tecnicoRepo)
        {
            _tecnicoRepo = tecnicoRepo;
        }

        public IEnumerable<Tecnico> ObtenerTodos()
        {
            return _tecnicoRepo.GetAll();
        }

        public Tecnico? ObtenerPorId(int id)
        {
            return _tecnicoRepo.GetById(id);
        }

        public void Crear(TecnicoViewModel model)
        {
            var existente = _tecnicoRepo.Find(t => t.Correo.ToLower() == model.Correo.ToLower()).FirstOrDefault();
            if (existente != null)
            {
                throw new InvalidOperationException($"Ya existe un técnico registrado con el correo {model.Correo}.");
            }

            var nuevoTecnico = new Tecnico
            {
                Nombre = model.Nombre,
                Especialidad = model.Especialidad,
                Correo = model.Correo,
                Activo = (short)model.Activo // Cast de int a short para la BD
            };

            _tecnicoRepo.Add(nuevoTecnico);
            _tecnicoRepo.Save();
        }

        public void Actualizar(TecnicoViewModel model)
        {
            var tecnico = _tecnicoRepo.GetById(model.IdTecnico);
            if (tecnico == null)
                throw new InvalidOperationException("El técnico especificado no existe.");

            tecnico.Nombre = model.Nombre;
            tecnico.Especialidad = model.Especialidad;
            tecnico.Correo = model.Correo;
            tecnico.Activo = (short)model.Activo; // Cast de int a short para la BD

            _tecnicoRepo.Update(tecnico);
            _tecnicoRepo.Save();
        }

        public void Eliminar(int id)
        {
            var tecnico = _tecnicoRepo.GetById(id);
            if (tecnico == null)
                throw new InvalidOperationException("El técnico especificado no existe.");

            _tecnicoRepo.Delete(id);
            _tecnicoRepo.Save();
        }
    }
}