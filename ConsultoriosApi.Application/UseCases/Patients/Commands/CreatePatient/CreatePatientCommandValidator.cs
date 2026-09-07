using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Patients.Commands.CreatePatient
{
    public class CreatePatientCommandValidator : AbstractValidator<CreatePatientCommand>
    {
        public CreatePatientCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.");
        }
    }
}
