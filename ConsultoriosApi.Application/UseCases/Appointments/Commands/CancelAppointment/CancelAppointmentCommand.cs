using ConsultoriosApi.Application.Utils.Mediator;
using System;

namespace ConsultoriosApi.Application.UseCases.Appointments.Commands.CancelAppointment
{
    public class CancelAppointmentCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
    }
}
