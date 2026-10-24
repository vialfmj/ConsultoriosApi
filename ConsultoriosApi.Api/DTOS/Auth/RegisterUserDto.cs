using System.ComponentModel.DataAnnotations;

namespace ConsultoriosApi.Api.DTOS.Auth
{
    public class RegisterUserDto
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [MinLength(8)]
        public required string Password { get; set; }

        [Required]
        public required string Role { get; set; }
    }
}
