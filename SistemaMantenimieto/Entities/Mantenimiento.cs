using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaMantenimiento.Entities
{
    [Table("Mantenimiento")]
    public class Mantenimiento
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdMantenimiento { get; set; }

        public int IdPlan { get; set; }

        public int? IdTecnico { get; set; }

        public DateTime FechaProgramada { get; set; }

        public DateTime? FechaRealizada { get; set; }

        [StringLength(500)]
        public string? Observaciones { get; set; }

        // PROPIEDAD CALCULADA AUTOMÁTICAMENTE
        // [NotMapped] indica a EF Core que no cree/busque esta columna en la BD.
        [NotMapped]
        public string Estado
        {
            get
            {
                // 1. Si ya se registró una fecha de realización, pasa a Realizado
                if (FechaRealizada.HasValue)
                {
                    return "Realizado";
                }

                var hoy = DateTime.Today;
                var diasDiferencia = (FechaProgramada.Date - hoy).Days;

                // 2. Si la fecha ya pasó y no se realizó
                if (diasDiferencia < 0)
                {
                    return "Vencido";
                }
                // 3. Si falta 1 semana o menos (0 a 7 días)
                else if (diasDiferencia <= 7)
                {
                    return "Próximo";
                }
                // 4. Si faltan más de 7 días
                else
                {
                    return "Vigente";
                }
            }
            set
            {
                // El set vacío evita el error de solo lectura durante el model binding del formulario
            }
        }

        // Relaciones de navegación
        [ForeignKey("IdPlan")]
        public virtual PlanMantenimiento? PlanMantenimiento { get; set; }

        [ForeignKey("IdTecnico")]
        public virtual Tecnico? Tecnico { get; set; }
    }
}