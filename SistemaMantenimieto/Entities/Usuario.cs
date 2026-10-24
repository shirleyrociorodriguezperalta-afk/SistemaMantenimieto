using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMantenimiento.Entities
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Clave { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string Rol { get; set; } = string.Empty;

        public int Activo { get; set; } = 1;

        // Propiedades auxiliares para C# (NO se crean en SQL Server)
        [NotMapped]
        public string Username
        {
            get => NombreUsuario;
            set => NombreUsuario = value;
        }

        [NotMapped]
        public string PasswordHash
        {
            get => Clave;
            set => Clave = value;
        }
    }
}