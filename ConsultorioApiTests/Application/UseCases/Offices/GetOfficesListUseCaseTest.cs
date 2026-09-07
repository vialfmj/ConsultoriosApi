using ConsultoriosApi.Application.Contracts.Repositories;
using ConsultoriosApi.Application.UseCases.Offices.Queries.GetOfficesList;
using ConsultoriosApi.Dominio.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsultorioApiTests.Application.UseCases.Offices
{
    [TestClass]
    public class GetOfficesListUseCaseTest
    {
#pragma warning disable CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
        private IOfficesRepository repository;
        private GetOfficesListUseCase useCase;
#pragma warning restore CS8618 // Un campo que no acepta valores NULL debe contener un valor distinto de NULL al salir del constructor. Considere la posibilidad de agregar el modificador "required" o declararlo como un valor que acepta valores NULL.
        [TestInitialize]
        public void Setup()
        {
             repository = Substitute.For<IOfficesRepository>();
             useCase = new GetOfficesListUseCase(repository);
        }
        [TestMethod]
        public async Task IfThereAreOffices_ReturnsOfficesListDto()
        {
            var offices = new List<Office>
            {
                new Office("Office A"),
                new Office("Office B")

            };

            repository.GetAll().Returns(offices);

            var expected = offices.Select(office => new OfficesListDTO { Id = office.Id, Name = office.Name }).ToList();


            var result = await useCase.Handle(new GetOfficesListQuery());
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, offices.Count);

            for(int i = 0; i< offices.Count; i++)
            {
                Assert.AreEqual(result[i].Id, offices[i].Id);
                Assert.AreEqual(result[i].Name, offices[i].Name);
            }
        }
    }
}
