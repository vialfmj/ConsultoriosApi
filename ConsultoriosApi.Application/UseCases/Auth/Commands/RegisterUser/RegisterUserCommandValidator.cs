using ConsultoriosApi.Application.Contracts.Authentication;
using FluentValidation;

namespace ConsultoriosApi.Application.UseCases.Auth.Commands.RegisterUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email is not valid.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must have at least 8 characters.");

            RuleFor(x => x.Role)
                .Must(role => AuthRoles.All.Contains(role)).WithMessage($"Role must be one of: {string.Join(", ", AuthRoles.All)}.");
        }
    }
}
