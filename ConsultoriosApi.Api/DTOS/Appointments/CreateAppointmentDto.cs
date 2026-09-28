using System.ComponentModel.DataAnnotations;

namespace ConsultoriosApi.Api.DTOS.Appointments
{
    public class CreateAppointmentDto
    {
        [Required]
        public required Guid PatientId { get; set; }

        [Required]
        public required Guid DentistId { get; set; }

        [Required]
        public required Guid OfficeId { get; set; }

        /// <summary>Incluir el offset de la zona horaria local (ej: -03:00).</summary>
        [Required]
        public required DateTimeOffset Start { get; set; }

        /// <summary>Incluir el offset de la zona horaria local (ej: -03:00).</summary>
        [Required]
        public required DateTimeOffset End { get; set; }
    }
}
