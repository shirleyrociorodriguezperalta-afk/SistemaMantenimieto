using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemaMantenimiento.Models
{
    public class EquipoViewModel
    {
        public int IdEquipo { get; set; }

        [Required(ErrorMessage = "El código de activo es obligatorio.")]
        [Display(Name = "Código de Activo")]
        public string CodigoActivo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del equipo es obligatorio.")]
        [Display(Name = "Nombre del Equipo")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar un tipo de equipo.")]
        [Display(Name = "Tipo de Equipo")]
        public int IdTipoEquipo { get; set; }

        [Required(ErrorMessage = "La ubicación es obligatoria.")]
        [Display(Name = "Ubicación / Aula / Lab")]
        public string Ubicacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estado operativo es obligatorio.")]
        [Display(Name = "Estado Operativo")]
        public string EstadoOperativo { get; set; } = "Operativo";

        // Para poblar el select de Tipos de Equipo en la vista
        public IEnumerable<SelectListItem>? TiposEquipo { get; set; }
    }
}