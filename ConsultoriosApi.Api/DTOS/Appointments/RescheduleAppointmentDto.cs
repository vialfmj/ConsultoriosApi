using System.ComponentModel.DataAnnotations;

namespace ConsultoriosApi.Api.DTOS.Appointments
{
    public class RescheduleAppointmentDto
    {
        [Required]
        public required DateTime Start { get; set; }

        [Required]
        public required DateTime End { get; set; }
    }
}
