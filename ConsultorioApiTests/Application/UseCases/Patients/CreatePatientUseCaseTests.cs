using ConsultoriosApi.Application.Contracts.Persistence;
using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.UseCases.Patients.Commands.CreatePatient;
using ConsultoriosApi.Dominio.Entities;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Threading.Tasks;

namespace ConsultorioApiTests.Application.UseCases.Patients
{
    [TestClass]
    public class CreatePatientUseCaseTests
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
        private IPatientsRepository repository;
        private IUnitOfWork unitOfWork;
        private CreatePatientUseCase useCase;
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.

        [TestInitialize]
        public void Setup()
        {
            repository = Substitute.For<IPatientsRepository>();
            unitOfWork = Substitute.For<IUnitOfWork>();
            useCase = new CreatePatientUseCase(repository, unitOfWork);
        }

        [TestMethod]
        public async Task Handle_ReturnsId_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreatePatientCommand { Name = "Paciente A", Email = "paciente@a.com" };
            repository.Add(Arg.Any<Patient>()).Returns(callInfo => callInfo.Arg<Patient>());

            // Act
            var result = await useCase.Handle(command);

            // Assert
            await repository.Received(1).Add(Arg.Any<Patient>());
            await unitOfWork.Received(1).Commit();
            Assert.AreNotEqual(Guid.Empty, result);
        }

        [TestMethod]
        public async Task Handle_ShouldRollback_WhenAnErrorOccurs()
        {
            // Arrange
            var command = new CreatePatientCommand { Name = "Paciente A", Email = "paciente@a.com" };
            repository.Add(Arg.Any<Patient>()).Throws<Exception>();

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(async () => await useCase.Handle(command));

            await unitOfWork.Received(1).RollBack();
        }
    }
}
