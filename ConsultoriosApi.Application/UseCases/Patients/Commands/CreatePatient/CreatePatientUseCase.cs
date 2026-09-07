using ConsultoriosApi.Application.Contracts.Persistence;
using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Utils.Mediator;
using ConsultoriosApi.Dominio.Entities;
using ConsultoriosApi.Dominio.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Patients.Commands.CreatePatient
{
    public class CreatePatientUseCase : IRequestHandler<CreatePatientCommand, Guid>
    {
        private readonly IPatientsRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public CreatePatientUseCase(IPatientsRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CreatePatientCommand command)
        {
            var patient = new Patient(command.Name, new Email(command.Email));
            try
            {
                var result = await repository.Add(patient);
                await unitOfWork.Commit();
                return result.Id;

            }
            catch (Exception)
            {
                await unitOfWork.RollBack();
                throw;
            }
        }
    }
}
