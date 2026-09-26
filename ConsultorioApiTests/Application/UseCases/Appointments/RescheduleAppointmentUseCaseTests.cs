using ConsultoriosApi.Application.Contracts.Persistence;
using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Exceptions;
using ConsultoriosApi.Application.UseCases.Appointments.Commands.RescheduleAppointment;
using ConsultoriosApi.Dominio.Entities;
using ConsultoriosApi.Dominio.Exceptions;
using ConsultoriosApi.Dominio.ValueObjects;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Threading.Tasks;

namespace ConsultorioApiTests.Application.UseCases.Appointments
{
    [TestClass]
    public class RescheduleAppointmentUseCaseTests
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
        private IAppointmentsRepository repository;
        private IUnitOfWork unitOfWork;
        private RescheduleAppointmentUseCase useCase;
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.

        [TestInitialize]
        public void Setup()
        {
            repository = Substitute.For<IAppointmentsRepository>();
            unitOfWork = Substitute.For<IUnitOfWork>();
            useCase = new RescheduleAppointmentUseCase(repository, unitOfWork);
        }

        private static Appointment CrearTurno()
        {
            return new Appointment(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
                new TimeInterval(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddDays(1).AddHours(1)));
        }

        [TestMethod]
        public async Task Handle_ReturnsId_WhenNoHaySolapamiento()
        {
            // Arrange
            var appointment = CrearTurno();
            var command = new RescheduleAppointmentCommand
            {
                Id = appointment.Id,
                Start = DateTime.UtcNow.AddDays(2),
                End = DateTime.UtcNow.AddDays(2).AddHours(1)
            };
            repository.GetById(appointment.Id).Returns(appointment);
            repository.HasOverlap(appointment.DentistId, appointment.OfficeId, Arg.Any<TimeInterval>(), appointment.Id).Returns(false);

            // Act
            var result = await useCase.Handle(command);

            // Assert
            await repository.Received(1).HasOverlap(appointment.DentistId, appointment.OfficeId, Arg.Any<TimeInterval>(), appointment.Id);
            await repository.Received(1).Update(appointment);
            await unitOfWork.Received(1).Commit();
            Assert.AreEqual(appointment.Id, result);
            Assert.AreEqual(command.Start, appointment.TimeInterval.Start);
            Assert.AreEqual(command.End, appointment.TimeInterval.End);
        }

        [TestMethod]
        public async Task Handle_ThrowsNotFoundException_WhenTurnoNoExiste()
        {
            // Arrange
            var command = new RescheduleAppointmentCommand
            {
                Id = Guid.NewGuid(),
                Start = DateTime.UtcNow.AddDays(2),
                End = DateTime.UtcNow.AddDays(2).AddHours(1)
            };
            repository.GetById(command.Id).Returns((Appointment?)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<NotFoundException>(async () => await useCase.Handle(command));
        }

        [TestMethod]
        public async Task Handle_ThrowsBusinessRuleException_WhenHaySolapamientoConOtroTurno()
        {
            // Arrange
            var appointment = CrearTurno();
            var command = new RescheduleAppointmentCommand
            {
                Id = appointment.Id,
                Start = DateTime.UtcNow.AddDays(2),
                End = DateTime.UtcNow.AddDays(2).AddHours(1)
            };
            repository.GetById(appointment.Id).Returns(appointment);
            repository.HasOverlap(appointment.DentistId, appointment.OfficeId, Arg.Any<TimeInterval>(), appointment.Id).Returns(true);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<BusinessRuleException>(async () => await useCase.Handle(command));

            await repository.DidNotReceive().Update(Arg.Any<Appointment>());
            await unitOfWork.DidNotReceive().Commit();
        }

        [TestMethod]
        public async Task Handle_ShouldRollback_WhenAnErrorOccurs()
        {
            // Arrange
            var appointment = CrearTurno();
            var command = new RescheduleAppointmentCommand
            {
                Id = appointment.Id,
                Start = DateTime.UtcNow.AddDays(2),
                End = DateTime.UtcNow.AddDays(2).AddHours(1)
            };
            repository.GetById(appointment.Id).Returns(appointment);
            repository.HasOverlap(appointment.DentistId, appointment.OfficeId, Arg.Any<TimeInterval>(), appointment.Id).Returns(false);
            repository.Update(Arg.Any<Appointment>()).Throws<Exception>();

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(async () => await useCase.Handle(command));

            await unitOfWork.Received(1).RollBack();
        }
    }
}
