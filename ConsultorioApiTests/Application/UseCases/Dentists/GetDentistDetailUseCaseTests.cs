using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Exceptions;
using ConsultoriosApi.Application.UseCases.Dentists.Queries.GetDentistDetail;
using ConsultoriosApi.Dominio.Entities;
using ConsultoriosApi.Dominio.ValueObjects;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Threading.Tasks;

namespace ConsultorioApiTests.Application.UseCases.Dentists
{
    [TestClass]
    public class GetDentistDetailUseCaseTests
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
        private IDentistsRepository repository;
        private GetDentistDetailUseCase useCase;
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.

        [TestInitialize]
        public void Setup()
        {
            repository = Substitute.For<IDentistsRepository>();
            useCase = new GetDentistDetailUseCase(repository);
        }

        [TestMethod]
        public async Task Handle_ReturnsDTO_WhenIsValid()
        {
            var dentist = new Dentist("Dentista A", new Email("dentista@a.com"));
            var id = dentist.Id;
            var query = new GetDentistDetailQuery { Id = id };
            repository.GetById(id).Returns(dentist);

            var result = await useCase.Handle(query);

            Assert.IsNotNull(result);
            Assert.AreEqual(dentist.Id, result.Id);
            Assert.AreEqual(dentist.Name, result.Name);
            Assert.AreEqual(dentist.Email.Valor, result.Email);
        }

        [TestMethod]
        public void Handle_ThrowsNotFoundException_WhenDentistNotFound()
        {
            Guid id = Guid.NewGuid();
            var query = new GetDentistDetailQuery { Id = id };

            repository.GetById(id).ReturnsNull();

            Assert.ThrowsExceptionAsync<NotFoundException>(async () => await useCase.Handle(query));
        }
    }
}
