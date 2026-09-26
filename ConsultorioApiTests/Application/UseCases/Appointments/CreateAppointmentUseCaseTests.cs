using ConsultoriosApi.Application.Contracts.Persistence;
using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.UseCases.Appointments.Commands.CreateAppointment;
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
    public class CreateAppointmentUseCaseTests
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
        private IAppointmentsRepository repository;
        private IUnitOfWork unitOfWork;
        private CreateAppointmentUseCase useCase;
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.

        [TestInitialize]
        public void Setup()
        {
            repository = Substitute.For<IAppointmentsRepository>();
            unitOfWork = Substitute.For<IUnitOfWork>();
            useCase = new CreateAppointmentUseCase(repository, unitOfWork);
        }

        [TestMethod]
        public async Task Handle_ReturnsId_WhenNoHaySolapamiento()
        {
            // Arrange
            var command = new CreateAppointmentCommand
            {
                PatientId = Guid.NewGuid(),
                DentistId = Guid.NewGuid(),
                OfficeId = Guid.NewGuid(),
                Start = DateTime.UtcNow.AddDays(1),
                End = DateTime.UtcNow.AddDays(1).AddHours(1)
            };
            repository.HasOverlap(command.DentistId, command.OfficeId, Arg.Any<TimeInterval>()).Returns(false);
            repository.Add(Arg.Any<Appointment>()).Returns(callInfo => callInfo.Arg<Appointment>());

            // Act
            var result = await useCase.Handle(command);

            // Assert
            await repository.Received(1).Add(Arg.Any<Appointment>());
            await unitOfWork.Received(1).Commit();
            Assert.AreNotEqual(Guid.Empty, result);
        }

        [TestMethod]
        public async Task Handle_ThrowsBusinessRuleException_WhenHaySolapamiento()
        {
            // Arrange
            var command = new CreateAppointmentCommand
            {
                PatientId = Guid.NewGuid(),
                DentistId = Guid.NewGuid(),
                OfficeId = Guid.NewGuid(),
                Start = DateTime.UtcNow.AddDays(1),
                End = DateTime.UtcNow.AddDays(1).AddHours(1)
            };
            repository.HasOverlap(command.DentistId, command.OfficeId, Arg.Any<TimeInterval>()).Returns(true);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<BusinessRuleException>(async () => await useCase.Handle(command));

            await repository.DidNotReceive().Add(Arg.Any<Appointment>());
            await unitOfWork.DidNotReceive().Commit();
        }

        [TestMethod]
        public async Task Handle_ShouldRollback_WhenAnErrorOccurs()
        {
            // Arrange
            var command = new CreateAppointmentCommand
            {
                PatientId = Guid.NewGuid(),
                DentistId = Guid.NewGuid(),
                OfficeId = Guid.NewGuid(),
                Start = DateTime.UtcNow.AddDays(1),
                End = DateTime.UtcNow.AddDays(1).AddHours(1)
            };
            repository.HasOverlap(command.DentistId, command.OfficeId, Arg.Any<TimeInterval>()).Returns(false);
            repository.Add(Arg.Any<Appointment>()).Throws<Exception>();

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(async () => await useCase.Handle(command));

            await unitOfWork.Received(1).RollBack();
        }
    }
}
