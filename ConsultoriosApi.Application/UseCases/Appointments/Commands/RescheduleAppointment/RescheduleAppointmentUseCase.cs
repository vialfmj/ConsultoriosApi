using ConsultoriosApi.Application.Contracts.Persistence;
using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Exceptions;
using ConsultoriosApi.Application.Utils.Mediator;
using ConsultoriosApi.Dominio.Exceptions;
using ConsultoriosApi.Dominio.ValueObjects;
using System;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Appointments.Commands.RescheduleAppointment
{
    public class RescheduleAppointmentUseCase : IRequestHandler<RescheduleAppointmentCommand, Guid>
    {
        private readonly IAppointmentsRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public RescheduleAppointmentUseCase(IAppointmentsRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(RescheduleAppointmentCommand command)
        {
            var appointment = await repository.GetById(command.Id);
            if (appointment is null)
            {
                throw new NotFoundException();
            }

            var timeInterval = new TimeInterval(command.Start, command.End);
            var hasOverlap = await repository.HasOverlap(appointment.DentistId, appointment.OfficeId, timeInterval, appointment.Id);
            if (hasOverlap)
            {
                throw new BusinessRuleException("El dentista o el consultorio ya tienen un turno programado que se superpone con el nuevo horario.");
            }

            appointment.Reschedule(timeInterval);
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
