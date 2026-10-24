using Microsoft.EntityFrameworkCore;
using SistemaMantenimiento.Entities;

namespace SistemaMantenimiento.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<Tecnico> Tecnicos { get; set; }
        public DbSet<Mantenimiento> Mantenimientos { get; set; }

        // Agregar este DbSet para mapear la tabla de Usuarios en Oracle:
        public DbSet<Usuario> Usuarios { get; set; }
    }
}