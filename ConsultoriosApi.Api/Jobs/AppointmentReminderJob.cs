using ConsultoriosApi.Application.Utils.Mediator;

public class AppointmentReminderJob : BackgroundService
{
    private readonly IServiceScopeFactory _serviceProvider;
        private static readonly TimeZoneInfo ArgentinaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Argentina/Buenos_Aires");
    public AppointmentReminderJob(IServiceScopeFactory serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, ArgentinaTimeZone);
            if(now.Hour == 8)
            {
                using var scope = _serviceProvider.CreateScope();
                    
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Send(new SendAppointmentReminderCommand());
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}