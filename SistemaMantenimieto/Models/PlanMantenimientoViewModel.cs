using System.ComponentModel.DataAnnotations;

namespace SistemaMantenimiento.Models
{
    public class PlanMantenimientoViewModel
    {
        [Required(ErrorMessage = "Debe seleccionar un equipo")]
        public int IdEquipo { get; set; }

        [Required(ErrorMessage = "Ingrese la descripción de la tarea")]
        [StringLength(255)]
        public string DescripcionTarea { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingrese la frecuencia en días")]
        [Range(1, 3650, ErrorMessage = "La frecuencia debe ser mayor a 0 días")]
        public int FrecuenciaDias { get; set; }

        [Required(ErrorMessage = "Seleccione la primera fecha de mantenimiento")]
        public DateTime FechaPrimerMantenimiento { get; set; } = DateTime.Today;
    }
}