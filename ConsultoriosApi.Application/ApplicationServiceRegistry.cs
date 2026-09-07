using ConsultoriosApi.Application.UseCases.Dentists.Commands.CreateDentist;
using ConsultoriosApi.Application.UseCases.Dentists.Commands.DeleteDentist;
using ConsultoriosApi.Application.UseCases.Dentists.Commands.UpdateDentist;
using ConsultoriosApi.Application.UseCases.Dentists.Queries.GetDentistDetail;
using ConsultoriosApi.Application.UseCases.Dentists.Queries.GetDentistsList;
using ConsultoriosApi.Application.UseCases.Offices.Commands.CreateOffice;
using ConsultoriosApi.Application.UseCases.Offices.Commands.DeleteOffice;
using ConsultoriosApi.Application.UseCases.Offices.Commands.UpdateOffice;
using ConsultoriosApi.Application.UseCases.Offices.Queries.GetOfficeDetail;
using ConsultoriosApi.Application.UseCases.Offices.Queries.GetOfficesList;
using ConsultoriosApi.Application.UseCases.Patients.Commands.CreatePatient;
using ConsultoriosApi.Application.UseCases.Patients.Commands.DeletePatient;
using ConsultoriosApi.Application.UseCases.Patients.Commands.UpdatePatient;
using ConsultoriosApi.Application.UseCases.Patients.Queries.GetPatientDetail;
using ConsultoriosApi.Application.UseCases.Patients.Queries.GetPatientsList;
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
            services.AddScoped<IRequestHandler<UpdateOfficeCommand, Guid>, UpdateOfficeUseCase>();
            services.AddScoped<IRequestHandler<DeleteOfficeCommand, Guid>, DeleteOfficeUseCase>();
            services.AddScoped<IRequestHandler<GetOfficeDetailQuery, OfficeDetailDTO>, GetOfficeDetailUseCase>();
            services.AddScoped<IRequestHandler<GetOfficesListQuery, List<OfficesListDTO>>, GetOfficesListUseCase>();
            services.AddScoped<IRequestHandler<CreatePatientCommand, Guid>, CreatePatientUseCase>();
            services.AddScoped<IRequestHandler<UpdatePatientCommand, Guid>, UpdatePatientUseCase>();
            services.AddScoped<IRequestHandler<DeletePatientCommand, Guid>, DeletePatientUseCase>();
            services.AddScoped<IRequestHandler<GetPatientsListQuery, PagedDTO<PatientsListDTO>>, GetPatientsListUseCase>();
            services.AddScoped<IRequestHandler<GetPatientDetailQuery, PatientDetailDTO>, GetPatientDetailUseCase>();
            services.AddScoped<IRequestHandler<CreateDentistCommand, Guid>, CreateDentistUseCase>();
            services.AddScoped<IRequestHandler<UpdateDentistCommand, Guid>, UpdateDentistUseCase>();
            services.AddScoped<IRequestHandler<DeleteDentistCommand, Guid>, DeleteDentistUseCase>();
            services.AddScoped<IRequestHandler<GetDentistsListQuery, PagedDTO<DentistsListDTO>>, GetDentistsListUseCase>();
            services.AddScoped<IRequestHandler<GetDentistDetailQuery, DentistDetailDTO>, GetDentistDetailUseCase>();
            return services;
        }

    }
}
