using System.ComponentModel.DataAnnotations;

namespace SistemaMantenimiento.Models
{
    public class RegistrarMantenimientoViewModel
    {
        public int IdMantenimiento { get; set; }

        [Required(ErrorMessage = "La fecha realizada es obligatoria")]
        [Display(Name = "Fecha Realizada")]
        [DataType(DataType.Date)]
        public DateTime FechaRealizada { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Debe seleccionar un técnico")]
        [Display(Name = "Técnico Asignado")]
        public int IdTecnico { get; set; }

        [Display(Name = "Observaciones / Detalles")]
        [StringLength(500)]
        public string? Observaciones { get; set; }
    }
}