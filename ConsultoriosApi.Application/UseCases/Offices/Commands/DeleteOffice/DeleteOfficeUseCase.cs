using ConsultoriosApi.Application.Contracts.Persistence;
using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Exceptions;
using ConsultoriosApi.Application.Utils.Mediator;
using System;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Offices.Commands.DeleteOffice
{
    public class DeleteOfficeUseCase : IRequestHandler<DeleteOfficeCommand, Guid>
    {
        private readonly IOfficesRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public DeleteOfficeUseCase(IOfficesRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(DeleteOfficeCommand command)
        {
            var office = await repository.GetById(command.Id);
            if (office is null)
            {
                throw new NotFoundException();
            }

            try
            {
                await repository.Delete(office);
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
