using ConsultoriosApi.Dominio.Entities;

namespace ConsultoriosApi.Application.UseCases.Appointments.Commands.SendAppointmentReminder
{

    public static class MapperExtentions
    {
        public static AppointmentReminderDTO ToDto(this Appointment appointment)
        {
            return new AppointmentReminderDTO
            {
                Id = appointment.Id,
                Date = appointment.TimeInterval.Start,
                Patient = appointment.Patient!.Name,
                Patient_Email = appointment.Patient.Email.Valor,
                Office = appointment.Office!.Name,
                Dentist = appointment.Dentist!.Name
            };
        }
    }
}