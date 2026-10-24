using Microsoft.EntityFrameworkCore;
using SistemaMantenimiento.Data;
using SistemaMantenimiento.Entities;

namespace SistemaMantenimiento.Repositories
{
    public class MantenimientoRepository : Repository<Mantenimiento>, IMantenimientoRepository
    {
        public MantenimientoRepository(ApplicationDbContext context) : base(context) { }

        // SOBREESCRIBIR GetAll() PARA INCLUIR LAS NAVEGACIONES
        public override IEnumerable<Mantenimiento> GetAll()
        {
            return _dbSet
                .Include(m => m.PlanMantenimiento)
                    .ThenInclude(p => p!.Equipo)
                .Include(m => m.Tecnico)
                .OrderByDescending(m => m.FechaProgramada)
                .ToList();
        }

        // SOBREESCRIBIR GetById() PARA EDICIÓN Y REGISTRO
        public override Mantenimiento? GetById(int id)
        {
            return _dbSet
                .Include(m => m.PlanMantenimiento)
                    .ThenInclude(p => p!.Equipo)
                .Include(m => m.Tecnico)
                .FirstOrDefault(m => m.IdMantenimiento == id);
        }

        public IEnumerable<Mantenimiento> ObtenerPendientesYVencidos()
        {
            return _dbSet
                .Include(m => m.PlanMantenimiento)
                    .ThenInclude(p => p!.Equipo)
                .Include(m => m.Tecnico)
                .Where(m => m.Estado != "Realizado")
                .OrderBy(m => m.FechaProgramada)
                .ToList();
        }

        public Mantenimiento? ObtenerUltimoRealizadoPorEquipo(int idEquipo)
        {
            return _dbSet
                .Include(m => m.PlanMantenimiento)
                .Where(m => m.PlanMantenimiento!.IdEquipo == idEquipo && m.Estado == "Realizado")
                .OrderByDescending(m => m.FechaRealizada)
                .FirstOrDefault();
        }

        public IEnumerable<Mantenimiento> ObtenerHistorialPorEquipo(int idEquipo)
        {
            return _dbSet
                .Include(m => m.PlanMantenimiento)
                .Include(m => m.Tecnico)
                .Where(m => m.PlanMantenimiento!.IdEquipo == idEquipo)
                .OrderByDescending(m => m.FechaProgramada)
                .ToList();
        }
    }
}