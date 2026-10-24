using ConsultoriosApi.Application.Utils.Mediator;

namespace ConsultoriosApi.Application.UseCases.Auth.Commands.RegisterUser
{
    public class RegisterUserCommand : IRequest<Guid>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
    }
}
