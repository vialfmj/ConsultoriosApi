using ConsultoriosApi.Application.Contracts.Persistence;
using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Exceptions;
using ConsultoriosApi.Application.Utils.Mediator;
using System;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Appointments.Commands.CompleteAppointment
{
    public class CompleteAppointmentUseCase : IRequestHandler<CompleteAppointmentCommand, Guid>
    {
        private readonly IAppointmentsRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public CompleteAppointmentUseCase(IAppointmentsRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CompleteAppointmentCommand command)
        {
            var appointment = await repository.GetById(command.Id);
            if (appointment is null)
            {
                throw new NotFoundException();
            }

            appointment.Complete();
            try
            {
                await repository.Update(appointment);
                await unitOfWork.Commit();
                return appointment.Id;
            }
            catch (Exception)
            {
                await unitOfWork.RollBack();
                throw;
            }
        }
    }
}
