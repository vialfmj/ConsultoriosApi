using FluentValidation;

namespace ConsultoriosApi.Application.UseCases.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
    {
        public CreateAppointmentCommandValidator()
        {
            RuleFor(x => x.PatientId)
                .NotEmpty().WithMessage("El paciente es obligatorio.");

            RuleFor(x => x.DentistId)
                .NotEmpty().WithMessage("El dentista es obligatorio.");

            RuleFor(x => x.OfficeId)
                .NotEmpty().WithMessage("El consultorio es obligatorio.");

            RuleFor(x => x.Start)
                .NotEmpty().WithMessage("La fecha de inicio es obligatoria.");

            RuleFor(x => x.End)
                .NotEmpty().WithMessage("La fecha de fin es obligatoria.");
        }
    }
}
