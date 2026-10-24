using SistemaMantenimiento.Entities;

namespace SistemaMantenimiento.Services
{
    public interface IMantenimientoService
    {
        // Consultas generales para el tablero y formularios
        IEnumerable<Mantenimiento> ObtenerTodos();
        Mantenimiento? ObtenerPorId(int id);
        IEnumerable<Mantenimiento> ObtenerPendientesYVencidos();
        IEnumerable<Tecnico> ObtenerTecnicosActivos();

        // Consulta del historial por equipo
        IEnumerable<Mantenimiento> ObtenerHistorialPorEquipo(int idEquipo);

        // Operaciones y reglas de negocio
        void Actualizar(Mantenimiento mantenimiento);
        void RegistrarMantenimientoRealizado(int idMantenimiento, DateTime fechaRealizada, int? idTecnico, string observaciones);
        DateTime CalcularProximaFecha(DateTime fechaRealizada, int frecuenciaDias);
    }
}