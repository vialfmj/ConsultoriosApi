using ConsultoriosApi.Application.Utils.Mediator;
using System;

namespace ConsultoriosApi.Application.UseCases.Appointments.Commands.RescheduleAppointment
{
    public class RescheduleAppointmentCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
