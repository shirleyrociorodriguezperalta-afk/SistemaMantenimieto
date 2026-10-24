using SistemaMantenimiento.Entities;
using SistemaMantenimiento.Models;

namespace SistemaMantenimiento.Services
{
    public interface ITecnicoService
    {
        IEnumerable<Tecnico> ObtenerTodos();
        Tecnico? ObtenerPorId(int id);
        void Crear(TecnicoViewModel model);
        void Actualizar(TecnicoViewModel model);
        void Eliminar(int id);
    }
}