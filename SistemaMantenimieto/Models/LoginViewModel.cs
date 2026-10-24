using System.ComponentModel.DataAnnotations;

namespace SistemaMantenimiento.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El correo o usuario es obligatorio.")]
        [Display(Name = "Usuario / Correo")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }
    }
}