using ConsultoriosApi.Application.Contracts.Authentication;
using ConsultoriosApi.Application.Utils.Mediator;

namespace ConsultoriosApi.Application.UseCases.Auth.Commands.Login
{
    public class LoginUseCase : IRequestHandler<LoginCommand, AuthResultDTO>
    {
        private readonly IAuthService authService;

        public LoginUseCase(IAuthService authService)
        {
            this.authService = authService;
        }
        public async Task<AuthResultDTO> Handle(LoginCommand command)
        {
            return await authService.Login(command.Email, command.Password);
        }
    }
}
