using ConsultoriosApi.Application.Contracts.Authentication;
using ConsultoriosApi.Application.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ConsultoriosApi.Infrastructure.Authentication;

public class IdentityAuthService : IAuthService
{
    private readonly UserManager<AppUser> userManager;
    private readonly JwtSettings jwtSettings;

    public IdentityAuthService(UserManager<AppUser> userManager, IOptions<JwtSettings> jwtSettings)
    {
        this.userManager = userManager;
        this.jwtSettings = jwtSettings.Value;
    }

    public async Task<AuthResultDTO> Login(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            throw new UnauthorizedException();

        if (await userManager.IsLockedOutAsync(user))
            throw new UnauthorizedException();

        if (!await userManager.CheckPasswordAsync(user, password))
        {
            await userManager.AccessFailedAsync(user);
            throw new UnauthorizedException();
        }

        await userManager.ResetAccessFailedCountAsync(user);
        var roles = await userManager.GetRolesAsync(user);
        return GenerateToken(user, roles);
    }

    public async Task<Guid> Register(string email, string password, string role)
    {
        var user = new AppUser { Id = Guid.CreateVersion7(), UserName = email, Email = email };

        var result = await userManager.CreateAsync(user, password);
        ThrowIfFailed(result);

        result = await userManager.AddToRoleAsync(user, role);
        if (!result.Succeeded)
        {
            await userManager.DeleteAsync(user);
            ThrowIfFailed(result);
        }

        return user.Id;
    }

    private AuthResultDTO GenerateToken(AppUser user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiresAt = DateTime.UtcNow.AddMinutes(jwtSettings.ExpirationMinutes);

        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials);

        return new AuthResultDTO { Token = new JwtSecurityTokenHandler().WriteToken(token), ExpiresAt = expiresAt };
    }

    private static void ThrowIfFailed(IdentityResult result)
    {
        if (result.Succeeded)
            return;

        var failures = result.Errors.Select(error => new FluentValidation.Results.ValidationFailure(error.Code, error.Description));
        throw new ValidationException(new FluentValidation.Results.ValidationResult(failures));
    }
}
