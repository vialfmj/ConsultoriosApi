using ConsultoriosApi.Application.Contracts.Authentication;
using ConsultoriosApi.Application.Utils.Mediator;

namespace ConsultoriosApi.Application.UseCases.Auth.Commands.RegisterUser
{
    public class RegisterUserUseCase : IRequestHandler<RegisterUserCommand, Guid>
    {
        private readonly IAuthService authService;

        public RegisterUserUseCase(IAuthService authService)
        {
            this.authService = authService;
        }
        // No usa IUnitOfWork: la persistencia del usuario es responsabilidad de la implementación de IAuthService.
        public async Task<Guid> Handle(RegisterUserCommand command)
        {
            return await authService.Register(command.Email, command.Password, command.Role);
        }
    }
}
