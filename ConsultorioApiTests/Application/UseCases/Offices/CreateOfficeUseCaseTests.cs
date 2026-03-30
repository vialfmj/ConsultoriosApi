using ConsultoriosApi.Application.Contracts.Persistence;
using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.UseCases.Offices.Commands.CreateOffice;
using ConsultoriosApi.Dominio.Entities;
using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultorioApiTests.Application.UseCases.Offices
{
    [TestClass]
    public class CreateOfficeUseCaseTests
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
        private IOfficeRepository repository;
        private IUnitOfWork unitOfWork;
        private CreateOfficeUseCase useCase;
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.


        [TestInitialize]
        public void Setup()
        {
            repository = Substitute.For<IOfficeRepository>();
            unitOfWork = Substitute.For<IUnitOfWork>();
            useCase = new CreateOfficeUseCase(repository, unitOfWork);
        }
        [TestMethod]
        public async Task Handle_ReturnsId_WhenCommandIsValid()
        {
            //Arrange
            var command = new CreateOfficeCommand { Name = "Office A" };

            var officeCreated = new Office("Office A");
            repository.Add(Arg.Any<Office>()).Returns(officeCreated);

            //Act
            var result = await useCase.Handle(command);
            await repository.Received(1).Add(Arg.Any<Office>());
            await unitOfWork.Received(1).Commit();

            Assert.AreNotEqual(Guid.Empty, result);
        }
        [TestMethod]
        public async Task Handle_ShouldRollback_WhenAnErrorOccurs()
        {
            // Arrange
            var command = new CreateOfficeCommand { Name = "Office A" };
            repository.Add(Arg.Any<Office>()).Throws<Exception>();
            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(async () => await useCase.Handle(command));

            await unitOfWork.Received(1).RollBack();
        }
    }
}
