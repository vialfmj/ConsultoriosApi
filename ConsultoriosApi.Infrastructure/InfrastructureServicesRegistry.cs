

using ConsultoriosApi.Infrastructure.Notifications;
using Microsoft.Extensions.DependencyInjection;

namespace ConsultoriosApi.Infrastructure
{
    public static class InfrastructureServicesRegistry
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<INotificationService, MailService>();
            return services;
        }
    }
}