using ConsultoriosApi.Dominio.Entities;

namespace ConsultoriosApi.Application.UseCases.Appointments.Queries.GetAppointmentsList
{
    public static class MapperExtensions
    {
        public static AppointmentsListDTO ToDto(this Appointment appointment)
        {
            var dto = new AppointmentsListDTO
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
