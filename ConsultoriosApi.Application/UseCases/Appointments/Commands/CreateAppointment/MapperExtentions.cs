

using ConsultoriosApi.Application.Contracts.Notifications;
using ConsultoriosApi.Dominio.Entities;

namespace ConsultoriosApi.Application.UseCases.Appointments.Commands.CreateAppointment
{
    public static class MapperExtentions
    {
        public static AppointmentConfirmationDTO ToDto(this Appointment appointment)
        {
            return new AppointmentConfirmationDTO
            {
                Id = appointment.Id,
                Patient = appointment.Patient!.Name,
                Patient_Email = appointment.Patient!.Email.Valor,
                Dentist = appointment.Dentist!.Name,
                Office = appointment.Office!.Name,
                Date = appointment.TimeInterval.Start,
            };
        }
    }
}