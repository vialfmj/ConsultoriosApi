using ConsultoriosApi.Application.Contracts.Persistence;
using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.UseCases.Dentists.Commands.CreateDentist;
using ConsultoriosApi.Dominio.Entities;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Threading.Tasks;

namespace ConsultorioApiTests.Application.UseCases.Dentists
{
    [TestClass]
    public class CreateDentistUseCaseTests
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
        private IDentistsRepository repository;
        private IUnitOfWork unitOfWork;
        private CreateDentistUseCase useCase;
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.

        [TestInitialize]
        public void Setup()
        {
            repository = Substitute.For<IDentistsRepository>();
            unitOfWork = Substitute.For<IUnitOfWork>();
            useCase = new CreateDentistUseCase(repository, unitOfWork);
        }

        [TestMethod]
        public async Task Handle_ReturnsId_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreateDentistCommand { Name = "Dentista A", Email = "dentista@a.com" };
            repository.Add(Arg.Any<Dentist>()).Returns(callInfo => callInfo.Arg<Dentist>());

            // Act
            var result = await useCase.Handle(command);

            // Assert
            await repository.Received(1).Add(Arg.Any<Dentist>());
            await unitOfWork.Received(1).Commit();
            Assert.AreNotEqual(Guid.Empty, result);
        }

        [TestMethod]
        public async Task Handle_ShouldRollback_WhenAnErrorOccurs()
        {
            // Arrange
            var command = new CreateDentistCommand { Name = "Dentista A", Email = "dentista@a.com" };
            repository.Add(Arg.Any<Dentist>()).Throws<Exception>();

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(async () => await useCase.Handle(command));

            await unitOfWork.Received(1).RollBack();
        }
    }
}
