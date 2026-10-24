using ConsultoriosApi.Api.DTOS.Auth;
using ConsultoriosApi.Application.Contracts.Authentication;
using ConsultoriosApi.Application.UseCases.Auth.Commands.Login;
using ConsultoriosApi.Application.UseCases.Auth.Commands.RegisterUser;
using ConsultoriosApi.Application.Utils.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConsultoriosApi.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;

        public AuthController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResultDTO>> Login(LoginDto dto)
        {
            var command = new LoginCommand { Email = dto.Email, Password = dto.Password };
            return await mediator.Send(command);
        }
        [Authorize(Roles = AuthRoles.Admin)]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            var command = new RegisterUserCommand { Email = dto.Email, Password = dto.Password, Role = dto.Role };
            await mediator.Send(command);
            return Ok();
        }
    }
}
