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

namespace ConsultoriosApi.Application.UseCases.Dentists.Commands.CreateDentist
{
    public class CreateDentistUseCase : IRequestHandler<CreateDentistCommand, Guid>
    {
        private readonly IDentistsRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public CreateDentistUseCase(IDentistsRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CreateDentistCommand command)
        {
            var dentist = new Dentist(command.Name, new Email(command.Email));
            try
            {
                var result = await repository.Add(dentist);
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
