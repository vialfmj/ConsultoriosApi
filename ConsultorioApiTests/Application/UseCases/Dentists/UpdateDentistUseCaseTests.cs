using ConsultoriosApi.Application.Contracts.Persistence;
using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Exceptions;
using ConsultoriosApi.Application.UseCases.Dentists.Commands.UpdateDentist;
using ConsultoriosApi.Dominio.Entities;
using ConsultoriosApi.Dominio.ValueObjects;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Threading.Tasks;

namespace ConsultorioApiTests.Application.UseCases.Dentists
{
    [TestClass]
    public class UpdateDentistUseCaseTests
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
        private IDentistsRepository repository;
        private IUnitOfWork unitOfWork;
        private UpdateDentistUseCase useCase;
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.

        [TestInitialize]
        public void Setup()
        {
            repository = Substitute.For<IDentistsRepository>();
            unitOfWork = Substitute.For<IUnitOfWork>();
            useCase = new UpdateDentistUseCase(repository, unitOfWork);
        }

        [TestMethod]
        public async Task Handle_ReturnsId_WhenCommandIsValid()
        {
            //Arrange
            var dentist = new Dentist("Dentista A", new Email("dentista@a.com"));
            var command = new UpdateDentistCommand { Id = dentist.Id, Name = "Dentista A - actualizado", Email = "actualizado@a.com" };
            repository.GetById(dentist.Id).Returns(dentist);

            //Act
            var result = await useCase.Handle(command);

            //Assert
            await repository.Received(1).Update(dentist);
            await unitOfWork.Received(1).Commit();
            Assert.AreEqual(dentist.Id, result);
            Assert.AreEqual("Dentista A - actualizado", dentist.Name);
            Assert.AreEqual("actualizado@a.com", dentist.Email.Valor);
        }

        [TestMethod]
        public async Task Handle_ShouldRollback_WhenAnErrorOccurs()
        {
            // Arrange
            var dentist = new Dentist("Dentista A", new Email("dentista@a.com"));
            var command = new UpdateDentistCommand { Id = dentist.Id, Name = "Dentista A - actualizado", Email = "actualizado@a.com" };
            repository.GetById(dentist.Id).Returns(dentist);
            repository.Update(Arg.Any<Dentist>()).Throws<Exception>();

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(async () => await useCase.Handle(command));

            await unitOfWork.Received(1).RollBack();
        }

        [TestMethod]
        public async Task Handle_ThrowsNotFoundException_WhenDentistDoesNotExist()
        {
            // Arrange
            var command = new UpdateDentistCommand { Id = Guid.NewGuid(), Name = "Dentista A", Email = "dentista@a.com" };
            repository.GetById(command.Id).Returns((Dentist?)null);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<NotFoundException>(async () => await useCase.Handle(command));
        }
    }
}
