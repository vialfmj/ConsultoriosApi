using ConsultoriosApi.Application.UseCases.Offices.Commands.CreateOffice;
using ConsultoriosApi.Application.UseCases.Offices.Queries.GetOfficeDetail;
using ConsultoriosApi.Application.Utils.Mediator;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application
{
    public static class ApplicationServiceRegistry
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IMediator, SimpleMediator>();
            services.AddScoped<IRequestHandler<CreateOfficeCommand, Guid>, CreateOfficeUseCase>();
            services.AddScoped<IRequestHandler<GetOfficeDetailQuery, OfficeDetailDTO>, GetOfficeDetailUseCase>();
            return services;
        }

    }
}
