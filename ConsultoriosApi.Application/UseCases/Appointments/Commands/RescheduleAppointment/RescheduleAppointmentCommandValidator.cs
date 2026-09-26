using FluentValidation;

namespace ConsultoriosApi.Application.UseCases.Appointments.Commands.RescheduleAppointment
{
    public class RescheduleAppointmentCommandValidator : AbstractValidator<RescheduleAppointmentCommand>
    {
        public RescheduleAppointmentCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("El turno es obligatorio.");

            RuleFor(x => x.Start)
                .NotEmpty().WithMessage("La fecha de inicio es obligatoria.");

            RuleFor(x => x.End)
                .NotEmpty().WithMessage("La fecha de fin es obligatoria.");
        }
    }
}
