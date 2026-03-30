using ClassLibrary1ConsultoriosApi.Persistence.Repositories;
using ClassLibrary1ConsultoriosApi.Persistence.UnitsOfWork;
using ConsultoriosApi.Application.Contracts.Persistence;
using ConsultoriosApi.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1ConsultoriosApi.Persistence
{
    public static class PersistenceServiceRegistry
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services)
        {
            services.AddDbContext<ConsultoriosApiDbContext>(options =>
                options.UseSqlServer("name=ConsultoriosApiConnectionString"));

            services.AddScoped<IUnitOfWork, EFUnitOfWork>();
            services.AddScoped<IOfficesRepository, OfficesRepository>();

            return services;
        } 
    }
}
