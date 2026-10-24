namespace ConsultoriosApi.Application.Contracts.Authentication
{
    public interface IAuthService
    {
        /// <exception cref="Exceptions.UnauthorizedException">Credenciales inválidas o usuario bloqueado.</exception>
        Task<AuthResultDTO> Login(string email, string password);

        /// <exception cref="Exceptions.ValidationException">El email ya existe o la password no cumple la política.</exception>
        Task<Guid> Register(string email, string password, string role);
    }
}
