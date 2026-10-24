using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMantenimiento.Entities
{
    [Table("PlanMantenimiento")]
    public class PlanMantenimiento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPlan { get; set; }

        public int IdEquipo { get; set; }

        [Required]
        [StringLength(255)]
        public string DescripcionTarea { get; set; } = string.Empty;

        public int FrecuenciaDias { get; set; }

        public int Activo { get; set; } = 1;

        // Propiedad de navegación
        [ForeignKey("IdEquipo")]
        public virtual Equipo? Equipo { get; set; }
    }
}