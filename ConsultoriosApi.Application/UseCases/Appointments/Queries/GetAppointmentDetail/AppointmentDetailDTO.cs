using ConsultoriosApi.Dominio.Enums;
using System;

namespace ConsultoriosApi.Application.UseCases.Appointments.Queries.GetAppointmentDetail
{
    public class AppointmentDetailDTO
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public Guid DentistId { get; set; }
        public Guid OfficeId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateState State { get; set; }
    }
}
