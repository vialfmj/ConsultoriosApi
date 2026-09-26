using ConsultoriosApi.Application.Utils.Mediator;
using System;

namespace ConsultoriosApi.Application.UseCases.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommand : IRequest<Guid>
    {
        public Guid PatientId { get; set; }
        public Guid DentistId { get; set; }
        public Guid OfficeId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
