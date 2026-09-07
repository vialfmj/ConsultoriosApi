using ConsultoriosApi.Application.Contracts.Persistence;
using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Exceptions;
using ConsultoriosApi.Application.Utils.Mediator;
using System;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Patients.Commands.DeletePatient
{
    public class DeletePatientUseCase : IRequestHandler<DeletePatientCommand, Guid>
    {
        private readonly IPatientsRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public DeletePatientUseCase(IPatientsRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(DeletePatientCommand command)
        {
            var patient = await repository.GetById(command.Id);
            if (patient is null)
            {
                throw new NotFoundException();
            }

            try
            {
                await repository.Delete(patient);
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
