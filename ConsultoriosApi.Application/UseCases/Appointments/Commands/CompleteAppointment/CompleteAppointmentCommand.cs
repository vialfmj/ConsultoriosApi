using ConsultoriosApi.Application.Utils.Mediator;
using System;

namespace ConsultoriosApi.Application.UseCases.Appointments.Commands.CompleteAppointment
{
    public class CompleteAppointmentCommand : IRequest<Guid>
    {
        public Guid Id { get; set; }
    }
}
