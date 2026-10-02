
using ConsultoriosApi.Application.Contracts.Notifications;
using Microsoft.Extensions.Configuration;

namespace ConsultoriosApi.Infrastructure.Notifications;

public class MailService : INotificationService
{
    private readonly IConfiguration configuration;

    public MailService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    private static readonly TimeZoneInfo ArgentinaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Argentina/Buenos_Aires");

    public async Task SendAppointmentConfirmationAsync(AppointmentConfirmationDTO confirmation)
    {
        var localDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(confirmation.Date, DateTimeKind.Utc), ArgentinaTimeZone);
        var subject = "Confirmación de cita";
        var body = $"Estimado/a {confirmation.Patient},\n\n" +
                   $"Su cita ha sido confirmada para el día {localDate:dd/MM/yyyy} a las {localDate:HH:mm}.\n\n" +
                   $"Consultorio: {confirmation.Office}\n\n" +
                   "Gracias por elegir nuestro consultorio.";
        await SendMesaje(confirmation.Patient_Email, subject, body);

    }

    public async Task SendAppointmentReminderAsync(AppointmentReminderDTO reminder)
    {
        var localDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(reminder.Date, DateTimeKind.Utc), ArgentinaTimeZone);
        var subject = "Recordatorio de cita";
        var body = $"Estimado/a {reminder.Patient},\n\n" +
                   $"Le recordamos que tiene una cita programada para el día {localDate:dd/MM/yyyy} a las {localDate:HH:mm}.\n\n" +
                   $"Consultorio: {reminder.Office}\n\n" +
                   "Por favor, asegúrese de llegar a tiempo.";
        await SendMesaje(reminder.Patient_Email, subject, body);
    }

    private async Task SendMesaje(string patient_Email, string subject, string body)
    {
        var ourEmail = configuration.GetValue<string>("EmailSettings:OurEmail");
        var ourEmailPassword = configuration.GetValue<string>("EmailSettings:OurEmailPassword");
        var host = configuration.GetValue<string>("EmailSettings:Host");
        var port = configuration.GetValue<int>("EmailSettings:Port");

        var smtpClient = new System.Net.Mail.SmtpClient(host, port);

        smtpClient.Credentials = new System.Net.NetworkCredential(ourEmail, ourEmailPassword);
        smtpClient.EnableSsl = true;
        smtpClient.UseDefaultCredentials = false;

        var mailMessage = new System.Net.Mail.MailMessage(ourEmail!, patient_Email, subject, body);
        await smtpClient.SendMailAsync(mailMessage);
    }
}