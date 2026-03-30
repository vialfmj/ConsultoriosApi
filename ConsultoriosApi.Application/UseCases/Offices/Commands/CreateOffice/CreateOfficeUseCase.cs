using ConsultoriosApi.Application.Contracts.Persistence;
using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Utils.Mediator;
using ConsultoriosApi.Dominio.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Offices.Commands.CreateOffice
{
    public class CreateOfficeUseCase : IRequestHandler<CreateOfficeCommand, Guid>
    {
        private readonly IOfficeRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public CreateOfficeUseCase(IOfficeRepository repository, IUnitOfWork unitOfWork) 
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CreateOfficeCommand command)
        {
            var office = new Office(command.Name);
            try
            {
                var result = await repository.Add(office);
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
