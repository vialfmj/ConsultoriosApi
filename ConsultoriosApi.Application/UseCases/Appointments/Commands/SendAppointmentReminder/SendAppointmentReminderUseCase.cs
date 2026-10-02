using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Contracts.Repositories.Models;
using ConsultoriosApi.Application.Utils.Mediator;
using ConsultoriosApi.Dominio.Enums;

namespace ConsultoriosApi.Application.UseCases.Appointments.Commands.SendAppointmentReminder
{
    public class SendAppointmentReminderUseCase : IRequestHandler<SendAppointmentReminderCommand>
    {
        private static readonly TimeZoneInfo ArgentinaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Argentina/Buenos_Aires");

        private readonly IAppointmentsRepository _appointmentRepository;
        private readonly INotificationService _notificationService;

        public SendAppointmentReminderUseCase(IAppointmentsRepository appointmentRepository, INotificationService emailService)
        {
            _appointmentRepository = appointmentRepository;
            _notificationService = emailService;
        }

        public async Task Handle(SendAppointmentReminderCommand request)
        {
            // Se calcula la fecha de "mañana" en horario de Argentina (no UTC),
            // para que la ventana del filtro sea el día calendario completo en esa zona horaria.
            var nowArgentina = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, ArgentinaTimeZone);
            var tomorrow = nowArgentina.Date.AddDays(1);
            var offset = ArgentinaTimeZone.GetUtcOffset(tomorrow);

            var appointmentfilter = new AppointmentsFilterDTO
            {
                State = DateState.Scheduled,
                From = new DateTimeOffset(tomorrow, offset),
                To = new DateTimeOffset(tomorrow.AddDays(1), offset)
            };

            var appointments = await _appointmentRepository.GetFiltered(appointmentfilter);

            foreach (var appointment in appointments)
            {
                var appointmentDTO = appointment.ToDto();

                await _notificationService.SendAppointmentReminderAsync(appointmentDTO);
            }
        }
    }
}

