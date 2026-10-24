using System.ComponentModel.DataAnnotations;

namespace ConsultoriosApi.Api.DTOS.Auth
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
