using ConsultoriosApi.Dominio.Entities;

namespace ConsultoriosApi.Application.UseCases.Appointments.Queries.GetAppointmentDetail
{
    public static class MapperExtensions
    {
        public static AppointmentDetailDTO ToDto(this Appointment appointment)
        {
            var dto = new AppointmentDetailDTO
            {
                Id = appointment.Id,
                PatientId = appointment.PatientId,
                DentistId = appointment.DentistId,
                OfficeId = appointment.OfficeId,
                Start = appointment.TimeInterval.Start,
                End = appointment.TimeInterval.End,
                State = appointment.State
            };
            return dto;
        }
    }
}
