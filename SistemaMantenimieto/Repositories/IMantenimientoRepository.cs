using SistemaMantenimiento.Entities;

namespace SistemaMantenimiento.Repositories
{
    public interface IMantenimientoRepository : IRepository<Mantenimiento>
    {
        IEnumerable<Mantenimiento> ObtenerPendientesYVencidos();
        Mantenimiento? ObtenerUltimoRealizadoPorEquipo(int idEquipo);
        IEnumerable<Mantenimiento> ObtenerHistorialPorEquipo(int idEquipo);
    }
}
