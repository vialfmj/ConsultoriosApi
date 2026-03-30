using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.Exceptions;
using ConsultoriosApi.Application.UseCases.Offices.Queries.GetOfficeDetail;
using ConsultoriosApi.Dominio.Entities;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultorioApiTests.Application.UseCases.Offices
{
    [TestClass]
    public class GetOfficeDetailUseCaseTests
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
        private IOfficesRepository repository;
        private GetOfficeDetailUseCase useCase;
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.

        [TestInitialize]
        public void Setup()
        {
            repository = Substitute.For<IOfficesRepository>();
            useCase = new GetOfficeDetailUseCase(repository);
        }
        [TestMethod]
        public async Task Handle_ReturnsDTO_WhenIsValid()
        {
            var office = new Office("Test");
            var id = office.Id;
            var query = new GetOfficeDetailQuery()
            {
                Id = id
            };
            repository.GetById(id).Returns(office);

            var result = await useCase.Handle(query);

            Assert.IsNotNull(result);
            Assert.AreEqual(office.Id, result.Id);
            Assert.AreEqual(office.Name, result.Name);


        }
        [TestMethod]
        public void Handle_ThrowsNotFoundException_WhenOfficeNotFound()
        {
            Guid id = Guid.NewGuid();
            var query = new GetOfficeDetailQuery()
            {
                Id = id
            };

            repository.GetById(id).ReturnsNull();

            Assert.ThrowsExceptionAsync<NotFoundException>(async () => await useCase.Handle(query));
        }
    }
}
