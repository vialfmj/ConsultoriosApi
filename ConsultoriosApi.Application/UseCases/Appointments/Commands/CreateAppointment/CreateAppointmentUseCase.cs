using ConsultoriosApi.Application.Contracts.Persistence;
using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Utils.Mediator;
using ConsultoriosApi.Dominio.Entities;
using ConsultoriosApi.Dominio.Exceptions;
using ConsultoriosApi.Dominio.ValueObjects;
using System;
using System.Threading.Tasks;

namespace ConsultoriosApi.Application.UseCases.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentUseCase : IRequestHandler<CreateAppointmentCommand, Guid>
    {
        private readonly IAppointmentsRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public CreateAppointmentUseCase(IAppointmentsRepository repository, IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Guid> Handle(CreateAppointmentCommand command)
        {
            var timeInterval = new TimeInterval(command.Start, command.End);
            var hasOverlap = await repository.HasOverlap(command.DentistId, command.OfficeId, timeInterval);
            if (hasOverlap)
            {
                throw new BusinessRuleException("El dentista o el consultorio ya tienen un turno programado que se superpone con el horario indicado.");
            }

            var appointment = new Appointment(command.PatientId, command.DentistId, command.OfficeId, timeInterval);
            try
            {
                var result = await repository.Add(appointment);
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
