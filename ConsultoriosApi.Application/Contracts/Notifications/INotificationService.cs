

using ConsultoriosApi.Application.Contracts.Notifications;

public interface INotificationService
{
    Task SendAppointmentConfirmationAsync(AppointmentConfirmationDTO confirmation);
    Task SendAppointmentReminderAsync(AppointmentReminderDTO reminder);
}