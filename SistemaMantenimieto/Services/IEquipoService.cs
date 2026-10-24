using SistemaMantenimiento.Entities;
using SistemaMantenimiento.Models;

namespace SistemaMantenimiento.Services
{
    public interface IEquipoService
    {
        IEnumerable<Equipo> ObtenerTodos();
        Equipo? ObtenerPorId(int id);
        void Crear(EquipoViewModel model);
        void Actualizar(EquipoViewModel model);
        void CambiarEstado(int id, string nuevoEstado);
        void Eliminar(int id);
        IEnumerable<TipoEquipo> ObtenerTiposEquipo();
    }
}