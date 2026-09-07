using ConsultoriosApi.Application.Contracts.Persistence;
using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Exceptions;
using ConsultoriosApi.Application.UseCases.Patients.Commands.DeletePatient;
using ConsultoriosApi.Dominio.Entities;
using ConsultoriosApi.Dominio.ValueObjects;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Threading.Tasks;

namespace ConsultorioApiTests.Application.UseCases.Patients
{
    [TestClass]
    public class DeletePatientUseCaseTests
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
        private IPatientsRepository repository;
        private IUnitOfWork unitOfWork;
        private DeletePatientUseCase useCase;
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.

        [TestInitialize]
        public void Setup()
        {
            repository = Substitute.For<IPatientsRepository>();
            unitOfWork = Substitute.For<IUnitOfWork>();
            useCase = new DeletePatientUseCase(repository, unitOfWork);
        }

        [TestMethod]
        public async Task Handle_ReturnsId_WhenCommandIsValid()
        {
            //Arrange
            var patient = new Patient("Paciente A", new Email("paciente@a.com"));
            var command = new DeletePatientCommand { Id = patient.Id };
            repository.GetById(patient.Id).Returns(patient);

            //Act
            var result = await useCase.Handle(command);

            //Assert
            await repository.Received(1).Delete(patient);
            await unitOfWork.Received(1).Commit();
            Assert.AreEqual(patient.Id, result);
        }

        [TestMethod]
        public async Task Handle_ShouldRollback_WhenAnErrorOccurs()
        {
            // Arrange
            var patient = new Patient("Paciente A", new Email("paciente@a.com"));
            var command = new DeletePatientCommand { Id = patient.Id };
            repository.GetById(patient.Id).Returns(patient);
            repository.Delete(Arg.Any<Patient>()).Throws<Exception>();

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(async () => await useCase.Handle(command));

            await unitOfWork.Received(1).RollBack();
        }

        [TestMethod]
        public async Task Handle_ThrowsNotFoundException_WhenPatientDoesNotExist()
        {
            // Arrange
            var command = new DeletePatientCommand { Id = Guid.NewGuid() };
            repository.GetById(command.Id).Returns((Patient?)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<NotFoundException>(async () => await useCase.Handle(command));
        }
    }
}
