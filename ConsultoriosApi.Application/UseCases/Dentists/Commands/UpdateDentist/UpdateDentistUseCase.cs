using ConsultoriosApi.Application.Contracts.Persistence;
using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Exceptions;
using ConsultoriosApi.Application.Utils.Mediator;
using ConsultoriosApi.Dominio.ValueObjects;
using System;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Dentists.Commands.UpdateDentist
{
    public class UpdateDentistUseCase : IRequestHandler<UpdateDentistCommand, Guid>
    {
        private readonly IDentistsRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public UpdateDentistUseCase(IDentistsRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(UpdateDentistCommand command)
        {
            var dentist = await repository.GetById(command.Id);
            if (dentist is null)
            {
                throw new NotFoundException();
            }

            dentist.UpdateName(command.Name);
            dentist.UpdateEmail(new Email(command.Email));
            try
            {
                await repository.Update(dentist);
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
