using ConsultoriosApi.Dominio.Enums;
using ConsultoriosApi.Dominio.Exceptions;
using ConsultoriosApi.Dominio.ValueObjects;

namespace ConsultoriosApi.Dominio.Entities
{
    public class Appointment
    {
        public Guid Id { get; private set; }
        public Guid PatientId { get; private set; }
        public Guid DentistId { get; private set; }
        public Guid OfficeId { get; private set; }
        public TimeInterval TimeInterval { get; private set; }
        public DateState State { get; private set; }
        public Patient? Patient { get; private set; }
        public Dentist? Dentist { get; private set; }
        public Office? Office { get; private set; }

        public Appointment(Guid patientId, Guid dentistId, Guid officeId, TimeInterval timeInterval)
        {
            if (timeInterval.Start > DateTime.UtcNow)
            {
                throw new BusinessRuleException("The Start date cannot be earlier than the current date,");
            }

            PatientId = patientId;
            DentistId = dentistId;
            OfficeId = officeId;
            TimeInterval = timeInterval;
            State = DateState.Scheduled;
            Id = Guid.CreateVersion7();

        }
        public void Cancel()
        {
            if (State != DateState.Scheduled)
            {
                throw new BusinessRuleException("Only scheduled appointments can be canceled.");
            }

            State = DateState.Canceled;
        }

        public void Complete()
        {
            if (State != DateState.Scheduled)
            {
                throw new BusinessRuleException("Only scheduled appointments can be completed.");
            }

            State = DateState.Canceled;
        }


    }
}
