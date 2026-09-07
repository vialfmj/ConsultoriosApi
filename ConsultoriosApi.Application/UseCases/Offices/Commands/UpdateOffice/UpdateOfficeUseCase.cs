using ConsultoriosApi.Application.Contracts.Persistence;
using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Exceptions;
using ConsultoriosApi.Application.Utils.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Offices.Commands.UpdateOffice
{
    public class UpdateOfficeUseCase : IRequestHandler<UpdateOfficeCommand, Guid>
    {
        private readonly IOfficesRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public UpdateOfficeUseCase(IOfficesRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(UpdateOfficeCommand command)
        {
            var office = await repository.GetById(command.Id);
            if (office is null)
            {
                throw new NotFoundException();
            }

            office.UpdateName(command.Name);
            try
            {
                await repository.Update(office);
                await unitOfWork.Commit();
                return office.Id;
            }
            catch (Exception)
            {
                await unitOfWork.RollBack();
                throw;
            }
        }
    }
}
