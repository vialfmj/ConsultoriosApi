using ConsultoriosApi.Application.Contracts.Persistence;
using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Persistence.Repositories;
using ConsultoriosApi.Persistence.UnitsOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace ConsultoriosApi.Persistence
{
    public static class PersistenceServiceRegistry
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<ConsultoriosApiDbContext>(options =>
                options.UseSqlServer("name=ConsultoriosApiConnectionString"));

            services.AddScoped<IUnitOfWork, EFUnitOfWork>();
            services.AddScoped<IOfficesRepository, OfficesRepository>();
            services.AddScoped<IPatientsRepository, PatientsRepository>();
            services.AddScoped<IDentistsRepository, DentistsRepository>();

            return services;
        } 
    }
}
