using System.ComponentModel.DataAnnotations;

namespace ConsultoriosApi.Api.DTOS.Dentists
{
    public class UpdateDentistDto
    {
        [Required]
        [StringLength(150)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }
    }
}
