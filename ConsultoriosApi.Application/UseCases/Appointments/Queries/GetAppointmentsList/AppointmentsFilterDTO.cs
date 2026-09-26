using ConsultoriosApi.Dominio.Enums;
using System;

namespace ConsultoriosApi.Application.UseCases.Appointments.Queries.GetAppointmentsList
{
    public class AppointmentsFilterDTO
    {
        public int Page { get; set; } = 1;
        public int RecordsPerPage { get; set; } = 10;
        public Guid? PatientId { get; set; }
        public Guid? DentistId { get; set; }
        public Guid? OfficeId { get; set; }
        public DateState? State { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
