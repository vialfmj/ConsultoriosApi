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

        [Required]
        public required DateTime Start { get; set; }

        [Required]
        public required DateTime End { get; set; }
    }
}
