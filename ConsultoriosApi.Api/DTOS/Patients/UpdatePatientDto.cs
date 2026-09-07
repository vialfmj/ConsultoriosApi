using System.ComponentModel.DataAnnotations;

namespace ConsultoriosApi.Api.DTOS.Patients
{
    public class UpdatePatientDto
    {
        [Required]
        [StringLength(150)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
