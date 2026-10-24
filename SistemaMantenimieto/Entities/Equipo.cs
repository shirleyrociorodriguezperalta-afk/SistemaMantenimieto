using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMantenimiento.Entities
{
    [Table("Equipo")]
    public class Equipo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEquipo { get; set; }

        [Required]
        [StringLength(50)]
        public string CodigoActivo { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        public int IdTipoEquipo { get; set; }

        [Required]
        [StringLength(150)]
        public string Ubicacion { get; set; } = string.Empty;

        [StringLength(50)]
        public string EstadoOperativo { get; set; } = "Operativo";

        public DateTime? FechaIngreso { get; set; } = DateTime.Now;

        // Relaciones
        [ForeignKey("IdTipoEquipo")]
        public virtual TipoEquipo? TipoEquipo { get; set; }
    }
}