using ConsultoriosApi.Application.Contracts.Persistence;
using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Exceptions;
using ConsultoriosApi.Application.UseCases.Offices.Commands.DeleteOffice;
using ConsultoriosApi.Dominio.Entities;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Threading.Tasks;

namespace ConsultorioApiTests.Application.UseCases.Offices
{
    [TestClass]
    public class DeleteOfficeUseCaseTests
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
        private IOfficesRepository repository;
        private IUnitOfWork unitOfWork;
        private DeleteOfficeUseCase useCase;
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.

        [TestInitialize]
        public void Setup()
        {
            repository = Substitute.For<IOfficesRepository>();
            unitOfWork = Substitute.For<IUnitOfWork>();
            useCase = new DeleteOfficeUseCase(repository, unitOfWork);
        }

        [TestMethod]
        public async Task Handle_ReturnsId_WhenCommandIsValid()
        {
            //Arrange
            var office = new Office("Office A");
            var command = new DeleteOfficeCommand { Id = office.Id };
            repository.GetById(office.Id).Returns(office);

            //Act
            var result = await useCase.Handle(command);

            //Assert
            await repository.Received(1).Delete(office);
            await unitOfWork.Received(1).Commit();
            Assert.AreEqual(office.Id, result);
        }

        [TestMethod]
        public async Task Handle_ShouldRollback_WhenAnErrorOccurs()
        {
            // Arrange
            var office = new Office("Office A");
            var command = new DeleteOfficeCommand { Id = office.Id };
            repository.GetById(office.Id).Returns(office);
            repository.Delete(Arg.Any<Office>()).Throws<Exception>();

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(async () => await useCase.Handle(command));

            await unitOfWork.Received(1).RollBack();
        }

        [TestMethod]
        public async Task Handle_ThrowsNotFoundException_WhenOfficeDoesNotExist()
        {
            // Arrange
            var command = new DeleteOfficeCommand { Id = Guid.NewGuid() };
            repository.GetById(command.Id).Returns((Office?)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<NotFoundException>(async () => await useCase.Handle(command));
        }
    }
}
