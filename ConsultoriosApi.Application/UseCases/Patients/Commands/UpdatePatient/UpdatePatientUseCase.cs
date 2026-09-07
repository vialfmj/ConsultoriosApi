using ConsultoriosApi.Application.Contracts.Persistence;
using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Exceptions;
using ConsultoriosApi.Application.Utils.Mediator;
using ConsultoriosApi.Dominio.ValueObjects;
using System;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Patients.Commands.UpdatePatient
{
    public class UpdatePatientUseCase : IRequestHandler<UpdatePatientCommand, Guid>
    {
        private readonly IPatientsRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public UpdatePatientUseCase(IPatientsRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(UpdatePatientCommand command)
        {
            var patient = await repository.GetById(command.Id);
            if (patient is null)
            {
                throw new NotFoundException();
            }

            patient.UpdateName(command.Name);
            patient.UpdateEmail(new Email(command.Email));
            try
            {
                await repository.Update(patient);
                await unitOfWork.Commit();
                return patient.Id;
            }
            catch (Exception)
            {
                await unitOfWork.RollBack();
                throw;
            }
        }
    }
}
