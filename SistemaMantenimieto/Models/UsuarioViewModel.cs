using System.ComponentModel.DataAnnotations;

namespace SistemaMantenimiento.Models
{
    public class CrearUsuarioViewModel
    {
        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [Display(Name = "Usuario")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar un rol.")]
        [Display(Name = "Rol asignado")]
        public string Rol { get; set; } = "Tecnico"; // 'Administrador' o 'Tecnico'
    }
}