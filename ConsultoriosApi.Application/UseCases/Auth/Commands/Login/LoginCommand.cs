using ConsultoriosApi.Application.Contracts.Authentication;
using ConsultoriosApi.Application.Utils.Mediator;

namespace ConsultoriosApi.Application.UseCases.Auth.Commands.Login
{
    public class LoginCommand : IRequest<AuthResultDTO>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
