using System.ComponentModel.DataAnnotations;

namespace SistemaMantenimiento.Models
{
    public class TecnicoViewModel
    {
        public int IdTecnico { get; set; }

        [Required(ErrorMessage = "El nombre del técnico es obligatorio.")]
        [Display(Name = "Nombre Completo")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La especialidad es obligatoria.")]
        [Display(Name = "Especialidad")]
        public string Especialidad { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingrese una dirección de correo válida.")]
        [Display(Name = "Correo Electrónico")]
        public string Correo { get; set; } = string.Empty;

        [Display(Name = "Estado Activo")]
        public int Activo { get; set; } = 1; // Cambiado a int
    }
}