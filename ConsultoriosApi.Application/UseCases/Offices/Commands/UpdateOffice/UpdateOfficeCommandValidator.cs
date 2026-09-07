using FluentValidation;
using ConsultoriosApi.Application.UseCases.Offices.Commands.UpdateOffice;

namespace ConsultoriosApi.Application.UseCases.Offices.Commands.UpdateOffice
{
    public class UpdateOfficeCommandValidator : AbstractValidator<UpdateOfficeCommand>
    {
        public UpdateOfficeCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Field name is required ")
                .MaximumLength(150).WithMessage("Name field length must to be equal or less than 150");
        }
    }
}
