using ConsultoriosApi.Application.Contracts.Persistence;
using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Exceptions;
using ConsultoriosApi.Application.Utils.Mediator;
using System;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Dentists.Commands.DeleteDentist
{
    public class DeleteDentistUseCase : IRequestHandler<DeleteDentistCommand, Guid>
    {
        private readonly IDentistsRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public DeleteDentistUseCase(IDentistsRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(DeleteDentistCommand command)
        {
            var dentist = await repository.GetById(command.Id);
            if (dentist is null)
            {
                throw new NotFoundException();
            }

            try
            {
                await repository.Delete(dentist);
                await unitOfWork.Commit();
                return dentist.Id;
            }
            catch (Exception)
            {
                await unitOfWork.RollBack();
                throw;
            }
        }
    }
}
