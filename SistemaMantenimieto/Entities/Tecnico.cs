using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMantenimiento.Entities
{
    [Table("Tecnico")]
    public class Tecnico
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTecnico { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(150)]
        public string? Correo { get; set; }

        [Required]
        [StringLength(100)]
        public string Especialidad { get; set; } = string.Empty;

        public int Activo { get; set; } = 1;
    }
}