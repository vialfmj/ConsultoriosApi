using FluentValidation;

namespace ConsultoriosApi.Application.UseCases.Dentists.Commands.UpdateDentist
{
    public class UpdateDentistCommandValidator : AbstractValidator<UpdateDentistCommand>
    {
        public UpdateDentistCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.");
        }
    }
}
