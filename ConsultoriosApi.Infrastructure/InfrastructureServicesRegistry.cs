using ConsultoriosApi.Application.Contracts.Authentication;
using ConsultoriosApi.Application.Contracts.Notifications;
using ConsultoriosApi.Infrastructure.Authentication;
using ConsultoriosApi.Infrastructure.Notifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConsultoriosApi.Infrastructure
{
    public static class InfrastructureServicesRegistry
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<INotificationService, MailService>();

            services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ConsultoriosApiConnectionString")));

            services.AddIdentityCore<AppUser>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequiredLength = 8;
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                })
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<AuthDbContext>();

            services.Configure<JwtSettings>(options => configuration.GetSection(JwtSettings.SectionName).Bind(options));
            services.AddScoped<IAuthService, IdentityAuthService>();
            return services;
        }
    }
}
