using System.ComponentModel.DataAnnotations;

namespace ConsultoriosApi.Api.DTOS.Appointments
{
    public class RescheduleAppointmentDto
    {
        /// <summary>Incluir el offset de la zona horaria local (ej: -03:00).</summary>
        [Required]
        public required DateTimeOffset Start { get; set; }

        /// <summary>Incluir el offset de la zona horaria local (ej: -03:00).</summary>
        [Required]
        public required DateTimeOffset End { get; set; }
    }
}
