using ConsultoriosApi.Application.Utils.Mediator;
using System;

namespace ConsultoriosApi.Application.UseCases.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentCommand : IRequest<Guid>
    {
        public Guid PatientId { get; set; }
        public Guid DentistId { get; set; }
        public Guid OfficeId { get; set; }
        /// <summary>Se interpreta en UTC.</summary>
        public DateTime Start { get; set; }
        /// <summary>Se interpreta en UTC.</summary>
        public DateTime End { get; set; }
    }
}
