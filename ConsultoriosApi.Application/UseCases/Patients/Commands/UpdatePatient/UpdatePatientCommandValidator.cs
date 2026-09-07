using FluentValidation;

namespace ConsultoriosApi.Application.UseCases.Patients.Commands.UpdatePatient
{
    public class UpdatePatientCommandValidator : AbstractValidator<UpdatePatientCommand>
    {
        public UpdatePatientCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.");
        }
    }
}
